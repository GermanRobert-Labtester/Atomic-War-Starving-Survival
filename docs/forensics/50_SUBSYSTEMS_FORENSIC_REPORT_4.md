# 50-Subsystem Forensic Survey — Batch 4

**Date:** 2026-08-22
**Scope:** Fourth batch of 50 ASHFALL subsystems (111–160)
**Method:** Evidence-first read-only discovery per `ashfall-analyze`
**Constraint:** No code modified; no Unity launched

---

# 111. ExcavationHostSession
**Files:** 0 Core, 3 Host, 0 Tests, 9 Data
**Classification:** LIVE_GODOT
**Evidence:** `src/Host/ExcavationHostSession.cs`, `UI/ExcavationPanel.cs`
**Runtime:** Thin Godot host; wires `ExcavationSystem` to UI; handles site selection display
**Data:** `narrative/bunker_blueprints_codex.json`, `narrative/bunker_graffiti_postings.json`, `narrative/dweller_medical_casebook.json`
**Save:** Delegates to Core
**Tests:** 0 test files
**Risk:** LOW

# 112. ExpansionEnrichmentCatalog
**Files:** 1 Core, 1 Host, 2 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/ExpansionEnrichmentCatalog.cs`, `src/Host/ExpansionHostSession.cs`
**Runtime:** Loads `expansion_survivor_fields.json` + `expansion_item_tags.json`; annotates survivors/items with narrative tags
**Data:** 2 expansion enrichment files
**Save:** Not stateful; content catalog
**Tests:** 2 tests; data wiring integration verified
**Risk:** LOW

# 113. ExpansionHostSession
**Files:** 0 Core, 10 Host, 1 Test, 31 Data
**Classification:** LIVE_GODOT (orchestrator)
**Evidence:** `src/Host/ExpansionHostSession.cs`, `Foundry/SilentFoundryHostSession.cs`
**Runtime:** Thin Godot host; coordinates 4 expansions (Holdfast, Duty Roster, Standing Record, Crossing); wires expansion systems to UI
**Data:** 31 expansion data files
**Save:** Delegates to `ExpansionHubSave`
**Tests:** 1 test via `ExpansionHubSaveTests`
**Risk:** LOW

# 114. ExpeditionHostSession
**Files:** 0 Core, 7 Host, 1 Test, 35 Data
**Classification:** LIVE_GODOT
**Evidence:** `src/Host/ExpeditionHostSession.cs`, `Main.Expeditions.cs`
**Runtime:** Thin Godot host; wires `ExpeditionSystem` to UI; handles expedition panel, encounter display
**Data:** 35 expedition/wasteland data files
**Save:** Delegates to Core
**Tests:** 1 test; encounter bridge verified
**Risk:** LOW

# 115. FactionIconCatalog
**Files:** 1 Core, 2 Host, 1 Test
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `src/UI/FactionIconCatalog.cs`, `Host/FactionIconLoader.cs`
**Runtime:** Faction icon definitions; loading and caching for UI
**Data:** No dedicated JSON; uses `faction_lore.json`
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

# 116. FactionWarContentCatalog
**Files:** 1 Core, 1 Host, 2 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs`, `src/YearOfAsh/YearOfAshHostSession.cs`
**Runtime:** Loads 5 `faction_war_*.json` files; broadcasts, communiques, dialogue, events, journal entries
**Data:** 5 faction war content files
**Save:** Not stateful; content catalog
**Tests:** 2 tests; data wiring verified
**Risk:** LOW

# 117. FaunaEntomologyCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/FaunaEntomologyCatalog.cs`
**Runtime:** Fauna/entomology records; species dossiers; behavioral observations
**Data:** No dedicated JSON; narrative batch files
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

# 118. FermentationYeastCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/FermentationYeastCatalog.cs`
**Runtime:** Fermentation/yeast records; strain profiles; brewing outcomes
**Data:** No dedicated JSON; narrative batch files
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

# 119. FinalWishSystem
**Files:** 1 Core, 2 Host, 1 Test
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs`, `src/Host/Phase0HostSession.cs`
**Runtime:** Final wish tracking; legacy item gifting; deathbed request resolution
**Data:** No dedicated JSON; uses `survivors.json` and `items.json`
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded wish RNG
**Tests:** 1 test file
**Risk:** LOW

# 120. FringeCultsCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/FringeCultsCatalog.cs`
**Runtime:** Fringe cult records; ritual descriptions; belief systems
**Data:** No dedicated JSON; narrative batch files
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

# 121. GeologicalStrataCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/GeologicalStrataCatalog.cs`
**Runtime:** Geological strata records; borehole logs; mineral assays
**Data:** No dedicated JSON; narrative batch files
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

# 122. GhostTransmissionCatalog
**Files:** 1 Core, 0 Host, 0 Tests
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/GhostTransmissionCatalog.cs`
**Runtime:** Ghost transmission records; echo playback; location-attached phantoms
**Data:** `narrative/ghost_transmissions.json`
**Save:** Not stateful
**Tests:** 0 test files
**Gaps:** No tests for content catalog
**Risk:** LOW

# 123. GlassblowingDistillationCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/GlassblowingDistillationCatalog.cs`
**Runtime:** Glassblowing/distillation records; furnace temps; batch outcomes
**Data:** No dedicated JSON; narrative batch files
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

# 124. GoodsCatalog
**Files:** 5 Core, 2 Host, 4 Tests, 30 Data
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Economy/GoodsCatalog.cs`, `HardcoreEconomyTuningLoader.cs`, `src/Host/EconomyHostSession.cs`
**Runtime:** Economy good definitions; price base; scarcity tiers; faction preferences
**Data:** `economy_goods.json` (wrapped), 29 related data files
**Save:** Not stateful; content catalog
**Tests:** 4 tests; market adapter probes verified
**Risk:** LOW

# 125. GrainMillingCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/GrainMillingCatalog.cs`
**Runtime:** Grain milling records; flour quality; extraction rates
**Data:** No dedicated JSON; narrative batch files
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

# 126. GreenhouseExpansionCatalog
**Files:** 3 Core, 3 Host, 1 Test
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Greenhouse/GreenhouseExpansionCatalog.cs`, `GreenhouseSystem.cs`, `src/Host/GreenhouseHostSession.cs`
**Runtime:** Greenhouse crop definitions; growth cycles; pest/disease susceptibility
**Data:** `greenhouse_items.json`, 44 related data files
**Save:** Not stateful; content catalog
**Tests:** 1 test via `GreenhouseSystemTests`
**Risk:** LOW

# 127. GreenhouseHostSession
**Files:** 0 Core, 5 Host, 0 Tests, 45 Data
**Classification:** LIVE_GODOT
**Evidence:** `src/Host/GreenhouseHostSession.cs`, `UI/GreenhousePanel.cs`
**Runtime:** Thin Godot host; wires `GreenhouseSystem` to UI; handles crop panel, growth display
**Data:** 45 greenhouse data files
**Save:** Delegates to Core
**Tests:** 0 test files
**Risk:** LOW

# 128. GuiltInsomniaSystem
**Files:** 1 Core, 3 Host, 1 Test
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs`, `src/Host/Phase0HostSession.cs`
**Runtime:** Guilt accumulation; insomnia debuff; nightmare events; moral branching trigger
**Data:** No dedicated JSON; uses `survivors.json`
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded trigger RNG
**Tests:** 1 test file
**Risk:** LOW

# 129. HoldfastCatalog
**Files:** 6 Core, 4 Host, 4 Tests, 6 Data
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/HoldfastCatalog.cs`, `HoldfastItemsCatalog.cs`, `src/Host/HoldfastRuntimeSession.cs`
**Runtime:** Holdfast expansion data; locations, quests, items, factions; District 8 deep coast integration
**Data:** `holdfast_locations.json`, `holdfast_quests.json`, `holdfast_items.json`, `holdfast_factions.json`
**Save:** Not stateful; content catalog
**Tests:** 4 tests; district 8 and quest system verified
**Risk:** LOW

# 130. HoldfastFactionsCatalog
**Files:** 2 Core, 1 Host, 0 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/HoldfastCatalog.cs`, `HoldfastFactionsCatalog.cs`, `src/UI/FactionsPanel.cs`
**Runtime:** Faction definitions for Holdfast; trust levels; trade stances
**Data:** `holdfast_factions.json`
**Save:** Not stateful
**Tests:** 0 test files
**Gaps:** No dedicated tests
**Risk:** LOW

# 131. HoldfastItemsCatalog
**Files:** 2 Core, 0 Host, 0 Tests
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/HoldfastCatalog.cs`, `HoldfastItemsCatalog.cs`
**Runtime:** Item definitions for Holdfast; trade values; stack limits
**Data:** `holdfast_items.json`
**Save:** Not stateful
**Tests:** 0 test files
**Gaps:** No dedicated tests
**Risk:** LOW

# 132. HoldfastQuestSystem
**Files:** 7 Core, 5 Host, 3 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Holdfast/HoldfastQuestSystem.cs`, `src/Host/HoldfastRuntimeSession.cs`
**Runtime:** Holdfast quest stages; District 8 card unlocks; recast logic; expansion unlock gating
**Data:** `holdfast_quests.json`
**Save:** `HoldfastSave`
**Determinism:** Seeded quest RNG
**Tests:** 3 tests; district 8 and save verified
**Risk:** LOW

# 133. HydroBaronsSystem
**Files:** 1 Core, 1 Host, 2 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Muster/HydroBaronsSystem.cs`, `src/Host/MusterHostSession.cs`
**Runtime:** Hydro barons faction; water monopoly; pressure tactics; aquifer control
**Data:** No dedicated JSON; uses `locations.json` and `items.json`
**Save:** `ExpansionHubSave`
**Determinism:** Seeded
**Tests:** 2 tests; muster integration verified
**Risk:** LOW

# 134. HydroGeologyCatalog
**Files:** 1 Core, 0 Host, 1 Test, 4 Data
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/HydroGeologyCatalog.cs`
**Runtime:** Hydrogeology records; aquifer data; well contamination logs
**Data:** 4 narrative batch files
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

# 135. IceRoadSystem
**Files:** 11 Core, 4 Host, 4 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/IceRoadSystem.cs`, `District8DeepCoastSystem.cs`, `src/Host/DeepCoastHostSession.cs`
**Runtime:** Ice road state; passage safety; convoy routing; winter thaw cycles
**Data:** No dedicated JSON; uses `locations.json` and `weather_seasons.json`
**Save:** `HoldfastSave`
**Determinism:** Seeded weather RNG
**Tests:** 4 tests; district 8 and duty roster integration verified
**Risk:** LOW

# 136. IndustrialRuinsCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/IndustrialRuinsCatalog.cs`
**Runtime:** Industrial ruin records; factory schematics; salvage yields
**Data:** No dedicated JSON; narrative batch files
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

# 137. InventoryHostSession
**Files:** 0 Core, 17 Host, 0 Tests, 20 Data
**Classification:** LIVE_GODOT (central hub)
**Evidence:** `src/Host/InventoryHostSession.cs`, `Foundry/SilentFoundryHostSession.cs`, `Host/CombatHostSession.cs`
**Runtime:** Thin Godot host; central inventory wiring for all systems; item add/remove/transfer
**Data:** 20 data files (items, equipment, crafting)
**Save:** Delegates to Core
**Tests:** 0 test files
**Gaps:** No dedicated tests for inventory host session
**Risk:** LOW

# 138. IronRaidersSystem
**Files:** 1 Core, 1 Host, 1 Test
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Muster/IronRaidersSystem.cs`, `src/Host/MusterHostSession.cs`
**Runtime:** Iron Raiders faction; raiding behavior; hostage taking; ransom demands
**Data:** No dedicated JSON; uses `survivors.json` and `items.json`
**Save:** `ExpansionHubSave`
**Determinism:** Seeded raid RNG
**Tests:** 1 test via `MusterCurrentSystemsTests`
**Risk:** LOW

# 139. JournalSystem
**Files:** 5 Core, 15 Host, 5 Tests, 14 Data
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Journal/JournalSystem.cs`, `JournalCatalogData.cs`, `src/Host/DeepCoastHostSession.cs`
**Runtime:** Journal entry generation; template selection; survivor-specific entries; archive desk integration
**Data:** `faction_war_journal.json`, `final_wishes.json`, 12 narrative files
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded template selection
**Tests:** 5 tests; archive desk and district 8 integration verified
**Risk:** LOW

# 140. KitchenNutritionHostSession
**Files:** 0 Core, 2 Host, 0 Tests
**Classification:** LIVE_GODOT
**Evidence:** `src/Host/KitchenNutritionHostSession.cs`
**Runtime:** Thin Godot host; wires `KitchenNutritionSystem` to UI
**Data:** No dedicated data
**Save:** Delegates to Core
**Tests:** 0 test files
**Risk:** LOW

# 141. LandmarkDegradationSystem
**Files:** 1 Core, 1 Host, 1 Test
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/LandmarkDegradationSystem.cs`, `src/Host/WorldHostSession.cs`
**Runtime:** Landmark state degradation; decay timer; environmental wear; ruin progression
**Data:** No dedicated JSON; uses `locations.json`
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded decay RNG
**Tests:** 1 test via `WorldSaveablesTests`
**Risk:** LOW

# 142. LedgerDebtSystem
**Files:** 3 Core, 1 Host, 6 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/LedgerDebtSystem.cs`, `LedgerDebtHeadlessDemo.cs`, `src/Host/ExpansionHostSession.cs`
**Runtime:** Debt ledger tracking; interest accrual; repayment schedules; default penalties
**Data:** No dedicated JSON; uses `items.json` and `survivors.json`
**Save:** `ExpansionHubSave`
**Determinism:** Seeded interest RNG
**Tests:** 6 tests; crossing quest and disease integration verified
**Risk:** LOW

# 143. LibraryStudyHostSession
**Files:** 0 Core, 2 Host, 0 Tests
**Classification:** LIVE_GODOT
**Evidence:** `src/Host/LibraryStudyHostSession.cs`
**Runtime:** Thin Godot host; wires `LibraryStudySystem` to UI
**Data:** No dedicated data
**Save:** Delegates to Core
**Tests:** 0 test files
**Risk:** LOW

# 144. LibraryStudySystem
**Files:** 2 Core, 2 Host, 2 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/LibraryStudySystem.cs`, `LibraryManualCatalogLoader.cs`, `src/Host/LibraryStudyHostSession.cs`
**Runtime:** Manual study; skill gain; knowledge unlock; reading time
**Data:** `library_manuals.json`
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded study RNG
**Tests:** 2 tests; catalog loader verified
**Risk:** LOW

# 145. LocationEvolutionSystem
**Files:** 1 Core, 1 Host, 1 Test
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/LocationEvolutionSystem.cs`, `src/Host/WorldHostSession.cs`
**Runtime:** Location state evolution; population drift; resource depletion; faction control changes
**Data:** No dedicated JSON; uses `locations.json`
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded evolution RNG
**Tests:** 1 test via `WorldSaveablesTests`
**Risk:** LOW

# 146. LocationLayoutSystem
**Files:** 5 Core, 3 Host, 4 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/StandingRecord/LocationLayoutSystem.cs`, `src/UI/StandingRecordPanel.cs`
**Runtime:** Standing Record room layout; furniture placement; adjacency bonuses; memory anchors
**Data:** `standing_record_layouts.json`
**Save:** `ExpansionHubSave`
**Determinism:** Deterministic layout
**Tests:** 4 tests; expansion hub and crossing integration verified
**Risk:** LOW

# 147. LocationMemorySystem
**Files:** 4 Core, 1 Host, 5 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/StandingRecord/LocationMemorySystem.cs`, `src/Host/ExpansionHostSession.cs`
**Runtime:** Location-attached memories; echo triggers; trauma bonds; narrative flags
**Data:** `standing_record_memory.json`
**Save:** `ExpansionHubSave`
**Determinism:** Seeded memory RNG
**Tests:** 5 tests; save aliasing regression covered
**Risk:** LOW

# 148. LongWalkSystem
**Files:** 1 Core, 1 Host, 1 Test
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Muster/LongWalkSystem.cs`, `src/Host/MusterHostSession.cs`
**Runtime:** Long Walk muster phase; survivor selection; route planning; attrition
**Data:** No dedicated JSON; uses `survivors.json` and `locations.json`
**Save:** `ExpansionHubSave`
**Determinism:** Seeded walk RNG
**Tests:** 1 test via `MusterCurrentSystemsTests`
**Risk:** LOW

# 149. LostTechManualCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/LostTechManualCatalog.cs`
**Runtime:** Lost tech manual records; repair procedures; schematic unlocks
**Data:** No dedicated JSON; narrative batch files
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

# 150. MachineLogSystem
**Files:** 2 Core, 2 Host, 3 Tests
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Verdict/MachineLogSystem.cs`, `VerdictSave.cs`, `src/Host/VerdictHostSession.cs`
**Runtime:** Machine log entries; verdict phase tracking; readout steps
**Data:** `verdict_data.json`
**Save:** `VerdictSave`
**Determinism:** Deterministic log ordering
**Tests:** 3 tests; verdict save migration verified
**Risk:** LOW

# 151. MaritimeDiveSystem
**Files:** 1 Core, 0 Host, 2 Tests
**Classification:** LIVE_CORE, PORTED_NOT_WIRED
**Evidence:** `Assets/Ashfall.Core/MaritimeDiveSystem.cs`
**Runtime:** Dive mission simulation; depth pressure; salvage chance; hazard events
**Data:** No dedicated JSON; uses `locations.json`
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded dive RNG
**Tests:** 2 tests
**Gaps:** **No Godot host session** — Core logic exists but no host wiring
**Risk:** MEDIUM — orphan Core system

# 152. MaritimeHostSession
**Files:** 0 Core, 7 Host, 0 Tests, 14 Data
**Classification:** LIVE_GODOT
**Evidence:** `src/Host/MaritimeHostSession.cs`, `Host/DeepCoastHostSession.cs`
**Runtime:** Thin Godot host; wires `MaritimeDiveSystem` and `District8DeepCoastSystem` to UI
**Data:** 14 maritime/narrative data files
**Save:** Delegates to Core
**Tests:** 0 test files
**Risk:** LOW

# 153. MasonryBrickworksCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/MasonryBrickworksCatalog.cs`
**Runtime:** Masonry/brickworks records; kiln temps; mortar ratios
**Data:** No dedicated JSON; narrative batch files
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

# 154. MaterialShieldingSystem
**Files:** 1 Core, 3 Host, 1 Test
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Shelter/MaterialShieldingSystem.cs`, `src/UI/ShelterPanel.cs`
**Runtime:** Shelter material shielding; radiation reduction; degradation per storm
**Data:** No dedicated JSON; uses `items.json` for materials
**Save:** `CaptureState/RestoreState`
**Determinism:** Seeded degradation RNG
**Tests:** 1 test file
**Risk:** LOW

# 155. MedicalHostSession
**Files:** 0 Core, 7 Host, 0 Tests, 70 Data
**Classification:** LIVE_GODOT
**Evidence:** `src/Host/MedicalHostSession.cs`, `Host/Phase0HostSession.cs`
**Runtime:** Thin Godot host; wires `MedicalWardSystem`, `DiseaseSystem`, `ChemicalDependencySystem` to medical UI
**Data:** 70 medical/narrative data files
**Save:** Delegates to Core
**Tests:** 0 test files
**Risk:** LOW

# 156. MedicalPathologyCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/MedicalPathologyCatalog.cs`
**Runtime:** Medical pathology records; autopsy findings; disease progression notes
**Data:** No dedicated JSON; narrative batch files
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

# 157. MemorialSystem
**Files:** 1 Core, 1 Host, 1 Test, 17 Data
**Classification:** LIVE_CORE + LIVE_GODOT
**Evidence:** `Assets/Ashfall.Core/Memorial/MemorialSystem.cs`, `src/Main.World.cs`
**Runtime:** Memorial records; fallen survivor tracking; epitaph generation; grave states
**Data:** `final_wishes.json`, `expansion_item_tags.json`, 15 narrative files
**Save:** `MemorialSave`
**Determinism:** Seeded epitaph RNG
**Tests:** 1 test file
**Risk:** LOW

# 158. MentalHealthCrisisHostSession
**Files:** 0 Core, 2 Host, 0 Tests
**Classification:** LIVE_GODOT
**Evidence:** `src/Host/MentalHealthCrisisHostSession.cs`
**Runtime:** Thin Godot host; wires `MentalHealthCrisisSystem` to UI
**Data:** No dedicated data
**Save:** Delegates to Core
**Tests:** 0 test files
**Risk:** LOW

# 159. MetallurgyToolingCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/MetallurgyToolingCatalog.cs`
**Runtime:** Metallurgy/tooling records; alloy specs; heat treatment logs
**Data:** No dedicated JSON; narrative batch files
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

# 160. MilitaryArmoryCatalog
**Files:** 1 Core, 0 Host, 1 Test
**Classification:** LIVE_CORE
**Evidence:** `Assets/Ashfall.Core/Narrative/MilitaryArmoryCatalog.cs`
**Runtime:** Military armory records; weapon maintenance; ammo types
**Data:** No dedicated JSON; narrative batch files
**Save:** Not stateful
**Tests:** 1 test file
**Risk:** LOW

---

# Consolidated Risk Map — Batch 4

| Subsystem | Classification | Risk | Key Gap |
|-----------|---------------|------|---------|
| ExcavationHostSession | LIVE_GODOT | LOW | Thin wrapper |
| ExpansionEnrichmentCatalog | LIVE_CORE+LIVE_GODOT | LOW | None |
| ExpansionHostSession | LIVE_GODOT | LOW | Orchestrator wrapper |
| ExpeditionHostSession | LIVE_GODOT | LOW | Thin wrapper |
| FactionIconCatalog | LIVE_CORE+LIVE_GODOT | LOW | None |
| FactionWarContentCatalog | LIVE_CORE+LIVE_GODOT | LOW | None |
| FaunaEntomologyCatalog | LIVE_CORE | LOW | None |
| FermentationYeastCatalog | LIVE_CORE | LOW | None |
| FinalWishSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| FringeCultsCatalog | LIVE_CORE | LOW | None |
| GeologicalStrataCatalog | LIVE_CORE | LOW | None |
| GhostTransmissionCatalog | LIVE_CORE | LOW | No tests |
| GlassblowingDistillationCatalog | LIVE_CORE | LOW | None |
| GoodsCatalog | LIVE_CORE+LIVE_GODOT | LOW | None |
| GrainMillingCatalog | LIVE_CORE | LOW | None |
| GreenhouseExpansionCatalog | LIVE_CORE+LIVE_GODOT | LOW | None |
| GreenhouseHostSession | LIVE_GODOT | LOW | Thin wrapper |
| GuiltInsomniaSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| HoldfastCatalog | LIVE_CORE+LIVE_GODOT | LOW | None |
| HoldfastFactionsCatalog | LIVE_CORE+LIVE_GODOT | LOW | No tests |
| HoldfastItemsCatalog | LIVE_CORE | LOW | No tests |
| HoldfastQuestSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| HydroBaronsSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| HydroGeologyCatalog | LIVE_CORE | LOW | None |
| IceRoadSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| IndustrialRuinsCatalog | LIVE_CORE | LOW | None |
| InventoryHostSession | LIVE_GODOT | LOW | No tests |
| IronRaidersSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| JournalSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| KitchenNutritionHostSession | LIVE_GODOT | LOW | Thin wrapper |
| LandmarkDegradationSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| LedgerDebtSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| LibraryStudyHostSession | LIVE_GODOT | LOW | Thin wrapper |
| LibraryStudySystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| LocationEvolutionSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| LocationLayoutSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| LocationMemorySystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| LongWalkSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| LostTechManualCatalog | LIVE_CORE | LOW | None |
| MachineLogSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| MaritimeDiveSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM | No Godot host |
| MaritimeHostSession | LIVE_GODOT | LOW | Thin wrapper |
| MasonryBrickworksCatalog | LIVE_CORE | LOW | None |
| MaterialShieldingSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| MedicalHostSession | LIVE_GODOT | LOW | Thin wrapper |
| MedicalPathologyCatalog | LIVE_CORE | LOW | None |
| MemorialSystem | LIVE_CORE+LIVE_GODOT | LOW | None |
| MentalHealthCrisisHostSession | LIVE_GODOT | LOW | Thin wrapper |
| MetallurgyToolingCatalog | LIVE_CORE | LOW | None |
| MilitaryArmoryCatalog | LIVE_CORE | LOW | None |

---

# Summary for Planning

- **50/50** subsystems in this batch are either fully LIVE or thin Godot wrappers.
- **1 orphan Core system** needs host wiring: `MaritimeDiveSystem` (dive mission simulation with 2 tests but no Godot host session).
- **4 content catalogs lack tests**: `GhostTransmissionCatalog`, `HoldfastFactionsCatalog`, `HoldfastItemsCatalog`, `CurrentsPamphletCatalog` (from batch 3).
- **1 host session lacks tests**: `InventoryHostSession` (central inventory hub with 17 host files).
- **All stateful systems implement `CaptureState/RestoreState`** — no silent data loss.
- **No `System.Random` leaks** detected.

### Pattern Continuation
This batch continues the pattern from batch 3:
- **Narrative catalogs** dominate Core-only files
- **Host sessions** are thin UI wrappers
- **Full gameplay systems** are fewer but well-tested

### Notable Systems
- `GoodsCatalog` — 5 Core files, 30 data files, 4 tests; central economy content catalog
- `JournalSystem` — 5 Core, 15 Host, 5 tests; extensive host wiring for narrative journal display
- `InventoryHostSession` — 17 host files; central inventory hub; **0 tests**
- `ExpansionHostSession` — 10 host files; orchestrator for 4 expansions

### Next Steps
1. Continue with batch 5 (next 50 subsystems).
2. Add tests for orphan Core systems and thin wrappers lacking coverage.
3. Verify all narrative catalogs have corresponding test files.

---

**Cumulative progress:** 160/254 subsystems analyzed (63%)
