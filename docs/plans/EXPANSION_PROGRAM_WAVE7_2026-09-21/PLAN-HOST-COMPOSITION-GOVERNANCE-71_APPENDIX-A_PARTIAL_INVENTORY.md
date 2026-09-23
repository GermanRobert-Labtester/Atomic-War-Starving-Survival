# PLAN-HOST-COMPOSITION-GOVERNANCE-71 — Appendix A: Main Partial Inventory

**Generated:** 2026-09-21 from `src/Main.*.cs` (146 partial files;
100 with Setup/Save methods).
**Use:** HC-71A/71B — the composition inventory. The naming gate flags
duplicate Setup/Save names across partials; the order probe pins the call order
in `ComposeCampaign`.

## Partial inventory

| File | Setup methods | Save methods |
|---|---|---|
| `Main.AdvancedShelterSystems.cs` | `SetupCaravanTrade`, `SetupSurgicalWard`, `SetupPowerSubgrids`, `SetupPerimeterDefense`, `SetupHydroponicBiomes`, `SetupNuclearCore`, `SetupArmoredCrawlers` | `SaveCaravanTrade`, `SaveSurgicalWard`, `SavePowerSubgrids`, `SavePerimeterDefense`, `SaveHydroponicBiomes`, `SaveNuclearCore`, `SaveArmoredCrawlers` |
| `Main.Anomaly.cs` | `SetupAnomalyHazard` | `SaveAnomalyHazard` |
| `Main.Bionics.cs` | `SetupBionics` | `SaveBionics` |
| `Main.BlackMarket.cs` | `SetupBlackMarket` | `SaveBlackMarket` |
| `Main.Campaign.cs` | `SetupCampaignDay`, `SetupDailyBriefingModal`, `SetupMemorial` | `SaveDailyBriefing`, `SaveCampaignDay`, `SaveMemorial` |
| `Main.Cascade.cs` | `SetupCascade` | — |
| `Main.ChemicalSynthesis.cs` | `SetupChemicalSynthesis` | `SaveChemicalSynthesis` |
| `Main.Codex.cs` | `SetupCodex` | — |
| `Main.Collectibles.cs` | `SetupCollectibles` | `SaveCollectibles` |
| `Main.Companion.cs` | `SetupCompanionAnimals` | `SaveCompanionAnimals` |
| `Main.DeepWell.cs` | `SetupDeepWell` | `SaveDeepWell` |
| `Main.Difficulty.cs` | `SetupDifficulty` | — |
| `Main.DutyRoster.cs` | `SetupDutyRoster` | `SaveDutyRoster` |
| `Main.Echoes.cs` | `SetupEchoes` | `SaveEchoes` |
| `Main.EcologicalInfestations.cs` | `SetupEcologicalInfestation`, `SetupFieldGuide` | `SaveEcologicalInfestation`, `SaveFieldGuide` |
| `Main.Economy.cs` | `SetupEconomy`, `SetupCaravans`, `SetupSilentFoundry` | `SaveEconomy`, `SaveCaravans`, `SaveSilentFoundry` |
| `Main.Endgame.cs` | `SetupEndgame` | `SaveEndgame` |
| `Main.Enrichment.cs` | `SetupEnrichment` | — |
| `Main.EvolvingWorld.cs` | `SetupEvolvingWorldInfluence`, `SetupWildlifeTrappingIfBound` | — |
| `Main.ExpandedShelterSystems.cs` | `SetupExpandedShelterSystems` | `SaveAllExpandedShelterSystems`, `SaveResearch` |
| `Main.ExpansionHub.cs` | `SetupExpansions` | `SaveExpansionHub` |
| `Main.Expeditions.cs` | `SetupExpeditions`, `SetupReconTelemetry`, `SetupCombat`, `SetupExpeditionCombatHandoff`, `SetupTravelEncounters`, `SetupEncounterChoiceResolver`, `SetupEncounterChoice` | `SaveExpeditions`, `SaveReconTelemetry`, `SaveCombat`, `SaveWastelandMap`, `SaveEncounterChoice`, `SaveTravelEncounters` |
| `Main.FactionBranch.cs` | `SetupFactionBranch`, `SetupCounterIntelligence` | `SaveFactionBranch`, `SaveCounterIntelligence` |
| `Main.FlagshipInstitutions.cs` | `SetupCulturalArchive`, `SetupDiplomaticSummit`, `SetupSkyDefense`, `SetupSanatorium`, `SetupFlagshipInstitutions` | `SaveCulturalArchive`, `SaveDiplomaticSummit`, `SaveSkyDefense`, `SaveSanatorium` |
| `Main.HiddenAgenda.cs` | `SetupHiddenAgenda`, `SetupHiddenAgendaPanel` | `SaveHiddenAgenda` |
| `Main.Holdfast.cs` | `SetupIceRoad`, `SetupHoldfastRuntime` | `SaveHoldfast`, `SaveHoldfastRuntime` |
| `Main.HydraulicExtrusion.cs` | `SetupHydraulicExtrusion` | `SaveHydraulicExtrusion` |
| `Main.InSarMapping.cs` | `SetupInSarMapping` | `SaveInSarMapping` |
| `Main.Inventory.cs` | `SetupInventory` | `SaveInventory` |
| `Main.LowBackgroundMetrology.cs` | `SetupLowBackgroundMetrology` | `SaveLowBackgroundMetrology` |
| `Main.Maritime.cs` | `SetupDeepCoast`, `SetupMaritime` | `SaveMaritime` |
| `Main.Medical.cs` | `SetupMedical`, `SetupMedicalWard`, `SetupDisease` | `SaveMedicalPipeline`, `SaveMedical`, `SaveMedicalWard`, `SaveDisease` |
| `Main.MoralChoice.cs` | `SetupMoralChoice` | `SaveMoralChoice` |
| `Main.MoraleContagion.cs` | `SetupMoraleContagion` | `SaveMoraleContagion` |
| `Main.Muster.cs` | `SetupMuster` | `SaveMuster` |
| `Main.Narrative.cs` | `SetupJournal`, `SetupEventAdapter`, `SetupNarrative`, `SetupRadio` | `SaveJournal`, `SaveNarrative`, `SaveEventAdapter`, `SaveRadio` |
| `Main.NarrativeQuestlines.cs` | `SetupNarrativeQuestlines` | `SaveNarrativeQuestlines` |
| `Main.NpcArcs.cs` | `SetupNpcArcs` | — |
| `Main.Onboarding.cs` | `SetupOnboarding` | `SaveOnboarding` |
| `Main.PathogenStrains.cs` | `SetupPathogenStrains` | `SavePathogenStrains` |
| `Main.PersonalQuests.cs` | `SetupPersonalQuests`, `SetupPersonalQuestPanel` | `SavePersonalQuests` |
| `Main.Phase0.cs` | `SetupPhantom`, `SetupPhase0`, `SetupDoseLedger` | `SavePhantomMemory`, `SavePhase0`, `SaveDoseLedger` |
| `Main.Piezometer.cs` | `SetupPiezometer` | `SavePiezometer` |
| `Main.Plans110_113.cs` | `SetupChlorAlkali`, `SetupSolarConcentrator`, `SetupPrecisionOptics`, `SetupBallisticShield`, `SetupPlans110To113` | `SaveChlorAlkali`, `SaveSolarConcentrator`, `SavePrecisionOptics`, `SaveBallisticShield` |
| `Main.Plans122to125.cs` | `SetupSofcPower`, `SetupCvdDiamond`, `SetupSoundRanging`, `SetupAmphibiousDraisine` | `SaveSofcPower`, `SaveCvdDiamond`, `SaveSoundRanging`, `SaveAmphibiousDraisine` |
| `Main.Plans126_129.cs` | `SetupBioFermentation` | `SaveBioFermentation` |
| `Main.Plans130_133.cs` | `SetupPlans130To133`, `SetupPowderMetallurgy`, `SetupNvisCommunications`, `SetupLyophilization`, `SetupDraisineRerailing`, `SetupPlans130To133Panel` | `SavePowderMetallurgy`, `SaveNvisCommunications`, `SaveLyophilization`, `SaveDraisineRerailing` |
| `Main.Plans146_149.cs` | `SetupRouteInfrastructure`, `SetupEbPvdCoating`, `SetupMicrofluidicDiagnostic`, `SetupMineClearingFlail`, `SetupRailGrinding`, `SetupPlans146To149` | `SaveRouteInfrastructure`, `SaveEbPvdCoating`, `SaveMicrofluidicDiagnostic`, `SaveMineClearingFlail`, `SaveRailGrinding` |
| `Main.Plans147.cs` | `SetupContrabandStash`, `SetupShelterBarter` | `SaveContrabandStash`, `SaveShelterBarter` |
| `Main.Plans152.cs` | `SetupBlackProjectsArchive` | `SaveBlackProjectsArchive` |
| `Main.Plans154.cs` | `SetupHydroGeologyDiscovery` | `SaveHydroGeologyDiscovery` |
| `Main.Plans155.cs` | `SetupOralLore` | `SaveOralLore` |
| `Main.Plans157.cs` | `SetupGrainMillingArchive` | `SaveGrainMillingArchive` |
| `Main.Plans158.cs` | `SetupTechnicalMaterialArchive` | `SaveTechnicalMaterialArchive` |
| `Main.Plans159.cs` | `SetupLeatherworkArchive` | `SaveLeatherworkArchive` |
| `Main.Plans162_165.cs` | `SetupAgriculture`, `SetupDefense`, `SetupPsychologyArcs`, `SetupWildlifeEcosystem` | `SaveAgriculture`, `SaveDefense`, `SavePsychologyArcs`, `SaveWildlifeEcosystem` |
| `Main.Plans166_169.cs` | `SetupPlans166To169` | `SaveEspionage`, `SaveFluidLogistics`, `SaveProceduralNarrative` |
| `Main.Plans178_181.cs` | `SetupGenerational`, `SetupPrisoners`, `SetupMutations`, `SetupStealth` | `SaveGenerational`, `SavePrisoners`, `SaveMutations`, `SaveStealth` |
| `Main.Plans182_185.cs` | `SetupAviation`, `SetupForcedLabor`, `SetupNarcotics`, `SetupPolitics` | `SaveAviation`, `SaveForcedLabor`, `SaveNarcotics`, `SavePolitics` |
| `Main.Plans186_189.cs` | `SetupFallout`, `SetupDesperation`, `SetupMercenary`, `SetupArchaeology` | `SaveFallout`, `SaveDesperation`, `SaveMercenary`, `SaveArchaeology` |
| `Main.Plans190_193.cs` | `SetupAmputation`, `SetupRailway`, `SetupFungi`, `SetupJustice` | `SaveAmputation`, `SaveRailway`, `SaveFungi`, `SaveJustice` |
| `Main.Plans194_197.cs` | `SetupRecreation` | `SaveRecreation` |
| `Main.Plans198_201.cs` | `SetupChemWarfare`, `SetupCommsArray`, `SetupCeremony`, `SetupRobotics` | `SaveChemWarfare`, `SaveCommsArray`, `SaveCeremony`, `SaveRobotics` |
| `Main.Plans202_205.cs` | `SetupPlasticPyrolysis`, `SetupCargoAirdrop` | `SavePlasticPyrolysis`, `SaveCargoAirdrop` |
| `Main.Plans46_49.cs` | `SetupWorkshop`, `SetupRadioStation`, `SetupShelterSocial`, `SetupExcavationHazards`, `SetupDynamicQuests` | `SaveWorkshop`, `SaveRadioStation`, `SaveShelterSocial`, `SaveExcavationHazards`, `SaveDynamicQuests` |
| `Main.Plans50_53.cs` | `SetupVehicleGarage`, `SetupShelterEspionage`, `SetupSurvivorMentalHealth`, `SetupShelterAcoustics`, `SetupPlans50To53` | `SaveVehicleGarage`, `SaveShelterEspionage`, `SaveSurvivorMentalHealth` |
| `Main.Plans62_65.cs` | `SetupPlans62To65` | `SaveFoodPreservation`, `SavePrewarArchives`, `SaveShelterPrisoners` |
| `Main.Plans74_77.cs` | `SetupGeothermalOrc`, `SetupBallisticsWorkbench`, `SetupAeroponics`, `SetupPneumaticDispatch` | `SaveGeothermalOrc`, `SaveBallisticsWorkbench`, `SaveAeroponics`, `SavePneumaticDispatch` |
| `Main.Plans78_81.cs` | `SetupGeodeticSurvey`, `SetupKineticStorage`, `SetupChemicalRecon`, `SetupPlans78To81` | `SaveGeodeticSurvey`, `SaveKineticStorage`, `SaveChemicalRecon` |
| `Main.Plans94_97.cs` | `SetupGrainProcessing`, `SetupCryogenicAirSeparation`, `SetupHeliograph`, `SetupPlans94To97Panel` | `SaveGrainProcessing`, `SaveCryogenicAirSeparation`, `SaveHeliograph` |
| `Main.PlansB68_B69.cs` | `SetupSeismicDynamics`, `SetupCryoVault` | `SaveSeismicDynamics`, `SaveCryoVault` |
| `Main.PlansB86_B89.cs` | `SetupPrecisionMetrology`, `SetupAquaponics` | `SavePrecisionMetrology`, `SaveAquaponics` |
| `Main.Propaganda.cs` | `SetupPropaganda`, `SetupPropagandaPanel` | `SavePropaganda` |
| `Main.PsyOps.cs` | `SetupPsyOps` | `SavePsyOps` |
| `Main.Quests.cs` | `SetupEventsHost`, `SetupExpansionQuests`, `SetupThirdonary` | `SaveExpansionQuests`, `SaveThirdonary` |
| `Main.RadioProgramProduction.cs` | `SetupRadioProgramProduction` | `SaveRadioProgramProduction` |
| `Main.RelationshipDecay.cs` | `SetupRelationshipDecay`, `SetupRelationshipDecayPanel` | `SaveRelationshipDecay` |
| `Main.RumorNetwork.cs` | `SetupRumorNetwork`, `SetupRumorBoardPanel` | `SaveRumorNetwork` |
| `Main.RunFlatTire.cs` | `SetupRunFlatTire` | `SaveRunFlatTire` |
| `Main.Sanitation.cs` | `SetupSanitation` | `SaveSanitation` |
| `Main.SaveOrchestrator.cs` | — | `SaveAll` |
| `Main.ShelterAtmosphere.cs` | `SetupShelterAtmosphere`, `SetupShelterAtmospherePanel` | `SaveShelterAtmosphere` |
| `Main.ShelterBatch3.cs` | `SetupSumpFlooding`, `SetupDecontamination`, `SetupKitchenNutrition`, `SetupEquipmentCondition`, `SetupLibraryStudy`, `SetupArchiveDesk`, `SetupContractorRoster`, `SetupMentalHealthCrisis`, `SetupShelterAssignment`, `SetupShelterDecor` | `SaveSumpFlooding`, `SaveDecontamination`, `SaveKitchenNutrition`, `SaveEquipmentCondition`, `SaveLibraryStudy`, `SaveArchiveDesk`, `SaveContractorRoster`, `SaveMentalHealthCrisis`, `SaveChemicalDependency`, `SaveShelterAssignment`, `SaveShelterDecor` |
| `Main.ShelterInfrastructure.cs` | `SetupWaterTreatment`, `SetupAirlockSecurity`, `SetupShelterThermal`, `SetupWeatherHardening`, `SetupGeothermalAquifer`, `SetupShelterSchedule`, `SetupAutopsy`, `SetupWaystation`, `SetupShelterFireHazard` | `SaveWaterTreatment`, `SaveAirlockSecurity`, `SaveShelterThermal`, `SaveWeatherHardening`, `SaveGeothermalAquifer`, `SaveShelterSchedule`, `SaveAutopsy`, `SaveWaystation`, `SaveShelterFire` |
| `Main.ShelterReputation.cs` | `SetupShelterReputation`, `SetupShelterReputationPanel` | `SaveShelterReputation` |
| `Main.ShelterSecurity.cs` | `SetupShelterSecurity`, `SetupShelterSecurityPanel` | `SaveShelterSecurity` |
| `Main.ShelterSocial.cs` | `SetupSurvivorRelations`, `SetupRegionalTreaty`, `SetupVinylMorale`, `SetupWildlifeTrapping`, `SetupExcavation`, `SetupApprenticeship`, `SetupCaregiving` | `SaveSurvivorRelations`, `SaveRegionalTreaty`, `SaveVinylMorale`, `SaveWildlifeTrapping`, `SaveExcavation`, `SaveApprenticeship`, `SaveCaregiving` |
| `Main.Spiritual.cs` | `SetupSpiritual` | `SaveSpiritual` |
| `Main.Subterranean.cs` | `SetupSubterranean` | `SaveSubterranean` |
| `Main.SurvivorDeathLegacy.cs` | `SetupDeathLegacy`, `SetupDeathLegacyPanel` | `SaveDeathLegacy` |
| `Main.SurvivorFate.cs` | `SetupSurvivorFate` | `SaveSurvivorFate` |
| `Main.SurvivorFitness.cs` | `SetupFitnessForDuty` | — |
| `Main.SurvivorSocial.cs` | `SetupSurvivorSocial` | `SaveSurvivorSocial` |
| `Main.Survivors.cs` | `SetupSurvivors`, `SetupUtilityAi` | `SaveSurvivors` |
| `Main.TimeCapsule.cs` | `SetupTimeCapsules`, `SetupTimeCapsulePanel` | `SaveTimeCapsules` |
| `Main.Verdict.cs` | `SetupVerdict` | `SaveVerdict` |
| `Main.WaterCondenser.cs` | `SetupWaterCondenser` | `SaveWaterCondenser` |
| `Main.World.cs` | `SetupWorld`, `SetupWeatherSonde`, `SetupCrafting`, `SetupStartingLevel`, `SetupPowerGrid`, `SetupGreenhouse` | `SaveWorld`, `SaveCrafting`, `SaveStartingLevel`, `SavePowerGrid`, `SaveGreenhouse` |
| `Main.YearOfAsh.cs` | `SetupYearOfAsh` | `SaveYearOfAsh` |
| `Main.Zealotry.cs` | `SetupZealotry` | `SaveZealotry` |

## Duplicate Setup names

none found
