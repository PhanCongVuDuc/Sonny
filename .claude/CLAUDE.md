# Knowledge graphs

Both graphify and codegraph index this repo. The routing rules, the arbitration table for when they disagree, and
the known traps all live in **one place: the "Knowledge graphs" section of `CLAUDE.md`** at the repo root. Read it
there — nothing about either graph is duplicated in this file.

`/graphify` invokes the graphify skill (`.claude/skills/graphify/SKILL.md`) for a full rebuild.

<!-- CODEGRAPH_START -->
CodeGraph guidance lives in `CLAUDE.md` → "Knowledge graphs". Short version: `codegraph_explore` takes a bag of
symbol names (not an English question) and returns verbatim line-numbered source — treat what it returns as already
read. It cannot see `.xaml` or `.md`; use graphify for those.
<!-- CODEGRAPH_END -->
