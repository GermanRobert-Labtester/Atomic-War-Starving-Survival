// GameBootstrap.CoreFamilies.cs — bulk Boot for remaining Core CaptureState systems.
using UnityEngine;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        private void BootActionFamily()
        {
            // DEMOTE-Action-remaining — ActionAdministerPlacebo demoted (ghost). Class kept dormant.
            // DEMOTE-Action-remaining — ActionBarricadeDoor demoted (ghost). Class kept dormant.
            // DEMOTE-Action batch — ActionBoilBatteries demoted (ghost). Class kept dormant.
            // DEMOTE-Action batch — ActionBroadcastPropaganda demoted (ghost). Class kept dormant.
            // DEMOTE-Action batch — ActionBurnCharcoal demoted (ghost). Class kept dormant.
            // DEMOTE-Action-remaining — ActionBuryTimeCapsule demoted (ghost). Class kept dormant.
            // DEMOTE-Action batch — ActionCallCaravan demoted (ghost). Class kept dormant.
            // DEMOTE-Action batch — ActionCoverTracks demoted (ghost). Class kept dormant.
            // DEMOTE-Action-remaining — ActionCrackMainframe demoted (ghost). Class kept dormant.
            // DEMOTE-Action batch — ActionDecrypt demoted (ghost). Class kept dormant.
            // DEMOTE-Action batch — ActionDemandTribute demoted (ghost). Class kept dormant.
            // DEMOTE-Action batch — ActionEstablishRoute demoted (ghost). Class kept dormant.
            // DEMOTE-Action-remaining — ActionExile demoted (ghost). Class kept dormant.
            // DEMOTE-Action-remaining — ActionFish demoted (ghost). Class kept dormant.
            // DEMOTE-Action-remaining — ActionHarvestOrgans demoted (ghost). Class kept dormant.
            // DEMOTE-Action-remaining — ActionInfectSelf demoted (ghost). Class kept dormant.
            // DEMOTE-Action-remaining — ActionIsotopeTrace demoted (ghost). Class kept dormant.
            // DEMOTE-Action batch — ActionMercy demoted (ghost). Class kept dormant.
            // DEMOTE-Action-remaining — ActionMixCement demoted (ghost). Class kept dormant.
            // DEMOTE-Action-remaining — ActionMixChems demoted (ghost). Class kept dormant.
            // DEMOTE-Action-remaining — ActionOverwatch demoted (ghost). Class kept dormant.
            // DEMOTE-Action-remaining — ActionPhysicalTherapy demoted (ghost). Class kept dormant.
            // DEMOTE-Action batch — ActionPirateRadio demoted (ghost). Class kept dormant.
            // DEMOTE-Action-remaining — ActionPlaceBait demoted (ghost). Class kept dormant.
            // DEMOTE-Action-remaining — ActionPullTooth demoted (ghost). Class kept dormant.
            // DEMOTE-Action batch — ActionRigCorpse demoted (ghost). Class kept dormant.
            // DEMOTE-Action-remaining — ActionRoutePower demoted (ghost). Class kept dormant.
            // DEMOTE-Action batch — ActionSabotage demoted (ghost). Class kept dormant.
            // DEMOTE-Action batch — ActionScorchedEarth demoted (ghost). Class kept dormant.
            // DEMOTE-Action batch — ActionSealRoom demoted (ghost). Class kept dormant.
            // DEMOTE-Action-remaining — ActionSelfSurgery demoted (ghost). Class kept dormant.
            // DEMOTE-Action-remaining — ActionSilentTakedown demoted (ghost). Class kept dormant.
            // DEMOTE-Action batch — ActionSiphonGas demoted (ghost). Class kept dormant.
            // DEMOTE-001 — ActionStabilizeDNA removed from Boot/Save (ghost).
            // Class kept under Action_StabilizeDNA.cs until a real host calls Stabilize().
            // DEMOTE-Action-remaining — ActionStargazing demoted (ghost). Class kept dormant.
            // DEMOTE-Action-remaining — ActionWorshipIdol demoted (ghost). Class kept dormant.
            Debug.Log("[GameBootstrap] Action family demoted (all zero-ref Action_* ghosts; classes dormant).");
        }

        private void BootAfflictionFamily()
        {
            // DEMOTE-AfflictionItem-batch — AfflictionAdrenalineCrash demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — AfflictionAmnesia demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — AfflictionBrainwashed demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — AfflictionBrittleBones demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — AfflictionCaveMadness demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — AfflictionFeralRegression demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — AfflictionImaginaryFriend demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — AfflictionNerveDamage demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — AfflictionOldAge demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — AfflictionPhantomLimb demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — AfflictionRadHallucinations demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — AfflictionRadiationBlindness demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — AfflictionScurvyDegeneration demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — AfflictionSporeLung demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — AfflictionSterile demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — AfflictionSurvivorsGuilt demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — AfflictionTBI demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — AfflictionThyroidCancer demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — AfflictionTrenchFoot demoted (ghost). Class kept dormant.
            Debug.Log("[GameBootstrap] Affliction family demoted (pure ghosts dormant).");
        }

        private void BootAudioEventFamily()
        {
            // DEMOTE-NodeAudio-batch — AudioEventDeafening demoted (ghost). Class kept dormant.
            // DEMOTE-NodeAudio-batch — AudioEventHeartbeat demoted (ghost). Class kept dormant.
            Debug.Log("[GameBootstrap] AudioEvent family demoted.");
        }

        private void BootCombatFamily()
        {
            // DEMOTE-CombatVehicle-batch — CombatBleedOut demoted (ghost). Class kept dormant.
            // DEMOTE-CombatVehicle-batch — CombatFlanking demoted (ghost). Class kept dormant.
            // DEMOTE-CombatVehicle-batch — CombatSuppression demoted (ghost). Class kept dormant.
            Debug.Log("[GameBootstrap] Combat family demoted.");
        }

        private void BootCombatStanceFamily()
        {
            // DEMOTE-CombatVehicle-batch — CombatStanceLastStand demoted (ghost). Class kept dormant.
            Debug.Log("[GameBootstrap] CombatStance family demoted.");
        }

        private void BootCrisisFamily()
        {
            // DEMOTE-CombatVehicle-batch — CrisisFeralFlora demoted (ghost). Class kept dormant.
            // DEMOTE-CombatVehicle-batch — CrisisStructuralFailure demoted (ghost). Class kept dormant.
            Debug.Log("[GameBootstrap] Crisis family demoted.");
        }

        private void BootDurabilityFamily()
        {
            // DEMOTE-NodeAudio-batch — DurabilitySuppressor demoted (ghost). Class kept dormant.
            Debug.Log("[GameBootstrap] Durability family demoted.");
        }

        private void BootEndgameFamily()
        {
            // DEMOTE-NodeAudio-batch — EndgameUltimatum demoted (ghost). Class kept dormant.
            Debug.Log("[GameBootstrap] Endgame family demoted.");
        }

        private void BootHazardFamily()
        {
            // DEMOTE-HazardCookOff-001 — no production host calls TryFire.
            // Keep Hazard_CookOff available as a dormant standalone class.
            // DEMOTE-HazardExplosiveCrafting-001 — no production host calls TryCraft.
            // Keep Hazard_ExplosiveCrafting available as a dormant standalone class.
            // DEMOTE-HazardFriendlyFire-001 — no production host calls CheckFriendlyFire.
            // Keep Hazard_FriendlyFire available as a dormant standalone class.
            HazardMethane = new MethaneSystem("hazard_methane");
            HazardMimicCrate = new Hazard_MimicCrate();
            HazardSurgicalBotch = new Hazard_SurgicalBotch();
            HazardWeaponBurst = new Hazard_WeaponBurst();
            Debug.Log("[GameBootstrap] Hazard family ready (4 live systems; 3 ghosts dormant).");
        }

        private void BootHiddenStatFamily()
        {
            // DEMOTE-NodeAudio-batch — HiddenStatUnseen demoted (ghost). Class kept dormant.
            Debug.Log("[GameBootstrap] HiddenStat family demoted.");
        }

        private void BootItemFamily()
        {
            // DEMOTE-AfflictionItem-batch — ItemAICoreData demoted (ghost). Class kept dormant.
            ItemAmmoTypes = new Item_AmmoTypes();
            // DEMOTE-AfflictionItem-batch — ItemAmmonia demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemAmphetamines demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemAshGhillie demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemAutoDoc demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemBioPlastic demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemBloodBag demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemBoneSaw demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemC4 demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemCaltrops demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemCarrierBird demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemChildsDrawing demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemCigarettes demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemClimbingGear demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemDecoy demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemDogTags demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemEMPGrenade demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemEncryptedDrive demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemEpiPen demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemExosuit demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemFaradayPack demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemForeignBook demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemGeigerCalibrator demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemGlowingMushroom demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemGoldBars demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemGuitar demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemHeirloom demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemIBeam demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemImpureIodine demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemJuggernautArmor demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemKevlarVest demoted (ghost). Class kept dormant.
            ItemKeycards = new Item_Keycards(); // REPROMOTE-Item-001
            // DEMOTE-AfflictionItem-batch — ItemLandmine demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemLeadApron demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemLiquidStitches demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemMaggots demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemMilGasMask demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemMutantGland demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemNanites demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemNightVision demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemPackMule demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemPasswordNote demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemPhotoAlbum demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemPotassiumIodide demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemPresidentialSeal demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemPrussianBlue demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemRTGBattery demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemSeedLedger demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemShockCollar demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemSnowshoes demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemSurgicalTubing demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemTearGas demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemTeddyBear demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemTrashHazmat demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemUndeliveredMail demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemVacuumTubes demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemVinylCollection demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemVitamins demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemWalkieTalkie demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemWastelandSoap demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemWaterTabs demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemWeldingGoggles demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemWristDosimeter demoted (ghost). Class kept dormant.
            Debug.Log("[GameBootstrap] Item family: AmmoTypes+Keycards live; remaining pure ghosts demoted.");
        }

        private void BootLocationFamily()
        {
            // DEMOTE-Location-batch — LocationArcade demoted (ghost). Class kept dormant.
            // DEMOTE-Location-batch — LocationSlaveMarket demoted (ghost). Class kept dormant.
            // DEMOTE-Location-batch — LocationStrandedYacht demoted (ghost). Class kept dormant.
            Debug.Log("[GameBootstrap] Location family demoted (3 ghosts dormant).");
        }

        private void BootMapFamily()
        {
            MapAquifer = new AquiferSystem("map_aquifer");
            Debug.Log("[GameBootstrap] Map family ready (1 systems).");
        }

        private void BootMiscFamily()
        {
            AshDriftSystem = new AshDriftSystem();
            BurnWardSystem = new BurnWardSystem();
            CognitiveDecaySystem = new CognitiveDecaySystem();
            LightningStrikesSystem = new LightningStrikesSystem();
            // DEMOTE-Location-batch — LocationStateRuinSystem demoted (ghost). Class kept dormant.
            MobileCampSystem = new MobileCampSystem();
            MoralDilemmaSystem = new MoralDilemmaSystem();
            NeedleSterilizationSystem = new NeedleSterilizationSystem();
            NightScavengeSystem = new NightScavengeSystem();
            ProstheticCraftingSystem = new ProstheticCraftingSystem();
            SeismicVentsSystem = new SeismicVentsSystem();
            SevereFrostbiteSystem = new SevereFrostbiteSystem();
            TetanusAfflictionSystem = new TetanusAfflictionSystem();
            ToothDecaySystem = new ToothDecaySystem();
            // DEMOTE-CombatVehicle-batch — VehicleStrandingSystem demoted (ghost). Class kept dormant.
            // DEMOTE-CombatVehicle-batch — VehicleSystem demoted (ghost). Class kept dormant.
            VisionLossSystem = new VisionLossSystem();
            // DEMOTE-VisitorRNG — VisitorRNGSystem demoted (ghost). Class kept dormant.
            Debug.Log("[GameBootstrap] Misc family ready (VisitorRNG demoted).");
        }

        private void BootNPCFamily()
        {
            // DEMOTE-NPC-batch — NPCAddictsPassive demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCAggroScavengers demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCAggroTrader demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCBandits demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCBlackOps demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCBroker demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCCannibals demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCChemScientists demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCCityResidents demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCCollaborators demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCConscripts demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCDesperateFamily demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCDrunksAggro demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCHomeless demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCLonePsychopath demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCLooters demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCMercenaries demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCMilitaryPatrol demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCPassiveScavengers demoted (ghost). Class kept dormant.
            // REPROMOTE-001 — PassiveTrader live: weather price mult → DynamicEconomySystem.
            NPCPassiveTrader = new NPC_PassiveTrader();
            // DEMOTE-NPC-batch — NPCPsychopathPair demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCRebelMilitia demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCRebelModerates demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCRebelSnipers demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCRebelZealots demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-001 — NPCSlavers demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCSpecOps demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCSurvivalists demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCTaxCollector demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCTerrorists demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCTheNegotiator demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCTheOld demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCTheParents demoted (ghost). Class kept dormant.
            // DEMOTE-NPC-batch — NPCTravelingCouple demoted (ghost). Class kept dormant.
            Debug.Log("[GameBootstrap] NPC family: PassiveTrader live (REPROMOTE-001); remaining ghosts dormant.");
        }

        private void BootNodeFamily()
        {
            // DEMOTE-NodeAudio-batch — NodeAutomatedArmory demoted (ghost). Class kept dormant.
            // DEMOTE-NodeAudio-batch — NodeGhostShip demoted (ghost). Class kept dormant.
            // DEMOTE-NodeAudio-batch — NodeMutantHive demoted (ghost). Class kept dormant.
            // DEMOTE-NodeAudio-batch — NodePlayerBank demoted (ghost). Class kept dormant.
            // DEMOTE-NodeAudio-batch — NodeSector7G demoted (ghost). Class kept dormant.
            // DEMOTE-NodeAudio-batch — NodeSporeHive demoted (ghost). Class kept dormant.
            Debug.Log("[GameBootstrap] Node family demoted.");
        }

        private void BootPetFamily()
        {
            // DEMOTE-TraitPetProject-batch — PetFeralCat demoted (ghost). Class kept dormant.
            Debug.Log("[GameBootstrap] Pet family demoted.");
        }

        private void BootProjectFamily()
        {
            // DEMOTE-TraitPetProject-batch — ProjectBioReactor demoted (ghost). Class kept dormant.
            // DEMOTE-TraitPetProject-batch — ProjectDeepWell demoted (ghost). Class kept dormant.
            // DEMOTE-TraitPetProject-batch — ProjectElevator demoted (ghost). Class kept dormant.
            // DEMOTE-TraitPetProject-batch — ProjectMinecart demoted (ghost). Class kept dormant.
            // DEMOTE-TraitPetProject-batch — ProjectRadioArray demoted (ghost). Class kept dormant.
            // DEMOTE-TraitPetProject-batch — ProjectSurfaceDome demoted (ghost). Class kept dormant.
            Debug.Log("[GameBootstrap] Project family demoted.");
        }

        private void BootShelterEventFamily()
        {
            // DEMOTE-TraitPetProject-batch — ShelterEventCaravanAmbush demoted (ghost). Class kept dormant.
            // DEMOTE-TraitPetProject-batch — ShelterEventFalseCure demoted (ghost). Class kept dormant.
            // DEMOTE-TraitPetProject-batch — ShelterEventRansom demoted (ghost). Class kept dormant.
            // DEMOTE-TraitPetProject-batch — ShelterEventRefugees demoted (ghost). Class kept dormant.
            // DEMOTE-TraitPetProject-batch — ShelterEventTheMirror demoted (ghost). Class kept dormant.
            // DEMOTE-TraitPetProject-batch — ShelterEventTribute demoted (ghost). Class kept dormant.
            Debug.Log("[GameBootstrap] ShelterEvent family demoted.");
        }

        private void BootSkirmishFamily()
        {
            SkirmishBandit_vs_Terror = new Skirmish_Bandit_vs_Terror("skirmish_bandit_vs_terror");
            SkirmishMil_vs_Rebel = new Skirmish_Mil_vs_Rebel("skirmish_mil_vs_rebel");
            SkirmishMil_vs_Terror = new Skirmish_Mil_vs_Terror("skirmish_mil_vs_terror");
            SkirmishRebel_vs_Bandit = new Skirmish_Rebel_vs_Bandit("skirmish_rebel_vs_bandit");
            SkirmishRebel_vs_Terror = new Skirmish_Rebel_vs_Terror("skirmish_rebel_vs_terror");
            Debug.Log("[GameBootstrap] Skirmish family ready (5 systems).");
        }

        private void BootTraderFamily()
        {
            TraderPlagueConvoy = new Trader_PlagueConvoy();
            Debug.Log("[GameBootstrap] Trader family ready (1 systems).");
        }

        private void BootTraitFamily()
        {
            // DEMOTE-TraitPetProject-batch — TraitAnthropophobia demoted (ghost). Class kept dormant.
            // DEMOTE-TraitPetProject-batch — TraitClairvoyant demoted (ghost). Class kept dormant.
            // DEMOTE-TraitPetProject-batch — TraitGenerationalTrauma demoted (ghost). Class kept dormant.
            // DEMOTE-TraitPetProject-batch — TraitInheritedGenetics demoted (ghost). Class kept dormant.
            // DEMOTE-TraitPetProject-batch — TraitMatriarch demoted (ghost). Class kept dormant.
            // DEMOTE-TraitPetProject-batch — TraitPTSD demoted (ghost). Class kept dormant.
            Debug.Log("[GameBootstrap] Trait family ready (6 systems).");
        }

        private void BootUIEventFamily()
        {
            UIEventBlurredVision = new UIEvent_BlurredVision();
            UIEventCorruptionScare = new UIEvent_CorruptionScare();
            UIEventFalseInventory = new UIEvent_FalseInventory();
            UIEventGhostRadio = new UIEvent_GhostRadio();
            UIEventHacking = new UIEvent_Hacking();
            UIEventLowPower = new UIEvent_LowPower();
            UIEventMapRot = new UIEvent_MapRot();
            UIEventPhantomBlip = new PhantomBlipSystem();
            Debug.Log("[GameBootstrap] UIEvent family ready (8 systems).");
        }

        private void BootVehicleFamily()
        {
            // DEMOTE-CombatVehicle-batch — VehicleArmoredTruck demoted (ghost). Class kept dormant.
            // DEMOTE-CombatVehicle-batch — VehicleMotorcycle demoted (ghost). Class kept dormant.
            // DEMOTE-CombatVehicle-batch — VehicleRowboat demoted (ghost). Class kept dormant.
            Debug.Log("[GameBootstrap] Vehicle family demoted.");
        }

        private void BootVisitorFamily()
        {
            // DEMOTE-Visitor-batch — VisitorAbandonedState demoted (ghost). Class kept dormant.
            // DEMOTE-Visitor-batch — VisitorChurchHostile demoted (ghost). Class kept dormant.
            // DEMOTE-Visitor-batch — VisitorChurchSanctuary demoted (ghost). Class kept dormant.
            // DEMOTE-Visitor-batch — VisitorExplodedState demoted (ghost). Class kept dormant.
            // DEMOTE-Visitor-batch — VisitorFleeingHorde demoted (ghost). Class kept dormant.
            // DEMOTE-Visitor-batch — VisitorHospitalPatients demoted (ghost). Class kept dormant.
            // DEMOTE-Visitor-batch — VisitorHospitalStaff demoted (ghost). Class kept dormant.
            // DEMOTE-Visitor-batch — VisitorMilTrainingYard demoted (ghost). Class kept dormant.
            // DEMOTE-Visitor-batch — VisitorQuestFaction demoted (ghost). Class kept dormant.
            // DEMOTE-Visitor-batch — VisitorRebelTrainingYard demoted (ghost). Class kept dormant.
            Debug.Log("[GameBootstrap] Visitor family demoted (10 ghosts dormant).");
        }

        private void BootWeaponFamily()
        {
            // DEMOTE-CombatVehicle-batch — WeaponChainsaw demoted (ghost). Class kept dormant.
            // DEMOTE-CombatVehicle-batch — WeaponFlamethrower demoted (ghost). Class kept dormant.
            WeaponHMG = new Weapon_HMG(); // REPROMOTE-Weapon-001
            // DEMOTE-CombatVehicle-batch — WeaponRPG demoted (ghost). Class kept dormant.
            Debug.Log("[GameBootstrap] Weapon family demoted.");
        }

        private void BootWorldEventFamily()
        {
            WorldEventDeforestation = new WorldEvent_Deforestation();
            WorldEventFinalWinter = new WorldEvent_FinalWinter();
            WorldEventFissure = new WorldEvent_Fissure();
            WorldEventGreatFamine = new WorldEvent_GreatFamine();
            WorldEventMegafauna = new WorldEvent_Megafauna();
            Debug.Log("[GameBootstrap] WorldEvent family ready (5 systems).");
        }

        private void BootRemainingComplexFamily()
        {
            // DEMOTE-Action batch — ActionCrawlspace demoted (ghost). Class kept dormant.
            // DEMOTE-Action batch — ActionPlay demoted (ghost). Class kept dormant.
            // DEMOTE-Action batch — ActionSlaughterPet demoted (ghost). Class kept dormant.
            // DEMOTE-Action batch — ActionTeachChild demoted (ghost). Class kept dormant.
            // DEMOTE-Action-remaining — ActionTellStories demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemAshGoat demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemBoots demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemLiveTrap demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemMutantChicken demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — ItemToys demoted (ghost). Class kept dormant.
            // DEMOTE-TraitPetProject-batch — TraitAshTongue demoted (ghost). Class kept dormant.
            // DEMOTE-TraitPetProject-batch — TraitKleptomaniac demoted (ghost). Class kept dormant.
            // DEMOTE-TraitPetProject-batch — TraitMascot demoted (ghost). Class kept dormant.
            // DEMOTE-TraitPetProject-batch — TraitStuntedEmpathy demoted (ghost). Class kept dormant.
            // DEMOTE-TraitPetProject-batch — TraitSuperstitious demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — AfflictionBunkerFever demoted (ghost). Class kept dormant.
            // DEMOTE-AfflictionItem-batch — AfflictionZoonoticFlu demoted (ghost). Class kept dormant.
            // DEMOTE-TraitPetProject-batch — ModuleRationLock demoted (ghost). Class kept dormant.
            // DEMOTE-NodeAudio-batch — NodeOrphanage demoted (ghost). Class kept dormant.
            PetGuardDog = new Pet_GuardDog("pet_guard_dog"); // REPROMOTE-Pet-001
            FalloutStormHazard = new FalloutStormHazardSystem(WeatherSystem);
            Debug.Log("[GameBootstrap] Remaining complex family ready (21 systems).");
        }

    }
}
