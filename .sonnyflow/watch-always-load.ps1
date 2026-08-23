# Watches for Revit's "unsigned add-in" security dialog and clicks "Always Load", then exits.
#
# Why this exists: Revit trusts an unsigned add-in by the HASH of its DLL, so every rebuild of the
# add-in brings the dialog back on the next cold start — and a headless test run then hangs until
# the 10-minute ricaun timeout. The dialog's buttons are CCPushButton controls that expose no UIA
# InvokePattern, and physical mouse clicks fail on a locked screen; PostMessage into the button's
# own HWND works in both cases.
#
# Exits after the FIRST successful click on purpose: keeping the UIA polling alive while tests run
# is useless noise. Start it right before a test run that follows a rebuild:
#
#   powershell -NoProfile -ExecutionPolicy Bypass -File .sonnyflow\watch-always-load.ps1 -TimeoutSeconds 180
param([int]$TimeoutSeconds = 180)

Add-Type @"
using System;
using System.Runtime.InteropServices;
public static class Win32Post
{
    [DllImport("user32.dll")]
    public static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    [DllImport("user32.dll")]
    public static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT { public int X; public int Y; }
    public const uint WM_LBUTTONDOWN = 0x0201;
    public const uint WM_LBUTTONUP = 0x0202;
    public const uint WM_MOUSEMOVE = 0x0200;
}
"@

Add-Type -AssemblyName UIAutomationClient
Add-Type -AssemblyName UIAutomationTypes

function Find-AlwaysLoadButton {
    $revit = Get-Process -Name "Revit" -ErrorAction SilentlyContinue | Select-Object -First 1
    if (-not $revit) { return $null }
    $root = [System.Windows.Automation.AutomationElement]::RootElement
    $pidCond = New-Object System.Windows.Automation.PropertyCondition(
        [System.Windows.Automation.AutomationElement]::ProcessIdProperty, $revit.Id)
    $windows = $root.FindAll([System.Windows.Automation.TreeScope]::Children, $pidCond)
    foreach ($window in $windows) {
        $btnCond = New-Object System.Windows.Automation.PropertyCondition(
            [System.Windows.Automation.AutomationElement]::NameProperty, "Always Load")
        $button = $window.FindFirst([System.Windows.Automation.TreeScope]::Subtree, $btnCond)
        if ($button) { return $button }
    }
    return $null
}

function Click-Button($button) {
    # The button exposes no UIA pattern; walk up to the nearest ancestor owning a real HWND and
    # post mouse messages at the button's client coordinates
    $walker = [System.Windows.Automation.TreeWalker]::RawViewWalker
    $node = $button
    $hostHwnd = [IntPtr]::Zero
    while ($node) {
        if ($node.Current.NativeWindowHandle -ne 0) {
            $hostHwnd = [IntPtr]$node.Current.NativeWindowHandle
            break
        }
        $node = $walker.GetParent($node)
    }
    if ($hostHwnd -eq [IntPtr]::Zero) { return $false }

    $rect = $button.Current.BoundingRectangle
    $point = New-Object Win32Post+POINT
    $point.X = [int]($rect.X + $rect.Width / 2)
    $point.Y = [int]($rect.Y + $rect.Height / 2)
    [Win32Post]::ScreenToClient($hostHwnd, [ref]$point) | Out-Null
    $lParam = [IntPtr](($point.X -band 0xFFFF) -bor (($point.Y -band 0xFFFF) -shl 16))
    [Win32Post]::PostMessage($hostHwnd, [Win32Post]::WM_MOUSEMOVE, [IntPtr]0, $lParam) | Out-Null
    Start-Sleep -Milliseconds 100
    [Win32Post]::PostMessage($hostHwnd, [Win32Post]::WM_LBUTTONDOWN, [IntPtr]1, $lParam) | Out-Null
    Start-Sleep -Milliseconds 120
    [Win32Post]::PostMessage($hostHwnd, [Win32Post]::WM_LBUTTONUP, [IntPtr]0, $lParam) | Out-Null
    return $true
}

$deadline = (Get-Date).AddSeconds($TimeoutSeconds)
while ((Get-Date) -lt $deadline) {
    $button = Find-AlwaysLoadButton
    if ($button) {
        if (Click-Button $button) {
            Start-Sleep -Seconds 3
            if (-not (Find-AlwaysLoadButton)) {
                Write-Output ("[{0}] clicked Always Load - exiting so UIA polling cannot disturb the test run" -f (Get-Date -Format HH:mm:ss))
                exit 0
            }
            Write-Output ("[{0}] click posted but dialog still present, retrying" -f (Get-Date -Format HH:mm:ss))
        }
    }
    Start-Sleep -Seconds 4
}
Write-Output "watcher timed out without seeing the dialog (already trusted, or Revit never started)"
exit 0
