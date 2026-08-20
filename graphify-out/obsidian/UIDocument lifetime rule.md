---
source_file: "docs/architecture/command-flow.md"
type: "rationale"
community: "Behaviour Documentation"
location: "docs/architecture/command-flow.md#L79-L103"
tags:
  - graphify/rationale
  - graphify/EXTRACTED
  - community/Behaviour_Documentation
---

# UIDocument lifetime rule

## Connections
- [[IRevitDocument (transient, expression-bodied passthrough)]] - `references` [EXTRACTED]
- [[IUIDocumentProvider (singleton, documented)]] - `references` [EXTRACTED]

#graphify/rationale #graphify/EXTRACTED #community/Behaviour_Documentation