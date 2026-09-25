# Plan 04 — Workshop Relic Blueprint Catalog and Reverse-Engineering Contract

> **Rebuild status:** COMPLETE DATA EXPANSION — CURRENT INTEGRATION AND CATALOG MAINTENANCE PLAN
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

- Relic recipes are loaded through a strict result-bearing loader that validates required dependencies, maps authored fields, and reports fatal errors. The reverse-engineering system owns teardown, tool wear, restoration and output behavior.
- The current data has 39 recipes with component bills. Plan 87 separately owns narrative provenance, and Plan 190 owns runtime item lore/provenance; neither should be folded into the recipe catalog.
- Future additions must prove that every required component resolves, every research unlock is consumed, and every restored output becomes reachable through the workshop surface.

**Bounded outcome:** The requested growth target is already exceeded: `relic_recipes.json` currently contains 39 recipes, not the historical six-to-thirty target. This plan preserves the existing `RelicCatalogLoader` and `WorkshopReverseEngineeringSystem` as the only restoration authority and shifts future work to validation, reachability, and balance.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `relic_recipes.json` has schema 1 and 39 recipe rows.
- `RelicCatalogLoader.LoadWithResult` is the strict loader and reports fatal errors.
- `WorkshopReverseEngineeringSystem` and `RelicCatalog` own runtime restoration behavior.
- Existing closeouts and tests cover relic coverage and workshop reverse engineering.

**Master-authority sections applied to this rebase:**

- Part III Lane A/B
- Volume 7 catalog contract
- Volume 32 restoration-log contract

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Retire the six-to-thirty implementation objective as complete.
- Define ongoing catalog admission and reachability checks for new recipes.
- Separate restoration authority, provenance authority and research-unlock ownership explicitly.
- Require balance evidence before adding high-value relic rows.

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
| recipe schema and component references | RelicCatalogLoader | `Assets/Ashfall.Core/Crafting/RelicCatalogLoader.cs` | Strict load authority. |
| teardown, wear, restoration and outputs | WorkshopReverseEngineeringSystem | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` | Runtime restoration authority. |
| authored restoration definitions | Relic recipe catalog | `Assets/StreamingAssets/Data/relic_recipes.json` | Sole recipe definition authority. |
| post-restoration instance history | ItemLoreSystem | `Assets/Ashfall.Core/Inventory/ItemLoreSystem.cs` | Plan 190 runtime provenance; no recipe duplication. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Workshop Relic Blueprint Catalog and Reverse-Engineering Contract
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ RelicCatalogLoader
│   recipe schema and component references
│ WorkshopReverseEngineeringSystem
│   teardown, wear, restoration and outputs
│ Relic recipe catalog
│   authored restoration definitions
│ ItemLoreSystem
│   post-restoration instance history
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

1. **Preserve current state ownership.** RelicCatalogLoader owns recipe schema and component references: Strict load authority.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| recipe schema and component references | RelicCatalogLoader | `Assets/Ashfall.Core/Crafting/RelicCatalogLoader.cs` | Strict load authority. |
| teardown, wear, restoration and outputs | WorkshopReverseEngineeringSystem | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` | Runtime restoration authority. |
| authored restoration definitions | Relic recipe catalog | `Assets/StreamingAssets/Data/relic_recipes.json` | Sole recipe definition authority. |
| post-restoration instance history | ItemLoreSystem | `Assets/Ashfall.Core/Inventory/ItemLoreSystem.cs` | Plan 190 runtime provenance; no recipe duplication. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. validate recipe row
2. verify components
3. select salvage instance
4. charge tool wear and time
5. complete restoration
6. grant output through host inventory port
7. register provenance through ItemLoreSystem

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Recipe definitions are immutable catalog data.
- Restoration progress and outputs are owned by the workshop runtime.
- Provenance is instance state in ItemLoreSystem.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Every `required_components` value resolves to a real item ID.
- Research unlock IDs resolve to the research authority.
- A restored output is observable in the workshop and downstream inventory.
- A recipe cannot mutate its own definition during simulation.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- Current catalog remains authoritative.
- New rows use the exact current schema.
- No duplicate item IDs are introduced when existing outputs suffice.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Workshop progress uses existing save ownership.
- Provenance additions ride the existing ItemLore state.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Tool wear/outcomes use injected seeded RNG.
- Catalog iteration and id maps are ordinal/deterministic.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Workshop restoration completion feeds existing item/provenance consumers.
- No new event family is required for a data-only addition.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/UI/WorkshopPanel.cs
- src/UI/RoboticsWorkshopPanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Relic dossier prose remains Plan 87/190 content; recipe descriptions may link to but not duplicate it.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A required component does not exist. | RelicCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A research unlock is unreachable. | WorkshopReverseEngineeringSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A recipe restores an output with no acquisition/consumer path. | Relic recipe catalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | Provenance creates a second restoration owner. | ItemLoreSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | High-value recipes dominate workshop time without balance evidence. | RelicCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/WorkshopReverseEngineeringSystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/RelicProvenanceCatalogTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Production/Plan87_91RelicGreenhouseIntegrationTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — current census | Verify 39 rows, schema, fields and references. | Current catalog report is generated. | No production path until the owning implementation package is separately claimed. |
| 1 — ownership freeze | Document recipe/provenance/research boundaries. | No duplicate owner remains. | No production path until the owning implementation package is separately claimed. |
| 2 — admission gate | Define strict new-row requirements. | Synthetic invalid row fails clearly. | No production path until the owning implementation package is separately claimed. |
| 3 — reachability audit | Prove every output is obtainable and consumed. | No orphan output remains. | No production path until the owning implementation package is separately claimed. |
| 4 — balance review | Measure restoration time and value before more rows. | Any tuning proposal is evidence-backed. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/relic_recipes.json | DATA-ONLY future additions | Recipe authority |
| Assets/Ashfall.Core/Crafting/RelicCatalogLoader.cs | READ; MODIFY only for proven schema defect | Strict loader |
| Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs | READ ONLY | Runtime authority |
| Assets/Ashfall.Core/Inventory/ItemLoreSystem.cs | READ ONLY | Provenance authority |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Recreating a provenance manager. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Adding recipes without components. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Using blueprint counts as a quality metric. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Changing restoration math during content-only work. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No relic combat power.
- No new currency or crafting authority.
- No restoration formula rewrite.

# 23. Rollback and Recovery

- Data-only additions revert by row.
- Loader changes require fixture-backed rollback.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 39-row current state is explicit.
- Strict loader is named.
- References and reachability are testable.
- Plan 87/190 ownership is preserved.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Retire the six-to-thirty implementation objective as complete.
- Define ongoing catalog admission and reachability checks for new recipes.
- Separate restoration authority, provenance authority and research-unlock ownership explicitly.
- Require balance evidence before adding high-value relic rows.

## MUST NOT DO

- No relic combat power.
- No new currency or crafting authority.
- No restoration formula rewrite.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/WorkshopReverseEngineeringSystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/RelicProvenanceCatalogTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Production/Plan87_91RelicGreenhouseIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — current census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: recipe schema and component references → RelicCatalogLoader; teardown, wear, restoration and outputs → WorkshopReverseEngineeringSystem; authored restoration definitions → Relic recipe catalog; post-restoration instance history → ItemLoreSystem. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 04.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 04 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by RelicCatalogLoader or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Crafting/RelicCatalogLoader.cs`

### `Assets/Ashfall.Core/Crafting/RelicCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 108 lines / 4582 bytes.
- SHA-256: `b2cb95988e493475d97356aa2dd6a871c542a1e3f6722a4e99b02fc8c82e9bff`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
internal sealed class RelicJsonDto
public string relic_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public List<string>? required_components { get; set; }
public float repair_time_hours { get; set; } = 8f;
public int morale_bonus { get; set; } = 3;
public string dialogue_event_id { get; set; } = string.Empty;
public string restoration_text { get; set; } = string.Empty;
public string world_flag { get; set; } = string.Empty;
public string research_unlock_id { get; set; } = string.Empty;
public string dismantle_yield_item { get; set; } = string.Empty;
public int dismantle_yield_amount { get; set; } = 1;
public string category { get; set; } = "relic";
public static class RelicCatalogLoader
public const string FileName = "relic_recipes.json";
public static RelicCatalog Load(string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
public static CatalogLoadResult<RelicDefinition> LoadWithResult( string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs`

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


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Inventory/ItemLoreSystem.cs`

### `Assets/Ashfall.Core/Inventory/ItemLoreSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 367 lines / 14913 bytes.
- SHA-256: `b44e502a94833538a4ff3301c9782446cd49c51cf0e37db732a247c09ab972a2`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum LoreTriggerType
public enum SignificanceLevel
public sealed class ItemLoreEntry
public string LoreId { get; set; } = string.Empty;
public string ItemInstanceId { get; set; } = string.Empty;
public LoreTriggerType TriggerType { get; set; } = LoreTriggerType.Crafting;
public string Text { get; set; } = string.Empty;
public int Day { get; set; } = 1;
public string AssociatedSurvivorId { get; set; } = string.Empty;
public string AssociatedLocationId { get; set; } = string.Empty;
public sealed class ItemProvenanceChain
public string ItemInstanceId { get; set; } = string.Empty;
public string CrafterSurvivorId { get; set; } = string.Empty;
public int CraftingDay { get; set; } = 0;
public string DiscoveryLocationId { get; set; } = string.Empty;
public int DiscoveryDay { get; set; } = 0;
public string DiscoveryContext { get; set; } = string.Empty;
public List<string> OwnershipChain { get; set; } = new List<string>();
public List<string> LoreEntryIds { get; set; } = new List<string>();
public SignificanceLevel Significance { get; set; } = SignificanceLevel.Mundane;
public sealed class ItemLoreState
public int SchemaVersion { get; set; } = 1;
public int NextSequence { get; set; } = 1;
public List<ItemLoreEntry> LoreEntries { get; set; } = new List<ItemLoreEntry>();
public List<ItemProvenanceChain> Provenances { get; set; } = new List<ItemProvenanceChain>();
public sealed class ItemLoreSystem
public event Action<ItemLoreEntry>? OnLoreAdded;
public event Action<ItemProvenanceChain, SignificanceLevel>? OnSignificanceChanged;
public event Action<string, string>? OnOwnershipTransferred;
public int TrackedItemCount => _state.Provenances.Count;
public int TotalLoreEntriesCount => _state.LoreEntries.Count;
public ItemProvenanceChain RegisterItem( string itemInstanceId, string? crafterId = null, int craftingDay = 0, string? discoveryLocationId = null, int discoveryDay = 0,
public bool TransferOwnership(string itemInstanceId, string newOwnerId, int day = 1) {
public ItemLoreEntry AddLore( string itemInstanceId, LoreTriggerType trigger, string text, int day, string? survivorId = null,
public ItemProvenanceChain? GetProvenance(string itemInstanceId) {
public IReadOnlyList<ItemLoreEntry> GetLoreEntries(string itemInstanceId) {
public ItemLoreState CaptureState() => CloneState(_state);
public void RestoreState(ItemLoreState state) {
```


# Appendix B.05 — Current Code Architecture: `src/UI/WorkshopPanel.cs`

### `src/UI/WorkshopPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 506 lines / 23023 bytes.
- SHA-256: `e7dd89dee09c6646242f336611059b508f793db24a25d11f5816371c4a6943a0`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class WorkshopPanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _shelterWorkshop != null || _legacyWorkshop != null;
public void Bind( ShelterWorkshopSystem workshop, Ashfall.Core.Inventory.Inventory inventory, EquipmentConditionSystem? equipment = null, ExpeditionVehicleSystem? vehicles = null, SurvivorsHostSession? survivors = null)
public void Bind( WorkshopReverseEngineeringSystem workshop, Ashfall.Core.Inventory.Inventory inventory, SurvivorsHostSession? survivors = null) {
public void BindRelicWorkshop( WorkshopReverseEngineeringSystem workshop, Ashfall.Core.Inventory.Inventory inventory, ItemCatalog? itemCatalog = null, SurvivorsHostSession? survivors = null) {
public void Unbind() {
public override void _Ready() {
public override void _ExitTree() {
public void Open() { Visible = true; RefreshView(); }
public void RefreshView() {
```


# Appendix B.06 — Current Code Architecture: `src/UI/RoboticsWorkshopPanel.cs`

### `src/UI/RoboticsWorkshopPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 294 lines / 13881 bytes.
- SHA-256: `a50a0badac8f8efc16d8bcbd1b1779e8990d4b2f8ca9785c24c5769dd970159f`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=8; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class RoboticsWorkshopPanel : Control, IBindablePanel
public event Action? OnClose;
public event Action<string, string>? OnActionRequested;
public bool IsBound => _system != null;
public void Bind(RoboticsSystem system) { _system = system; _feedbackText = string.Empty; RefreshView(); }
public void Unbind() { _system = null; }
public override void _Ready() {
public void Open() { Visible = true; RefreshView(); }
public void Close() {
public string LastFeedback { get; private set; } = string.Empty;
public void ShowFeedback(string message, bool isFailure) {
public void RefreshView() {
public override void _UnhandledInput(InputEvent @event) {
```


# Appendix C.07 — Catalog Census: `Assets/StreamingAssets/Data/relic_recipes.json`

### `Assets/StreamingAssets/Data/relic_recipes.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 28485 bytes / 28477 characters.
- SHA-256: `0058b68b94b1a287f6b06639d910afd25c85936a3e5104141d7c67fc18d071b3`.
- Root keys: `recipes`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
recipes: min=39, max=39, observed_paths=1
recipes[].required_components: min=3, max=3, observed_paths=2
```

Representative record fields:

- `category`
- `description`
- `dialogue_event_id`
- `dismantle_yield_amount`
- `dismantle_yield_item`
- `display_name`
- `morale_bonus`
- `relic_id`
- `repair_time_hours`
- `required_components`
- `research_unlock_id`
- `restoration_text`
- `world_flag`

Representative identifiers (ordered, capped for readability):

```text
gramophone
film_projector
ham_radio
music_box
typewriter
camera
mantel_clock
sewing_machine
telescope
hand_printing_press
violin
laboratory_microscope
brass_compass
box_kite
coffee_grinder
relic_hand_crunch_battery_charger
relic_water_filter_cartridge_press
relic_signal_mirror_array
relic_thermoelectric_junction
relic_hand_drill_generator
relic_pressure_lantern_mantle
relic_mechanical_timer_switch
relic_improvised_anemometer
relic_micro_dosimeter_pen
relic_water_condenser_coil
relic_signal_amplifier_stage
relic_battery_reconditioner
relic_hydroponic_nutrient_doser
relic_uv_sterilizer_wand
relic_hand_centrifuge
relic_seismic_geophone
relic_automated_turret_controller
relic_field_encrypted_radio
relic_portable_radar_scope
relic_power_armor_servo
relic_vault_seal_breach_charges
relic_iff_transponder
relic_cbrn_filter_bank
relic_field_surgical_robot_arm
```


# Appendix C.08 — Catalog Census: `Assets/StreamingAssets/Data/narrative/relic_provenance_dossiers.json`

### `Assets/StreamingAssets/Data/narrative/relic_provenance_dossiers.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 25448 bytes / 25446 characters.
- SHA-256: `8d1240cd36f1ce80b5d347a66ecea0a1adbd9c995d79b981bbf0fd70fb496695`.
- Root keys: `collection_id`, `relics`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
relics: min=32, max=32, observed_paths=1
relics[].tags: min=5, max=5, observed_paths=2
```

Representative record fields:

- `curator_note`
- `discovery_location`
- `gameplay_effect`
- `material`
- `name`
- `relic_id`
- `tags`
- `tone`

Representative identifiers (ordered, capped for readability):

```text
relic_01_reverse_pocketwatch
relic_02_indestructible_processed_cheese
relic_03_hydraulic_exo_gauntlet
relic_04_lead_lined_music_box
relic_05_frozen_moth_cylinder
relic_06_inflatable_rubber_flamingo
relic_07_tungsten_harpoon_point
relic_08_vel_stained_scalpel
relic_09_the_singing_vacuum_tube
relic_10_the_disco_cassette
relic_11_armored_snow_sled
relic_12_the_black_parchment_contract
relic_13_the_mercury_barometer_flask
relic_14_the_taxidermy_badger
relic_15_portable_gamma_spectrometer
relic_16_the_charred_accordion
relic_17_the_phantom_compass
relic_18_the_novelty_alarm_clock
relic_19_heavy_salvage_plasma_torch
relic_20_the_first_harvest_ear_of_rye
relic_21_the_lead_encased_lens
relic_22_the_rubber_chicken_decoy
relic_23_solid_fuel_cutting_lance
relic_24_the_unspoken_triage_list
relic_25_the_telegraph_key_in_amber
relic_26_the_indestructible_bowler_hat
relic_27_pneumatic_rail_spear
relic_28_the_lead_coffin_nameplate
relic_29_the_glow_in_the_dark_dice
relic_30_the_battery_powered_bubble_gun
relic_31_hydraulic_mine_jack
relic_32_the_valley_constitution_chisel
```


# Appendix D.09 — Existing Focused Test Inventory: `Ashfall.Core.Tests/WorkshopReverseEngineeringSystemTests.cs`

### `Ashfall.Core.Tests/WorkshopReverseEngineeringSystemTests.cs`

- Current test declarations: Fact=17, Theory=0, InlineData=0.
- File lines: 309; SHA-256: `a2eaf2aa8f89cdbfad936009a13666c975fcb7c1e1be214af1561382fdfdc95a`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Examine_ReturnsRelicInfo
Examine_UnknownRelic_Fails
StartDismantle_BeginsJob
StartDismantle_WhenBusy_Blocks
Dismantle_CompletesAndYieldsItems
StartRepair_WithoutComponents_Blocks
StartRepair_WithComponents_ReservesThem
StartRepair_LateComponentMissing_LeavesAllComponentsInInventory
Repair_CompletesAndSetsFlag
StartResearch_UnlocksKnowledgeNode
StartResearch_NoUnlockId_Blocks
TickProgress_ReportsProgress
CancelJob_RefundsComponents
CancelJob_WhenIdle_Blocks
CaptureRestoreState_PreservesCompletedRelics
CaptureRestoreState_PreservesActiveJob
LoadCatalog_PopulatesRelics
```


# Appendix D.10 — Existing Focused Test Inventory: `Ashfall.Core.Tests/RelicProvenanceCatalogTests.cs`

### `Ashfall.Core.Tests/RelicProvenanceCatalogTests.cs`

- Current test declarations: Fact=2, Theory=0, InlineData=0.
- File lines: 81; SHA-256: `2c9c709a0ba0c5e0f0a42dd8f3b85c43f93785bbc0ec53d8194fb387d4ea7502`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RelicProvenance_LoadsAll32MasterDossiers
RelicProvenance_AllEntriesHaveValidCuratorNotesAndEffects
```


# Appendix D.11 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Production/Plan87_91RelicGreenhouseIntegrationTests.cs`

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


# Appendix E.12 — Supporting Code Evidence: `Assets/Ashfall.Core/Inventory/InventoryTransaction.cs`

### `Assets/Ashfall.Core/Inventory/InventoryTransaction.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 387 lines / 14478 bytes.
- SHA-256: `2efcf72bae930c0c8ddc43036f44cb41ea02a76a4445233c86319df02578ecdb`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public readonly struct InventoryBillItem : IEquatable<InventoryBillItem>
public string ItemId { get; }
public int Amount { get; }
public ItemDefinition? Definition { get; }
public bool Equals(InventoryBillItem other) =>
public override bool Equals(object? obj) => obj is InventoryBillItem other && Equals(other);
public override int GetHashCode() => StableHash.Combine(StableHash.Of(ItemId), Amount);
public override string ToString() => $"{Amount}x {ItemId}";
public sealed class InventoryBill
public IReadOnlyList<InventoryBillItem> Costs => _costs;
public IReadOnlyList<InventoryBillItem> Grants => _grants;
public bool IsEmpty => _costs.Count == 0 && _grants.Count == 0;
public InventoryBill AddCost(string itemId, int amount, ItemDefinition? def = null) {
public InventoryBill AddCost(ItemDefinition def, int amount) {
public InventoryBill AddGrant(string itemId, int amount, ItemDefinition? def = null) {
public InventoryBill AddGrant(ItemDefinition def, int amount) {
public Dictionary<string, int> GetAggregatedCosts() {
public Dictionary<string, int> GetAggregatedGrants() {
public static InventoryBill FromCosts(IReadOnlyDictionary<string, int> costs) {
public static InventoryBill FromCosts(IEnumerable<KeyValuePair<string, int>> costs) {
public static InventoryBill FromCosts(IEnumerable<string> itemIds) {
public static InventoryBill FromCostsAndGrants( IReadOnlyDictionary<string, int>? costs, IReadOnlyDictionary<string, int>? grants) {
public enum InventoryTransactionStatus
public sealed class InventoryTransactionValidationResult
public bool IsValid => Status == InventoryTransactionStatus.Success;
public InventoryTransactionStatus Status { get; }
public string FailureReason { get; }
public string FailedItemId { get; }
public int RequiredAmount { get; }
public int AvailableAmount { get; }
public static InventoryTransactionValidationResult Success() =>
public static InventoryTransactionValidationResult Insufficient(string itemId, int required, int available) =>
public static InventoryTransactionValidationResult CapacityExceeded(int requiredSlots, int availableSlots) =>
public static InventoryTransactionValidationResult WeightExceeded(float currentWeight, float addedWeight, float maxWeight) =>
public static InventoryTransactionValidationResult Invalid(string reason) =>
public static InventoryTransactionValidationResult Cancelled() =>
public static InventoryTransactionValidationResult CallbackError(string message) =>
public override string ToString() => IsValid ? "Valid" : $"{Status}: {FailureReason}";
public sealed class InventoryTransactionQuote
public InventoryBill Bill { get; }
public IReadOnlyDictionary<string, int> AggregatedCosts { get; }
public IReadOnlyDictionary<string, int> AggregatedGrants { get; }
public float TotalCostWeight { get; }
public float TotalGrantWeight { get; }
public float NetWeightChange => TotalGrantWeight - TotalCostWeight;
public InventoryTransactionValidationResult Validation { get; }
public bool CanExecute => Validation.IsValid;
internal sealed class InventorySnapshot
public int Capacity { get; }
public float MaxWeight { get; }
public List<InventorySlot> Slots { get; }
public List<EquippedItem> Equipped { get; }
public sealed class InventoryTransaction : IDisposable
public InventoryBill Bill { get; }
public InventoryTransactionValidationResult Validation { get; }
public bool IsCommitted => _isCommitted;
public bool IsCancelled => _isCancelled;
public bool IsActive => !_isCommitted && !_isCancelled;
public bool TryCommit(Action? onCommitted = null) {
public void Cancel() {
public void Dispose() {
```


# Appendix E.13 — Supporting Code Evidence: `Assets/Ashfall.Core/Inventory/Inventory.cs`

### `Assets/Ashfall.Core/Inventory/Inventory.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1352 lines / 53625 bytes.
- SHA-256: `38b5c11e1b92bf7d767b47beb3ff290b3dfc8360233858a7dd6e2854c3e264a9`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=29; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public struct EquippedGearData
public float RadProtection;
public float MaxDurability;
public float CurrentDurability;
public float DegradeRate;
public class WornGear
public EquippedItem? SourceEquipped;
public IEquipmentConditionSink? ConditionSink;
public ItemDefinition? SourceItem;
public float RadProtection;
public float MaxDurability;
public float CurrentDurability;
public float DegradeRate;
public Action<float>? OnDegraded;
public float DurabilityFraction() {
public float EffectiveProtection() {
public void Degrade(float gameHours) {
public class Inventory : IPlayerInventoryPort, IEquipmentConditionSink
public const float ContaminationDosePerUnit = 50f;
public int Capacity = 20;
public float MaxWeight = 100f;
public IReadOnlyList<InventorySlot> Slots => _slots;
public IReadOnlyList<EquippedItem> Equipped => _equipped;
public IReadOnlyList<InventorySlot> GetSlots() => _slots;
public event Action<ItemDefinition, int> OnItemAdded;
public event Action<ItemDefinition, int> OnItemRemoved;
public event Action OnInventoryChanged;
public bool HasSufficient(string itemId, int count) {
public bool TryConsume(string itemId, int count, Action? onCommitted = null) {
public bool TryConsumeById(string itemId, int count) => TryConsume(itemId, count);
public bool TryProduce(string itemId, int count, ItemDefinition? def = null) {
public bool Remove(string itemId, int amount) => RemoveById(itemId, amount);
public int Count(ItemDefinition item) {
public int CountById(string itemId) {
public int CountByType(ItemType type) {
public int RemoveByType(ItemType type, int amount) {
public float FoodFillRatio() {
public float WaterFillRatio() {
public float FuelFillRatio() {
public InventorySlot? FindSlot(string itemId) {
public InventorySlot? FindBestWorkingDevice(string itemId) {
public bool HasWorkingGeiger() {
public DeviceState? GetBestGeigerState() {
public void DriftAllDevices(float days = 1f) {
public bool RechargeDevice(string deviceItemId, ItemDefinition batteryItem) {
public bool RecalibrateDevice(string deviceItemId, ItemDefinition kitItem, int currentDay) {
public float GetCurrentWeight() {
public bool CanAdd(ItemDefinition item, int amount) {
public bool Add(ItemDefinition item, int amount) {
public bool AddById(string itemId, int amount) {
public bool CanAddById(string itemId, int amount) {
public bool Remove(ItemDefinition item, int amount) {
public bool RemoveById(string itemId, int amount) {
public void Clear() {
public InventoryTransactionValidationResult ValidateTransaction( InventoryBill bill, Func<string, ItemDefinition?>? lookup = null) {
public InventoryTransactionQuote Quote( InventoryBill bill, Func<string, ItemDefinition?>? lookup = null) {
public InventoryTransactionQuote QuoteTransaction( InventoryBill bill, Func<string, ItemDefinition?>? lookup = null) {
public InventoryTransaction BeginTransaction( InventoryBill bill, Func<string, ItemDefinition?>? lookup = null) {
public bool TryExecuteTransaction( InventoryBill bill, Action? onCommitted = null, Func<string, ItemDefinition?>? lookup = null) {
public bool TryConsumeBill(IReadOnlyDictionary<string, int> costs, Action? onCommitted = null) {
public bool TryConsumeBill(IEnumerable<KeyValuePair<string, int>> costs, Action? onCommitted = null) {
public bool TryConsumeBill(IEnumerable<string> itemIds, Action? onCommitted = null) {
internal void ApplyTransactionMutations(InventoryBill bill, Func<string, ItemDefinition?>? lookup = null) {
internal void NotifyTransactionCommitted(InventoryBill bill, Func<string, ItemDefinition?>? lookup = null) {
internal void RestoreSnapshot(InventorySnapshot snapshot) {
public bool Transfer(ItemDefinition item, int amount, Inventory destination) {
public bool Equip(ItemDefinition item) {
public bool Equip( ItemDefinition item, IEnumerable<Medical.LimbState>? limbs, Func<string, ItemDefinition?>? catalog = null, Func<string, float>? conditionProvider = null) {
public ItemDefinition? Unequip(EquipSlot slot) {
public bool TryUnequipTo(EquipSlot slot, Inventory destination) {
public EquippedItem? GetEquipped(EquipSlot slot) {
public float GetEquippedProtection() {
public List<WornGear> BuildWornGear() {
public void FillWornGear(List<WornGear> buffer) {
public event Action<EquippedItem, string>? OnProtectiveGearFailed;
public bool TryRepairEquippedGear(EquippedItem item) {
public event Action<EquippedItem, float>? OnProtectiveGearRepaired;
public void RecordWear(EquippedItem item, float wearDelta, string cause = "radiation") {
public void DegradeEquippedGear(float gameHours, float multiplier = 1f, string cause = "use") {
public sealed class ProtectiveLifeEstimate
public string ItemId = string.Empty;
public string DisplayName = string.Empty;
public float CurrentDurability;
public float MaxDurability;
public float DegradeRate;
public float ExposureMultiplier = 1f;
public float HoursRemaining;
public bool TryEstimateWeakestProtectiveLife( float exposureMultiplier, out ProtectiveLifeEstimate? life) {
public bool Consume( ItemDefinition item, Func<ItemType, float, bool>? applyNeed = null, Action<float>? applyRadCleanse = null, Action? applyIodine = null, Action<float>? applyContamination = null,
public InventorySaveState CaptureState() {
public void ResortSlotsByType() {
public bool IsSortedByType() {
public void RestoreState(InventorySaveState state, Func<string, ItemDefinition?> lookup) {
public class InventorySlot
public ItemDefinition Item;
public int Amount;
public DeviceState? Device;
public float CurrentDurability = -1f;
public float GetDurability() {
public bool IsBrokenOrDegraded() {
public class EquippedItem
public ItemDefinition Item;
public float CurrentDurability;
public class InventorySaveState
public int capacity;
public float maxWeight;
public List<SlotSave> slots = new List<SlotSave>();
public List<EquippedSave> equipped = new List<EquippedSave>();
public class SlotSave
public string itemId;
public int amount;
public bool hasDevice;
public float battery;
public float calibration;
public bool broken;
public int lastCalibratedDay;
public class EquippedSave
public string itemId;
public float durability;
```


# Appendix E.14 — Supporting Code Evidence: `src/Main.Inventory.cs`

### `src/Main.Inventory.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 269 lines / 11760 bytes.
- SHA-256: `7f22de47729189d6c5ab6223cd95c137e04d977b50300fcb8f608a796f456ddc`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix E.15 — Supporting Code Evidence: `Assets/Ashfall.Core/HoldfastTradeSession.cs`

### `Assets/Ashfall.Core/HoldfastTradeSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1130 lines / 51598 bytes.
- SHA-256: `68e3c2b379443e5c780d7aba9b7cdf430a9e95892f44278816ca6412ebc49708`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=1; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum HoldfastFactionStance
public enum HoldfastTradeFailure
public sealed class HoldfastTradeInventorySlot
public HoldfastItemDefinition Item { get; }
public int Amount { get; }
public sealed class HoldfastTradeInventory
public int Capacity { get; set; } = 20;
public float MaxWeight { get; set; } = 100f;
public int OccupiedCount => _backingInventory != null ? _backingInventory.Slots.Count : _items.Count;
public float GetCurrentWeight() {
public bool CanAdd(string itemId, int count) {
public bool AddItem(string itemId, int count) {
public void RemoveItem(string itemId, int count) {
public bool HasSufficient(string itemId, int count) {
public bool ValidateBill(IReadOnlyDictionary<string, int> bill) {
public bool TryConsumeBill(IReadOnlyDictionary<string, int> bill, Action? onCommitted = null) {
public void Clear() {
public sealed class HoldfastTradeResult
public bool Success { get; set; }
public string ItemId { get; set; } = string.Empty;
public int Quantity { get; set; }
public string FactionId { get; set; } = string.Empty;
public int TotalValue { get; set; }
public string Message { get; set; } = string.Empty;
public string WhyLine { get; set; } = string.Empty;
public HoldfastTradeFailure Failure { get; set; } = HoldfastTradeFailure.None;
public int FundsDelta { get; set; }
public Economy.FundsFailure FundsFailure { get; set; } = Economy.FundsFailure.None;
public static HoldfastTradeResult Ok(string itemId, int quantity, string factionId, int totalValue, string whyLine = "", int fundsDelta = 0) => new HoldfastTradeResult { Success = true, ItemId = itemId, Quantity = quantity, FactionId = factionId, TotalValue = totalValue, Message = "Trade completed.", WhyLine = whyLine, FundsDelta = fundsDelta };
public static HoldfastTradeResult Fail(string message, HoldfastTradeFailure failure = HoldfastTradeFailure.None, Economy.FundsFailure fundsFailure = Economy.FundsFailure.None) => new HoldfastTradeResult { Success = false, Message = message, Failure = failure, FundsFailure = fundsFailure };
public sealed class HoldfastTradeSession
public const int DefaultInventoryCapacity = 20;
public HoldfastTradeInventory Inventory { get; }
public Inventory.Inventory? PlayerInventory => _playerInventory;
public string SelectedFactionId { get; private set; } = string.Empty;
public Func<string, bool>? EmbargoQuery { get; set; }
public Func<string, HoldfastFactionStance>? StanceQuery { get; set; }
public event Action StateChanged;
public bool SelectFaction(string factionId) {
public void SeedInventory(string itemId, int count) {
public void ResetToDefaults() {
public int GetHeld(string itemId) {
public int GetStock(string itemId) {
public void SetStock(string itemId, int count) {
public long Value => _value;
public long PlayerValue => _value;
public bool CanDebitValue(long amount) => amount >= 0 && amount <= _value;
public bool CanCreditValue(long amount) =>
public bool TryDebitValue(long amount, Func<bool>? secondLeg = null) =>
public bool TryCreditValue(long amount, Func<bool>? secondLeg = null) =>
internal bool TryDebitValueForSettlement(long amount, Func<bool>? secondLeg = null) =>
internal bool TryCreditValueForSettlement(long amount, Func<bool>? secondLeg = null) =>
internal void NotifyExternalValueSettlement() => StateChanged?.Invoke();
public bool TryGetUnitValue(string itemId, out long unitValue) {
public long GetBuyPrice(string itemId, string factionId, int quantity = 1) {
public long GetSellPrice(string itemId, string factionId, int quantity = 1) {
public string GetWhyLine(string itemId, string factionId, bool isBuy) {
public HoldfastTradeResult Buy(string itemId, int quantity, string factionId) {
public HoldfastTradeResult Sell(string itemId, int quantity, string factionId) {
public static int ChitsFromSettlementUnits(float units) {
public HoldfastTradeResult BuyWithFunds(string itemId, int quantity, string factionId, Economy.FundsLedger ledger, int day, string counterpartyId = "holdfast") {
public HoldfastTradeResult SellWithFunds(string itemId, int quantity, string factionId, Economy.FundsLedger ledger, int day, string counterpartyId = "holdfast") {
public bool TryRestoreState(HoldfastTradeSaveState state, out string error) {
public CommandPreview PreviewBuy(string itemId, int quantity, string factionId, long stateVersion = 0) {
public CommandResult ExecuteBuy(string itemId, int quantity, string factionId, long expectedStateVersion = 0, long currentStateVersion = 0) {
public CommandPreview PreviewSell(string itemId, int quantity, string factionId, long stateVersion = 0) {
public CommandResult ExecuteSell(string itemId, int quantity, string factionId, long expectedStateVersion = 0, long currentStateVersion = 0) {
public HoldfastTradeSaveState CaptureState() {
public class HoldfastTradeSaveState
public int schemaVersion = 0;
public long value;
public Dictionary<string, int> held = new Dictionary<string, int>();
public Dictionary<string, int> stock = new Dictionary<string, int>();
```


# Appendix E.16 — Supporting Code Evidence: `src/Host/HostCli.PanelTests.cs`

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


# Appendix G.17 — Supporting Regression Evidence: `Ashfall.Core.Tests/Inventory/UnifiedInventoryOwnershipTests.cs`

### `Ashfall.Core.Tests/Inventory/UnifiedInventoryOwnershipTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 219; SHA-256: `35602f17a17a1925a64f5bc3af71b766c8d0e58b385283f755d75fc2430a749e`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CanonicalAliases_MapLegacyAndPrefixedIds_ToAuthoritativeIds
Inventory_ImplementsPlayerInventoryPort_WithAtomicTransactions
HoldfastTrading_TransactsAgainstAuthoritativeInventory_AndMaintainsSeparateMerchantStock
TryRestoreState_WithBackingInventory_DoesNotDiscardUnrelatedPlayerItems
LegacySaveMigration_MergesHoldings_WithoutDuplication
CrossSystemFlow_HarvestToConsumeToTrade_OperatesOnSingleLedger
ProvenanceRecords_TrackMutationCausality
```


# Appendix G.18 — Supporting Regression Evidence: `Ashfall.Core.Tests/WildlifeTrappingIntegrationTests.cs`

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


# Appendix G.19 — Supporting Regression Evidence: `Ashfall.Core.Tests/CraftingCommandTests.cs`

### `Ashfall.Core.Tests/CraftingCommandTests.cs`

- Current test declarations: Fact=14, Theory=0, InlineData=0.
- File lines: 302; SHA-256: `ade3127765341b5da9b9b7084ea88a1b89541f6d90027911ac9a27d4163f496c`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
PreviewCraft_UnknownRecipe_ReturnsUnavailable
PreviewCraft_Available_ShowsProjectedDeltas
ExecuteCraft_Success_AdvancesStateVersion
ExecuteCraft_StalePreview_RejectsWithoutMutation
ExecuteCraft_Failure_NoPartialMutation
PreviewStart_InvalidParams_ReturnsUnavailable
PreviewStart_AlreadyActive_ReturnsUnavailable
PreviewStart_Available_ShowsProjectedDeltas
ExecuteStart_Success_AdvancesStateVersion
ExecuteStart_StalePreview_RejectsWithoutMutation
PreviewPushLuck_WrongPhase_ReturnsUnavailable
OnCraftCompleted_CarriesAssignedCrafterId
OnCraftCompleted_UnassignedCraft_ReportsEmptyCrafter
OnCraftCompleted_CrafterSurvivesSaveRestore
```


# Appendix G.20 — Supporting Regression Evidence: `Ashfall.Core.Tests/Journeys/EndToEndPlayerJourneyTests.cs`

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


# Appendix H.21 — Supporting Authority Document: `docs/crafting/RELIC_RESTORATION_RUNTIME_CONTRACT.md`

### `docs/crafting/RELIC_RESTORATION_RUNTIME_CONTRACT.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 68 lines / 5384 bytes.
- SHA-256: `980f876604d5283317abe07467749943f566ae811409324a0c229a9a1731b553`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix H.22 — Supporting Authority Document: `docs/crafting/PLAN_87_RELIC_COVERAGE_MATRIX.md`

### `docs/crafting/PLAN_87_RELIC_COVERAGE_MATRIX.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 58 lines / 5052 bytes.
- SHA-256: `53a97fc5c02bbb82d708aaa8d1a13a72d9f5c3484a4488a394cffa69d954980d`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| recipe schema and component references | RelicCatalogLoader | teardown, wear, restoration and outputs | WorkshopReverseEngineeringSystem | Owner emits/reads a typed fact; no mirror state. |
| recipe schema and component references | RelicCatalogLoader | authored restoration definitions | Relic recipe catalog | Owner emits/reads a typed fact; no mirror state. |
| recipe schema and component references | RelicCatalogLoader | post-restoration instance history | ItemLoreSystem | Owner emits/reads a typed fact; no mirror state. |
| teardown, wear, restoration and outputs | WorkshopReverseEngineeringSystem | recipe schema and component references | RelicCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| teardown, wear, restoration and outputs | WorkshopReverseEngineeringSystem | authored restoration definitions | Relic recipe catalog | Owner emits/reads a typed fact; no mirror state. |
| teardown, wear, restoration and outputs | WorkshopReverseEngineeringSystem | post-restoration instance history | ItemLoreSystem | Owner emits/reads a typed fact; no mirror state. |
| authored restoration definitions | Relic recipe catalog | recipe schema and component references | RelicCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| authored restoration definitions | Relic recipe catalog | teardown, wear, restoration and outputs | WorkshopReverseEngineeringSystem | Owner emits/reads a typed fact; no mirror state. |
| authored restoration definitions | Relic recipe catalog | post-restoration instance history | ItemLoreSystem | Owner emits/reads a typed fact; no mirror state. |
| post-restoration instance history | ItemLoreSystem | recipe schema and component references | RelicCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| post-restoration instance history | ItemLoreSystem | teardown, wear, restoration and outputs | WorkshopReverseEngineeringSystem | Owner emits/reads a typed fact; no mirror state. |
| post-restoration instance history | ItemLoreSystem | authored restoration definitions | Relic recipe catalog | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Retire the six-to-thirty implementation objective as complete. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Define ongoing catalog admission and reachability checks for new recipes. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Separate restoration authority, provenance authority and research-unlock ownership explicitly. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Require balance evidence before adding high-value relic rows. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.554 — Additional Current Architecture Evidence: `src/Main.UiTests.RealCampaignJourney.cs`

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


# Appendix Q.555 — Additional Current Architecture Evidence: `src/Main.World.cs`

### `src/Main.World.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 950 lines / 42412 bytes.
- SHA-256: `4f75bdf8f2f5c5022d744a331f9da271f16d7ab47d1bc3b9605b8a3a8fd95261`.
- Architecture signals: seeded references=0; save/restore symbols=8; typed event declarations=0; textual Godot mentions=9; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.556 — Additional Current Architecture Evidence: `src/UI/SurvivalWorkstationPanel.cs`

### `src/UI/SurvivalWorkstationPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 665 lines / 29498 bytes.
- SHA-256: `f6b87e513f85cb11a0d467ecb8f6ee57bba53c751df6134c1dcde53083dffcdb`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=11; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class SurvivalWorkstationPanel : Control, IBindablePanel
public event Action? OnClose;
public event Action? OnOpenInventoryOverlay;
public event Action? OnOpenCraftingOverlay;
public event Action? OnCraftStarted;
public bool IsBound => _craftingHost != null;
public string SelectedCrafterId =>
public void Bind( CraftingHostSession crafting, InventoryHostSession? inventory = null, SurvivorsHostSession? survivors = null) {
public void RefreshView() {
public override void _Ready() {
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix Q.557 — Additional Current Architecture Evidence: `src/Host/InventoryHostSession.cs`

### `src/Host/InventoryHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 721 lines / 38324 bytes.
- SHA-256: `89d74d0fe6814c5dc75f9106ac26714153475da73d5f44a46bd2ba6fb12f7e7e`.
- Architecture signals: seeded references=0; save/restore symbols=5; typed event declarations=3; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class InventoryHostSession
public InventoryContainer Inventory { get; }
public ItemCatalog Catalog { get; }
public ItemDescriptionCatalog DescriptionCatalog { get; set; }
public ExpansionEnrichmentCatalog? EnrichmentCatalog { get; set; }
public SurvivorsHostSession? Survivors { get; set; }
public Func<string, ItemType, float, bool>? ApplyNeedOverride { get; set; }
public Func<string, string, int, int, int, ResourceAllocationDecision?>? RationingAuthorizer { get; set; }
public Action<string, float>? ApplyRadCleanseOverride { get; set; }
public Action<string>? ApplyIodineOverride { get; set; }
public Action<string, float>? ApplyContaminationOverride { get; set; }
public Func<string?>? DefaultSurvivorResolver { get; set; }
public string LastEvent { get; private set; } = string.Empty;
public static InventoryHostSession CreateForFixture( InventoryContainer? inventory = null, ItemCatalog? catalog = null) {
public static void SeedCatalogForTest(ItemCatalog catalog) {
public static InventoryHostSession Create( string? dataDir = null, string? startingSuppliesProfileId = null, bool seedWhenNoSave = true) {
public ItemInspectionModel? GetInspection(string itemId) {
public void LoadOrSeedStartingSupplies( string dataDir, IFileIO fileIO = null!, IJsonSerializer serializer = null!, bool failClosed = true, string? profileId = null)
public void SeedStartingSupplies() {
public bool TryAdd(string itemId, int amount) {
public string Add(string itemId, int amount) {
public string Remove(string itemId, int amount) {
public string Equip(string itemId) => EquipResult(itemId).MessageKey;
public ActionResult EquipResult(string itemId) {
public string Unequip(string slotName) {
public string? ResolveTargetSurvivorId(string? requestedSurvivorId = null) {
public string Consume(string itemId, float therapeuticScale = 1f) => ConsumeResult(itemId, null, therapeuticScale).MessageKey;
public string Consume(string itemId, string? survivorId, float therapeuticScale = 1f) => ConsumeResult(itemId, survivorId, therapeuticScale).MessageKey;
public ActionResult ConsumeResult(string itemId, float therapeuticScale = 1f) => ConsumeResult(itemId, null, therapeuticScale);
public ActionResult ConsumeResult(string itemId, string? survivorId, float therapeuticScale = 1f) {
public Action<string, string>? OnConsumed;
public int CurrentDay { get; set; } = 1;
public Action<string, int>? OnAntiRadAdministered { get; set; }
public Action<string, string, Ashfall.Core.Medical.ChemicalDependencyKind>? OnChemicalSubstanceConsumed { get; set; }
public Ashfall.Core.Medical.MedicalRecordLog? MedicalRecordLog { get; set; }
public List<Ashfall.Core.Campaign.DayStateChangeEvent> PendingDayEvents { get; } = new List<Ashfall.Core.Campaign.DayStateChangeEvent>();
public void DrainDayEvents(List<Ashfall.Core.Campaign.DayStateChangeEvent> target) {
public string InventoryLine() {
public string EquipLine() {
public InventorySaveState CaptureSave() => Inventory.CaptureState();
public void RestoreSave(InventorySaveState state) {
```


# Appendix Q.558 — Additional Current Architecture Evidence: `src/Host/HoldfastRuntimeSession.cs`

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


# Appendix Q.559 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Crafting/CraftingSystem.cs`

### `Assets/Ashfall.Core/Crafting/CraftingSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 495 lines / 20750 bytes.
- SHA-256: `1c2b994cf1bfc8ba2578c6fe49a8a1feae92f025fcab02b3093a1d4a8dbbc4e2`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class CraftingSystem
public const float StationWearPerCraft = 5f;
public bool IsPaused { get; set; }
public InventoryContainer OverflowStash { get; set; }
public event Action<Recipe> OnCraftStarted;
public event Action<Recipe, string> OnCraftCompleted; // recipe, crafterId (empty when unassigned)
public event Action<Recipe, string, int> OnCraftResultOverflow; // recipe, itemId, amount
public void BindCraftResultGate(Func<string, bool> isResultAllowed) => _isCraftResultAllowed = isResultAllowed;
public void SetDayProvider(Func<int> getDay) => _getDay = getDay;
public void SetCrafterCostMultiplier(Func<string, float> mult) => _crafterCostMultiplier = mult;
public void SetCrafterCraftTimeMultiplier(Func<string, float> mult) => _crafterCraftTimeMultiplier = mult;
public void SetCrafterProductivityTimeMultiplier(Func<string, float> mult) => _crafterProductivityTimeMultiplier = mult;
public void SetMoonshineGate(Func<string, bool> canCraftMoonshine) => _canCraftMoonshine = canCraftMoonshine;
public void BindResearchGate(Func<string, bool> isUnlocked) => _researchGate = isUnlocked;
public int ActiveCraftCount => _active.Count;
public IReadOnlyList<ActiveCraft> ActiveCrafts => _active;
public void AddStation(CraftingStation station) {
public void RemoveStation(CraftingStation station) => _stations.Remove(station);
public CraftingStation? GetStation(string id) {
public bool CanCraft(Recipe recipe) => CanCraft(recipe, null!);
public bool CanCraft(Recipe recipe, string crafterId) {
public bool StartCraft(Recipe recipe, string? crafterId = null) {
public CommandPreview PreviewCraft(Recipe recipe, string? crafterId = null, long stateVersion = 0) {
public CommandResult ExecuteCraft(Recipe recipe, string? crafterId = null, long expectedStateVersion = 0, long currentStateVersion = 0) {
public void Tick(float gameHours) {
public static bool IsMedicalRecipe(Recipe recipe) => IsMedicalCraftResult(recipe);
public CraftingSystemSave CaptureState() {
public void SetRecipeLookup(Func<string, Recipe?> lookup) => _recipeLookup = lookup;
public void RestoreState(CraftingSystemSave save) {
public class Recipe
public string id = string.Empty;
public string recipeName = string.Empty;
public List<Ingredient> ingredients = new List<Ingredient>();
public ItemDefinition result;
public int resultAmount = 1;
public float craftingTimeHours = 1f;
public string requiredStationId = string.Empty;
public string requiredResearchId = string.Empty;
public string requiredBlueprintId = string.Empty;
public class Ingredient
public ItemDefinition item;
public int amount = 1;
public class CraftingStation
public string id = string.Empty;
public string displayName = string.Empty;
public float condition = 100f;
public bool IsOperational => condition > 0f;
public void Degrade(float amount) {
public void Repair(float amount) {
public class ActiveCraft
public Recipe Recipe;
public float HoursRemaining;
public string CrafterId = string.Empty;
public class CraftingSystemSave
public ActiveCraftSave[] ActiveCrafts = Array.Empty<ActiveCraftSave>();
public WorkshopState? WorkshopState;
public PharmaLabState? PharmaState;
public class ActiveCraftSave
public string RecipeId;
public float HoursRemaining;
public string CrafterId;
```


# Appendix Q.560 — Additional Current Architecture Evidence: `src/Host/HostCli.AdvancedIndustrialRecon.cs`

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


# Appendix Q.561 — Additional Current Architecture Evidence: `src/Main.PlayerSurfaces.cs`

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


# Appendix Q.562 — Additional Current Architecture Evidence: `src/Host/CraftingHostSession.cs`

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


# Appendix Q.563 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs`

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


# Appendix Q.564 — Additional Current Architecture Evidence: `src/Main.UiTests.WorkshopRelic.cs`

### `src/Main.UiTests.WorkshopRelic.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 162 lines / 7785 bytes.
- SHA-256: `040e84a0977a8b70671286dc710f5c46e39952c95a153922910385e5350e9954`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.565 — Additional Current Architecture Evidence: `src/UI/CraftingPanel.cs`

### `src/UI/CraftingPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 470 lines / 19822 bytes.
- SHA-256: `b6f10d7b99d59eaf8f648bbedfbeb864aaa427a9643e26a003ada88d7a753142`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=9; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class CraftingPanel : Control, IBindablePanel
public event Action? OnClose;
public event Action? OnCraftStarted;
public event Action? OnOpenWorkshopRequested;
public event Action? OnOpenPharmaLabRequested;
public bool IsBound => _craftingHost != null;
public string SelectedCrafterId =>
public void Bind( CraftingHostSession crafting, InventoryHostSession? inventory = null, SurvivorsHostSession? survivors = null, Ashfall.Core.Survivors.TradeSpecialtySystem? tradeSpecialty = null, Func<string, string>? professionResolver = null)
public void RefreshView() {
public override void _Ready() {
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix Q.566 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/TravelingCaravanSystem.cs`

### `Assets/Ashfall.Core/TravelingCaravanSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 518 lines / 22717 bytes.
- SHA-256: `9c7a92b23c61fab7d3f7b434eb3a9d78773ce6c37d2e5de21cd3d1723ccbc349`.
- Architecture signals: seeded references=2; save/restore symbols=3; typed event declarations=11; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class CaravanInventoryItem
public string itemId;
public int quantity;
public int priceRations;
public class CaravanEntry
public string caravanId;
public string caravanName;
public string factionId;
public string originRegion; // flotilla, foundry, greenhouse, settlement, traplines
public string currentNodeId;
public List<string> routeNodeIds = new List<string>();
public int routeIndex = 0;
public int daysAtCurrentNode = 0;
public int stayDurationDays = 2;
public int guardCount = 4;
public bool isRobbed = false;
public bool embargoBlocked = false;
public List<CaravanInventoryItem> inventory = new List<CaravanInventoryItem>();
public class TravelingCaravanState
public List<CaravanEntry> activeCaravans = new List<CaravanEntry>();
public int completedTradesCount = 0;
public class TravelingCaravanSystem
public const string SystemId = "traveling_caravan_system";
public GoodsCatalog? Catalog { get; set; }
public Narrative.TravelEncounterSystem? TravelEncounters { get; set; }
public TradeEmbargoSystem? Embargoes { get; set; }
public WastelandMapSystem? Map { get; set; }
public event Action<CaravanEntry, string>? OnCaravanArrivedAtNode;
public event Action<CaravanEntry, string, int>? OnTradeCompleted;
public event Action<CaravanEntry, Narrative.TravelEncounterDefinition>? OnCaravanPatrolEncountered;
public event Action<CaravanEntry, string>? OnCaravanEmbargoed;
public event Action<CaravanEntry>? OnCaravanResumed;
public TravelingCaravanState State => _state;
public int CaravanCount => _state.activeCaravans?.Count ?? 0;
public void SpawnCaravan(string caravanId, string name, string factionId, List<string> route, string originRegion = "settlement") {
public CaravanEntry? GetCaravanAtNode(string nodeId) {
public void DailyTick() => DailyTick(0, null);
public Func<WeatherKind, float>? WeatherAvailabilityProvider { get; set; }
public void DailyTick( int currentDay, ISeededRng? rng = null, string defaultRegion = "the_toll", int dangerLevel = 2, string season = "all",
public Narrative.TravelEncounterDefinition? CheckRouteEncounter( CaravanEntry caravan, string region, int dangerLevel, string season, int currentDay,
public bool ResolveRouteEncounterChoice( string encounterId, string choiceId, int currentDay, out Narrative.TravelEncounterResolutionResult? result) {
public bool TryBuyItem(string caravanId, string itemId, int amount, ref int playerRations) {
public TravelingCaravanState CaptureState() {
public void RestoreState(TravelingCaravanState state) {
```


# Appendix Q.567 — Additional Current Architecture Evidence: `src/Host/GreenhouseHostSession.cs`

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


# Appendix Q.568 — Additional Current Architecture Evidence: `src/Main.UiTests.Inventory.cs`

### `src/Main.UiTests.Inventory.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 113 lines / 5748 bytes.
- SHA-256: `11df7b77455a7af12edd0aa06c3beea944630dd533e3888c87b50e91e3b82f27`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.569 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Economy/BlackMarketSettlementService.cs`

### `Assets/Ashfall.Core/Economy/BlackMarketSettlementService.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 372 lines / 19874 bytes.
- SHA-256: `e8914870d671c1b6a02b3ff9abdc2140ef8861e044d437ed5a187262749cf2fe`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public readonly struct BlackMarketActionPreview
public readonly bool IsAvailable;
public readonly string ActionId;
public readonly string SyndicateId;
public readonly string EntryId;
public readonly string DebtId;
public readonly string ItemId;
public readonly int Quantity;
public readonly long SettlementUnits;
public readonly int DueDay;
public readonly string ReasonId;
public readonly string Message;
public readonly struct BlackMarketActionResult
public readonly bool Success;
public readonly string ActionId;
public readonly string SyndicateId;
public readonly string EntryId;
public readonly string DebtId;
public readonly string ItemId;
public readonly int Quantity;
public readonly long WalletDelta;
public readonly int InventoryDelta;
public readonly long SettlementUnits;
public readonly int DueDay;
public readonly string ReasonId;
public readonly string Message;
public sealed class BlackMarketSettlementService
public const string BuyAction = "buy";
public const string SellAction = "sell";
public const string LoanAction = "take_loan";
public const string RepayAction = "repay";
public long WalletValue => _wallet.Value;
public int InventoryCount(string itemId) =>
public BlackMarketActionPreview PreviewBuy(string syndicateId, string entryId, int quantity, int day) {
public BlackMarketActionResult Buy(string syndicateId, string entryId, int quantity, int day) {
public BlackMarketActionPreview PreviewSell(string syndicateId, string entryId, int quantity, int day) {
public BlackMarketActionResult Sell(string syndicateId, string entryId, int quantity, int day) {
public BlackMarketActionPreview PreviewLoan(string syndicateId, long units, int day, int durationDays) {
public BlackMarketActionResult TakeLoan(string syndicateId, long units, int day, int durationDays) {
public BlackMarketActionPreview PreviewRepay(string debtId, long units, int day) {
public BlackMarketActionResult Repay(string debtId, long units, int day) {
public static string ReasonText(string reasonId) {
```


# Appendix Q.570 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`

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


# Appendix Q.571 — Additional Current Architecture Evidence: `src/Main.UiPanels.cs`

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


# Appendix Q.572 — Additional Current Architecture Evidence: `src/UI/InventoryPanel.cs`

### `src/UI/InventoryPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 282 lines / 12769 bytes.
- SHA-256: `5ccf7a7ab62a105b4f5b4bb6175116f7456301bcbd8ef5a668f10e000a061daa`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=5; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class InventoryPanel : Control, IBindablePanel
public event Action? OnClose;
public event Action<string>? OnItemSelected;
public bool IsBound => _inventoryHost != null;
public void Bind(InventoryHostSession inventory) {
public void RefreshView() {
public override void _Ready() {
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix Q.573 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

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


# Appendix Q.574 — Additional Current Architecture Evidence: `src/Host/WildlifeTrappingHostSession.cs`

### `src/Host/WildlifeTrappingHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 643 lines / 28260 bytes.
- SHA-256: `9318ab54790453b20a0f62683207ac150c70862300b1fbbdba9ac6fcd06cf8e6`.
- Architecture signals: seeded references=1; save/restore symbols=1; typed event declarations=6; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class WildlifeTrappingHostSession
public WildlifeTrappingSystem System { get; }
public string LastEvent { get; private set; } = string.Empty;
public WildlifeTrappingCatalog? Catalog { get; set; }
public InventoryHostSession? Inventory { get; set; }
public Action<string, string, int>? ApplyDisease { get; set; }
public Action<string, float>? ApplyContamination { get; set; }
public Func<string, string, string, bool>? DeliverMoralConsequence { get; set; }
public Func<string, string, int, bool>? DeliverTrapEncounter { get; set; }
public Func<string, bool>? DeliverTrappingBroadcast { get; set; }
public Func<string, string, int, string, bool>? DeliverNarrativeIncident { get; set; }
public event Action<BycatchOccurredEvent>? OnBycatchOccurred;
public Func<int, bool>? DeliverButcheryFood { get; set; }
public Action<string, float, string>? ApplyMorale { get; set; }
public Func<PreyDefinition, string>? DiseaseResolver { get; set; }
public event Action<string>? OnTrapCrafted;
public ActionResult SetTrap(string siteId, string baitType, string hunterId) {
public bool CanSetTrapAtSite(string siteId, out string failureCode) => System.CanSetTrapAtSite(siteId, out failureCode);
public void SetSelectionContext(WildlifeSelectionContext context) => System.SetSelectionContext(context);
public ActionResult TrySetTrap(string siteId, string trapId, string baitType, string hunterId) {
public bool TryGetSetupBill(string trapId, out InventoryBill bill, out string reason) {
public bool CanAffordSetup(string trapId, out InventoryBill bill, out string failureReason) {
public float WildlifeDensityMultiplier { get; set; } = 1f;
public ActionResult CheckTraps(float? densityMultiplier = null) {
public void DeliverPendingEvents() {
public string ComposeBroadcastMessage(WildlifeTrappingPendingEvent ev) {
public const float FallbackContaminationDose = PreyDefinition.FallbackContaminationDose;
public const string FallbackDiseaseId = PreyDefinition.FallbackDiseaseId;
public ActionResult Butcher(string siteId, string butcherId = "") {
public static string ResolveDiseaseId(PreyDefinition prey) => PreyDefinition.ResolveDiseaseId(prey);
public ActionResult RemoveToxin(string siteId) {
public ActionResult PreserveHide(string siteId) {
public bool TryGetRepairBill(string siteId, out InventoryBill bill, out string reason) {
public bool CanAffordRepair(string siteId, out InventoryBill bill, out string failureReason) {
public ActionResult TryRepairTrap(string siteId) {
public ActionResult RemoveTrap(string siteId) {
public void ReconcileMapMarkers() {
public void TickDay(int day) {
public event Action<int>? OnCatchPressure;
public override void Save() {
```


# Appendix Q.575 — Additional Current Architecture Evidence: `src/Main.GameFlow.cs`

### `src/Main.GameFlow.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 950 lines / 43281 bytes.
- SHA-256: `2fdc11c80e9e23418cf05a6d8dbd621c66c8d6aba34503e7cde474922c403685`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=2; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.576 — Additional Current Architecture Evidence: `src/UI/InventoryDetailPanel.cs`

### `src/UI/InventoryDetailPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 288 lines / 13820 bytes.
- SHA-256: `3442795d2c64a19bc1358f43a62108a6ff34fc7cfc660e979bb579f722fc2db5`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=7; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class InventoryDetailPanel : Control
public event Action? OnClose;
public event Action<string>? OnConsume;
public event Action<string>? OnEquip;
public bool IsBound => _inventory != null && !string.IsNullOrEmpty(_itemId);
public int RenderedRowCount { get; private set; }
public ItemInspectionModel? CurrentInspection { get; private set; }
public void Bind(InventoryHostSession? inventory, string itemId, ItemDescriptionCatalog? descriptions = null, ExpansionEnrichmentCatalog? enrichment = null) => Bind(inventory, itemId, descriptions, enrichment, technicalProvenance: null);
public void Bind(InventoryHostSession? inventory, string itemId, ItemDescriptionCatalog? descriptions, ExpansionEnrichmentCatalog? enrichment, System.Collections.Generic.IReadOnlyList<Ashfall.Core.Narrative.TechnicalMaterialRecord>? technicalProvenance) => Bind(inventory, itemId, descriptions, enrichment, technicalProvenance, leatherProvenance: null);
public void Bind(InventoryHostSession? inventory, string itemId, ItemDescriptionCatalog? descriptions, ExpansionEnrichmentCatalog? enrichment, System.Collections.Generic.IReadOnlyList<Ashfall.Core.Narrative.TechnicalMaterialRecord>? technicalProvenance, System.Collections.Generic.IReadOnlyList<Ashfall.Core.Narrative.LeatherworkRecord>? leatherProvenance) {
public override void _Ready() {
public void RefreshView() {
public void Open() {
public override void _GuiInput(InputEvent @event) {
public override void _UnhandledInput(InputEvent @event) {
```


# Appendix Q.577 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Balance/ResourceMassBalanceSimulator.cs`

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


# Appendix Q.578 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs`

### `Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1150 lines / 50198 bytes.
- SHA-256: `89e132b6253ae2c5c11629d2fcdae4544b4b03a004d12bf0de89f37a0a7fd2bf`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=7; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum ChoiceRequirementFailureType
public sealed class ChoiceRequirementFailure
public ChoiceRequirementFailureType FailureType { get; set; }
public string ItemId { get; set; } = string.Empty;
public int RequiredQuantity { get; set; }
public int AvailableQuantity { get; set; }
public string Reason { get; set; } = string.Empty;
public sealed class ChoiceAvailabilityResult
public bool IsAvailable => Failures.Count == 0;
public List<ChoiceRequirementFailure> Failures { get; } = new List<ChoiceRequirementFailure>();
public sealed class TravelEncounterResolutionPlan
public string EncounterId { get; init; } = string.Empty;
public string ChoiceId { get; init; } = string.Empty;
public int CurrentDay { get; init; }
public string CooldownKey { get; init; } = string.Empty;
public int CooldownExpiryDay { get; init; }
public bool IsOnCooldown { get; init; }
public int MoraleDelta { get; init; }
public int GuiltDelta { get; init; }
public string RawFactionId { get; init; } = string.Empty;
public string CanonicalFactionId { get; init; } = string.Empty;
public int FactionStandingDelta { get; init; }
public string UnlocksFieldGuideId { get; init; } = string.Empty;
public int AdvancesChainStage { get; init; }
public string? ChainId { get; init; }
public IReadOnlyList<NormalizedItemCost> Costs { get; init; } = Array.Empty<NormalizedItemCost>();
public string RequiredItemId { get; init; } = string.Empty;
public int RequiredItemQuantity { get; init; }
public string RequiredFlag { get; init; } = string.Empty;
public ChoiceAvailabilityResult Availability { get; init; } = new ChoiceAvailabilityResult();
public bool CanExecute => !IsOnCooldown && Availability.IsAvailable;
public sealed class TravelEncounterResolutionResult
public string EncounterId { get; set; } = string.Empty;
public string ChoiceId { get; set; } = string.Empty;
public int Day { get; set; }
public int MoraleDelta { get; set; }
public int GuiltDelta { get; set; }
public string FactionId { get; set; } = string.Empty;
public string CanonicalFactionId { get; set; } = string.Empty;
public int FactionStandingDelta { get; set; }
public string UnlocksFieldGuideId { get; set; } = string.Empty;
public int ChainStageAdvanced { get; set; }
public List<NormalizedItemCost> DeductedCosts { get; set; } = new List<NormalizedItemCost>();
public string CooldownKey { get; set; } = string.Empty;
public FactionBountyRecord? BountyRecord { get; set; }
public sealed class PatrolRecognitionContext
public string FactionId { get; init; } = string.Empty;
public string EncounterId { get; init; } = string.Empty;
public IReadOnlyList<string> PriorChoiceIds { get; init; } = Array.Empty<string>();
public int EncountersWithFaction { get; init; }
public int PaidTollCount { get; init; }
public int FoughtPatrolCount { get; init; }
public int CurrentChainStage { get; init; }
public int CurrentStanding { get; init; }
public IReadOnlyList<string> RecognitionTags { get; init; } = Array.Empty<string>();
public bool HasChoice(string choiceId) {
public bool HasTag(string tag) {
public sealed class PatrolChoicePresentation
public string ChoiceId { get; init; } = string.Empty;
public string Text { get; init; } = string.Empty;
public bool IsAvailable { get; init; }
public string DisabledReasonCode { get; init; } = string.Empty;
public string RequiredItemId { get; init; } = string.Empty;
public int RequiredItemQuantity { get; init; }
public IReadOnlyList<NormalizedItemCost> Costs { get; init; } = Array.Empty<NormalizedItemCost>();
public int MoraleDelta { get; init; }
public int GuiltDelta { get; init; }
public string FactionId { get; init; } = string.Empty;
public int FactionStandingDelta { get; init; }
public IReadOnlyList<ChoiceRequirementFailure> Failures { get; init; } = Array.Empty<ChoiceRequirementFailure>();
public sealed class PatrolEncounterPresentation
public string EncounterId { get; init; } = string.Empty;
public string FactionId { get; init; } = string.Empty;
public string DisplayFactionId { get; init; } = string.Empty;
public string TerritoryState { get; init; } = string.Empty;
public string PatrolArchetype { get; init; } = string.Empty;
public int CurrentChainStage { get; init; }
public string RecognitionLabel { get; init; } = string.Empty;
public IReadOnlyList<PatrolChoicePresentation> Choices { get; init; } = Array.Empty<PatrolChoicePresentation>();
public sealed class TravelEncounterSystem
public TravelEncounterCatalog Catalog => _catalog;
public event Action<string, string>? OnChoiceResolved;
public event Action<string, int>? OnChainStageAdvanced;
public event Action<string, string>? OnPatrolHistoryRecorded;
public static string GetCooldownKey(TravelEncounterDefinition encounter) {
public int GetCooldownExpiry(string cooldownKey) {
public int GetCooldownExpiry(TravelEncounterDefinition encounter) {
public int GetChainStage(string chainId) {
public void SetChainStage(string chainId, int stage) {
public int GetPatrolChainStage(string factionId, string chainId) {
public int GetPatrolChainStage(TravelEncounterDefinition encounter) {
public PatrolRecognitionContext GetRecognitionContext(string factionId, string encounterId = "", string chainId = "") {
public PatrolRecognitionContext GetRecognitionContext(TravelEncounterDefinition encounter) {
public bool IsEncounterEligible( TravelEncounterDefinition encounter, string region, float dangerLevel, string currentSeason, int currentDay,
public bool IsEncounterEligible(TravelEncounterDefinition encounter, TravelEncounterSelectionContext context) {
public float GetEffectiveWeight(TravelEncounterDefinition encounter, string stance) {
public const float MigrationEncounterBiasK = 0.5f;
public Func<string, float>? RegionEncounterPressureProvider { get; set; }
public float GetEffectiveWeight(TravelEncounterDefinition encounter, string stance, string region) {
public static float MigrationSusceptibility(TravelEncounterDefinition encounter) {
public TravelEncounterDefinition? SelectEncounter( string region, float dangerLevel, string stance, string currentSeason, int currentDay,
public TravelEncounterDefinition? SelectEncounter(TravelEncounterSelectionContext context) {
public ChoiceAvailabilityResult EvaluateChoiceAvailability( TravelEncounterChoice choice, Inventory.Inventory? inventory = null, Func<string, bool>? flagEvaluator = null) {
public ChoiceAvailabilityResult EvaluateChoiceAvailability( TravelEncounterDefinition encounter, TravelEncounterChoice choice, Inventory.Inventory? inventory = null, Func<string, bool>? flagEvaluator = null) {
public PatrolEncounterPresentation? BuildPatrolPresentation( string encounterId, Inventory.Inventory? inventory = null, Func<string, bool>? flagEvaluator = null) {
public bool TryBuildResolutionPlan( string encounterId, string choiceId, int currentDay, out TravelEncounterResolutionPlan? plan, Inventory.Inventory? inventory = null,
public bool ResolveChoice(string encounterId, string choiceId, int currentDay, out TravelEncounterResolutionResult? result) {
public bool ResolveChoice(string encounterId, string choiceId, int currentDay, out int moraleDelta, out int guiltDelta, out string unlockedFieldGuideId) {
public TravelEncounterState CaptureState() {
public void RestoreState(TravelEncounterState? state) {
```


# Appendix Q.579 — Additional Current Architecture Evidence: `src/Foundry/SilentFoundryHostSession.cs`

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


# Appendix Q.580 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Inventory/InventoryMigrator.cs`

### `Assets/Ashfall.Core/Inventory/InventoryMigrator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 60 lines / 2479 bytes.
- SHA-256: `ffdfb7fb08b151e460a218b26d9e31387ae596436fb48f27b9c06d15c06fb7da`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class InventoryMigrator
public static int MigrateHoldfastHeld( HoldfastTradeSaveState? legacyTradeState, Inventory targetInventory, Func<string, ItemDefinition?>? catalogLookup = null, bool allowResurrectLowerPhysicalCount = true) {
```


# Appendix Q.581 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Shelter/HydroponicBiomeSystem.cs`

### `Assets/Ashfall.Core/Shelter/HydroponicBiomeSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 440 lines / 16777 bytes.
- SHA-256: `fd5c5cfead3cc5fbfbe4226bc5412f3aa2cd63ee3d902997e4539b8cfbd8fbe3`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=8; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class HydroponicRackState
public string rackId = string.Empty;
public string cropId = string.Empty;
public int growthPermille; // 0..1000
public string brinePHBand = "Optimal"; // Acidic, Optimal, Alkaline
public int brinePPM = 800;
public string ledSpectrum = "Growth_Blue"; // Growth_Blue, Flowering_Red, Hardening_Infrared
public float contaminationLevel; // 0..100
public bool isPowered = true;
public float rootHealth = 100.0f; // 0..100
public string assignedWorkerId = string.Empty;
public int lastTickDay;
public List<string> activeTraits = new List<string>();
public HydroponicRackState Clone() {
public sealed class HydroponicBiomeSave
public List<HydroponicRackState> racks = new List<HydroponicRackState>();
public float nutrientTankReserve;
public Dictionary<string, int> seedVaultInventory = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
public List<string> unlockedStabilizedTraits = new List<string>();
public float maintenanceState = 100.0f;
public int lastTickDay;
public HydroponicBiomeSave Clone() {
public sealed class HydroponicBiomeSystem
public const float PowerDrawPerRackWatts = 450.0f;
public const float RootHealthMax = 100.0f;
public const int MaturePermille = 1000;
public static readonly string[] MutationTraitPool = new[] {
public IReadOnlyList<HydroponicRackState> Racks => _racks;
public float NutrientTankReserve => _nutrientTankReserve;
public IReadOnlyDictionary<string, int> SeedVaultInventory => _seedVaultInventory;
public IReadOnlyCollection<string> UnlockedStabilizedTraits => _unlockedStabilizedTraits;
public float MaintenanceState => _maintenanceState;
public int LastTickDay => _lastTickDay;
public event Action<string, string>? OnCropPlanted;
public event Action<string, string, int>? OnCropHarvested;
public event Action<string, string>? OnCropMutated;
public event Action<string>? OnCropDied;
public HydroponicRackState? GetRack(string rackId) {
public void AddSeed(string cropId, int count = 1) {
public bool TryMixNutrientBatch(int batches = 1) {
public bool TryPlantCrop(string rackId, string cropId, string workerId = "") {
public bool SetLedSpectrum(string rackId, string spectrum) {
public bool SetBrinePH(string rackId, string phBand) {
public bool TryStabilizeTrait(string traitId) {
public bool UnlockStabilizedTrait(string traitId) {
public bool TryHarvest(string rackId) {
public void TickDay(int currentDay, float ambientRadiation = 0f, bool? gridPoweredOverride = null) {
public HydroponicBiomeSave CaptureState() {
public void RestoreState(HydroponicBiomeSave? save) {
```


# Appendix Q.582 — Additional Current Architecture Evidence: `src/Host/ShelterDecorHostSession.cs`

### `src/Host/ShelterDecorHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 326 lines / 13499 bytes.
- SHA-256: `52b2a24194d91d6e412f85fd1cd2dbf28cd0d147f088100d24d9d642785e27b8`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ShelterDecorHostSession : HostSessionBase
public const string MemorialWallRoomId = "room_memorial_wall";
public ShelterDecorSystem System { get; }
public ShelterAssignmentSystem Assignments => _assignments;
public NeedsSystem Needs => _needs;
public ItemCatalog InventoryCatalog => _inventory.Catalog;
public InventoryContainer Inventory => _inventory.Inventory;
public string LastEvent { get; private set; } = string.Empty;
public int CatalogModifierCount { get; private set; }
public int LastMoraleRecipientCount { get; private set; }
public float LastMoraleGranted { get; private set; }
public int CurrentDay { get; private set; }
public int LoadCatalogModifiers() {
public IReadOnlyList<ShelterRoom> Rooms => _assignments.Rooms;
public void SetCurrentDay(int day) {
public void SetPanelMessage(string message) {
public string DisplayNameForRoom(string roomId) {
public List<ItemDefinition> ListAvailableDecor() {
public bool TryMount(string roomId, string slotId, string itemId, int day, out string reason) {
public bool TryRemoveMount(string roomId, string slotId, out string reason) {
public bool TryMountMemorialPlaque(MemorialEntry entry, out string reason) {
public int ApplyDailyMorale(int day) {
protected override void UnsubscribeSystemEvents() {
```


# Appendix Q.583 — Additional Current Architecture Evidence: `src/Main.AdvancedShelterSystems.cs`

### `src/Main.AdvancedShelterSystems.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 613 lines / 27033 bytes.
- SHA-256: `88b198ecb0532edcf5924ebe49168fc581cd5e3467c69fae3fa6a603f89c5df4`.
- Architecture signals: seeded references=7; save/restore symbols=14; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public CaravanTradeNetworkSystem EnsureCaravanTrade() {
public AdvancedSurgicalWardSystem EnsureSurgicalWard() {
public PowerDistributionSubgridSystem EnsurePowerSubgrids() {
public void TickPowerSubgrids(int day) {
public PerimeterDefenseSystem EnsurePerimeterDefense() {
public HydroponicBiomeSystem EnsureHydroponicBiomes() {
public NuclearCoreLifecycleSystem EnsureNuclearCore() {
public ArmoredCrawlerExpeditionSystem EnsureArmoredCrawlers() {
public void TickAdvancedShelterSystems(int day) {
```


# Appendix Q.584 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Expeditions/RailwaySystem.cs`

### `Assets/Ashfall.Core/Expeditions/RailwaySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 854 lines / 37947 bytes.
- SHA-256: `78037d319e77123b88f0a77c19a4d80e9435c2c41edeb0f54e3145ae80751a6b`.
- Architecture signals: seeded references=3; save/restore symbols=2; typed event declarations=13; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum TrainDispatchStatus
public sealed class RailNodeDef
public string node_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string zone_id { get; set; } = string.Empty;
public string node_type { get; set; } = "Terminal";
public sealed class TrackSegmentDef
public string segment_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string start_node_id { get; set; } = string.Empty;
public string end_node_id { get; set; } = string.Empty;
public float distance_km { get; set; } = 20.0f;
public float base_integrity { get; set; } = 0.8f;
public bool bridge_required { get; set; } = false;
public float max_train_mass { get; set; } = 200.0f;
public List<string> hazard_tags { get; set; } = new List<string>();
public sealed class TrainCarDef
public string car_type_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public float empty_mass { get; set; } = 30.0f;
public float cargo_capacity { get; set; } = 50.0f;
public float armor_rating { get; set; } = 50.0f;
public float max_fuel_capacity { get; set; } = 0.0f;
public float fuel_burn_per_km { get; set; } = 0.0f;
public string vehicle_class { get; set; } = "locomotive";      // "locomotive", "handcar"
public float crew_stamina_max { get; set; } = 1.0f;              // 0..1
public float stamina_drain_per_km { get; set; } = 0.0f;           // stamina units per km
public float stamina_recovery_per_stop { get; set; } = 0.3f;       // recovered at terminal
public sealed class RailwayNetworkCatalog
public int schema_version { get; set; } = 1;
public List<RailNodeDef> nodes { get; set; } = new List<RailNodeDef>();
public List<TrackSegmentDef> segments { get; set; } = new List<TrackSegmentDef>();
public List<TrainCarDef> cars { get; set; } = new List<TrainCarDef>();
public sealed class TrackSegmentState
public string segmentId { get; set; } = string.Empty;
public float integrity { get; set; } = 1.0f;
public bool bridgeIntact { get; set; } = true;
public bool isSabotaged { get; set; } = false;
public sealed class TrainCarInstance
public string instanceId { get; set; } = string.Empty;
public string carTypeId { get; set; } = string.Empty;
public float condition { get; set; } = 100.0f;
public sealed class TrainState
public string trainId { get; set; } = string.Empty;
public string displayName { get; set; } = string.Empty;
public string currentNodeId { get; set; } = string.Empty;
public string? activeSegmentId { get; set; } = null;
public float segmentProgress { get; set; } = 0.0f;
public float currentFuel { get; set; } = 100.0f;
public float maxFuel { get; set; } = 300.0f;
public List<TrainCarInstance> cars { get; set; } = new List<TrainCarInstance>();
public TrainDispatchStatus status { get; set; } = TrainDispatchStatus.Idle;
public List<string> plannedPath { get; set; } = new List<string>();
public float crewStamina { get; set; } = 1.0f;                  // 0..1
public float maxCrewStamina { get; set; } = 1.0f;
public float staminaDrainPerKm { get; set; } = 0.0f;
public float staminaRecoveryPerStop { get; set; } = 0.3f;
public bool isCrewExhausted { get; set; } = false;
public string vehicleClass { get; set; } = "locomotive";
public bool isOnExpedition { get; set; } = false;
public int transmissionWearPermille { get; set; } = 0;
public bool transmissionServiceRequired { get; set; } = false;
public int lastTransmissionServiceDay { get; set; } = -1;
public sealed class RailwayState
public int schema_version { get; set; } = 1;
public int last_tick_day { get; set; } = -1;
public Dictionary<string, TrackSegmentState> segments { get; set; } = new Dictionary<string, TrackSegmentState>(StringComparer.Ordinal);
public List<TrainState> trains { get; set; } = new List<TrainState>();
public sealed class RailwaySystem
public RailwayState State => _state;
public event Action<string, string>? OnTrainDispatched;
public event Action<string, string>? OnTrainArrived;
public event Action<string, string>? OnDerailment;
public event Action<string, string>? OnTrainAmbushed;
public event Action<string, float>? OnTrackRepaired;
public const string CatalogPath = "rail_logistics_catalog.json";
public IReadOnlyDictionary<string, RailLogisticsEdgeDef> LogisticsEdges => _logisticsEdges;
public void RegisterLogisticsCatalog(IEnumerable<RailLogisticsEdgeDef> edges) {
public RailLogisticsEdgeDef? GetLogisticsEdge(string fromNode, string toNode) {
public IReadOnlyDictionary<string, RailNodeDef> Nodes => _nodes;
public IReadOnlyDictionary<string, TrackSegmentDef> SegmentDefs => _segmentDefs;
public void RegisterCatalog(RailwayNetworkCatalog catalog) {
public TrackSegmentState EnsureSegmentState(string segmentId) {
public TrainState CreateStarterTrain(string trainId, string displayName, string startingNodeId) {
public ActionResult RepairTrack(string segmentId, float integrityRestored) {
public ActionResult RepairBridge(string segmentId) {
public ActionResult ClearTrackObstacle(string segmentId) {
public bool CanTraverseSegment(TrainState train, string segmentId) {
public float CalculateTrainMass(TrainState train) {
public float EstimateCoalRequired(TrainState train, string segmentId) {
public ActionResult DispatchTrain(string trainId, string segmentId) {
public void TickTravel(string trainId, float progressDelta = 0.5f) {
public void TickDay(int day) {
public ActionResult ServiceTransmission(string trainId, int serviceDay = -1, int repairPermille = 1000) {
public ActionResult PlanRoute(string trainId, List<string> segmentIds) {
public RailExpeditionEstimate? EstimateExpeditionTravel(string originNodeId, string destinationNodeId) {
public ActionResult DispatchExpedition(string trainId, string destinationNodeId) {
public ActionResult ClearDerailment(string trainId) {
public bool RestoreTrainAfterRecovery( string trainId, float trainConditionRestored, string segmentId, float trackIntegrityRestored) {
public TrainState? GetTrain(string trainId) => _state.trains.Find(t => t != null && t.trainId == trainId);
public bool IsRailCorridorOperational(string originNodeId, string destinationNodeId) {
public List<string>? GetOperationalRailCorridor(string originNodeId, string destinationNodeId) {
public RailwayState CaptureState() {
public void RestoreState(RailwayState state) {
public sealed class RailExpeditionEstimate
public int travelTicks { get; set; }
public float fuelRequired { get; set; }
public float staminaCost { get; set; }
public List<string> path { get; set; } = new List<string>();
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.
