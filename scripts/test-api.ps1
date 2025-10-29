Param(
    [string]$BaseUrl = "http://localhost:5000",
    [switch]$Https = $false
)

$ErrorActionPreference = 'Stop'

function Write-Result {
    Param([string]$Name, [bool]$Passed, [int]$Status, [string]$Detail)
    if ($Passed) {
        Write-Host "[PASS] $Name ($Status)" -ForegroundColor Green
    } else {
        Write-Host "[FAIL] $Name ($Status)" -ForegroundColor Red
        if ($Detail) { Write-Host "       $Detail" -ForegroundColor DarkGray }
    }
}

function Invoke-Request {
    Param(
        [string]$Method,
        [string]$Url,
        [object]$Body
    )
    try {
        if ($null -ne $Body) {
            $json = ($Body | ConvertTo-Json -Depth 5)
            return Invoke-WebRequest -Method $Method -Uri $Url -ContentType 'application/json' -Body $json -ErrorAction Stop
        } else {
            return Invoke-WebRequest -Method $Method -Uri $Url -ErrorAction Stop
        }
    } catch {
        # Return the response even on non-2xx if available
        if ($_.Exception.Response) { return $_.Exception.Response }
        throw
    }
}

function Parse-Json {
    Param([string]$Content)
    try { return $Content | ConvertFrom-Json -ErrorAction Stop } catch { return $null }
}

$total = 0
$passed = 0

function Test-Endpoint {
    Param(
        [string]$Name,
        [string]$Method,
        [string]$Path,
        [object]$Body = $null,
        [int]$ExpectedStatus = 200,
        [scriptblock]$Validate = $null
    )
    $script:total++
    $url = "$BaseUrl$Path"
    $resp = Invoke-Request -Method $Method -Url $url -Body $Body
    $status = [int]$resp.StatusCode
    $content = $null
    try { $content = [System.IO.StreamReader]::new($resp.GetResponseStream()).ReadToEnd() } catch { $content = $resp.Content }
    $json = Parse-Json -Content $content
    $ok = ($status -eq $ExpectedStatus)
    if ($ok -and $Validate) { try { $ok = [bool](& $Validate $json) } catch { $ok = $false } }
    if ($ok) { $script:passed++ }
    $detail = if ($json) { $json | ConvertTo-Json -Depth 6 } else { $content }
    Write-Result -Name $Name -Passed $ok -Status $status -Detail $detail
    return @{ Status = $status; Json = $json }
}

if ($Https) { $BaseUrl = $BaseUrl -replace '^http:', 'https:' }

# Smoke
Test-Endpoint -Name 'Root' -Method 'GET' -Path '/'
Test-Endpoint -Name 'Swagger' -Method 'GET' -Path '/swagger' | Out-Null

# Lists
Test-Endpoint -Name 'Books - list' -Method 'GET' -Path '/api/v1/books' -Validate { param($j) $j -is [System.Array] -and $j.Count -ge 3 }
Test-Endpoint -Name 'Books - filter by year' -Method 'GET' -Path '/api/v1/books?publicationYear=2011' -Validate { param($j) @($j | Where-Object { $_.publicationYear -ne 2011 }).Count -eq 0 }
Test-Endpoint -Name 'Books - sort by title' -Method 'GET' -Path '/api/v1/books?sortBy=title' -Validate { param($j) $titles = @($j | ForEach-Object { $_.title }); ($titles -join '|') -eq ((@($titles | Sort-Object)) -join '|') }
Test-Endpoint -Name 'Books - paginate' -Method 'GET' -Path '/api/v1/books?page=1&pageSize=2' -Validate { param($j) $j.Count -le 2 }

# Author books
Test-Endpoint -Name 'Author books - exists' -Method 'GET' -Path '/api/v1/authors/1/books' -Validate { param($j) $j -is [System.Array] }
Test-Endpoint -Name 'Author books - not found' -Method 'GET' -Path '/api/v1/authors/999/books' -ExpectedStatus 404 -Validate { param($j) $j.title -eq 'Author not found' }

# Create book
$create = Test-Endpoint -Name 'Books - create' -Method 'POST' -Path '/api/v1/books' -ExpectedStatus 201 -Body @{ title = 'Agile Principles'; authorId = 1; publicationYear = 2009 } -Validate { param($j) $j.id -gt 0 -and $j.title -ne $null }
$newId = $create.Json.id
Test-Endpoint -Name 'Books - create invalid title' -Method 'POST' -Path '/api/v1/books' -ExpectedStatus 400 -Body @{ title = ''; authorId = 1; publicationYear = 2009 } -Validate { param($j) $j.title -eq 'Validation failed' }
Test-Endpoint -Name 'Books - create unknown author' -Method 'POST' -Path '/api/v1/books' -ExpectedStatus 400 -Body @{ title = 'X'; authorId = 999; publicationYear = 2009 } -Validate { param($j) $j.title -eq 'Author not found' }

# Update book
if ($newId -gt 0) {
    Test-Endpoint -Name 'Books - update' -Method 'PUT' -Path "/api/v1/books/$newId" -ExpectedStatus 204 -Body @{ title = 'Agile Principles (Updated)'; authorId = 1; publicationYear = 2009 } | Out-Null
}
Test-Endpoint -Name 'Books - update missing book' -Method 'PUT' -Path '/api/v1/books/99999' -ExpectedStatus 404 -Body @{ title = 'X'; authorId = 1; publicationYear = 2008 } -Validate { param($j) $j.title -eq 'Book not found' }
Test-Endpoint -Name 'Books - update invalid' -Method 'PUT' -Path "/api/v1/books/$newId" -ExpectedStatus 400 -Body @{ title = ''; authorId = 1; publicationYear = 2008 } -Validate { param($j) $j.title -eq 'Validation failed' }
Test-Endpoint -Name 'Books - update unknown author' -Method 'PUT' -Path "/api/v1/books/$newId" -ExpectedStatus 400 -Body @{ title = 'X'; authorId = 999; publicationYear = 2008 } -Validate { param($j) $j.title -eq 'Author not found' }

Write-Host "`nSummary: $passed / $total tests passed" -ForegroundColor Cyan
if ($passed -ne $total) { exit 1 } else { exit 0 }


