# Ensures Revit auto-loads the Sonny.Application add-in.
#
# Nice3point's DeployRevitAddin copies the DLLs into
#   %AppData%\Autodesk\Revit\Addins\<year>\Sonny.Application\
# but Revit only loads what a .addin manifest next to that folder points to. When the manifest
# is missing (e.g. the folder was cleaned, or deploy was skipped with -p:DeployRevitAddin=false),
# the tab silently never appears. This script writes the manifest for every Revit year that has
# a deployed build.
#
# Usage:  powershell -ExecutionPolicy Bypass -File scripts\Install-SonnyAddinManifests.ps1 [-Force]
param(
    # Overwrite manifests that already exist
    [switch] $Force
)

$addinsRoot = Join-Path $env:APPDATA 'Autodesk\Revit\Addins'

$manifest = @'
<RevitAddIns>
    <AddIn Type="Application">
        <Name>Sonny.Application</Name>
        <Assembly>Sonny.Application\Sonny.Application.dll</Assembly>
        <AddInId>C6D911F3-BD17-4DF2-89CE-279D9D4EB990</AddInId>
        <FullClassName>Sonny.Application.SonnyApp</FullClassName>
        <VendorId>Development</VendorId>
        <VendorDescription></VendorDescription>
        <VendorEmail></VendorEmail>
    </AddIn>
    <ManifestSettings>
        <UseRevitContext>False</UseRevitContext>
        <ContextName>Sonny.Application</ContextName>
    </ManifestSettings>
</RevitAddIns>
'@

$written = 0
foreach ($year in 2021..2026) {
    $deployedDll = Join-Path $addinsRoot "$year\Sonny.Application\Sonny.Application.dll"
    $manifestPath = Join-Path $addinsRoot "$year\Sonny.Application.addin"

    if (-not (Test-Path $deployedDll)) {
        Write-Host "Revit $year : no deployed build - skipped"
        continue
    }

    if ((Test-Path $manifestPath) -and -not $Force) {
        Write-Host "Revit $year : manifest already present - skipped"
        continue
    }

    $manifest | Out-File -FilePath $manifestPath -Encoding utf8
    Write-Host "Revit $year : manifest written -> $manifestPath"
    $written++
}

Write-Host "Done. $written manifest(s) written."
