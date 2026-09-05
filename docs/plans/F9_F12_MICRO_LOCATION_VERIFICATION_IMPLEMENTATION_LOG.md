# F9–F12 Micro-Location Verification Wave — Implementation Log

Plan: Flagship Micro-Location Persistence, Determinism, Utilization & Reward-Economy Verification (Tasks F9–F12).

## Phase 0 — Architecture Reconnaissance (Wave A)

Status: PASS (no code changes; baseline recorded)

### Baseline evidence

- `dotnet build Ashfall.csproj` — 0 errors, 0 warnings.
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — 0 errors, 0 warnings.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — (results recorded in Wave B entry below).

### Verified call chain (production)

```
ExpeditionSystem.TickHours(hours, rng)            [single SeededRng stream]
  -> per-leg RollEncounter(exp, rng)              ExpeditionSystem.cs:1160
     rng.NextDouble() < exp.encounterChancePerTick
     -> OnEncounterTriggered(exp)
        -> ExpeditionHostSession bridge surface   src/Host/ExpeditionHostSession.cs:206
           -> ExpeditionEncounterBridge.Surface   Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs:91
              -> NarrativeEncounterSystem.SelectEncounter(stance, danger, locationId, rng, lootCategories)
                 pass 1: eligible-weight sum (depleted / weather-gated / weight-0 excluded BEFORE weighting)
                 pass 2: single rng.NextDouble() roll over filtered total
              -> OnSurfaced(dto) -> EnqueuePending(encounterId, locationId, legIndex, day)
Player choice
  -> bridge.ResolveChoice / NarrativeEncounterSystem.TryResolve (validation precedes mutation)
  -> host ApplyEncounterConsequences: item -> journal -> location -> world flag (each via its idempotent authority)
```

### Save contract (F9) — ALREADY IMPLEMENTED in Core; wave adds missing test evidence

- `NarrativeEncounterState.depletedEncounterIds` (`EncounterCatalog.cs:151`) is part of the
  production save DTO. `CaptureState()` copies through `CaptureDepletedIds()` — defensive copy,
  `string.CompareOrdinal` sort (INV-01, INV-05 satisfied).
- `RestoreState()` clears then rebuilds; a present list (even empty) is authoritative; a **null**
  list (legacy pre-F1 save) reconstructs depletion from resolution history via
  `ReconstructDepletionFromHistory()` (documented §48 migration — unknown ids skipped, never guessed).
- Store: `src/Host/NarrativeSaveStore.cs` — checksummed envelope via `SaveStoreHub.Checksummed`
  + `CapturePersisted` into the single campaign envelope (Initiative #42).
- Host application is idempotent by authority: journal via KnowledgeBase dedup gate
  (`TryDiscoverKnowledge`), location via `DiscoverLocation`/`IsLocationKnown`, world flag via
  `Flags.IsSet` + ResolutionId. Rewards ride the persisted `history`; the depletion set is the
  selection gate (INV-02, INV-03 satisfied — depletion is never inferred from effects).

### RNG contract (F10)

- One authoritative stream: the host passes the same `ISeededRng` to `TickHours` and the bridge
  (documented in `ExpeditionEncounterBridge` header). No `new Random()`/`Guid`/time entropy in
  the selection path (INV-06 satisfied).
- `SeededRng` (xorshift64*, `HostDefaults.cs:121`) exposes `State` (ulong) and a
  `(int seed, ulong state)` constructor — full draw-position capture/restore is available to harnesses.
- **Known architectural boundary (documented, intentional):** the host session constructs
  `SeededRng(DemoSeed)` at startup and does not persist the draw position. Depletion, pending
  queue, and history persist; the post-reload encounter stream restarts from `DemoSeed`. Core
  harnesses prove continuation parity at the level where RNG state is serialized (plan F10.9
  escape clause: document the actual architecture rather than forcing a different one).

### Cadence/cooldown

No cooldown/cadence state exists for micro-locations. Per-tick exposure is gated solely by
`encounterChancePerTick` (ExpeditionSystem), weights, route affinity, danger, weather gates, and
depletion. There is no cooldown state to persist (plan F10.12 maps to proving the chance-roll +
selection pipeline deterministic; nothing to carry across a save boundary).

### Content schema (F11/F12)

- `micro_locations.json`: `schema_version 1`, `collection_id micro_locations_catalog`,
  wrapped `encounters` list.
- **Divergence D1:** catalog holds **28** entries (plan assumed 25; content grew after drafting).
  Audits target all 28; count assertions pin 28.
- **Divergence D2:** `micro_roadside_memorial` has a depleting `take_offering` choice and a
  non-depleting `leave_memorial` choice. INV-04 is exercised through `leave_memorial`
  (resolving leave must not convert the encounter into a one-shot).
- Named-outlier values verified against `items.json`: `medical_kit` 10, `canned_food` 12,
  `cloth` 1.2, `wedding_ring` 25, `fuel` 14, `clean_water` 15.
- 53 authored expedition destinations (`expeditions.json`) drive audit contexts.

### Plan adaptations (recorded per ashfall-implement divergence policy)

- F9.13 "legacy missing field → empty set" is superseded by the shipped, documented
  reconstruct-from-history migration (strictly better: legacy campaigns do not refill resolved
  content). Test already pins it (`Restore_LegacyStateWithoutDepletion_ReconstructsFromHistory`).
- F11 count 25 → 28 (D1).
- F10.12 cadence → no-op (no cooldown state exists; determinism proof covers chance + selection).

## Phase 1 — Wave B (F9 persistence evidence)

Status: PASS (commit 45307130)

Changed:
- `Ashfall.Core.Tests/MicroLocationPersistenceWaveTests.cs` — 8 tests: production wire
  round-trips (SystemTextJsonSerializer payload) for depletion and pending surfacing,
  insertion-order-independent ordinal capture, capture immutability, malformed
  duplicate-id collapse, INV-03 reward independence, legacy null-field reconstruction,
  world-flag reapply convergence.

Tests: 8/8 pass. Divergences: F9.13 empty-set default superseded by shipped §48
history reconstruction (see D3 above); wire test pins that contract instead.

## Phase 2 — Wave C (F10 determinism)

Status: PASS

Changed:
- `Ashfall.Core.Tests/MicroLocationDeterminismHarness.cs` — production-wiring fixture
  (full catalog, registered destinations, scavenging authority, ONE shared stream),
  passive per-tick trace, ordinal depletion snapshots via CaptureState, draw-count
  continuation replay (`CountingRng.ReplayDraws`).
- `Ashfall.Core.Tests/MicroLocationDeterminismTests.cs` — 9 tests: named seeds 42/99/7
  ×8 ticks repeat (runA/B/C), save@tick4 continuation == uninterrupted (INV-09),
  metadata zero-draw contract, zero-eligible zero-draw, source-scan INV-06 gate,
  stream-identical replay, depletion filtering determinism, 100-seed sweep 100/100.
- `docs/discovery/MICRO_LOCATION_DETERMINISM.md` — the 10-section verified contract.

Divergences: `SeededRng` lost its `State` getter/ctor mid-wave (upstream commit) —
continuation checkpoints are draw counts replayed into fresh worlds; documented in
the harness header and determinism doc §2/§7.

## Phase 3 — Wave D (F11 utilization)

Status: PASS

Changed:
- `Ashfall.Core.Tests/MicroLocationUtilizationAuditTests.cs` — 7 tests: 28 unique ids +
  required fields, item/requiredItem references resolve, discovery/required locations
  resolve + structural reachability, journal namespace discipline, eligibility-context
  matrix over all authored destinations (28/28 have ≥1 context), reproducible
  1000-opportunity simulation with dead/orphan/too-common/sample-luck classification,
  redundancy scan (0 pairs above threshold).
- `docs/discovery/MICRO_LOCATION_UTILIZATION.md` — generated (env
  `ASHFALL_GEN_MICRO_REPORTS=1`), regeneration documented in-file.

Findings (reported, not failures, per INV-10): 0 dead, 0 orphan; 3 entries not
selected in the 1000-opportunity sample with expected < 1 (supply_drop, chapel
ledger, levy board — rare by weight); 4 low-yield; 0 redundant pairs.

## Phase 4 — Wave E (F12 economy)

Status: PASS

Changed:
- `Ashfall.Core.Tests/MicroLocationEconomyAuditTests.cs` — 7 tests: ledger validation
  (all grants resolve, finite values, outlier figures pinned: supply_drop 20, ring 25,
  memorial 1.2, shrine cost), deterministic 100-expedition production-path simulation,
  farming resistance (depleting grants one-shot across 64 seeds × every grant entry;
  non-depleting grants must be net non-positive), outlier shape pinning, env-gated
  balance report.
- `docs/discovery/MICRO_LOCATION_BALANCE.md` — generated (same env gate).

Headline: mean primary 7.69 vs mean micro 1.50 trade value/expedition → **19.5%**
micro/primary ratio — inside the 10–30% band → **no balance tuning** (INV-10).
Methodology notes: greedy max-item-value resolution = upper bound (0 journal unlocks
and −3 morale/+6 guilt under greedy are methodology artifacts; the full reward
structure — 16 journal keys, 2 discoveries, morale/guilt economics — is in the
per-choice ledger table); only 8 micro-locations surfaced across 100 sorties because
micros are a minority of the full encounter pool at ~0.06 trigger odds/tick.

## Phase 5 — Wave F (revalidation)

Status: PARTIAL — see below.

- Wave suite (31 tests: F9 8 + F10 9 + F11 7 + F12 7): **PASS** (serial execution;
  `ExpeditionDefinitionRegistry` is a static shared by loader paths, so the audit
  classes require the repo's DisableTestParallelization contract).
- `dotnet build Ashfall.csproj` / full-repo `dotnet test`: **blocked by concurrent
  in-flight work from other streams** (untracked Collectible*/CraftAttribution* /
  ContentAcceptanceLadder test files reference APIs that no longer exist on trunk —
  `SeededRng.State`, `CollectibleCatalogFileRaw`, `TradeSpecialtySystem.OnCraftCompleted`,
  `ContentExemption.ExpiryDate`). Not attributable to this wave; this wave's files
  compile against current trunk (proven by the scaffold build).
- Disclosed one-token fix to unblock the whole tree for every stream:
  `Assets/Ashfall.Core/Farming/CropStrainCatalog.cs:140` `files.Exists(path)` →
  `files.FileExists(path)` (the `IFileIO` port has no `Exists`).

## Cross-cutting adaptations (mid-wave trunk drift)

1. Route-affinity overload removal: another stream removed the F14
   `lootCategories` overloads (`GetEffectiveWeight` 4-arg, `SelectEncounter` 5-arg)
   during this wave. All wave code pins to the API intersection (3-arg weight,
   4-arg select) which compiles against both the pre- and post-drift trunk.
   Utilization/economy numbers measure **current-trunk selection semantics**.
2. Verification ran in an untracked local scaffold project
   (`.f9f12_scaffold/`, removed after the wave) referencing the live Core sources,
   because no commit boundary of the shared tree compiled during the wave
   (streams commit individual files while interdependent work sits uncommitted).
