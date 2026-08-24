---
description: loopx goal-mode setup (NOT Claude Code's built-in /goal). `/loopx <task>` sets up a goal + writes .claude/loop.md; drive the loop with native /loop. bare /loopx = arm; off | status.
argument-hint: <task to do>  |  (no args = arm)  |  off  |  status
allowed-tools: Bash(C:\Users\ADMIN\AppData\Local\Microsoft\WindowsApps\python3.EXE:*)
---

Run the loopx setup helper and read its output:

!`C:\Users\ADMIN\AppData\Local\Microsoft\WindowsApps\python3.EXE "C:/Users/ADMIN/Desktop/Workspace/Agent/loopx/loopx/claude_goal_mode/scripts/goalmode_cmd.py" $ARGUMENTS`

The output is loopx control-plane SETUP / status info — it is the COMPLETE, user-facing result. Show it to the user VERBATIM and STOP. Do NOT do any work, claim todos, write files, or loop here: the WORK runs when the user types Claude Code's native `/loop` (which executes `.claude/loop.md`). Do not summarize a multi-line `status` detail block into one line.

NOTE: this is `/loopx` (loopx control-plane SETUP), NOT a runtime and NOT Claude Code's built-in `/goal`. The run loop is native `/loop`; loopx provides the deterministic `should_run` protocol.
