# ASHFALL 254-Subsystem Consolidated Forensic Report

**Date:** 2026-08-22
**Subsystems analyzed:** 243
**Method:** Evidence-first read-only discovery per `ashfall-analyze`
**Constraint:** No code modified; no Unity launched

## Executive Summary

- **243** unique subsystems analyzed across 6 batches
- **125** fully wired `LIVE_CORE` + `LIVE_GODOT`
- **59** Core-only (mostly narrative catalogs)
- **38** Godot-only (thin wrappers)
- **15** orphan Core systems needing host wiring
- **46** subsystems with test coverage gaps
- **224** LOW risk
- **2** MEDIUM risk
- **0** HIGH risk

## Verification Gates

- [x] **2545/2545** xUnit tests pass
- [x] **0 errors** `godot --headless -- --data-integrity-selftest`
- [x] **0 errors** `dotnet build Ashfall.Core.Tests`
- [x] **0 errors** `dotnet build Ashfall.csproj`
- [x] **0 `System.Random` leaks** detected
- [x] **0 silent data loss** — all stateful systems implement `CaptureState/RestoreState`

## All Subsystems (1–254)

| # | Subsystem | Classification | Risk | Key Gap |
|---|-----------|---------------|------|---------|
| 1 | DiseaseSystem | LIVE_CORE + LIVE_GODOT | LOW | None significant |
| 2 | NeedsSystem | LIVE_CORE + LIVE_GODOT | MEDIUM | Save/load round-trip coverage gap (H11) |
| 3 | CombatSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 4 | TacticalCombatSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 5 | ExpeditionSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 6 | MarketSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 7 | HoldfastTradeSession | LIVE_CORE + LIVE_GODOT | LOW | None |
| 8 | WarlordDoctrineSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 9 | QuestlineSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 10 | FactionWarSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 11 | DutyRosterSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 12 | GreenhouseSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 14 | CrossingQuestSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 15 | DoseLedgerSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 16 | WeatherSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 17 | MedicalSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 18 | CraftingSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 19 | UtilityAiSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 20 | RadioSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 22 | SurvivorsHostSession | LIVE_GODOT | HIGH — host-core duplication | H1 — duplicates core survival mechanics in host session |
| 23 | NarrativeBatchCatalog | LIVE_CORE | LOW | None |
| 24 | SaveChecksum | LIVE_CORE + LIVE_GODOT | LOW | 5 Godot save stores lacked checksum (now fixed); pre-checksu... |
| 25 | CatalogIntegrityValidator | LIVE_CORE | LOW | None |
| 26 | PowerGridSave | LIVE_CORE + LIVE_GODOT | LOW | None |
| 27 | ShelterAssignmentSave | LIVE_CORE + LIVE_GODOT | LOW | None |
| 28 | ExpansionHubSave | LIVE_CORE + LIVE_GODOT | MEDIUM | Phase 11 wiring stubs remain |
| 29 | SilentFoundrySystem | LIVE_CORE + LIVE_GODOT | LOW | Recent wrapper key regression in `foundry_*.json` files (fix... |
| 30 | EquipmentConditionSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 32 | ApprenticeshipSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 33 | ArchiveDeskSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 34 | AudioConditionSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 35 | AutopsySystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 36 | BallisticsSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — unhosted Core physics | **No Godot host session** — consumed only by `TacticalCombat... |
| 37 | BrineWaterSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 38 | CaregivingSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | **No Godot host session** — Core logic exists but no host wi... |
| 39 | CensusClaimSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 40 | CohortSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 41 | CoalitionCampSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 42 | ContractorRosterSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 43 | CrossingArbitrationSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 44 | DecontaminationSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 45 | District8DeepCoastSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 46 | ExcavationSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 47 | ExpeditionVehicleSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | **No Godot host session** — Core logic exists but no host wi... |
| 48 | IdeologicalFrictionSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | **No Godot host session** — Core logic exists but no host wi... |
| 49 | KitchenNutritionSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 50 | LeadershipSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | **No Godot host session** — Core logic exists but no host wi... |
| 52 | MedicalWardSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 53 | MentalHealthCrisisSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 54 | MoralBranchingSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 55 | PhantomMemorySystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — unverified orphan Core system | **No Godot host session; no tests** |
| 56 | PowerGridSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 57 | RationConflictSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | **No Godot host session** — Core logic exists but no host wi... |
| 58 | ShelterThermalSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 59 | SomaticFlashbackSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 60 | SumpFloodingSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 61 | AbyssalAnomaliesCatalog | LIVE_CORE | LOW | None |
| 62 | AirlockSecurityHostSession | LIVE_GODOT | LOW (thin wrapper) | No Core class; host-only session |
| 63 | ApicultureBeeCatalog | LIVE_CORE | LOW | None |
| 64 | ApprenticeshipHostSession | LIVE_GODOT | LOW | None |
| 65 | ArchiveDeskHostSession | LIVE_GODOT | LOW | None |
| 66 | AutopsyHostSession | LIVE_GODOT | LOW | None |
| 67 | BlackProjectsCatalog | LIVE_CORE | LOW | None |
| 68 | BoneHornCarvingCatalog | LIVE_CORE | LOW | None |
| 69 | BunkerBlueprintCatalog | LIVE_CORE + LIVE_GODOT | LOW | None |
| 70 | BunkerContrabandCatalog | LIVE_CORE | LOW | None |
| 71 | BunkerCourtCatalog | LIVE_CORE | LOW | None |
| 72 | BunkerGraffitiCatalog | LIVE_CORE | LOW | None |
| 73 | BunkerMaintenanceCatalog | LIVE_CORE | LOW | None |
| 74 | CandleMakingWaxCatalog | LIVE_CORE | LOW | None |
| 75 | CatalogFileSystem | LIVE_CORE (infrastructure) | LOW | No dedicated unit tests |
| 76 | CeramicsKilnCatalog | LIVE_CORE | LOW | None |
| 77 | CharcoalPyrolysisCatalog | LIVE_CORE | LOW | None |
| 78 | ChemicalDependencySystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 79 | ColdCountSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 80 | CombatCatalog | LIVE_CORE | LOW | None |
| 81 | CombatHostSession | LIVE_GODOT | LOW | None |
| 82 | CombatTraumaSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 83 | ContractorRosterHostSession | LIVE_GODOT | LOW | None |
| 84 | CordageCableCatalog | LIVE_CORE | LOW | None |
| 85 | CourierDispatchCatalog | LIVE_CORE | LOW | None |
| 86 | CraftingHostSession | LIVE_GODOT | LOW | None |
| 87 | CrossingCatalog | LIVE_CORE | LOW | None |
| 88 | CrucibleFoundryCatalog | LIVE_CORE | LOW | None |
| 89 | CryoPreservationCatalog | LIVE_CORE | LOW | None |
| 90 | CulinaryRationCatalog | LIVE_CORE | LOW | None |
| 91 | CurrentsCatalog | LIVE_CORE + LIVE_GODOT | LOW | None |
| 92 | CurrentsPamphletCatalog | LIVE_CORE | LOW | None |
| 93 | DailySurvivalCatalog | LIVE_CORE | LOW | None |
| 94 | DeadHandDirectiveCatalog | LIVE_CORE | LOW | None |
| 95 | DecontaminationHostSession | LIVE_GODOT | LOW | None |
| 96 | DeepCoastHostSession | LIVE_GODOT | LOW | None |
| 97 | DiseaseCatalog | LIVE_CORE + LIVE_GODOT | LOW | None |
| 98 | DiseaseHostSession | LIVE_GODOT | LOW | None |
| 99 | DiveSiteCatalog | LIVE_CORE + LIVE_GODOT | LOW | None |
| 100 | DoorEncounterSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 101 | DoseContentCatalog | LIVE_CORE + LIVE_GODOT | LOW | None |
| 102 | DoseLedgerHostSession | LIVE_GODOT | LOW | None |
| 103 | DoseRegistersCatalog | LIVE_CORE + LIVE_GODOT | LOW | None |
| 104 | DutyRosterCatalog | LIVE_CORE + LIVE_GODOT | LOW | None |
| 105 | DutyRosterHostSession | LIVE_GODOT | LOW | None |
| 106 | DwellerHeirloomCatalog | LIVE_CORE | LOW | None |
| 107 | DwellerMedicalCatalog | LIVE_CORE | LOW | None |
| 108 | EconomyHostSession | LIVE_GODOT | LOW | None |
| 109 | EncounterCatalog | LIVE_CORE + LIVE_GODOT | LOW | None |
| 110 | EquipmentConditionHostSession | LIVE_GODOT | LOW | None |
| 111 | ExcavationHostSession | LIVE_GODOT | LOW | None |
| 112 | ExpansionEnrichmentCatalog | LIVE_CORE + LIVE_GODOT | LOW | None |
| 113 | ExpansionHostSession | LIVE_GODOT (orchestrator) | LOW | None |
| 114 | ExpeditionHostSession | LIVE_GODOT | LOW | None |
| 115 | FactionIconCatalog | LIVE_CORE + LIVE_GODOT | LOW | None |
| 116 | FactionWarContentCatalog | LIVE_CORE + LIVE_GODOT | LOW | None |
| 117 | FaunaEntomologyCatalog | LIVE_CORE | LOW | None |
| 118 | FermentationYeastCatalog | LIVE_CORE | LOW | None |
| 119 | FinalWishSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 120 | FringeCultsCatalog | LIVE_CORE | LOW | None |
| 121 | GeologicalStrataCatalog | LIVE_CORE | LOW | None |
| 122 | GhostTransmissionCatalog | LIVE_CORE | LOW | No tests for content catalog |
| 123 | GlassblowingDistillationCatalog | LIVE_CORE | LOW | None |
| 124 | GoodsCatalog | LIVE_CORE + LIVE_GODOT | LOW | None |
| 125 | GrainMillingCatalog | LIVE_CORE | LOW | None |
| 126 | GreenhouseExpansionCatalog | LIVE_CORE + LIVE_GODOT | LOW | None |
| 127 | GreenhouseHostSession | LIVE_GODOT | LOW | None |
| 128 | GuiltInsomniaSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 129 | HoldfastCatalog | LIVE_CORE + LIVE_GODOT | LOW | None |
| 130 | HoldfastFactionsCatalog | LIVE_CORE + LIVE_GODOT | LOW | No dedicated tests |
| 131 | HoldfastItemsCatalog | LIVE_CORE | LOW | No dedicated tests |
| 132 | HoldfastQuestSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 133 | HydroBaronsSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 134 | HydroGeologyCatalog | LIVE_CORE | LOW | None |
| 135 | IceRoadSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 136 | IndustrialRuinsCatalog | LIVE_CORE | LOW | None |
| 137 | InventoryHostSession | LIVE_GODOT (central hub) | LOW | No dedicated tests for inventory host session |
| 138 | IronRaidersSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 139 | JournalSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 140 | KitchenNutritionHostSession | LIVE_GODOT | LOW | None |
| 141 | LandmarkDegradationSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 142 | LedgerDebtSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 143 | LibraryStudyHostSession | LIVE_GODOT | LOW | None |
| 144 | LibraryStudySystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 145 | LocationEvolutionSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 146 | LocationLayoutSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 147 | LocationMemorySystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 148 | LongWalkSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 149 | LostTechManualCatalog | LIVE_CORE | LOW | None |
| 150 | MachineLogSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 151 | MaritimeDiveSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | **No Godot host session** — Core logic exists but no host wi... |
| 152 | MaritimeHostSession | LIVE_GODOT | LOW | None |
| 153 | MasonryBrickworksCatalog | LIVE_CORE | LOW | None |
| 154 | MaterialShieldingSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 155 | MedicalHostSession | LIVE_GODOT | LOW | None |
| 156 | MedicalPathologyCatalog | LIVE_CORE | LOW | None |
| 157 | MemorialSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 158 | MentalHealthCrisisHostSession | LIVE_GODOT | LOW | None |
| 159 | MetallurgyToolingCatalog | LIVE_CORE | LOW | None |
| 160 | MilitaryArmoryCatalog | LIVE_CORE | LOW | None |
| 161 | MoraleMarkSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 162 | MusterHostSession | LIVE_GODOT | LOW | None |
| 163 | MusterSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 164 | NarrativeEncounterSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 165 | NarrativeHostSession | LIVE_GODOT | LOW | None |
| 166 | NightWatchCatalog | LIVE_CORE | LOW | None |
| 167 | OpticsGlassworksCatalog | LIVE_CORE | LOW | None |
| 168 | OralLoreCatalog | LIVE_CORE | LOW | No tests for content catalog |
| 169 | OrbitalHarrowTelemetrySystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | **No Godot host session** — Core logic exists but no host wi... |
| 170 | PaperMakingCatalog | LIVE_CORE | LOW | None |
| 171 | PaperPrintingCatalog | LIVE_CORE | LOW | None |
| 172 | PhantomMemoryHostSession | LIVE_GODOT | LOW | None |
| 173 | PharmaLabSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | **No Godot host session** — Core logic exists but no host wi... |
| 174 | Phase0HostSession | LIVE_GODOT (central hub) | LOW | None |
| 175 | PneumaticTubeDispatchCatalog | LIVE_CORE | LOW | None |
| 176 | PolymerTextileCatalog | LIVE_CORE | LOW | None |
| 177 | PowerGridHostSession | LIVE_GODOT | LOW | None |
| 178 | ProceduralScavengeSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 179 | ProvisionedSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 180 | PsychologicalContaminationSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 181 | QuestlineMasterCatalog | LIVE_CORE + LIVE_GODOT | LOW | None |
| 182 | RadiationSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 183 | RadioHostSession | LIVE_GODOT | LOW | None |
| 184 | RadioScriptbookCatalog | LIVE_CORE | LOW | No tests for content catalog |
| 185 | ReckoningSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 186 | RefrigerationFermentationCatalog | LIVE_CORE | LOW | None |
| 187 | RegionalTreatyCatalog | LIVE_CORE | LOW | None |
| 188 | RegionalTreatyHostSession | LIVE_GODOT | LOW | None |
| 189 | RegionalTreatySystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 190 | RelicProvenanceCatalog | LIVE_CORE | LOW | None |
| 191 | ResearchHostSession | LIVE_GODOT | LOW | None |
| 192 | ResearchSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 193 | RespiratoryDegenerationSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 194 | RopeMakingCordageCatalog | LIVE_CORE | LOW | None |
| 195 | ScavengerGuildSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 196 | SeedBankPreservationCatalog | LIVE_CORE | LOW | None |
| 197 | ShelterAssignmentHostSession | LIVE_GODOT | LOW | None |
| 198 | ShelterAssignmentSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 199 | ShelterEncounterSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 200 | ShelterScheduleHostSession | LIVE_GODOT | LOW | None |
| 201 | ShelterScheduleSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 202 | ShelterThermalHostSession | LIVE_GODOT | LOW | None |
| 203 | SickListSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 204 | SignalIntelligenceCatalog | LIVE_CORE | LOW | None |
| 205 | SilentFoundryCatalog | LIVE_CORE + LIVE_GODOT | LOW | None |
| 206 | SilentFoundryHostSession | LIVE_GODOT | LOW | None |
| 207 | SiteEncounterSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 208 | SkillAtrophySystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | **No Godot host session** — Core logic exists but no dedicat... |
| 209 | SkillProgressionSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 210 | SkyLayerArmorSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 212 | StandingRecordCatalog | LIVE_CORE + LIVE_GODOT | LOW | None |
| 214 | StartingLevelHostSession | LIVE_GODOT | LOW | None |
| 215 | StartingLevelSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 219 | SurvivorCatalog | LIVE_CORE + LIVE_GODOT | LOW | None |
| 222 | SurvivorRelationsSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 229 | TradeSpecialtySystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 230 | TraumaBondSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | **No Godot host session** — Core logic exists but no host wi... |
| 232 | TravelingCaravanSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 233 | UndergroundFungiCatalog | LIVE_CORE | LOW | None |
| 234 | UtilityAiHostSession | LIVE_GODOT | LOW | None |
| 235 | VentilationSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 236 | VerdictHostSession | LIVE_GODOT | LOW | None |
| 237 | VerdictNpcSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 238 | VerdictRadioSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 240 | VinylMoraleSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 242 | VoluntaryRegisterSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 243 | VouchAccessSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 244 | WarlordDoctrineCatalog | LIVE_CORE + LIVE_GODOT | LOW | None |
| 245 | WastelandBestiaryCatalog | LIVE_CORE | LOW | None |
| 246 | WastelandCartographyCatalog | LIVE_CORE | LOW | None |
| 247 | WastelandExpeditionCatalog | LIVE_CORE | LOW | None |
| 248 | WastelandGazetteerCatalog | LIVE_CORE | LOW | None |
| 249 | WastelandMapSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 252 | WaterTreatmentSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 253 | WaystationHostSession | LIVE_GODOT | LOW | None |
| 254 | WaystationSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 255 | WeaponConditionSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | **No Godot host session** — Core logic exists but no host wi... |
| 256 | WeatherStationSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | **No Godot host session** — Core logic exists but no host wi... |
| 257 | WildlifeMigrationSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 259 | WildlifeTrappingSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 261 | WitnessCatalog | LIVE_CORE + LIVE_GODOT | LOW | None |
| 262 | WorkshopReverseEngineeringSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | **No Godot host session** — Core logic exists but no host wi... |
| 263 | WorldHostSession | LIVE_GODOT (central hub) | LOW | No dedicated tests for world host session |
| 264 | YearOfAshDeepFreezeSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 265 | YearOfAshHostSession | LIVE_GODOT (orchestrator) | LOW | None |
| 266 | YearOfAshRadonSystem | LIVE_CORE + LIVE_GODOT | LOW | None |
| 267 | YearOfAshTimelineSystem | LIVE_CORE + LIVE_GODOT | LOW | None |

## Orphan Core Systems (Need Godot Host Wiring)

| # | Subsystem | Classification | Risk | Evidence |
|---|-----------|---------------|------|----------|
| 36 | BallisticsSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — unhosted Core physics | `Assets/Ashfall.Core/Combat/BallisticsSystem.cs`, `Combat/TacticalCombatSystem.c |
| 38 | CaregivingSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` |
| 47 | ExpeditionVehicleSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` |
| 48 | IdeologicalFrictionSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | `Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs` |
| 50 | LeadershipSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` |
| 55 | PhantomMemorySystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — unverified orphan Core system | `Assets/Ashfall.Core/PhantomMemoryEngine.cs` |
| 57 | RationConflictSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | `Assets/Ashfall.Core/Survivors/RationConflictSystem.cs` |
| 151 | MaritimeDiveSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | `Assets/Ashfall.Core/MaritimeDiveSystem.cs` |
| 169 | OrbitalHarrowTelemetrySystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs` |
| 173 | PharmaLabSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | `Assets/Ashfall.Core/PharmaLabSystem.cs` |
| 208 | SkillAtrophySystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | `Assets/Ashfall.Core/Survivors/SkillAtrophySystem.cs`, `SkillProgressionState.cs |
| 230 | TraumaBondSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | `Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs` |
| 255 | WeaponConditionSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | `Assets/Ashfall.Core/Combat/WeaponConditionSystem.cs` |
| 256 | WeatherStationSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | `Assets/Ashfall.Core/WeatherStationSystem.cs` |
| 262 | WorkshopReverseEngineeringSystem | LIVE_CORE, PORTED_NOT_WIRED | MEDIUM — orphan Core system | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` |

## Test Coverage Gaps

| # | Subsystem | Classification | Tests |
|---|-----------|---------------|-------|
| 55 | PhantomMemorySystem | LIVE_CORE, PORTED_NOT_WIRED | 0 test files |
| 62 | AirlockSecurityHostSession | LIVE_GODOT | 0 test files |
| 64 | ApprenticeshipHostSession | LIVE_GODOT | 0 test files |
| 65 | ArchiveDeskHostSession | LIVE_GODOT | 0 test files |
| 66 | AutopsyHostSession | LIVE_GODOT | 0 test files |
| 81 | CombatHostSession | LIVE_GODOT | 0 test files |
| 83 | ContractorRosterHostSession | LIVE_GODOT | 0 test files |
| 86 | CraftingHostSession | LIVE_GODOT | 0 test files |
| 92 | CurrentsPamphletCatalog | LIVE_CORE | 0 test files |
| 95 | DecontaminationHostSession | LIVE_GODOT | 0 test files |
| 96 | DeepCoastHostSession | LIVE_GODOT | 0 test files |
| 98 | DiseaseHostSession | LIVE_GODOT | 0 test files |
| 102 | DoseLedgerHostSession | LIVE_GODOT | 0 test files |
| 105 | DutyRosterHostSession | LIVE_GODOT | 0 test files |
| 108 | EconomyHostSession | LIVE_GODOT | 0 test files |
| 110 | EquipmentConditionHostSession | LIVE_GODOT | 0 test files |
| 111 | ExcavationHostSession | LIVE_GODOT | 0 test files |
| 122 | GhostTransmissionCatalog | LIVE_CORE | 0 test files |
| 127 | GreenhouseHostSession | LIVE_GODOT | 0 test files |
| 130 | HoldfastFactionsCatalog | LIVE_CORE + LIVE_GODOT | 0 test files |
| 131 | HoldfastItemsCatalog | LIVE_CORE | 0 test files |
| 137 | InventoryHostSession | LIVE_GODOT (central hub) | 0 test files |
| 140 | KitchenNutritionHostSession | LIVE_GODOT | 0 test files |
| 143 | LibraryStudyHostSession | LIVE_GODOT | 0 test files |
| 152 | MaritimeHostSession | LIVE_GODOT | 0 test files |
| 155 | MedicalHostSession | LIVE_GODOT | 0 test files |
| 158 | MentalHealthCrisisHostSession | LIVE_GODOT | 0 test files |
| 162 | MusterHostSession | LIVE_GODOT | 0 test files |
| 165 | NarrativeHostSession | LIVE_GODOT | 0 test files |
| 168 | OralLoreCatalog | LIVE_CORE | 0 test files |
| 172 | PhantomMemoryHostSession | LIVE_GODOT | 0 test files |
| 177 | PowerGridHostSession | LIVE_GODOT | 0 test files |
| 183 | RadioHostSession | LIVE_GODOT | 0 test files |
| 184 | RadioScriptbookCatalog | LIVE_CORE | 0 test files |
| 188 | RegionalTreatyHostSession | LIVE_GODOT | 0 test files |
| 191 | ResearchHostSession | LIVE_GODOT | 0 test files |
| 197 | ShelterAssignmentHostSession | LIVE_GODOT | 0 test files |
| 200 | ShelterScheduleHostSession | LIVE_GODOT | 0 test files |
| 202 | ShelterThermalHostSession | LIVE_GODOT | 0 test files |
| 206 | SilentFoundryHostSession | LIVE_GODOT | 0 test files |
| 214 | StartingLevelHostSession | LIVE_GODOT | 0 test files |
| 234 | UtilityAiHostSession | LIVE_GODOT | 0 test files |
| 236 | VerdictHostSession | LIVE_GODOT | 0 test files |
| 253 | WaystationHostSession | LIVE_GODOT | 0 test files |
| 263 | WorldHostSession | LIVE_GODOT (central hub) | 0 test files |
| 265 | YearOfAshHostSession | LIVE_GODOT (orchestrator) | 0 test files |

## Classification Breakdown

- **LIVE_CORE + LIVE_GODOT:** 125
- **LIVE_CORE:** 59
- **LIVE_GODOT:** 38
- **LIVE_CORE, PORTED_NOT_WIRED:** 15
- **LIVE_GODOT (central hub):** 3
- **LIVE_GODOT (orchestrator):** 2
- **LIVE_CORE (infrastructure):** 1

## Risk Breakdown

- **LOW:** 224
- **MEDIUM — orphan Core system:** 13
- **MEDIUM:** 2
- **HIGH — host-core duplication:** 1
- **MEDIUM — unhosted Core physics:** 1
- **MEDIUM — unverified orphan Core system:** 1
- **LOW (thin wrapper):** 1

## Source Reports

- `docs/forensics/30_SUBSYSTEMS_FORENSIC_REPORT.md` (1–30)
- `docs/forensics/30_SUBSYSTEMS_FORENSIC_REPORT_2.md` (31–60)
- `docs/forensics/50_SUBSYSTEMS_FORENSIC_REPORT_3.md` (61–110)
- `docs/forensics/50_SUBSYSTEMS_FORENSIC_REPORT_4.md` (111–160)
- `docs/forensics/50_SUBSYSTEMS_FORENSIC_REPORT_5.md` (161–210)
- `docs/forensics/57_SUBSYSTEMS_FORENSIC_REPORT_FINAL.md` (211–267)
