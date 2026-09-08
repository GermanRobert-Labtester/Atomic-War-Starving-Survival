# Autopsy Runtime Contract

Verified 2026-09 from `Assets/Ashfall.Core/AutopsySystem.cs` (244 lines) and `AutopsyProcedureCatalogLoader.cs` (50 lines), read end-to-end.

## Catalog DTO (`AutopsyProcedure`)

| JSON field | C# | Type | Notes |
|---|---|---|---|
| `procedure_id` | `procedure_id` | string | Unique key (ordinal dict); `procedure_` prefix convention |
| `display_name` | `display_name` | string | |
| `required_tools` | `requiredTools` | List\<string\> | item IDs |
| `required_consumables` | `requiredConsumables` | List\<string\> | item IDs |
| `airborne_risk` | `airborneRisk` | float | default 0.1 |
| `pathogen_risk` | `pathogenRisk` | float | default 0.05; **authored data not consumed by current TickDay risk roll** (see below) |
| `procedure_hours` | `procedureHours` | **int** | existing values 3–7 |
| `possible_findings` | `possibleFindings` | List\<string\> | inline `finding_*` strings; no external finding catalog |
| `research_unlocks` | `researchUnlocks` | List\<string\> | `knowledge_*` IDs from `research_knowledge.json` |

Loader: simple deserialize of `{schema_version, collection_id, procedures: [...]}`; no cross-validation of item/finding/research references (references verified at authoring time + by data-integrity selftest).

## Selection model

**Player-selected method.** `QueueAutopsy(specimenId, procedureId, medicId)` — any cataloged procedure may be queued for any unprocessed specimen. No cause filtering, no corpse-state filtering, no recommendation.

- **One autopsy per specimen ever** (`completedSpecimenIds`); a processed specimen cannot be re-examined with a different procedure.
- Tool + consumable availability checked at queue time (`Blocked("missing_tool")` / `Blocked("missing_consumable")`).

## Cost semantics

At `BeginAutopsy`, **tools AND consumables are both consumed** (1 each) atomically via `Inventory.TryExecuteTransaction`. "Tools" are treated as single-use supplies by the current runtime — authored tool lists follow this existing semantic.

## Duration & tick

`TickDay(day)` advances each in-progress case by **+8 hours/day**; completes when `progressHours >= procedureHours`. Staff occupancy is modeled by the case being InProgress.

## Risk resolution

Per day-tick per in-progress case: `_rng.NextDouble() < airborneRisk` → `containmentBreach = true` + a `VentilationSource` (requiresExhaust) registered with the owned `VentilationSystem` (host subscribes for hazard warnings).

- Seeded `ISeededRng` — deterministic.
- **`pathogenRisk` is currently authored-but-unconsumed** (no code path rolls it). Preserved as data; documented for future consumers.
- PPE/tool modifiers: none in the current roll.

## Findings & research

- On completion: **one finding chosen at random** from `possible_findings` (seeded `_rng.Next`) — not all findings fire.
- **All** `research_unlocks` granted via `ResearchSystem.UnlockManual(id)` on completion. IDs resolve against `research_knowledge.json` (56 `knowledge_*` nodes).

## Save

`AutopsyState { cases, completedSpecimenIds }` via `CaptureState`/`RestoreState` (JSON clone). No schema change under Plan 79; new stable procedure IDs fit the existing store (only `procedureId` strings are persisted in cases).

## Failure/edge behavior

- Unknown procedure/case → `Failed`; wrong state → `Blocked`.
- Completed/failed cases are removed from the active list after tick; completion is one-shot per specimen.
