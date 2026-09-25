# Plan 60 — Vehicle Roster, Expedition Logistics and Garage Reachability

> **Rebuild status:** PARTIAL 8 CURRENT VEHICLES — SYSTEM, CATALOG AND GARAGE EXIST; TARGETED ROW/REACHABILITY AUDIT REMAINS
>
> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-5`
>
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round5-2026-09-25`
>
> **Current-evidence date:** 2026-09-25
>
> **Authority order:** live source and data → `AGENTS.md` → `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` → plan ledgers → this document.
>
> **Authority checksum:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
>
> **Length policy:** 150k–170k is the first quality checkpoint; 250k is an evidence-backed depth target, not a ceiling. The plan may exceed 250k when verified current architecture and evidence justify it, and it must stop rather than pad when that evidence is exhausted.

## 0. Integrity Statement and Plan Status

This file replaces unclaimed generated sections that mixed current evidence, fictional APIs, and unsupported save claims. It is a planning and architecture artifact only. It authorizes no production, data, test, save, generated-index, or UI edits. Every path labeled current must exist at rebuild time. Any future `CREATE` proposal is explicitly hypothetical and belongs to a later, separately claimed implementation package.

The rebuild follows four passes: content/current-reality first; integration framework second; accuracy and contradiction removal third; independent precision and handoff review fourth. Character count is recorded by external verification, not embedded recursively in the document.

# 1. Objective

- The current vehicle catalog has 8 vehicles and one track_gear row. ExpeditionVehicleSystem owns catalog, ownership, fuel, repair, equipment, track gear, profiles, breakdown outcome and persistence.
- VehicleGarageSystem adds current garage/customization behavior and armor/module catalogs; ExpeditionAggregate persists vehicle state.
- Current tests cover deterministic breakdown, profiles, save/restore, legacy envelopes and catalog loading.

**Bounded outcome:** Retire the 3-to-10 premise. vehicles.json currently has 8 vehicles plus track gear, while ExpeditionVehicleSystem, VehicleGarageSystem, armor/module catalogs, expedition host and garage panel exist. The plan must reconcile the 8-row reality with current acquisition, fuel, repair, cargo, track and expedition consumers; it must not add two vehicles just to hit ten.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- Replace the old target with an 8-vehicle/1-track-gear census and consumer matrix.
- Trace acquisition, fuel, repair, cargo, breakdown, track gear, armor and expedition profile consumers.
- Keep catalog definitions, vehicle instances, expedition aggregate state and garage customization state in current owners.

**Master-authority sections applied to this rebase:**

- Part II factory protocol: establish current reality, reject duplicate authority, and name one safe extension seam before design.
- Part II continuity checklist: data presence, a loader, a host route, a player-visible outcome, and persistence are separate proofs.
- Part III cluster map: preserve the current Core owner and route cross-system effects through typed facts rather than panel copies.
- Part VI Multi-Session Growth Protocol: 250k is an evidence-backed depth target, not a mandate to manufacture prose or row count.
- Live source/data authority outranks this plan; a future audit that contradicts a current declaration returns the package to STALE_PLAN.
- Anti-padding rule: preserve completed work as maintenance scope and spend detail only on proven residual gaps.
- Part II current-reality rule: JSON presence, a loader, a host route, a player-visible outcome, and persistence are separate proofs.
- Part III one-authority rule: extend the existing owner and route typed facts through it; do not create a second ledger, save store, registry, simulation, or panel cache.
- Part VI replay rule: any new randomness must use the existing seeded campaign stream and stable ordinal ordering; no System.Random or wall-clock decision path.
- Part VI save rule: a new mutable field is incomplete until CaptureState, RestoreState, old-save defaults, and checksum migration are specified.
- Part VII UI rule: presentation projects owner state and routes real commands; it never becomes a gameplay authority or a fake operational route.
- Part VIII quality rule: the 150k–170k band is an initial completeness checkpoint; 250k is an evidence-backed depth target, not a ceiling or a reason to pad.
- Live source/data evidence outranks the original plan. If a future audit contradicts a declaration here, the package returns to STALE_PLAN rather than reviving an obsolete API.
- The next safe delta is truthful roster reachability, not arbitrary row growth.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Current evidence requires a bounded owner/reachability audit; no new authority is implied.

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
| vehicle definitions, ownership, fuel, repair, equipment, track and profiles | ExpeditionVehicleSystem | `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` | Sole vehicle logistics owner. |
| vehicle JSON definitions | VehicleCatalogLoader | `Assets/Ashfall.Core/Expeditions/ExpeditionAggregate.cs` | Static catalog owner. |
| garage/customization and armor integration | VehicleGarageSystem | `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs` | Separate current garage concern. |
| expedition composition and host commands | ExpeditionHostSession | `src/Host/ExpeditionHostSession.cs` | Thin host seam. |
| current garage UI | VehicleGaragePanel | `src/UI/VehicleGaragePanel.cs` | Presentation/commands only. |
| vehicle math/save/catalog contracts | ExpeditionVehicleLogisticsTests | `Ashfall.Core.Tests/ExpeditionVehicleLogisticsTests.cs` | Focused evidence. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Vehicle Roster, Expedition Logistics and Garage Reachability
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ ExpeditionVehicleSystem
│   vehicle definitions, ownership, fuel, repair, equipment, track and profiles
│ VehicleCatalogLoader
│   vehicle JSON definitions
│ VehicleGarageSystem
│   garage/customization and armor integration
│ ExpeditionHostSession
│   expedition composition and host commands
│ VehicleGaragePanel
│   current garage UI
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

1. **Preserve current state ownership.** ExpeditionVehicleSystem owns vehicle definitions, ownership, fuel, repair, equipment, track and profiles: Sole vehicle logistics owner.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| vehicle definitions, ownership, fuel, repair, equipment, track and profiles | ExpeditionVehicleSystem | `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` | Sole vehicle logistics owner. |
| vehicle JSON definitions | VehicleCatalogLoader | `Assets/Ashfall.Core/Expeditions/ExpeditionAggregate.cs` | Static catalog owner. |
| garage/customization and armor integration | VehicleGarageSystem | `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs` | Separate current garage concern. |
| expedition composition and host commands | ExpeditionHostSession | `src/Host/ExpeditionHostSession.cs` | Thin host seam. |
| current garage UI | VehicleGaragePanel | `src/UI/VehicleGaragePanel.cs` | Presentation/commands only. |
| vehicle math/save/catalog contracts | ExpeditionVehicleLogisticsTests | `Ashfall.Core.Tests/ExpeditionVehicleLogisticsTests.cs` | Focused evidence. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load 8 vehicles and track gear
2. validate ranges/references
3. read current owned vehicle state
4. present garage and expedition availability
5. route acquire/refuel/repair/attach commands
6. create expedition profile through current owner
7. capture vehicle/expedition/garage state

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Vehicle definitions are immutable catalog rows.
- ExpeditionVehicleState owns owned vehicle instances and condition/fuel/equipment.
- ExpeditionAggregate persists active expedition and vehicle state.
- Armor/modules/track gear retain their current separate catalogs and state.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- A vehicle must be owned before expedition use.
- Fuel, cargo, speed, condition and breakdown math remain in current owners.
- A breakdown fallback to foot is deterministic and visible.
- An attachment cannot be installed on an incompatible vehicle without current validation.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- vehicles.json, vehicle modules and armor grades are separate authorities.
- Items/fuel/attachments remain in their own catalogs.
- No duplicate vehicle roster.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use ExpeditionAggregate/Vehicle save and current garage saves.
- No Plan-60 save section.
- Legacy envelopes restore empty garage state safely.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Breakdown and expedition outcomes use the existing seeded stream.
- Vehicle and attachment ordering is stable.
- Same catalog, state and seed produce identical profile/outcome.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Vehicle system emits state changes and breakdown outcomes.
- Expedition host consumes profiles and consequences.
- Garage UI requests current commands; it does not calculate range or condition.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/ExpeditionHostSession.cs
- src/UI/VehicleGaragePanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Vehicle names and descriptions remain fictional and grounded.
- No real-world technical claims are needed for balance or acquisition.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A row is counted as reachable without an acquisition/consumer path. | ExpeditionVehicleSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | Fuel/cargo is double charged. | VehicleCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | Breakdown state resets on reload. | VehicleGarageSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | Garage panel invents a vehicle. | ExpeditionHostSession | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A new vehicle save duplicates current state. | VehicleGaragePanel | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/ExpeditionVehicleLogisticsTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/VehicleGarageSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan50VehicleGarageIntegrationTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 | 8+1 catalog census. | No artificial ten-row target remains. | No production path until the owning implementation package is separately claimed. |
| 1 | Acquisition/consumer trace. | Every row has an actual route or is marked unreachable. | No production path until the owning implementation package is separately claimed. |
| 2 | Vehicle math/save/replay audit. | Current breakdown and profile contracts remain intact. | No production path until the owning implementation package is separately claimed. |
| 3 | Garage/UI polish. | No fake vehicle command or hidden state. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/vehicles.json | READ ONLY; MODIFY only for approved content delta | 8 vehicles/1 track row |
| Assets/Ashfall.Core/ExpeditionVehicleSystem.cs | READ ONLY | Vehicle owner |
| src/UI/VehicleGaragePanel.cs | READ ONLY | Garage surface |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Parallel vehicle state. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Count-only expansion. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Double charging. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Untracked break-down replay. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new vehicles in this package.
- No new vehicle system.
- No production/data/test/UI changes here.

# 23. Rollback and Recovery

- Revert planning artifact.
- Future vehicle content is isolated and must pass current acquisition/save gates.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- Current 8+1 reality and all consumers are named.
- A future row-count decision is separated from gameplay reachability.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Current evidence requires a bounded owner/reachability audit; no new authority is implied.

## MUST NOT DO

- No new vehicles in this package.
- No new vehicle system.
- No production/data/test/UI changes here.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/ExpeditionVehicleLogisticsTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/VehicleGarageSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan50VehicleGarageIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: vehicle definitions, ownership, fuel, repair, equipment, track and profiles → ExpeditionVehicleSystem; vehicle JSON definitions → VehicleCatalogLoader; garage/customization and armor integration → VehicleGarageSystem; expedition composition and host commands → ExpeditionHostSession; current garage UI → VehicleGaragePanel; vehicle math/save/catalog contracts → ExpeditionVehicleLogisticsTests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 60.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 60 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by ExpeditionVehicleSystem or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs`

### `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 464 lines / 20023 bytes.
- SHA-256: `c897d11e111c3ff4e559d01a4be0241b2e291de005cd4fce076090f3ac450eb2`.
- Architecture signals: seeded references=4; save/restore symbols=2; typed event declarations=12; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum VehicleBreakdownKind
public sealed class VehicleBreakdownOutcome
public bool BrokeDown { get; }
public VehicleBreakdownKind Kind { get; }
public float Severity01 { get; }
public float NominalMsV { get; }
public string Cause { get; }
public string VehicleId { get; }
public string SurvivorId { get; set; } = string.Empty;
public static VehicleBreakdownOutcome None(string vehicleId, string reason) => new VehicleBreakdownOutcome(VehicleBreakdownKind.None, 0f, 0f, reason, vehicleId);
public sealed class ExpeditionVehicleState
public string systemId = ExpeditionVehicleSystem.SystemId;
public Dictionary<string, VehicleInstance> ownedVehicles = new Dictionary<string, VehicleInstance>();
public string activeExpeditionVehicleId = string.Empty;
public sealed class VehicleInstance
public string vehicleId = string.Empty;
public string displayName = string.Empty;
public float condition = 100f;
public float fuel;
public float maxFuel = 50f;
public float cargoCapacity = 100f;
public float speedMultiplier = 1f;
public string terrainType = "road";
public bool isBrokenDown;
public string breakdownCause = string.Empty;
public List<string> attachments = new List<string>();
public VehicleTrackGearState trackGear = new VehicleTrackGearState();
public sealed class VehicleTrackGearState
public string gearId = string.Empty;
public float condition = 100f;
public float tractionMultiplier = 1f;
public float breakdownRiskMultiplier = 1f;
public bool IsInstalled => !string.IsNullOrEmpty(gearId);
public float EffectiveTractionMultiplier() {
public float EffectiveBreakdownRiskMultiplier() {
public sealed class VehicleDefinition
public string vehicle_id = string.Empty;
public string display_name = string.Empty;
public float max_fuel = 50f;
public float cargo_capacity = 100f;
public float speed_multiplier = 1f;
public string terrain_type = "road";
public float condition_max = 100f;
public float fuel_consumption_per_km = 0.5f;
public float breakdown_threshold = 0.2f;
public List<string> default_attachments = new List<string>();
public sealed class VehicleTrackGearDefinition
public string gear_id = string.Empty;
public string display_name = string.Empty;
public float traction_multiplier = 1.1f;
public float breakdown_risk_multiplier = 0.9f;
public sealed class VehicleCatalog
public int schema_version = 1;
public List<VehicleDefinition> vehicles = new List<VehicleDefinition>();
public List<VehicleTrackGearDefinition> track_gear = new List<VehicleTrackGearDefinition>();
public sealed class ExpeditionVehicleSystem
public const string SystemId = "expedition_vehicle";
public ExpeditionVehicleState State => _state;
public event Action OnVehicleStateChanged;
public void LoadCatalog(VehicleCatalog catalog) {
public VehicleDefinition? GetDefinition(string id) {
public VehicleTrackGearDefinition? GetTrackGearDefinition(string id) {
public ActionResult AcquireVehicle(string vehicleId) {
public VehicleInstance? GetVehicle(string vehicleId) {
public ActionResult Refuel(string vehicleId, float amount) {
public ActionResult Repair(string vehicleId, float amount) {
public ActionResult AttachEquipment(string vehicleId, string equipmentId) {
public ActionResult InstallTrackGear( string vehicleId, string gearId, float tractionMultiplier, float breakdownRiskMultiplier, float condition = 100f)
public ActionResult InstallTrackGear(string vehicleId, string gearId, float condition = 100f) {
public ActionResult RemoveTrackGear(string vehicleId) {
public ActionResult RepairTrackGear(string vehicleId, float amount) {
public ExpeditionVehicleProfile? CreateExpeditionProfile( string vehicleId, float kmPerTravelTick = 2.5f) {
public event Action<VehicleBreakdownOutcome>? OnBreakdownResolved;
public VehicleBreakdownOutcome ResolvePrepBreakdown(string vehicleId, float distanceKm, ISeededRng? rng = null) {
public ExpeditionVehicleState CaptureState() => CloneState(_state);
public void RestoreState(ExpeditionVehicleState saved) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Expeditions/ExpeditionAggregate.cs`

### `Assets/Ashfall.Core/Expeditions/ExpeditionAggregate.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 115 lines / 5002 bytes.
- SHA-256: `513c8d961b30670f32c5040ee0facf4cdb5e869da06f129c6f607f0378be88d9`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExpeditionAggregateState
public string systemId = "expedition_aggregate";
public List<ExpeditionState> expeditions = new List<ExpeditionState>();
public ExpeditionVehicleState vehicles = new ExpeditionVehicleState();
public List<string>? knownLocationIds = new List<string>();
public DiscoveryConsequenceState? discoveryConsequences;
public int completedCount;
public static class VehicleCatalogLoader
public const string FileName = "vehicles.json";
public static VehicleCatalog Load(string dataDir, IFileIO files, IJsonSerializer json) {
public static VehicleCatalog Load(string dataDir) => Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
public static class ExpeditionAggregateCodec
public static string Encode(ExpeditionAggregateState aggregate, IJsonSerializer json) => SaveEnvelopeHelper.CaptureEnvelope(aggregate, json);
public static ExpeditionAggregateState? Decode(string raw, IJsonSerializer json) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs`

### `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 864 lines / 36461 bytes.
- SHA-256: `6493ee6924ace913e975210a1a66500670279fd2ffc0d3099902373b0b4fd982`.
- Architecture signals: seeded references=3; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class VehicleCustomizationRecord
public string vehicleId = string.Empty;
public Dictionary<string, string> installedSlots = new Dictionary<string, string>(StringComparer.Ordinal);
public int chassisStressPermille;
public int engineFoulingPermille;
public int transmissionWearPermille;
public bool isImmobilized;
public string immobilizedReason = string.Empty;
public string armorGradeId = string.Empty;
public int armorIntegrityPermille;
public int armorIntegrityMaxPermille;
public string armorMaterialProfileId = string.Empty;
public string armorPurity = FoundryPurityNames.Standard;
public sealed class VehicleRecoveryMission
public string missionId = string.Empty;
public string strandedVehicleId = string.Empty;
public string locationId = string.Empty;
public int requiredFuelUnits = 10;
public int progressTicks;
public int requiredTicks = 120;
public bool isComplete;
public sealed class VehicleGarageState
public string systemId = VehicleGarageSystem.SystemId;
public Dictionary<string, VehicleCustomizationRecord> vehicleRecords = new Dictionary<string, VehicleCustomizationRecord>(StringComparer.Ordinal);
public Dictionary<string, VehicleRecoveryMission> activeRecoveries = new Dictionary<string, VehicleRecoveryMission>(StringComparer.Ordinal);
public int nextRecoveryCounter = 1;
public sealed class VehicleGarageSystem
public const string SystemId = "vehicle_garage";
public const string SlotCargo = "cargo";
public const string SlotProtection = "protection";
public const string SlotMobility = "mobility";
public const string SlotEngine = "engine";
public const string SlotUtility = "utility";
public delegate bool TryGetArmorMaterialQuality(out FoundryMaterialQuality quality);
public TryGetArmorMaterialQuality? ArmorMaterialQualitySource { get; set; }
public Func<string, string?>? VehicleTerrainResolver { get; set; }
public void LoadCatalog(VehicleGarageCatalog catalog) {
public bool HasModification(string modId) => _modCatalog.ContainsKey(modId);
public VehicleModificationDefinition? GetModification(string modId) {
public IReadOnlyDictionary<string, VehicleModificationDefinition> GetAllModifications() => _modCatalog;
public void LoadArmorCatalog(VehicleArmorGradeCatalog catalog) {
public bool HasArmorGrade(string gradeId) => !string.IsNullOrEmpty(gradeId) && _armorCatalog.ContainsKey(gradeId);
public VehicleArmorGradeDefinition? GetArmorGrade(string gradeId) {
public IReadOnlyDictionary<string, VehicleArmorGradeDefinition> GetAllArmorGrades() => _armorCatalog;
public VehicleArmorProfile GetArmorProfile(string vehicleId) {
public static string ArmorConditionBand(int integrityPermille, int maxPermille) {
public VehicleCustomizationRecord? GetRecord(string vehicleId) =>
public bool IsImmobilized(string vehicleId) => GetRecord(vehicleId)?.isImmobilized == true;
public IReadOnlyDictionary<string, VehicleRecoveryMission> ActiveRecoveries => _state.activeRecoveries;
public IReadOnlyDictionary<string, string> GetInstalledSlots(string vehicleId) =>
public void DecorateProfile(ExpeditionVehicleProfile? profile) {
public int AdvanceRecoveries(int deltaTicks) {
public VehicleCustomizationRecord GetOrCreateRecord(string vehicleId) {
public bool HasVehicleRecord(string vehicleId) =>
public bool CanInstallArmorGrade(string vehicleId, string gradeId, IPlayerInventoryPort? inventory, out string reason) {
public bool InstallArmorGrade(string vehicleId, string gradeId, IPlayerInventoryPort? inventory, out string reason) {
public bool ReforgeArmorPlate(string vehicleId, IPlayerInventoryPort? inventory, out string reason) {
public bool CanInstallModification(string vehicleId, string slotType, string modId, IPlayerInventoryPort? inventory, out string reason) {
public bool InstallModification(string vehicleId, string slotType, string modId, IPlayerInventoryPort? inventory, out string reason) {
public bool UninstallModification(string vehicleId, string slotType, IPlayerInventoryPort? inventory, out string reason) {
public float GetEffectiveCargoCapacityDelta(string vehicleId) {
public float GetEffectiveSpeedMultiplierDelta(string vehicleId) {
public float GetEffectiveFuelConsumptionMultiplier(string vehicleId) {
public float GetEffectiveWearRateMultiplier(string vehicleId) {
public int GetEffectiveRadiationProtectionPermille(string vehicleId) {
public void RecordTripWear(string vehicleId, float distanceKm, float roadRoughnessMultiplier = 1f) {
public bool ServiceChassis(string vehicleId, IPlayerInventoryPort? inventory, int repairPermille, out string reason) {
public bool ServiceEngine(string vehicleId, IPlayerInventoryPort? inventory, int repairPermille, out string reason) {
public bool ServiceTransmission(string vehicleId, IPlayerInventoryPort? inventory, int repairPermille, out string reason) {
public bool RegisterRecoveryMission(string vehicleId, string locationId, int requiredFuelUnits, out string missionId, out string reason) {
public bool AdvanceRecoveryMission(string missionId, int deltaTicks, out bool completed) {
public bool CompleteRecoveryMission(string missionId, IPlayerInventoryPort? inventory, out string reason) {
public VehicleGarageState CaptureState() {
public void RestoreState(VehicleGarageState? state) {
```


# Appendix B.05 — Current Code Architecture: `src/Host/ExpeditionHostSession.cs`

### `src/Host/ExpeditionHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1663 lines / 85060 bytes.
- SHA-256: `46c1ce80f030a53183aea292df6246ab3278b2c2d481baf91e91cbf1f1c8af37`.
- Architecture signals: seeded references=6; save/restore symbols=13; typed event declarations=11; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExpeditionHostSession : HostSessionBase
public const int DemoSeed = 7071;
public const int VehicleSeed = 7072;
public const float KmPerTravelTick = 2.5f;
public const string StarterVehicleId = "vehicle_utility_quad";
public ExpeditionSystem Engine { get; }
public List<ExpeditionDefinition> Definitions { get; }
public List<ExpeditionDefinition> DemoDefinitions => Definitions;
public DiveInstanceRunner DiveRunner { get; private set; }
public Ashfall.Core.Flags.IFlagLedger Flags { get; set; } = new Ashfall.Core.Flags.CampaignConsequenceLedger();
public DiscoveryConsequenceSystem DiscoveryConsequences { get; }
public event Action<ConsequenceOutcome>? OnDiscoveryConsequenceApplied;
public Action<string, string, int>? ApplyDisease { get; set; }
public JournalSystem? Journal { get; set; }
public Ashfall.Core.Inventory.Inventory? ShelterInventory { get; set; }
public ItemCatalog? Items { get; set; }
public ExpeditionVehicleSystem Vehicles { get; }
public VehicleGarageSystem? Garage { get; set; }
public VouchAccessSystem CrossingGate { get; set; }
public WastelandMapSystem? WastelandMap { get; set; }
public Func<string, bool> ExtraBlocked { get; set; }
public Func<string, FitnessVerdict?>? SurvivorFitnessProvider { get; set; }
public Func<string, RoleFitnessVerdict?>? ExpeditionFitnessProvider { get; set; }
public Func<string, float>? SurvivorMovementSpeedProvider { get; set; }
public Func<string, float>? PackCapacityProvider { get; set; }
public Func<string, WeatherGateBlock?> ExtraGateBlock { get; set; }
public string? GetBlockReason(string locationId) {
public WeatherGateBlock? GetWeatherGateBlock(string locationId) => ExtraGateBlock?.Invoke(locationId);
public void SetEncounterChanceMultiplier(Func<string, float> multiplier) => Engine.SetEncounterChanceMultiplier(multiplier);
public void SetEstimateWeatherInputs(Func<string, ExpeditionWeatherInputs?>? provider) {
public void SetEstimateProtectiveInputs(Func<string, ExpeditionProtectiveInputs?>? provider) {
public void SetEstimateRouteModifiers(Func<string, float>? hazard, Func<string, float>? travel) {
public void AttachDamagedMapFeedback(DamagedMapSystem? damagedMap) {
public string LastEvent { get; private set; } = string.Empty;
public event Action<ExpeditionEncounterBridge.EncounterSurfaced>? OnEncounterSurfaced;
public sealed class TravelCombatTrigger
public string EncounterId = string.Empty;
public string Title = string.Empty;
public string LocationId = string.Empty;
public int DangerLevel;
public IReadOnlyList<string> CombatantIds = Array.Empty<string>();
public event Action<TravelCombatTrigger>? OnTravelEncounterCombatTriggered;
public event Action<string, string, WeatherGateBlock>? OnWeatherGateForced;
public static bool UseEncounterModal { get; set; } = true;
public ExpeditionEncounterBridge Bridge => _bridge;
public Dictionary<string, float> WaterRouteHazards { get; } = new(StringComparer.Ordinal);
public IReadOnlyList<PendingSurfacedEncounter> Pending =>
public EncounterDefinition? FindEncounter(string encounterId) => _narrative?.Find(encounterId);
public void ClearAllPending() => _narrative?.ClearAllPending();
public NarrativeEncounterSystem? NarrativeEngine => _narrative;
public static ExpeditionHostSession Create(string dataDir, NarrativeEncounterSystem narrative = null!, TravelEncounterSystem travel = null!, ICampaignRngManager? campaignRng = null) {
public bool IsLocationBlocked(string locationId) => GetBlockReason(locationId) != null;
public CommandResult StartExpedition( string survivorId, string locationId, ExpeditionStance stance = ExpeditionStance.Stealth, int staminaBudget = 40, string vehicleId = "",
public CommandResult RefuelVehicle(string vehicleId, float units) {
public CommandResult RepairVehicle(string vehicleId, float amount) {
public CommandResult InstallTrackGear(string vehicleId, string gearId, float condition = 100f) {
public CommandResult RemoveTrackGear(string vehicleId) {
public CommandResult RepairTrackGear(string vehicleId, float amount) {
public CommandResult AssembleVehicleFromKit(string kitItemId, Inventory shelterInventory) {
public Action<VehicleBreakdownOutcome>? BreakdownConsequenceSink { get; set; }
public CommandResult StartDemoExpedition(string survivorId, string locationId) => StartExpedition(survivorId, locationId);
public CommandResult DispatchSortie( string survivorId, string locationId, ExpeditionStance stance, int day, string vehicleId = "",
public FitnessVerdict? GetSurvivorFitness(string survivorId) => SurvivorFitnessProvider?.Invoke(survivorId);
public RoleFitnessVerdict? GetExpeditionFitness(string survivorId) => ExpeditionFitnessProvider?.Invoke(survivorId);
public string TickHours(float hours) {
public sealed class EncounterApplicationResult
public string ResolutionId = string.Empty;
public enum Status { NotApplicable, Applied, AlreadyKnown, RejectedCapacity, RejectedInsufficientItems, NoActiveExpedition, SkippedNoAuthority, RejectedUnknownId } public Status Item = Status.NotApplicable; public string ItemId = string.Empty; public int ItemQuantity; public Status Journal = Status.NotApplicable; public string JournalId = string.Empty; public Status Location = Status.NotApplicable; public string LocationId = string.Empty; public Status Flag = Status.NotApplicable; public string FlagId = string.Empty; /// <summary>F17 — micro-location hazard routing outcome. NotApplicable /// for flags without a registered hazard; Applied when the canonical /// disease authority received the consequence exactly once.</summary> public MicroLocationHazardRegistry.HazardStatus Hazard = MicroLocationHazardRegistry.HazardStatus.NotApplicable; public string HazardDiseaseId = string.Empty; }
public EncounterApplicationResult? LastApplication { get; private set; }
public event Action<EncounterApplicationResult>? OnEncounterConsequencesApplied;
public bool EncounterApplyChoice(string encounterId, string choiceId, int day) => EncounterApplyChoice(encounterId, choiceId, day, null!);
public bool EncounterApplyChoice(string encounterId, string choiceId, int day, string locationId) {
public bool ResolveTravelChoiceWithCombat( string encounterId, string choiceId, int day, string locationId, int dangerLevel, int enemyCount) {
public static readonly ExpeditionJournalAuthor Instance = new ExpeditionJournalAuthor();
public string Id => "expedition";
public string DisplayName => "Expedition";
public RiskBiasTrait RiskBias => RiskBiasTrait.Realist;
public string PushLuck(string survivorId) {
public string PushLuckDemo(string survivorId) => PushLuck(survivorId);
public string Retreat(string survivorId) {
public string RetreatDemo(string survivorId) => Retreat(survivorId);
public string EnterCamp( string survivorId, float temperatureC = -10f, string weatherCondition = "Clear", float firewood = 8f, float water = 4f,
public string EnterCampDemo( string survivorId, float temperatureC = -10f, string weatherCondition = "Clear", float firewood = 8f, float water = 4f,
public string CampTick(string survivorId) {
public string CampTickDemo(string survivorId) => CampTick(survivorId);
public string ResolveCampEncounter(string survivorId, string outcome) {
public string ResolveCampEncounterDemo(string survivorId, string outcome) => ResolveCampEncounter(survivorId, outcome);
public string BreakCamp(string survivorId, bool retreat = false) {
public string BreakCampDemo(string survivorId, bool retreat = false) => BreakCamp(survivorId, retreat);
public CampState? GetCampState(string survivorId) => Engine.GetCampState(survivorId);
public string StatusLine() {
public List<ExpeditionState> CaptureSave() => Engine.CaptureState();
public void RestoreSave(List<ExpeditionState> state) => Engine.RestoreState(state);
public ExpeditionAggregateState CaptureSaveAggregate() {
public void RestoreSaveAggregate(ExpeditionAggregateState aggregate) {
public string StartDive(string siteId = "site_exp09_ss_sovereign") {
public string StartDiveDemo(string siteId = "site_exp09_ss_sovereign") => StartDive(siteId);
public string AdvanceDive() {
public string AdvanceDiveDemo() => AdvanceDive();
public string TickDiveOxygen() {
public string TickDiveOxygenDemo() => TickDiveOxygen();
public string CommitDiveChoice(string choice) {
public string CommitDiveChoiceDemo(string choice) => CommitDiveChoice(choice);
public string DiveStatusLine() {
```


# Appendix B.06 — Current Code Architecture: `src/Host/ExpeditionHostSession.cs`

### `src/Host/ExpeditionHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1663 lines / 85060 bytes.
- SHA-256: `46c1ce80f030a53183aea292df6246ab3278b2c2d481baf91e91cbf1f1c8af37`.
- Architecture signals: seeded references=6; save/restore symbols=13; typed event declarations=11; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExpeditionHostSession : HostSessionBase
public const int DemoSeed = 7071;
public const int VehicleSeed = 7072;
public const float KmPerTravelTick = 2.5f;
public const string StarterVehicleId = "vehicle_utility_quad";
public ExpeditionSystem Engine { get; }
public List<ExpeditionDefinition> Definitions { get; }
public List<ExpeditionDefinition> DemoDefinitions => Definitions;
public DiveInstanceRunner DiveRunner { get; private set; }
public Ashfall.Core.Flags.IFlagLedger Flags { get; set; } = new Ashfall.Core.Flags.CampaignConsequenceLedger();
public DiscoveryConsequenceSystem DiscoveryConsequences { get; }
public event Action<ConsequenceOutcome>? OnDiscoveryConsequenceApplied;
public Action<string, string, int>? ApplyDisease { get; set; }
public JournalSystem? Journal { get; set; }
public Ashfall.Core.Inventory.Inventory? ShelterInventory { get; set; }
public ItemCatalog? Items { get; set; }
public ExpeditionVehicleSystem Vehicles { get; }
public VehicleGarageSystem? Garage { get; set; }
public VouchAccessSystem CrossingGate { get; set; }
public WastelandMapSystem? WastelandMap { get; set; }
public Func<string, bool> ExtraBlocked { get; set; }
public Func<string, FitnessVerdict?>? SurvivorFitnessProvider { get; set; }
public Func<string, RoleFitnessVerdict?>? ExpeditionFitnessProvider { get; set; }
public Func<string, float>? SurvivorMovementSpeedProvider { get; set; }
public Func<string, float>? PackCapacityProvider { get; set; }
public Func<string, WeatherGateBlock?> ExtraGateBlock { get; set; }
public string? GetBlockReason(string locationId) {
public WeatherGateBlock? GetWeatherGateBlock(string locationId) => ExtraGateBlock?.Invoke(locationId);
public void SetEncounterChanceMultiplier(Func<string, float> multiplier) => Engine.SetEncounterChanceMultiplier(multiplier);
public void SetEstimateWeatherInputs(Func<string, ExpeditionWeatherInputs?>? provider) {
public void SetEstimateProtectiveInputs(Func<string, ExpeditionProtectiveInputs?>? provider) {
public void SetEstimateRouteModifiers(Func<string, float>? hazard, Func<string, float>? travel) {
public void AttachDamagedMapFeedback(DamagedMapSystem? damagedMap) {
public string LastEvent { get; private set; } = string.Empty;
public event Action<ExpeditionEncounterBridge.EncounterSurfaced>? OnEncounterSurfaced;
public sealed class TravelCombatTrigger
public string EncounterId = string.Empty;
public string Title = string.Empty;
public string LocationId = string.Empty;
public int DangerLevel;
public IReadOnlyList<string> CombatantIds = Array.Empty<string>();
public event Action<TravelCombatTrigger>? OnTravelEncounterCombatTriggered;
public event Action<string, string, WeatherGateBlock>? OnWeatherGateForced;
public static bool UseEncounterModal { get; set; } = true;
public ExpeditionEncounterBridge Bridge => _bridge;
public Dictionary<string, float> WaterRouteHazards { get; } = new(StringComparer.Ordinal);
public IReadOnlyList<PendingSurfacedEncounter> Pending =>
public EncounterDefinition? FindEncounter(string encounterId) => _narrative?.Find(encounterId);
public void ClearAllPending() => _narrative?.ClearAllPending();
public NarrativeEncounterSystem? NarrativeEngine => _narrative;
public static ExpeditionHostSession Create(string dataDir, NarrativeEncounterSystem narrative = null!, TravelEncounterSystem travel = null!, ICampaignRngManager? campaignRng = null) {
public bool IsLocationBlocked(string locationId) => GetBlockReason(locationId) != null;
public CommandResult StartExpedition( string survivorId, string locationId, ExpeditionStance stance = ExpeditionStance.Stealth, int staminaBudget = 40, string vehicleId = "",
public CommandResult RefuelVehicle(string vehicleId, float units) {
public CommandResult RepairVehicle(string vehicleId, float amount) {
public CommandResult InstallTrackGear(string vehicleId, string gearId, float condition = 100f) {
public CommandResult RemoveTrackGear(string vehicleId) {
public CommandResult RepairTrackGear(string vehicleId, float amount) {
public CommandResult AssembleVehicleFromKit(string kitItemId, Inventory shelterInventory) {
public Action<VehicleBreakdownOutcome>? BreakdownConsequenceSink { get; set; }
public CommandResult StartDemoExpedition(string survivorId, string locationId) => StartExpedition(survivorId, locationId);
public CommandResult DispatchSortie( string survivorId, string locationId, ExpeditionStance stance, int day, string vehicleId = "",
public FitnessVerdict? GetSurvivorFitness(string survivorId) => SurvivorFitnessProvider?.Invoke(survivorId);
public RoleFitnessVerdict? GetExpeditionFitness(string survivorId) => ExpeditionFitnessProvider?.Invoke(survivorId);
public string TickHours(float hours) {
public sealed class EncounterApplicationResult
public string ResolutionId = string.Empty;
public enum Status { NotApplicable, Applied, AlreadyKnown, RejectedCapacity, RejectedInsufficientItems, NoActiveExpedition, SkippedNoAuthority, RejectedUnknownId } public Status Item = Status.NotApplicable; public string ItemId = string.Empty; public int ItemQuantity; public Status Journal = Status.NotApplicable; public string JournalId = string.Empty; public Status Location = Status.NotApplicable; public string LocationId = string.Empty; public Status Flag = Status.NotApplicable; public string FlagId = string.Empty; /// <summary>F17 — micro-location hazard routing outcome. NotApplicable /// for flags without a registered hazard; Applied when the canonical /// disease authority received the consequence exactly once.</summary> public MicroLocationHazardRegistry.HazardStatus Hazard = MicroLocationHazardRegistry.HazardStatus.NotApplicable; public string HazardDiseaseId = string.Empty; }
public EncounterApplicationResult? LastApplication { get; private set; }
public event Action<EncounterApplicationResult>? OnEncounterConsequencesApplied;
public bool EncounterApplyChoice(string encounterId, string choiceId, int day) => EncounterApplyChoice(encounterId, choiceId, day, null!);
public bool EncounterApplyChoice(string encounterId, string choiceId, int day, string locationId) {
public bool ResolveTravelChoiceWithCombat( string encounterId, string choiceId, int day, string locationId, int dangerLevel, int enemyCount) {
public static readonly ExpeditionJournalAuthor Instance = new ExpeditionJournalAuthor();
public string Id => "expedition";
public string DisplayName => "Expedition";
public RiskBiasTrait RiskBias => RiskBiasTrait.Realist;
public string PushLuck(string survivorId) {
public string PushLuckDemo(string survivorId) => PushLuck(survivorId);
public string Retreat(string survivorId) {
public string RetreatDemo(string survivorId) => Retreat(survivorId);
public string EnterCamp( string survivorId, float temperatureC = -10f, string weatherCondition = "Clear", float firewood = 8f, float water = 4f,
public string EnterCampDemo( string survivorId, float temperatureC = -10f, string weatherCondition = "Clear", float firewood = 8f, float water = 4f,
public string CampTick(string survivorId) {
public string CampTickDemo(string survivorId) => CampTick(survivorId);
public string ResolveCampEncounter(string survivorId, string outcome) {
public string ResolveCampEncounterDemo(string survivorId, string outcome) => ResolveCampEncounter(survivorId, outcome);
public string BreakCamp(string survivorId, bool retreat = false) {
public string BreakCampDemo(string survivorId, bool retreat = false) => BreakCamp(survivorId, retreat);
public CampState? GetCampState(string survivorId) => Engine.GetCampState(survivorId);
public string StatusLine() {
public List<ExpeditionState> CaptureSave() => Engine.CaptureState();
public void RestoreSave(List<ExpeditionState> state) => Engine.RestoreState(state);
public ExpeditionAggregateState CaptureSaveAggregate() {
public void RestoreSaveAggregate(ExpeditionAggregateState aggregate) {
public string StartDive(string siteId = "site_exp09_ss_sovereign") {
public string StartDiveDemo(string siteId = "site_exp09_ss_sovereign") => StartDive(siteId);
public string AdvanceDive() {
public string AdvanceDiveDemo() => AdvanceDive();
public string TickDiveOxygen() {
public string TickDiveOxygenDemo() => TickDiveOxygen();
public string CommitDiveChoice(string choice) {
public string CommitDiveChoiceDemo(string choice) => CommitDiveChoice(choice);
public string DiveStatusLine() {
```


# Appendix B.07 — Current Code Architecture: `src/UI/VehicleGaragePanel.cs`

### `src/UI/VehicleGaragePanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 635 lines / 29929 bytes.
- SHA-256: `0f7ed9ba942430e7bf5b35fc289bce28176d35d8534876302d9c86223a425b4f`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class VehicleGaragePanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _garage != null;
public string LastFeedback { get; private set; } = string.Empty;
public void Bind(VehicleGarageSystem garage, ExpeditionVehicleSystem vehicles, Inventory inventory) {
public void Unbind() {
public override void _Ready() {
public override void _ExitTree() {
public void RefreshView() {
```


# Appendix C.08 — Catalog Census: `Assets/StreamingAssets/Data/vehicles.json`

### `Assets/StreamingAssets/Data/vehicles.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 3044 bytes / 3044 characters.
- SHA-256: `cdec845708de1d2d3181767aeed9f4b4565a48596835415340ed557fd9b9ce82`.
- Root keys: `schema_version`, `track_gear`, `vehicles`.

Array-path census (minimum, maximum, observed rows):

```text
track_gear: min=1, max=1, observed_paths=1
vehicles: min=8, max=8, observed_paths=1
vehicles[].default_attachments: min=0, max=0, observed_paths=2
```

Representative record fields:

- `breakdown_threshold`
- `cargo_capacity`
- `condition_max`
- `default_attachments`
- `display_name`
- `fuel_consumption_per_km`
- `max_fuel`
- `speed_multiplier`
- `terrain_type`
- `vehicle_id`


# Appendix C.09 — Catalog Census: `Assets/StreamingAssets/Data/vehicle_armor_grades.json`

### `Assets/StreamingAssets/Data/vehicle_armor_grades.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 4568 bytes / 4568 characters.
- SHA-256: `8e62dc6b78f7e97b107078bd3802577edfdba0b399abc8e70d1479dd76d03faa`.
- Root keys: `default_grade_id`, `grades`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
grades: min=5, max=5, observed_paths=1
grades[].compatible_terrain_types: min=3, max=3, observed_paths=2
grades[].install_cost: min=0, max=2, observed_paths=2
grades[].reforge_cost: min=0, max=1, observed_paths=2
grades[].tags: min=2, max=3, observed_paths=2
```

Representative record fields:

- `compatible_terrain_types`
- `description`
- `display_name`
- `fuel_consumption_multiplier`
- `id`
- `install_cost`
- `install_labor_ticks`
- `integrity_pool_permille`
- `is_default`
- `mitigation_permille`
- `reforge_cost`
- `speed_multiplier_delta`
- `tags`
- `tier`
- `wear_absorption_permille`

Representative identifiers (ordered, capped for readability):

```text
grade_0_stock
grade_1_scrap_plate
grade_2_sheet_plate
grade_3_composite_plate
grade_4_alloyed_heavy_plate
```


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/vehicle_modifications.json`

### `Assets/StreamingAssets/Data/vehicle_modifications.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 4882 bytes / 4882 characters.
- SHA-256: `f1c6e40d1d97f17af70faed4b7e09ac201a0c64e540d61b33087fb2c88373001`.
- Root keys: `modifications`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
modifications: min=8, max=8, observed_paths=1
modifications[].compatible_vehicle_tags: min=4, max=4, observed_paths=2
modifications[].install_cost: min=2, max=2, observed_paths=2
modifications[].tags: min=3, max=3, observed_paths=2
```

Representative record fields:

- `compatible_vehicle_tags`
- `effects`
- `id`
- `install_cost`
- `install_labor_ticks`
- `slot_type`
- `tags`

Representative identifiers (ordered, capped for readability):

```text
vmod_expanded_flatbed
vmod_lead_lined_cab
vmod_reinforced_bullbar
vmod_aux_fuel_rack
vmod_traction_cleats
vmod_stretcher_mount
vmod_heavy_winch
vmod_engine_supercharger
```


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/vehicle_modules.json`

### `Assets/StreamingAssets/Data/vehicle_modules.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 8238 bytes / 8238 characters.
- SHA-256: `832474455de424b275dc335fd40ca8263dd55ca4f6b208dc46072baaec039319`.
- Root keys: `modules`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
modules: min=20, max=20, observed_paths=1
```

Representative record fields:

- `bunk_capacity`
- `cargo_bonus`
- `components_cost`
- `defense_bonus`
- `description`
- `installation_days`
- `module_id`
- `module_type`
- `name`
- `scrap_cost`
- `speed_modifier`


# Appendix D.12 — Existing Focused Test Inventory: `Ashfall.Core.Tests/ExpeditionVehicleLogisticsTests.cs`

### `Ashfall.Core.Tests/ExpeditionVehicleLogisticsTests.cs`

- Current test declarations: Fact=17, Theory=0, InlineData=0.
- File lines: 493; SHA-256: `2f574c226a8a522a1d4925d66d9c507ecb75d09ebb8593d907b37741f4991752`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
VehicleSpeed_ShortensOutboundTravel
VehicleBreakdown_IsDeterministic_AndRevertsToFoot
VehicleBreakdown_FiresEventOnce
NeutralVehicle_NeverConsumesBreakdownRolls
Estimate_MirrorsTickMath_ForFootAndVehicle
Bridge_ProjectsAuthorityConditionIntoCombatToken
Bridge_PrefersOwnerAndBestCondition
Bridge_WriteBack_ConvertsUnits_ThroughAuthority
Bridge_Readiness_FollowsAuthorityRisks
AggregateCodec_RoundTripsExpeditionsAndGarage
RestoreCompletedCount_SurvivesAggregateRestore
AggregateCodec_MigratesLegacyEnvelopeAndBareList
AggregateState_RestoreContinuesInFlightVehicleExpedition
VehicleCatalogLoader_ReadsDataAuthority_AndToleratesAbsence
TrackGear_InstallsRepairsAndPersistsNormalizedEffects
TrackGear_ProfileProjectionKeepsTravelMathInCore
TrackGear_RejectsInvalidFactsWithoutMutation
```


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Expeditions/VehicleGarageSystemTests.cs`

### `Ashfall.Core.Tests/Expeditions/VehicleGarageSystemTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 225; SHA-256: `e90e41b6cc29857ddf5f73315bd919423a85ff32fb61f2a6d30b102d23aae7cb`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
InstallModification_DeductsInventoryAndAppliesEffects
InstallModification_FailsOnMismatchedSlotOrInsufficientMaterials
TripWear_AccumulatesAndCanImmobilize
ServiceComponents_ConsumesMaterialsAndRestoresHealth
RecoveryMission_LifecycleCompletesAndClearsImmobilization
StateRoundtrip_PreservesModificationsAndWear
```


# Appendix D.14 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Expeditions/Plan50VehicleGarageIntegrationTests.cs`

### `Ashfall.Core.Tests/Expeditions/Plan50VehicleGarageIntegrationTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 137; SHA-256: `e694405fa9c33f726620bd13f45516bf2a284b36e6f16e7edf80ef5e30e0d5dd`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
DecorateProfile_AppliesFittedCargoSpeedAndFuel
DecorateProfile_NullOrUnrecorded_NoOp
GetInstalledSlots_ReflectsInstallsAndRemoval
CatastrophicWear_Immobilizes_AndRecoveryAdvancesThenClears
AdvanceRecoveries_NonPositiveDelta_NoProgress
```


# Appendix E.15 — Supporting Code Evidence: `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs`

### `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 464 lines / 20023 bytes.
- SHA-256: `c897d11e111c3ff4e559d01a4be0241b2e291de005cd4fce076090f3ac450eb2`.
- Architecture signals: seeded references=4; save/restore symbols=2; typed event declarations=12; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum VehicleBreakdownKind
public sealed class VehicleBreakdownOutcome
public bool BrokeDown { get; }
public VehicleBreakdownKind Kind { get; }
public float Severity01 { get; }
public float NominalMsV { get; }
public string Cause { get; }
public string VehicleId { get; }
public string SurvivorId { get; set; } = string.Empty;
public static VehicleBreakdownOutcome None(string vehicleId, string reason) => new VehicleBreakdownOutcome(VehicleBreakdownKind.None, 0f, 0f, reason, vehicleId);
public sealed class ExpeditionVehicleState
public string systemId = ExpeditionVehicleSystem.SystemId;
public Dictionary<string, VehicleInstance> ownedVehicles = new Dictionary<string, VehicleInstance>();
public string activeExpeditionVehicleId = string.Empty;
public sealed class VehicleInstance
public string vehicleId = string.Empty;
public string displayName = string.Empty;
public float condition = 100f;
public float fuel;
public float maxFuel = 50f;
public float cargoCapacity = 100f;
public float speedMultiplier = 1f;
public string terrainType = "road";
public bool isBrokenDown;
public string breakdownCause = string.Empty;
public List<string> attachments = new List<string>();
public VehicleTrackGearState trackGear = new VehicleTrackGearState();
public sealed class VehicleTrackGearState
public string gearId = string.Empty;
public float condition = 100f;
public float tractionMultiplier = 1f;
public float breakdownRiskMultiplier = 1f;
public bool IsInstalled => !string.IsNullOrEmpty(gearId);
public float EffectiveTractionMultiplier() {
public float EffectiveBreakdownRiskMultiplier() {
public sealed class VehicleDefinition
public string vehicle_id = string.Empty;
public string display_name = string.Empty;
public float max_fuel = 50f;
public float cargo_capacity = 100f;
public float speed_multiplier = 1f;
public string terrain_type = "road";
public float condition_max = 100f;
public float fuel_consumption_per_km = 0.5f;
public float breakdown_threshold = 0.2f;
public List<string> default_attachments = new List<string>();
public sealed class VehicleTrackGearDefinition
public string gear_id = string.Empty;
public string display_name = string.Empty;
public float traction_multiplier = 1.1f;
public float breakdown_risk_multiplier = 0.9f;
public sealed class VehicleCatalog
public int schema_version = 1;
public List<VehicleDefinition> vehicles = new List<VehicleDefinition>();
public List<VehicleTrackGearDefinition> track_gear = new List<VehicleTrackGearDefinition>();
public sealed class ExpeditionVehicleSystem
public const string SystemId = "expedition_vehicle";
public ExpeditionVehicleState State => _state;
public event Action OnVehicleStateChanged;
public void LoadCatalog(VehicleCatalog catalog) {
public VehicleDefinition? GetDefinition(string id) {
public VehicleTrackGearDefinition? GetTrackGearDefinition(string id) {
public ActionResult AcquireVehicle(string vehicleId) {
public VehicleInstance? GetVehicle(string vehicleId) {
public ActionResult Refuel(string vehicleId, float amount) {
public ActionResult Repair(string vehicleId, float amount) {
public ActionResult AttachEquipment(string vehicleId, string equipmentId) {
public ActionResult InstallTrackGear( string vehicleId, string gearId, float tractionMultiplier, float breakdownRiskMultiplier, float condition = 100f)
public ActionResult InstallTrackGear(string vehicleId, string gearId, float condition = 100f) {
public ActionResult RemoveTrackGear(string vehicleId) {
public ActionResult RepairTrackGear(string vehicleId, float amount) {
public ExpeditionVehicleProfile? CreateExpeditionProfile( string vehicleId, float kmPerTravelTick = 2.5f) {
public event Action<VehicleBreakdownOutcome>? OnBreakdownResolved;
public VehicleBreakdownOutcome ResolvePrepBreakdown(string vehicleId, float distanceKm, ISeededRng? rng = null) {
public ExpeditionVehicleState CaptureState() => CloneState(_state);
public void RestoreState(ExpeditionVehicleState saved) {
```


# Appendix E.16 — Supporting Code Evidence: `Assets/Ashfall.Core/Expeditions/ExpeditionAggregate.cs`

### `Assets/Ashfall.Core/Expeditions/ExpeditionAggregate.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 115 lines / 5002 bytes.
- SHA-256: `513c8d961b30670f32c5040ee0facf4cdb5e869da06f129c6f607f0378be88d9`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExpeditionAggregateState
public string systemId = "expedition_aggregate";
public List<ExpeditionState> expeditions = new List<ExpeditionState>();
public ExpeditionVehicleState vehicles = new ExpeditionVehicleState();
public List<string>? knownLocationIds = new List<string>();
public DiscoveryConsequenceState? discoveryConsequences;
public int completedCount;
public static class VehicleCatalogLoader
public const string FileName = "vehicles.json";
public static VehicleCatalog Load(string dataDir, IFileIO files, IJsonSerializer json) {
public static VehicleCatalog Load(string dataDir) => Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
public static class ExpeditionAggregateCodec
public static string Encode(ExpeditionAggregateState aggregate, IJsonSerializer json) => SaveEnvelopeHelper.CaptureEnvelope(aggregate, json);
public static ExpeditionAggregateState? Decode(string raw, IJsonSerializer json) {
```


# Appendix E.17 — Supporting Code Evidence: `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs`

### `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 864 lines / 36461 bytes.
- SHA-256: `6493ee6924ace913e975210a1a66500670279fd2ffc0d3099902373b0b4fd982`.
- Architecture signals: seeded references=3; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class VehicleCustomizationRecord
public string vehicleId = string.Empty;
public Dictionary<string, string> installedSlots = new Dictionary<string, string>(StringComparer.Ordinal);
public int chassisStressPermille;
public int engineFoulingPermille;
public int transmissionWearPermille;
public bool isImmobilized;
public string immobilizedReason = string.Empty;
public string armorGradeId = string.Empty;
public int armorIntegrityPermille;
public int armorIntegrityMaxPermille;
public string armorMaterialProfileId = string.Empty;
public string armorPurity = FoundryPurityNames.Standard;
public sealed class VehicleRecoveryMission
public string missionId = string.Empty;
public string strandedVehicleId = string.Empty;
public string locationId = string.Empty;
public int requiredFuelUnits = 10;
public int progressTicks;
public int requiredTicks = 120;
public bool isComplete;
public sealed class VehicleGarageState
public string systemId = VehicleGarageSystem.SystemId;
public Dictionary<string, VehicleCustomizationRecord> vehicleRecords = new Dictionary<string, VehicleCustomizationRecord>(StringComparer.Ordinal);
public Dictionary<string, VehicleRecoveryMission> activeRecoveries = new Dictionary<string, VehicleRecoveryMission>(StringComparer.Ordinal);
public int nextRecoveryCounter = 1;
public sealed class VehicleGarageSystem
public const string SystemId = "vehicle_garage";
public const string SlotCargo = "cargo";
public const string SlotProtection = "protection";
public const string SlotMobility = "mobility";
public const string SlotEngine = "engine";
public const string SlotUtility = "utility";
public delegate bool TryGetArmorMaterialQuality(out FoundryMaterialQuality quality);
public TryGetArmorMaterialQuality? ArmorMaterialQualitySource { get; set; }
public Func<string, string?>? VehicleTerrainResolver { get; set; }
public void LoadCatalog(VehicleGarageCatalog catalog) {
public bool HasModification(string modId) => _modCatalog.ContainsKey(modId);
public VehicleModificationDefinition? GetModification(string modId) {
public IReadOnlyDictionary<string, VehicleModificationDefinition> GetAllModifications() => _modCatalog;
public void LoadArmorCatalog(VehicleArmorGradeCatalog catalog) {
public bool HasArmorGrade(string gradeId) => !string.IsNullOrEmpty(gradeId) && _armorCatalog.ContainsKey(gradeId);
public VehicleArmorGradeDefinition? GetArmorGrade(string gradeId) {
public IReadOnlyDictionary<string, VehicleArmorGradeDefinition> GetAllArmorGrades() => _armorCatalog;
public VehicleArmorProfile GetArmorProfile(string vehicleId) {
public static string ArmorConditionBand(int integrityPermille, int maxPermille) {
public VehicleCustomizationRecord? GetRecord(string vehicleId) =>
public bool IsImmobilized(string vehicleId) => GetRecord(vehicleId)?.isImmobilized == true;
public IReadOnlyDictionary<string, VehicleRecoveryMission> ActiveRecoveries => _state.activeRecoveries;
public IReadOnlyDictionary<string, string> GetInstalledSlots(string vehicleId) =>
public void DecorateProfile(ExpeditionVehicleProfile? profile) {
public int AdvanceRecoveries(int deltaTicks) {
public VehicleCustomizationRecord GetOrCreateRecord(string vehicleId) {
public bool HasVehicleRecord(string vehicleId) =>
public bool CanInstallArmorGrade(string vehicleId, string gradeId, IPlayerInventoryPort? inventory, out string reason) {
public bool InstallArmorGrade(string vehicleId, string gradeId, IPlayerInventoryPort? inventory, out string reason) {
public bool ReforgeArmorPlate(string vehicleId, IPlayerInventoryPort? inventory, out string reason) {
public bool CanInstallModification(string vehicleId, string slotType, string modId, IPlayerInventoryPort? inventory, out string reason) {
public bool InstallModification(string vehicleId, string slotType, string modId, IPlayerInventoryPort? inventory, out string reason) {
public bool UninstallModification(string vehicleId, string slotType, IPlayerInventoryPort? inventory, out string reason) {
public float GetEffectiveCargoCapacityDelta(string vehicleId) {
public float GetEffectiveSpeedMultiplierDelta(string vehicleId) {
public float GetEffectiveFuelConsumptionMultiplier(string vehicleId) {
public float GetEffectiveWearRateMultiplier(string vehicleId) {
public int GetEffectiveRadiationProtectionPermille(string vehicleId) {
public void RecordTripWear(string vehicleId, float distanceKm, float roadRoughnessMultiplier = 1f) {
public bool ServiceChassis(string vehicleId, IPlayerInventoryPort? inventory, int repairPermille, out string reason) {
public bool ServiceEngine(string vehicleId, IPlayerInventoryPort? inventory, int repairPermille, out string reason) {
public bool ServiceTransmission(string vehicleId, IPlayerInventoryPort? inventory, int repairPermille, out string reason) {
public bool RegisterRecoveryMission(string vehicleId, string locationId, int requiredFuelUnits, out string missionId, out string reason) {
public bool AdvanceRecoveryMission(string missionId, int deltaTicks, out bool completed) {
public bool CompleteRecoveryMission(string missionId, IPlayerInventoryPort? inventory, out string reason) {
public VehicleGarageState CaptureState() {
public void RestoreState(VehicleGarageState? state) {
```


# Appendix E.18 — Supporting Code Evidence: `src/Host/ExpeditionHostSession.cs`

### `src/Host/ExpeditionHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1663 lines / 85060 bytes.
- SHA-256: `46c1ce80f030a53183aea292df6246ab3278b2c2d481baf91e91cbf1f1c8af37`.
- Architecture signals: seeded references=6; save/restore symbols=13; typed event declarations=11; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExpeditionHostSession : HostSessionBase
public const int DemoSeed = 7071;
public const int VehicleSeed = 7072;
public const float KmPerTravelTick = 2.5f;
public const string StarterVehicleId = "vehicle_utility_quad";
public ExpeditionSystem Engine { get; }
public List<ExpeditionDefinition> Definitions { get; }
public List<ExpeditionDefinition> DemoDefinitions => Definitions;
public DiveInstanceRunner DiveRunner { get; private set; }
public Ashfall.Core.Flags.IFlagLedger Flags { get; set; } = new Ashfall.Core.Flags.CampaignConsequenceLedger();
public DiscoveryConsequenceSystem DiscoveryConsequences { get; }
public event Action<ConsequenceOutcome>? OnDiscoveryConsequenceApplied;
public Action<string, string, int>? ApplyDisease { get; set; }
public JournalSystem? Journal { get; set; }
public Ashfall.Core.Inventory.Inventory? ShelterInventory { get; set; }
public ItemCatalog? Items { get; set; }
public ExpeditionVehicleSystem Vehicles { get; }
public VehicleGarageSystem? Garage { get; set; }
public VouchAccessSystem CrossingGate { get; set; }
public WastelandMapSystem? WastelandMap { get; set; }
public Func<string, bool> ExtraBlocked { get; set; }
public Func<string, FitnessVerdict?>? SurvivorFitnessProvider { get; set; }
public Func<string, RoleFitnessVerdict?>? ExpeditionFitnessProvider { get; set; }
public Func<string, float>? SurvivorMovementSpeedProvider { get; set; }
public Func<string, float>? PackCapacityProvider { get; set; }
public Func<string, WeatherGateBlock?> ExtraGateBlock { get; set; }
public string? GetBlockReason(string locationId) {
public WeatherGateBlock? GetWeatherGateBlock(string locationId) => ExtraGateBlock?.Invoke(locationId);
public void SetEncounterChanceMultiplier(Func<string, float> multiplier) => Engine.SetEncounterChanceMultiplier(multiplier);
public void SetEstimateWeatherInputs(Func<string, ExpeditionWeatherInputs?>? provider) {
public void SetEstimateProtectiveInputs(Func<string, ExpeditionProtectiveInputs?>? provider) {
public void SetEstimateRouteModifiers(Func<string, float>? hazard, Func<string, float>? travel) {
public void AttachDamagedMapFeedback(DamagedMapSystem? damagedMap) {
public string LastEvent { get; private set; } = string.Empty;
public event Action<ExpeditionEncounterBridge.EncounterSurfaced>? OnEncounterSurfaced;
public sealed class TravelCombatTrigger
public string EncounterId = string.Empty;
public string Title = string.Empty;
public string LocationId = string.Empty;
public int DangerLevel;
public IReadOnlyList<string> CombatantIds = Array.Empty<string>();
public event Action<TravelCombatTrigger>? OnTravelEncounterCombatTriggered;
public event Action<string, string, WeatherGateBlock>? OnWeatherGateForced;
public static bool UseEncounterModal { get; set; } = true;
public ExpeditionEncounterBridge Bridge => _bridge;
public Dictionary<string, float> WaterRouteHazards { get; } = new(StringComparer.Ordinal);
public IReadOnlyList<PendingSurfacedEncounter> Pending =>
public EncounterDefinition? FindEncounter(string encounterId) => _narrative?.Find(encounterId);
public void ClearAllPending() => _narrative?.ClearAllPending();
public NarrativeEncounterSystem? NarrativeEngine => _narrative;
public static ExpeditionHostSession Create(string dataDir, NarrativeEncounterSystem narrative = null!, TravelEncounterSystem travel = null!, ICampaignRngManager? campaignRng = null) {
public bool IsLocationBlocked(string locationId) => GetBlockReason(locationId) != null;
public CommandResult StartExpedition( string survivorId, string locationId, ExpeditionStance stance = ExpeditionStance.Stealth, int staminaBudget = 40, string vehicleId = "",
public CommandResult RefuelVehicle(string vehicleId, float units) {
public CommandResult RepairVehicle(string vehicleId, float amount) {
public CommandResult InstallTrackGear(string vehicleId, string gearId, float condition = 100f) {
public CommandResult RemoveTrackGear(string vehicleId) {
public CommandResult RepairTrackGear(string vehicleId, float amount) {
public CommandResult AssembleVehicleFromKit(string kitItemId, Inventory shelterInventory) {
public Action<VehicleBreakdownOutcome>? BreakdownConsequenceSink { get; set; }
public CommandResult StartDemoExpedition(string survivorId, string locationId) => StartExpedition(survivorId, locationId);
public CommandResult DispatchSortie( string survivorId, string locationId, ExpeditionStance stance, int day, string vehicleId = "",
public FitnessVerdict? GetSurvivorFitness(string survivorId) => SurvivorFitnessProvider?.Invoke(survivorId);
public RoleFitnessVerdict? GetExpeditionFitness(string survivorId) => ExpeditionFitnessProvider?.Invoke(survivorId);
public string TickHours(float hours) {
public sealed class EncounterApplicationResult
public string ResolutionId = string.Empty;
public enum Status { NotApplicable, Applied, AlreadyKnown, RejectedCapacity, RejectedInsufficientItems, NoActiveExpedition, SkippedNoAuthority, RejectedUnknownId } public Status Item = Status.NotApplicable; public string ItemId = string.Empty; public int ItemQuantity; public Status Journal = Status.NotApplicable; public string JournalId = string.Empty; public Status Location = Status.NotApplicable; public string LocationId = string.Empty; public Status Flag = Status.NotApplicable; public string FlagId = string.Empty; /// <summary>F17 — micro-location hazard routing outcome. NotApplicable /// for flags without a registered hazard; Applied when the canonical /// disease authority received the consequence exactly once.</summary> public MicroLocationHazardRegistry.HazardStatus Hazard = MicroLocationHazardRegistry.HazardStatus.NotApplicable; public string HazardDiseaseId = string.Empty; }
public EncounterApplicationResult? LastApplication { get; private set; }
public event Action<EncounterApplicationResult>? OnEncounterConsequencesApplied;
public bool EncounterApplyChoice(string encounterId, string choiceId, int day) => EncounterApplyChoice(encounterId, choiceId, day, null!);
public bool EncounterApplyChoice(string encounterId, string choiceId, int day, string locationId) {
public bool ResolveTravelChoiceWithCombat( string encounterId, string choiceId, int day, string locationId, int dangerLevel, int enemyCount) {
public static readonly ExpeditionJournalAuthor Instance = new ExpeditionJournalAuthor();
public string Id => "expedition";
public string DisplayName => "Expedition";
public RiskBiasTrait RiskBias => RiskBiasTrait.Realist;
public string PushLuck(string survivorId) {
public string PushLuckDemo(string survivorId) => PushLuck(survivorId);
public string Retreat(string survivorId) {
public string RetreatDemo(string survivorId) => Retreat(survivorId);
public string EnterCamp( string survivorId, float temperatureC = -10f, string weatherCondition = "Clear", float firewood = 8f, float water = 4f,
public string EnterCampDemo( string survivorId, float temperatureC = -10f, string weatherCondition = "Clear", float firewood = 8f, float water = 4f,
public string CampTick(string survivorId) {
public string CampTickDemo(string survivorId) => CampTick(survivorId);
public string ResolveCampEncounter(string survivorId, string outcome) {
public string ResolveCampEncounterDemo(string survivorId, string outcome) => ResolveCampEncounter(survivorId, outcome);
public string BreakCamp(string survivorId, bool retreat = false) {
public string BreakCampDemo(string survivorId, bool retreat = false) => BreakCamp(survivorId, retreat);
public CampState? GetCampState(string survivorId) => Engine.GetCampState(survivorId);
public string StatusLine() {
public List<ExpeditionState> CaptureSave() => Engine.CaptureState();
public void RestoreSave(List<ExpeditionState> state) => Engine.RestoreState(state);
public ExpeditionAggregateState CaptureSaveAggregate() {
public void RestoreSaveAggregate(ExpeditionAggregateState aggregate) {
public string StartDive(string siteId = "site_exp09_ss_sovereign") {
public string StartDiveDemo(string siteId = "site_exp09_ss_sovereign") => StartDive(siteId);
public string AdvanceDive() {
public string AdvanceDiveDemo() => AdvanceDive();
public string TickDiveOxygen() {
public string TickDiveOxygenDemo() => TickDiveOxygen();
public string CommitDiveChoice(string choice) {
public string CommitDiveChoiceDemo(string choice) => CommitDiveChoice(choice);
public string DiveStatusLine() {
```


# Appendix E.19 — Supporting Code Evidence: `src/Host/ExpeditionHostSession.cs`

### `src/Host/ExpeditionHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1663 lines / 85060 bytes.
- SHA-256: `46c1ce80f030a53183aea292df6246ab3278b2c2d481baf91e91cbf1f1c8af37`.
- Architecture signals: seeded references=6; save/restore symbols=13; typed event declarations=11; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExpeditionHostSession : HostSessionBase
public const int DemoSeed = 7071;
public const int VehicleSeed = 7072;
public const float KmPerTravelTick = 2.5f;
public const string StarterVehicleId = "vehicle_utility_quad";
public ExpeditionSystem Engine { get; }
public List<ExpeditionDefinition> Definitions { get; }
public List<ExpeditionDefinition> DemoDefinitions => Definitions;
public DiveInstanceRunner DiveRunner { get; private set; }
public Ashfall.Core.Flags.IFlagLedger Flags { get; set; } = new Ashfall.Core.Flags.CampaignConsequenceLedger();
public DiscoveryConsequenceSystem DiscoveryConsequences { get; }
public event Action<ConsequenceOutcome>? OnDiscoveryConsequenceApplied;
public Action<string, string, int>? ApplyDisease { get; set; }
public JournalSystem? Journal { get; set; }
public Ashfall.Core.Inventory.Inventory? ShelterInventory { get; set; }
public ItemCatalog? Items { get; set; }
public ExpeditionVehicleSystem Vehicles { get; }
public VehicleGarageSystem? Garage { get; set; }
public VouchAccessSystem CrossingGate { get; set; }
public WastelandMapSystem? WastelandMap { get; set; }
public Func<string, bool> ExtraBlocked { get; set; }
public Func<string, FitnessVerdict?>? SurvivorFitnessProvider { get; set; }
public Func<string, RoleFitnessVerdict?>? ExpeditionFitnessProvider { get; set; }
public Func<string, float>? SurvivorMovementSpeedProvider { get; set; }
public Func<string, float>? PackCapacityProvider { get; set; }
public Func<string, WeatherGateBlock?> ExtraGateBlock { get; set; }
public string? GetBlockReason(string locationId) {
public WeatherGateBlock? GetWeatherGateBlock(string locationId) => ExtraGateBlock?.Invoke(locationId);
public void SetEncounterChanceMultiplier(Func<string, float> multiplier) => Engine.SetEncounterChanceMultiplier(multiplier);
public void SetEstimateWeatherInputs(Func<string, ExpeditionWeatherInputs?>? provider) {
public void SetEstimateProtectiveInputs(Func<string, ExpeditionProtectiveInputs?>? provider) {
public void SetEstimateRouteModifiers(Func<string, float>? hazard, Func<string, float>? travel) {
public void AttachDamagedMapFeedback(DamagedMapSystem? damagedMap) {
public string LastEvent { get; private set; } = string.Empty;
public event Action<ExpeditionEncounterBridge.EncounterSurfaced>? OnEncounterSurfaced;
public sealed class TravelCombatTrigger
public string EncounterId = string.Empty;
public string Title = string.Empty;
public string LocationId = string.Empty;
public int DangerLevel;
public IReadOnlyList<string> CombatantIds = Array.Empty<string>();
public event Action<TravelCombatTrigger>? OnTravelEncounterCombatTriggered;
public event Action<string, string, WeatherGateBlock>? OnWeatherGateForced;
public static bool UseEncounterModal { get; set; } = true;
public ExpeditionEncounterBridge Bridge => _bridge;
public Dictionary<string, float> WaterRouteHazards { get; } = new(StringComparer.Ordinal);
public IReadOnlyList<PendingSurfacedEncounter> Pending =>
public EncounterDefinition? FindEncounter(string encounterId) => _narrative?.Find(encounterId);
public void ClearAllPending() => _narrative?.ClearAllPending();
public NarrativeEncounterSystem? NarrativeEngine => _narrative;
public static ExpeditionHostSession Create(string dataDir, NarrativeEncounterSystem narrative = null!, TravelEncounterSystem travel = null!, ICampaignRngManager? campaignRng = null) {
public bool IsLocationBlocked(string locationId) => GetBlockReason(locationId) != null;
public CommandResult StartExpedition( string survivorId, string locationId, ExpeditionStance stance = ExpeditionStance.Stealth, int staminaBudget = 40, string vehicleId = "",
public CommandResult RefuelVehicle(string vehicleId, float units) {
public CommandResult RepairVehicle(string vehicleId, float amount) {
public CommandResult InstallTrackGear(string vehicleId, string gearId, float condition = 100f) {
public CommandResult RemoveTrackGear(string vehicleId) {
public CommandResult RepairTrackGear(string vehicleId, float amount) {
public CommandResult AssembleVehicleFromKit(string kitItemId, Inventory shelterInventory) {
public Action<VehicleBreakdownOutcome>? BreakdownConsequenceSink { get; set; }
public CommandResult StartDemoExpedition(string survivorId, string locationId) => StartExpedition(survivorId, locationId);
public CommandResult DispatchSortie( string survivorId, string locationId, ExpeditionStance stance, int day, string vehicleId = "",
public FitnessVerdict? GetSurvivorFitness(string survivorId) => SurvivorFitnessProvider?.Invoke(survivorId);
public RoleFitnessVerdict? GetExpeditionFitness(string survivorId) => ExpeditionFitnessProvider?.Invoke(survivorId);
public string TickHours(float hours) {
public sealed class EncounterApplicationResult
public string ResolutionId = string.Empty;
public enum Status { NotApplicable, Applied, AlreadyKnown, RejectedCapacity, RejectedInsufficientItems, NoActiveExpedition, SkippedNoAuthority, RejectedUnknownId } public Status Item = Status.NotApplicable; public string ItemId = string.Empty; public int ItemQuantity; public Status Journal = Status.NotApplicable; public string JournalId = string.Empty; public Status Location = Status.NotApplicable; public string LocationId = string.Empty; public Status Flag = Status.NotApplicable; public string FlagId = string.Empty; /// <summary>F17 — micro-location hazard routing outcome. NotApplicable /// for flags without a registered hazard; Applied when the canonical /// disease authority received the consequence exactly once.</summary> public MicroLocationHazardRegistry.HazardStatus Hazard = MicroLocationHazardRegistry.HazardStatus.NotApplicable; public string HazardDiseaseId = string.Empty; }
public EncounterApplicationResult? LastApplication { get; private set; }
public event Action<EncounterApplicationResult>? OnEncounterConsequencesApplied;
public bool EncounterApplyChoice(string encounterId, string choiceId, int day) => EncounterApplyChoice(encounterId, choiceId, day, null!);
public bool EncounterApplyChoice(string encounterId, string choiceId, int day, string locationId) {
public bool ResolveTravelChoiceWithCombat( string encounterId, string choiceId, int day, string locationId, int dangerLevel, int enemyCount) {
public static readonly ExpeditionJournalAuthor Instance = new ExpeditionJournalAuthor();
public string Id => "expedition";
public string DisplayName => "Expedition";
public RiskBiasTrait RiskBias => RiskBiasTrait.Realist;
public string PushLuck(string survivorId) {
public string PushLuckDemo(string survivorId) => PushLuck(survivorId);
public string Retreat(string survivorId) {
public string RetreatDemo(string survivorId) => Retreat(survivorId);
public string EnterCamp( string survivorId, float temperatureC = -10f, string weatherCondition = "Clear", float firewood = 8f, float water = 4f,
public string EnterCampDemo( string survivorId, float temperatureC = -10f, string weatherCondition = "Clear", float firewood = 8f, float water = 4f,
public string CampTick(string survivorId) {
public string CampTickDemo(string survivorId) => CampTick(survivorId);
public string ResolveCampEncounter(string survivorId, string outcome) {
public string ResolveCampEncounterDemo(string survivorId, string outcome) => ResolveCampEncounter(survivorId, outcome);
public string BreakCamp(string survivorId, bool retreat = false) {
public string BreakCampDemo(string survivorId, bool retreat = false) => BreakCamp(survivorId, retreat);
public CampState? GetCampState(string survivorId) => Engine.GetCampState(survivorId);
public string StatusLine() {
public List<ExpeditionState> CaptureSave() => Engine.CaptureState();
public void RestoreSave(List<ExpeditionState> state) => Engine.RestoreState(state);
public ExpeditionAggregateState CaptureSaveAggregate() {
public void RestoreSaveAggregate(ExpeditionAggregateState aggregate) {
public string StartDive(string siteId = "site_exp09_ss_sovereign") {
public string StartDiveDemo(string siteId = "site_exp09_ss_sovereign") => StartDive(siteId);
public string AdvanceDive() {
public string AdvanceDiveDemo() => AdvanceDive();
public string TickDiveOxygen() {
public string TickDiveOxygenDemo() => TickDiveOxygen();
public string CommitDiveChoice(string choice) {
public string CommitDiveChoiceDemo(string choice) => CommitDiveChoice(choice);
public string DiveStatusLine() {
```


# Appendix F.20 — Supporting Data Evidence: `Assets/StreamingAssets/Data/vehicles.json`

### `Assets/StreamingAssets/Data/vehicles.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 3044 bytes / 3044 characters.
- SHA-256: `cdec845708de1d2d3181767aeed9f4b4565a48596835415340ed557fd9b9ce82`.
- Root keys: `schema_version`, `track_gear`, `vehicles`.

Array-path census (minimum, maximum, observed rows):

```text
track_gear: min=1, max=1, observed_paths=1
vehicles: min=8, max=8, observed_paths=1
vehicles[].default_attachments: min=0, max=0, observed_paths=2
```

Representative record fields:

- `breakdown_threshold`
- `cargo_capacity`
- `condition_max`
- `default_attachments`
- `display_name`
- `fuel_consumption_per_km`
- `max_fuel`
- `speed_multiplier`
- `terrain_type`
- `vehicle_id`


# Appendix F.21 — Supporting Data Evidence: `Assets/StreamingAssets/Data/vehicle_armor_grades.json`

### `Assets/StreamingAssets/Data/vehicle_armor_grades.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 4568 bytes / 4568 characters.
- SHA-256: `8e62dc6b78f7e97b107078bd3802577edfdba0b399abc8e70d1479dd76d03faa`.
- Root keys: `default_grade_id`, `grades`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
grades: min=5, max=5, observed_paths=1
grades[].compatible_terrain_types: min=3, max=3, observed_paths=2
grades[].install_cost: min=0, max=2, observed_paths=2
grades[].reforge_cost: min=0, max=1, observed_paths=2
grades[].tags: min=2, max=3, observed_paths=2
```

Representative record fields:

- `compatible_terrain_types`
- `description`
- `display_name`
- `fuel_consumption_multiplier`
- `id`
- `install_cost`
- `install_labor_ticks`
- `integrity_pool_permille`
- `is_default`
- `mitigation_permille`
- `reforge_cost`
- `speed_multiplier_delta`
- `tags`
- `tier`
- `wear_absorption_permille`

Representative identifiers (ordered, capped for readability):

```text
grade_0_stock
grade_1_scrap_plate
grade_2_sheet_plate
grade_3_composite_plate
grade_4_alloyed_heavy_plate
```


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/ExpeditionVehicleLogisticsTests.cs`

### `Ashfall.Core.Tests/ExpeditionVehicleLogisticsTests.cs`

- Current test declarations: Fact=17, Theory=0, InlineData=0.
- File lines: 493; SHA-256: `2f574c226a8a522a1d4925d66d9c507ecb75d09ebb8593d907b37741f4991752`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
VehicleSpeed_ShortensOutboundTravel
VehicleBreakdown_IsDeterministic_AndRevertsToFoot
VehicleBreakdown_FiresEventOnce
NeutralVehicle_NeverConsumesBreakdownRolls
Estimate_MirrorsTickMath_ForFootAndVehicle
Bridge_ProjectsAuthorityConditionIntoCombatToken
Bridge_PrefersOwnerAndBestCondition
Bridge_WriteBack_ConvertsUnits_ThroughAuthority
Bridge_Readiness_FollowsAuthorityRisks
AggregateCodec_RoundTripsExpeditionsAndGarage
RestoreCompletedCount_SurvivesAggregateRestore
AggregateCodec_MigratesLegacyEnvelopeAndBareList
AggregateState_RestoreContinuesInFlightVehicleExpedition
VehicleCatalogLoader_ReadsDataAuthority_AndToleratesAbsence
TrackGear_InstallsRepairsAndPersistsNormalizedEffects
TrackGear_ProfileProjectionKeepsTravelMathInCore
TrackGear_RejectsInvalidFactsWithoutMutation
```


# Appendix G.23 — Supporting Regression Evidence: `Ashfall.Core.Tests/Expeditions/VehicleGarageSystemTests.cs`

### `Ashfall.Core.Tests/Expeditions/VehicleGarageSystemTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 225; SHA-256: `e90e41b6cc29857ddf5f73315bd919423a85ff32fb61f2a6d30b102d23aae7cb`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
InstallModification_DeductsInventoryAndAppliesEffects
InstallModification_FailsOnMismatchedSlotOrInsufficientMaterials
TripWear_AccumulatesAndCanImmobilize
ServiceComponents_ConsumesMaterialsAndRestoresHealth
RecoveryMission_LifecycleCompletesAndClearsImmobilization
StateRoundtrip_PreservesModificationsAndWear
```


# Appendix G.24 — Supporting Regression Evidence: `Ashfall.Core.Tests/Expeditions/Plan50VehicleGarageIntegrationTests.cs`

### `Ashfall.Core.Tests/Expeditions/Plan50VehicleGarageIntegrationTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 137; SHA-256: `e694405fa9c33f726620bd13f45516bf2a284b36e6f16e7edf80ef5e30e0d5dd`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
DecorateProfile_AppliesFittedCargoSpeedAndFuel
DecorateProfile_NullOrUnrecorded_NoOp
GetInstalledSlots_ReflectsInstallsAndRemoval
CatastrophicWear_Immobilizes_AndRecoveryAdvancesThenClears
AdvanceRecoveries_NonPositiveDelta_NoProgress
```


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| vehicle definitions, ownership, fuel, repair, equipment, track and profiles | ExpeditionVehicleSystem | vehicle JSON definitions | VehicleCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| vehicle definitions, ownership, fuel, repair, equipment, track and profiles | ExpeditionVehicleSystem | garage/customization and armor integration | VehicleGarageSystem | Owner emits/reads a typed fact; no mirror state. |
| vehicle definitions, ownership, fuel, repair, equipment, track and profiles | ExpeditionVehicleSystem | expedition composition and host commands | ExpeditionHostSession | Owner emits/reads a typed fact; no mirror state. |
| vehicle definitions, ownership, fuel, repair, equipment, track and profiles | ExpeditionVehicleSystem | current garage UI | VehicleGaragePanel | Owner emits/reads a typed fact; no mirror state. |
| vehicle definitions, ownership, fuel, repair, equipment, track and profiles | ExpeditionVehicleSystem | vehicle math/save/catalog contracts | ExpeditionVehicleLogisticsTests | Owner emits/reads a typed fact; no mirror state. |
| vehicle JSON definitions | VehicleCatalogLoader | vehicle definitions, ownership, fuel, repair, equipment, track and profiles | ExpeditionVehicleSystem | Owner emits/reads a typed fact; no mirror state. |
| vehicle JSON definitions | VehicleCatalogLoader | garage/customization and armor integration | VehicleGarageSystem | Owner emits/reads a typed fact; no mirror state. |
| vehicle JSON definitions | VehicleCatalogLoader | expedition composition and host commands | ExpeditionHostSession | Owner emits/reads a typed fact; no mirror state. |
| vehicle JSON definitions | VehicleCatalogLoader | current garage UI | VehicleGaragePanel | Owner emits/reads a typed fact; no mirror state. |
| vehicle JSON definitions | VehicleCatalogLoader | vehicle math/save/catalog contracts | ExpeditionVehicleLogisticsTests | Owner emits/reads a typed fact; no mirror state. |
| garage/customization and armor integration | VehicleGarageSystem | vehicle definitions, ownership, fuel, repair, equipment, track and profiles | ExpeditionVehicleSystem | Owner emits/reads a typed fact; no mirror state. |
| garage/customization and armor integration | VehicleGarageSystem | vehicle JSON definitions | VehicleCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| garage/customization and armor integration | VehicleGarageSystem | expedition composition and host commands | ExpeditionHostSession | Owner emits/reads a typed fact; no mirror state. |
| garage/customization and armor integration | VehicleGarageSystem | current garage UI | VehicleGaragePanel | Owner emits/reads a typed fact; no mirror state. |
| garage/customization and armor integration | VehicleGarageSystem | vehicle math/save/catalog contracts | ExpeditionVehicleLogisticsTests | Owner emits/reads a typed fact; no mirror state. |
| expedition composition and host commands | ExpeditionHostSession | vehicle definitions, ownership, fuel, repair, equipment, track and profiles | ExpeditionVehicleSystem | Owner emits/reads a typed fact; no mirror state. |
| expedition composition and host commands | ExpeditionHostSession | vehicle JSON definitions | VehicleCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| expedition composition and host commands | ExpeditionHostSession | garage/customization and armor integration | VehicleGarageSystem | Owner emits/reads a typed fact; no mirror state. |
| expedition composition and host commands | ExpeditionHostSession | current garage UI | VehicleGaragePanel | Owner emits/reads a typed fact; no mirror state. |
| expedition composition and host commands | ExpeditionHostSession | vehicle math/save/catalog contracts | ExpeditionVehicleLogisticsTests | Owner emits/reads a typed fact; no mirror state. |
| current garage UI | VehicleGaragePanel | vehicle definitions, ownership, fuel, repair, equipment, track and profiles | ExpeditionVehicleSystem | Owner emits/reads a typed fact; no mirror state. |
| current garage UI | VehicleGaragePanel | vehicle JSON definitions | VehicleCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| current garage UI | VehicleGaragePanel | garage/customization and armor integration | VehicleGarageSystem | Owner emits/reads a typed fact; no mirror state. |
| current garage UI | VehicleGaragePanel | expedition composition and host commands | ExpeditionHostSession | Owner emits/reads a typed fact; no mirror state. |
| current garage UI | VehicleGaragePanel | vehicle math/save/catalog contracts | ExpeditionVehicleLogisticsTests | Owner emits/reads a typed fact; no mirror state. |
| vehicle math/save/catalog contracts | ExpeditionVehicleLogisticsTests | vehicle definitions, ownership, fuel, repair, equipment, track and profiles | ExpeditionVehicleSystem | Owner emits/reads a typed fact; no mirror state. |
| vehicle math/save/catalog contracts | ExpeditionVehicleLogisticsTests | vehicle JSON definitions | VehicleCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| vehicle math/save/catalog contracts | ExpeditionVehicleLogisticsTests | garage/customization and armor integration | VehicleGarageSystem | Owner emits/reads a typed fact; no mirror state. |
| vehicle math/save/catalog contracts | ExpeditionVehicleLogisticsTests | expedition composition and host commands | ExpeditionHostSession | Owner emits/reads a typed fact; no mirror state. |
| vehicle math/save/catalog contracts | ExpeditionVehicleLogisticsTests | current garage UI | VehicleGaragePanel | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Current evidence requires a bounded owner/reachability audit; no new authority is implied. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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

> **DR-02 — The docs tree has substantially more subdirectories than the v1.0 map. VERIFIED.**
Live `docs/` subdirectories observed in the audit include (selection; the listing was long and partially truncated): `adr/`, `agents/`, `architecture/`, `archive/`, `balance/`, `bodymind/`, `campaign/`, `cartography/`, `ci/`, `cli/`, `collectibles/`, `combat/`, `content/`, `contracts/`, `crafting/`, `crossing/`, `culture/`, `decisions/`, `design/`, `discovery/`, `duty_roster/`, `ecology/`, `economy/`, `endgame/`, `expansions/`, `expeditions/`, `faction_war/`, `factions/`, `foreman/`, `forensics/`, `foundry/`, `gaps/`, `governance/`, `greenhouse/`, `health/`, `holdfast/`, `hygiene/`, `i18n/`, `implementation/`, `incidents/`, `integration/`, `journal/`, `lore/`, `maritime/`, `medical/`, `memorials/`, `mods/`, `moral/`, `moral_choice/`, `muster/`, `narrative/`, `onboarding/`, `orbital/`, `perf/`, `phantoms/`, `plans/`, `power/`, `process/`, `production/`, and a `player_surface_manifest.json`. Two of these — `gaps/` and `incidents/` — are first-class *expansion feedstock*: directories whose entire purpose is to record what is missing or broken. The Factory Protocol (Part II, step 2) now treats `docs/gaps/` and `docs/incidents/` as mandatory inputs.

> **DR-03 — New top-level authority documents absent from the v1.0 docs map. VERIFIED.**
Observed live and not listed in v1.0 Part 5.8: `ECONOMY_FAIRNESS_AUDIT.md`, `ENGINE_SUPPORT_POLICY.md`, `GODOT_MIGRATION_STATUS.md`, `REPO_HISTORY_REWRITE.md`, `HUMAN_AUTHORSHIP.md`, `AI_DISCLOSURE.md`, `ASSET_MIGRATION_LEDGER.md`, `CODEX_SOURCE_MATRIX.md`, `ARCHIVE_INDEX.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, `MEDICAL_30_DAY_CAPACITY_REPORT.md`, `SHELTER_MAINTENANCE_MATRIX.md`, `SHELTER_30_DAY_MAINTENANCE_REPORT.md`, `L10N_WAVE2_ROADMAP.md`, `INPUT.md`, `RELEASE_EXPORT.md`, `ENGINE_SUPPORT_POLICY.md`. Of these, `ECONOMY_FAIRNESS_AUDIT.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, and `SHELTER_MAINTENANCE_MATRIX.md` are pre-computed balance baselines: they convert Lane C (economy and balance) planning from speculative to evidence-anchored. Subject plans in Lane C must cite these baselines instead of re-deriving numbers.

> C1 Shelter operations (rooms, thermal, schedules, fire, decor, barter, noise, prisoners, sanitation, airlock, decon, atmosphere) · C2 Medical pipeline (disease, dose ledger, ARS, surgery, autopsy, pharma, diagnostics, therapies, dependency, crises) · C3 Water, food, agriculture (treatment, condensers, wells, brine, nutrition, kitchen, preservation, grain, greenhouse, crops, aquaponics, apiculture) · C4 Power and industry (grid, SOFC, solar, kinetic, geothermal, foundry, CVD diamond, coatings, optics, powder metallurgy, pyrolysis, Fischer-Tropsch, chlor-alkali, acids, fermentation, ethanol, air separation, metrology) · C5 Expeditions and travel (destinations, scavenging tables, vehicles, waystations, caravans, routes, travel encounters, micro-locations) · C6 Map and geography (wasteland map, damaged zones, fog, route gates, cartography, survey instruments) · C7 Factions and war (stance, doctrines, war chains, tributes, treaties, embargoes, espionage, psyops, infiltration, musters, labor camps, bounties) · C8 Radio and information (stations, programs, intercepts, distress signals, rumors, sound ranging, direction finding, NVIS, heliograph) · C9 Survivors and interiority (needs, skills, traits, arcs, trauma, guilt, therapies, relations, caregiving, beliefs, rituals, memorials, final wishes, lineage, cohorts, apprenticeships) · C10 Quests and moral choice (questline master, dynamic questlines, personal quests, NPC arcs, moral-choice chains/flags/gossip, branching, bureaucratic morality, expansion quests) · C11 Economy (market, baselines, regional prices, shocks, rumors, black market, debt ledger, tributes, trade screens, tell lines) · C12 Weather and Year of Ash (weather system, seasons, effects, gates, hardening, storm windows, Year-of-Ash families, epilogue pressure) · C13 Endgame and epilogue (Reckoning, verdict, epilogue matrix, chronicle, muster epilogues, standing records, census) · C14 Ecology and wildlife (migration, trapping, ecosystem, bestiary, flora, infestations, contagion, pathogens, crop genomes) · C15 Defense and security (perimeter, defense grid, sky defense, ordnance, chemical defense, orbital harrow, interlocks, EMP effects) · C16 Progression and meta (skills, research, collectibles, trophies, achievements, difficulty presets, XP wave, codex, field guide, bestiary, L10N, mods, settings, input) · C17 Host surface and UI (panels, shell, focus navigation, snapshots, a11y, briefings, dashboards).

> | Cluster | Opening archetype | Confidence |
|---|---|---|
| C1 | Bureaucratic texture for under-documented rooms: shift notices, maintenance glitch reports, load-shed amendments for rooms lacking corpus coverage | HIGH CONFIDENCE |
| C2 | Casebook and therapy-note expansion for affliction states with thin prose coverage; dose-treatment narrative pairing against `MEDICAL_DOSE_TREATMENT_MATRIX.md` | HIGH CONFIDENCE |
| C3 | Assay/log corpus for preservation and processing chains that have catalogs but no narrative corpus twin (v1.0 Part 16.4 pattern: every process ships technical + prose) | HIGH CONFIDENCE |
| C4 | Same pattern for the newest industrial catalogs confirmed live in DR-04 (`hydraulic_extrusion`, `metrology_standards`): audit records, calibration logs | HIGH CONFIDENCE |
| C5 | Expedition field reports and waypoint notes for destinations with sparse `arrival_description`/`revisit_description` coverage; route-waypoint batches | HIGH CONFIDENCE |
| C6 | Gazetteer entries and damaged-zone survey prose; cartographic marginalia | INFERENCE — verify current coverage |
| C7 | Communiqué, directive, and verdict-corpus expansion for factions with thin public/private language separation | HIGH CONFIDENCE |
| C8 | Radio rundown/transcript batches for stations with thin programming; numbers-station and cipher follow-ups | HIGH CONFIDENCE — but distress-signal content is SEALED under `CF-P1-DISTRESS-CONTENT-SEAL` (DR-06); do not add signal scenarios |
| C9 | Delayed moral-choice callbacks (~100-day returns) via `IFlagLedger` flags; phantom-memory triggers tied to surviving cohorts | HIGH CONFIDENCE (v1.0 Part 7 gap 2) |
| C10 | Quest prose fields (`quest_hook`, `objective_text`, outcome texts) for quest records with skeleton prose; follow Part 9 contracts exactly | HIGH CONFIDENCE |
| C11 | Ledger, statement, and debt-template prose; rumor batches within deterministic bands | HIGH CONFIDENCE |
| C12 | Mid-winter slump pressure (Days 90–180) story arcs; storm-window almanac entries | HIGH CONFIDENCE (v1.0 Part 7 gap 1) |
| C13 | Epilogue-chronicle depth for under-served permutations of the 32-permutation matrix | HIGH CONFIDENCE |
| C14 | Bestiary and natural-history corpus extension; mutated-botanical and limnology follow-on batches | HIGH CONFIDENCE |
| C15 | Defense-log and ordnance-manifest prose; orbital-harrow telemetry transcripts | INFERENCE — verify coverage |
| C16 | Codex and field-guide entries for systems that gained content since the last codex wave | HIGH CONFIDENCE |
| C17 | Ambient environmental text and atmosphere cues for panels rendering newer systems with sparse surface prose | INFERENCE — verify via `--ui-layout-selftest` and snapshot coverage |

> | Cluster | Opening archetype | Confidence |
|---|---|---|
| C1 | Room-level effect extensions routed through `IsRoomPowered`; shelter-failure follow-ons building on the quarantined failure-effects wiring logs observed in `docs/plans/` | HIGH CONFIDENCE |
| C2 | Ward-staffing and recovery-ramp follow-ons are CLOSED (Plan 24, DR-06); open instead: cross-links between medical and cohort/lineage (child health), and between dose ledger and Year-of-Ash fallout windows | PROPOSAL — premise sweep required |
| C3 | Zoonosis-style bridges: kitchen/preservation × disease; cellar-rot × greenhouse economics; apiculture × morale | PROPOSAL |
| C4 | Bind the newest industrial catalogs (DR-04) into consumption/production ledgers through the existing power-grid and foundry seams | PROPOSAL — needs live loader verification |
| C5 | Per-destination scavenging-table parity for destinations beyond the 49-table coverage; vehicle-breakdown consequences into medical and dose ledgers | HIGH CONFIDENCE |
| C6 | Flooded-route topology tags and authored map edges (foreman-flagged open decision — needs the named signature first) | BLOCKED — decision-gated |
| C7 | FactionWar per-strike emitter extension (foreman-flagged open decision — needs signature) | BLOCKED — decision-gated |
| C8 | Radio-signal follow-up chaining is SEALED (DISTRESS-SIGNALS-9-12 COMPLETE, DR-06); open instead: market-rumor band extension and intercept-driven journal depth | HIGH CONFIDENCE |
| C9 | Survivor interiority bridges: belief movements × faction stance; memorial rites × epilogue evidence; chemical dependency × medical ward | PROPOSAL |
| C10 | Quest state reopening after new discoveries (failure-recovery grammar, v1.0 Part 6.7); moral-choice flag consumers beyond the flag ledger | HIGH CONFIDENCE |
| C11 | Black-market funds/goods legs remain decision-gated (canonical funds authority); merchant restock priority is SEALED by DEC-05 (DR-06) | BLOCKED / SEALED |
| C12 | Year-of-Ash tick-window extensions (180–360) for systems not yet producing winter pressure | PROPOSAL |
| C13 | Reckoning evidence enrollment for systems added since the last endgame wave (19A/19B/19C closed, DR-06) | HIGH CONFIDENCE |
| C14 | Trapping→disease zoonosis bridge exists; open: migration × expedition route encounters; infestation × crop economy | PROPOSAL |
| C15 | EMP effects exist (shelter EMP/medical power logs observed); open: defense grid × warlord siege math; sky-armor × orbital harrow telemetry | PROPOSAL |
| C16 | XP Expansion W1 is ACTIVE (DR-06): difficulty-authority consumer binding is the sanctioned open seam in this cluster — extend it, do not parallel it | HIGH CONFIDENCE |
| C17 | Panels rendering stale or missing data for newer systems; verify against `--ui-layout-selftest` before claiming | HIGH CONFIDENCE |

> Anchored by the live baselines (DR-03): `ECONOMY_FAIRNESS_AUDIT.md`, `ECONOMY_PRICE_FACTOR_MATRIX.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `docs/balance/`.

> | Cluster | Opening archetype | Confidence |
|---|---|---|
| C4 | Fuel and feedstock income-versus-expenditure audits for each industrial chain; dominated-process analysis (do any catalogs produce strictly dominated outputs?) | HIGH CONFIDENCE |
| C5 | Scavenging E[value] re-runs after any loot authoring (Plan 76.2 harness pattern); vehicle dominance follow-ups against the live dominance table | HIGH CONFIDENCE |
| C7 | Tribute-cycle sustainability (7-day cadence) versus mid-game income; embargo economic pressure | HIGH CONFIDENCE |
| C11 | Price-shock and rumor-band systemic outcomes; debt-interest runaway analysis; black-market pricing tiers | HIGH CONFIDENCE |
| C12 | Winter resource compression (Days 90–180): calories, fuel, filters, morale — sustainability-day math per difficulty preset | HIGH CONFIDENCE |
| C14 | Trapping yield versus equipment degradation cost; zoonosis risk premium on uncooked yield | HIGH CONFIDENCE |
| All others | Balance audits only where numbers exist; never invent tuning targets without an intended design statement | — |

> | Cluster | Opening archetype | Confidence |
|---|---|---|
| C2 | Dose-treatment matrix paired tests against `MEDICAL_DOSE_TREATMENT_MATRIX.md` | HIGH CONFIDENCE |
| C11 | Debt-ledger consequence dispatcher coverage; rumor-band determinism pins | HIGH CONFIDENCE |
| C10 | Moral-choice flag consumer coverage for newly added consumers | HIGH CONFIDENCE |
| C13 | Epilogue permutation reachability tests for under-served permutations | HIGH CONFIDENCE |
| Cross | Determinism two-pass proofs for every new simulation; TEST-AGGREGATION metadata for catalog checks | CANON process |

**Applied constraints:** one bounded outcome, live-source collision sweep, explicit data/loader/consumer/save/test seams, no parallel authority, no unsupported content growth, and a final precision pass. Master file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Recorded SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.


# Appendix — Deep integration architecture

# Appendix — Deep Integration Architecture

## A. Boundary and responsibility map

Current evidence and safe integration boundary for Plan 60: Vehicle Roster, Expedition Logistics and Garage Reachability.

- **vehicle definitions, ownership, fuel, repair, equipment, track and profiles** remains with `ExpeditionVehicleSystem` at `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs`. Sole vehicle logistics owner.
- **vehicle JSON definitions** remains with `VehicleCatalogLoader` at `Assets/Ashfall.Core/Expeditions/ExpeditionAggregate.cs`. Static catalog owner.
- **garage/customization and armor integration** remains with `VehicleGarageSystem` at `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs`. Separate current garage concern.
- **expedition composition and host commands** remains with `ExpeditionHostSession` at `src/Host/ExpeditionHostSession.cs`. Thin host seam.
- **current garage UI** remains with `VehicleGaragePanel` at `src/UI/VehicleGaragePanel.cs`. Presentation/commands only.
- **vehicle math/save/catalog contracts** remains with `ExpeditionVehicleLogisticsTests` at `Ashfall.Core.Tests/ExpeditionVehicleLogisticsTests.cs`. Focused evidence.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load 8 vehicles and track gear
2. validate ranges/references
3. read current owned vehicle state
4. present garage and expedition availability
5. route acquire/refuel/repair/attach commands
6. create expedition profile through current owner
7. capture vehicle/expedition/garage state

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- Vehicle definitions are immutable catalog rows.
- ExpeditionVehicleState owns owned vehicle instances and condition/fuel/equipment.
- ExpeditionAggregate persists active expedition and vehicle state.
- Armor/modules/track gear retain their current separate catalogs and state.

- A vehicle must be owned before expedition use.
- Fuel, cargo, speed, condition and breakdown math remain in current owners.
- A breakdown fallback to foot is deterministic and visible.
- An attachment cannot be installed on an incompatible vehicle without current validation.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/Host/ExpeditionHostSession.cs
- src/UI/VehicleGaragePanel.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/ExpeditionVehicleLogisticsTests.cs
- Ashfall.Core.Tests/Expeditions/VehicleGarageSystemTests.cs
- Ashfall.Core.Tests/Expeditions/Plan50VehicleGarageIntegrationTests.cs

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
| S-01 | 60-01 8 vehicles load | load 8 vehicles and track gear | Vehicle definitions are immutable catalog rows. | A row is counted as reachable without an acquisition/consumer path. | ExpeditionVehicleSystem |
| S-02 | 60-02 track gear load | validate ranges/references | ExpeditionVehicleState owns owned vehicle instances and condition/fuel/equipment. | Fuel/cargo is double charged. | ExpeditionVehicleSystem |
| S-03 | 60-03 acquisition refusal | read current owned vehicle state | ExpeditionAggregate persists active expedition and vehicle state. | Breakdown state resets on reload. | ExpeditionVehicleSystem |
| S-04 | 60-04 fuel use | present garage and expedition availability | Armor/modules/track gear retain their current separate catalogs and state. | Garage panel invents a vehicle. | ExpeditionVehicleSystem |
| S-05 | 60-05 repair | route acquire/refuel/repair/attach commands | Vehicle definitions are immutable catalog rows. | A new vehicle save duplicates current state. | ExpeditionVehicleSystem |
| S-06 | 60-06 cargo profile | create expedition profile through current owner | ExpeditionVehicleState owns owned vehicle instances and condition/fuel/equipment. | A row is counted as reachable without an acquisition/consumer path. | ExpeditionVehicleSystem |
| S-07 | 60-07 breakdown replay | capture vehicle/expedition/garage state | ExpeditionAggregate persists active expedition and vehicle state. | Fuel/cargo is double charged. | ExpeditionVehicleSystem |
| S-08 | 60-08 attachment validation | load 8 vehicles and track gear | Armor/modules/track gear retain their current separate catalogs and state. | Breakdown state resets on reload. | ExpeditionVehicleSystem |
| S-09 | 60-09 save continuation | validate ranges/references | Vehicle definitions are immutable catalog rows. | Garage panel invents a vehicle. | ExpeditionVehicleSystem |
| S-10 | 60-10 garage UI | read current owned vehicle state | ExpeditionVehicleState owns owned vehicle instances and condition/fuel/equipment. | A new vehicle save duplicates current state. | ExpeditionVehicleSystem |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 60-TC-01 schema/count | data | schema/count; verify the named current owner and its negative boundary without inventing a second authority. | ExpeditionVehicleSystem |
| T-02 | 60-TC-02 range validation | unit | range validation; verify the named current owner and its negative boundary without inventing a second authority. | ExpeditionVehicleSystem |
| T-03 | 60-TC-03 ownership | persistence | ownership; verify the named current owner and its negative boundary without inventing a second authority. | ExpeditionVehicleSystem |
| T-04 | 60-TC-04 fuel/repair | determinism | fuel/repair; verify the named current owner and its negative boundary without inventing a second authority. | ExpeditionVehicleSystem |
| T-05 | 60-TC-05 profile math | host | profile math; verify the named current owner and its negative boundary without inventing a second authority. | ExpeditionVehicleSystem |
| T-06 | 60-TC-06 breakdown determinism | UI/accessibility | breakdown determinism; verify the named current owner and its negative boundary without inventing a second authority. | ExpeditionVehicleSystem |
| T-07 | 60-TC-07 save round trip | cross-system | save round trip; verify the named current owner and its negative boundary without inventing a second authority. | ExpeditionVehicleSystem |
| T-08 | 60-TC-08 armor/track boundary | data | armor/track boundary; verify the named current owner and its negative boundary without inventing a second authority. | ExpeditionVehicleSystem |
| T-09 | 60-TC-09 UI truth | unit | UI truth; verify the named current owner and its negative boundary without inventing a second authority. | ExpeditionVehicleSystem |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 27 | `src/Host/HostCli.PanelTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `Ashfall.Core.Tests/Campaign/Plans50_53_SharedIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 9 | `src/Host/ExpeditionHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 8 | `Ashfall.Core.Tests/Expeditions/VehicleGarageSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Ashfall.Core.Tests/ExpeditionVehicleLogisticsTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `src/Host/HostCli.VehicleGarage.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `src/Main.Plans50_53.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `src/UI/VehicleGaragePanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/ExpeditionVehicleSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Expeditions/BreakdownConsequenceTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Expeditions/Plan213VehicleArmorGradeTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/UI/ExpeditionPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/UI/ExpeditionRadarPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/UI/MapAtlasPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Expeditions/PlanE1_29VehicleEspionageTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/UI/ExpeditionCampPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Expeditions/Plan50VehicleGarageIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Expeditions/VehicleArmorGradesTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Integration/Plans46_49_CrossSystemIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/IslandBridgesTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Shelter/ShelterWorkshopTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Assets/Ashfall.Core/Shelter/ShelterWorkshopSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/Host/ContentUtilizationRuntimeCollector.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/Host/HostCli.ExpeditionPlaytest.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/Main.Expeditions.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/Main.VehicleGarage.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/UI/MapPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/UI/Plans130To133Panel.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/UI/WorkshopPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/ExpeditionEncounterBridgeTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/Expeditions/WeatherGateRadDoseTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/MicroLocationDeterminismHarness.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/Plan10CatalogCoverageTests.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/vehicles.json`

### `Assets/StreamingAssets/Data/vehicles.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 3044; characters: 3044.
- SHA-256: `cdec845708de1d2d3181767aeed9f4b4565a48596835415340ed557fd9b9ce82`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `vehicles`, `track_gear`

#### `vehicles` — 8 current rows

- Row 001 `vehicle_utility_quad`: `{"breakdown_threshold":0.2,"cargo_capacity":90,"condition_max":100,"default_attachments":[],"display_name":"Utility Quad","fuel_consumption_per_km":0.3,"max_fuel":40,"speed_multiplier":1.3,"terrain_type":"rough","vehicle_id":"vehicle_utili…`
- Row 002 `vehicle_dirt_bike`: `{"breakdown_threshold":0.25,"cargo_capacity":30,"condition_max":100,"default_attachments":[],"display_name":"Dirt Bike","fuel_consumption_per_km":0.2,"max_fuel":25,"speed_multiplier":1.8,"terrain_type":"rough","vehicle_id":"vehicle_dirt_bi…`
- Row 003 `vehicle_cargo_truck`: `{"breakdown_threshold":0.15,"cargo_capacity":250,"condition_max":100,"default_attachments":["winch_kit"],"display_name":"Cargo Truck","fuel_consumption_per_km":0.5,"max_fuel":80,"speed_multiplier":1.6,"terrain_type":"road","vehicle_id":"ve…`
- Row 004 `vehicle_steam_halftrack`: `{"breakdown_threshold":0.18,"cargo_capacity":180,"condition_max":100,"default_attachments":[],"display_name":"Steam Halftrack","fuel_consumption_per_km":0.7,"max_fuel":120,"speed_multiplier":0.85,"terrain_type":"rough","vehicle_id":"vehicl…`
- Row 005 `vehicle_armored_mobile_base`: `{"breakdown_threshold":0.15,"cargo_capacity":380,"condition_max":100,"default_attachments":[],"display_name":"Armored Mobile Base","fuel_consumption_per_km":0.95,"max_fuel":200,"speed_multiplier":0.7,"terrain_type":"road","vehicle_id":"veh…`
- Row 006 `vehicle_salvage_dredger`: `{"breakdown_threshold":0.2,"cargo_capacity":260,"condition_max":100,"default_attachments":[],"display_name":"Salvage Dredger","fuel_consumption_per_km":0.55,"max_fuel":95,"speed_multiplier":0.95,"terrain_type":"coastal","vehicle_id":"vehic…`
- Row 007 `vehicle_scout_motorcycle`: `{"breakdown_threshold":0.3,"cargo_capacity":18,"condition_max":100,"default_attachments":[],"display_name":"Scout Motorcycle","fuel_consumption_per_km":0.18,"max_fuel":18,"speed_multiplier":2.4,"terrain_type":"rough","vehicle_id":"vehicle_…`
- Row 008 `vehicle_ambulance_rig`: `{"breakdown_threshold":0.22,"cargo_capacity":140,"condition_max":100,"default_attachments":[],"display_name":"Ambulance Expedition Rig","fuel_consumption_per_km":0.45,"max_fuel":60,"speed_multiplier":1.25,"terrain_type":"road","vehicle_id"…`

#### `track_gear` — 1 current rows

- Row 001 `vehicle_track_gear_standard`: `{"breakdown_risk_multiplier":0.82,"display_name":"Standard Track Gear","gear_id":"vehicle_track_gear_standard","traction_multiplier":1.18}`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/vehicle_armor_grades.json`

### `Assets/StreamingAssets/Data/vehicle_armor_grades.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 4568; characters: 4568.
- SHA-256: `8e62dc6b78f7e97b107078bd3802577edfdba0b399abc8e70d1479dd76d03faa`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `default_grade_id`, `grades`

#### `grades` — 5 current rows

- Row 001 `grade_0_stock`: `{"compatible_terrain_types":["road","rough","coastal"],"description":"Factory sheet metal and hope. Every vehicle leaves the bay with this; it stops weather, not consequences.","display_name":"Stock Plating","fuel_consumption_multiplier":1…`
- Row 002 `grade_1_scrap_plate`: `{"compatible_terrain_types":["road","rough","coastal"],"description":"Door skins and road sign bolted over the soft spots. It rattles, it drags a little, and once in a while it eats the hit that would have killed the axle.","display_name":…`
- Row 003 `grade_2_sheet_plate`: `{"compatible_terrain_types":["road","rough","coastal"],"description":"Cut sheet, welded seams, a real work order in the log. The crew stops flinching at every stone strike.","display_name":"Sheet Plate","fuel_consumption_multiplier":1.05,"…`
- Row 004 `grade_3_composite_plate`: `{"compatible_terrain_types":["road","rough"],"description":"Layered stock from the foundry's good days, ceramic face over iron back. Heavy enough to need its own line in the fuel ledger.","display_name":"Composite Plate","fuel_consumption_…`
- Row 005 `grade_4_alloyed_heavy_plate`: `{"compatible_terrain_types":["road","rough"],"description":"The heaviest set the bay can hang on a frame. Slow, thirsty, and the closest thing this world has to a promise.","display_name":"Alloyed Heavy Plate","fuel_consumption_multiplier"…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/vehicle_modifications.json`

### `Assets/StreamingAssets/Data/vehicle_modifications.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 4882; characters: 4882.
- SHA-256: `f1c6e40d1d97f17af70faed4b7e09ac201a0c64e540d61b33087fb2c88373001`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `modifications`

#### `modifications` — 8 current rows

- Row 001 `vmod_expanded_flatbed`: `{"compatible_vehicle_tags":["road","rough","hauler","truck"],"effects":{"cargo_capacity_delta":60,"fuel_consumption_multiplier":1.05,"radiation_protection_permille":0,"speed_multiplier_delta":-0.05,"wear_rate_multiplier":1.1},"id":"vmod_ex…`
- Row 002 `vmod_lead_lined_cab`: `{"compatible_vehicle_tags":["road","rough","hauler","truck"],"effects":{"cargo_capacity_delta":-20,"fuel_consumption_multiplier":1.1,"radiation_protection_permille":400,"speed_multiplier_delta":-0.1,"wear_rate_multiplier":1.05},"id":"vmod_…`
- Row 003 `vmod_reinforced_bullbar`: `{"compatible_vehicle_tags":["quad","bike","road","rough","truck","hauler"],"effects":{"cargo_capacity_delta":0,"fuel_consumption_multiplier":1.0,"radiation_protection_permille":50,"speed_multiplier_delta":-0.02,"wear_rate_multiplier":0.85}…`
- Row 004 `vmod_aux_fuel_rack`: `{"compatible_vehicle_tags":["quad","bike","road","rough","hauler","truck"],"effects":{"cargo_capacity_delta":-10,"fuel_consumption_multiplier":0.95,"radiation_protection_permille":0,"speed_multiplier_delta":0.0,"wear_rate_multiplier":1.0},…`
- Row 005 `vmod_traction_cleats`: `{"compatible_vehicle_tags":["rough","quad","hauler","truck"],"effects":{"cargo_capacity_delta":0,"fuel_consumption_multiplier":1.08,"radiation_protection_permille":0,"speed_multiplier_delta":0.15,"wear_rate_multiplier":0.9},"id":"vmod_trac…`
- Row 006 `vmod_stretcher_mount`: `{"compatible_vehicle_tags":["road","truck","hauler"],"effects":{"cargo_capacity_delta":-15,"fuel_consumption_multiplier":1.0,"radiation_protection_permille":100,"speed_multiplier_delta":0.0,"wear_rate_multiplier":1.0},"id":"vmod_stretcher_…`
- Row 007 `vmod_heavy_winch`: `{"compatible_vehicle_tags":["road","rough","truck","hauler"],"effects":{"cargo_capacity_delta":-10,"fuel_consumption_multiplier":1.02,"radiation_protection_permille":0,"speed_multiplier_delta":-0.02,"wear_rate_multiplier":0.9},"id":"vmod_h…`
- Row 008 `vmod_engine_supercharger`: `{"compatible_vehicle_tags":["road","bike","quad","truck"],"effects":{"cargo_capacity_delta":0,"fuel_consumption_multiplier":1.2,"radiation_protection_permille":0,"speed_multiplier_delta":0.35,"wear_rate_multiplier":1.25},"id":"vmod_engine_…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/vehicle_modules.json`

### `Assets/StreamingAssets/Data/vehicle_modules.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 8238; characters: 8238.
- SHA-256: `832474455de424b275dc335fd40ca8263dd55ca4f6b208dc46072baaec039319`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `modules`

#### `modules` — 20 current rows

- Row 001 `reinforced_hull`: `{"bunk_capacity":0,"cargo_bonus":0.0,"components_cost":30,"defense_bonus":20.0,"description":"Plated sheet welded over the cab and doors. Heavy, and it shows in the fuel ledger.","installation_days":3,"module_id":"reinforced_hull","module_…`
- Row 002 `bulletproof_glass`: `{"bunk_capacity":0,"cargo_bonus":0.0,"components_cost":25,"defense_bonus":15.0,"description":"Shatter layers cut to the cab frame. You can still see out, which is the entire point.","installation_days":2,"module_id":"bulletproof_glass","mo…`
- Row 003 `spike_strips`: `{"bunk_capacity":0,"cargo_bonus":0.0,"components_cost":10,"defense_bonus":8.0,"description":"Angle iron run along the sills. Anything that grabs the vehicle hurts itself doing it.","installation_days":1,"module_id":"spike_strips","module_t…`
- Row 004 `underbody_plate`: `{"bunk_capacity":0,"cargo_bonus":0.0,"components_cost":15,"defense_bonus":12.0,"description":"A skid plate under the pan. Meant for debris, often it is mines.","installation_days":2,"module_id":"underbody_plate","module_type":"armor","name…`
- Row 005 `extended_cargo_bed`: `{"bunk_capacity":0,"cargo_bonus":50.0,"components_cost":15,"defense_bonus":0.0,"description":"The bed runs back further than the frame was built for. It sags when it is full.","installation_days":2,"module_id":"extended_cargo_bed","module_…`
- Row 006 `roof_rack`: `{"bunk_capacity":0,"cargo_bonus":25.0,"components_cost":10,"defense_bonus":0.0,"description":"Bolt-on rails over the cab. Cheap capacity, paid for in drag and in dust.","installation_days":1,"module_id":"roof_rack","module_type":"cargo","n…`
- Row 007 `trailer`: `{"bunk_capacity":0,"cargo_bonus":100.0,"components_cost":20,"defense_bonus":0.0,"description":"Everything the vehicle cannot carry, dragged behind it. Turning becomes a decision.","installation_days":4,"module_id":"trailer","module_type":"…`
- Row 008 `insulated_hold`: `{"bunk_capacity":0,"cargo_bonus":15.0,"components_cost":25,"defense_bonus":0.0,"description":"A lined compartment that keeps a load out of the heat. It does not run itself.","installation_days":3,"module_id":"insulated_hold","module_type":…`
- Row 009 `bunk_beds_module`: `{"bunk_capacity":4,"cargo_bonus":0.0,"components_cost":20,"defense_bonus":0.0,"description":"Two doubles stacked into the back. It is not a room. It is enough to sleep.","installation_days":2,"module_id":"bunk_beds_module","module_type":"l…`
- Row 010 `mobile_medbay`: `{"bunk_capacity":1,"cargo_bonus":0.0,"components_cost":35,"defense_bonus":0.0,"description":"A stretcher bracket, a lamp, a triage bed, and a locked case. The case is the reason for the rest.","installation_days":3,"module_id":"mobile_medb…`
- Row 011 `galley_stove`: `{"bunk_capacity":0,"cargo_bonus":0.0,"components_cost":25,"defense_bonus":0.0,"description":"A bolted stove and a work top. Something warm, somewhere that is not the shelter.","installation_days":2,"module_id":"galley_stove","module_type":…`
- Row 012 `water_reservoir`: `{"bunk_capacity":0,"cargo_bonus":10.0,"components_cost":20,"defense_bonus":0.0,"description":"A baffled tank under the bed. The weight sits low, which helps, and slows.","installation_days":3,"module_id":"water_reservoir","module_type":"li…`
- Row 013 `mounted_gun`: `{"bunk_capacity":0,"cargo_bonus":0.0,"components_cost":40,"defense_bonus":10.0,"description":"A pintle mount over the cab. It answers a roadblock, and it invites one.","installation_days":4,"module_id":"mounted_gun","module_type":"weapon",…`
- Row 014 `ram_plate`: `{"bunk_capacity":0,"cargo_bonus":0.0,"components_cost":15,"defense_bonus":6.0,"description":"A wedge on the front frame. Designed to end a conversation about who goes first.","installation_days":2,"module_id":"ram_plate","module_type":"wea…`
- Row 015 `smoke_launcher`: `{"bunk_capacity":0,"cargo_bonus":0.0,"components_cost":20,"defense_bonus":5.0,"description":"Tubes along the rail. Whatever the situation is, the answer is not being seen in it.","installation_days":2,"module_id":"smoke_launcher","module_t…`
- Row 016 `flare_cannon`: `{"bunk_capacity":0,"cargo_bonus":0.0,"components_cost":15,"defense_bonus":3.0,"description":"Short-range launcher for signal and for fire. Mostly for signal.","installation_days":1,"module_id":"flare_cannon","module_type":"weapon","name":"…`
- Row 017 `heavy_winch`: `{"bunk_capacity":0,"cargo_bonus":0.0,"components_cost":20,"defense_bonus":3.0,"description":"Cable, drum, and a hook. The most used module on any vehicle that leaves the road.","installation_days":2,"module_id":"heavy_winch","module_type":…`
- Row 018 `light_crane`: `{"bunk_capacity":0,"cargo_bonus":20.0,"components_cost":30,"defense_bonus":0.0,"description":"A folding jib for lifting what two people should not try to lift. It changes what a stop is for.","installation_days":3,"module_id":"light_crane"…`
- Row 019 `solar_panels`: `{"bunk_capacity":0,"cargo_bonus":0.0,"components_cost":35,"defense_bonus":1.0,"description":"Panels over the bed charging a separate bank. On a grey day it is a decoration.","installation_days":3,"module_id":"solar_panels","module_type":"u…`
- Row 020 `field_radio`: `{"bunk_capacity":0,"cargo_bonus":0.0,"components_cost":25,"defense_bonus":0.0,"description":"A set and an antenna on the rail. Being able to call is worth more than being able to carry.","installation_days":1,"module_id":"field_radio","mod…`


# Appendix — Current Source Detail: `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs`

### `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` — complete current file

- Size: 464 lines / 20023 bytes.
- SHA-256: `c897d11e111c3ff4e559d01a4be0241b2e291de005cd4fce076090f3ac450eb2`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core.Expeditions;
00005: #pragma warning disable CS8618
00006:
00007: namespace Ashfall.Core
00008: {
00009:     /// <summary>CORE-MECH W4 — what a vehicle breakdown costs the crew.</summary>
00010:     public enum VehicleBreakdownKind
00011:     {
00012:         None = 0,
00013:         Injury = 1,
00014:         RadiationExposure = 2,
00015:         Contamination = 3
00016:     }
00017:
00018:     /// <summary>
00019:     /// CORE-MECH W4 — typed breakdown consequence. Pure data: the owner computes
00020:     /// the band and raises it; the host routes it into the medical, dose, and
00021:     /// disease owners (AA.2/AA.4 receiver contracts). Nothing here is persisted
00022:     /// beyond the vehicle's own isBrokenDown / breakdownCause state.
00023:     /// </summary>
00024:     public sealed class VehicleBreakdownOutcome
00025:     {
00026:         /// <summary>True when the vehicle actually broke (independent of crew band).</summary>
00027:         public bool BrokeDown { get; }
00028:
00029:         public VehicleBreakdownKind Kind { get; }
00030:         public float Severity01 { get; }
00031:         public float NominalMsV { get; }
00032:         public string Cause { get; }
00033:         public string VehicleId { get; }
00034:
00035:         /// <summary>
00036:         /// CORE-MECH W4 — survivor the crew consequence applies to; filled by the
00037:         /// host at dispatch (empty when the caller supplied no survivor).
00038:         /// </summary>
00039:         public string SurvivorId { get; set; } = string.Empty;
00040:
00041:         public VehicleBreakdownOutcome(
00042:             VehicleBreakdownKind kind, float severity01, float nominalMsV, string cause, string vehicleId,
00043:             bool brokeDown = false)
00044:         {
00045:             Kind = kind;
00046:             Severity01 = severity01;
00047:             NominalMsV = nominalMsV;
00048:             Cause = cause ?? string.Empty;
00049:             VehicleId = vehicleId ?? string.Empty;
00050:             BrokeDown = brokeDown;
00051:         }
00052:
00053:         /// <summary>No breakdown occurred (unknown vehicle, already broken, clean dispatch).</summary>
00054:         public static VehicleBreakdownOutcome None(string vehicleId, string reason)
00055:             => new VehicleBreakdownOutcome(VehicleBreakdownKind.None, 0f, 0f, reason, vehicleId);
00056:     }
00057:
00058:     [Serializable]
00059:     public sealed class ExpeditionVehicleState
00060:     {
00061:         public string systemId = ExpeditionVehicleSystem.SystemId;
00062:         public Dictionary<string, VehicleInstance> ownedVehicles = new Dictionary<string, VehicleInstance>();
00063:         public string activeExpeditionVehicleId = string.Empty;
00064:     }
00065:
00066:     [Serializable]
00067:     public sealed class VehicleInstance
00068:     {
00069:         public string vehicleId = string.Empty;
00070:         public string displayName = string.Empty;
00071:         public float condition = 100f;
00072:         public float fuel;
00073:         public float maxFuel = 50f;
00074:         public float cargoCapacity = 100f;
00075:         public float speedMultiplier = 1f;
00076:         public string terrainType = "road";
00077:         public bool isBrokenDown;
00078:         public string breakdownCause = string.Empty;
00079:         public List<string> attachments = new List<string>();
00080:         public VehicleTrackGearState trackGear = new VehicleTrackGearState();
00081:     }
00082:
00083:     /// <summary>
00084:     /// Persisted, normalized track-gear facts for one vehicle. Track gear is
00085:     /// an attachment effect, not a second vehicle or terrain simulator.
00086:     /// </summary>
00087:     [Serializable]
00088:     public sealed class VehicleTrackGearState
00089:     {
00090:         public string gearId = string.Empty;
00091:         public float condition = 100f;
00092:         public float tractionMultiplier = 1f;
00093:         public float breakdownRiskMultiplier = 1f;
00094:
00095:         public bool IsInstalled => !string.IsNullOrEmpty(gearId);
00096:
00097:         public float EffectiveTractionMultiplier()
00098:         {
00099:             float condition01 = Math.Clamp(condition / 100f, 0f, 1f);
00100:             float configured = Math.Clamp(tractionMultiplier, 0.5f, 2f);
00101:             return 1f + (configured - 1f) * condition01;
00102:         }
00103:
00104:         public float EffectiveBreakdownRiskMultiplier()
00105:         {
00106:             float condition01 = Math.Clamp(condition / 100f, 0f, 1f);
00107:             float configured = Math.Clamp(breakdownRiskMultiplier, 0f, 1f);
00108:             return 1f + (configured - 1f) * condition01;
00109:         }
00110:     }
00111:
00112:     [Serializable]
00113:     public sealed class VehicleDefinition
00114:     {
00115:         public string vehicle_id = string.Empty;
00116:         public string display_name = string.Empty;
00117:         public float max_fuel = 50f;
00118:         public float cargo_capacity = 100f;
00119:         public float speed_multiplier = 1f;
00120:         public string terrain_type = "road";
00121:         public float condition_max = 100f;
00122:         public float fuel_consumption_per_km = 0.5f;
00123:         public float breakdown_threshold = 0.2f;
00124:         public List<string> default_attachments = new List<string>();
00125:     }
00126:
00127:     [Serializable]
00128:     public sealed class VehicleTrackGearDefinition
00129:     {
00130:         public string gear_id = string.Empty;
00131:         public string display_name = string.Empty;
00132:         public float traction_multiplier = 1.1f;
00133:         public float breakdown_risk_multiplier = 0.9f;
00134:     }
00135:
00136:     [Serializable]
00137:     public sealed class VehicleCatalog
00138:     {
00139:         public int schema_version = 1;
00140:         public List<VehicleDefinition> vehicles = new List<VehicleDefinition>();
00141:         public List<VehicleTrackGearDefinition> track_gear = new List<VehicleTrackGearDefinition>();
00142:     }
00143:
00144:     public sealed class ExpeditionVehicleSystem
00145:     {
00146:         public const string SystemId = "expedition_vehicle";
00147:         private ExpeditionVehicleState _state = new ExpeditionVehicleState();
00148:         private readonly Dictionary<string, VehicleDefinition> _catalog = new Dictionary<string, VehicleDefinition>(StringComparer.Ordinal);
00149:         private readonly Dictionary<string, VehicleTrackGearDefinition> _trackGearCatalog = new Dictionary<string, VehicleTrackGearDefinition>(StringComparer.Ordinal);
00150:         private readonly ISeededRng _rng;
00151:         private readonly ILog _log;
00152:
00153:         public ExpeditionVehicleState State => _state;
00154:         public event Action OnVehicleStateChanged;
00155:
00156:         public ExpeditionVehicleSystem(ISeededRng rng, ILog? log = null)
00157:         {
00158:             _rng = rng ?? throw new ArgumentNullException(nameof(rng));
00159:             _log = log ?? NullLog.Instance;
00160:         }
00161:
00162:         public void LoadCatalog(VehicleCatalog catalog)
00163:         {
00164:             if (catalog?.vehicles == null) return;
00165:             _catalog.Clear();
00166:             _trackGearCatalog.Clear();
00167:             foreach (var v in catalog.vehicles)
00168:                 if (!string.IsNullOrEmpty(v.vehicle_id))
00169:                     _catalog[v.vehicle_id] = v;
00170:             foreach (var gear in catalog.track_gear)
00171:                 if (!string.IsNullOrEmpty(gear.gear_id))
00172:                     _trackGearCatalog[gear.gear_id] = gear;
00173:         }
00174:
00175:         public VehicleDefinition? GetDefinition(string id)
00176:         {
00177:             _catalog.TryGetValue(id, out var def);
00178:             return def;
00179:         }
00180:
00181:         public VehicleTrackGearDefinition? GetTrackGearDefinition(string id)
00182:         {
00183:             _trackGearCatalog.TryGetValue(id, out var def);
00184:             return def;
00185:         }
00186:
00187:         public ActionResult AcquireVehicle(string vehicleId)
00188:         {
00189:             if (_state.ownedVehicles.ContainsKey(vehicleId))
00190:                 return ActionResult.Blocked("already_owned", "vehicle.already_owned");
00191:             if (!_catalog.TryGetValue(vehicleId, out var def))
00192:                 return ActionResult.Failed("unknown_vehicle", "vehicle.unknown");
00193:
00194:             _state.ownedVehicles[vehicleId] = new VehicleInstance
00195:             {
00196:                 vehicleId = vehicleId,
00197:                 displayName = def.display_name,
00198:                 condition = def.condition_max,
00199:                 fuel = def.max_fuel * 0.5f,
00200:                 maxFuel = def.max_fuel,
00201:                 cargoCapacity = def.cargo_capacity,
00202:                 speedMultiplier = def.speed_multiplier,
00203:                 terrainType = def.terrain_type,
00204:                 attachments = new List<string>(def.default_attachments)
00205:             };
00206:             OnVehicleStateChanged?.Invoke();
00207:             return ActionResult.Success("vehicle.acquired",
00208:                 new Dictionary<string, double> { { "fuel", def.max_fuel * 0.5f } });
00209:         }
00210:
00211:         public VehicleInstance? GetVehicle(string vehicleId)
00212:         {
00213:             _state.ownedVehicles.TryGetValue(vehicleId, out var v);
00214:             return v;
00215:         }
00216:
00217:         public ActionResult Refuel(string vehicleId, float amount)
00218:         {
00219:             if (!_state.ownedVehicles.TryGetValue(vehicleId, out var v))
00220:                 return ActionResult.Failed("unknown_vehicle", "vehicle.unknown");
00221:             float added = Math.Min(amount, v.maxFuel - v.fuel);
00222:             v.fuel += added;
00223:             OnVehicleStateChanged?.Invoke();
00224:             return ActionResult.Success("vehicle.refueled",
00225:                 new Dictionary<string, double> { { "fuel_added", added }, { "fuel_total", v.fuel } });
00226:         }
00227:
00228:         public ActionResult Repair(string vehicleId, float amount)
00229:         {
00230:             if (!_state.ownedVehicles.TryGetValue(vehicleId, out var v))
00231:                 return ActionResult.Failed("unknown_vehicle", "vehicle.unknown");
00232:             v.condition = Math.Min(100f, v.condition + amount);
00233:             v.isBrokenDown = false;
00234:             v.breakdownCause = string.Empty;
00235:             OnVehicleStateChanged?.Invoke();
00236:             return ActionResult.Success("vehicle.repaired",
00237:                 new Dictionary<string, double> { { "condition", v.condition } });
00238:         }
00239:
00240:         public ActionResult AttachEquipment(string vehicleId, string equipmentId)
00241:         {
00242:             if (!_state.ownedVehicles.TryGetValue(vehicleId, out var v))
00243:                 return ActionResult.Failed("unknown_vehicle", "vehicle.unknown");
00244:             if (v.attachments.Contains(equipmentId))
00245:                 return ActionResult.Blocked("already_attached", "vehicle.already_attached");
00246:             v.attachments.Add(equipmentId);
00247:             OnVehicleStateChanged?.Invoke();
00248:             return ActionResult.Success("vehicle.attached",
00249:                 new Dictionary<string, double> { { "attachments", v.attachments.Count } });
00250:         }
00251:
00252:         /// <summary>
00253:         /// Install normalized track gear on a vehicle. The host supplies the
00254:         /// authored gear facts; this authority owns the resulting condition
00255:         /// and projects it into expedition mobility.
00256:         /// </summary>
00257:         public ActionResult InstallTrackGear(
00258:             string vehicleId,
00259:             string gearId,
00260:             float tractionMultiplier,
00261:             float breakdownRiskMultiplier,
00262:             float condition = 100f)
00263:         {
00264:             if (!_state.ownedVehicles.TryGetValue(vehicleId, out var v))
00265:                 return ActionResult.Failed("unknown_vehicle", "vehicle.unknown");
00266:             if (string.IsNullOrEmpty(gearId))
00267:                 return ActionResult.Blocked("invalid_track_gear", "vehicle.invalid_track_gear");
00268:             if (tractionMultiplier < 0.5f || tractionMultiplier > 2f
00269:                 || breakdownRiskMultiplier < 0f || breakdownRiskMultiplier > 1f)
00270:                 return ActionResult.Blocked("invalid_track_gear", "vehicle.invalid_track_gear");
00271:
00272:             v.trackGear = new VehicleTrackGearState
00273:             {
00274:                 gearId = gearId,
00275:                 condition = Math.Clamp(condition, 0f, 100f),
00276:                 tractionMultiplier = tractionMultiplier,
00277:                 breakdownRiskMultiplier = breakdownRiskMultiplier
00278:             };
00279:             OnVehicleStateChanged?.Invoke();
00280:             return ActionResult.Success("vehicle.track_gear_installed",
00281:                 new Dictionary<string, double>
00282:                 {
00283:                     { "traction_multiplier", v.trackGear.EffectiveTractionMultiplier() },
00284:                     { "breakdown_risk_multiplier", v.trackGear.EffectiveBreakdownRiskMultiplier() }
00285:                 });
00286:         }
00287:
00288:         public ActionResult InstallTrackGear(string vehicleId, string gearId, float condition = 100f)
00289:         {
00290:             if (!_trackGearCatalog.TryGetValue(gearId, out var def))
00291:                 return ActionResult.Failed("unknown_track_gear", "vehicle.unknown_track_gear");
00292:             return InstallTrackGear(
00293:                 vehicleId,
00294:                 gearId,
00295:                 def.traction_multiplier,
00296:                 def.breakdown_risk_multiplier,
00297:                 condition);
00298:         }
00299:
00300:         public ActionResult RemoveTrackGear(string vehicleId)
00301:         {
00302:             if (!_state.ownedVehicles.TryGetValue(vehicleId, out var v))
00303:                 return ActionResult.Failed("unknown_vehicle", "vehicle.unknown");
00304:             if (!v.trackGear.IsInstalled)
00305:                 return ActionResult.Blocked("no_track_gear", "vehicle.no_track_gear");
00306:
00307:             v.trackGear = new VehicleTrackGearState();
00308:             OnVehicleStateChanged?.Invoke();
00309:             return ActionResult.Success("vehicle.track_gear_removed");
00310:         }
00311:
00312:         public ActionResult RepairTrackGear(string vehicleId, float amount)
00313:         {
00314:             if (!_state.ownedVehicles.TryGetValue(vehicleId, out var v))
00315:                 return ActionResult.Failed("unknown_vehicle", "vehicle.unknown");
00316:             if (!v.trackGear.IsInstalled)
00317:                 return ActionResult.Blocked("no_track_gear", "vehicle.no_track_gear");
00318:
00319:             v.trackGear.condition = Math.Clamp(v.trackGear.condition + Math.Max(0f, amount), 0f, 100f);
00320:             OnVehicleStateChanged?.Invoke();
00321:             return ActionResult.Success("vehicle.track_gear_repaired",
00322:                 new Dictionary<string, double> { { "condition", v.trackGear.condition } });
00323:         }
00324:
00325:         /// <summary>
00326:         /// Project the garage facts into the existing expedition profile.
00327:         /// This keeps vehicle preparation and travel as separate authorities.
00328:         /// </summary>
00329:         public ExpeditionVehicleProfile? CreateExpeditionProfile(
00330:             string vehicleId,
00331:             float kmPerTravelTick = 2.5f)
00332:         {
00333:             if (!_state.ownedVehicles.TryGetValue(vehicleId, out var v))
00334:                 return null;
00335:
00336:             var def = _catalog.TryGetValue(vehicleId, out var catalogDef) ? catalogDef : null;
00337:             float consumption = def?.fuel_consumption_per_km ?? 0.5f;
00338:             var gear = v.trackGear ?? new VehicleTrackGearState();
00339:             return new ExpeditionVehicleProfile
00340:             {
00341:                 vehicleId = vehicleId,
00342:                 speedMultiplier = Math.Max(0.01f, v.speedMultiplier * gear.EffectiveTractionMultiplier()),
00343:                 cargoCapacityKg = v.cargoCapacity,
00344:                 fuelPerTravelTick = Math.Max(0f, consumption * Math.Max(0f, kmPerTravelTick)),
00345:                 breakdownChancePerTick = Math.Clamp(
00346:                     (100f - v.condition) / 100f * 0.15f * gear.EffectiveBreakdownRiskMultiplier(),
00347:                     0f,
00348:                     1f)
00349:             };
00350:         }
00351:
00352:         public (float fuelCost, float travelTimeMod, bool breakdown) PrepareForExpedition(string vehicleId, float distanceKm)
00353:         {
00354:             return PrepareForExpeditionCore(vehicleId, distanceKm, null);
00355:         }
00356:
00357:         /// <summary>
00358:         /// CORE-MECH W4 — the consuming preparation travel, with an optional rng for
00359:         /// the breakdown roll. Null keeps the legacy path (the system's own stream)
00360:         /// byte-for-byte; the outcome-aware caller passes a seed so the whole
00361:         /// resolution (breakdown + crew band) is one deterministic draw sequence.
00362:         /// </summary>
00363:         private (float fuelCost, float travelTimeMod, bool breakdown) PrepareForExpeditionCore(
00364:             string vehicleId, float distanceKm, ISeededRng? rng)
00365:         {
00366:
00367:             if (!_state.ownedVehicles.TryGetValue(vehicleId, out var v))
00368:                 return (0, 1f, false);
00369:
00370:             float fuelNeeded = distanceKm * 0.5f; // 0.5 fuel per km
00371:             if (_catalog.TryGetValue(vehicleId, out var def))
00372:                 fuelNeeded = distanceKm * def.fuel_consumption_per_km;
00373:
00374:             if (v.fuel < fuelNeeded) return (fuelNeeded, 1f, false);
00375:
00376:             v.fuel -= fuelNeeded;
00377:             float wear = distanceKm * 0.5f;
00378:             v.condition = Math.Max(0, v.condition - wear);
00379:             if (v.trackGear != null && v.trackGear.IsInstalled)
00380:                 v.trackGear.condition = Math.Max(0f, v.trackGear.condition - distanceKm * 0.25f);
00381:
00382:             bool breakdown = false;
00383:             if (v.condition < 20f && (rng ?? _rng).NextDouble() < 0.3f)
00384:             {
00385:                 breakdown = true;
00386:                 v.isBrokenDown = true;
00387:                 v.breakdownCause = $"vehicle broke down at {distanceKm}km (condition={v.condition:F1})";
00388:             }
00389:
00390:             OnVehicleStateChanged?.Invoke();
00391:             return (fuelNeeded, v.speedMultiplier, breakdown);
00392:         }
00393:
00394:         /// <summary>
00395:         /// CORE-MECH W4 — a prep breakdown now names what it costs the crew.
00396:         /// Raised once per breakdown resolution; not raised for clean dispatches
00397:         /// or when the vehicle is already broken (the repair gate is the
00398:         /// exactly-once guard, AC.3).
00399:         /// </summary>
00400:         public event Action<VehicleBreakdownOutcome>? OnBreakdownResolved;
00401:
00402:         /// <summary>
00403:         /// CORE-MECH W4 — consume the same preparation travel as
00404:         /// <see cref="PrepareForExpedition"/> and, when the vehicle breaks down,
00405:         /// resolve a typed crew consequence (injury / exposure / contamination).
00406:         /// Deterministic: the band is a pure function of the vehicle's condition
00407:         /// and the injected rng. The existing tuple API is untouched; this is the
00408:         /// consequence-aware call the host uses.
00409:         /// </summary>
00410:         public VehicleBreakdownOutcome ResolvePrepBreakdown(string vehicleId, float distanceKm, ISeededRng? rng = null)
00411:         {
00412:             if (!_state.ownedVehicles.TryGetValue(vehicleId, out var v))
00413:                 return VehicleBreakdownOutcome.None(vehicleId, "unknown_vehicle");
00414:
00415:             if (v.isBrokenDown)
00416:                 return VehicleBreakdownOutcome.None(vehicleId, "already_broken");
00417:
00418:             var (_, _, broke) = PrepareForExpeditionCore(vehicleId, distanceKm, rng);
00419:             if (!broke)
00420:                 return VehicleBreakdownOutcome.None(vehicleId, "clean_dispatch");
00421:
00422:             // Severity rises as the vehicle is closer to failure; the band is a
00423:             // pure function so the same seed always yields the same outcome.
00424:             float severity = Math.Clamp(1f - v.condition / 100f, 0.05f, 1f);
00425:             double roll = (rng ?? _rng).NextDouble();
00426:
00427:             VehicleBreakdownKind kind;
00428:             if (roll < 0.55) kind = VehicleBreakdownKind.None;
00429:             else if (roll < 0.80) kind = VehicleBreakdownKind.Injury;
00430:             else if (roll < 0.92) kind = VehicleBreakdownKind.RadiationExposure;
00431:             else kind = VehicleBreakdownKind.Contamination;
00432:
00433:             // Nominal exposure scales with severity and stays inside the ledger's
00434:             // own band discipline; magnitude is authored data pending W12 (BE.2).
00435:             float nominalMsV = kind == VehicleBreakdownKind.RadiationExposure
00436:                 ? 0.15f + severity * 0.85f
00437:                 : 0f;
00438:
00439:             var outcome = new VehicleBreakdownOutcome(
00440:                 kind, severity, nominalMsV,
00441:                 v.breakdownCause ?? string.Empty, vehicleId, brokeDown: true);
00442:
00443:             OnBreakdownResolved?.Invoke(outcome);
00444:             OnVehicleStateChanged?.Invoke();
00445:             return outcome;
00446:         }
00447:
00448:         public ExpeditionVehicleState CaptureState() => CloneState(_state);
00449:
00450:         public void RestoreState(ExpeditionVehicleState saved)
00451:         {
00452:             if (saved == null) return;
00453:             _state = CloneState(saved);
00454:         }
00455:
00456:         private static ExpeditionVehicleState CloneState(ExpeditionVehicleState src)
00457:         {
00458:             if (src == null) return new ExpeditionVehicleState();
00459:             var s = new SystemTextJsonSerializer();
00460:             var json = s.Serialize(src);
00461:             return s.Deserialize<ExpeditionVehicleState>(json) ?? new ExpeditionVehicleState();
00462:         }
00463:     }
00464: }
```


# Appendix — Current Source Detail: `src/UI/VehicleGaragePanel.cs`

### `src/UI/VehicleGaragePanel.cs` — complete current file

- Size: 635 lines / 29929 bytes.
- SHA-256: `0f7ed9ba942430e7bf5b35fc289bce28176d35d8534876302d9c86223a425b4f`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Godot;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Expeditions;
00007: using Ashfall.Core.Inventory;
00008: using Ashfall.Core.UI;
00009: using DesignTheme = Ashfall.Core.UI.Theme;
00010:
00011: namespace AtomicWar.GodotApp.UI
00012: {
00013:     /// <summary>
00014:     /// Plan 50 Phase 3 — expedition overland vehicle customization &
00015:     /// maintenance garage.
00016:     ///
00017:     /// Presentation only: renders the Core garage read model (fitted
00018:     /// modifications, chassis/engine/transmission wear, immobilization, active
00019:     /// recovery missions) and forwards player commands to
00020:     /// <see cref="VehicleGarageSystem"/> (install / uninstall / service /
00021:     /// complete recovery). Inventory consumption and effect math stay in Core.
00022:     /// The garage's fitted-modification effects now decorate the expedition
00023:     /// profile, so the displayed deltas are the ones the sortie actually uses.
00024:     /// </summary>
00025:     public partial class VehicleGaragePanel : Control, IBindablePanel
00026:     {
00027:         public event Action? OnClose;
00028:
00029:         private AshfallDashboardShell _shell = null!;
00030:         private AshfallStatusRail? _statusRail;
00031:         private VBoxContainer _contentStack = null!;
00032:         private Label _detailText = null!;
00033:         private Label _vehicleText = null!;
00034:         private Label _modText = null!;
00035:         private Label _armorText = null!;
00036:         private Label _armorCostText = null!;
00037:         private Label _maintenanceText = null!;
00038:         private Label _recoveryText = null!;
00039:         private Label _commandResult = null!;
00040:
00041:         private OptionButton _vehicleSelector = null!;
00042:         private OptionButton _modSelector = null!;
00043:         private OptionButton _armorSelector = null!;
00044:
00045:         private Button _installButton = null!;
00046:         private Button _uninstallButton = null!;
00047:         private Button _armorInstallButton = null!;
00048:         private Button _armorReforgeButton = null!;
00049:         private Button _serviceChassisButton = null!;
00050:         private Button _serviceEngineButton = null!;
00051:         private Button _serviceTransmissionButton = null!;
00052:         private Button _completeRecoveryButton = null!;
00053:
00054:         private Label _modCostText = null!;
00055:
00056:         private VehicleGarageSystem? _garage;
00057:         private ExpeditionVehicleSystem? _vehicles;
00058:         private Inventory? _inventory;
00059:
00060:         private string _selectedVehicleId = string.Empty;
00061:         private string _selectedModId = string.Empty;
00062:         private string _selectedArmorGradeId = string.Empty;
00063:         private string _selectedRecoveryId = string.Empty;
00064:         private bool _syncing;
00065:
00066:         public bool IsBound => _garage != null;
00067:
00068:         public string LastFeedback { get; private set; } = string.Empty;
00069:
00070:         public void Bind(VehicleGarageSystem garage, ExpeditionVehicleSystem vehicles, Inventory inventory)
00071:         {
00072:             _garage = garage;
00073:             _vehicles = vehicles;
00074:             _inventory = inventory;
00075:             RefreshView();
00076:         }
00077:
00078:         public void Unbind()
00079:         {
00080:             _garage = null;
00081:             _vehicles = null;
00082:             _inventory = null;
00083:         }
00084:
00085:         public override void _Ready()
00086:         {
00087:             SetAnchorsPreset(LayoutPreset.FullRect);
00088:
00089:             _shell = new AshfallDashboardShell("VEHICLE GARAGE // OVERLAND MAINTENANCE", minWidth: 1180, minHeight: 700);
00090:             AddChild(_shell);
00091:
00092:             _statusRail = _shell.SetStatusRail();
00093:             _statusRail.AddCard("vehicles", "Vehicles", "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
00094:             _statusRail.AddCard("immobilized", "Immobilized", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
00095:             _statusRail.AddCard("recoveries", "Recoveries", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
00096:             _statusRail.AddCard("fitted", "Fitted Mods", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
00097:
00098:             var scroll = new ScrollContainer
00099:             {
00100:                 SizeFlagsVertical = SizeFlags.ExpandFill,
00101:                 SizeFlagsHorizontal = SizeFlags.ExpandFill
00102:             };
00103:             _contentStack = new VBoxContainer();
00104:             _contentStack.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
00105:             _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00106:             _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;
00107:             scroll.AddChild(_contentStack);
00108:
00109:             _contentStack.AddChild(_detailText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });
00110:
00111:             _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
00112:             _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("VEHICLE"));
00113:
00114:             var vehicleRow = new HBoxContainer();
00115:             vehicleRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
00116:             vehicleRow.AddChild(AshfallUiHelpers.MakeBody("Select:"));
00117:             _vehicleSelector = new OptionButton { CustomMinimumSize = new Vector2(320, 34) };
00118:             _vehicleSelector.ItemSelected += OnVehicleSelected;
00119:             vehicleRow.AddChild(_vehicleSelector);
00120:             _contentStack.AddChild(vehicleRow);
00121:             _contentStack.AddChild(_vehicleText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });
00122:
00123:             _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
00124:             _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("MODIFICATION CATALOG"));
00125:
00126:             var modRow = new HBoxContainer();
00127:             modRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
00128:             modRow.AddChild(AshfallUiHelpers.MakeBody("Modification:"));
00129:             _modSelector = new OptionButton { CustomMinimumSize = new Vector2(420, 34) };
00130:             _modSelector.ItemSelected += OnModSelected;
00131:             modRow.AddChild(_modSelector);
00132:             _installButton = AshfallUiHelpers.MakeButton("INSTALL", OnInstallPressed);
00133:             modRow.AddChild(_installButton);
00134:             _uninstallButton = AshfallUiHelpers.MakeButton("UNINSTALL", OnUninstallPressed);
00135:             modRow.AddChild(_uninstallButton);
00136:             _contentStack.AddChild(modRow);
00137:             _contentStack.AddChild(_modCostText = AshfallUiHelpers.MakeSmall("—"));
00138:             _contentStack.AddChild(_modText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });
00139:
00140:             _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
00141:             _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("MAINTENANCE"));
00142:
00143:             var maintRow = new HBoxContainer();
00144:             maintRow.AddThemeConstantOverride("separation", 8);
00145:             _serviceChassisButton = AshfallUiHelpers.MakeButton("SERVICE CHASSIS", OnServiceChassisPressed);
00146:             maintRow.AddChild(_serviceChassisButton);
00147:             _serviceEngineButton = AshfallUiHelpers.MakeButton("SERVICE ENGINE", OnServiceEnginePressed);
00148:             maintRow.AddChild(_serviceEngineButton);
00149:             _serviceTransmissionButton = AshfallUiHelpers.MakeButton("SERVICE TRANSMISSION", OnServiceTransmissionPressed);
00150:             maintRow.AddChild(_serviceTransmissionButton);
00151:             _contentStack.AddChild(maintRow);
00152:             _contentStack.AddChild(_maintenanceText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });
00153:
00154:             _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
00155:             _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("ARMOR PLATING"));
00156:
00157:             var armorRow = new HBoxContainer();
00158:             armorRow.AddThemeConstantOverride("separation", 8);
00159:             armorRow.AddChild(AshfallUiHelpers.MakeBody("Grade:"));
00160:             _armorSelector = new OptionButton { CustomMinimumSize = new Vector2(360, 34) };
00161:             _armorSelector.ItemSelected += OnArmorSelected;
00162:             armorRow.AddChild(_armorSelector);
00163:             _armorInstallButton = AshfallUiHelpers.MakeButton("FIT PLATE", OnInstallArmorPressed);
00164:             armorRow.AddChild(_armorInstallButton);
00165:             _armorReforgeButton = AshfallUiHelpers.MakeButton("RE-FORGE", OnReforgeArmorPressed);
00166:             armorRow.AddChild(_armorReforgeButton);
00167:             _contentStack.AddChild(armorRow);
00168:             _contentStack.AddChild(_armorCostText = AshfallUiHelpers.MakeSmall("—"));
00169:             _contentStack.AddChild(_armorText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });
00170:
00171:             _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
00172:             _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("RECOVERY OPERATIONS"));
00173:
00174:             var recRow = new HBoxContainer();
00175:             recRow.AddThemeConstantOverride("separation", 8);
00176:             _completeRecoveryButton = AshfallUiHelpers.MakeButton("COMPLETE RECOVERY", OnCompleteRecoveryPressed);
00177:             recRow.AddChild(_completeRecoveryButton);
00178:             _contentStack.AddChild(recRow);
00179:             _contentStack.AddChild(_recoveryText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });
00180:
00181:             _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
00182:             _commandResult = AshfallUiHelpers.MakeSmall("—");
00183:             _commandResult.AutowrapMode = TextServer.AutowrapMode.WordSmart;
00184:             _contentStack.AddChild(_commandResult);
00185:
00186:             var note = AshfallUiHelpers.MakeBody(
00187:                 "Fitted modifications change the sortie profile the expedition authority uses "
00188:                 + "(cargo, speed, fuel); travelled distance feeds component wear. A vehicle whose "
00189:                 + "chassis, engine, or transmission reaches catastrophic wear is immobilized and "
00190:                 + "cannot be dispatched until a recovery team completes its work.");
00191:             note.AutowrapMode = TextServer.AutowrapMode.WordSmart;
00192:             note.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
00193:             _contentStack.AddChild(note);
00194:
00195:             _shell.SetContent(scroll);
00196:             _shell.AttachHeaderCloseButton("CLOSE", () =>
00197:             {
00198:                 Visible = false;
00199:                 OnClose?.Invoke();
00200:             });
00201:
00202:             RefreshView();
00203:         }
00204:
00205:         public override void _ExitTree()
00206:         {
00207:             Unbind();
00208:             base._ExitTree();
00209:         }
00210:
00211:         // ── Selection ─────────────────────────────────────────────────────
00212:
00213:         private void OnVehicleSelected(long index)
00214:         {
00215:             if (_syncing) return;
00216:             var ids = OwnedVehicleIds();
00217:             if (index >= 0 && index < ids.Count) _selectedVehicleId = ids[(int)index];
00218:             RefreshView();
00219:         }
00220:
00221:         private void OnModSelected(long index)
00222:         {
00223:             if (_syncing) return;
00224:             var mods = SortedMods();
00225:             if (index >= 0 && index < mods.Count) _selectedModId = mods[(int)index].id;
00226:             RefreshView();
00227:         }
00228:
00229:         private void OnArmorSelected(long index)
00230:         {
00231:             if (_syncing) return;
00232:             var grades = SortedArmorGrades();
00233:             if (index >= 0 && index < grades.Count) _selectedArmorGradeId = grades[(int)index].id;
00234:             RefreshView();
00235:         }
00236:
00237:         // ── Commands (Core owns all math and inventory mutation) ──────────
00238:
00239:         private void OnInstallPressed()
00240:         {
00241:             if (_garage == null || _inventory == null) return;
00242:             string vehicleId = SelectedVehicleId();
00243:             var mod = FindMod(_selectedModId);
00244:             if (string.IsNullOrEmpty(vehicleId) || mod == null) return;
00245:             bool ok = _garage.InstallModification(vehicleId, mod.slot_type, mod.id, _inventory, out string reason);
00246:             SetResult(ok, ok ? $"Fitted {mod.display_name}." : reason);
00247:         }
00248:
00249:         private void OnUninstallPressed()
00250:         {
00251:             if (_garage == null || _inventory == null) return;
00252:             string vehicleId = SelectedVehicleId();
00253:             var mod = FindMod(_selectedModId);
00254:             if (string.IsNullOrEmpty(vehicleId) || mod == null) return;
00255:             bool ok = _garage.UninstallModification(vehicleId, mod.slot_type, _inventory, out string reason);
00256:             SetResult(ok, ok ? $"Removed {mod.display_name}." : reason);
00257:         }
00258:
00259:         private void OnInstallArmorPressed()
00260:         {
00261:             if (_garage == null || _inventory == null) return;
00262:             string vehicleId = SelectedVehicleId();
00263:             var grade = _garage.GetArmorGrade(_selectedArmorGradeId);
00264:             if (string.IsNullOrEmpty(vehicleId) || grade == null) return;
00265:             bool ok = _garage.InstallArmorGrade(vehicleId, grade.id, _inventory, out string reason);
00266:             SetResult(ok, ok ? $"Fitted {grade.display_name}." : reason);
00267:         }
00268:
00269:         private void OnReforgeArmorPressed()
00270:         {
00271:             if (_garage == null || _inventory == null) return;
00272:             string vehicleId = SelectedVehicleId();
00273:             if (string.IsNullOrEmpty(vehicleId)) return;
00274:             bool ok = _garage.ReforgeArmorPlate(vehicleId, _inventory, out string reason);
00275:             SetResult(ok, ok ? "Armor plate re-forged to its stamped integrity." : reason);
00276:         }
00277:
00278:         private void OnServiceChassisPressed()
00279:         {
00280:             if (_garage == null || _inventory == null) return;
00281:             string vehicleId = SelectedVehicleId();
00282:             var record = _garage.GetRecord(vehicleId);
00283:             if (record == null) return;
00284:             bool ok = _garage.ServiceChassis(vehicleId, _inventory, Math.Max(1, record.chassisStressPermille), out string reason);
00285:             SetResult(ok, ok ? "Chassis serviced to nominal." : reason);
00286:         }
00287:
00288:         private void OnServiceEnginePressed()
00289:         {
00290:             if (_garage == null || _inventory == null) return;
00291:             string vehicleId = SelectedVehicleId();
00292:             var record = _garage.GetRecord(vehicleId);
00293:             if (record == null) return;
00294:             bool ok = _garage.ServiceEngine(vehicleId, _inventory, Math.Max(1, record.engineFoulingPermille), out string reason);
00295:             SetResult(ok, ok ? "Engine serviced to nominal." : reason);
00296:         }
00297:
00298:         private void OnServiceTransmissionPressed()
00299:         {
00300:             if (_garage == null || _inventory == null) return;
00301:             string vehicleId = SelectedVehicleId();
00302:             var record = _garage.GetRecord(vehicleId);
00303:             if (record == null) return;
00304:             bool ok = _garage.ServiceTransmission(vehicleId, _inventory, Math.Max(1, record.transmissionWearPermille), out string reason);
00305:             SetResult(ok, ok ? "Transmission serviced to nominal." : reason);
00306:         }
00307:
00308:         private void OnCompleteRecoveryPressed()
00309:         {
00310:             if (_garage == null || _inventory == null) return;
00311:             if (string.IsNullOrEmpty(_selectedRecoveryId)) return;
00312:             bool ok = _garage.CompleteRecoveryMission(_selectedRecoveryId, _inventory, out string reason);
00313:             SetResult(ok, ok ? "Recovery complete — vehicle returned to the garage." : reason);
00314:         }
00315:
00316:         private void SetResult(bool ok, string message)
00317:         {
00318:             LastFeedback = message;
00319:             if (_commandResult != null)
00320:             {
00321:                 _commandResult.Text = message;
00322:                 _commandResult.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(
00323:                     ok ? DesignTheme.Success : DesignTheme.Warning));
00324:             }
00325:             RefreshView();
00326:         }
00327:
00328:         // ── Read-model rendering ──────────────────────────────────────────
00329:
00330:         public void RefreshView()
00331:         {
00332:             if (_garage == null || _statusRail == null) return;
00333:
00334:             _syncing = true;
00335:             try
00336:             {
00337:                 SyncVehicles();
00338:                 SyncMods();
00339:                 SyncArmorGrades();
00340:             }
00341:             finally
00342:             {
00343:                 _syncing = false;
00344:             }
00345:
00346:             var ids = OwnedVehicleIds();
00347:             int immobilized = 0;
00348:             int fitted = 0;
00349:             foreach (var id in ids)
00350:             {
00351:                 if (_garage.IsImmobilized(id)) immobilized++;
00352:                 fitted += _garage.GetInstalledSlots(id).Count;
00353:             }
00354:
00355:             _statusRail.Set("vehicles", ids.Count.ToString(), AshfallMetricCard.Criticality.Normal);
00356:             _statusRail.Set("immobilized", immobilized.ToString(),
00357:                 immobilized > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
00358:             _statusRail.Set("recoveries", _garage.ActiveRecoveries.Count.ToString(),
00359:                 _garage.ActiveRecoveries.Count > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
00360:             _statusRail.Set("fitted", fitted.ToString(), AshfallMetricCard.Criticality.Normal);
00361:
00362:             if (_detailText != null)
00363:             {
00364:                 _detailText.Text =
00365:                     $"Owned vehicles: {ids.Count} | Immobilized: {immobilized} | "
00366:                     + $"Active recoveries: {_garage.ActiveRecoveries.Count} | Fitted modifications: {fitted}";
00367:             }
00368:
00369:             RenderVehicle();
00370:             RenderFittedAndEffective();
00371:             RenderMaintenance();
00372:             RenderArmor();
00373:             RenderRecovery();
00374:         }
00375:
00376:         private void SyncVehicles()
00377:         {
00378:             if (_vehicleSelector == null) return;
00379:             _vehicleSelector.Clear();
00380:             var ids = OwnedVehicleIds();
00381:             int selected = 0;
00382:             for (int i = 0; i < ids.Count; i++)
00383:             {
00384:                 _vehicleSelector.AddItem(VehicleLabel(ids[i]), i);
00385:                 if (ids[i] == _selectedVehicleId) selected = i;
00386:             }
00387:             if (string.IsNullOrEmpty(_selectedVehicleId) && ids.Count > 0) _selectedVehicleId = ids[0];
00388:             _vehicleSelector.Selected = selected;
00389:         }
00390:
00391:         private void SyncMods()
00392:         {
00393:             if (_modSelector == null) return;
00394:             _modSelector.Clear();
00395:             var mods = SortedMods();
00396:             int selected = 0;
00397:             for (int i = 0; i < mods.Count; i++)
00398:             {
00399:                 _modSelector.AddItem($"{mods[i].display_name} [{mods[i].slot_type}]", i);
00400:                 if (mods[i].id == _selectedModId) selected = i;
00401:             }
00402:             if (string.IsNullOrEmpty(_selectedModId) && mods.Count > 0) _selectedModId = mods[0].id;
00403:             _modSelector.Selected = selected;
00404:         }
00405:
00406:         private void SyncArmorGrades()
00407:         {
00408:             if (_armorSelector == null || _garage == null) return;
00409:             _armorSelector.Clear();
00410:             var grades = SortedArmorGrades();
00411:             int selected = 0;
00412:             for (int i = 0; i < grades.Count; i++)
00413:             {
00414:                 _armorSelector.AddItem($"{grades[i].display_name} [T{grades[i].tier}]", i);
00415:                 if (grades[i].id == _selectedArmorGradeId) selected = i;
00416:             }
00417:             if (string.IsNullOrEmpty(_selectedArmorGradeId) && grades.Count > 0)
00418:                 _selectedArmorGradeId = grades[0].id;
00419:             _armorSelector.Selected = selected;
00420:         }
00421:
00422:         private void RenderVehicle()
00423:         {
00424:             if (_vehicleText == null || _garage == null) return;
00425:             string vehicleId = SelectedVehicleId();
00426:             if (string.IsNullOrEmpty(vehicleId))
00427:             {
00428:                 _vehicleText.Text = "No vehicle is owned. Acquire one through the expedition authority first.";
00429:                 return;
00430:             }
00431:             var record = _garage.GetRecord(vehicleId);
00432:             if (record == null)
00433:             {
00434:                 _vehicleText.Text = $"{VehicleLabel(vehicleId)}: no garage record yet — fitting a modification or running a sortie opens one.";
00435:                 return;
00436:             }
00437:             _vehicleText.Text =
00438:                 $"{VehicleLabel(vehicleId)} | Chassis {record.chassisStressPermille}/1000 | "
00439:                 + $"Engine {record.engineFoulingPermille}/1000 | Transmission {record.transmissionWearPermille}/1000 | "
00440:                 + $"{(record.isImmobilized ? "IMMOBILIZED — " + record.immobilizedReason : "SERVICEABLE")}";
00441:         }
00442:
00443:         private void RenderFittedAndEffective()
00444:         {
00445:             if (_modText == null || _garage == null) return;
00446:             string vehicleId = SelectedVehicleId();
00447:             var mod = FindMod(_selectedModId);
00448:
00449:             if (mod != null)
00450:             {
00451:                 var costs = new List<string>();
00452:                 foreach (var c in mod.install_cost) costs.Add($"{c.amount} × {c.item_id}");
00453:                 string effects =
00454:                     $"cargo {mod.effects.cargo_capacity_delta:+0.#;-0.#;0}, speed {mod.effects.speed_multiplier_delta:+0.##;-0.##;0}, "
00455:                     + $"fuel ×{mod.effects.fuel_consumption_multiplier:0.##}, wear ×{mod.effects.wear_rate_multiplier:0.##}, "
00456:                     + $"cab rad protection {mod.effects.radiation_protection_permille}‰";
00457:                 _modCostText!.Text =
00458:                     $"Slot: {mod.slot_type} | Install cost: {(costs.Count == 0 ? "none" : string.Join(", ", costs))}";
00459:                 _modText.Text = $"{mod.display_name} — {effects}\n{mod.description}";
00460:             }
00461:             else
00462:             {
00463:                 _modCostText!.Text = "No modification selected.";
00464:                 _modText.Text = string.Empty;
00465:             }
00466:
00467:             if (_installButton != null) _installButton.Disabled = mod == null || string.IsNullOrEmpty(vehicleId);
00468:             bool installed = mod != null && !string.IsNullOrEmpty(vehicleId)
00469:                 && _garage.GetInstalledSlots(vehicleId).TryGetValue(mod.slot_type, out var fittedId)
00470:                 && string.Equals(fittedId, mod.id, StringComparison.Ordinal);
00471:             if (_uninstallButton != null) _uninstallButton.Disabled = !installed;
00472:
00473:             var fittedSlots = string.IsNullOrEmpty(vehicleId) ? null : _garage.GetInstalledSlots(vehicleId);
00474:             if (fittedSlots != null && fittedSlots.Count > 0 && !string.IsNullOrEmpty(vehicleId))
00475:             {
00476:                 var lines = new List<string> { "FITTED:" };
00477:                 foreach (var kv in fittedSlots)
00478:                 {
00479:                     var def = _garage.GetModification(kv.Value);
00480:                     lines.Add($"  {kv.Key}: {def?.display_name ?? kv.Value}");
00481:                 }
00482:                 _modText.Text += "\n" + string.Join("\n", lines);
00483:             }
00484:         }
00485:
00486:         private void RenderMaintenance()
00487:         {
00488:             if (_maintenanceText == null || _garage == null) return;
00489:             string vehicleId = SelectedVehicleId();
00490:             var record = string.IsNullOrEmpty(vehicleId) ? null : _garage.GetRecord(vehicleId);
00491:             if (record == null)
00492:             {
00493:                 _maintenanceText.Text = "No component wear on file for the selected vehicle.";
00494:                 SetServiceButtons(false);
00495:                 return;
00496:             }
00497:
00498:             int scrap = _inventory?.CountById("scrap_metal") ?? 0;
00499:             int parts = _inventory?.CountById("mechanical_parts") ?? 0;
00500:             _maintenanceText.Text =
00501:                 $"Chassis {record.chassisStressPermille}/1000 (scrap metal on hand {scrap}) | "
00502:                 + $"Engine {record.engineFoulingPermille}/1000, Transmission {record.transmissionWearPermille}/1000 "
00503:                 + $"(mechanical parts on hand {parts})";
00504:             SetServiceButtons(true);
00505:             if (_serviceChassisButton != null) _serviceChassisButton.Disabled = record.chassisStressPermille <= 0;
00506:             if (_serviceEngineButton != null) _serviceEngineButton.Disabled = record.engineFoulingPermille <= 0;
00507:             if (_serviceTransmissionButton != null) _serviceTransmissionButton.Disabled = record.transmissionWearPermille <= 0;
00508:         }
00509:
00510:         private void RenderArmor()
00511:         {
00512:             if (_armorText == null || _armorCostText == null || _garage == null) return;
00513:             string vehicleId = SelectedVehicleId();
00514:             var profile = string.IsNullOrEmpty(vehicleId)
00515:                 ? new VehicleArmorProfile { IsDefault = true, DisplayName = "Stock Plating", ConditionBand = "none" }
00516:                 : _garage.GetArmorProfile(vehicleId);
00517:             var selected = _garage.GetArmorGrade(_selectedArmorGradeId);
00518:
00519:             if (selected != null)
00520:             {
00521:                 var costs = new List<string>();
00522:                 foreach (var cost in selected.install_cost) costs.Add($"{cost.amount} × {cost.item_id}");
00523:                 _armorCostText.Text = $"Install cost: {(costs.Count == 0 ? "none" : string.Join(", ", costs))} | labor {selected.install_labor_ticks} ticks";
00524:             }
00525:             else _armorCostText.Text = "No armor grade selected.";
00526:
00527:             if (profile.IsDefault)
00528:                 _armorText.Text = "Stock Plating — no armor fitted.";
00529:             else
00530:             {
00531:                 string material = string.IsNullOrEmpty(profile.MaterialProfileId) ? "unknown material" : profile.MaterialProfileId;
00532:                 _armorText.Text = $"{profile.DisplayName} — mitigation {profile.MitigationPermille}‰, "
00533:                     + $"absorption {profile.WearAbsorptionPermille}‰, integrity {profile.IntegrityPermille}/{profile.IntegrityMaxPermille} — "
00534:                     + $"{profile.ConditionBand} (material {material}, {profile.Purity}).";
00535:                 if (profile.ConditionBand == "depleted")
00536:                     _armorText.Text += " Mitigation inactive; mass penalty remains. Re-forge to restore.";
00537:             }
00538:
00539:             bool canInstall = selected != null && !string.IsNullOrEmpty(vehicleId)
00540:                 && _garage.CanInstallArmorGrade(vehicleId, selected.id, _inventory, out _);
00541:             if (_armorInstallButton != null) _armorInstallButton.Disabled = !canInstall;
00542:             if (_armorReforgeButton != null)
00543:                 _armorReforgeButton.Disabled = profile.IsDefault || profile.IntegrityPermille >= profile.IntegrityMaxPermille
00544:                     || _garage.IsImmobilized(vehicleId);
00545:         }
00546:
00547:         private void SetServiceButtons(bool enabled)
00548:         {
00549:             if (_serviceChassisButton != null) _serviceChassisButton.Disabled = !enabled;
00550:             if (_serviceEngineButton != null) _serviceEngineButton.Disabled = !enabled;
00551:             if (_serviceTransmissionButton != null) _serviceTransmissionButton.Disabled = !enabled;
00552:         }
00553:
00554:         private void RenderRecovery()
00555:         {
00556:             if (_recoveryText == null || _garage == null) return;
00557:             if (_garage.ActiveRecoveries.Count == 0)
00558:             {
00559:                 _recoveryText.Text = "No active recovery operations.";
00560:                 _selectedRecoveryId = string.Empty;
00561:                 if (_completeRecoveryButton != null) _completeRecoveryButton.Disabled = true;
00562:                 return;
00563:             }
00564:
00565:             var lines = new List<string>();
00566:             string firstComplete = string.Empty;
00567:             foreach (var kv in _garage.ActiveRecoveries)
00568:             {
00569:                 var m = kv.Value;
00570:                 if (m == null) continue;
00571:                 lines.Add($"{m.missionId}: {m.strandedVehicleId} stranded at {m.locationId} — "
00572:                     + $"progress {m.progressTicks}/{m.requiredTicks} ticks, fuel {m.requiredFuelUnits}, "
00573:                     + $"{(m.isComplete ? "READY TO RECOVER" : "team en route")}");
00574:                 if (m.isComplete && string.IsNullOrEmpty(firstComplete)) firstComplete = m.missionId;
00575:             }
00576:             _recoveryText.Text = string.Join("\n", lines);
00577:             _selectedRecoveryId = firstComplete;
00578:             if (_completeRecoveryButton != null) _completeRecoveryButton.Disabled = string.IsNullOrEmpty(firstComplete);
00579:         }
00580:
00581:         // ── Helpers ───────────────────────────────────────────────────────
00582:
00583:         private List<string> OwnedVehicleIds()
00584:         {
00585:             var ids = new List<string>();
00586:             if (_vehicles == null) return ids;
00587:             foreach (var kv in _vehicles.State.ownedVehicles)
00588:             {
00589:                 if (!string.IsNullOrEmpty(kv.Key)) ids.Add(kv.Key);
00590:             }
00591:             ids.Sort(StringComparer.Ordinal);
00592:             return ids;
00593:         }
00594:
00595:         private string SelectedVehicleId()
00596:         {
00597:             if (!string.IsNullOrEmpty(_selectedVehicleId)) return _selectedVehicleId;
00598:             var ids = OwnedVehicleIds();
00599:             return ids.Count > 0 ? ids[0] : string.Empty;
00600:         }
00601:
00602:         private string VehicleLabel(string vehicleId)
00603:         {
00604:             var inst = _vehicles?.GetVehicle(vehicleId);
00605:             string name = inst?.displayName ?? string.Empty;
00606:             return string.IsNullOrWhiteSpace(name) ? vehicleId : $"{name} ({vehicleId})";
00607:         }
00608:
00609:         private List<VehicleModificationDefinition> SortedMods()
00610:         {
00611:             var list = new List<VehicleModificationDefinition>();
00612:             if (_garage == null) return list;
00613:             foreach (var kv in _garage.GetAllModifications())
00614:                 if (kv.Value != null) list.Add(kv.Value);
00615:             list.Sort((a, b) => string.CompareOrdinal(a.id, b.id));
00616:             return list;
00617:         }
00618:
00619:         private List<VehicleArmorGradeDefinition> SortedArmorGrades()
00620:         {
00621:             var list = new List<VehicleArmorGradeDefinition>();
00622:             if (_garage == null) return list;
00623:             foreach (var kv in _garage.GetAllArmorGrades())
00624:                 if (kv.Value != null && !kv.Value.is_default) list.Add(kv.Value);
00625:             list.Sort((a, b) => a.tier != b.tier ? a.tier.CompareTo(b.tier) : string.CompareOrdinal(a.id, b.id));
00626:             return list;
00627:         }
00628:
00629:         private VehicleModificationDefinition? FindMod(string modId)
00630:         {
00631:             if (_garage == null || string.IsNullOrEmpty(modId)) return null;
00632:             return _garage.GetModification(modId);
00633:         }
00634:     }
00635: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/ExpeditionVehicleLogisticsTests.cs`

### `Ashfall.Core.Tests/ExpeditionVehicleLogisticsTests.cs` — complete current file

- Size: 493 lines / 22751 bytes.
- SHA-256: `2f574c226a8a522a1d4925d66d9c507ecb75d09ebb8593d907b37741f4991752`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Combat;
00007: using Ashfall.Core.Crafting;
00008: using Ashfall.Core.Expeditions;
00009: using Ashfall.Core.Inventory;
00010: using Ashfall.Core.Save;
00011: using Xunit;
00012:
00013: namespace Ashfall.Core.Tests
00014: {
00015:     /// <summary>
00016:     /// Task #101 — expedition vehicle logistics + weapon-condition bridging.
00017:     /// Pins: vehicle speed/capacity change expedition outcomes, deterministic
00018:     /// mid-route breakdowns revert to foot, the estimate math mirrors the tick
00019:     /// loop, and the bridge converts units without owning durability state.
00020:     /// </summary>
00021:     public class ExpeditionVehicleLogisticsTests
00022:     {
00023:         private static ExpeditionDefinition Def(int ticks = 8, float encounter = 0.12f) => new ExpeditionDefinition
00024:         {
00025:             id = "loc_test_range",
00026:             displayName = "Test Range",
00027:             distanceTicks = ticks,
00028:             dangerLevel = 1,
00029:             encounterChancePerTick = encounter,
00030:         };
00031:
00032:         private static ExpeditionVehicleProfile Vehicle(float speed = 1.6f, float cargo = 120f, float breakdown = 0f, float fuelPerTick = 0.5f) => new ExpeditionVehicleProfile
00033:         {
00034:             vehicleId = "vehicle_cargo_truck",
00035:             speedMultiplier = speed,
00036:             cargoCapacityKg = cargo,
00037:             breakdownChancePerTick = breakdown,
00038:             fuelPerTravelTick = fuelPerTick,
00039:         };
00040:
00041:         /// <summary>Tick until the expedition leaves the given phase (bounded).</summary>
00042:         private static int TickUntilPhase(ExpeditionSystem sys, string survivor, ExpeditionPhase from, ExpeditionPhase to, int maxTicks = 64)
00043:         {
00044:             var rng = new SeededRng(1234);
00045:             int ticks = 0;
00046:             while (ticks < maxTicks && sys.Active[survivor].phase == (int)from)
00047:             {
00048:                 sys.TickHours(1f, rng);
00049:                 ticks++;
00050:             }
00051:             Assert.Equal((int)to, sys.Active[survivor].phase);
00052:             return ticks;
00053:         }
00054:
00055:         [Fact]
00056:         public void VehicleSpeed_ShortensOutboundTravel()
00057:         {
00058:             // Foot: 8 ticks at 1.0/tick. Vehicle 2.0x: 4 ticks at 2.0/tick.
00059:             var foot = new ExpeditionSystem();
00060:             foot.Start(Def(), "s_foot", 1);
00061:             int footTicks = TickUntilPhase(foot, "s_foot", ExpeditionPhase.Outbound, ExpeditionPhase.Looting);
00062:
00063:             var driven = new ExpeditionSystem();
00064:             driven.Start(Def(), "s_drv", 1, vehicle: Vehicle(speed: 2.0f));
00065:             Assert.Equal(120f, driven.Active["s_drv"].maxLootCapacityKg);
00066:             int drivenTicks = TickUntilPhase(driven, "s_drv", ExpeditionPhase.Outbound, ExpeditionPhase.Looting);
00067:
00068:             Assert.Equal(8, footTicks);
00069:             Assert.Equal(4, drivenTicks);
00070:         }
00071:
00072:         [Fact]
00073:         public void VehicleBreakdown_IsDeterministic_AndRevertsToFoot()
00074:         {
00075:             // Breakdown guaranteed on the first travel tick; the vehicle's 2x
00076:             // speed then stops applying for the remainder of the sortie.
00077:             var sysA = new ExpeditionSystem();
00078:             var sysB = new ExpeditionSystem();
00079:             foreach (var sys in new[] { sysA, sysB })
00080:                 sys.Start(Def(10), "s", 1, vehicle: Vehicle(speed: 2.0f, breakdown: 1f));
00081:
00082:             var rngA = new SeededRng(77);
00083:             var rngB = new SeededRng(77);
00084:             sysA.TickHours(1f, rngA);
00085:             sysB.TickHours(1f, rngB);
00086:
00087:             Assert.True(sysA.Active["s"].vehicleBrokenDown);
00088:             Assert.True(sysB.Active["s"].vehicleBrokenDown);
00089:             Assert.Contains("on foot", sysA.Active["s"].outcomeText, StringComparison.Ordinal);
00090:             // Capacity reverts to the foot cap after the breakdown.
00091:             Assert.Equal(40f, sysA.Active["s"].maxLootCapacityKg);
00092:
00093:             // Same remaining rollout for both engines: determinism after the flip.
00094:             for (int i = 0; i < 12; i++)
00095:             {
00096:                 sysA.TickHours(1f, rngA);
00097:                 sysB.TickHours(1f, rngB);
00098:             }
00099:             Assert.Equal(sysB.Active["s"].travelTicksCompleted, sysA.Active["s"].travelTicksCompleted);
00100:             Assert.Equal(sysB.Active["s"].phase, sysA.Active["s"].phase);
00101:         }
00102:
00103:         [Fact]
00104:         public void VehicleBreakdown_FiresEventOnce()
00105:         {
00106:             var sys = new ExpeditionSystem();
00107:             sys.Start(Def(10), "s", 1, vehicle: Vehicle(speed: 2f, breakdown: 1f));
00108:             int fired = 0;
00109:             sys.OnVehicleBreakdown += _ => fired++;
00110:             var rng = new SeededRng(5);
00111:             sys.TickHours(1f, rng);
00112:             sys.TickHours(1f, rng);
00113:             sys.TickHours(1f, rng);
00114:             Assert.Equal(1, fired);
00115:         }
00116:
00117:         [Fact]
00118:         public void NeutralVehicle_NeverConsumesBreakdownRolls()
00119:         {
00120:             // A vehicle with speed 1.0, no capacity override and zero
00121:             // breakdown chance must produce the EXACT legacy foot trajectory
00122:             // (and therefore the identical RNG stream): the breakdown roll is
00123:             // guarded on chance > 0, so foot determinism is untouched.
00124:             var foot = new ExpeditionSystem();
00125:             var neutral = new ExpeditionSystem();
00126:             foot.Start(Def(4), "s", 1);
00127:             neutral.Start(Def(4), "s", 1,
00128:                 vehicle: Vehicle(speed: 1f, cargo: 0f, breakdown: 0f, fuelPerTick: 0f));
00129:
00130:             var rngFoot = new SeededRng(99);
00131:             var rngNeutral = new SeededRng(99);
00132:             for (int i = 0; i < 10; i++)
00133:             {
00134:                 foot.TickHours(1f, rngFoot);
00135:                 neutral.TickHours(1f, rngNeutral);
00136:             }
00137:             var a = foot.Active["s"];
00138:             var b = neutral.Active["s"];
00139:             Assert.Equal(a.phase, b.phase);
00140:             Assert.Equal(a.travelTicksCompleted, b.travelTicksCompleted);
00141:             Assert.Equal(a.lootingTicksCompleted, b.lootingTicksCompleted);
00142:             Assert.Equal(a.currentWeightKg, b.currentWeightKg, 5);
00143:             // And the streams stayed in lockstep.
00144:             Assert.Equal(rngFoot.NextDouble(), rngNeutral.NextDouble(), 10);
00145:         }
00146:
00147:         [Fact]
00148:         public void Estimate_MirrorsTickMath_ForFootAndVehicle()
00149:         {
00150:             var foot = ExpeditionSystem.Estimate(Def(8), ExpeditionStance.Speed);
00151:             Assert.False(foot.usingVehicle);
00152:             Assert.Equal(4, foot.outboundTicks);            // four 2-tick discrete steps
00153:             Assert.Equal(4, foot.inboundTicks);
00154:             Assert.Equal(0f, foot.fuelRequired);
00155:             Assert.Equal(40f, foot.cargoCapacityKg);
00156:             Assert.Equal(0f, foot.breakdownRiskTotal);
00157:
00158:             var driven = ExpeditionSystem.Estimate(
00159:                 Def(8), ExpeditionStance.Stealth, vehicle: Vehicle(speed: 2f, cargo: 120f, breakdown: 0.25f, fuelPerTick: 0.5f));
00160:             Assert.True(driven.usingVehicle);
00161:             Assert.Equal(4, driven.outboundTicks);          // ceil(8/2.0)
00162:             Assert.Equal(4, driven.inboundTicks);
00163:             Assert.Equal(4f, driven.fuelRequired);          // 0.5 * 8 travel ticks
00164:             Assert.Equal(120f, driven.cargoCapacityKg);
00165:             Assert.Equal(0.25f, driven.breakdownRiskPerTick);
00166:             Assert.Equal(1f - (float)Math.Pow(0.75, 8), driven.breakdownRiskTotal, 5);
00167:
00168:             // Stealth halves encounter risk; poor weapon readiness raises it.
00169:             // (This foot estimate is Speed stance: no stealth discount.)
00170:             Assert.Equal(0.12f, foot.encounterRiskPerTick, 5);
00171:             var stealthFoot = ExpeditionSystem.Estimate(Def(8), ExpeditionStance.Stealth);
00172:             Assert.Equal(0.06f, stealthFoot.encounterRiskPerTick, 5);
00173:             var degraded = ExpeditionSystem.Estimate(Def(8), ExpeditionStance.Stealth, weaponReadiness: 0f);
00174:             Assert.Equal(0.06f * 1.5f, degraded.encounterRiskPerTick, 5);
00175:         }
00176:
00177:         // ── Weapon/equipment bridge ─────────────────────────────────────
00178:
00179:         private static EquipmentConditionSystem Equipment(out Inventory.Inventory inv)
00180:         {
00181:             inv = new Inventory.Inventory();
00182:             return new EquipmentConditionSystem(new SeededRng(42), inv, new CraftingSystem(inv));
00183:         }
00184:
00185:         [Fact]
00186:         public void Bridge_ProjectsAuthorityConditionIntoCombatToken()
00187:         {
00188:             var eq = Equipment(out _);
00189:             eq.RegisterItem("eq_w1", "weapon_bolt_rifle", "survivor_a", EquipmentFamily.Weapon);
00190:             eq.UseItem("eq_w1", 30f); // 70/100
00191:
00192:             var token = WeaponEquipmentBridge.ToCombatInstance(eq, "weapon_bolt_rifle", "survivor_a");
00193:             Assert.Equal("eq_w1", token.InstanceId);
00194:             Assert.Equal(0.7f, token.ConditionPct, 3);
00195:
00196:             // No tracked instance → pristine, unbound token (combat fallback path).
00197:             var unbound = WeaponEquipmentBridge.ToCombatInstance(eq, "weapon_pipe_rifle", "survivor_a");
00198:             Assert.Equal(string.Empty, unbound.InstanceId);
00199:             Assert.Equal(1f, unbound.ConditionPct);
00200:         }
00201:
00202:         [Fact]
00203:         public void Bridge_PrefersOwnerAndBestCondition()
00204:         {
00205:             var eq = Equipment(out _);
00206:             eq.RegisterItem("eq_a", "weapon_bolt_rifle", "survivor_a", EquipmentFamily.Weapon);
00207:             eq.RegisterItem("eq_b", "weapon_bolt_rifle", "survivor_a", EquipmentFamily.Weapon);
00208:             eq.RegisterItem("eq_c", "weapon_bolt_rifle", "survivor_b", EquipmentFamily.Weapon);
00209:             eq.UseItem("eq_b", 10f); // eq_a (100) beats eq_b (90); eq_c is another owner's
00210:
00211:             Assert.Equal("eq_a", WeaponEquipmentBridge.FindWeaponFor(eq, "weapon_bolt_rifle", "survivor_a")!.instanceId);
00212:             Assert.Equal("eq_c", WeaponEquipmentBridge.FindWeaponFor(eq, "weapon_bolt_rifle", "survivor_b")!.instanceId);
00213:         }
00214:
00215:         [Fact]
00216:         public void Bridge_WriteBack_ConvertsUnits_ThroughAuthority()
00217:         {
00218:             var eq = Equipment(out _);
00219:             eq.RegisterItem("eq_w1", "weapon_bolt_rifle", "survivor_a", EquipmentFamily.Weapon);
00220:
00221:             WeaponEquipmentBridge.ApplyWear(eq, "eq_w1", 0.05f);
00222:             Assert.Equal(95f, eq.State.items[0].condition);
00223:
00224:             // SyncAfterCombat only writes the delta, and only for bound tokens.
00225:             var token = WeaponEquipmentBridge.ToCombatInstance(eq, "weapon_bolt_rifle", "survivor_a");
00226:             float before = token.ConditionPct;
00227:             token.ConditionPct = 0.60f;
00228:             WeaponEquipmentBridge.SyncAfterCombat(eq, token, before);
00229:             Assert.Equal(60f, eq.State.items[0].condition, 3);
00230:
00231:             float untouched = eq.State.items[0].condition;
00232:             var unbound = new WeaponInstanceState { InstanceId = "", WeaponId = "x", ConditionPct = 0.1f };
00233:             WeaponEquipmentBridge.SyncAfterCombat(eq, unbound, 1f);
00234:             Assert.Equal(untouched, eq.State.items[0].condition);
00235:         }
00236:
00237:         [Fact]
00238:         public void Bridge_Readiness_FollowsAuthorityRisks()
00239:         {
00240:             var eq = Equipment(out _);
00241:             eq.RegisterItem("eq_w1", "weapon_bolt_rifle", "survivor_a", EquipmentFamily.Weapon);
00242:
00243:             Assert.Equal(1f, WeaponEquipmentBridge.Readiness(eq, "eq_w1"));   // pristine
00244:             Assert.Equal(1f, WeaponEquipmentBridge.Readiness(null, "eq_w1")); // no authority
00245:             Assert.Equal(1f, WeaponEquipmentBridge.Readiness(eq, null));      // no selection
00246:
00247:             eq.UseItem("eq_w1", 85f); // condition 15 → jam (20-15)/20, slip (30-15)/30
00248:             float readiness = WeaponEquipmentBridge.Readiness(eq, "eq_w1");
00249:             Assert.Equal(1f - (0.25f + 0.5f) / 2f, readiness, 3);
00250:             Assert.Equal(0.25f, WeaponEquipmentBridge.JamRisk(eq, "eq_w1"), 3);
00251:
00252:             eq.UseItem("eq_w1", 15f); // condition 0 → unusable
00253:             Assert.Equal(0f, WeaponEquipmentBridge.Readiness(eq, "eq_w1"));
00254:         }
00255:
00256:         // ── Aggregate persistence (expeditions + vehicle garage) ────────
00257:
00258:         private static ExpeditionAggregateState SampleAggregate() => new ExpeditionAggregateState
00259:         {
00260:             expeditions = new List<ExpeditionState>
00261:             {
00262:                 new ExpeditionState
00263:                 {
00264:                     expeditionId = "survivor_a:loc_x",
00265:                     survivorId = "survivor_a",
00266:                     locationId = "loc_x",
00267:                     vehicleId = "vehicle_utility_quad",
00268:                     vehicleSpeedMultiplier = 1.3f,
00269:                     vehicleBreakdownChancePerTick = 0.1f,
00270:                 },
00271:             },
00272:             vehicles = new ExpeditionVehicleState
00273:             {
00274:                 ownedVehicles = new Dictionary<string, VehicleInstance>
00275:                 {
00276:                     { "vehicle_utility_quad", new VehicleInstance
00277:                         { vehicleId = "vehicle_utility_quad", condition = 80f, fuel = 12f, maxFuel = 40f } },
00278:                 },
00279:                 activeExpeditionVehicleId = "vehicle_utility_quad",
00280:             },
00281:         };
00282:
00283:         [Fact]
00284:         public void AggregateCodec_RoundTripsExpeditionsAndGarage()
00285:         {
00286:             var json = new SystemTextJsonSerializer();
00287:             var aggregate = SampleAggregate();
00288:             aggregate.completedCount = 7;
00289:             aggregate.knownLocationIds = new List<string> { "loc_x", "loc_y" };
00290:
00291:             string encoded = ExpeditionAggregateCodec.Encode(aggregate, json);
00292:             ExpeditionAggregateState? decoded = ExpeditionAggregateCodec.Decode(encoded, json);
00293:
00294:             Assert.NotNull(decoded);
00295:             Assert.Single(decoded!.expeditions);
00296:             Assert.Equal("vehicle_utility_quad", decoded.expeditions[0].vehicleId);
00297:             Assert.NotNull(decoded.vehicles);
00298:             Assert.True(decoded.vehicles.ownedVehicles.ContainsKey("vehicle_utility_quad"));
00299:             Assert.Equal(80f, decoded.vehicles.ownedVehicles["vehicle_utility_quad"].condition);
00300:             Assert.Equal(7, decoded.completedCount);
00301:             Assert.Equal(2, decoded.knownLocationIds!.Count);
00302:             Assert.True(encoded.Contains("Checksum", StringComparison.Ordinal), "payload keeps the checksummed envelope");
00303:         }
00304:
00305:         [Fact]
00306:         public void RestoreCompletedCount_SurvivesAggregateRestore()
00307:         {
00308:             var sys = new ExpeditionSystem();
00309:             sys.RestoreCompletedCount(4);
00310:             Assert.Equal(4, sys.CompletedCount);
00311:
00312:             var aggregate = new ExpeditionAggregateState
00313:             {
00314:                 expeditions = sys.CaptureState(),
00315:                 vehicles = new ExpeditionVehicleState(),
00316:                 knownLocationIds = new List<string> { "loc_known" },
00317:                 completedCount = sys.CompletedCount,
00318:             };
00319:
00320:             var restored = new ExpeditionSystem();
00321:             restored.RestoreState(aggregate.expeditions);
00322:             restored.RestoreCompletedCount(aggregate.completedCount);
00323:             restored.RestoreKnownLocations(aggregate.knownLocationIds);
00324:             Assert.Equal(4, restored.CompletedCount);
00325:             Assert.True(restored.IsLocationKnown("loc_known"));
00326:         }
00327:
00328:         [Fact]
00329:         public void AggregateCodec_MigratesLegacyEnvelopeAndBareList()
00330:         {
00331:             var json = new SystemTextJsonSerializer();
00332:             var legacyList = new List<ExpeditionState>
00333:             {
00334:                 new ExpeditionState { expeditionId = "s1:loc_y", survivorId = "s1", locationId = "loc_y" },
00335:             };
00336:
00337:             // Legacy shape 1: the pre-aggregate { State, Checksum } envelope.
00338:             var legacyEnvelope = new SaveEnvelope<List<ExpeditionState>> { State = legacyList };
00339:             legacyEnvelope.Checksum = SaveChecksum.Compute(legacyEnvelope);
00340:             string envelopeJson = json.Serialize(legacyEnvelope);
00341:             ExpeditionAggregateState? fromEnvelope = ExpeditionAggregateCodec.Decode(envelopeJson, json);
00342:             Assert.NotNull(fromEnvelope);
00343:             Assert.Single(fromEnvelope!.expeditions);
00344:             Assert.NotNull(fromEnvelope.vehicles); // fresh empty garage
00345:
00346:             // Legacy shape 2: the bare list (pre-checksum stores).
00347:             string bareJson = json.Serialize(legacyList);
00348:             ExpeditionAggregateState? fromBare = ExpeditionAggregateCodec.Decode(bareJson, json);
00349:             Assert.NotNull(fromBare);
00350:             Assert.Single(fromBare!.expeditions);
00351:             Assert.Equal("s1:loc_y", fromBare.expeditions[0].expeditionId);
00352:
00353:             // Malformed JSON propagates — the store service's logging catch
00354:             // rejects the save, exactly as the legacy bare-list store did.
00355:             Assert.ThrowsAny<Exception>(() => ExpeditionAggregateCodec.Decode("{\"nope\"", json));
00356:         }
00357:
00358:         [Fact]
00359:         public void AggregateState_RestoreContinuesInFlightVehicleExpedition()
00360:         {
00361:             var sys = new ExpeditionSystem();
00362:             sys.Start(Def(6), "s", 1, vehicle: Vehicle(speed: 2f, breakdown: 1f));
00363:             var rng = new SeededRng(31);
00364:             sys.TickHours(1f, rng); // guaranteed breakdown on the first travel tick
00365:             Assert.True(sys.Active["s"].vehicleBrokenDown);
00366:
00367:             var aggregate = new ExpeditionAggregateState
00368:             {
00369:                 expeditions = sys.CaptureState(),
00370:                 vehicles = new ExpeditionVehicleState(),
00371:             };
00372:
00373:             var restored = new ExpeditionSystem();
00374:             restored.RestoreState(aggregate.expeditions);
00375:             var e = restored.Active["s"];
00376:             Assert.True(e.vehicleBrokenDown, "mid-route breakdown must survive save/restore");
00377:             Assert.Equal("vehicle_cargo_truck", e.vehicleId);
00378:             // And the restored sortie keeps walking on foot speed.
00379:             var rng2 = new SeededRng(32);
00380:             int before = e.travelTicksCompleted;
00381:             restored.TickHours(1f, rng2);
00382:             Assert.True(restored.Active["s"].travelTicksCompleted - before <= 1);
00383:         }
00384:
00385:         [Fact]
00386:         public void VehicleCatalogLoader_ReadsDataAuthority_AndToleratesAbsence()
00387:         {
00388:             var json = new SystemTextJsonSerializer();
00389:             var files = new FileSystemIO();
00390:
00391:             // Repo data dir: walk up like the host does.
00392:             string? dir = AppContext.BaseDirectory;
00393:             string? dataDir = null;
00394:             while (dir != null)
00395:             {
00396:                 string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
00397:                 if (Directory.Exists(probe)) { dataDir = probe; break; }
00398:                 dir = Path.GetDirectoryName(dir);
00399:             }
00400:             Assert.NotNull(dataDir);
00401:
00402:             VehicleCatalog catalog = VehicleCatalogLoader.Load(dataDir!, files, json);
00403:             Assert.True(catalog.vehicles.Count >= 3, "vehicles.json must define the starter fleet");
00404:             Assert.Contains(catalog.track_gear, g => g.gear_id == "vehicle_track_gear_standard");
00405:             foreach (var v in catalog.vehicles)
00406:                 Assert.False(string.IsNullOrEmpty(v.vehicle_id));
00407:
00408:             // Missing dir → empty catalog (foot expeditions stay valid).
00409:             VehicleCatalog empty = VehicleCatalogLoader.Load("/nonexistent_dir_xyz", files, json);
00410:             Assert.Empty(empty.vehicles);
00411:         }
00412:
00413:         [Fact]
00414:         public void TrackGear_InstallsRepairsAndPersistsNormalizedEffects()
00415:         {
00416:             var vehicles = new ExpeditionVehicleSystem(new SeededRng(7));
00417:             vehicles.LoadCatalog(new VehicleCatalog
00418:             {
00419:                 vehicles = new List<VehicleDefinition>
00420:                 {
00421:                     new VehicleDefinition
00422:                     {
00423:                         vehicle_id = "vehicle_test",
00424:                         display_name = "Test Vehicle",
00425:                         cargo_capacity = 80f,
00426:                         speed_multiplier = 1.2f,
00427:                         fuel_consumption_per_km = 0.4f
00428:                     }
00429:                 }
00430:             });
00431:             Assert.True(vehicles.AcquireVehicle("vehicle_test").IsSuccess);
00432:
00433:             var installed = vehicles.InstallTrackGear("vehicle_test", "vehicle_track_gear_test", 1.4f, 0.5f, 40f);
00434:             Assert.True(installed.IsSuccess);
00435:             var live = vehicles.GetVehicle("vehicle_test")!;
00436:             Assert.Equal(1.16f, live.trackGear.EffectiveTractionMultiplier(), 3);
00437:             Assert.Equal(0.8f, live.trackGear.EffectiveBreakdownRiskMultiplier(), 3);
00438:
00439:             Assert.True(vehicles.RepairTrackGear("vehicle_test", 60f).IsSuccess);
00440:             var saved = vehicles.CaptureState();
00441:             var restored = new ExpeditionVehicleSystem(new SeededRng(8));
00442:             restored.RestoreState(saved);
00443:             var gear = restored.GetVehicle("vehicle_test")!.trackGear;
00444:             Assert.Equal("vehicle_track_gear_test", gear.gearId);
00445:             Assert.Equal(100f, gear.condition);
00446:             Assert.Equal(1.4f, gear.EffectiveTractionMultiplier(), 3);
00447:             Assert.Equal(0.5f, gear.EffectiveBreakdownRiskMultiplier(), 3);
00448:         }
00449:
00450:         [Fact]
00451:         public void TrackGear_ProfileProjectionKeepsTravelMathInCore()
00452:         {
00453:             var vehicles = new ExpeditionVehicleSystem(new SeededRng(9));
00454:             vehicles.LoadCatalog(new VehicleCatalog
00455:             {
00456:                 vehicles = new List<VehicleDefinition>
00457:                 {
00458:                     new VehicleDefinition
00459:                     {
00460:                         vehicle_id = "vehicle_test",
00461:                         speed_multiplier = 1.2f,
00462:                         fuel_consumption_per_km = 0.4f
00463:                     }
00464:                 }
00465:             });
00466:             Assert.True(vehicles.AcquireVehicle("vehicle_test").IsSuccess);
00467:             Assert.True(vehicles.InstallTrackGear("vehicle_test", "vehicle_track_gear_test", 1.5f, 0.4f).IsSuccess);
00468:
00469:             ExpeditionVehicleProfile profile = vehicles.CreateExpeditionProfile("vehicle_test", 2.5f)!;
00470:             Assert.Equal(1.8f, profile.speedMultiplier, 3);
00471:             Assert.Equal(1f, profile.fuelPerTravelTick, 3);
00472:             Assert.Equal(0f, profile.breakdownChancePerTick, 3);
00473:         }
00474:
00475:         [Fact]
00476:         public void TrackGear_RejectsInvalidFactsWithoutMutation()
00477:         {
00478:             var vehicles = new ExpeditionVehicleSystem(new SeededRng(10));
00479:             vehicles.LoadCatalog(new VehicleCatalog
00480:             {
00481:                 vehicles = new List<VehicleDefinition>
00482:                 {
00483:                     new VehicleDefinition { vehicle_id = "vehicle_test" }
00484:                 }
00485:             });
00486:             Assert.True(vehicles.AcquireVehicle("vehicle_test").IsSuccess);
00487:             var before = vehicles.GetVehicle("vehicle_test")!.trackGear.gearId;
00488:
00489:             Assert.False(vehicles.InstallTrackGear("vehicle_test", "bad", 2.1f, 0.5f).IsSuccess);
00490:             Assert.Equal(before, vehicles.GetVehicle("vehicle_test")!.trackGear.gearId);
00491:         }
00492:     }
00493: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Expeditions/ExpeditionAggregate.cs`

### `Assets/Ashfall.Core/Expeditions/ExpeditionAggregate.cs` — complete current file

- Size: 115 lines / 5002 bytes.
- SHA-256: `513c8d961b30670f32c5040ee0facf4cdb5e869da06f129c6f607f0378be88d9`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using Ashfall.Core.Save;
00006: #pragma warning disable CS8618
00007:
00008: namespace Ashfall.Core.Expeditions
00009: {
00010:     /// <summary>
00011:     /// Aggregate expedition save payload (section "expedition"): active
00012:     /// expeditions plus the vehicle garage. Prior formats — a bare
00013:     /// List&lt;ExpeditionState&gt; and the { State: [...] } envelope — are
00014:     /// migrated by the host store's decode.
00015:     /// </summary>
00016:     [Serializable]
00017:     public sealed class ExpeditionAggregateState
00018:     {
00019:         public string systemId = "expedition_aggregate";
00020:         public List<ExpeditionState> expeditions = new List<ExpeditionState>();
00021:         public ExpeditionVehicleState vehicles = new ExpeditionVehicleState();
00022:
00023:         /// <summary>F4 — discovered expedition-destination IDs. Null on legacy
00024:         /// aggregates (restore then reconstructs from the narrative resolution
00025:         /// history); a present list (even empty) is authoritative.</summary>
00026:         public List<string>? knownLocationIds = new List<string>();
00027:
00028:         /// <summary>Plan 133 — consequence provenance projected from the
00029:         /// canonical expedition destination-discovery ledger.</summary>
00030:         public DiscoveryConsequenceState? discoveryConsequences;
00031:
00032:         /// <summary>Lifetime successful returns. Defaults to 0 on legacy aggregates.</summary>
00033:         public int completedCount;
00034:     }
00035:
00036:     /// <summary>
00037:     /// Loads the vehicles.json catalog from the data authority. A missing file
00038:     /// yields an empty catalog — expeditions simply stay on foot.
00039:     /// </summary>
00040:     public static class VehicleCatalogLoader
00041:     {
00042:         public const string FileName = "vehicles.json";
00043:
00044:         public static VehicleCatalog Load(string dataDir, IFileIO files, IJsonSerializer json)
00045:         {
00046:             if (string.IsNullOrEmpty(dataDir) || files == null || json == null)
00047:                 return new VehicleCatalog();
00048:
00049:             try
00050:             {
00051:                 string path = files.Combine(dataDir, FileName);
00052:                 if (!files.FileExists(path))
00053:                     return new VehicleCatalog();
00054:                 string raw = files.ReadAllText(path);
00055:                 if (string.IsNullOrWhiteSpace(raw))
00056:                     return new VehicleCatalog();
00057:                 return json.Deserialize<VehicleCatalog>(raw) ?? new VehicleCatalog();
00058:             }
00059:             catch
00060:             {
00061:                 // A corrupt catalog must never block the game — empty garage.
00062:                 return new VehicleCatalog();
00063:             }
00064:         }
00065:
00066:         /// <summary>Convenience for hosts holding a data directory string.</summary>
00067:         public static VehicleCatalog Load(string dataDir)
00068:             => Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00069:     }
00070:
00071:     /// <summary>
00072:     /// Codec for the expedition aggregate section payload: the current
00073:     /// checksummed envelope plus migration from the two legacy shapes (the
00074:     /// pre-aggregate { State: [...] } envelope and the older bare list).
00075:     /// Payload bytes for live aggregates are the standard checksummed
00076:     /// envelope, so integrity guarantees match every other section.
00077:     /// </summary>
00078:     public static class ExpeditionAggregateCodec
00079:     {
00080:         public static string Encode(ExpeditionAggregateState aggregate, IJsonSerializer json)
00081:             => SaveEnvelopeHelper.CaptureEnvelope(aggregate, json);
00082:
00083:         public static ExpeditionAggregateState? Decode(string raw, IJsonSerializer json)
00084:         {
00085:             if (string.IsNullOrWhiteSpace(raw)) return null;
00086:
00087:             var (ok, aggregate, _) = SaveEnvelopeHelper.RestoreEnvelope<ExpeditionAggregateState>(raw, json);
00088:             if (ok) return aggregate;
00089:
00090:             // Not the current aggregate. Route by payload shape — no probing
00091:             // catches: malformed JSON propagates to the store service's
00092:             // logging catch, which rejects the save exactly as before.
00093:             string trimmed = raw.TrimStart();
00094:             if (trimmed.StartsWith("[", StringComparison.Ordinal))
00095:             {
00096:                 // Legacy shape 2: the bare expedition list (pre-checksum store).
00097:                 var bare = json.Deserialize<List<ExpeditionState>>(raw);
00098:                 return bare != null ? new ExpeditionAggregateState { expeditions = bare } : null;
00099:             }
00100:
00101:             if (trimmed.StartsWith("{", StringComparison.Ordinal))
00102:             {
00103:                 // Legacy shape 1: the pre-aggregate checksummed envelope whose
00104:                 // State is the bare expedition list. An object that is neither
00105:                 // shape (or fails integrity) decodes to null — rejected.
00106:                 var legacy = json.Deserialize<SaveEnvelope<List<ExpeditionState>>>(raw);
00107:                 if (legacy?.State != null)
00108:                     return new ExpeditionAggregateState { expeditions = legacy.State };
00109:                 return null;
00110:             }
00111:
00112:             return null;
00113:         }
00114:     }
00115: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs`

### `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs` — bounded current excerpt (712 of 864 lines)

- Size: 864 lines / 36461 bytes.
- SHA-256: `6493ee6924ace913e975210a1a66500670279fd2ffc0d3099902373b0b4fd982`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Linq;
00005: using Ashfall.Core.Foundry;
00006: using Ashfall.Core.Inventory;
00007:
00008: namespace Ashfall.Core.Expeditions
00009: {
00010:     [Serializable]
00011:     public sealed class VehicleCustomizationRecord
00012:     {
00013:         public string vehicleId = string.Empty;
00014:         public Dictionary<string, string> installedSlots = new Dictionary<string, string>(StringComparer.Ordinal);
00015:         public int chassisStressPermille;
00016:         public int engineFoulingPermille;
00017:         public int transmissionWearPermille;
00018:         public bool isImmobilized;
00019:         public string immobilizedReason = string.Empty;
00020:         // CF-P6 — additive armor state. Missing fields in old saves deserialize
00021:         // to the stock/default values and therefore preserve legacy behavior.
00022:         public string armorGradeId = string.Empty;
00023:         public int armorIntegrityPermille;
00024:         public int armorIntegrityMaxPermille;
00025:         public string armorMaterialProfileId = string.Empty;
00026:         public string armorPurity = FoundryPurityNames.Standard;
00027:     }
00028:
00029:     [Serializable]
00030:     public sealed class VehicleRecoveryMission
00031:     {
00032:         public string missionId = string.Empty;
00033:         public string strandedVehicleId = string.Empty;
00034:         public string locationId = string.Empty;
00035:         public int requiredFuelUnits = 10;
00036:         public int progressTicks;
00037:         public int requiredTicks = 120;
00038:         public bool isComplete;
00039:     }
00040:
00041:     [Serializable]
00042:     public sealed class VehicleGarageState
00043:     {
00044:         public string systemId = VehicleGarageSystem.SystemId;
00045:         public Dictionary<string, VehicleCustomizationRecord> vehicleRecords = new Dictionary<string, VehicleCustomizationRecord>(StringComparer.Ordinal);
00046:         public Dictionary<string, VehicleRecoveryMission> activeRecoveries = new Dictionary<string, VehicleRecoveryMission>(StringComparer.Ordinal);
00047:         public int nextRecoveryCounter = 1;
00048:     }
00049:
00050:     public sealed class VehicleGarageSystem
00051:     {
00052:         public const string SystemId = "vehicle_garage";
00053:
00054:         public const string SlotCargo = "cargo";
00055:         public const string SlotProtection = "protection";
00056:         public const string SlotMobility = "mobility";
00057:         public const string SlotEngine = "engine";
00058:         public const string SlotUtility = "utility";
00059:
00060:         private readonly Dictionary<string, VehicleModificationDefinition> _modCatalog =
00061:             new Dictionary<string, VehicleModificationDefinition>(StringComparer.Ordinal);
00062:         private readonly Dictionary<string, VehicleArmorGradeDefinition> _armorCatalog =
00063:             new Dictionary<string, VehicleArmorGradeDefinition>(StringComparer.Ordinal);
00064:         private readonly ISeededRng _rng;
00065:         private VehicleGarageState _state = new VehicleGarageState();
00066:
00067:         public delegate bool TryGetArmorMaterialQuality(out FoundryMaterialQuality quality);
00068:
00069:         /// <summary>Optional foundry handoff. Unset/false produces a neutral stamp.</summary>
00070:         public TryGetArmorMaterialQuality? ArmorMaterialQualitySource { get; set; }
00071:
00072:         /// <summary>Optional vehicle classifier. Unset keeps pure-Core callers neutral.</summary>
00073:         public Func<string, string?>? VehicleTerrainResolver { get; set; }
00074:
00075:         private string _armorDefaultGradeId = string.Empty;
00076:
00077:         public VehicleGarageSystem(VehicleGarageCatalog? catalog = null, ISeededRng? rng = null)
00078:         {
00079:             _rng = rng ?? new SeededRng(1337);
00080:             if (catalog != null)
00081:             {
00082:                 LoadCatalog(catalog);
00083:             }
00084:         }
00085:
00086:         public void LoadCatalog(VehicleGarageCatalog catalog)
00087:         {
00088:             if (catalog == null) throw new ArgumentNullException(nameof(catalog));
00089:             _modCatalog.Clear();
00090:             foreach (var mod in catalog.modifications)
00091:             {
00092:                 if (!string.IsNullOrEmpty(mod.id))
00093:                 {
00094:                     _modCatalog[mod.id] = mod;
00095:                 }
00096:             }
00097:         }
00098:
00099:         public bool HasModification(string modId) => _modCatalog.ContainsKey(modId);
00100:
00101:         public VehicleModificationDefinition? GetModification(string modId)
00102:         {
00103:             _modCatalog.TryGetValue(modId, out var def);
00104:             return def;
00105:         }
00106:
00107:         public IReadOnlyDictionary<string, VehicleModificationDefinition> GetAllModifications() => _modCatalog;
00108:
00109:         public void LoadArmorCatalog(VehicleArmorGradeCatalog catalog)
00110:         {
00111:             if (catalog == null) throw new ArgumentNullException(nameof(catalog));
00112:             _armorCatalog.Clear();
00113:             foreach (var grade in catalog.grades)
00114:             {
00115:                 if (grade != null && !string.IsNullOrEmpty(grade.id))
00116:                     _armorCatalog[grade.id] = grade;
00117:             }
00118:             _armorDefaultGradeId = catalog.default_grade_id ?? string.Empty;
00119:         }
00120:
00121:         public bool HasArmorGrade(string gradeId) => !string.IsNullOrEmpty(gradeId) && _armorCatalog.ContainsKey(gradeId);
00122:
00123:         public VehicleArmorGradeDefinition? GetArmorGrade(string gradeId)
00124:         {
00125:             _armorCatalog.TryGetValue(gradeId, out var definition);
00126:             return definition;
00127:         }
00128:
00129:         public IReadOnlyDictionary<string, VehicleArmorGradeDefinition> GetAllArmorGrades() => _armorCatalog;
00130:
00131:         public VehicleArmorProfile GetArmorProfile(string vehicleId)
00132:         {
00133:             var record = GetRecord(vehicleId);
00134:             if (record == null || string.IsNullOrEmpty(record.armorGradeId)
00135:                 || !_armorCatalog.TryGetValue(record.armorGradeId, out var grade)
00136:                 || grade.is_default)
00137:             {
00138:                 return CreateDefaultArmorProfile();
00139:             }
00148:                 Tier = grade.tier,
00149:                 IsDefault = false,
00150:                 MitigationPermille = active ? Math.Clamp(grade.mitigation_permille, 0, 400) : 0,
00151:                 WearAbsorptionPermille = active ? Math.Clamp(grade.wear_absorption_permille, 0, 500) : 0,
00152:                 IntegrityPermille = integrity,
00153:                 IntegrityMaxPermille = integrityMax,
00154:                 MaterialProfileId = record.armorMaterialProfileId ?? string.Empty,
00155:                 Purity = string.IsNullOrEmpty(record.armorPurity) ? FoundryPurityNames.Standard : record.armorPurity,
00156:                 ConditionBand = ArmorConditionBand(integrity, integrityMax),
00157:                 SpeedMultiplierDelta = grade.speed_multiplier_delta,
00158:                 FuelConsumptionMultiplier = grade.fuel_consumption_multiplier
00159:             };
00160:         }
00161:
00162:         public static string ArmorConditionBand(int integrityPermille, int maxPermille)
00163:         {
00164:             if (maxPermille <= 0) return "none";
00165:             if (integrityPermille <= 0) return "depleted";
00166:             if (integrityPermille * 100 >= maxPermille * 75) return "nominal";
00167:             if (integrityPermille * 100 >= maxPermille * 25) return "worn";
00168:             return "critical";
00173:             VehicleArmorGradeDefinition? grade = null;
00174:             if (!string.IsNullOrEmpty(_armorDefaultGradeId))
00175:                 _armorCatalog.TryGetValue(_armorDefaultGradeId, out grade);
00176:             return new VehicleArmorProfile
00177:             {
00178:                 GradeId = grade?.id ?? _armorDefaultGradeId,
00179:                 DisplayName = grade?.display_name ?? "Stock Plating",
00180:                 Tier = grade?.tier ?? 0,
00181:                 IsDefault = true,
00182:                 ConditionBand = "none",
00183:                 MaterialProfileId = string.Empty,
00184:                 Purity = FoundryPurityNames.Standard,
00185:                 SpeedMultiplierDelta = grade?.speed_multiplier_delta ?? 0f,
00186:                 FuelConsumptionMultiplier = grade?.fuel_consumption_multiplier ?? 1f
00187:             };
00188:         }
00189:
00190:         /// <summary>Read-only customization record for a vehicle, or null when it has none yet.</summary>
00191:         public VehicleCustomizationRecord? GetRecord(string vehicleId) =>
00192:             !string.IsNullOrEmpty(vehicleId) && _state.vehicleRecords.TryGetValue(vehicleId, out var record) ? record : null;
00193:
00194:         /// <summary>True when the vehicle is stranded/mission-critical-down in the garage.</summary>
00195:         public bool IsImmobilized(string vehicleId) => GetRecord(vehicleId)?.isImmobilized == true;
00196:
00197:         /// <summary>Active recovery missions (read-only).</summary>
00198:         public IReadOnlyDictionary<string, VehicleRecoveryMission> ActiveRecoveries => _state.activeRecoveries;
00199:
00200:         private static readonly Dictionary<string, string> EmptySlots = new Dictionary<string, string>(StringComparer.Ordinal);
00201:
00202:         /// <summary>Installed slot→modification map for a vehicle (empty when none).</summary>
00203:         public IReadOnlyDictionary<string, string> GetInstalledSlots(string vehicleId) =>
00204:             GetRecord(vehicleId)?.installedSlots ?? EmptySlots;
00205:
00206:         /// <summary>
00207:         /// Apply the installed-modification effects to an expedition profile
00208:         /// built by <see cref="ExpeditionVehicleSystem.CreateExpeditionProfile"/>.
00209:         /// The garage decorates; the expedition core stays decoupled and owns
00210:         /// travel. Read-only over persisted state; zero RNG.
00211:         /// </summary>
00212:         public void DecorateProfile(ExpeditionVehicleProfile? profile)
00213:         {
00214:             if (profile == null) return;
00215:             string vehicleId = profile.vehicleId;
00216:             if (string.IsNullOrEmpty(vehicleId)) return;
00217:
00218:             profile.cargoCapacityKg += GetEffectiveCargoCapacityDelta(vehicleId);
00219:             profile.speedMultiplier = Math.Max(0.01f, profile.speedMultiplier * (1f + GetEffectiveSpeedMultiplierDelta(vehicleId)));
00220:             profile.fuelPerTravelTick = Math.Max(0f, profile.fuelPerTravelTick * GetEffectiveFuelConsumptionMultiplier(vehicleId));
00221:
00222:             // CF-P6 armor is an additive second decorator. Mass remains present
00223:             // when a plate is depleted; mitigation alone is gated by integrity.
00224:             var armor = GetArmorProfile(vehicleId);
00225:             if (!armor.IsDefault)
00226:             {
00227:                 profile.speedMultiplier = Math.Max(0.01f, profile.speedMultiplier * (1f + armor.SpeedMultiplierDelta));
00228:                 profile.fuelPerTravelTick = Math.Max(0f, profile.fuelPerTravelTick * armor.FuelConsumptionMultiplier);
00229:                 if (profile.breakdownChancePerTick > 0f && armor.MitigationPermille > 0)
00230:                 {
00231:                     profile.breakdownChancePerTick = Math.Clamp(
00232:                         profile.breakdownChancePerTick * (1000 - armor.MitigationPermille) / 1000f,
00233:                         0f, 1f);
00234:                 }
00235:             }
00236:         }
00237:
00238:         /// <summary>
00239:         /// Advance every active recovery mission by the given ticks. Missions
00240:         /// complete at their authored <see cref="VehicleRecoveryMission.requiredTicks"/>
00241:         /// threshold; completion itself remains the player's command
00242:         /// (<see cref="CompleteRecoveryMission"/>). Deterministic; no RNG.
00243:         /// </summary>
00244:         public int AdvanceRecoveries(int deltaTicks)
00245:         {
00246:             if (deltaTicks <= 0) return 0;
00247:             int advanced = 0;
00248:             foreach (var mission in _state.activeRecoveries.Values)
00249:             {
00250:                 if (mission == null || mission.isComplete) continue;
00251:                 mission.progressTicks += deltaTicks;
00252:                 if (mission.progressTicks >= mission.requiredTicks)
00253:                     mission.isComplete = true;
00254:                 advanced++;
00255:             }
00256:             return advanced;
00257:         }
00258:
00259:         public VehicleCustomizationRecord GetOrCreateRecord(string vehicleId)
00260:         {
00261:             if (string.IsNullOrEmpty(vehicleId))
00262:                 throw new ArgumentException("Vehicle ID cannot be null or empty.", nameof(vehicleId));
00263:
00264:             if (!_state.vehicleRecords.TryGetValue(vehicleId, out var record))
00265:             {
00266:                 record = new VehicleCustomizationRecord { vehicleId = vehicleId };
00267:                 _state.vehicleRecords[vehicleId] = record;
00268:             }
00269:             return record;
00270:         }
00271:
00272:         public bool HasVehicleRecord(string vehicleId) =>
00273:             !string.IsNullOrEmpty(vehicleId) && _state.vehicleRecords.ContainsKey(vehicleId);
00274:
00275:         public bool CanInstallArmorGrade(string vehicleId, string gradeId, IPlayerInventoryPort? inventory, out string reason)
00276:         {
00277:             reason = string.Empty;
00278:             if (string.IsNullOrEmpty(vehicleId))
00279:             {
00281:                 return false;
00282:             }
00283:             if (!_armorCatalog.TryGetValue(gradeId, out var grade))
00284:             {
00285:                 reason = $"Armor grade '{gradeId}' not found in catalog.";
00286:                 return false;
00287:             }
00288:             if (VehicleTerrainResolver != null && VehicleTerrainResolver(vehicleId) == null)
00289:             {
00290:                 reason = $"Vehicle '{vehicleId}' was not found.";
00291:                 return false;
00292:             }
00297:             }
00298:
00299:             var record = GetOrCreateRecord(vehicleId);
00300:             if (string.Equals(record.armorGradeId, grade.id, StringComparison.Ordinal))
00301:             {
00302:                 reason = $"Vehicle already wears grade '{grade.id}'.";
00303:                 return false;
00309:             }
00310:
00311:             string? terrain = VehicleTerrainResolver?.Invoke(vehicleId);
00312:             if (!string.IsNullOrEmpty(terrain)
00313:                 && (grade.compatible_terrain_types == null
00314:                     || !grade.compatible_terrain_types.Contains(terrain, StringComparer.Ordinal)))
00315:             {
00316:                 reason = $"Grade '{grade.id}' cannot be fitted to '{terrain}' terrain vehicles.";
00317:                 return false;
00318:             }
00332:         }
00333:
00334:         public bool InstallArmorGrade(string vehicleId, string gradeId, IPlayerInventoryPort? inventory, out string reason)
00335:         {
00336:             if (!CanInstallArmorGrade(vehicleId, gradeId, inventory, out reason)) return false;
00337:
00338:             var grade = _armorCatalog[gradeId];
00339:             var bill = BuildArmorBill(grade.install_cost);
00340:             if (inventory != null && bill.Count > 0 && !inventory.TryConsumeBill(bill))
00341:             {
00342:                 reason = "Failed to consume armor installation materials atomically from inventory.";
00343:                 return false;
00344:             }
00345:
00346:             var record = GetOrCreateRecord(vehicleId);
00347:             if (!string.IsNullOrEmpty(record.armorGradeId)
00348:                 && _armorCatalog.TryGetValue(record.armorGradeId, out var oldGrade))
00349:             {
00350:                 foreach (var cost in oldGrade.install_cost)
00351:                 {
00352:                     if (cost.item_id == "scrap_metal" && inventory != null)
00383:         }
00384:
00385:         public bool ReforgeArmorPlate(string vehicleId, IPlayerInventoryPort? inventory, out string reason)
00386:         {
00387:             reason = string.Empty;
00388:             var record = GetRecord(vehicleId);
00389:             if (record == null || string.IsNullOrEmpty(record.armorGradeId)
00390:                 || !_armorCatalog.TryGetValue(record.armorGradeId, out var grade)
00391:                 || grade.is_default)
00392:             {
00393:                 reason = "No fitted armor plate to re-forge.";
00394:                 return false;
00417:             }
00418:             var bill = BuildArmorBill(grade.reforge_cost);
00419:             if (inventory != null && bill.Count > 0 && !inventory.TryConsumeBill(bill))
00420:             {
00421:                 reason = "Failed to consume re-forge materials atomically from inventory.";
00422:                 return false;
00423:             }
00424:             record.armorIntegrityPermille = Math.Max(0, record.armorIntegrityMaxPermille);
00425:             return true;
00426:         }
00427:
00428:         private static Dictionary<string, int> BuildArmorBill(IReadOnlyList<VehicleArmorGradeCost> costs)
00429:         {
00430:             var bill = new Dictionary<string, int>(StringComparer.Ordinal);
00431:             foreach (var cost in costs)
00432:             {
00433:                 if (string.IsNullOrEmpty(cost.item_id) || cost.amount <= 0) continue;
00434:                 bill[cost.item_id] = bill.TryGetValue(cost.item_id, out int current)
00435:                     ? current + cost.amount : cost.amount;
00436:             }
00437:             return bill;
00438:         }
00442:             FoundryPurityTier.Poor => 850,
00443:             FoundryPurityTier.High => 1100,
00444:             FoundryPurityTier.Exceptional => 1200,
00445:             _ => 1000
00446:         };
00447:
00448:         private int GetEffectiveArmorMitigationPermille(string vehicleId)
00449:         {
00450:             var profile = GetArmorProfile(vehicleId);
00451:             return profile.IsDefault ? 0 : profile.MitigationPermille;
00452:         }
00453:
00454:         private int GetEffectiveArmorWearAbsorptionPermille(string vehicleId)
00455:         {
00456:             var profile = GetArmorProfile(vehicleId);
00457:             return profile.IsDefault ? 0 : profile.WearAbsorptionPermille;
00458:         }
00459:
00460:         public bool CanInstallModification(string vehicleId, string slotType, string modId, IPlayerInventoryPort? inventory, out string reason)
00461:         {
00462:             reason = string.Empty;
00463:             if (string.IsNullOrEmpty(vehicleId))
00464:             {
00471:                 return false;
00472:             }
00473:             if (!_modCatalog.TryGetValue(modId, out var modDef))
00474:             {
00475:                 reason = $"Modification '{modId}' not found in catalog.";
00476:                 return false;
00477:             }
00478:             if (!string.Equals(modDef.slot_type, slotType, StringComparison.OrdinalIgnoreCase))
00479:             {
00482:             }
00483:
00484:             var record = GetOrCreateRecord(vehicleId);
00485:             if (record.isImmobilized)
00486:             {
00487:                 reason = $"Vehicle '{vehicleId}' is immobilized and cannot be modified until recovered.";
00488:                 return false;
00504:         }
00505:
00506:         public bool InstallModification(string vehicleId, string slotType, string modId, IPlayerInventoryPort? inventory, out string reason)
00507:         {
00508:             if (!CanInstallModification(vehicleId, slotType, modId, inventory, out reason))
00509:             {
00510:                 return false;
00511:             }
00512:
00513:             var modDef = _modCatalog[modId];
00514:             if (inventory != null && modDef.install_cost.Count > 0)
00515:             {
00516:                 var bill = new Dictionary<string, int>(StringComparer.Ordinal);
00517:                 foreach (var cost in modDef.install_cost)
00518:                 {
00519:                     bill[cost.item_id] = cost.amount;
00520:                 }
00521:                 if (!inventory.TryConsumeBill(bill))
00522:                 {
00523:                     reason = "Failed to consume installation materials atomically from inventory.";
00524:                     return false;
00525:                 }
00526:             }
00527:
00528:             var record = GetOrCreateRecord(vehicleId);
00529:             record.installedSlots[slotType] = modId;
00530:             return true;
00531:         }
00532:
00533:         public bool UninstallModification(string vehicleId, string slotType, IPlayerInventoryPort? inventory, out string reason)
00534:         {
00535:             reason = string.Empty;
00536:             if (!HasVehicleRecord(vehicleId))
00537:             {
00540:             }
00541:
00542:             var record = GetOrCreateRecord(vehicleId);
00543:             if (!record.installedSlots.TryGetValue(slotType, out var currentModId))
00544:             {
00545:                 reason = $"Slot '{slotType}' has no modification installed.";
00546:                 return false;
00547:             }
00549:             record.installedSlots.Remove(slotType);
00550:
00551:             // Refund 50% scrap metal if catalog defines install cost
00552:             if (inventory != null && _modCatalog.TryGetValue(currentModId, out var modDef))
00553:             {
00554:                 foreach (var cost in modDef.install_cost)
00555:                 {
00556:                     int refund = Math.Max(1, cost.amount / 2);
00562:         }
00563:
00564:         public float GetEffectiveCargoCapacityDelta(string vehicleId)
00565:         {
00566:             if (!_state.vehicleRecords.TryGetValue(vehicleId, out var record)) return 0f;
00567:             float total = 0f;
00568:             foreach (var kvp in record.installedSlots)
00569:             {
00570:                 if (_modCatalog.TryGetValue(kvp.Value, out var modDef))
00571:                     total += modDef.effects.cargo_capacity_delta;
00572:             }
00573:             return total;
00574:         }
00575:
00576:         public float GetEffectiveSpeedMultiplierDelta(string vehicleId)
00577:         {
00578:             if (!_state.vehicleRecords.TryGetValue(vehicleId, out var record)) return 0f;
00579:             float total = 0f;
00580:             foreach (var kvp in record.installedSlots)
00581:             {
00582:                 if (_modCatalog.TryGetValue(kvp.Value, out var modDef))
00583:                     total += modDef.effects.speed_multiplier_delta;
00584:             }
00585:             return total;
00586:         }
00587:
00588:         public float GetEffectiveFuelConsumptionMultiplier(string vehicleId)
00589:         {
00590:             if (!_state.vehicleRecords.TryGetValue(vehicleId, out var record)) return 1f;
00591:             float mult = 1f;
00592:             foreach (var kvp in record.installedSlots)
00593:             {
00594:                 if (_modCatalog.TryGetValue(kvp.Value, out var modDef))
00595:                     mult *= modDef.effects.fuel_consumption_multiplier;
00596:             }
00597:             return Math.Max(0.1f, mult);
00598:         }
00599:
00600:         public float GetEffectiveWearRateMultiplier(string vehicleId)
00601:         {
00602:             if (!_state.vehicleRecords.TryGetValue(vehicleId, out var record)) return 1f;
00603:             float mult = 1f;
00604:             foreach (var kvp in record.installedSlots)
00605:             {
00606:                 if (_modCatalog.TryGetValue(kvp.Value, out var modDef))
00607:                     mult *= modDef.effects.wear_rate_multiplier;
00608:             }
00609:             return Math.Max(0.1f, mult);
00610:         }
00611:
00612:         public int GetEffectiveRadiationProtectionPermille(string vehicleId)
00613:         {
00614:             if (!_state.vehicleRecords.TryGetValue(vehicleId, out var record)) return 0;
00615:             int total = 0;
00616:             foreach (var kvp in record.installedSlots)
00617:             {
00618:                 if (_modCatalog.TryGetValue(kvp.Value, out var modDef))
00619:                     total += modDef.effects.radiation_protection_permille;
00620:             }
00621:             return Math.Clamp(total, 0, 950);
00622:         }
00623:
00624:         public void RecordTripWear(string vehicleId, float distanceKm, float roadRoughnessMultiplier = 1f)
00625:         {
00626:             if (string.IsNullOrEmpty(vehicleId) || distanceKm <= 0f) return;
00627:             var record = GetOrCreateRecord(vehicleId);
00628:             if (record.isImmobilized) return;
00629:
00630:             float wearMult = GetEffectiveWearRateMultiplier(vehicleId) * Math.Max(0.5f, roadRoughnessMultiplier);
00631:             int baseWear = (int)Math.Round(distanceKm * 2f * wearMult);
00632:
00633:             int absorbed = 0;
00634:             int absorption = GetEffectiveArmorWearAbsorptionPermille(vehicleId);
00635:             if (absorption > 0)
00636:             {
00637:                 absorbed = Math.Min(
00638:                     Math.Max(0, record.armorIntegrityPermille),
00643:             record.chassisStressPermille = Math.Clamp(record.chassisStressPermille + baseWear - absorbed, 0, 1000);
00644:             record.engineFoulingPermille = Math.Clamp(record.engineFoulingPermille + (int)Math.Round(baseWear * 0.8f), 0, 1000);
00645:             record.transmissionWearPermille = Math.Clamp(record.transmissionWearPermille + (int)Math.Round(baseWear * 0.9f), 0, 1000);
00646:
00647:             if (record.chassisStressPermille >= 1000 || record.engineFoulingPermille >= 1000 || record.transmissionWearPermille >= 1000)
00648:             {
00649:                 record.isImmobilized = true;
00650:                 record.immobilizedReason = "Critical component catastrophic failure during overland transit.";
00651:             }
00652:         }
00653:
00654:         public bool ServiceChassis(string vehicleId, IPlayerInventoryPort? inventory, int repairPermille, out string reason)
00655:         {
00656:             reason = string.Empty;
00657:             var record = GetOrCreateRecord(vehicleId);
00658:             if (record.chassisStressPermille <= 0)
00659:             {
00660:                 reason = "Chassis is already at nominal condition.";
00661:                 return false;
00662:             }
00663:
00664:             int toRepair = Math.Min(repairPermille, record.chassisStressPermille);
00672:                     return false;
00673:                 }
00674:                 if (!inventory.TryConsume("scrap_metal", costScrap))
00675:                 {
00676:                     reason = "Failed to consume scrap metal for chassis service.";
00677:                     return false;
00678:                 }
00679:             }
00680:
00684:         }
00685:
00686:         public bool ServiceEngine(string vehicleId, IPlayerInventoryPort? inventory, int repairPermille, out string reason)
00687:         {
00688:             reason = string.Empty;
00689:             var record = GetOrCreateRecord(vehicleId);
00690:             if (record.engineFoulingPermille <= 0)
00691:             {
00692:                 reason = "Engine is already at nominal condition.";
00693:                 return false;
00694:             }
00695:
00696:             int toRepair = Math.Min(repairPermille, record.engineFoulingPermille);
00704:                     return false;
00705:                 }
00706:                 if (!inventory.TryConsume("mechanical_parts", costParts))
00707:                 {
00708:                     reason = "Failed to consume mechanical parts for engine service.";
00709:                     return false;
00710:                 }
00711:             }
00712:
00716:         }
00717:
00718:         public bool ServiceTransmission(string vehicleId, IPlayerInventoryPort? inventory, int repairPermille, out string reason)
00719:         {
00720:             reason = string.Empty;
00721:             var record = GetOrCreateRecord(vehicleId);
00722:             if (record.transmissionWearPermille <= 0)
00723:             {
00724:                 reason = "Transmission is already at nominal condition.";
00725:                 return false;
00726:             }
00727:
00728:             int toRepair = Math.Min(repairPermille, record.transmissionWearPermille);
00729:             int costParts = Math.Max(1, (int)Math.Ceiling(toRepair / 100.0));
00730:
00731:             if (inventory != null)
00732:             {
00736:                     return false;
00737:                 }
00738:                 if (!inventory.TryConsume("mechanical_parts", costParts))
00739:                 {
00740:                     reason = "Failed to consume mechanical parts for transmission service.";
00741:                     return false;
00742:                 }
00743:             }
00744:
00745:             record.transmissionWearPermille = Math.Max(0, record.transmissionWearPermille - toRepair);
00746:             CheckClearImmobilization(record);
00747:             return true;
00748:         }
00749:
00750:         private static void CheckClearImmobilization(VehicleCustomizationRecord record)
00751:         {
00752:             if (record.isImmobilized &&
00753:                 record.chassisStressPermille < 900 &&
00754:                 record.engineFoulingPermille < 900 &&
00755:                 record.transmissionWearPermille < 900)
00756:             {
00757:                 record.isImmobilized = false;
00758:                 record.immobilizedReason = string.Empty;
00759:             }
00760:         }
00761:
00762:         public bool RegisterRecoveryMission(string vehicleId, string locationId, int requiredFuelUnits, out string missionId, out string reason)
00763:         {
00764:             missionId = string.Empty;
00765:             reason = string.Empty;
00766:             var record = GetOrCreateRecord(vehicleId);
00767:             if (!record.isImmobilized)
00768:             {
00769:                 reason = $"Vehicle '{vehicleId}' is not immobilized.";
00770:                 return false;
00775:                 if (string.Equals(mission.strandedVehicleId, vehicleId, StringComparison.Ordinal))
00776:                 {
00777:                     missionId = mission.missionId;
00778:                     reason = "Recovery mission already active for this vehicle.";
00779:                     return false;
00780:                 }
00781:             }
00782:
00783:             missionId = $"recov_{_state.nextRecoveryCounter++}_{vehicleId}";
00784:             var newMission = new VehicleRecoveryMission
00785:             {
00786:                 missionId = missionId,
00787:                 strandedVehicleId = vehicleId,
00788:                 locationId = locationId,
00789:                 requiredFuelUnits = requiredFuelUnits,
00790:                 progressTicks = 0,
00791:                 requiredTicks = 120,
00792:                 isComplete = false
00793:             };
00794:             _state.activeRecoveries[missionId] = newMission;
00795:             return true;
00796:         }
00797:
00798:         public bool AdvanceRecoveryMission(string missionId, int deltaTicks, out bool completed)
00799:         {
00800:             completed = false;
00801:             if (!_state.activeRecoveries.TryGetValue(missionId, out var mission))
00802:                 return false;
00803:
00804:             if (mission.isComplete)
00805:             {
00806:                 completed = true;
00807:                 return true;
00808:             }
00809:
00810:             mission.progressTicks += Math.Max(0, deltaTicks);
00811:             if (mission.progressTicks >= mission.requiredTicks)
00812:             {
00813:                 mission.isComplete = true;
00814:                 completed = true;
00815:             }
00816:             return true;
00817:         }
00818:
00819:         public bool CompleteRecoveryMission(string missionId, IPlayerInventoryPort? inventory, out string reason)
00820:         {
00821:             reason = string.Empty;
00822:             if (!_state.activeRecoveries.TryGetValue(missionId, out var mission))
00823:             {
00824:                 reason = $"Recovery mission '{missionId}' not found.";
00825:                 return false;
00826:             }
00827:             if (!mission.isComplete)
00828:             {
00829:                 reason = $"Recovery mission '{missionId}' is still in progress ({mission.progressTicks}/{mission.requiredTicks} ticks).";
00830:                 return false;
00831:             }
00832:
00833:             var record = GetOrCreateRecord(mission.strandedVehicleId);
00834:             // Hauling vehicle back resets catastrophic wear to 800 permille so it can be serviced in garage
00835:             record.chassisStressPermille = Math.Min(800, record.chassisStressPermille);
00836:             record.engineFoulingPermille = Math.Min(800, record.engineFoulingPermille);
00837:             record.transmissionWearPermille = Math.Min(800, record.transmissionWearPermille);
00838:             record.isImmobilized = false;
00839:             record.immobilizedReason = string.Empty;
00840:
00841:             _state.activeRecoveries.Remove(missionId);
00842:             return true;
00843:         }
00844:
00845:         public VehicleGarageState CaptureState()
00846:         {
00847:             var s = new SystemTextJsonSerializer();
00848:             var json = s.Serialize(_state);
00849:             return s.Deserialize<VehicleGarageState>(json) ?? new VehicleGarageState();
00850:         }
00851:
00852:         public void RestoreState(VehicleGarageState? state)
00853:         {
00854:             if (state == null)
00855:             {
00856:                 _state = new VehicleGarageState();
00857:                 return;
00858:             }
00859:             var s = new SystemTextJsonSerializer();
00860:             var json = s.Serialize(state);
00861:             _state = s.Deserialize<VehicleGarageState>(json) ?? new VehicleGarageState();
00862:         }
00863:     }
00864: }
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **Current evidence and safe integration boundary for Plan 60: Vehicle Roster, Expedition Logistics and Garage Reachability.**.

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
