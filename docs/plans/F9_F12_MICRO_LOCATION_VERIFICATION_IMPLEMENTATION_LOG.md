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

Status: PENDING
