# Plan 91 — Greenhouse Item Catalog, Crop/Supply Reachability and Production QA

> **Rebuild status:** COMPLETE 34-ROW GREENHOUSE CONTENT AUTHORITY — REACHABILITY AND QA MAINTENANCE
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

- The current file contains 34 unique rows: the original 14 plus 16 planned supplies and additional crop/output rows. The data is not an isolated list; `ItemCatalogLoader` merges it into the global item authority and current tests check no collisions or dead parity copies.
- The live production route is `greenhouse_items.json` → global item/crop catalog → `GreenhouseSystem`/`GreenhouseHostSession` → planting, watering, treatment, nutrient, harvest, apiary and inventory effects → existing greenhouse save.
- The useful quality work is to ensure every supply has a real acquisition/crafting/scavenging or production path, every item type is valid, and the panel does not imply that a supply alone causes a crop outcome.

**Bounded outcome:** Retire the old 14→30 expansion premise. The current greenhouse catalog has 34 rows, is loaded into the global item registry, has current greenhouse crop/recipe/scavenging bindings and focused tests. The rebase preserves the expanded authority and focuses on truthful production/reachability QA.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `greenhouse_items.json` is present with 34 unique items; `GreenhouseItemCatalogTests` asserts the current 34-row reality, original 14, 16 new supplies, global registration, no collisions, four greenhouse recipes and three scavenging bindings.
- `GreenhouseExpansionCatalog`, `GreenhouseSystem` and `GreenhouseHostSession` own crop/supply constants, plot state and inventory transactions; `GreenhousePanel` is the live player surface.
- `GreenhouseCropExpansionTests` covers 13 crops, clean/tainted harvest and save restore; the catalog’s row count is not a production proof by itself.
- The current Plan 22/Round 1 work and current greenhouse system share the same owner; this plan must not create a second greenhouse item authority.

**Master-authority sections applied to this rebase:**

- Master authority Volume 28 verification cookbook: focused evidence before broad gates.
- Lane D save/state/compatibility guidance: owner DTOs, migration and restore proof.
- Lane E UI/UX/accessibility guidance: truthful projections and keyboard/controller lifecycle.
- Lane G testing guidance: smallest affected target, negative cases and deterministic replay.
- Anti-padding protocol: content exhaustion may end the plan before the character checkpoint.
- Volume 32 production/data-authority guidance.

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the 14→30 target with a 34-row current census and acquisition/production matrix.
- Separate supply, seed, crop and output semantics in validation and UI.
- Verify every new row resolves in the merged item catalog and has a current consumer or explicit future-only status.
- Preserve clean/tainted harvest, apiary and greenhouse save semantics.

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
| merged global item registry | ItemCatalogLoader | `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs` | Owns item identity and type resolution across catalogs. |
| crop/supply constants and definitions | GreenhouseExpansionCatalog | `Assets/Ashfall.Core/Greenhouse/GreenhouseExpansionCatalog.cs` | Owns greenhouse-specific content references. |
| plot lifecycle and harvest state | GreenhouseSystem | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` | Owns crop simulation, not item catalog rows. |
| inventory and production commands | GreenhouseHostSession | `src/Host/GreenhouseHostSession.cs` | Routes canonical inventory effects and save. |
| player-facing crop/supply projection | Greenhouse UI | `src/UI/GreenhousePanel.cs` | Presentation only. |
| catalog, production and save proof | Greenhouse focused tests | `Ashfall.Core.Tests/GreenhouseItemCatalogTests.cs; Ashfall.Core.Tests/GreenhouseCropExpansionTests.cs` | Executable current evidence. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Greenhouse Item Catalog, Crop/Supply Reachability and Production QA
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ ItemCatalogLoader
│   merged global item registry
│ GreenhouseExpansionCatalog
│   crop/supply constants and definitions
│ GreenhouseSystem
│   plot lifecycle and harvest state
│ GreenhouseHostSession
│   inventory and production commands
│ Greenhouse UI
│   player-facing crop/supply projection
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

1. **Preserve current state ownership.** ItemCatalogLoader owns merged global item registry: Owns item identity and type resolution across catalogs.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| merged global item registry | ItemCatalogLoader | `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs` | Owns item identity and type resolution across catalogs. |
| crop/supply constants and definitions | GreenhouseExpansionCatalog | `Assets/Ashfall.Core/Greenhouse/GreenhouseExpansionCatalog.cs` | Owns greenhouse-specific content references. |
| plot lifecycle and harvest state | GreenhouseSystem | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` | Owns crop simulation, not item catalog rows. |
| inventory and production commands | GreenhouseHostSession | `src/Host/GreenhouseHostSession.cs` | Routes canonical inventory effects and save. |
| player-facing crop/supply projection | Greenhouse UI | `src/UI/GreenhousePanel.cs` | Presentation only. |
| catalog, production and save proof | Greenhouse focused tests | `Ashfall.Core.Tests/GreenhouseItemCatalogTests.cs; Ashfall.Core.Tests/GreenhouseCropExpansionTests.cs` | Executable current evidence. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. load/merge greenhouse items
2. resolve seed/supply/output identity
3. preview planting/water/treatment/nutrient action
4. consume seed/material through inventory transaction
5. advance plot growth and blight through GreenhouseSystem
6. harvest clean/tainted output into inventory
7. project greenhouse panel and capture current save

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Catalog item definitions are immutable; plot, water, blight, crop and harvest state belong to `GreenhouseSystem`.
- A seed item is consumed through the host/inventory transaction; a crop output is added by the current harvest path.
- Clean and tainted outputs are canonical item outcomes and must not be inferred from supply rows.
- Restore preserves plots, rotation, water, blight, apiary and harvest state without replaying production.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Every greenhouse item ID is unique in the merged catalog and uses a supported type.
- A crop row cannot be planted unless the current crop catalog recognizes its seed ID.
- A supply row does not claim consumable effects unless the current item/catalog contract supports it.
- The same crop, water, blight, seed and day state produces the same harvest result.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- `greenhouse_items.json` is the greenhouse item authority merged by `ItemCatalogLoader`.
- Do not add a second greenhouse-specific item loader or duplicate rows in root items.
- New rows need a current consumer, valid type and bounded stack/weight/value semantics.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Use the existing greenhouse save section/store and preserve apiary state in its envelope.
- No new save section is justified by content changes.
- Legacy plots and missing fields normalize through the current greenhouse restore contract.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Growth, contamination and harvest use the existing seeded/system owner.
- Crop catalog iteration and UI ordering are stable.
- Paired runs and save/restore runs produce identical crop outcomes.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Plant/mature/harvest/fail events are emitted by the greenhouse system.
- Supply and recipe actions are host commands through canonical inventory/crafting owners.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Host/GreenhouseHostSession.cs
- src/Main.ShelterSocial.cs
- src/UI/GreenhousePanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Greenhouse content should communicate practical cultivation, scarcity and trade-offs.
- No supply should be described as a magical fix for a systemic problem.
- The panel’s tone should remain grounded and legible under pressure.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A greenhouse row is not merged into the global item catalog. | ItemCatalogLoader | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A seed ID is displayed but cannot be planted. | GreenhouseExpansionCatalog | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A supply claims an effect the loader ignores. | GreenhouseSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | Harvest adds duplicate or wrong clean/tainted output. | GreenhouseHostSession | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | A panel writes crop state directly. | Greenhouse UI | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/GreenhouseItemCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/GreenhouseCropExpansionTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/GreenhouseSystemTests.cs`

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — current census | Read catalog, merged loader, crop catalog, host and tests. | 34 rows and current owners are proven. | No production path until the owning implementation package is separately claimed. |
| 1 — consumer matrix | Classify each row as seed, supply, crop, output or future-only. | No orphan or duplicate item. | No production path until the owning implementation package is separately claimed. |
| 2 — production/replay proof | Trace plant, water, treat, harvest and restore. | Clean/tainted outcomes and save match. | No production path until the owning implementation package is separately claimed. |
| 3 — UI/content QA | Review labels, availability, accessibility and tone. | Player can understand inputs and consequences. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/StreamingAssets/Data/greenhouse_items.json | READ ONLY; MODIFY only for proven row gap | 34-row authority |
| Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs | READ ONLY | Merged item owner |
| Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs | READ ONLY | Crop state owner |
| src/Host/GreenhouseHostSession.cs | READ ONLY | Inventory/production adapter |
| src/UI/GreenhousePanel.cs | READ ONLY | Current presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Creating a second item authority. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Treating decorative supplies as gameplay effects. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Duplicating crop state in the panel. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Breaking global item ID collision checks. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No new catalog row count for this rebase.
- No new greenhouse system.
- No save-section change.
- No production edits in this rebase.

# 23. Rollback and Recovery

- Revert the plan file.
- Future data/host changes retain prior catalog and focused greenhouse tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- 34 rows and current merged loader are explicit.
- Production and item ownership boundaries are documented.
- Save/replay and UI contracts are named.
- No parallel greenhouse authority is proposed.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the 14→30 target with a 34-row current census and acquisition/production matrix.
- Separate supply, seed, crop and output semantics in validation and UI.
- Verify every new row resolves in the merged item catalog and has a current consumer or explicit future-only status.
- Preserve clean/tainted harvest, apiary and greenhouse save semantics.

## MUST NOT DO

- No new catalog row count for this rebase.
- No new greenhouse system.
- No save-section change.
- No production edits in this rebase.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/GreenhouseItemCatalogTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/GreenhouseCropExpansionTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/GreenhouseSystemTests.cs`

## FIRST SAFE IMPLEMENTATION STEP

0 — current census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: merged global item registry → ItemCatalogLoader; crop/supply constants and definitions → GreenhouseExpansionCatalog; plot lifecycle and harvest state → GreenhouseSystem; inventory and production commands → GreenhouseHostSession; player-facing crop/supply projection → Greenhouse UI; catalog, production and save proof → Greenhouse focused tests. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 91.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 91 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by ItemCatalogLoader or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Greenhouse/GreenhouseExpansionCatalog.cs`

### `Assets/Ashfall.Core/Greenhouse/GreenhouseExpansionCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 346 lines / 15287 bytes.
- SHA-256: `43d1cbbb4a22fde12c8a36f951ea834fd7ac36c1ace32d210069f12708977efc`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class GreenhouseExpansionCatalog
public const string SaveId = "greenhouse";
public static class Items
public const string SeedPacketsMixed = "seed_packets";
public const string SeedMushroom = "item_seed_mushroom";
public const string SeedTuber = "item_seed_tuber";
public const string SeedGrain = "item_seed_grain";
public const string SeedWheat = "item_seed_wheat";
public const string SeedHardyTuber = "item_seed_hardy_tuber";
public const string SeedAshGrain = "item_seed_ash_grain";
public const string SeedBiolumMushroom = "item_seed_biolum_mushroom";
public const string SeedNutrientAlgae = "item_seed_nutrient_algae";
public const string SeedMedicinalHerb = "item_seed_medicinal_herb";
public const string SeedLeafyGreen = "item_seed_leafy_green";
public const string SeedOilseed = "item_seed_oilseed";
public const string SeedColdLegume = "item_seed_cold_legume";
public const string SeedFrostPea = "item_seed_frost_pea";
public const string CropFrostPea = "crop_frost_pea";
public const string SeedGlacierGreens = "item_seed_glacier_greens";
public const string CropGlacierGreens = "crop_glacier_greens";
public const string PlanterBox = "item_planter_box";
public const string GrowLamp = "item_grow_lamp";
public const string LeadGlassPane = "item_lead_glass_pane";
public const string BlightTreatment = "item_blight_treatment";
public const string GrowMedium = "item_grow_medium";
public const string CropMushroom = "crop_mushroom";
public const string CropTuber = "crop_tuber";
public const string CropGrain = "crop_grain";
public const string CropWheat = "crop_wheat";
public const string CropHardyTuber = "crop_hardy_tuber";
public const string CropAshGrain = "crop_ash_grain";
public const string CropBiolumMushroom = "crop_biolum_mushroom";
public const string CropNutrientAlgae = "crop_nutrient_algae";
public const string CropMedicinalHerb = "crop_medicinal_herb";
public const string CropLeafyGreen = "crop_leafy_green";
public const string CropOilseed = "crop_oilseed";
public const string CropColdLegume = "crop_cold_legume";
public const string TaintedFood = "tainted_food";
public static class Locations
public const string GlasshouseRuins = "location_glasshouse_ruins";
public const string SeedVault = "location_seed_vault";
public const string HydroBaronsAquaponics = "location_hydro_barons_aquaponics";
public const string RotFarmersCompostYard = "location_rot_farmers_compost_yard";
public static class Events
public const string FirstSprout = "greenhouse_first_sprout";
public const string BlightOutbreak = "greenhouse_blight_outbreak";
public const string TaintedHarvest = "greenhouse_tainted_harvest";
public const string TheOffering = "greenhouse_the_offering";
public const string DeadGardener = "greenhouse_dead_gardener";
public const string GlassBreaks = "greenhouse_glass_breaks";
public static class Flags
public const string FirstSproutSeen = "flag_greenhouse_first_sprout_seen";
public const string WheatUnlocked = "flag_greenhouse_wheat_unlocked";
public static class Lore
public const string MunicipalFeeding = "lore_greenhouse_municipal_feeding";
public const string SeedVaultPurpose = "lore_greenhouse_seed_vault";
public const string FirstGardener = "lore_greenhouse_first_gardener";
public const string LeadGlassWorks = "lore_greenhouse_lead_glass_works";
public class CropDef
public string SeedItemId;
public string YieldCleanId;
public string YieldTaintedId;
public float GrowthHoursToMature;
public float WaterPerDay;
public float LightHoursPerDay;
public int BaseYield;
public float BlightResistance;
public float ContaminationTolerance;
public bool RequiresUnlock;
public static class CropCatalog
public static readonly CropDef[] All = {
public static CropDef? Get(string seedItemId) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs`

### `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 559 lines / 24040 bytes.
- SHA-256: `b51fcdb9157772ce9ca0a444ab8c3130ba0e5e043b39f68be29461d98a2cf05e`.
- Architecture signals: seeded references=1; save/restore symbols=2; typed event declarations=13; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum GreenhouseStage
public class GreenhousePlotState
public int plotIndex;
public string seedItemId;
public int stage;
public float growth;
public float water;
public float soilContamination;
public float blight;
public int plantedDay;
public float nutrientLevel;
public int sameCropStreak;
public string lastCropId = string.Empty;
public class GreenhouseState
public string saveId = GreenhouseExpansionCatalog.SaveId;
public List<GreenhousePlotState> plots = new List<GreenhousePlotState>();
public bool preWarWheatUnlocked;
public int totalHarvests;
public long blightRollCount;
public ApicultureState? apiculture;
public struct GreenhouseHarvest
public bool success;
public int plotIndex;
public string yieldItemId;
public int amount;
public bool contaminated;
public struct BlightRiskProfile
public int PlotIndex;
public bool PlotExists;
public float BaseChancePerDay;
public float ResistanceFactor;
public float ContaminationPressure;
public float DroughtStress;
public float NutrientReduction;
public float RotationPressure;
public int RotationStreak;
public float NutrientLevel;
public float FinalChancePerDay;
public class GreenhouseSystem
public const float MaxWater = 100f;
public const float MaxContamination = 100f;
public const float GrowingThreshold = 33f;
public const float DroughtBlightRatePerDay = 0.25f;
public const float OutbreakBlightStep = 0.3f;
public const float BaseBlightChancePerDay = 0.06f;
public const float TaintedWaterContaminationPerUnit = 1.5f;
public const float ResidualContaminationAfterHarvest = 0.5f;
public const string NutrientItemId = "item_hydroponic_nutrients";
public const float NutrientApplicationLevel = 0.5f;
public const float NutrientDecayPerDay = 0.1f;
public const float NutrientBlightRiskReduction = 0.04f;
public const float NutrientFullBandLevel = 0.5f;
public const float RotationBlightStepPerStreak = 0.015f;
public const int MaxRotationStreakCount = 10;
public string SaveId => _state.saveId;
public GreenhouseState CaptureState() {
public void RestoreState(GreenhouseState gs) {
public event Action<int, string, int> OnCropPlanted;
public event Action<int, string> OnCropMatured;
public event Action<GreenhouseHarvest> OnCropHarvested;
public event Action<int> OnBlightOutbreak;
public event Action<int> OnPlotDriedOut;
public event Action<int> OnCropFailed;
public GreenhouseState State => _state;
public int PlotCount => _state.plots.Count;
public int TotalHarvests => _state.totalHarvests;
public bool IsPreWarWheatUnlocked => _state.preWarWheatUnlocked;
public IReadOnlyList<GreenhousePlotState> Plots => _state.plots;
public void EnsurePlots(int planterBoxCount) {
public static bool IsFallow(GreenhousePlotState p) =>
public bool Plant(int plotIndex, string seedItemId, int currentDay, out string consumedSeedId) {
public bool Water(int plotIndex, float waterUnits, bool tainted) {
public bool ApplyNutrients(int plotIndex, out string consumedItemId) {
public BlightRiskProfile GetBlightRiskProfile(int plotIndex, bool hasWater) {
public GreenhouseHarvest Harvest(int plotIndex) {
public bool Clear(int plotIndex) {
public CommandPreview PreviewTreatBlight(int plotIndex, long stateVersion = 0) {
public CommandResult ExecuteTreatBlight(int plotIndex, long expectedStateVersion = 0, long currentStateVersion = 0) {
public bool TreatBlight(int plotIndex, out string consumedTreatmentId) {
public void SurgeContamination(float amount) {
public void UnlockPreWarWheat() {
public void TickDay(int currentDay, float growLightHours, float ashContaminationRate) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs`

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


# Appendix B.05 — Current Code Architecture: `src/Host/GreenhouseHostSession.cs`

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


# Appendix B.06 — Current Code Architecture: `src/Main.ShelterSocial.cs`

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


# Appendix B.07 — Current Code Architecture: `src/UI/GreenhousePanel.cs`

### `src/UI/GreenhousePanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 805 lines / 43610 bytes.
- SHA-256: `7040021a886005a8e2200d671a7f54fa3cc569738e3e62e5418ed355ec418db4`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=17; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class GreenhousePanel : Control
public event Action? OnClose;
public event Action<int>? OnPlotSelected;
public bool IsBound => _host != null;
public void Bind(GreenhouseHostSession session) {
public override void _Ready() {
public void RefreshView() {
public event Action<string, int>? OnActionRequested;
internal static List<AshfallDataGrid.Row> BuildFixtureRows() {
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix C.08 — Catalog Census: `Assets/StreamingAssets/Data/greenhouse_items.json`

### `Assets/StreamingAssets/Data/greenhouse_items.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 11933 bytes / 11921 characters.
- SHA-256: `c677db12b90d1e805cbaa785b20557fb90320a4f685a683b23a2a073672c644a`.
- Root keys: `items`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
items: min=34, max=34, observed_paths=1
```

Representative record fields:

- `contamination`
- `description`
- `displayName`
- `durability`
- `empShielded`
- `healthEffect`
- `hungerRestore`
- `id`
- `moraleEffect`
- `stackMax`
- `thirstRestore`
- `tradeValue`
- `type`
- `weight`

Representative identifiers (ordered, capped for readability):

```text
item_seed_mushroom
item_seed_tuber
item_seed_frost_pea
crop_frost_pea
item_seed_glacier_greens
crop_glacier_greens
item_seed_grain
item_seed_wheat
item_planter_box
item_grow_lamp
item_lead_glass_pane
item_blight_treatment
item_grow_medium
crop_mushroom
crop_tuber
crop_grain
crop_wheat
tainted_food
item_greenhouse_trowel
item_greenhouse_pruning_shears
item_greenhouse_watering_can
item_greenhouse_hand_cultivator
item_greenhouse_compost
item_greenhouse_ash_fertilizer
item_greenhouse_fish_emulsion
item_greenhouse_insecticidal_soap
item_greenhouse_sticky_traps
item_greenhouse_pest_mesh
item_greenhouse_drip_kit
item_greenhouse_line_filter
item_greenhouse_catchment_kit
item_greenhouse_glass_pane
item_greenhouse_uv_sheeting
item_greenhouse_shade_cloth
```


# Appendix C.09 — Catalog Census: `Assets/StreamingAssets/Data/recipes.json`

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


# Appendix C.10 — Catalog Census: `Assets/StreamingAssets/Data/scavenging_tables.json`

### `Assets/StreamingAssets/Data/scavenging_tables.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 146802 bytes / 146778 characters.
- SHA-256: `e8362d3a73c64ed1000cdd3e5ae9200a955c6fd93f9183b91362ea0cbaec3e41`.
- Root keys: `collection_id`, `schema_version`, `tables`.

Array-path census (minimum, maximum, observed rows):

```text
tables: min=54, max=54, observed_paths=1
tables[].entries: min=13, max=19, observed_paths=2
```

Representative record fields:

- `base_hazard_chance`
- `depletion_model`
- `description`
- `display_name`
- `entries`
- `id`
- `location_type`
- `primary_hazard_type`

Representative identifiers (ordered, capped for readability):

```text
table_loot_hospital
table_loot_rail_yard
table_loot_school
table_loot_military_depot
table_loot_apartment_block
table_loot_fire_station
table_loot_metro_station
table_loot_police_station
table_loot_industrial_district
table_loot_shopping_center
table_loot_power_substation
table_loot_chemical_plant
table_loot_warehouse
table_loot_farm
table_loot_forestry_compound
table_loot_hunting_cabin
table_loot_monastery
table_loot_clinic
table_loot_observatory
table_loot_greenhouse
table_loot_veterinary_surgery
table_loot_dentists_row
table_loot_hospice_ward
table_loot_collapsed_structure
table_loot_weighbridge
table_loot_recovery_yard
table_loot_tank_farm
table_loot_concert_hall
table_loot_ration_plaza
table_loot_checkpoint
table_loot_conscription_office
table_loot_ordnance_shoulder
table_loot_relay_mast
table_loot_transit_depot
table_loot_geo_thermal_plant
table_loot_arcology_sector_4
table_loot_tinkers_notch
table_loot_waterworks
table_loot_swimming_baths
table_loot_apiary_rows
table_loot_convoy_cache
table_loot_municipal_archive
table_loot_printworks
table_loot_ministry_bunker
table_loot_government_bunker
table_loot_dead_hand_core
table_loot_shallows_market
table_loot_pilgrim_hearth
table_loot_brine_pans
table_loot_weather_station
table_loot_geological_survey
table_loot_forest_edge
table_loot_frozen_wetland
table_loot_burned_woodland
```


# Appendix D.11 — Existing Focused Test Inventory: `Ashfall.Core.Tests/GreenhouseItemCatalogTests.cs`

### `Ashfall.Core.Tests/GreenhouseItemCatalogTests.cs`

- Current test declarations: Fact=21, Theory=0, InlineData=0.
- File lines: 395; SHA-256: `8d0455261b65808c3cd8fc08c876df318761924dd0e99ccb67581bbf7881ee96`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
GreenhouseFile_ContainsExactlyThirtyEntries
GreenhouseFile_PreservesOriginalFourteen
GreenhouseFile_ContainsAllSixteenNewSupplies
GreenhouseFile_HasUniqueIds
GreenhouseFile_AllTypesAreValidItemTypeValues
GreenhouseFile_NamesAndDescriptionsNonEmpty
GreenhouseFile_NumericRangesValid
GreenhouseFile_HandToolsHaveLowStacksAndLowWeight
GreenhouseFile_NewSuppliesAreNotSameValueClones
GlobalCatalog_RegistersAllThirtyGreenhouseEntries
GlobalCatalog_NewSuppliesResolveAcrossCategories
GlobalCatalog_NoIdCollisionsAcrossItemFiles
GreenhouseFile_DeadParityCopiesRemoved
GreenhouseFile_NewSuppliesClaimNoConsumableEffectFields
Crafting_FourGreenhouseRecipesExistAndAreUnique
Crafting_GreenhouseRecipeOutputsResolveInGlobalRegistry
Crafting_GreenhouseRecipeIngredientsResolve
Crafting_GreenhouseOutputsNotPricedBelowInputValue
Scavenging_GreenhouseTableBindsThreePlan91Items
Scavenging_BoundItemIdsResolveInGlobalRegistry
Scavenging_BoundEntriesUseSaneWeightsAndRarity
```


# Appendix D.12 — Existing Focused Test Inventory: `Ashfall.Core.Tests/GreenhouseCropExpansionTests.cs`

### `Ashfall.Core.Tests/GreenhouseCropExpansionTests.cs`

- Current test declarations: Fact=6, Theory=0, InlineData=0.
- File lines: 181; SHA-256: `74fe8e15894c195f4a50093b67162fa16c5d6afdc8adc79e529f5300a15713ec`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CropCatalog_ContainsAll13Crops
CropCatalog_MixedSeedPacket_IsPlantable_AndCanonical
CropCatalog_ResolvesSeedToCleanYield
GreenhouseSystem_SimulatesFrostTuberLifecycle
GreenhouseSystem_SimulatesMedicinalHerbAndLeafyGreen
GreenhouseSystem_SaveRestore_PreservesExpandedCropState
```


# Appendix D.13 — Existing Focused Test Inventory: `Ashfall.Core.Tests/GreenhouseSystemTests.cs`

### `Ashfall.Core.Tests/GreenhouseSystemTests.cs`

- Current test declarations: Fact=9, Theory=0, InlineData=0.
- File lines: 172; SHA-256: `cd6560460c6991afcefcd421e1643a6b5841181c3442280e7902f268d4ebac8e`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Plant_Requires_FallowPlot_AndKnownSeed
WellTendedCrop_Matures_AndHarvests_Clean
TaintedIrrigation_ContaminatesHarvest
Drought_StallsGrowth_AndFiresDriedOut
PreWarWheat_RequiresUnlock
SaveState_RoundTrips_Losslessly
HeadlessDemo_PassesAllChecks
Probe_SameSeedSameSequence_TwoInstances
Probe_RestoreContinuesBlightStream
```


# Appendix E.14 — Supporting Code Evidence: `Assets/Ashfall.Core/Expeditions/ScavengingTableCatalog.cs`

### `Assets/Ashfall.Core/Expeditions/ScavengingTableCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 208 lines / 8514 bytes.
- SHA-256: `7209d613e76a35b35037de1165b265e181cd8ea53186faa5543d864d38f5e45d`.
- Architecture signals: seeded references=2; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ScavengingTableCatalogContainer
public int schema_version { get; set; } = 1;
public string collection_id { get; set; } = "scavenging_tables_catalog";
public List<ScavengingTableDef> tables { get; set; } = new List<ScavengingTableDef>();
public sealed class ScavengingTableDef
public string id { get; set; } = string.Empty;
public string location_type { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string description { get; set; } = string.Empty;
public string depletion_model { get; set; } = "finite"; // finite, renewable, one_time, slow_regeneration
public float base_hazard_chance { get; set; } = 0.0f;
public string primary_hazard_type { get; set; } = string.Empty;
public List<ScavengingLootEntryDef> entries { get; set; } = new List<ScavengingLootEntryDef>();
public int TotalWeight => entries?.Sum(e => Math.Max(0, e.weight)) ?? 0;
public sealed class ScavengingLootEntryDef
public string item_id { get; set; } = string.Empty;
public int weight { get; set; } = 10;
public int min_quantity { get; set; } = 1;
public int max_quantity { get; set; } = 1;
public string rarity_tier { get; set; } = "common"; // common, uncommon, rare, unique
public float hazard_chance { get; set; } = 0.0f;
public string hazard_type { get; set; } = string.Empty;
public string codex_unlock_id { get; set; } = string.Empty;
public string map_fragment_id { get; set; } = string.Empty;
public sealed class ScavengingRollResult
public string TableId { get; set; } = string.Empty;
public string ItemId { get; set; } = string.Empty;
public int Quantity { get; set; } = 1;
public string RarityTier { get; set; } = "common";
public bool HazardTriggered { get; set; }
public string HazardType { get; set; } = string.Empty;
public string CodexUnlockId { get; set; } = string.Empty;
public string MapFragmentId { get; set; } = string.Empty;
public sealed class ScavengingTableCatalog
public const string DefaultFileName = "scavenging_tables.json";
public IReadOnlyList<ScavengingTableDef> Tables => _tables;
public int TableCount => _tables.Count;
public bool TryGetTable(string tableId, out ScavengingTableDef table) {
public bool TryGetTableByLocationType(string locationType, out ScavengingTableDef table) {
public ScavengingRollResult? RollLoot(string tableId, ISeededRng rng, Func<string, bool>? itemFilter = null) {
public static ScavengingTableCatalog LoadFromDirectory(string dataDirectory, IFileIO fileIO, IJsonSerializer? jsonSerializer = null) {
public static ScavengingTableCatalog LoadFromJson(string json, IJsonSerializer? jsonSerializer = null) {
```


# Appendix E.15 — Supporting Code Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

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


# Appendix E.16 — Supporting Code Evidence: `Assets/Ashfall.Core/Crafting/CraftingSystem.cs`

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


# Appendix E.17 — Supporting Code Evidence: `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs`

### `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 459 lines / 20381 bytes.
- SHA-256: `2f468b2270d22a78d1725fea0b5278b8ce7124315c99ced7229104c8c3f3a555`.
- Architecture signals: seeded references=1; save/restore symbols=2; typed event declarations=18; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class HiveState
public string hiveId = string.Empty;
public string greenhouseBayId = string.Empty;
public float queenVitality = 1.0f;     // 0..1
public float colonyPopulation = 1.0f;  // 0..1, normalized
public float temperatureC = 20f;       // ambient temperature
public float humidityPct = 50f;        // 0..100
public float contamination = 0f;       // 0..1
public float radiationStress = 0f;     // 0..1
public float feedLevel = 1.0f;         // 0..1, sugar water
public float waterLevel = 1.0f;        // 0..1
public float honeyBuffer = 0f;         // kg accumulated
public float waxBuffer = 0f;           // kg accumulated
public bool isSwarming = false;
public bool isDead = false;
public int lastInspectionDay = -1;
public int installedDay = -1;
public List<string> linkedPlotIds = new List<string>();
public class ApicultureState
public string systemId = ApicultureSystem.SystemId;
public List<HiveState> hives = new List<HiveState>();
public float totalHoneyProduced = 0f;
public float totalWaxProduced = 0f;
public class ApicultureSystem
public const string SystemId = "apiculture_system";
public const float OptimalTemperatureMin = 15f;
public const float OptimalTemperatureMax = 30f;
public const float OptimalHumidityMin = 40f;
public const float OptimalHumidityMax = 70f;
public const float FeedConsumptionPerDay = 0.02f;
public const float WaterConsumptionPerDay = 0.03f;
public const float QueenAgingRate = 0.001f;      // per day
public const float PopulationGrowthRate = 0.01f;  // per day when healthy
public const float PopulationDeclineRate = 0.03f; // per day when stressed
public const float SwarmThreshold = 0.9f;         // population above this triggers swarm risk
public const float SwarmChance = 0.05f;           // per day when above threshold
public const float DeathThreshold = 0.05f;        // population below this = colony death
public const float HoneyPerDayPerPop = 0.01f;     // kg per population unit per day
public const float WaxPerDayPerPop = 0.005f;      // kg per population unit per day
public const float MaxHoneyBuffer = 5f;            // kg
public const float MaxWaxBuffer = 2f;              // kg
public const float MaxPollinationBonus = 0.25f;    // 25% max yield increase
public const float PollinationPerPopulation = 0.3f; // pollination strength per population unit
public const float ContaminationStressThreshold = 0.2f;
public const float RadiationStressThreshold = 0.3f;
public event Action<string> OnHiveInstalled;           // hiveId
public event Action<string> OnInspectionCompleted;     // hiveId
public event Action<string, float> OnPollinationChanged; // hiveId, strength
public event Action<string, float, float> OnProductionTick; // hiveId, honey, wax
public event Action<string> OnColonyStressed;          // hiveId
public event Action<string> OnColonySwarming;          // hiveId
public event Action<string> OnColonyDied;              // hiveId
public event Action<string> OnMedicalProcessingCompleted; // hiveId
public event Action<ApicultureState> OnStateChanged;
public ApicultureState State => _state;
public IReadOnlyDictionary<string, HiveState> Hives => _hives;
public bool InstallHive(string hiveId, string bayId, int day) {
public bool LinkPlots(string hiveId, List<string> plotIds) {
public void TickDaily(int day, float greenhouseTemperatureC, float greenhouseContamination, float radiationLevel, ISeededRng rng) {
public float GetPollinationBonus(string plotId) {
public float GetHivePollinationStrength(string hiveId) {
public HiveState? InspectHive(string hiveId, int day) {
public bool RefillFeed(string hiveId, float amount = 1f) {
public bool RefillWater(string hiveId, float amount = 1f) {
public bool ReplaceQueen(string hiveId) {
public bool ProcessMedicalSupplies(string hiveId) {
public HiveState? GetHive(string hiveId) {
public int GetAliveHiveCount() {
public ApicultureState CaptureState() {
public void RestoreState(ApicultureState saved) {
```


# Appendix E.18 — Supporting Code Evidence: `Assets/Ashfall.Core/Farming/AgricultureSystem.cs`

### `Assets/Ashfall.Core/Farming/AgricultureSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 904 lines / 40109 bytes.
- SHA-256: `4d405ead4542d1f14c8b388b2e8d12c94587c8b5761e07176dad3fa293b486c8`.
- Architecture signals: seeded references=5; save/restore symbols=2; typed event declarations=18; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum AgriWaterBand
public enum AgriMutationOutcome
public struct AgricultureEnvironmentSnapshot
public float TemperaturePenaltyC;
public float OutdoorRadModifier;
public float LightingAvailabilityPermille;
public float AshContaminationRate;
public string SeasonWindowId;
public static AgricultureEnvironmentSnapshot Default() => new AgricultureEnvironmentSnapshot
public class AgriPlotState
public int plot_index;
public string strain_id = "";
public float medium_quality = 100f;
public int toxicity_permille;
public float pest_severity;
public string pest_id = "";
public int mutation_outcome;
public string mutation_strain_id = "";
public int planted_day;
public class CompostBatchState
public string recipe_id = "";
public int started_day;
public bool collected;
public class AgricultureState
public string system_id = "agriculture";
public int schema_version = 1;
public List<AgriPlotState> plots = new List<AgriPlotState>();
public List<CompostBatchState> compost_batches = new List<CompostBatchState>();
public bool first_harvest_narrative_fired;
public List<int> blight_narrative_plots = new List<int>();
public List<string> unlocked_strains = new List<string>();
public int last_tick_day;
public struct YieldBreakdown
public int BaseYield;
public float StrainModifier;
public float WaterModifier;
public float LightModifier;
public float ToxicityModifier;
public float PestModifier;
public float MutationModifier;
public float MediumModifier;
public float FinalMultiplier;
public int FinalYield;
public string QualityTier;
public bool Contaminated;
public struct AgricultureHarvest
public bool success;
public int plotIndex;
public string yieldItemId;
public int baseAmount;
public int finalAmount;
public bool contaminated;
public string qualityTier;
public YieldBreakdown Breakdown;
public NutritionProfileDef Nutrition;
public sealed class AgricultureSystem
public const float MediumDecayPerPlantedDay = 0.5f;
public const float MediumDecayPerFallowDay = 0.1f;
public const float ToxicityNaturalDecayPerDay = 2f;
public const float CompostMediumBoost = 40f;
public const int CompostToxicityReduction = 200;
public const float BaseGrowLightHours = 6f;
public const float WaterUnitsPerWatering = 50f;
public static readonly int[] ToxicityPerUnitByBand = { 0, 2, 8, 15 };
public static readonly string[] DeepWinterWindowIds = { "window_deep_freeze", "window_long_winter" };
public const int DeepWinterLightPermille = 600;
public static int WinterAdjustedLightPermille(int poweredLightPermille, string seasonWindowId, bool hasMicroclimateCapability, bool greenhouseRoomPowered) {
public string SystemId => _state.system_id;
public AgricultureState State => _state;
public GreenhouseSystem Greenhouse => _greenhouse;
public CropStrainCatalogContainer Catalog => _catalog;
public int LastTickDay => _state.last_tick_day;
public void LoadCatalog(CropStrainCatalogContainer catalog) {
public event Action<int, string, int> OnStrainPlanted;
public event Action<int, string> OnPlotInfested;
public event Action<int, string> OnPestTreated;
public event Action<int, AgriMutationOutcome, string> OnMutationDetermined;
public event Action<AgricultureHarvest> OnHarvest;
public event Action<string, int> OnCompostStarted;
public event Action<string, int> OnCompostCollected;
public event Action<int> OnFirstHarvest;
public event Action<int> OnBlightNarrative;
public AgricultureState CaptureState() {
public void RestoreState(AgricultureState state) {
public CropStrainDef Strain(string strainId) {
public CropStrainDef EffectiveStrain(int plotIndex) {
public PestDef Pest(string pestId) {
public CompostRecipeDef CompostRecipe(string recipeId) {
public bool CanPlantStrain(int plotIndex, string strainId) {
public bool PlantWithStrain(int plotIndex, string strainId, int currentDay) {
public bool ClearPlot(int plotIndex) {
public void Water(int plotIndex, AgriWaterBand band) {
public bool CanTreatPest(int plotIndex, string treatmentItemId) {
public bool TryTreatPestInfestation(int plotIndex, string treatmentItemId) {
public bool CanStartCompost(string recipeId) {
public bool TryStartCompostBatch(string recipeId, int currentDay) {
public bool IsCompostReady(string recipeId, int currentDay) {
public int TryCollectCompost(string recipeId, int currentDay) {
public bool TryApplyCompost(int plotIndex) {
public struct YieldInputs
public int BaseYield;
public float StrainModifier;
public float LightFactor;          // 0..1 grow-light availability
public float WaterBandFactor;      // 1 clean .. 0.55 unsafe
public int ToxicityPermille;
public int ToxicityTolerancePermille;
public float PestSeverity;
public float PestYieldDamageAtFull;
public int MutationOutcome;
public float MediumQuality;
public bool SoilContaminated;
public static YieldBreakdown CalculateYield(in YieldInputs inputs) {
public YieldBreakdown ForecastYield(int plotIndex, in AgricultureEnvironmentSnapshot env) {
public AgricultureHarvest Harvest(int plotIndex) {
public bool IsStrainUnlocked(string strainId) => _state.unlocked_strains.Contains(strainId);
public void UnlockStrain(string strainId) {
public void TickDay( int day, in AgricultureEnvironmentSnapshot env, ISeededRng pestRng, ISeededRng mutationRng, string seasonWindowId = "any")
public static AgriMutationOutcome ParseOutcome(string outcome) => outcome switch
public void NotifyBlightOutbreak(int plotIndex) {
public void CloseBlightNarrative(int plotIndex) {
```


# Appendix G.19 — Supporting Regression Evidence: `Ashfall.Core.Tests/Greenhouse/GreenhousePhase4LoopClosureTests.cs`

### `Ashfall.Core.Tests/Greenhouse/GreenhousePhase4LoopClosureTests.cs`

- Current test declarations: Fact=17, Theory=0, InlineData=0.
- File lines: 298; SHA-256: `daf2728f9b4af16d9d72001e686cf1c181c50d6fabcd9598eb26f2d1b6277937`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ApplyNutrients_RaisesBand_TowardSaturation
ApplyNutrients_Blocked_OnFallowAndFailedPlots
Nutrients_LowerBlightRisk_NeverNegative
NutrientBand_DecaysDaily_DosingIsRecurring
BlightRiskProfile_IsReadOnly_ConsumingNoRng
BlightRiskProfile_FinalChance_MatchesLegacyFormula
WinterLight_NonWinter_PassesThroughUnchanged
WinterLight_Powered_WithoutCapability_IsPenalized
WinterLight_WithCapability_AndPower_IsFullyCompensated
WinterLight_WithoutPower_IsZero_RegardlessOfCapability
NutrientLevel_SaveRoundTrips
LegacySave_WithoutNutrientLevel_RestoresZero
SaturatedNutrients_FiveDayCycle_BlightChanceClampsToZero
CropRotation_SameCropStreak_RaisesRisk_OtherCropResets
CropRotation_Pressure_IsVisible_InRiskProfile_AndClamped
CropRotation_StreakSurvivesHarvest_ButNotDifferentCrop
CropRotation_SaveRoundTrips_AndLegacyRestoresZero
```


# Appendix G.20 — Supporting Regression Evidence: `Ashfall.Core.Tests/CraftingSystemTests.cs`

### `Ashfall.Core.Tests/CraftingSystemTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 251; SHA-256: `7adf7b670437317b141dd5191d2b941477a472741cb6e35931f897f702ef8b20`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
CanCraft_TrueWhenAllIngredientsHeld
BindCraftResultGate_RefusesUnknownResult_AllowsCataloguedResult
CanCraft_False_WhenStationMissingOrBroken
StartCraft_ConsumesIngredients_CompletesAfterTick
StartCraft_Fails_WhenIngredientsInsufficient
FullInventory_StartRejected_NoIngredientsConsumed
OverflowPath_WithFreedSlot_RefundsWhenNoStash
OverflowPath_WithStash_StashesResultAndRefundsNothing
SaveRoundtrip_PreservesActiveCrafts
Station_DegradesPerCraft_AndCanRepair
```


# Appendix G.21 — Supporting Regression Evidence: `Ashfall.Core.Tests/AgricultureSystemTests.cs`

### `Ashfall.Core.Tests/AgricultureSystemTests.cs`

- Current test declarations: Fact=23, Theory=0, InlineData=0.
- File lines: 601; SHA-256: `a112b69d340427be117411ed65b9bbde0763e5a98be3ff34b3d51f3409f4efdd`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ShippedCatalog_ValidatesWithZeroDiagnostics
ShippedCatalog_SeedItemsResolveInCanonicalCropCatalog
CatalogValidation_RejectsBadDefinitions
CatalogValidation_RejectsCompostValueLoop
Growth_IsDeterministicForSameSeedAndEnvironment
Growth_EndOfDayPhaseTransitionsAreExact
Power_LightingOutageHaltsGrowthAndCutsYield
Water_BandToxicityAccumulatesOncePerWatering
Toxicity_ReducesYieldWithinBounds
Yield_NeverNegativeNaNOrRunaway
Mutation_SameSeedSameStateSameOutcome
Mutation_IneligibleConditionsConsumeNoMutationRng
Mutation_ToxicHarvestMarksHarvestContaminated
Mutation_HardyStrainResolvesVariantAndUnlocksOnHarvest
Pest_ProgressionIsDeterministicAndSeverityStepsWithoutRng
Pest_TreatmentValidatesItemThenClears
Compost_LifecycleStartsWaitsAndCollectsOnce
Compost_ApplicationImprovesMediumAndReducesToxicity
FirstHarvest_NarrativeFiresExactlyOnce
BlightNarrative_FiresOncePerEpisode
TickDay_IsIdempotentPerDay
Nutrition_MonotonousDietBecomesDeficientAndDiversityClears
Nutrition_UnmappedFoodIsCaloriesOnly
```


# Appendix G.22 — Supporting Regression Evidence: `Ashfall.Core.Tests/CraftingAfflictionLoopTests.cs`

### `Ashfall.Core.Tests/CraftingAfflictionLoopTests.cs`

- Current test declarations: Fact=20, Theory=0, InlineData=0.
- File lines: 472; SHA-256: `75be59b0227945d42a2e1ec6d090c4b1b18ae38fc4cc472cafac55d5e09ab0e4`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
StartCraft_ConsumesIngredientsAtomically
StartCraft_WhenInsufficientIngredients_ConsumesNothing
StartCraft_WhenNullRecipe_ReturnsFalse
Tick_CompletesQueuedCraft_ExactlyOnce
Tick_DoesNotCompleteBeforeDuration
MultipleRecipes_QueueBothAndCompleteInOrder
RestoreState_ReconstitutesQueueWithHoursRemaining
RestoreState_ThenTickToCompletion_DoesNotDuplicate
RestoreState_WithNullSave_ClearsQueue
CraftingSystem_Station_BlocksCraftWhenInoperational
ApplyInhaler_ReducesDegradationAndGivesRelief
ApplyInhaler_OnHealthySurvivor_ReturnsFalse
ApplyHerbalTea_ReducesMildDegradation
ApplyHerbalTea_OnHealthySurvivor_ReturnsFalse
SevereCoughThreshold_ReducesStaminaMultiplier
InhalerRelief_SuppressesStaminaPenalty
CaptureRestoreRoundTrip_PreservesAllRespiratoryFields
RestoreRespiratoryState_WithNullSave_ClearsSurvivors
OnStateChanged_FiresAfterInhaler
GetStaminaMultiplier_RestoredSurvivor_MatchesOriginal
```


# Appendix H.23 — Supporting Authority Document: `docs/CURRENT_AUTHORITY.md`

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
| merged global item registry | ItemCatalogLoader | crop/supply constants and definitions | GreenhouseExpansionCatalog | Owner emits/reads a typed fact; no mirror state. |
| merged global item registry | ItemCatalogLoader | plot lifecycle and harvest state | GreenhouseSystem | Owner emits/reads a typed fact; no mirror state. |
| merged global item registry | ItemCatalogLoader | inventory and production commands | GreenhouseHostSession | Owner emits/reads a typed fact; no mirror state. |
| merged global item registry | ItemCatalogLoader | player-facing crop/supply projection | Greenhouse UI | Owner emits/reads a typed fact; no mirror state. |
| merged global item registry | ItemCatalogLoader | catalog, production and save proof | Greenhouse focused tests | Owner emits/reads a typed fact; no mirror state. |
| crop/supply constants and definitions | GreenhouseExpansionCatalog | merged global item registry | ItemCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| crop/supply constants and definitions | GreenhouseExpansionCatalog | plot lifecycle and harvest state | GreenhouseSystem | Owner emits/reads a typed fact; no mirror state. |
| crop/supply constants and definitions | GreenhouseExpansionCatalog | inventory and production commands | GreenhouseHostSession | Owner emits/reads a typed fact; no mirror state. |
| crop/supply constants and definitions | GreenhouseExpansionCatalog | player-facing crop/supply projection | Greenhouse UI | Owner emits/reads a typed fact; no mirror state. |
| crop/supply constants and definitions | GreenhouseExpansionCatalog | catalog, production and save proof | Greenhouse focused tests | Owner emits/reads a typed fact; no mirror state. |
| plot lifecycle and harvest state | GreenhouseSystem | merged global item registry | ItemCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| plot lifecycle and harvest state | GreenhouseSystem | crop/supply constants and definitions | GreenhouseExpansionCatalog | Owner emits/reads a typed fact; no mirror state. |
| plot lifecycle and harvest state | GreenhouseSystem | inventory and production commands | GreenhouseHostSession | Owner emits/reads a typed fact; no mirror state. |
| plot lifecycle and harvest state | GreenhouseSystem | player-facing crop/supply projection | Greenhouse UI | Owner emits/reads a typed fact; no mirror state. |
| plot lifecycle and harvest state | GreenhouseSystem | catalog, production and save proof | Greenhouse focused tests | Owner emits/reads a typed fact; no mirror state. |
| inventory and production commands | GreenhouseHostSession | merged global item registry | ItemCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| inventory and production commands | GreenhouseHostSession | crop/supply constants and definitions | GreenhouseExpansionCatalog | Owner emits/reads a typed fact; no mirror state. |
| inventory and production commands | GreenhouseHostSession | plot lifecycle and harvest state | GreenhouseSystem | Owner emits/reads a typed fact; no mirror state. |
| inventory and production commands | GreenhouseHostSession | player-facing crop/supply projection | Greenhouse UI | Owner emits/reads a typed fact; no mirror state. |
| inventory and production commands | GreenhouseHostSession | catalog, production and save proof | Greenhouse focused tests | Owner emits/reads a typed fact; no mirror state. |
| player-facing crop/supply projection | Greenhouse UI | merged global item registry | ItemCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| player-facing crop/supply projection | Greenhouse UI | crop/supply constants and definitions | GreenhouseExpansionCatalog | Owner emits/reads a typed fact; no mirror state. |
| player-facing crop/supply projection | Greenhouse UI | plot lifecycle and harvest state | GreenhouseSystem | Owner emits/reads a typed fact; no mirror state. |
| player-facing crop/supply projection | Greenhouse UI | inventory and production commands | GreenhouseHostSession | Owner emits/reads a typed fact; no mirror state. |
| player-facing crop/supply projection | Greenhouse UI | catalog, production and save proof | Greenhouse focused tests | Owner emits/reads a typed fact; no mirror state. |
| catalog, production and save proof | Greenhouse focused tests | merged global item registry | ItemCatalogLoader | Owner emits/reads a typed fact; no mirror state. |
| catalog, production and save proof | Greenhouse focused tests | crop/supply constants and definitions | GreenhouseExpansionCatalog | Owner emits/reads a typed fact; no mirror state. |
| catalog, production and save proof | Greenhouse focused tests | plot lifecycle and harvest state | GreenhouseSystem | Owner emits/reads a typed fact; no mirror state. |
| catalog, production and save proof | Greenhouse focused tests | inventory and production commands | GreenhouseHostSession | Owner emits/reads a typed fact; no mirror state. |
| catalog, production and save proof | Greenhouse focused tests | player-facing crop/supply projection | Greenhouse UI | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the 14→30 target with a 34-row current census and acquisition/production matrix. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Separate supply, seed, crop and output semantics in validation and UI. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Verify every new row resolves in the merged item catalog and has a current consumer or explicit future-only status. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Preserve clean/tainted harvest, apiary and greenhouse save semantics. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix Q.556 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Balance/ResourceMassBalanceSimulator.cs`

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


# Appendix Q.557 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Greenhouse/GreenhouseHeadlessDemo.cs`

### `Assets/Ashfall.Core/Greenhouse/GreenhouseHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 109 lines / 4835 bytes.
- SHA-256: `dd194b237bdf55ac6c5f1bf8a81492f9e66bf5c08a1b5b83c1499f0eecdf0518`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class GreenhouseHeadlessReport : HeadlessReport
public int Harvests;
public int Blights;
public static class GreenhouseHeadlessDemo
public static GreenhouseHeadlessReport Run(ILog? log = null) {
```


# Appendix Q.558 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs`

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


# Appendix Q.559 — Additional Current Architecture Evidence: `src/Host/HostCli.PanelTests.cs`

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


# Appendix Q.560 — Additional Current Architecture Evidence: `src/Host/CraftingSaveStore.cs`

### `src/Host/CraftingSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 51 lines / 2532 bytes.
- SHA-256: `4e19861824873163ab9dcf804ad90d866bd3e52b5e766b23a30c0a40ef9892cd`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class CraftingSaveStore
public const string FileName = "crafting_save.json";
public const string SectionName = "crafting";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static bool TrySave(CraftingSystemSave state) => s_store.TrySave(state);
public static CraftingSystemSave? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(CraftingSystemSave state) => s_store.CapturePersisted(state);
public static string TryCaptureDirect(CraftingSystemSave state) => s_store.CaptureBare(state);
public static CraftingSystemSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(CraftingSystemSave state) => s_store.CaptureBare(state);
public static CraftingSystemSave? TryRestore(string json) => s_store.RestoreBare(json);
```


# Appendix Q.561 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Farming/CropStrainCatalog.cs`

### `Assets/Ashfall.Core/Farming/CropStrainCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 297 lines / 13982 bytes.
- SHA-256: `65be5a26c725beeeabd75fd2603ada23fc696020671c4512d144d03dff9384cb`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class NutritionProfileDef
public float calories { get; set; } = 1f;
public float protein { get; set; }
public float vitamin_c { get; set; }
public float micronutrients { get; set; }
public float fats { get; set; }
public float fiber { get; set; }
public static readonly string[] Categories = {
public float Get(string category) => category switch
public sealed class MutationOutcomeDef
public string outcome { get; set; } = "no_mutation";
public string result_strain_id { get; set; } = "";
public float weight { get; set; } = 1f;
public sealed class CropStrainDef
public string id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string seed_item_id { get; set; } = string.Empty;
public float yield_modifier { get; set; } = 1f;
public float soil_tolerance { get; set; } = 0.5f;
public int toxicity_tolerance_permille { get; set; } = 300;
public float radiation_tolerance { get; set; } = 0.5f;
public float pest_susceptibility { get; set; } = 0.5f;
public float mutation_threshold { get; set; } = 1f;
public NutritionProfileDef nutrition_profile { get; set; } = new NutritionProfileDef();
public List<MutationOutcomeDef> mutation_outcomes { get; set; } = new List<MutationOutcomeDef>();
public List<string> tags { get; set; } = new List<string>();
public sealed class PestDef
public string id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public float base_chance_per_day { get; set; } = 0.02f;
public float severity_step { get; set; } = 0.1f;
public float yield_damage_at_full { get; set; } = 0.6f;
public List<string> treats_with_item_ids { get; set; } = new List<string>();
public string season_tag { get; set; } = "any";
public List<string> target_strain_tags { get; set; } = new List<string>();
public sealed class CompostRecipeDef
public string id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string input_item_id { get; set; } = string.Empty;
public int input_count { get; set; } = 2;
public string output_item_id { get; set; } = string.Empty;
public int output_count { get; set; } = 1;
public int duration_days { get; set; } = 5;
public sealed class CropStrainCatalogContainer
public int schema_version { get; set; } = 1;
public List<CropStrainDef> strains { get; set; } = new List<CropStrainDef>();
public List<PestDef> pests { get; set; } = new List<PestDef>();
public List<CompostRecipeDef> compost_recipes { get; set; } = new List<CompostRecipeDef>();
public sealed class CropStrainCatalogDiagnostic
public string File { get; }
public string EntryId { get; }
public string Field { get; }
public string Problem { get; }
public override string ToString() =>
public static class CropStrainCatalogLoader
public const string DefaultFileName = "crop_strains.json";
public static CropStrainCatalogContainer Load( string dataDir, IFileIO files, IJsonSerializer json) {
public static List<CropStrainCatalogDiagnostic> Validate(CropStrainCatalogContainer catalog) {
public static bool IsLegalOutcome(string outcome) => outcome switch
```


# Appendix Q.562 — Additional Current Architecture Evidence: `src/Host/PanelBindLifecycleSelfTest.cs`

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


# Appendix Q.564 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentDeepChainGate.cs`

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


# Appendix Q.565 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/EquipmentConditionSystem.cs`

### `Assets/Ashfall.Core/EquipmentConditionSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 554 lines / 21900 bytes.
- SHA-256: `c2d5298a09dd8193c69012a033640f740bf8da21e537a4674b1d152d4a052c8b`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=32; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class EquipmentConditionState
public string systemId = EquipmentConditionSystem.SystemId;
public List<EquipmentInstance> items = new List<EquipmentInstance>();
public List<MaintenanceJob> pendingJobs = new List<MaintenanceJob>();
public sealed class EquipmentInstance
public string instanceId = string.Empty;
public string itemId = string.Empty;
public string ownerId = string.Empty;
public float condition = 100f;
public float maxCondition = 100f;
public EquipmentFamily family;
public string material = string.Empty;
public int usesRemaining = -1;      // -1 = unlimited
public float lastMaintainedDay = -1;
public List<string> maintenanceHistory = new List<string>();
public float originalMaxCondition = 100f;
public float rustLevel = 0f;
public float repairQuality = 1f;
public bool temporaryPatch = false;
public bool isBroken = false;
public bool isJammed = false;
public enum EquipmentFamily { Tool, Weapon, Medical, Clothing, Electronics, Container, Watercraft } [Serializable] public sealed class WearEvent { public string source = string.Empty; public float intensity = 1f; public float environmentModifier = 1f; public string actionId = string.Empty; }
public sealed class DegradationProfileDef
public string profile_id = string.Empty;
public string item_family = string.Empty;
public string display_name = string.Empty;
public float base_wear_per_use = 1f;
public float base_wear_per_day_exposed = 0.5f;
public float corrosion_susceptibility = 1f;
public float cold_brittleness = 1f;
public float heat_sensitivity = 1f;
public float jam_threshold = 20f;
public float break_threshold = 5f;
public float max_durability_loss_per_repair = 5f;
public List<string> maintenance_materials = new List<string>();
public float repair_efficiency = 0.8f;
public bool jury_rig_allowed = true;
public List<string> tags = new List<string>();
public sealed class ItemDegradationCatalog
public int schema_version = 1;
public List<DegradationProfileDef> profiles = new List<DegradationProfileDef>();
public sealed class MaintenanceJob
public string jobId = string.Empty;
public string instanceId = string.Empty;
public string stationId = string.Empty;
public MaintenanceType type;
public float progress;
public float totalRequired = 1f;
public bool isComplete;
public List<string> reservedParts = new List<string>();
public enum MaintenanceType { Sharpen, Repair, Calibrate, Clean, ReplacePart } public sealed class EquipmentConditionSystem { public const string SystemId = "equipment_condition"; private EquipmentConditionState _state = new EquipmentConditionState(); private readonly ISeededRng _rng; private readonly ILog _log; private readonly Inventory.Inventory _inventory; private readonly CraftingSystem _crafting; private int _currentDay; public EquipmentConditionState State => _state; public event Action<EquipmentInstance> OnConditionChanged; public event Action<MaintenanceJob> OnMaintenanceCompleted; public event Action OnEquipmentChanged; public event Action<EquipmentInstance>? OnItemConditionChanged; public event Action<EquipmentInstance>? OnItemJammed; public event Action<EquipmentInstance>? OnItemBroken; public event Action<EquipmentInstance>? OnItemRepaired; /// <summary>Optional campaign multiplier for use wear.</summary> public Func<float>? WearRateMultiplierProvider { get; set; }
public EquipmentInstance? GetItem(string instanceId) {
public void ReduceDurability(string instanceId, float amount) {
public void RegisterProfile(DegradationProfileDef profile) {
public void LoadProfiles(string jsonContent) {
public bool TryGetProfile(string id, out DegradationProfileDef profile) {
public ActionResult RegisterItem(string instanceId, string itemId, string ownerId, EquipmentFamily family, float maxCondition = 100f) {
public ActionResult UseItem(string instanceId, float wearAmount = 1f) {
public ActionResult ApplyWear(string instanceId, WearEvent evt) {
public ActionResult ApplyCorrosion(string instanceId, float exposureAmount, string environmentType = "weather") {
public ActionResult JuryRig(string instanceId, List<string> scrapMaterialIds) {
public ActionResult RepairItem(string instanceId, MaintenanceType type, List<string> parts, float repairQuality = 1.0f) {
public ActionResult ClearJam(string instanceId) {
public float GetConditionPercent(string key) {
public ActionResult StartMaintenance(string instanceId, string stationId, MaintenanceType type, List<string> requiredParts) {
public void TickDay(int day) {
public float GetSlipRisk(string instanceId) {
public float GetJamRisk(string instanceId) {
public bool IsUsable(string instanceId) {
public EquipmentConditionState CaptureState() => CloneState(_state);
public void RestoreState(EquipmentConditionState saved) {
```


# Appendix Q.566 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/ExpansionHubSave.cs`

### `Assets/Ashfall.Core/ExpansionHubSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 507 lines / 28559 bytes.
- SHA-256: `6f9efccbfed1c5101889fb1aeff9b33800d110d217b1f669924bbbfdf2d1e282`.
- Architecture signals: seeded references=0; save/restore symbols=31; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class ExpansionHubSave
public const int CurrentSaveVersion = 6;
public int saveVersion = CurrentSaveVersion;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public DiseaseSystemState disease = new DiseaseSystemState();
public DebtDispatcherState debtDispatcher = new DebtDispatcherState();
public FactionEmbargoLedgerState embargoes = new FactionEmbargoLedgerState();
public DebtConsequenceBridgeState debtBridge = new DebtConsequenceBridgeState();
public SaltMineState saltMine = new SaltMineState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV5
public int saveVersion = 5;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public DiseaseSystemState disease = new DiseaseSystemState();
public DebtDispatcherState debtDispatcher = new DebtDispatcherState();
public FactionEmbargoLedgerState embargoes = new FactionEmbargoLedgerState();
public DebtConsequenceBridgeState debtBridge = new DebtConsequenceBridgeState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV1
public int saveVersion = 1;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV2
public int saveVersion = 2;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV3
public int saveVersion = 3;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV4
public int saveVersion = 4;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public DiseaseSystemState disease = new DiseaseSystemState();
public string Checksum = string.Empty;
public static class ExpansionHubSaveCodec
public static ExpansionHubSave Capture( int simDay, WaystationSystem waystation, LocationLayoutSystem layouts, LocationMemorySystem memory, SiteEncounterSystem siteEncounters,
public static string Encode(ExpansionHubSave save, IJsonSerializer json) {
public static ExpansionHubSave Decode(string jsonText, IJsonSerializer json) {
public static void Restore( ExpansionHubSave save, WaystationSystem waystation, LocationLayoutSystem layouts, LocationMemorySystem memory, SiteEncounterSystem siteEncounters,
```


# Appendix Q.567 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Narrative/ContrabandStashSystem.cs`

### `Assets/Ashfall.Core/Narrative/ContrabandStashSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 274 lines / 13341 bytes.
- SHA-256: `f4cc0a9ccaa5e780f8b70e61196f03f882562f7f762b04103cd42f9fc54d2c1f`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=5; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ContrabandStashActivation
public string entryId = string.Empty;
public string canonicalItemId = string.Empty;
public int grantQuantity = 1;
public int minDay = 1;
public override string ToString() => $"{entryId} -> {canonicalItemId} x{grantQuantity} (day >= {minDay})";
public sealed class ContrabandStashState
public string systemId = ContrabandStashSystem.SystemId;
public Dictionary<string, int> claimedDayByEntry = new Dictionary<string, int>(StringComparer.Ordinal);
public sealed class ContrabandStashSystem
public const string SystemId = "contraband_stash";
public event Action<string, int>? OnStashClaimed; // entryId, day
public event Action? OnStateChanged;
public IReadOnlyCollection<ContrabandStashActivation> Activations => _activationsByEntry.Values;
public ContrabandStashState State => _state;
public bool TryRegisterActivation(ContrabandStashActivation activation) {
public static IReadOnlyList<ContrabandStashActivation> DefaultActivations() => new List<ContrabandStashActivation>
public bool IsActivated(string entryId) => !string.IsNullOrEmpty(entryId) && _activationsByEntry.ContainsKey(entryId);
public bool IsClaimed(string entryId) => !string.IsNullOrEmpty(entryId) && _state.claimedDayByEntry.ContainsKey(entryId);
public bool IsDiscoverable(string entryId, int day) {
public List<ContrabandEntry> ListDiscoverable(int day) {
public ContrabandStashActivation? GetActivation(string entryId) => IsActivated(entryId) ? _activationsByEntry[entryId] : null;
public ActionResult TryClaimStash(string entryId, int day) {
public ContrabandStashState CaptureState() => CloneState(_state);
public void RestoreState(ContrabandStashState? saved) {
```


# Appendix Q.568 — Additional Current Architecture Evidence: `src/Host/CraftingHostSession.cs`

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


# Appendix Q.569 — Additional Current Architecture Evidence: `src/UI/FarmingPanel.cs`

### `src/UI/FarmingPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 481 lines / 21224 bytes.
- SHA-256: `bd294ce4494a3a805f205b00618d5fdae3a33593efd4e512b8c09c4e2c194638`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=11; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class FarmingPanel : Control
public event Action? OnClose;
public event Action<string, string>? OnActionRequested;
public bool IsBound => _host != null;
public void Bind(AgricultureHostSession session) {
public override void _Ready() {
public void RefreshView() {
public void Open() {
public void Close() {
public override void _UnhandledInput(InputEvent @event) {
public void Unbind() {
public override void _ExitTree() {
```


# Appendix Q.570 — Additional Current Architecture Evidence: `src/Host/ExpansionHostSession.cs`

### `src/Host/ExpansionHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 500 lines / 25698 bytes.
- SHA-256: `9809fca6ed766dfd903f6dcc7df79e68f779d539a85bbd8a66066710d570da45`.
- Architecture signals: seeded references=0; save/restore symbols=5; typed event declarations=2; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExpansionHostSession
public const int DefaultSeed = 1117; // greenhouse + vouch demo seed
public WaystationSystem Waystation { get; }
public LocationLayoutSystem Layouts { get; }
public LocationMemorySystem Memory { get; }
public SiteEncounterSystem SiteEncounters { get; }
public StandingRecordCatalog RecordQuests { get; }
public VouchAccessSystem Vouch { get; }
public GreenhouseSystem Greenhouse { get; private set; }
public CrossingArbitrationSystem Arbitration { get; }
public LedgerDebtSystem Ledger { get; }
public CrossingQuestSystem CrossingQuests { get; }
public GenerationalSuccessionEngine Generational { get; }
public EpilogueMatrixRuntime Epilogue { get; }
public DutyRosterSystem DutyRoster { get; private set; }
public Ashfall.Core.Foundry.SilentFoundrySystem SilentFoundry { get; private set; }
public Ashfall.Core.Foundry.SilentFoundryCatalog FoundryData { get; private set; }
public Ashfall.Core.Disease.DiseaseSystem Disease { get; private set; }
public Ashfall.Core.Disease.DiseaseCatalog DiseaseData { get; private set; }
public DebtTemplateCatalog? DebtCatalog { get; private set; }
public DebtConsequenceDispatcher? DebtDispatcher { get; private set; }
public FactionEmbargoLedger Embargoes { get; } = new FactionEmbargoLedger();
public void BindDutyRoster(DutyRosterSystem roster) {
public void BindGreenhouse(GreenhouseSystem shared) {
public event Action<CrossingStageNarrativeEvent>? OnCrossingStageNarrative;
public static ExpansionHostSession Create( string dataDirectory, ILog log = null!, Ashfall.Core.Flags.IFlagLedger? consequenceLedger = null) {
public void ShutdownDebtIntegration() {
public override void Dispose() {
public ExpansionHubSave CaptureSave(int simDay, DebtConsequenceBridgeState? debtBridge = null) =>
public void RestoreSave(ExpansionHubSave save, DebtConsequenceHostBridge? debtBridge = null) =>
public void LoadDefaultBackerPool() {
public string ArbitrationLine() {
public string LedgerLine() {
public void UnlockWaystation() => Waystation.Unlock();
public void SetWaystationWintering(bool wintering) => Waystation.SetWintering(wintering);
public bool AssignWaystationWatch(string[] ids) => Waystation.AssignWatch(ids);
public void ResupplyWaystation() => Waystation.Resupply();
public void TickWaystation(bool iceRoadOpen) => Waystation.TickDaily(iceRoadOpen);
public string WaystationLine() {
public void UnlockRecord() {
public bool ArriveAtSite(string parentId) => Layouts.ArriveAtParent(parentId);
public bool EnterSiteRoom(string roomId) => Layouts.EnterRoom(roomId);
public bool InspectSiteRoom(string roomId) => Layouts.InspectRoom(roomId);
public string RoomLine(string parentId, string roomId) {
public string StandingRecordLine() {
public string RecordQuestLine() {
public bool GrantVouch(string npcId) => Vouch.GrantVouch(npcId, isLastResort: false);
public bool BurnVouch() => Vouch.BurnVouch();
public bool SoftenAccess() => Vouch.SoftenAccess();
public string CrossingLine() {
public bool StartCrossingQuest(string questId, int currentDay) => CrossingQuests.StartQuest(questId, currentDay);
public void TickCrossingQuests(int currentDay) => CrossingQuests.TickDaily(currentDay, hasVouchAccess: Vouch.HasAccess);
public int AdvanceCrossingQuestStage(string questId) => CrossingQuests.AdvanceStage(questId);
public bool MakeCrossingChoice(string questId, string choiceId) => CrossingQuests.MakeChoice(questId, choiceId);
public List<CrossingQuestDef> GetAvailableCrossingQuests(int currentDay) => CrossingQuests.GetAvailableQuests(currentDay);
public bool FailCrossingQuest(string questId) => CrossingQuests.FailQuest(questId);
public bool IsCrossingQuestFailed(string questId) => CrossingQuests.IsQuestFailed(questId);
public bool IsCrossingQuestCompleted(string questId) => CrossingQuests.IsQuestCompleted(questId);
public string CrossingQuestLine() {
public void EnsureGreenhousePlots(int count) => Greenhouse.EnsurePlots(count);
public bool PlantGreenhouse(int plotIndex, string seedItemId, int day) => Greenhouse.Plant(plotIndex, seedItemId, day, out _);
public void WaterGreenhouse(int plotIndex, float units) => Greenhouse.Water(plotIndex, units, tainted: false);
public GreenhouseHarvest HarvestGreenhouse(int plotIndex) => Greenhouse.Harvest(plotIndex);
public void TickGreenhouse(int simDay) =>
public string GreenhouseLine() {
public void RegisterGenerationDweller(string dwellerId, int age, int generation = 0) => Generational.RegisterDweller(dwellerId, age, generation);
public string AdvanceGenerationalTime(int days) {
public string FormMentorshipDemo(string mentorId, string apprenticeId, string traitId) {
public string GenerationalLine() {
public GenerationalSuccessionSaveState CaptureGenerationalSave() => Generational.CaptureState();
public void RestoreGenerationalSave(GenerationalSuccessionSaveState state) => Generational.RestoreState(state);
public string GenerateEpilogueNarrativeDemo(EpilogueEvaluationContext ctx) => Epilogue.GenerateEpilogueNarrative(ctx);
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


# Appendix Q.572 — Additional Current Architecture Evidence: `src/Audio/AudioEventBridge.cs`

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


# Appendix Q.573 — Additional Current Architecture Evidence: `src/Host/HostCli.Plans162_165.cs`

### `src/Host/HostCli.Plans162_165.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 489 lines / 26252 bytes.
- SHA-256: `50eed813c71e4acdc008e32fac4d07a404b9782e05500e6f04d75e9b90820bd2`.
- Architecture signals: seeded references=22; save/restore symbols=9; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunAgricultureSelfTest(string dataDirectory) {
public static int RunDefenseSelfTest(string dataDirectory) {
public static int RunPsychologySelfTest(string dataDirectory) {
public static int RunWildlifeSelfTest(string dataDirectory) {
public static int RunTrappingHostSelfTest(string dataDirectory) {
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


# Appendix Q.575 — Additional Current Architecture Evidence: `src/Host/HostCli.ExpeditionPlaytest.cs`

### `src/Host/HostCli.ExpeditionPlaytest.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 542 lines / 28977 bytes.
- SHA-256: `c9e8a9935c3281af8ecfbb31d467605c9eb29e774afb1b6206fa4c4569710586`.
- Architecture signals: seeded references=5; save/restore symbols=6; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static partial class HostCli
public static int RunExpeditionPlaytestSelfTest(string dataDirectory) {
public string id = string.Empty;
public bool passed;
public string evidence = string.Empty;
public int schema_version = 1;
public int master_seed;
public int snapshot_count;
public int sorties_launched;
public int sorties_completed;
public int completed_loot_entries;
public int returned_loot_entries;
public int estimate_tick_mismatches;
public int breakdown_events;
public int profiles_exercised;
public bool same_seed_byte_equal;
public bool midpoint_save_load_byte_equal;
public bool different_seed_diverged;
public List<ExpeditionPlaytestSortie> sorties = new List<ExpeditionPlaytestSortie>();
public List<ExpeditionPlaytestSnapshot> snapshots = new List<ExpeditionPlaytestSnapshot>();
public List<ExpeditionPlaytestCheck> checks = new List<ExpeditionPlaytestCheck>();
public int sortie_index;
public int launch_day;
public string survivor_id = string.Empty;
public string location_id = string.Empty;
public string vehicle_id = "foot";
public float estimate_ticks;
public int actual_ticks;
public float estimate_fuel;
public float fuel_spent;
public int encounters;
public int breakdowns;
public float loot_gross_value;
public int loot_entries;
public string result = "active";
public int day;
public int active_sorties;
public int launched_sorties;
public int completed_sorties;
public float fuel_spent;
public int encounters;
public int breakdowns;
public float returned_loot_value;
public List<string> active_survivors = new List<string>();
public int schema_version = 1;
public List<ExpeditionState> active = new List<ExpeditionState>();
public List<ExpeditionPlaytestSortie> sorties = new List<ExpeditionPlaytestSortie>();
public ulong rng_state;
public int next_launch_index;
public int next_sortie_index;
public Dictionary<string, float> fuel = new Dictionary<string, float>(StringComparer.Ordinal);
public int completed_count;
public Dictionary<string, int> ticks_by_survivor = new Dictionary<string, int>(StringComparer.Ordinal);
public List<ExpeditionPlaytestSnapshot> Snapshots { get; } = new List<ExpeditionPlaytestSnapshot>();
public List<ExpeditionPlaytestSortie> Sorties { get; } = new List<ExpeditionPlaytestSortie>();
public ExpeditionSystem System { get; }
public int LaunchedSorties => Sorties.Count;
public int CompletedSorties => Sorties.Count(s => s.result == "completed");
public int CompletedLootEntries => Sorties.Where(s => s.result == "completed").Sum(s => s.loot_entries);
public int ReturnedLootEntries => Sorties.Where(s => s.result == "completed").Sum(s => s.loot_entries);
public int EstimateTickMismatches => Sorties.Count(s => s.result == "completed" && Math.Abs(s.estimate_ticks - s.actual_ticks) > 0.01f);
public int NegativeResourceCount { get; private set; }
public int BreakdownEvents => Sorties.Sum(s => s.breakdowns);
public int ProfilesExercised => Sorties.Select(s => s.vehicle_id).Distinct(StringComparer.Ordinal).Count();
public bool NoEngagementTravelRolls { get; private set; } = true;
public static ExpeditionPlaytestRun Create(string dataDirectory, int seed) {
public void AdvanceThrough(int firstDay, int lastDay) {
public ExpeditionPlaytestSave CaptureSave() {
public void RestoreSave(ExpeditionPlaytestSave save) {
public bool AllRecordsSane() {
public ExpeditionPlaytestArtifact BuildArtifact(List<ExpeditionPlaytestCheck> checks, bool same, bool saveParity, bool different) => new ExpeditionPlaytestArtifact
```


# Appendix Q.576 — Additional Current Architecture Evidence: `src/Main.PlayerSurfaces.cs`

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


# Appendix Q.577 — Additional Current Architecture Evidence: `src/Host/HostCli.Collectibles.cs`

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


# Appendix Q.578 — Additional Current Architecture Evidence: `src/Host/HostCli.StartingSupplies.cs`

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


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.


# Appendix S — Quality Assurance Pass Record: Plan 91

This record is part of the planning artifact, not a fresh runtime test result.

## Pass 1 — content and premise accuracy
- Content pass replaced the 14→30 premise with the current 34-row greenhouse authority.
- The historical baseline is separated from the current source/data/test authority.
- Current row counts and owner boundaries are stated without using count as a quality proxy.

## Pass 2 — integration architecture
- Integration pass traced merged item identity → greenhouse commands → harvest → save.
- Core, data, host, UI, save, event and test seams are named with current paths.
- The plan does not authorize a parallel save section, catalog, manager or host cache.

## Final precision and reaccuracy pass
- Precision pass distinguishes seed, supply, crop and clean/tainted output semantics.
- Every embedded current-file hash, focused runner command and master-authority reference is rechecked.
- Any proposed future seam is labeled as requiring a separate claim and premise verification.
