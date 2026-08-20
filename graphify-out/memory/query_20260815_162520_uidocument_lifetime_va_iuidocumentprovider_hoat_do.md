---
type: "query"
date: "2026-08-15T16:25:20.089722+00:00"
question: "UIDocument lifetime va IUIDocumentProvider hoat dong the nao"
contributor: "graphify"
outcome: "useful"
source_nodes: ["IUIDocumentProvider", "UIDocumentProvider", "IRevitDocument", "RevitDocument", "BaseExternalCommand"]
---

# Q: UIDocument lifetime va IUIDocumentProvider hoat dong the nao

## Answer

BaseExternalCommand.Execute goi IUIDocumentProvider.SetUIDocument moi lan command chay. UIDocumentProvider la singleton giu _uiDocument trong lock. RevitDocument la transient, toan bo property deu expression-bodied doc qua provider moi lan goi, khong cache. Singleton consumer phai doc qua IRevitDocument moi lan, khong duoc cache UIDocument/Document/ActiveView vao field.

## Outcome

- Signal: useful

## Source Nodes

- IUIDocumentProvider
- UIDocumentProvider
- IRevitDocument
- RevitDocument
- BaseExternalCommand