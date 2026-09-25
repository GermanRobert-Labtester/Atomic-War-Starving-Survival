# Plan 22 — Foundry, Greenhouse, Preservation, and Labor Decision Integration

> **Rebuild status:** CORE AUTHORITIES PRESENT — BOUNDED COMMISSION/FOOD-SECURITY INTEGRATION REMAINS
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

- The live foundry already owns heat, cast, treaty quotas, labor disputes, strikes, inventory ports, power demand and waste heat. The greenhouse already owns plots, growth, water, nutrients, blight and harvest. Preservation owns cohorts, curing jobs, temperature, power and spoilage.
- The remaining opportunity is a player-facing decision layer that compares current production, reserve and labor constraints without re-owning their values.
- The plan must also disambiguate this file from the separate greenhouse Plan 22 and the one-food authority Plan 22. All documentation and verification names the file path, never only the number.

**Bounded outcome:** Extend `SilentFoundrySystem`, `GreenhouseSystem`, `FoodPreservationSystem`, inventory, kitchen, rationing, duty roster and faction/treaty owners. Do not create `FoundryLaborDisputeSystem`, `HydroponicNutritionSystem`, a second crop model, or a second preservation store.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- The foundry catalog has 35 products; SilentFoundry has typed production, treaty, labor and safety events.
- The greenhouse catalog has 34 items and current plot state includes nutrient and crop-rotation fields.
- Food preservation has six tiers and three curing recipes, with temperature and power decay.
- Foundry labor dispute and treaty consequence state already exist inside SilentFoundrySystem partials.
- Greenhouse and foundry save through their existing session/store families.

**Master-authority sections applied to this rebase:**

- Volume 8 H-C1/H-C10
- Volume 17 C3/C4/C11 roadmaps
- Volume 20 decision briefs

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Add a read-only production/reserve decision projection across current owners.
- Expose commission priority as a host command over current catalog products, not a second product catalog.
- Connect harvest/preservation shortfalls to current food-security and needs owners.
- Keep labor disputes and faction consequences in SilentFoundrySystem; UI only presents them.

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
| production, heat, labor, treaty and safety | SilentFoundrySystem | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem*.cs` | Sole foundry production authority. |
| plots, crop growth, nutrients and blight | GreenhouseSystem | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` | Sole greenhouse growth authority. |
| cohorts, curing, freshness and spoilage | FoodPreservationSystem | `Assets/Ashfall.Core/Shelter/FoodPreservationSystem.cs` | Sole preservation cohort authority. |
| material bills, consumption and scarcity | Inventory/Kitchen/Rationing | `Assets/Ashfall.Core/Inventory/Inventory.cs; Assets/Ashfall.Core/KitchenNutritionSystem.cs; Assets/Ashfall.Core/Economy/ResourceRationingSystem.cs` | Downstream consumers own their mutations. |
| labor availability and political consequences | DutyRoster/Faction | `DutyRosterSystem; FactionWarSystem` | No foundry-local personnel registry. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Foundry, Greenhouse, Preservation, and Labor Decision Integration
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ SilentFoundrySystem
│   production, heat, labor, treaty and safety
│ GreenhouseSystem
│   plots, crop growth, nutrients and blight
│ FoodPreservationSystem
│   cohorts, curing, freshness and spoilage
│ Inventory/Kitchen/Rationing
│   material bills, consumption and scarcity
│ DutyRoster/Faction
│   labor availability and political consequences
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

1. **Preserve current state ownership.** SilentFoundrySystem owns production, heat, labor, treaty and safety: Sole foundry production authority.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| production, heat, labor, treaty and safety | SilentFoundrySystem | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem*.cs` | Sole foundry production authority. |
| plots, crop growth, nutrients and blight | GreenhouseSystem | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` | Sole greenhouse growth authority. |
| cohorts, curing, freshness and spoilage | FoodPreservationSystem | `Assets/Ashfall.Core/Shelter/FoodPreservationSystem.cs` | Sole preservation cohort authority. |
| material bills, consumption and scarcity | Inventory/Kitchen/Rationing | `Assets/Ashfall.Core/Inventory/Inventory.cs; Assets/Ashfall.Core/KitchenNutritionSystem.cs; Assets/Ashfall.Core/Economy/ResourceRationingSystem.cs` | Downstream consumers own their mutations. |
| labor availability and political consequences | DutyRoster/Faction | `DutyRosterSystem; FactionWarSystem` | No foundry-local personnel registry. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. select current product/crop/reserve need
2. preview owner-specific requirements
3. atomically bill inventory/resources
4. start current production/growth/curing job
5. daily owner tick
6. route output to canonical inventory/food systems
7. capture each owner state

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Foundry state owns active heat, workers, labor dispute, treaty and outputs.
- Greenhouse state owns each plot and harvest totals.
- Preservation state owns cohorts and active curing jobs.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Commission selection never changes product definitions.
- Labor availability is read from duty roster/assignment owners.
- A harvest enters preservation/inventory through one path.
- Spoilage and food-security pressure reach needs through canonical owners.
- A strike changes current foundry state, not a duplicate labor ledger.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- No new product, crop or recipe rows in the integration tranche.
- Use foundry_treaty_consequences and current product/crop metadata.
- New commission presets require a consuming host surface.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use existing foundry, greenhouse and preservation saves.
- No aggregate production save section.
- Restore must not duplicate outputs or clear active jobs.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Foundry outcomes use injected seeded RNG.
- Greenhouse blight and preservation spoilage use their existing deterministic rolls.
- No wall-clock production schedules.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Foundry emits production, safety, labor and treaty events.
- Greenhouse emits planted/matured/harvest/blight/failure events.
- Preservation emits spoilage, cure and consumption events.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Foundry/SilentFoundryHostSession.cs
- src/UI/SilentFoundryPanel.cs
- src/UI/GreenhousePanel.cs
- src/UI/KitchenNutritionPanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Labor and faction disputes use foundry treaty prose and current standing; no second political simulation.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | Commission preview and execution use different bills. | SilentFoundrySystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A labor assignment is consumed twice. | GreenhouseSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | Greenhouse growth and preservation both mutate the same crop item. | FoodPreservationSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | Food scarcity changes morale outside NeedsSystem. | Inventory/Kitchen/Rationing | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A strike blocks production outside foundry state. | DutyRoster/Faction | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/SilentFoundrySystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/SilentFoundryConsequenceTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/GreenhouseSystemTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/FoodPreservationSystemTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/Production/Plan87_91RelicGreenhouseIntegrationTests.cs`
6. `godot --headless --path . -- --data-integrity-selftest` when a production/product/recipe reference changes.

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — disambiguation and census | Name this Plan 22 by file and inventory all current rows/state. | No numbering collision remains in the plan. | No production path until the owning implementation package is separately claimed. |
| 1 — foundry commission seam | Design a host command using current SilentFoundry APIs. | No new product or labor authority. | No production path until the owning implementation package is separately claimed. |
| 2 — food-reserve seam | Join greenhouse, preservation, kitchen and rationing read models. | No duplicate consumption. | No production path until the owning implementation package is separately claimed. |
| 3 — labor consequence trace | Verify duty assignment and faction consequences. | Exactly-once mutation. | No production path until the owning implementation package is separately claimed. |
| 4 — player decision surface | Expose priorities and trade-offs in existing panels. | Truthful, accessible and reversible. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs | READ; MODIFY only through a foundry claim | Foundry owner |
| Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs | READ; MODIFY only for proven integration gap | Greenhouse owner |
| Assets/Ashfall.Core/Shelter/FoodPreservationSystem.cs | READ | Preservation owner |
| src/UI/SilentFoundryPanel.cs | READ; MODIFY only for truthful commission surface | Presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Plan-number collision. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Second product/crop/labor authority. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Double inventory billing. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Combined board becoming a gameplay owner. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Treating foundry rows as recipes. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No foundry/crop/recipe expansion.
- No new labor union system.
- No direct faction-standing mutation from UI.
- No production tuning without harness evidence.

# 23. Rollback and Recovery

- Host command/UI changes are isolated.
- Any state extension uses current owner version/migration.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- File identity is unambiguous.
- Current owners and row counts are explicit.
- Commission and reserve flows use current commands.
- No duplicate production/labor state is proposed.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Add a read-only production/reserve decision projection across current owners.
- Expose commission priority as a host command over current catalog products, not a second product catalog.
- Connect harvest/preservation shortfalls to current food-security and needs owners.
- Keep labor disputes and faction consequences in SilentFoundrySystem; UI only presents them.

## MUST NOT DO

- No foundry/crop/recipe expansion.
- No new labor union system.
- No direct faction-standing mutation from UI.
- No production tuning without harness evidence.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/SilentFoundrySystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/SilentFoundryConsequenceTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/GreenhouseSystemTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/FoodPreservationSystemTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/Production/Plan87_91RelicGreenhouseIntegrationTests.cs`
6. `godot --headless --path . -- --data-integrity-selftest` when a production/product/recipe reference changes.

## FIRST SAFE IMPLEMENTATION STEP

0 — disambiguation and census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: production, heat, labor, treaty and safety → SilentFoundrySystem; plots, crop growth, nutrients and blight → GreenhouseSystem; cohorts, curing, freshness and spoilage → FoodPreservationSystem; material bills, consumption and scarcity → Inventory/Kitchen/Rationing; labor availability and political consequences → DutyRoster/Faction. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 22.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 22 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by SilentFoundrySystem or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 689 lines / 34425 bytes.
- SHA-256: `0f779f80dada6c90e5cb5bfe305b5a9cfe253a6d12036b85044329a3ffff2caa`.
- Architecture signals: seeded references=6; save/restore symbols=0; typed event declarations=20; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=1; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed partial class SilentFoundrySystem
public const int DefaultSeed = 1009;
public const int MaxWorkers = 8;            // room_bp_11 max_dweller_capacity
public const float BlueprintBasePowerKw = 45f;
public const float BlueprintWaterFlowLpm = 40f;
public const string EventUnlocked = "silent_foundry_unlocked";
public const string EventRepairStarted = "silent_foundry_repair_started";
public const string EventRepaired = "silent_foundry_repaired";
public const string EventMaintenanceDue = "silent_foundry_maintenance_due";
public const string EventHeatPrepared = "silent_foundry_heat_prepared";
public const string EventHeatStarted = "silent_foundry_heat_started";
public const string EventHeatCompleted = "silent_foundry_heat_completed";
public const string EventCastCompleted = "silent_foundry_cast_completed";
public const string EventCastFailed = "silent_foundry_cast_failed";
public const string EventSafetyWarning = "silent_foundry_safety_warning";
public const string EventIncident = "silent_foundry_incident";
public const string EventTreatyQuotaMet = "silent_foundry_treaty_quota_met";
public const string EventTreatyQuotaMissed = "silent_foundry_treaty_quota_missed";
public const string EventConsequenceApplied = "silent_foundry_treaty_consequence_applied";
public const string EventLaborDispute = "silent_foundry_labor_dispute";
public const string EventStrikeStarted = "silent_foundry_strike_started";
public const string EventStrikeResolved = "silent_foundry_strike_resolved";
public const string EventBlueprintReferenced = "silent_foundry_blueprint_referenced";
public const string EventJournalTriggered = "silent_foundry_journal_triggered";
public event Action<SilentFoundryState> OnStateChanged;
public event Action<FoundryProductionRecord> OnProductionCompleted;
public event Action<FoundryFailedCastRecord> OnCastFailed;
public event Action<string> OnSafetyWarning;
public event Action<FoundryIncidentRecord> OnIncident;
public event Action<FoundryTreatyCompliance> OnTreatyQuotaMet;
public event Action<FoundryTreatyCompliance> OnTreatyQuotaMissed;
public event Action<FoundryConsequenceRecord> OnConsequenceApplied;
public event Action<FoundryLaborDispute, int> OnLaborDisputeChanged;
public event Action<FoundryStrikeResolution, int> OnStrikeResolved;
public event Action<FoundryJournalTrigger> OnJournalTriggered;
public event Action<string> OnEventRaised;
public const float StandingMin = -100f;
public const float StandingMax = 100f;
public const float StandingNeutral = 0f;
public void BindCatalog(SilentFoundryCatalog catalog, int maintenanceCycleDaysFromBlueprint) {
public void BindTreaties(IReadOnlyDictionary<string, int> ratificationDaysById) {
public void BindConsequencePolicy(SilentFoundryConsequencePolicyCatalog policy) {
public void BindInventory( Func<string, int> getCount, Func<string, int, bool> canAdd, Action<string, int> addItem, Action<string, int> consume) {
public SilentFoundryState State => _state;
public SilentFoundryCatalog Catalog => _catalog;
public bool IsUnlocked => _state.unlocked;
public FoundryHeatStage HeatStage => _state.heatStage;
public FoundryLaborDispute LaborDispute => _state.laborDispute;
public bool IsMaintenanceOverdue => _state.daysSinceMaintenance > _state.maintenanceCycleDays;
public int DaysOverdue => Math.Max(0, _state.daysSinceMaintenance - _state.maintenanceCycleDays);
public int OverdueCycles => _state.maintenanceCycleDays > 0
public float GetComponentCondition(FoundryFacilityComponent component) {
public float AverageFacilityCondition() {
public bool IsJournalTriggered(string templateId) =>
public FoundryTreatyCompliance? GetTreatyCompliance(string treatyId) {
public IReadOnlyList<FoundryProductionRecord> CompletedProduction => _state.completed;
public IReadOnlyList<FoundryFailedCastRecord> FailedCasts => _state.failed;
public IReadOnlyList<FoundryIncidentRecord> Incidents => _state.incidents;
public int TotalProductionCount => _state.completed.Count;
public int TotalFailedCount => _state.failed.Count;
public float CumulativeStress => _state.cumulativeStress;
public float CumulativeHope => _state.cumulativeHope;
public bool IsHeatActive => HeatStage != FoundryHeatStage.Idle && HeatStage != FoundryHeatStage.Complete;
public float CurrentPowerDemandKw => HeatStage switch
public float CurrentWasteHeatKw => HeatStage switch
public void SuspendHeat(string reason, int day) {
public float GuildStanding => _consequenceState.guildStanding;
public IReadOnlyList<FoundryConsequenceRecord> AppliedConsequences => _consequenceState.applied;
public bool IsConsequenceApplied(string treatyId, int cycleMarker) => _consequenceState.IsApplied(treatyId, cycleMarker);
public FoundryTreatyOutcome GetTreatyOutcome(string treatyId, int day) {
public bool Unlock(int day) {
public string StartRepair(FoundryFacilityComponent component, int day) {
public string PerformMaintenance(int day) {
public string PrepareSand(int waterLitres) {
public string CompactMold(float skill) {
public void AssessTreatyCompliance(int day) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.TreatyLabor.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.TreatyLabor.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 178 lines / 9066 bytes.
- SHA-256: `a8794132ecd09dd57853a547aeef2ded060d38958993d6d597927025e1e6a5f9`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=1; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed partial class SilentFoundrySystem
public static bool TryGetJournalDeltas(string templateId, out float stressDelta, out float hopeEarned) {
public SilentFoundryState CaptureState() {
public SilentFoundryConsequenceState CaptureConsequenceState() {
public void RestoreConsequenceState(SilentFoundryConsequenceState save) {
public void RestoreState(SilentFoundryState save) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs`

### `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 559 lines / 24040 bytes.
- SHA-256: `b51fcdb9157772ce9ca0a444ab8c3130ba0e5e043b39f68be29461d98a2cf05e`.
- Architecture signals: seeded references=1; save/restore symbols=2; typed event declarations=13; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum GreenhouseStage
public class GreenhousePlotState
public int plotIndex;
public string seedItemId;
public int stage;
public float growth;
public float water;
public float soilContamination;
public float blight;
public int plantedDay;
public float nutrientLevel;
public int sameCropStreak;
public string lastCropId = string.Empty;
public class GreenhouseState
public string saveId = GreenhouseExpansionCatalog.SaveId;
public List<GreenhousePlotState> plots = new List<GreenhousePlotState>();
public bool preWarWheatUnlocked;
public int totalHarvests;
public long blightRollCount;
public ApicultureState? apiculture;
public struct GreenhouseHarvest
public bool success;
public int plotIndex;
public string yieldItemId;
public int amount;
public bool contaminated;
public struct BlightRiskProfile
public int PlotIndex;
public bool PlotExists;
public float BaseChancePerDay;
public float ResistanceFactor;
public float ContaminationPressure;
public float DroughtStress;
public float NutrientReduction;
public float RotationPressure;
public int RotationStreak;
public float NutrientLevel;
public float FinalChancePerDay;
public class GreenhouseSystem
public const float MaxWater = 100f;
public const float MaxContamination = 100f;
public const float GrowingThreshold = 33f;
public const float DroughtBlightRatePerDay = 0.25f;
public const float OutbreakBlightStep = 0.3f;
public const float BaseBlightChancePerDay = 0.06f;
public const float TaintedWaterContaminationPerUnit = 1.5f;
public const float ResidualContaminationAfterHarvest = 0.5f;
public const string NutrientItemId = "item_hydroponic_nutrients";
public const float NutrientApplicationLevel = 0.5f;
public const float NutrientDecayPerDay = 0.1f;
public const float NutrientBlightRiskReduction = 0.04f;
public const float NutrientFullBandLevel = 0.5f;
public const float RotationBlightStepPerStreak = 0.015f;
public const int MaxRotationStreakCount = 10;
public string SaveId => _state.saveId;
public GreenhouseState CaptureState() {
public void RestoreState(GreenhouseState gs) {
public event Action<int, string, int> OnCropPlanted;
public event Action<int, string> OnCropMatured;
public event Action<GreenhouseHarvest> OnCropHarvested;
public event Action<int> OnBlightOutbreak;
public event Action<int> OnPlotDriedOut;
public event Action<int> OnCropFailed;
public GreenhouseState State => _state;
public int PlotCount => _state.plots.Count;
public int TotalHarvests => _state.totalHarvests;
public bool IsPreWarWheatUnlocked => _state.preWarWheatUnlocked;
public IReadOnlyList<GreenhousePlotState> Plots => _state.plots;
public void EnsurePlots(int planterBoxCount) {
public static bool IsFallow(GreenhousePlotState p) =>
public bool Plant(int plotIndex, string seedItemId, int currentDay, out string consumedSeedId) {
public bool Water(int plotIndex, float waterUnits, bool tainted) {
public bool ApplyNutrients(int plotIndex, out string consumedItemId) {
public BlightRiskProfile GetBlightRiskProfile(int plotIndex, bool hasWater) {
public GreenhouseHarvest Harvest(int plotIndex) {
public bool Clear(int plotIndex) {
public CommandPreview PreviewTreatBlight(int plotIndex, long stateVersion = 0) {
public CommandResult ExecuteTreatBlight(int plotIndex, long expectedStateVersion = 0, long currentStateVersion = 0) {
public bool TreatBlight(int plotIndex, out string consumedTreatmentId) {
public void SurgeContamination(float amount) {
public void UnlockPreWarWheat() {
public void TickDay(int currentDay, float growLightHours, float ashContaminationRate) {
```


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/Shelter/FoodPreservationSystem.cs`

### `Assets/Ashfall.Core/Shelter/FoodPreservationSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 326 lines / 12639 bytes.
- SHA-256: `7a8392e5a96ab4af5e738dff8d41c41b01a95a18cf97fad2921d7a4ee60b566e`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FoodBatchCohort
public string CohortId { get; set; } = string.Empty;
public string FoodItemId { get; set; } = string.Empty;
public int Quantity { get; set; }
public string TierId { get; set; } = "preservation_ambient";
public int DayStored { get; set; }
public float FreshnessPercent { get; set; } = 100f;
public bool IsSpoiled { get; set; }
public sealed class ActiveCuringJob
public string JobId { get; set; } = string.Empty;
public string RecipeId { get; set; } = string.Empty;
public string AssignedCookId { get; set; } = string.Empty;
public int DayStarted { get; set; }
public int ProgressTicks { get; set; }
public int TargetTicks { get; set; } = 4;
public bool IsComplete { get; set; }
public sealed class FoodPreservationState
public string SystemId { get; set; } = FoodPreservationSystem.SystemId;
public List<FoodBatchCohort> Cohorts { get; set; } = new List<FoodBatchCohort>();
public List<ActiveCuringJob> ActiveJobs { get; set; } = new List<ActiveCuringJob>();
public bool IsPowerOnline { get; set; } = true;
public int UnpoweredDays { get; set; }
public int TotalSpoiledDiscarded { get; set; }
public int TotalCured { get; set; }
public int TotalConsumed { get; set; }
public int NextCohortSeq { get; set; }
public sealed class FoodPreservationSystem
public const string SystemId = "food_preservation";
public const string DefaultStorageRoomId = "room_storage_bay";
public const float DefaultStorageTemperatureC = 10f;
public FoodPreservationState State => _state;
public bool IsPowerOnline => _state.IsPowerOnline;
public int UnpoweredDays => _state.UnpoweredDays;
public float StorageTemperatureC => _storageTemperatureC;
public event Action<FoodBatchCohort>? OnFoodSpoiled;
public event Action<ActiveCuringJob>? OnCuringCompleted;
public event Action<string, int, bool>? OnFoodConsumed;
public void SetPowerStatus(bool isOnline) {
public void SetStorageTemperatureC(float temperatureC) {
public static float StorageTempShelfLifeFactor(float temperatureC) {
public ActionResult AddCohort(string foodItemId, int quantity, string tierId, int currentDay) {
public ActionResult StartCuringJob(string recipeId, string cookId, int currentDay) {
public void TickDay(int day) {
public int ConsumeFood(string foodItemId, int neededCount, out int spoiledConsumed) {
public int DiscardSpoiled(string? foodItemId = null) {
public int GetTotalFood(string? foodItemId = null) {
public int GetSpoiledFood(string? foodItemId = null) {
public FoodPreservationState CaptureState() {
public void RestoreState(FoodPreservationState saved) {
```


# Appendix B.06 — Current Code Architecture: `Assets/Ashfall.Core/Economy/ResourceRationingSystem.cs`

### `Assets/Ashfall.Core/Economy/ResourceRationingSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 630 lines / 24091 bytes.
- SHA-256: `0fe19392476c433c5ae0e6a3e47dd136ffc1039e4c28a0e3dc84d75447be3752`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=10; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RationingProtocolDefinition
public string Id { get; set; } = string.Empty;
public string Name { get; set; } = string.Empty;
public string ProtocolType { get; set; } = "standard";
public string DefaultTier { get; set; } = "full";
public float MoralePenaltyScale { get; set; } = 0f;
public string Description { get; set; } = string.Empty;
public RationingTier ParseDefaultTier() => DefaultTier?.ToLowerInvariant() switch
public sealed class RationingProtocolCatalogData
public int SchemaVersion { get; set; } = 1;
public List<RationingProtocolDefinition> Protocols { get; set; } = new List<RationingProtocolDefinition>();
public enum RationingTier
public enum ResourceCrisisType
public enum PriorityGroupTier
public sealed class RationTarget
public string ResourceId { get; set; } = string.Empty;
public RationingTier Tier { get; set; } = RationingTier.Full;
public float BaseMultiplier { get; set; } = 1.0f;
public sealed class SurvivorRationAssignment
public string SurvivorId { get; set; } = string.Empty;
public PriorityGroupTier Priority { get; set; } = PriorityGroupTier.Standard;
public float PriorityBonus { get; set; } = 1.0f;
public sealed class ResourceCrisis
public string CrisisId { get; set; } = string.Empty;
public ResourceCrisisType Type { get; set; } = ResourceCrisisType.FoodShortage;
public int DeclaredDay { get; set; } = 1;
public bool IsResolved { get; set; } = false;
public string Severity { get; set; } = "moderate";
public string Notes { get; set; } = string.Empty;
public sealed class RationingEvent
public string EventId { get; set; } = string.Empty;
public string EventType { get; set; } = string.Empty;
public int Day { get; set; } = 1;
public string ResourceId { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public float MoraleImpact { get; set; } = 0f;
public sealed class ResourceAllocationRequest
public string ResourceId { get; set; } = string.Empty;
public string ConsumerId { get; set; } = string.Empty;
public int DemandUnits { get; set; }
public int AvailableUnits { get; set; }
public int CurrentDay { get; set; }
public sealed class ResourceAllocationDecision
public bool Authorized { get; set; }
public string ResourceId { get; set; } = string.Empty;
public string ConsumerId { get; set; } = string.Empty;
public int RequestedUnits { get; set; }
public int AllocatedUnits { get; set; }
public int AvailableUnits { get; set; }
public float AppliedMultiplier { get; set; } = 1f;
public string Reason { get; set; } = string.Empty;
public sealed class ResourceRationingState
public int SchemaVersion { get; set; } = 1;
public int NextSequence { get; set; } = 1;
public string ActiveProtocolId { get; set; } = "protocol_standard_distribution";
public List<RationTarget> Targets { get; set; } = new List<RationTarget>();
public List<SurvivorRationAssignment> Assignments { get; set; } = new List<SurvivorRationAssignment>();
public List<ResourceCrisis> Crises { get; set; } = new List<ResourceCrisis>();
public List<RationingEvent> Events { get; set; } = new List<RationingEvent>();
public sealed class ResourceRationingSystem
public event Action<RationTarget>? OnRationingTierChanged;
public event Action<ResourceCrisis>? OnCrisisDeclared;
public event Action<ResourceCrisis>? OnCrisisResolved;
public event Action<ResourceAllocationDecision>? OnAllocationAuthorized;
public event Action<string>? OnProtocolApplied;
public int ActiveCrisesCount => _state.Crises.Count(c => !c.IsResolved);
public int TargetCount => _state.Targets.Count;
public string ActiveProtocolId => _state.ActiveProtocolId;
public RationingProtocolDefinition? ActiveProtocol =>
public IReadOnlyCollection<RationingProtocolDefinition> Protocols => _protocols.Values;
public void LoadCatalog(string json) {
public void LoadCatalog(RationingProtocolCatalogData catalog) {
public RationingProtocolDefinition? GetProtocol(string protocolId) {
public bool ApplyProtocol(string protocolId, IEnumerable<string> resourceIds, int currentDay = 1) {
public void BindResourceValidator(Func<string, bool>? validator) {
public static float GetTierBaseMultiplier(RationingTier tier) => tier switch
public static float GetPriorityBonus(PriorityGroupTier priority) => priority switch
public RationTarget SetRationTier(string resourceId, RationingTier tier, int currentDay) {
public RationingTier GetRationTier(string resourceId) {
public float GetRationMultiplier(string resourceId) => GetAllocationMultiplier(resourceId);
public void AssignSurvivorPriority(string survivorId, PriorityGroupTier priority) {
public PriorityGroupTier GetSurvivorPriority(string survivorId) {
public float GetAllocationMultiplier(string resourceId, string? survivorId = null) {
public ResourceAllocationDecision AuthorizeAllocation(ResourceAllocationRequest request) {
public ResourceCrisis DeclareCrisis(ResourceCrisisType type, string severity, int currentDay, string notes = "") {
public bool ResolveCrisis(string crisisId, int currentDay) {
public float CalculateMoraleImpact(string survivorId) {
public ResourceRationingState CaptureState() {
public void RestoreState(ResourceRationingState state) {
```


# Appendix B.07 — Current Code Architecture: `src/Foundry/SilentFoundryHostSession.cs`

### `src/Foundry/SilentFoundryHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 650 lines / 31351 bytes.
- SHA-256: `bbbed72dc2e0ebf48a735ed8f9f561cd61fd27bc7635edfd15ade6c688e89299`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=1; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class SilentFoundryHostSession
public const int DefaultSeed = 1009;
public SilentFoundrySystem Engine { get; }
public SaltMineExtractionSystem SaltMine { get; }
public SilentFoundryCatalog Catalog { get; }
public ItemCatalog FoundryItems { get; }
public SilentFoundryConsequencePolicyCatalog ConsequencePolicy { get; }
public FactionStanceEngine GuildStanceEngine { get; }
public Ashfall.Core.Shelter.PowerGridSystem? PowerGrid { get; set; }
public Ashfall.Core.ShelterThermalSystem? ThermalSystem { get; set; }
public string LastEvent { get; set; } = string.Empty;
public event Action? StateChanged;
public void BindPowerAndThermal(Ashfall.Core.Shelter.PowerGridSystem? powerGrid, Ashfall.Core.ShelterThermalSystem? thermal) {
public string BeginForging(string outputItemId, int day) {
public string SubmitForgingCommand(Ashfall.Core.Foundry.FoundryForgingCommand command, int day) {
public string CompleteForging(int day) {
public void TickDaily(int day) {
public static SilentFoundryHostSession Create( string dataDir, ExpansionHostSession expansions, InventoryHostSession inventory, JournalSystem? journal = null, MarketSystem? market = null,
public float GuildTrust => Engine.GuildStanding;
public TradeStance GuildStance => GuildStanceEngine.GetStance(SilentFoundryIds.FactionId);
public void SyncGuildStanding() {
public void BindStanceProviders(Func<int> campaignDayProvider, Func<float> partyRadiationProvider, Func<SurvivorsHostSession?> survivorsProvider) {
public string Id { get; }
public string DisplayName { get; }
public RiskBiasTrait RiskBias => RiskBiasTrait.Realist;
public string Unlock(int day) => Engine.Unlock(day) ? "The Silent Foundry is open." : "Already open.";
public string Repair(FoundryFacilityComponent component, int day) => Engine.StartRepair(component, day);
public string Maintain(int day) => Engine.PerformMaintenance(day);
public string PrepareSand(int water) => Engine.PrepareSand(water);
public string CompactMold() => Engine.CompactMold(0.6f);
public string StartHeat(string productId, int workers, float skill, int day) {
public string Tap(int day) => Engine.TapAndCast(day);
public string SetOvertime(bool on) { Engine.SetOvertime(on); return on ? "Overtime ordered." : "Overtime rescinded."; }
public string SetChildLabor(bool on) { Engine.SetChildLaborUsed(on); return on ? "Children sent to the charging floor." : "Children returned to lessons."; }
public string OpenDispute(int day) => Engine.BeginLaborDispute(day);
public string ResolveStrike(FoundryStrikeResolution resolution, int day) => Engine.ResolveStrike(resolution, day);
public string OpenSaltMine(string veinId = "vein_salt_01", string displayName = "Main Salt Vein", int initialWorkers = 2) {
public string OpenSaltMineDemo() => OpenSaltMine();
public string TickSaltMine(int day) {
public string TickSaltMineDemo(int day) => TickSaltMine(day);
public string DeliverSaltTreaty(int day) {
public string DeliverSaltTreatyDemo(int day) => DeliverSaltTreaty(day);
public string SaltMineStatusLine() {
public string StatusLine() {
public sealed class FoundryItemJson
public string? id;
public string? displayName;
public string? description;
public string? type;
public int stackMax = 1;
public float weight;
public float tradeValue;
public float durability;
```


# Appendix B.08 — Current Code Architecture: `src/UI/SilentFoundryPanel.cs`

### `src/UI/SilentFoundryPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 589 lines / 27918 bytes.
- SHA-256: `84d462c5bad418e6959814aab73cd019e88dc668a9fffc1b1a574dcf803e78ff`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=4; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class SilentFoundryPanel : Control, IBindablePanel
public event Action? OnClose;
public event Action<int>? OnProductSelected;
public bool IsBound => _host != null;
public void Bind(SilentFoundryHostSession session, int currentDay) {
public void SetMachineTellCatalog(Ashfall.Core.Shelter.ShelterMachineTellCatalog? catalog) {
public override void _Ready() {
public void RefreshView() {
internal static List<AshfallDataGrid.Row> BuildFixtureRows() {
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix B.09 — Current Code Architecture: `src/UI/GreenhousePanel.cs`

### `src/UI/GreenhousePanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 805 lines / 43610 bytes.
- SHA-256: `7040021a886005a8e2200d671a7f54fa3cc569738e3e62e5418ed355ec418db4`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=17; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class GreenhousePanel : Control
public event Action? OnClose;
public event Action<int>? OnPlotSelected;
public bool IsBound => _host != null;
public void Bind(GreenhouseHostSession session) {
public override void _Ready() {
public void RefreshView() {
public event Action<string, int>? OnActionRequested;
internal static List<AshfallDataGrid.Row> BuildFixtureRows() {
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix B.10 — Current Code Architecture: `src/UI/KitchenNutritionPanel.cs`

### `src/UI/KitchenNutritionPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 394 lines / 20090 bytes.
- SHA-256: `ef6996fed6ec3e8bf54c92e9567ab1091435eee7c89a1b63c9941fe03789e4d9`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=3; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class KitchenNutritionPanel : Control, IBindablePanel
public event Action? OnClose;
public Func<string?>? DefaultSurvivorResolver { get; set; }
public Func<IReadOnlyList<string>>? LivingSurvivorsResolver { get; set; }
public string id = string.Empty;
public string name = string.Empty;
public string desc = string.Empty;
public string cost = string.Empty;
public int morale;
public Dictionary<string, int> inputs = new Dictionary<string, int>();
public bool IsBound => _host != null;
public void Bind(KitchenNutritionHostSession session) {
public void Unbind() {
public override void _Ready() {
public void Open() {
public void RefreshView() {
public override void _UnhandledInput(InputEvent @event) {
public override void _ExitTree() {
```


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/foundry_production.json`

### `Assets/StreamingAssets/Data/foundry_production.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 32750 bytes / 32750 characters.
- SHA-256: `233635e5809f364aa76256867d922018551a8eeaae9fc6487470133f1f32c836`.
- Root keys: `collection_id`, `products`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
products: min=35, max=35, observed_paths=1
products[].ingredients: min=2, max=2, observed_paths=2
products[].tags: min=4, max=4, observed_paths=2
```

Representative record fields:

- `cast_hours`
- `category`
- `display_name`
- `fuel_units`
- `ingredients`
- `labor_hours`
- `notes`
- `product_id`
- `quality_target`
- `quota_amount`
- `result_amount`
- `result_item_id`
- `sink`
- `skill_target`
- `tags`
- `treaty_id`
- `water_litres`


# Appendix C.12 — Catalog Census: `Assets/StreamingAssets/Data/greenhouse_items.json`

### `Assets/StreamingAssets/Data/greenhouse_items.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 11933 bytes / 11921 characters.
- SHA-256: `c677db12b90d1e805cbaa785b20557fb90320a4f685a683b23a2a073672c644a`.
- Root keys: `items`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
items: min=34, max=34, observed_paths=1
```

Representative record fields:

- `contamination`
- `description`
- `displayName`
- `durability`
- `empShielded`
- `healthEffect`
- `hungerRestore`
- `id`
- `moraleEffect`
- `stackMax`
- `thirstRestore`
- `tradeValue`
- `type`
- `weight`

Representative identifiers (ordered, capped for readability):

```text
item_seed_mushroom
item_seed_tuber
item_seed_frost_pea
crop_frost_pea
item_seed_glacier_greens
crop_glacier_greens
item_seed_grain
item_seed_wheat
item_planter_box
item_grow_lamp
item_lead_glass_pane
item_blight_treatment
item_grow_medium
crop_mushroom
crop_tuber
crop_grain
crop_wheat
tainted_food
item_greenhouse_trowel
item_greenhouse_pruning_shears
item_greenhouse_watering_can
item_greenhouse_hand_cultivator
item_greenhouse_compost
item_greenhouse_ash_fertilizer
item_greenhouse_fish_emulsion
item_greenhouse_insecticidal_soap
item_greenhouse_sticky_traps
item_greenhouse_pest_mesh
item_greenhouse_drip_kit
item_greenhouse_line_filter
item_greenhouse_catchment_kit
item_greenhouse_glass_pane
item_greenhouse_uv_sheeting
item_greenhouse_shade_cloth
```


# Appendix C.13 — Catalog Census: `Assets/StreamingAssets/Data/food_preservation.json`

### `Assets/StreamingAssets/Data/food_preservation.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 5234 bytes / 5234 characters.
- SHA-256: `e0292b12456c974b5b57e76615f6a220cf2f400543fedbb9400e5d7f60c8ec82`.
- Root keys: `curing_recipes`, `food_type_by_item_id`, `preservation_tiers`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
curing_recipes: min=3, max=3, observed_paths=1
preservation_tiers: min=6, max=6, observed_paths=1
preservation_tiers[].allowed_food_types: min=3, max=4, observed_paths=2
```

Representative record fields:

- `allowed_food_types`
- `description`
- `display_name`
- `flavor_morale_bonus`
- `id`
- `power_draw_watts`
- `shelf_life_days`
- `shelf_life_multiplier`
- `spoilage_toxin_risk`

Representative identifiers (ordered, capped for readability):

```text
preservation_ambient
preservation_root_cellar
preservation_salt_cured
preservation_smokehouse
preservation_fermented
preservation_cryogenic
```


# Appendix C.14 — Catalog Census: `Assets/StreamingAssets/Data/foundry_treaty_consequences.json`

### `Assets/StreamingAssets/Data/foundry_treaty_consequences.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 10615 bytes / 10615 characters.
- SHA-256: `8f9a0cffa4e257f13fa06c962c3bad29bf716f93a12e78b08e8da7f074861f16`.
- Root keys: `collection_id`, `policies`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
policies: min=15, max=15, observed_paths=1
policies[].market_modifiers: min=2, max=2, observed_paths=2
```

Representative record fields:

- `faction_id`
- `market_modifiers`
- `outcome`
- `reason`
- `standing_delta`
- `treaty_id`


# Appendix D.15 — Existing Focused Test Inventory: `Ashfall.Core.Tests/SilentFoundrySystemTests.cs`

### `Ashfall.Core.Tests/SilentFoundrySystemTests.cs`

- Current test declarations: Fact=35, Theory=0, InlineData=0.
- File lines: 916; SHA-256: `4f3c37bec6f3232e20309bddda99525206c3c11d3d34f775792865f5118adfd8`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Identity_ExactIdsResolve
Catalog_LoadsAllAuthoredProductsAndFaction
Catalog_QuotaProductsMapToExactTreaties
JournalTemplates_LoadWithExpansionIsolation
JournalDeltas_MatchAuthoredTemplate
Blueprint_ResolvesAndAnchorsMaintenanceCycle
Treaties_GuildIsExactSignatoryOfTenFoundryTreaties
Unlock_IsIdempotentAndRaisesOnce
Repair_ConsumesFirebrickAndRestoresComponent
Maintenance_FourDayCycleAndOverdueConsequences
SandPrep_ConsumesSandAndWaterAndImprovesBed
SandPrep_BlocksWithoutSandOrWater
Production_StartValidatesChargeAndConsumesResources
Production_MissingChargeGivesVisibleReason
Production_FirstHeatCompletesAndTriggersJournalOnce
Production_SecondHeatDoesNotRetriggerJournal
Production_UntappedHeatBurnsOutAndRecordsFailure
Production_QualityTiersAreDeterministicPerSeed
Safety_WarningsSurfaceBeforeIrreversibleTap
Incident_SameSeedSameOutcome
Incident_IsNeverHiddenAndLeavesARecord
Incident_WellMaintainedFurnaceNeverIncidents
Treaty_RailQuotaMetOnDeadline
Treaty_RailQuotaMissedOnDeadline
Treaty_LaborShiftViolationWhenStrikeOrOvertime
Treaty_RatificationDaysAreNotAssessedBeforeRatification
Strike_FatigueAloneDoesNotTriggerDispute
Strike_ProductionPressurePlusShiftGrievanceTriggersAndEscalates
Strike_ResolutionIsTypedAndOnceOnly
Save_RoundTripPreservesAllFoundryState
Save_ActiveFurnaceSurvivesRoundTrip
Save_MissingFoundryStateDefaultsSafely
Save_ChecksumStableAcrossHostSerializers
Save_ExpansionHubEnvelopeRoundTripsWithMigration
Events_EmitExactlyOncePerOutcome
```


# Appendix D.16 — Existing Focused Test Inventory: `Ashfall.Core.Tests/SilentFoundryConsequenceTests.cs`

### `Ashfall.Core.Tests/SilentFoundryConsequenceTests.cs`

- Current test declarations: Fact=27, Theory=0, InlineData=0.
- File lines: 651; SHA-256: `3ea7d13b6148ed9794f643f58bfa74aab8c8ecbcc166b198e0f1bcdaddd30f30`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Identity_ExactGuildFactionIsPreserved
Policy_ExactMappingsLoadAndResolve
Policy_MarketGoodsResolveInTheActiveEconomyCatalog
Treaty05_MissedAppliesStandingAndMarketConsequences
Treaty05_MetRestoresStanding
Treaty12_MissedAppliesRailAndCoalLogisticsConsequences
Treaty12_MetCarriesNoMarketPenalty
Treaty10_CompliantLaborPreservesAccess
Treaty10_GenuineViolationPenalizesStandingAndFuel
Treaty10_FatigueAloneIsNeverAViolation
Outcome_PreRatificationIsNeutral
Treaty16_NoConsequenceIsEverApplied
Standing_AppliedExactlyOncePerCycle
Standing_NextCycleAppliesOnceMoreButDoesNotSnowballPastBounds
Standing_RestoreDoesNotReapplyOrStack
Standing_MissingStateDefaultsToNeutral
Market_ModifierRaisesDemandAndPriceOnTheRealMarket
Market_MissThenMetRestoresDemandOnTheRealMarket
Policy_MetReliefRowsMirrorTheMissPenalties
Standing_TradeFloorGatesTheStall
Market_PolicyModifierSetMatchesTheEconomySurface
Standing_NeverLeaksToTheFoundryUnion
Consequences_AreDeterministicGivenSameInputs
Save_RoundTripsConsequenceLedgerThroughTheHubEnvelope
Save_V2LegacyMigratesWithEmptyConsequenceLedger
Save_ChecksumStableAcrossSerializerRoundTrip
Data_AllPolicyReferencesResolveInAuthoritativeCatalogs
```


# Appendix D.17 — Existing Focused Test Inventory: `Ashfall.Core.Tests/GreenhouseSystemTests.cs`

### `Ashfall.Core.Tests/GreenhouseSystemTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 172; SHA-256: `cd6560460c6991afcefcd421e1643a6b5841181c3442280e7902f268d4ebac8e`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plant_Requires_FallowPlot_AndKnownSeed
WellTendedCrop_Matures_AndHarvests_Clean
TaintedIrrigation_ContaminatesHarvest
Drought_StallsGrowth_AndFiresDriedOut
PreWarWheat_RequiresUnlock
SaveState_RoundTrips_Losslessly
HeadlessDemo_PassesAllChecks
Probe_SameSeedSameSequence_TwoInstances
Probe_RestoreContinuesBlightStream
```


# Appendix D.18 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Shelter/FoodPreservationSystemTests.cs`

### `Ashfall.Core.Tests/Shelter/FoodPreservationSystemTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 254; SHA-256: `d34eb1cadaeaa925a8a95dc71a332b5ffaac3f5e8d1e3512b6971f01e5a1c25a`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
AddCohort_InitializesCorrectly
TickDay_DecaysFreshness_BasedOnTier
TickDay_CryogenicOutage_AcceleratesDecayAfterBuffer
TickDay_SpoilsCohort_WhenFreshnessReachesZero
StartCuringJob_ConsumesInventoryAndProducesFood
ConsumeFood_FIFOOrder_PrioritizesLowestFreshness
DiscardSpoiled_RemovesOnlySpoiledCohorts
SaveRestoreState_RoundtripsIdenticalState
```


# Appendix D.19 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Production/Plan87_91RelicGreenhouseIntegrationTests.cs`

### `Ashfall.Core.Tests/Production/Plan87_91RelicGreenhouseIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 167; SHA-256: `33a2c9b41cf087e8bfdeaf0cde0658d5651b7bc86f62a446a21c192e6e703928`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RelicRecipes_LoadsAllThirtyNineRecipesWithValidContracts
GreenhouseItems_LoadsAtLeastThirtyItemsWithValidCategories
CrossSystem_BothCatalogsLoadIndependentlyWithoutIdCollision
CrossSystem_RelicsAndGreenhouseReflectCivilizationRebuildingInfrastructure
```


# Appendix E.20 — Supporting Code Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

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


# Appendix E.21 — Supporting Code Evidence: `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.Heat.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.Heat.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 548 lines / 25964 bytes.
- SHA-256: `01dd24634d0735a7591acf9174c42c2382bf7d9c7bf8c374e937ebd752d9b7c0`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=7; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed partial class SilentFoundrySystem
public string StartProduction(string productId, int workers, float workerSkill, int day) {
public string TapAndCast(int day) {
public List<string> GetSafetyWarnings() {
public int ComputeIncidentChance() {
public void SetOvertime(bool overtime) { _state.overtimeFlag = overtime; RaiseStateChanged(); }
public void SetChildLaborUsed(bool used) { _state.childLaborUsed = used; RaiseStateChanged(); }
public string BeginLaborDispute(int day) {
public bool EscalateToStrike(int day) {
public string ResolveStrike(FoundryStrikeResolution resolution, int day) {
public void TickDaily(int day) {
public static FoundryQualityTier QualityTier(float quality) {
```


# Appendix E.22 — Supporting Code Evidence: `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.Glassworks.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.Glassworks.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 19 lines / 711 bytes.
- SHA-256: `3bf5639e932b64fb2b17a8825e81c15243f02ef9d5e57091efb8e96a857352d0`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed partial class SilentFoundrySystem
public void BindGlassworksCatalog(GlassworksCatalog catalog) {
```


# Appendix E.23 — Supporting Code Evidence: `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.Material.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.Material.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 332 lines / 15293 bytes.
- SHA-256: `056bec67748c50875071fa3b1338257989f4e41e0f4f53091b6f45dfd1f25bb3`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum FoundryPurityTier
public static class FoundryPurityNames
public const string Poor = "Poor";
public const string Standard = "Standard";
public const string High = "High";
public const string Exceptional = "Exceptional";
public static string Name(FoundryPurityTier tier) => tier switch
public enum FoundryForgingCommand
public sealed class FoundryForgingSessionState
public string productOutputItemId = string.Empty;   // the batch output being worked
public string materialProfileId = string.Empty;
public string purity = FoundryPurityNames.Standard;
public List<string> submitted = new List<string>();  // authored command names
public int startedDay = 0;
public bool completed = false;
public int finalQualityPermille = 0;
public readonly struct FoundryForgingResult
public readonly bool Accepted;
public readonly string ProductOutputItemId;
public readonly int FinalQualityPermille;
public readonly FoundryPurityTier Purity;
public readonly int MatchedCommands;
public readonly int ExpectedCommands;
public readonly string Reason;
public readonly struct FoundryMaterialQuality
public readonly string MaterialProfileId;
public readonly FoundryPurityTier Purity;
public readonly int DurabilityModifierBp;
public readonly int ArmorModifierBp;
public readonly int CorrosionModifierBp;
public sealed partial class SilentFoundrySystem
public void BindMaterialProfiles(MaterialProfileCatalog catalog, IReadOnlyDictionary<string, string> outputItemToMaterialId) {
public static FoundryPurityTier DerivePurityTier(float quality, float contamination, float slag) {
public bool TryGetLatestMaterialQualityAny(out FoundryMaterialQuality quality) {
public bool TryGetLatestMaterialQuality(string outputItemId, out FoundryMaterialQuality quality) {
public FoundryForgingSessionState? ActiveForging =>
public string BeginForging(string outputItemId, int day) {
public string SubmitForgingCommand(FoundryForgingCommand command, int day) {
public FoundryForgingResult CompleteForging(int day) {
public event Action<FoundryForgingResult>? OnForgingCompleted;
```


# Appendix E.24 — Supporting Code Evidence: `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.Metallurgy.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.Metallurgy.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 221 lines / 10158 bytes.
- SHA-256: `c625f6ec47f5e01a72e3d7b75083612d4ecf64b8723ae41e862fb1eec4c900ab`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed partial class SilentFoundrySystem
public void BindMetallurgyCatalog(MetallurgyHeavyCatalog catalog) {
public void BindVentilation(VentilationSystem ventilation) {
public float SlagLevel => MathfCompat.Clamp(_state.metallurgySlag, 0f, 100f);
public bool IsHeavyBatchActive => !string.IsNullOrEmpty(_state.activeMetallurgyRecipeId);
public string StartHeavyBatch(string recipeId, int workers, float workerSkill, int day) {
public string SkimSlag(int day) {
```


# Appendix G.25 — Supporting Regression Evidence: `Ashfall.Core.Tests/AgricultureSystemTests.cs`

### `Ashfall.Core.Tests/AgricultureSystemTests.cs`

- Current test declarations: Fact=23, Theory=0, InlineData=0.
- File lines: 601; SHA-256: `a112b69d340427be117411ed65b9bbde0763e5a98be3ff34b3d51f3409f4efdd`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ShippedCatalog_ValidatesWithZeroDiagnostics
ShippedCatalog_SeedItemsResolveInCanonicalCropCatalog
CatalogValidation_RejectsBadDefinitions
CatalogValidation_RejectsCompostValueLoop
Growth_IsDeterministicForSameSeedAndEnvironment
Growth_EndOfDayPhaseTransitionsAreExact
Power_LightingOutageHaltsGrowthAndCutsYield
Water_BandToxicityAccumulatesOncePerWatering
Toxicity_ReducesYieldWithinBounds
Yield_NeverNegativeNaNOrRunaway
Mutation_SameSeedSameStateSameOutcome
Mutation_IneligibleConditionsConsumeNoMutationRng
Mutation_ToxicHarvestMarksHarvestContaminated
Mutation_HardyStrainResolvesVariantAndUnlocksOnHarvest
Pest_ProgressionIsDeterministicAndSeverityStepsWithoutRng
Pest_TreatmentValidatesItemThenClears
Compost_LifecycleStartsWaitsAndCollectsOnce
Compost_ApplicationImprovesMediumAndReducesToxicity
FirstHarvest_NarrativeFiresExactlyOnce
BlightNarrative_FiresOncePerEpisode
TickDay_IsIdempotentPerDay
Nutrition_MonotonousDietBecomesDeficientAndDiversityClears
Nutrition_UnmappedFoodIsCaloriesOnly
```


# Appendix G.26 — Supporting Regression Evidence: `Ashfall.Core.Tests/Greenhouse/GreenhousePhase4LoopClosureTests.cs`

### `Ashfall.Core.Tests/Greenhouse/GreenhousePhase4LoopClosureTests.cs`

- Current test declarations: Fact=17, Theory=0, InlineData=0.
- File lines: 298; SHA-256: `daf2728f9b4af16d9d72001e686cf1c181c50d6fabcd9598eb26f2d1b6277937`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ApplyNutrients_RaisesBand_TowardSaturation
ApplyNutrients_Blocked_OnFallowAndFailedPlots
Nutrients_LowerBlightRisk_NeverNegative
NutrientBand_DecaysDaily_DosingIsRecurring
BlightRiskProfile_IsReadOnly_ConsumingNoRng
BlightRiskProfile_FinalChance_MatchesLegacyFormula
WinterLight_NonWinter_PassesThroughUnchanged
WinterLight_Powered_WithoutCapability_IsPenalized
WinterLight_WithCapability_AndPower_IsFullyCompensated
WinterLight_WithoutPower_IsZero_RegardlessOfCapability
NutrientLevel_SaveRoundTrips
LegacySave_WithoutNutrientLevel_RestoresZero
SaturatedNutrients_FiveDayCycle_BlightChanceClampsToZero
CropRotation_SameCropStreak_RaisesRisk_OtherCropResets
CropRotation_Pressure_IsVisible_InRiskProfile_AndClamped
CropRotation_StreakSurvivesHarvest_ButNotDifferentCrop
CropRotation_SaveRoundTrips_AndLegacyRestoresZero
```


# Appendix G.27 — Supporting Regression Evidence: `Ashfall.Core.Tests/Foundry/Plan213MetallurgyReconciliationTests.cs`

### `Ashfall.Core.Tests/Foundry/Plan213MetallurgyReconciliationTests.cs`

- Current test declarations: Fact=11, Theory=0, InlineData=0.
- File lines: 342; SHA-256: `69189acc581d895a004494f5f554fa3d44011c209caba776a695e76690d6ba89`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
NoDuplicateMetallurgyAuthority_ReflectionGate
Plan213_Extends_TheExistingFoundryState_NoNewSaveStore
RealMaterialCatalog_Loads_SixProfiles_NoErrors
MaterialCatalog_IsPropertiesOnly_RecipesStayInMetallurgyRecipes
PurityDerivation_IsBounded_Deterministic_AndSensible
HeavyBatch_Completes_WithPurityAndProvenanceStamps
ForgingSequence_IsDeterministicHeadlessCommandFlow
Forging_WrongSequence_ScoresLower_ButStaysBounded
Forging_Gates_AreHonest
SaveRestore_MidForging_PreservesSequenceExactly
OldItems_MigrateToDefaults_NeverRecalculated
```


# Appendix G.28 — Supporting Regression Evidence: `Ashfall.Core.Tests/GreenhouseItemCatalogTests.cs`

### `Ashfall.Core.Tests/GreenhouseItemCatalogTests.cs`

- Current test declarations: Fact=21, Theory=0, InlineData=0.
- File lines: 395; SHA-256: `8d0455261b65808c3cd8fc08c876df318761924dd0e99ccb67581bbf7881ee96`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
GreenhouseFile_ContainsExactlyThirtyEntries
GreenhouseFile_PreservesOriginalFourteen
GreenhouseFile_ContainsAllSixteenNewSupplies
GreenhouseFile_HasUniqueIds
GreenhouseFile_AllTypesAreValidItemTypeValues
GreenhouseFile_NamesAndDescriptionsNonEmpty
GreenhouseFile_NumericRangesValid
GreenhouseFile_HandToolsHaveLowStacksAndLowWeight
GreenhouseFile_NewSuppliesAreNotSameValueClones
GlobalCatalog_RegistersAllThirtyGreenhouseEntries
GlobalCatalog_NewSuppliesResolveAcrossCategories
GlobalCatalog_NoIdCollisionsAcrossItemFiles
GreenhouseFile_DeadParityCopiesRemoved
GreenhouseFile_NewSuppliesClaimNoConsumableEffectFields
Crafting_FourGreenhouseRecipesExistAndAreUnique
Crafting_GreenhouseRecipeOutputsResolveInGlobalRegistry
Crafting_GreenhouseRecipeIngredientsResolve
Crafting_GreenhouseOutputsNotPricedBelowInputValue
Scavenging_GreenhouseTableBindsThreePlan91Items
Scavenging_BoundItemIdsResolveInGlobalRegistry
Scavenging_BoundEntriesUseSaneWeightsAndRarity
```


# Appendix H.29 — Supporting Authority Document: `docs/plans/PLAN_22_GREENHOUSE_RUNTIME_ITEM_CONSUMPTION.md`

### `docs/plans/PLAN_22_GREENHOUSE_RUNTIME_ITEM_CONSUMPTION.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 3398 lines / 214336 bytes.
- SHA-256: `e6d70c8877031a78d01749eb5fe3e6a90c45efa283133f03f10c61a8b27bf740`.
- Architecture signals: seeded references=7; save/restore symbols=13; typed event declarations=0; textual Godot mentions=7; textual Unity/JsonUtility mentions=1; `System.Random`=1; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public float fertility;        // 0–100, default DefaultFertility (50)
public int   pestControlDays;   // 0 = none; ticks down once/day
public bool  dripInstalled;     // auto-irrigation enabled
public int   dripFilterUses;    // remaining auto-water events
public bool  catchmentInstalled;// auto-water item cost reduced
public float glazingCondition;  // 0–100, default 100
public int   shadeClothDays;    // ash-ingress damping, ticks down once/day
public float glazingCondition = MaxGlazingCondition; // absent ⇒ pristine
public const string Compost       = "item_greenhouse_compost";
public const string AshFertilizer = "item_greenhouse_ash_fertilizer";
public const string FishEmulsion  = "item_greenhouse_fish_emulsion";
public const string InsecticidalSoap = "item_greenhouse_insecticidal_soap";
public const string StickyTraps   = "item_greenhouse_sticky_traps";
public const string PestMesh      = "item_greenhouse_pest_mesh";
public const string DripKit       = "item_greenhouse_drip_kit";
public const string LineFilter    = "item_greenhouse_line_filter";
public const string CatchmentKit  = "item_greenhouse_catchment_kit";
public const string GlassPane     = "item_greenhouse_glass_pane";
public const string UvSheeting    = "item_greenhouse_uv_sheeting";
public const string ShadeCloth    = "item_greenhouse_shade_cloth";
public float fertility;                       // plot; sentinel-normalized
public int   pestControlDays;                 // greenhouse; Max(0,·)
public bool  dripInstalled;
public int   dripFilterUses;                  // Max(0,·)
public bool  catchmentInstalled;
public float glazingCondition = MaxGlazingCondition;  // initializer, NOT sentinel
public int   shadeClothDays;                  // Max(0,·)
public bool ApplyAmendment(int plotIndex, string amendmentItemId, out string consumedAmendmentId) {
public static float ComputeDailyBlightChance( CropDef def, float soilContamination, bool hasWater, float nutrientLevel, int sameCropStreak) {
public struct AutoIrrigationRequest
public int PlotIndex; public float WaterUnits; public int CleanWaterCost;
public List<AutoIrrigationRequest> ComputeAutoIrrigationRequests() {
public bool ExecuteAutoIrrigation(int plotIndex, float waterUnits) {
public bool AmendSoil(int plotIndex, string amendmentItemId) {
public void RefreshPlotCapacity() {
public static int GrowLightHoursFor(int lampCount) =>       // Core, pure
```


# Appendix H.30 — Supporting Authority Document: `docs/foundry/FOUNDRY_RESOURCE_ALLOCATION_MATRIX.md`

### `docs/foundry/FOUNDRY_RESOURCE_ALLOCATION_MATRIX.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 36 lines / 3137 bytes.
- SHA-256: `1256101a68febc6bb140b403f8abb4fe33daf1e42599c8f24914492adbe17a36`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix H.31 — Supporting Authority Document: `docs/production/PRESERVATION_RECIPE_MATRIX.md`

### `docs/production/PRESERVATION_RECIPE_MATRIX.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 30 lines / 2715 bytes.
- SHA-256: `a81025135548e62e37dc67b9f29c92b23bbc25026025d8bd7c1cbadd3fe5c80b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| production, heat, labor, treaty and safety | SilentFoundrySystem | plots, crop growth, nutrients and blight | GreenhouseSystem | Owner emits/reads a typed fact; no mirror state. |
| production, heat, labor, treaty and safety | SilentFoundrySystem | cohorts, curing, freshness and spoilage | FoodPreservationSystem | Owner emits/reads a typed fact; no mirror state. |
| production, heat, labor, treaty and safety | SilentFoundrySystem | material bills, consumption and scarcity | Inventory/Kitchen/Rationing | Owner emits/reads a typed fact; no mirror state. |
| production, heat, labor, treaty and safety | SilentFoundrySystem | labor availability and political consequences | DutyRoster/Faction | Owner emits/reads a typed fact; no mirror state. |
| plots, crop growth, nutrients and blight | GreenhouseSystem | production, heat, labor, treaty and safety | SilentFoundrySystem | Owner emits/reads a typed fact; no mirror state. |
| plots, crop growth, nutrients and blight | GreenhouseSystem | cohorts, curing, freshness and spoilage | FoodPreservationSystem | Owner emits/reads a typed fact; no mirror state. |
| plots, crop growth, nutrients and blight | GreenhouseSystem | material bills, consumption and scarcity | Inventory/Kitchen/Rationing | Owner emits/reads a typed fact; no mirror state. |
| plots, crop growth, nutrients and blight | GreenhouseSystem | labor availability and political consequences | DutyRoster/Faction | Owner emits/reads a typed fact; no mirror state. |
| cohorts, curing, freshness and spoilage | FoodPreservationSystem | production, heat, labor, treaty and safety | SilentFoundrySystem | Owner emits/reads a typed fact; no mirror state. |
| cohorts, curing, freshness and spoilage | FoodPreservationSystem | plots, crop growth, nutrients and blight | GreenhouseSystem | Owner emits/reads a typed fact; no mirror state. |
| cohorts, curing, freshness and spoilage | FoodPreservationSystem | material bills, consumption and scarcity | Inventory/Kitchen/Rationing | Owner emits/reads a typed fact; no mirror state. |
| cohorts, curing, freshness and spoilage | FoodPreservationSystem | labor availability and political consequences | DutyRoster/Faction | Owner emits/reads a typed fact; no mirror state. |
| material bills, consumption and scarcity | Inventory/Kitchen/Rationing | production, heat, labor, treaty and safety | SilentFoundrySystem | Owner emits/reads a typed fact; no mirror state. |
| material bills, consumption and scarcity | Inventory/Kitchen/Rationing | plots, crop growth, nutrients and blight | GreenhouseSystem | Owner emits/reads a typed fact; no mirror state. |
| material bills, consumption and scarcity | Inventory/Kitchen/Rationing | cohorts, curing, freshness and spoilage | FoodPreservationSystem | Owner emits/reads a typed fact; no mirror state. |
| material bills, consumption and scarcity | Inventory/Kitchen/Rationing | labor availability and political consequences | DutyRoster/Faction | Owner emits/reads a typed fact; no mirror state. |
| labor availability and political consequences | DutyRoster/Faction | production, heat, labor, treaty and safety | SilentFoundrySystem | Owner emits/reads a typed fact; no mirror state. |
| labor availability and political consequences | DutyRoster/Faction | plots, crop growth, nutrients and blight | GreenhouseSystem | Owner emits/reads a typed fact; no mirror state. |
| labor availability and political consequences | DutyRoster/Faction | cohorts, curing, freshness and spoilage | FoodPreservationSystem | Owner emits/reads a typed fact; no mirror state. |
| labor availability and political consequences | DutyRoster/Faction | material bills, consumption and scarcity | Inventory/Kitchen/Rationing | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Add a read-only production/reserve decision projection across current owners. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Expose commission priority as a host command over current catalog products, not a second product catalog. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Connect harvest/preservation shortfalls to current food-security and needs owners. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Keep labor disputes and faction consequences in SilentFoundrySystem; UI only presents them. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.563 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Farming/AgricultureSystem.cs`

### `Assets/Ashfall.Core/Farming/AgricultureSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 904 lines / 40109 bytes.
- SHA-256: `4d405ead4542d1f14c8b388b2e8d12c94587c8b5761e07176dad3fa293b486c8`.
- Architecture signals: seeded references=5; save/restore symbols=2; typed event declarations=18; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum AgriWaterBand
public enum AgriMutationOutcome
public struct AgricultureEnvironmentSnapshot
public float TemperaturePenaltyC;
public float OutdoorRadModifier;
public float LightingAvailabilityPermille;
public float AshContaminationRate;
public string SeasonWindowId;
public static AgricultureEnvironmentSnapshot Default() => new AgricultureEnvironmentSnapshot
public class AgriPlotState
public int plot_index;
public string strain_id = "";
public float medium_quality = 100f;
public int toxicity_permille;
public float pest_severity;
public string pest_id = "";
public int mutation_outcome;
public string mutation_strain_id = "";
public int planted_day;
public class CompostBatchState
public string recipe_id = "";
public int started_day;
public bool collected;
public class AgricultureState
public string system_id = "agriculture";
public int schema_version = 1;
public List<AgriPlotState> plots = new List<AgriPlotState>();
public List<CompostBatchState> compost_batches = new List<CompostBatchState>();
public bool first_harvest_narrative_fired;
public List<int> blight_narrative_plots = new List<int>();
public List<string> unlocked_strains = new List<string>();
public int last_tick_day;
public struct YieldBreakdown
public int BaseYield;
public float StrainModifier;
public float WaterModifier;
public float LightModifier;
public float ToxicityModifier;
public float PestModifier;
public float MutationModifier;
public float MediumModifier;
public float FinalMultiplier;
public int FinalYield;
public string QualityTier;
public bool Contaminated;
public struct AgricultureHarvest
public bool success;
public int plotIndex;
public string yieldItemId;
public int baseAmount;
public int finalAmount;
public bool contaminated;
public string qualityTier;
public YieldBreakdown Breakdown;
public NutritionProfileDef Nutrition;
public sealed class AgricultureSystem
public const float MediumDecayPerPlantedDay = 0.5f;
public const float MediumDecayPerFallowDay = 0.1f;
public const float ToxicityNaturalDecayPerDay = 2f;
public const float CompostMediumBoost = 40f;
public const int CompostToxicityReduction = 200;
public const float BaseGrowLightHours = 6f;
public const float WaterUnitsPerWatering = 50f;
public static readonly int[] ToxicityPerUnitByBand = { 0, 2, 8, 15 };
public static readonly string[] DeepWinterWindowIds = { "window_deep_freeze", "window_long_winter" };
public const int DeepWinterLightPermille = 600;
public static int WinterAdjustedLightPermille(int poweredLightPermille, string seasonWindowId, bool hasMicroclimateCapability, bool greenhouseRoomPowered) {
public string SystemId => _state.system_id;
public AgricultureState State => _state;
public GreenhouseSystem Greenhouse => _greenhouse;
public CropStrainCatalogContainer Catalog => _catalog;
public int LastTickDay => _state.last_tick_day;
public void LoadCatalog(CropStrainCatalogContainer catalog) {
public event Action<int, string, int> OnStrainPlanted;
public event Action<int, string> OnPlotInfested;
public event Action<int, string> OnPestTreated;
public event Action<int, AgriMutationOutcome, string> OnMutationDetermined;
public event Action<AgricultureHarvest> OnHarvest;
public event Action<string, int> OnCompostStarted;
public event Action<string, int> OnCompostCollected;
public event Action<int> OnFirstHarvest;
public event Action<int> OnBlightNarrative;
public AgricultureState CaptureState() {
public void RestoreState(AgricultureState state) {
public CropStrainDef Strain(string strainId) {
public CropStrainDef EffectiveStrain(int plotIndex) {
public PestDef Pest(string pestId) {
public CompostRecipeDef CompostRecipe(string recipeId) {
public bool CanPlantStrain(int plotIndex, string strainId) {
public bool PlantWithStrain(int plotIndex, string strainId, int currentDay) {
public bool ClearPlot(int plotIndex) {
public void Water(int plotIndex, AgriWaterBand band) {
public bool CanTreatPest(int plotIndex, string treatmentItemId) {
public bool TryTreatPestInfestation(int plotIndex, string treatmentItemId) {
public bool CanStartCompost(string recipeId) {
public bool TryStartCompostBatch(string recipeId, int currentDay) {
public bool IsCompostReady(string recipeId, int currentDay) {
public int TryCollectCompost(string recipeId, int currentDay) {
public bool TryApplyCompost(int plotIndex) {
public struct YieldInputs
public int BaseYield;
public float StrainModifier;
public float LightFactor;          // 0..1 grow-light availability
public float WaterBandFactor;      // 1 clean .. 0.55 unsafe
public int ToxicityPermille;
public int ToxicityTolerancePermille;
public float PestSeverity;
public float PestYieldDamageAtFull;
public int MutationOutcome;
public float MediumQuality;
public bool SoilContaminated;
public static YieldBreakdown CalculateYield(in YieldInputs inputs) {
public YieldBreakdown ForecastYield(int plotIndex, in AgricultureEnvironmentSnapshot env) {
public AgricultureHarvest Harvest(int plotIndex) {
public bool IsStrainUnlocked(string strainId) => _state.unlocked_strains.Contains(strainId);
public void UnlockStrain(string strainId) {
public void TickDay( int day, in AgricultureEnvironmentSnapshot env, ISeededRng pestRng, ISeededRng mutationRng, string seasonWindowId = "any")
public static AgriMutationOutcome ParseOutcome(string outcome) => outcome switch
public void NotifyBlightOutbreak(int plotIndex) {
public void CloseBlightNarrative(int plotIndex) {
```


# Appendix Q.564 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentDeepChainGate.cs`

### `Assets/Ashfall.Core/Content/ContentDeepChainGate.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 444 lines / 21947 bytes.
- SHA-256: `c5d60671673ea3333c1f484f8cfd293905a42799056208eada0956173951c301`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DeepChainHopSpec
public string HopId { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public string RequiredFile { get; set; } = string.Empty;
public string? RequiredLoader { get; set; }
public string[]? RequiredSystems { get; set; }
public string? RequiredSurface { get; set; }
public sealed class DeepChainSpec
public string ChainId { get; set; } = string.Empty;
public string Narrative { get; set; } = string.Empty;
public bool IsHardGate { get; set; }
public DeepChainHopSpec[] Hops { get; set; } = Array.Empty<DeepChainHopSpec>();
public sealed class DeepChainFinding
public string ChainId { get; set; } = string.Empty;
public string HopId { get; set; } = string.Empty;
public string MissingCategory { get; set; } = string.Empty;
public string Details { get; set; } = string.Empty;
public string Severity { get; set; } = "HARD"; // HARD | WARN
public string RecommendedFix { get; set; } = string.Empty;
public sealed class DeepChainReport
public string SchemaVersion { get; set; } = "1.0.0";
public List<DeepChainFinding> Findings { get; set; } = new();
public int ChainsEvaluated { get; set; }
public int HardFailures => Findings.Count(f => f.Severity == "HARD");
public int Warnings => Findings.Count(f => f.Severity == "WARN");
public bool HardGatePassed => HardFailures == 0;
public void Stabilize() =>
public static class ContentDeepChainGate
public static readonly DeepChainSpec ResearchToCraft = new() {
public static readonly DeepChainSpec ExpeditionToUse = new() {
public static readonly DeepChainSpec FactionTreatyBriefing = new() {
public static readonly DeepChainSpec[] WarnTierChains = new[] {
public static IEnumerable<DeepChainSpec> AllChains =>
public static DeepChainReport Evaluate(ContentUtilizationGraph graph) {
```


# Appendix Q.565 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/ExpansionHubSave.cs`

### `Assets/Ashfall.Core/ExpansionHubSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 507 lines / 28559 bytes.
- SHA-256: `6f9efccbfed1c5101889fb1aeff9b33800d110d217b1f669924bbbfdf2d1e282`.
- Architecture signals: seeded references=0; save/restore symbols=31; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class ExpansionHubSave
public const int CurrentSaveVersion = 6;
public int saveVersion = CurrentSaveVersion;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public DiseaseSystemState disease = new DiseaseSystemState();
public DebtDispatcherState debtDispatcher = new DebtDispatcherState();
public FactionEmbargoLedgerState embargoes = new FactionEmbargoLedgerState();
public DebtConsequenceBridgeState debtBridge = new DebtConsequenceBridgeState();
public SaltMineState saltMine = new SaltMineState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV5
public int saveVersion = 5;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public DiseaseSystemState disease = new DiseaseSystemState();
public DebtDispatcherState debtDispatcher = new DebtDispatcherState();
public FactionEmbargoLedgerState embargoes = new FactionEmbargoLedgerState();
public DebtConsequenceBridgeState debtBridge = new DebtConsequenceBridgeState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV1
public int saveVersion = 1;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV2
public int saveVersion = 2;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV3
public int saveVersion = 3;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV4
public int saveVersion = 4;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public DiseaseSystemState disease = new DiseaseSystemState();
public string Checksum = string.Empty;
public static class ExpansionHubSaveCodec
public static ExpansionHubSave Capture( int simDay, WaystationSystem waystation, LocationLayoutSystem layouts, LocationMemorySystem memory, SiteEncounterSystem siteEncounters,
public static string Encode(ExpansionHubSave save, IJsonSerializer json) {
public static ExpansionHubSave Decode(string jsonText, IJsonSerializer json) {
public static void Restore( ExpansionHubSave save, WaystationSystem waystation, LocationLayoutSystem layouts, LocationMemorySystem memory, SiteEncounterSystem siteEncounters,
```


# Appendix Q.566 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/ExpansionMasterSession.cs`

### `Assets/Ashfall.Core/ExpansionMasterSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 212 lines / 10386 bytes.
- SHA-256: `568760f21b08597c7cd360ea5f147c07feebc84edac5a7ca2f20addfd7545f0b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExpansionMasterSession
public HoldfastSession Holdfast { get; }
public DutyRosterSystem DutyRoster { get; }
public DutyRosterCatalog DutyRosterData { get; }
public LocationLayoutSystem StandingRecord { get; }
public CrossingSession Crossing { get; }
public SimClock Clock { get; }
public ILog Log { get; }
public SilentFoundrySystem SilentFoundry { get; }
public SilentFoundryCatalog FoundryData { get; }
public DiseaseSystem Disease { get; }
public DiseaseCatalog DiseaseData { get; }
public bool AllExpansionsActive =>
public static ExpansionMasterSession Load(string dataDirectory, int seed =808, ILog? log = null) {
public void TickDaily(WeatherKind weather, float outdoorTemp, List<DutyRosterOccupant>? homeOccupants = null, IReadOnlyList<string>? diseaseCandidates = null) {
public static HeadlessReport RunAllSelfTests(string? dataDirectory = null, ILog? log = null) {
```


# Appendix Q.567 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Foundry/FoundryActionSurface.cs`

### `Assets/Ashfall.Core/Foundry/FoundryActionSurface.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 109 lines / 3939 bytes.
- SHA-256: `659086666a1f48e3dc91373dee772a5635a7c227fab418c356021ed573af15fc`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FoundryActionSurface
public SilentFoundrySystem System => _system;
public FoundryActionResult AddCharge(string materialId, int units) {
public FoundryActionResult SelectRecipe(string recipeId) {
public FoundryActionResult Preheat(int targetTempC) {
public FoundryActionResult TapAndCast(int day) {
public FoundryActionResult ResolveStrike(string resolutionId) {
public sealed class FoundryActionResult
public bool Succeeded;
public string ReasonCode;
public string OutcomeLabel;
public Dictionary<string, int> IntDeltas = new Dictionary<string, int>();
public static FoundryActionResult Ok(Dictionary<string, int> deltas, string label) {
public static FoundryActionResult Fail(string reason) => new FoundryActionResult
```


# Appendix Q.568 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Greenhouse/GreenhouseHeadlessDemo.cs`

### `Assets/Ashfall.Core/Greenhouse/GreenhouseHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 109 lines / 4835 bytes.
- SHA-256: `dd194b237bdf55ac6c5f1bf8a81492f9e66bf5c08a1b5b83c1499f0eecdf0518`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class GreenhouseHeadlessReport : HeadlessReport
public int Harvests;
public int Blights;
public static class GreenhouseHeadlessDemo
public static GreenhouseHeadlessReport Run(ILog? log = null) {
```


# Appendix Q.569 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Shelter/MachineIdentity/ShelterMachineTellCatalog.cs`

### `Assets/Ashfall.Core/Shelter/MachineIdentity/ShelterMachineTellCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 653 lines / 36876 bytes.
- SHA-256: `5807c6ec3fdad9169ddd6c47d30ae978055a2b1d124f97d3360538513a4b5503`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class MachineConditionKeys
public const string HepaFilterHealth = "hepa.filter_health";
public const string HepaRadon = "hepa.radon_bqm3";
public const string FoundryRefractoryLining = "foundry.refractory_lining";
public const string FoundryHearthTuyeres = "foundry.hearth_tuyeres";
public const string FoundrySandBeds = "foundry.sand_beds";
public const string FoundryStructuralSupports = "foundry.structural_supports";
public const string FoundrySafetyExhaust = "foundry.safety_exhaust";
public const string FoundryAverageCondition = "foundry.average_condition";
public const string PowerFuelUnits = "power.fuel_units";
public const string PowerBatteryReserve = "power.battery_reserve";
public const string VentilationFilterSaturation = "ventilation.filter_saturation";
public const string VentilationDuctIntegrity = "ventilation.duct_integrity";
public const string WaterFilterIntegrity = "water.filter_integrity";
public const string ThermalBoilerFuel = "thermal.boiler_fuel";
public const string AirlockIncidentActive = "airlock.incident_active";
public static readonly string[] All = {
public static string FamilyOf(string machineId) {
public sealed class MachineConditionReadings
public float HepaFilterHealth = 100f;
public float HepaRadon = 12f;
public bool HazardWeather;
public float FoundryRefractoryLining = 100f;
public float FoundryHearthTuyeres = 100f;
public float FoundrySandBeds = 100f;
public float FoundryStructuralSupports = 100f;
public float FoundrySafetyExhaust = 100f;
public float PowerFuelUnits = 100f;
public float PowerBatteryReserve = 100f;
public bool PowerBrownout;
public float VentilationFilterSaturation;
public float VentilationDuctIntegrity = 100f;
public float VentilationSmokeSoot;
public bool VentilationMainDuctOpen = true;
public float WaterFilterIntegrity = 100f;
public float ThermalBoilerFuel = 100f;
public bool AirlockIncidentActive;
public string source = string.Empty;
public float? Get(string conditionKey) {
public bool HasContext(string context) {
public static class MachineQuirkKinds
public const string Diagnostic = "diagnostic";
public const string Personality = "personality";
public enum MachineConditionBand
public sealed class ShelterMachineCatalogData
public int schema_version = 1;
public string collection_id = string.Empty;
public List<MachineIdentityRecord> machines = new List<MachineIdentityRecord>();
public List<MachineQuirkRecord> quirks = new List<MachineQuirkRecord>();
public List<ShelterGlitchEvent>? glitch_events;
public sealed class MachineIdentityRecord
public string id = string.Empty;
public string condition_owner = string.Empty;
public string display_name = string.Empty;
public string nickname = string.Empty;
public string room_id = string.Empty;
public string purpose = string.Empty;
public string age_origin = string.Empty;
public string baseline_sound = string.Empty;
public string condition_key = string.Empty;
public List<string> quirk_ids = new List<string>();
public string audio_cue_family = string.Empty;
public string maintenance_skill_hook = string.Empty;
public string memorial_hook = string.Empty;
public sealed class MachineQuirkRecord
public string id = string.Empty;
public string machine_id = string.Empty;
public string kind = MachineQuirkKinds.Diagnostic;
public string condition_key = string.Empty;
public string comparison = "below";
public float trigger_below = -1f;
public string context = string.Empty;
public string text_cue = string.Empty;
public string audio_cue = string.Empty;
public string severity = "info";
public string maintenance_action = string.Empty;
public string repeat_policy = "continuous";
public sealed class ShelterGlitchEvent
public string id = string.Empty;
public string machine_id = string.Empty;
public string kind = "real_fault";
public string log_code = string.Empty;
public string title = string.Empty;
public string condition_key = string.Empty;
public string comparison = "below";
public float trigger_value = -1f;
public string context = string.Empty;
public string presentation = string.Empty;
public string resolution = string.Empty;
public List<string> repair_kit = new List<string>();
public string severity = "warning";
public string repeat_policy = "continuous";
public int cooldown_days;
public sealed class ShelterMachineTellCatalog
public const string FileName = "shelter_machine_identities.json";
public IReadOnlyList<MachineIdentityRecord> Machines => _machines;
public IReadOnlyList<MachineQuirkRecord> Quirks => _quirks;
public IReadOnlyList<ShelterGlitchEvent> GlitchEvents => _glitchEvents;
public int MachineCount => _machines.Count;
public static ShelterMachineTellCatalog Load(IFileIO files, IJsonSerializer json, string dataDirectory) {
public IReadOnlyList<ShelterGlitchEvent> GetGlitchEventsForMachine(string machineId) {
public ShelterGlitchEvent? GetGlitchEvent(string glitchId) {
public IReadOnlyList<ShelterGlitchEvent> EvaluateGlitchEvents( string machineId, MachineConditionReadings readings, Func<string, bool>? isNoted = null) {
public MachineIdentityRecord? GetMachine(string machineId) {
public MachineQuirkRecord? GetQuirk(string quirkId) {
public IReadOnlyList<MachineQuirkRecord> GetQuirksForMachine(string machineId) {
public IReadOnlyList<MachineQuirkRecord> EvaluateQuirks(string machineId, MachineConditionReadings readings) {
public MachineConditionBand EvaluateBand(string machineId, MachineConditionReadings readings) {
public static MachineConditionBand BandFor(float condition) {
public List<string> Validate() {
```


# Appendix Q.570 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 303 lines / 12451 bytes.
- SHA-256: `f1630ce8db3a4c544cd1889159df443cbc87fa346a587cf679f5858b3021a33a`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FoundryIngredientEntry
public string item_id = string.Empty;
public int amount = 1;
public sealed class FoundryProductEntry
public string product_id = string.Empty;
public string display_name = string.Empty;
public string category = string.Empty;
public string result_item_id = string.Empty;
public int result_amount = 1;
public List<FoundryIngredientEntry> ingredients = new List<FoundryIngredientEntry>();
public float labor_hours = 0f;
public float cast_hours = 0f;
public int fuel_units = 0;
public int water_litres = 0;
public float skill_target = 0.5f;
public float quality_target = 70f;
public string treaty_id = string.Empty;
public int quota_amount = 0;
public string sink = string.Empty;
public string notes = string.Empty;
public string[] tags = Array.Empty<string>();
public sealed class FoundryProductionFile
public int schema_version = 1;
public string collection_id = string.Empty;
public List<FoundryProductEntry> products = new List<FoundryProductEntry>();
public sealed class FoundryFactionRelation
public string faction_id = string.Empty;
public string stance = string.Empty;   // ally | trade_partner | rival | internal
public string notes = string.Empty;
public sealed class FoundryFactionEntry
public string faction_id = string.Empty;
public string display_name = string.Empty;
public string short_name = string.Empty;
public string identity = string.Empty;
public string icon_path = string.Empty;
public string[] internal_divisions = Array.Empty<string>();
public List<FoundryFactionRelation> relationships = new List<FoundryFactionRelation>();
public string[] tags = Array.Empty<string>();
public static class SilentFoundryCatalogLoader
public const string ProductionFileName = "foundry_production.json";
public const string FactionFileName = "foundry_faction.json";
public const string AccordsFileName = "foundry_accords.json";
public static Dictionary<string, int> LoadAccordRatificationDays( string dataDirectory, IFileIO? files = null, IJsonSerializer? serializer = null) {
public static FoundryProductionFile LoadProduction( string dataDirectory, IFileIO? files = null, IJsonSerializer? serializer = null) {
public static FoundryFactionEntry? LoadFaction( string dataDirectory, IFileIO? files = null, IJsonSerializer? serializer = null) {
public sealed class SilentFoundryCatalog
public FoundryFactionEntry Faction { get; private set; }
public IReadOnlyList<FoundryProductEntry> AllProducts => _products;
public int ProductCount => _products.Count;
public void Load(FoundryProductionFile production, FoundryFactionEntry faction) {
public void MergeHeavyRecipes(IEnumerable<FoundryProductEntry> heavyProducts) {
public void MergeGlassworksRecipes(IEnumerable<FoundryProductEntry> glassProducts) {
public FoundryProductEntry? GetProduct(string productId) {
public List<FoundryProductEntry> GetByCategory(string category) {
public List<FoundryProductEntry> GetQuotaProducts() {
```


# Appendix Q.571 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Foundry/SilentFoundryHeadlessDemo.cs`

### `Assets/Ashfall.Core/Foundry/SilentFoundryHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 187 lines / 11583 bytes.
- SHA-256: `d09f8c8d3f60830df15c9028c4cae67540b0c213d1c543dc0bfb4a419fe8022f`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class SilentFoundryHeadlessDemo
public const int DemoSeed = 1009;
public static HeadlessReport Run(string? dataDirectory = null, ILog? log = null) {
```


# Appendix Q.572 — Additional Current Architecture Evidence: `src/Host/GreenhouseHostSession.cs`

### `src/Host/GreenhouseHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 450 lines / 19290 bytes.
- SHA-256: `dfb777160557cbc0d0475e4c0bca79b326e6bb35039fa2a78f1ce993f2829a45`.
- Architecture signals: seeded references=1; save/restore symbols=5; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class GreenhouseHostSession
public GreenhouseSystem System { get; }
public ApicultureSystem Apiculture { get; }
public InventoryHostSession? InventoryHost { get; set; }
public string LastEvent { get; private set; } = string.Empty;
public Func<string>? SeasonWindowProvider { get; set; }
public string CurrentSeasonLabel => SeasonWindowProvider?.Invoke() ?? "Standard";
public static GreenhouseHostSession Create(InventoryHostSession? inventoryHost = null) {
public bool Plant(int plotIndex, string seedItemId, int currentDay) {
public bool Water(int plotIndex, float waterUnits, bool tainted) {
public CommandResult PreviewTreatBlight(int plotIndex) {
public CommandResult ExecuteTreatBlight(int plotIndex) {
public bool TreatBlight(int plotIndex) {
public bool Harvest(int plotIndex) {
public bool Clear(int plotIndex) {
public bool ApplyNutrients(int plotIndex) {
public bool InstallHive(string hiveId, string bayId, int currentDay) {
public bool InspectHive(string hiveId, int currentDay) {
public bool FeedHive(string hiveId, float amount = 0.5f) {
public bool HarvestHoney(string hiveId) {
public void TickDay(int currentDay, float growLightHours = 6f, float ashContaminationRate = 0.05f) {
public GreenhouseState CaptureSave() {
public static class GreenhouseSaveStore
public const string FileName = "greenhouse_save.json";
public const string SectionName = "greenhouse";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static bool TrySave(GreenhouseState state) => s_store.TrySave(state);
public static GreenhouseState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(GreenhouseState state) => s_store.CapturePersisted(state);
public sealed class GreenhouseSaveEnvelope
public GreenhouseState? State { get; set; }
public string? Checksum { get; set; }
```


# Appendix Q.573 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Balance/ResourceMassBalanceSimulator.cs`

### `Assets/Ashfall.Core/Balance/ResourceMassBalanceSimulator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 627 lines / 29850 bytes.
- SHA-256: `f8d99d0521a523fa7b58d4afdd4154ef288eaa9beab2f3c4e2d3f5b391e052e2`.
- Architecture signals: seeded references=1; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ResourceMassBalanceConfig
public int Seed { get; set; } = 42;
public int Days { get; set; } = 30;
public int CrewSize { get; set; } = 4;
public float DailyRawWaterInflow { get; set; } = 12f;
public float InitialCleanWater { get; set; } = 25f;
public float InitialRawWater { get; set; } = 30f;
public float InitialFuel { get; set; } = 80f;
public int InitialCannedFood { get; set; } = 80;
public int InitialRawMeat { get; set; } = 12;
public bool EnableTrapping { get; set; } = true;
public bool EnableGreenhouse { get; set; } = true;
public bool EnableKitchen { get; set; } = true;
public bool EnablePowerGrid { get; set; } = true;
public string ScenarioName { get; set; } = "Baseline";
public sealed class ResourceMassBalanceDailyTelemetry
public int Day { get; set; }
public float AvgHealth { get; set; }
public float AvgHunger { get; set; }
public float AvgThirst { get; set; }
public float AvgMorale { get; set; }
public float AvgWarmth { get; set; }
public int AliveCrew { get; set; }
public double StoredWaterTotal { get; set; }
public int CleanWaterBottles { get; set; }
public double WaterDiscrepancy { get; set; }
public int FoodInventoryCount { get; set; }
public int PantryMealPortions { get; set; }
public int MealsServedToday { get; set; }
public int FoodSpoiledToday { get; set; }
public float FuelUnitsRemaining { get; set; }
public float BatteryReserveWh { get; set; }
public float BrownoutHours { get; set; }
public int TrappingCatchesToday { get; set; }
public int GreenhouseHarvestsToday { get; set; }
public sealed class ResourceMassBalanceResult
public bool Success { get; set; } = true;
public string ScenarioName { get; set; } = string.Empty;
public int Seed { get; set; }
public int DaysSimulated { get; set; }
public float FinalSurvivalRate { get; set; }
public int SurvivorsAlive { get; set; }
public int TotalDeaths { get; set; }
public float AvgSurvivorHealth { get; set; }
public float AvgSurvivorHunger { get; set; }
public float AvgSurvivorThirst { get; set; }
public float AvgSurvivorMorale { get; set; }
public double TotalWaterInflow { get; set; }
public double TotalWaterConsumedCrew { get; set; }
public double TotalWaterCropTranspiration { get; set; }
public double TotalWaterFilterWaste { get; set; }
public double FinalWaterStored { get; set; }
public double MaxWaterDiscrepancy { get; set; }
public int TotalMeatProduced { get; set; }
public int TotalCropsHarvested { get; set; }
public int TotalMealsServed { get; set; }
public int TotalFoodSpoiled { get; set; }
public float TotalFuelBurned { get; set; }
public float TotalBrownoutHours { get; set; }
public List<ResourceMassBalanceDailyTelemetry> Telemetry { get; } = new List<ResourceMassBalanceDailyTelemetry>();
public List<string> InvariantViolations { get; } = new List<string>();
public static class ResourceMassBalanceSimulator
public static ResourceMassBalanceResult Run(ResourceMassBalanceConfig config, ILog? log = null) {
```


# Appendix Q.574 — Additional Current Architecture Evidence: `src/Host/ExpansionHostSession.cs`

### `src/Host/ExpansionHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 500 lines / 25698 bytes.
- SHA-256: `9809fca6ed766dfd903f6dcc7df79e68f779d539a85bbd8a66066710d570da45`.
- Architecture signals: seeded references=0; save/restore symbols=5; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExpansionHostSession
public const int DefaultSeed = 1117; // greenhouse + vouch demo seed
public WaystationSystem Waystation { get; }
public LocationLayoutSystem Layouts { get; }
public LocationMemorySystem Memory { get; }
public SiteEncounterSystem SiteEncounters { get; }
public StandingRecordCatalog RecordQuests { get; }
public VouchAccessSystem Vouch { get; }
public GreenhouseSystem Greenhouse { get; private set; }
public CrossingArbitrationSystem Arbitration { get; }
public LedgerDebtSystem Ledger { get; }
public CrossingQuestSystem CrossingQuests { get; }
public GenerationalSuccessionEngine Generational { get; }
public EpilogueMatrixRuntime Epilogue { get; }
public DutyRosterSystem DutyRoster { get; private set; }
public Ashfall.Core.Foundry.SilentFoundrySystem SilentFoundry { get; private set; }
public Ashfall.Core.Foundry.SilentFoundryCatalog FoundryData { get; private set; }
public Ashfall.Core.Disease.DiseaseSystem Disease { get; private set; }
public Ashfall.Core.Disease.DiseaseCatalog DiseaseData { get; private set; }
public DebtTemplateCatalog? DebtCatalog { get; private set; }
public DebtConsequenceDispatcher? DebtDispatcher { get; private set; }
public FactionEmbargoLedger Embargoes { get; } = new FactionEmbargoLedger();
public void BindDutyRoster(DutyRosterSystem roster) {
public void BindGreenhouse(GreenhouseSystem shared) {
public event Action<CrossingStageNarrativeEvent>? OnCrossingStageNarrative;
public static ExpansionHostSession Create( string dataDirectory, ILog log = null!, Ashfall.Core.Flags.IFlagLedger? consequenceLedger = null) {
public void ShutdownDebtIntegration() {
public override void Dispose() {
public ExpansionHubSave CaptureSave(int simDay, DebtConsequenceBridgeState? debtBridge = null) =>
public void RestoreSave(ExpansionHubSave save, DebtConsequenceHostBridge? debtBridge = null) =>
public void LoadDefaultBackerPool() {
public string ArbitrationLine() {
public string LedgerLine() {
public void UnlockWaystation() => Waystation.Unlock();
public void SetWaystationWintering(bool wintering) => Waystation.SetWintering(wintering);
public bool AssignWaystationWatch(string[] ids) => Waystation.AssignWatch(ids);
public void ResupplyWaystation() => Waystation.Resupply();
public void TickWaystation(bool iceRoadOpen) => Waystation.TickDaily(iceRoadOpen);
public string WaystationLine() {
public void UnlockRecord() {
public bool ArriveAtSite(string parentId) => Layouts.ArriveAtParent(parentId);
public bool EnterSiteRoom(string roomId) => Layouts.EnterRoom(roomId);
public bool InspectSiteRoom(string roomId) => Layouts.InspectRoom(roomId);
public string RoomLine(string parentId, string roomId) {
public string StandingRecordLine() {
public string RecordQuestLine() {
public bool GrantVouch(string npcId) => Vouch.GrantVouch(npcId, isLastResort: false);
public bool BurnVouch() => Vouch.BurnVouch();
public bool SoftenAccess() => Vouch.SoftenAccess();
public string CrossingLine() {
public bool StartCrossingQuest(string questId, int currentDay) => CrossingQuests.StartQuest(questId, currentDay);
public void TickCrossingQuests(int currentDay) => CrossingQuests.TickDaily(currentDay, hasVouchAccess: Vouch.HasAccess);
public int AdvanceCrossingQuestStage(string questId) => CrossingQuests.AdvanceStage(questId);
public bool MakeCrossingChoice(string questId, string choiceId) => CrossingQuests.MakeChoice(questId, choiceId);
public List<CrossingQuestDef> GetAvailableCrossingQuests(int currentDay) => CrossingQuests.GetAvailableQuests(currentDay);
public bool FailCrossingQuest(string questId) => CrossingQuests.FailQuest(questId);
public bool IsCrossingQuestFailed(string questId) => CrossingQuests.IsQuestFailed(questId);
public bool IsCrossingQuestCompleted(string questId) => CrossingQuests.IsQuestCompleted(questId);
public string CrossingQuestLine() {
public void EnsureGreenhousePlots(int count) => Greenhouse.EnsurePlots(count);
public bool PlantGreenhouse(int plotIndex, string seedItemId, int day) => Greenhouse.Plant(plotIndex, seedItemId, day, out _);
public void WaterGreenhouse(int plotIndex, float units) => Greenhouse.Water(plotIndex, units, tainted: false);
public GreenhouseHarvest HarvestGreenhouse(int plotIndex) => Greenhouse.Harvest(plotIndex);
public void TickGreenhouse(int simDay) =>
public string GreenhouseLine() {
public void RegisterGenerationDweller(string dwellerId, int age, int generation = 0) => Generational.RegisterDweller(dwellerId, age, generation);
public string AdvanceGenerationalTime(int days) {
public string FormMentorshipDemo(string mentorId, string apprenticeId, string traitId) {
public string GenerationalLine() {
public GenerationalSuccessionSaveState CaptureGenerationalSave() => Generational.CaptureState();
public void RestoreGenerationalSave(GenerationalSuccessionSaveState state) => Generational.RestoreState(state);
public string GenerateEpilogueNarrativeDemo(EpilogueEvaluationContext ctx) => Epilogue.GenerateEpilogueNarrative(ctx);
```


# Appendix Q.575 — Additional Current Architecture Evidence: `src/Main.Plans62_65.cs`

### `src/Main.Plans62_65.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 218 lines / 9686 bytes.
- SHA-256: `32d983d505b35066c4366e68303224a5a98a0e88b5ffccc7ef528dbf3b9febb8`.
- Architecture signals: seeded references=2; save/restore symbols=4; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public PrewarArchiveDecryptionSystem? ArchiveDecryptionSystem => _archiveDecryption62;
public FoodPreservationSystem? FoodPreservationSystem => _foodPreservation64;
public CampaignEpilogueEngine? CampaignEpilogueEngine => _epilogueEngine65;
public void TickPlans62To65(int day) {
```


# Appendix Q.576 — Additional Current Architecture Evidence: `src/UI/FarmingPanel.cs`

### `src/UI/FarmingPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 481 lines / 21224 bytes.
- SHA-256: `bd294ce4494a3a805f205b00618d5fdae3a33593efd4e512b8c09c4e2c194638`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=11; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class FarmingPanel : Control
public event Action? OnClose;
public event Action<string, string>? OnActionRequested;
public bool IsBound => _host != null;
public void Bind(AgricultureHostSession session) {
public override void _Ready() {
public void RefreshView() {
public void Open() {
public void Close() {
public override void _UnhandledInput(InputEvent @event) {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix Q.577 — Additional Current Architecture Evidence: `src/Host/HostCli.Plans162_165.cs`

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


# Appendix Q.578 — Additional Current Architecture Evidence: `src/Host/ContentUtilizationRuntimeCollector.cs`

### `src/Host/ContentUtilizationRuntimeCollector.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1230 lines / 64739 bytes.
- SHA-256: `4be289e49ddef6d5dcd29ccdc988a5f397b6153d4d20f1aa585ca9fe39df9f65`.
- Architecture signals: seeded references=3; save/restore symbols=0; typed event declarations=0; textual Godot mentions=47; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class ContentUtilizationRuntimeCollector
public const int DefaultSeed = 9001;
public static ContentUtilizationInstrumentation Collect(string dataDir) {
public bool CanApplyMorale(string survivorId, int delta, bool shelterWide, out string reason) {
public void ApplyMorale(string survivorId, int delta, bool shelterWide) { }
public bool CanGrantFactionIntel(string canonicalFactionId, out string reason) {
public void GrantFactionIntel(string canonicalFactionId) { }
public bool CanOfferExpedition(string locationId, out string reason) {
public void OfferExpedition(string locationId) { }
public bool CanApplyFactionStanding(string canonicalFactionId, int delta, out string reason) {
public void ApplyFactionStanding(string canonicalFactionId, int delta) { }
```


# Appendix Q.579 — Additional Current Architecture Evidence: `src/Host/HostCli.PanelTests.cs`

### `src/Host/HostCli.PanelTests.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 4479 lines / 250824 bytes.
- SHA-256: `23b1c0498d6a8cea2b2f69b49d342aed1d444425b6bee4d6c0b144f195e78443`.
- Architecture signals: seeded references=15; save/restore symbols=102; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=3; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=12.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunYearOfAshSaveSelfTest(string dataDirectory) {
public static int RunDutyRosterSaveSelfTest(string dataDirectory) {
public static int RunExpansionHubSaveSelfTest(string dataDirectory) {
public static int RunExpeditionSelfTest() {
public static int RunBridgeSelfTest() {
public static int RunPowerGridCatalogSelfTest() {
public static int RunExpeditionEncounterBridgeSelfTest() {
public static int RunMedicalSelfTest() {
public static int RunNarrativeSelfTest() {
public static int RunOralLoreSelfTest(string dataDirectory) {
public static int RunSurvivorsSelfTest() {
public static int RunWorldSelfTest() {
public static int RunEconomySelfTest(string dataDirectory) {
public static int RunUtilityAiSelfTest(string dataDirectory) {
public static int RunDoseLedgerSelfTest(string dataDirectory) {
public static int RunBlackFlotillaSelfTest(string dataDirectory) {
public static int RunRadioSelfTest() {
public static int RunHoldfastBriefing(string dataDirectory) {
public static int RunIceRoadTickDemo(string dataDirectory) {
public static int RunHoldfastSaveSelfTest(string dataDirectory) {
public static int RunStandaloneSystemsSelfTest() {
public static int RunPhase0SelfTest() {
public static int RunCaravanSelfTest() {
public static int RunAssetRegistrySelfTest(string dataDirectory) {
public static int RunAssetCoverageReport(string dataDirectory) {
public static int RunDay1PlayableSelfTest(string dataDirectory) {
public static int RunDay1ToDay2MilestoneSelfTest(string dataDirectory) {
public static int RunUiLayoutSelfTest(string dataDirectory) {
public static int RunSettingsSelfTest(string dataDirectory) {
public static int RunPlayableShellSelfTest(string dataDirectory) {
public static int RunShelterHazardLoopSelfTest(string dataDirectory) {
public static int RunShelterOperationsSelfTest(string dataDirectory) {
public static string SnapshotGoldenRoot() {
public static string SnapshotCaptureRoot() {
internal sealed class PanelTestFaultyFileIo : Ashfall.Core.IFileIO
public bool DirectoryExists(string path) => true;
public bool FileExists(string path) => true;
public string ReadAllText(string path) => throw new System.IO.IOException("Simulated I/O disk error");
public void WriteAllText(string path, string contents) { }
public string Combine(params string[] parts) => System.IO.Path.Combine(parts);
internal sealed class PanelTestCorruptJsonFileIo : Ashfall.Core.IFileIO
public bool DirectoryExists(string path) => true;
public bool FileExists(string path) => true;
public string ReadAllText(string path) => "{ not valid json syntax !!!";
public void WriteAllText(string path, string contents) { }
public string Combine(params string[] parts) => System.IO.Path.Combine(parts);
```


# Appendix R.580 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/GreenhouseCropExpansionTests.cs`

### `Ashfall.Core.Tests/GreenhouseCropExpansionTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 181; SHA-256: `74fe8e15894c195f4a50093b67162fa16c5d6afdc8adc79e529f5300a15713ec`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CropCatalog_ContainsAll13Crops
CropCatalog_MixedSeedPacket_IsPlantable_AndCanonical
CropCatalog_ResolvesSeedToCleanYield
GreenhouseSystem_SimulatesFrostTuberLifecycle
GreenhouseSystem_SimulatesMedicinalHerbAndLeafyGreen
GreenhouseSystem_SaveRestore_PreservesExpandedCropState
```


# Appendix R.581 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/AgriculturePersistenceTests.cs`

### `Ashfall.Core.Tests/AgriculturePersistenceTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 170; SHA-256: `7ae5561f1f8c81a609ed74fb58bbe33cb1515e6ae9c4cc34d9e0371154712adf`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CaptureState_IsADeepCopyNotAnAlias
SaveRestore_ContinuationMatchesUninterruptedRun
RestoreState_OldSaveDefaultsSurvive
UnlockedStrainsAndNarrativeFlags_PersistAcrossRoundTrip
NutritionState_RoundTripsAndDrivesContinuation
```


# Appendix R.582 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Foundry/FoundryActionSurfaceTests.cs`

### `Ashfall.Core.Tests/Foundry/FoundryActionSurfaceTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 69; SHA-256: `612658ad445a95d26032f760ac17c50b06de75ab0827af314597b58de5415500`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
TapAndCast_ReturnsResultWithStableCode
AddCharge_ValidatesInputs
SelectRecipe_ValidatesInput
Preheat_ValidatesRange
ResolveStrike_ValidatesInput
Actions_ReturnOkOrFailWithReasonCode
```


# Appendix R.583 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/GreenhouseEquipmentScalingTests.cs`

### `Ashfall.Core.Tests/GreenhouseEquipmentScalingTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 80; SHA-256: `8e4abc6ff79d7cd9ce13b49ecf64d95de03e832e68b13436902e75e228f5893c`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
GrowLightInput_ScalesCropGrowthAndCapsAtCropRequirement
EnsurePlots_GrowsAndRemovesOnlyTrailingFallowPlots
HarvestAndClear_KeepTheCurrentResidualContaminationContract
```


# Appendix R.584 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/ShelterMachineTellTests.cs`

### `Ashfall.Core.Tests/ShelterMachineTellTests.cs`

- Current test declarations: Fact=17, Theory=0, InlineData=0.
- File lines: 352; SHA-256: `8efba80cec8ad8c88907cb99896fd68f5e436bf7f1846b2a174549639ae9c2a6`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
MachineCatalog_Loads_FromDataAuthority_AndValidates
MachineCatalog_MissingFile_YieldsEmptyValidCatalog
Machines_BindToCanonicalRooms_WithRealConditionOwners
HepaTell_FiresExactlyWhenOwnerRaisesItsAirHazard
HepaBand_MatchesOwnerWarningState
FoundryTells_MirrorOwnerSafetyWarningFloors
FoundryTell_FiresExactlyWhenLiveOwnerWouldWarn
FoundryBand_UsesTheOwnerOverallCondition
PersonalityTell_AlwaysPresent_NeverFaultStyled
PersonalityAndDiagnosticTells_AreClassifiedInData
Evaluation_IsDeterministic_AndAuthoredOrdered
UnknownMachineOrMissingCondition_YieldsNothing
BandFor_MapsTheDocumentedBands
GlitchEvents_Harmless_AlwaysEligible_AndOneShotGatedByJournal
GlitchEvents_RealFault_FiresOnlyWhenThresholdMet
QuirkComparison_Above_TriggersWhenValueExceedsThreshold
GlitchEvents_UnknownMachine_ReturnsEmpty
```


# Appendix R.585 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Shelter/Plan196FoodTypeTempSeamTests.cs`

### `Ashfall.Core.Tests/Shelter/Plan196FoodTypeTempSeamTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 210; SHA-256: `b268ad5c0a673ca3e53f89fa342db3ef6bbeb045ea71c1489bd17347565364e1`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
AddCohort_AllowsMappedFoodType_OnTierAllowList
AddCohort_BlocksFoodType_NotOnTierAllowList
AddCohort_EmptyAllowList_RemainsUnrestricted
TickDay_DefaultStorageTemp_PreservesPriorAmbientDecay
TickDay_ColdStorage_SlowsDecay
TickDay_WarmStorage_AcceleratesDecay
ResolveFoodType_UsesMapThenIdentity
AuthoredCatalog_LoadsFoodTypeMap_AndGatesSaltCured
```


# Appendix R.586 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/World/Plan126_129CrossingFoundryIntegrationTests.cs`

### `Ashfall.Core.Tests/World/Plan126_129CrossingFoundryIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 233; SHA-256: `aa4f405426b00ae803ede9eb65259a230dcd601dee7ed142ef8d794a56f1beb2`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plan126_CrossingItems_LoadsExactTwentyFiveItemsAndValidatesExpansionTier
Plan129_FoundryProduction_LoadsExactThirtyFiveProductsAndValidatesNineExpansionProducts
Plan126_129_CrossDomainCoherence_IndustrialSupplyChainAndBorderCommerceLinkages
Plan126_129_DeterministicInventoryTradeAndProductionSimulation
```


# Appendix R.587 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/DirtyFlushNoOpRegressionTests.cs`

### `Ashfall.Core.Tests/DirtyFlushNoOpRegressionTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 132; SHA-256: `e71cc87cc9b79babd3dbeeb6dadd0160daf552918afb09102d52fb40591393e5`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Greenhouse_FallowPlots_TickDay_DoesNotMutateState
Greenhouse_FallowPlots_TickDay_FiresNoEvents
Apiculture_NoHives_TickDaily_DoesNotMutateState
Apiculture_NoHives_TickDaily_FiresNoDomainEvents
Greenhouse_ActiveCrop_TickDay_DoesMutateState
```


# Appendix R.588 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Foundry/FoundryPlan129IntegrationTests.cs`

### `Ashfall.Core.Tests/Foundry/FoundryPlan129IntegrationTests.cs`

- Current test declarations: Fact=12, Theory=0, InlineData=0.
- File lines: 488; SHA-256: `2473f0bafad7082ebcbe94cb0000747d5b824f9911adb31ca9fcc7f55250874b`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CatalogCardinality_IsExactlyThirtyFiveProducts
ProductIds_AreUniqueAndFollowPrefixConvention
ForeignKeys_EveryResultItemAndIngredientResolves
NumericBounds_AllCostsAndTargetsAreValid
RoleAndSinkCoverage_SpansAllStrategicDomains
Plan129Additions_ResolveToCanonicalIndustrialConsumers
Plan116Loot_ContainsTwoNewIndustrialOutputs
StrategicChoice_ScenarioA_WinterFuelConflict
StrategicChoice_ScenarioB_TreatyVsInternalRepair
StrategicChoice_ScenarioC_ExpeditionPreparation
ActiveHeat_SurvivesSaveAndRestoreRoundtrip
SimulationOutcomes_AreIdenticalWithSameSeed
```


# Appendix R.589 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/GreenhouseCommandTests.cs`

### `Ashfall.Core.Tests/GreenhouseCommandTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 62; SHA-256: `805e0a9d9a368f6c816a00b8682b1e3e82076dd4a32146164101c92822625bcb`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
PreviewTreatBlight_Available_WhenPlotHasBlight
ExecuteTreatBlight_StalePreview_RejectsWithoutMutation
ExecuteTreatBlight_FreshPreview_CuresBlight
```


# Appendix R.590 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/MicroLocationGreenhouseIntegrationTests.cs`

### `Ashfall.Core.Tests/MicroLocationGreenhouseIntegrationTests.cs`

- Current test declarations: Fact=13, Theory=0, InlineData=0.
- File lines: 268; SHA-256: `ca93b893a860171b6a997220e4f47a0caf500cc05515cfc099ff821ff778d1aa`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
F18_01_TakeGreenhouseSeeds_GrantsExactlyTwoSeedPackets
F18_02_OpenGreenhouseCabinet_GrantsExactlyOneMedicinalHerb
F18_03_BothRewardItems_ResolveAgainstItemCatalog
F18_04_SeedPackets_HasRealAgricultureConsumer
F18_05_MedicinalHerb_IsCanonicalCropYield_AndRecipeIngredient
F18_06_MedicinalHerb_ReferencedByCanonicalRecipe
F18_07_GrantedSeeds_PlantThroughNormalFlow_CropGrows
F18_08_GrantedSeeds_AreConsumed_PlantingIsNotFree
F18_09_PlantedPacket_MaturesThroughNormalGrowthTicks
F18_10_SeedsDoNotBypassGreenhouseGates_UnavailablePlotRefuses
F18_11_RewardsAreOneShot_SaveReloadCannotReGrant
F18_12_LeaveChoice_NeutralNonDepleting
F18_13_Deterministic_SameSeedSameChoice_IdenticalGrantTrace
```


# Appendix R.591 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Shelter/PlanE1_28CaptivePreservationTests.cs`

### `Ashfall.Core.Tests/Shelter/PlanE1_28CaptivePreservationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 181; SHA-256: `733547b6886424954babfe3b11e64eb0f831e8256d1e5fbe04d19d6b2455808e`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CaptiveDetention_Capture_AssignPenalLabor_ExecutesShift
CaptiveInterrogation_BreaksResistance_ExtractsIntelTopic
FoodPreservation_CuringJob_ConsumesInputsAndProducesPreservedOutput
CaptiveAndPreservation_CaptureAndRestore_PreservesIntegrity
```


# Appendix R.592 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Campaign/Plans62_65_SharedIntegrationTests.cs`

### `Ashfall.Core.Tests/Campaign/Plans62_65_SharedIntegrationTests.cs`

- Current test declarations: Fact=1, Theory=0, InlineData=0.
- File lines: 317; SHA-256: `6cea79cc95bd3d737a142fd0c62b204825219cda168baae79e4700c7391c114a`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ThirtyDay_FlagshipIntegrationScenario_SimulatesAllPillarsAndEvaluatesEpilogue
```


# Appendix R.593 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/ContentDeepChainGateTests.cs`

### `Ashfall.Core.Tests/ContentDeepChainGateTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 183; SHA-256: `02ef4c693e33f3fa924f4bc025931e49a5bb724d368bfb61d084594cd387ea21`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CompleteFlagshipChains_Pass
ProducerWithoutLoader_Fails
LoaderWithoutSystem_Fails
ConsumerWithoutSurface_Fails
MissingProducer_Fails
WarnTierBreaks_Warn_NeverHardFail
ExactMissingHop_IsReported
Output_IsDeterministic
Evaluation_IsBounded
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.
