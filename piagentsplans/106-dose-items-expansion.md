# Plan 106 — Dose Item Catalog and Radiation-Equipment Reachability

> **Rebuild status:** COMPLETE 15-ITEM DOSE CONTENT AUTHORITY — ITEM-INTEGRATION MAINTENANCE
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

- The original plan correctly identified that a radiation bureaucracy needs physical tools, documentation and protective supplies. The current catalog is now 15 rows and the global item loader includes `dose_items.json` explicitly.
- The live route is dose item catalog → merged item authority → dose content/quest grant or inventory use → current medical/dose/quest owners. The item catalog does not itself apply radiation, disease or palliative effects.
- The remaining quality work is a reachability and semantics audit: every item must resolve in the merged catalog, every grant must be connected to a current consumer, and the UI must not imply an item has an unmodeled medical effect.

**Bounded outcome:** Retire the old 5→15 data-only premise. Current `dose_items.json` has 15 unique items, is merged by `ItemCatalogLoader`, and is consumed through dose content/quest/location surfaces. The rebase protects item resolution, bounded bureaucratic tone and the distinction between an item grant and a dose/health effect.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `dose_items.json` is present with 15 unique items and schema version 1; `ItemCatalogLoader` includes it in the merged item file set.
- `DoseContentCatalog` loads the dose item content alongside locations and quests; `DoseContentHostHarness` and focused tests validate quest grants against the item catalog.
- `DoseLedgerHostSession` and `DoseGeographyPanel` are current presentation/state seams; medical/palliative effects remain owned by their respective systems.
- The current Plan 90/81 integration tests prove catalog coexistence; fresh item consumer tests should be run when a row or grant changes.

**Master-authority sections applied to this rebase:**

- Master authority Volume 28 verification cookbook: focused evidence before broad gates.
- Lane D save/state/compatibility guidance: owner DTOs, migration and restore proof.
- Lane E UI/UX/accessibility guidance: truthful projections and keyboard/controller lifecycle.
- Lane G testing guidance: smallest affected target, negative cases and deterministic replay.
- Anti-padding protocol: content exhaustion may end the plan before the character checkpoint.
- Volume 32 data-authority and item-consumer guidance.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace 5→15 with a 15-row current census and merged-item reference matrix.
- Classify items as measurement, documentation, protection, palliative, cohort or grant-only.
- Require every item to resolve in `ItemCatalogLoader` and have a current consumer or explicit non-playable archive status.
- Keep item metadata separate from dose, disease, health and quest state.

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
| merged dose item identity and type | ItemCatalogLoader | `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs` | Sole global item registry. |
| dose location/item/quest definitions | DoseContentCatalog | `Assets/Ashfall.Core/DoseContentCatalog.cs` | Loads dose content without applying gameplay effects. |
| content quest progression and item grants | Dose quest/grant owner | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs; src/Host/DoseLedgerHostSession.cs` | Routes grants through canonical quest/inventory seams. |
| item resolution and content integration | Dose focused tests | `Ashfall.Core.Tests/DoseContentCatalogTests.cs; Ashfall.Core.Tests/Culture/Plan90_78DoseInksIntegrationTests.cs` | Executable current evidence. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Dose Item Catalog and Radiation-Equipment Reachability
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ ItemCatalogLoader
│   merged dose item identity and type
│ DoseContentCatalog
│   dose location/item/quest definitions
│ Dose quest/grant owner
│   content quest progression and item grants
│ Dose focused tests
│   item resolution and content integration
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

1. **Preserve current state ownership.** ItemCatalogLoader owns merged dose item identity and type: Sole global item registry.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| merged dose item identity and type | ItemCatalogLoader | `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs` | Sole global item registry. |
| dose location/item/quest definitions | DoseContentCatalog | `Assets/Ashfall.Core/DoseContentCatalog.cs` | Loads dose content without applying gameplay effects. |
| content quest progression and item grants | Dose quest/grant owner | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs; src/Host/DoseLedgerHostSession.cs` | Routes grants through canonical quest/inventory seams. |
| item resolution and content integration | Dose focused tests | `Ashfall.Core.Tests/DoseContentCatalogTests.cs; Ashfall.Core.Tests/Culture/Plan90_78DoseInksIntegrationTests.cs` | Executable current evidence. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load/merge dose items
2. validate IDs, types, weights and trade values
3. resolve quest grant or inventory source
4. commit item through canonical inventory/quest owner
5. apply only an explicitly wired medical/dose effect
6. project current dose/item UI
7. capture/restore existing inventory, quest and dose state

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Dose item definitions are catalog data; inventory possession and quest progress are owner state.
- An item grant must be atomic and cannot create a hidden dose or health effect.
- Medical/palliative behavior belongs to its current system and requires an explicit consumer contract.
- Restore preserves inventory/quest state through existing owners without re-granting items.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Every dose item ID is unique in the merged item catalog.
- A quest grant references an existing item and cannot double-grant on replay.
- An item with no current effect is described as equipment/documentation, not as a treatment.
- Unknown item grants fail closed without mutating inventory.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `dose_items.json` is a dedicated source file merged by `ItemCatalogLoader`.
- No parallel dose item list or duplicate root item rows.
- New rows need valid item type, bounded physical values, a consumer and a data-integrity check.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use existing inventory, quest and dose save sections.
- No new dose-item save section is justified.
- A legacy item grant must not replay when quest completion is already recorded.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Quest/inventory grant order is stable and transaction-safe.
- No wall-clock or random ordering determines item resolution.
- Replay compares inventory, quest and effect facts.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Quest completion/grant facts come from the current quest owner.
- Inventory add/remove facts come from the canonical inventory transaction boundary.
- Dose/health effects are emitted only by their current owners when a real consumer exists.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/DoseLedgerHostSession.cs
- src/UI/DoseGeographyPanel.cs
- src/Main.Quests.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Dose items should feel bureaucratic, practical and fictional.
- A ledger or calibration tool can carry story without becoming a magic stat item.
- Descriptions should avoid real-world medical misinformation and copied text.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A dose item is absent from the merged catalog. | ItemCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A quest grants an item twice after restore. | DoseContentCatalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A catalog description promises an effect no owner applies. | Dose quest/grant owner | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A panel or host mutates inventory outside the transaction owner. | Dose focused tests | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A new item becomes a parallel medical authority. | ItemCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/DoseContentCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Culture/Plan90_78DoseInksIntegrationTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — merged catalog census | Read dose JSON, ItemCatalogLoader and dose content owner. | 15 rows and global resolution are proven. | No production path until the owning implementation package is separately claimed. |
| 1 — grant/effect matrix | Classify grants, equipment and real effects. | No item claims unsupported gameplay. | No production path until the owning implementation package is separately claimed. |
| 2 — transaction/save proof | Trace quest grant and restore. | Exactly-once and no shadow state. | No production path until the owning implementation package is separately claimed. |
| 3 — content/UI QA | Review bureaucratic tone and truthful labels. | Player sees actual item role. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/dose_items.json | READ ONLY; MODIFY only for proven content gap | 15-row authority |
| Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs | READ ONLY | Merged registry |
| Assets/Ashfall.Core/DoseContentCatalog.cs | READ ONLY | Dose content loader |
| Ashfall.Core.Tests/DoseContentCatalogTests.cs | EXTEND only for a proven gap | Focused proof |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Adding a second item registry. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Granting an item outside inventory transaction. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Describing an item as a treatment without a gameplay consumer. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Duplicating dose content into root items. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new dose item count for this rebase.
- No new medical/dose effect.
- No save-section change.
- No production edits in this rebase.

# 23. Rollback and Recovery

- Revert the plan file.
- Future data/quest changes retain prior valid catalog and focused grant tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 15 current dose items and merged loading are documented.
- Item/quest/dose ownership boundaries are explicit.
- Exactly-once and transaction contracts are named.
- No parallel item authority is proposed.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace 5→15 with a 15-row current census and merged-item reference matrix.
- Classify items as measurement, documentation, protection, palliative, cohort or grant-only.
- Require every item to resolve in `ItemCatalogLoader` and have a current consumer or explicit non-playable archive status.
- Keep item metadata separate from dose, disease, health and quest state.

## MUST NOT DO

- No new dose item count for this rebase.
- No new medical/dose effect.
- No save-section change.
- No production edits in this rebase.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/DoseContentCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Culture/Plan90_78DoseInksIntegrationTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — merged catalog census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: merged dose item identity and type → ItemCatalogLoader; dose location/item/quest definitions → DoseContentCatalog; content quest progression and item grants → Dose quest/grant owner; item resolution and content integration → Dose focused tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 106.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 106 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by ItemCatalogLoader or any other authority, hide uncertainty, or introduce a gameplay-only counter.

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


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs`

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


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs`

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


# Appendix B.05 — Current Code Architecture: `src/Host/DoseLedgerHostSession.cs`

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


# Appendix B.06 — Current Code Architecture: `src/UI/DoseGeographyPanel.cs`

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


# Appendix B.07 — Current Code Architecture: `src/Main.Quests.cs`

### `src/Main.Quests.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 123 lines / 4391 bytes.
- SHA-256: `2e07f31b85bff510e1d6f1e365fc00a559078496cdc90d25fd57bf68afd3d1a2`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
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


# Appendix C.09 — Catalog Census: `Assets/StreamingAssets/Data/items.json`

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


# Appendix D.10 — Existing Focused Test Inventory: `Ashfall.Core.Tests/DoseContentCatalogTests.cs`

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


# Appendix D.11 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Culture/Plan90_78DoseInksIntegrationTests.cs`

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


# Appendix E.13 — Supporting Code Evidence: `Assets/Ashfall.Core/Radiation/RadiationSystem.cs`

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


# Appendix E.14 — Supporting Code Evidence: `Assets/Ashfall.Core/Medical/MedicalWardSystem.cs`

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


# Appendix E.15 — Supporting Code Evidence: `Assets/Ashfall.Core/Quests/NarrativeQuestlineSystem.cs`

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


# Appendix E.16 — Supporting Code Evidence: `src/Dose/DoseRegisterSurface.cs`

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


# Appendix G.17 — Supporting Regression Evidence: `Ashfall.Core.Tests/QuestlineSystemTests.cs`

### `Ashfall.Core.Tests/QuestlineSystemTests.cs`

- Current test declarations: Fact=33, Theory=0, InlineData=0.
- File lines: 576; SHA-256: `c199235a9c39313d20003da716f52d8af47a482870e1c93c547e52337a0e9906`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
RegisterQuestline_AddsToInternalCatalog
RegisterQuestline_NoDuplicates
RegisterQuestline_NullIsSafe
BuiltInCatalog_HasEightQuestlines
BuiltInCatalog_AllQuestlinesHaveFirstStage
BuiltInCatalog_AllQuestlinesHaveValidDayRanges
BuiltInCatalog_GarrisonBloodDebt_Exists
BuiltInCatalog_TheLastBroadcast_Exists
GetAvailableQuestlines_ReturnsQuestlinesInDayWindow
GetAvailableQuestlines_ExcludesOutOfWindowQuests
GetAvailableQuestlines_ExcludesAlreadyActive
GetAvailableQuestlines_ExcludesCompleted
StartQuestline_ReturnsTrue_AndCreatesActiveRecord
StartQuestline_ReturnsFalse_IfAlreadyActive
StartQuestline_ReturnsFalse_ForUnknownId
StartQuestline_FiresOnQuestlineStartedEvent
TakeChoice_ReturnsNull_IfQuestNotActive
TakeChoice_ReturnsNull_ForInvalidChoiceId
TakeChoice_GoodPath_AdvancesToNextStage
TakeChoice_RuthlessPath_ResolvesImmediately
TakeChoice_RecordsChoiceInHistory
TakeChoice_AccumulatesMoraleAndGuilt
TakeChoice_FiresOnQuestChoiceTakenEvent
TakeChoice_FiresOnQuestlineResolved_WhenTerminal
TakeChoice_CannotActOnResolvedQuestline
TakeChoice_FailedTerminal_RecordsAsFailure
CaptureState_IsDeepCopy
CaptureState_CanRestoreSystemFromState
GarrisonBloodDebt_CompassionatePath_ReachesProtectedResolution
GarrisonBloodDebt_ComplyCausesHighGuiltDelta
TheLastBroadcast_DenyingAntennaEndsQuestWithHighGuilt
TheLastBroadcast_RequestArchive_GrantsItem
AshSignRevelation_BuryingEvidenceLeadsToFailure
```


# Appendix G.18 — Supporting Regression Evidence: `Ashfall.Core.Tests/NeedsRadiationSystemTests.cs`

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


# Appendix G.19 — Supporting Regression Evidence: `Ashfall.Core.Tests/DoseItemExpansionTests.cs`

### `Ashfall.Core.Tests/DoseItemExpansionTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 291; SHA-256: `160925dbba061f6b945ecb8ebc04939293c541fc9c6311ac879fe5d1859f0c11`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
DoseItems_CatalogLoadsExactlyFifteenItems
DoseItems_PreservesOriginalFiveAndPlan27Items
DoseItems_NewSixItemsPresentWithExpectedMetadata
DoseItems_AllIdsUniqueAndFollowItemPrefix
DoseItems_NamesAndDescriptionsNonEmpty
DoseItems_WeightsAndTradeValuesWithinValidRanges
DoseItems_CategoriesMatchValidGrammar
DoseItems_GlobalItemNamespaceCollisionSafety
DoseItems_MedicalRealismSanityChecks
DoseItems_LoadedIntoCanonicalItemCatalog
```


# Appendix G.20 — Supporting Regression Evidence: `Ashfall.Core.Tests/NarrativeQuestlineSystemTests.cs`

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


# Appendix H.21 — Supporting Authority Document: `docs/CURRENT_AUTHORITY.md`

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
| merged dose item identity and type | ItemCatalogLoader | dose location/item/quest definitions | DoseContentCatalog | Owner emits/reads a typed fact; no mirror state. |
| merged dose item identity and type | ItemCatalogLoader | content quest progression and item grants | Dose quest/grant owner | Owner emits/reads a typed fact; no mirror state. |
| merged dose item identity and type | ItemCatalogLoader | item resolution and content integration | Dose focused tests | Owner emits/reads a typed fact; no mirror state. |
| dose location/item/quest definitions | DoseContentCatalog | merged dose item identity and type | ItemCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| dose location/item/quest definitions | DoseContentCatalog | content quest progression and item grants | Dose quest/grant owner | Owner emits/reads a typed fact; no mirror state. |
| dose location/item/quest definitions | DoseContentCatalog | item resolution and content integration | Dose focused tests | Owner emits/reads a typed fact; no mirror state. |
| content quest progression and item grants | Dose quest/grant owner | merged dose item identity and type | ItemCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| content quest progression and item grants | Dose quest/grant owner | dose location/item/quest definitions | DoseContentCatalog | Owner emits/reads a typed fact; no mirror state. |
| content quest progression and item grants | Dose quest/grant owner | item resolution and content integration | Dose focused tests | Owner emits/reads a typed fact; no mirror state. |
| item resolution and content integration | Dose focused tests | merged dose item identity and type | ItemCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| item resolution and content integration | Dose focused tests | dose location/item/quest definitions | DoseContentCatalog | Owner emits/reads a typed fact; no mirror state. |
| item resolution and content integration | Dose focused tests | content quest progression and item grants | Dose quest/grant owner | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace 5→15 with a 15-row current census and merged-item reference matrix. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Classify items as measurement, documentation, protection, palliative, cohort or grant-only. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Require every item to resolve in `ItemCatalogLoader` and have a current consumer or explicit non-playable archive status. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Keep item metadata separate from dose, disease, health and quest state. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.553 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/YearOfAsh/YearOfAshSave.cs`

### `Assets/Ashfall.Core/YearOfAsh/YearOfAshSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 371 lines / 17304 bytes.
- SHA-256: `bcfc35c7049581fe1513883dda1e95adfc06458f1b8a10eb520ff4ca1e54fc5d`.
- Architecture signals: seeded references=0; save/restore symbols=20; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class YearOfAshSave
public const int CurrentSaveVersion = 5;
public int saveVersion = CurrentSaveVersion;
public int simDay = 180;
public YearOfAshTimelineState timeline = new YearOfAshTimelineState();
public DoorEncounterSystemState encounters = new DoorEncounterSystemState();
public FactionWarSystemState factionWar = new FactionWarSystemState();
public WarlordDoctrineState warlord = new WarlordDoctrineState();
public YearOfAshDeepFreezeState deepFreeze = new YearOfAshDeepFreezeState();
public YearOfAshRadonState radon = new YearOfAshRadonState();
public QuestlineSystemState quests = new QuestlineSystemState();
public FactionWarChainRunnerState factionWarChainRunner = new FactionWarChainRunnerState();
public IceRoadState iceRoad = new IceRoadState();
public string Checksum = string.Empty;
public class YearOfAshSaveV4
public int saveVersion = 4;
public int simDay = 180;
public YearOfAshTimelineState timeline = new YearOfAshTimelineState();
public DoorEncounterSystemState encounters = new DoorEncounterSystemState();
public FactionWarSystemState factionWar = new FactionWarSystemState();
public WarlordDoctrineState warlord = new WarlordDoctrineState();
public YearOfAshDeepFreezeState deepFreeze = new YearOfAshDeepFreezeState();
public YearOfAshRadonState radon = new YearOfAshRadonState();
public QuestlineSystemState quests = new QuestlineSystemState();
public FactionWarChainRunnerState factionWarChainRunner = new FactionWarChainRunnerState();
public string Checksum = string.Empty;
public class YearOfAshSaveV1
public int saveVersion = 1;
public int simDay = 180;
public YearOfAshTimelineState timeline = new YearOfAshTimelineState();
public DoorEncounterSystemState encounters = new DoorEncounterSystemState();
public FactionWarSystemState factionWar = new FactionWarSystemState();
public string Checksum = string.Empty;
public class YearOfAshSaveV2
public int saveVersion = 2;
public int simDay = 180;
public YearOfAshTimelineState timeline = new YearOfAshTimelineState();
public DoorEncounterSystemState encounters = new DoorEncounterSystemState();
public FactionWarSystemState factionWar = new FactionWarSystemState();
public YearOfAshDeepFreezeState deepFreeze = new YearOfAshDeepFreezeState();
public YearOfAshRadonState radon = new YearOfAshRadonState();
public QuestlineSystemState quests = new QuestlineSystemState();
public string Checksum = string.Empty;
public class YearOfAshSaveV3
public int saveVersion = 3;
public int simDay = 180;
public YearOfAshTimelineState timeline = new YearOfAshTimelineState();
public DoorEncounterSystemState encounters = new DoorEncounterSystemState();
public FactionWarSystemState factionWar = new FactionWarSystemState();
public WarlordDoctrineState warlord = new WarlordDoctrineState();
public YearOfAshDeepFreezeState deepFreeze = new YearOfAshDeepFreezeState();
public YearOfAshRadonState radon = new YearOfAshRadonState();
public QuestlineSystemState quests = new QuestlineSystemState();
public string Checksum = string.Empty;
public static class YearOfAshSaveCodec
public static YearOfAshSave Capture( YearOfAshTimelineSystem timeline, DoorEncounterSystem encounters, FactionWarSystem factionWar, IClock clock, YearOfAshDeepFreezeSystem? deepFreeze = null,
public static void Restore( YearOfAshSave save, YearOfAshTimelineSystem timeline, DoorEncounterSystem encounters, FactionWarSystem factionWar, YearOfAshDeepFreezeSystem? deepFreeze = null,
public static string Encode(YearOfAshSave save, IJsonSerializer json) {
public static YearOfAshSave Decode(string jsonText, IJsonSerializer json) {
```


# Appendix Q.554 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Verdict/VerdictSave.cs`

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


# Appendix Q.555 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/DoseLedgerSave.cs`

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


# Appendix Q.556 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Economy/TradeCreditCoordinator.cs`

### `Assets/Ashfall.Core/Economy/TradeCreditCoordinator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 276 lines / 13673 bytes.
- SHA-256: `0e404a480a80b155b382a7ad9b99b296f2c67ff2a2afaef3b86a58edc97eae74`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CreditOffer
public string TemplateId { get; }
public string CreditorId { get; }
public string PrincipalItemId { get; }
public int PrincipalQuantity { get; }
public int TermDays { get; }
public float Rate { get; }
public string ForfeitDescription { get; }
public string ConsequenceSummary { get; }
public string CreditorDisplayName { get; }
public sealed class CreditOfferResult
public bool Eligible { get; private set; }
public CreditOffer? Offer { get; private set; }
public string Reason { get; private set; } = string.Empty;
public static CreditOfferResult Eligible_(CreditOffer offer) =>
public static CreditOfferResult Reject(string reason) =>
public sealed class CreditAcceptResult
public bool Success { get; private set; }
public string DebtorId { get; private set; } = string.Empty;
public string Reason { get; private set; } = string.Empty;
public static CreditAcceptResult Ok(string debtorId) =>
public static CreditAcceptResult Fail(string reason) =>
public sealed class TradeCreditCoordinator
public bool HasUnpaidDebtFromCreditor(string creditorId) {
public CreditOfferResult TryBuildCreditOffer(string creditorId, string requestedItemId) {
public CreditAcceptResult TryAcceptCredit(string templateId, string creditorId) {
```


# Appendix Q.557 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Disease/DiseaseQuarantineCoordinator.cs`

### `Assets/Ashfall.Core/Disease/DiseaseQuarantineCoordinator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 375 lines / 15683 bytes.
- SHA-256: `3a926de1f7f6e5bb05b4f4bd0a57492ceb92dac2142f13ddd6ca4aa3303529fc`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DiseaseQuarantineCoordinator
public event Action<string, string, int>? OnQuarantineAssigned;
public event Action<string, string, int>? OnQuarantineReleased;
public event Action<int, int, float>? OnDailyBurdenProcessed;
public MedicalWardSystem Ward => _medicalWard;
public DiseaseSystem Disease => _diseaseSystem;
public DutyRosterSystem? Roster => _dutyRoster;
public ContainmentCapability Containment =>
public bool IsIsolated(string survivorId) {
public float GetIsolationQuality(string survivorId) {
public MedicalBed? FindAvailableIsolationBed() {
public QuarantineCommandPreview PreviewAssignIsolation(string survivorId) {
public QuarantineCommandResult ExecuteAssignIsolation(string survivorId, int day) {
public QuarantineCommandPreview PreviewReleaseIsolation(string survivorId) {
public QuarantineCommandResult ExecuteReleaseIsolation(string survivorId, int day) {
public void TickDaily(int day) {
public void Rehydrate() {
public sealed class QuarantineCommandPreview
public bool CanExecute { get; set; }
public string Reason { get; set; } = string.Empty;
public string SurvivorId { get; set; } = string.Empty;
public string TargetBedId { get; set; } = string.Empty;
public string? ConflictingRole { get; set; }
public float ProjectedIsolationQuality { get; set; } = 1.0f;
public Dictionary<string, int> DailySupplyCost { get; set; } = new Dictionary<string, int>();
public static QuarantineCommandPreview Success(string survivorId, string bedId, string? conflictingRole, float projectedQuality, Dictionary<string, int> costs) =>
public static QuarantineCommandPreview Blocked(string reason, string survivorId) =>
public sealed class QuarantineCommandResult
public bool Success { get; set; }
public string Reason { get; set; } = string.Empty;
public string SurvivorId { get; set; } = string.Empty;
public string BedId { get; set; } = string.Empty;
public int Day { get; set; }
public static QuarantineCommandResult Ok(string survivorId, string bedId, int day) =>
public static QuarantineCommandResult Fail(string reason, string survivorId, string bedId = "", int day = 0) =>
```


# Appendix Q.558 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/DoseQuestMigration.cs`

### `Assets/Ashfall.Core/DoseQuestMigration.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 161 lines / 6557 bytes.
- SHA-256: `fed1e2e9473b560586b087b3e61adb70849e8af6349caf5411195f6eb89a640d`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class DoseQuestMigration
public static readonly string[] CanonicalQuestlineIds = {
public static bool IsDoseQuestline(string questlineId) {
public static int AdoptFromYearOfAsh( QuestlineSystemState doseState, QuestlineSystemState yearOfAshState) {
public static int StripFromYearOfAsh(QuestlineSystemState yearOfAshState) {
```


# Appendix Q.559 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/AutopsySystem.cs`

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


# Appendix Q.560 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`

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


# Appendix Q.561 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radiation/ExposureBreakdown.cs`

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


# Appendix Q.562 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs`

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


# Appendix Q.563 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Verdict/VerdictQuestMigration.cs`

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


# Appendix Q.564 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/DecontaminationSystem.cs`

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


# Appendix Q.565 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Quests/DynamicQuestlines.cs`

### `Assets/Ashfall.Core/Quests/DynamicQuestlines.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 373 lines / 13835 bytes.
- SHA-256: `ca0b16a60b54272e6e6fa84f585b21d82a457d61a7213f773d35a6889f41dc92`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=19; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum DynamicQuestStatus
public sealed class DynamicQuestInstance
public string QuestId { get; set; } = string.Empty;
public string IncidentId { get; set; } = string.Empty;
public string Title { get; set; } = string.Empty;
public string Description { get; set; } = string.Empty;
public string TargetLocationId { get; set; } = string.Empty;
public List<string> TargetSurvivorIds { get; set; } = new List<string>();
public int TriggerDay { get; set; }
public int? DeadlineDay { get; set; }
public int CurrentStageIndex { get; set; }
public List<string> Stages { get; set; } = new List<string>();
public int ProgressCurrent { get; set; }
public int ProgressRequired { get; set; }
public DynamicQuestStatus Status { get; set; } = DynamicQuestStatus.Active;
public sealed class DynamicQuestSave
public string systemId = DynamicQuestlineSystem.SystemId;
public int schemaVersion = 1;
public List<DynamicQuestInstance> activeInstances = new List<DynamicQuestInstance>();
public List<string> completedIds = new List<string>();
public List<string> failedIds = new List<string>();
public List<string> triggeredIncidentIds = new List<string>();
public int currentDay;
public sealed class DynamicQuestlineSystem
public const string SystemId = "dynamic_quests";
public const string RescueMinersQuestId = "quest_rescue_trapped_miners";
public const string InvestigateRadioDepotQuestId = "quest_investigate_radio_depot";
public const string ArmoryMunitionsRefurbishQuestId = "quest_armory_munitions_refurbish";
public DynamicQuestSave State => _state;
public IReadOnlyList<DynamicQuestInstance> ActiveQuests => _state.activeInstances;
public IReadOnlyList<string> CompletedIds => _state.completedIds;
public IReadOnlyList<string> FailedIds => _state.failedIds;
public event Action<DynamicQuestInstance>? OnQuestTriggered;
public event Action<DynamicQuestInstance>? OnQuestStageAdvanced;
public event Action<DynamicQuestInstance>? OnQuestCompleted;
public event Action<DynamicQuestInstance>? OnQuestFailed;
public event Action? OnStateChanged;
public bool HasIncidentTriggered(string incidentId) {
public DynamicQuestInstance? GetActiveQuest(string questId) {
public DynamicQuestInstance? TriggerRescueMinersQuest( string incidentId, string sectorId, IReadOnlyList<string> trappedSurvivorIds, int triggerDay, int deadlineDays = 3,
public DynamicQuestInstance? TriggerInvestigateRadioDepotQuest( string interceptId, string canonicalLocationId, int triggerDay, int? deadlineDays = null) {
public DynamicQuestInstance? TriggerArmoryMunitionsRefurbishQuest( string incidentId, int triggerDay, int weaponsNeedingRepair) {
public bool AdvanceQuestProgress(string questId, int progressAmount = 1) {
public bool AdvanceQuestStage(string questId) {
public bool CompleteQuest(string questId) {
public bool FailQuest(string questId) {
public void TickDay(int day) {
public DynamicQuestSave CaptureState() {
public void RestoreState(DynamicQuestSave? saved) {
```


# Appendix Q.566 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/World/AnomalyHazardSystem.cs`

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


# Appendix Q.567 — Additional Current Architecture Evidence: `src/Host/SurvivorsHostSession.cs`

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


# Appendix Q.568 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`

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


# Appendix Q.569 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Medical/ClinicalWardTriageEngine.cs`

### `Assets/Ashfall.Core/Medical/ClinicalWardTriageEngine.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 307 lines / 13247 bytes.
- SHA-256: `400c8f9b33d692cd0fd0907e8cb2509d030534d7d1882894ae69f1575373556b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum TriagePriorityTier
public enum WardCleanlinessGrade
public sealed class ClinicalWardState
public string WardId                     { get; set; } = string.Empty;
public int TotalBeds                     { get; set; } = 10;
public int OccupiedBeds                  { get; set; } = 0;
public int SterileSupplyStockPermille    { get; set; } = 800;
public int StaffingReadinessPermille     { get; set; } = 750;
public WardCleanlinessGrade Cleanliness  { get; set; } = WardCleanlinessGrade.AntisepticStandard;
public int IsolationBedsTotal           { get; set; } = 2;
public int IsolationBedsOccupied         { get; set; } = 0;
public ClinicalWardState Clone() => new ClinicalWardState
public readonly struct TriageEvaluationResult
public TriagePriorityTier AssignedPriority    { get; }
public int EstimatedUrgencyMinutes            { get; }
public bool BedAvailable                      { get; }
public bool RequiresIsolation                 { get; }
public string RecommendationNotice            { get; }
public readonly struct SurgicalReadinessResult
public bool IsApprovedForSurgery               { get; }
public int ShockRiskPermille                   { get; }
public int InfectionRiskPermille               { get; }
public int SterileSuppliesConsumedPermille     { get; }
public string BottleneckReason                 { get; }
public static class ClinicalWardTriageEngine
public const int MinimumSterileStockForSurgeryPermille = 300;
public const int MinimumStaffingForSurgeryPermille = 500;
public static TriageEvaluationResult EvaluatePatientTriage( int traumaSeverityPermille, int vitalStabilityPermille, bool isContagious, ClinicalWardState ward) {
public static SurgicalReadinessResult EvaluateSurgicalPreparation( ClinicalWardState ward, int procedureComplexityPermille, int patientConditionPermille, int surgerySeed) {
public static int ComputeBedTurnoverCapacity(ClinicalWardState ward, int averageLengthOfStayDays) {
public static int CalculateNosocomialInfectionRisk( WardCleanlinessGrade cleanliness, int wardOccupancyPermille, int sterileSupplyPermille) {
```


# Appendix Q.570 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs`

### `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 208 lines / 9043 bytes.
- SHA-256: `d657335e24b67edfcc6456f8b6b17d5056e1b58a045b8082da0f5a8a76415d81`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=7; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MentalHealthState
public string systemId = MentalHealthCrisisSystem.SystemId;
public List<CrisisCase> activeCases = new List<CrisisCase>();
public List<CrisisCase> resolvedCases = new List<CrisisCase>();
public int wardCapacity = 2;
public int currentOccupancy;
public sealed class CrisisCase
public string caseId = string.Empty;
public string survivorId = string.Empty;
public CrisisProfile profile;
public CrisisAcuity acuity;
public int dayStarted = -1;
public int dayResolved = -1;
public CrisisStatus status;
public string assignedCaregiverId = string.Empty;
public string intervention = string.Empty;
public float stressInput;
public float recoveryProgress;
public List<string> sideEffects = new List<string>();
public enum CrisisAcuity { Mild, Moderate, Severe, Critical } public enum CrisisStatus { Active, InTreatment, Recovering, Recovered, Chronic }
public enum CrisisProfile { AcuteStress, SomaticFlashback, GuiltInsomnia, ChemicalWithdrawal, IsolationParanoia } public sealed class MentalHealthCrisisSystem { public const string SystemId = "mental_health"; private MentalHealthState _state = new MentalHealthState(); private readonly ISeededRng _rng; private readonly ILog _log; private readonly NeedsSystem _needs; private readonly MedicalWardSystem _medical; private readonly ChemicalDependencySystem _dependency; private readonly DutyRosterSystem _roster; private int _currentDay; public MentalHealthState State => _state; public event Action<CrisisCase> OnCrisisResolved; public event Action OnMentalHealthChanged; public MentalHealthCrisisSystem( ISeededRng rng, NeedsSystem needs, MedicalWardSystem medical, ChemicalDependencySystem dependency, DutyRosterSystem roster, ILog? log = null) { _rng = rng ?? throw new ArgumentNullException(nameof(rng)); _needs = needs ?? throw new ArgumentNullException(nameof(needs)); _medical = medical ?? throw new ArgumentNullException(nameof(medical)); _dependency = dependency ?? throw new ArgumentNullException(nameof(dependency)); _roster = roster ?? throw new ArgumentNullException(nameof(roster)); _log = log ?? NullLog.Instance; }
public ActionResult TriggerCrisis(string survivorId, float stressInput, CrisisProfile profile) {
public ActionResult BeginTreatment(string caseId, string caregiverId, string intervention) {
public const int ChronicThresholdDays = 14;
public void TickDay(int day) {
public bool IsInCrisis(string survivorId) {
public bool IsEligibleForWork(string survivorId) {
public MentalHealthState CaptureState() => CloneState(_state);
public void RestoreState(MentalHealthState saved) {
```


# Appendix Q.571 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs`

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


# Appendix Q.572 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/UI/CrisisPresentationCoordinator.cs`

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


# Appendix Q.573 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Verdict/VerdictQuestCatalogLoader.cs`

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


# Appendix Q.574 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/YearOfAsh/DynamicQuestlineCatalogLoader.cs`

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


# Appendix Q.575 — Additional Current Architecture Evidence: `src/Host/ContentUtilizationRuntimeCollector.cs`

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


# Appendix Q.576 — Additional Current Architecture Evidence: `src/Main.Plans46_49.cs`

### `src/Main.Plans46_49.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 474 lines / 19620 bytes.
- SHA-256: `312d6602edf690061111377cd4798a738c47e6c5c344ed258c88e91167262bb8`.
- Architecture signals: seeded references=4; save/restore symbols=10; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public ShelterWorkshopSystem EnsureShelterWorkshop() {
public ShelterRadioStationSystem EnsureRadioStation() {
public ShelterSocialDynamicsSystem EnsureShelterSocialDynamics() {
public ExcavationHazardSystem EnsureExcavationHazards() {
public DynamicQuestlineSystem EnsureDynamicQuests() {
```


# Appendix Q.577 — Additional Current Architecture Evidence: `src/Host/PanelBindLifecycleSelfTest.cs`

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


# Appendix Q.578 — Additional Current Architecture Evidence: `src/YearOfAsh/YearOfAshHostSession.cs`

### `src/YearOfAsh/YearOfAshHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 345 lines / 16205 bytes.
- SHA-256: `ccefd687193231434f67fa00f2a465d6a8558f7d6c3c7748f94a35fb28e4b6d2`.
- Architecture signals: seeded references=2; save/restore symbols=3; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class YearOfAshHostSession
public YearOfAshTimelineSystem Timeline => _timeline;
public DoorEncounterSystem Encounters => _encounters;
public FactionWarSystem FactionWar => _factionWar;
public QuestlineSystem Quests => _quests;
public YearOfAshDeepFreezeSystem DeepFreeze => _deepFreeze;
public YearOfAshRadonSystem Radon => _radon;
public WarlordDoctrineSystem Warlord => _warlord;
public FactionWarChainRunner WarRunner => _warRunner;
public IReadOnlyList<SurvivorOccupantSnapshot> DemoRoster => _demoRoster;
public void BindWarlord(WarlordDoctrineSystem warlord) {
public static YearOfAshHostSession Create(string dataDir = "", bool loadExistingSave = true) {
public void TickDay(int day) {
public void RecordWarLocationVisited(string locationId) {
public void ResolveWarChoice(string chainId, string stageId, string choiceId, int currentDay) {
public string GetStatusSummary() {
public int CurrentTributeAsk =>
public bool SettleWarlordTribute(int amountPaid, int day, out int nextAsk) {
public string CollectorLine(string state, int day) => _warlord.Catalog.CollectorLine(state, day);
public string WarlordLine() {
public YearOfAshSave CaptureSave() {
public void RestoreSave(YearOfAshSave save) {
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.


# Appendix S — Quality Assurance Pass Record: Plan 106

This record is part of the planning artifact, not a fresh runtime test result.

## Pass 1 — content and premise accuracy
- Content pass replaced the 5→15 premise with the current 15-item dose catalog.
- The historical baseline is separated from the current source/data/test authority.
- Current row counts and owner boundaries are stated without using count as a quality proxy.

## Pass 2 — integration architecture
- Integration pass traced merged item identity → quest/inventory grants → medical boundaries.
- Core, data, host, UI, save, event and test seams are named with current paths.
- The plan does not authorize a parallel save section, catalog, manager or host cache.

## Final precision and reaccuracy pass
- Precision pass prevents catalog descriptions from becoming unmodeled medical effects.
- Every embedded current-file hash, focused runner command and master-authority reference is rechecked.
- Any proposed future seam is labeled as requiring a separate claim and premise verification.
