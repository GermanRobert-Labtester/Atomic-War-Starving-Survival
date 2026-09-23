# PLAN-ARCHITECTURE-BOUNDARY-31 — Appendix A: Core IO Site Inventory

**Generated:** 2026-09-21. Every direct IO call in `Assets/Ashfall.Core/**`
(672 call sites across 127 files).
**Families:** `loader` (catalog/loader files) · `demo` (headless demos) ·
`other`.
**Verdicts:** `MIGRATE` (direct read → `IFileSystem` port) · `REVIEW`
(write/delete from Core) · `KEEP` (dev-only demo) · `ALLOWED` (`Path.*`
helpers — confirm no user-path construction).
**Use:** AB-31A/31B — the classification input for the boundary migration; the
gate allowlist starts from the KEEP/ALLOWED rows.

## Inventory (by family, descending call count)

| File | Calls | APIs used | Family | Verdict |
|---|---:|---|---|---|
| `Assets/Ashfall.Core/InfrastructureHeadlessDemo.cs` | 4 | `Path.Combine` | demo | KEEP (dev-only demo; record reason) |
| `Assets/Ashfall.Core/Foundry/SilentFoundryHeadlessDemo.cs` | 3 | `Path.Combine` | demo | KEEP (dev-only demo; record reason) |
| `Assets/Ashfall.Core/LedgerDebtHeadlessDemo.cs` | 1 | `Directory.GetCurrentDirectory` | demo | KEEP (dev-only demo; record reason) |
| `Assets/Ashfall.Core/Muster/FactionEcologyHeadlessDemo.cs` | 1 | `Directory.GetCurrentDirectory` | demo | KEEP (dev-only demo; record reason) |
| `Assets/Ashfall.Core/Narrative/TravelEncounterHeadlessDemo.cs` | 1 | `Directory.GetCurrentDirectory` | demo | KEEP (dev-only demo; record reason) |
| `Assets/Ashfall.Core/Endgame/EndgameHeadlessDemo.cs` | 1 | `Path.Combine` | demo | KEEP (dev-only demo; record reason) |
| `Assets/Ashfall.Core/Quests/PersonalQuestHeadlessDemo.cs` | 1 | `Path.Combine` | demo | KEEP (dev-only demo; record reason) |
| `Assets/Ashfall.Core/Narrative/BlackProjectsCatalog.cs` | 19 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/HydroGeologyCatalog.cs` | 19 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` | 18 | `Path.Combine`, `Path.GetFileName` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Narrative/AbyssalAnomaliesCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/ApicultureBeeCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/CharcoalPyrolysisCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/CordageCableCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/CrucibleFoundryCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/CryoPreservationCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/FaunaEntomologyCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/FermentationYeastCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/FringeCultsCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/GlassblowingDistillationCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/GrainMillingCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/IndustrialRuinsCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/MasonryBrickworksCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/MedicalPathologyCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/MetallurgyToolingCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/MilitaryArmoryCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/OpticsGlassworksCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/PaperMakingCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/PaperPrintingCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/PneumaticTubeDispatchCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/PolymerTextileCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/RefrigerationFermentationCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/SeedBankPreservationCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/SignalIntelligenceCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/SoapSaponificationCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/SteamTurbinePowerCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/StructuralFortificationCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/TanningLeatherCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/TimberCarpentryCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/TimekeepingHorologyCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/WastelandCartographyCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/WaterTreatmentPotableCatalog.cs` | 13 | `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/DailySurvivalCatalog.cs` | 5 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Shelter/WallCarvingCatalog.cs` | 3 | `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Survivors/GuiltSourceCatalog.cs` | 3 | `File.Exists`, `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/World/SettlementCatalog.cs` | 3 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs` | 3 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Narrative/BoneHornCarvingCatalog.cs` | 2 | `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/BunkerContrabandCatalog.cs` | 2 | `File.Exists`, `File.ReadAllText` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/CandleMakingWaxCatalog.cs` | 2 | `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/CeramicsKilnCatalog.cs` | 2 | `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/RopeMakingCordageCatalog.cs` | 2 | `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/TanningLeatherworkCatalog.cs` | 2 | `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/TextileSpinningWeavingCatalog.cs` | 2 | `File.ReadAllText`, `Path.Combine` | loader | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/NarrativeDiscoveryCatalog.cs` | 2 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Radio/RadioStationCatalog.cs` | 2 | `File.Exists`, `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/RegionalTreatyCatalogLoader.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Crafting/ChemicalSynthesisCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/DutyRoster/DutyRosterCatalog.cs` | 1 | `Path.GetFileName` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Economy/CaravanTradeRouteCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Economy/CaravanCatalogLoader.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Expeditions/ArmoredCrawlerModuleCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Expeditions/ReconTelemetryCatalogLoader.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Expeditions/ScavengingTableCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Medical/SurgicalProcedureCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Narrative/TravelEncounterCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Narrative/NarrativeProgressionCatalogLoader.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Radio/RadioStationCatalogLoader.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Radio/RadioProgramCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Shelter/CaptiveInterrogationCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Shelter/PowerSubgridCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Shelter/FoodPreservationCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Shelter/GeothermalCatalogLoader.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Shelter/HydroponicCropCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Shelter/NuclearCoreCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Shelter/CascadeRuleCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Shelter/MachineIdentity/ShelterMachineTellCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Shelter/RoomIdentity/ShelterRoomIdentityCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/World/FactionTerritoryCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/World/FieldGuideCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/World/WeatherHardeningCatalogLoader.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Endgame/EpilogueChronicleCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Foundry/GlassworksCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Foundry/MetallurgyHeavyCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Research/PrewarArchiveCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Campaign/CampaignEpilogueCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Excavation/ExcavationCatalogLoader.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Factions/InfiltratorCatalogLoader.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Farming/CropStrainCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Waystation/WaystationCatalogLoader.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Defense/PerimeterDefenseCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/NpcArcs/NpcArcCatalog.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Commitments/CommitmentCatalogLoader.cs` | 1 | `Path.Combine` | loader | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs` | 28 | `Directory.Exists`, `Directory.GetDirectories`, `Directory.GetFiles`, `File.Exists`, `File.ReadAllText`, `Path.Combine`, `Path.GetFileName` | other | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/HostDefaults.cs` | 16 | `Directory.CreateDirectory`, `Directory.Exists`, `Directory.GetCurrentDirectory`, `Directory.GetFiles`, `File.Exists`, `File.ReadAllText`, `File.WriteAllText`, `Path.Combine`, `Path.GetDirectoryName`, `Path.GetFullPath` | other | REVIEW (writes/deletes from Core) |
| `Assets/Ashfall.Core/Ports.cs` | 9 | `Directory.CreateDirectory`, `Directory.Delete`, `Directory.Exists`, `Directory.GetDirectories`, `Directory.GetFiles`, `File.Delete`, `File.Exists`, `Path.GetDirectoryName`, `Path.GetFileName` | other | REVIEW (writes/deletes from Core) |
| `Assets/Ashfall.Core/Content/ContentUtilizationManifest.cs` | 8 | `Directory.CreateDirectory`, `Directory.Exists`, `File.WriteAllText`, `Path.GetDirectoryName` | other | REVIEW (writes/deletes from Core) |
| `Assets/Ashfall.Core/Content/ContentUtilizationGate.cs` | 6 | `Directory.CreateDirectory`, `Directory.Exists`, `File.Exists`, `File.ReadAllText`, `File.WriteAllText`, `Path.GetDirectoryName` | other | REVIEW (writes/deletes from Core) |
| `Assets/Ashfall.Core/Economy/MercenarySystem.cs` | 4 | `File.Exists`, `File.ReadAllText`, `Path.Combine` | other | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Survivors/DesperationSystem.cs` | 4 | `File.Exists`, `File.ReadAllText`, `Path.Combine` | other | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/World/FalloutSystem.cs` | 4 | `File.Exists`, `File.ReadAllText`, `Path.Combine` | other | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Archaeology/ArchaeologySystem.cs` | 4 | `File.Exists`, `File.ReadAllText`, `Path.Combine` | other | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/VersionReport.cs` | 3 | `Directory.EnumerateFiles`, `Directory.Exists`, `File.ReadAllText` | other | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Narrative/Continuity/NarrativeContinuityEngine.cs` | 3 | `File.Exists`, `File.ReadAllText`, `Path.Combine` | other | MIGRATE (direct read → IFileSystem port) |
| `Assets/Ashfall.Core/Save/SaveEnvelopeHelper.cs` | 3 | `File.Exists`, `File.Move`, `Path.GetDirectoryName` | other | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/ExpansionMasterSession.cs` | 2 | `Path.Combine` | other | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Mods/ModDataContract.cs` | 2 | `Path.GetFileName` | other | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/ExpansionQuestSystem.cs` | 1 | `Path.Combine` | other | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Expeditions/RunFlatTireEngine.cs` | 1 | `Path.Combine` | other | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Radiation/LowBackgroundLeadEngine.cs` | 1 | `Path.Combine` | other | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Shelter/AquaponicsSystem.cs` | 1 | `Path.Combine` | other | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Shelter/AeroponicsSystem.cs` | 1 | `Path.Combine` | other | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Shelter/CryoVaultSystem.cs` | 1 | `Path.Combine` | other | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Shelter/GeothermalOrcSystem.cs` | 1 | `Path.Combine` | other | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Shelter/PneumaticDispatchSystem.cs` | 1 | `Path.Combine` | other | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Survivors/PsychologicalArcSystem.cs` | 1 | `Path.Combine` | other | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/World/InSarDeformationEngine.cs` | 1 | `Path.Combine` | other | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` | 1 | `Path.Combine` | other | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/World/WorldEvolutionEngine.cs` | 1 | `Path.Combine` | other | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` | 1 | `Path.Combine` | other | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Foundry/HydraulicExtrusionEngine.cs` | 1 | `Path.Combine` | other | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Foundry/SilentFoundryConsequencePolicy.cs` | 1 | `Path.Combine` | other | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Combat/CombatBreachingEngine.cs` | 1 | `Path.Combine` | other | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Combat/BallisticsWorkbenchSystem.cs` | 1 | `Path.Combine` | other | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Farming/NutritionDiversitySystem.cs` | 1 | `Path.Combine` | other | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Save/SaveStore.cs` | 1 | `Path.Combine` | other | ALLOWED (Path.* helpers) |
| `Assets/Ashfall.Core/Defense/DefenseSystem.cs` | 1 | `Path.Combine` | other | ALLOWED (Path.* helpers) |

## Family counts

demo files: 7, loader files: 86, other files: 34

**Total call sites:** 672
