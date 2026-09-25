# Plan 28 — Wildlife Observation, Infestations, and Food-Web Integration

> **Rebuild status:** CORE INTEGRATION LARGELY PRESENT — FORECAST, MARKET, AND PLAYER-SURFACE AUDIT REMAINS
>
> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE`
>
> **Claim:** `claim-oldest-15-piagents-quality-rebase-2026-09-25`
>
> **Current-evidence date:** 2026-09-25
>
> **Authority order:** live source and data → `AGENTS.md` → `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` → plan ledgers → this document.
>
> **Authority checksum:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
>
> **Length policy:** the 150k–170k band is a completeness checkpoint, never a reason to add filler. This plan is allowed to trim below the band if the verified architecture is exhausted.

## 0. Integrity Statement and Plan Status

This file replaces unclaimed generated sections that mixed current evidence, fictional APIs, and unsupported save claims. It is a planning and architecture artifact only. It authorizes no production, data, test, save, generated-index, or UI edits. Every path labeled current must exist at rebuild time. Any future `CREATE` proposal is explicitly hypothetical and belongs to a later, separately claimed implementation package.

The rebuild follows four passes: content/current-reality first; integration framework second; accuracy and contradiction removal third; independent precision and handoff review fourth. Character count is recorded by external verification, not embedded recursively in the document.

# 1. Objective

- Current source already implements migration-derived density, hunting pressure, observations, knowledge levels, seasonal movement, infestations, disease risk, food loss and bounded tolerated harvest.
- The remaining plan is a legibility and consequence-composition package: field observations and forecasts should read current migration/ecosystem state, infestations should route consequences through existing owners, and food-web opportunities should be authored projections rather than a parallel ledger.
- The plan must also distinguish this file from the separate migration and trapping plans by file path and owner.

**Bounded outcome:** Use `WildlifeMigrationSystem` as population truth, `WildlifeEcosystemSystem` for ecology/observations, `EcologicalInfestationSystem` for blooms, `BestiarySystem` for knowledge, trapping for harvest and disease/food/market owners for consequences. Do not add routes, migration windows, a second population model or a new infestation simulation.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- The ecosystem catalog has 13 species, five predator-prey edges and two seasonal moves.
- The infestation catalog has 10 rows with seasons, clear options, food loss and disease links.
- Wildlife ecosystem observations are bounded to 200 and feed bestiary knowledge levels.
- The trapping catalog owns 10 traps and 15 prey; it consumes migration/ecosystem density.
- Bestiary and wildlife field encounter prose already exist.

**Master-authority sections applied to this rebase:**

- Volumes 2–3 ecology seeds
- Volume 8 H-C9
- Volume 17 C14 roadmap
- Volume 45 incident census

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Expose known/suspected field observations and migration-derived forecasts as read models.
- Verify infestations route food loss and disease risk exactly once.
- Map a small food-web opportunity set to existing market/harvest owners.
- Close only proven player-surface and balance gaps.

Anything beyond this list is a different package. In particular, this plan does not convert a documentation gap into permission to create a second domain owner.

# 4. Current Evidence and Premise Audit

The current evidence set for this plan is enumerated in the appendices with file hashes, declaration digests, catalog schema/counts and focused test inventories. A source declaration proves an API surface exists; it does not prove a fresh test run or live player reachability. Those claims require the verification steps in this document.

**Evidence classes used here:**

- **VERIFIED CURRENT:** the named path exists and its contents were read during this rebuild.
- **HISTORICAL RECORD:** an archived closeout or old plan says a package once landed; it is useful context but is not current pass evidence.
- **PROPOSAL:** a future seam or file shape that requires a new claim and premise recheck.
- **UNKNOWN:** deliberately unresolved because the present plan does not need to invent an answer.

# 5. Existing Extension Seams

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| packs, population and routes | WildlifeMigrationSystem | `Assets/Ashfall.Core/WildlifeMigrationSystem*.cs` | Sole population store. |
| ecology pressure, observations and knowledge contribution | WildlifeEcosystemSystem | `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` | No second population. |
| infestation lifecycle and bounded consequences | EcologicalInfestationSystem | `Assets/Ashfall.Core/Ecology/EcologicalInfestationSystem.cs` | Sole infestation state. |
| player species knowledge | BestiarySystem | `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` | Knowledge authority. |
| harvest and downstream effects | Trapping/disease/market | `WildlifeTrappingSystem; DiseaseSystem; MarketSystem` | No ecology-local ledgers. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Wildlife Observation, Infestations, and Food-Web Integration
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ WildlifeMigrationSystem
│   packs, population and routes
│ WildlifeEcosystemSystem
│   ecology pressure, observations and knowledge contribution
│ EcologicalInfestationSystem
│   infestation lifecycle and bounded consequences
│ BestiarySystem
│   player species knowledge
│ Trapping/disease/market
│   harvest and downstream effects
                │
                ▼
Host projection → existing command → owner mutation → typed fact
                │
                ├─ UI / briefing / journal / audio presentation
                ├─ existing save envelope and checksum
                └─ focused Core / host / headless verification
```

The architecture is deliberately projection-first where a read model is sufficient, owner-extension-first where new mutable facts are required, and data-first only when an existing catalog can express the content. It does not permit a new subsystem merely to make the plan look larger.

## 6.1 Architectural decisions

1. **Preserve current state ownership.** WildlifeMigrationSystem owns packs, population and routes: Sole population store.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| packs, population and routes | WildlifeMigrationSystem | `Assets/Ashfall.Core/WildlifeMigrationSystem*.cs` | Sole population store. |
| ecology pressure, observations and knowledge contribution | WildlifeEcosystemSystem | `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` | No second population. |
| infestation lifecycle and bounded consequences | EcologicalInfestationSystem | `Assets/Ashfall.Core/Ecology/EcologicalInfestationSystem.cs` | Sole infestation state. |
| player species knowledge | BestiarySystem | `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs` | Knowledge authority. |
| harvest and downstream effects | Trapping/disease/market | `WildlifeTrappingSystem; DiseaseSystem; MarketSystem` | No ecology-local ledgers. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. migration/ecosystem daily update
2. derive density/season/observation forecast
3. surface confirmed versus uncertain signs
4. player acts through expedition/trapping/clear command
5. infestation/market/disease owners mutate
6. record observation and journal fact
7. persist each owner

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Migration packs persist in the world save.
- Ecosystem state stores pressure/extinction/apex/observations, not population.
- Infestation records persist lifecycle and roll counts.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Population is always read from migration packs.
- Forecast confidence never claims more than evidence.
- Infestation clear cost uses inventory callback.
- Food-web effects route through market/harvest owners.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- No migration-route or prey-row expansion in this package.
- Maintain current ecosystem/infestation/bestiary/trapping catalogs.
- New forecast prose must reference existing species/season/location IDs.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use existing world/ecosystem/infestation/bestiary/trap saves.
- No combined ecology save section.
- Restore cannot reroll infestation or duplicate harvest.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Migration and infestation use sanctioned seeded forks.
- Observations are state facts, not random UI projections.
- Stable ordering prevents hash iteration drift.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Ecosystem raises observation/extinction/apex/migration events.
- Infestation raises trigger/clear/harvest/exhaustion events.
- Bestiary consumes observations.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Main.EcologicalInfestations.cs
- src/UI/BestiaryPanel.cs
- src/UI/WildlifeTrappingPanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Field signs obey species/season/location facts.
- Food-web opportunities include costs and uncertainty.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | Ecosystem copies population. | WildlifeMigrationSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | Forecast exposes hidden exact population as certainty. | WildlifeEcosystemSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | Infestation consumes items directly outside host callback. | EcologicalInfestationSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | Disease risk bypasses immunity. | BestiarySystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | Market opportunity is guaranteed every season. | Trapping/disease/market | Focused negative test or static source gate; no broad-suite dependency. |
| F-06 | Bestiary unlocks from catalog presence rather than observation. | WildlifeMigrationSystem | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeEcosystemSystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/EcologicalInfestationSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeTrappingReplayTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeDiseaseBridgeTests.cs`
6. `godot --headless --path . -- --data-integrity-selftest` and content-utilization for ecology/trapping rows.

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — owner/count census | Map migration, ecosystem, infestation, bestiary and trapping. | No duplicate population/state. | No production path until the owning implementation package is separately claimed. |
| 1 — forecast projection | Derive signs/confidence from live owners. | Known/suspected labels are tested. | No production path until the owning implementation package is separately claimed. |
| 2 — infestation consequence trace | Verify food/disease/item callbacks. | Exactly-once application. | No production path until the owning implementation package is separately claimed. |
| 3 — food-web map | Document small authored opportunity/consequence chains. | No parallel ledger. | No production path until the owning implementation package is separately claimed. |
| 4 — surface and balance polish | Expose current truth in existing panels. | No guaranteed exploit. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs | READ; MODIFY only for proven forecast seam | Ecology owner |
| Assets/Ashfall.Core/Ecology/EcologicalInfestationSystem.cs | READ | Infestation owner |
| Assets/Ashfall.Core/Bestiary/BestiarySystem.cs | READ | Knowledge owner |
| src/UI/BestiaryPanel.cs | READ; MODIFY only for truthful projection gap | Presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Second population store. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Forecast certainty fabrication. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Double food/disease consequences. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Market exploit. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Plan-number collision with migration/trapping files. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No migration route/window data.
- No prey/trap catalog expansion.
- No new ecology ledger.
- No guaranteed food chain.

# 23. Rollback and Recovery

- Projection/UI work is reversible.
- State changes require focused migration and replay tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- Population and infestation owners are explicit.
- Current row counts are recorded.
- Forecast evidence classes are testable.
- No duplicate ecology state is proposed.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Expose known/suspected field observations and migration-derived forecasts as read models.
- Verify infestations route food loss and disease risk exactly once.
- Map a small food-web opportunity set to existing market/harvest owners.
- Close only proven player-surface and balance gaps.

## MUST NOT DO

- No migration route/window data.
- No prey/trap catalog expansion.
- No new ecology ledger.
- No guaranteed food chain.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeEcosystemSystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/EcologicalInfestationSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeTrappingReplayTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeDiseaseBridgeTests.cs`
6. `godot --headless --path . -- --data-integrity-selftest` and content-utilization for ecology/trapping rows.

## FIRST SAFE IMPLEMENTATION STEP

0 — owner/count census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: packs, population and routes → WildlifeMigrationSystem; ecology pressure, observations and knowledge contribution → WildlifeEcosystemSystem; infestation lifecycle and bounded consequences → EcologicalInfestationSystem; player species knowledge → BestiarySystem; harvest and downstream effects → Trapping/disease/market. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 28.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 28 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by WildlifeMigrationSystem or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/WildlifeMigrationSystem.cs`

### `Assets/Ashfall.Core/WildlifeMigrationSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 126 lines / 4626 bytes.
- SHA-256: `2eb9ed3fa4fc9ff2b919ff8dafa69aaa17461be2d7491ee100239f9e84ab76c8`.
- Architecture signals: seeded references=3; save/restore symbols=2; typed event declarations=2; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WildlifeSaveState
public int schema_version = 1;
public string systemId = WildlifeMigrationSystem.SystemId;
public int lastMigrationDay = -1;
public List<WildlifePackRecord> packs = new List<WildlifePackRecord>();
public sealed class WildlifePackRecord
public string packId = string.Empty;
public string speciesId = string.Empty;
public string currentSectorId = string.Empty;
public int population = 5;
public int seededPopulation;
public float aggressionScore = 0.5f;
public float starvationLevel;
public bool isRabid;
public int lastThreatFiredDay = -1;
public sealed partial class WildlifeMigrationSystem
public const string SystemId = "wildlife_migration";
public WildlifeSaveState State => _state;
public event Action<WildlifePackRecord> OnPackMigrated;
public ActionResult RegisterPack(string packId, string speciesId, string sectorId, int population) {
public ActionResult MigratePack(string packId, string targetSectorId) {
public void TickDay(int day) {
public WildlifeSaveState CaptureState() => CloneState(_state);
public void RestoreState(WildlifeSaveState saved) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/WildlifeMigrationSystem.Live.cs`

### `Assets/Ashfall.Core/WildlifeMigrationSystem.Live.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 312 lines / 14998 bytes.
- SHA-256: `a4179929ec4084166854cb2363faa1c393e176f437bd90249a4e1b3ece2ded55`.
- Architecture signals: seeded references=1; save/restore symbols=0; typed event declarations=1; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class WildlifeMigrationSystem
public const float MigrationChancePerDay = 0.25f;
public const float HungerDriveThreshold = 0.5f;
public const float MigrationStarvationRelief = 0.35f;
public const float StarvationLossThreshold = 0.7f;
public const float RabiesChancePerDay = 0.03f;
public const int BreathingRoomDaysForBirth = 3;
public void BindSeasonProfile(World.SeasonProfileDef? profile) => _seasonProfile = profile;
public void SetWaterSectors(IEnumerable<string>? sectorIds) {
public bool IsWaterSector(string sectorId) =>
public void SetSectorAdjacency(IEnumerable<(string sectorId, List<string> neighbors)> links) {
public void MergeSectorAdjacency(IEnumerable<(string sectorId, List<string> neighbors)> links) {
public bool TryGetNeighbors(string sectorId, out List<string> neighbors) {
public void SetSectorBlocked(string sectorId, bool blocked) {
public bool IsSectorBlocked(string sectorId) =>
public void ClearSectorBlockages() => _blockedSectors.Clear();
public int ApplyHarvestPressure(string sectorId, int amount) {
public WildlifePackRecord? TryGetPack(string packId) {
public int ThinSpeciesInSector(string speciesId, string sectorId, int amount, int floor = 2) {
public int GetSectorPackPopulation(string sectorId) {
public float GetGlobalPopulationRatio() {
public void TickDay(int day, ISeededRng? dayRng = null) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs`

### `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 651 lines / 32234 bytes.
- SHA-256: `beef926f2f7a20485888a8c6112ee6cbdbe2b5ace9750ac879c41022aabf64c8`.
- Architecture signals: seeded references=4; save/restore symbols=2; typed event declarations=13; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FaunaSpeciesDef
public string id { get; set; } = string.Empty;              // species_*
public string display_name { get; set; } = string.Empty;
public float radiation_tolerance { get; set; } = 0.5f;      // 0..1 vs OutdoorRadModifier/250
public string diet_type { get; set; } = "herbivore";        // herbivore | carnivore | scavenger
public int apex_population_threshold { get; set; }          // 0 = not an apex species
public bool tameable { get; set; }
public float tameness_chance { get; set; } = 0.15f;
public List<string> tags { get; set; } = new List<string>();
public sealed class PredatorPreyEdgeDef
public string predator_species_id { get; set; } = string.Empty;
public string prey_species_id { get; set; } = string.Empty;
public float predation_pressure { get; set; } = 0.05f;      // fraction of prey removed per day
public sealed class SeasonalMoveDef
public string species_id { get; set; } = string.Empty;
public string season_window_id { get; set; } = string.Empty;
public float move_chance { get; set; } = 0.1f;              // per pack per day in window
public sealed class WildlifeEcosystemContainer
public int schema_version { get; set; } = 1;
public List<FaunaSpeciesDef> species { get; set; } = new List<FaunaSpeciesDef>();
public List<PredatorPreyEdgeDef> predator_prey { get; set; } = new List<PredatorPreyEdgeDef>();
public List<SeasonalMoveDef> seasonal_moves { get; set; } = new List<SeasonalMoveDef>();
public sealed class PressureKeyState
public string sector_id { get; set; } = string.Empty;
public string species_id { get; set; } = string.Empty;
public int pressure { get; set; }
public sealed class ApexActivityState
public string species_id { get; set; } = string.Empty;
public string sector_id { get; set; } = string.Empty;
public int since_day { get; set; }
public int until_day { get; set; }
public bool spotted_reported { get; set; }
public sealed class DomesticAnimalState
public string animal_id { get; set; } = string.Empty;
public string species_id { get; set; } = string.Empty;
public int tamed_day { get; set; }
public string caretaker_id { get; set; } = string.Empty;
public sealed class WildlifeObservation
public string species_id { get; set; } = string.Empty;
public string sector_id { get; set; } = string.Empty;
public int day { get; set; }
public float confidence { get; set; } = 1f;
public sealed class WildlifeEcosystemState
public string system_id { get; set; } = "wildlife_ecosystem";
public int schema_version { get; set; } = 1;
public int last_tick_day { get; set; }
public List<PressureKeyState> pressures { get; set; } = new List<PressureKeyState>();
public List<string> extinct_species_sectors { get; set; } = new List<string>(); // "sector|species"
public List<ApexActivityState> apex_activities { get; set; } = new List<ApexActivityState>();
public List<DomesticAnimalState> domestic_animals { get; set; } = new List<DomesticAnimalState>();
public List<WildlifeObservation> observations { get; set; } = new List<WildlifeObservation>();
public int domestic_counter;
public sealed class WildlifeEcosystemSystem
public const string SystemId = "wildlife_ecosystem";
public const int ExtinctionThreshold = 2;      // remnant pair = locally extinct
public const int RecolonizationPopulation = 4;
public const int ApexDurationDays = 5;
public const int ObservationLogCapacity = 200;
public const int PressureDecayPerDay = 1;
public const float HazardAvoidanceMigrationChance = 0.35f;
public event Action<string, string>? OnWildlifeObserved;             // species, sector
public event Action<string, string>? OnLocalExtinction;              // species, sector
public event Action<string, string, string>? OnHazardAvoidanceMigration; // species, fromSector, toSector
public event Action<string, string>? OnApexPredatorSpotted;          // species, sector
public event Action<DomesticAnimalState>? OnWildlifeTamed;
public event Action<string, string, int>? OnWildlifePopulationShifted; // species, sector, delta
public string SaveId => SystemId;
public WildlifeEcosystemState State => _state;
public WildlifeEcosystemContainer Catalog => _catalog;
public IReadOnlyList<DomesticAnimalState> DomesticAnimals => _state.domestic_animals;
public IReadOnlyList<ApexActivityState> ApexActivities => _state.apex_activities;
public IReadOnlyList<WildlifeObservation> Observations => _state.observations;
public void LoadCatalog(WildlifeEcosystemContainer catalog) {
public FaunaSpeciesDef? Species(string speciesId) =>
public int SectorSpeciesPopulation(WildlifeMigrationSystem migration, string sectorId, string speciesId) {
public bool IsLocallyExtinct(string sectorId, string speciesId) =>
public float SectorDensityMultiplier(WildlifeMigrationSystem migration, string sectorId) {
public static string ExtinctionKey(string sectorId, string speciesId) => sectorId + "|" + speciesId;
public void RecordHuntingPressure(string sectorId, string speciesId, int amount) {
public int PressureOn(string sectorId, string speciesId) {
public void RecordObservation(string speciesId, string sectorId, int day, float confidence = 1f) {
public int ObservationCount(string speciesId) {
public string KnowledgeLevel(string speciesId) {
public void TickDay( int day, WildlifeMigrationSystem migration, float outdoorRadModifier, string seasonWindowId, ISeededRng populationRng,
public bool CanTame(string speciesId) =>
public DomesticAnimalState? TryTame( string speciesId, string sectorId, int day, string caretakerId, WildlifeMigrationSystem migration, ISeededRng tamingRng) {
public WildlifeEcosystemState CaptureState() {
public void RestoreState(WildlifeEcosystemState? state) {
public static class WildlifeEcosystemCatalogLoader
public const string DefaultFileName = "wildlife_ecosystem.json";
public static WildlifeEcosystemContainer Load( string dataDir, IFileIO? files = null, IJsonSerializer? json = null) {
public static List<string> Validate(WildlifeEcosystemContainer catalog) {
```


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/Ecology/EcologicalInfestationSystem.cs`

### `Assets/Ashfall.Core/Ecology/EcologicalInfestationSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 380 lines / 16936 bytes.
- SHA-256: `6489aa2766999583bc8973a62ec3a58283c485ee4d5d6113eb407231285c217f`.
- Architecture signals: seeded references=3; save/restore symbols=2; typed event declarations=8; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class EcologicalInfestationSystem
public const string SystemId = "ecological_infestation";
public const int RecurrenceCooldownDays = 5;
public const int MaxFoodLossPerDay = 3;
public EcologicalInfestationState State => _state;
public event Action<string>? OnInfestationTriggered;
public event Action<string, bool>? OnInfestationClearAttempt;
public event Action<string, string, int>? OnInfestationHarvested;
public event Action<string>? OnInfestationExhausted;
public void LoadDefinitions(IEnumerable<EcologicalInfestationDefinition>? definitions) {
public IReadOnlyCollection<EcologicalInfestationDefinition> Definitions => _definitions.Values;
public bool TryGetDefinition(string infestationId, out EcologicalInfestationDefinition? def) =>
public EcologicalInfestationRecord? GetRecord(string infestationId) {
public EcologicalInfestationRecord GetOrCreateRecord(string infestationId) {
public bool IsActive(string infestationId) {
public bool IsEligibleToTrigger(string infestationId, int currentDay, World.SeasonWindowDef? season) {
public bool TryTrigger(string infestationId, int currentDay, ISeededRng? dayRng, out string reason) {
public bool TryClear( string infestationId, string optionId, int currentDay, Func<string, int, bool> consumeItem, ISeededRng dayRng, out string resultSummary) {
public bool TryTolerateAndHarvest( string infestationId, int currentDay, out string itemId, out int amount, out string summary) {
public void TickDay( int day, ISeededRng? dayRng, Action<string, int>? foodLoss, Action<string>? diseaseRisk, World.SeasonWindowDef? season = null)
public EcologicalInfestationState CaptureState() {
public void RestoreState(EcologicalInfestationState? saved) {
```


# Appendix B.06 — Current Code Architecture: `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs`

### `Assets/Ashfall.Core/Bestiary/BestiarySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 316 lines / 12926 bytes.
- SHA-256: `0172edb956df8e3ed9e87ce2cb19d01cadddf975cb01758f4b55c41f312c784c`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=8; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CreatureDiscoveryRecord
public string CreatureId { get; set; } = string.Empty;
public int DiscoveredDay { get; set; } = 1;
public int EncounterCount { get; set; }
public int KillCount { get; set; }
public int ButcherCount { get; set; }
public string FirstLocationId { get; set; } = string.Empty;
public int LastEncounterDay { get; set; } = 1;
public List<string> UnlockedNoteKeys { get; set; } = new List<string>();
public sealed class CreatureSightingRecord
public string SightingId { get; set; } = string.Empty;
public string CreatureId { get; set; } = string.Empty;
public int Day { get; set; }
public string LocationId { get; set; } = string.Empty;
public string WitnessSurvivorId { get; set; } = string.Empty;
public string SightingType { get; set; } = "spotted"; // spotted, attacked, fled, tracks
public sealed class BestiaryState
public int SchemaVersion { get; set; } = 1;
public int NextSequence { get; set; } = 1;
public List<CreatureDiscoveryRecord> Discoveries { get; set; } = new List<CreatureDiscoveryRecord>();
public List<CreatureSightingRecord> Sightings { get; set; } = new List<CreatureSightingRecord>();
public sealed class BestiarySystem
public const int TotalCanonicalFaunaCount = 24;
public event Action<string>? OnCreatureDiscovered;
public event Action<string, int>? OnCreatureEncountered; // (creatureId, count)
public event Action<string, int>? OnCreatureKilled;      // (creatureId, count)
public event Action<string, string>? OnNoteUnlocked;     // (creatureId, noteKey)
public int DiscoveredCount => _state.Discoveries.Count;
public WastelandBestiaryCatalog Catalog => _catalog;
public void LoadCatalog(string json, IJsonSerializer? serializer = null) {
public CreatureDiscoveryRecord RecordEncounter(string creatureId, int day, string locationId = "", string witnessId = "") {
public void RecordKill(string creatureId, int day, string locationId = "") {
public void RecordButcher(string creatureId, int day) {
public CreatureDiscoveryRecord? GetDiscovery(string creatureId) {
public IReadOnlyList<CreatureDiscoveryRecord> GetAllDiscoveries() => _state.Discoveries;
public IReadOnlyList<CreatureSightingRecord> GetRecentSightings(int limit = 20) {
public float GetCompletionPercentage() {
public bool IsBasicStatsUnlocked(string creatureId) {
public bool IsBehaviorUnlocked(string creatureId) {
public bool IsCombatTacticsUnlocked(string creatureId) {
public BestiaryState CaptureState() {
public void RestoreState(BestiaryState? saved) {
public BestiaryCensus GetCensus() {
public struct BestiaryCensus
public readonly int TotalDiscovered;
public readonly int TotalCanonicalFauna;
public readonly float CompletionPercentage;
public readonly int TotalSightings;
public readonly int TotalKills;
public readonly int TotalButchered;
```


# Appendix B.07 — Current Code Architecture: `Assets/Ashfall.Core/WildlifeTrappingSystem.cs`

### `Assets/Ashfall.Core/WildlifeTrappingSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1620 lines / 78902 bytes.
- SHA-256: `bc94947453b31bc97de2d069c1a56487a684e7679a736acec13c7f34e30d1c59`.
- Architecture signals: seeded references=13; save/restore symbols=2; typed event declarations=33; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WildlifeTrappingState
public string systemId = WildlifeTrappingSystem.SystemId;
public List<TrapSite> trapSites = new List<TrapSite>();
public int totalCatch;
public int totalToxicRemoved;
public List<string> firstCatchLoggedSpeciesIds = new List<string>();
public List<WildlifeTrappingPendingEvent> pendingEvents = new List<WildlifeTrappingPendingEvent>();
public int eventSequence;
public int nextDeploymentSequence;
public int rngSeed;
public ulong primaryRngState;
public int encounterRngSeed;
public ulong encounterRngState;
public int incidentRngSeed;
public ulong incidentRngState;
public sealed class TrapSite
public string siteId = string.Empty;
public string assignedHunterId = string.Empty;
public string baitType = string.Empty;
public string trapType = "snare"; // snare, deadfall, cage, pit
public string trapId = string.Empty; // Plan 36: catalog link
public int setDay = -1;
public int checkDay = -1;
public int checkIntervalDays = 2;
public int remainingDurability = -1; // -1 = legacy/untracked, >0 = operational, 0 = broken
public bool isBroken; // Plan 36: trap cannot produce catches when true
public bool hasCatch;
public string catchSpecies = string.Empty;
public string bycatchSpecies = string.Empty; // Plan 36 III: bycatch species if occurred
public float bycatchYield; // Plan VI: independently resolved secondary carcass yield
public bool bycatchToxic; // Plan VI: independently resolved secondary toxicity
public float carcassYield;
public bool isToxic;
public bool toxinRemoved;
public bool isMeatProcessed;
public bool hidePreserved;
public string diseaseId = string.Empty; // Tasks 5-8: resolved disease ID from catch
public float contaminationDose; // Tasks 5-8: resolved contamination dose in rads
public string bycatchDiseaseId = string.Empty; // Plan VI: resolved secondary disease ID
public float bycatchContaminationDose; // Plan VI: resolved secondary contamination dose
public string pendingNarrativeEvent = string.Empty;
public int deploymentSequence;
public sealed class BaitProfile
public string baitId = string.Empty;
public string displayName = string.Empty;
public float catchBonusMultiplier = 1.0f; // multiplies base catch chance
public float toxicReduction = 0.0f; // reduces toxic chance by this fraction
public List<string> preferredSpecies = new List<string>();
public int craftCostScrapMeat = 0;
public int craftCostRoots = 0;
public int craftCostChemicals = 0;
public sealed class QuarrySpecies
public string speciesId = string.Empty;
public string displayName = string.Empty;
public float baseYieldKg = 1.0f;
public float toxicChance = 0.2f;
public float hideYield = 0.0f;
public string hideItemId = string.Empty;
public string preferredTrapType = "snare";
public List<string> attractedByBaitIds = new List<string>();
public float minSkillLevel = 0.0f;
public sealed class WildlifeSelectionContext
public static readonly WildlifeSelectionContext Default = new WildlifeSelectionContext();
public string SeasonWindowId { get; set; } = string.Empty;
public WeatherKind CurrentWeather { get; set; } = WeatherKind.Clear;
public HashSet<string> PresentMigrationSpecies { get; set; } = new HashSet<string>(StringComparer.Ordinal);
public Dictionary<string, float> AbundanceFactors { get; set; } = new Dictionary<string, float>(StringComparer.Ordinal);
public Dictionary<string, float> HunterSkillLevels { get; set; } = new Dictionary<string, float>(StringComparer.Ordinal);
public sealed class WildlifeTrappingSystem
public const string SystemId = "wildlife_trapping";
public WildlifeTrappingState State => _state;
public event Action OnTrappingChanged;
public event Action<string, string, string, bool> OnButcheryCompleted; // siteId, butcherId, species, isToxic
public event Action<ButcheryCompletedEvent>? OnButcheryCompletedDetailed;
public event Action<string, string> OnHidePreserved; // siteId, hideItemId
public event Action<string, string>? OnTrophyReady;
public event Action<string, string, string>? OnNewSpeciesDiscovered;
public event Action<string, string, string, string, int, string>? OnBycatchOccurred;
public event Action<BycatchOccurredEvent>? OnBycatchResolved;
public event Action<TrapLifecycleEvent>? OnTrapDeployed;
public event Action<TrapLifecycleEvent>? OnTrapBroken;
public event Action<TrapLifecycleEvent>? OnTrapRepaired;
public event Action<TrapLifecycleEvent>? OnTrapRemoved;
public event Action<WildlifeTrappingPendingEvent>? OnPendingEventCreated;
public static int DeriveEncounterStreamSeed(int parentSeed) {
public static int DeriveIncidentStreamSeed(int parentSeed) {
public void RegisterBait(BaitProfile bait) {
public void RegisterQuarry(QuarrySpecies species) {
public void RegisterPreyDefinition(PreyDefinition prey) {
public void RegisterTrapDefinition(TrapDefinition trap) {
public void SetHunterSkill(float skillLevel) {
public void SetSelectionContext(WildlifeSelectionContext context) {
public static float SkillMultiplierFor(float skillLevel) {
public static float WeatherPenaltyFor(WeatherKind kind) {
public static float CalculateWeatherMultiplier(float weatherSensitivity, WeatherKind weather) {
public Func<WeatherKind, float>? WeatherPenaltyProvider { get; set; }
public float EffectiveWeatherPenalty(WeatherKind weather) => WeatherPenaltyProvider != null
public static float CalculatePrimaryCatchChance( float densityMultiplier, float hunterSkillLevel, float baitMultiplier, float weatherSensitivity, WeatherKind weather,
public List<WildlifeTrappingPendingEvent> GetPendingEvents() {
public bool MarkEventDelivered(string eventId) {
public int CountPendingEvents(string kind) {
public bool CanSetTrapAtSite(string siteId, out string failureCode) {
public ActionResult SetTrap(string siteId, string baitType, string hunterId, string trapType = "snare", string trapId = "", int checkIntervalDays = -1, int durabilityChecks = -1) {
public const float BaseCatchChance = 0.5f;
public List<string> GetEligibleQuarryIds(string baitType, string trapType, float hunterSkillLevel, string trapId = "") {
public ActionResult CheckTraps(float densityMultiplier = 1f) {
public ActionResult Butcher(string siteId, string butcherId = "") {
public ActionResult PreserveHide(string siteId, out string hideItemId, out float hideQuantity) {
public ActionResult TransferCatchToInventory(string siteId, Inventory.Inventory inventory, string fallbackRawMeatId = "raw_meat") {
public string GetTrophyRecipeForSpecies(string speciesId) {
public IReadOnlyDictionary<string, BaitProfile> GetBaitCatalog() => _baitCatalog;
public IReadOnlyDictionary<string, QuarrySpecies> GetQuarryCatalog() => _quarryCatalog;
public IReadOnlyDictionary<string, TrapDefinition> GetTrapDefinitionCatalog() => _trapDefinitionCatalog;
public IReadOnlyDictionary<string, PreyDefinition> GetPreyDefinitionCatalog() => _preyDefinitionCatalog;
public bool RollDiseaseRisk(float diseaseRisk) {
public bool RollContaminationRisk(float contaminationRisk) {
public ActionResult RepairTrap(string siteId, int restoreDurability) {
public ActionResult RemoveTrap(string siteId) {
public ActionResult RemoveToxin(string siteId) {
public void TickDay(int day, float densityMultiplier = 1f) {
public WildlifeTrappingState CaptureState() {
public void RestoreState(WildlifeTrappingState saved) {
public static string BuildNarrativeIncidentSourceId(string siteId, string eventId) => $"wildlife-trap:{siteId ?? string.Empty}:incident:{eventId ?? string.Empty}";
```


# Appendix B.08 — Current Code Architecture: `src/Main.EcologicalInfestations.cs`

### `src/Main.EcologicalInfestations.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 245 lines / 11294 bytes.
- SHA-256: `125b192654c1b3a8358614a7c50079f7cddc2bffdd5b56e102f3f75f18c2e426`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public string SourceId => "ecological_infestation";
public IReadOnlyList<string> AuthoredDiseaseIds { get; } =
```


# Appendix B.09 — Current Code Architecture: `src/UI/BestiaryPanel.cs`

### `src/UI/BestiaryPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 281 lines / 11914 bytes.
- SHA-256: `b03a11488eaf5948a47f73cebfe9991ac5541c044f4b538a6889a67112f245d4`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=5; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class BestiaryPanel : Control
public event Action? OnClose;
public event Action<string, string>? OnActionRequested;
public bool IsBound => _host != null;
public void Bind(WildlifeEcosystemHostSession session, Func<WildlifeMigrationSystem?>? migrationProvider) {
public override void _Ready() {
public void RefreshView() {
public void SetWorldSector(string sectorId) => _worldSector = sectorId;
public void Open() {
public void Close() {
public override void _UnhandledInput(InputEvent @event) {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix B.10 — Current Code Architecture: `src/UI/WildlifeTrappingPanel.cs`

### `src/UI/WildlifeTrappingPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 524 lines / 21950 bytes.
- SHA-256: `9cacd807a62a562475a738f5344fae0374a26cce1df74fe7c35c191e8dabcb60`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class WildlifeTrappingPanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _host != null;
public string? SelectedRepairSiteId => _selectedRepairSiteId;
public Button? RepairButton => _repairBtn;
public OptionButton? RepairSiteDropdown => _repairSiteDropdown;
public Button? SetTrapButton => _setTrapBtn;
public AshfallStatusRail? StatusRail => _statusRail;
public void Bind(WildlifeTrappingHostSession session) {
public void Unbind() {
public override void _Ready() {
public void SelectRepairSite(string siteId) {
public void RefreshView() {
public override void _ExitTree() {
```


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/wildlife_ecosystem.json`

### `Assets/StreamingAssets/Data/wildlife_ecosystem.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 3850 bytes / 3850 characters.
- SHA-256: `8af036291671f572573fdb5d7afc1a751a2ed31c3d9ebc0157a203bb9723ffbf`.
- Root keys: `predator_prey`, `schema_version`, `seasonal_moves`, `species`.

Array-path census (minimum, maximum, observed rows):

```text
predator_prey: min=5, max=5, observed_paths=1
seasonal_moves: min=2, max=2, observed_paths=1
species: min=13, max=13, observed_paths=1
species[].tags: min=2, max=3, observed_paths=2
```

Representative record fields:

- `apex_population_threshold`
- `diet_type`
- `display_name`
- `id`
- `radiation_tolerance`
- `tags`
- `tameable`
- `tameness_chance`

Representative identifiers (ordered, capped for readability):

```text
species_cotton_hare
species_ash_hound
species_feral_goat
species_blight_rat
species_ash_boar
species_mirror_carp
species_ghost_moth
species_rad_dog
species_wolf
species_dust_lynx
species_iron_crow
species_ash_gull
species_gray_heron
```


# Appendix C.12 — Catalog Census: `Assets/StreamingAssets/Data/ecological_infestations.json`

### `Assets/StreamingAssets/Data/ecological_infestations.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 16361 bytes / 16357 characters.
- SHA-256: `2f0b2f8bd498895f344c1a4f5b0eaf942f2adcd4429173966527b07dc5d1e1e1`.
- Root keys: `collection_id`, `description`, `infestations`, `notes`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
infestations: min=10, max=10, observed_paths=1
infestations[].clear_options: min=2, max=2, observed_paths=2
infestations[].eligible_seasons: min=0, max=2, observed_paths=2
```

Representative record fields:

- `clear_options`
- `eligible_seasons`
- `food_loss_per_day`
- `hazard_summary`
- `id`
- `leave_benefit_summary`
- `leave_resource_amount`
- `leave_resource_item_id`
- `linked_disease_id`
- `max_harvests`
- `name`
- `requires_state`
- `scope`
- `target_id`
- `tolerated_hazard_risk`
- `trigger_chance_per_day`
- `trigger_summary`

Representative identifiers (ordered, capped for readability):

```text
infestation_subway_molerat_nest
infestation_quarry_hornet_hive
infestation_cellar_mold_bloom
infestation_bunker_roach_colony
infestation_canal_fungal_carpet
infestation_mill_rat_king
infestation_shelter_vent_mold
infestation_shelter_pantry_weevils
infestation_shelter_wall_nest
infestation_shelter_tray_cutworm
```


# Appendix C.13 — Catalog Census: `Assets/StreamingAssets/Data/wildlife_trapping_catalog.json`

### `Assets/StreamingAssets/Data/wildlife_trapping_catalog.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 23204 bytes / 23194 characters.
- SHA-256: `ad76e65163efc3860f4ef7d6b209479f27489c0f5cb34a717fe4e64bd5d64d00`.
- Root keys: `baits`, `prey`, `schema_version`, `traps`.

Array-path census (minimum, maximum, observed rows):

```text
baits: min=6, max=6, observed_paths=1
baits[].preferredSpecies: min=3, max=3, observed_paths=2
prey: min=15, max=15, observed_paths=1
prey[].activeSeasons: min=0, max=3, observed_paths=2
prey[].attractedByBaitIds: min=2, max=2, observed_paths=2
traps: min=10, max=10, observed_paths=1
traps[].compatiblePrey: min=4, max=5, observed_paths=2
traps[].narrativeIncidentIds: min=3, max=3, observed_paths=2
traps[].setupCosts: min=1, max=1, observed_paths=2
```

Representative record fields:

- `baseCatchModifier`
- `bycatchChance`
- `bycatchSpecies`
- `checkIntervalDays`
- `compatiblePrey`
- `description`
- `displayName`
- `durabilityChecks`
- `narrativeIncidentChance`
- `narrativeIncidentIds`
- `requiresWater`
- `setupCosts`
- `trapEncounterChance`
- `trapType`
- `trap_id`
- `weatherSensitivity`

Representative identifiers (ordered, capped for readability):

```text
trap_snare
trap_deadfall
trap_pit
trap_net
trap_fish
trap_cage
trap_bird_snare
trap_body_grip
trap_box
trap_improvised_wire
```


# Appendix C.14 — Catalog Census: `Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json`

### `Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 19491 bytes / 19488 characters.
- SHA-256: `e8a2feda7886c8ffa53710bb86de837825fe36ad1faaf32f51d2474047cb66b9`.
- Root keys: `collection_id`, `creatures`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
creatures: min=24, max=24, observed_paths=1
creatures[].harvestable_materials: min=3, max=3, observed_paths=2
creatures[].tags: min=6, max=6, observed_paths=2
```

Representative record fields:

- `acoustic_lure_frequency_hz`
- `butchered_meat_calories`
- `colloquial_name`
- `common_name`
- `creature_id`
- `harlan_scout_notes`
- `harvestable_materials`
- `pack_size_range`
- `primary_habitat`
- `tags`
- `threat_level`


# Appendix C.15 — Catalog Census: `Assets/StreamingAssets/Data/narrative/wildlife_field_encounter_logs.json`

### `Assets/StreamingAssets/Data/narrative/wildlife_field_encounter_logs.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 14717 bytes / 14629 characters.
- SHA-256: `220011a0a4285ae8a0c72d6356a85ef2d5f7b01874a2b031cc205c05b342b129`.
- Root keys: `collection_id`, `description`, `logs`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
logs: min=10, max=10, observed_paths=1
logs[].tags: min=4, max=5, observed_paths=2
```

Representative record fields:

- `behaviour`
- `day`
- `distance_metres`
- `individuals_observed`
- `location`
- `log_id`
- `observer`
- `species_common`
- `tags`
- `threat_assessment`


# Appendix D.16 — Existing Focused Test Inventory: `Ashfall.Core.Tests/WildlifeEcosystemSystemTests.cs`

### `Ashfall.Core.Tests/WildlifeEcosystemSystemTests.cs`

- Current test declarations: Fact=13, Theory=0, InlineData=0.
- File lines: 303; SHA-256: `4f728436468477c236b778ff1449c548ebafb814264ed84e5b8ea09feabf0563`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ShippedCatalog_Validates
CatalogValidation_RejectsBadEntries
Populations_DeriveFromTheSingleAuthority
PredatorPressure_ReducesPrey_BoundedByRemnant
RadiationAttrition_IsSpeciesSpecific
EcologyTick_IsDeterministic
LocalExtinction_FlagsAtThreshold_AndRecolonizes
ApexActivity_TriggersDeterministically_AndReportsOnce
Taming_OnlyForEligibleSpecies_AndTransfersOutOfWild
Pressure_DecaysOverDays
BestiaryKnowledge_IsObservationGated
SaveLoad_NextEcologyTickMatchesUninterrupted
OldSaveDefaults_RestoreSurvivesNulls
```


# Appendix D.17 — Existing Focused Test Inventory: `Ashfall.Core.Tests/EcologicalInfestationSystemTests.cs`

### `Ashfall.Core.Tests/EcologicalInfestationSystemTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 177; SHA-256: `9e8df420ed398e047cc8d0d45e650cc6c6194e3280823dde4df60812c3a4b1c2`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
TryTrigger_EligibleZeroChance_Triggers_AndNeverReTriggers
TryClear_ConsumesCostThroughTheCaller_AndResolvesOnSuccess
TryClear_MissingItem_Fails_AndStaysActive
TolerateAndHarvest_GrantsBoundedBenefit_ThenExhausts
TickDay_RoutesBoundedFoodLoss_OnlyWhileActive
TickDay_HazardRoll_IsDeterministic_UnderTheSameFork
CaptureRestore_RoundTriipsExactly_AndNeverDuplicatesOutcomes
```


# Appendix D.18 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs`

### `Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 173; SHA-256: `bcac2aa9b4ebfce8de4fa59e841d47a253bc1840e968d9c84778680f3a374ad5`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
LoadCatalog_LoadsWastelandBestiary
RecordEncounter_DiscoversCreatureAndLogsSighting
TieredUnlocks_ProgressWithEncounters
RecordKillAndButcher_UnlocksCombatAndHarvestNotes
GetCompletionPercentage_TracksCollectionProgress
SaveRestoreState_PreservesDiscoveriesAndSightings
```


# Appendix D.19 — Existing Focused Test Inventory: `Ashfall.Core.Tests/WildlifeTrappingReplayTests.cs`

### `Ashfall.Core.Tests/WildlifeTrappingReplayTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 506; SHA-256: `557b71ad022e24993159476705eb6fca6c1317df56400c700987c39e21b6fb0c`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ButcheryReplay_SaveLoadBoundary_Seed42_PreservesDiseaseAndContaminationOutcome
ButcheryReplay_SaveLoadBoundary_Seed123_PreservesDiseaseAndContaminationOutcome
ButcheryReplay_LowRiskPrey_RemainsDeterministic
ButcheryReplay_FinalHealthStateHash_MatchesAfterRestore
DeterministicReplay_MultiDayCampaignTrace_UninterruptedVsSavedRestored_MatchesExactEventTraceAndHash
DeterministicReplay_ThreeConsecutiveRuns_ProduceIdenticalStateAndEventHash
DeterministicReplay_BreakageBoundary_MatchesBreakDayAndPreventsSubsequentCatches
```


# Appendix D.20 — Existing Focused Test Inventory: `Ashfall.Core.Tests/WildlifeDiseaseBridgeTests.cs`

### `Ashfall.Core.Tests/WildlifeDiseaseBridgeTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 127; SHA-256: `7de8c8e1092d8b2ede8ab61b548f52c58961b2c381a0ac286594bb6ff3873a7e`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Butchery_WithoutSterile_HasChanceToInfect
Butchery_WithSterileTechnique_NeverInfects
Butchery_NoButcherId_DoesNotInfect
DiseaseSaveRoundTrip_PreservesInfection
```


# Appendix E.21 — Supporting Code Evidence: `src/Main.ShelterSocial.cs`

### `src/Main.ShelterSocial.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 602 lines / 30015 bytes.
- SHA-256: `efdcc3d73d212c2e6349a7a1bd586b42f082827ea76457826c93321a1fea5415`.
- Architecture signals: seeded references=0; save/restore symbols=17; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public string Id { get; }
public string DisplayName { get; }
public Ashfall.Core.Journal.RiskBiasTrait RiskBias => Ashfall.Core.Journal.RiskBiasTrait.Realist;
```


# Appendix E.22 — Supporting Code Evidence: `src/Main.Bestiary.cs`

### `src/Main.Bestiary.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 107 lines / 3208 bytes.
- SHA-256: `5551be1760c010cd9bf50a8bb24fcd9719753d788edd9d7f273da190ef03402f`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public BestiaryHostSession EnsureBestiary() {
public void FlushBestiarySave() {
public void ResetBestiary() {
public CreatureDiscoveryRecord RecordCreatureEncounter(string creatureId, string locationId = "", string witnessId = "") {
public void RecordCreatureKill(string creatureId, string locationId = "") {
public void RecordCreatureButcher(string creatureId) {
public BestiaryCensus GetBestiaryCensus() {
```


# Appendix E.23 — Supporting Code Evidence: `src/Main.Plans162_165.cs`

### `src/Main.Plans162_165.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 847 lines / 40656 bytes.
- SHA-256: `bc9a1f9abdf25d45de5c6d76902d4423d91cd2846802b0f7a48c08a08d6f5d9b`.
- Architecture signals: seeded references=11; save/restore symbols=10; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
```


# Appendix E.24 — Supporting Code Evidence: `src/Host/BestiaryHostSession.cs`

### `src/Host/BestiaryHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 80 lines / 2973 bytes.
- SHA-256: `5bb2029e86a40cab15f62e6cceeb7c631c53f5e0ed28838e36a8da962fe880d3`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class BestiarySaveStore
public const string SectionName = "bestiary_knowledge";
public const string FileName = "bestiary_knowledge_save.json";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string? TryCapturePersisted(BestiaryState state) => s_store.CaptureBare(state);
public static BestiaryState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
public static bool TrySave(BestiaryState state) => s_store.TrySave(state);
public static BestiaryState? TryLoad() => s_store.TryLoad();
public sealed class BestiaryHostSession : HostSessionBase
public BestiarySystem System { get; }
public static BestiaryHostSession Create(string dataDir, BestiaryState? restoredState = null) {
public CreatureDiscoveryRecord RecordEncounter(string creatureId, int day, string locationId = "", string witnessId = "") {
public void RecordKill(string creatureId, int day, string locationId = "") {
public void RecordButcher(string creatureId, int day) {
public BestiaryCensus GetCensus() => System.GetCensus();
public override void Save() {
```


# Appendix E.25 — Supporting Code Evidence: `Assets/Ashfall.Core/WildlifeTrappingCatalog.cs`

### `Assets/Ashfall.Core/WildlifeTrappingCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 388 lines / 16243 bytes.
- SHA-256: `a0934ce15a436f9b891633cf235112c96f0a716601ae22d4174e0096ee0dc63c`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class TrapDefinition
public string trap_id = string.Empty;
public string displayName = string.Empty;
public string description = string.Empty;
public string trapType = "snare";
public List<TrapSetupCost> setupCosts = new List<TrapSetupCost>();
public int checkIntervalDays = 2;
public int durabilityChecks = 8;
public float baseCatchModifier = 1.0f;
public List<string> compatiblePrey = new List<string>();
public bool requiresWater = false;
public float weatherSensitivity = 0.0f;
public float networkPenaltyPerTrap = 0f;
public float bycatchChance = 0f; // Plan 36 III: probability of bycatch on successful catch
public List<BycatchCandidate> bycatchSpecies = new List<BycatchCandidate>(); // Plan 36 III: weighted bycatch pool
public float narrativeIncidentChance = 0f;
public List<string> narrativeIncidentIds = new List<string>();
public float trapEncounterChance = 0f;
public InventoryBill CalculateRepairBill() {
public InventoryBill CalculateSetupBill() {
public bool Validate(out string error) {
public sealed class TrapSetupCost
public string itemId = string.Empty;
public int amount = 1;
public sealed class BycatchCandidate
public string speciesId = string.Empty;
public float weight = 1.0f;
public sealed class PreyDefinition
public string speciesId = string.Empty;
public string displayName = string.Empty;
public string description = string.Empty;
public float baseYieldKg = 1.0f;
public float toxicChance = 0.2f;
public float hideYield = 0.0f;
public string hideItemId = string.Empty;
public string preferredTrapType = "snare";
public List<string> attractedByBaitIds = new List<string>();
public float minSkillLevel = 0.0f;
public string migrationSpeciesId = string.Empty;
public List<string> activeSeasons = new List<string>();
public float diseaseRisk = 0.1f;
public float contaminationRisk = 0.05f;
public string diseaseId = string.Empty; // Plan 36 Closure II: per-species disease mapping
public float contaminationDose = 0f; // Plan 36 Closure II: explicit contamination dose in rads
public float moraleEffect = 0f;
public float moralWeight = 0f;
public bool isRareSpecies = false;
public const string FallbackDiseaseId = "disease_zoonotic_flu";
public const float FallbackContaminationDose = 2.0f;
public bool Validate(out string error) {
public string ResolveDiseaseId() => ResolveDiseaseId(this);
public static string ResolveDiseaseId(PreyDefinition? prey) {
public static string ResolveDiseaseId(float diseaseRisk, string? explicitDiseaseId = null) {
internal sealed class WildlifeTrappingCatalogFileRaw
public int schema_version = 1;
public List<TrapDefinition> traps = new List<TrapDefinition>();
public List<PreyDefinition> prey = new List<PreyDefinition>();
public List<BaitProfile> baits = new List<BaitProfile>();
public static class WildlifeTrappingCatalogLoader
public const string FileName = "wildlife_trapping_catalog.json";
public static WildlifeTrappingCatalog? Load( string dataDir, IFileIO fileIO, IJsonSerializer json, ILog? log = null) {
public sealed class WildlifeTrappingCatalog
public IReadOnlyDictionary<string, TrapDefinition> Traps => _traps;
public IReadOnlyDictionary<string, PreyDefinition> Prey => _prey;
public IReadOnlyDictionary<string, BaitProfile> Baits => _baits;
public void RegisterWith(WildlifeTrappingSystem system) {
```


# Appendix G.26 — Supporting Regression Evidence: `Ashfall.Core.Tests/WildlifeTrappingIntegrationTests.cs`

### `Ashfall.Core.Tests/WildlifeTrappingIntegrationTests.cs`

- Current test declarations: Fact=52, Theory=0, InlineData=0.
- File lines: 1375; SHA-256: `dbd9b3501f9e0e35166d96a69a821ca6e77baf8bdcc983d4a9329210311ef04b`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
SetTrap_AndCheck_ResolvesCatch
SaveAndRestore_PreservesTrapSites
WT_WX_001_ZeroSensitivityTrap_IgnoresWeatherPenalty
WT_WX_002_WeatherPenalty_FalloutStormWithSensitivity03_Produces15PercentReduction
WT_WX_003_WeatherPenalty_BlizzardWithSensitivity03_Produces24PercentReduction
WT_WX_004_ClearWeather_ProducesZeroPenalty
WT_WX_005_DeterministicReplay_SameSeedAndWeather_ProducesIdenticalCatch
WT_WX_006_BycatchIsolation_WeatherDoesNotAlterBycatchFormula
WT_WX_007_DurabilityDecrementsOnCheck_RegardlessOfWeather
WT_WX_008_ExhaustiveEnumPolicy_EveryWeatherKindHasExplicitMapping
WT_WX_009_PrimaryCatchChance_ClampsBetween005And095
WT_SK_001_SkillMultiplier_CurveEvaluation
WT_SK_002_SkillMultiplier_ClampsOutOfRangeValues
WT_SK_003_PerSiteHunterSkill_UsesAssignedHunterProgression
WT_SK_004_UnassignedSite_FallsBackToGlobalHunterSkill
WT_SK_005_SkillProgression_GetDisciplineProgress01_NormalizesCorrectly
WT_SK_006_TwoTraps_TwoHunters_EvaluatedIndependently
WT_SK_007_QuarryEligibilityPerHunter_MinSkillLevelGating
WT_SK_008_MidCampaignProgressionUpdate_SeenOnNextCheck
WT_SK_009_BycatchIsolation_SkillDoesNotModifyBycatch
WT_SK_010_DurabilityIsolation_SkillDoesNotAlterDurabilityDecrement
WT_SK_011_SharedAuthorityGuard_TrappingResolvesSharedSkillProgression
WT_JC_001_FirstCatch_FiresOnNewSpeciesDiscovered
WT_JC_002_SecondCatchSameSpecies_DoesNotFireEventAgain
WT_JC_003_DifferentSpecies_SequentialDiscovery_FiresOnceEach
WT_JC_004_FirstCatchLoggedSpeciesIds_RoundTripsThroughSaveRestore
WT_JC_005_LegacySaveWithoutFirstCatch_RestoresAsEmptyList
WT_JC_006_JournalEntry_CreatedOnce_WithValidAuthorAndDedup
WT_JC_007_JournalSystem_UnlockWildlifeCaught_UnlocksCodexKey
WT_JC_008_CodexEntries_ContainsAll15AuthoritativePreySpecies
WT_JC_009_Bycatch_NotCountedAsFirstCatch
WT_CS_001_DataContract_ImprovisedWire_RequiresNoStation
WT_CS_002_DataContract_BoxTrap_RequiresWorkbench
WT_CS_003_DataContract_FishTrap_RequiresWorkbench
WT_CS_004_StationlessRecipe_CraftableWithoutWorkbench
WT_CS_005_BoxTrap_BlockedWithoutWorkbench
WT_CS_006_FishTrap_BlockedWithoutWorkbench
WT_CS_007_BrokenWorkbench_BlocksBoxAndFishTraps
WT_CS_008_OperationalWorkbench_AllowsBoxTrap
WT_CS_009_OperationalWorkbench_AllowsFishTrap
WT_CS_010_ShelterNotBuilt_WorkbenchAbsent
WT_CS_011_ShelterBuilt_WorkbenchSynchronizes
WT_CS_012_StationLosesAvailability_BlocksNewCraft
WT_CS_013_NoUnconditionalProductionSeed_SourceGate
WT_XI_001_DailyWorldRefresh_OccursBeforeTrapCheck
WT_XI_002_FullCheck_UsesWeather_Density_Hunter_And_Bait
WT_XI_003_PostLoadContextRebuild_BeforeCheck
WT_XI_004_DiseaseAndContaminationBridge_Unchanged
WT_XI_005_OverhuntCatchPressure_Unchanged
WT_XI_006_PanelBinding_StillWorks_SourceGate
WildlifeTrapping_EndToEnd_CraftDeployMigrateButcherSaveRestoreBreak_IsDeterministic
WildlifeTrapping_EndToEnd_CatalogIntegrity_Deploy_BaitReach_Durability_SaveRoundTrip
```


# Appendix G.27 — Supporting Regression Evidence: `Ashfall.Core.Tests/WildlifeTrappingCatalogTests.cs`

### `Ashfall.Core.Tests/WildlifeTrappingCatalogTests.cs`

- Current test declarations: Fact=53, Theory=0, InlineData=0.
- File lines: 1085; SHA-256: `282c418468777621d6e9b517bc980962915302802818f11c804a7e0c8f837835`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_Loads
Catalog_Has10Traps
Catalog_Has15Prey
Catalog_Has6Baits
TrapIds_AreUnique
PreyIds_AreUnique
TrapSetupCosts_ResolveToItems
PreyHideItemIds_ResolveToItems
PreyMigrationIds_ResolveToKnownSpecies
PreyActiveSeasons_ResolveToKnownWindows
RegisterWith_PopulatesQuarryCatalog
RegisterWith_PopulatesBaitCatalog
TrapDefinitions_HaveDistinctTrapTypes
TrapDefinitions_HaveCompatiblePrey
PreyDefinitions_HaveValidPreferredTrapType
CatchResolution_WorksWithCatalogPrey
SaveRoundTrip_PreservesState
MissingFile_ReturnsNull
TrapIds_FollowConvention
PreyYieldItems_ResolveToRawMeat
SetTrap_WithCatalogParams_PersistsTrapId
SetTrap_LegacyCall_HasDefaultDurability
CheckTraps_DecrementsDurability
CheckTraps_DecrementsOnNoCatch
CheckTraps_BreaksAtZero
BrokenTrap_ProducesNoCatches
LegacyTrap_NeverBreaks
RepairTrap_RestoresDurability
RepairTrap_BlocksWhenNotBroken
SaveRoundTrip_PreservesDurability
SaveRoundTrip_PreservesBrokenState
ImprovisedWireSnare_BreaksBeforeCageTrap
LegacySave_DeserializesWithDefaults
LegacySave_MixedOldNewTraps
LegacySave_RestoreIntoRuntime
LegacyTrap_NeverBreaksAfterManyChecks
LegacySave_RoundTripPreservesDefaults
Replay_UninterruptedVsRestored_IdenticalOutcome
Replay_ThreeRuns_IdenticalHash
Replay_BreakOccursOnSameDay
EdgeCase_UnknownTrapId_BlocksSafely
EdgeCase_ExactCostBalance_DeploySucceeds
EdgeCase_FinalDurabilityCatch_ResolvesBeforeBreak
EdgeCase_BrokenTrap_NoRNGAdvancement
EdgeCase_RepairAfterSaveLoad
EdgeCase_LegacyTrap_RepairBlocked
CheckTraps_SetsHasCatch_AfterSuccessfulCatch
CheckTraps_RespectsCheckInterval
CheckTraps_DecrementsDurability_EveryCheck
CalculateRepairBill_SnareTrap_ComputesCeilHalf
CalculateRepairBill_CageTrap_ComputesCeilHalfPerItem
CalculateRepairBill_AggregatesDuplicateItemsBeforeHalving
CalculateRepairBill_EmptySetupCosts_YieldsEmptyBill
```


# Appendix G.28 — Supporting Regression Evidence: `Ashfall.Core.Tests/WildlifeTrapping/WildlifeTrappingRuntimeCompletionTests.cs`

### `Ashfall.Core.Tests/WildlifeTrapping/WildlifeTrappingRuntimeCompletionTests.cs`

- Current test declarations: Fact=28, Theory=0, InlineData=0.
- File lines: 995; SHA-256: `c423548dfec379a10a223738751c33a1f1eca7fdd0fe0eaff87bea1f62eccd1a`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Task1_01_SetupCosts_ExactBalance_AtomicallyDeductedAndTrapDeployed
Task1_02_SetupCosts_ShortByOneItem_FailsAtomically_ZeroItemsConsumed
Task1_03_SetupCosts_DuplicateCostEntries_AggregatedCorrectly
Task1_04_SetupCosts_ZeroCostTrap_DeploysWithoutDeductions
Task1_05_SetupCosts_TrapIdentityPreservedAcrossSaveLoad
Task2_01_Durability_DecrementsOnCheck_Catch
Task2_02_Durability_DecrementsOnCheck_NoCatch
Task2_03_Durability_BreaksAtZero
Task2_04_Durability_BrokenTrapProducesNoCatches_SkipsRng
Task2_05_Durability_LegacySave_DefaultsToMinusOne_NeverBreaks
Task2_06_Durability_SnareBreaksEarlierThanCage
Task2_07_Repair_RestoresDefinitionDurability_ClearsBroken
Task2_08_RepairBill_AffordabilityPreflight_WireAndBox
Task3_01_Crafting_ThreeCoreTrapRecipesExistAndResolveCleanly
Task3_02_Crafting_CraftItemThenDeploy_ConsumesItemWithoutDoubleCharging
Task4_01_SeasonGating_PreyExcludedOutOfSeason
Task4_02_SeasonGating_PreyIncludedInSeason
Task4_03_SeasonGating_YearRoundPrey_AlwaysEligible
Task4_04_MigrationGating_AbsentPackExcludesMigrationPrey
Task4_05_MigrationGating_PresentPackIncludesMigrationPrey
Task4_06_SeasonalAbundance_ZeroAbundanceExcludesPrey
Task4_07_DeterministicReplay_SameContextProducesIdenticalCatch
Task5_01_HighRiskPrey_DiseaseAndContaminationRecordedOnCatch
Task5_02_DeterministicMiss_NoDiseaseOrContaminationApplied
Task5_03_Contamination_DoseAppliedToRadiationSystem
Task5_04_Disease_InfectionRecordedInDiseaseSystem
Task5_05_SaveLoad_PreservesCatchRiskFields
Task8_01_Deterministic100DaySimulation_AndBaselineMetrics
```


# Appendix G.29 — Supporting Regression Evidence: `Ashfall.Core.Tests/WildlifeTrappingPersistenceTests.cs`

### `Ashfall.Core.Tests/WildlifeTrappingPersistenceTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 552; SHA-256: `e855f5ca0b876976f340316c20d93b96a296ca5c02c9c911a55fbf3843333aef`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CaptureSerializeDeserialize_PartiallyWornTrap_PreservesNewFields
CaptureSerializeDeserialize_BrokenTrap_PreservesBrokenState
Deserialize_LegacyTrapWithoutDurabilityFields_UsesFunctionalDefaults
RestoreLegacyThenCapture_EmitsCurrentTrapFields
WildlifeTrappingSaveStore_RoundTrip_PreservesDiseaseContaminationAndTrapFields
LegacySaveFixture_PrePlan36_LoadsWithFunctionalDefaultsAndPreservesState
MixedSaveFixture_LoadsAndPreservesAllFourTrapCategories
RestoreState_NullStringFields_NormalizedToEmptyString
RestoreState_NegativeOrInconsistentDurability_NormalizedCorrectly
```


# Appendix H.30 — Supporting Authority Document: `docs/ecology/WILDLIFE_MIGRATION_SCHEMA.md`

### `docs/ecology/WILDLIFE_MIGRATION_SCHEMA.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 63 lines / 3114 bytes.
- SHA-256: `71b22df394f987e00998b41d93bcd75841f3f14c717188dc2296b59e580232d3`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix H.31 — Supporting Authority Document: `docs/ecology/INFESTATION_MATRIX.md`

### `docs/ecology/INFESTATION_MATRIX.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 59 lines / 3757 bytes.
- SHA-256: `a6cebca783b69c49755896de185ba79cd6c2cbd92379f053daebdcbaf624dfb3`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| packs, population and routes | WildlifeMigrationSystem | ecology pressure, observations and knowledge contribution | WildlifeEcosystemSystem | Owner emits/reads a typed fact; no mirror state. |
| packs, population and routes | WildlifeMigrationSystem | infestation lifecycle and bounded consequences | EcologicalInfestationSystem | Owner emits/reads a typed fact; no mirror state. |
| packs, population and routes | WildlifeMigrationSystem | player species knowledge | BestiarySystem | Owner emits/reads a typed fact; no mirror state. |
| packs, population and routes | WildlifeMigrationSystem | harvest and downstream effects | Trapping/disease/market | Owner emits/reads a typed fact; no mirror state. |
| ecology pressure, observations and knowledge contribution | WildlifeEcosystemSystem | packs, population and routes | WildlifeMigrationSystem | Owner emits/reads a typed fact; no mirror state. |
| ecology pressure, observations and knowledge contribution | WildlifeEcosystemSystem | infestation lifecycle and bounded consequences | EcologicalInfestationSystem | Owner emits/reads a typed fact; no mirror state. |
| ecology pressure, observations and knowledge contribution | WildlifeEcosystemSystem | player species knowledge | BestiarySystem | Owner emits/reads a typed fact; no mirror state. |
| ecology pressure, observations and knowledge contribution | WildlifeEcosystemSystem | harvest and downstream effects | Trapping/disease/market | Owner emits/reads a typed fact; no mirror state. |
| infestation lifecycle and bounded consequences | EcologicalInfestationSystem | packs, population and routes | WildlifeMigrationSystem | Owner emits/reads a typed fact; no mirror state. |
| infestation lifecycle and bounded consequences | EcologicalInfestationSystem | ecology pressure, observations and knowledge contribution | WildlifeEcosystemSystem | Owner emits/reads a typed fact; no mirror state. |
| infestation lifecycle and bounded consequences | EcologicalInfestationSystem | player species knowledge | BestiarySystem | Owner emits/reads a typed fact; no mirror state. |
| infestation lifecycle and bounded consequences | EcologicalInfestationSystem | harvest and downstream effects | Trapping/disease/market | Owner emits/reads a typed fact; no mirror state. |
| player species knowledge | BestiarySystem | packs, population and routes | WildlifeMigrationSystem | Owner emits/reads a typed fact; no mirror state. |
| player species knowledge | BestiarySystem | ecology pressure, observations and knowledge contribution | WildlifeEcosystemSystem | Owner emits/reads a typed fact; no mirror state. |
| player species knowledge | BestiarySystem | infestation lifecycle and bounded consequences | EcologicalInfestationSystem | Owner emits/reads a typed fact; no mirror state. |
| player species knowledge | BestiarySystem | harvest and downstream effects | Trapping/disease/market | Owner emits/reads a typed fact; no mirror state. |
| harvest and downstream effects | Trapping/disease/market | packs, population and routes | WildlifeMigrationSystem | Owner emits/reads a typed fact; no mirror state. |
| harvest and downstream effects | Trapping/disease/market | ecology pressure, observations and knowledge contribution | WildlifeEcosystemSystem | Owner emits/reads a typed fact; no mirror state. |
| harvest and downstream effects | Trapping/disease/market | infestation lifecycle and bounded consequences | EcologicalInfestationSystem | Owner emits/reads a typed fact; no mirror state. |
| harvest and downstream effects | Trapping/disease/market | player species knowledge | BestiarySystem | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Expose known/suspected field observations and migration-derived forecasts as read models. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Verify infestations route food loss and disease risk exactly once. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Map a small food-web opportunity set to existing market/harvest owners. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Close only proven player-surface and balance gaps. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

Every requirement in the objective and delta must resolve to at least one current owner, one negative condition and one future focused verification. A requirement with no owner is removed or returned as `STALE_PLAN`.

# Appendix K — Status and Evidence Labels

| Claim type | Label | Meaning |
| --- | --- | --- |
| Current source | VERIFIED PATH INVENTORY | Appendices hash current files; declarations are read-only. |
| Historical completion | HISTORICAL, NOT FRESH TEST PROOF | Focused tests must be rerun by an implementation/verification package. |
| Proposed types/paths | PROPOSAL ONLY | Never shown as current evidence; requires a new claim. |
| Character count | COMPLETENESS CHECK ONLY | External verifier records it; no padding or repeated boilerplate. |

# Appendix L — Plan Maintenance and Re-Audit Triggers

Re-run the premise sweep when any of the following occurs:

1. A listed Core owner is renamed, split, merged or removed.
2. A listed catalog changes `schema_version`, root shape or consumer.
3. A save section, checksum contract or campaign-day order changes.
4. A listed test is removed, renamed or moved to quarantine.
5. A live ledger marks a surface sealed, retired, accepted or blocked.
6. A generated architecture/save/catalog matrix changes the owner relationship.
7. A new active claim touches any current or proposed path.

The re-audit records only changed evidence. Historical prose is not rewritten merely to appear current, and current evidence is not deleted merely because an old plan disagrees with it.

# Appendix M — Definition of a Safe No-Change Result

A safe no-change result is valid when the current implementation already satisfies the requested behavior. It records: current owner paths, focused tests that exist, any rerun performed by a future verification package, and the precise condition that would justify reopening. A no-change result does not create a placeholder subsystem, a synthetic integration framework, or a test solely to increase counts.

# Appendix N — Final Precision Checklist

- [ ] Every current path in this document exists or is explicitly labeled unavailable.
- [ ] Every proposed path/type is labeled `PROPOSAL` and excluded from current claims.
- [ ] No current owner is duplicated.
- [ ] Core architecture remains engine-free.
- [ ] JSON is described as data authority, not automatic reachability.
- [ ] Save impact names the current owner and migration behavior.
- [ ] Determinism names streams or explicitly states no randomness.
- [ ] UI remains presentation over owner commands.
- [ ] Tests are focused and current commands use `scripts/run_test.sh` policy.
- [ ] Sealed/retired/blocked ledger decisions are respected.
- [ ] No full-suite result is claimed without a dedicated execution window.
- [ ] No Unity dependency or historical architecture is proposed.
- [ ] Character count is not used as evidence of quality.
- [ ] The first implementation step is a premise recheck, not code creation.
- [ ] The implementation handoff can be executed without reinterpreting ownership.

# Appendix O — Handoff Record Template

```text
Package:
Current status rechecked:
Outcome implemented:
Files changed:
Current owner contract used:
Save section/version touched:
Determinism streams touched:
Focused verification commands and results:
Tests reused/added:
Known limitation or debt:
Shared files intentionally untouched:
Ready for independent sweep: yes/no
```


# Appendix Q.563 — Additional Current Architecture Evidence: `src/Host/WildlifeTrappingHostSession.cs`

### `src/Host/WildlifeTrappingHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 643 lines / 28260 bytes.
- SHA-256: `9318ab54790453b20a0f62683207ac150c70862300b1fbbdba9ac6fcd06cf8e6`.
- Architecture signals: seeded references=1; save/restore symbols=1; typed event declarations=6; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WildlifeTrappingHostSession
public WildlifeTrappingSystem System { get; }
public string LastEvent { get; private set; } = string.Empty;
public WildlifeTrappingCatalog? Catalog { get; set; }
public InventoryHostSession? Inventory { get; set; }
public Action<string, string, int>? ApplyDisease { get; set; }
public Action<string, float>? ApplyContamination { get; set; }
public Func<string, string, string, bool>? DeliverMoralConsequence { get; set; }
public Func<string, string, int, bool>? DeliverTrapEncounter { get; set; }
public Func<string, bool>? DeliverTrappingBroadcast { get; set; }
public Func<string, string, int, string, bool>? DeliverNarrativeIncident { get; set; }
public event Action<BycatchOccurredEvent>? OnBycatchOccurred;
public Func<int, bool>? DeliverButcheryFood { get; set; }
public Action<string, float, string>? ApplyMorale { get; set; }
public Func<PreyDefinition, string>? DiseaseResolver { get; set; }
public event Action<string>? OnTrapCrafted;
public ActionResult SetTrap(string siteId, string baitType, string hunterId) {
public bool CanSetTrapAtSite(string siteId, out string failureCode) => System.CanSetTrapAtSite(siteId, out failureCode);
public void SetSelectionContext(WildlifeSelectionContext context) => System.SetSelectionContext(context);
public ActionResult TrySetTrap(string siteId, string trapId, string baitType, string hunterId) {
public bool TryGetSetupBill(string trapId, out InventoryBill bill, out string reason) {
public bool CanAffordSetup(string trapId, out InventoryBill bill, out string failureReason) {
public float WildlifeDensityMultiplier { get; set; } = 1f;
public ActionResult CheckTraps(float? densityMultiplier = null) {
public void DeliverPendingEvents() {
public string ComposeBroadcastMessage(WildlifeTrappingPendingEvent ev) {
public const float FallbackContaminationDose = PreyDefinition.FallbackContaminationDose;
public const string FallbackDiseaseId = PreyDefinition.FallbackDiseaseId;
public ActionResult Butcher(string siteId, string butcherId = "") {
public static string ResolveDiseaseId(PreyDefinition prey) => PreyDefinition.ResolveDiseaseId(prey);
public ActionResult RemoveToxin(string siteId) {
public ActionResult PreserveHide(string siteId) {
public bool TryGetRepairBill(string siteId, out InventoryBill bill, out string reason) {
public bool CanAffordRepair(string siteId, out InventoryBill bill, out string failureReason) {
public ActionResult TryRepairTrap(string siteId) {
public ActionResult RemoveTrap(string siteId) {
public void ReconcileMapMarkers() {
public void TickDay(int day) {
public event Action<int>? OnCatchPressure;
public override void Save() {
```


# Appendix Q.564 — Additional Current Architecture Evidence: `src/Host/WildlifeTrappingSaveStore.cs`

### `src/Host/WildlifeTrappingSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 56 lines / 2823 bytes.
- SHA-256: `e7466c5984ace8f5eda69dc4f37361fdef786ec1a425251bc51342ca416b6518`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class WildlifeTrappingSaveStore
public const string FileName = "wildlife_trapping_save.json";
public const string SectionName = "wildlife_trapping";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static bool TrySave(WildlifeTrappingState state) => s_store.TrySave(state);
public static WildlifeTrappingState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(WildlifeTrappingState state) => s_store.CapturePersisted(state);
public static string TryCaptureDirect(WildlifeTrappingState state) => s_store.CaptureBare(state);
public static WildlifeTrappingState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(WildlifeTrappingState state) => s_store.CaptureBare(state);
public static WildlifeTrappingState? TryRestore(string json) => s_store.RestoreBare(json);
```


# Appendix Q.565 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Ecology/EcologicalInfestationDefs.cs`

### `Assets/Ashfall.Core/Ecology/EcologicalInfestationDefs.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 89 lines / 4101 bytes.
- SHA-256: `5f6f322744f0c1d65dc43d218a9f8c0898e66988124c3bfb5881708ae226a78f`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum EcologicalInfestationStatus
public sealed class InfestationClearOption
public string option_id = string.Empty;
public string display_name = string.Empty;
public string required_item_id = string.Empty;
public int required_item_count;
public float success_chance = 0.5f;
public string outcome_summary = string.Empty;
public float failure_backlash_chance;
public sealed class EcologicalInfestationDefinition
public string id = string.Empty;
public string name = string.Empty;
public string scope = "location";
public string target_id = string.Empty;
public List<string> eligible_seasons = new List<string>();
public float trigger_chance_per_day;
public string requires_state = string.Empty;
public string trigger_summary = string.Empty;
public string hazard_summary = string.Empty;
public int food_loss_per_day;
public float tolerated_hazard_risk;
public string linked_disease_id = string.Empty;
public string leave_resource_item_id = string.Empty;
public int leave_resource_amount;
public string leave_benefit_summary = string.Empty;
public int max_harvests = 1;
public List<InfestationClearOption> clear_options = new List<InfestationClearOption>();
public sealed class EcologicalInfestationRecord
public string infestation_id = string.Empty;
public int status = (int)EcologicalInfestationStatus.Inactive;
public int triggered_day = -1;
public int last_action_day = -1;
public int harvests_taken;
public int failed_clear_count;
public int trigger_roll_count;
public sealed class EcologicalInfestationState
public int schema_version = 1;
public string systemId = EcologicalInfestationSystem.SystemId;
public List<EcologicalInfestationRecord> records = new List<EcologicalInfestationRecord>();
public long rollCount;
```


# Appendix Q.566 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Ecology/EcologicalInfestationCatalog.cs`

### `Assets/Ashfall.Core/Ecology/EcologicalInfestationCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 49 lines / 1897 bytes.
- SHA-256: `594f1e8776d1d013e82d9db808d2865248c4f97a1f193e760941ab946ebbaac9`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class EcologicalInfestationCatalogLoader
public const string DefaultFileName = "ecological_infestations.json";
public static List<EcologicalInfestationDefinition>? Load( string dataDir, IFileIO fileIO, IJsonSerializer json, ILog? log = null) {
public const string FileName = "ecological_infestations.json";
public sealed class EcologicalInfestationFileRaw
public int schema_version = 1;
public string collection_id = string.Empty;
public string description = string.Empty;
public List<EcologicalInfestationDefinition> infestations = new List<EcologicalInfestationDefinition>();
```


# Appendix Q.567 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/WildlifeTrappingEvents.cs`

### `Assets/Ashfall.Core/WildlifeTrappingEvents.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 163 lines / 6491 bytes.
- SHA-256: `ff6c6808d26d4dc0699047da3c849e54269a72706bac6f6c83c9fddcc33bd8ac`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class TrapLifecycleEvent
public string siteId = string.Empty;
public string trapId = string.Empty;
public string trapType = string.Empty;
public bool isBroken;
public int day;
public sealed class ButcheryCompletedEvent
public string actionId = string.Empty;
public string siteId = string.Empty;
public string butcherId = string.Empty;
public string primarySpeciesId = string.Empty;
public string bycatchSpeciesId = string.Empty;
public float primaryYield;
public float bycatchYield;
public float totalYield;
public bool isToxic;
public bool bycatchToxic;
public int setDay;
public sealed class BycatchOccurredEvent
public string siteId = string.Empty;
public string trapId = string.Empty;
public string primarySpeciesId = string.Empty;
public string bycatchSpeciesId = string.Empty;
public float bycatchYield;
public bool bycatchToxic;
public int day;
public string hunterId = string.Empty;
public static class WildlifeTrappingEventStatus
public const string Pending = "pending";
public const string Delivered = "delivered";
public static class WildlifeTrappingEventKinds
public const string MoralConsequence = "moral_consequence";
public const string TrapEncounter = "trap_encounter";
public const string TrappingBroadcast = "trapping_broadcast";
public const string NarrativeIncident = "narrative_incident";
public static class TrapEncounterIds
public const string BaitStolen = "enc_trap_bait_stolen";
public const string Tampered = "enc_trap_tampered";
public const string StrangerDiscovery = "enc_trap_stranger_discovery";
public static class TrapNarrativeIncidentIds
public const string SprungBloodTrail = "trap_sprung_blood_trail";
public const string BaitStolen = "trap_bait_stolen";
public const string HumanBootprints = "trap_human_bootprints";
public static readonly string[] Ordered = {
public static class TrappingBroadcastIds
public const string FirstCatch = "radio_wildlife_first_catch";
public const string TrapBroken = "radio_wildlife_trap_broken";
public const string RareBycatch = "radio_wildlife_rare_bycatch";
public static class TrappingMoralTier
public const string HighQuestId = "quest_moral_trap_prey_high";
public const string MediumQuestId = "quest_moral_trap_prey_medium";
public const string LowQuestId = "quest_moral_trap_prey_low";
public const float HighThreshold = 0.75f;
public const float MediumThreshold = 0.35f;
public static string ResolveQuestId(float moralWeight) {
public sealed class WildlifeTrappingPendingEvent
public string eventId = string.Empty;
public int sequence;
public string kind = string.Empty; // WildlifeTrappingEventKinds
public string sourceTrapSiteId = string.Empty;
public string survivorId = string.Empty;
public string speciesId = string.Empty;
public string payloadId = string.Empty; // quest/encounter/broadcast content ID
public float weight;
public int day;
public string status = WildlifeTrappingEventStatus.Pending;
```


# Appendix Q.568 — Additional Current Architecture Evidence: `src/Host/BestiarySelfTest.cs`

### `src/Host/BestiarySelfTest.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 116 lines / 6161 bytes.
- SHA-256: `6fa7a19b6c41eed7f3d69107ca0a2e27b26a40bbb872a5144476e3b2d62d0dd8`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class BestiarySelfTest
public static int Run(string dataDir) {
```


# Appendix Q.569 — Additional Current Architecture Evidence: `src/Host/EcologicalInfestationSaveStore.cs`

### `src/Host/EcologicalInfestationSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 45 lines / 2129 bytes.
- SHA-256: `e530af066417b654e385b0d61e5063eb33b771ce57de3074c2565e1933e5ce56`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class EcologicalInfestationSaveStore
public const string FileName = "ecological_infestation_save.json";
public const string SectionName = "ecological_infestation";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static bool TrySave(EcologicalInfestationState state) => s_store.TrySave(state);
public static EcologicalInfestationState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(EcologicalInfestationState state) => s_store.CapturePersisted(state);
```


# Appendix Q.570 — Additional Current Architecture Evidence: `src/Host/WildlifeEcosystemSaveStore.cs`

### `src/Host/WildlifeEcosystemSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 33 lines / 1551 bytes.
- SHA-256: `7bb966070309ac572faed48d05aaada45e7c3d3fada5153061d5f06a1281659b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class WildlifeEcosystemSaveStore
public const string FileName = "wildlife_ecosystem_save.json";
public const string SectionName = "wildlife_ecosystem";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static bool TrySave(WildlifeEcosystemState state) => s_store.TrySave(state);
public static WildlifeEcosystemState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(WildlifeEcosystemState state) => s_store.CapturePersisted(state);
```


# Appendix Q.571 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Narrative/WastelandBestiaryCatalog.cs`

### `Assets/Ashfall.Core/Narrative/WastelandBestiaryCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 114 lines / 3836 bytes.
- SHA-256: `5aa25d28bdd26211a129c959ca304373f7e451b4ec586121744b6284af9e7492`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WastelandCreatureEntry
public string creature_id;
public string common_name;
public string colloquial_name;
public string primary_habitat;
public int threat_level;
public string pack_size_range;
public float acoustic_lure_frequency_hz;
public string[] harvestable_materials;
public float butchered_meat_calories;
public string harlan_scout_notes;
public string[] tags;
public sealed class WastelandBestiaryFile
public int schema_version;
public string collection_id;
public List<WastelandCreatureEntry> creatures = new List<WastelandCreatureEntry>();
public sealed class WastelandBestiaryCatalog
public IReadOnlyList<WastelandCreatureEntry> AllCreatures => _allCreatures;
public void Load(string json, IJsonSerializer serializer) {
public WastelandCreatureEntry? GetById(string creatureId) {
public List<WastelandCreatureEntry> GetByThreatLevel(int threatLevel) {
public List<WastelandCreatureEntry> GetGameYields(float minCalories = 1000.0f) {
public List<WastelandCreatureEntry> GetByTag(string tag) {
```


# Appendix Q.572 — Additional Current Architecture Evidence: `src/Main.CampaignOwners.cs`

### `src/Main.CampaignOwners.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 2957 lines / 146936 bytes.
- SHA-256: `6c612e267459f02941f6c11c6536eaba89417aff5cf2a99aa28350aba04b7930`.
- Architecture signals: seeded references=0; save/restore symbols=121; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* retention is idempotent; captured via save section */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* derived read projection */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) => _m._commitments?.System.CapturePreDaySnapshot(day);
public void RestorePreDaySnapshot(int day) => _m._commitments?.System.RestorePreDaySnapshot(day);
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* no snapshot: campaign days are day-local */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* jobs are day-local; capture via save section */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* no snapshot: hazards are day-local */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
```


# Appendix Q.573 — Additional Current Architecture Evidence: `src/Host/WildlifeEcosystemHostSession.cs`

### `src/Host/WildlifeEcosystemHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 52 lines / 2014 bytes.
- SHA-256: `15a381e4bf286d3f564e154091d1d1ea48a028ac5290ed7447f72fd60ea9dfa2`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WildlifeEcosystemHostSession : HostSessionBase
public WildlifeEcosystemSystem System { get; }
public string LastEvent { get; private set; } = string.Empty;
public void MarkDirty(string reason) {
public override void Save() {
```


# Appendix Q.574 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Localization/WildlifeTrappingLocalization.cs`

### `Assets/Ashfall.Core/Localization/WildlifeTrappingLocalization.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 33 lines / 1701 bytes.
- SHA-256: `1b9c2b80e02d7bc2200b46d87985be131d521765686231462c2642687c1bed7d`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class WildlifeTrappingLocalization
public const string FirstSnareTutorialId = "wildlife.trapping.first_snare";
public const string WearOutTutorialId = "wildlife.trapping.wear_out";
public const string BycatchTutorialId = "wildlife.trapping.bycatch";
public static string TrapNameKey(string trapId) => Key("trap", trapId, "name");
public static string TrapDescriptionKey(string trapId) => Key("trap", trapId, "description");
public static string PreyNameKey(string speciesId) => Key("prey", speciesId, "name");
public static string PreyDescriptionKey(string speciesId) => Key("prey", speciesId, "description");
public static string BaitNameKey(string baitId) => Key("bait", baitId, "name");
public static string TutorialTitleKey(string tutorialId) => $"{tutorialId}.title";
public static string TutorialBodyKey(string tutorialId) => $"{tutorialId}.body";
```


# Appendix Q.575 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`

### `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 727 lines / 75985 bytes.
- SHA-256: `f6c7527c4c8d3a554a58ae7690d54f773a331203ddbb26b65b6c05377616ee74`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public record SaveSectionMetadata(
public static class SaveSectionRegistry
public const string ExpandedShelterLifecycleGroup = "expanded_shelter";
public static readonly IReadOnlyDictionary<string, string> LifecycleSectionAliases = new Dictionary<string, string>(StringComparer.Ordinal) {
public static readonly IReadOnlyList<SaveSectionMetadata> All = new List<SaveSectionMetadata> {
public static readonly IReadOnlyDictionary<string, string> SectionFileNames = new Dictionary<string, string>(StringComparer.Ordinal) {
public static readonly IReadOnlyDictionary<string, int> SchemaVersions = new Dictionary<string, int>(StringComparer.Ordinal) {
public static string? CanonicalizeSectionKey(string? sectionKey) {
public static IReadOnlyList<string> SectionKeysForLifecycleGroup(string lifecycleGroup) {
public static bool IsLifecycleGroup(string lifecycleGroup) =>
public static IReadOnlyCollection<string> LifecycleGroupKeys => SectionsByLifecycleGroup.Keys.ToArray();
public static string? FileNameFor(string sectionKey) {
public static int SchemaVersionFor(string sectionKey) {
public static bool TryGetKeyForSectionName(string sectionName, out string? sectionKey) {
public static bool TryGetSection(string sectionKey, out SaveSectionMetadata? metadata) {
public static IReadOnlyList<string> SectionKeys => All.Select(s => s.SectionKey).ToList();
```


# Appendix Q.576 — Additional Current Architecture Evidence: `src/Main.ExpandedShelterSystems.cs`

### `src/Main.ExpandedShelterSystems.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 898 lines / 42067 bytes.
- SHA-256: `a6f038a1dc347c2ab4b861767374e75aa2a79d980a2e6a7b71396061978c3da2`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public void OpenExpandedPanel(string panelKey) {
```


# Appendix Q.577 — Additional Current Architecture Evidence: `src/Main.EvolvingWorld.cs`

### `src/Main.EvolvingWorld.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 202 lines / 9353 bytes.
- SHA-256: `fb3213dda76ce4eeeb14d3b45b0aef876de7309cb1f523f6041867b76826f61a`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
internal static float AshfallMmFor(WeatherKind kind) => kind switch
```


# Appendix Q.578 — Additional Current Architecture Evidence: `src/Host/HostCli.EvolvingWorld.cs`

### `src/Host/HostCli.EvolvingWorld.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 326 lines / 17962 bytes.
- SHA-256: `bba8c22c26e75333d0c850b2eb5252b73781d1d0cae72ccc9b58382f64b5cefc`.
- Architecture signals: seeded references=4; save/restore symbols=19; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunEvolvingWorldSelfTest(string dataDirectory) {
```


# Appendix Q.579 — Additional Current Architecture Evidence: `src/Host/HostCli.Plans162_165.cs`

### `src/Host/HostCli.Plans162_165.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 489 lines / 26252 bytes.
- SHA-256: `50eed813c71e4acdc008e32fac4d07a404b9782e05500e6f04d75e9b90820bd2`.
- Architecture signals: seeded references=22; save/restore symbols=9; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunAgricultureSelfTest(string dataDirectory) {
public static int RunDefenseSelfTest(string dataDirectory) {
public static int RunPsychologySelfTest(string dataDirectory) {
public static int RunWildlifeSelfTest(string dataDirectory) {
public static int RunTrappingHostSelfTest(string dataDirectory) {
```


# Appendix Q.580 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`

### `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 3459 lines / 190529 bytes.
- SHA-256: `d79f9fa53bb0e6eed315b2dbb58e4c2a2f32991272ddd44200da26afa1458719`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CatalogIntegrityReport
public readonly List<string> Errors = new List<string>();
public readonly List<string> Warnings = new List<string>();
public int ErrorCount => Errors.Count;
public bool Clean => Errors.Count == 0;
public int AuthoredIds;
public int ReuseCount;
public void Error(string message) {
public void Warn(string message) {
public static class CatalogIntegrityValidator
public static readonly string[] IdPrefixes = {
public static readonly string[] DefinitionKeys = {
public static readonly string[] ReferenceKeys = {
public static readonly string[] RangeKeys = { "minDay", "maxDay", "MinDay", "min_day" };
public static readonly string[] VocabularyKeys = {
public static readonly string[] KnownRuntimeIds = {
public static readonly string[] PrefixPatternKeys = {
public readonly Dictionary<string, List<string>> Registry = new Dictionary<string, List<string>>(StringComparer.Ordinal);
public readonly List<Ref> PendingRefs = new List<Ref>();
public readonly Dictionary<string, RangeMemoEntry> RangeMemo = new Dictionary<string, RangeMemoEntry>(StringComparer.Ordinal);
public CatalogIntegrityReport Report;
public string File;
public int Authored;
public int Reuse;
public string Value;
public string Path;
public bool Strict;
public string? EntityContext;
public static CatalogIntegrityReport Validate(string dataDirectory, IFileIO files) => Validate(dataDirectory, files, SearchOption.TopDirectoryOnly);
public static CatalogIntegrityReport Validate(string dataDirectory, IFileIO files, SearchOption searchOption) {
public static void ValidateNightWatchOperationsCatalog( string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateShelterOperationsCatalogs( string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateDifficultyPresetCatalog( string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateVehicleArmorGradeCatalog( string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public int? Min;
public int? Max;
public static void ValidateDistressSignalStages(string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateTradeEmbargoRules(string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateRegionalPriceAtlas(string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateCommitments(string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateWildlifeTrappingCatalog(string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
```


# Appendix Q.581 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

### `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 2073 lines / 130366 bytes.
- SHA-256: `7588dbb7ed053936964371ce06c49160f772cb9fffd2e7d519884ab430f044c4`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ContentUtilizationScanner
public static bool IsNarrativeSubdirectoryFile(string relativePath) {
public static bool IsAuthoritativeCatalog(string fileName) {
public ContentUtilizationGraph Scan() {
```


# Appendix Q.582 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/HostCliRegistry.cs`

### `Assets/Ashfall.Core/HostCliRegistry.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1912 lines / 102959 bytes.
- SHA-256: `827182dd993f0bff556c0f2e1ba84448391b5b2b728a0d2242780da4594d9940`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=1; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum HostCliAction
public sealed class HostCliActionDescriptor
public HostCliAction Action { get; }
public string Category { get; }
public string PrimaryFlag { get; }
public IReadOnlyList<string> Aliases { get; }
public string Description { get; }
public string ValuePlaceholder { get; }
public IReadOnlyList<string> AllFlags { get; }
public bool IsSelfTest { get; }
public bool IsTest { get; }
public bool HeadlessCompatible { get; }
public string TestId { get; }
public string FormatHelpLine() {
public static class HostCliRegistry
public static readonly IReadOnlyList<string> Categories = new ReadOnlyCollection<string>(new[] {
public static IReadOnlyList<HostCliActionDescriptor> AllDescriptors => _descriptors;
public static IReadOnlyDictionary<string, HostCliActionDescriptor> FlagMap => _flagMap;
public static IReadOnlyList<HostCliActionDescriptor> CoreDescriptors => _coreDescriptors;
public static IReadOnlyList<HostCliActionDescriptor> ExpansionDescriptors => _expansionDescriptors;
public static IReadOnlyList<HostCliActionDescriptor> HostDomainDescriptors => _hostDomainDescriptors;
public static IReadOnlyList<HostCliActionDescriptor> UiDescriptors => _uiDescriptors;
public static IReadOnlyList<HostCliActionDescriptor> ConfigDescriptors => _configDescriptors;
public static IReadOnlyList<HostCliActionDescriptor> InfoDescriptors => _infoDescriptors;
public static IReadOnlyDictionary<string, HostCliActionDescriptor> ValidateFlagRegistry() {
public static IReadOnlyDictionary<string, HostCliActionDescriptor> ValidateDescriptors(IEnumerable<HostCliActionDescriptor> descriptors) {
public static HostCliAction Resolve(string[]? args) {
public static void PrintHelp(Action<string> print) {
public static void PrintSelfTests(Action<string> print) {
public static HostSelfTestManifest CreateSelfTestManifest() {
public static string GenerateJsonManifest() {
public static string GenerateMarkdownCatalog(string verifiedDate) {
public sealed class HostSelfTestManifest
public string SchemaVersion { get; set; } = "1.0.0";
public string Description { get; set; } = "";
public int TotalTests { get; set; }
public int HeadlessTestCount { get; set; }
public List<HostSelfTestItem> Tests { get; set; } = new List<HostSelfTestItem>();
public sealed class HostSelfTestItem
public string TestId { get; set; } = "";
public string Action { get; set; } = "";
public string Category { get; set; } = "";
public string PrimaryFlag { get; set; } = "";
public string[] Aliases { get; set; } = Array.Empty<string>();
public string Description { get; set; } = "";
public bool HeadlessCompatible { get; set; }
public string ExpectedSummaryId { get; set; } = "";
public int TimeoutSeconds { get; set; } = 30;
```


# Appendix Q.583 — Additional Current Architecture Evidence: `src/Host/PanelBindLifecycleSelfTest.cs`

### `src/Host/PanelBindLifecycleSelfTest.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1371 lines / 70877 bytes.
- SHA-256: `a96e666a51d3760bb8e972a3acf6ab4fed7a74328402409cdbe372d614a9fe1c`.
- Architecture signals: seeded references=3; save/restore symbols=0; typed event declarations=0; textual Godot mentions=5; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=1; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class PanelBindLifecycleSelfTest
public static int Run(string dataDirectory = "") {
```


# Appendix Q.584 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`

### `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 321 lines / 32201 bytes.
- SHA-256: `4cacf42780f11dbe1a7ff0b8495bd9a8c5b97f0ae99fda5bcf7ca3e1751df5e7`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class PanelRegistryBootstrap
public static void RegisterAll() {
```


# Appendix Q.585 — Additional Current Architecture Evidence: `src/Host/HostCli.WorldPlaytest.cs`

### `src/Host/HostCli.WorldPlaytest.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1008 lines / 56167 bytes.
- SHA-256: `96d58936693cbbb9fc62a949e176a97480f7ce3654f83151cec2af518576e3c9`.
- Architecture signals: seeded references=8; save/restore symbols=26; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunWorldPlaytestSelfTest(string dataDirectory, Main? productionMain = null) {
public string id = string.Empty;
public bool passed;
public string evidence = string.Empty;
public int schema_version = 1;
public string source_commit = string.Empty;
public string day_owner = "world_evolution";
public int master_seed;
public int snapshot_count;
public List<WorldPlaytestBand> target_bands = new List<WorldPlaytestBand>();
public int first_migration_day = -1;
public int first_degradation_transition_day = -1;
public float wildlife_density_min;
public float wildlife_density_max;
public float wildlife_density_delta;
public float encounter_multiplier_min;
public float encounter_multiplier_max;
public float canned_food_scarcity_min;
public float canned_food_scarcity_max;
public int briefing_event_count;
public int journal_event_count;
public int radio_event_count;
public int trapping_density_reads;
public int encounter_multiplier_reads;
public int dead_seed_count;
public bool same_seed_byte_equal;
public bool midpoint_save_load_byte_equal;
public bool different_seed_diverged;
public bool duplicate_narrative_events_after_restore;
public List<WorldPlaytestInfluenceRow> influence_map = new List<WorldPlaytestInfluenceRow>();
public List<WorldPlaytestCheck> checks = new List<WorldPlaytestCheck>();
public List<WorldPlaytestSnapshot> snapshots = new List<WorldPlaytestSnapshot>();
public string tuning_decision = string.Empty;
public string measure = string.Empty;
public string band = string.Empty;
public string producer = string.Empty;
public string value = string.Empty;
public string consumer = string.Empty;
public string read_cadence = string.Empty;
public string player_visible_consequence = string.Empty;
public int day;
public float global_wildlife_ratio;
public float total_wildlife_density;
public List<WorldPlaytestLocation> locations = new List<WorldPlaytestLocation>();
public List<WorldPlaytestWildlife> wildlife = new List<WorldPlaytestWildlife>();
public WorldPlaytestEconomy economy = new WorldPlaytestEconomy();
public List<WorldPlaytestSurfaceEvent> surfaced_events = new List<WorldPlaytestSurfaceEvent>();
public string location_id = string.Empty;
public string owner = string.Empty;
public string degradation_tier = string.Empty;
public float contamination;
public float loot_depletion;
public float encounter_multiplier;
public string pack_id = string.Empty;
public string species_id = string.Empty;
public string region_id = string.Empty;
public int density;
public float starvation;
public bool rabid;
public float canned_food_scarcity_proxy;
public List<WorldPlaytestGood> scarcity_goods = new List<WorldPlaytestGood>();
public string item_id = string.Empty;
public float demand_multiplier;
public string channel = string.Empty;
public string kind = string.Empty;
public string primary_id = string.Empty;
public string secondary_id = string.Empty;
public float numeric;
public string expedition_id = string.Empty;
public string location_id = string.Empty;
public int phase;
public int schema_version = 1;
public WorldWeatherState weather = new WorldWeatherState();
public LocationEvolutionSaveState locations = new LocationEvolutionSaveState();
public WildlifeSaveState wildlife = new WildlifeSaveState();
public LandmarkSaveState landmarks = new LandmarkSaveState();
public MarketState market = new MarketState();
public WildlifeTrappingState trapping = new WildlifeTrappingState();
public List<ExpeditionState> expeditions = new List<ExpeditionState>();
public int expedition_completed_count;
public CampaignDaySave coordinator = new CampaignDaySave();
public List<string> previous_sectors = new List<string>();
public List<string> surfaced_event_keys = new List<string>();
public int last_trapping_catch;
public WeatherSystem Weather { get; }
public LocationEvolutionSystem Locations { get; }
public WildlifeMigrationSystem Wildlife { get; }
public LandmarkDegradationSystem Landmarks { get; }
public MarketSystem Market { get; }
public WildlifeTrappingSystem Trapping { get; }
public ExpeditionSystem Expeditions { get; }
public CampaignDayCoordinator Coordinator { get; }
public List<WorldPlaytestSnapshot> Snapshots { get; } = new List<WorldPlaytestSnapshot>();
public int Migrations { get; private set; }
public int DegradationTransitions { get; private set; }
public int TrappingDayTicks { get; private set; }
public int TrappingDensityReads { get; private set; }
public int EncounterMultiplierReads { get; private set; }
public int EncounterEvaluationTicks { get; private set; }
public int SurfacedBriefingEvents { get; private set; }
public int SurfacedJournalEvents { get; private set; }
public int SurfacedRadioEvents { get; private set; }
public bool NoDuplicateNarrativeEvents { get; private set; } = true;
public int DeadSeedCount { get; private set; }
public static WorldPlaytestRun Create(string dataDirectory, int seed) {
public void AdvanceThrough(int firstDay, int lastDay) {
public WorldPlaytestSave CaptureSave() {
public void RestoreSave(WorldPlaytestSave save) {
public float EncounterMultiplierMovement => Snapshots.Count == 0
public float ScarcityMovement => Snapshots.Count < 2
public bool AllSnapshotsSane() {
public bool NoDuplicateSnapshotRecords() {
public bool RuinedStatesAreSticky() {
public WorldPlaytestArtifact BuildArtifact(string dataDirectory, List<WorldPlaytestCheck> checks, bool sameSeedEqual, bool midpointEqual, bool differentSeedDiverged) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
```


# Appendix Q.586 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/CatalogIntegrityRules.cs`

### `Assets/Ashfall.Core/CatalogIntegrityRules.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 389 lines / 27704 bytes.
- SHA-256: `95fdf21d1303b18783040dc9fd098f60b9b52584e8ee88ff3de6e1522f084983`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class CatalogIntegrityRules
public static readonly string[] IdPrefixes = {
public static readonly string[] DefinitionKeys = {
public static readonly string[] ReferenceKeys = {
public static readonly string[] RangeKeys = { "minDay", "maxDay", "MinDay", "min_day" };
public static readonly string[] VocabularyKeys = {
public static readonly string[] KnownRuntimeIds = {
public static bool IsVocabularyKey(string key) =>
public static bool IsKnownRuntimeId(string value) =>
public static bool IsDefinitionKey(string key) =>
public static bool IsReferenceKey(string key) =>
public static bool IsRangeKey(string key) =>
public static bool StartsWithAnyPrefix(string value) =>
public static bool StartsWithAny(string value, string[] prefixes) {
```


# Appendix Q.587 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Performance/Workloads/PerformanceCampaignHarness.cs`

### `Assets/Ashfall.Core/Performance/Workloads/PerformanceCampaignHarness.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 403 lines / 14637 bytes.
- SHA-256: `364d311490e3a73f9e9d2d31861f78f70ecb80a4615f9b48ade364cb2330e7a8`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class PerformanceCampaignHarness : IDisposable
public CampaignDayCoordinator Coordinator { get; }
public SurvivorRosterSystem Survivors { get; }
public Inventory.Inventory Inventory { get; }
public JournalSystem Journal { get; }
public WeatherSystem Weather { get; }
public ExpeditionSystem Expeditions { get; }
public LocationEvolutionSystem LocationEvolution { get; }
public WildlifeMigrationSystem Wildlife { get; }
public LandmarkDegradationSystem Landmark { get; }
public int CurrentDay => Coordinator.Calendar is Ashfall.Core.Clock.ISimClock simClock ? simClock.DayIndex : Coordinator.LastAdvancedDay;
public ISeededRng Rng { get; }
public double AdvanceDays(int days) {
public string CaptureSavePayload() {
public double MeasureSaveLatency() {
public double MeasureLoadLatency(string payload) {
public static double MeasureChecksumLatency(string payload) {
public long MeasureRetainedMemoryAfterNewGame() {
public void Dispose() {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public string Id => "perf_author";
public string DisplayName => "Perf";
public Ashfall.Core.Journal.RiskBiasTrait RiskBias => Ashfall.Core.Journal.RiskBiasTrait.Realist;
```


# Appendix Q.588 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/World/WildlifeHarvestQuotaEngine.cs`

### `Assets/Ashfall.Core/World/WildlifeHarvestQuotaEngine.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 268 lines / 12524 bytes.
- SHA-256: `a063daff72a8bbe28dbe9fdedbd6e2535a928954f3dd89b2505b4ac6df3a8301`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum SpeciesPopulationBand
public enum PredatorConflictPosture
public readonly struct HarvestQuotaResult
public int MaxSafeHarvestUnits     { get; }
public SpeciesPopulationBand PostHarvestBand { get; }
public bool IsWithinQuota          { get; }
public int OverhuntCollapseRiskPermille { get; }
public readonly struct TamingReadinessResult
public int ReadinessPermille       { get; }
public bool IsTameable             { get; }
public int EstimatedSessionsNeeded { get; }
public static class WildlifeHarvestQuotaEngine
public const int CriticalPopulationThreshold = 100;  // 10%
public const int ConflictProximityMetres = 300;
public const int TamingReadinessThreshold = 700;
public static HarvestQuotaResult EvaluateHarvestQuota( int currentPopulationPermille, int seasonalReproductionPermille, int requestedHarvestUnits) {
public static PredatorConflictPosture EvaluatePredatorConflict( int predatorPopulationPermille, int proximityMetres, int shelterNoisePermille) {
public static TamingReadinessResult EvaluateTamingReadiness( int animalHungerPermille, int trustExposurePermille, int speciesTamabilityPermille, int tamingSeed) {
```


# Appendix Q.589 — Additional Current Architecture Evidence: `src/Host/HostCli.cs`

### `src/Host/HostCli.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1130 lines / 85981 bytes.
- SHA-256: `f83b99e37e991491c5a219476b27a93aa2fdb6ba530de12ba746cc75bfea81c5`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=6; textual Unity/JsonUtility mentions=1; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum HostCliAction
public static partial class HostCli
public static string? ExtractArgValue(string[]? args, string flag) {
public static void ConfigureHostEnvironment(string[]? args) {
public static HostCliAction Parse(string[] args) {
public static void PrintHelp() {
public static void PrintVersion(string dataDir) {
```


# Appendix Q.590 — Additional Current Architecture Evidence: `src/Main.MoralChoice.cs`

### `src/Main.MoralChoice.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 344 lines / 16147 bytes.
- SHA-256: `e12534c6d2a8cd86c90d7283de173c51bf07cd68e46bb52f6129bbeb05375b8d`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=4; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public MoralChoiceSystem MoralChoice => _moralChoice;
public IReadOnlyList<MoralChoiceQuestDefinition> MoralChoiceDefs => _moralChoiceDefs;
public MoralChoiceQuestDefinition? GetMoralChoiceDef(string questId) {
public List<MoralChoiceQuestDefinition> GetAvailableMoralChoices() {
public const string TrappingMoralQuestIdPrefix = "quest_moral_trap_prey_";
public List<MoralChoiceQuestDefinition> GetResolvedMoralChoices() {
public MoralChoiceResolution? GetMoralChoiceResolution(string questId) {
public IReadOnlyList<MoralChoiceQuestDefinition> GetDailyMoralOffers(int maxOffers = 1) {
public bool TryResolveMoralChoice(string questId, int choiceIndex) {
```


# Appendix Q.591 — Additional Current Architecture Evidence: `src/Main.SaveOrchestrator.cs`

### `src/Main.SaveOrchestrator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 673 lines / 25810 bytes.
- SHA-256: `9f8558388d9ab84652b8d8880ec1dc06e52cb6239b895770b65903f3f2aa44ad`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=9; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
internal bool CaptureSection(string sectionKey, string payload) {
internal void FlushDirtyStoresForDayAdvance() {
public bool TryLoadAndRestoreGame(SaveSlotId slotId, out string message) {
```


# Appendix Q.592 — Additional Current Architecture Evidence: `src/Main.UiPanels.cs`

### `src/Main.UiPanels.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1755 lines / 86737 bytes.
- SHA-256: `4c6b58cef5ed68a8ccfa1e2a51ca6da94a201322a745aa4c3b7993fc385782a1`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public Ashfall.Core.Feedback.IFeedbackService FeedbackService => _feedbackService;
public FeedbackPanel FeedbackPanel => _feedbackPanel;
public ConfirmationModal ConfirmationModal => _confirmationModal;
public void PromptConfirmation(string title, string message, Action onConfirm, Action? onCancel = null) {
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.
