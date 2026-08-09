using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        private void InitFactionMapSystems()
        {
            // Prompt #17 — inter-faction raid plans / wiretap choices.
            // Antenna gate uses RadioTunerSystem once it is constructed later in
            // InitializeSystems; the provider reads live state each call.
            // Map is rebound after GeneratedMap is created below.
            FactionRaidPlanSystem = new FactionRaidPlanSystem(new System.Random(_worldSeed + 21));
            FactionRaidPlanSystem.Bind(
                EconomySystem,
                FactionRadioIntercepts,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0,
                isAntennaOperational: IsWiretapAntennaOperational,
                map: null,
                radiation: RadiationSystem);
            FactionRaidPlanSystem.OnInterceptOffered += HandleRaidPlanInterceptOffered;

            _onWorldPhaseChanged = phase =>
            {
                EconomySystem.NotifyPhaseChanged(phase);
                // Keep weather/rad systems in sync with campaign phase labels
                if (phase == WorldPhase.Flashpoint || phase == WorldPhase.NuclearWinter)
                {
                    // Exchange already unpauses rads in HandleNuclearExchange
                }
            };
            WorldPhaseSystem.OnPhaseChanged += _onWorldPhaseChanged;

            // Proc-gen wasteland map (seed-stable layout for this playthrough)
            GeneratedMap = MapGenerator.Generate(_worldSeed);
            FactionRaidPlanSystem?.SetMap(GeneratedMap);

            // River crossings need the live node list (seed-stable with map).
            if (RiverNodeSystem != null && GeneratedMap?.Nodes != null)
                RiverNodeSystem.GenerateRiverNodes(GeneratedMap.Nodes, _worldSeed + 569);

            // Knowledge map must exist before SaveSystem can capture it
            KnowledgeMap = new RadiationKnowledgeMap();
            SeedKnowledgeMap();

        }

        private void InitSaveAndExpeditions()
        {
            // Save
            SaveSystem = new SaveSystem(new SaveSystem.CoreDeps
            {
                GameState = GameState,
                WeatherSystem = WeatherSystem,
                TemperatureSystem = TemperatureSystem,
                NeedsSystem = NeedsSystem,
                RadiationSystem = RadiationSystem,
                Shelter = Shelter,
                GetSurvivors = () => Survivors,
                ItemLookup = id =>
                {
                    var fromCatalog = _itemCatalog?.GetById(id);
                    if (fromCatalog != null) return fromCatalog;
                    // Prompt #13 — poisoned iodine may only exist as a runtime plant.
                    if (SabotagedCacheSystem != null
                        && string.Equals(id, SabotagedCacheSystem.PoisonedIodineItemId,
                            System.StringComparison.OrdinalIgnoreCase))
                        return SabotagedCacheSystem.PoisonedIodineDefinition;
                    return null;
                },
                // Prefer SO already on the installed module (fresh boot has definitions).
                // Do not return null for known modules — that used to clear Definition on load.
                ModuleLookup = id =>
                {
                    if (string.IsNullOrEmpty(id) || Shelter == null) return null;
                    return Shelter.GetModule(id)?.Definition;
                }
            });
            // P1 / AUDIT-004: fail-fast ISaveable restore in Editor + Development
            // (game-ci batchmode) only. Release players keep best-effort restore.
            SaveSystem.FailFastRestore = SaveSystem.DefaultFailFastRestoreForEnvironment();
            SaveSystem.SetPhotoPeriodSystem(PhotoperiodSystem);
            SaveSystem.SetKnowledgeMap(KnowledgeMap);
            SaveSystem.SetGeneratedMap(GeneratedMap);
            SaveSystem.SetInventory(Inventory);
            SaveSystem.SetMedicalSystem(MedicalSystem);
            SaveSystem.SetBloodTransfusionSystem(BloodTransfusion);
            SaveSystem.SetAmputationSystem(AmputationSystem);
            SaveSystem.SetScurvySystem(ScurvySystem);
            SaveSystem.SetAddictionSystem(Addiction);
            SaveSystem.SetBloodToxicitySystem(BloodToxicity);
            SaveSystem.SetGraftRejectionSystem(GraftRejection);
            SaveSystem.SetPheromoneMaskingSystem(PheromoneMasking);
            SaveSystem.SetChemToleranceSystem(ChemTolerance);
            SaveSystem.SetLastWillSystem(LastWill);
            SaveSystem.SetLegacyStartSystem(LegacyStart);
            SaveSystem.SetBloodTypesSystem(BloodTypes);
            SaveSystem.SetEpilogueStatsSystem(EpilogueStats);
            SaveSystem.SetGossipSystem(Gossip);
            SaveSystem.SetAdaptiveWarlordsSystem(AdaptiveWarlords);
            SaveSystem.SetBilgePumpsSystem(BilgePumps);
            SaveSystem.SetCarrionBirdsSystem(CarrionBirds);
            SaveSystem.SetLogicGatesSystem(LogicGates);
            SaveSystem.SetModLoaderSystem(ModLoader);
            SaveSystem.SetTwitchApiSystem(TwitchAPI);
            SaveSystem.SetDiseaseExpansionSystem(DiseaseExpansion);
            SaveSystem.SetDynamicScapegoatSystem(Scapegoat);
            SaveSystem.SetIronManMode(IronMan);
            SaveSystem.SetAndroidNpcSystem(AndroidNpcs);
            SaveSystem.SetSheriffRoleSystem(Sheriff);
            SaveSystem.SetScenarioGenSystem(ScenarioGen);
            SaveSystem.SetSpeedrunTimerSystem(SpeedrunTimer);
            SaveSystem.SetTrueEndingSystem(TrueEnding);
            SaveSystem.SetVictoryAirliftSystem(VictoryAirlift);
            SaveSystem.SetVictoryAscendancySystem(VictoryAscendancy);
            SaveSystem.SetVictoryBuriedAliveSystem(VictoryBuriedAlive);
            SaveSystem.SetVictoryCannibalKingSystem(VictoryCannibalKing);
            SaveSystem.SetVictoryDefectionSystem(VictoryDefection);
            SaveSystem.SetVictoryIcebreakerSystem(VictoryIcebreaker);
            SaveSystem.SetVictoryLoneSurvivorSystem(VictoryLoneSurvivor);
            SaveSystem.SetVictoryMadSystem(VictoryMAD);
            SaveSystem.SetVictoryMigrationSystem(VictoryMigration);
            SaveSystem.SetVictoryTheBroadcastSystem(VictoryTheBroadcast);
            SaveSystem.SetVictoryTheCureSystem(VictoryTheCure);
            SaveSystem.SetVictoryTheMartianSystem(VictoryTheMartian);
            SaveSystem.SetVictoryUndergroundCitySystem(VictoryUndergroundCity);
            SaveSystem.SetVictoryUnifierSystem(VictoryUnifier);

            RegisterSaveOnlySystemsBulk();
            InitAutosaveScavengingExpedition();
            InitWorldEventAndUtilitySystems();
            WireCraftingAndPerkBindings();
            InitAtmosphereCorpsePantrySystems();
        }

        /// <summary>
        /// Bulk ISaveable registration for systems that need no further wiring —
        /// mechanically extracted from InitSaveAndExpeditions, which qlty flagged
        /// as the most complex function in the project purely from its length.
        /// No logic here, just SaveSystem.SetXxx calls in their original order.
        /// </summary>
        private void RegisterSaveOnlySystemsBulk()
        {
            SaveSystem.SetMapHazardAcidGeyser(MapHazardAcidGeyser);
            SaveSystem.SetMapHazardAshlanche(MapHazardAshlanche);
            // demoted ghost — SetMapHazardBiometricDoor skipped
            // demoted ghost — SetMapHazardCraterWall skipped
            // demoted ghost — SetMapHazardCrevice skipped
            // demoted ghost — SetMapHazardFlammableGas skipped
            // demoted ghost — SetMapHazardGasPockets skipped
            // demoted ghost — SetMapHazardMagneticAnomaly skipped
            // demoted ghost — SetMapHazardSinkholeCollapse skipped
            SaveSystem.SetMapHazardVenusTrap(MapHazardVenusTrap);
            // demoted ghost — SetMapAnomalyAshDunes skipped
            // demoted ghost — SetMapAnomalyBoilingLake skipped
            // demoted ghost — SetMapAnomalyCherenkov skipped
            // demoted ghost — SetMapAnomalyDogDen skipped
            // demoted ghost — SetMapAnomalyDontLook skipped
            SaveSystem.SetMapAnomalyDryCoral(MapAnomalyDryCoral);
            // demoted ghost — SetMapAnomalyFloodedSubway skipped
            // demoted ghost — SetMapAnomalyGlassCrater skipped
            // demoted ghost — SetMapAnomalyMassGrave skipped
            // demoted ghost — SetMapAnomalyMirage skipped
            // demoted ghost — SetMapAnomalyPetrifiedForest skipped
            // demoted ghost — SetMapAnomalyQuietZone skipped
            // demoted ghost — SetMapAnomalyRustedTank skipped
            // demoted ghost — SetMapAnomalyServerFarm skipped
            // demoted ghost — SetMapAnomalySinkhole skipped
            // demoted ghost — SetMapAnomalyTangledDrop skipped
            // demoted ghost — SetMapAnomalyTireFire skipped
            // demoted ghost — SetMapAnomalyUxoNuke skipped
            // demoted ghost — SetBiome* skipped
            // demoted ghost — SetShelterModule* skipped
            // demoted ghost — SetEvent* skipped
            // ── CoreFamilies bulk Set (auto) ────────────────────────────
            SaveSystem.SetFalloutStormHazard(FalloutStormHazard);
            // demoted ghost — SetActionCrawlspace skipped
            // demoted ghost — SetActionPlay skipped
            // demoted ghost — SetActionSlaughterPet skipped
            // demoted ghost — SetActionTeachChild skipped
            // demoted ghost — SetActionTellStories skipped
            // demoted ghost — SetItemAshGoat skipped
            // demoted ghost — SetItemBoots skipped
            // demoted ghost — SetItemLiveTrap skipped
            // demoted ghost — SetItemMutantChicken skipped
            // demoted ghost — SetItemToys skipped
            SaveSystem.SetTraitAshTongue(TraitAshTongue);
            SaveSystem.SetTraitKleptomaniac(TraitKleptomaniac);
            SaveSystem.SetTraitMascot(TraitMascot);
            SaveSystem.SetTraitStuntedEmpathy(TraitStuntedEmpathy);
            SaveSystem.SetTraitSuperstitious(TraitSuperstitious);
            // demoted ghost — SetAfflictionBunkerFever skipped
            // demoted ghost — SetAfflictionZoonoticFlu skipped
            SaveSystem.SetModuleRationLock(ModuleRationLock);
            // demoted ghost — SetNodeOrphanage skipped
            SaveSystem.SetPetGuardDog(PetGuardDog);
            // demoted ghost — SetActionAdministerPlacebo skipped
            // demoted ghost — SetActionBarricadeDoor skipped
            // demoted ghost — SetActionBoilBatteries skipped
            // demoted ghost — SetActionBroadcastPropaganda skipped
            // demoted ghost — SetActionBurnCharcoal skipped
            // demoted ghost — SetActionBuryTimeCapsule skipped
            // demoted ghost — SetActionCallCaravan skipped
            // demoted ghost — SetActionCoverTracks skipped
            // demoted ghost — SetActionCrackMainframe skipped
            // demoted ghost — SetActionDecrypt skipped
            // demoted ghost — SetActionDemandTribute skipped
            // demoted ghost — SetActionEstablishRoute skipped
            // demoted ghost — SetActionExile skipped
            // demoted ghost — SetActionFish skipped
            // demoted ghost — SetActionHarvestOrgans skipped
            // demoted ghost — SetActionInfectSelf skipped
            // demoted ghost — SetActionIsotopeTrace skipped
            // demoted ghost — SetActionMercy skipped
            // demoted ghost — SetActionMixCement skipped
            // demoted ghost — SetActionMixChems skipped
            // demoted ghost — SetActionOverwatch skipped
            // demoted ghost — SetActionPhysicalTherapy skipped
            // demoted ghost — SetActionPirateRadio skipped
            // demoted ghost — SetActionPlaceBait skipped
            // demoted ghost — SetActionPullTooth skipped
            // demoted ghost — SetActionRigCorpse skipped
            // demoted ghost — SetActionRoutePower skipped
            // demoted ghost — SetActionSabotage skipped
            // demoted ghost — SetActionScorchedEarth skipped
            // demoted ghost — SetActionSealRoom skipped
            // demoted ghost — SetActionSelfSurgery skipped
            // demoted ghost — SetActionSilentTakedown skipped
            // demoted ghost — SetActionSiphonGas skipped
            // DEMOTE-001 — ActionStabilizeDNA not save-wired (ghost demoted).
            // demoted ghost — SetActionStargazing skipped
            // demoted ghost — SetActionWorshipIdol skipped
            // demoted ghost — SetAfflictionAdrenalineCrash skipped
            // demoted ghost — SetAfflictionAmnesia skipped
            // demoted ghost — SetAfflictionBrainwashed skipped
            // demoted ghost — SetAfflictionBrittleBones skipped
            // demoted ghost — SetAfflictionCaveMadness skipped
            // demoted ghost — SetAfflictionFeralRegression skipped
            // demoted ghost — SetAfflictionImaginaryFriend skipped
            // demoted ghost — SetAfflictionNerveDamage skipped
            // demoted ghost — SetAfflictionOldAge skipped
            // demoted ghost — SetAfflictionPhantomLimb skipped
            // demoted ghost — SetAfflictionRadHallucinations skipped
            // demoted ghost — SetAfflictionRadiationBlindness skipped
            // demoted ghost — SetAfflictionScurvyDegeneration skipped
            // demoted ghost — SetAfflictionSporeLung skipped
            // demoted ghost — SetAfflictionSterile skipped
            // demoted ghost — SetAfflictionSurvivorsGuilt skipped
            // demoted ghost — SetAfflictionTBI skipped
            // demoted ghost — SetAfflictionThyroidCancer skipped
            // demoted ghost — SetAfflictionTrenchFoot skipped
            // demoted ghost — SetAudioEventDeafening skipped
            // demoted ghost — SetAudioEventHeartbeat skipped
            // demoted ghost — SetCombatBleedOut skipped
            // demoted ghost — SetCombatFlanking skipped
            // demoted ghost — SetCombatSuppression skipped
            // demoted ghost — SetCombatStanceLastStand skipped
            // demoted ghost — SetCrisisFeralFlora skipped
            // demoted ghost — SetCrisisStructuralFailure skipped
            // demoted ghost — SetDurabilitySuppressor skipped
            // demoted ghost — SetEndgameUltimatum skipped
            // demoted ghost — SetHazardCookOff skipped
            // demoted ghost — SetHazardExplosiveCrafting skipped
            // demoted ghost — SetHazardFriendlyFire skipped
            SaveSystem.SetHazardMethane(HazardMethane);
            SaveSystem.SetHazardMimicCrate(HazardMimicCrate);
            SaveSystem.SetHazardSurgicalBotch(HazardSurgicalBotch);
            SaveSystem.SetHazardWeaponBurst(HazardWeaponBurst);
            // demoted ghost — SetHiddenStatUnseen skipped
            // demoted ghost — SetItemAICoreData skipped
            SaveSystem.SetItemAmmoTypes(ItemAmmoTypes);
            // demoted ghost — SetItemAmmonia skipped
            // demoted ghost — SetItemAmphetamines skipped
            // demoted ghost — SetItemAshGhillie skipped
            // demoted ghost — SetItemAutoDoc skipped
            // demoted ghost — SetItemBioPlastic skipped
            // demoted ghost — SetItemBloodBag skipped
            // demoted ghost — SetItemBoneSaw skipped
            // demoted ghost — SetItemC4 skipped
            // demoted ghost — SetItemCaltrops skipped
            // demoted ghost — SetItemCarrierBird skipped
            // demoted ghost — SetItemChildsDrawing skipped
            // demoted ghost — SetItemCigarettes skipped
            // demoted ghost — SetItemClimbingGear skipped
            // demoted ghost — SetItemDecoy skipped
            // demoted ghost — SetItemDogTags skipped
            // demoted ghost — SetItemEMPGrenade skipped
            // demoted ghost — SetItemEncryptedDrive skipped
            // demoted ghost — SetItemEpiPen skipped
            // demoted ghost — SetItemExosuit skipped
            // demoted ghost — SetItemFaradayPack skipped
            // demoted ghost — SetItemForeignBook skipped
            // demoted ghost — SetItemGeigerCalibrator skipped
            // demoted ghost — SetItemGlowingMushroom skipped
            // demoted ghost — SetItemGoldBars skipped
            // demoted ghost — SetItemGuitar skipped
            // demoted ghost — SetItemHeirloom skipped
            // demoted ghost — SetItemIBeam skipped
            // demoted ghost — SetItemImpureIodine skipped
            // demoted ghost — SetItemJuggernautArmor skipped
            // demoted ghost — SetItemKevlarVest skipped
            SaveSystem.SetItemKeycards(ItemKeycards);
            // demoted ghost — SetItemLandmine skipped
            // demoted ghost — SetItemLeadApron skipped
            // demoted ghost — SetItemLiquidStitches skipped
            // demoted ghost — SetItemMaggots skipped
            // demoted ghost — SetItemMilGasMask skipped
            // demoted ghost — SetItemMutantGland skipped
            // demoted ghost — SetItemNanites skipped
            // demoted ghost — SetItemNightVision skipped
            // demoted ghost — SetItemPackMule skipped
            // demoted ghost — SetItemPasswordNote skipped
            // demoted ghost — SetItemPhotoAlbum skipped
            // demoted ghost — SetItemPotassiumIodide skipped
            // demoted ghost — SetItemPresidentialSeal skipped
            // demoted ghost — SetItemPrussianBlue skipped
            // demoted ghost — SetItemRTGBattery skipped
            // demoted ghost — SetItemSeedLedger skipped
            // demoted ghost — SetItemShockCollar skipped
            // demoted ghost — SetItemSnowshoes skipped
            // demoted ghost — SetItemSurgicalTubing skipped
            // demoted ghost — SetItemTearGas skipped
            // demoted ghost — SetItemTeddyBear skipped
            // demoted ghost — SetItemTrashHazmat skipped
            // demoted ghost — SetItemUndeliveredMail skipped
            // demoted ghost — SetItemVacuumTubes skipped
            // demoted ghost — SetItemVinylCollection skipped
            // demoted ghost — SetItemVitamins skipped
            // demoted ghost — SetItemWalkieTalkie skipped
            // demoted ghost — SetItemWastelandSoap skipped
            // demoted ghost — SetItemWaterTabs skipped
            // demoted ghost — SetItemWeldingGoggles skipped
            // demoted ghost — SetItemWristDosimeter skipped
            // demoted ghost — SetLocationArcade skipped
            // demoted ghost — SetLocationSlaveMarket skipped
            // demoted ghost — SetLocationStrandedYacht skipped
            SaveSystem.SetMapAquifer(MapAquifer);
            SaveSystem.SetAshDriftSystem(AshDriftSystem);
            SaveSystem.SetBurnWardSystem(BurnWardSystem);
            SaveSystem.SetCognitiveDecaySystem(CognitiveDecaySystem);
            SaveSystem.SetLightningStrikesSystem(LightningStrikesSystem);
            // demoted ghost — SetLocationStateRuinSystem skipped
            SaveSystem.SetMobileCampSystem(MobileCampSystem);
            SaveSystem.SetMoralDilemmaSystem(MoralDilemmaSystem);
            SaveSystem.SetNeedleSterilizationSystem(NeedleSterilizationSystem);
            SaveSystem.SetNightScavengeSystem(NightScavengeSystem);
            SaveSystem.SetProstheticCraftingSystem(ProstheticCraftingSystem);
            SaveSystem.SetSeismicVentsSystem(SeismicVentsSystem);
            SaveSystem.SetSevereFrostbiteSystem(SevereFrostbiteSystem);
            SaveSystem.SetTetanusAfflictionSystem(TetanusAfflictionSystem);
            SaveSystem.SetTimeSystem(TimeSystem);
            SaveSystem.SetToothDecaySystem(ToothDecaySystem);
            // demoted ghost — SetVehicleStrandingSystem skipped
            // demoted ghost — SetVehicleSystem skipped
            SaveSystem.SetVisionLossSystem(VisionLossSystem);
            // demoted ghost — SetVisitorRNGSystem skipped
            // demoted ghost — SetNPCAddictsPassive skipped
            // demoted ghost — SetNPCAggroScavengers skipped
            // demoted ghost — SetNPCAggroTrader skipped
            // demoted ghost — SetNPCBandits skipped
            // demoted ghost — SetNPCBlackOps skipped
            // demoted ghost — SetNPCBroker skipped
            // demoted ghost — SetNPCCannibals skipped
            // demoted ghost — SetNPCChemScientists skipped
            // demoted ghost — SetNPCCityResidents skipped
            // demoted ghost — SetNPCCollaborators skipped
            // demoted ghost — SetNPCConscripts skipped
            // demoted ghost — SetNPCDesperateFamily skipped
            // demoted ghost — SetNPCDrunksAggro skipped
            // demoted ghost — SetNPCHomeless skipped
            // demoted ghost — SetNPCLonePsychopath skipped
            // demoted ghost — SetNPCLooters skipped
            // demoted ghost — SetNPCMercenaries skipped
            // demoted ghost — SetNPCMilitaryPatrol skipped
            // demoted ghost — SetNPCPassiveScavengers skipped
            // REPROMOTE-001 — PassiveTrader save-wired + economy weather mult.
            SaveSystem.SetNPCPassiveTrader(NPCPassiveTrader);
            // demoted ghost — SetNPCPsychopathPair skipped
            // demoted ghost — SetNPCRebelMilitia skipped
            // demoted ghost — SetNPCRebelModerates skipped
            // demoted ghost — SetNPCRebelSnipers skipped
            // demoted ghost — SetNPCRebelZealots skipped
            // demoted ghost — SetNPCSlavers skipped
            // demoted ghost — SetNPCSpecOps skipped
            // demoted ghost — SetNPCSurvivalists skipped
            // demoted ghost — SetNPCTaxCollector skipped
            // demoted ghost — SetNPCTerrorists skipped
            // demoted ghost — SetNPCTheNegotiator skipped
            // demoted ghost — SetNPCTheOld skipped
            // demoted ghost — SetNPCTheParents skipped
            // demoted ghost — SetNPCTravelingCouple skipped
            // demoted ghost — SetNodeAutomatedArmory skipped
            // demoted ghost — SetNodeGhostShip skipped
            // demoted ghost — SetNodeMutantHive skipped
            // demoted ghost — SetNodePlayerBank skipped
            // demoted ghost — SetNodeSector7G skipped
            // demoted ghost — SetNodeSporeHive skipped
            SaveSystem.SetPetFeralCat(PetFeralCat);
            SaveSystem.SetProjectBioReactor(ProjectBioReactor);
            SaveSystem.SetProjectDeepWell(ProjectDeepWell);
            SaveSystem.SetProjectElevator(ProjectElevator);
            SaveSystem.SetProjectMinecart(ProjectMinecart);
            SaveSystem.SetProjectRadioArray(ProjectRadioArray);
            SaveSystem.SetProjectSurfaceDome(ProjectSurfaceDome);
            SaveSystem.SetShelterEventCaravanAmbush(ShelterEventCaravanAmbush);
            SaveSystem.SetShelterEventFalseCure(ShelterEventFalseCure);
            SaveSystem.SetShelterEventRansom(ShelterEventRansom);
            SaveSystem.SetShelterEventRefugees(ShelterEventRefugees);
            SaveSystem.SetShelterEventTheMirror(ShelterEventTheMirror);
            SaveSystem.SetShelterEventTribute(ShelterEventTribute);
            SaveSystem.SetSkirmishBandit_vs_Terror(SkirmishBandit_vs_Terror);
            SaveSystem.SetSkirmishMil_vs_Rebel(SkirmishMil_vs_Rebel);
            SaveSystem.SetSkirmishMil_vs_Terror(SkirmishMil_vs_Terror);
            SaveSystem.SetSkirmishRebel_vs_Bandit(SkirmishRebel_vs_Bandit);
            SaveSystem.SetSkirmishRebel_vs_Terror(SkirmishRebel_vs_Terror);
            SaveSystem.SetTraderPlagueConvoy(TraderPlagueConvoy);
            SaveSystem.SetTraitAnthropophobia(TraitAnthropophobia);
            SaveSystem.SetTraitClairvoyant(TraitClairvoyant);
            SaveSystem.SetTraitGenerationalTrauma(TraitGenerationalTrauma);
            SaveSystem.SetTraitInheritedGenetics(TraitInheritedGenetics);
            SaveSystem.SetTraitMatriarch(TraitMatriarch);
            SaveSystem.SetTraitPTSD(TraitPTSD);
            SaveSystem.SetUIEventBlurredVision(UIEventBlurredVision);
            SaveSystem.SetUIEventCorruptionScare(UIEventCorruptionScare);
            SaveSystem.SetUIEventFalseInventory(UIEventFalseInventory);
            SaveSystem.SetUIEventGhostRadio(UIEventGhostRadio);
            SaveSystem.SetUIEventHacking(UIEventHacking);
            SaveSystem.SetUIEventLowPower(UIEventLowPower);
            SaveSystem.SetUIEventMapRot(UIEventMapRot);
            SaveSystem.SetUIEventPhantomBlip(UIEventPhantomBlip);
            // demoted ghost — SetVehicleArmoredTruck skipped
            // demoted ghost — SetVehicleMotorcycle skipped
            // demoted ghost — SetVehicleRowboat skipped
            // demoted ghost — SetVisitorAbandonedState skipped
            // demoted ghost — SetVisitorChurchHostile skipped
            // demoted ghost — SetVisitorChurchSanctuary skipped
            // demoted ghost — SetVisitorExplodedState skipped
            // demoted ghost — SetVisitorFleeingHorde skipped
            // demoted ghost — SetVisitorHospitalPatients skipped
            // demoted ghost — SetVisitorHospitalStaff skipped
            // demoted ghost — SetVisitorMilTrainingYard skipped
            // demoted ghost — SetVisitorQuestFaction skipped
            // demoted ghost — SetVisitorRebelTrainingYard skipped
            // demoted ghost — SetWeaponChainsaw skipped
            // demoted ghost — SetWeaponFlamethrower skipped
            SaveSystem.SetWeaponHMG(WeaponHMG);
            // demoted ghost — SetWeaponRPG skipped
            SaveSystem.SetWorldEventDeforestation(WorldEventDeforestation);
            SaveSystem.SetWorldEventFinalWinter(WorldEventFinalWinter);
            SaveSystem.SetWorldEventFissure(WorldEventFissure);
            SaveSystem.SetWorldEventGreatFamine(WorldEventGreatFamine);
            SaveSystem.SetWorldEventMegafauna(WorldEventMegafauna);

            SaveSystem.SetSiegeArtillerySystem(SiegeArtillery);
            SaveSystem.SetSiegeBiowarfareSystem(SiegeBiowarfare);
            SaveSystem.SetSiegeBlockadeSystem(SiegeBlockade);
            SaveSystem.SetSiegeHostageShieldSystem(SiegeHostageShield);
            SaveSystem.SetSiegeNightRaidSystem(SiegeNightRaid);
            SaveSystem.SetSiegeSappersSystem(SiegeSappers);
            SaveSystem.SetSiegeSmokeOutSystem(SiegeSmokeOut);
            SaveSystem.SetSiegeVehicleRamSystem(SiegeVehicleRam);
            SaveSystem.SetRiverNodeSystem(RiverNodeSystem);
            SaveSystem.SetMutagenesisSystem(Mutagenesis);
            SaveSystem.SetWorldPhaseSystem(WorldPhaseSystem);
            SaveSystem.SetEconomySystem(EconomySystem);
            SaveSystem.SetPowerNetwork(PowerNetwork);
            SaveSystem.SetHatchDefense(HatchDefenseSystem);
            SaveSystem.SetFactionRadioIntercepts(FactionRadioIntercepts);
            SaveSystem.SetFactionRaidPlanSystem(FactionRaidPlanSystem);
            SaveSystem.SetJournalSystem(JournalSystem);
            SaveSystem.SetVictoryProjectManager(VictoryProject);
            SaveSystem.SetEventRunner(EventRunner);
            SaveSystem.SetSuspicionTracker(SuspicionTracker);
            SaveSystem.SetPreCaptureHook(SnapshotRadioHudToInterceptSystem);
            SaveSystem.SetWaterStorage(WaterStorage);
            // SetFlashpointChoreographer is called later in InitializeSystems
            // after the Choreographer itself is constructed (it depends on
            // RadioTunerSystem and other systems wired after SaveSystem).

            // BUG HUNT: BootWeather() constructs, event-wires, and ticks these 8
            // trackers, but none had a SaveSystem.SetWeatherX call anywhere — an
            // active weather event (e.g. a Solar Flare mid-blackout) silently
            // reset to inactive on every save/load. SetWeatherX runs after
            // BootWeather() (InitFoundation) but BootWeather() runs before
            // SaveSystem exists, so the wiring has to live here instead.
            SaveSystem.SetWeatherBloodRain(WeatherBloodRain);
            SaveSystem.SetWeatherDeepFreeze(WeatherDeepFreeze);
            SaveSystem.SetWeatherEmpStorm(WeatherEmpStorm);
            SaveSystem.SetWeatherFalseSpring(WeatherFalseSpring);
            SaveSystem.SetWeatherOzoneHole(WeatherOzoneHole);
            SaveSystem.SetWeatherSilentSpring(WeatherSilentSpring);
            SaveSystem.SetWeatherSolarFlare(WeatherSolarFlare);
            SaveSystem.SetWeatherStaticCharge(WeatherStaticCharge);
            // Same gap for the reprometed Roadblock encounter (REPROMOTE-Encounter-001).
            SaveSystem.SetEncounterRoadblock(EncounterRoadblock);
        }

        /// <summary>Autosave hook, plus the scavenging and expedition engines.</summary>
        private void InitAutosaveScavengingExpedition()
        {
            // Subscribe to phase changes for autosave.
            // _suppressAutoSave is held while Awake restores a "Continue" slot:
            // SaveSystem.Load restores the snapshot's phase, which would
            // otherwise re-fire this hook and write the just-loaded state back
            // over the autosave slot -- wrong when the player continued from
            // quicksave, since it would destroy their separate autosave.
            _onGameStateChanged = phase =>
            {
                if (phase == GamePhase.Running && !_suppressAutoSave) SaveSystem.AutoSave();
            };
            GameState.OnPhaseChanged += _onGameStateChanged;

            // Scavenging + survey (shares KnowledgeMap with SaveSystem)
            ScavengingSystem = new LocationScavengingSystem(
                RadiationSystem, Inventory, _itemCatalog, _worldSeed,
                KnowledgeMap, () => TimeSystem.CurrentDay,
                _lootTable, () => WorldPhaseSystem.CurrentPhase);
            ScavengingSystem.OnSurveyCompleted += (mission, success) => RefreshMapKnowledgeHUD();
            ScavengingSystem.OnMissionCompleted += (mission, loot) => RefreshMapKnowledgeHUD();

            // Expedition Engine (node-based events, stances, stamina drain, push-your-luck)
            // Wired with the MedicalSystem so the Day-30 flashpoint intercept
            // can inflict trauma afflictions on survivors caught outside, and
            // with the Shelter + Survivors list so the hatch-dilemma handler
            // can spike bunker contamination and propagate deny-entry morale.
            ExpeditionSystem = new ExpeditionSystem(
                RadiationSystem, Inventory, _itemCatalog,
                new ExpeditionSystem.Config
                {
                    WeatherSystem = WeatherSystem,
                    KnowledgeMap = KnowledgeMap,
                    MedicalSystem = MedicalSystem,
                    Shelter = Shelter,
                    Survivors = Survivors,
                    Seed = _worldSeed
                });
            ExpeditionSystem.SetGeneratedMap(GeneratedMap);
            ExpeditionSystem.SetNeedsSystem(NeedsSystem);
            ExpeditionSystem.SetBicycleSystem(BicycleSystem);
            ExpeditionSystem.SetFloodedNodeSystem(FloodedNodeSystem);
            ExpeditionSystem.SetRiverNodeSystem(RiverNodeSystem);
            ExpeditionSystem.SetBloodToxicitySystem(BloodToxicity);
            ExpeditionSystem.SetHasItem(itemId =>
                Inventory != null && !string.IsNullOrEmpty(itemId)
                && Inventory.CountById(itemId) > 0);
            ExpeditionSystem.SetItemHandlers(
                itemId => Inventory != null && !string.IsNullOrEmpty(itemId)
                    ? Inventory.CountById(itemId) : 0,
                (itemId, amount) => Inventory != null
                    && !string.IsNullOrEmpty(itemId)
                    && amount > 0
                    && Inventory.RemoveById(itemId, amount));
            // Prompts #182–#188 — combat milestone tracking on encounters / flee
            ExpeditionSystem.BindCombatPerks(
                CombatPerks,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0,
                affinity: MentalBreakSystem != null ? MentalBreakSystem.Affinity : null,
                getAllSurvivors: () => Survivors);
            SaveSystem.SetExpeditionSystem(ExpeditionSystem);
            SaveSystem.SetFloodedNodeSystem(FloodedNodeSystem);
            SaveSystem.SetBicycleSystem(BicycleSystem);
            SaveSystem.SetWaterEconomySystem(WaterEconomySystem);
            SaveSystem.SetBlackRainHazardSystem(BlackRainHazardSystem);
            SaveSystem.SetClothingSystem(ClothingSystem);

        }

        /// <summary>Sabotaged caches, shifting hotspots, hatch entrapment, and the second bulk ISaveable block.</summary>
        private void InitWorldEventAndUtilitySystems()
        {
            // Prompt #13 — hostile factions learn scavenging habits and plant
            // poisoned medical crates. High Medical skill / Paranoid spots them.
            SabotagedCacheSystem = new SabotagedCacheSystem(new System.Random(_worldSeed + 19));
            SabotagedCacheSystem.BindEconomy(EconomySystem);
            SabotagedCacheSystem.SetPoisonedIodineDefinition(
                SabotagedCacheSystem.CreatePoisonedIodineDefinition());
            ExpeditionSystem.SetSabotagedCacheSystem(SabotagedCacheSystem);
            SaveSystem.SetSabotagedCacheSystem(SabotagedCacheSystem);
            ExpeditionSystem.OnSabotagedCacheDetected += (exp, msg) =>
            {
                GameLog.Log($"[Sabotaged Cache] Detected by {exp?.Survivor?.DisplayName}: {msg}");
            };

            // Prompt #14 — post-Day-30 windstorms move death-zone rad two path-hops.
            ShiftingHotspotSystem = new ShiftingHotspotSystem(new System.Random(_worldSeed + 20));
            ShiftingHotspotSystem.Bind(GeneratedMap, KnowledgeMap);
            SaveSystem.SetShiftingHotspotSystem(ShiftingHotspotSystem);
            ShiftingHotspotSystem.OnHotspotShifted += shift =>
            {
                if (shift == null) return;
                GameLog.Log(
                    $"[Shifting Hotspot] Windstorm day {shift.Day}: " +
                    $"{shift.FromNodeId} → {shift.ToNodeId} " +
                    $"(moved {shift.MovedRad:F0} rad/hr)");
                RefreshMapKnowledgeHUD();
            };

            // Prompt #48 — weather buries/freezes the hatch after 72 continuous
            // hours of Blizzard/FalloutStorm; DigOut spikes entry CO2; broken
            // air filter while sealed starts suffocation countdown.
            HatchEntrapmentSystem = new HatchEntrapmentSystem();
            _entryRoom = new ShelterRoom(HatchEntrapmentSystem.EntryRoomId, null);
            HatchEntrapmentSystem.OnHatchStateChanged += (prev, next) =>
            {
                SyncHatchExpeditionLock();
                GameLog.Log($"[Hatch Entrapment] HatchState {prev} → {next}");
            };
            HatchEntrapmentSystem.OnBuriedAliveTriggered += () =>
            {
                // Present the Buried Alive event immediately when the seal lands.
                if (EventRunner == null) return;
                var buried = EventRunner.FindInPool(EventRunner.BuriedAliveEventId)
                             ?? EventRunner.CreateBuriedAliveEvent();
                var ctx = BuildEventContext(TimeSystem != null ? TimeSystem.CurrentDay : 1);
                ctx.SetEventFlag(HatchEntrapmentSystem.FlagBuriedAliveOffered, true);
                if (buried != null && buried.CanTrigger(ctx))
                    EventRunner.Run(buried, ctx);
            };
            SaveSystem.SetHatchEntrapment(HatchEntrapmentSystem);
            SaveSystem.SetChildDependentSystem(ChildSystem);
            SaveSystem.SetStructuralIntegritySystem(StructuralIntegrity);
            SaveSystem.SetWasteSystem(WasteSystem);
            SaveSystem.SetVerminSystem(VerminSystem);
            SaveSystem.SetPetSystem(PetSystem);
            SaveSystem.SetFuelDecaySystem(FuelDecaySystem);
            SaveSystem.SetJuryRigSystem(JuryRigSystem);
            SaveSystem.SetFreezePipeSystem(FreezePipeSystem);
            SaveSystem.SetCartographySystem(CartographySystem);
            SaveSystem.SetTrackerSystem(TrackerSystem);
            SaveSystem.SetDeadDropSystem(DeadDropSystem);
            SaveSystem.SetHostageSystem(HostageSystem);
            SaveSystem.SetPropagandaSystem(PropagandaSystem);
            SaveSystem.SetDeserterSystem(DeserterSystem);
            SaveSystem.SetScapegoatSystem(ScapegoatSystem);
            SaveSystem.SetLaborCampSystem(LaborCampSystem);
            SaveSystem.SetCultMoralSystem(CultMoralSystem);
            SaveSystem.SetEcosystemSystem(EcosystemSystem);
            SaveSystem.SetHouseToBunkerSystem(HouseToBunkerSystem);
            SaveSystem.SetLocationQuestSystem(LocationQuestSystem);
            SaveSystem.SetExcavationSystem(ExcavationSystem);
            SaveSystem.SetFloodingSystem(FloodingSystem);
            SaveSystem.SetHiddenStorageSystem(HiddenStorageSystem);
            SaveSystem.SetCeilingCollapseSystem(CeilingCollapseSystem);
            SaveSystem.SetPerimeterTrapSystem(PerimeterTrapSystem);
            SaveSystem.SetTunnelingSystem(TunnelingSystem);
            SaveSystem.SetHatchVisibilitySystem(HatchVisibilitySystem);
            SaveSystem.SetEscapeHatchSystem(EscapeHatchSystem);
            SaveSystem.SetMaterialShieldingSystem(MaterialShieldingSystem);
            SaveSystem.SetAirlockSystem(AirlockSystem);
            SaveSystem.SetNoiseSystem(NoiseSystem);
            SaveSystem.SetResilienceSystem(ResilienceSystem);
            SaveSystem.SetCompostSystem(CompostSystem);
            SaveSystem.SetScrapWeaponSystem(ScrapWeaponSystem);
            SaveSystem.SetSterilizationSystem(SterilizationSystem);
            SaveSystem.SetChelationSystem(ChelationSystem);
            SaveSystem.SetWindTurbineSystem(WindTurbineSystem);
            SaveSystem.SetAntibioticResistSystem(AntibioticResistSystem);
            SaveSystem.SetHaulingSystem(HaulingSystem);
            SaveSystem.SetWeaponMaintenanceSystem(WeaponMaintenanceSystem);
            SaveSystem.SetAestheticsSystem(AestheticsSystem);
            SaveSystem.SetHamRadioSystem(HamRadioSystem);
            SaveSystem.SetTriageSystem(TriageSystem);
            SaveSystem.SetPolypharmacySystem(PolypharmacySystem);
            SaveSystem.SetSkillProgressionSystem(SkillProgression);
            SaveSystem.SetCombatPerkSystem(CombatPerks);
            SaveSystem.SetSurvivalPerkSystem(SurvivalPerks);
            SaveSystem.SetShelterPerkSystem(ShelterPerks);
            SaveSystem.SetMedicalPerkSystem(MedicalPerks);
            SaveSystem.SetExpeditionPerkSystem(ExpeditionPerks);
            SaveSystem.SetSocialPerkSystem(SocialPerks);
            SaveSystem.SetPersonalQuestSystem(PersonalQuests);
            SaveSystem.SetHallucinationSystem(HallucinationSystem);
            SaveSystem.SetInternalLockSystem(InternalLockSystem);
            SaveSystem.SetSurvivorDiariesSystem(SurvivorDiaries);
            SaveSystem.SetRadioBroadcastSystem(RadioSystem);

        }

        /// <summary>Crafting/scavenging save-restore lookups, then perk and personal-quest wiring.</summary>
        private void WireCraftingAndPerkBindings()
        {
            // CraftingSystem needs a recipe lookup for restoring active crafts.
            CraftingSystem.SetRecipeLookup(id =>
            {
                if (_recipeCatalog == null || _recipeCatalog.recipes == null) return null;
                for (int r = 0; r < _recipeCatalog.recipes.Count; r++)
                    if (_recipeCatalog.recipes[r] != null && _recipeCatalog.recipes[r].id == id)
                        return _recipeCatalog.recipes[r];
                return null;
            });
            // ...and a survivor lookup so a restored craft keeps its crafter
            // (CrafterId is saved; Crafter itself is [NonSerialized]).
            CraftingSystem.SetSurvivorLookup(id => Survivors?.Find(s => s.Id == id));
            SaveSystem.SetCraftingSystem(CraftingSystem);
            SaveSystem.SetWorkbenchSystem(WorkbenchSystem);

            // ScavengingSystem needs a survivor lookup for restoring active missions.
            ScavengingSystem.SetSurvivorLookup(id => Survivors?.Find(s => s.Id == id));
            SaveSystem.SetScavengingSystem(ScavengingSystem);

            WireCombatPerkBindings();
            WireSurvivalPerkBindings();
            WireShelterPerkBindings();
            WireMedicalPerkBindings();
            WireExpeditionPerkBindings();
            WireSocialPerkBindings();
            WirePersonalQuestBindings();
            SyncHatchExpeditionLock();

            // ───────────────────────────────────────────────────────────
        }

        /// <summary>Internal Horror — atmosphere, corpses, pantry rust, and the hatch-dilemma event bridge.</summary>
        private void InitAtmosphereCorpsePantrySystems()
        {
            // Internal Horror — atmosphere / corpses / pantry rust
            // ───────────────────────────────────────────────────────────
            _storesRoom = new ShelterRoom("stores", null);
            AtmosphereSystem = new ShelterAtmosphereSystem(new System.Random(_worldSeed + 16));
            AtmosphereSystem.RegisterRoom(_entryRoom);
            AtmosphereSystem.RegisterRoom(_storesRoom);
            AtmosphereSystem.RegisterRoom(new ShelterRoom("quarters", null));
            AtmosphereSystem.RegisterRoom(new ShelterRoom("plant", null));

            CorpseSystem = new CorpseManagementSystem(
                NeedsSystem, Inventory, MedicalSystem, RadiationSystem,
                CreateSaltedRng(_worldSeed, "corpse"));
            CorpseSystem.SetItemDefinitions(
                CorpseManagementSystem.CreateCorpseDefinition(),
                CorpseManagementSystem.CreateFertilizerDefinition());
            CorpseSystem.SetStoresRoom(_storesRoom);
            CorpseSystem.SetSurvivorProvider(() => Survivors);
            // Prompt #188 — Desensitized: no corpse morale drain
            CorpseSystem.BindCombatPerks(CombatPerks);
            // Prompt #192 — The Butcher yields / process time
            CorpseSystem.BindSurvivalPerks(
                SurvivalPerks,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0);
            CorpseSystem.BindDeathHandler();
            // Prompt #658 — outdoor burial / disposal feeds carrion birds.
            WireCarrionBirds();

            PantrySystem = new PantryContaminationSystem(
                Inventory, new System.Random(_worldSeed + 18));
            PantrySystem.SetContaminatedFoodDefinition(
                PantryContaminationSystem.CreateContaminatedFoodDefinition());
            PantrySystem.SetStoresRoom(_storesRoom);

            SaveSystem.SetAtmosphereSystem(AtmosphereSystem);
            SaveSystem.SetCorpseSystem(CorpseSystem);
            SaveSystem.SetPantrySystem(PantrySystem);

            // Hatch dilemma: when a comms-severed expedition arrives at the
            // bunker, run a forced dilemma GameEventSO with three choices
            // (let them in, force decon, deny). The ExpeditionSystem raises
            // HatchDilemmaReadySignal when an expedition enters the
            // AtHatchDilemma phase; we build the event here and run it
            // through the EventRunner, then forward the choice back via
            // HatchDilemmaResolvedSignal (which the ExpeditionSystem listens to).
            ExpeditionSystem.OnHatchDilemmaReady += OnHatchDilemmaReady_Handle;
            if (Inventory != null)
            {
                Inventory.OnInventoryChanged += RefreshMapKnowledgeHUD;
                Inventory.OnInventoryChanged += RefreshInventoryStrip;
            }
            if (KnowledgeMap != null)
            {
                KnowledgeMap.OnKnowledgeChanged += RefreshMapKnowledgeHUD;
            }

        }

        /// <summary>
        /// Prompts #182–#188 — bind combat perk milestones into hatch defense,
        /// perimeter traps, weapon jam hooks, and expedition encounter tracking.
        /// </summary>
        private void WireCombatPerkBindings()
        {
            // Structured caliber combat: ResolveHit power, raid armor, spend order,
            // expedition combat shots, and military/rebel exclusive loot injection.
            // Independent of CombatPerks so ammo wiring still applies if perks are null.
            WireAmmoTypesBindings();

            if (CombatPerks == null) return;

            HatchDefenseSystem?.BindCombatPerks(CombatPerks);
            HatchDefenseSystem?.BindPerimeterTraps(PerimeterTrapSystem);

            WireGuardDogAlert();
            WireMountedHmg();
            WireHatchRaidEpilogueStats();
            WireAdaptiveWarlordsStrategyHooks();
            WireWeaponJamClearing();

            PerimeterTrapSystem?.BindCombatPerks(CombatPerks, getSurvivor: GetSurvivorById);
            ExpeditionSystem?.BindCombatPerks(
                CombatPerks,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0,
                affinity: MentalBreakSystem != null ? MentalBreakSystem.Affinity : null,
                getAllSurvivors: () => Survivors);
        }

        /// <summary>REPROMOTE-Pet-001 — guard dog Alert on raid start → defense bonus when fed.</summary>
        private void WireGuardDogAlert()
        {
            if (HatchDefenseSystem == null || PetGuardDog == null) return;

            HatchDefenseSystem.TryAlertGuardDog = () => PetGuardDog.Alert("bunker");
            PetGuardDog.OnDogAlerted += (shelterId, canFight) =>
                GameLog.Log($"[GameBootstrap] Guard dog alert at '{shelterId}' canFight={canFight}");
        }

        /// <summary>REPROMOTE-Weapon-001 — mounted HMG stock contributes via GetWeaponPower.</summary>
        private void WireMountedHmg()
        {
            if (HatchDefenseSystem == null || WeaponHMG == null) return;

            // Ensure a default mount so stockpile HMG is usable as hatch defense.
            if (!WeaponHMG.IsMounted)
                WeaponHMG.Mount("hatch");

            HatchDefenseSystem.GetMountedHmgDefensePower = inv =>
            {
                if (inv == null || WeaponHMG == null) return 0f;
                bool hasStock = inv.CountById(Weapon_HMG.InventoryItemId) > 0
                    || inv.CountById("hmg") > 0;
                if (!hasStock) return 0f;
                if (!WeaponHMG.IsMounted)
                    WeaponHMG.Mount("hatch");
                // HMG needs 2 operators — count assigned hatch guards.
                int operators = HatchDefenseSystem.ActiveGuardCount;
                // Oil: not jammed = treated as oiled for defense-power path.
                bool isOiled = !WeaponHMG.IsJammed;
                return WeaponHMG.GetHatchDefensePower(operators, isOiled);
            };

            // HMG-002 — fire the mounted HMG each raid. Fire() handles the jam risk
            // and OnRaidShredded so the HMG risk-reward is not bypassed. Operators
            // and raid level are read at fire time.
            HatchDefenseSystem.FireMountedHmg = (operators, isOiled, raidLevel) =>
            {
                if (WeaponHMG == null) return;
                WeaponHMG.Fire("hatch", operators, isOiled, raidLevel);
            };
        }

        /// <summary>Prompt #768 — epilogue bullet tally from hatch raids.</summary>
        private void WireHatchRaidEpilogueStats()
        {
            if (HatchDefenseSystem == null) return;

            HatchDefenseSystem.OnRaidResolved += result =>
            {
                if (result.AmmoConsumed > 0)
                    EpilogueStats?.RecordBulletsFired(result.AmmoConsumed);
                // Prompt #861 — heavy ammo spend counts as sniper/suppression strategy.
                if (result.AmmoConsumed >= 10)
                    AdaptiveWarlords?.RecordStrategy(System_AdaptiveWarlords.StrategySnipers);
            };
        }

        /// <summary>
        /// Prompt #861 — trap deployments → traps strategy; combat milestones →
        /// stealth/snipers/traps, read from CombatPerks' milestone key.
        /// </summary>
        private void WireAdaptiveWarlordsStrategyHooks()
        {
            if (PerimeterTrapSystem != null)
            {
                PerimeterTrapSystem.OnTrapDeployed += () =>
                    AdaptiveWarlords?.RecordStrategy(System_AdaptiveWarlords.StrategyTraps);
            }

            CombatPerks.OnMilestoneProgress += (sv, key, value) =>
            {
                if (string.IsNullOrEmpty(key)) return;
                if (string.Equals(key, "stealth_kills", StringComparison.Ordinal))
                    AdaptiveWarlords?.RecordStrategy(System_AdaptiveWarlords.StrategyStealth);
                else if (string.Equals(key, "ammo_expended", StringComparison.Ordinal) && value >= 20)
                    AdaptiveWarlords?.RecordStrategy(System_AdaptiveWarlords.StrategySnipers);
                else if (string.Equals(key, "traps_deployed", StringComparison.Ordinal))
                    AdaptiveWarlords?.RecordStrategy(System_AdaptiveWarlords.StrategyTraps);
            };
        }

        /// <summary>Prompt #174 / #182 — jam during hatch defense uses WeaponMaintenance clear ticks.</summary>
        private void WireWeaponJamClearing()
        {
            if (HatchDefenseSystem == null || WeaponMaintenanceSystem == null) return;

            HatchDefenseSystem.TryJamWeapon = (weaponId, clearTicks) =>
                WeaponMaintenanceSystem.TryJam(weaponId, clearTicks: clearTicks);
        }

        private Survivor GetSurvivorById(string id)
        {
            if (Survivors == null || string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < Survivors.Count; i++)
            {
                if (Survivors[i] != null && Survivors[i].Id == id)
                    return Survivors[i];
            }
            return null;
        }

        /// <summary>
        /// Wire Item_AmmoTypes into hatch defense (ResolveHit stockpile power +
        /// faction armor + civilian-first spend), expedition combat/loot, location
        /// scavenging military tables, and craft gate (civilian loads only).
        /// </summary>
        private void WireAmmoTypesBindings()
        {
            if (ItemAmmoTypes == null) return;

            Func<string, ItemDefinition> ammoFactory = id =>
            {
                if (string.IsNullOrEmpty(id) || _itemCatalog?.items == null) return null;
                for (int i = 0; i < _itemCatalog.items.Count; i++)
                {
                    var it = _itemCatalog.items[i];
                    if (it != null && string.Equals(it.id, id, StringComparison.Ordinal))
                        return it;
                }
                return null;
            };

            if (HatchDefenseSystem != null)
            {
                HatchDefenseSystem.AmmoDefensePowerResolver =
                    (ammoId, amount, armor) => ItemAmmoTypes.GetAmmoStockpileDefensePower(ammoId, amount, armor);
                HatchDefenseSystem.FactionArmorResolver = Item_AmmoTypes.GetFactionArmor;
                HatchDefenseSystem.AmmoSpendPriorityResolver = Item_AmmoTypes.AmmoSpendPriority;
            }

            ExpeditionSystem?.BindAmmoTypes(ItemAmmoTypes, ammoFactory);
            ScavengingSystem?.BindAmmoTypes(
                ItemAmmoTypes,
                ammoFactory,
                getLocationLootTableId: locId =>
                {
                    if (GeneratedMap == null || string.IsNullOrEmpty(locId)) return null;
                    var node = GeneratedMap.GetNode(locId);
                    return node != null ? node.LootTableId : null;
                });

            CraftingSystem?.BindCraftResultGate(Item_AmmoTypes.IsWorkbenchCraftAllowed);

            // UI: stockpile breakdown, exclusive tooltips, hatch arms preview, encounter log.
            WireAmmoUiBindings();
        }

        /// <summary>
        /// Hatch ammo stockpile / arms preview, inventory exclusive tooltips,
        /// and expedition combat lines → encounter log HUD.
        /// </summary>
        private void WireAmmoUiBindings()
        {
            if (ExpeditionEncounterLog == null)
                ExpeditionEncounterLog = new ExpeditionEncounterLog();

            if (_hud != null)
            {
                var hatchHud = _hud.HatchDefenseHUD;
                if (hatchHud != null && ItemAmmoTypes != null)
                {
                    var proxy = new HatchDefenseSystemProxy(armor =>
                        HatchDefenseSystem != null
                            ? HatchDefenseSystem.GetWeaponPower(null, armor)
                            : 0f);
                    hatchHud.BindAmmoUi(
                        ammoStockpileBreakdown: () =>
                            ItemAmmoTypes.FormatStockpileBreakdown(Inventory),
                        armsPowerPreview: () =>
                            ItemAmmoTypes.FormatHatchPowerPreview(
                                Inventory,
                                HatchDefenseSystem != null
                                    ? HatchDefenseSystem.GetShelterSecurity()
                                    : 0f,
                                proxy));
                }

                var strip = _hud.InventoryStripUI;
                if (strip != null)
                {
                    strip.TooltipResolver = Item_AmmoTypes.FormatItemTooltip;
                    strip.MilitaryExclusiveChecker = Item_AmmoTypes.IsMilitaryExclusiveTooltip;
                    strip.Sync(Inventory);
                }

                var logHud = _hud.EnsureExpeditionEncounterLog();
                if (logHud != null && ExpeditionEncounterLog != null)
                {
                    logHud.SetLines(ExpeditionEncounterLog.Lines);
                    ExpeditionEncounterLog.OnLineAdded -= logHud.Push;
                    ExpeditionEncounterLog.OnLineAdded += logHud.Push;
                }

                _hud.EnsureDiegeticHud();
                _hud.RefreshDiegeticHud();
            }

            if (ExpeditionSystem != null && ExpeditionEncounterLog != null)
            {
                ExpeditionSystem.OnEncounterResolved -= OnExpeditionEncounterResolved_LogCombat;
                ExpeditionSystem.OnEncounterResolved += OnExpeditionEncounterResolved_LogCombat;
            }

            // REPROMOTE-Encounter-001 — map/SO roadblock tags → class ResolveChoice.
            if (ExpeditionSystem != null && EncounterRoadblock != null)
                ExpeditionSystem.BindClassRoadblock(EncounterRoadblock);

            // REPROMOTE-MapHazard-001 — swamp looting → VenusTrap harvest check.
            if (ExpeditionSystem != null && MapHazardVenusTrap != null)
                ExpeditionSystem.BindVenusTrap(MapHazardVenusTrap);

            // REPROMOTE-Item-001 — keycard doors on secure/military expedition nodes.
            if (ExpeditionSystem != null && ItemKeycards != null)
                ExpeditionSystem.BindKeycards(ItemKeycards);
        }

        private void OnExpeditionEncounterResolved_LogCombat(
            ExpeditionState exp, EncounterSO selected, EventChoice chosen)
        {
            if (ExpeditionEncounterLog == null || ExpeditionSystem == null) return;

            string combatLine = ExpeditionSystem.LastCombatLogLine;
            if (!string.IsNullOrEmpty(combatLine))
            {
                ExpeditionEncounterLog.Add(combatLine);
                return;
            }

            // Non-combat or no ammo spent — still note the beat for the field log.
            if (selected == null) return;
            string who = exp?.Survivor != null
                ? (exp.Survivor.DisplayName ?? exp.Survivor.Id)
                : "Scavenger";
            string choice = chosen?.ChoiceId ?? "resolve";
            string enc = selected.id ?? "contact";
            ExpeditionEncounterLog.Add($"{who} · {enc.Replace('_', ' ')} · {choice}");
        }

        /// <summary>
        /// Prompts #189–#194 — bind survival perk milestones into cooking, medical
        /// cures, crafting, corpse processing, and AI context.
        /// </summary>
        private void WireSurvivalPerkBindings()
        {
            if (SurvivalPerks == null) return;

            CookingSystem = new CookingSystem(Inventory, WaterStorage, new System.Random(_worldSeed + 189));
            CookingSystem.SetNeedsSystem(NeedsSystem);
            CookingSystem.BindSurvivalPerks(
                SurvivalPerks,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0);
            CookingSystem.SetMealDefinition(CookingSystem.CreateCookedMealDefinition());
            // Prompt #768 — epilogue meal tally.
            CookingSystem.OnMealCooked += (_, __) => EpilogueStats?.RecordMealCooked(1);
            SaveSystem?.SetCookingSystem(CookingSystem);

            CraftingSystem?.BindSurvivalPerks(
                SurvivalPerks,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0);

            WorkbenchSystem?.BindSurvivalPerks(SurvivalPerks, NeedsSystem);
            WorkbenchSystem?.SetMoonshineItems(
                WorkbenchSystem.CreateMoonshineDefinition(),
                WorkbenchSystem.CreateMutatedFungiDefinition());

            // Prompt #190 — gastric illness recoveries grant Iron Stomach
            if (MedicalSystem != null)
            {
                MedicalSystem.OnAfflictionCured += (sv, active) =>
                {
                    if (active == null) return;
                    int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;
                    SurvivalPerks.RecordIllnessRecovery(sv, active.AfflictionId, day);
                };
            }
        }

        /// <summary>
        /// Prompts #195–#200 — bind shelter-engineering perks into jury-rig,
        /// workbench scrap, struts, excavation, tunneling, and atmosphere.
        /// </summary>
        private void WireShelterPerkBindings()
        {
            if (ShelterPerks == null) return;

            Func<int> getDay = () => TimeSystem != null ? TimeSystem.CurrentDay : 0;

            JuryRigSystem?.BindShelterPerks(ShelterPerks, getDay);
            WorkbenchSystem?.BindShelterPerks(ShelterPerks, new System.Random(_worldSeed + 198));
            WorkbenchSystem?.SetRareComponentItems(
                WorkbenchSystem.CreateBatteryDefinition(),
                WorkbenchSystem.CreateSpringDefinition());

            StructuralIntegrity?.BindShelterPerks(
                ShelterPerks, getDay, CeilingCollapseSystem);
            ExcavationSystem?.BindShelterPerks(ShelterPerks, getDay);
            TunnelingSystem?.BindShelterPerks(ShelterPerks, new System.Random(_worldSeed + 199));
        }

        /// <summary>
        /// Prompts #201–#205 — bind medical milestone perks into surgery, amputation,
        /// Death's Door, and raid-window bandaging.
        /// </summary>
        private void WireMedicalPerkBindings()
        {
            if (MedicalPerks == null) return;

            Func<int> getDay = () => TimeSystem != null ? TimeSystem.CurrentDay : 0;
            Func<string, Survivor> findSv = id =>
            {
                if (Survivors == null || string.IsNullOrEmpty(id)) return null;
                for (int i = 0; i < Survivors.Count; i++)
                {
                    if (Survivors[i] != null && Survivors[i].Id == id)
                        return Survivors[i];
                }
                return null;
            };

            MedicalPerks.SetSurvivorProvider(() => Survivors);

            MedicalSystem?.BindMedicalPerks(
                MedicalPerks,
                findSurvivor: findSv,
                surgeryRng: new System.Random(_worldSeed + 201));
            if (MedicalSystem != null)
            {
                MedicalSystem.IsRaidWindowActive = () =>
                    HatchDefenseSystem != null && HatchDefenseSystem.IsRaidWindowActive;
            }

            AmputationSystem?.BindMedicalPerks(MedicalPerks, getDay);

            // Prompt #205 — Death's Door when colony has a Paramedic.
            if (NeedsSystem != null)
            {
                NeedsSystem.TryDeferDeath = sv => MedicalPerks.TryEnterDeathsDoor(sv);
            }
        }

        /// <summary>
        /// Prompts #206–#210 — bind expedition milestone perks into carry weight,
        /// stealth, city travel, night combat, darkness morale, and foraging.
        /// </summary>
        private void WireExpeditionPerkBindings()
        {
            if (ExpeditionPerks == null) return;

            Func<int> getDay = () => TimeSystem != null ? TimeSystem.CurrentDay : 0;

            ExpeditionSystem?.BindExpeditionPerks(
                ExpeditionPerks,
                getDay: getDay,
                noiseSystem: NoiseSystem,
                isStormActive: () =>
                    WeatherSystem != null
                    && (WeatherSystem.Current == WeatherKind.FalloutStorm
                        || WeatherSystem.Current == WeatherKind.Blizzard
                        || WeatherSystem.Current == WeatherKind.BlackRain));

            PerimeterTrapSystem?.BindExpeditionPerks(ExpeditionPerks, getDay);

            ScavengingSystem?.BindExpeditionPerks(
                ExpeditionPerks,
                getNodeTags: id =>
                {
                    var n = GeneratedMap?.GetNode(id);
                    return n?.Tags;
                },
                getNodeRingName: id =>
                {
                    var n = GeneratedMap?.GetNode(id);
                    return n != null ? n.Ring.ToString() : null;
                });

            // Prompt #209 — Night Terror: zero darkness morale penalty.
            if (NeedsSystem != null)
            {
                NeedsSystem.IgnoresDarknessMorale = sv =>
                    ExpeditionPerks != null && ExpeditionPerks.IgnoresDarknessMorale(sv);
            }
        }

        /// <summary>
        /// Prompts #211–#213 — bind social perks into pantry spoil rate and
        /// (optionally) weapon rust when a Quartermaster shares the room.
        /// </summary>
        private void WireSocialPerkBindings()
        {
            if (SocialPerks == null) return;

            // Prompt #212 — food spoil 50% slower in Quartermaster's room.
            PantrySystem?.BindDegradationMultiplier(roomId =>
                SocialPerks.GetItemDegradationMultiplier(roomId, Survivors));
        }

        /// <summary>
        /// Prompts #214–#219 — bind personal quests into medical, social, crafting,
        /// corpse, and (when present) pet systems. UI evolution toast is logged.
        /// </summary>
        private void WirePersonalQuestBindings()
        {
            if (PersonalQuests == null) return;

            MedicalPerks?.BindPersonalQuests(PersonalQuests);
            SocialPerks?.BindPersonalQuests(PersonalQuests);
            CraftingSystem?.BindPersonalQuests(PersonalQuests, new System.Random(_worldSeed + 214));
            CorpseSystem?.BindPersonalQuests(PersonalQuests);
            CombatPerks?.BindPersonalQuests(PersonalQuests);
            MedicalSystem?.BindPersonalQuests(PersonalQuests);
            HatchDefenseSystem?.BindPersonalQuests(PersonalQuests);
            ExpeditionSystem?.BindPersonalQuests(PersonalQuests);
            EventRunner?.BindPersonalQuests(PersonalQuests);
            PantrySystem?.BindPersonalQuests(PersonalQuests, () => Survivors);
            // Prompts #225–#234 host hooks
            WaterEconomySystem?.BindPersonalQuests(PersonalQuests, () => Survivors);
            PowerNetwork?.BindPersonalQuests(PersonalQuests, () => Survivors);
            AtmosphereSystem?.BindPersonalQuests(PersonalQuests, () => Survivors);
            StructuralIntegrity?.BindPersonalQuests(PersonalQuests);
            ExcavationSystem?.BindPersonalQuests(PersonalQuests);
            VictoryProject?.BindPersonalQuests(PersonalQuests, () => Survivors);
            BicycleSystem?.BindPersonalQuests(PersonalQuests, () => Survivors);
            RadiationSystem?.BindPersonalQuests(PersonalQuests, () => Survivors);
            DeadDropSystem?.BindPersonalQuests(PersonalQuests, () => Survivors);
            HatchVisibilitySystem?.BindPersonalQuests(PersonalQuests, () => Survivors);
            WeatherSystem?.BindPersonalQuests(PersonalQuests, () => Survivors);
            // Prompts #235–#248
            SkillProgression?.BindPersonalQuests(PersonalQuests);
            MentorshipSystem?.BindPersonalQuests(PersonalQuests);
            EconomySystem?.BindPersonalQuests(PersonalQuests, () => Survivors);
            ClothingSystem?.BindPersonalQuests(PersonalQuests, () => Survivors);
            Addiction?.BindPersonalQuests(PersonalQuests);
            CookingSystem?.BindPersonalQuests(PersonalQuests);
            JournalSystem?.BindPersonalQuests(PersonalQuests, () => Survivors);
            RadioTunerSystem?.BindPersonalQuests(PersonalQuests, () => Survivors);
            // Prompts #249–#256 bond/burden host loops
            // Prompts #257–#266 civil-war / interpersonal host loops
            // (HatchDefense Art of War + Sacrificial; Economy Hated military trust;
            //  Crafting Supply Chain cost; PowerNetwork fuel mult; Expedition trap/ghost;
            //  Needs Living Saint / Hyper-Empath; Medical God Complex / Humbled;
            //  Sleep bed-only; ActionScorer labor filters; NeedsBar Denialist anxiety)
            NeedsSystem?.BindPersonalQuests(PersonalQuests, () => Survivors);
            MentalBreakSystem?.BindPersonalQuests(PersonalQuests, () => Survivors);
            // Prompts #267–#283 chemistry / titles host loops
            NoiseSystem?.BindPersonalQuests(PersonalQuests, () => Survivors);
            // Prompts #284–#298 rebuilders / scholars / outlaws
            HaulingSystem?.BindPersonalQuests(PersonalQuests);
            AtmosphereSystem?.BindPersonalQuests(PersonalQuests, () => Survivors);

            // #267 Relapsing Addict: force-chem drains amphetamines stock (no free dose).
            if (NeedsSystem != null)
            {
                NeedsSystem.ForcedChemConsumeHandler = sv =>
                {
                    if (sv == null || Inventory == null) return false;
                    string chemId = PersonalQuestSystem.AmphetaminesItemId;
                    if (Inventory.CountById(chemId) < 1) return false;
                    if (!Inventory.RemoveById(chemId, 1)) return false;
                    ChemUse?.Notify(sv, chemId);
                    return true;
                };
            }

            PersonalQuests.OnCharacterEvolution += (sv, traitId, display) =>
            {
                string name = sv != null ? sv.DisplayName : "?";
                GameLog.Log(
                    "CharacterEvolution",
                    $"{name} unlocked latent expert trait: {display} ({traitId})");
            };
            PersonalQuests.OnMapNodeSpawnRequested += (nodeId, ownerId) =>
            {
                GameLog.Log(
                    "PersonalQuest",
                    $"Map node requested: {nodeId} for survivor {ownerId}");
            };
            PersonalQuests.OnBunkerEventRequested += (eventId, ownerId) =>
            {
                GameLog.Log(
                    "PersonalQuest",
                    $"Bunker event requested: {eventId} for survivor {ownerId}");
            };
        }

    }
}
