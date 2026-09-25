# Plan 34 — Research Knowledge Catalog, Unlock Bridge and Save-Safe Externalization

> **Rebuild status:** COMPLETE 62-NODE RESEARCH AUTHORITY — DAG, UNLOCK AND CONSUMER MAINTENANCE
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

- The original externalization objective is complete: the research catalog is loaded into `ResearchSystem`, the host has no hardcoded fallback authority, the DAG validator detects cycles/missing prerequisites, and research completion routes to unlock/item/recipe/capability consumers.
- The live route is `research_knowledge.json` → `ResearchKnowledgeCatalogLoader` → `ResearchSystem` eligibility/progress/completion → `ResearchUnlockBridge`/`ResearchUnlockHostSession` → inventory/recipe/capability consumers → current research save.
- The quality target is a truthful, acyclic, reachable tree whose authored unlock rows point to current downstream authorities and whose save/restore does not re-fire completion events.

**Bounded outcome:** Retire the old 15-hardcoded/40-node premise. Current `research_knowledge.json` has 62 nodes, `research_unlocks.json` has 30 unlock rows, the loader validates a DAG, and the research system/host/UI/save bridge are live. The next package is a reference and consumer audit, not another research tree expansion.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `research_knowledge.json` is present with 62 unique knowledge nodes and valid prerequisite references; `research_unlocks.json` contains 30 authored unlock rows.
- `ResearchKnowledgeCatalogLoader` loads the catalog, registers definitions and validates DAG shape; `ResearchSystem` owns progress/completion and `ResearchUnlockBridge` owns exactly-once grants.
- `ResearchHostSession`, `ResearchUnlockHostSession`, `ResearchPanel` and `Main.ExpandedShelterSystems` are current host/UI/save seams.
- Focused tests cover loader count/fields, DAG failures, legacy parity, unlock grants, retroactive synchronization and save integration; fresh pass claims still require focused runs.

**Master-authority sections applied to this rebase:**

- Master authority Volume 28 verification cookbook: focused evidence before broad gates.
- Lane D save/state/compatibility guidance: owner DTOs, migration and restore proof.
- Lane E UI/UX/accessibility guidance: truthful projections and keyboard/controller lifecycle.
- Lane G testing guidance: smallest affected target, negative cases and deterministic replay.
- Anti-padding protocol: content exhaustion may end the plan before the character checkpoint.
- Volume 32 externalized authority and unlock guidance.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace 15→40 with a 62-node current census and DAG/parity matrix.
- Separate knowledge-node authority from unlock-row authority and downstream recipe/item/capability ownership.
- Require every unlock target to resolve through current catalogs or an explicit capability owner.
- Preserve exactly-once completion, retroactive synchronization and no-event-on-restore behavior.

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
| knowledge definitions, registration and DAG validation | ResearchKnowledgeCatalogLoader | `Assets/Ashfall.Core/Research/ResearchKnowledgeCatalogLoader.cs` | Sole knowledge-node loader. |
| eligibility, progress and completion state | ResearchSystem | `Assets/Ashfall.Core/Research/ResearchSystem.cs` | Owns research state and completion event. |
| exactly-once unlock grants and retroactive sync | ResearchUnlockBridge | `Assets/Ashfall.Core/Research/ResearchUnlockBridge.cs` | Owns unlock state, not recipe/inventory effects. |
| catalog binding, panels and save composition | Research host/UI | `src/Host/ResearchHostSession.cs; src/Host/ResearchUnlockHostSession.cs; src/UI/ResearchPanel.cs; src/Main.ExpandedShelterSystems.cs` | Host/presentation/save seam. |
| DAG, parity, unlock and persistence proof | Research focused tests | `Ashfall.Core.Tests/Progression/ResearchKnowledgeCatalogLoaderTests.cs; Ashfall.Core.Tests/Research/Plan141ResearchUnlockBridgeIntegrationTests.cs; Ashfall.Core.Tests/ResearchSaveIntegrationTests.cs` | Executable current evidence. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Research Knowledge Catalog, Unlock Bridge and Save-Safe Externalization
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ ResearchKnowledgeCatalogLoader
│   knowledge definitions, registration and DAG validation
│ ResearchSystem
│   eligibility, progress and completion state
│ ResearchUnlockBridge
│   exactly-once unlock grants and retroactive sync
│ Research host/UI
│   catalog binding, panels and save composition
│ Research focused tests
│   DAG, parity, unlock and persistence proof
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

1. **Preserve current state ownership.** ResearchKnowledgeCatalogLoader owns knowledge definitions, registration and DAG validation: Sole knowledge-node loader.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| knowledge definitions, registration and DAG validation | ResearchKnowledgeCatalogLoader | `Assets/Ashfall.Core/Research/ResearchKnowledgeCatalogLoader.cs` | Sole knowledge-node loader. |
| eligibility, progress and completion state | ResearchSystem | `Assets/Ashfall.Core/Research/ResearchSystem.cs` | Owns research state and completion event. |
| exactly-once unlock grants and retroactive sync | ResearchUnlockBridge | `Assets/Ashfall.Core/Research/ResearchUnlockBridge.cs` | Owns unlock state, not recipe/inventory effects. |
| catalog binding, panels and save composition | Research host/UI | `src/Host/ResearchHostSession.cs; src/Host/ResearchUnlockHostSession.cs; src/UI/ResearchPanel.cs; src/Main.ExpandedShelterSystems.cs` | Host/presentation/save seam. |
| DAG, parity, unlock and persistence proof | Research focused tests | `Ashfall.Core.Tests/Progression/ResearchKnowledgeCatalogLoaderTests.cs; Ashfall.Core.Tests/Research/Plan141ResearchUnlockBridgeIntegrationTests.cs; Ashfall.Core.Tests/ResearchSaveIntegrationTests.cs` | Executable current evidence. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load/validate knowledge DAG
2. register current nodes in one ResearchSystem
3. evaluate eligibility from prerequisites/day/cost
4. record progress and completion through owner
5. emit one completion fact
6. bridge unlock rows into current downstream owners
7. project available/locked/completed research
8. capture/restore without re-firing completion

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Knowledge definitions and authored unlock rows are immutable data; research progress and granted unlock IDs are owner state.
- The prerequisite graph is acyclic, resolvable and stable.
- Completion emits one transition fact; restore restores state without emitting a new completion event.
- Unlock effects are routed to their existing inventory/recipe/capability owners and recorded once by the bridge.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Every prerequisite resolves to a current node and the graph is acyclic.
- A missing/corrupt catalog does not silently substitute hardcoded nodes.
- Completion and unlock grants are exactly-once across replay, retroactive sync and save restore.
- Every unlock target resolves to a current item, recipe, capability or explicitly documented external target.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `research_knowledge.json` is the sole knowledge-node authority.
- `research_unlocks.json` is the separate authored unlock-row authority consumed by the bridge.
- No parallel research tree or hardcoded fallback is introduced.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use the current research save section and unlock bridge state.
- No new save section is justified by catalog content.
- Legacy progress and granted IDs restore exactly once; restore must not fire completion.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Eligibility, progress and unlock synchronization are deterministic.
- Catalog traversal order is stable and does not depend on hash order.
- Paired runs and restored runs produce identical completion/unlock traces.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Research completion is emitted by `ResearchSystem`.
- Unlock grants and synchronization are emitted by `ResearchUnlockBridge`/host.
- Downstream item/recipe/capability owners apply effects through their own contracts.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/ResearchHostSession.cs
- src/Host/ResearchUnlockHostSession.cs
- src/UI/ResearchPanel.cs
- src/Main.ExpandedShelterSystems.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Research descriptions should be practical, fictional and grounded.
- A node should describe a real current capability or unlock, not a fantasy ability with no consumer.
- The tree should support multiple disciplines without creating opaque prerequisites.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A prerequisite cycle or missing node reaches production. | ResearchKnowledgeCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A hardcoded fallback shadows JSON. | ResearchSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A completion event fires again on restore. | ResearchUnlockBridge | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | An unlock target is absent or granted twice. | Research host/UI | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | The panel shows a node the current system cannot start. | Research focused tests | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Progression/ResearchKnowledgeCatalogLoaderTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/ResearchCatalogParityTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Research/Plan141ResearchUnlockBridgeIntegrationTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/ResearchSaveIntegrationTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — current census | Read knowledge/unlock JSON, loaders, owners and tests. | 62 nodes/30 unlocks and authority are proven. | No production path until the owning implementation package is separately claimed. |
| 1 — DAG/reference audit | Validate graph and downstream targets. | No cycle, orphan or shadow target. | No production path until the owning implementation package is separately claimed. |
| 2 — completion/save proof | Trace completion, retroactive sync and restore. | Exactly-once event/unlock behavior. | No production path until the owning implementation package is separately claimed. |
| 3 — panel/content seal | Review locked reasons, costs and descriptions. | Player can understand and reach current nodes. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/research_knowledge.json | READ ONLY; MODIFY only for proven gap | 62-node authority |
| Assets/StreamingAssets/Data/research_unlocks.json | READ ONLY | 30-row unlock authority |
| Assets/Ashfall.Core/Research/ResearchKnowledgeCatalogLoader.cs | READ ONLY | Loader/DAG |
| Assets/Ashfall.Core/Research/ResearchUnlockBridge.cs | READ ONLY | Unlock owner |
| src/UI/ResearchPanel.cs | READ ONLY | Current UI |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Adding a second research catalog. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Changing DAG traversal order. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Re-firing completion on restore. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Pointing unlock rows at stale item/recipe IDs. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new research node count for this rebase.
- No new research system.
- No save-section change.
- No production edits in this rebase.

# 23. Rollback and Recovery

- Revert the plan file.
- Future catalog/code changes retain prior JSON, unlock fixtures and focused DAG/save tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 62 nodes and 30 unlock rows are current.
- DAG, consumer, event and save contracts are explicit.
- No hardcoded or parallel authority is proposed.
- Focused commands are listed.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace 15→40 with a 62-node current census and DAG/parity matrix.
- Separate knowledge-node authority from unlock-row authority and downstream recipe/item/capability ownership.
- Require every unlock target to resolve through current catalogs or an explicit capability owner.
- Preserve exactly-once completion, retroactive synchronization and no-event-on-restore behavior.

## MUST NOT DO

- No new research node count for this rebase.
- No new research system.
- No save-section change.
- No production edits in this rebase.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Progression/ResearchKnowledgeCatalogLoaderTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/ResearchCatalogParityTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Research/Plan141ResearchUnlockBridgeIntegrationTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/ResearchSaveIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — current census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: knowledge definitions, registration and DAG validation → ResearchKnowledgeCatalogLoader; eligibility, progress and completion state → ResearchSystem; exactly-once unlock grants and retroactive sync → ResearchUnlockBridge; catalog binding, panels and save composition → Research host/UI; DAG, parity, unlock and persistence proof → Research focused tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 34.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 34 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by ResearchKnowledgeCatalogLoader or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Research/ResearchKnowledgeCatalogLoader.cs`

### `Assets/Ashfall.Core/Research/ResearchKnowledgeCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 213 lines / 7884 bytes.
- SHA-256: `dc640db9e0c30d89287138694a4f6a59bbc289564a985de10039898805b89187`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ResearchKnowledgeNodeWireDto
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public string Category { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public int DaysToComplete { get; set; } = 5;
public List<string> Prerequisites { get; set; } = new List<string>();
public string? BreakthroughItem { get; set; }
public ResearchKnowledgeDef ToDomain() {
public sealed class ResearchKnowledgeCatalogContainer
public int SchemaVersion { get; set; } = 1;
public string CollectionId { get; set; } = "research_knowledge";
public List<ResearchKnowledgeNodeWireDto> KnowledgeNodes { get; set; } = new List<ResearchKnowledgeNodeWireDto>();
public static class ResearchKnowledgeCatalogLoader
public const string DefaultFileName = "research_knowledge.json";
public static List<ResearchKnowledgeDef> Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static int LoadAndRegister( ResearchSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static bool ValidateDag(IEnumerable<ResearchKnowledgeDef> defs, out string errorMessage) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Research/ResearchSystem.cs`

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


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Research/ResearchUnlockBridge.cs`

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


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/Research/ResearchKnowledgeDef.cs`

### `Assets/Ashfall.Core/Research/ResearchKnowledgeDef.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 61 lines / 2099 bytes.
- SHA-256: `a1791a29795cafe916dc0cfdd8e577690fac6d68eb6c4341c3d008c4df105edb`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=2; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ResearchKnowledgeDef
public string id;
public string displayName;
public string category;
public string description;
public string[] prerequisites;
public string breakthroughItem;
public int daysToComplete;
public bool isUnlocked;
public bool isCompleted;
```


# Appendix B.06 — Current Code Architecture: `src/Host/ResearchHostSession.cs`

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


# Appendix B.07 — Current Code Architecture: `src/Host/ResearchUnlockHostSession.cs`

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


# Appendix B.08 — Current Code Architecture: `src/UI/ResearchPanel.cs`

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


# Appendix B.09 — Current Code Architecture: `src/Main.ExpandedShelterSystems.cs`

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


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/research_knowledge.json`

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


# Appendix C.11 — Catalog Census: `Assets/StreamingAssets/Data/research_unlocks.json`

### `Assets/StreamingAssets/Data/research_unlocks.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 8728 bytes / 8728 characters.
- SHA-256: `6b4204348caa9cacabab6cb5f87cf07292a1047c2a4ff63f35a3d5a0c3fd2caf`.
- Root keys: `research_unlocks`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
research_unlocks: min=30, max=30, observed_paths=1
```

Representative record fields:

- `description`
- `id`
- `research_node_id`
- `unlock_target_id`
- `unlock_type`

Representative identifiers (ordered, capped for readability):

```text
unlock_water_advanced_filter
unlock_water_purify_recipe
unlock_rad_shielding_panel
unlock_shelter_lead_lining
unlock_gas_mask_improved
unlock_respirator_crafting
unlock_solar_inverter
unlock_solar_roof_array
unlock_air_filter_hepa
unlock_ventilation_scrubber
unlock_radio_cipher_rotor
unlock_encrypted_intercepts
unlock_hydroponic_greens
unlock_greenhouse_microclimate
unlock_canning_preservation
unlock_apiculture_honey
unlock_field_trauma_surgery
unlock_pathogen_isolation
unlock_chelation_infusion
unlock_precision_rifling
unlock_automated_sentry
unlock_guerrilla_ambush
unlock_defensive_tripwires
unlock_deep_well_hydraulics
unlock_geothermal_power
unlock_submersible_salvage
unlock_seismic_fault_routes
unlock_ruin_structural_survey
unlock_cbrn_filter_blueprint
unlock_power_armor_servos
```


# Appendix C.12 — Catalog Census: `Assets/StreamingAssets/Data/skills.json`

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


# Appendix C.13 — Catalog Census: `Assets/StreamingAssets/Data/recipes.json`

### `Assets/StreamingAssets/Data/recipes.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 61479 bytes / 61479 characters.
- SHA-256: `c98b0c502df8f74f4bc498f583c0eaf659cec6320e39080de056703d85662d49`.
- Root keys: `recipes`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
recipes: min=123, max=123, observed_paths=1
recipes[].ingredients: min=1, max=1, observed_paths=2
```

Representative record fields:

- `craftingTimeHours`
- `id`
- `ingredients`
- `recipeName`
- `requiredBlueprintId`
- `requiredStationId`
- `resultAmount`
- `resultItemId`

Representative identifiers (ordered, capped for readability):

```text
craft_bandage
purify_water
craft_anti_rad
craft_water_filter
craft_air_filter
cook_meat
boil_water
craft_hazmat_patch
craft_gas_mask
refuel_heater
craft_dosimeter
craft_medical_kit
craft_geiger_counter
craft_calibration_kit
craft_battery
craft_engine
craft_frostbite_salve
craft_improvised_snow_goggles
craft_co2_scrubber
thaw_frozen_pipe
craft_mycelium_bricks
purify_black_water
craft_rebreather
craft_fungicide_fogger
craft_faraday_mesh
craft_acoustic_decoy
craft_mine_prod
craft_sound_baffling
craft_epoxy_injector
brew_lethe_substitute
craft_lead_visor
inject_concrete_pillar
recipe_water_filter
recipe_bandage
recipe_iodine_kit
recipe_rad_away
recipe_gas_mask_filter
recipe_filter_pack_carbon
recipe_inhaler
recipe_herbal_tea
reload_9x19
reload_22lr
reload_357_jhp
reload_12g_buck
reload_308_incendiary
craft_pipe_shotgun
craft_nail_driver
craft_rebar_spear
craft_molotov_thrower
craft_advanced_water_purifier
craft_desalination_still
craft_filter_reconditioning
craft_canned_rations
craft_rendered_fat
craft_press_oilseed
craft_herbal_poultice
craft_distilled_spirits
craft_fuel_gel
craft_antiseptic_solution
craft_textile_repair
craft_improvised_heater
craft_charcoal_filter
craft_pickled_tubers
craft_dried_mushrooms
craft_smoked_meat_rations
craft_canned_grain_stew
craft_salted_fish_meat
craft_rendered_fat_confit
craft_fermented_sauerkraut
craft_honey_preserved_pulp
craft_dried_herb_packets
craft_brined_legume_mash
craft_trap_improvised_wire
craft_trap_box
craft_trap_fish
craft_battery_maintenance_fluid
craft_advanced_water_filter
craft_hepa_scrubber_assembly
craft_reconditioned_deep_cycle_bank
craft_cbrn_respirator_filter
process_cloud_seeding_condensate
refit_vulcanized_diving_rig
craft_calibrated_field_geiger
transcribe_field_guide_cultivation
refit_improved_gas_mask_rig
craft_hydraulic_armored_shield
batch_hydroponic_enrichment
integrate_iff_transponder
assemble_hardened_military_transceiver
assemble_cathode_radar_scope
fabricate_radiation_blast_barrier
encode_tactical_cipher_codebook
wire_vacuum_tube_headset
synthesize_reagent_radaway
assemble_piezoelectric_geophone
program_automated_sentry_feed
assemble_pure_sine_solar_inverter
assemble_precision_surgical_arm
repack_sterile_surgical_trauma_kit
assemble_thermal_breaching_rig
preserve_rations_vacuum_canner
recipe_ballistics_refurbish_rifle
recipe_ballistics_refurbish_sidearm
recipe_aeroponics_nutrient_batch
recipe_geothermal_descaling_kit
recipe_pneumatic_capsule_50mm
recipe_pneumatic_capsule_100mm
craft_greenhouse_trowel
craft_greenhouse_watering_can
craft_greenhouse_drip_kit
craft_greenhouse_catchment_kit
recipe_trophy_wolf_head
recipe_trophy_deer_antlers
recipe_trophy_boar_tusks
recipe_trophy_fox_pelt
recipe_trophy_beetle_carapace
recipe_trophy_molerat_skull
recipe_trophy_crow_feathers
recipe_trophy_pheasant_plume
craft_silver_iodide_cartridge_bulk
recipe_trophy_ash_hound_pelt
recipe_trophy_gulden_wolf
recipe_trophy_kestrel_wings
```


# Appendix D.14 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Progression/ResearchKnowledgeCatalogLoaderTests.cs`

### `Ashfall.Core.Tests/Progression/ResearchKnowledgeCatalogLoaderTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 116; SHA-256: `644c24989c182670d728e4c41137774d39afe2dfaeb0e0574d30d1adef64d4e5`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Load_Loads56NodesFromCatalog
Load_AllNodesHaveValidIdAndDisplayName
ValidateDag_SucceedsOnAuthoritativeCatalog
ValidateDag_DetectsDirectCycle
ValidateDag_DetectsMissingPrerequisite
LoadAndRegister_PopulatesResearchSystemCatalog
```


# Appendix D.15 — Existing Focused Test Inventory: `Ashfall.Core.Tests/ResearchCatalogParityTests.cs`

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


# Appendix D.16 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Research/Plan141ResearchUnlockBridgeIntegrationTests.cs`

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


# Appendix D.17 — Existing Focused Test Inventory: `Ashfall.Core.Tests/ResearchSaveIntegrationTests.cs`

### `Ashfall.Core.Tests/ResearchSaveIntegrationTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 219; SHA-256: `50e5186c38ba61ae24dd074d2e97f6ffa5b6373bde7c1e5bbbd3a403dad74bcb`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CompletedEvent_FiresOncePerNode_OnTransitionOnly
RestoreState_DoesNotFireCompletedEvent_ButRestoresFlags
CaptureState_PreservesUnknownNodeIds
SaveRoundTrip_PreservesProgressAcrossFreshEngine
LoadAndRegister_MissingCatalog_RegistersNothing_NoFallback
Catalog_BreakthroughItemsResolve_AcrossAuthoritativeItemIds
```


# Appendix E.18 — Supporting Code Evidence: `Assets/Ashfall.Core/Research/PrewarArchiveCatalog.cs`

### `Assets/Ashfall.Core/Research/PrewarArchiveCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 70 lines / 2518 bytes.
- SHA-256: `151795f4a300e3b9b200933cda03907870944037a4d41d68ae5194b59f46ed62`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class PrewarArchiveDef
public string id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public string encryption_grade { get; set; } = "basic";
public string required_room { get; set; } = "room_laboratory_research";
public string cleaning_solvent_id { get; set; } = "chemicals";
public int cleaning_solvent_count { get; set; } = 1;
public float base_effort_points { get; set; } = 100.0f;
public List<string> reward_research_ids { get; set; } = new List<string>();
public List<string> tags { get; set; } = new List<string>();
public sealed class PrewarArchiveCatalog
public int schema_version { get; set; } = 1;
public List<PrewarArchiveDef> archives { get; set; } = new List<PrewarArchiveDef>();
public void Index() {
public PrewarArchiveDef? GetArchive(string archiveId) {
public IReadOnlyCollection<PrewarArchiveDef> GetAllArchives() => archives;
public static class PrewarArchiveCatalogLoader
public static PrewarArchiveCatalog Load(string dataDir, IFileIO fileIo) {
```


# Appendix E.19 — Supporting Code Evidence: `Assets/Ashfall.Core/Research/TechSalvageCatalog.cs`

### `Assets/Ashfall.Core/Research/TechSalvageCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 195 lines / 7266 bytes.
- SHA-256: `137a044d19b972bb99ff96b588dc1864aa74aa3a8c1a4165cec80b37ad30d710`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class TechSalvageYieldDef
public string ItemId { get; set; } = string.Empty;
public int Amount { get; set; } = 1;
public sealed class PreWarTechDef
public string Id { get; set; } = string.Empty;
public string SourceItemId { get; set; } = string.Empty;
public string DisplayNameKey { get; set; } = string.Empty;
public int Complexity { get; set; } = 1;
public int BaseResearchPoints { get; set; } = 1;
public List<TechSalvageYieldDef> BaseScrapYields { get; set; } = new List<TechSalvageYieldDef>();
public List<string> PossibleBlueprintIds { get; set; } = new List<string>();
public int BlueprintRequiredPoints { get; set; } = 10;
public List<string> RequiredResearchEquipmentTags { get; set; } = new List<string>();
public string RequiredSkillDiscipline { get; set; } = string.Empty;
public float BaseSuccessChance { get; set; } = 0.65f;
public float CatastrophicFailureChance { get; set; } = 0.05f;
public float PreservationValue { get; set; } = 0.5f;
public List<string> ResearchNotesPool { get; set; } = new List<string>();
public List<string> Tags { get; set; } = new List<string>();
public sealed class TechSalvageCatalog
public int SchemaVersion { get; set; } = 1;
public List<PreWarTechDef> Technologies { get; set; } = new List<PreWarTechDef>();
public static class TechSalvageCatalogLoader
public const string FileName = "tech_salvage.json";
public static List<PreWarTechDef> Load(string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
public static bool Validate(IEnumerable<PreWarTechDef> definitions, out string error) {
public sealed class ResearchFacilityContext
public bool IsAvailable = true;
public bool PowerStable = true;
public float EquipmentQuality01;
public List<string> EquipmentTags = new List<string>();
public ResearchFacilityContext Clone() {
public sealed class TechDismantlePreview
public string techId = string.Empty;
public string sourceItemId = string.Empty;
public float successChance;
public float catastrophicFailureChance;
public int researchPoints;
public int blueprintRequiredPoints;
public bool isAvailable;
public string failureCode = string.Empty;
public sealed class TechSalvageFailure
public string techId = string.Empty;
public string sourceItemId = string.Empty;
public string failureType = string.Empty;
public int day;
```


# Appendix E.20 — Supporting Code Evidence: `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs`

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


# Appendix E.21 — Supporting Code Evidence: `src/Host/PanelBindLifecycleSelfTest.cs`

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


# Appendix E.22 — Supporting Code Evidence: `Assets/Ashfall.Core/Research/ResearchState.cs`

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


# Appendix G.23 — Supporting Regression Evidence: `Ashfall.Core.Tests/ResearchLegacyCatalogFixture.cs`

### `Ashfall.Core.Tests/ResearchLegacyCatalogFixture.cs`

- Current test declarations: Fact=0, Theory=0, InlineData=0.
- File lines: 236; SHA-256: `b22e1549b1cd6569456e11f9820d92e760355f46183e24792f01d506a81d7339`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.


# Appendix G.24 — Supporting Regression Evidence: `Ashfall.Core.Tests/ResearchSystemTests.cs`

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


# Appendix G.25 — Supporting Regression Evidence: `Ashfall.Core.Tests/Codex/CodexProjectionTests.cs`

### `Ashfall.Core.Tests/Codex/CodexProjectionTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 155; SHA-256: `11c156d86cf478ba5f959748020fb96819f9dd26ed254dca076a629b808a75b7`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Projection_Aggregates_FromMultipleSources
Deduplication_And_ProvenanceUnion
Deterministic_Ordering_PreservesStrictSequence
```


# Appendix G.26 — Supporting Regression Evidence: `Ashfall.Core.Tests/ContentDeepChainGateTests.cs`

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


# Appendix H.27 — Supporting Authority Document: `docs/CURRENT_AUTHORITY.md`

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
| knowledge definitions, registration and DAG validation | ResearchKnowledgeCatalogLoader | eligibility, progress and completion state | ResearchSystem | Owner emits/reads a typed fact; no mirror state. |
| knowledge definitions, registration and DAG validation | ResearchKnowledgeCatalogLoader | exactly-once unlock grants and retroactive sync | ResearchUnlockBridge | Owner emits/reads a typed fact; no mirror state. |
| knowledge definitions, registration and DAG validation | ResearchKnowledgeCatalogLoader | catalog binding, panels and save composition | Research host/UI | Owner emits/reads a typed fact; no mirror state. |
| knowledge definitions, registration and DAG validation | ResearchKnowledgeCatalogLoader | DAG, parity, unlock and persistence proof | Research focused tests | Owner emits/reads a typed fact; no mirror state. |
| eligibility, progress and completion state | ResearchSystem | knowledge definitions, registration and DAG validation | ResearchKnowledgeCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| eligibility, progress and completion state | ResearchSystem | exactly-once unlock grants and retroactive sync | ResearchUnlockBridge | Owner emits/reads a typed fact; no mirror state. |
| eligibility, progress and completion state | ResearchSystem | catalog binding, panels and save composition | Research host/UI | Owner emits/reads a typed fact; no mirror state. |
| eligibility, progress and completion state | ResearchSystem | DAG, parity, unlock and persistence proof | Research focused tests | Owner emits/reads a typed fact; no mirror state. |
| exactly-once unlock grants and retroactive sync | ResearchUnlockBridge | knowledge definitions, registration and DAG validation | ResearchKnowledgeCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| exactly-once unlock grants and retroactive sync | ResearchUnlockBridge | eligibility, progress and completion state | ResearchSystem | Owner emits/reads a typed fact; no mirror state. |
| exactly-once unlock grants and retroactive sync | ResearchUnlockBridge | catalog binding, panels and save composition | Research host/UI | Owner emits/reads a typed fact; no mirror state. |
| exactly-once unlock grants and retroactive sync | ResearchUnlockBridge | DAG, parity, unlock and persistence proof | Research focused tests | Owner emits/reads a typed fact; no mirror state. |
| catalog binding, panels and save composition | Research host/UI | knowledge definitions, registration and DAG validation | ResearchKnowledgeCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| catalog binding, panels and save composition | Research host/UI | eligibility, progress and completion state | ResearchSystem | Owner emits/reads a typed fact; no mirror state. |
| catalog binding, panels and save composition | Research host/UI | exactly-once unlock grants and retroactive sync | ResearchUnlockBridge | Owner emits/reads a typed fact; no mirror state. |
| catalog binding, panels and save composition | Research host/UI | DAG, parity, unlock and persistence proof | Research focused tests | Owner emits/reads a typed fact; no mirror state. |
| DAG, parity, unlock and persistence proof | Research focused tests | knowledge definitions, registration and DAG validation | ResearchKnowledgeCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| DAG, parity, unlock and persistence proof | Research focused tests | eligibility, progress and completion state | ResearchSystem | Owner emits/reads a typed fact; no mirror state. |
| DAG, parity, unlock and persistence proof | Research focused tests | exactly-once unlock grants and retroactive sync | ResearchUnlockBridge | Owner emits/reads a typed fact; no mirror state. |
| DAG, parity, unlock and persistence proof | Research focused tests | catalog binding, panels and save composition | Research host/UI | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace 15→40 with a 62-node current census and DAG/parity matrix. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Separate knowledge-node authority from unlock-row authority and downstream recipe/item/capability ownership. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Require every unlock target to resolve through current catalogs or an explicit capability owner. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Preserve exactly-once completion, retroactive synchronization and no-event-on-restore behavior. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.559 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentDeepChainGate.cs`

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


# Appendix Q.560 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

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


# Appendix Q.561 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Codex/CodexProjectionBuilder.cs`

### `Assets/Ashfall.Core/Codex/CodexProjectionBuilder.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 366 lines / 18515 bytes.
- SHA-256: `e13379e433814fe417ac6090a719688a7f5bc6a6ac2d2c3ea10421fcd36875f6`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class CodexProjectionBuilder
public static IReadOnlyList<CodexEntryProjection> Build( FieldGuideCatalog? fieldGuide, ResearchState? researchState, IReadOnlyDictionary<string, ResearchKnowledgeDef>? researchCatalog, JournalSystem? journalSystem, int currentDay = 1,
```


# Appendix Q.562 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Archaeology/ArchaeologySystem.cs`

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


# Appendix Q.563 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/CollectibleCatalogIntegrityValidator.cs`

### `Assets/Ashfall.Core/Content/CollectibleCatalogIntegrityValidator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 432 lines / 20841 bytes.
- SHA-256: `fad9423c257b5fcc2c8e38b078acf1669cc01e3cd3156e8b2764f3c79703dc43`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CollectibleIntegrityFinding
public string SourceCatalog { get; }
public string SourceId { get; }
public string FieldPath { get; }
public string TargetId { get; }
public string TargetCatalog { get; }
public string ErrorCode { get; }
public string Message { get; }
public override string ToString() =>
public static class CollectibleCatalogIntegrityValidator
public static readonly HashSet<string> ValidCategories = new HashSet<string>(StringComparer.Ordinal) {
public static readonly HashSet<string> ValidRarities = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
public static readonly HashSet<string> ValidEffectTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
public static List<CollectibleIntegrityFinding> Validate( string dataDir, IFileIO fileIO, IJsonSerializer json, ILog? log = null) {
public sealed class ItemFileRootDto
public int schema_version { get; set; } = 1;
public List<ItemHeaderDto> items { get; set; } = new List<ItemHeaderDto>();
public sealed class ItemHeaderDto
public string id { get; set; } = string.Empty;
public sealed class JournalVoiceProseFileRaw
public int schema_version { get; set; } = 1;
public Dictionary<string, Dictionary<string, string>> prose_variants { get; set; } =
```


# Appendix Q.564 — Additional Current Architecture Evidence: `src/Host/CodexHostSession.cs`

### `src/Host/CodexHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 132 lines / 5551 bytes.
- SHA-256: `79dc646ce6df36cfcea262b37953ab42519cda19a46b0bafc29580bc614000cb`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CodexHostSession
public static CodexHostSession Create(string dataDir, ILog? log = null) {
public int AuthoredEntryCount => _authored.Count;
public IReadOnlyList<AuthoredCodexEntry> AuthoredEntries => _authored;
public IReadOnlyList<CodexEntryProjection> Build( FieldGuideCatalog? fieldGuide, ResearchState? researchState, IReadOnlyDictionary<string, ResearchKnowledgeDef>? researchCatalog, JournalSystem? journal, int currentDay,
public IReadOnlyList<CodexEntryProjection> BuildKnown( FieldGuideCatalog? fieldGuide, ResearchState? researchState, IReadOnlyDictionary<string, ResearchKnowledgeDef>? researchCatalog, JournalSystem? journal, int currentDay,
public IReadOnlyList<CodexEntryProjection> BuildForLocation( string locationId, JournalSystem? journal, int currentDay, Func<string, bool>? factionContact = null, int maxSpoilerTier = int.MaxValue)
public IReadOnlyDictionary<CodexCategory, int> KnownCountByCategory( FieldGuideCatalog? fieldGuide, ResearchState? researchState, IReadOnlyDictionary<string, ResearchKnowledgeDef>? researchCatalog, JournalSystem? journal, int currentDay,
```


# Appendix Q.565 — Additional Current Architecture Evidence: `src/Main.UiTests.PlayerPanels.cs`

### `src/Main.UiTests.PlayerPanels.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 236 lines / 10980 bytes.
- SHA-256: `d452fcebe3e50f0eeb0804046534750abec83d9794cd5bcece126922745e9d62`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.566 — Additional Current Architecture Evidence: `src/Host/ResearchSaveStore.cs`

### `src/Host/ResearchSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 56 lines / 2673 bytes.
- SHA-256: `7cfac6563ed2ff940a713b15b329fd738bf578ad171deb1c219858d8a9ef507a`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class ResearchSaveStore
public const string FileName = "research_save.json";
public const string SectionName = "research";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCaptureDirect(ResearchState state) => s_store.CaptureBare(state);
public static ResearchState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(ResearchState state) => s_store.CaptureBare(state);
public static ResearchState? TryRestore(string json) => s_store.RestoreBare(json);
public static bool TrySave(ResearchState state) => s_store.TrySave(state);
public static ResearchState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(ResearchState state) => s_store.CapturePersisted(state);
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


# Appendix Q.568 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Collectibles/CollectibleEffectDispatcher.cs`

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


# Appendix Q.570 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/LibraryStudySystem.cs`

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


# Appendix Q.571 — Additional Current Architecture Evidence: `src/Host/CraftingHostSession.cs`

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


# Appendix Q.572 — Additional Current Architecture Evidence: `src/Main.UiPanels.cs`

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


# Appendix Q.573 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Research/PrewarArchiveDecryptionSystem.cs`

### `Assets/Ashfall.Core/Research/PrewarArchiveDecryptionSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 242 lines / 9793 bytes.
- SHA-256: `ec6d0d7c1e74534b716244d4b78e4c762f133b02721796e89ccaf31c75a80222`.
- Architecture signals: seeded references=3; save/restore symbols=2; typed event declarations=8; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum ArchiveDecryptionStatus
public sealed class PrewarArchiveProject
public string ArchiveId { get; set; } = string.Empty;
public ArchiveDecryptionStatus Status { get; set; }
public float Progress { get; set; }
public float TargetProgress { get; set; }
public string AssignedResearcherId { get; set; } = string.Empty;
public int DayStarted { get; set; }
public int DayCompleted { get; set; }
public bool HasSolventApplied { get; set; }
public sealed class PrewarArchiveDecryptionState
public string SystemId { get; set; } = PrewarArchiveDecryptionSystem.SystemId;
public List<PrewarArchiveProject> Projects { get; set; } = new List<PrewarArchiveProject>();
public List<string> DiscoveredArchiveIds { get; set; } = new List<string>();
public List<string> CompletedArchiveIds { get; set; } = new List<string>();
public bool IsPowerOnline { get; set; } = true;
public int TotalDecrypted { get; set; }
public int TotalBreakthroughs { get; set; }
public sealed class PrewarArchiveDecryptionSystem
public const string SystemId = "prewar_archive_decryption";
public PrewarArchiveDecryptionState State => _state;
public bool IsPowerOnline => _state.IsPowerOnline;
public int TotalDecrypted => _state.TotalDecrypted;
public event Action<string>? OnArchiveDiscovered;
public event Action<PrewarArchiveProject>? OnArchiveStabilized;
public event Action<PrewarArchiveProject, PrewarArchiveDef>? OnArchiveDecrypted;
public event Action<PrewarArchiveProject, float>? OnDecryptionBreakthrough;
public void SetPowerStatus(bool isOnline) {
public ActionResult DiscoverArchive(string archiveId, int currentDay) {
public ActionResult StabilizeArchive(string archiveId, string researcherId, int currentDay) {
public ActionResult StartDecryption(string archiveId, string researcherId, int currentDay) {
public ActionResult AssignResearcher(string archiveId, string researcherId) {
public void TickDay(int day) {
public PrewarArchiveProject? GetProject(string archiveId) {
public PrewarArchiveDecryptionState CaptureState() {
public void RestoreState(PrewarArchiveDecryptionState saved) {
```


# Appendix Q.574 — Additional Current Architecture Evidence: `src/Host/ContentUtilizationRuntimeCollector.cs`

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


# Appendix Q.575 — Additional Current Architecture Evidence: `src/Main.ResearchUnlock.cs`

### `src/Main.ResearchUnlock.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 77 lines / 2443 bytes.
- SHA-256: `90270a1e3a96d34c920abe708592a9022ba230cf9e45d439708af6a47f7b9bd8`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public ResearchUnlockHostSession? ResearchUnlock => _researchUnlock;
public void SetupResearchUnlockBridge() {
public void SaveResearchUnlock() {
public void TickResearchUnlock(int day) {
public void FlushResearchUnlockIfDirty() {
public void ResetResearchUnlock() {
```


# Appendix Q.576 — Additional Current Architecture Evidence: `src/Host/HostCli.Collectibles.cs`

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


# Appendix Q.577 — Additional Current Architecture Evidence: `src/Host/HostCli.SelfTests.cs`

### `src/Host/HostCli.SelfTests.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1232 lines / 59617 bytes.
- SHA-256: `a56bb8a0a9355f3575257da4bc68ff518c6fc9dfa1fdb5c3f2b98ad371fea61d`.
- Architecture signals: seeded references=1; save/restore symbols=1; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=1.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunDataIntegritySelfTest(string dataDirectory) {
public static int RunResearchCatalogSelfTest(string dataDirectory) {
public static int RunRadioCatalogSelfTest(string dataDirectory) {
public static int RunExpansionsSelfTest(string dataDirectory) {
public static int RunDeepCoastSelfTest(string dataDirectory) {
public static int RunWarlordSelfTest(string dataDirectory) {
public static int RunWarlordHostSelfTest(string dataDirectory) {
public static int RunWarlordUiSelfTest(string dataDirectory) {
public static int RunDeepCoastHostSelfTest(string dataDirectory = null!) {
public static int RunGreenhouseSelfTest() {
public static int RunSilentFoundrySelfTest(string dataDirectory) {
public static int RunDiseaseSelfTest(string dataDirectory) {
public static int RunCombatSelfTest(string dataDirectory) {
public static int RunArbitrationSelfTest() {
public static int RunLedgerDebtSelfTest() {
public static int RunPatrolEncounterSelfTest(string dataDirectory) {
public static int RunHoldfastSelfTest(string dataDirectory) {
public static int RunDutyRosterSelfTest(string dataDirectory) {
public static int RunStandingRecordSelfTest(string dataDirectory) {
public static int RunCrossingSelfTest(string dataDirectory) {
public static int RunIceRoadSelfTest(string dataDirectory) {
public static int RunCensusSelfTest() {
public static int RunBrineSelfTest() {
public static int RunMusterSelfTest() {
public static int RunFactionEcologySelfTest(string dataDirectory) {
public static int RunVerdictSelfTest(string dataDirectory) {
public long LivingRegisteredSouls() => _n;
public static int RunClusterSelfTest(string dataDirectory) {
public static int RunEndingsSelfTest() {
public static int RunJournalSaveSelfTest() {
public static int RunChemicalDependencySaveSelfTest() {
public static int RunContrabandStashSelfTest() {
public static int RunMedicalWardSaveSelfTest() {
public static int RunWeatherSaveSelfTest() {
public static int RunJournalWeatherPanelSelfTest() {
public static int RunInventorySaveSelfTest() {
public static int RunSaveLoadUiFailureSelfTest(string dataDirectory) {
public static int RunPanelBindLifecycleSelfTest(string dataDirectory) {
public static int RunSaveStoreChecksumSelfTest(string dataDirectory) {
public static int RunSevenDayDeterministicSmokeSelfTest(string dataDirectory) {
public static int RunUiAccessibilitySelfTest() {
public static int RunCoreSelfTest(string dataDirectory) {
public static int RunCatalogBootPreflight(string dataDirectory) {
public static int RunCampaignFuzzSelfTest(string dataDirectory) {
```


# Appendix Q.578 — Additional Current Architecture Evidence: `src/Main.PanelLifecycle.cs`

### `src/Main.PanelLifecycle.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 306 lines / 10902 bytes.
- SHA-256: `f65e95e260df8647148a77d4e9c3ac644ab97c42ba8c8cba85b861d04938aa3f`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.579 — Additional Current Architecture Evidence: `src/Host/WaterSourcesHostSession.cs`

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


# Appendix Q.580 — Additional Current Architecture Evidence: `src/Main.PlayerSurfaces.cs`

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


# Appendix Q.581 — Additional Current Architecture Evidence: `src/Main.CampaignOwners.cs`

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


# Appendix Q.582 — Additional Current Architecture Evidence: `src/Main.GameFlow.cs`

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


# Appendix R.583 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Research/ResearchQueueEligibilityTests.cs`

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


# Appendix R.584 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Plan166ResearchSalvageTests.cs`

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


# Appendix R.585 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Collectibles/CollectibleResearchIntegrationTests.cs`

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


# Appendix R.586 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Archaeology/ArchaeologySystemTests.cs`

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


# Appendix R.587 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Integration/Roadmap42Batch1IntegrationTests.cs`

### `Ashfall.Core.Tests/Integration/Roadmap42Batch1IntegrationTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 410; SHA-256: `58130dae9d890fd61d10c85f141e3fa59852d2b910e0cba09663d9b5a5ea7155`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Test1_ReferentialIntegrity_CrossCatalogContracts_AllResolve
Test2_FullDeterministicCampaignJourney_TouchesAllTenSystems_ReplaysIdentically
Test3_Batch1CatalogCountsAndLoaderClassifications_MatchAuthoritativeTruth
```


# Appendix R.588 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Library/LibraryStudyContractTests.cs`

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


# Appendix R.589 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Collectibles/CollectibleCampaignSmokeTests.cs`

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


# Appendix R.590 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Plan166_169ReloadReplayTests.cs`

### `Ashfall.Core.Tests/Plan166_169ReloadReplayTests.cs`

- Current test declarations: Fact=2, Theory=0, InlineData=0.
- File lines: 352; SHA-256: `eab80782e8bb82232aa480340f7962b93e9a3dbd27b89b2577e2127ac7252fe3`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ContinuousVersusMidCampaignReload_EndsWithEqualCanonicalState
ReloadDoesNotReplayEspionageIntelOrDoubleApplySupplyDisruption
```


# Appendix R.591 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/ProductionSliceTests.cs`

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


# Appendix R.592 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Progression/Plan80LibraryManualsExpansionTests.cs`

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


# Appendix R.593 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Research/Plan141ResearchUnlockHostIntegrationTests.cs`

### `Ashfall.Core.Tests/Research/Plan141ResearchUnlockHostIntegrationTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 135; SHA-256: `b729dbf70c49d35c0f1437c8eb30dfa301fc58b07b1dd7d1117f70795582f7bf`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Bridge_GetCensus_ReportsValidCounts
Bridge_ProcessResearchCompletion_GrantsItemsAndRecipesIdempotently
Bridge_Queries_ReflectGrantedState
Bridge_SynchronizeCompletedResearch_HandlesPreExistingNodes
Bridge_StateRoundTrip_PreservesAllUnlocks
```


# Appendix R.594 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Collectibles/CollectibleSaveMigrationTests.cs`

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


# Appendix R.595 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Integration/Plans186_189_CampaignContinuityTests.cs`

### `Ashfall.Core.Tests/Integration/Plans186_189_CampaignContinuityTests.cs`

- Current test declarations: Fact=1, Theory=0, InlineData=0.
- File lines: 115; SHA-256: `5b874ee1ae076370aa2475de61998050f05783f9c9f5c48ca5c495be54e381ea`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plans186_189_FullContinuityPipeline_IntegratesAcrossAllFourSystems
```


# Appendix R.596 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Shelter/Plan29_34ShelterResearchIntegrationTests.cs`

### `Ashfall.Core.Tests/Shelter/Plan29_34ShelterResearchIntegrationTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 124; SHA-256: `beba480ad7bd6e0424c1fed9a2f7b500eb75814435cd030119f6359c894f2b2b`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ShelterRoomAndMachineCatalogs_LoadFromAuthoritativeJson
ResearchKnowledgeCatalog_LoadsFromAuthoritativeJson_AndValidatesTechTree
ShelterInfrastructure_And_ResearchTree_CoexistAndCrossValidate
```


# Appendix R.597 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Collectibles/CollectibleDispatcherHardeningTests.cs`

### `Ashfall.Core.Tests/Collectibles/CollectibleDispatcherHardeningTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 295; SHA-256: `b06445d21c0eda460dd08ee20dafa80650177fbacc1615b97ee19f791f2e3d4f`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ManualAcquisition_RevealsKnowledgeNode_NeverCompletesIt
ManualReveal_UnrelatedNodesUnchanged
ManualAcquisition_AllFiveAuthoredTargetsReveal
MapClue_RevealsSurveyed_NeverVisited_WithClueProvenance
MapClue_RouteDiscoveryStateUntouched
MapClue_UnknownNode_TypedFailure_DiscoveryNotRegistered
KnowledgeUnknownTarget_TypedFailure_DiscoveryNotRegistered
Reacquisition_Idempotent_NoDuplicateDiscoveryEvent
VinylCollectible_DispatcherAppliesNoMorale
```


# Appendix R.598 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/EventSurfaceArchitectureTests.cs`

### `Ashfall.Core.Tests/EventSurfaceArchitectureTests.cs`

- Current test declarations: Fact=3, Theory=0, InlineData=0.
- File lines: 186; SHA-256: `5992579593bd1b3bf5a64abc2d7dedec3a9b8a2d7f7edb7e084cdc4148130085`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RestoreState_SuppressesMutationEvents_AcrossKeyDomains
MutationToEventOrdering_StateIsConsistent_WhenHandlerExecutes
ArchitectureGuard_IEventBus_IsStrictlyBoundedToAllowlist
```


# Appendix R.599 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Medical/AutopsyProceduresCatalogTests.cs`

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


# Appendix R.600 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/Phase1SharedContractsTests.cs`

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


# Appendix R.601 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/WorkshopReverseEngineeringSystemTests.cs`

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


# Appendix R.602 — Additional Focused Regression Evidence: `Ashfall.Core.Tests/CampaignContinuityFlagshipTests.cs`

### `Ashfall.Core.Tests/CampaignContinuityFlagshipTests.cs`

- Current test declarations: Fact=2, Theory=0, InlineData=0.
- File lines: 217; SHA-256: `6eaa29400ca3b5038ea02f563f428f56d214bb71f7b6d2b3dfb54eba0669b4a0`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CampaignContinuity_30DayDeterministicReplay_ExactMatch
CampaignContinuity_SaveLoadSplitAtDay15_MatchesContinuousRun
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.


# Appendix S — Quality Assurance Pass Record: Plan 34

This record is part of the planning artifact, not a fresh runtime test result.

## Pass 1 — content and premise accuracy
- Content pass replaced the 15-hardcoded/40-node premise with 62 nodes and 30 unlocks.
- The historical baseline is separated from the current source/data/test authority.
- Current row counts and owner boundaries are stated without using count as a quality proxy.

## Pass 2 — integration architecture
- Integration pass traced DAG validation → research completion → unlock bridge → consumers.
- Core, data, host, UI, save, event and test seams are named with current paths.
- The plan does not authorize a parallel save section, catalog, manager or host cache.

## Final precision and reaccuracy pass
- Precision pass requires acyclic references and no completion re-fire on restore.
- Every embedded current-file hash, focused runner command and master-authority reference is rechecked.
- Any proposed future seam is labeled as requiring a separate claim and premise verification.
