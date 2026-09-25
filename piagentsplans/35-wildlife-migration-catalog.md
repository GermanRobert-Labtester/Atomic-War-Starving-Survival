# Plan 35 — Wildlife Migration, Ecology Data and Single-Population Authority

> **Rebuild status:** PARTIAL DATA/OWNER AUDIT — ECOLOGY CONTENT IS LIVE; DEDICATED MIGRATION CATALOG REMAINS UNPROVEN
>
> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-3`
>
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round3-2026-09-25`
>
> **Current-evidence date:** 2026-09-25
>
> **Authority order:** live source and data → `AGENTS.md` → `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` → plan ledgers → this document.
>
> **Authority checksum:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
>
> **Length policy:** 150k–170k is the first quality checkpoint; 250k is an evidence-backed depth target, not a ceiling. The plan may exceed 250k when verified architecture and current evidence justify it, and it must stop rather than pad when that evidence is exhausted.

## 0. Integrity Statement and Plan Status

This file replaces unclaimed generated sections that mixed current evidence, fictional APIs, and unsupported save claims. It is a planning and architecture artifact only. It authorizes no production, data, test, save, generated-index, or UI edits. Every path labeled current must exist at rebuild time. Any future `CREATE` proposal is explicitly hypothetical and belongs to a later, separately claimed implementation package.

The rebuild follows four passes: content/current-reality first; integration framework second; accuracy and contradiction removal third; independent precision and handoff review fourth. Character count is recorded by external verification, not embedded recursively in the document.

# 1. Objective

- The original plan’s premise that migration state had no data is partly stale and partly still true in a narrower form: there is no dedicated `wildlife_migration.json`, but current `wildlife_ecosystem.json` carries species, predator/prey and seasonal movement data consumed by `WildlifeEcosystemSystem`.
- The live population authority is `WildlifeMigrationSystem` packs. `WildlifeEcosystemSystem` derives density/pressure/extinction/taming/knowledge over those packs and must never keep a second population store.
- The safe leap-forward plan is a seam audit: map current ecology rows, migration movement, map adjacency, season profile, hazard avoidance, trapping and bestiary consumers; then choose data-only maintenance, loader extension or a bounded new catalog with no parallel state.

**Bounded outcome:** Do not blindly create the historical `wildlife_migration.json` or 12 migration rows. Current live evidence already has `WildlifeMigrationSystem`, a 13-species/5-edge/2-season-move `wildlife_ecosystem.json`, map adjacency overlays, seasonal binding and a separate ecology state owner. First decide whether the existing ecosystem catalog is the intended migration authority; only add a dedicated catalog if a current loader/consumer gap is proven.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `wildlife_ecosystem.json` is valid schema version 1 with 13 species, 5 predator/prey edges and 2 seasonal moves; no `wildlife_migration.json` exists at the data root.
- `WildlifeMigrationSystem` owns pack records, seeded baseline, sector movement, hunger/starvation, seasonal factors, blocked/water sectors and deterministic migration RNG; its state is restored through the world path.
- `WildlifeEcosystemSystem` loads the ecology catalog and derives density, predation/radiation pressure, local extinction, apex activity, observations and taming over the migration population source.
- `WorldHostSession` binds the season profile and evolving-world seeds, overlays the map graph, and `Main.Plans162_165` supplies separate seeded population/migration/apex/taming streams and binds the Bestiary panel.

**Master-authority sections applied to this rebase:**

- Part II Factory Protocol: premise sweep, collision check, one lane/cluster, and evidence labels before drafting.
- Part II Step 5 continuity and anti-duplication checklist: data presence is not reachability.
- Part III cluster map: use the live C1–C17 owner map rather than a historical plan title.
- Part IV backlog discipline: consume a verified candidate or record why it is stale; do not widen a bounded outcome.
- Part V Template S/R: subject intent and recommended route remain separate from implementation commitments.
- Part VI Multi-Session Growth Protocol: 250k is a depth target, not permission to manufacture volume.
- Live source/data authority: current catalog, loader, host, save, and focused tests outrank generated prose.
- Anti-padding rule: if the evidence queue is exhausted, stop and report no warranted continuation.
- C14 Ecology/wildlife cluster: migration population, ecology pressure, trapping and bestiary knowledge remain separate layers.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the missing-file/12-row premise with a current ecology-vs-migration authority census.
- Trace every species/edge/seasonal row to a loader, migration/ecosystem method, host tick, save owner and player-visible projection.
- Decide whether seasonal movement belongs in the existing ecology catalog or requires a new dedicated catalog with a proven loader; do not create both.
- Preserve the single population source, remnant-floor/extinction semantics, seeded stream boundaries and map adjacency overlays.

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
| pack population, movement and migration state | WildlifeMigrationSystem | `Assets/Ashfall.Core/WildlifeMigrationSystem.cs; Assets/Ashfall.Core/WildlifeMigrationSystem.Live.cs` | Sole pack/population authority; accepts map/season providers and seeded streams. |
| ecology pressure, density, extinction, apex and taming projection | WildlifeEcosystemSystem | `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` | Derives over migration packs and owns its separate ecology state. |
| species/edge/season definitions | Wildlife ecology catalog | `Assets/StreamingAssets/Data/wildlife_ecosystem.json` | Current authored data authority; no dedicated migration file is assumed. |
| season/map/seed graph composition | WorldHostSession | `src/Host/WorldHostSession.cs` | Binds current world providers and overlays map adjacency. |
| daily ecology tick and Bestiary host route | Main.Plans162_165 | `src/Main.Plans162_165.cs; src/UI/BestiaryPanel.cs` | Supplies named RNG streams and presentation; not a second ecology owner. |
| migration/ecosystem/trapping/consumer proof | Ecology focused tests | `Ashfall.Core.Tests/WildlifeEcosystemSystemTests.cs; Ashfall.Core.Tests/WildlifeSeasonalCalendarTests.cs; Ashfall.Core.Tests/World/Plan176CrossSystemConsumerTests.cs` | Focused evidence surface. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Wildlife Migration, Ecology Data and Single-Population Authority
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ WildlifeMigrationSystem
│   pack population, movement and migration state
│ WildlifeEcosystemSystem
│   ecology pressure, density, extinction, apex and taming projection
│ Wildlife ecology catalog
│   species/edge/season definitions
│ WorldHostSession
│   season/map/seed graph composition
│ Main.Plans162_165
│   daily ecology tick and Bestiary host route
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

1. **Preserve current state ownership.** WildlifeMigrationSystem owns pack population, movement and migration state: Sole pack/population authority; accepts map/season providers and seeded streams.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| pack population, movement and migration state | WildlifeMigrationSystem | `Assets/Ashfall.Core/WildlifeMigrationSystem.cs; Assets/Ashfall.Core/WildlifeMigrationSystem.Live.cs` | Sole pack/population authority; accepts map/season providers and seeded streams. |
| ecology pressure, density, extinction, apex and taming projection | WildlifeEcosystemSystem | `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` | Derives over migration packs and owns its separate ecology state. |
| species/edge/season definitions | Wildlife ecology catalog | `Assets/StreamingAssets/Data/wildlife_ecosystem.json` | Current authored data authority; no dedicated migration file is assumed. |
| season/map/seed graph composition | WorldHostSession | `src/Host/WorldHostSession.cs` | Binds current world providers and overlays map adjacency. |
| daily ecology tick and Bestiary host route | Main.Plans162_165 | `src/Main.Plans162_165.cs; src/UI/BestiaryPanel.cs` | Supplies named RNG streams and presentation; not a second ecology owner. |
| migration/ecosystem/trapping/consumer proof | Ecology focused tests | `Ashfall.Core.Tests/WildlifeEcosystemSystemTests.cs; Ashfall.Core.Tests/WildlifeSeasonalCalendarTests.cs; Ashfall.Core.Tests/World/Plan176CrossSystemConsumerTests.cs` | Focused evidence surface. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load current ecology catalog and evolving-world/map seeds
2. bind season profile and sector adjacency to the migration owner
3. seed or restore migration packs
4. run deterministic migration/population/apex/taming streams in campaign order
5. derive ecology pressure/density/extinction/observation facts
6. project Bestiary/map/harvest/trapping views
7. save migration and ecology owners through their existing sections

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Pack records and `seededPopulation` are the current population baseline; ecology state stores pressures, observations, extinctions, apex windows and domestic-animal facts separately.
- Migration never enters blocked sectors and can filter water-bound neighbors; map adjacency is host-supplied projection data.
- A remnant pair/floor protects local extinction while the ecology layer derives density from the same population source.
- The host must pass deterministic population/migration/apex/taming streams; no System.Random or frame-based tick is permitted.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- No ecology method may create or mutate a second population collection.
- A movement target must be an allowed, known sector and a water-bound species must respect water constraints.
- A blocked sector can repel a pack but cannot erase the pack record or fabricate a new route.
- Same seed, map graph, season, day and state produce the same movement/ecology trace.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `wildlife_ecosystem.json` is the current verified ecology catalog; a new `wildlife_migration.json` is not automatic.
- Any new movement catalog must define loader ownership, species references, sector endpoints, day windows and a current consumer before authoring.
- Reuse canonical species and sector IDs; do not invent a parallel map namespace.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use the existing world migration state and `wildlife_ecosystem` section; no new migration section without a proven owner contract.
- Migration packs and ecology state restore independently but must be captured in a deterministic order.
- Legacy saves with no seeded baseline use the current documented fallback semantics and must not double-seed.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Use `CampaignStreamIds.WildlifePopulation`, `.WildlifeMigration`, `.WildlifeApex` and `.WildlifeTaming` through the host’s seeded fork contract.
- Fallback seeded constructors are for headless tests only and must not silently replace a live campaign stream.
- Tie-breaks use ordinal pack IDs and fixed candidate ordering; hash iteration cannot choose movement.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Migration emits pack-migrated facts; ecology emits observed/extinction/apex/taming/hazard facts.
- Main journals ecology facts and refreshes Bestiary; the journal is presentation, not population authority.
- Trapping/harvest consumers read the current population/pressure projections rather than writing a second count.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/WorldHostSession.cs
- src/Host/WildlifeEcosystemHostSession.cs
- src/Main.Plans162_165.cs
- src/UI/BestiaryPanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Wildlife movement should communicate seasonal pressure and scarcity, not guarantee a hunting jackpot.
- Radiation, disease, predation and hazard avoidance must be described through current owner facts and never invented in prose.
- Keep the tone grounded and fictional; no real-world species exploitation or copied hunting guide.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A new migration catalog duplicates species/seasonal movement already in ecology data. | WildlifeMigrationSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | Population is copied into ecology state and diverges after restore. | WildlifeEcosystemSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A blocked/water constraint is bypassed by a host or panel. | Wildlife ecology catalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | Unseeded movement makes paired runs diverge. | WorldHostSession | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A Bestiary projection reveals or mutates a population the owner did not expose. | Main.Plans162_165 | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeEcosystemSystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeSeasonalCalendarTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan176CrossSystemConsumerTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — authority census | Read migration, ecology, world host, main tick, catalogs and save paths. | Current owners and the missing-file premise are separated. | No production path until the owning implementation package is separately claimed. |
| 1 — row/consumer matrix | Trace all 13 species, 5 edges and 2 moves to current consumers. | No orphan or duplicate authority is hidden. | No production path until the owning implementation package is separately claimed. |
| 2 — route decision | Choose existing ecology data, a loader extension or a separately claimed migration catalog. | The decision is evidence-backed and bounded. | No production path until the owning implementation package is separately claimed. |
| 3 — replay/rollback proof | Verify map/season/RNG/restore behavior and document the handoff. | No padding or second mutable state. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/wildlife_ecosystem.json | READ ONLY; MODIFY only for a proven consumer/content gap | Current ecology data |
| Assets/Ashfall.Core/WildlifeMigrationSystem.cs | READ ONLY | Population/migration owner |
| Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs | READ ONLY | Ecology projection owner |
| src/Main.Plans162_165.cs | READ ONLY; MODIFY only under a new host claim | Tick/consumer seam |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Creating `wildlife_migration.json` without a loader/consumer. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Copying pack population into ecology state. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Changing the seeded stream split while adding content. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Treating historical “12 patterns” as current data truth. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new wildlife manager.
- No unproven migration catalog.
- No new save section.
- No production/data/test/UI changes in this planning package.

# 23. Rollback and Recovery

- Revert the planning document.
- Any future catalog decision retains current ecology/migration save fixtures and deterministic replay tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- Current ecology census and absent dedicated migration file are explicit.
- Population/migration/ecology/host/UI boundaries are named.
- The data decision is deferred to a proof step rather than assumed.
- Focused replay and consumer tests are specified.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the missing-file/12-row premise with a current ecology-vs-migration authority census.
- Trace every species/edge/seasonal row to a loader, migration/ecosystem method, host tick, save owner and player-visible projection.
- Decide whether seasonal movement belongs in the existing ecology catalog or requires a new dedicated catalog with a proven loader; do not create both.
- Preserve the single population source, remnant-floor/extinction semantics, seeded stream boundaries and map adjacency overlays.

## MUST NOT DO

- No new wildlife manager.
- No unproven migration catalog.
- No new save section.
- No production/data/test/UI changes in this planning package.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeEcosystemSystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/WildlifeSeasonalCalendarTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan176CrossSystemConsumerTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — authority census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: pack population, movement and migration state → WildlifeMigrationSystem; ecology pressure, density, extinction, apex and taming projection → WildlifeEcosystemSystem; species/edge/season definitions → Wildlife ecology catalog; season/map/seed graph composition → WorldHostSession; daily ecology tick and Bestiary host route → Main.Plans162_165; migration/ecosystem/trapping/consumer proof → Ecology focused tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 35.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 35 does not authorize a new save section when an existing owner can carry the fact.

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


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/WildlifeSeasonalCalendar.cs`

### `Assets/Ashfall.Core/WildlifeSeasonalCalendar.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 306 lines / 16604 bytes.
- SHA-256: `9d760fd3e7c6a64c8fcd5b368284f08d8db9b6b7e4c149aba4c3f42eec92dfd2`.
- Architecture signals: seeded references=1; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum MigrationArchetype
public static class WildlifeSeasonalCalendar
public const string SeasonFirstThaw = "window_first_thaw";
public const string SeasonAshSettling = "window_ash_settling";
public const string SeasonAshfall = SeasonAshSettling;          // legacy Plan 19 name
public const string SeasonDeepFreeze = "window_deep_freeze";
public const string SeasonThaw = "window_spring_storms";        // legacy Plan 19 name (wet interval)
public const string SeasonSpringStorms = SeasonThaw;
public const string SeasonDryAsh = "window_dry_ash";
public const string SeasonFirstFallout = "window_first_fallout";
public const string SeasonFalseSpring = "window_false_spring";
public const string SeasonDeepAsh = "window_deep_ash";
public const string SeasonLongWinter = "window_long_winter";
public const string SeasonBlackRainSeason = "window_black_rain_season";
public const string SeasonBlackBloom = SeasonDryAsh;            // legacy Plan 19 name
public const string SeasonHighCold = SeasonLongWinter;          // legacy Plan 19 name
public const string SeasonTheTurning = SeasonFalseSpring;       // legacy Plan 19 name
public const float HungerFactorMin = 0.6f;
public const float HungerFactorMax = 1.5f;
public const float AbundanceFactorMin = 0.2f;
public const float AbundanceFactorMax = 1.5f;
public static MigrationArchetype ArchetypeOf(string speciesId) => speciesId switch
public static SeasonWindowDef SeasonWindowForDay(SeasonProfileDef? profile, int day) {
public static float HungerFactor(SeasonWindowDef? season, MigrationArchetype archetype) {
public static float AbundanceFactor(SeasonWindowDef? season, MigrationArchetype archetype) {
public static float SectorAbundanceFactor( SeasonProfileDef? profile, int day, string sectorId, IEnumerable<WildlifePackRecord>? packs) {
public static List<string> FilterNeighbors( MigrationArchetype archetype, string currentSectorId, List<string> neighbors, HashSet<string>? waterSectors) {
public static string? FieldGuideEntryFor(string speciesId) => speciesId switch
public static string? MigrationNotice( MigrationArchetype archetype, string speciesId, string fromSector, string toSector, int day) {
```


# Appendix B.06 — Current Code Architecture: `src/Host/WorldHostSession.cs`

### `src/Host/WorldHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 402 lines / 19415 bytes.
- SHA-256: `b932e2d0b66a2c471eb95d222294abeb1f921424e2d1b157d41c53c32c68a816`.
- Architecture signals: seeded references=1; save/restore symbols=15; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WorldHostSession
public const int DemoSeed = 1234;
public WeatherSystem Weather { get; }
public SkyLayerArmorSystem SkyArmor { get; }
public WeatherIntelligenceCoordinator WeatherIntelligence { get; }
public LocationEvolutionSystem LocationEvolution { get; }
public WildlifeMigrationSystem Wildlife { get; }
public LandmarkDegradationSystem Landmarks { get; }
public WastelandMapSystem WastelandMap { get; }
public DamagedMapSystem? DamagedMap { get; private set; }
public SeasonProfileDef Profile { get; private set; }
public WeatherGateCatalog GateCatalog { get; private set; } = new WeatherGateCatalog();
public AtmosphereTextSystem AtmosphereTexts { get; } = new AtmosphereTextSystem();
public EnvironmentalTextSystem EnvironmentalTexts { get; } = new EnvironmentalTextSystem();
public EvolvingWorldSeedContainer? Seeds { get; private set; }
public string ShelterSectorId => EvolvingWorldSeeder.ShelterSectorId(Seeds);
public string WildlifeSightingFor(string locationId) {
public string HomeSectorWildlifeStatus() {
public string LastEvent { get; private set; } = string.Empty;
public WeatherEffectsCatalog? WeatherEffects { get; private set; }
public static WorldHostSession Create(string dataDir, ICampaignRngManager? campaignRng = null) {
public string FlavorTextForLocation(string locationId, string? weather = null) {
public void TickHours(float hours) {
public string ForceDemo(WeatherKind kind) {
public string StatusLine() {
public WorldWeatherState CaptureSave() => Weather.CaptureState();
public void RestoreSave(WorldWeatherState state) => Weather.RestoreState(state);
public string SetSkyArmorDemo(int gridX, string material, float thickness) {
public string ImpactDemo(int gridX, float energyMJ) {
public string SkyArmorStatusLine() {
public SkyArmorSaveState CaptureSkyArmorSave() => SkyArmor.CaptureState();
public void RestoreSkyArmorSave(SkyArmorSaveState state) => SkyArmor.RestoreState(state);
public WeatherIntelligenceSaveState CaptureWeatherIntelligenceSave() => WeatherIntelligence.CaptureState();
public string InstallWeatherStationDemo(int day) {
public string CalibrateWeatherStationDemo(int day) {
public string ActivateOrbitalTelemetryDemo(int day) {
public string ScheduleOrbitalImpactDemo(int day, int gridX, float energyMj) {
public string WeatherIntelligenceStatusLine() {
internal static bool IsHazardWeather(WeatherKind kind) {
internal bool IsSevereWeather(WeatherKind kind) {
```


# Appendix B.07 — Current Code Architecture: `src/Host/WildlifeEcosystemHostSession.cs`

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


# Appendix B.08 — Current Code Architecture: `src/Main.Plans162_165.cs`

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


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/wildlife_ecosystem.json`

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


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/seasonal_human_migration.json`

### `Assets/StreamingAssets/Data/seasonal_human_migration.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 2368 bytes / 2368 characters.
- SHA-256: `e17f8a4316477e85a12852a7a2b8a60df336157c518afc63dc96dfcbc2a69622`.
- Root keys: `dwell_days`, `factions`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
factions: min=4, max=4, observed_paths=1
factions[].schedule: min=4, max=4, observed_paths=2
```

Representative record fields:

- `faction_id`
- `schedule`


# Appendix C.12 — Catalog Census: `Assets/StreamingAssets/Data/wildlife_trapping_catalog.json`

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


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/WildlifeEcosystemSystemTests.cs`

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


# Appendix D.14 — Existing Focused Test Inventory: `Ashfall.Core.Tests/WildlifeSeasonalCalendarTests.cs`

### `Ashfall.Core.Tests/WildlifeSeasonalCalendarTests.cs`

- Current test declarations: Fact=16, Theory=0, InlineData=0.
- File lines: 382; SHA-256: `1641c76237f923acdd9332a9b85a28a2f0736e84d9459314e48e59c8daa43485`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ArchetypeTable_CoversAllSeededSpecies_WithDistinctRoles
SeasonWindowForDay_MatchesTheWeatherAuthority
Factors_AreBounded_ForEveryArchetypeAndWindow
AbundancePeaks_AreStaggered_AcrossTheYear
FishRun_PeaksInThawAndBloom_EmptiesInDeepFreeze
HungerFactor_WinterStarvesHerds_FasterThanThaw
UnboundCalendar_IsExactlyLegacyNeutral
BoundCalendar_ChangesTrajectory_UnderIdenticalRolls
SameSeed_ProducesIdenticalSeasonalTrajectory
FishRun_NeverStandsOnDryGround
NeighborFilter_BoundsWaterRunners_WithoutStranding
SectorAbundance_IsDeterministic_AndObservationNeverMutates
MigrationNotice_IsPlausible_AndNeverExposesExactPopulation
SaveRestore_WithBoundCalendar_RoundTripsExactly
SeasonalCalendar_DoesNotAlterTheSectorGraph
SeededCatalog_KeepsWaterFlagsOnTheWaterwayPair
```


# Appendix D.15 — Existing Focused Test Inventory: `Ashfall.Core.Tests/World/Plan176CrossSystemConsumerTests.cs`

### `Ashfall.Core.Tests/World/Plan176CrossSystemConsumerTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 223; SHA-256: `f649cfc5397fcfad5488a40ae3174806ee06212f691fbfda4e8ca688107a0f70`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ExposureResolver_LegacyPath_UnchangedWithoutAnomalyProvider
ExposureResolver_Expedition_IncludesAnomalyRate
ExposureResolver_ShelterInterior_IgnoresAnomalyRate
WildlifeAvoidance_NullModifiers_NeverMigrates
WildlifeAvoidance_FullAvoidance_MigratesDeterministically
WildlifeAvoidance_Attraction_IsNoOp
WildlifeAvoidance_EventFiresExactlyOncePerMigration
WildlifeAvoidance_SplitRun_ProducesIdenticalTrack
```


# Appendix E.16 — Supporting Code Evidence: `Assets/Ashfall.Core/WildlifeMigrationSystem.cs`

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


# Appendix E.17 — Supporting Code Evidence: `Assets/Ashfall.Core/WildlifeMigrationSystem.Live.cs`

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


# Appendix E.18 — Supporting Code Evidence: `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs`

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


# Appendix E.19 — Supporting Code Evidence: `Assets/Ashfall.Core/WildlifeSeasonalCalendar.cs`

### `Assets/Ashfall.Core/WildlifeSeasonalCalendar.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 306 lines / 16604 bytes.
- SHA-256: `9d760fd3e7c6a64c8fcd5b368284f08d8db9b6b7e4c149aba4c3f42eec92dfd2`.
- Architecture signals: seeded references=1; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum MigrationArchetype
public static class WildlifeSeasonalCalendar
public const string SeasonFirstThaw = "window_first_thaw";
public const string SeasonAshSettling = "window_ash_settling";
public const string SeasonAshfall = SeasonAshSettling;          // legacy Plan 19 name
public const string SeasonDeepFreeze = "window_deep_freeze";
public const string SeasonThaw = "window_spring_storms";        // legacy Plan 19 name (wet interval)
public const string SeasonSpringStorms = SeasonThaw;
public const string SeasonDryAsh = "window_dry_ash";
public const string SeasonFirstFallout = "window_first_fallout";
public const string SeasonFalseSpring = "window_false_spring";
public const string SeasonDeepAsh = "window_deep_ash";
public const string SeasonLongWinter = "window_long_winter";
public const string SeasonBlackRainSeason = "window_black_rain_season";
public const string SeasonBlackBloom = SeasonDryAsh;            // legacy Plan 19 name
public const string SeasonHighCold = SeasonLongWinter;          // legacy Plan 19 name
public const string SeasonTheTurning = SeasonFalseSpring;       // legacy Plan 19 name
public const float HungerFactorMin = 0.6f;
public const float HungerFactorMax = 1.5f;
public const float AbundanceFactorMin = 0.2f;
public const float AbundanceFactorMax = 1.5f;
public static MigrationArchetype ArchetypeOf(string speciesId) => speciesId switch
public static SeasonWindowDef SeasonWindowForDay(SeasonProfileDef? profile, int day) {
public static float HungerFactor(SeasonWindowDef? season, MigrationArchetype archetype) {
public static float AbundanceFactor(SeasonWindowDef? season, MigrationArchetype archetype) {
public static float SectorAbundanceFactor( SeasonProfileDef? profile, int day, string sectorId, IEnumerable<WildlifePackRecord>? packs) {
public static List<string> FilterNeighbors( MigrationArchetype archetype, string currentSectorId, List<string> neighbors, HashSet<string>? waterSectors) {
public static string? FieldGuideEntryFor(string speciesId) => speciesId switch
public static string? MigrationNotice( MigrationArchetype archetype, string speciesId, string fromSector, string toSector, int day) {
```


# Appendix E.20 — Supporting Code Evidence: `src/Host/WorldHostSession.cs`

### `src/Host/WorldHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 402 lines / 19415 bytes.
- SHA-256: `b932e2d0b66a2c471eb95d222294abeb1f921424e2d1b157d41c53c32c68a816`.
- Architecture signals: seeded references=1; save/restore symbols=15; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WorldHostSession
public const int DemoSeed = 1234;
public WeatherSystem Weather { get; }
public SkyLayerArmorSystem SkyArmor { get; }
public WeatherIntelligenceCoordinator WeatherIntelligence { get; }
public LocationEvolutionSystem LocationEvolution { get; }
public WildlifeMigrationSystem Wildlife { get; }
public LandmarkDegradationSystem Landmarks { get; }
public WastelandMapSystem WastelandMap { get; }
public DamagedMapSystem? DamagedMap { get; private set; }
public SeasonProfileDef Profile { get; private set; }
public WeatherGateCatalog GateCatalog { get; private set; } = new WeatherGateCatalog();
public AtmosphereTextSystem AtmosphereTexts { get; } = new AtmosphereTextSystem();
public EnvironmentalTextSystem EnvironmentalTexts { get; } = new EnvironmentalTextSystem();
public EvolvingWorldSeedContainer? Seeds { get; private set; }
public string ShelterSectorId => EvolvingWorldSeeder.ShelterSectorId(Seeds);
public string WildlifeSightingFor(string locationId) {
public string HomeSectorWildlifeStatus() {
public string LastEvent { get; private set; } = string.Empty;
public WeatherEffectsCatalog? WeatherEffects { get; private set; }
public static WorldHostSession Create(string dataDir, ICampaignRngManager? campaignRng = null) {
public string FlavorTextForLocation(string locationId, string? weather = null) {
public void TickHours(float hours) {
public string ForceDemo(WeatherKind kind) {
public string StatusLine() {
public WorldWeatherState CaptureSave() => Weather.CaptureState();
public void RestoreSave(WorldWeatherState state) => Weather.RestoreState(state);
public string SetSkyArmorDemo(int gridX, string material, float thickness) {
public string ImpactDemo(int gridX, float energyMJ) {
public string SkyArmorStatusLine() {
public SkyArmorSaveState CaptureSkyArmorSave() => SkyArmor.CaptureState();
public void RestoreSkyArmorSave(SkyArmorSaveState state) => SkyArmor.RestoreState(state);
public WeatherIntelligenceSaveState CaptureWeatherIntelligenceSave() => WeatherIntelligence.CaptureState();
public string InstallWeatherStationDemo(int day) {
public string CalibrateWeatherStationDemo(int day) {
public string ActivateOrbitalTelemetryDemo(int day) {
public string ScheduleOrbitalImpactDemo(int day, int gridX, float energyMj) {
public string WeatherIntelligenceStatusLine() {
internal static bool IsHazardWeather(WeatherKind kind) {
internal bool IsSevereWeather(WeatherKind kind) {
```


# Appendix G.21 — Supporting Regression Evidence: `Ashfall.Core.Tests/WildlifeEcosystemSystemTests.cs`

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


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/WildlifeSeasonalCalendarTests.cs`

### `Ashfall.Core.Tests/WildlifeSeasonalCalendarTests.cs`

- Current test declarations: Fact=16, Theory=0, InlineData=0.
- File lines: 382; SHA-256: `1641c76237f923acdd9332a9b85a28a2f0736e84d9459314e48e59c8daa43485`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ArchetypeTable_CoversAllSeededSpecies_WithDistinctRoles
SeasonWindowForDay_MatchesTheWeatherAuthority
Factors_AreBounded_ForEveryArchetypeAndWindow
AbundancePeaks_AreStaggered_AcrossTheYear
FishRun_PeaksInThawAndBloom_EmptiesInDeepFreeze
HungerFactor_WinterStarvesHerds_FasterThanThaw
UnboundCalendar_IsExactlyLegacyNeutral
BoundCalendar_ChangesTrajectory_UnderIdenticalRolls
SameSeed_ProducesIdenticalSeasonalTrajectory
FishRun_NeverStandsOnDryGround
NeighborFilter_BoundsWaterRunners_WithoutStranding
SectorAbundance_IsDeterministic_AndObservationNeverMutates
MigrationNotice_IsPlausible_AndNeverExposesExactPopulation
SaveRestore_WithBoundCalendar_RoundTripsExactly
SeasonalCalendar_DoesNotAlterTheSectorGraph
SeededCatalog_KeepsWaterFlagsOnTheWaterwayPair
```


# Appendix G.23 — Supporting Regression Evidence: `Ashfall.Core.Tests/World/Plan176CrossSystemConsumerTests.cs`

### `Ashfall.Core.Tests/World/Plan176CrossSystemConsumerTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 223; SHA-256: `f649cfc5397fcfad5488a40ae3174806ee06212f691fbfda4e8ca688107a0f70`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ExposureResolver_LegacyPath_UnchangedWithoutAnomalyProvider
ExposureResolver_Expedition_IncludesAnomalyRate
ExposureResolver_ShelterInterior_IgnoresAnomalyRate
WildlifeAvoidance_NullModifiers_NeverMigrates
WildlifeAvoidance_FullAvoidance_MigratesDeterministically
WildlifeAvoidance_Attraction_IsNoOp
WildlifeAvoidance_EventFiresExactlyOncePerMigration
WildlifeAvoidance_SplitRun_ProducesIdenticalTrack
```


# Appendix H.24 — Supporting Authority Document: `docs/CURRENT_AUTHORITY.md`

### `docs/CURRENT_AUTHORITY.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 102 lines / 9950 bytes.
- SHA-256: `7dea2c12b4863bfc9a3c2ebb161ba51512ebc06475b762abd5d5d205bef47e5c`.
- Architecture signals: seeded references=1; save/restore symbols=0; typed event declarations=0; textual Godot mentions=6; textual Unity/JsonUtility mentions=2; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| pack population, movement and migration state | WildlifeMigrationSystem | ecology pressure, density, extinction, apex and taming projection | WildlifeEcosystemSystem | Owner emits/reads a typed fact; no mirror state. |
| pack population, movement and migration state | WildlifeMigrationSystem | species/edge/season definitions | Wildlife ecology catalog | Owner emits/reads a typed fact; no mirror state. |
| pack population, movement and migration state | WildlifeMigrationSystem | season/map/seed graph composition | WorldHostSession | Owner emits/reads a typed fact; no mirror state. |
| pack population, movement and migration state | WildlifeMigrationSystem | daily ecology tick and Bestiary host route | Main.Plans162_165 | Owner emits/reads a typed fact; no mirror state. |
| pack population, movement and migration state | WildlifeMigrationSystem | migration/ecosystem/trapping/consumer proof | Ecology focused tests | Owner emits/reads a typed fact; no mirror state. |
| ecology pressure, density, extinction, apex and taming projection | WildlifeEcosystemSystem | pack population, movement and migration state | WildlifeMigrationSystem | Owner emits/reads a typed fact; no mirror state. |
| ecology pressure, density, extinction, apex and taming projection | WildlifeEcosystemSystem | species/edge/season definitions | Wildlife ecology catalog | Owner emits/reads a typed fact; no mirror state. |
| ecology pressure, density, extinction, apex and taming projection | WildlifeEcosystemSystem | season/map/seed graph composition | WorldHostSession | Owner emits/reads a typed fact; no mirror state. |
| ecology pressure, density, extinction, apex and taming projection | WildlifeEcosystemSystem | daily ecology tick and Bestiary host route | Main.Plans162_165 | Owner emits/reads a typed fact; no mirror state. |
| ecology pressure, density, extinction, apex and taming projection | WildlifeEcosystemSystem | migration/ecosystem/trapping/consumer proof | Ecology focused tests | Owner emits/reads a typed fact; no mirror state. |
| species/edge/season definitions | Wildlife ecology catalog | pack population, movement and migration state | WildlifeMigrationSystem | Owner emits/reads a typed fact; no mirror state. |
| species/edge/season definitions | Wildlife ecology catalog | ecology pressure, density, extinction, apex and taming projection | WildlifeEcosystemSystem | Owner emits/reads a typed fact; no mirror state. |
| species/edge/season definitions | Wildlife ecology catalog | season/map/seed graph composition | WorldHostSession | Owner emits/reads a typed fact; no mirror state. |
| species/edge/season definitions | Wildlife ecology catalog | daily ecology tick and Bestiary host route | Main.Plans162_165 | Owner emits/reads a typed fact; no mirror state. |
| species/edge/season definitions | Wildlife ecology catalog | migration/ecosystem/trapping/consumer proof | Ecology focused tests | Owner emits/reads a typed fact; no mirror state. |
| season/map/seed graph composition | WorldHostSession | pack population, movement and migration state | WildlifeMigrationSystem | Owner emits/reads a typed fact; no mirror state. |
| season/map/seed graph composition | WorldHostSession | ecology pressure, density, extinction, apex and taming projection | WildlifeEcosystemSystem | Owner emits/reads a typed fact; no mirror state. |
| season/map/seed graph composition | WorldHostSession | species/edge/season definitions | Wildlife ecology catalog | Owner emits/reads a typed fact; no mirror state. |
| season/map/seed graph composition | WorldHostSession | daily ecology tick and Bestiary host route | Main.Plans162_165 | Owner emits/reads a typed fact; no mirror state. |
| season/map/seed graph composition | WorldHostSession | migration/ecosystem/trapping/consumer proof | Ecology focused tests | Owner emits/reads a typed fact; no mirror state. |
| daily ecology tick and Bestiary host route | Main.Plans162_165 | pack population, movement and migration state | WildlifeMigrationSystem | Owner emits/reads a typed fact; no mirror state. |
| daily ecology tick and Bestiary host route | Main.Plans162_165 | ecology pressure, density, extinction, apex and taming projection | WildlifeEcosystemSystem | Owner emits/reads a typed fact; no mirror state. |
| daily ecology tick and Bestiary host route | Main.Plans162_165 | species/edge/season definitions | Wildlife ecology catalog | Owner emits/reads a typed fact; no mirror state. |
| daily ecology tick and Bestiary host route | Main.Plans162_165 | season/map/seed graph composition | WorldHostSession | Owner emits/reads a typed fact; no mirror state. |
| daily ecology tick and Bestiary host route | Main.Plans162_165 | migration/ecosystem/trapping/consumer proof | Ecology focused tests | Owner emits/reads a typed fact; no mirror state. |
| migration/ecosystem/trapping/consumer proof | Ecology focused tests | pack population, movement and migration state | WildlifeMigrationSystem | Owner emits/reads a typed fact; no mirror state. |
| migration/ecosystem/trapping/consumer proof | Ecology focused tests | ecology pressure, density, extinction, apex and taming projection | WildlifeEcosystemSystem | Owner emits/reads a typed fact; no mirror state. |
| migration/ecosystem/trapping/consumer proof | Ecology focused tests | species/edge/season definitions | Wildlife ecology catalog | Owner emits/reads a typed fact; no mirror state. |
| migration/ecosystem/trapping/consumer proof | Ecology focused tests | season/map/seed graph composition | WorldHostSession | Owner emits/reads a typed fact; no mirror state. |
| migration/ecosystem/trapping/consumer proof | Ecology focused tests | daily ecology tick and Bestiary host route | Main.Plans162_165 | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the missing-file/12-row premise with a current ecology-vs-migration authority census. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Trace every species/edge/seasonal row to a loader, migration/ecosystem method, host tick, save owner and player-visible projection. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Decide whether seasonal movement belongs in the existing ecology catalog or requires a new dedicated catalog with a proven loader; do not create both. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Preserve the single population source, remnant-floor/extinction semantics, seeded stream boundaries and map adjacency overlays. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix — Master authority re-read

# Appendix — Master Authority Re-read Record

The following excerpts were selected from the live master authority by this plan's subject terms. They are planning constraints, not claims that the historical backlog is current.

> This document is the complete compiled edition of the ASHFALL Master Expansion Authority v2.0. It combines, in order: (1) the uploaded source document (Volumes 1–24 and its Parts 0 through V, reproduced verbatim), and (2) the Plan Factory's expansion volumes 25 through 57 (the factory batches of 2026-09-24, reproduced verbatim). No content has been altered, merged, or summarized; the two bodies are concatenated at their natural boundary. The source document's own authority order stands: live repository source first; then AGENTS.md; then this document. The factory's constitution (evidence labels, honest bounds, anti-padding) governs Volumes 25 onward.

> **Repository:** `GermanRobert-Labtester/Atomic-War-Starving-Survival` (ASHFALL: Atomic War – Starving Survival)
**Document version:** 2.0.0 — compiled 2026-09-24, supersedes the v1.0 master world bible (compiled 2026-09-23) as an expansion scaffold.
**Document class:** SUBJECT-PLAN FACTORY. This document is not itself an integration plan. It is a repeatable generator: any future planning session can consume its matrices, backlog, and templates to produce an unbounded series of bounded subject plans, each of which names its own best integration route.
**Audit basis:** Live repository inspection performed 2026-09-24 (repository root listing, `Assets/StreamingAssets/Data/` listing at 342 entries, `docs/` listing, `docs/plans/` listing at 126 entries, `INTEGRATION_PLANS.md`, `SESSION_HANDOFF.md`, `AGENTS.md`, branch list). Every claim in the Drift Register (Part I) is labeled VERIFIED, HIGH CONFIDENCE, or UNVERIFIED.
**Authority order:** unchanged from v1.0 — live repository source and data first; then `AGENTS.md`; then this document; then the docs registry and atlas; then plan ledgers. Where this document and live source disagree, live source wins and this document must be corrected.

> The audit inspected the live repository directly: root directory listing, `Assets/StreamingAssets/Data/` (342 entries), `docs/` (top-level documents and subdirectories), `docs/plans/` (126 entries), `INTEGRATION_PLANS.md` (32,793 characters, read head and tail), `SESSION_HANDOFF.md`, `AGENTS.md` (head), and the branch list. No working-tree clone was available in the audit environment; findings marked VERIFIED are directly demonstrated by these listings and file reads. Findings marked UNVERIFIED could not be confirmed in this pass and require a follow-up read before any plan relies on them.

> **DR-02 — The docs tree has substantially more subdirectories than the v1.0 map. VERIFIED.**
Live `docs/` subdirectories observed in the audit include (selection; the listing was long and partially truncated): `adr/`, `agents/`, `architecture/`, `archive/`, `balance/`, `bodymind/`, `campaign/`, `cartography/`, `ci/`, `cli/`, `collectibles/`, `combat/`, `content/`, `contracts/`, `crafting/`, `crossing/`, `culture/`, `decisions/`, `design/`, `discovery/`, `duty_roster/`, `ecology/`, `economy/`, `endgame/`, `expansions/`, `expeditions/`, `faction_war/`, `factions/`, `foreman/`, `forensics/`, `foundry/`, `gaps/`, `governance/`, `greenhouse/`, `health/`, `holdfast/`, `hygiene/`, `i18n/`, `implementation/`, `incidents/`, `integration/`, `journal/`, `lore/`, `maritime/`, `medical/`, `memorials/`, `mods/`, `moral/`, `moral_choice/`, `muster/`, `narrative/`, `onboarding/`, `orbital/`, `perf/`, `phantoms/`, `plans/`, `power/`, `process/`, `production/`, and a `player_surface_manifest.json`. Two of these — `gaps/` and `incidents/` — are first-class *expansion feedstock*: directories whose entire purpose is to record what is missing or broken. The Factory Protocol (Part II, step 2) now treats `docs/gaps/` and `docs/incidents/` as mandatory inputs.

> **DR-03 — New top-level authority documents absent from the v1.0 docs map. VERIFIED.**
Observed live and not listed in v1.0 Part 5.8: `ECONOMY_FAIRNESS_AUDIT.md`, `ENGINE_SUPPORT_POLICY.md`, `GODOT_MIGRATION_STATUS.md`, `REPO_HISTORY_REWRITE.md`, `HUMAN_AUTHORSHIP.md`, `AI_DISCLOSURE.md`, `ASSET_MIGRATION_LEDGER.md`, `CODEX_SOURCE_MATRIX.md`, `ARCHIVE_INDEX.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, `MEDICAL_30_DAY_CAPACITY_REPORT.md`, `SHELTER_MAINTENANCE_MATRIX.md`, `SHELTER_30_DAY_MAINTENANCE_REPORT.md`, `L10N_WAVE2_ROADMAP.md`, `INPUT.md`, `RELEASE_EXPORT.md`, `ENGINE_SUPPORT_POLICY.md`. Of these, `ECONOMY_FAIRNESS_AUDIT.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, and `SHELTER_MAINTENANCE_MATRIX.md` are pre-computed balance baselines: they convert Lane C (economy and balance) planning from speculative to evidence-anchored. Subject plans in Lane C must cite these baselines instead of re-deriving numbers.

> **DR-04 — The data catalog inventory has grown; several catalogs are absent from the v1.0 inventory. VERIFIED.**
`Assets/StreamingAssets/Data/` currently holds 342 entries. Catalogs observed live but not present in the v1.0 Part 5.4 inventory include: `dive_sites.json`, `hydroponic_crops.json`, `hydraulic_extrusion_catalog.json`, `metrology_standards_catalog.json`, `muster_camp_scenes.json`, `muster_epilogues.json`, `muster_faction_actions.json`, `muster_faction_culture.json`, `muster_witnesses.json`, `utility_actions.json`, `moral_choice_quests_branching.json`, `moral_choice_quests_distress.json`. Consequence: the duplication firewall (v1.0 Part 5) is stale in these domains; a planner could propose a "new" muster or moral-choice catalog that already exists. The ID-collision sweep in Factory Protocol step 1 must always run against the live listing, never against this document.

> **DR-05 — A process script lives inside the data authority. VERIFIED (hygiene finding).**
`Assets/StreamingAssets/Data/` contains `rewrite.py` alongside the JSON catalogs. The data directory is canonically "the sole authored JSON data authority" (`AGENTS.md` rule 3); a Python rewrite script inside it is a process artifact in a content directory. Recommended handling: a Tooling-lane (Lane H) subject plan proposing relocation of the script to `scripts/` or `tools/` with a documented rationale, after verifying what the script rewrites and who calls it. Do not move it without call-site verification; it may be load-bearing for a historical catalog migration.

> **DR-06 — Integration ledger state differs from the v1.0 queue snapshot. VERIFIED.**
Live `INTEGRATION_PLANS.md` (read 2026-09-24) shows, at minimum: the **XP Expansion W1** batch ACTIVE (difficulty authority package `XP-WAVE1-DIFFICULTY-AUTHORITY`, with a premise correction recorded against Plan 122 SOFC fuel); the **DISTRESS-SIGNALS-9-12** flagship COMPLETE and presented for acceptance; **Plan 24 CLOSED** (Wave 8, 2026-09-17, signatures resolved 2026-09-18, ward staffing sealed under option b, `DEBT-PLAN24-MEDICAL-WARD-STAFFING` RETIRED); **19A/19B/19C** waves closed with evidence (Endgame 84/84 PASS, focused suites 47 PASS, `verify-fast.sh` reported ALL 47 GATES PASSED); **C2[2] Plan 17** legibility executed with a documented not-executed list (Plan 31 semantic-kind authority, 17C audio phases, 17B deep test matrix remain open gaps); **PR 3 content seal SEALED 2026-09-19** (`CF-P1-DISTRESS-CONTENT-SEAL`); the **availability consumer RETIRED** (Wave 9 Part 2, Option B approved; `SignalTrustAvailability` retained as a pure-math specification pin). Consequence: subject plans in the radio/distress domain must treat the rescue-signal runtime as *sealed and closed*, not as an open expansion surface, unless they extend it through its recorded seams.

**Applied constraints:** one bounded outcome, live-source collision sweep, explicit data/loader/consumer/save/test seams, no parallel authority, no unsupported content growth, and a final precision pass. Master file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Recorded SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.


# Appendix — Deep integration architecture

# Appendix — Deep Integration Architecture

## A. Boundary and responsibility map

The subject is the current ecology data and single-population migration authority, with an explicit decision gate on whether a dedicated migration catalog is warranted. This is a conservative evidence plan, not a row-count expansion.

- **pack population, movement and migration state** remains with `WildlifeMigrationSystem` at `Assets/Ashfall.Core/WildlifeMigrationSystem.cs; Assets/Ashfall.Core/WildlifeMigrationSystem.Live.cs`. Sole pack/population authority; accepts map/season providers and seeded streams.
- **ecology pressure, density, extinction, apex and taming projection** remains with `WildlifeEcosystemSystem` at `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs`. Derives over migration packs and owns its separate ecology state.
- **species/edge/season definitions** remains with `Wildlife ecology catalog` at `Assets/StreamingAssets/Data/wildlife_ecosystem.json`. Current authored data authority; no dedicated migration file is assumed.
- **season/map/seed graph composition** remains with `WorldHostSession` at `src/Host/WorldHostSession.cs`. Binds current world providers and overlays map adjacency.
- **daily ecology tick and Bestiary host route** remains with `Main.Plans162_165` at `src/Main.Plans162_165.cs; src/UI/BestiaryPanel.cs`. Supplies named RNG streams and presentation; not a second ecology owner.
- **migration/ecosystem/trapping/consumer proof** remains with `Ecology focused tests` at `Ashfall.Core.Tests/WildlifeEcosystemSystemTests.cs; Ashfall.Core.Tests/WildlifeSeasonalCalendarTests.cs; Ashfall.Core.Tests/World/Plan176CrossSystemConsumerTests.cs`. Focused evidence surface.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load current ecology catalog and evolving-world/map seeds
2. bind season profile and sector adjacency to the migration owner
3. seed or restore migration packs
4. run deterministic migration/population/apex/taming streams in campaign order
5. derive ecology pressure/density/extinction/observation facts
6. project Bestiary/map/harvest/trapping views
7. save migration and ecology owners through their existing sections

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- Pack records and `seededPopulation` are the current population baseline; ecology state stores pressures, observations, extinctions, apex windows and domestic-animal facts separately.
- Migration never enters blocked sectors and can filter water-bound neighbors; map adjacency is host-supplied projection data.
- A remnant pair/floor protects local extinction while the ecology layer derives density from the same population source.
- The host must pass deterministic population/migration/apex/taming streams; no System.Random or frame-based tick is permitted.

- No ecology method may create or mutate a second population collection.
- A movement target must be an allowed, known sector and a water-bound species must respect water constraints.
- A blocked sector can repel a pack but cannot erase the pack record or fabricate a new route.
- Same seed, map graph, season, day and state produce the same movement/ecology trace.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/Host/WorldHostSession.cs
- src/Host/WildlifeEcosystemHostSession.cs
- src/Main.Plans162_165.cs
- src/UI/BestiaryPanel.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/WildlifeEcosystemSystemTests.cs
- Ashfall.Core.Tests/WildlifeSeasonalCalendarTests.cs
- Ashfall.Core.Tests/World/Plan176CrossSystemConsumerTests.cs

These commands are intentionally small and named. A planning rebuild does not run them and does not convert historical pass counts into fresh evidence. An implementation package records the actual command, result, fixture and limitation.

## F. Precision questions for the next owner

- Which current method is the single mutation point for each durable fact?
- Which loader and validator prove that every authored row is admitted?
- Which host command makes the feature reachable from the live game?
- Which existing save section carries the fact, and what is its frozen legacy shape?
- Which deterministic stream, ordering rule or no-RNG contract governs repeated execution?
- What visible refusal prevents a player from mistaking a projection for authority?
- What focused test would fail if the owner were bypassed?
- What future file or type is explicitly *not* part of this package?


# Appendix — Scenario matrix

# Appendix — Scenario and Negative-Contract Matrix

| ID | Scenario | Precondition | Expected owner outcome | Negative proof | Owner |
| --- | --- | --- | --- | --- | --- |
| S-01 | 35-01 load current ecology catalog | load current ecology catalog and evolving-world/map seeds | Pack records and `seededPopulation` are the current population baseline; ecology state stores pressures, observations, extinctions, apex windows and domestic-animal facts separately. | A new migration catalog duplicates species/seasonal movement already in ecology data. | WildlifeMigrationSystem |
| S-02 | 35-02 seed/restore migration packs | bind season profile and sector adjacency to the migration owner | Migration never enters blocked sectors and can filter water-bound neighbors; map adjacency is host-supplied projection data. | Population is copied into ecology state and diverges after restore. | WildlifeMigrationSystem |
| S-03 | 35-03 seasonal movement with adjacency | seed or restore migration packs | A remnant pair/floor protects local extinction while the ecology layer derives density from the same population source. | A blocked/water constraint is bypassed by a host or panel. | WildlifeMigrationSystem |
| S-04 | 35-04 blocked-sector refusal | run deterministic migration/population/apex/taming streams in campaign order | The host must pass deterministic population/migration/apex/taming streams; no System.Random or frame-based tick is permitted. | Unseeded movement makes paired runs diverge. | WildlifeMigrationSystem |
| S-05 | 35-05 water-sector constraint | derive ecology pressure/density/extinction/observation facts | Pack records and `seededPopulation` are the current population baseline; ecology state stores pressures, observations, extinctions, apex windows and domestic-animal facts separately. | A Bestiary projection reveals or mutates a population the owner did not expose. | WildlifeMigrationSystem |
| S-06 | 35-06 predation/radiation pressure | project Bestiary/map/harvest/trapping views | Migration never enters blocked sectors and can filter water-bound neighbors; map adjacency is host-supplied projection data. | A new migration catalog duplicates species/seasonal movement already in ecology data. | WildlifeMigrationSystem |
| S-07 | 35-07 local extinction/remnant floor | save migration and ecology owners through their existing sections | A remnant pair/floor protects local extinction while the ecology layer derives density from the same population source. | Population is copied into ecology state and diverges after restore. | WildlifeMigrationSystem |
| S-08 | 35-08 hazard avoidance and journal fact | load current ecology catalog and evolving-world/map seeds | The host must pass deterministic population/migration/apex/taming streams; no System.Random or frame-based tick is permitted. | A blocked/water constraint is bypassed by a host or panel. | WildlifeMigrationSystem |
| S-09 | 35-09 Bestiary projection | bind season profile and sector adjacency to the migration owner | Pack records and `seededPopulation` are the current population baseline; ecology state stores pressures, observations, extinctions, apex windows and domestic-animal facts separately. | Unseeded movement makes paired runs diverge. | WildlifeMigrationSystem |
| S-10 | 35-10 paired seeded replay | seed or restore migration packs | Migration never enters blocked sectors and can filter water-bound neighbors; map adjacency is host-supplied projection data. | A Bestiary projection reveals or mutates a population the owner did not expose. | WildlifeMigrationSystem |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 35-TC-01 ecology schema and ID uniqueness | data | ecology schema and ID uniqueness; verify the current owner and its negative boundary without inventing a second authority. | WildlifeMigrationSystem |
| T-02 | 35-TC-02 species/predator-prey references | unit | species/predator-prey references; verify the current owner and its negative boundary without inventing a second authority. | WildlifeMigrationSystem |
| T-03 | 35-TC-03 season move references | persistence | season move references; verify the current owner and its negative boundary without inventing a second authority. | WildlifeMigrationSystem |
| T-04 | 35-TC-04 migration pack duplicate rejection | determinism | migration pack duplicate rejection; verify the current owner and its negative boundary without inventing a second authority. | WildlifeMigrationSystem |
| T-05 | 35-TC-05 adjacency known-sector filter | host | adjacency known-sector filter; verify the current owner and its negative boundary without inventing a second authority. | WildlifeMigrationSystem |
| T-06 | 35-TC-06 water-bound neighbor filter | UI/accessibility | water-bound neighbor filter; verify the current owner and its negative boundary without inventing a second authority. | WildlifeMigrationSystem |
| T-07 | 35-TC-07 blocked-sector filter | cross-system | blocked-sector filter; verify the current owner and its negative boundary without inventing a second authority. | WildlifeMigrationSystem |
| T-08 | 35-TC-08 remnant floor/extinction recovery | data | remnant floor/extinction recovery; verify the current owner and its negative boundary without inventing a second authority. | WildlifeMigrationSystem |
| T-09 | 35-TC-09 ecology derives from migration population | unit | ecology derives from migration population; verify the current owner and its negative boundary without inventing a second authority. | WildlifeMigrationSystem |
| T-10 | 35-TC-10 seeded movement replay | persistence | seeded movement replay; verify the current owner and its negative boundary without inventing a second authority. | WildlifeMigrationSystem |
| T-11 | 35-TC-11 separate RNG streams | determinism | separate RNG streams; verify the current owner and its negative boundary without inventing a second authority. | WildlifeMigrationSystem |
| T-12 | 35-TC-12 legacy save baseline | host | legacy save baseline; verify the current owner and its negative boundary without inventing a second authority. | WildlifeMigrationSystem |
| T-13 | 35-TC-13 Bestiary/host projection | UI/accessibility | Bestiary/host projection; verify the current owner and its negative boundary without inventing a second authority. | WildlifeMigrationSystem |
| T-14 | 35-TC-14 no second population store | cross-system | no second population store; verify the current owner and its negative boundary without inventing a second authority. | WildlifeMigrationSystem |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 21 | `Ashfall.Core.Tests/Campaign/CampaignRngStreamTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 17 | `src/Main.Plans162_165.cs` | current reference count; inspect the caller before treating it as a live route |
| 16 | `Ashfall.Core.Tests/World/Plan176CrossSystemConsumerTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 13 | `Ashfall.Core.Tests/WildlifeEcosystemSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 13 | `src/Host/HostCli.Plans122to125.cs` | current reference count; inspect the caller before treating it as a live route |
| 12 | `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `Ashfall.Core.Tests/WildlifeDisruptionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `src/Main.CampaignOwners.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Ashfall.Core.Tests/EvolvingWorldActivationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `src/Host/HostCli.WorldPlaytest.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `src/Host/WildlifeEcosystemHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `src/Host/WorldHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `src/Main.ShelterBatch3.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Ashfall.Core.Tests/WildlifeSeasonalCalendarTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `src/UI/BestiaryPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Ashfall.Core.Tests/Economy/Plan211BlackMarketHostWiringTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `src/Host/HostCli.Plans162_165.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `src/Main.OrphanSealWave1.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Campaign/CampaignDayCoordinatorTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/EcologyBalanceSimulationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/Random/CampaignRngStream.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Host/ExpeditionHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Main.Plans110_113.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Main.Plans122to125.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Main.Plans130_133.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Main.Plans146_149.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Main.Plans74_77.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Main.ShelterSocial.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Radio/DirectionFindingSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/WorldSaveablesTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/Performance/Workloads/PerformanceCampaignHarness.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/WildlifeMigrationSystem.Live.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/WildlifeMigrationSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/Host/HostCli.EvolvingWorld.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/Host/HostCli.OutpostSettlement.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/Main.Plans78_81.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/wildlife_ecosystem.json`

### `Assets/StreamingAssets/Data/wildlife_ecosystem.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 3850; characters: 3850.
- SHA-256: `8af036291671f572573fdb5d7afc1a751a2ed31c3d9ebc0157a203bb9723ffbf`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `species`, `predator_prey`, `seasonal_moves`

#### `species` — 13 current rows

- Row 001 `species_cotton_hare`: `{"apex_population_threshold":0,"diet_type":"herbivore","display_name":"Cotton Hare","id":"species_cotton_hare","radiation_tolerance":0.3,"tags":["prey","soft_fur"],"tameable":true,"tameness_chance":0.2}`
- Row 002 `species_ash_hound`: `{"apex_population_threshold":0,"diet_type":"carnivore","display_name":"Ash Hound","id":"species_ash_hound","radiation_tolerance":0.55,"tags":["predator","pack_canine","loyal"],"tameable":true,"tameness_chance":0.08}`
- Row 003 `species_feral_goat`: `{"apex_population_threshold":0,"diet_type":"herbivore","display_name":"Feral Goat","id":"species_feral_goat","radiation_tolerance":0.5,"tags":["prey","pack_animal"],"tameable":true,"tameness_chance":0.12}`
- Row 004 `species_blight_rat`: `{"apex_population_threshold":0,"diet_type":"scavenger","display_name":"Blight Rat","id":"species_blight_rat","radiation_tolerance":0.9,"tags":["prey","mutant","swarm"],"tameable":false,"tameness_chance":0}`
- Row 005 `species_ash_boar`: `{"apex_population_threshold":0,"diet_type":"herbivore","display_name":"Ash Boar","id":"species_ash_boar","radiation_tolerance":0.6,"tags":["prey","large"],"tameable":false,"tameness_chance":0}`
- Row 006 `species_mirror_carp`: `{"apex_population_threshold":0,"diet_type":"herbivore","display_name":"Mirror Carp","id":"species_mirror_carp","radiation_tolerance":0.4,"tags":["prey","water"],"tameable":false,"tameness_chance":0}`
- Row 007 `species_ghost_moth`: `{"apex_population_threshold":0,"diet_type":"herbivore","display_name":"Ghost Moth","id":"species_ghost_moth","radiation_tolerance":0.7,"tags":["prey","night"],"tameable":false,"tameness_chance":0}`
- Row 008 `species_rad_dog`: `{"apex_population_threshold":0,"diet_type":"carnivore","display_name":"Rad-Dog","id":"species_rad_dog","radiation_tolerance":0.85,"tags":["predator","mutant","pack_canine"],"tameable":false,"tameness_chance":0}`
- Row 009 `species_wolf`: `{"apex_population_threshold":14,"diet_type":"carnivore","display_name":"Ash Wolf","id":"species_wolf","radiation_tolerance":0.45,"tags":["predator","apex","pack_canine"],"tameable":false,"tameness_chance":0}`
- Row 010 `species_dust_lynx`: `{"apex_population_threshold":10,"diet_type":"carnivore","display_name":"Dust Lynx","id":"species_dust_lynx","radiation_tolerance":0.55,"tags":["predator","apex","lurker"],"tameable":false,"tameness_chance":0}`
- Row 011 `species_iron_crow`: `{"apex_population_threshold":0,"diet_type":"scavenger","display_name":"Iron Crow","id":"species_iron_crow","radiation_tolerance":0.65,"tags":["scavenger"],"tameable":false,"tameness_chance":0}`
- Row 012 `species_ash_gull`: `{"apex_population_threshold":0,"diet_type":"scavenger","display_name":"Ash Gull","id":"species_ash_gull","radiation_tolerance":0.5,"tags":["scavenger","water"],"tameable":false,"tameness_chance":0}`
- Row 013 `species_gray_heron`: `{"apex_population_threshold":0,"diet_type":"carnivore","display_name":"Gray Heron","id":"species_gray_heron","radiation_tolerance":0.45,"tags":["predator","water"],"tameable":false,"tameness_chance":0}`

#### `predator_prey` — 5 current rows

- Row 001 `species_wolf`: `{"predation_pressure":0.06,"predator_species_id":"species_wolf","prey_species_id":"species_cotton_hare"}`
- Row 002 `species_wolf`: `{"predation_pressure":0.04,"predator_species_id":"species_wolf","prey_species_id":"species_feral_goat"}`
- Row 003 `species_dust_lynx`: `{"predation_pressure":0.05,"predator_species_id":"species_dust_lynx","prey_species_id":"species_cotton_hare"}`
- Row 004 `species_rad_dog`: `{"predation_pressure":0.07,"predator_species_id":"species_rad_dog","prey_species_id":"species_blight_rat"}`
- Row 005 `species_gray_heron`: `{"predation_pressure":0.05,"predator_species_id":"species_gray_heron","prey_species_id":"species_mirror_carp"}`

#### `seasonal_moves` — 2 current rows

- Row 001 `species_cotton_hare`: `{"move_chance":0.08,"season_window_id":"window_deep_freeze","species_id":"species_cotton_hare"}`
- Row 002 `species_ash_gull`: `{"move_chance":0.1,"season_window_id":"window_spring_storms","species_id":"species_ash_gull"}`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/seasonal_human_migration.json`

### `Assets/StreamingAssets/Data/seasonal_human_migration.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 2368; characters: 2368.
- SHA-256: `e17f8a4316477e85a12852a7a2b8a60df336157c518afc63dc96dfcbc2a69622`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `dwell_days`, `factions`

#### `factions` — 4 current rows

- Row 001 `faction_the_compact`: `{"faction_id":"faction_the_compact","schedule":[{"phase":"deep_winter","population_delta":25,"region_id":"settlement"},{"phase":"thaw","population_delta":20,"region_id":"iron_basin"},{"phase":"dry_heat","population_delta":-15,"region_id":"…`
- Row 002 `faction_the_scale`: `{"faction_id":"faction_the_scale","schedule":[{"phase":"thaw","population_delta":30,"region_id":"ash_flats"},{"phase":"dry_heat","population_delta":25,"region_id":"deep_coast"},{"phase":"ash_winds","population_delta":-20,"region_id":"ash_f…`
- Row 003 `faction_black_flotilla`: `{"faction_id":"faction_black_flotilla","schedule":[{"phase":"dry_heat","population_delta":35,"region_id":"deep_coast"},{"phase":"ash_winds","population_delta":-15,"region_id":"deep_coast"},{"phase":"deep_winter","population_delta":20,"regi…`
- Row 004 `faction_the_overlay`: `{"faction_id":"faction_the_overlay","schedule":[{"phase":"deep_winter","population_delta":15,"region_id":"iron_basin"},{"phase":"thaw","population_delta":10,"region_id":"settlement"},{"phase":"dry_heat","population_delta":20,"region_id":"i…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/wildlife_trapping_catalog.json`

### `Assets/StreamingAssets/Data/wildlife_trapping_catalog.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 23204; characters: 23194.
- SHA-256: `ad76e65163efc3860f4ef7d6b209479f27489c0f5cb34a717fe4e64bd5d64d00`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `traps`, `prey`, `baits`

#### `traps` — 10 current rows

- Row 001 `trap_snare`: `{"baseCatchModifier":1.0,"checkIntervalDays":2,"compatiblePrey":["rabbit","cotton_hare","fox","rat"],"description":"A loop of cord or wire set across a game trail, tightened by a springy branch. Cheap enough to deploy more than one, unreli…`
- Row 002 `trap_deadfall`: `{"baseCatchModifier":0.8,"checkIntervalDays":2,"compatiblePrey":["rat","rabbit","hedgehog","irradiated_squirrel","fox"],"description":"A flat stone or salvaged plate balanced on a trigger stick. Low-tech, material-light, and as old as hung…`
- Row 003 `trap_pit`: `{"baseCatchModifier":0.6,"checkIntervalDays":4,"compatiblePrey":["boar","deer"],"description":"A concealed pit dug into a game trail, lined with sharpened stakes. Labor-heavy, location-dependent, and capable of stopping something large. Th…`
- Row 004 `trap_net`: `{"baseCatchModifier":0.9,"bycatchChance":0.25,"bycatchSpecies":[{"speciesId":"rat","weight":3.0},{"speciesId":"hedgehog","weight":2.0},{"speciesId":"irradiated_squirrel","weight":1.0}],"checkIntervalDays":1,"compatiblePrey":["pheasant","as…`
- Row 005 `trap_fish`: `{"baseCatchModifier":0.7,"checkIntervalDays":3,"compatiblePrey":["mirror_carp","ash_pike","muskrat"],"description":"A woven funnel of wood and cord set in moving water. Fish enter but cannot find the exit. Strong where geography supports i…`
- Row 006 `trap_cage`: `{"baseCatchModifier":0.8,"checkIntervalDays":2,"compatiblePrey":["rabbit","rat","fox","rad_dog"],"description":"A sprung cage of salvaged metal and wire, triggered by weight on a plate. Expensive to build, durable once built. The animal li…`
- Row 007 `trap_bird_snare`: `{"baseCatchModifier":0.7,"bycatchChance":0.15,"bycatchSpecies":[{"speciesId":"irradiated_squirrel","weight":2.0},{"speciesId":"rat","weight":1.0}],"checkIntervalDays":1,"compatiblePrey":["pheasant","ash_crow","contaminated_fowl"],"descript…`
- Row 008 `trap_body_grip`: `{"baseCatchModifier":1.0,"checkIntervalDays":2,"compatiblePrey":["muskrat","fox","cotton_hare"],"description":"A heavy spring-loaded mechanism that closes steel jaws on anything that passes through the frame. Efficient, durable, and severe…`
- Row 009 `trap_box`: `{"baseCatchModifier":0.8,"checkIntervalDays":2,"compatiblePrey":["rabbit","rat","hedgehog","irradiated_squirrel"],"description":"A wooden box with a gravity door, triggered by a treadle inside. Bulky, dependable, and shelter-craftable. The…`
- Row 010 `trap_improvised_wire`: `{"baseCatchModifier":0.5,"checkIntervalDays":1,"compatiblePrey":["rat","rabbit"],"description":"A twist of copper wire shaped into a loose loop and pegged to the ground. The poorest survivor's option, better than doing nothing. It breaks o…`

#### `prey` — 15 current rows

- Row 001 `row-1`: `{"activeSeasons":[],"attractedByBaitIds":["bait_grain_lure","bait_berry_mash"],"baseYieldKg":1.2,"contaminationRisk":0.05,"description":"A small grey-brown rabbit, lean from foraging in contaminated soil. Common enough to teach the trappin…`
- Row 002 `row-2`: `{"activeSeasons":["window_spring_storms","window_dry_ash","window_false_spring"],"attractedByBaitIds":["bait_grain_lure","bait_berry_mash"],"baseYieldKg":1.5,"contaminationRisk":0.05,"description":"A larger hare with pale winter fur that d…`
- Row 003 `row-3`: `{"activeSeasons":["window_spring_storms","window_dry_ash","window_false_spring"],"attractedByBaitIds":["bait_berry_mash","bait_salt_lick"],"baseYieldKg":15.0,"contaminationRisk":0.05,"description":"A gaunt mule deer, ribs showing through a…`
- Row 004 `row-4`: `{"activeSeasons":["window_dry_ash","window_false_spring"],"attractedByBaitIds":["bait_salt_lick","bait_fat_cake"],"baseYieldKg":12.0,"contaminationRisk":0.1,"description":"A heavy-bodied boar with a ridge of bristled hair along its spine. …`
- Row 005 `row-5`: `{"activeSeasons":[],"attractedByBaitIds":["bait_scrap_meat","bait_fat_cake"],"baseYieldKg":2.0,"contaminationRisk":0.1,"description":"A lean fox with patchy fur and alert eyes. Uncommon, lower food-efficiency than rabbit, with elevated dis…`
- Row 006 `row-6`: `{"activeSeasons":[],"attractedByBaitIds":["bait_scrap_meat"],"baseYieldKg":0.6,"contaminationDose":4.0,"contaminationRisk":0.2,"description":"A bloated rat with thinning fur, common near ruins and shelter edges. Desperate food — low yield,…`
- Row 007 `row-7`: `{"activeSeasons":["window_spring_storms","window_dry_ash","window_false_spring"],"attractedByBaitIds":["bait_grain_lure","bait_berry_mash"],"baseYieldKg":1.5,"contaminationRisk":0.1,"description":"A ground-feeding bird with dull plumage, c…`
- Row 008 `row-8`: `{"activeSeasons":["window_spring_storms","window_dry_ash","window_false_spring"],"attractedByBaitIds":["bait_pheromone","bait_grain_lure"],"baseYieldKg":0.8,"contaminationDose":6.0,"contaminationRisk":0.15,"description":"A scavenger bird a…`
- Row 009 `row-9`: `{"activeSeasons":["window_spring_storms","window_dry_ash"],"attractedByBaitIds":["bait_grain_lure","bait_berry_mash"],"baseYieldKg":1.8,"contaminationRisk":0.08,"description":"A large-scaled carp, reliable in moving water. Water location r…`
- Row 010 `row-10`: `{"activeSeasons":["window_spring_storms","window_dry_ash"],"attractedByBaitIds":["bait_scrap_meat","bait_fat_cake"],"baseYieldKg":2.5,"contaminationRisk":0.1,"description":"A predatory freshwater fish, rarer and larger than the carp. Stron…`
- Row 011 `row-11`: `{"activeSeasons":[],"attractedByBaitIds":["bait_berry_mash","bait_grain_lure"],"baseYieldKg":0.4,"contaminationDose":20.0,"contaminationRisk":0.35,"description":"A small rodent with patchy fur and swollen glands, common in contaminated zon…`
- Row 012 `row-12`: `{"activeSeasons":["window_spring_storms","window_dry_ash"],"attractedByBaitIds":["bait_grain_lure","bait_berry_mash"],"baseYieldKg":1.0,"contaminationDose":12.0,"contaminationRisk":0.4,"description":"A bird with discolored plumage and labo…`
- Row 013 `row-13`: `{"activeSeasons":[],"attractedByBaitIds":["bait_scrap_meat","bait_fat_cake"],"baseYieldKg":4.0,"contaminationDose":8.0,"contaminationRisk":0.2,"description":"A feral canine with patchy fur and wary eyes, traveling in small packs. Rare, mor…`
- Row 014 `row-14`: `{"activeSeasons":["window_spring_storms","window_dry_ash","window_false_spring"],"attractedByBaitIds":["bait_berry_mash","bait_grain_lure"],"baseYieldKg":1.5,"contaminationRisk":0.15,"description":"A semi-aquatic rodent found near waterway…`
- Row 015 `row-15`: `{"activeSeasons":["window_spring_storms","window_dry_ash"],"attractedByBaitIds":["bait_berry_mash","bait_grain_lure"],"baseYieldKg":0.5,"contaminationRisk":0.05,"description":"A small spined mammal, seasonal and low-yield. Limited active w…`

#### `baits` — 6 current rows

- Row 001 `row-1`: `{"baitId":"bait_scrap_meat","catchBonusMultiplier":1.3,"craftCostChemicals":0,"craftCostRoots":0,"craftCostScrapMeat":1,"displayName":"Scrap-Meat Bait","preferredSpecies":["rat","fox","rad_dog"],"toxicReduction":0.0}`
- Row 002 `row-2`: `{"baitId":"bait_grain_lure","catchBonusMultiplier":1.5,"craftCostChemicals":0,"craftCostRoots":2,"craftCostScrapMeat":0,"displayName":"Grain Lure","preferredSpecies":["rabbit","pheasant","cotton_hare"],"toxicReduction":0.1}`
- Row 003 `row-3`: `{"baitId":"bait_pheromone","catchBonusMultiplier":2.0,"craftCostChemicals":1,"craftCostRoots":0,"craftCostScrapMeat":2,"displayName":"Mutated-Beast Pheromone Lure","preferredSpecies":["molerat","slag_beetle","ash_crow"],"toxicReduction":0.…`
- Row 004 `row-4`: `{"baitId":"bait_fat_cake","catchBonusMultiplier":1.8,"craftCostChemicals":0,"craftCostRoots":0,"craftCostScrapMeat":2,"displayName":"Rendered Fat Cake","preferredSpecies":["fox","dust_lynx","wolf","rad_dog"],"toxicReduction":0.15}`
- Row 005 `row-5`: `{"baitId":"bait_berry_mash","catchBonusMultiplier":1.2,"craftCostChemicals":0,"craftCostRoots":3,"craftCostScrapMeat":0,"displayName":"Fermented Berry Mash","preferredSpecies":["rabbit","pheasant","deer","cotton_hare"],"toxicReduction":0.2}`
- Row 006 `row-6`: `{"baitId":"bait_salt_lick","catchBonusMultiplier":1.6,"craftCostChemicals":1,"craftCostRoots":1,"craftCostScrapMeat":0,"displayName":"Mineral Salt Lick","preferredSpecies":["deer","wolf","boar"],"toxicReduction":0.1}`


# Appendix — Current Source Detail: `Assets/Ashfall.Core/WildlifeMigrationSystem.cs`

### `Assets/Ashfall.Core/WildlifeMigrationSystem.cs` — complete current file

- Size: 126 lines / 4626 bytes.
- SHA-256: `2eb9ed3fa4fc9ff2b919ff8dafa69aaa17461be2d7491ee100239f9e84ab76c8`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005:
00006: namespace Ashfall.Core
00007: {
00008:     [Serializable]
00009:     public sealed class WildlifeSaveState
00010:     {
00011:         public int schema_version = 1;
00012:         public string systemId = WildlifeMigrationSystem.SystemId;
00013:         public int lastMigrationDay = -1;
00014:         public List<WildlifePackRecord> packs = new List<WildlifePackRecord>();
00015:     }
00016:
00017:     [Serializable]
00018:     public sealed class WildlifePackRecord
00019:     {
00020:         public string packId = string.Empty;
00021:         public string speciesId = string.Empty;
00022:         public string currentSectorId = string.Empty;
00023:         public int population = 5;
00024:         /// <summary>Population at seed time; the baseline population recovery and scarcity read against. 0 on legacy saves.</summary>
00025:         public int seededPopulation;
00026:         public float aggressionScore = 0.5f;
00027:         public float starvationLevel;
00028:         public bool isRabid;
00029:         public int lastThreatFiredDay = -1;
00030:     }
00031:
00032:     public sealed partial class WildlifeMigrationSystem
00033:     {
00034:         public const string SystemId = "wildlife_migration";
00035:         private WildlifeSaveState _state = new WildlifeSaveState();
00036:         private readonly ISeededRng _rng;
00037:         private readonly ILog _log;
00038:
00039:         public WildlifeSaveState State => _state;
00040:         public event Action<WildlifePackRecord> OnPackMigrated;
00041:
00042:         public WildlifeMigrationSystem(ISeededRng? rng = null, ILog? log = null)
00043:         {
00044:             _rng = rng ?? new SeededRng(42);
00045:             _log = log ?? NullLog.Instance;
00046:         }
00047:
00048:         public ActionResult RegisterPack(string packId, string speciesId, string sectorId, int population)
00049:         {
00050:             if (_state.packs.Exists(p => string.Equals(p.packId, packId, StringComparison.Ordinal)))
00051:                 return ActionResult.Blocked("pack_exists", "wildlife.pack_exists");
00052:
00053:             var pack = new WildlifePackRecord
00054:             {
00055:                 packId = packId,
00056:                 speciesId = speciesId,
00057:                 currentSectorId = sectorId,
00058:                 population = population,
00059:                 seededPopulation = population
00060:             };
00061:             _state.packs.Add(pack);
00062:             return ActionResult.Success("wildlife.pack_registered");
00063:         }
00064:
00065:         public ActionResult MigratePack(string packId, string targetSectorId)
00066:         {
00067:             var pack = _state.packs.Find(p => string.Equals(p.packId, packId, StringComparison.Ordinal));
00068:             if (pack == null) return ActionResult.Failed("unknown_pack", "wildlife.unknown_pack");
00069:
00070:             pack.currentSectorId = targetSectorId;
00071:             OnPackMigrated?.Invoke(pack);
00072:             return ActionResult.Success("wildlife.pack_migrated");
00073:         }
00074:
00075:         public void TickDay(int day)
00076:         {
00077:             _state.lastMigrationDay = day;
00078:             foreach (var pack in _state.packs)
00079:             {
00080:                 pack.starvationLevel = Math.Min(1f, pack.starvationLevel + 0.05f);
00081:                 if (pack.starvationLevel > 0.7f)
00082:                 {
00083:                     pack.aggressionScore = Math.Min(1f, pack.aggressionScore + 0.1f);
00084:                 }
00085:             }
00086:         }
00087:
00088:         public WildlifeSaveState CaptureState() => CloneState(_state);
00089:
00090:         public void RestoreState(WildlifeSaveState saved)
00091:         {
00092:             if (saved == null) return;
00093:             _state = CloneState(saved);
00094:         }
00095:
00096:         private static WildlifeSaveState CloneState(WildlifeSaveState src)
00097:         {
00098:             if (src == null) return new WildlifeSaveState();
00099:             var clone = new WildlifeSaveState
00100:             {
00101:                 schema_version = src.schema_version,
00102:                 systemId = src.systemId,
00103:                 lastMigrationDay = src.lastMigrationDay,
00104:                 packs = new List<WildlifePackRecord>(src.packs.Count)
00105:             };
00106:             for (int i = 0; i < src.packs.Count; i++)
00107:             {
00108:                 var p = src.packs[i];
00109:                 if (p == null) continue;
00110:                 clone.packs.Add(new WildlifePackRecord
00111:                 {
00112:                     packId = p.packId,
00113:                     speciesId = p.speciesId,
00114:                     currentSectorId = p.currentSectorId,
00115:                     population = p.population,
00116:                     seededPopulation = p.seededPopulation,
00117:                     aggressionScore = p.aggressionScore,
00118:                     starvationLevel = p.starvationLevel,
00119:                     isRabid = p.isRabid,
00120:                     lastThreatFiredDay = p.lastThreatFiredDay
00121:                 });
00122:             }
00123:             return clone;
00124:         }
00125:     }
00126: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/WildlifeMigrationSystem.Live.cs`

### `Assets/Ashfall.Core/WildlifeMigrationSystem.Live.cs` — complete current file

- Size: 312 lines / 14998 bytes.
- SHA-256: `a4179929ec4084166854cb2363faa1c393e176f437bd90249a4e1b3ece2ded55`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core.World;
00005:
00006: namespace Ashfall.Core
00007: {
00008:     public partial class WildlifeMigrationSystem
00009:     {
00010:         // ── Tunables ────────────────────────────────────────────────────
00011:         /// <summary>Daily chance a starving pack (starvation &gt; HungerDriveThreshold) moves to an adjacent sector.</summary>
00012:         public const float MigrationChancePerDay = 0.25f;
00013:         /// <summary>Starvation level above which a pack feels the hunger drive to move.</summary>
00014:         public const float HungerDriveThreshold = 0.5f;
00015:         /// <summary>Moving to new ground relieves hunger: the pack found something to eat on the way.</summary>
00016:         public const float MigrationStarvationRelief = 0.35f;
00017:         /// <summary>Starvation above which the pack loses members daily.</summary>
00018:         public const float StarvationLossThreshold = 0.7f;
00019:         /// <summary>Daily chance a starving pack turns rabid (checked while above the loss threshold).</summary>
00020:         public const float RabiesChancePerDay = 0.03f;
00021:         /// <summary>Days a well-fed pack needs between births.</summary>
00022:         public const int BreathingRoomDaysForBirth = 3;
00023:
00024:         /// <summary>Sector adjacency for auto-migration, keyed by sector id. Set at seed time.</summary>
00025:         private readonly Dictionary<string, List<string>> _sectorNeighbors =
00026:             new Dictionary<string, List<string>>(StringComparer.Ordinal);
00027:
00028:         /// <summary>Plan 28 Phase 3: war-blocked sectors. Packs will not move INTO these; a pack already inside may still flee to unblocked ground.</summary>
00029:         private readonly HashSet<string> _blockedSectors = new HashSet<string>(StringComparer.Ordinal);
00030:
00031:         /// <summary>Waterway sectors (Plan 28): water-bound runners migrate only along these.</summary>
00032:         private readonly HashSet<string> _waterSectors = new HashSet<string>(StringComparer.Ordinal);
00033:
00034:         /// <summary>Plan 19 season authority bound at host seed time; null keeps legacy season-neutral behavior.</summary>
00035:         private SeasonProfileDef? _seasonProfile;
00036:
00037:         /// <summary>
00038:         /// Bind the Plan 19 season profile (weather_seasons.json). Optional:
00039:         /// without a bound profile every pack reads as season-neutral and the
00040:         /// tick behaves exactly as before Plan 28.
00041:         /// </summary>
00042:         public void BindSeasonProfile(World.SeasonProfileDef? profile) => _seasonProfile = profile;
00043:
00044:         /// <summary>
00045:         /// Mark waterway sectors so water-bound runners (fish, piscivore birds)
00046:         /// migrate along the waterway pair instead of crossing dry land.
00047:         /// </summary>
00048:         public void SetWaterSectors(IEnumerable<string>? sectorIds)
00049:         {
00050:             if (sectorIds == null) return;
00051:             _waterSectors.Clear();
00052:             foreach (var s in sectorIds)
00053:                 if (!string.IsNullOrEmpty(s)) _waterSectors.Add(s);
00054:         }
00055:
00056:         public bool IsWaterSector(string sectorId) =>
00057:             !string.IsNullOrEmpty(sectorId) && _waterSectors.Contains(sectorId);
00058:
00059:         /// <summary>
00060:         /// Provide the sector graph packs migrate along. Only links between
00061:         /// known sectors are kept; packs in unlinked sectors stay put.
00062:         /// </summary>
00063:         public void SetSectorAdjacency(IEnumerable<(string sectorId, List<string> neighbors)> links)
00064:         {
00065:             if (links == null) return;
00066:             foreach (var (sectorId, neighbors) in links)
00067:             {
00068:                 if (string.IsNullOrEmpty(sectorId) || neighbors == null || neighbors.Count == 0) continue;
00069:                 _sectorNeighbors[sectorId] = new List<string>(neighbors);
00070:             }
00071:         }
00072:
00073:         /// <summary>
00074:         /// Plan 30C — overlay extra geographic neighbors without replacing the seed graph.
00075:         /// </summary>
00076:         public void MergeSectorAdjacency(IEnumerable<(string sectorId, List<string> neighbors)> links)
00077:         {
00078:             if (links == null) return;
00079:             foreach (var (sectorId, neighbors) in links)
00080:             {
00081:                 if (string.IsNullOrEmpty(sectorId) || neighbors == null || neighbors.Count == 0) continue;
00082:                 if (!_sectorNeighbors.TryGetValue(sectorId, out var existing) || existing == null)
00083:                 {
00084:                     _sectorNeighbors[sectorId] = new List<string>(neighbors);
00085:                     continue;
00086:                 }
00087:                 for (int i = 0; i < neighbors.Count; i++)
00088:                 {
00089:                     string n = neighbors[i];
00090:                     if (string.IsNullOrEmpty(n) || existing.Contains(n)) continue;
00091:                     existing.Add(n);
00092:                 }
00093:             }
00094:         }
00095:
00096:         public bool TryGetNeighbors(string sectorId, out List<string> neighbors)
00097:         {
00098:             return _sectorNeighbors.TryGetValue(sectorId, out neighbors!);
00099:         }
00100:
00101:         // ── Plan 28 Phase 3: war-blocked corridors & harvest pressure ──
00102:
00103:         /// <summary>
00104:         /// Mark a sector as blocked (faction war, fortified ground). Blocked
00105:         /// sectors are removed from movement targets; packs already inside may
00106:         /// still flee to unblocked neighbors. Projection state: recomputed
00107:         /// from the host's live faction dominance each day, never persisted.
00108:         /// </summary>
00109:         public void SetSectorBlocked(string sectorId, bool blocked)
00110:         {
00111:             if (string.IsNullOrEmpty(sectorId)) return;
00112:             if (blocked) _blockedSectors.Add(sectorId);
00113:             else _blockedSectors.Remove(sectorId);
00114:         }
00115:
00116:         public bool IsSectorBlocked(string sectorId) =>
00117:             !string.IsNullOrEmpty(sectorId) && _blockedSectors.Contains(sectorId);
00118:
00119:         /// <summary>Clear all blockages (re-projected fresh each day by the host).</summary>
00120:         public void ClearSectorBlockages() => _blockedSectors.Clear();
00121:
00122:         /// <summary>
00123:         /// Plan 28 Phase 3 — harvest pressure: each trapped animal thins the
00124:         /// largest population pack holding <paramref name="sectorId"/> (ties
00125:         /// broken by pack id, ordinal). Bounded at zero by the pack record;
00126:         /// recovery remains the existing birth rule. No RNG: deterministic.
00127:         /// Returns the population actually removed.
00128:         /// </summary>
00129:         public int ApplyHarvestPressure(string sectorId, int amount)
00130:         {
00131:             if (string.IsNullOrEmpty(sectorId) || amount <= 0) return 0;
00132:             int removed = 0;
00133:             for (int i = 0; i < amount; i++)
00134:             {
00135:                 WildlifePackRecord? best = null;
00136:                 foreach (var p in _state.packs)
00137:                 {
00138:                     if (p == null || p.population <= 0
00139:                         || !string.Equals(p.currentSectorId, sectorId, StringComparison.Ordinal)) continue;
00140:                     if (p.population <= 1) continue; // a remnant pair always survives
00141:                     if (best == null
00142:                         || p.population > best.population
00143:                         || (p.population == best.population && string.CompareOrdinal(p.packId, best.packId) < 0))
00144:                     {
00145:                         best = p;
00146:                     }
00147:                 }
00148:                 if (best == null) break;
00149:                 best.population--;
00150:                 removed++;
00151:             }
00152:             return removed;
00153:         }
00154:
00155:         public WildlifePackRecord? TryGetPack(string packId)
00156:         {
00157:             if (string.IsNullOrEmpty(packId)) return null;
00158:             return _state.packs.Find(p => string.Equals(p.packId, packId, StringComparison.Ordinal));
00159:         }
00160:
00161:         /// <summary>
00162:         /// Plan 165: species-targeted deterministic thinning for the ecology
00163:         /// layer (predation, radiation attrition, taming removal). Same rules
00164:         /// as harvest pressure — largest-first, ties by pack id, remnant floor
00165:         /// respected — but scoped to one species so the ecosystem never touches
00166:         /// another species' packs. Returns the population actually removed.
00167:         /// </summary>
00168:         public int ThinSpeciesInSector(string speciesId, string sectorId, int amount, int floor = 2)
00169:         {
00170:             if (string.IsNullOrEmpty(speciesId) || string.IsNullOrEmpty(sectorId) || amount <= 0) return 0;
00171:             if (floor < 0) floor = 0;
00172:             int removed = 0;
00173:             for (int i = 0; i < amount; i++)
00174:             {
00175:                 WildlifePackRecord? best = null;
00176:                 foreach (var p in _state.packs)
00177:                 {
00178:                     if (p == null || p.population <= floor
00179:                         || !string.Equals(p.currentSectorId, sectorId, StringComparison.Ordinal)
00180:                         || !string.Equals(p.speciesId, speciesId, StringComparison.Ordinal)) continue;
00181:                     if (best == null
00182:                         || p.population > best.population
00183:                         || (p.population == best.population && string.CompareOrdinal(p.packId, best.packId) < 0))
00184:                     {
00185:                         best = p;
00186:                     }
00187:                 }
00188:                 if (best == null) break;
00189:                 best.population--;
00190:                 removed++;
00191:             }
00192:             return removed;
00193:         }
00194:
00195:         /// <summary>
00196:         /// Pack pressure in a sector: total population of packs currently
00197:         /// holding it. Trapping, encounter weighting, and scarcity read this.
00198:         /// </summary>
00199:         public int GetSectorPackPopulation(string sectorId)
00200:         {
00201:             int total = 0;
00202:             foreach (var p in _state.packs)
00203:             {
00204:                 if (p != null && p.population > 0
00205:                     && string.Equals(p.currentSectorId, sectorId, StringComparison.Ordinal))
00206:                 {
00207:                     total += p.population;
00208:                 }
00209:             }
00210:             return total;
00211:         }
00212:
00213:         /// <summary>Live population over seeded population, 1.0 when never seeded. Drives scarcity.</summary>
00214:         public float GetGlobalPopulationRatio()
00215:         {
00216:             int live = 0, seeded = 0;
00217:             foreach (var p in _state.packs)
00218:             {
00219:                 if (p == null) continue;
00220:                 live += p.population;
00221:                 seeded += p.seededPopulation > 0 ? p.seededPopulation : p.population;
00222:             }
00223:             return seeded > 0 ? (float)live / seeded : 1f;
00224:         }
00225:
00226:         /// <summary>
00227:         /// Live-world daily tick: hunger, movement along the sector graph,
00228:         /// population loss and recovery, rabies. All rolls come from the
00229:         /// caller's per-day fork so the trajectory depends on the day, not on
00230:         /// call counts. The legacy <see cref="TickDay(int)"/> delegates here.
00231:         /// </summary>
00232:         public void TickDay(int day, ISeededRng? dayRng = null)
00233:         {
00234:             var rng = dayRng ?? _rng;
00235:             _state.lastMigrationDay = day;
00236:
00237:             // Plan 28: one season window per day, resolved once. No bound
00238:             // profile (headless tools, legacy hosts) keeps every factor at 1.0.
00239:             var season = _seasonProfile != null && _seasonProfile.seasons is { Count: > 0 }
00240:                 ? WildlifeSeasonalCalendar.SeasonWindowForDay(_seasonProfile, day)
00241:                 : null;
00242:
00243:             foreach (var pack in _state.packs)
00244:             {
00245:                 if (pack == null) continue;
00246:
00247:                 // Hunger grows on held ground, paced by the seasonal calendar.
00248:                 float hungerFactor = season != null
00249:                     ? WildlifeSeasonalCalendar.HungerFactor(
00250:                         season, WildlifeSeasonalCalendar.ArchetypeOf(pack.speciesId))
00251:                     : 1f;
00252:                 pack.starvationLevel = Math.Min(1f, pack.starvationLevel + 0.05f * hungerFactor);
00253:
00254:                 // The hunger drive: starving packs move to adjacent ground,
00255:                 // paced by the Plan 28 seasonal calendar. War-blocked sectors
00256:                 // (Plan 28 Phase 3) are never entered; a pack already inside
00257:                 // may still flee to unblocked ground.
00258:                 if (rng != null && pack.starvationLevel > HungerDriveThreshold
00259:                     && pack.population > 0
00260:                     && _sectorNeighbors.TryGetValue(pack.currentSectorId, out var neighbors)
00261:                     && neighbors.Count > 0
00262:                     && rng.NextDouble() < MigrationChancePerDay)
00263:                 {
00264:                     var archetype = WildlifeSeasonalCalendar.ArchetypeOf(pack.speciesId);
00265:                     var candidates = WildlifeSeasonalCalendar.FilterNeighbors(
00266:                         archetype, pack.currentSectorId, neighbors, _waterSectors);
00267:                     if (_blockedSectors.Count > 0)
00268:                     {
00269:                         var passable = new List<string>();
00270:                         foreach (var n in candidates)
00271:                             if (!_blockedSectors.Contains(n)) passable.Add(n);
00272:                         // Blocked ground is never entered; a pack already
00273:                         // inside may still flee to the last open neighbor.
00274:                         // Fully enclosed packs stay put (siege: hunger grows).
00275:                         candidates = passable;
00276:                     }
00277:                     if (candidates.Count > 0)
00278:                     {
00279:                         string target = candidates[rng.Next(0, candidates.Count)];
00280:                         pack.currentSectorId = target;
00281:                         pack.starvationLevel = Math.Max(0f, pack.starvationLevel - MigrationStarvationRelief);
00282:                         OnPackMigrated?.Invoke(pack);
00283:                     }
00284:                 }
00285:
00286:                 if (pack.starvationLevel > StarvationLossThreshold)
00287:                 {
00288:                     // Starving packs thin out and can turn rabid.
00289:                     if (pack.population > 0) pack.population--;
00290:                     if (!pack.isRabid && rng != null && rng.NextDouble() < RabiesChancePerDay)
00291:                     {
00292:                         pack.isRabid = true;
00293:                         pack.lastThreatFiredDay = day;
00294:                     }
00295:                     // Desperation reads as aggression (existing rule).
00296:                     pack.aggressionScore = Math.Min(1f, pack.aggressionScore + 0.1f);
00297:                 }
00298:                 else if (pack.starvationLevel < 0.3f && pack.population > 0
00299:                          && day - pack.lastThreatFiredDay >= BreathingRoomDaysForBirth)
00300:                 {
00301:                     // Fed ground lets a pack recover toward twice its seed size.
00302:                     int ceiling = (pack.seededPopulation > 0 ? pack.seededPopulation : pack.population) * 2;
00303:                     if (pack.population < ceiling)
00304:                     {
00305:                         pack.population++;
00306:                         pack.aggressionScore = Math.Max(0f, pack.aggressionScore - 0.05f);
00307:                     }
00308:                 }
00309:             }
00310:         }
00311:     }
00312: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs`

### `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` — bounded current excerpt (580 of 651 lines)

- Size: 651 lines / 32234 bytes.
- SHA-256: `beef926f2f7a20485888a8c6112ee6cbdbe2b5ace9750ac879c41022aabf64c8`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // ============================================================================
00003: // System     : WildlifeEcosystemSystem (Plan 165 — Wasteland Wildlife Ecology)
00004: // Authority  : WildlifeMigrationSystem remains THE population store (packs in
00005: //      the `world` save section). This system is the upstream ecology layer:
00006: //      it derives per-sector densities from packs, applies predation /
00007: //      radiation pressure / seasonal moves THROUGH migration APIs, tracks
00008: //      hunting/trapping pressure, local extinction, apex activity, taming
00009: //      handoff, and bestiary knowledge. It never keeps a second population.
00010: // Catalog     : wildlife_ecosystem.json (species aligned with the existing
00011: //      species_* archetype ids, predator-prey edges, seasonal patterns).
00012: // RNG         : injected per tick (wildlife.population / .migration / .apex /
00013: //      .taming forks owned by the host).
00014: // ============================================================================
00015: using System;
00016: using System.Collections.Generic;
00017:
00018: namespace Ashfall.Core.World
00019: {
00020:     [Serializable]
00021:     public sealed class FaunaSpeciesDef
00022:     {
00023:         public string id { get; set; } = string.Empty;              // species_*
00024:         public string display_name { get; set; } = string.Empty;
00025:         public float radiation_tolerance { get; set; } = 0.5f;      // 0..1 vs OutdoorRadModifier/250
00026:         public string diet_type { get; set; } = "herbivore";        // herbivore | carnivore | scavenger
00027:         public int apex_population_threshold { get; set; }          // 0 = not an apex species
00028:         public bool tameable { get; set; }
00029:         public float tameness_chance { get; set; } = 0.15f;
00030:         public List<string> tags { get; set; } = new List<string>();
00031:     }
00032:
00033:     [Serializable]
00034:     public sealed class PredatorPreyEdgeDef
00035:     {
00036:         public string predator_species_id { get; set; } = string.Empty;
00037:         public string prey_species_id { get; set; } = string.Empty;
00038:         public float predation_pressure { get; set; } = 0.05f;      // fraction of prey removed per day
00039:     }
00040:
00041:     [Serializable]
00042:     public sealed class SeasonalMoveDef
00043:     {
00044:         public string species_id { get; set; } = string.Empty;
00045:         public string season_window_id { get; set; } = string.Empty;
00046:         public float move_chance { get; set; } = 0.1f;              // per pack per day in window
00047:     }
00048:
00049:     [Serializable]
00050:     public sealed class WildlifeEcosystemContainer
00051:     {
00052:         public int schema_version { get; set; } = 1;
00053:         public List<FaunaSpeciesDef> species { get; set; } = new List<FaunaSpeciesDef>();
00054:         public List<PredatorPreyEdgeDef> predator_prey { get; set; } = new List<PredatorPreyEdgeDef>();
00055:         public List<SeasonalMoveDef> seasonal_moves { get; set; } = new List<SeasonalMoveDef>();
00056:     }
00057:
00058:     [Serializable]
00059:     public sealed class PressureKeyState
00060:     {
00061:         public string sector_id { get; set; } = string.Empty;
00062:         public string species_id { get; set; } = string.Empty;
00063:         public int pressure { get; set; }
00064:     }
00065:
00066:     [Serializable]
00067:     public sealed class ApexActivityState
00068:     {
00069:         public string species_id { get; set; } = string.Empty;
00070:         public string sector_id { get; set; } = string.Empty;
00071:         public int since_day { get; set; }
00072:         public int until_day { get; set; }
00073:         public bool spotted_reported { get; set; }
00074:     }
00075:
00076:     [Serializable]
00077:     public sealed class DomesticAnimalState
00078:     {
00079:         public string animal_id { get; set; } = string.Empty;
00080:         public string species_id { get; set; } = string.Empty;
00081:         public int tamed_day { get; set; }
00082:         public string caretaker_id { get; set; } = string.Empty;
00083:     }
00084:
00085:     [Serializable]
00086:     public sealed class WildlifeObservation
00087:     {
00088:         public string species_id { get; set; } = string.Empty;
00089:         public string sector_id { get; set; } = string.Empty;
00090:         public int day { get; set; }
00091:         public float confidence { get; set; } = 1f;
00092:     }
00093:
00094:     [Serializable]
00095:     public sealed class WildlifeEcosystemState
00096:     {
00097:         public string system_id { get; set; } = "wildlife_ecosystem";
00098:         public int schema_version { get; set; } = 1;
00099:         public int last_tick_day { get; set; }
00100:         public List<PressureKeyState> pressures { get; set; } = new List<PressureKeyState>();
00101:         public List<string> extinct_species_sectors { get; set; } = new List<string>(); // "sector|species"
00102:         public List<ApexActivityState> apex_activities { get; set; } = new List<ApexActivityState>();
00103:         public List<DomesticAnimalState> domestic_animals { get; set; } = new List<DomesticAnimalState>();
00104:         public List<WildlifeObservation> observations { get; set; } = new List<WildlifeObservation>();
00105:         public int domestic_counter;
00106:     }
00107:
00108:     public sealed class WildlifeEcosystemSystem
00109:     {
00110:         public const string SystemId = "wildlife_ecosystem";
00111:         public const int ExtinctionThreshold = 2;      // remnant pair = locally extinct
00112:         public const int RecolonizationPopulation = 4;
00113:         public const int ApexDurationDays = 5;
00114:         public const int ObservationLogCapacity = 200;
00115:         public const int PressureDecayPerDay = 1;
00116:
00117:         /// <summary>Plan 176 — max per-day probability a pack migrates away from a
00118:         /// full-avoidance (−1.0) hazard sector; scales linearly with avoidance.</summary>
00119:         public const float HazardAvoidanceMigrationChance = 0.35f;
00120:
00121:         private readonly WildlifeEcosystemContainer _catalog = new WildlifeEcosystemContainer();
00122:         private readonly Dictionary<string, FaunaSpeciesDef> _speciesById =
00123:             new Dictionary<string, FaunaSpeciesDef>(StringComparer.Ordinal);
00124:         private readonly WildlifeEcosystemState _state = new WildlifeEcosystemState();
00125:
00126:         public event Action<string, string>? OnWildlifeObserved;             // species, sector
00127:         public event Action<string, string>? OnLocalExtinction;              // species, sector
00128:         public event Action<string, string, string>? OnHazardAvoidanceMigration; // species, fromSector, toSector
00129:         public event Action<string, string>? OnApexPredatorSpotted;          // species, sector
00130:         public event Action<DomesticAnimalState>? OnWildlifeTamed;
00131:         public event Action<string, string, int>? OnWildlifePopulationShifted; // species, sector, delta
00132:
00133:         public string SaveId => SystemId;
00134:         public WildlifeEcosystemState State => _state;
00135:         public WildlifeEcosystemContainer Catalog => _catalog;
00136:         public IReadOnlyList<DomesticAnimalState> DomesticAnimals => _state.domestic_animals;
00137:         public IReadOnlyList<ApexActivityState> ApexActivities => _state.apex_activities;
00138:         public IReadOnlyList<WildlifeObservation> Observations => _state.observations;
00139:
00140:         public void LoadCatalog(WildlifeEcosystemContainer catalog)
00141:         {
00142:             _catalog.species = new List<FaunaSpeciesDef>();
00143:             _catalog.predator_prey = new List<PredatorPreyEdgeDef>();
00144:             _catalog.seasonal_moves = new List<SeasonalMoveDef>();
00145:             _speciesById.Clear();
00146:             if (catalog == null) return;
00147:             _catalog.species.AddRange(catalog.species ?? new List<FaunaSpeciesDef>());
00148:             _catalog.predator_prey.AddRange(catalog.predator_prey ?? new List<PredatorPreyEdgeDef>());
00149:             _catalog.seasonal_moves.AddRange(catalog.seasonal_moves ?? new List<SeasonalMoveDef>());
00150:             _catalog.schema_version = catalog.schema_version;
00151:             foreach (var s in _catalog.species)
00152:                 if (s != null && !string.IsNullOrEmpty(s.id)) _speciesById[s.id] = s;
00153:             _catalog.species.Sort((a, b) => string.CompareOrdinal(a.id, b.id));
00154:             _catalog.predator_prey.Sort((a, b) =>
00155:                 string.CompareOrdinal(a.predator_species_id + "|" + a.prey_species_id,
00156:                     b.predator_species_id + "|" + b.prey_species_id));
00157:         }
00158:
00159:         public FaunaSpeciesDef? Species(string speciesId) =>
00160:             _speciesById.TryGetValue(speciesId ?? string.Empty, out var s) ? s : null;
00161:
00162:         // ------------------------------------------------------------------
00163:         // Derived queries (single population source: migration packs)
00164:         // ------------------------------------------------------------------
00165:
00166:         public int SectorSpeciesPopulation(WildlifeMigrationSystem migration, string sectorId, string speciesId)
00167:         {
00168:             if (migration?.State?.packs == null) return 0;
00169:             int total = 0;
00170:             foreach (var p in migration.State.packs)
00175:         }
00176:
00177:         public bool IsLocallyExtinct(string sectorId, string speciesId) =>
00178:             _state.extinct_species_sectors.Contains(ExtinctionKey(sectorId, speciesId));
00179:
00180:         /// <summary>Density multiplier for the trapping host (plan §8.3): one
00181:         /// population source, so trap checks and the ecosystem can never
00182:         /// diverge. Extinct species contribute nothing.</summary>
00183:         public float SectorDensityMultiplier(WildlifeMigrationSystem migration, string sectorId)
00184:         {
00185:             if (migration?.State?.packs == null) return 0f;
00186:             int population = 0;
00187:             foreach (var p in migration.State.packs)
00188:             {
00189:                 if (p == null || !string.Equals(p.currentSectorId, sectorId, StringComparison.Ordinal)) continue;
00190:                 if (IsLocallyExtinct(sectorId, p.speciesId)) continue;
00191:                 population += p.population;
00192:             }
00193:             return Math.Clamp(population / 15f, 0f, 1.5f);
00194:         }
00195:
00196:         public static string ExtinctionKey(string sectorId, string speciesId) => sectorId + "|" + speciesId;
00197:
00198:         // ------------------------------------------------------------------
00199:         // Pressure (hunting / trapping feedback)
00200:         // ------------------------------------------------------------------
00201:
00202:         public void RecordHuntingPressure(string sectorId, string speciesId, int amount)
00203:         {
00204:             if (amount <= 0) return;
00205:             var key = _state.pressures.Find(p =>
00206:                 p != null && string.Equals(p.sector_id, sectorId, StringComparison.Ordinal)
00216:         }
00217:
00218:         public int PressureOn(string sectorId, string speciesId)
00219:         {
00220:             var key = _state.pressures.Find(p =>
00221:                 p != null && string.Equals(p.sector_id, sectorId, StringComparison.Ordinal)
00222:                 && string.Equals(p.species_id, speciesId, StringComparison.Ordinal));
00225:
00226:         // ------------------------------------------------------------------
00227:         // Observations / bestiary knowledge
00228:         // ------------------------------------------------------------------
00229:
00230:         public void RecordObservation(string speciesId, string sectorId, int day, float confidence = 1f)
00231:         {
00232:             _state.observations.Add(new WildlifeObservation
00233:             {
00234:                 species_id = speciesId,
00235:                 sector_id = sectorId,
00236:                 day = day,
00237:                 confidence = Math.Clamp(confidence, 0f, 1f)
00238:             });
00239:             if (_state.observations.Count > ObservationLogCapacity)
00240:                 _state.observations.RemoveRange(0, _state.observations.Count - ObservationLogCapacity);
00241:             OnWildlifeObserved?.Invoke(speciesId, sectorId);
00242:         }
00243:
00244:         public int ObservationCount(string speciesId)
00245:         {
00246:             int count = 0;
00247:             foreach (var o in _state.observations)
00248:                 if (o != null && string.Equals(o.species_id, speciesId, StringComparison.Ordinal)) count++;
00249:             return count;
00250:         }
00251:
00252:         /// <summary>Knowledge-gated bestiary level (plan §8.23): exact
00253:         /// populations stay hidden until Fully Documented.</summary>
00254:         public string KnowledgeLevel(string speciesId)
00255:         {
00256:             int count = ObservationCount(speciesId);
00257:             if (count >= 6) return "documented";
00258:             if (count >= 3) return "studied";
00259:             if (count >= 1) return "observed";
00260:             return "unknown";
00262:
00263:         // ------------------------------------------------------------------
00264:         // Daily ecology tick (plan §8.10 order: predation → pressure decay →
00265:         // seasonal moves → hazard avoidance → extinction/recolonization → apex)
00266:         // ------------------------------------------------------------------
00267:
00268:         public void TickDay(
00269:             int day,
00270:             WildlifeMigrationSystem migration,
00271:             float outdoorRadModifier,
00272:             string seasonWindowId,
00273:             ISeededRng populationRng,
00274:             ISeededRng migrationRng,
00275:             ISeededRng apexRng,
00276:             IReadOnlyDictionary<string, float>? sectorHazardModifiers = null)
00277:         {
00278:             if (_state.last_tick_day == day) return;
00279:             _state.last_tick_day = day;
00280:             if (migration?.State?.packs == null) return;
00281:
00282:             float radPressure = Math.Clamp(outdoorRadModifier / 250f, 0f, 1.5f);
00283:
00284:             // 1. Predation: thin prey through the ONE population authority,
00285:             //    remnant pair preserved (extinction is a flagged state, not a
00286:             //    wipe). Radiation thins intolerant species the same way.
00287:             foreach (var edge in _catalog.predator_prey)
00288:             {
00289:                 if (edge == null) continue;
00290:                 var sectors = SectorsOfSpecies(migration, edge.prey_species_id);
00291:                 foreach (var sector in sectors)
00292:                 {
00293:                     int prey = SectorSpeciesPopulation(migration, sector, edge.prey_species_id);
00294:                     int removable = Math.Max(0, prey - ExtinctionThreshold);
00295:                     int remove = (int)Math.Round(removable * edge.predation_pressure);
00296:                     if (remove <= 0) continue;
00297:                     migration.ThinSpeciesInSector(edge.prey_species_id, sector, remove, ExtinctionThreshold);
00298:                     OnWildlifePopulationShifted?.Invoke(edge.prey_species_id, sector, -remove);
00299:                 }
00300:             }
00301:
00302:             foreach (var species in _catalog.species)
00303:             {
00304:                 if (species == null || species.radiation_tolerance >= radPressure) continue;
00305:                 foreach (var sector in SectorsOfSpecies(migration, species.id))
00306:                 {
00307:                     // Over-tolerance radiation: deterministic 1/day attrition
00308:                     // for species outside their tolerance band.
00309:                     if (populationRng != null && populationRng.NextDouble() < 0.5)
00310:                     {
00311:                         migration.ThinSpeciesInSector(species.id, sector, 1, ExtinctionThreshold);
00312:                         OnWildlifePopulationShifted?.Invoke(species.id, sector, -1);
00313:                     }
00314:                 }
00315:             }
00316:
00319:             {
00320:                 var p = _state.pressures[i];
00321:                 if (p == null) continue;
00322:                 p.pressure -= PressureDecayPerDay;
00323:                 if (p.pressure <= 0) _state.pressures.RemoveAt(i);
00324:             }
00325:
00326:             // 3. Seasonal moves through the migration authority's own routing.
00327:             if (migrationRng != null)
00328:             {
00329:                 foreach (var move in _catalog.seasonal_moves)
00330:                 {
00331:                     if (move == null || !string.Equals(move.season_window_id, seasonWindowId, StringComparison.Ordinal))
00332:                         continue;
00333:                     foreach (var pack in migration.State.packs)
00334:                     {
00335:                         if (pack == null || !string.Equals(pack.speciesId, move.species_id, StringComparison.Ordinal))
00336:                             continue;
00337:                         if (migrationRng.NextDouble() >= move.move_chance) continue;
00338:                         if (!migration.TryGetNeighbors(pack.currentSectorId, out var neighbors)
00339:                             || neighbors == null || neighbors.Count == 0) continue;
00340:                         int index = migrationRng.Next(0, neighbors.Count);
00341:                         migration.MigratePack(pack.packId, neighbors[index]);
00342:                         break; // one seasonal move per species per day
00343:                     }
00344:                 }
00345:             }
00346:
00347:             // 4. Anomaly hazard avoidance (Plan 176): packs in sectors under
00348:             //    authored avoidance pressure (negative modifier) migrate to a
00349:             //    deterministic neighbor — through the ONE migration authority,
00350:             //    using the ecology's own migration fork. Attraction (positive
00351:             //    modifier) is a v1 no-op; no synthetic spawning.
00352:             if (sectorHazardModifiers != null && migrationRng != null)
00353:             {
00354:                 foreach (var pack in migration.State.packs)
00355:                 {
00356:                     if (pack == null) continue;
00357:                     if (!sectorHazardModifiers.TryGetValue(pack.currentSectorId, out float mod)) continue;
00358:                     float avoidance = Math.Clamp(-mod, 0f, 1f);
00359:                     if (avoidance <= 0f) continue;
00360:                     if (migrationRng.NextDouble() >= avoidance * HazardAvoidanceMigrationChance) continue;
00361:                     if (!migration.TryGetNeighbors(pack.currentSectorId, out var hazardNeighbors)
00362:                         || hazardNeighbors == null || hazardNeighbors.Count == 0) continue;
00363:                     int hazardIndex = migrationRng.Next(0, hazardNeighbors.Count);
00364:                     string fromSector = pack.currentSectorId;
00365:                     migration.MigratePack(pack.packId, hazardNeighbors[hazardIndex]);
00366:                     OnHazardAvoidanceMigration?.Invoke(pack.speciesId, fromSector, hazardNeighbors[hazardIndex]);
00367:                 }
00368:             }
00369:
00370:             // 5. Extinction / recolonization flags over the pack populations.
00371:             var flags = new HashSet<string>(_state.extinct_species_sectors, StringComparer.Ordinal);
00372:             foreach (var sector in AllSectors(migration))
00373:             {
00374:                 foreach (var species in _catalog.species)
00375:                 {
00376:                     if (species == null) continue;
00377:                     int pop = SectorSpeciesPopulation(migration, sector, species.id);
00378:                     string key = ExtinctionKey(sector, species.id);
00379:                     if (pop > 0 && pop <= ExtinctionThreshold && !flags.Contains(key))
00380:                     {
00381:                         flags.Add(key);
00382:                         _state.extinct_species_sectors.Add(key);
00383:                         OnLocalExtinction?.Invoke(species.id, sector);
00384:                     }
00385:                     else if (pop >= RecolonizationPopulation && flags.Contains(key))
00386:                     {
00387:                         flags.Remove(key);
00388:                         _state.extinct_species_sectors.Remove(key);
00389:                     }
00399:             if (apexRng != null)
00400:             {
00401:                 foreach (var species in _catalog.species)
00402:                 {
00403:                     if (species == null || species.apex_population_threshold <= 0) continue;
00404:                     bool already = _state.apex_activities.Exists(a =>
00405:                         a != null && string.Equals(a.species_id, species.id, StringComparison.Ordinal));
00406:                     if (already) continue;
00407:                     int total = GlobalPopulation(migration, species.id);
00408:                     if (total < species.apex_population_threshold) continue;
00409:                     if (apexRng.NextDouble() >= 0.1) continue;
00410:                     string? sector = LargestSectorOf(migration, species.id);
00411:                     if (sector == null) continue;
00412:                     var activity = new ApexActivityState
00413:                     {
00414:                         species_id = species.id,
00415:                         sector_id = sector,
00416:                         since_day = day,
00417:                         until_day = day + ApexDurationDays
00418:                     };
00419:                     _state.apex_activities.Add(activity);
00420:                     OnApexPredatorSpotted?.Invoke(species.id, sector);
00421:                 }
00422:             }
00423:         }
00424:
00425:         private List<string> SectorsOfSpecies(WildlifeMigrationSystem migration, string speciesId)
00426:         {
00427:             var sectors = new List<string>();
00428:             foreach (var p in migration.State.packs)
00429:             {
00430:                 if (p == null || !string.Equals(p.speciesId, speciesId, StringComparison.Ordinal)) continue;
00431:                 if (!sectors.Contains(p.currentSectorId)) sectors.Add(p.currentSectorId);
00432:             }
00433:             sectors.Sort(StringComparer.Ordinal);
00434:             return sectors;
00435:         }
00436:
00437:         private List<string> AllSectors(WildlifeMigrationSystem migration)
00438:         {
00439:             var sectors = new List<string>();
00440:             foreach (var p in migration.State.packs)
00441:                 if (p != null && !sectors.Contains(p.currentSectorId)) sectors.Add(p.currentSectorId);
00442:             sectors.Sort(StringComparer.Ordinal);
00443:             return sectors;
00444:         }
00445:
00446:         private int GlobalPopulation(WildlifeMigrationSystem migration, string speciesId)
00447:         {
00448:             int total = 0;
00449:             foreach (var p in migration.State.packs)
00450:                 if (p != null && string.Equals(p.speciesId, speciesId, StringComparison.Ordinal))
00453:         }
00454:
00455:         private string? LargestSectorOf(WildlifeMigrationSystem migration, string speciesId)
00456:         {
00457:             string? best = null;
00458:             int bestPop = 0;
00459:             var counts = new Dictionary<string, int>(StringComparer.Ordinal);
00460:             foreach (var p in migration.State.packs)
00461:             {
00462:                 if (p == null || !string.Equals(p.speciesId, speciesId, StringComparison.Ordinal)) continue;
00463:                 counts.TryGetValue(p.currentSectorId, out int c);
00464:                 counts[p.currentSectorId] = c + p.population;
00465:             }
00466:             foreach (var kv in counts)
00467:             {
00479:         // ------------------------------------------------------------------
00480:
00481:         public bool CanTame(string speciesId) =>
00482:             Species(speciesId) is { tameable: true };
00483:
00484:         public DomesticAnimalState? TryTame(
00485:             string speciesId, string sectorId, int day, string caretakerId,
00486:             WildlifeMigrationSystem migration, ISeededRng tamingRng)
00487:         {
00488:             var species = Species(speciesId);
00489:             if (species == null || !species.tameable || tamingRng == null) return null;
00490:             if (migration == null
00491:                 || SectorSpeciesPopulation(migration, sectorId, speciesId) <= ExtinctionThreshold)
00492:                 return null;
00493:             if (tamingRng.NextDouble() >= species.tameness_chance) return null;
00494:
00495:             // One unit leaves the wild population through the one authority.
00496:             migration.ThinSpeciesInSector(speciesId, sectorId, 1, ExtinctionThreshold);
00497:             _state.domestic_counter++;
00498:             var animal = new DomesticAnimalState
00499:             {
00500:                 animal_id = $"domestic_{_state.domestic_counter}",
00504:             };
00505:             _state.domestic_animals.Add(animal);
00506:             OnWildlifeTamed?.Invoke(animal);
00507:             return animal;
00508:         }
00509:
00510:         // ------------------------------------------------------------------
00511:         // Save
00512:         // ------------------------------------------------------------------
00513:
00514:         public WildlifeEcosystemState CaptureState()
00515:         {
00516:             var copy = new WildlifeEcosystemState
00517:             {
00518:                 last_tick_day = _state.last_tick_day,
00519:                 domestic_counter = _state.domestic_counter,
00520:                 extinct_species_sectors = new List<string>(_state.extinct_species_sectors),
00521:                 pressures = new List<PressureKeyState>(_state.pressures.Count),
00522:                 apex_activities = new List<ApexActivityState>(_state.apex_activities.Count),
00523:                 domestic_animals = new List<DomesticAnimalState>(_state.domestic_animals.Count),
00524:                 observations = new List<WildlifeObservation>(_state.observations.Count)
00525:             };
00526:             foreach (var p in _state.pressures)
00527:                 copy.pressures.Add(new PressureKeyState
00528:                 { sector_id = p.sector_id, species_id = p.species_id, pressure = p.pressure });
00539:                     tamed_day = d.tamed_day, caretaker_id = d.caretaker_id
00540:                 });
00541:             foreach (var o in _state.observations)
00542:                 copy.observations.Add(new WildlifeObservation
00543:                 {
00544:                     species_id = o.species_id, sector_id = o.sector_id,
00545:                     day = o.day, confidence = o.confidence
00546:                 });
00547:             return copy;
00548:         }
00549:
00550:         public void RestoreState(WildlifeEcosystemState? state)
00551:         {
00552:             if (state == null) return;
00553:             _state.last_tick_day = state.last_tick_day;
00554:             _state.domestic_counter = state.domestic_counter;
00555:             _state.extinct_species_sectors = state.extinct_species_sectors != null
00556:                 ? new List<string>(state.extinct_species_sectors) : new List<string>();
00557:             _state.pressures = new List<PressureKeyState>(state.pressures?.Count ?? 0);
00576:                         tamed_day = d.tamed_day, caretaker_id = d.caretaker_id
00577:                     });
00578:             _state.observations = new List<WildlifeObservation>(state.observations?.Count ?? 0);
00579:             if (state.observations != null)
00580:                 foreach (var o in state.observations)
00581:                     _state.observations.Add(new WildlifeObservation
00582:                     {
00583:                         species_id = o.species_id, sector_id = o.sector_id,
00584:                         day = o.day, confidence = o.confidence
00585:                     });
00586:         }
00587:     }
00588:
00589:     public static class WildlifeEcosystemCatalogLoader
00590:     {
00591:         public const string DefaultFileName = "wildlife_ecosystem.json";
00592:
00593:         public static WildlifeEcosystemContainer Load(
00594:             string dataDir, IFileIO? files = null, IJsonSerializer? json = null)
00595:         {
00596:             files ??= new FileSystemIO();
00597:             json ??= new SystemTextJsonSerializer();
00598:             var path = System.IO.Path.Combine(dataDir ?? string.Empty, DefaultFileName);
00599:             if (!files.FileExists(path)) return new WildlifeEcosystemContainer();
00600:             try
00601:             {
00602:                 var text = files.ReadAllText(path);
00603:                 return json.Deserialize<WildlifeEcosystemContainer>(text) ?? new WildlifeEcosystemContainer();
00604:             }
00605:             catch (Exception)
00606:             {
00607:                 return new WildlifeEcosystemContainer();
00608:             }
00609:         }
00610:
00611:         public static List<string> Validate(WildlifeEcosystemContainer catalog)
00612:         {
00613:             var diags = new List<string>();
00614:             var seen = new HashSet<string>(StringComparer.Ordinal);
00615:             foreach (var s in catalog.species)
00616:             {
00617:                 if (s == null) continue;
00618:                 if (string.IsNullOrEmpty(s.id) || !s.id.StartsWith("species_", StringComparison.Ordinal))
00619:                     diags.Add($"{s.id}: id must use the species_ prefix");
00620:                 else if (!seen.Add(s.id))
00621:                     diags.Add($"{s.id}: duplicate species id");
00622:                 if (s.radiation_tolerance < 0f || s.radiation_tolerance > 1f)
00623:                     diags.Add($"{s.id}: radiation_tolerance must be within [0,1]");
00624:                 if (s.tameness_chance < 0f || s.tameness_chance > 1f)
00625:                     diags.Add($"{s.id}: tameness_chance must be within [0,1]");
00626:             }
00627:             foreach (var e in catalog.predator_prey)
00628:             {
00629:                 if (e == null) continue;
00630:                 if (!seen.Contains(e.predator_species_id))
00631:                     diags.Add($"edge {e.predator_species_id}->{e.prey_species_id}: predator not defined");
00632:                 if (!seen.Contains(e.prey_species_id))
00633:                     diags.Add($"edge {e.predator_species_id}->{e.prey_species_id}: prey not defined");
00634:                 if (string.Equals(e.predator_species_id, e.prey_species_id, StringComparison.Ordinal))
00635:                     diags.Add($"edge {e.predator_species_id}: self-predation is not supported");
00636:                 if (e.predation_pressure < 0f || e.predation_pressure > 0.5f)
00637:                     diags.Add($"edge {e.predator_species_id}->{e.prey_species_id}: pressure must be within [0,0.5]");
00638:             }
00639:             foreach (var m in catalog.seasonal_moves)
00640:             {
00641:                 if (m == null) continue;
00642:                 if (!seen.Contains(m.species_id))
00643:                     diags.Add($"move {m.species_id}: species not defined");
00644:                 if (m.move_chance < 0f || m.move_chance > 1f)
00645:                     diags.Add($"move {m.species_id}: move_chance must be within [0,1]");
00646:             }
00647:             diags.Sort(StringComparer.Ordinal);
00648:             return diags;
00649:         }
00650:     }
00651: }
```


# Appendix — Current Source Detail: `src/Host/WorldHostSession.cs`

### `src/Host/WorldHostSession.cs` — complete current file

- Size: 402 lines / 19415 bytes.
- SHA-256: `b932e2d0b66a2c471eb95d222294abeb1f921424e2d1b157d41c53c32c68a816`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005: using Ashfall.Core;
00006: using Ashfall.Core.Random;
00007: using Ashfall.Core.Shelter;
00008: using Ashfall.Core.World;
00009:
00010: namespace AtomicWar.GodotApp
00011: {
00012:     /// <summary>
00013:     /// Thin Godot-host session for the World port (weather core). Loads the
00014:     /// season profile JSON, ticks the weather clock, persists state. No rules
00015:     /// here — hosts only wire and present.
00016:     /// </summary>
00017:     public sealed class WorldHostSession
00018:     : HostSessionBase{
00019:         public const int DemoSeed = 1234;
00020:
00021:         public WeatherSystem Weather { get; }
00022:         public SkyLayerArmorSystem SkyArmor { get; }
00023:         public WeatherIntelligenceCoordinator WeatherIntelligence { get; }
00024:         public LocationEvolutionSystem LocationEvolution { get; }
00025:         public WildlifeMigrationSystem Wildlife { get; }
00026:         public LandmarkDegradationSystem Landmarks { get; }
00027:         public WastelandMapSystem WastelandMap { get; }
00028:         public DamagedMapSystem? DamagedMap { get; private set; }
00029:         public SeasonProfileDef Profile { get; private set; }
00030:
00031:         /// <summary>Plan 48 weather gate catalog loaded from weather_route_gates.json.</summary>
00032:         public WeatherGateCatalog GateCatalog { get; private set; } = new WeatherGateCatalog();
00033:
00034:         /// <summary>Location atmosphere flavor texts loaded from environmental_atmosphere_expansion.json.</summary>
00035:         public AtmosphereTextSystem AtmosphereTexts { get; } = new AtmosphereTextSystem();
00036:
00037:         /// <summary>Environmental flavor texts loaded from environmental_texts_expansion_05.json.</summary>
00038:         public EnvironmentalTextSystem EnvironmentalTexts { get; } = new EnvironmentalTextSystem();
00039:
00040:         /// <summary>
00041:         /// Seed catalog for the evolving-world trio (loaded once in Create).
00042:         /// Null when no data dir was provided; hosts read the shelter sector
00043:         /// and scarcity goods from here.
00044:         /// </summary>
00045:         public EvolvingWorldSeedContainer? Seeds { get; private set; }
00046:
00047:         public string ShelterSectorId => EvolvingWorldSeeder.ShelterSectorId(Seeds);
00048:
00049:         /// <summary>
00050:         /// Plan 28 Phase 5 (28L) — coarse wildlife sighting for a location,
00051:         /// via its seed-bound sector. Discovery-gated: unknown ground reads
00052:         /// empty. "holding" | "passing" | "" (never a population count).
00053:         /// </summary>
00054:         public string WildlifeSightingFor(string locationId)
00055:         {
00056:             if (Wildlife == null || Seeds?.location_seeds == null) return string.Empty;
00057:             string? sector = null;
00058:             foreach (var seed in Seeds.location_seeds)
00059:                 if (seed != null && string.Equals(seed.location_id, locationId, StringComparison.Ordinal))
00060:                 { sector = seed.sector_id; break; }
00061:             if (string.IsNullOrEmpty(sector)) return string.Empty;
00062:
00063:             int pop = Wildlife.GetSectorPackPopulation(sector);
00064:             if (pop <= 0) return string.Empty;
00065:             return pop >= 8 ? "wildlife holding" : "wildlife passing";
00066:         }
00067:
00068:         /// <summary>28L overview contract — home-sector wildlife band, no counts.</summary>
00069:         public string HomeSectorWildlifeStatus()
00070:         {
00071:             if (Wildlife == null || string.IsNullOrEmpty(ShelterSectorId)) return string.Empty;
00072:             int pop = Wildlife.GetSectorPackPopulation(ShelterSectorId);
00073:             if (pop <= 0) return string.Empty;
00074:             return pop >= 8 ? "Wildlife: herds reported" : "Wildlife: movement reported";
00075:         }
00076:
00077:         public string LastEvent { get; private set; } = string.Empty;
00078:         /// <summary>C2 / Plan 20C — the bound weather-effects authority
00079:         /// (null when no valid data file; consumers fall back to legacy).</summary>
00080:         public WeatherEffectsCatalog? WeatherEffects { get; private set; }
00081:
00082:         public WorldHostSession(
00083:             WeatherSystem weather = null!,
00084:             SkyLayerArmorSystem skyArmor = null!,
00085:             LocationEvolutionSystem locationEvolution = null!,
00086:             WildlifeMigrationSystem wildlife = null!,
00087:             LandmarkDegradationSystem landmarks = null!,
00088:             WastelandMapSystem wastelandMap = null!,
00089:             DamagedMapSystem? damagedMap = null,
00090:             ICampaignRngManager? campaignRng = null)
00091:         {
00092:             int weatherSeed = campaignRng != null
00093:                 ? campaignRng.GetStream(CampaignStreamIds.Weather).DerivedBaseSeed
00094:                 : DemoSeed;
00095:             Weather = weather ?? new WeatherSystem();
00096:             SkyArmor = skyArmor ?? new SkyLayerArmorSystem();
00097:             WeatherIntelligence = new WeatherIntelligenceCoordinator(Weather, SkyArmor, new SeededRng(weatherSeed));
00098:             LocationEvolution = locationEvolution ?? new LocationEvolutionSystem();
00099:             Wildlife = wildlife ?? new WildlifeMigrationSystem();
00100:             Landmarks = landmarks ?? new LandmarkDegradationSystem();
00101:             WastelandMap = wastelandMap ?? WastelandMapCatalogLoader.CreateSystem(string.Empty);
00102:             DamagedMap = damagedMap;
00103:             Weather.OnWeatherChanged += kind =>
00104:             {
00105:                 LastEvent = $"Weather: {kind}";
00106:                 if (IsSevereWeather(kind))
00107:                     AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayWeatherAlert();
00108:                 RaiseStateChanged();
00109:             };
00110:             Weather.OnStateChanged += _ => RaiseStateChanged();
00111:             WeatherIntelligence.OnIntelligenceChanged += () => RaiseStateChanged();
00112:         }
00113:
00114:         public static WorldHostSession Create(string dataDir, ICampaignRngManager? campaignRng = null)
00115:         {
00116:             var mapSystem = !string.IsNullOrEmpty(dataDir)
00117:                 ? WastelandMapCatalogLoader.CreateSystem(dataDir)
00118:                 : null!;
00119:             var session = new WorldHostSession(wastelandMap: mapSystem, campaignRng: campaignRng);
00120:             if (!string.IsNullOrEmpty(dataDir))
00121:             {
00122:                 session.DamagedMap = DamagedMapCatalogLoader.CreateSystem(dataDir, session.WastelandMap);
00123:             }
00124:             var profile = !string.IsNullOrEmpty(dataDir)
00125:                 ? WeatherProfileLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer())
00126:                 : null;
00127:             if (profile != null)
00128:             {
00129:                 session.Profile = profile;
00130:                 int weatherSeed = campaignRng != null
00131:                     ? campaignRng.GetStream(CampaignStreamIds.Weather).DerivedBaseSeed
00132:                     : DemoSeed;
00133:                 session.Weather.BindProfile(profile, weatherSeed);
00134:                 // Plan 28: the same Plan 19 authority paces wildlife abundance.
00135:                 session.Wildlife.BindSeasonProfile(profile);
00136:             }
00137:             // C2 / Plan 20C (§36/§40/§41) — expose the bound weather-effects
00138:             // authority so consumer hosts (trapping, expeditions, caravans)
00139:             // sample the ONE table.
00140:             if (!string.IsNullOrEmpty(dataDir))
00141:             {
00142:                 var effects = WeatherEffectsCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
00143:                 if (effects.LoadedCount > 0)
00144:                 {
00145:                     session.Weather.BindWeatherEffects(effects);
00146:                     session.WeatherEffects = effects;
00147:                 }
00148:             }
00149:             var env = WorldSaveStore.TryLoadEnvelope();
00150:             if (env != null)
00151:             {
00152:                 if (env.State != null) session.Weather.RestoreState(env.State);
00153:                 if (env.SkyArmor != null) session.RestoreSkyArmorSave(env.SkyArmor);
00154:                 if (env.WeatherIntelligence != null) session.WeatherIntelligence.RestoreState(env.WeatherIntelligence);
00155:                 if (env.LocationEvolution != null) session.LocationEvolution.RestoreState(env.LocationEvolution);
00156:                 if (env.Wildlife != null) session.Wildlife.RestoreState(env.Wildlife);
00157:                 if (env.Landmark != null) session.Landmarks.RestoreState(env.Landmark);
00158:                 session.LastEvent = "World state restored from save.";
00159:             }
00160:
00161:             // Evolving-world activation (task 122): load the seed authority,
00162:             // then seed — AFTER restore, and only into empty ledgers, so a
00163:             // restored save is never overwritten by the starting world.
00164:             session.Seeds = !string.IsNullOrEmpty(dataDir)
00165:                 ? EvolvingWorldCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer())
00166:                 : null;
00167:             EvolvingWorldSeeder.Seed(session.LocationEvolution, session.Wildlife, session.Landmarks, session.Seeds);
00168:
00169:             // Seasonal events activation (Plan 19)
00170:             var seasonalEvents = !string.IsNullOrEmpty(dataDir)
00171:                 ? SeasonalEventCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer())
00172:                 : null;
00173:             if (seasonalEvents != null && seasonalEvents.Count > 0)
00174:                 session.WeatherIntelligence.Seasonal.BindDefinitions(seasonalEvents);
00175:
00176:             // Atmosphere / environmental flavor catalogs — consumed by location
00177:             // presentation (expedition/map detail) via FlavorTextForLocation.
00178:             if (!string.IsNullOrEmpty(dataDir))
00179:             {
00180:                 var files = new FileSystemIO();
00181:                 var json = new SystemTextJsonSerializer();
00182:                 session.GateCatalog = WeatherGateCatalogLoader.LoadFromDirectory(dataDir, files, json);
00183:                 session.WeatherIntelligence.Station.GateCatalog = session.GateCatalog;
00184:                 AtmosphereCatalogLoader.LoadAndRegister(session.AtmosphereTexts, dataDir, files, json);
00185:                 EnvironmentalTextCatalogLoader.LoadAndRegister(session.EnvironmentalTexts, dataDir, files, json);
00186:             }
00187:
00188:             var mapSave = WastelandMapSaveStore.TryLoad();
00189:             if (mapSave != null)
00190:                 session.WastelandMap.RestoreState(mapSave);
00191:             OverlayWildlifeMapGraph(session.Wildlife, session.WastelandMap, session.Seeds);
00192:             return session;
00193:         }
00194:
00195:         /// <summary>
00196:         /// Plan 30C — packs also migrate across sectors that the wasteland map
00197:         /// connects through location seeds. Seed neighbors are kept.
00198:         /// </summary>
00199:         private static void OverlayWildlifeMapGraph(
00200:             WildlifeMigrationSystem wildlife,
00201:             WastelandMapSystem map,
00202:             EvolvingWorldSeedContainer? seeds)
00203:         {
00204:             if (wildlife == null || map == null || seeds?.location_seeds == null) return;
00205:             var locToSector = new Dictionary<string, string>(StringComparer.Ordinal);
00206:             for (int i = 0; i < seeds.location_seeds.Count; i++)
00207:             {
00208:                 var seed = seeds.location_seeds[i];
00209:                 if (seed == null || string.IsNullOrEmpty(seed.location_id) || string.IsNullOrEmpty(seed.sector_id))
00210:                     continue;
00211:                 locToSector[seed.location_id] = seed.sector_id;
00212:             }
00213:             if (locToSector.Count == 0) return;
00214:
00215:             var merged = new Dictionary<string, List<string>>(StringComparer.Ordinal);
00216:             foreach (var route in map.Routes)
00217:             {
00218:                 if (route == null) continue;
00219:                 if (!locToSector.TryGetValue(route.From, out string fromSector)) continue;
00220:                 if (!locToSector.TryGetValue(route.To, out string toSector)) continue;
00221:                 if (string.Equals(fromSector, toSector, StringComparison.Ordinal)) continue;
00222:                 if (!merged.TryGetValue(fromSector, out var neighbors))
00223:                 {
00224:                     neighbors = new List<string>();
00225:                     merged[fromSector] = neighbors;
00226:                 }
00227:                 if (!neighbors.Contains(toSector))
00228:                     neighbors.Add(toSector);
00229:             }
00230:             if (merged.Count == 0) return;
00231:
00232:             var links = new List<(string sectorId, List<string> neighbors)>(merged.Count);
00233:             foreach (var kv in merged)
00234:                 links.Add((kv.Key, kv.Value));
00235:             wildlife.MergeSectorAdjacency(links);
00236:         }
00237:
00238:         /// <summary>
00239:         /// Prefer atmosphere catalog text for a location; fall back to environmental texts.
00240:         /// Empty string when neither catalog has an entry (presentation must handle silence).
00241:         /// </summary>
00242:         public string FlavorTextForLocation(string locationId, string? weather = null)
00243:         {
00244:             if (string.IsNullOrEmpty(locationId)) return string.Empty;
00245:
00246:             if (!string.IsNullOrEmpty(weather))
00247:             {
00248:                 var atmWeather = AtmosphereTexts.GetTextForLocationAndWeather(locationId, weather);
00249:                 if (atmWeather != null && !string.IsNullOrEmpty(atmWeather.text))
00250:                     return atmWeather.text;
00251:             }
00252:
00253:             var atm = AtmosphereTexts.GetTextForLocation(locationId);
00254:             if (atm != null && !string.IsNullOrEmpty(atm.text))
00255:                 return atm.text;
00256:
00257:             var env = EnvironmentalTexts.GetTextForLocation(locationId);
00258:             if (env != null && !string.IsNullOrEmpty(env.text))
00259:                 return env.text;
00260:
00261:             return string.Empty;
00262:         }
00263:
00264:         // ── Production Runtime Actions ───────────────────────────────
00265:         public void TickHours(float hours)
00266:         {
00267:             Weather.Tick(hours);
00268:         }
00269:
00270:         // ── Demo actions ─────────────────────────────────────────────
00271:
00272:         public string ForceDemo(WeatherKind kind)
00273:         {
00274:             Weather.ForceWeather(kind);
00275:             return $"Weather forced to {kind}.";
00276:         }
00277:
00278:         public string StatusLine()
00279:         {
00280:             return $"Weather: {Weather.Current} · visibility {Weather.VisibilityFactor:P0} · " +
00281:                    $"outdoor rad {Weather.OutdoorRadModifier:0} · " +
00282:                    $"temp penalty {Weather.TemperaturePenaltyC(Weather.Current):0}°C";
00283:         }
00284:
00285:         // ── Save / Load ──────────────────────────────────────────────
00286:
00287:         public WorldWeatherState CaptureSave() => Weather.CaptureState();
00288:         public void RestoreSave(WorldWeatherState state) => Weather.RestoreState(state);
00289:
00290:         // ── Sky Layer Armor (Exp 11) ────────────────────────────────
00291:
00292:         public string SetSkyArmorDemo(int gridX, string material, float thickness)
00293:         {
00294:             var tier = material switch
00295:             {
00296:                 "dirt" => CeilingMaterialTier.Dirt,
00297:                 "wood" => CeilingMaterialTier.Wood,
00298:                 "concrete" => CeilingMaterialTier.ReinforcedConcrete,
00299:                 "lead" => CeilingMaterialTier.LeadSheeting,
00300:                 "tungsten" => CeilingMaterialTier.TungstenComposite,
00301:                 _ => CeilingMaterialTier.Dirt
00302:             };
00303:             SkyArmor.SetCellArmor(gridX, tier, thickness);
00304:             return $"Sky armor set at grid {gridX}: {tier} ({thickness}m). Attenuation: {SkyArmor.GetAttenuationFactor(gridX):F3}.";
00305:         }
00306:
00307:         public string ImpactDemo(int gridX, float energyMJ)
00308:         {
00309:             bool breached = SkyArmor.EvaluateKineticImpact(gridX, energyMJ, out float damage);
00310:             return breached ? $"BREACH at grid {gridX}! {damage:F1} MJ through." : $"Impact absorbed at grid {gridX}.";
00311:         }
00312:
00313:         public string SkyArmorStatusLine()
00314:         {
00315:             var save = SkyArmor.CaptureState();
00316:             if (save.cells.Count == 0) return "Sky armor: no cells plated";
00317:             return $"Sky armor: {save.cells.Count} cells · avg attenuation {AvgAttenuation():F3}";
00318:         }
00319:
00320:         private float AvgAttenuation()
00321:         {
00322:             var save = SkyArmor.CaptureState();
00323:             if (save.cells.Count == 0) return 1f;
00324:             float sum = 0f;
00325:             foreach (var c in save.cells) sum += SkyArmor.GetAttenuationFactor(c.gridX);
00326:             return sum / save.cells.Count;
00327:         }
00328:
00329:         public SkyArmorSaveState CaptureSkyArmorSave() => SkyArmor.CaptureState();
00330:         public void RestoreSkyArmorSave(SkyArmorSaveState state) => SkyArmor.RestoreState(state);
00331:
00332:         // ── Weather Intelligence (station + orbital telemetry) ────────────
00333:
00334:         public WeatherIntelligenceSaveState CaptureWeatherIntelligenceSave()
00335:             => WeatherIntelligence.CaptureState();
00336:
00337:         public string InstallWeatherStationDemo(int day)
00338:         {
00339:             var r = WeatherIntelligence.Station.Install(day);
00340:             return r.Status == ActionResult.StatusKind.Success
00341:                 ? $"Weather station installed on day {day}."
00342:                 : "Station already installed.";
00343:         }
00344:
00345:         public string CalibrateWeatherStationDemo(int day)
00346:         {
00347:             var r = WeatherIntelligence.Station.Calibrate(day);
00348:             return r.Status == ActionResult.StatusKind.Success
00349:                 ? $"Station calibrated (accuracy {WeatherIntelligence.Station.State.accuracy:P0})."
00350:                 : "Cannot calibrate — station not installed or already calibrated.";
00351:         }
00352:
00353:         public string ActivateOrbitalTelemetryDemo(int day)
00354:         {
00355:             WeatherIntelligence.Orbital.ActivateTelemetry(day);
00356:             return $"Orbital Harrow telemetry activated on day {day}.";
00357:         }
00358:
00359:         public string ScheduleOrbitalImpactDemo(int day, int gridX, float energyMj)
00360:         {
00361:             WeatherIntelligence.Orbital.ScheduleImpact(day, gridX, energyMj);
00362:             return $"Orbital impact scheduled: day {day}, grid {gridX}, {energyMj:F1} MJ. Warning lead: {WeatherIntelligence.Orbital.State.warningLeadDays}d.";
00363:         }
00364:
00365:         public string WeatherIntelligenceStatusLine()
00366:         {
00367:             var rm = WeatherIntelligence.BuildReadModel();
00368:             return rm.advisory;
00369:         }
00370:
00371:         /// <summary>
00372:         /// Hazard weather kinds that warrant an audio alert on transition.
00373:         /// Matches the Core-rollable hazard set: FalloutStorm, BlackRain, Blizzard.
00374:         /// </summary>
00375:         internal static bool IsHazardWeather(WeatherKind kind)
00376:         {
00377:             return kind == WeatherKind.FalloutStorm
00378:                 || kind == WeatherKind.BlackRain
00379:                 || kind == WeatherKind.Blizzard;
00380:         }
00381:
00382:         /// <summary>
00383:         /// C2 / Plan 20C (§43) — data-driven alert parity from the ONE effects
00384:         /// table: a transition cue fires for every SEVERE kind (rad ≥ 60,
00385:         /// visibility ≤ 0.5, or thermal ≤ −10 °C) — previously only three kinds
00386:         /// alerted while equally severe states (GlassStorm, RadHail, IceStorm)
00387:         /// stayed silent. Exactly-once: OnWeatherChanged fires on the
00388:         /// transition edge only. Unbound → the legacy hazard classification.
00389:         /// </summary>
00390:         internal bool IsSevereWeather(WeatherKind kind)
00391:         {
00392:             if (WeatherEffects != null && WeatherEffects.TryGetEffects(kind, out var fx)
00393:                 && fx != null)
00394:             {
00395:                 return fx.outdoor_rad_modifier >= 60f
00396:                     || fx.visibility_modifier <= 0.5f
00397:                     || fx.thermal_load_additive_c <= -10f;
00398:             }
00399:             return IsHazardWeather(kind);
00400:         }
00401:     }
00402: }
```


# Appendix — Current Source Detail: `src/Main.Plans162_165.cs`

### `src/Main.Plans162_165.cs` — bounded current excerpt (618 of 847 lines)

- Size: 847 lines / 40656 bytes.
- SHA-256: `bc9a1f9abdf25d45de5c6d76902d4423d91cd2846802b0f7a48c08a08d6f5d9b`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // ============================================================================
00003: // Main Partial : Main.Plans162_165 — flagship survival layer wiring
00004: // Plan 162     : AgricultureSystem + NutritionDiversitySystem (this file).
00005: // Plans 163-165: wired in later phases onto the same partial.
00006: // Ownership    : the host composes the environment snapshot (weather, power,
00007: //      water treatment), gates every command on the canonical inventory, and
00008: //      applies nutrition-deficiency consequences through NeedsSystem.Modify.
00009: // ============================================================================
00010: using System;
00011: using System.Collections.Generic;
00012: using Ashfall.Core;
00013: using Ashfall.Core.Campaign;
00014: using Ashfall.Core.Farming;
00015: using Ashfall.Core.Inventory;
00016: using Ashfall.Core.Random;
00017: using Ashfall.Core.Survivors;
00018:
00019: namespace AtomicWar.GodotApp
00020: {
00021:     public partial class Main
00022:     {
00023:         private AgricultureHostSession _agriculture = null!;
00024:         private bool _agricultureDirty;
00025:         private UI.FarmingPanel _farmingPanel = null!;
00026:         private bool _farmingPanelBound;
00027:
00028:         // ------------------------------------------------------------------
00029:         // Setup / Save
00030:         // ------------------------------------------------------------------
00031:
00032:         private void SetupAgriculture()
00033:         {
00034:             if (_agriculture != null) return;
00035:             SetupGreenhouse();
00036:             SetupInventory();
00037:
00038:             var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
00039:             var json = new SystemTextJsonSerializer();
00040:             var strainCatalog = CropStrainCatalogLoader.Load(_dataDir, fileIO, json);
00041:             var nutritionCatalog = NutritionProfileCatalogLoader.Load(_dataDir, fileIO, json);
00042:
00043:             var agriSystem = new AgricultureSystem(_greenhouse.System);
00044:             agriSystem.LoadCatalog(strainCatalog);
00045:             var nutrition = new NutritionDiversitySystem();
00046:             nutrition.LoadCatalog(nutritionCatalog);
00047:
00048:             var saved = AgricultureSaveStore.TryLoad();
00049:             if (saved != null)
00050:             {
00051:                 agriSystem.RestoreState(saved.agriculture);
00052:                 nutrition.RestoreState(saved.nutrition);
00053:             }
00054:
00055:             _agriculture = new AgricultureHostSession(agriSystem, nutrition);
00056:             _agriculture.StateChanged += () => _agricultureDirty = true;
00057:
00058:             // The greenhouse owns blight; agriculture only dedupes the narrative
00059:             // episode (one journal entry per outbreak, plan §5.23).
00060:             _greenhouse.System.OnBlightOutbreak += plotIndex => _agriculture.System.NotifyBlightOutbreak(plotIndex);
00061:
00062:             _agriculture.System.OnFirstHarvest += plot =>
00063:                 _journal?.TryAddRawEntry("agriculture_first_harvest",
00064:                     "First harvest under the glass. The bench holds something we grew ourselves.",
00065:                     null!, _simDay);
00066:             _agriculture.System.OnPlotInfested += (plot, pest) =>
00067:                 _journal?.TryAddRawEntry($"agriculture_infestation_{plot}",
00068:                     $"Plot {plot + 1}: {pest.Replace("infestation_", "").Replace('_', ' ')} found on the bench.",
00069:                     null!, _simDay);
00070:             _agriculture.System.OnBlightNarrative += plot =>
00071:                 _journal?.TryAddRawEntry($"agriculture_blight_{plot}",
00072:                     $"Plot {plot + 1}: blight is spreading. Treat it or lose the crop.",
00073:                     null!, _simDay);
00074:
00075:             // Nutrition: record every consumed food item through the canonical
00076:             // inventory consumption path (plan §5.19 — diet, not crop, drives state).
00077:             _inventory.OnConsumed += (survivorId, itemId) =>
00078:             {
00079:                 var def = _inventory.Catalog.Get(itemId);
00080:                 if (def != null && (def.type == ItemType.Food || def.hungerRestore > 0f))
00081:                     _agriculture.Nutrition.RecordMeal(survivorId ?? "", itemId, _simDay);
00082:             };
00083:         }
00084:
00085:         private void SaveAgriculture()
00086:         {
00087:             if (_agriculture == null) return;
00088:             var payload = AgricultureSaveStore.TryCapturePersisted(new AgricultureCampaignState
00089:             {
00090:                 agriculture = _agriculture.System.CaptureState(),
00091:                 nutrition = _agriculture.Nutrition.CaptureState()
00092:             });
00093:             if (!string.IsNullOrEmpty(payload))
00094:             {
00095:                 CaptureSection(AgricultureSaveStore.SectionName, payload);
00096:                 _agricultureDirty = false;
00097:             }
00098:         }
00099:
00100:         // ------------------------------------------------------------------
00101:         // Day tick (called from GreenhouseFoundryDayOwner, phase 2)
00102:         // ------------------------------------------------------------------
00103:
00104:         private AgricultureEnvironmentSnapshot BuildAgricultureEnvironment(int day)
00105:         {
00106:             var weather = _world?.Weather;
00107:             bool lightsPowered = _powerGrid?.System != null && _powerGrid.System.IsRoomPowered("room_greenhouse");
00108:             string season = weather?.GetSeasonForDay(day)?.id ?? "any";
00109:
00110:             // B5–B8 Phase 4 (Plan 64): winter light pressure via the Core
00111:             // pure helper (deterministic, testable). Microclimate compensation
00112:             // queries the Phase 1 capability contract live — never cached.
00113:             int lightingPermille = AgricultureSystem.WinterAdjustedLightPermille(
00114:                 lightsPowered ? 1000 : 0, season,
00115:                 _sharedResearch?.HasCapability("knowledge_greenhouse_microclimate") ?? false,
00116:                 lightsPowered);
00117:
00118:             return new AgricultureEnvironmentSnapshot
00119:             {
00120:                 TemperaturePenaltyC = weather?.GetTemperaturePenaltyCelsius() ?? 0f,
00121:                 OutdoorRadModifier = weather?.OutdoorRadModifier ?? 100f,
00122:                 LightingAvailabilityPermille = lightingPermille,
00123:                 AshContaminationRate = weather != null && weather.Current == WeatherKind.FalloutStorm ? 0.08f : 0.04f,
00124:                 SeasonWindowId = season
00125:             };
00126:         }
00127:
00128:         /// <summary>
00129:         /// One agriculture day: environment snapshot → RNG forks → Core tick
00130:         /// (which ticks the greenhouse growth authority exactly once) →
00131:         /// nutrition evaluation → deficiency consequences via NeedsSystem.
00132:         /// </summary>
00133:         private void TickAgricultureDay(int day)
00134:         {
00135:             SetupAgriculture();
00136:             var env = BuildAgricultureEnvironment(day);
00137:             var pestRng = _campaignDay != null
00138:                 ? _campaignDay.Rng.Fork(CampaignStreamIds.AgriculturePest, day, 0)
00139:                 : new SeededRng(1620 + day);
00140:             var mutationRng = _campaignDay != null
00141:                 ? _campaignDay.Rng.Fork(CampaignStreamIds.AgricultureMutation, day, 0)
00142:                 : new SeededRng(1621 + day);
00143:             _agriculture.System.TickDay(day, env, pestRng, mutationRng, env.SeasonWindowId);
00144:
00145:             _agriculture.Nutrition.TickDay(day);
00146:             ApplyNutritionDeficiencyModifiers();
00147:             _farmingPanel?.RefreshView();
00148:         }
00149:
00150:         /// <summary>Deficiency pressure surfaces through the canonical needs
00151:         /// authority: each deficient category wears on morale (max -3/day).</summary>
00152:         private void ApplyNutritionDeficiencyModifiers()
00153:         {
00154:             if (_survivors?.Needs == null) return;
00155:             foreach (var s in _survivors.Needs.Registered)
00156:             {
00157:                 if (s == null) continue;
00158:                 float pressure = _agriculture.Nutrition.DeficiencyPressure(s.Id);
00159:                 if (pressure > 0f)
00160:                     _survivors.Needs.Modify(s.Id, NeedKind.Morale, Math.Min(3f, pressure * 3f));
00161:             }
00163:
00164:         // ------------------------------------------------------------------
00165:         // Player actions (routed from FarmingPanel via Main.UiPanels wiring)
00166:         // ------------------------------------------------------------------
00167:
00168:         private void HandleAgricultureAction(string action, string param)
00169:         {
00207:                         if (!sys.CanPlantStrain(pi, strainId)) { feedback = "Plot occupied or invalid."; break; }
00208:                         if (!inv.HasSufficient(strain.seed_item_id, 1)) { feedback = $"No {strain.seed_item_id} in inventory."; break; }
00209:                         if (inv.TryConsume(strain.seed_item_id, 1))
00210:                             ok = sys.PlantWithStrain(pi, strainId, day);
00211:                     }
00212:                     break;
00213:                 }
00219:                     if (plot < 0) break;
00220:                     if (!inv.HasSufficient(waterItem, 5)) { feedback = $"Needs 5 × {waterItem}."; break; }
00221:                     if (inv.TryConsume(waterItem, 5))
00222:                     {
00223:                         var band = tainted
00224:                             ? AgriWaterBand.Unsafe
00225:                             : (_waterTreatment != null && _waterTreatment.System.State.incomingContaminationLevel >= 0.5f
00226:                                 ? AgriWaterBand.Marginal
00227:                                 : AgriWaterBand.Clean);
00228:                         sys.Water(plot, band);
00229:                         ok = true;
00236:                     if (!sys.CanTreatPest(plot, "item_pest_treatment_dust")) { feedback = "No treatable infestation."; break; }
00237:                     if (!inv.HasSufficient("item_pest_treatment_dust", 1)) { feedback = "No pest treatment dust held."; break; }
00238:                     if (inv.TryConsume("item_pest_treatment_dust", 1))
00239:                         ok = sys.TryTreatPestInfestation(plot, "item_pest_treatment_dust");
00240:                     break;
00241:                 }
00242:                 case "COMPOST_START": // param: recipeId
00246:                     if (!inv.HasSufficient(recipe.input_item_id, recipe.input_count))
00247:                     { feedback = $"Needs {recipe.input_count} × {recipe.input_item_id}."; break; }
00248:                     if (inv.TryConsume(recipe.input_item_id, recipe.input_count))
00249:                         ok = sys.TryStartCompostBatch(recipe.id, day);
00250:                     break;
00251:                 }
00252:                 case "COMPOST_COLLECT": // param: recipeId
00266:                     if (plot < 0) break;
00267:                     if (!inv.HasSufficient("item_compost_humus", 1)) { feedback = "No compost humus held."; break; }
00268:                     if (inv.TryConsume("item_compost_humus", 1))
00269:                         ok = sys.TryApplyCompost(plot);
00270:                     break;
00271:                 }
00272:                 case "HARVEST":
00300:         // ==================================================================
00301:
00302:         private DefenseHostSession _defense = null!;
00303:         private bool _defenseDirty;
00304:         private UI.DefenseGridPanel _defenseGridPanel = null!;
00305:         private bool _defenseGridPanelBound;
00306:
00310:             SetupInventory();
00311:
00312:             var traps = Ashfall.Core.Defense.TrapCatalogLoader.Load(_dataDir);
00313:             var system = new Ashfall.Core.Defense.DefenseSystem(traps);
00314:             var saved = DefenseSaveStore.TryLoad();
00315:             if (saved != null)
00316:                 system.RestoreState(saved);
00317:
00318:             _defense = new DefenseHostSession(system);
00319:             // Plan 203: attach the perimeter authority when it already exists
00320:             // (defense setup can run before or after perimeter setup).
00321:             if (_perimeterDefense != null) _defense.AttachPerimeter(_perimeterDefense);
00322:             _defense.StateChanged += () => _defenseDirty = true;
00323:
00324:             // Capture handoff: DefenseSystem only reports the fact; the
00325:             // PrisonerSystem is the single captive authority (plan §6.13-6.14).
00326:             _defense.OnCaptivesToHandOff += (day, count) =>
00327:             {
00328:                 var prisoners = EnsurePrisoners();
00329:                 int taken = 0;
00330:                 for (int i = 0; i < count; i++)
00331:                 {
00332:                     string captiveId = $"captive_raid_{day}_{i + 1}";
00333:                     if (prisoners.TakePrisoner(captiveId, "faction_iron_raiders", day))
00334:                         taken++;
00335:                 }
00336:                 _journal?.TryAddRawEntry($"raid_captures_{day}",
00337:                     taken > 0
00338:                         ? $"{taken} raider(s) taken alive from the pit and moved to the cells."
00339:                         : "The pit held raiders, but the cells are full — they were turned loose at the wire.",
00340:                     null!, _simDay);
00341:             };
00342:
00343:             _defense.System.OnTrapSprung += (installationId, trapId) =>
00344:                 _journal?.TryAddRawEntry($"trap_sprung_{installationId}",
00345:                     $"A {trapId.Replace("trap_", "").Replace('_', ' ')} sprang at the wire.",
00346:                     null!, _simDay);
00347:         }
00348:
00349:         private void SaveDefense()
00350:         {
00351:             if (_defense == null) return;
00352:             var payload = DefenseSaveStore.TryCapturePersisted(_defense.System.CaptureState());
00353:             if (!string.IsNullOrEmpty(payload))
00354:             {
00355:                 CaptureSection(DefenseSaveStore.SectionName, payload);
00356:                 _defenseDirty = false;
00357:             }
00358:         }
00359:
00360:         /// <summary>
00361:         /// Plan 163 pre-combat phase: static defenses (traps, then perimeter
00362:         /// emplacements) resolve BEFORE direct survivor combat. Returns the
00363:         /// engagement; the caller escalates only what breached.
00364:         /// </summary>
00365:         private Ashfall.Core.Defense.DefenseEngagementResult ResolveRaidDefenses(
00366:             int day, int raiderStrength, bool isNight)
00367:         {
00368:             SetupDefense();
00369:             var targetingRng = _campaignDay != null
00370:                 ? _campaignDay.Rng.Fork(CampaignStreamIds.DefenseTargeting, day, 0)
00371:                 : new SeededRng(1630 + day);
00372:             var captureRng = _campaignDay != null
00373:                 ? _campaignDay.Rng.Fork(CampaignStreamIds.DefenseCapture, day, 0)
00374:                 : new SeededRng(1631 + day);
00375:             // Turrets draw from the grid: emplacements are exterior hardware.
00376:             // Plan 71: automated emplacements additionally draw through the
00377:             // room_armory_munitions circuit — while it is shed or tripped the
00378:             // turrets freeze even if the grid as a whole is healthy.
00379:             // B5–B8 Phase 7: allocation-aware served state (was the global-
00380:             // outage IsRoomPowered read) — during a brownout the armory
00381:             // circuit stays live while generation covers it, and sheds by
00382:             // priority otherwise. Same migration the sump pump received.
00383:             // CORE-MECH W5: the raid resolver is the single authority (DP-CM-2). We
00384:             // publish its result to the defense surface and the journal so the
00385:             // player can see what their fortifications actually did — the mechanic
00386:             // was already live; this makes it legible.
00387:             _defense.EmplacementPoweredProvider =
00388:                 id => _powerGrid?.System?.IsRoomServed("room_armory_munitions") ?? false;
00389:
00390:             var engagement = _defense.System.ResolvePreCombatRaid(
00391:                 day, raiderStrength, isNight,
00392:                 _perimeterDefense,
00393:                 id => (_powerGrid?.System?.IsRoomServed("room_armory_munitions") ?? false),
00394:                 targetingRng, captureRng,
00395:                 // Plan 174 — guard animals improve night detection (§5.10):
00396:                 // the authored guard rating normalized to a 0..1 fraction.
00397:                 _companions != null ? Math.Clamp(_companions.GetGuardModifierTotal() / 100f, 0f, 1f) : 0f);
00398:
00399:             _defense.RecordEngagement(engagement);
00400:             SetupJournal();
00401:             _journal?.TryAddRawEntry(
00402:                 $"raid_defenses_{day}_{raiderStrength}",
00403:                 engagement.Repelled
00404:                     ? $"Raiders ({raiderStrength}) turned back at the perimeter — {engagement.RaidersNeutralizedByTraps} down, {engagement.RaidersCaptured} captured."
00405:                     : $"Perimeter breached — {engagement.RemainingRaiders} raiders reached the shelter ({engagement.RaidersNeutralizedByTraps} stopped, {engagement.RaidersCaptured} captured).",
00406:                 null!, day);
00407:             return engagement;
00408:         }
00409:
00434:             string? feedback = null;
00435:
00436:             // param shapes: INSTALL:<trapId>|<placement> or <installationId>
00437:             int colon = param?.IndexOf(':') ?? -1;
00438:             string arg = colon > 0 ? param!.Substring(colon + 1) : param ?? "";
00439:
00440:             switch (action)
00453:                             if (!inv.HasSufficient(cost.Key, cost.Value)) { costsOk = false; feedback = $"Needs {cost.Value}× {cost.Key}."; break; }
00454:                         if (!costsOk) break;
00455:                         ok = sys.InstallTrap(trapId, placement, (item, n) => inv.TryConsume(item, n));
00456:                     }
00457:                     break;
00458:                 }
00459:                 case "RESET":
00466:                         if (!inv.HasSufficient(cost.Key, cost.Value)) { costsOk = false; feedback = $"Needs {cost.Value}× {cost.Key}."; break; }
00467:                     if (!costsOk) break;
00468:                     ok = sys.TryResetTrap(arg, (item, n) => inv.TryConsume(item, n));
00469:                     break;
00470:                 }
00471:                 case "REPAIR":
00472:                 {
00478:                         if (!inv.HasSufficient(cost.Key, cost.Value)) { costsOk = false; feedback = $"Needs {cost.Value}× {cost.Key}."; break; }
00479:                     if (!costsOk) break;
00480:                     ok = sys.TryRepairTrap(arg, (item, n) => inv.TryConsume(item, n));
00481:                     break;
00482:                 }
00483:                 case "SIMULATE": // manual drill: resolve against a dummy squad
00484:                 {
00485:                     var result = ResolveRaidDefenses(_simDay, int.TryParse(arg, out var s) && s > 0 ? Math.Min(s, 12) : 4, isNight: false);
00486:                     ok = true;
00487:                     _journal?.TryAddRawEntry($"defense_drill_{_simDay}",
00488:                         $"Defense drill against {result.InitialRaiderStrength} simulated raiders: {(result.Repelled ? "repelled." : $"breached with {result.RemainingRaiders} through.")}",
00489:                         null!, _simDay);
00490:                     break;
00491:                 }
00492:                 case "BUILD": // B5–B8 Phase 7 (Plan 67 §10.5): emplacement construction
00493:                 {
00494:                     var def = _perimeterDefense!.FindDefinition(arg);
00495:                     if (def == null) { feedback = "Unknown emplacement."; break; }
00496:                     // Live capability query — research is permission, never a
00498:                     bool capability = string.IsNullOrEmpty(def.required_knowledge)
00499:                         || (_sharedResearch?.HasCapability(def.required_knowledge) ?? false);
00500:                     var construct = _perimeterDefense!.ConstructEmplacement(def.defense_id, capability);
00501:                     ok = construct.IsSuccess;
00502:                     if (!ok) feedback = construct.MessageKey;
00503:                     break;
00504:                 }
00505:             }
00506:
00514:         // ==================================================================
00515:
00516:         private PsychologyArcHostSession _psychologyArcs = null!;
00517:         private bool _psychologyArcsDirty;
00518:         private UI.PsychologyArcPanel _psychologyArcPanel = null!;
00519:         private bool _psychologyArcPanelBound;
00520:
00525:             SetupInventory();
00526:
00527:             var catalog = Ashfall.Core.Survivors.MentalArcCatalogLoader.Load(_dataDir);
00528:             var system = new Ashfall.Core.Survivors.PsychologicalArcSystem(catalog.arcs);
00529:             var saved = PsychologyArcSaveStore.TryLoad();
00530:             if (saved != null)
00531:                 system.RestoreState(saved);
00532:
00533:             _psychologyArcs = new PsychologyArcHostSession(system);
00534:             _psychologyArcs.StateChanged += () => _psychologyArcsDirty = true;
00535:
00536:             // Hoarding: host picks the item deterministically (first low-value
00537:             // staple in slot order) and moves it out of the shared inventory.
00538:             // Refusal returns (string, int)? null — nothing ever vanishes.
00539:             system.TryTransferToStash = (survivorId, _, day) =>
00540:             {
00541:                 var inv = _inventory.Inventory;
00542:                 if (inv.CountById("canned_food") > 2 && inv.TryConsume("canned_food", 1))
00543:                     return ("canned_food", 1);
00544:                 return null;
00545:             };
00546:
00547:             system.OnBreakdownArcStarted += (survivorId, arcId) =>
00548:             {
00549:                 // A first canonical breakdown is the existing trauma authority's
00550:                 // producer for the scarred-state narrative gate. The flag is
00551:                 // monotonic and provenance-backed; EchoSystem only reads it.
00552:                 _consequenceLedger.Set(
00553:                     "scarred_state",
00554:                     Ashfall.Core.Survivors.PsychologicalArcSystem.SystemId,
00555:                     $"breakdown:{survivorId}:{arcId}",
00556:                     _simDay);
00557:                 _journal?.TryAddRawEntry($"arc_started_{survivorId}",
00558:                     $"{survivorId} is not holding together — {arcId.Replace("arc_", "").Replace('_', ' ')} taking hold.",
00559:                     null!, _simDay);
00560:             };
00561:             system.OnBreakdownEscalated += (survivorId, arcId, from, to) =>
00562:                 _journal?.TryAddRawEntry($"arc_stage_{survivorId}_{to}",
00563:                     $"{survivorId}'s crisis deepened ({from} → {to}).",
00564:                     null!, _simDay);
00565:             system.OnUnsafeFireIncidentRequested += (survivorId, day) =>
00566:             {
00567:                 // The fire authority owns ignition and damage (plan §7.11).
00568:                 var fire = ShelterFireSession?.System;
00569:                 if (fire == null) return;
00570:                 var incidentId = $"arc_fire_{survivorId}_{day}";
00571:                 fire.Ignite(incidentId, "room_bunker_corridor", day, new List<Ashfall.Core.Shelter.FireZoneState>());
00572:                 _journal?.TryAddRawEntry(incidentId,
00573:                     $"A small fire started near {survivorId}'s bunk. It was put out. Nobody said much after.",
00574:                     null!, _simDay);
00575:             };
00576:             system.OnBreakdownBehaviorOccurred += (survivorId, behavior) =>
00577:             {
00578:                 if (behavior == Ashfall.Core.Survivors.ArcBehavior.RefuseAssignment)
00579:                 {
00580:                     // Canonical needs + relations authorities carry the effect.
00581:                     _survivors?.Needs.Modify(survivorId, NeedKind.Morale, 3f);
00582:                     string other = FirstOtherSurvivor(survivorId);
00583:                     if (other != null && _survivorRelationsCore != null)
00584:                         _survivorRelationsCore.ModifyAffinity(survivorId, other, -5f);
00585:                 }
00586:                 else if (behavior == Ashfall.Core.Survivors.ArcBehavior.WithdrawSelfCare)
00587:                 {
00588:                     // Plan 24B (A1): the self-care lapse routes through the
00589:                     // shared attributed seam — same +8 hygiene (higher =
00590:                     // worse), now named in the contributor display.
00591:                     _survivors?.Needs.ApplyAttributedDelta(
00592:                         survivorId, NeedKind.Hygiene, 8f,
00593:                         "hygiene.self_care_withdrawn");
00594:                 }
00595:             };
00596:             system.OnStashDiscovered += survivorId =>
00597:                 _journal?.TryAddRawEntry($"stash_found_{survivorId}",
00598:                     $"A hidden stash was found in {survivorId}'s things.",
00599:                     null!, _simDay);
00600:         }
00608:         }
00609:
00610:         private void SavePsychologyArcs()
00611:         {
00612:             if (_psychologyArcs == null) return;
00613:             var payload = PsychologyArcSaveStore.TryCapturePersisted(_psychologyArcs.System.CaptureState());
00614:             if (!string.IsNullOrEmpty(payload))
00615:             {
00616:                 CaptureSection(PsychologyArcSaveStore.SectionName, payload);
00617:                 _psychologyArcsDirty = false;
00618:             }
00619:         }
00620:
00621:         /// <summary>Phase-4 day tick — after needs (phase 3) are finalized.</summary>
00622:         private void TickPsychologyArcsDay(int day)
00623:         {
00624:             SetupPsychologyArcs();
00625:             if (_survivors?.Needs == null) return;
00626:             var ids = new List<string>();
00630:             float ReadStress(string id)
00631:             {
00632:                 var s = _survivors.Needs.Get(id);
00633:                 return s?.Morale ?? 0f; // canonical stress proxy (higher = worse)
00634:             }
00635:
00636:             var triggerRng = _campaignDay != null
00637:                 ? _campaignDay.Rng.Fork(CampaignStreamIds.PsychologyArcTrigger, day, 0)
00643:                 ? _campaignDay.Rng.Fork(CampaignStreamIds.PsychologyRecovery, day, 0)
00644:                 : new SeededRng(1642 + day);
00645:             _psychologyArcs.System.TickDay(day, ids, ReadStress, triggerRng, behaviorRng, recoveryRng);
00646:             _psychologyArcPanel?.RefreshView();
00647:         }
00648:
00649:         private void HandlePsychologyAction(string action, string param)
00699:                     if (all)
00700:                     {
00701:                         sys.ConfirmStashReturned(param);
00702:                         _psychologyArcs.MarkDirty($"{param}'s stash returned to stores.");
00703:                     }
00704:                     else _psychologyArcs.MarkDirty("Return blocked — stores cannot take the items.");
00705:                     break;
00713:         // ==================================================================
00714:
00715:         private WildlifeEcosystemHostSession _wildlifeEcosystem = null!;
00716:         private bool _wildlifeEcosystemDirty;
00717:         private UI.BestiaryPanel _bestiaryPanel = null!;
00718:         private bool _bestiaryPanelBound;
00719:
00724:
00725:             var system = new Ashfall.Core.World.WildlifeEcosystemSystem();
00726:             system.LoadCatalog(Ashfall.Core.World.WildlifeEcosystemCatalogLoader.Load(_dataDir));
00727:             var saved = WildlifeEcosystemSaveStore.TryLoad();
00728:             if (saved != null)
00729:                 system.RestoreState(saved);
00730:
00731:             _wildlifeEcosystem = new WildlifeEcosystemHostSession(system);
00732:             _wildlifeEcosystem.StateChanged += () => _wildlifeEcosystemDirty = true;
00733:
00734:             system.OnApexPredatorSpotted += (species, sector) =>
00735:                 _journal?.TryAddRawEntry($"apex_{species}_{sector}",
00736:                     $"Something big is working {sector}: {species.Replace("species_", "").Replace('_', ' ')}. Keep the parties armed.",
00737:                     null!, _simDay);
00738:             system.OnLocalExtinction += (species, sector) =>
00739:                 _journal?.TryAddRawEntry($"extinct_{species}_{sector}",
00740:                     $"No more {species.Replace("species_", "").Replace('_', ' ')} around {sector}. The traps will come up empty.",
00741:                     null!, _simDay);
00742:             system.OnWildlifeTamed += a =>
00743:                 _journal?.TryAddRawEntry($"tamed_{a.animal_id}",
00744:                     $"A {a.species_id.Replace("species_", "").Replace('_', ' ')} was tamed and moved into the pens.",
00745:                     null!, _simDay);
00746:             // Plan 176 — anomaly-driven avoidance migrations journal through the
00747:             // ecology system's own typed event.
00748:             system.OnHazardAvoidanceMigration += (species, fromSector, toSector) =>
00749:                 _journal?.TryAddRawEntry($"wildlife_avoid_{species}_{fromSector}",
00750:                     $"{species.Replace("species_", "").Replace('_', ' ')} packs are leaving {fromSector} — something in that sector is wrong.",
00751:                     null!, _simDay);
00752:         }
00753:
00754:         private void SaveWildlifeEcosystem()
00755:         {
00756:             if (_wildlifeEcosystem == null) return;
00757:             var payload = WildlifeEcosystemSaveStore.TryCapturePersisted(_wildlifeEcosystem.System.CaptureState());
00758:             if (!string.IsNullOrEmpty(payload))
00759:             {
00760:                 CaptureSection(WildlifeEcosystemSaveStore.SectionName, payload);
00761:                 _wildlifeEcosystemDirty = false;
00762:             }
00763:         }
00764:
00765:         /// <summary>Ecology day — ticks inside EvolvingWorldDayOwner right after
00766:         /// the migration authority moved the packs (single population source).</summary>
00767:         private void TickWildlifeEcosystemDay(int day)
00768:         {
00769:             SetupWildlifeEcosystem();
00770:             var world = _world;
00771:             if (world?.Wildlife == null) return;
00772:             string season = world.Weather?.GetSeasonForDay(day)?.id ?? "any";
00773:             float rad = world.Weather?.OutdoorRadModifier ?? 100f;
00774:
00775:             var popRng = _campaignDay != null
00776:                 ? _campaignDay.Rng.Fork(CampaignStreamIds.WildlifePopulation, day, 0) : new SeededRng(1650 + day);
00777:             var migRng = _campaignDay != null
00778:                 ? _campaignDay.Rng.Fork(CampaignStreamIds.WildlifeMigration, day, 0) : new SeededRng(1651 + day);
00779:             var apexRng = _campaignDay != null
00780:                 ? _campaignDay.Rng.Fork(CampaignStreamIds.WildlifeApex, day, 0) : new SeededRng(1652 + day);
00781:             // Plan 176 — anomaly hazard modifiers feed wildlife avoidance through
00782:             // the migration authority; null keeps legacy behavior unchanged.
00783:             var hazardModifiers = BuildSectorHazardModifiers();
00784:             _wildlifeEcosystem.System.TickDay(day, world.Wildlife, rad, season, popRng, migRng, apexRng, hazardModifiers);
00785:             _bestiaryPanel?.RefreshView();
00786:         }
00787:
00788:         private void HandleWildlifeAction(string action, string param)
00789:         {
00790:             if (action == "OPEN")
00791:             {
00792:                 SetupWildlifeEcosystem();
00793:                 if (!_bestiaryPanelBound && _wildlifeEcosystem != null)
00794:                 {
00795:                     _bestiaryPanel.Bind(_wildlifeEcosystem, () => _world?.Wildlife);
00796:                     _bestiaryPanelBound = true;
00797:                 }
00798:                 _bestiaryPanel.SetWorldSector(_world?.ShelterSectorId ?? "");
00799:                 _bestiaryPanel.Open();
00800:                 return;
00801:             }
00802:             if (action == "CLOSE")
00803:             {
00804:                 _bestiaryPanel.Close();
00805:                 return;
00806:             }
00807:
00808:             if (_wildlifeEcosystem == null) return;
00809:             var world = _world;
00810:             if (world?.Wildlife == null) return;
00811:
00812:             switch (action)
00813:             {
00814:                 case "TAME": // param "speciesId|sectorId|caretakerId"
00815:                 {
00816:                     var parts = (param ?? "").Split('|');
00817:                     if (parts.Length < 3) { _wildlifeEcosystem.MarkDirty("Malformed tame request."); break; }
00818:                     var tamingRng = _campaignDay != null
00819:                         ? _campaignDay.Rng.Fork(CampaignStreamIds.WildlifeTaming, _simDay, 0)
00820:                         : new SeededRng(1653 + _simDay);
00821:                     var animal = _wildlifeEcosystem.System.TryTame(
00822:                         parts[0], parts[1], _simDay, parts[2], world.Wildlife, tamingRng);
00823:                     if (animal == null)
00824:                         _wildlifeEcosystem.MarkDirty($"Could not tame a {parts[0]} here.");
00825:                     else
00826:                     {
00827:                         // Plan 174 — the taming record becomes a persistent
00828:                         // companion under the companion authority (identity =
00829:                         // the wildlife animal_id, never duplicated).
00830:                         SetupCompanionAnimals();
00831:                         _companions?.RegisterCompanion(animal.animal_id, animal.species_id, animal.tamed_day, null);
00832:                     }
00833:                     break;
00834:                 }
00835:                 case "OBSERVE": // param speciesId — records a sighting from the field
00836:                 {
00837:                     if (string.IsNullOrEmpty(param)) break;
00838:                     string sector = _world.ShelterSectorId ?? "";
00839:                     _wildlifeEcosystem.System.RecordObservation(param, sector, _simDay, 1f);
00840:                     _wildlifeEcosystem.MarkDirty($"Observed {param.Replace("species_", "").Replace('_', ' ')} near {sector}.");
00841:                     break;
00842:                 }
00843:             }
00844:             _bestiaryPanel?.RefreshView();
00845:         }
00846:     }
00847: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/WildlifeEcosystemSystemTests.cs`

### `Ashfall.Core.Tests/WildlifeEcosystemSystemTests.cs` — complete current file

- Size: 303 lines / 15091 bytes.
- SHA-256: `4f728436468477c236b778ff1449c548ebafb814264ed84e5b8ea09feabf0563`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // Plan 165 — WildlifeEcosystemSystem tests: single population authority,
00003: // predation/radiation pressure, extinction/recolonization, apex, taming
00004: // transfer, knowledge gating, persistence equivalence.
00005: using System;
00006: using System.Collections.Generic;
00007: using System.IO;
00008: using Ashfall.Core;
00009: using Ashfall.Core.World;
00010: using Xunit;
00011:
00012: namespace Ashfall.Core.Tests
00013: {
00014:     public sealed class WildlifeEcosystemSystemTests
00015:     {
00016:         private static string FindDataDir()
00017:         {
00018:             string dataDir;
00019:             if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir))
00020:                 CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
00021:             return dataDir ?? string.Empty;
00022:         }
00023:
00024:         private static WildlifeEcosystemContainer ShippedCatalog() =>
00025:             WildlifeEcosystemCatalogLoader.Load(FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
00026:
00027:         private static WildlifeMigrationSystem SeededMigration()
00028:         {
00029:             var m = new WildlifeMigrationSystem(new SeededRng(42));
00030:             m.RegisterPack("pack_hare_a", "species_cotton_hare", "sector_4_hinterlands", 8);
00031:             m.RegisterPack("pack_hare_b", "species_cotton_hare", "sector_2_north_ridge", 6);
00032:             m.RegisterPack("pack_wolf_a", "species_wolf", "sector_4_hinterlands", 12);
00033:             m.RegisterPack("pack_goat_a", "species_feral_goat", "sector_2_north_ridge", 7);
00034:             m.RegisterPack("pack_rat_a", "species_blight_rat", "sector_4_hinterlands", 9);
00035:             m.SetSectorAdjacency(new[]
00036:             {
00037:                 ("sector_4_hinterlands", new List<string> { "sector_2_north_ridge" }),
00038:                 ("sector_2_north_ridge", new List<string> { "sector_4_hinterlands" })
00039:             });
00040:             return m;
00041:         }
00042:
00043:         private static WildlifeEcosystemSystem MakeEcosystem(WildlifeEcosystemContainer catalog) =>
00044:             new WildlifeEcosystemSystem().Also(e => e.LoadCatalog(catalog));
00045:
00046:         [Fact]
00047:         public void ShippedCatalog_Validates()
00048:         {
00049:             var catalog = ShippedCatalog();
00050:             Assert.True(catalog.species.Count >= 12, $"expected 12 species, got {catalog.species.Count}");
00051:             Assert.Empty(WildlifeEcosystemCatalogLoader.Validate(catalog));
00052:         }
00053:
00054:         [Fact]
00055:         public void CatalogValidation_RejectsBadEntries()
00056:         {
00057:             var bad = new WildlifeEcosystemContainer();
00058:             bad.species.Add(new FaunaSpeciesDef { id = "species_dup" });
00059:             bad.species.Add(new FaunaSpeciesDef { id = "species_dup" });
00060:             bad.species.Add(new FaunaSpeciesDef { id = "species_prey_ok" });
00061:             bad.predator_prey.Add(new PredatorPreyEdgeDef
00062:             { predator_species_id = "species_missing", prey_species_id = "species_prey_ok" });
00063:             bad.predator_prey.Add(new PredatorPreyEdgeDef
00064:             { predator_species_id = "species_prey_ok", prey_species_id = "species_prey_ok" });
00065:             var diags = WildlifeEcosystemCatalogLoader.Validate(bad);
00066:             Assert.Contains(diags, d => d.Contains("duplicate"));
00067:             Assert.Contains(diags, d => d.Contains("predator not defined"));
00068:             Assert.Contains(diags, d => d.Contains("self-predation"));
00069:         }
00070:
00071:         [Fact]
00072:         public void Populations_DeriveFromTheSingleAuthority()
00073:         {
00074:             var eco = MakeEcosystem(ShippedCatalog());
00075:             var m = SeededMigration();
00076:             Assert.Equal(8, eco.SectorSpeciesPopulation(m, "sector_4_hinterlands", "species_cotton_hare"));
00077:             Assert.Equal(6, eco.SectorSpeciesPopulation(m, "sector_2_north_ridge", "species_cotton_hare"));
00078:             // Trapping density reads the same packs.
00079:             Assert.Equal(eco.SectorDensityMultiplier(m, "sector_4_hinterlands"),
00080:                 eco.SectorDensityMultiplier(m, "sector_4_hinterlands"));
00081:             Assert.True(eco.SectorDensityMultiplier(m, "sector_4_hinterlands") > 0f);
00082:         }
00083:
00084:         [Fact]
00085:         public void PredatorPressure_ReducesPrey_BoundedByRemnant()
00086:         {
00087:             var eco = MakeEcosystem(ShippedCatalog());
00088:             var m = SeededMigration();
00089:             int preyBefore = eco.SectorSpeciesPopulation(m, "sector_4_hinterlands", "species_cotton_hare");
00090:
00091:             eco.TickDay(1, m, outdoorRadModifier: 100f, "window_spring_storms",
00092:                 new SeededRng(1), new SeededRng(2), new SeededRng(3));
00093:
00094:             int preyAfter = eco.SectorSpeciesPopulation(m, "sector_4_hinterlands", "species_cotton_hare");
00095:             Assert.True(preyAfter < preyBefore, $"wolves must thin hares ({preyBefore} -> {preyAfter})");
00096:             Assert.True(preyAfter >= WildlifeEcosystemSystem.ExtinctionThreshold, "remnant floor holds");
00097:         }
00098:
00099:         [Fact]
00100:         public void RadiationAttrition_IsSpeciesSpecific()
00101:         {
00102:             var eco = MakeEcosystem(ShippedCatalog());
00103:             var m = SeededMigration();
00104:             int haresBefore = eco.SectorSpeciesPopulation(m, "sector_4_hinterlands", "species_cotton_hare");
00105:             int ratsBefore = eco.SectorSpeciesPopulation(m, "sector_4_hinterlands", "species_blight_rat");
00106:
00107:             // FalloutStorm-scale radiation (200): hares (tolerance 0.3) suffer
00108:             // radiological attrition on top of predation; blight rats (0.9) are
00109:             // under their tolerance band, so only rad-dog predation touches them.
00110:             for (int d = 1; d <= 5; d++)
00111:                 eco.TickDay(d, m, outdoorRadModifier: 200f, "window_spring_storms",
00112:                     new SeededRng(100 + d), new SeededRng(200 + d), new SeededRng(300 + d));
00113:
00114:             int haresAfter = eco.SectorSpeciesPopulation(m, "sector_4_hinterlands", "species_cotton_hare");
00115:             int ratsAfter = eco.SectorSpeciesPopulation(m, "sector_4_hinterlands", "species_blight_rat");
00116:             Assert.True(haresBefore - haresAfter > ratsBefore - ratsAfter,
00117:                 $"radiation-intolerant hares must decline faster than tolerant rats " +
00118:                 $"(hares {haresBefore}->{haresAfter}, rats {ratsBefore}->{ratsAfter})");
00119:         }
00120:
00121:         [Fact]
00122:         public void EcologyTick_IsDeterministic()
00123:         {
00124:             var run = new Func<string>(() =>
00125:             {
00126:                 var eco = MakeEcosystem(ShippedCatalog());
00127:                 var m = SeededMigration();
00128:                 for (int d = 1; d <= 10; d++)
00129:                     eco.TickDay(d, m, 150f, "window_spring_storms",
00130:                         new SeededRng(500 + d), new SeededRng(600 + d), new SeededRng(700 + d));
00131:                 var sb = new System.Text.StringBuilder();
00132:                 foreach (var p in m.State.packs)
00133:                     sb.Append(p.packId).Append('=').Append(p.population).Append('@').Append(p.currentSectorId).Append(';');
00134:                 foreach (var x in eco.State.extinct_species_sectors) sb.Append(x).Append(';');
00135:                 return sb.ToString();
00136:             });
00137:             Assert.Equal(run(), run());
00138:         }
00139:
00140:         [Fact]
00141:         public void LocalExtinction_FlagsAtThreshold_AndRecolonizes()
00142:         {
00143:             var eco = MakeEcosystem(ShippedCatalog());
00144:             var m = SeededMigration();
00145:             // Drive hares in sector 2 down to the floor by hand through the
00146:             // one authority, then tick to evaluate flags.
00147:             m.ThinSpeciesInSector("species_cotton_hare", "sector_2_north_ridge", 4, floor: 2);
00148:             eco.TickDay(1, m, 100f, "window_spring_storms", new SeededRng(1), new SeededRng(2), new SeededRng(3));
00149:             Assert.True(eco.IsLocallyExtinct("sector_2_north_ridge", "species_cotton_hare"),
00150:                 "population at the threshold must flag local extinction");
00151:
00152:             // Recolonization: migration brings numbers back past the bar.
00153:             var pack = m.TryGetPack("pack_hare_b");
00154:             pack!.population = WildlifeEcosystemSystem.RecolonizationPopulation + 2;
00155:             eco.TickDay(2, m, 100f, "window_spring_storms", new SeededRng(4), new SeededRng(5), new SeededRng(6));
00156:             Assert.False(eco.IsLocallyExtinct("sector_2_north_ridge", "species_cotton_hare"),
00157:                 "recovered population must clear the extinction flag");
00158:         }
00159:
00160:         [Fact]
00161:         public void ApexActivity_TriggersDeterministically_AndReportsOnce()
00162:         {
00163:             var eco = MakeEcosystem(ShippedCatalog());
00164:             var m = SeededMigration();
00165:             m.TryGetPack("pack_wolf_a")!.population = 16; // past threshold 14
00166:             int spotted = 0;
00167:             eco.OnApexPredatorSpotted += (_, _) => spotted++;
00168:
00169:             var a = new SeededRng(7);   // fresh stream each run
00170:             for (int d = 1; d <= 6 && spotted == 0; d++)
00171:                 eco.TickDay(d, m, 100f, "window_spring_storms", new SeededRng(1), new SeededRng(2), a);
00172:
00173:             var b = new SeededRng(7);
00174:             var eco2 = MakeEcosystem(ShippedCatalog());
00175:             var m2 = SeededMigration();
00176:             m2.TryGetPack("pack_wolf_a")!.population = 16;
00177:             int spotted2 = 0;
00178:             eco2.OnApexPredatorSpotted += (_, _) => spotted2++;
00179:             for (int d = 1; d <= 6 && spotted2 == 0; d++)
00180:                 eco2.TickDay(d, m2, 100f, "window_spring_storms", new SeededRng(1), new SeededRng(2), b);
00181:
00182:             Assert.Equal(spotted, spotted2);
00183:             Assert.True(spotted == 1 || spotted == 0, "at most one apex report per activation");
00184:             if (spotted == 1)
00185:             {
00186:                 Assert.Single(eco.ApexActivities);
00187:                 Assert.Equal("species_wolf", eco.ApexActivities[0].species_id);
00188:             }
00189:         }
00190:
00191:         [Fact]
00192:         public void Taming_OnlyForEligibleSpecies_AndTransfersOutOfWild()
00193:         {
00194:             var eco = MakeEcosystem(ShippedCatalog());
00195:             var m = SeededMigration();
00196:             Assert.False(eco.CanTame("species_wolf"), "wolves are not tameable");
00197:             Assert.True(eco.CanTame("species_cotton_hare"));
00198:
00199:             int before = eco.SectorSpeciesPopulation(m, "sector_4_hinterlands", "species_cotton_hare");
00200:             DomesticAnimalState? tamed = null;
00201:             for (int attempt = 0; attempt < 20 && tamed == null; attempt++)
00202:                 tamed = eco.TryTame("species_cotton_hare", "sector_4_hinterlands", 5, "survivor_a", m, new SeededRng(90 + attempt));
00203:             Assert.NotNull(tamed);
00204:             Assert.Single(eco.DomesticAnimals);
00205:             Assert.True(before - 1 == eco.SectorSpeciesPopulation(m, "sector_4_hinterlands", "species_cotton_hare"),
00206:                 "a tamed animal must leave the wild population through the one authority");
00207:
00208:             // Untameable species never tame.
00209:             Assert.Null(eco.TryTame("species_wolf", "sector_4_hinterlands", 6, "s", m, new SeededRng(1)));
00210:         }
00211:
00212:         [Fact]
00213:         public void Pressure_DecaysOverDays()
00214:         {
00215:             var eco = MakeEcosystem(ShippedCatalog());
00216:             var m = SeededMigration();
00217:             eco.RecordHuntingPressure("sector_4_hinterlands", "species_cotton_hare", 3);
00218:             Assert.Equal(3, eco.PressureOn("sector_4_hinterlands", "species_cotton_hare"));
00219:             eco.TickDay(1, m, 100f, "window_spring_storms", new SeededRng(1), new SeededRng(2), new SeededRng(3));
00220:             Assert.Equal(2, eco.PressureOn("sector_4_hinterlands", "species_cotton_hare"));
00221:             eco.TickDay(2, m, 100f, "window_spring_storms", new SeededRng(4), new SeededRng(5), new SeededRng(6));
00222:             eco.TickDay(3, m, 100f, "window_spring_storms", new SeededRng(7), new SeededRng(8), new SeededRng(9));
00223:             Assert.Equal(0, eco.PressureOn("sector_4_hinterlands", "species_cotton_hare"));
00224:         }
00225:
00226:         [Fact]
00227:         public void BestiaryKnowledge_IsObservationGated()
00228:         {
00229:             var eco = MakeEcosystem(ShippedCatalog());
00230:             Assert.Equal("unknown", eco.KnowledgeLevel("species_wolf"));
00231:             eco.RecordObservation("species_wolf", "sector_4_hinterlands", 1);
00232:             Assert.Equal("observed", eco.KnowledgeLevel("species_wolf"));
00233:             eco.RecordObservation("species_wolf", "sector_4_hinterlands", 2);
00234:             eco.RecordObservation("species_wolf", "sector_2_north_ridge", 3);
00235:             Assert.Equal("studied", eco.KnowledgeLevel("species_wolf"));
00236:             for (int i = 0; i < 5; i++)
00237:                 eco.RecordObservation("species_wolf", "sector_4_hinterlands", 4 + i);
00238:             Assert.Equal("documented", eco.KnowledgeLevel("species_wolf"));
00239:         }
00240:
00241:         [Fact]
00242:         public void SaveLoad_NextEcologyTickMatchesUninterrupted()
00243:         {
00244:             var run = new Func<string>(() =>
00245:             {
00246:                 var eco = MakeEcosystem(ShippedCatalog());
00247:                 var m = SeededMigration();
00248:                 for (int d = 1; d <= 12; d++)
00249:                     eco.TickDay(d, m, 180f, "window_spring_storms",
00250:                         new SeededRng(800 + d), new SeededRng(850 + d), new SeededRng(900 + d));
00251:                 var sb = new System.Text.StringBuilder();
00252:                 foreach (var p in m.State.packs)
00253:                     sb.Append(p.population).Append(',');
00254:                 foreach (var x in eco.State.extinct_species_sectors) sb.Append(x).Append(';');
00255:                 return sb.ToString();
00256:             });
00257:
00258:             // B: 6 days -> save -> restore -> 6 more days.
00259:             var ecoB = MakeEcosystem(ShippedCatalog());
00260:             var mB = SeededMigration();
00261:             for (int d = 1; d <= 6; d++)
00262:                 ecoB.TickDay(d, mB, 180f, "window_spring_storms",
00263:                     new SeededRng(800 + d), new SeededRng(850 + d), new SeededRng(900 + d));
00264:             var ecoSave = ecoB.CaptureState();
00265:             var packsSave = mB.CaptureState();
00266:
00267:             var mB2 = new WildlifeMigrationSystem(new SeededRng(42));
00268:             mB2.RestoreState(packsSave);
00269:             var ecoB2 = MakeEcosystem(ShippedCatalog());
00270:             ecoB2.RestoreState(ecoSave);
00271:             for (int d = 7; d <= 12; d++)
00272:                 ecoB2.TickDay(d, mB2, 180f, "window_spring_storms",
00273:                     new SeededRng(800 + d), new SeededRng(850 + d), new SeededRng(900 + d));
00274:
00275:             var expected = run();
00276:             var actual = new System.Text.StringBuilder();
00277:             foreach (var p in mB2.State.packs) actual.Append(p.population).Append(',');
00278:             foreach (var x in ecoB2.State.extinct_species_sectors) actual.Append(x).Append(';');
00279:             Assert.Equal(expected, actual.ToString());
00280:         }
00281:
00282:         [Fact]
00283:         public void OldSaveDefaults_RestoreSurvivesNulls()
00284:         {
00285:             var eco = MakeEcosystem(ShippedCatalog());
00286:             eco.RestoreState(null);
00287:             Assert.Empty(eco.State.extinct_species_sectors);
00288:             var bare = new WildlifeEcosystemState
00289:             { extinct_species_sectors = null, pressures = null, observations = null };
00290:             eco.RestoreState(bare);
00291:             Assert.NotNull(eco.State.extinct_species_sectors);
00292:         }
00293:     }
00294:
00295:     internal static class TestExtensions
00296:     {
00297:         public static T Also<T>(this T self, Action<T> action)
00298:         {
00299:             action(self);
00300:             return self;
00301:         }
00302:     }
00303: }
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **The subject is the current ecology data and single-population migration authority, with an explicit decision gate on whether a dedicated migration catalog is warranted. This is a conservative evidence plan, not a row-count expansion.**.

The first polish also checks continuity against the master authority: the plan is one bounded outcome, uses an existing owner seam, names a data authority, and does not widen into unrelated economy, UI, save or content work. Any apparently attractive addition that lacks a current owner is recorded as out of scope rather than smuggled into the architecture.

# Appendix — Polishing Pass 2: Integration and Code Architecture

This pass turns the evidence into an executable route. It distinguishes Core domain rules, host composition, Godot presentation, current save envelopes, deterministic streams, typed events and focused tests. The route is deliberately extend-first. A future builder may add a field, catalog row, read model or host adapter only after claiming the exact path and proving that the existing owner can accept it.

The second pass also reviews the handoff from data to player experience. A row that cannot be reached from a command is an orphan; a command that updates a shadow field is a split authority; a panel that recomputes a result is a presentation bug; a save that restores a display but not the owner is a persistence bug. Each failure is given a focused negative obligation.

# Appendix — Final Precision and Reaccuracy Pass

Before handoff, re-read every current path, hash, catalog count, public declaration, test declaration and save owner named above. Correct stale terminology, remove fictional type names, replace old section pins with current owner names, and downgrade any unsupported pass claim to historical evidence. Re-run the structural verifier after this pass. The final artifact should let a builder execute the first safe step without reinterpreting ownership.

**Precision result:** current implementation claims are separated from future proposals; content is not counted as reachability; Core remains engine-free; UI remains a projection; save and determinism are explicit; and every residual gap has a named verification route. If a future source audit contradicts this record, the source wins and the plan returns `STALE_PLAN` for re-audit.

# Appendix — Quality Assurance Pass Record

This record is part of the planning artifact, not a fresh runtime test result.

## Pass A — premise and content
- Current catalog rows, source owners, historical closeout and remaining residual are separated.
- The old baseline count is not presented as the current count.
- No copied, real-world, fabricated or unowned content is proposed.

## Pass B — integration architecture
- Data → loader → Core owner → host command → UI/event → save → replay is named.
- Existing save sections and codecs are identified; no parallel section is invented.
- Deterministic ordering, no-RNG cases, seeded streams and legacy defaults are explicit.

## Pass C — precision and handoff
- Every referenced current path is hash-pinned in the evidence appendices.
- Proposed future seams are labeled as proposals and excluded from current claims.
- Focused test commands, failure responses, rollback and non-goals are included.
