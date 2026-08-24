# PreToolUse hook: raw `dotnet test` against Sonny.Application.Tests bypasses the loop contract
# (trust-dialog watcher, verdict codes, Revit reuse), so block it and point at .sonnyflow/loop.ps1.
# Escape hatch for a deliberate direct run (CI, debugging the loop itself): put the literal token
# SONNY_DIRECT_TEST anywhere in the command.
try {
    $payload = [Console]::In.ReadToEnd() | ConvertFrom-Json
}
catch {
    exit 0
}

$command = $payload.tool_input.command
if (-not $command) { exit 0 }
if ($command -notmatch 'dotnet\s+(test|vstest)') { exit 0 }
if ($command -notmatch 'Sonny\.Application\.Tests') { exit 0 }
if ($command -match 'SONNY_DIRECT_TEST') { exit 0 }

[Console]::Error.WriteLine(@"
Sonny.Application.Tests launches a real Revit - run it through the loop gate, not raw dotnet test:
  powershell -ExecutionPolicy Bypass -File .sonnyflow\loop.ps1 [-Filter <name>] [-Final]
Read .sonnyflow/lessons/test-environment.md and sonny-flow rules/revit-loop.md first.
Deliberate direct run (CI only): include the token SONNY_DIRECT_TEST in the command.
"@)
exit 2
