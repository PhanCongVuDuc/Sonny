# Builds and deploys the Sonny.Application add-in so Revit auto-loads the current code.
#
# A `dotnet build` of Sonny.Application already deploys to
# %AppData%\Autodesk\Revit\Addins\<year>\ via Nice3point's DeployRevitAddin target; this script
# just drives that per year and re-checks the .addin manifest afterwards. Run it for the Revit
# year you are about to open. First load of an unsigned add-in still shows Revit's security
# dialog once - choose "Always Load".
#
# Usage:
#   powershell -ExecutionPolicy Bypass -File scripts\Deploy-SonnyAddin.ps1              # 2023
#   powershell -ExecutionPolicy Bypass -File scripts\Deploy-SonnyAddin.ps1 -Years 2023,2025
param(
    [int[]] $Years = @(2023)
)

$repoRoot = Split-Path $PSScriptRoot -Parent
$project = Join-Path $repoRoot 'source\Sonny.Application\Sonny.Application.csproj'

# Deploy overwrites the DLLs Revit has loaded - refuse while any Revit is running
if (Get-Process Revit -ErrorAction SilentlyContinue) {
    Write-Error 'Revit is running. Close every Revit window first, then run this script again.'
    exit 1
}

foreach ($year in $Years) {
    $shortYear = $year.ToString().Substring(2)
    $configuration = "Debug R$shortYear"
    Write-Host "=== Building '$configuration' (build auto-deploys the add-in) ===" -ForegroundColor Cyan

    dotnet build $project -c $configuration --nologo -v m
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Build failed for $configuration - add-in for Revit $year NOT deployed."
        exit $LASTEXITCODE
    }
}

# Make sure every deployed build has its manifest, so Revit actually picks it up
& (Join-Path $PSScriptRoot 'Install-SonnyAddinManifests.ps1')

Write-Host 'Deploy finished. Open Revit and choose "Always Load" if the security dialog appears.' -ForegroundColor Green
