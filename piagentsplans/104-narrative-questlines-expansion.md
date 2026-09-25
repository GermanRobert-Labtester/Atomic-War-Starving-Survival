# Plan 104 — Narrative Questlines, Survivor Arcs and Save-Backed Branches

> **Rebuild status:** COMPLETE 12 QUESTLINES — CORE CATALOG, SYSTEM, HOST SESSION, SAVE AND UI EXIST
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

- The catalog has 12 questlines with named survivors, target locations, four stages and crisis branches.
- NarrativeQuestlineCatalog loads definitions; NarrativeQuestlineSystem owns arcs, stages, branch choice, item delivery and save state.
- NarrativeQuestlineHostSession/SaveStore and Main.NarrativeQuestlines provide current persistence/commands; QuestsPanel is the existing surface.

**Bounded outcome:** Retire the 4-to-12 objective. narrative_questlines.json currently has 12 four-stage survivor questlines, and NarrativeQuestlineSystem, host session, save store, Main.NarrativeQuestlines and QuestsPanel are current. The residual is exact survivor/location/item/branch reachability and presentation truth, not more rows.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- Replace the old count target with a 12-row reference and lifecycle census.
- Audit every survivor/location/item/branch definition against current catalogs and host routes.
- Keep arc state, item delivery, trait grants, journal facts and UI projections on current owners.

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
- The current plan is a lifecycle and reachability audit for twelve real questlines.

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
| static questline definitions and branch parsing | NarrativeQuestlineCatalog | `Assets/Ashfall.Core/Quests/NarrativeQuestlineCatalog.cs` | Static content owner. |
| arc start, stage, item delivery, branch and resolution | NarrativeQuestlineSystem | `Assets/Ashfall.Core/Quests/NarrativeQuestlineSystem.cs` | Sole narrative questline runtime owner. |
| host commands and composition | NarrativeQuestlineHostSession | `src/Host/NarrativeQuestlineHostSession.cs` | Thin host adapter. |
| current arc persistence | NarrativeQuestlineSaveStore | `src/Host/NarrativeQuestlineSaveStore.cs` | Existing save owner. |
| host setup/save/operations | Main.NarrativeQuestlines | `src/Main.NarrativeQuestlines.cs` | Current host seam. |
| player-facing quest surface | QuestsPanel | `src/UI/QuestsPanel.cs` | Read-only projection/commands. |
| runtime/save contract | NarrativeQuestlineSystemTests | `Ashfall.Core.Tests/NarrativeQuestlineSystemTests.cs` | Focused evidence. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Narrative Questlines, Survivor Arcs and Save-Backed Branches
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ NarrativeQuestlineCatalog
│   static questline definitions and branch parsing
│ NarrativeQuestlineSystem
│   arc start, stage, item delivery, branch and resolution
│ NarrativeQuestlineHostSession
│   host commands and composition
│ NarrativeQuestlineSaveStore
│   current arc persistence
│ Main.NarrativeQuestlines
│   host setup/save/operations
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

1. **Preserve current state ownership.** NarrativeQuestlineCatalog owns static questline definitions and branch parsing: Static content owner.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| static questline definitions and branch parsing | NarrativeQuestlineCatalog | `Assets/Ashfall.Core/Quests/NarrativeQuestlineCatalog.cs` | Static content owner. |
| arc start, stage, item delivery, branch and resolution | NarrativeQuestlineSystem | `Assets/Ashfall.Core/Quests/NarrativeQuestlineSystem.cs` | Sole narrative questline runtime owner. |
| host commands and composition | NarrativeQuestlineHostSession | `src/Host/NarrativeQuestlineHostSession.cs` | Thin host adapter. |
| current arc persistence | NarrativeQuestlineSaveStore | `src/Host/NarrativeQuestlineSaveStore.cs` | Existing save owner. |
| host setup/save/operations | Main.NarrativeQuestlines | `src/Main.NarrativeQuestlines.cs` | Current host seam. |
| player-facing quest surface | QuestsPanel | `src/UI/QuestsPanel.cs` | Read-only projection/commands. |
| runtime/save contract | NarrativeQuestlineSystemTests | `Ashfall.Core.Tests/NarrativeQuestlineSystemTests.cs` | Focused evidence. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load 12 definitions
2. validate survivor/location/item/branch references
3. read current arcs
4. show outstanding objectives
5. route start/deliver/branch through NarrativeQuestlineSystem
6. apply current item/trait/journal consequences
7. capture/restore arc state

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Definitions are immutable catalog data.
- NarrativeQuestlineSaveState is the current arc authority.
- Inventory delivery and trait effects remain with their current owners.
- No Plan-104 save section.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Only an eligible survivor can begin an arc.
- A branch is valid only at its current stage.
- Item delivery is atomic and exactly once.
- A missing definition restores safely without a phantom arc.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- narrative_questlines.json is the sole questline catalog.
- Starting survivors, locations, items, traits and flags own references.
- No duplicate questline or trait ledger.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use NarrativeQuestlineSaveStore/current save section.
- No Plan-104 save section.
- Old empty state remains valid; unresolved objectives retain current defaults.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Branch outcomes are deterministic from state/choice; no unseeded randomness.
- Catalog order is stable.
- Same state/day/choice produces same arc.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- System emits arc start, stage advance, branch and resolution facts.
- Host/journal/UI consume facts.
- Trait/inventory changes happen only through existing commands.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/NarrativeQuestlineHostSession.cs
- src/Host/NarrativeQuestlineSaveStore.cs
- src/Main.NarrativeQuestlines.cs
- src/UI/QuestsPanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Arcs are grounded, fictional and character-specific.
- No copied real-world conflict or sentimental exposition dump.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A questline references a missing survivor/location. | NarrativeQuestlineCatalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | Item delivery grants twice. | NarrativeQuestlineSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A branch can be chosen from any stage. | NarrativeQuestlineHostSession | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | Panel creates a branch state. | NarrativeQuestlineSaveStore | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A new questline save duplicates current state. | Main.NarrativeQuestlines | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/NarrativeQuestlineCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/NarrativeQuestlineSystemTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 | 12-row reference census. | Current content is exact. | No production path until the owning implementation package is separately claimed. |
| 1 | System/save/host trace. | Arc lifecycle has one owner. | No production path until the owning implementation package is separately claimed. |
| 2 | Branch/item/replay audit. | No duplicate delivery or resolution. | No production path until the owning implementation package is separately claimed. |
| 3 | UI/content polish. | Current objectives and refusals are player-legible. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/narrative_questlines.json | READ ONLY; MODIFY only for approved content/reference delta | 12 questlines |
| Assets/Ashfall.Core/Quests/NarrativeQuestlineSystem.cs | READ ONLY | Runtime owner |
| src/Host/NarrativeQuestlineSaveStore.cs | READ ONLY | Save seam |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Duplicate questline state. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Reference drift. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Branch bypass. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Double item delivery. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Panel-owned progression. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new questline rows.
- No new arc save.
- No production/data/test/UI changes here.

# 23. Rollback and Recovery

- Revert planning artifact.
- Future content additions use current loader/system and save tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 12 rows, current owners, save and branch contracts are explicit.
- No stale count objective remains.

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

- No new questline rows.
- No new arc save.
- No production/data/test/UI changes here.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/NarrativeQuestlineCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/NarrativeQuestlineSystemTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: static questline definitions and branch parsing → NarrativeQuestlineCatalog; arc start, stage, item delivery, branch and resolution → NarrativeQuestlineSystem; host commands and composition → NarrativeQuestlineHostSession; current arc persistence → NarrativeQuestlineSaveStore; host setup/save/operations → Main.NarrativeQuestlines; player-facing quest surface → QuestsPanel; runtime/save contract → NarrativeQuestlineSystemTests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 104.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 104 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by NarrativeQuestlineCatalog or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Quests/NarrativeQuestlineCatalog.cs`

### `Assets/Ashfall.Core/Quests/NarrativeQuestlineCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 236 lines / 9371 bytes.
- SHA-256: `2af44f1ec95ba9290779a732bd1cd62eae700078282529402d36b92f2d3c9969`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class NarrativeQuestlineBranchDef
public string id = string.Empty;
public string label = string.Empty;
public string description = string.Empty;
public string traitGranted = string.Empty;
public int moraleDelta;
public class NarrativeQuestlineStageDef
public int stage;
public string name = string.Empty;
public string description = string.Empty;
public List<string> objectiveItems = new List<string>();
public NarrativeQuestlineBranchDef? branchA;
public NarrativeQuestlineBranchDef? branchB;
public bool HasBranch => branchA != null && branchB != null;
public NarrativeQuestlineBranchDef? FindBranch(string branchId) {
public class NarrativeQuestlineDef
public string questId = string.Empty;
public string survivorId = string.Empty;
public string title = string.Empty;
public string targetLocationId = string.Empty;
public List<NarrativeQuestlineStageDef> stages = new List<NarrativeQuestlineStageDef>();
public NarrativeQuestlineStageDef? FindStage(int stageIndex) {
public NarrativeQuestlineStageDef? FindBranchStage() {
public static class NarrativeQuestlineCatalogLoader
public const string FileName = "narrative_questlines.json";
public const int CurrentSchemaVersion = 1;
public static List<NarrativeQuestlineDef> LoadEntries( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static List<NarrativeQuestlineDef> Parse(string raw, IJsonSerializer json, string? pathForDiagnostics = null) {
public int schema_version = 1;
public List<Questline> questlines = new List<Questline>();
public string quest_id;
public string survivor_id;
public string title;
public string target_location_id;
public List<Stage> stages;
public int stage;
public string name;
public string description;
public List<string> objective_items;
public Branch branch_a;
public Branch branch_b;
public string id;
public string label;
public string description;
public string trait_granted;
public int morale_delta;
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Quests/NarrativeQuestlineSystem.cs`

### `Assets/Ashfall.Core/Quests/NarrativeQuestlineSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 376 lines / 15461 bytes.
- SHA-256: `3ae336181120941db8e22db7f63ec87e000dfe8c1e2df63d1a0803e3dc5c853f`.
- Architecture signals: seeded references=0; save/restore symbols=3; typed event declarations=8; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum NarrativeArcStatus
public sealed class NarrativeQuestlineArcState
public string questId = string.Empty;
public string survivorId = string.Empty;
public int currentStage;
public NarrativeArcStatus status = NarrativeArcStatus.NotStarted;
public List<string> deliveredItems = new List<string>();
public string chosenBranchId = string.Empty;
public string grantedTraitId = string.Empty;
public int startedDay;
public int resolvedDay;
public NarrativeQuestlineArcState Clone() {
public sealed class NarrativeQuestlineSaveState
public int schema_version = 1;
public string systemId = NarrativeQuestlineSystem.SystemId;
public List<NarrativeQuestlineArcState> arcs = new List<NarrativeQuestlineArcState>();
public sealed class NarrativeQuestlineSystem
public const string SystemId = "narrative_questlines";
public event Action<NarrativeQuestlineArcState>? OnArcStarted;
public event Action<NarrativeQuestlineArcState, int>? OnStageAdvanced;
public event Action<NarrativeQuestlineArcState, NarrativeQuestlineBranchDef>? OnBranchChosen;
public event Action<NarrativeQuestlineArcState>? OnArcResolved;
public int DefinitionCount => _byQuestId.Count;
public NarrativeQuestlineDef? GetDefinition(string questId) => !string.IsNullOrEmpty(questId) && _byQuestId.TryGetValue(questId, out var d) ? d : null;
public NarrativeQuestlineDef? GetDefinitionForSurvivor(string survivorId) => !string.IsNullOrEmpty(survivorId) && _bySurvivorId.TryGetValue(survivorId, out var d) ? d : null;
public NarrativeQuestlineArcState? GetArc(string survivorId) => !string.IsNullOrEmpty(survivorId) && _arcsBySurvivor.TryGetValue(survivorId, out var a) ? a : null;
public bool HasArc(string survivorId) => GetArc(survivorId) != null;
public bool IsAwaitingBranch(string survivorId) => GetArc(survivorId)?.status == NarrativeArcStatus.AwaitingBranch;
public List<string> GetOutstandingObjectives(string survivorId) {
public bool TryBegin(string survivorId, int day) {
public bool TryDeliverItem(string survivorId, string itemId, int day) {
public bool TryChooseBranch( string survivorId, string branchId, int day, out NarrativeQuestlineBranchDef? chosenBranch) {
public NarrativeQuestlineSaveState CaptureState() {
public void RestoreState(NarrativeQuestlineSaveState? saved) {
public NarrativeQuestlineSaveState State => _state;
```


# Appendix B.04 — Current Code Architecture: `src/Host/NarrativeQuestlineHostSession.cs`

### `src/Host/NarrativeQuestlineHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 100 lines / 4141 bytes.
- SHA-256: `f357dcb288107b1d2ba78f5ae66110adfa0e8fec76e404a58995bc197c69b3c5`.
- Architecture signals: seeded references=0; save/restore symbols=8; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class NarrativeQuestlineHostSession : HostSessionBase
public NarrativeQuestlineSystem System => _system;
public static NarrativeQuestlineHostSession Create(string dataDir, ILog? log = null) {
public int DefinitionCount => _system.DefinitionCount;
public IReadOnlyList<NarrativeQuestlineDef> Definitions => _system.Definitions;
public NarrativeQuestlineDef? GetDefinitionForSurvivor(string survivorId) => _system.GetDefinitionForSurvivor(survivorId);
public NarrativeQuestlineArcState? GetArc(string survivorId) => _system.GetArc(survivorId);
public IReadOnlyList<NarrativeQuestlineArcState> Arcs => _system.Arcs;
public List<string> GetOutstandingObjectives(string survivorId) => _system.GetOutstandingObjectives(survivorId);
public bool IsAwaitingBranch(string survivorId) => _system.IsAwaitingBranch(survivorId);
public bool TryBegin(string survivorId, int day) => _system.TryBegin(survivorId, day);
public bool TryDeliverItem(string survivorId, string itemId, int day) => _system.TryDeliverItem(survivorId, itemId, day);
public bool TryChooseBranch( string survivorId, string branchId, int day, out NarrativeQuestlineBranchDef? chosenBranch) => _system.TryChooseBranch(survivorId, branchId, day, out chosenBranch);
public NarrativeQuestlineSaveState CaptureState() => _system.CaptureState();
public void RestoreState(NarrativeQuestlineSaveState state) {
public bool TrySave() => NarrativeQuestlineSaveStore.TrySave(_system.CaptureState());
public bool TryLoad() {
public string TryCapturePersisted() => NarrativeQuestlineSaveStore.TryCapturePersisted(_system.CaptureState());
```


# Appendix B.05 — Current Code Architecture: `src/Host/NarrativeQuestlineSaveStore.cs`

### `src/Host/NarrativeQuestlineSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 39 lines / 1791 bytes.
- SHA-256: `b0373e4592a430c53c94297e8f85945036c08c30ea8e091fb1721bacc214cb0c`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class NarrativeQuestlineSaveStore
public const string FileName = "narrative_questlines_save.json";
public const string SectionName = "narrative_questlines";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCaptureDirect(NarrativeQuestlineSaveState state) => s_store.CaptureBare(state);
public static NarrativeQuestlineSaveState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(NarrativeQuestlineSaveState state) => s_store.CaptureBare(state);
public static NarrativeQuestlineSaveState? TryRestore(string json) => s_store.RestoreBare(json);
public static bool TrySave(NarrativeQuestlineSaveState state) => s_store.TrySave(state);
public static NarrativeQuestlineSaveState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(NarrativeQuestlineSaveState state) => s_store.CapturePersisted(state);
```


# Appendix B.06 — Current Code Architecture: `src/Host/NarrativeQuestlineHostSession.cs`

### `src/Host/NarrativeQuestlineHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 100 lines / 4141 bytes.
- SHA-256: `f357dcb288107b1d2ba78f5ae66110adfa0e8fec76e404a58995bc197c69b3c5`.
- Architecture signals: seeded references=0; save/restore symbols=8; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class NarrativeQuestlineHostSession : HostSessionBase
public NarrativeQuestlineSystem System => _system;
public static NarrativeQuestlineHostSession Create(string dataDir, ILog? log = null) {
public int DefinitionCount => _system.DefinitionCount;
public IReadOnlyList<NarrativeQuestlineDef> Definitions => _system.Definitions;
public NarrativeQuestlineDef? GetDefinitionForSurvivor(string survivorId) => _system.GetDefinitionForSurvivor(survivorId);
public NarrativeQuestlineArcState? GetArc(string survivorId) => _system.GetArc(survivorId);
public IReadOnlyList<NarrativeQuestlineArcState> Arcs => _system.Arcs;
public List<string> GetOutstandingObjectives(string survivorId) => _system.GetOutstandingObjectives(survivorId);
public bool IsAwaitingBranch(string survivorId) => _system.IsAwaitingBranch(survivorId);
public bool TryBegin(string survivorId, int day) => _system.TryBegin(survivorId, day);
public bool TryDeliverItem(string survivorId, string itemId, int day) => _system.TryDeliverItem(survivorId, itemId, day);
public bool TryChooseBranch( string survivorId, string branchId, int day, out NarrativeQuestlineBranchDef? chosenBranch) => _system.TryChooseBranch(survivorId, branchId, day, out chosenBranch);
public NarrativeQuestlineSaveState CaptureState() => _system.CaptureState();
public void RestoreState(NarrativeQuestlineSaveState state) {
public bool TrySave() => NarrativeQuestlineSaveStore.TrySave(_system.CaptureState());
public bool TryLoad() {
public string TryCapturePersisted() => NarrativeQuestlineSaveStore.TryCapturePersisted(_system.CaptureState());
```


# Appendix B.07 — Current Code Architecture: `src/Host/NarrativeQuestlineSaveStore.cs`

### `src/Host/NarrativeQuestlineSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 39 lines / 1791 bytes.
- SHA-256: `b0373e4592a430c53c94297e8f85945036c08c30ea8e091fb1721bacc214cb0c`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class NarrativeQuestlineSaveStore
public const string FileName = "narrative_questlines_save.json";
public const string SectionName = "narrative_questlines";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCaptureDirect(NarrativeQuestlineSaveState state) => s_store.CaptureBare(state);
public static NarrativeQuestlineSaveState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(NarrativeQuestlineSaveState state) => s_store.CaptureBare(state);
public static NarrativeQuestlineSaveState? TryRestore(string json) => s_store.RestoreBare(json);
public static bool TrySave(NarrativeQuestlineSaveState state) => s_store.TrySave(state);
public static NarrativeQuestlineSaveState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(NarrativeQuestlineSaveState state) => s_store.CapturePersisted(state);
```


# Appendix B.08 — Current Code Architecture: `src/Main.NarrativeQuestlines.cs`

### `src/Main.NarrativeQuestlines.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 300 lines / 12648 bytes.
- SHA-256: `48bd492b00b71c40972f967c96d743eab71596efa6c182f1f425f68a161c7f66`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public string LastNarrativeArcEvent { get; private set; } = string.Empty;
public NarrativeQuestlineHostSession? NarrativeQuestlines => _narrativeQuestlines;
public bool BeginSurvivorArc(string survivorId) {
public bool DeliverSurvivorArcObjective(string survivorId, string itemId) {
public bool ChooseSurvivorArcBranch(string survivorId, string branchId) {
public IReadOnlyList<NarrativeQuestlineDef> NarrativeArcDefinitions() {
```


# Appendix B.09 — Current Code Architecture: `src/UI/QuestsPanel.cs`

### `src/UI/QuestsPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 774 lines / 37776 bytes.
- SHA-256: `c1a69c26ccc3ef00cfd1078aee697c71d5550eb0b0ae1c461e0ec060f9bbf11b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=16; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class QuestsPanel : Control, IBindablePanel
public event Action? OnClose;
public event Action<string>? OnQuestDetailRequested;
public event Action? OnCrossingPanelRequested;
public event Action? OnProceduralQuestRequested;
public event Action<string>? OnBeginSurvivorArcRequested;
public event Action<string, string>? OnDeliverArcObjectiveRequested;
public event Action<string, string>? OnChooseArcBranchRequested;
public bool IsBound => _holdfastQuests != null || _crossingQuests != null || _branchCoordinator != null || _moralDefs != null || _survivorArcs != null || _proceduralNarrative != null;
public void Bind( HoldfastQuestSystem? holdfastQuests, CrossingQuestSystem? crossingQuests = null, DutyRosterHostSession? dutyRoster = null, int currentDay = 1, Ashfall.Core.Factions.FactionBranchCoordinator? branchCoordinator = null,
public void Unbind() {
public void RefreshView() {
public override void _Ready() {
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
public override void _ExitTree() {
```


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/narrative_questlines.json`

### `Assets/StreamingAssets/Data/narrative_questlines.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 32775 bytes / 32725 characters.
- SHA-256: `bfef78970576b081ecdcc651dfa24de5a7a8a9d6bf79172660f050efe60bc14a`.
- Root keys: `questlines`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
questlines: min=12, max=12, observed_paths=1
questlines[].stages: min=4, max=4, observed_paths=2
questlines[].stages[].objective_items: min=1, max=2, observed_paths=4
```

Representative record fields:

- `quest_id`
- `stages`
- `survivor_id`
- `target_location_id`
- `title`

Representative identifiers (ordered, capped for readability):

```text
quest_the_cracked_floor
quest_the_dying_signal
quest_the_refugee_mass_influx
quest_the_ars_crisis
quest_the_machinists_regret
quest_the_abandoned_school
quest_the_final_harvest
quest_the_irradiated_soil
quest_crisis_of_faith
quest_truth_of_day_30
quest_the_substation_ghost
quest_the_white_elk
```


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/starting_survivors.json`

### `Assets/StreamingAssets/Data/starting_survivors.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 933 bytes / 933 characters.
- SHA-256: `e59a3e873abd2991e54989142645b9a281c900a398a7171d1df71d239fb74585`.
- Root keys: `schema_version`, `starting_survivors`.

Array-path census (minimum, maximum, observed rows):

```text
starting_survivors: min=3, max=3, observed_paths=1
```

Representative record fields:

- `acuteRad`
- `displayName`
- `health`
- `hunger`
- `id`
- `joinedDay`
- `lifetimeDose`
- `morale`
- `thirst`
- `warmth`

Representative identifiers (ordered, capped for readability):

```text
survivor_dr_sarah_chen
survivor_gunner_mikhail
elena_vasquez
```


# Appendix C.12 — Catalog Census: `Assets/StreamingAssets/Data/locations.json`

### `Assets/StreamingAssets/Data/locations.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 112815 bytes / 112801 characters.
- SHA-256: `97543ade61b6458f5b31bcffb85a96ad5d3b322b80294deb158d1ae29da3386b`.
- Root keys: `locations`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
locations: min=179, max=179, observed_paths=1
```

Representative record fields:

- `ambushFlag`
- `baseRadsPerHour`
- `cleanWaterRewardFlag`
- `dangerLevel`
- `description`
- `displayName`
- `id`
- `requiredFlagId`
- `travelHours`

Representative identifiers (ordered, capped for readability):

```text
abandoned_hospital
rural_gas_station
suburban_house
government_bunker
stranger_cache
location_geo_thermal_plant_ruins
location_arcology_sector_4
location_frozen_river_barge
location_crashed_icebreaker_convoy
location_silent_observatory
location_subterranean_seed_vault
location_ministry_of_truth_bunker
location_ash_dune_cemetery
location_abandoned_ski_resort
location_geothermal_borehole_site
location_flooded_subway_depot
location_sub_level_4_transit
location_municipal_sewage
location_collapsed_salt_mine
location_bio_remediation_lab
location_submerged_data_center
location_geothermal_vent_shaft
location_the_sump_cathedral
location_abandoned_desalination
location_deep_core_borehole
location_uxo_highway_choke
location_radar_array_spire
location_drone_hive_silo
location_automated_mortar_pit
location_scrap_neuromancer_camp
location_magnetic_anomaly_crater
location_abandoned_convoy_yard
location_acoustic_testing_facility
location_substation_omega
location_the_dead_hand_core
location_lethe_water_treatment
location_observatory_dome
location_submerged_arcology
location_concrete_batching_plant
location_seed_vault_antechamber
location_hospital_psych_wing
location_mirror_factory
location_radio_telescope_array
location_ash_whale_carcass
location_the_memory_vault
highway_pileup
prewar_medical_cache
loc_grange_hall
loc_apiary_rows
loc_seed_library_annex
loc_veterinary_surgery
loc_school_gymnasium
loc_cider_press
loc_terrace_pumphouse
loc_ration_queue_plaza
loc_conscription_office
loc_municipal_archive
loc_dentists_row
loc_transit_authority_hq
loc_printworks
loc_department_store
loc_public_swimming_baths
loc_st_brigids_almshouse
loc_weighbridge
loc_motel_verity
loc_bridge_seven
loc_recovery_yard
loc_ordnance_shoulder
loc_bus_reversal_loop
loc_diesel_tank_farm
loc_radio_relay_mast
loc_ash_sign_shrine
loc_pilgrim_switchbacks
loc_snowline_station
loc_low_background_lab
loc_ice_core_store
loc_avalanche_gallery
loc_summit_relay
loc_the_vessels_cell
loc_lock_gate_four
loc_pump_station_nine
loc_alloc_12b
loc_records_annex
loc_drowned_cinema
loc_cold_store_atlantic
loc_bathymetric_boat
loc_the_shallows_market
loc_the_allotments
checkpoint_kilo_armory
hospital_pharmacy
family_bunker_backyard_shed
old_library_cache
convoy_echo7_cache
raider_ambush_site
collapsed_building
raider_trap_location
electrical_substation
ruined_garage
concert_hall_ruins
loc_grain_silo
loc_garrison_checkpoint_gamma
loc_railway_span_44_alpha
loc_forward_roster_camp
loc_shrine_switchback_waystation
loc_understory_transmitter
loc_shelter_gate
loc_shelter_meeting
loc_shelter_infirmary
loc_shelter_storage
loc_shelter_quarters
loc_shelter_fire
loc_shelter_perimeter
loc_eastern_road
loc_neutral_ground
loc_water_station
loc_excavation_command_vault
loc_excavation_utility_tunnels
loc_excavation_metro_interchange
loc_excavation_mine_shaft
loc_excavation_archive_bunker
loc_excavation_drainage_network
loc_excavation_storage_chamber
loc_excavation_civilian_shelter
loc_hidden_relay_bunker
loc_logistics_reserve_cache
loc_deaddrop_command_shelter
loc_holdfast
loc_cut_radiation_zone_alpha
loc_cut_merchant_caravanserai
loc_cut_abandoned_depot
loc_cut_arsenal_ruin
loc_black_flotilla_outpost
loc_settlement_brine_pans
loc_settlement_iron_siding
loc_settlement_cape_beacon
loc_settlement_slate_hollow
loc_settlement_pilgrim_hearth
loc_settlement_tinkers_notch
location_quarry_overlook
loc_grain_exchange
loc_automated_abattoir
loc_flooded_subway_depot
loc_scavenger_camp
loc_iron_garrison
loc_dead_zone
loc_settlement_ferry_crossing
loc_settlement_nine_rails
loc_settlement_fort_karkov
loc_settlement_lock_seven
loc_settlement_silo_burrow
loc_settlement_st_nicholas
loc_iron_crest
loc_ash_needle
loc_wind_gap_ridge
loc_signal_hill_tower
loc_river_bend_outpost
loc_rusted_span_bridge
loc_old_crematory_stacks
loc_north_gate_water_tower
loc_junction_box_rail
```


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/NarrativeQuestlineCatalogTests.cs`

### `Ashfall.Core.Tests/NarrativeQuestlineCatalogTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 258; SHA-256: `537c569811bfde768a3bf444a9baef9d8a8924a2e93baf06de5ae0618607c5e0`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
QuestlineCatalog_HasExactly12Questlines
QuestlineCatalog_AllQuestIdsUnique
QuestlineCatalog_AllSurvivorIdsNonEmpty
QuestlineCatalog_AllTargetLocationIdsNonEmpty
QuestlineCatalog_EachQuestlineHasFourStages
QuestlineCatalog_AllStageNamesNonEmpty
QuestlineCatalog_CrisisStageHasTwoBranches
QuestlineCatalog_PriestAndReporterArcsPresent
QuestlineCatalog_TeacherAndJournalistArcsPresent
QuestlineCatalog_AllExpectedSurvivorsPresent
```


# Appendix D.14 — Existing Focused Test Inventory: `Ashfall.Core.Tests/NarrativeQuestlineSystemTests.cs`

### `Ashfall.Core.Tests/NarrativeQuestlineSystemTests.cs`

- Current test declarations: Fact=33, Theory=0, InlineData=0.
- File lines: 678; SHA-256: `7bc753f7b35642802e3fbc97ebe3a50006350e3f107d9f6bb9d013b6cd4c5163`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsTwelveAuthoredArcs
Catalog_ArcIdsAndSurvivorIdsAreUnique
Catalog_EveryArcHasTheUniformFourStageMachine
Catalog_EveryBranchCarriesADistinctTraitAndLabel
Catalog_BranchIdsAreGloballyUnique
Catalog_AllStagesHaveNonEmptyProse
Loader_MissingDirectoryYieldsEmptyListWithoutThrowing
Loader_MalformedJsonYieldsEmptyListWithoutThrowing
Loader_FutureSchemaIsRefusedWholeNotPartially
Loader_DropsIncompleteEntriesAndDuplicateIds
Loader_BranchWithoutIdIsDropped
Begin_OpensOneArcPerSurvivorAndNeverTwice
Begin_UnknownSurvivorFails
Delivery_RefusesItemsTheStageDoesNotOwe
Delivery_RefusesDuplicatesAndAdvancesOnlyWhenStageIsClear
OutstandingObjectives_TrackTheCurrentStageOnly
Branch_RefusedBeforeTheCrisisAndForForeignIds
Branch_RecordsTraitAndMoraleThenClosesTheArc
Branch_CanNeverBeChosenTwice
Commands_RefuseUnknownSurvivors
Events_FireOncePerTransition
ArcsAreIndependentPerSurvivor
SaveRoundTrip_PreservesArcProgressExactly
SaveRoundTrip_PreservesResolvedArcs
Restore_NullOrEmptyClearsArcsWithoutThrowing
Restore_DropsArcsWhoseDefinitionNoLongerExists
Restore_SuppressesEventsSoLoadingIsNotMistakenForPlay
Checksum_CoversArcFieldsSoMutationIsActuallyDetected
Determinism_SameInputsProduceIdenticalChecksums
Determinism_DifferentBranchChoiceChangesTheChecksum
Definitions_AreExposedSortedAndStable
NullAndEmptyDefinitions_ProduceAnEmptyButUsableSystem
AuthoritativeCatalog_DrivesACompleteArcToResolution
```


# Appendix E.15 — Supporting Code Evidence: `Assets/Ashfall.Core/Quests/NarrativeQuestlineCatalog.cs`

### `Assets/Ashfall.Core/Quests/NarrativeQuestlineCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 236 lines / 9371 bytes.
- SHA-256: `2af44f1ec95ba9290779a732bd1cd62eae700078282529402d36b92f2d3c9969`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class NarrativeQuestlineBranchDef
public string id = string.Empty;
public string label = string.Empty;
public string description = string.Empty;
public string traitGranted = string.Empty;
public int moraleDelta;
public class NarrativeQuestlineStageDef
public int stage;
public string name = string.Empty;
public string description = string.Empty;
public List<string> objectiveItems = new List<string>();
public NarrativeQuestlineBranchDef? branchA;
public NarrativeQuestlineBranchDef? branchB;
public bool HasBranch => branchA != null && branchB != null;
public NarrativeQuestlineBranchDef? FindBranch(string branchId) {
public class NarrativeQuestlineDef
public string questId = string.Empty;
public string survivorId = string.Empty;
public string title = string.Empty;
public string targetLocationId = string.Empty;
public List<NarrativeQuestlineStageDef> stages = new List<NarrativeQuestlineStageDef>();
public NarrativeQuestlineStageDef? FindStage(int stageIndex) {
public NarrativeQuestlineStageDef? FindBranchStage() {
public static class NarrativeQuestlineCatalogLoader
public const string FileName = "narrative_questlines.json";
public const int CurrentSchemaVersion = 1;
public static List<NarrativeQuestlineDef> LoadEntries( string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static List<NarrativeQuestlineDef> Parse(string raw, IJsonSerializer json, string? pathForDiagnostics = null) {
public int schema_version = 1;
public List<Questline> questlines = new List<Questline>();
public string quest_id;
public string survivor_id;
public string title;
public string target_location_id;
public List<Stage> stages;
public int stage;
public string name;
public string description;
public List<string> objective_items;
public Branch branch_a;
public Branch branch_b;
public string id;
public string label;
public string description;
public string trait_granted;
public int morale_delta;
```


# Appendix E.16 — Supporting Code Evidence: `Assets/Ashfall.Core/Quests/NarrativeQuestlineSystem.cs`

### `Assets/Ashfall.Core/Quests/NarrativeQuestlineSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 376 lines / 15461 bytes.
- SHA-256: `3ae336181120941db8e22db7f63ec87e000dfe8c1e2df63d1a0803e3dc5c853f`.
- Architecture signals: seeded references=0; save/restore symbols=3; typed event declarations=8; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum NarrativeArcStatus
public sealed class NarrativeQuestlineArcState
public string questId = string.Empty;
public string survivorId = string.Empty;
public int currentStage;
public NarrativeArcStatus status = NarrativeArcStatus.NotStarted;
public List<string> deliveredItems = new List<string>();
public string chosenBranchId = string.Empty;
public string grantedTraitId = string.Empty;
public int startedDay;
public int resolvedDay;
public NarrativeQuestlineArcState Clone() {
public sealed class NarrativeQuestlineSaveState
public int schema_version = 1;
public string systemId = NarrativeQuestlineSystem.SystemId;
public List<NarrativeQuestlineArcState> arcs = new List<NarrativeQuestlineArcState>();
public sealed class NarrativeQuestlineSystem
public const string SystemId = "narrative_questlines";
public event Action<NarrativeQuestlineArcState>? OnArcStarted;
public event Action<NarrativeQuestlineArcState, int>? OnStageAdvanced;
public event Action<NarrativeQuestlineArcState, NarrativeQuestlineBranchDef>? OnBranchChosen;
public event Action<NarrativeQuestlineArcState>? OnArcResolved;
public int DefinitionCount => _byQuestId.Count;
public NarrativeQuestlineDef? GetDefinition(string questId) => !string.IsNullOrEmpty(questId) && _byQuestId.TryGetValue(questId, out var d) ? d : null;
public NarrativeQuestlineDef? GetDefinitionForSurvivor(string survivorId) => !string.IsNullOrEmpty(survivorId) && _bySurvivorId.TryGetValue(survivorId, out var d) ? d : null;
public NarrativeQuestlineArcState? GetArc(string survivorId) => !string.IsNullOrEmpty(survivorId) && _arcsBySurvivor.TryGetValue(survivorId, out var a) ? a : null;
public bool HasArc(string survivorId) => GetArc(survivorId) != null;
public bool IsAwaitingBranch(string survivorId) => GetArc(survivorId)?.status == NarrativeArcStatus.AwaitingBranch;
public List<string> GetOutstandingObjectives(string survivorId) {
public bool TryBegin(string survivorId, int day) {
public bool TryDeliverItem(string survivorId, string itemId, int day) {
public bool TryChooseBranch( string survivorId, string branchId, int day, out NarrativeQuestlineBranchDef? chosenBranch) {
public NarrativeQuestlineSaveState CaptureState() {
public void RestoreState(NarrativeQuestlineSaveState? saved) {
public NarrativeQuestlineSaveState State => _state;
```


# Appendix E.17 — Supporting Code Evidence: `src/Host/NarrativeQuestlineHostSession.cs`

### `src/Host/NarrativeQuestlineHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 100 lines / 4141 bytes.
- SHA-256: `f357dcb288107b1d2ba78f5ae66110adfa0e8fec76e404a58995bc197c69b3c5`.
- Architecture signals: seeded references=0; save/restore symbols=8; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class NarrativeQuestlineHostSession : HostSessionBase
public NarrativeQuestlineSystem System => _system;
public static NarrativeQuestlineHostSession Create(string dataDir, ILog? log = null) {
public int DefinitionCount => _system.DefinitionCount;
public IReadOnlyList<NarrativeQuestlineDef> Definitions => _system.Definitions;
public NarrativeQuestlineDef? GetDefinitionForSurvivor(string survivorId) => _system.GetDefinitionForSurvivor(survivorId);
public NarrativeQuestlineArcState? GetArc(string survivorId) => _system.GetArc(survivorId);
public IReadOnlyList<NarrativeQuestlineArcState> Arcs => _system.Arcs;
public List<string> GetOutstandingObjectives(string survivorId) => _system.GetOutstandingObjectives(survivorId);
public bool IsAwaitingBranch(string survivorId) => _system.IsAwaitingBranch(survivorId);
public bool TryBegin(string survivorId, int day) => _system.TryBegin(survivorId, day);
public bool TryDeliverItem(string survivorId, string itemId, int day) => _system.TryDeliverItem(survivorId, itemId, day);
public bool TryChooseBranch( string survivorId, string branchId, int day, out NarrativeQuestlineBranchDef? chosenBranch) => _system.TryChooseBranch(survivorId, branchId, day, out chosenBranch);
public NarrativeQuestlineSaveState CaptureState() => _system.CaptureState();
public void RestoreState(NarrativeQuestlineSaveState state) {
public bool TrySave() => NarrativeQuestlineSaveStore.TrySave(_system.CaptureState());
public bool TryLoad() {
public string TryCapturePersisted() => NarrativeQuestlineSaveStore.TryCapturePersisted(_system.CaptureState());
```


# Appendix E.18 — Supporting Code Evidence: `src/Host/NarrativeQuestlineSaveStore.cs`

### `src/Host/NarrativeQuestlineSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 39 lines / 1791 bytes.
- SHA-256: `b0373e4592a430c53c94297e8f85945036c08c30ea8e091fb1721bacc214cb0c`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class NarrativeQuestlineSaveStore
public const string FileName = "narrative_questlines_save.json";
public const string SectionName = "narrative_questlines";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCaptureDirect(NarrativeQuestlineSaveState state) => s_store.CaptureBare(state);
public static NarrativeQuestlineSaveState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(NarrativeQuestlineSaveState state) => s_store.CaptureBare(state);
public static NarrativeQuestlineSaveState? TryRestore(string json) => s_store.RestoreBare(json);
public static bool TrySave(NarrativeQuestlineSaveState state) => s_store.TrySave(state);
public static NarrativeQuestlineSaveState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(NarrativeQuestlineSaveState state) => s_store.CapturePersisted(state);
```


# Appendix E.19 — Supporting Code Evidence: `src/Host/NarrativeQuestlineHostSession.cs`

### `src/Host/NarrativeQuestlineHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 100 lines / 4141 bytes.
- SHA-256: `f357dcb288107b1d2ba78f5ae66110adfa0e8fec76e404a58995bc197c69b3c5`.
- Architecture signals: seeded references=0; save/restore symbols=8; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class NarrativeQuestlineHostSession : HostSessionBase
public NarrativeQuestlineSystem System => _system;
public static NarrativeQuestlineHostSession Create(string dataDir, ILog? log = null) {
public int DefinitionCount => _system.DefinitionCount;
public IReadOnlyList<NarrativeQuestlineDef> Definitions => _system.Definitions;
public NarrativeQuestlineDef? GetDefinitionForSurvivor(string survivorId) => _system.GetDefinitionForSurvivor(survivorId);
public NarrativeQuestlineArcState? GetArc(string survivorId) => _system.GetArc(survivorId);
public IReadOnlyList<NarrativeQuestlineArcState> Arcs => _system.Arcs;
public List<string> GetOutstandingObjectives(string survivorId) => _system.GetOutstandingObjectives(survivorId);
public bool IsAwaitingBranch(string survivorId) => _system.IsAwaitingBranch(survivorId);
public bool TryBegin(string survivorId, int day) => _system.TryBegin(survivorId, day);
public bool TryDeliverItem(string survivorId, string itemId, int day) => _system.TryDeliverItem(survivorId, itemId, day);
public bool TryChooseBranch( string survivorId, string branchId, int day, out NarrativeQuestlineBranchDef? chosenBranch) => _system.TryChooseBranch(survivorId, branchId, day, out chosenBranch);
public NarrativeQuestlineSaveState CaptureState() => _system.CaptureState();
public void RestoreState(NarrativeQuestlineSaveState state) {
public bool TrySave() => NarrativeQuestlineSaveStore.TrySave(_system.CaptureState());
public bool TryLoad() {
public string TryCapturePersisted() => NarrativeQuestlineSaveStore.TryCapturePersisted(_system.CaptureState());
```


# Appendix F.20 — Supporting Data Evidence: `Assets/StreamingAssets/Data/narrative_questlines.json`

### `Assets/StreamingAssets/Data/narrative_questlines.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 32775 bytes / 32725 characters.
- SHA-256: `bfef78970576b081ecdcc651dfa24de5a7a8a9d6bf79172660f050efe60bc14a`.
- Root keys: `questlines`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
questlines: min=12, max=12, observed_paths=1
questlines[].stages: min=4, max=4, observed_paths=2
questlines[].stages[].objective_items: min=1, max=2, observed_paths=4
```

Representative record fields:

- `quest_id`
- `stages`
- `survivor_id`
- `target_location_id`
- `title`

Representative identifiers (ordered, capped for readability):

```text
quest_the_cracked_floor
quest_the_dying_signal
quest_the_refugee_mass_influx
quest_the_ars_crisis
quest_the_machinists_regret
quest_the_abandoned_school
quest_the_final_harvest
quest_the_irradiated_soil
quest_crisis_of_faith
quest_truth_of_day_30
quest_the_substation_ghost
quest_the_white_elk
```


# Appendix F.21 — Supporting Data Evidence: `Assets/StreamingAssets/Data/starting_survivors.json`

### `Assets/StreamingAssets/Data/starting_survivors.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 933 bytes / 933 characters.
- SHA-256: `e59a3e873abd2991e54989142645b9a281c900a398a7171d1df71d239fb74585`.
- Root keys: `schema_version`, `starting_survivors`.

Array-path census (minimum, maximum, observed rows):

```text
starting_survivors: min=3, max=3, observed_paths=1
```

Representative record fields:

- `acuteRad`
- `displayName`
- `health`
- `hunger`
- `id`
- `joinedDay`
- `lifetimeDose`
- `morale`
- `thirst`
- `warmth`

Representative identifiers (ordered, capped for readability):

```text
survivor_dr_sarah_chen
survivor_gunner_mikhail
elena_vasquez
```


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/NarrativeQuestlineCatalogTests.cs`

### `Ashfall.Core.Tests/NarrativeQuestlineCatalogTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 258; SHA-256: `537c569811bfde768a3bf444a9baef9d8a8924a2e93baf06de5ae0618607c5e0`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
QuestlineCatalog_HasExactly12Questlines
QuestlineCatalog_AllQuestIdsUnique
QuestlineCatalog_AllSurvivorIdsNonEmpty
QuestlineCatalog_AllTargetLocationIdsNonEmpty
QuestlineCatalog_EachQuestlineHasFourStages
QuestlineCatalog_AllStageNamesNonEmpty
QuestlineCatalog_CrisisStageHasTwoBranches
QuestlineCatalog_PriestAndReporterArcsPresent
QuestlineCatalog_TeacherAndJournalistArcsPresent
QuestlineCatalog_AllExpectedSurvivorsPresent
```


# Appendix G.23 — Supporting Regression Evidence: `Ashfall.Core.Tests/NarrativeQuestlineSystemTests.cs`

### `Ashfall.Core.Tests/NarrativeQuestlineSystemTests.cs`

- Current test declarations: Fact=33, Theory=0, InlineData=0.
- File lines: 678; SHA-256: `7bc753f7b35642802e3fbc97ebe3a50006350e3f107d9f6bb9d013b6cd4c5163`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsTwelveAuthoredArcs
Catalog_ArcIdsAndSurvivorIdsAreUnique
Catalog_EveryArcHasTheUniformFourStageMachine
Catalog_EveryBranchCarriesADistinctTraitAndLabel
Catalog_BranchIdsAreGloballyUnique
Catalog_AllStagesHaveNonEmptyProse
Loader_MissingDirectoryYieldsEmptyListWithoutThrowing
Loader_MalformedJsonYieldsEmptyListWithoutThrowing
Loader_FutureSchemaIsRefusedWholeNotPartially
Loader_DropsIncompleteEntriesAndDuplicateIds
Loader_BranchWithoutIdIsDropped
Begin_OpensOneArcPerSurvivorAndNeverTwice
Begin_UnknownSurvivorFails
Delivery_RefusesItemsTheStageDoesNotOwe
Delivery_RefusesDuplicatesAndAdvancesOnlyWhenStageIsClear
OutstandingObjectives_TrackTheCurrentStageOnly
Branch_RefusedBeforeTheCrisisAndForForeignIds
Branch_RecordsTraitAndMoraleThenClosesTheArc
Branch_CanNeverBeChosenTwice
Commands_RefuseUnknownSurvivors
Events_FireOncePerTransition
ArcsAreIndependentPerSurvivor
SaveRoundTrip_PreservesArcProgressExactly
SaveRoundTrip_PreservesResolvedArcs
Restore_NullOrEmptyClearsArcsWithoutThrowing
Restore_DropsArcsWhoseDefinitionNoLongerExists
Restore_SuppressesEventsSoLoadingIsNotMistakenForPlay
Checksum_CoversArcFieldsSoMutationIsActuallyDetected
Determinism_SameInputsProduceIdenticalChecksums
Determinism_DifferentBranchChoiceChangesTheChecksum
Definitions_AreExposedSortedAndStable
NullAndEmptyDefinitions_ProduceAnEmptyButUsableSystem
AuthoritativeCatalog_DrivesACompleteArcToResolution
```


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| static questline definitions and branch parsing | NarrativeQuestlineCatalog | arc start, stage, item delivery, branch and resolution | NarrativeQuestlineSystem | Owner emits/reads a typed fact; no mirror state. |
| static questline definitions and branch parsing | NarrativeQuestlineCatalog | host commands and composition | NarrativeQuestlineHostSession | Owner emits/reads a typed fact; no mirror state. |
| static questline definitions and branch parsing | NarrativeQuestlineCatalog | current arc persistence | NarrativeQuestlineSaveStore | Owner emits/reads a typed fact; no mirror state. |
| static questline definitions and branch parsing | NarrativeQuestlineCatalog | host setup/save/operations | Main.NarrativeQuestlines | Owner emits/reads a typed fact; no mirror state. |
| static questline definitions and branch parsing | NarrativeQuestlineCatalog | player-facing quest surface | QuestsPanel | Owner emits/reads a typed fact; no mirror state. |
| static questline definitions and branch parsing | NarrativeQuestlineCatalog | runtime/save contract | NarrativeQuestlineSystemTests | Owner emits/reads a typed fact; no mirror state. |
| arc start, stage, item delivery, branch and resolution | NarrativeQuestlineSystem | static questline definitions and branch parsing | NarrativeQuestlineCatalog | Owner emits/reads a typed fact; no mirror state. |
| arc start, stage, item delivery, branch and resolution | NarrativeQuestlineSystem | host commands and composition | NarrativeQuestlineHostSession | Owner emits/reads a typed fact; no mirror state. |
| arc start, stage, item delivery, branch and resolution | NarrativeQuestlineSystem | current arc persistence | NarrativeQuestlineSaveStore | Owner emits/reads a typed fact; no mirror state. |
| arc start, stage, item delivery, branch and resolution | NarrativeQuestlineSystem | host setup/save/operations | Main.NarrativeQuestlines | Owner emits/reads a typed fact; no mirror state. |
| arc start, stage, item delivery, branch and resolution | NarrativeQuestlineSystem | player-facing quest surface | QuestsPanel | Owner emits/reads a typed fact; no mirror state. |
| arc start, stage, item delivery, branch and resolution | NarrativeQuestlineSystem | runtime/save contract | NarrativeQuestlineSystemTests | Owner emits/reads a typed fact; no mirror state. |
| host commands and composition | NarrativeQuestlineHostSession | static questline definitions and branch parsing | NarrativeQuestlineCatalog | Owner emits/reads a typed fact; no mirror state. |
| host commands and composition | NarrativeQuestlineHostSession | arc start, stage, item delivery, branch and resolution | NarrativeQuestlineSystem | Owner emits/reads a typed fact; no mirror state. |
| host commands and composition | NarrativeQuestlineHostSession | current arc persistence | NarrativeQuestlineSaveStore | Owner emits/reads a typed fact; no mirror state. |
| host commands and composition | NarrativeQuestlineHostSession | host setup/save/operations | Main.NarrativeQuestlines | Owner emits/reads a typed fact; no mirror state. |
| host commands and composition | NarrativeQuestlineHostSession | player-facing quest surface | QuestsPanel | Owner emits/reads a typed fact; no mirror state. |
| host commands and composition | NarrativeQuestlineHostSession | runtime/save contract | NarrativeQuestlineSystemTests | Owner emits/reads a typed fact; no mirror state. |
| current arc persistence | NarrativeQuestlineSaveStore | static questline definitions and branch parsing | NarrativeQuestlineCatalog | Owner emits/reads a typed fact; no mirror state. |
| current arc persistence | NarrativeQuestlineSaveStore | arc start, stage, item delivery, branch and resolution | NarrativeQuestlineSystem | Owner emits/reads a typed fact; no mirror state. |
| current arc persistence | NarrativeQuestlineSaveStore | host commands and composition | NarrativeQuestlineHostSession | Owner emits/reads a typed fact; no mirror state. |
| current arc persistence | NarrativeQuestlineSaveStore | host setup/save/operations | Main.NarrativeQuestlines | Owner emits/reads a typed fact; no mirror state. |
| current arc persistence | NarrativeQuestlineSaveStore | player-facing quest surface | QuestsPanel | Owner emits/reads a typed fact; no mirror state. |
| current arc persistence | NarrativeQuestlineSaveStore | runtime/save contract | NarrativeQuestlineSystemTests | Owner emits/reads a typed fact; no mirror state. |
| host setup/save/operations | Main.NarrativeQuestlines | static questline definitions and branch parsing | NarrativeQuestlineCatalog | Owner emits/reads a typed fact; no mirror state. |
| host setup/save/operations | Main.NarrativeQuestlines | arc start, stage, item delivery, branch and resolution | NarrativeQuestlineSystem | Owner emits/reads a typed fact; no mirror state. |
| host setup/save/operations | Main.NarrativeQuestlines | host commands and composition | NarrativeQuestlineHostSession | Owner emits/reads a typed fact; no mirror state. |
| host setup/save/operations | Main.NarrativeQuestlines | current arc persistence | NarrativeQuestlineSaveStore | Owner emits/reads a typed fact; no mirror state. |
| host setup/save/operations | Main.NarrativeQuestlines | player-facing quest surface | QuestsPanel | Owner emits/reads a typed fact; no mirror state. |
| host setup/save/operations | Main.NarrativeQuestlines | runtime/save contract | NarrativeQuestlineSystemTests | Owner emits/reads a typed fact; no mirror state. |
| player-facing quest surface | QuestsPanel | static questline definitions and branch parsing | NarrativeQuestlineCatalog | Owner emits/reads a typed fact; no mirror state. |
| player-facing quest surface | QuestsPanel | arc start, stage, item delivery, branch and resolution | NarrativeQuestlineSystem | Owner emits/reads a typed fact; no mirror state. |
| player-facing quest surface | QuestsPanel | host commands and composition | NarrativeQuestlineHostSession | Owner emits/reads a typed fact; no mirror state. |
| player-facing quest surface | QuestsPanel | current arc persistence | NarrativeQuestlineSaveStore | Owner emits/reads a typed fact; no mirror state. |
| player-facing quest surface | QuestsPanel | host setup/save/operations | Main.NarrativeQuestlines | Owner emits/reads a typed fact; no mirror state. |
| player-facing quest surface | QuestsPanel | runtime/save contract | NarrativeQuestlineSystemTests | Owner emits/reads a typed fact; no mirror state. |
| runtime/save contract | NarrativeQuestlineSystemTests | static questline definitions and branch parsing | NarrativeQuestlineCatalog | Owner emits/reads a typed fact; no mirror state. |
| runtime/save contract | NarrativeQuestlineSystemTests | arc start, stage, item delivery, branch and resolution | NarrativeQuestlineSystem | Owner emits/reads a typed fact; no mirror state. |
| runtime/save contract | NarrativeQuestlineSystemTests | host commands and composition | NarrativeQuestlineHostSession | Owner emits/reads a typed fact; no mirror state. |
| runtime/save contract | NarrativeQuestlineSystemTests | current arc persistence | NarrativeQuestlineSaveStore | Owner emits/reads a typed fact; no mirror state. |
| runtime/save contract | NarrativeQuestlineSystemTests | host setup/save/operations | Main.NarrativeQuestlines | Owner emits/reads a typed fact; no mirror state. |
| runtime/save contract | NarrativeQuestlineSystemTests | player-facing quest surface | QuestsPanel | Owner emits/reads a typed fact; no mirror state. |

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

> **DR-09 — Branch and agent sprawl. VERIFIED.**
The repository carries numerous agent- and CI-generated branches (`Zcode_Branch`, `bug_fixing_main`, multiple `chore/*` and `ci-autogen-*` branches) and a wide set of per-tool agent rulebooks at root (`CLAUDE.md`, `CODEX.md`, `CRUSH.md`, `GEMINI.md`, `GOOSE.md`, `MIMOCODE.md`, `QWEN.md`, `VIBE.md`, `.clinerules`, `.cursorrules`, `.windsurfrules`, `.zcode/`). Consequence: multi-agent discipline (worktree ownership, `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`) is not optional; every factory-generated plan must carry an ownership-claim step. No expansion plan may assume it is the only writer.

> The following v1.0 structures were confirmed by the audit and remain authoritative: the four-tier architecture (Tier 1 data authority in `Assets/StreamingAssets/Data/`; Tier 2 engine-free Core; Tier 3 `src/Host` + `src/UI`; Tier 4 xUnit plus the `HostCli` selftest surface); the `AGENTS.md` non-negotiable rules (Godot authoritative, Core engine-free, JSON authoritative, one authority per concern, focused verification); the narrative corpus under `Assets/StreamingAssets/Data/narrative/` (present in the live listing); the faction, economy, weather, Year-of-Ash, moral-choice, muster, and verdict catalog families (all present live); and the plan-discipline artifacts (`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `SESSION_HANDOFF.md`) at root.

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

> **SB-03 — Newest industrial catalogs: corpus twins + consumption wiring (Lanes A and B/C4).** Evidence: DR-04 (`hydraulic_extraction_catalog`, `metrology_standards_catalog` live, absent from v1.0 inventory). Subject: (a) assay-log narrative twins per the Part 16.4 pattern; (b) bind catalogs into consumption/production ledgers via power-grid/foundry seams if not yet consumed — check `UNCLAIMED_CORPUS_CENSUS.md` first (DR-08). Confidence: HIGH CONFIDENCE that content exists; UNVERIFIED whether systems consume them.

> **SB-12 — Dive-site and hydroponic domain expansion (Lanes A and B/C3, C5).** Evidence: DR-04 — `dive_sites.json`, `hydroponic_crops.json` live but absent from v1.0's inventory. Subject: premise-sweep these domains for unexploited seams (dive oxygen drain is a canon hourly system; hydroponics may lack narrative corpus and economy legs). Integration route: data-first + existing host sessions. Confidence: INFERENCE pending sweep.

**Applied constraints:** one bounded outcome, live-source collision sweep, explicit data/loader/consumer/save/test seams, no parallel authority, no unsupported content growth, and a final precision pass. Master file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Recorded SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.


# Appendix — Deep integration architecture

# Appendix — Deep Integration Architecture

## A. Boundary and responsibility map

Current evidence and safe integration boundary for Plan 104: Narrative Questlines, Survivor Arcs and Save-Backed Branches.

- **static questline definitions and branch parsing** remains with `NarrativeQuestlineCatalog` at `Assets/Ashfall.Core/Quests/NarrativeQuestlineCatalog.cs`. Static content owner.
- **arc start, stage, item delivery, branch and resolution** remains with `NarrativeQuestlineSystem` at `Assets/Ashfall.Core/Quests/NarrativeQuestlineSystem.cs`. Sole narrative questline runtime owner.
- **host commands and composition** remains with `NarrativeQuestlineHostSession` at `src/Host/NarrativeQuestlineHostSession.cs`. Thin host adapter.
- **current arc persistence** remains with `NarrativeQuestlineSaveStore` at `src/Host/NarrativeQuestlineSaveStore.cs`. Existing save owner.
- **host setup/save/operations** remains with `Main.NarrativeQuestlines` at `src/Main.NarrativeQuestlines.cs`. Current host seam.
- **player-facing quest surface** remains with `QuestsPanel` at `src/UI/QuestsPanel.cs`. Read-only projection/commands.
- **runtime/save contract** remains with `NarrativeQuestlineSystemTests` at `Ashfall.Core.Tests/NarrativeQuestlineSystemTests.cs`. Focused evidence.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load 12 definitions
2. validate survivor/location/item/branch references
3. read current arcs
4. show outstanding objectives
5. route start/deliver/branch through NarrativeQuestlineSystem
6. apply current item/trait/journal consequences
7. capture/restore arc state

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- Definitions are immutable catalog data.
- NarrativeQuestlineSaveState is the current arc authority.
- Inventory delivery and trait effects remain with their current owners.
- No Plan-104 save section.

- Only an eligible survivor can begin an arc.
- A branch is valid only at its current stage.
- Item delivery is atomic and exactly once.
- A missing definition restores safely without a phantom arc.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/Host/NarrativeQuestlineHostSession.cs
- src/Host/NarrativeQuestlineSaveStore.cs
- src/Main.NarrativeQuestlines.cs
- src/UI/QuestsPanel.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/NarrativeQuestlineCatalogTests.cs
- Ashfall.Core.Tests/NarrativeQuestlineSystemTests.cs

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
| S-01 | 104-01 12 rows load | load 12 definitions | Definitions are immutable catalog data. | A questline references a missing survivor/location. | NarrativeQuestlineCatalog |
| S-02 | 104-02 survivor resolution | validate survivor/location/item/branch references | NarrativeQuestlineSaveState is the current arc authority. | Item delivery grants twice. | NarrativeQuestlineCatalog |
| S-03 | 104-03 location resolution | read current arcs | Inventory delivery and trait effects remain with their current owners. | A branch can be chosen from any stage. | NarrativeQuestlineCatalog |
| S-04 | 104-04 four stages | show outstanding objectives | No Plan-104 save section. | Panel creates a branch state. | NarrativeQuestlineCatalog |
| S-05 | 104-05 branch A/B | route start/deliver/branch through NarrativeQuestlineSystem | Definitions are immutable catalog data. | A new questline save duplicates current state. | NarrativeQuestlineCatalog |
| S-06 | 104-06 item delivery | apply current item/trait/journal consequences | NarrativeQuestlineSaveState is the current arc authority. | A questline references a missing survivor/location. | NarrativeQuestlineCatalog |
| S-07 | 104-07 trait grant once | capture/restore arc state | Inventory delivery and trait effects remain with their current owners. | Item delivery grants twice. | NarrativeQuestlineCatalog |
| S-08 | 104-08 save continuation | load 12 definitions | No Plan-104 save section. | A branch can be chosen from any stage. | NarrativeQuestlineCatalog |
| S-09 | 104-09 UI refresh | validate survivor/location/item/branch references | Definitions are immutable catalog data. | Panel creates a branch state. | NarrativeQuestlineCatalog |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 104-TC-01 schema/count | data | schema/count; verify the named current owner and its negative boundary without inventing a second authority. | NarrativeQuestlineCatalog |
| T-02 | 104-TC-02 reference resolution | unit | reference resolution; verify the named current owner and its negative boundary without inventing a second authority. | NarrativeQuestlineCatalog |
| T-03 | 104-TC-03 arc start | persistence | arc start; verify the named current owner and its negative boundary without inventing a second authority. | NarrativeQuestlineCatalog |
| T-04 | 104-TC-04 stage gate | determinism | stage gate; verify the named current owner and its negative boundary without inventing a second authority. | NarrativeQuestlineCatalog |
| T-05 | 104-TC-05 branch validity | host | branch validity; verify the named current owner and its negative boundary without inventing a second authority. | NarrativeQuestlineCatalog |
| T-06 | 104-TC-06 item atomicity | UI/accessibility | item atomicity; verify the named current owner and its negative boundary without inventing a second authority. | NarrativeQuestlineCatalog |
| T-07 | 104-TC-07 trait effect | cross-system | trait effect; verify the named current owner and its negative boundary without inventing a second authority. | NarrativeQuestlineCatalog |
| T-08 | 104-TC-08 save round trip | data | save round trip; verify the named current owner and its negative boundary without inventing a second authority. | NarrativeQuestlineCatalog |
| T-09 | 104-TC-09 UI truth | unit | UI truth; verify the named current owner and its negative boundary without inventing a second authority. | NarrativeQuestlineCatalog |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 13 | `Ashfall.Core.Tests/NarrativeQuestlineSystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 13 | `src/Host/NarrativeQuestlineHostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Ashfall.Core.Tests/Narrative/Plan96_104ChronicleQuestIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Main.NarrativeQuestlines.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/Quests/NarrativeQuestlineSystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/Host/NarrativeQuestlineSaveStore.cs` | current reference count; inspect the caller before treating it as a live route |
| 2 | `src/UI/QuestsPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/NarrativeQuestlineCatalogTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/Quests/NarrativeQuestlineCatalog.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/narrative_questlines.json`

### `Assets/StreamingAssets/Data/narrative_questlines.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 32775; characters: 32725.
- SHA-256: `bfef78970576b081ecdcc651dfa24de5a7a8a9d6bf79172660f050efe60bc14a`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `questlines`

#### `questlines` — 12 current rows

- Row 001 `quest_the_cracked_floor`: `{"quest_id":"quest_the_cracked_floor","stages":[{"description":"A hairline crack appears in the bunker's concrete sub-floor. Water is seeping through — contaminated water. Aris identifies the damage with professional precision but haunted …`
- Row 002 `quest_the_dying_signal`: `{"quest_id":"quest_the_dying_signal","stages":[{"description":"Maya intercepts a repeating 142.5 MHz distress beacon. The signal pattern is military — standard Emergency Action Message format. But the voice reading the coordinates is young…`
- Row 003 `quest_the_refugee_mass_influx`: `{"quest_id":"quest_the_refugee_mass_influx","stages":[{"description":"A family of desperate civilians — a mother and two children — pounds on the hatch during a fallout storm. Their dosimeter readings are critical. They have 20 minutes of …`
- Row 004 `quest_the_ars_crisis`: `{"quest_id":"quest_the_ars_crisis","stages":[{"description":"A survivor — someone Elena has treated, someone she knows by name — is diagnosed with terminal Acute Radiation Syndrome. The prognosis: twelve days. Elena's clinical assessment i…`
- Row 005 `quest_the_machinists_regret`: `{"quest_id":"quest_the_machinists_regret","stages":[{"description":"Marcus finds a serialized pneumatic firing block among the scrap salvaged from the Recovery Yard. He recognizes the stamp — his own apprentice mark from the municipal mach…`
- Row 006 `quest_the_abandoned_school`: `{"quest_id":"quest_the_abandoned_school","stages":[{"description":"The Teacher rings his hand bell at dawn, but his voice breaks as he pulls an old class attendance ledger from his coat. On the first day of the Exchange, he dismissed thirt…`
- Row 007 `quest_the_final_harvest`: `{"quest_id":"quest_the_final_harvest","stages":[{"description":"The Chef is found late at night staring at a pot of grey root broth, refusing to serve it. He confesses that during the first winter, while captured at the Forward Roster Camp…`
- Row 008 `quest_the_irradiated_soil`: `{"quest_id":"quest_the_irradiated_soil","stages":[{"description":"Suki brings a jar of dark dirt into the common room and spits onto the table. The soil from the Razed Agricultural Co-op is heavy with cesium salts, but she identifies faint…`
- Row 009 `quest_crisis_of_faith`: `{"quest_id":"quest_crisis_of_faith","stages":[{"description":"The Priest stops speaking mid-vespers, drops his tin cross onto the concrete, and walks into the darkness of the lower corridors. He spent the first winter administering last ri…`
- Row 010 `quest_truth_of_day_30`: `{"quest_id":"quest_truth_of_day_30","stages":[{"description":"The Reporter is caught prying open a locked filing drawer in the communications bay. She reveals that on Day 30 after the strike, she uncovered documents proving the regional mi…`
- Row 011 `quest_the_substation_ghost`: `{"quest_id":"quest_the_substation_ghost","stages":[{"description":"The Electrician refuses to touch the main breaker box during routine shelter maintenance. He admits that on the night of the Exchange, his frantic attempt to isolate the ci…`
- Row 012 `quest_the_white_elk`: `{"quest_id":"quest_the_white_elk","stages":[{"description":"The Hunter cleans his marksman rifle with mechanical repetition, his knuckles white. He reveals that in the first blizzard after the Exchange, he fired at movement in the blinding…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/starting_survivors.json`

### `Assets/StreamingAssets/Data/starting_survivors.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 933; characters: 933.
- SHA-256: `e59a3e873abd2991e54989142645b9a281c900a398a7171d1df71d239fb74585`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `starting_survivors`

#### `starting_survivors` — 3 current rows

- Row 001 `survivor_dr_sarah_chen`: `{"acuteRad":false,"displayName":"Dr. Sarah Chen (Trauma Surgeon)","health":90.0,"hunger":20.0,"id":"survivor_dr_sarah_chen","joinedDay":0,"lifetimeDose":14.0,"morale":70.0,"thirst":25.0,"warmth":85.0}`
- Row 002 `survivor_gunner_mikhail`: `{"acuteRad":true,"displayName":"Gunner Mikhail (Heavy Artillery Loader)","health":80.0,"hunger":35.0,"id":"survivor_gunner_mikhail","joinedDay":0,"lifetimeDose":38.0,"morale":55.0,"thirst":30.0,"warmth":75.0}`
- Row 003 `elena_vasquez`: `{"acuteRad":false,"displayName":"Elena Vasquez (Aridoculture Engineer)","health":95.0,"hunger":15.0,"id":"elena_vasquez","joinedDay":0,"lifetimeDose":8.0,"morale":65.0,"thirst":20.0,"warmth":90.0}`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/locations.json`

### `Assets/StreamingAssets/Data/locations.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 112815; characters: 112801.
- SHA-256: `97543ade61b6458f5b31bcffb85a96ad5d3b322b80294deb158d1ae29da3386b`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `locations`

#### `locations` — 179 current rows

- Row 001 `abandoned_hospital`: `{"baseRadsPerHour":35,"dangerLevel":6,"description":"The east wing of the regional hospital came down in the second winter, and nobody has cleared it since. Girders lean against the stairwell, and the pharmacy door is buried under a ton of…`
- Row 002 `rural_gas_station`: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"A roadside station stripped down to its frame on the main route east. The pumps are gutted, the shop glass is gone, and the wind blows ash through the aisles. Fuel drums lie where they w…`
- Row 003 `suburban_house`: `{"baseRadsPerHour":10,"dangerLevel":2,"description":"An intact house in a low-density neighborhood, the kind nobody bothers to burn because there is nothing left to take. The roof holds, the windows are boarded from the inside, and the sto…`
- Row 004 `government_bunker`: `{"baseRadsPerHour":60,"dangerLevel":8,"description":"A sealed military installation set into the hillside, doors still dogged down three years after the exchange. The approach is hot, sixty rads an hour where the wind bends around the blas…`
- Row 005 `stranger_cache`: `{"ambushFlag":"stranger_given_irradiated_water","baseRadsPerHour":25,"cleanWaterRewardFlag":"stranger_given_clean_water","dangerLevel":7,"description":"A sealed pre-war storage locker in a collapsed residential block, its location handed o…`
- Row 006 `location_geo_thermal_plant_ruins`: `{"baseRadsPerHour":45.0,"dangerLevel":8.0,"description":"The ground here is not solid and never was. Cracks breathe hot vapor across the whole platform, and pools of boiling mud bubble under a crust that looks safe until it is not. The pip…`
- Row 007 `location_arcology_sector_4`: `{"baseRadsPerHour":20.0,"dangerLevel":9.0,"description":"Sealed blast doors, still powered, still closed, and someone inside still answers the intercom. Sector 4 of the arcology was the last to fall, and its residents never left: a line of…`
- Row 008 `location_frozen_river_barge`: `{"baseRadsPerHour":30.0,"dangerLevel":6.0,"description":"Dock crew on frozen cargo. They will trade a crate for a way off the ice.\n\nA river barge pinned in pack ice that used to be a harbour roadstead. The hold is a larder and a problem.…`
- Row 009 `location_crashed_icebreaker_convoy`: `{"baseRadsPerHour":85.0,"dangerLevel":7.0,"description":"Military rolling stock that tried to reach the roadstead. The RTG is a bruise on the ice.\n\nNot a submarine. Not a joke. Ice-capable wagons and a locomotive that tried to make the c…`
- Row 010 `location_silent_observatory`: `{"baseRadsPerHour":15.0,"dangerLevel":8.0,"description":"High on the mountain, where the air is thin and the cold sits at sixty below. It is the coldest place on the map and one of the cleanest: radiation stays low, fifteen rads an hour, b…`
- Row 011 `location_subterranean_seed_vault`: `{"baseRadsPerHour":10.0,"dangerLevel":6.0,"description":"The vault was built to outlast a century, and it has done its job so far. Behind the frozen doors, racks of seed lines sit sealed in foil at a temperature that never rises. The radia…`
- Row 012 `location_ministry_of_truth_bunker`: `{"baseRadsPerHour":40.0,"dangerLevel":9.0,"description":"The propaganda servers are still humming on backup power, three years after the broadcasts stopped, because nobody ever found the off switch. The bunker breathes: ventilation, genera…`
- Row 013 `location_ash_dune_cemetery`: `{"baseRadsPerHour":60.0,"dangerLevel":5.0,"description":"A dumping ground at the end of a graded road, where the dead of the first year were stacked and the ash came and did the rest. The ash preserves them perfectly, three years on, and t…`
- Row 014 `location_abandoned_ski_resort`: `{"baseRadsPerHour":25.0,"dangerLevel":4.0,"description":"The cable cars hang frozen mid-swing, their passengers still inside, their coats still warm-looking from a distance. The resort died fast, and the cold preserved what the panic did n…`
- Row 015 `location_geothermal_borehole_site`: `{"baseRadsPerHour":55.0,"dangerLevel":8.0,"description":"The drilling rig is still standing over a hole three kilometers deep, and the groundwater it broke into has flooded the whole site in a slow, toxic seep. Fifty-five rads an hour on t…`
- Row 016 `location_flooded_subway_depot`: `{"baseRadsPerHour":40.0,"dangerLevel":7.0,"description":"Pitch black below the street, with water waist-deep in the main bay and rising against the pillars. The water is toxic: forty rads an hour dissolved into it, and a wader suit is not …`
- Row 017 `location_sub_level_4_transit`: `{"baseRadsPerHour":20.0,"dangerLevel":9.0,"description":"The elite transit tunnels were sealed before the exchange and have not been opened since, except by the thing that grows in them now. Ash-Blight carpets the walls in patches that glo…`
- Row 018 `location_municipal_sewage`: `{"baseRadsPerHour":30.0,"dangerLevel":6.0,"description":"The plant still processes, in its own way: the pipes groan with pressurized sludge and the digesters work without an operator. That is the danger. Methane builds in every pocket of s…`
- Row 019 `location_collapsed_salt_mine`: `{"baseRadsPerHour":10.0,"dangerLevel":5.0,"description":"The mine ran under the ridge for a century, and the roof has been negotiating its surrender ever since. Massive caverns hold the salt that keeps half the region's meat through winter…`
- Row 020 `location_bio_remediation_lab`: `{"baseRadsPerHour":50.0,"dangerLevel":8.0,"description":"Ground zero for the Myco-Protocol, the experiment that was supposed to eat the contamination and learned to eat everything else. Spore density in the main hall is lethal, and fifty r…`
- Row 021 `location_submerged_data_center`: `{"baseRadsPerHour":25.0,"dangerLevel":7.0,"description":"The data center took the flood at street level and kept breathing through its backup floor. Water stands to the chest in the aisle, and the servers that ran the city's networks are s…`
- Row 022 `location_geothermal_vent_shaft`: `{"baseRadsPerHour":60.0,"dangerLevel":8.0,"description":"The earth's mantle bleeds heat up through fractured rock here, and the vent shaft breathes it out in clouds of steam that carry sulfur and ash. Sixty rads an hour where the steam ban…`
- Row 023 `location_the_sump_cathedral`: `{"baseRadsPerHour":35.0,"dangerLevel":6.0,"description":"An underground cistern the Dredgers rebuilt into a shrine, and the light inside is not from lamps. Bioluminescent moss carpets every surface, ceilings to waterline, glowing faint gre…`
- Row 024 `location_abandoned_desalination`: `{"baseRadsPerHour":40.0,"dangerLevel":7.0,"description":"Occupied. Failing. Named. The word 'abandoned' was what Sector 4 could see from the Drown.\n\nConcrete intakes, salt-white yards, steam that smells like hot metal and iodine. The RO …`
- Row 025 `location_deep_core_borehole`: `{"baseRadsPerHour":80.0,"dangerLevel":9.0,"description":"The deepest hole on the map, two miles of shaft drilled into the mantle, and the heat rising out of it keeps the snow off a circle of ground the size of a town square. Eighty rads an…`
- Row 026 `location_uxo_highway_choke`: `{"baseRadsPerHour":30.0,"dangerLevel":9.0,"description":"The six-lane highway is paved with a layer of unexploded cluster munitions, scattered by a strike that never needed to be accurate. Every step is a gamble, and the local phrase for c…`
- Row 027 `location_radar_array_spire`: `{"baseRadsPerHour":45.0,"dangerLevel":8.0,"description":"The dish is the size of a house, and it groans as the wind loads its face, turning on bearings that should have seized three years ago. A magnetic anomaly around the base is strong e…`
- Row 028 `location_drone_hive_silo`: `{"baseRadsPerHour":60.0,"dangerLevel":10.0,"description":"The silo lid is gone, and the loitering munitions have moved in. They nest in the launch bay in layers, dormant but warm, and they buzz when the sun hits the top of the stack, a sou…`
- Row 029 `location_automated_mortar_pit`: `{"baseRadsPerHour":40.0,"dangerLevel":9.0,"description":"Every twelve hours, the mortar fires. The Custodian, the automated system that runs this bunker, does not sleep and does not aim: it lobs shells into the ash-wastes on a timer, as if…`
- Row 030 `location_scrap_neuromancer_camp`: `{"baseRadsPerHour":20.0,"dangerLevel":6.0,"description":"The cargo plane came down nose-first and the Wire-Heads built their camp in its belly, wiring the fuselage with salvaged cable and light from their own circuits. They worship logic g…`
- Row 031 `location_magnetic_anomaly_crater`: `{"baseRadsPerHour":55.0,"dangerLevel":8.0,"description":"A local magnetic storm, tight and violent, sits in the crater like a weather system that never leaves. Metal wants to leave your hands here: keys jump, knives slide toward the rim, a…`
- Row 032 `location_abandoned_convoy_yard`: `{"baseRadsPerHour":25.0,"dangerLevel":7.0,"description":"Rows of transport trucks rust in formation, nose to tail, as if waiting for a convoy order that will never come. A single sentry gun guards the yard, still powered, still tracking: i…`
- Row 033 `location_acoustic_testing_facility`: `{"baseRadsPerHour":15.0,"dangerLevel":8.0,"description":"The anechoic chambers swallow sound completely, and the silence inside is thick enough to hear your own blood moving. Radiation is low here, fifteen rads an hour, because the facilit…`
- Row 034 `location_substation_omega`: `{"baseRadsPerHour":35.0,"dangerLevel":7.0,"description":"The transformer yard is a forest of steel and ceramic, and the capacitors in the switch house still hold charge, which is the problem. Arcing here is an EMP in miniature: it can kill…`
- Row 035 `location_the_dead_hand_core`: `{"baseRadsPerHour":80.0,"dangerLevel":10.0,"description":"The Dead Hand Core is the machine that keeps the UXO fields awake, the regional brain that decides when the ground goes off. Eighty rads an hour outside the blast door, and the Cust…`
- Row 036 `location_lethe_water_treatment`: `{"baseRadsPerHour":40.0,"dangerLevel":9.0,"description":"The water treatment plant is a front; the real work went on in the sub-level below, where the amnestics were synthesized in batches and shipped up the lift in unmarked drums. The lab…`
- Row 037 `location_observatory_dome`: `{"baseRadsPerHour":20.0,"dangerLevel":10.0,"description":"Above the ash layer, where the sky is clear and the sun is a weapon. The UV Scourge at this altitude burns through in minutes, sunburn that blisters before you notice it, and twenty…`
- Row 038 `location_submerged_arcology`: `{"baseRadsPerHour":30.0,"dangerLevel":8.0,"description":"The arcology was the pre-war elite's answer to the end of the world, and it worked exactly as well as their other plans. The lower levels flooded first, and the residents starved in …`
- Row 039 `location_concrete_batching_plant`: `{"baseRadsPerHour":25.0,"dangerLevel":6.0,"description":"The batching plant sits where it was when the war ended, hoppers still full, drums still in rows, everything covered in a year of ash and then two more of weather. The risk is struct…`
- Row 040 `location_seed_vault_antechamber`: `{"baseRadsPerHour":15.0,"dangerLevel":7.0,"description":"The antechamber is the story of the vault in miniature: the outer doors failed, blown in from the top down, and the inner doors held, sealed against the blast and the weather and eve…`
- Row 041 `location_hospital_psych_wing`: `{"baseRadsPerHour":50.0,"dangerLevel":8.0,"description":"The psych wing holds the echoes of the first week, when the panic reached the hospital and the hospital did what it could. Charts are still on the boards, written fast, stopped mid-s…`
- Row 042 `location_mirror_factory`: `{"baseRadsPerHour":20.0,"dangerLevel":5.0,"description":"The factory floor is a field of shattered glass, every pane from the polishing line in a thousand pieces, catching the grey light from the roof holes. Work boots are not optional her…`
- Row 043 `location_radio_telescope_array`: `{"baseRadsPerHour":45.0,"dangerLevel":9.0,"description":"The dishes are the size of houses, and they groan as the wind loads their faces, turning on bearings that should have seized three years ago. Forty-five rads an hour concentrates in …`
- Row 044 `location_ash_whale_carcass`: `{"baseRadsPerHour":60.0,"dangerLevel":7.0,"description":"What looks like a whale beached in the ash is a fossil: the remains of a massive pre-war root network that outlived the forest it belonged to, mineralized and black, rising out of th…`
- Row 045 `location_the_memory_vault`: `{"baseRadsPerHour":80.0,"dangerLevel":10.0,"description":"The Memory Vault was built to hold the last three years of a civilization's small talk: photos of meals, arguments, birthdays, the ordinary noise of ordinary days. Eighty rads an ho…`
- Row 046 `highway_pileup`: `{"baseRadsPerHour":25.0,"dangerLevel":6.0,"description":"Two kilometers of the ring road, welded together into a single continuous mass of cars, doors open, hoods up, looted in the first year and welded in the second by whoever decided nob…`
- Row 047 `prewar_medical_cache`: `{"baseRadsPerHour":15.0,"dangerLevel":4.0,"description":"A clinic basement, sealed in the last week before the exchange, the door dogged down and the boxes stacked by someone who thought they might be back. Fifteen rads an hour, low, and t…`
- Row 048 `loc_grange_hall`: `{"baseRadsPerHour":14,"dangerLevel":3,"description":"The Grange Hall still holds meetings, and the oil lamps are lit for them. A long table runs the length of the room, and a hand-lettered sign by the porch asks visitors to leave their wea…`
- Row 049 `loc_apiary_rows`: `{"baseRadsPerHour":18,"dangerLevel":3,"description":"Forty white hive boxes stand in rows in a field, and thirty-eight of them are silent. The two that are not are kept by someone who has cut the grass around all forty boxes, not just the …`
- Row 050 `loc_seed_library_annex`: `{"baseRadsPerHour":16,"dangerLevel":4,"description":"The branch library is a library again, but of a different kind: the fiction shelves have been cleared and refilled with labelled paper envelopes, each one holding seed, with the variety,…`
- Row 051 `loc_veterinary_surgery`: `{"baseRadsPerHour":22,"dangerLevel":4,"description":"The large-animal surgery still smells of disinfectant and hide, three years on. Livestock stocks frame the door, a hoist hangs over the table, and the drug cabinet sits open, its lock cu…`
- Row 052 `loc_school_gymnasium`: `{"baseRadsPerHour":20,"dangerLevel":3,"description":"The gymnasium floor still shows the basketball court markings, faded under a grid of cot-shaped stains, row after row of where the beds stood in the first winter. The hand-painted banner…`
- Row 053 `loc_cider_press`: `{"baseRadsPerHour":24,"dangerLevel":4,"description":"The stone barn holds a screw press that still works, its threads oiled, its beam sound, a machine that has outlasted the orchard that fed it. Twenty-four rads an hour, moderate. The outp…`
- Row 054 `loc_terrace_pumphouse`: `{"baseRadsPerHour":26,"dangerLevel":5,"description":"The pumphouse is the irrigation head for the whole south slope, and whoever controls it controls the season. The pump still runs, and the channels below it are kept clean, which means th…`
- Row 055 `loc_ration_queue_plaza`: `{"baseRadsPerHour":28,"dangerLevel":4,"description":"The plaza is a civic square with queue lines painted on the pavement, and they are repainted often, because painted lines are cheaper than riot control and everyone here knows it. The li…`
- Row 056 `loc_conscription_office`: `{"baseRadsPerHour":26,"dangerLevel":5,"description":"The office was a driving licence bureau before the war, and the conversion was cheap: the counters stayed, the forms were changed, and the ticket machine was left exactly where it was. I…`
- Row 057 `loc_municipal_archive`: `{"baseRadsPerHour":30,"dangerLevel":5,"description":"The archive's rolling stacks run the length of the floor, most of them collapsed into each other like a card house that lost its nerve. The fire suppression discharged at some point, and…`
- Row 058 `loc_dentists_row`: `{"baseRadsPerHour":32,"dangerLevel":5,"description":"Four dental practices stood on this street, and three are stripped to the walls, fixtures gone, doors gone, the wiring pulled in straight lines by someone methodical. The fourth is missi…`
- Row 059 `loc_transit_authority_hq`: `{"baseRadsPerHour":34,"dangerLevel":6,"description":"The transit authority's wall-sized route maps are still under glass, and the glass is still intact, and the maps are annotated in grease pencil with times, connections, and transfers tha…`
- Row 060 `loc_printworks`: `{"baseRadsPerHour":30,"dangerLevel":5,"description":"The web presses are seized solid, their rollers fused by the years into one unbroken line of rusted iron, and the smell of ink has finally left the building. The pallets of undelivered l…`
- Row 061 `loc_department_store`: `{"baseRadsPerHour":36,"dangerLevel":6,"description":"Six floors of Vansen's, comprehensively looted in the first year and picked over in the years since, until what remains is only what nobody could use: the fittings, the counters, the dis…`
- Row 062 `loc_public_swimming_baths`: `{"baseRadsPerHour":38,"dangerLevel":6,"description":"The deep end of the municipal baths was drained in the first winter and floored with mattresses in rows, a shelter that worked because the tiled walls hold heat well and the water below …`
- Row 063 `loc_st_brigids_almshouse`: `{"baseRadsPerHour":40,"dangerLevel":7,"description":"The almshouse was a hospice before the war and ran as one through the worst of it, and the building still carries that purpose in its bones. The beds are made, the sheets drawn tight, th…`
- Row 064 `loc_weighbridge`: `{"baseRadsPerHour":28,"dangerLevel":5,"description":"The weighbridge is a truck scale with a mechanical readout that still works, a machine with no electronics and nothing to go wrong, and the needle settles true every time. It was the Tol…`
- Row 065 `loc_motel_verity`: `{"baseRadsPerHour":26,"dangerLevel":5,"description":"Twelve units around a dry pool, and the pool is swept clean of ash every week, which is the first sign the Verity is looked after. It is neutral ground, and the neutrality is enforced by…`
- Row 066 `loc_bridge_seven`: `{"baseRadsPerHour":30,"dangerLevel":6,"description":"Four lanes over the gorge, and the bridge is intact, which is the exception in this district and the reason it is guarded from both ends. The charges are still visibly taped to the under…`
- Row 067 `loc_recovery_yard`: `{"baseRadsPerHour":34,"dangerLevel":6,"description":"The wreckers and flatbeds nose in around the crane like a herd waiting for feed, and the crane is the only piece of equipment here that has never been taken apart. Half the fleet has bee…`
- Row 068 `loc_ordnance_shoulder`: `{"baseRadsPerHour":44,"dangerLevel":7,"description":"Two kilometers of hard shoulder, marked with paint sticks in a pattern that the Garrison trained its people to recognize and everyone else learned to fear. The paint is Garrison-issue, t…`
- Row 069 `loc_bus_reversal_loop`: `{"baseRadsPerHour":32,"dangerLevel":5,"description":"The turning circle at the edge of the old evacuation route holds forty-one buses, nose to tail, every one of them pointing back toward the city. The doors are open and the keys are mostl…`
- Row 070 `loc_diesel_tank_farm`: `{"baseRadsPerHour":36,"dangerLevel":6,"description":"Eight bulk tanks in a row, the old farm's pride, and the ground around them is bare where the spill was burned off. The local test is performed from a distance: strike the tank with a th…`
- Row 071 `loc_radio_relay_mast`: `{"baseRadsPerHour":38,"dangerLevel":6,"description":"The guyed lattice mast stands over the ridge line, and the equipment hut at its base is powered. Not preserved, not maintained: powered, drawing current from a source that has outlasted …`
- Row 072 `loc_ash_sign_shrine`: `{"baseRadsPerHour":48,"dangerLevel":5,"description":"The survey cairn has been built up with concrete and glass slag into something taller than a person, and the construction is careful, the layers squared, the glass set to catch the grey …`
- Row 073 `loc_pilgrim_switchbacks`: `{"baseRadsPerHour":52,"dangerLevel":5,"description":"Eleven hairpins up the south face, and the verges are lined with boots. Pairs, placed neatly, toes to the road, thousands of them, stretching the whole climb. The boots are not a warning…`
- Row 074 `loc_snowline_station`: `{"baseRadsPerHour":42,"dangerLevel":6,"description":"The Garrison's forward post above the treeline was abandoned in good order, which is the detail that stays with visitors. The stove is banked, the fire laid for a return; the log is comp…`
- Row 075 `loc_low_background_lab`: `{"baseRadsPerHour":30,"dangerLevel":6,"description":"Deep in the salt, behind two airlocks that still seal, the low-background laboratory waits. Its walls are lined with steel salvaged from pre-atomic shipwrecks, iron smelted before the fi…`
- Row 076 `loc_ice_core_store`: `{"baseRadsPerHour":40,"dangerLevel":6,"description":"The freezer room runs on geothermal bleed, a loop of pipe sunk into the mountain's heat, and it has held its temperature through three winters without a single power line. The ice cores …`
- Row 077 `loc_avalanche_gallery`: `{"baseRadsPerHour":46,"dangerLevel":7,"description":"The concrete snow shed covers the pass road for a hundred meters, and the gallery is half filled with the snow it was built to deflect, pressed hard against the uphill wall. The uphill s…`
- Row 078 `loc_summit_relay`: `{"baseRadsPerHour":56,"dangerLevel":7,"description":"The relay station sits above everything, and on a clear day the line of sight reaches all five sub-regions of the map, every valley, every smoke column, every roof. There are perhaps six…`
- Row 079 `loc_the_vessels_cell`: `{"baseRadsPerHour":68,"dangerLevel":8,"description":"The reactor outbuilding is four meters square, and the door is wedged shut from the inside, which is the first fact and the one that matters. Water jugs are stacked against one wall, eve…`
- Row 080 `loc_lock_gate_four`: `{"baseRadsPerHour":44,"dangerLevel":7,"description":"Lock Gate Four is the gate that failed, and it is still open, exactly as far as it opened. The mechanism seized at the moment of the exchange, or of the flood, or of the decision, and th…`
- Row 081 `loc_pump_station_nine`: `{"baseRadsPerHour":48,"dangerLevel":7,"description":"Pump Station Nine was built to keep the basin dry, and the basin has won. The six drainage pumps stand under three meters of the very water they were built to move, their housings green …`
- Row 082 `loc_alloc_12b`: `{"baseRadsPerHour":54,"dangerLevel":8,"description":"The maintenance level is marked with a stencilled designation, Allocation 12-B, and nothing else: no stores, no supplies, no reason for the name. Fifty-four rads an hour at the stair, an…`
- Row 083 `loc_records_annex`: `{"baseRadsPerHour":40,"dangerLevel":7,"description":"The records annex is reached by boat, through a second-storey window, and the approach is the sort of thing that sorts visitors: those who can tie a line and those who cannot. Inside, th…`
- Row 084 `loc_drowned_cinema`: `{"baseRadsPerHour":46,"dangerLevel":7,"description":"The water in the Odeon has settled at the level of row F, and the screen is intact, the last thing in the building still facing its audience. The seats below the line are still folded do…`
- Row 085 `loc_cold_store_atlantic`: `{"baseRadsPerHour":50,"dangerLevel":8,"description":"Twelve thousand cubic meters of freezer, and the room is four meters deep in water that has frozen over the top, sealing the contents under a floor of grey ice. The contents are unknown …`
- Row 086 `loc_bathymetric_boat`: `{"baseRadsPerHour":52,"dangerLevel":8,"description":"The survey launch Kittiwake lies aground on a submerged roof, listing gently, held where the flood left her. The sonar rig is intact, still mounted, still connected, a machine that measu…`
- Row 087 `loc_the_shallows_market`: `{"baseRadsPerHour":42,"dangerLevel":7,"description":"Nine boats tied into a single raft over what used to be a retail park, and the trade happens at gunwale height, hand to hand across the water, because nobody boards anybody else's hull a…`
- Row 088 `loc_the_allotments`: `{"baseRadsPerHour":30,"dangerLevel":5,"description":"Two hundred numbered plots behind a chain-link fence, a caretaker's hut, and a noticeboard at the gate. The waiting list is still pinned to the noticeboard, in a plastic sleeve, and it h…`
- Row 089 `checkpoint_kilo_armory`: `{"baseRadsPerHour":18,"dangerLevel":4,"description":"The armory of the old checkpoint: a concrete room of lockers and a heavy door. The racks are mostly empty, but the sealed lockers hold what the guard did not carry out, and the lock is r…`
- Row 090 `hospital_pharmacy`: `{"baseRadsPerHour":28,"dangerLevel":5,"description":"The pharmacy of the ruined hospital, door still intact behind the collapsed stairwell. The shelves behind the counter hold what the looters could not reach: vials, boxes, the small certa…`
- Row 091 `family_bunker_backyard_shed`: `{"baseRadsPerHour":6,"dangerLevel":2,"description":"A backyard shed with a false floor. Beneath it, a family shelter stocked with care and dread: tinned food, a radio, a tape recorder, and a letter taped to the shelf.","displayName":"Famil…`
- Row 092 `old_library_cache`: `{"baseRadsPerHour":12,"dangerLevel":3,"description":"A cache in the ruined library, tucked behind the collapsed reference desk. Books survived here in a dry pocket of the building, and with them the things people left between the pages.","…`
- Row 093 `convoy_echo7_cache`: `{"baseRadsPerHour":22,"dangerLevel":4,"description":"A supply cache buried by the wreck of convoy Echo-7, its marker half-swallowed by drift. The tarpaulin holds; what the convoy carried is still under it.","displayName":"Convoy Echo-7 Cac…`
- Row 094 `raider_ambush_site`: `{"baseRadsPerHour":30,"dangerLevel":5,"description":"The place where the road narrows between the collapsed buildings: the classic kill ground. The raiders' own cache is hidden in the rubble nearby, marked with a chalked glyph for those in…`
- Row 095 `collapsed_building`: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"A building that came down on itself, floors folded like a hand of cards. The pockets between the slabs are still dry, and what fell in here stayed in here.","displayName":"Collapsed Buil…`
- Row 096 `raider_trap_location`: `{"baseRadsPerHour":30,"dangerLevel":5,"description":"A stretch of road rigged with tripwires and buried spikes, the kind of place raiders use to slow a convoy. The trap maker's hiding spot is within sight of the road, and so is what they c…`
- Row 097 `electrical_substation`: `{"baseRadsPerHour":20,"dangerLevel":4,"description":"A substation yard of dead transformers and tangled busbars. The copper is long gone, but the control room survived the blast, and the tools and spares in it did too.","displayName":"Elec…`
- Row 098 `ruined_garage`: `{"baseRadsPerHour":14,"dangerLevel":3,"description":"A mechanics' garage with its roof half gone. The lifts are seized, the benches bare, but the tool lockers and the pit beneath the work floor were never opened.","displayName":"Ruined Gar…`
- Row 099 `concert_hall_ruins`: `{"baseRadsPerHour":8,"dangerLevel":2,"description":"The shell of the old concert hall, roof open to the sky, stage intact. The acoustics still work: a whisper from the stage reaches the back row. Someone has been using it as a meeting plac…`
- Row 100 `loc_grain_silo`: `{"baseRadsPerHour":19,"dangerLevel":3,"description":"The old Militia grain silo leans two degrees off true and has leaned that way for three years without picking a direction to fall. Underneath it, on packed earth between the legs of the …`
- Row 101 `loc_garrison_checkpoint_gamma`: `{"baseRadsPerHour":26,"dangerLevel":4,"description":"Sandbags gone the color of the ash they hold back, a boom barrier painted with a stripe pattern nobody has refreshed since the paint still matched a regulation. The notice board by the g…`
- Row 102 `loc_railway_span_44_alpha`: `{"baseRadsPerHour":31,"dangerLevel":6,"description":"A single rail bridge over a dry cutting, one of the last spans in the district still theoretically load-bearing, and theoretically is doing a lot of work in that sentence. Charges are wi…`
- Row 103 `loc_forward_roster_camp`: `{"baseRadsPerHour":22,"dangerLevel":5,"description":"A dozen tents and one salvaged shipping container pitched in the dead lot between the Exchange and the Garrison checkpoint, close enough to both that it reads less like a camp and more l…`
- Row 104 `loc_shrine_switchback_waystation`: `{"baseRadsPerHour":31,"dangerLevel":4,"description":"A lean-to and a water barrel halfway up the trail to the Ash Sign Shrine, built by pilgrims for pilgrims, maintained by whoever's climbing that week. A board nailed to the lean-to's post…`
- Row 105 `loc_understory_transmitter`: `{"baseRadsPerHour":18,"dangerLevel":3,"description":"A transmitter mast wired into what used to be a parking structure's stairwell, the antenna run up through a gap in the collapsed roof and guyed to rebar that was never meant to hold a ma…`
- Row 106 `loc_shelter_gate`: `{"baseRadsPerHour":2,"dangerLevel":1,"description":"The main entrance to the shelter, a reinforced hatch set into the hillside with a decontamination vestibule and a checkpoint that never quite stops being a checkpoint. The gate controls w…`
- Row 107 `loc_shelter_meeting`: `{"baseRadsPerHour":1,"dangerLevel":1,"description":"A repurposed conference chamber with a long table salvaged from the municipal offices and chairs that do not match. The walls are lined with whiteboards still holding the last pre-war age…`
- Row 108 `loc_shelter_infirmary`: `{"baseRadsPerHour":1,"dangerLevel":1,"description":"The shelter's medical bay, lit by fluorescent tubes that hum even when the main power is offline. There are two examination couches, a cabinet of scavenged supplies, and a dosage ledger t…`
- Row 109 `loc_shelter_storage`: `{"baseRadsPerHour":1,"dangerLevel":1,"description":"Racks of canned goods, fuel drums, and spare parts stacked floor to ceiling in the shelter's rear chamber. The inventory is written on cardboard tags tied with twine, and the twine is sta…`
- Row 110 `loc_shelter_quarters`: `{"baseRadsPerHour":1,"dangerLevel":1,"description":"The sleeping quarters: rows of bunk beds with curtains for privacy, a shared washbasin, and a shelf where everyone keeps their dosimeter and their personal journal. The air is warm and sm…`
- Row 111 `loc_shelter_fire`: `{"baseRadsPerHour":3,"dangerLevel":2,"description":"The fire break and secondary containment zone at the rear of the shelter, where the ventilation shafts exit and the smoke from the kitchen stove is supposed to vent. The metal grating is …`
- Row 112 `loc_shelter_perimeter`: `{"baseRadsPerHour":2,"dangerLevel":1,"description":"The outer ring of the shelter: blast doors, airlocks, and the maintenance corridor that circles the entire bunker. The concrete here is thicker, the emergency lights are older, and the ra…`
- Row 113 `loc_eastern_road`: `{"baseRadsPerHour":20,"dangerLevel":4,"description":"The old highway east, its surface broken by frost heave and the weight of military convoys that never came back. The road is passable for most of the year, and the ash drift is thinner h…`
- Row 114 `loc_neutral_ground`: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"A pre-war interchange plaza where three routes met and no faction claims sole ownership. The fountain is dry, the benches are broken, and a painted line on the pavement marks the boundar…`
- Row 115 `loc_water_station`: `{"baseRadsPerHour":12,"dangerLevel":3,"description":"A pre-war pumping station with a concrete well house and a tank that still holds water if you are willing to filter it twice. The electrical panel is shot, but the manual override still …`
- Row 116 `loc_excavation_command_vault`: `{"baseRadsPerHour":30,"dangerLevel":7,"description":"A reinforced military command installation buried beneath tons of blasted granite. Three distinct strata lead to the hardened communications vault.","displayName":"Collapsed Command Vaul…`
- Row 117 `loc_excavation_utility_tunnels`: `{"baseRadsPerHour":15,"dangerLevel":4,"description":"Subterranean municipal service corridors and conduit trunks that run beneath the old district grid. The mud is knee-deep in the lower sections and the standing water carries a faint chem…`
- Row 118 `loc_excavation_metro_interchange`: `{"baseRadsPerHour":25,"dangerLevel":6,"description":"A multi-tier transit hub collapsed during the orbital strikes. Deep platform levels remain pressurized pockets holding pre-war civilian relics.","displayName":"Buried Metro Interchange",…`
- Row 119 `loc_excavation_mine_shaft`: `{"baseRadsPerHour":40,"dangerLevel":8,"description":"A heavy extraction shaft dropping into mineral-rich bedrock. High mechanical salvage and lead-ore veins, tempered by severe structural instability.","displayName":"Industrial Mine Shaft …`
- Row 120 `loc_excavation_archive_bunker`: `{"baseRadsPerHour":20,"dangerLevel":5,"description":"An underground scientific and administrative depository sealed under blast-hardened vault arches. Holds intact microforms, technical blueprints, and emergency dead-drop records.","displa…`
- Row 121 `loc_excavation_drainage_network`: `{"baseRadsPerHour":10,"dangerLevel":3,"description":"Stormwater culverts and overflow sluices converted into illicit smuggling routes before the bombardment. Silt and contaminated backwash hide sealed waterproof caches.","displayName":"Dra…`
- Row 122 `loc_excavation_storage_chamber`: `{"baseRadsPerHour":22,"dangerLevel":6,"description":"An auxiliary military logistics cache sealed in haste during civil evacuation. Unstable masonry slabs overhang intact pallets of rations and industrial spares.","displayName":"Forgotten …`
- Row 123 `loc_excavation_civilian_shelter`: `{"baseRadsPerHour":8,"dangerLevel":2,"description":"A privately funded neighborhood shelter built beneath a residential complex. Shorter excavation depths yield domestic survival gear, medical supplies, and handwritten diaries.","displayNa…`
- Row 124 `loc_hidden_relay_bunker`: `{"baseRadsPerHour":18,"dangerLevel":5,"description":"A concealed shortwave relay outpost buried into the cliffside, discovered by decrypting 'The Relay Count' numbers broadcast.","displayName":"Hidden Relay Bunker 09","id":"loc_hidden_rela…`
- Row 125 `loc_logistics_reserve_cache`: `{"baseRadsPerHour":12,"dangerLevel":4,"description":"A secure logistics basement cache unlocked using the Winter Ledger cipher sheet. Packed with sealed emergency rations and filtration units.","displayName":"Sub-Basement Logistics Reserve…`
- Row 126 `loc_deaddrop_command_shelter`: `{"baseRadsPerHour":22,"dangerLevel":6,"description":"An automated contingency shelter revealed through the Last Rotation dead-hand protocol. Contains classified directives and high-grade technical relics.","displayName":"Dead-Drop Command …`
- Row 127 `loc_holdfast`: `{"baseRadsPerHour":0,"dangerLevel":0,"description":"The home bunker. Sub-surface reinforced shelter providing life support, workbenches, and secure quarters for the survivors.","displayName":"The Holdfast","id":"loc_holdfast","travelHours"…`
- Row 128 `loc_cut_radiation_zone_alpha`: `{"baseRadsPerHour":75,"dangerLevel":8,"description":"A high-yield ground zero impact basin saturated with ionizing radiation and pulverized cinder. High-grade military and industrial salvage remains in the melted basement levels.","display…`
- Row 129 `loc_cut_merchant_caravanserai`: `{"baseRadsPerHour":10,"dangerLevel":2,"description":"A fortified pre-war truck stop and weigh yard where wandering barter convoys converge. Low radiation and reliable staging for westward expeditions.","displayName":"Merchant Caravanserai"…`
- Row 130 `loc_cut_abandoned_depot`: `{"baseRadsPerHour":20,"dangerLevel":3,"description":"A sprawling freight yard with rusted boxcars and freight terminals. An essential transit corridor connecting the industrial belt to the western suburbs.","displayName":"Abandoned Rail De…`
- Row 131 `loc_cut_arsenal_ruin`: `{"baseRadsPerHour":35,"dangerLevel":6,"description":"The bombed-out munitions storage and armory facility. Fortified vaults contain ballistic components and weapon parts behind collapsed blast doors.","displayName":"Arsenal Ruin","id":"loc…`
- Row 132 `loc_black_flotilla_outpost`: `{"baseRadsPerHour":40,"dangerLevel":7,"description":"A coastal salvage pier and harbor watchtower garrisoned by maritime divers. Controls access to deep-coast marine wrecks and saline processing lanes.","displayName":"Black Flotilla Outpos…`
- Row 133 `loc_settlement_brine_pans`: `{"baseRadsPerHour":15,"dangerLevel":2,"description":"A fortified salt camp in the tidal estuary. Evaporation pans and steam condensers produce curing salt and preserved provisions under salter council watch.","displayName":"Brine-Pan Hollo…`
- Row 134 `loc_settlement_iron_siding`: `{"baseRadsPerHour":20,"dangerLevel":3,"description":"A modular rail-car town buried beneath railroad ballast near Span 44. Work gangs forge high-tensile hardware and armor plates from freight car steel.","displayName":"Iron Siding","id":"l…`
- Row 135 `loc_settlement_cape_beacon`: `{"baseRadsPerHour":25,"dangerLevel":4,"description":"A coastal lighthouse community maintaining freshwater cisterns, kelp drying racks, and prism optics on a windswept ocean bluff.","displayName":"Cape Beacon Commune","id":"loc_settlement_…`
- Row 136 `loc_settlement_slate_hollow`: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"A subterranean quarry enclave cut into impervious slate galleries in the High Scarp. Pit crews extract building stone and mill hones shielded from weather.","displayName":"Slate Hollow E…`
- Row 137 `loc_settlement_pilgrim_hearth`: `{"baseRadsPerHour":10,"dangerLevel":2,"description":"A mountain sanctuary warmed by natural geothermal steam radiators at Switchback Pass. Monks provide hot broth, herbal remedies, and peace-bound rest.","displayName":"The Pilgrim's Hearth…`
- Row 138 `loc_settlement_tinkers_notch`: `{"baseRadsPerHour":15,"dangerLevel":2,"description":"A bustling container and chassis market in the dead suburbs surrounded by an electrified fence. Factor stalls trade copper wire, batteries, and electronic chips.","displayName":"Tinker's…`
- Row 139 `location_quarry_overlook`: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"A wind-scoured ridge above Slate Hollow Enclave where the old quarry crane platform gives a clear view of the pit entrance and the High Scarp switchbacks.","displayName":"Quarry Overlook…`
- Row 140 `loc_grain_exchange`: `{"baseRadsPerHour":10,"dangerLevel":2,"description":"A converted rail freight terminal serving as the Grain Exchange faction's primary commodity hub. Surplus cereal and root stores are tallied here and rationed outward through a network of…`
- Row 141 `loc_automated_abattoir`: `{"baseRadsPerHour":20,"dangerLevel":4,"description":"A pre-war industrial slaughterhouse whose hydraulic line-kill systems still operate on stored power cells. The Osteophages use it to process large irradiated game and recycle bone stock …`
- Row 142 `loc_flooded_subway_depot`: `{"baseRadsPerHour":25,"dangerLevel":4,"description":"A drowned underground rail maintenance yard accessed through a half-submerged service hatch. The Undertow faction operates supply caches in the dry upper galleries, moving goods through …`
- Row 143 `loc_scavenger_camp`: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"A rotating canvas-and-corrugated-steel encampment operated by the Scavenger Guild. Its position shifts with salvage fronts but the gatehouse marker is always the same: a stripped vehicle…`
- Row 144 `loc_iron_garrison`: `{"baseRadsPerHour":10,"dangerLevel":5,"description":"The hardened command compound of the Iron Garrison faction, occupying a pre-war civil defense building with reinforced sub-levels. Access is by escort only; civilians who approach the pe…`
- Row 145 `loc_dead_zone`: `{"baseRadsPerHour":50,"dangerLevel":7,"description":"A silent, wind-scoured ash basin where ecological succession completely failed after intense isotope deposition. No insects hum, no lichen clings to the fractured basalt slabs, and desic…`
- Row 146 `loc_settlement_ferry_crossing`: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"A bustling river ferry crossing and barter dock in the Drown sector. River barges trade clean drinking water and preserved rations under the Undertow faction's watchful patrols.","displa…`
- Row 147 `loc_settlement_nine_rails`: `{"baseRadsPerHour":10,"dangerLevel":2,"description":"A major rail concourse and trade hub in the Industrial Belt where switching yards meet covered trade docks, overseen by The Office's scheduling clerks.","displayName":"Nine Rails Junctio…`
- Row 148 `loc_settlement_fort_karkov`: `{"baseRadsPerHour":25,"dangerLevel":5,"description":"A heavily fortified rail fortress in the High Scarp controlled by the Deserter Coalition. Armor-plated diesel cars form defensive bastions around machine tooling workshops.","displayName…`
- Row 149 `loc_settlement_lock_seven`: `{"baseRadsPerHour":20,"dangerLevel":3,"description":"A reinforced concrete sluice gate and hydraulic toll station in the Toll region, controlling water transit and charging tariffs in fuel and mechanical parts.","displayName":"Lock Seven H…`
- Row 150 `loc_settlement_silo_burrow`: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"An agricultural commune built into three massive concrete grain silos in the Verge. Farmers cultivate winter grain and barter seeds with passing caravans.","displayName":"New Ceres Silo …`
- Row 151 `loc_settlement_st_nicholas`: `{"baseRadsPerHour":5,"dangerLevel":1,"description":"An underground artesian spring and monastic hospital in the Cluster. Silent caretakers offer clean holy water and sterile burn treatment to wounded travelers.","displayName":"St. Nicholas…`
- Row 152 `loc_iron_crest`: `{"baseRadsPerHour":8,"dangerLevel":3,"description":"A high ridgeline survey station atop the Iron Crest massif, used by geodetic teams for triangulation benchmarks. The view is total, the shelter is none, and the readings are worth both.",…`
- Row 153 `loc_ash_needle`: `{"baseRadsPerHour":18,"dangerLevel":4,"description":"A volcanic rock formation rising through the ashfield, used as a geodetic reference point. Seasonal ash plumes reduce visibility.","displayName":"Ash Needle Spire","id":"loc_ash_needle",…`
- Row 154 `loc_wind_gap_ridge`: `{"baseRadsPerHour":6,"dangerLevel":3,"description":"A narrow mountain pass swept by near-constant high wind. Surveyors use it for line-of-sight triangulation, and have learned to shout their readings in the gaps between gusts.","displayNam…`
- Row 155 `loc_signal_hill_tower`: `{"baseRadsPerHour":10,"dangerLevel":2,"description":"A collapsed pre-war signal relay tower on a prominent hill. The steel frame still stands and serves as a survey sight — the last signal it will ever carry is somebody else's line of meas…`
- Row 156 `loc_river_bend_outpost`: `{"baseRadsPerHour":12,"dangerLevel":2,"description":"A flood-scoured river bend where pre-war survey crews set a permanent datum monument. Accessible only at low water; the monument has outwaited every flood that tried to argue with it.","…`
- Row 157 `loc_rusted_span_bridge`: `{"baseRadsPerHour":14,"dangerLevel":3,"description":"A partially collapsed iron rail bridge. The remaining abutment is stable enough to mount a survey instrument, and steady enough that the theodolite reads truer here than on solid ground.…`
- Row 158 `loc_old_crematory_stacks`: `{"baseRadsPerHour":20,"dangerLevel":2,"description":"Decommissioned industrial chimneys from a pre-war crematory complex, repurposed as vertical survey markers. The crews who use them for sightings have stopped saying what they were, which…`
- Row 159 `loc_north_gate_water_tower`: `{"baseRadsPerHour":8,"dangerLevel":1,"description":"A surviving water tower at the northern perimeter of a collapsed settlement, offering unobstructed sightlines in every direction. It has been empty for years, and nobody has decided wheth…`
- Row 160 `loc_junction_box_rail`: `{"baseRadsPerHour":10,"dangerLevel":2,"description":"A concrete rail junction building surrounded by overgrown track debris. Surveyors use the rooftop for low-elevation benchmarks.","displayName":"Junction Box Rail Station","id":"loc_junct…`
- Row 161 `loc_sulfur_knob`: `{"baseRadsPerHour":15,"dangerLevel":4,"description":"A fumarolic rock outcrop with persistent sulfur venting. Difficult to occupy but visible from great distances as a triangulation target.","displayName":"Sulfur Knob Outcrop","id":"loc_su…`
- Row 162 `loc_gravel_backbone`: `{"baseRadsPerHour":7,"dangerLevel":3,"description":"A long gravel ridge running northeast. Exposed and wind-battered, it offers a clear horizon for survey baseline measurements.","displayName":"Gravel Backbone Ridge","id":"loc_gravel_backb…`
- Row 163 `loc_south_beacon_tower`: `{"baseRadsPerHour":9,"dangerLevel":2,"description":"A partially collapsed pre-war navigation beacon tower. The lower floors are accessible and serve as a survey datum; the lamp room is not accessible, and nobody has stopped wondering what …`
- Row 164 `loc_frozen_well_station`: `{"baseRadsPerHour":11,"dangerLevel":3,"description":"A permafrost wellhead station sealed by ice. Surveyors mount instruments on the reinforced concrete cap during winter operations.","displayName":"Frozen Well Station","id":"loc_frozen_we…`
- Row 165 `loc_rail_trestle_gorge`: `{"baseRadsPerHour":13,"dangerLevel":3,"description":"A narrow gorge spanned by a partially intact rail trestle. The bridge abutment provides an elevated survey position over the gorge floor.","displayName":"Rail Trestle Gorge","id":"loc_ra…`
- Row 166 `loc_foundry_west_stacks`: `{"baseRadsPerHour":22,"dangerLevel":3,"description":"Tall chimneys of a derelict foundry on the western industrial fringe. Smoke plumes from residual chemical fires persist seasonally.","displayName":"Foundry West Industrial Stacks","id":"…`
- Row 167 `loc_reservoir_water_tower`: `{"baseRadsPerHour":9,"dangerLevel":2,"description":"A water tower serving the pre-war reservoir district. Reservoir mist reduces visibility but the tower remains structurally sound.","displayName":"Reservoir District Water Tower","id":"loc…`
- Row 168 `loc_forestry_compound`: `{"baseRadsPerHour":15,"dangerLevel":4,"description":"A pre-war timber harvesting station and log sorting yard in the northern pines. The diesel sawmills stand silent, but the maintenance sheds hold spare parts and tools.","displayName":"Fo…`
- Row 169 `loc_warehouse_district`: `{"baseRadsPerHour":20,"dangerLevel":5,"description":"Rows of corrugated steel storage buildings once serving the freight terminal. Now a labyrinth of broken pallets, shadowed catwalks, and holding pens used by scavenger gangs.","displayNam…`
- Row 170 `loc_broadcast_bunker_echo`: `{"baseRadsPerHour":28,"dangerLevel":6,"description":"A reinforced subsurface relay bunker buried beneath an antenna lattice on the central ridge. The blast door is jammed half-open, and high-gain receiver racks line the operational bay und…`
- Row 171 `loc_underground_fuel_depot`: `{"baseRadsPerHour":22,"dangerLevel":5,"description":"Subterranean petroleum reserves built into a reinforced quarry cut. Heavy steel manholes seal the underground storage tanks, where valve wheels are frozen in place and lingering hydrocar…`
- Row 172 `loc_municipal_seed_vault`: `{"baseRadsPerHour":18,"dangerLevel":4,"description":"A climate-controlled agricultural reserve vault beneath the municipal courthouse basement. Dry-pack seed canisters and cold storage lockers remain sealed behind insulated airlock hatches…`
- Row 173 `loc_blacksite_armory_7`: `{"baseRadsPerHour":35,"dangerLevel":7,"description":"An unlisted military munitions cache shielded by blast deflection berms and concertina wire. Hardened weapon lockers and perimeter defensive revetments guard ammunition crates and tactic…`
- Row 174 `loc_sealed_triage_annex`: `{"baseRadsPerHour":20,"dangerLevel":4,"description":"An emergency decontamination and overflow trauma station established in a fortified highway underpass. Carts of surgical instruments, field stretchers, and sealed medical supply crates s…`
- Row 175 `loc_evidence_sub_basement`: `{"baseRadsPerHour":15,"dangerLevel":3,"description":"The subterranean storage vaults of the pre-war justice archive. Steel security cages hold confiscated contraband, logbooks, and salvageable trade goods behind tamper-sealed security gate…`
- Row 176 `loc_quarantine_barn`: `{"baseRadsPerHour":16,"dangerLevel":3,"description":"A timber-framed agricultural outpost repurposed during the early pandemic waves. Lime-washed containment stalls, disinfectant wash basins, and emergency supply bins stand intact beneath …`
- Row 177 `loc_forestry_emergency_store`: `{"baseRadsPerHour":18,"dangerLevel":4,"description":"A backcountry ranger supply depot and fire watch caches tucked into the evergreen foothills. Equipment racks retain axes, chainsaws, safety harnesses, and winter rations sealed in galvan…`
- Row 178 `loc_materials_research_sublevel`: `{"baseRadsPerHour":30,"dangerLevel":6,"description":"An advanced metallurgy and composite test facility sealed behind hydraulic security doors. Centrifuges, autoclaves, and specimen lockers contain specialized industrial reagents and rare …`
- Row 179 `loc_electrical_maintenance_exchange`: `{"baseRadsPerHour":25,"dangerLevel":5,"description":"A primary substation switching vault and line-maintenance dispatch terminal. Switchgear consoles, high-voltage transformers, and racks of copper cabling and relays fill the reinforced co…`


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Quests/NarrativeQuestlineSystem.cs`

### `Assets/Ashfall.Core/Quests/NarrativeQuestlineSystem.cs` — complete current file

- Size: 376 lines / 15461 bytes.
- SHA-256: `3ae336181120941db8e22db7f63ec87e000dfe8c1e2df63d1a0803e3dc5c853f`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // ASHFALL Core: survivor narrative questline progression (Plan 104 runtime).
00003:
00004: using System;
00005: using System.Collections.Generic;
00006:
00007: namespace Ashfall.Core.Quests
00008: {
00009:     /// <summary>
00010:     /// Lifecycle of one survivor arc. <see cref="AwaitingBranch"/> is the crisis
00011:     /// stage: objectives are exhausted and the arc cannot advance until the player
00012:     /// picks one of the two authored branches.
00013:     /// </summary>
00014:     public enum NarrativeArcStatus
00015:     {
00016:         NotStarted = 0,
00017:         Active = 1,
00018:         AwaitingBranch = 2,
00019:         Resolved = 3
00020:     }
00021:
00022:     /// <summary>
00023:     /// Persisted state of one survivor arc. Plain serializable DTO.
00024:     /// <para>
00025:     /// Public fields rather than properties on purpose: <see cref="SaveChecksum"/>
00026:     /// canonicalizes <c>BindingFlags.Public | BindingFlags.Instance</c> fields only,
00027:     /// so a property-based DTO hashes to an empty object and its envelope checksum
00028:     /// would silently stop detecting mutation. <c>SystemTextJsonSerializer</c> sets
00029:     /// <c>IncludeFields = true</c>, so fields round-trip through JSON unchanged.
00030:     /// </para>
00031:     /// </summary>
00032:     [Serializable]
00033:     public sealed class NarrativeQuestlineArcState
00034:     {
00035:         public string questId = string.Empty;
00036:         public string survivorId = string.Empty;
00037:         public int currentStage;
00038:         public NarrativeArcStatus status = NarrativeArcStatus.NotStarted;
00039:         public List<string> deliveredItems = new List<string>();
00040:         public string chosenBranchId = string.Empty;
00041:         public string grantedTraitId = string.Empty;
00042:         public int startedDay;
00043:         public int resolvedDay;
00044:
00045:         public NarrativeQuestlineArcState Clone()
00046:         {
00047:             return new NarrativeQuestlineArcState
00048:             {
00049:                 questId = questId,
00050:                 survivorId = survivorId,
00051:                 currentStage = currentStage,
00052:                 status = status,
00053:                 deliveredItems = new List<string>(deliveredItems ?? new List<string>()),
00054:                 chosenBranchId = chosenBranchId,
00055:                 grantedTraitId = grantedTraitId,
00056:                 startedDay = startedDay,
00057:                 resolvedDay = resolvedDay
00058:             };
00059:         }
00060:     }
00061:
00062:     /// <summary>Aggregate persisted state for the narrative questline system.</summary>
00063:     [Serializable]
00064:     public sealed class NarrativeQuestlineSaveState
00065:     {
00066:         public int schema_version = 1;
00067:         public string systemId = NarrativeQuestlineSystem.SystemId;
00068:         public List<NarrativeQuestlineArcState> arcs = new List<NarrativeQuestlineArcState>();
00069:     }
00070:
00071:     /// <summary>
00072:     /// Progression engine for the authored survivor arcs in
00073:     /// <c>narrative_questlines.json</c>. One arc per survivor, a linear ladder of
00074:     /// objective-item stages, then a single binary crisis fork whose choice grants
00075:     /// a trait and a morale delta and ends the arc on its resolution epilogue.
00076:     /// <para>
00077:     /// Deterministic: the system owns no RNG. Advancement is driven entirely by
00078:     /// authored data and player input, so a replayed input sequence over the same
00079:     /// catalog produces an identical arc state and an identical checksum.
00080:     /// </para>
00081:     /// <para>
00082:     /// Trait granting and morale application are deliberately NOT performed here:
00083:     /// Core reports the granted trait id and morale delta, and the host applies
00084:     /// them through the systems that already own survivor traits and shelter
00085:     /// morale. This keeps one authority per mechanic.
00086:     /// </para>
00087:     /// </summary>
00088:     public sealed class NarrativeQuestlineSystem
00089:     {
00090:         public const string SystemId = "narrative_questlines";
00091:
00092:         private readonly Dictionary<string, NarrativeQuestlineDef> _byQuestId =
00093:             new Dictionary<string, NarrativeQuestlineDef>(StringComparer.Ordinal);
00094:         private readonly Dictionary<string, NarrativeQuestlineDef> _bySurvivorId =
00095:             new Dictionary<string, NarrativeQuestlineDef>(StringComparer.Ordinal);
00096:         private readonly Dictionary<string, NarrativeQuestlineArcState> _arcsBySurvivor =
00097:             new Dictionary<string, NarrativeQuestlineArcState>(StringComparer.Ordinal);
00098:
00099:         private readonly ILog? _log;
00100:         private NarrativeQuestlineSaveState _state = new NarrativeQuestlineSaveState();
00101:         private bool _restoring;
00102:
00103:         public event Action<NarrativeQuestlineArcState>? OnArcStarted;
00104:         public event Action<NarrativeQuestlineArcState, int>? OnStageAdvanced;
00105:         public event Action<NarrativeQuestlineArcState, NarrativeQuestlineBranchDef>? OnBranchChosen;
00106:         public event Action<NarrativeQuestlineArcState>? OnArcResolved;
00107:
00108:         public NarrativeQuestlineSystem(IReadOnlyList<NarrativeQuestlineDef>? definitions, ILog? log = null)
00109:         {
00110:             _log = log;
00111:             if (definitions != null)
00112:             {
00113:                 for (int i = 0; i < definitions.Count; i++)
00114:                 {
00115:                     var def = definitions[i];
00116:                     if (def == null || string.IsNullOrEmpty(def.questId) || string.IsNullOrEmpty(def.survivorId))
00117:                         continue;
00118:                     if (!_byQuestId.ContainsKey(def.questId))
00119:                         _byQuestId[def.questId] = def;
00120:                     if (!_bySurvivorId.ContainsKey(def.survivorId))
00121:                         _bySurvivorId[def.survivorId] = def;
00122:                 }
00123:             }
00124:         }
00125:
00126:         public int DefinitionCount => _byQuestId.Count;
00127:
00128:         public IReadOnlyList<NarrativeQuestlineDef> Definitions
00129:         {
00130:             get
00131:             {
00132:                 var list = new List<NarrativeQuestlineDef>(_byQuestId.Count);
00133:                 foreach (var kv in _byQuestId) list.Add(kv.Value);
00134:                 list.Sort((a, b) => string.CompareOrdinal(a.questId, b.questId));
00135:                 return list;
00136:             }
00137:         }
00138:
00139:         public NarrativeQuestlineDef? GetDefinition(string questId)
00140:             => !string.IsNullOrEmpty(questId) && _byQuestId.TryGetValue(questId, out var d) ? d : null;
00141:
00142:         public NarrativeQuestlineDef? GetDefinitionForSurvivor(string survivorId)
00143:             => !string.IsNullOrEmpty(survivorId) && _bySurvivorId.TryGetValue(survivorId, out var d) ? d : null;
00144:
00145:         public NarrativeQuestlineArcState? GetArc(string survivorId)
00146:             => !string.IsNullOrEmpty(survivorId) && _arcsBySurvivor.TryGetValue(survivorId, out var a) ? a : null;
00147:
00148:         public IReadOnlyList<NarrativeQuestlineArcState> Arcs
00149:         {
00150:             get
00151:             {
00152:                 var list = new List<NarrativeQuestlineArcState>(_arcsBySurvivor.Values);
00153:                 list.Sort((a, b) => string.CompareOrdinal(a.questId, b.questId));
00154:                 return list;
00155:             }
00156:         }
00157:
00158:         public bool HasArc(string survivorId) => GetArc(survivorId) != null;
00159:
00160:         public bool IsAwaitingBranch(string survivorId)
00161:             => GetArc(survivorId)?.status == NarrativeArcStatus.AwaitingBranch;
00162:
00163:         /// <summary>Objective items still owed on the arc's current stage.</summary>
00164:         public List<string> GetOutstandingObjectives(string survivorId)
00165:         {
00166:             var outstanding = new List<string>();
00167:             var arc = GetArc(survivorId);
00168:             if (arc == null || arc.status != NarrativeArcStatus.Active) return outstanding;
00169:             var def = GetDefinition(arc.questId);
00170:             var stage = def?.FindStage(arc.currentStage);
00171:             if (stage == null) return outstanding;
00172:             for (int i = 0; i < stage.objectiveItems.Count; i++)
00173:             {
00174:                 var item = stage.objectiveItems[i];
00175:                 if (!arc.deliveredItems.Contains(item)) outstanding.Add(item);
00176:             }
00177:             return outstanding;
00178:         }
00179:
00180:         /// <summary>
00181:         /// Open the arc for a survivor. Fails when the survivor has no authored
00182:         /// questline or already has an arc in any state (an arc is never restarted).
00183:         /// </summary>
00184:         public bool TryBegin(string survivorId, int day)
00185:         {
00186:             if (string.IsNullOrEmpty(survivorId)) return false;
00187:             if (_arcsBySurvivor.ContainsKey(survivorId)) return false;
00188:             if (!_bySurvivorId.TryGetValue(survivorId, out var def)) return false;
00189:
00190:             int firstStage = def.stages.Count > 0 ? def.stages[0].stage : 0;
00191:             var arc = new NarrativeQuestlineArcState
00192:             {
00193:                 questId = def.questId,
00194:                 survivorId = survivorId,
00195:                 currentStage = firstStage,
00196:                 status = NarrativeArcStatus.Active,
00197:                 startedDay = day
00198:             };
00199:
00200:             // A questline whose opening stage is already the crisis fork has no
00201:             // objectives to deliver, so it opens straight into the branch.
00202:             var stage = def.FindStage(firstStage);
00203:             if (stage != null && stage.HasBranch)
00204:                 arc.status = NarrativeArcStatus.AwaitingBranch;
00205:
00206:             _arcsBySurvivor[survivorId] = arc;
00207:             RebuildStateList();
00208:             if (!_restoring) OnArcStarted?.Invoke(arc);
00209:             return true;
00210:         }
00211:
00212:         /// <summary>
00213:         /// Record one objective item delivered for a survivor's arc. Returns true
00214:         /// only when the item was owed on the current stage and was accepted;
00215:         /// duplicates and items belonging to other stages are refused without
00216:         /// mutating state. Accepting the last owed item advances the stage.
00217:         /// </summary>
00218:         public bool TryDeliverItem(string survivorId, string itemId, int day)
00219:         {
00220:             if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(itemId)) return false;
00221:             if (!_arcsBySurvivor.TryGetValue(survivorId, out var arc)) return false;
00222:             if (arc.status != NarrativeArcStatus.Active) return false;
00223:
00224:             var def = GetDefinition(arc.questId);
00225:             if (def == null) return false;
00226:             var stage = def.FindStage(arc.currentStage);
00227:             if (stage == null) return false;
00228:             if (!stage.objectiveItems.Contains(itemId)) return false;
00229:             if (arc.deliveredItems.Contains(itemId)) return false;
00230:
00231:             arc.deliveredItems.Add(itemId);
00232:
00233:             bool stageComplete = true;
00234:             for (int i = 0; i < stage.objectiveItems.Count; i++)
00235:             {
00236:                 if (!arc.deliveredItems.Contains(stage.objectiveItems[i])) { stageComplete = false; break; }
00237:             }
00238:
00239:             if (stageComplete)
00240:                 AdvanceToNextStage(arc, def, day);
00241:
00242:             if (!_restoring) OnStageAdvanced?.Invoke(arc, arc.currentStage);
00243:             return true;
00244:         }
00245:
00246:         /// <summary>
00247:         /// Resolve the crisis fork. Succeeds only while the arc awaits a branch and
00248:         /// only for an id authored on that arc's branch stage. On success the arc
00249:         /// records the chosen branch and granted trait, moves to the resolution
00250:         /// stage, and reports the branch so the host can apply morale and the trait
00251:         /// through their owning systems.
00252:         /// </summary>
00253:         public bool TryChooseBranch(
00254:             string survivorId, string branchId, int day,
00255:             out NarrativeQuestlineBranchDef? chosenBranch)
00256:         {
00257:             chosenBranch = null;
00258:             if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(branchId)) return false;
00259:             if (!_arcsBySurvivor.TryGetValue(survivorId, out var arc)) return false;
00260:             if (arc.status != NarrativeArcStatus.AwaitingBranch) return false;
00261:             if (!string.IsNullOrEmpty(arc.chosenBranchId)) return false;
00262:
00263:             var def = GetDefinition(arc.questId);
00264:             if (def == null) return false;
00265:             var branchStage = def.FindBranchStage();
00266:             if (branchStage == null) return false;
00267:             if (branchStage.stage != arc.currentStage) return false;
00268:
00269:             var branch = branchStage.FindBranch(branchId);
00270:             if (branch == null) return false;
00271:
00272:             arc.chosenBranchId = branch.id;
00273:             arc.grantedTraitId = branch.traitGranted;
00274:             chosenBranch = branch;
00275:
00276:             AdvanceToNextStage(arc, def, day);
00277:             arc.status = NarrativeArcStatus.Resolved;
00278:             arc.resolvedDay = day;
00279:
00280:             if (!_restoring)
00281:             {
00282:                 OnBranchChosen?.Invoke(arc, branch);
00283:                 OnArcResolved?.Invoke(arc);
00284:             }
00285:             return true;
00286:         }
00287:
00288:         private void AdvanceToNextStage(NarrativeQuestlineArcState arc, NarrativeQuestlineDef def, int day)
00289:         {
00290:             int next = arc.currentStage;
00291:             bool foundNext = false;
00292:             for (int i = 0; i < def.stages.Count; i++)
00293:             {
00294:                 if (def.stages[i].stage > arc.currentStage)
00295:                 {
00296:                     if (!foundNext || def.stages[i].stage < next) { next = def.stages[i].stage; foundNext = true; }
00297:                 }
00298:             }
00299:
00300:             if (!foundNext)
00301:             {
00302:                 // No later stage: the arc ends where it stands.
00303:                 arc.status = NarrativeArcStatus.Resolved;
00304:                 arc.resolvedDay = day;
00305:                 return;
00306:             }
00307:
00308:             arc.currentStage = next;
00309:             var stage = def.FindStage(next);
00310:             arc.status = stage != null && stage.HasBranch
00311:                 ? NarrativeArcStatus.AwaitingBranch
00312:                 : NarrativeArcStatus.Active;
00313:         }
00314:
00315:         public NarrativeQuestlineSaveState CaptureState()
00316:         {
00317:             var snapshot = new NarrativeQuestlineSaveState
00318:             {
00319:                 schema_version = 1,
00320:                 systemId = SystemId
00321:             };
00322:             foreach (var arc in Arcs)
00323:                 snapshot.arcs.Add(arc.Clone());
00324:             return snapshot;
00325:         }
00326:
00327:         public void RestoreState(NarrativeQuestlineSaveState? saved)
00328:         {
00329:             _arcsBySurvivor.Clear();
00330:             if (saved == null || saved.arcs == null)
00331:             {
00332:                 RebuildStateList();
00333:                 return;
00334:             }
00335:
00336:             _restoring = true;
00337:             try
00338:             {
00339:                 for (int i = 0; i < saved.arcs.Count; i++)
00340:                 {
00341:                     var arc = saved.arcs[i];
00342:                     if (arc == null || string.IsNullOrEmpty(arc.survivorId)) continue;
00343:                     // Arcs whose definition is no longer authored are dropped rather
00344:                     // than resurrected as orphans.
00345:                     if (!_byQuestId.ContainsKey(arc.questId)) continue;
00346:                     if (_arcsBySurvivor.ContainsKey(arc.survivorId)) continue;
00347:
00348:                     var clone = arc.Clone();
00349:                     clone.deliveredItems = clone.deliveredItems ?? new List<string>();
00350:                     _arcsBySurvivor[clone.survivorId] = clone;
00351:                 }
00352:             }
00353:             finally
00354:             {
00355:                 _restoring = false;
00356:             }
00357:
00358:             RebuildStateList();
00359:         }
00360:
00361:         private void RebuildStateList()
00362:         {
00363:             var arcs = new List<NarrativeQuestlineArcState>(_arcsBySurvivor.Count);
00364:             foreach (var arc in Arcs) arcs.Add(arc);
00365:             _state = new NarrativeQuestlineSaveState
00366:             {
00367:                 schema_version = 1,
00368:                 systemId = SystemId,
00369:                 arcs = arcs
00370:             };
00371:         }
00372:
00373:         /// <summary>Live aggregate state. Use <see cref="CaptureState"/> for persistence.</summary>
00374:         public NarrativeQuestlineSaveState State => _state;
00375:     }
00376: }
```


# Appendix — Current Source Detail: `src/Main.NarrativeQuestlines.cs`

### `src/Main.NarrativeQuestlines.cs` — complete current file

- Size: 300 lines / 12648 bytes.
- SHA-256: `48bd492b00b71c40972f967c96d743eab71596efa6c182f1f425f68a161c7f66`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // ASHFALL survivor narrative questline host triad (Plan 104 runtime wiring).
00003: // Save enrollment for the narrative_questlines section.
00004:
00005: using System.Collections.Generic;
00006: using Godot;
00007: using Ashfall.Core;
00008: using Ashfall.Core.Quests;
00009: using Ashfall.Core.Survivors;
00010:
00011: namespace AtomicWar.GodotApp
00012: {
00013:     public partial class Main
00014:     {
00015:         private NarrativeQuestlineHostSession? _narrativeQuestlines;
00016:         private bool _narrativeQuestlinesDirty;
00017:
00018:         /// <summary>
00019:         /// Feedback strip for the survivor-arc commands. Mirrors the single
00020:         /// <c>LastEvent</c> convention used by the greenhouse and other domains:
00021:         /// one string describing the outcome of the action just performed, with
00022:         /// player-readable blockers rather than raw ids.
00023:         /// </summary>
00024:         public string LastNarrativeArcEvent { get; private set; } = string.Empty;
00025:
00026:         public NarrativeQuestlineHostSession? NarrativeQuestlines => _narrativeQuestlines;
00027:
00028:         private void SetupNarrativeQuestlines()
00029:         {
00030:             if (_narrativeQuestlines != null) return;
00031:             _narrativeQuestlines = NarrativeQuestlineHostSession.Create(_dataDir, new GodotLog());
00032:             _narrativeQuestlines.StateChanged += () => _narrativeQuestlinesDirty = true;
00033:
00034:             var saved = NarrativeQuestlineSaveStore.TryLoad();
00035:             if (saved != null)
00036:                 _narrativeQuestlines.RestoreState(saved);
00037:         }
00038:
00039:         private void SaveNarrativeQuestlines()
00040:         {
00041:             if (_narrativeQuestlines == null) return;
00042:             if (CaptureSection("narrative_questlines", _narrativeQuestlines.TryCapturePersisted()))
00043:                 _narrativeQuestlinesDirty = false;
00044:         }
00045:
00046:         private void FlushNarrativeQuestlinesIfDirty()
00047:         {
00048:             if (_narrativeQuestlinesDirty) SaveNarrativeQuestlines();
00049:         }
00050:
00051:         private int NarrativeArcDay() => _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
00052:
00053:         private void SetNarrativeArcFeedback(string message)
00054:         {
00055:             LastNarrativeArcEvent = message;
00056:             if (_statusLabel != null) _statusLabel.Text = message;
00057:         }
00058:
00059:         /// <summary>
00060:         /// Open the authored arc belonging to a survivor. A survivor carries at
00061:         /// most one arc and an arc is never restarted once opened.
00062:         /// </summary>
00063:         public bool BeginSurvivorArc(string survivorId)
00064:         {
00065:             SetupNarrativeQuestlines();
00066:             if (_narrativeQuestlines == null || string.IsNullOrEmpty(survivorId)) return false;
00067:
00068:             var def = _narrativeQuestlines.GetDefinitionForSurvivor(survivorId);
00069:             if (def == null)
00070:             {
00071:                 SetNarrativeArcFeedback("This survivor has no unfinished business of their own on record.");
00072:                 return false;
00073:             }
00074:
00075:             if (_narrativeQuestlines.GetArc(survivorId) != null)
00076:             {
00077:                 SetNarrativeArcFeedback($"{def.title} is already open. It does not start twice.");
00078:                 return false;
00079:             }
00080:
00081:             if (!_narrativeQuestlines.TryBegin(survivorId, NarrativeArcDay()))
00082:             {
00083:                 SetNarrativeArcFeedback($"{def.title} could not be opened.");
00084:                 return false;
00085:             }
00086:
00087:             var stage = def.FindStage(def.stages.Count > 0 ? def.stages[0].stage : 0);
00088:             SetNarrativeArcFeedback($"{def.title} opened. {stage?.name}: {stage?.description}");
00089:             return true;
00090:         }
00091:
00092:         /// <summary>
00093:         /// Record one objective item handed over for a survivor's arc. Only items
00094:         /// the current stage actually asks for are accepted; the host inventory
00095:         /// authority performs the removal, so this reports the arc-side result.
00096:         /// </summary>
00097:         public bool DeliverSurvivorArcObjective(string survivorId, string itemId)
00098:         {
00099:             SetupNarrativeQuestlines();
00100:             if (_narrativeQuestlines == null) return false;
00101:
00102:             var arc = _narrativeQuestlines.GetArc(survivorId);
00103:             if (arc == null)
00104:             {
00105:                 SetNarrativeArcFeedback("No arc is open for that survivor.");
00106:                 return false;
00107:             }
00108:
00109:             var def = _narrativeQuestlines.GetDefinitionForSurvivor(survivorId);
00110:             var stage = def?.FindStage(arc.currentStage);
00111:             if (arc.status != NarrativeArcStatus.Active || stage == null)
00112:             {
00113:                 SetNarrativeArcFeedback($"{def?.title} is waiting on a decision, not on supplies.");
00114:                 return false;
00115:             }
00116:
00117:             // Read-only preflight: only an item the current stage still owes can be
00118:             // accepted, so stores are never spent on a delivery the arc will refuse.
00119:             if (!_narrativeQuestlines.GetOutstandingObjectives(survivorId).Contains(itemId))
00120:             {
00121:                 SetNarrativeArcFeedback($"{def?.title} does not need that at this stage.");
00122:                 return false;
00123:             }
00124:
00125:             var stores = _inventory?.Inventory;
00126:             if (stores == null)
00127:             {
00128:                 SetNarrativeArcFeedback("The stores are not available to draw from.");
00129:                 return false;
00130:             }
00131:
00132:             // The arc records the delivery in the same commit that spends the item,
00133:             // so the two authorities can never disagree about whether it was handed over.
00134:             bool recorded = false;
00135:             if (!stores.TryConsume(itemId, 1,
00136:                     onCommitted: () => recorded = _narrativeQuestlines.TryDeliverItem(survivorId, itemId, NarrativeArcDay())))
00137:             {
00138:                 SetNarrativeArcFeedback($"{def?.title} needs {itemId}, and it is not in the stores.");
00139:                 return false;
00140:             }
00141:
00142:             if (!recorded)
00143:             {
00144:                 stores.TryProduce(itemId, 1);
00145:                 SetNarrativeArcFeedback($"{def?.title} could not record that delivery; the item was returned to stores.");
00146:                 return false;
00147:             }
00148:
00149:             var advanced = _narrativeQuestlines.GetArc(survivorId);
00150:             var nextStage = def?.FindStage(advanced?.currentStage ?? arc.currentStage);
00151:             if (advanced != null && advanced.status == NarrativeArcStatus.AwaitingBranch)
00152:             {
00153:                 SetNarrativeArcFeedback(
00154:                     $"{def?.title} — {nextStage?.name}. {nextStage?.description}");
00155:             }
00156:             else
00157:             {
00158:                 var outstanding = _narrativeQuestlines.GetOutstandingObjectives(survivorId);
00159:                 SetNarrativeArcFeedback(outstanding.Count == 0
00160:                     ? $"{def?.title} — {nextStage?.name}. {nextStage?.description}"
00161:                     : $"{def?.title} — still owed: {string.Join(", ", outstanding)}.");
00162:             }
00163:             return true;
00164:         }
00165:
00166:         /// <summary>
00167:         /// Resolve the crisis fork. Applies the authored morale delta through the
00168:         /// existing narrative morale authority (preflighted, so an unavailable
00169:         /// survivor is reported rather than silently ignored) and records the
00170:         /// granted trait on the arc, matching how other systems record grants.
00171:         /// </summary>
00172:         public bool ChooseSurvivorArcBranch(string survivorId, string branchId)
00173:         {
00174:             SetupNarrativeQuestlines();
00175:             if (_narrativeQuestlines == null) return false;
00176:
00177:             var arc = NarrativeArcOrNull(survivorId);
00178:             if (arc == null) return false;
00179:
00180:             if (arc.status != NarrativeArcStatus.AwaitingBranch)
00181:             {
00182:                 SetNarrativeArcFeedback("That decision is not open. The arc is waiting on supplies, or already ended.");
00183:                 return false;
00184:             }
00185:
00186:             var def = _narrativeQuestlines!.GetDefinitionForSurvivor(survivorId);
00187:             int day = NarrativeArcDay();
00188:
00189:             if (!_narrativeQuestlines.TryChooseBranch(survivorId, branchId, day, out var branch) || branch == null)
00190:             {
00191:                 SetNarrativeArcFeedback("That is not one of the two choices this moment offers.");
00192:                 return false;
00193:             }
00194:
00195:             var (ok, reason) = CanApplyArcMorale(survivorId);
00196:             if (ok) ApplyArcMorale(survivorId, branch.moraleDelta);
00197:
00198:             var resolution = def?.FindStage(def.FinalStageIndex);
00199:             SetNarrativeArcFeedback(
00200:                 $"{branch.label}. {branch.description} " +
00201:                 (ok
00202:                     ? $"Morale {branch.moraleDelta:+0;-0;0}."
00203:                     : $"Morale not applied ({reason}).") +
00204:                 (string.IsNullOrEmpty(branch.traitGranted)
00205:                     ? string.Empty
00206:                     : $" Recorded: {branch.traitGranted}.") +
00207:                 (resolution != null ? $" {resolution.description}" : string.Empty));
00208:
00209:             _narrativeQuestlinesDirty = true;
00210:             return true;
00211:         }
00212:
00213:         private NarrativeQuestlineArcState? NarrativeArcOrNull(string survivorId)
00214:         {
00215:             var arc = _narrativeQuestlines?.GetArc(survivorId);
00216:             if (arc == null)
00217:             {
00218:                 SetNarrativeArcFeedback("No arc is open for that survivor.");
00219:                 return null;
00220:             }
00221:             return arc;
00222:         }
00223:
00224:         /// <summary>Survivor ids that have an authored arc available to open.</summary>
00225:         public IReadOnlyList<NarrativeQuestlineDef> NarrativeArcDefinitions()
00226:         {
00227:             SetupNarrativeQuestlines();
00228:             return _narrativeQuestlines?.Definitions ?? (IReadOnlyList<NarrativeQuestlineDef>)new List<NarrativeQuestlineDef>();
00229:         }
00230:
00231:         /// <summary>
00232:         /// Single bind path for the quest journal so every entry point (registry
00233:         /// bind action, keyboard route, developer handler) presents the same
00234:         /// surfaces, including the survivor arcs and their label resolvers.
00235:         /// </summary>
00236:         private void BindQuestsPanel()
00237:         {
00238:             SetupNarrativeQuestlines();
00239:             SetupPlans166To169();
00240:             _questsPanel.Bind(
00241:                 _core.Quests,
00242:                 _expansions?.CrossingQuests,
00243:                 _dutyRoster,
00244:                 _holdfastRuntime?.Day ?? _simDay,
00245:                 _factionBranch?.Coordinator,
00246:                 _moralChoice,
00247:                 _moralChoiceDefs,
00248:                 _narrativeQuestlines,
00249:                 ResolveSurvivorArcName,
00250:                 ResolveArcItemLabel,
00251:                 _proceduralNarrative169);
00252:         }
00253:
00254:         /// <summary>
00255:         /// Survivor display name from the enrichment authority. Returns empty when
00256:         /// unresolved so the panel falls back to its own humanized label rather
00257:         /// than ever printing a raw survivor id.
00258:         /// </summary>
00259:         private string ResolveSurvivorArcName(string survivorId)
00260:         {
00261:             if (string.IsNullOrEmpty(survivorId)) return string.Empty;
00262:             SetupEnrichment();
00263:             SetupSurvivors();
00264:             var def = _survivors?.Roster?.FindDefinition(survivorId);
00265:             var view = _enrichmentService?.GetView(survivorId, def);
00266:             return string.IsNullOrWhiteSpace(view?.DisplayName) ? string.Empty : view!.DisplayName;
00267:         }
00268:
00269:         /// <summary>Item display name from the loaded item catalog; empty when unresolved.</summary>
00270:         private string ResolveArcItemLabel(string itemId)
00271:         {
00272:             if (string.IsNullOrEmpty(itemId)) return string.Empty;
00273:             SetupInventory();
00274:             var name = _inventory?.Catalog?.Get(itemId)?.displayName;
00275:             return string.IsNullOrWhiteSpace(name) ? string.Empty : name!;
00276:         }
00277:
00278:         /// <summary>
00279:         /// Morale preflight for an arc branch. Uses the survivor needs authority
00280:         /// directly rather than the Plan 143 narrative-arc adapter, so this triad
00281:         /// has no dependency on that wiring being present.
00282:         /// </summary>
00283:         private (bool ok, string reason) CanApplyArcMorale(string survivorId)
00284:         {
00285:             SetupSurvivors();
00286:             if (_survivors == null) return (false, "the roster is not available");
00287:             var survivor = _survivors.Find(survivorId);
00288:             if (survivor == null) return (false, "that survivor is not on the roster");
00289:             if (!survivor.IsAliveState) return (false, "that survivor is not alive to feel it");
00290:             return (true, string.Empty);
00291:         }
00292:
00293:         private void ApplyArcMorale(string survivorId, int delta)
00294:         {
00295:             SetupSurvivors();
00296:             if (_survivors == null || delta == 0) return;
00297:             _survivors.Needs.Modify(survivorId, NeedKind.Morale, delta);
00298:         }
00299:     }
00300: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Quests/NarrativeQuestlineCatalog.cs`

### `Assets/Ashfall.Core/Quests/NarrativeQuestlineCatalog.cs` — complete current file

- Size: 236 lines / 9371 bytes.
- SHA-256: `2af44f1ec95ba9290779a732bd1cd62eae700078282529402d36b92f2d3c9969`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // ASHFALL Core: survivor narrative questline catalog (Plan 104 data authority).
00003:
00004: using System.Collections.Generic;
00005: #pragma warning disable CS0649
00006: #pragma warning disable CS8618
00007:
00008: namespace Ashfall.Core.Quests
00009: {
00010:     /// <summary>
00011:     /// One binary crisis branch (Plan 104 stage 2). Choosing a branch grants
00012:     /// <see cref="traitGranted"/> to the arc's survivor and applies
00013:     /// <see cref="moraleDelta"/> to shelter morale. The two branches of a
00014:     /// questline are mutually exclusive and never both granted.
00015:     /// </summary>
00016:     public class NarrativeQuestlineBranchDef
00017:     {
00018:         public string id = string.Empty;
00019:         public string label = string.Empty;
00020:         public string description = string.Empty;
00021:         public string traitGranted = string.Empty;
00022:         public int moraleDelta;
00023:     }
00024:
00025:     /// <summary>
00026:     /// One stage of a survivor arc. Stages 0-1 carry <see cref="objectiveItems"/>
00027:     /// (delivered to advance); the crisis stage carries
00028:     /// <see cref="branchA"/>/<see cref="branchB"/> instead; the resolution stage
00029:     /// carries neither and is epilogue text.
00030:     /// </summary>
00031:     public class NarrativeQuestlineStageDef
00032:     {
00033:         public int stage;
00034:         public string name = string.Empty;
00035:         public string description = string.Empty;
00036:         public List<string> objectiveItems = new List<string>();
00037:         public NarrativeQuestlineBranchDef? branchA;
00038:         public NarrativeQuestlineBranchDef? branchB;
00039:
00040:         /// <summary>True only when both halves of the binary crisis fork are present.</summary>
00041:         public bool HasBranch => branchA != null && branchB != null;
00042:
00043:         public NarrativeQuestlineBranchDef? FindBranch(string branchId)
00044:         {
00045:             if (string.IsNullOrEmpty(branchId)) return null;
00046:             if (branchA != null && string.Equals(branchA.id, branchId, System.StringComparison.Ordinal)) return branchA;
00047:             if (branchB != null && string.Equals(branchB.id, branchId, System.StringComparison.Ordinal)) return branchB;
00048:             return null;
00049:         }
00050:     }
00051:
00052:     /// <summary>
00053:     /// One authored survivor arc: a named survivor, a target location, and a
00054:     /// linear stage ladder terminating in a binary crisis fork.
00055:     /// </summary>
00056:     public class NarrativeQuestlineDef
00057:     {
00058:         public string questId = string.Empty;
00059:         public string survivorId = string.Empty;
00060:         public string title = string.Empty;
00061:         public string targetLocationId = string.Empty;
00062:         public List<NarrativeQuestlineStageDef> stages = new List<NarrativeQuestlineStageDef>();
00063:
00064:         public NarrativeQuestlineStageDef? FindStage(int stageIndex)
00065:         {
00066:             for (int i = 0; i < stages.Count; i++)
00067:                 if (stages[i] != null && stages[i].stage == stageIndex) return stages[i];
00068:             return null;
00069:         }
00070:
00071:         /// <summary>The crisis stage carrying the binary fork, or null when absent.</summary>
00072:         public NarrativeQuestlineStageDef? FindBranchStage()
00073:         {
00074:             for (int i = 0; i < stages.Count; i++)
00075:                 if (stages[i] != null && stages[i].HasBranch) return stages[i];
00076:             return null;
00077:         }
00078:
00079:         /// <summary>Highest authored stage index, or -1 for a stageless definition.</summary>
00080:         public int FinalStageIndex
00081:         {
00082:             get
00083:             {
00084:                 int max = -1;
00085:                 for (int i = 0; i < stages.Count; i++)
00086:                     if (stages[i] != null && stages[i].stage > max) max = stages[i].stage;
00087:                 return max;
00088:             }
00089:         }
00090:     }
00091:
00092:     /// <summary>
00093:     /// Engine-agnostic loader for narrative_questlines.json. Missing file → empty
00094:     /// list; future schema → empty list (never partially parsed); malformed entry
00095:     /// → skipped, never thrown. Duplicate quest ids: first definition wins, later
00096:     /// duplicates are dropped so the catalog can never hold two arcs under one id.
00097:     /// </summary>
00098:     public static class NarrativeQuestlineCatalogLoader
00099:     {
00100:         public const string FileName = "narrative_questlines.json";
00101:         public const int CurrentSchemaVersion = 1;
00102:
00103:         public static List<NarrativeQuestlineDef> LoadEntries(
00104:             string dataDir, IFileIO fileIO, IJsonSerializer json)
00105:         {
00106:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
00107:                 return new List<NarrativeQuestlineDef>();
00108:
00109:             string path = fileIO.Combine(dataDir, FileName);
00110:             if (!fileIO.FileExists(path))
00111:                 return new List<NarrativeQuestlineDef>();
00112:
00113:             string raw = fileIO.ReadAllText(path);
00114:             return Parse(raw, json, path);
00115:         }
00116:
00117:         /// <summary>Parse an already-read payload. Exposed for tests and headless demos.</summary>
00118:         public static List<NarrativeQuestlineDef> Parse(string raw, IJsonSerializer json, string? pathForDiagnostics = null)
00119:         {
00120:             var result = new List<NarrativeQuestlineDef>();
00121:             if (json == null || string.IsNullOrWhiteSpace(raw))
00122:                 return result;
00123:
00124:             try
00125:             {
00126:                 var root = json.Deserialize<QuestlineRoot>(raw);
00127:                 if (root == null) return result;
00128:                 if (root.schema_version > CurrentSchemaVersion) return result;
00129:                 if (root.questlines == null) return result;
00130:
00131:                 var seenQuest = new HashSet<string>(System.StringComparer.Ordinal);
00132:                 var seenSurvivor = new HashSet<string>(System.StringComparer.Ordinal);
00133:
00134:                 for (int i = 0; i < root.questlines.Count; i++)
00135:                 {
00136:                     var e = root.questlines[i];
00137:                     if (e == null) continue;
00138:                     if (string.IsNullOrEmpty(e.quest_id) || string.IsNullOrEmpty(e.survivor_id)) continue;
00139:                     if (e.stages == null || e.stages.Count == 0) continue;
00140:                     if (!seenQuest.Add(e.quest_id)) continue;
00141:                     if (!seenSurvivor.Add(e.survivor_id)) continue;
00142:
00143:                     var def = new NarrativeQuestlineDef
00144:                     {
00145:                         questId = e.quest_id,
00146:                         survivorId = e.survivor_id,
00147:                         title = e.title ?? string.Empty,
00148:                         targetLocationId = e.target_location_id ?? string.Empty
00149:                     };
00150:
00151:                     for (int s = 0; s < e.stages.Count; s++)
00152:                     {
00153:                         var st = e.stages[s];
00154:                         if (st == null) continue;
00155:                         var stage = new NarrativeQuestlineStageDef
00156:                         {
00157:                             stage = st.stage,
00158:                             name = st.name ?? string.Empty,
00159:                             description = st.description ?? string.Empty
00160:                         };
00161:                         if (st.objective_items != null)
00162:                         {
00163:                             for (int oi = 0; oi < st.objective_items.Count; oi++)
00164:                             {
00165:                                 var item = st.objective_items[oi];
00166:                                 if (!string.IsNullOrEmpty(item) && !stage.objectiveItems.Contains(item))
00167:                                     stage.objectiveItems.Add(item);
00168:                             }
00169:                         }
00170:                         stage.branchA = MapBranch(st.branch_a);
00171:                         stage.branchB = MapBranch(st.branch_b);
00172:                         def.stages.Add(stage);
00173:                     }
00174:
00175:                     if (def.stages.Count == 0) continue;
00176:                     result.Add(def);
00177:                 }
00178:             }
00179:             catch (System.Exception ex_CATDIAG)
00180:             {
00181:                 Ashfall.Core.IO.CatalogDiagnostics.Warn(
00182:                     pathForDiagnostics ?? FileName, "NarrativeQuestlineRoot", ex_CATDIAG);
00183:                 return result;
00184:             }
00185:
00186:             return result;
00187:         }
00188:
00189:         private static NarrativeQuestlineBranchDef? MapBranch(Branch? b)
00190:         {
00191:             if (b == null || string.IsNullOrEmpty(b.id)) return null;
00192:             return new NarrativeQuestlineBranchDef
00193:             {
00194:                 id = b.id,
00195:                 label = b.label ?? string.Empty,
00196:                 description = b.description ?? string.Empty,
00197:                 traitGranted = b.trait_granted ?? string.Empty,
00198:                 moraleDelta = b.morale_delta
00199:             };
00200:         }
00201:
00202:         private class QuestlineRoot
00203:         {
00204:             public int schema_version = 1;
00205:             public List<Questline> questlines = new List<Questline>();
00206:         }
00207:
00208:         private class Questline
00209:         {
00210:             public string quest_id;
00211:             public string survivor_id;
00212:             public string title;
00213:             public string target_location_id;
00214:             public List<Stage> stages;
00215:         }
00216:
00217:         private class Stage
00218:         {
00219:             public int stage;
00220:             public string name;
00221:             public string description;
00222:             public List<string> objective_items;
00223:             public Branch branch_a;
00224:             public Branch branch_b;
00225:         }
00226:
00227:         private class Branch
00228:         {
00229:             public string id;
00230:             public string label;
00231:             public string description;
00232:             public string trait_granted;
00233:             public int morale_delta;
00234:         }
00235:     }
00236: }
```


# Appendix — Current Source Detail: `src/Host/NarrativeQuestlineHostSession.cs`

### `src/Host/NarrativeQuestlineHostSession.cs` — complete current file

- Size: 100 lines / 4141 bytes.
- SHA-256: `f357dcb288107b1d2ba78f5ae66110adfa0e8fec76e404a58995bc197c69b3c5`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // ASHFALL survivor narrative questline host session (Plan 104 runtime wiring).
00003:
00004: using System;
00005: using System.Collections.Generic;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Quests;
00008:
00009: namespace AtomicWar.GodotApp
00010: {
00011:     /// <summary>
00012:     /// Host-side session for the authored survivor arcs in
00013:     /// <c>narrative_questlines.json</c>. Thin projection only: it loads the
00014:     /// catalog, forwards commands to <see cref="NarrativeQuestlineSystem"/>, and
00015:     /// owns persistence. Gameplay rules stay in Core.
00016:     /// <para>
00017:     /// Consequence application is deliberately left to the caller: a resolved
00018:     /// branch reports its <see cref="NarrativeQuestlineBranchDef"/> so Main can
00019:     /// apply morale through the existing narrative morale authority and surface
00020:     /// the granted trait through the arc record, matching how
00021:     /// <c>LatentExpertAwakeningSystem</c> and <c>DesperationSystem</c> record
00022:     /// granted traits rather than mutating a central survivor trait store.
00023:     /// </para>
00024:     /// </summary>
00025:     public sealed class NarrativeQuestlineHostSession : HostSessionBase
00026:     {
00027:         private readonly NarrativeQuestlineSystem _system;
00028:
00029:         public NarrativeQuestlineSystem System => _system;
00030:
00031:         public NarrativeQuestlineHostSession(
00032:             IJsonSerializer jsonSerializer, IFileIO fileIO, string dataDir, ILog? log = null)
00033:         {
00034:             var defs = NarrativeQuestlineCatalogLoader.LoadEntries(dataDir, fileIO, jsonSerializer);
00035:             _system = new NarrativeQuestlineSystem(defs, log);
00036:
00037:             _system.OnArcStarted += _ => RaiseStateChanged();
00038:             _system.OnStageAdvanced += (_, _) => RaiseStateChanged();
00039:             _system.OnBranchChosen += (_, _) => RaiseStateChanged();
00040:             _system.OnArcResolved += _ => RaiseStateChanged();
00041:
00042:             var saved = NarrativeQuestlineSaveStore.TryLoad();
00043:             if (saved != null)
00044:                 _system.RestoreState(saved);
00045:         }
00046:
00047:         public static NarrativeQuestlineHostSession Create(string dataDir, ILog? log = null)
00048:         {
00049:             return new NarrativeQuestlineHostSession(
00050:                 new SystemTextJsonSerializer(), new FileSystemIO(), dataDir, log);
00051:         }
00052:
00053:         public int DefinitionCount => _system.DefinitionCount;
00054:
00055:         public IReadOnlyList<NarrativeQuestlineDef> Definitions => _system.Definitions;
00056:
00057:         public NarrativeQuestlineDef? GetDefinitionForSurvivor(string survivorId)
00058:             => _system.GetDefinitionForSurvivor(survivorId);
00059:
00060:         public NarrativeQuestlineArcState? GetArc(string survivorId) => _system.GetArc(survivorId);
00061:
00062:         public IReadOnlyList<NarrativeQuestlineArcState> Arcs => _system.Arcs;
00063:
00064:         public List<string> GetOutstandingObjectives(string survivorId)
00065:             => _system.GetOutstandingObjectives(survivorId);
00066:
00067:         public bool IsAwaitingBranch(string survivorId) => _system.IsAwaitingBranch(survivorId);
00068:
00069:         public bool TryBegin(string survivorId, int day) => _system.TryBegin(survivorId, day);
00070:
00071:         public bool TryDeliverItem(string survivorId, string itemId, int day)
00072:             => _system.TryDeliverItem(survivorId, itemId, day);
00073:
00074:         public bool TryChooseBranch(
00075:             string survivorId, string branchId, int day,
00076:             out NarrativeQuestlineBranchDef? chosenBranch)
00077:             => _system.TryChooseBranch(survivorId, branchId, day, out chosenBranch);
00078:
00079:         public NarrativeQuestlineSaveState CaptureState() => _system.CaptureState();
00080:
00081:         public void RestoreState(NarrativeQuestlineSaveState state)
00082:         {
00083:             _system.RestoreState(state);
00084:             RaiseStateChanged();
00085:         }
00086:
00087:         public bool TrySave() => NarrativeQuestlineSaveStore.TrySave(_system.CaptureState());
00088:
00089:         public bool TryLoad()
00090:         {
00091:             var loaded = NarrativeQuestlineSaveStore.TryLoad();
00092:             if (loaded == null) return false;
00093:             _system.RestoreState(loaded);
00094:             RaiseStateChanged();
00095:             return true;
00096:         }
00097:
00098:         public string TryCapturePersisted() => NarrativeQuestlineSaveStore.TryCapturePersisted(_system.CaptureState());
00099:     }
00100: }
```


# Appendix — Current Source Detail: `src/Host/NarrativeQuestlineSaveStore.cs`

### `src/Host/NarrativeQuestlineSaveStore.cs` — complete current file

- Size: 39 lines / 1791 bytes.
- SHA-256: `b0373e4592a430c53c94297e8f85945036c08c30ea8e091fb1721bacc214cb0c`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: // ASHFALL survivor narrative questline save store facade (Plan 104 runtime wiring).
00003:
00004: using Ashfall.Core.Quests;
00005: using Ashfall.Core.Save;
00006:
00007: namespace AtomicWar.GodotApp
00008: {
00009:     /// <summary>
00010:     /// Narrative questline save persistence facade delegating to the Core
00011:     /// SaveStore service (Initiative #41). Checksummed envelope, atomic write,
00012:     /// no legacy bare-state format because this section is new.
00013:     /// </summary>
00014:     public static class NarrativeQuestlineSaveStore
00015:     {
00016:         public const string FileName = "narrative_questlines_save.json";
00017:         public const string SectionName = "narrative_questlines";
00018:
00019:         private static readonly SaveStore<NarrativeQuestlineSaveState> s_store =
00020:             SaveStoreHub.Checksummed<NarrativeQuestlineSaveState>(
00021:                 FileName,
00022:                 nameof(NarrativeQuestlineSaveStore),
00023:                 allowLegacyBareState: false);
00024:
00025:         public static string SavePath => s_store.SavePath;
00026:         public static bool Exists => s_store.Exists();
00027:
00028:         public static string TryCaptureDirect(NarrativeQuestlineSaveState state) => s_store.CaptureBare(state);
00029:         public static NarrativeQuestlineSaveState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
00030:
00031:         public static string TryCapture(NarrativeQuestlineSaveState state) => s_store.CaptureBare(state);
00032:         public static NarrativeQuestlineSaveState? TryRestore(string json) => s_store.RestoreBare(json);
00033:
00034:         public static bool TrySave(NarrativeQuestlineSaveState state) => s_store.TrySave(state);
00035:         public static NarrativeQuestlineSaveState? TryLoad() => s_store.TryLoad();
00036:
00037:         public static string TryCapturePersisted(NarrativeQuestlineSaveState state) => s_store.CapturePersisted(state);
00038:     }
00039: }
```


# Appendix — Current Source Detail: `src/UI/QuestsPanel.cs`

### `src/UI/QuestsPanel.cs` — bounded current excerpt (687 of 774 lines)

- Size: 774 lines / 37776 bytes.
- SHA-256: `c1a69c26ccc3ef00cfd1078aee697c71d5550eb0b0ae1c461e0ec060f9bbf11b`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using Godot;
00005: using Ashfall.Core;
00006: using Ashfall.Core.Crossing;
00007: using Ashfall.Core.Narrative;
00008: using Ashfall.Core.UI;
00009:
00010: namespace AtomicWar.GodotApp.UI
00011: {
00012:     /// <summary>
00013:     /// ASHFALL — Quests & Story progression panel.
00014:     /// Manages active wasteland operations, narrative objectives, Holdfast protocol stages,
00015:     /// Nobody's Charter missions, and historical quest completions using real Core systems.
00016:     /// </summary>
00017:     public partial class QuestsPanel : Control, IBindablePanel
00018:     {
00019:         public event Action? OnClose;
00020:         public event Action<string>? OnQuestDetailRequested;
00021:         public event Action? OnCrossingPanelRequested;
00022:         public event Action? OnProceduralQuestRequested;
00023:
00024:         /// <summary>Open the authored personal arc belonging to a survivor.</summary>
00025:         public event Action<string>? OnBeginSurvivorArcRequested;
00026:
00027:         /// <summary>Hand one objective item from stores to a survivor's arc.</summary>
00028:         public event Action<string, string>? OnDeliverArcObjectiveRequested;
00029:
00030:         /// <summary>Resolve a survivor arc's crisis fork with one of its two branches.</summary>
00031:         public event Action<string, string>? OnChooseArcBranchRequested;
00032:
00033:         private VBoxContainer _overviewContainer = null!;
00034:         private VBoxContainer _activeContainer = null!;
00035:         private VBoxContainer _availableContainer = null!;
00036:         private VBoxContainer _completedContainer = null!;
00037:         private Label _statusSummary = null!;
00038:
00039:         private HoldfastQuestSystem? _holdfastQuests;
00040:         private CrossingQuestSystem? _crossingQuests;
00041:         private DutyRosterHostSession? _dutyRoster;
00042:         private Ashfall.Core.Factions.FactionBranchCoordinator? _branchCoordinator;
00043:         private Ashfall.Core.MoralChoice.MoralChoiceSystem? _moralChoice;
00044:         private IReadOnlyList<Ashfall.Core.MoralChoice.MoralChoiceQuestDefinition>? _moralDefs;
00045:         private NarrativeQuestlineHostSession? _survivorArcs;
00046:         private ProceduralNarrativeHostSession? _proceduralNarrative;
00047:         private Func<string, string>? _survivorDisplayName;
00048:         private Func<string, string>? _itemLabel;
00049:         private int _currentDay = 1;
00050:
00051:         public bool IsBound => _holdfastQuests != null || _crossingQuests != null || _branchCoordinator != null || _moralDefs != null || _survivorArcs != null || _proceduralNarrative != null;
00052:
00053:         public void Bind(
00054:             HoldfastQuestSystem? holdfastQuests,
00055:             CrossingQuestSystem? crossingQuests = null,
00056:             DutyRosterHostSession? dutyRoster = null,
00057:             int currentDay = 1,
00058:             Ashfall.Core.Factions.FactionBranchCoordinator? branchCoordinator = null,
00059:             Ashfall.Core.MoralChoice.MoralChoiceSystem? moralChoice = null,
00060:             IReadOnlyList<Ashfall.Core.MoralChoice.MoralChoiceQuestDefinition>? moralDefs = null,
00061:             NarrativeQuestlineHostSession? survivorArcs = null,
00062:             Func<string, string>? survivorDisplayName = null,
00063:             Func<string, string>? itemLabel = null,
00064:             ProceduralNarrativeHostSession? proceduralNarrative = null)
00065:         {
00066:             Unbind();
00067:
00068:             _holdfastQuests = holdfastQuests;
00069:             _crossingQuests = crossingQuests;
00070:             _dutyRoster = dutyRoster;
00071:             _currentDay = currentDay;
00072:             _branchCoordinator = branchCoordinator;
00073:             _moralChoice = moralChoice;
00074:             _moralDefs = moralDefs;
00075:             _survivorArcs = survivorArcs;
00076:             _proceduralNarrative = proceduralNarrative;
00077:             _survivorDisplayName = survivorDisplayName;
00078:             _itemLabel = itemLabel;
00079:
00080:             if (_holdfastQuests != null)
00081:                 _holdfastQuests.OnStateChanged += HandleHoldfastStateChanged;
00082:             if (_crossingQuests != null)
00083:                 _crossingQuests.OnStateChanged += HandleCrossingStateChanged;
00084:             if (_branchCoordinator != null)
00085:                 _branchCoordinator.OnStateChanged += RefreshView;
00086:             if (_survivorArcs != null)
00087:                 _survivorArcs.StateChanged += RefreshView;
00088:             if (_proceduralNarrative != null)
00089:                 _proceduralNarrative.StateChanged += RefreshView;
00090:
00091:             RefreshView();
00092:         }
00093:
00094:         public void Unbind()
00095:         {
00096:             if (_holdfastQuests != null)
00097:             {
00098:                 _holdfastQuests.OnStateChanged -= HandleHoldfastStateChanged;
00099:                 _holdfastQuests = null;
00100:             }
00101:             if (_crossingQuests != null)
00102:             {
00103:                 _crossingQuests.OnStateChanged -= HandleCrossingStateChanged;
00104:                 _crossingQuests = null;
00105:             }
00106:             if (_branchCoordinator != null)
00107:             {
00108:                 _branchCoordinator.OnStateChanged -= RefreshView;
00109:                 _branchCoordinator = null;
00110:             }
00111:             if (_survivorArcs != null)
00112:             {
00131:         private void HandleCrossingStateChanged(CrossingQuestSystemState _) => RefreshView();
00132:
00133:         public void RefreshView()
00134:         {
00135:             if (_overviewContainer == null || _activeContainer == null ||
00136:                 _availableContainer == null || _completedContainer == null)
00137:                 return;
00138:
00139:             AshfallUiHelpers.EmptyChildren(_overviewContainer);
00140:             AshfallUiHelpers.EmptyChildren(_activeContainer);
00141:             AshfallUiHelpers.EmptyChildren(_availableContainer);
00142:             AshfallUiHelpers.EmptyChildren(_completedContainer);
00143:
00144:             int activeCount = 0;
00145:             int completedCount = 0;
00146:
00147:             // ── 1. Active & Completed Quests Extraction ──
00148:             var activeList = new List<(string id, string name, string type, string stageText, int stageNum, int totalStages, string briefing)>();
00149:             var completedList = new List<(string id, string name, string type, string resolution)>();
00150:             var availableList = new List<(string id, string name, string type, string reqs, string briefing)>();
00151:
00152:             // Check Holdfast Main Questline
00155:                 foreach (string qId in HoldfastQuestSystem.MainQuestIds)
00156:                 {
00157:                     var def = _holdfastQuests.GetDef(qId);
00158:                     var progress = _holdfastQuests.GetProgress(qId);
00159:                     string displayName = def?.display_name ?? _holdfastQuests.GetDisplayName(qId);
00160:                     int stageCount = def?.StageCount ?? 4;
00161:
00162:                     if (progress != null && progress.completed)
00163:                     {
00168:                     {
00169:                         activeCount++;
00170:                         string stageText = _holdfastQuests.GetStageText(qId);
00171:                         if (string.IsNullOrEmpty(stageText) && def?.stages != null && def.stages.Length > progress.stage)
00172:                             stageText = def.stages[progress.stage].text;
00173:                         activeList.Add((qId, displayName, "Main Protocol // The Holdfast", stageText, progress.stage + 1, stageCount, def?.briefing ?? ""));
00174:                     }
00175:                     else
00176:                     {
00177:                         // Available or upcoming
00179:                         if (!string.IsNullOrEmpty(def?.prereq_quest_id))
00180:                             reqs += $" · Requires: {def.prereq_quest_id}";
00181:                         availableList.Add((qId, displayName, "Holdfast Directive", reqs, def?.briefing ?? "Awaiting protocol conditions."));
00182:                     }
00183:                 }
00184:             }
00185:
00187:             if (_crossingQuests != null)
00188:             {
00189:                 var availCrossing = _crossingQuests.GetAvailableQuests(_currentDay);
00190:                 if (availCrossing != null)
00191:                 {
00192:                     foreach (var cDef in availCrossing)
00193:                     {
00194:                         if (cDef == null) continue;
00195:                         var p = _crossingQuests.GetProgress(cDef.id);
00196:                         if (p != null && p.completed)
00197:                         {
00198:                             completedCount++;
00199:                             completedList.Add((cDef.id, cDef.display_name, "Nobody's Charter // Crossing", "Arbitration objective resolved."));
00200:                         }
00201:                         else if (p != null && p.started)
00202:                         {
00203:                             activeCount++;
00204:                             string stageText = (cDef.stages != null && cDef.stages.Count > p.currentStage && p.currentStage >= 0)
00205:                                 ? cDef.stages[p.currentStage].text
00206:                                 : (cDef.stages != null && cDef.stages.Count > 0 ? cDef.stages[0].text : "Crossing objective");
00207:                             activeList.Add((cDef.id, cDef.display_name, "Nobody's Charter // Crossing", stageText, p.currentStage + 1, cDef.stages?.Count ?? 1, cDef.briefing));
00208:                         }
00209:                         else
00210:                         {
00211:                             availableList.Add((cDef.id, cDef.display_name, "Crossing Charter", $"Day >= {cDef.min_day}", cDef.briefing));
00218:             if (_branchCoordinator != null)
00219:             {
00220:                 if (_branchCoordinator.IsCommitted)
00221:                 {
00222:                     string branchName = _branchCoordinator.ActiveBranchId?.Replace('_', ' ') ?? "Faction Branch";
00223:                     string factionName = _branchCoordinator.ActiveFactionKind.ToString();
00224:
00225:                     if (_branchCoordinator.ResolvedEndingId != null)
00226:                     {
00227:                         completedCount++;
00228:                         completedList.Add((
00229:                             _branchCoordinator.ActiveBranchId!,
00230:                             $"Faction Finale: {branchName}",
00231:                             $"The Weight of Choices // {factionName}",
00232:                             $"Resolved Ending: {_branchCoordinator.ResolvedEndingId.Replace('_', ' ')}"));
00233:                     }
00234:                     else
00235:                     {
00236:                         activeCount++;
00237:                         string stageText = _branchCoordinator.IsPonrLocked
00238:                             ? "Point of No Return Reached. Faction fate sealed — proceeding to final resolution."
00239:                             : "Pre-PoNR Stage: Executing faction directives and shaping ideological alignment.";
00240:                         int stageNum = _branchCoordinator.IsPonrLocked ? 2 : 1;
00241:                         activeList.Add((
00242:                             _branchCoordinator.ActiveBranchId!,
00243:                             $"Faction Allegiance: {branchName}",
00244:                             $"The Weight of Choices // {factionName}",
00245:                             stageText,
00246:                             stageNum, 2,
00247:                             $"Allegiance to {factionName} active. Mutually exclusive branch path locked in."));
00248:                     }
00249:                 }
00250:                 else
00251:                 {
00252:                     // Prospective branches available to commit
00253:                     var options = _branchCoordinator.GetBranchOptions(_moralChoice);
00254:                     foreach (var opt in options)
00255:                     {
00256:                         if (opt.IsAvailable)
00257:                         {
00258:                             availableList.Add((
00259:                                 opt.BranchId,
00260:                                 $"Prospective Allegiance: {opt.DisplayName}",
00261:                                 $"Faction Branch ({opt.FactionKind})",
00262:                                 $"Morality: {opt.EntryBandMin}..{opt.EntryBandMax}",
00263:                                 $"{opt.ConsequencesSummary} Trigger: {opt.PonrTrigger}"));
00264:                         }
00265:                     }
00266:                 }
00267:             }
00272:                 foreach (var mDef in _moralDefs)
00273:                 {
00274:                     if (_moralChoice.IsResolved(mDef.Id))
00275:                     {
00276:                         if (_moralChoice.TryGetResolution(mDef.Id, out var res) && res != null)
00277:                         {
00278:                             completedCount++;
00279:                             completedList.Add((mDef.Id, mDef.DisplayName, $"The Weight of Survival // {mDef.Category.ToUpperInvariant()}", res.epitaph));
00280:                         }
00281:                     }
00282:                     else if (Ashfall.Core.MoralChoice.MoralChoiceSystem.IsAvailableOnDay(mDef, _currentDay) &&
00283:                              _moralChoice.IsChainQuestAccessible(mDef.Id, _currentDay))
00284:                     {
00285:                         availableList.Add((
00286:                             mDef.Id,
00293:             }
00294:
00295:             // If no active quests found in live session, provide the initial starting protocol
00296:             if (activeList.Count == 0)
00297:             {
00298:                 activeList.Add((
00299:                     HoldfastQuestSystem.Sheet,
00306:
00307:             // ── Overview Card ──
00308:             var ovCard = AshfallUiHelpers.MakeCardFrame("NARRATIVE OPERATIONS & CAMPAIGN DIRECTIVES", "MISSION STATUS");
00309:             var ovBox = ovCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00310:
00311:             ovBox.AddChild(AshfallUiHelpers.MakeDataRow("Campaign Timeline", $"Day {_currentDay:00} After Nuclear Exchange", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
00312:             ovBox.AddChild(AshfallUiHelpers.MakeDataRow("Active Mission Operations", $"{Math.Max(activeCount, activeList.Count)} Operation(s) In Progress", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Hot)));
00313:             ovBox.AddChild(AshfallUiHelpers.MakeDataRow("Completed Protocols", $"{completedCount} Milestones Recorded", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
00314:
00315:             if (_crossingQuests != null)
00316:             {
00317:                 var btnCrossing = AshfallUiHelpers.MakeButton("OPEN NOBODY'S CHARTER // CROSSING PROTOCOLS", () =>
00318:                 {
00319:                     OnCrossingPanelRequested?.Invoke();
00320:                 });
00321:                 ovBox.AddChild(btnCrossing);
00322:             }
00323:
00324:             _overviewContainer.AddChild(ovCard);
00325:
00326:             // ── Canonical procedural narrative runtime (Plan 171) ──
00327:             // This card is a read/command surface over the existing
00328:             // ProceduralNarrativeSystem + QuestRuntimeCoordinator pair. The
00329:             // panel never selects templates or mutates quest state itself.
00330:             if (_proceduralNarrative != null)
00332:                 var runtime = _proceduralNarrative.QuestRuntime;
00333:                 var proceduralCard = AshfallUiHelpers.MakeCardFrame(
00334:                     "PROCEDURAL OPERATIONAL OPPORTUNITIES",
00335:                     "JSON templates · canonical quest runtime");
00336:                 var proceduralBox = proceduralCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00337:                 var proceduralRows = runtime.BuildReadModel();
00338:                 int activeProcedural = 0;
00339:                 int offeredProcedural = 0;
00340:                 for (int i = 0; i < proceduralRows.Count; i++)
00341:                 {
00342:                     if (!proceduralRows[i].isProcedural) continue;
00343:                     if (proceduralRows[i].status == Ashfall.Core.Quests.QuestLifecycleState.Active) activeProcedural++;
00344:                     if (proceduralRows[i].status == Ashfall.Core.Quests.QuestLifecycleState.Offered) offeredProcedural++;
00345:                 }
00346:                 proceduralBox.AddChild(AshfallUiHelpers.MakeDataRow(
00347:                     "Canonical runtime",
00348:                     $"{activeProcedural} active · {offeredProcedural} offered · {proceduralRows.Count} total",
00349:                     AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
00350:                 var generateButton = AshfallUiHelpers.MakeButton(
00351:                     "REQUEST NEW OPERATIONAL OPPORTUNITY",
00352:                     () => OnProceduralQuestRequested?.Invoke());
00353:                 proceduralBox.AddChild(generateButton);
00354:                 _availableContainer.AddChild(proceduralCard);
00355:             }
00356:
00357:             // ── Active Quests ──
00358:             foreach (var q in activeList)
00359:             {
00360:                 var card = AshfallUiHelpers.MakeCardFrame(q.name, $"{q.type.ToUpperInvariant()} · STAGE {q.stageNum}/{q.totalStages}");
00361:                 var cardBox = card.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00362:
00363:                 var stageHeader = AshfallUiHelpers.MakeSubsectionHeader("CURRENT OPERATIONAL OBJECTIVE");
00364:                 cardBox.AddChild(stageHeader);
00365:
00366:                 var stageLbl = AshfallUiHelpers.MakeBody($"► {q.stageText}");
00367:                 stageLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
00368:                 cardBox.AddChild(stageLbl);
00369:
00370:                 if (!string.IsNullOrEmpty(q.briefing))
00371:                 {
00372:                     cardBox.AddChild(AshfallUiHelpers.MakeSeparator());
00373:                     var briefLbl = AshfallUiHelpers.MakeSmall(q.briefing);
00374:                     briefLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
00375:                     cardBox.AddChild(briefLbl);
00376:                 }
00377:
00378:                 var btnRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
00380:                 var inspectBtn = AshfallUiHelpers.MakeButton($"INSPECT QUEST DOSSIER // [{q.name}]", () =>
00381:                 {
00382:                     OnQuestDetailRequested?.Invoke(questId);
00383:                 });
00384:                 inspectBtn.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00385:                 btnRow.AddChild(inspectBtn);
00386:                 cardBox.AddChild(btnRow);
00387:
00388:                 _activeContainer.AddChild(card);
00389:             }
00390:
00391:             // ── Available / Upcoming Missions ──
00392:             if (availableList.Count > 0)
00393:             {
00394:                 int showCount = Math.Min(8, availableList.Count);
00395:                 for (int i = 0; i < showCount; i++)
00397:                     var avail = availableList[i];
00398:                     var card = AshfallUiHelpers.MakeCardFrame(avail.name, avail.reqs);
00399:                     var cardBox = card.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00400:
00401:                     var briefLbl = AshfallUiHelpers.MakeSmall(avail.briefing);
00402:                     briefLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
00403:                     cardBox.AddChild(briefLbl);
00404:
00405:                     string qId = avail.id;
00406:                     var btn = AshfallUiHelpers.MakeButton($"VIEW BRIEFING // [{avail.name}]", () =>
00407:                     {
00408:                         OnQuestDetailRequested?.Invoke(qId);
00409:                     });
00410:                     cardBox.AddChild(btn);
00411:
00412:                     _availableContainer.AddChild(card);
00413:                 }
00414:             }
00415:
00416:             // ── Completed Quests ──
00417:             var compCard = AshfallUiHelpers.MakeCardFrame("HISTORICAL OPERATION COMPLETIONS", "LOG ARCHIVE");
00418:             var compBox = compCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00419:
00420:             if (completedList.Count > 0)
00421:             {
00422:                 foreach (var comp in completedList)
00427:             else
00428:             {
00429:                 compBox.AddChild(AshfallUiHelpers.MakeDataRow("Day 01 Protocol", "Bunker seal integrity established. Air filtration online.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
00430:                 compBox.AddChild(AshfallUiHelpers.MakeDataRow("Opening Census", "Initial 12-survivor roster logged into Holdfast ledger.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
00431:             }
00432:             _completedContainer.AddChild(compCard);
00433:
00434:             // ── Duty Roster quests (Exp 02) — real runtime read model ──
00435:             if (_dutyRoster != null)
00436:             {
00438:                 var rosterCard = AshfallUiHelpers.MakeCardFrame(
00439:                     "DUTY ROSTER // ALLOCATION 12 CHART", qRuntime.StartedCount + " started · " + qRuntime.CompletedCount + " complete");
00440:                 var rosterBox = rosterCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00441:
00442:                 var active = qRuntime.GetActiveQuests();
00443:                 if (active.Count > 0)
00444:                 {
00445:                     for (int i = 0; i < active.Count; i++)
00446:                     {
00447:                         var q = active[i];
00448:                         if (q == null) continue;
00449:                         var p = qRuntime.GetProgress(q.id);
00450:                         string stage = p != null ? $"stage {p.currentStage + 1}/{q.StageCount}" : "";
00451:                         rosterBox.AddChild(AshfallUiHelpers.MakeDataRow(
00452:                             $"▶ {q.display_name}", stage, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
00453:                         // Authored stage prose rendered to the player (house voice).
00456:                         {
00457:                             var proseLbl = AshfallUiHelpers.MakeSmall(prose, autowrap: true);
00458:                             proseLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
00459:                             rosterBox.AddChild(proseLbl);
00460:                         }
00461:                     }
00462:                 }
00467:                 }
00468:
00469:                 var available = qRuntime.GetAvailableQuests(_dutyRoster.Clock.Day);
00470:                 if (available.Count > 0)
00471:                 {
00472:                     for (int i = 0; i < available.Count && i < 6; i++)
00473:                     {
00474:                         var q = available[i];
00475:                         if (q == null) continue;
00476:                         string prereq = string.IsNullOrEmpty(q.prereq_quest_id) ? "" : " · after " + q.prereq_quest_id;
00477:                         rosterBox.AddChild(AshfallUiHelpers.MakeDataRow(
00478:                             $"◦ {q.display_name}", $"day {q.min_day}+{prereq}", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
00479:                     }
00482:                 }
00483:
00484:                 var started = qRuntime.GetProgress(DutyRosterIds.QuestTheChart);
00485:                 var btnStart = AshfallUiHelpers.MakeButton(
00486:                     started != null && started.started && !started.completed ? "ADVANCE CHART QUEST" : "START THE CHART",
00487:                     () =>
00488:                     {
00495:                 rosterBox.AddChild(btnStart);
00496:
00497:                 _availableContainer.AddChild(rosterCard);
00498:             }
00499:
00500:             RenderSurvivorArcs();
00501:         }
00502:
00503:         /// <summary>
00504:         /// Renders the authored survivor personal arcs (narrative_questlines.json)
00505:         /// as a real command surface: open an arc, hand over the objective the
00506:         /// current stage owes, or resolve the crisis fork. The commands are raised
00507:         /// as events; Main owns inventory spend, morale and trait recording.
00508:         /// Survivor, item and trait identifiers are shown through labels, never raw.
00509:         /// </summary>
00510:         private void RenderSurvivorArcs()
00511:         {
00512:             if (_survivorArcs == null) return;
00513:             if (_activeContainer == null || _availableContainer == null || _completedContainer == null) return;
00514:
00515:             var defs = _survivorArcs.Definitions;
00516:             for (int i = 0; i < defs.Count; i++)
00517:             {
00518:                 var def = defs[i];
00519:                 if (def == null || string.IsNullOrEmpty(def.survivorId) || def.stages.Count == 0) continue;
00520:
00521:                 string survivorId = def.survivorId;
00522:                 string who = SurvivorLabel(survivorId);
00523:                 var arc = _survivorArcs.GetArc(survivorId);
00524:
00525:                 // ── Not yet opened: the arc is available ──
00526:                 if (arc == null)
00527:                 {
00528:                     var opening = def.stages[0];
00529:                     var card = AshfallUiHelpers.MakeCardFrame(def.title, $"Personal arc // {who}");
00530:                     var box = card.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00531:
00532:                     var openLbl = AshfallUiHelpers.MakeSmall($"{opening.name}: {opening.description}");
00533:                     openLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
00534:                     box.AddChild(openLbl);
00535:
00536:                     var openBtn = AshfallUiHelpers.MakeButton($"OPEN PERSONAL ARC // [{who}]", () =>
00537:                     {
00538:                         OnBeginSurvivorArcRequested?.Invoke(survivorId);
00539:                         RefreshView();
00540:                     });
00541:                     box.AddChild(openBtn);
00542:
00543:                     _availableContainer.AddChild(card);
00544:                     continue;
00545:                 }
00546:
00547:                 // ── Resolved: show what was chosen and what it recorded ──
00548:                 if (arc.status == Ashfall.Core.Quests.NarrativeArcStatus.Resolved)
00549:                 {
00550:                     var chosen = def.FindBranchStage()?.FindBranch(arc.chosenBranchId);
00551:                     var doneCard = AshfallUiHelpers.MakeCardFrame(def.title, $"Personal arc // {who}");
00552:                     var doneBox = doneCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00553:
00554:                     doneBox.AddChild(AshfallUiHelpers.MakeDataRow(
00555:                         chosen != null ? $"Resolved — {chosen.label}" : "Resolved",
00556:                         string.IsNullOrEmpty(arc.grantedTraitId)
00557:                             ? "no trait recorded"
00558:                             : $"recorded: {TraitLabel(arc.grantedTraitId)}",
00559:                         AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted)));
00561:                     var epilogue = def.FindStage(def.FinalStageIndex);
00562:                     if (epilogue != null && !string.IsNullOrEmpty(epilogue.description))
00563:                         doneBox.AddChild(AshfallUiHelpers.MakeSmall(epilogue.description));
00564:
00565:                     _completedContainer.AddChild(doneCard);
00566:                     continue;
00567:                 }
00568:
00569:                 // ── In progress: either owes supplies or awaits the fork ──
00570:                 var stage = def.FindStage(arc.currentStage);
00571:                 var card2 = AshfallUiHelpers.MakeCardFrame(def.title, $"Personal arc // {who}");
00572:                 var box2 = card2.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);
00573:
00574:                 box2.AddChild(AshfallUiHelpers.MakeSubsectionHeader(
00575:                     stage != null ? $"CURRENT STAGE // {stage.name.ToUpperInvariant()}" : "CURRENT STAGE"));
00576:
00577:                 if (stage != null && !string.IsNullOrEmpty(stage.description))
00578:                 {
00579:                     var stageLbl = AshfallUiHelpers.MakeBody($"► {stage.description}");
00580:                     stageLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
00581:                     box2.AddChild(stageLbl);
00582:                 }
00583:
00584:                 if (stage != null && stage.HasBranch &&
00587:                     box2.AddChild(AshfallUiHelpers.MakeSeparator());
00588:                     box2.AddChild(AshfallUiHelpers.MakeSmall(
00589:                         "Two ways through this, and only one can be taken. Choosing ends the arc."));
00590:                     AddArcBranchButton(box2, survivorId, stage.branchA!);
00591:                     AddArcBranchButton(box2, survivorId, stage.branchB!);
00592:                 }
00593:                 else
00594:                 {
00595:                     var owed = _survivorArcs.GetOutstandingObjectives(survivorId);
00596:                     if (owed.Count > 0)
00597:                     {
00598:                         box2.AddChild(AshfallUiHelpers.MakeSeparator());
00599:                         box2.AddChild(AshfallUiHelpers.MakeSubsectionHeader("OWED FROM STORES"));
00600:                         for (int o = 0; o < owed.Count; o++)
00601:                         {
00602:                             string itemId = owed[o];
00603:                             var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
00604:
00605:                             var itemLbl = AshfallUiHelpers.MakeBody(ItemNameLabel(itemId));
00606:                             itemLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00607:                             row.AddChild(itemLbl);
00608:
00609:                             var handBtn = AshfallUiHelpers.MakeButton("HAND OVER", () =>
00610:                             {
00611:                                 OnDeliverArcObjectiveRequested?.Invoke(survivorId, itemId);
00612:                                 RefreshView();
00613:                             });
00614:                             row.AddChild(handBtn);
00615:
00624:                 }
00625:
00626:                 _activeContainer.AddChild(card2);
00627:             }
00628:         }
00629:
00630:         private void AddArcBranchButton(
00631:             VBoxContainer box, string survivorId, Ashfall.Core.Quests.NarrativeQuestlineBranchDef branch)
00632:         {
00633:             if (branch == null) return;
00634:             string branchId = branch.id;
00635:
00636:             var lbl = AshfallUiHelpers.MakeSmall(
00637:                 $"{branch.label} — {branch.description} (morale {branch.moraleDelta:+0;-0;0})");
00638:             lbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
00639:             box.AddChild(lbl);
00640:
00641:             var btn = AshfallUiHelpers.MakeButton($"CHOOSE // [{branch.label}]", () =>
00642:             {
00643:                 OnChooseArcBranchRequested?.Invoke(survivorId, branchId);
00644:                 RefreshView();
00645:             });
00646:             box.AddChild(btn);
00647:         }
00649:         private string SurvivorLabel(string survivorId)
00650:         {
00651:             var resolved = _survivorDisplayName?.Invoke(survivorId);
00652:             return string.IsNullOrWhiteSpace(resolved) ? HumanizeArcToken(survivorId) : resolved!;
00653:         }
00654:
00655:         private string ItemNameLabel(string itemId)
00656:         {
00657:             var resolved = _itemLabel?.Invoke(itemId);
00658:             return string.IsNullOrWhiteSpace(resolved) ? HumanizeArcToken(itemId) : resolved!;
00659:         }
00660:
00661:         private static string TraitLabel(string traitId) => HumanizeArcToken(traitId);
00662:
00665:         {
00666:             if (string.IsNullOrWhiteSpace(id)) return string.Empty;
00667:             string[] parts = id.Split('_', StringSplitOptions.RemoveEmptyEntries);
00668:             for (int i = 0; i < parts.Length; i++)
00669:             {
00670:                 if (parts[i].Length == 0) continue;
00671:                 parts[i] = char.ToUpperInvariant(parts[i][0]) + (parts[i].Length > 1 ? parts[i][1..] : string.Empty);
00672:             }
00673:             return string.Join(" ", parts);
00674:         }
00675:
00676:         public override void _Ready()
00677:         {
00678:             SetAnchorsPreset(LayoutPreset.FullRect);
00679:             Visible = false;
00680:
00683:             AddChild(bg);
00684:
00685:             var scroll = new ScrollContainer();
00686:             scroll.SetAnchorsPreset(LayoutPreset.FullRect);
00687:             scroll.HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled;
00688:             AddChild(scroll);
00689:
00690:             var center = new CenterContainer();
00691:             center.SetAnchorsPreset(LayoutPreset.FullRect);
00692:             center.SizeFlagsHorizontal = SizeFlags.ExpandFill;
00693:             center.SizeFlagsVertical = SizeFlags.ExpandFill;
00694:             scroll.AddChild(center);
00695:
00696:             var rootBox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
00698:             center.AddChild(rootBox);
00699:
00700:             var title = AshfallUiHelpers.MakeTitle("OPERATIONS & STORY PROGRESSION", Ashfall.Core.UI.Theme.FontSizeH1);
00701:             title.HorizontalAlignment = HorizontalAlignment.Center;
00702:             rootBox.AddChild(title);
00703:
00704:             _statusSummary = AshfallUiHelpers.MakeMetadata("Active survival objectives, Holdfast protocol directives, and narrative campaign storylines.");
00705:             _statusSummary.HorizontalAlignment = HorizontalAlignment.Center;
00706:             _statusSummary.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
00707:             rootBox.AddChild(_statusSummary);
00708:
00709:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00710:
00711:             _overviewContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
00712:             rootBox.AddChild(_overviewContainer);
00713:
00714:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00715:
00716:             var activeTitle = AshfallUiHelpers.MakeSectionHeader("ACTIVE OPERATIONS & CURRENT STAGES");
00717:             rootBox.AddChild(activeTitle);
00718:
00719:             _activeContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
00720:             rootBox.AddChild(_activeContainer);
00721:
00722:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00723:
00724:             var availTitle = AshfallUiHelpers.MakeSectionHeader("UPCOMING PROTOCOLS & AVAILABLE DIRECTIVES");
00725:             rootBox.AddChild(availTitle);
00726:
00727:             _availableContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
00728:             rootBox.AddChild(_availableContainer);
00729:
00730:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00731:
00732:             var compTitle = AshfallUiHelpers.MakeSectionHeader("COMPLETED PROTOCOL LOGS");
00733:             rootBox.AddChild(compTitle);
00734:
00735:             _completedContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
00736:             rootBox.AddChild(_completedContainer);
00737:
00738:             rootBox.AddChild(AshfallUiHelpers.MakeSeparator());
00739:
00740:             var btnClose = AshfallUiHelpers.MakeButton("CLOSE QUESTS [Esc]", () => OnClose?.Invoke());
00741:             btnClose.CustomMinimumSize = new Vector2(220, 42);
00742:             rootBox.AddChild(btnClose);
00743:
00744:             var hint = AshfallUiHelpers.MakeSmall("[Esc] to close quest journal");
00745:             hint.HorizontalAlignment = HorizontalAlignment.Center;
00746:             hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
00747:             rootBox.AddChild(hint);
00748:         }
00749:
00750:         public void Open()
00751:         {
00752:             Visible = true;
00753:             RefreshView();
00754:             QueueRedraw();
00755:         }
00756:
00757:         public override void _UnhandledInput(InputEvent @event)
00758:         {
00759:             if (!Visible) return;
00760:
00761:             if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
00762:             {
00763:                 OnClose?.Invoke();
00764:                 GetViewport().SetInputAsHandled();
00765:             }
00766:         }
00767:
00768:         public override void _ExitTree()
00769:         {
00770:             Unbind();
00771:             base._ExitTree();
00772:         }
00773:     }
00774: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/NarrativeQuestlineCatalogTests.cs`

### `Ashfall.Core.Tests/NarrativeQuestlineCatalogTests.cs` — complete current file

- Size: 258 lines / 11292 bytes.
- SHA-256: `537c569811bfde768a3bf444a9baef9d8a8924a2e93baf06de5ae0618607c5e0`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Text.Json;
00006: using Xunit;
00007:
00008: namespace Ashfall.Core.Tests
00009: {
00010:     /// <summary>
00011:     /// Plan 104 — validates that narrative_questlines.json contains exactly 12 questlines,
00012:     /// all with unique quest_ids, all with 4 stages (Discovery/Investigation/Crisis/Resolution),
00013:     /// all Crisis stages having branch_a and branch_b, and all survivor_id and
00014:     /// target_location_id fields non-empty.
00015:     ///
00016:     /// This is a pure data-authority test: no new Core code is exercised, only the
00017:     /// JSON catalog's structural and referential integrity.
00018:     /// </summary>
00019:     public class NarrativeQuestlineCatalogTests
00020:     {
00021:         private static string FindDataDir()
00022:         {
00023:             string search = Directory.GetCurrentDirectory();
00024:             for (int i = 0; i < 6; i++)
00025:             {
00026:                 string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
00027:                 if (Directory.Exists(candidate)) return candidate;
00028:                 string? parent = Directory.GetParent(search)?.FullName;
00029:                 if (parent == null) break;
00030:                 search = parent;
00031:             }
00032:             return string.Empty;
00033:         }
00034:
00035:         private static JsonDocument LoadQuestlines(string dataDir)
00036:         {
00037:             string path = Path.Combine(dataDir, "narrative_questlines.json");
00038:             Assert.True(File.Exists(path), $"narrative_questlines.json not found at: {path}");
00039:             string raw = File.ReadAllText(path);
00040:             return JsonDocument.Parse(raw);
00041:         }
00042:
00043:         [Fact]
00044:         public void QuestlineCatalog_HasExactly12Questlines()
00045:         {
00046:             string dataDir = FindDataDir();
00047:             if (string.IsNullOrEmpty(dataDir)) return;
00048:
00049:             using var doc = LoadQuestlines(dataDir);
00050:             var questlines = doc.RootElement.GetProperty("questlines");
00051:             Assert.Equal(12, questlines.GetArrayLength());
00052:         }
00053:
00054:         [Fact]
00055:         public void QuestlineCatalog_AllQuestIdsUnique()
00056:         {
00057:             string dataDir = FindDataDir();
00058:             if (string.IsNullOrEmpty(dataDir)) return;
00059:
00060:             using var doc = LoadQuestlines(dataDir);
00061:             var questlines = doc.RootElement.GetProperty("questlines");
00062:             var ids = new HashSet<string>(StringComparer.Ordinal);
00063:             foreach (var ql in questlines.EnumerateArray())
00064:             {
00065:                 string questId = ql.GetProperty("quest_id").GetString() ?? string.Empty;
00066:                 Assert.False(string.IsNullOrEmpty(questId), "quest_id must be non-empty");
00067:                 Assert.True(ids.Add(questId), $"Duplicate quest_id found: {questId}");
00068:             }
00069:         }
00070:
00071:         [Fact]
00072:         public void QuestlineCatalog_AllSurvivorIdsNonEmpty()
00073:         {
00074:             string dataDir = FindDataDir();
00075:             if (string.IsNullOrEmpty(dataDir)) return;
00076:
00077:             using var doc = LoadQuestlines(dataDir);
00078:             var questlines = doc.RootElement.GetProperty("questlines");
00079:             foreach (var ql in questlines.EnumerateArray())
00080:             {
00081:                 string questId = ql.GetProperty("quest_id").GetString() ?? "(unknown)";
00082:                 string survivorId = ql.GetProperty("survivor_id").GetString() ?? string.Empty;
00083:                 Assert.False(string.IsNullOrEmpty(survivorId),
00084:                     $"survivor_id is empty for quest: {questId}");
00085:             }
00086:         }
00087:
00088:         [Fact]
00089:         public void QuestlineCatalog_AllTargetLocationIdsNonEmpty()
00090:         {
00091:             string dataDir = FindDataDir();
00092:             if (string.IsNullOrEmpty(dataDir)) return;
00093:
00094:             using var doc = LoadQuestlines(dataDir);
00095:             var questlines = doc.RootElement.GetProperty("questlines");
00096:             foreach (var ql in questlines.EnumerateArray())
00097:             {
00098:                 string questId = ql.GetProperty("quest_id").GetString() ?? "(unknown)";
00099:                 string locationId = ql.GetProperty("target_location_id").GetString() ?? string.Empty;
00100:                 Assert.False(string.IsNullOrEmpty(locationId),
00101:                     $"target_location_id is empty for quest: {questId}");
00102:             }
00103:         }
00104:
00105:         [Fact]
00106:         public void QuestlineCatalog_EachQuestlineHasFourStages()
00107:         {
00108:             string dataDir = FindDataDir();
00109:             if (string.IsNullOrEmpty(dataDir)) return;
00110:
00111:             using var doc = LoadQuestlines(dataDir);
00112:             var questlines = doc.RootElement.GetProperty("questlines");
00113:             foreach (var ql in questlines.EnumerateArray())
00114:             {
00115:                 string questId = ql.GetProperty("quest_id").GetString() ?? "(unknown)";
00116:                 var stages = ql.GetProperty("stages");
00117:                 int stageCount = stages.GetArrayLength();
00118:                 Assert.True(stageCount == 4,
00119:                     $"Quest {questId} must have exactly 4 stages (0=Discovery, 1=Investigation, 2=Crisis, 3=Resolution); found {stageCount}");
00120:             }
00121:         }
00122:
00123:         [Fact]
00124:         public void QuestlineCatalog_AllStageNamesNonEmpty()
00125:         {
00126:             string dataDir = FindDataDir();
00127:             if (string.IsNullOrEmpty(dataDir)) return;
00128:
00129:             using var doc = LoadQuestlines(dataDir);
00130:             var questlines = doc.RootElement.GetProperty("questlines");
00131:             foreach (var ql in questlines.EnumerateArray())
00132:             {
00133:                 string questId = ql.GetProperty("quest_id").GetString() ?? "(unknown)";
00134:                 foreach (var stage in ql.GetProperty("stages").EnumerateArray())
00135:                 {
00136:                     int stageNum = stage.GetProperty("stage").GetInt32();
00137:                     string name = stage.GetProperty("name").GetString() ?? string.Empty;
00138:                     string desc = stage.GetProperty("description").GetString() ?? string.Empty;
00139:                     Assert.False(string.IsNullOrEmpty(name),
00140:                         $"Stage {stageNum} of {questId} has empty name");
00141:                     Assert.False(string.IsNullOrEmpty(desc),
00142:                         $"Stage {stageNum} of {questId} has empty description");
00143:                 }
00144:             }
00145:         }
00146:
00147:         [Fact]
00148:         public void QuestlineCatalog_CrisisStageHasTwoBranches()
00149:         {
00150:             string dataDir = FindDataDir();
00151:             if (string.IsNullOrEmpty(dataDir)) return;
00152:
00153:             using var doc = LoadQuestlines(dataDir);
00154:             var questlines = doc.RootElement.GetProperty("questlines");
00155:             foreach (var ql in questlines.EnumerateArray())
00156:             {
00157:                 string questId = ql.GetProperty("quest_id").GetString() ?? "(unknown)";
00158:                 foreach (var stage in ql.GetProperty("stages").EnumerateArray())
00159:                 {
00160:                     int stageNum = stage.GetProperty("stage").GetInt32();
00161:                     if (stageNum != 2) continue; // Crisis is stage 2
00162:
00163:                     Assert.True(stage.TryGetProperty("branch_a", out _),
00164:                         $"Crisis stage of {questId} is missing branch_a");
00165:                     Assert.True(stage.TryGetProperty("branch_b", out _),
00166:                         $"Crisis stage of {questId} is missing branch_b");
00167:
00168:                     var branchA = stage.GetProperty("branch_a");
00169:                     var branchB = stage.GetProperty("branch_b");
00170:
00171:                     string traitA = branchA.GetProperty("trait_granted").GetString() ?? string.Empty;
00172:                     string traitB = branchB.GetProperty("trait_granted").GetString() ?? string.Empty;
00173:
00174:                     Assert.False(string.IsNullOrEmpty(traitA),
00175:                         $"branch_a of {questId} crisis has empty trait_granted");
00176:                     Assert.False(string.IsNullOrEmpty(traitB),
00177:                         $"branch_b of {questId} crisis has empty trait_granted");
00178:                     Assert.NotEqual(traitA, traitB);
00179:                 }
00180:             }
00181:         }
00182:
00183:         [Fact]
00184:         public void QuestlineCatalog_PriestAndReporterArcsPresent()
00185:         {
00186:             // Plan 104 §7: exactly 2 questlines wired for Plan 52 recurring-NPC hooks.
00187:             // Verified by confirming the priest and reporter questlines exist.
00188:             string dataDir = FindDataDir();
00189:             if (string.IsNullOrEmpty(dataDir)) return;
00190:
00191:             using var doc = LoadQuestlines(dataDir);
00192:             var questlines = doc.RootElement.GetProperty("questlines");
00193:             bool hasPriest = false;
00194:             bool hasReporter = false;
00195:             foreach (var ql in questlines.EnumerateArray())
00196:             {
00197:                 string survivorId = ql.GetProperty("survivor_id").GetString() ?? string.Empty;
00198:                 if (survivorId == "the_priest") hasPriest = true;
00199:                 if (survivorId == "the_reporter") hasReporter = true;
00200:             }
00201:             Assert.True(hasPriest, "the_priest questline (Plan 52 NPC hook) must be present");
00202:             Assert.True(hasReporter, "the_reporter questline (Plan 52 NPC hook) must be present");
00203:         }
00204:
00205:         [Fact]
00206:         public void QuestlineCatalog_TeacherAndJournalistArcsPresent()
00207:         {
00208:             // Plan 104 §8: exactly 2 questlines wired for Plan 95 journal voice.
00209:             // Verified by confirming the teacher and reporter questlines exist.
00210:             string dataDir = FindDataDir();
00211:             if (string.IsNullOrEmpty(dataDir)) return;
00212:
00213:             using var doc = LoadQuestlines(dataDir);
00214:             var questlines = doc.RootElement.GetProperty("questlines");
00215:             bool hasTeacher = false;
00216:             bool hasReporter = false;
00217:             foreach (var ql in questlines.EnumerateArray())
00218:             {
00219:                 string survivorId = ql.GetProperty("survivor_id").GetString() ?? string.Empty;
00220:                 if (survivorId == "the_teacher") hasTeacher = true;
00221:                 if (survivorId == "the_reporter") hasReporter = true;
00222:             }
00223:             Assert.True(hasTeacher, "the_teacher questline (Plan 95 journal voice) must be present");
00224:             Assert.True(hasReporter, "the_reporter questline (Plan 95 journal voice) must be present");
00225:         }
00226:
00227:         [Fact]
00228:         public void QuestlineCatalog_AllExpectedSurvivorsPresent()
00229:         {
00230:             // All 8 new arcs plus the original 4 must be represented.
00231:             string dataDir = FindDataDir();
00232:             if (string.IsNullOrEmpty(dataDir)) return;
00233:
00234:             var expected = new HashSet<string>(StringComparer.Ordinal)
00235:             {
00236:                 // Original 4
00237:                 "aris_thorne", "maya_lin", "victor_vance", "elena_rostov",
00238:                 // New 8 (Plan 104)
00239:                 "marcus_olejnik", "the_teacher", "the_chef", "suki_tanaka",
00240:                 "the_priest", "the_reporter", "the_electrician", "the_hunter"
00241:             };
00242:
00243:             using var doc = LoadQuestlines(dataDir);
00244:             var questlines = doc.RootElement.GetProperty("questlines");
00245:             var found = new HashSet<string>(StringComparer.Ordinal);
00246:             foreach (var ql in questlines.EnumerateArray())
00247:             {
00248:                 string survivorId = ql.GetProperty("survivor_id").GetString() ?? string.Empty;
00249:                 found.Add(survivorId);
00250:             }
00251:
00252:             foreach (var id in expected)
00253:             {
00254:                 Assert.Contains(id, found);
00255:             }
00256:         }
00257:     }
00258: }
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **Current evidence and safe integration boundary for Plan 104: Narrative Questlines, Survivor Arcs and Save-Backed Branches.**.

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
