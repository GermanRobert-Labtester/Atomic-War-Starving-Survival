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
            SaveSystem.SetMapHazardAcidGeyser(MapHazardAcidGeyser);
            SaveSystem.SetMapHazardAshlanche(MapHazardAshlanche);
            // demoted ghost — SetMapHazardBiometricDoor skipped
            // demoted ghost — SetMapHazardCraterWall skipped
            // demoted ghost — SetMapHazardCrevice skipped
            // demoted ghost — SetMapHazardFlammableGas skipped
            // demoted ghost — SetMapHazardGasPockets skipped
            // demoted ghost — SetMapHazardMagneticAnomaly skipped
            // demoted ghost — SetMapHazardSinkholeCollapse skipped
            // demoted ghost — SetMapHazardVenusTrap skipped
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
            SaveSystem.SetBiomeAshSwamp(BiomeAshSwamp);
            SaveSystem.SetBiomeGlassDesert(BiomeGlassDesert);
            SaveSystem.SetBiomeHighwayTunnel(BiomeHighwayTunnel);
            SaveSystem.SetBiomeSaltFlats(BiomeSaltFlats);
            SaveSystem.SetBiomeSkyscraperTops(BiomeSkyscraperTops);
            SaveSystem.SetBiomeSuburbs(BiomeSuburbs);
            // demoted ghost — SetWeatherAcidSnow skipped
            // demoted ghost — SetWeatherBioFog skipped
            // demoted ghost — SetWeatherBlackSnow skipped
            SaveSystem.SetWeatherBloodRain(WeatherBloodRain);
            // demoted ghost — SetWeatherDeadWind skipped
            SaveSystem.SetWeatherDeepFreeze(WeatherDeepFreeze);
            // demoted ghost — SetWeatherDustDevil skipped
            SaveSystem.SetWeatherEmpStorm(WeatherEmpStorm);
            SaveSystem.SetWeatherFalseSpring(WeatherFalseSpring);
            // demoted ghost — SetWeatherGlassStorm skipped
            SaveSystem.SetWeatherOzoneHole(WeatherOzoneHole);
            // demoted ghost — SetWeatherRadHail skipped
            SaveSystem.SetWeatherSilentSpring(WeatherSilentSpring);
            SaveSystem.SetWeatherSolarFlare(WeatherSolarFlare);
            SaveSystem.SetWeatherStaticCharge(WeatherStaticCharge);
            // demoted ghost — SetEncounterAmalgamation skipped
            // demoted ghost — SetEncounterBurrowers skipped
            // demoted ghost — SetEncounterFloodedMaze skipped
            // demoted ghost — SetEncounterGlowingDead skipped
            // demoted ghost — SetEncounterGlowingStag skipped
            // demoted ghost — SetEncounterHitAndRun skipped
            // demoted ghost — SetEncounterLeeches skipped
            // demoted ghost — SetEncounterMirelurker skipped
            // demoted ghost — SetEncounterPressurePlate skipped
            // demoted ghost — SetEncounterRiverPirates skipped
            // REPROMOTE-Encounter-001 — Roadblock class tracker save-wired.
            SaveSystem.SetEncounterRoadblock(EncounterRoadblock);
            // demoted ghost — SetEncounterRobotDog skipped
            // demoted ghost — SetEncounterSleepingCamp skipped
            // demoted ghost — SetEncounterTripwireMaze skipped
            // demoted ghost — SetEncounterWarlordTank skipped
            SaveSystem.SetShelterModuleAcidTrap(ShelterModuleAcidTrap);
            SaveSystem.SetShelterModuleAutodoc(ShelterModuleAutodoc);
            SaveSystem.SetShelterModuleCctv(ShelterModuleCctv);
            SaveSystem.SetShelterModuleClassroom(ShelterModuleClassroom);
            SaveSystem.SetShelterModuleConfessional(ShelterModuleConfessional);
            SaveSystem.SetShelterModuleConveyor(ShelterModuleConveyor);
            SaveSystem.SetShelterModuleDaylightSensor(ShelterModuleDaylightSensor);
            SaveSystem.SetShelterModuleDroneStation(ShelterModuleDroneStation);
            SaveSystem.SetShelterModuleHoloEmitter(ShelterModuleHoloEmitter);
            SaveSystem.SetShelterModuleInsectFarm(ShelterModuleInsectFarm);
            SaveSystem.SetShelterModuleLathe(ShelterModuleLathe);
            SaveSystem.SetShelterModuleMortar(ShelterModuleMortar);
            SaveSystem.SetShelterModulePanicButton(ShelterModulePanicButton);
            SaveSystem.SetShelterModulePitfall(ShelterModulePitfall);
            SaveSystem.SetShelterModuleReloader(ShelterModuleReloader);
            SaveSystem.SetShelterModuleSorter(ShelterModuleSorter);
            SaveSystem.SetShelterModuleThermostat(ShelterModuleThermostat);
            SaveSystem.SetShelterModuleWasteChute(ShelterModuleWasteChute);
            SaveSystem.SetShelterModuleAutopsy(ShelterModuleAutopsy);
            SaveSystem.SetShelterModuleBatteryBank(ShelterModuleBatteryBank);
            SaveSystem.SetShelterModuleBioLatrine(ShelterModuleBioLatrine);
            SaveSystem.SetShelterModuleChoreBoard(ShelterModuleChoreBoard);
            SaveSystem.SetShelterModuleDeadManSwitch(ShelterModuleDeadManSwitch);
            SaveSystem.SetShelterModuleDeconShower(ShelterModuleDeconShower);
            SaveSystem.SetShelterModuleDialysis(ShelterModuleDialysis);
            SaveSystem.SetShelterModuleDistressBeacon(ShelterModuleDistressBeacon);
            SaveSystem.SetShelterModuleDronePad(ShelterModuleDronePad);
            SaveSystem.SetShelterModuleGarage(ShelterModuleGarage);
            SaveSystem.SetShelterModuleGunRack(ShelterModuleGunRack);
            SaveSystem.SetShelterModuleHammock(ShelterModuleHammock);
            SaveSystem.SetShelterModuleHandCrank(ShelterModuleHandCrank);
            SaveSystem.SetShelterModuleHotShower(ShelterModuleHotShower);
            SaveSystem.SetShelterModuleIncinerator(ShelterModuleIncinerator);
            SaveSystem.SetShelterModuleMagmaTap(ShelterModuleMagmaTap);
            SaveSystem.SetShelterModuleMotionSensor(ShelterModuleMotionSensor);
            SaveSystem.SetShelterModulePanicRoom(ShelterModulePanicRoom);
            SaveSystem.SetShelterModulePrintingPress(ShelterModulePrintingPress);
            SaveSystem.SetShelterModulePunchingBag(ShelterModulePunchingBag);
            SaveSystem.SetShelterModuleRainBarrel(ShelterModuleRainBarrel);
            SaveSystem.SetShelterModuleRecordPlayer(ShelterModuleRecordPlayer);
            SaveSystem.SetShelterModuleSprinklers(ShelterModuleSprinklers);
            SaveSystem.SetShelterModuleThumper(ShelterModuleThumper);
            SaveSystem.SetShelterModuleTreadmillGen(ShelterModuleTreadmillGen);
            SaveSystem.SetShelterModuleTurret(ShelterModuleTurret);
            SaveSystem.SetShelterModuleVaultDoor(ShelterModuleVaultDoor);
            SaveSystem.SetShelterModuleWoodStove(ShelterModuleWoodStove);
            SaveSystem.SetEventBrawl(EventBrawl);
            SaveSystem.SetEventComingOfAge(EventComingOfAge);
            SaveSystem.SetEventCultBlessing(EventCultBlessing);
            SaveSystem.SetEventCultInitiation(EventCultInitiation);
            SaveSystem.SetEventCultOfAi(EventCultOfAi);
            SaveSystem.SetEventEmpCascade(EventEmpCascade);
            SaveSystem.SetEventFeralRescue(EventFeralRescue);
            SaveSystem.SetEventFoundDiary(EventFoundDiary);
            SaveSystem.SetEventGriefCascade(EventGriefCascade);
            SaveSystem.SetEventHungerStrike(EventHungerStrike);
            SaveSystem.SetEventNodeCollapse(EventNodeCollapse);
            SaveSystem.SetEventRansomNote(EventRansomNote);
            SaveSystem.SetEventSchism(EventSchism);
            SaveSystem.SetEventSecretSociety(EventSecretSociety);
            SaveSystem.SetEventSiblingFeud(EventSiblingFeud);
            SaveSystem.SetEventSpontaneousMurder(EventSpontaneousMurder);
            SaveSystem.SetEventTeenRebellion(EventTeenRebellion);
            SaveSystem.SetEventWitchHunt(EventWitchHunt);
            SaveSystem.SetEventEuthanasiaPact(EventEuthanasiaPact);
            SaveSystem.SetEventFactionMerger(EventFactionMerger);
            SaveSystem.SetEventMudslide(EventMudslide);
            SaveSystem.SetEventNumbersStation(EventNumbersStation);
            SaveSystem.SetEventProjectSabotage(EventProjectSabotage);
            SaveSystem.SetEventSinkhole(EventSinkhole);
            SaveSystem.SetEventTriangulation(EventTriangulation);
            SaveSystem.SetEventVaultCollision(EventVaultCollision);
            SaveSystem.SetEventWarlordSuccession(EventWarlordSuccession);
            // ── CoreFamilies bulk Set (auto) ────────────────────────────
            SaveSystem.SetFalloutStormHazard(FalloutStormHazard);
            // demoted ghost — SetActionCrawlspace skipped
            // demoted ghost — SetActionPlay skipped
            // demoted ghost — SetActionSlaughterPet skipped
            // demoted ghost — SetActionTeachChild skipped
            // demoted ghost — SetActionTellStories skipped
            SaveSystem.SetItemAshGoat(ItemAshGoat);
            SaveSystem.SetItemBoots(ItemBoots);
            SaveSystem.SetItemLiveTrap(ItemLiveTrap);
            SaveSystem.SetItemMutantChicken(ItemMutantChicken);
            SaveSystem.SetItemToys(ItemToys);
            SaveSystem.SetTraitAshTongue(TraitAshTongue);
            SaveSystem.SetTraitKleptomaniac(TraitKleptomaniac);
            SaveSystem.SetTraitMascot(TraitMascot);
            SaveSystem.SetTraitStuntedEmpathy(TraitStuntedEmpathy);
            SaveSystem.SetTraitSuperstitious(TraitSuperstitious);
            SaveSystem.SetAfflictionBunkerFever(AfflictionBunkerFever);
            SaveSystem.SetAfflictionZoonoticFlu(AfflictionZoonoticFlu);
            SaveSystem.SetModuleRationLock(ModuleRationLock);
            SaveSystem.SetNodeOrphanage(NodeOrphanage);
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
            SaveSystem.SetAfflictionAdrenalineCrash(AfflictionAdrenalineCrash);
            SaveSystem.SetAfflictionAmnesia(AfflictionAmnesia);
            SaveSystem.SetAfflictionBrainwashed(AfflictionBrainwashed);
            SaveSystem.SetAfflictionBrittleBones(AfflictionBrittleBones);
            SaveSystem.SetAfflictionCaveMadness(AfflictionCaveMadness);
            SaveSystem.SetAfflictionFeralRegression(AfflictionFeralRegression);
            SaveSystem.SetAfflictionImaginaryFriend(AfflictionImaginaryFriend);
            SaveSystem.SetAfflictionNerveDamage(AfflictionNerveDamage);
            SaveSystem.SetAfflictionOldAge(AfflictionOldAge);
            SaveSystem.SetAfflictionPhantomLimb(AfflictionPhantomLimb);
            SaveSystem.SetAfflictionRadHallucinations(AfflictionRadHallucinations);
            SaveSystem.SetAfflictionRadiationBlindness(AfflictionRadiationBlindness);
            SaveSystem.SetAfflictionScurvyDegeneration(AfflictionScurvyDegeneration);
            SaveSystem.SetAfflictionSporeLung(AfflictionSporeLung);
            SaveSystem.SetAfflictionSterile(AfflictionSterile);
            SaveSystem.SetAfflictionSurvivorsGuilt(AfflictionSurvivorsGuilt);
            SaveSystem.SetAfflictionTBI(AfflictionTBI);
            SaveSystem.SetAfflictionThyroidCancer(AfflictionThyroidCancer);
            SaveSystem.SetAfflictionTrenchFoot(AfflictionTrenchFoot);
            SaveSystem.SetAudioEventDeafening(AudioEventDeafening);
            SaveSystem.SetAudioEventHeartbeat(AudioEventHeartbeat);
            SaveSystem.SetCombatBleedOut(CombatBleedOut);
            SaveSystem.SetCombatFlanking(CombatFlanking);
            SaveSystem.SetCombatSuppression(CombatSuppression);
            SaveSystem.SetCombatStanceLastStand(CombatStanceLastStand);
            SaveSystem.SetCrisisFeralFlora(CrisisFeralFlora);
            SaveSystem.SetCrisisStructuralFailure(CrisisStructuralFailure);
            SaveSystem.SetDurabilitySuppressor(DurabilitySuppressor);
            SaveSystem.SetEndgameUltimatum(EndgameUltimatum);
            SaveSystem.SetHazardCookOff(HazardCookOff);
            SaveSystem.SetHazardExplosiveCrafting(HazardExplosiveCrafting);
            SaveSystem.SetHazardFriendlyFire(HazardFriendlyFire);
            SaveSystem.SetHazardMethane(HazardMethane);
            SaveSystem.SetHazardMimicCrate(HazardMimicCrate);
            SaveSystem.SetHazardSurgicalBotch(HazardSurgicalBotch);
            SaveSystem.SetHazardWeaponBurst(HazardWeaponBurst);
            SaveSystem.SetHiddenStatUnseen(HiddenStatUnseen);
            SaveSystem.SetItemAICoreData(ItemAICoreData);
            SaveSystem.SetItemAmmoTypes(ItemAmmoTypes);
            SaveSystem.SetItemAmmonia(ItemAmmonia);
            SaveSystem.SetItemAmphetamines(ItemAmphetamines);
            SaveSystem.SetItemAshGhillie(ItemAshGhillie);
            SaveSystem.SetItemAutoDoc(ItemAutoDoc);
            SaveSystem.SetItemBioPlastic(ItemBioPlastic);
            SaveSystem.SetItemBloodBag(ItemBloodBag);
            SaveSystem.SetItemBoneSaw(ItemBoneSaw);
            SaveSystem.SetItemC4(ItemC4);
            SaveSystem.SetItemCaltrops(ItemCaltrops);
            SaveSystem.SetItemCarrierBird(ItemCarrierBird);
            SaveSystem.SetItemChildsDrawing(ItemChildsDrawing);
            SaveSystem.SetItemCigarettes(ItemCigarettes);
            SaveSystem.SetItemClimbingGear(ItemClimbingGear);
            SaveSystem.SetItemDecoy(ItemDecoy);
            SaveSystem.SetItemDogTags(ItemDogTags);
            SaveSystem.SetItemEMPGrenade(ItemEMPGrenade);
            SaveSystem.SetItemEncryptedDrive(ItemEncryptedDrive);
            SaveSystem.SetItemEpiPen(ItemEpiPen);
            SaveSystem.SetItemExosuit(ItemExosuit);
            SaveSystem.SetItemFaradayPack(ItemFaradayPack);
            SaveSystem.SetItemForeignBook(ItemForeignBook);
            SaveSystem.SetItemGeigerCalibrator(ItemGeigerCalibrator);
            SaveSystem.SetItemGlowingMushroom(ItemGlowingMushroom);
            SaveSystem.SetItemGoldBars(ItemGoldBars);
            SaveSystem.SetItemGuitar(ItemGuitar);
            SaveSystem.SetItemHeirloom(ItemHeirloom);
            SaveSystem.SetItemIBeam(ItemIBeam);
            SaveSystem.SetItemImpureIodine(ItemImpureIodine);
            SaveSystem.SetItemJuggernautArmor(ItemJuggernautArmor);
            SaveSystem.SetItemKevlarVest(ItemKevlarVest);
            SaveSystem.SetItemKeycards(ItemKeycards);
            SaveSystem.SetItemLandmine(ItemLandmine);
            SaveSystem.SetItemLeadApron(ItemLeadApron);
            SaveSystem.SetItemLiquidStitches(ItemLiquidStitches);
            SaveSystem.SetItemMaggots(ItemMaggots);
            SaveSystem.SetItemMilGasMask(ItemMilGasMask);
            SaveSystem.SetItemMutantGland(ItemMutantGland);
            SaveSystem.SetItemNanites(ItemNanites);
            SaveSystem.SetItemNightVision(ItemNightVision);
            SaveSystem.SetItemPackMule(ItemPackMule);
            SaveSystem.SetItemPasswordNote(ItemPasswordNote);
            SaveSystem.SetItemPhotoAlbum(ItemPhotoAlbum);
            SaveSystem.SetItemPotassiumIodide(ItemPotassiumIodide);
            SaveSystem.SetItemPresidentialSeal(ItemPresidentialSeal);
            SaveSystem.SetItemPrussianBlue(ItemPrussianBlue);
            SaveSystem.SetItemRTGBattery(ItemRTGBattery);
            SaveSystem.SetItemSeedLedger(ItemSeedLedger);
            SaveSystem.SetItemShockCollar(ItemShockCollar);
            SaveSystem.SetItemSnowshoes(ItemSnowshoes);
            SaveSystem.SetItemSurgicalTubing(ItemSurgicalTubing);
            SaveSystem.SetItemTearGas(ItemTearGas);
            SaveSystem.SetItemTeddyBear(ItemTeddyBear);
            SaveSystem.SetItemTrashHazmat(ItemTrashHazmat);
            SaveSystem.SetItemUndeliveredMail(ItemUndeliveredMail);
            SaveSystem.SetItemVacuumTubes(ItemVacuumTubes);
            SaveSystem.SetItemVinylCollection(ItemVinylCollection);
            SaveSystem.SetItemVitamins(ItemVitamins);
            SaveSystem.SetItemWalkieTalkie(ItemWalkieTalkie);
            SaveSystem.SetItemWastelandSoap(ItemWastelandSoap);
            SaveSystem.SetItemWaterTabs(ItemWaterTabs);
            SaveSystem.SetItemWeldingGoggles(ItemWeldingGoggles);
            SaveSystem.SetItemWristDosimeter(ItemWristDosimeter);
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
            SaveSystem.SetVehicleStrandingSystem(VehicleStrandingSystem);
            SaveSystem.SetVehicleSystem(VehicleSystem);
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
            SaveSystem.SetNodeAutomatedArmory(NodeAutomatedArmory);
            SaveSystem.SetNodeGhostShip(NodeGhostShip);
            SaveSystem.SetNodeMutantHive(NodeMutantHive);
            SaveSystem.SetNodePlayerBank(NodePlayerBank);
            SaveSystem.SetNodeSector7G(NodeSector7G);
            SaveSystem.SetNodeSporeHive(NodeSporeHive);
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
            SaveSystem.SetVehicleArmoredTruck(VehicleArmoredTruck);
            SaveSystem.SetVehicleMotorcycle(VehicleMotorcycle);
            SaveSystem.SetVehicleRowboat(VehicleRowboat);
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
            SaveSystem.SetWeaponChainsaw(WeaponChainsaw);
            SaveSystem.SetWeaponFlamethrower(WeaponFlamethrower);
            SaveSystem.SetWeaponHMG(WeaponHMG);
            SaveSystem.SetWeaponRPG(WeaponRPG);
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
                Debug.Log($"[Sabotaged Cache] Detected by {exp?.Survivor?.DisplayName}: {msg}");
            };

            // Prompt #14 — post-Day-30 windstorms move death-zone rad two path-hops.
            ShiftingHotspotSystem = new ShiftingHotspotSystem(new System.Random(_worldSeed + 20));
            ShiftingHotspotSystem.Bind(GeneratedMap, KnowledgeMap);
            SaveSystem.SetShiftingHotspotSystem(ShiftingHotspotSystem);
            ShiftingHotspotSystem.OnHotspotShifted += shift =>
            {
                if (shift == null) return;
                Debug.Log(
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
                Debug.Log($"[Hatch Entrapment] HatchState {prev} → {next}");
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
            // Prompt #768 — epilogue bullet tally from hatch raids.
            // Prompt #861 — heavy ammo spend counts as sniper/suppression strategy.
            if (HatchDefenseSystem != null)
            {
                HatchDefenseSystem.OnRaidResolved += result =>
                {
                    if (result.AmmoConsumed > 0)
                        EpilogueStats?.RecordBulletsFired(result.AmmoConsumed);
                    if (result.AmmoConsumed >= 10)
                        AdaptiveWarlords?.RecordStrategy(System_AdaptiveWarlords.StrategySnipers);
                };
            }

            // Prompt #861 — trap deployments → traps strategy; combat milestones → stealth/snipers.
            if (PerimeterTrapSystem != null)
            {
                PerimeterTrapSystem.OnTrapDeployed += () =>
                    AdaptiveWarlords?.RecordStrategy(System_AdaptiveWarlords.StrategyTraps);
            }
            if (CombatPerks != null)
            {
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

            // Prompt #174 / #182 — jam during hatch defense uses WeaponMaintenance clear ticks.
            if (HatchDefenseSystem != null && WeaponMaintenanceSystem != null)
            {
                HatchDefenseSystem.TryJamWeapon = (weaponId, clearTicks) =>
                    WeaponMaintenanceSystem.TryJam(weaponId, clearTicks: clearTicks);
            }

            PerimeterTrapSystem?.BindCombatPerks(
                CombatPerks,
                getSurvivor: id =>
                {
                    if (Survivors == null || string.IsNullOrEmpty(id)) return null;
                    for (int i = 0; i < Survivors.Count; i++)
                    {
                        if (Survivors[i] != null && Survivors[i].Id == id)
                            return Survivors[i];
                    }
                    return null;
                });

            ExpeditionSystem?.BindCombatPerks(
                CombatPerks,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0,
                affinity: MentalBreakSystem != null ? MentalBreakSystem.Affinity : null,
                getAllSurvivors: () => Survivors);
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
