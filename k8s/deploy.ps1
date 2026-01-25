$Manifests = @(
    'k8s/base/db/db-config.yaml',
    'k8s/base/db/db-secret.yaml',
    'k8s/base/db/db.yaml',
    'k8s/base/cache/cache-config.yaml',
    'k8s/base/cache/cache.yaml',
    'k8s/base/dashboard/dashboard-config.yaml',
    'k8s/base/dashboard/dashboard.yaml',
    'k8s/base/api/api-config.yaml',
    'k8s/base/api/api-secret.yaml',
    'k8s/base/api/api.yaml'
)

$ErrorActionPreference = 'Stop'   
$ColorHeader = 'Cyan'
$ColorSuccess = 'Green'
$ColorFailure = 'Red'
$ColorPrompt = 'Magenta'
$ColorInfo = 'Yellow'

function Write-Header {
    param([string]$Message)
    Write-Host "`n$Message" -ForegroundColor $ColorHeader
}

function Try-Apply {
    param(
        [Parameter(Mandatory)][string]$Path
    )

    Write-Header "kubectl apply -f $Path"
    try {
        kubectl apply -f $Path
        Write-Host "`n$Path applied successfully" -ForegroundColor $ColorSuccess
        return $true
    } catch {
        $err = $_.Exception.Message
        Write-Host "`nFailed to apply $Path" -ForegroundColor $ColorFailure
        Write-Host $err -ForegroundColor $ColorInfo
        return $false
    }
}

function Prompt-ToFix {
    param([string]$File)
    Write-Host "`nFix the file $File, then press ENTER to retry…" -ForegroundColor $ColorPrompt
    Read-Host
}

foreach ($file in $Manifests) {
    if (-not (Test-Path $file)) {
        Write-Host "`nFile not found: $file" -ForegroundColor $ColorFailure
        exit 1
    }

    while (-not (Try-Apply -Path $file)) {
        Prompt-ToFix -File $file
    }
}

Write-Header "Deployment finished"