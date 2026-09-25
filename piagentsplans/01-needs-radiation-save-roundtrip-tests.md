# Plan 01 — NeedsSystem & RadiationSystem Save Round-Trip Assurance

> **Rebuild status:** COMPLETE CORE TEST CONTRACT — MAINTENANCE AND REBASE PLAN
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

- This plan began as a two-line test gap and has already been implemented. The live suite covers exact DTO round trips, restored-state-driven ticking, no-op capture guards, default-state restoration, checksum stability, paired determinism, and the Core half of the no-survivor host fallback.
- The rebase therefore does not propose another save section, another needs authority, or a new radiation model. It defines how future needs and radiation changes must preserve the existing proof surface and how host-only fallback behavior remains tested without forcing Godot types into Core tests.
- The plan is still valuable as an executable architectural contract: it identifies which fields are authoritative, which mutations require checksum changes, and how restore must continue into the same deterministic tick sequence.

**Bounded outcome:** Retire H10 as an implementation project and preserve it as a standing save/determinism contract. The requested behavior already exists in `NeedsRadiationSaveRoundTripTests.cs`; future work may extend tests only when a live DTO or host projection changes.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `NeedsRadiationSaveRoundTripTests.cs` exists and contains the H10 cases named by the archived rulebook.
- `NeedsSystem` and `RadiationSystem` expose serializable state consumed through the current host save stack; Core remains engine-free.
- The current save registry already carries the relevant systems through existing sections; this plan must not add a parallel section.
- Historical H10 evidence is present, but current pass counts must be obtained by running the focused test rather than copied from an old closeout.

**Master-authority sections applied to this rebase:**

- Part II factory protocol
- Part VI anti-padding protocol
- Volume 28 verification cookbook

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the obsolete open-issue framing with a maintenance contract and current-source traceability.
- Require every new `SurvivorNeedsState` or radiation-state field to be classified as persistence-relevant or transient.
- Keep the no-survivor host fallback in the Godot host test surface; Core tests assert only the boundary contract they can reference.
- Add a field-addition review checklist and a two-run continuous-versus-interrupted proof template for future changes.

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
| vitals, thresholds, caps, alive/dead projection | Survivor needs state | `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` | NeedsSystem owns need mutation and derived threshold state. |
| acute/chronic dose and phase evolution | Radiation dose state | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` | RadiationSystem owns dose mutation; no UI or test writes its fields. |
| DTO capture, campaign envelope, no-survivor fallback | Host save projection | `src/Host/HoldfastRuntimeSession.cs; src/Main.SaveOrchestrator.cs` | The host projects Core state into canonical save sections and owns fallback decay. |
| round-trip, checksum, restored-tick and determinism proof | Focused contract suite | `Ashfall.Core.Tests/NeedsRadiationSaveRoundTripTests.cs` | The suite is the executable H10 seal. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ NeedsSystem & RadiationSystem Save Round-Trip Assurance
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ Survivor needs state
│   vitals, thresholds, caps, alive/dead projection
│ Radiation dose state
│   acute/chronic dose and phase evolution
│ Host save projection
│   DTO capture, campaign envelope, no-survivor fallback
│ Focused contract suite
│   round-trip, checksum, restored-tick and determinism proof
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

1. **Preserve current state ownership.** Survivor needs state owns vitals, thresholds, caps, alive/dead projection: NeedsSystem owns need mutation and derived threshold state.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| vitals, thresholds, caps, alive/dead projection | Survivor needs state | `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` | NeedsSystem owns need mutation and derived threshold state. |
| acute/chronic dose and phase evolution | Radiation dose state | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` | RadiationSystem owns dose mutation; no UI or test writes its fields. |
| DTO capture, campaign envelope, no-survivor fallback | Host save projection | `src/Host/HoldfastRuntimeSession.cs; src/Main.SaveOrchestrator.cs` | The host projects Core state into canonical save sections and owns fallback decay. |
| round-trip, checksum, restored-tick and determinism proof | Focused contract suite | `Ashfall.Core.Tests/NeedsRadiationSaveRoundTripTests.cs` | The suite is the executable H10 seal. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. Mutate authoritative Core state
2. project through the existing save DTO
3. serialize and checksum
4. restore into fresh instances
5. continue the same seeded tick
6. compare final state and checksum

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Every persistent needs field must round-trip exactly or through a documented normalization rule.
- Transient thresholds may be recomputed, but tests must prove recomputation does not erase meaningful state.
- Radiation phase and dose state must continue driving the next tick after restore.
- Checksum output must be culture-invariant and stable for equivalent state.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Capture must be a deep snapshot; later ticks must not mutate an envelope already being written.
- Restore must normalize null collections without replacing meaningful non-null values.
- A default or legacy DTO must produce documented neutral behavior, not an exception or silent identity reset.
- The host fallback must not project fabricated survivor state when no roster is present.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- No JSON catalog change.
- No new save file.
- No new item, recipe, or stat.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use current DTOs and current registered sections.
- No codec bump is justified by this maintenance plan.
- Any future additive field requires a default-tolerant restore test.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Paired fresh runs use the same seed and command sequence.
- Restore must not consume or reseed a different stream.
- Checksum formatting remains invariant-culture.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- No new gameplay event is required.
- Existing host state-change notifications remain downstream of restored mutation.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/HoldfastRuntimeSession.cs
- src/Main.SaveOrchestrator.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- None. This is a persistence and determinism plan.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A DTO field serializes but is absent from checksum coverage. | Survivor needs state | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | Restore returns a live alias that later ticks mutate. | Radiation dose state | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | Default restoration silently turns a meaningful value into zero. | Host save projection | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | The host fallback fabricates a survivor projection. | Focused contract suite | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A new test proves only tick behavior and misses persistence. | Survivor needs state | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/NeedsRadiationSaveRoundTripTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/NeedsRadiationSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/RadiationPhaseProgressionTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — current proof | Read current DTOs and rerun the focused suite. | Focused test passes and source references match. | No production path until the owning implementation package is separately claimed. |
| 1 — field inventory | Classify every current field as persistent, derived or transient. | No unclassified field remains. | No production path until the owning implementation package is separately claimed. |
| 2 — host boundary audit | Confirm the fallback and envelope paths use the same Core state. | No fabricated host projection. | No production path until the owning implementation package is separately claimed. |
| 3 — future-change gate | Publish the additive-field and determinism review checklist. | Checklist is referenced by future save-touching plans. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Ashfall.Core.Tests/NeedsRadiationSaveRoundTripTests.cs | MODIFY only if a current contract gap is proven | Test-only |
| Assets/Ashfall.Core/Survivors/NeedsSystem.cs | READ ONLY | Authoritative state |
| Assets/Ashfall.Core/Radiation/RadiationSystem.cs | READ ONLY | Authoritative state |
| src/Host/HoldfastRuntimeSession.cs | READ ONLY | Host fallback boundary |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Treating the archived pass count as current evidence. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Adding a test-only DTO that no production codec uses. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Moving host fallback math into Core. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No gameplay tuning.
- No new needs or radiation status.
- No full-suite run unless separately authorized.
- No save-section renumbering.

# 23. Rollback and Recovery

- Documentation-only rebase is reversible by restoring the prior file.
- Any future test change is isolated and leaves production untouched.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- Current focused suite is identified and runnable.
- Every state field has a persistence classification.
- No new authority or save section is proposed.
- The plan clearly retires H10 rather than reopening it.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the obsolete open-issue framing with a maintenance contract and current-source traceability.
- Require every new `SurvivorNeedsState` or radiation-state field to be classified as persistence-relevant or transient.
- Keep the no-survivor host fallback in the Godot host test surface; Core tests assert only the boundary contract they can reference.
- Add a field-addition review checklist and a two-run continuous-versus-interrupted proof template for future changes.

## MUST NOT DO

- No gameplay tuning.
- No new needs or radiation status.
- No full-suite run unless separately authorized.
- No save-section renumbering.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/NeedsRadiationSaveRoundTripTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/NeedsRadiationSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/RadiationPhaseProgressionTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — current proof — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: vitals, thresholds, caps, alive/dead projection → Survivor needs state; acute/chronic dose and phase evolution → Radiation dose state; DTO capture, campaign envelope, no-survivor fallback → Host save projection; round-trip, checksum, restored-tick and determinism proof → Focused contract suite. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 01.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 01 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by Survivor needs state or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Survivors/NeedsSystem.cs`

### `Assets/Ashfall.Core/Survivors/NeedsSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 440 lines / 19434 bytes.
- SHA-256: `2f5a0f324dcba195220f042ec220d6414211adca408010094f6e12e7872c4027`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=11; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum NeedKind
public class SurvivorNeedsState
public string Id = string.Empty;
public float Hunger;
public float Thirst;
public float Fatigue;
public float Warmth = 100f;
public float Morale = 50f;
public float Health = 100f;
public float Hygiene = 100f;
public float Numbness;
public float RadiationAnxiety;
public bool WasHungerCritical;
public bool WasThirstCritical;
public bool WasWarmthCritical;
public float MaxHealthCap = 100f;
public bool IsAlive = true;
public bool IsDead;
public bool IsAliveState => !IsDead && IsAlive;
public class NeedsProfile
public float hungerPerHour = 0.8f;
public float thirstPerHour = 1.2f;
public float fatiguePerHour = 0.4f;
public float warmthLossPerHourInCold = 0.5f;
public float warmthRestorePerHourNearHeat = 3f;
public float moraleLossPerHourWhileCritical = 1f;
public float healthLossFromHunger = 0.4f;
public float healthLossFromThirst = 0.6f;
public float healthLossFromCold = 0.3f;
public float hungerCritical = 90f;
public float thirstCritical = 90f;
public float warmthCritical = 20f;
public class NeedsSystem
public int CurrentDay { get; set; } = -1;
public NeedsModifierStack ModifierStack { get; } = new NeedsModifierStack();
public Func<float>? HungerRateMultiplier { get; set; }
public Func<float>? ThirstRateMultiplier { get; set; }
public Func<string, float>? ClothingWarmthReductionProvider { get; set; }
public event Action<SurvivorNeedsState, NeedKind, float>? OnNeedChanged;
public event Action<SurvivorNeedsState, NeedsModifierContribution>? OnAttributedContribution;
public event Action<SurvivorNeedsState, NeedKind>? OnNeedCritical;
public event Action<SurvivorNeedsState>? OnDied;
public Func<SurvivorNeedsState, bool>? TryDeferDeath;
public void Register(SurvivorNeedsState survivor) {
public void Unregister(SurvivorNeedsState survivor) {
public bool UnregisterById(string id) {
public int RegisteredCount => _survivors.Count;
public System.Collections.Generic.IReadOnlyList<SurvivorNeedsState> Registered => _survivors;
public SurvivorNeedsState? Get(string id) {
public void Modify(string survivorId, NeedKind need, float delta) {
public void Tick(float gameHours) {
public void Tick(SurvivorNeedsState survivor, float gameHours) {
public void SetExternalModifier(string survivorId, string sourceId, NeedKind need, float deltaPerHour, int priority = 0, int startDay = -1, int endDay = -1) => ModifierStack.Set(survivorId, sourceId, need, deltaPerHour, priority, startDay, endDay);
public bool RemoveExternalModifier(string survivorId, string sourceId, NeedKind need) => ModifierStack.Remove(survivorId, sourceId, need);
public int ClearExternalModifiers(string sourceId) => ModifierStack.ClearSource(sourceId);
public bool ApplyAttributedDelta(string survivorId, NeedKind need, float delta, string sourceId) {
internal void NotifyAttributedContribution(SurvivorNeedsState survivor, NeedsModifierContribution contribution) {
public void Modify(SurvivorNeedsState survivor, NeedKind need, float delta) {
public void ForceDeath(SurvivorNeedsState survivor) {
public void SetHealth(SurvivorNeedsState survivor, float health) {
public void AdjustHealth(SurvivorNeedsState survivor, float delta) {
public void NotifyNeedsRestored(SurvivorNeedsState survivor) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Radiation/RadiationSystem.cs`

### `Assets/Ashfall.Core/Radiation/RadiationSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 485 lines / 21480 bytes.
- SHA-256: `e58c304cc21aebc631f6c74c6f9872adc539d56fc33f8d43eb302bf0ef590989`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=15; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class SurvivorRadState
public string Id = string.Empty;
public float RadiationDose;              // 0..100 current dose (acute scale)
public float LifetimeRadiationExposure;  // unclamped lifetime mSv
public bool HasRadResistance;
public float RadResistanceHoursRemaining;
public float IodineProtectionTimer;
public bool HasAcuteRadiationSickness;
public bool HasChronicIllness;
public bool HasAcuteRadiationSyndrome;
public bool IsAlive = true;
public string LastExposureReason = string.Empty;
public bool HasStatus(SurvivorStatus status) {
public enum SurvivorStatus
public class ExposureContext
public float ZoneRadLevel;
public float ShelterShielding;
public Func<float, float> ShelterRadQuery; // zone -> interior rads/hr
public List<InventoryWornGear> WornGear = new List<InventoryWornGear>();
public string ExposureReason = string.Empty;
public ExposureEnvironment? Environment;
public class Contamination
public float RadsPerHour;
public float DecayPerHour;
public bool IsActive;
public void Decay(float gameHours) {
public float AmbientContribution() {
public class RadiationSystem
public const float AcuteThreshold = 80f;
public const float ChronicLifetimeThreshold = 400f;
public const float HealthLossPerHourAtAcute = 5f;
public const float IodineResistanceHours = 6f;
public const float RadResistanceFactor = 0.5f;
public bool IsPaused { get; set; }
public event Action<SurvivorRadState, float> OnDoseChanged;
public event Action<SurvivorRadState, SurvivorStatus> OnStatusGained;
public event Action<SurvivorRadState, SurvivorStatus> OnStatusLost;
public event Action<SurvivorRadState> OnExposureStarted;
public event Action<SurvivorRadState> OnExposureEnded;
public Func<float>? ExposureRateMultiplier { get; set; }
public void Register(SurvivorRadState survivor) {
public int RegisteredCount => _survivors.Count;
public IReadOnlyList<SurvivorRadState> Registered => _survivors;
public void Unregister(SurvivorRadState survivor) {
public void Tick(float gameHours) {
public static float ComputeGearProtection(IReadOnlyList<InventoryWornGear> worn) {
public static float ComputeEffectiveAmbient(float zoneRadLevel, float shelterShielding) {
public static float ComputeExposurePerHour(float zoneRadLevel, float gearProtection, float shelterShielding) {
public static float ComputeEffectiveRate( float zoneRadLevel, float gearProtection, float shelterShielding, float? interiorRads, float rateMultiplier = 1f) {
public static float ComputeContaminationAmbient(System.Collections.Generic.IEnumerable<Contamination> contaminations) {
public bool IsExposureActive(string survivorId) {
public Dosimeter GetDosimeter(string survivorId) {
public void Expose(SurvivorRadState survivor, float radsPerHour, float hours) {
public void AdministerIodine(SurvivorRadState survivor) {
public const float IodineWindowHours = 24f;
public void AdministerAntiRad(SurvivorRadState survivor, float radsRemoved) {
public void SetDose(SurvivorRadState survivor, float dose) {
public void AdjustDose(SurvivorRadState survivor, float delta) {
public void SeedLifetimeExposure(SurvivorRadState survivor, float lifetime) {
public class Dosimeter
public string SurvivorId = string.Empty;
public float CurrentReading;
public float LifetimeDose;
public string LastExposureReason = string.Empty;
public void Record(float doseRecorded, float hours) {
internal sealed class ReferenceEqualityComparer : IEqualityComparer<object>
public static readonly ReferenceEqualityComparer Instance = new ReferenceEqualityComparer();
public new bool Equals(object? x, object? y) => ReferenceEquals(x, y);
public int GetHashCode(object obj) => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
```


# Appendix B.04 — Current Code Architecture: `src/Host/HoldfastRuntimeSession.cs`

### `src/Host/HoldfastRuntimeSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 763 lines / 32845 bytes.
- SHA-256: `f9755a5dc96e2dd96c5968d5c6fe88c42582ef88dfba04531e406a15f6dde991`.
- Architecture signals: seeded references=0; save/restore symbols=7; typed event declarations=5; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=1; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class HoldfastRuntimeSession
public const long DefaultStartingValue = 100;
public const int MaxHealth = 100;
public const int MaxHunger = 100;
public const int MaxThirst = 100;
public const float RadDamageThreshold = 50f; // mSv/day causes HP loss
public const float StarvationThreshold = 90f; // hunger above this causes HP loss
public const float DehydrationThreshold = 90f; // thirst above this causes HP loss
public CoreDemoSession World { get; }
public HoldfastTradeSession Trade { get; }
public HoldfastCatalog Catalog => World.Catalog;
public string LastPersistenceMessage { get; private set; } = string.Empty;
public bool HasPurchasedThisSession { get; set; }
public string PlayerSurvivorId { get; set; } = "survivor_dr_sarah_chen";
public Ashfall.Core.Inventory.Inventory? Inventory { get; set; }
public Ashfall.Core.Inventory.Inventory? EffectiveInventory =>
public int Health => Survivors?.Find(PlayerSurvivorId) != null
public float Radiation => Survivors != null
public int Hunger => Survivors?.Find(PlayerSurvivorId) != null
public int Thirst => Survivors?.Find(PlayerSurvivorId) != null
public int Day => World.Clock.Day;
public bool IsDead => Health <= 0;
public string DeathCause { get; private set; } = string.Empty;
public bool IsGameWon => World.Quests != null && World.Quests.IsCompleted(HoldfastQuestSystem.Hatch);
public string WinMessage { get; private set; } = string.Empty;
public event Action StateChanged;
public event Action<string> OnPlayerDied; // passes cause of death
public event Action<string> OnGameWon; // passes win message
public static HoldfastRuntimeSession Create( CoreDemoSession world, bool seedDevelopmentState = false, bool loadTradeSave = true, Ashfall.Core.Inventory.Inventory? inventory = null) {
public bool TrySaveToLegacyFiles(string basePathOverride = null!, string tradePathOverride = null!) {
public bool TryReloadFromLegacyFiles(string basePathOverride = null!, string tradePathOverride = null!) {
public void SeedDevelopmentState() {
public string TickDay() {
public bool ConsumeFood(string itemId, int amount = 1) {
public ActionResult ConsumeFoodResult(string itemId, int amount = 1, string? survivorId = null) {
public Action<string, int, string>? FoodConsumed { get; set; }
public bool ConsumeWater(string itemId, int amount = 1) {
public ActionResult ConsumeWaterResult(string itemId, int amount = 1, string? survivorId = null) {
public void ExposeRadiation(float msv) {
public bool UseAntiRad(string itemId, float reduction = 0f) {
public ActionResult UseAntiRadResult(string itemId, string? survivorId = null, float reduction = 0f) {
public ActionResult FeedAllCrewResult(string? itemId = null) {
public string? FindAvailableFoodItemId() {
public string? FindAvailableWaterItemId() {
public string? FindAvailableAntiRadItemId() {
public void Heal(int amount) {
public string VisitLocation(string locationId) {
public string GetQuestSummary() {
public bool ArchiveAndFreshStart(string basePathOverride = null!, string tradePathOverride = null!) {
```


# Appendix B.05 — Current Code Architecture: `src/Main.SaveOrchestrator.cs`

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


# Appendix D.06 — Existing Focused Test Inventory: `Ashfall.Core.Tests/NeedsRadiationSaveRoundTripTests.cs`

### `Ashfall.Core.Tests/NeedsRadiationSaveRoundTripTests.cs`

- Current test declarations: Fact=17, Theory=0, InlineData=0.
- File lines: 613; SHA-256: `94c12f9e934798dbc9869a44609343e1d5309337a8a554e0d27ef0d09a0f7ffe`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
SurvivorNeeds_RoundTrip_PreservesExactValues
SurvivorNeeds_MutationChangesChecksum
NeedsSystem_Tick_CalculatesAccurateDriftAndConsequences
Needs_AllFields_RoundTrip_PreservesEveryStateField
Needs_RestoredState_DrivesSystemTick
Needs_CaptureAfterTick_DiffersFromPriorCapture
Needs_RestoreDefaultState_NoExceptionAndDocumentedDefaults
Needs_SameState_StableChecksumAcrossCaptures
Radiation_DoseAndPhase_RoundTrip_IntoFreshSystem
Radiation_CaptureAfterTick_DiffersFromPriorCapture
Radiation_RestoreDefaultState_NoExceptionAndDocumentedDefaults
Radiation_SameState_StableChecksumAcrossCaptures
NeedsSystem_WithNoRegisteredSurvivors_TickIsNoOp
RadiationSystem_WithNoRegisteredSurvivors_TickIsNoOp
Determinism_Needs_PairedCaptureRestore_YieldsIdenticalTickOutcomes
Determinism_Radiation_PairedCaptureRestore_YieldsIdenticalTickOutcomes
Determinism_CaptureRestore_DoesNotDisturbISeededRngStream
```


# Appendix D.07 — Existing Focused Test Inventory: `Ashfall.Core.Tests/NeedsRadiationSystemTests.cs`

### `Ashfall.Core.Tests/NeedsRadiationSystemTests.cs`

- Current test declarations: Fact=27, Theory=0, InlineData=0.
- File lines: 561; SHA-256: `54b75e92bc8bc01250feb06cef9e351131f81d68147ab168aef1327d1c16e2ac`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Tick_AdvancesHungerThirstFatigue
Tick_NearHeatSource_RestoresWarmth
CriticalHunger_LosesHealthAndFiresEvent
Modify_ClampsToCapAnd100
HealthZero_FiresDied
TryDeferDeath_GatesDeathAtZero
Expose_AccumulatesDoseAndLifetime
Expose_AcuteThreshold_GrantsStatusAndDamagesHealth
ChronicThreshold_OnLifetime_GrantsChronic
AdministerIodine_GrantsTimedResistance_ThatExpires
AdministerAntiRad_LowersDose_KeepsLifetime
Tick_WithContext_AppliesGearProtection
Tick_Paused_AccumulatesNothing
GearProtection_ScalesWithDurability
MathfCompat_MirrorsUnitySemantics
SurvivorNeedsState_RoundTrips_MutatedValues
SurvivorRadState_RoundTrips_MutatedValues
RadiationSystem_RegisteredDoseSurvivesRoundTrip
EquippedGear_ReducesDose_BelowAcuteThreshold
DegradedGear_ProvidesProportionalProtection
OldSave_Deserialization_Equivalence
DoseCalculation_Equivalence_WithConsolidatedWornGear
EquippedInventoryGear_SumsToAuthorityProtection
EquippedInventoryGear_ReducesExposureBelowAcute
NoEquippedGear_StillReachesAcute
SaveLoad_NeedsState_RoundTrip_PreservesAllFields
SaveLoad_RadState_RoundTrip_PreservesAllFields
```


# Appendix D.08 — Existing Focused Test Inventory: `Ashfall.Core.Tests/RadiationPhaseProgressionTests.cs`

### `Ashfall.Core.Tests/RadiationPhaseProgressionTests.cs`

- Current test declarations: Fact=37, Theory=0, InlineData=0.
- File lines: 710; SHA-256: `ab6af539eeb7bd2419c3bc51f4c9ac35844b0acf11a960fe8a58d26a0a1f2b9d`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
OnExposure_BelowTriggerDose_StaysHealthy
OnExposure_AtTriggerDose_TransitionsToProdromal
OnExposure_Prodromal_FiresHealthAndMoraleEvents
OnExposure_Prodromal_FiresDoseResetEvent
Tick_Prodromal_TransitionsToLatentAfterDuration
Tick_Prodromal_OngoingHealthDrain
Tick_Latent_TransitionsToManifestAfterDuration
Tick_Manifest_FiresHealthCrashOnEntry
Tick_Manifest_BedRestReducesBleed
ResolveOutcome_LowLatentDamage_TransitionsToRecoveryOrDeath
ResolveOutcome_HighLatentDamage_ChronicFibrosis
ResolveOutcome_TerminalPrognosis_FiresEvent
AdministerIodine_ReducesAcuteLumpOnTrigger
IodineTimer_DecaysOverTime
AcuteDoseWindow_DecaysOverTime
AcuteDoseWindow_SmallValuesZeroOut
SaveLoad_Roundtrip_PreservesState
SaveLoad_Roundtrip_PreservesChronicFibrosis
SaveLoad_Roundtrip_PreservesTerminalPrognosis
SaveLoad_NullRestore_ResetsToHealthy
SaveLoad_MultipleSurvivors_OrdinalSorted
OnStateChanged_FiresOnExposure
OnStateChanged_FiresOnTick
OnPhaseChanged_FiresOnlyOnActualTransition
OnChronicIllnessRequested_FiresWhenLatentDamageCrossesThreshold
OnExposure_NullSurvivorId_Ignored
OnExposure_ZeroDose_Ignored
OnExposure_DeadSurvivor_Ignored
OnExposure_UnregisteredSurvivor_Ignored
Tick_ZeroHours_NoEffect
Tick_HealthySurvivor_NoEvents
GetPhasePrognosisText_ReturnsCorrectText
GetPhasePrognosisText_UnknownSurvivor_ReturnsUnknown
Unregister_RemovesSurvivor
LungCapacity_Floor_Is20
Determinism_SameSeed_SameOutcome
Determinism_DifferentSeed_DifferentOutcome
```


# Appendix E.09 — Supporting Code Evidence: `src/Main.CampaignOwners.cs`

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


# Appendix E.10 — Supporting Code Evidence: `Assets/Ashfall.Core/ExpansionHubSave.cs`

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


# Appendix E.11 — Supporting Code Evidence: `Assets/Ashfall.Core/YearOfAsh/YearOfAshSave.cs`

### `Assets/Ashfall.Core/YearOfAsh/YearOfAshSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 371 lines / 17304 bytes.
- SHA-256: `bcfc35c7049581fe1513883dda1e95adfc06458f1b8a10eb520ff4ca1e54fc5d`.
- Architecture signals: seeded references=0; save/restore symbols=20; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class YearOfAshSave
public const int CurrentSaveVersion = 5;
public int saveVersion = CurrentSaveVersion;
public int simDay = 180;
public YearOfAshTimelineState timeline = new YearOfAshTimelineState();
public DoorEncounterSystemState encounters = new DoorEncounterSystemState();
public FactionWarSystemState factionWar = new FactionWarSystemState();
public WarlordDoctrineState warlord = new WarlordDoctrineState();
public YearOfAshDeepFreezeState deepFreeze = new YearOfAshDeepFreezeState();
public YearOfAshRadonState radon = new YearOfAshRadonState();
public QuestlineSystemState quests = new QuestlineSystemState();
public FactionWarChainRunnerState factionWarChainRunner = new FactionWarChainRunnerState();
public IceRoadState iceRoad = new IceRoadState();
public string Checksum = string.Empty;
public class YearOfAshSaveV4
public int saveVersion = 4;
public int simDay = 180;
public YearOfAshTimelineState timeline = new YearOfAshTimelineState();
public DoorEncounterSystemState encounters = new DoorEncounterSystemState();
public FactionWarSystemState factionWar = new FactionWarSystemState();
public WarlordDoctrineState warlord = new WarlordDoctrineState();
public YearOfAshDeepFreezeState deepFreeze = new YearOfAshDeepFreezeState();
public YearOfAshRadonState radon = new YearOfAshRadonState();
public QuestlineSystemState quests = new QuestlineSystemState();
public FactionWarChainRunnerState factionWarChainRunner = new FactionWarChainRunnerState();
public string Checksum = string.Empty;
public class YearOfAshSaveV1
public int saveVersion = 1;
public int simDay = 180;
public YearOfAshTimelineState timeline = new YearOfAshTimelineState();
public DoorEncounterSystemState encounters = new DoorEncounterSystemState();
public FactionWarSystemState factionWar = new FactionWarSystemState();
public string Checksum = string.Empty;
public class YearOfAshSaveV2
public int saveVersion = 2;
public int simDay = 180;
public YearOfAshTimelineState timeline = new YearOfAshTimelineState();
public DoorEncounterSystemState encounters = new DoorEncounterSystemState();
public FactionWarSystemState factionWar = new FactionWarSystemState();
public YearOfAshDeepFreezeState deepFreeze = new YearOfAshDeepFreezeState();
public YearOfAshRadonState radon = new YearOfAshRadonState();
public QuestlineSystemState quests = new QuestlineSystemState();
public string Checksum = string.Empty;
public class YearOfAshSaveV3
public int saveVersion = 3;
public int simDay = 180;
public YearOfAshTimelineState timeline = new YearOfAshTimelineState();
public DoorEncounterSystemState encounters = new DoorEncounterSystemState();
public FactionWarSystemState factionWar = new FactionWarSystemState();
public WarlordDoctrineState warlord = new WarlordDoctrineState();
public YearOfAshDeepFreezeState deepFreeze = new YearOfAshDeepFreezeState();
public YearOfAshRadonState radon = new YearOfAshRadonState();
public QuestlineSystemState quests = new QuestlineSystemState();
public string Checksum = string.Empty;
public static class YearOfAshSaveCodec
public static YearOfAshSave Capture( YearOfAshTimelineSystem timeline, DoorEncounterSystem encounters, FactionWarSystem factionWar, IClock clock, YearOfAshDeepFreezeSystem? deepFreeze = null,
public static void Restore( YearOfAshSave save, YearOfAshTimelineSystem timeline, DoorEncounterSystem encounters, FactionWarSystem factionWar, YearOfAshDeepFreezeSystem? deepFreeze = null,
public static string Encode(YearOfAshSave save, IJsonSerializer json) {
public static YearOfAshSave Decode(string jsonText, IJsonSerializer json) {
```


# Appendix E.12 — Supporting Code Evidence: `Assets/Ashfall.Core/HoldfastSave.cs`

### `Assets/Ashfall.Core/HoldfastSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 368 lines / 15600 bytes.
- SHA-256: `f040881d090c261dc109d0674ba2db2372183566b12ce6c772e66112210bdc2f`.
- Architecture signals: seeded references=0; save/restore symbols=23; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class HoldfastSave
public const int CurrentSaveVersion = 5;
public int saveVersion = CurrentSaveVersion;
public int simDay;
public IceRoadSystemState iceRoad = new IceRoadSystemState();
public CensusClaimSystemState census = new CensusClaimSystemState();
public BrineWaterSystemState brineWater = new BrineWaterSystemState();
public HoldfastQuestSystemState quests = new HoldfastQuestSystemState();
public District8DeepCoastState deepCoast = new District8DeepCoastState();
public string Checksum = "";
public class HoldfastSaveV1
public int saveVersion = 1;
public int simDay;
public IceRoadSystemStateV1toV3 iceRoad = new IceRoadSystemStateV1toV3();
public CensusClaimSystemState census = new CensusClaimSystemState();
public string Checksum = "";
public class HoldfastSaveV2
public int saveVersion = 2;
public int simDay;
public IceRoadSystemStateV1toV3 iceRoad = new IceRoadSystemStateV1toV3();
public CensusClaimSystemState census = new CensusClaimSystemState();
public BrineWaterSystemState brineWater = new BrineWaterSystemState();
public string Checksum = "";
public class HoldfastSaveV3
public int saveVersion = 3;
public int simDay;
public IceRoadSystemStateV1toV3 iceRoad = new IceRoadSystemStateV1toV3();
public CensusClaimSystemState census = new CensusClaimSystemState();
public BrineWaterSystemState brineWater = new BrineWaterSystemState();
public HoldfastQuestSystemState quests = new HoldfastQuestSystemState();
public string Checksum = "";
public class HoldfastSaveV4
public int saveVersion = 4;
public int simDay;
public IceRoadSystemState iceRoad = new IceRoadSystemState();
public CensusClaimSystemState census = new CensusClaimSystemState();
public BrineWaterSystemState brineWater = new BrineWaterSystemState();
public HoldfastQuestSystemState quests = new HoldfastQuestSystemState();
public string Checksum = "";
public static class HoldfastSaveCodec
public static HoldfastSave Capture( IceRoadSystem iceRoad, CensusClaimSystem census, BrineWaterSystem brine, HoldfastQuestSystem quests, District8DeepCoastSystem deepCoast,
public static HoldfastSave Capture( IceRoadSystem iceRoad, CensusClaimSystem census, BrineWaterSystem brine, HoldfastQuestSystem quests, IClock clock)
public static HoldfastSave Capture( IceRoadSystem iceRoad, CensusClaimSystem census, BrineWaterSystem brine, IClock clock) {
public static HoldfastSave Capture( IceRoadSystem iceRoad, CensusClaimSystem census, IClock clock) {
public static string Encode(HoldfastSave save, IJsonSerializer json) {
public static HoldfastSave Decode(string jsonText, IJsonSerializer json) {
public static void Restore( HoldfastSave save, IceRoadSystem iceRoad, CensusClaimSystem census, BrineWaterSystem brine, HoldfastQuestSystem quests,
public static void Restore( HoldfastSave save, IceRoadSystem iceRoad, CensusClaimSystem census, BrineWaterSystem brine, HoldfastQuestSystem quests,
public static void Restore( HoldfastSave save, IceRoadSystem iceRoad, CensusClaimSystem census, BrineWaterSystem brine, IClock clock)
public static void Restore( HoldfastSave save, IceRoadSystem iceRoad, CensusClaimSystem census, IClock clock) {
```


# Appendix E.13 — Supporting Code Evidence: `Assets/Ashfall.Core/Verdict/VerdictSave.cs`

### `Assets/Ashfall.Core/Verdict/VerdictSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 272 lines / 12122 bytes.
- SHA-256: `35bd29e9933555d17a90114245b9395ee98ec22077e7d46e2467dd185f603806`.
- Architecture signals: seeded references=0; save/restore symbols=14; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class VerdictSave
public const int CurrentSaveVersion = 4;
public const int MigrationFromVersion = 1;
public int saveVersion = CurrentSaveVersion;
public int simDay;
public MachineLogSystemState machineLog = new MachineLogSystemState();
public ReckoningState reckoning = new ReckoningState();
public EvidenceLedgerState evidence = new EvidenceLedgerState();
public VerdictNpcState npcs = new VerdictNpcState();
public VerdictRadioSystem.VerdictRadioState radio = new VerdictRadioSystem.VerdictRadioState();
public QuestlineSystemState quests = new QuestlineSystemState();
public int censusLastWindowDay = -1;
public VerdictAccusationState accusations = new VerdictAccusationState();
public string Checksum = string.Empty;
public class VerdictSaveV3
public int saveVersion = 3;
public int simDay;
public MachineLogSystemState machineLog = new MachineLogSystemState();
public ReckoningState reckoning = new ReckoningState();
public EvidenceLedgerState evidence = new EvidenceLedgerState();
public VerdictNpcState npcs = new VerdictNpcState();
public VerdictRadioSystem.VerdictRadioState radio = new VerdictRadioSystem.VerdictRadioState();
public QuestlineSystemState quests = new QuestlineSystemState();
public int censusLastWindowDay = -1;
public string Checksum = string.Empty;
public class VerdictSaveV1
public int saveVersion = 1;
public int simDay;
public MachineLogSystemState machineLog = new MachineLogSystemState();
public ReckoningState reckoning = new ReckoningState();
public EvidenceLedgerState evidence = new EvidenceLedgerState();
public int censusLastWindowDay = -1;
public string Checksum = string.Empty;
public class VerdictSaveV2
public int saveVersion = 2;
public int simDay;
public MachineLogSystemState machineLog = new MachineLogSystemState();
public ReckoningState reckoning = new ReckoningState();
public EvidenceLedgerState evidence = new EvidenceLedgerState();
public VerdictNpcState npcs = new VerdictNpcState();
public VerdictRadioSystem.VerdictRadioState radio = new VerdictRadioSystem.VerdictRadioState();
public int censusLastWindowDay = -1;
public string Checksum = string.Empty;
public static class VerdictSaveCodec
public static VerdictSave Capture( int simDay, MachineLogSystem machineLog, ReckoningSystem reckoning, EvidenceLedger evidence, int censusLastWindowDay,
public static string Encode(VerdictSave save, IJsonSerializer json) {
public static bool TryDecode(string json, IJsonSerializer serializer, out VerdictSave save) {
public static void Restore( VerdictSave save, MachineLogSystem machineLog, ReckoningSystem reckoning, EvidenceLedger evidence, VerdictNpcSystem? npcs = null,
```


# Appendix G.14 — Supporting Regression Evidence: `Ashfall.Core.Tests/SurvivorNeedsCharacterizationTests.cs`

### `Ashfall.Core.Tests/SurvivorNeedsCharacterizationTests.cs`

- Current test declarations: Fact=15, Theory=0, InlineData=0.
- File lines: 218; SHA-256: `0a37ea3d62809a556dc0fea7b15212202b453ca312ec33063126002fc5a2d21f`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
SurvivorNeedsState_DefaultsAreCorrect
NeedsSystem_RegisterAndGet
NeedsSystem_UnregisterRemovesSurvivor
NeedsSystem_TickAdvancesAllNeeds
NeedsSystem_TickNearHeatRestoresWarmth
NeedsSystem_CriticalHungerFiresEventAndHurtsHealth
NeedsSystem_DeathAtZeroHealth
NeedsSystem_ForceDeath
NeedsSystem_SetHealthClampsAndFiresEvent
NeedsSystem_AdjustHealthAddsDelta
NeedsSystem_NotifyNeedsRestoredFiresAllEvents
NeedsSystem_TickSkipsDeadSurvivors
NeedsSystem_ModifyById
NeedsSystem_TryDeferDeath
SurvivorNeedsState_MaxHealthCapConstrainsSetHealth
```


# Appendix G.15 — Supporting Regression Evidence: `Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs`

### `Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs`

- Current test declarations: Fact=24, Theory=0, InlineData=0.
- File lines: 434; SHA-256: `37a0ff424b3761dcbce89db5dc1d416459e20e1cbeb789e37e77680385ee4b0f`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CleanRoundTrip_PreservesChecksum
TamperedBond_ChangesChecksum
NullChecksumField_RejectsLoadRatherThanBypassing
CleanRoundTrip_PreservesChecksum
TamperedCleanWater_ChangesChecksum
NullChecksumField_RejectsLoadRatherThanBypassing
CleanRoundTrip_PreservesChecksum
TamperedIntegrity_ChangesChecksum
NullChecksumField_RejectsLoadRatherThanBypassing
CleanRoundTrip_PreservesChecksum
TamperedXp_ChangesChecksum
NullChecksumField_RejectsLoadRatherThanBypassing
CleanRoundTrip_PreservesChecksum
TamperedAffinity_ChangesChecksum
NullChecksumField_RejectsLoadRatherThanBypassing
CleanRoundTrip_PreservesChecksum
TamperedTreaty_ChangesChecksum
NullChecksumField_RejectsLoadRatherThanBypassing
CleanRoundTrip_PreservesChecksum
TamperedPlays_ChangesChecksum
NullChecksumField_RejectsLoadRatherThanBypassing
CleanRoundTrip_PreservesChecksum
TamperedFuel_ChangesChecksum
NullChecksumField_RejectsLoadRatherThanBypassing
```


# Appendix G.16 — Supporting Regression Evidence: `Ashfall.Core.Tests/Integration/Plans46_49_CrossSystemIntegrationTests.cs`

### `Ashfall.Core.Tests/Integration/Plans46_49_CrossSystemIntegrationTests.cs`

- Current test declarations: Fact=12, Theory=0, InlineData=0.
- File lines: 609; SHA-256: `7f5b55f6615d624e4670e8bb6ab7ad04b84b5d91acc3956849e5304512c2085a`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
WorkshopAndEquipmentBridge_WeaponRefurbishment_RestoresCombatReadiness
HeavyWorkshopAndVehicleGarage_PowertrainRebuild_EnablesExpeditionReadiness
RadioIntelligenceAndMap_TriangulatesHiddenDepotLocation
CrowdedSleepingQuartersAndMediation_TracksAffinityDriftAndAccord
SubterraneanHazardsAndRescue_EmergencyClearance_SavesTrappedMiners
FullCampaignSaves_RoundTripCaptureAndRestoreAllFourSubsystems
CrossSystemDeterminism_PairedRunsYieldIdenticalStateSnapshots
ScenarioA_ArmoryScarcityLoop_ExecutesReloadServiceAndSaveRoundtrip
ScenarioB_RadioToExpeditionDiscovery_TriangulatesSOSAndSchedulesExpiry
ScenarioC_SocialPressureFromShelterCapacity_BunkFrictionToPrivateRelief
ScenarioD_DeepStrataEmergency_MitigationAndCaveInRescueLifecycle
ScenarioE_CrossSystemShelterCrisis_DeterministicSimulationAcrossAllFourDomains
```


# Appendix G.17 — Supporting Regression Evidence: `Ashfall.Core.Tests/MusterCurrentSystemsTests.cs`

### `Ashfall.Core.Tests/MusterCurrentSystemsTests.cs`

- Current test declarations: Fact=23, Theory=0, InlineData=0.
- File lines: 316; SHA-256: `2690c5b8c73aa6e16e579aa83092d53858461893e7a31805cad6c07cfe971afa`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ColdCount_ProvenanceRequiresPowerAndShielding
ColdCount_PartialSupplyCannotBroadcastFullCredibility
ColdCount_CompleteRunBroadcastsUncaveated
ColdCount_SaveLoadRoundTrips
Provisioned_ContactOnlyViaUnpromptedHelp
Provisioned_NoTradeOfferedUntilContact
Provisioned_SaveLoadRoundTrips
LongWalk_CircuitAdvancesOnDeparture
LongWalk_EscortImprovesFledgingIntelligence
LongWalk_SaveLoadRoundTrips
ScavengerGuild_OverstripBlacklistsPermanently
ScavengerGuild_UnclaimedSiteCannotBeOverStripped
ScavengerGuild_ClaimTracksSite
ScavengerGuild_SaveLoadRoundTrips
IronRaiders_FortifyLowersRaidWindow
IronRaiders_RaidIsCombatOnly
IronRaiders_SaveLoadRoundTrips
HydroBarons_ApproachCSeizesPlant
HydroBarons_ApproachAOrDFixesCard
HydroBarons_ApproachBImposesReform
HydroBarons_ApproachDiscardableOnceOnly
HydroBarons_QueueAdvancesBeforeResolution
HydroBarons_SaveLoadRoundTrips
```


# Appendix H.18 — Supporting Authority Document: `docs/architecture/TRIAD_GATE_AND_SAVE_OWNERSHIP.md`

### `docs/architecture/TRIAD_GATE_AND_SAVE_OWNERSHIP.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 95 lines / 8367 bytes.
- SHA-256: `7a545bc2dc116cdeb074a0f8576d29d90ff324985af6c59375191cf6789322e3`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix H.19 — Supporting Authority Document: `docs/testing/COVERAGE.md`

### `docs/testing/COVERAGE.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 85 lines / 4659 bytes.
- SHA-256: `dfeaeb1ee23235cee435b354279a56f4451fb43cab7e0f535ed8017bea662bcd`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| vitals, thresholds, caps, alive/dead projection | Survivor needs state | acute/chronic dose and phase evolution | Radiation dose state | Owner emits/reads a typed fact; no mirror state. |
| vitals, thresholds, caps, alive/dead projection | Survivor needs state | DTO capture, campaign envelope, no-survivor fallback | Host save projection | Owner emits/reads a typed fact; no mirror state. |
| vitals, thresholds, caps, alive/dead projection | Survivor needs state | round-trip, checksum, restored-tick and determinism proof | Focused contract suite | Owner emits/reads a typed fact; no mirror state. |
| acute/chronic dose and phase evolution | Radiation dose state | vitals, thresholds, caps, alive/dead projection | Survivor needs state | Owner emits/reads a typed fact; no mirror state. |
| acute/chronic dose and phase evolution | Radiation dose state | DTO capture, campaign envelope, no-survivor fallback | Host save projection | Owner emits/reads a typed fact; no mirror state. |
| acute/chronic dose and phase evolution | Radiation dose state | round-trip, checksum, restored-tick and determinism proof | Focused contract suite | Owner emits/reads a typed fact; no mirror state. |
| DTO capture, campaign envelope, no-survivor fallback | Host save projection | vitals, thresholds, caps, alive/dead projection | Survivor needs state | Owner emits/reads a typed fact; no mirror state. |
| DTO capture, campaign envelope, no-survivor fallback | Host save projection | acute/chronic dose and phase evolution | Radiation dose state | Owner emits/reads a typed fact; no mirror state. |
| DTO capture, campaign envelope, no-survivor fallback | Host save projection | round-trip, checksum, restored-tick and determinism proof | Focused contract suite | Owner emits/reads a typed fact; no mirror state. |
| round-trip, checksum, restored-tick and determinism proof | Focused contract suite | vitals, thresholds, caps, alive/dead projection | Survivor needs state | Owner emits/reads a typed fact; no mirror state. |
| round-trip, checksum, restored-tick and determinism proof | Focused contract suite | acute/chronic dose and phase evolution | Radiation dose state | Owner emits/reads a typed fact; no mirror state. |
| round-trip, checksum, restored-tick and determinism proof | Focused contract suite | DTO capture, campaign envelope, no-survivor fallback | Host save projection | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the obsolete open-issue framing with a maintenance contract and current-source traceability. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Require every new `SurvivorNeedsState` or radiation-state field to be classified as persistence-relevant or transient. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Keep the no-survivor host fallback in the Godot host test surface; Core tests assert only the boundary contract they can reference. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Add a field-addition review checklist and a two-run continuous-versus-interrupted proof template for future changes. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.551 — Additional Current Architecture Evidence: `src/Host/HostCli.PanelTests.cs`

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


# Appendix Q.552 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs`

### `Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 533 lines / 23509 bytes.
- SHA-256: `91c30f7399793a9cdcb3459e8b2fa0aa1168b7185bb403d4421cff14316bcd85`.
- Architecture signals: seeded references=4; save/restore symbols=16; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class SurvivorSocialSaveState
public LeadershipSaveState leadership = new LeadershipSaveState();
public IdeologicalFrictionSaveState friction = new IdeologicalFrictionSaveState();
public RationConflictSaveState ration = new RationConflictSaveState();
public TraumaBondSaveState trauma = new TraumaBondSaveState();
public SkillAtrophySaveState atrophy = new SkillAtrophySaveState();
public ExerciseSystemState exercise = new ExerciseSystemState();
public PersonalBelongingsState belongings = new PersonalBelongingsState();
public sealed class SurvivorSocialReadModel
public string leaderId = string.Empty;
public float leaderStress;
public string designatedSuccessorId = string.Empty;
public string deputyLeaderId = string.Empty;
public List<LeadershipChallengeDTO> leadershipChallenges = new List<LeadershipChallengeDTO>();
public List<Entry> entries = new List<Entry>();
public sealed class Entry
public string survivorId = string.Empty;
public string belief = string.Empty;
public int bondCount;
public string strongestBondPartnerId = string.Empty;
public float strongestBondStrength;
public string resentmentTargetId = string.Empty;
public float resentmentLevel;
public List<string> atrophiedSkills = new List<string>();
public float rationAllocation;
public float perceivedFairness;
public float conditioning;
public sealed class SurvivorSocialCoordinator
public LeadershipSystem Leadership { get; }
public IdeologicalFrictionSystem Friction { get; }
public RationConflictSystem Ration { get; }
public TraumaBondSystem TraumaBond { get; }
public SkillAtrophySystem Atrophy { get; }
public ExerciseSystem Exercise { get; }
public PersonalBelongingsSystem Belongings { get; }
public event Action? OnBelongingsChanged;
public const string BelongingsMoraleSource = "personal_belongings";
public SurvivorRelationsSystem Relations => _relations;
public const string LeadershipCrisisAuraSource = "leadership.crisis_aura";
public const string LeadershipModifierSource = "leadership.morale";
public const string ExerciseFatigueSource = "exercise.workout";
public RationPolicy RationPolicy { get; set; }
public void RegisterBelief(string survivorId, string beliefProfileId) {
public void SetAliveSurvivors(IReadOnlyList<string> aliveIds) {
public void TickDay(int day, IReadOnlyList<SurvivorNeedsState> survivors) {
public WorkoutResult? ExecuteWorkout( string survivorId, WorkoutRoutineType routine, int currentDay, float intensity = 1f, ISeededRng? rng = null)
public void OnSharedHazardEndured(List<string> participantIds, string hazardId) =>
public void OnSurvivorDied(string survivorId) => Leadership.OnSurvivorDied(survivorId);
public void OnSurvivorInjured(string survivorId) => Leadership.OnSurvivorInjured(survivorId);
public void OnCrisisEvent() => Leadership.OnCrisisEvent();
public bool DesignateLeader(string survivorId) => Leadership.DesignateLeader(survivorId);
public bool StepDown(string survivorId) => Leadership.StepDown(survivorId);
public bool DesignateSuccessor(string survivorId) => Leadership.DesignateSuccessor(survivorId);
public bool AppointDeputy(string survivorId) => Leadership.AppointDeputy(survivorId);
public LeadershipChallengeDTO? InitiateLeadershipChallenge(string challengerId, string reason) =>
public bool ResolveLeadershipChallenge(string challengeId, bool challengerWon) =>
public SurvivorSocialReadModel BuildReadModel() {
public SurvivorSocialSaveState CaptureState() {
public void RestoreState(SurvivorSocialSaveState save) {
public string Id => _state.Id;
public bool IsAlive => _state.IsAliveState;
public float Morale => _state.Morale;
public float Health => _state.Health;
public string ExpertDisciplineId => string.Empty;
public void SetSkillBonus(string disciplineId, float bonus) { }
```


# Appendix Q.553 — Additional Current Architecture Evidence: `src/Host/HostCli.Plans122to125.cs`

### `src/Host/HostCli.Plans122to125.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1016 lines / 62435 bytes.
- SHA-256: `2f1c66b881c24627b48bb31ab5b2cd1555bda6ec7bcf4baf2dc8e61a56fad81b`.
- Architecture signals: seeded references=10; save/restore symbols=28; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunPlans122to125SelfTest(string dataDirectory) {
public SofcPowerHostSession Sofc = null!;
public CvdDiamondHostSession Cvd = null!;
public SoundRangingHostSession Sra = null!;
public AmphibiousDraisineHostSession Amb = null!;
public SofcElectrochemistryEngine SofcEngine = null!;
public CvdDiamondSynthesisEngine CvdEngine = null!;
public SoundRangingThreatEngine SraEngine = null!;
public AmphibiousDraisineEngine AmbEngine = null!;
public float RoutedHeatKw;
public float FuelDrawn;
public string DiamondBatchA = "flagship_a";
public string DiamondBatchB = "flagship_b";
public int BatchesConsumed;
public System.Collections.Generic.List<string> CvdTrace = new System.Collections.Generic.List<string>();
public void ForkDay(CampaignRngManager rng, int day) {
public void RunDay(int day) {
public static int RunLateTechMobilitySelfTest(string dataDirectory) {
public static int RunPlans122to125BalanceSoak(string dataDirectory) {
```


# Appendix Q.554 — Additional Current Architecture Evidence: `src/Host/SevenDayDeterministicSmokeTest.cs`

### `src/Host/SevenDayDeterministicSmokeTest.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 622 lines / 34110 bytes.
- SHA-256: `1a71e183eed8e9213203c4d9607bf8a1c622c49dc0ead88132c1fce6613ce081`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=1; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class SevenDayDeterministicSmokeTest
public static int Run(string dataDirectory) {
public int WeatherRollCount;
public WeatherKind FinalWeather;
public int WeatherChangeCount;
public SurvivorNeedsState? SurvivorA;
public SurvivorNeedsState? SurvivorB;
public List<string> MapDiscovered = new List<string>();
public List<string> MapLocked = new List<string>();
public List<string> MapCompleted = new List<string>();
public int WeatherRollCount;
public WeatherKind FinalWeather;
public SurvivorNeedsState? SurvivorA;
public SurvivorNeedsState? SurvivorB;
public List<string> MapDiscovered = new List<string>();
public List<string> MapLocked = new List<string>();
public List<string> MapCompleted = new List<string>();
public string Id = string.Empty;
public float Hunger;
public float Thirst;
public float Fatigue;
public float Warmth;
public float Morale;
public float Health;
public bool IsAlive;
public List<SmokeSurvivorSlice> Survivors = new List<SmokeSurvivorSlice>();
public SmokeRosterSaveState? State;
public string Checksum = string.Empty;
public WorldWeatherState? State;
public string Checksum = string.Empty;
```


# Appendix Q.555 — Additional Current Architecture Evidence: `src/Host/HostCli.OrphanSealWave1.cs`

### `src/Host/HostCli.OrphanSealWave1.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 221 lines / 12781 bytes.
- SHA-256: `174b7636c65361479ed53efd0a106deb10c040389e2dac05b900689fb6e59e5f`.
- Architecture signals: seeded references=5; save/restore symbols=27; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunOrphanSealWave1SelfTest(string dataDirectory) {
```


# Appendix Q.556 — Additional Current Architecture Evidence: `src/Host/Phase0HostSession.cs`

### `src/Host/Phase0HostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1095 lines / 56129 bytes.
- SHA-256: `56eab2884bd11ab7fedfa64542e059d3126ad3d3b977bdbca6ceb41f8559a199`.
- Architecture signals: seeded references=6; save/restore symbols=22; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class Phase0SurvivorEffects
public string survivorId = string.Empty;
public float workEfficiencyMultiplier = 1f;
public float workRefusalHours = 0f;
public float staminaMultiplier = 1f;
public float guiltInsomniaSeverity = 0f;
public float hypervigilance = 0f;
public string moralBranch = "Neutral";
public string radiationPhase = "Healthy";
public float dependencyCraftingPenalty = 0f;
public float dependencyCombatPenalty = 0f;
public string finalWishState = string.Empty;
public string finalWishTitle = string.Empty;
public string finalWishDescription = string.Empty;
public float finalWishDaysRemaining;
public int finalWishStepsDone;
public int finalWishStepsTotal;
public string finalWishCompletionText = string.Empty;
public class Phase0EffectsSaveState
public PhaseProgressionSaveState radiationPhase = new PhaseProgressionSaveState();
public PhantomMemoryEngineState phantom = new PhantomMemoryEngineState();
public GuiltInsomniaSaveState guilt = new GuiltInsomniaSaveState();
public CombatTraumaSaveState combatTrauma = new CombatTraumaSaveState();
public SomaticFlashbackSaveState flashbacks = new SomaticFlashbackSaveState();
public MoralBranchingSaveState moral = new MoralBranchingSaveState();
public TradeSpecialtySaveState tradeSpecialty = new TradeSpecialtySaveState();
public FinalWishSaveState finalWishes = new FinalWishSaveState();
public RespiratoryDegenerationState respiratory = new RespiratoryDegenerationState();
public List<Phase0SurvivorEffects> effects = new List<Phase0SurvivorEffects>();
public float permanentShelterMoraleBuff = 0f;
public sealed class Phase0EffectConsumers
public Action<string, float> ApplyMoraleDelta { get; }
public Action<string, float> ApplyHealthDelta { get; }
public Action<string, float> ApplyFatigueDelta { get; }
public Action<float> ApplyShelterMoraleDelta { get; }
public Action<string, float> ApplyWorkEfficiencyMultiplier { get; }
public Action<string, float> ApplyCraftingPenaltyFactor { get; }
public Action<string, float> ApplyCombatPenaltyFactor { get; }
public Action<string, float> ApplyStaminaDrainMultiplier { get; }
public Action<string, string> FireNarrativeEvent { get; }
public Action<string, string> GrantChronicIllness { get; }
public Action<string> ResetRadiationDose { get; }
public Action<string, float> ApplyWorkRefusalHours { get; }
public IReadOnlyList<string> UnboundRequiredEffects => _unboundRequired;
public static Phase0EffectConsumers NoOp( Action<string, float>? applyMoraleDelta = null, Action<string, float>? applyHealthDelta = null, Action<string, float>? applyFatigueDelta = null, Action<float>? applyShelterMoraleDelta = null, Action<string, float>? applyWorkEfficiencyMultiplier = null,
public static readonly Action<string, float> NoOpMoraleDelta = (_, __) => { };
public static readonly Action<string, float> NoOpHealthDelta = (_, __) => { };
public static readonly Action<string, float> NoOpFatigueDelta = (_, __) => { };
public static readonly Action<float> NoOpShelterMoraleDelta = _ => { };
public sealed class Phase0HostSession
public const int DefaultSeed = 808;
public RadiationPhaseProgression RadiationPhase { get; }
public PhantomMemoryEngine Phantom { get; }
public GuiltInsomniaSystem Guilt { get; }
public CombatTraumaSystem CombatTrauma { get; }
public SomaticFlashbackSystem Flashbacks { get; }
public MoralBranchingSystem Moral { get; }
public ChemicalDependencySystem Dependency { get; }
public TradeSpecialtySystem TradeSpecialty { get; }
public FinalWishSystem FinalWish { get; }
public RespiratoryDegenerationSystem Respiratory { get; }
public Phase0EffectConsumers Consumers { get; set; } = Phase0EffectConsumers.NoOp();
public float PermanentShelterMoraleBuff { get; private set; }
public bool IsInAshZone { get; set; }
public bool IsInFalloutStorm { get; set; }
public bool IsNightTime { get; set; }
public int CurrentDay { get; set; } = 1;
public Func<float> GetFilterHealth;
public IReadOnlyList<Phase0SurvivorEffects> Effects => _effects;
public Phase0SurvivorEffects GetEffects(string survivorId) => GetOrCreateEffects(survivorId);
public string LastEvent { get; private set; } = string.Empty;
public void ValidateConsumers() {
public void LoadTradeSpecialties(string dataDir) {
public void LoadPhantomRules(string dataDir) {
public void LoadFinalWishCatalog(string dataDir) {
public void RegisterDefaultRules() {
public void RegisterSurvivors(IEnumerable<string> ids) {
public void SeedDemoRoster() {
public string ScavengeItem(string survivorId, string itemId) {
public string RaiseNoise(string survivorId) {
public string CraftItem(string survivorId, string professionId, string itemId) {
public string RecordMoralChoice(string survivorId, bool isEmpathyChoice) {
public string RecordGuilt(string survivorId, string sourceId, float severity) {
public string RegisterCombatSurvived(string survivorId) {
public string ConsumeSubstance(string survivorId, string itemId, ChemicalDependencyKind kind) {
public string DeclareTerminalPrognosis(string survivorId, string archetypeId) {
public string AdvanceFinalWish(string survivorId, string stepId) {
public string ApplyInhaler(string survivorId) {
public string TickHour(float gameHours = 1f) {
public string TickDay(int day) {
public string StatusLine() {
public Phase0EffectsSaveState CaptureSave() {
public void RestoreSave(Phase0EffectsSaveState save) {
public int Seed { get; }
public int Next(int min, int max) => _rng.Next(min, max);
public float NextFloat() => _rng.NextFloat();
public double NextDouble() => _rng.NextDouble();
public void BindShelterAssignment(ShelterAssignmentSystem shelterAssignment) {
protected override void UnsubscribeSystemEvents() {
```


# Appendix Q.557 — Additional Current Architecture Evidence: `src/Main.OrphanSealWave1.cs`

### `src/Main.OrphanSealWave1.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 525 lines / 23378 bytes.
- SHA-256: `9486381260760b9e2420d8d033ad67cc7e0c23a7c0afabb120d5368675664c90`.
- Architecture signals: seeded references=5; save/restore symbols=24; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public SurvivorAutonomyHostSession? SurvivorAutonomy => _survivorAutonomy;
public NuclearWinterHostSession? NuclearWinter => _nuclearWinter;
public SeasonalCelebrationHostSession? SeasonalCelebration => _seasonalCelebration;
public CommunicationsHostSession? Communications => _communications;
public ColonyHostSession? Colony => _colony;
public SurvivorEducationHostSession? SurvivorEducation => _survivorEducation;
```


# Appendix Q.558 — Additional Current Architecture Evidence: `src/Host/HostCli.WorldPlaytest.cs`

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


# Appendix Q.559 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/DoseLedgerSave.cs`

### `Assets/Ashfall.Core/DoseLedgerSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 163 lines / 7440 bytes.
- SHA-256: `8aa7f6abe7bef1e206f12773566725e3b38e4bde30bcdbbd12b001565d55bdd7`.
- Architecture signals: seeded references=0; save/restore symbols=10; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class DoseLedgerSave
public const int CurrentSaveVersion = 2;
public const int MigrationFromVersion = 1;
public int saveVersion = CurrentSaveVersion;
public int simDay;
public DoseLedgerSystemState doseLedger = new DoseLedgerSystemState();
public SickListSystemState sickList = new SickListSystemState();
public CohortSystemState cohort = new CohortSystemState();
public VoluntaryRegisterSystemState voluntaryRegister = new VoluntaryRegisterSystemState();
public QuestlineSystemState quests = new QuestlineSystemState();
public string Checksum = string.Empty;
public class DoseLedgerSaveV1
public int saveVersion = 1;
public int simDay;
public DoseLedgerSystemState doseLedger = new DoseLedgerSystemState();
public SickListSystemState sickList = new SickListSystemState();
public CohortSystemState cohort = new CohortSystemState();
public VoluntaryRegisterSystemState voluntaryRegister = new VoluntaryRegisterSystemState();
public string Checksum = string.Empty;
public static class DoseLedgerSaveCodec
public static DoseLedgerSave Capture( int simDay, DoseLedgerSystem doseLedger, SickListSystem sickList, CohortSystem cohort, VoluntaryRegisterSystem voluntaryRegister,
public static string Encode(DoseLedgerSave save, IJsonSerializer json) {
public static DoseLedgerSave Decode(string jsonText, IJsonSerializer json) {
public static void Restore( DoseLedgerSave save, DoseLedgerSystem doseLedger, SickListSystem sickList, CohortSystem cohort, VoluntaryRegisterSystem voluntaryRegister,
```


# Appendix Q.560 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/DutyRoster/DutyRosterSave.cs`

### `Assets/Ashfall.Core/DutyRoster/DutyRosterSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 384 lines / 17537 bytes.
- SHA-256: `7a3bf9432946caed1d300118cabe1b393d45701b46e54c8f5319d69998f0675f`.
- Architecture signals: seeded references=0; save/restore symbols=8; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class DutyRosterSave
public const int CurrentSaveVersion = 3;
public int saveVersion = CurrentSaveVersion;
public int simDay;
public DutyRosterSystemState roster = new DutyRosterSystemState();
public MoraleMarkSystemState marks = new MoraleMarkSystemState();
public ShelterEncounterSystemState encounters = new ShelterEncounterSystemState();
public DutyRosterOverflowState overflow = new DutyRosterOverflowState();
public DutyRosterQuestState quests = new DutyRosterQuestState();
public string Checksum = string.Empty;
public class DutyRosterOverflowState
public bool access;
public List<string> visitedNodes = new List<string>();
public sealed class DutyRosterSaveV1
public int saveVersion = 1;
public int simDay;
public DutyRosterSystemState roster = new DutyRosterSystemState();
public MoraleMarkSystemState marks = new MoraleMarkSystemState();
public ShelterEncounterSystemState encounters = new ShelterEncounterSystemState();
public string Checksum = string.Empty;
public sealed class DutyRosterSaveV2
public int saveVersion = 2;
public int simDay;
public DutyRosterSystemState roster = new DutyRosterSystemState();
public MoraleMarkSystemState marks = new MoraleMarkSystemState();
public ShelterEncounterSystemState encounters = new ShelterEncounterSystemState();
public DutyRosterOverflowState overflow = new DutyRosterOverflowState();
public string Checksum = string.Empty;
internal sealed class LegacyDutyRosterSystemStateChecksum
public string systemId = string.Empty;
public bool expansionUnlocked;
public bool wallInspected;
public string chartScript = string.Empty;
public bool kessPencilAllowed;
public bool waitInk;
public bool blankRowsAccess;
public bool mutationRosterInUse;
public bool mutationRosterStillBlank;
public bool mutationRosterBurned;
public bool mutationRationProtocol;
public string endingId = string.Empty;
public bool secondWinterActive;
public int seedSalt;
public int lastMorningDay;
public int daysLeftBlank;
public int lastBurnDay;
public bool overflowAccess;
public List<string> overflowVisited = new List<string>();
public List<DutyRosterRow> rows = new List<DutyRosterRow>();
public List<DutyRosterAssignmentEntry> assignments = new List<DutyRosterAssignmentEntry>();
public List<DutyRosterPneumaticMemo> pneumaticMemos = new List<DutyRosterPneumaticMemo>();
public List<string> hiddenFromNorth = new List<string>();
public List<string> blankRowsLivingNames = new List<string>();
public static LegacyDutyRosterSystemStateChecksum From(DutyRosterSystemState? state) {
internal sealed class LegacyDutyRosterSaveV1Checksum
public int saveVersion;
public int simDay;
public LegacyDutyRosterSystemStateChecksum roster = new LegacyDutyRosterSystemStateChecksum();
public MoraleMarkSystemState marks = new MoraleMarkSystemState();
public ShelterEncounterSystemState encounters = new ShelterEncounterSystemState();
public string Checksum = string.Empty;
internal sealed class LegacyDutyRosterSaveV2Checksum
public int saveVersion;
public int simDay;
public LegacyDutyRosterSystemStateChecksum roster = new LegacyDutyRosterSystemStateChecksum();
public MoraleMarkSystemState marks = new MoraleMarkSystemState();
public ShelterEncounterSystemState encounters = new ShelterEncounterSystemState();
public DutyRosterOverflowState overflow = new DutyRosterOverflowState();
public string Checksum = string.Empty;
internal sealed class LegacyDutyRosterSaveV3Checksum
public int saveVersion;
public int simDay;
public LegacyDutyRosterSystemStateChecksum roster = new LegacyDutyRosterSystemStateChecksum();
public MoraleMarkSystemState marks = new MoraleMarkSystemState();
public ShelterEncounterSystemState encounters = new ShelterEncounterSystemState();
public DutyRosterOverflowState overflow = new DutyRosterOverflowState();
public DutyRosterQuestState quests = new DutyRosterQuestState();
public string Checksum = string.Empty;
public static class DutyRosterSaveCodec
public static DutyRosterSave Capture( DutyRosterSystem roster, MoraleMarkSystem marks, ShelterEncounterSystem encounters, IClock clock, DutyRosterQuestRuntime? quests = null)
public static string Encode(DutyRosterSave save, IJsonSerializer json) {
public static DutyRosterSave Decode(string jsonText, IJsonSerializer json) {
public static void Restore( DutyRosterSave save, DutyRosterSystem roster, MoraleMarkSystem marks, ShelterEncounterSystem encounters, IClock clock,
```


# Appendix Q.561 — Additional Current Architecture Evidence: `src/Host/SurvivorsHostSession.cs`

### `src/Host/SurvivorsHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 790 lines / 37440 bytes.
- SHA-256: `e7c8e46fdda5f76892830358a67782ee0feb152869b104f21f43000b77061f4f`.
- Architecture signals: seeded references=0; save/restore symbols=5; typed event declarations=4; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class SurvivorSliceState
public string id = string.Empty;
public float hunger;
public float thirst;
public float fatigue;
public float warmth = 100f;
public float morale = 50f;
public float health = 100f;
public float hygiene = 100f;
public float numbness;
public float radiationAnxiety;
public float radiationDose;
public float lifetimeRadiationExposure;
public bool hasRadResistance;
public float radResistanceHoursRemaining;
public float iodineProtectionTimer;
public bool hasAcuteSickness;
public bool hasChronicIllness;
public bool hasAcuteRadiationSyndrome;
public bool radiationIsAlive = true;
public bool wasHungerCritical;
public bool wasThirstCritical;
public bool wasWarmthCritical;
public float maxHealthCap = 100f;
public bool isDead;
public bool isAlive = true;
public string locationKind = "ShelterInterior";
public string locationId = string.Empty;
public sealed class SurvivorsHostSession
public NeedsSystem Needs { get; }
public RadiationSystem Radiation { get; }
public MaterialShieldingSystem Shelter { get; } = new MaterialShieldingSystem();
public System.Collections.Generic.List<SurvivorNeedsState> RosterState { get; } =
public InventoryHostSession Inventory { get; set; }
public ExposureEnvironmentResolver ExposureResolver { get; } = new ExposureEnvironmentResolver();
public string LastEvent { get; private set; } = string.Empty;
public event System.Action<string, Ashfall.Core.Survivors.SurvivorDeathCause, string> OnSurvivorDied;
public event Action<string, float>? OnSurvivorExposed;
public SurvivorRosterSystem Roster { get; } = new SurvivorRosterSystem();
public void LoadStartingRoster(string dataDir, bool failClosed = true) {
public void LoadStartingCohort( StartingCohortProfile profile, bool failClosed = true) {
public void SeedDemoRoster() {
public void LoadCatalog(string dataDir) {
public bool AddSurvivor( string id, string displayName, float health = 100f, float hunger = 0f, float thirst = 0f,
public SurvivorNeedsState? Find(string id) {
public SurvivorRadState? RadStateFor(string id) {
public ExposureEnvironment? GetLastExposureEnvironment(string survivorId) {
public ExposureBreakdown? GetExposureBreakdown(string survivorId) {
public void SetSurvivorLocation(string survivorId, SurvivorExposureLocation kind, string locationId = "") {
public float ApplyAcuteRadDose(string survivorId, float acuteDose, string reason) {
public void BindWeatherProvider(Func<float> weatherRadModifierProvider) {
public void BindLocationRadRateProvider(Func<string, float> locationRadRateProvider) {
public void BindFalloutContaminationProvider(Func<string, float> falloutContaminationProvider) {
public void BindShelterShieldingModel(ShelterShieldingModel? model) {
public ShelterShieldingModel? ShieldingModel { get; private set; }
public ShelterShieldingBreakdown? GetShieldingBreakdown() {
public void BindExpeditionSession(ExpeditionHostSession expeditionSession) {
public void BindHazmatWearMultiplier(Func<float>? multiplierProvider) {
public Ashfall.Core.Inventory.Inventory.ProtectiveLifeEstimate? GetWeakestProtectiveLife() {
public Ashfall.Core.Expeditions.ExpeditionProtectiveInputs? BuildProtectiveEstimateInputs(string locationId) {
public string TickHour(float gameHours = 1f) {
public string AdministerIodine(string survivorId) {
public string AdministerAntiRad(string survivorId, float rads) {
public string HealSurvivor(string survivorId, float amount = 25f) {
public string ExposeToZone(string survivorId, float radsPerHour) {
public string StatusLine() {
public SurvivorsSaveState CaptureSave() {
public void RestoreSave(SurvivorsSaveState save) {
public class SurvivorsSaveState
public System.Collections.Generic.List<SurvivorSliceState> survivors = new System.Collections.Generic.List<SurvivorSliceState>();
public SurvivorRosterState? roster;
```


# Appendix Q.562 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Survivors/NeedsComponentStore.cs`

### `Assets/Ashfall.Core/Survivors/NeedsComponentStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 321 lines / 12054 bytes.
- SHA-256: `59152e857e5b372c3a17b6a476fd80c92b559b5429214c105f81af215251c72d`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class NeedsComponentState
public string survivor_id = string.Empty;
public float hunger;
public float thirst;
public float fatigue;
public float warmth = 100f;
public float morale = 50f;
public float health = 100f;
public float hygiene = 100f;
public bool was_hunger_critical;
public bool was_thirst_critical;
public bool was_warmth_critical;
public float max_health_cap = 100f;
public bool is_alive = true;
public bool is_dead;
internal static NeedsComponentState Capture(SurvivorId owner, SurvivorNeedsState source) {
internal SurvivorNeedsState ToRuntimeState() {
public sealed class NeedsComponentStoreState
public const string CurrentSystemId = NeedsComponentStore.SystemId;
public const int CurrentSchemaVersion = NeedsComponentStore.SchemaVersion;
public int schema_version = CurrentSchemaVersion;
public string system_id = CurrentSystemId;
public List<NeedsComponentState> survivors = new List<NeedsComponentState>();
public sealed class NeedsComponentRestoreReport
public int Accepted { get; internal set; }
public List<string> Rejected { get; } = new List<string>();
public bool IsFatal { get; internal set; }
public string FatalReason { get; internal set; } = string.Empty;
public bool IsClean => !IsFatal && Rejected.Count == 0;
public override string ToString() => IsFatal
public sealed class NeedsComponentStore : ISurvivorComponentStore
public const string SystemId = "needs_component";
public const int SchemaVersion = 1;
public string ComponentName => "needs";
public SurvivorComponentCardinality Cardinality => SurvivorComponentCardinality.ZeroOrOne;
public bool RetainsHistoryAfterDeath => false;
public IEnumerable<SurvivorId> OwnerIds => OrderedOwnerIds();
public int Count => _byOwner.Count;
public bool Contains(SurvivorId owner) => !owner.IsEmpty && _byOwner.ContainsKey(owner);
public bool TryGet(SurvivorId owner, out SurvivorNeedsState? state) {
public bool TryUpsert(SurvivorId owner, SurvivorNeedsState? state, out string error) {
public bool TryRegister(SurvivorId owner, SurvivorNeedsState? state, out string error) => TryUpsert(owner, state, out error);
public bool TryUpsert(SurvivorNeedsState? state, out string error) {
public bool Release(SurvivorId owner) {
public void Reset() => _byOwner.Clear();
public NeedsComponentStoreState CaptureState() {
public NeedsComponentRestoreReport RestoreState(NeedsComponentStoreState? saved) {
```


# Appendix Q.563 — Additional Current Architecture Evidence: `src/Main.ShelterBatch3.cs`

### `src/Main.ShelterBatch3.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 467 lines / 24198 bytes.
- SHA-256: `641492ff2b80e3c6ae9004000c647a0d8401a3a46b16ea5f9300fab2c3d91338`.
- Architecture signals: seeded references=0; save/restore symbols=20; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.564 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Factions/WeightOfChoicesSave.cs`

### `Assets/Ashfall.Core/Factions/WeightOfChoicesSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 188 lines / 9036 bytes.
- SHA-256: `c6a10fc9ec25bcc1b02b9ec098a6cd614d6a4cf60a6fc50fd53809fa574058a4`.
- Architecture signals: seeded references=0; save/restore symbols=8; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class WeightOfChoicesSave
public const int CurrentSaveVersion = 2;
public int saveVersion = CurrentSaveVersion;
public MilitaryBranchSystemState militaryBranch = new MilitaryBranchSystemState();
public RebelBranchSystemState rebelBranch = new RebelBranchSystemState();
public IndependentBranchSystemState independentBranch = new IndependentBranchSystemState();
public PrpfSystemState prpf = new PrpfSystemState();
public string Checksum = string.Empty;
public class WeightOfChoicesSaveV1
public int saveVersion = 1;
public MilitaryBranchSystemState militaryBranch = new MilitaryBranchSystemState();
public RebelBranchSystemState rebelBranch = new RebelBranchSystemState();
public PrpfSystemState prpf = new PrpfSystemState();
public string Checksum = string.Empty;
public static class WeightOfChoicesSaveCodec
public static WeightOfChoicesSave Capture( MilitaryBranchSystem militaryBranch, RebelBranchSystem rebelBranch, IndependentBranchSystem independentBranch, PrpfStandingSystem prpf) {
public static void Restore( WeightOfChoicesSave save, MilitaryBranchSystem militaryBranch, RebelBranchSystem rebelBranch, IndependentBranchSystem independentBranch, PrpfStandingSystem prpf)
public static string Encode(WeightOfChoicesSave save, IJsonSerializer json) {
public static WeightOfChoicesSave Decode(string jsonText, IJsonSerializer json) {
public static bool HasConflictingFactionCommitment(WeightOfChoicesSave save) {
```


# Appendix Q.565 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/SaveChecksum.cs`

### `Assets/Ashfall.Core/SaveChecksum.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 197 lines / 8754 bytes.
- SHA-256: `62333d38efefbeb778aee2ca8af936bb120c5197a2a107a6e6c005c923c23da4`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=1; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class SaveChecksum
public const int MaxDepth = 32;
public const string ChecksumFieldName = "Checksum";
public static string Compute(object root) {
public static string Canonicalize(object root) {
```


# Appendix Q.566 — Additional Current Architecture Evidence: `src/Host/HostCli.EvolvingWorld.cs`

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


# Appendix Q.567 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/StandingRecord/StandingRecordEngine.cs`

### `Assets/Ashfall.Core/StandingRecord/StandingRecordEngine.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 176 lines / 6981 bytes.
- SHA-256: `6a003a2fcbf7ac6ee0891bf4ecc32b19f1b3633895575755f84c2a7e8d84ec66`.
- Architecture signals: seeded references=2; save/restore symbols=13; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class StandingRecordState
public string systemId = StandingRecordEngine.SystemId;
public bool expansionUnlocked;
public int currentDay;
public bool overlayAccess = true;
public LocationLayoutState layout = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState encounters = new SiteEncounterState();
public sealed class StandingRecordEngine
public const string SystemId = "standing_record_system";
public const string FlagExpUnlocked = "exp_standing_record_unlocked";
public StandingRecordState State { get; private set; }
public LocationLayoutSystem Layouts { get; }
public LocationMemorySystem Memory { get; }
public SiteEncounterSystem Encounters { get; }
public void Load(string dataDir) {
public bool IsUnlocked => State.expansionUnlocked;
public int CurrentDay => State.currentDay;
public bool HasOverlayAccess => State.overlayAccess;
public void UnlockExpansion(int currentDay) {
public void Tick(int newDay) {
public bool ApplySiteMutation(string siteId, string mutation) {
public string? GetActiveRecast(string siteId) {
public StandingRecordState CaptureState() {
public void RestoreState(StandingRecordState saved) {
```


# Appendix Q.568 — Additional Current Architecture Evidence: `src/Host/HostCli.AdvancedIndustrialRecon.cs`

### `src/Host/HostCli.AdvancedIndustrialRecon.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 417 lines / 27411 bytes.
- SHA-256: `edf24dbb38c174cd7815bb216c75f86822b6556d77c3373365a9542b397966aa`.
- Architecture signals: seeded references=16; save/restore symbols=22; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunSyntheticLubricantSelfTest(string dataDirectory) {
public static int RunUvCoronaSelfTest(string dataDirectory) {
public static int RunCarbonCompositeSelfTest(string dataDirectory) {
public static int RunGprCartographySelfTest(string dataDirectory) {
public static int RunAdvancedIndustrialReconSelfTest(string dataDirectory) {
public string id = string.Empty;
public bool passed;
public string evidence = string.Empty;
public int schema_version;
public int seed;
public int days;
public bool same_seed_byte_equal;
public bool different_seed_diverged;
public bool midpoint_save_load_equal;
public List<AdvancedIndustrialDaySnapshot> snapshots = new List<AdvancedIndustrialDaySnapshot>();
public List<AdvancedCheck> checks = new List<AdvancedCheck>();
public int day;
public FischerTropschSynthesisState synthesis = new FischerTropschSynthesisState();
public CarbonCompositeState composites = new CarbonCompositeState();
public UvCoronaDetectionState uv = new UvCoronaDetectionState();
public GroundPenetratingRadarState gpr = new GroundPenetratingRadarState();
public InventorySaveState inventory = new InventorySaveState();
public int day;
public float catalyst_condition;
public bool synthesis_active;
public int synthesis_buffer;
public bool composite_active;
public int composite_buffer;
public float autoclave_condition;
public int uv_observations;
public int gpr_observations;
public int gpr_leads;
public int fuel_stock;
public int prepreg_stock;
public int battery_stock;
public int battery_pack_stock;
public float latest_uv_confidence;
public float latest_gpr_confidence;
public float latest_gpr_depth_band;
public string latest_gpr_anomaly_class = string.Empty;
public int Seed { get; }
public bool CatalogsLoaded => _fischer != null && _composites != null && _uv != null && _gpr != null;
public List<AdvancedIndustrialDaySnapshot> Snapshots { get; } = new List<AdvancedIndustrialDaySnapshot>();
public int ProductionCompletions { get; private set; }
public int CompositeCompletions { get; private set; }
public int UvObservations => _uvEngine.State.observations.Count;
public int GprObservations => _gprEngine.State.observations.Count;
public static AdvancedIndustrialReconRun Create(string dataDirectory, int seed) => new AdvancedIndustrialReconRun(dataDirectory, seed);
public void AdvanceTo(int day) {
public void PrepareMidpointSave() {
public AdvancedIndustrialSave CaptureSave() => new AdvancedIndustrialSave
public void RestoreSave(AdvancedIndustrialSave save) {
public bool AllBoundsSane() {
```


# Appendix Q.569 — Additional Current Architecture Evidence: `src/Host/MusterHostSession.cs`

### `src/Host/MusterHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 327 lines / 15896 bytes.
- SHA-256: `927200bbce62aae09560713c2d3a4fb265f2f2581c078344e4c6ad01e3f1a5b5`.
- Architecture signals: seeded references=0; save/restore symbols=21; typed event declarations=4; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MusterHostSession
public MusterSystem Engine { get; }
public CoalitionCampSystem Camp { get; }
public ColdCountSystem ColdCount { get; }
public ProvisionedSystem Provisioned { get; }
public LongWalkSystem LongWalk { get; }
public ScavengerGuildSystem ScavengerGuild { get; }
public IronRaidersSystem IronRaiders { get; }
public HydroBaronsSystem HydroBarons { get; }
public FactionActionBoard Board { get; }
public List<CurrentDefinition> Roster { get; }
public List<WitnessDefinition> Witnesses { get; }
public List<EndingDefinition> Epilogues { get; }
public List<CampSceneDefinition> CampScenes { get; }
public List<FactionCultureEntry> Culture { get; }
public List<string> CampScenesSeen { get; } = new List<string>();
public string LastEvent { get; private set; } = string.Empty;
public RiskBiasTrait AuthorBias { get; private set; } = RiskBiasTrait.Realist;
public event Action<MusterRecord>? OnQuestlineResolved;
public event Action<FactionActionResolutionRecord>? OnActionResolved;
public static MusterHostSession Create(string dataDir) {
public Func<string, bool>? SubjectLivingResolver { get; set; }
public IFactionActionItemSink? ItemSink { get; set; }
public bool ResolveFactionAction(string actionId, string choiceId, int day) {
public CampSceneSelection? StageCampScene(string sceneId, int day) {
public List<WitnessDelivery> DeliverWitnesses(int day, int maxCount = 0) {
public bool IsFlagSet(string flagId) => _board.IsFlagSet(flagId);
public bool IsSubjectAlive(string subjectId) => _isSubjectAlive != null ? _isSubjectAlive(subjectId) : true;
public bool IsFactionPresent(string factionId) =>
public string EndingProseFor(string endingKey) {
public string Escalate(int day) {
public string CycleAuthorBias() {
public string RallyDeserter() {
public string SetStrategy(QuestApproach strategy) {
public string SelectApproach(string questlineId, QuestApproach approach) {
public MusterHostSave CaptureSave() => new MusterHostSave
public void RestoreSave(MusterHostSave save) {
```


# Appendix Q.570 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Accessibility/AccessibilitySettingsSystem.cs`

### `Assets/Ashfall.Core/Accessibility/AccessibilitySettingsSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 321 lines / 13947 bytes.
- SHA-256: `3677981850f1789e93c73ed13a0507f7ccb07cffa78024c0e1d327528def32c5`.
- Architecture signals: seeded references=0; save/restore symbols=12; typed event declarations=13; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class AccessibilityProfileDef
public string profile_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public string colorblind_mode { get; set; } = "None";
public float font_scale { get; set; } = 1.0f;
public bool high_contrast { get; set; }
public bool reduced_motion { get; set; }
public bool screen_reader_friendly { get; set; }
public bool audio_descriptions { get; set; }
public bool visual_audio_alerts { get; set; }
public bool mono_audio { get; set; }
public string subtitle_size { get; set; } = "Medium";
public bool cognitive_load_reduction { get; set; }
public bool auto_walk { get; set; }
public bool aim_assist { get; set; }
public sealed class AccessibilityCatalog
public int schema_version { get; set; } = 1;
public string default_profile_id { get; set; } = "acc_profile_default";
public List<AccessibilityProfileDef> profiles { get; set; } = new List<AccessibilityProfileDef>();
public sealed class AccessibilitySettingsState
public int SchemaVersion { get; set; } = 1;
public string ActiveProfileId { get; set; } = "acc_profile_default";
public string ColorblindMode { get; set; } = "None";
public float FontScale { get; set; } = 1.0f;
public bool HighContrast { get; set; }
public bool ReducedMotion { get; set; }
public bool ScreenReaderFriendly { get; set; }
public bool AudioDescriptions { get; set; }
public bool VisualAudioAlerts { get; set; }
public bool MonoAudio { get; set; }
public string SubtitleSize { get; set; } = "Medium";
public bool CognitiveLoadReduction { get; set; }
public bool AutoWalk { get; set; }
public bool AimAssist { get; set; }
public sealed class AccessibilitySettingsSystem
public event Action<AccessibilitySettingsState>? OnSettingsChanged;
public event Action<string>? OnProfileApplied;
public string ActiveProfileId => _state.ActiveProfileId;
public string ColorblindMode => _state.ColorblindMode;
public float FontScale => _state.FontScale;
public bool HighContrast => _state.HighContrast;
public bool ReducedMotion => _state.ReducedMotion;
public bool ScreenReaderFriendly => _state.ScreenReaderFriendly;
public bool AudioDescriptions => _state.AudioDescriptions;
public bool VisualAudioAlerts => _state.VisualAudioAlerts;
public bool MonoAudio => _state.MonoAudio;
public string SubtitleSize => _state.SubtitleSize;
public bool CognitiveLoadReduction => _state.CognitiveLoadReduction;
public bool AutoWalk => _state.AutoWalk;
public bool AimAssist => _state.AimAssist;
public void LoadCatalog(string json) {
public IReadOnlyList<AccessibilityProfileDef> GetAllProfiles() => _profiles;
public AccessibilityProfileDef? GetProfile(string profileId) {
public bool ApplyProfile(string profileId) {
public void SetColorblindMode(string mode) {
public void SetFontScale(float scale) {
public void SetHighContrast(bool enabled) {
public void SetReducedMotion(bool enabled) {
public void SetVisualAudioAlerts(bool enabled) {
public void SetSubtitleSize(string size) {
public void SetCognitiveLoadReduction(bool enabled) {
public void SetAutoWalk(bool enabled) {
public void SetAimAssist(bool enabled) {
public AccessibilitySettingsState CaptureState() {
public void RestoreState(AccessibilitySettingsState? saved) {
public AccessibilityCensus GetCensus() {
public struct AccessibilityCensus
public string ActiveProfileId { get; }
public int LoadedProfilesCount { get; }
public float FontScale { get; }
public bool HighContrast { get; }
public bool ReducedMotion { get; }
public string ColorblindMode { get; }
public bool ScreenReaderFriendly { get; }
public bool VisualAudioAlerts { get; }
public bool MonoAudio { get; }
public bool AutoWalk { get; }
public bool AimAssist { get; }
public bool CognitiveLoadReduction { get; }
```


# Appendix Q.571 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Survivors/NeedsComponentParity.cs`

### `Assets/Ashfall.Core/Survivors/NeedsComponentParity.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 339 lines / 13859 bytes.
- SHA-256: `3f949031afda896d2601dfd0c6b97abc917b4baba6aedf47ca277cf94af2b88b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class NeedsParityCode
public const string LegacyIdInvalid = "legacy_id_invalid";
public const string LegacyDuplicateId = "legacy_duplicate_id";
public const string TypedIdMismatch = "typed_id_mismatch";
public const string TypedRecordMissing = "typed_record_missing";
public const string TypedRecordExtra = "typed_record_extra";
public const string FieldMismatch = "field_mismatch";
public sealed class NeedsParityFinding
public string Code { get; }
public SurvivorId SurvivorId { get; }
public string RawId { get; }
public string Field { get; }
public string Expected { get; }
public string Actual { get; }
public string Message { get; }
public override string ToString() {
public sealed class NeedsParityReport
public int LegacyRows { get; internal set; }
public int TypedRows { get; internal set; }
public List<NeedsParityFinding> Findings { get; } = new List<NeedsParityFinding>();
public bool IsMatch => Findings.Count == 0;
public int FindingCount => Findings.Count;
public string Describe() {
public override string ToString() => $"[NeedsParity] legacy={LegacyRows} typed={TypedRows} findings={Findings.Count}";
public static class NeedsComponentParity
public static NeedsParityReport Compare( NeedsSystem legacy, NeedsComponentStore typed) {
```


# Appendix Q.572 — Additional Current Architecture Evidence: `src/Main.ShelterSocial.cs`

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


# Appendix Q.573 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Disease/DiseaseHeadlessDemo.cs`

### `Assets/Ashfall.Core/Disease/DiseaseHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 489 lines / 28179 bytes.
- SHA-256: `d985f4110e9c912b9818316dad3b3cf219b2d9b5703fed11e1e5531a51a47b1b`.
- Architecture signals: seeded references=8; save/restore symbols=11; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class DiseaseHeadlessDemo
public const int DemoSeed = 1013;
public static HeadlessReport Run(string? dataDirectory = null, ILog? log = null) {
public string id = string.Empty;
public int schema_version;
public List<DiseaseDemoItemRow> items = new List<DiseaseDemoItemRow>();
```


# Appendix Q.574 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Muster/FactionEcologyHeadlessDemo.cs`

### `Assets/Ashfall.Core/Muster/FactionEcologyHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 198 lines / 11351 bytes.
- SHA-256: `bbb53742f4a2a55e6817a118db2b3dbdfff9ae1571a900006694e4f7a1588d15`.
- Architecture signals: seeded references=0; save/restore symbols=9; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class FactionEcologyHeadlessReport : HeadlessReport
public FactionActionBoardState Board;
public string MusterPath;
public static class FactionEcologyHeadlessDemo
public static FactionEcologyHeadlessReport Run(string? dataDirectory = null, ILog? log = null) {
public bool IsFlagSet(string flagId) => _isFlagSet(flagId);
public bool IsSubjectAlive(string subjectId) => true;
public bool IsFactionPresent(string factionId) => true;
```


# Appendix Q.575 — Additional Current Architecture Evidence: `src/Host/SaveStoreChecksumSelfTest.cs`

### `src/Host/SaveStoreChecksumSelfTest.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 344 lines / 16580 bytes.
- SHA-256: `a1fe315b49443876131fe6b11a6cfb526be58fdfe38e347713abe1915116675c`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class SaveStoreChecksumSelfTest
public static int Run(string dataDirectory) {
```


# Appendix Q.576 — Additional Current Architecture Evidence: `src/Main.ShelterInfrastructure.cs`

### `src/Main.ShelterInfrastructure.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 645 lines / 31304 bytes.
- SHA-256: `5ec59e6a0a93e8e5c67bc7a39c91ff4cca74a518400fd09468938167c23630ea`.
- Architecture signals: seeded references=6; save/restore symbols=16; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public Ashfall.Core.Shelter.ShelterFireHazardSystem ShelterFireHazard => GetShelterFireHazardSystem();
public ShelterFireHostSession? ShelterFireSession => _shelterFireSession;
public Ashfall.Core.Narrative.BunkerGraffitiCatalog GetBunkerGraffitiCatalog() {
public Ashfall.Core.Narrative.BunkerCourtCatalog GetBunkerCourtCatalog() {
public Ashfall.Core.Narrative.BunkerMaintenanceCatalog GetBunkerMaintenanceCatalog() {
public Ashfall.Core.Narrative.PersonalLetterCatalog GetPersonalLetterCatalog() {
public Ashfall.Core.Narrative.AbyssalAnomaliesCatalog GetAbyssalAnomaliesCatalog() {
public string BuildMachineTellText(ISeededRng? rng = null) {
public Ashfall.Core.Shelter.ShelterFireHazardSystem GetShelterFireHazardSystem() {
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.
