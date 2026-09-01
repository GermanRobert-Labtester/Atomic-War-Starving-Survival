# Plan 34 Baseline — Research Tree (reconnaissance 2026-09-01)

Plan 34 was authored against the assumption that 15 nodes were hardcoded and no catalog existed.
Reconnaissance found a materially different repository truth (details in
[PLAN26A_PLAN34_RECONCILIATION.md](PLAN26A_PLAN34_RECONCILIATION.md)). This file records the measured
baseline; the reconciliation file derives the remaining work.

## What exists at baseline

| Artifact | State |
|---|---|
| `Assets/StreamingAssets/Data/research_knowledge.json` | Exists: `schema_version: 1`, `collection_id: research_knowledge`, **56 nodes**, snake_case fields, `breakthrough_item` unlock refs |
| `Assets/Ashfall.Core/Research/ResearchKnowledgeCatalogLoader.cs` | Exists: `Load` (schema-versioned DTO), `LoadAndRegister`, `ValidateDag` (duplicate IDs, missing prerequisites, DFS cycle detection) |
| `Assets/Ashfall.Core/Research/ResearchSystem.cs` | Runtime + `RegisterDefaults()` hardcoding **31 nodes** (15 original + 16 blueprint) — dual authority |
| `Ashfall.Core.Tests/Progression/ResearchKnowledgeCatalogLoaderTests.cs` | 6 tests pinning ≥56-node load, field sanity, DAG success/cycle/missing-prereq, LoadAndRegister |
| `Ashfall.Core.Tests/ResearchSystemTests.cs` | 8 tests pinned to the 31-node hardcoded catalog |
| Save representation | `ResearchState` is **ID-based** (`unlockedIds`/`completedIds`/`activeResearchId`) — no index/order hazard (Plan 34 §34D.1 best case) |

## Catalog shape (56 nodes)

- Categories: survival 11, engineering 13, medical 9, science 8, combat 8, scavenging 7 — six substantive categories (Plan 34 §34B.2 satisfied by a different taxonomy; repository truth wins per §1.11).
- Graph: 25 roots, 40 leaves, max depth 2, 0 orphan prerequisites, DAG-valid.
- Unlock targets: 32 `breakthrough_item` refs, all resolve to authored item IDs.

## Baseline verification results (before any Plan 34 edit)

| Gate | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests/…` | PASS — 0 errors |
| `dotnet test Ashfall.Core.Tests/…` | 5846 pass / **23 fail** — all pre-existing, none research-related (20× `DescriptiveTextsTests.TradeTexts_*`, 2× save-registry gates, `DataRuleComplianceTests`, `MoralChoiceCatalogTests`) — concurrent in-flight streams; recorded, not misattributed |
| `dotnet build Ashfall.csproj` | PASS — 0 errors |
| `godot --headless -- --data-integrity-selftest` | PASS — 0 findings, 162 catalogs, 7281 authored ids |

## Defects found at baseline (the actual Plan 34 work)

1. **D — Dual authority / dead JSON in the host.** `ResearchHostSession.LoadCatalog` has **no production caller**; the live game's research catalog is the 31 hardcoded `RegisterDefaults()` nodes (injected via `CraftingHostSession`'s ctor side effect). The 56-node JSON catalog is loaded by nothing in the runtime.
2. **D — Two research instances in the host.** `_sharedResearch` (Main; used by crafting/workshop, autopsy, library study, research panel) and `ResearchHostSession.Engine` (atlas panel) are separate `ResearchSystem`s.
3. **C — Silent fallback (§1.10 violation).** `LoadAndRegister` falls back to `RegisterDefaults()` when the data file is missing.
4. **C — Runtime def fabrication.** `WorkshopReverseEngineeringSystem.cs:353` registers an invented `ResearchKnowledgeDef` when a relic's `research_unlock_id` is absent from the catalog — production definitions outside the catalog (§1.1).
5. **C — Research progress is never persisted.** No `CaptureSection("research")`, no restore path, no save store. Completed research is lost on save/load (§34D DoD "progress round-trips" fails outright).
6. **H — `CaptureState` drops unknown IDs.** It rebuilds `unlockedIds`/`completedIds` from the catalog only, so IDs not in the catalog (legacy phantom IDs, manual/autopsy unlocks outside the catalog) are silently discarded on save (§34D.5).
7. **H — Breakthrough items are never granted.** `CompleteResearch` logs `breakthroughItem` but no system awards the item; the unlock chain stops at an authored ID with no consumer (§34C.1).
8. **M — Phantom ID in UI selftest.** `Main.UiTests.PlayerPanels.cs:138` unlocks `res_rad_mapping` — not a `knowledge_*` ID, not in any catalog.
9. **M — No structural gate.** `ValidateDag` exists but nothing runs it in the headless selftest pipeline; a prereq cycle or empty catalog would not fail any gate.

## Parity evidence (pre-removal)

Field-by-field comparison of the 31 C# defs vs JSON: **31/31 identical, 0 mismatches**
(script + permanent regression pin added under `Ashfall.Core.Tests` — see
`ResearchCatalogParityTests`). The 25 JSON-only nodes are additive content.
