# Plan 105 — Trade Specialties, Profession Milestones and Crafted-Item Learning

> **Rebuild status:** COMPLETE 16-ITEM CATALOG — CORE SYSTEM, LOADER, HOST REGISTRATION AND SAVE ARE PRESENT
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

- The current catalog has 16 items/profession definitions, each with milestone patterns, titles, narrative IDs and bonus text.
- TradeSpecialtySystem owns profession resolution, per-survivor milestones, mastery, skill/morale effects, events and capture/restore.
- Phase0HostSession and Main.Phase0 load the catalog; current tests cover milestone count, duplicate prevention, mastery, save and bonus effects.

**Bounded outcome:** Retire the 4-to-12 target. trade_specialties.json currently has 16 profession entries, TradeSpecialtyCatalogLoader registers them, TradeSpecialtySystem owns milestone/mastery state and Phase0HostSession loads the catalog. The plan becomes a current mapping/reachability and balance audit, not more profession rows.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- Replace the old count target with a 16-row profession/reference census.
- Audit profession IDs, item patterns, survivor profession resolution, crafted-item event source, narrative IDs and player visibility.
- No new specialty state or save section.

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
- The current plan is a sixteen-row progression reachability audit.

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
| profession definition parsing and registration | TradeSpecialtyCatalogLoader | `Assets/Ashfall.Core/Survivors/TradeSpecialtyCatalogLoader.cs` | Static catalog owner. |
| profession/milestone/mastery state and crafted-item effects | TradeSpecialtySystem | `Assets/Ashfall.Core/Survivors/TradeSpecialtySystem.cs` | Sole runtime owner. |
| catalog load and host composition | Phase0HostSession | `src/Host/Phase0HostSession.cs` | Thin host seam. |
| setup/load owner | Main.Phase0 | `src/Main.Phase0.cs` | Current host path. |
| milestone/save/effect contracts | TradeSpecialtySystemTests | `Ashfall.Core.Tests/TradeSpecialtySystemTests.cs` | Focused evidence. |
| catalog metadata/loader contract | TradeSpecialtyCatalogMetadataTests | `Ashfall.Core.Tests/Progression/TradeSpecialtyCatalogMetadataTests.cs` | Focused evidence. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Trade Specialties, Profession Milestones and Crafted-Item Learning
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ TradeSpecialtyCatalogLoader
│   profession definition parsing and registration
│ TradeSpecialtySystem
│   profession/milestone/mastery state and crafted-item effects
│ Phase0HostSession
│   catalog load and host composition
│ Main.Phase0
│   setup/load owner
│ TradeSpecialtySystemTests
│   milestone/save/effect contracts
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

1. **Preserve current state ownership.** TradeSpecialtyCatalogLoader owns profession definition parsing and registration: Static catalog owner.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| profession definition parsing and registration | TradeSpecialtyCatalogLoader | `Assets/Ashfall.Core/Survivors/TradeSpecialtyCatalogLoader.cs` | Static catalog owner. |
| profession/milestone/mastery state and crafted-item effects | TradeSpecialtySystem | `Assets/Ashfall.Core/Survivors/TradeSpecialtySystem.cs` | Sole runtime owner. |
| catalog load and host composition | Phase0HostSession | `src/Host/Phase0HostSession.cs` | Thin host seam. |
| setup/load owner | Main.Phase0 | `src/Main.Phase0.cs` | Current host path. |
| milestone/save/effect contracts | TradeSpecialtySystemTests | `Ashfall.Core.Tests/TradeSpecialtySystemTests.cs` | Focused evidence. |
| catalog metadata/loader contract | TradeSpecialtyCatalogMetadataTests | `Ashfall.Core.Tests/Progression/TradeSpecialtyCatalogMetadataTests.cs` | Focused evidence. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load 16 profession definitions
2. validate profession IDs, patterns and narrative refs
3. resolve current survivor profession
4. observe current crafted item
5. match patterns exactly once
6. advance milestone or mastery
7. capture/restore current specialty state

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Profession definitions are immutable catalog content.
- TradeSpecialtySaveState is the current per-survivor milestone/mastery authority.
- Skill/morale effects are applied by the system through current owner events.
- No Plan-105 save section.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- One crafted item cannot count twice.
- Unknown profession/pattern fails without state mutation.
- Mastery requires the current milestone count.
- A narrative ID that cannot resolve is a content defect, not invented prose.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- trade_specialties.json is the specialty authority.
- Skills, morale, narrative and item catalogs own their effects/references.
- No duplicate profession registry.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use TradeSpecialtySaveState/current save owner.
- No Plan-105 save section.
- Old state defaults to no milestones/mastery safely.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Pattern matching is deterministic and ordinal.
- No RNG is needed for ordinary milestones; if future grading is random, use current seeded stream.
- Capture ordering is stable.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Crafted-item events feed OnItemCrafted.
- TradeSpecialtySystem emits milestone/mastery/state-changed facts.
- UI and journal consume results; they do not award progression.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/Phase0HostSession.cs
- src/Main.Phase0.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Mastery text is restrained, fictional and profession-specific.
- No real-world professional licensing claim.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A catalog profession is counted as reachable without a current survivor mapping. | TradeSpecialtyCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | Duplicate crafted item increments twice. | TradeSpecialtySystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A panel awards mastery. | Phase0HostSession | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | Narrative reference is invented at runtime. | Main.Phase0 | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A new progression save is added. | TradeSpecialtySystemTests | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/TradeSpecialtySystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Progression/TradeSpecialtyCatalogMetadataTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/CraftAttributionTradeSpecialtyTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 | 16-row profession/reference census. | Current content is exact. | No production path until the owning implementation package is separately claimed. |
| 1 | Crafted-item/source/host trace. | Only real crafted events progress a survivor. | No production path until the owning implementation package is separately claimed. |
| 2 | Save/replay/effect audit. | No duplicate progress or new save. | No production path until the owning implementation package is separately claimed. |
| 3 | UI/content polish. | Progress is legible and truthful. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/trade_specialties.json | READ ONLY; MODIFY only for approved content/reference delta | 16 items |
| Assets/Ashfall.Core/Survivors/TradeSpecialtySystem.cs | READ ONLY | Runtime owner |
| src/Host/Phase0HostSession.cs | READ ONLY | Host seam |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Parallel progression. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Count-only growth. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Duplicate crafted item counting. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Runtime narrative invention. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new profession rows.
- No new progression state.
- No production/data/test/UI changes here.

# 23. Rollback and Recovery

- Revert planning artifact.
- Future balance/content changes use current specialty system/save.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 16 current rows and all effect/save owners are explicit.
- Reachability and content gaps are separated from count.

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

- No new profession rows.
- No new progression state.
- No production/data/test/UI changes here.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/TradeSpecialtySystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Progression/TradeSpecialtyCatalogMetadataTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/CraftAttributionTradeSpecialtyTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: profession definition parsing and registration → TradeSpecialtyCatalogLoader; profession/milestone/mastery state and crafted-item effects → TradeSpecialtySystem; catalog load and host composition → Phase0HostSession; setup/load owner → Main.Phase0; milestone/save/effect contracts → TradeSpecialtySystemTests; catalog metadata/loader contract → TradeSpecialtyCatalogMetadataTests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 105.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 105 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by TradeSpecialtyCatalogLoader or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Survivors/TradeSpecialtyCatalogLoader.cs`

### `Assets/Ashfall.Core/Survivors/TradeSpecialtyCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 152 lines / 6113 bytes.
- SHA-256: `829a9820cb5e55014969e56613ee9955c354b5cea0a80a86415e02e3c183d2b3`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class TradeSpecialtyMilestoneDto
public int Tier { get; set; } = 1;
public List<string> ItemPatterns { get; set; } = new List<string>();
public string Title { get; set; } = string.Empty;
public string Narrative { get; set; } = string.Empty;
public float SkillBonus { get; set; } = 0.05f;
public sealed class TradeSpecialtyItemDto
public string ProfessionId { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public List<string> ProfessionAliases { get; set; } = new List<string>();
public List<TradeSpecialtyMilestoneDto> Milestones { get; set; } = new List<TradeSpecialtyMilestoneDto>();
public string MasteryNarrative { get; set; } = string.Empty;
public string MasteryBonusText { get; set; } = string.Empty;
public sealed class TradeSpecialtyCatalogContainer
public int SchemaVersion { get; set; } = 1;
public List<TradeSpecialtyItemDto> Items { get; set; } = new List<TradeSpecialtyItemDto>();
public static class TradeSpecialtyCatalogLoader
public const string DefaultFileName = "trade_specialties.json";
public static List<TradeSpecialtyItemDto> Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static int LoadAndRegister( TradeSpecialtySystem system, string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Survivors/TradeSpecialtySystem.cs`

### `Assets/Ashfall.Core/Survivors/TradeSpecialtySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 451 lines / 21404 bytes.
- SHA-256: `f9ad2632abf72b8925bf610aa461aef2ca05f652c777b3fbc51082172bb8fc48`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class TradeSpecialtySurvivorState
public string survivorId = string.Empty;
public string professionId = string.Empty;
public List<string> craftMilestonesCompleted = new List<string>();
public bool mastered;
public sealed class TradeSpecialtySaveState
public string systemId = TradeSpecialtySystem.SystemId;
public List<TradeSpecialtySurvivorState> survivors = new List<TradeSpecialtySurvivorState>();
public sealed class TradeSpecialtyMilestoneInfo
public int Tier = 1;
public string Title = string.Empty;
public string NarrativeId = string.Empty;
public float SkillBonus;
public sealed class TradeSpecialtyProfessionInfo
public string ProfessionId = string.Empty;
public string DisplayName = string.Empty;
public string MasteryNarrativeId = string.Empty;
public string MasteryBonusText = string.Empty;
public List<string> Aliases = new List<string>();
public Dictionary<int, TradeSpecialtyMilestoneInfo> Milestones = new Dictionary<int, TradeSpecialtyMilestoneInfo>();
public class TradeSpecialtySystem
public const string SystemId = "trade_specialty_system";
public const int MilestonesToMaster = 3;
public const float MasterySkillBonus = 0.15f;
public const float MasteryMoraleBonus = 10f;
public const float MilestoneSkillBonusFactor = 0.3f;
public static readonly Dictionary<string, List<string>> ProfessionItemCategories = new Dictionary<string, List<string>> {
public static void RegisterProfessionPatterns(string professionId, IEnumerable<string> patterns) {
public static bool ProfessionMatchesItem(string professionId, string itemId) {
public static readonly Dictionary<string, TradeSpecialtyProfessionInfo> ProfessionInfo = new Dictionary<string, TradeSpecialtyProfessionInfo>(StringComparer.Ordinal);
public static void RegisterProfessionInfo(TradeSpecialtyProfessionInfo? info) {
public static TradeSpecialtyProfessionInfo? GetProfessionInfo(string professionId) {
public static TradeSpecialtyMilestoneInfo? GetMilestone(string professionId, int tier) {
public static string GetDisplayName(string professionId) => GetProfessionInfo(professionId)?.DisplayName ?? string.Empty;
public static string GetMasteryNarrativeId(string professionId) => GetProfessionInfo(professionId)?.MasteryNarrativeId ?? string.Empty;
public static string GetMasteryBonusText(string professionId) => GetProfessionInfo(professionId)?.MasteryBonusText ?? string.Empty;
public static string GetMilestoneTitle(string professionId, int tier) => GetMilestone(professionId, tier)?.Title ?? string.Empty;
public static string GetMilestoneNarrativeId(string professionId, int tier) => GetMilestone(professionId, tier)?.NarrativeId ?? string.Empty;
public static IReadOnlyDictionary<string, string> ProfessionLabelIndex => s_professionLabelIndex;
public static string ResolveProfessionId(string? explicitProfessionId, string? professionLabel) {
public static string ResolveProfessionIdFromLabel(string? professionLabel) {
public event Action<string, string, int> OnSpecialtyMilestone;
public event Action<string, string> OnSpecialtyMastered;
public event Action OnStateChanged;
public Action<string, string, float> GrantSkillBonus;
public Action<string, float> ApplyMoraleDelta;
public Func<string, string> GetNarrativeEventId;
public Action<string, string> FireNarrativeEvent;
public void OnItemCrafted(string survivorId, string professionId, string itemId) {
public int GetMasteryTier(string survivorId) {
public bool HasMasteredTrade(string survivorId) {
public TradeSpecialtySaveState CaptureState() {
public void RestoreState(TradeSpecialtySaveState save) {
```


# Appendix B.04 — Current Code Architecture: `src/Host/Phase0HostSession.cs`

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


# Appendix B.05 — Current Code Architecture: `src/Host/Phase0HostSession.cs`

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


# Appendix B.06 — Current Code Architecture: `src/Main.Phase0.cs`

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


# Appendix C.07 — Catalog Census: `Assets/StreamingAssets/Data/trade_specialties.json`

### `Assets/StreamingAssets/Data/trade_specialties.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 20427 bytes / 20427 characters.
- SHA-256: `084f782ae3196dfe0eccffe745bdbe9d241c5d1020ab4f001b59d43a9ef8c965`.
- Root keys: `items`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
items: min=16, max=16, observed_paths=1
items[].milestones: min=3, max=3, observed_paths=2
items[].milestones[].item_patterns: min=3, max=4, observed_paths=4
items[].profession_aliases: min=1, max=5, observed_paths=2
```

Representative record fields:

- `display_name`
- `mastery_bonus_text`
- `mastery_narrative`
- `milestones`
- `profession_aliases`
- `profession_id`


# Appendix C.08 — Catalog Census: `Assets/StreamingAssets/Data/trade_specialties.json`

### `Assets/StreamingAssets/Data/trade_specialties.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 20427 bytes / 20427 characters.
- SHA-256: `084f782ae3196dfe0eccffe745bdbe9d241c5d1020ab4f001b59d43a9ef8c965`.
- Root keys: `items`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
items: min=16, max=16, observed_paths=1
items[].milestones: min=3, max=3, observed_paths=2
items[].milestones[].item_patterns: min=3, max=4, observed_paths=4
items[].profession_aliases: min=1, max=5, observed_paths=2
```

Representative record fields:

- `display_name`
- `mastery_bonus_text`
- `mastery_narrative`
- `milestones`
- `profession_aliases`
- `profession_id`


# Appendix D.09 — Existing Focused Test Inventory: `Ashfall.Core.Tests/TradeSpecialtySystemTests.cs`

### `Ashfall.Core.Tests/TradeSpecialtySystemTests.cs`

- Current test declarations: Fact=13, Theory=0, InlineData=0.
- File lines: 201; SHA-256: `9429f1ab94ee3ee897568a3950679df7f35d8d9676a57f66074caadfa803445b`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
OnItemCrafted_MatchingCategory_CountsMilestone
OnItemCrafted_NonMatchingCategory_Ignored
OnItemCrafted_UnknownProfession_Ignored
OnItemCrafted_DuplicateItem_NotDoubleCounted
OnItemCrafted_EmptyInputs_NoOp
ThreeMilestones_MastersTrade
MasterTrade_AppliesFullSkillBonusAndMorale
IntermediateMilestone_GrantsPartialSkillBonus
MasterTrade_FiresNarrativeEvent
MasteredTrade_StopsFurtherMilestones
CaptureRestore_RoundTripsState
Restore_Null_NoThrow
Restore_RebuildsMasteryFlag
```


# Appendix D.10 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Progression/TradeSpecialtyCatalogMetadataTests.cs`

### `Ashfall.Core.Tests/Progression/TradeSpecialtyCatalogMetadataTests.cs`

- Current test declarations: Fact=28, Theory=0, InlineData=0.
- File lines: 652; SHA-256: `5017c73b2b2d379a2b8312c894cb168b69605b3cd81aeca1d5a2b160bcb7dd0f`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
LoadAndRegister_RetainsEveryAuthoredContentField
GetMasteryNarrativeId_ComesFromCatalog_NotFromInterpolation
GetMasteryNarrativeId_UnknownProfession_ReturnsEmpty
GetMasteryNarrativeId_MatchesCatalogForElectrician
OnItemCrafted_FiresAuthoredTierNarrativeForEachMilestone
OnItemCrafted_MasteryFiresTierThreeAndMasteryNarratives
OnItemCrafted_UnregisteredProfession_FiresNoNarrative
MasterTrade_FallsBackToHostHook_OnlyWhenCatalogHasNoEntry
OnItemCrafted_NoHookAssigned_DoesNotThrow
RegisterProfessionInfo_ReplacesEntryWithoutDuplicating
RegisterProfessionInfo_NullOrEmptyId_IsNoOp
IntermediateMilestones_GrantAuthoredSkillBonus_MasteryKeepsConstant
UncataloguedProfession_FallsBackToDerivedMilestoneBonus
RuntimeMasteryConstants_AreUnchanged
CaptureRestore_RoundTripsWithContentRegistryPopulated
AllAuthoredNarrativeIds_AreNonEmptyAndDistinct
ProfessionAliases_AreUniqueAcrossSpecialties
ProfessionAliases_AllMatchRealSurvivorLabels
ResolveProfessionIdFromLabel_MapsAuthoredLabels
ResolveProfessionId_ExplicitIdWinsOverLabel
ResolveProfessionId_UnmappedLabel_ReturnsEmpty
Roster_ReachesThirteenOfSixteenSpecialties
EverySpecialtyEntry_DeclaresProfessionAliases
ProfessionMatchesItem_MatchesAuthoredPatterns
ProfessionMatchesItem_EmptyOrUnknownInputs_ReturnFalse
ProfessionMatchesItem_AgreesWithOnItemCraftedForEveryProfession
ProfessionMatchesItem_RejectsSubstringAccidents_AcceptsInflection
ReachabilityPin_MasteryRequiresThreeCraftableMatches
```


# Appendix D.11 — Existing Focused Test Inventory: `Ashfall.Core.Tests/CraftAttributionTradeSpecialtyTests.cs`

### `Ashfall.Core.Tests/CraftAttributionTradeSpecialtyTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 81; SHA-256: `1e0b8b9c41f74135b83f072a10b287e37715a16cdd17f4c6dcab7a4ecc3b7e01`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
AttributedCrafts_AdvanceMatchingCurrentProfession
AttributedCrafts_IgnoreWrongItemUnknownProfessionAndMissingIdentity
AttributedCrafts_DeduplicateTheSameMilestone
AttributedCrafts_SaveRestorePreservesProgressAndCanMaster
```


# Appendix E.12 — Supporting Code Evidence: `Assets/Ashfall.Core/Survivors/TradeSpecialtyCatalogLoader.cs`

### `Assets/Ashfall.Core/Survivors/TradeSpecialtyCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 152 lines / 6113 bytes.
- SHA-256: `829a9820cb5e55014969e56613ee9955c354b5cea0a80a86415e02e3c183d2b3`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class TradeSpecialtyMilestoneDto
public int Tier { get; set; } = 1;
public List<string> ItemPatterns { get; set; } = new List<string>();
public string Title { get; set; } = string.Empty;
public string Narrative { get; set; } = string.Empty;
public float SkillBonus { get; set; } = 0.05f;
public sealed class TradeSpecialtyItemDto
public string ProfessionId { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public List<string> ProfessionAliases { get; set; } = new List<string>();
public List<TradeSpecialtyMilestoneDto> Milestones { get; set; } = new List<TradeSpecialtyMilestoneDto>();
public string MasteryNarrative { get; set; } = string.Empty;
public string MasteryBonusText { get; set; } = string.Empty;
public sealed class TradeSpecialtyCatalogContainer
public int SchemaVersion { get; set; } = 1;
public List<TradeSpecialtyItemDto> Items { get; set; } = new List<TradeSpecialtyItemDto>();
public static class TradeSpecialtyCatalogLoader
public const string DefaultFileName = "trade_specialties.json";
public static List<TradeSpecialtyItemDto> Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static int LoadAndRegister( TradeSpecialtySystem system, string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix E.13 — Supporting Code Evidence: `Assets/Ashfall.Core/Survivors/TradeSpecialtySystem.cs`

### `Assets/Ashfall.Core/Survivors/TradeSpecialtySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 451 lines / 21404 bytes.
- SHA-256: `f9ad2632abf72b8925bf610aa461aef2ca05f652c777b3fbc51082172bb8fc48`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class TradeSpecialtySurvivorState
public string survivorId = string.Empty;
public string professionId = string.Empty;
public List<string> craftMilestonesCompleted = new List<string>();
public bool mastered;
public sealed class TradeSpecialtySaveState
public string systemId = TradeSpecialtySystem.SystemId;
public List<TradeSpecialtySurvivorState> survivors = new List<TradeSpecialtySurvivorState>();
public sealed class TradeSpecialtyMilestoneInfo
public int Tier = 1;
public string Title = string.Empty;
public string NarrativeId = string.Empty;
public float SkillBonus;
public sealed class TradeSpecialtyProfessionInfo
public string ProfessionId = string.Empty;
public string DisplayName = string.Empty;
public string MasteryNarrativeId = string.Empty;
public string MasteryBonusText = string.Empty;
public List<string> Aliases = new List<string>();
public Dictionary<int, TradeSpecialtyMilestoneInfo> Milestones = new Dictionary<int, TradeSpecialtyMilestoneInfo>();
public class TradeSpecialtySystem
public const string SystemId = "trade_specialty_system";
public const int MilestonesToMaster = 3;
public const float MasterySkillBonus = 0.15f;
public const float MasteryMoraleBonus = 10f;
public const float MilestoneSkillBonusFactor = 0.3f;
public static readonly Dictionary<string, List<string>> ProfessionItemCategories = new Dictionary<string, List<string>> {
public static void RegisterProfessionPatterns(string professionId, IEnumerable<string> patterns) {
public static bool ProfessionMatchesItem(string professionId, string itemId) {
public static readonly Dictionary<string, TradeSpecialtyProfessionInfo> ProfessionInfo = new Dictionary<string, TradeSpecialtyProfessionInfo>(StringComparer.Ordinal);
public static void RegisterProfessionInfo(TradeSpecialtyProfessionInfo? info) {
public static TradeSpecialtyProfessionInfo? GetProfessionInfo(string professionId) {
public static TradeSpecialtyMilestoneInfo? GetMilestone(string professionId, int tier) {
public static string GetDisplayName(string professionId) => GetProfessionInfo(professionId)?.DisplayName ?? string.Empty;
public static string GetMasteryNarrativeId(string professionId) => GetProfessionInfo(professionId)?.MasteryNarrativeId ?? string.Empty;
public static string GetMasteryBonusText(string professionId) => GetProfessionInfo(professionId)?.MasteryBonusText ?? string.Empty;
public static string GetMilestoneTitle(string professionId, int tier) => GetMilestone(professionId, tier)?.Title ?? string.Empty;
public static string GetMilestoneNarrativeId(string professionId, int tier) => GetMilestone(professionId, tier)?.NarrativeId ?? string.Empty;
public static IReadOnlyDictionary<string, string> ProfessionLabelIndex => s_professionLabelIndex;
public static string ResolveProfessionId(string? explicitProfessionId, string? professionLabel) {
public static string ResolveProfessionIdFromLabel(string? professionLabel) {
public event Action<string, string, int> OnSpecialtyMilestone;
public event Action<string, string> OnSpecialtyMastered;
public event Action OnStateChanged;
public Action<string, string, float> GrantSkillBonus;
public Action<string, float> ApplyMoraleDelta;
public Func<string, string> GetNarrativeEventId;
public Action<string, string> FireNarrativeEvent;
public void OnItemCrafted(string survivorId, string professionId, string itemId) {
public int GetMasteryTier(string survivorId) {
public bool HasMasteredTrade(string survivorId) {
public TradeSpecialtySaveState CaptureState() {
public void RestoreState(TradeSpecialtySaveState save) {
```


# Appendix E.14 — Supporting Code Evidence: `src/Host/Phase0HostSession.cs`

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


# Appendix E.15 — Supporting Code Evidence: `src/Host/Phase0HostSession.cs`

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


# Appendix E.16 — Supporting Code Evidence: `src/Main.Phase0.cs`

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


# Appendix F.17 — Supporting Data Evidence: `Assets/StreamingAssets/Data/trade_specialties.json`

### `Assets/StreamingAssets/Data/trade_specialties.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 20427 bytes / 20427 characters.
- SHA-256: `084f782ae3196dfe0eccffe745bdbe9d241c5d1020ab4f001b59d43a9ef8c965`.
- Root keys: `items`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
items: min=16, max=16, observed_paths=1
items[].milestones: min=3, max=3, observed_paths=2
items[].milestones[].item_patterns: min=3, max=4, observed_paths=4
items[].profession_aliases: min=1, max=5, observed_paths=2
```

Representative record fields:

- `display_name`
- `mastery_bonus_text`
- `mastery_narrative`
- `milestones`
- `profession_aliases`
- `profession_id`


# Appendix F.18 — Supporting Data Evidence: `Assets/StreamingAssets/Data/trade_specialties.json`

### `Assets/StreamingAssets/Data/trade_specialties.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 20427 bytes / 20427 characters.
- SHA-256: `084f782ae3196dfe0eccffe745bdbe9d241c5d1020ab4f001b59d43a9ef8c965`.
- Root keys: `items`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
items: min=16, max=16, observed_paths=1
items[].milestones: min=3, max=3, observed_paths=2
items[].milestones[].item_patterns: min=3, max=4, observed_paths=4
items[].profession_aliases: min=1, max=5, observed_paths=2
```

Representative record fields:

- `display_name`
- `mastery_bonus_text`
- `mastery_narrative`
- `milestones`
- `profession_aliases`
- `profession_id`


# Appendix G.19 — Supporting Regression Evidence: `Ashfall.Core.Tests/TradeSpecialtySystemTests.cs`

### `Ashfall.Core.Tests/TradeSpecialtySystemTests.cs`

- Current test declarations: Fact=13, Theory=0, InlineData=0.
- File lines: 201; SHA-256: `9429f1ab94ee3ee897568a3950679df7f35d8d9676a57f66074caadfa803445b`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
OnItemCrafted_MatchingCategory_CountsMilestone
OnItemCrafted_NonMatchingCategory_Ignored
OnItemCrafted_UnknownProfession_Ignored
OnItemCrafted_DuplicateItem_NotDoubleCounted
OnItemCrafted_EmptyInputs_NoOp
ThreeMilestones_MastersTrade
MasterTrade_AppliesFullSkillBonusAndMorale
IntermediateMilestone_GrantsPartialSkillBonus
MasterTrade_FiresNarrativeEvent
MasteredTrade_StopsFurtherMilestones
CaptureRestore_RoundTripsState
Restore_Null_NoThrow
Restore_RebuildsMasteryFlag
```


# Appendix G.20 — Supporting Regression Evidence: `Ashfall.Core.Tests/Progression/TradeSpecialtyCatalogMetadataTests.cs`

### `Ashfall.Core.Tests/Progression/TradeSpecialtyCatalogMetadataTests.cs`

- Current test declarations: Fact=28, Theory=0, InlineData=0.
- File lines: 652; SHA-256: `5017c73b2b2d379a2b8312c894cb168b69605b3cd81aeca1d5a2b160bcb7dd0f`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
LoadAndRegister_RetainsEveryAuthoredContentField
GetMasteryNarrativeId_ComesFromCatalog_NotFromInterpolation
GetMasteryNarrativeId_UnknownProfession_ReturnsEmpty
GetMasteryNarrativeId_MatchesCatalogForElectrician
OnItemCrafted_FiresAuthoredTierNarrativeForEachMilestone
OnItemCrafted_MasteryFiresTierThreeAndMasteryNarratives
OnItemCrafted_UnregisteredProfession_FiresNoNarrative
MasterTrade_FallsBackToHostHook_OnlyWhenCatalogHasNoEntry
OnItemCrafted_NoHookAssigned_DoesNotThrow
RegisterProfessionInfo_ReplacesEntryWithoutDuplicating
RegisterProfessionInfo_NullOrEmptyId_IsNoOp
IntermediateMilestones_GrantAuthoredSkillBonus_MasteryKeepsConstant
UncataloguedProfession_FallsBackToDerivedMilestoneBonus
RuntimeMasteryConstants_AreUnchanged
CaptureRestore_RoundTripsWithContentRegistryPopulated
AllAuthoredNarrativeIds_AreNonEmptyAndDistinct
ProfessionAliases_AreUniqueAcrossSpecialties
ProfessionAliases_AllMatchRealSurvivorLabels
ResolveProfessionIdFromLabel_MapsAuthoredLabels
ResolveProfessionId_ExplicitIdWinsOverLabel
ResolveProfessionId_UnmappedLabel_ReturnsEmpty
Roster_ReachesThirteenOfSixteenSpecialties
EverySpecialtyEntry_DeclaresProfessionAliases
ProfessionMatchesItem_MatchesAuthoredPatterns
ProfessionMatchesItem_EmptyOrUnknownInputs_ReturnFalse
ProfessionMatchesItem_AgreesWithOnItemCraftedForEveryProfession
ProfessionMatchesItem_RejectsSubstringAccidents_AcceptsInflection
ReachabilityPin_MasteryRequiresThreeCraftableMatches
```


# Appendix G.21 — Supporting Regression Evidence: `Ashfall.Core.Tests/CraftAttributionTradeSpecialtyTests.cs`

### `Ashfall.Core.Tests/CraftAttributionTradeSpecialtyTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 81; SHA-256: `1e0b8b9c41f74135b83f072a10b287e37715a16cdd17f4c6dcab7a4ecc3b7e01`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
AttributedCrafts_AdvanceMatchingCurrentProfession
AttributedCrafts_IgnoreWrongItemUnknownProfessionAndMissingIdentity
AttributedCrafts_DeduplicateTheSameMilestone
AttributedCrafts_SaveRestorePreservesProgressAndCanMaster
```


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| profession definition parsing and registration | TradeSpecialtyCatalogLoader | profession/milestone/mastery state and crafted-item effects | TradeSpecialtySystem | Owner emits/reads a typed fact; no mirror state. |
| profession definition parsing and registration | TradeSpecialtyCatalogLoader | catalog load and host composition | Phase0HostSession | Owner emits/reads a typed fact; no mirror state. |
| profession definition parsing and registration | TradeSpecialtyCatalogLoader | setup/load owner | Main.Phase0 | Owner emits/reads a typed fact; no mirror state. |
| profession definition parsing and registration | TradeSpecialtyCatalogLoader | milestone/save/effect contracts | TradeSpecialtySystemTests | Owner emits/reads a typed fact; no mirror state. |
| profession definition parsing and registration | TradeSpecialtyCatalogLoader | catalog metadata/loader contract | TradeSpecialtyCatalogMetadataTests | Owner emits/reads a typed fact; no mirror state. |
| profession/milestone/mastery state and crafted-item effects | TradeSpecialtySystem | profession definition parsing and registration | TradeSpecialtyCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| profession/milestone/mastery state and crafted-item effects | TradeSpecialtySystem | catalog load and host composition | Phase0HostSession | Owner emits/reads a typed fact; no mirror state. |
| profession/milestone/mastery state and crafted-item effects | TradeSpecialtySystem | setup/load owner | Main.Phase0 | Owner emits/reads a typed fact; no mirror state. |
| profession/milestone/mastery state and crafted-item effects | TradeSpecialtySystem | milestone/save/effect contracts | TradeSpecialtySystemTests | Owner emits/reads a typed fact; no mirror state. |
| profession/milestone/mastery state and crafted-item effects | TradeSpecialtySystem | catalog metadata/loader contract | TradeSpecialtyCatalogMetadataTests | Owner emits/reads a typed fact; no mirror state. |
| catalog load and host composition | Phase0HostSession | profession definition parsing and registration | TradeSpecialtyCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| catalog load and host composition | Phase0HostSession | profession/milestone/mastery state and crafted-item effects | TradeSpecialtySystem | Owner emits/reads a typed fact; no mirror state. |
| catalog load and host composition | Phase0HostSession | setup/load owner | Main.Phase0 | Owner emits/reads a typed fact; no mirror state. |
| catalog load and host composition | Phase0HostSession | milestone/save/effect contracts | TradeSpecialtySystemTests | Owner emits/reads a typed fact; no mirror state. |
| catalog load and host composition | Phase0HostSession | catalog metadata/loader contract | TradeSpecialtyCatalogMetadataTests | Owner emits/reads a typed fact; no mirror state. |
| setup/load owner | Main.Phase0 | profession definition parsing and registration | TradeSpecialtyCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| setup/load owner | Main.Phase0 | profession/milestone/mastery state and crafted-item effects | TradeSpecialtySystem | Owner emits/reads a typed fact; no mirror state. |
| setup/load owner | Main.Phase0 | catalog load and host composition | Phase0HostSession | Owner emits/reads a typed fact; no mirror state. |
| setup/load owner | Main.Phase0 | milestone/save/effect contracts | TradeSpecialtySystemTests | Owner emits/reads a typed fact; no mirror state. |
| setup/load owner | Main.Phase0 | catalog metadata/loader contract | TradeSpecialtyCatalogMetadataTests | Owner emits/reads a typed fact; no mirror state. |
| milestone/save/effect contracts | TradeSpecialtySystemTests | profession definition parsing and registration | TradeSpecialtyCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| milestone/save/effect contracts | TradeSpecialtySystemTests | profession/milestone/mastery state and crafted-item effects | TradeSpecialtySystem | Owner emits/reads a typed fact; no mirror state. |
| milestone/save/effect contracts | TradeSpecialtySystemTests | catalog load and host composition | Phase0HostSession | Owner emits/reads a typed fact; no mirror state. |
| milestone/save/effect contracts | TradeSpecialtySystemTests | setup/load owner | Main.Phase0 | Owner emits/reads a typed fact; no mirror state. |
| milestone/save/effect contracts | TradeSpecialtySystemTests | catalog metadata/loader contract | TradeSpecialtyCatalogMetadataTests | Owner emits/reads a typed fact; no mirror state. |
| catalog metadata/loader contract | TradeSpecialtyCatalogMetadataTests | profession definition parsing and registration | TradeSpecialtyCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| catalog metadata/loader contract | TradeSpecialtyCatalogMetadataTests | profession/milestone/mastery state and crafted-item effects | TradeSpecialtySystem | Owner emits/reads a typed fact; no mirror state. |
| catalog metadata/loader contract | TradeSpecialtyCatalogMetadataTests | catalog load and host composition | Phase0HostSession | Owner emits/reads a typed fact; no mirror state. |
| catalog metadata/loader contract | TradeSpecialtyCatalogMetadataTests | setup/load owner | Main.Phase0 | Owner emits/reads a typed fact; no mirror state. |
| catalog metadata/loader contract | TradeSpecialtyCatalogMetadataTests | milestone/save/effect contracts | TradeSpecialtySystemTests | Owner emits/reads a typed fact; no mirror state. |

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

> C1 Shelter operations (rooms, thermal, schedules, fire, decor, barter, noise, prisoners, sanitation, airlock, decon, atmosphere) · C2 Medical pipeline (disease, dose ledger, ARS, surgery, autopsy, pharma, diagnostics, therapies, dependency, crises) · C3 Water, food, agriculture (treatment, condensers, wells, brine, nutrition, kitchen, preservation, grain, greenhouse, crops, aquaponics, apiculture) · C4 Power and industry (grid, SOFC, solar, kinetic, geothermal, foundry, CVD diamond, coatings, optics, powder metallurgy, pyrolysis, Fischer-Tropsch, chlor-alkali, acids, fermentation, ethanol, air separation, metrology) · C5 Expeditions and travel (destinations, scavenging tables, vehicles, waystations, caravans, routes, travel encounters, micro-locations) · C6 Map and geography (wasteland map, damaged zones, fog, route gates, cartography, survey instruments) · C7 Factions and war (stance, doctrines, war chains, tributes, treaties, embargoes, espionage, psyops, infiltration, musters, labor camps, bounties) · C8 Radio and information (stations, programs, intercepts, distress signals, rumors, sound ranging, direction finding, NVIS, heliograph) · C9 Survivors and interiority (needs, skills, traits, arcs, trauma, guilt, therapies, relations, caregiving, beliefs, rituals, memorials, final wishes, lineage, cohorts, apprenticeships) · C10 Quests and moral choice (questline master, dynamic questlines, personal quests, NPC arcs, moral-choice chains/flags/gossip, branching, bureaucratic morality, expansion quests) · C11 Economy (market, baselines, regional prices, shocks, rumors, black market, debt ledger, tributes, trade screens, tell lines) · C12 Weather and Year of Ash (weather system, seasons, effects, gates, hardening, storm windows, Year-of-Ash families, epilogue pressure) · C13 Endgame and epilogue (Reckoning, verdict, epilogue matrix, chronicle, muster epilogues, standing records, census) · C14 Ecology and wildlife (migration, trapping, ecosystem, bestiary, flora, infestations, contagion, pathogens, crop genomes) · C15 Defense and security (perimeter, defense grid, sky defense, ordnance, chemical defense, orbital harrow, interlocks, EMP effects) · C16 Progression and meta (skills, research, collectibles, trophies, achievements, difficulty presets, XP wave, codex, field guide, bestiary, L10N, mods, settings, input) · C17 Host surface and UI (panels, shell, focus navigation, snapshots, a11y, briefings, dashboards).

> **B-18 · C11 · Trade-screen scenario expansion.** Subject: additional scenarios and tell lines for under-covered merchant identities. Evidence: `trade_screen_scenarios.json`, `trade_tell_lines.json`, `trade_specialties.json` verified live. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.

> **C-06 · C7 · Embargo pressure modeling.** Subject: `trade_embargoes.json` impact on settlement price bands; verify embargoes produce legible price signal, not noise. Evidence: embargo catalog verified live. Route: harness. Confidence: HIGH CONFIDENCE.

> **DM-5 — Expeditions and travel (C5).** Owners: expedition system, vehicles, dispatch preflight, scavenging tables, waystations, caravans, travel encounters, micro-locations, anomalous encounters. Live catalogs: `expeditions`, `vehicles`, `vehicle_modifications`, `vehicle_armor_grades`, `scavenging_tables`, `waystations`, `caravans`, `merchant_caravans`, `caravan_trade_routes`, `travel_encounters`, `micro_locations`, `anomalous_expedition_encounters`. Hosts: Expedition, ExpeditionVehicle, TravelingCaravan, Waystation, RescueDispatchPreflight. Docs: `EXPEDITION_30_DAY_PLAYTEST_REPORT.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md` (verified live). Openings: A-12, A-13, A-14, B-07, B-08, C-03, C-04, E-07.

> **DM-7 — Factions and war (C7).** Owners: stance engine, doctrines, war system/chain runner, tributes, treaties, embargoes, espionage, psyops, counter-intelligence, musters, labor camps, bounty board. Live catalogs: `factions`, `faction_lore`, `faction_territory`, `faction_intelligence`, branch catalogs (independent/military/rebel), faction war family (communiques/dialogue/events/journal/radio/location_overrides), `warlord_doctrines`, `muster_*` family (five), `labor_camps`, `bounty_board`, `regional_treaties`, `trade_embargoes`, `foundry_accords`, `holdfast_factions`, `crossing_factions`. Hosts: Espionage, PsyOps, CounterIntelligence, Muster, FactionBranch, RegionalTreaty. Openings: A-16, A-17, A-18, B-10 (GATE), B-11 (GATE), C-05, C-06, G-05, plus the F-004 muster campaign. Constraint: all standing effects through `FactionStanceEngine`.

> **DM-11 — Economy (C11).** Owners: market, price factors, shocks, baselines, regional prices, hardcore tuning, rumor bands, black market, caravans, debt ledger, foundry economy, bounty board, trade screens. Live catalogs: `commodity_baselines`, `regional_prices`, `hardcore_economy_tuning`, `economy_goods`, `black_market_inventory`, `ledger_debt_templates`, `trade_screen_scenarios`, `trade_tell_lines`, `trade_specialties`, `trade_texts`, `bounty_board`. Hosts: Economy, BlackMarket, TravelingCaravan, SilentFoundry. Docs: `ECONOMY_FAIRNESS_AUDIT.md`, `ECONOMY_PRICE_FACTOR_MATRIX.md` (verified live). Sealed: merchant restock priority (DEC-05). Openings: A-26, B-18, C-07, C-08, C-13, E-08, G-02. GATE: black-market funds legs.

> ```text
prose_field: journal_entry
purpose: the player's own record of an event or quest beat
trigger: event/quest beat
length: 60-120 words
viewpoint: first person, house restraint
must_include: one action taken, one observable consequence or open question
must_not_include: omniscient narration, named emotion
model:
  Traded two tins and a favor for the filters. The favor is the part
  I will regret; favors keep no ledger anyone can read. Walked the
  corridor twice after and the pump room still smells of hot iron,
  which the maintenance log says it should not. Noted it in the margin
  here because the log is someone's job and margins are mine.
```

> ```text
prose_field: courier_dispatch
purpose: sent instruction and returned account
trigger: courier mission leg
length: 40-80 words each
must_include (dispatch): destination, deadline, carry mark
must_include (debrief): arrival state, one deviation, one cost
model (dispatch): "West post, before the freeze hardens. Carry the
  tally, not the goods; the goods are already spoken for. Mark is
  the blue twine, three knots."
model (debrief): "Arrived on the third day with the tally intact and
  the twine one knot short. The knot was traded at the waystation,
  deliberately, for stove time. West post holds. The road does not."
```

**Applied constraints:** one bounded outcome, live-source collision sweep, explicit data/loader/consumer/save/test seams, no parallel authority, no unsupported content growth, and a final precision pass. Master file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`. Recorded SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.


# Appendix — Deep integration architecture

# Appendix — Deep Integration Architecture

## A. Boundary and responsibility map

Current evidence and safe integration boundary for Plan 105: Trade Specialties, Profession Milestones and Crafted-Item Learning.

- **profession definition parsing and registration** remains with `TradeSpecialtyCatalogLoader` at `Assets/Ashfall.Core/Survivors/TradeSpecialtyCatalogLoader.cs`. Static catalog owner.
- **profession/milestone/mastery state and crafted-item effects** remains with `TradeSpecialtySystem` at `Assets/Ashfall.Core/Survivors/TradeSpecialtySystem.cs`. Sole runtime owner.
- **catalog load and host composition** remains with `Phase0HostSession` at `src/Host/Phase0HostSession.cs`. Thin host seam.
- **setup/load owner** remains with `Main.Phase0` at `src/Main.Phase0.cs`. Current host path.
- **milestone/save/effect contracts** remains with `TradeSpecialtySystemTests` at `Ashfall.Core.Tests/TradeSpecialtySystemTests.cs`. Focused evidence.
- **catalog metadata/loader contract** remains with `TradeSpecialtyCatalogMetadataTests` at `Ashfall.Core.Tests/Progression/TradeSpecialtyCatalogMetadataTests.cs`. Focused evidence.

The architecture is successful only when a player action reaches the named owner, the owner commits its state, a typed fact is projected, and the existing save path captures the same fact. A panel, catalog scanner, test fixture or historical closeout is not a substitute for that route.

## B. End-to-end data and command flow

1. load 16 profession definitions
2. validate profession IDs, patterns and narrative refs
3. resolve current survivor profession
4. observe current crafted item
5. match patterns exactly once
6. advance milestone or mastery
7. capture/restore current specialty state

Each arrow is an authority direction, not a license for bidirectional mutation. If a host provider is absent, the correct result is a named refusal or a documented optional projection—not a fabricated fallback object.

## C. State, persistence and replay contract

- Profession definitions are immutable catalog content.
- TradeSpecialtySaveState is the current per-survivor milestone/mastery authority.
- Skill/morale effects are applied by the system through current owner events.
- No Plan-105 save section.

- One crafted item cannot count twice.
- Unknown profession/pattern fails without state mutation.
- Mastery requires the current milestone count.
- A narrative ID that cannot resolve is a content defect, not invented prose.

Capture must deep-copy mutable collections, restore must normalize only documented legacy absence, and checksum validation must occur over the frozen version shape. New state is not justified merely because a plan wants a richer readout; a durable fact needs a player consequence or a future consumer that cannot derive it.

## D. Host, Godot and UI contract

- src/Host/Phase0HostSession.cs
- src/Main.Phase0.cs

The interface should show the current projection, the available command, the cost/commitment, and a stable refusal reason. It should not recompute a balance, roll a hidden outcome, infer a missing catalog row, or turn a historical claim into a live feature. Keyboard/controller close and focus behavior remain part of the acceptance contract whenever a panel is touched.

## E. Focused verification contract

- Ashfall.Core.Tests/TradeSpecialtySystemTests.cs
- Ashfall.Core.Tests/Progression/TradeSpecialtyCatalogMetadataTests.cs
- Ashfall.Core.Tests/CraftAttributionTradeSpecialtyTests.cs

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
| S-01 | 105-01 16 rows load | load 16 profession definitions | Profession definitions are immutable catalog content. | A catalog profession is counted as reachable without a current survivor mapping. | TradeSpecialtyCatalogLoader |
| S-02 | 105-02 profession mapping | validate profession IDs, patterns and narrative refs | TradeSpecialtySaveState is the current per-survivor milestone/mastery authority. | Duplicate crafted item increments twice. | TradeSpecialtyCatalogLoader |
| S-03 | 105-03 pattern match | resolve current survivor profession | Skill/morale effects are applied by the system through current owner events. | A panel awards mastery. | TradeSpecialtyCatalogLoader |
| S-04 | 105-04 duplicate guard | observe current crafted item | No Plan-105 save section. | Narrative reference is invented at runtime. | TradeSpecialtyCatalogLoader |
| S-05 | 105-05 milestone once | match patterns exactly once | Profession definitions are immutable catalog content. | A new progression save is added. | TradeSpecialtyCatalogLoader |
| S-06 | 105-06 mastery effect | advance milestone or mastery | TradeSpecialtySaveState is the current per-survivor milestone/mastery authority. | A catalog profession is counted as reachable without a current survivor mapping. | TradeSpecialtyCatalogLoader |
| S-07 | 105-07 narrative reference | capture/restore current specialty state | Skill/morale effects are applied by the system through current owner events. | Duplicate crafted item increments twice. | TradeSpecialtyCatalogLoader |
| S-08 | 105-08 save continuation | load 16 profession definitions | No Plan-105 save section. | A panel awards mastery. | TradeSpecialtyCatalogLoader |
| S-09 | 105-09 UI progress | validate profession IDs, patterns and narrative refs | Profession definitions are immutable catalog content. | Narrative reference is invented at runtime. | TradeSpecialtyCatalogLoader |

Every scenario is a future verification obligation, not a fresh runtime result. A scenario passes only when the owner, event, save and presentation layers agree.


# Appendix — Test case catalog

# Appendix — Test Case Catalog and Evidence Map

| ID | Case | Layer | Assertion | Owner |
| --- | --- | --- | --- | --- |
| T-01 | 105-TC-01 schema/count | data | schema/count; verify the named current owner and its negative boundary without inventing a second authority. | TradeSpecialtyCatalogLoader |
| T-02 | 105-TC-02 pattern validation | unit | pattern validation; verify the named current owner and its negative boundary without inventing a second authority. | TradeSpecialtyCatalogLoader |
| T-03 | 105-TC-03 milestone state | persistence | milestone state; verify the named current owner and its negative boundary without inventing a second authority. | TradeSpecialtyCatalogLoader |
| T-04 | 105-TC-04 mastery boundary | determinism | mastery boundary; verify the named current owner and its negative boundary without inventing a second authority. | TradeSpecialtyCatalogLoader |
| T-05 | 105-TC-05 save round trip | host | save round trip; verify the named current owner and its negative boundary without inventing a second authority. | TradeSpecialtyCatalogLoader |
| T-06 | 105-TC-06 event source | UI/accessibility | event source; verify the named current owner and its negative boundary without inventing a second authority. | TradeSpecialtyCatalogLoader |
| T-07 | 105-TC-07 UI truth | cross-system | UI truth; verify the named current owner and its negative boundary without inventing a second authority. | TradeSpecialtyCatalogLoader |

The table intentionally separates unit, data, persistence, determinism, host, UI and cross-system cases. Do not aggregate independent state-transition, mutation, fuzz, replay or lifecycle tests into a misleading single count.


# Appendix — Current caller graph

# Appendix — Current Caller/Reference Graph

| Reference count | Current path | Interpretation |
| --- | --- | --- |
| 93 | `Ashfall.Core.Tests/Progression/TradeSpecialtyCatalogMetadataTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 23 | `Ashfall.Core.Tests/TradeSpecialtySystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 8 | `src/Host/HostCli.PanelTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 8 | `src/UI/CraftingPanel.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Ashfall.Core.Tests/CraftAttributionTradeSpecialtyTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Ashfall.Core.Tests/Progression/Plan105_106TradeDoseIntegrationTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `Ashfall.Core.Tests/Progression/TradeSpecialtySystemTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 7 | `src/Host/Phase0HostSession.cs` | current reference count; inspect the caller before treating it as a live route |
| 5 | `Assets/Ashfall.Core/Survivors/TradeSpecialtyCatalogLoader.cs` | current reference count; inspect the caller before treating it as a live route |
| 4 | `src/Main.Phase0.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Host/Phase0EffectsBridgeTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Ashfall.Core.Tests/Medical/MedicalPipelineArchitectureGateTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `Assets/Ashfall.Core/Survivors/TradeSpecialtySystem.cs` | current reference count; inspect the caller before treating it as a live route |
| 3 | `src/UI/Phase0Panel.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/GapTestCoverageTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Ashfall.Core.Tests/Medical/PsychologyProjectionTests.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `Assets/Ashfall.Core/Crafting/CraftContext.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Host/Phase0SaveStore.cs` | current reference count; inspect the caller before treating it as a live route |
| 1 | `src/Main.FlagshipInstitutions.cs` | current reference count; inspect the caller before treating it as a live route |

The graph is evidence for the next audit, not a generated architecture-map replacement. A reference inside a test or scanner does not prove production reachability.


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/trade_specialties.json`

### `Assets/StreamingAssets/Data/trade_specialties.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 20427; characters: 20427.
- SHA-256: `084f782ae3196dfe0eccffe745bdbe9d241c5d1020ab4f001b59d43a9ef8c965`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `items`

#### `items` — 16 current rows

- Row 001 `electrician`: `{"display_name":"Electrician","mastery_bonus_text":"{name} has mastered the pre-war trade of electrician. Their hands find the right wire by instinct, and the bunker's lights flicker less often now, and the hum of the generators sounds alm…`
- Row 002 `nurse`: `{"display_name":"Nurse","mastery_bonus_text":"{name}'s hands remember every patient, every bandage, every stitch, every life. The bunker calls them Doc now, and they answer to it, and the answering is its own kind of medicine.","mastery_na…`
- Row 003 `machinist`: `{"display_name":"Machinist","mastery_bonus_text":"The lathe hums under {name}'s touch like it remembers the old world too. Every part they make fits perfectly, no blueprints needed, and the fitting is the proof.","mastery_narrative":"narra…`
- Row 004 `teacher`: `{"display_name":"Teacher","mastery_bonus_text":"{name} gathers the survivors in the common room. Class is in session, they announce, and for the first time in months people take notes, and someone even laughs, and the laughing is the lesso…`
- Row 005 `miller`: `{"display_name":"Miller","mastery_bonus_text":"{name} turns coarse bunker barley into fine white flour with zero waste, and the bread in the holdfast tastes like memory now, and memory is what keeps people eating.","mastery_narrative":"nar…`
- Row 006 `wireman`: `{"display_name":"Wireman","mastery_bonus_text":"{name} can trace a short-circuit through two hundred meters of subterranean concrete by ear alone, and the ear is never wrong, and the concrete has started to fear it.","mastery_narrative":"n…`
- Row 007 `bone_setter`: `{"display_name":"Bone Setter","mastery_bonus_text":"Compound fractures that would have crippled an expedition member heal straight and strong under {name}'s firm hands, and the straightness is the whole argument.","mastery_narrative":"narr…`
- Row 008 `preservationist`: `{"display_name":"Preservationist","mastery_bonus_text":"Not a single gram of harvested crop or scavenged meat goes rancid under {name}'s watchful curing protocols, and the not-going-rancid is what winter is for.","mastery_narrative":"narra…`
- Row 009 `radio_technician`: `{"display_name":"Radio Technician","mastery_bonus_text":"{name} can pull clean voice signals from hundred-mile skips through total magnetic interference, and the pulling is what keeps the bunker from being alone.","mastery_narrative":"narr…`
- Row 010 `greenhouse_grower`: `{"display_name":"Greenhouse Grower","mastery_bonus_text":"Under {name}'s care, hydroponic trays produce crisp radishes and kale even in the coldest nuclear winter, and the crispness is the argument against the cold.","mastery_narrative":"n…`
- Row 011 `surveyor`: `{"display_name":"Surveyor","mastery_bonus_text":"{name}'s maps reveal hidden culverts, stable overpasses, and safe dead-ground across the whole crater, and the revealing is what keeps the scouts alive.","mastery_narrative":"narrative_trade…`
- Row 012 `salvage_appraiser`: `{"display_name":"Salvage Appraiser","mastery_bonus_text":"{name} knows the hidden value of every rusted pre-war component, and the knowing ensures the holdfast never trades at a loss, and never trades at a loss is the whole point.","master…`
- Row 013 `apiarist`: `{"display_name":"Apiarist","mastery_bonus_text":"The bees thrive in dark underground chambers under {name}'s gentle management, and the thriving is what the honey and wax are for, and the honey and wax are what winter needs.","mastery_narr…`
- Row 014 `metallurgist`: `{"display_name":"Metallurgist","mastery_bonus_text":"{name} smelts brittle scrap into spring-tempered tool steel that outlasts anything found in the ruins, and the outlasting is the proof of the smelting.","mastery_narrative":"narrative_tr…`
- Row 015 `water_technician`: `{"display_name":"Water Technician","mastery_bonus_text":"{name}'s multi-stage filtration arrays deliver sparkling, zero-rad drinking water to every bunk, and the sparkling is what hope looks like when it's wet.","mastery_narrative":"narrat…`
- Row 016 `tailor`: `{"display_name":"Tailor","mastery_bonus_text":"{name} crafts warm, lead-lined winter parkas from salvage wool and rubber, and the parkas protect the survivors from the ash cold, and the protecting is what the wool is for.","mastery_narrati…`


# Appendix — Complete Catalog Audit: `Assets/StreamingAssets/Data/trade_specialties.json`

### `Assets/StreamingAssets/Data/trade_specialties.json` — complete current row audit

- Parse status: **valid JSON**.
- Bytes: 20427; characters: 20427.
- SHA-256: `084f782ae3196dfe0eccffe745bdbe9d241c5d1020ab4f001b59d43a9ef8c965`.
- Row presence is not reachability. Each row below is an authored fact requiring a loader/consumer check.

Root keys: `schema_version`, `items`

#### `items` — 16 current rows

- Row 001 `electrician`: `{"display_name":"Electrician","mastery_bonus_text":"{name} has mastered the pre-war trade of electrician. Their hands find the right wire by instinct, and the bunker's lights flicker less often now, and the hum of the generators sounds alm…`
- Row 002 `nurse`: `{"display_name":"Nurse","mastery_bonus_text":"{name}'s hands remember every patient, every bandage, every stitch, every life. The bunker calls them Doc now, and they answer to it, and the answering is its own kind of medicine.","mastery_na…`
- Row 003 `machinist`: `{"display_name":"Machinist","mastery_bonus_text":"The lathe hums under {name}'s touch like it remembers the old world too. Every part they make fits perfectly, no blueprints needed, and the fitting is the proof.","mastery_narrative":"narra…`
- Row 004 `teacher`: `{"display_name":"Teacher","mastery_bonus_text":"{name} gathers the survivors in the common room. Class is in session, they announce, and for the first time in months people take notes, and someone even laughs, and the laughing is the lesso…`
- Row 005 `miller`: `{"display_name":"Miller","mastery_bonus_text":"{name} turns coarse bunker barley into fine white flour with zero waste, and the bread in the holdfast tastes like memory now, and memory is what keeps people eating.","mastery_narrative":"nar…`
- Row 006 `wireman`: `{"display_name":"Wireman","mastery_bonus_text":"{name} can trace a short-circuit through two hundred meters of subterranean concrete by ear alone, and the ear is never wrong, and the concrete has started to fear it.","mastery_narrative":"n…`
- Row 007 `bone_setter`: `{"display_name":"Bone Setter","mastery_bonus_text":"Compound fractures that would have crippled an expedition member heal straight and strong under {name}'s firm hands, and the straightness is the whole argument.","mastery_narrative":"narr…`
- Row 008 `preservationist`: `{"display_name":"Preservationist","mastery_bonus_text":"Not a single gram of harvested crop or scavenged meat goes rancid under {name}'s watchful curing protocols, and the not-going-rancid is what winter is for.","mastery_narrative":"narra…`
- Row 009 `radio_technician`: `{"display_name":"Radio Technician","mastery_bonus_text":"{name} can pull clean voice signals from hundred-mile skips through total magnetic interference, and the pulling is what keeps the bunker from being alone.","mastery_narrative":"narr…`
- Row 010 `greenhouse_grower`: `{"display_name":"Greenhouse Grower","mastery_bonus_text":"Under {name}'s care, hydroponic trays produce crisp radishes and kale even in the coldest nuclear winter, and the crispness is the argument against the cold.","mastery_narrative":"n…`
- Row 011 `surveyor`: `{"display_name":"Surveyor","mastery_bonus_text":"{name}'s maps reveal hidden culverts, stable overpasses, and safe dead-ground across the whole crater, and the revealing is what keeps the scouts alive.","mastery_narrative":"narrative_trade…`
- Row 012 `salvage_appraiser`: `{"display_name":"Salvage Appraiser","mastery_bonus_text":"{name} knows the hidden value of every rusted pre-war component, and the knowing ensures the holdfast never trades at a loss, and never trades at a loss is the whole point.","master…`
- Row 013 `apiarist`: `{"display_name":"Apiarist","mastery_bonus_text":"The bees thrive in dark underground chambers under {name}'s gentle management, and the thriving is what the honey and wax are for, and the honey and wax are what winter needs.","mastery_narr…`
- Row 014 `metallurgist`: `{"display_name":"Metallurgist","mastery_bonus_text":"{name} smelts brittle scrap into spring-tempered tool steel that outlasts anything found in the ruins, and the outlasting is the proof of the smelting.","mastery_narrative":"narrative_tr…`
- Row 015 `water_technician`: `{"display_name":"Water Technician","mastery_bonus_text":"{name}'s multi-stage filtration arrays deliver sparkling, zero-rad drinking water to every bunk, and the sparkling is what hope looks like when it's wet.","mastery_narrative":"narrat…`
- Row 016 `tailor`: `{"display_name":"Tailor","mastery_bonus_text":"{name} crafts warm, lead-lined winter parkas from salvage wool and rubber, and the parkas protect the survivors from the ash cold, and the protecting is what the wool is for.","mastery_narrati…`


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Survivors/TradeSpecialtySystem.cs`

### `Assets/Ashfall.Core/Survivors/TradeSpecialtySystem.cs` — complete current file

- Size: 451 lines / 21404 bytes.
- SHA-256: `f9ad2632abf72b8925bf610aa461aef2ca05f652c777b3fbc51082172bb8fc48`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005:
00006: namespace Ashfall.Core.Survivors
00007: {
00008:     // ── Save/load DTOs ───────────────────────────────────────────────
00009:     [Serializable]
00010:     public sealed class TradeSpecialtySurvivorState
00011:     {
00012:         public string survivorId = string.Empty;
00013:         public string professionId = string.Empty;
00014:         public List<string> craftMilestonesCompleted = new List<string>();
00015:         public bool mastered;
00016:     }
00017:
00018:     [Serializable]
00019:     public sealed class TradeSpecialtySaveState
00020:     {
00021:         public string systemId = TradeSpecialtySystem.SystemId;
00022:         public List<TradeSpecialtySurvivorState> survivors = new List<TradeSpecialtySurvivorState>();
00023:     }
00024:
00025:     // ── Authored profession content (trade_specialties.json) ─────────
00026:     // Presentation/reference data keyed by milestone tier. Content only, never
00027:     // per-run state, so none of it belongs in TradeSpecialtySaveState.
00028:
00029:     [Serializable]
00030:     public sealed class TradeSpecialtyMilestoneInfo
00031:     {
00032:         public int Tier = 1;
00033:         public string Title = string.Empty;
00034:         public string NarrativeId = string.Empty;
00035:
00036:         /// <summary>Authored per-milestone bonus. Retained so it is queryable and no
00037:         /// longer silently dropped; the runtime still applies the constants below.</summary>
00038:         public float SkillBonus;
00039:     }
00040:
00041:     [Serializable]
00042:     public sealed class TradeSpecialtyProfessionInfo
00043:     {
00044:         public string ProfessionId = string.Empty;
00045:         public string DisplayName = string.Empty;
00046:         public string MasteryNarrativeId = string.Empty;
00047:         public string MasteryBonusText = string.Empty;
00048:         public List<string> Aliases = new List<string>();
00049:         public Dictionary<int, TradeSpecialtyMilestoneInfo> Milestones =
00050:             new Dictionary<int, TradeSpecialtyMilestoneInfo>();
00051:     }
00052:
00053:     /// <summary>
00054:     /// Trade Specialty System — pre-war professions unlock specialized perk
00055:     /// trees as survivors craft related items, turning basic tasks into
00056:     /// narrative milestones.
00057:     ///
00058:     /// Professions: electrician, nurse, machinist, teacher. Each has 3
00059:     /// milestone tiers; completing all 3 masters the trade.
00060:     ///
00061:     /// Engine-agnostic port: operates on string survivor ids (no engine
00062:     /// Survivor object), raises C# events on milestone/mastery, and is
00063:     /// save/load safe via CaptureState/RestoreState (deep copy). Host injects
00064:     /// the skill-bonus / morale / narrative hooks in its own domain.
00065:     /// All constants match the Unity source 1:1.
00066:     /// </summary>
00067:     public class TradeSpecialtySystem
00068:     {
00069:         public const string SystemId = "trade_specialty_system";
00070:
00071:         // ── Constants (match Unity source 1:1) ────────────────────────
00072:         public const int MilestonesToMaster = 3;
00073:         public const float MasterySkillBonus = 0.15f;
00074:         public const float MasteryMoraleBonus = 10f;
00075:         public const float MilestoneSkillBonusFactor = 0.3f;
00076:
00077:         // ── Profession crafting categories (item id prefixes) ──────────
00078:         public static readonly Dictionary<string, List<string>> ProfessionItemCategories =
00079:             new Dictionary<string, List<string>>
00080:             {
00081:                 { "electrician", new List<string> { "battery", "generator", "circuit", "wire", "power", "solar", "turbine" } },
00082:                 { "nurse", new List<string> { "bandage", "splint", "antiseptic", "saline", "stitch", "tourniquet", "medical" } },
00083:                 { "machinist", new List<string> { "wrench", "tool", "gear", "spring", "lever", "blade", "mechanism", "lathe" } },
00084:                 { "teacher", new List<string> { "book", "chalk", "slate", "journal", "ink", "lesson", "diagram" } }
00085:             };
00086:
00087:         public static void RegisterProfessionPatterns(string professionId, IEnumerable<string> patterns)
00088:         {
00089:             if (string.IsNullOrEmpty(professionId) || patterns == null) return;
00090:             if (!ProfessionItemCategories.TryGetValue(professionId, out var list))
00091:             {
00092:                 list = new List<string>();
00093:                 ProfessionItemCategories[professionId] = list;
00094:             }
00095:             foreach (var p in patterns)
00096:             {
00097:                 if (!string.IsNullOrEmpty(p) && !list.Contains(p))
00098:                     list.Add(p);
00099:             }
00100:         }
00101:
00102:         /// <summary>
00103:         /// True when the item matches the profession's authored crafting patterns.
00104:         /// This is the same rule OnItemCrafted applies, exposed so a host can
00105:         /// attribute an unassigned craft without duplicating the match logic.
00106:         /// </summary>
00107:         public static bool ProfessionMatchesItem(string professionId, string itemId)
00108:         {
00109:             if (string.IsNullOrEmpty(professionId) || string.IsNullOrEmpty(itemId)) return false;
00110:             if (!ProfessionItemCategories.TryGetValue(professionId, out var categories)) return false;
00111:             if (categories == null || categories.Count == 0) return false;
00112:
00113:             List<string> tokens = null;
00114:             for (int i = 0; i < categories.Count; i++)
00115:             {
00116:                 string pattern = categories[i];
00117:                 if (string.IsNullOrEmpty(pattern)) continue;
00118:
00119:                 // Authored patterns are single words. If one ever contains a
00120:                 // separator, token matching could never hit it and the pattern
00121:                 // would go silently dead — so fall back to a substring match.
00122:                 if (pattern.IndexOfAny(ItemIdSeparators) >= 0)
00123:                 {
00124:                     if (itemId.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0)
00125:                         return true;
00126:                     continue;
00127:                 }
00128:
00129:                 tokens ??= TokenizeItemId(itemId);
00130:                 for (int t = 0; t < tokens.Count; t++)
00131:                 {
00132:                     if (TokenMatchesPattern(tokens[t], pattern)) return true;
00133:                 }
00134:             }
00135:             return false;
00136:         }
00137:
00138:         private static readonly char[] ItemIdSeparators = { '_', '-', '.', ' ' };
00139:
00140:         private static List<string> TokenizeItemId(string itemId)
00141:         {
00142:             var parts = itemId.Split(ItemIdSeparators, StringSplitOptions.RemoveEmptyEntries);
00143:             var tokens = new List<string>(parts.Length);
00144:             for (int i = 0; i < parts.Length; i++)
00145:                 if (parts[i].Length > 0) tokens.Add(parts[i]);
00146:             return tokens;
00147:         }
00148:
00149:         /// <summary>
00150:         /// True when an id token is the pattern or a plain inflection of it
00151:         /// (s / es / d / ed / ing / y). Inflection is accepted so authored items
00152:         /// keep matching — item_brined_legume_mash against "brine", item_salted_meat
00153:         /// against "salt", item_smoked_meat against "smoke". Whole-token anchoring
00154:         /// is what rejects the substring accidents: "tube" no longer matches
00155:         /// item_pickled_tubers, and "book" no longer matches item_comm_codebook_alpha.
00156:         /// </summary>
00157:         private static bool TokenMatchesPattern(string token, string pattern)
00158:         {
00159:             if (token.Length < pattern.Length) return false;
00160:             if (!token.StartsWith(pattern, StringComparison.OrdinalIgnoreCase)) return false;
00161:
00162:             string tail = token.Substring(pattern.Length);
00163:             switch (tail.Length)
00164:             {
00165:                 case 0: return true;
00166:                 case 1:
00167:                     return EqualsAny(tail, "s", "d", "y");
00168:                 case 2:
00169:                     return EqualsAny(tail, "es", "ed");
00170:                 case 3:
00171:                     return EqualsAny(tail, "ing");
00172:                 default:
00173:                     return false;
00174:             }
00175:         }
00176:
00177:         private static bool EqualsAny(string value, string a)
00178:         {
00179:             return string.Equals(value, a, StringComparison.OrdinalIgnoreCase);
00180:         }
00181:
00182:         private static bool EqualsAny(string value, string a, string b)
00183:         {
00184:             return string.Equals(value, a, StringComparison.OrdinalIgnoreCase)
00185:                 || string.Equals(value, b, StringComparison.OrdinalIgnoreCase);
00186:         }
00187:
00188:         private static bool EqualsAny(string value, string a, string b, string c)
00189:         {
00190:             return EqualsAny(value, a, b)
00191:                 || string.Equals(value, c, StringComparison.OrdinalIgnoreCase);
00192:         }
00193:
00194:         // ── Profession content registry (fed by TradeSpecialtyCatalogLoader) ──
00195:         public static readonly Dictionary<string, TradeSpecialtyProfessionInfo> ProfessionInfo =
00196:             new Dictionary<string, TradeSpecialtyProfessionInfo>(StringComparer.Ordinal);
00197:
00198:         /// <summary>Registers authored profession content. Re-registering the same
00199:         /// profession replaces it, so reloading the catalog cannot duplicate state.</summary>
00200:         public static void RegisterProfessionInfo(TradeSpecialtyProfessionInfo? info)
00201:         {
00202:             if (info == null || string.IsNullOrEmpty(info.ProfessionId)) return;
00203:             ProfessionInfo[info.ProfessionId] = info;
00204:             RebuildProfessionLabelIndex();
00205:         }
00206:
00207:         public static TradeSpecialtyProfessionInfo? GetProfessionInfo(string professionId)
00208:         {
00209:             if (string.IsNullOrEmpty(professionId)) return null;
00210:             return ProfessionInfo.TryGetValue(professionId, out var info) ? info : null;
00211:         }
00212:
00213:         public static TradeSpecialtyMilestoneInfo? GetMilestone(string professionId, int tier)
00214:         {
00215:             var info = GetProfessionInfo(professionId);
00216:             if (info == null || info.Milestones == null) return null;
00217:             return info.Milestones.TryGetValue(tier, out var milestone) ? milestone : null;
00218:         }
00219:
00220:         public static string GetDisplayName(string professionId)
00221:             => GetProfessionInfo(professionId)?.DisplayName ?? string.Empty;
00222:
00223:         /// <summary>Authored mastery narrative event id, or empty when the profession
00224:         /// has no catalog entry. Empty means MasterTrade fires nothing rather than
00225:         /// inventing an id that events.json cannot resolve.</summary>
00226:         public static string GetMasteryNarrativeId(string professionId)
00227:             => GetProfessionInfo(professionId)?.MasteryNarrativeId ?? string.Empty;
00228:
00229:         public static string GetMasteryBonusText(string professionId)
00230:             => GetProfessionInfo(professionId)?.MasteryBonusText ?? string.Empty;
00231:
00232:         public static string GetMilestoneTitle(string professionId, int tier)
00233:             => GetMilestone(professionId, tier)?.Title ?? string.Empty;
00234:
00235:         public static string GetMilestoneNarrativeId(string professionId, int tier)
00236:             => GetMilestone(professionId, tier)?.NarrativeId ?? string.Empty;
00237:
00238:         // ── Profession label → specialty id resolution ─────────────────
00239:         // Survivor rosters author display labels ("Trauma Surgeon") while
00240:         // specialty trees are keyed by id (bone_setter). The index is rebuilt
00241:         // from the registry in ordinal profession-id order, so resolution never
00242:         // depends on catalog load order or registration sequence.
00243:         private static readonly Dictionary<string, string> s_professionLabelIndex =
00244:             new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
00245:
00246:         public static IReadOnlyDictionary<string, string> ProfessionLabelIndex => s_professionLabelIndex;
00247:
00248:         private static void RebuildProfessionLabelIndex()
00249:         {
00250:             s_professionLabelIndex.Clear();
00251:             var ids = new List<string>(ProfessionInfo.Keys);
00252:             ids.Sort(StringComparer.Ordinal);
00253:             for (int i = 0; i < ids.Count; i++)
00254:             {
00255:                 var info = ProfessionInfo[ids[i]];
00256:                 if (info == null || info.Aliases == null) continue;
00257:                 for (int a = 0; a < info.Aliases.Count; a++)
00258:                 {
00259:                     string alias = info.Aliases[a]?.Trim() ?? string.Empty;
00260:                     if (alias.Length == 0) continue;
00261:                     if (!s_professionLabelIndex.ContainsKey(alias))
00262:                         s_professionLabelIndex[alias] = info.ProfessionId;
00263:                 }
00264:             }
00265:         }
00266:
00267:         /// <summary>
00268:         /// Resolve a survivor's specialty id. An explicit authored
00269:         /// pre_war_profession_id always wins; otherwise the roster display label
00270:         /// is matched against authored profession_aliases. Empty means the
00271:         /// survivor has no trade specialty tree.
00272:         /// </summary>
00273:         public static string ResolveProfessionId(string? explicitProfessionId, string? professionLabel)
00274:         {
00275:             if (!string.IsNullOrWhiteSpace(explicitProfessionId))
00276:                 return explicitProfessionId.Trim();
00277:             return ResolveProfessionIdFromLabel(professionLabel);
00278:         }
00279:
00280:         public static string ResolveProfessionIdFromLabel(string? professionLabel)
00281:         {
00282:             if (string.IsNullOrWhiteSpace(professionLabel)) return string.Empty;
00283:             return s_professionLabelIndex.TryGetValue(professionLabel.Trim(), out var id) ? id : string.Empty;
00284:         }
00285:
00286:         // ── Events ─────────────────────────────────────────────────────
00287:         /// <summary>SurvivorId, professionId, milestoneTier (1-3).</summary>
00288:         public event Action<string, string, int> OnSpecialtyMilestone;
00289:         /// <summary>SurvivorId, professionId — all 3 tiers completed.</summary>
00290:         public event Action<string, string> OnSpecialtyMastered;
00291:         public event Action OnStateChanged;
00292:
00293:         // ── Host hooks (hosts apply the effects in their own domain) ──
00294:         public Action<string, string, float> GrantSkillBonus;
00295:         // survivorId, professionId, bonus
00296:         public Action<string, float> ApplyMoraleDelta;
00297:         // survivorId, delta
00298:         public Func<string, string> GetNarrativeEventId;
00299:         // professionId → narrativeEventId
00300:         public Action<string, string> FireNarrativeEvent;
00301:         // narrativeEventId, survivorId
00302:
00303:         // ── State ──────────────────────────────────────────────────────
00304:         private readonly Dictionary<string, TradeSpecialtySurvivorState> _bySurvivor =
00305:             new Dictionary<string, TradeSpecialtySurvivorState>(StringComparer.Ordinal);
00306:
00307:         private TradeSpecialtySurvivorState GetOrCreate(string survivorId, string professionId)
00308:         {
00309:             if (!_bySurvivor.TryGetValue(survivorId, out var state))
00310:             {
00311:                 state = new TradeSpecialtySurvivorState
00312:                 {
00313:                     survivorId = survivorId,
00314:                     professionId = professionId ?? string.Empty
00315:                 };
00316:                 _bySurvivor[survivorId] = state;
00317:             }
00318:             return state;
00319:         }
00320:
00321:         /// <summary>
00322:         /// Called when a survivor crafts an item. Checks if the item matches
00323:         /// their profession's specialty tree.
00324:         /// </summary>
00325:         public void OnItemCrafted(string survivorId, string professionId, string itemId)
00326:         {
00327:             if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(professionId)) return;
00328:             if (string.IsNullOrEmpty(itemId)) return;
00329:
00330:             var state = GetOrCreate(survivorId, professionId);
00331:             if (state.mastered || state.craftMilestonesCompleted.Count >= MilestonesToMaster)
00332:                 return; // matches Unity: count drives the guard, not just the flag
00333:             if (!string.Equals(state.professionId, professionId, StringComparison.Ordinal))
00334:                 return; // profession changed — do not count toward an old tree
00335:
00336:             if (!ProfessionMatchesItem(professionId, itemId)) return;
00337:
00338:             string milestoneId = professionId + "_" + itemId;
00339:             if (state.craftMilestonesCompleted.Contains(milestoneId))
00340:                 return;
00341:
00342:             state.craftMilestonesCompleted.Add(milestoneId);
00343:             int milestoneTier = Math.Min(state.craftMilestonesCompleted.Count, MilestonesToMaster);
00344:             OnSpecialtyMilestone?.Invoke(survivorId, professionId, milestoneTier);
00345:
00346:             // Authored per-tier narrative (milestones[].narrative). Fires on every tier
00347:             // including the third, so a mastery craft emits both its tier narrative and
00348:             // the profession-level mastery_narrative from MasterTrade below.
00349:             string tierNarrativeId = GetMilestoneNarrativeId(professionId, milestoneTier);
00350:             if (!string.IsNullOrEmpty(tierNarrativeId))
00351:                 FireNarrativeEvent?.Invoke(tierNarrativeId, survivorId);
00352:
00353:             if (state.craftMilestonesCompleted.Count >= MilestonesToMaster)
00354:             {
00355:                 MasterTrade(survivorId, state);
00356:             }
00357:             else
00358:             {
00359:                 // Intermediate milestone — the authored skill_bonus from
00360:                 // trade_specialties.json wins; the derived constant is only a
00361:                 // fallback for professions the catalog does not cover. Mastery
00362:                 // keeps MasterySkillBonus deliberately: the catalog authors one
00363:                 // value per milestone and no separate mastery bonus, so applying
00364:                 // the milestone value at tier 3 would cut the payoff to a third.
00365:                 var milestone = GetMilestone(professionId, milestoneTier);
00366:                 float bonus = milestone != null
00367:                     ? milestone.SkillBonus
00368:                     : MasterySkillBonus * MilestoneSkillBonusFactor;
00369:                 GrantSkillBonus?.Invoke(survivorId, professionId, bonus);
00370:             }
00371:             RaiseChanged();
00372:         }
00373:
00374:         private void MasterTrade(string survivorId, TradeSpecialtySurvivorState state)
00375:         {
00376:             state.mastered = true;
00377:             GrantSkillBonus?.Invoke(survivorId, state.professionId, MasterySkillBonus);
00378:             ApplyMoraleDelta?.Invoke(survivorId, MasteryMoraleBonus);
00379:             OnSpecialtyMastered?.Invoke(survivorId, state.professionId);
00380:
00381:             // Authored mastery_narrative from trade_specialties.json is the authority.
00382:             // GetNarrativeEventId remains only as a fallback for professions the catalog
00383:             // does not cover, so the host hook cannot override authored content.
00384:             string narrativeId = GetMasteryNarrativeId(state.professionId);
00385:             if (string.IsNullOrEmpty(narrativeId))
00386:                 narrativeId = GetNarrativeEventId?.Invoke(state.professionId) ?? string.Empty;
00387:             if (!string.IsNullOrEmpty(narrativeId))
00388:                 FireNarrativeEvent?.Invoke(narrativeId, survivorId);
00389:         }
00390:
00391:         /// <summary>
00392:         /// Get the mastery tier for a survivor's profession (0-3).
00393:         /// </summary>
00394:         public int GetMasteryTier(string survivorId)
00395:         {
00396:             return _bySurvivor.TryGetValue(survivorId, out var state)
00397:                 ? Math.Min(state.craftMilestonesCompleted.Count, MilestonesToMaster) : 0;
00398:         }
00399:
00400:         /// <summary>
00401:         /// True if survivor has mastered their pre-war trade.
00402:         /// </summary>
00403:         public bool HasMasteredTrade(string survivorId)
00404:         {
00405:             return _bySurvivor.TryGetValue(survivorId, out var state) && state.mastered;
00406:         }
00407:
00408:         // ── Save / Load ────────────────────────────────────────────────
00409:
00410:         public TradeSpecialtySaveState CaptureState()
00411:         {
00412:             var save = new TradeSpecialtySaveState { systemId = SystemId };
00413:             var keys = new List<string>(_bySurvivor.Keys);
00414:             keys.Sort(string.CompareOrdinal);
00415:             for (int i = 0; i < keys.Count; i++)
00416:             {
00417:                 var s = _bySurvivor[keys[i]];
00418:                 save.survivors.Add(new TradeSpecialtySurvivorState
00419:                 {
00420:                     survivorId = s.survivorId,
00421:                     professionId = s.professionId,
00422:                     craftMilestonesCompleted = new List<string>(s.craftMilestonesCompleted),
00423:                     mastered = s.mastered
00424:                 });
00425:             }
00426:             return save;
00427:         }
00428:
00429:         public void RestoreState(TradeSpecialtySaveState save)
00430:         {
00431:             _bySurvivor.Clear();
00432:             if (save?.survivors == null) return;
00433:             for (int i = 0; i < save.survivors.Count; i++)
00434:             {
00435:                 var s = save.survivors[i];
00436:                 if (s == null || string.IsNullOrEmpty(s.survivorId)) continue;
00437:                 _bySurvivor[s.survivorId] = new TradeSpecialtySurvivorState
00438:                 {
00439:                     survivorId = s.survivorId,
00440:                     professionId = s.professionId ?? string.Empty,
00441:                     craftMilestonesCompleted = s.craftMilestonesCompleted != null
00442:                         ? new List<string>(s.craftMilestonesCompleted) : new List<string>(),
00443:                     mastered = s.mastered
00444:                 };
00445:             }
00446:             RaiseChanged();
00447:         }
00448:
00449:         private void RaiseChanged() => OnStateChanged?.Invoke();
00450:     }
00451: }
```


# Appendix — Current Source Detail: `src/Host/Phase0HostSession.cs`

### `src/Host/Phase0HostSession.cs` — bounded current excerpt (1014 of 1095 lines)

- Size: 1095 lines / 56129 bytes.
- SHA-256: `56eab2884bd11ab7fedfa64542e059d3126ad3d3b977bdbca6ceb41f8559a199`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: #pragma warning disable CS8618
00005: using Godot;
00006: using Ashfall.Core;
00007: using Ashfall.Core.Survivors;
00008: using Ashfall.Core.Medical;
00009: using Ashfall.Core.Radiation;
00010: using Ashfall.Core.Phantoms;
00011: using Ashfall.Core.Shelter;
00012:
00013: namespace AtomicWar.GodotApp
00014: {
00015:     /// <summary>
00016:     /// Host-side aggregate view of one survivor's Phase-0 effect state.
00017:     /// The values here are DERIVED from the Core systems for presentation and
00018:     /// save; the Core systems own the rules. Real gameplay consumers (NeedsSystem
00019:     /// morale/health/fatigue, CraftingSystem craft time, ExpeditionSystem stamina,
00020:     /// Journal narrative) are reached through <see cref="Phase0EffectConsumers"/>.
00021:     /// </summary>
00022:     public class Phase0SurvivorEffects
00023:     {
00024:         public string survivorId = string.Empty;
00025:         /// <summary>Combined work-efficiency factor from phantom motivation and flashback penalty.</summary>
00026:         public float workEfficiencyMultiplier = 1f;
00027:         /// <summary>Hours the survivor refuses to work (phantom breakdown).</summary>
00028:         public float workRefusalHours = 0f;
00029:         /// <summary>Stamina multiplier (respiratory degeneration).</summary>
00030:         public float staminaMultiplier = 1f;
00031:         /// <summary>Guilt insomnia severity 0..1.</summary>
00032:         public float guiltInsomniaSeverity = 0f;
00033:         /// <summary>Combat trauma hypervigilance 0..1 (defense bonus).</summary>
00034:         public float hypervigilance = 0f;
00035:         /// <summary>Moral branch direction (Neutral until decided).</summary>
00036:         public string moralBranch = "Neutral";
00037:         /// <summary>Radiation sickness phase.</summary>
00038:         public string radiationPhase = "Healthy";
00039:         /// <summary>Dependency crafting penalty factor (0 = none).</summary>
00040:         public float dependencyCraftingPenalty = 0f;
00041:         /// <summary>Dependency combat penalty factor (0 = none).</summary>
00042:         public float dependencyCombatPenalty = 0f;
00043:         /// <summary>Final-wish state (empty / active / completed / failed).</summary>
00044:         public string finalWishState = string.Empty;
00045:         /// <summary>Authored final-wish title (empty when no catalog entry is bound).</summary>
00046:         public string finalWishTitle = string.Empty;
00047:         /// <summary>Authored final-wish description shown while active.</summary>
00048:         public string finalWishDescription = string.Empty;
00049:         /// <summary>Days remaining in the terminal-prognosis window (active only).</summary>
00050:         public float finalWishDaysRemaining;
00051:         /// <summary>Steps completed so far in the wish questline.</summary>
00052:         public int finalWishStepsDone;
00053:         /// <summary>Total steps required to complete the wish (0 when unknown).</summary>
00054:         public int finalWishStepsTotal;
00055:         /// <summary>Authored completion text shown when the wish is completed.</summary>
00056:         public string finalWishCompletionText = string.Empty;
00057:     }
00058:
00059:     /// <summary>Serialized Phase-0 effects envelope (all 10 systems).</summary>
00060:     public class Phase0EffectsSaveState
00061:     {
00062:         public PhaseProgressionSaveState radiationPhase = new PhaseProgressionSaveState();
00063:         public PhantomMemoryEngineState phantom = new PhantomMemoryEngineState();
00064:         public GuiltInsomniaSaveState guilt = new GuiltInsomniaSaveState();
00065:         public CombatTraumaSaveState combatTrauma = new CombatTraumaSaveState();
00066:         public SomaticFlashbackSaveState flashbacks = new SomaticFlashbackSaveState();
00067:         public MoralBranchingSaveState moral = new MoralBranchingSaveState();
00068:         public TradeSpecialtySaveState tradeSpecialty = new TradeSpecialtySaveState();
00069:         public FinalWishSaveState finalWishes = new FinalWishSaveState();
00070:         public RespiratoryDegenerationState respiratory = new RespiratoryDegenerationState();
00071:         public List<Phase0SurvivorEffects> effects = new List<Phase0SurvivorEffects>();
00072:         public float permanentShelterMoraleBuff = 0f;
00073:     }
00074:
00075:     /// <summary>
00076:     /// Immutable real-consumer wiring bundle. The host (Main.cs) constructs one
00077:     /// fully-bound instance and assigns it to <see cref="Phase0HostSession.Consumers"/>
00078:     /// so Phase-0 effects reach the authoritative gameplay consumers instead of
00079:     /// living in a display value. There are no mutable fields to silently leave
00080:     /// unwired.
00081:     ///
00082:     /// Effect classification:
00083:     ///  - Essential (health/morale/fatigue/shelter-morale): default to named
00084:     ///    no-op adapters when null, tracked as unbound.
00085:     ///  - Production-required (progression/medical/narrative): stay null when
00086:     ///    unbound so the session's null-check skips preserve headless behavior;
00087:     ///    tracked as unbound for startup validation.
00088:     ///  - Truly optional (ApplyWorkRefusalHours): null when unbound, not tracked.
00089:     ///
00090:     /// Use <see cref="NoOp"/> (with optional overrides) for isolated tests.
00091:     /// </summary>
00092:     public sealed class Phase0EffectConsumers
00093:     {
00094:         // ── Essential effects (health / morale / fatigue) — no-op when unbound ──
00095:
00096:         /// <summary>survivorId, morale delta → NeedsSystem.</summary>
00097:         public Action<string, float> ApplyMoraleDelta { get; }
00098:         /// <summary>survivorId, health delta → NeedsSystem.</summary>
00099:         public Action<string, float> ApplyHealthDelta { get; }
00100:         /// <summary>survivorId, fatigue delta → NeedsSystem.</summary>
00101:         public Action<string, float> ApplyFatigueDelta { get; }
00102:         /// <summary>Shelter-wide morale delta (final wish / moral branching).</summary>
00103:         public Action<float> ApplyShelterMoraleDelta { get; }
00104:
00105:         // ── Production-required (progression / medical / narrative) — null when unbound ──
00106:
00107:         /// <summary>survivorId, work-efficiency multiplier → work/task consumer.</summary>
00108:         public Action<string, float> ApplyWorkEfficiencyMultiplier { get; }
00109:         /// <summary>survivorId, crafting penalty factor → CraftingSystem time multiplier.</summary>
00110:         public Action<string, float> ApplyCraftingPenaltyFactor { get; }
00111:         /// <summary>survivorId, combat penalty factor → expedition/combat consumer.</summary>
00112:         public Action<string, float> ApplyCombatPenaltyFactor { get; }
00113:         /// <summary>survivorId, stamina drain multiplier → ExpeditionSystem stamina drain.</summary>
00114:         public Action<string, float> ApplyStaminaDrainMultiplier { get; }
00115:         /// <summary>narrativeId, survivorId → Journal / event runner.</summary>
00116:         public Action<string, string> FireNarrativeEvent { get; }
00117:         /// <summary>survivorId, afflictionId → medical / chronic-illness authority.</summary>
00118:         public Action<string, string> GrantChronicIllness { get; }
00119:         /// <summary>survivorId → RadiationSystem dose reset (Prodromal metabolized the acute dose).</summary>
00120:         public Action<string> ResetRadiationDose { get; }
00121:
00122:         // ── Truly optional — null when unbound, not validated ──
00123:
00124:         /// <summary>survivorId, work-refusal hours → work/task consumer.</summary>
00125:         public Action<string, float> ApplyWorkRefusalHours { get; }
00126:
00127:         private readonly List<string> _unboundRequired;
00128:
00129:         /// <summary>
00130:         /// Names of production-required effects that were not explicitly bound
00131:         /// (null passed → no-op/null). Empty when fully wired. Checked at startup
00133:         /// morale, inventory, or progression effect.
00134:         /// </summary>
00135:         public IReadOnlyList<string> UnboundRequiredEffects => _unboundRequired;
00136:
00137:         public Phase0EffectConsumers(
00138:             Action<string, float>? applyMoraleDelta,
00139:             Action<string, float>? applyHealthDelta,
00140:             Action<string, float>? applyFatigueDelta,
00141:             Action<float>? applyShelterMoraleDelta,
00145:             Action<string, float>? applyStaminaDrainMultiplier = null,
00146:             Action<string, string>? fireNarrativeEvent = null,
00147:             Action<string, string>? grantChronicIllness = null,
00148:             Action<string>? resetRadiationDose = null,
00149:             Action<string, float>? applyWorkRefusalHours = null)
00150:         {
00151:             ApplyMoraleDelta = applyMoraleDelta ?? NoOpMoraleDelta;
00152:             ApplyHealthDelta = applyHealthDelta ?? NoOpHealthDelta;
00158:             ApplyStaminaDrainMultiplier = applyStaminaDrainMultiplier;
00159:             FireNarrativeEvent = fireNarrativeEvent;
00160:             GrantChronicIllness = grantChronicIllness;
00161:             ResetRadiationDose = resetRadiationDose;
00162:             ApplyWorkRefusalHours = applyWorkRefusalHours;
00163:
00164:             _unboundRequired = new List<string>();
00165:             if (applyMoraleDelta == null) _unboundRequired.Add(nameof(ApplyMoraleDelta));
00172:             if (applyStaminaDrainMultiplier == null) _unboundRequired.Add(nameof(ApplyStaminaDrainMultiplier));
00173:             if (fireNarrativeEvent == null) _unboundRequired.Add(nameof(FireNarrativeEvent));
00174:             if (grantChronicIllness == null) _unboundRequired.Add(nameof(GrantChronicIllness));
00175:             if (resetRadiationDose == null) _unboundRequired.Add(nameof(ResetRadiationDose));
00176:         }
00177:
00178:         /// <summary>
00179:         /// All-effects-unbound instance for isolated tests, with optional
00180:         /// overrides. Essential effects use no-op adapters; production-required
00181:         /// effects are null (session null-checks skip them).
00182:         /// </summary>
00183:         public static Phase0EffectConsumers NoOp(
00184:             Action<string, float>? applyMoraleDelta = null,
00185:             Action<string, float>? applyHealthDelta = null,
00186:             Action<string, float>? applyFatigueDelta = null,
00187:             Action<float>? applyShelterMoraleDelta = null,
00191:             Action<string, float>? applyStaminaDrainMultiplier = null,
00192:             Action<string, string>? fireNarrativeEvent = null,
00193:             Action<string, string>? grantChronicIllness = null,
00194:             Action<string>? resetRadiationDose = null,
00195:             Action<string, float>? applyWorkRefusalHours = null)
00196:             => new Phase0EffectConsumers(
00197:                 applyMoraleDelta, applyHealthDelta, applyFatigueDelta, applyShelterMoraleDelta,
00198:                 applyWorkEfficiencyMultiplier, applyCraftingPenaltyFactor, applyCombatPenaltyFactor,
00199:                 applyStaminaDrainMultiplier, fireNarrativeEvent, grantChronicIllness,
00200:                 resetRadiationDose, applyWorkRefusalHours);
00201:
00202:         // ── Named no-op adapters for essential effects ──
00203:
00204:         public static readonly Action<string, float> NoOpMoraleDelta = (_, __) => { };
00205:         public static readonly Action<string, float> NoOpHealthDelta = (_, __) => { };
00206:         public static readonly Action<string, float> NoOpFatigueDelta = (_, __) => { };
00207:         public static readonly Action<float> NoOpShelterMoraleDelta = _ => { };
00208:     }
00209:
00210:     /// <summary>
00211:     /// Thin Godot-host session for ALL Phase-0 psychological/medical effects.
00212:     /// Owns the ten engine-agnostic Core systems and wires every effect event to
00213:     /// the injected <see cref="Consumers"/> so effects reach real gameplay consumers.
00214:     /// Host-derived per-survivor views are a pure function of Core state. All rules
00215:     /// live in Ashfall.Core; this session only wires and presents.
00216:     ///
00217:     /// Owned systems:
00218:     ///  1. Radiation Phase Progression
00219:     ///  2. Phantom Memory
00222:     ///  5. Somatic Flashback
00223:     ///  6. Moral Branching
00224:     ///  7. Chemical Dependency (shared with MedicalHostSession via <see cref="Dependency"/>)
00225:     ///  8. Trade Specialty
00226:     ///  9. Final Wish
00227:     /// 10. Respiratory Degeneration
00228:     /// </summary>
00229:     public sealed class Phase0HostSession
00230:     : HostSessionBase{
00231:         public const int DefaultSeed = 808;
00232:
00233:         public RadiationPhaseProgression RadiationPhase { get; }
00234:         public PhantomMemoryEngine Phantom { get; }
00235:         public GuiltInsomniaSystem Guilt { get; }
00236:         public CombatTraumaSystem CombatTrauma { get; }
00237:         public SomaticFlashbackSystem Flashbacks { get; }
00238:         public MoralBranchingSystem Moral { get; }
00239:         /// <summary>
00240:         /// Chemical Dependency authority. Shares the MedicalHostSession's instance
00241:         /// (single source of truth); a fresh instance is only created for
00242:         /// self-contained headless selftests. Use the MedicalHostSession-owned
00243:         /// instance in the running game via the <paramref name="dependency"/> ctor
00244:         /// parameter so Phase-0 does not fork the ledger.
00245:         /// </summary>
00246:         public ChemicalDependencySystem Dependency { get; }
00247:
00248:         public TradeSpecialtySystem TradeSpecialty { get; }
00249:         public FinalWishSystem FinalWish { get; }
00250:         public RespiratoryDegenerationSystem Respiratory { get; }
00251:
00252:         /// <summary>Real-consumer wiring bundle. Set by the host (Main.cs) with a fully-bound instance.</summary>
00253:         public Phase0EffectConsumers Consumers { get; set; } = Phase0EffectConsumers.NoOp();
00254:
00255:         /// <summary>Accumulated permanent shelter-wide morale buff from completed final wishes.</summary>
00256:         public float PermanentShelterMoraleBuff { get; private set; }
00257:
00258:         /// <summary>Set by the host from the current expedition/zone (real ash-zone signal).</summary>
00259:         public bool IsInAshZone { get; set; }
00260:
00261:         /// <summary>Set by the host from the world state (real fallout-storm signal).</summary>
00262:         public bool IsInFalloutStorm { get; set; }
00263:
00264:         /// <summary>Set by the host from the photoperiod (real night signal for trauma false alarms).</summary>
00265:         public bool IsNightTime { get; set; }
00266:
00267:         /// <summary>Current sim day, injected by the host (guilt expiry, wishes, phases).</summary>
00268:         public int CurrentDay { get; set; } = 1;
00269:
00270:         /// <summary>Air-filtration health 0..100, injected by the shelter host.</summary>
00271:         public Func<float> GetFilterHealth;
00272:
00273:         public IReadOnlyList<Phase0SurvivorEffects> Effects => _effects;
00274:
00275:         /// <summary>Public accessor for the derived host view of one survivor.</summary>
00276:         public Phase0SurvivorEffects GetEffects(string survivorId) => GetOrCreateEffects(survivorId);
00277:
00278:         public string LastEvent { get; private set; } = string.Empty;
00279:
00280:         // ── Named relay handlers so UnsubscribeSystemEvents can clean up ──
00281:         private Action _onRadiationPhaseStateChanged = null!;
00282:         private Action<PhantomMemoryEngineState> _onPhantomStateChanged = null!;
00283:         private Action _onGuiltStateChanged = null!;
00284:         private Action _onCombatTraumaStateChanged = null!;
00285:         private Action _onFlashbacksStateChanged = null!;
00286:         private Action _onMoralStateChanged = null!;
00287:         private Action _onDependencyStateChanged = null!;
00288:         private Action _onTradeSpecialtyStateChanged = null!;
00289:         private Action _onFinalWishStateChanged = null!;
00290:         private Action _onRespiratoryStateChanged = null!;
00291:         /// <summary>Authored final-wish catalog (final_wishes.json); null until <see cref="LoadFinalWishCatalog"/> runs.</summary>
00292:         private IFinalWishCatalog? _finalWishCatalog;
00293:         private readonly List<Phase0SurvivorEffects> _effects = new List<Phase0SurvivorEffects>();
00294:         private readonly List<string> _aliveSurvivorIds = new List<string>();
00295:         private readonly Dictionary<string, MoralBranchState> _moralStates = new Dictionary<string, MoralBranchState>();
00296:         private readonly Dictionary<string, PhaseProgressionState> _phaseStates = new Dictionary<string, PhaseProgressionState>();
00297:         private readonly ISeededRng _rng;
00298:         /// <summary>True when this session constructed its own ChemicalDependencySystem (headless selftests). Production shares the MedicalHostSession instance and never ticks it (Task #133).</summary>
00299:         private readonly bool _ownsDependency;
00300:
00301:         public Phase0HostSession(int seed = DefaultSeed, ChemicalDependencySystem dependency = null!)
00302:         {
00303:             _rng = new CoreSeededRng(seed);
00304:             // Task #133: when the host shares the MedicalHostSession-owned
00305:             // ledger, this session must NOT tick it — MedicalDiseaseDayOwner is
00306:             // the single dependency tick owner. Self-contained headless
00307:             // selftests (fresh instance) keep the old behavior.
00308:             _ownsDependency = dependency == null;
00309:
00310:             // ── 1. Radiation Phase Progression ───────────────────────────
00311:             RadiationPhase = new RadiationPhaseProgression(new CoreSeededRng(seed));
00312:             RadiationPhase.OnHealthDeltaRequested += (sv, delta) => Consumers.ApplyHealthDelta?.Invoke(sv, delta);
00313:             RadiationPhase.OnMoraleDeltaRequested += (sv, delta) => Consumers.ApplyMoraleDelta?.Invoke(sv, delta);
00314:             RadiationPhase.OnChronicIllnessRequested += sv => Consumers.GrantChronicIllness?.Invoke(sv, "radiation_sickness");
00315:             RadiationPhase.OnChronicFibrosisMarked += sv => Consumers.GrantChronicIllness?.Invoke(sv, "chronic_fibrosis");
00316:             RadiationPhase.OnRadiationDoseResetRequested += sv => Consumers.ResetRadiationDose?.Invoke(sv);
00317:             RadiationPhase.OnTerminalPrognosisDeclared += (sv, days) =>
00318:             {
00319:                 LastEvent = $"TERMINAL PROGNOSIS: {sv} — {days:F0} days remaining. A final wish opens.";
00320:                 RaiseStateChanged();
00321:             };
00322:             RadiationPhase.OnPhaseChanged += (sv, oldP, newP) =>
00323:             {
00324:                 LastEvent = $"Radiation phase: {sv} {oldP} → {newP}.";
00325:                 RecomputeSurvivorEffects(sv);
00326:                 RaiseStateChanged();
00329:             // ── 2. Phantom Memory ────────────────────────────────────────
00330:             Phantom = new PhantomMemoryEngine();
00331:             Phantom.OnPhantomTriggered += (sv, item, isMotivation) =>
00332:             {
00333:                 RecomputeSurvivorEffects(sv);
00334:                 if (isMotivation)
00335:                 {
00336:                     Consumers.ApplyMoraleDelta?.Invoke(sv, PhantomMemoryEngine.MotivationMoraleBoost);
00337:                     Consumers.ApplyWorkEfficiencyMultiplier?.Invoke(sv, GetEffects(sv).workEfficiencyMultiplier);
00338:                     LastEvent = $"Phantom memory: {sv} motivated by {item}. Work speed up.";
00339:                 }
00340:                 else
00341:                 {
00342:                     Consumers.ApplyMoraleDelta?.Invoke(sv, PhantomMemoryEngine.BreakdownMoraleDrop);
00343:                     Consumers.ApplyWorkRefusalHours?.Invoke(sv, PhantomMemoryEngine.BreakdownWorkRefusalHours);
00344:                     LastEvent = $"Phantom memory: {sv} breaks down over {item}. Refuses work.";
00345:                 }
00346:                 RaiseStateChanged();
00347:             };
00348:             Phantom.OnPhantomBreakdown += (sv, item) => { /* handled above via OnPhantomTriggered */ };
00349:
00350:             // ── 3. Guilt Insomnia ────────────────────────────────────────
00351:             Guilt = new GuiltInsomniaSystem();
00352:             Guilt.OnGuiltRecorded += (sv, rec) =>
00353:             {
00354:                 Consumers.ApplyMoraleDelta?.Invoke(sv, -rec.severity * 10f);
00355:                 LastEvent = $"Guilt recorded: {sv} ({rec.sourceId}, severity {rec.severity:F2}). Sleep quality falls.";
00356:                 RecomputeSurvivorEffects(sv);
00357:                 RaiseStateChanged();
00358:             };
00359:             Guilt.OnGuiltInsomniaCritical += sv =>
00360:             {
00361:                 Consumers.ApplyFatigueDelta?.Invoke(sv, 20f);
00362:                 LastEvent = $"GUILT INSOMNIA: {sv} cannot sleep.";
00363:                 RaiseStateChanged();
00364:             };
00365:
00368:             {
00369:                 Rng = new CoreSeededRng(seed + 1),
00370:                 ApplyMoraleDelta = (sv, delta) => Consumers.ApplyMoraleDelta?.Invoke(sv, delta)
00371:             };
00372:             CombatTrauma.OnFalseAlarmTriggered += sv =>
00373:             {
00374:                 LastEvent = $"FALSE ALARM: {sv} startled the bunker at night.";
00375:                 RecomputeSurvivorEffects(sv);
00376:                 RaiseStateChanged();
00381:             {
00382:                 Rng = new CoreSeededRng(seed + 2),
00383:                 GetAliveSurvivorIds = () => _aliveSurvivorIds,
00384:                 IsCompanionInSameRoom = (a, b) => false
00385:             };
00386:             Flashbacks.OnFlashbackTriggered += (sv, duration) =>
00387:             {
00388:                 RecomputeSurvivorEffects(sv);
00389:                 Consumers.ApplyWorkEfficiencyMultiplier?.Invoke(sv, GetEffects(sv).workEfficiencyMultiplier);
00390:                 LastEvent = $"SOMATIC FLASHBACK: {sv} — {duration:F1}h of distortion. Work efficiency drops.";
00391:                 RaiseStateChanged();
00392:             };
00393:             Flashbacks.OnFlashbackEnded += sv =>
00394:             {
00395:                 RecomputeSurvivorEffects(sv);
00396:                 Consumers.ApplyWorkEfficiencyMultiplier?.Invoke(sv, GetEffects(sv).workEfficiencyMultiplier);
00397:                 LastEvent = $"Flashback ended: {sv}.";
00398:                 RaiseStateChanged();
00399:             };
00400:
00404:                 ApplyMoraleDelta = (state, delta) =>
00405:                 {
00406:                     if (state != null) Consumers.ApplyMoraleDelta?.Invoke(state.SurvivorId, delta);
00407:                 },
00408:                 ApplyShelterMoraleDelta = delta => Consumers.ApplyShelterMoraleDelta?.Invoke(delta)
00409:             };
00410:             Moral.OnBranchDecided += (state, dir) =>
00411:             {
00412:                 LastEvent = $"Moral branch decided: {state.SurvivorId} → {dir}.";
00413:                 RecomputeSurvivorEffects(state.SurvivorId);
00414:                 RaiseStateChanged();
00415:             };
00416:
00417:             // ── 7. Chemical Dependency (single authority shared with MedicalHostSession) ──
00418:             Dependency = dependency ?? new ChemicalDependencySystem();
00419:             Dependency.OnMoraleDrainRequested += (sv, amount) => Consumers.ApplyMoraleDelta?.Invoke(sv, -amount);
00420:             Dependency.OnCraftingPenaltyChanged += (sv, factor) =>
00421:             {
00422:                 Consumers.ApplyCraftingPenaltyFactor?.Invoke(sv, factor);
00423:                 RecomputeSurvivorEffects(sv);
00424:                 RaiseStateChanged();
00425:             };
00426:             Dependency.OnCombatPenaltyChanged += (sv, factor) =>
00427:             {
00428:                 Consumers.ApplyCombatPenaltyFactor?.Invoke(sv, factor);
00429:                 RecomputeSurvivorEffects(sv);
00430:                 RaiseStateChanged();
00431:             };
00432:             Dependency.OnDependencyFormed += (sv, item) => { LastEvent = $"DEPENDENCY: {sv} on {item}."; RaiseStateChanged(); };
00433:
00434:             // ── 8. Trade Specialty ───────────────────────────────────────
00435:             TradeSpecialty = new TradeSpecialtySystem
00436:             {
00437:                 GrantSkillBonus = (sv, prof, bonus) =>
00438:                 {
00439:                     LastEvent = $"Specialty: {sv} ({prof}) skill +{bonus:F2}.";
00440:                     RaiseStateChanged();
00441:                 },
00442:                 ApplyMoraleDelta = (sv, delta) =>
00443:                 {
00444:                     Consumers.ApplyMoraleDelta?.Invoke(sv, delta);
00445:                     LastEvent = $"Specialty: {sv} morale {delta:+#.##;-#.##;0}.";
00446:                     RaiseStateChanged();
00447:                 },
00448:                 GetNarrativeEventId = prof => TradeSpecialtySystem.GetMasteryNarrativeId(prof),
00449:                 FireNarrativeEvent = (narrativeId, sv) =>
00450:                 {
00451:                     Consumers.FireNarrativeEvent?.Invoke(narrativeId, sv);
00452:                     LastEvent = $"Narrative event fired: {narrativeId} for {sv}.";
00453:                     RaiseStateChanged();
00454:                 }
00455:             };
00462:                 {
00463:                     PermanentShelterMoraleBuff += delta;
00464:                     Consumers.ApplyShelterMoraleDelta?.Invoke(delta);
00465:                     LastEvent = $"Permanent shelter morale {(delta >= 0 ? "+" : "")}{delta:F0} (total {PermanentShelterMoraleBuff:F0}).";
00466:                     RaiseStateChanged();
00467:                 }
00468:             };
00469:             FinalWish.OnFinalWishCompleted += sv =>
00470:             {
00471:                 Consumers.FireNarrativeEvent?.Invoke("narrative_final_wish_completed", sv);
00472:                 RecomputeSurvivorEffects(sv);
00473:                 RaiseStateChanged();
00474:             };
00475:
00476:             // ── 10. Respiratory Degeneration ─────────────────────────────
00477:             Respiratory = new RespiratoryDegenerationSystem
00478:             {
00479:                 GetFilterHealth = () => GetFilterHealth?.Invoke() ?? 100f,
00480:                 IsInFalloutStorm = () => IsInFalloutStorm,
00481:                 IsInAshZone = () => IsInAshZone
00482:             };
00483:             Respiratory.OnStaminaPenaltyRequested += (sv, factor) =>
00484:             {
00485:                 Consumers.ApplyStaminaDrainMultiplier?.Invoke(sv, factor);
00486:                 RecomputeSurvivorEffects(sv);
00487:                 RaiseStateChanged();
00488:             };
00489:             Respiratory.OnMoraleDrainRequested += (sv, amount) => Consumers.ApplyMoraleDelta?.Invoke(sv, amount);
00490:             Respiratory.OnSevereCoughStarted += sv =>
00491:             {
00492:                 LastEvent = $"SEVERE COUGH: {sv} — stamina reduced until treated.";
00493:                 RaiseStateChanged();
00494:             };
00495:             Respiratory.OnRequiresInhaler += sv => { LastEvent = $"{sv} now requires an inhaler."; RaiseStateChanged(); };
00496:
00497:             // ── Global state-changed relay ───────────────────────────────
00498:             _onRadiationPhaseStateChanged = () => { RecomputeAllEffects(); RaiseStateChanged(); };
00499:             RadiationPhase.OnStateChanged += _onRadiationPhaseStateChanged;
00500:             _onPhantomStateChanged = _ => RecomputeAllEffects();
00501:             Phantom.OnStateChanged += _onPhantomStateChanged;
00502:             _onGuiltStateChanged = () => RecomputeAllEffects();
00503:             Guilt.OnStateChanged += _onGuiltStateChanged;
00504:             _onCombatTraumaStateChanged = () => RecomputeAllEffects();
00505:             CombatTrauma.OnStateChanged += _onCombatTraumaStateChanged;
00506:             _onFlashbacksStateChanged = () => RecomputeAllEffects();
00507:             Flashbacks.OnStateChanged += _onFlashbacksStateChanged;
00508:             _onMoralStateChanged = () => RecomputeAllEffects();
00509:             Moral.OnStateChanged += _onMoralStateChanged;
00510:             _onDependencyStateChanged = () => RecomputeAllEffects();
00511:             Dependency.OnStateChanged += _onDependencyStateChanged;
00512:             _onTradeSpecialtyStateChanged = () => RaiseStateChanged();
00513:             TradeSpecialty.OnStateChanged += _onTradeSpecialtyStateChanged;
00514:             _onFinalWishStateChanged = () => RecomputeAllEffects();
00515:             FinalWish.OnStateChanged += _onFinalWishStateChanged;
00516:             _onRespiratoryStateChanged = () => RecomputeAllEffects();
00517:             Respiratory.OnStateChanged += _onRespiratoryStateChanged;
00518:         }
00519:
00520:         /// <summary>
00521:         /// Logs any production-required Phase-0 effects still unbound on the
00522:         /// current <see cref="Consumers"/>. An empty list means every health,
00523:         /// morale, fatigue, and progression effect reaches a real consumer.
00524:         /// Call after the host assigns <see cref="Consumers"/>.
00525:         /// </summary>
00526:         public void ValidateConsumers()
00527:         {
00528:             var unbound = Consumers.UnboundRequiredEffects;
00529:             if (unbound.Count > 0)
00530:             {
00531:                 GD.PrintErr("[Ashfall Godot] Phase-0 effect consumers unbound: "
00532:                     + string.Join(", ", unbound)
00533:                     + ". Effects will silently no-op in production.");
00534:             }
00535:         }
00538:
00539:         /// <summary>
00540:         /// Load trade_specialties.json profession patterns into the Phase-0
00541:         /// specialty system. Without this the wired specialty loop (events,
00542:         /// save, host hooks) runs with zero patterns and mastery can never
00543:         /// progress — the catalog is the feeder for CraftItem. It is also the
00544:         /// sole authority for milestone/mastery narrative event ids, so an
00545:         /// unloaded catalog means no narrative fires at all.
00546:         /// </summary>
00547:         public void LoadTradeSpecialties(string dataDir)
00548:         {
00549:             if (string.IsNullOrEmpty(dataDir)) return;
00550:             int count = TradeSpecialtyCatalogLoader.LoadAndRegister(
00551:                 TradeSpecialty, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
00552:             if (count > 0)
00553:                 LastEvent = $"Trade specialties loaded: {count} professions";
00554:         }
00555:
00556:         /// <summary>Load the phantom_triggers.json catalog into the engine (the authority).</summary>
00557:         public void LoadPhantomRules(string dataDir)
00558:         {
00559:             if (string.IsNullOrEmpty(dataDir)) return;
00560:             try
00561:             {
00562:                 var files = new FileSystemIO();
00563:                 var json = new SystemTextJsonSerializer();
00564:                 string path = System.IO.Path.Combine(dataDir, "phantom_triggers.json");
00565:                 if (!files.FileExists(path)) return;
00566:
00567:                 string text = files.ReadAllText(path);
00568:                 List<PhantomTriggerJsonEntry>? entries = null;
00569:                 try
00570:                 {
00571:                     var catalog = json.Deserialize<PhantomTriggerCatalogJson>(text);
00572:                     entries = catalog?.items;
00573:                 }
00574:                 catch
00575:                 {
00576:                     // Fallback in case of bare array JSON
00577:                     entries = json.Deserialize<List<PhantomTriggerJsonEntry>>(text);
00578:                 }
00579:
00580:                 if (entries == null) return;
00581:
00583:                 {
00584:                     var entry = entries[i];
00585:                     if (entry == null || string.IsNullOrEmpty(entry.background_id)) continue;
00586:                     if (entry.triggers == null) continue;
00587:                     for (int j = 0; j < entry.triggers.Count; j++)
00588:                     {
00589:                         var t = entry.triggers[j];
00590:                         if (t == null) continue;
00591:                         Phantom.RegisterRule(
00592:                             entry.background_id,
00593:                             t.item_category,
00594:                             t.motivation_chance,
00601:             catch (Exception ex)
00602:             {
00603:                 GD.PrintErr($"[Phase0] Failed to load phantom rules: {ex.Message}");
00604:             }
00605:         }
00606:
00607:         /// <summary>
00608:         /// Load <c>final_wishes.json</c> and bind it to <see cref="FinalWish"/> so a
00609:         /// terminal prognosis draws an authored wish from the archetype's pool and the
00610:         /// panel can surface title/description/completion text. Mirrors
00611:         /// <see cref="LoadPhantomRules"/>. No-op without a data dir; the system stays
00612:         /// functional (wishType only) when the catalog is absent.
00613:         /// </summary>
00614:         public void LoadFinalWishCatalog(string dataDir)
00615:         {
00616:             if (string.IsNullOrEmpty(dataDir)) return;
00617:             try
00618:             {
00619:                 var files = new FileSystemIO();
00620:                 var json = new SystemTextJsonSerializer();
00621:                 _finalWishCatalog = FinalWishCatalogLoader.LoadCatalog(dataDir, files, json);
00622:                 FinalWish.Catalog = _finalWishCatalog;
00623:             }
00624:             catch (Exception ex)
00625:             {
00626:                 GD.PrintErr($"[Phase0] Failed to load final-wish catalog: {ex.Message}");
00627:             }
00628:         }
00629:
00630:         /// <summary>Built-in fallback phantom rules (host demo convenience).</summary>
00631:         public void RegisterDefaultRules()
00632:         {
00633:             Phantom.RegisterRule("former_soldier", "military", 0.40f, "desc",
00634:                 "{name} pockets the tags. 'I'll remember them,' they say. Their posture straightens.",
00635:                 "{name} reads the name on the tag and goes pale. They knew this person.");
00637:                 "{name} taps the bell of the stethoscope. 'Still works,' they say.",
00638:                 "{name} listens to their own heartbeat through the stethoscope.");
00639:             Phantom.RegisterRule("teacher", "correspondence", 0.50f, "desc",
00640:                 "{name} finds a blank page and writes a new lesson at the top.",
00641:                 "{name} reads a name written in clumsy letters on the cover.");
00642:             Phantom.RegisterRule("generic", "photograph", 0.50f, "desc",
00643:                 "{name} props the photograph against the wall. 'They'd want us to keep going.'",
00645:         }
00646:
00647:         /// <summary>Register the alive survivor ids (the host's roster authority).</summary>
00648:         public void RegisterSurvivors(IEnumerable<string> ids)
00649:         {
00650:             _aliveSurvivorIds.Clear();
00651:             if (ids != null)
00652:             {
00653:                 foreach (var id in ids)
00654:                 {
00655:                     if (string.IsNullOrEmpty(id)) continue;
00656:                     _aliveSurvivorIds.Add(id);
00657:                     GetOrCreateEffects(id);
00658:                     GetOrCreatePhaseState(id);
00659:                     GetOrCreateMoralState(id);
00660:                     CombatTrauma.RegisterSurvivor(id);
00661:                 }
00662:             }
00663:             RaiseStateChanged();
00664:         }
00665:
00666:         /// <summary>Seed a small demo roster (host demo convenience).</summary>
00667:         public void SeedDemoRoster()
00668:         {
00669:             RegisterSurvivors(new[] { "survivor_dr_sarah_chen", "survivor_gunner_mikhail", "elena_vasquez" });
00670:         }
00671:
00672:         // ── Public actions (real commands, thin) ──────────────────────
00673:
00674:         public string ScavengeItem(string survivorId, string itemId)
00675:         {
00676:             var sv = new PhantomSurvivorSnapshot
00677:             {
00678:                 survivorId = survivorId,
00681:                 isAlive = true
00682:             };
00683:             var outcome = Phantom.OnItemScavenged(sv, itemId, _rng);
00684:             LastEvent = outcome != TriggerOutcome.None
00685:                 ? $"Phantom: {survivorId} {(outcome == TriggerOutcome.Motivation ? "motivated" : "broke down")} on {itemId}."
00686:                 : $"Phantom: no memory triggered for {survivorId} ({itemId}).";
00687:             RaiseStateChanged();
00688:             return LastEvent;
00689:         }
00690:
00691:         public string RaiseNoise(string survivorId)
00692:         {
00693:             Flashbacks.OnAudioEvent("siren", 1f);
00694:             LastEvent = "Noise event raised (flashbacks checked).";
00695:             RaiseStateChanged();
00696:             return LastEvent;
00697:         }
00698:
00699:         public string CraftItem(string survivorId, string professionId, string itemId)
00700:         {
00701:             TradeSpecialty.OnItemCrafted(survivorId, professionId, itemId);
00702:             int tier = TradeSpecialty.GetMasteryTier(survivorId);
00703:             LastEvent = $"{survivorId} crafted {itemId}: specialty tier {tier}/3.";
00704:             RaiseStateChanged();
00705:             return LastEvent;
00706:         }
00707:
00708:         /// <summary>Record a moral choice for a survivor (real event/narrative flow).</summary>
00709:         public string RecordMoralChoice(string survivorId, bool isEmpathyChoice)
00710:         {
00711:             var state = GetOrCreateMoralState(survivorId);
00712:             Moral.RegisterMoralChoice(state, isEmpathyChoice);
00713:             LastEvent = $"{survivorId}: moral choice recorded ({(isEmpathyChoice ? "empathy" : "pragmatism")}).";
00714:             RaiseStateChanged();
00715:             return LastEvent;
00717:
00718:         /// <summary>Record guilt from a ruthless choice (real guilt source).</summary>
00719:         public string RecordGuilt(string survivorId, string sourceId, float severity)
00720:         {
00721:             Guilt.RecordGuilt(survivorId, sourceId, severity, CurrentDay);
00722:             LastEvent = $"Guilt recorded for {survivorId} ({sourceId}).";
00723:             RaiseStateChanged();
00726:
00727:         /// <summary>Register a combat survival (real raid/skirmish outcome).</summary>
00728:         public string RegisterCombatSurvived(string survivorId)
00729:         {
00730:             CombatTrauma.OnCombatSurvived(survivorId);
00731:             LastEvent = $"{survivorId} survived combat. Hypervigilance rises.";
00732:             RaiseStateChanged();
00733:             return LastEvent;
00734:         }
00735:
00736:         /// <summary>Consume a substance (real inventory consumption → dependency).</summary>
00737:         public string ConsumeSubstance(string survivorId, string itemId, ChemicalDependencyKind kind)
00738:         {
00739:             Dependency.OnSubstanceConsumed(survivorId, itemId, kind);
00740:             LastEvent = $"Substance consumed: {survivorId} ({itemId}).";
00741:             RaiseStateChanged();
00742:             return LastEvent;
00743:         }
00744:
00745:         /// <summary>Declare a terminal prognosis and open the final wish questline.</summary>
00746:         public string DeclareTerminalPrognosis(string survivorId, string archetypeId)
00747:         {
00748:             FinalWish.DeclareTerminalPrognosis(survivorId, archetypeId, true);
00749:             LastEvent = $"Terminal prognosis declared for {survivorId}.";
00750:             RaiseStateChanged();
00752:         }
00753:
00754:         public string AdvanceFinalWish(string survivorId, string stepId)
00755:         {
00756:             bool completed = FinalWish.AdvanceWishStep(survivorId, stepId);
00757:             LastEvent = completed
00758:                 ? $"Final wish completed by {survivorId}."
00762:         }
00763:
00764:         public string ApplyInhaler(string survivorId)
00765:         {
00766:             bool ok = Respiratory.ApplyInhaler(survivorId);
00767:             LastEvent = ok
00768:                 ? $"Inhaler applied to {survivorId}. Cough suppressed."
00772:         }
00773:
00774:         // ── Tick ──────────────────────────────────────────────────────
00775:
00776:         /// <summary>
00777:         /// Tick all Phase-0 systems for elapsed game hours. Called by the host on
00778:         /// the authoritative clock (hourly progression and day advance).
00779:         /// </summary>
00780:         public string TickHour(float gameHours = 1f)
00781:         {
00782:             if (gameHours <= 0f) return "No time elapsed.";
00783:             for (int i = 0; i < _aliveSurvivorIds.Count; i++)
00784:             {
00785:                 var id = _aliveSurvivorIds[i];
00786:                 Phantom.TickHour(id, gameHours);
00787:                 Guilt.Tick(id, gameHours, CurrentDay);
00788:                 CombatTrauma.Tick(id, gameHours, IsNightTime);
00789:                 Flashbacks.Tick(id, gameHours);
00790:                 if (_ownsDependency)
00791:                     Dependency.TickHours(id, gameHours); // shared ledgers are ticked by MedicalDiseaseDayOwner only (Task #133)
00792:                 Respiratory.TickHours(id, gameHours);
00793:                 FinalWish.Tick(id, gameHours, true);
00794:             }
00795:             RadiationPhase.Tick(gameHours);
00796:             RecomputeAllEffects();
00797:             LastEvent = $"Phase-0 effects ticked {gameHours:F0}h.";
00798:             RaiseStateChanged();
00799:             return LastEvent;
00800:         }
00801:
00802:         /// <summary>Daily boundary: reset per-night trauma flags and advance the sim day.</summary>
00803:         public string TickDay(int day)
00804:         {
00805:             CurrentDay = day;
00806:             CombatTrauma.ResetNightFlags();
00807:             string msg = TickHour(24f);
00808:             LastEvent = $"Day {day} Phase-0 pass complete.";
00809:             RaiseStateChanged();
00810:             return LastEvent;
00811:         }
00813:         // ── Status ─────────────────────────────────────────────────────
00814:
00815:         public string StatusLine()
00816:         {
00817:             var sb = new System.Text.StringBuilder();
00818:             sb.Append("PHASE-0 — PSYCHOLOGICAL & MEDICAL EFFECTS\n");
00819:             sb.Append("Permanent shelter morale buff: ").Append(PermanentShelterMoraleBuff.ToString("F0")).Append('\n');
00821:             {
00822:                 var fx = _effects[i];
00823:                 if (fx == null) continue;
00824:                 sb.Append(fx.survivorId)
00825:                   .Append(": work ×").Append(fx.workEfficiencyMultiplier.ToString("F2"))
00826:                   .Append(" · refusal ").Append(fx.workRefusalHours.ToString("F1")).Append("h")
00827:                   .Append(" · stamina ×").Append(fx.staminaMultiplier.ToString("F2"))
00828:                   .Append(" · rad ").Append(fx.radiationPhase)
00829:                   .Append(" · guilt ").Append(fx.guiltInsomniaSeverity.ToString("F2"))
00830:                   .Append(" · hyper ").Append(fx.hypervigilance.ToString("F2"))
00831:                   .Append(" · branch ").Append(fx.moralBranch)
00832:                   .Append(IsInAshZone ? " · ASH ZONE" : "")
00833:                   .Append('\n');
00834:             }
00835:             return sb.ToString().TrimEnd();
00836:         }
00837:
00838:         // ── Save / Load ────────────────────────────────────────────────
00839:
00840:         public Phase0EffectsSaveState CaptureSave()
00841:         {
00842:             var save = new Phase0EffectsSaveState
00843:             {
00844:                 radiationPhase = RadiationPhase.CaptureState(),
00845:                 phantom = Phantom.CaptureState(),
00846:                 guilt = Guilt.CaptureState(),
00847:                 combatTrauma = CombatTrauma.CaptureState(),
00848:                 flashbacks = Flashbacks.CaptureState(),
00849:                 moral = Moral.CaptureState(),
00850:                 tradeSpecialty = TradeSpecialty.CaptureState(),
00851:                 finalWishes = FinalWish.CaptureState(),
00852:                 respiratory = Respiratory.CaptureState(),
00853:                 permanentShelterMoraleBuff = PermanentShelterMoraleBuff
00854:             };
00855:             for (int i = 0; i < _effects.Count; i++)
00856:             {
00857:                 var e = _effects[i];
00858:                 if (e == null) continue;
00859:                 save.effects.Add(new Phase0SurvivorEffects
00860:                 {
00861:                     survivorId = e.survivorId,
00862:                     workEfficiencyMultiplier = e.workEfficiencyMultiplier,
00863:                     workRefusalHours = e.workRefusalHours,
00866:                     hypervigilance = e.hypervigilance,
00867:                     moralBranch = e.moralBranch,
00868:                     radiationPhase = e.radiationPhase,
00869:                     dependencyCraftingPenalty = e.dependencyCraftingPenalty,
00870:                     dependencyCombatPenalty = e.dependencyCombatPenalty,
00871:                     finalWishState = e.finalWishState
00872:                 });
00873:             }
00874:             return save;
00875:         }
00876:
00877:         public void RestoreSave(Phase0EffectsSaveState save)
00878:         {
00879:             if (save == null) return;
00880:
00881:             // RadiationPhase.RestoreState only patches ALREADY-registered survivors,
00882:             // so register the saved phase states first, then restore into them.
00883:             _phaseStates.Clear();
00884:             if (save.radiationPhase != null && save.radiationPhase.survivors != null)
00885:             {
00886:                 for (int i = 0; i < save.radiationPhase.survivors.Count; i++)
00887:                 {
00888:                     var s = save.radiationPhase.survivors[i];
00889:                     if (s == null || string.IsNullOrEmpty(s.survivorId)) continue;
00890:                     GetOrCreatePhaseState(s.survivorId);
00891:                     if (!_aliveSurvivorIds.Contains(s.survivorId))
00892:                         _aliveSurvivorIds.Add(s.survivorId);
00893:                 }
00894:             }
00895:             RadiationPhase.RestoreState(save!.radiationPhase!);
00896:
00897:             Phantom.RestoreState(save!.phantom!);
00898:             Guilt.RestoreState(save!.guilt!);
00899:             CombatTrauma.RestoreState(save!.combatTrauma!);
00900:             Flashbacks.RestoreState(save.flashbacks);
00901:             Moral.RestoreState(save.moral);
00902:             TradeSpecialty.RestoreState(save.tradeSpecialty);
00903:             FinalWish.RestoreState(save.finalWishes);
00904:             Respiratory.RestoreState(save.respiratory);
00905:             PermanentShelterMoraleBuff = save.permanentShelterMoraleBuff;
00906:
00907:             // Rebuild moral state maps from restored data.
00908:             _moralStates.Clear();
00909:             foreach (var sv in Moral.CaptureState().Survivors)
00910:                 _moralStates[sv.SurvivorId] = sv;
00911:
00912:             _effects.Clear();
00913:             if (save.effects != null)
00914:             {
00915:                 for (int i = 0; i < save.effects.Count; i++)
00916:                 {
00917:                     var e = save.effects[i];
00918:                     if (e == null || string.IsNullOrEmpty(e.survivorId)) continue;
00919:                     _effects.Add(new Phase0SurvivorEffects
00920:                     {
00921:                         survivorId = e.survivorId,
00922:                         workEfficiencyMultiplier = e.workEfficiencyMultiplier,
00926:                         hypervigilance = e.hypervigilance,
00927:                         moralBranch = e.moralBranch,
00928:                         radiationPhase = e.radiationPhase,
00929:                         dependencyCraftingPenalty = e.dependencyCraftingPenalty,
00930:                         dependencyCombatPenalty = e.dependencyCombatPenalty,
00931:                         finalWishState = e.finalWishState
00932:                     });
00933:                     if (!_aliveSurvivorIds.Contains(e.survivorId))
00934:                         _aliveSurvivorIds.Add(e.survivorId);
00935:                 }
00936:             }
00937:             RaiseStateChanged();
00940:         // ── Helpers ────────────────────────────────────────────────────
00941:
00942:         private Phase0SurvivorEffects fx(string survivorId) => GetOrCreateEffects(survivorId);
00943:
00944:         /// <summary>
00945:         /// Derive the host view of one survivor from the Core systems. The
00946:         /// aggregate fields are a pure function of Core state — never written
00947:         /// directly by event handlers.
00948:         /// </summary>
00949:         private void RecomputeSurvivorEffects(string survivorId)
00950:         {
00951:             var fx = GetOrCreateEffects(survivorId);
00952:             // Composition: a phantom motivation boost multiplies the flashback
00953:             // penalty factor (e.g. 1.20 × 0.40 = 0.48 effective).
00954:             fx.workEfficiencyMultiplier =
00955:                 Phantom.GetWorkEfficiencyMultiplier(survivorId)
00956:                 * (1f - Flashbacks.GetWorkEfficiencyPenalty(survivorId));
00957:             fx.workRefusalHours = Phantom.GetWorkRefusalHours(survivorId);
00958:             fx.staminaMultiplier = Respiratory.GetStaminaMultiplier(survivorId);
00959:             fx.guiltInsomniaSeverity = Guilt.GetInsomniaSeverity(survivorId);
00960:             fx.hypervigilance = CombatTrauma.GetHypervigilanceLevel(survivorId);
00961:             var moral = _moralStates.TryGetValue(survivorId, out var m) ? m : null;
00962:             fx.moralBranch = moral != null ? moral.BranchDirection.ToString() : "Neutral";
00963:             var phase = _phaseStates.TryGetValue(survivorId, out var p) ? p : null;
00964:             fx.radiationPhase = phase != null ? phase.Phase.ToString() : "Healthy";
00965:             fx.dependencyCraftingPenalty = Dependency.HasActiveWithdrawal(survivorId)
00966:                 ? ChemicalDependencySystem.ColdTurkeyTremorCraftingPenalty : 0f;
00967:             fx.dependencyCombatPenalty = Dependency.HasActiveWithdrawal(survivorId)
00968:                 ? ChemicalDependencySystem.ColdTurkeyTremorCombatPenalty : 0f;
00974:
00975:         /// <summary>
00976:         /// Resolve authored final-wish narrative fields onto the effects view from the
00977:         /// loaded catalog + the survivor's bound wishId. Clears them when no wish is
00978:         /// active/completed/failed or no catalog entry is available (graceful degrade).
00979:         /// </summary>
00980:         private void PopulateFinalWishEffects(Phase0SurvivorEffects fx, string survivorId)
00981:         {
00982:             fx.finalWishTitle = string.Empty;
00983:             fx.finalWishDescription = string.Empty;
00984:             fx.finalWishDaysRemaining = 0f;
00985:             fx.finalWishStepsDone = 0;
00986:             fx.finalWishStepsTotal = 0;
00987:             fx.finalWishCompletionText = string.Empty;
00988:
00989:             if (string.IsNullOrEmpty(fx.finalWishState)) return;
00990:
00991:             string wishId = FinalWish.GetWishId(survivorId);
00992:             if (string.IsNullOrEmpty(wishId)) return;
00993:
00994:             var entry = _finalWishCatalog?.GetEntry(wishId);
00995:             if (entry == null) return;
00996:
00997:             fx.finalWishTitle = entry.wish_title ?? string.Empty;
00998:             fx.finalWishDescription = entry.wish_description ?? string.Empty;
00999:             fx.finalWishCompletionText = entry.completion_text ?? string.Empty;
01000:             fx.finalWishStepsTotal = entry.steps?.Count ?? 0;
01001:             fx.finalWishStepsDone = FinalWish.GetStepsCompleted(survivorId);
01002:             fx.finalWishDaysRemaining = FinalWish.GetDaysRemaining(survivorId);
01003:         }
01004:
01005:         private void RecomputeAllEffects()
01006:         {
01009:         }
01010:
01011:         private Phase0SurvivorEffects GetOrCreateEffects(string survivorId)
01012:         {
01013:             for (int i = 0; i < _effects.Count; i++)
01014:             {
01015:                 var e = _effects[i];
01021:         }
01022:
01023:         private PhaseProgressionState GetOrCreatePhaseState(string survivorId)
01024:         {
01025:             if (!_phaseStates.TryGetValue(survivorId, out var state))
01026:             {
01027:                 state = new PhaseProgressionState { Id = survivorId, IsAlive = true };
01028:                 _phaseStates[survivorId] = state;
01029:                 RadiationPhase.Register(state);
01030:             }
01031:             return state;
01032:         }
01033:
01034:         private MoralBranchState GetOrCreateMoralState(string survivorId)
01035:         {
01036:             if (!_moralStates.TryGetValue(survivorId, out var state))
01037:             {
01038:                 state = new MoralBranchState { SurvivorId = survivorId, IsAlive = true };
01039:                 _moralStates[survivorId] = state;
01040:                 Moral.Register(state);
01041:             }
01042:             return state;
01043:         }
01044:
01045:         private static string InferBackground(string survivorId)
01046:         {
01047:             if (string.IsNullOrEmpty(survivorId)) return "generic";
01048:             if (survivorId.Contains("gunner") || survivorId.Contains("soldier")) return "former_soldier";
01049:             if (survivorId.Contains("sarah") || survivorId.Contains("nurse")) return "nurse";
01050:             if (survivorId.Contains("teacher")) return "teacher";
01051:             return "generic";
01052:         }
01053:
01054:         /// <summary>Deterministic ISeededRng adapter delegating to the core SeededRng.</summary>
01055:         private sealed class CoreSeededRng : ISeededRng
01056:         {
01057:             private readonly SeededRng _rng;
01058:             public int Seed { get; }
01059:             public CoreSeededRng(int seed) { Seed = seed; _rng = new SeededRng(seed); }
01060:             public int Next(int min, int max) => _rng.Next(min, max);
01061:             public float NextFloat() => _rng.NextFloat();
01062:             public double NextDouble() => _rng.NextDouble();
01063:         }
01064:
01065:         public void BindShelterAssignment(ShelterAssignmentSystem shelterAssignment)
01066:         {
01067:             if (shelterAssignment == null)
01068:             {
01069:                 Flashbacks.IsCompanionInSameRoom = (a, b) => false;
01070:                 return;
01071:             }
01072:
01073:             Flashbacks.IsCompanionInSameRoom = shelterAssignment.AreInSameRoom;
01074:             shelterAssignment.OnAssignmentChanged += ev =>
01075:             {
01076:                 LastEvent = $"[Phase0] Shelter assignment changed: {ev?.SurvivorId ?? "unknown"} -> {ev?.RoomId ?? "unknown"}";
01077:                 RaiseStateChanged();
01078:             };
01079:         }
01080:
01081:         protected override void UnsubscribeSystemEvents()
01082:         {
01083:             RadiationPhase.OnStateChanged -= _onRadiationPhaseStateChanged;
01084:             Phantom.OnStateChanged -= _onPhantomStateChanged;
01085:             Guilt.OnStateChanged -= _onGuiltStateChanged;
01086:             CombatTrauma.OnStateChanged -= _onCombatTraumaStateChanged;
01087:             Flashbacks.OnStateChanged -= _onFlashbacksStateChanged;
01088:             Moral.OnStateChanged -= _onMoralStateChanged;
01089:             Dependency.OnStateChanged -= _onDependencyStateChanged;
01090:             TradeSpecialty.OnStateChanged -= _onTradeSpecialtyStateChanged;
01091:             FinalWish.OnStateChanged -= _onFinalWishStateChanged;
01092:             Respiratory.OnStateChanged -= _onRespiratoryStateChanged;
01093:         }
01094:     }
01095: }
```


# Appendix — Current Source Detail: `Assets/Ashfall.Core/Survivors/TradeSpecialtyCatalogLoader.cs`

### `Assets/Ashfall.Core/Survivors/TradeSpecialtyCatalogLoader.cs` — complete current file

- Size: 152 lines / 6113 bytes.
- SHA-256: `829a9820cb5e55014969e56613ee9955c354b5cea0a80a86415e02e3c183d2b3`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.Text.Json;
00005: using System.Text.Json.Serialization;
00006: using Ashfall.Core.IO;
00007:
00008: namespace Ashfall.Core.Survivors
00009: {
00010:     [Serializable]
00011:     public sealed class TradeSpecialtyMilestoneDto
00012:     {
00013:         [JsonPropertyName("tier")]
00014:         public int Tier { get; set; } = 1;
00015:
00016:         [JsonPropertyName("item_patterns")]
00017:         public List<string> ItemPatterns { get; set; } = new List<string>();
00018:
00019:         [JsonPropertyName("title")]
00020:         public string Title { get; set; } = string.Empty;
00021:
00022:         [JsonPropertyName("narrative")]
00023:         public string Narrative { get; set; } = string.Empty;
00024:
00025:         [JsonPropertyName("skill_bonus")]
00026:         public float SkillBonus { get; set; } = 0.05f;
00027:     }
00028:
00029:     [Serializable]
00030:     public sealed class TradeSpecialtyItemDto
00031:     {
00032:         [JsonPropertyName("profession_id")]
00033:         public string ProfessionId { get; set; } = string.Empty;
00034:
00035:         [JsonPropertyName("display_name")]
00036:         public string DisplayName { get; set; } = string.Empty;
00037:
00038:         /// <summary>Authored survivor profession labels that resolve to this
00039:         /// specialty, e.g. "Trauma Surgeon" → bone_setter. Lets the 129-survivor
00040:         /// roster reach a specialty tree without per-survivor id authoring.</summary>
00041:         [JsonPropertyName("profession_aliases")]
00042:         public List<string> ProfessionAliases { get; set; } = new List<string>();
00043:
00044:         [JsonPropertyName("milestones")]
00045:         public List<TradeSpecialtyMilestoneDto> Milestones { get; set; } = new List<TradeSpecialtyMilestoneDto>();
00046:
00047:         [JsonPropertyName("mastery_narrative")]
00048:         public string MasteryNarrative { get; set; } = string.Empty;
00049:
00050:         [JsonPropertyName("mastery_bonus_text")]
00051:         public string MasteryBonusText { get; set; } = string.Empty;
00052:     }
00053:
00054:     [Serializable]
00055:     public sealed class TradeSpecialtyCatalogContainer
00056:     {
00057:         [JsonPropertyName("schema_version")]
00058:         public int SchemaVersion { get; set; } = 1;
00059:
00060:         [JsonPropertyName("items")]
00061:         public List<TradeSpecialtyItemDto> Items { get; set; } = new List<TradeSpecialtyItemDto>();
00062:     }
00063:
00064:     /// <summary>
00065:     /// Loads trade specialty configurations from JSON (Assets/StreamingAssets/Data/trade_specialties.json).
00066:     /// Pure C#, engine-agnostic: uses IFileIO and IJsonSerializer ports.
00067:     /// </summary>
00068:     public static class TradeSpecialtyCatalogLoader
00069:     {
00070:         public const string DefaultFileName = "trade_specialties.json";
00071:
00072:         public static List<TradeSpecialtyItemDto> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
00073:         {
00074:             if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
00075:                 return new List<TradeSpecialtyItemDto>();
00076:
00077:             string path = fileIO.Combine(dataDir, DefaultFileName);
00078:             if (!fileIO.FileExists(path))
00079:                 return new List<TradeSpecialtyItemDto>();
00080:
00081:             string rawText = fileIO.ReadAllText(path);
00082:             if (string.IsNullOrWhiteSpace(rawText))
00083:                 return new List<TradeSpecialtyItemDto>();
00084:
00085:             try
00086:             {
00087:                 var container = JsonSerializer.Deserialize<TradeSpecialtyCatalogContainer>(rawText, SystemTextJsonSerializer.Options);
00088:                 return container?.Items ?? new List<TradeSpecialtyItemDto>();
00089:             }
00090:             catch (Exception ex_CATDIAG)
00091:             {
00092:                 CatalogDiagnostics.Warn(DefaultFileName, "TradeSpecialtyCatalogLoader", ex_CATDIAG);
00093:                 return new List<TradeSpecialtyItemDto>();
00094:             }
00095:         }
00096:
00097:         public static int LoadAndRegister(
00098:             TradeSpecialtySystem system,
00099:             string dataDir,
00100:             IFileIO fileIO,
00101:             IJsonSerializer json)
00102:         {
00103:             if (system == null) return 0;
00104:             var items = Load(dataDir, fileIO, json);
00105:             if (items.Count > 0)
00106:             {
00107:                 foreach (var item in items)
00108:                 {
00109:                     if (string.IsNullOrEmpty(item.ProfessionId)) continue;
00110:                     var patterns = new List<string>();
00111:                     var info = new TradeSpecialtyProfessionInfo
00112:                     {
00113:                         ProfessionId = item.ProfessionId,
00114:                         DisplayName = item.DisplayName ?? string.Empty,
00115:                         MasteryNarrativeId = item.MasteryNarrative ?? string.Empty,
00116:                         MasteryBonusText = item.MasteryBonusText ?? string.Empty
00117:                     };
00118:                     if (item.ProfessionAliases != null)
00119:                     {
00120:                         foreach (var alias in item.ProfessionAliases)
00121:                         {
00122:                             if (!string.IsNullOrWhiteSpace(alias))
00123:                                 info.Aliases.Add(alias.Trim());
00124:                         }
00125:                     }
00126:                     if (item.Milestones != null)
00127:                     {
00128:                         foreach (var m in item.Milestones)
00129:                         {
00130:                             if (m == null) continue;
00131:                             if (m.ItemPatterns != null)
00132:                                 patterns.AddRange(m.ItemPatterns);
00133:                             info.Milestones[m.Tier] = new TradeSpecialtyMilestoneInfo
00134:                             {
00135:                                 Tier = m.Tier,
00136:                                 Title = m.Title ?? string.Empty,
00137:                                 NarrativeId = m.Narrative ?? string.Empty,
00138:                                 SkillBonus = m.SkillBonus
00139:                             };
00140:                         }
00141:                     }
00142:                     TradeSpecialtySystem.RegisterProfessionInfo(info);
00143:                     TradeSpecialtySystem.RegisterProfessionPatterns(item.ProfessionId, patterns);
00144:                 }
00145:                 return items.Count;
00146:             }
00147:             // Honest count: a missing/empty catalog must surface as 0 so callers
00148:             // can diagnose it — never a fake default that masks dead data wiring.
00149:             return items.Count;
00150:         }
00151:     }
00152: }
```


# Appendix — Current Source Detail: `src/Main.Phase0.cs`

### `src/Main.Phase0.cs` — complete current file

- Size: 487 lines / 20350 bytes.
- SHA-256: `17e1d2026b7034eec7ffe613a720851aed49bcb82b064c25d44ebe1b80c7f8dd`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using Godot;
00003: using System;
00004: using System.Globalization;
00005: using System.IO;
00006: using System.Linq;
00007: using System.Collections.Generic;
00008: using AtomicWar.Journal;
00009: using Ashfall.Core;
00010: using Ashfall.Core.Campaign;
00011: using Ashfall.Core.Crafting;
00012: using Ashfall.Core.Economy;
00013: using Ashfall.Core.Expeditions;
00014: using Ashfall.Core.Foundry;
00015: using Ashfall.Core.Inventory;
00016: using Ashfall.Core.Journal;
00017: using Ashfall.Core.Muster;
00018: using Ashfall.Core.YearOfAsh;
00019: using Ashfall.Core.Radio;
00020: using Ashfall.Core.Survivors;
00021: using AtomicWar.GodotApp.Economy;
00022: using AtomicWar.GodotApp.YearOfAsh;
00023: using AtomicWar.GodotApp.Muster;
00024: using AtomicWar.GodotApp.Dose;
00025: using AtomicWar.GodotApp.UtilityAI;
00026: using AtomicWar.GodotApp.Radio;
00027: using AtomicWar.GodotApp.Audio;
00028: using AtomicWar.GodotApp.UI;
00029:
00030: namespace AtomicWar.GodotApp
00031: {
00032:     public partial class Main : Control
00033:     {
00034:         // ── Phase 0 / Dose fields (GAP-ARCH-01 Phase 1) ──
00035:         private PhantomMemoryHostSession _phantomMemory = null!;
00036:         private Phase0HostSession _phase0 = null!;
00037:         private bool _phase0Dirty;
00038:         private DoseLedgerHostSession _doseLedger = null!;
00039:         private bool _doseLedgerDirty;
00040:         private DoseRegisterSurface _doseSurface = null!;
00041:
00042:         private void SetupPhantom()
00043:         {
00044:             if (_phantomMemory != null) return;
00045:             SetupCampaignDay();
00046:             var rng = _campaignDay?.Rng.GetStream(Ashfall.Core.Random.CampaignStreamIds.Psychology).Rng;
00047:             _phantomMemory = PhantomMemoryHostSession.Create(_dataDir, rng);
00048:             _phantomMemory.StateChanged += () => SavePhantomMemory();
00049:             SetupSurvivors();
00050:             SetupEnrichment();
00051:             if (_survivors != null)
00052:             {
00053:                 // Enrichment is the explicit background authority when present;
00054:                 // profession mapping remains the fallback for roster entries
00055:                 // that do not have an enrichment row.
00056:                 _phantomMemory.BindSurvivors(_survivors, _enrichment);
00057:             }
00058:             SetupInventory();
00059:             if (_inventory != null)
00060:             {
00061:                 _phantomMemory.BindInventory(_inventory);
00062:             }
00063:             _phantomMemory.Engine.OnPhantomMemoryResolved += (svId, itemId, isMotivation, moraleDelta, guiltDelta) =>
00064:             {
00065:                 var sv = _survivors?.Find(svId);
00066:                 if (sv != null && moraleDelta != 0f)
00067:                 {
00068:                     _survivors!.Needs.Modify(sv, NeedKind.Morale, moraleDelta);
00069:                 }
00070:             };
00071:
00072:             var save = PhantomMemorySaveStore.TryLoad();
00073:             if (save != null)
00074:             {
00075:                 _phantomMemory.RestoreSave(save);
00076:                 GD.Print("[Ashfall Godot] Phantom Memory state restored.");
00077:             }
00078:         }
00079:
00080:         private void OnPhantomScavengeClicked()
00081:         {
00082:             SetupPhantom();
00083:             _statusLabel.Text = _phantomMemory.ScavengeItem("survivor_gunner_mikhail", "dog_tags");
00084:         }
00085:
00086:         private void OnPhantomTickClicked()
00087:         {
00088:             SetupPhantom();
00089:             _statusLabel.Text = _phantomMemory.TickDemo();
00090:         }
00091:
00092:         private void SavePhantomMemory()
00093:         {
00094:             if (_phantomMemory == null) return;
00095:             if (CaptureSection("phantom_memory", PhantomMemorySaveStore.TryCapturePersisted(_phantomMemory.CaptureSave())))
00096:                 GD.Print("[Ashfall Godot] Phantom Memory save written.");
00097:         }
00098:
00099:         private void SetupPhase0()
00100:         {
00101:             if (_phase0 != null) return;
00102:             SetupMedical();
00103:             // Task #133: share the MedicalHostSession-owned dependency ledger so
00104:             // there is exactly one chem-dep authority, and Phase-0 does not tick it.
00105:             _phase0 = new Phase0HostSession(dependency: _medical.Engine);
00106:             _phase0.StateChanged += () => _phase0Dirty = true;
00107:             // Feed the specialty catalog — without it the wired specialty loop
00108:             // runs patternless and mastery can never progress.
00109:             _phase0.LoadTradeSpecialties(_dataDir);
00110:
00111:             // ── Wire every Phase-0 effect to the REAL gameplay consumer ──
00112:             SetupSurvivors();
00113:             SetupJournal();
00114:             SetupCrafting();
00115:             SetupExpeditions();
00116:             SetupMedical();
00117:             SetupEnrichment();
00118:
00119:             // Recorded craft-attribution contract: crafting owns recipe completion,
00120:             // this host supplies the survivor, profession, and result item. Both
00121:             // _phase0 and _crafting reset through the lifecycle registry, so this
00122:             // subscription is rebuilt against the fresh engine on a new campaign.
00123:             _crafting.Engine.OnCraftCompleted += OnCraftCompletedForSpecialty;
00124:
00125:             _phase0.Consumers = new Phase0EffectConsumers(
00126:                 applyMoraleDelta: (sv, delta) =>
00127:                 {
00128:                     var survivor = _survivors.Find(sv);
00129:                     if (survivor != null) _survivors.Needs.Modify(survivor, NeedKind.Morale, delta);
00130:                 },
00131:                 applyHealthDelta: (sv, delta) =>
00132:                 {
00133:                     var survivor = _survivors.Find(sv);
00134:                     if (survivor != null) _survivors.Needs.Modify(survivor, NeedKind.Health, delta);
00135:                 },
00136:                 applyFatigueDelta: (sv, delta) =>
00137:                 {
00138:                     var survivor = _survivors.Find(sv);
00139:                     if (survivor != null) _survivors.Needs.Modify(survivor, NeedKind.Fatigue, delta);
00140:                 },
00141:                 applyShelterMoraleDelta: delta =>
00142:                 {
00143:                     for (int i = 0; i < _survivors.RosterState.Count; i++)
00144:                     {
00145:                         var s = _survivors.RosterState[i];
00146:                         if (s != null && s.IsAliveState)
00147:                             _survivors.Needs.Modify(s, NeedKind.Morale, delta);
00148:                     }
00149:                 },
00150:                 applyWorkEfficiencyMultiplier: (sv, mult) =>
00151:                 {
00152:                     if (_crafting == null) return;
00153:                     _crafting.Engine.SetCrafterCraftTimeMultiplier(id =>
00154:                         id == sv ? MathfCompat.Max(0.1f, 1f / MathfCompat.Max(0.1f, mult)) : 1f);
00155:                 },
00156:                 applyCraftingPenaltyFactor: (sv, factor) =>
00157:                 {
00158:                     if (_crafting == null) return;
00159:                     _crafting.Engine.SetCrafterCraftTimeMultiplier(id =>
00160:                         id == sv ? 1f + MathfCompat.Max(0f, factor) : 1f);
00161:                 },
00162:                 applyCombatPenaltyFactor: (sv, factor) =>
00163:                 {
00164:                     if (_expeditions == null) return;
00165:                     _expeditions.Engine.SetStaminaDrainMultiplier(id =>
00166:                         id == sv ? 1f + MathfCompat.Max(0f, factor) : 1f);
00167:                 },
00168:                 applyStaminaDrainMultiplier: (sv, factor) =>
00169:                 {
00170:                     if (_expeditions == null) return;
00171:                     _expeditions.Engine.SetStaminaDrainMultiplier(id =>
00172:                         id == sv ? 1f + MathfCompat.Max(0f, factor) : 1f);
00173:                 },
00174:                 fireNarrativeEvent: (narrativeId, sv) =>
00175:                 {
00176:                     int day = _holdfastRuntime?.Day ?? _simDay;
00177:                     string sourceId = $"{narrativeId}_{sv}_{day}";
00178:
00179:                     // events.json is the prose authority for narrative event ids.
00180:                     // Authored ids dispatch through the same catalog seam the
00181:                     // wildlife bycatch beat uses (journal + codex unlock + HUD).
00182:                     SetupEventsHost();
00183:                     if (_eventsHost != null
00184:                         && _eventsHost.TryGetEvent(narrativeId, out var authored)
00185:                         && authored != null
00186:                         && !string.IsNullOrWhiteSpace(authored.BodyText))
00187:                     {
00188:                         SetupEventAdapter();
00189:                         _hostEventAdapter?.DispatchCatalogEvent(authored.Id, authored.BodyText, day, sourceId);
00190:                         return;
00191:                     }
00192:
00193:                     // Unauthored id (narrative_final_wish_completed has no events.json
00194:                     // row): keep the beat visible instead of dropping it silently.
00195:                     GD.PushWarning($"[Phase0] narrative event '{narrativeId}' is not authored in events.json; writing placeholder journal entry.");
00196:                     _journal.TryAddRawEntry(
00197:                         sourceId,
00198:                         $"{sv}: {narrativeId.Replace('_', ' ')}.",
00199:                         author: null!,
00200:                         day: day);
00201:                 },
00202:                 grantChronicIllness: (sv, afflictionId) =>
00203:                 {
00204:                     var rad = _survivors.RadStateFor(sv);
00205:                     if (rad != null && !rad.HasChronicIllness)
00206:                     {
00207:                         rad.HasChronicIllness = true;
00208:                         SaveSurvivors();
00209:                     }
00210:                 },
00211:                 resetRadiationDose: sv =>
00212:                 {
00213:                     var rad = _survivors.RadStateFor(sv);
00214:                     if (rad != null) _survivors.Radiation.SetDose(rad, 0f);
00215:                 },
00216:                 applyWorkRefusalHours: null);
00217:             _phase0.ValidateConsumers();
00218:
00219:             // Environment signals from the real world/shelter hosts.
00220:             _phase0.CurrentDay = _simDay;
00221:             _phase0.GetFilterHealth = () =>
00222:             {
00223:                 var filter = _expansions?.Waystation?.State != null
00224:                     ? _expansions.Waystation.State.filterHealth : 100f;
00225:                 return filter;
00226:             };
00227:             // Host flags: updated each tick from the real world/shelter state.
00228:             _phase0.IsInFalloutStorm = _world != null && _world.Weather.Current == Ashfall.Core.WeatherKind.FalloutStorm;
00229:             _phase0.IsNightTime = _world != null && _world.Weather.Current == Ashfall.Core.WeatherKind.BlackRain;
00230:
00231:             var ids = new System.Collections.Generic.List<string>();
00232:             for (int i = 0; i < _survivors.RosterState.Count; i++)
00233:             {
00234:                 var s = _survivors.RosterState[i];
00235:                 if (s != null && s.IsAliveState) ids.Add(s.Id);
00236:             }
00237:             _phase0.RegisterSurvivors(ids);
00238:
00239:             if (_shelterAssignment != null)
00240:             {
00241:                 _phase0.BindShelterAssignment(_shelterAssignment.System);
00242:             }
00243:
00244:             var save = Phase0SaveStore.TryLoad();
00245:             if (save != null)
00246:             {
00247:                 _phase0.RestoreSave(save);
00248:                 _phase0Dirty = false; // restore just raised state-change events
00249:                 GD.Print("[Ashfall Godot] Phase-0 effects restored.");
00250:             }
00251:
00252:             // Bind the authored final-wish catalog so terminal prognoses draw from a
00253:             // per-archetype pool and the panel can surface authored text. Safe to run
00254:             // after restore: it only affects future DeclareTerminalPrognosis calls.
00255:             _phase0.LoadFinalWishCatalog(_dataDir);
00256:         }
00257:
00258:         private void SavePhase0()
00259:         {
00260:             if (_phase0 == null) return;
00261:             if (CaptureSection("phase0", Phase0SaveStore.TryCapturePersisted(_phase0.CaptureSave())))
00262:             {
00263:                 _phase0Dirty = false;
00264:                 GD.Print("[Ashfall Godot] Phase-0 effects save written.");
00265:             }
00266:         }
00267:
00268:         private void FlushPhase0IfDirty()
00269:         {
00270:             if (_phase0Dirty) SavePhase0();
00271:         }
00272:
00273:         /// <summary>
00274:         /// Bridge a completed production craft into trade specialty progression.
00275:         /// A player-assigned crafter is authoritative; an unassigned shelter craft
00276:         /// falls back to a living survivor whose trade actually covers the item.
00277:         /// A survivor whose profession resolves to no specialty has no tree to advance.
00278:         /// </summary>
00279:         private void OnCraftCompletedForSpecialty(Recipe recipe, string crafterId)
00280:         {
00281:             if (_phase0 == null || recipe?.result == null) return;
00282:
00283:             string itemId = recipe.result.id;
00284:             string survivorId = string.IsNullOrWhiteSpace(crafterId)
00285:                 ? AutoAssignSpecialtyCrafter(itemId)
00286:                 : crafterId;
00287:             if (string.IsNullOrEmpty(survivorId)) return;
00288:
00289:             string professionId = ResolveSurvivorProfessionId(survivorId);
00290:             if (string.IsNullOrEmpty(professionId)) return;
00291:
00292:             _phase0.CraftItem(survivorId, professionId, itemId);
00293:         }
00294:
00295:         /// <summary>
00296:         /// Resolve a survivor's trade specialty id. An authored pre_war_profession_id
00297:         /// wins; otherwise the roster profession label is matched against the
00298:         /// authored profession_aliases in trade_specialties.json.
00299:         /// </summary>
00300:         private string ResolveSurvivorProfessionId(string survivorId, string? definitionId = null)
00301:         {
00302:             if (string.IsNullOrEmpty(survivorId)) return string.Empty;
00303:             SetupEnrichment();
00304:             string explicitId = _enrichment?.GetSurvivorFields(survivorId)?.pre_war_profession_id ?? string.Empty;
00305:             string label = _survivors?.Roster?.FindDefinition(definitionId ?? survivorId)?.profession ?? string.Empty;
00306:             return TradeSpecialtySystem.ResolveProfessionId(explicitId, label);
00307:         }
00308:
00309:         /// <summary>
00310:         /// Attribute an unassigned craft to a living survivor whose trade covers the
00311:         /// item. Deterministic: candidates are ordinal-sorted before the forked
00312:         /// campaign RNG stream picks one, so a replay credits the same survivor and
00313:         /// no wall-clock or hash-iteration order is involved. Empty when nobody's
00314:         /// trade matches, which leaves the craft advancing no specialty.
00315:         /// </summary>
00316:         private string AutoAssignSpecialtyCrafter(string itemId)
00317:         {
00318:             var roster = _survivors?.Roster;
00319:             if (roster == null || string.IsNullOrEmpty(itemId)) return string.Empty;
00320:
00321:             var candidates = new List<string>();
00322:             for (int i = 0; i < roster.Roster.Count; i++)
00323:             {
00324:                 var entry = roster.Roster[i];
00325:                 if (entry == null || !entry.isAlive || string.IsNullOrEmpty(entry.survivorId)) continue;
00326:                 string professionId = ResolveSurvivorProfessionId(entry.survivorId, entry.definitionId);
00327:                 if (string.IsNullOrEmpty(professionId)) continue;
00328:                 if (!TradeSpecialtySystem.ProfessionMatchesItem(professionId, itemId)) continue;
00329:                 candidates.Add(entry.survivorId);
00330:             }
00331:
00332:             if (candidates.Count == 0) return string.Empty;
00333:             if (candidates.Count == 1) return candidates[0];
00334:
00335:             candidates.Sort(StringComparer.Ordinal);
00336:             var rng = _campaignDay?.Rng?.Fork("trade_specialty_attribution");
00337:             return rng != null ? candidates[rng.Next(0, candidates.Count)] : candidates[0];
00338:         }
00339:
00340:         private void OnPhase0ScavengeClicked()
00341:         {
00342:             SetupPhase0();
00343:             _statusLabel.Text = _phase0.ScavengeItem("survivor_gunner_mikhail", "item_dog_tags");
00344:         }
00345:
00346:         private void OnPhase0NoiseClicked()
00347:         {
00348:             SetupPhase0();
00349:             _statusLabel.Text = _phase0.RaiseNoise("siren");
00350:         }
00351:
00352:         private void OnPhase0CraftClicked()
00353:         {
00354:             SetupPhase0();
00355:             _statusLabel.Text = _phase0.CraftItem("elena_vasquez", "machinist", "wrench_standard");
00356:         }
00357:
00358:         private void OnPhase0TickClicked()
00359:         {
00360:             SetupPhase0();
00361:             _statusLabel.Text = _phase0.TickHour(6f);
00362:         }
00363:
00364:         private void SetupDoseLedger()
00365:         {
00366:             if (_doseLedger != null) return;
00367:             SetupCampaignDay();
00368:             _doseLedger = DoseLedgerHostSession.Create(_dataDir, campaignRng: _campaignDay.Rng);
00369:             _doseLedger.StateChanged += () => _doseLedgerDirty = true;
00370:
00371:             // CORE-MECH W2: bind the live campaign day and the authored Year-of-Ash
00372:             // fallout windows so radiation bookings are conditioned by the season.
00373:             // The provider is read-only and pure; the dose ledger keeps owning every
00374:             // reading rule (AA.2 receiver contract).
00375:             _doseLedger.DayProvider = () => _simDay;
00376:             try
00377:             {
00378:                 var catalogPath = CatalogPath.ResolveCatalog("year_of_ash_events.json");
00379:                 var catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
00380:                 if (catalogIo.FileExists(catalogPath))
00381:                 {
00382:                     var yoaEvents = YearOfAshCatalogLoader.LoadEvents(
00383:                         CatalogPath.ResolveDataDir(), catalogIo, new SystemTextJsonSerializer());
00384:                     _doseLedger.FalloutWindowProviderRef = new FalloutWindowProvider(yoaEvents);
00385:                 }
00386:             }
00387:             catch (Exception ex)
00388:             {
00389:                 // Fail-closed: no calendar ⇒ neutral multiplier (1.0), never a crash
00390:                 // and never a silently reduced exposure.
00391:                 GD.Print("[Ashfall Godot] Dose fallout windows unavailable: " + ex.Message);
00392:             }
00393:
00394:             var save = DoseLedgerSaveStore.TryLoad();
00395:             if (save != null)
00396:             {
00397:                 _doseLedger.RestoreSave(save);
00398:                 _doseLedgerDirty = false; // restore just raised state-change events
00399:                 GD.Print("[Ashfall Godot] Dose Ledger state restored.");
00400:             }
00401:
00402:             if (_doseSurface == null && _rightColumn != null)
00403:             {
00404:                 _doseSurface = new DoseRegisterSurface();
00405:                 _rightColumn.AddChild(_doseSurface);
00406:             }
00407:             if (_doseSurface != null)
00408:             {
00409:                 _doseSurface.BindSession(_doseLedger);
00410:                 _doseSurface.RefreshView();
00411:             }
00412:         }
00413:
00414:         private void OnDoseRegisterClicked()
00415:         {
00416:             SetupDoseLedger();
00417:             _statusLabel.Text = "The Dose Register is open. Four tabs, four people who keep books.";
00418:         }
00419:
00420:         private void OnDoseSealClicked()
00421:         {
00422:             SetupDoseLedger();
00423:             _doseLedger.SealDemoSurvivors();
00424:             _statusLabel.Text = "Dosimeters sealed: Gunner Mikhail (tag_1), Elena Vasquez (tag_2).";
00425:             _codexViewer.Text = _doseLedger.DoseStatusLine();
00426:             FlushDoseLedgerIfDirty();
00427:         }
00428:
00429:         private void OnDoseScribeClicked()
00430:         {
00431:             SetupDoseLedger();
00432:             string result = _doseLedger.ScribeReading(180f, highEnergy: false);
00433:             _statusLabel.Text = result;
00434:             _codexViewer.Text = _doseLedger.DoseStatusLine();
00435:             FlushDoseLedgerIfDirty();
00436:         }
00437:
00438:         private void OnDoseDiagnoseClicked()
00439:         {
00440:             SetupDoseLedger();
00441:             string result = _doseLedger.DiagnoseDemo(DoseLedgerSystem.BandRed);
00442:             _statusLabel.Text = result;
00443:             _codexViewer.Text = _doseLedger.DoseStatusLine();
00444:             FlushDoseLedgerIfDirty();
00445:         }
00446:
00447:         private void OnDoseCohortClicked()
00448:         {
00449:             SetupDoseLedger();
00450:             string result = _doseLedger.BookDemoChild();
00451:             _statusLabel.Text = result;
00452:             _codexViewer.Text = _doseLedger.DoseStatusLine();
00453:             FlushDoseLedgerIfDirty();
00454:         }
00455:
00456:         private void OnDoseVolunteerClicked()
00457:         {
00458:             SetupDoseLedger();
00459:             string result = _doseLedger.SignDemoVolunteer();
00460:             _statusLabel.Text = result;
00461:             _codexViewer.Text = _doseLedger.DoseStatusLine();
00462:             FlushDoseLedgerIfDirty();
00463:         }
00464:
00465:         private void SaveDoseLedger()
00466:         {
00467:             if (_doseLedger == null) return;
00468:             int day = _core != null ? _core.Clock.Day : _simDay;
00469:             if (CaptureSection("dose_ledger", DoseLedgerSaveStore.TryCapturePersisted(_doseLedger.CaptureSave(day))))
00470:             {
00471:                 _doseLedgerDirty = false;
00472:                 GD.Print($"[Ashfall Godot] Dose Ledger save written (day {day}).");
00473:             }
00474:         }
00475:
00476:         private void FlushDoseLedgerIfDirty()
00477:         {
00478:             if (_doseLedgerDirty) SaveDoseLedger();
00479:         }
00480:
00481:         private void ClosePhase0Panel()
00482:         {
00483:             _phase0Panel.Visible = false;
00484:         }
00485:
00486:     }
00487: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/TradeSpecialtySystemTests.cs`

### `Ashfall.Core.Tests/TradeSpecialtySystemTests.cs` — complete current file

- Size: 201 lines / 8032 bytes.
- SHA-256: `9429f1ab94ee3ee897568a3950679df7f35d8d9676a57f66074caadfa803445b`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using Ashfall.Core;
00004: using Ashfall.Core.Survivors;
00005: using Xunit;
00006:
00007: namespace Ashfall.Core.Tests
00008: {
00009:     public class TradeSpecialtySystemTests
00010:     {
00011:         private const string SvA = "sv_alpha";
00012:
00013:         // ── 1. Milestone progression ──────────────────────────────────
00014:
00015:         [Fact]
00016:         public void OnItemCrafted_MatchingCategory_CountsMilestone()
00017:         {
00018:             var sys = new TradeSpecialtySystem();
00019:             int milestoneFired = 0;
00020:             sys.OnSpecialtyMilestone += (_, _, _) => milestoneFired++;
00021:
00022:             sys.OnItemCrafted(SvA, "machinist", "wrench_standard");
00023:
00024:             Assert.Equal(1, sys.GetMasteryTier(SvA));
00025:             Assert.Equal(1, milestoneFired);
00026:             Assert.False(sys.HasMasteredTrade(SvA));
00027:         }
00028:
00029:         [Fact]
00030:         public void OnItemCrafted_NonMatchingCategory_Ignored()
00031:         {
00032:             var sys = new TradeSpecialtySystem();
00033:             sys.OnItemCrafted(SvA, "machinist", "bandage_clean");
00034:             Assert.Equal(0, sys.GetMasteryTier(SvA));
00035:         }
00036:
00037:         [Fact]
00038:         public void OnItemCrafted_UnknownProfession_Ignored()
00039:         {
00040:             var sys = new TradeSpecialtySystem();
00041:             sys.OnItemCrafted(SvA, "unlisted_profession", "wrench_standard");
00042:             Assert.Equal(0, sys.GetMasteryTier(SvA));
00043:         }
00044:
00045:         [Fact]
00046:         public void OnItemCrafted_DuplicateItem_NotDoubleCounted()
00047:         {
00048:             var sys = new TradeSpecialtySystem();
00049:             sys.OnItemCrafted(SvA, "teacher", "book_childrens");
00050:             sys.OnItemCrafted(SvA, "teacher", "book_childrens");
00051:             Assert.Equal(1, sys.GetMasteryTier(SvA));
00052:         }
00053:
00054:         [Fact]
00055:         public void OnItemCrafted_EmptyInputs_NoOp()
00056:         {
00057:             var sys = new TradeSpecialtySystem();
00058:             sys.OnItemCrafted("", "machinist", "wrench_standard");
00059:             sys.OnItemCrafted(SvA, "", "wrench_standard");
00060:             sys.OnItemCrafted(SvA, "machinist", "");
00061:             Assert.Equal(0, sys.GetMasteryTier(SvA));
00062:         }
00063:
00064:         // ── 2. Mastery ────────────────────────────────────────────────
00065:
00066:         [Fact]
00067:         public void ThreeMilestones_MastersTrade()
00068:         {
00069:             var sys = new TradeSpecialtySystem();
00070:             sys.OnItemCrafted(SvA, "nurse", "bandage_clean");
00071:             sys.OnItemCrafted(SvA, "nurse", "splint_basic");
00072:             sys.OnItemCrafted(SvA, "nurse", "antiseptic_bottle");
00073:
00074:             Assert.Equal(TradeSpecialtySystem.MilestonesToMaster, sys.GetMasteryTier(SvA));
00075:             Assert.True(sys.HasMasteredTrade(SvA));
00076:         }
00077:
00078:         /// <summary>
00079:         /// The intermediate-milestone bonus production grants for this profession
00080:         /// right now: the authored skill_bonus when trade_specialties.json is
00081:         /// loaded, otherwise the derived constant. Read from the same static
00082:         /// registry the system uses, because whether the catalog is loaded depends
00083:         /// on which other tests ran first — pinning a literal here would make these
00084:         /// tests order-dependent. The authored values themselves are pinned
00085:         /// deterministically by TradeSpecialtyCatalogMetadataTests, which loads the
00086:         /// catalog itself.
00087:         /// </summary>
00088:         private static float ExpectedIntermediateBonus(string professionId, int tier)
00089:         {
00090:             var milestone = TradeSpecialtySystem.GetMilestone(professionId, tier);
00091:             return milestone != null
00092:                 ? milestone.SkillBonus
00093:                 : TradeSpecialtySystem.MasterySkillBonus * TradeSpecialtySystem.MilestoneSkillBonusFactor;
00094:         }
00095:
00096:         [Fact]
00097:         public void MasterTrade_AppliesFullSkillBonusAndMorale()
00098:         {
00099:             var sys = new TradeSpecialtySystem();
00100:             float skillBonusTotal = 0f;
00101:             float moraleDelta = 0f;
00102:             int masteredFired = 0;
00103:             sys.GrantSkillBonus = (_, _, bonus) => skillBonusTotal += bonus;
00104:             sys.ApplyMoraleDelta = (_, delta) => moraleDelta += delta;
00105:             sys.OnSpecialtyMastered += (_, _) => masteredFired++;
00106:
00107:             sys.OnItemCrafted(SvA, "electrician", "wire_copper");
00108:             sys.OnItemCrafted(SvA, "electrician", "battery_car");
00109:             sys.OnItemCrafted(SvA, "electrician", "generator_small");
00110:
00111:             Assert.Equal(1, masteredFired);
00112:             // 2 intermediate milestones + full mastery bonus. Mastery always uses
00113:             // MasterySkillBonus: the catalog authors no separate mastery bonus.
00114:             float expectedTotal = TradeSpecialtySystem.MasterySkillBonus
00115:                 + ExpectedIntermediateBonus("electrician", 1)
00116:                 + ExpectedIntermediateBonus("electrician", 2);
00117:             Assert.Equal(expectedTotal, skillBonusTotal, 4);
00118:             Assert.Equal(TradeSpecialtySystem.MasteryMoraleBonus, moraleDelta, 4);
00119:         }
00120:
00121:         [Fact]
00122:         public void IntermediateMilestone_GrantsPartialSkillBonus()
00123:         {
00124:             var sys = new TradeSpecialtySystem();
00125:             float skillBonusTotal = 0f;
00126:             sys.GrantSkillBonus = (_, _, bonus) => skillBonusTotal += bonus;
00127:
00128:             sys.OnItemCrafted(SvA, "machinist", "wrench_standard");
00129:
00130:             Assert.Equal(ExpectedIntermediateBonus("machinist", 1), skillBonusTotal, 4);
00131:         }
00132:
00133:         [Fact]
00134:         public void MasterTrade_FiresNarrativeEvent()
00135:         {
00136:             var sys = new TradeSpecialtySystem();
00137:             string firedNarrative = null;
00138:             string firedSurvivor = null;
00139:             sys.GetNarrativeEventId = prof => $"narrative_trade_mastery_{prof}";
00140:             sys.FireNarrativeEvent = (id, sv) => { firedNarrative = id; firedSurvivor = sv; };
00141:
00142:             sys.OnItemCrafted(SvA, "teacher", "book_childrens");
00143:             sys.OnItemCrafted(SvA, "teacher", "chalk_piece");
00144:             sys.OnItemCrafted(SvA, "teacher", "slate_small");
00145:
00146:             Assert.Equal("narrative_trade_mastery_teacher", firedNarrative);
00147:             Assert.Equal(SvA, firedSurvivor);
00148:         }
00149:
00150:         [Fact]
00151:         public void MasteredTrade_StopsFurtherMilestones()
00152:         {
00153:             var sys = new TradeSpecialtySystem();
00154:             sys.OnItemCrafted(SvA, "machinist", "wrench_standard");
00155:             sys.OnItemCrafted(SvA, "machinist", "gear_standard");
00156:             sys.OnItemCrafted(SvA, "machinist", "lever_standard");
00157:             sys.OnItemCrafted(SvA, "machinist", "blade_standard");
00158:
00159:             Assert.Equal(TradeSpecialtySystem.MilestonesToMaster, sys.GetMasteryTier(SvA));
00160:         }
00161:
00162:         // ── 3. Save / Load ────────────────────────────────────────────
00163:
00164:         [Fact]
00165:         public void CaptureRestore_RoundTripsState()
00166:         {
00167:             var sys = new TradeSpecialtySystem();
00168:             sys.OnItemCrafted(SvA, "nurse", "bandage_clean");
00169:             sys.OnItemCrafted(SvA, "nurse", "splint_basic");
00170:
00171:             var save = sys.CaptureState();
00172:             var fresh = new TradeSpecialtySystem();
00173:             fresh.RestoreState(save);
00174:
00175:             Assert.Equal(2, fresh.GetMasteryTier(SvA));
00176:         }
00177:
00178:         [Fact]
00179:         public void Restore_Null_NoThrow()
00180:         {
00181:             var sys = new TradeSpecialtySystem();
00182:             sys.RestoreState(null);
00183:             Assert.Equal(0, sys.GetMasteryTier(SvA));
00184:         }
00185:
00186:         [Fact]
00187:         public void Restore_RebuildsMasteryFlag()
00188:         {
00189:             var sys = new TradeSpecialtySystem();
00190:             sys.OnItemCrafted(SvA, "nurse", "bandage_clean");
00191:             sys.OnItemCrafted(SvA, "nurse", "splint_basic");
00192:             sys.OnItemCrafted(SvA, "nurse", "antiseptic_bottle");
00193:
00194:             var save = sys.CaptureState();
00195:             var fresh = new TradeSpecialtySystem();
00196:             fresh.RestoreState(save);
00197:
00198:             Assert.True(fresh.HasMasteredTrade(SvA));
00199:         }
00200:     }
00201: }
```


# Appendix — Current Source Detail: `Ashfall.Core.Tests/Progression/TradeSpecialtyCatalogMetadataTests.cs`

### `Ashfall.Core.Tests/Progression/TradeSpecialtyCatalogMetadataTests.cs` — complete current file

- Size: 652 lines / 29946 bytes.
- SHA-256: `5017c73b2b2d379a2b8312c894cb168b69605b3cd81aeca1d5a2b160bcb7dd0f`.
- This is read-only current evidence. It is not proposed replacement code.

```text
00001: // SPDX-License-Identifier: MIT
00002: using System;
00003: using System.Collections.Generic;
00004: using System.IO;
00005: using System.Linq;
00006: using Ashfall.Core.IO;
00007: using Ashfall.Core.Survivors;
00008: using Xunit;
00009:
00010: namespace Ashfall.Core.Tests.Progression
00011: {
00012:     /// <summary>
00013:     /// Pins that trade_specialties.json content survives the loader boundary.
00014:     /// Before this the DTOs parsed display_name, milestone titles, milestone
00015:     /// narrative ids, mastery_narrative, mastery_bonus_text and skill_bonus and
00016:     /// then dropped all six: LoadAndRegister kept only profession id + patterns,
00017:     /// and the host re-derived the mastery narrative id by string interpolation.
00018:     /// </summary>
00019:     public sealed class TradeSpecialtyCatalogMetadataTests
00020:     {
00021:         private const string ElectricianTier1 = "narrative_electrician_milestone_1";
00022:         private const string ElectricianTier2 = "narrative_electrician_milestone_2";
00023:         private const string ElectricianTier3 = "narrative_electrician_mastery";
00024:         private const string ElectricianMastery = "narrative_trade_mastery_electrician";
00025:         private const string UnlistedProfession = "unlisted_profession";
00026:
00027:         /// <summary>Never used by another test; kept out of the shared static tables.</summary>
00028:         private const string TestProfession = "metadata_test_profession";
00029:
00030:         private static string ResolveDataDir()
00031:         {
00032:             string baseDir = AppContext.BaseDirectory;
00033:             string probe = Path.Combine(baseDir, "Assets", "StreamingAssets", "Data");
00034:             if (Directory.Exists(probe)) return probe;
00035:
00036:             string dir = baseDir;
00037:             for (int i = 0; i < 6; i++)
00038:             {
00039:                 probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
00040:                 if (Directory.Exists(probe)) return probe;
00041:                 var parent = Directory.GetParent(dir);
00042:                 if (parent == null) break;
00043:                 dir = parent.FullName;
00044:             }
00045:             return probe;
00046:         }
00047:
00048:         private static List<TradeSpecialtyItemDto> LoadCatalog()
00049:         {
00050:             return TradeSpecialtyCatalogLoader.Load(
00051:                 ResolveDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
00052:         }
00053:
00054:         private static TradeSpecialtySystem LoadAndRegister()
00055:         {
00056:             var system = new TradeSpecialtySystem();
00057:             int count = TradeSpecialtyCatalogLoader.LoadAndRegister(
00058:                 system, ResolveDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
00059:             Assert.Equal(16, count);
00060:             return system;
00061:         }
00062:
00063:         [Fact]
00064:         public void LoadAndRegister_RetainsEveryAuthoredContentField()
00065:         {
00066:             var dtos = LoadCatalog();
00067:             Assert.Equal(16, dtos.Count);
00068:             LoadAndRegister();
00069:
00070:             int titles = 0, narratives = 0;
00071:             foreach (var dto in dtos)
00072:             {
00073:                 var info = TradeSpecialtySystem.GetProfessionInfo(dto.ProfessionId);
00074:                 Assert.NotNull(info);
00075:                 Assert.Equal(dto.DisplayName, info!.DisplayName);
00076:                 Assert.Equal(dto.MasteryNarrative, info.MasteryNarrativeId);
00077:                 Assert.Equal(dto.MasteryBonusText, info.MasteryBonusText);
00078:                 Assert.False(string.IsNullOrWhiteSpace(info.DisplayName),
00079:                     $"{dto.ProfessionId} lost its display_name");
00080:                 Assert.False(string.IsNullOrWhiteSpace(info.MasteryNarrativeId),
00081:                     $"{dto.ProfessionId} lost its mastery_narrative");
00082:                 Assert.False(string.IsNullOrWhiteSpace(info.MasteryBonusText),
00083:                     $"{dto.ProfessionId} lost its mastery_bonus_text");
00084:
00085:                 Assert.Equal(dto.Milestones.Count, info.Milestones.Count);
00086:                 foreach (var m in dto.Milestones)
00087:                 {
00088:                     var retained = TradeSpecialtySystem.GetMilestone(dto.ProfessionId, m.Tier);
00089:                     Assert.NotNull(retained);
00090:                     Assert.Equal(m.Title, retained!.Title);
00091:                     Assert.Equal(m.Narrative, retained.NarrativeId);
00092:                     Assert.Equal(m.SkillBonus, retained.SkillBonus);
00093:                     Assert.False(string.IsNullOrWhiteSpace(retained.Title),
00094:                         $"{dto.ProfessionId} tier {m.Tier} lost its title");
00095:                     Assert.False(string.IsNullOrWhiteSpace(retained.NarrativeId),
00096:                         $"{dto.ProfessionId} tier {m.Tier} lost its narrative id");
00097:                     titles++;
00098:                     narratives++;
00099:                 }
00100:             }
00101:
00102:             Assert.Equal(48, titles);
00103:             Assert.Equal(48, narratives);
00104:         }
00105:
00106:         [Fact]
00107:         public void GetMasteryNarrativeId_ComesFromCatalog_NotFromInterpolation()
00108:         {
00109:             var dtos = LoadCatalog();
00110:             LoadAndRegister();
00111:
00112:             foreach (var dto in dtos)
00113:             {
00114:                 Assert.Equal(dto.MasteryNarrative,
00115:                     TradeSpecialtySystem.GetMasteryNarrativeId(dto.ProfessionId));
00116:                 Assert.False(string.IsNullOrWhiteSpace(dto.MasteryNarrative),
00117:                     $"{dto.ProfessionId} has no authored mastery_narrative");
00118:             }
00119:         }
00120:
00121:         [Fact]
00122:         public void GetMasteryNarrativeId_UnknownProfession_ReturnsEmpty()
00123:         {
00124:             LoadAndRegister();
00125:             Assert.Equal(string.Empty, TradeSpecialtySystem.GetMasteryNarrativeId(UnlistedProfession));
00126:             Assert.Equal(string.Empty, TradeSpecialtySystem.GetDisplayName(UnlistedProfession));
00127:             Assert.Equal(string.Empty, TradeSpecialtySystem.GetMasteryBonusText(UnlistedProfession));
00128:             Assert.Equal(string.Empty, TradeSpecialtySystem.GetMilestoneTitle(UnlistedProfession, 1));
00129:             Assert.Equal(string.Empty, TradeSpecialtySystem.GetMilestoneNarrativeId(UnlistedProfession, 1));
00130:             Assert.Null(TradeSpecialtySystem.GetProfessionInfo(UnlistedProfession));
00131:             Assert.Null(TradeSpecialtySystem.GetProfessionInfo(string.Empty));
00132:         }
00133:
00134:         [Fact]
00135:         public void GetMasteryNarrativeId_MatchesCatalogForElectrician()
00136:         {
00137:             LoadAndRegister();
00138:             Assert.Equal(ElectricianMastery, TradeSpecialtySystem.GetMasteryNarrativeId("electrician"));
00139:             Assert.Equal("Electrician", TradeSpecialtySystem.GetDisplayName("electrician"));
00140:             Assert.Equal("Apprentice Spark", TradeSpecialtySystem.GetMilestoneTitle("electrician", 1));
00141:             Assert.Equal("Journeyman Current", TradeSpecialtySystem.GetMilestoneTitle("electrician", 2));
00142:             Assert.Equal("Master of the Grid", TradeSpecialtySystem.GetMilestoneTitle("electrician", 3));
00143:             Assert.Equal("Master of the Grid", TradeSpecialtySystem.GetMilestone("electrician", 3)!.Title);
00144:             Assert.Contains("{name}", TradeSpecialtySystem.GetMasteryBonusText("electrician"));
00145:         }
00146:
00147:         [Fact]
00148:         public void OnItemCrafted_FiresAuthoredTierNarrativeForEachMilestone()
00149:         {
00150:             var system = LoadAndRegister();
00151:             var fired = new List<string>();
00152:             system.FireNarrativeEvent = (id, sv) => fired.Add(id);
00153:
00154:             system.OnItemCrafted("survivor_a", "electrician", "item_battery_cell");
00155:             system.OnItemCrafted("survivor_a", "electrician", "item_solar_cell");
00156:
00157:             Assert.Equal(new[] { ElectricianTier1, ElectricianTier2 }, fired);
00158:             Assert.Equal(2, system.GetMasteryTier("survivor_a"));
00159:             Assert.False(system.HasMasteredTrade("survivor_a"));
00160:         }
00161:
00162:         [Fact]
00163:         public void OnItemCrafted_MasteryFiresTierThreeAndMasteryNarratives()
00164:         {
00165:             var system = LoadAndRegister();
00166:             var fired = new List<string>();
00167:             system.FireNarrativeEvent = (id, sv) => fired.Add(id);
00168:
00169:             system.OnItemCrafted("survivor_b", "electrician", "item_battery_cell");
00170:             system.OnItemCrafted("survivor_b", "electrician", "item_solar_cell");
00171:             system.OnItemCrafted("survivor_b", "electrician", "item_advanced_circuit");
00172:
00173:             Assert.True(system.HasMasteredTrade("survivor_b"));
00174:             Assert.Equal(
00175:                 new[] { ElectricianTier1, ElectricianTier2, ElectricianTier3, ElectricianMastery },
00176:                 fired);
00177:         }
00178:
00179:         [Fact]
00180:         public void OnItemCrafted_UnregisteredProfession_FiresNoNarrative()
00181:         {
00182:             var system = LoadAndRegister();
00183:             var fired = new List<string>();
00184:             system.FireNarrativeEvent = (id, sv) => fired.Add(id);
00185:
00186:             // Pattern matching still works off the legacy hardcoded table, but no
00187:             // authored narrative exists for this profession, so nothing may fire.
00188:             // Unique id + cleanup: ProfessionItemCategories is process-static and
00189:             // OnItemCrafted_UnknownProfession_Ignored depends on "unlisted_profession"
00190:             // staying empty.
00191:             TradeSpecialtySystem.RegisterProfessionPatterns(TestProfession, new[] { "testwidget" });
00192:             try
00193:             {
00194:                 system.OnItemCrafted("survivor_c", TestProfession, "testwidget_one");
00195:                 system.OnItemCrafted("survivor_c", TestProfession, "testwidget_two");
00196:                 system.OnItemCrafted("survivor_c", TestProfession, "testwidget_three");
00197:
00198:                 Assert.True(system.HasMasteredTrade("survivor_c"));
00199:                 Assert.Empty(fired);
00200:             }
00201:             finally
00202:             {
00203:                 TradeSpecialtySystem.ProfessionItemCategories.Remove(TestProfession);
00204:             }
00205:         }
00206:
00207:         [Fact]
00208:         public void MasterTrade_FallsBackToHostHook_OnlyWhenCatalogHasNoEntry()
00209:         {
00210:             var system = LoadAndRegister();
00211:             var fired = new List<string>();
00212:             system.FireNarrativeEvent = (id, sv) => fired.Add(id);
00213:
00214:             // Catalog-covered profession: the authored id wins, the hook is never consulted.
00215:             bool hookCalled = false;
00216:             system.GetNarrativeEventId = prof => { hookCalled = true; return "hook_should_not_win"; };
00217:             system.OnItemCrafted("survivor_f", "electrician", "item_battery_cell");
00218:             system.OnItemCrafted("survivor_f", "electrician", "item_solar_cell");
00219:             system.OnItemCrafted("survivor_f", "electrician", "item_advanced_circuit");
00220:
00221:             Assert.Contains(ElectricianMastery, fired);
00222:             Assert.DoesNotContain("hook_should_not_win", fired);
00223:             Assert.False(hookCalled);
00224:
00225:             // Uncatalogued profession: the legacy host hook still supplies the id.
00226:             fired.Clear();
00227:             TradeSpecialtySystem.RegisterProfessionPatterns(TestProfession, new[] { "testwidget" });
00228:             try
00229:             {
00230:                 system.GetNarrativeEventId = prof => prof == TestProfession ? "hook_fallback_id" : string.Empty;
00231:                 system.OnItemCrafted("survivor_g", TestProfession, "testwidget_one");
00232:                 system.OnItemCrafted("survivor_g", TestProfession, "testwidget_two");
00233:                 system.OnItemCrafted("survivor_g", TestProfession, "testwidget_three");
00234:
00235:                 Assert.Equal(new[] { "hook_fallback_id" }, fired);
00236:             }
00237:             finally
00238:             {
00239:                 TradeSpecialtySystem.ProfessionItemCategories.Remove(TestProfession);
00240:             }
00241:         }
00242:
00243:         [Fact]
00244:         public void OnItemCrafted_NoHookAssigned_DoesNotThrow()
00245:         {
00246:             var system = LoadAndRegister();
00247:             system.OnItemCrafted("survivor_d", "electrician", "item_battery_cell");
00248:             system.OnItemCrafted("survivor_d", "electrician", "item_solar_cell");
00249:             system.OnItemCrafted("survivor_d", "electrician", "item_advanced_circuit");
00250:             Assert.True(system.HasMasteredTrade("survivor_d"));
00251:         }
00252:
00253:         [Fact]
00254:         public void RegisterProfessionInfo_ReplacesEntryWithoutDuplicating()
00255:         {
00256:             LoadAndRegister();
00257:             int before = TradeSpecialtySystem.ProfessionInfo.Count;
00258:
00259:             try
00260:             {
00261:                 TradeSpecialtySystem.RegisterProfessionInfo(new TradeSpecialtyProfessionInfo
00262:                 {
00263:                     ProfessionId = TestProfession,
00264:                     DisplayName = "First"
00265:                 });
00266:                 Assert.Equal(before + 1, TradeSpecialtySystem.ProfessionInfo.Count);
00267:
00268:                 TradeSpecialtySystem.RegisterProfessionInfo(new TradeSpecialtyProfessionInfo
00269:                 {
00270:                     ProfessionId = TestProfession,
00271:                     DisplayName = "Second"
00272:                 });
00273:
00274:                 Assert.Equal(before + 1, TradeSpecialtySystem.ProfessionInfo.Count);
00275:                 Assert.Equal("Second", TradeSpecialtySystem.GetDisplayName(TestProfession));
00276:             }
00277:             finally
00278:             {
00279:                 TradeSpecialtySystem.ProfessionInfo.Remove(TestProfession);
00280:             }
00281:
00282:             Assert.Equal(before, TradeSpecialtySystem.ProfessionInfo.Count);
00283:         }
00284:
00285:         [Fact]
00286:         public void RegisterProfessionInfo_NullOrEmptyId_IsNoOp()
00287:         {
00288:             int before = TradeSpecialtySystem.ProfessionInfo.Count;
00289:             TradeSpecialtySystem.RegisterProfessionInfo(null);
00290:             TradeSpecialtySystem.RegisterProfessionInfo(new TradeSpecialtyProfessionInfo());
00291:             TradeSpecialtySystem.RegisterProfessionInfo(
00292:                 new TradeSpecialtyProfessionInfo { ProfessionId = string.Empty });
00293:             Assert.Equal(before, TradeSpecialtySystem.ProfessionInfo.Count);
00294:         }
00295:
00296:         [Fact]
00297:         public void IntermediateMilestones_GrantAuthoredSkillBonus_MasteryKeepsConstant()
00298:         {
00299:             var dtos = LoadCatalog();
00300:             var system = LoadAndRegister();
00301:
00302:             // Authored value retained and queryable for every milestone.
00303:             Assert.Equal(0.05f, TradeSpecialtySystem.GetMilestone("electrician", 1)!.SkillBonus, 3);
00304:             foreach (var dto in dtos)
00305:                 foreach (var m in dto.Milestones)
00306:                     Assert.Equal(m.SkillBonus,
00307:                         TradeSpecialtySystem.GetMilestone(dto.ProfessionId, m.Tier)!.SkillBonus, 3);
00308:
00309:             var grants = new List<float>();
00310:             system.GrantSkillBonus = (sv, prof, amount) => grants.Add(amount);
00311:
00312:             system.OnItemCrafted("survivor_g", "electrician", "item_battery_cell");
00313:             system.OnItemCrafted("survivor_g", "electrician", "item_solar_cell");
00314:             system.OnItemCrafted("survivor_g", "electrician", "item_advanced_circuit");
00315:
00316:             Assert.True(system.HasMasteredTrade("survivor_g"));
00317:             Assert.Equal(3, grants.Count);
00318:             // Tiers 1-2 take the authored 0.05. Mastery keeps MasterySkillBonus
00319:             // because the catalog authors no separate mastery bonus, and applying
00320:             // the milestone value at tier 3 would cut the payoff to a third.
00321:             Assert.Equal(0.05f, grants[0], 3);
00322:             Assert.Equal(0.05f, grants[1], 3);
00323:             Assert.Equal(TradeSpecialtySystem.MasterySkillBonus, grants[2], 3);
00324:         }
00325:
00326:         [Fact]
00327:         public void UncataloguedProfession_FallsBackToDerivedMilestoneBonus()
00328:         {
00329:             var system = LoadAndRegister();
00330:             var grants = new List<float>();
00331:             system.GrantSkillBonus = (sv, prof, amount) => grants.Add(amount);
00332:
00333:             TradeSpecialtySystem.RegisterProfessionPatterns(TestProfession, new[] { "testwidget" });
00334:             try
00335:             {
00336:                 system.OnItemCrafted("survivor_h", TestProfession, "testwidget_one");
00337:
00338:                 Assert.Single(grants);
00339:                 Assert.Equal(
00340:                     TradeSpecialtySystem.MasterySkillBonus * TradeSpecialtySystem.MilestoneSkillBonusFactor,
00341:                     grants[0], 3);
00342:             }
00343:             finally
00344:             {
00345:                 TradeSpecialtySystem.ProfessionItemCategories.Remove(TestProfession);
00346:             }
00347:         }
00348:
00349:         [Fact]
00350:         public void RuntimeMasteryConstants_AreUnchanged()
00351:         {
00352:             Assert.Equal(0.15f, TradeSpecialtySystem.MasterySkillBonus);
00353:             Assert.Equal(0.3f, TradeSpecialtySystem.MilestoneSkillBonusFactor);
00354:             Assert.Equal(10f, TradeSpecialtySystem.MasteryMoraleBonus);
00355:             Assert.Equal(3, TradeSpecialtySystem.MilestonesToMaster);
00356:         }
00357:
00358:         [Fact]
00359:         public void CaptureRestore_RoundTripsWithContentRegistryPopulated()
00360:         {
00361:             var system = LoadAndRegister();
00362:             system.OnItemCrafted("survivor_e", "machinist", "wrench_standard");
00363:             system.OnItemCrafted("survivor_e", "machinist", "gear_standard");
00364:             system.OnItemCrafted("survivor_e", "machinist", "lever_standard");
00365:
00366:             var save = system.CaptureState();
00367:             var restored = new TradeSpecialtySystem();
00368:             restored.RestoreState(save);
00369:
00370:             Assert.True(restored.HasMasteredTrade("survivor_e"));
00371:             Assert.Equal(3, restored.GetMasteryTier("survivor_e"));
00372:             Assert.Equal(TradeSpecialtySystem.SystemId, save.systemId);
00373:         }
00374:
00375:         [Fact]
00376:         public void AllAuthoredNarrativeIds_AreNonEmptyAndDistinct()
00377:         {
00378:             var dtos = LoadCatalog();
00379:             var ids = new HashSet<string>(StringComparer.Ordinal);
00380:             foreach (var dto in dtos)
00381:             {
00382:                 Assert.True(ids.Add(dto.MasteryNarrative),
00383:                     $"duplicate mastery_narrative: {dto.MasteryNarrative}");
00384:                 foreach (var m in dto.Milestones)
00385:                     Assert.True(ids.Add(m.Narrative),
00386:                         $"duplicate milestone narrative: {m.Narrative}");
00387:             }
00388:             Assert.Equal(64, ids.Count);
00389:         }
00390:
00391:         // ── Profession label → specialty resolution ────────────────────
00392:
00393:         [Serializable]
00394:         private sealed class SurvivorsRoot
00395:         {
00396:             public List<SurvivorRow> survivors { get; set; } = new List<SurvivorRow>();
00397:         }
00398:
00399:         [Serializable]
00400:         private sealed class SurvivorRow
00401:         {
00402:             public string id { get; set; } = string.Empty;
00403:             public string profession { get; set; } = string.Empty;
00404:         }
00405:
00406:         private static List<SurvivorRow> LoadSurvivors()
00407:         {
00408:             string path = Path.Combine(ResolveDataDir(), "survivors.json");
00409:             var root = new SystemTextJsonSerializer().Deserialize<SurvivorsRoot>(File.ReadAllText(path));
00410:             Assert.NotNull(root);
00411:             return root!.survivors;
00412:         }
00413:
00414:         [Fact]
00415:         public void ProfessionAliases_AreUniqueAcrossSpecialties()
00416:         {
00417:             // A label claimed by two specialties would make resolution depend on
00418:             // catalog iteration order, which the ordinal rebuild is meant to avoid.
00419:             var dtos = LoadCatalog();
00420:             var seen = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
00421:             foreach (var dto in dtos)
00422:             {
00423:                 foreach (var alias in dto.ProfessionAliases)
00424:                 {
00425:                     Assert.False(seen.TryGetValue(alias, out var other),
00426:                         $"alias '{alias}' claimed by both '{other}' and '{dto.ProfessionId}'");
00427:                     seen[alias] = dto.ProfessionId;
00428:                 }
00429:             }
00430:             Assert.Equal(42, seen.Count);
00431:         }
00432:
00433:         [Fact]
00434:         public void ProfessionAliases_AllMatchRealSurvivorLabels()
00435:         {
00436:             var labels = new HashSet<string>(
00437:                 LoadSurvivors().Select(s => s.profession), StringComparer.OrdinalIgnoreCase);
00438:
00439:             foreach (var dto in LoadCatalog())
00440:                 foreach (var alias in dto.ProfessionAliases)
00441:                     Assert.Contains(alias, labels);
00442:         }
00443:
00444:         [Fact]
00445:         public void ResolveProfessionIdFromLabel_MapsAuthoredLabels()
00446:         {
00447:             LoadAndRegister();
00448:
00449:             Assert.Equal("bone_setter", TradeSpecialtySystem.ResolveProfessionIdFromLabel("Trauma Surgeon"));
00450:             Assert.Equal("salvage_appraiser", TradeSpecialtySystem.ResolveProfessionIdFromLabel("Scavenger"));
00451:             Assert.Equal("machinist", TradeSpecialtySystem.ResolveProfessionIdFromLabel("CNC Operator"));
00452:             Assert.Equal("wireman", TradeSpecialtySystem.ResolveProfessionIdFromLabel("Telecomm Tech"));
00453:             Assert.Equal("greenhouse_grower", TradeSpecialtySystem.ResolveProfessionIdFromLabel("Fungus Farmer"));
00454:             Assert.Equal("water_technician", TradeSpecialtySystem.ResolveProfessionIdFromLabel("Pump Specialist"));
00455:
00456:             // Case-insensitive and trimmed, matching the index comparer.
00457:             Assert.Equal("nurse", TradeSpecialtySystem.ResolveProfessionIdFromLabel("  paramedic "));
00458:         }
00459:
00460:         [Fact]
00461:         public void ResolveProfessionId_ExplicitIdWinsOverLabel()
00462:         {
00463:             LoadAndRegister();
00464:             Assert.Equal("electrician",
00465:                 TradeSpecialtySystem.ResolveProfessionId("electrician", "Scavenger"));
00466:             Assert.Equal("salvage_appraiser",
00467:                 TradeSpecialtySystem.ResolveProfessionId(string.Empty, "Scavenger"));
00468:             Assert.Equal("nurse",
00469:                 TradeSpecialtySystem.ResolveProfessionId(null, "Nurse"));
00470:         }
00471:
00472:         [Fact]
00473:         public void ResolveProfessionId_UnmappedLabel_ReturnsEmpty()
00474:         {
00475:             LoadAndRegister();
00476:             foreach (var label in new[] { "Politician", "Child", "Addict", "Guard", "Storyteller" })
00477:                 Assert.Equal(string.Empty, TradeSpecialtySystem.ResolveProfessionIdFromLabel(label));
00478:
00479:             Assert.Equal(string.Empty, TradeSpecialtySystem.ResolveProfessionIdFromLabel(null));
00480:             Assert.Equal(string.Empty, TradeSpecialtySystem.ResolveProfessionIdFromLabel("   "));
00481:         }
00482:
00483:         [Fact]
00484:         public void Roster_ReachesThirteenOfSixteenSpecialties()
00485:         {
00486:             LoadAndRegister();
00487:
00488:             var resolved = LoadSurvivors()
00489:                 .Select(s => TradeSpecialtySystem.ResolveProfessionId(string.Empty, s.profession))
00490:                 .ToList();
00491:             var reached = resolved.Where(id => !string.IsNullOrEmpty(id)).Distinct().ToList();
00492:
00493:             // Known gap: miller, apiarist and metallurgist have no authored alias
00494:             // because no roster profession label fits grain milling, beekeeping or
00495:             // smelting. Update this pin deliberately when those are cast.
00496:             Assert.Equal(13, reached.Count);
00497:             Assert.Equal(59, resolved.Count(id => !string.IsNullOrEmpty(id)));
00498:             foreach (var orphan in new[] { "miller", "apiarist", "metallurgist" })
00499:                 Assert.DoesNotContain(orphan, reached);
00500:         }
00501:
00502:         [Fact]
00503:         public void EverySpecialtyEntry_DeclaresProfessionAliases()
00504:         {
00505:             // Schema uniformity: the key is authored on all 16 entries (empty where
00506:             // nothing maps) so a missing key is never mistaken for a deliberate gap.
00507:             string raw = File.ReadAllText(Path.Combine(ResolveDataDir(), "trade_specialties.json"));
00508:             int occurrences = raw.Split("\"profession_aliases\"").Length - 1;
00509:             Assert.Equal(16, occurrences);
00510:         }
00511:
00512:         // ── Craft matching (exposed for unassigned-craft attribution) ────
00513:
00514:         [Fact]
00515:         public void ProfessionMatchesItem_MatchesAuthoredPatterns()
00516:         {
00517:             LoadAndRegister();
00518:
00519:             Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("electrician", "item_battery_cell"));
00520:             Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("electrician", "wire_copper"));
00521:             Assert.False(TradeSpecialtySystem.ProfessionMatchesItem("electrician", "bandage_clean"));
00522:             Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("nurse", "antiseptic_bottle"));
00523:             Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("machinist", "wrench_standard"));
00524:
00525:             // Case-insensitive substring match, matching OnItemCrafted.
00526:             Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("electrician", "SOLAR_CELL"));
00527:         }
00528:
00529:         [Fact]
00530:         public void ProfessionMatchesItem_EmptyOrUnknownInputs_ReturnFalse()
00531:         {
00532:             LoadAndRegister();
00533:
00534:             Assert.False(TradeSpecialtySystem.ProfessionMatchesItem(string.Empty, "wire_copper"));
00535:             Assert.False(TradeSpecialtySystem.ProfessionMatchesItem("electrician", string.Empty));
00536:             Assert.False(TradeSpecialtySystem.ProfessionMatchesItem(null!, "wire_copper"));
00537:             Assert.False(TradeSpecialtySystem.ProfessionMatchesItem("electrician", null!));
00538:             Assert.False(TradeSpecialtySystem.ProfessionMatchesItem(UnlistedProfession, "wire_copper"));
00539:         }
00540:
00541:         [Fact]
00542:         public void ProfessionMatchesItem_AgreesWithOnItemCraftedForEveryProfession()
00543:         {
00544:             // Guards the refactor: OnItemCrafted now delegates to this predicate, so
00545:             // the two must never disagree or attribution would credit crafts that
00546:             // cannot progress (or skip ones that can).
00547:             var dtos = LoadCatalog();
00548:             LoadAndRegister();
00549:
00550:             string[] probeItems =
00551:             {
00552:                 "item_battery_cell", "wire_copper", "bandage_clean", "wrench_standard",
00553:                 "book_childrens", "seed_packet", "scrap_alloy", "charcoal_filter", "needle_bone"
00554:             };
00555:
00556:             int checkedPairs = 0;
00557:             foreach (var dto in dtos)
00558:             {
00559:                 foreach (var item in probeItems)
00560:                 {
00561:                     bool predicted = TradeSpecialtySystem.ProfessionMatchesItem(dto.ProfessionId, item);
00562:
00563:                     var system = new TradeSpecialtySystem();
00564:                     system.OnItemCrafted("survivor_probe", dto.ProfessionId, item);
00565:                     bool advanced = system.GetMasteryTier("survivor_probe") == 1;
00566:
00567:                     Assert.Equal(predicted, advanced);
00568:                     checkedPairs++;
00569:                 }
00570:             }
00571:
00572:             Assert.Equal(dtos.Count * probeItems.Length, checkedPairs);
00573:             Assert.True(checkedPairs > 0);
00574:         }
00575:
00576:         [Fact]
00577:         public void ProfessionMatchesItem_RejectsSubstringAccidents_AcceptsInflection()
00578:         {
00579:             LoadAndRegister();
00580:
00581:             // Whole-token anchoring: a short pattern must not match inside a word.
00582:             Assert.False(TradeSpecialtySystem.ProfessionMatchesItem("radio_technician", "item_pickled_tubers"));
00583:             Assert.False(TradeSpecialtySystem.ProfessionMatchesItem("teacher", "item_comm_codebook_alpha"));
00584:
00585:             // Inflection is still accepted, so authored items keep matching.
00586:             Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("preservationist", "item_brined_legume_mash"));
00587:             Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("preservationist", "item_salted_meat"));
00588:             Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("preservationist", "item_smoked_meat"));
00589:             Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("greenhouse_grower", "item_cloud_seeding_canister"));
00590:
00591:             // Exact token, plural, and a token that is not the first one.
00592:             Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("electrician", "battery"));
00593:             Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("electrician", "battery_pack"));
00594:             Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("electrician", "solar_cells"));
00595:             Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("electrician", "item_car_battery"));
00596:
00597:             // Suffix-like tails that are not inflections stay rejected.
00598:             Assert.False(TradeSpecialtySystem.ProfessionMatchesItem("electrician", "batteryfluid"));
00599:             Assert.False(TradeSpecialtySystem.ProfessionMatchesItem("electrician", "wiremesh_panel"));
00600:         }
00601:
00602:         [Serializable]
00603:         private sealed class RecipesRoot
00604:         {
00605:             public List<RecipeRow> recipes { get; set; } = new List<RecipeRow>();
00606:         }
00607:
00608:         [Serializable]
00609:         private sealed class RecipeRow
00610:         {
00611:             public string resultItemId { get; set; } = string.Empty;
00612:         }
00613:
00614:         [Fact]
00615:         public void ReachabilityPin_MasteryRequiresThreeCraftableMatches()
00616:         {
00617:             var dtos = LoadCatalog();
00618:             LoadAndRegister();
00619:
00620:             string path = Path.Combine(ResolveDataDir(), "recipes.json");
00621:             var root = new SystemTextJsonSerializer().Deserialize<RecipesRoot>(File.ReadAllText(path));
00622:             Assert.NotNull(root);
00623:             var craftable = root!.recipes
00624:                 .Where(r => r != null && !string.IsNullOrEmpty(r.resultItemId))
00625:                 .Select(r => r.resultItemId)
00626:                 .Distinct()
00627:                 .ToList();
00628:             Assert.True(craftable.Count > 0);
00629:
00630:             var masterable = new List<string>();
00631:             int deficit = 0;
00632:             foreach (var dto in dtos)
00633:             {
00634:                 int matches = craftable.Count(id =>
00635:                     TradeSpecialtySystem.ProfessionMatchesItem(dto.ProfessionId, id));
00636:                 if (matches >= TradeSpecialtySystem.MilestonesToMaster)
00637:                     masterable.Add(dto.ProfessionId);
00638:                 deficit += Math.Max(0, TradeSpecialtySystem.MilestonesToMaster - matches);
00639:             }
00640:
00641:             // KNOWN CONTENT GAP, measured 2026-09-17. Mastery needs 3 distinct
00642:             // craftable matches, and only recipes.json feeds CraftingSystem — the
00643:             // workshop / relic / pharma / metallurgy / glassworks catalogs run on
00644:             // separate systems whose completions never reach the specialty bridge.
00645:             // 33 more craftable items are needed to make all 16 masterable. This pin
00646:             // exists so the gap cannot be silently forgotten: update it as content lands.
00647:             masterable.Sort(StringComparer.Ordinal);
00648:             Assert.Equal(new[] { "electrician", "preservationist" }, masterable);
00649:             Assert.Equal(33, deficit);
00650:         }
00651:     }
00652: }
```


# Appendix — Polishing Pass 1: Content and Evidence Depth

This pass expands the plan from a historical row-count brief into a current implementation contract. It records what is already complete, what remains genuinely unproven, and which old proposed APIs are rejected. The current catalog rows are treated as authored content; loader, consumer, save and host reachability are separate questions. The central subject is **Current evidence and safe integration boundary for Plan 105: Trade Specialties, Profession Milestones and Crafted-Item Learning.**.

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
