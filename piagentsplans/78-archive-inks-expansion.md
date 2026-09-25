# Plan 78 — Archive Ink Catalog and Transcription Conservation Loop

> **Rebuild status:** COMPLETE 12-INK CONTENT LOOP — CONSERVATION AND REACHABILITY MAINTENANCE
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

- Archive inks are a meaningful preservation trade-off: legibility, longevity, fade rate and ingredient cost should remain distinct and non-dominant. The current 12-row catalog is already the authored authority.
- The live route is `archive_inks.json` → `ArchiveInkCatalogLoader` → `ArchiveDeskSystem` queue/transcription → knowledge/document results → existing archive state. The plan must not create a parallel ink economy or document manager.
- The remaining quality work is to protect content provenance, avoid universal-best rows, ensure required ingredients resolve in the merged item catalog, and verify the current runtime path after any catalog change.

**Bounded outcome:** Retire the old 3→12 data-only brief as an implementation gap. The current archive ink catalog has 12 validated rows, the loader registers them with the archive desk, and focused tests cover ingredients, quality trade-offs, transcription and save restore. The rebase is a conservation-quality maintenance plan.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `archive_inks.json` is present with schema version 1 and 12 unique `ink_id` rows.
- `ArchiveInkCatalogLoader` loads and registers the catalog; `ArchiveDeskSystem` consumes ingredient amounts, legibility and queue state.
- `ArchiveInksCatalogTests` covers exact 12 rows, original three preservation, unique IDs, ingredient resolution, numeric ranges, non-dominance, queue consumption, transcription and save restore.
- The catalog is a content authority; inventory, document and knowledge owners remain responsible for their state.

**Master-authority sections applied to this rebase:**

- Master authority Volume 28 verification cookbook: focused evidence before broad gates.
- Lane D save/state/compatibility guidance: owner DTOs, migration and restore proof.
- Lane E UI/UX/accessibility guidance: truthful projections and keyboard/controller lifecycle.
- Lane G testing guidance: smallest affected target, negative cases and deterministic replay.
- Anti-padding protocol: content exhaustion may end the plan before the character checkpoint.
- Volume 32 cultural/archive restoration log.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the old 3→12 count target with a 12-row conservation census and quality matrix.
- Keep every required ingredient, quality dimension and trade-off visible to future content authors.
- Add a reachability proof from each ink to an archive desk action and resulting document state.
- Preserve save/queue behavior and avoid new parallel item or document stores.

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
| ink definitions and registration | ArchiveInkCatalogLoader | `Assets/Ashfall.Core/ArchiveInkCatalogLoader.cs` | Sole archive-ink catalog loader. |
| queue, ingredient consumption and transcription | ArchiveDeskSystem | `Assets/Ashfall.Core/ArchiveDeskSystem.cs` | Owns desk work and document result state. |
| ingredient availability and consumption | Inventory authority | `Assets/Ashfall.Core/Inventory/Inventory.cs` | Owns physical item quantities. |
| catalog and conservation proof | Archive focused tests | `Ashfall.Core.Tests/ArchiveInksCatalogTests.cs` | Executable current contract. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Archive Ink Catalog and Transcription Conservation Loop
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ ArchiveInkCatalogLoader
│   ink definitions and registration
│ ArchiveDeskSystem
│   queue, ingredient consumption and transcription
│ Inventory authority
│   ingredient availability and consumption
│ Archive focused tests
│   catalog and conservation proof
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

1. **Preserve current state ownership.** ArchiveInkCatalogLoader owns ink definitions and registration: Sole archive-ink catalog loader.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| ink definitions and registration | ArchiveInkCatalogLoader | `Assets/Ashfall.Core/ArchiveInkCatalogLoader.cs` | Sole archive-ink catalog loader. |
| queue, ingredient consumption and transcription | ArchiveDeskSystem | `Assets/Ashfall.Core/ArchiveDeskSystem.cs` | Owns desk work and document result state. |
| ingredient availability and consumption | Inventory authority | `Assets/Ashfall.Core/Inventory/Inventory.cs` | Owns physical item quantities. |
| catalog and conservation proof | Archive focused tests | `Ashfall.Core.Tests/ArchiveInksCatalogTests.cs` | Executable current contract. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. Load ink definitions
2. resolve required item through canonical inventory catalog
3. queue a document with a selected ink
4. atomically consume configured ingredients
5. apply configured legibility/fade/longevity rules
6. write the existing document/knowledge result
7. capture and restore desk state

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Ink definitions are immutable catalog rows; queue and transcription progress are owner state.
- An ink is consumed atomically with its configured ingredient amount.
- Quality dimensions are bounded and comparable; no row may strictly dominate all alternatives at equal or lower cost.
- Restore preserves queue and transcription state without duplicating consumed ingredients.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Unknown ink fails closed without consuming inventory.
- Ingredient IDs must resolve in the merged item authority.
- The same queued document and ink produce the same result for the same state and seed.
- The plan must not make a new permanent ink collection.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `archive_inks.json` remains the sole ink catalog.
- New rows require a real consumer, resolved ingredients and a distinct quality/cost profile.
- Do not infer effects from prose; the loader schema is the contract.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use the existing ArchiveDeskSystem save state.
- No new save section is justified by a catalog-only change.
- A queue restore must not re-consume ingredients or re-run a completed transcription.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Transcription and quality selection use existing seeded/system contracts.
- Catalog ordering must not depend on hash iteration order.
- Two identical runs produce identical queue and result state.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Archive desk queue/transcription facts are emitted by the current desk owner.
- No new event vocabulary is required for a catalog-only maintenance change.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/UI/ArchiveDeskPanel.cs
- src/Host/ContentUtilizationRuntimeCollector.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Ink names and descriptions should communicate archival trade-offs without real-world copied text.
- A rare archival ink is a resource decision, not a magic permanent unlock.
- The tone is practical, restrained and compatible with evidence-room language.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | An ink references an absent item. | ArchiveInkCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A universal-best ink makes all other rows decorative. | ArchiveDeskSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | Queue consumption succeeds while transcription fails to restore. | Inventory authority | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | The UI displays a row the loader did not register. | Archive focused tests | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A new document store duplicates ArchiveDeskSystem. | ArchiveInkCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/ArchiveInksCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Culture/Plan90_78DoseInksIntegrationTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — current census | Read catalog, loader, desk and 12-row tests. | Current count and owner are proven. | No production path until the owning implementation package is separately claimed. |
| 1 — quality matrix | Check uniqueness, ranges, ingredients and trade-offs. | No dominant or orphaned row. | No production path until the owning implementation package is separately claimed. |
| 2 — runtime proof | Trace queue, consume, transcribe and restore. | Focused loop passes. | No production path until the owning implementation package is separately claimed. |
| 3 — content seal | Review new-row admission and tone. | Future row has a consumer and evidence. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/archive_inks.json | READ ONLY; MODIFY only for a proven row gap | 12-row authority |
| Assets/Ashfall.Core/ArchiveInkCatalogLoader.cs | READ ONLY | Loader |
| Assets/Ashfall.Core/ArchiveDeskSystem.cs | READ ONLY | Runtime owner |
| Ashfall.Core.Tests/ArchiveInksCatalogTests.cs | EXTEND only for a proven gap | Focused proof |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Adding effects not supported by the schema. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Breaking ingredient resolution by editing items separately. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Making archive rows a second document authority. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Padding the catalog with duplicate profiles. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new ink mechanic.
- No new archive save section.
- No production/data edits in this rebase.
- No arbitrary row-count growth.

# 23. Rollback and Recovery

- Revert the plan document.
- Future data changes require the previous valid JSON and focused catalog/runtime tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- The 12-row authority and current loader are explicit.
- Conservation trade-offs and runtime route are documented.
- No duplicate owner or save path is proposed.
- Focused verification commands are exact.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the old 3→12 count target with a 12-row conservation census and quality matrix.
- Keep every required ingredient, quality dimension and trade-off visible to future content authors.
- Add a reachability proof from each ink to an archive desk action and resulting document state.
- Preserve save/queue behavior and avoid new parallel item or document stores.

## MUST NOT DO

- No new ink mechanic.
- No new archive save section.
- No production/data edits in this rebase.
- No arbitrary row-count growth.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/ArchiveInksCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Culture/Plan90_78DoseInksIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — current census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: ink definitions and registration → ArchiveInkCatalogLoader; queue, ingredient consumption and transcription → ArchiveDeskSystem; ingredient availability and consumption → Inventory authority; catalog and conservation proof → Archive focused tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 78.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 78 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by ArchiveInkCatalogLoader or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/ArchiveInkCatalogLoader.cs`

### `Assets/Ashfall.Core/ArchiveInkCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 51 lines / 1764 bytes.
- SHA-256: `8a96a58c771a128c26230333377925524ffc14ed034e910d8b492ac187d2853a`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ArchiveInkCatalogContainer
public List<InkMaterialDefinition> inks = new List<InkMaterialDefinition>();
public static class ArchiveInkCatalogLoader
public const string DefaultFileName = "archive_inks.json";
public static List<InkMaterialDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static int LoadAndRegister( ArchiveDeskSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/ArchiveDeskSystem.cs`

### `Assets/Ashfall.Core/ArchiveDeskSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 204 lines / 8404 bytes.
- SHA-256: `9bde74b2c74012b2c4e4c47cef31fc7e91b2145a1e6c6f1a9b2ff787d8bd91f2`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ArchiveDeskState
public string systemId = ArchiveDeskSystem.SystemId;
public List<TranscriptionJob> queue = new List<TranscriptionJob>();
public List<string> unlockedEvidenceIds = new List<string>();
public int totalTranscriptions;
public sealed class InkMaterialDefinition
public string ink_id = string.Empty;
public string display_name = string.Empty;
public float legibilityScore = 1f;      // 0-1
public float archivalLongevityDays = 365f;
public float fadeRatePerDay = 0.001f;
public string requiredItemId = string.Empty;
public int requiredAmount = 1;
public sealed class TranscriptionJob
public string jobId = string.Empty;
public string evidenceId = string.Empty;
public string archivistId = string.Empty;
public string inkId = string.Empty;
public int dayStarted = -1;
public float progressHours;
public float totalHoursRequired = 4f;
public bool isComplete;
public bool isCancelled;
public float legibilityScore = 1f;
public string journalEntryId = string.Empty;
public sealed class ArchiveDeskSystem
public string Id { get; set; } = string.Empty;
public string DisplayName { get; set; } = string.Empty;
public RiskBiasTrait RiskBias { get; set; } = RiskBiasTrait.Realist;
public const string SystemId = "archive_desk";
public ArchiveDeskState State => _state;
public IReadOnlyDictionary<string, InkMaterialDefinition> Catalog => _inkCatalog;
public event Action<TranscriptionJob> OnJobCompleted;
public event Action OnArchiveChanged;
public void LoadInkCatalog(List<InkMaterialDefinition> inks) {
public ActionResult QueueTranscription(string evidenceId, string archivistId, string inkId) {
public ActionResult CancelJob(string jobId) {
public void TickDay(int day) {
public List<TranscriptionJob> GetActiveJobs() => _state.queue.FindAll(j => !j.isComplete && !j.isCancelled);
public bool IsEvidenceUnlocked(string evidenceId) => _state.unlockedEvidenceIds.Contains(evidenceId);
public ArchiveDeskState CaptureState() => CloneState(_state);
public void RestoreState(ArchiveDeskState saved) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Inventory/Inventory.cs`

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


# Appendix B.05 — Current Code Architecture: `src/UI/ArchiveDeskPanel.cs`

### `src/UI/ArchiveDeskPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 302 lines / 14057 bytes.
- SHA-256: `ebef244e3e3db9738bed43807cbb6244322a490ba9f17f9d101e4a8d76346e99`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=3; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class ArchiveDeskPanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _host != null;
public void Bind(ArchiveDeskHostSession session) {
public void Unbind() {
public override void _Ready() {
public void Open() {
public void RefreshView() {
public override void _UnhandledInput(InputEvent @event) {
public override void _ExitTree() {
```


# Appendix B.06 — Current Code Architecture: `src/Host/ContentUtilizationRuntimeCollector.cs`

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


# Appendix C.07 — Catalog Census: `Assets/StreamingAssets/Data/archive_inks.json`

### `Assets/StreamingAssets/Data/archive_inks.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 3191 bytes / 3191 characters.
- SHA-256: `83fc3d37907fc3e632829d56da3dcc4a83605f91001bee0f102a854ac25e9a5e`.
- Root keys: `collection_id`, `inks`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
inks: min=12, max=12, observed_paths=1
```

Representative record fields:

- `archival_longevity_days`
- `display_name`
- `fade_rate_per_day`
- `ink_id`
- `legibility_score`
- `required_amount`
- `required_item_id`


# Appendix C.08 — Catalog Census: `Assets/StreamingAssets/Data/items.json`

### `Assets/StreamingAssets/Data/items.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 390056 bytes / 390056 characters.
- SHA-256: `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`.
- Root keys: `items`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
items: min=724, max=724, observed_paths=1
```

Representative record fields:

- `category`
- `contamination`
- `degradeRate`
- `description`
- `disassembleYieldFraction`
- `displayName`
- `display_name`
- `durability`
- `empShielded`
- `equipSlot`
- `healthEffect`
- `hungerRestore`
- `id`
- `isEquipable`
- `moraleEffect`
- `radCleanse`
- `radProtection`
- `repairCosts`
- `repairRecipe`
- `scrapValue`
- `stackMax`
- `tags`
- `thirstRestore`
- `tradeValue`
- `type`
- `value`
- `weight`
- `weight_kg`

Representative identifiers (ordered, capped for readability):

```text
item_decon_chelator_concentrate
item_lead_lined_effluent_filter
item_heavy_neoprene_scrub_brush
item_sealed_waste_bin
item_theodolite_brass_precision
item_surveyor_stadia_rod
item_datum_plate_bronze
item_concrete_mix
item_forged_rotor_shaft
item_magnetic_bearing_coil
item_high_vacuum_pump
item_containment_ring_steel
item_reinforced_concrete_vault
item_seismic_damper_pad
item_vacuum_pump_oil
item_bearing_grease
item_rotor_balancing_kit
item_portable_pid_detector
item_detector_sensor_module
item_hermetic_sample_ampoule
item_hot_dust_drum
item_sludge_cake
item_tailings_drum
dosimeter
geiger_counter
iodine_pills
anti_rad
gas_mask
hazmat_suit
water_filter
air_filter
clean_water
irradiated_water
canned_food
fuel
cloth
scrap_metal
bandage
raw_meat
cooked_meat
dirty_water
morphine
chelation_agent
potassium_iodide
medical_kit
battery
calibration_kit
tweezers
splint
antibiotics
jewelry
diamond
currency
mechanical_parts
electronic_scrap
item_radiosonde
solar_cell
chemicals
handheld_radio
engine
roots
berries
vacuum_tube
spring_mechanism
phonograph_needle
projector_bulb
lubricant_oil
film_reel
antenna_coil
soldering_kit
music_box_comb
spring_key
typewriter_ribbon
machine_oil
camera_lens_cleaner
photographic_film
item_acoustic_decoy
item_ammonium_nitrate_sack
item_amnestic_syrup
item_anchor_notes
item_ash_ghillie
item_bio_plastic
item_black_water_vial
item_co2_scrubber_cartridge
item_epoxy_injector
item_faraday_mesh
item_frostbite_salve
item_fungicide_fogger
item_galvanized_rebar
item_glycol_antifreeze_canister
item_hermetic_hatch_silicone_gasket
item_high_tensile_steel_culvert_brace
item_insulated_snowmobile_battery
item_lead_shielded_sample_cask
item_lead_visor
item_lithium_salts
item_mine_prod
item_mycelium_bricks
item_prussian_blue_chelating_pellets
item_radon_detector_electret
item_rebreather_scrubber
item_ro_membrane
item_scopolamine_root
item_sealed_lead_pig
item_snow_goggles_improvised
item_sound_baffling
item_suitcase_locked
item_surgical_bone_chisel
item_teddy_bear
item_thermal_paste
item_welders_glass
aa_batteries
alcohol_wipes_box_10_of_10
ammo_762x54r_jhp_ap
ammo_357
ammo_12g
ammo_308
ammo_556
ammo_762
antiseptic_1l_of_1l
battery_pack
box_of_nails_10
canned_soup
childrens_books
cigarette_lighter
clean_water_jug
cooking_oil
copper_wire_10m_of_10m
diesel_fuel
dried_rations
faraday_pack
field_surgical_kit
fuel_1l
fuel_cell
growing_manual
iodine_tablets
item_cassette_tape
item_pre_war_photo_album
item_vinyl_collection
mechanical_components
medkit
metal_pipe
military_grade_hatchet
military_mre
military_radio
military_rations
military_supply_crate
music_box_fur_elise
night_vision_scope
plastic_material
scrap_plastic
synthetic_fuel_canister
carbon_black_powder
protective_childs_coat
rubber_hose
scrap_wood
sealed_government_document
seed_packets
spirits
steel_rebar
```


# Appendix D.09 — Existing Focused Test Inventory: `Ashfall.Core.Tests/ArchiveInksCatalogTests.cs`

### `Ashfall.Core.Tests/ArchiveInksCatalogTests.cs`

- Current test declarations: Fact=12, Theory=0, InlineData=0.
- File lines: 273; SHA-256: `c6b0a6e6f1e63c7b40c97f0ea0d06db9bae00a9ab740b6773e6899c535b12065`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsExact12Inks
Catalog_OriginalThreeInksPreserved
Catalog_AllTwelveIdsUniqueAndPrefixed
Catalog_AllDisplayNamesNonEmptyAndDistinct
Catalog_AllIngredientsResolveInItemCatalog
Catalog_AllAmountsPositiveValidIntegers
Catalog_AllNumericRangesValid
Catalog_NoTwoInksHaveIdenticalProfiles
Catalog_NoUniversalDominance
Runtime_ArchiveDeskQueuesAndConsumesCorrectIngredientAmount
Runtime_ArchiveDeskTranscribesWithConfiguredLegibility
Runtime_SaveLoadPreservesTranscriptionQueueAndState
```


# Appendix D.10 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Culture/Plan90_78DoseInksIntegrationTests.cs`

### `Ashfall.Core.Tests/Culture/Plan90_78DoseInksIntegrationTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 127; SHA-256: `8aa74cc5c553e524d4cfda7475652564dd5ec0d9eadd524afb6f9ccea46f464e`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
DoseRegisters_HasTwelveBandsAndEightPlansWithFourNpcs
DoseRegisters_BandsAreStrictlyIncreasingAndBackwardCompatible
ArchiveInks_HasTwelveEntriesWithUniqueIds
CrossSystem_DoseAndInkCatalogsLoadIndependently
```


# Appendix E.11 — Supporting Code Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

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


# Appendix E.12 — Supporting Code Evidence: `Assets/Ashfall.Core/Journal/JournalSystem.cs`

### `Assets/Ashfall.Core/Journal/JournalSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 490 lines / 21046 bytes.
- SHA-256: `c0d8316adaed06ce3b5415dcfd5fe79f63ad31fa6675181ad85859191601d220`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=12; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class JournalSystem
public const int MaxEntries = 64;
public const int TabCount = 5;
public event Action<JournalEntry> OnEntryAdded;
public event Action<JournalEntry> OnNotificationPing;
public event Action<int> OnTabChanged;
public event Action<string> OnCodexUnlocked;
public int ActiveTab { get; private set; }
public int CodexUnlockCount { get; private set; }
public int GetLastSeenIndex(int tab) {
public int GetLastSeenCodexIndex(int tab) {
public bool HasUnreadForTab(int tab) {
public void SwitchTab(int tab) {
public void MarkTabViewed(int tab) {
public bool UnlockItemSeen(string itemId) => UnlockCodex(KnowledgeKeys.ItemSeen(itemId));
public bool UnlockLocationVisited(string locationId) => UnlockCodex(KnowledgeKeys.LocationVisited(locationId));
public bool UnlockSurvivorMet(string survivorId) => UnlockCodex(KnowledgeKeys.SurvivorMet(survivorId));
public bool UnlockEventFired(string eventId) => UnlockCodex(KnowledgeKeys.EventFired(eventId));
public bool UnlockRoomHistorySeen(string vignetteId) => UnlockCodex(KnowledgeKeys.RoomHistorySeen(vignetteId));
public bool UnlockGlitchNoted(string glitchId) => UnlockCodex(KnowledgeKeys.GlitchNoted(glitchId));
public bool UnlockWildlifeCaught(string speciesId) => UnlockCodex(KnowledgeKeys.WildlifeSpeciesCaught(speciesId));
public bool UnlockNarrativeDiscovered(string discoveryId) => UnlockCodex(KnowledgeKeys.NarrativeDiscovered(discoveryId));
public bool UnlockBureaucraticDocument(string docId) => UnlockCodex(KnowledgeKeys.BureaucraticDocument(docId));
public bool AddKnowledgeEvidence(string survivorId, string knowledgeKey) => UnlockCodex(knowledgeKey);
public bool IsItemSeen(string itemId) => _knowledge.Has(KnowledgeKeys.ItemSeen(itemId));
public bool IsLocationVisited(string locationId) => _knowledge.Has(KnowledgeKeys.LocationVisited(locationId));
public bool IsSurvivorMet(string survivorId) => _knowledge.Has(KnowledgeKeys.SurvivorMet(survivorId));
public bool IsEventFired(string eventId) => _knowledge.Has(KnowledgeKeys.EventFired(eventId));
public bool IsRoomHistorySeen(string vignetteId) => _knowledge.Has(KnowledgeKeys.RoomHistorySeen(vignetteId));
public bool IsGlitchNoted(string glitchId) => _knowledge.Has(KnowledgeKeys.GlitchNoted(glitchId));
public bool IsWildlifeCaught(string speciesId) => _knowledge.Has(KnowledgeKeys.WildlifeSpeciesCaught(speciesId));
public bool IsNarrativeDiscovered(string discoveryId) => _knowledge.Has(KnowledgeKeys.NarrativeDiscovered(discoveryId));
public bool IsBureaucraticDocumentDiscovered(string docId) => _knowledge.Has(KnowledgeKeys.BureaucraticDocument(docId));
public void SetEntryFactory(Func<JournalEntry> factory, Action<JournalEntry> recycler) {
public KnowledgeBase Knowledge => _knowledge;
public void BindAuthoredCorpus(JournalCorpusAdapter? adapter) {
public bool HasAuthoredCorpus => _authoredCorpus != null;
public JournalEntry? TryAddAuthoredEntry( string knowledgeKey, ISurvivorAuthor? fallbackAuthor = null) {
public IReadOnlyList<JournalEntry> Entries => _entries;
public int EntryCount => _entries.Count;
public string LatestText =>
public bool HasUnread { get; set; }
public bool NotificationPing { get; private set; }
public int NotificationPingCount { get; private set; }
public bool HudIsOpen { get; set; }
public JournalEntry? TryDiscover( string knowledgeKey, ISurvivorAuthor author, int day, float hour = -1f) {
public JournalEntry? TryDiscoverKnowledge( string knowledgeKey, ISurvivorAuthor? author, int day, float hour = -1f) {
public JournalEntry? TryDiscoverRawKnowledge( string knowledgeKey, string text, ISurvivorAuthor? author, int day, float hour = -1f)
public JournalEntry? TryAddRawEntry( string knowledgeKey, string text, ISurvivorAuthor author, int day, float hour = -1f)
public void AcknowledgePing() {
public void MarkRead() {
public void Clear() {
public JournalSave CaptureState() {
public void RestoreState(JournalSave save) {
public class JournalSave
public JournalEntry[] Entries;
public KnowledgeBaseSave Knowledge;
public int NextSeq;
public bool HasUnread;
public bool NotificationPing;
public int NotificationPingCount;
public bool HudIsOpen;
public int ActiveTab;
public int[] LastSeenIndexPerTab;
public int[] LastSeenCodexPerTab;
public int CodexUnlockCount;
```


# Appendix E.13 — Supporting Code Evidence: `Assets/Ashfall.Core/Culture/CulturalArchiveVaultSystem.cs`

### `Assets/Ashfall.Core/Culture/CulturalArchiveVaultSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 674 lines / 31034 bytes.
- SHA-256: `44b077c262c9c05de45f09f97809e9067c307d3a0349bdaa8edf5fa008e94c34`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=23; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ArchiveDocumentState
public string document_id = string.Empty;
public int physical_degradation_permille;        // 0..1000
public bool is_chemically_stabilized;
public int transcription_permille;               // 0..1000
public string active_scholar_id = string.Empty;
public int microfiche_copy_count;
public bool knowledge_preserved;                 // the permanent unlock (microfiche)
public string status = "archived";               // archived | transcribing | transcribed | lost
public sealed class ArchiveProjectState
public string document_id = string.Empty;
public string kind = string.Empty;               // restoration | transcription
public string survivor_id = string.Empty;
public int started_day = -1;
public int last_progress_day = -1;
public sealed class ArchiveRecordingState
public string recording_id = string.Empty;
public string category = string.Empty;           // music_performance | oral_history | survivor_testimony | radio_archive | commemorative
public string operator_id = string.Empty;
public int recorded_day = -1;
public sealed class ArchiveSalonState
public bool active;
public string modifier_key = "salon_stress_resistance";
public int start_day = -1;
public int duration_days;
public int cooldown_until_day = -1;
public sealed class ArchiveChronicleEntry
public string chronicle_id = string.Empty;
public int campaign_day;
public string event_type = string.Empty;
public string summary_key = string.Empty;
public List<string> participants = new();
public string author_id = string.Empty;
public string volume_id = string.Empty;
public sealed class CulturalArchiveVaultSave
public int schema_version = 1;
public List<ArchiveDocumentState> documents = new();
public List<ArchiveProjectState> active_projects = new();
public List<ArchiveRecordingState> recordings = new();
public List<ArchiveChronicleEntry> chronicle_entries = new();
public ArchiveSalonState salon = new();
public int next_chronicle_ordinal;
public float degradation_remainder;              // deterministic fractional permille carry
public DocumentationState documentation = new();
public sealed class CulturalArchiveVaultSystem
public const string SystemId = "cultural_archives";
public const string InstitutionId = "institution_cultural_archive";
public const int RestorationReliefPermille = 350;
public const int LegibilityLimitPermille = 900;   // above this, pages cannot be worked
public const int LostThresholdPermille = 1000;
public const float BaseDailyDegradationPermille = 2f;
public const int SalonDefaultDurationDays = 5;
public const int SalonCooldownDays = 10;
public const float SalonMoralePerDay = 2f;
public const string CutDiscCostItemId = "acetate_blank_disc";
public DocumentationSystem Documentation { get; }
public event Action<string>? OnDocumentRestored;               // documentId
public event Action<string>? OnMicroficheCreated;              // documentId
public event Action<string>? OnTomeTranscribed;                // documentId
public event Action<string>? OnDocumentLost;                   // documentId
public event Action<string, VinylRecordDefinition>? OnArchiveRecordingCreated;
public event Action<int>? OnSalonStarted;                      // day
public event Action<int>? OnSalonEnded;                        // day
public event Action<float>? OnSalonMoraleTick;                 // morale delta, once per day while active
public event Action<ArchiveChronicleEntry>? OnChronicleEntryAdded;
public event Action? OnDocumentationChanged;
public void LoadTomeCatalog(List<CulturalArchiveTomeDefinition> tomes) {
public IReadOnlyList<ArchiveDocumentState> Documents => _state.documents.AsReadOnly();
public IReadOnlyList<ArchiveRecordingState> Recordings => _state.recordings.AsReadOnly();
public IReadOnlyList<ArchiveChronicleEntry> Chronicle => _state.chronicle_entries.AsReadOnly();
public ArchiveSalonState Salon => _state.salon;
public ArchiveDocumentState? GetDocument(string documentId) =>
public ActionResult TryRestoreDocument(string documentId) {
public ActionResult TryStartTranscription(string documentId, string scholarId) {
public ActionResult TryCreateMicroficheCopy(string documentId, string operatorId) {
public static readonly string[] LegalRecordingCategories = {
public ActionResult TryCutArchiveDisc(string recordingId, string category, string operatorId, int day) {
public static VinylRecordDefinition BuildRecordDefinition(ArchiveRecordingState recording) {
public ActionResult TryStartSalon(int day) {
public ActionResult TryRecordChronicleEntry( string eventType, int campaignDay, string summaryKey, IReadOnlyList<string>? participants, string authorId = "", string? volumeId = null) {
public void TickDay(int day) {
public ActionResult TryCreatePhotograph( string authorId, string title, string cameraUsed, IEnumerable<string>? subjects, string locationId,
public ActionResult TryCreateSketch( string authorId, string title, string subject, string medium, float artisticQuality,
public ActionResult TryCreateWrittenRecord( string authorId, string title, string recordType, string content, float writingQuality,
public ActionResult TryShareDocumentation(string documentationId, int currentDay, out float moraleBoost) {
public CulturalArchiveVaultSave CaptureState() {
public void RestoreState(CulturalArchiveVaultSave? saved) {
```


# Appendix E.14 — Supporting Code Evidence: `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs`

### `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 782 lines / 31079 bytes.
- SHA-256: `1c3897163c7cd9260c589b5d055f69aeed910b69cbe318aa629523bb5aa5f424`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: The loader is the compatibility authority. Required/optional presence, accepted shapes, migrations and diagnostics must be read here rather than inferred from JSON.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
internal sealed class ItemJsonDto
public string id { get; set; } = string.Empty;
public string displayName { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public string iconPath { get; set; } = string.Empty;
public string type { get; set; } = string.Empty;
public int stackMax { get; set; } = 1;
public float weight { get; set; }
public float radProtection { get; set; }
public float durability { get; set; }
public float degradeRate { get; set; }
public float degrade_rate { get; set; }
public bool isEquipable { get; set; }
public string equipSlot { get; set; } = string.Empty;
public float contamination { get; set; }
public float hungerRestore { get; set; }
public float thirstRestore { get; set; }
public float healthEffect { get; set; }
public float radCleanse { get; set; }
public float moraleEffect { get; set; }
public float decorLocalizedMoraleDelta { get; set; }
public bool empShielded { get; set; }
public float tradeValue { get; set; }
public int tradeTier { get; set; }
public float disassembleYieldFraction { get; set; } = 0.5f;
public List<string>? tags { get; set; }
public List<ScrapYieldDto>? scrapValue { get; set; }
public RepairRecipeDto? repairRecipe { get; set; }
public LimbRequirementDto? limbRequirements { get; set; }
public LimbRequirementDto? limb_requirements { get; set; }
public LimbProvisionDto? providesLimb { get; set; }
public LimbProvisionDto? provides_limb { get; set; }
internal sealed class LimbRequirementDto
public int hands { get; set; } = 1;
public string? gripClass { get; set; }
public string? grip_class { get; set; }
internal sealed class LimbProvisionDto
public int hands { get; set; }
public int legs { get; set; }
public int qualityPermille { get; set; } = 500;
public int quality_permille { get; set; } = 500;
public string? gripClass { get; set; }
public string? grip_class { get; set; }
internal sealed class ScrapYieldDto
public string materialId { get; set; } = string.Empty;
public int amount { get; set; } = 1;
internal sealed class RepairRecipeDto
public List<ScrapYieldDto>? costs { get; set; }
public float hours { get; set; } = 0.5f;
public bool requiresTools { get; set; } = true;
public float max_repair_condition_fraction { get; set; } = 1.0f;
internal sealed class StartingSupplyJsonDto
public string itemId { get; set; } = string.Empty;
public int amount { get; set; } = 1;
internal sealed class StartingSuppliesProfileJsonDto
public string id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public List<StartingSupplyJsonDto> supplies { get; set; } =
internal sealed class StartingSuppliesRootJsonDto
public int schema_version { get; set; } = 1;
public string default_profile_id { get; set; } = string.Empty;
public List<StartingSupplyJsonDto>? starting_supplies { get; set; }
public List<StartingSuppliesProfileJsonDto>? profiles { get; set; }
public enum StartingSuppliesLoadStatus
public sealed class StartingSuppliesLoadResult
public StartingSuppliesLoadStatus Status { get; set; } = StartingSuppliesLoadStatus.Success;
public string ErrorMessage { get; set; } = string.Empty;
public string SelectedProfileId { get; set; } = StartingSuppliesCatalog.StandardProfileId;
public int AcceptedRowCount => Supplies.Count;
public bool IsSuccess => Status == StartingSuppliesLoadStatus.Success;
public sealed class StartingSuppliesCatalogLoadResult
public StartingSuppliesCatalog Catalog { get; internal set; } =
public List<string> Errors { get; } = new List<string>();
public List<string> Warnings { get; } = new List<string>();
public bool UsedLegacyFallback { get; internal set; }
public bool IsUsable => Catalog.Profiles.Count > 0;
public static class ItemCatalogLoader
public const string PrimaryFileName = "items.json";
public const string StartingSuppliesFileName = "starting_supplies.json";
public const string ItemDescriptionsFileName = ItemDescriptionCatalogLoader.PrimaryFileName;
public static ItemCatalog LoadCatalog(string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
public static List<ItemDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
public static ItemDescriptionCatalog LoadDescriptionCatalog(string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
public static CatalogLoadResult<ItemDescriptionCatalog> LoadDescriptionCatalogWithResult(string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
public static CatalogLoadResult<ItemCatalog> LoadCatalogWithResult( string dataDir, IFileIO fileIO, IJsonSerializer serializer, ItemCatalog? targetCatalog = null) {
public static void LoadInto(ItemCatalog catalog, string dataDir, IFileIO fileIO, IJsonSerializer serializer) {
public static StartingSuppliesCatalog LoadStartingSuppliesCatalog( string dataDir, IFileIO fileIO, IJsonSerializer serializer, ItemCatalog? catalog = null) {
public static StartingSuppliesCatalogLoadResult LoadStartingSuppliesCatalogDetailed( string dataDir, IFileIO fileIO, IJsonSerializer serializer, ItemCatalog? catalog = null) {
public static StartingSuppliesLoadResult LoadStartingSuppliesDetailed( string dataDir, IFileIO fileIO, IJsonSerializer serializer, ItemCatalog? catalog = null, string? profileId = null)
internal static ItemDefinition ConvertDto(ItemJsonDto dto) {
```


# Appendix E.15 — Supporting Code Evidence: `Assets/Ashfall.Core/Journal/KnowledgeBase.cs`

### `Assets/Ashfall.Core/Journal/KnowledgeBase.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 151 lines / 6762 bytes.
- SHA-256: `fb9e6ef54b94b7411eef963c19538931786db1735b057c052c2335b4f5c662dc`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class KnowledgeKeys
public const string HighCo2 = "high_co2";
public const string HasSeenRadiation = "has_seen_radiation";
public const string HasExperiencedStorm = "has_experienced_storm";
public const string FilterFailing = "filter_failing";
public const string FreezingShelter = "freezing_shelter";
public const string ContinuityReclamationDecree = "history_continuity_reclamation_decree";
public const string HydroBaronRateCardOrigin = "history_hydro_baron_rate_card_origin";
public const string DeserterCoalitionFounding = "history_deserter_coalition_founding";
public const string ColdCountBeforeTheLab = "history_cold_count_before_the_lab";
public const string ProvisionedAdvanceKnowledge = "history_the_provisioned_advance_knowledge";
public const string CheckpointConscriptsConfession = "history_checkpoint_conscripts_confession";
public const string QuartermastersPaperwork = "history_quartermasters_paperwork";
public const string InterceptedCipher = "history_the_intercepted_cipher";
public const string LedgerNobodySigned = "history_the_ledger_nobody_signed";
public static readonly string[] All = {
public static string ItemSeen(string itemId) => "item_seen_" + itemId;
public static string LocationVisited(string locationId) => "location_visited_" + locationId;
public static string SurvivorMet(string survivorId) => "survivor_met_" + survivorId;
public static string EventFired(string eventId) => "event_fired_" + eventId;
public static string RoomHistorySeen(string vignetteId) => "room_history_seen_" + vignetteId;
public static string GlitchNoted(string glitchId) => "glitch_noted_" + glitchId;
public static string WildlifeSpeciesCaught(string speciesId) => "wildlife_species_caught_" + speciesId;
public static string NarrativeDiscovered(string discoveryId) => "narrative_disc_" + discoveryId;
public static string BureaucraticDocument(string docId) => "bureaucratic_document_" + docId;
public static string FactionIntel(string canonicalFactionId) => "faction_intel_" + canonicalFactionId;
public class KnowledgeBase
public int Count => _discovered.Count;
public bool Has(string key) {
public bool Discover(string key) {
public void Clear() => _discovered.Clear();
public IReadOnlyCollection<string> Snapshot() {
public KnowledgeBaseSave CaptureState() {
public void RestoreState(KnowledgeBaseSave save) {
public class KnowledgeBaseSave
public string[] DiscoveredKeys;
```


# Appendix G.16 — Supporting Regression Evidence: `Ashfall.Core.Tests/ArchiveDeskSystemTests.cs`

### `Ashfall.Core.Tests/ArchiveDeskSystemTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 178; SHA-256: `cf3aa6ba52dda08cf0449bbe6b587933768db52529c99a9762d31f245926ffc0`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.


# Appendix G.17 — Supporting Regression Evidence: `Ashfall.Core.Tests/JournalSystemTests.cs`

### `Ashfall.Core.Tests/JournalSystemTests.cs`

- Current test declarations: Fact=17, Theory=0, InlineData=0.
- File lines: 376; SHA-256: `1b17cc5ad807b917c4745d5552d7755ed96a30b61aca4290ad49ff55b83ae8ce`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
TryDiscover_DeduplicatesPerKnowledgeKey
TryDiscover_RejectsEmptyKey
TryAddRawEntry_RecordsFreeformText_OncePerKey
MaxEntries_EvictsOldest
CodexUnlock_RecordsAndFlags
MarkReadAndAcknowledgePing_ClearFlags
CaptureRestore_RoundTrips_EntriesKnowledgeAndFlags
Clear_ResetsEverything
RestoreState_HandlesNull
EntryLifecycle_NewestFirstOrdering_AndSequenceId
TryDiscoverKnowledge_DualContract_EntryAndCodexUnlock
AddKnowledgeEvidence_Vs_TryDiscoverKnowledge_Interaction
TryAddRawEntry_EdgeCases_NullAuthor_ClampedDay_FormattedHour
MaxEntries_WithRecyclerAndFactory_RecyclesEvictedAndClears
Tabs_Switching_Clamping_AndLastSeenTracking
RestoreState_SuppressesAllEvents
DeterministicOrdering_WithoutWallClock
```


# Appendix G.18 — Supporting Regression Evidence: `Ashfall.Core.Tests/Plan17DArchiveTests.cs`

### `Ashfall.Core.Tests/Plan17DArchiveTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 262; SHA-256: `c74ad5d4882b348deeeb0bf3c05ccc490e68f5c525e8705d0eec11c6a59bb668`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ArchiveInkCatalogLoader_LoadsInksFromJson
ArchiveInkCatalogLoader_LoadAndRegister_PopulatesSystem
ArchiveDeskSystem_LoadInkCatalog_PopulatesCatalog
ArchiveDeskSystem_QueueTranscription_ConsumesInk
ArchiveDeskSystem_QueueTranscription_FailsWithInsufficientInk
ArchiveDeskSystem_TickDay_ProgressesTranscription
ArchiveDeskSystem_TranscriptionCompletion_CreatesJournalEntry
ArchiveDeskSystem_CancelJob_RefundsInk
ArchiveDeskSystem_SaveLoad_RoundTrip_PreservesState
ArchiveDeskSystem_QueueTranscription_UnknownInk_Fails
```


# Appendix G.19 — Supporting Regression Evidence: `Ashfall.Core.Tests/Plan17CCodexTests.cs`

### `Ashfall.Core.Tests/Plan17CCodexTests.cs`

- Current test declarations: Fact=12, Theory=0, InlineData=0.
- File lines: 212; SHA-256: `922d975ab12135779a4e1058b0b119e580c28a7238bab3d6b24ccf471c0d94cf`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
JournalSystem_HasFiveTabs
JournalSystem_ActiveTabDefaultsToZero
JournalSystem_SwitchTab_ClampsToValidRange
KnowledgeBase_Discover_ReturnsTrueOnlyFirstTime
KnowledgeBase_Discover_RejectsNullOrEmpty
KnowledgeBase_Has_ReflectsDiscoverState
KnowledgeBase_Has_ReturnsFalseForNullOrEmpty
KnowledgeBase_CaptureState_KeysAreOrdinalSorted
KnowledgeBase_CaptureRestore_RoundTrips
JournalSystem_TryDiscover_CreatesEntryWithCorrectKey
JournalSystem_TryDiscover_DeduplicatesSameKey
JournalSystem_SaveLoad_RoundTrip_PreservesCodexEntries
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
| ink definitions and registration | ArchiveInkCatalogLoader | queue, ingredient consumption and transcription | ArchiveDeskSystem | Owner emits/reads a typed fact; no mirror state. |
| ink definitions and registration | ArchiveInkCatalogLoader | ingredient availability and consumption | Inventory authority | Owner emits/reads a typed fact; no mirror state. |
| ink definitions and registration | ArchiveInkCatalogLoader | catalog and conservation proof | Archive focused tests | Owner emits/reads a typed fact; no mirror state. |
| queue, ingredient consumption and transcription | ArchiveDeskSystem | ink definitions and registration | ArchiveInkCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| queue, ingredient consumption and transcription | ArchiveDeskSystem | ingredient availability and consumption | Inventory authority | Owner emits/reads a typed fact; no mirror state. |
| queue, ingredient consumption and transcription | ArchiveDeskSystem | catalog and conservation proof | Archive focused tests | Owner emits/reads a typed fact; no mirror state. |
| ingredient availability and consumption | Inventory authority | ink definitions and registration | ArchiveInkCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| ingredient availability and consumption | Inventory authority | queue, ingredient consumption and transcription | ArchiveDeskSystem | Owner emits/reads a typed fact; no mirror state. |
| ingredient availability and consumption | Inventory authority | catalog and conservation proof | Archive focused tests | Owner emits/reads a typed fact; no mirror state. |
| catalog and conservation proof | Archive focused tests | ink definitions and registration | ArchiveInkCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| catalog and conservation proof | Archive focused tests | queue, ingredient consumption and transcription | ArchiveDeskSystem | Owner emits/reads a typed fact; no mirror state. |
| catalog and conservation proof | Archive focused tests | ingredient availability and consumption | Inventory authority | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the old 3→12 count target with a 12-row conservation census and quality matrix. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Keep every required ingredient, quality dimension and trade-off visible to future content authors. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Add a reachability proof from each ink to an archive desk action and resulting document state. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Preserve save/queue behavior and avoid new parallel item or document stores. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.552 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Codex/CodexProjectionBuilder.cs`

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


# Appendix Q.553 — Additional Current Architecture Evidence: `src/Host/CulturalArchiveSaveStore.cs`

### `src/Host/CulturalArchiveSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 25 lines / 1129 bytes.
- SHA-256: `6c01a5aa5dfe64c2580c36943133c1507eed85288aec9dc7c1fae2003d25ccf9`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class CulturalArchiveSaveStore
public const string FileName = "cultural_archives_save.json";
public const string SectionName = "cultural_archives";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static bool TrySave(CulturalArchiveVaultSave state) => s_store.TrySave(state);
public static CulturalArchiveVaultSave? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(CulturalArchiveVaultSave state) => s_store.CapturePersisted(state);
```


# Appendix Q.554 — Additional Current Architecture Evidence: `src/Journal/JournalBookUI.cs`

### `src/Journal/JournalBookUI.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 627 lines / 26158 bytes.
- SHA-256: `6529212c56a3b9c7b7f2d9204fd07c0746e29d5dd85d2aaf1065e3eadc6ce497`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=8; textual Godot mentions=3; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class JournalBookUI : Control
public const int MaxVisibleCollapsed = 4;
public const int MaxVisibleOpen = JournalSystem.MaxEntries;
public bool IsOpen { get; private set; }
public bool HasUnread { get; private set; }
public bool NotificationPing { get; private set; }
public int NotificationPingCount { get; private set; }
public int EntryCount { get; private set; }
public string LatestText { get; private set; } = string.Empty;
public string LatestAuthor { get; private set; } = string.Empty;
public string LatestTimestamp { get; private set; } = string.Empty;
public string StatusLine { get; private set; } = "JOURNAL: —";
public string DetailSummary { get; private set; } = "No entries yet.";
public int ActiveTab { get; private set; }
public IReadOnlyList<JournalEntry> Entries => _entries;
public event Action? OnOpened;
public event Action? OnClosed;
public event Action<JournalEntry>? OnEntryPushed;
public event Action<int>? OnTabChanged;
public void Bind( JournalSystem journal, Func<JournalTab, IReadOnlyList<JournalCodexRow>> codexProvider, Func<int, bool>? unreadProvider = null, Func<int>? dayProvider = null) {
public override void _Ready() {
public override void _ExitTree() {
public void SwitchTab(int tab) {
public void Push(JournalEntry entry) {
public void SetEntries(IReadOnlyList<JournalEntry> entries) {
public void ApplyUiState(bool isOpen, bool hasUnread, bool notificationPing = false, int activeTab = 0) {
public void Clear() {
public void Open() {
public void Close() {
public void Toggle() {
public void MarkRead() {
public void AcknowledgePing() {
public string ActiveTabContent => _content != null ? _content.Text : string.Empty;
public void Refresh() {
```


# Appendix Q.555 — Additional Current Architecture Evidence: `src/Main.ShelterBatch3.cs`

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


# Appendix Q.556 — Additional Current Architecture Evidence: `src/Host/ArchiveDeskHostSession.cs`

### `src/Host/ArchiveDeskHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 139 lines / 5384 bytes.
- SHA-256: `61d9683b119b70656d4d7b3f42081d9da338b002b8c96426849615331727280e`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ArchiveDeskHostSession
public ArchiveDeskSystem System { get; }
public string LastEvent { get; private set; } = string.Empty;
public void LoadInkCatalog(List<InkMaterialDefinition> inks) {
public void LoadInkCatalog(string dataDir) {
public ActionResult QueueTranscription(string evidenceId, string archivistId, string inkId) {
public ActionResult CancelJob(string jobId) {
public void TickDay(int day) {
public override void Save() {
public static class ArchiveDeskSaveStore
public const string FileName = "archive_desk_save.json";
public const string SectionName = "archive_desk";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static bool TrySave(ArchiveDeskState state) => s_store.TrySave(state);
public static ArchiveDeskState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(ArchiveDeskState state) => s_store.CapturePersisted(state);
public static string TryCaptureDirect(ArchiveDeskState state) => s_store.CaptureBare(state);
public static ArchiveDeskState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(ArchiveDeskState state) => s_store.CaptureBare(state);
public static ArchiveDeskState? TryRestore(string json) => s_store.RestoreBare(json);
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


# Appendix Q.558 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Narrative/BureaucraticDocumentCatalog.cs`

### `Assets/Ashfall.Core/Narrative/BureaucraticDocumentCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 530 lines / 21274 bytes.
- SHA-256: `edfd58f1440ff67778b8396e389bbc8038143dd0c9e4eb833a7e269dcd6b8bb2`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum BureaucraticDocumentTruthClass
public sealed class BureaucraticDocumentDefinition
public string DocId { get; }
public string DocType { get; }
public string Title { get; }
public int PostedDay { get; }
public string PostedBy { get; }
public string Location { get; }
public string Material { get; }
public string Transcript { get; }
public IReadOnlyList<string> Tags { get; }
public BureaucraticDocumentTruthClass TruthClass { get; }
public string LocationId { get; }
public IReadOnlyList<string> ProducerIds { get; }
public IReadOnlyList<string> RelatedDocumentIds { get; }
public string KnowledgeKey => BureaucraticDocumentDiscoverySystem.KnowledgeKey(DocId);
public sealed class BureaucraticDocumentCatalog
public IReadOnlyList<BureaucraticDocumentDefinition> Documents => _readOnlyDocuments;
public int Count => _documents.Count;
public bool TryGet(string docId, out BureaucraticDocumentDefinition document) {
public sealed class BureaucraticDocumentCatalogLoadResult
public int SchemaVersion { get; internal set; }
public BureaucraticDocumentCatalog Catalog { get; internal set; } =
public List<string> Warnings { get; } = new List<string>();
public List<string> Errors { get; } = new List<string>();
public bool IsSuccess => Errors.Count == 0;
public sealed class BureaucraticDocumentCatalogLoader
public const string DocumentsFileName = "narrative/bureaucratic_documents_expansion.json";
public const string RuntimeMapFileName = "narrative/bureaucratic_document_runtime_map.json";
public BureaucraticDocumentCatalogLoadResult Load(string dataDirectory) {
public int schema_version;
public List<RawDocument?>? documents;
public string? doc_id;
public string? doc_type;
public string? title;
public int posted_day;
public string? posted_by;
public string? location;
public string? material;
public string? transcript;
public string[]? tags;
public int schema_version;
public List<RawMapping?>? documents;
public string? doc_id;
public string? truth_class;
public string? location_id;
public string[]? producer_ids;
public string[]? related_doc_ids;
public static RawMapping Unresolved(string id) => new RawMapping
public enum BureaucraticDocumentDiscoveryStatus
public sealed class BureaucraticDocumentDiscoveryResult
public string DocId { get; }
public string ProducerId { get; }
public BureaucraticDocumentDiscoveryStatus Status { get; }
public bool Changed => Status == BureaucraticDocumentDiscoveryStatus.Discovered;
public sealed class BureaucraticDocumentDiscoverySystem
public const string KnowledgePrefix = "bureaucratic_document_";
public BureaucraticDocumentCatalog Catalog => _catalog;
public static string KnowledgeKey(string docId) => KnowledgePrefix + (docId ?? string.Empty);
public bool IsDiscovered(JournalSystem journal, string docId) {
public BureaucraticDocumentDiscoveryResult Discover( string docId, string producerId, int currentDay, JournalSystem journal) {
public IReadOnlyList<BureaucraticDocumentDiscoveryResult> DiscoverByProducer( string producerId, int currentDay, JournalSystem journal) {
```


# Appendix Q.559 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/LibraryStudySystem.cs`

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


# Appendix Q.560 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Narrative/NarrativeDiscoveryCatalog.cs`

### `Assets/Ashfall.Core/Narrative/NarrativeDiscoveryCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1551 lines / 83735 bytes.
- SHA-256: `b745f1e9265ba36bb0169d2fddadc1274d5f551f0572b5992e5d94c371e35240`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class NarrativeDiscoveryManifestEntry
public string discovery_id = string.Empty;
public string source_catalog = string.Empty;
public string source_record_id = string.Empty;
public string channel = string.Empty;
public string producer_id = string.Empty;
public string[] producer_ids = Array.Empty<string>();
public int min_day = 1;
public int weight = 1;
public bool one_time = true;
public string truth_class = string.Empty;
public string provenance_label = string.Empty;
public string identity_status = string.Empty;
public string numeric_claim_label = string.Empty;
public string[] related_discovery_ids = Array.Empty<string>();
public string DiscoveryId => discovery_id;
public string SourceCatalog => source_catalog;
public string SourceRecordId => source_record_id;
public string Channel => channel;
public string ProducerId => producer_id;
public IReadOnlyList<string> ProducerIds => producer_ids;
public int MinDay => min_day;
public int Weight => weight;
public bool OneTime => one_time;
public string TruthClass => truth_class;
public string ProvenanceLabel => provenance_label;
public string IdentityStatus => identity_status;
public string NumericClaimLabel => numeric_claim_label;
public IReadOnlyList<string> RelatedDiscoveryIds => related_discovery_ids;
public sealed class NarrativeDiscoveryManifestFile
public int schema_version = 1;
public List<NarrativeDiscoveryManifestEntry> entries = new List<NarrativeDiscoveryManifestEntry>();
public sealed class NarrativeDiscoveredRecord
public string DiscoveryId { get; set; } = string.Empty;
public string KnowledgeKey { get; set; } = string.Empty;
public string SourceCatalog { get; set; } = string.Empty;
public string SourceRecordId { get; set; } = string.Empty;
public string Channel { get; set; } = string.Empty;
public string ProducerId { get; set; } = string.Empty;
public string[] ProducerIds { get; set; } = Array.Empty<string>();
public int MinDay { get; set; } = 1;
public string Title { get; set; } = string.Empty;
public string Subtitle { get; set; } = string.Empty;
public string BodyText { get; set; } = string.Empty;
public string Category { get; set; } = string.Empty;
public string[] Tags { get; set; } = Array.Empty<string>();
public string TruthClass { get; set; } = string.Empty;
public string ProvenanceLabel { get; set; } = string.Empty;
public string IdentityStatus { get; set; } = string.Empty;
public string NumericClaimLabel { get; set; } = string.Empty;
public string RecordFamily { get; set; } = string.Empty;
public string FacilityOrStationLabel { get; set; } = string.Empty;
public string TechnicalSummary { get; set; } = string.Empty;
public string[] RelatedDiscoveryIds { get; set; } = Array.Empty<string>();
public interface INarrativeSourceAdapter
public static class NarrativeJsonHelpers
public static string GetStringProp(JsonElement elem, string propName, string fallback = "") {
public static string[] GetStringArrayProp(JsonElement elem, string propName) {
public static int GetIntProp(JsonElement elem, string propName, int fallback = 0) {
public static float GetFloatProp(JsonElement elem, string propName, float fallback = 0f) {
public sealed class ProcessLogSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) => HandledCatalogs.Contains(sourceCatalog);
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public sealed class BunkerGlitchSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) =>
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public sealed class BunkerBlueprintSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) =>
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public sealed class BunkerCourtSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) =>
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public sealed class WireConfessionSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) =>
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public sealed class TradeLedgerSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) =>
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public sealed class RegionalTreatySourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) =>
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public sealed class SurgeonsCasebookSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) =>
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public sealed class DeadHandDirectiveSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) =>
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public sealed class CourierDispatchSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) =>
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public static class PersonalLetterRuntimeContract
public const string LettersExpansionCatalog = "narrative/letters_expansion.json";
public const string UnsentLettersBatch2Catalog = "narrative/unsent_letters_batch_2.json";
public static readonly string[] SourceCatalogs = {
public static bool IsSourceCatalog(string sourceCatalog) {
public sealed class PersonalLetterSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) =>
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public static class AbyssalAnomaliesRuntimeContract
public const string HydrophoneCatalog = "narrative/hydrophone_acoustic_logs.json";
public const string GeothermalCatalog = "narrative/geothermal_borehole_logs.json";
public const string CryopodCatalog = "narrative/cryopod_failure_logs.json";
public const string SaltMineCatalog = "narrative/salt_mine_inscriptions.json";
public static readonly string[] SourceCatalogs = {
public static bool IsSourceCatalog(string sourceCatalog) {
public static bool IsActivatedSourceRecord(string sourceRecordId) => AbyssalAnomaliesProjection.IsActivated(sourceRecordId);
public sealed class AbyssalAnomaliesSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) =>
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public static class FringeCultRuntimeContract
public const string CobaltCatalog = "narrative/cobalt_liturgies.json";
public const string IronCatalog = "narrative/iron_synod_canons.json";
public const string HymnalCatalog = "narrative/geophone_hymnals.json";
public const string EpitaphCatalog = "narrative/wasteland_grave_epitaphs.json";
public static readonly string[] SourceCatalogs = {
public static bool IsSourceCatalog(string sourceCatalog) {
public static string DefaultTruthClass(string sourceCatalog) {
public static bool IsValidTruthClass(string truthClass) {
public sealed class FringeCultSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) => FringeCultRuntimeContract.IsSourceCatalog(sourceCatalog);
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public static class PaperPrintRuntimeContract
public const string HollanderCatalog = "narrative/hollander_beater_pulping_logs.json";
public const string DeckleCatalog = "narrative/deckle_mould_watermark_audits.json";
public const string PressCatalog = "narrative/screw_press_felt_reports.json";
public const string SizingCatalog = "narrative/tub_sizing_gelatin_assays.json";
public const string RagPulpCatalog = "narrative/rag_pulp_beater_records.json";
public const string InkCatalog = "narrative/iron_gall_ink_acidity_reports.json";
public const string TypeCatalog = "narrative/typographic_lead_wear_logs.json";
public const string StencilCatalog = "narrative/stencil_propaganda_smear_logs.json";
public static readonly string[] SourceCatalogs = {
public static bool IsSourceCatalog(string sourceCatalog) {
public static bool IsPaperMakingCatalog(string sourceCatalog) =>
public static string GetFamily(string sourceCatalog) {
public sealed class PaperPrintSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) => PaperPrintRuntimeContract.IsSourceCatalog(sourceCatalog);
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public static class BoneHornRuntimeContract
public const string DegreasingCatalog = "narrative/bone_degreasing_prep_logs.json";
public const string SawingCatalog = "narrative/antler_horn_sawing_records.json";
public const string PolishingCatalog = "narrative/scraping_polishing_reports.json";
public const string ToolAssayCatalog = "narrative/needle_awl_hook_assays.json";
public static readonly string[] SourceCatalogs = {
public static bool IsSourceCatalog(string sourceCatalog) {
public static string GetFamily(string sourceCatalog) {
public sealed class BoneHornSourceAdapter : INarrativeSourceAdapter
public bool CanAdapt(string sourceCatalog) => BoneHornRuntimeContract.IsSourceCatalog(sourceCatalog);
public NarrativeDiscoveredRecord Adapt(NarrativeDiscoveryManifestEntry entry, JsonElement sourceRecord) {
public sealed class NarrativeDiscoveryCatalog
public IReadOnlyList<NarrativeDiscoveredRecord> AllRecords => _records;
public int Count => _records.Count;
public void RegisterAdapter(INarrativeSourceAdapter adapter) {
public void Clear() {
public void LoadFromFiles(string dataDirectory, IFileIO files) {
public void Load(string manifestJson, string dataDirectory, IFileIO files) {
public bool TryGetRecord(string discoveryId, out NarrativeDiscoveredRecord? record) {
public IReadOnlyList<NarrativeDiscoveredRecord> GetByProducer(string producerId) {
public IReadOnlyList<NarrativeDiscoveredRecord> GetByChannel(string channel) {
public bool TryDiscover(string discoveryId, JournalSystem journal, out NarrativeDiscoveredRecord? record) {
```


# Appendix Q.561 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs`

### `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 475 lines / 21068 bytes.
- SHA-256: `a973da78df420959de81f7a109374059b3fbc4f84752836dc06f34b0aa8103ce`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=4; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum SurvivorDeathCause
public sealed class SurvivorFateEvent
public string survivorId = string.Empty;
public SurvivorDeathCause cause = SurvivorDeathCause.Unknown;
public string causeDetail = string.Empty;  // e.g. disease id, encounter id
public int day;
public string source = string.Empty;       // reporting system id, for audit
public bool isPlayerAvatar;                // distinguishes avatar death from roster death
public SurvivorFateEvent Clone() => new SurvivorFateEvent
public sealed class SurvivorFateSaveState
public string systemId = SurvivorFateSystem.SystemId;
public List<SurvivorFateEvent> fates = new List<SurvivorFateEvent>();
public sealed class SurvivorFateSave
public const int CurrentSaveVersion = 1;
public int saveVersion = CurrentSaveVersion;
public int simDay;
public SurvivorFateSaveState State = new SurvivorFateSaveState();
public string Checksum = string.Empty;
public sealed class SurvivorFateSystem
public const string SystemId = "survivor_fate_system";
public const string CounterDeathsTotal = "deaths_total";
public const string FlagSurvivorDiedPrefix = "flag_survivor_died_";
public const string JournalKeyPrefix = "survivor_death_";
public const float GriefMoraleDelta = -8f;
public const string GriefModifierSource = "grief.shelter_loss";
public event Action<SurvivorFateEvent> OnSurvivorFate;
public event Action<SurvivorFateEvent> OnLastSurvivorDied;
public IReadOnlyList<SurvivorFateEvent> Fates => _state.fates;
public bool HasFate(string survivorId) =>
public SurvivorFateEvent FindFate(string survivorId) =>
public int DeathCount => _state.fates.Count;
public SurvivorFateEvent ReportDeath(SurvivorFateEvent fate) {
public SurvivorFateEvent ReportDeath( string survivorId, SurvivorDeathCause cause, string causeDetail = "", string source = "", bool isPlayerAvatar = false,
public int ReconcileFromRoster() {
public void DrainDayEvents(List<DayStateChangeEvent> target) {
public int PendingDayEventCount => _pendingDayEvents.Count;
public SurvivorFateSaveState CaptureState() {
public void RestoreState(SurvivorFateSaveState saved) {
public static string DescribeCause(SurvivorFateEvent fate) {
```


# Appendix Q.562 — Additional Current Architecture Evidence: `src/Journal/JournalSelfTest.cs`

### `src/Journal/JournalSelfTest.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 228 lines / 12076 bytes.
- SHA-256: `ce4c5c32c80278be6bd03f50dcd52d1d87b49478fa2f8d4463c45c1f7dbc3c70`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class JournalSelfTest
public static int Run(JournalCatalogs catalogs) {
```


# Appendix Q.563 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/DeepCoastHeadlessDemo.cs`

### `Assets/Ashfall.Core/DeepCoastHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 379 lines / 21629 bytes.
- SHA-256: `aa6e8be3cbea443b4290465ced3815d6aee2faa57780c9c951e878a4aed50d6a`.
- Architecture signals: seeded references=7; save/restore symbols=6; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class DeepCoastHeadlessDemo
public const int DefaultSeed = 4048;
public static HeadlessReport Run(string? dataDirectory = null, ILog? log = null) {
```


# Appendix Q.564 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Performance/Workloads/PerformanceCampaignHarness.cs`

### `Assets/Ashfall.Core/Performance/Workloads/PerformanceCampaignHarness.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 403 lines / 14637 bytes.
- SHA-256: `364d311490e3a73f9e9d2d31861f78f70ecb80a4615f9b48ade364cb2330e7a8`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class PerformanceCampaignHarness : IDisposable
public CampaignDayCoordinator Coordinator { get; }
public SurvivorRosterSystem Survivors { get; }
public Inventory.Inventory Inventory { get; }
public JournalSystem Journal { get; }
public WeatherSystem Weather { get; }
public ExpeditionSystem Expeditions { get; }
public LocationEvolutionSystem LocationEvolution { get; }
public WildlifeMigrationSystem Wildlife { get; }
public LandmarkDegradationSystem Landmark { get; }
public int CurrentDay => Coordinator.Calendar is Ashfall.Core.Clock.ISimClock simClock ? simClock.DayIndex : Coordinator.LastAdvancedDay;
public ISeededRng Rng { get; }
public double AdvanceDays(int days) {
public string CaptureSavePayload() {
public double MeasureSaveLatency() {
public double MeasureLoadLatency(string payload) {
public static double MeasureChecksumLatency(string payload) {
public long MeasureRetainedMemoryAfterNewGame() {
public void Dispose() {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public string Id => "perf_author";
public string DisplayName => "Perf";
public Ashfall.Core.Journal.RiskBiasTrait RiskBias => Ashfall.Core.Journal.RiskBiasTrait.Realist;
```


# Appendix Q.565 — Additional Current Architecture Evidence: `src/Main.FlagshipInstitutions.cs`

### `src/Main.FlagshipInstitutions.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 579 lines / 26298 bytes.
- SHA-256: `583894a805a235c894e1dfb161df6b1834511001e727b4f0480a25d2162cd433`.
- Architecture signals: seeded references=1; save/restore symbols=8; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public string RecordArchiveChronicle( string eventType, string summaryKey, IReadOnlyList<string>? participants = null, string authorId = "") {
internal sealed class HostFactionStandingPort : IFactionStandingPort
public float GetStanding(string factionId) =>
public void AdjustStanding(string factionId, float delta, string reasonCode) =>
internal sealed class HostSurvivorSkillsPort : ISurvivorSkillsPort
public bool HasSkill(string survivorId, string skillId) =>
internal sealed class HostSurvivorConditionPort : ISurvivorConditionPort
public bool HasCondition(string survivorId, string conditionId) {
public int GetAcuteStressPermille(string survivorId) {
public void ApplyAcuteStressReduction(string survivorId, int permille) {
public void ApplyRecoveryProgress(string survivorId, int progress) {
public void SuppressReversibleCondition(string survivorId, string conditionId) {
public int GetRelationshipTrust(string therapistId, string patientId) => 50;
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
```


# Appendix Q.566 — Additional Current Architecture Evidence: `src/Host/DeepCoastHostSession.cs`

### `src/Host/DeepCoastHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 429 lines / 22369 bytes.
- SHA-256: `9bc0543abcb1ccb531d92489e906c66c950c69da29668beb6244f6b5ba601499`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DeepCoastHostSession
public const int DemoSeed = 4048;
public District8DeepCoastSystem DeepCoast { get; }
public JournalSystem Journal { get; }
public FactionStanceEngine Stances { get; }
public HoldfastTradeInventory Inventory { get; }
public MaritimeHostSession Maritime { get; }
public string LastEvent { get; private set; } = string.Empty;
public static DeepCoastHostSession Create( District8DeepCoastSystem deepCoast = null!, JournalSystem journal = null!, FactionStanceEngine stances = null!, HoldfastTradeInventory inventory = null!, MaritimeHostSession maritime = null!,
public string Survey(int day) {
public string Decide(string decisionId, int day) {
public string ClearPerimeter(int day) {
public string ClearChannel(int day) {
public CommandResult RepairBerth(int day) {
public string StartDockDive(string diverId, string operatorId, int day) {
public string TickDockDive(float seconds) {
public string CrankDockDive() {
public string AdvanceDockDive(int noise) {
public string CompleteDockDive(bool success, List<SalvageEntry> rewards = null!, int day = 1) {
public bool IsRouteNodeBlocked(string nodeId) {
public bool DockExpeditionAvailable => DeepCoast.IsNodeAccessible(District8DeepCoastSystem.DockId);
public bool IsFleetActive => DeepCoast.IsFleetStoodUp;
public void TickDaily(int day, WeatherKind weather = WeatherKind.Clear) {
public string StatusLine() {
public District8DeepCoastState CaptureDeepCoast() => DeepCoast.CaptureState();
public void RestoreDeepCoast(District8DeepCoastState state) => DeepCoast.RestoreState(state);
public void SetCurrentDay(int day) => _lastDay = day > 0 ? day : 1;
```


# Appendix Q.567 — Additional Current Architecture Evidence: `src/Main.ExpandedShelterSystems.cs`

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


# Appendix Q.568 — Additional Current Architecture Evidence: `src/Foundry/SilentFoundryHostSession.cs`

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


# Appendix Q.569 — Additional Current Architecture Evidence: `src/Host/CodexHostSession.cs`

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


# Appendix Q.570 — Additional Current Architecture Evidence: `src/Host/DutyRosterHostSession.cs`

### `src/Host/DutyRosterHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 440 lines / 19490 bytes.
- SHA-256: `3beb81558d909f0dc287c797f1b8f5c188a99ecfdc4c833f470aabc19056f658`.
- Architecture signals: seeded references=0; save/restore symbols=3; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DutyRosterHostSession
public const int DefaultSeed = 908; // roster seed offset: _worldSeed + 1208 style
public DutyRosterSystem Roster { get; }
public MoraleMarkSystem Marks { get; }
public ShelterEncounterSystem Encounters { get; }
public DutyRosterQuestRuntime Quests { get; }
public DutyRosterCatalog Catalog { get; }
public SimClock Clock { get; }
public string LastEvent { get; private set; } = string.Empty;
public int LocationCount => Catalog.Locations.Count;
public int QuestCount => Catalog.Quests.Count;
public int MarkCount => Catalog.Marks.Count;
public int SeasonCount => Catalog.Seasons.Count;
public RoleFitnessVerdict? PreviewRoleFitness(string survivorId, string roleId) => Roster.PreviewRoleFitness(survivorId, roleId);
public DutyHourSnapshot? PreviewDutyHours(string survivorId) => Roster.PreviewDutyHours(survivorId);
public static DutyRosterHostSession Create(string dataDirectory, ILog? log = null, Ashfall.Core.Journal.JournalSystem journal = null!) {
public void Unlock(int day) {
public DutyRosterSave CaptureSave() =>
public void RestoreSave(DutyRosterSave save) =>
public bool SaveState() {
public string StartRosterQuest(string questId) {
public string AdvanceRosterQuest(string questId) {
public string ResolveRosterChoice(string questId, string choiceId) {
public string ActiveQuestProse(string questId) {
public string QuestsLine() {
public string TickDay() {
public void SyncDay(int day) {
public void DrainDayEvents(List<DayStateChangeEvent> target) {
public string TickDay(IReadOnlyList<DutyRosterOccupant> occupants) {
public void SyncHoldfastToDuty( CensusClaimSystem census, IceRoadSystem iceRoad, WaystationSystem waystation, BrineWaterSystem brine, int day)
public DutyRosterHoldfastSnapshot SnapshotForHoldfast() {
public string InspectWall() {
public string ResolveChart(string choiceId) {
public string ResolveInk() {
public string BurnChart() {
public string QueueVisitor(string visitorId) {
public string StartEncounter(string kind) {
public string ActivateSecondWinter() {
public string GrantOverflowAccess() {
public string RegisterOverflowVisit(string nodeId) {
public string BridgeHatchReturn(string survivorId = null!, bool crisis = false) {
public string GrantBlankRowsAccess() {
public string WallLine() {
public string EncountersLine() {
public string MarksLine() {
public string CatalogLine() {
public CommandResult AssignDuty(string role, string survivorId, bool confirmFitnessWarning = false) {
```


# Appendix Q.571 — Additional Current Architecture Evidence: `src/UI/JournalDetailPanel.cs`

### `src/UI/JournalDetailPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 134 lines / 5031 bytes.
- SHA-256: `707f3dfef12868787d8b04ab19d0fc2fe85ac37d79068e908a39d2439c7ba395`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=3; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class JournalDetailPanel : Control
public event Action? OnClose;
public bool IsBound => _journal != null;
public int RenderedRowCount { get; private set; }
public void Bind(JournalSystem? journal) {
public void RefreshView() {
public override void _Ready() {
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
```


# Appendix Q.572 — Additional Current Architecture Evidence: `src/UI/JournalPanel.cs`

### `src/UI/JournalPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 486 lines / 18930 bytes.
- SHA-256: `412d1eb12aed0d996c58bb0b653d3b90517d40855ec738f9b5cfb4c139f5bf26`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=3; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class JournalPanel : Control
public event Action? OnClose;
public void Bind(JournalHostSession session) => Bind(session?.System!);
public void Bind(JournalSystem journal) {
public void RefreshView() {
public override void _Ready() {
public void Open() {
public void Close() {
public void Unbind() {
public override void _UnhandledInput(InputEvent @event) {
public override void _ExitTree() {
```


# Appendix Q.573 — Additional Current Architecture Evidence: `src/Host/JournalHostSession.cs`

### `src/Host/JournalHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 63 lines / 1760 bytes.
- SHA-256: `5df4457cf7b981872e2a48f8ca6698cdb0f43202d3a69e8047340d70d151263a`.
- Architecture signals: seeded references=0; save/restore symbols=3; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class JournalHostSession : HostSessionBase
public JournalSystem System { get; }
public string LastEvent { get; private set; } = string.Empty;
public override void Save() {
public void RestoreSave(JournalSave state) {
```


# Appendix Q.574 — Additional Current Architecture Evidence: `src/Host/ExpeditionHostSession.cs`

### `src/Host/ExpeditionHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1663 lines / 85060 bytes.
- SHA-256: `46c1ce80f030a53183aea292df6246ab3278b2c2d481baf91e91cbf1f1c8af37`.
- Architecture signals: seeded references=6; save/restore symbols=13; typed event declarations=11; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExpeditionHostSession : HostSessionBase
public const int DemoSeed = 7071;
public const int VehicleSeed = 7072;
public const float KmPerTravelTick = 2.5f;
public const string StarterVehicleId = "vehicle_utility_quad";
public ExpeditionSystem Engine { get; }
public List<ExpeditionDefinition> Definitions { get; }
public List<ExpeditionDefinition> DemoDefinitions => Definitions;
public DiveInstanceRunner DiveRunner { get; private set; }
public Ashfall.Core.Flags.IFlagLedger Flags { get; set; } = new Ashfall.Core.Flags.CampaignConsequenceLedger();
public DiscoveryConsequenceSystem DiscoveryConsequences { get; }
public event Action<ConsequenceOutcome>? OnDiscoveryConsequenceApplied;
public Action<string, string, int>? ApplyDisease { get; set; }
public JournalSystem? Journal { get; set; }
public Ashfall.Core.Inventory.Inventory? ShelterInventory { get; set; }
public ItemCatalog? Items { get; set; }
public ExpeditionVehicleSystem Vehicles { get; }
public VehicleGarageSystem? Garage { get; set; }
public VouchAccessSystem CrossingGate { get; set; }
public WastelandMapSystem? WastelandMap { get; set; }
public Func<string, bool> ExtraBlocked { get; set; }
public Func<string, FitnessVerdict?>? SurvivorFitnessProvider { get; set; }
public Func<string, RoleFitnessVerdict?>? ExpeditionFitnessProvider { get; set; }
public Func<string, float>? SurvivorMovementSpeedProvider { get; set; }
public Func<string, float>? PackCapacityProvider { get; set; }
public Func<string, WeatherGateBlock?> ExtraGateBlock { get; set; }
public string? GetBlockReason(string locationId) {
public WeatherGateBlock? GetWeatherGateBlock(string locationId) => ExtraGateBlock?.Invoke(locationId);
public void SetEncounterChanceMultiplier(Func<string, float> multiplier) => Engine.SetEncounterChanceMultiplier(multiplier);
public void SetEstimateWeatherInputs(Func<string, ExpeditionWeatherInputs?>? provider) {
public void SetEstimateProtectiveInputs(Func<string, ExpeditionProtectiveInputs?>? provider) {
public void SetEstimateRouteModifiers(Func<string, float>? hazard, Func<string, float>? travel) {
public void AttachDamagedMapFeedback(DamagedMapSystem? damagedMap) {
public string LastEvent { get; private set; } = string.Empty;
public event Action<ExpeditionEncounterBridge.EncounterSurfaced>? OnEncounterSurfaced;
public sealed class TravelCombatTrigger
public string EncounterId = string.Empty;
public string Title = string.Empty;
public string LocationId = string.Empty;
public int DangerLevel;
public IReadOnlyList<string> CombatantIds = Array.Empty<string>();
public event Action<TravelCombatTrigger>? OnTravelEncounterCombatTriggered;
public event Action<string, string, WeatherGateBlock>? OnWeatherGateForced;
public static bool UseEncounterModal { get; set; } = true;
public ExpeditionEncounterBridge Bridge => _bridge;
public Dictionary<string, float> WaterRouteHazards { get; } = new(StringComparer.Ordinal);
public IReadOnlyList<PendingSurfacedEncounter> Pending =>
public EncounterDefinition? FindEncounter(string encounterId) => _narrative?.Find(encounterId);
public void ClearAllPending() => _narrative?.ClearAllPending();
public NarrativeEncounterSystem? NarrativeEngine => _narrative;
public static ExpeditionHostSession Create(string dataDir, NarrativeEncounterSystem narrative = null!, TravelEncounterSystem travel = null!, ICampaignRngManager? campaignRng = null) {
public bool IsLocationBlocked(string locationId) => GetBlockReason(locationId) != null;
public CommandResult StartExpedition( string survivorId, string locationId, ExpeditionStance stance = ExpeditionStance.Stealth, int staminaBudget = 40, string vehicleId = "",
public CommandResult RefuelVehicle(string vehicleId, float units) {
public CommandResult RepairVehicle(string vehicleId, float amount) {
public CommandResult InstallTrackGear(string vehicleId, string gearId, float condition = 100f) {
public CommandResult RemoveTrackGear(string vehicleId) {
public CommandResult RepairTrackGear(string vehicleId, float amount) {
public CommandResult AssembleVehicleFromKit(string kitItemId, Inventory shelterInventory) {
public Action<VehicleBreakdownOutcome>? BreakdownConsequenceSink { get; set; }
public CommandResult StartDemoExpedition(string survivorId, string locationId) => StartExpedition(survivorId, locationId);
public CommandResult DispatchSortie( string survivorId, string locationId, ExpeditionStance stance, int day, string vehicleId = "",
public FitnessVerdict? GetSurvivorFitness(string survivorId) => SurvivorFitnessProvider?.Invoke(survivorId);
public RoleFitnessVerdict? GetExpeditionFitness(string survivorId) => ExpeditionFitnessProvider?.Invoke(survivorId);
public string TickHours(float hours) {
public sealed class EncounterApplicationResult
public string ResolutionId = string.Empty;
public enum Status { NotApplicable, Applied, AlreadyKnown, RejectedCapacity, RejectedInsufficientItems, NoActiveExpedition, SkippedNoAuthority, RejectedUnknownId } public Status Item = Status.NotApplicable; public string ItemId = string.Empty; public int ItemQuantity; public Status Journal = Status.NotApplicable; public string JournalId = string.Empty; public Status Location = Status.NotApplicable; public string LocationId = string.Empty; public Status Flag = Status.NotApplicable; public string FlagId = string.Empty; /// <summary>F17 — micro-location hazard routing outcome. NotApplicable /// for flags without a registered hazard; Applied when the canonical /// disease authority received the consequence exactly once.</summary> public MicroLocationHazardRegistry.HazardStatus Hazard = MicroLocationHazardRegistry.HazardStatus.NotApplicable; public string HazardDiseaseId = string.Empty; }
public EncounterApplicationResult? LastApplication { get; private set; }
public event Action<EncounterApplicationResult>? OnEncounterConsequencesApplied;
public bool EncounterApplyChoice(string encounterId, string choiceId, int day) => EncounterApplyChoice(encounterId, choiceId, day, null!);
public bool EncounterApplyChoice(string encounterId, string choiceId, int day, string locationId) {
public bool ResolveTravelChoiceWithCombat( string encounterId, string choiceId, int day, string locationId, int dangerLevel, int enemyCount) {
public static readonly ExpeditionJournalAuthor Instance = new ExpeditionJournalAuthor();
public string Id => "expedition";
public string DisplayName => "Expedition";
public RiskBiasTrait RiskBias => RiskBiasTrait.Realist;
public string PushLuck(string survivorId) {
public string PushLuckDemo(string survivorId) => PushLuck(survivorId);
public string Retreat(string survivorId) {
public string RetreatDemo(string survivorId) => Retreat(survivorId);
public string EnterCamp( string survivorId, float temperatureC = -10f, string weatherCondition = "Clear", float firewood = 8f, float water = 4f,
public string EnterCampDemo( string survivorId, float temperatureC = -10f, string weatherCondition = "Clear", float firewood = 8f, float water = 4f,
public string CampTick(string survivorId) {
public string CampTickDemo(string survivorId) => CampTick(survivorId);
public string ResolveCampEncounter(string survivorId, string outcome) {
public string ResolveCampEncounterDemo(string survivorId, string outcome) => ResolveCampEncounter(survivorId, outcome);
public string BreakCamp(string survivorId, bool retreat = false) {
public string BreakCampDemo(string survivorId, bool retreat = false) => BreakCamp(survivorId, retreat);
public CampState? GetCampState(string survivorId) => Engine.GetCampState(survivorId);
public string StatusLine() {
public List<ExpeditionState> CaptureSave() => Engine.CaptureState();
public void RestoreSave(List<ExpeditionState> state) => Engine.RestoreState(state);
public ExpeditionAggregateState CaptureSaveAggregate() {
public void RestoreSaveAggregate(ExpeditionAggregateState aggregate) {
public string StartDive(string siteId = "site_exp09_ss_sovereign") {
public string StartDiveDemo(string siteId = "site_exp09_ss_sovereign") => StartDive(siteId);
public string AdvanceDive() {
public string AdvanceDiveDemo() => AdvanceDive();
public string TickDiveOxygen() {
public string TickDiveOxygenDemo() => TickDiveOxygen();
public string CommitDiveChoice(string choice) {
public string CommitDiveChoiceDemo(string choice) => CommitDiveChoice(choice);
public string DiveStatusLine() {
```


# Appendix Q.575 — Additional Current Architecture Evidence: `src/Host/HostCli.AudioAccessibility.cs`

### `src/Host/HostCli.AudioAccessibility.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 220 lines / 10335 bytes.
- SHA-256: `bdc02a8dc17fac2a8f84cfce67d17001bf2f66666b48d3ff8a6890c42b2dbe32`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class HostCliAudioAccessibility
public static int RunSelfTest(string dataDir) {
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


# Appendix Q.577 — Additional Current Architecture Evidence: `src/Host/HostCli.StartingSupplies.cs`

### `src/Host/HostCli.StartingSupplies.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 154 lines / 6155 bytes.
- SHA-256: `741b811cd864dfe7e7775ceb17d795f7054dffa6a12ab0fa273a6af3aac9bd30`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=1.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunStartingSuppliesSelfTest(string dataDirectory) {
```


# Appendix Q.578 — Additional Current Architecture Evidence: `src/Host/HostEventAdapter.cs`

### `src/Host/HostEventAdapter.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 257 lines / 11622 bytes.
- SHA-256: `996e3e8410684edcbe5863d95ee66ac01ef48defe66758674318045cca36205f`.
- Architecture signals: seeded references=0; save/restore symbols=3; typed event declarations=7; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class HostEventState
public List<string> triggeredEventIds = new List<string>();
public Dictionary<string, int> eventTriggerDays = new Dictionary<string, int>();
public List<string> dispatchedSourceIds = new List<string>();
public string lastDispatchedEvent = string.Empty;
public class HostEventAdapter
public const string EventThinMarginDisclosure = "event_the_thin_margin_disclosure";
public const string EventThirstySeason = "event_the_thirsty_season";
public const string EventOsteophageExplanation = "event_osteophage_explanation";
public const string EventMeasurementBroadcast = "event_measurement_broadcast";
public event Action<string, string>? OnEventDispatched;
public event Action? StateChanged;
public HostEventState State => _state;
public IReadOnlyList<string> TriggeredEventIds => _state.triggeredEventIds;
public string LastDispatchedEvent => _state.lastDispatchedEvent;
public IReadOnlyList<string> DispatchedSourceIds => _state.dispatchedSourceIds;
public bool HasTriggered(string eventId) => _state.triggeredEventIds.Contains(eventId);
public int GetTriggerDay(string eventId) {
public void TriggerEvent(string eventId, int currentDay) {
public bool DispatchCatalogEvent(string eventId, string bodyText, int currentDay, string sourceId) {
public void EvaluateTriggers( int day, bool hydroAuditDone, bool hydroSeized, bool osteophageInquiry, bool coldCountBroadcast)
public void Dispose() {
public HostEventState CaptureState() {
public void RestoreState(HostEventState? state) {
```


# Appendix Q.579 — Additional Current Architecture Evidence: `src/Host/InventoryHostSession.cs`

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


# Appendix Q.580 — Additional Current Architecture Evidence: `src/Journal/JournalCodex.cs`

### `src/Journal/JournalCodex.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 464 lines / 19139 bytes.
- SHA-256: `1881856b99bb59df45dc78c3db6f2d4944d2bcf971d7b8ea0ef9955300a4f798`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum JournalTab
public struct JournalCodexRow
public string? DisplayName;
public string? Meta;
public string? Body;
public bool IsLocked;
public IReadOnlyList<JournalCodexLink>? Links;
public string? NavigationId;
public static JournalCodexRow Locked(string? displayName) {
public sealed class JournalCodexLink
public string Id { get; }
public string Label { get; }
public string RoutePrefix { get; }
public class JournalCodex
public JournalCatalogs Catalogs => _catalogs;
public IReadOnlyList<JournalCodexRow> BuildRows(JournalTab tab) {
```


# Appendix Q.581 — Additional Current Architecture Evidence: `src/Main.Inventory.cs`

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


# Appendix Q.582 — Additional Current Architecture Evidence: `src/Main.Narrative.cs`

### `src/Main.Narrative.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 728 lines / 29845 bytes.
- SHA-256: `b588f3d83df8088676183b79152ffa57c62a3b5d30ed8c7cb73ae0a4b7ece863`.
- Architecture signals: seeded references=0; save/restore symbols=6; typed event declarations=0; textual Godot mentions=9; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.583 — Additional Current Architecture Evidence: `src/Main.UiTests.Journal.cs`

### `src/Main.UiTests.Journal.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 80 lines / 3046 bytes.
- SHA-256: `2417f02d3c895eb0854c5aee4be2b669007306b54ec477a5acc0e67a08073028`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.584 — Additional Current Architecture Evidence: `src/UI/BlackProjectsArchivePanel.cs`

### `src/UI/BlackProjectsArchivePanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 882 lines / 38369 bytes.
- SHA-256: `436bc8edc0c04168069cc7cc05b4886056047b6134f99088b650fae7d2936d1d`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class BlackProjectsArchivePanel : Control, IBindablePanel
public event Action? OnClose;
public enum FamilyFilter
public bool IsBound => _system != null;
public string? SelectedRecordId => _selectedRecordId;
public FamilyFilter CurrentFilter => _currentFilter;
public void Bind(BlackProjectsArchiveSystem system, JournalSystem? journal = null) {
public void Unbind() {
public override void _ExitTree() {
public override void _Ready() {
public void Open() {
public void Close() {
public void RefreshView() {
public void SelectRecord(string recordId) {
```


# Appendix Q.585 — Additional Current Architecture Evidence: `src/UI/ShelterBarterPanel.cs`

### `src/UI/ShelterBarterPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1141 lines / 53648 bytes.
- SHA-256: `e3ccac6bcde276573c923c8616bd130b7497d3afdd1feb095b239637f5b065c6`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class ShelterBarterPanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _barterSystem != null;
public string? SelectedCaravanId => _selectedCaravanId;
public IReadOnlyDictionary<string, int> PlayerOffers => _playerOffers;
public IReadOnlyDictionary<string, int> PlayerRequests => _playerRequests;
public void Bind( ShelterBarterSystem barterSystem, Ashfall.Core.Inventory.Inventory inventory, JournalSystem? journal = null, Func<string, ItemDefinition?>? itemLookup = null) {
public void Unbind() {
public override void _Ready() {
public void Open() {
public void Close() {
public override void _UnhandledInput(InputEvent @event) {
public override void _ExitTree() {
public void RefreshView() {
public static string FormatItemName(string itemId, Func<string, ItemDefinition?>? lookup = null) {
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.


# Appendix S — Quality Assurance Pass Record: Plan 78

This record is part of the planning artifact, not a fresh runtime test result.

## Pass 1 — content and premise accuracy
- Content pass replaced the stale 3→12 target with the current 12-ink census.
- The historical baseline is separated from the current source/data/test authority.
- Current row counts and owner boundaries are stated without using count as a quality proxy.

## Pass 2 — integration architecture
- Integration pass traced loader → archive desk → inventory → knowledge result → save.
- Core, data, host, UI, save, event and test seams are named with current paths.
- The plan does not authorize a parallel save section, catalog, manager or host cache.

## Final precision and reaccuracy pass
- Precision pass corrected the live ArchiveDeskSystem path and rejects document duplication.
- Every embedded current-file hash, focused runner command and master-authority reference is rechecked.
- Any proposed future seam is labeled as requiring a separate claim and premise verification.
