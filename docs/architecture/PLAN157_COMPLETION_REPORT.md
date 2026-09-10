# Plan 157 Completion Report — Grain Milling, Storage & Food-Processing Knowledge Runtime Integration

## 1. Executive Summary

Plan 157 activates ASHFALL's authored 30-record `GrainMillingCatalog` as an authentic, discoverable industrial food-processing and storage knowledge layer connecting shelter workshops, mess halls, greenhouses, wasteland grain silos, quarries, and agricultural outposts to exploration and shelter room inspections—without creating duplicate food, agriculture, spoilage, crafting, or economy simulation subsystems.

All non-negotiable architectural directives and authority firewalls have been strictly maintained:
1. **Simulation Authority (Invariant 5)**:
   - **Flour & Inventory Authority**: Historical extraction yields (e.g. 74.5% yield on #12 silk, 71.0% yield on #14 silk) represent historical milling laboratory observations. They never modify `InventorySystem` item counts, recipe conversion coefficients in `recipe_ash_grain_flour`, or flour crafting yields.
   - **Pest & Spoilage Authority**: Silo infestation records (e.g. 14.8% moisture, 12 insects/kg *Rhyzopertha dominica*) never mutate shelter food spoilage ticks, grain rot rates, or pest outbreaks in `AgricultureSystem` or `FoodPreservationSystem`.
   - **Nutrition & Survival Authority**: Tempering assays and flour dressing notes never alter calorie density, satiety, or hunger progression in `NeedsSystem` or `KitchenSystem`.
   - **Machinery & Power Grid Authority**: Burr millstone RPM records (e.g. 115 RPM runner stone, 130 RPM semolina stone) and drive chatter logs never alter shelter power consumption, generator draw, or mechanical wear in `PowerGridSystem`.
   - **Barter & Economy Authority**: Historical grain batch values and moisture classifications never mutate trader exchange ratios or merchant prices in `ShelterBarterSystem` or `HoldfastTradeSession`.
2. **Zero Engine Coupling in Core (Invariant 1)**: `GrainMillingCatalog.cs`, `GrainMillingProjection.cs`, and `GrainMillingDiscoverySystem.cs` reside strictly within `Assets/Ashfall.Core/Narrative/` with zero references to `UnityEngine`, `Godot`, or `JsonUtility`.
3. **Epistemic Privacy Firewall**: `GrainMillingDiscoverySystem.GetRelated(recordId)` enforces strict forward privacy: it returns cross-referenced records only if they have already been discovered by the player. Undiscovered records are never spoiled.
4. **Save Envelope Integrity (Invariant 3)**: Registered in `SaveSectionRegistry` as `grain_milling_archive` with stable IDs and schema versioning (`grain_milling_archive_save.json`), wrapped via `SchemaVersionedEnvelope<GrainMillingArchiveState>` and `SaveStoreHub.FromCodec`.
5. **Presentation Discipline**: Discovered records pipe directly into the canonical `Journal` feedback strip (`JournalSystem`), eliminating stub UI panel registration.

---

## 2. Baseline Census & Provenance Mapping

The 30 authored records across 4 JSON catalogs are cataloged in detail in [`docs/content/GRAIN_MILLING_CORPUS_INVENTORY.md`](../content/GRAIN_MILLING_CORPUS_INVENTORY.md) and mapped in [`docs/content/GRAIN_MILLING_PRODUCER_MATRIX.md`](../content/GRAIN_MILLING_PRODUCER_MATRIX.md):

| Record Family | File | Total Records | Active (First-Pass) | Deferred (Expansions) | Provenance Class |
|---|---|---|---|---|---|
| **Burr Millstone Dressing** | `burr_millstone_dressing_logs.json` | 8 | 6 | 2 | TechnicalMaintenanceLog |
| **Bolting Silk Mesh** | `bolting_silk_mesh_reports.json` | 8 | 6 | 2 | TechnicalSiftingAssay |
| **Grain Silo Weevil** | `grain_silo_weevil_audits.json` | 7 | 4 | 3 | BiologicalStorageAudit |
| **Dampener Tempering** | `mill_dampener_tempering_assays.json` | 7 | 4 | 3 | ConditioningAssay |
| **TOTAL** | **4 Catalogs** | **30** | **20** | **10** | **4 Industrial Classes** |

### 2.1 Spatial Distribution Across Facilities & Wasteland
The 20 active records are mapped across 5 canonical producer archetypes with MinDay temporal progression:
- `room_workshop`:
  - `burr_millstone_french_chert_chisel_cracking` (Day 1)
  - `burr_millstone_bridge_tree_tentering_screw_chatter` (Day 15)
  - `bolting_silk_gauze_number_mesh_selection` (Day 1)
  - `bolting_silk_plansifter_gyratory_counterweight_wobble` (Day 12)
- `room_common_mess_hall`:
  - `mill_tempering_hard_wheat_bran_toughening` (Day 1)
  - `mill_tempering_water_spray_penetration_rate` (Day 14)
- `room_greenhouse`:
  - `grain_silo_weevil_rhizopertha_bore_dust` (Day 1)
  - `grain_silo_diatomaceous_earth_desiccant_dusting` (Day 16)
- `loc_grain_silo`:
  - `burr_millstone_furrow_lands_feather_edge_wear` (Day 8)
  - `bolting_silk_centrifugal_reel_beater_tear` (Day 7)
  - `grain_silo_granary_weevil_larva_hollow_berry` (Day 4)
  - `mill_tempering_flour_ash_yield_bran_cleave` (Day 6)
- `loc_quarry`:
  - `burr_millstone_runner_stone_rynd_balance_lead` (Day 5)
  - `burr_millstone_spindle_footstep_bearing_lignum_vitae` (Day 12)
- `loc_settlement_silo_burrow`:
  - `burr_millstone_swallow_eye_centrifugal_feed_shoe` (Day 18)
  - `bolting_silk_middlings_purifier_air_current_aspiration` (Day 15)
  - `grain_silo_co2_hypobaric_suffocation_pilot` (Day 22)
  - `mill_tempering_pre_break_warm_conditioning_temp` (Day 20)
- `loc_agricultural_outpost`:
  - `bolting_silk_grit_gauze_scalping_oversize_bran` (Day 10)
  - `bolting_silk_static_charge_grounding_copper_tinsel` (Day 20)

---

## 3. Inventory of Created & Modified Code & Documentation

### 3.1 Created Core & Host Components
- [`Assets/Ashfall.Core/Narrative/GrainMillingProjection.cs`](../../Assets/Ashfall.Core/Narrative/GrainMillingProjection.cs):
  - Defines `GrainMillingRecordFamily`, `GrainMillingProvenanceClass`, and `GrainMillingFacilityType`.
  - Maintains `GrainMillingRecordMetadata` with strongly-typed units (cracks/in, RPM, microns, % yield, % moisture, °C, dwell hours), canonical producers, min-day thresholds, and cross-reference relations.
  - Exposes static query helpers (`GetMetadata`, `GetAllMetadata`, `GetFamily`, `GetByProducer`, `GetRelated`).
- [`Assets/Ashfall.Core/Narrative/GrainMillingDiscoverySystem.cs`](../../Assets/Ashfall.Core/Narrative/GrainMillingDiscoverySystem.cs):
  - Core domain discovery ledger managing discovered IDs, timestamps, and discovery producers.
  - Implements `TryDiscoverRecord(recordId, currentDay)`, `DiscoverAtProducer(producerId, currentDay)`, `IsDiscovered(recordId)`.
  - Dispatches strongly-typed `OnRecordFirstDiscovered` events with idempotency protections.
  - Implements privacy-firewalled `GetRelated(recordId)` ensuring undisclosed records cannot be leaked.
  - Exposes `CaptureState()` and `RestoreState(state)` supporting unknown ID preservation and ordinal sorting.
- [`src/Host/GrainMillingArchiveSaveStore.cs`](../../src/Host/GrainMillingArchiveSaveStore.cs):
  - Sealed Godot host save store wrapping `GrainMillingArchiveState`.
  - Implements `SaveStoreHub.FromCodec` pattern with `SchemaVersionedEnvelope<GrainMillingArchiveState>` and SHA256 `SaveChecksum` verification.
- [`src/Main.Plans157.cs`](../../src/Main.Plans157.cs):
  - Partial class providing `EnsureGrainMillingDiscovery()`, `SetupGrainMillingArchive()`, `SaveGrainMillingArchive()`, and `CheckShelterGrainMillingRecords(day)`.
  - Connects expedition discovery triggers via `OnLocationDiscovered` in `ExpeditionHostSession`.
  - Emits formatted diegetic industrial logs to the player journal upon first discovery.
- [`Ashfall.Core.Tests/Narrative/GrainMillingDiscoveryTests.cs`](../../Ashfall.Core.Tests/Narrative/GrainMillingDiscoveryTests.cs):
  - 18 comprehensive unit tests covering:
    - 30-record census validation.
    - Idempotent single-event emission.
    - Day gating enforcement.
    - Epistemic privacy firewall (zero information leakage).
    - Save/restore roundtrip with SHA256 checksum stability.
    - Unknown-ID preservation across save versions.
    - Invariant 5 reflection audit verifying zero mutator coupling to live survival mechanics.

### 3.2 Modified Core, Host & Test Components
- [`Assets/Ashfall.Core/Narrative/GrainMillingCatalog.cs`](../../Assets/Ashfall.Core/Narrative/GrainMillingCatalog.cs):
  - Added seamless file resolution between root `Data/` and `Data/narrative/` subdirectories.
  - Added query accessors: `AllRecordIds`, `GetAny(id)`, `GetMillstone(id)`, `GetSilk(id)`, `GetSilo(id)`, `GetTemper(id)`.
- [`Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`](../../Assets/Ashfall.Core/Save/SaveSectionRegistry.cs):
  - Enrolled section 170: `grain_milling_archive` -> `grain_milling_archive_save.json`.
- [`src/Main.CampaignServices.cs`](../../src/Main.CampaignServices.cs) & [`src/Main.SaveOrchestrator.cs`](../../src/Main.SaveOrchestrator.cs):
  - Registered `SetupGrainMillingArchive()` and `SaveGrainMillingArchive()` in the host lifecycle.
- [`scripts/ci/generate-architecture-map.py`](../../scripts/ci/generate-architecture-map.py) & [`docs/architecture/ARCHITECTURE_TEST_MAP.md`](ARCHITECTURE_TEST_MAP.md):
  - Added 6 missing sections (`contraband_stash`, `shelter_barter`, `black_projects_archive`, `oral_lore`, `hydrogeology_archive`, `grain_milling_archive`) to bring total mapped subsystems to 170/170 (100.0% verified).
- Updated regression count assertions:
  - `Ashfall.Core.Tests/VersionReportContractTests.cs` (164 envelopes, 170 sections).
  - `Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` (170 sections).

### 3.3 Authored Documentation Deliverables
- [`docs/content/GRAIN_MILLING_CORPUS_INVENTORY.md`](../content/GRAIN_MILLING_CORPUS_INVENTORY.md): Complete forensic census of all 30 records across the 4 families, facility timeline analysis, and plausibility audit.
- [`docs/content/GRAIN_MILLING_AUTHORITY_MAP.md`](../content/GRAIN_MILLING_AUTHORITY_MAP.md): Strict authority boundaries, numeric firewalls, and system ownership matrix.
- [`docs/content/GRAIN_MILLING_PRODUCER_MATRIX.md`](../content/GRAIN_MILLING_PRODUCER_MATRIX.md): Full routing matrix for all 20 active and 10 deferred records across wasteland and shelter producers.
- [`docs/content/GRAIN_MILLING_LOCATION_IDENTITY_AUDIT.md`](../content/GRAIN_MILLING_LOCATION_IDENTITY_AUDIT.md): Location identity audit mapping industrial equipment without map node fabrication.
- [`docs/content/GRAIN_MILLING_ITEM_AND_RECIPE_CROSSWALK.md`](../content/GRAIN_MILLING_ITEM_AND_RECIPE_CROSSWALK.md): Crosswalk linking catalog knowledge to game items and recipes.

---

## 4. Verification Evidence & Quality Gates

The system conforms to all ASHFALL verification gates:
- `dotnet test Ashfall.Core.Tests`: 0 failed.
- `python3 scripts/ci/generate-architecture-map.py --check`: Passed (170/170 subsystems mapped).
- `CatalogIntegrityValidator`: All 4 data catalogs carry valid `schema_version`.
- Epistemic privacy firewall verified by automated unit tests.
- Reflection boundary check confirms zero mutation methods exist on `GrainMillingDiscoverySystem`.
