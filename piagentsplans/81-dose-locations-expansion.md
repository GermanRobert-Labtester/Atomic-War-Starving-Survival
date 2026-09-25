# Plan 81 — Dose Location Geography and Radiation Attribution

> **Rebuild status:** COMPLETE 14-LOCATION DOSE GEOGRAPHY — ATTRIBUTION AND BALANCE MAINTENANCE
>
> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-2`
>
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round2-2026-09-25`
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

- The old plan’s premise that the catalog had three bunker rows is stale. The current authority has 14 unique locations across bunker, surface, expedition, external and faction sectors, with bounded risk and radiation values.
- The live flow is catalog → `DoseContentCatalog` → dose ledger attribution / `DoseGeographyPanel`; the ledger remains the owner of dose state and the catalog remains the owner of location metadata.
- A strong rebase protects the difference between an authored baseline rate, a ledger reading attribution, a weather modifier and an expedition exposure calculation. It must not turn display-only dose rows into a second exposure system.

**Bounded outcome:** Retire the old 3→12 expansion brief. Current `dose_locations.json` has 14 rows, the five standing bunker rooms remain preserved, nine authored external/surface/faction rows are present, and the dose geography panel and ledger attribution are live. The remaining work is a precise radiation-semantics and reachability audit.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `dose_locations.json` is present with 14 unique locations; `DoseContentCatalog.cs` loads location and item/quest content.
- `Plan81DoseLocationsExpansionTests` asserts at least 12, exactly 14 authored, five original bunker rooms, five sectors, bounded values and ledger attribution/restore.
- `DoseGeographyPanel` projects sector, risk, µSv/h, one-hour dwell and four-hour sortie values from the current catalog.
- Current tests provide executable evidence; fresh pass status still belongs to the focused runner, not this document.

**Master-authority sections applied to this rebase:**

- Master authority Volume 28 verification cookbook: focused evidence before broad gates.
- Lane D save/state/compatibility guidance: owner DTOs, migration and restore proof.
- Lane E UI/UX/accessibility guidance: truthful projections and keyboard/controller lifecycle.
- Lane G testing guidance: smallest affected target, negative cases and deterministic replay.
- Anti-padding protocol: content exhaustion may end the plan before the character checkpoint.
- Volume 32 fact projection and attribution guidance.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the 3→12 target with a 14-row census and sector/risk/rate matrix.
- Define the exact boundary between catalog baseline radiation, dose-ledger attribution, weather and expedition exposure.
- Verify every location has a truthful player-visible or host consumer path.
- Preserve original bunker rows and avoid creating a second location catalog.

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
| dose location/item/quest metadata | DoseContentCatalog | `Assets/Ashfall.Core/DoseContentCatalog.cs` | Sole dose content loader. |
| actual dose state and attribution | Dose ledger | `src/Host/DoseLedgerHostSession.cs; Assets/Ashfall.Core/DoseLedgerSystem.cs` | Owns exposure readings and history. |
| read-only location projection | Dose geography panel | `src/UI/DoseGeographyPanel.cs` | Does not calculate or mutate dose. |
| catalog, attribution and persistence proof | Dose focused tests | `Ashfall.Core.Tests/Radiation/Plan81DoseLocationsExpansionTests.cs; Ashfall.Core.Tests/DoseContentCatalogTests.cs` | Current executable evidence. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Dose Location Geography and Radiation Attribution
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ DoseContentCatalog
│   dose location/item/quest metadata
│ Dose ledger
│   actual dose state and attribution
│ Dose geography panel
│   read-only location projection
│ Dose focused tests
│   catalog, attribution and persistence proof
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

1. **Preserve current state ownership.** DoseContentCatalog owns dose location/item/quest metadata: Sole dose content loader.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| dose location/item/quest metadata | DoseContentCatalog | `Assets/Ashfall.Core/DoseContentCatalog.cs` | Sole dose content loader. |
| actual dose state and attribution | Dose ledger | `src/Host/DoseLedgerHostSession.cs; Assets/Ashfall.Core/DoseLedgerSystem.cs` | Owns exposure readings and history. |
| read-only location projection | Dose geography panel | `src/UI/DoseGeographyPanel.cs` | Does not calculate or mutate dose. |
| catalog, attribution and persistence proof | Dose focused tests | `Ashfall.Core.Tests/Radiation/Plan81DoseLocationsExpansionTests.cs; Ashfall.Core.Tests/DoseContentCatalogTests.cs` | Current executable evidence. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load dose locations
2. resolve sector/risk/baseline rate
3. read current ledger attribution/history
4. apply the existing weather/exposure modifiers at the owner seam
5. project dose geography panel
6. capture/restore ledger state

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Location metadata is static catalog data; readings, attribution and history are dose-ledger state.
- Radiation values are finite, positive and bounded; risk level is a presentation/authoring dimension, not a replacement for dose.
- A location can be read as context without implying that a survivor automatically entered it.
- Restore preserves ledger history and attribution without rewriting authored location rows.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- A dose location ID is unique and its displayed rate matches the catalog.
- The panel must distinguish baseline rate, recorded attribution and current exposure.
- Unknown location attribution fails closed and does not fabricate a reading.
- A UI refresh never consumes RNG or changes dose state.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `dose_locations.json` is the sole location authority.
- No cross-file copy of location IDs is needed; if a consumer needs a location, resolve the canonical catalog.
- New rows require sector semantics, bounded dose, a description and a consumer.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use the current dose-ledger save path.
- No new save section is needed for location metadata.
- Legacy ledger state with no attribution must restore neutrally and remain explainable.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Dose calculations use the existing seeded/deterministic owner stream.
- Catalog ordering must be stable and not hash-dependent.
- The same weather, location and ledger state produce the same attribution and projection.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Dose ledger readings and attribution are emitted by the current ledger owner.
- The geography panel refreshes from owner state and never creates a dose event.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/DoseLedgerHostSession.cs
- src/UI/DoseGeographyPanel.cs
- src/Main.Survivors.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Location descriptions are environmental and fictional.
- Dose geography should communicate risk and trade-offs without real-world disaster specificity.
- Do not use a catalog row as a hidden quest outcome.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A panel value diverges from the canonical row. | DoseContentCatalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A location is displayed as safe because it is unvisited. | Dose ledger | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | Attribution is applied twice through weather and expedition paths. | Dose geography panel | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A new row lacks a consumer or uses an unbounded dose. | Dose focused tests | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | Save restore loses ledger history. | DoseContentCatalog | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/DoseContentCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Radiation/Plan81DoseLocationsExpansionTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — census | Read current dose catalog, loader, panel and ledger owner. | 14-row current authority is confirmed. | No production path until the owning implementation package is separately claimed. |
| 1 — semantics matrix | Separate baseline, attribution, weather and exposure. | No semantic overlap remains. | No production path until the owning implementation package is separately claimed. |
| 2 — UI/route audit | Check every sector and row is truthfully projected. | No orphan or misleading row. | No production path until the owning implementation package is separately claimed. |
| 3 — balance review | Validate ranges and authored descriptions against current radiation owner. | No unsafe or duplicate exposure path. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/dose_locations.json | READ ONLY; MODIFY only for a proven content gap | 14-row authority |
| Assets/Ashfall.Core/DoseContentCatalog.cs | READ ONLY | Loader |
| src/Host/DoseLedgerHostSession.cs | READ ONLY | Ledger host |
| src/UI/DoseGeographyPanel.cs | READ ONLY | Projection |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Treating the catalog as the dose ledger. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Adding a second radiation calculation in UI. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Changing dose units or attribution semantics silently. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Overwriting the five original bunker rows. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new dose system.
- No arbitrary location count increase.
- No save-section change.
- No production edits in this rebase.

# 23. Rollback and Recovery

- Revert the plan file.
- Future data edits retain the prior valid catalog and focused attribution tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 14 current rows and five sectors are named.
- Attribution and exposure boundaries are explicit.
- No duplicate radiation authority is proposed.
- Focused catalog, ledger and UI checks are specified.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the 3→12 target with a 14-row census and sector/risk/rate matrix.
- Define the exact boundary between catalog baseline radiation, dose-ledger attribution, weather and expedition exposure.
- Verify every location has a truthful player-visible or host consumer path.
- Preserve original bunker rows and avoid creating a second location catalog.

## MUST NOT DO

- No new dose system.
- No arbitrary location count increase.
- No save-section change.
- No production edits in this rebase.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/DoseContentCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Radiation/Plan81DoseLocationsExpansionTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: dose location/item/quest metadata → DoseContentCatalog; actual dose state and attribution → Dose ledger; read-only location projection → Dose geography panel; catalog, attribution and persistence proof → Dose focused tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 81.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 81 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by DoseContentCatalog or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/DoseContentCatalog.cs`

### `Assets/Ashfall.Core/DoseContentCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 223 lines / 8490 bytes.
- SHA-256: `0dfe989872898d06f0482b1b234da837a59eb8f9e8ec76e6263cf30f72429935`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class DoseLocationDef
public string id = string.Empty;
public string displayName = string.Empty;
public string sector = string.Empty;
public int riskLevel;
public float radiationUsv;
public string description = string.Empty;
public class DoseItemDef
public string id = string.Empty;
public string name = string.Empty;
public float weightKg;
public float tradeValue;
public string category = string.Empty;
public string description = string.Empty;
public class DoseQuestDef
public string questlineId = string.Empty;
public string title = string.Empty;
public string synopsis = string.Empty;
public string factionTag = string.Empty;
public int minDay = 40;
public int maxDay = 360;
public List<DoseQuestStage> stages = new List<DoseQuestStage>();
public class DoseQuestStage
public string stageId = string.Empty;
public string title = string.Empty;
public string narrativePrompt = string.Empty;
public bool isTerminal;
public List<DoseQuestChoice> choices = new List<DoseQuestChoice>();
public class DoseQuestChoice
public string choiceId = string.Empty;
public string text = string.Empty;
public string nextStageId = string.Empty;
public int moraleDelta;
public int guiltDelta;
public string grantItemId = string.Empty;
public int grantItemQuantity;
public string outcomeNarrative = string.Empty;
public class DoseContentCatalog
public List<DoseLocationDef> locations = new List<DoseLocationDef>();
public List<DoseItemDef> items = new List<DoseItemDef>();
public List<QuestlineDefinition> quests = new List<QuestlineDefinition>();
internal sealed class DoseLocationsRoot
public int schema_version;
public List<DoseLocationDef> locations = new List<DoseLocationDef>();
internal sealed class DoseItemsRoot
public int schema_version;
public List<DoseItemDef> items = new List<DoseItemDef>();
public static class DoseContentCatalogLoader
public const string LocationsFile = "dose_locations.json";
public const string ItemsFile = "dose_items.json";
public const string QuestsFile = "dose_quests.json";
public static DoseContentCatalog Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static QuestlineDefinition? ToQuestlineDefinition(DoseQuestDef rq) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/DoseLedgerSystem.cs`

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


# Appendix B.04 — Current Code Architecture: `src/Host/DoseLedgerHostSession.cs`

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


# Appendix B.05 — Current Code Architecture: `src/UI/DoseGeographyPanel.cs`

### `src/UI/DoseGeographyPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 432 lines / 18300 bytes.
- SHA-256: `98e2b11413ad42058ccb5a1bf3a2332856399804e88538bf2b2f0c9931502f40`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=3; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class DoseGeographyPanel : Control
public event Action? OnClose;
public bool IsBound => _dose != null;
internal string RenderDump => _renderDump.ToString();
public void Bind(DoseLedgerHostSession? session) {
public void Unbind() {
public void RefreshView() {
internal static string RiskTier(int risk) => risk switch
public override void _Ready() {
public void Open() {
public void Close() => Visible = false;
public override void _UnhandledInput(InputEvent @event) {
public override void _ExitTree() {
```


# Appendix B.06 — Current Code Architecture: `src/Main.Survivors.cs`

### `src/Main.Survivors.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 320 lines / 13614 bytes.
- SHA-256: `d108f0909d7a2740f0cdd71e2acc8a5b9fce87beb7f7438253e8b5330972df4f`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=4; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public SurvivorsHostSession? Survivors => _survivors;
```


# Appendix C.07 — Catalog Census: `Assets/StreamingAssets/Data/dose_locations.json`

### `Assets/StreamingAssets/Data/dose_locations.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 5141 bytes / 5141 characters.
- SHA-256: `e04f67e7ff9136bbadac845bc060613eb083f517e2f8466c64bb935a2ef9be77`.
- Root keys: `locations`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
locations: min=14, max=14, observed_paths=1
```

Representative record fields:

- `description`
- `displayName`
- `id`
- `radiationUsv`
- `riskLevel`
- `sector`

Representative identifiers (ordered, capped for readability):

```text
loc_the_dose_room
loc_the_calibration_bench
loc_the_childrens_baseline_board
loc_the_register_hall
loc_the_screening_station
loc_shelter_exterior_approach
loc_surface_observation_post
loc_contaminated_water_access
loc_irradiated_forest_edge
loc_ruined_hospital_grounds
loc_military_depot_perimeter
loc_frozen_wetland_crossing
loc_burned_woodland_ridge
loc_garrison_checkpoint_gamma_exterior
```


# Appendix C.08 — Catalog Census: `Assets/StreamingAssets/Data/dose_items.json`

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


# Appendix D.09 — Existing Focused Test Inventory: `Ashfall.Core.Tests/DoseContentCatalogTests.cs`

### `Ashfall.Core.Tests/DoseContentCatalogTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 233; SHA-256: `8654d15f170cf0db4eecb909429ffc5111b6c74605166365ff4222b0c59848c5`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Load_FindsExpandedLocationsItemsQuests
Locations_AreTheFiveStandingRooms
Items_AreBooksToolsAndMedicine
Quests_ExposeStagesAndChoices_NoDeadEnds
Quests_HaveTheFourExpectedIds
QuestGateDays_MatchThePlan
GrantItems_ResolveAgainstItemCatalog
Host_RegistersContentQuestsIntoQuestlineSystem
```


# Appendix D.10 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Radiation/Plan81DoseLocationsExpansionTests.cs`

### `Ashfall.Core.Tests/Radiation/Plan81DoseLocationsExpansionTests.cs`

- Current test declarations: Fact=11, Theory=0, InlineData=0.
- File lines: 263; SHA-256: `d031903c878acb08642d49bd2db59b79732ebb88620c3dc5459ea9b77b2401de`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Load_FindsAtLeastTwelveLocations_ExactlyFourteenAuthored
PreservesAllOriginalFiveBunkerLocations
VerifiesAllNineNewLocationsExistWithCorrectSectors
HasExpectedSectorDistributionAcrossFiveSectors
AllLocationIdsAreUniqueAndFollowCanonicalPrefix
RiskLevelsWithinValidZeroToEightRange
RadiationDoseIsFinitePositiveAndBounded
AllLocationsHaveNonEmptyDisplayNamesAndEnvironmentalDescriptions
RiskLevelAndDoseCorrelationCoherent
DoseLedgerReadingAttributionWorksForNewLocations
DoseLedgerStateCaptureAndRestoreRoundTripPreservesLocationAttribution
```


# Appendix E.11 — Supporting Code Evidence: `Assets/Ashfall.Core/DoseRegistersCatalog.cs`

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


# Appendix E.12 — Supporting Code Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

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


# Appendix E.13 — Supporting Code Evidence: `src/Dose/DoseRegisterSurface.cs`

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


# Appendix E.14 — Supporting Code Evidence: `Assets/Ashfall.Core/Radiation/RadiationSystem.cs`

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


# Appendix E.15 — Supporting Code Evidence: `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs`

### `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 439 lines / 17985 bytes.
- SHA-256: `28fc9ffb8bf90fdaa26eeef1e40ef040855dd68d8ebf4df9026f527b4fe0cd74`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=12; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MedicalRecordTemplateDef
public string template_id { get; set; } = string.Empty;
public string record_type { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string severity_default { get; set; } = "mild";
public int typical_duration_days { get; set; } = 7;
public string description { get; set; } = string.Empty;
public sealed class MedicalRecordTemplatesCatalog
public int schema_version { get; set; } = 1;
public List<MedicalRecordTemplateDef> templates { get; set; } = new List<MedicalRecordTemplateDef>();
public sealed class MedicalRecord
public string RecordId { get; set; } = string.Empty;
public string SurvivorId { get; set; } = string.Empty;
public string RecordType { get; set; } = "illness"; // illness, injury, treatment, vaccination, radiation_exposure, chronic_condition, checkup
public int RecordedDay { get; set; } = 1;
public string Description { get; set; } = string.Empty;
public string Severity { get; set; } = "mild";       // mild, moderate, severe, critical
public int DurationDays { get; set; } = 0;
public string Outcome { get; set; } = "ongoing";     // ongoing, resolved, chronic, fatal
public List<string> TreatmentApplied { get; set; } = new List<string>();
public string TreatingMedicId { get; set; } = string.Empty;
public string Notes { get; set; } = string.Empty;
public sealed class HealthEvent
public string EventId { get; set; } = string.Empty;
public string SurvivorId { get; set; } = string.Empty;
public string EventType { get; set; } = "diagnosis"; // diagnosis, treatment, recovery, relapse, complication, vaccination, checkup
public int EventDay { get; set; } = 1;
public string Description { get; set; } = string.Empty;
public string RelatedCondition { get; set; } = string.Empty;
public string Outcome { get; set; } = "success";     // success, partial, failure
public string Notes { get; set; } = string.Empty;
public sealed class VaccinationRecord
public string VaccinationId { get; set; } = string.Empty;
public string SurvivorId { get; set; } = string.Empty;
public string VaccineType { get; set; } = string.Empty;
public int AdministeredDay { get; set; } = 1;
public string AdministeredByMedicId { get; set; } = string.Empty;
public float ImmunityLevel { get; set; } = 100.0f; // 0 to 100
public int ImmunityDurationDays { get; set; } = 60;
public int BoosterDueDay { get; set; } = 61;
public bool BoosterAlertFired { get; set; } = false;
public sealed class HealthTrend
public string TrendId { get; set; } = string.Empty;
public string SurvivorId { get; set; } = string.Empty;
public string HealthMetric { get; set; } = "overall_health"; // overall_health, radiation_dose, immune_strength, chronic_condition_count
public int MeasurementDay { get; set; } = 1;
public float Value { get; set; } = 100.0f;
public string Trend { get; set; } = "stable"; // improving, stable, declining
public sealed class HealthHistoryState
public int SchemaVersion { get; set; } = 1;
public int NextSequence { get; set; } = 1;
public List<MedicalRecord> Records { get; set; } = new List<MedicalRecord>();
public List<HealthEvent> Events { get; set; } = new List<HealthEvent>();
public List<VaccinationRecord> Vaccinations { get; set; } = new List<VaccinationRecord>();
public List<HealthTrend> Trends { get; set; } = new List<HealthTrend>();
public bool AutoRecordTreatments { get; set; } = true;
public bool ShowTrends { get; set; } = true;
public sealed class HealthHistorySystem
public event Action<MedicalRecord>? OnRecordAdded;
public event Action<MedicalRecord>? OnRecordResolved;
public event Action<HealthEvent>? OnHealthEventLogged;
public event Action<VaccinationRecord>? OnVaccinationAdministered;
public event Action<VaccinationRecord>? OnBoosterDueAlert;
public event Action<HealthTrend>? OnTrendRecorded;
public int TotalRecordsCount => _state.Records.Count;
public int TotalEventsCount => _state.Events.Count;
public int TotalVaccinationsCount => _state.Vaccinations.Count;
public int TotalTrendsCount => _state.Trends.Count;
public void LoadCatalog(string json) {
public IReadOnlyList<MedicalRecordTemplateDef> GetAllTemplates() => _templateDefs.Values.ToList();
public MedicalRecordTemplateDef? GetTemplate(string templateId) {
public MedicalRecord LogRecord( string survivorId, string recordType, string description, string severity, int day,
public bool ResolveRecord(string recordId, string outcome, int day, string notes = "") {
public HealthEvent LogHealthEvent( string survivorId, string eventType, string description, int day, string relatedCondition = "",
public VaccinationRecord AdministerVaccine( string survivorId, string vaccineType, int day, string medicId = "", float initialImmunity = 100.0f,
public List<HealthTrend> RecordDailyHealthTrend( string survivorId, int day, float overallHealth, float radiationDose, float immuneStrength,
public void TickDay(int day) {
public IReadOnlyList<MedicalRecord> GetSurvivorRecords(string survivorId) =>
public IReadOnlyList<HealthEvent> GetSurvivorEvents(string survivorId) =>
public IReadOnlyList<VaccinationRecord> GetVaccinations(string survivorId) =>
public float GetVaccinationImmunity(string survivorId, string vaccineType) {
public IReadOnlyList<HealthTrend> GetLatestTrends(string survivorId) {
public HealthHistoryState CaptureState() => _state;
public void RestoreState(HealthHistoryState? state) {
public HealthHistoryCensus GetCensus() {
public struct HealthHistoryCensus
public readonly int TotalRecords;
public readonly int TotalEvents;
public readonly int TotalVaccinations;
public readonly int TotalTrends;
public readonly int ActiveChronicConditions;
```


# Appendix G.16 — Supporting Regression Evidence: `Ashfall.Core.Tests/DoseLedgerSystemTests.cs`

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


# Appendix G.17 — Supporting Regression Evidence: `Ashfall.Core.Tests/NeedsRadiationSystemTests.cs`

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


# Appendix G.18 — Supporting Regression Evidence: `Ashfall.Core.Tests/SickListSystemTests.cs`

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


# Appendix G.19 — Supporting Regression Evidence: `Ashfall.Core.Tests/DoseRegistersCatalogTests.cs`

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


# Appendix H.20 — Supporting Authority Document: `docs/CURRENT_AUTHORITY.md`

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
| dose location/item/quest metadata | DoseContentCatalog | actual dose state and attribution | Dose ledger | Owner emits/reads a typed fact; no mirror state. |
| dose location/item/quest metadata | DoseContentCatalog | read-only location projection | Dose geography panel | Owner emits/reads a typed fact; no mirror state. |
| dose location/item/quest metadata | DoseContentCatalog | catalog, attribution and persistence proof | Dose focused tests | Owner emits/reads a typed fact; no mirror state. |
| actual dose state and attribution | Dose ledger | dose location/item/quest metadata | DoseContentCatalog | Owner emits/reads a typed fact; no mirror state. |
| actual dose state and attribution | Dose ledger | read-only location projection | Dose geography panel | Owner emits/reads a typed fact; no mirror state. |
| actual dose state and attribution | Dose ledger | catalog, attribution and persistence proof | Dose focused tests | Owner emits/reads a typed fact; no mirror state. |
| read-only location projection | Dose geography panel | dose location/item/quest metadata | DoseContentCatalog | Owner emits/reads a typed fact; no mirror state. |
| read-only location projection | Dose geography panel | actual dose state and attribution | Dose ledger | Owner emits/reads a typed fact; no mirror state. |
| read-only location projection | Dose geography panel | catalog, attribution and persistence proof | Dose focused tests | Owner emits/reads a typed fact; no mirror state. |
| catalog, attribution and persistence proof | Dose focused tests | dose location/item/quest metadata | DoseContentCatalog | Owner emits/reads a typed fact; no mirror state. |
| catalog, attribution and persistence proof | Dose focused tests | actual dose state and attribution | Dose ledger | Owner emits/reads a typed fact; no mirror state. |
| catalog, attribution and persistence proof | Dose focused tests | read-only location projection | Dose geography panel | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the 3→12 target with a 14-row census and sector/risk/rate matrix. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Define the exact boundary between catalog baseline radiation, dose-ledger attribution, weather and expedition exposure. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Verify every location has a truthful player-visible or host consumer path. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Preserve original bunker rows and avoid creating a second location catalog. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.552 — Additional Current Architecture Evidence: `src/UI/DoseLedgerPanel.cs`

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


# Appendix Q.553 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Disease/DiseaseTriage.cs`

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


# Appendix Q.554 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/DoseLedgerSave.cs`

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


# Appendix Q.555 — Additional Current Architecture Evidence: `src/Host/SurvivorsHostSession.cs`

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


# Appendix Q.556 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`

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


# Appendix Q.557 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radiation/ExposureBreakdown.cs`

### `Assets/Ashfall.Core/Radiation/ExposureBreakdown.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 110 lines / 5549 bytes.
- SHA-256: `d73fa2620a7ecccdd9211fc1c2ba641a5efc0fecb70c538350ac06c932413f57`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExposureBreakdown
public SurvivorExposureLocation LocationKind { get; set; } = SurvivorExposureLocation.ShelterInterior;
public string LocationId { get; set; } = string.Empty;
public string PositionLabel { get; set; } = string.Empty;
public float ZoneAmbient { get; set; }
public float BaseRadRate { get; set; }
public float WeatherModifier { get; set; }
public float FalloutContamination { get; set; }
public float AnomalyRate { get; set; }
public float ShelterShielding { get; set; }
public float GearProtection { get; set; }
public float EffectiveExposurePerHour { get; set; }
public float AccumulatedDose { get; set; }
public float LifetimeDose { get; set; }
public static ExposureBreakdown Build( ExposureEnvironment env, float gearProtection, float accumulatedDose, float lifetimeDose, float? interiorRads = null,
public static string BuildPositionLabel( SurvivorExposureLocation kind, string? locationId, string? locationDisplayName = null) {
public string ToDisplayLine() {
```


# Appendix Q.558 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs`

### `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 532 lines / 24978 bytes.
- SHA-256: `8ac6c3f37ec2c6f5cdda67f00f04766c7c1ae43582ceb0698379c9a8ef6cc1e0`.
- Architecture signals: seeded references=3; save/restore symbols=4; typed event declarations=23; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum RadiationSicknessPhase
public class PhaseProgressionState
public string Id = string.Empty;
public bool IsAlive = true;
public RadiationSicknessPhase Phase = RadiationSicknessPhase.Healthy;
public float PhaseHoursElapsed;
public float AcuteDoseWindow;
public float LatentDamage;
public float OnsetTimer;
public float IodineProtectionTimer;
public float LungCapacity = 100f;
public bool HasPermanentLungDamage;
public bool HasTerminalPrognosis;
public float TerminalPrognosisDaysRemaining;
public float Health = 100f;
public bool IsResting;
public class PhaseProgressionSaveState
public string systemId = RadiationPhaseProgression.SystemId;
public List<PhaseProgressionSurvivorSave> survivors = new List<PhaseProgressionSurvivorSave>();
public class PhaseProgressionSurvivorSave
public string survivorId = string.Empty;
public string phase = "Healthy";
public float phaseHoursElapsed;
public float acuteDoseWindow;
public float latentDamage;
public float onsetTimer;
public float iodineProtectionTimer;
public float lungCapacity = 100f;
public bool hasPermanentLungDamage;
public bool hasTerminalPrognosis;
public float terminalPrognosisDaysRemaining;
public class RadiationPhaseProgression
public const string SystemId = "radiation_phase_progression";
public const float AcuteDoseWindowDecayPerDay = 0.75f;
public const float ProdromalTriggerDose = 100f;
public const float ChronicDamageFactor = 0.05f;
public const float AcuteLatentDamageFactor = 1f;
public const float LatentDamageChronicThreshold = 100f;
public const float IodineWindowHours = 12f;
public const float IodineMitigationFactor = 0.35f;
public const float LatentDamageSeverityReference = 60f;
public const float ProdromalDurationHours = 24f;
public const float LatentMinDurationHours = 144f;   // 6 days
public const float LatentMaxDurationHours = 288f;   // 12 days
public const float ManifestMinDurationHours = 72f;  // ~3 days
public const float ManifestMaxDurationHours = 96f;  // 4 days
public const float ProdromalHealthDip = 5f;
public const float ProdromalMoraleDip = 10f;
public const float ProdromalFatigueRate = 1.5f;
public const float ManifestHealthCrashMin = 30f;
public const float ManifestHealthCrashMax = 150f;
public const float ManifestBleedPerDay = 8f;
public const float BedRestMitigation = 0.6f;
public const float ChronicFibrosisLungCapacityMin = 0.40f;
public const float ChronicFibrosisLungCapacityMax = 0.70f;
public const float ChronicFibrosisThreshold = 120f;
public event Action<string, RadiationSicknessPhase, RadiationSicknessPhase> OnPhaseChanged;
public event Action<string, float> OnLungCapacityReduced;
public event Action<string, float> OnTerminalPrognosisDeclared;
public event Action<string, float> OnHealthDeltaRequested;
public event Action<string, float> OnMoraleDeltaRequested;
public event Action<string> OnChronicIllnessRequested;
public event Action<string> OnChronicFibrosisMarked;
public event Action<string> OnRadiationDoseResetRequested;
public event Action OnStateChanged;
public IReadOnlyDictionary<string, PhaseProgressionState> Survivors => _survivors;
public void Register(PhaseProgressionState state) {
public void Unregister(string survivorId) {
public void OnExposure(string survivorId, float dose) {
public void AdministerIodine(string survivorId) {
public void Tick(float gameHours) {
public string GetPhasePrognosisText(string survivorId) {
public RadiationSicknessPhase GetPhase(string survivorId) {
public float GetOnsetTimer(string survivorId) {
public float GetLatentDamage(string survivorId) {
public PhaseProgressionSaveState CaptureState() {
public void RestoreState(PhaseProgressionSaveState saved) {
```


# Appendix Q.559 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/DecontaminationSystem.cs`

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


# Appendix Q.560 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs`

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


# Appendix Q.561 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radiation/ExposureEnvironment.cs`

### `Assets/Ashfall.Core/Radiation/ExposureEnvironment.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 338 lines / 15012 bytes.
- SHA-256: `ec0a0d2fd65c2cb02b1e657049da3a88ac9fe0f1bbd1a70cbadeb42f22eb831e`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum SurvivorExposureLocation
public sealed class ExposureEnvironment
public SurvivorExposureLocation LocationKind { get; set; } = SurvivorExposureLocation.ShelterInterior;
public string LocationId { get; set; } = string.Empty;
public float BaseRadRate { get; set; }
public float WeatherRadModifier { get; set; }
public float FalloutContamination { get; set; }
public float AnomalyRadRate { get; set; }
public float ShelterShielding { get; set; }
public float EffectiveZoneRadLevel { get; set; }
public string ExposureReason { get; set; } = string.Empty;
public Func<float, float>? InteriorRadQuery { get; set; }
public ExposureContext ToExposureContext(List<WornGear>? wornGear = null) {
public override string ToString() =>
public sealed class ExposureEnvironmentResolver
public const float DefaultShelterInteriorRadRate = 2.0f;
public const float DefaultShelterPerimeterRadRate = 20.0f;
public const float DefaultWastelandOutdoorRadRate = 40.0f;
public float ShelterInteriorBaseRadRate { get; set; } = DefaultShelterInteriorRadRate;
public float ShelterPerimeterBaseRadRate { get; set; } = DefaultShelterPerimeterRadRate;
public float WastelandOutdoorBaseRadRate { get; set; } = DefaultWastelandOutdoorRadRate;
public Func<float>? ShelterAttenuationProvider { get; set; }
public Func<float>? WeatherRadModifierProvider { get; set; }
public Func<string, float>? LocationRadRateProvider { get; set; }
public Func<string, float>? FalloutContaminationProvider { get; set; }
public Func<string, float>? AnomalyRadRateProvider { get; set; }
public Func<float, float>? ShelterInteriorRadQuery { get; set; }
public void SetSurvivorLocation(string survivorId, SurvivorExposureLocation kind, string locationId = "") {
public void ClearSurvivorLocation(string survivorId) {
public ExposureEnvironment Resolve(string survivorId) {
public ExposureEnvironment ResolveForEnvironment(SurvivorExposureLocation kind, string locationId = "") {
public static Dictionary<string, float> LoadLocationRadRates( string dataDir, IFileIO? fileIO = null, IJsonSerializer? json = null) {
```


# Appendix Q.562 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/World/AnomalyHazardSystem.cs`

### `Assets/Ashfall.Core/World/AnomalyHazardSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 646 lines / 31342 bytes.
- SHA-256: `381195c245ef61bf6ddd41d714b35165bebc4ca49b6531b99be857214347077a`.
- Architecture signals: seeded references=1; save/restore symbols=2; typed event declarations=8; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum HazardDetection
public sealed class AnomalyHazardInstance
public string hazard_id { get; set; } = string.Empty;      // hazard_anomaly_<n>_<anomalyId>
public string anomaly_id { get; set; } = string.Empty;     // authored definition id
public float position_x { get; set; }
public float position_y { get; set; }
public float bearing_deg { get; set; }                     // storm_front travel bearing (0=N, 90=E)
public float radius_km { get; set; }
public float radiation_rate { get; set; }                  // authored center rate (bp-free, rads/hr)
public string movement_profile { get; set; } = "static";
public float movement_speed_kph { get; set; }
public float wind_response { get; set; }
public float intensity { get; set; } = 1f;                 // decays linearly with age
public int age_days { get; set; }
public int duration_days { get; set; }
public int spawn_day { get; set; }
public string warning_profile { get; set; } = "standard";
public float warning_radius_km { get; set; }
public float detection_threshold { get; set; }
public string loot_table_id { get; set; } = string.Empty;
public int wildlife_modifier_bp { get; set; }
public bool active { get; set; } = true;
public List<string> fired_warning_keys { get; set; } = new List<string>();
public sealed class AnomalyLootSiteState
public string site_id { get; set; } = string.Empty;        // hazard_anomaly_1_anomaly_x_site_1
public string hazard_id { get; set; } = string.Empty;
public string loot_table_id { get; set; } = string.Empty;  // canonical table reference
public bool resolved { get; set; }
public int resolved_day { get; set; }
public sealed class AnomalyHazardSystemState
public string system_id { get; set; } = "anomaly_hazard";
public int schema_version { get; set; } = 1;
public int hazard_counter { get; set; }
public int last_tick_day { get; set; }
public List<AnomalyHazardInstance> hazards { get; set; } = new List<AnomalyHazardInstance>();
public List<AnomalyLootSiteState> loot_sites { get; set; } = new List<AnomalyLootSiteState>();
public sealed class AnomalySpawnResult
public bool Success;
public string ReasonCode = string.Empty;                    // unknown_anomaly | hazard_cap_reached | invalid_position
public AnomalyHazardInstance? Hazard;
public static AnomalySpawnResult Fail(string reason) => new AnomalySpawnResult { Success = false, ReasonCode = reason };
public sealed class AnomalyLootResolution
public bool Success;
public string ReasonCode = string.Empty;                   // unknown_loot_site | loot_already_resolved | hazard_expired
public string HazardId = string.Empty;
public string LootTableId = string.Empty;
public sealed class AnomalyApproachWarning
public string HazardId = string.Empty;
public string AnomalyId = string.Empty;
public string TargetId = string.Empty;                     // usually the shelter zone id
public float DistanceKm;
public float EtaDays;                                      // distance / daily advance
public string DirectionCardinal = string.Empty;            // hazard travel bearing as cardinal text
public string IntensityBand = string.Empty;                // light | moderate | severe
public float Confidence;                                   // authored from warning profile
public sealed class AnomalyHazardSystem
public const string SystemId = "anomaly_hazard";
public const int MaxConcurrentHazards = 4;
public const float MaxCombinedRadiationRate = 600f;
public const float WorldMinKm = 0f;
public const float WorldMaxKm = 512f;
public const float WanderMaxDegrees = 15f;
public const float ExpireIntensityFloor = 0.05f;
public const float ConfidenceEarly = 0.9f;
public const float ConfidenceStandard = 0.7f;
public const float ConfidenceLate = 0.5f;
public const float ConfidenceSignature = 0.4f;
public const float ConfidenceContact = 1.0f;
public const float GeigerCapabilityRadsPerHour = 60f;
public const float DosimeterCapabilityRadsPerHour = 15f;
public event Action<AnomalyHazardInstance>? OnHazardSpawned;
public event Action<AnomalyHazardInstance>? OnHazardExpired;
public event Action<AnomalyApproachWarning>? OnStormApproaching;
public event Action<AnomalyHazardInstance>? OnHazardContact; // hazard began overlapping a target
public AnomalyHazardSystemState State => _state;
public AnomalyDefinitionCatalog Catalog => _catalog;
public void BindCatalog(AnomalyDefinitionCatalog catalog) {
public AnomalyDefinition? Definition(string anomalyId) => _catalog.Find(anomalyId);
public AnomalySpawnResult TrySpawn(string anomalyId, float x, float y, int day, float bearingDeg = -1f) {
public void TickDay(int day, float windDirDeg, float windSpeedKph, ISeededRng? variationRng) {
public float GetRadiationRate(float x, float y) {
public List<string> GetEnvironmentalEffectTags(float x, float y) {
public float GetWildlifeModifier(float x, float y) {
public IReadOnlyList<AnomalyHazardInstance> GetOverlappingHazardIds(float x, float y) {
public HazardDetection ClassifyDetection(float x, float y, float detectorCapability) {
public float GetDetectionConfidence(float x, float y, float detectorCapability) {
public void EvaluateApproach(string targetId, float targetX, float targetY) {
public float WindDailyAdvanceHintKm { get; set; }
public AnomalyLootResolution TryResolveLootSite(string siteId, int day) {
public IReadOnlyList<AnomalyLootSiteState> LootSites => _state.loot_sites;
public AnomalyHazardSystemState CaptureState() {
public void RestoreState(AnomalyHazardSystemState? state) {
```


# Appendix Q.563 — Additional Current Architecture Evidence: `src/Main.UiPanels.cs`

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


# Appendix Q.564 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/AutopsySystem.cs`

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


# Appendix Q.565 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`

### `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1579 lines / 71512 bytes.
- SHA-256: `6a179c24dead762831790f2f5aa3d322792b33de20ba91fe5cd651ffcbd31ed7`.
- Architecture signals: seeded references=7; save/restore symbols=2; typed event declarations=45; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum ExpeditionStance
public enum ExpeditionPhase
public class ExpeditionLootEntry
public string itemId = string.Empty;
public int quantity = 0;
public float weightKg = 0f;
public class CampShelterAssignment
public string survivorId = string.Empty;
public bool hasTent = false;
public bool hasBedroll = false;
public string shelterType = "none"; // none, lean_to, tent, cave
public class CampWatchShift
public string survivorId = string.Empty;
public int shiftIndex = 0;       // 0 = first half, 1 = second half
public float alertness = 1.0f;   // 0..1, degrades with fatigue
public bool isActive = false;
public class CampState
public int campStartDay = 0;
public float campStartHour = 0f;
public int nightSegmentsCompleted = 0;
public int totalNightSegments = 4;  // 4 segments = one night
public float firewoodRemaining = 0f;
public float firewoodConsumed = 0f;
public float heatOutput = 0f;       // degrees C added
public float waterReserved = 0f;
public float waterConsumed = 0f;
public float foodReserved = 0f;
public float foodConsumed = 0f;
public float temperatureC = 0f;    // ambient at camp
public string weatherCondition = "Clear";
public float coldExposure = 0f;    // accumulated cold damage
public float radiationExposure = 0f;
public int wildlifeThreatLevel = 0;
public bool encounterTriggered = false;
public string encounterKey = string.Empty;
public bool encounterResolved = false;
public string campOutcome = string.Empty; // resume, retreat, injury, loss, failed
public List<CampShelterAssignment> shelterAssignments = new List<CampShelterAssignment>();
public List<CampWatchShift> watchShifts = new List<CampWatchShift>();
public sealed class ExpeditionVehicleProfile
public string vehicleId = string.Empty;
public float speedMultiplier = 1f;
public float cargoCapacityKg = 0f;
public float breakdownChancePerTick = 0f;
public float fuelPerTravelTick = 0f;
public sealed class ExpeditionEstimate
public string locationId = string.Empty;
public string stance = string.Empty;
public int distanceTicks;
public float outboundTicks;
public float inboundTicks;
public float lootingTicks;
public float totalTicks;
public float cargoCapacityKg;
public float fuelRequired;
public float breakdownRiskPerTick;
public float breakdownRiskTotal;
public float encounterRiskPerTick;
public float weaponReadiness = 1f;
public float weaponJamRisk;
public bool usingVehicle;
public float partyProtection;
public int unprotectedCount;
public float projectedDosePerHour;
public float projectedDoseTotal;
public float projectedTripHours;
public float projectedGearWear;
public float protectiveLifeHours;
public bool predictsMidRouteFailure;
public float weatherSpeedMultiplier = 1f;
public float weatherEncounterMultiplier = 1f;
public float survivorSpeedMultiplier = 1f;
public sealed class ExpeditionProtectiveInputs
public float LocationRadRatePerHour;
public float WorkingProtection;
public int UnprotectedCount;
public float WeakestGearDegradeRate;
public float WeakestGearDurability;
public float WearMultiplier = 1f;
public float HoursPerTick = 1f;
public sealed class ExpeditionWeatherInputs
public float SpeedMultiplier = 1.0f;
public float EncounterMultiplier = 1.0f;
public class ExpeditionState
public string systemId = ExpeditionSystem.SystemId;
public string expeditionId = string.Empty;
public string survivorId = string.Empty;
public string locationId = string.Empty;
public string displayName = string.Empty;
public string stance = "Stealth";
public int phase = (int)ExpeditionPhase.Outbound;
public int startedDay = 0;
public int distanceTicks = 0;
public int travelTicksCompleted = 0;
public int lootingTicksCompleted = 0;
public float stamina = 100f;
public float maxLootCapacityKg = 40f;
public float currentWeightKg = 0f;
public int dangerLevel = 1;
public float encounterChancePerTick = 0.12f;
public int encounterCount = 0;
public bool isPushingLuck = false;
public bool isNightScavenge = false;
public bool hasBicycle = false;
public bool hasFlashlight = false;
public string vehicleId = string.Empty;
public float vehicleSpeedMultiplier = 1f;
public float weatherSpeedMultiplier = 1f;
public float survivorSpeedMultiplier = 1f;
public float vehicleBreakdownChancePerTick = 0f;
public bool vehicleBrokenDown = false;
public string outcomeText = string.Empty;
public List<ExpeditionLootEntry> loot = new List<ExpeditionLootEntry>();
public CampState campState = new CampState();
public class ExpeditionDefinition
public string id = string.Empty;
public string displayName = string.Empty;
public int distanceTicks = 8;
public int dangerLevel = 1;
public float encounterChancePerTick = 0.12f;
public float baseStaminaDrainPerHour = 2.0f;
public List<string> lootCategories = new List<string>();
public string scavenging_table_id = string.Empty;
public bool requiresDiscovery = false;
public class ExpeditionSystem
public const string SystemId = "expedition_system";
public const float MaxStamina = 100f;
public const int AutoRetreatAfterLootTicks = 3;
public const float EncumberPenaltyPerTickMax = 15f;
public const int CampNightSegments = 4;
public const float CampFirewoodPerSegment = 2.0f;
public const float CampHeatPerFirewood = 3.0f;     // degrees C per unit
public const float CampWaterPerSegment = 0.5f;
public const float CampFoodPerSegment = 0.5f;
public const float CampColdDamageThresholdC = -5f;  // below this, cold damage
public const float CampColdDamagePerSegment = 5f;   // HP per segment below threshold
public const float CampStaminaRecoveryPerSegment = 8f;
public const float CampEncounterChanceBase = 0.15f;
public const float CampSentryDetectionBonus = 0.3f; // reduces encounter chance
public ScavengingTableCatalog? ScavengingCatalog { get; set; }
public Func<string, bool>? IsItemGenerationAvailable { get; set; }
public Action<string>? OnItemGenerationCommitted { get; set; }
public DamagedMapSystem? DamagedMap { get; set; }
public event Action<ExpeditionState> OnExpeditionStarted;
public event Action<string>? OnLocationDiscovered;
public event Action<ExpeditionState> OnExpeditionTick;
public event Action<ExpeditionState> OnPhaseChanged;
public event Action<ExpeditionState> OnLootAdded;                 // state, itemId, qty
public event Action<ExpeditionState> OnEncounterTriggered;
public event Action<ExpeditionState> OnVehicleBreakdown;
public event Action<ExpeditionState> OnExpeditionCompleted;
public event Action<ExpeditionState, string> OnExpeditionFailed;
public event Action<ExpeditionState> OnStateChanged;
public event Action<ExpeditionState> OnCampEntered;
public event Action<ExpeditionState> OnCampSuppliesReserved;
public event Action<ExpeditionState> OnCampNightSegmentResolved;
public event Action<ExpeditionState> OnCampEncounterSurfaced;
public event Action<ExpeditionState> OnCampEncounterResolved;
public event Action<ExpeditionState> OnCampDawnResolved;
public void SetStaminaDrainMultiplier(Func<string, float> multiplier) {
public void SetSurvivorSpeedMultiplierQuery(Func<string, float> query) {
public void SetPackCapacityBonusQuery(Func<string, float>? query) {
public void SetEncounterChanceMultiplier(Func<string, float> multiplier) {
public IReadOnlyDictionary<string, ExpeditionState> Active => _active;
public int ActiveCount => _active.Count;
public int CompletedCount => _completedCount;
public bool Start( ExpeditionDefinition def, string survivorId, int day, ExpeditionStance stance = ExpeditionStance.Stealth, bool isNightScavenge = false,
public CommandPreview PreviewStart( ExpeditionDefinition def, string survivorId, int day, ExpeditionStance stance = ExpeditionStance.Stealth, bool isNightScavenge = false,
public CommandResult ExecuteStart( ExpeditionDefinition def, string survivorId, int day, ExpeditionStance stance = ExpeditionStance.Stealth, bool isNightScavenge = false,
public CommandPreview PreviewPushLuck(string survivorId, long stateVersion = 0) {
public CommandResult ExecutePushLuck(string survivorId, long expectedStateVersion = 0, long currentStateVersion = 0) {
public CommandPreview PreviewRetreat(string survivorId, long stateVersion = 0) {
public CommandResult ExecuteRetreat(string survivorId, long expectedStateVersion = 0, long currentStateVersion = 0) {
public static ExpeditionEstimate Estimate( ExpeditionDefinition def, ExpeditionStance stance, bool isNightScavenge = false, ExpeditionVehicleProfile? vehicle = null, float weaponReadiness = 1f,
public void TickHours(float hours, ISeededRng rng) {
public bool PushLuck(string survivorId) {
public bool Retreat(string survivorId) {
public bool EnterCamp( string survivorId, int day, float hour, float temperatureC, string weatherCondition,
public bool ReserveCampSupplies( string survivorId, float firewood, float water, float food) {
public bool CampTick(string survivorId, ISeededRng rng) {
```


# Appendix Q.566 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Factions/FactionCovertOpsCoordinator.cs`

### `Assets/Ashfall.Core/Factions/FactionCovertOpsCoordinator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 559 lines / 25887 bytes.
- SHA-256: `b7510ea1ef688833b260d2a5d7ef393db2a5066ab633440c347e75882eccb416`.
- Architecture signals: seeded references=2; save/restore symbols=4; typed event declarations=7; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum CovertOperationStatus
public enum SuspicionBand
public sealed class EspionageOperationDef
public string OperationId { get; set; } = string.Empty;
public string Name { get; set; } = string.Empty;
public string OperationType { get; set; } = "infiltrate"; // infiltrate, steal, sabotage, propaganda, assassinate
public string TargetFactionId { get; set; } = string.Empty;
public float BaseSuccessRate { get; set; } = 0.60f;
public string RiskLevel { get; set; } = "medium";
public int BaseDurationDays { get; set; } = 7;
public int SuspicionGain { get; set; } = 15;
public string IntelType { get; set; } = "military";
public int IntelValue { get; set; } = 40;
public string Description { get; set; } = string.Empty;
public sealed class ActiveCovertOperation
public string OperationId { get; set; } = string.Empty;
public string OperationType { get; set; } = string.Empty;
public string TargetFactionId { get; set; } = string.Empty;
public string AssignedAgentId { get; set; } = string.Empty;
public int StartDay { get; set; } = 1;
public int DurationDays { get; set; } = 7;
public int RemainingDays { get; set; } = 7;
public CovertOperationStatus Status { get; set; } = CovertOperationStatus.Active;
public float SuccessChance { get; set; } = 0.60f;
public sealed class IntelligenceReport
public string ReportId { get; set; } = string.Empty;
public string SourceFactionId { get; set; } = string.Empty;
public string IntelligenceType { get; set; } = "military";
public int Value { get; set; } = 40;
public float Accuracy { get; set; } = 0.85f;
public int DayObtained { get; set; } = 1;
public bool IsDecoded { get; set; } = false;
public string Summary { get; set; } = string.Empty;
public sealed class CovertOpsCatalog
public IReadOnlyCollection<EspionageOperationDef> AllOperations => _operations.Values;
public float DailyCoolingRate { get; set; } = 2.0f;
public bool TryGetOperation(string operationId, out EspionageOperationDef op) {
public static CovertOpsCatalog LoadFromJson(string json) {
public sealed class FactionCovertOpsCoordinator
public delegate void OperationLaunchedDelegate(string operationId, string agentId, string targetFaction);
public delegate void OperationResolvedDelegate(string operationId, string agentId, CovertOperationStatus status);
public delegate void IntelligenceDecodedDelegate(string reportId, string factionId, string intelType, int value);
public delegate void SuspicionChangedDelegate(string factionId, float newSuspicion, SuspicionBand band);
public delegate void AgentCompromisedDelegate(string agentId, string factionId, string reason);
public event OperationLaunchedDelegate? OnCovertOperationLaunchedSeam;
public event OperationResolvedDelegate? OnCovertOperationResolvedSeam;
public event IntelligenceDecodedDelegate? OnIntelligenceDecodedSeam;
public event SuspicionChangedDelegate? OnFactionSuspicionChangedSeam;
public event AgentCompromisedDelegate? OnAgentCompromisedSeam;
public CovertOpsCatalog Catalog => _catalog;
public IReadOnlyList<ActiveCovertOperation> ActiveOperations => _activeOperations;
public IReadOnlyList<IntelligenceReport> Reports => _reports;
public float GetSuspicion(string factionId) {
public SuspicionBand GetSuspicionBand(string factionId) {
public void AddSuspicion(string factionId, float delta) {
public bool LaunchOperation(string operationId, string agentId, float agentStealthSkill = 1.0f, int currentDay = 1) {
public void AdvanceDay(ISeededRng rng, int currentDay) {
public void ResolveOperation(ActiveCovertOperation op, ISeededRng rng, int currentDay, bool forceSuccess = false, bool forceCompromise = false) {
public bool DecodeIntelligenceReport(string reportId) {
public string CaptureState() {
public void RestoreState(string json) {
```


# Appendix Q.567 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Medical/MedicalWardSystem.cs`

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


# Appendix Q.568 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/SickListSystem.cs`

### `Assets/Ashfall.Core/SickListSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 190 lines / 7273 bytes.
- SHA-256: `58076a921f6f9cef0b10258b355a49b8400d5ce9599ae69cf7edb111a79fd11b`.
- Architecture signals: seeded references=0; save/restore symbols=5; typed event declarations=8; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class SickBand
public string survivorId;
public int band;
public int diagnosedDay;
public int releaseDay = -1;
public string palliativePlan;
public string severitySource;
public string sourceId;
public class SickListSystemState
public string systemId = SickListSystem.SystemId;
public List<SickBand> bands = new List<SickBand>();
public class SickListSystem
public const string SystemId = "sick_list_system";
public const string SourceDose = "dose";
public const string SourceIllness = "illness";
public event Action<string, int> OnDiagnosed;
public event Action<string> OnReleased;
public event Action<string, string> OnPalliativeAssigned;
public event Action<SickListSystemState> OnStateChanged;
public SickListSystemState State => CaptureState();
public IReadOnlyList<SickBand> Bands => CaptureState().bands;
public bool Diagnose(string survivorId, int band, int day) =>
public bool Diagnose(string survivorId, int band, int day, string severitySource, string sourceId) {
public bool Release(string survivorId, int day) {
public bool AssignPalliative(string survivorId, string plan) {
public SickBand? GetBand(string survivorId) {
public SickListSystemState CaptureState() {
public void RestoreState(SickListSystemState saved) {
```


# Appendix Q.569 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/UI/CrisisPresentationCoordinator.cs`

### `Assets/Ashfall.Core/UI/CrisisPresentationCoordinator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 460 lines / 19159 bytes.
- SHA-256: `f9fa108ddfc1e6c5d77e0114ebcf3622b4739b0bf7f5efd85580483a491b8486`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=3; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CrisisPresentationCoordinator
public CrisisPresentationSnapshot CurrentSnapshot => _currentSnapshot;
public event Action<CrisisPresentationSnapshot>? OnCrisisChanged;
public void Bind( PowerGridSystem? power, DiseaseSystem? disease, WeatherSystem? weather, StartingLevelSystem? startingLevel = null, RadiationSystem? radiation = null,
public void TriggerCustomCrisis(CrisisPresentationSnapshot snapshot) {
public void ClearCrisis() {
public void AcknowledgeCurrentCrisis() {
public void EvaluateCrisisState() {
```


# Appendix Q.570 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs`

### `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 462 lines / 19109 bytes.
- SHA-256: `d409b646a69d42b1145131b16fd6ef992025ab3bd739118c48b23e2a1ebed209`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class YearOfAshItemEntry
public string id = string.Empty;
public string name = string.Empty;
public string category = string.Empty;
public string description = string.Empty;
public float tradeValue = 0f;
public float weightKg = 0f;
public class YearOfAshEventEntry
public string id = string.Empty;
public string title = string.Empty;
public string description = string.Empty;
public int day = 180;
public string hazardType = string.Empty;
public string phase = string.Empty;
public float temperatureDeltaC = 0f;
public float exposureMultiplier = 1.0f;
public float pressureMultiplier = 1.0f;
public class YearOfAshLocationEntry
public string id = string.Empty;
public string displayName = string.Empty;
public string sector = string.Empty;
public int riskLevel = 1;
public float radiationUsv = 0f;
public string description = string.Empty;
public class YearOfAshRadioEntry
public string id = string.Empty;
public string frequency = string.Empty;
public int dayTrigger = 180;
public bool isEmergency = false;
public string message = string.Empty;
public string signalStrength = string.Empty; // "S7" etc. — the radio signal scale, not a number
public string source = string.Empty;
public string audio_cue = string.Empty;
public class YearOfAshSurvivorEntry
public string id = string.Empty;
public string name = string.Empty;
public string occupation = string.Empty;
public float rurScore = 10.0f;
public string moralAlignment = string.Empty;
public int age = 30;
public float healthPercent = 100f;
public float radiationDoseMsv = 0f;
public int guiltScore = 0;
public string backstory = string.Empty;
public string confession = string.Empty;
public string factionAffinity = string.Empty;
public List<string> traits = new List<string>();
public class YearOfAshItemContainer { public int schema_version; public List<YearOfAshItemEntry> items = new List<YearOfAshItemEntry>(); }
public class YearOfAshEventContainer { public int schema_version; public List<YearOfAshEventEntry> events = new List<YearOfAshEventEntry>(); }
public class YearOfAshLocationContainer { public int schema_version; public List<YearOfAshLocationEntry> locations = new List<YearOfAshLocationEntry>(); }
public class YearOfAshRadioContainer { public int schema_version; public List<YearOfAshRadioEntry> broadcasts = new List<YearOfAshRadioEntry>(); }
public class YearOfAshSurvivorContainer { public int schema_version; public List<YearOfAshSurvivorEntry> survivors = new List<YearOfAshSurvivorEntry>(); }
public class YearOfAshQuestContainer { public int schema_version; public List<QuestlineDefinition> quests = new List<QuestlineDefinition>(); }
public class RawQuestEntry
public string id = string.Empty;
public string questlineId = string.Empty;
public string title = string.Empty;
public string synopsis = string.Empty;
public string faction = string.Empty;
public string factionTag = string.Empty;
public int minDay = 180;
public int maxDay = 360;
public List<RawQuestStage> stages = new List<RawQuestStage>();
public class RawQuestStage
public string stageId = string.Empty;
public int stageIndex = 0;
public string objective = string.Empty;
public string title = string.Empty;
public string narrativePrompt = string.Empty;
public string requiredItemId = string.Empty;
public bool isCompleted = false;
public class RawQuestContainer
public List<RawQuestEntry> quests = new List<RawQuestEntry>();
public static class YearOfAshCatalogLoader
public const string ItemsFile = "year_of_ash_items.json";
public const string EventsFile = "year_of_ash_events.json";
public const string QuestsFile = "year_of_ash_quests.json";
public const string QuestlinesFile = "year_of_ash_questlines.json";
public const string LocationsFile = "year_of_ash_locations.json";
public const string RadioFile = "year_of_ash_radio.json";
public const string SurvivorsFile = "year_of_ash_survivors.json";
public static List<QuestlineDefinition> LoadQuestlines(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static List<YearOfAshItemEntry> LoadItems(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static List<YearOfAshEventEntry> LoadEvents(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static List<QuestlineDefinition> LoadQuests(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static List<YearOfAshLocationEntry> LoadLocations(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static List<YearOfAshRadioEntry> LoadRadioBroadcasts(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static List<YearOfAshSurvivorEntry> LoadSurvivors(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static int LoadAndRegisterQuests(QuestlineSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix Q.571 — Additional Current Architecture Evidence: `src/Main.UiTests.Dose.cs`

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


# Appendix Q.572 — Additional Current Architecture Evidence: `src/Main.Phase0.cs`

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


# Appendix Q.573 — Additional Current Architecture Evidence: `src/Audio/AudioEventBridge.cs`

### `src/Audio/AudioEventBridge.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 633 lines / 26686 bytes.
- SHA-256: `840935ff49fb41437d3afaf2892e049e4a0f298cc25ae16650b66df0c07139e7`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public interface IAudioDomainProvider
public sealed class AudioEventBridge : IDisposable
public void SubscribeAll( RadiationSystem? radiation = null, WeatherSystem? weather = null, TacticalCombatSystem? combat = null, CraftingSystem? crafting = null, ExpeditionSystem? expeditions = null,
public void BindRadiation(RadiationSystem? radiation) {
public void BindWeather(WeatherSystem? weather) {
public void BindCombat(TacticalCombatSystem? combat) {
public void BindCrafting(CraftingSystem? crafting) {
public void BindExpeditions(ExpeditionSystem? expeditions) {
public void BindDisease(DiseaseSystem? disease) {
public void BindSurvivorFate(SurvivorFateSystem? survivorFate) {
public void BindFlashbacks(SomaticFlashbackSystem? flashbacks) {
public void BindEchoes(EchoSystem? echoes) {
public void NotifyGameFlow(string cueId) {
public void Dispose() {
internal bool HasRadiationBinding => _radiation != null;
internal bool HasWeatherBinding => _weather != null;
internal bool HasCombatBinding => _combat != null;
internal bool HasCraftingBinding => _crafting != null;
internal bool HasExpeditionsBinding => _expeditions != null;
internal bool HasDiseaseBinding => _disease != null;
internal bool HasSurvivorFateBinding => _survivorFate != null;
internal bool HasFlashbacksBinding => _flashbacks != null;
internal bool HasEchoesBinding => _echoes != null;
```


# Appendix Q.574 — Additional Current Architecture Evidence: `src/Host/HealthHistoryHostSession.cs`

### `src/Host/HealthHistoryHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 125 lines / 4445 bytes.
- SHA-256: `b006cc0a4d9583998b6b83f2416125cd9ce171e75a4dc5e3daf5a491f5dc518a`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class HealthHistorySaveStore
public const string SectionName = "health_history";
public const string FileName = "health_history_save.json";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string? TryCapturePersisted(HealthHistoryState state) => s_store.CaptureBare(state);
public static HealthHistoryState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
public static bool TrySave(HealthHistoryState state) => s_store.TrySave(state);
public static HealthHistoryState? TryLoad() => s_store.TryLoad();
public sealed class HealthHistoryHostSession : HostSessionBase
public HealthHistorySystem System { get; }
public static HealthHistoryHostSession Create(string dataDir, HealthHistoryState? restoredState = null) {
public MedicalRecord LogRecord( string survivorId, string recordType, string description, string severity, int day,
public bool ResolveRecord(string recordId, string outcome, int day, string notes = "") {
public VaccinationRecord AdministerVaccine( string survivorId, string vaccineType, int day, string medicId = "", float initialImmunity = 100.0f,
public List<HealthTrend> RecordDailyHealthTrend( string survivorId, int day, float overallHealth, float radiationDose, float immuneStrength,
public void TickDay(int day) {
public HealthHistoryCensus GetCensus() => System.GetCensus();
public override void Save() {
```


# Appendix Q.575 — Additional Current Architecture Evidence: `src/Host/PanelBindLifecycleSelfTest.cs`

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


# Appendix Q.576 — Additional Current Architecture Evidence: `src/Main.UiTests.RealCampaignJourney.cs`

### `src/Main.UiTests.RealCampaignJourney.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 648 lines / 43276 bytes.
- SHA-256: `54dba7778958e0fd3ef887d36d13af4d7d72c42fd51bde7700cbb967f9490fc1`.
- Architecture signals: seeded references=2; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=1; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.577 — Additional Current Architecture Evidence: `src/Host/ContentUtilizationRuntimeCollector.cs`

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


# Appendix Q.578 — Additional Current Architecture Evidence: `src/Host/HostCli.PanelTests.cs`

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


# Appendix Q.579 — Additional Current Architecture Evidence: `src/Host/HostCli.SliceScenario.cs`

### `src/Host/HostCli.SliceScenario.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 215 lines / 11322 bytes.
- SHA-256: `31332c42ae07a26e58b5bb54ab357e82cd7ec404e2b01abe57bab6c97a66dc2d`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunSliceScenarioSelfTest(string dataDirectory) {
```


# Appendix Q.580 — Additional Current Architecture Evidence: `src/Host/RetentionHostSession.cs`

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


# Appendix Q.581 — Additional Current Architecture Evidence: `src/Main.PlayerSurfaces.cs`

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


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.


# Appendix S — Quality Assurance Pass Record: Plan 81

This record is part of the planning artifact, not a fresh runtime test result.

## Pass 1 — content and premise accuracy
- Content pass replaced the stale 3→12 target with the current 14-location census.
- The historical baseline is separated from the current source/data/test authority.
- Current row counts and owner boundaries are stated without using count as a quality proxy.

## Pass 2 — integration architecture
- Integration pass separated catalog baseline, ledger attribution, weather and exposure.
- Core, data, host, UI, save, event and test seams are named with current paths.
- The plan does not authorize a parallel save section, catalog, manager or host cache.

## Final precision and reaccuracy pass
- Precision pass keeps the dose ledger and geography panel as distinct owners.
- Every embedded current-file hash, focused runner command and master-authority reference is rechecked.
- Any proposed future seam is labeled as requiring a separate claim and premise verification.
