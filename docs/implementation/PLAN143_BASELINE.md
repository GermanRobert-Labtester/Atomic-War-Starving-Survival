# Plan 143 baseline (pre-activation snapshot)

Verified 2026-09-09 against the current repository.

## Runtime baseline before Plan 143 activation

- `Assets/StreamingAssets/Data/narrative_arc_events.json` exists, has
  `schema_version: 1`, and contains 15 event records.
- The file is listed by the data-integrity scanner as optional content, but
  the live `NarrativeEncounterCatalogLoader` loads only
  `narrative_encounters.json`, `narrative_encounters_npc_arcs.json`, and
  `micro_locations.json`. The arc file therefore had no production consumer.
- `NarrativeEncounterSystem` owns the older encounter catalog, weighted
  selection, depleting choices, and its own resolution history. It does not
  own day-gated survivor arcs or the five effect verbs in this file.
- The campaign RNG manager already exposes the named `narrative` stream. The
  Plan 143 daily draw uses `CampaignRngStream`/`CampaignRngManager.Fork` with
  the campaign day and a fixed action index.
- The existing narrative save section is `narrative_save.json`, represented by
  `NarrativeEncounterState` and written through `NarrativeSaveStore` and the
  campaign envelope. Arc progress is added as one nested field so downstream
  faction, survivor, journal, and expedition state remains authoritative in
  its existing store.

## Baseline verification

```text
godot --headless --path . -- --data-integrity-selftest
PASS — 300 catalogs, 0 errors, 0 warnings

dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-restore \
  --filter FullyQualifiedName~NarrativeEncounter --verbosity:minimal
PASS — 36 tests

dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-restore \
  --filter 'FullyQualifiedName~NarrativeSaveChecksum|FullyQualifiedName~SaveStore|FullyQualifiedName~SaveWireContract' \
  --verbosity:minimal
PASS — 1,033 tests
```

## Historical drift

Older narrative documents describe display-only progression or other catalogs.
They are retained as historical context. The current JSON, live Core systems,
and current Godot host determine Plan 143 behavior.

## Live entry points and authorities

The pre-existing `NarrativeEncounterSystem` entry points are
`RegisterEncounter`, `RegisterRange`, `Find`, `GetEligibleCandidates`,
`SelectEncounter`, `Resolve`/`TryResolve`, `EnqueuePending`,
`ClearPending`/`ClearAllPending`, `CaptureState`, and `RestoreState`.
`NarrativeHostSession` exposes the Godot wrappers for that engine and now also
exposes `SelectArcForDay`, `CanApplyArcChoice`, `ResolveArcChoice`, and
`AcknowledgeArcEvent` for the Plan 143 system. The two systems share the
narrative save section but keep separate catalogs and authorities.

The campaign day coordinator is the day-advance authority. The Plan 143 host
owner performs one selection after the existing narrative/quest updates, using
`CampaignStreamIds.Narrative` and the day-indexed campaign RNG fork. Selection
stores only a pending event; choice callbacks are the only execution boundary.

Before activation, completion history existed only for old narrative encounters
and their depleting/pending records. There was no live survivor-arc ledger,
faction-intel score, or arc-specific completion state. The current integration
adds a bounded `NarrativeArcEventState` child to the narrative save DTO. The
existing `NarrativeSaveStore` and campaign envelope preserve it; morale remains
owned by `NeedsSystem`, faction standing by `FactionWarSystem`, intel by
`JournalSystem.Knowledge`, and expedition dispatch by `ExpeditionHostSession`.

The existing choice surfaces are `Main`'s player-panel registry and the new
presentation-only `NarrativeArcModal`. It emits typed event/choice IDs. The
host applies consequences through `NarrativeArcConsequenceAdapter`, updates
`LastEvent`, marks the narrative section dirty, and saves after a committed
choice. Restore binds pending presentation without replaying callbacks.

The baseline narrative checks were the data-integrity gate, 36 filtered
narrative tests, and 1,033 filtered save/checksum/wire tests. Final regression
results are recorded in `PLAN143_COMPLETION_REPORT.md`.
