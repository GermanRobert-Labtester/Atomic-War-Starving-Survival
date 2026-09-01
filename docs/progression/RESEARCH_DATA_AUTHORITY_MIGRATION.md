# Research Data Authority Migration

> **Document Status:** Authoritative Migration Reference
> **Project:** ASHFALL (Godot 4.7+ / .NET 8 / C# Core)
> **Date:** September 2026
> **Target:** Migration of inline hardcoded research knowledge nodes to `Assets/StreamingAssets/Data/research_knowledge.json`.

---

## 1. Migration Overview

Prior to Plan 26, `ResearchSystem.cs` declared 15 base nodes and 16 relic reverse-engineering nodes directly in C# inside `RegisterDefaults()`. This violated Invariant 6 ("Data authority is JSON in `Assets/StreamingAssets/Data/`").

### Changes Implemented:
1. **JSON Authority Created:** `Assets/StreamingAssets/Data/research_knowledge.json` containing 56 total nodes (40 expanded core progression nodes across 6 disciplines + 16 relic blueprint nodes) with `schema_version: 1`.
2. **Core Loader Created:** `Assets/Ashfall.Core/Research/ResearchKnowledgeCatalogLoader.cs` with `Load` and `LoadAndRegister` methods using `IFileIO` and `IJsonSerializer` ports.
3. **DAG Validation:** Integrated cycle detection and unresolved prerequisite validation in `ResearchKnowledgeCatalogLoader.ValidateDag()`.
4. **Zero-Drift Fallback:** `ResearchSystem.RegisterDefaults()` is preserved for backwards-compatible test fixtures while runtime sessions load from JSON.

---

## 2. Catalog Comparison

| Parameter | C# Hardcoded Baseline | JSON Authority (`research_knowledge.json`) |
|---|---|---|
| Core Knowledge Nodes | 15 nodes | 40 nodes |
| Relic Blueprint Nodes | 16 nodes | 16 nodes |
| Total Nodes | 31 nodes | 56 nodes |
| Disciplines / Categories | 5 (`survival`, `medical`, `engineering`, `science`, `combat`, `scavenging`) | 6 (`survival`, `medical`, `engineering`, `science`, `combat`, `scavenging`) |
| Schema Version | None (in code) | `1` |
| Serialization Format | Inline C# | Standard snake_case JSON |

---

## 3. Data Flow

```text
Assets/StreamingAssets/Data/research_knowledge.json
                         │
                         ▼
       ResearchKnowledgeCatalogLoader.Load()
                         │
                         ▼
           ResearchSystem.Register(node)
                         │
                         ▼
             ResearchHostSession / UI
```
