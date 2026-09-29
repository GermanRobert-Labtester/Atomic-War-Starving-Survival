# Main host decomposition map

Historical plan bundles are split into domain partials. Member bodies and method names are preserved; cross-domain lifecycle composition remains in `Main.SubsystemComposition.cs`. No dead gameplay code was inferred from missing local callers.

## Historical bundle destinations

| Previous file | Domain files |
|---|---|
| `src/Main.Plans110_113.cs` | `src/Main.BallisticShield.Integration.cs`, `src/Main.ChlorAlkali.Integration.cs`, `src/Main.PrecisionOptics.Integration.cs`, `src/Main.SolarConcentrator.Integration.cs`, `src/Main.SubsystemComposition.cs` |
| `src/Main.Plans122to125.cs` | `src/Main.AmphibiousDraisine.Integration.cs`, `src/Main.CvdDiamond.Integration.cs`, `src/Main.SofcPower.Integration.cs`, `src/Main.SoundRanging.Integration.cs`, `src/Main.SubsystemComposition.cs` |
| `src/Main.Plans126_129.cs` | `src/Main.BioFermentation.Integration.cs`, `src/Main.SubsystemComposition.cs` |
| `src/Main.Plans130_133.cs` | `src/Main.DraisineRerailing.Integration.cs`, `src/Main.Lyophilization.Integration.cs`, `src/Main.NvisCommunications.Integration.cs`, `src/Main.PowderMetallurgy.Integration.cs`, `src/Main.SubsystemComposition.cs` |
| `src/Main.Plans146_149.cs` | `src/Main.EbPvdCoating.Integration.cs`, `src/Main.MicrofluidicDiagnostic.Integration.cs`, `src/Main.MineClearingFlail.Integration.cs`, `src/Main.RailGrinding.Integration.cs`, `src/Main.RouteInfrastructure.Integration.cs`, `src/Main.SubsystemComposition.cs`, `src/Main.UiTests.IndustrialPanels.cs` |
| `src/Main.Plans147.cs` | `src/Main.ContrabandStash.Integration.cs`, `src/Main.DependencyConsumption.Integration.cs`, `src/Main.ShelterBarter.Integration.cs` |
| `src/Main.Plans152.cs` | `src/Main.BlackProjectsArchive.Integration.cs` |
| `src/Main.Plans154.cs` | `src/Main.HydroGeologyDiscovery.Integration.cs` |
| `src/Main.Plans155.cs` | `src/Main.OralLore.Integration.cs` |
| `src/Main.Plans157.cs` | `src/Main.GrainMillingArchive.Integration.cs` |
| `src/Main.Plans158.cs` | `src/Main.TechnicalMaterialArchive.Integration.cs` |
| `src/Main.Plans159.cs` | `src/Main.LeatherworkArchive.Integration.cs` |
| `src/Main.Plans162_165.cs` | `src/Main.Agriculture.Integration.cs`, `src/Main.Defense.Integration.cs`, `src/Main.PsychologyArcs.Integration.cs`, `src/Main.WildlifeEcosystem.Integration.cs` |
| `src/Main.Plans162_185.cs` | `src/Main.MemoryDecay.cs`, `src/Main.ShelterArchiveProjection.Integration.cs` |
| `src/Main.Plans163_210.cs` | `src/Main.Cartography.Integration.cs`, `src/Main.PersonalBelongings.cs` |
| `src/Main.Plans166_169.cs` | `src/Main.Espionage.Integration.cs`, `src/Main.FluidLogistics.Integration.cs`, `src/Main.ProceduralNarrative.Integration.cs`, `src/Main.ResearchProgression.Integration.cs`, `src/Main.SubsystemComposition.cs` |
| `src/Main.Plans167_219.cs` | `src/Main.SurvivorDocumentation.Integration.cs`, `src/Main.TunnelNetwork.cs` |
| `src/Main.Plans178_181.cs` | `src/Main.Generational.Integration.cs`, `src/Main.Mutations.Integration.cs`, `src/Main.Prisoners.Integration.cs`, `src/Main.Stealth.Integration.cs`, `src/Main.SubsystemComposition.cs` |
| `src/Main.Plans182_185.cs` | `src/Main.Aviation.Integration.cs`, `src/Main.ForcedLabor.Integration.cs`, `src/Main.Narcotics.Integration.cs`, `src/Main.Politics.Integration.cs`, `src/Main.SubsystemComposition.cs` |
| `src/Main.Plans186_189.cs` | `src/Main.Archaeology.Integration.cs`, `src/Main.Desperation.Integration.cs`, `src/Main.Fallout.Integration.cs`, `src/Main.Mercenary.Integration.cs`, `src/Main.SubsystemComposition.cs` |
| `src/Main.Plans190_193.cs` | `src/Main.Amputation.Integration.cs`, `src/Main.Archaeology.Integration.cs`, `src/Main.Desperation.Integration.cs`, `src/Main.Fallout.Integration.cs`, `src/Main.Fungi.Integration.cs`, `src/Main.Justice.Integration.cs`, `src/Main.Mercenary.Integration.cs`, `src/Main.Railway.Integration.cs`, `src/Main.SubsystemComposition.cs` |
| `src/Main.Plans194_197.cs` | `src/Main.NavalExpeditions.Integration.cs`, `src/Main.Recreation.Integration.cs`, `src/Main.SubsystemComposition.cs`, `src/Main.WinterSurvival.Integration.cs` |
| `src/Main.Plans198_201.cs` | `src/Main.Ceremony.Integration.cs`, `src/Main.ChemWarfare.Integration.cs`, `src/Main.CommsArray.Integration.cs`, `src/Main.Robotics.Integration.cs`, `src/Main.SubsystemComposition.cs` |
| `src/Main.Plans202_205.cs` | `src/Main.CargoAirdrop.Integration.cs`, `src/Main.PlasticPyrolysis.Integration.cs` |
| `src/Main.Plans216_202Interpersonal.cs` | `src/Main.Exercise.cs`, `src/Main.InterpersonalConflict.cs` |
| `src/Main.Plans46_49.cs` | `src/Main.DynamicQuests.cs`, `src/Main.ExcavationHazards.Integration.cs`, `src/Main.RadioStation.Integration.cs`, `src/Main.ShelterSocialDynamics.Integration.cs`, `src/Main.SubsystemComposition.cs`, `src/Main.Workshop.Integration.cs` |
| `src/Main.Plans50_53.cs` | `src/Main.ShelterAcoustics.Integration.cs`, `src/Main.ShelterEspionage.Integration.cs`, `src/Main.SubsystemComposition.cs`, `src/Main.SurvivorMentalHealth.Integration.cs`, `src/Main.VehicleGarage.cs` |
| `src/Main.Plans62_65.cs` | `src/Main.CampaignEpilogue.Integration.cs`, `src/Main.FoodPreservation.Integration.cs`, `src/Main.PrewarArchives.Integration.cs`, `src/Main.SubsystemComposition.cs` |
| `src/Main.Plans74_77.cs` | `src/Main.Aeroponics.Integration.cs`, `src/Main.BallisticsWorkbench.Integration.cs`, `src/Main.GeothermalOrc.Integration.cs`, `src/Main.PneumaticDispatch.Integration.cs`, `src/Main.SubsystemComposition.cs` |
| `src/Main.Plans78_81.cs` | `src/Main.ChemicalRecon.Integration.cs`, `src/Main.GeodeticSurvey.Integration.cs`, `src/Main.KineticStorage.Integration.cs`, `src/Main.SubsystemComposition.cs` |
| `src/Main.Plans94_97.cs` | `src/Main.CryogenicAirSeparation.Integration.cs`, `src/Main.GrainProcessing.Integration.cs`, `src/Main.Heliograph.Integration.cs`, `src/Main.SubsystemComposition.cs` |
| `src/Main.PlansB68_B69.cs` | `src/Main.CryoVault.Integration.cs`, `src/Main.SeismicDynamics.Integration.cs` |
| `src/Main.PlansB86_B89.cs` | `src/Main.Aquaponics.Integration.cs`, `src/Main.BallisticsWorkbench.Integration.cs`, `src/Main.PrecisionMetrology.Integration.cs` |
| `src/Host/HostCli.PanelTests.cs` | `src/Host/HostCli.Command.RunAssetCoverageReport.cs`, `src/Host/HostCli.Command.RunAssetRegistrySelfTest.cs`, `src/Host/HostCli.Command.RunBlackFlotillaSelfTest.cs`, `src/Host/HostCli.Command.RunBridgeSelfTest.cs`, `src/Host/HostCli.Command.RunCaravanSelfTest.cs`, `src/Host/HostCli.Command.RunDay1PlayableSelfTest.cs`, `src/Host/HostCli.Command.RunDay1ToDay2MilestoneSelfTest.cs`, `src/Host/HostCli.Command.RunDoseLedgerSelfTest.cs`, `src/Host/HostCli.Command.RunDutyRosterSaveSelfTest.cs`, `src/Host/HostCli.Command.RunEconomySelfTest.cs`, `src/Host/HostCli.Command.RunExpansionHubSaveSelfTest.cs`, `src/Host/HostCli.Command.RunExpeditionEncounterBridgeSelfTest.cs`, `src/Host/HostCli.Command.RunExpeditionSelfTest.cs`, `src/Host/HostCli.Command.RunHoldfastBriefing.cs`, `src/Host/HostCli.Command.RunHoldfastSaveSelfTest.cs`, `src/Host/HostCli.Command.RunIceRoadTickDemo.cs`, `src/Host/HostCli.Command.RunMedicalSelfTest.cs`, `src/Host/HostCli.Command.RunNarrativeSelfTest.cs`, `src/Host/HostCli.Command.RunOralLoreSelfTest.cs`, `src/Host/HostCli.Command.RunPhase0SelfTest.cs`, `src/Host/HostCli.Command.RunPlayableShellSelfTest.cs`, `src/Host/HostCli.Command.RunPowerGridCatalogSelfTest.cs`, `src/Host/HostCli.Command.RunRadioSelfTest.cs`, `src/Host/HostCli.Command.RunSettingsSelfTest.cs`, `src/Host/HostCli.Command.RunShelterHazardLoopSelfTest.cs`, `src/Host/HostCli.Command.RunShelterOperationsSelfTest.cs`, `src/Host/HostCli.Command.RunStandaloneSystemsSelfTest.cs`, `src/Host/HostCli.Command.RunSurvivorsSelfTest.cs`, `src/Host/HostCli.Command.RunUiLayoutSelfTest.cs`, `src/Host/HostCli.Command.RunUtilityAiSelfTest.cs`, `src/Host/HostCli.Command.RunWorldSelfTest.cs`, `src/Host/HostCli.Command.RunYearOfAshSaveSelfTest.cs`, `src/Host/HostCli.Command.SnapshotPaths.cs` |

## Current Main partial ownership

Namespaces below describe Core dependencies; composition and UI partials legitimately span domains.

| File | Declared Core namespace dependencies |
|---|---|
| `src/Main.AccessibilitySettings.cs` | Accessibility |
| `src/Main.AdvancedIndustrial.cs` | Inventory, Radio, Shelter, World |
| `src/Main.AdvancedShelterSystems.cs` | Defense, Economy, Expeditions, IO, Medical, Shelter |
| `src/Main.Aeroponics.Integration.cs` | Campaign, Combat, Shelter |
| `src/Main.Aging.cs` | Survivors |
| `src/Main.Agriculture.Integration.cs` | Campaign, Farming, Inventory, Random, Survivors |
| `src/Main.AmphibiousDraisine.Integration.cs` | AdvancedMachinery, Combat, Expeditions, Medical, Random, Shelter, World |
| `src/Main.Amputation.Integration.cs` | Archaeology, Expeditions, Farming, Medical, Narrative |
| `src/Main.Anomaly.cs` | Expeditions, IO, World |
| `src/Main.AntenatalMaternalHealth.cs` | Survivors |
| `src/Main.Application.cs` | Campaign, Economy, Expeditions, Foundry, IO, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.ApprenticeshipCurriculum.cs` | Education |
| `src/Main.Aquaponics.Integration.cs` | IO, Random, Shelter |
| `src/Main.Archaeology.Integration.cs` | Archaeology, Economy, Expeditions, Farming, Medical, Narrative, Survivors, World |
| `src/Main.Audio.cs` | Combat, Crafting, Disease, Excavation, Expeditions, Radiation, Radio, Shelter, StartingLevel, Survivors, World |
| `src/Main.AudioAccessibility.cs` | Audio, Campaign |
| `src/Main.Aviation.Integration.cs` | Expeditions, Factions, Medical, Narrative |
| `src/Main.Backstory.cs` | Survivors |
| `src/Main.BallisticShield.Integration.cs` | Combat, Shelter |
| `src/Main.BallisticsWorkbench.Integration.cs` | Campaign, Combat, IO, Random, Shelter |
| `src/Main.Bestiary.cs` | Bestiary |
| `src/Main.BioFermentation.Integration.cs` | Inventory, Shelter |
| `src/Main.Bionics.cs` | Medical, Random |
| `src/Main.BlackMarket.cs` | Economy |
| `src/Main.BlackProjectsArchive.Integration.cs` | Expeditions, Maritime, Narrative |
| `src/Main.BriefingCrisis.cs` | Campaign, Inventory |
| `src/Main.BroadsheetPress.cs` | Print, Radiation, Survivors |
| `src/Main.Campaign.cs` | Campaign, Expeditions, Memorial, Spiritual, Survivors, UI |
| `src/Main.CampaignActionLog.cs` | PlayerCommand |
| `src/Main.CampaignEpilogue.Integration.cs` | Campaign, Disease, Research, Shelter |
| `src/Main.CampaignLegacy.cs` | Legacy |
| `src/Main.CampaignOwners.cs` | Campaign, Economy, Expeditions, Farming, Nutrition, PlayerCommand, Shelter |
| `src/Main.CampaignServices.cs` |  |
| `src/Main.CargoAirdrop.Integration.cs` | Shelter, World |
| `src/Main.Cartography.Integration.cs` | Exploration, Inventory, Random, Survivors |
| `src/Main.Cascade.cs` | Campaign, Shelter, World |
| `src/Main.CassettePlayback.cs` | Audio |
| `src/Main.Ceremony.Integration.cs` | Campaign, Combat, Crafting, Inventory, Narrative, Survivors, World |
| `src/Main.ChemicalPlume.cs` | Combat |
| `src/Main.ChemicalReagentSynthesis.cs` | Shelter |
| `src/Main.ChemicalRecon.Integration.cs` | Expeditions, Inventory, Shelter, World |
| `src/Main.ChemicalSynthesis.cs` | Crafting, IO |
| `src/Main.ChemWarfare.Integration.cs` | Campaign, Combat, Crafting, Inventory, Narrative, Survivors, World |
| `src/Main.ChildDevelopment.cs` | Survivors |
| `src/Main.ChlorAlkali.Integration.cs` | Combat, Shelter |
| `src/Main.ChronicConditions.cs` | Medical |
| `src/Main.CipherQuestChain.cs` | Radio |
| `src/Main.ClinicalWardTriage.cs` | Medical |
| `src/Main.ClothingWarmth.cs` | Inventory |
| `src/Main.CloudSeeding.cs` | World |
| `src/Main.Codex.cs` | Codex, IO, World |
| `src/Main.Collectibles.cs` | IO, Inventory |
| `src/Main.Commitments.cs` | Campaign, Commitments |
| `src/Main.CommonTableRationing.cs` | Nutrition |
| `src/Main.CommsArray.Integration.cs` | Campaign, Combat, Crafting, Inventory, Narrative, Survivors, World |
| `src/Main.Companion.cs` | Ecology, IO, Random |
| `src/Main.ContentCertification.cs` |  |
| `src/Main.ContrabandStash.Integration.cs` | Economy, Inventory, Medical, Narrative |
| `src/Main.Cooking.cs` | Cooking, Random, Save |
| `src/Main.CryogenicAirSeparation.Integration.cs` | Radio |
| `src/Main.CryoVault.Integration.cs` | Shelter |
| `src/Main.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.CultureCreation.cs` | Culture |
| `src/Main.CvdDiamond.Integration.cs` | Combat, Expeditions, Random, Shelter |
| `src/Main.DayRecord.cs` | Campaign |
| `src/Main.DebtCredit.cs` | Campaign, Economy, Inventory |
| `src/Main.DeepWell.cs` | Shelter |
| `src/Main.Defense.Integration.cs` | Campaign, Farming, Inventory, Random, Survivors |
| `src/Main.DependencyConsumption.Integration.cs` | Economy, Inventory, Medical, Narrative |
| `src/Main.DependencyTaperWithdrawal.cs` | Medical |
| `src/Main.Desperation.Integration.cs` | Archaeology, Economy, Expeditions, Farming, Medical, Narrative, Survivors, World |
| `src/Main.Difficulty.cs` | Campaign, Difficulty, Save |
| `src/Main.DifficultySettings.cs` | Difficulty |
| `src/Main.Diplomacy.cs` | Diplomacy |
| `src/Main.DraisineRerailing.Integration.cs` | Expeditions, Foundry, Medical, Radio, Random |
| `src/Main.DreamSystem.cs` | Random, Survivors |
| `src/Main.DutyRoster.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.DynamicQuestGeneration.cs` | Quests |
| `src/Main.DynamicQuests.cs` | Campaign, Excavation, IO, Quests, Radio, Shelter, Survivors |
| `src/Main.EbPvdCoating.Integration.cs` | AdvancedMachinery, Expeditions, Medical, Shelter, World |
| `src/Main.Echoes.cs` | Flags, Narrative, Radiation, Survivors |
| `src/Main.EcologicalInfestations.cs` | Campaign, Disease, Ecology, Random, Survivors, World |
| `src/Main.Economy.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.EconomyFamily.cs` |  |
| `src/Main.EmergencyAlerts.cs` | Emergency |
| `src/Main.EmergencyMusterReadiness.cs` | Shelter |
| `src/Main.Endgame.cs` | Endgame |
| `src/Main.Enrichment.cs` | IO, Survivors |
| `src/Main.Espionage.Integration.cs` | Campaign, Factions, Narrative, Quests, Random, Shelter, Survivors |
| `src/Main.EvolvingWorld.cs` | Random |
| `src/Main.ExcavationHazards.Integration.cs` | Campaign, Excavation, IO, Quests, Radio, Shelter, Survivors |
| `src/Main.Exercise.cs` | Random, Survivors |
| `src/Main.ExpandedShelterSystems.cs` | Crafting, Disease, Expeditions, Inventory, Journal, Medical, Radiation, Shelter, StartingLevel, Survivors, World, YearOfAsh |
| `src/Main.ExpansionHub.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.ExpeditionFamily.cs` |  |
| `src/Main.Expeditions.cs` | Campaign, Combat, Disease, Economy, Expeditions, Foundry, IO, Inventory, Journal, Muster, Narrative, Radio, Random, Survivors, YearOfAsh |
| `src/Main.FactionBranch.cs` | Factions |
| `src/Main.Fallout.Integration.cs` | AdvancedMachinery, Archaeology, Economy, Expeditions, Farming, Medical, Narrative, Shelter, Survivors, World |
| `src/Main.FlagshipInstitutions.cs` | Campaign, Catalogs, Culture, Diplomacy, Institutions, Lifecycle, Memorial, Sanatorium, Shelter, SkyDefense, Survivors |
| `src/Main.FlagshipPanels.cs` |  |
| `src/Main.FluidLogistics.Integration.cs` | Campaign, Factions, Narrative, Quests, Random, Shelter, Survivors |
| `src/Main.FoodPreservation.Integration.cs` | Campaign, Disease, Research, Shelter |
| `src/Main.ForcedLabor.Integration.cs` | Expeditions, Factions, Medical, Narrative |
| `src/Main.Fungi.Integration.cs` | Archaeology, Expeditions, Farming, Medical, Narrative |
| `src/Main.GameFlow.cs` | Campaign, Difficulty, Expeditions, Inventory, Save |
| `src/Main.Genealogy.cs` | Legacy, Survivors |
| `src/Main.Generational.Integration.cs` | Campaign, Combat, Factions, Medical, Survivors |
| `src/Main.GeodeticSurvey.Integration.cs` | Expeditions, Inventory, Shelter, World |
| `src/Main.GeothermalOrc.Integration.cs` | Campaign, Combat, Shelter |
| `src/Main.Glassworks.cs` | Optics |
| `src/Main.GrainMillingArchive.Integration.cs` | Narrative |
| `src/Main.GrainProcessing.Integration.cs` | Radio |
| `src/Main.HealthHistory.cs` | Medical |
| `src/Main.Heliograph.Integration.cs` | Radio |
| `src/Main.HiddenAgenda.cs` | Survivors |
| `src/Main.Holdfast.cs` | Campaign, Economy, Expeditions, Feedback, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.HoldfastPresentation.cs` | Presentation |
| `src/Main.HumanMigration.cs` | Economy |
| `src/Main.HydraulicExtrusion.cs` | Foundry, Random |
| `src/Main.HydroGeologyDiscovery.Integration.cs` | Narrative |
| `src/Main.IdeologicalFriction.cs` | Survivors |
| `src/Main.InSarMapping.cs` | Random, World |
| `src/Main.InternalCommunication.cs` | Communication |
| `src/Main.InterpersonalConflict.cs` | Random, Survivors |
| `src/Main.Inventory.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.ItemLore.cs` | Crafting, Inventory |
| `src/Main.JourneyDiagnostics.cs` |  |
| `src/Main.Justice.Integration.cs` | Archaeology, Expeditions, Farming, Medical, Narrative |
| `src/Main.Kilnworks.cs` | Shelter |
| `src/Main.KineticStorage.Integration.cs` | Expeditions, Inventory, Shelter, World |
| `src/Main.KnockWhitelist.cs` |  |
| `src/Main.LeatherworkArchive.Integration.cs` | Maritime, Narrative |
| `src/Main.Letters.cs` | Narrative |
| `src/Main.Lifecycle.cs` | Lifecycle, Orchestration, Save |
| `src/Main.LoanShark.cs` |  |
| `src/Main.LowBackgroundMetrology.cs` | Radiation |
| `src/Main.Lyophilization.Integration.cs` | Expeditions, Foundry, Medical, Radio, Random |
| `src/Main.Maritime.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.MechanicalDriveline.cs` | Shelter |
| `src/Main.Medical.cs` | Campaign, Economy, Expeditions, Feedback, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.MedicalTriage.cs` | Campaign, Disease, Memorial |
| `src/Main.MemoryDecay.cs` | Cognition, Shelter |
| `src/Main.Mercenary.Integration.cs` | Archaeology, Economy, Expeditions, Farming, Medical, Narrative, Survivors, World |
| `src/Main.MetaProgression.cs` | Endgame |
| `src/Main.MicrofluidicDiagnostic.Integration.cs` | AdvancedMachinery, Expeditions, Medical, Shelter, World |
| `src/Main.MigrationConsequence.cs` | Economy |
| `src/Main.MineClearingFlail.Integration.cs` | AdvancedMachinery, Expeditions, Medical, Shelter, World |
| `src/Main.ModalTravelDispatch.cs` | Weather, World |
| `src/Main.ModSupport.cs` | Mods |
| `src/Main.MoralChoice.cs` | MoralChoice |
| `src/Main.MoraleContagion.cs` | Inventory, Survivors |
| `src/Main.Muster.cs` | Campaign, Combat, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.Mutations.Integration.cs` | Campaign, Combat, Factions, Medical, Survivors |
| `src/Main.Narcotics.Integration.cs` | Expeditions, Factions, Medical, Narrative |
| `src/Main.Narrative.cs` | Campaign, Economy, Expeditions, Factions, Foundry, Inventory, Journal, Muster, Narrative, Radiation, Radio, Survivors, YearOfAsh |
| `src/Main.NarrativeQuestlines.cs` | Quests, Survivors |
| `src/Main.NavalExpeditions.Integration.cs` | Expeditions, Inventory, Recreation |
| `src/Main.NeedsPerformance.cs` | Survivors |
| `src/Main.NightWatch.cs` | Narrative, World |
| `src/Main.NpcArcs.cs` | NpcArcs |
| `src/Main.NpcMemory.cs` | Narrative |
| `src/Main.NvisCommunications.Integration.cs` | Expeditions, Foundry, Medical, Radio, Random |
| `src/Main.OilseedPressing.cs` | Farming |
| `src/Main.Onboarding.cs` | Inventory, Onboarding |
| `src/Main.OralLore.Integration.cs` | Narrative |
| `src/Main.OriginMechanics.cs` | Survivors |
| `src/Main.OrphanSealWave1.cs` | Communications, Culture, Education, Events, Expeditions, Factions, Phantoms, Random, Shelter, Survivors, Weather |
| `src/Main.OutpostSettlement.cs` | Save, Settlements, Survivors |
| `src/Main.PackageGAdapters.cs` | Survivors |
| `src/Main.PackageHBindings.cs` | Combat, Memorial, Narrative |
| `src/Main.PackageIBindings.cs` | Economy, IO, Relations, Survivors |
| `src/Main.PalliativeCare.cs` | Medical |
| `src/Main.PanelLifecycle.cs` |  |
| `src/Main.PathogenStrains.cs` | Disease, Inventory |
| `src/Main.PatrolRadio.cs` | Narrative, Radio |
| `src/Main.PerimeterEarlyWarning.cs` | Defense |
| `src/Main.PersonalBelongings.cs` | Exploration, Inventory, Random, Survivors |
| `src/Main.PersonalQuests.cs` | Quests |
| `src/Main.PfglOctetBoards.cs` | Expeditions, Survivors |
| `src/Main.PharmaceuticalTablet.cs` | Medical |
| `src/Main.Phase0.cs` | Campaign, Crafting, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.Piezometer.cs` | Shelter, Survivors, World |
| `src/Main.Plan49DepthPass.cs` |  |
| `src/Main.PlasticPyrolysis.Integration.cs` | Shelter, World |
| `src/Main.PlayerSurfaces.cs` | UI |
| `src/Main.PlayMetrics.cs` | Difficulty, Economy, Shelter, Telemetry |
| `src/Main.PneumaticDispatch.Integration.cs` | Campaign, Combat, Shelter |
| `src/Main.Politics.Integration.cs` | Expeditions, Factions, Medical, Narrative |
| `src/Main.PowderMetallurgy.Integration.cs` | Expeditions, Foundry, Medical, Radio, Random |
| `src/Main.PowerLoadShedding.cs` | Shelter, Survivors |
| `src/Main.PrecisionMetrology.Integration.cs` | IO, Random, Shelter |
| `src/Main.PrecisionOptics.Integration.cs` | Combat, Shelter |
| `src/Main.PrewarArchives.Integration.cs` | Campaign, Disease, Research, Shelter |
| `src/Main.Prisoners.Integration.cs` | Campaign, Combat, Factions, Medical, Survivors |
| `src/Main.ProceduralEulogy.cs` | Journal |
| `src/Main.ProceduralNarrative.Integration.cs` | Campaign, Factions, Narrative, Quests, Random, Shelter, Survivors |
| `src/Main.Propaganda.cs` | Propaganda |
| `src/Main.PsychologicalProfiles.cs` | Psychology |
| `src/Main.PsychologyArcs.Integration.cs` | Campaign, Farming, Inventory, Random, Survivors |
| `src/Main.PsyOps.cs` | Radio |
| `src/Main.Quests.cs` | Thirdonary |
| `src/Main.RadiationEconomy.cs` | Radiation |
| `src/Main.RadiationSocial.cs` | Radiation |
| `src/Main.RadioProgramProduction.cs` | Radio |
| `src/Main.RadioStation.Integration.cs` | Campaign, Excavation, IO, Quests, Radio, Shelter, Survivors |
| `src/Main.RailGrinding.Integration.cs` | AdvancedMachinery, Expeditions, Medical, Shelter, World |
| `src/Main.RailTrackMaintenance.cs` | Expeditions, Rail |
| `src/Main.Railway.Integration.cs` | Archaeology, Expeditions, Farming, Medical, Narrative |
| `src/Main.RationConflict.cs` | Survivors |
| `src/Main.Recreation.Integration.cs` | Expeditions, Inventory, Recreation |
| `src/Main.Recruitment.cs` | Survivors |
| `src/Main.RelationshipDecay.cs` | Survivors |
| `src/Main.ResearchProgression.Integration.cs` | Campaign, Factions, Narrative, Quests, Random, Shelter, Survivors |
| `src/Main.ResearchUnlock.cs` | Research |
| `src/Main.Retention.cs` | Records, Save, Survivors, Verdict, YearOfAsh |
| `src/Main.Robotics.Integration.cs` | Campaign, Combat, Crafting, Inventory, Narrative, Survivors, World |
| `src/Main.RomanceFamily.cs` | Random, Survivors |
| `src/Main.RouteInfrastructure.Integration.cs` | AdvancedMachinery, Expeditions, Medical, Shelter, World |
| `src/Main.RumorNetwork.cs` | InformationFlow |
| `src/Main.RunFlatTire.cs` | Expeditions, Random |
| `src/Main.Sanitation.cs` | Shelter |
| `src/Main.SaveOrchestrator.cs` | Campaign, Economy, Expeditions, Feedback, Foundry, Inventory, Journal, Muster, Radio, Save, Survivors, YearOfAsh |
| `src/Main.SecondGenerationMilestones.cs` |  |
| `src/Main.SeismicDynamics.Integration.cs` | Shelter |
| `src/Main.SessionDurability.cs` | Save |
| `src/Main.ShelterAcoustics.Integration.cs` | Audio, Expeditions, Factions, Needs |
| `src/Main.ShelterArchive.cs` | Memorial, Shelter |
| `src/Main.ShelterArchiveProjection.Integration.cs` | Cognition, Shelter |
| `src/Main.ShelterAtmosphere.cs` | Shelter, Survivors |
| `src/Main.ShelterBarter.Integration.cs` | Economy, Inventory, Medical, Narrative |
| `src/Main.ShelterBatch3.cs` | Crafting, Expeditions, Inventory, Journal, Medical, Radiation, Shelter, StartingLevel, Survivors, World, YearOfAsh |
| `src/Main.ShelterEspionage.Integration.cs` | Audio, Expeditions, Factions, Needs |
| `src/Main.ShelterGovernance.cs` | Governance |
| `src/Main.ShelterIdentity.cs` | Campaign, Shelter |
| `src/Main.ShelterInfrastructure.cs` | Crafting, Expeditions, Inventory, Journal, Medical, Radiation, Shelter, StartingLevel, Survivors, Waystation, World, YearOfAsh |
| `src/Main.ShelterMaintenance.cs` | Shelter |
| `src/Main.ShelterMuseum.cs` | Culture |
| `src/Main.ShelterOperations.cs` | Random, Settlements, Survivors, UI |
| `src/Main.ShelterReputation.cs` | Reputation |
| `src/Main.ShelterSecurity.cs` | Shelter |
| `src/Main.ShelterSocial.cs` | Crafting, Expeditions, Inventory, Journal, Medical, Narrative, Radiation, Shelter, StartingLevel, Survivors, World, YearOfAsh |
| `src/Main.ShelterSocialDynamics.Integration.cs` | Campaign, Excavation, IO, Quests, Radio, Shelter, Survivors |
| `src/Main.SkillAtrophy.cs` | Survivors |
| `src/Main.SkillCertification.cs` | Survivors |
| `src/Main.SkyDefense.cs` | SkyDefense |
| `src/Main.SleepAcousticRest.cs` | Needs |
| `src/Main.SleepNarrative.cs` | Needs, Random |
| `src/Main.SliceScenario.cs` | Campaign |
| `src/Main.SofcPower.Integration.cs` | Combat, Expeditions, Random, Shelter |
| `src/Main.SoilReclamationProfile.cs` | Farming |
| `src/Main.SolarConcentrator.Integration.cs` | Combat, Shelter |
| `src/Main.SoundRanging.Integration.cs` | Combat, Expeditions, Random, Shelter |
| `src/Main.Spiritual.cs` | Spiritual, Survivors |
| `src/Main.SpiritualRitual.cs` | Spiritual, Survivors |
| `src/Main.Stealth.Integration.cs` | Campaign, Combat, Factions, Medical, Survivors |
| `src/Main.StormForecast.cs` | World |
| `src/Main.SubsystemComposition.cs` | AdvancedMachinery, Archaeology, Audio, Campaign, Combat, Crafting, Disease, Economy, Excavation, Expeditions, Factions, Farming, Foundry, IO, Inventory, Medical, Narrative, Needs, Quests, Radio, Random, Recreation, Research, Shelter, Survivors, World |
| `src/Main.Subterranean.cs` | Expeditions, Subterranean, Survivors |
| `src/Main.SurgicalGraft.cs` | Medical |
| `src/Main.SurvivorBarter.cs` | Economy |
| `src/Main.SurvivorDeathLegacy.cs` | Survivors |
| `src/Main.SurvivorDocumentation.Integration.cs` | Culture, Inventory, Survivors, Underground |
| `src/Main.SurvivorFate.cs` | Feedback, Survivors |
| `src/Main.SurvivorFitness.cs` | Disease, DutyRoster, Radiation, Survivors |
| `src/Main.SurvivorLetterDelivery.cs` | IO, Narrative |
| `src/Main.SurvivorMentalHealth.Integration.cs` | Audio, Expeditions, Factions, Needs |
| `src/Main.SurvivorRoles.cs` | Survivors |
| `src/Main.SurvivorRoutines.cs` | Shelter, Survivors |
| `src/Main.Survivors.cs` | Campaign, Economy, Expeditions, Foundry, IO, Inventory, Journal, Muster, Radiation, Radio, Shelter, Survivors, YearOfAsh |
| `src/Main.SurvivorSocial.cs` | Survivors |
| `src/Main.SurvivorVoice.cs` | Campaign, Economy, Radiation, Random, Voice |
| `src/Main.TechnicalMaterialArchive.Integration.cs` | Maritime, Narrative |
| `src/Main.TerritoryControl.cs` | Factions, Save |
| `src/Main.TimeCapsule.cs` | Communication |
| `src/Main.TradeRoutes.cs` | Economy |
| `src/Main.TradeTell.cs` |  |
| `src/Main.TraumaBond.cs` | Survivors |
| `src/Main.Trophies.cs` | Shelter |
| `src/Main.TunnelNetwork.cs` | Culture, Inventory, Survivors, Underground |
| `src/Main.UiHandlers.cs` | Campaign, Expeditions, Inventory |
| `src/Main.UiPanels.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.UiTests.CompositionRoot.cs` | UI |
| `src/Main.UiTests.cs` |  |
| `src/Main.UiTests.Dose.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.UiTests.DutyRoster.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.UiTests.Economy.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.UiTests.Expeditions.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Narrative, Radio, Survivors, YearOfAsh |
| `src/Main.UiTests.Holdfast.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.UiTests.IndustrialPanels.cs` | AdvancedMachinery, Expeditions, Medical, Shelter, World |
| `src/Main.UiTests.Inventory.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.UiTests.Journal.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.UiTests.Muster.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.UiTests.Phase0.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.UiTests.Plans198_201.cs` | Combat, Crafting, Economy, Medical, Narrative, World |
| `src/Main.UiTests.PlayerPanels.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.UiTests.RealCampaignJourney.cs` | Save |
| `src/Main.UiTests.SilentFoundry.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.UiTests.StartingCohortLifecycle.cs` | Campaign, Save, Survivors |
| `src/Main.UiTests.Survivors.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.UiTests.UtilityAi.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.UiTests.Verdict.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.UiTests.Wave6.cs` |  |
| `src/Main.UiTests.WorkshopRelic.cs` |  |
| `src/Main.UnifiedEnding.cs` | Endgame |
| `src/Main.VehicleCustomization.cs` | Vehicles |
| `src/Main.VehicleGarage.cs` | Audio, Expeditions, Factions, Needs |
| `src/Main.Verdict.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.VerdictAccusation.cs` |  |
| `src/Main.VisitorIntegration.cs` | Visitors |
| `src/Main.VoluntaryRegister.cs` |  |
| `src/Main.WarlordResponse.cs` | Warlords |
| `src/Main.WaterCondenser.cs` | Shelter |
| `src/Main.WaterQualityProfile.cs` | Water |
| `src/Main.WaterSources.cs` |  |
| `src/Main.WeatherCascade.cs` | Records, Weather, World |
| `src/Main.WeatherForecastReliability.cs` | World |
| `src/Main.WildlifeEcosystem.Integration.cs` | Campaign, Farming, Inventory, Random, Survivors |
| `src/Main.WildlifeHarvest.cs` | World |
| `src/Main.WinterSurvival.Integration.cs` | Expeditions, Inventory, Recreation |
| `src/Main.Workshop.Integration.cs` | Campaign, Excavation, IO, Quests, Radio, Shelter, Survivors |
| `src/Main.World.cs` | Campaign, Economy, Expeditions, Foundry, Greenhouse, Inventory, Journal, Muster, Radio, Shelter, Survivors, YearOfAsh |
| `src/Main.WorldEvolution.cs` | World |
| `src/Main.WorldPlaytest.cs` | Campaign |
| `src/Main.YearOfAsh.cs` | Campaign, Economy, Expeditions, Foundry, Inventory, Journal, Muster, Radio, Survivors, YearOfAsh |
| `src/Main.Zealotry.cs` | Spiritual, Survivors |

## Mechanical equivalence and source contracts

A Roslyn syntax-token comparison found **631 members before and 631 after, with an identical token multiset**. The shared `Has` helper is the deliberate exception: it moved into `HostCli.cs` for release compatibility. The four industrial UI probes now live in the development-only `Main.UiTests.IndustrialPanels.cs` partial. Method names remain unchanged so the existing save triads and CLI dispatch retain their current contracts.

The namespace inventory above is an explicit dependency overapproximation: domain partials retain their original using directives to keep this move mechanical. Historical table paths describe where code came from, not files that should still exist. Source-reading museum, internal-communication and railway-maintenance gates point to their new owner files.

## Day execution ownership

Current source disproved the old assumption that day execution lived in `Main.CampaignOwners.cs`: that file registers typed owner adapters, while `Main.Holdfast.cs` performs the actual advance. The extracted `CampaignDayHostSession` wraps the existing `CampaignDayCoordinator`; `Main` retains the same coordinator reference for save/restore and deterministic owner registration. The coordinator's phase ordering, snapshots, rollback and persistence remain authoritative. Committed day facts reach the existing `IEventBus` as `campaign_day_advanced` with the committed day number.

The boot audit also found two different production owners registered under `world_evolution`. The established phase-4 location/wildlife/landmark owner retains that identity. The authored threshold-event owner now registers as `world_evolution_events` in phase 5, retaining its existing `world_evolution` save section.

Seven matching existing domain files now contain their former bundle members directly (DynamicQuests, Exercise, InterpersonalConflict, MemoryDecay, PersonalBelongings, TunnelNetwork and VehicleGarage). Including those pre-existing members, the merged inventory is **688 before / 688 after, token-identical**. The validation-only WorldPlaytest continues to use its deliberate isolated coordinator; normal day advancement uses the host session.
