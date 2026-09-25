# Plan 33 — Skill Catalog Authority, Progression and Future Catalog Onboarding

> **Rebuild status:** COMPLETE 161-SKILL EXTERNALIZED AUTHORITY — ONBOARDING AND REGRESSION MAINTENANCE
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

- The original externalization goal is complete: JSON definitions load and register into `SkillProgressionSystem`, the old hardcoded registration path is no longer a second authority, and current tests preserve baseline values plus new grounded skills.
- The live route is `skills.json` → `SkillCatalogLoader` → `SkillProgressionSystem.RegisterSkill` → action XP/tiers/dormancy/expert unlocks → `SkillMatrixPanel`/consumers → existing progression save.
- The rebase should focus on strict ID/field/reference validation, consumer discovery, migration from legacy catalogs and preventing a future hardcoded fallback from silently taking authority.

**Bounded outcome:** Retire the old “47 hardcoded skills / missing skills.json” premise. Current `skills.json` contains 161 skills, `SkillCatalogLoader` is the production authority, `RegisterDefaultSkills` is a compatibility no-op, and progression/save tests cover the externalized route. The remaining work is catalog onboarding and consumer-reference maintenance.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `skills.json` is present with 161 unique skills; `Plan33SkillCatalogExternalizationTests` asserts the current count, baseline values, new grounded skills, loader failure behavior, progression and save restore.
- `SkillDef.cs` documents JSON authority; `SkillCatalogLoader.cs` loads and registers; `SkillProgressionSystem.RegisterDefaultSkills` is a backward-compatible zero-op.
- Current skill tests cover action XP, dormancy, expert gating, milestone grants, epiphany and state round-trip.
- Skills are consumed by medical, crafting, trapping, research, certification and other systems through shared skill IDs; the catalog does not own those gameplay effects.

**Master-authority sections applied to this rebase:**

- Master authority Volume 28 verification cookbook: focused evidence before broad gates.
- Lane D save/state/compatibility guidance: owner DTOs, migration and restore proof.
- Lane E UI/UX/accessibility guidance: truthful projections and keyboard/controller lifecycle.
- Lane G testing guidance: smallest affected target, negative cases and deterministic replay.
- Anti-padding protocol: content exhaustion may end the plan before the character checkpoint.
- Volume 32 externalized authority and consumer guidance.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace 47→50 with a 161-row current census and baseline/consumer parity matrix.
- Define strict admission rules for IDs, display names, disciplines, thresholds and bonus values.
- Keep `RegisterDefaultSkills` non-authoritative and prevent fallback definitions from shadowing JSON.
- Require every gameplay consumer to resolve a current skill ID and remain save/determinism tested.

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
| JSON skill definitions and registration | SkillCatalogLoader | `Assets/Ashfall.Core/Survivors/SkillCatalogLoader.cs` | Sole production skill definition loader. |
| XP, tiers, dormancy and expert state | SkillProgressionSystem | `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` | Owns progression state and effects. |
| wire/domain shape | SkillDef | `Assets/Ashfall.Core/Survivors/SkillDef.cs` | Defines the contract consumed by Core and host. |
| skill checks and matrix projection | Skill consumers/UI | `src/UI/SkillMatrixPanel.cs; src/Main.ShelterSocial.cs` | Consume canonical skill state/IDs. |
| authority, progression and persistence proof | Skill focused tests | `Ashfall.Core.Tests/Progression/Plan33SkillCatalogExternalizationTests.cs; Ashfall.Core.Tests/SkillProgressionSystemTests.cs` | Executable current evidence. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Skill Catalog Authority, Progression and Future Catalog Onboarding
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ SkillCatalogLoader
│   JSON skill definitions and registration
│ SkillProgressionSystem
│   XP, tiers, dormancy and expert state
│ SkillDef
│   wire/domain shape
│ Skill consumers/UI
│   skill checks and matrix projection
│ Skill focused tests
│   authority, progression and persistence proof
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

1. **Preserve current state ownership.** SkillCatalogLoader owns JSON skill definitions and registration: Sole production skill definition loader.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| JSON skill definitions and registration | SkillCatalogLoader | `Assets/Ashfall.Core/Survivors/SkillCatalogLoader.cs` | Sole production skill definition loader. |
| XP, tiers, dormancy and expert state | SkillProgressionSystem | `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` | Owns progression state and effects. |
| wire/domain shape | SkillDef | `Assets/Ashfall.Core/Survivors/SkillDef.cs` | Defines the contract consumed by Core and host. |
| skill checks and matrix projection | Skill consumers/UI | `src/UI/SkillMatrixPanel.cs; src/Main.ShelterSocial.cs` | Consume canonical skill state/IDs. |
| authority, progression and persistence proof | Skill focused tests | `Ashfall.Core.Tests/Progression/Plan33SkillCatalogExternalizationTests.cs; Ashfall.Core.Tests/SkillProgressionSystemTests.cs` | Executable current evidence. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load/validate skills.json
2. register definitions into one progression system
3. resolve consumer skill checks by ID
4. record canonical action XP through owner
5. apply tier/dormancy/expert transitions
6. project skill matrix and gameplay effects
7. capture/restore progression state and re-register catalog

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Skill definitions are immutable catalog data; XP, active/dormant skill IDs and expert state are progression state.
- An unknown or malformed skill ID cannot create a shadow definition.
- Action XP and threshold tiers are deterministic under the current owner contract.
- Restore preserves progression state and re-registers the current catalog before applying state.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Every skill ID is unique, prefixed and non-empty with a valid discipline/threshold.
- A missing/corrupt catalog fails visibly or returns the documented empty result without a hardcoded shadow.
- The same action sequence and seed produce the same XP/tier state.
- Consumer effects never mutate skill definitions or duplicate progression state.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `skills.json` is the sole production skill authority.
- No new hardcoded skill list or parallel catalog is allowed.
- A new skill needs at least one current consumer, a grounded effect contract and a save/replay test.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use the current skill progression save state and existing host section.
- No new save section is justified by a catalog-only change.
- Legacy state restores against the current catalog with explicit unknown-ID handling.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- XP and epiphany use the existing seeded owner stream.
- Skill catalog order is stable and not hash dependent.
- Paired runs and save/restore runs match progression and consumer effects.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- RecordAction, XP gained, skill earned, dormancy and epiphany facts come from the current progression owner.
- Consumer effects subscribe to owner facts; they do not award XP independently.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Main.ShelterSocial.cs
- src/UI/SkillMatrixPanel.cs
- src/Main.CampaignServices.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Skill descriptions should be practical, grounded and fictional.
- A skill definition must not promise a magical outcome unsupported by current mechanics.
- The catalog should support tone and role variety without exposing internal IDs.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A hardcoded fallback reappears beside JSON. | SkillCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A consumer references an absent skill ID. | SkillProgressionSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | An unknown skill state silently drops XP or effects. | SkillDef | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | Save restore registers a different catalog order and changes outcomes. | Skill consumers/UI | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A skill is added with no gameplay consumer. | Skill focused tests | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Progression/Plan33SkillCatalogExternalizationTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Progression/SkillCatalogLoaderTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/SkillProgressionSystemTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — authority census | Read JSON, loader, domain shape and tests. | 161 current skills and no hardcoded shadow. | No production path until the owning implementation package is separately claimed. |
| 1 — admission matrix | Validate IDs, disciplines, thresholds and consumers. | No orphan skill or invalid field. | No production path until the owning implementation package is separately claimed. |
| 2 — progression/save proof | Verify action XP, dormancy, expert and restore. | Current owner remains deterministic. | No production path until the owning implementation package is separately claimed. |
| 3 — onboarding seal | Publish future skill-row checklist. | New rows cannot bypass loader/consumer/test gates. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/skills.json | READ ONLY; MODIFY only for proven catalog gap | 161-row authority |
| Assets/Ashfall.Core/Survivors/SkillCatalogLoader.cs | READ ONLY | Loader |
| Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs | READ ONLY | Progression owner |
| src/UI/SkillMatrixPanel.cs | READ ONLY | Current UI |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Restoring a hardcoded fallback. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Adding skills without consumer hooks. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Changing thresholds without parity tests. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Persisting derived catalog metadata. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new skill count for this rebase.
- No new progression system.
- No save-section change.
- No production edits in this rebase.

# 23. Rollback and Recovery

- Revert the plan file.
- Future catalog changes retain previous skills JSON and focused parity/progression tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 161 current skills and loader authority are documented.
- No hardcoded shadow is proposed.
- Progression/save/determinism contracts are explicit.
- Future onboarding gates are named.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace 47→50 with a 161-row current census and baseline/consumer parity matrix.
- Define strict admission rules for IDs, display names, disciplines, thresholds and bonus values.
- Keep `RegisterDefaultSkills` non-authoritative and prevent fallback definitions from shadowing JSON.
- Require every gameplay consumer to resolve a current skill ID and remain save/determinism tested.

## MUST NOT DO

- No new skill count for this rebase.
- No new progression system.
- No save-section change.
- No production edits in this rebase.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Progression/Plan33SkillCatalogExternalizationTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Progression/SkillCatalogLoaderTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/SkillProgressionSystemTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — authority census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: JSON skill definitions and registration → SkillCatalogLoader; XP, tiers, dormancy and expert state → SkillProgressionSystem; wire/domain shape → SkillDef; skill checks and matrix projection → Skill consumers/UI; authority, progression and persistence proof → Skill focused tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 33.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 33 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by SkillCatalogLoader or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Survivors/SkillCatalogLoader.cs`

### `Assets/Ashfall.Core/Survivors/SkillCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 131 lines / 4252 bytes.
- SHA-256: `92e0ce859aeed2c7d77c05a9a6624ce30684391272c1a4136df55311e5a56df7`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class SkillWireDto
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public string DisciplineId { get; set; } = string.Empty;
public float XpThreshold { get; set; } = 0f;
public float SkillBonus { get; set; } = 0f;
public bool IsExpertSkill { get; set; } = false;
public SkillDef ToDomain() {
public sealed class SkillCatalogContainer
public int SchemaVersion { get; set; } = 1;
public string CollectionId { get; set; } = "skills";
public List<SkillWireDto> Skills { get; set; } = new List<SkillWireDto>();
public static class SkillCatalogLoader
public const string DefaultFileName = "skills.json";
public static List<SkillDef> Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static int LoadAndRegister( SkillProgressionSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json) {
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


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Survivors/SkillDef.cs`

### `Assets/Ashfall.Core/Survivors/SkillDef.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 45 lines / 2036 bytes.
- SHA-256: `43618d9c40776856b3fdae5d3526d757811210cf67306d71405f933111ff31b5`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=2; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class SkillDef
public string id = string.Empty;
public string displayName = string.Empty;
public string description = string.Empty;
public string disciplineId = string.Empty;
public float xpThreshold = 0f;
public float skillBonus = 0f;
public bool isExpertSkill = false;
```


# Appendix B.05 — Current Code Architecture: `src/Main.ShelterSocial.cs`

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


# Appendix B.06 — Current Code Architecture: `src/UI/SkillMatrixPanel.cs`

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


# Appendix B.07 — Current Code Architecture: `src/Main.CampaignServices.cs`

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


# Appendix C.08 — Catalog Census: `Assets/StreamingAssets/Data/skills.json`

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


# Appendix D.10 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Progression/Plan33SkillCatalogExternalizationTests.cs`

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


# Appendix D.11 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Progression/SkillCatalogLoaderTests.cs`

### `Ashfall.Core.Tests/Progression/SkillCatalogLoaderTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 75; SHA-256: `80c37ab345ba12de4db3af8ca4fe81e944f2c9effef952d8a90107b9c72822ca`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Load_Loads110SkillsFromCatalog
Load_AllSkillsHaveValidIdAndDisplayName
LoadAndRegister_PopulatesSkillProgressionSystem
```


# Appendix D.12 — Existing Focused Test Inventory: `Ashfall.Core.Tests/SkillProgressionSystemTests.cs`

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


# Appendix E.13 — Supporting Code Evidence: `Assets/Ashfall.Core/ApprenticeshipSystem.cs`

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


# Appendix E.14 — Supporting Code Evidence: `Assets/Ashfall.Core/Research/ResearchSystem.cs`

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


# Appendix E.15 — Supporting Code Evidence: `Assets/Ashfall.Core/Survivors/SkillAtrophySystem.cs`

### `Assets/Ashfall.Core/Survivors/SkillAtrophySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 165 lines / 6970 bytes.
- SHA-256: `88a0d47e8b5e7fa68bf71d073d3110871343eca77cf2f9052b54099fa2e984ff`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=4; textual Godot mentions=0; textual Unity/JsonUtility mentions=3; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class SkillAtrophySystem
public const float AtrophyMoraleThreshold = 20f;
public const float AtrophyWindowDays = 14f;
public const float AtrophyMultiplier = 0.5f;
public event Action<string, string> OnSkillAtrophied;
public event Action<string> OnAtrophyDangerPassed;
public void Tick(float gameHours, IReadOnlyList<SkillActor> actors) {
public bool IsAtrophied(string actorId, string skillId) {
public IReadOnlyList<string> GetAtrophiedSkillIds(string actorId) {
public sealed class AtrophyState
public string survivorId = string.Empty;
public float consecutiveLowMoraleDays = 0f;
public List<string> atrophiedSkillIds = new List<string>();
public SkillAtrophySaveState CaptureState() {
public void RestoreState(SkillAtrophySaveState save) {
```


# Appendix E.16 — Supporting Code Evidence: `Assets/Ashfall.Core/Survivors/SurvivorRoleSystem.cs`

### `Assets/Ashfall.Core/Survivors/SurvivorRoleSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 320 lines / 13121 bytes.
- SHA-256: `c079614e9cf178eec9420ed510ba79d7cfab3de5ec70594289375fd0c8702eac`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=8; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class SurvivorRoleDef
public string role_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string category { get; set; } = "technical";
public Dictionary<string, int> required_skills { get; set; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
public string primary_bonus_type { get; set; } = string.Empty;
public float primary_bonus_value { get; set; } = 0.20f;
public string secondary_bonus_type { get; set; } = string.Empty;
public float secondary_bonus_value { get; set; } = 0.15f;
public string auto_action_type { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public sealed class SurvivorRolesCatalog
public int schema_version { get; set; } = 1;
public List<SurvivorRoleDef> roles { get; set; } = new List<SurvivorRoleDef>();
public sealed class SurvivorRoleAssignment
public string SurvivorId { get; set; } = string.Empty;
public string RoleId { get; set; } = string.Empty;
public int Level { get; set; } = 1; // 1: Novice, 2: Apprentice, 3: Journeyman, 4: Expert, 5: Master
public int ExperiencePoints { get; set; }
public int AssignedDay { get; set; } = 1;
public int TotalAutoActionsExecuted { get; set; }
public sealed class SurvivorRoleState
public int SchemaVersion { get; set; } = 1;
public int NextSequence { get; set; } = 1;
public int MaxSurvivorsPerRole { get; set; } = 2;
public List<SurvivorRoleAssignment> Assignments { get; set; } = new List<SurvivorRoleAssignment>();
public sealed class SurvivorRoleSystem
public event Action<SurvivorRoleAssignment>? OnRoleAssigned;
public event Action<string>? OnRoleUnassigned; // (survivorId)
public event Action<string, int>? OnRoleLeveledUp; // (survivorId, newLevel)
public event Action<string, string>? OnAutoActionExecuted; // (survivorId, actionType)
public int ActiveAssignmentCount => _state.Assignments.Count;
public int MaxSurvivorsPerRole => _state.MaxSurvivorsPerRole;
public void LoadCatalog(string json) {
public IReadOnlyCollection<SurvivorRoleDef> GetAllRoleDefs() => _roleDefs.Values;
public SurvivorRoleDef? GetRoleDef(string roleId) {
public bool CanAssignRole( string survivorId, string roleId, Dictionary<string, int>? survivorSkills, out string failureReason) {
public SurvivorRoleAssignment? AssignRole( string survivorId, string roleId, Dictionary<string, int>? skills = null, int day = 1, bool force = false)
public bool UnassignRole(string survivorId) {
public SurvivorRoleAssignment? GetRoleAssignment(string survivorId) {
public bool AddRoleXp(string survivorId, int xp) {
public float GetRoleBonus(string survivorId, string bonusType) {
public bool TriggerAutoAction(string survivorId, string actionType) {
public SurvivorRoleState CaptureState() {
public void RestoreState(SurvivorRoleState? saved) {
```


# Appendix E.17 — Supporting Code Evidence: `Assets/Ashfall.Core/Survivors/SkillCertificationSystem.cs`

### `Assets/Ashfall.Core/Survivors/SkillCertificationSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 499 lines / 20251 bytes.
- SHA-256: `8c2d5626ac88e58f3ab2432fdd1adc56f38c30aa2b2980f53ac9c61871aa69f3`.
- Architecture signals: seeded references=1; save/restore symbols=2; typed event declarations=7; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum SkillTier
public sealed class SkillTierDef
public SkillTier Tier { get; set; }
public string TierName { get; set; } = string.Empty;
public float LevelThreshold { get; set; }
public float BonusModifier { get; set; } = 1.0f;
public string Description { get; set; } = string.Empty;
public sealed class SkillCertificationDef
public string cert_id { get; set; } = string.Empty;
public string cert_name { get; set; } = string.Empty;
public string discipline { get; set; } = string.Empty;
public string required_tier { get; set; } = string.Empty;
public float required_level { get; set; }
public float exam_difficulty { get; set; } = 50.0f;
public List<string> required_experience { get; set; } = new List<string>();
public List<string> benefits { get; set; } = new List<string>();
public string description { get; set; } = string.Empty;
public sealed class SkillSpecializationDef
public string spec_id { get; set; } = string.Empty;
public string spec_name { get; set; } = string.Empty;
public List<string> required_cert_ids { get; set; } = new List<string>();
public List<string> unique_abilities { get; set; } = new List<string>();
public string description { get; set; } = string.Empty;
public sealed class SkillCertificationCatalog
public int schema_version { get; set; } = 1;
public List<SkillCertificationDef> certifications { get; set; } = new List<SkillCertificationDef>();
public List<SkillSpecializationDef> specializations { get; set; } = new List<SkillSpecializationDef>();
public sealed class SurvivorCertificationRecord
public string CertId { get; set; } = string.Empty;
public int EarnedDay { get; set; }
public string CertifyingSurvivorId { get; set; } = string.Empty;
public float ExamScore { get; set; }
public sealed class SurvivorSpecializationRecord
public string SpecId { get; set; } = string.Empty;
public int EarnedDay { get; set; }
public sealed class SurvivorCertificationProfile
public string SurvivorId { get; set; } = string.Empty;
public List<SurvivorCertificationRecord> Certifications { get; set; } = new List<SurvivorCertificationRecord>();
public List<SurvivorSpecializationRecord> Specializations { get; set; } = new List<SurvivorSpecializationRecord>();
public int LastExamDay { get; set; }
public sealed class SkillCertificationState
public int SchemaVersion { get; set; } = 1;
public int TotalCertificationsAwarded { get; set; }
public int TotalExamsConduct { get; set; }
public List<SurvivorCertificationProfile> Profiles { get; set; } = new List<SurvivorCertificationProfile>();
public struct SkillCertificationCensus
public readonly int CertifiedSurvivorsCount;
public readonly int TotalCertificationsAwarded;
public readonly int TotalSpecializationsAwarded;
public readonly int TotalExamsConducted;
public sealed class SkillCertificationSystem
public event Action<string, string>? OnCertificationEarned;     // survivorId, certId
public event Action<string, string>? OnSpecializationEarned;    // survivorId, specId
public event Action<string, string, string>? OnExamFailed;       // survivorId, certId, reason
public IReadOnlyList<SkillCertificationDef> Certifications => _certifications;
public IReadOnlyList<SkillSpecializationDef> Specializations => _specializations;
public IReadOnlyList<SurvivorCertificationProfile> Profiles => _state.Profiles;
public IReadOnlyList<SkillCertificationDef> GetAllCertifications() => _certifications;
public IReadOnlyList<SkillSpecializationDef> GetAllSpecializations() => _specializations;
public static SkillTier GetTierForLevel(float skillLevel) {
public static SkillTierDef GetTierDef(SkillTier tier) {
public void LoadCatalog(string json) {
public SurvivorCertificationProfile GetOrCreateProfile(string survivorId) {
public SurvivorCertificationProfile? GetProfile(string survivorId) =>
public bool HasCertification(string survivorId, string certId) {
public bool HasSpecialization(string survivorId, string specId) {
public bool CanAttemptExam(string survivorId, string certId, float candidateSkillLevel, int currentDay, out string reason) {
public IReadOnlyList<string> GetUnlockedBenefits(string survivorId) {
public SkillCertificationCensus GetCensus() {
public SkillCertificationState CaptureState() {
public void RestoreState(SkillCertificationState state) {
```


# Appendix G.18 — Supporting Regression Evidence: `Ashfall.Core.Tests/ApprenticeshipSystemTests.cs`

### `Ashfall.Core.Tests/ApprenticeshipSystemTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 117; SHA-256: `55a9799180ecb6143ccc5816301be6351b0ffdfe8b0f502a7d60aba03e7f56aa`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.


# Appendix G.19 — Supporting Regression Evidence: `Ashfall.Core.Tests/ResearchSystemTests.cs`

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


# Appendix G.20 — Supporting Regression Evidence: `Ashfall.Core.Tests/Plan12DCrossSystemContinuityTests.cs`

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


# Appendix G.21 — Supporting Regression Evidence: `Ashfall.Core.Tests/Integration/Roadmap42Batch1IntegrationTests.cs`

### `Ashfall.Core.Tests/Integration/Roadmap42Batch1IntegrationTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 410; SHA-256: `58130dae9d890fd61d10c85f141e3fa59852d2b910e0cba09663d9b5a5ea7155`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Test1_ReferentialIntegrity_CrossCatalogContracts_AllResolve
Test2_FullDeterministicCampaignJourney_TouchesAllTenSystems_ReplaysIdentically
Test3_Batch1CatalogCountsAndLoaderClassifications_MatchAuthoritativeTruth
```


# Appendix H.22 — Supporting Authority Document: `docs/CURRENT_AUTHORITY.md`

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
| JSON skill definitions and registration | SkillCatalogLoader | XP, tiers, dormancy and expert state | SkillProgressionSystem | Owner emits/reads a typed fact; no mirror state. |
| JSON skill definitions and registration | SkillCatalogLoader | wire/domain shape | SkillDef | Owner emits/reads a typed fact; no mirror state. |
| JSON skill definitions and registration | SkillCatalogLoader | skill checks and matrix projection | Skill consumers/UI | Owner emits/reads a typed fact; no mirror state. |
| JSON skill definitions and registration | SkillCatalogLoader | authority, progression and persistence proof | Skill focused tests | Owner emits/reads a typed fact; no mirror state. |
| XP, tiers, dormancy and expert state | SkillProgressionSystem | JSON skill definitions and registration | SkillCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| XP, tiers, dormancy and expert state | SkillProgressionSystem | wire/domain shape | SkillDef | Owner emits/reads a typed fact; no mirror state. |
| XP, tiers, dormancy and expert state | SkillProgressionSystem | skill checks and matrix projection | Skill consumers/UI | Owner emits/reads a typed fact; no mirror state. |
| XP, tiers, dormancy and expert state | SkillProgressionSystem | authority, progression and persistence proof | Skill focused tests | Owner emits/reads a typed fact; no mirror state. |
| wire/domain shape | SkillDef | JSON skill definitions and registration | SkillCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| wire/domain shape | SkillDef | XP, tiers, dormancy and expert state | SkillProgressionSystem | Owner emits/reads a typed fact; no mirror state. |
| wire/domain shape | SkillDef | skill checks and matrix projection | Skill consumers/UI | Owner emits/reads a typed fact; no mirror state. |
| wire/domain shape | SkillDef | authority, progression and persistence proof | Skill focused tests | Owner emits/reads a typed fact; no mirror state. |
| skill checks and matrix projection | Skill consumers/UI | JSON skill definitions and registration | SkillCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| skill checks and matrix projection | Skill consumers/UI | XP, tiers, dormancy and expert state | SkillProgressionSystem | Owner emits/reads a typed fact; no mirror state. |
| skill checks and matrix projection | Skill consumers/UI | wire/domain shape | SkillDef | Owner emits/reads a typed fact; no mirror state. |
| skill checks and matrix projection | Skill consumers/UI | authority, progression and persistence proof | Skill focused tests | Owner emits/reads a typed fact; no mirror state. |
| authority, progression and persistence proof | Skill focused tests | JSON skill definitions and registration | SkillCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| authority, progression and persistence proof | Skill focused tests | XP, tiers, dormancy and expert state | SkillProgressionSystem | Owner emits/reads a typed fact; no mirror state. |
| authority, progression and persistence proof | Skill focused tests | wire/domain shape | SkillDef | Owner emits/reads a typed fact; no mirror state. |
| authority, progression and persistence proof | Skill focused tests | skill checks and matrix projection | Skill consumers/UI | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace 47→50 with a 161-row current census and baseline/consumer parity matrix. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Define strict admission rules for IDs, display names, disciplines, thresholds and bonus values. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Keep `RegisterDefaultSkills` non-authoritative and prevent fallback definitions from shadowing JSON. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Require every gameplay consumer to resolve a current skill ID and remain save/determinism tested. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.554 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs`

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


# Appendix Q.555 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

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


# Appendix Q.556 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Archaeology/ArchaeologySystem.cs`

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


# Appendix Q.557 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Collectibles/CollectibleEffectDispatcher.cs`

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


# Appendix Q.558 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/LibraryStudySystem.cs`

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


# Appendix Q.559 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Research/ResearchUnlockBridge.cs`

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


# Appendix Q.560 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Survivors/LatentExpertAwakeningSystem.cs`

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


# Appendix Q.561 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Survivors/SkillProgressionState.cs`

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


# Appendix Q.562 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/AutopsySystem.cs`

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


# Appendix Q.563 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentDeepChainGate.cs`

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


# Appendix Q.564 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Research/ResearchState.cs`

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


# Appendix Q.565 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs`

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


# Appendix Q.566 — Additional Current Architecture Evidence: `src/Host/ResearchHostSession.cs`

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


# Appendix Q.567 — Additional Current Architecture Evidence: `src/Host/HostCli.ResearchUnlock.cs`

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


# Appendix Q.568 — Additional Current Architecture Evidence: `src/Host/SkillCertificationHostSession.cs`

### `src/Host/SkillCertificationHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 87 lines / 3564 bytes.
- SHA-256: `91d4d42df088f0f962620668d98b4a9a24476233e1a701125ba35b5046c9618c`.
- Architecture signals: seeded references=1; save/restore symbols=4; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class SkillCertificationSaveStore
public const string SectionName = "skill_certifications";
public const string FileName = "skill_certifications_save.json";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string? TryCapturePersisted(SkillCertificationState state) => s_store.CaptureBare(state);
public static SkillCertificationState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
public static bool TrySave(SkillCertificationState state) => s_store.TrySave(state);
public static SkillCertificationState? TryLoad() => s_store.TryLoad();
public sealed class SkillCertificationHostSession : HostSessionBase
public SkillCertificationSystem System { get; }
public static SkillCertificationHostSession Create(string dataDir, SkillCertificationState? restoredState = null) {
public bool CanAttemptExam(string survivorId, string certId, float candidateSkill, int currentDay, out string reason) {
public bool HasCertification(string survivorId, string certId) => System.HasCertification(survivorId, certId);
public bool HasSpecialization(string survivorId, string specId) => System.HasSpecialization(survivorId, specId);
public IReadOnlyList<string> GetUnlockedBenefits(string survivorId) => System.GetUnlockedBenefits(survivorId);
public SkillTier GetTierForLevel(float skillLevel) => SkillCertificationSystem.GetTierForLevel(skillLevel);
public SurvivorCertificationProfile? GetProfile(string survivorId) => System.GetProfile(survivorId);
public SkillCertificationCensus GetCensus() => System.GetCensus();
public SkillCertificationState CaptureState() => System.CaptureState();
public void RestoreState(SkillCertificationState state) {
```


# Appendix Q.569 — Additional Current Architecture Evidence: `src/Host/CraftingHostSession.cs`

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


# Appendix Q.570 — Additional Current Architecture Evidence: `src/Host/ResearchUnlockHostSession.cs`

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


# Appendix Q.571 — Additional Current Architecture Evidence: `src/Main.ExpandedShelterSystems.cs`

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


# Appendix Q.572 — Additional Current Architecture Evidence: `src/UI/ResearchPanel.cs`

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


# Appendix Q.573 — Additional Current Architecture Evidence: `src/Host/ApprenticeshipHostSession.cs`

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


# Appendix Q.574 — Additional Current Architecture Evidence: `src/Host/HostCli.Collectibles.cs`

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


# Appendix Q.575 — Additional Current Architecture Evidence: `src/Host/LibraryStudyHostSession.cs`

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


# Appendix Q.576 — Additional Current Architecture Evidence: `src/Host/WaterSourcesHostSession.cs`

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


# Appendix Q.577 — Additional Current Architecture Evidence: `src/Main.Plans162_185.cs`

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


# Appendix R.578 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Research/Plan141ResearchUnlockBridgeIntegrationTests.cs`

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


# Appendix R.579 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/WildlifeTrappingIntegrationTests.cs`

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


# Appendix R.580 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Survivors/Plan195SurvivorRoleIntegrationTests.cs`

### `Ashfall.Core.Tests/Survivors/Plan195SurvivorRoleIntegrationTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 163; SHA-256: `bb948e56fcc3ea7a584e275aa15df3e009ad4f198a48bb9d24a812f06aa63449`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
LoadCatalog_LoadsAllEightRoles
CanAssignRole_EnforcesSkillPrerequisitesAndCaps
AssignRole_GrantsRoleAndCalculatesBonuses
AddRoleXp_LevelsUpRoleAndEnhancesBonuses
TriggerAutoAction_ExecutesAndTracksPerformance
SaveRestoreState_PreservesAssignmentsAndProgression
```


# Appendix R.581 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Endgame/Plan19CohortContinuityTests.cs`

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


# Appendix R.582 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/ProductionSliceTests.cs`

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


# Appendix R.583 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Library/LibraryStudyContractTests.cs`

### `Ashfall.Core.Tests/Library/LibraryStudyContractTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 402; SHA-256: `2ef4e6cbd958e0abbb3321372e52bba0605d746219680042aac9c4981f514279`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
B2_001_ManualStudy_CallsUnlockManual_NeverCompleteResearch
B2_002_And_B2_004_JournalEvidenceAddedAndDedupedWithStableProvenance
B2_003_DuplicateResearchUnlock_IsIdempotent
B2_005_And_B2_006_SkillRaisesStudyRateMonotonically_WithinStrictBounds
B2_007_InvalidZeroOrNegativeHours_Rejected
B2_008_And_B2_009_BidirectionalAvailabilityReservation_DutyRoster
B2_010_To_B2_014_AuthoritativeCatalogIntegrity_24Manuals_6Disciplines
B2_016_And_B2_017_SaveRestore_PreservesJobsAndUnknownCompletedIds
```


# Appendix R.584 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Progression/Plan80LibraryManualsExpansionTests.cs`

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


# Appendix R.585 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Survivors/Plan180SkillCertificationTests.cs`

### `Ashfall.Core.Tests/Survivors/Plan180SkillCertificationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 151; SHA-256: `15b470bf891383c3ac1550dabb8c5477701fbed4775a9c0d50289980f2e823bd`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
LoadCatalog_LoadsAllCertificationsAndSpecializations
CanAttemptExam_EnforcesPrerequisitesAndCooldown
ConductExam_GrantsCertificationAndSpecializationPerks
StateRoundTrip_CapturesAndRestoresAllProfilesAndCertifications
```


# Appendix R.586 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/ApprenticeshipIntegrationTests.cs`

### `Ashfall.Core.Tests/ApprenticeshipIntegrationTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 61; SHA-256: `cea0bd92e9f14cf2fa33ac562f2f64c0f955149bcb275ad4fae0495a052290f8`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
StartPair_AndTick_AdvancesProgress
CancelPair_RemovesPair
SaveAndRestore_PreservesApprenticeshipState
```


# Appendix R.587 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/CampaignContinuityFlagship54_57Tests.cs`

### `Ashfall.Core.Tests/CampaignContinuityFlagship54_57Tests.cs`

- Current test declarations: Fact=2, Theory=0, InlineData=0.
- File lines: 204; SHA-256: `afeaa1b6d204749d02296af2fb3b06d75309f18d7957f67148f903e93d8403f1`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Continuity_30DayDeterministicReplay_ProducesIdenticalOutputs
Continuity_Day15SaveRestoreSplit_MatchesContinuousRun
```


# Appendix R.588 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Collectibles/CollectibleSaveMigrationTests.cs`

### `Ashfall.Core.Tests/Collectibles/CollectibleSaveMigrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 215; SHA-256: `14d582081994ea61924ac8bf5497896ef59eb2cf7148bb8cd5b7dcd6ebda033b`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
LegacyCaseA_ManualDiscovered_NodeReconciledOnce_Idempotent
LegacyCaseB_MapDiscovered_LocationReconciledAsSurveyed_RoutesUntouched
LegacyCaseC_VinylDiscovered_OwnershipReconciled_NeverMorale
FullMigration_CaptureReload_NoRepeatEffect
```


# Appendix R.589 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Host/HostSessionStateSemanticsTests.cs`

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


# Appendix R.590 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/LibraryStudySystemTests.cs`

### `Ashfall.Core.Tests/LibraryStudySystemTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 141; SHA-256: `ebebdeefe40e1e8593ae004cb917fb0833d54b898a34bb570338eba74651167f`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.


# Appendix R.591 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Phase1SharedContractsTests.cs`

### `Ashfall.Core.Tests/Phase1SharedContractsTests.cs`

- Current test declarations: Fact=13, Theory=0, InlineData=0.
- File lines: 203; SHA-256: `a0134ce801cc4f299327692ef6cff285d397ff650f818795feca56a04c45dcaa`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
HasCapability_MirrorsManualUnlock_Exactly
HasCapability_NullOrEmpty_IsFalse
HasCapability_UnknownId_IsFalse
WaterPreview_SufficientWater_CanCommit_AndMutatesNothing
WaterPreview_InsufficientWater_ReportsReason_AndMutatesNothing
WaterPreview_InvalidAmount_IsBlocked
WaterCommit_ConsumesExactAmount_ExactlyOnce
WaterCommit_InsufficientWater_MutatesNothing
WaterCommit_PreviewThenCommit_IsConsistent
WaterCommit_QualityIsEnforced_PerPool
OnTickSummary_FiresExactlyOnce_PerTickDay_WithReturnedPayload
OnTickSummary_NoSubscribers_DoesNotThrow
OnTickSummary_Deterministic_ForSameSeedAndState
```


# Appendix R.592 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Plan166ResearchSalvageTests.cs`

### `Ashfall.Core.Tests/Plan166ResearchSalvageTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 172; SHA-256: `9480605a781fc37bec36c51703550049c9be0b3d9e437de6f32daadf55d3ff54`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
SuccessfulRecovery_UsesCanonicalResearchWalletAndBlueprintProgress
CatastrophicFailure_IsSeededAndConsumesSourceExactlyOnce
ResearchFacilityQualityAndSkillRaisePreviewChance
ResearchFacilityQualityIsPersistedIntoOutcomeResolution
TechSalvageCatalogLoadsAndValidatesAuthoritativeData
ResearchPointsAndBlueprintProgressSurviveRoundTripWithoutAliasing
```


# Appendix R.593 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Progression/Plan80_61LibraryTradeIntegrationTests.cs`

### `Ashfall.Core.Tests/Progression/Plan80_61LibraryTradeIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 231; SHA-256: `f67e3c01031b535672055663c72a3eb2d411dd56038c31c16dc7a5598b09cc8a`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plan80_61_TradeManual_AcquisitionAndShelterStudy_RoundTrip
Plan80_61_Prerequisites_And_PowerGating_Enforced
Plan80_61_TradeFairness_And_KnowledgeValuation
Plan80_61_LibraryStudy_SaveRestore_Parity
```


# Appendix R.594 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Research/ResearchQueueEligibilityTests.cs`

### `Ashfall.Core.Tests/Research/ResearchQueueEligibilityTests.cs`

- Current test declarations: Fact=13, Theory=0, InlineData=0.
- File lines: 251; SHA-256: `b54ddd15d74003702b7badc58829b16312f813b48adb7d57d1b8dee4176ccebb`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
EmptyCatalog_ReturnsUnknownNode
UnknownNode_ReturnsUnknownCode
MissingPrerequisites_ReportsLockedAndListsPrereqs
RootNode_WithoutPrerequisites_IsEligible
AlreadyActive_ReturnsAlreadyActiveCode
AnotherResearchActive_BlocksOtherNodes
AlreadyCompleted_ReturnsAlreadyCompletedCode
PrerequisiteSatisfied_UnlocksDependentNode
SameDay_RepeatedTick_DoesNotAdvanceProgress
SkippedDayTick_DerivesProgressFromDayDelta
SaveRestore_MidProgress_PreservesExactDaysRemaining
GetDependents_HandlesDiamondGraphAndEmpty
Projections_AvailableLockedCompleted_ArePartitionedAndSorted
```


# Appendix R.595 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/ResearchCatalogParityTests.cs`

### `Ashfall.Core.Tests/ResearchCatalogParityTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 137; SHA-256: `b6bbd61cd6ab2d4aae55903a8baa3de9cf231384ebf9ae6d02a96eec3bef84bd`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
LegacyFixture_Has37Defs_Original15First
Catalog_ContainsEveryLegacyDef_WithValueParity
Catalog_PreservesOriginal15RegistrationOrder
Behavior_EligibilityAndCompletionMatchLegacyBaseline
```


# Appendix R.596 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Survivors/Plan185SkillDormancyTickTests.cs`

### `Ashfall.Core.Tests/Survivors/Plan185SkillDormancyTickTests.cs`

- Current test declarations: Fact=2, Theory=0, InlineData=0.
- File lines: 84; SHA-256: `3a91d3b9c6dab3e58e178272eee375603448a5d046d070a927d42ef9fb144737`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Host_shaped_daily_tick_dormants_an_unused_skill_and_practice_reactivates_it
Dead_survivors_are_excluded_from_the_host_actor_list
```


# Appendix R.597 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Survivors/ShelterApprenticeshipAndWillPlan55Tests.cs`

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


# Appendix R.598 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Archaeology/ArchaeologySystemTests.cs`

### `Ashfall.Core.Tests/Archaeology/ArchaeologySystemTests.cs`

- Current test declarations: Fact=14, Theory=0, InlineData=0.
- File lines: 379; SHA-256: `493b9b485a5b9bf2848aabcd22bf38b30dbd1b22cd97c83181852f11192c0b60`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
SurveyRuins_ReservesArchiveUntilItIsRecovered
SurveyRuins_CatalogSelectionAndZoneIdentity_AreDeterministic
InvalidCatalogReload_ClearsStaleAuthority
RestoreSequence_DoesNotReusePersistedSiteIds
ProgressExcavation_InvalidHours_DoNotRewindOrPoisonProgress
ProgressDecryption_InvalidWork_IsBlockedWithoutMutation
Restore_MalformedState_FiltersAndNormalizes
StateQueriesAndEvents_AreDetachedSnapshots
SellArchiveToBroker_PaymentFailure_DoesNotConsumeArchive
CaptureRestore_AreDeepCopies
ArchaeologySystem_SurveyRuins_DiscoversExcavationSite
ArchaeologySystem_ProgressExcavation_CompletesAndRecoversArchive
ArchaeologySystem_ProgressDecryption_RequiresPower_UnlocksLoreAndResearch
ArchaeologySystem_SellArchiveToBroker_GrantsScrap_CannotSellTwice
```


# Appendix R.599 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Collectibles/CollectibleCampaignSmokeTests.cs`

### `Ashfall.Core.Tests/Collectibles/CollectibleCampaignSmokeTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 529; SHA-256: `c47c5a1157190d73e0f256e720f645d23ab3d9435d60fc1a6bc81eb28237f53b`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CollectibleCampaignSmoke_CatalogValuesWithinBounds
CollectibleCampaignSmoke_Seed42_CompletesFullLifecycle
CollectibleCampaignSmoke_UniqueCollectiblesAppearAtMostOnce
CollectibleCampaignSmoke_ThreeRunsProduceIdenticalTrace
CollectibleCampaignSmoke_ThreeRunsProduceIdenticalFinalHash
```


# Appendix R.600 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Collectibles/CollectibleResearchIntegrationTests.cs`

### `Ashfall.Core.Tests/Collectibles/CollectibleResearchIntegrationTests.cs`

- Current test declarations: Fact=8, Theory=0, InlineData=0.
- File lines: 186; SHA-256: `a60923d56b72a1b1a02aeb1347b5581e4a97ef59a5231af4ddf01b0d11314d38`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
DieselServiceManual_RevealsKnowledge_DoesNotComplete
RadioRepairGuide_RevealsKnowledge_DoesNotComplete
WaterTreatmentHandbook_RevealsKnowledge_DoesNotComplete
AirFilterManual_RevealsKnowledge_DoesNotComplete
DosimeterGuide_RevealsKnowledge_DoesNotComplete
ManualReacquisition_Idempotent_NodeStillNotCompleted
ManualAcquisition_SaveRestore_NodeStillRevealed_ReacquireNoRepeat
ManualReveal_RaisesResearchStateEvent_PanelObservable
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.


# Appendix S — Quality Assurance Pass Record: Plan 33

This record is part of the planning artifact, not a fresh runtime test result.

## Pass 1 — content and premise accuracy
- Content pass replaced the 47-hardcoded premise with the current 161-skill JSON authority.
- The historical baseline is separated from the current source/data/test authority.
- Current row counts and owner boundaries are stated without using count as a quality proxy.

## Pass 2 — integration architecture
- Integration pass traced loader → progression → consumers → skill matrix → save.
- Core, data, host, UI, save, event and test seams are named with current paths.
- The plan does not authorize a parallel save section, catalog, manager or host cache.

## Final precision and reaccuracy pass
- Precision pass keeps the compatibility RegisterDefaultSkills method non-authoritative.
- Every embedded current-file hash, focused runner command and master-authority reference is rechecked.
- Any proposed future seam is labeled as requiring a separate claim and premise verification.
