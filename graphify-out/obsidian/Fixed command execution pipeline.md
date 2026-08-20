---
source_file: "docs/architecture/command-flow.md"
type: "rationale"
community: "Behaviour Documentation"
location: "docs/architecture/command-flow.md#L8-L22"
tags:
  - graphify/rationale
  - graphify/EXTRACTED
  - community/Behaviour_Documentation
---

# Fixed command execution pipeline

## Connections
- [[BaseExternalCommand (documented)]] - `references` [EXTRACTED]
- [[IRevitTaskRunner (documented)]] - `references` [EXTRACTED]
- [[IUIDocumentProvider (singleton, documented)]] - `references` [EXTRACTED]
- [[License check with ShouldCheckLicense() opt-out]] - `references` [EXTRACTED]

#graphify/rationale #graphify/EXTRACTED #community/Behaviour_Documentation