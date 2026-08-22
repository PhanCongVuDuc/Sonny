# The single gate for running Sonny.Application.Tests — this is the `loopCommand` CLAUDE.md declares.
# Rules it implements: sonny-flow rules/revit-loop.md (reload first, restart last; verdict contract).
#
#   scripts\loop.ps1                  dev loop: builds with RevitTestKeepOpen=true so the test DLL
#                                     carries NUnit.Open=false/Close=false -> ricaun reuses the open
#                                     Revit and reloads the freshly built (repacked) test DLL
#   scripts\loop.ps1 -Filter AutoJoin name filter (FullyQualifiedName~)
#   scripts\loop.ps1 -Final           acceptance run: default metadata -> fresh Revit, closed after
#
# Exit codes (the verdict contract — 2 is NOT a pass):
#   0  GREEN, at least one test really ran
#   1  RED, or the build failed (read output to tell which)
#   2  no test ran at all
param(
    [string]$Filter,
    [string]$Configuration = "Debug R23",
    [switch]$Final
)

$repoRoot = Split-Path $PSScriptRoot -Parent
$project = Join-Path $repoRoot 'source\Sonny.Application.Tests\Sonny.Application.Tests.csproj'
$keepOpen = -not $Final

Write-Host "[revit-loop] verdict: 0 GREEN / 1 RED-or-build / 2 no-test-ran (NOT a pass) | 3 RED on one task -> stop | never weaken a test" -ForegroundColor Cyan
if ($keepOpen) {
    Write-Host "[revit-loop] dev mode: reusing the open Revit; repacked test DLL carries product code, so code changes reload without a restart" -ForegroundColor Cyan
}

# --- 1. Build (test DLL metadata + repack driven by RevitTestKeepOpen) ---
# Dev mode also disables the Nice3point add-in deploy: the open Revit locks the Addins folder and
# the copy would fail the build. The stale add-in in Revit does not matter - the repacked test DLL
# carries the fresh product code.
$buildArgs = @('build', $project, '-c', $Configuration, '-v', 'q', '--nologo')
if ($keepOpen) { $buildArgs += @('-p:RevitTestKeepOpen=true', '-p:DeployRevitAddin=false') }
& dotnet @buildArgs
if ($LASTEXITCODE -ne 0) {
    Write-Host "[revit-loop] BUILD FAILED" -ForegroundColor Red
    exit 1
}

# --- 2. Trust dialog watcher — only a cold start can show it ---
$revitRunning = [bool](Get-Process Revit -ErrorAction SilentlyContinue)
if ($Final -or -not $revitRunning) {
    Start-Process powershell -WindowStyle Hidden -ArgumentList @(
        '-NoProfile', '-ExecutionPolicy', 'Bypass',
        '-File', (Join-Path $PSScriptRoot 'Watch-AlwaysLoad.ps1'),
        '-TimeoutSeconds', '240'
    ) | Out-Null
    Write-Host "[revit-loop] cold start expected - Always Load watcher armed" -ForegroundColor Cyan
}

# --- 3. Run ---
$testArgs = @('test', $project, '-c', $Configuration, '--no-build', '--nologo')
if ($Filter) { $testArgs += @('--filter', "FullyQualifiedName~$Filter") }
$output = & dotnet @testArgs 2>&1 | ForEach-Object { "$_" }
$output | ForEach-Object { Write-Host $_ }
$text = $output -join "`n"

# --- 4. Verdict ---
if ($text -match 'No test matches|No test is available') {
    Write-Host "[revit-loop] verdict 2: no test ran (this is NOT a pass)" -ForegroundColor Yellow
    exit 2
}
if ($text -match 'Failed!') {
    exit 1
}
if ($text -match 'Passed!' -and $text -match 'Passed:\s+(\d+)') {
    if ([int]$Matches[1] -gt 0) { exit 0 }
    Write-Host "[revit-loop] verdict 2: zero tests actually ran (this is NOT a pass)" -ForegroundColor Yellow
    exit 2
}
Write-Host "[revit-loop] verdict 2: could not find a test summary in the output" -ForegroundColor Yellow
exit 2
