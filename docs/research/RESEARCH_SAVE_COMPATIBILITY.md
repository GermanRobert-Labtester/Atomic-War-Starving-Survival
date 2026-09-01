# Research Save Compatibility (Plan 34 §34D)

## Representation

`Ashfall.Core.ResearchState` (ID-based, no indexes, no registration-order contract):

| Field | Meaning |
|---|---|
| `systemId` | `"research_system"` |
| `expansionUnlocked` | research feature flag |
| `currentDay` | day anchor for the active node's day budget |
| `activeResearchId` / `activeResearchDays` | queue head + accumulated days |
| `unlockedIds` | discovered/queued nodes (+ producer-granted ids) |
| `completedIds` | completed nodes (+ preserved unknown ids) |

**Best case confirmed (§34D.1):** saves persist IDs, so the 15→JSON externalization required
**no semantic migration** — old saves restore byte-for-byte via the unchanged ID set.

## Persistence (added by Plan 34)

Research progress previously was **never persisted** (no save section existed). Plan 34 added:

- `ResearchSaveStore` (`src/Host/ResearchSaveStore.cs`) — `SaveStore<ResearchState>` façade,
  file `research_save.json`, section key `research`, legacy `{ SchemaVersion, State, Checksum }`
  envelope, registered in `SaveSectionRegistry` (envelope whitelist + V1 migration derive from the registry).
- `Main.EnsureSharedResearch()` — on first construction loads `ResearchSaveStore.TryLoad()`
  into the engine **before** the catalog registers nodes, so saved flags mirror onto defs.
- `Main.SaveResearch()` — `CaptureSection("research", …)` in `SaveAllExpandedShelterSystems`.
- New game: `ResetSlotForNewGame` clears the slot exactly as for every other section.

## Unknown-node policy (§34D.5)

`ResearchSystem.CaptureState()` preserves every ID found in the state lists that is absent from
the loaded catalog (removed nodes, producer ids outside the catalog, legacy phantoms). Nothing
is silently discarded; a save round-tripped through a smaller catalog keeps its full ID set.
Pinned by `ResearchSaveIntegrationTests.CaptureState_PreservesUnknownNodeIds`.

## Round-trip matrix (§34D.11) — covered

| Scenario | Test |
|---|---|
| partial node + completed node → save → restore → resume | `ResearchSystemTests.CaptureState_RoundTrip_PreservesState` |
| envelope encode/decode → fresh engine → resume to completion | `ResearchSaveIntegrationTests.SaveRoundTrip_PreservesProgressAcrossFreshEngine` |
| unknown/removed node IDs in save | `ResearchSaveIntegrationTests.CaptureState_PreservesUnknownNodeIds` |
| completed node restored without re-firing completion (no re-grant) | `ResearchSaveIntegrationTests.RestoreState_DoesNotFireCompletedEvent_ButRestoresFlags` |
| ordering: save lists follow registration (JSON file) order; deterministic per catalog load | documented (§34D.2); UI sorts ordinally |

## Ordering (§34D.2)

- Identity/prerequisite resolution: dictionary keyed by ID.
- Save list order: catalog registration order == JSON file order (deterministic per catalog).
- UI: `ResearchPanel` iterates `Catalog.OrderBy(key)` (ordinal).
- Never filesystem/dictionary-order dependent in a way that changes semantics.
