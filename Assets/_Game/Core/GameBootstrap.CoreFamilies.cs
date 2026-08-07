// GameBootstrap.CoreFamilies.cs — bulk Boot for remaining Core CaptureState systems.
using UnityEngine;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        private void BootActionFamily()
        {
            ActionAdministerPlacebo = new Action_AdministerPlacebo();
            ActionBarricadeDoor = new Action_BarricadeDoor();
            ActionBoilBatteries = new Action_BoilBatteries();
            ActionBroadcastPropaganda = new Action_BroadcastPropaganda();
            ActionBurnCharcoal = new Action_BurnCharcoal();
            ActionBuryTimeCapsule = new Action_BuryTimeCapsule();
            ActionCallCaravan = new Action_CallCaravan();
            ActionCoverTracks = new Action_CoverTracks();
            ActionCrackMainframe = new Action_CrackMainframe();
            ActionDecrypt = new Action_Decrypt();
            ActionDemandTribute = new Action_DemandTribute();
            ActionEstablishRoute = new Action_EstablishRoute();
            ActionExile = new Action_Exile();
            ActionFish = new Action_Fish();
            ActionHarvestOrgans = new Action_HarvestOrgans();
            ActionInfectSelf = new Action_InfectSelf();
            ActionIsotopeTrace = new Action_IsotopeTrace();
            ActionMercy = new Action_Mercy();
            ActionMixCement = new Action_MixCement();
            ActionMixChems = new Action_MixChems();
            ActionOverwatch = new Action_Overwatch();
            ActionPhysicalTherapy = new Action_PhysicalTherapy();
            ActionPirateRadio = new Action_PirateRadio();
            ActionPlaceBait = new Action_PlaceBait();
            ActionPullTooth = new Action_PullTooth();
            ActionRigCorpse = new Action_RigCorpse();
            ActionRoutePower = new Action_RoutePower();
            ActionSabotage = new Action_Sabotage();
            ActionScorchedEarth = new Action_ScorchedEarth();
            ActionSealRoom = new Action_SealRoom();
            ActionSelfSurgery = new Action_SelfSurgery();
            ActionSilentTakedown = new Action_SilentTakedown();
            ActionSiphonGas = new Action_SiphonGas();
            ActionStabilizeDNA = new Action_StabilizeDNA();
            ActionStargazing = new Action_Stargazing();
            ActionWorshipIdol = new Action_WorshipIdol();
            Debug.Log("[GameBootstrap] Action family ready (36 systems).");
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
            HazardCookOff = new Hazard_CookOff();
            HazardExplosiveCrafting = new Hazard_ExplosiveCrafting();
            HazardFriendlyFire = new Hazard_FriendlyFire();
            HazardMethane = new MethaneSystem("hazard_methane");
            HazardMimicCrate = new Hazard_MimicCrate();
            HazardSurgicalBotch = new Hazard_SurgicalBotch();
            HazardWeaponBurst = new Hazard_WeaponBurst();
            Debug.Log("[GameBootstrap] Hazard family ready (7 systems).");
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
            LocationArcade = new Location_Arcade();
            LocationSlaveMarket = new Location_SlaveMarket();
            LocationStrandedYacht = new Location_StrandedYacht();
            Debug.Log("[GameBootstrap] Location family ready (3 systems).");
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
            LocationStateRuinSystem = new LocationStateRuinSystem();
            MobileCampSystem = new MobileCampSystem();
            MoralDilemmaSystem = new MoralDilemmaSystem();
            NeedleSterilizationSystem = new NeedleSterilizationSystem();
            NightScavengeSystem = new NightScavengeSystem();
            ProstheticCraftingSystem = new ProstheticCraftingSystem();
            SeismicVentsSystem = new SeismicVentsSystem();
            SevereFrostbiteSystem = new SevereFrostbiteSystem();
            TetanusAfflictionSystem = new TetanusAfflictionSystem();
            TimeSystemSys = new TimeSystem();
            ToothDecaySystem = new ToothDecaySystem();
            VehicleStrandingSystem = new VehicleStrandingSystem();
            VehicleSystem = new VehicleSystem();
            VisionLossSystem = new VisionLossSystem();
            VisitorRNGSystem = new VisitorRNGSystem();
            Debug.Log("[GameBootstrap] Misc family ready (19 systems).");
        }

        private void BootNPCFamily()
        {
            NPCAddictsPassive = new NPC_AddictsPassive();
            NPCAggroScavengers = new NPC_AggroScavengers();
            NPCAggroTrader = new NPC_AggroTrader();
            NPCBandits = new NPC_Bandits();
            NPCBlackOps = new NPC_BlackOps();
            NPCBroker = new NPC_Broker();
            NPCCannibals = new NPC_Cannibals();
            NPCChemScientists = new NPC_ChemScientists();
            NPCCityResidents = new NPC_CityResidents();
            NPCCollaborators = new NPC_Collaborators();
            NPCConscripts = new NPC_Conscripts();
            NPCDesperateFamily = new NPC_DesperateFamily();
            NPCDrunksAggro = new NPC_DrunksAggro();
            NPCHomeless = new NPC_Homeless();
            NPCLonePsychopath = new NPC_LonePsychopath();
            NPCLooters = new NPC_Looters();
            NPCMercenaries = new NPC_Mercenaries();
            NPCMilitaryPatrol = new NPC_MilitaryPatrol();
            NPCPassiveScavengers = new NPC_PassiveScavengers();
            NPCPassiveTrader = new NPC_PassiveTrader();
            NPCPsychopathPair = new NPC_PsychopathPair();
            NPCRebelMilitia = new NPC_RebelMilitia();
            NPCRebelModerates = new NPC_RebelModerates();
            NPCRebelSnipers = new NPC_RebelSnipers();
            NPCRebelZealots = new NPC_RebelZealots();
            NPCSlavers = new NPC_Slavers();
            NPCSpecOps = new NPC_SpecOps();
            NPCSurvivalists = new NPC_Survivalists();
            NPCTaxCollector = new NPC_TaxCollector();
            NPCTerrorists = new NPC_Terrorists();
            NPCTheNegotiator = new NPC_TheNegotiator();
            NPCTheOld = new NPC_TheOld();
            NPCTheParents = new NPC_TheParents();
            NPCTravelingCouple = new NPC_TravelingCouple();
            Debug.Log("[GameBootstrap] NPC family ready (34 systems).");
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
            VisitorAbandonedState = new Visitor_AbandonedState();
            VisitorChurchHostile = new Visitor_ChurchHostile();
            VisitorChurchSanctuary = new Visitor_ChurchSanctuary();
            VisitorExplodedState = new Visitor_ExplodedState();
            VisitorFleeingHorde = new Visitor_FleeingHorde();
            VisitorHospitalPatients = new Visitor_HospitalPatients();
            VisitorHospitalStaff = new Visitor_HospitalStaff();
            VisitorMilTrainingYard = new Visitor_MilTrainingYard();
            VisitorQuestFaction = new Visitor_QuestFaction();
            VisitorRebelTrainingYard = new Visitor_RebelTrainingYard();
            Debug.Log("[GameBootstrap] Visitor family ready (10 systems).");
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
            ActionCrawlspace = new Action_Crawlspace();
            ActionPlay = new Action_Play();
            ActionSlaughterPet = new Action_SlaughterPet();
            ActionTeachChild = new Action_TeachChild();
            ActionTellStories = new Action_TellStories();
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
