# ASHFALL Plans 62–65 Authority and Reconnaissance Map

**Scope:** Deep-Strata Relic Archaeology & Archive Decryption (Plan 62), Raider Captives, Interrogation & Penal Labor Shifts (Plan 63), Food Spoilage, Cryogenic Refrigeration & Meat-Smoker Curing (Plan 64), and Grand Epilogue Simulator & Shelter Chronicle (Plan 65).

**Status:** Reconnaissance complete. Architectural boundaries, integration seams, and authority definitions established.

---

## 1. Executive Mission & System Architecture

Plans 62–65 form a unified late-midgame through endgame progression arc:
1. **Plan 62 (Relic Archaeology & Archive Decryption):** Converts deep excavation yields into decipherable pre-war encrypted archives, unlocking high-tier research, codex history, and unique shelter capabilities without creating parallel research trees.
2. **Plan 63 (Captive Management & Penal Labor):** Manages raider/hostile captives captured during combat or defense, providing humane or coercive interrogation options, guarded penal labor shifts, security risk, and parole pathways into full shelter citizenship without corrupting the canonical survivor roster.
3. **Plan 64 (Food Preservation, Spoilage & Cryogenics):** Elevates shelter sustenance from simple aggregate calories into spoilage cohorts governed by temperature, power grid brownouts, cellar aging, smoking/curing, and cryogenic freezing, with acute gastroenteritis/botulism disease risks.
4. **Plan 65 (Grand Epilogue Simulator & Shelter Chronicle):** A pure, deterministic, read-only terminal evaluation engine that aggregates campaign state across survival, diplomacy, ethics, technology, loss, and legacy, producing a multifaceted narrative chronicle and exportable JSON summary.

---

## 2. One-Authority Matrix

| Concern | Canonical Authority | Boundary for Plans 62–65 |
|---|---|---|
| **Core Simulation** | `Assets/Ashfall.Core/` | All state machines, validation rules, probability draws via `ISeededRng`, DTOs, and event declarations live here. Zero engine references (`Godot`, `UnityEngine`). |
| **Presentation & Host Wiring** | `src/` | Godot nodes forward commands, observe domain state, register save stores, and update UI. Never calculate simulation outcomes or mutations. |
| **Authored Data** | `Assets/StreamingAssets/Data/` | JSON catalogs with `schema_version: 1`, snake_case keys, validated via `CatalogIntegrityValidator` and tracked in `ContentUtilizationScanner`. |
| **Item & Inventory Mutations** | `Ashfall.Core.Inventory.Inventory` | All material costs (fragments, chemicals, food, salt, fuel) and rewards must execute via `InventoryBill` / `TryExecuteTransaction`. No subsystem shadows inventory. |
| **Survivor Roster & Lifecycle** | `SurvivorEntityStore` / `SurvivorAggregate` | Captives remain isolated in `ShelterPrisonerSystem` until lawful parole/citizenship conversion assigns a real `SurvivorId` and registers in `SurvivorEntityStore`. |
| **Survivor Skills & Traits** | `ISurvivorSkillsPort` / `SurvivorEntityStore` | Guard effectiveness, cryptanalyst speed, and culinary preservation read standard survivor skills (`skill_cold_analysis`, `skill_mechanical`, `skill_medicine`, etc.). |
| **Room Infrastructure** | `ShelterRoom` / `shelter_rooms.json` | Relic deciphering requires `room_laboratory_research` / `room_workshop_precision`; captive cells require `room_ward_quarantine` / `room_storage_secure`; smoking/cooling requires kitchen/pantry fixtures. |
| **Power Infrastructure** | `PowerGridSystem` | Cryo-lockers and lab decryptors draw active power; grid blackouts/brownouts trigger acute spoilage and decrypt stalls. |
| **Health & Disease** | `MedicalPipelineCoordinator` / `disease_catalog.json` | Ingesting spoiled rations applies canonical diseases (`disease_cholera`, `disease_wellspring_cramps`) through standard affliction ports. |
| **Campaign Persistence** | `SaveStoreHub` / `ISaveStore` | Each system owns an envelope with `SchemaVersion` and `SaveChecksum`. |
| **Random Determinism** | `ISeededRng` | Explicit domain keys (`archive_decrypt`, `captive_interrogate`, `food_spoilage`, `epilogue_eval`). Zero `System.Random` or `Guid.NewGuid()`. |

---

## 3. Pillar 1: Plan 62 — Deep-Strata Relic Archaeology & Archive Decryption

### Existing Seams
- **`ExcavationSystem` & `excavation_sites.json`:** Excavations uncover relic fragments, encrypted data-plates, and magnetic media from subterranean strata.
- **`ResearchSystem` & `research_nodes.json`:** Pre-war archives feed directly into research knowledge requirements or provide instant tech unlocks.
- **`JournalSystem`:** Codex discovery notifications and historical lore entries.
- **`relic_recipes.json` & `relic_provenance_dossiers.json`:** Material handling and restoration.

### System Design
- **Catalog:** `prewar_archives.json`
  - Defines encrypted archive records: ID, title, encryption grade (Basic, MilSpec, Sub-Quantum, Orbital), required decryption stages, laboratory apparatus needed, skill affinities, output research IDs, codex unlock IDs, and schematic grants.
- **Core Engine:** `PrewarArchiveDecryptionSystem`
  - Manages active decryption projects in the laboratory.
  - Handles fragment assembly, chemical bath cleaning (consumes solvents/acids), and algorithmic decryption.
  - Emits completion events wired to `ResearchSystem` and `JournalSystem`.
- **Persistence:** `PrewarArchiveSaveStore`
  - Enrolls in `SaveStoreHub`, serializes discovered, in-progress, and deciphered archives.

---

## 4. Pillar 2: Plan 63 — Raider Captives, Interrogation & Penal Labor Shifts

### Existing Seams
- **Combat & Defense:** Defeated raiders / infiltrators yields captives instead of only casualties.
- **Shelter Rooms:** Captives occupy secure containment (`room_ward_quarantine` or `room_storage_secure`). Capacity is strictly bounded by room count and operational condition.
- **Faction Diplomacy (`FactionWarSystem`, `FactionStandingIdResolver`):** Captive faction origins affect faction relations, ransom negotiations, or retaliatory raids.
- **Morale (`MoralChoiceSystem`, `NeedsSystem`):** Treatment of captives (humane vs harsh interrogation, ration tiers, penal labor) influences community morale and survivor trauma.

### System Design
- **Catalog:** `captive_interrogations.json`
  - Defines captive personality archetypes, resistance thresholds, interrogation topics (cache locations, raid timings, cipher keys, faction secrets), compliance gains, trauma risks, and parole conditions.
- **Core Engine:** `ShelterPrisonerSystem`
  - Maintains `PrisonerRecord` instances separate from `SurvivorAggregate`.
  - Enforces guard-to-prisoner ratios using assigned survivors. Lack of guards increases escape/riot risk.
  - Implements non-graphic, psychological/dialogue questioning options: Rapport Building, Strategic Bargaining, Firm Pressure, Isolation.
  - Governs penal labor (demanding surface clearing, scrap sorting, slurry shoveling) with exhaustion and revolt metrics.
  - Implements Parole / Integration: when compliance is high and hostility drops to zero, prisoners can be granted amnesty and inducted into the survivor roster.
- **Persistence:** `ShelterPrisonerSaveStore`
  - Enrolls in `SaveStoreHub`, stores captive roster, interrogation progress, compliance levels, and parole history.

---

## 5. Pillar 3: Plan 64 — Food Spoilage, Cryogenic Refrigeration & Meat-Smoker Curing

### Existing Seams
- **`KitchenNutritionSystem`:** Contains initial pantry and preservation stubs (`PreservationMethod`, `PantryItem`, `SetCellar`, `SetRefrigeration`).
- **`PowerGridSystem`:** Powers cryogenic chillers and electric refrigerators. Outages switch items to ambient temperature decay.
- **`disease_catalog.json` / `MedicalPipelineCoordinator`:** Spoiled consumption causes enteric afflictions.
- **`Inventory`:** Stores raw ingredients (raw meat, root vegetables, tubers, dried grain, salt, herbs).

### System Design
- **Catalog:** `food_preservation.json`
  - Defines preservation methods: Fresh/Unpreserved, Root Cellar, Salted/Cured, Wood Smoked, Pickled/Fermented, Cryogenic Freezing.
  - Specifies decay half-lives, power consumption per 100 rations, temperature sensitivity, flavor/morale ratings, and spoilage toxicity rates.
- **Core Engine:** `FoodPreservationSystem` (integrated with `KitchenNutritionSystem`)
  - Tracks food items in discrete age cohorts rather than per-unit instances to guarantee zero allocation bloat.
  - Daily tick updates freshness based on storage tier, ambient shelter temperature, and power grid status.
  - Curing & Smoking processing jobs: convert perishable raw protein + salt/wood into long-lasting shelf-stable provisions via inventory bills.
  - Contamination logic: spoiled food is flagged or discarded; forced consumption triggers illness events.
- **Persistence:** `FoodPreservationSaveStore`
  - Enrolls in `SaveStoreHub`, serializes food inventory cohorts, active preservation facilities, and spoilage logs.

---

## 6. Pillar 4: Plan 65 — Grand Epilogue Simulator & Shelter Chronicle

### Existing Seams
- **Survivor Outcomes:** Total survivors, deceased list from `MemorialSystem`, health/trauma status.
- **World & Factions:** Faction war status (`FactionWarSystem`), regional dominance, trade pacts.
- **Technological Legacy:** Research tier, decrypted pre-war archives (Plan 62), operational foundry/infrastructure.
- **Moral & Social Chronicle:** Captive treatment record (Plan 63), moral choices logged, starvation/disease frequency (Plan 64).

### System Design
- **Catalog:** `campaign_epilogues.json`
  - Authored narrative vignettes for distinct thematic vectors:
    - Survival & Demographics (flourishing, decimated, machine-sustained, extinct)
    - Societal Order & Justice (benevolent democracy, militarized regime, penal colony, pariah holdout)
    - Technological Reconstruction (pre-war revival, neo-feudal scavengers, lost knowledge)
    - Regional Wasteland Impact (unifier, isolated hermit vault, war-torn ruin)
    - Faction Destinies (partnered, subjugated, betrayed, annihilated)
  - Evaluation rules: condition expressions evaluating numeric and boolean metrics.
- **Core Engine:** `CampaignEpilogueEngine`
  - Pure, stateless calculation engine: accepts `CampaignEpilogueSnapshot` containing all final metrics.
  - Deterministically evaluates matching vignette rules and selects tone variants using injected `ISeededRng`.
  - Assembles cohesive multipage chronicle with structured headers, narrative body, and key metric tallies.
  - Serializes to JSON for external export and record-keeping.
- **Presentation:**
  - `CampaignEpilogueModal.tscn` / `CampaignEpilogueModal.cs`: Paged epilogue reader with thematic typography, parchment styling, and export buttons.

---

## 7. Verification & Invariant Guards

1. **Deterministic Execution:**
   - All RNG calls must pass through `ISeededRng` with explicit purpose tags (`archive_decrypt`, `captive_interrogate`, `food_spoilage`, `epilogue_eval`).
   - Gated by `Ashfall.Core.Tests/DeterminismGuardTests.cs`.
2. **Save Compatibility & Checksums:**
   - Every new save store (`PrewarArchiveSaveStore`, `ShelterPrisonerSaveStore`, `FoodPreservationSaveStore`) must provide complete serialization, checksum hashing via `SaveChecksum`, and round-trip fuzzing tests.
3. **Data Integrity & Content Reachability:**
   - All 4 new JSON files must load through `CatalogIntegrityValidator` with 0 errors and register in `ContentUtilizationScanner`.
4. **End-to-End Integration Scenario:**
   - A dedicated 30-day scripted integration test will simulate excavation yielding archives, raiders captured and questioned, rations preserved through a power outage, and endgame epilogue calculation.
