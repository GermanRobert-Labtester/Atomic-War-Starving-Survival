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
            AfflictionAdrenalineCrash = new Affliction_AdrenalineCrash();
            AfflictionAmnesia = new AmnesiaSystem();
            AfflictionBrainwashed = new Affliction_Brainwashed("affliction_brainwashed");
            AfflictionBrittleBones = new BrittleBonesSystem();
            AfflictionCaveMadness = new CaveMadnessSystem("affliction_cave_madness");
            AfflictionFeralRegression = new FeralRegressionSystem();
            AfflictionImaginaryFriend = new ImaginaryFriendSystem();
            AfflictionNerveDamage = new Affliction_NerveDamage();
            AfflictionOldAge = new Affliction_OldAge();
            AfflictionPhantomLimb = new Affliction_PhantomLimb();
            AfflictionRadHallucinations = new Affliction_RadHallucinations();
            AfflictionRadiationBlindness = new RadiationBlindnessSystem();
            AfflictionScurvyDegeneration = new Affliction_ScurvyDegeneration();
            AfflictionSporeLung = new SporeLungSystem();
            AfflictionSterile = new Affliction_Sterile();
            AfflictionSurvivorsGuilt = new SurvivorsGuiltSystem();
            AfflictionTBI = new Affliction_TBI();
            AfflictionThyroidCancer = new Affliction_ThyroidCancer();
            AfflictionTrenchFoot = new TrenchFootSystem();
            Debug.Log("[GameBootstrap] Affliction family ready (19 systems).");
        }

        private void BootAudioEventFamily()
        {
            AudioEventDeafening = new AudioEvent_Deafening();
            AudioEventHeartbeat = new AudioEvent_Heartbeat();
            Debug.Log("[GameBootstrap] AudioEvent family ready (2 systems).");
        }

        private void BootCombatFamily()
        {
            CombatBleedOut = new Combat_BleedOut();
            CombatFlanking = new Combat_Flanking();
            CombatSuppression = new Combat_Suppression();
            Debug.Log("[GameBootstrap] Combat family ready (3 systems).");
        }

        private void BootCombatStanceFamily()
        {
            CombatStanceLastStand = new CombatStance_LastStand();
            Debug.Log("[GameBootstrap] CombatStance family ready (1 systems).");
        }

        private void BootCrisisFamily()
        {
            CrisisFeralFlora = new Crisis_FeralFlora();
            CrisisStructuralFailure = new Crisis_StructuralFailure();
            Debug.Log("[GameBootstrap] Crisis family ready (2 systems).");
        }

        private void BootDurabilityFamily()
        {
            DurabilitySuppressor = new Durability_Suppressor();
            Debug.Log("[GameBootstrap] Durability family ready (1 systems).");
        }

        private void BootEndgameFamily()
        {
            EndgameUltimatum = new Endgame_Ultimatum();
            Debug.Log("[GameBootstrap] Endgame family ready (1 systems).");
        }

        private void BootHazardFamily()
        {
            // DEMOTE-HazardCookOff-001 — no production host calls TryFire.
            // Keep Hazard_CookOff available as a dormant standalone class.
            // DEMOTE-HazardExplosiveCrafting-001 — no production host calls TryCraft.
            // Keep Hazard_ExplosiveCrafting available as a dormant standalone class.
            HazardFriendlyFire = new Hazard_FriendlyFire();
            HazardMethane = new MethaneSystem("hazard_methane");
            HazardMimicCrate = new Hazard_MimicCrate();
            HazardSurgicalBotch = new Hazard_SurgicalBotch();
            HazardWeaponBurst = new Hazard_WeaponBurst();
            Debug.Log("[GameBootstrap] Hazard family ready (5 systems; CookOff and ExplosiveCrafting dormant).");
        }

        private void BootHiddenStatFamily()
        {
            HiddenStatUnseen = new HiddenStat_Unseen();
            Debug.Log("[GameBootstrap] HiddenStat family ready (1 systems).");
        }

        private void BootItemFamily()
        {
            ItemAICoreData = new Item_AICoreData();
            ItemAmmoTypes = new Item_AmmoTypes();
            ItemAmmonia = new Item_Ammonia();
            ItemAmphetamines = new Item_Amphetamines();
            ItemAshGhillie = new Item_AshGhillie();
            ItemAutoDoc = new Item_AutoDoc();
            ItemBioPlastic = new Item_BioPlastic();
            ItemBloodBag = new Item_BloodBag();
            ItemBoneSaw = new Item_BoneSaw();
            ItemC4 = new Item_C4();
            ItemCaltrops = new Item_Caltrops();
            ItemCarrierBird = new Item_CarrierBird();
            ItemChildsDrawing = new Item_ChildsDrawing();
            ItemCigarettes = new Item_Cigarettes();
            ItemClimbingGear = new Item_ClimbingGear();
            ItemDecoy = new Item_Decoy();
            ItemDogTags = new Item_DogTags();
            ItemEMPGrenade = new Item_EMPGrenade();
            ItemEncryptedDrive = new Item_EncryptedDrive();
            ItemEpiPen = new Item_EpiPen();
            ItemExosuit = new Item_Exosuit();
            ItemFaradayPack = new Item_FaradayPack();
            ItemForeignBook = new Item_ForeignBook();
            ItemGeigerCalibrator = new Item_GeigerCalibrator();
            ItemGlowingMushroom = new GlowingMushroomSystem("item_glowing_mushroom");
            ItemGoldBars = new Item_GoldBars();
            ItemGuitar = new Item_Guitar();
            ItemHeirloom = new Item_Heirloom();
            ItemIBeam = new Item_IBeam();
            ItemImpureIodine = new Item_ImpureIodine();
            ItemJuggernautArmor = new Item_JuggernautArmor();
            ItemKevlarVest = new Item_KevlarVest();
            ItemKeycards = new Item_Keycards();
            ItemLandmine = new Item_Landmine();
            ItemLeadApron = new Item_LeadApron();
            ItemLiquidStitches = new Item_LiquidStitches();
            ItemMaggots = new Item_Maggots();
            ItemMilGasMask = new Item_MilGasMask();
            ItemMutantGland = new Item_MutantGland();
            ItemNanites = new Item_Nanites();
            ItemNightVision = new Item_NightVision();
            ItemPackMule = new Item_PackMule();
            ItemPasswordNote = new Item_PasswordNote();
            ItemPhotoAlbum = new Item_PhotoAlbum();
            ItemPotassiumIodide = new Item_PotassiumIodide();
            ItemPresidentialSeal = new Item_PresidentialSeal();
            ItemPrussianBlue = new Item_PrussianBlue();
            ItemRTGBattery = new Item_RTGBattery();
            ItemSeedLedger = new Item_SeedLedger();
            ItemShockCollar = new Item_ShockCollar();
            ItemSnowshoes = new Item_Snowshoes();
            ItemSurgicalTubing = new Item_SurgicalTubing();
            ItemTearGas = new Item_TearGas();
            ItemTeddyBear = new Item_TeddyBear();
            ItemTrashHazmat = new Item_TrashHazmat();
            ItemUndeliveredMail = new Item_UndeliveredMail();
            ItemVacuumTubes = new Item_VacuumTubes();
            ItemVinylCollection = new Item_VinylCollection();
            ItemVitamins = new Item_Vitamins();
            ItemWalkieTalkie = new Item_WalkieTalkie();
            ItemWastelandSoap = new Item_WastelandSoap();
            ItemWaterTabs = new Item_WaterTabs();
            ItemWeldingGoggles = new Item_WeldingGoggles();
            ItemWristDosimeter = new Item_WristDosimeter();
            Debug.Log("[GameBootstrap] Item family ready (64 systems).");
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
            VehicleStrandingSystem = new VehicleStrandingSystem();
            VehicleSystem = new VehicleSystem();
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
            NodeAutomatedArmory = new Node_AutomatedArmory();
            NodeGhostShip = new Node_GhostShip();
            NodeMutantHive = new Node_MutantHive();
            NodePlayerBank = new Node_PlayerBank();
            NodeSector7G = new Node_Sector7G();
            NodeSporeHive = new Node_SporeHive();
            Debug.Log("[GameBootstrap] Node family ready (6 systems).");
        }

        private void BootPetFamily()
        {
            PetFeralCat = new Pet_FeralCat();
            Debug.Log("[GameBootstrap] Pet family ready (1 systems).");
        }

        private void BootProjectFamily()
        {
            ProjectBioReactor = new Project_BioReactor();
            ProjectDeepWell = new Project_DeepWell();
            ProjectElevator = new Project_Elevator();
            ProjectMinecart = new Project_Minecart();
            ProjectRadioArray = new Project_RadioArray();
            ProjectSurfaceDome = new Project_SurfaceDome();
            Debug.Log("[GameBootstrap] Project family ready (6 systems).");
        }

        private void BootShelterEventFamily()
        {
            ShelterEventCaravanAmbush = new ShelterEvent_CaravanAmbush();
            ShelterEventFalseCure = new ShelterEvent_FalseCure();
            ShelterEventRansom = new ShelterEvent_Ransom();
            ShelterEventRefugees = new ShelterEvent_Refugees();
            ShelterEventTheMirror = new ShelterEvent_TheMirror();
            ShelterEventTribute = new ShelterEvent_Tribute();
            Debug.Log("[GameBootstrap] ShelterEvent family ready (6 systems).");
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
            TraitAnthropophobia = new Trait_Anthropophobia();
            TraitClairvoyant = new ClairvoyantSystem();
            TraitGenerationalTrauma = new Trait_GenerationalTrauma();
            TraitInheritedGenetics = new Trait_InheritedGenetics();
            TraitMatriarch = new Trait_Matriarch();
            TraitPTSD = new Trait_PTSD();
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
            VehicleArmoredTruck = new Vehicle_ArmoredTruck();
            VehicleMotorcycle = new Vehicle_Motorcycle();
            VehicleRowboat = new Vehicle_Rowboat();
            Debug.Log("[GameBootstrap] Vehicle family ready (3 systems).");
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
            WeaponChainsaw = new Weapon_Chainsaw();
            WeaponFlamethrower = new Weapon_Flamethrower();
            WeaponHMG = new Weapon_HMG();
            WeaponRPG = new Weapon_RPG();
            Debug.Log("[GameBootstrap] Weapon family ready (4 systems).");
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
            ItemAshGoat = new Item_AshGoat("item_ash_goat");
            ItemBoots = new Item_Boots();
            ItemLiveTrap = new Item_LiveTrap("item_live_trap");
            ItemMutantChicken = new Item_MutantChicken("item_mutant_chicken");
            ItemToys = new Item_Toys();
            TraitAshTongue = new Trait_AshTongue();
            TraitKleptomaniac = new Trait_Kleptomaniac();
            TraitMascot = new Trait_Mascot();
            TraitStuntedEmpathy = new Trait_StuntedEmpathy();
            TraitSuperstitious = new Trait_Superstitious();
            AfflictionBunkerFever = new Affliction_BunkerFever();
            AfflictionZoonoticFlu = new Affliction_ZoonoticFlu();
            ModuleRationLock = new Module_RationLock();
            NodeOrphanage = new Node_Orphanage();
            PetGuardDog = new Pet_GuardDog("pet_guard_dog");
            FalloutStormHazard = new FalloutStormHazardSystem(WeatherSystem);
            Debug.Log("[GameBootstrap] Remaining complex family ready (21 systems).");
        }

    }
}
