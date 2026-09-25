# Plan 59 — Dynamic Questlines, Year of Ashes and Multi-Stage State

> **Rebuild status:** PARTIAL 2 CURRENT QUESTLINES — CORE/LOADER/UI EXIST; CONTENT GAP REMAINS
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

- The current JSON is schema version 1 with two questlines. QuestlineSystem owns active records, stages, choices and status; DynamicQuestlineCatalogLoader parses the current file; QuestlineMasterCatalog is a separate larger authority and must not be duplicated.

**Bounded outcome:** The old 4-to-15 target is stale in the opposite direction: dynamic_questlines.json currently has 2 questlines, while QuestlineSystem, DynamicQuestlineCatalogLoader, questline master data, host sessions and a dynamic questline panel exist. The valuable plan is a bounded content/consumer audit, with a future decision about whether authored rows or the master quest catalog should be expanded.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- Replace the stale count objective with a two-row current census and an explicit content/ownership decision. Audit stage references, objective items, target locations, flags, rewards and host presentation.

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
- The current evidence supports a truthful two-row plan, not a fabricated fifteen-row claim.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- No new questline system, save section or master catalog. Any new row must resolve through the existing loader and current questline state machine.

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
| active questline records, stages, choices and persistence DTO | QuestlineSystem | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` | Sole dynamic questline runtime owner. |
| dynamic questline data loading | DynamicQuestlineCatalogLoader | `Assets/Ashfall.Core/YearOfAsh/DynamicQuestlineCatalogLoader.cs` | Static dynamic catalog owner. |
| separate authored questline master authority | QuestlineMasterCatalog | `Assets/Ashfall.Core/QuestlineMasterCatalog.cs` | Do not duplicate or merge by row count. |
| host state and commands | DynamicQuestHostSession | `src/Host/DynamicQuestHostSession.cs` | Thin host adapter. |
| current presentation | DynamicQuestlinePanel | `src/UI/DynamicQuestlinePanel.cs` | Read-only projection/commands. |
| current quest data parity evidence | YearOfAshQuestJsonParityTests | `Ashfall.Core.Tests/Narrative/YearOfAshQuestJsonParityTests.cs` | Focused evidence. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Dynamic Questlines, Year of Ashes and Multi-Stage State
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ QuestlineSystem
│   active questline records, stages, choices and persistence DTO
│ DynamicQuestlineCatalogLoader
│   dynamic questline data loading
│ QuestlineMasterCatalog
│   separate authored questline master authority
│ DynamicQuestHostSession
│   host state and commands
│ DynamicQuestlinePanel
│   current presentation
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

1. **Preserve current state ownership.** QuestlineSystem owns active questline records, stages, choices and persistence DTO: Sole dynamic questline runtime owner.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| active questline records, stages, choices and persistence DTO | QuestlineSystem | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` | Sole dynamic questline runtime owner. |
| dynamic questline data loading | DynamicQuestlineCatalogLoader | `Assets/Ashfall.Core/YearOfAsh/DynamicQuestlineCatalogLoader.cs` | Static dynamic catalog owner. |
| separate authored questline master authority | QuestlineMasterCatalog | `Assets/Ashfall.Core/QuestlineMasterCatalog.cs` | Do not duplicate or merge by row count. |
| host state and commands | DynamicQuestHostSession | `src/Host/DynamicQuestHostSession.cs` | Thin host adapter. |
| current presentation | DynamicQuestlinePanel | `src/UI/DynamicQuestlinePanel.cs` | Read-only projection/commands. |
| current quest data parity evidence | YearOfAshQuestJsonParityTests | `Ashfall.Core.Tests/Narrative/YearOfAshQuestJsonParityTests.cs` | Focused evidence. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load two dynamic definitions
2. validate IDs, stages, items, locations and flags
3. read current questline state
4. show playable/withheld definitions
5. route start/choice through QuestlineSystem
6. apply current reward/consequence owner
7. capture/restore current questline state

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Current two authored definitions are immutable.
- QuestlineSystemState owns active/resolved records and stage progress.
- Master questline state is separate and must be reconciled by IDs, not copied.
- Rewards and consequences route to current owners.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- A questline cannot start without a current valid definition.
- Stage order and choice IDs are exact.
- Unknown item/location/flag references fail closed.
- A resolved questline cannot re-resolve or replay rewards.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- dynamic_questlines.json is the dynamic authority.
- questline_master.json, items, locations and flags remain independent.
- No duplicate questline master file.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use current questline/Year of Ash save owner.
- No Plan-59 save section.
- Old empty and partially complete states restore with current defaults.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Quest selection/branch outcomes use existing seeded RNG only where current system requires it.
- Definition and stage order are stable.
- Same state, day and seed produce same availability/resolution.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- QuestlineSystem emits start, choice and resolution facts.
- Host and panel consume current state.
- Narrative flags/journal receive explicit facts, not hidden writes from UI.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/DynamicQuestHostSession.cs
- src/UI/DynamicQuestlinePanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Quest prose is grounded and fictional.
- Objectives must describe actual reachable items/locations, not aspirational design-only tokens.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A new row duplicates a master questline. | QuestlineSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A missing item/location passes validation. | DynamicQuestlineCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A panel advances a stage directly. | QuestlineMasterCatalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | Rewards are granted twice after reload. | DynamicQuestHostSession | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A new quest state store is introduced. | DynamicQuestlinePanel | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/YearOfAshQuestJsonParityTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 | Two-row and master-catalog census. | The actual content gap is visible. | No production path until the owning implementation package is separately claimed. |
| 1 | Reference/stage/reward audit. | No design-only quest is called live. | No production path until the owning implementation package is separately claimed. |
| 2 | Host/state/replay trace. | One questline owner and save path. | No production path until the owning implementation package is separately claimed. |
| 3 | Content/UI polish. | New rows require a separately approved content delta. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/dynamic_questlines.json | READ ONLY; MODIFY only for approved content delta | 2 current questlines |
| Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs | READ ONLY | Runtime owner |
| src/Host/DynamicQuestHostSession.cs | READ ONLY | Host seam |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Duplicating master quests. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Adding unowned quest state. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Presenting design-only rows as reachable. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Duplicate rewards. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new questline rows in this package.
- No new quest runtime.
- No production/data/test/UI changes here.

# 23. Rollback and Recovery

- Revert planning artifact.
- Future content additions are isolated and validated by the existing loader.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- Two current rows and all separate master-catalog boundaries are explicit.
- A future content expansion has a safe route and acceptance gate.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- No new questline system, save section or master catalog. Any new row must resolve through the existing loader and current questline state machine.

## MUST NOT DO

- No new questline rows in this package.
- No new quest runtime.
- No production/data/test/UI changes here.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/YearOfAshQuestJsonParityTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: active questline records, stages, choices and persistence DTO → QuestlineSystem; dynamic questline data loading → DynamicQuestlineCatalogLoader; separate authored questline master authority → QuestlineMasterCatalog; host state and commands → DynamicQuestHostSession; current presentation → DynamicQuestlinePanel; current quest data parity evidence → YearOfAshQuestJsonParityTests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 59.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 59 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by QuestlineSystem or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs`

### `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 432 lines / 19009 bytes.
- SHA-256: `efb9ca627d7070493ffaf6cbbae608c3835f66b3fd489e0fe52f6e101ea5bf90`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=6; textual Godot mentions=1; textual Unity/JsonUtility mentions=1; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum QuestlineStatus
public class QuestCondition
public string conditionTag = string.Empty;
public bool isBlocker = false;  // false = soft warning, true = hard gate
public class QuestChoice
public string choiceId   = string.Empty;
public string text       = string.Empty;
public string nextStageId = string.Empty;   // empty = quest ends
public int moraleDelta   = 0;
public int guiltDelta    = 0;
public string grantItemId = string.Empty;    // empty = no item
public int grantItemQuantity = 0;
public string targetFactionId   = string.Empty;
public int factionStandingDelta = 0;
public string unlockEncounterId = string.Empty;
public List<QuestCondition> conditions = new List<QuestCondition>();
public string outcomeNarrative = string.Empty;
public class QuestStage
public string stageId          = string.Empty;
public string title            = string.Empty;
public string narrativePrompt  = string.Empty;  // What the player sees
public int    unlockOnDay      = 0;             // Day >= this to surface
public bool   isTerminal       = false;         // true = no further choices
public QuestlineStatus terminalOutcome = QuestlineStatus.Completed;
public List<QuestChoice> choices = new List<QuestChoice>();
public class QuestlineDefinition
public string questlineId  = string.Empty;
public string title        = string.Empty;
public string synopsis     = string.Empty;
public string factionTag   = string.Empty;     // primary faction context
public string firstStageId = string.Empty;
public int    minDay       = 180;
public int    maxDay       = 360;
public List<QuestStage> stages = new List<QuestStage>();
public QuestStage? FindStage(string id) {
public class ActiveQuestlineRecord
public string questlineId     = string.Empty;
public string currentStageId  = string.Empty;
public QuestlineStatus status = QuestlineStatus.Active;
public List<string> choiceHistory = new List<string>();   // ordered choice IDs taken
public int dayStarted = 0;
public int dayResolved = -1;
public class QuestlineSystemState
public List<ActiveQuestlineRecord> active = new List<ActiveQuestlineRecord>();
public List<string> completedQuestlineIds = new List<string>();
public List<string> failedQuestlineIds    = new List<string>();
public int totalMoraleDeltaFromQuests     = 0;
public int totalGuiltDeltaFromQuests      = 0;
public class QuestChoiceResult
public string questlineId    = string.Empty;
public string stageId        = string.Empty;
public string choiceId       = string.Empty;
public string nextStageId    = string.Empty;
public int    moraleDelta    = 0;
public int    guiltDelta     = 0;
public string grantItemId    = string.Empty;
public int    grantItemQty   = 0;
public string factionId      = string.Empty;
public int    factionDelta   = 0;
public string unlockedEncounterId = string.Empty;
public string outcomeNarrative    = string.Empty;
public QuestlineStatus newQuestStatus = QuestlineStatus.Active;
public class QuestlineSystem
public QuestlineSystemState State => _state;
public IReadOnlyList<QuestlineDefinition> Catalog => _catalog;
public event Action<QuestlineDefinition>  OnQuestlineStarted;
public event Action<QuestChoiceResult>    OnQuestChoiceTaken;
public event Action<string, QuestlineStatus> OnQuestlineResolved;
public void RegisterQuestline(QuestlineDefinition def) {
public QuestlineDefinition? FindDefinition(string questlineId) {
public List<QuestlineDefinition> GetAvailableQuestlines(int currentDay) {
public bool IsPlayable(QuestlineDefinition def) {
public List<QuestlineDefinition> GetPlayableQuestlines(int currentDay) {
public int WithheldQuestlineCount(int currentDay) {
public bool StartQuestline(string questlineId, int day) {
public QuestChoiceResult? TakeChoice(string questlineId, string choiceId, int day) {
public ActiveQuestlineRecord? GetActiveRecord(string questlineId) {
public QuestlineSystemState CaptureState() {
public void RestoreState(QuestlineSystemState state) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/YearOfAsh/DynamicQuestlineCatalogLoader.cs`

### `Assets/Ashfall.Core/YearOfAsh/DynamicQuestlineCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 62 lines / 2286 bytes.
- SHA-256: `2a7054663163429068e4765faf91dba6001ba3b1ee0acf9e68eb024b40cc4cbe`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class DynamicQuestlineCatalogLoader
public const string FileName = "dynamic_questlines.json";
public static int LoadAndRegister( QuestlineSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/QuestlineMasterCatalog.cs`

### `Assets/Ashfall.Core/QuestlineMasterCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 155 lines / 5930 bytes.
- SHA-256: `fe54bcda711a61a040345e87487202fee52853fcd2d794806dee370b9114a18a`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class QuestlineMasterCatalog
public int Count => _ids.Count;
public IReadOnlyList<string> All => _ordered;
public bool IsRegistered(string questId) {
public bool HasContent(string questId, params IReadOnlyList<string>[] catalogQuestIds) {
public List<string> FindUnregistered(IEnumerable<string> catalogQuestIds) {
internal void Add(string id) {
public sealed class QuestlineMasterEntry
public string id = string.Empty;
public sealed class QuestlineMasterRoot
public int schema_version;
public List<QuestlineMasterEntry> entries = new List<QuestlineMasterEntry>();
public sealed class QuestlineMasterCatalogLoader
public const string FileName = "questline_master.json";
public QuestlineMasterCatalog Load(string dataDirectory) {
```


# Appendix B.05 — Current Code Architecture: `src/Host/DynamicQuestHostSession.cs`

### `src/Host/DynamicQuestHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 94 lines / 3962 bytes.
- SHA-256: `e24b34b7ca00062195d137b359d0e9821d755fe63c2287c11b7bac311c02b0fd`.
- Architecture signals: seeded references=1; save/restore symbols=4; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DynamicQuestHostSession : HostSessionBase
public DynamicQuestGenerator Generator => _generator;
public DynamicQuestGeneratorCensus Census => _generator.GetCensus();
public string LastEvent => _lastEvent;
public IReadOnlyList<ProceduralQuestTemplate> Templates { get; private set; } = Array.Empty<ProceduralQuestTemplate>();
public static DynamicQuestHostSession Create(string dataDir) {
public void LoadCatalog(string dataDir) {
public IReadOnlyList<ProceduralQuest> GenerateCandidates(int day, ISeededRng? rng = null) =>
public bool AcceptCandidate(string questId, string survivorId = "") =>
public bool ProgressCandidate(string questId, int amount = 1) =>
public bool CompleteCandidate(string questId, int day) =>
public void CheckDeadlines(int day) => _generator.CheckDeadlines(day);
public DynamicQuestGeneratorState CaptureState() => _generator.CaptureState();
public void RestoreState(DynamicQuestGeneratorState state) => _generator.RestoreState(state);
```


# Appendix B.06 — Current Code Architecture: `src/Host/DynamicQuestHostSession.cs`

### `src/Host/DynamicQuestHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 94 lines / 3962 bytes.
- SHA-256: `e24b34b7ca00062195d137b359d0e9821d755fe63c2287c11b7bac311c02b0fd`.
- Architecture signals: seeded references=1; save/restore symbols=4; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DynamicQuestHostSession : HostSessionBase
public DynamicQuestGenerator Generator => _generator;
public DynamicQuestGeneratorCensus Census => _generator.GetCensus();
public string LastEvent => _lastEvent;
public IReadOnlyList<ProceduralQuestTemplate> Templates { get; private set; } = Array.Empty<ProceduralQuestTemplate>();
public static DynamicQuestHostSession Create(string dataDir) {
public void LoadCatalog(string dataDir) {
public IReadOnlyList<ProceduralQuest> GenerateCandidates(int day, ISeededRng? rng = null) =>
public bool AcceptCandidate(string questId, string survivorId = "") =>
public bool ProgressCandidate(string questId, int amount = 1) =>
public bool CompleteCandidate(string questId, int day) =>
public void CheckDeadlines(int day) => _generator.CheckDeadlines(day);
public DynamicQuestGeneratorState CaptureState() => _generator.CaptureState();
public void RestoreState(DynamicQuestGeneratorState state) => _generator.RestoreState(state);
```


# Appendix B.07 — Current Code Architecture: `src/UI/DynamicQuestlinePanel.cs`

### `src/UI/DynamicQuestlinePanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 199 lines / 8548 bytes.
- SHA-256: `2599df8f165471a02239bc26cd4dca5db22e4ed08600172e940164bdef154f97`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class DynamicQuestlinePanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _system != null;
public void Bind(DynamicQuestlineSystem system) {
public void Unbind() {
public override void _Ready() {
public override void _ExitTree() {
public void RefreshView() {
```


# Appendix C.08 — Catalog Census: `Assets/StreamingAssets/Data/dynamic_questlines.json`

### `Assets/StreamingAssets/Data/dynamic_questlines.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 4153 bytes / 4147 characters.
- SHA-256: `d02872c38df6a2c8e7bba7f093f5850ec92adff03645e33be5b4cab6dac80d26`.
- Root keys: `questlines`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
questlines: min=2, max=2, observed_paths=1
questlines[].stages: min=4, max=4, observed_paths=2
questlines[].stages[].objective_items: min=1, max=2, observed_paths=4
```

Representative record fields:

- `quest_id`
- `stages`
- `target_location_id`
- `title`

Representative identifiers (ordered, capped for readability):

```text
quest_dying_signal
quest_aquifer_contamination
```


# Appendix C.09 — Catalog Census: `Assets/StreamingAssets/Data/questline_master.json`

### `Assets/StreamingAssets/Data/questline_master.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 66388 bytes / 66372 characters.
- SHA-256: `f7ef6d26b20fd2f87137cc1836cfd72e88c6baebae6bccee80affad841c2efc8`.
- Root keys: `entries`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
entries: min=511, max=511, observed_paths=1
```

Representative record fields:

- `factionTag`
- `id`
- `narrativeHook`
- `status`
- `synopsis`
- `title`

Representative identifiers (ordered, capped for readability):

```text
quest_a_good_death
quest_a_real_leader
quest_a_voice_in_the_dark
quest_acoustic_bait
quest_ashen_communion
quest_assassination_deserter
quest_black_vein
quest_breaking_chains
quest_broken_chronometer
quest_burnt_harvest
quest_checkpoint_kilo_truth
quest_child_soldier_rifle
quest_cold_turkey
quest_court_martial
quest_crisis_of_faith
quest_crossing_companion_mattis
quest_crossing_first_weigh
quest_crossing_scale_integrity
quest_crossing_the_forfeit
quest_crossing_the_marker
quest_crossing_the_petition
quest_crossing_the_standing
quest_crossing_the_terms
quest_crossing_the_vote_that_isnt
quest_crossing_the_vouch
quest_crossing_three_dry_pages
quest_crossing_who_holds_the_ledger
quest_cult_glow_communion
quest_cult_purity
quest_dead_air
quest_deep_well
quest_dropping_the_rifle
quest_earning_keep
quest_elena_triage
quest_embracing_the_glow
quest_first_blood
quest_garrison_census
quest_garrison_last_order
quest_ghosts_of_day_1
quest_glow_communion
quest_grain_war
quest_ground_zero
quest_growing_up_fast
quest_hell_is_other_people
quest_holdfast_authentication
quest_holdfast_the_clerk
quest_holdfast_the_clerk_started
quest_holdfast_the_drawer
quest_holdfast_the_hatch
quest_holdfast_the_levy
quest_holdfast_the_membrane
quest_holdfast_the_plant
quest_holdfast_the_second_list
quest_holdfast_the_sheet
quest_holdfast_the_window
quest_holding_the_line
quest_in_the_black
quest_inventory_audit
quest_iron_ledger
quest_loss_of_faith
quest_mans_best_friend
quest_mechanic_highway_heart
quest_militia_grain_war
quest_militia_smugglers_route
quest_museum_archive
quest_player_one
quest_putting_down_roots
quest_rebuilders_thirst
quest_record_fallback
quest_record_friendly_obstacle
quest_record_grease_pencil
quest_record_hands
quest_record_mass_or_lot
quest_record_the_book
quest_record_the_failure
quest_record_the_plate
quest_record_which_gazetteer
quest_record_wrong_stacks
quest_redemption_arc
quest_registry
quest_rep_meal_row
quest_rep_night_slate
quest_roster_12b_kit
quest_roster_ansel_truth
quest_roster_blank_access
quest_roster_boot_crate
quest_roster_brigid
quest_roster_caretaker
quest_roster_chair
quest_roster_fourteenth
quest_roster_frayne_minutes
quest_roster_grange_vote
quest_roster_hadi_shift
quest_roster_ink
quest_roster_ivy_oil
quest_roster_kess_pencil
quest_roster_len_tag
quest_roster_missing_strip
quest_roster_nila_eleven
quest_roster_pell_numbers
quest_roster_quiet
quest_roster_sole
quest_roster_tamsin_watch
quest_roster_the_chart
quest_roster_the_column
quest_roster_the_tin
quest_roster_who_eats
quest_roster_window
quest_separation_anxiety
quest_shattered_glass
quest_st_maren_last_shift
quest_target
quest_tears_in_rain
quest_the_abandoned_school
quest_the_anesthetic
quest_the_archive
quest_the_awakening
quest_the_backdraft
quest_the_bank_heist
quest_the_big_score
quest_the_blueprints
quest_the_botched_job
quest_the_boy_who_cried_wolf
quest_the_broken_mind
quest_the_broken_promise
quest_the_bunker_breached
quest_the_canary
quest_the_city_mains
quest_the_cleansing
quest_the_clearcut
quest_the_dead_stars
quest_the_empty_bottles
quest_the_empty_crib
quest_the_escape
quest_the_final_broadcast
quest_the_final_harvest
quest_the_final_payload
quest_the_first_save
quest_the_gladiator
quest_the_golden_parachute
quest_the_hard_reboot
quest_the_holdout
quest_the_independent
quest_the_inferno
quest_the_iron_gate
quest_the_iron_worm
quest_the_kevlar_loom
quest_the_lab_ruin
quest_the_last_contract
quest_the_last_ride
quest_the_last_seed
quest_the_last_stash
quest_the_locket
quest_the_long_haul
quest_the_long_night
quest_the_lost_route
quest_the_marathon
quest_the_mask_slips
quest_the_mass_grave
quest_the_masterpiece
```


# Appendix D.10 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Narrative/YearOfAshQuestJsonParityTests.cs`

### `Ashfall.Core.Tests/Narrative/YearOfAshQuestJsonParityTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 164; SHA-256: `e3d50b8f1763b2722dfca58e89c22efd814191f07ebb200ecd382752c45bdf3b`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CanonicalCatalog_ExistingEightQuestlines_MatchBuiltInBaseline
CanonicalCatalog_ContainsExactlySevenPlan114Questlines
PilotQuestline_GarrisonBloodDebt_LoadsAndPlaysThroughChoices
```


# Appendix E.11 — Supporting Code Evidence: `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs`

### `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 432 lines / 19009 bytes.
- SHA-256: `efb9ca627d7070493ffaf6cbbae608c3835f66b3fd489e0fe52f6e101ea5bf90`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=6; textual Godot mentions=1; textual Unity/JsonUtility mentions=1; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum QuestlineStatus
public class QuestCondition
public string conditionTag = string.Empty;
public bool isBlocker = false;  // false = soft warning, true = hard gate
public class QuestChoice
public string choiceId   = string.Empty;
public string text       = string.Empty;
public string nextStageId = string.Empty;   // empty = quest ends
public int moraleDelta   = 0;
public int guiltDelta    = 0;
public string grantItemId = string.Empty;    // empty = no item
public int grantItemQuantity = 0;
public string targetFactionId   = string.Empty;
public int factionStandingDelta = 0;
public string unlockEncounterId = string.Empty;
public List<QuestCondition> conditions = new List<QuestCondition>();
public string outcomeNarrative = string.Empty;
public class QuestStage
public string stageId          = string.Empty;
public string title            = string.Empty;
public string narrativePrompt  = string.Empty;  // What the player sees
public int    unlockOnDay      = 0;             // Day >= this to surface
public bool   isTerminal       = false;         // true = no further choices
public QuestlineStatus terminalOutcome = QuestlineStatus.Completed;
public List<QuestChoice> choices = new List<QuestChoice>();
public class QuestlineDefinition
public string questlineId  = string.Empty;
public string title        = string.Empty;
public string synopsis     = string.Empty;
public string factionTag   = string.Empty;     // primary faction context
public string firstStageId = string.Empty;
public int    minDay       = 180;
public int    maxDay       = 360;
public List<QuestStage> stages = new List<QuestStage>();
public QuestStage? FindStage(string id) {
public class ActiveQuestlineRecord
public string questlineId     = string.Empty;
public string currentStageId  = string.Empty;
public QuestlineStatus status = QuestlineStatus.Active;
public List<string> choiceHistory = new List<string>();   // ordered choice IDs taken
public int dayStarted = 0;
public int dayResolved = -1;
public class QuestlineSystemState
public List<ActiveQuestlineRecord> active = new List<ActiveQuestlineRecord>();
public List<string> completedQuestlineIds = new List<string>();
public List<string> failedQuestlineIds    = new List<string>();
public int totalMoraleDeltaFromQuests     = 0;
public int totalGuiltDeltaFromQuests      = 0;
public class QuestChoiceResult
public string questlineId    = string.Empty;
public string stageId        = string.Empty;
public string choiceId       = string.Empty;
public string nextStageId    = string.Empty;
public int    moraleDelta    = 0;
public int    guiltDelta     = 0;
public string grantItemId    = string.Empty;
public int    grantItemQty   = 0;
public string factionId      = string.Empty;
public int    factionDelta   = 0;
public string unlockedEncounterId = string.Empty;
public string outcomeNarrative    = string.Empty;
public QuestlineStatus newQuestStatus = QuestlineStatus.Active;
public class QuestlineSystem
public QuestlineSystemState State => _state;
public IReadOnlyList<QuestlineDefinition> Catalog => _catalog;
public event Action<QuestlineDefinition>  OnQuestlineStarted;
public event Action<QuestChoiceResult>    OnQuestChoiceTaken;
public event Action<string, QuestlineStatus> OnQuestlineResolved;
public void RegisterQuestline(QuestlineDefinition def) {
public QuestlineDefinition? FindDefinition(string questlineId) {
public List<QuestlineDefinition> GetAvailableQuestlines(int currentDay) {
public bool IsPlayable(QuestlineDefinition def) {
public List<QuestlineDefinition> GetPlayableQuestlines(int currentDay) {
public int WithheldQuestlineCount(int currentDay) {
public bool StartQuestline(string questlineId, int day) {
public QuestChoiceResult? TakeChoice(string questlineId, string choiceId, int day) {
public ActiveQuestlineRecord? GetActiveRecord(string questlineId) {
public QuestlineSystemState CaptureState() {
public void RestoreState(QuestlineSystemState state) {
```


# Appendix E.12 — Supporting Code Evidence: `Assets/Ashfall.Core/YearOfAsh/DynamicQuestlineCatalogLoader.cs`

### `Assets/Ashfall.Core/YearOfAsh/DynamicQuestlineCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 62 lines / 2286 bytes.
- SHA-256: `2a7054663163429068e4765faf91dba6001ba3b1ee0acf9e68eb024b40cc4cbe`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class DynamicQuestlineCatalogLoader
public const string FileName = "dynamic_questlines.json";
public static int LoadAndRegister( QuestlineSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix E.13 — Supporting Code Evidence: `Assets/Ashfall.Core/QuestlineMasterCatalog.cs`

### `Assets/Ashfall.Core/QuestlineMasterCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 155 lines / 5930 bytes.
- SHA-256: `fe54bcda711a61a040345e87487202fee52853fcd2d794806dee370b9114a18a`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class QuestlineMasterCatalog
public int Count => _ids.Count;
public IReadOnlyList<string> All => _ordered;
public bool IsRegistered(string questId) {
public bool HasContent(string questId, params IReadOnlyList<string>[] catalogQuestIds) {
public List<string> FindUnregistered(IEnumerable<string> catalogQuestIds) {
internal void Add(string id) {
public sealed class QuestlineMasterEntry
public string id = string.Empty;
public sealed class QuestlineMasterRoot
public int schema_version;
public List<QuestlineMasterEntry> entries = new List<QuestlineMasterEntry>();
public sealed class QuestlineMasterCatalogLoader
public const string FileName = "questline_master.json";
public QuestlineMasterCatalog Load(string dataDirectory) {
```


# Appendix E.14 — Supporting Code Evidence: `src/Host/DynamicQuestHostSession.cs`

### `src/Host/DynamicQuestHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 94 lines / 3962 bytes.
- SHA-256: `e24b34b7ca00062195d137b359d0e9821d755fe63c2287c11b7bac311c02b0fd`.
- Architecture signals: seeded references=1; save/restore symbols=4; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DynamicQuestHostSession : HostSessionBase
public DynamicQuestGenerator Generator => _generator;
public DynamicQuestGeneratorCensus Census => _generator.GetCensus();
public string LastEvent => _lastEvent;
public IReadOnlyList<ProceduralQuestTemplate> Templates { get; private set; } = Array.Empty<ProceduralQuestTemplate>();
public static DynamicQuestHostSession Create(string dataDir) {
public void LoadCatalog(string dataDir) {
public IReadOnlyList<ProceduralQuest> GenerateCandidates(int day, ISeededRng? rng = null) =>
public bool AcceptCandidate(string questId, string survivorId = "") =>
public bool ProgressCandidate(string questId, int amount = 1) =>
public bool CompleteCandidate(string questId, int day) =>
public void CheckDeadlines(int day) => _generator.CheckDeadlines(day);
public DynamicQuestGeneratorState CaptureState() => _generator.CaptureState();
public void RestoreState(DynamicQuestGeneratorState state) => _generator.RestoreState(state);
```


# Appendix E.15 — Supporting Code Evidence: `src/Host/DynamicQuestHostSession.cs`

### `src/Host/DynamicQuestHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 94 lines / 3962 bytes.
- SHA-256: `e24b34b7ca00062195d137b359d0e9821d755fe63c2287c11b7bac311c02b0fd`.
- Architecture signals: seeded references=1; save/restore symbols=4; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DynamicQuestHostSession : HostSessionBase
public DynamicQuestGenerator Generator => _generator;
public DynamicQuestGeneratorCensus Census => _generator.GetCensus();
public string LastEvent => _lastEvent;
public IReadOnlyList<ProceduralQuestTemplate> Templates { get; private set; } = Array.Empty<ProceduralQuestTemplate>();
public static DynamicQuestHostSession Create(string dataDir) {
public void LoadCatalog(string dataDir) {
public IReadOnlyList<ProceduralQuest> GenerateCandidates(int day, ISeededRng? rng = null) =>
public bool AcceptCandidate(string questId, string survivorId = "") =>
public bool ProgressCandidate(string questId, int amount = 1) =>
public bool CompleteCandidate(string questId, int day) =>
public void CheckDeadlines(int day) => _generator.CheckDeadlines(day);
public DynamicQuestGeneratorState CaptureState() => _generator.CaptureState();
public void RestoreState(DynamicQuestGeneratorState state) => _generator.RestoreState(state);
```


# Appendix F.16 — Supporting Data Evidence: `Assets/StreamingAssets/Data/dynamic_questlines.json`

### `Assets/StreamingAssets/Data/dynamic_questlines.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 4153 bytes / 4147 characters.
- SHA-256: `d02872c38df6a2c8e7bba7f093f5850ec92adff03645e33be5b4cab6dac80d26`.
- Root keys: `questlines`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
questlines: min=2, max=2, observed_paths=1
questlines[].stages: min=4, max=4, observed_paths=2
questlines[].stages[].objective_items: min=1, max=2, observed_paths=4
```

Representative record fields:

- `quest_id`
- `stages`
- `target_location_id`
- `title`

Representative identifiers (ordered, capped for readability):

```text
quest_dying_signal
quest_aquifer_contamination
```


# Appendix F.17 — Supporting Data Evidence: `Assets/StreamingAssets/Data/questline_master.json`

### `Assets/StreamingAssets/Data/questline_master.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 66388 bytes / 66372 characters.
- SHA-256: `f7ef6d26b20fd2f87137cc1836cfd72e88c6baebae6bccee80affad841c2efc8`.
- Root keys: `entries`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
entries: min=511, max=511, observed_paths=1
```

Representative record fields:

- `factionTag`
- `id`
- `narrativeHook`
- `status`
- `synopsis`
- `title`

Representative identifiers (ordered, capped for readability):

```text
quest_a_good_death
quest_a_real_leader
quest_a_voice_in_the_dark
quest_acoustic_bait
quest_ashen_communion
quest_assassination_deserter
quest_black_vein
quest_breaking_chains
quest_broken_chronometer
quest_burnt_harvest
quest_checkpoint_kilo_truth
quest_child_soldier_rifle
quest_cold_turkey
quest_court_martial
quest_crisis_of_faith
quest_crossing_companion_mattis
quest_crossing_first_weigh
quest_crossing_scale_integrity
quest_crossing_the_forfeit
quest_crossing_the_marker
quest_crossing_the_petition
quest_crossing_the_standing
quest_crossing_the_terms
quest_crossing_the_vote_that_isnt
quest_crossing_the_vouch
quest_crossing_three_dry_pages
quest_crossing_who_holds_the_ledger
quest_cult_glow_communion
quest_cult_purity
quest_dead_air
quest_deep_well
quest_dropping_the_rifle
quest_earning_keep
quest_elena_triage
quest_embracing_the_glow
quest_first_blood
quest_garrison_census
quest_garrison_last_order
quest_ghosts_of_day_1
quest_glow_communion
quest_grain_war
quest_ground_zero
quest_growing_up_fast
quest_hell_is_other_people
quest_holdfast_authentication
quest_holdfast_the_clerk
quest_holdfast_the_clerk_started
quest_holdfast_the_drawer
quest_holdfast_the_hatch
quest_holdfast_the_levy
quest_holdfast_the_membrane
quest_holdfast_the_plant
quest_holdfast_the_second_list
quest_holdfast_the_sheet
quest_holdfast_the_window
quest_holding_the_line
quest_in_the_black
quest_inventory_audit
quest_iron_ledger
quest_loss_of_faith
quest_mans_best_friend
quest_mechanic_highway_heart
quest_militia_grain_war
quest_militia_smugglers_route
quest_museum_archive
quest_player_one
quest_putting_down_roots
quest_rebuilders_thirst
quest_record_fallback
quest_record_friendly_obstacle
quest_record_grease_pencil
quest_record_hands
quest_record_mass_or_lot
quest_record_the_book
quest_record_the_failure
quest_record_the_plate
quest_record_which_gazetteer
quest_record_wrong_stacks
quest_redemption_arc
quest_registry
quest_rep_meal_row
quest_rep_night_slate
quest_roster_12b_kit
quest_roster_ansel_truth
quest_roster_blank_access
quest_roster_boot_crate
quest_roster_brigid
quest_roster_caretaker
quest_roster_chair
quest_roster_fourteenth
quest_roster_frayne_minutes
quest_roster_grange_vote
quest_roster_hadi_shift
quest_roster_ink
quest_roster_ivy_oil
quest_roster_kess_pencil
quest_roster_len_tag
quest_roster_missing_strip
quest_roster_nila_eleven
quest_roster_pell_numbers
quest_roster_quiet
quest_roster_sole
quest_roster_tamsin_watch
quest_roster_the_chart
quest_roster_the_column
quest_roster_the_tin
quest_roster_who_eats
quest_roster_window
quest_separation_anxiety
quest_shattered_glass
quest_st_maren_last_shift
quest_target
quest_tears_in_rain
quest_the_abandoned_school
quest_the_anesthetic
quest_the_archive
quest_the_awakening
quest_the_backdraft
quest_the_bank_heist
quest_the_big_score
quest_the_blueprints
quest_the_botched_job
quest_the_boy_who_cried_wolf
quest_the_broken_mind
quest_the_broken_promise
quest_the_bunker_breached
quest_the_canary
quest_the_city_mains
quest_the_cleansing
quest_the_clearcut
quest_the_dead_stars
quest_the_empty_bottles
quest_the_empty_crib
quest_the_escape
quest_the_final_broadcast
quest_the_final_harvest
quest_the_final_payload
quest_the_first_save
quest_the_gladiator
quest_the_golden_parachute
quest_the_hard_reboot
quest_the_holdout
quest_the_independent
quest_the_inferno
quest_the_iron_gate
quest_the_iron_worm
quest_the_kevlar_loom
quest_the_lab_ruin
quest_the_last_contract
quest_the_last_ride
quest_the_last_seed
quest_the_last_stash
quest_the_locket
quest_the_long_haul
quest_the_long_night
quest_the_lost_route
quest_the_marathon
quest_the_mask_slips
quest_the_mass_grave
quest_the_masterpiece
```


# Appendix G.18 — Supporting Regression Evidence: `Ashfall.Core.Tests/Narrative/YearOfAshQuestJsonParityTests.cs`

### `Ashfall.Core.Tests/Narrative/YearOfAshQuestJsonParityTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 164; SHA-256: `e3d50b8f1763b2722dfca58e89c22efd814191f07ebb200ecd382752c45bdf3b`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CanonicalCatalog_ExistingEightQuestlines_MatchBuiltInBaseline
CanonicalCatalog_ContainsExactlySevenPlan114Questlines
PilotQuestline_GarrisonBloodDebt_LoadsAndPlaysThroughChoices
```


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| active questline records, stages, choices and persistence DTO | QuestlineSystem | dynamic questline data loading | DynamicQuestlineCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| active questline records, stages, choices and persistence DTO | QuestlineSystem | separate authored questline master authority | QuestlineMasterCatalog | Owner emits/reads a typed fact; no mirror state. |
| active questline records, stages, choices and persistence DTO | QuestlineSystem | host state and commands | DynamicQuestHostSession | Owner emits/reads a typed fact; no mirror state. |
| active questline records, stages, choices and persistence DTO | QuestlineSystem | current presentation | DynamicQuestlinePanel | Owner emits/reads a typed fact; no mirror state. |
| active questline records, stages, choices and persistence DTO | QuestlineSystem | current quest data parity evidence | YearOfAshQuestJsonParityTests | Owner emits/reads a typed fact; no mirror state. |
| dynamic questline data loading | DynamicQuestlineCatalogLoader | active questline records, stages, choices and persistence DTO | QuestlineSystem | Owner emits/reads a typed fact; no mirror state. |
| dynamic questline data loading | DynamicQuestlineCatalogLoader | separate authored questline master authority | QuestlineMasterCatalog | Owner emits/reads a typed fact; no mirror state. |
| dynamic questline data loading | DynamicQuestlineCatalogLoader | host state and commands | DynamicQuestHostSession | Owner emits/reads a typed fact; no mirror state. |
| dynamic questline data loading | DynamicQuestlineCatalogLoader | current presentation | DynamicQuestlinePanel | Owner emits/reads a typed fact; no mirror state. |
| dynamic questline data loading | DynamicQuestlineCatalogLoader | current quest data parity evidence | YearOfAshQuestJsonParityTests | Owner emits/reads a typed fact; no mirror state. |
| separate authored questline master authority | QuestlineMasterCatalog | active questline records, stages, choices and persistence DTO | QuestlineSystem | Owner emits/reads a typed fact; no mirror state. |
| separate authored questline master authority | QuestlineMasterCatalog | dynamic questline data loading | DynamicQuestlineCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| separate authored questline master authority | QuestlineMasterCatalog | host state and commands | DynamicQuestHostSession | Owner emits/reads a typed fact; no mirror state. |
| separate authored questline master authority | QuestlineMasterCatalog | current presentation | DynamicQuestlinePanel | Owner emits/reads a typed fact; no mirror state. |
| separate authored questline master authority | QuestlineMasterCatalog | current quest data parity evidence | YearOfAshQuestJsonParityTests | Owner emits/reads a typed fact; no mirror state. |
| host state and commands | DynamicQuestHostSession | active questline records, stages, choices and persistence DTO | QuestlineSystem | Owner emits/reads a typed fact; no mirror state. |
| host state and commands | DynamicQuestHostSession | dynamic questline data loading | DynamicQuestlineCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| host state and commands | DynamicQuestHostSession | separate authored questline master authority | QuestlineMasterCatalog | Owner emits/reads a typed fact; no mirror state. |
| host state and commands | DynamicQuestHostSession | current presentation | DynamicQuestlinePanel | Owner emits/reads a typed fact; no mirror state. |
| host state and commands | DynamicQuestHostSession | current quest data parity evidence | YearOfAshQuestJsonParityTests | Owner emits/reads a typed fact; no mirror state. |
| current presentation | DynamicQuestlinePanel | active questline records, stages, choices and persistence DTO | QuestlineSystem | Owner emits/reads a typed fact; no mirror state. |
| current presentation | DynamicQuestlinePanel | dynamic questline data loading | DynamicQuestlineCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| current presentation | DynamicQuestlinePanel | separate authored questline master authority | QuestlineMasterCatalog | Owner emits/reads a typed fact; no mirror state. |
| current presentation | DynamicQuestlinePanel | host state and commands | DynamicQuestHostSession | Owner emits/reads a typed fact; no mirror state. |
| current presentation | DynamicQuestlinePanel | current quest data parity evidence | YearOfAshQuestJsonParityTests | Owner emits/reads a typed fact; no mirror state. |
| current quest data parity evidence | YearOfAshQuestJsonParityTests | active questline records, stages, choices and persistence DTO | QuestlineSystem | Owner emits/reads a typed fact; no mirror state. |
| current quest data parity evidence | YearOfAshQuestJsonParityTests | dynamic questline data loading | DynamicQuestlineCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| current quest data parity evidence | YearOfAshQuestJsonParityTests | separate authored questline master authority | QuestlineMasterCatalog | Owner emits/reads a typed fact; no mirror state. |
| current quest data parity evidence | YearOfAshQuestJsonParityTests | host state and commands | DynamicQuestHostSession | Owner emits/reads a typed fact; no mirror state. |
| current quest data parity evidence | YearOfAshQuestJsonParityTests | current presentation | DynamicQuestlinePanel | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | No new questline system, save section or master catalog. Any new row must resolve through the existing loader and current questline state machine. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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

> **DR-06 — Integration ledger state differs from the v1.0 queue snapshot. VERIFIED.**
Live `INTEGRATION_PLANS.md` (read 2026-09-24) shows, at minimum: the **XP Expansion W1** batch ACTIVE (difficulty authority package `XP-WAVE1-DIFFICULTY-AUTHORITY`, with a premise correction recorded against Plan 122 SOFC fuel); the **DISTRESS-SIGNALS-9-12** flagship COMPLETE and presented for acceptance; **Plan 24 CLOSED** (Wave 8, 2026-09-17, signatures resolved 2026-09-18, ward staffing sealed under option b, `DEBT-PLAN24-MEDICAL-WARD-STAFFING` RETIRED); **19A/19B/19C** waves closed with evidence (Endgame 84/84 PASS, focused suites 47 PASS, `verify-fast.sh` reported ALL 47 GATES PASSED); **C2[2] Plan 17** legibility executed with a documented not-executed list (Plan 31 semantic-kind authority, 17C audio phases, 17B deep test matrix remain open gaps); **PR 3 content seal SEALED 2026-09-19** (`CF-P1-DISTRESS-CONTENT-SEAL`); the **availability consumer RETIRED** (Wave 9 Part 2, Option B approved; `SignalTrustAvailability` retained as a pure-math specification pin). Consequence: subject plans in the radio/distress domain must treat the rescue-signal runtime as *sealed and closed*, not as an open expansion surface, unless they extend it through its recorded seams.

> **DR-07 — Gate-count and test-total drift. HIGH CONFIDENCE.**
The v1.0 bible states 57 CI gates at v1.1.0 (53 fast + 3 full + 1 performance) and quotes both 11,098 and 11,697 full-suite totals from different handoffs. The live 19-wave closeout evidence in `INTEGRATION_PLANS.md` records `verify-fast.sh` ALL 47 GATES PASSED at that batch's close. These figures cannot all describe the same instant. Factory rule: any subject plan that names a gate count or test total must re-verify the number against the live gate inventory at drafting time and cite the closeout it came from. Never carry counts forward from this or any prior document.

> **DR-10 — v1.0 items the audit could not confirm in this pass. UNVERIFIED.**
Not confirmed in this audit pass (single-session, listing-level access): the 11,697 test total; the D1 seal state; the full 57-gate inventory; codec version pin values; the `ClaimPersonalBelonging` no-caller status; decision-blocked item states beyond those the ledger records as resolved. Each of these remains plausible but must be re-verified in live source before any plan depends on it. Factory rule: UNVERIFIED premises get a verification step inside the plan, never silent trust.

> The following v1.0 structures were confirmed by the audit and remain authoritative: the four-tier architecture (Tier 1 data authority in `Assets/StreamingAssets/Data/`; Tier 2 engine-free Core; Tier 3 `src/Host` + `src/UI`; Tier 4 xUnit plus the `HostCli` selftest surface); the `AGENTS.md` non-negotiable rules (Godot authoritative, Core engine-free, JSON authoritative, one authority per concern, focused verification); the narrative corpus under `Assets/StreamingAssets/Data/narrative/` (present in the live listing); the faction, economy, weather, Year-of-Ash, moral-choice, muster, and verdict catalog families (all present live); and the plan-discipline artifacts (`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `SESSION_HANDOFF.md`) at root.

> **Step 2 — Select exactly one lane and one subsystem cluster.**
From Part III. The selection rule is lane rotation discipline (v1.0 Part 11): at most one plan per lane per wave; data-first lanes (A, C, J) precede wiring lanes (B, D, E) within the same domain. The session states the lane and cluster in the plan header.

> **Step 4 — Draft the subject plan in the v2.0 subject-plan format (Part V, Template S).**
A subject plan is not an integration plan. It states WHAT should expand, WHY (with evidence), WHAT MUST NOT CHANGE, and WHICH INTEGRATION ROUTE the repository should prefer — but it does not prescribe line-level implementation. The integration route recommendation (Part V, Template R) names the tier (data-only / host wiring / Core extension), the seams, the save impact class, and the verification class. This preserves the repo's own separation: subject plans propose; integration plans (drafted later, against the live tree, in an owning session) commit.

> - One plan = one bounded outcome riding existing seams (v1.0 Part 10). The factory never widens a plan to reach a size target.
- No plan may create a parallel authority. Every state change names its owning system.
- Data-first preference: if an expansion can be authored as JSON through an existing loader, it must be, and the plan must say so.
- The factory never drafts against decision-blocked items (the current list must be re-read from `INTEGRATION_PLANS.md` each session — DR-06 shows signatures resolve over time).
- Subject plans do not edit files. Implementation happens only in an owning session after plan selection (v1.0 approval-based workflow).
- Every generated plan must state its position relative to each epilogue permutation it touches (v1.0 Part 6.6).

> Ten lanes (A–J, from v1.0 Part 11) against seventeen subsystem clusters distilled from the live Core inventory (v1.0 Parts 5.1–5.2 and 16, confirmed live). Each cell names an opening archetype. Confidence labels reflect the audit state as of 2026-09-24 and must be re-checked at drafting time. This matrix is the combinatorial engine: 170 cells, each capable of yielding multiple subject plans over time as content lands and seams mature. Not every cell is currently open; cells marked SEALED are closed by evidence (e.g., the distress-signal content seal, DR-06) and may not be opened without new evidence and foreman signature.

**Applied constraints:** one bounded outcome, live-source collision sweep, explicit data/loader/consumer/save/test seams, no parallel authority, no unsupported content growth, and a final precision pass. Master file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Recorded SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.


# Appendix — Deep integration architecture

# Appendix — Deep Integration Architecture

## A. Boundary and responsibility map

Current evidence and safe integration boundary for Plan 59: Dynamic Questlines, Year of Ashes and Multi-Stage State.

- **active questline records, stages, choices and persistence DTO** remains with `QuestlineSystem` at `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs`. Sole dynamic questline runtime owner.
- **dynamic questline data loading** remains with `DynamicQuestlineCatalogLoader` at `Assets/Ashfall.Core/YearOfAsh/DynamicQuestlineCatalogLoader.cs`. Static dynamic catalog owner.
- **separate authored questline master authority** remains with `QuestlineMasterCatalog` at `Assets/Ashfall.Core/QuestlineMasterCatalog.cs`. Do not duplicate or merge by row count.
- **host state and commands** remains with `DynamicQuestHostSession` at `src/Host/DynamicQuestHostSession.cs`. Thin host adapter.
- **current presentation** remains with `DynamicQuestlinePanel` at `src/UI/DynamicQuestlinePanel.cs`. Read-only projection/commands.
- **current quest data parity evidence** remains with `YearOfAshQuestJsonParityTests` at `Ashfall.Core.Tests/Narrative/YearOfAshQuestJsonParityTests.cs`. Focused evidence.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load two dynamic definitions
2. validate IDs, stages, items, locations and flags
3. read current questline state
4. show playable/withheld definitions
5. route start/choice through QuestlineSystem
6. apply current reward/consequence owner
7. capture/restore current questline state

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- Current two authored definitions are immutable.
- QuestlineSystemState owns active/resolved records and stage progress.
- Master questline state is separate and must be reconciled by IDs, not copied.
- Rewards and consequences route to current owners.

- A questline cannot start without a current valid definition.
- Stage order and choice IDs are exact.
- Unknown item/location/flag references fail closed.
- A resolved questline cannot re-resolve or replay rewards.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/Host/DynamicQuestHostSession.cs
- src/UI/DynamicQuestlinePanel.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/Narrative/YearOfAshQuestJsonParityTests.cs

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
| S-01 | 59-01 two rows load | load two dynamic definitions | Current two authored definitions are immutable. | A new row duplicates a master questline. | QuestlineSystem |
| S-02 | 59-02 master ID parity | validate IDs, stages, items, locations and flags | QuestlineSystemState owns active/resolved records and stage progress. | A missing item/location passes validation. | QuestlineSystem |
| S-03 | 59-03 stage order | read current questline state | Master questline state is separate and must be reconciled by IDs, not copied. | A panel advances a stage directly. | QuestlineSystem |
| S-04 | 59-04 item reference | show playable/withheld definitions | Rewards and consequences route to current owners. | Rewards are granted twice after reload. | QuestlineSystem |
| S-05 | 59-05 location reference | route start/choice through QuestlineSystem | Current two authored definitions are immutable. | A new quest state store is introduced. | QuestlineSystem |
| S-06 | 59-06 flag reference | apply current reward/consequence owner | QuestlineSystemState owns active/resolved records and stage progress. | A new row duplicates a master questline. | QuestlineSystem |
| S-07 | 59-07 start once | capture/restore current questline state | Master questline state is separate and must be reconciled by IDs, not copied. | A missing item/location passes validation. | QuestlineSystem |
| S-08 | 59-08 choice result | load two dynamic definitions | Rewards and consequences route to current owners. | A panel advances a stage directly. | QuestlineSystem |
| S-09 | 59-09 reward once | validate IDs, stages, items, locations and flags | Current two authored definitions are immutable. | Rewards are granted twice after reload. | QuestlineSystem |
| S-10 | 59-10 reload continuation | read current questline state | QuestlineSystemState owns active/resolved records and stage progress. | A new quest state store is introduced. | QuestlineSystem |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 59-TC-01 schema/count | data | schema/count; verify the named current owner and its negative boundary without inventing a second authority. | QuestlineSystem |
| T-02 | 59-TC-02 ID uniqueness | unit | ID uniqueness; verify the named current owner and its negative boundary without inventing a second authority. | QuestlineSystem |
| T-03 | 59-TC-03 stage contract | persistence | stage contract; verify the named current owner and its negative boundary without inventing a second authority. | QuestlineSystem |
| T-04 | 59-TC-04 reference resolution | determinism | reference resolution; verify the named current owner and its negative boundary without inventing a second authority. | QuestlineSystem |
| T-05 | 59-TC-05 state transition | host | state transition; verify the named current owner and its negative boundary without inventing a second authority. | QuestlineSystem |
| T-06 | 59-TC-06 reward exactly once | UI/accessibility | reward exactly once; verify the named current owner and its negative boundary without inventing a second authority. | QuestlineSystem |
| T-07 | 59-TC-07 save round trip | cross-system | save round trip; verify the named current owner and its negative boundary without inventing a second authority. | QuestlineSystem |
| T-08 | 59-TC-08 UI truth | data | UI truth; verify the named current owner and its negative boundary without inventing a second authority. | QuestlineSystem |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 36 | `Ashfall.Core.Tests/QuestlineSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 11 | `Ashfall.Core.Tests/DoseQuestOwnershipTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 11 | `Ashfall.Core.Tests/Integration/FullCampaign30DayShelterPlaythroughTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 11 | `Ashfall.Core.Tests/VerdictQuestOwnershipTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `Ashfall.Core.Tests/YearOfAshTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 10 | `Assets/Ashfall.Core/YearOfAsh/YearOfAshSave.cs` | current reference count; inspect the caller before treating it as a live route |
| 9 | `Ashfall.Core.Tests/Quests/DynamicQuestlineTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 9 | `Assets/Ashfall.Core/Verdict/VerdictSave.cs` | current reference count; inspect the caller before treating it as a live route |
| 8 | `src/Host/DoseLedgerHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Ashfall.Core.Tests/NarrativeQuestlineSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `src/Host/ContentUtilizationRuntimeCollector.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Assets/Ashfall.Core/DoseLedgerSave.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `Assets/Ashfall.Core/QuestlineMasterCatalog.cs` | current reference count; inspect the caller before treating it as a live route |
| 6 | `src/Main.Plans46_49.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Ashfall.Core.Tests/QuestlineMasterCatalogTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Assets/Ashfall.Core/DoseQuestMigration.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `src/YearOfAsh/YearOfAshHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Ashfall.Core.Tests/Medical/Plan112_113DiseaseVerdictIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `Assets/Ashfall.Core/Verdict/VerdictQuestMigration.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Host/DynamicQuestHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Host/NarrativeQuestlineHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Host/VerdictHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/DataWiringIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/DoseContentCatalogTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/YearOfAshPlan114ExpansionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/Quests/DynamicQuestlines.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/Quests/NarrativeQuestlineSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/YearOfAsh/DynamicQuestlineCatalogLoader.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/Main.DynamicQuestGeneration.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/UI/DynamicQuestlinePanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/DoseCollectibleSaveFuzzTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/DoseQuestExpansionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Narrative/Plan96_104ChronicleQuestIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `Ashfall.Core.Tests/Tooling/LoaderWiringGateTests.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/dynamic_questlines.json`

### `Assets/StreamingAssets/Data/dynamic_questlines.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 4153; characters: 4147.
- SHA-256: `d02872c38df6a2c8e7bba7f093f5850ec92adff03645e33be5b4cab6dac80d26`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `questlines`

#### `questlines` — 2 current rows

- Row 001 `quest_dying_signal`: `{"quest_id":"quest_dying_signal","stages":[{"description":"Intercept a repeating 142.5 MHz distress beacon from the Regional Communications Array. The signal pattern is military — but the voice sounds civilian.","name":"Discovery","objecti…`
- Row 002 `quest_aquifer_contamination`: `{"quest_id":"quest_aquifer_contamination","stages":[{"description":"The Water Purity Gauge reports a sudden 40% spike in heavy metal contamination. The source must be upstream.","name":"Discovery","objective_items":["water_sample_contamina…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/questline_master.json`

### `Assets/StreamingAssets/Data/questline_master.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 66388; characters: 66372.
- SHA-256: `f7ef6d26b20fd2f87137cc1836cfd72e88c6baebae6bccee80affad841c2efc8`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `entries`

#### `entries` — 511 current rows

- Row 001 `quest_a_good_death`: `{"id":"quest_a_good_death"}`
- Row 002 `quest_a_real_leader`: `{"id":"quest_a_real_leader"}`
- Row 003 `quest_a_voice_in_the_dark`: `{"id":"quest_a_voice_in_the_dark"}`
- Row 004 `quest_acoustic_bait`: `{"id":"quest_acoustic_bait"}`
- Row 005 `quest_ashen_communion`: `{"id":"quest_ashen_communion"}`
- Row 006 `quest_assassination_deserter`: `{"id":"quest_assassination_deserter"}`
- Row 007 `quest_black_vein`: `{"id":"quest_black_vein"}`
- Row 008 `quest_breaking_chains`: `{"id":"quest_breaking_chains"}`
- Row 009 `quest_broken_chronometer`: `{"id":"quest_broken_chronometer"}`
- Row 010 `quest_burnt_harvest`: `{"id":"quest_burnt_harvest"}`
- Row 011 `quest_checkpoint_kilo_truth`: `{"id":"quest_checkpoint_kilo_truth"}`
- Row 012 `quest_child_soldier_rifle`: `{"id":"quest_child_soldier_rifle"}`
- Row 013 `quest_cold_turkey`: `{"id":"quest_cold_turkey"}`
- Row 014 `quest_court_martial`: `{"id":"quest_court_martial"}`
- Row 015 `quest_crisis_of_faith`: `{"id":"quest_crisis_of_faith"}`
- Row 016 `quest_crossing_companion_mattis`: `{"id":"quest_crossing_companion_mattis"}`
- Row 017 `quest_crossing_first_weigh`: `{"id":"quest_crossing_first_weigh"}`
- Row 018 `quest_crossing_scale_integrity`: `{"id":"quest_crossing_scale_integrity"}`
- Row 019 `quest_crossing_the_forfeit`: `{"id":"quest_crossing_the_forfeit"}`
- Row 020 `quest_crossing_the_marker`: `{"id":"quest_crossing_the_marker"}`
- Row 021 `quest_crossing_the_petition`: `{"id":"quest_crossing_the_petition"}`
- Row 022 `quest_crossing_the_standing`: `{"id":"quest_crossing_the_standing"}`
- Row 023 `quest_crossing_the_terms`: `{"id":"quest_crossing_the_terms"}`
- Row 024 `quest_crossing_the_vote_that_isnt`: `{"id":"quest_crossing_the_vote_that_isnt"}`
- Row 025 `quest_crossing_the_vouch`: `{"id":"quest_crossing_the_vouch"}`
- Row 026 `quest_crossing_three_dry_pages`: `{"id":"quest_crossing_three_dry_pages"}`
- Row 027 `quest_crossing_who_holds_the_ledger`: `{"id":"quest_crossing_who_holds_the_ledger"}`
- Row 028 `quest_cult_glow_communion`: `{"id":"quest_cult_glow_communion"}`
- Row 029 `quest_cult_purity`: `{"id":"quest_cult_purity"}`
- Row 030 `quest_dead_air`: `{"id":"quest_dead_air"}`
- Row 031 `quest_deep_well`: `{"id":"quest_deep_well"}`
- Row 032 `quest_dropping_the_rifle`: `{"factionTag":"","id":"quest_dropping_the_rifle","narrativeHook":"apprenticeship_medic_or_smith","status":"active","synopsis":"The Child Soldier keeps the rifle the way soldiers keep things they shouldn't. A hand-me-down maxim: the rifle k…`
- Row 033 `quest_earning_keep`: `{"id":"quest_earning_keep"}`
- Row 034 `quest_elena_triage`: `{"id":"quest_elena_triage"}`
- Row 035 `quest_embracing_the_glow`: `{"id":"quest_embracing_the_glow"}`
- Row 036 `quest_first_blood`: `{"factionTag":"","id":"quest_first_blood","narrativeHook":"mentorship_veteran_pair","status":"active","synopsis":"The Hardened Daughter eats with a piece of broken glass wrapped in cloth and watches the door. Someone will eventually have t…`
- Row 037 `quest_garrison_census`: `{"id":"quest_garrison_census"}`
- Row 038 `quest_garrison_last_order`: `{"id":"quest_garrison_last_order"}`
- Row 039 `quest_ghosts_of_day_1`: `{"id":"quest_ghosts_of_day_1"}`
- Row 040 `quest_glow_communion`: `{"id":"quest_glow_communion"}`
- Row 041 `quest_grain_war`: `{"id":"quest_grain_war"}`
- Row 042 `quest_ground_zero`: `{"id":"quest_ground_zero"}`
- Row 043 `quest_growing_up_fast`: `{"factionTag":"","id":"quest_growing_up_fast","narrativeHook":"schooling_curriculum_choice","status":"active","synopsis":"The Naive Son draws animals on the wall and asks questions about birds. The adults answer in halves. The bunker has a…`
- Row 044 `quest_hell_is_other_people`: `{"id":"quest_hell_is_other_people"}`
- Row 045 `quest_holdfast_authentication`: `{"id":"quest_holdfast_authentication"}`
- Row 046 `quest_holdfast_the_clerk`: `{"id":"quest_holdfast_the_clerk"}`
- Row 047 `quest_holdfast_the_clerk_started`: `{"id":"quest_holdfast_the_clerk_started"}`
- Row 048 `quest_holdfast_the_drawer`: `{"id":"quest_holdfast_the_drawer"}`
- Row 049 `quest_holdfast_the_hatch`: `{"id":"quest_holdfast_the_hatch"}`
- Row 050 `quest_holdfast_the_levy`: `{"id":"quest_holdfast_the_levy"}`
- Row 051 `quest_holdfast_the_membrane`: `{"id":"quest_holdfast_the_membrane"}`
- Row 052 `quest_holdfast_the_plant`: `{"id":"quest_holdfast_the_plant"}`
- Row 053 `quest_holdfast_the_second_list`: `{"id":"quest_holdfast_the_second_list"}`
- Row 054 `quest_holdfast_the_sheet`: `{"id":"quest_holdfast_the_sheet"}`
- Row 055 `quest_holdfast_the_window`: `{"id":"quest_holdfast_the_window"}`
- Row 056 `quest_holding_the_line`: `{"id":"quest_holding_the_line"}`
- Row 057 `quest_in_the_black`: `{"id":"quest_in_the_black"}`
- Row 058 `quest_inventory_audit`: `{"id":"quest_inventory_audit"}`
- Row 059 `quest_iron_ledger`: `{"id":"quest_iron_ledger"}`
- Row 060 `quest_loss_of_faith`: `{"id":"quest_loss_of_faith"}`
- Row 061 `quest_mans_best_friend`: `{"id":"quest_mans_best_friend"}`
- Row 062 `quest_mechanic_highway_heart`: `{"id":"quest_mechanic_highway_heart"}`
- Row 063 `quest_militia_grain_war`: `{"id":"quest_militia_grain_war"}`
- Row 064 `quest_militia_smugglers_route`: `{"id":"quest_militia_smugglers_route"}`
- Row 065 `quest_museum_archive`: `{"id":"quest_museum_archive"}`
- Row 066 `quest_player_one`: `{"id":"quest_player_one"}`
- Row 067 `quest_putting_down_roots`: `{"id":"quest_putting_down_roots"}`
- Row 068 `quest_rebuilders_thirst`: `{"id":"quest_rebuilders_thirst"}`
- Row 069 `quest_record_fallback`: `{"id":"quest_record_fallback"}`
- Row 070 `quest_record_friendly_obstacle`: `{"id":"quest_record_friendly_obstacle"}`
- Row 071 `quest_record_grease_pencil`: `{"id":"quest_record_grease_pencil"}`
- Row 072 `quest_record_hands`: `{"id":"quest_record_hands"}`
- Row 073 `quest_record_mass_or_lot`: `{"id":"quest_record_mass_or_lot"}`
- Row 074 `quest_record_the_book`: `{"id":"quest_record_the_book"}`
- Row 075 `quest_record_the_failure`: `{"id":"quest_record_the_failure"}`
- Row 076 `quest_record_the_plate`: `{"id":"quest_record_the_plate"}`
- Row 077 `quest_record_which_gazetteer`: `{"id":"quest_record_which_gazetteer"}`
- Row 078 `quest_record_wrong_stacks`: `{"id":"quest_record_wrong_stacks"}`
- Row 079 `quest_redemption_arc`: `{"id":"quest_redemption_arc"}`
- Row 080 `quest_registry`: `{"id":"quest_registry"}`
- Row 081 `quest_rep_meal_row`: `{"id":"quest_rep_meal_row"}`
- Row 082 `quest_rep_night_slate`: `{"id":"quest_rep_night_slate"}`
- Row 083 `quest_roster_12b_kit`: `{"id":"quest_roster_12b_kit"}`
- Row 084 `quest_roster_ansel_truth`: `{"id":"quest_roster_ansel_truth"}`
- Row 085 `quest_roster_blank_access`: `{"id":"quest_roster_blank_access"}`
- Row 086 `quest_roster_boot_crate`: `{"id":"quest_roster_boot_crate"}`
- Row 087 `quest_roster_brigid`: `{"id":"quest_roster_brigid"}`
- Row 088 `quest_roster_caretaker`: `{"id":"quest_roster_caretaker"}`
- Row 089 `quest_roster_chair`: `{"id":"quest_roster_chair"}`
- Row 090 `quest_roster_fourteenth`: `{"id":"quest_roster_fourteenth"}`
- Row 091 `quest_roster_frayne_minutes`: `{"id":"quest_roster_frayne_minutes"}`
- Row 092 `quest_roster_grange_vote`: `{"id":"quest_roster_grange_vote"}`
- Row 093 `quest_roster_hadi_shift`: `{"id":"quest_roster_hadi_shift"}`
- Row 094 `quest_roster_ink`: `{"id":"quest_roster_ink"}`
- Row 095 `quest_roster_ivy_oil`: `{"id":"quest_roster_ivy_oil"}`
- Row 096 `quest_roster_kess_pencil`: `{"id":"quest_roster_kess_pencil"}`
- Row 097 `quest_roster_len_tag`: `{"id":"quest_roster_len_tag"}`
- Row 098 `quest_roster_missing_strip`: `{"id":"quest_roster_missing_strip"}`
- Row 099 `quest_roster_nila_eleven`: `{"id":"quest_roster_nila_eleven"}`
- Row 100 `quest_roster_pell_numbers`: `{"id":"quest_roster_pell_numbers"}`
- Row 101 `quest_roster_quiet`: `{"id":"quest_roster_quiet"}`
- Row 102 `quest_roster_sole`: `{"id":"quest_roster_sole"}`
- Row 103 `quest_roster_tamsin_watch`: `{"id":"quest_roster_tamsin_watch"}`
- Row 104 `quest_roster_the_chart`: `{"id":"quest_roster_the_chart"}`
- Row 105 `quest_roster_the_column`: `{"id":"quest_roster_the_column"}`
- Row 106 `quest_roster_the_tin`: `{"id":"quest_roster_the_tin"}`
- Row 107 `quest_roster_who_eats`: `{"id":"quest_roster_who_eats"}`
- Row 108 `quest_roster_window`: `{"id":"quest_roster_window"}`
- Row 109 `quest_separation_anxiety`: `{"id":"quest_separation_anxiety"}`
- Row 110 `quest_shattered_glass`: `{"id":"quest_shattered_glass"}`
- Row 111 `quest_st_maren_last_shift`: `{"id":"quest_st_maren_last_shift"}`
- Row 112 `quest_target`: `{"id":"quest_target"}`
- Row 113 `quest_tears_in_rain`: `{"id":"quest_tears_in_rain"}`
- Row 114 `quest_the_abandoned_school`: `{"id":"quest_the_abandoned_school"}`
- Row 115 `quest_the_anesthetic`: `{"id":"quest_the_anesthetic"}`
- Row 116 `quest_the_archive`: `{"id":"quest_the_archive"}`
- Row 117 `quest_the_awakening`: `{"id":"quest_the_awakening"}`
- Row 118 `quest_the_backdraft`: `{"id":"quest_the_backdraft"}`
- Row 119 `quest_the_bank_heist`: `{"id":"quest_the_bank_heist"}`
- Row 120 `quest_the_big_score`: `{"id":"quest_the_big_score"}`
- Row 121 `quest_the_blueprints`: `{"id":"quest_the_blueprints"}`
- Row 122 `quest_the_botched_job`: `{"id":"quest_the_botched_job"}`
- Row 123 `quest_the_boy_who_cried_wolf`: `{"id":"quest_the_boy_who_cried_wolf"}`
- Row 124 `quest_the_broken_mind`: `{"id":"quest_the_broken_mind"}`
- Row 125 `quest_the_broken_promise`: `{"id":"quest_the_broken_promise"}`
- Row 126 `quest_the_bunker_breached`: `{"id":"quest_the_bunker_breached"}`
- Row 127 `quest_the_canary`: `{"id":"quest_the_canary"}`
- Row 128 `quest_the_city_mains`: `{"id":"quest_the_city_mains"}`
- Row 129 `quest_the_cleansing`: `{"id":"quest_the_cleansing"}`
- Row 130 `quest_the_clearcut`: `{"id":"quest_the_clearcut"}`
- Row 131 `quest_the_dead_stars`: `{"id":"quest_the_dead_stars"}`
- Row 132 `quest_the_empty_bottles`: `{"id":"quest_the_empty_bottles"}`
- Row 133 `quest_the_empty_crib`: `{"id":"quest_the_empty_crib"}`
- Row 134 `quest_the_escape`: `{"id":"quest_the_escape"}`
- Row 135 `quest_the_final_broadcast`: `{"id":"quest_the_final_broadcast"}`
- Row 136 `quest_the_final_harvest`: `{"id":"quest_the_final_harvest"}`
- Row 137 `quest_the_final_payload`: `{"id":"quest_the_final_payload"}`
- Row 138 `quest_the_first_save`: `{"id":"quest_the_first_save"}`
- Row 139 `quest_the_gladiator`: `{"id":"quest_the_gladiator"}`
- Row 140 `quest_the_golden_parachute`: `{"id":"quest_the_golden_parachute"}`
- Row 141 `quest_the_hard_reboot`: `{"id":"quest_the_hard_reboot"}`
- Row 142 `quest_the_holdout`: `{"id":"quest_the_holdout"}`
- Row 143 `quest_the_independent`: `{"id":"quest_the_independent"}`
- Row 144 `quest_the_inferno`: `{"id":"quest_the_inferno"}`
- Row 145 `quest_the_iron_gate`: `{"id":"quest_the_iron_gate"}`
- Row 146 `quest_the_iron_worm`: `{"id":"quest_the_iron_worm"}`
- Row 147 `quest_the_kevlar_loom`: `{"id":"quest_the_kevlar_loom"}`
- Row 148 `quest_the_lab_ruin`: `{"id":"quest_the_lab_ruin"}`
- Row 149 `quest_the_last_contract`: `{"id":"quest_the_last_contract"}`
- Row 150 `quest_the_last_ride`: `{"id":"quest_the_last_ride"}`
- Row 151 `quest_the_last_seed`: `{"id":"quest_the_last_seed"}`
- Row 152 `quest_the_last_stash`: `{"id":"quest_the_last_stash"}`
- Row 153 `quest_the_locket`: `{"id":"quest_the_locket"}`
- Row 154 `quest_the_long_haul`: `{"id":"quest_the_long_haul"}`
- Row 155 `quest_the_long_night`: `{"id":"quest_the_long_night"}`
- Row 156 `quest_the_lost_route`: `{"id":"quest_the_lost_route"}`
- Row 157 `quest_the_marathon`: `{"id":"quest_the_marathon"}`
- Row 158 `quest_the_mask_slips`: `{"id":"quest_the_mask_slips"}`
- Row 159 `quest_the_mass_grave`: `{"id":"quest_the_mass_grave"}`
- Row 160 `quest_the_masterpiece`: `{"id":"quest_the_masterpiece"}`
- Row 161 `quest_the_masters_fate`: `{"id":"quest_the_masters_fate"}`
- Row 162 `quest_the_mess`: `{"id":"quest_the_mess"}`
- Row 163 `quest_the_motorpool`: `{"id":"quest_the_motorpool"}`
- Row 164 `quest_the_old_woods`: `{"id":"quest_the_old_woods"}`
- Row 165 `quest_the_pack`: `{"factionTag":"","id":"quest_the_pack","narrativeHook":"orphan_adoption_pair_choice","status":"active","synopsis":"The Feral Orphan will not eat food that has been cooked or touched. A blanket folded and left on warm concrete stays a blank…`
- Row 166 `quest_the_perfect_equation`: `{"id":"quest_the_perfect_equation"}`
- Row 167 `quest_the_perfect_fake`: `{"id":"quest_the_perfect_fake"}`
- Row 168 `quest_the_precinct`: `{"id":"quest_the_precinct"}`
- Row 169 `quest_the_prosthetic`: `{"id":"quest_the_prosthetic"}`
- Row 170 `quest_the_rabid_pack`: `{"id":"quest_the_rabid_pack"}`
- Row 171 `quest_the_radar_station`: `{"id":"quest_the_radar_station"}`
- Row 172 `quest_the_rally`: `{"id":"quest_the_rally"}`
- Row 173 `quest_the_real_illness`: `{"id":"quest_the_real_illness"}`
- Row 174 `quest_the_real_world`: `{"id":"quest_the_real_world"}`
- Row 175 `quest_the_seed_vault`: `{"id":"quest_the_seed_vault"}`
- Row 176 `quest_the_shaking_hand`: `{"id":"quest_the_shaking_hand"}`
- Row 177 `quest_the_sponge`: `{"id":"quest_the_sponge"}`
- Row 178 `quest_the_stash`: `{"id":"quest_the_stash"}`
- Row 179 `quest_the_strain`: `{"id":"quest_the_strain"}`
- Row 180 `quest_the_substation_ghost`: `{"id":"quest_the_substation_ghost"}`
- Row 181 `quest_the_transit_pass`: `{"id":"quest_the_transit_pass"}`
- Row 182 `quest_the_turing_test`: `{"id":"quest_the_turing_test"}`
- Row 183 `quest_the_ultimate_price`: `{"id":"quest_the_ultimate_price"}`
- Row 184 `quest_the_ultimate_test`: `{"id":"quest_the_ultimate_test"}`
- Row 185 `quest_the_value_of_breath`: `{"id":"quest_the_value_of_breath"}`
- Row 186 `quest_the_wardens_key`: `{"id":"quest_the_wardens_key"}`
- Row 187 `quest_the_weight_of_gold`: `{"id":"quest_the_weight_of_gold"}`
- Row 188 `quest_the_white_elk`: `{"id":"quest_the_white_elk"}`
- Row 189 `quest_third_abandoned_vehicle`: `{"id":"quest_third_abandoned_vehicle"}`
- Row 190 `quest_third_collapsed_structure`: `{"id":"quest_third_collapsed_structure"}`
- Row 191 `quest_third_dead_drop`: `{"id":"quest_third_dead_drop"}`
- Row 192 `quest_third_graffiti_message`: `{"id":"quest_third_graffiti_message"}`
- Row 193 `quest_third_hidden_bunker`: `{"id":"quest_third_hidden_bunker"}`
- Row 194 `quest_third_hidden_stash`: `{"id":"quest_third_hidden_stash"}`
- Row 195 `quest_third_landmark`: `{"id":"quest_third_landmark"}`
- Row 196 `quest_third_memorial`: `{"id":"quest_third_memorial"}`
- Row 197 `quest_third_overgrown_garden`: `{"id":"quest_third_overgrown_garden"}`
- Row 198 `quest_third_radio_signal`: `{"id":"quest_third_radio_signal"}`
- Row 199 `quest_third_resource_spot`: `{"id":"quest_third_resource_spot"}`
- Row 200 `quest_third_safe_route`: `{"id":"quest_third_safe_route"}`
- Row 201 `quest_third_scorched_earth`: `{"id":"quest_third_scorched_earth"}`
- Row 202 `quest_third_supply_cache`: `{"id":"quest_third_supply_cache"}`
- Row 203 `quest_third_water_source`: `{"id":"quest_third_water_source"}`
- Row 204 `quest_third_craft_battery_swap`: `{"id":"quest_third_craft_battery_swap"}`
- Row 205 `quest_third_craft_boot_sole`: `{"id":"quest_third_craft_boot_sole"}`
- Row 206 `quest_third_craft_can_opener`: `{"id":"quest_third_craft_can_opener"}`
- Row 207 `quest_third_craft_clothing_mend`: `{"id":"quest_third_craft_clothing_mend"}`
- Row 208 `quest_third_craft_door_hinge`: `{"id":"quest_third_craft_door_hinge"}`
- Row 209 `quest_third_craft_improvise_lockpick`: `{"id":"quest_third_craft_improvise_lockpick"}`
- Row 210 `quest_third_craft_repair_tool`: `{"id":"quest_third_craft_repair_tool"}`
- Row 211 `quest_third_craft_salvage_wiring`: `{"id":"quest_third_craft_salvage_wiring"}`
- Row 212 `quest_third_craft_sharpen_blade`: `{"id":"quest_third_craft_sharpen_blade"}`
- Row 213 `quest_third_craft_shelter_patch`: `{"id":"quest_third_craft_shelter_patch"}`
- Row 214 `quest_third_craft_stove_fix`: `{"id":"quest_third_craft_stove_fix"}`
- Row 215 `quest_third_craft_trap_repair`: `{"id":"quest_third_craft_trap_repair"}`
- Row 216 `quest_third_craft_upgrade_flashlight`: `{"id":"quest_third_craft_upgrade_flashlight"}`
- Row 217 `quest_third_craft_water_filter`: `{"id":"quest_third_craft_water_filter"}`
- Row 218 `quest_third_craft_window_board`: `{"id":"quest_third_craft_window_board"}`
- Row 219 `quest_third_med_bandage_wound`: `{"id":"quest_third_med_bandage_wound"}`
- Row 220 `quest_third_med_boil_water`: `{"id":"quest_third_med_boil_water"}`
- Row 221 `quest_third_med_breathe_exercise`: `{"id":"quest_third_med_breathe_exercise"}`
- Row 222 `quest_third_med_clean_infection`: `{"id":"quest_third_med_clean_infection"}`
- Row 223 `quest_third_med_find_antibiotics`: `{"id":"quest_third_med_find_antibiotics"}`
- Row 224 `quest_third_med_find_painkillers`: `{"id":"quest_third_med_find_painkillers"}`
- Row 225 `quest_third_med_food_safety`: `{"id":"quest_third_med_food_safety"}`
- Row 226 `quest_third_med_iodine_pill`: `{"id":"quest_third_med_iodine_pill"}`
- Row 227 `quest_third_med_rest_exhaustion`: `{"id":"quest_third_med_rest_exhaustion"}`
- Row 228 `quest_third_med_sleep_hygiene`: `{"id":"quest_third_med_sleep_hygiene"}`
- Row 229 `quest_third_med_splint_break`: `{"id":"quest_third_med_splint_break"}`
- Row 230 `quest_third_med_sunburn_treat`: `{"id":"quest_third_med_sunburn_treat"}`
- Row 231 `quest_third_med_treat_blister`: `{"id":"quest_third_med_treat_blister"}`
- Row 232 `quest_third_med_treat_burn`: `{"id":"quest_third_med_treat_burn"}`
- Row 233 `quest_third_med_water_ration`: `{"id":"quest_third_med_water_ration"}`
- Row 234 `quest_third_combat_ambush_avoid`: `{"id":"quest_third_combat_ambush_avoid"}`
- Row 235 `quest_third_combat_darkness_move`: `{"id":"quest_third_combat_darkness_move"}`
- Row 236 `quest_third_combat_defend_camp`: `{"id":"quest_third_combat_defend_camp"}`
- Row 237 `quest_third_combat_dog_encounter`: `{"id":"quest_third_combat_dog_encounter"}`
- Row 238 `quest_third_combat_escape_pursuit`: `{"id":"quest_third_combat_escape_pursuit"}`
- Row 239 `quest_third_combat_group_encounter`: `{"id":"quest_third_combat_group_encounter"}`
- Row 240 `quest_third_combat_mutant_scare`: `{"id":"quest_third_combat_mutant_scare"}`
- Row 241 `quest_third_combat_negotiate_passage`: `{"id":"quest_third_combat_negotiate_passage"}`
- Row 242 `quest_third_combat_noise_discipline`: `{"id":"quest_third_combat_noise_discipline"}`
- Row 243 `quest_third_combat_raider_patrol`: `{"id":"quest_third_combat_raider_patrol"}`
- Row 244 `quest_third_combat_scavenge_threat`: `{"id":"quest_third_combat_scavenge_threat"}`
- Row 245 `quest_third_combat_shelter_fortify`: `{"id":"quest_third_combat_shelter_fortify"}`
- Row 246 `quest_third_combat_sniper_awareness`: `{"id":"quest_third_combat_sniper_awareness"}`
- Row 247 `quest_third_combat_trap_detect`: `{"id":"quest_third_combat_trap_detect"}`
- Row 248 `quest_third_combat_weapon_find`: `{"id":"quest_third_combat_weapon_find"}`
- Row 249 `quest_third_lore_children_drawing`: `{"id":"quest_third_lore_children_drawing"}`
- Row 250 `quest_third_lore_clock_tower`: `{"id":"quest_third_lore_clock_tower"}`
- Row 251 `quest_third_lore_coded_message`: `{"id":"quest_third_lore_coded_message"}`
- Row 252 `quest_third_lore_folk_song`: `{"id":"quest_third_lore_folk_song"}`
- Row 253 `quest_third_lore_graffiti_story`: `{"id":"quest_third_lore_graffiti_story"}`
- Row 254 `quest_third_lore_inscription`: `{"id":"quest_third_lore_inscription"}`
- Row 255 `quest_third_lore_letter_fragment`: `{"id":"quest_third_lore_letter_fragment"}`
- Row 256 `quest_third_lore_map_fragment`: `{"id":"quest_third_lore_map_fragment"}`
- Row 257 `quest_third_lore_medical_record`: `{"id":"quest_third_lore_medical_record"}`
- Row 258 `quest_third_lore_memorial_wall`: `{"id":"quest_third_lore_memorial_wall"}`
- Row 259 `quest_third_lore_newspaper`: `{"id":"quest_third_lore_newspaper"}`
- Row 260 `quest_third_lore_photograph`: `{"id":"quest_third_lore_photograph"}`
- Row 261 `quest_third_lore_radio_archive`: `{"id":"quest_third_lore_radio_archive"}`
- Row 262 `quest_third_lore_recording`: `{"id":"quest_third_lore_recording"}`
- Row 263 `quest_third_lore_school_exercise`: `{"id":"quest_third_lore_school_exercise"}`
- Row 264 `quest_tower_seven_broadcast`: `{"id":"quest_tower_seven_broadcast"}`
- Row 265 `quest_trial_by_fire`: `{"id":"quest_trial_by_fire"}`
- Row 266 `quest_tribute_break`: `{"id":"quest_tribute_break"}`
- Row 267 `quest_truth_of_day_30`: `{"id":"quest_truth_of_day_30"}`
- Row 268 `quest_vindicated`: `{"id":"quest_vindicated"}`
- Row 269 `quest_worthless_paper`: `{"id":"quest_worthless_paper"}`
- Row 270 `quest_continental_convoy_gate`: `{"factionTag":"","id":"quest_continental_convoy_gate","status":"active","synopsis":"","title":"Continental Convoy Gate"}`
- Row 271 `quest_granite_foundry_brass_smuggling`: `{"factionTag":"","id":"quest_granite_foundry_brass_smuggling","status":"active","synopsis":"","title":"Granite Foundry Brass Smuggling"}`
- Row 272 `quest_day_360_final_reckoning`: `{"factionTag":"","id":"quest_day_360_final_reckoning","status":"active","synopsis":"","title":"Day 360 Final Reckoning"}`
- Row 273 `quest_the_dose_the_first_reading`: `{"factionTag":"","id":"quest_the_dose_the_first_reading","status":"active","synopsis":"","title":"The Dose The First Reading"}`
- Row 274 `quest_warlord_tribute_first_payment`: `{"factionTag":"","id":"quest_warlord_tribute_first_payment","status":"active","synopsis":"","title":"Warlord Tribute First Payment"}`
- Row 275 `quest_standing_record_faction_intro`: `{"factionTag":"","id":"quest_standing_record_faction_intro","status":"active","synopsis":"","title":"Standing Record Faction Intro"}`
- Row 276 `quest_crossing_arbitration_first_case`: `{"factionTag":"","id":"quest_crossing_arbitration_first_case","status":"active","synopsis":"","title":"Crossing Arbitration First Case"}`
- Row 277 `quest_holdfast_duty_roster_intro`: `{"factionTag":"","id":"quest_holdfast_duty_roster_intro","status":"active","synopsis":"","title":"Holdfast Duty Roster Intro"}`
- Row 278 `quest_greenhouse_first_planting`: `{"factionTag":"","id":"quest_greenhouse_first_planting","status":"active","synopsis":"","title":"Greenhouse First Planting"}`
- Row 279 `quest_black_flotilla_first_contact`: `{"factionTag":"","id":"quest_black_flotilla_first_contact","status":"active","synopsis":"","title":"Black Flotilla First Contact"}`
- Row 280 `quest_faction_war_sector_4_opening`: `{"factionTag":"","id":"quest_faction_war_sector_4_opening","status":"active","synopsis":"","title":"Faction War Sector 4 Opening"}`
- Row 281 `quest_night_watch_sentry_01`: `{"factionTag":"","id":"quest_night_watch_sentry_01","status":"active","synopsis":"","title":"Night Watch Sentry 01"}`
- Row 282 `quest_ghost_transmission_01`: `{"factionTag":"","id":"quest_ghost_transmission_01","status":"active","synopsis":"","title":"Ghost Transmission 01"}`
- Row 283 `quest_relic_provenance_01`: `{"factionTag":"","id":"quest_relic_provenance_01","status":"active","synopsis":"","title":"Relic Provenance 01"}`
- Row 284 `quest_bunker_graffiti_01`: `{"factionTag":"","id":"quest_bunker_graffiti_01","status":"active","synopsis":"","title":"Bunker Graffiti 01"}`
- Row 285 `quest_dweller_medical_01`: `{"factionTag":"","id":"quest_dweller_medical_01","status":"active","synopsis":"","title":"Dweller Medical 01"}`
- Row 286 `quest_courier_dispatch_01`: `{"factionTag":"","id":"quest_courier_dispatch_01","status":"active","synopsis":"","title":"Courier Dispatch 01"}`
- Row 287 `quest_oral_lore_01`: `{"factionTag":"","id":"quest_oral_lore_01","status":"active","synopsis":"","title":"Oral Lore 01"}`
- Row 288 `quest_trade_caravan_01`: `{"factionTag":"","id":"quest_trade_caravan_01","status":"active","synopsis":"","title":"Trade Caravan 01"}`
- Row 289 `quest_underground_fungi_01`: `{"factionTag":"","id":"quest_underground_fungi_01","status":"active","synopsis":"","title":"Underground Fungi 01"}`
- Row 290 `quest_vinyl_record_01`: `{"factionTag":"","id":"quest_vinyl_record_01","status":"active","synopsis":"","title":"Vinyl Record 01"}`
- Row 291 `quest_geological_strata_01`: `{"factionTag":"","id":"quest_geological_strata_01","status":"active","synopsis":"","title":"Geological Strata 01"}`
- Row 292 `quest_lost_tech_manual_01`: `{"factionTag":"","id":"quest_lost_tech_manual_01","status":"active","synopsis":"","title":"Lost Tech Manual 01"}`
- Row 293 `quest_currents_pamphlet_01`: `{"factionTag":"","id":"quest_currents_pamphlet_01","status":"active","synopsis":"","title":"Currents Pamphlet 01"}`
- Row 294 `quest_dead_hand_directive_01`: `{"factionTag":"","id":"quest_dead_hand_directive_01","status":"active","synopsis":"","title":"Dead Hand Directive 01"}`
- Row 295 `quest_radio_scriptbook_01`: `{"factionTag":"","id":"quest_radio_scriptbook_01","status":"active","synopsis":"","title":"Radio Scriptbook 01"}`
- Row 296 `quest_survivor_letter_01`: `{"factionTag":"","id":"quest_survivor_letter_01","status":"active","synopsis":"","title":"Survivor Letter 01"}`
- Row 297 `quest_wasteland_gazetteer_01`: `{"factionTag":"","id":"quest_wasteland_gazetteer_01","status":"active","synopsis":"","title":"Wasteland Gazetteer 01"}`
- Row 298 `quest_wasteland_expedition_01`: `{"factionTag":"","id":"quest_wasteland_expedition_01","status":"active","synopsis":"","title":"Wasteland Expedition 01"}`
- Row 299 `quest_wasteland_bestiary_01`: `{"factionTag":"","id":"quest_wasteland_bestiary_01","status":"active","synopsis":"","title":"Wasteland Bestiary 01"}`
- Row 300 `quest_regional_treaty_01`: `{"factionTag":"","id":"quest_regional_treaty_01","status":"active","synopsis":"","title":"Regional Treaty 01"}`
- Row 301 `quest_daily_survival_01`: `{"factionTag":"","id":"quest_daily_survival_01","status":"active","synopsis":"","title":"Daily Survival 01"}`
- Row 302 `quest_narrative_batch_01`: `{"factionTag":"","id":"quest_narrative_batch_01","status":"active","synopsis":"","title":"Narrative Batch 01"}`
- Row 303 `quest_found_documents_01`: `{"factionTag":"","id":"quest_found_documents_01","status":"active","synopsis":"","title":"Found Documents 01"}`
- Row 304 `quest_eulogy_corpus_01`: `{"factionTag":"","id":"quest_eulogy_corpus_01","status":"active","synopsis":"","title":"Eulogy Corpus 01"}`
- Row 305 `quest_vel_triage_log_01`: `{"factionTag":"","id":"quest_vel_triage_log_01","status":"active","synopsis":"","title":"Vel Triage Log 01"}`
- Row 306 `quest_garrison_blood_debt`: `{"factionTag":"","id":"quest_garrison_blood_debt","status":"active","synopsis":"","title":"Garrison Blood Debt"}`
- Row 307 `quest_rebuilder_seed_vault`: `{"factionTag":"","id":"quest_rebuilder_seed_vault","status":"active","synopsis":"","title":"Rebuilder Seed Vault"}`
- Row 308 `quest_ash_sign_pyre_apostasy`: `{"factionTag":"","id":"quest_ash_sign_pyre_apostasy","status":"active","synopsis":"","title":"Ash Sign Pyre Apostasy"}`
- Row 309 `quest_hydro_baron_aqueduct_sabotage`: `{"factionTag":"","id":"quest_hydro_baron_aqueduct_sabotage","status":"active","synopsis":"","title":"Hydro Baron Aqueduct Sabotage"}`
- Row 310 `quest_d9_null_stand_down`: `{"factionTag":"","id":"quest_d9_null_stand_down","status":"active","synopsis":"","title":"D9 Null Stand Down"}`
- Row 311 `quest_deep_freeze_heating_crisis`: `{"factionTag":"","id":"quest_deep_freeze_heating_crisis","status":"active","synopsis":"","title":"Deep Freeze Heating Crisis"}`
- Row 312 `quest_low_background_provenance`: `{"factionTag":"","id":"quest_low_background_provenance","status":"active","synopsis":"","title":"Low Background Provenance"}`
- Row 313 `quest_allotment_brass_treaty`: `{"factionTag":"","id":"quest_allotment_brass_treaty","status":"active","synopsis":"","title":"Allotment Brass Treaty"}`
- Row 314 `quest_black_thaw_drainage_rescue`: `{"factionTag":"","id":"quest_black_thaw_drainage_rescue","status":"active","synopsis":"","title":"Black Thaw Drainage Rescue"}`
- Row 315 `quest_radio_142_carrier_lock`: `{"factionTag":"","id":"quest_radio_142_carrier_lock","status":"active","synopsis":"","title":"Radio 142 Carrier Lock"}`
- Row 316 `quest_final_manifest_muster`: `{"factionTag":"","id":"quest_final_manifest_muster","status":"active","synopsis":"","title":"Final Manifest Muster"}`
- Row 317 `quest_railway_guild_telegraph_loop`: `{"factionTag":"","id":"quest_railway_guild_telegraph_loop","status":"active","synopsis":"","title":"Railway Guild Telegraph Loop"}`
- Row 318 `quest_penal_battalion_sanctuary_corridor`: `{"factionTag":"","id":"quest_penal_battalion_sanctuary_corridor","status":"active","synopsis":"","title":"Penal Battalion Sanctuary Corridor"}`
- Row 319 `quest_deep_salt_hospital_quarantine`: `{"factionTag":"","id":"quest_deep_salt_hospital_quarantine","status":"active","synopsis":"","title":"Deep Salt Hospital Quarantine"}`
- Row 320 `quest_supply_corps_armored_relief`: `{"factionTag":"","id":"quest_supply_corps_armored_relief","status":"active","synopsis":"","title":"Supply Corps Armored Relief"}`
- Row 321 `quest_ash_militia_switchback_redoubt`: `{"factionTag":"","id":"quest_ash_militia_switchback_redoubt","status":"active","synopsis":"","title":"Ash Militia Switchback Redoubt"}`
- Row 322 `quest_ammonium_nitrate_bomb_disarmament`: `{"factionTag":"","id":"quest_ammonium_nitrate_bomb_disarmament","status":"active","synopsis":"","title":"Ammonium Nitrate Bomb Disarmament"}`
- Row 323 `quest_mustard_gas_culvert_neutralization`: `{"factionTag":"","id":"quest_mustard_gas_culvert_neutralization","status":"active","synopsis":"","title":"Mustard Gas Culvert Neutralization"}`
- Row 324 `quest_vitrified_crater_spectrometry_core`: `{"factionTag":"","id":"quest_vitrified_crater_spectrometry_core","status":"active","synopsis":"","title":"Vitrified Crater Spectrometry Core"}`
- Row 325 `quest_salt_cavern_dynamite_treaty`: `{"factionTag":"","id":"quest_salt_cavern_dynamite_treaty","status":"active","synopsis":"","title":"Salt Cavern Dynamite Treaty"}`
- Row 326 `quest_periscope_optics_restoration`: `{"factionTag":"","id":"quest_periscope_optics_restoration","status":"active","synopsis":"","title":"Periscope Optics Restoration"}`
- Row 327 `quest_the_muster_uprising`: `{"factionTag":"","id":"quest_the_muster_uprising","status":"active","synopsis":"","title":"The Muster Uprising"}`
- Row 328 `quest_the_rate_card_war`: `{"factionTag":"","id":"quest_the_rate_card_war","status":"active","synopsis":"","title":"The Rate Card War"}`
- Row 329 `quest_the_unsigned_order`: `{"factionTag":"","id":"quest_the_unsigned_order","status":"active","synopsis":"","title":"The Unsigned Order"}`
- Row 330 `quest_four_names_on_the_roster`: `{"factionTag":"","id":"quest_four_names_on_the_roster","status":"active","synopsis":"","title":"Four Names On The Roster"}`
- Row 331 `quest_the_second_winter`: `{"factionTag":"","id":"quest_the_second_winter","status":"active","synopsis":"","title":"The Second Winter"}`
- Row 332 `quest_the_eleven_month_circuit`: `{"factionTag":"","id":"quest_the_eleven_month_circuit","status":"active","synopsis":"","title":"The Eleven Month Circuit"}`
- Row 333 `quest_the_second_color_ledger`: `{"factionTag":"","id":"quest_the_second_color_ledger","status":"active","synopsis":"","title":"The Second Color Ledger"}`
- Row 334 `quest_nothing_to_offer`: `{"factionTag":"","id":"quest_nothing_to_offer","status":"active","synopsis":"","title":"Nothing To Offer"}`
- Row 335 `quest_the_sick_of_room_seven`: `{"factionTag":"","id":"quest_the_sick_of_room_seven","status":"active","synopsis":"","title":"The Sick Of Room Seven"}`
- Row 336 `quest_the_childs_number`: `{"factionTag":"","id":"quest_the_childs_number","status":"active","synopsis":"","title":"The Childs Number"}`
- Row 337 `quest_the_signed_hour`: `{"factionTag":"","id":"quest_the_signed_hour","status":"active","synopsis":"","title":"The Signed Hour"}`
- Row 338 `quest_the_falsified_reading`: `{"factionTag":"","id":"quest_the_falsified_reading","status":"active","synopsis":"","title":"The Falsified Reading"}`
- Row 339 `quest_the_stolen_dosimeter`: `{"factionTag":"","id":"quest_the_stolen_dosimeter","status":"active","synopsis":"","title":"The Stolen Dosimeter"}`
- Row 340 `quest_child_over_the_limit`: `{"factionTag":"","id":"quest_child_over_the_limit","status":"active","synopsis":"","title":"Child Over the Limit"}`
- Row 341 `quest_the_register_audit`: `{"factionTag":"","id":"quest_the_register_audit","status":"active","synopsis":"","title":"The Register Audit"}`
- Row 342 `quest_black_market_clean_bill`: `{"factionTag":"","id":"quest_black_market_clean_bill","status":"active","synopsis":"","title":"Black-Market Clean Bill"}`
- Row 343 `quest_the_broken_calibration_chain`: `{"factionTag":"","id":"quest_the_broken_calibration_chain","status":"active","synopsis":"","title":"The Broken Calibration Chain"}`
- Row 344 `quest_exposure_for_the_essential_worker`: `{"factionTag":"","id":"quest_exposure_for_the_essential_worker","status":"active","synopsis":"","title":"Exposure for Essential Worker"}`
- Row 345 `quest_the_missing_page`: `{"factionTag":"","id":"quest_the_missing_page","status":"active","synopsis":"","title":"The Missing Page"}`
- Row 346 `quest_scavenger_alliance`: `{"factionTag":"scavengers","id":"quest_scavenger_alliance","status":"active","synopsis":"The scavengers who came to the door are not the ones who came last season, and their offer of alliance is written in the grammar of the desperate. The…`
- Row 347 `quest_farmers_bargain`: `{"factionTag":"farmers","id":"quest_farmers_bargain","status":"active","synopsis":"The growers offer fresh food for protection, and the offer is careful, because both sides know what protection becomes when it fails. The food is real. The …`
- Row 348 `quest_radio_signal`: `{"factionTag":"neutral","id":"quest_radio_signal","status":"active","synopsis":"Something is transmitting from the ruin on the ridge, on a schedule, too clean for a survivor and too weak for a station. The radio log has it twice a day. Nob…`
- Row 349 `quest_medical_supply_run`: `{"factionTag":"neutral","id":"quest_medical_supply_run","status":"active","synopsis":"The settlement down-valley is out of medicine and asked correctly, without exaggeration, which is what makes it land. The expedition is dangerous; the as…`
- Row 350 `quest_black_flotilla_trade`: `{"factionTag":"black_flotilla","id":"quest_black_flotilla_trade","status":"active","synopsis":"The Black Flotilla offers terms generous enough to be suspicious, and the suspicion is the terms. Their reputation arrives before their boats, a…`
- Row 351 `quest_exp09_sunken_submarine`: `{"factionTag":"black_flotilla","id":"quest_exp09_sunken_submarine","status":"active","synopsis":"Coordinates and a Flotilla challenge ribbon have led you to the Half-Submerged Barrik. Inside, the remains of a pre-war crew wait in the dark.…`
- Row 352 `quest_bunker_upgrade`: `{"factionTag":"neutral","id":"quest_bunker_upgrade","status":"active","synopsis":"The survivors have drafted an expansion: more beds, better facilities. The drafting is the point, because people plan for futures they intend to see. The wal…`
- Row 353 `quest_radiation_study`: `{"factionTag":"scientists","id":"quest_radiation_study","status":"active","synopsis":"Scientists from a distant settlement want to measure the radiation here, and they ask the way people with instruments ask. What they find will be written…`
- Row 354 `quest_survivor_rescue`: `{"factionTag":"neutral","id":"quest_survivor_rescue","status":"active","synopsis":"A lone figure lies injured near the perimeter fence, in camera range, in the open, for hours. Rescue means opening a door; leaving means everyone keeps watc…`
- Row 355 `quest_raider_peace_offer`: `{"factionTag":"raiders","id":"quest_raider_peace_offer","status":"active","synopsis":"The raiders offer a truce for tribute, and the offer is polite, and the politeness is arithmetic. Tribute paid once is tribute expected forever, and ever…`
- Row 356 `quest_expedition_equipment`: `{"factionTag":"neutral","id":"quest_expedition_equipment","status":"active","synopsis":"The expedition gear is failing in stages — straps, seals, soles — and the stages are logged in the injury reports. Upgrades cost; not upgrading also co…`
- Row 357 `quest_medical_triage`: `{"factionTag":"neutral","id":"quest_medical_triage","status":"active","synopsis":"Two patients, one course of treatment, one night. The medic has laid the arithmetic on the table and refuses to do it alone, which is both the right thing an…`
- Row 358 `quest_scout_training`: `{"factionTag":"neutral","id":"quest_scout_training","status":"active","synopsis":"The scouts keep coming back hurt, and the word keep is the finding. A training program: drills, routes, and the unglamorous habits that keep people alive in …`
- Row 359 `quest_food_storage_theft`: `{"factionTag":"neutral","id":"quest_food_storage_theft","status":"active","synopsis":"The stores are lighter than the ledger, three nights running. The theft is small and it is inside, and the investigation will end at a name everyone susp…`
- Row 360 `quest_medical_experiment`: `{"factionTag":"neutral","id":"quest_medical_experiment","status":"active","synopsis":"A desperate medic proposes an untested procedure for radiation sickness, and the word untested is doing a great deal of work in that sentence. The patien…`
- Row 361 `quest_survivor_leadership`: `{"factionTag":"neutral","id":"quest_survivor_leadership","status":"active","synopsis":"A charismatic survivor is asking, politely and in public, who decided what. The question is legitimate, and the legitimacy is exactly what makes it dang…`
- Row 362 `quest_raider_ambush`: `{"factionTag":"raiders","id":"quest_raider_ambush","status":"active","synopsis":"The expedition walked into an ambush on a route that was supposed to be quiet. Fight, run, or talk — the choice is being made in the open, and the raiders are…`
- Row 363 `quest_bunker_defense`: `{"factionTag":"neutral","id":"quest_bunker_defense","status":"active","synopsis":"A raid is a rumor with a direction, and the drill is how the shelter argues with it. Stations, doors, signals — practiced until the practice is boring, becau…`
- Row 364 `quest_survivor_memorial`: `{"factionTag":"neutral","id":"quest_survivor_memorial","status":"active","synopsis":"The names have accumulated faster than the ceremony. The shelter owes its dead one, and the living owe it to themselves to be present while it is paid.","…`
- Row 365 `quest_food_rationing`: `{"factionTag":"neutral","id":"quest_food_rationing","status":"active","synopsis":"Stores have dropped past the line where arithmetic becomes policy. Rationing keeps people; not rationing keeps kindness, for a while. The council must choose…`
- Row 366 `quest_technology_scavenging`: `{"factionTag":"neutral","id":"quest_technology_scavenging","status":"active","synopsis":"Pre-war technology sits in a ruin the survey flagged twice, both flags about the dose. The expedition is a cost-benefit argument where the cost is mea…`
- Row 367 `quest_medical_training`: `{"factionTag":"neutral","id":"quest_medical_training","status":"active","synopsis":"The infirmary runs on one person's knowledge, and that person sleeps. Train hands: dressings, doses, and the arithmetic of fever. The shelter's medicine sh…`
- Row 368 `quest_survivor_skills_assessment`: `{"factionTag":"neutral","id":"quest_survivor_skills_assessment","status":"active","synopsis":"Who can do what has never been written down, and the not-writing is starting to cost. Assess the survivors, put labor where labor fits, and find …`
- Row 369 `quest_bunker_medical_bay`: `{"factionTag":"neutral","id":"quest_bunker_medical_bay","status":"active","synopsis":"The medical bay is two beds and a curtain pretending to be a ward. Expansion means a second table, better light, and a door that closes. The patients des…`
- Row 370 `quest_survivor_romance`: `{"factionTag":"neutral","id":"quest_survivor_romance","status":"active","synopsis":"Two survivors have stopped being careful around each other, and the bunker — which notices everything — has started placing bets. Acknowledging or ignoring…`
- Row 371 `quest_fuel_crisis`: `{"factionTag":"neutral","id":"quest_fuel_crisis","status":"active","synopsis":"Fuel has dropped below the number where the generator starts making decisions. Scavenge and send people into the cold, or conserve and let the bunker get colder…`
- Row 372 `quest_apprentice_pipefitting`: `{"factionTag":"apprentice_arc","id":"quest_apprentice_pipefitting","narrativeHook":"apprenticeship_completion:skill_rough_repairs","status":"active","synopsis":"Eli was the only one who knew which elbow joint took packing and which one too…`
- Row 373 `quest_apprentice_dressing`: `{"factionTag":"apprentice_arc","id":"quest_apprentice_dressing","narrativeHook":"apprenticeship_completion:skill_field_dressing","status":"active","synopsis":"Sasha had the steady hands the bunker trusted with the children and the wounded.…`
- Row 374 `quest_apprentice_radio`: `{"factionTag":"apprentice_arc","id":"quest_apprentice_radio","narrativeHook":"apprenticeship_completion:skill_signal_ear","status":"active","synopsis":"The radio operator taught an apprentice the listening hour. The numbers station came in…`
- Row 375 `quest_apprentice_recycling`: `{"factionTag":"apprentice_arc","id":"quest_apprentice_recycling","narrativeHook":"apprenticeship_completion:skill_workshop_sense","status":"active","synopsis":"She was always the one who fed the forge. The apprentice learned the rhythm las…`
- Row 376 `quest_apprentice_hatch`: `{"factionTag":"apprentice_arc","id":"quest_apprentice_hatch","narrativeHook":"apprenticeship_completion:skill_watchful","status":"active","synopsis":"A child took the hatch watch alone for ninety minutes for the first time and the click of…`
- Row 377 `quest_apprentice_triage`: `{"factionTag":"apprentice_arc","id":"quest_apprentice_triage","narrativeHook":"apprenticeship_completion:skill_steady_hands","status":"active","synopsis":"The triage line at 03:00 is the one the bunker runs once a week so the new ones can …`
- Row 378 `quest_schooling_curriculum_letters`: `{"factionTag":"childhood_path","id":"quest_schooling_curriculum_letters","narrativeHook":"schooling_letters_unlock","status":"active","synopsis":"The bunker's only alphabet chart lives on a wall in the cold room. The children have to learn…`
- Row 379 `quest_schooling_curriculum_mechanics`: `{"factionTag":"childhood_path","id":"quest_schooling_curriculum_mechanics","narrativeHook":"schooling_mechanics_unlock","status":"active","synopsis":"The Mechanical Hour starts after the Reading Hour, in the same room, at the same lamp. Th…`
- Row 380 `quest_schooling_curriculum_medicine`: `{"factionTag":"childhood_path","id":"quest_schooling_curriculum_medicine","narrativeHook":"schooling_medicine_unlock","status":"active","synopsis":"A child hands the needle, a child hands the thread, and eventually a child closes the first…`
- Row 381 `quest_schooling_curriculum_marksmanship`: `{"factionTag":"childhood_path","id":"quest_schooling_curriculum_marksmanship","narrativeHook":"schooling_marksmanship_unlock","status":"active","synopsis":"There is a rifle with no ammunition, in the storeroom, on a shelf, that the childre…`
- Row 382 `quest_adoption_warmarms`: `{"factionTag":"childhood_path","id":"quest_adoption_warmarms","narrativeHook":"adoption_warmarms","status":"active","synopsis":"A casualty orphan from the winter convoy was folded into the bunker's census without ceremony. The adults who r…`
- Row 383 `quest_adoption_fierce_mother`: `{"factionTag":"childhood_path","id":"quest_adoption_fierce_mother","narrativeHook":"adoption_rotational_vigil","status":"active","synopsis":"The Feral Orphan was assigned a rotation with the Fierce Mother and the Vet, on the council sheet,…`
- Row 384 `quest_adoption_grange`: `{"factionTag":"childhood_path","id":"quest_adoption_grange","narrativeHook":"adoption_grange_plot","status":"active","synopsis":"A casualty orphan was assigned, by plot number, to a farmer couple who still held the allotment waiting list f…`
- Row 385 `quest_adoption_archive`: `{"factionTag":"childhood_path","id":"quest_adoption_archive","narrativeHook":"adoption_archive_intake","status":"active","synopsis":"The municipal archive adopted four orphans when the bunker census crossed the binder threshold for the fir…`
- Row 386 `quest_coming_of_age_first_surface`: `{"factionTag":"coming_of_age","id":"quest_coming_of_age_first_surface","narrativeHook":"trigger_maturation","status":"active","synopsis":"The bunker's hatch opened the morning of day ninety-four with a child on the perimeter walk, in clean…`
- Row 387 `quest_coming_of_age_first_watch`: `{"factionTag":"coming_of_age","id":"quest_coming_of_age_first_watch","narrativeHook":"trigger_maturation","status":"active","synopsis":"Solo watch starts at minute zero and ends when the clock says it ends. The first time a child took the …`
- Row 388 `quest_cipher_relay_count`: `{"factionTag":"sigint_exploration","id":"quest_cipher_relay_count","narrativeHook":"cipher_relay_decode","status":"active","synopsis":"An eerie five-figure shortwave broadcast loops on 104.5 MHz. Decrypting the coordinates requires Communi…`
- Row 389 `quest_cipher_winter_ledger`: `{"factionTag":"sigint_exploration","id":"quest_cipher_winter_ledger","narrativeHook":"cipher_winter_decode","status":"active","synopsis":"Automated logistics transmissions report cold-storage inventory offsets. A laminated cipher sheet rev…`
- Row 390 `quest_cipher_last_rotation`: `{"factionTag":"sigint_exploration","id":"quest_cipher_last_rotation","narrativeHook":"cipher_rotation_decode","status":"active","synopsis":"A military dead-hand standby broadcast prompts for authentication at Waypoint November. Microfilm i…`
- Row 391 `quest_waystation_alpha_filter_rebuild`: `{"factionTag":"infrastructure","id":"quest_waystation_alpha_filter_rebuild","status":"active","synopsis":"Warden Kessel reports severe coal dust clogging the primary ventilation intake at Waystation A in the Cut. Deliver clean water and fi…`
- Row 392 `quest_waystation_switchback_scree_shoring`: `{"factionTag":"infrastructure","id":"quest_waystation_switchback_scree_shoring","status":"active","synopsis":"Recent rockslides threaten to sweep the lower bunkhouse off the cliff ledge at the Switchback Waystation. Transport structural ti…`
- Row 393 `quest_waystation_span44_abutment_weld`: `{"factionTag":"infrastructure","id":"quest_waystation_span44_abutment_weld","status":"active","synopsis":"Foreman Taggart requires mechanical fasteners and welding electrodes to secure the settling western bridge pier on Railway Span 44 be…`
- Row 394 `quest_waystation_verity_pump_clear`: `{"factionTag":"infrastructure","id":"quest_waystation_verity_pump_clear","status":"active","synopsis":"Fine sand and silt have jammed the deep-aquifer impeller at the Verity Motel staging post. Clean the manifold and replace the drive belt…`
- Row 395 `quest_waystation_coast_lock_sluice_reclaim`: `{"factionTag":"infrastructure","id":"quest_waystation_coast_lock_sluice_reclaim","status":"active","synopsis":"Heavy brine scale has frozen the tidal sluice bypass at Lock Gate Four. Apply anti-corrosive solvent and grease to re-open the m…`
- Row 396 `quest_waystation_grain_verge_sentry_accord`: `{"factionTag":"infrastructure","id":"quest_waystation_grain_verge_sentry_accord","status":"active","synopsis":"Central Garrison sentries are demanding excess grain tithes at the Verge Silo Waystation. Negotiate a standardized tariff or pro…`
- Row 397 `quest_crossing_asylum_in_the_truss`: `{"id":"quest_crossing_asylum_in_the_truss"}`
- Row 398 `quest_crossing_contraband_medical_vial`: `{"id":"quest_crossing_contraband_medical_vial"}`
- Row 399 `quest_crossing_displaced_kin_roll`: `{"id":"quest_crossing_displaced_kin_roll"}`
- Row 400 `quest_crossing_embargo_transit_escort`: `{"id":"quest_crossing_embargo_transit_escort"}`
- Row 401 `quest_crossing_flotilla_docking_rights`: `{"id":"quest_crossing_flotilla_docking_rights"}`
- Row 402 `quest_crossing_quarantine_breach_trial`: `{"id":"quest_crossing_quarantine_breach_trial"}`
- Row 403 `quest_crossing_the_null_charter_vote`: `{"id":"quest_crossing_the_null_charter_vote"}`
- Row 404 `quest_crossing_vehicle_lien_arbitration`: `{"id":"quest_crossing_vehicle_lien_arbitration"}`
- Row 405 `quest_holdfast_boiler_crack_panic`: `{"id":"quest_holdfast_boiler_crack_panic"}`
- Row 406 `quest_holdfast_brine_boiler_scum`: `{"id":"quest_holdfast_brine_boiler_scum"}`
- Row 407 `quest_holdfast_brine_intake_poisoning`: `{"id":"quest_holdfast_brine_intake_poisoning"}`
- Row 408 `quest_holdfast_broken_runner_rescue`: `{"id":"quest_holdfast_broken_runner_rescue"}`
- Row 409 `quest_holdfast_census_absentee_defense`: `{"id":"quest_holdfast_census_absentee_defense"}`
- Row 410 `quest_holdfast_census_claimant_audit`: `{"id":"quest_holdfast_census_claimant_audit"}`
- Row 411 `quest_holdfast_census_estate_division`: `{"id":"quest_holdfast_census_estate_division"}`
- Row 412 `quest_holdfast_census_forged_voucher`: `{"id":"quest_holdfast_census_forged_voucher"}`
- Row 413 `quest_holdfast_estuary_water_compact`: `{"id":"quest_holdfast_estuary_water_compact"}`
- Row 414 `quest_holdfast_ration_lockup_breach`: `{"id":"quest_holdfast_ration_lockup_breach"}`
- Row 415 `quest_holdfast_rival_sled_overtake`: `{"id":"quest_holdfast_rival_sled_overtake"}`
- Row 416 `quest_holdfast_salt_convoy_haul`: `{"id":"quest_holdfast_salt_convoy_haul"}`
- Row 417 `quest_holdfast_salter_work_stoppage`: `{"id":"quest_holdfast_salter_work_stoppage"}`
- Row 418 `quest_holdfast_scree_blockage_clear`: `{"id":"quest_holdfast_scree_blockage_clear"}`
- Row 419 `quest_record_archive_burn_layer`: `{"id":"quest_record_archive_burn_layer"}`
- Row 420 `quest_record_cold_store_sublevel`: `{"id":"quest_record_cold_store_sublevel"}`
- Row 421 `quest_record_metro_derailment_triage`: `{"id":"quest_record_metro_derailment_triage"}`
- Row 422 `quest_record_mine_shaft_adit_collapse`: `{"id":"quest_record_mine_shaft_adit_collapse"}`
- Row 423 `quest_record_seed_bank_purge_trace`: `{"id":"quest_record_seed_bank_purge_trace"}`
- Row 424 `quest_record_sluice_failure_verdict`: `{"id":"quest_record_sluice_failure_verdict"}`
- Row 425 `quest_record_sub_basement_blueprint`: `{"id":"quest_record_sub_basement_blueprint"}`
- Row 426 `quest_record_the_boundary_dispute`: `{"id":"quest_record_the_boundary_dispute"}`
- Row 427 `quest_record_the_cold_survey`: `{"id":"quest_record_the_cold_survey"}`
- Row 428 `quest_record_the_lamp_keepers_oath`: `{"id":"quest_record_the_lamp_keepers_oath"}`
- Row 429 `quest_record_the_lamp_oil_ledger`: `{"id":"quest_record_the_lamp_oil_ledger"}`
- Row 430 `quest_record_the_last_sector`: `{"id":"quest_record_the_last_sector"}`
- Row 431 `quest_record_the_last_watch_beacon`: `{"id":"quest_record_the_last_watch_beacon"}`
- Row 432 `quest_record_the_missing_plate`: `{"id":"quest_record_the_missing_plate"}`
- Row 433 `quest_record_the_overlay_pigment`: `{"id":"quest_record_the_overlay_pigment"}`
- Row 434 `quest_record_the_rejected_survey`: `{"id":"quest_record_the_rejected_survey"}`
- Row 435 `quest_record_the_second_count`: `{"id":"quest_record_the_second_count"}`
- Row 436 `quest_record_the_survey_nail`: `{"id":"quest_record_the_survey_nail"}`
- Row 437 `quest_record_the_unmarked_plaque`: `{"id":"quest_record_the_unmarked_plaque"}`
- Row 438 `quest_record_transit_vent_shaft_route`: `{"id":"quest_record_transit_vent_shaft_route"}`
- Row 439 `quest_record_utility_junction_crossover`: `{"id":"quest_record_utility_junction_crossover"}`
- Row 440 `quest_record_vault_breach_forensics`: `{"id":"quest_record_vault_breach_forensics"}`
- Row 441 `quest_verdict_alibi_verification`: `{"id":"quest_verdict_alibi_verification"}`
- Row 442 `quest_verdict_chain_of_custody`: `{"id":"quest_verdict_chain_of_custody"}`
- Row 443 `quest_verdict_charter_authentication`: `{"id":"quest_verdict_charter_authentication"}`
- Row 444 `quest_verdict_eden_grabs`: `{"id":"quest_verdict_eden_grabs"}`
- Row 445 `quest_verdict_forged_evidence_inquest`: `{"id":"quest_verdict_forged_evidence_inquest"}`
- Row 446 `quest_verdict_machine_interpretation_contest`: `{"id":"quest_verdict_machine_interpretation_contest"}`
- Row 447 `quest_verdict_prior_verdict_appeal`: `{"id":"quest_verdict_prior_verdict_appeal"}`
- Row 448 `quest_verdict_reconciled_testimony`: `{"id":"quest_verdict_reconciled_testimony"}`
- Row 449 `quest_verdict_the_hold`: `{"id":"quest_verdict_the_hold"}`
- Row 450 `quest_verdict_the_mortars_timetable`: `{"id":"quest_verdict_the_mortars_timetable"}`
- Row 451 `quest_verdict_the_reckoning_call`: `{"id":"quest_verdict_the_reckoning_call"}`
- Row 452 `quest_verdict_the_shift_charter`: `{"id":"quest_verdict_the_shift_charter"}`
- Row 453 `quest_verdict_the_summons`: `{"id":"quest_verdict_the_summons"}`
- Row 454 `quest_verdict_the_tape_silo`: `{"id":"quest_verdict_the_tape_silo"}`
- Row 455 `quest_verdict_the_warm_range`: `{"id":"quest_verdict_the_warm_range"}`
- Row 456 `quest_verdict_witness_subpoena`: `{"id":"quest_verdict_witness_subpoena"}`
- Row 457 `quest_distress_trapped_mechanic`: `{"deadlineDays":3,"factionTag":"neutral","id":"quest_distress_trapped_mechanic","rewardItems":["scrap_metal","mechanical_parts"],"rewardReputation":5,"signalId":"freq_distress_88_3","status":"active","synopsis":"A distress signal on 88.3 M…`
- Row 458 `quest_distress_injured_trader`: `{"deadlineDays":2,"factionTag":"neutral","id":"quest_distress_injured_trader","rewardItems":["bandage","battery","iodine_pills"],"rewardReputation":3,"signalId":"freq_distress_156_8","status":"active","synopsis":"A distress signal on 156.8…`
- Row 459 `quest_distress_family_shelter`: `{"deadlineDays":2,"factionTag":"neutral","id":"quest_distress_family_shelter","rewardItems":["clean_water","canned_food"],"rewardReputation":8,"signalId":"freq_distress_445_2","status":"active","synopsis":"A child's voice on 445.2 MHz repo…`
- Row 460 `quest_distress_raider_trap`: `{"deadlineDays":3,"factionTag":"raiders","id":"quest_distress_raider_trap","rewardItems":[],"rewardReputation":0,"signalId":"freq_distress_192_4","status":"active","synopsis":"A suspicious signal on 192.4 MHz mentions an unguarded fuel cac…`
- Row 461 `quest_distress_military_patrol`: `{"deadlineDays":3,"factionTag":"military","id":"quest_distress_military_patrol","rewardItems":["ammo_556","field_dressing_kit"],"rewardReputation":10,"signalId":"freq_distress_901_2","status":"active","synopsis":"A military distress signal…`
- Row 462 `quest_arc_mara_01_waystation`: `{"factionTag":"faction_grain_exchange","id":"quest_arc_mara_01_waystation","status":"active","synopsis":"A caravan factor working the water stations is short after a bad crossing. What you do about it is the first entry in her ledger.","ti…`
- Row 463 `quest_arc_mara_02_route`: `{"factionTag":"faction_grain_exchange","id":"quest_arc_mara_02_route","status":"active","synopsis":"The caravan factor's second wagon is through the road crust, and she is camped on the wreck guarding the cargo.","title":"Wreckage on the C…`
- Row 464 `quest_arc_mara_03_office`: `{"factionTag":"faction_grain_exchange","id":"quest_arc_mara_03_office","status":"active","synopsis":"The grain exchange has given Mara Veln the southern routes. The rate sheet has a line for friends of the coordinator.","title":"The Coordi…`
- Row 465 `quest_arc_ilze_01_clinic`: `{"factionTag":"none","id":"quest_arc_ilze_01_clinic","status":"active","synopsis":"An almshouse clinic is down to its last shelves; its physician shows the inventory before asking for anything.","title":"Eight Beds"}`
- Row 466 `quest_arc_ilze_02_outbreak`: `{"factionTag":"none","id":"quest_arc_ilze_02_outbreak","status":"active","synopsis":"The clinic is full of a fever she can name but not stop, and the camp is one rumor from a cordon.","title":"The Fever Finds the Almshouse"}`
- Row 467 `quest_arc_ilze_03_clinic`: `{"factionTag":"none","id":"quest_arc_ilze_03_clinic","status":"active","synopsis":"The clinic trains more hands than it loses now; the verge sends its sick to a door never shut at the appointed hour.","title":"The Door That Stays Open"}`
- Row 468 `quest_arc_marek_01_patrol`: `{"factionTag":"faction_central_garrison","id":"quest_arc_marek_01_patrol","status":"active","synopsis":"A garrison checkpoint runs by the book, and the corporal keeps the book where petitioners can read it.","title":"Checkpoint Gamma"}`
- Row 469 `quest_arc_marek_02_deserter`: `{"factionTag":"faction_central_garrison","id":"quest_arc_marek_02_deserter","status":"active","synopsis":"The corporal who wrote dated objections to his own orders is now the subject of them.","title":"The Margin Notes"}`
- Row 470 `quest_arc_marek_03_watch`: `{"factionTag":"faction_central_garrison","id":"quest_arc_marek_03_watch","status":"active","synopsis":"An outside ally sends warning before every sweep: routes, timing, the mood of the command.","title":"Word Before the Sweeps"}`
- Row 471 `quest_arc_lina_01_found`: `{"factionTag":"none","id":"quest_arc_lina_01_found","status":"active","synopsis":"A child keeps order in a queue at the baths, and her count is better than yours.","title":"The Girl Who Counts"}`
- Row 472 `quest_arc_lina_02_lessons`: `{"factionTag":"none","id":"quest_arc_lina_02_lessons","status":"active","synopsis":"The shelter's foundling sits the lessons it gives and invents the ones it will not.","title":"Lessons Twice Given"}`
- Row 473 `quest_arc_lina_03_apprentice`: `{"factionTag":"none","id":"quest_arc_lina_03_apprentice","status":"active","synopsis":"The counting becomes a trade instead of a defense: medic's apprentice, roster, and all.","title":"Small Hands, Steady Ones"}`
- Row 474 `quest_arc_anete_01_substation`: `{"factionTag":"faction_rebuilders","id":"quest_arc_anete_01_substation","status":"active","synopsis":"A grid engineer holds a live substation with working grounds and no copper to speak of.","title":"Live Grounds, No Copper"}`
- Row 475 `quest_arc_anete_02_project`: `{"factionTag":"faction_rebuilders","id":"quest_arc_anete_02_project","status":"active","synopsis":"A regional recovery project wants her name on it and her switching orders behind it.","title":"One Feeder at a Time"}`
- Row 476 `quest_arc_anete_03_grid`: `{"factionTag":"faction_rebuilders","id":"quest_arc_anete_03_grid","status":"active","synopsis":"The grid is small, documented, and true: two feeders, a schedule, and switching orders done safely.","title":"The Region On Schedule"}`
- Row 477 `quest_arc_sava_01_camp`: `{"factionTag":"cult_of_the_glow","id":"quest_arc_sava_01_camp","status":"active","synopsis":"A refugee camp keeps its hours, and raiders have begun keeping watch on it.","title":"The Hours of the Camp"}`
- Row 478 `quest_arc_sava_02_schism`: `{"factionTag":"cult_of_the_glow","id":"quest_arc_sava_02_schism","status":"active","synopsis":"The camps quarrel over the right way to keep the hours, and both sides quote her.","title":"The Rota and the Schism"}`
- Row 479 `quest_arc_sava_03_hours`: `{"factionTag":"cult_of_the_glow","id":"quest_arc_sava_03_hours","status":"active","synopsis":"When the camps quarrel, they send for her before they send for anyone with a rifle.","title":"The Verdict-Free Hour"}`
- Row 480 `quest_arc_rika_01_cache`: `{"factionTag":"faction_scavenger_guild","id":"quest_arc_rika_01_cache","status":"active","synopsis":"A scavenger knows which cellar floors are real, and offers to split a find rather than race.","title":"The Real Floors"}`
- Row 481 `quest_arc_rika_02_trap`: `{"factionTag":"faction_scavenger_guild","id":"quest_arc_rika_02_trap","status":"active","synopsis":"A cellar floor that was a rumor took her ankle, and she is camp-bound with a registry full of addresses.","title":"A Rumor With a Drop Shaf…`
- Row 482 `quest_arc_rika_03_guild`: `{"factionTag":"faction_scavenger_guild","id":"quest_arc_rika_03_guild","status":"active","synopsis":"The guild gave her the cellar registry, and the registry has your name in the margin.","title":"First Refusal"}`
- Row 483 `quest_arc_liva_01_tower`: `{"factionTag":"none","id":"quest_arc_liva_01_tower","status":"active","synopsis":"A relay mast lives on scavenged cells and inherited discipline, and its operator answers on schedule.","title":"The Station That Answers"}`
- Row 484 `quest_arc_liva_02_seizure`: `{"factionTag":"none","id":"quest_arc_liva_02_seizure","status":"active","synopsis":"The faction that maps towers has mapped hers, and their survey team is two valleys out.","title":"Somebody Maps the Towers"}`
- Row 485 `quest_arc_liva_03_signal`: `{"factionTag":"none","id":"quest_arc_liva_03_signal","status":"active","synopsis":"A station that answers on schedule, under a flag that leaves it alone, with a band of its own.","title":"Fixed Hours"}`
- Row 486 `quest_arc_oskar_01_debt`: `{"factionTag":"faction_the_underwrite","id":"quest_arc_oskar_01_debt","status":"active","synopsis":"Half a wagon and a whole debt, both priced daily. The buyout figure exists and is written down. The only question is whose hand moves first…`
- Row 487 `quest_arc_tomas_01_rounding`: `{"factionTag":"faction_central_garrison","id":"quest_arc_tomas_01_rounding","status":"active","synopsis":"The overflow clinic's stock does not reconcile with the garrison's ledgers, and the difference keeps matching a list of people who ar…`
- Row 488 `quest_arc_joren_01_plant`: `{"factionTag":"faction_silent_foundry","id":"quest_arc_joren_01_plant","status":"active","synopsis":"The batching plant restart is real, the schedule is honest, and the schedule does not care what happened to the night shift. He needs crew…`
- Row 489 `quest_arc_pavel_01_map`: `{"factionTag":"none","id":"quest_arc_pavel_01_map","status":"active","synopsis":"The route scavenger sells only what he has walked, and the best route in the roll is the one he learned the impolite way. The copy costs less than the questio…`
- Row 490 `quest_arc_sena_01_log`: `{"factionTag":"faction_ash_militia","id":"quest_arc_sena_01_log","status":"active","synopsis":"The bridge corridor's crossing log is kept in full view, and the incident reports come back in two versions: what she filed and what command ack…`
- Row 491 `quest_arc_dalia_01_annex`: `{"factionTag":"faction_salt_freeholders","id":"quest_arc_dalia_01_annex","status":"active","synopsis":"The seed annex holds tested lots and one roof of uncertain honesty. She will not leave it and she will not hurry it, and the private tin…`
- Row 492 `quest_arc_anton_01_herd`: `{"factionTag":"none","id":"quest_arc_anton_01_herd","status":"active","synopsis":"The herd ledger and the herd disagree by exactly the animals a hard winter eats, and the well has been dosed since before anyone asked. What the herd needs i…`
- Row 493 `quest_arc_emil_01_numbers`: `{"factionTag":"none","id":"quest_arc_emil_01_numbers","status":"active","synopsis":"Four notebooks of repeats, every published solution refuted in writing including his own, and a standing rule that patrons get warned before they get taken…`
- Row 494 `quest_arc_nadia_01_pot`: `{"factionTag":"none","id":"quest_arc_nadia_01_pot","status":"active","synopsis":"The field kitchen's pot is honest and the manifest is not, and she is the difference. The portion ledger disagrees with the pot by exactly the amount the chil…`
- Row 495 `quest_arc_arvo_01_crate`: `{"factionTag":"faction_the_provisioned","id":"quest_arc_arvo_01_crate","status":"active","synopsis":"The store mess runs institutional-exact under a cook with one private crate of good oil and an argument with himself that has been going s…`
- Row 496 `quest_arc_kaspar_01_bearing`: `{"factionTag":"faction_long_walk","id":"quest_arc_kaspar_01_bearing","status":"active","synopsis":"The wagon mechanic keeps one vehicle alive past decency and tells you the odds on every road like a man reading weather. The bearing in his …`
- Row 497 `quest_arc_mira_01_intervals`: `{"factionTag":"faction_the_cutters","id":"quest_arc_mira_01_intervals","status":"active","synopsis":"The generator runs to stated hours and the private log runs to stated truth: a missed maintenance interval in her own handwriting, kept so…`
- Row 498 `quest_arc_janek_01_dawn`: `{"factionTag":"none","id":"quest_arc_janek_01_dawn","status":"active","synopsis":"The old hunting lines fed the settlement for two winters and have stopped feeding it, and the hunter will hear no word against them before dawn. At dawn, the…`
- Row 499 `quest_arc_veda_01_circuit`: `{"factionTag":"none","id":"quest_arc_veda_01_circuit","status":"active","synopsis":"The trap circuit is marked, posted, and paid for, once, in the way that taught her. The standing fee for the maimed still stands. The fog month is a closed…`
- Row 500 `quest_arc_mirael_01_page`: `{"factionTag":"faction_archivists","id":"quest_arc_mirael_01_page","status":"active","synopsis":"The archive's copy lessons are exact and one page of the community record is in a different hand than the rest. It sits face-up on the desk th…`
- Row 501 `quest_arc_niko_01_corridors`: `{"factionTag":"none","id":"quest_arc_niko_01_corridors","status":"active","synopsis":"The runner knows the corridors grown-ups cannot stand up in, and has decided your column watches. The routes have never been written down, because paper …`
- Row 502 `quest_crossing_the_salvaged_accord`: `{"factionTag":"none","id":"quest_crossing_the_salvaged_accord","status":"active","synopsis":"A disputed covenant over river transit rights resurfaces when a salvage crew claims prior agreement over the same crossing lane. Resolve who holds…`
- Row 503 `quest_crossing_the_registry_dispute`: `{"factionTag":"none","id":"quest_crossing_the_registry_dispute","status":"active","synopsis":"Two factions claim the same entry in the passage ledger. The crossing administrator needs an outside arbitration before trade can resume through …`
- Row 504 `quest_crossing_the_long_toll`: `{"factionTag":"none","id":"quest_crossing_the_long_toll","status":"active","synopsis":"The toll keeper is collecting more than the standard crossing fee. Survivors must decide whether to challenge the practice or find a route that avoids t…`
- Row 505 `quest_distress_hostage_call`: `{"deadlineDays":4,"factionTag":"neutral","id":"quest_distress_hostage_call","rewardItems":["canned_food","clean_water"],"rewardReputation":8,"signalId":"freq_distress_726_5","status":"active","synopsis":"A hostage on 726.5 MHz keys the set…`
- Row 506 `quest_distress_infected_survivor`: `{"deadlineDays":5,"factionTag":"neutral","id":"quest_distress_infected_survivor","rewardItems":["antibiotics","clean_water"],"rewardReputation":10,"signalId":"freq_distress_609_4","status":"active","synopsis":"A warden on 609.4 MHz has sea…`
- Row 507 `quest_distress_convoy_sos`: `{"deadlineDays":4,"factionTag":"neutral","id":"quest_distress_convoy_sos","rewardItems":["mechanical_parts","battery"],"rewardReputation":6,"signalId":"freq_distress_455_7","status":"active","synopsis":"A three-person salvage crew on 455.7…`
- Row 508 `quest_distress_ransom_demand`: `{"deadlineDays":4,"factionTag":"river_nomads","id":"quest_distress_ransom_demand","rewardItems":["clean_water","bandage"],"rewardReputation":8,"signalId":"freq_distress_555_0","status":"active","synopsis":"Holders on 555.0 MHz demand a tol…`
- Row 509 `quest_distress_false_evacuation`: `{"deadlineDays":3,"factionTag":"raiders","id":"quest_distress_false_evacuation","rewardItems":[],"rewardReputation":0,"signalId":"freq_distress_380_2","status":"active","synopsis":"A civil-defense directive on 380.2 MHz orders survivors to…`
- Row 510 `quest_distress_military_beacon`: `{"deadlineDays":5,"factionTag":"military","id":"quest_distress_military_beacon","rewardItems":["iodine_pills","battery"],"rewardReputation":12,"signalId":"freq_distress_318_0","status":"active","synopsis":"An automated beacon on 318.0 MHz …`
- Row 511 `quest_distress_winter_crossing`: `{"deadlineDays":4,"factionTag":"neutral","id":"quest_distress_winter_crossing","rewardItems":["clean_water","bandage"],"rewardReputation":7,"signalId":"freq_distress_269_3","status":"active","synopsis":"A salt-route courier on 269.3 MHz is…`


# Appendix — Current Source Detail: `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs`

### `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` — complete current file

- Size: 432 lines / 19009 bytes.
- SHA-256: `efb9ca627d7070493ffaf6cbbae608c3835f66b3fd489e0fe52f6e101ea5bf90`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005: using System.Linq;
00006:
00007: namespace Ashfall.Core.YearOfAsh
00008: {
00009:     // ─────────────────────────────────────────────────────────────────────────────
00010:     //  DATA CONTRACTS  (serializable via IJsonSerializer — no engine references)
00011:     // ─────────────────────────────────────────────────────────────────────────────
00012:
00013:     [Serializable]
00014:     public enum QuestlineStatus
00015:     {
00016:         NotStarted,
00017:         Active,
00018:         Completed,
00019:         Failed,
00020:         Abandoned
00021:     }
00022:
00023:     [Serializable]
00024:     public class QuestCondition
00025:     {
00026:         /// <summary>Arbitrary tag the host evaluates (e.g. "faction_rebuilders_standing>=30").</summary>
00027:         public string conditionTag = string.Empty;
00028:         public bool isBlocker = false;  // false = soft warning, true = hard gate
00029:     }
00030:
00031:     [Serializable]
00032:     public class QuestChoice
00033:     {
00034:         public string choiceId   = string.Empty;
00035:         public string text       = string.Empty;
00036:
00037:         // Where this choice leads in the questline graph
00038:         public string nextStageId = string.Empty;   // empty = quest ends
00039:
00040:         // Rewards / penalties applied when this choice is taken
00041:         public int moraleDelta   = 0;
00042:         public int guiltDelta    = 0;
00043:         public string grantItemId = string.Empty;    // empty = no item
00044:         public int grantItemQuantity = 0;
00045:         public string targetFactionId   = string.Empty;
00046:         public int factionStandingDelta = 0;
00047:
00048:         // Optional: unlocks a door-encounter later
00049:         public string unlockEncounterId = string.Empty;
00050:
00051:         public List<QuestCondition> conditions = new List<QuestCondition>();
00052:
00053:         /// <summary>Flavour text shown after resolution. Tone: cold, restrained.</summary>
00054:         public string outcomeNarrative = string.Empty;
00055:     }
00056:
00057:     [Serializable]
00058:     public class QuestStage
00059:     {
00060:         public string stageId          = string.Empty;
00061:         public string title            = string.Empty;
00062:         public string narrativePrompt  = string.Empty;  // What the player sees
00063:         public int    unlockOnDay      = 0;             // Day >= this to surface
00064:         public bool   isTerminal       = false;         // true = no further choices
00065:         public QuestlineStatus terminalOutcome = QuestlineStatus.Completed;
00066:
00067:         public List<QuestChoice> choices = new List<QuestChoice>();
00068:     }
00069:
00070:     [Serializable]
00071:     public class QuestlineDefinition
00072:     {
00073:         public string questlineId  = string.Empty;
00074:         public string title        = string.Empty;
00075:         public string synopsis     = string.Empty;
00076:         public string factionTag   = string.Empty;     // primary faction context
00077:         public string firstStageId = string.Empty;
00078:         public int    minDay       = 180;
00079:         public int    maxDay       = 360;
00080:
00081:         public List<QuestStage> stages = new List<QuestStage>();
00082:
00083:         public QuestStage? FindStage(string id)
00084:         {
00085:             foreach (var s in stages)
00086:                 if (s.stageId == id) return s;
00087:             return null;
00088:         }
00089:     }
00090:
00091:     [Serializable]
00092:     public class ActiveQuestlineRecord
00093:     {
00094:         public string questlineId     = string.Empty;
00095:         public string currentStageId  = string.Empty;
00096:         public QuestlineStatus status = QuestlineStatus.Active;
00097:         public List<string> choiceHistory = new List<string>();   // ordered choice IDs taken
00098:         public int dayStarted = 0;
00099:         public int dayResolved = -1;
00100:     }
00101:
00102:     [Serializable]
00103:     public class QuestlineSystemState
00104:     {
00105:         public List<ActiveQuestlineRecord> active = new List<ActiveQuestlineRecord>();
00106:         public List<string> completedQuestlineIds = new List<string>();
00107:         public List<string> failedQuestlineIds    = new List<string>();
00108:         public int totalMoraleDeltaFromQuests     = 0;
00109:         public int totalGuiltDeltaFromQuests      = 0;
00110:     }
00111:
00112:     // ─────────────────────────────────────────────────────────────────────────────
00113:     //  RESOLUTION RESULT
00114:     // ─────────────────────────────────────────────────────────────────────────────
00115:
00116:     public class QuestChoiceResult
00117:     {
00118:         public string questlineId    = string.Empty;
00119:         public string stageId        = string.Empty;
00120:         public string choiceId       = string.Empty;
00121:         public string nextStageId    = string.Empty;
00122:         public int    moraleDelta    = 0;
00123:         public int    guiltDelta     = 0;
00124:         public string grantItemId    = string.Empty;
00125:         public int    grantItemQty   = 0;
00126:         public string factionId      = string.Empty;
00127:         public int    factionDelta   = 0;
00128:         public string unlockedEncounterId = string.Empty;
00129:         public string outcomeNarrative    = string.Empty;
00130:         public QuestlineStatus newQuestStatus = QuestlineStatus.Active;
00131:     }
00132:
00133:     // ─────────────────────────────────────────────────────────────────────────────
00134:     //  SYSTEM
00135:     // ─────────────────────────────────────────────────────────────────────────────
00136:
00137:     /// <summary>
00138:     /// Engine-agnostic branching questline manager for Days 180–360.
00139:     /// Maintains a directed graph of stages per questline; all state is serializable.
00140:     /// Zero references to UnityEngine or Godot.
00141:     /// </summary>
00142:     public class QuestlineSystem
00143:     {
00144:         private readonly QuestlineSystemState _state;
00145:         private readonly List<QuestlineDefinition> _catalog = new List<QuestlineDefinition>();
00146:
00147:         public QuestlineSystemState State => _state;
00148:         public IReadOnlyList<QuestlineDefinition> Catalog => _catalog;
00149:
00150:         // Events — host layers subscribe to drive UI, audio, journal
00151:         public event Action<QuestlineDefinition>  OnQuestlineStarted;
00152:         public event Action<QuestChoiceResult>    OnQuestChoiceTaken;
00153:         public event Action<string, QuestlineStatus> OnQuestlineResolved;
00154:
00155:         public QuestlineSystem(QuestlineSystemState? state = null)
00156:         {
00157:             _state = state ?? new QuestlineSystemState();
00158:             if (_state.active == null) _state.active = new List<ActiveQuestlineRecord>();
00159:             if (_state.completedQuestlineIds == null) _state.completedQuestlineIds = new List<string>();
00160:             if (_state.failedQuestlineIds == null) _state.failedQuestlineIds = new List<string>();
00161:             PopulateBuiltInCatalog();
00162:         }
00163:
00164:         // ── Catalog management ──────────────────────────────────────────────────
00165:
00166:         public void RegisterQuestline(QuestlineDefinition def)
00167:         {
00168:             if (def != null && !_catalog.Exists(q => q.questlineId == def.questlineId))
00169:                 _catalog.Add(def);
00170:         }
00171:
00172:         public QuestlineDefinition? FindDefinition(string questlineId)
00173:         {
00174:             foreach (var q in _catalog)
00175:                 if (q.questlineId == questlineId) return q;
00176:             return null;
00177:         }
00178:
00179:         // ── Lifecycle ───────────────────────────────────────────────────────────
00180:
00181:         /// <summary>
00182:         /// Offer all questlines whose unlock window contains <paramref name="currentDay"/>
00183:         /// and that have not yet been started or completed.
00184:         /// </summary>
00185:         public List<QuestlineDefinition> GetAvailableQuestlines(int currentDay)
00186:         {
00187:             var result = new List<QuestlineDefinition>();
00188:             foreach (var def in _catalog)
00189:             {
00190:                 if (currentDay < def.minDay || currentDay > def.maxDay) continue;
00191:                 bool alreadyDone =
00192:                     _state.completedQuestlineIds.Contains(def.questlineId) ||
00193:                     _state.failedQuestlineIds.Contains(def.questlineId) ||
00194:                     _state.active.Exists(a => a.questlineId == def.questlineId);
00195:                 if (!alreadyDone)
00196:                     result.Add(def);
00197:             }
00198:             return result;
00199:         }
00200:
00201:         /// <summary>
00202:         /// True when a questline can actually be traversed: its first stage exists and
00203:         /// offers at least one choice. A definition that fails this can be started but
00204:         /// never advanced — <see cref="TakeChoice"/> finds no matching choice and returns
00205:         /// null, stranding the record in <see cref="QuestlineStatus.Active"/> forever.
00206:         /// The JSON catalog shape (stageIndex/objective/requiredItemId) carries no
00207:         /// choices, so every questline loaded from it fails this until choices are
00208:         /// authored. Hosts offer <see cref="GetPlayableQuestlines"/>, not the raw list.
00209:         /// </summary>
00210:         public bool IsPlayable(QuestlineDefinition def)
00211:         {
00212:             if (def == null) return false;
00213:             var first = def.FindStage(def.firstStageId);
00214:             return first != null && first.choices.Count > 0;
00215:         }
00216:
00217:         /// <summary>
00218:         /// <see cref="GetAvailableQuestlines"/> minus the ones that cannot be advanced.
00219:         /// This is what a host should offer the player.
00220:         /// </summary>
00221:         public List<QuestlineDefinition> GetPlayableQuestlines(int currentDay)
00222:         {
00223:             var result = new List<QuestlineDefinition>();
00224:             foreach (var def in GetAvailableQuestlines(currentDay))
00225:                 if (IsPlayable(def)) result.Add(def);
00226:             return result;
00227:         }
00228:
00229:         /// <summary>
00230:         /// How many otherwise-available questlines were withheld for having no authored
00231:         /// choices. Hosts surface this so the content gap stays visible instead of the
00232:         /// catalog silently looking smaller than it is.
00233:         /// </summary>
00234:         public int WithheldQuestlineCount(int currentDay)
00235:         {
00236:             int withheld = 0;
00237:             foreach (var def in GetAvailableQuestlines(currentDay))
00238:                 if (!IsPlayable(def)) withheld++;
00239:             return withheld;
00240:         }
00241:
00242:         /// <summary>Starts a questline on <paramref name="day"/>. No-op if already active.</summary>
00243:         public bool StartQuestline(string questlineId, int day)
00244:         {
00245:             var def = FindDefinition(questlineId);
00246:             if (def == null) return false;
00247:             if (_state.active.Exists(a => a.questlineId == questlineId)) return false;
00248:
00249:             var record = new ActiveQuestlineRecord
00250:             {
00251:                 questlineId    = questlineId,
00252:                 currentStageId = def.firstStageId,
00253:                 status         = QuestlineStatus.Active,
00254:                 dayStarted     = day
00255:             };
00256:             _state.active.Add(record);
00257:             OnQuestlineStarted?.Invoke(def);
00258:             return true;
00259:         }
00260:
00261:         /// <summary>
00262:         /// Player picks a choice in the current stage of an active questline.
00263:         /// Returns null if questline not found or choice invalid.
00264:         /// </summary>
00265:         public QuestChoiceResult? TakeChoice(string questlineId, string choiceId, int day)
00266:         {
00267:             var record = _state.active.Find(a => a.questlineId == questlineId);
00268:             if (record == null || record.status != QuestlineStatus.Active) return null;
00269:
00270:             var def   = FindDefinition(questlineId);
00271:             if (def == null) return null;
00272:
00273:             var stage = def.FindStage(record.currentStageId);
00274:             if (stage == null) return null;
00275:
00276:             QuestChoice? choice = null;
00277:             foreach (var c in stage.choices)
00278:                 if (c.choiceId == choiceId) { choice = c; break; }
00279:             if (choice == null) return null;
00280:
00281:             // Build result
00282:             var result = new QuestChoiceResult
00283:             {
00284:                 questlineId  = questlineId,
00285:                 stageId      = stage.stageId,
00286:                 choiceId     = choiceId,
00287:                 nextStageId  = choice.nextStageId,
00288:                 moraleDelta  = choice.moraleDelta,
00289:                 guiltDelta   = choice.guiltDelta,
00290:                 grantItemId  = choice.grantItemId,
00291:                 grantItemQty = choice.grantItemQuantity,
00292:                 factionId    = choice.targetFactionId,
00293:                 factionDelta = choice.factionStandingDelta,
00294:                 unlockedEncounterId = choice.unlockEncounterId,
00295:                 outcomeNarrative    = choice.outcomeNarrative
00296:             };
00297:
00298:             // Persist history
00299:             record.choiceHistory.Add(choiceId);
00300:             _state.totalMoraleDeltaFromQuests += choice.moraleDelta;
00301:             _state.totalGuiltDeltaFromQuests  += choice.guiltDelta;
00302:
00303:             // Advance to next stage or terminal
00304:             if (string.IsNullOrEmpty(choice.nextStageId))
00305:             {
00306:                 // Questline ends — derive outcome from current stage terminal flag
00307:                 var outcome = stage.isTerminal ? stage.terminalOutcome : QuestlineStatus.Completed;
00308:                 result.newQuestStatus = outcome;
00309:                 FinalizeQuestline(record, outcome, day);
00310:             }
00311:             else
00312:             {
00313:                 var nextStage = def.FindStage(choice.nextStageId);
00314:                 if (nextStage == null || nextStage.isTerminal)
00315:                 {
00316:                     var outcome = nextStage?.terminalOutcome ?? QuestlineStatus.Completed;
00317:                     record.currentStageId = choice.nextStageId;
00318:                     result.newQuestStatus = outcome;
00319:                     FinalizeQuestline(record, outcome, day);
00320:                 }
00321:                 else
00322:                 {
00323:                     record.currentStageId = choice.nextStageId;
00324:                     result.newQuestStatus = QuestlineStatus.Active;
00325:                 }
00326:             }
00327:
00328:             OnQuestChoiceTaken?.Invoke(result);
00329:             return result;
00330:         }
00331:
00332:         public ActiveQuestlineRecord? GetActiveRecord(string questlineId)
00333:         {
00334:             return _state.active.Find(a => a.questlineId == questlineId);
00335:         }
00336:
00337:         // ── State capture ───────────────────────────────────────────────────────
00338:
00339:         public QuestlineSystemState CaptureState()
00340:         {
00341:             var copy = new QuestlineSystemState
00342:             {
00343:                 totalMoraleDeltaFromQuests = _state.totalMoraleDeltaFromQuests,
00344:                 totalGuiltDeltaFromQuests  = _state.totalGuiltDeltaFromQuests,
00345:                 completedQuestlineIds = new List<string>(_state.completedQuestlineIds),
00346:                 failedQuestlineIds    = new List<string>(_state.failedQuestlineIds),
00347:                 active = new List<ActiveQuestlineRecord>()
00348:             };
00349:             foreach (var r in _state.active)
00350:             {
00351:                 copy.active.Add(new ActiveQuestlineRecord
00352:                 {
00353:                     questlineId    = r.questlineId,
00354:                     currentStageId = r.currentStageId,
00355:                     status         = r.status,
00356:                     dayStarted     = r.dayStarted,
00357:                     dayResolved    = r.dayResolved,
00358:                     choiceHistory  = new List<string>(r.choiceHistory)
00359:                 });
00360:             }
00361:             return copy;
00362:         }
00363:
00364:         /// <summary>
00365:         /// Rebuilds live questline progress from a snapshot. Deep-copies like its
00366:         /// siblings so the restored system never aliases the save object, and
00367:         /// tolerates a null section (a save written before quests were persisted).
00368:         /// </summary>
00369:         public void RestoreState(QuestlineSystemState state)
00370:         {
00371:             if (state == null) return;
00372:
00373:             _state.totalMoraleDeltaFromQuests = state.totalMoraleDeltaFromQuests;
00374:             _state.totalGuiltDeltaFromQuests  = state.totalGuiltDeltaFromQuests;
00375:
00376:             _state.completedQuestlineIds.Clear();
00377:             if (state.completedQuestlineIds != null)
00378:                 _state.completedQuestlineIds.AddRange(state.completedQuestlineIds);
00379:
00380:             _state.failedQuestlineIds.Clear();
00381:             if (state.failedQuestlineIds != null)
00382:                 _state.failedQuestlineIds.AddRange(state.failedQuestlineIds);
00383:
00384:             _state.active.Clear();
00385:             if (state.active != null)
00386:             {
00387:                 foreach (var r in state.active)
00388:                 {
00389:                     if (r == null || string.IsNullOrEmpty(r.questlineId)) continue;
00390:                     _state.active.Add(new ActiveQuestlineRecord
00391:                     {
00392:                         questlineId    = r.questlineId,
00393:                         currentStageId = r.currentStageId,
00394:                         status         = r.status,
00395:                         dayStarted     = r.dayStarted,
00396:                         dayResolved    = r.dayResolved,
00397:                         choiceHistory  = r.choiceHistory != null
00398:                             ? new List<string>(r.choiceHistory)
00399:                             : new List<string>()
00400:                     });
00401:                 }
00402:             }
00403:         }
00404:
00405:         // ── Private ─────────────────────────────────────────────────────────────
00406:
00407:         private void FinalizeQuestline(ActiveQuestlineRecord record, QuestlineStatus outcome, int day)
00408:         {
00409:             record.status      = outcome;
00410:             record.dayResolved = day;
00411:
00412:             if (outcome == QuestlineStatus.Completed)
00413:                 _state.completedQuestlineIds.Add(record.questlineId);
00414:             else if (outcome == QuestlineStatus.Failed)
00415:                 _state.failedQuestlineIds.Add(record.questlineId);
00416:
00417:             OnQuestlineResolved?.Invoke(record.questlineId, outcome);
00418:         }
00419:
00420:         /// <summary>
00421:         /// Built-in questlines covering all 5 factions across Days 180–360.
00422:         /// Narrative tone: cold, exhausted, human. No magic, no real-world nations.
00423:         /// </summary>
00424:         private void PopulateBuiltInCatalog()
00425:         {
00426:             foreach (var q in BuiltInQuestlineCatalog.CreateAll())
00427:             {
00428:                 RegisterQuestline(q);
00429:             }
00430:         }
00431:     }
00432: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/YearOfAsh/DynamicQuestlineCatalogLoader.cs`

### `Assets/Ashfall.Core/YearOfAsh/DynamicQuestlineCatalogLoader.cs` — complete current file

- Size: 62 lines / 2286 bytes.
- SHA-256: `2a7054663163429068e4765faf91dba6001ba3b1ee0acf9e68eb024b40cc4cbe`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Ashfall.Core.IO;
00005:
00006: namespace Ashfall.Core.YearOfAsh
00007: {
00008:     /// <summary>
00009:     /// Plan 59 — loader for <c>dynamic_questlines.json</c> (the shelter campaign
00010:     /// questline catalog). Parses the same <see cref="YearOfAshQuestContainer"/>
00011:     /// choice-graph schema the runtime already consumes from
00012:     /// <c>year_of_ash_questlines.json</c> and registers each questline into the
00013:     /// supplied <see cref="QuestlineSystem"/> via its existing
00014:     /// <c>RegisterQuestline</c> API.
00015:     ///
00016:     /// Justification (Plan 59 §72): the file existed with a schema no loader
00017:     /// consumed; this adds typed reference resolution only — no new quest
00018:     /// logic, stage interpreter, or reward engine.
00019:     /// </summary>
00020:     public static class DynamicQuestlineCatalogLoader
00021:     {
00022:         public const string FileName = "dynamic_questlines.json";
00023:
00024:         public static int LoadAndRegister(
00025:             QuestlineSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json)
00026:         {
00027:             if (system == null || fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
00028:                 return 0;
00029:
00030:             string path = fileIO.Combine(dataDir, FileName);
00031:             if (!fileIO.FileExists(path))
00032:                 return 0;
00033:
00034:             string raw = fileIO.ReadAllText(path);
00035:             if (string.IsNullOrWhiteSpace(raw))
00036:                 return 0;
00037:
00038:             List<QuestlineDefinition> quests;
00039:             try
00040:             {
00041:                 var container = json.Deserialize<YearOfAshQuestContainer>(raw);
00042:                 if (container == null || container.quests == null || container.quests.Count == 0)
00043:                     return 0;
00044:                 quests = container.quests;
00045:             }
00046:             catch (Exception ex_CATDIAG)
00047:             {
00048:                 CatalogDiagnostics.Warn(path, "YearOfAshQuestContainer (dynamic questlines)", ex_CATDIAG);
00049:                 return 0;
00050:             }
00051:
00052:             int count = 0;
00053:             foreach (var q in quests)
00054:             {
00055:                 if (q == null || string.IsNullOrEmpty(q.questlineId)) continue;
00056:                 system.RegisterQuestline(q);
00057:                 count++;
00058:             }
00059:             return count;
00060:         }
00061:     }
00062: }
```


# Appendix — Current Source Detail: `src/Host/DynamicQuestHostSession.cs`

### `src/Host/DynamicQuestHostSession.cs` — complete current file

- Size: 94 lines / 3962 bytes.
- SHA-256: `e24b34b7ca00062195d137b359d0e9821d755fe63c2287c11b7bac311c02b0fd`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // ============================================================================
00003: // ASHFALL Plan 171 — Dynamic Quest Generation host session.
00004: // Loads the authored dynamic_quest_templates.json through the strict loader and
00005: // binds it to the Core DynamicQuestGenerator. Per the sealed Plan 171
00006: // disposition the generator is the authored template + deterministic candidate
00007: // authority; the canonical QuestRuntimeCoordinator owns the accepted-quest
00008: // lifecycle and its save, so this session creates no second quest lifecycle and
00009: // no second save section.
00010: // ============================================================================
00011:
00012: using System;
00013: using System.Collections.Generic;
00014: using System.IO;
00015: using Godot;
00016: using Ashfall.Core;
00017: using Ashfall.Core.Quests;
00018:
00019: namespace AtomicWar.GodotApp
00020: {
00021:     public sealed class DynamicQuestHostSession : HostSessionBase
00022:     {
00023:         private readonly DynamicQuestGenerator _generator = new();
00024:         private string _lastEvent = string.Empty;
00025:
00026:         public DynamicQuestGenerator Generator => _generator;
00027:         public DynamicQuestGeneratorCensus Census => _generator.GetCensus();
00028:         public string LastEvent => _lastEvent;
00029:         public IReadOnlyList<ProceduralQuestTemplate> Templates { get; private set; } = Array.Empty<ProceduralQuestTemplate>();
00030:
00031:         public DynamicQuestHostSession()
00032:         {
00033:             _generator.OnQuestGenerated += quest =>
00034:             {
00035:                 _lastEvent = $"Generated dynamic candidate {quest.QuestId} ({quest.Type})";
00036:                 RaiseStateChanged();
00037:             };
00038:             _generator.OnQuestCompleted += quest =>
00039:             {
00040:                 _lastEvent = $"Completed dynamic candidate {quest.QuestId}";
00041:                 RaiseStateChanged();
00042:             };
00043:             _generator.OnQuestExpired += quest =>
00044:             {
00045:                 _lastEvent = $"Dynamic candidate {quest.QuestId} is now {quest.Status}";
00046:                 RaiseStateChanged();
00047:             };
00048:         }
00049:
00050:         public static DynamicQuestHostSession Create(string dataDir)
00051:         {
00052:             var session = new DynamicQuestHostSession();
00053:             if (!string.IsNullOrEmpty(dataDir))
00054:                 session.LoadCatalog(dataDir);
00055:             return session;
00056:         }
00057:
00058:         /// <summary>
00059:         /// Loads and binds the authored dynamic quest templates through the strict
00060:         /// loader; the built-in defaults are replaced so they cannot mask authoring.
00061:         /// </summary>
00062:         public void LoadCatalog(string dataDir)
00063:         {
00064:             if (string.IsNullOrEmpty(dataDir)) return;
00065:
00066:             string path = Path.Combine(dataDir, "dynamic_quest_templates.json");
00067:             if (!File.Exists(path)) return;
00068:
00069:             var templates = DynamicQuestTemplateCatalogLoader.LoadFromJson(File.ReadAllText(path));
00070:             Templates = templates;
00071:             _generator.BindAuthoredTemplates(templates);
00072:             _lastEvent = $"Loaded {templates.Count} authored dynamic quest templates.";
00073:             RaiseStateChanged();
00074:         }
00075:
00076:         /// <summary>Deterministic candidate projection over the authored templates.</summary>
00077:         public IReadOnlyList<ProceduralQuest> GenerateCandidates(int day, ISeededRng? rng = null) =>
00078:             _generator.GenerateQuests(day, rng);
00079:
00080:         public bool AcceptCandidate(string questId, string survivorId = "") =>
00081:             _generator.AcceptQuest(questId, survivorId);
00082:
00083:         public bool ProgressCandidate(string questId, int amount = 1) =>
00084:             _generator.ProgressQuest(questId, amount);
00085:
00086:         public bool CompleteCandidate(string questId, int day) =>
00087:             _generator.CompleteQuest(questId, day);
00088:
00089:         public void CheckDeadlines(int day) => _generator.CheckDeadlines(day);
00090:
00091:         public DynamicQuestGeneratorState CaptureState() => _generator.CaptureState();
00092:         public void RestoreState(DynamicQuestGeneratorState state) => _generator.RestoreState(state);
00093:     }
00094: }
```


# Appendix — Current Source Detail: `src/Main.DynamicQuests.cs`

### `src/Main.DynamicQuests.cs` — complete current file

- Size: 51 lines / 1869 bytes.
- SHA-256: `4601d9b32143b0fbc649b2e9750fe6578a8da649627cb7acd73392fd14911604`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // ============================================================================
00003: // Main Partial : Wave 8 B2 — Dynamic Questline player route
00004: // Subsystems   : presentation glue only. The Core authority
00005: //                (DynamicQuestlineSystem) and its host driver
00006: //                (Main.Plans46_49.cs event triggers + daily tick + save) already
00007: //                exist; this file adds the read-only player board. No command
00008: //                authority is introduced.
00009: // ============================================================================
00010: using Godot;
00011: using Ashfall.Core.Quests;
00012:
00013: namespace AtomicWar.GodotApp
00014: {
00015:     public partial class Main
00016:     {
00017:         private UI.DynamicQuestlinePanel? _dynamicQuestlinePanel;
00018:         private DynamicQuestlineSystem? _dynamicQuestlinePanelBoundSystem;
00019:
00020:         private void EnsureDynamicQuestlinePanel()
00021:         {
00022:             var system = EnsureDynamicQuests();
00023:
00024:             if (_dynamicQuestlinePanel == null)
00025:             {
00026:                 _dynamicQuestlinePanel = new UI.DynamicQuestlinePanel();
00027:                 _dynamicQuestlinePanel.Bind(system);
00028:                 _dynamicQuestlinePanelBoundSystem = system;
00029:                 _dynamicQuestlinePanel.Visible = false;
00030:                 AddChild(_dynamicQuestlinePanel);
00031:                 return;
00032:             }
00033:
00034:             if (!ReferenceEquals(_dynamicQuestlinePanelBoundSystem, system))
00035:             {
00036:                 _dynamicQuestlinePanel.Bind(system);
00037:                 _dynamicQuestlinePanelBoundSystem = system;
00038:             }
00039:         }
00040:
00041:         private void OpenDynamicQuestlinePanel()
00042:         {
00043:             EnsureDynamicQuestlinePanel();
00044:             if (_dynamicQuestlinePanel != null)
00045:             {
00046:                 _dynamicQuestlinePanel.Visible = true;
00047:                 _dynamicQuestlinePanel.RefreshView();
00048:             }
00049:         }
00050:     }
00051: }
```


# Appendix — Current Source Detail: `src/Main.DynamicQuestGeneration.cs`

### `src/Main.DynamicQuestGeneration.cs` — complete current file

- Size: 37 lines / 1333 bytes.
- SHA-256: `9d8115268545e098edda9d31d79ecd246251b5d0efbc5fc65551df8a97a8d5a4`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // ============================================================================
00003: // ASHFALL Plan 171 — Dynamic Quest Generation host wiring.
00004: // The authored dynamic_quest_templates.json is loaded into the Core
00005: // DynamicQuestGenerator (the authored template + candidate authority). The
00006: // canonical QuestRuntimeCoordinator remains the accepted-quest lifecycle owner,
00007: // so this host exposes the candidate authority without a second lifecycle or a
00008: // second save section.
00009: // ============================================================================
00010:
00011: using System;
00012: using Godot;
00013: using Ashfall.Core.Quests;
00014:
00015: namespace AtomicWar.GodotApp
00016: {
00017:     public partial class Main
00018:     {
00019:         private DynamicQuestHostSession? _dynamicQuestGeneration;
00020:
00021:         public DynamicQuestHostSession? DynamicQuestGeneration => _dynamicQuestGeneration;
00022:
00023:         public void SetupDynamicQuestGeneration()
00024:         {
00025:             if (_dynamicQuestGeneration != null) return;
00026:             _dynamicQuestGeneration = DynamicQuestHostSession.Create(_dataDir);
00027:         }
00028:
00029:         public DynamicQuestGeneratorCensus GetDynamicQuestGenerationCensus() =>
00030:             _dynamicQuestGeneration?.Census ?? default;
00031:
00032:         public void ResetDynamicQuestGeneration()
00033:         {
00034:             _dynamicQuestGeneration = null;
00035:         }
00036:     }
00037: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/Narrative/YearOfAshQuestJsonParityTests.cs`

### `Ashfall.Core.Tests/Narrative/YearOfAshQuestJsonParityTests.cs` — complete current file

- Size: 164 lines / 7698 bytes.
- SHA-256: `e3d50b8f1763b2722dfca58e89c22efd814191f07ebb200ecd382752c45bdf3b`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using Xunit;
00006: using Ashfall.Core;
00007: using Ashfall.Core.IO;
00008: using Ashfall.Core.YearOfAsh;
00009:
00010: namespace Ashfall.Core.Tests.Narrative
00011: {
00012:     public class YearOfAshQuestJsonParityTests
00013:     {
00014:         private static string FindDataDir()
00015:         {
00016:             string search = Directory.GetCurrentDirectory();
00017:             for (int i = 0; i < 6; i++)
00018:             {
00019:                 string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
00020:                 if (Directory.Exists(candidate)) return candidate;
00021:                 string parent = Directory.GetParent(search)?.FullName;
00022:                 if (parent == null) break;
00023:                 search = parent;
00024:             }
00025:             return Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data");
00026:         }
00027:
00028:         [Fact]
00029:         public void CanonicalCatalog_ExistingEightQuestlines_MatchBuiltInBaseline()
00030:         {
00031:             var baseline = BuiltInQuestlineCatalog.CreateAll();
00032:             Assert.Equal(8, baseline.Count);
00033:
00034:             string dataDir = FindDataDir();
00035:             var fileIO = new FileSystemIO();
00036:
00037:             // Load back through Core catalog loader
00038:             var serializer = new SystemTextJsonSerializer();
00039:             var loaded = YearOfAshCatalogLoader.LoadQuestlines(dataDir, fileIO, serializer);
00040:
00041:             Assert.Equal(15, loaded.Count);
00042:
00043:             for (int i = 0; i < baseline.Count; i++)
00044:             {
00045:                 var baseQ = baseline[i];
00046:                 var loadQ = loaded.Find(q => q.questlineId == baseQ.questlineId);
00047:                 Assert.NotNull(loadQ);
00048:
00049:                 Assert.Equal(baseQ.questlineId, loadQ.questlineId);
00050:                 Assert.Equal(baseQ.title, loadQ.title);
00051:                 Assert.Equal(baseQ.synopsis, loadQ.synopsis);
00052:                 Assert.Equal(baseQ.factionTag, loadQ.factionTag);
00053:                 Assert.Equal(baseQ.firstStageId, loadQ.firstStageId);
00054:                 Assert.Equal(baseQ.minDay, loadQ.minDay);
00055:                 Assert.Equal(baseQ.maxDay, loadQ.maxDay);
00056:                 Assert.Equal(baseQ.stages.Count, loadQ.stages.Count);
00057:
00058:                 for (int s = 0; s < baseQ.stages.Count; s++)
00059:                 {
00060:                     var baseStage = baseQ.stages[s];
00061:                     var loadStage = loadQ.stages.Find(st => st.stageId == baseStage.stageId);
00062:                     Assert.NotNull(loadStage);
00063:
00064:                     Assert.Equal(baseStage.stageId, loadStage.stageId);
00065:                     Assert.Equal(baseStage.title, loadStage.title);
00066:                     Assert.Equal(baseStage.narrativePrompt, loadStage.narrativePrompt);
00067:                     Assert.Equal(baseStage.unlockOnDay, loadStage.unlockOnDay);
00068:                     Assert.Equal(baseStage.isTerminal, loadStage.isTerminal);
00069:                     Assert.Equal(baseStage.terminalOutcome, loadStage.terminalOutcome);
00070:                     Assert.Equal(baseStage.choices.Count, loadStage.choices.Count);
00071:
00072:                     for (int c = 0; c < baseStage.choices.Count; c++)
00073:                     {
00074:                         var baseChoice = baseStage.choices[c];
00075:                         var loadChoice = loadStage.choices.Find(ch => ch.choiceId == baseChoice.choiceId);
00076:                         Assert.NotNull(loadChoice);
00077:
00078:                         Assert.Equal(baseChoice.choiceId, loadChoice.choiceId);
00079:                         Assert.Equal(baseChoice.text, loadChoice.text);
00080:                         Assert.Equal(baseChoice.nextStageId, loadChoice.nextStageId);
00081:                         Assert.Equal(baseChoice.moraleDelta, loadChoice.moraleDelta);
00082:                         Assert.Equal(baseChoice.guiltDelta, loadChoice.guiltDelta);
00083:                         Assert.Equal(baseChoice.grantItemId ?? string.Empty, loadChoice.grantItemId ?? string.Empty);
00084:                         Assert.Equal(baseChoice.grantItemQuantity, loadChoice.grantItemQuantity);
00085:                         Assert.Equal(baseChoice.targetFactionId ?? string.Empty, loadChoice.targetFactionId ?? string.Empty);
00086:                         Assert.Equal(baseChoice.factionStandingDelta, loadChoice.factionStandingDelta);
00087:                         Assert.Equal(baseChoice.unlockEncounterId ?? string.Empty, loadChoice.unlockEncounterId ?? string.Empty);
00088:                         Assert.Equal(baseChoice.outcomeNarrative, loadChoice.outcomeNarrative);
00089:
00090:                         int baseCondCount = baseChoice.conditions?.Count ?? 0;
00091:                         int loadCondCount = loadChoice.conditions?.Count ?? 0;
00092:                         Assert.Equal(baseCondCount, loadCondCount);
00093:
00094:                         for (int k = 0; k < baseCondCount; k++)
00095:                         {
00096:                             Assert.Equal(baseChoice.conditions[k].conditionTag, loadChoice.conditions[k].conditionTag);
00097:                             Assert.Equal(baseChoice.conditions[k].isBlocker, loadChoice.conditions[k].isBlocker);
00098:                         }
00099:                     }
00100:                 }
00101:             }
00102:         }
00103:
00104:         [Fact]
00105:         public void CanonicalCatalog_ContainsExactlySevenPlan114Questlines()
00106:         {
00107:             string dataDir = FindDataDir();
00108:             var loaded = YearOfAshCatalogLoader.LoadQuestlines(
00109:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00110:
00111:             var expected = new[]
00112:             {
00113:                 "quest_garrison_amnesty_offer",
00114:                 "quest_ash_sign_pilgrimage",
00115:                 "quest_rebuilder_irrigation",
00116:                 "quest_hydro_baron_water_tax",
00117:                 "quest_black_ops_blackmail",
00118:                 "quest_garrison_mutiny",
00119:                 "quest_rebuilder_seed_failure"
00120:             };
00121:
00122:             Assert.Equal(15, loaded.Count);
00123:             foreach (var id in expected)
00124:                 Assert.Contains(loaded, quest => quest.questlineId == id);
00125:         }
00126:
00127:         [Fact]
00128:         public void PilotQuestline_GarrisonBloodDebt_LoadsAndPlaysThroughChoices()
00129:         {
00130:             string dataDir = FindDataDir();
00131:             var fileIO = new FileSystemIO();
00132:             var serializer = new SystemTextJsonSerializer();
00133:
00134:             var quests = YearOfAshCatalogLoader.LoadQuestlines(dataDir, fileIO, serializer);
00135:             var bloodDebt = quests.Find(q => q.questlineId == "quest_garrison_blood_debt");
00136:             Assert.NotNull(bloodDebt);
00137:
00138:             var system = new QuestlineSystem();
00139:             system.RegisterQuestline(bloodDebt);
00140:
00141:             Assert.True(system.StartQuestline("quest_garrison_blood_debt", 185));
00142:             var record = system.State.active.Find(a => a.questlineId == "quest_garrison_blood_debt");
00143:             Assert.NotNull(record);
00144:             Assert.Equal("stage_blood_debt_demand", record.currentStageId);
00145:
00146:             // Choice 1: confront Ola
00147:             var r1 = system.TakeChoice("quest_garrison_blood_debt", "choice_confront_ola", 185);
00148:             Assert.NotNull(r1);
00149:             Assert.Equal("stage_blood_debt_ola_testimony", record.currentStageId);
00150:
00151:             // Choice 2: forge rebuttal
00152:             var r2 = system.TakeChoice("quest_garrison_blood_debt", "choice_forge_tribunal_rebuttal", 187);
00153:             Assert.NotNull(r2);
00154:             Assert.Equal("stage_blood_debt_garrison_bluff", record.currentStageId);
00155:             Assert.Equal("item_falsified_clearance", r2.grantItemId);
00156:
00157:             // Choice 3: pass bluff
00158:             var r3 = system.TakeChoice("quest_garrison_blood_debt", "choice_pass_the_bluff", 200);
00159:             Assert.NotNull(r3);
00160:             Assert.Equal("stage_blood_debt_resolution_protected", record.currentStageId);
00161:             Assert.Equal(QuestlineStatus.Completed, record.status);
00162:         }
00163:     }
00164: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/QuestlineSystemTests.cs`

### `Ashfall.Core.Tests/QuestlineSystemTests.cs` — complete current file

- Size: 576 lines / 22700 bytes.
- SHA-256: `c199235a9c39313d20003da716f52d8af47a482870e1c93c547e52337a0e9906`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Linq;
00004: using Xunit;
00005: using Ashfall.Core.YearOfAsh;
00006:
00007: namespace Ashfall.Core.Tests
00008: {
00009:     /// <summary>
00010:     /// QA reviewer tests for QuestlineSystem.
00011:     /// Written independently from the implementation — only the spec and the diff.
00012:     /// Covers: registration, lifecycle, branching, terminal outcomes, state capture.
00013:     /// </summary>
00014:     public class QuestlineSystemTests
00015:     {
00016:         // ─── Helpers ───────────────────────────────────────────────────────────────
00017:
00018:         private static QuestlineDefinition MakeSimpleQuest(
00019:             string id = "quest_simple",
00020:             int minDay = 100,
00021:             int maxDay = 200)
00022:         {
00023:             var def = new QuestlineDefinition
00024:             {
00025:                 questlineId = id,
00026:                 title = "Simple Quest",
00027:                 synopsis = "A two-stage test questline.",
00028:                 firstStageId = "stage_a",
00029:                 minDay = minDay,
00030:                 maxDay = maxDay
00031:             };
00032:
00033:             def.stages.Add(new QuestStage
00034:             {
00035:                 stageId = "stage_a",
00036:                 title = "Stage A",
00037:                 narrativePrompt = "You face a choice.",
00038:                 unlockOnDay = 100,
00039:                 choices = new System.Collections.Generic.List<QuestChoice>
00040:                 {
00041:                     new QuestChoice
00042:                     {
00043:                         choiceId         = "choice_a_good",
00044:                         text             = "The compassionate path.",
00045:                         nextStageId      = "stage_b",
00046:                         moraleDelta      = 15,
00047:                         guiltDelta       = 0,
00048:                         targetFactionId  = "faction_rebuilders",
00049:                         factionStandingDelta = 10,
00050:                         outcomeNarrative = "You choose well."
00051:                     },
00052:                     new QuestChoice
00053:                     {
00054:                         choiceId         = "choice_a_ruthless",
00055:                         text             = "The ruthless path.",
00056:                         nextStageId      = "",   // terminal immediately
00057:                         moraleDelta      = -20,
00058:                         guiltDelta       = 25,
00059:                         outcomeNarrative = "The shelter endures. Others do not."
00060:                     }
00061:                 }
00062:             });
00063:
00064:             def.stages.Add(new QuestStage
00065:             {
00066:                 stageId = "stage_b",
00067:                 title = "Stage B — Resolution",
00068:                 narrativePrompt = "The consequence arrives.",
00069:                 unlockOnDay = 110,
00070:                 isTerminal = true,
00071:                 terminalOutcome = QuestlineStatus.Completed,
00072:                 choices = new System.Collections.Generic.List<QuestChoice>()
00073:             });
00074:
00075:             return def;
00076:         }
00077:
00078:         private static QuestlineDefinition MakeFailQuest(string id = "quest_fail")
00079:         {
00080:             var def = new QuestlineDefinition
00081:             {
00082:                 questlineId = id,
00083:                 title = "Fail Quest",
00084:                 firstStageId = "stage_only",
00085:                 minDay = 100,
00086:                 maxDay = 300
00087:             };
00088:             def.stages.Add(new QuestStage
00089:             {
00090:                 stageId = "stage_only",
00091:                 narrativePrompt = "Last chance.",
00092:                 isTerminal = false,
00093:                 choices = new System.Collections.Generic.List<QuestChoice>
00094:                 {
00095:                     new QuestChoice
00096:                     {
00097:                         choiceId         = "choice_fail",
00098:                         text             = "Fail.",
00099:                         nextStageId      = "stage_fail_terminal",
00100:                         moraleDelta      = -5,
00101:                         guiltDelta       = 0,
00102:                         outcomeNarrative = "It fails."
00103:                     }
00104:                 }
00105:             });
00106:             def.stages.Add(new QuestStage
00107:             {
00108:                 stageId = "stage_fail_terminal",
00109:                 isTerminal = true,
00110:                 terminalOutcome = QuestlineStatus.Failed,
00111:                 choices = new System.Collections.Generic.List<QuestChoice>()
00112:             });
00113:             return def;
00114:         }
00115:
00116:         // ─── Registration ──────────────────────────────────────────────────────────
00117:
00118:         [Fact]
00119:         public void RegisterQuestline_AddsToInternalCatalog()
00120:         {
00121:             var sys = new QuestlineSystem();
00122:             var extra = MakeSimpleQuest("quest_extra");
00123:             sys.RegisterQuestline(extra);
00124:
00125:             var found = sys.FindDefinition("quest_extra");
00126:             Assert.NotNull(found);
00127:             Assert.Equal("quest_extra", found.questlineId);
00128:         }
00129:
00130:         [Fact]
00131:         public void RegisterQuestline_NoDuplicates()
00132:         {
00133:             var sys = new QuestlineSystem();
00134:             var q = MakeSimpleQuest("quest_dup");
00135:             sys.RegisterQuestline(q);
00136:             sys.RegisterQuestline(q); // second add should be ignored
00137:
00138:             int count = sys.Catalog.Count(c => c.questlineId == "quest_dup");
00139:             Assert.Equal(1, count);
00140:         }
00141:
00142:         [Fact]
00143:         public void RegisterQuestline_NullIsSafe()
00144:         {
00145:             var sys = new QuestlineSystem();
00146:             // Must not throw
00147:             sys.RegisterQuestline(null);
00148:             sys.RegisterQuestline(null);
00149:             Assert.Empty(sys.GetAvailableQuestlines(1));
00150:             var def = new QuestlineDefinition { questlineId = "ql_test" };
00151:             sys.RegisterQuestline(def);
00152:             Assert.Same(def, sys.FindDefinition("ql_test"));
00153:             Assert.Null(sys.FindDefinition("ql_missing"));
00154:         }
00155:
00156:         // ─── Built-in catalog ──────────────────────────────────────────────────────
00157:
00158:         [Fact]
00159:         public void BuiltInCatalog_HasEightQuestlines()
00160:         {
00161:             var sys = new QuestlineSystem();
00162:             Assert.Equal(8, sys.Catalog.Count);
00163:         }
00164:
00165:         [Fact]
00166:         public void BuiltInCatalog_AllQuestlinesHaveFirstStage()
00167:         {
00168:             var sys = new QuestlineSystem();
00169:             foreach (var def in sys.Catalog)
00170:             {
00171:                 Assert.False(string.IsNullOrEmpty(def.firstStageId),
00172:                     $"{def.questlineId} has no firstStageId");
00173:
00174:                 var firstStage = def.FindStage(def.firstStageId);
00175:                 Assert.NotNull(firstStage);
00176:             }
00177:         }
00178:
00179:         [Fact]
00180:         public void BuiltInCatalog_AllQuestlinesHaveValidDayRanges()
00181:         {
00182:             var sys = new QuestlineSystem();
00183:             foreach (var def in sys.Catalog)
00184:             {
00185:                 Assert.True(def.minDay >= 180 && def.minDay <= 360,
00186:                     $"{def.questlineId} minDay={def.minDay} is outside 180-360");
00187:                 Assert.True(def.maxDay > def.minDay,
00188:                     $"{def.questlineId} maxDay must be > minDay");
00189:             }
00190:         }
00191:
00192:         [Fact]
00193:         public void BuiltInCatalog_GarrisonBloodDebt_Exists()
00194:         {
00195:             var sys = new QuestlineSystem();
00196:             Assert.NotNull(sys.FindDefinition("quest_garrison_blood_debt"));
00197:         }
00198:
00199:         [Fact]
00200:         public void BuiltInCatalog_TheLastBroadcast_Exists()
00201:         {
00202:             var sys = new QuestlineSystem();
00203:             Assert.NotNull(sys.FindDefinition("quest_the_last_broadcast"));
00204:         }
00205:
00206:         // ─── Availability ──────────────────────────────────────────────────────────
00207:
00208:         [Fact]
00209:         public void GetAvailableQuestlines_ReturnsQuestlinesInDayWindow()
00210:         {
00211:             var sys = new QuestlineSystem();
00212:             sys.RegisterQuestline(MakeSimpleQuest("quest_simple", minDay: 150, maxDay: 200));
00213:
00214:             var available = sys.GetAvailableQuestlines(175);
00215:             Assert.Contains(available, q => q.questlineId == "quest_simple");
00216:         }
00217:
00218:         [Fact]
00219:         public void GetAvailableQuestlines_ExcludesOutOfWindowQuests()
00220:         {
00221:             var sys = new QuestlineSystem();
00222:             sys.RegisterQuestline(MakeSimpleQuest("quest_future", minDay: 300, maxDay: 360));
00223:
00224:             var available = sys.GetAvailableQuestlines(100);
00225:             Assert.DoesNotContain(available, q => q.questlineId == "quest_future");
00226:         }
00227:
00228:         [Fact]
00229:         public void GetAvailableQuestlines_ExcludesAlreadyActive()
00230:         {
00231:             var sys = new QuestlineSystem();
00232:             sys.RegisterQuestline(MakeSimpleQuest("quest_simple", minDay: 100, maxDay: 200));
00233:             sys.StartQuestline("quest_simple", 120);
00234:
00235:             var available = sys.GetAvailableQuestlines(120);
00236:             Assert.DoesNotContain(available, q => q.questlineId == "quest_simple");
00237:         }
00238:
00239:         [Fact]
00240:         public void GetAvailableQuestlines_ExcludesCompleted()
00241:         {
00242:             var sys = new QuestlineSystem();
00243:             sys.RegisterQuestline(MakeSimpleQuest("quest_simple", minDay: 100, maxDay: 200));
00244:             sys.StartQuestline("quest_simple", 100);
00245:             // Take ruthless path → ends immediately (no nextStageId)
00246:             sys.TakeChoice("quest_simple", "choice_a_ruthless", 100);
00247:
00248:             var available = sys.GetAvailableQuestlines(150);
00249:             Assert.DoesNotContain(available, q => q.questlineId == "quest_simple");
00250:         }
00251:
00252:         // ─── Start ────────────────────────────────────────────────────────────────
00253:
00254:         [Fact]
00255:         public void StartQuestline_ReturnsTrue_AndCreatesActiveRecord()
00256:         {
00257:             var sys = new QuestlineSystem();
00258:             sys.RegisterQuestline(MakeSimpleQuest());
00259:
00260:             bool started = sys.StartQuestline("quest_simple", 100);
00261:
00262:             Assert.True(started);
00263:             var record = sys.GetActiveRecord("quest_simple");
00264:             Assert.NotNull(record);
00265:             Assert.Equal(QuestlineStatus.Active, record.status);
00266:             Assert.Equal("stage_a", record.currentStageId);
00267:             Assert.Equal(100, record.dayStarted);
00268:         }
00269:
00270:         [Fact]
00271:         public void StartQuestline_ReturnsFalse_IfAlreadyActive()
00272:         {
00273:             var sys = new QuestlineSystem();
00274:             sys.RegisterQuestline(MakeSimpleQuest());
00275:             sys.StartQuestline("quest_simple", 100);
00276:             bool second = sys.StartQuestline("quest_simple", 101);
00277:
00278:             Assert.False(second);
00279:         }
00280:
00281:         [Fact]
00282:         public void StartQuestline_ReturnsFalse_ForUnknownId()
00283:         {
00284:             var sys = new QuestlineSystem();
00285:             bool result = sys.StartQuestline("quest_nonexistent", 100);
00286:             Assert.False(result);
00287:         }
00288:
00289:         [Fact]
00290:         public void StartQuestline_FiresOnQuestlineStartedEvent()
00291:         {
00292:             var sys = new QuestlineSystem();
00293:             sys.RegisterQuestline(MakeSimpleQuest());
00294:
00295:             QuestlineDefinition? eventDef = null;
00296:             sys.OnQuestlineStarted += def => eventDef = def;
00297:
00298:             sys.StartQuestline("quest_simple", 100);
00299:             Assert.NotNull(eventDef);
00300:             Assert.Equal("quest_simple", eventDef.questlineId);
00301:         }
00302:
00303:         // ─── TakeChoice ───────────────────────────────────────────────────────────
00304:
00305:         [Fact]
00306:         public void TakeChoice_ReturnsNull_IfQuestNotActive()
00307:         {
00308:             var sys = new QuestlineSystem();
00309:             sys.RegisterQuestline(MakeSimpleQuest());
00310:
00311:             var result = sys.TakeChoice("quest_simple", "choice_a_good", 100);
00312:             Assert.Null(result);
00313:         }
00314:
00315:         [Fact]
00316:         public void TakeChoice_ReturnsNull_ForInvalidChoiceId()
00317:         {
00318:             var sys = new QuestlineSystem();
00319:             sys.RegisterQuestline(MakeSimpleQuest());
00320:             sys.StartQuestline("quest_simple", 100);
00321:
00322:             var result = sys.TakeChoice("quest_simple", "choice_bogus", 100);
00323:             Assert.Null(result);
00324:         }
00325:
00326:         [Fact]
00327:         public void TakeChoice_GoodPath_AdvancesToNextStage()
00328:         {
00329:             var sys = new QuestlineSystem();
00330:             sys.RegisterQuestline(MakeSimpleQuest());
00331:             sys.StartQuestline("quest_simple", 100);
00332:
00333:             var result = sys.TakeChoice("quest_simple", "choice_a_good", 105);
00334:
00335:             Assert.NotNull(result);
00336:             Assert.Equal("stage_a", result.stageId);
00337:             Assert.Equal("stage_b", result.nextStageId);
00338:             Assert.Equal(QuestlineStatus.Completed, result.newQuestStatus);
00339:
00340:             // Record should be resolved
00341:             var record = sys.GetActiveRecord("quest_simple");
00342:             Assert.Equal(QuestlineStatus.Completed, record.status);
00343:             Assert.Equal(105, record.dayResolved);
00344:         }
00345:
00346:         [Fact]
00347:         public void TakeChoice_RuthlessPath_ResolvesImmediately()
00348:         {
00349:             var sys = new QuestlineSystem();
00350:             sys.RegisterQuestline(MakeSimpleQuest());
00351:             sys.StartQuestline("quest_simple", 100);
00352:
00353:             var result = sys.TakeChoice("quest_simple", "choice_a_ruthless", 100);
00354:
00355:             Assert.NotNull(result);
00356:             Assert.Equal(string.Empty, result.nextStageId);
00357:             Assert.Equal(-20, result.moraleDelta);
00358:             Assert.Equal(25, result.guiltDelta);
00359:         }
00360:
00361:         [Fact]
00362:         public void TakeChoice_RecordsChoiceInHistory()
00363:         {
00364:             var sys = new QuestlineSystem();
00365:             sys.RegisterQuestline(MakeSimpleQuest());
00366:             sys.StartQuestline("quest_simple", 100);
00367:             sys.TakeChoice("quest_simple", "choice_a_good", 100);
00368:
00369:             var record = sys.GetActiveRecord("quest_simple");
00370:             Assert.Single(record.choiceHistory);
00371:             Assert.Equal("choice_a_good", record.choiceHistory[0]);
00372:         }
00373:
00374:         [Fact]
00375:         public void TakeChoice_AccumulatesMoraleAndGuilt()
00376:         {
00377:             var sys = new QuestlineSystem();
00378:             var q = MakeSimpleQuest("quest_m");
00379:             sys.RegisterQuestline(q);
00380:             sys.StartQuestline("quest_m", 100);
00381:             sys.TakeChoice("quest_m", "choice_a_good", 100);
00382:
00383:             Assert.Equal(15, sys.State.totalMoraleDeltaFromQuests);
00384:             Assert.Equal(0, sys.State.totalGuiltDeltaFromQuests);
00385:         }
00386:
00387:         [Fact]
00388:         public void TakeChoice_FiresOnQuestChoiceTakenEvent()
00389:         {
00390:             var sys = new QuestlineSystem();
00391:             sys.RegisterQuestline(MakeSimpleQuest());
00392:             sys.StartQuestline("quest_simple", 100);
00393:
00394:             QuestChoiceResult? eventResult = null;
00395:             sys.OnQuestChoiceTaken += r => eventResult = r;
00396:
00397:             sys.TakeChoice("quest_simple", "choice_a_good", 100);
00398:             Assert.NotNull(eventResult);
00399:             Assert.Equal("choice_a_good", eventResult.choiceId);
00400:         }
00401:
00402:         [Fact]
00403:         public void TakeChoice_FiresOnQuestlineResolved_WhenTerminal()
00404:         {
00405:             var sys = new QuestlineSystem();
00406:             sys.RegisterQuestline(MakeSimpleQuest());
00407:             sys.StartQuestline("quest_simple", 100);
00408:
00409:             string? resolvedId = null;
00410:             QuestlineStatus resolvedStatus = QuestlineStatus.NotStarted;
00411:             sys.OnQuestlineResolved += (id, status) => { resolvedId = id; resolvedStatus = status; };
00412:
00413:             sys.TakeChoice("quest_simple", "choice_a_good", 100);
00414:
00415:             Assert.Equal("quest_simple", resolvedId);
00416:             Assert.Equal(QuestlineStatus.Completed, resolvedStatus);
00417:         }
00418:
00419:         [Fact]
00420:         public void TakeChoice_CannotActOnResolvedQuestline()
00421:         {
00422:             var sys = new QuestlineSystem();
00423:             sys.RegisterQuestline(MakeSimpleQuest());
00424:             sys.StartQuestline("quest_simple", 100);
00425:             sys.TakeChoice("quest_simple", "choice_a_ruthless", 100);
00426:
00427:             // Quest is now resolved — second call should return null
00428:             var result = sys.TakeChoice("quest_simple", "choice_a_good", 101);
00429:             Assert.Null(result);
00430:         }
00431:
00432:         // ─── Failed terminal ───────────────────────────────────────────────────────
00433:
00434:         [Fact]
00435:         public void TakeChoice_FailedTerminal_RecordsAsFailure()
00436:         {
00437:             var sys = new QuestlineSystem();
00438:             sys.RegisterQuestline(MakeFailQuest());
00439:             sys.StartQuestline("quest_fail", 100);
00440:
00441:             var result = sys.TakeChoice("quest_fail", "choice_fail", 100);
00442:
00443:             Assert.NotNull(result);
00444:             Assert.Equal(QuestlineStatus.Failed, result.newQuestStatus);
00445:             Assert.Contains("quest_fail", sys.State.failedQuestlineIds);
00446:             Assert.DoesNotContain("quest_fail", sys.State.completedQuestlineIds);
00447:         }
00448:
00449:         // ─── State capture / restore ───────────────────────────────────────────────
00450:
00451:         [Fact]
00452:         public void CaptureState_IsDeepCopy()
00453:         {
00454:             var sys = new QuestlineSystem();
00455:             sys.RegisterQuestline(MakeSimpleQuest());
00456:             sys.StartQuestline("quest_simple", 100);
00457:             sys.TakeChoice("quest_simple", "choice_a_good", 100);
00458:
00459:             var snap1 = sys.CaptureState();
00460:
00461:             // Mutate original
00462:             sys.RegisterQuestline(MakeFailQuest());
00463:             sys.StartQuestline("quest_fail", 101);
00464:             sys.TakeChoice("quest_fail", "choice_fail", 101);
00465:
00466:             var snap2 = sys.CaptureState();
00467:
00468:             // snap1 must not reflect changes made after capture
00469:             Assert.Single(snap1.completedQuestlineIds);
00470:             Assert.Empty(snap1.failedQuestlineIds);
00471:
00472:             Assert.Single(snap2.completedQuestlineIds);
00473:             Assert.Single(snap2.failedQuestlineIds);
00474:         }
00475:
00476:         [Fact]
00477:         public void CaptureState_CanRestoreSystemFromState()
00478:         {
00479:             var sys1 = new QuestlineSystem();
00480:             sys1.RegisterQuestline(MakeSimpleQuest());
00481:             sys1.StartQuestline("quest_simple", 100);
00482:             sys1.TakeChoice("quest_simple", "choice_a_ruthless", 100);
00483:
00484:             var savedState = sys1.CaptureState();
00485:
00486:             // Reconstruct from saved state
00487:             var sys2 = new QuestlineSystem(savedState);
00488:
00489:             Assert.Contains("quest_simple", sys2.State.completedQuestlineIds);
00490:             Assert.Equal(-20, sys2.State.totalMoraleDeltaFromQuests);
00491:             Assert.Equal(25, sys2.State.totalGuiltDeltaFromQuests);
00492:         }
00493:
00494:         // ─── Garrison Blood Debt integration smoke test ────────────────────────────
00495:
00496:         [Fact]
00497:         public void GarrisonBloodDebt_CompassionatePath_ReachesProtectedResolution()
00498:         {
00499:             var sys = new QuestlineSystem();
00500:             // Built-in catalog already has it
00501:             sys.StartQuestline("quest_garrison_blood_debt", 186);
00502:
00503:             var r1 = sys.TakeChoice("quest_garrison_blood_debt", "choice_confront_ola", 186);
00504:             Assert.NotNull(r1);
00505:             Assert.Equal("stage_blood_debt_ola_testimony", r1.nextStageId);
00506:             Assert.Equal(QuestlineStatus.Active, r1.newQuestStatus);
00507:
00508:             var r2 = sys.TakeChoice("quest_garrison_blood_debt", "choice_send_ola_underground", 190);
00509:             Assert.NotNull(r2);
00510:             Assert.Equal("stage_blood_debt_garrison_search", r2.nextStageId);
00511:             Assert.Equal(QuestlineStatus.Active, r2.newQuestStatus);
00512:
00513:             var r3 = sys.TakeChoice("quest_garrison_blood_debt", "choice_scatter_rad_bait", 206);
00514:             Assert.NotNull(r3);
00515:             Assert.Equal("stage_blood_debt_resolution_protected", r3.nextStageId);
00516:             // Terminal stage → should resolve
00517:             Assert.Equal(QuestlineStatus.Completed, r3.newQuestStatus);
00518:         }
00519:
00520:         [Fact]
00521:         public void GarrisonBloodDebt_ComplyCausesHighGuiltDelta()
00522:         {
00523:             var sys = new QuestlineSystem();
00524:             sys.StartQuestline("quest_garrison_blood_debt", 186);
00525:
00526:             var r = sys.TakeChoice("quest_garrison_blood_debt", "choice_comply_immediately", 186);
00527:             Assert.NotNull(r);
00528:             Assert.Equal(40, r.guiltDelta);
00529:             Assert.Equal(-30, r.moraleDelta);
00530:         }
00531:
00532:         // ─── The Last Broadcast smoke test ─────────────────────────────────────────
00533:
00534:         [Fact]
00535:         public void TheLastBroadcast_DenyingAntennaEndsQuestWithHighGuilt()
00536:         {
00537:             var sys = new QuestlineSystem();
00538:             sys.StartQuestline("quest_the_last_broadcast", 320);
00539:
00540:             var r = sys.TakeChoice("quest_the_last_broadcast", "choice_deny_antenna_access", 320);
00541:             Assert.NotNull(r);
00542:             Assert.Equal(40, r.guiltDelta);
00543:             Assert.Equal(-30, r.moraleDelta);
00544:             Assert.Equal(QuestlineStatus.Completed, r.newQuestStatus); // no next stage → completed by default
00545:         }
00546:
00547:         [Fact]
00548:         public void TheLastBroadcast_RequestArchive_GrantsItem()
00549:         {
00550:             var sys = new QuestlineSystem();
00551:             sys.StartQuestline("quest_the_last_broadcast", 320);
00552:
00553:             var r = sys.TakeChoice("quest_the_last_broadcast", "choice_request_copy_of_archive", 320);
00554:             Assert.NotNull(r);
00555:             Assert.Equal("item_meridian_archive_copy", r.grantItemId);
00556:             Assert.Equal(1, r.grantItemQty);
00557:         }
00558:
00559:         // ─── Ash Sign Revelation smoke test ───────────────────────────────────────
00560:
00561:         [Fact]
00562:         public void AshSignRevelation_BuryingEvidenceLeadsToFailure()
00563:         {
00564:             var sys = new QuestlineSystem();
00565:             sys.StartQuestline("quest_ash_sign_revelation", 220);
00566:
00567:             var r1 = sys.TakeChoice("quest_ash_sign_revelation", "choice_verify_documents", 220);
00568:             Assert.NotNull(r1);
00569:
00570:             var r2 = sys.TakeChoice("quest_ash_sign_revelation", "choice_bury_evidence", 226);
00571:             Assert.NotNull(r2);
00572:             Assert.Equal("stage_revelation_buried", r2.nextStageId);
00573:             Assert.Equal(QuestlineStatus.Failed, r2.newQuestStatus);
00574:         }
00575:     }
00576: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/Quests/DynamicQuestlineTests.cs`

### `Ashfall.Core.Tests/Quests/DynamicQuestlineTests.cs` — complete current file

- Size: 115 lines / 4999 bytes.
- SHA-256: `d6e01b987655e100f14724dc5fb4a191684d75256db8ef6653824d783d76995d`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System.Collections.Generic;
00003: using Ashfall.Core.Quests;
00004: using Xunit;
00005:
00006: namespace Ashfall.Core.Tests.Quests
00007: {
00008:     public sealed class DynamicQuestlineTests
00009:     {
00010:         [Fact]
00011:         public void RescueMinersQuest_TriggersAndAdvancesStages()
00012:         {
00013:             var system = new DynamicQuestlineSystem();
00014:             var trapped = new List<string> { "survivor_miner_1", "survivor_miner_2" };
00015:
00016:             var quest = system.TriggerRescueMinersQuest("inc_cavein_sector_c", "sec_deep_quarry", trapped, triggerDay: 10, deadlineDays: 3, requiredLabor: 200);
00017:
00018:             Assert.NotNull(quest);
00019:             Assert.Equal(DynamicQuestlineSystem.RescueMinersQuestId, quest.QuestId);
00020:             Assert.Equal("sec_deep_quarry", quest.TargetLocationId);
00021:             Assert.Equal(2, quest.TargetSurvivorIds.Count);
00022:             Assert.Equal(10, quest.TriggerDay);
00023:             Assert.Equal(13, quest.DeadlineDay);
00024:             Assert.Equal(DynamicQuestStatus.Active, quest.Status);
00025:             Assert.Equal(0, quest.CurrentStageIndex);
00026:             Assert.Equal(4, quest.Stages.Count);
00027:
00028:             // Cannot trigger same incident twice
00029:             var duplicate = system.TriggerRescueMinersQuest("inc_cavein_sector_c", "sec_deep_quarry", trapped, 10);
00030:             Assert.Null(duplicate);
00031:
00032:             // Advance progress halfway
00033:             system.AdvanceQuestProgress(quest.QuestId, 100);
00034:             Assert.Equal(100, quest.ProgressCurrent);
00035:             Assert.True(quest.CurrentStageIndex >= 1);
00036:
00037:             // Complete progress
00038:             system.AdvanceQuestProgress(quest.QuestId, 100);
00039:             Assert.Equal(DynamicQuestStatus.Completed, quest.Status);
00040:             Assert.Empty(system.ActiveQuests);
00041:             Assert.Contains(quest.QuestId, system.CompletedIds);
00042:         }
00043:
00044:         [Fact]
00045:         public void RescueMinersQuest_FailsWhenDeadlineExpires()
00046:         {
00047:             var system = new DynamicQuestlineSystem();
00048:             var trapped = new List<string> { "survivor_miner_1" };
00049:             var quest = system.TriggerRescueMinersQuest("inc_cavein_sec_a", "sec_a", trapped, triggerDay: 5, deadlineDays: 2);
00050:
00051:             Assert.NotNull(quest);
00052:             Assert.Equal(7, quest.DeadlineDay);
00053:
00054:             system.TickDay(6);
00055:             Assert.Equal(DynamicQuestStatus.Active, quest.Status);
00056:
00057:             system.TickDay(7); // Deadline reached
00058:             Assert.Equal(DynamicQuestStatus.Failed, quest.Status);
00059:             Assert.Empty(system.ActiveQuests);
00060:             Assert.Contains(quest.QuestId, system.FailedIds);
00061:         }
00062:
00063:         [Fact]
00064:         public void InvestigateRadioDepotQuest_TriggersAndCompletes()
00065:         {
00066:             var system = new DynamicQuestlineSystem();
00067:
00068:             var quest = system.TriggerInvestigateRadioDepotQuest("intercept_depot_delta", "loc_military_depot", triggerDay: 12);
00069:             Assert.NotNull(quest);
00070:             Assert.Equal(DynamicQuestlineSystem.InvestigateRadioDepotQuestId, quest.QuestId);
00071:             Assert.Equal("loc_military_depot", quest.TargetLocationId);
00072:
00073:             // Advance stages
00074:             Assert.True(system.AdvanceQuestStage(quest.QuestId));
00075:             Assert.Equal(1, quest.CurrentStageIndex);
00076:             Assert.True(system.AdvanceQuestStage(quest.QuestId));
00077:             Assert.Equal(2, quest.CurrentStageIndex);
00078:             Assert.True(system.AdvanceQuestStage(quest.QuestId)); // Final stage completes
00079:             Assert.Equal(DynamicQuestStatus.Completed, quest.Status);
00080:             Assert.Empty(system.ActiveQuests);
00081:         }
00082:
00083:         [Fact]
00084:         public void ArmoryMunitionsRefurbishQuest_ProgressAndStateRoundtrip()
00085:         {
00086:             var system = new DynamicQuestlineSystem();
00087:             var quest = system.TriggerArmoryMunitionsRefurbishQuest("inc_armory_wear_1", triggerDay: 8, weaponsNeedingRepair: 4);
00088:
00089:             Assert.NotNull(quest);
00090:             Assert.Equal(DynamicQuestlineSystem.ArmoryMunitionsRefurbishQuestId, quest.QuestId);
00091:             Assert.Equal(4, quest.ProgressRequired);
00092:
00093:             system.AdvanceQuestProgress(quest.QuestId, 2);
00094:             Assert.Equal(2, quest.ProgressCurrent);
00095:
00096:             // Capture and restore
00097:             var captured = system.CaptureState();
00098:             Assert.NotNull(captured);
00099:
00100:             var system2 = new DynamicQuestlineSystem();
00101:             system2.RestoreState(captured);
00102:
00103:             var restoredQuest = system2.GetActiveQuest(DynamicQuestlineSystem.ArmoryMunitionsRefurbishQuestId);
00104:             Assert.NotNull(restoredQuest);
00105:             Assert.Equal(2, restoredQuest.ProgressCurrent);
00106:             Assert.Equal(4, restoredQuest.ProgressRequired);
00107:             Assert.Equal(restoredQuest.CurrentStageIndex, quest.CurrentStageIndex);
00108:
00109:             // Ensure incident is still recorded as triggered
00110:             Assert.True(system2.HasIncidentTriggered("inc_armory_wear_1"));
00111:             var dupe = system2.TriggerArmoryMunitionsRefurbishQuest("inc_armory_wear_1", 9, 4);
00112:             Assert.Null(dupe);
00113:         }
00114:     }
00115: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/QuestlineMasterCatalog.cs`

### `Assets/Ashfall.Core/QuestlineMasterCatalog.cs` — complete current file

- Size: 155 lines / 5930 bytes.
- SHA-256: `fe54bcda711a61a040345e87487202fee52853fcd2d794806dee370b9114a18a`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004:
00005: namespace Ashfall.Core
00006: {
00007:     /// <summary>
00008:     /// Canonical quest ID registry. <c>questline_master.json</c> declares every
00009:     /// legitimate quest ID in the game — both those backed by full quest
00010:     /// definitions in expansion catalogs and those reserved for planned content.
00011:     ///
00012:     /// Two purposes:
00013:     /// 1. <b>Validation</b>: quest IDs in any catalog can be checked against the
00014:     ///    registry. An authored quest whose ID is absent from the registry is
00015:     ///    either a typo or unregistered content — both are authoring errors.
00016:     /// 2. <b>Enumeration</b>: UI and debugging tools can list all known quest IDs,
00017:     ///    including registered-but-contentless IDs that are reserved for future
00018:     ///    expansions.
00019:     ///
00020:     /// The registry does NOT contain quest content (stages, choices, rewards).
00021:     /// It contains only IDs. Content lives in the per-expansion quest catalogs
00022:     /// (holdfast_quests.json, crossing_quests.json, etc.).
00023:     /// </summary>
00024:     public sealed class QuestlineMasterCatalog
00025:     {
00026:         private readonly HashSet<string> _ids = new HashSet<string>(StringComparer.Ordinal);
00027:         private readonly List<string> _ordered = new List<string>();
00028:
00029:         /// <summary>Total registered quest IDs.</summary>
00030:         public int Count => _ids.Count;
00031:
00032:         /// <summary>All registered IDs in file order.</summary>
00033:         public IReadOnlyList<string> All => _ordered;
00034:
00035:         /// <summary>Returns true if the quest ID is registered in the master list.</summary>
00036:         public bool IsRegistered(string questId)
00037:         {
00038:             return !string.IsNullOrEmpty(questId) && _ids.Contains(questId);
00039:         }
00040:
00041:         /// <summary>Returns true if the quest ID is registered AND has content in at least one expansion catalog.</summary>
00042:         public bool HasContent(string questId, params IReadOnlyList<string>[] catalogQuestIds)
00043:         {
00044:             if (!IsRegistered(questId)) return false;
00045:             for (int i = 0; i < catalogQuestIds.Length; i++)
00046:             {
00047:                 var catalog = catalogQuestIds[i];
00048:                 for (int j = 0; j < catalog.Count; j++)
00049:                 {
00050:                     if (string.Equals(catalog[j], questId, StringComparison.Ordinal))
00051:                         return true;
00052:                 }
00053:             }
00054:             return false;
00055:         }
00056:
00057:         /// <summary>
00058:         /// Validates that every quest ID in the provided catalogs is registered.
00059:         /// Returns the list of unregistered IDs (empty if all are registered).
00060:         /// </summary>
00061:         public List<string> FindUnregistered(IEnumerable<string> catalogQuestIds)
00062:         {
00063:             var missing = new List<string>();
00064:             foreach (string id in catalogQuestIds)
00065:             {
00066:                 if (!string.IsNullOrEmpty(id) && !_ids.Contains(id))
00067:                     missing.Add(id);
00068:             }
00069:             return missing;
00070:         }
00071:
00072:         /// <summary>Registers an ID. Internal — use the loader.</summary>
00073:         internal void Add(string id)
00074:         {
00075:             if (string.IsNullOrEmpty(id)) return;
00076:             if (_ids.Add(id))
00077:                 _ordered.Add(id);
00078:         }
00079:     }
00080:
00081:     /// <summary>DTO for deserializing questline_master.json entries.</summary>
00082:     [Serializable]
00083:     public sealed class QuestlineMasterEntry
00084:     {
00085:         public string id = string.Empty;
00086:     }
00087:
00088:     /// <summary>DTO for deserializing questline_master.json root.</summary>
00089:     [Serializable]
00090:     public sealed class QuestlineMasterRoot
00091:     {
00092:         public int schema_version;
00093:         public List<QuestlineMasterEntry> entries = new List<QuestlineMasterEntry>();
00094:     }
00095:
00096:     /// <summary>
00097:     /// Loads <c>questline_master.json</c> into a <see cref="QuestlineMasterCatalog"/>.
00098:     /// Follows the same pattern as other catalog loaders: IFileIO + IJsonSerializer,
00099:     /// tolerant of missing files, warns on parse failure.
00100:     /// </summary>
00101:     public sealed class QuestlineMasterCatalogLoader
00102:     {
00103:         public const string FileName = "questline_master.json";
00104:
00105:         private readonly IFileIO _files;
00106:         private readonly IJsonSerializer _json;
00107:         private readonly ILog _log;
00108:
00109:         public QuestlineMasterCatalogLoader(IFileIO files, IJsonSerializer json, ILog? log = null)
00110:         {
00111:             _files = files ?? throw new ArgumentNullException(nameof(files));
00112:             _json = json ?? throw new ArgumentNullException(nameof(json));
00113:             _log = log ?? NullLog.Instance;
00114:         }
00115:
00116:         public QuestlineMasterCatalog Load(string dataDirectory)
00117:         {
00118:             var catalog = new QuestlineMasterCatalog();
00119:             if (string.IsNullOrEmpty(dataDirectory) || !_files.DirectoryExists(dataDirectory))
00120:             {
00121:                 _log.Warn("Questline master directory missing: " + dataDirectory);
00122:                 return catalog;
00123:             }
00124:
00125:             string path = _files.Combine(dataDirectory, FileName);
00126:             if (!_files.FileExists(path))
00127:             {
00128:                 _log.Warn("Questline master file missing: " + path);
00129:                 return catalog;
00130:             }
00131:
00132:             try
00133:             {
00134:                 string raw = _files.ReadAllText(path);
00135:                 var root = _json.Deserialize<QuestlineMasterRoot>(raw);
00136:                 if (root?.entries == null) return catalog;
00137:
00138:                 for (int i = 0; i < root.entries.Count; i++)
00139:                 {
00140:                     var e = root.entries[i];
00141:                     if (e == null || string.IsNullOrEmpty(e.id)) continue;
00142:                     catalog.Add(e.id);
00143:                 }
00144:
00145:                 _log.Info("Questline master registry loaded: " + catalog.Count + " quest IDs");
00146:             }
00147:             catch (Exception ex)
00148:             {
00149:                 _log.Warn("Questline master parse failed: " + ex.Message);
00150:             }
00151:
00152:             return catalog;
00153:         }
00154:     }
00155: }
```


# Appendix — Current Source Detail: `src/UI/DynamicQuestlinePanel.cs`

### `src/UI/DynamicQuestlinePanel.cs` — complete current file

- Size: 199 lines / 8548 bytes.
- SHA-256: `2599df8f165471a02239bc26cd4dca5db22e4ed08600172e940164bdef154f97`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Godot;
00005: using Ashfall.Core.Quests;
00006: using Ashfall.Core.UI;
00007: using DesignTheme = Ashfall.Core.UI.Theme;
00008:
00009: namespace AtomicWar.GodotApp.UI
00010: {
00011:     /// <summary>
00012:     /// Wave 8 B2 — Emergency Dynamic Questline board.
00013:     ///
00014:     /// Read-only player surface over the campaign-wide emergency quest runtime.
00015:     /// Quests are opened, advanced, completed, and failed by real shelter and
00016:     /// field events (workshop refurbishment, radio triangulation, excavation
00017:     /// cave-ins); this board only reports the authoritative
00018:     /// <see cref="DynamicQuestlineSystem"/> state. It owns no command and
00019:     /// recomputes nothing.
00020:     /// </summary>
00021:     public partial class DynamicQuestlinePanel : Control, IBindablePanel
00022:     {
00023:         public event Action? OnClose;
00024:
00025:         private AshfallDashboardShell _shell = null!;
00026:         private AshfallStatusRail? _statusRail;
00027:         private VBoxContainer _contentStack = null!;
00028:         private Label _detailText = null!;
00029:         private Label _activeText = null!;
00030:         private Label _historyText = null!;
00031:
00032:         private DynamicQuestlineSystem? _system;
00033:
00034:         public bool IsBound => _system != null;
00035:
00036:         public void Bind(DynamicQuestlineSystem system)
00037:         {
00038:             Unbind();
00039:             _system = system;
00040:             _system.OnStateChanged += HandleStateChanged;
00041:             RefreshView();
00042:         }
00043:
00044:         public void Unbind()
00045:         {
00046:             if (_system != null)
00047:             {
00048:                 _system.OnStateChanged -= HandleStateChanged;
00049:                 _system = null;
00050:             }
00051:         }
00052:
00053:         private void HandleStateChanged() => RefreshView();
00054:
00055:         public override void _Ready()
00056:         {
00057:             SetAnchorsPreset(LayoutPreset.FullRect);
00058:
00059:             _shell = new AshfallDashboardShell("EMERGENCY QUESTS // DYNAMIC OPERATIONS", minWidth: 1100, minHeight: 680);
00060:             AddChild(_shell);
00061:
00062:             _statusRail = _shell.SetStatusRail();
00063:             _statusRail.AddCard("active", "Active", "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
00064:             _statusRail.AddCard("completed", "Resolved", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
00065:             _statusRail.AddCard("failed", "Failed / Expired", "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);
00066:             _statusRail.AddCard("incidents", "Incidents Seen", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
00067:             _statusRail.AddCard("day", "Campaign Day", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
00068:
00069:             var scroll = new ScrollContainer
00070:             {
00071:                 SizeFlagsVertical = SizeFlags.ExpandFill,
00072:                 SizeFlagsHorizontal = SizeFlags.ExpandFill
00073:             };
00074:             _contentStack = new VBoxContainer();
00075:             _contentStack.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
00076:             _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00077:             scroll.AddChild(_contentStack);
00078:
00079:             _contentStack.AddChild(_detailText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });
00080:
00081:             _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
00082:             _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("ACTIVE OPERATIONS"));
00083:             _contentStack.AddChild(_activeText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });
00084:
00085:             _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
00086:             _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("RESOLVED & FAILED"));
00087:             _contentStack.AddChild(_historyText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });
00088:
00089:             var note = AshfallUiHelpers.MakeBody(
00090:                 "Emergency operations are opened by what happens in the shelter and the field — a "
00091:                 + "collapsed gallery, a triangulated broadcast, an armory going unserviceable. This "
00092:                 + "board reports the active response; it does not start, advance, or cancel one.");
00093:             note.AutowrapMode = TextServer.AutowrapMode.WordSmart;
00094:             note.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
00095:             _contentStack.AddChild(note);
00096:
00097:             _shell.SetContent(scroll);
00098:             _shell.AttachHeaderCloseButton("CLOSE", () =>
00099:             {
00100:                 Visible = false;
00101:                 OnClose?.Invoke();
00102:             });
00103:
00104:             RefreshView();
00105:         }
00106:
00107:         public override void _ExitTree()
00108:         {
00109:             Unbind();
00110:             base._ExitTree();
00111:         }
00112:
00113:         public void RefreshView()
00114:         {
00115:             if (_system == null || _statusRail == null) return;
00116:
00117:             int active = _system.ActiveQuests.Count;
00118:             _statusRail.Set("active", active.ToString(),
00119:                 active > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
00120:             _statusRail.Set("completed", _system.CompletedIds.Count.ToString(), AshfallMetricCard.Criticality.Normal);
00121:             _statusRail.Set("failed", _system.FailedIds.Count.ToString(),
00122:                 _system.FailedIds.Count > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
00123:             _statusRail.Set("incidents", _system.State.triggeredIncidentIds.Count.ToString(), AshfallMetricCard.Criticality.Normal);
00124:             _statusRail.Set("day", _system.State.currentDay.ToString(), AshfallMetricCard.Criticality.Normal);
00125:
00126:             if (_detailText != null)
00127:             {
00128:                 _detailText.Text =
00129:                     $"Active: {active} | Resolved: {_system.CompletedIds.Count} | Failed: {_system.FailedIds.Count} | "
00130:                     + $"Tracked incidents: {_system.State.triggeredIncidentIds.Count}";
00131:             }
00132:
00133:             RenderActive();
00134:             RenderHistory();
00135:         }
00136:
00137:         private void RenderActive()
00138:         {
00139:             if (_activeText == null || _system == null) return;
00140:             if (_system.ActiveQuests.Count == 0)
00141:             {
00142:                 _activeText.Text = "No emergency operations are active. The response board is clear.";
00143:                 return;
00144:             }
00145:
00146:             var lines = new List<string>();
00147:             foreach (var q in _system.ActiveQuests)
00148:             {
00149:                 if (q == null) continue;
00150:                 lines.Add(FormatQuest(q));
00151:                 lines.Add(string.Empty);
00152:             }
00153:             _activeText.Text = string.Join("\n", lines).TrimEnd();
00154:         }
00155:
00156:         private void RenderHistory()
00157:         {
00158:             if (_historyText == null || _system == null) return;
00159:             if (_system.CompletedIds.Count == 0 && _system.FailedIds.Count == 0)
00160:             {
00161:                 _historyText.Text = "Nothing has been resolved or lost yet.";
00162:                 return;
00163:             }
00164:
00165:             var lines = new List<string>();
00166:             if (_system.CompletedIds.Count > 0)
00167:                 lines.Add("Resolved: " + string.Join(", ", _system.CompletedIds));
00168:             if (_system.FailedIds.Count > 0)
00169:                 lines.Add("Failed / expired: " + string.Join(", ", _system.FailedIds));
00170:             _historyText.Text = string.Join("\n", lines);
00171:         }
00172:
00173:         private string FormatQuest(DynamicQuestInstance q)
00174:         {
00175:             string stage = q.CurrentStageIndex >= 0 && q.CurrentStageIndex < q.Stages.Count
00176:                 ? q.Stages[q.CurrentStageIndex]
00177:                 : "—";
00178:             string deadline = FormatDeadline(q);
00179:             string targets = q.TargetSurvivorIds != null && q.TargetSurvivorIds.Count > 0
00180:                 ? $" | people at risk: {q.TargetSurvivorIds.Count}"
00181:                 : string.Empty;
00182:             return $"{q.Title}\n"
00183:                 + $"  {q.Description}\n"
00184:                 + $"  Site: {q.TargetLocationId} | Stage {q.CurrentStageIndex + 1}/{q.Stages.Count} ({stage}) | "
00185:                 + $"Progress {q.ProgressCurrent}/{q.ProgressRequired} | Opened day {q.TriggerDay} | Deadline {deadline}{targets}";
00186:         }
00187:
00188:         private string FormatDeadline(DynamicQuestInstance q)
00189:         {
00190:             if (!q.DeadlineDay.HasValue) return "none";
00191:             int day = _system?.State.currentDay ?? 0;
00192:             int daysLeft = q.DeadlineDay.Value - day;
00193:             string when = daysLeft > 0 ? $"day {q.DeadlineDay.Value} ({daysLeft}d left)"
00194:                 : daysLeft == 0 ? $"day {q.DeadlineDay.Value} (due today)"
00195:                 : $"day {q.DeadlineDay.Value} (overdue)";
00196:             return when;
00197:         }
00198:     }
00199: }
```


# Appendix — Focused Current Evidence: `Ashfall.Core.Tests/Narrative/YearOfAshQuestJsonParityTests.cs`

### `Ashfall.Core.Tests/Narrative/YearOfAshQuestJsonParityTests.cs` — complete current file

- Size: 164 lines / 7698 bytes.
- SHA-256: `e3d50b8f1763b2722dfca58e89c22efd814191f07ebb200ecd382752c45bdf3b`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using Xunit;
00006: using Ashfall.Core;
00007: using Ashfall.Core.IO;
00008: using Ashfall.Core.YearOfAsh;
00009:
00010: namespace Ashfall.Core.Tests.Narrative
00011: {
00012:     public class YearOfAshQuestJsonParityTests
00013:     {
00014:         private static string FindDataDir()
00015:         {
00016:             string search = Directory.GetCurrentDirectory();
00017:             for (int i = 0; i < 6; i++)
00018:             {
00019:                 string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
00020:                 if (Directory.Exists(candidate)) return candidate;
00021:                 string parent = Directory.GetParent(search)?.FullName;
00022:                 if (parent == null) break;
00023:                 search = parent;
00024:             }
00025:             return Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data");
00026:         }
00027:
00028:         [Fact]
00029:         public void CanonicalCatalog_ExistingEightQuestlines_MatchBuiltInBaseline()
00030:         {
00031:             var baseline = BuiltInQuestlineCatalog.CreateAll();
00032:             Assert.Equal(8, baseline.Count);
00033:
00034:             string dataDir = FindDataDir();
00035:             var fileIO = new FileSystemIO();
00036:
00037:             // Load back through Core catalog loader
00038:             var serializer = new SystemTextJsonSerializer();
00039:             var loaded = YearOfAshCatalogLoader.LoadQuestlines(dataDir, fileIO, serializer);
00040:
00041:             Assert.Equal(15, loaded.Count);
00042:
00043:             for (int i = 0; i < baseline.Count; i++)
00044:             {
00045:                 var baseQ = baseline[i];
00046:                 var loadQ = loaded.Find(q => q.questlineId == baseQ.questlineId);
00047:                 Assert.NotNull(loadQ);
00048:
00049:                 Assert.Equal(baseQ.questlineId, loadQ.questlineId);
00050:                 Assert.Equal(baseQ.title, loadQ.title);
00051:                 Assert.Equal(baseQ.synopsis, loadQ.synopsis);
00052:                 Assert.Equal(baseQ.factionTag, loadQ.factionTag);
00053:                 Assert.Equal(baseQ.firstStageId, loadQ.firstStageId);
00054:                 Assert.Equal(baseQ.minDay, loadQ.minDay);
00055:                 Assert.Equal(baseQ.maxDay, loadQ.maxDay);
00056:                 Assert.Equal(baseQ.stages.Count, loadQ.stages.Count);
00057:
00058:                 for (int s = 0; s < baseQ.stages.Count; s++)
00059:                 {
00060:                     var baseStage = baseQ.stages[s];
00061:                     var loadStage = loadQ.stages.Find(st => st.stageId == baseStage.stageId);
00062:                     Assert.NotNull(loadStage);
00063:
00064:                     Assert.Equal(baseStage.stageId, loadStage.stageId);
00065:                     Assert.Equal(baseStage.title, loadStage.title);
00066:                     Assert.Equal(baseStage.narrativePrompt, loadStage.narrativePrompt);
00067:                     Assert.Equal(baseStage.unlockOnDay, loadStage.unlockOnDay);
00068:                     Assert.Equal(baseStage.isTerminal, loadStage.isTerminal);
00069:                     Assert.Equal(baseStage.terminalOutcome, loadStage.terminalOutcome);
00070:                     Assert.Equal(baseStage.choices.Count, loadStage.choices.Count);
00071:
00072:                     for (int c = 0; c < baseStage.choices.Count; c++)
00073:                     {
00074:                         var baseChoice = baseStage.choices[c];
00075:                         var loadChoice = loadStage.choices.Find(ch => ch.choiceId == baseChoice.choiceId);
00076:                         Assert.NotNull(loadChoice);
00077:
00078:                         Assert.Equal(baseChoice.choiceId, loadChoice.choiceId);
00079:                         Assert.Equal(baseChoice.text, loadChoice.text);
00080:                         Assert.Equal(baseChoice.nextStageId, loadChoice.nextStageId);
00081:                         Assert.Equal(baseChoice.moraleDelta, loadChoice.moraleDelta);
00082:                         Assert.Equal(baseChoice.guiltDelta, loadChoice.guiltDelta);
00083:                         Assert.Equal(baseChoice.grantItemId ?? string.Empty, loadChoice.grantItemId ?? string.Empty);
00084:                         Assert.Equal(baseChoice.grantItemQuantity, loadChoice.grantItemQuantity);
00085:                         Assert.Equal(baseChoice.targetFactionId ?? string.Empty, loadChoice.targetFactionId ?? string.Empty);
00086:                         Assert.Equal(baseChoice.factionStandingDelta, loadChoice.factionStandingDelta);
00087:                         Assert.Equal(baseChoice.unlockEncounterId ?? string.Empty, loadChoice.unlockEncounterId ?? string.Empty);
00088:                         Assert.Equal(baseChoice.outcomeNarrative, loadChoice.outcomeNarrative);
00089:
00090:                         int baseCondCount = baseChoice.conditions?.Count ?? 0;
00091:                         int loadCondCount = loadChoice.conditions?.Count ?? 0;
00092:                         Assert.Equal(baseCondCount, loadCondCount);
00093:
00094:                         for (int k = 0; k < baseCondCount; k++)
00095:                         {
00096:                             Assert.Equal(baseChoice.conditions[k].conditionTag, loadChoice.conditions[k].conditionTag);
00097:                             Assert.Equal(baseChoice.conditions[k].isBlocker, loadChoice.conditions[k].isBlocker);
00098:                         }
00099:                     }
00100:                 }
00101:             }
00102:         }
00103:
00104:         [Fact]
00105:         public void CanonicalCatalog_ContainsExactlySevenPlan114Questlines()
00106:         {
00107:             string dataDir = FindDataDir();
00108:             var loaded = YearOfAshCatalogLoader.LoadQuestlines(
00109:                 dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00110:
00111:             var expected = new[]
00112:             {
00113:                 "quest_garrison_amnesty_offer",
00114:                 "quest_ash_sign_pilgrimage",
00115:                 "quest_rebuilder_irrigation",
00116:                 "quest_hydro_baron_water_tax",
00117:                 "quest_black_ops_blackmail",
00118:                 "quest_garrison_mutiny",
00119:                 "quest_rebuilder_seed_failure"
00120:             };
00121:
00122:             Assert.Equal(15, loaded.Count);
00123:             foreach (var id in expected)
00124:                 Assert.Contains(loaded, quest => quest.questlineId == id);
00125:         }
00126:
00127:         [Fact]
00128:         public void PilotQuestline_GarrisonBloodDebt_LoadsAndPlaysThroughChoices()
00129:         {
00130:             string dataDir = FindDataDir();
00131:             var fileIO = new FileSystemIO();
00132:             var serializer = new SystemTextJsonSerializer();
00133:
00134:             var quests = YearOfAshCatalogLoader.LoadQuestlines(dataDir, fileIO, serializer);
00135:             var bloodDebt = quests.Find(q => q.questlineId == "quest_garrison_blood_debt");
00136:             Assert.NotNull(bloodDebt);
00137:
00138:             var system = new QuestlineSystem();
00139:             system.RegisterQuestline(bloodDebt);
00140:
00141:             Assert.True(system.StartQuestline("quest_garrison_blood_debt", 185));
00142:             var record = system.State.active.Find(a => a.questlineId == "quest_garrison_blood_debt");
00143:             Assert.NotNull(record);
00144:             Assert.Equal("stage_blood_debt_demand", record.currentStageId);
00145:
00146:             // Choice 1: confront Ola
00147:             var r1 = system.TakeChoice("quest_garrison_blood_debt", "choice_confront_ola", 185);
00148:             Assert.NotNull(r1);
00149:             Assert.Equal("stage_blood_debt_ola_testimony", record.currentStageId);
00150:
00151:             // Choice 2: forge rebuttal
00152:             var r2 = system.TakeChoice("quest_garrison_blood_debt", "choice_forge_tribunal_rebuttal", 187);
00153:             Assert.NotNull(r2);
00154:             Assert.Equal("stage_blood_debt_garrison_bluff", record.currentStageId);
00155:             Assert.Equal("item_falsified_clearance", r2.grantItemId);
00156:
00157:             // Choice 3: pass bluff
00158:             var r3 = system.TakeChoice("quest_garrison_blood_debt", "choice_pass_the_bluff", 200);
00159:             Assert.NotNull(r3);
00160:             Assert.Equal("stage_blood_debt_resolution_protected", record.currentStageId);
00161:             Assert.Equal(QuestlineStatus.Completed, record.status);
00162:         }
00163:     }
00164: }
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **Current evidence and safe integration boundary for Plan 59: Dynamic Questlines, Year of Ashes and Multi-Stage State.**.

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
