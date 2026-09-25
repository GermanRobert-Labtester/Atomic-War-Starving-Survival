# Plan 27 — Dose Justice, Death Inquiry, and Psychological Contamination

> **Rebuild status:** CORE AUTHORITIES PRESENT — CASEWORK AND CROSS-SYSTEM REACHABILITY REMAINS
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

- Dose bands, care plans, dose quests, autopsy procedures and contamination effects already exist. The original proposal overlaps heavily with implemented systems and extensive documentation.
- The remaining work is a consent-aware casework projection: show evidence, uncertainty, appeal/work-clearance consequences, autopsy consent/findings, and contamination recovery without copying source state.
- Any institutional decision must route through existing duty, medical, verdict, memorial and relationship owners. The UI is not a court or diagnosis authority.

**Bounded outcome:** Use `DoseLedgerSystem`, dose registers/plans, `AutopsySystem`, verdict/tribunal surfaces, `PsychologicalContaminationSystem`, trauma/guilt/relationship owners and current decontamination. Do not add a second dose meter, autopsy catalog, sanity meter, justice authority or psychological-contamination simulation.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- Dose registers contain 12 bands, eight plans, guesses, registers and NPCs.
- Autopsy procedures contain 12 procedures with tools, consumables, findings and research unlocks.
- AutopsySystem persists procedures, findings, consent/evidence-related state and research consequences.
- PsychologicalContaminationSystem applies named contamination types, blocks selected actions, supports grounding/rest and persists entries.
- Verdict and memorial/tribunal surfaces already provide institutional and inquiry contexts.

**Master-authority sections applied to this rebase:**

- Volume 7 catalog contracts
- Volume 8 H-C2
- Volume 27 dose/case seeds
- Volume 43 gap audit

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Build a read-only casework projection joining dose, duty, verdict and medical facts.
- Expose consent, uncertainty, conflicting evidence and appeal capacity without inventing legal procedure.
- Connect autopsy outcomes to memorial/inquiry consumers through current owners.
- Treat psychological contamination as a temporary interaction layer, not a second mental-health meter.

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
| cumulative dose, bands and bookings | DoseLedgerSystem | `Assets/Ashfall.Core/DoseLedgerSystem.cs` | Sole dose authority. |
| authored bands/plans and institutional guidance | Dose registers | `Assets/StreamingAssets/Data/dose_registers.json` | Data authority for guidance. |
| procedure, consent, findings and research effects | AutopsySystem | `Assets/Ashfall.Core/AutopsySystem.cs` | Sole autopsy runtime. |
| institutional evidence and judgment | Verdict/tribunal | `Assets/Ashfall.Core/Verdict/VerdictEndingEvaluator.cs; Assets/Ashfall.Core/Verdict/ReckoningSystem.cs; src/UI/JusticeTribunalPanel.cs` | No new justice engine. |
| temporary exposure/action blocks/recovery | PsychologicalContaminationSystem | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` | Interaction layer only. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Dose Justice, Death Inquiry, and Psychological Contamination
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ DoseLedgerSystem
│   cumulative dose, bands and bookings
│ Dose registers
│   authored bands/plans and institutional guidance
│ AutopsySystem
│   procedure, consent, findings and research effects
│ Verdict/tribunal
│   institutional evidence and judgment
│ PsychologicalContaminationSystem
│   temporary exposure/action blocks/recovery
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

1. **Preserve current state ownership.** DoseLedgerSystem owns cumulative dose, bands and bookings: Sole dose authority.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| cumulative dose, bands and bookings | DoseLedgerSystem | `Assets/Ashfall.Core/DoseLedgerSystem.cs` | Sole dose authority. |
| authored bands/plans and institutional guidance | Dose registers | `Assets/StreamingAssets/Data/dose_registers.json` | Data authority for guidance. |
| procedure, consent, findings and research effects | AutopsySystem | `Assets/Ashfall.Core/AutopsySystem.cs` | Sole autopsy runtime. |
| institutional evidence and judgment | Verdict/tribunal | `Assets/Ashfall.Core/Verdict/VerdictEndingEvaluator.cs; Assets/Ashfall.Core/Verdict/ReckoningSystem.cs; src/UI/JusticeTribunalPanel.cs` | No new justice engine. |
| temporary exposure/action blocks/recovery | PsychologicalContaminationSystem | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` | Interaction layer only. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. record authoritative dose/case facts
2. load owned catalogs/records
3. derive uncertainty and evidence projection
4. present consent/capacity choices
5. execute current owner command
6. route duty/medical/verdict/memorial consequences
7. capture owner states

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Dose state remains in dose-ledger save.
- Autopsy case state remains in autopsy save.
- Contamination entries persist per survivor and location.
- Casework UI is derived unless a current owner receives an explicit decision record.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- A dose band is never recalculated in UI.
- Autopsy consent is explicit and refusal is non-punitive except for bounded operational consequences.
- Contamination never creates permanent insanity outside current trauma/mental-health owners.
- Casework decisions name the owning system that persists them.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- No dose/autopsy/contamination row expansion in the architecture tranche.
- Use existing dose quests and narrative records as evidence sources.
- No new legal-code catalog.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use current dose, autopsy, trauma and memorial sections.
- A new case decision requires a named owner and migration.
- Old saves default to no pending case.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Dose is cumulative and deterministic.
- Autopsy findings use current seeded procedure contracts.
- Contamination expiry uses campaign-day state, not wall clock.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Dose band/booking events feed medical and duty views.
- Autopsy completion emits findings/research facts.
- Contamination applies/expires and requests grounding/rest.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Dose/DoseRegisterSurface.cs
- src/Host/AutopsyHostSession.cs
- src/UI/JusticeTribunalPanel.cs
- src/UI/PsychologyArcPanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Inquiry language remains fictional, restrained and evidence-bound.
- Psychological content avoids sensationalism and real-world diagnosis claims.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A UI band disagrees with DoseLedgerSystem. | DoseLedgerSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | Autopsy tools are consumed twice. | Dose registers | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A refused autopsy is treated as guilt. | AutopsySystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | Contamination blocks an action forever. | Verdict/tribunal | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A case decision exists only in UI state. | PsychologicalContaminationSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-06 | A second mental-health scalar is introduced. | DoseLedgerSystem | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/DoseLedgerSystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/AutopsySystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/AutopsyProceduresCatalogTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/PsychologicalContaminationSystemTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/Verdict/Plan93_101VerdictDoseQuestIntegrationTests.cs`
6. `godot --headless --path . -- --data-integrity-selftest` when dose/autopsy data is touched.

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — owner census | Map dose, autopsy, verdict, trauma and memorial authorities. | No parallel case authority. | No production path until the owning implementation package is separately claimed. |
| 1 — dose work-clearance projection | Join dose facts to duty eligibility. | No UI-recomputed bands. | No production path until the owning implementation package is separately claimed. |
| 2 — autopsy consent/inquiry flow | Expose current procedure/finding choices. | Refusal and incomplete evidence are tested. | No production path until the owning implementation package is separately claimed. |
| 3 — contamination integration audit | Verify action blocks, grounding and expiry use current owners. | No permanent parallel meter. | No production path until the owning implementation package is separately claimed. |
| 4 — presentation polish | Improve evidence hierarchy and consent copy. | Accessible and restrained. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/Ashfall.Core/DoseLedgerSystem.cs | READ ONLY | Dose owner |
| Assets/Ashfall.Core/AutopsySystem.cs | READ; MODIFY only for proven case gap | Autopsy owner |
| Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs | READ | Contamination owner |
| src/UI/JusticeTribunalPanel.cs | READ; MODIFY only for truthful case projection | Presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Parallel justice state. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Punitive consent model. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Dose duplication. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Trauma duplication. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Sensational psychological content. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No dose/autopsy catalog expansion.
- No real legal system.
- No sanity meter.
- No diagnosis claims.

# 23. Rollback and Recovery

- Read-only UI changes are reversible.
- New case state requires owner migration and focused save tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- Current catalog counts are explicit.
- Each concern has one owner.
- Consent and uncertainty are testable.
- No second meter or justice engine is proposed.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Build a read-only casework projection joining dose, duty, verdict and medical facts.
- Expose consent, uncertainty, conflicting evidence and appeal capacity without inventing legal procedure.
- Connect autopsy outcomes to memorial/inquiry consumers through current owners.
- Treat psychological contamination as a temporary interaction layer, not a second mental-health meter.

## MUST NOT DO

- No dose/autopsy catalog expansion.
- No real legal system.
- No sanity meter.
- No diagnosis claims.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/DoseLedgerSystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/AutopsySystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/AutopsyProceduresCatalogTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/PsychologicalContaminationSystemTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/Verdict/Plan93_101VerdictDoseQuestIntegrationTests.cs`
6. `godot --headless --path . -- --data-integrity-selftest` when dose/autopsy data is touched.

## FIRST SAFE IMPLEMENTATION STEP

0 — owner census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: cumulative dose, bands and bookings → DoseLedgerSystem; authored bands/plans and institutional guidance → Dose registers; procedure, consent, findings and research effects → AutopsySystem; institutional evidence and judgment → Verdict/tribunal; temporary exposure/action blocks/recovery → PsychologicalContaminationSystem. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 27.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 27 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by DoseLedgerSystem or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/DoseLedgerSystem.cs`

### `Assets/Ashfall.Core/DoseLedgerSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 352 lines / 15416 bytes.
- SHA-256: `8de849230f31198890a51cffd56dd5d740a2a8fa8d1535ae130bac248cfec9fb`.
- Architecture signals: seeded references=1; save/restore symbols=2; typed event declarations=8; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class DoseReading
public int day;
public string source;              // event id or freeform cause
public float nominalMsv;           // what the dial showed
public float bookedMsv;            // what was written after flux/shielding/anti-rad
public bool fluxAmbiguous;         // the reading was a range, not a point
public bool antiRadAfter;          // booked dose reduced post-exposure
public class DoseEntry
public string survivorId;
public float baselineMsv;          // inherited, never zeroed
public float cumulativeMsv;
public string assignedDosimeterTag; // null/empty = not booked going forward
public List<DoseReading> readingsHistory = new List<DoseReading>();
public int radiationPhaseCaught;   // the phase index when the ledger caught it
public float shieldingFactor = 1f;
public int lastAntiRadDay = -1;
public bool hasForgedCleanBill;
public string administrativeClassificationOverride = string.Empty;
public class DoseLedgerSystemState
public string systemId = DoseLedgerSystem.SystemId;
public List<DoseEntry> entries = new List<DoseEntry>();
public float ceilingMsv = 600f;        // the Black threshold
public int readingsSinceLastCalibration;
public bool calibrationOverdue;
public class DoseLedgerSystem
public const string SystemId = "dose_ledger_system";
public const float AmberMsv = 100f;
public const float RedMsv = 300f;
public const float BlackMsv = 600f;
public const int ReadingsPerCalibration = 40;
public const int BandGreen = 0;
public const int BandAmber = 1;
public const int BandRed = 2;
public const int BandBlack = 3;
public event Action<string, float> OnDoseCorrected;       // survivorId, bookedMsv
public event Action<string, int> OnBandReached;           // survivorId, band
public event Action OnLedgerCalibrated;
public event Action<DoseLedgerSystemState> OnStateChanged;
public DoseLedgerSystemState State => _state;
public IReadOnlyList<DoseEntry> Entries => _state.entries;
public bool AssignDosimeter(string survivorId, string tag, float baselineMsv = 0f) {
public void SetShieldingFactor(string survivorId, float factor) {
public void RecordAntiRadTreatment(string survivorId, int day) {
public void Calibrate(string survivorId, int day) {
public DoseBandResult BookReading( string survivorId, int day, float nominalMsv, string source, bool highEnergyEvent,
public static int BandFor(float mSv) {
public void SetForgedCleanBill(string survivorId, bool hasForged) {
public void SetAdministrativeClassificationOverride(string survivorId, string overrideBand) {
public int GetAdministrativeBand(string survivorId) {
public DoseEntry? GetEntry(string survivorId) =>
public float GetCumulative(string survivorId) =>
public DoseLedgerSystemState CaptureState() {
public void RestoreState(DoseLedgerSystemState saved) {
public int ApplyRetention(Records.RetentionPolicyCatalog? catalog) {
public enum DoseBandResult
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/AutopsySystem.cs`

### `Assets/Ashfall.Core/AutopsySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 245 lines / 9957 bytes.
- SHA-256: `46013e6ddbea2db195799e2ba47f2184edecaa241a7f8e2a84e98b167ebdf261`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class AutopsyState
public string systemId = AutopsySystem.SystemId;
public List<AutopsyCase> cases = new List<AutopsyCase>();
public List<string> completedSpecimenIds = new List<string>();
public sealed class AutopsyProcedure
public string procedure_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public List<string> requiredTools { get; set; } = new List<string>();
public List<string> requiredConsumables { get; set; } = new List<string>();
public float airborneRisk { get; set; } = 0.1f;
public float pathogenRisk { get; set; } = 0.05f;
public int procedureHours { get; set; } = 4;
public List<string> possibleFindings { get; set; } = new List<string>();
public List<string> researchUnlocks { get; set; } = new List<string>();
public sealed class AutopsyCase
public string caseId = string.Empty;
public string specimenId = string.Empty;   // deceased survivor ID
public string procedureId = string.Empty;
public string assignedMedicId = string.Empty;
public int dayStarted = -1;
public float progressHours;
public AutopsyStatus status;
public string finding = string.Empty;
public bool containmentBreach;
public List<string> sideEffects = new List<string>();
public enum AutopsyStatus { Queued, InProgress, Complete, Failed, ContainmentBreach } public sealed class AutopsySystem { public const string SystemId = "autopsy"; private AutopsyState _state = new AutopsyState(); private readonly Dictionary<string, AutopsyProcedure> _catalog = new Dictionary<string, AutopsyProcedure>(StringComparer.Ordinal); private readonly ISeededRng _rng; private readonly ILog _log; private readonly Inventory.Inventory _inventory; private readonly RadiationSystem _radiation; private readonly VentilationSystem _ventilation; private readonly ResearchSystem _research; private readonly MedicalWardSystem _medical; private int _currentDay; public AutopsyState State => _state; /// <summary>Owned ventilation subsystem; host code subscribes to its /// hazard warnings without the Core layer depending on audio.</summary> public VentilationSystem Ventilation => _ventilation; public event Action<AutopsyCase> OnCaseCompleted; public event Action OnAutopsyChanged; public AutopsySystem( ISeededRng rng, Inventory.Inventory inventory, RadiationSystem radiation, VentilationSystem ventilation, ResearchSystem research, MedicalWardSystem medical, ILog? log = null) { _rng = rng ?? throw new ArgumentNullException(nameof(rng)); _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory)); _radiation = radiation ?? throw new ArgumentNullException(nameof(radiation)); _ventilation = ventilation ?? throw new ArgumentNullException(nameof(ventilation)); _research = research ?? throw new ArgumentNullException(nameof(research)); _medical = medical ?? throw new ArgumentNullException(nameof(medical)); _log = log ?? NullLog.Instance; }
public void LoadCatalog(List<AutopsyProcedure> procedures) {
public ActionResult QueueAutopsy(string specimenId, string procedureId, string medicId) {
public ActionResult BeginAutopsy(string caseId) {
public void TickDay(int day) {
public List<AutopsyCase> GetActiveCases() => _state.cases.FindAll(c => c.status == AutopsyStatus.InProgress);
public AutopsyState CaptureState() => CloneState(_state);
public void RestoreState(AutopsyState saved) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs`

### `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 323 lines / 14240 bytes.
- SHA-256: `503990509e718c20d205ca06fb2b9998adf038cc3949fa69e0374fb645335c50`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=10; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class PsychologicalContaminationSystem
public const string Contam_ThousandYardStare = "contam_thousand_yard_stare";
public const string Contam_DisgustCascade = "contam_disgust_cascade";
public const string Contam_PhantomSmell = "contam_phantom_smell";
public const string Contam_ChildCotTrauma = "contam_child_cot_trauma";
public static readonly Dictionary<string, string[]> LocationContaminations = new Dictionary<string, string[]>(StringComparer.Ordinal) {
public const int StareDurationDays = 3;
public const int DisgustDurationDays = 2;
public const int PhantomSmellDurationDays = 5;
public const int ChildCotDurationDays = 4;
public static readonly string[] StareBlockedActions = { "action_teach_child", "action_tell_stories" };
public static readonly string[] DisgustBlockedActions = { "action_cook", "action_tend_hydroponics" };
public static readonly string[] ChildCotBlockedActions = { "action_teach_child", "action_comfort_child" };
public event Action<string, string> OnContaminationApplied;
public event Action<string, string> OnContaminationExpired;
public event Action<string> OnMentalBreakFromContamination;
public event Action<string, string> OnMoralChronicleEntry;
public void ApplyContamination(string survivorId, string locationId, float moraleAtVisit, string? survivorArchetype = null) {
public bool HasContamination(string survivorId, string type) {
public bool IsActionBlocked(string survivorId, string actionId) {
public static readonly string[] RestrainedDreadTexts = {
public int GetStage(string survivorId, string currentAssignment = "") {
public string GetUIStatusTag(string survivorId, string currentAssignment = "") {
public bool GroundSurvivor(string survivorId, string companionId, float bondStrength = 60f) {
public void ApplyShelterRest(string survivorId, float daysRest = 1f) {
public IReadOnlyList<ContaminationEntry>? GetContaminations(string survivorId) => _bySurvivor.TryGetValue(survivorId, out var list) ? list : null;
public void Tick(float gameDays, string survivorId, float currentMorale, string currentAssignment) {
public PsychContaminationSave CaptureState() {
public void RestoreState(PsychContaminationSave save) {
public class ContaminationEntry
public string Type = string.Empty;
public string LocationId = string.Empty;
public float DaysRemaining;
public float MoraleAtExposure;
public class PsychContaminationSave
public ContaminationSurvivorSave[] Survivors;
public class ContaminationSurvivorSave
public string SurvivorId = string.Empty;
public ContaminationEntrySave[] Entries;
public class ContaminationEntrySave
public string Type = string.Empty;
public string LocationId = string.Empty;
public float DaysRemaining;
public float MoraleAtExposure;
```


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/DecontaminationSystem.cs`

### `Assets/Ashfall.Core/DecontaminationSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 650 lines / 29249 bytes.
- SHA-256: `884ecd58278abc58a533f97c2a08842fa461dbc9b46589d350469cc951464e1a`.
- Architecture signals: seeded references=3; save/restore symbols=2; typed event declarations=14; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DecontaminationState
public string systemId = DecontaminationSystem.SystemId;
public List<DeconCase> queue = new List<DeconCase>();
public DeconCase? activeCase;
public bool shelterContaminated;
public float shelterContaminationLevel;
public List<DeconIncident> incidentLog = new List<DeconIncident>();
public float effluentTankVolume;
public float effluentTankContamination;
public float effluentTankCapacity = 200f;
public float effluentFilterRemainingLiters = 500f;
public bool effluentFilterInstalled;
public float effluentSludgeVolume;
public bool manualOverrideEngaged;
public List<DeconIncident> overrideLog = new List<DeconIncident>();
public List<string> disposedGearIds = new List<string>();
public sealed class DeconCase
public string caseId = string.Empty;
public string survivorId = string.Empty;
public string gearId = string.Empty;
public float surfaceContamination;         // 0-1 (surface dust only, NOT lifetime dose)
public float radiationDoseBeforeDecon;
public DeconStatus status;
public float progress;
public int queuedDay = -1;
public int startDay = -1;
public int completeDay = -1;
public bool bypassed;
public string outcome = string.Empty;
public string protocolId = string.Empty;
public int currentStageIndex;
public int totalStages;
public string currentStageId = string.Empty;
public int stageTicksRemaining;
public float waterConsumedThisCycle;
public float chelatorConsumedThisCycle;
public float surfactantConsumedThisCycle;
public float radiometricGateReading;
public enum DeconStatus { Queued, InProgress, Complete, Bypassed, Failed, RewashRequired, GearDisposalRequired, QuarantineRequired } [Serializable] public sealed class DeconIncident { public int day; public string caseId = string.Empty; public string description = string.Empty; }
public sealed class DeconStageResult
public bool stageComplete;
public bool cycleComplete;
public string stageId = string.Empty;
public string nextStageId = string.Empty;
public string stageDisplayName = string.Empty;
public string nextStageDisplayName = string.Empty;
public int ticksRemaining;
public float surfaceContamination;
public float radiometricGateReading;
public string outcome = string.Empty;
public string error = string.Empty;
public sealed class DecontaminationSystem
public const string SystemId = "decontamination";
public const float SafeReleaseSurfaceDelta = -0.8f;
public const float SafeReleaseShelterDelta = -0.05f;
public const float BypassSurfaceDelta = -0.1f;
public const float BypassShelterDelta = 0f;
public DecontaminationState State => _state;
public bool HasActiveCase => _state.activeCase != null && _state.activeCase.status == DeconStatus.InProgress;
public event Action<DeconCase> OnCaseCompleted;
public event Action OnDeconChanged;
public ActionResult Enqueue(string survivorId, string gearId, float surfaceContamination) {
public ActionResult ProcessQueue() {
public ActionResult CompleteCycle(bool safeRelease) {
public void TickDay(int day) {
public ActionResult StartProtocolCycle(string protocolId, string survivorId, string gearId, float surfaceContamination, float operatorSkill = 0.5f) {
public DeconStageResult TickActiveStage(float operatorSkill = 0.5f) {
public ActionResult EngageManualOverride() {
public ActionResult DisposeContaminatedGear(string gearId) {
public bool ShouldDisposeGear(float contaminationLevel) {
public ActionResult TreatEffluent() {
public ActionResult InstallEffluentFilter() {
public bool CanOpenInnerDoor() {
public string InnerDoorFailureReason() {
public IReadOnlyList<DeconProtocolDef> Protocols => _protocolCatalog.protocols;
public DeconProtocolDef? FindProtocol(string protocolId) {
public DeconProtocolCatalog ProtocolCatalog => _protocolCatalog;
public DecontaminationState CaptureState() => CloneState(_state);
public void RestoreState(DecontaminationState saved) {
```


# Appendix B.06 — Current Code Architecture: `src/Dose/DoseRegisterSurface.cs`

### `src/Dose/DoseRegisterSurface.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 380 lines / 16237 bytes.
- SHA-256: `8a2d71f77334c11072827892c266bf565b3ff248d4ef935925032abe52afb07d`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class DoseRegisterSurface : PanelContainer
public override void _Ready() {
public void BindSession(DoseLedgerHostSession session) {
public override void _ExitTree() {
internal string ContentTabText => _lblContent?.Text ?? string.Empty;
internal bool ContentTabScrollable => _lblContent?.GetParent() is ScrollContainer;
public void RefreshView() {
```


# Appendix B.07 — Current Code Architecture: `src/Host/AutopsyHostSession.cs`

### `src/Host/AutopsyHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 101 lines / 3742 bytes.
- SHA-256: `450915d45eacda93a7544dcdbb322c08d5077ed441256fbe456493bfd2deb8ff`.
- Architecture signals: seeded references=1; save/restore symbols=1; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class AutopsyHostSession
public AutopsySystem System { get; }
public string LastEvent { get; private set; } = string.Empty;
public ActionResult QueueCase(string specimenId, string procedureId, string medicId, int currentDay) {
public ActionResult BeginAutopsy(string caseId) {
public void LoadCatalog(string dataDir) {
public void TickDay(int day) {
public override void Save() {
```


# Appendix B.08 — Current Code Architecture: `src/UI/JusticeTribunalPanel.cs`

### `src/UI/JusticeTribunalPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 223 lines / 10772 bytes.
- SHA-256: `188aaecc8e590cd4682afc4bda00361762ec3e4a255d8b74db7570cb1ad0d366`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=6; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class JusticeTribunalPanel : Control, IBindablePanel
public event Action? OnClose;
public event Action<string, string>? OnActionRequested;
public bool IsBound => _system != null;
public void Bind(JusticeSystem system) { _system = system; _feedbackText = string.Empty; RefreshView(); }
public void Unbind() { _system = null; }
public override void _Ready() {
public void Open() { Visible = true; RefreshView(); }
public void Close() {
public string LastFeedback { get; private set; } = string.Empty;
public void ShowFeedback(string message, bool isFailure) {
public void RefreshView() {
public override void _UnhandledInput(InputEvent @event) {
```


# Appendix B.09 — Current Code Architecture: `src/UI/PsychologyArcPanel.cs`

### `src/UI/PsychologyArcPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 331 lines / 14473 bytes.
- SHA-256: `79ccdb1dfe19626f9d2e62bd48610bec967303ec257ae8f46316e92f33b6bab1`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=5; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class PsychologyArcPanel : Control
public event Action? OnClose;
public event Action<string, string>? OnActionRequested;
public bool IsBound => _host != null;
public void Bind(PsychologyArcHostSession session, Func<NeedsSystem?>? needsProvider) {
public override void _Ready() {
public void RefreshView() {
public void Open() {
public void Close() {
public override void _UnhandledInput(InputEvent @event) {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/dose_registers.json`

### `Assets/StreamingAssets/Data/dose_registers.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 6225 bytes / 6225 characters.
- SHA-256: `eec0f33b433f4ae15549f3fc4a61cdde840aba6707c870fc65c6608a69820b08`.
- Root keys: `bands`, `calibration`, `guesses`, `npcs`, `plans`, `registers`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
bands: min=12, max=12, observed_paths=1
guesses: min=3, max=3, observed_paths=1
npcs: min=4, max=4, observed_paths=1
plans: min=8, max=8, observed_paths=1
registers: min=4, max=4, observed_paths=1
```

Representative record fields:

- `disposition`
- `id`
- `label`
- `threshold_msv`

Representative identifiers (ordered, capped for readability):

```text
band_green
band_white
band_yellow
band_amber
band_orange
band_rose
band_red
band_crimson
band_violet
band_black
band_indigo
band_void
```


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/dose_items.json`

### `Assets/StreamingAssets/Data/dose_items.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 5341 bytes / 5341 characters.
- SHA-256: `b4711e702815be948ed7384c738df9265b528cb7aa6e96adecbc1e373d4651bf`.
- Root keys: `items`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
items: min=15, max=15, observed_paths=1
```

Representative record fields:

- `category`
- `description`
- `id`
- `name`
- `tradeValue`
- `weightKg`

Representative identifiers (ordered, capped for readability):

```text
item_dose_ledger
item_calibration_key
item_dosimeter_tag
item_palliative_morphine
item_cohort_first_board
item_calibrated_dosimeter
item_forged_clean_bill_chit
item_chelation_decorporation_course
item_shielded_badge_case
item_pocket_dosimeter
item_radiation_survey_meter
item_dose_register_book
item_cohort_baseline_card
item_shielding_apron
item_potassium_iodide_pack
```


# Appendix C.12 — Catalog Census: `Assets/StreamingAssets/Data/autopsy_procedures.json`

### `Assets/StreamingAssets/Data/autopsy_procedures.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 7566 bytes / 7566 characters.
- SHA-256: `ee08b054769678c94cdc5cfd4d398987ff51c8e42452af29750abc3b388559cb`.
- Root keys: `collection_id`, `procedures`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
procedures: min=12, max=12, observed_paths=1
procedures[].possible_findings: min=2, max=3, observed_paths=2
procedures[].required_consumables: min=2, max=2, observed_paths=2
procedures[].required_tools: min=2, max=3, observed_paths=2
procedures[].research_unlocks: min=1, max=1, observed_paths=2
```

Representative record fields:

- `airborne_risk`
- `display_name`
- `pathogen_risk`
- `possible_findings`
- `procedure_hours`
- `procedure_id`
- `required_consumables`
- `required_tools`
- `research_unlocks`


# Appendix C.13 — Catalog Census: `Assets/StreamingAssets/Data/narrative/rad_pathology_autopsy_records.json`

### `Assets/StreamingAssets/Data/narrative/rad_pathology_autopsy_records.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 5967 bytes / 5967 characters.
- SHA-256: `ca01b0b19e8af637f3c80d0c5d6b8583002e9bdec02667349ae9f1b66472462d`.
- Root keys: `items`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
items: min=8, max=8, observed_paths=1
items[].tags: min=5, max=5, observed_paths=2
```

Representative record fields:

- `anatomical_region`
- `case_number`
- `cause_of_death`
- `estimated_dose_rads`
- `id`
- `prose`
- `tags`
- `timestamp_relative`

Representative identifiers (ordered, capped for readability):

```text
pathology_autopsy_bone_marrow_aplasia
pathology_autopsy_gastrointestinal_denudation
pathology_autopsy_pulmonary_fibrotic_consolidation
pathology_autopsy_cerebrovascular_edema
pathology_autopsy_beta_burn_stratum_corneum_necrosis
pathology_autopsy_thyroid_hypertrophy_radioiodine
pathology_autopsy_hepatic_microvascular_thrombosis
pathology_autopsy_lymphatic_system_atrophy
```


# Appendix D.14 — Existing Focused Test Inventory: `Ashfall.Core.Tests/DoseLedgerSystemTests.cs`

### `Ashfall.Core.Tests/DoseLedgerSystemTests.cs`

- Current test declarations: Fact=12, Theory=0, InlineData=0.
- File lines: 201; SHA-256: `e983e95d8bc0ef4c41532ae7d17735990ffe850c3259a92b892e4b0d9ac069bc`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
BookReading_WithoutTag_IsNotBooked
BookReading_CrossesAmberBand_AndFiresEvent
AntiRadAfter_ReducesBookedDose
FluxAmbiguity_IsDeterministicPerSeed
CaptureRestore_RoundTrips
ForgedCleanBill_ProvidesGreenBandAdministratively_WithoutMutatingPhysicalDose
AdminOverrideBand_ChangesAdminBand_PreservesBookedDose
CaptureRestore_PreservesForgedAndOverrideState
TamperedSave_IsHardRejectedOnDecode
ChecksumlessSave_IsRejectedOnDecode
BookChild_ThenCorrectBaseline
Volunteer_SignAndComplete_BanksDose
```


# Appendix D.15 — Existing Focused Test Inventory: `Ashfall.Core.Tests/AutopsySystemTests.cs`

### `Ashfall.Core.Tests/AutopsySystemTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 138; SHA-256: `b485137e5f68596dbc6f5c49ecc3f5ae3114f3448f918b5cb2314d2e08f4a558`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
BeginAutopsy_ConsumableMissing_LeavesToolsInInventory
```


# Appendix D.16 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Medical/AutopsyProceduresCatalogTests.cs`

### `Ashfall.Core.Tests/Medical/AutopsyProceduresCatalogTests.cs`

- Current test declarations: Fact=12, Theory=0, InlineData=0.
- File lines: 307; SHA-256: `d157de51bf7d1b1291e081df493a65cb1189c838320e79690db927caa4d022ee`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsExact12Procedures
Catalog_OriginalProceduresPreserved
Catalog_AllTwelveIdsUniqueAndPrefixed
Catalog_AllDisplayNamesNonEmptyAndUnique
Catalog_AllRequiredToolsResolveInItemsJson
Catalog_AllRequiredConsumablesResolveInItemsJson
Catalog_AllResearchUnlocksResolveInKnowledgeCatalog
Catalog_AllRisksAndDurationsWithinValidRanges
Catalog_PossibleFindingsNonEmptyAndUniquePerProcedure
Runtime_AutopsySystemQueueAndBeginConsumesSupplies
Runtime_AutopsySystemCompletionYieldsFindingAndResearchUnlock
Runtime_SaveLoadPreservesCompletedSpecimensAndCases
```


# Appendix D.17 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Verdict/Plan93_101VerdictDoseQuestIntegrationTests.cs`

### `Ashfall.Core.Tests/Verdict/Plan93_101VerdictDoseQuestIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 217; SHA-256: `4fc5a94aeab9132f1282222f25b67d05edb8cb60f9a05cfee8b41b23b505087e`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plan93_VerdictNpcCatalog_LoadsAll18Npcs_WithValidGatingAndDialogue
Plan101_DoseQuestCatalog_LoadsAll12Questlines_WithValidTransitions
CrossSystem_VerdictArchivistsAndDosimetryQuests_ExhibitNarrativeCoherence
CrossSystem_DeterministicExecution_UnderSimulationPasses
```


# Appendix E.18 — Supporting Code Evidence: `Assets/Ashfall.Core/DoseLedgerSave.cs`

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


# Appendix E.19 — Supporting Code Evidence: `src/Main.Phase0.cs`

### `src/Main.Phase0.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 487 lines / 20350 bytes.
- SHA-256: `17e1d2026b7034eec7ffe613a720851aed49bcb82b064c25d44ebe1b80c7f8dd`.
- Architecture signals: seeded references=0; save/restore symbols=6; typed event declarations=0; textual Godot mentions=8; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix E.20 — Supporting Code Evidence: `src/Host/DoseLedgerHostSession.cs`

### `src/Host/DoseLedgerHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 297 lines / 14982 bytes.
- SHA-256: `0e8180d60ce3225f3722ee704d8c6d6d2bfcbaee29b8a0510044e536fb551150`.
- Architecture signals: seeded references=8; save/restore symbols=2; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=1; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DoseLedgerHostSession
public const int DemoSeed = 1401;
public DoseLedgerSystem Ledger { get; }
public SickListSystem SickList { get; }
public CohortSystem Cohort { get; }
public VoluntaryRegisterSystem Voluntary { get; }
public DoseRegistersCatalog Registers { get; }
public DoseContentCatalog Content { get; }
public QuestlineSystem Quests { get; }
public DosimeterCalibrationSystem Calibration { get; }
public static DoseLedgerHostSession Create(string dataDir, ILog log = null!, ICampaignRngManager? campaignRng = null) {
public DoseLedgerSave CaptureSave(int simDay) =>
public void RestoreSave(DoseLedgerSave save) =>
public void SealDemoSurvivors() {
public string StartCalibration(string deviceTag, int currentDay) {
public string StartCalibrationDemo(string deviceTag, int currentDay) => StartCalibration(deviceTag, currentDay);
public string CompleteCalibration(string deviceTag, int currentDay) {
public string CompleteCalibrationDemo(string deviceTag, int currentDay) => CompleteCalibration(deviceTag, currentDay);
public string ReplaceBattery(string deviceTag) {
public string ReplaceBatteryDemo(string deviceTag) => ReplaceBattery(deviceTag);
public string ServiceSensor(string deviceTag) {
public string ServiceSensorDemo(string deviceTag) => ServiceSensor(deviceTag);
public string CalibrationStatusLine(string deviceTag) {
public string ScribeReading(float nominalMsv, bool highEnergy) {
public Func<int>? DayProvider { get; set; }
public FalloutWindowProvider? FalloutWindowProviderRef { get; set; }
public string DiagnoseDemo(int band) {
public string BookDemoChild() {
public string SignDemoVolunteer() {
public int RegisterContentQuests(QuestlineSystem questSystem) {
public string ContentStatusLine() {
public string LedgerLine() {
public string DoseStatusLine() {
internal sealed class CoreSeededRng : ISeededRng
public int Seed { get; }
public int Next(int min, int max) => _rng.Next(min, max);
public float NextFloat() => _rng.NextFloat();
public double NextDouble() => _rng.NextDouble();
```


# Appendix E.21 — Supporting Code Evidence: `Assets/Ashfall.Core/Verdict/VerdictSave.cs`

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


# Appendix E.22 — Supporting Code Evidence: `Assets/Ashfall.Core/Verdict/VerdictAccusationSystem.cs`

### `Assets/Ashfall.Core/Verdict/VerdictAccusationSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 262 lines / 12423 bytes.
- SHA-256: `5abba83661cd9f7b50e6d16beaea4dbf909d0262847260dac58e6ba0b4162c7c`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum AccusationAllowed
public sealed class AccusationResult
public AccusationAllowed Status;
public string CaseId = string.Empty;
public string SuspectId = string.Empty;
public string Reason = string.Empty;
public sealed class TribunalVerdict
public string CaseId = string.Empty;
public string SuspectId = string.Empty;
public bool Guilty;
public int EvidenceCount;
public string EndingKey = string.Empty;
public string FactionStandingEffect = string.Empty;
public int MoralDelta;
public string JournalEntry = string.Empty;
public class VerdictAccusationState
public List<string> resolvedCaseIds = new List<string>();
public Dictionary<string, string> caseVerdicts = new Dictionary<string, string>(StringComparer.Ordinal);
public sealed class VerdictAccusationSystem
public IReadOnlyList<string> ResolvedCaseIds => _state.resolvedCaseIds;
public void Bind(ReckoningSystem reckoning, VerdictEvidenceChain? evidenceChain = null) {
public AccusationResult CanAccuse(string caseId, string suspectId, int currentDay) {
public TribunalVerdict? ResolveTribunal( string caseId, string suspectId, int currentDay, MoralChoiceSystem? moralSystem = null) {
public bool IsResolved(string caseId) =>
public VerdictAccusationState CaptureState() {
public void RestoreState(VerdictAccusationState? state) {
```


# Appendix G.23 — Supporting Regression Evidence: `Ashfall.Core.Tests/SickListSystemTests.cs`

### `Ashfall.Core.Tests/SickListSystemTests.cs`

- Current test declarations: Fact=15, Theory=0, InlineData=0.
- File lines: 229; SHA-256: `016a6a2e8aa858f1df06722ff7c154944f12c7f2d333021b05750e5013dfc4b4`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Identity_IsTrimmedCaseInsensitiveAndReDiagnosisKeepsOneRow
InvalidBandDayAndReleaseTransitions_AreRejected
UnsupportedSeveritySource_FailsBackToDoseProvenance
Restore_FiltersMalformedAndDuplicateRows
StateBandAndChangeEventQueries_AreDetachedSnapshots
Diagnose_AddsBandAndFiresEvent
Diagnose_MovesBandKeepsRow
Diagnose_NullOrEmptySurvivorRejected
Release_SetsDayAndKeepsRow
AssignPalliative_RequiresDiagnosedSurvivor
BlackBand_RemainsOnRoster
CaptureState_ReturnsSnapshotNotLiveState
CaptureState_EmitsInOrdinalOrder
SaveLoad_RoundTripsAllState
SaveLoad_ChecksumStable
```


# Appendix G.24 — Supporting Regression Evidence: `Ashfall.Core.Tests/DoseCollectibleSaveFuzzTests.cs`

### `Ashfall.Core.Tests/DoseCollectibleSaveFuzzTests.cs`

- Current test declarations: Fact=16, Theory=0, InlineData=0.
- File lines: 374; SHA-256: `b91db5d6125a06b65e37301033d23b19feae9375f4f426487f093ab810620ff5`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
DoseFuzz_FullFiveRegisterEnvelope_CleanRoundTrip
DoseFuzz_ChecksumMutation_IsRejectedWithExactError
DoseFuzz_NullChecksum_IsRejected_NotSilentlyAccepted
DoseFuzz_V1Legacy_MigratesWithEmptyQuestSection
DoseFuzz_FutureVersion_IsRejected
DoseFuzz_SerializeTwice_IsByteIdentical
DiscoveryFuzz_CleanRoundTrip_PreservesPartitionsAndLocations
DiscoveryFuzz_SerializeTwice_IsByteIdentical
DiscoveryFuzz_V1LegacyRestore_MarksAllDiscoveredAsAcknowledged
DiscoveryFuzz_NullRestore_IsHonestEmptyState
ClaimsFuzz_CleanRoundTrip_PreservesClaimsAndAvailabilityGate
ClaimsFuzz_SerializeTwice_IsByteIdentical_AndOrdinalSorted
ClaimsFuzz_StaleSaveIds_NoLongerUnique_AreDroppedOnRestore
TutorialFuzz_CleanRoundTrip_PreservesSeenAndQueue
TutorialFuzz_NullRestore_IsHonestEmptyState
ScavengingCatalog_IsPinnedAsDataOnly_NoSaveSurface
```


# Appendix G.25 — Supporting Regression Evidence: `Ashfall.Core.Tests/DoseRegistersCatalogTests.cs`

### `Ashfall.Core.Tests/DoseRegistersCatalogTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 128; SHA-256: `2538daa36d3bb88e10dd9689a15263cf2cbdcaf244109efa3a4e45687fea712d`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Load_FindsFourBandsThreePlansThreeGuesses
Load_FindsTheFourAntagonists
Load_BandThresholdsBind
BandLabel_MapsCoreBandsToVocabulary
Load_MissingDirectoryReturnsEmptyCatalog
Characters_RegisterTheFourAntagonists
```


# Appendix G.26 — Supporting Regression Evidence: `Ashfall.Core.Tests/Plan90DoseRegistersExpansionTests.cs`

### `Ashfall.Core.Tests/Plan90DoseRegistersExpansionTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 163; SHA-256: `0e9bf585db0e5c71b66c76ec3d26909f60f0e9a08cfe5761088ae6abb689068c`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_HasTwelveBandsAndEightPlans
Bands_ThresholdsAreStrictlyIncreasing
Bands_AllIdsAreUniqueAndNonEmpty
Bands_NewEntriesExist
Plans_AllIdsAreUniqueAndNonEmpty
Plans_NewEntriesExist
Plans_CostsAreNonEmpty
BandLabel_BackwardCompatibilityForCoreFourBands
```


# Appendix H.27 — Supporting Authority Document: `docs/bodymind/DOSE_REGISTER_STATE_MODEL.md`

### `docs/bodymind/DOSE_REGISTER_STATE_MODEL.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 6341 lines / 274876 bytes.
- SHA-256: `5042141894e08e243457a9f753daabd064eab80287e4fafceef49f48c326ba33`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum DoseClassificationBand
public sealed class DoseRegisterEntry
public string SurvivorId { get; }
public float BiologicalCumulativeDoseMsv { get; private set; }
public float BookedLedgerDoseMsv { get; private set; }
public bool HasForgedCleanBill { get; private set; }
public string AdministrativeOverrideBand { get; private set; }
public int DaysSinceLastCalibration { get; private set; }
public DoseClassificationBand GetBiologicalBand() {
public DoseClassificationBand GetAdministrativeBand() {
public void ApplyForgedCleanBill() {
public void RevokeForgedCleanBill() {
public void SetAdministrativeOverride(string bandId) {
public void AddDoseReading(float environmentalDoseMsv, float shieldingFactor) {
public void RecalibrateSensor() {
public void AdvanceDays(int days) {
public sealed class DoseRegisterStateModelOrchestrator
public IReadOnlyDictionary<string, DoseRegisterEntry> Entries => new ReadOnlyDictionary<string, DoseRegisterEntry>(_entries);
public DoseRegisterEntry GetOrCreateEntry(string survivorId, float initialBioDose, float initialBookedDose) {
public string ComputeRegisterDigest() {
public sealed class DoseRegisterStateModelTests
public void Test_001_DoseRegister_StateModelAndBandResolution() {
public void Test_002_DoseRegister_StateModelAndBandResolution() {
public void Test_003_DoseRegister_StateModelAndBandResolution() {
public void Test_004_DoseRegister_StateModelAndBandResolution() {
public void Test_005_DoseRegister_StateModelAndBandResolution() {
public void Test_006_DoseRegister_StateModelAndBandResolution() {
public void Test_007_DoseRegister_StateModelAndBandResolution() {
public void Test_008_DoseRegister_StateModelAndBandResolution() {
public void Test_009_DoseRegister_StateModelAndBandResolution() {
public void Test_010_DoseRegister_StateModelAndBandResolution() {
public void Test_011_DoseRegister_StateModelAndBandResolution() {
public void Test_012_DoseRegister_StateModelAndBandResolution() {
public void Test_013_DoseRegister_StateModelAndBandResolution() {
public void Test_014_DoseRegister_StateModelAndBandResolution() {
public void Test_015_DoseRegister_StateModelAndBandResolution() {
public void Test_016_DoseRegister_StateModelAndBandResolution() {
public void Test_017_DoseRegister_StateModelAndBandResolution() {
public void Test_018_DoseRegister_StateModelAndBandResolution() {
public void Test_019_DoseRegister_StateModelAndBandResolution() {
public void Test_020_DoseRegister_StateModelAndBandResolution() {
public void Test_021_DoseRegister_StateModelAndBandResolution() {
public void Test_022_DoseRegister_StateModelAndBandResolution() {
public void Test_023_DoseRegister_StateModelAndBandResolution() {
public void Test_024_DoseRegister_StateModelAndBandResolution() {
public void Test_025_DoseRegister_StateModelAndBandResolution() {
public void Test_026_DoseRegister_StateModelAndBandResolution() {
public void Test_027_DoseRegister_StateModelAndBandResolution() {
public void Test_028_DoseRegister_StateModelAndBandResolution() {
public void Test_029_DoseRegister_StateModelAndBandResolution() {
public void Test_030_DoseRegister_StateModelAndBandResolution() {
public void Test_031_DoseRegister_StateModelAndBandResolution() {
public void Test_032_DoseRegister_StateModelAndBandResolution() {
public void Test_033_DoseRegister_StateModelAndBandResolution() {
public void Test_034_DoseRegister_StateModelAndBandResolution() {
public void Test_035_DoseRegister_StateModelAndBandResolution() {
public void Test_036_DoseRegister_StateModelAndBandResolution() {
public void Test_037_DoseRegister_StateModelAndBandResolution() {
public void Test_038_DoseRegister_StateModelAndBandResolution() {
public void Test_039_DoseRegister_StateModelAndBandResolution() {
public void Test_040_DoseRegister_StateModelAndBandResolution() {
public void Test_041_DoseRegister_StateModelAndBandResolution() {
public void Test_042_DoseRegister_StateModelAndBandResolution() {
public void Test_043_DoseRegister_StateModelAndBandResolution() {
public void Test_044_DoseRegister_StateModelAndBandResolution() {
public void Test_045_DoseRegister_StateModelAndBandResolution() {
public void Test_046_DoseRegister_StateModelAndBandResolution() {
public void Test_047_DoseRegister_StateModelAndBandResolution() {
public void Test_048_DoseRegister_StateModelAndBandResolution() {
public void Test_049_DoseRegister_StateModelAndBandResolution() {
public void Test_050_DoseRegister_StateModelAndBandResolution() {
public void Test_051_DoseRegister_StateModelAndBandResolution() {
public void Test_052_DoseRegister_StateModelAndBandResolution() {
public void Test_053_DoseRegister_StateModelAndBandResolution() {
public void Test_054_DoseRegister_StateModelAndBandResolution() {
public void Test_055_DoseRegister_StateModelAndBandResolution() {
public void Test_056_DoseRegister_StateModelAndBandResolution() {
public void Test_057_DoseRegister_StateModelAndBandResolution() {
public void Test_058_DoseRegister_StateModelAndBandResolution() {
public void Test_059_DoseRegister_StateModelAndBandResolution() {
public void Test_060_DoseRegister_StateModelAndBandResolution() {
public void Test_061_DoseRegister_StateModelAndBandResolution() {
public void Test_062_DoseRegister_StateModelAndBandResolution() {
public void Test_063_DoseRegister_StateModelAndBandResolution() {
public void Test_064_DoseRegister_StateModelAndBandResolution() {
public void Test_065_DoseRegister_StateModelAndBandResolution() {
public void Test_066_DoseRegister_StateModelAndBandResolution() {
public void Test_067_DoseRegister_StateModelAndBandResolution() {
public void Test_068_DoseRegister_StateModelAndBandResolution() {
public void Test_069_DoseRegister_StateModelAndBandResolution() {
public void Test_070_DoseRegister_StateModelAndBandResolution() {
public void Test_071_DoseRegister_StateModelAndBandResolution() {
public void Test_072_DoseRegister_StateModelAndBandResolution() {
public void Test_073_DoseRegister_StateModelAndBandResolution() {
public void Test_074_DoseRegister_StateModelAndBandResolution() {
public void Test_075_DoseRegister_StateModelAndBandResolution() {
public void Test_076_DoseRegister_StateModelAndBandResolution() {
public void Test_077_DoseRegister_StateModelAndBandResolution() {
public void Test_078_DoseRegister_StateModelAndBandResolution() {
public void Test_079_DoseRegister_StateModelAndBandResolution() {
public void Test_080_DoseRegister_StateModelAndBandResolution() {
public void Test_081_DoseRegister_StateModelAndBandResolution() {
public void Test_082_DoseRegister_StateModelAndBandResolution() {
public void Test_083_DoseRegister_StateModelAndBandResolution() {
public void Test_084_DoseRegister_StateModelAndBandResolution() {
public void Test_085_DoseRegister_StateModelAndBandResolution() {
public void Test_086_DoseRegister_StateModelAndBandResolution() {
public void Test_087_DoseRegister_StateModelAndBandResolution() {
public void Test_088_DoseRegister_StateModelAndBandResolution() {
public void Test_089_DoseRegister_StateModelAndBandResolution() {
public void Test_090_DoseRegister_StateModelAndBandResolution() {
public void Test_091_DoseRegister_StateModelAndBandResolution() {
public void Test_092_DoseRegister_StateModelAndBandResolution() {
public void Test_093_DoseRegister_StateModelAndBandResolution() {
public void Test_094_DoseRegister_StateModelAndBandResolution() {
public void Test_095_DoseRegister_StateModelAndBandResolution() {
public void Test_096_DoseRegister_StateModelAndBandResolution() {
public void Test_097_DoseRegister_StateModelAndBandResolution() {
public void Test_098_DoseRegister_StateModelAndBandResolution() {
public void Test_099_DoseRegister_StateModelAndBandResolution() {
public void Test_100_DoseRegister_StateModelAndBandResolution() {
```


# Appendix H.28 — Supporting Authority Document: `docs/bodymind/AUTOPSY_CONSENT_MATRIX.md`

### `docs/bodymind/AUTOPSY_CONSENT_MATRIX.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 9883 lines / 431419 bytes.
- SHA-256: `35d0f4acd1d512e97a4a2a5c4c9f75cc9faecf65879d1c50d9c9fa7be4b3d715`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum AutopsyConsentStatus
public readonly struct AutopsyCaseRecord : IEquatable<AutopsyCaseRecord>
public readonly string SpecimenSurvivorId;
public readonly string DeceasedName;
public readonly string KinSurvivorId;
public readonly int KinBondScore;
public readonly AutopsyConsentStatus Status;
public readonly bool IsBuriedOrCremated;
public readonly bool IsDissectionCompleted;
public readonly string DiscoveredCauseOfDeath;
public bool Equals(AutopsyCaseRecord other) => SpecimenSurvivorId == other.SpecimenSurvivorId;
public override bool Equals(object obj) => obj is AutopsyCaseRecord other && Equals(other);
public override int GetHashCode() => SpecimenSurvivorId.GetHashCode();
public sealed class AutopsyConsentOrchestrator
public IReadOnlyDictionary<string, AutopsyCaseRecord> Specimens => new ReadOnlyDictionary<string, AutopsyCaseRecord>(_specimens);
public IReadOnlyCollection<string> CompletedIds => _completedSpecimenIds;
public void RegisterCorpse(string specimenId, string name, string kinId, int kinBond, bool isEpidemic, bool isHomicide) {
public bool CanPerformAutopsy(string specimenId, out string denialReason) {
public bool EnactExecutiveOverride(string specimenId, out string moralConsequenceLog) {
public void CompleteDissection(string specimenId, string pathologyFinding) {
public void MarkBuried(string specimenId) {
public string GenerateConsentDigest() {
public sealed class AutopsyConsentMatrixTests
public void Test_001_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_002_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_003_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_004_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_005_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_006_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_007_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_008_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_009_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_010_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_011_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_012_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_013_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_014_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_015_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_016_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_017_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_018_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_019_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_020_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_021_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_022_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_023_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_024_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_025_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_026_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_027_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_028_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_029_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_030_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_031_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_032_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_033_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_034_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_035_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_036_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_037_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_038_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_039_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_040_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_041_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_042_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_043_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_044_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_045_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_046_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_047_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_048_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_049_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_050_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_051_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_052_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_053_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_054_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_055_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_056_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_057_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_058_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_059_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_060_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_061_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_062_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_063_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_064_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_065_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_066_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_067_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_068_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_069_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_070_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_071_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_072_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_073_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_074_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_075_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_076_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_077_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_078_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_079_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_080_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_081_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_082_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_083_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_084_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_085_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_086_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_087_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_088_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_089_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_090_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_091_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_092_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_093_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_094_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_095_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_096_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_097_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_098_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_099_AutopsyConsent_HierarchyAndBurialLockContract() {
public void Test_100_AutopsyConsent_HierarchyAndBurialLockContract() {
```


# Appendix H.29 — Supporting Authority Document: `docs/medical/AUTOPSY_RUNTIME_CONTRACT.md`

### `docs/medical/AUTOPSY_RUNTIME_CONTRACT.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 56 lines / 3435 bytes.
- SHA-256: `2c1506a80b438f4b715723731882b246441b6e87c714817ff1f6f937dcd37553`.
- Architecture signals: seeded references=1; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| cumulative dose, bands and bookings | DoseLedgerSystem | authored bands/plans and institutional guidance | Dose registers | Owner emits/reads a typed fact; no mirror state. |
| cumulative dose, bands and bookings | DoseLedgerSystem | procedure, consent, findings and research effects | AutopsySystem | Owner emits/reads a typed fact; no mirror state. |
| cumulative dose, bands and bookings | DoseLedgerSystem | institutional evidence and judgment | Verdict/tribunal | Owner emits/reads a typed fact; no mirror state. |
| cumulative dose, bands and bookings | DoseLedgerSystem | temporary exposure/action blocks/recovery | PsychologicalContaminationSystem | Owner emits/reads a typed fact; no mirror state. |
| authored bands/plans and institutional guidance | Dose registers | cumulative dose, bands and bookings | DoseLedgerSystem | Owner emits/reads a typed fact; no mirror state. |
| authored bands/plans and institutional guidance | Dose registers | procedure, consent, findings and research effects | AutopsySystem | Owner emits/reads a typed fact; no mirror state. |
| authored bands/plans and institutional guidance | Dose registers | institutional evidence and judgment | Verdict/tribunal | Owner emits/reads a typed fact; no mirror state. |
| authored bands/plans and institutional guidance | Dose registers | temporary exposure/action blocks/recovery | PsychologicalContaminationSystem | Owner emits/reads a typed fact; no mirror state. |
| procedure, consent, findings and research effects | AutopsySystem | cumulative dose, bands and bookings | DoseLedgerSystem | Owner emits/reads a typed fact; no mirror state. |
| procedure, consent, findings and research effects | AutopsySystem | authored bands/plans and institutional guidance | Dose registers | Owner emits/reads a typed fact; no mirror state. |
| procedure, consent, findings and research effects | AutopsySystem | institutional evidence and judgment | Verdict/tribunal | Owner emits/reads a typed fact; no mirror state. |
| procedure, consent, findings and research effects | AutopsySystem | temporary exposure/action blocks/recovery | PsychologicalContaminationSystem | Owner emits/reads a typed fact; no mirror state. |
| institutional evidence and judgment | Verdict/tribunal | cumulative dose, bands and bookings | DoseLedgerSystem | Owner emits/reads a typed fact; no mirror state. |
| institutional evidence and judgment | Verdict/tribunal | authored bands/plans and institutional guidance | Dose registers | Owner emits/reads a typed fact; no mirror state. |
| institutional evidence and judgment | Verdict/tribunal | procedure, consent, findings and research effects | AutopsySystem | Owner emits/reads a typed fact; no mirror state. |
| institutional evidence and judgment | Verdict/tribunal | temporary exposure/action blocks/recovery | PsychologicalContaminationSystem | Owner emits/reads a typed fact; no mirror state. |
| temporary exposure/action blocks/recovery | PsychologicalContaminationSystem | cumulative dose, bands and bookings | DoseLedgerSystem | Owner emits/reads a typed fact; no mirror state. |
| temporary exposure/action blocks/recovery | PsychologicalContaminationSystem | authored bands/plans and institutional guidance | Dose registers | Owner emits/reads a typed fact; no mirror state. |
| temporary exposure/action blocks/recovery | PsychologicalContaminationSystem | procedure, consent, findings and research effects | AutopsySystem | Owner emits/reads a typed fact; no mirror state. |
| temporary exposure/action blocks/recovery | PsychologicalContaminationSystem | institutional evidence and judgment | Verdict/tribunal | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Build a read-only casework projection joining dose, duty, verdict and medical facts. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Expose consent, uncertainty, conflicting evidence and appeal capacity without inventing legal procedure. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Connect autopsy outcomes to memorial/inquiry consumers through current owners. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Treat psychological contamination as a temporary interaction layer, not a second mental-health meter. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.561 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs`

### `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 225 lines / 10025 bytes.
- SHA-256: `abee1d19deb019bb058c9435069600549265c68836d2723c607fccd88c23d334`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class VerdictCatalogLoader
public const string DataFile = "verdict_data.json";
public const string LocationsFile = "verdict_locations.json";
public const string ItemsFile = "verdict_items.json";
public const string RadioFile = "verdict_radio.json";
public class VerdictLocationEntry
public string id = string.Empty;
public string displayName = string.Empty;
public string description = string.Empty;
public int dangerLevel = 5;
public float travelHours = 5f;
public float baseRadsPerHour = 30f;
public static List<VerdictLocationEntry> LoadLocations( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public class VerdictItemEffects
public int enrolled_evidence;
public string note = string.Empty;
public class VerdictItemEntry
public string id = string.Empty;
public string displayName = string.Empty;
public float weightKg;
public float tradeValue;
public string category = "story_item";
public string tier = string.Empty;
public string description = string.Empty;
public VerdictItemEffects mechanical_effects = null!;
public string downstream_quest_trigger = string.Empty;
public string faction_affinity = string.Empty;
public string rarity = string.Empty;
public static List<VerdictItemEntry> LoadItems( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public class VerdictRadioEntry
public string id = string.Empty;
public string frequency = string.Empty;
public int dayTrigger = 180;
public string source = string.Empty;
public string message = string.Empty;
public string signalStrength = string.Empty;
public string kind = "telemetry";
public string audio_cue = string.Empty;
public static List<VerdictRadioEntry> LoadRadio( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public class VerdictWorldHistoryLadderEntry
public int layer { get; set; }
public string knowledge_key { get; set; } = string.Empty;
public string title { get; set; } = string.Empty;
public string discovery_location_id { get; set; } = string.Empty;
public string body_summary { get; set; } = string.Empty;
public List<string> corruption_corpus = new List<string>();
public List<VerdictWorldHistoryLadderEntry> world_history_ladder = new List<VerdictWorldHistoryLadderEntry>();
public static List<string> LoadCorruptionCorpus( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static List<VerdictWorldHistoryLadderEntry> LoadWorldHistoryLadder( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public List<VerdictRadioEntry> broadcasts = new List<VerdictRadioEntry>();
```


# Appendix Q.562 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Verdict/VerdictCensusBroadcast.cs`

### `Assets/Ashfall.Core/Verdict/VerdictCensusBroadcast.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 93 lines / 3756 bytes.
- SHA-256: `92e2faf06e2267b87fa104dbee7d53ac02512150751bf9216a9cb898140e9c81`.
- Architecture signals: seeded references=2; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public interface IWorldCensus
public sealed class VerdictCensusBroadcast
public const double CarrierSeconds = 4.0;
public const double HeldBreathPauseSeconds = 1.7; // canon — do not tune
public const long ExpectedProvincialCount = 211004;
public bool IsWindowOpen() => _clock.DayIndex % 7 == 0 && _clock.HourOfDay == 3;
public void BroadcastIfDue() {
public void ResetWindowLatch() => _lastWindowDay = -1;
public int LastWindowDay => _lastWindowDay;
```


# Appendix Q.563 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Verdict/VerdictEndingEvaluator.cs`

### `Assets/Ashfall.Core/Verdict/VerdictEndingEvaluator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 71 lines / 3244 bytes.
- SHA-256: `2d360d2f9576e56acc1755668fa80a1db46a2a5ec0016c9969306b89cb27f9cd`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class VerdictEndingEvaluator
public static readonly string[] EndingKeys = {
public const string EndingKeyCounted = "ending_verdict_the_sector_recounts";
public const string EndingKeyHeld = "ending_verdict_the_count_is_held";
public const string EndingKeyLease = "ending_verdict_the_offer_is_a_lease";
public const int MinimumEvidenceForRecount = 4;
public static string? ResolvedEnding(ReckoningState state) {
public static string? DecideEnding(ReckoningState state, int enrolledEvidence, int day) {
public static bool IsTempestDecommissioned(ReckoningState state) {
```


# Appendix Q.564 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Verdict/VerdictEvidenceChain.cs`

### `Assets/Ashfall.Core/Verdict/VerdictEvidenceChain.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 62 lines / 2164 bytes.
- SHA-256: `7060cd4df81d95b718431f86c1b6252c4b552627b461880ae22b6b5f8874257a`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class VerdictEvidenceChain
public int ReconcileReadEntries() {
```


# Appendix Q.565 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs`

### `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 155 lines / 5915 bytes.
- SHA-256: `09f7ee1e6d99e55b7559acda53ff56389392ff3ae5cf9ca2069d4b3db2a28ca4`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=2; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class VerdictNpcEntry
public string id = string.Empty;
public string name = string.Empty;
public string role = string.Empty;
public string kind = "paper_ghost";  // tape_echo | paper_ghost | living | readings
public string gatingFlag = string.Empty;
public string locationId = string.Empty;
public int phaseMin = 1;
public List<string> dialogue = new List<string>();
public class VerdictNpcState
public List<string> spokenNpcIds = new List<string>();
public sealed class VerdictNpcSystem
public VerdictNpcState State => _state;
public IReadOnlyList<VerdictNpcEntry> Catalog => _catalog;
public event Action<VerdictNpcEntry> OnSpoken;
public void Register(VerdictNpcEntry entry) {
public VerdictNpcEntry? Find(string id) {
public List<VerdictNpcEntry> GetAvailable( IReadOnlyCollection<string> setFlags, int phase, string? locationId = null) {
public bool Speak(string npcId, string? locationId = null) {
public VerdictNpcState CaptureState() {
public void RestoreState(VerdictNpcState state) {
public static class VerdictNpcCatalogLoader
public const string FileName = "verdict_npcs.json";
public static int LoadAndRegister(VerdictNpcSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix Q.566 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Verdict/VerdictQuestCatalogLoader.cs`

### `Assets/Ashfall.Core/Verdict/VerdictQuestCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 63 lines / 2293 bytes.
- SHA-256: `5f1a7bc4b7265f09f93b104b8e569dab426eea3aec5e50b14ff4656c7f59c6c4`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class VerdictQuestCatalogLoader
public const string FileName = "verdict_questlines.json";
public static int LoadAndRegister( QuestlineSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix Q.567 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Verdict/VerdictQuestMigration.cs`

### `Assets/Ashfall.Core/Verdict/VerdictQuestMigration.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 136 lines / 5372 bytes.
- SHA-256: `7d74ea14dea0667b45952380723f65ed2ed256d96fa50d009e10867859f85574`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class VerdictQuestMigration
public const string VerdictQuestPrefix = "quest_verdict_";
public static bool IsVerdictQuestline(string questlineId) {
public static int AdoptFromYearOfAsh( QuestlineSystemState verdictState, QuestlineSystemState yearOfAshState) {
public static int StripFromYearOfAsh(QuestlineSystemState yearOfAshState) {
```


# Appendix Q.568 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs`

### `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 114 lines / 4583 bytes.
- SHA-256: `fb560fa3710ed2ac870f7e1cf16f5dd087e316eb26ffc60d4cd6bc4be960f5db`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class VerdictRadioSystem
public const string SystemId = "verdict_radio_system";
public const int CarrierOpenDay = 210;
public IReadOnlyList<VerdictCatalogLoader.VerdictRadioEntry> Corpus => _corpus;
public bool HasFired(string id) => _firedIds.Contains(id);
public int FiredCount => _firedIds.Count;
public System.Collections.Generic.List<string> Poll(int day, ReckoningPhase phase) {
public int LoadFrom(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public class VerdictRadioState
public string systemId = SystemId;
public List<string> firedIds = new List<string>();
public VerdictRadioState CaptureState() {
public void RestoreState(VerdictRadioState state) {
```


# Appendix Q.569 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Verdict/VerdictReadout.cs`

### `Assets/Ashfall.Core/Verdict/VerdictReadout.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 96 lines / 4916 bytes.
- SHA-256: `fd0d6b70232ce3c357cf7a3c631f65ff6c2c6a9632b7951a74e32c7f45d511a8`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class VerdictReadout
public static string RiteTraceLine(int riteTraceTotal, int enrolledEvidence = 0) {
public static string LineFor(ReckoningState state, int enrolledEvidence, int readCount) {
```


# Appendix Q.570 — Additional Current Architecture Evidence: `src/Host/DoseLedgerSaveStore.cs`

### `src/Host/DoseLedgerSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 58 lines / 2915 bytes.
- SHA-256: `5d16d85d0bc7ebde0fc2f9900a147ace6ddae49156587c8185e7f289a5c78703`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class DoseLedgerSaveStore
public const string FileName = "dose_ledger_save.json";
public const string SectionName = "dose_ledger";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCaptureDirect(DoseLedgerSave state) => s_store.CaptureBare(state);
public static DoseLedgerSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(DoseLedgerSave state) => s_store.CaptureBare(state);
public static DoseLedgerSave? TryRestore(string json) => s_store.RestoreBare(json);
public static bool TrySave(DoseLedgerSave save, string pathOverride = null!) =>
public static DoseLedgerSave? TryLoad(string pathOverride = null!) =>
public static string TryCapturePersisted(DoseLedgerSave save) => s_store.CapturePersisted(save);
```


# Appendix Q.571 — Additional Current Architecture Evidence: `src/UI/DoseLedgerPanel.cs`

### `src/UI/DoseLedgerPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 451 lines / 20496 bytes.
- SHA-256: `4238960b70761f9089982d81facd2f41086749c0b24028ef7c84f944a6ca6ac0`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=5; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class DoseLedgerPanel : Control, IBindablePanel
public event Action? OnClose;
public event Action<string>? OnSurvivorSelected;
public bool IsBound => _doseSession != null;
public void Bind(DoseLedgerHostSession session, SurvivorsHostSession? survivors = null) {
public void Unbind() {
public void RefreshView() {
internal static AshfallDataGrid.CellState MapBand(float cumulativeMsv) {
internal static string BandName(AshfallDataGrid.CellState s) => s switch
public override void _Ready() {
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
public override void _ExitTree() {
```


# Appendix Q.572 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/DoseRegistersCatalog.cs`

### `Assets/Ashfall.Core/DoseRegistersCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 114 lines / 3973 bytes.
- SHA-256: `1052c82073cc4256279484603721a46e084caebc35e17f3646c0ac8a1fa74363`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class DoseBandDef
public string id = string.Empty;
public string label = string.Empty;
public float threshold_msv;
public string disposition = string.Empty;
public class DosePlanDef
public string id = string.Empty;
public string label = string.Empty;
public string cost = string.Empty;
public string note = string.Empty;
public class DoseGuessDef
public string id = string.Empty;
public string label = string.Empty;
public bool pencil;
public string note = string.Empty;
public class DoseRegisterNpcDef
public string id = string.Empty;
public string name = string.Empty;
public string register = string.Empty;
public string disposition = string.Empty;
public string action_label = string.Empty;
public string action = string.Empty;
public class DoseRegistersCatalog
public List<DoseBandDef> bands = new List<DoseBandDef>();
public List<DosePlanDef> plans = new List<DosePlanDef>();
public List<DoseGuessDef> guesses = new List<DoseGuessDef>();
public List<DoseRegisterNpcDef> npcs = new List<DoseRegisterNpcDef>();
public static class DoseRegistersCatalogLoader
public const string FileName = "dose_registers.json";
public static DoseRegistersCatalog Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static string BandLabel(DoseRegistersCatalog catalog, int band) {
public static string BandIdFor(int band) {
```


# Appendix Q.573 — Additional Current Architecture Evidence: `src/Main.UiTests.Dose.cs`

### `src/Main.UiTests.Dose.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 197 lines / 11167 bytes.
- SHA-256: `98597238d39be162a159a7d6e9b94d12d612adf6d298d53cdc9d4de6b4d7ecbb`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.574 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Survivors/CrewConsentVerdict.cs`

### `Assets/Ashfall.Core/Survivors/CrewConsentVerdict.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 27 lines / 1109 bytes.
- SHA-256: `9c23fc73394cc0f4f8b77e1ee010f566eee8cfe845afd359622d38ff9d028907`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CrewConsentVerdict
public bool Consented { get; set; }
public bool Warning { get; set; }
public string RefusalReason { get; set; } = string.Empty;
public string WarningReason { get; set; } = string.Empty;
public static CrewConsentVerdict Accepted() =>
public static CrewConsentVerdict AcceptedWithWarning(string warning) =>
public static CrewConsentVerdict Refused(string reason) =>
```


# Appendix Q.575 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Verdict/EvidenceLedger.cs`

### `Assets/Ashfall.Core/Verdict/EvidenceLedger.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 111 lines / 3948 bytes.
- SHA-256: `30bcfc9f91c1ab7b69cb80cb8a06d882a0f1802a3314f26d4551834aacc079ae`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=2; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class EvidenceLedgerState
public List<string> enrolled = new List<string>();
public string lastEnrolled = string.Empty;
public int enrollmentDay = -1;
public class EvidenceDefinition
public string id = string.Empty;
public string name = string.Empty;
public string category = "story_item";
public string tier = "Old-World";
public string flavor = string.Empty;
public string questTrigger = string.Empty;
public string factionAffinity = string.Empty;
public string rarity = "Rare";
public sealed class EvidenceLedger
public EvidenceLedgerState State => _state;
public IReadOnlyList<string> Enrolled => _state.enrolled;
public event Action<string> OnEnrolled;
public void Register(EvidenceDefinition def) {
public EvidenceDefinition? Get(string id) => string.IsNullOrEmpty(id) ? null : (_catalog.TryGetValue(id, out var d) ? d : null);
public bool IsEnrolled(string id) {
public bool Enroll(string id, int day) {
public int Count => _state.enrolled.Count;
public EvidenceLedgerState CaptureState() {
public void RestoreState(EvidenceLedgerState state) {
```


# Appendix Q.576 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Verdict/MachineLogSystem.cs`

### `Assets/Ashfall.Core/Verdict/MachineLogSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 200 lines / 7460 bytes.
- SHA-256: `4b4ef8e95fb5d12e9c6c9329151d355e68c9614439b4188f3380d79617d4458e`.
- Architecture signals: seeded references=1; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MachineLogEntry
public string facilityId = string.Empty;
public int day = 0;
public string kind = "operating";   // operating | maintenance | anomaly | count
public string bodyShort = string.Empty;
public string evidenceTag = string.Empty;
public bool read;
public sealed class MachineLogSystemState
public List<MachineLogEntry> entries = new List<MachineLogEntry>();
public int lastTapeSpinDay = -1;
public int logIndex;
public bool countdownActive;
public int countdownDaysLeft;
public sealed class MachineLogSystem
public MachineLogSystemState State => _state;
public IReadOnlyList<MachineLogEntry> Entries => _state.entries;
public event Action<MachineLogEntry> OnLogPosted;
public event Action<MachineLogEntry> OnEntryRead;
public event Action OnTapeSpin;
public bool Post(string facilityId, int day, string kind, string bodyShort, string evidenceTag) {
public int ApplyRetention(Records.RetentionPolicyCatalog? catalog) {
public bool InsertCorruptionMarker(int day, ISeededRng rng, IReadOnlyList<string>? corpus = null) {
public string ReadEntry(int index) {
public void SpinTape(int day) {
public int UnreadCount() {
public int ReadCount() {
public MachineLogSystemState CaptureState() {
public void RestoreState(MachineLogSystemState state) {
```


# Appendix Q.577 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Verdict/ReckoningSystem.cs`

### `Assets/Ashfall.Core/Verdict/ReckoningSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 301 lines / 14132 bytes.
- SHA-256: `2832fec6c2b2b59cd23f65b6dd3332864d8c54bf43e3b5f90af7168e4b162cf8`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=9; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum ReckoningPhase
public sealed class ReckoningState
public ReckoningPhase phase = ReckoningPhase.Dormant;
public int phaseChangedDay = -1;
public bool carrierHeard;          // the 99.0 MHz pilot tone (one-shot)
public bool callResolved;          // the Call fired (one-shot)
public bool countPresented;        // PRESENT chosen
public bool countHeld;             // HOLD chosen
public bool offerIsLease;          // DISCHARGE chosen
public int enrolledEvidence;
public int riteTraceTotal;
public int driftDays = 3;          // the machine's clock disagrees with the wars' by 3 days
public int dwellingDriftTotal;     // running count of lost/withdrew/drifted/yielded dwellings
public int lastDriftDay = -1;
public int lastDriftDeltaToday;    // delta applied on lastDriftDay
public float cumulativeDoseSieverts;
public bool highDosePromoted;      // one-shot auto-promote past Knowing from dose
public sealed class ReckoningSystem
public const int KnowingDay = 160;
public const int CulpableDay = 210;
public const int CountedDay = 240;
public const int EvidenceCulpableGate = 1;   // at least one read entry to open CULPABLE early is allowed
public const int ExpectedProvincialCount = 211004;
public const float HighDoseKnowingThresholdSieverts = 4.0f;
public const float HighDoseCulpableFloorSieverts = 8.0f;
public ReckoningState State => _state;
public ReckoningPhase Phase => _state.phase;
public event Action<ReckoningPhase> OnPhaseChanged;
public event Action OnCarrierHeard;
public event Action<int> OnReckoningCall;      // payload = observed living count
public event Action<string> OnVerdictResolved; // payload = ending key
public System.Collections.Generic.List<string> Poll( int day, int livingCount, int logReadCount, int evidenceCount) {
public void EnrollEvidence(int amount = 1) {
public void EnrollRiteTrace(int amount = 1) {
public int RiteTraceCount => _state.riteTraceTotal;
internal void ReconcileEvidenceCount(int amount) {
public bool IsCensusWindowOpen(int day) => _state.phase >= ReckoningPhase.Culpable && day >= CulpableDay;
public int ClockDriftDays => _state.driftDays;
public bool SelectEnding(string endingKey, int day) {
public ReckoningState CaptureState() {
public void RestoreState(ReckoningState state) {
public void RecordDrift(int day, int count) {
public void RecordCumulativeDose(int day, float sieverts) {
public int DwellingDriftTotal => _state.dwellingDriftTotal;
public int LastDriftDeltaToday => _state.lastDriftDeltaToday;
public int LastDriftDay => _state.lastDriftDay;
public float CumulativeDoseSieverts => _state.cumulativeDoseSieverts;
public bool HighDosePromoted => _state.highDosePromoted;
```


# Appendix Q.578 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

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


# Appendix Q.579 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Disease/DiseaseTriage.cs`

### `Assets/Ashfall.Core/Disease/DiseaseTriage.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 262 lines / 12294 bytes.
- SHA-256: `bf9a33da7a8912b615f0d9e9188ff241821e1edaabc594c82e321699b24ec2e7`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum DiseaseClinicalStage
public sealed class DiseaseClinicalPicture
public string DiseaseId { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public DiseaseClinicalStage Stage { get; set; }
public string StageToken { get; set; } = string.Empty;
public string Tell { get; set; } = string.Empty;
public string SecondaryTell { get; set; } = string.Empty;
public string TimingClue { get; set; } = string.Empty;
public string Guidance { get; set; } = string.Empty;
public string Vector { get; set; } = string.Empty;
public int DaysSick { get; set; }
public int DaysUntilOutcome { get; set; }
public float AuthoredLethality { get; set; }
public float EffectiveLethality { get; set; }
public bool HasTreatmentPath { get; set; }
public bool HasCure { get; set; }
public bool Terminal { get; set; }
public int DosesGiven { get; set; }
public bool Diagnosed { get; set; }
public static class DiseaseTriage
public const float TerminalWindowFraction = 0.75f;
public const float TerminalLethalityFloor = 0.25f;
public const float HeavySedationLethalityFloor = 0.5f;
public static class Plans
public const string ComfortRounds = "plan_comfort_rounds";
public const string MorphineTray = "plan_morphine_tray";
public static DiseaseClinicalStage StageOf(DiseaseDefinition def, int daysSick) {
public static bool IsTerminalPrognosis(DiseaseDefinition def, int daysSick) {
public static int SickBandFor(DiseaseDefinition def, int daysSick) {
public static bool ShouldNameToSickList(DiseaseDefinition def, int daysSick) =>
public static string PalliativePlanFor(DiseaseDefinition def, int daysSick) {
public static string StageToken(DiseaseClinicalStage stage) {
public static DiseaseClinicalPicture PictureOf( DiseaseDefinition def, int daysSick, float effectiveLethality = float.NaN, int dosesGiven = 0, bool diagnosed = true) {
```


# Appendix Q.580 — Additional Current Architecture Evidence: `src/Host/HostCli.PanelTests.cs`

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


# Appendix Q.581 — Additional Current Architecture Evidence: `src/Host/VerdictHostSession.cs`

### `src/Host/VerdictHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 284 lines / 14368 bytes.
- SHA-256: `eb9abc73b3557cdfa1d7b975f426399da4cbf2e299387681232521df4b1f6f8b`.
- Architecture signals: seeded references=4; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class VerdictHostSession
public MachineLogSystem MachineLog { get; }
public ReckoningSystem Reckoning { get; }
public EvidenceLedger Evidence { get; }
public VerdictEvidenceChain EvidenceChain { get; }
public VerdictNpcSystem Npcs { get; }
public VerdictCensusBroadcast Census { get; }
public VerdictRadioSystem Radio { get; internal set; }
public QuestlineSystem Quests { get; }
public IReadOnlyList<VerdictCatalogLoader.VerdictLocationEntry> Locations { get; }
public IReadOnlyList<VerdictCatalogLoader.VerdictItemEntry> Items { get; }
public IReadOnlyList<VerdictCatalogLoader.VerdictRadioEntry> RadioEntries { get; }
public IReadOnlyList<string> CorruptionCorpus { get; private set; }
public System.Collections.Generic.HashSet<string> MaterializedNpcFlags() {
public System.Collections.Generic.List<Ashfall.Core.Verdict.VerdictNpcEntry> AvailableNpcs(string locationId = null!) {
public string LastEvent { get; private set; } = string.Empty;
public static VerdictHostSession Create( string dataDir, ISimClock clock = null!, IEventBus bus = null!, IFlagLedger flags = null!, ISeededRng radioRng = null!,
public int LoadedSaveVersion { get; private set; }
public bool WasSaveMigrated { get; private set; }
public void AdvanceDay(int day, int livingCount, int logReadCount) {
public void TickCensus() {
public System.Collections.Generic.List<string> TickRadio(int day) {
public int EnrollEvidenceFromItems(int day) {
public void TickCorruption(int day) {
public VerdictSave CaptureSave() {
public void RestoreSave(VerdictSave save) {
public string StatusLine() {
public VerdictCatalogLoader.VerdictLocationEntry? FindLocation(string id) {
```


# Appendix Q.582 — Additional Current Architecture Evidence: `src/Host/VerdictSaveStore.cs`

### `src/Host/VerdictSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 64 lines / 2904 bytes.
- SHA-256: `29963fb9ec5a0497d197114ba7f8e3fb5d3ac36bb3cdcb1e8fb32f694ce84e02`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class VerdictSaveStore
public const string FileName = "verdict_save.json";
public const string SectionName = "verdict";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCaptureDirect(VerdictSave state) => s_store.CaptureBare(state);
public static VerdictSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(VerdictSave state) => s_store.CaptureBare(state);
public static VerdictSave? TryRestore(string json) => s_store.RestoreBare(json);
public static bool TrySave(VerdictSave save, string pathOverride = null!) =>
public static VerdictSave? TryLoad(string pathOverride = null!) =>
public static string TryCapturePersisted(VerdictSave save) => s_store.CapturePersisted(save);
```


# Appendix Q.583 — Additional Current Architecture Evidence: `src/Main.UiTests.Verdict.cs`

### `src/Main.UiTests.Verdict.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 81 lines / 3437 bytes.
- SHA-256: `c1da92005e0a2903ec55c985e7f3344a1c67559b21aea7303a856d2f70105a12`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.584 — Additional Current Architecture Evidence: `src/Main.Verdict.cs`

### `src/Main.Verdict.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 212 lines / 8065 bytes.
- SHA-256: `f106b77d3b9ef1bd7ddae6b19a9c3c09b6624c7467d27ee6e575cfed7015b0d9`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=4; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public float LivingCumulativeDoseSieverts() {
```


# Appendix Q.585 — Additional Current Architecture Evidence: `src/UI/VerdictDashboardPanel.cs`

### `src/UI/VerdictDashboardPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 178 lines / 7701 bytes.
- SHA-256: `d1d57d14186f49a51faace91d3448d4971629702410ebb6ba660f31a7a64e322`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=4; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class VerdictDashboardPanel : Control
public event Action? OnClose;
public bool IsBound => _session != null;
public void Bind(VerdictPanel verdict, VerdictHostSession session) {
public override void _Ready() {
public void RefreshView() {
public void Open() {
public void Close() {
public override void _UnhandledInput(InputEvent @event) {
```


# Appendix Q.586 — Additional Current Architecture Evidence: `src/VerdictPanel.cs`

### `src/VerdictPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 432 lines / 17793 bytes.
- SHA-256: `880c980f41d127d45df8a9b2d4b42bb34d01603c53f4015a86e39b45345af50e`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=1; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class VerdictPanel : PanelContainer
public event System.Action? OnClose;
public void Open() {
public void Close() {
public override void _UnhandledInput(InputEvent @event) {
public override void _Ready() {
public void Bind(VerdictHostSession verdict) {
public void RefreshView() {
public int RenderedRadioRowCount() {
public delegate void NpcSpokenEventHandler(string npcId);
public override void _ExitTree() {
```


# Appendix Q.587 — Additional Current Architecture Evidence: `src/Host/PanelBindLifecycleSelfTest.cs`

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


# Appendix Q.588 — Additional Current Architecture Evidence: `src/Host/RetentionHostSession.cs`

### `src/Host/RetentionHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 240 lines / 10362 bytes.
- SHA-256: `aa47578888a778378ec9529448226fa9154ec7376d56d2bccd021e6cb70de9f1`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class RetentionOwnerResult
public readonly string CollectionKey;
public readonly int CountBefore;
public readonly int CountAfter;
public readonly int Pruned;
public sealed class RetentionHostSession : HostSessionBase
public const string PolicyFile = RetentionPolicyCatalogLoader.FileName;
public RetentionPolicyCatalog Catalog { get; }
public Ashfall.Core.KitchenNutritionSystem? Kitchen { get; set; }
public Ashfall.Core.YearOfAsh.FactionWarSystem? FactionWar { get; set; }
public Ashfall.Core.Verdict.MachineLogSystem? MachineLog { get; set; }
public Ashfall.Core.DoseLedgerSystem? DoseLedger { get; set; }
public bool UsingAuthoredPolicies { get; private set; }
public string? LoadError { get; private set; }
public int Passes { get; private set; }
public IReadOnlyList<RetentionOwnerResult> LastResults => _lastResults;
public void LoadAuthoredPolicies(string dataDirectory, IFileIO files) {
public void BindOwners( Ashfall.Core.KitchenNutritionSystem? kitchen, Ashfall.Core.YearOfAsh.FactionWarSystem? factionWar, Ashfall.Core.Verdict.MachineLogSystem? machineLog, Ashfall.Core.DoseLedgerSystem? doseLedger) {
public RetentionAuditReport ApplyRetention() {
public string Describe() {
public sealed class RetentionAuditState
public int schema_version { get; set; } = 1;
public int passes { get; set; }
public bool using_authored_policies { get; set; }
public int total_collections_audited { get; set; }
public int total_entries_pruned { get; set; }
public int total_protected_collections_preserved { get; set; }
public List<string> pruned_collection_keys { get; set; } = new List<string>();
public List<string> protected_collection_keys { get; set; } = new List<string>();
public static class RetentionSaveStore
public const string FileName = "retention_save.json";
public const string SectionName = "retention";
public static bool TrySave(RetentionAuditState state) => s_store.TrySave(state);
public static RetentionAuditState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(RetentionAuditState state) => s_store.CapturePersisted(state);
public static RetentionAuditState? TryRestore(string json) => s_store.RestoreEnvelope(json);
public static RetentionAuditState? TryRestoreBare(string json) => s_store.RestoreBare(json);
public static RetentionAuditState From(RetentionAuditReport report, int passes, bool usingAuthoredPolicies) => new RetentionAuditState
```


# Appendix Q.589 — Additional Current Architecture Evidence: `src/Main.PlayerSurfaces.cs`

### `src/Main.PlayerSurfaces.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1141 lines / 66230 bytes.
- SHA-256: `359ab8fa9162114b544f2a9629d5ea490a6cf866323880035b63ab47ab8c9056`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.590 — Additional Current Architecture Evidence: `src/Main.UiPanels.cs`

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


# Appendix Q.591 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/HostCliRegistry.cs`

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


# Appendix Q.592 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Medical/MedicalWardSystem.cs`

### `Assets/Ashfall.Core/Medical/MedicalWardSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 362 lines / 13288 bytes.
- SHA-256: `03800899c4d6f2d88ea6c4fe7ee3606d480adafa42d898bcfe697a234dedb06d`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MedicalWardSystem
public event Action<MedicalWardEvent>? OnWardChanged;
public event Action<string>? OnPatientAdmitted;
public Func<bool>? StaffingPreflight { get; set; }
public IReadOnlyList<MedicalBed> Beds => _beds;
public IReadOnlyList<MedicalProcedureDef> Procedures => _procedures;
public MedicalWardState State => _state;
public MedicalWardAdmissionResult Admit(string patientId, string bedId, int day) {
public MedicalWardAdmissionResult Discharge(string patientId, int day) {
public MedicalWardProcedureResult RunProcedure(string patientId, string procedureId, int day) {
public MedicalWardState CaptureState() => _state.Capture();
public void RestoreState(MedicalWardState state) {
public string? GetBedOccupant(string bedId) {
public MedicalAdmissionRecord? GetActiveAdmission(string patientId) {
public sealed class MedicalBed
public string BedId;
public string DisplayName;
public MedicalBedCategory Category;
public bool Isolation;
public enum MedicalBedCategory
public sealed class MedicalProcedureDef
public string ProcedureId;
public string DisplayName;
public string DelegatedSystemId; // e.g. "MedicalSystem", "DoseLedgerSystem"
public Dictionary<string, int> SupplyCost = new Dictionary<string, int>();
public float DurationHours;
public sealed class MedicalAdmissionRecord
public string PatientId;
public string BedId;
public int AdmittedDay;
public int DischargedDay;
public MedicalAdmissionStatus Status;
public enum MedicalAdmissionStatus
public sealed class MedicalProcedureRecord
public string PatientId;
public string ProcedureId;
public string BedId;
public int Day;
public sealed class MedicalWardState
public List<MedicalAdmissionRecord> Admissions = new List<MedicalAdmissionRecord>();
public List<MedicalProcedureRecord> ProceduresRun = new List<MedicalProcedureRecord>();
public void NormalizeAndValidate(IReadOnlyList<MedicalBed> beds) {
public MedicalWardState Capture() => new MedicalWardState
public void RestoreInto(MedicalWardState state, IReadOnlyList<MedicalBed> beds) {
public enum MedicalWardEventKind
public sealed class MedicalWardEvent
public MedicalWardEventKind Kind;
public string PatientId;
public string BedId;
public int Day;
public string Detail;
public sealed class MedicalWardAdmissionResult
public bool Succeeded;
public string ReasonCode;
public MedicalAdmissionRecord Record;
public static MedicalWardAdmissionResult Ok(MedicalAdmissionRecord r) => new MedicalWardAdmissionResult { Succeeded = true, ReasonCode = "ok", Record = r };
public static MedicalWardAdmissionResult Fail(string reason) => new MedicalWardAdmissionResult { Succeeded = false, ReasonCode = reason ?? "fail", Record = null! };
public sealed class MedicalWardProcedureResult
public bool Succeeded;
public string ReasonCode;
public string ProcedureId;
public Dictionary<string, int> SupplyCost;
public static MedicalWardProcedureResult Ok(string procedureId, Dictionary<string, int> cost) => new MedicalWardProcedureResult
public static MedicalWardProcedureResult Fail(string reason) => new MedicalWardProcedureResult
```


# Appendix Q.593 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs`

### `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 372 lines / 16531 bytes.
- SHA-256: `852e29665a9e622447d2c4e84e43ddd3439f6aeba89b24a03d8af0d836da4a7c`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=17; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class DosimeterDeviceState
public string deviceTag = string.Empty;
public string assignedSurvivorId = string.Empty;
public float batteryLevel = 1.0f;       // 0..1
public float sensorCondition = 1.0f;     // 0..1, degrades with use
public float calibrationQuality = 1.0f;  // 0..1, 1 = perfect calibration
public int readingsSinceCalibration = 0;
public int lastCalibrationDay = -1;
public int calibrationCount = 0;
public bool isOverdue = false;
public bool isStationOccupied = false;
public int stationOccupiedUntilDay = -1;
public float errorBandMsv = 0f;          // current measurement uncertainty (+/-)
public class DosimeterCalibrationState
public string systemId = DosimeterCalibrationSystem.SystemId;
public List<DosimeterDeviceState> devices = new List<DosimeterDeviceState>();
public class DosimeterCalibrationSystem
public const string SystemId = "dosimeter_calibration_system";
public const int ReadingsPerCalibration = 40;
public const float OverdueErrorBandMultiplier = 2.5f;
public const float CalibratedErrorBandMultiplier = 0.3f;
public const float BatteryDrainPerReading = 0.005f;
public const float SensorWearPerReading = 0.003f;
public const float MinBatteryForReading = 0.05f;
public const float MinSensorCondition = 0.1f;
public const int CalibrationDurationDays = 1;
public const float TestSourceExposureMsv = 5f;
public const float BaseErrorBandMsv = 15f;
public const float PerfectCalibrationErrorBandMsv = 3f;
public event Action<string> OnCalibrationStarted;        // deviceTag
public event Action<string> OnCalibrationCompleted;      // deviceTag
public event Action<string> OnCalibrationFailed;         // deviceTag
public event Action<string> OnDeviceConditionChanged;    // deviceTag
public event Action<string> OnReadingConfidenceChanged;  // deviceTag
public event Action<string> OnCalibrationOverdue;        // deviceTag
public event Action<DosimeterCalibrationState> OnStateChanged;
public DosimeterCalibrationState State => _state;
public IReadOnlyDictionary<string, DosimeterDeviceState> Devices => _devices;
public bool RegisterDevice(string deviceTag, string survivorId) {
public bool UnregisterDevice(string deviceTag) {
public void ConsumeReading(string deviceTag) {
public bool StartCalibration(string deviceTag, int currentDay) {
public bool CompleteCalibration(string deviceTag, int currentDay) {
public bool CancelCalibration(string deviceTag) {
public bool ReplaceBattery(string deviceTag) {
public bool ServiceSensor(string deviceTag) {
public float GetErrorBand(string deviceTag) {
public float GetConfidence(string deviceTag) {
public bool CanTakeReading(string deviceTag) {
public bool IsCalibrating(string deviceTag) {
public bool IsCalibrationComplete(string deviceTag, int currentDay) {
public DosimeterDeviceState? GetDevice(string deviceTag) {
public DosimeterCalibrationState CaptureState() {
public void RestoreState(DosimeterCalibrationState saved) {
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.
