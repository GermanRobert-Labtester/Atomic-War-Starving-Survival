# Plan 26 — Latent Expertise Awakening and Adult Re-Specialization

> **Rebuild status:** PARTIAL — AWAKENING INTEGRATED; ADULT RE-SPECIALIZATION REMAINS
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

- The current awakening system maps twelve latent traits to existing skills, records progress, persists awakening facts and grants the existing skill through `TryGrantSkill`. That core outcome is already implemented.
- The missing portion is adult re-specialization: regain, redirect or relinquish expertise after injury, role loss, bereavement or sustained work, with human trade-offs and no random bonus.
- The plan should also externalize or validate awakening definitions if the current hardcoded table becomes a content-growth bottleneck, while preserving strict references to existing trait and skill IDs.

**Bounded outcome:** Keep `LatentExpertAwakeningSystem` and `SkillProgressionSystem` as the only expertise/skill owners. Extend awakening definitions and context producers; implement re-specialization as bounded progression and duty consequences, not a second education or skill catalog.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `LatentExpertAwakeningSystem` contains twelve default trait-to-skill mappings and deterministic progress records.
- Capture/restore persists survivor, trait, skill, day, context and progress.
- Skill grants route to `SkillProgressionSystem` when both actor and system are supplied.
- Research has 62 knowledge nodes and skills has 161 rows; neither should be expanded by this plan.
- Apprenticeship and survivor education remain separate child/development owners.

**Master-authority sections applied to this rebase:**

- Volume 7 record contracts
- Volume 17 C9/C16 roadmaps
- Volume 21 FP-B06/B14

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Add a strict awakening definition catalog only if current growth requires it; otherwise maintain code-owned definitions with integrity checks.
- Define adult competence states as projections over existing skills, duties, injuries and relationships.
- Add explicit re-specialization commands with previews and bounded support/recovery choices.
- Expose known, suspected and awakened expertise truthfully in roster/duty/expedition views.

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
| latent progress and awakening facts | LatentExpertAwakeningSystem | `Assets/Ashfall.Core/Survivors/LatentExpertAwakeningSystem.cs` | Sole awakening owner. |
| skill levels/XP and active skill grants | SkillProgressionSystem | `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` | Sole skill authority. |
| research nodes and unlocks | ResearchSystem | `Assets/Ashfall.Core/Research/ResearchSystem.cs` | Research is not awakening. |
| mentor/apprentice development | ApprenticeshipSystem | `Assets/Ashfall.Core/ApprenticeshipSystem.cs` | Child/ongoing education owner. |
| available roles and physical constraints | DutyRoster/medical | `DutyRosterSystem; medical owners` | Re-specialization consumes their facts. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Latent Expertise Awakening and Adult Re-Specialization
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ LatentExpertAwakeningSystem
│   latent progress and awakening facts
│ SkillProgressionSystem
│   skill levels/XP and active skill grants
│ ResearchSystem
│   research nodes and unlocks
│ ApprenticeshipSystem
│   mentor/apprentice development
│ DutyRoster/medical
│   available roles and physical constraints
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

1. **Preserve current state ownership.** LatentExpertAwakeningSystem owns latent progress and awakening facts: Sole awakening owner.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| latent progress and awakening facts | LatentExpertAwakeningSystem | `Assets/Ashfall.Core/Survivors/LatentExpertAwakeningSystem.cs` | Sole awakening owner. |
| skill levels/XP and active skill grants | SkillProgressionSystem | `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` | Sole skill authority. |
| research nodes and unlocks | ResearchSystem | `Assets/Ashfall.Core/Research/ResearchSystem.cs` | Research is not awakening. |
| mentor/apprentice development | ApprenticeshipSystem | `Assets/Ashfall.Core/ApprenticeshipSystem.cs` | Child/ongoing education owner. |
| available roles and physical constraints | DutyRoster/medical | `DutyRosterSystem; medical owners` | Re-specialization consumes their facts. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. read latent trait or current competence
2. record relevant work/crisis context
3. awaken existing skill when threshold met
4. evaluate re-specialization support
5. preview changes to duty eligibility and progression
6. execute through owning commands
7. journal and persist

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Awakening records persist progress and completed day.
- Active skills remain in SkillProgressionState.
- Re-specialization state should be small, owner-local and default-tolerant.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Awakening reveals an existing skill; it never creates one.
- Progress and awakening are deterministic and idempotent.
- Re-specialization cannot erase medical truth or mandatory injury restrictions.
- Known versus suspected expertise is not hidden by a false confidence percentage.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- No research/skill/manual expansion.
- Optional awakening catalog must reference existing trait and skill IDs.
- No new education catalog.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Current awakening save remains authoritative.
- Any re-specialization marker rides current owner sections or a small owner extension.
- Old saves default to unchanged active skills.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Current awakening uses deterministic counters, not RNG.
- Future context selection must use owner state or sanctioned seeded streams.
- No random expertise gain.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- OnTraitAwakened and OnStateChanged exist.
- New re-specialization events must be typed and routed to roster/journal owners.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/UI/ApprenticeshipPanel.cs
- src/UI/SurvivorsPanel.cs
- src/Main.DutyRoster.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Awakening/re-specialization prose states the lived context and tradeoff, not a magical promotion.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A trait maps to a missing skill. | LatentExpertAwakeningSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | The same awakening fires twice. | SkillProgressionSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | Re-specialization bypasses injury restrictions. | ResearchSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A second skill ledger diverges from SkillProgressionSystem. | ApprenticeshipSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | UI labels suspected expertise as confirmed. | DutyRoster/medical | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Progression/LatentExpertAwakeningSystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/SkillProgressionSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/ApprenticeshipSystemTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Research/ResearchSaveIntegrationTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — awakening audit | Verify all twelve mappings, references, host producers and persistence. | Current core is complete or gaps are named. | No production path until the owning implementation package is separately claimed. |
| 1 — definition authority decision | Choose code-owned versus strict catalog definitions based on measured growth need. | No duplicate definition source. | No production path until the owning implementation package is separately claimed. |
| 2 — adult state model | Design bounded re-specialization state over existing skills/duties. | No second progression system. | No production path until the owning implementation package is separately claimed. |
| 3 — command/preview seam | Add support, redirect and relinquish flows. | Stale previews and invalid actions fail safely. | No production path until the owning implementation package is separately claimed. |
| 4 — visibility and narrative | Expose suspected/known/active states in current views. | Truthful, accessible and non-magical. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/Ashfall.Core/Survivors/LatentExpertAwakeningSystem.cs | MODIFY through a future progression claim | Awakening owner |
| Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs | READ; additive seam only if required | Skill owner |
| Assets/Ashfall.Core/Research/ResearchSystem.cs | READ ONLY | Research owner |
| src/UI/ApprenticeshipPanel.cs | READ; MODIFY only for truthful adult view | Presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Parallel skill state. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Child education scope creep. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Hardcoded definitions becoming an unvalidated authority. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Injury restrictions being overwritten. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| False expertise certainty in UI. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new research tree.
- No new skill definitions.
- No child progression rewrite.
- No random awakening.

# 23. Rollback and Recovery

- Definition-source changes are reversible.
- Re-specialization state requires migration and focused replay tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- Awakening is explicitly complete.
- Re-specialization gap is bounded.
- All traits/skills resolve.
- No second education or skill owner is proposed.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Add a strict awakening definition catalog only if current growth requires it; otherwise maintain code-owned definitions with integrity checks.
- Define adult competence states as projections over existing skills, duties, injuries and relationships.
- Add explicit re-specialization commands with previews and bounded support/recovery choices.
- Expose known, suspected and awakened expertise truthfully in roster/duty/expedition views.

## MUST NOT DO

- No new research tree.
- No new skill definitions.
- No child progression rewrite.
- No random awakening.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Progression/LatentExpertAwakeningSystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/SkillProgressionSystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/ApprenticeshipSystemTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Research/ResearchSaveIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — awakening audit — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: latent progress and awakening facts → LatentExpertAwakeningSystem; skill levels/XP and active skill grants → SkillProgressionSystem; research nodes and unlocks → ResearchSystem; mentor/apprentice development → ApprenticeshipSystem; available roles and physical constraints → DutyRoster/medical. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 26.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 26 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by LatentExpertAwakeningSystem or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Survivors/LatentExpertAwakeningSystem.cs`

### `Assets/Ashfall.Core/Survivors/LatentExpertAwakeningSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 240 lines / 10737 bytes.
- SHA-256: `5b6c17055d4f6e01421a65002d54202d6d5418223617e7c5488d2bd3cfa81e10`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class LatentAwakeningRecord
public string survivorId = string.Empty;
public string traitId = string.Empty;
public string skillId = string.Empty;
public int awakenedDay = -1;
public string contextReason = string.Empty;
public int progressCount = 0;
public sealed class LatentAwakeningSaveState
public string systemId = LatentExpertAwakeningSystem.SystemId;
public List<LatentAwakeningRecord> records = new List<LatentAwakeningRecord>();
public sealed class LatentAwakeningDefinition
public string TraitId { get; }
public string SkillId { get; }
public string DisplayName { get; }
public string Discipline { get; }
public int RequiredCount { get; }
public string ConditionDescription { get; }
public sealed class LatentExpertAwakeningSystem
public const string SystemId = "latent_expert_awakening_system";
public event Action<string, string, string, int>? OnTraitAwakened;
public event Action? OnStateChanged;
public void RegisterDefinition(LatentAwakeningDefinition def) {
public LatentAwakeningDefinition? GetDefinition(string traitId) {
public bool IsAwakened(string survivorId, string traitId) {
public int GetProgress(string survivorId, string traitId) {
public bool RecordProgress( string survivorId, string traitId, int increment, int currentDay, string contextReason,
public bool TryAwaken( string survivorId, string traitId, int currentDay, string contextReason, SkillActor? actor = null)
public LatentAwakeningSaveState CaptureState() {
public void RestoreState(LatentAwakeningSaveState? state) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs`

### `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 643 lines / 27323 bytes.
- SHA-256: `11f6bc3123bf0dcf53d5805d6ffea6272ae61406d4951e6b4ac1e6a46f976404`.
- Architecture signals: seeded references=3; save/restore symbols=2; typed event declarations=10; textual Godot mentions=1; textual Unity/JsonUtility mentions=1; `System.Random`=1; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class SkillProgressionSystem
public const float DefaultXpPerAction = 5f;
public const int DormantAfterUnusedDays = 14;
public const float EpiphanyMoraleThreshold = 10f;
public const float EpiphanyHealthThreshold = 20f;
public const float EpiphanyChance = 0.05f;
public const float EpiphanyMoraleRestore = 100f;
public static readonly string[] Disciplines = {
public const float UnreachableXp = 999999f;
public Func<string, float>? ActionXpMultiplier { get; set; }
public Func<bool>? BunkerSkillDecayStopped { get; set; }
public Func<string, float>? MaxMoraleCap { get; set; }
public Action<string, float>? ApplyMorale { get; set; }
public event Action<SkillActor, string, float>? OnXpGained;
public event Action<SkillActor, string>? OnSkillEarned;
public event Action<SkillActor, string>? OnSkillDormant;
public event Action<SkillActor, string>? OnSkillReactivated;
public event Action<SkillActor, string?>? OnEpiphany;
public int CatalogCount => _catalog.Count;
public void RegisterSkill(SkillDef def) {
public SkillDef? GetSkill(string id) {
public void RegisterDefaultSkills() {
public bool TryGrantSkill(SkillActor actor, string skillId, int currentDay = 0) {
public void RecordAction(SkillActor actor, string disciplineId, float xpAmount, int currentDay, ISeededRng? rng = null) {
public float GetXp(string actorId, string disciplineId) {
public float GetDisciplineProgress01(string actorId, string disciplineId) {
public bool HasActiveSkill(string actorId, string skillId) => _bySurvivor.TryGetValue(actorId ?? string.Empty, out var state)
public bool HasDormantSkill(string actorId, string skillId) => _bySurvivor.TryGetValue(actorId ?? string.Empty, out var state)
public bool HasEarnedExpertSkill(string actorId) => _bySurvivor.TryGetValue(actorId ?? string.Empty, out var state) && state.expertSkillEarned;
public IReadOnlyList<string> GetActiveSkillIds(string actorId) {
public int DaysSinceLastPractice(string actorId, string disciplineId, int currentDay) {
public void TickDaily(int currentDay, IReadOnlyList<SkillActor> actors) {
public void SyncSkillBonuses(SkillActor actor, SkillProgressionState? state = null) {
public float GetCachedBonus(string actorId, string disciplineId) {
public float GetDisciplineSkillBonus(string actorId, string disciplineId) {
public SkillProgressionSaveState CaptureState() {
public void RestoreState(SkillProgressionSaveState save, IReadOnlyList<SkillActor>? actors = null) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Research/ResearchSystem.cs`

### `Assets/Ashfall.Core/Research/ResearchSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 559 lines / 23598 bytes.
- SHA-256: `2469bdb77e91c39446bdb78411378f5555aead27bb9f60df47be9e081c24cd9d`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ResearchSystem
public const string SystemId = "research_system";
public ResearchState State { get; private set; }
public int CatalogCount => _catalog.Count;
public IReadOnlyDictionary<string, ResearchKnowledgeDef> Catalog => _catalog;
public event Action<ResearchKnowledgeDef>? OnResearchCompleted;
public event Action<string>? OnManualUnlocked;
public void Register(ResearchKnowledgeDef def) {
public void UnlockManual(string id) {
public bool IsManualUnlocked(string id) => !string.IsNullOrEmpty(id) && State.unlockedIds.Contains(id);
public bool HasCapability(string knowledgeId) => IsManualUnlocked(knowledgeId);
public ResearchEligibility GetEligibility(string id) {
public bool StartResearch(string id, int day) {
public int GetDaysRemaining(string id) {
public IReadOnlyList<ResearchKnowledgeDef> GetAvailableNodes() {
public IReadOnlyList<ResearchKnowledgeDef> GetLockedNodes() {
public IReadOnlyList<ResearchKnowledgeDef> GetCompletedNodes() {
public IReadOnlyList<string> GetDependents(string id) {
public void Tick(int newDay) {
public bool CompleteResearch(string id) {
public ResearchKnowledgeDef? GetActiveResearch() {
public ResearchKnowledgeDef? GetKnowledge(string id) {
public bool TryAddResearchPoints(int amount, string sourceId) {
public bool TrySpendResearchPoints(int amount, string purposeId) {
public int GetResearchPoints() => Math.Max(0, State.researchPointsAvailable);
public BlueprintProgressState? GetBlueprintProgress(string blueprintId) {
public bool IsBlueprintUnlocked(string blueprintId) {
public bool TryAddBlueprintProgress( string blueprintId, int amount, int requiredPoints, int completedDay, string sourceTechId = "")
public event Action<BlueprintProgressState>? OnBlueprintUnlocked;
public ResearchState CaptureState() {
public void RestoreState(ResearchState saved) {
```


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/ApprenticeshipSystem.cs`

### `Assets/Ashfall.Core/ApprenticeshipSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 495 lines / 21116 bytes.
- SHA-256: `1c8336bbdcdd6d156ca6163857e76bc31ee0960d3348098de2e184fe33536a24`.
- Architecture signals: seeded references=3; save/restore symbols=2; typed event declarations=17; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MentorshipDef
public string mentorship_id { get; set; } = string.Empty;
public string name { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public string discipline { get; set; } = string.Empty;
public string required_room_tag { get; set; } = "workshop";
public float daily_xp_rate { get; set; } = 15f;
public string manual_item_id { get; set; } = string.Empty;
public int manual_transcription_days { get; set; } = 4;
public string legacy_trait_id { get; set; } = "trait_stoic_craftsman";
public float legacy_xp_grant { get; set; } = 150f;
public sealed class MentorshipCatalog
public int schema_version { get; set; } = 1;
public List<MentorshipDef> mentorships { get; set; } = new List<MentorshipDef>();
public sealed class TranscriptionTask
public string taskId = string.Empty;
public string survivorId = string.Empty;
public string mentorshipId = string.Empty;
public int daysWorked;
public int daysRequired = 4;
public bool isComplete;
public sealed class SurvivorWill
public string willId = string.Empty;
public string testatorSurvivorId = string.Empty;
public string primaryBeneficiaryId = string.Empty;
public string fallbackBeneficiaryId = string.Empty;
public List<string> bequeathedItemIds = new List<string>();
public string finalWords = string.Empty;
public bool isExecuted;
public int executionDay = -1;
public sealed class ApprenticeshipState
public string systemId = ApprenticeshipSystem.SystemId;
public List<Apprenticeship> activePairs = new List<Apprenticeship>();
public List<string> completedSkillIds = new List<string>();
public List<TranscriptionTask> transcriptionTasks = new List<TranscriptionTask>();
public List<SurvivorWill> registeredWills = new List<SurvivorWill>();
public List<SurvivorWill> executedWills = new List<SurvivorWill>();
public Dictionary<string, List<string>> legacyTraitsGranted = new Dictionary<string, List<string>>(StringComparer.Ordinal);
public SkillProgressionSaveState skillProgression = new SkillProgressionSaveState();
public sealed class Apprenticeship
public string pairId = string.Empty;
public string mentorId = string.Empty;
public string apprenticeId = string.Empty;
public string targetSkillId = string.Empty;
public string mentorshipId = string.Empty;
public string requiredRoomTag = string.Empty;
public float progressXp;
public float targetXp = 100f;
public int dayStarted = -1;
public bool isComplete;
public bool isCancelled;
public bool isLegacyInherited;
public string milestonePerkId = string.Empty;
public sealed class ApprenticeshipSystem
public const string SystemId = "apprenticeship";
public const string CatalogPath = "apprenticeship_catalog.json";
public ApprenticeshipState State => _state;
public IReadOnlyDictionary<string, MentorshipDef> Catalog => _catalog;
public int MaxConcurrentPairs { get; set; } = 3;
public Func<string, bool>? IsApprenticeEligible { get; set; }
public event Action<Apprenticeship>? OnApprenticeshipCompleted;
public event Action? OnApprenticeshipChanged;
public event Action<string, string>? OnManualTranscribed; // survivorId, manualItemId
public event Action<SurvivorWill, string>? OnWillExecuted; // will, recipientSurvivorId
public event Action<string, string, float>? OnMentorLegacyInherited; // apprenticeId, traitId, xpGranted
public void LoadCatalog(string jsonContent) {
public void RegisterMentorship(MentorshipDef def) {
public ActionResult StartPair(string mentorId, string apprenticeId, string targetSkillId, float targetXp = 100f, string? mentorshipId = null) {
public ActionResult CancelPair(string pairId) {
public ActionResult StartTranscription(string survivorId, string mentorshipId, InventoryContainer? inv = null) {
public ActionResult RegisterWill(SurvivorWill will) {
public ActionResult ExecuteWill(string deceasedSurvivorId, InventoryContainer targetInventory, HashSet<string>? livingSurvivors = null) {
public void NotifyMentorDeath(string deceasedMentorId) {
public void TickDay(int day) {
public List<Apprenticeship> GetActivePairs() => _state.activePairs.FindAll(p => !p.isComplete && !p.isCancelled);
public ApprenticeshipState CaptureState() => CloneState(_state);
public void RestoreState(ApprenticeshipState saved) {
```


# Appendix B.06 — Current Code Architecture: `src/UI/ApprenticeshipPanel.cs`

### `src/UI/ApprenticeshipPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 112 lines / 3864 bytes.
- SHA-256: `673fd6ec4f3d02c7538a6324eedf2eb44a5b4d50d34c27aa5d1d5ebed4b9c611`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class ApprenticeshipPanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _host != null;
public void Bind(ApprenticeshipHostSession session) {
public void Unbind() {
public override void _Ready() {
public void RefreshView() {
public override void _ExitTree() {
```


# Appendix B.07 — Current Code Architecture: `src/UI/SurvivorsPanel.cs`

### `src/UI/SurvivorsPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 332 lines / 15730 bytes.
- SHA-256: `470a17c92ae1a01e75db87d69bf016a31a542c2e771c784189e41a45e56f547a`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=3; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class SurvivorsPanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _survivorsHost != null;
public int RenderedSurvivorCount => _survivorList?.GetChildCount() ?? 0;
public void Bind(SurvivorsHostSession survivors) {
public void Unbind() {
public void RefreshView() {
public override void _Ready() {
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
public override void _ExitTree() {
```


# Appendix B.08 — Current Code Architecture: `src/Main.DutyRoster.cs`

### `src/Main.DutyRoster.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 222 lines / 8335 bytes.
- SHA-256: `642d99ce3aa72601d8b4e170646bd56dd44bea8850f2db0400d69c61afabe1e3`.
- Architecture signals: seeded references=0; save/restore symbols=3; typed event declarations=0; textual Godot mentions=4; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix C.09 — Catalog Census: `Assets/StreamingAssets/Data/research_knowledge.json`

### `Assets/StreamingAssets/Data/research_knowledge.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 21516 bytes / 21514 characters.
- SHA-256: `6eef977ff53a20c22577be27cddf6564426d7d484cece6b17634c136f29d31c7`.
- Root keys: `collection_id`, `knowledge_nodes`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
knowledge_nodes: min=62, max=62, observed_paths=1
knowledge_nodes[].prerequisites: min=0, max=1, observed_paths=2
```

Representative record fields:

- `breakthrough_item`
- `category`
- `days_to_complete`
- `description`
- `display_name`
- `id`
- `prerequisites`

Representative identifiers (ordered, capped for readability):

```text
knowledge_water_basics
knowledge_water_advanced
knowledge_radiation_basics
knowledge_radiation_shielding
knowledge_gas_mask_improved
knowledge_hydroponics
knowledge_solar_basics
knowledge_solar_advanced
knowledge_food_preservation
knowledge_radio_basics
knowledge_radio_advanced
knowledge_shelter_insulation
knowledge_air_filtration
knowledge_scavenge_efficiency
knowledge_combat_training
knowledge_deep_well_hydraulics
knowledge_greenhouse_microclimate
knowledge_cold_canning_preservation
knowledge_apiculture_ecology
knowledge_field_trauma_surgery
knowledge_pathogen_containment
knowledge_pharmacology_synthesis
knowledge_high_temp_metallurgy
knowledge_geothermal_tap
knowledge_submersible_salvage_rig
knowledge_atmospheric_cloud_seeding
knowledge_ionospheric_propagation
knowledge_seismic_fault_mapping
knowledge_ruin_structural_survey
knowledge_field_guide_taxonomy
knowledge_hazmat_breaching_technique
knowledge_fortified_chokepoints
knowledge_defensive_tripwire_arrays
knowledge_automated_sentry_doctrine
knowledge_precision_ballistics
knowledge_subterranean_fungiculture
knowledge_chelation_therapy
knowledge_heavy_foundry_casting
knowledge_signal_triangulation
knowledge_guerrilla_ambush_tactics
knowledge_micro_dosimeter_blueprint
knowledge_water_condenser_blueprint
knowledge_signal_amplifier_blueprint
knowledge_battery_reconditioner_blueprint
knowledge_hydroponic_doser_blueprint
knowledge_uv_sterilizer_blueprint
knowledge_hand_centrifuge_blueprint
knowledge_seismic_geophone_blueprint
knowledge_turret_controller_blueprint
knowledge_encrypted_radio_blueprint
knowledge_radar_scope_blueprint
knowledge_power_armor_servo_blueprint
knowledge_vault_breach_blueprint
knowledge_iff_transponder_blueprint
knowledge_cbrn_filter_blueprint
knowledge_surgical_robot_blueprint
knowledge_field_medicine
knowledge_basic_engineering
knowledge_diesel_mechanics
knowledge_radio_repair
knowledge_water_treatment
knowledge_radiation_measurement
```


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/skills.json`

### `Assets/StreamingAssets/Data/skills.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 55682 bytes / 55664 characters.
- SHA-256: `2bd8a261fc1ce61b1134cce9396810b21131330e85b3acca71dd1dfc0b2848d0`.
- Root keys: `collection_id`, `schema_version`, `skills`.

Array-path census (minimum, maximum, observed rows):

```text
skills: min=161, max=161, observed_paths=1
```

Representative record fields:

- `description`
- `discipline_id`
- `display_name`
- `id`
- `is_expert_skill`
- `skill_bonus`
- `xp_threshold`

Representative identifiers (ordered, capped for readability):

```text
skill_field_dressing
skill_steady_hands
skill_rough_repairs
skill_crafting
skill_workshop_sense
skill_signal_ear
skill_cold_analysis
skill_watchful
skill_trail_memory
skill_hard_living
skill_tap_rack_bang
skill_cold_bore
skill_suppressing_fire
skill_close_quarters
skill_trap_setter
skill_looters_reflex
skill_desensitized
skill_ration_stretcher
skill_iron_stomach
skill_wasteland_brewer
skill_butcher
skill_pharmacologist
skill_mycology
skill_jury_rigger
skill_structural_engineer
skill_hvac_tech
skill_scrapper
skill_sandhog
skill_thermodynamics
skill_steady_hands_field
skill_triage_under_fire
skill_radiologist
skill_anatomist
skill_paramedic
skill_pack_mule
skill_light_step
skill_urban_pathfinder
skill_night_terror
skill_forager
skill_de_escalator
skill_quartermaster
skill_taskmaster
skill_miracle_worker
skill_alchemist
skill_zoonotic_expert
skill_anchor
skill_death_blind
skill_warlord
skill_peacekeeper
skill_juggernaut
skill_apex_predator
skill_survivalist
skill_hydraulic_master
skill_grid_walker
skill_vault_builder
skill_grease_monkey
skill_synthesizer
skill_gaia
skill_wasteland_runner
skill_ghost
skill_stormcaller
skill_rad_walker
skill_polymath
skill_demagogue
skill_shepherd
skill_muckraker
skill_voice_of_the_wastes
skill_iron_chef
skill_tireless
skill_asbestos
skill_armorer
skill_tinkerer
skill_lorekeeper
skill_zealots_bane
skill_chem_resistant
skill_protector
skill_matriarch
skill_pillar_of_atlas
skill_wasteland_scout
skill_child_of_the_ash
skill_cold_calculus
skill_butcher_of_day_30
skill_master_manipulator
skill_dragons_hoard
skill_art_of_war
skill_demolitions_expert
skill_ghost_shooter
skill_supply_chain_master
skill_reclaimed_youth
skill_soul_weaver
skill_lone_wolf
skill_grounded_optimist
skill_living_saint
skill_humbled_healer
skill_clean_and_sober
skill_the_watcher
skill_hyper_aware
skill_fire_breather
skill_sonar
skill_improvised_engineering
skill_radiotrophic
skill_apex_scavenger
skill_zen_state
skill_master_geneticist
skill_the_enforcer
skill_legend_of_the_wastes
skill_the_statesman
skill_cybernetics
skill_beacon_of_truth
skill_master_pathologist
skill_monopolist
skill_deep_delver
skill_logistics_master
skill_forge_master
skill_sanitization_expert
skill_deforester
skill_epidemiologist
skill_celestial_navigator
skill_archivist
skill_auditor
skill_maestro
skill_blockade_runner
skill_executioner
skill_shadow
skill_master_of_disguise
skill_mechanic_prodigy
skill_diplomat
skill_wasteland_gladiator
skill_chief_of_medicine
skill_drone_operator
skill_choir_of_one
skill_hive_tactics
skill_hive_healing
skill_truth_seeker
skill_wildman
skill_second_life
skill_iron_will
skill_unseen_listener
skill_ruthless_capitalist
skill_prodigy
skill_commander
skill_cyber_arm
skill_redemption
skill_overclocked
skill_wasteland_guardian
skill_omniscience
skill_field_surgery
skill_water_filtration
skill_radio_repair
skill_reading_comprehension
skill_mathematical_logic
skill_communal_diplomacy
skill_radiation_awareness
skill_machining_basics
skill_field_triage
skill_cartography
skill_firearm_handling
skill_reactor_maintenance
skill_surgery_assistance
skill_patrol_command
```


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/apprenticeship_catalog.json`

### `Assets/StreamingAssets/Data/apprenticeship_catalog.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 1749 bytes / 1749 characters.
- SHA-256: `5cda72739042dd5796569da304ea63ea8784de451e73c6be77fc5cb710b34c6c`.
- Root keys: `mentorships`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
mentorships: min=4, max=4, observed_paths=1
```

Representative record fields:

- `daily_xp_rate`
- `description`
- `discipline`
- `legacy_trait_id`
- `legacy_xp_grant`
- `manual_item_id`
- `manual_transcription_days`
- `mentorship_id`
- `name`
- `required_room_tag`


# Appendix D.12 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Progression/LatentExpertAwakeningSystemTests.cs`

### `Ashfall.Core.Tests/Progression/LatentExpertAwakeningSystemTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 117; SHA-256: `9e8ae38efac129708e8542f36645d88173e19a1436554f5dc93f3db469cd102f`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
InitialState_Has12RegisteredAwakeningDefinitions
RecordProgress_IncrementsProgressAndAwakensAtThreshold
MultiStepProgress_AwakensOnlyAfterFullThreshold
SaveAndRestore_PreservesProgressAndAwakenedStatus
```


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/SkillProgressionSystemTests.cs`

### `Ashfall.Core.Tests/SkillProgressionSystemTests.cs`

- Current test declarations: Fact=12, Theory=0, InlineData=0.
- File lines: 309; SHA-256: `afe96f5ea4bcfc105c10034a9209a890535f80a2067711326a2fe847af636e25`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CatalogLoader_PopulatesKnownDisciplineSkills
RecordAction_AwardsXpAndReachesTier1
TickDaily_DecaysUnusedDisciplineSkillsToDormant
DormantSkill_ReactivatesWhenPracticed
ExpertSkill_GatedToPredeterminedDiscipline
TryGrantSkill_MilestoneId_BypassesXpThreshold
Epiphany_TriggersOnDesperateSurvivors
Epiphany_PathRunsUnderDrowningMorale
CaptureState_RoundTripsPreservingAllFields
SkillAtrophySystem_FiresAfterWindow_HoldsMultiplier
SkillAtrophySystem_DoesNotFireIfMoraleRecovers
SkillAtrophySystem_RoundTripsViaSaveState
```


# Appendix D.14 — Existing Focused Test Inventory: `Ashfall.Core.Tests/ApprenticeshipSystemTests.cs`

### `Ashfall.Core.Tests/ApprenticeshipSystemTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 117; SHA-256: `55a9799180ecb6143ccc5816301be6351b0ffdfe8b0f502a7d60aba03e7f56aa`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.


# Appendix E.15 — Supporting Code Evidence: `src/Host/ApprenticeshipSaveStore.cs`

### `src/Host/ApprenticeshipSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 56 lines / 2736 bytes.
- SHA-256: `308ee309d4b33071a44d625451b9ae7646ac7263d23e0383d814e428e2ef679b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class ApprenticeshipSaveStore
public const string FileName = "apprenticeship_save.json";
public const string SectionName = "apprenticeship";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCaptureDirect(ApprenticeshipState state) => s_store.CaptureBare(state);
public static ApprenticeshipState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(ApprenticeshipState state) => s_store.CaptureBare(state);
public static ApprenticeshipState? TryRestore(string json) => s_store.RestoreBare(json);
public static bool TrySave(ApprenticeshipState state) => s_store.TrySave(state);
public static ApprenticeshipState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(ApprenticeshipState state) => s_store.CapturePersisted(state);
```


# Appendix E.16 — Supporting Code Evidence: `src/Main.ShelterSocial.cs`

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


# Appendix E.17 — Supporting Code Evidence: `Assets/Ashfall.Core/Survivors/SkillProgressionState.cs`

### `Assets/Ashfall.Core/Survivors/SkillProgressionState.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 115 lines / 4532 bytes.
- SHA-256: `8bd8ed984d90f4aa830075bf691790511e9589d98fc8e4cc456443dd4ad7b4d9`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=1; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public interface SkillActor
public class SimpleSkillActor : SkillActor
public string Id { get; }
public bool IsAlive { get; set; } = true;
public float Morale { get; set; } = 50f;
public float Health { get; set; } = 100f;
public string ExpertDisciplineId { get; set; } = string.Empty;
public void SetSkillBonus(string disciplineId, float bonus) {
public sealed class SkillProgressionState
public bool expertSkillEarned = false;
public List<string> activeSkillIds = new List<string>();
public List<string> dormantSkillIds = new List<string>();
public List<string> disciplineIds = new List<string>();
public List<float> xpValues = new List<float>();
public List<int> lastUsedDays = new List<int>();
public sealed class SkillProgressionSaveState
public List<SkillProgressionState> entries = new List<SkillProgressionState>();
public List<string> survivorIds = new List<string>();
public sealed class SkillAtrophySaveState
public List<string> survivorIds = new List<string>();
public List<float> consecutiveLowMoraleDays = new List<float>();
public List<List<string>> atrophiedSkillIds = new List<List<string>>();
```


# Appendix E.18 — Supporting Code Evidence: `src/Host/ApprenticeshipHostSession.cs`

### `src/Host/ApprenticeshipHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 69 lines / 2327 bytes.
- SHA-256: `d8d91401f3de957b012bf5fc618fa04847ee0cfe0d556f5e22bf621ebb4bb9ae`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ApprenticeshipHostSession
public ApprenticeshipSystem System { get; }
public string LastEvent { get; private set; } = string.Empty;
public ActionResult StartPair(string mentorId, string apprenticeId, string targetSkillId, float targetXp = 100f) {
public ActionResult CancelPair(string pairId) {
public void TickDay(int day) {
public override void Save() {
```


# Appendix E.19 — Supporting Code Evidence: `Assets/Ashfall.Core/Education/ApprenticeshipCurriculumEngine.cs`

### `Assets/Ashfall.Core/Education/ApprenticeshipCurriculumEngine.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 219 lines / 10507 bytes.
- SHA-256: `dc8b33190c8075b30eac21859d6589b4fc4c9eae9d8f125ca1fc716e0794ddd6`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum LiteracyLevel
public sealed class LearnerRecord
public string SurvivorId             { get; set; } = string.Empty;
public LiteracyLevel LiteracyLevel   { get; set; } = LiteracyLevel.Illiterate;
public int ComprehensionPermille     { get; set; } = 0;
public Dictionary<string, int> TradeSkillPermille { get; set; } = new();
public int FatigueSessionCount       { get; set; } = 0;
public LearnerRecord Clone() {
public readonly struct CurriculumSessionResult
public int ComprehensionGained       { get; }
public bool AdvancedLiteracyTier     { get; }
public LiteracyLevel NewLiteracyLevel { get; }
public int FatiguePenaltyPermille    { get; }
public static class ApprenticeshipCurriculumEngine
public const int TierAdvanceThreshold = 1000;
public const int CertificationReadyThreshold = 900;
public const int FatigueOnsetSessions = 3;
public static CurriculumSessionResult AdvanceLiteracySession( LearnerRecord learner, int teacherSkillPermille, int manualAvailabilityPermille, int sessionSeed) {
public static bool EvaluateVocationalCertification(LearnerRecord learner, string tradeId) {
public static int CalculateManualTranscriptionYield(LiteracyLevel masterLiteracyLevel, int laborHours) {
```


# Appendix G.20 — Supporting Regression Evidence: `Ashfall.Core.Tests/Plan12AGenerationTests.cs`

### `Ashfall.Core.Tests/Plan12AGenerationTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 350; SHA-256: `cbccdeff9e84be69134c3f28d9b74866342e220849fe83ae7943d9dd3a3b7084`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Cohort_TryMaturation_BookedChild_FiresAndPersistsOnce
Cohort_TryMaturation_BadInputRejected
Cohort_PersistsMaturationThroughSaveRoundTrip
Apprenticeship_OnCompletion_GrantsCanonicalSkillOnly
Events_Plan12A_Bundle_ExistsAndHooksAreAuthored
Events_Plan12A_ConditionFlags_AreClosedAndDocumented
Questlines_Plan12A_FourChildren_FilledBody_AndNewArcsPresent
Questlines_Plan12A_NarrativeHook_AreClosedAndDocumented
```


# Appendix G.21 — Supporting Regression Evidence: `Ashfall.Core.Tests/CampaignContinuityFlagship54_57Tests.cs`

### `Ashfall.Core.Tests/CampaignContinuityFlagship54_57Tests.cs`

- Current test declarations: Fact=2, Theory=0, InlineData=0.
- File lines: 204; SHA-256: `afeaa1b6d204749d02296af2fb3b06d75309f18d7957f67148f903e93d8403f1`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Continuity_30DayDeterministicReplay_ProducesIdenticalOutputs
Continuity_Day15SaveRestoreSplit_MatchesContinuousRun
```


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/Education/ApprenticeshipCurriculumEngineTests.cs`

### `Ashfall.Core.Tests/Education/ApprenticeshipCurriculumEngineTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 100; SHA-256: `963a8a150b4b92bf76d496e2e73765008eef91ff4e9dd6eea3a5b0dc17889769`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
AdvanceLiteracySession_AdvancesTier_WhenComprehensionReachesThreshold
AdvanceLiteracySession_FatiguePenalty_AppliesAfterOnsetSessions
EvaluateVocationalCertification_ReturnsFalse_WhenLiteracyBelowFunctional
EvaluateVocationalCertification_ReturnsTrue_WhenQualified
CalculateManualTranscriptionYield_ZeroForNonScholarly_PositiveForScholarly
```


# Appendix G.23 — Supporting Regression Evidence: `Ashfall.Core.Tests/Plan12DCrossSystemContinuityTests.cs`

### `Ashfall.Core.Tests/Plan12DCrossSystemContinuityTests.cs`

- Current test declarations: Fact=21, Theory=0, InlineData=0.
- File lines: 451; SHA-256: `b7d649ce70d8312f39a3366181c0ae664aab14ff0e56a947676de29d6e940b64`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ApprenticeshipCompletion_FlowsThrough_SkillProgressionSystem
CohortMaturation_FiresThrough_CohortSystem_TryMaturation
DecorMorale_RoutesThrough_NeedsSystemModifiers
MemorialPlaque_References_MemorialSystemProvenance
ApprenticeshipCompletion_CannotHappenBefore_StartPair
Maturation_CannotHappenBefore_BookChild
DecorRemoval_OnEmptySlot_ReturnsFalse
ResolvePlaqueSlot_DoesNotCrash_WithEmptyHeirloom
ResolvePlaqueSlot_DoesNotCrash_WithNullInputs
CohortTryMaturation_UnknownChild_ReturnsFalse
ApprenticeshipStartPair_UnqualifiedMentor_Rejected
ShelterDecorRemove_UnknownRoomSlot_ReturnsFalse
ShelterDecorAssign_EmptyRoomId_ReturnsFalse
ShelterDecorAssign_EmptySlotId_ReturnsFalse
ShelterDecorAssign_EmptyItemId_StillSucceeds
CohortSystem_BookChild_SaveRestore_MaturationStillWorks
ApprenticeshipSystem_StartPair_Tick_SaveRestore_CompletionStillFires
ShelterDecorSystem_Assign_SaveRestore_PlacementPreserved
RationConflictSystem_BuildResentment_SaveRestore_ResentmentPreserved
IdeologicalFrictionSystem_RegisterBeliefs_SaveRestore_FrictionStillDetected
IdeologicalFrictionSystem_SynergyBeliefs_SaveRestore_SynergyPreserved
```


# Appendix H.24 — Supporting Authority Document: `docs/progression/LATENT_EXPERT_AWAKENING_MATRIX.md`

### `docs/progression/LATENT_EXPERT_AWAKENING_MATRIX.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 26 lines / 2300 bytes.
- SHA-256: `65e141cb6b78c949f7175003d6012f03a2e430bc348c22e564d7c258737d826d`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix H.25 — Supporting Authority Document: `docs/progression/LATENT_EXPERT_TRAIT_INVENTORY.md`

### `docs/progression/LATENT_EXPERT_TRAIT_INVENTORY.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 5671 lines / 457091 bytes.
- SHA-256: `1bd46cc7b8e4566963c288e7e235ec2406306a2f6790abd6da424024023187c0`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=2; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum TraitCategory
public sealed class LatentTraitDefinition
public string TraitId { get; }
public string DisplayName { get; }
public TraitCategory Category { get; }
public string RequiredProfession { get; }
public string CrisisTriggerType { get; }
public double MoraleAwakeningBonus { get; }
public sealed class SurvivorExpertiseRecord
public string SurvivorId { get; }
public string LatentTraitId { get; }
public bool IsAwakened { get; private set; }
public int DayAwakened { get; private set; }
public string AwakeningCrisisContext { get; private set; }
public bool TryAwaken(int currentDay, string crisisContext) {
public sealed class LatentExpertAwakeningSystem
public void RegisterTraitDefinition(LatentTraitDefinition def) {
public void EnrollSurvivor(string survivorId, string latentTraitId) {
public bool TriggerAwakening(string survivorId, int currentDay, string crisisContext, out LatentTraitDefinition awakenedTrait) {
public bool IsSurvivorAwakened(string survivorId) {
public sealed class LatentExpertAwakeningTests
public void LatentTrait_AwakeningScenario_001_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_002_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_003_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_004_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_005_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_006_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_007_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_008_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_009_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_010_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_011_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_012_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_013_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_014_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_015_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_016_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_017_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_018_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_019_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_020_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_021_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_022_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_023_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_024_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_025_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_026_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_027_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_028_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_029_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_030_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_031_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_032_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_033_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_034_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_035_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_036_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_037_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_038_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_039_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_040_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_041_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_042_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_043_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_044_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_045_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_046_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_047_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_048_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_049_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_050_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_051_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_052_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_053_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_054_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_055_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_056_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_057_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_058_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_059_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_060_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_061_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_062_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_063_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_064_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_065_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_066_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_067_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_068_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_069_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_070_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_071_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_072_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_073_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_074_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_075_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_076_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_077_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_078_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_079_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_080_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_081_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_082_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_083_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_084_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_085_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_086_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_087_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_088_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_089_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_090_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_091_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_092_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_093_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_094_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_095_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_096_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_097_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_098_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_099_ExecutesDeterministically() {
public void LatentTrait_AwakeningScenario_100_ExecutesDeterministically() {
```


# Appendix H.26 — Supporting Authority Document: `docs/research/PLAN26A_PLAN34_RECONCILIATION.md`

### `docs/research/PLAN26A_PLAN34_RECONCILIATION.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 45 lines / 4722 bytes.
- SHA-256: `5e216256b8a380dec4a0588e56646426f102d43da7600199d2cd279564ba0290`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| latent progress and awakening facts | LatentExpertAwakeningSystem | skill levels/XP and active skill grants | SkillProgressionSystem | Owner emits/reads a typed fact; no mirror state. |
| latent progress and awakening facts | LatentExpertAwakeningSystem | research nodes and unlocks | ResearchSystem | Owner emits/reads a typed fact; no mirror state. |
| latent progress and awakening facts | LatentExpertAwakeningSystem | mentor/apprentice development | ApprenticeshipSystem | Owner emits/reads a typed fact; no mirror state. |
| latent progress and awakening facts | LatentExpertAwakeningSystem | available roles and physical constraints | DutyRoster/medical | Owner emits/reads a typed fact; no mirror state. |
| skill levels/XP and active skill grants | SkillProgressionSystem | latent progress and awakening facts | LatentExpertAwakeningSystem | Owner emits/reads a typed fact; no mirror state. |
| skill levels/XP and active skill grants | SkillProgressionSystem | research nodes and unlocks | ResearchSystem | Owner emits/reads a typed fact; no mirror state. |
| skill levels/XP and active skill grants | SkillProgressionSystem | mentor/apprentice development | ApprenticeshipSystem | Owner emits/reads a typed fact; no mirror state. |
| skill levels/XP and active skill grants | SkillProgressionSystem | available roles and physical constraints | DutyRoster/medical | Owner emits/reads a typed fact; no mirror state. |
| research nodes and unlocks | ResearchSystem | latent progress and awakening facts | LatentExpertAwakeningSystem | Owner emits/reads a typed fact; no mirror state. |
| research nodes and unlocks | ResearchSystem | skill levels/XP and active skill grants | SkillProgressionSystem | Owner emits/reads a typed fact; no mirror state. |
| research nodes and unlocks | ResearchSystem | mentor/apprentice development | ApprenticeshipSystem | Owner emits/reads a typed fact; no mirror state. |
| research nodes and unlocks | ResearchSystem | available roles and physical constraints | DutyRoster/medical | Owner emits/reads a typed fact; no mirror state. |
| mentor/apprentice development | ApprenticeshipSystem | latent progress and awakening facts | LatentExpertAwakeningSystem | Owner emits/reads a typed fact; no mirror state. |
| mentor/apprentice development | ApprenticeshipSystem | skill levels/XP and active skill grants | SkillProgressionSystem | Owner emits/reads a typed fact; no mirror state. |
| mentor/apprentice development | ApprenticeshipSystem | research nodes and unlocks | ResearchSystem | Owner emits/reads a typed fact; no mirror state. |
| mentor/apprentice development | ApprenticeshipSystem | available roles and physical constraints | DutyRoster/medical | Owner emits/reads a typed fact; no mirror state. |
| available roles and physical constraints | DutyRoster/medical | latent progress and awakening facts | LatentExpertAwakeningSystem | Owner emits/reads a typed fact; no mirror state. |
| available roles and physical constraints | DutyRoster/medical | skill levels/XP and active skill grants | SkillProgressionSystem | Owner emits/reads a typed fact; no mirror state. |
| available roles and physical constraints | DutyRoster/medical | research nodes and unlocks | ResearchSystem | Owner emits/reads a typed fact; no mirror state. |
| available roles and physical constraints | DutyRoster/medical | mentor/apprentice development | ApprenticeshipSystem | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Add a strict awakening definition catalog only if current growth requires it; otherwise maintain code-owned definitions with integrity checks. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Define adult competence states as projections over existing skills, duties, injuries and relationships. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Add explicit re-specialization commands with previews and bounded support/recovery choices. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Expose known, suspected and awakened expertise truthfully in roster/duty/expedition views. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.558 — Additional Current Architecture Evidence: `src/Main.ExpandedShelterSystems.cs`

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


# Appendix Q.559 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

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


# Appendix Q.560 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs`

### `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 764 lines / 36041 bytes.
- SHA-256: `88acf308958529905272f8a5e7b772bf3cd765d80051d6442820b5319944f652`.
- Architecture signals: seeded references=4; save/restore symbols=2; typed event declarations=21; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WorkshopState
public string systemId = WorkshopReverseEngineeringSystem.SystemId;
public string selectedRelicId = string.Empty;
public string assignedResearcherId = string.Empty;
public int workPhase; // 0=idle, 1=examining, 2=dismantling, 3=repairing, 4=researching
public float progressHours;
public float hoursRequired;
public List<string> reservedComponentIds = new List<string>();
public List<int> reservedComponentAmounts = new List<int>();
public bool isComplete;
public string completionUnlockId = string.Empty; // research or recipe unlocked
public List<string> completedRelicIds = new List<string>();
public string activeTechSalvageId = string.Empty;
public string techSourceItemId = string.Empty;
public bool techSourceConsumed;
public int techStartedDay;
public float techEquipmentQuality01;
public List<string> completedTechSalvageIds = new List<string>();
public List<ResearchNoteState> researchNotes = new List<ResearchNoteState>();
public sealed class RelicDefinition
public string relic_id = string.Empty;
public string display_name = string.Empty;
public string description = string.Empty;
public List<string> required_components = new List<string>();
public float repair_time_hours = 8f;
public int morale_bonus;
public string dialogue_event_id = string.Empty;
public string restoration_text = string.Empty;
public string world_flag = string.Empty;
public string research_unlock_id = string.Empty; // knowledge node unlocked on research
public string dismantle_yield_item = string.Empty;
public int dismantle_yield_amount = 1;
public string category = "relic";
public sealed class RelicCatalog
public string schema_version = "1.0";
public List<RelicDefinition> relics = new List<RelicDefinition>();
public List<RelicDefinition> recipes { get => relics; set => relics = value; }
public sealed class ResearchNoteState
public string noteId = string.Empty;
public string techId = string.Empty;
public string researcherId = string.Empty;
public int day;
public string progressBand = string.Empty;
public sealed class WorkshopReverseEngineeringSystem
public const string SystemId = "workshop_reverse_engineering";
public WorkshopState State => _state;
public IReadOnlyDictionary<string, RelicDefinition> Catalog => _relicCatalog;
public event Action<ActionResult> OnActionCompleted;
public event Action OnWorkshopStateChanged;
public void BindSkillEvaluator(Func<string, float> evaluator) {
public void LoadCatalog(RelicCatalog catalog) {
public void LoadTechSalvageCatalog(IEnumerable<PreWarTechDef> definitions) {
public void BindTechSalvageRng(ISeededRng rng) {
public IReadOnlyDictionary<string, PreWarTechDef> TechSalvageCatalog => _techCatalog;
public PreWarTechDef? GetTechSalvage(string techId) {
public bool IsTechSalvageCompleted(string techId) =>
public void RegisterRelic(RelicDefinition relic) {
public RelicDefinition? GetRelic(string relicId) {
public bool IsRelicCompleted(string relicId) =>
public bool IsBusy => _state.workPhase > 0 && !_state.isComplete;
public TechDismantlePreview PreviewTechDismantle( string sourceItemId, string researcherId, ResearchFacilityContext? facility = null) {
public ActionResult StartTechDismantle( string sourceItemId, string researcherId, int day = 0, ResearchFacilityContext? facility = null) {
public ActionResult Examine(string relicId) {
public ActionResult StartDismantle(string relicId, string researcherId) {
public ActionResult StartRepair(string relicId, string researcherId) {
public ActionResult StartResearch(string relicId, string researcherId) {
public ActionResult TickProgress(float hoursElapsed) {
public ActionResult CancelJob() {
public WorkshopState CaptureState() {
public void RestoreState(WorkshopState saved) {
public event Action<TechSalvageFailure>? OnTechSalvageFailure;
public event Action<string>? OnBlueprintInsight;
```


# Appendix Q.561 — Additional Current Architecture Evidence: `src/Main.CampaignServices.cs`

### `src/Main.CampaignServices.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 217 lines / 8492 bytes.
- SHA-256: `ae7f8aaebcb44eb311fdd249962e2d1fd784c302a3463aec9a75e7e8abedc7d0`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public int ComposeCampaignCallCount => _composeCampaignCallCount;
public void ResetComposeCampaignCallCount() => _composeCampaignCallCount = 0;
public void ComposeCampaign() {
public void TickSharedSkillProgression(int day) {
public Ashfall.Core.Survivors.SkillProgressionSystem EnsureSharedSkillProgression() {
public Ashfall.Core.Economy.FactionStanceEngine EnsureSharedFactionStance() {
```


# Appendix Q.562 — Additional Current Architecture Evidence: `src/UI/SkillMatrixPanel.cs`

### `src/UI/SkillMatrixPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 499 lines / 22392 bytes.
- SHA-256: `06ff9ca51668f28d3477bc9df97ef9d90d07d86149ab2b6cde6bf3972be2240a`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=4; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class SkillMatrixPanel : Control, IBindablePanel
public event Action? OnClose;
public event Action<string>? OnSurvivorSelected;
public bool IsBound => _skills != null;
public void Bind(SkillProgressionSystem skills, SurvivorsHostSession? survivors = null) {
public void Unbind() {
public override void _Ready() {
public override void _ExitTree() {
public void RefreshView() {
internal static List<AshfallDataGrid.Row> BuildFixtureRows() {
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
```


# Appendix Q.563 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Archaeology/ArchaeologySystem.cs`

### `Assets/Ashfall.Core/Archaeology/ArchaeologySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 570 lines / 24961 bytes.
- SHA-256: `4e0292bdf601c43ef6d76ebba89b0ed7f6761bcc14b6b8ac81ff2b5857fc3a9b`.
- Architecture signals: seeded references=2; save/restore symbols=5; typed event declarations=13; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class LoreArchiveDef
public string archive_id { get; set; } = string.Empty;
public string title_key { get; set; } = string.Empty;
public string summary_key { get; set; } = string.Empty;
public string era { get; set; } = "PreExchange";
public List<string> topic_tags { get; set; } = new List<string>();
public int encryption_tier { get; set; } = 1;
public float required_engineering { get; set; } = 2.0f;
public float base_work_hours { get; set; } = 12.0f;
public float power_kw { get; set; } = 2.0f;
public float corruption_risk { get; set; } = 0.08f;
public string required_key_item_id { get; set; } = "item_decryption_keycard_prewar";
public int research_reward { get; set; } = 20;
public float broker_value { get; set; } = 100.0f;
public bool unique { get; set; } = true;
public sealed class ArchaeologyCatalogContainer
public int schema_version { get; set; } = 1;
public List<LoreArchiveDef> archives { get; set; } = new List<LoreArchiveDef>();
public sealed class ExcavationSite
public string siteId { get; set; } = string.Empty;
public string zoneId { get; set; } = string.Empty;
public string displayName { get; set; } = string.Empty;
public bool discovered { get; set; }
public float excavationProgress { get; set; } // 0..100
public bool exhausted { get; set; }
public string archiveId { get; set; } = string.Empty;
public sealed class PreWarArchiveInstance
public string archiveId { get; set; } = string.Empty;
public string titleKey { get; set; } = string.Empty;
public string summaryKey { get; set; } = string.Empty;
public int encryptionTier { get; set; } = 1;
public float decryptionProgress { get; set; } // 0..100
public bool encrypted { get; set; } = true;
public bool corrupted { get; set; }
public bool unlocked { get; set; }
public bool sold { get; set; }
public bool researchClaimed { get; set; }
public int researchPoints { get; set; } = 20;
public float brokerValue { get; set; } = 100.0f;
public sealed class ArchaeologyState
public string systemId = ArchaeologySystem.SystemId;
public List<ExcavationSite> sites = new List<ExcavationSite>();
public List<PreWarArchiveInstance> archives = new List<PreWarArchiveInstance>();
public List<string> unlockedLoreIds = new List<string>();
public List<string> soldArchiveIds = new List<string>();
public sealed class ArchaeologySystem
public const string SystemId = "archaeology";
public ArchaeologyState State => CaptureState();
public IReadOnlyList<ExcavationSite> Sites => CaptureState().sites;
public IReadOnlyList<PreWarArchiveInstance> Archives => CaptureState().archives;
public event Action<ExcavationSite>? OnExcavationSiteDiscovered;
public event Action<PreWarArchiveInstance>? OnArchiveRecovered;
public event Action<PreWarArchiveInstance>? OnDecryptionStarted;
public event Action<PreWarArchiveInstance>? OnArchiveCorrupted;
public event Action<PreWarArchiveInstance, int>? OnLoreUnlocked;
public event Action<PreWarArchiveInstance, float>? OnArchiveSold;
public void LoadCatalog(string dataPath) {
public void RegisterArchive(LoreArchiveDef def) {
public ExcavationSite? SurveyRuins(string zoneId, float scoutSkill) {
public PreWarArchiveInstance? ProgressExcavation(string siteId, float laborHours) {
public ActionResult ProgressDecryption( string archiveId, float hours, float engineerSkill, bool hasPower, bool hasKeycard = false)
public ActionResult SellArchiveToBroker(string archiveId) {
public ArchaeologyState CaptureState() => NormalizeState(_state);
public void RestoreState(ArchaeologyState state) {
```


# Appendix Q.564 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`

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


# Appendix Q.565 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Collectibles/CollectibleEffectDispatcher.cs`

### `Assets/Ashfall.Core/Collectibles/CollectibleEffectDispatcher.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 351 lines / 16641 bytes.
- SHA-256: `8b68c52ae37580be2981c55da7f02b4c4b83dc63480d13f19525b5f38c0e5d93`.
- Architecture signals: seeded references=1; save/restore symbols=0; typed event declarations=2; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class CollectibleDispatchResult
public bool IsCollectible;
public bool AlreadyDiscovered;
public string EffectType = string.Empty;
public bool EffectApplied;
public bool DiscoveryRegistered;
public string FailureReason = string.Empty;
public string? DiscoveryLocationId;
public bool HasDiscoveryEffects => !string.IsNullOrEmpty(EffectType) && EffectType != "none" && EffectApplied;
public class CollectibleEffectDispatcher
public const float MaxMoraleEffectValue = 10f;
public CollectibleDiscoveryState Discovery => _discovery;
public event Action<CollectibleDispatchResult>? OnCollectibleDiscovered;
public CollectibleDispatchResult DispatchOnAcquire(string itemId, string? discoveryLocationId = null) {
public CollectibleMigrationReport ReconcileDiscoveredSubsystemState( Func<VinylMoraleSystem?>? vinylProvider = null, ISeededRng? vinylRng = null) {
public sealed class CollectibleMigrationReport
public int KnowledgeReconciled;
public int LocationReconciled;
public int VinylChecked;
```


# Appendix Q.566 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Generations/SecondGenerationMilestoneEngine.cs`

### `Assets/Ashfall.Core/Generations/SecondGenerationMilestoneEngine.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 228 lines / 8593 bytes.
- SHA-256: `1686ef258075fcf811506cad5777fa76f1a148dde2f4da1c01c9932c165fc524`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum MilestoneKind
public readonly struct MilestoneEvaluationResult
public bool Eligible { get; }
public MilestoneKind Milestone { get; }
public DevelopmentStage RequiredStage { get; }
public float RequiredEducation { get; }
public int AptitudeBonusPermille { get; }
public string DossierNote { get; }
public static class SecondGenerationMilestoneEngine
public static MilestoneEvaluationResult EvaluateNextMilestone(ChildProfile profile, int currentDay) {
public static int CalculateSuccessionReadinessPermille(ChildProfile profile, bool parentDeceased, int parentKinshipPermille = 500) {
public static Dictionary<string, int> CalculateAptitudeModifiers(ChildProfile profile) {
```


# Appendix Q.567 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/LibraryStudySystem.cs`

### `Assets/Ashfall.Core/LibraryStudySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 353 lines / 15082 bytes.
- SHA-256: `d8ed15df8fd325172f8ad2b9e2ae8629172d95cbf94708b833150fda32039f5f`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class LibraryStudyState
public string systemId = LibraryStudySystem.SystemId;
public List<StudyJob> activeJobs = new List<StudyJob>();
public List<string> completedManualIds = new List<string>();
public int totalStudyHours;
public sealed class ManualDefinition
public string manual_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string category { get; set; } = string.Empty;       // "technical", "medical", "military", etc.
public int studyHoursRequired { get; set; } = 10;
public float fatiguePerHour { get; set; } = 0.3f;
public float moraleEffect { get; set; } = -0.5f;           // studying is draining
public List<string> skillXpGrants { get; set; } = new List<string>(); // skill_id, xp_amount pairs
public List<string> researchUnlocks { get; set; } = new List<string>();
public List<string> knowledgeUnlocks { get; set; } = new List<string>();
public List<string> prerequisites { get; set; } = new List<string>();
public bool requiresPower { get; set; } = true;
public List<string> lootTableIds { get; set; } = new List<string>();
public List<string> expeditionRewardIds { get; set; } = new List<string>();
public List<string> traderPoolIds { get; set; } = new List<string>();
public string archiveScribingRecipeId { get; set; } = string.Empty;
public List<string> startingOriginIds { get; set; } = new List<string>();
public string originFacility { get; set; } = string.Empty;
public int technicalComplexityTier { get; set; } = 1;
public string schematicSummary { get; set; } = string.Empty;
public sealed class StudyJob
public string jobId = string.Empty;
public string manualId = string.Empty;
public string readerId = string.Empty;
public int dayStarted = -1;
public float progressHours;
public bool isComplete;
public bool isCancelled;
public sealed class LibraryStudySystem
public const string SystemId = "library_study";
public Func<bool>? PowerAvailable { get; set; }
public bool IsManualPowered(string manualId) {
public LibraryStudyState State => _state;
public IReadOnlyDictionary<string, ManualDefinition> Catalog => _catalog;
public event Action<StudyJob> OnJobCompleted;
public event Action OnLibraryChanged;
public bool IsReaderStudying(string survivorId) {
public static string NormalizeDiscipline(string category) {
public float GetComprehensionRate(string readerId, string manualId) {
public float GetEffectiveStudyHours(string readerId, string manualId) {
public float GetEstimatedDays(string readerId, string manualId) {
public void LoadCatalog(List<ManualDefinition> manuals) {
public ActionResult StartStudy(string manualId, string readerId) {
public ActionResult CancelStudy(string jobId) {
public void TickDay(int day) {
public List<StudyJob> GetActiveJobs() => _state.activeJobs.FindAll(j => !j.isComplete && !j.isCancelled);
public bool IsManualCompleted(string manualId) => _state.completedManualIds.Contains(manualId);
public LibraryStudyState CaptureState() => CloneState(_state);
public void RestoreState(LibraryStudyState saved) {
```


# Appendix Q.568 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Research/ResearchUnlockBridge.cs`

### `Assets/Ashfall.Core/Research/ResearchUnlockBridge.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 305 lines / 12403 bytes.
- SHA-256: `0fd3512906ad9f269014fdcd010871a07f47d4ae2b105ff0504752ddea752250`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=4; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ResearchUnlockDef
public string id { get; set; } = string.Empty;
public string research_node_id { get; set; } = string.Empty;
public string unlock_type { get; set; } = "item"; // item, recipe, expedition, shelter, combat, medical
public string unlock_target_id { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public sealed class ResearchUnlockState
public int schema_version { get; set; } = 1;
public List<string> grantedUnlockIds { get; set; } = new List<string>();
public List<string> grantedItems { get; set; } = new List<string>();
public List<string> unlockedRecipeIds { get; set; } = new List<string>();
public List<string> unlockedCapabilities { get; set; } = new List<string>();
public interface IResearchUnlockSink
public sealed class InventoryResearchUnlockSink : IResearchUnlockSink
public IReadOnlyCollection<string> UnlockedRecipes => _unlockedRecipes;
public IReadOnlyCollection<string> EnabledCapabilities => _enabledCapabilities;
public bool GrantBreakthroughItem(string itemId, int quantity = 1) {
public bool UnlockRecipe(string recipeId) {
public bool EnableCapability(string capabilityId, string domain) {
public sealed class ResearchUnlockBridge
public const string SystemId = "research_unlock_bridge";
public const string DefaultCatalogFileName = "research_unlocks.json";
public Action<ResearchUnlockDef>? OnUnlockGrantedSeam { get; set; }
public Action<string, int>? OnBreakthroughItemAwardedSeam { get; set; }
public Action<string>? OnRecipeUnlockedSeam { get; set; }
public Action<string, string>? OnCapabilityEnabledSeam { get; set; }
public ResearchUnlockState State => _state;
public IReadOnlyList<ResearchUnlockDef> Catalog => _catalog;
public void RegisterUnlock(ResearchUnlockDef def) {
public void LoadCatalog(string json) {
public static ResearchUnlockBridge LoadFromDirectory(string dataDir, IFileIO fileIO) {
public void BindResearchSystem(ResearchSystem system, IResearchUnlockSink? sink = null) {
public int ProcessResearchCompletion(string nodeId, IResearchUnlockSink? sink = null) {
public int SynchronizeCompletedResearch(IEnumerable<string> completedNodeIds, IResearchUnlockSink? sink = null) {
public ResearchUnlockState CaptureState() {
public void RestoreState(ResearchUnlockState? state) {
public bool HasCapability(string capabilityId) {
public bool HasRecipe(string recipeId) {
public bool HasItem(string itemId) {
public bool HasUnlock(string unlockId) {
public IReadOnlyList<string> UnlockedCapabilities => _state.unlockedCapabilities;
public IReadOnlyList<string> UnlockedRecipes => _state.unlockedRecipeIds;
public IReadOnlyList<string> GrantedItems => _state.grantedItems;
public IReadOnlyList<string> GrantedUnlockIds => _state.grantedUnlockIds;
public ResearchUnlockCensus GetCensus() {
public readonly struct ResearchUnlockCensus
public readonly int TotalCatalogUnlocks;
public readonly int GrantedUnlocksCount;
public readonly int GrantedItemsCount;
public readonly int UnlockedRecipesCount;
public readonly int UnlockedCapabilitiesCount;
```


# Appendix Q.569 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/AutopsySystem.cs`

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


# Appendix Q.570 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentDeepChainGate.cs`

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


# Appendix Q.571 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Research/ResearchState.cs`

### `Assets/Ashfall.Core/Research/ResearchState.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 55 lines / 1963 bytes.
- SHA-256: `934efa676f07ef3c9ab1bfad65f915cfdd36899e9f1a9faebce8daee60beb620`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ResearchState
public string systemId = ResearchSystem.SystemId;
public bool expansionUnlocked;
public int currentDay;
public List<string> unlockedIds = new List<string>();
public string activeResearchId = string.Empty;
public int activeResearchDays;
public List<string> completedIds = new List<string>();
public int researchPointsAvailable;
public int researchPointsLifetimeEarned;
public List<BlueprintProgressState> blueprintProgress = new List<BlueprintProgressState>();
public sealed class BlueprintProgressState
public string blueprintId = string.Empty;
public int progressPoints;
public int requiredPoints;
public string discoveryState = "unknown";
public int completedDay;
public List<string> sourceTechIds = new List<string>();
public BlueprintProgressState Clone() {
```


# Appendix Q.572 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`

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


# Appendix Q.573 — Additional Current Architecture Evidence: `src/Host/ResearchHostSession.cs`

### `src/Host/ResearchHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 185 lines / 7526 bytes.
- SHA-256: `c95b57af5524ee1baf3cff89582c68bd3d7b1932c95b3c8c4c68e047b592d0d3`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ResearchHostSession
public ResearchSystem Engine { get; }
public string LastEvent { get; private set; } = string.Empty;
public static ResearchHostSession Create(string dataDir, ResearchSystem? engine = null) {
public void LoadCatalog(string dataDir) {
public bool IsUnlocked => Engine.State.expansionUnlocked;
public int CurrentDay => Engine.State.currentDay;
public int CatalogCount => Engine.CatalogCount;
public int CompletedCount => Engine.State.completedIds.Count;
public int UnlockedCount => Engine.State.unlockedIds.Count;
public string ActiveResearchId => Engine.State.activeResearchId ?? string.Empty;
public int ActiveResearchDays => Engine.State.activeResearchDays;
public IReadOnlyDictionary<string, ResearchKnowledgeDef> Catalog => Engine.Catalog;
public ResearchKnowledgeDef? GetActiveResearch() => Engine.GetActiveResearch();
public void Unlock(int day) {
public Func<bool>? StartResearchGate { get; set; }
public bool StartResearch(string id, int day) {
public bool TryStart(string id) => TryStart(id, CurrentDay, out _);
public bool TryStart(string id, int day, out ResearchEligibilityCode code) {
public string FormatFailureCode(ResearchEligibility eligibility) {
public IReadOnlyList<ResearchKnowledgeDef> AvailableNodes() => Engine.GetAvailableNodes();
public IReadOnlyList<ResearchKnowledgeDef> LockedNodes() => Engine.GetLockedNodes();
public IReadOnlyList<ResearchKnowledgeDef> CompletedNodes() => Engine.GetCompletedNodes();
public int DaysRemaining(string id) => Engine.GetDaysRemaining(id);
public ResearchEligibility GetEligibility(string id) => Engine.GetEligibility(id);
public IReadOnlyList<string> GetDependents(string id) => Engine.GetDependents(id);
public void AdvanceDay(int day) {
public bool CompleteResearch(string id) {
public ResearchSave CaptureSave() {
public void RestoreSave(ResearchSave save) {
public sealed class ResearchSave
public string systemId = ResearchSystem.SystemId;
public ResearchState state = new ResearchState();
```


# Appendix Q.574 — Additional Current Architecture Evidence: `src/Host/HostCli.ResearchUnlock.cs`

### `src/Host/HostCli.ResearchUnlock.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 203 lines / 9391 bytes.
- SHA-256: `a91f702f4e5abef7a22a04a859a7bf573b007c0682cd9f9e730d399d094279cf`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class HostCliResearchUnlock
public static int RunSelfTest(string dataDir) {
```


# Appendix Q.575 — Additional Current Architecture Evidence: `src/Host/CraftingHostSession.cs`

### `src/Host/CraftingHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 391 lines / 18284 bytes.
- SHA-256: `5e226d17d80a157213152dddc664ee701f5e79c7d8b3ff8ce08b3c103459ebb0`.
- Architecture signals: seeded references=3; save/restore symbols=8; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CraftingHostSession
public CraftingSystem Engine { get; }
public InventoryContainer Inventory { get; }
public WorkshopReverseEngineeringSystem Workshop { get; }
public PharmaLabSystem PharmaLab { get; }
public ResearchSystem Research { get; }
public System.Collections.Generic.List<Recipe> Recipes { get; } =
public string LastEvent { get; private set; } = string.Empty;
public ItemCatalog? LoadedItemCatalog { get; private set; }
public static CraftingHostSession Create( string dataDir, InventoryContainer inventory, ResearchSystem? research = null, ISeededRng? rng = null, ILog? log = null)
public void SeedStation() {
public void SyncStations(IEnumerable<CraftingStation> stations) {
public void RemoveStation(string stationId) {
public static ItemCatalog Catalog { get; } = BuildSeedCatalog();
public Recipe? FindRecipe(string id) {
public CommandResult Start(string recipeId, string? crafterId = null) {
public string CompleteAll(float gameHours) {
public string CraftingLine() {
public string CheckRecipe(string recipeId) {
public void TickDay(int day, float hours = 24f) {
public void TickHours(float hours) {
public CraftingSystemSave CaptureSave() {
public void RestoreSave(CraftingSystemSave save) {
```


# Appendix Q.576 — Additional Current Architecture Evidence: `src/Host/ResearchUnlockHostSession.cs`

### `src/Host/ResearchUnlockHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 159 lines / 6346 bytes.
- SHA-256: `078c97fa47840cd9e92603f166a648bb32ba1b2f93a011521694962653722214`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class ResearchUnlockSaveStore
public const string FileName = "research_unlock_save.json";
public const string SectionName = "research_unlock";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCapturePersisted(ResearchUnlockState state) => s_store.CaptureBare(state);
public static ResearchUnlockState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
public static bool TrySave(ResearchUnlockState state) => s_store.TrySave(state);
public static ResearchUnlockState? TryLoad() => s_store.TryLoad();
public sealed class ResearchUnlockHostSession : HostSessionBase
public ResearchUnlockBridge Bridge => _bridge;
public IResearchUnlockSink Sink => _sink;
public string LastEvent => _lastEvent;
public ResearchUnlockCensus Census => _bridge.GetCensus();
public IReadOnlyList<string> UnlockedCapabilities => _bridge.UnlockedCapabilities;
public IReadOnlyList<string> UnlockedRecipes => _bridge.UnlockedRecipes;
public IReadOnlyList<string> GrantedItems => _bridge.GrantedItems;
public IReadOnlyList<string> GrantedUnlockIds => _bridge.GrantedUnlockIds;
public static ResearchUnlockHostSession Create( string dataDir, Ashfall.Core.Inventory.Inventory? inventory = null, ResearchUnlockBridge? bridge = null) {
public void LoadCatalog(string dataDir) {
public void BindResearchSystem(ResearchSystem system) {
public int ProcessResearchCompletion(string nodeId) {
public int SynchronizeCompletedResearch(IEnumerable<string> completedNodeIds) {
public bool HasCapability(string capabilityId) => _bridge.HasCapability(capabilityId);
public bool HasRecipe(string recipeId) => _bridge.HasRecipe(recipeId);
public bool HasItem(string itemId) => _bridge.HasItem(itemId);
public bool HasUnlock(string unlockId) => _bridge.HasUnlock(unlockId);
public ResearchUnlockState CaptureState() => _bridge.CaptureState();
public void RestoreState(ResearchUnlockState state) {
```


# Appendix Q.577 — Additional Current Architecture Evidence: `src/Main.OriginMechanics.cs`

### `src/Main.OriginMechanics.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 90 lines / 4097 bytes.
- SHA-256: `aa325c8fd909fd86146c1288ec248506814886100fdac113b341a4738da01fb7`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public void ApplySurvivorOriginModifiers() {
public bool ApplyOriginModifierFor(string survivorId, out SurvivorOriginModifier modifier) {
public void ResetOriginMechanics() => _originMechanicsApplied = false;
```


# Appendix Q.578 — Additional Current Architecture Evidence: `src/UI/ResearchPanel.cs`

### `src/UI/ResearchPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 481 lines / 20137 bytes.
- SHA-256: `d9c2025599c7ea89fa4bb522c78838dac62e6ebd145d6761ce33213e2b15fbc7`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=5; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class ResearchPanel : Control
public event Action? OnClose;
public event Action<string>? OnResearchStarted;
public bool IsBound => _research != null || _host != null;
public int RenderedRowCount { get; private set; }
public void Bind(ResearchSystem? research) {
public void Bind(ResearchHostSession? host) {
public void Bind(ResearchSystem? research, ResearchHostSession? host) {
public void Bind(ResearchSystem? research, ResearchHostSession? host, ResearchUnlockHostSession? unlockHost) {
public void RefreshView() {
public override void _Ready() {
public void Open() {
public void Close() {
public void Unbind() {
public override void _ExitTree() {
public override void _UnhandledInput(InputEvent @event) {
```


# Appendix Q.579 — Additional Current Architecture Evidence: `src/Host/HostCli.Collectibles.cs`

### `src/Host/HostCli.Collectibles.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 495 lines / 30626 bytes.
- SHA-256: `3ac21d702fc5aa4eab294bb7503aef4f7638ad980a5e80e49de3d986ac7f942c`.
- Architecture signals: seeded references=5; save/restore symbols=10; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunCollectibleSelfTest(string dataDirectory) {
```


# Appendix Q.580 — Additional Current Architecture Evidence: `src/Main.EvolvingWorld.cs`

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


# Appendix Q.581 — Additional Current Architecture Evidence: `src/Main.Plans162_185.cs`

### `src/Main.Plans162_185.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 111 lines / 4820 bytes.
- SHA-256: `7a7e82255a44917e8fc1ec03617608e8749d992daa4fec6993c3edf578d98fa8`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public IReadOnlyList<MemoryRecord> BuildMemoryDecayProjection( string survivorId = "", int currentDay = -1) {
public IReadOnlyList<ArchiveEntry> BuildShelterArchiveProjection() {
```


# Appendix Q.582 — Additional Current Architecture Evidence: `src/Host/LibraryStudyHostSession.cs`

### `src/Host/LibraryStudyHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 129 lines / 5063 bytes.
- SHA-256: `aa2494ed7d7e7148b927868c88f9f2e15b32397ad2762bbbe329346f64d5249a`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class LibraryStudyHostSession
public LibraryStudySystem System { get; }
public string LastEvent { get; private set; } = string.Empty;
public void LoadCatalog(List<ManualDefinition> manuals) {
public void LoadCatalog(string dataDir) {
public ActionResult StartStudy(string manualId, string readerId) {
public void TickDay(int day) {
public override void Save() {
public static class LibraryStudySaveStore
public const string FileName = "library_study_save.json";
public const string SectionName = "library_study";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static bool TrySave(LibraryStudyState state) => s_store.TrySave(state);
public static LibraryStudyState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(LibraryStudyState state) => s_store.CapturePersisted(state);
public static string TryCaptureDirect(LibraryStudyState state) => s_store.CaptureBare(state);
public static LibraryStudyState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(LibraryStudyState state) => s_store.CaptureBare(state);
public static LibraryStudyState? TryRestore(string json) => s_store.RestoreBare(json);
```


# Appendix Q.583 — Additional Current Architecture Evidence: `src/Host/WaterSourcesHostSession.cs`

### `src/Host/WaterSourcesHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 281 lines / 11498 bytes.
- SHA-256: `a0414c1e0afedbe902f841cc3689986190b430e15b0aeec9a9015f5457777513`.
- Architecture signals: seeded references=0; save/restore symbols=6; typed event declarations=1; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WaterSourcesHostSession : IDisposable
public DeepWellHostSession DeepWell { get; }
public WaterCondenserHostSession Condenser { get; }
public PiezometerHostSession Piezometer { get; }
public event Action? StateChanged;
public string LastEvent { get; private set; } = string.Empty;
public DeepWellState DeepWellState => DeepWell.System.CaptureState();
public AtmosphericCondenserState CondenserState => Condenser.System.CaptureState();
public HydrogeologyNetworkState PiezometerState => Piezometer.CaptureSave();
public bool DeepWellPowerServed =>
public bool CondenserPowerServed =>
public bool HasDeepWellCapability =>
public bool HasCondenserCapability =>
public bool CanBuildDeepWell =>
public bool CanServiceDeepWell =>
public bool CanBuildCondenser =>
public bool CanReplaceCondenserMembrane =>
public bool CanConstructPiezometer =>
public int ItemCount(string itemId) => _inventory.CountById(itemId);
public WaterSourcesPersistedSnapshots CapturePersistedSnapshots() =>
public bool TryBuildDeepWell() {
public bool TrySetDeepWellEnabled(bool enabled) {
public bool TryServiceDeepWell() {
public bool TryBuildCondenser() {
public bool TrySetCondenserEnabled(bool enabled) {
public bool TryReplaceCondenserMembrane() {
public bool TryConstructPiezometer() {
public void Dispose() {
public sealed class WaterSourcesPersistedSnapshots
public string DeepWell { get; }
public string Condenser { get; }
public string Piezometer { get; }
```


# Appendix Q.584 — Additional Current Architecture Evidence: `src/Main.OrphanSealWave1.cs`

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


# Appendix Q.585 — Additional Current Architecture Evidence: `src/Main.PlayerSurfaces.cs`

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


# Appendix Q.586 — Additional Current Architecture Evidence: `src/Main.ShelterBatch3.cs`

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


# Appendix Q.587 — Additional Current Architecture Evidence: `src/Main.SurvivorFitness.cs`

### `src/Main.SurvivorFitness.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 478 lines / 23757 bytes.
- SHA-256: `254edb2c51b81907ddf13a2ab3459d521ca753e854ec4bcdb5d06a0f5d2ee64c`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public RoleFitnessVerdict? PreviewDutyFitness(string survivorId, string roleId) {
public DutyHourLedger EnsureDutyHourLedger() {
public WorkerProductivityContract EnsureWorkerProductivityContract() {
public void ApplyGriefNeedsModifiers() {
public void ApplyOverworkNeedsModifiers() {
```


# Appendix R.588 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/ApprenticeshipIntegrationTests.cs`

### `Ashfall.Core.Tests/ApprenticeshipIntegrationTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 61; SHA-256: `cea0bd92e9f14cf2fa33ac562f2f64c0f955149bcb275ad4fae0495a052290f8`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
StartPair_AndTick_AdvancesProgress
CancelPair_RemovesPair
SaveAndRestore_PreservesApprenticeshipState
```


# Appendix R.589 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Survivors/ShelterApprenticeshipAndWillPlan55Tests.cs`

### `Ashfall.Core.Tests/Survivors/ShelterApprenticeshipAndWillPlan55Tests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 196; SHA-256: `87a1a414fe312baa72fc2a790143347d6df6feb96ebca4e266f9b90bf92f8cd5`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CoOccupancy_GrantsFullXpWhenInSameRoom_ReducedWhenSeparated
ManualTranscription_ProducesManualItemOnCompletion
SurvivorWill_RegistersAndExecutesAtomically
SurvivorWill_FallsBackWhenPrimaryBeneficiaryDeceased
MentorDeath_AwardsLegacyTraitAndBonusXp
StatePreservation_RoundTripsWillsAndTasks
```


# Appendix R.590 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/ResearchSystemTests.cs`

### `Ashfall.Core.Tests/ResearchSystemTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 144; SHA-256: `6646f87554bed06d2c96fad8ead28199a4783dc293c5115af024df9cfba5e3ac`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
AuthoritativeCatalog_LoadsAll62Nodes
StartResearch_SetsActiveId
Tick_CompletesNodeAfterDaysBudget
StartResearch_PrerequisiteGated_Rejects
StartResearch_PrerequisiteGated_AcceptsAfterPrereqCompleted
StartResearch_AlreadyCompleted_Rejected
CaptureState_RoundTrip_PreservesState
Tick_IsDeterministicUnderSameSeed
Catalog_CoversAllCanonicalDisciplines
```


# Appendix R.591 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs`

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


# Appendix R.592 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Journeys/EndToEndPlayerJourneyTests.cs`

### `Ashfall.Core.Tests/Journeys/EndToEndPlayerJourneyTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 419; SHA-256: `174a76734becf16e420b841493898a32956db6a5c83306dcd0781746b2e545fb`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ShelterSurvivalJourney_Seed42_RunsSavesReloadsAndCompletes
ExpeditionCombatJourney_Seed1986_RunsSavesReloadsAndCompletes
FactionMedicalJourney_Seed2026_RunsSavesReloadsAndCompletes
JourneyRunner_EmitsStandardizedMachineReadableFailureContext_OnFailure
JourneyRunner_DeterministicSeed_ProducesIdenticalOutcome
```


# Appendix R.593 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Endgame/Plan19CohortContinuityTests.cs`

### `Ashfall.Core.Tests/Endgame/Plan19CohortContinuityTests.cs`

- Current test declarations: Fact=23, Theory=0, InlineData=0.
- File lines: 513; SHA-256: `82e2490716b2de26983b7f7257df9221730e97039ef8cbac50578391eed6f774`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ChildRations_ZeroChildren_RequiresZeroRations
ChildRations_SingleChild_StandardPolicy_CalculatesDeterministicCeiling
ChildRations_SingleChild_HalfPolicy_CalculatesDeterministicCeiling
ChildRations_NChildren_RoundsCorrectly
ChildRations_CustomTuningFraction_AppliesCorrectly
SchoolingEligibility_UnderAgeChild_NotEligible
SchoolingEligibility_SchoolAgeChild_Eligible
SchoolingEligibility_MaturedChild_NoLongerEligible
SchoolingEligibility_DeceasedChild_NotEligible
Apprenticeship_RejectsIneligibleChild_ViaDelegate
Apprenticeship_AcceptsEligibleSchoolAgeChild
Apprenticeship_CapacityCap_RejectsFourthPair
DutyRoster_UnderAgeUnmaturedChild_RejectedFromAssignment
DutyRoster_MaturedChild_AcceptedIntoAssignment
DutyRoster_NonCohortAdult_AcceptedIntoAssignment
ChildLoss_MarkChildLost_UpdatesState_AndFiresEvent
ChildLoss_DeceasedChildren_NotCountedInSurvivingChildren
Memorial_CircumstanceVariance_PreservesDistinctDeathAttributes
CampaignOutcome_ChildrenSurvived_FalseWhenNoChildren
CampaignOutcome_ChildrenSurvived_FalseWhenAllChildrenDeceased
CampaignOutcome_ChildrenSurvived_TrueWhenAtLeastOneSurvives
CohortSystem_SaveLoad_RoundTripsMortalityAndMaturation
ThreeYearBalanceSimulation_DeterministicScaling
```


# Appendix R.594 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Host/HostSessionStateSemanticsTests.cs`

### `Ashfall.Core.Tests/Host/HostSessionStateSemanticsTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 290; SHA-256: `d9342d57600436311d7f2d5a9942aa53453dc9102c9920414263273d2f533087`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
InitialSession_IsClean_StateVersionZero_SaveCountZero
SuccessfulAction_IncrementsStateVersion_MarksDirty_FiresEvents
RejectedAction_DoesNotIncrementStateVersion_DoesNotMarkDirty_DoesNotFireEvents
PresentationRefresh_DoesNotModifyDirty_Or_StateVersion
CoalescedMutations_FlushOnce_IncrementsSaveCountOnce
ClearDirty_LeavesVersionIntact_ClearsDirty
SaveStore_FailedOrNoOpActions_ProduceZeroWrites
DomainSession_Apprenticeship_RejectedPair_EmitsZeroChanges
DomainSession_AirlockSecurity_NoIncident_Resolve_EmitsZeroChanges
```


# Appendix R.595 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Research/Plan141ResearchUnlockBridgeIntegrationTests.cs`

### `Ashfall.Core.Tests/Research/Plan141ResearchUnlockBridgeIntegrationTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 152; SHA-256: `bbd941d932621bd5741e03892acfaa92b43667eb3448f5ae7699250b65e025a1`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ResearchUnlockCatalog_LoadsAuthoredUnlocks_Cleanly
ResearchCompletion_AwardsBreakthroughItem_AndUnlocksRecipe
ResearchSystem_BoundEvent_AutomaticallyTriggersDownstreamUnlocks
RetroactiveSynchronization_GrantsMissingUnlocks_ForOldSaves
ResearchUnlockBridge_CaptureRestore_PreservesAllGrantedState
```


# Appendix R.596 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/UI/PlayerSurfaceCoverageGateTests.cs`

### `Ashfall.Core.Tests/UI/PlayerSurfaceCoverageGateTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 253; SHA-256: `f3f775aa6b6017ad82794e7db2abc65f810a7a864e9a21d278bc1ec85aa1e322`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Manifest_GeneratesFromPanelRegistry_MatchesTotalCount
Prototypes_ExcludedFromManifestAndNotPlayerNavigable
Manifest_AllSurfaces_HaveReachablePlayerRoute
Manifest_AllSurfaces_HaveBindingTargetOrSetupDeps
Manifest_AllSurfaces_HaveDesignatedCloseBehavior
Manifest_TracksActionCoverageSeparatelyFromRendering
Manifest_SerializesValidJsonAndMarkdown
DeterministicFixtures_InstantiateExpandedDomainSystems_CleanState
```


# Appendix R.597 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/WildlifeTrappingIntegrationTests.cs`

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


# Appendix R.598 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/ProductionSliceTests.cs`

### `Ashfall.Core.Tests/ProductionSliceTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 389; SHA-256: `b2370bfc0dcb7115a5b48dd3539ff7575035c4821c8a00623b3c65b69bc253cb`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
AuthoritativeJson_RelicRecipes_LoadsSuccessfully
AuthoritativeJson_PharmaRecipes_LoadsSuccessfully
Workshop_InsufficientStock_BlocksRepairAtomically
Workshop_CancelJob_RefundsReservedComponents
Workshop_SkillMultiplier_AcceleratesRepair
Workshop_ResearchBlueprint_UnlocksAndCompletesResearchNode
PharmaLab_InsufficientInputs_BlocksTransactionAtomically
PharmaLab_CancelBatch_RefundsReagents
PharmaLab_DeterministicCompletion_DeliversMedicine
ProductionSlice_CraftingSaveStore_AggregateRoundTrip
```


# Appendix R.599 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Integration/Roadmap42Batch1IntegrationTests.cs`

### `Ashfall.Core.Tests/Integration/Roadmap42Batch1IntegrationTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 410; SHA-256: `58130dae9d890fd61d10c85f141e3fa59852d2b910e0cba09663d9b5a5ea7155`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Test1_ReferentialIntegrity_CrossCatalogContracts_AllResolve
Test2_FullDeterministicCampaignJourney_TouchesAllTenSystems_ReplaysIdentically
Test3_Batch1CatalogCountsAndLoaderClassifications_MatchAuthoritativeTruth
```


# Appendix R.600 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Progression/Plan33SkillCatalogExternalizationTests.cs`

### `Ashfall.Core.Tests/Progression/Plan33SkillCatalogExternalizationTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 227; SHA-256: `26dee51cfbc60c93729fbab92dbf3784a6504ea6ef3c32af4a1efd5b97289f1f`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsExactExpectedCount_FromAuthoritativeJson
AllSkills_HaveValidIdsPrefixAndNonNegativeThresholds
BaselineSkills_MatchExpectedValues
NewGroundedSkills_PresentAndConfigured
Loader_HandlesMissingOrCorruptedPathGracefully
Progression_ActionDrivenXp_UnlocksTierAndAppliesBonus
MilestoneGranting_WorksForNewGroundedSkills
SaveAndRestore_RoundTripsWithLoadedCatalog
```


# Appendix R.601 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Progression/Plan80LibraryManualsExpansionTests.cs`

### `Ashfall.Core.Tests/Progression/Plan80LibraryManualsExpansionTests.cs`

- Current test declarations: Fact=18, Theory=0, InlineData=0.
- File lines: 503; SHA-256: `34fd40ed5afa2b721b793af51bff093ddd6eccadcb1407ec78396de550206ba5`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsAtLeast24Manuals
Catalog_AnchorManualsPreserved
Catalog_AllIdsUniqueAndCanonicalPrefix
Catalog_AllDisplayNamesNonEmptyAndUnique
Catalog_CategoriesAreCanonicalAndCoverAllSixDomains
Catalog_NumericBoundsValid
Catalog_AllSkillXpGrantsAreValidDisciplineXpPairs
Catalog_AllResearchAndKnowledgeUnlocksResolve
Catalog_NoDuplicateReferencesWithinOneManual
Graph_PrerequisiteReferencesResolve
Graph_IsAcyclic
Graph_AllManualsReachableFromFoundations
Graph_HasIntermediateAndAdvancedDepth
Runtime_PrerequisiteEnforcement_WithRealChain
Runtime_CompletionGrantsSkillXpResearchAndKnowledge
Runtime_RewardsApplyExactlyOnce_RepeatStudyBlocked
Runtime_PartialStudyProgressRoundTripsThroughSave
Runtime_ThreeTierBranchCompletesDeterministically
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.
