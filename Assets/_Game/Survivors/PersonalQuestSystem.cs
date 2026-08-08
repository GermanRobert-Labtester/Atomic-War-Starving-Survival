using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Personal Quest Engine &amp; Latent Expert Traits (Prompts #214–#283).
    /// Survivors do not start with their Expert Trait. After 30 days alive OR
    /// a Morale 0→100 recovery, their assigned questline begins. Completing
    /// the final stage permanently unlocks the latent expert trait.
    /// Base personality traits (Selfless, Workaholic, …) are granted on assign.
    /// Plain C#, save/load safe, inventory-free (Survivors leaf assembly).
    /// </summary>
    public partial class PersonalQuestSystem
    {
        // ── Latent expert trait ids ──────────────────────────────────────
        public const string MiracleWorkerId = "trait_miracle_worker";
        public const string AlchemistId = "trait_alchemist";
        public const string ZoonoticExpertId = "trait_zoonotic_expert";
        public const string AnchorId = "trait_anchor";
        public const string DeathBlindId = "trait_death_blind";
        // Prompts #220–#224
        public const string WarlordId = "trait_warlord";
        public const string PeacekeeperId = "trait_peacekeeper";
        public const string JuggernautId = "trait_juggernaut";
        public const string ApexPredatorId = "trait_apex_predator";
        public const string SurvivalistId = "trait_survivalist";
        // Prompts #225–#234
        public const string HydraulicMasterId = "trait_hydraulic_master";
        public const string GridWalkerId = "trait_grid_walker";
        public const string VaultBuilderId = "trait_vault_builder";
        public const string GreaseMonkeyId = "trait_grease_monkey";
        public const string SynthesizerId = "trait_synthesizer";
        public const string GaiaId = "trait_gaia";
        public const string WastelandRunnerId = "trait_wasteland_runner";
        public const string GhostId = "trait_ghost";
        public const string StormcallerId = "trait_stormcaller";
        public const string RadWalkerId = "trait_rad_walker";
        // Prompts #235–#248
        public const string PolymathId = "trait_polymath";
        public const string DemagogueId = "trait_demagogue";
        public const string ShepherdId = "trait_shepherd";
        public const string MuckrakerId = "trait_muckraker";
        public const string VoiceOfTheWastesId = "trait_voice_of_the_wastes";
        public const string IronChefId = "trait_iron_chef";
        public const string TirelessId = "trait_tireless";
        public const string AsbestosId = "trait_asbestos";
        public const string ArmorerId = "trait_armorer";
        public const string TinkererId = "trait_tinkerer";
        public const string LorekeeperId = "trait_lorekeeper";
        public const string ZealotsBaneId = "trait_zealots_bane";
        public const string ChemResistantId = "trait_chem_resistant";
        public const string ProtectorId = "trait_protector";
        // Prompts #249–#256 latent
        public const string MatriarchId = "trait_matriarch";
        public const string PillarOfAtlasId = "trait_pillar_of_atlas";
        public const string WastelandScoutId = "trait_wasteland_scout";
        public const string ChildOfTheAshId = "trait_child_of_the_ash";
        public const string ColdCalculusId = "trait_cold_calculus";
        public const string ButcherOfDay30Id = "trait_butcher_of_day_30";
        public const string MasterManipulatorId = "trait_master_manipulator";
        public const string DragonsHoardId = "trait_dragons_hoard";
        // Prompts #257–#266 latent
        public const string ArtOfWarId = "trait_art_of_war";
        public const string DemolitionsExpertId = "trait_demolitions_expert";
        public const string GhostShooterId = "trait_ghost_shooter";
        public const string SupplyChainMasterId = "trait_supply_chain_master";
        public const string ReclaimedYouthId = "trait_reclaimed_youth";
        public const string SoulWeaverId = "trait_soul_weaver";
        public const string LoneWolfId = "trait_lone_wolf";
        public const string GroundedOptimistId = "trait_grounded_optimist";
        public const string LivingSaintId = "trait_living_saint";
        public const string HumbledHealerId = "trait_humbled_healer";
        // Prompts #267–#276 latent
        public const string CleanAndSoberId = "trait_clean_and_sober";
        public const string TheWatcherId = "trait_the_watcher";
        public const string HyperAwareId = "trait_hyper_aware";
        public const string FireBreatherId = "trait_fire_breather";
        public const string SonarId = "trait_sonar";
        public const string ImprovisedEngineeringId = "trait_improvised_engineering";
        public const string RadiotrophicId = "trait_radiotrophic";
        public const string ApexScavengerId = "trait_apex_scavenger";
        public const string ZenStateId = "trait_zen_state";
        public const string MasterGeneticistId = "trait_master_geneticist";
        // Prompts #277–#283 latent
        public const string TheEnforcerId = "trait_the_enforcer";
        public const string LegendOfTheWastesId = "trait_legend_of_the_wastes";
        public const string TheStatesmanId = "trait_the_statesman";
        public const string CyberneticsId = "trait_cybernetics";
        public const string BeaconOfTruthId = "trait_beacon_of_truth";
        public const string MasterPathologistId = "trait_master_pathologist";
        public const string MonopolistId = "trait_monopolist";
        // Prompts #284–#298 latent
        public const string DeepDelverId = "trait_deep_delver";
        public const string LogisticsMasterId = "trait_logistics_master";
        public const string ForgeMasterId = "trait_forge_master";
        public const string SanitizationExpertId = "trait_sanitization_expert";
        public const string DeforesterId = "trait_deforester";
        public const string EpidemiologistId = "trait_epidemiologist";
        public const string CelestialNavigatorId = "trait_celestial_navigator";
        public const string ArchivistId = "trait_archivist";
        public const string AuditorId = "trait_auditor";
        public const string MaestroId = "trait_maestro";
        public const string BlockadeRunnerId = "trait_blockade_runner";
        public const string ExecutionerId = "trait_executioner";
        public const string ShadowId = "trait_shadow";
        public const string MasterOfDisguiseId = "trait_master_of_disguise";
        public const string MechanicProdigyId = "trait_mechanic_prodigy";
        // Prompts #299–#318 latent
        public const string DiplomatId = "trait_diplomat";
        public const string WastelandGladiatorId = "trait_wasteland_gladiator";
        public const string ChiefOfMedicineId = "trait_chief_of_medicine";
        public const string DroneOperatorId = "trait_drone_operator";
        public const string ChoirOfOneId = "trait_choir_of_one";
        public const string HiveTacticsId = "trait_hive_tactics";
        public const string HiveHealingId = "trait_hive_healing";
        public const string TruthSeekerId = "trait_truth_seeker";
        public const string WildmanId = "trait_wildman";
        public const string SecondLifeId = "trait_second_life";
        public const string IronWillId = "trait_iron_will";
        public const string UnseenListenerId = "trait_unseen_listener";
        public const string RuthlessCapitalistId = "trait_ruthless_capitalist";
        public const string ProdigyId = "trait_prodigy";
        public const string CommanderId = "trait_commander";
        public const string CyberArmId = "trait_cyber_arm";
        public const string RedemptionId = "trait_redemption";
        public const string OverclockedId = "trait_overclocked";
        public const string WastelandGuardianId = "trait_wasteland_guardian";
        public const string OmniscienceId = "trait_omniscience";

        // ── Base personality traits (granted on profile assign) ──────────
        public const string SelflessId = "trait_selfless";
        public const string WorkaholicId = "trait_workaholic";
        public const string DependentId = "trait_dependent";
        public const string PollyannaId = "trait_pollyanna";
        public const string TraumatizedId = "trait_traumatized";
        public const string SociopathId = "trait_sociopath";
        public const string ArrogantId = "trait_arrogant";
        public const string KindId = "trait_kind";
        public const string CharismaticId = "trait_charismatic";
        public const string DeceptiveId = "trait_deceptive";
        public const string SelfishId = "trait_selfish";
        // Prompts #257–#266 base
        public const string TacticianId = "trait_tactician";
        public const string HatedId = "trait_hated";
        public const string AntiAuthorityId = "trait_anti_authority";
        public const string CowardId = "trait_coward";
        public const string StrictId = "trait_strict";
        public const string StuntedId = "trait_stunted";
        public const string HyperEmpatheticId = "trait_hyper_empathetic";
        public const string RudeId = "trait_rude";
        public const string DenialistId = "trait_denialist";
        public const string SacrificialId = "trait_sacrificial";
        public const string GodComplexId = "trait_god_complex";
        // Prompts #267–#276 base
        public const string RestlessId = "trait_restless";
        public const string ParanoidHealthId = "trait_paranoid_health";
        public const string FascinationId = "trait_fascination";
        public const string BlindId = "trait_blind";
        public const string ParanoidId = "trait_paranoid";
        public const string AnimalisticId = "trait_animalistic";
        public const string VowOfNonviolenceId = "trait_vow_of_nonviolence";
        public const string GrievingId = "trait_grieving";
        // Prompts #277–#283 base
        public const string DistrustedId = "trait_distrusted";
        public const string MoralCompassId = "trait_moral_compass";
        public const string FailingHeartId = "trait_failing_heart";
        public const string SilverTongueId = "trait_silver_tongue";
        public const string DelusionalId = "trait_delusional";
        public const string PhotogenicId = "trait_photogenic";
        public const string AgoraphileId = "trait_agoraphile";
        public const string RuthlessId = "trait_ruthless";
        // Prompts #284–#298 base
        public const string BlackLungId = "trait_black_lung";
        public const string ClaustrophilicId = "trait_claustrophilic";
        public const string CaffeinatedId = "trait_caffeinated";
        public const string CallousedId = "trait_calloused";
        public const string DeafInOneEarId = "trait_deaf_in_one_ear";
        public const string InvisibleId = "trait_invisible";
        public const string NeatFreakId = "trait_neat_freak";
        public const string BrawnId = "trait_brawn";
        public const string GermaphobeId = "trait_germaphobe";
        public const string NightOwlId = "trait_night_owl";
        public const string QuietId = "trait_quiet";
        public const string FrailId = "trait_frail";
        public const string PennyPincherId = "trait_penny_pincher";
        public const string FragileEgoId = "trait_fragile_ego";
        public const string ShadyId = "trait_shady";
        public const string ProfessionalId = "trait_professional";
        public const string SlightOfHandId = "trait_slight_of_hand";
        public const string CounterfeiterId = "trait_counterfeiter";
        public const string AntsyId = "trait_antsy";
        // Prompts #299–#318 base
        public const string SpoiledId = "trait_spoiled";
        public const string HighMetabolismId = "trait_high_metabolism";
        public const string TextbookKnowledgeId = "trait_textbook_knowledge";
        public const string AgoraphobicId = "trait_agoraphobic";
        public const string InnocentId = "trait_innocent";
        public const string SymbioticBondId = "trait_symbiotic_bond";
        public const string TinfoilHatId = "trait_tinfoil_hat";
        public const string UncivilizedId = "trait_uncivilized";
        public const string ComatoseBurdenId = "trait_comatose_burden";
        public const string BrandedId = "trait_branded";
        public const string UndocumentedId = "trait_undocumented";
        public const string EntitledId = "trait_entitled";
        public const string ClumsyId = "trait_clumsy";
        public const string BurnScarsId = "trait_burn_scars";
        public const string MissingArmId = "trait_missing_arm";
        public const string LethalId = "trait_lethal";
        public const string UntrustedId = "trait_untrusted";
        public const string AndroidId = "trait_android";
        public const string GoodBoyId = "trait_good_boy";
        public const string BunkerCoreId = "trait_bunker_core";

        // ── Archetype ids ────────────────────────────────────────────────
        public const string SurgeonId = "the_surgeon";
        public const string PharmacistId = "the_pharmacist";
        public const string VetId = "the_vet";
        public const string TherapistId = "the_therapist";
        public const string UndertakerId = "the_undertaker";
        public const string VeteranId = "the_veteran";
        public const string CopId = "the_cop";
        public const string BouncerId = "the_bouncer";
        public const string HunterId = "the_hunter";
        public const string PrisonerId = "the_prisoner";
        public const string PlumberId = "the_plumber";
        public const string ElectricianId = "the_electrician";
        public const string ArchitectId = "the_architect";
        public const string MechanicId = "the_mechanic";
        public const string ChemistId = "the_chemist";
        public const string BotanistId = "the_botanist";
        public const string CourierId = "the_courier";
        public const string BurglarId = "the_burglar";
        public const string MeteorologistId = "the_meteorologist";
        public const string HazmatTechId = "the_hazmat_tech";
        // Prompts #235–#248
        public const string TeacherId = "the_teacher";
        public const string PoliticianId = "the_politician";
        public const string PriestId = "the_priest";
        public const string ReporterId = "the_reporter";
        public const string RadioHostId = "the_radio_host";
        public const string ChefId = "the_chef";
        public const string AthleteId = "the_athlete";
        public const string FirefighterId = "the_firefighter";
        public const string TailorId = "the_tailor";
        public const string WatchmakerId = "the_watchmaker";
        public const string HistorianId = "the_historian";
        public const string DefectorId = "the_defector";
        public const string AddictId = "the_addict";
        public const string ParentId = "the_parent";
        // Prompts #249–#256
        public const string FierceMotherId = "the_fierce_mother";
        public const string ExhaustedFatherId = "the_exhausted_father";
        public const string NaiveSonId = "the_naive_son";
        public const string HardenedDaughterId = "the_hardened_daughter";
        public const string PsychopathId = "the_psychopath";
        public const string SerialKillerId = "the_serial_killer";
        public const string LiarId = "the_liar";
        public const string HoarderId = "the_hoarder";
        // Prompts #257–#266
        public const string GeneralId = "the_general";
        public const string SaboteurId = "the_saboteur";
        public const string DeserterId = "the_deserter";
        public const string QuartermasterId = "the_quartermaster";
        public const string ChildSoldierId = "the_child_soldier";
        public const string EmpathId = "the_empath";
        public const string MisanthropeId = "the_misanthrope";
        public const string ThePollyannaId = "the_pollyanna";
        public const string MartyrId = "the_martyr";
        public const string ArrogantSurgeonId = "the_arrogant_surgeon";
        // Prompts #267–#276 (relapsing addict is distinct from #247 the_addict)
        public const string RelapsingAddictId = "the_relapsing_addict";
        public const string InsomniacId = "the_insomniac";
        public const string HypochondriacId = "the_hypochondriac";
        public const string PyromaniacId = "the_pyromaniac";
        public const string BlindPreacherId = "the_blind_preacher";
        public const string PrepperId = "the_prepper";
        public const string OutcastId = "the_outcast";
        public const string FeralOrphanId = "the_feral_orphan";
        public const string PacifistId = "the_pacifist";
        public const string WidowId = "the_widow";
        // Prompts #277–#283 (former politician distinct from #236 the_politician)
        public const string ExConId = "the_ex_con";
        public const string SheriffId = "the_sheriff";
        public const string FormerPoliticianId = "the_former_politician";
        public const string TechBroId = "the_tech_bro";
        public const string NewsAnchorId = "the_news_anchor";
        public const string NomadId = "the_nomad";
        public const string ExecId = "the_exec";
        // Prompts #284–#298
        public const string CoalMinerId = "the_coal_miner";
        public const string TruckDriverId = "the_truck_driver";
        public const string WelderId = "the_welder";
        public const string CustodianId = "the_custodian";
        public const string LumberjackId = "the_lumberjack";
        public const string MicrobiologistId = "the_microbiologist";
        public const string AstronomerId = "the_astronomer";
        public const string LibrarianId = "the_librarian";
        public const string AccountantId = "the_accountant";
        public const string MusicianId = "the_musician";
        public const string SmugglerId = "the_smuggler";
        public const string HitmanId = "the_hitman";
        public const string PickpocketId = "the_pickpocket";
        public const string ForgerId = "the_forger";
        public const string GetawayDriverId = "the_getaway_driver";
        // Prompts #299–#318
        public const string PromQueenId = "the_prom_queen";
        public const string JockId = "the_jock";
        public const string MedStudentId = "the_med_student";
        public const string GamerId = "the_gamer";
        public const string ChoirBoyId = "the_choir_boy";
        public const string TwinAlphaId = "twin_alpha";
        public const string TwinBetaId = "twin_beta";
        public const string TheoristId = "the_theorist";
        public const string HermitId = "the_hermit";
        public const string PatientId = "the_patient";
        public const string EscapeeId = "the_escapee";
        public const string StowawayId = "the_stowaway";
        public const string BillionaireId = "the_billionaire";
        public const string ApprenticeId = "the_apprentice";
        public const string FireChiefId = "the_fire_chief";
        public const string AmputeeId = "the_amputee";
        public const string InmateId = "the_inmate";
        public const string SynthId = "the_synth";
        public const string DogId = "the_dog";
        public const string CoreId = "the_core";

        // ── Quest thresholds ─────────────────────────────────────────────
        public const int DaysAliveToStartQuest = 30;
        public const float MoraleFloorTrigger = 0f;
        public const float MoraleRecoveryTrigger = 100f;

        public const int SurgeonStressOpsRequired = 3;
        public const float SurgeonStressMoraleMax = 30f;

        public const int TherapistDeEscalationsRequired = 3;

        public const int VetMedicalKitsRequired = 3;
        public const float VetAirlockHoursRequired = 48f;

        public const float MassGraveHoursRequired = 24f;
        public const float MassGraveFatigueHit = 40f;
        public const float MassGraveRadHit = 25f;

        public const float AlchemistDoubleYieldChance = 0.30f;
        public const float MiracleWorkerSurgeryDurationMult = 0.50f;
        public const int ZoonoticMaxTamedAnimals = 3;
        public const float AnchorRoomMoraleFloor = 20f;
        public const float DeathBlindDebrisSleepMoralePerHour = 1.5f;

        // #220 Warlord
        public const float AssaultRifleWeaponPower = 28f;
        public const float Level3RaidStrength = 75f;
        public const float WarlordUnarmedDefensePower = 80f;

        // #222 Juggernaut
        public const float JuggernautHealthMultiplier = 2f;

        // #223 Apex Predator
        public const int WhiteElkNodesRequired = 3;
        public const int ApexPredatorMeatYield = 50;
        public const float ApexPredatorStealth = 1f; // 100% stealth

        // #224 Survivalist
        public const float SurvivalistAloneStaminaMult = 0.25f; // 75% reduced drain

        // #225 Hydraulic Master
        public const float HydraulicPurifierSpeedMult = 3f;
        public const float HumidityWaterExtractPerHour = 2f;
        public const float PipeBurstIrradiatedRadSpike = 40f;

        // #226 Grid Walker
        public const float GridWalkerPowerCapacityMult = 1.5f;

        // #227 Vault Builder
        public const float VaultBuilderBuildCostMult = 0.5f;

        // #228 Grease Monkey
        public const float GreaseMonkeyVehicleCostMult = 0.5f;
        public const float EngineBlockWeightKg = 80f;

        // #229 Synthesizer
        public const float SynthesizerRadAwayMult = 2f;

        // #230 Gaia
        public const int GaiaCropYieldMult = 3;
        public const int SeedVaultPerfectDaysRequired = 14;

        // #231 Wasteland Runner
        public const float WastelandRunnerTravelMult = 0.5f;
        public const int LostRouteDeadDropsRequired = 5;

        // #233 Stormcaller
        public const int StormcallerForecastDays = 10;
        public const float StormcallerStormMoraleBuff = 15f;

        // #234 Rad-Walker
        public const float RadWalkerAbsorbCap = 0.5f; // 50% max absorption
        public const float GroundZeroRadPerHour = 10000f;

        // #235 Polymath
        public const int TeacherMourningDaysRequired = 7;
        public const float PolymathPerkXpMult = 3f;

        // #236 Demagogue
        public const int PropagandaResolutionsRequired = 3;
        public const float DemagogueTrustFloor = 0f;

        // #237 Shepherd
        public const float SermonDurationHours = 2f;
        public const float SermonMoraleBoost = 20f;
        public const string CrisisOfFaithBreakId = "crisis_of_faith";

        // #238 Muckraker
        public const int FirstStrikeIntelRequired = 5;

        // #239 Voice of the Wastes
        public const float DeadAirBroadcastHoursRequired = 48f;

        // #240 Iron Chef
        public const float LastSupperCookHours = 24f;
        public const string LastSupperItemId = "the_last_supper";

        // #241 Tireless
        public const int MarathonMinNodesAway = 15;
        public const float MarathonMaxHours = 48f;
        public const float TirelessPoolMult = 3f;
        public const float TirelessSleepHoursPerDay = 1f;

        // #242 Asbestos
        public const string GeneratorRoomId = "plant";
        public const float InfernoBurnDamage = 40f;

        // #243 Armorer
        public const int ClothingScrapsRequired = 10;
        public const float ArmorerClothingDegradeMult = 0.25f; // 75% slower
        public const string ReinforcedHazmatSuitId = "reinforced_hazmat_suit";

        // #244 Tinkerer
        public const int WatchRepairScrapRequired = 50;

        // #245 Lorekeeper
        public const float LorekeeperJournalMoraleBoost = 15f;
        public const float LorekeeperArtifactTradeMult = 2f;

        // #246 Zealot's Bane
        public const float ZealotsBaneCombatMult = 1.5f;

        // #247 Chem-Resistant
        public const int WithdrawalCleanDaysRequired = 14;
        public const float ChemResistantHealMult = 2f;

        // #248 Protector
        public const float ProtectorHealthTriggerFrac = 0.10f;
        public const float ProtectorBoostMult = 3f; // +200% = 3x
        public const string DespairBreakId = "despair";
        public const float ParentMourningDaysRequired = 7f;

        // #249 Selfless / Matriarch
        public const float SelflessMoraleAbsorbFrac = 0.10f;
        public const float ChildNeedsCancelThreshold = 20f;
        public const float MatriarchRoomHealthBonus = 20f;
        public const float SevereRadiationThreshold = 70f;
        public const string DaycareNodeId = "the_daycare";
        public const string PrewarToyItemId = "prewar_daycare_toy";

        // #250 Workaholic / Pillar of Atlas
        public const float WorkaholicCraftFatigueDrainMult = 0.5f;
        public const float WorkaholicSleepRestoreMult = 0.5f;
        public const float WorkaholicRestIgnoreFatigue = 95f;
        public const int BrokenPromiseTier3Required = 5;
        public const int BrokenPromiseDayDeadline = 50;
        public const float PillarDeathRepairSpeedMult = 0.8f; // permanent -20%

        // #251 Dependent / Wasteland Scout
        public const float DependentCarryKg = 10f;
        public const float NaiveSonHopeBuff = 25f;

        // #252 Traumatized / Child of the Ash
        public const float TraumatizedMoraleCap = 50f;

        // #253 Sociopath / Cold Calculus
        public const float PsychopathAffinityDrainPerHour = 8f;
        public const float ColdCalculusPopThreshold = 3f;
        public const float ColdCalculusExecSpeedMult = 1.5f;

        // #254 Serial Killer / The Urge
        public const float UrgeMax = 100f;
        public const float UrgeMurderThreshold = 100f;

        // #255 Deceptive / Master Manipulator
        public const float DeceptiveMaskChance = 0.35f;

        // #256 Selfish / Dragon's Hoard
        public const float SelfishRationMult = 2f;
        public const float SelfishMissRationMoraleHit = 15f;
        public const float WeightOfGoldSafeKg = 50f;
        public const int WeightOfGoldNodesRequired = 3;
        public const string EmptySafeItemId = "empty_safe";

        // #257 Disgraced General / Art of War
        public const float HatedMilitaryTrust = -100f;
        public const float FloorSleepFatiguePenaltyPerHour = 12f;
        public const float ArtOfWarShelterSecurityMult = 1.25f;
        public const string HitSquadNodeId = "the_hit_squad";

        // #258 Rebel Saboteur / Demolitions Expert
        public const float AntiAuthorityOrderMoraleHit = 12f;
        public const float DemolitionsExplosiveDamageMult = 3f;
        public const string MilitaryCheckpointNodeId = "the_military_checkpoint";
        public const string IedItemId = "improvised_explosive";

        // #259 Deserter Sniper / Ghost Shooter
        public const float CowardFleeHealthFrac = 0.5f;
        public const float GhostShooterCombatBonus = 1.5f;

        // #260 Quartermaster / Supply Chain Master
        public const float StrictInventoryFullMorale = 2f;
        public const float StrictInventoryLowMoraleHit = 4f;
        public const float StrictResourceLowFrac = 0.2f;
        public const float SupplyChainCraftCostMult = 0.8f;
        public const float SupplyChainFuelBurnMult = 0.7f;
        public const int InventoryAuditScrapEach = 100;

        // #261 Child Soldier / Reclaimed Youth
        public const int DroppingRifleDaysRequired = 30;
        public const float ChildSoldierAnxietyDebuff = 8f;
        public const float ReclaimedYouthHopeAura = 10f;
        public const string NightTerrorsBreakId = "night_terrors";

        // #262 Pure Empath / Soul Weaver
        public const int SpongeCuresRequired = 3;
        public const float SpongeAbsorbHealthFloor = 1f;
        public const float ComfortTalkUtilityBias = 2.5f;

        // #263 Bitter Misanthrope / Lone Wolf
        public const float RudeAffinityDrainPerHour = 6f;
        public const float SoloRoomActionSpeedMult = 1.25f;
        public const int HellIsOtherPeopleDaysRequired = 15;
        public const float LoneWolfNeedsDecayMult = 0.5f;
        public const float LoneWolfCombatMult = 1.75f;

        // #264 Pollyanna Denialist / Grounded Optimist
        public const float GroundedOptimistBaseMorale = 5f;
        public const float GroundedOptimistHardshipScale = 0.25f;

        // #265 Selfless Martyr / Living Saint
        public const float LivingSaintMoraleFloor = 50f;
        public const float MartyrSecretFoodHungerSpike = 25f;

        // #266 Arrogant Surgeon / Humbled Healer
        public const float GodComplexPatientMoraleHit = 10f;
        public const int BotchedJobDepressionDays = 10;
        public const string DepressionBreakId = "depression";
        public const float ArrogantSurgeonMedicalSkill = 100f;

        // #267 Relapsing Addict / Clean & Sober
        public const int ColdTurkeyDaysRequired = 21;
        public const float ForcedChemMoraleThreshold = 40f;
        public const float CleanAndSoberStaminaMult = 2f;
        public const string AmphetaminesItemId = "amphetamines";

        // #268 Insomniac / The Watcher
        public const float RestlessSleepRestoreFrac = 0.20f;
        public const float RestlessMaxFatigueCap = 80f;
        public const int LongNightGuardNightsRequired = 5;
        public const float InsomniacNightNoisePerHour = 8f;

        // #269 Hypochondriac / Hyper-Aware
        public const float PlaceboMoraleRestore = 8f;
        public const float FakeIllnessMoraleHit = 6f;
        public const float FakeIllnessFatigueHit = 4f;

        // #270 Pyromaniac / Fire-Breather
        public const float FascinationHeaterMoralePerHour = 4f;
        public const float PyromaniacFireMoraleThreshold = 30f;
        public const float PyromaniacDailyFireChance = 0.05f;
        public const int TrialByFireExtinguishRequired = 5;

        // #271 Blind Preacher / Sonar
        public const int VoiceInDarkConvertsRequired = 3;
        public const float SonarRaidWarningHours = 12f;

        // #272 Prepper / Improvised Engineering
        public const float PrepperBaseRadiationAnxiety = 0.75f;

        // #273 Mutated Outcast / Radiotrophic
        public const float OutcastStartLifetimeRads = 800f;
        public const float EmbracingGlowLifetimeRads = 1000f;
        public const float OutcastRoomMealMoraleHit = 3f;
        public const float RadiotrophicHealPerHour = 4f;

        // #274 Feral Orphan / Apex Scavenger
        public const int PackTrainingDaysRequired = 30;
        public const float AnimalisticAffinityDrain = 5f;

        // #275 Pacifist / Zen State
        public const float ZenNeedsDecayMult = 0.20f; // 80% reduced

        // #276 Widow / Master Geneticist
        public const float GrievingActionEfficiencyMult = 0.55f;
        public const string PreWarRoseItemId = "pre_war_rose";

        // #277 Ex-Con / Enforcer
        // (no extra numeric thresholds)

        // #278 Sheriff / Legend
        public const float MoralCompassBunkerMorale = 3f;
        public const float MoralCompassEvilHit = 25f;
        public const float FailingHeartStaminaDecayPerDay = 0.5f;
        public const float LegendRaidFrequencyMult = 0.25f; // 75% drop

        // #279 Former Politician / Statesman
        public const int RealLeaderDirtyDaysRequired = 14;
        public const float SilverTongueLaborMoraleHit = 20f;

        // #280 Tech Bro / Cybernetics
        public const float TechBroPowerWasteWatts = 15f;

        // #281 News Anchor / Beacon
        public const float PhotogenicHygieneMoraleHit = 5f;
        public const float BeaconTradePriceMult = 0.70f; // 30% cheaper

        // #282 Nomad / Master Pathologist
        public const int NomadInsideDaysBeforeFlee = 5;
        public const float AgoraphileBunkerMoraleHitPerDay = 8f;

        // #283 Exec / Monopolist
        public const float RuthlessModuleEfficiencyMult = 1.20f;
        public const float RuthlessModuleWearMult = 1.35f;
        public const float GoldenParachuteTradeValue = 10000f;

        // #284 Coal Miner / Deep Delver
        public const float BlackLungStaminaMaxMult = 0.80f;
        public const float ClaustrophilicMoralePerHour = 4f;
        public const float DeepDelverExcavationDurationMult = 0.25f;

        // #285 Truck Driver / Logistics Master
        public const float CaffeinatedSleepRestoreMult = 0.50f;
        public const float CaffeinatedFatigueCrash = 35f;
        public const float TruckDriverCabinFeverMult = 2f;
        public const float LongHaulEncumbranceRatio = 2f;
        public const float LogisticsMasterCarryCapacityMult = 3f;

        // #286 Welder / Forge Master
        public const float DeafStealthFailChance = 0.50f;
        public const float WelderRepairScrapMult = 2f;
        public const float WelderRepairMaxDurabilityMult = 1.50f;

        // #287 Custodian / Sanitization Expert
        public const float NeatFreakHygieneThreshold = 0.80f;
        public const float NeatFreakMoraleHit = 6f;
        public const int MessCorpsesRequired = 5;

        // #288 Lumberjack / Deforester
        public const float BrawnMeleeDamageMult = 2f;
        public const float BrawnFirearmsAccuracyMult = 0.50f;
        public const float LumberjackSalvageMoraleThreshold = 30f;
        public const float DeforesterWoodYieldMult = 5f;
        public const string MutatedBearEnemyId = "mutated_bear";
        public const string AxeWeaponId = "axe";

        // #289 Microbiologist / Epidemiologist
        public const float MicrobiologistRefuseRationChance = 0.20f;

        // #290 Astronomer / Celestial Navigator
        public const float NightOwlDaySpeedMult = 0.50f;
        public const float NightOwlNightSpeedMult = 1.50f;
        public const int DeadStarsStormsRequired = 3;
        public const float CelestialNavigatorNightTravelMult = 0.50f;

        // #291 Librarian / Archivist
        public const float FrailMaxHealthCap = 60f;
        public const int ArchiveIntelRequired = 50;

        // #292 Accountant / Auditor
        public const float PennyPincherHungerEatThreshold = 0.95f; // hunger need 0..1 (eat at 5% remaining)
        public const int AccountantDeficitDaysToLock = 3;

        // #293 Musician / Maestro
        public const float FragileEgoFailureMoraleMult = 2f;
        public const float PlayInstrumentMoraleAura = 8f;
        public const float MaestroPlayMoraleAura = 15f;
        public const int MaestroSuppressDays = 1;
        public const string RadioTowerNodeId = "the_radio_tower";

        // #294 Smuggler / Blockade Runner
        public const float ShadySuspicionMult = 1.75f;
        public const float SmugglerContrabandChance = 0.25f;

        // #295 Hitman / Executioner
        public const float HitmanAccidentalDischargeChance = 0.10f;
        public const float ExecutionerHumanCritMult = 3f;

        // #296 Pickpocket / Shadow
        public const float PickpocketCatchChance = 0.30f;

        // #297 Forger / Master of Disguise
        public const float ForgerFakeJournalChance = 0.40f;
        public const int PerfectFakeCheckpointLevel = 5;

        // #298 Getaway Driver / Mechanic Prodigy
        public const int GetawayAntsyDays = 3;
        public const float GetawayAntsyMoraleHitPerDay = 10f;
        public const float MechanicProdigyFuelMult = 0.50f;

        // #299–#318 ashes / improbable / final flawed
        public const float PromQueenMakeupHoursPerDay = 2f;
        public const float PromQueenMakeupWaterUnits = 2f;
        public const float HighMetabolismRationMult = 2f;
        public const float JockPunchMoraleThreshold = 30f;
        public const float JockWallPunchIntegrityDamage = 4f;
        public const float WastelandGladiatorUnarmedMult = 2.5f;
        public const float TextbookHealingDurationMult = 2f;
        public const float ChiefOfMedicineSupplyMult = 0.50f;
        public const float MedStudentPukeHygieneHit = 25f;
        public const float MedStudentPukeFatigueHit = 20f;
        public const int GamerDroneNodesRequired = 10;
        public const float InnocentMurderShockMoraleHit = 40f;
        public const float ChoirBoyCorpseMoralePenaltyMult = 0.35f;
        public const int ChoirBoyStarvationDaysRequired = 5;
        public const float HiveTacticsCombatMult = 3f;
        public const float TheoristRadioSabotageChance = 0.15f;
        public const float TruthSeekerIntelMult = 3f;
        public const int HermitIndoorSleepMaxPop = 2;
        public const string AirlockRoomId = "airlock";
        public const string MutatedForestNodeId = "the_mutated_forest";
        public const int PatientAwakeningDaysRequired = 15;
        public const float PatientAwakeningHydrationThreshold = 80f;
        public const float SecondLifeStatMaxMult = 1.50f;
        public const float BrandedCultDamageMult = 2f;
        public const float UndocumentedRationMult = 1.20f;
        public const float EntitledLaborMoraleHitPerDay = 10f;
        public const float BillionaireStartingMoney = 5000f;
        public const float BillionaireBribeMoraleHit = 3f;
        public const float RuthlessCapitalistSellMult = 0.10f;
        public const float ClumsyToolBreakChance = 0.10f;
        public const float ApprenticeQuestionSpeedMult = 0.85f;
        public const float BurnScarsCharismaMult = 0.50f;
        public const int FireChiefSavesRequired = 3;
        public const float MissingArmSpeedMult = 0.50f;
        public const float AmputeeTinkerNoise = 2f;
        public const float CyberArmCraftMeleeMult = 3f;
        public const int ProstheticMechanicalPartsRequired = 50;
        public const int ProstheticElectronicScrapRequired = 10;
        public const string MissingArmDisabilityId = "missing_arm";
        public const float LethalInstantKillChance = 0.05f;
        public const float GoodBoyRoomMoraleAura = 6f;
        public const int TuringTestDeadlineDay = 100;

        public const string RuinedCvsNodeId = "the_ruined_cvs";
        public const string MassGraveNodeId = "the_mass_grave";
        public const string FortifiedSquadNodeId = "fortified_squad_holdout";
        public const string RuinedPrecinctNodeId = "the_ruined_precinct";
        public const string WhiteElkNodeId = "the_white_elk";
        public const string PenitentiaryNodeId = "the_penitentiary";
        public const string EncounterLooters = "encounter_looters";
        public const string PharmacyLogbookItemId = "pharmacy_logbook";
        public const string SpoiledMeatItemId = "spoiled_meat";
        public const string MoldItemId = "mold";
        public const string DirtyWaterItemId = "dirty_water";
        public const string AntibioticsItemId = "antibiotics";
        public const string MedicalKitItemId = "medical_kit";
        public const string PipeWeaponId = "pipe_weapon";
        public const string PipeShotgunId = "pipe_shotgun";
        public const string ScrapBowId = "scrap_bow";
        public const string MeatItemId = "meat";
        public const string EvidenceLockboxItemId = "evidence_lockbox";
        public const string FamilyPhotosItemId = "family_photos";
        public const string WardenKeysItemId = "warden_keys";
        public const string InternalSaboteurEventId = "shelter_saboteur";
        public const string RationThiefEventId = "missing_rations";
        public const string RationThiefAgainEventId = "missing_rations_again";
        public const string SquadDistressRadioEventId = "evt_squad_distress_signal";
        public const string SubstationNodeId = "the_substation";
        public const string TheFirmNodeId = "the_firm";
        public const string HighwayPileupNodeId = "the_highway_pileup";
        public const string RuinedBankNodeId = "the_ruined_bank";
        public const string WeatherTowerNodeId = "the_weather_tower";
        public const string GroundZeroCraterNodeId = "the_ground_zero_crater";
        public const string EngineBlockItemId = "engine_block";
        public const string ChemicalScrapItemId = "chemical_scrap";
        public const string AntiRadItemId = "anti_rad";
        public const string BlackBoxItemId = "military_black_box";
        public const string CityBlueprintsItemId = "city_blueprints";
        public const string MedicinalHerbItemId = "medicinal_herb";
        public const string ScarredLungsId = "scarred_lungs";
        public const string PipeBurstEventId = "evt_pipe_burst_city_mains";
        public const string ChlorineLeakEventId = "evt_chlorine_tank_leak";
        public const string AbandonedSchoolNodeId = "the_abandoned_school";
        public const string RationManifestItemId = "ration_manifest";
        public const string RuinedMuseumNodeId = "the_ruined_museum";
        public const string ConstitutionItemId = "the_constitution";
        public const string HeirloomWatchItemId = "heirloom_watch";
        public const string CultLeaderId = "cult_leader";
        public const string FirstStrikeIntelPrefix = "first_strike_intel_";
        public const string ChildDeathIntelId = "intel_child_death";

        private SkillProgressionSystem _progression;
        private readonly Dictionary<string, QuestlineSO> _questlines =
            new Dictionary<string, QuestlineSO>();
        private readonly Dictionary<string, PersonalQuestState> _bySurvivor =
            new Dictionary<string, PersonalQuestState>();

        /// <summary>
        /// #250 Pillar of Atlas death fallout — permanent shelter repair speed debuff
        /// once a living Pillar dies. Hosts read via GetShelterRepairSpeedMultiplier.
        /// </summary>
        public bool PillarOfAtlasDeathDebuffActive { get; private set; }

        /// <summary>
        /// #265 Living Saint death — bunker-wide Inspired buff; hosts read via
        /// GetLivingSaintMoraleFloor (minimum Morale capped at 50 forever).
        /// </summary>
        public bool LivingSaintInspiredActive { get; private set; }

        public event Action<Survivor, string> OnQuestlineStarted;       // sv, questlineId
        public event Action<Survivor, string, int> OnQuestProgress;     // sv, key, value
        public event Action<Survivor, string> OnQuestlineCompleted;     // sv, questlineId
        public event Action<Survivor, string> OnLatentTraitUnlocked;    // sv, traitId
        /// <summary>Host: spawn expedition map node when quest begins (nodeId, ownerSvId).</summary>
        public event Action<string, string> OnMapNodeSpawnRequested;
        /// <summary>Host: queue bunker narrative event (eventId, ownerSvId).</summary>
        public event Action<string, string> OnBunkerEventRequested;
        /// <summary>UI: monumental character evolution (sv, traitId, displayName).</summary>
        public event Action<Survivor, string, string> OnCharacterEvolution;
        /// <summary>#254 Serial Killer attempted murder (killer, targetId, targetKind).</summary>
        public event Action<Survivor, string, string> OnSecretMurderAttempted;
        /// <summary>#255 Liar planted a false intel node id for the radio.</summary>
        public event Action<Survivor, string> OnFalseIntelReported;

        public void Bind(SkillProgressionSystem progression)
        {
            _progression = progression;
            _progression?.RegisterLatentExpertTraits();
            EnsureDefaultQuestlines();
        }

        /// <summary>
        /// Optional dose applicator (host wires to RadiationSystem.Expose / AdjustDose).
        /// Avoids Survivors→Radiation assembly cycle while keeping quest rad spikes
        /// on the same event path as ambient exposure (MISC-007). Positive deltas
        /// should use Expose so lifetime accumulates; negative cleanse uses AdjustDose.
        /// </summary>
        private Action<Survivor, float> _applyRadiationDelta;

        /// <summary>
        /// Optional lifetime seed (host wires to RadiationSystem.SeedLifetimeExposure).
        /// Used by archetypes such as the Mutated Outcast that begin already cooked.
        /// </summary>
        private Action<Survivor, float> _seedLifetimeRadiation;

        public void BindRadiationDose(Action<Survivor, float> applyDelta) =>
            _applyRadiationDelta = applyDelta;

        public void BindLifetimeRadiation(Action<Survivor, float> seedLifetime) =>
            _seedLifetimeRadiation = seedLifetime;

        private void ApplyQuestRadiation(Survivor sv, float delta)
        {
            if (sv == null || !sv.IsAlive || delta == 0f) return;
            if (_applyRadiationDelta != null)
            {
                _applyRadiationDelta(sv, delta);
                return;
            }
            // Unbound unit-test fallback only — production host always injects.
            sv.RadiationDose = Mathf.Clamp(sv.RadiationDose + delta, 0f, 100f);
        }

        private void ApplyLifetimeRadiationSeed(Survivor sv, float lifetime)
        {
            if (sv == null || lifetime <= 0f) return;
            if (_seedLifetimeRadiation != null)
            {
                _seedLifetimeRadiation(sv, lifetime);
                return;
            }
            // Unbound unit-test fallback only.
            sv.LifetimeRadiationExposure = Mathf.Max(sv.LifetimeRadiationExposure, lifetime);
        }

        public void RegisterQuestline(QuestlineSO quest)
        {
            if (quest == null || string.IsNullOrEmpty(quest.id)) return;
            _questlines[quest.id] = quest;
        }

        public QuestlineSO GetQuestline(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _questlines.TryGetValue(id, out var q) ? q : null;
        }

        // ── Profile assignment ───────────────────────────────────────────

        /// <summary>
        /// Apply predetermined latent trait + questline from an archetype profile.
        /// Does NOT grant the latent trait — only stores the destiny.
        /// Grants base personality traits (Selfless, Workaholic, …) immediately.
        /// </summary>
        public void AssignProfile(Survivor sv, SurvivorProfile profile)
        {
            if (sv == null || profile == null) return;
            sv.ArchetypeId = profile.ArchetypeId;
            sv.LatentExpertTraitId = profile.LatentExpertTraitId;
            sv.ActiveQuestlineId = profile.ActiveQuestlineId;
            sv.QuestlineActive = false;
            sv.LatentTraitUnlocked = false;
            sv.QuestStage = 0;
            sv.QuestProgress = 0f;
            sv.DaysAlive = 0;
            sv.MoraleHitZero = false;

            var state = GetOrCreate(sv.Id);
            state.ArchetypeId = profile.ArchetypeId;
            state.LatentTraitId = profile.LatentExpertTraitId;
            state.QuestlineId = profile.ActiveQuestlineId;
            state.QuestActive = false;
            state.TraitUnlocked = false;
            state.Stage = 0;
            state.Progress = 0f;
            state.DaysAlive = 0;
            state.MoraleHitZero = false;

            ApplyBaseTraits(sv, profile.ArchetypeId);
            ApplyArchetypeFlags(sv, profile.ArchetypeId);
            // MISC-007 — Outcast lifetime seed via injected RadiationSystem when bound.
            // ApplyArchetypeFlags is static (no host bind); re-apply through the instance
            // seed path so production hosts never leave a raw lifetime write as the
            // only path when BindLifetimeRadiation is wired.
            if (string.Equals(profile.ArchetypeId, OutcastId, System.StringComparison.Ordinal))
                ApplyLifetimeRadiationSeed(sv, OutcastStartLifetimeRads);
        }

        /// <summary>Grant day-0 personality traits for bond/burden archetypes.</summary>
        public static void ApplyBaseTraits(Survivor sv, string archetypeId)
        {
            if (sv == null || string.IsNullOrEmpty(archetypeId)) return;
            switch (archetypeId)
            {
                case FierceMotherId:
                    GrantBaseTrait(sv, SelflessId);
                    break;
                case ExhaustedFatherId:
                    GrantBaseTrait(sv, WorkaholicId);
                    break;
                case NaiveSonId:
                    GrantBaseTrait(sv, DependentId);
                    GrantBaseTrait(sv, PollyannaId);
                    break;
                case HardenedDaughterId:
                    GrantBaseTrait(sv, TraumatizedId);
                    break;
                case PsychopathId:
                    GrantBaseTrait(sv, SociopathId);
                    GrantBaseTrait(sv, ArrogantId);
                    break;
                case SerialKillerId:
                    // Appears Kind + Charismatic; true nature is hidden.
                    GrantBaseTrait(sv, KindId);
                    GrantBaseTrait(sv, CharismaticId);
                    break;
                case LiarId:
                    GrantBaseTrait(sv, DeceptiveId);
                    break;
                case HoarderId:
                    GrantBaseTrait(sv, SelfishId);
                    break;
                case GeneralId:
                    GrantBaseTrait(sv, TacticianId);
                    GrantBaseTrait(sv, HatedId);
                    break;
                case SaboteurId:
                    GrantBaseTrait(sv, AntiAuthorityId);
                    break;
                case DeserterId:
                    GrantBaseTrait(sv, CowardId);
                    break;
                case QuartermasterId:
                    GrantBaseTrait(sv, StrictId);
                    break;
                case ChildSoldierId:
                    GrantBaseTrait(sv, StuntedId);
                    break;
                case EmpathId:
                    GrantBaseTrait(sv, HyperEmpatheticId);
                    break;
                case MisanthropeId:
                    GrantBaseTrait(sv, RudeId);
                    break;
                case ThePollyannaId:
                    GrantBaseTrait(sv, DenialistId);
                    break;
                case MartyrId:
                    GrantBaseTrait(sv, SacrificialId);
                    break;
                case ArrogantSurgeonId:
                    GrantBaseTrait(sv, GodComplexId);
                    break;
                // #267–#276
                case RelapsingAddictId:
                    // Starts addicted to amphetamines — host wires AddictionSystem.
                    break;
                case InsomniacId:
                    GrantBaseTrait(sv, RestlessId);
                    break;
                case HypochondriacId:
                    GrantBaseTrait(sv, ParanoidHealthId);
                    break;
                case PyromaniacId:
                    GrantBaseTrait(sv, FascinationId);
                    break;
                case BlindPreacherId:
                    GrantBaseTrait(sv, BlindId);
                    break;
                case PrepperId:
                    GrantBaseTrait(sv, ParanoidId);
                    break;
                case OutcastId:
                    // Lifetime rads / mutagenesis applied in ApplyArchetypeFlags.
                    break;
                case FeralOrphanId:
                    GrantBaseTrait(sv, AnimalisticId);
                    break;
                case PacifistId:
                    GrantBaseTrait(sv, VowOfNonviolenceId);
                    break;
                case WidowId:
                    GrantBaseTrait(sv, GrievingId);
                    break;
                // #277–#283
                case ExConId:
                    GrantBaseTrait(sv, DistrustedId);
                    break;
                case SheriffId:
                    GrantBaseTrait(sv, MoralCompassId);
                    GrantBaseTrait(sv, FailingHeartId);
                    break;
                case FormerPoliticianId:
                    GrantBaseTrait(sv, SilverTongueId);
                    break;
                case TechBroId:
                    GrantBaseTrait(sv, DelusionalId);
                    GrantBaseTrait(sv, ArrogantId);
                    break;
                case NewsAnchorId:
                    GrantBaseTrait(sv, PhotogenicId);
                    break;
                case NomadId:
                    GrantBaseTrait(sv, AgoraphileId);
                    break;
                case ExecId:
                    GrantBaseTrait(sv, RuthlessId);
                    break;
                // #284–#298
                case CoalMinerId:
                    GrantBaseTrait(sv, BlackLungId);
                    GrantBaseTrait(sv, ClaustrophilicId);
                    break;
                case TruckDriverId:
                    GrantBaseTrait(sv, CaffeinatedId);
                    break;
                case WelderId:
                    GrantBaseTrait(sv, CallousedId);
                    GrantBaseTrait(sv, DeafInOneEarId);
                    break;
                case CustodianId:
                    GrantBaseTrait(sv, InvisibleId);
                    GrantBaseTrait(sv, NeatFreakId);
                    break;
                case LumberjackId:
                    GrantBaseTrait(sv, BrawnId);
                    break;
                case MicrobiologistId:
                    GrantBaseTrait(sv, GermaphobeId);
                    break;
                case AstronomerId:
                    GrantBaseTrait(sv, NightOwlId);
                    break;
                case LibrarianId:
                    GrantBaseTrait(sv, QuietId);
                    GrantBaseTrait(sv, FrailId);
                    break;
                case AccountantId:
                    GrantBaseTrait(sv, PennyPincherId);
                    break;
                case MusicianId:
                    GrantBaseTrait(sv, FragileEgoId);
                    break;
                case SmugglerId:
                    GrantBaseTrait(sv, ShadyId);
                    break;
                case HitmanId:
                    GrantBaseTrait(sv, ProfessionalId);
                    break;
                case PickpocketId:
                    GrantBaseTrait(sv, SlightOfHandId);
                    break;
                case ForgerId:
                    GrantBaseTrait(sv, CounterfeiterId);
                    break;
                case GetawayDriverId:
                    GrantBaseTrait(sv, AntsyId);
                    break;
                // #299–#318
                case PromQueenId:
                    GrantBaseTrait(sv, SpoiledId);
                    break;
                case JockId:
                    GrantBaseTrait(sv, HighMetabolismId);
                    break;
                case MedStudentId:
                    GrantBaseTrait(sv, TextbookKnowledgeId);
                    break;
                case GamerId:
                    GrantBaseTrait(sv, AgoraphobicId);
                    break;
                case ChoirBoyId:
                    GrantBaseTrait(sv, InnocentId);
                    break;
                case TwinAlphaId:
                case TwinBetaId:
                    GrantBaseTrait(sv, SymbioticBondId);
                    break;
                case TheoristId:
                    GrantBaseTrait(sv, TinfoilHatId);
                    break;
                case HermitId:
                    GrantBaseTrait(sv, UncivilizedId);
                    break;
                case PatientId:
                    GrantBaseTrait(sv, ComatoseBurdenId);
                    break;
                case EscapeeId:
                    GrantBaseTrait(sv, BrandedId);
                    break;
                case StowawayId:
                    GrantBaseTrait(sv, BlindId);
                    GrantBaseTrait(sv, UndocumentedId);
                    break;
                case BillionaireId:
                    GrantBaseTrait(sv, EntitledId);
                    break;
                case ApprenticeId:
                    GrantBaseTrait(sv, ClumsyId);
                    break;
                case FireChiefId:
                    GrantBaseTrait(sv, BurnScarsId);
                    break;
                case AmputeeId:
                    GrantBaseTrait(sv, MissingArmId);
                    break;
                case InmateId:
                    GrantBaseTrait(sv, LethalId);
                    GrantBaseTrait(sv, UntrustedId);
                    break;
                case SynthId:
                    GrantBaseTrait(sv, AndroidId);
                    break;
                case DogId:
                    GrantBaseTrait(sv, GoodBoyId);
                    break;
                case CoreId:
                    GrantBaseTrait(sv, BunkerCoreId);
                    break;
            }
        }

        private static void GrantBaseTrait(Survivor sv, string traitId)
        {
            if (sv.Traits == null) sv.Traits = new List<string>();
            if (!sv.HasTrait(traitId))
                sv.Traits.Add(traitId);
        }

        /// <summary>Child / combat flags for kid archetypes.</summary>
        public static void ApplyArchetypeFlags(Survivor sv, string archetypeId)
        {
            if (sv == null) return;
            if (archetypeId == NaiveSonId)
            {
                sv.IsChild = true;
                sv.CannotFight = true; // Dependent — no firearms
            }
            else if (archetypeId == HardenedDaughterId)
            {
                sv.IsChild = true;
                // Can fight; Traumatized caps morale via GetMaxMoraleCap.
            }
            else if (archetypeId == ChildSoldierId)
            {
                sv.IsChild = true;
                // Adult weapon damage; cannot learn Science/Medical until Reclaimed Youth.
            }
            else if (archetypeId == PatientId)
            {
                sv.State = SurvivorState.Incapacitated;
                sv.CannotFight = true;
                sv.CannotCraft = true;
                sv.CannotScavenge = true;
            }
            else if (archetypeId == AmputeeId)
            {
                if (sv.DisabilityIds == null) sv.DisabilityIds = new List<string>();
                if (!sv.HasDisability(MissingArmDisabilityId))
                    sv.DisabilityIds.Add(MissingArmDisabilityId);
            }
            else if (archetypeId == DogId)
            {
                sv.CannotCraft = true;
                sv.CannotFight = false;
            }
            else if (archetypeId == CoreId)
            {
                sv.CannotCraft = true;
                sv.CannotFight = true;
                sv.CannotScavenge = true;
            }
            else if (archetypeId == ArrogantSurgeonId)
            {
                // Medical skill starts maxed — hosts read GetStartingMedicalSkill.
                sv.ExpertDisciplineId = "medical";
            }
            else if (archetypeId == RelapsingAddictId)
            {
                // AddictionSystem.AddictedTraitId — string to avoid Medical assembly ref.
                if (sv.Traits == null) sv.Traits = new List<string>();
                if (!sv.HasTrait("addicted"))
                    sv.Traits.Add("addicted");
                sv.HoursSinceLastDose = 0f;
            }
            else if (archetypeId == BlindPreacherId)
            {
                sv.CannotFight = true; // blind — no guns / no expeditions (host gates)
            }
            else if (archetypeId == PrepperId)
            {
                sv.RadiationAnxiety = PrepperBaseRadiationAnxiety;
                if (sv.HiddenItemIds == null) sv.HiddenItemIds = new List<string>();
                string[] stash = { "mre_prewar", "mre_prewar", "antibiotics", "battery", "ammo_box" };
                for (int i = 0; i < stash.Length; i++)
                {
                    if (!sv.HiddenItemIds.Contains(stash[i]))
                        sv.HiddenItemIds.Add(stash[i]);
                }
            }
            else if (archetypeId == OutcastId)
            {
                // Static path (unit tests / MakeArchetypeSurvivor): seed lifetime
                // directly. AssignProfile re-applies via ApplyLifetimeRadiationSeed
                // so a host BindLifetimeRadiation overrides through RadiationSystem.
                sv.LifetimeRadiationExposure = Mathf.Max(
                    sv.LifetimeRadiationExposure, OutcastStartLifetimeRads);
                sv.HasChronicIllness = true;
            }
            else if (archetypeId == FeralOrphanId)
            {
                sv.IsChild = true;
                sv.CannotFight = false;
            }
            else if (archetypeId == PacifistId)
            {
                sv.CannotFight = true;
            }
            else if (archetypeId == TechBroId)
            {
                sv.ExpertDisciplineId = "survival"; // engineering / power
            }
            else if (archetypeId == FormerPoliticianId)
            {
                sv.ExpertDisciplineId = "social";
            }
        }

        /// <summary>Built-in archetype profiles for Prompts #215–#234.</summary>
        public static SurvivorProfile ProfileForArchetype(string archetypeId)
        {
            switch (archetypeId)
            {
                case SurgeonId:
                    return new SurvivorProfile(SurgeonId, MiracleWorkerId, QuestlineSO.Ids.ShakingHand);
                case PharmacistId:
                    return new SurvivorProfile(PharmacistId, AlchemistId, QuestlineSO.Ids.EmptyBottles);
                case VetId:
                    return new SurvivorProfile(VetId, ZoonoticExpertId, QuestlineSO.Ids.RabidPack);
                case TherapistId:
                    return new SurvivorProfile(TherapistId, AnchorId, QuestlineSO.Ids.BrokenMind);
                case UndertakerId:
                    return new SurvivorProfile(UndertakerId, DeathBlindId, QuestlineSO.Ids.MassGrave);
                case VeteranId:
                    return new SurvivorProfile(VeteranId, WarlordId, QuestlineSO.Ids.GhostsOfDay1);
                case CopId:
                    return new SurvivorProfile(CopId, PeacekeeperId, QuestlineSO.Ids.ThePrecinct);
                case BouncerId:
                    return new SurvivorProfile(BouncerId, JuggernautId, QuestlineSO.Ids.TheHoldout);
                case HunterId:
                    return new SurvivorProfile(HunterId, ApexPredatorId, QuestlineSO.Ids.TheWhiteElk);
                case PrisonerId:
                    return new SurvivorProfile(PrisonerId, SurvivalistId, QuestlineSO.Ids.TheWardensKey);
                case PlumberId:
                    return new SurvivorProfile(PlumberId, HydraulicMasterId, QuestlineSO.Ids.TheCityMains);
                case ElectricianId:
                    return new SurvivorProfile(ElectricianId, GridWalkerId, QuestlineSO.Ids.TheSubstationGhost);
                case ArchitectId:
                    return new SurvivorProfile(ArchitectId, VaultBuilderId, QuestlineSO.Ids.TheBlueprints);
                case MechanicId:
                    return new SurvivorProfile(MechanicId, GreaseMonkeyId, QuestlineSO.Ids.TheMotorpool);
                case ChemistId:
                    return new SurvivorProfile(ChemistId, SynthesizerId, QuestlineSO.Ids.TheLabRuin);
                case BotanistId:
                    return new SurvivorProfile(BotanistId, GaiaId, QuestlineSO.Ids.TheSeedVault);
                case CourierId:
                    return new SurvivorProfile(CourierId, WastelandRunnerId, QuestlineSO.Ids.TheLostRoute);
                case BurglarId:
                    return new SurvivorProfile(BurglarId, GhostId, QuestlineSO.Ids.TheBankHeist);
                case MeteorologistId:
                    return new SurvivorProfile(MeteorologistId, StormcallerId, QuestlineSO.Ids.TheRadarStation);
                case HazmatTechId:
                    return new SurvivorProfile(HazmatTechId, RadWalkerId, QuestlineSO.Ids.GroundZero);
                case TeacherId:
                    return new SurvivorProfile(TeacherId, PolymathId, QuestlineSO.Ids.TheAbandonedSchool);
                case PoliticianId:
                    return new SurvivorProfile(PoliticianId, DemagogueId, QuestlineSO.Ids.TheRally);
                case PriestId:
                    return new SurvivorProfile(PriestId, ShepherdId, QuestlineSO.Ids.CrisisOfFaith);
                case ReporterId:
                    return new SurvivorProfile(ReporterId, MuckrakerId, QuestlineSO.Ids.TruthOfDay30);
                case RadioHostId:
                    return new SurvivorProfile(RadioHostId, VoiceOfTheWastesId, QuestlineSO.Ids.DeadAir);
                case ChefId:
                    return new SurvivorProfile(ChefId, IronChefId, QuestlineSO.Ids.TheFinalHarvest);
                case AthleteId:
                    return new SurvivorProfile(AthleteId, TirelessId, QuestlineSO.Ids.TheMarathon);
                case FirefighterId:
                    return new SurvivorProfile(FirefighterId, AsbestosId, QuestlineSO.Ids.TheInferno);
                case TailorId:
                    return new SurvivorProfile(TailorId, ArmorerId, QuestlineSO.Ids.TheKevlarLoom);
                case WatchmakerId:
                    return new SurvivorProfile(WatchmakerId, TinkererId, QuestlineSO.Ids.BrokenChronometer);
                case HistorianId:
                    return new SurvivorProfile(HistorianId, LorekeeperId, QuestlineSO.Ids.MuseumArchive);
                case DefectorId:
                    return new SurvivorProfile(DefectorId, ZealotsBaneId, QuestlineSO.Ids.TheCleansing);
                case AddictId:
                    return new SurvivorProfile(AddictId, ChemResistantId, QuestlineSO.Ids.TheLastStash);
                case ParentId:
                    return new SurvivorProfile(ParentId, ProtectorId, QuestlineSO.Ids.TheLocket);
                case FierceMotherId:
                    return new SurvivorProfile(FierceMotherId, MatriarchId, QuestlineSO.Ids.TheEmptyCrib);
                case ExhaustedFatherId:
                    return new SurvivorProfile(ExhaustedFatherId, PillarOfAtlasId, QuestlineSO.Ids.TheBrokenPromise);
                case NaiveSonId:
                    return new SurvivorProfile(NaiveSonId, WastelandScoutId, QuestlineSO.Ids.GrowingUpFast);
                case HardenedDaughterId:
                    return new SurvivorProfile(HardenedDaughterId, ChildOfTheAshId, QuestlineSO.Ids.FirstBlood);
                case PsychopathId:
                    return new SurvivorProfile(PsychopathId, ColdCalculusId, QuestlineSO.Ids.ThePerfectEquation);
                case SerialKillerId:
                    return new SurvivorProfile(SerialKillerId, ButcherOfDay30Id, QuestlineSO.Ids.TheMaskSlips);
                case LiarId:
                    return new SurvivorProfile(LiarId, MasterManipulatorId, QuestlineSO.Ids.TheBoyWhoCriedWolf);
                case HoarderId:
                    return new SurvivorProfile(HoarderId, DragonsHoardId, QuestlineSO.Ids.TheWeightOfGold);
                case GeneralId:
                    return new SurvivorProfile(GeneralId, ArtOfWarId, QuestlineSO.Ids.CourtMartial);
                case SaboteurId:
                    return new SurvivorProfile(SaboteurId, DemolitionsExpertId, QuestlineSO.Ids.TheFinalPayload);
                case DeserterId:
                    return new SurvivorProfile(DeserterId, GhostShooterId, QuestlineSO.Ids.HoldingTheLine);
                case QuartermasterId:
                    return new SurvivorProfile(QuartermasterId, SupplyChainMasterId, QuestlineSO.Ids.InventoryAudit);
                case ChildSoldierId:
                    return new SurvivorProfile(ChildSoldierId, ReclaimedYouthId, QuestlineSO.Ids.DroppingTheRifle);
                case EmpathId:
                    return new SurvivorProfile(EmpathId, SoulWeaverId, QuestlineSO.Ids.TheSponge);
                case MisanthropeId:
                    return new SurvivorProfile(MisanthropeId, LoneWolfId, QuestlineSO.Ids.HellIsOtherPeople);
                case ThePollyannaId:
                    return new SurvivorProfile(ThePollyannaId, GroundedOptimistId, QuestlineSO.Ids.ShatteredGlass);
                case MartyrId:
                    return new SurvivorProfile(MartyrId, LivingSaintId, QuestlineSO.Ids.TheUltimatePrice);
                case ArrogantSurgeonId:
                    return new SurvivorProfile(ArrogantSurgeonId, HumbledHealerId, QuestlineSO.Ids.TheBotchedJob);
                case RelapsingAddictId:
                    return new SurvivorProfile(RelapsingAddictId, CleanAndSoberId, QuestlineSO.Ids.ColdTurkey);
                case InsomniacId:
                    return new SurvivorProfile(InsomniacId, TheWatcherId, QuestlineSO.Ids.TheLongNight);
                case HypochondriacId:
                    return new SurvivorProfile(HypochondriacId, HyperAwareId, QuestlineSO.Ids.TheRealIllness);
                case PyromaniacId:
                    return new SurvivorProfile(PyromaniacId, FireBreatherId, QuestlineSO.Ids.TrialByFire);
                case BlindPreacherId:
                    return new SurvivorProfile(BlindPreacherId, SonarId, QuestlineSO.Ids.AVoiceInTheDark);
                case PrepperId:
                    return new SurvivorProfile(PrepperId, ImprovisedEngineeringId, QuestlineSO.Ids.TheBunkerBreached);
                case OutcastId:
                    return new SurvivorProfile(OutcastId, RadiotrophicId, QuestlineSO.Ids.EmbracingTheGlow);
                case FeralOrphanId:
                    return new SurvivorProfile(FeralOrphanId, ApexScavengerId, QuestlineSO.Ids.ThePack);
                case PacifistId:
                    return new SurvivorProfile(PacifistId, ZenStateId, QuestlineSO.Ids.TheUltimateTest);
                case WidowId:
                    return new SurvivorProfile(WidowId, MasterGeneticistId, QuestlineSO.Ids.TheLastSeed);
                case ExConId:
                    return new SurvivorProfile(ExConId, TheEnforcerId, QuestlineSO.Ids.RedemptionArc);
                case SheriffId:
                    return new SurvivorProfile(SheriffId, LegendOfTheWastesId, QuestlineSO.Ids.TheLastRide);
                case FormerPoliticianId:
                    return new SurvivorProfile(FormerPoliticianId, TheStatesmanId, QuestlineSO.Ids.ARealLeader);
                case TechBroId:
                    return new SurvivorProfile(TechBroId, CyberneticsId, QuestlineSO.Ids.TheHardReboot);
                case NewsAnchorId:
                    return new SurvivorProfile(NewsAnchorId, BeaconOfTruthId, QuestlineSO.Ids.TheFinalBroadcast);
                case NomadId:
                    return new SurvivorProfile(NomadId, MasterPathologistId, QuestlineSO.Ids.PuttingDownRoots);
                case ExecId:
                    return new SurvivorProfile(ExecId, MonopolistId, QuestlineSO.Ids.TheGoldenParachute);
                case CoalMinerId:
                    return new SurvivorProfile(CoalMinerId, DeepDelverId, QuestlineSO.Ids.TheCanary);
                case TruckDriverId:
                    return new SurvivorProfile(TruckDriverId, LogisticsMasterId, QuestlineSO.Ids.TheLongHaul);
                case WelderId:
                    return new SurvivorProfile(WelderId, ForgeMasterId, QuestlineSO.Ids.TheIronGate);
                case CustodianId:
                    return new SurvivorProfile(CustodianId, SanitizationExpertId, QuestlineSO.Ids.TheMess);
                case LumberjackId:
                    return new SurvivorProfile(LumberjackId, DeforesterId, QuestlineSO.Ids.TheClearcut);
                case MicrobiologistId:
                    return new SurvivorProfile(MicrobiologistId, EpidemiologistId, QuestlineSO.Ids.TheStrain);
                case AstronomerId:
                    return new SurvivorProfile(AstronomerId, CelestialNavigatorId, QuestlineSO.Ids.TheDeadStars);
                case LibrarianId:
                    return new SurvivorProfile(LibrarianId, ArchivistId, QuestlineSO.Ids.TheArchive);
                case AccountantId:
                    return new SurvivorProfile(AccountantId, AuditorId, QuestlineSO.Ids.InTheBlack);
                case MusicianId:
                    return new SurvivorProfile(MusicianId, MaestroId, QuestlineSO.Ids.TheMasterpiece);
                case SmugglerId:
                    return new SurvivorProfile(SmugglerId, BlockadeRunnerId, QuestlineSO.Ids.TheStash);
                case HitmanId:
                    return new SurvivorProfile(HitmanId, ExecutionerId, QuestlineSO.Ids.TheLastContract);
                case PickpocketId:
                    return new SurvivorProfile(PickpocketId, ShadowId, QuestlineSO.Ids.TheBigScore);
                case ForgerId:
                    return new SurvivorProfile(ForgerId, MasterOfDisguiseId, QuestlineSO.Ids.ThePerfectFake);
                case GetawayDriverId:
                    return new SurvivorProfile(GetawayDriverId, MechanicProdigyId, QuestlineSO.Ids.TheEscape);
                case PromQueenId:
                    return new SurvivorProfile(PromQueenId, DiplomatId, QuestlineSO.Ids.TheRealWorld);
                case JockId:
                    return new SurvivorProfile(JockId, WastelandGladiatorId, QuestlineSO.Ids.TheGladiator);
                case MedStudentId:
                    return new SurvivorProfile(MedStudentId, ChiefOfMedicineId, QuestlineSO.Ids.TheFirstSave);
                case GamerId:
                    return new SurvivorProfile(GamerId, DroneOperatorId, QuestlineSO.Ids.PlayerOne);
                case ChoirBoyId:
                    return new SurvivorProfile(ChoirBoyId, ChoirOfOneId, QuestlineSO.Ids.LossOfFaith);
                case TwinAlphaId:
                    return new SurvivorProfile(TwinAlphaId, HiveTacticsId, QuestlineSO.Ids.SeparationAnxiety);
                case TwinBetaId:
                    return new SurvivorProfile(TwinBetaId, HiveHealingId, QuestlineSO.Ids.TheIndependent);
                case TheoristId:
                    return new SurvivorProfile(TheoristId, TruthSeekerId, QuestlineSO.Ids.Vindicated);
                case HermitId:
                    return new SurvivorProfile(HermitId, WildmanId, QuestlineSO.Ids.TheOldWoods);
                case PatientId:
                    return new SurvivorProfile(PatientId, SecondLifeId, QuestlineSO.Ids.TheAwakening);
                case EscapeeId:
                    return new SurvivorProfile(EscapeeId, IronWillId, QuestlineSO.Ids.BreakingChains);
                case StowawayId:
                    return new SurvivorProfile(StowawayId, UnseenListenerId, QuestlineSO.Ids.EarningKeep);
                case BillionaireId:
                    return new SurvivorProfile(BillionaireId, RuthlessCapitalistId, QuestlineSO.Ids.WorthlessPaper);
                case ApprenticeId:
                    return new SurvivorProfile(ApprenticeId, ProdigyId, QuestlineSO.Ids.TheMastersFate);
                case FireChiefId:
                    return new SurvivorProfile(FireChiefId, CommanderId, QuestlineSO.Ids.TheBackdraft);
                case AmputeeId:
                    return new SurvivorProfile(AmputeeId, CyberArmId, QuestlineSO.Ids.TheProsthetic);
                case InmateId:
                    return new SurvivorProfile(InmateId, RedemptionId, QuestlineSO.Ids.AGoodDeath);
                case SynthId:
                    return new SurvivorProfile(SynthId, OverclockedId, QuestlineSO.Ids.TearsInRain);
                case DogId:
                    return new SurvivorProfile(DogId, WastelandGuardianId, QuestlineSO.Ids.MansBestFriend);
                case CoreId:
                    return new SurvivorProfile(CoreId, OmniscienceId, QuestlineSO.Ids.TheTuringTest);
                default:
                    return null;
            }
        }

        public static Survivor MakeArchetypeSurvivor(string archetypeId, string runtimeId = null)
        {
            var profile = ProfileForArchetype(archetypeId);
            if (profile == null) return null;
            string name = archetypeId switch
            {
                SurgeonId => "The Surgeon",
                PharmacistId => "The Pharmacist",
                VetId => "The Vet",
                TherapistId => "The Therapist",
                UndertakerId => "The Undertaker",
                VeteranId => "The Veteran",
                CopId => "The Cop",
                BouncerId => "The Bouncer",
                HunterId => "The Hunter",
                PrisonerId => "The Prisoner",
                PlumberId => "The Plumber",
                ElectricianId => "The Electrician",
                ArchitectId => "The Architect",
                MechanicId => "The Mechanic",
                ChemistId => "The Chemist",
                BotanistId => "The Botanist",
                CourierId => "The Courier",
                BurglarId => "The Burglar",
                MeteorologistId => "The Meteorologist",
                HazmatTechId => "The Hazmat Tech",
                TeacherId => "The Teacher",
                PoliticianId => "The Politician",
                PriestId => "The Priest",
                ReporterId => "The Reporter",
                RadioHostId => "The Radio Host",
                ChefId => "The Chef",
                AthleteId => "The Athlete",
                FirefighterId => "The Firefighter",
                TailorId => "The Tailor",
                WatchmakerId => "The Watchmaker",
                HistorianId => "The Historian",
                DefectorId => "The Defector",
                AddictId => "The Addict",
                ParentId => "The Parent",
                FierceMotherId => "The Fierce Mother",
                ExhaustedFatherId => "The Exhausted Father",
                NaiveSonId => "The Naive Son",
                HardenedDaughterId => "The Hardened Daughter",
                PsychopathId => "The Psychopath",
                SerialKillerId => "The Serial Killer",
                LiarId => "The Liar",
                HoarderId => "The Hoarder",
                GeneralId => "The General",
                SaboteurId => "The Saboteur",
                DeserterId => "The Deserter",
                QuartermasterId => "The Quartermaster",
                ChildSoldierId => "The Child Soldier",
                EmpathId => "The Empath",
                MisanthropeId => "The Misanthrope",
                ThePollyannaId => "The Pollyanna",
                MartyrId => "The Martyr",
                ArrogantSurgeonId => "The Arrogant Surgeon",
                RelapsingAddictId => "The Relapsing Addict",
                InsomniacId => "The Insomniac",
                HypochondriacId => "The Hypochondriac",
                PyromaniacId => "The Pyromaniac",
                BlindPreacherId => "The Blind Preacher",
                PrepperId => "The Prepper",
                OutcastId => "The Outcast",
                FeralOrphanId => "The Feral Orphan",
                PacifistId => "The Pacifist",
                WidowId => "The Widow",
                ExConId => "The Ex-Con",
                SheriffId => "The Sheriff",
                FormerPoliticianId => "The Former Politician",
                TechBroId => "The Tech Bro",
                NewsAnchorId => "The News Anchor",
                NomadId => "The Nomad",
                ExecId => "The Exec",
                CoalMinerId => "The Coal Miner",
                TruckDriverId => "The Truck Driver",
                WelderId => "The Welder",
                CustodianId => "The Custodian",
                LumberjackId => "The Lumberjack",
                MicrobiologistId => "The Microbiologist",
                AstronomerId => "The Astronomer",
                LibrarianId => "The Librarian",
                AccountantId => "The Accountant",
                MusicianId => "The Musician",
                SmugglerId => "The Smuggler",
                HitmanId => "The Hitman",
                PickpocketId => "The Pickpocket",
                ForgerId => "The Forger",
                GetawayDriverId => "The Getaway Driver",
                PromQueenId => "The Prom Queen",
                JockId => "The Jock",
                MedStudentId => "The Med Student",
                GamerId => "The Gamer",
                ChoirBoyId => "The Choir Boy",
                TwinAlphaId => "Twin Alpha",
                TwinBetaId => "Twin Beta",
                TheoristId => "The Theorist",
                HermitId => "The Hermit",
                PatientId => "The Patient",
                EscapeeId => "The Escapee",
                StowawayId => "The Stowaway",
                BillionaireId => "The Billionaire",
                ApprenticeId => "The Apprentice",
                FireChiefId => "The Fire Chief",
                AmputeeId => "The Amputee",
                InmateId => "The Inmate",
                SynthId => "The Synth",
                DogId => "The Dog",
                CoreId => "The Core",
                _ => archetypeId
            };
            string discipline = "survival";
            if (archetypeId == SurgeonId || archetypeId == PharmacistId
                || archetypeId == VetId || archetypeId == TherapistId
                || archetypeId == ChemistId || archetypeId == AddictId
                || archetypeId == ArrogantSurgeonId || archetypeId == RelapsingAddictId
                || archetypeId == HypochondriacId || archetypeId == OutcastId
                || archetypeId == MicrobiologistId || archetypeId == MedStudentId
                || archetypeId == PatientId)
                discipline = "medical";
            else if (archetypeId == VeteranId || archetypeId == CopId || archetypeId == BouncerId
                || archetypeId == FirefighterId || archetypeId == DefectorId
                || archetypeId == AthleteId || archetypeId == HardenedDaughterId
                || archetypeId == SerialKillerId || archetypeId == GeneralId
                || archetypeId == DeserterId || archetypeId == ChildSoldierId
                || archetypeId == SaboteurId || archetypeId == SheriffId
                || archetypeId == ExConId || archetypeId == PyromaniacId
                || archetypeId == LumberjackId || archetypeId == HitmanId
                || archetypeId == JockId || archetypeId == InmateId
                || archetypeId == FireChiefId || archetypeId == TwinAlphaId)
                discipline = "combat";
            else if (archetypeId == HunterId || archetypeId == CourierId
                || archetypeId == BurglarId || archetypeId == MechanicId
                || archetypeId == ReporterId || archetypeId == TailorId
                || archetypeId == NaiveSonId || archetypeId == HoarderId
                || archetypeId == QuartermasterId || archetypeId == PrepperId
                || archetypeId == NomadId || archetypeId == FeralOrphanId
                || archetypeId == ExecId || archetypeId == TechBroId
                || archetypeId == TruckDriverId || archetypeId == SmugglerId
                || archetypeId == PickpocketId || archetypeId == GetawayDriverId
                || archetypeId == WelderId || archetypeId == CoalMinerId
                || archetypeId == GamerId || archetypeId == HermitId
                || archetypeId == StowawayId || archetypeId == AmputeeId
                || archetypeId == SynthId)
                discipline = "scavenging";
            else if (archetypeId == TeacherId || archetypeId == PoliticianId
                || archetypeId == PriestId || archetypeId == RadioHostId
                || archetypeId == HistorianId || archetypeId == ParentId
                || archetypeId == FierceMotherId || archetypeId == LiarId
                || archetypeId == PsychopathId || archetypeId == EmpathId
                || archetypeId == MartyrId || archetypeId == ThePollyannaId
                || archetypeId == BlindPreacherId || archetypeId == PacifistId
                || archetypeId == WidowId || archetypeId == FormerPoliticianId
                || archetypeId == NewsAnchorId || archetypeId == LibrarianId
                || archetypeId == AccountantId || archetypeId == MusicianId
                || archetypeId == ForgerId || archetypeId == CustodianId
                || archetypeId == AstronomerId || archetypeId == PromQueenId
                || archetypeId == ChoirBoyId || archetypeId == TwinBetaId
                || archetypeId == TheoristId || archetypeId == EscapeeId
                || archetypeId == BillionaireId || archetypeId == ApprenticeId
                || archetypeId == DogId || archetypeId == CoreId)
                discipline = "social";
            else if (archetypeId == ExhaustedFatherId || archetypeId == MisanthropeId
                || archetypeId == InsomniacId)
                discipline = "survival";
            var sv = new Survivor
            {
                Id = runtimeId ?? archetypeId,
                DisplayName = name,
                State = SurvivorState.Idle,
                ExpertDisciplineId = discipline
            };
            sv.Needs.Morale = 60f;
            sv.Needs.Health = 100f;
            if (archetypeId == HardenedDaughterId)
                sv.Needs.Morale = Mathf.Min(sv.Needs.Morale, TraumatizedMoraleCap);
            if (archetypeId == LibrarianId)
                sv.Needs.Health = Mathf.Min(sv.Needs.Health, FrailMaxHealthCap);
            return sv;
        }

        // ── Daily / trigger ticks ─────────────────────────────────────────

        /// <summary>
        /// Advance days-alive and auto-start questlines at day 30.
        /// Call once per campaign day.
        /// </summary>
        public void TickDaily(IReadOnlyList<Survivor> survivors, int currentDay = 0)
        {
            if (survivors == null) return;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                var state = GetOrCreate(sv.Id);
                SyncFromSurvivor(sv, state);

                state.DaysAlive++;
                sv.DaysAlive = state.DaysAlive;

                if (!state.QuestActive && !state.TraitUnlocked
                    && state.DaysAlive >= DaysAliveToStartQuest
                    && !string.IsNullOrEmpty(state.QuestlineId))
                {
                    TryStartQuestline(sv, "days_alive", currentDay);
                }

                // Anchor: lock morale at 100 every day tick.
                if (HasTrait(sv, AnchorId))
                    sv.Needs.Morale = 100f;

                // ── #267–#283 daily host quest progress (production paths) ──
                TickChemistryTitlesDaily(sv, state, survivors, currentDay);

                // ── #284–#298 rebuilders / scholars / outlaws ─────────────
                TickRebuildersDaily(sv, state, survivors, currentDay);

                // ── #299–#318 ashes / improbable / final flawed ───────────
                TickAshesDaily(sv, state, survivors, currentDay);
            }
        }

        /// <summary>#267: mark that this survivor used an addictive chem today.</summary>
        public void NotifyChemUsed(Survivor sv)
        {
            if (sv == null || string.IsNullOrEmpty(sv.Id)) return;
            GetOrCreate(sv.Id).UsedChemThisDay = true;
        }

        /// <summary>
        /// Daily host orchestration for chemistry/titles quests that can advance
        /// from survivor state alone (no external event).
        /// </summary>
        private void TickChemistryTitlesDaily(
            Survivor sv,
            PersonalQuestState state,
            IReadOnlyList<Survivor> survivors,
            int currentDay)
        {
            // #267 Cold Turkey: one clean day when no chem was used.
            if (state.QuestActive
                && string.Equals(state.QuestlineId, QuestlineSO.Ids.ColdTurkey, StringComparison.Ordinal))
            {
                RecordColdTurkeyCleanDay(sv, usedAnyChem: state.UsedChemThisDay, currentDay);
            }
            state.UsedChemThisDay = false;

            // #282 Agoraphile: full day inside advances flee counter + morale hit.
            if (HasAgoraphile(sv) && !sv.IsOnExpedition)
                ApplyAgoraphileBunkerDay(sv, spentDayInside: true);
            else if (HasAgoraphile(sv) && sv.IsOnExpedition)
                RecordOutsideDay(sv);

            // #273 Radiotrophic unlock: lifetime mSv milestone.
            if (state.QuestActive
                && string.Equals(state.QuestlineId, QuestlineSO.Ids.EmbracingTheGlow, StringComparison.Ordinal)
                && sv.LifetimeRadiationExposure >= EmbracingGlowLifetimeRads)
            {
                RecordLifetimeRadsMilestone(
                    sv, sv.LifetimeRadiationExposure, isAlive: true, currentDay);
            }

            // #279 Real Leader: if they did dirty labor today, credit the day.
            if (state.DidDirtyLaborThisDay)
            {
                RecordDirtyLaborDay(sv, didDirtyJob: true, currentDay);
                state.DidDirtyLaborThisDay = false;
            }
        }

        /// <summary>
        /// Daily host orchestration for #284–#298 quirks that advance from
        /// survivor state alone (antsy bunker days, caffeinated water crash).
        /// </summary>
        private void TickRebuildersDaily(
            Survivor sv,
            PersonalQuestState state,
            IReadOnlyList<Survivor> survivors,
            int currentDay)
        {
            // #298 Antsy: full day inside advances flee-from-bunker pressure.
            if ((HasAntsy(sv) || string.Equals(sv.ArchetypeId, GetawayDriverId, StringComparison.Ordinal))
                && !sv.IsOnExpedition)
                TickAntsyBunkerDay(sv, spentDayInside: true, currentDay);
            else if (string.Equals(sv.ArchetypeId, GetawayDriverId, StringComparison.Ordinal)
                     || HasAntsy(sv) || HasMechanicProdigy(sv))
                TickAntsyBunkerDay(sv, spentDayInside: false, currentDay);

            // #285 Caffeinated: miss clean water → fatigue crash.
            if (NeedsConstantCleanWater(sv))
            {
                ApplyCaffeinatedWaterCrash(sv, drankCleanWaterToday: state.DrankCleanWaterThisDay);
                state.DrankCleanWaterThisDay = false;
            }

            // #287 Neat Freak: hygiene below 80% hits morale once per day.
            if (HasNeatFreak(sv) && sv.Needs != null)
                ApplyNeatFreakHygienePressure(sv, sv.Needs.Hygiene / 100f);
        }

        /// <summary>
        /// Daily host orchestration for #299–#318 ashes quirks that advance
        /// from survivor state alone (patient hydration streak, dog aura).
        /// </summary>
        private void TickAshesDaily(
            Survivor sv,
            PersonalQuestState state,
            IReadOnlyList<Survivor> survivors,
            int currentDay)
        {
            // #308 Patient: a day kept alive and hydrated advances The Awakening.
            if (IsComatoseBurden(sv))
            {
                bool hydrated = sv.Needs != null
                    && sv.Needs.Thirst < PatientAwakeningHydrationThreshold
                    && sv.Needs.Hunger < PatientAwakeningHydrationThreshold;
                TickAwakeningDay(sv, keptAliveAndHydrated: hydrated, currentDay);
            }

            // #317 Dog: room-mates enjoy a small passive morale aura.
            if (ProvidesGoodBoyRoomMorale(sv) && survivors != null)
                ApplyGoodBoyMoraleAura(sv, survivors);
        }

        /// <summary>
        /// Watch morale for the 0→100 emotional trigger. Call from needs tick
        /// or any path that mutates morale.
        /// </summary>
        public void WatchMorale(Survivor sv, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive) return;
            var state = GetOrCreate(sv.Id);
            SyncFromSurvivor(sv, state);
            if (state.QuestActive || state.TraitUnlocked) return;
            if (string.IsNullOrEmpty(state.QuestlineId)) return;

            if (sv.Needs.Morale <= MoraleFloorTrigger)
            {
                state.MoraleHitZero = true;
                sv.MoraleHitZero = true;
            }
            else if (state.MoraleHitZero && sv.Needs.Morale >= MoraleRecoveryTrigger)
            {
                TryStartQuestline(sv, "morale_recovery", currentDay);
            }
        }

        public void WatchMoraleAll(IReadOnlyList<Survivor> survivors, int currentDay = 0)
        {
            if (survivors == null) return;
            for (int i = 0; i < survivors.Count; i++)
                WatchMorale(survivors[i], currentDay);
        }

        /// <summary>Start the assigned questline if not already active/done.</summary>
        public bool TryStartQuestline(Survivor sv, string reason, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive) return false;
            var state = GetOrCreate(sv.Id);
            SyncFromSurvivor(sv, state);
            if (state.QuestActive || state.TraitUnlocked) return false;
            if (string.IsNullOrEmpty(state.QuestlineId)) return false;

            state.QuestActive = true;
            state.Stage = 0;
            state.Progress = 0f;
            sv.QuestlineActive = true;
            sv.QuestStage = 0;
            sv.QuestProgress = 0f;

            var ql = GetQuestline(state.QuestlineId);
            if (ql != null)
            {
                if (!string.IsNullOrEmpty(ql.spawnMapNodeId))
                    OnMapNodeSpawnRequested?.Invoke(ql.spawnMapNodeId, sv.Id);
                if (!string.IsNullOrEmpty(ql.spawnBunkerEventId))
                    OnBunkerEventRequested?.Invoke(ql.spawnBunkerEventId, sv.Id);
            }

            OnQuestlineStarted?.Invoke(sv, state.QuestlineId);
            return true;
        }

        // ── #215 Surgeon — The Shaking Hand ──────────────────────────────

        /// <summary>
        /// Record a successful Phase-2 operation while the Surgeon's morale is
        /// below 30. After 3, completes The Shaking Hand.
        /// </summary>
        public void RecordStressPhase2Operation(Survivor surgeon, int currentDay = 0)
        {
            if (surgeon == null || !surgeon.IsAlive) return;
            var state = GetOrCreate(surgeon.Id);
            SyncFromSurvivor(surgeon, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.ShakingHand, StringComparison.Ordinal))
                return;
            if (surgeon.Needs.Morale >= SurgeonStressMoraleMax) return;

            state.Progress += 1f;
            surgeon.QuestProgress = state.Progress;
            OnQuestProgress?.Invoke(surgeon, "stress_ops", (int)state.Progress);
            if (state.Progress >= SurgeonStressOpsRequired)
                CompleteQuestline(surgeon, currentDay);
        }

        // ── #216 Pharmacist — The Empty Bottles ──────────────────────────

        /// <summary>
        /// Record a solo visit to The Ruined CVS that recovered the pharmacy
        /// logbook (and survived Encounter_Looters). Completes Empty Bottles.
        /// </summary>
        public void RecordPharmacyLogbookRetrieved(Survivor pharmacist, int currentDay = 0)
        {
            if (pharmacist == null || !pharmacist.IsAlive) return;
            var state = GetOrCreate(pharmacist.Id);
            SyncFromSurvivor(pharmacist, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.EmptyBottles, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            pharmacist.QuestProgress = 1f;
            OnQuestProgress?.Invoke(pharmacist, "logbook_retrieved", 1);
            CompleteQuestline(pharmacist, currentDay);
        }

        // ── #217 Vet — The Rabid Pack ────────────────────────────────────

        /// <summary>
        /// Record hours spent in the airlock curing the Alpha with medical kits.
        /// Completes when kits spent ≥ 3 and hours ≥ 48.
        /// </summary>
        public void RecordVetAlphaCure(
            Survivor vet,
            float hoursSpent,
            int medicalKitsSpent,
            int currentDay = 0)
        {
            if (vet == null || !vet.IsAlive) return;
            var state = GetOrCreate(vet.Id);
            SyncFromSurvivor(vet, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.RabidPack, StringComparison.Ordinal))
                return;

            state.VetAirlockHours += Mathf.Max(0f, hoursSpent);
            state.VetKitsSpent += Mathf.Max(0, medicalKitsSpent);
            state.Progress = state.VetAirlockHours;
            vet.QuestProgress = state.Progress;
            OnQuestProgress?.Invoke(vet, "vet_airlock_hours", Mathf.FloorToInt(state.VetAirlockHours));

            if (state.VetAirlockHours >= VetAirlockHoursRequired
                && state.VetKitsSpent >= VetMedicalKitsRequired)
            {
                CompleteQuestline(vet, currentDay);
            }
        }

        // ── #218 Therapist — The Broken Mind ─────────────────────────────

        /// <summary>
        /// Record a successful ViolentParanoia de-escalation by the Therapist.
        /// After 3, completes The Broken Mind.
        /// </summary>
        public void RecordTherapistDeEscalation(Survivor therapist, int currentDay = 0)
        {
            if (therapist == null || !therapist.IsAlive) return;
            var state = GetOrCreate(therapist.Id);
            SyncFromSurvivor(therapist, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.BrokenMind, StringComparison.Ordinal))
                return;

            state.Progress += 1f;
            therapist.QuestProgress = state.Progress;
            OnQuestProgress?.Invoke(therapist, "de_escalations", (int)state.Progress);
            if (state.Progress >= TherapistDeEscalationsRequired)
                CompleteQuestline(therapist, currentDay);
        }

        // ── #219 Undertaker — The Mass Grave ─────────────────────────────

        /// <summary>
        /// Record burial hours at The Mass Grave. At 24h applies fatigue/rad
        /// hits and completes the questline (closure).
        /// </summary>
        public void RecordMassGraveBurial(
            Survivor undertaker,
            float hours,
            int currentDay = 0)
        {
            if (undertaker == null || !undertaker.IsAlive) return;
            var state = GetOrCreate(undertaker.Id);
            SyncFromSurvivor(undertaker, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.MassGrave, StringComparison.Ordinal))
                return;

            state.Progress += Mathf.Max(0f, hours);
            undertaker.QuestProgress = state.Progress;
            OnQuestProgress?.Invoke(undertaker, "burial_hours", Mathf.FloorToInt(state.Progress));

            if (state.Progress >= MassGraveHoursRequired && !state.TraitUnlocked)
            {
                undertaker.Needs.Fatigue = Mathf.Clamp(
                    undertaker.Needs.Fatigue + MassGraveFatigueHit, 0f, 100f);
                ApplyQuestRadiation(undertaker, MassGraveRadHit);
                CompleteQuestline(undertaker, currentDay);
            }
        }

        // ── #220 Veteran — Ghosts of Day 1 ───────────────────────────────

        /// <summary>
        /// Record that the Veteran alone executed their feral cannibal squad
        /// at the fortified holdout. Completes Ghosts of Day 1.
        /// </summary>
        public void RecordFeralSquadExecuted(Survivor veteran, int currentDay = 0)
        {
            if (veteran == null || !veteran.IsAlive) return;
            var state = GetOrCreate(veteran.Id);
            SyncFromSurvivor(veteran, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.GhostsOfDay1, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            veteran.QuestProgress = 1f;
            OnQuestProgress?.Invoke(veteran, "feral_squad_executed", 1);
            CompleteQuestline(veteran, currentDay);
        }

        // ── #221 Cop — The Precinct ──────────────────────────────────────

        /// <summary>
        /// Record that the Cop returned the evidence lockbox and cracked it
        /// (finding only family photos). Completes The Precinct.
        /// </summary>
        public void RecordEvidenceLockboxCracked(Survivor cop, int currentDay = 0)
        {
            if (cop == null || !cop.IsAlive) return;
            var state = GetOrCreate(cop.Id);
            SyncFromSurvivor(cop, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.ThePrecinct, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            cop.QuestProgress = 1f;
            OnQuestProgress?.Invoke(cop, "lockbox_cracked", 1);
            CompleteQuestline(cop, currentDay);
        }

        // ── #222 Bouncer — The Holdout ───────────────────────────────────

        /// <summary>
        /// Record a HatchBreach raid where the Bouncer was the sole guard and survived.
        /// Completes The Holdout.
        /// </summary>
        public void RecordSoloHatchDefense(
            Survivor bouncer,
            int activeGuardCount,
            bool survived,
            int currentDay = 0)
        {
            if (bouncer == null || !bouncer.IsAlive || !survived) return;
            var state = GetOrCreate(bouncer.Id);
            SyncFromSurvivor(bouncer, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheHoldout, StringComparison.Ordinal))
                return;
            if (activeGuardCount != 1) return;

            state.Progress = 1f;
            bouncer.QuestProgress = 1f;
            OnQuestProgress?.Invoke(bouncer, "solo_hatch_holdout", 1);
            CompleteQuestline(bouncer, currentDay);
        }

        // ── #223 Hunter — The White Elk ──────────────────────────────────

        /// <summary>
        /// Record a tracking visit on a White Elk node. After 3 distinct nodes
        /// the Hunter may finish with a ScrapBow kill.
        /// </summary>
        public void RecordWhiteElkNodeVisit(Survivor hunter, string nodeId, int currentDay = 0)
        {
            if (hunter == null || !hunter.IsAlive || string.IsNullOrEmpty(nodeId)) return;
            var state = GetOrCreate(hunter.Id);
            SyncFromSurvivor(hunter, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheWhiteElk, StringComparison.Ordinal))
                return;

            if (state.VisitedNodeIds == null)
                state.VisitedNodeIds = new List<string>();
            for (int i = 0; i < state.VisitedNodeIds.Count; i++)
            {
                if (string.Equals(state.VisitedNodeIds[i], nodeId, StringComparison.Ordinal))
                    return;
            }
            state.VisitedNodeIds.Add(nodeId);
            state.Progress = state.VisitedNodeIds.Count;
            hunter.QuestProgress = state.Progress;
            OnQuestProgress?.Invoke(hunter, "white_elk_nodes", state.VisitedNodeIds.Count);
        }

        /// <summary>
        /// Finish the White Elk hunt. Requires 3 nodes tracked and ScrapBow (no firearm).
        /// </summary>
        public void RecordWhiteElkKill(
            Survivor hunter,
            bool usedScrapBow,
            bool usedFirearm,
            int currentDay = 0)
        {
            if (hunter == null || !hunter.IsAlive) return;
            var state = GetOrCreate(hunter.Id);
            SyncFromSurvivor(hunter, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheWhiteElk, StringComparison.Ordinal))
                return;
            if (!usedScrapBow || usedFirearm) return;
            int nodes = state.VisitedNodeIds != null ? state.VisitedNodeIds.Count : 0;
            if (nodes < WhiteElkNodesRequired) return;

            CompleteQuestline(hunter, currentDay);
        }

        // ── #224 Prisoner — The Warden's Key ─────────────────────────────

        /// <summary>
        /// Record that the Prisoner confronted the ghoulified Warden and took the keys.
        /// Completes The Warden's Key.
        /// </summary>
        public void RecordWardenKeysRetrieved(Survivor prisoner, int currentDay = 0)
        {
            if (prisoner == null || !prisoner.IsAlive) return;
            var state = GetOrCreate(prisoner.Id);
            SyncFromSurvivor(prisoner, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheWardensKey, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            prisoner.QuestProgress = 1f;
            OnQuestProgress?.Invoke(prisoner, "warden_keys", 1);
            CompleteQuestline(prisoner, currentDay);
        }


        // ── #225 Plumber — The City Mains ────────────────────────────────

        /// <summary>
        /// Record fixing a massive pipe burst while submerged in irradiated water.
        /// Completes The City Mains.
        /// </summary>
        public void RecordPipeBurstFixed(
            Survivor plumber,
            bool submergedInIrradiatedWater,
            int currentDay = 0)
        {
            if (plumber == null || !plumber.IsAlive || !submergedInIrradiatedWater) return;
            var state = GetOrCreate(plumber.Id);
            SyncFromSurvivor(plumber, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheCityMains, StringComparison.Ordinal))
                return;

            ApplyQuestRadiation(plumber, PipeBurstIrradiatedRadSpike);
            state.Progress = 1f;
            plumber.QuestProgress = 1f;
            OnQuestProgress?.Invoke(plumber, "pipe_burst_fixed", 1);
            CompleteQuestline(plumber, currentDay);
        }

        // ── #226 Electrician — The Substation Ghost ──────────────────────

        public void RecordSubstationRepaired(
            Survivor electrician,
            bool duringFalloutStorm,
            int currentDay = 0)
        {
            if (electrician == null || !electrician.IsAlive || !duringFalloutStorm) return;
            var state = GetOrCreate(electrician.Id);
            SyncFromSurvivor(electrician, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheSubstationGhost, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            electrician.QuestProgress = 1f;
            OnQuestProgress?.Invoke(electrician, "substation_repaired", 1);
            CompleteQuestline(electrician, currentDay);
        }

        // ── #227 Architect — The Blueprints ──────────────────────────────

        public void RecordBlueprintsRecovered(Survivor architect, int currentDay = 0)
        {
            if (architect == null || !architect.IsAlive) return;
            var state = GetOrCreate(architect.Id);
            SyncFromSurvivor(architect, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheBlueprints, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            architect.QuestProgress = 1f;
            OnQuestProgress?.Invoke(architect, "blueprints_recovered", 1);
            CompleteQuestline(architect, currentDay);
        }

        // ── #228 Mechanic — The Motorpool ────────────────────────────────

        public void RecordEngineBlockRetrieved(Survivor mechanic, int currentDay = 0)
        {
            if (mechanic == null || !mechanic.IsAlive) return;
            var state = GetOrCreate(mechanic.Id);
            SyncFromSurvivor(mechanic, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheMotorpool, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            mechanic.QuestProgress = 1f;
            OnQuestProgress?.Invoke(mechanic, "engine_block", 1);
            CompleteQuestline(mechanic, currentDay);
        }

        // ── #229 Chemist — The Lab Ruin ──────────────────────────────────

        /// <summary>
        /// Cap a leaking chlorine tank with the Chemist's body as a shield.
        /// Grants permanent ScarredLungs and completes The Lab Ruin.
        /// </summary>
        public void RecordChlorineTankCapped(Survivor chemist, int currentDay = 0)
        {
            if (chemist == null || !chemist.IsAlive) return;
            var state = GetOrCreate(chemist.Id);
            SyncFromSurvivor(chemist, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheLabRuin, StringComparison.Ordinal))
                return;

            if (!chemist.HasDisability(ScarredLungsId))
                chemist.DisabilityIds.Add(ScarredLungsId);

            state.Progress = 1f;
            chemist.QuestProgress = 1f;
            OnQuestProgress?.Invoke(chemist, "chlorine_capped", 1);
            CompleteQuestline(chemist, currentDay);
        }

        // ── #230 Botanist — The Seed Vault ───────────────────────────────

        /// <summary>
        /// Record one day of perfect 100% PlanterBox health. Completes after 14 straight days.
        /// </summary>
        public void RecordPlanterPerfectDay(
            Survivor botanist,
            bool planterAtFullHealth,
            int currentDay = 0)
        {
            if (botanist == null || !botanist.IsAlive) return;
            var state = GetOrCreate(botanist.Id);
            SyncFromSurvivor(botanist, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheSeedVault, StringComparison.Ordinal))
                return;

            if (!planterAtFullHealth)
            {
                state.PerfectPlanterDays = 0;
                botanist.QuestProgress = 0f;
                OnQuestProgress?.Invoke(botanist, "planter_streak_reset", 0);
                return;
            }

            state.PerfectPlanterDays++;
            state.Progress = state.PerfectPlanterDays;
            botanist.QuestProgress = state.PerfectPlanterDays;
            OnQuestProgress?.Invoke(botanist, "planter_perfect_days", state.PerfectPlanterDays);
            if (state.PerfectPlanterDays >= SeedVaultPerfectDaysRequired)
                CompleteQuestline(botanist, currentDay);
        }

        // ── #231 Courier — The Lost Route ────────────────────────────────

        public void RecordDeadDropSuccess(Survivor courier, int currentDay = 0)
        {
            if (courier == null || !courier.IsAlive) return;
            var state = GetOrCreate(courier.Id);
            SyncFromSurvivor(courier, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheLostRoute, StringComparison.Ordinal))
                return;

            state.DeadDropSuccesses++;
            state.Progress = state.DeadDropSuccesses;
            courier.QuestProgress = state.DeadDropSuccesses;
            OnQuestProgress?.Invoke(courier, "dead_drop_success", state.DeadDropSuccesses);
            if (state.DeadDropSuccesses >= LostRouteDeadDropsRequired)
                CompleteQuestline(courier, currentDay);
        }

        /// <summary>Stolen / robbed dead drop resets Lost Route progress.</summary>
        public void RecordDeadDropFailure(Survivor courier)
        {
            if (courier == null) return;
            var state = GetOrCreate(courier.Id);
            SyncFromSurvivor(courier, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheLostRoute, StringComparison.Ordinal))
                return;
            state.DeadDropSuccesses = 0;
            state.Progress = 0f;
            courier.QuestProgress = 0f;
            OnQuestProgress?.Invoke(courier, "dead_drop_reset", 0);
        }

        // ── #232 Burglar — The Bank Heist ────────────────────────────────

        public void RecordVaultCracked(
            Survivor burglar,
            bool alarmTriggered,
            int currentDay = 0)
        {
            if (burglar == null || !burglar.IsAlive || alarmTriggered) return;
            var state = GetOrCreate(burglar.Id);
            SyncFromSurvivor(burglar, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheBankHeist, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            burglar.QuestProgress = 1f;
            OnQuestProgress?.Invoke(burglar, "vault_cracked", 1);
            CompleteQuestline(burglar, currentDay);
        }

        // ── #233 Meteorologist — The Radar Station ───────────────────────

        public void RecordRadarDishAligned(
            Survivor meteorologist,
            bool duringFalloutStorm,
            int currentDay = 0)
        {
            if (meteorologist == null || !meteorologist.IsAlive || !duringFalloutStorm) return;
            var state = GetOrCreate(meteorologist.Id);
            SyncFromSurvivor(meteorologist, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheRadarStation, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            meteorologist.QuestProgress = 1f;
            OnQuestProgress?.Invoke(meteorologist, "radar_aligned", 1);
            CompleteQuestline(meteorologist, currentDay);
        }

        // ── #234 Hazmat Tech — Ground Zero ───────────────────────────────

        public void RecordBlackBoxRetrieved(Survivor hazmat, int currentDay = 0)
        {
            if (hazmat == null || !hazmat.IsAlive) return;
            var state = GetOrCreate(hazmat.Id);
            SyncFromSurvivor(hazmat, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.GroundZero, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            hazmat.QuestProgress = 1f;
            OnQuestProgress?.Invoke(hazmat, "black_box", 1);
            CompleteQuestline(hazmat, currentDay);
        }


        // ── #235 Teacher — The Abandoned School ──────────────────────────

        public void RecordRationManifestFound(Survivor teacher, int currentDay = 0)
        {
            if (teacher == null || !teacher.IsAlive) return;
            var state = GetOrCreate(teacher.Id);
            SyncFromSurvivor(teacher, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheAbandonedSchool, StringComparison.Ordinal))
                return;
            if (state.ManifestFound) return;
            state.ManifestFound = true;
            state.Progress = 0.5f;
            teacher.QuestProgress = 0.5f;
            OnQuestProgress?.Invoke(teacher, "ration_manifest", 1);
        }

        public void RecordTeacherMourningDay(Survivor teacher, int currentDay = 0)
        {
            if (teacher == null || !teacher.IsAlive) return;
            var state = GetOrCreate(teacher.Id);
            SyncFromSurvivor(teacher, state);
            if (!state.QuestActive || !state.ManifestFound) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheAbandonedSchool, StringComparison.Ordinal))
                return;
            state.TeacherMourningDays++;
            state.Progress = 0.5f + 0.5f * state.TeacherMourningDays / TeacherMourningDaysRequired;
            teacher.QuestProgress = state.Progress;
            OnQuestProgress?.Invoke(teacher, "mourning_days", state.TeacherMourningDays);
            if (state.TeacherMourningDays >= TeacherMourningDaysRequired)
                CompleteQuestline(teacher, currentDay);
        }

        // ── #236 Politician — The Rally ──────────────────────────────────

        public void RecordPropagandaHostileResolution(Survivor politician, int currentDay = 0)
        {
            if (politician == null || !politician.IsAlive) return;
            var state = GetOrCreate(politician.Id);
            SyncFromSurvivor(politician, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheRally, StringComparison.Ordinal))
                return;
            state.PropagandaResolutions++;
            state.Progress = state.PropagandaResolutions;
            politician.QuestProgress = state.PropagandaResolutions;
            OnQuestProgress?.Invoke(politician, "propaganda_resolution", state.PropagandaResolutions);
            if (state.PropagandaResolutions >= PropagandaResolutionsRequired)
                CompleteQuestline(politician, currentDay);
        }

        // ── #237 Priest — Crisis of Faith ────────────────────────────────

        public void RecordCrisisOfFaith(Survivor priest, int currentDay = 0)
        {
            if (priest == null || !priest.IsAlive) return;
            var state = GetOrCreate(priest.Id);
            SyncFromSurvivor(priest, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.CrisisOfFaith, StringComparison.Ordinal))
                return;
            state.CrisisOfFaithActive = true;
            priest.Needs.Morale = 0f;
            priest.currentMentalBreakId = CrisisOfFaithBreakId;
            state.Progress = 0.5f;
            priest.QuestProgress = 0.5f;
            OnQuestProgress?.Invoke(priest, "crisis_of_faith", 1);
        }

        public void RecordTalkDownSavedPriest(Survivor priest, Survivor savior, int currentDay = 0)
        {
            if (priest == null || !priest.IsAlive) return;
            if (savior == null || !savior.IsAlive || savior.Id == priest.Id) return;
            var state = GetOrCreate(priest.Id);
            SyncFromSurvivor(priest, state);
            if (!state.QuestActive || !state.CrisisOfFaithActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.CrisisOfFaith, StringComparison.Ordinal))
                return;
            state.CrisisOfFaithActive = false;
            if (string.Equals(priest.currentMentalBreakId, CrisisOfFaithBreakId, StringComparison.OrdinalIgnoreCase))
                priest.currentMentalBreakId = null;
            state.Progress = 1f;
            priest.QuestProgress = 1f;
            OnQuestProgress?.Invoke(priest, "talked_down", 1);
            CompleteQuestline(priest, currentDay);
        }

        /// <summary>#237 Shepherd — 2h sermon raises every bunker survivor morale by 20, ignoring debuffs.</summary>
        public bool TryPerformSermon(Survivor priest, IReadOnlyList<Survivor> survivors, float durationHours = SermonDurationHours)
        {
            if (priest == null || !priest.IsAlive || !HasShepherd(priest)) return false;
            if (durationHours + 0.0001f < SermonDurationHours) return false;
            if (survivors == null) return false;
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s == null || !s.IsAlive) continue;
                s.Needs.Morale = Mathf.Clamp(s.Needs.Morale + SermonMoraleBoost, 0f, 100f);
            }
            priest.Needs.Fatigue = Mathf.Clamp(priest.Needs.Fatigue + 10f, 0f, 100f);
            return true;
        }

        // ── #238 Reporter — Truth of Day 30 ──────────────────────────────

        public void RecordFirstStrikeIntel(Survivor reporter, string intelId, int currentDay = 0)
        {
            if (reporter == null || !reporter.IsAlive || string.IsNullOrEmpty(intelId)) return;
            var state = GetOrCreate(reporter.Id);
            SyncFromSurvivor(reporter, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TruthOfDay30, StringComparison.Ordinal))
                return;
            if (state.FirstStrikeIntelIds == null)
                state.FirstStrikeIntelIds = new List<string>();
            for (int i = 0; i < state.FirstStrikeIntelIds.Count; i++)
            {
                if (string.Equals(state.FirstStrikeIntelIds[i], intelId, StringComparison.Ordinal))
                    return;
            }
            state.FirstStrikeIntelIds.Add(intelId);
            state.Progress = state.FirstStrikeIntelIds.Count;
            reporter.QuestProgress = state.FirstStrikeIntelIds.Count;
            OnQuestProgress?.Invoke(reporter, "first_strike_intel", state.FirstStrikeIntelIds.Count);
            if (state.FirstStrikeIntelIds.Count >= FirstStrikeIntelRequired)
                CompleteQuestline(reporter, currentDay);
        }

        // ── #239 Radio Host — Dead Air ───────────────────────────────────

        public void RecordContinuousBroadcastHours(
            Survivor host,
            float hours,
            bool duringBlizzard,
            bool maxedFatigueAndThirst,
            int currentDay = 0)
        {
            if (host == null || !host.IsAlive || hours <= 0f) return;
            var state = GetOrCreate(host.Id);
            SyncFromSurvivor(host, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.DeadAir, StringComparison.Ordinal))
                return;
            if (!duringBlizzard) return;
            state.BroadcastHours += hours;
            if (maxedFatigueAndThirst)
            {
                host.Needs.Fatigue = 100f;
                host.Needs.Thirst = 100f;
            }
            state.Progress = state.BroadcastHours;
            host.QuestProgress = state.BroadcastHours;
            OnQuestProgress?.Invoke(host, "broadcast_hours", Mathf.FloorToInt(state.BroadcastHours));
            if (state.BroadcastHours + 0.0001f >= DeadAirBroadcastHoursRequired)
                CompleteQuestline(host, currentDay);
        }

        // ── #240 Chef — The Final Harvest ────────────────────────────────

        public void RecordFoodItemHoarded(Survivor chef, string foodItemId, int currentDay = 0)
        {
            if (chef == null || !chef.IsAlive || string.IsNullOrEmpty(foodItemId)) return;
            var state = GetOrCreate(chef.Id);
            SyncFromSurvivor(chef, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheFinalHarvest, StringComparison.Ordinal))
                return;
            if (state.HoardedFoodIds == null)
                state.HoardedFoodIds = new List<string>();
            for (int i = 0; i < state.HoardedFoodIds.Count; i++)
            {
                if (string.Equals(state.HoardedFoodIds[i], foodItemId, StringComparison.Ordinal))
                    return;
            }
            state.HoardedFoodIds.Add(foodItemId);
            state.Progress = state.HoardedFoodIds.Count;
            chef.QuestProgress = state.HoardedFoodIds.Count;
            OnQuestProgress?.Invoke(chef, "food_hoarded", state.HoardedFoodIds.Count);
        }

        /// <summary>
        /// Complete The Final Harvest after hoarding every required food id and cooking 24h.
        /// </summary>
        public void RecordLastSupperCooked(
            Survivor chef,
            IReadOnlyList<string> requiredFoodIds,
            float cookHours,
            int currentDay = 0)
        {
            if (chef == null || !chef.IsAlive) return;
            var state = GetOrCreate(chef.Id);
            SyncFromSurvivor(chef, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheFinalHarvest, StringComparison.Ordinal))
                return;
            if (cookHours + 0.0001f < LastSupperCookHours) return;
            if (requiredFoodIds == null || requiredFoodIds.Count == 0) return;
            if (state.HoardedFoodIds == null) return;
            for (int i = 0; i < requiredFoodIds.Count; i++)
            {
                string need = requiredFoodIds[i];
                bool found = false;
                for (int j = 0; j < state.HoardedFoodIds.Count; j++)
                {
                    if (string.Equals(state.HoardedFoodIds[j], need, StringComparison.Ordinal))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found) return;
            }
            state.Progress = 1f;
            chef.QuestProgress = 1f;
            OnQuestProgress?.Invoke(chef, "last_supper", 1);
            CompleteQuestline(chef, currentDay);
        }

        /// <summary>#240 Iron Chef meal: full Hunger/Thirst/Fatigue restore (0 = sated).</summary>
        public void ApplyIronChefMeal(Survivor eater)
        {
            if (eater == null || !eater.IsAlive) return;
            eater.Needs.Hunger = 0f;
            eater.Needs.Thirst = 0f;
            eater.Needs.Fatigue = 0f;
        }

        // ── #241 Athlete — The Marathon ──────────────────────────────────

        public void RecordMarathonExpedition(
            Survivor athlete,
            int nodesAway,
            float hoursElapsed,
            bool onFoot,
            bool returnedHome,
            int currentDay = 0)
        {
            if (athlete == null || !athlete.IsAlive) return;
            var state = GetOrCreate(athlete.Id);
            SyncFromSurvivor(athlete, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheMarathon, StringComparison.Ordinal))
                return;
            if (!onFoot || !returnedHome) return;
            if (nodesAway < MarathonMinNodesAway) return;
            if (hoursElapsed > MarathonMaxHours + 0.0001f) return;
            state.Progress = 1f;
            athlete.QuestProgress = 1f;
            OnQuestProgress?.Invoke(athlete, "marathon", 1);
            CompleteQuestline(athlete, currentDay);
        }

        // ── #242 Firefighter — The Inferno ───────────────────────────────

        public void RecordInfernoExtinguished(
            Survivor firefighter,
            string roomId,
            bool woreHazmatSuit,
            int currentDay = 0)
        {
            if (firefighter == null || !firefighter.IsAlive) return;
            if (woreHazmatSuit) return;
            var state = GetOrCreate(firefighter.Id);
            SyncFromSurvivor(firefighter, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheInferno, StringComparison.Ordinal))
                return;
            if (!string.IsNullOrEmpty(roomId)
                && !string.Equals(roomId, GeneratorRoomId, StringComparison.OrdinalIgnoreCase)
                && !string.Equals(roomId, "generator_room", StringComparison.OrdinalIgnoreCase))
                return;
            SurvivorNeedWrite.SetHealth(firefighter, Mathf.Max(1f, firefighter.Needs.Health - InfernoBurnDamage));
            state.Progress = 1f;
            firefighter.QuestProgress = 1f;
            OnQuestProgress?.Invoke(firefighter, "inferno_extinguished", 1);
            CompleteQuestline(firefighter, currentDay);
        }

        // ── #243 Tailor — The Kevlar Loom ────────────────────────────────

        public void RecordClothingDisassembled(Survivor tailor, int currentDay = 0)
        {
            if (tailor == null || !tailor.IsAlive) return;
            var state = GetOrCreate(tailor.Id);
            SyncFromSurvivor(tailor, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheKevlarLoom, StringComparison.Ordinal))
                return;
            state.ClothingScrapsDisassembled++;
            state.Progress = state.ClothingScrapsDisassembled;
            tailor.QuestProgress = state.ClothingScrapsDisassembled;
            OnQuestProgress?.Invoke(tailor, "clothing_scraps", state.ClothingScrapsDisassembled);
            if (state.ClothingScrapsDisassembled >= ClothingScrapsRequired)
                CompleteQuestline(tailor, currentDay);
        }

        // ── #244 Watchmaker — Broken Chronometer ─────────────────────────

        public void RecordWatchRepaired(Survivor watchmaker, int electronicScrapSpent, int currentDay = 0)
        {
            if (watchmaker == null || !watchmaker.IsAlive) return;
            if (electronicScrapSpent < WatchRepairScrapRequired) return;
            var state = GetOrCreate(watchmaker.Id);
            SyncFromSurvivor(watchmaker, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.BrokenChronometer, StringComparison.Ordinal))
                return;
            state.Progress = 1f;
            watchmaker.QuestProgress = 1f;
            OnQuestProgress?.Invoke(watchmaker, "watch_repaired", electronicScrapSpent);
            CompleteQuestline(watchmaker, currentDay);
        }

        // ── #245 Historian — Museum Archive ──────────────────────────────

        public void RecordConstitutionRetrieved(
            Survivor historian,
            bool museumBurning,
            int currentDay = 0)
        {
            if (historian == null || !historian.IsAlive || !museumBurning) return;
            var state = GetOrCreate(historian.Id);
            SyncFromSurvivor(historian, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.MuseumArchive, StringComparison.Ordinal))
                return;
            state.Progress = 1f;
            historian.QuestProgress = 1f;
            OnQuestProgress?.Invoke(historian, "constitution", 1);
            CompleteQuestline(historian, currentDay);
        }

        // ── #246 Cult Defector — The Cleansing ───────────────────────────

        public void RecordCultLeaderKilled(Survivor defector, string killedId, int currentDay = 0)
        {
            if (defector == null || !defector.IsAlive) return;
            if (!string.Equals(killedId, CultLeaderId, StringComparison.OrdinalIgnoreCase)
                && !string.Equals(killedId, "the_cult_leader", StringComparison.OrdinalIgnoreCase))
                return;
            var state = GetOrCreate(defector.Id);
            SyncFromSurvivor(defector, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheCleansing, StringComparison.Ordinal))
                return;
            state.Progress = 1f;
            defector.QuestProgress = 1f;
            OnQuestProgress?.Invoke(defector, "cult_leader_killed", 1);
            CompleteQuestline(defector, currentDay);
        }

        // ── #247 Addict — The Last Stash ─────────────────────────────────

        public void RecordWithdrawalCleanDay(Survivor addict, bool relapsed, int currentDay = 0)
        {
            if (addict == null || !addict.IsAlive) return;
            var state = GetOrCreate(addict.Id);
            SyncFromSurvivor(addict, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheLastStash, StringComparison.Ordinal))
                return;
            if (relapsed)
            {
                state.WithdrawalCleanDays = 0;
                state.Progress = 0f;
                addict.QuestProgress = 0f;
                OnQuestProgress?.Invoke(addict, "withdrawal_relapse", 0);
                return;
            }
            state.WithdrawalCleanDays++;
            state.Progress = state.WithdrawalCleanDays;
            addict.QuestProgress = state.WithdrawalCleanDays;
            OnQuestProgress?.Invoke(addict, "withdrawal_clean_days", state.WithdrawalCleanDays);
            if (state.WithdrawalCleanDays >= WithdrawalCleanDaysRequired)
                CompleteQuestline(addict, currentDay);
        }

        // ── #248 Parent — The Locket ─────────────────────────────────────

        public void RecordChildDeathIntel(Survivor parent, int currentDay = 0)
        {
            if (parent == null || !parent.IsAlive) return;
            var state = GetOrCreate(parent.Id);
            SyncFromSurvivor(parent, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheLocket, StringComparison.Ordinal))
                return;
            state.ChildDeathKnown = true;
            parent.Needs.Morale = 0f;
            parent.currentMentalBreakId = DespairBreakId;
            state.Progress = 0.5f;
            parent.QuestProgress = 0.5f;
            OnQuestProgress?.Invoke(parent, "child_death_intel", 1);
        }

        public void RecordParentMourningSurvived(Survivor parent, float mourningDays, int currentDay = 0)
        {
            if (parent == null || !parent.IsAlive) return;
            var state = GetOrCreate(parent.Id);
            SyncFromSurvivor(parent, state);
            if (!state.QuestActive || !state.ChildDeathKnown) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheLocket, StringComparison.Ordinal))
                return;
            if (mourningDays + 0.0001f < ParentMourningDaysRequired) return;
            if (string.Equals(parent.currentMentalBreakId, DespairBreakId, StringComparison.OrdinalIgnoreCase))
                parent.currentMentalBreakId = null;
            state.Progress = 1f;
            parent.QuestProgress = 1f;
            OnQuestProgress?.Invoke(parent, "mourning_survived", 1);
            CompleteQuestline(parent, currentDay);
        }

        // ── #249 Fierce Mother — The Empty Crib ──────────────────────────

        /// <summary>
        /// Recover the pre-war daycare toy from The Daycare node while severely
        /// irradiated (radiation ≥ SevereRadiationThreshold). Completes Empty Crib.
        /// </summary>
        public void RecordDaycareToyRetrieved(
            Survivor mother,
            float radiationLevel,
            string nodeId = null,
            int currentDay = 0)
        {
            if (mother == null || !mother.IsAlive) return;
            var state = GetOrCreate(mother.Id);
            SyncFromSurvivor(mother, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheEmptyCrib, StringComparison.Ordinal))
                return;
            if (radiationLevel + 0.0001f < SevereRadiationThreshold) return;
            if (!string.IsNullOrEmpty(nodeId)
                && !string.Equals(nodeId, DaycareNodeId, StringComparison.OrdinalIgnoreCase))
                return;

            state.Progress = 1f;
            mother.QuestProgress = 1f;
            OnQuestProgress?.Invoke(mother, "daycare_toy", 1);
            CompleteQuestline(mother, currentDay);
        }

        // ── #250 Exhausted Father — The Broken Promise ───────────────────

        /// <summary>
        /// Record a Tier-3 shelter module completed by the father.
        /// Completes when count ≥ 5 and currentDay ≤ 50.
        /// </summary>
        public void RecordTier3ModuleBuilt(Survivor father, int moduleLevel, int currentDay = 0)
        {
            if (father == null || !father.IsAlive) return;
            if (moduleLevel < 3) return;
            var state = GetOrCreate(father.Id);
            SyncFromSurvivor(father, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheBrokenPromise, StringComparison.Ordinal))
                return;
            if (currentDay > BrokenPromiseDayDeadline) return;

            state.Tier3ModulesBuilt++;
            state.Progress = state.Tier3ModulesBuilt;
            father.QuestProgress = state.Tier3ModulesBuilt;
            OnQuestProgress?.Invoke(father, "tier3_modules", state.Tier3ModulesBuilt);
            if (state.Tier3ModulesBuilt >= BrokenPromiseTier3Required
                && currentDay <= BrokenPromiseDayDeadline)
            {
                CompleteQuestline(father, currentDay);
            }
        }

        // ── #251 Naive Son — Growing Up Fast ─────────────────────────────

        /// <summary>
        /// Survive a raid event alone in a room with no adults present.
        /// </summary>
        public void RecordSoloRaidSurvived(
            Survivor son,
            bool adultsPresentInRoom,
            bool raidSurvived,
            int currentDay = 0)
        {
            if (son == null || !son.IsAlive) return;
            if (adultsPresentInRoom || !raidSurvived) return;
            var state = GetOrCreate(son.Id);
            SyncFromSurvivor(son, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.GrowingUpFast, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            son.QuestProgress = 1f;
            OnQuestProgress?.Invoke(son, "solo_raid", 1);
            CompleteQuestline(son, currentDay);
        }

        // ── #252 Hardened Daughter — First Blood ─────────────────────────

        /// <summary>
        /// Land the killing blow on a Faction Raider during a hatch breach.
        /// </summary>
        public void RecordRaiderKillingBlow(
            Survivor daughter,
            bool duringHatchBreach,
            bool isFactionRaider,
            int currentDay = 0)
        {
            if (daughter == null || !daughter.IsAlive) return;
            if (!duringHatchBreach || !isFactionRaider) return;
            var state = GetOrCreate(daughter.Id);
            SyncFromSurvivor(daughter, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.FirstBlood, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            daughter.QuestProgress = 1f;
            OnQuestProgress?.Invoke(daughter, "first_blood", 1);
            CompleteQuestline(daughter, currentDay);
        }

        // ── #253 Psychopath — The Perfect Equation ───────────────────────

        /// <summary>
        /// Deliberately allow a survivor to die of starvation or thirst
        /// "to preserve resources." Completes Perfect Equation.
        /// </summary>
        public void RecordDeliberateNeedDeath(
            Survivor psychopath,
            string causeOfDeath,
            bool wasDeliberate,
            int currentDay = 0)
        {
            if (psychopath == null || !psychopath.IsAlive) return;
            if (!wasDeliberate) return;
            if (string.IsNullOrEmpty(causeOfDeath)) return;
            bool starvationOrThirst =
                causeOfDeath.IndexOf("starv", StringComparison.OrdinalIgnoreCase) >= 0
                || causeOfDeath.IndexOf("thirst", StringComparison.OrdinalIgnoreCase) >= 0
                || string.Equals(causeOfDeath, "hunger", StringComparison.OrdinalIgnoreCase);
            if (!starvationOrThirst) return;

            var state = GetOrCreate(psychopath.Id);
            SyncFromSurvivor(psychopath, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.ThePerfectEquation, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            psychopath.QuestProgress = 1f;
            OnQuestProgress?.Invoke(psychopath, "perfect_equation", 1);
            CompleteQuestline(psychopath, currentDay);
        }

        // ── #254 Serial Killer — The Mask Slips ──────────────────────────

        /// <summary>Advance The Urge hidden need (0–100). At max, may attempt murder.</summary>
        public void TickUrge(Survivor killer, float delta, IReadOnlyList<Survivor> survivors = null)
        {
            if (killer == null || !killer.IsAlive) return;
            var state = GetOrCreate(killer.Id);
            SyncFromSurvivor(killer, state);
            bool isKiller =
                string.Equals(state.ArchetypeId, SerialKillerId, StringComparison.Ordinal)
                || string.Equals(killer.ArchetypeId, SerialKillerId, StringComparison.Ordinal);
            if (!isKiller) return;

            state.UrgeNeed = Mathf.Clamp(state.UrgeNeed + Mathf.Max(0f, delta), 0f, UrgeMax);
            if (state.UrgeNeed + 0.0001f >= UrgeMurderThreshold)
                TrySecretMurder(killer, survivors);
        }

        public float GetUrgeNeed(Survivor killer)
        {
            if (killer == null) return 0f;
            return GetOrCreate(killer.Id).UrgeNeed;
        }

        /// <summary>
        /// Attempt secret murder of a disabled/comatose survivor or hatch emissary.
        /// Fires OnSecretMurderAttempted; host resolves capture vs success.
        /// </summary>
        public bool TrySecretMurder(Survivor killer, IReadOnlyList<Survivor> survivors)
        {
            if (killer == null || !killer.IsAlive) return false;
            var state = GetOrCreate(killer.Id);
            if (state.UrgeNeed + 0.0001f < UrgeMurderThreshold) return false;

            string targetId = null;
            string targetKind = "emissary";
            if (survivors != null)
            {
                for (int i = 0; i < survivors.Count; i++)
                {
                    var s = survivors[i];
                    if (s == null || !s.IsAlive || s.Id == killer.Id) continue;
                    // Disabled / comatose proxies: low health or active mental break / zero health near death.
                    if (s.Needs.Health <= 5f || s.HasMentalBreak
                        || string.Equals(s.State.ToString(), "Comatose", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(s.State.ToString(), "Disabled", StringComparison.OrdinalIgnoreCase))
                    {
                        targetId = s.Id;
                        targetKind = "disabled";
                        break;
                    }
                }
            }
            if (string.IsNullOrEmpty(targetId))
                targetId = "faction_emissary";

            state.MurderAttempted = true;
            OnSecretMurderAttempted?.Invoke(killer, targetId, targetKind);
            OnQuestProgress?.Invoke(killer, "murder_attempt", 1);
            return true;
        }

        /// <summary>
        /// Player catches the killer. Execute = quest ends without latent.
        /// Embrace = unlocks Butcher of Day 30.
        /// </summary>
        public void RecordMaskSlipsChoice(Survivor killer, bool embrace, int currentDay = 0)
        {
            if (killer == null || !killer.IsAlive) return;
            var state = GetOrCreate(killer.Id);
            SyncFromSurvivor(killer, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheMaskSlips, StringComparison.Ordinal))
                return;

            state.MaskSlipsResolved = true;
            state.SerialKillerEmbraced = embrace;
            state.Progress = 1f;
            killer.QuestProgress = 1f;
            OnQuestProgress?.Invoke(killer, embrace ? "embraced" : "executed", 1);

            if (embrace)
            {
                CompleteQuestline(killer, currentDay);
            }
            else
            {
                // Execute: quest complete, no latent trait.
                state.QuestActive = false;
                killer.QuestlineActive = false;
                state.TraitUnlocked = false;
                killer.LatentTraitUnlocked = false;
                killer.State = SurvivorState.Dead;
                OnQuestlineCompleted?.Invoke(killer, state.QuestlineId);
            }
        }

        // ── #255 Pathological Liar — The Boy Who Cried Wolf ──────────────

        /// <summary>Liar randomly plants a false intel node id (AI quirk).</summary>
        public string GenerateFalseIntelNode(Survivor liar, System.Random rng = null)
        {
            if (liar == null || !HasDeceptive(liar)) return null;
            rng ??= AtomicWar._Game.Utilities.SeededRandom.CreateFixed("personalquestsystem");
            string fakeId = "fake_stash_" + rng.Next(1000, 9999);
            var state = GetOrCreate(liar.Id);
            state.FalseIntelCount++;
            OnFalseIntelReported?.Invoke(liar, fakeId);
            return fakeId;
        }

        /// <summary>
        /// Cure a lethal Phase-2 affliction the Liar tried to hide.
        /// Completes Boy Who Cried Wolf.
        /// </summary>
        public void RecordLethalPhase2Cured(
            Survivor liar,
            bool wasHiddenFromPlayer,
            bool isPhase2Lethal,
            int currentDay = 0)
        {
            if (liar == null || !liar.IsAlive) return;
            if (!wasHiddenFromPlayer || !isPhase2Lethal) return;
            var state = GetOrCreate(liar.Id);
            SyncFromSurvivor(liar, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheBoyWhoCriedWolf, StringComparison.Ordinal))
                return;

            state.Progress = 1f;
            liar.QuestProgress = 1f;
            OnQuestProgress?.Invoke(liar, "hidden_affliction_cured", 1);
            CompleteQuestline(liar, currentDay);
        }

        // ── #256 Selfish Hoarder — The Weight of Gold ────────────────────

        /// <summary>
        /// Carry the 50kg safe across map nodes. Completes after 3 nodes when
        /// nearly dead of fatigue; the safe is empty.
        /// </summary>
        public void RecordSafeCarried(
            Survivor hoarder,
            float safeWeightKg,
            float fatigueLevel,
            int currentDay = 0)
        {
            if (hoarder == null || !hoarder.IsAlive) return;
            if (safeWeightKg + 0.0001f < WeightOfGoldSafeKg) return;
            var state = GetOrCreate(hoarder.Id);
            SyncFromSurvivor(hoarder, state);
            if (!state.QuestActive) return;
            if (!string.Equals(state.QuestlineId, QuestlineSO.Ids.TheWeightOfGold, StringComparison.Ordinal))
                return;

            state.SafeNodesCarried++;
            state.Progress = state.SafeNodesCarried;
            hoarder.QuestProgress = state.SafeNodesCarried;
            OnQuestProgress?.Invoke(hoarder, "safe_nodes", state.SafeNodesCarried);

            if (state.SafeNodesCarried >= WeightOfGoldNodesRequired
                && fatigueLevel >= 90f)
            {
                // Empty safe reveal — host may grant EmptySafeItemId.
                state.SafeWasEmpty = true;
                CompleteQuestline(hoarder, currentDay);
            }
        }

        /// <summary>#256 AI quirk — steal item id from main storage into personal stash.</summary>
        public bool TryStealToPersonalInventory(Survivor hoarder, string itemId)
        {
            if (hoarder == null || !hoarder.IsAlive || string.IsNullOrEmpty(itemId)) return false;
            if (!HasSelfish(hoarder) && !string.Equals(hoarder.ArchetypeId, HoarderId, StringComparison.Ordinal))
                return false;
            if (hoarder.HiddenItemIds == null)
                hoarder.HiddenItemIds = new List<string>();
            hoarder.HiddenItemIds.Add(itemId);
            hoarder.HasHiddenStash = true;
            var state = GetOrCreate(hoarder.Id);
            state.ItemsStolen++;
            OnQuestProgress?.Invoke(hoarder, "stolen_item", state.ItemsStolen);
            return true;
        }

        // ── Completion / unlock ──────────────────────────────────────────

        public bool CompleteQuestline(Survivor sv, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive) return false;
            var state = GetOrCreate(sv.Id);
            SyncFromSurvivor(sv, state);
            if (state.TraitUnlocked) return false;
            if (string.IsNullOrEmpty(state.LatentTraitId)) return false;

            state.QuestActive = false;
            state.TraitUnlocked = true;
            state.Stage = 1;
            sv.QuestlineActive = false;
            sv.LatentTraitUnlocked = true;
            sv.QuestStage = 1;

            OnQuestlineCompleted?.Invoke(sv, state.QuestlineId);

            bool granted = false;
            if (_progression != null)
            {
                granted = _progression.TryGrantPerk(sv, state.LatentTraitId, currentDay);
            }

            // Always mark trait ownership even if progression was unbound (tests).
            if (!sv.HasTrait(state.LatentTraitId))
                sv.Traits.Add(state.LatentTraitId);

            ApplyLatentTraitSideEffects(sv, state.LatentTraitId);

            string display = _progression?.GetPerk(state.LatentTraitId)?.displayName
                             ?? state.LatentTraitId;
            OnLatentTraitUnlocked?.Invoke(sv, state.LatentTraitId);
            OnCharacterEvolution?.Invoke(sv, state.LatentTraitId, display);
            return granted || state.TraitUnlocked;
        }

        /// <summary>#222 — permanent health doubling when Juggernaut unlocks.</summary>
        private static void ApplyLatentTraitSideEffects(Survivor sv, string traitId)
        {
            if (sv == null || string.IsNullOrEmpty(traitId)) return;
            if (string.Equals(traitId, JuggernautId, StringComparison.Ordinal))
            {
                float baseHp = sv.BaseMaxHealth > 0f ? sv.BaseMaxHealth : 100f;
                sv.BaseMaxHealth = baseHp * JuggernautHealthMultiplier;
                sv.Needs.Health = Mathf.Min(sv.Needs.Health * JuggernautHealthMultiplier, sv.MaxHealthCap);
            }
            if (string.Equals(traitId, TirelessId, StringComparison.Ordinal))
            {
                // Stamina/fatigue pools tripled — hosts read GetStaminaPoolMultiplier.
                sv.BaseMaxStamina = (sv.BaseMaxStamina > 0f ? sv.BaseMaxStamina : 100f) * TirelessPoolMult;
            }
            // #252 Child of the Ash inherits Sociopath benefits.
            if (string.Equals(traitId, ChildOfTheAshId, StringComparison.Ordinal))
            {
                if (sv.Traits == null) sv.Traits = new List<string>();
                if (!sv.HasTrait(SociopathId))
                    sv.Traits.Add(SociopathId);
                // Adult weapons unlocked — clear child fight restriction if any.
                sv.CannotFight = false;
            }
            // #251 Wasteland Scout can fight/scavenge as a scout.
            if (string.Equals(traitId, WastelandScoutId, StringComparison.Ordinal))
            {
                sv.CannotFight = false; // still no firearms via Dependent; host uses CanEquipFirearms
            }
            // #261 Reclaimed Youth — clear Stunted learning block + Night Terrors.
            if (string.Equals(traitId, ReclaimedYouthId, StringComparison.Ordinal))
            {
                if (sv.Traits != null)
                    sv.Traits.Remove(StuntedId);
                if (string.Equals(sv.currentMentalBreakId, NightTerrorsBreakId, StringComparison.OrdinalIgnoreCase))
                    sv.currentMentalBreakId = null;
            }
            // #264 Grounded Optimist — denial permanently broken.
            if (string.Equals(traitId, GroundedOptimistId, StringComparison.Ordinal))
            {
                if (sv.Traits != null)
                    sv.Traits.Remove(DenialistId);
            }
            // #266 Humbled Healer — remove God Complex labor refusal / patient abuse.
            if (string.Equals(traitId, HumbledHealerId, StringComparison.Ordinal))
            {
                if (sv.Traits != null)
                    sv.Traits.Remove(GodComplexId);
            }
        }

        // ── Trait queries ────────────────────────────────────────────────

        public bool HasTrait(Survivor sv, string traitId)
        {
            if (sv == null || string.IsNullOrEmpty(traitId)) return false;
            if (sv.LatentTraitUnlocked
                && string.Equals(sv.LatentExpertTraitId, traitId, StringComparison.Ordinal))
                return true;
            if (sv.HasTrait(traitId)) return true;
            if (_progression != null && _progression.HasActivePerk(sv.Id, traitId))
                return true;
            var state = GetOrCreate(sv.Id);
            return state.TraitUnlocked
                   && string.Equals(state.LatentTraitId, traitId, StringComparison.Ordinal);
        }

        public bool HasMiracleWorker(Survivor sv) => HasTrait(sv, MiracleWorkerId);
        public bool HasAlchemist(Survivor sv) => HasTrait(sv, AlchemistId);
        public bool HasZoonoticExpert(Survivor sv) => HasTrait(sv, ZoonoticExpertId);
        public bool HasAnchor(Survivor sv) => HasTrait(sv, AnchorId);
        public bool HasDeathBlind(Survivor sv) => HasTrait(sv, DeathBlindId);
        public bool HasWarlord(Survivor sv) => HasTrait(sv, WarlordId);
        public bool HasPeacekeeper(Survivor sv) => HasTrait(sv, PeacekeeperId);
        public bool HasJuggernaut(Survivor sv) => HasTrait(sv, JuggernautId);
        public bool HasApexPredator(Survivor sv) => HasTrait(sv, ApexPredatorId);
        public bool HasSurvivalist(Survivor sv) => HasTrait(sv, SurvivalistId);
        public bool HasHydraulicMaster(Survivor sv) => HasTrait(sv, HydraulicMasterId);
        public bool HasGridWalker(Survivor sv) => HasTrait(sv, GridWalkerId);
        public bool HasVaultBuilder(Survivor sv) => HasTrait(sv, VaultBuilderId);
        public bool HasGreaseMonkey(Survivor sv) => HasTrait(sv, GreaseMonkeyId);
        public bool HasSynthesizer(Survivor sv) => HasTrait(sv, SynthesizerId);
        public bool HasGaia(Survivor sv) => HasTrait(sv, GaiaId);
        public bool HasWastelandRunner(Survivor sv) => HasTrait(sv, WastelandRunnerId);
        public bool HasGhost(Survivor sv) => HasTrait(sv, GhostId);
        public bool HasStormcaller(Survivor sv) => HasTrait(sv, StormcallerId);
        public bool HasRadWalker(Survivor sv) => HasTrait(sv, RadWalkerId);
        public bool HasPolymath(Survivor sv) => HasTrait(sv, PolymathId);
        public bool HasDemagogue(Survivor sv) => HasTrait(sv, DemagogueId);
        public bool HasShepherd(Survivor sv) => HasTrait(sv, ShepherdId);
        public bool HasMuckraker(Survivor sv) => HasTrait(sv, MuckrakerId);
        public bool HasVoiceOfTheWastes(Survivor sv) => HasTrait(sv, VoiceOfTheWastesId);
        public bool HasIronChef(Survivor sv) => HasTrait(sv, IronChefId);
        public bool HasTireless(Survivor sv) => HasTrait(sv, TirelessId);
        public bool HasAsbestos(Survivor sv) => HasTrait(sv, AsbestosId);
        public bool HasArmorer(Survivor sv) => HasTrait(sv, ArmorerId);
        public bool HasTinkerer(Survivor sv) => HasTrait(sv, TinkererId);
        public bool HasLorekeeper(Survivor sv) => HasTrait(sv, LorekeeperId);
        public bool HasZealotsBane(Survivor sv) => HasTrait(sv, ZealotsBaneId);
        public bool HasChemResistant(Survivor sv) => HasTrait(sv, ChemResistantId);
        public bool HasProtector(Survivor sv) => HasTrait(sv, ProtectorId);
        public bool HasMatriarch(Survivor sv) => HasTrait(sv, MatriarchId);
        public bool HasPillarOfAtlas(Survivor sv) => HasTrait(sv, PillarOfAtlasId);
        public bool HasWastelandScout(Survivor sv) => HasTrait(sv, WastelandScoutId);
        public bool HasChildOfTheAsh(Survivor sv) => HasTrait(sv, ChildOfTheAshId);
        public bool HasColdCalculus(Survivor sv) => HasTrait(sv, ColdCalculusId);
        public bool HasButcherOfDay30(Survivor sv) => HasTrait(sv, ButcherOfDay30Id);
        public bool HasMasterManipulator(Survivor sv) => HasTrait(sv, MasterManipulatorId);
        public bool HasDragonsHoard(Survivor sv) => HasTrait(sv, DragonsHoardId);
        public bool HasArtOfWar(Survivor sv) => HasTrait(sv, ArtOfWarId);
        public bool HasDemolitionsExpert(Survivor sv) => HasTrait(sv, DemolitionsExpertId);
        public bool HasGhostShooter(Survivor sv) => HasTrait(sv, GhostShooterId);
        public bool HasSupplyChainMaster(Survivor sv) => HasTrait(sv, SupplyChainMasterId);
        public bool HasReclaimedYouth(Survivor sv) => HasTrait(sv, ReclaimedYouthId);
        public bool HasSoulWeaver(Survivor sv) => HasTrait(sv, SoulWeaverId);
        public bool HasLoneWolf(Survivor sv) => HasTrait(sv, LoneWolfId);
        public bool HasGroundedOptimist(Survivor sv) => HasTrait(sv, GroundedOptimistId);
        public bool HasLivingSaint(Survivor sv) => HasTrait(sv, LivingSaintId);
                public bool HasHumbledHealer(Survivor sv) => HasTrait(sv, HumbledHealerId);

        // Prompts #267–#276 latent
        public bool HasCleanAndSober(Survivor sv) => HasTrait(sv, CleanAndSoberId);
        public bool HasTheWatcher(Survivor sv) => HasTrait(sv, TheWatcherId);
        public bool HasHyperAware(Survivor sv) => HasTrait(sv, HyperAwareId);
        public bool HasFireBreather(Survivor sv) => HasTrait(sv, FireBreatherId);
        public bool HasSonar(Survivor sv) => HasTrait(sv, SonarId);
        public bool HasImprovisedEngineering(Survivor sv) => HasTrait(sv, ImprovisedEngineeringId);
        public bool HasRadiotrophic(Survivor sv) => HasTrait(sv, RadiotrophicId);
        public bool HasApexScavenger(Survivor sv) => HasTrait(sv, ApexScavengerId);
        public bool HasZenState(Survivor sv) => HasTrait(sv, ZenStateId);
        public bool HasMasterGeneticist(Survivor sv) => HasTrait(sv, MasterGeneticistId);
        // Prompts #277–#283 latent
        public bool HasTheEnforcer(Survivor sv) => HasTrait(sv, TheEnforcerId);
        public bool HasLegendOfTheWastes(Survivor sv) => HasTrait(sv, LegendOfTheWastesId);
        public bool HasTheStatesman(Survivor sv) => HasTrait(sv, TheStatesmanId);
        public bool HasCybernetics(Survivor sv) => HasTrait(sv, CyberneticsId);
        public bool HasBeaconOfTruth(Survivor sv) => HasTrait(sv, BeaconOfTruthId);
        public bool HasMasterPathologist(Survivor sv) => HasTrait(sv, MasterPathologistId);
        public bool HasMonopolist(Survivor sv) => HasTrait(sv, MonopolistId);
        // Prompts #284–#298 latent
        public bool HasDeepDelver(Survivor sv) => HasTrait(sv, DeepDelverId);
        public bool HasLogisticsMaster(Survivor sv) => HasTrait(sv, LogisticsMasterId);
        public bool HasForgeMaster(Survivor sv) => HasTrait(sv, ForgeMasterId);
        public bool HasSanitizationExpert(Survivor sv) => HasTrait(sv, SanitizationExpertId);
        public bool HasDeforester(Survivor sv) => HasTrait(sv, DeforesterId);
        public bool HasEpidemiologist(Survivor sv) => HasTrait(sv, EpidemiologistId);
        public bool HasCelestialNavigator(Survivor sv) => HasTrait(sv, CelestialNavigatorId);
        public bool HasArchivist(Survivor sv) => HasTrait(sv, ArchivistId);
        public bool HasAuditor(Survivor sv) => HasTrait(sv, AuditorId);
        public bool HasMaestro(Survivor sv) => HasTrait(sv, MaestroId);
        public bool HasBlockadeRunner(Survivor sv) => HasTrait(sv, BlockadeRunnerId);
        public bool HasExecutioner(Survivor sv) => HasTrait(sv, ExecutionerId);
        public bool HasShadow(Survivor sv) => HasTrait(sv, ShadowId);
        public bool HasMasterOfDisguise(Survivor sv) => HasTrait(sv, MasterOfDisguiseId);
        public bool HasMechanicProdigy(Survivor sv) => HasTrait(sv, MechanicProdigyId);
        // Prompts #299–#318 latent
        public bool HasDiplomat(Survivor sv) => HasTrait(sv, DiplomatId);
        public bool HasWastelandGladiator(Survivor sv) => HasTrait(sv, WastelandGladiatorId);
        public bool HasChiefOfMedicine(Survivor sv) => HasTrait(sv, ChiefOfMedicineId);
        public bool HasDroneOperator(Survivor sv) => HasTrait(sv, DroneOperatorId);
        public bool HasChoirOfOne(Survivor sv) => HasTrait(sv, ChoirOfOneId);
        public bool HasHiveTactics(Survivor sv) => HasTrait(sv, HiveTacticsId);
        public bool HasHiveHealing(Survivor sv) => HasTrait(sv, HiveHealingId);
        public bool HasTruthSeeker(Survivor sv) => HasTrait(sv, TruthSeekerId);
        public bool HasWildman(Survivor sv) => HasTrait(sv, WildmanId);
        public bool HasSecondLife(Survivor sv) => HasTrait(sv, SecondLifeId);
        public bool HasIronWill(Survivor sv) => HasTrait(sv, IronWillId);
        public bool HasUnseenListener(Survivor sv) => HasTrait(sv, UnseenListenerId);
        public bool HasRuthlessCapitalist(Survivor sv) => HasTrait(sv, RuthlessCapitalistId);
        public bool HasProdigy(Survivor sv) => HasTrait(sv, ProdigyId);
        public bool HasCommander(Survivor sv) => HasTrait(sv, CommanderId);
        public bool HasCyberArm(Survivor sv) => HasTrait(sv, CyberArmId);
        public bool HasRedemption(Survivor sv) => HasTrait(sv, RedemptionId);
        public bool HasOverclocked(Survivor sv) => HasTrait(sv, OverclockedId);
        public bool HasWastelandGuardian(Survivor sv) => HasTrait(sv, WastelandGuardianId);
        public bool HasOmniscience(Survivor sv) => HasTrait(sv, OmniscienceId);

        // Base traits
        public bool HasSelfless(Survivor sv) => HasBaseTrait(sv, SelflessId);
        public bool HasWorkaholic(Survivor sv) => HasBaseTrait(sv, WorkaholicId);
        public bool HasDependent(Survivor sv) => HasBaseTrait(sv, DependentId);
        public bool HasPollyanna(Survivor sv) => HasBaseTrait(sv, PollyannaId);
        public bool HasTraumatized(Survivor sv) => HasBaseTrait(sv, TraumatizedId);
        public bool HasSociopath(Survivor sv) =>
            HasBaseTrait(sv, SociopathId) || HasChildOfTheAsh(sv);
        public bool HasArrogant(Survivor sv) => HasBaseTrait(sv, ArrogantId);
        public bool HasDeceptive(Survivor sv) => HasBaseTrait(sv, DeceptiveId);
        public bool HasSelfish(Survivor sv) => HasBaseTrait(sv, SelfishId);
        public bool HasTactician(Survivor sv) => HasBaseTrait(sv, TacticianId);
        public bool HasHated(Survivor sv) => HasBaseTrait(sv, HatedId);
        public bool HasAntiAuthority(Survivor sv) => HasBaseTrait(sv, AntiAuthorityId);
        public bool HasCoward(Survivor sv) => HasBaseTrait(sv, CowardId);
        public bool HasStrict(Survivor sv) => HasBaseTrait(sv, StrictId);
        public bool HasStunted(Survivor sv) =>
            HasBaseTrait(sv, StuntedId) && !HasReclaimedYouth(sv);
        public bool HasHyperEmpathetic(Survivor sv) => HasBaseTrait(sv, HyperEmpatheticId);
        public bool HasRude(Survivor sv) => HasBaseTrait(sv, RudeId);
        public bool HasDenialist(Survivor sv) => HasBaseTrait(sv, DenialistId);
        public bool HasSacrificial(Survivor sv) => HasBaseTrait(sv, SacrificialId);
        public bool HasGodComplex(Survivor sv) =>
            HasBaseTrait(sv, GodComplexId) && !HasHumbledHealer(sv);
        // #267–#276 base
        public bool HasRestless(Survivor sv) => HasBaseTrait(sv, RestlessId) && !HasTheWatcher(sv);
        public bool HasParanoidHealth(Survivor sv) => HasBaseTrait(sv, ParanoidHealthId) && !HasHyperAware(sv);
        public bool HasFascination(Survivor sv) => HasBaseTrait(sv, FascinationId);
        public bool HasBlind(Survivor sv) => HasBaseTrait(sv, BlindId);
        public bool HasParanoid(Survivor sv) => HasBaseTrait(sv, ParanoidId);
        public bool HasAnimalistic(Survivor sv) => HasBaseTrait(sv, AnimalisticId) && !HasApexScavenger(sv);
        public bool HasVowOfNonviolence(Survivor sv) => HasBaseTrait(sv, VowOfNonviolenceId);
        public bool HasGrieving(Survivor sv) => HasBaseTrait(sv, GrievingId) && !HasMasterGeneticist(sv);
        // #277–#283 base
        public bool HasDistrusted(Survivor sv) => HasBaseTrait(sv, DistrustedId) && !HasTheEnforcer(sv);
        public bool HasMoralCompass(Survivor sv) => HasBaseTrait(sv, MoralCompassId);
        public bool HasFailingHeart(Survivor sv) => HasBaseTrait(sv, FailingHeartId) && !HasLegendOfTheWastes(sv);
        public bool HasSilverTongue(Survivor sv) => HasBaseTrait(sv, SilverTongueId);
        public bool HasDelusional(Survivor sv) => HasBaseTrait(sv, DelusionalId) && !HasCybernetics(sv);
        public bool HasPhotogenic(Survivor sv) => HasBaseTrait(sv, PhotogenicId);
        public bool HasAgoraphile(Survivor sv) => HasBaseTrait(sv, AgoraphileId) && !HasMasterPathologist(sv);
        public bool HasRuthless(Survivor sv) => HasBaseTrait(sv, RuthlessId) && !HasMonopolist(sv);
        // #284–#298 base (cleared when latent replaces the burden where noted)
        public bool HasBlackLung(Survivor sv) => HasBaseTrait(sv, BlackLungId) && !HasDeepDelver(sv);
        public bool HasClaustrophilic(Survivor sv) => HasBaseTrait(sv, ClaustrophilicId);
        public bool HasCaffeinated(Survivor sv) => HasBaseTrait(sv, CaffeinatedId) && !HasLogisticsMaster(sv);
        public bool HasCalloused(Survivor sv) => HasBaseTrait(sv, CallousedId);
        public bool HasDeafInOneEar(Survivor sv) => HasBaseTrait(sv, DeafInOneEarId) && !HasForgeMaster(sv);
        public bool HasInvisible(Survivor sv) => HasBaseTrait(sv, InvisibleId);
        public bool HasNeatFreak(Survivor sv) => HasBaseTrait(sv, NeatFreakId) && !HasSanitizationExpert(sv);
        public bool HasBrawn(Survivor sv) => HasBaseTrait(sv, BrawnId);
        public bool HasGermaphobe(Survivor sv) => HasBaseTrait(sv, GermaphobeId) && !HasEpidemiologist(sv);
        public bool HasNightOwl(Survivor sv) => HasBaseTrait(sv, NightOwlId);
        public bool HasQuiet(Survivor sv) => HasBaseTrait(sv, QuietId);
        public bool HasFrail(Survivor sv) => HasBaseTrait(sv, FrailId) && !HasArchivist(sv);
        public bool HasPennyPincher(Survivor sv) => HasBaseTrait(sv, PennyPincherId) && !HasAuditor(sv);
        public bool HasFragileEgo(Survivor sv) => HasBaseTrait(sv, FragileEgoId) && !HasMaestro(sv);
        public bool HasShady(Survivor sv) => HasBaseTrait(sv, ShadyId) && !HasBlockadeRunner(sv);
        public bool HasProfessional(Survivor sv) => HasBaseTrait(sv, ProfessionalId);
        public bool HasSlightOfHand(Survivor sv) => HasBaseTrait(sv, SlightOfHandId);
        public bool HasCounterfeiter(Survivor sv) => HasBaseTrait(sv, CounterfeiterId);
        public bool HasAntsy(Survivor sv) => HasBaseTrait(sv, AntsyId) && !HasMechanicProdigy(sv);
        // #299–#318 base (cleared when latent replaces the burden where noted)
        public bool HasSpoiled(Survivor sv) => HasBaseTrait(sv, SpoiledId) && !HasDiplomat(sv);
        public bool HasHighMetabolism(Survivor sv) => HasBaseTrait(sv, HighMetabolismId);
        public bool HasTextbookKnowledge(Survivor sv) => HasBaseTrait(sv, TextbookKnowledgeId) && !HasChiefOfMedicine(sv);
        public bool HasAgoraphobic(Survivor sv) => HasBaseTrait(sv, AgoraphobicId) && !HasDroneOperator(sv);
        public bool HasInnocent(Survivor sv) => HasBaseTrait(sv, InnocentId) && !HasChoirOfOne(sv);
        public bool HasTinfoilHat(Survivor sv) => HasBaseTrait(sv, TinfoilHatId) && !HasTruthSeeker(sv);
        public bool HasUncivilized(Survivor sv) => HasBaseTrait(sv, UncivilizedId) && !HasWildman(sv);
        public bool HasComatoseBurden(Survivor sv) => HasBaseTrait(sv, ComatoseBurdenId) && !HasSecondLife(sv);
        public bool HasBranded(Survivor sv) => HasBaseTrait(sv, BrandedId) && !HasIronWill(sv);
        public bool HasUndocumented(Survivor sv) => HasBaseTrait(sv, UndocumentedId) && !HasUnseenListener(sv);
        public bool HasEntitled(Survivor sv) => HasBaseTrait(sv, EntitledId) && !HasRuthlessCapitalist(sv);
        public bool HasClumsy(Survivor sv) => HasBaseTrait(sv, ClumsyId) && !HasProdigy(sv);
        public bool HasBurnScars(Survivor sv) => HasBaseTrait(sv, BurnScarsId);
        public bool HasMissingArm(Survivor sv) =>
            (HasBaseTrait(sv, MissingArmId) || (sv != null && sv.HasDisability(MissingArmDisabilityId)))
            && !HasCyberArm(sv);
        public bool HasLethal(Survivor sv) => HasBaseTrait(sv, LethalId);
        public bool HasUntrusted(Survivor sv) => HasBaseTrait(sv, UntrustedId) && !HasRedemption(sv);
        public bool HasAndroid(Survivor sv) => HasBaseTrait(sv, AndroidId);
        public bool HasGoodBoy(Survivor sv) => HasBaseTrait(sv, GoodBoyId);
        public bool HasBunkerCore(Survivor sv) => HasBaseTrait(sv, BunkerCoreId);

        public static bool HasBaseTrait(Survivor sv, string traitId)
        {
            if (sv == null || string.IsNullOrEmpty(traitId)) return false;
            return sv.HasTrait(traitId);
        }

        /// <summary>#215 — surgery duration mult (0.5 with Miracle Worker).</summary>
        public float GetSurgeryDurationMultiplier(Survivor medic) =>
            HasMiracleWorker(medic) ? MiracleWorkerSurgeryDurationMult : 1f;

        /// <summary>#215 — surgical tools take 0 durability / are not consumed.</summary>
        public bool ConsumesSurgicalTools(Survivor medic) => !HasMiracleWorker(medic);

        /// <summary>#215 — can cure Acute Radiation Syndrome without chelation.</summary>
        public bool CanCureArsWithoutChelation(Survivor medic) => HasMiracleWorker(medic);

        /// <summary>#216 — chance (0..1) to double med craft yield.</summary>
        public float GetAlchemistDoubleYieldChance(Survivor crafter) =>
            HasAlchemist(crafter) ? AlchemistDoubleYieldChance : 0f;

        /// <summary>
        /// Roll double yield for medical crafts. Returns final amount.
        /// </summary>
        public int ApplyAlchemistYield(Survivor crafter, int baseAmount, System.Random rng = null)
        {
            if (baseAmount <= 0 || !HasAlchemist(crafter)) return baseAmount;
            rng ??= AtomicWar._Game.Utilities.SeededRandom.CreateFixed("personalquestsystem");
            if (rng.NextDouble() < AlchemistDoubleYieldChance)
                return baseAmount * 2;
            return baseAmount;
        }

        /// <summary>#216 — mold + dirty water → antibiotics craft unlocked.</summary>
        public bool CanCraftAntibioticsFromMold(Survivor crafter) => HasAlchemist(crafter);

        /// <summary>#217 — max wasteland animals this survivor may tame.</summary>
        public int GetMaxTamedAnimals(Survivor vet) =>
            HasZoonoticExpert(vet) ? ZoonoticMaxTamedAnimals : 0;

        /// <summary>#217 — tamed animals eat spoiled meat only (not standard rations).</summary>
        public bool PetsEatSpoiledMeatOnly(Survivor owner) => HasZoonoticExpert(owner);

        /// <summary>#218 — Anchor locks own morale at 100.</summary>
        public void ApplyAnchorMoraleLock(Survivor sv)
        {
            if (sv != null && HasAnchor(sv))
                sv.Needs.Morale = 100f;
        }

        /// <summary>
        /// #218 — floor morale for any survivor sharing a room with an Anchor.
        /// Returns the floor to enforce (20), or 0 if no Anchor present.
        /// </summary>
        public float GetRoomMoraleFloor(string roomId, IReadOnlyList<Survivor> survivors)
        {
            if (string.IsNullOrEmpty(roomId) || survivors == null) return 0f;
            for (int i = 0; i < survivors.Count; i++)
            {
                var a = survivors[i];
                if (a == null || !a.IsAlive || !HasAnchor(a)) continue;
                if (string.Equals(a.CurrentRoomId, roomId, StringComparison.Ordinal))
                    return AnchorRoomMoraleFloor;
            }
            return 0f;
        }

        /// <summary>Clamp survivor morale to Anchor room floor when applicable.</summary>
        public void ApplyRoomMoraleFloor(Survivor sv, IReadOnlyList<Survivor> survivors)
        {
            if (sv == null || !sv.IsAlive) return;
            // Anchor self is locked at 100.
            if (HasAnchor(sv))
            {
                sv.Needs.Morale = 100f;
                return;
            }
            float floor = GetRoomMoraleFloor(sv.CurrentRoomId, survivors);
            if (floor > 0f && sv.Needs.Morale < floor)
                sv.Needs.Morale = floor;
        }

        /// <summary>#219 / #253 — zero morale penalty from corpses / murder / butchering.</summary>
        public bool IsImmuneToDeathMorale(Survivor sv) =>
            HasDeathBlind(sv) || HasSociopath(sv);

        /// <summary>#219 — morale regen per hour while sleeping near debris.</summary>
        public float GetDebrisSleepMoraleRegen(Survivor sv, bool nearDebris) =>
            HasDeathBlind(sv) && nearDebris ? DeathBlindDebrisSleepMoralePerHour : 0f;

        // ── #220 Warlord trait queries ───────────────────────────────────

        public static bool IsPipeWeaponId(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return false;
            return string.Equals(itemId, PipeWeaponId, StringComparison.OrdinalIgnoreCase)
                   || string.Equals(itemId, PipeShotgunId, StringComparison.OrdinalIgnoreCase)
                   || itemId.StartsWith("pipe_", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>#220 — pipe weapons deal assault-rifle power when Warlord owns them.</summary>
        public float GetWeaponPowerOverride(Survivor wielder, string weaponId, float basePower)
        {
            if (!HasWarlord(wielder) || !IsPipeWeaponId(weaponId)) return basePower;
            return AssaultRifleWeaponPower;
        }

        /// <summary>
        /// #220 — unarmed Warlord hatch defense power vs Level 3 raids.
        /// Returns bonus defense when a living Warlord is among active guards.
        /// </summary>
        public float GetWarlordUnarmedDefenseBonus(
            IReadOnlyList<Survivor> guards,
            bool weaponsPresent)
        {
            if (weaponsPresent || guards == null) return 0f;
            for (int i = 0; i < guards.Count; i++)
            {
                if (guards[i] != null && guards[i].IsAlive && HasWarlord(guards[i]))
                    return WarlordUnarmedDefensePower;
            }
            return 0f;
        }

        public bool CanDefendLevel3Unarmed(Survivor sv) => HasWarlord(sv);

        // ── #221 Peacekeeper trait queries ───────────────────────────────

        /// <summary>#221 — Warning Shot: 100% safe flee during encounters.</summary>
        public bool CanUseWarningShot(Survivor sv) => HasPeacekeeper(sv);

        /// <summary>#221 — blocks Internal Saboteur / Ration Thief bunker events.</summary>
        public bool BlocksInternalCrimeEvent(string eventId, IReadOnlyList<Survivor> survivors)
        {
            if (string.IsNullOrEmpty(eventId) || survivors == null) return false;
            if (!IsInternalCrimeEventId(eventId)) return false;
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s != null && s.IsAlive && HasPeacekeeper(s))
                    return true;
            }
            return false;
        }

        public static bool IsInternalCrimeEventId(string eventId)
        {
            if (string.IsNullOrEmpty(eventId)) return false;
            return string.Equals(eventId, InternalSaboteurEventId, StringComparison.OrdinalIgnoreCase)
                   || string.Equals(eventId, RationThiefEventId, StringComparison.OrdinalIgnoreCase)
                   || string.Equals(eventId, RationThiefAgainEventId, StringComparison.OrdinalIgnoreCase)
                   || string.Equals(eventId, "missing_rations_caught", StringComparison.OrdinalIgnoreCase);
        }

        // ── #222 Juggernaut trait queries ────────────────────────────────

        /// <summary>#222 — immune to Broken Bone and Laceration.</summary>
        public bool IsImmuneToTraumaAffliction(Survivor sv, string afflictionId)
        {
            if (!HasJuggernaut(sv) || string.IsNullOrEmpty(afflictionId)) return false;
            return string.Equals(afflictionId, "broken_bone", StringComparison.OrdinalIgnoreCase)
                   || string.Equals(afflictionId, "laceration", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>#222 — encumbrance weight limits removed on expeditions.</summary>
        public bool IgnoresEncumbrance(Survivor sv) => HasJuggernaut(sv);

        /// <summary>#222 — effective carry capacity (unlimited sentinel when Juggernaut).
        /// #251 Dependent children cap at 10kg.</summary>
        public float GetExpeditionCarryCapacity(Survivor sv, float baseCapacity)
        {
            if (HasJuggernaut(sv)) return 99999f;
            if (HasDependent(sv)) return DependentCarryKg;
            return baseCapacity;
        }

        // ── #223 Apex Predator trait queries ─────────────────────────────

        /// <summary>#223 — stealth permanently 100% (1.0 factor / full stealth).</summary>
        public float GetStealthFactor(Survivor sv) =>
            HasApexPredator(sv) ? ApexPredatorStealth : -1f; // -1 = no override

        /// <summary>#223 — guaranteed meat on forest/swamp scavenge.</summary>
        public int GetApexPredatorMeatYield(Survivor sv, bool isForestOrSwamp) =>
            HasApexPredator(sv) && isForestOrSwamp ? ApexPredatorMeatYield : 0;

        // ── #224 Survivalist trait queries ───────────────────────────────

        /// <summary>#224 — raw ContaminatedFood without sickness.</summary>
        public bool CanEatContaminatedWithoutSickness(Survivor sv) => HasSurvivalist(sv);

        /// <summary>#224 — stamina drain mult when alone on the map (0.25 = 75% reduced).</summary>
        public float GetAloneStaminaDrainMultiplier(Survivor sv, bool isAloneOnMap)
        {
            if (!HasSurvivalist(sv) || !isAloneOnMap) return 1f;
            return SurvivalistAloneStaminaMult;
        }


        // ── #225 Hydraulic Master ────────────────────────────────────────

        public float GetPurifierSpeedMultiplier(IReadOnlyList<Survivor> survivors)
        {
            if (AnyLivingWithTrait(survivors, HydraulicMasterId))
                return HydraulicPurifierSpeedMult;
            return 1f;
        }

        public bool CanExtractWaterFromHumidity(Survivor sv) => HasHydraulicMaster(sv);

        public bool AnyHydraulicMaster(IReadOnlyList<Survivor> survivors) =>
            AnyLivingWithTrait(survivors, HydraulicMasterId);

        // ── #226 Grid Walker ─────────────────────────────────────────────

        public bool GeneratorsImmuneToBreakdown(IReadOnlyList<Survivor> survivors) =>
            AnyLivingWithTrait(survivors, GridWalkerId);

        public float GetPowerCapacityMultiplier(IReadOnlyList<Survivor> survivors) =>
            AnyLivingWithTrait(survivors, GridWalkerId) ? GridWalkerPowerCapacityMult : 1f;

        public bool CanHotwireBunkerPower(Survivor sv) => HasGridWalker(sv);

        // ── #227 Vault Builder ───────────────────────────────────────────

        public float GetRoomBuildCostMultiplier(Survivor builder) =>
            HasVaultBuilder(builder) ? VaultBuilderBuildCostMult : 1f;

        public bool LocksStructuralIntegrityAtMax(IReadOnlyList<Survivor> survivors) =>
            AnyLivingWithTrait(survivors, VaultBuilderId);

        // ── #228 Grease Monkey ───────────────────────────────────────────

        public float GetVehicleEscapeCostMultiplier(IReadOnlyList<Survivor> survivors) =>
            AnyLivingWithTrait(survivors, GreaseMonkeyId) ? GreaseMonkeyVehicleCostMult : 1f;

        public bool UnlocksVehicleEscape(IReadOnlyList<Survivor> survivors) =>
            AnyLivingWithTrait(survivors, GreaseMonkeyId);

        public bool BicyclesNeverDegrade(IReadOnlyList<Survivor> survivors) =>
            AnyLivingWithTrait(survivors, GreaseMonkeyId);

        public bool BicyclesNeverDegrade(Survivor sv) => HasGreaseMonkey(sv);

        // ── #229 Synthesizer ─────────────────────────────────────────────

        public bool CanCraftAntiRadFromChemicalScrap(Survivor sv) => HasSynthesizer(sv);

        public float GetRadAwayEfficiencyMultiplier(IReadOnlyList<Survivor> survivors) =>
            AnyLivingWithTrait(survivors, SynthesizerId) ? SynthesizerRadAwayMult : 1f;

        // ── #230 Gaia ────────────────────────────────────────────────────

        public int GetCropYieldMultiplier(Survivor botanist) =>
            HasGaia(botanist) ? GaiaCropYieldMult : 1;

        public bool CropsImmuneToMold(IReadOnlyList<Survivor> survivors) =>
            AnyLivingWithTrait(survivors, GaiaId);

        public bool CanGrowMedicinalHerbs(Survivor sv) => HasGaia(sv);

        // ── #231 Wasteland Runner ────────────────────────────────────────

        public float GetExpeditionTravelTimeMultiplier(Survivor sv) =>
            HasWastelandRunner(sv) ? WastelandRunnerTravelMult : 1f;

        public bool IgnoresWeatherMovementPenalty(Survivor sv) => HasWastelandRunner(sv);

        // ── #232 Ghost ───────────────────────────────────────────────────

        public bool ForcesZeroHatchVisibilityWhenOutside(Survivor sv) => HasGhost(sv);

        public bool BypassesLocksAndSafes(Survivor sv) => HasGhost(sv);

        public bool AnyGhostOutside(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return false;
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s == null || !s.IsAlive || !HasGhost(s)) continue;
                if (s.State == SurvivorState.Working || s.IsOnExpedition)
                    return true;
            }
            return false;
        }

        // ── #233 Stormcaller ─────────────────────────────────────────────

        public bool HasPerfectTenDayForecast(IReadOnlyList<Survivor> survivors) =>
            AnyLivingWithTrait(survivors, StormcallerId);

        public float GetStormMoraleBuff(Survivor sv, bool outsideDuringStorm) =>
            HasStormcaller(sv) && outsideDuringStorm ? StormcallerStormMoraleBuff : 0f;

        // ── #234 Rad-Walker ──────────────────────────────────────────────

        public float GetRadiationAbsorbFactor(Survivor sv) =>
            HasRadWalker(sv) ? RadWalkerAbsorbCap : 1f;

        public bool SkipsDeconOnReturn(Survivor sv) => HasRadWalker(sv);

        // ── #235 Polymath ────────────────────────────────────────────────

        public bool UnlocksSkillMentorshipForAllSkills(Survivor sv) => HasPolymath(sv);

        public float GetActionPerkXpMultiplier(Survivor sv) =>
            HasPolymath(sv) ? PolymathPerkXpMult : 1f;

        // ── #236 Demagogue ───────────────────────────────────────────────

        public float GetFactionTrustFloor(IReadOnlyList<Survivor> survivors) =>
            AnyLivingWithTrait(survivors, DemagogueId) ? DemagogueTrustFloor : float.NegativeInfinity;

        public float ClampFactionTrust(float trust, IReadOnlyList<Survivor> survivors)
        {
            float floor = GetFactionTrustFloor(survivors);
            if (float.IsNegativeInfinity(floor)) return trust;
            return Mathf.Max(trust, floor);
        }

        public bool FactionsDropTribute(IReadOnlyList<Survivor> survivors) =>
            AnyLivingWithTrait(survivors, DemagogueId);

        // ── #237 Shepherd ────────────────────────────────────────────────

        public bool CanPerformSermon(Survivor sv) => HasShepherd(sv);

        // ── #238 Muckraker ───────────────────────────────────────────────

        public bool RevealsAllMapFog(IReadOnlyList<Survivor> survivors) =>
            AnyLivingWithTrait(survivors, MuckrakerId);

        public bool RevealsAllMapFog(Survivor sv) => HasMuckraker(sv);

        // ── #239 Voice of the Wastes ─────────────────────────────────────

        public bool RadioPowerIsFree(IReadOnlyList<Survivor> survivors) =>
            AnyLivingWithTrait(survivors, VoiceOfTheWastesId);

        public bool RadioIntelIsInstant(Survivor sv) => HasVoiceOfTheWastes(sv);

        public bool BlocksTrapIntel(Survivor sv) => HasVoiceOfTheWastes(sv);

        // ── #240 Iron Chef ───────────────────────────────────────────────

        public bool MealsFullyRestoreNeeds(Survivor cook) => HasIronChef(cook);

        public bool MealsCurePhase1Afflictions(Survivor cook) => HasIronChef(cook);

        // ── #241 Tireless ────────────────────────────────────────────────

        public float GetStaminaPoolMultiplier(Survivor sv) =>
            HasTireless(sv) ? TirelessPoolMult : 1f;

        public float GetFatiguePoolMultiplier(Survivor sv) =>
            HasTireless(sv) ? TirelessPoolMult : 1f;

        public float GetDailySleepHoursRequired(Survivor sv) =>
            HasTireless(sv) ? TirelessSleepHoursPerDay : 8f;

        // ── #242 Asbestos ────────────────────────────────────────────────

        /// <summary>
        /// #243 Asbestos, #286 Calloused, or #313 Fire Chief (Burn Scars/Commander):
        /// immune to fire/temperature damage.
        /// </summary>
        public bool IsImmuneToFireAndTemperature(Survivor sv) =>
            HasAsbestos(sv) || HasCalloused(sv) || IsImmuneToSuperficialFireDamage(sv);

        public bool IgnoresColdSleepQuality(Survivor sv) => HasAsbestos(sv);

        // ── #243 Armorer ─────────────────────────────────────────────────

        public bool CanCraftReinforcedHazmatSuits(Survivor sv) => HasArmorer(sv);

        public float GetClothingDegradeMultiplier(IReadOnlyList<Survivor> survivors) =>
            AnyLivingWithTrait(survivors, ArmorerId) ? ArmorerClothingDegradeMult : 1f;

        // ── #244 Tinkerer ────────────────────────────────────────────────

        public bool DevicesNeverLoseCalibration(IReadOnlyList<Survivor> survivors) =>
            AnyLivingWithTrait(survivors, TinkererId);

        public bool ShowsTrueRadiation(IReadOnlyList<Survivor> survivors) =>
            AnyLivingWithTrait(survivors, TinkererId);

        // ── #245 Lorekeeper ──────────────────────────────────────────────

        public float GetJournalMoraleBoost(IReadOnlyList<Survivor> survivors) =>
            AnyLivingWithTrait(survivors, LorekeeperId) ? LorekeeperJournalMoraleBoost : 0f;

        public float GetArtifactTradeValueMultiplier(IReadOnlyList<Survivor> survivors) =>
            AnyLivingWithTrait(survivors, LorekeeperId) ? LorekeeperArtifactTradeMult : 1f;

        // ── #246 Zealot's Bane ───────────────────────────────────────────

        public bool CultistsFleeFrom(Survivor sv) => HasZealotsBane(sv);

        public float GetFactionCombatDamageMultiplier(Survivor sv) =>
            HasZealotsBane(sv) ? ZealotsBaneCombatMult : 1f;

        // ── #247 Chem-Resistant ──────────────────────────────────────────

        public bool ImmuneToAddiction(Survivor sv) => HasChemResistant(sv) || HasCleanAndSober(sv);

        public float GetMedicalHealMultiplier(Survivor patient) =>
            HasChemResistant(patient) ? ChemResistantHealMult : 1f;

        // ── #248 Protector ───────────────────────────────────────────────

        public bool IsProtectorEnraged(Survivor parent, IReadOnlyList<Survivor> survivors)
        {
            if (!HasProtector(parent) || survivors == null) return false;
            float cap = parent.MaxHealthCap > 0f ? parent.MaxHealthCap : 100f;
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s == null || !s.IsAlive || s.Id == parent.Id) continue;
                float otherCap = s.MaxHealthCap > 0f ? s.MaxHealthCap : 100f;
                if (s.Needs.Health < otherCap * ProtectorHealthTriggerFrac)
                    return true;
            }
            return false;
        }

        public float GetProtectorActionSpeedMultiplier(Survivor parent, IReadOnlyList<Survivor> survivors) =>
            IsProtectorEnraged(parent, survivors) ? ProtectorBoostMult : 1f;

        // ── #249 Selfless / Matriarch ────────────────────────────────────

        /// <summary>
        /// #249 Selfless: absorber takes 10% of morale damage aimed at others.
        /// Returns the amount the Selfless survivor should absorb (caller applies).
        /// </summary>
        public float GetSelflessMoraleAbsorb(Survivor absorber, float moraleDamageToOther)
        {
            if (!HasSelfless(absorber) || moraleDamageToOther <= 0f) return 0f;
            return moraleDamageToOther * SelflessMoraleAbsorbFrac;
        }

        /// <summary>
        /// Apply morale damage with Selfless redistribution. Returns final damage
        /// applied to <paramref name="target"/> after Selfless absorbers take 10%.
        /// </summary>
        public float ApplyMoraleDamageWithSelfless(
            Survivor target,
            float damage,
            IReadOnlyList<Survivor> survivors)
        {
            if (target == null || damage <= 0f) return 0f;
            float remaining = damage;
            if (survivors != null)
            {
                for (int i = 0; i < survivors.Count; i++)
                {
                    var s = survivors[i];
                    if (s == null || !s.IsAlive || s.Id == target.Id) continue;
                    float absorb = GetSelflessMoraleAbsorb(s, remaining);
                    if (absorb <= 0f) continue;
                    s.Needs.Morale = Mathf.Max(0f, s.Needs.Morale - absorb);
                    remaining -= absorb;
                }
            }
            target.Needs.Morale = Mathf.Max(0f, target.Needs.Morale - remaining);
            return remaining;
        }

        /// <summary>
        /// #249 AI quirk: cancel Eat/Sleep if any living child has a need below 20%.
        /// </summary>
        public bool ShouldCancelEatOrSleepForChild(Survivor mother, IReadOnlyList<Survivor> survivors)
        {
            if (mother == null || !HasSelfless(mother)) return false;
            if (survivors == null) return false;
            for (int i = 0; i < survivors.Count; i++)
            {
                var c = survivors[i];
                if (c == null || !c.IsAlive || !c.IsChild || c.Id == mother.Id) continue;
                if (c.Needs.Hunger < ChildNeedsCancelThreshold
                    || c.Needs.Thirst < ChildNeedsCancelThreshold
                    || c.Needs.Fatigue > (100f - ChildNeedsCancelThreshold)
                    || c.Needs.Health < ChildNeedsCancelThreshold
                    || c.Needs.Warmth < ChildNeedsCancelThreshold
                    || c.Needs.Morale < ChildNeedsCancelThreshold)
                    return true;
            }
            return false;
        }

        /// <summary>#249 Matriarch: +20 max health for others sharing her room.</summary>
        public float GetMatriarchRoomHealthBonus(Survivor subject, IReadOnlyList<Survivor> survivors)
        {
            if (subject == null || survivors == null) return 0f;
            if (HasMatriarch(subject)) return 0f; // bonus is for others
            string room = subject.CurrentRoomId;
            for (int i = 0; i < survivors.Count; i++)
            {
                var m = survivors[i];
                if (m == null || !m.IsAlive || !HasMatriarch(m)) continue;
                if (string.Equals(m.CurrentRoomId, room, StringComparison.Ordinal)
                    || (string.IsNullOrEmpty(room) && string.IsNullOrEmpty(m.CurrentRoomId)))
                    return MatriarchRoomHealthBonus;
            }
            return 0f;
        }

        /// <summary>#249 Matriarch cannot suffer mental breaks while another survivor is alive.</summary>
        public bool CanSufferMentalBreak(Survivor sv, IReadOnlyList<Survivor> survivors)
        {
            if (sv == null) return false;
            if (HasPollyanna(sv) && string.Equals(sv.currentMentalBreakId, DespairBreakId, StringComparison.OrdinalIgnoreCase))
                return false;
            if (!HasMatriarch(sv)) return true;
            if (survivors == null) return true;
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s != null && s.IsAlive && s.Id != sv.Id)
                    return false;
            }
            return true;
        }

        /// <summary>Block Despair breaks for Pollyanna (#251).</summary>
        public bool IsImmuneToDespairBreak(Survivor sv) => HasPollyanna(sv);

        /// <summary>Block any mental break for Matriarch when others live.</summary>
        public bool BlocksMentalBreak(Survivor sv, string breakId, IReadOnlyList<Survivor> survivors)
        {
            if (IsImmuneToDespairBreak(sv)
                && string.Equals(breakId, DespairBreakId, StringComparison.OrdinalIgnoreCase))
                return true;
            if (HasMatriarch(sv) && !CanSufferMentalBreak(sv, survivors))
                return true;
            return false;
        }

        // ── #250 Workaholic / Pillar of Atlas ────────────────────────────

        public float GetCraftRepairFatigueDrainMultiplier(Survivor sv) =>
            HasWorkaholic(sv) ? WorkaholicCraftFatigueDrainMult : 1f;

        public float GetSleepFatigueRestoreMultiplier(Survivor sv)
        {
            // #268 Restless: sleep restores only 20% fatigue per cycle.
            if (HasRestless(sv)) return RestlessSleepRestoreFrac;
            // #285 Caffeinated: sleeps 50% less (half fatigue restore).
            if (HasCaffeinated(sv)) return CaffeinatedSleepRestoreMult;
            // #250 Workaholic: half restore.
            if (HasWorkaholic(sv)) return WorkaholicSleepRestoreMult;
            return 1f;
        }

        /// <summary>#291 Frail: max health capped at 60 until Archivist unlocks.</summary>
        public float GetMaxHealthCapForQuests(Survivor sv)
        {
            if (HasFrail(sv)) return FrailMaxHealthCap;
            return sv != null ? sv.MaxHealthCap : 100f;
        }

        /// <summary>#284 Black Lung: max stamina permanently −20%.</summary>
        public float GetEffectiveMaxStamina(Survivor sv)
        {
            float baseStam = sv != null ? sv.MaxStaminaCap : 100f;
            return baseStam * GetBlackLungStaminaMaxMultiplier(sv);
        }

        /// <summary>#250 AI quirk: ignore Rest until fatigue ≥ 95%.</summary>
        public bool ShouldIgnoreRestAction(Survivor sv) =>
            HasWorkaholic(sv) && sv.Needs.Fatigue < WorkaholicRestIgnoreFatigue;

        /// <summary>#250 Pillar: no fatigue penalties to action speed.</summary>
        public bool IgnoresFatigueActionSpeedPenalty(Survivor sv) => HasPillarOfAtlas(sv);

        public float GetFatigueActionSpeedMultiplier(Survivor sv, float baseMultFromFatigue)
        {
            if (HasPillarOfAtlas(sv)) return 1f;
            return baseMultFromFatigue;
        }

        /// <summary>
        /// Call when any survivor dies. If they had Pillar of Atlas, permanent
        /// 20% repair speed debuff is applied to the shelter.
        /// </summary>
        public void NotifySurvivorDied(Survivor sv)
        {
            if (sv == null) return;
            if (HasPillarOfAtlas(sv) || (sv.LatentTraitUnlocked
                && string.Equals(sv.LatentExpertTraitId, PillarOfAtlasId, StringComparison.Ordinal)))
            {
                PillarOfAtlasDeathDebuffActive = true;
            }
            if (HasLivingSaint(sv) || (sv.LatentTraitUnlocked
                && string.Equals(sv.LatentExpertTraitId, LivingSaintId, StringComparison.Ordinal)))
            {
                LivingSaintInspiredActive = true;
            }
        }

        public float GetShelterRepairSpeedMultiplier() =>
            PillarOfAtlasDeathDebuffActive ? PillarDeathRepairSpeedMult : 1f;

        // ── #251 Dependent / Wasteland Scout / Pollyanna ──────────────────

        public bool CanEquipFirearms(Survivor sv) => !HasDependent(sv);

        /// <summary>#251 AI: interacting with Naive Son grants Hope to adults.</summary>
        public float GetChildInteractionHopeBuff(Survivor child, Survivor adult)
        {
            if (child == null || adult == null || !adult.IsAlive) return 0f;
            if (adult.IsChild) return 0f;
            if (string.Equals(child.ArchetypeId, NaiveSonId, StringComparison.Ordinal)
                || (child.IsChild && HasPollyanna(child)))
                return NaiveSonHopeBuff;
            return 0f;
        }

        public void ApplyChildInteractionHope(Survivor child, Survivor adult)
        {
            float buff = GetChildInteractionHopeBuff(child, adult);
            if (buff > 0f)
                adult.Needs.Morale = Mathf.Min(100f, adult.Needs.Morale + buff);
        }

        /// <summary>#251 Wasteland Scout: immune to Sniper encounters.</summary>
        public bool IsImmuneToSniperEncounters(Survivor sv) => HasWastelandScout(sv);

        /// <summary>#251 Wasteland Scout: crawl through Debris instantly for rare loot.</summary>
        public bool CanCrawlDebrisInstantly(Survivor sv) => HasWastelandScout(sv);

        // ── #252 Traumatized / Child of the Ash ──────────────────────────

        public float GetMaxMoraleCap(Survivor sv)
        {
            if (HasTraumatized(sv)) return TraumatizedMoraleCap;
            return 100f;
        }

        public void ClampMoraleToCap(Survivor sv)
        {
            if (sv == null) return;
            float cap = GetMaxMoraleCap(sv);
            if (sv.Needs.Morale > cap)
                sv.Needs.Morale = cap;
        }

        /// <summary>
        /// QUEST-002 hardened: write Morale on a survivor and immediately clamp to
        /// the trait-derived cap (Traumatized = 50%, etc.). Replaces the
        /// pattern <c>sv.Needs.Morale = Mathf.Max(0f, sv.Needs.Morale - x);</c>
        /// which bypasses the cap and lets a Traumatized survivor drop below 50%.
        /// </summary>
        public void ApplyMoraleDelta(Survivor sv, float delta)
        {
            if (sv == null) return;
            sv.Needs.Morale = Mathf.Clamp(sv.Needs.Morale + delta, 0f, 100f);
            ClampMoraleToCap(sv);
        }

        /// <summary>#252 AI: refuses Play / Comfort actions.</summary>
        public bool RefusesPlayOrComfort(Survivor sv) => HasTraumatized(sv);

        /// <summary>#252 AI: favor Train / Guard utility scores.</summary>
        public float GetTrainGuardUtilityBias(Survivor sv) =>
            HasTraumatized(sv) ? 2f : 1f;

        public bool IsImmuneToRadiationAnxiety(Survivor sv) => HasChildOfTheAsh(sv);

        /// <summary>#252 Child of the Ash: adult weapons, zero accuracy penalty.</summary>
        public float GetChildWeaponAccuracyMultiplier(Survivor sv) =>
            HasChildOfTheAsh(sv) ? 1f : (sv != null && sv.IsChild ? 0.5f : 1f);

        public bool CanEquipAdultWeapons(Survivor sv) =>
            HasChildOfTheAsh(sv) || (sv != null && !sv.IsChild && !HasDependent(sv));

        // ── #253 Sociopath / Arrogant / Cold Calculus ────────────────────

        /// <summary>#253 / #219: zero morale loss from death/murder (Sociopath or Death-Blind).</summary>
        public bool IsImmuneToDeathMoraleLoss(Survivor sv) =>
            IsImmuneToDeathMorale(sv) || HasSociopath(sv);

        /// <summary>#253 Arrogant: refuses heal from others; must self-heal.</summary>
        public bool MustSelfHeal(Survivor sv) => HasArrogant(sv);

        public bool CanBeHealedBy(Survivor patient, Survivor healer)
        {
            if (patient == null) return false;
            if (!HasArrogant(patient)) return true;
            if (healer == null) return false;
            return string.Equals(patient.Id, healer.Id, StringComparison.Ordinal);
        }

        /// <summary>#253 AI: high InterpersonalAffinity drain on everyone nearby.</summary>
        public float GetInterpersonalAffinityDrainPerHour(Survivor source) =>
            (HasArrogant(source) || HasSociopath(source)
             || string.Equals(source?.ArchetypeId, PsychopathId, StringComparison.Ordinal))
                ? PsychopathAffinityDrainPerHour
                : 0f;

        /// <summary>#253 Cold Calculus: 50% faster task execution when pop &lt; 3.</summary>
        public float GetUtilityExecutionSpeedMultiplier(Survivor sv, int livingPopulation)
        {
            if (HasColdCalculus(sv) && livingPopulation < ColdCalculusPopThreshold)
                return ColdCalculusExecSpeedMult;
            return 1f;
        }

        // ── #254 Butcher of Day 30 ───────────────────────────────────────

        public bool HasFullExpeditionStealth(Survivor sv) =>
            HasButcherOfDay30(sv) || (HasApexPredator(sv) && GetStealthFactor(sv) >= 1f);

        public float GetExpeditionStealthFactor(Survivor sv)
        {
            if (HasButcherOfDay30(sv)) return 1f;
            float apex = GetStealthFactor(sv);
            return apex >= 0f ? apex : -1f;
        }

        /// <summary>#254 Embraced Butcher: auto-clear human encounters via silent kill.</summary>
        public bool AutoClearsHumanEncounters(Survivor sv) => HasButcherOfDay30(sv);

        public bool BringsBackAssassinatedGear(Survivor sv) => HasButcherOfDay30(sv);

        // ── #255 Deceptive / Master Manipulator ──────────────────────────

        /// <summary>
        /// #255 Deceptive UI mask: randomly report Needs as 100% while starving.
        /// Returns true if UI should lie this frame.
        /// </summary>
        public bool ShouldMaskNeedsInUi(Survivor sv, System.Random rng = null)
        {
            if (!HasDeceptive(sv)) return false;
            // Only mask when actually in distress.
            if (sv.Needs.Hunger > 40f && sv.Needs.Thirst > 40f && sv.Needs.Health > 40f)
                return false;
            rng ??= new System.Random(sv.Id?.GetHashCode() ?? 0 ^ (int)sv.Needs.Hunger);
            return rng.NextDouble() < DeceptiveMaskChance;
        }

        /// <summary>Displayed need value for UI (100 when masked).</summary>
        public float GetDisplayedNeed(Survivor sv, float realValue, System.Random rng = null) =>
            ShouldMaskNeedsInUi(sv, rng) ? 100f : realValue;

        /// <summary>#255 Master Manipulator: Junk trades as high-tier Medicine price.</summary>
        public bool TradesJunkAsMedicine(Survivor trader) => HasMasterManipulator(trader);

        public float GetJunkTradeValueAsMedicine(Survivor trader, float junkBaseValue, float medicineTierValue)
        {
            if (!HasMasterManipulator(trader)) return junkBaseValue;
            return medicineTierValue;
        }

        // ── #256 Selfish / Dragon's Hoard ────────────────────────────────

        public float GetRationConsumptionMultiplier(Survivor sv) =>
            HasSelfish(sv) ? SelfishRationMult : 1f;

        public float GetSelfishMissedRationMoraleHit(Survivor sv) =>
            HasSelfish(sv) ? SelfishMissRationMoraleHit : 0f;

        /// <summary>#256 Dragon's Hoard: personal/hidden inventory never degrades.</summary>
        public bool PersonalInventoryNeverDegrades(Survivor sv) => HasDragonsHoard(sv);

        public bool ItemInPersonalStashNeverSpoils(Survivor sv, string itemId = null) =>
            HasDragonsHoard(sv);

        private bool AnyLivingWithTrait(IReadOnlyList<Survivor> survivors, string traitId)
        {
            if (survivors == null || string.IsNullOrEmpty(traitId)) return false;
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s != null && s.IsAlive && HasTrait(s, traitId))
                    return true;
            }
            return false;
        }

        public PersonalQuestState GetState(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return new PersonalQuestState();
            return GetOrCreate(survivorId).Clone();
        }

        // ── Defaults / save ──────────────────────────────────────────────

        private void EnsureDefaultQuestlines()
        {
            RegisterDefault(QuestlineSO.Ids.ShakingHand, "The Shaking Hand", MiracleWorkerId,
                maxStages: SurgeonStressOpsRequired, node: null, evt: "evt_shaking_hand");
            RegisterDefault(QuestlineSO.Ids.EmptyBottles, "The Empty Bottles", AlchemistId,
                maxStages: 1, node: RuinedCvsNodeId, evt: null);
            RegisterDefault(QuestlineSO.Ids.RabidPack, "The Rabid Pack", ZoonoticExpertId,
                maxStages: 1, node: null, evt: "evt_feral_dog_pack");
            RegisterDefault(QuestlineSO.Ids.BrokenMind, "The Broken Mind", AnchorId,
                maxStages: TherapistDeEscalationsRequired, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.MassGrave, "The Mass Grave", DeathBlindId,
                maxStages: 1, node: MassGraveNodeId, evt: null);
            RegisterDefault(QuestlineSO.Ids.GhostsOfDay1, "Ghosts of Day 1", WarlordId,
                maxStages: 1, node: FortifiedSquadNodeId, evt: SquadDistressRadioEventId);
            RegisterDefault(QuestlineSO.Ids.ThePrecinct, "The Precinct", PeacekeeperId,
                maxStages: 1, node: RuinedPrecinctNodeId, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheHoldout, "The Holdout", JuggernautId,
                maxStages: 1, node: null, evt: "evt_hatch_breach_holdout");
            RegisterDefault(QuestlineSO.Ids.TheWhiteElk, "The White Elk", ApexPredatorId,
                maxStages: WhiteElkNodesRequired, node: WhiteElkNodeId, evt: "evt_white_elk_rumor");
            RegisterDefault(QuestlineSO.Ids.TheWardensKey, "The Warden's Key", SurvivalistId,
                maxStages: 1, node: PenitentiaryNodeId, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheCityMains, "The City Mains", HydraulicMasterId,
                maxStages: 1, node: null, evt: PipeBurstEventId);
            RegisterDefault(QuestlineSO.Ids.TheSubstationGhost, "The Substation Ghost", GridWalkerId,
                maxStages: 1, node: SubstationNodeId, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheBlueprints, "The Blueprints", VaultBuilderId,
                maxStages: 1, node: TheFirmNodeId, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheMotorpool, "The Motorpool", GreaseMonkeyId,
                maxStages: 1, node: HighwayPileupNodeId, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheLabRuin, "The Lab Ruin", SynthesizerId,
                maxStages: 1, node: null, evt: ChlorineLeakEventId);
            RegisterDefault(QuestlineSO.Ids.TheSeedVault, "The Seed Vault", GaiaId,
                maxStages: SeedVaultPerfectDaysRequired, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheLostRoute, "The Lost Route", WastelandRunnerId,
                maxStages: LostRouteDeadDropsRequired, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheBankHeist, "The Bank Heist", GhostId,
                maxStages: 1, node: RuinedBankNodeId, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheRadarStation, "The Radar Station", StormcallerId,
                maxStages: 1, node: WeatherTowerNodeId, evt: null);
            RegisterDefault(QuestlineSO.Ids.GroundZero, "Ground Zero", RadWalkerId,
                maxStages: 1, node: GroundZeroCraterNodeId, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheAbandonedSchool, "The Abandoned School", PolymathId,
                maxStages: TeacherMourningDaysRequired, node: AbandonedSchoolNodeId, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheRally, "The Rally", DemagogueId,
                maxStages: PropagandaResolutionsRequired, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.CrisisOfFaith, "Crisis of Faith", ShepherdId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TruthOfDay30, "Truth of Day 30", MuckrakerId,
                maxStages: FirstStrikeIntelRequired, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.DeadAir, "Dead Air", VoiceOfTheWastesId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheFinalHarvest, "The Final Harvest", IronChefId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheMarathon, "The Marathon", TirelessId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheInferno, "The Inferno", AsbestosId,
                maxStages: 1, node: null, evt: GeneratorRoomId);
            RegisterDefault(QuestlineSO.Ids.TheKevlarLoom, "The Kevlar Loom", ArmorerId,
                maxStages: ClothingScrapsRequired, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.BrokenChronometer, "Broken Chronometer", TinkererId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.MuseumArchive, "Museum Archive", LorekeeperId,
                maxStages: 1, node: RuinedMuseumNodeId, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheCleansing, "The Cleansing", ZealotsBaneId,
                maxStages: 1, node: null, evt: "evt_cult_defector_raid");
            RegisterDefault(QuestlineSO.Ids.TheLastStash, "The Last Stash", ChemResistantId,
                maxStages: WithdrawalCleanDaysRequired, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheLocket, "The Locket", ProtectorId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheEmptyCrib, "The Empty Crib", MatriarchId,
                maxStages: 1, node: DaycareNodeId, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheBrokenPromise, "The Broken Promise", PillarOfAtlasId,
                maxStages: BrokenPromiseTier3Required, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.GrowingUpFast, "Growing Up Fast", WastelandScoutId,
                maxStages: 1, node: null, evt: "evt_raid_solo_child");
            RegisterDefault(QuestlineSO.Ids.FirstBlood, "First Blood", ChildOfTheAshId,
                maxStages: 1, node: null, evt: "evt_hatch_breach_first_blood");
            RegisterDefault(QuestlineSO.Ids.ThePerfectEquation, "The Perfect Equation", ColdCalculusId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheMaskSlips, "The Mask Slips", ButcherOfDay30Id,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheBoyWhoCriedWolf, "The Boy Who Cried Wolf", MasterManipulatorId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheWeightOfGold, "The Weight of Gold", DragonsHoardId,
                maxStages: WeightOfGoldNodesRequired, node: null, evt: null);
            // Prompts #257–#266
            RegisterDefault(QuestlineSO.Ids.CourtMartial, "Court Martial", ArtOfWarId,
                maxStages: 1, node: HitSquadNodeId, evt: "evt_hit_squad_intercept");
            RegisterDefault(QuestlineSO.Ids.TheFinalPayload, "The Final Payload", DemolitionsExpertId,
                maxStages: 1, node: MilitaryCheckpointNodeId, evt: null);
            RegisterDefault(QuestlineSO.Ids.HoldingTheLine, "Holding the Line", GhostShooterId,
                maxStages: 1, node: null, evt: "evt_raid_holding_the_line");
            RegisterDefault(QuestlineSO.Ids.InventoryAudit, "Inventory Audit", SupplyChainMasterId,
                maxStages: InventoryAuditScrapEach, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.DroppingTheRifle, "Dropping the Rifle", ReclaimedYouthId,
                maxStages: DroppingRifleDaysRequired, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheSponge, "The Sponge", SoulWeaverId,
                maxStages: SpongeCuresRequired, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.HellIsOtherPeople, "Hell is Other People", LoneWolfId,
                maxStages: HellIsOtherPeopleDaysRequired, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.ShatteredGlass, "Shattered Glass", GroundedOptimistId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheUltimatePrice, "The Ultimate Price", LivingSaintId,
                maxStages: 1, node: null, evt: "evt_ultimate_price");
            RegisterDefault(QuestlineSO.Ids.TheBotchedJob, "The Botched Job", HumbledHealerId,
                maxStages: BotchedJobDepressionDays, node: null, evt: null);
            // Prompts #267–#276
            RegisterDefault(QuestlineSO.Ids.ColdTurkey, "Cold Turkey", CleanAndSoberId,
                maxStages: ColdTurkeyDaysRequired, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheLongNight, "The Long Night", TheWatcherId,
                maxStages: LongNightGuardNightsRequired, node: null, evt: "evt_long_night_guard");
            RegisterDefault(QuestlineSO.Ids.TheRealIllness, "The Real Illness", HyperAwareId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TrialByFire, "Trial by Fire", FireBreatherId,
                maxStages: TrialByFireExtinguishRequired, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.AVoiceInTheDark, "A Voice in the Dark", SonarId,
                maxStages: VoiceInDarkConvertsRequired, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheBunkerBreached, "The Bunker Breached", ImprovisedEngineeringId,
                maxStages: 1, node: null, evt: "evt_bunker_breached");
            RegisterDefault(QuestlineSO.Ids.EmbracingTheGlow, "Embracing the Glow", RadiotrophicId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.ThePack, "The Pack", ApexScavengerId,
                maxStages: PackTrainingDaysRequired, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheUltimateTest, "The Ultimate Test", ZenStateId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheLastSeed, "The Last Seed", MasterGeneticistId,
                maxStages: 1, node: null, evt: null);
            // Prompts #277–#283
            RegisterDefault(QuestlineSO.Ids.RedemptionArc, "Redemption Arc", TheEnforcerId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheLastRide, "The Last Ride", LegendOfTheWastesId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.ARealLeader, "A Real Leader", TheStatesmanId,
                maxStages: RealLeaderDirtyDaysRequired, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheHardReboot, "The Hard Reboot", CyberneticsId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheFinalBroadcast, "The Final Broadcast", BeaconOfTruthId,
                maxStages: 1, node: "the_radio_tower", evt: null);
            RegisterDefault(QuestlineSO.Ids.PuttingDownRoots, "Putting Down Roots", MasterPathologistId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheGoldenParachute, "The Golden Parachute", MonopolistId,
                maxStages: 1, node: null, evt: null);
            // #284–#298 rebuilders / scholars / outlaws
            RegisterDefault(QuestlineSO.Ids.TheCanary, "The Canary", DeepDelverId,
                maxStages: 1, node: null, evt: "shelter_event_co_leak");
            RegisterDefault(QuestlineSO.Ids.TheLongHaul, "The Long Haul", LogisticsMasterId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheIronGate, "The Iron Gate", ForgeMasterId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheMess, "The Mess", SanitizationExpertId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheClearcut, "The Clearcut", DeforesterId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheStrain, "The Strain", EpidemiologistId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheDeadStars, "The Dead Stars", CelestialNavigatorId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheArchive, "The Archive", ArchivistId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.InTheBlack, "In the Black", AuditorId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheMasterpiece, "The Masterpiece", MaestroId,
                maxStages: 1, node: RadioTowerNodeId, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheStash, "The Stash", BlockadeRunnerId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheLastContract, "The Last Contract", ExecutionerId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheBigScore, "The Big Score", ShadowId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.ThePerfectFake, "The Perfect Fake", MasterOfDisguiseId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheEscape, "The Escape", MechanicProdigyId,
                maxStages: 1, node: null, evt: null);
            // #299–#318 ashes / improbable / final flawed
            RegisterDefault(QuestlineSO.Ids.TheRealWorld, "The Real World", DiplomatId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheGladiator, "The Gladiator", WastelandGladiatorId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheFirstSave, "The First Save", ChiefOfMedicineId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.PlayerOne, "Player One", DroneOperatorId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.LossOfFaith, "Loss of Faith", ChoirOfOneId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.SeparationAnxiety, "Separation Anxiety", HiveTacticsId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheIndependent, "The Independent", HiveHealingId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.Vindicated, "Vindicated", TruthSeekerId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheOldWoods, "The Old Woods", WildmanId,
                maxStages: 1, node: MutatedForestNodeId, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheAwakening, "The Awakening", SecondLifeId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.BreakingChains, "Breaking Chains", IronWillId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.EarningKeep, "Earning Keep", UnseenListenerId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.WorthlessPaper, "Worthless Paper", RuthlessCapitalistId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheMastersFate, "The Master's Fate", ProdigyId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheBackdraft, "The Backdraft", CommanderId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheProsthetic, "The Prosthetic", CyberArmId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.AGoodDeath, "A Good Death", RedemptionId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TearsInRain, "Tears in Rain", OverclockedId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.MansBestFriend, "Man's Best Friend", WastelandGuardianId,
                maxStages: 1, node: null, evt: null);
            RegisterDefault(QuestlineSO.Ids.TheTuringTest, "The Turing Test", OmniscienceId,
                maxStages: 1, node: null, evt: null);
        }

        private void RegisterDefault(
            string id, string display, string trait, int maxStages, string node, string evt)
        {
            if (_questlines.ContainsKey(id)) return;
            var q = ScriptableObject.CreateInstance<QuestlineSO>();
            q.id = id;
            q.displayName = display;
            q.description = display;
            q.latentExpertTraitId = trait;
            q.maxStages = maxStages;
            q.spawnMapNodeId = node;
            q.spawnBunkerEventId = evt;
            _questlines[id] = q;
        }

        private void SyncFromSurvivor(Survivor sv, PersonalQuestState state)
        {
            if (!string.IsNullOrEmpty(sv.LatentExpertTraitId))
                state.LatentTraitId = sv.LatentExpertTraitId;
            if (!string.IsNullOrEmpty(sv.ActiveQuestlineId))
                state.QuestlineId = sv.ActiveQuestlineId;
            if (!string.IsNullOrEmpty(sv.ArchetypeId))
                state.ArchetypeId = sv.ArchetypeId;
            if (sv.LatentTraitUnlocked) state.TraitUnlocked = true;
            if (sv.QuestlineActive) state.QuestActive = true;
            if (sv.DaysAlive > state.DaysAlive) state.DaysAlive = sv.DaysAlive;
            if (sv.MoraleHitZero) state.MoraleHitZero = true;
        }

        private PersonalQuestState GetOrCreate(string survivorId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var s))
            {
                s = new PersonalQuestState();
                _bySurvivor[survivorId] = s;
            }
            return s;
        }

        public PersonalQuestSave CaptureState()
        {
            var save = new PersonalQuestSave { Entries = new List<PersonalQuestEntrySave>() };
            foreach (var kv in _bySurvivor)
            {
                var s = kv.Value;
                save.Entries.Add(new PersonalQuestEntrySave
                {
                    SurvivorId = kv.Key,
                    ArchetypeId = s.ArchetypeId,
                    LatentTraitId = s.LatentTraitId,
                    QuestlineId = s.QuestlineId,
                    QuestActive = s.QuestActive,
                    TraitUnlocked = s.TraitUnlocked,
                    Stage = s.Stage,
                    Progress = s.Progress,
                    DaysAlive = s.DaysAlive,
                    MoraleHitZero = s.MoraleHitZero,
                    VetAirlockHours = s.VetAirlockHours,
                    VetKitsSpent = s.VetKitsSpent,
                    VisitedNodeIds = s.VisitedNodeIds != null
                        ? new List<string>(s.VisitedNodeIds)
                        : new List<string>(),
                    PerfectPlanterDays = s.PerfectPlanterDays,
                    DeadDropSuccesses = s.DeadDropSuccesses,
                    ManifestFound = s.ManifestFound,
                    TeacherMourningDays = s.TeacherMourningDays,
                    PropagandaResolutions = s.PropagandaResolutions,
                    CrisisOfFaithActive = s.CrisisOfFaithActive,
                    FirstStrikeIntelIds = s.FirstStrikeIntelIds != null
                        ? new List<string>(s.FirstStrikeIntelIds) : new List<string>(),
                    BroadcastHours = s.BroadcastHours,
                    HoardedFoodIds = s.HoardedFoodIds != null
                        ? new List<string>(s.HoardedFoodIds) : new List<string>(),
                    ClothingScrapsDisassembled = s.ClothingScrapsDisassembled,
                    WithdrawalCleanDays = s.WithdrawalCleanDays,
                    ChildDeathKnown = s.ChildDeathKnown,
                    Tier3ModulesBuilt = s.Tier3ModulesBuilt,
                    SafeNodesCarried = s.SafeNodesCarried,
                    SafeWasEmpty = s.SafeWasEmpty,
                    UrgeNeed = s.UrgeNeed,
                    MurderAttempted = s.MurderAttempted,
                    MaskSlipsResolved = s.MaskSlipsResolved,
                    SerialKillerEmbraced = s.SerialKillerEmbraced,
                    FalseIntelCount = s.FalseIntelCount,
                    ItemsStolen = s.ItemsStolen,
                    // #257–#266
                    UnequippedWeaponDays = s.UnequippedWeaponDays,
                    MentalBreaksCured = s.MentalBreaksCured,
                    SoloExpeditionDays = s.SoloExpeditionDays,
                    DepressionDays = s.DepressionDays,
                    CriticalSurgeryFailed = s.CriticalSurgeryFailed,
                    ScrapMechanicalParts = s.ScrapMechanicalParts,
                    ScrapElectronicScrap = s.ScrapElectronicScrap,
                    ScrapChemicals = s.ScrapChemicals,
                    ColdTurkeyCleanDays = s.ColdTurkeyCleanDays,
                    LongNightGuardNights = s.LongNightGuardNights,
                    FiresExtinguished = s.FiresExtinguished,
                    DespairToHopeConverts = s.DespairToHopeConverts,
                    PackTrainingDays = s.PackTrainingDays,
                    RealLeaderDirtyDays = s.RealLeaderDirtyDays,
                    NomadInsideDays = s.NomadInsideDays,
                    PrepperMreRemaining = s.PrepperMreRemaining,
                    HungerStrikeActive = s.HungerStrikeActive,
                    TechTabletDead = s.TechTabletDead,
                    SheriffStaminaMax = s.SheriffStaminaMax,
                    CorpsesCleaned = s.CorpsesCleaned,
                    FalloutStormsSurvivedExposed = s.FalloutStormsSurvivedExposed,
                    UniqueIntelIds = s.UniqueIntelIds != null
                        ? new List<string>(s.UniqueIntelIds) : new List<string>(),
                    ResourceDeficitDays = s.ResourceDeficitDays,
                    StorageLockedByAccountant = s.StorageLockedByAccountant,
                    MaestroRadAnxietySuppressUntilDay = s.MaestroRadAnxietySuppressUntilDay,
                    GetawayInsideDays = s.GetawayInsideDays,
                    // #299–#318
                    MedStudentHasPukedOnTrauma = s.MedStudentHasPukedOnTrauma,
                    DroneMappedNodeIds = s.DroneMappedNodeIds != null
                        ? new List<string>(s.DroneMappedNodeIds) : new List<string>(),
                    StarvationFoodlessDays = s.StarvationFoodlessDays,
                    TwinPartnerId = s.TwinPartnerId,
                    PatientHydratedDays = s.PatientHydratedDays,
                    BackdraftSaves = s.BackdraftSaves
                });
            }
            save.PillarOfAtlasDeathDebuffActive = PillarOfAtlasDeathDebuffActive;
            save.LivingSaintInspiredActive = LivingSaintInspiredActive;
            return save;
        }

        public void RestoreState(PersonalQuestSave save)
        {
            _bySurvivor.Clear();
            PillarOfAtlasDeathDebuffActive = save != null && save.PillarOfAtlasDeathDebuffActive;
            LivingSaintInspiredActive = save != null && save.LivingSaintInspiredActive;
            if (save?.Entries == null) return;
            for (int i = 0; i < save.Entries.Count; i++)
            {
                var e = save.Entries[i];
                if (e == null || string.IsNullOrEmpty(e.SurvivorId)) continue;
                _bySurvivor[e.SurvivorId] = new PersonalQuestState
                {
                    ArchetypeId = e.ArchetypeId,
                    LatentTraitId = e.LatentTraitId,
                    QuestlineId = e.QuestlineId,
                    QuestActive = e.QuestActive,
                    TraitUnlocked = e.TraitUnlocked,
                    Stage = e.Stage,
                    Progress = e.Progress,
                    DaysAlive = e.DaysAlive,
                    MoraleHitZero = e.MoraleHitZero,
                    VetAirlockHours = e.VetAirlockHours,
                    VetKitsSpent = e.VetKitsSpent,
                    VisitedNodeIds = e.VisitedNodeIds != null
                        ? new List<string>(e.VisitedNodeIds)
                        : new List<string>(),
                    PerfectPlanterDays = e.PerfectPlanterDays,
                    DeadDropSuccesses = e.DeadDropSuccesses,
                    ManifestFound = e.ManifestFound,
                    TeacherMourningDays = e.TeacherMourningDays,
                    PropagandaResolutions = e.PropagandaResolutions,
                    CrisisOfFaithActive = e.CrisisOfFaithActive,
                    FirstStrikeIntelIds = e.FirstStrikeIntelIds != null
                        ? new List<string>(e.FirstStrikeIntelIds) : new List<string>(),
                    BroadcastHours = e.BroadcastHours,
                    HoardedFoodIds = e.HoardedFoodIds != null
                        ? new List<string>(e.HoardedFoodIds) : new List<string>(),
                    ClothingScrapsDisassembled = e.ClothingScrapsDisassembled,
                    WithdrawalCleanDays = e.WithdrawalCleanDays,
                    ChildDeathKnown = e.ChildDeathKnown,
                    Tier3ModulesBuilt = e.Tier3ModulesBuilt,
                    SafeNodesCarried = e.SafeNodesCarried,
                    SafeWasEmpty = e.SafeWasEmpty,
                    UrgeNeed = e.UrgeNeed,
                    MurderAttempted = e.MurderAttempted,
                    MaskSlipsResolved = e.MaskSlipsResolved,
                    SerialKillerEmbraced = e.SerialKillerEmbraced,
                    FalseIntelCount = e.FalseIntelCount,
                    ItemsStolen = e.ItemsStolen,
                    UnequippedWeaponDays = e.UnequippedWeaponDays,
                    MentalBreaksCured = e.MentalBreaksCured,
                    SoloExpeditionDays = e.SoloExpeditionDays,
                    DepressionDays = e.DepressionDays,
                    CriticalSurgeryFailed = e.CriticalSurgeryFailed,
                    ScrapMechanicalParts = e.ScrapMechanicalParts,
                    ScrapElectronicScrap = e.ScrapElectronicScrap,
                    ScrapChemicals = e.ScrapChemicals,
                    ColdTurkeyCleanDays = e.ColdTurkeyCleanDays,
                    LongNightGuardNights = e.LongNightGuardNights,
                    FiresExtinguished = e.FiresExtinguished,
                    DespairToHopeConverts = e.DespairToHopeConverts,
                    PackTrainingDays = e.PackTrainingDays,
                    RealLeaderDirtyDays = e.RealLeaderDirtyDays,
                    NomadInsideDays = e.NomadInsideDays,
                    PrepperMreRemaining = e.PrepperMreRemaining,
                    HungerStrikeActive = e.HungerStrikeActive,
                    TechTabletDead = e.TechTabletDead,
                    SheriffStaminaMax = e.SheriffStaminaMax,
                    // #284–#298
                    CorpsesCleaned = e.CorpsesCleaned,
                    FalloutStormsSurvivedExposed = e.FalloutStormsSurvivedExposed,
                    UniqueIntelIds = e.UniqueIntelIds != null
                        ? new List<string>(e.UniqueIntelIds) : new List<string>(),
                    ResourceDeficitDays = e.ResourceDeficitDays,
                    StorageLockedByAccountant = e.StorageLockedByAccountant,
                    MaestroRadAnxietySuppressUntilDay = e.MaestroRadAnxietySuppressUntilDay,
                    GetawayInsideDays = e.GetawayInsideDays,
                    MedStudentHasPukedOnTrauma = e.MedStudentHasPukedOnTrauma,
                    DroneMappedNodeIds = e.DroneMappedNodeIds != null
                        ? new List<string>(e.DroneMappedNodeIds) : new List<string>(),
                    StarvationFoodlessDays = e.StarvationFoodlessDays,
                    TwinPartnerId = e.TwinPartnerId,
                    PatientHydratedDays = e.PatientHydratedDays,
                    BackdraftSaves = e.BackdraftSaves
                };
            }
        }

        public sealed class PersonalQuestState
        {
            public string ArchetypeId;
            public string LatentTraitId;
            public string QuestlineId;
            public bool QuestActive;
            public bool TraitUnlocked;
            public int Stage;
            public float Progress;
            public int DaysAlive;
            public bool MoraleHitZero;
            public float VetAirlockHours;
            public int VetKitsSpent;
            /// <summary>#223 White Elk nodes visited (distinct).</summary>
            public List<string> VisitedNodeIds = new List<string>();
            /// <summary>#230 consecutive perfect planter days.</summary>
            public int PerfectPlanterDays;
            /// <summary>#231 successful dead drops (not robbed).</summary>
            public int DeadDropSuccesses;
            public bool ManifestFound;
            public int TeacherMourningDays;
            public int PropagandaResolutions;
            public bool CrisisOfFaithActive;
            public List<string> FirstStrikeIntelIds = new List<string>();
            public float BroadcastHours;
            public List<string> HoardedFoodIds = new List<string>();
            public int ClothingScrapsDisassembled;
            public int WithdrawalCleanDays;
            public bool ChildDeathKnown;
            // #249–#256
            public int Tier3ModulesBuilt;
            public int SafeNodesCarried;
            public bool SafeWasEmpty;
            public float UrgeNeed;
            public bool MurderAttempted;
            public bool MaskSlipsResolved;
            public bool SerialKillerEmbraced;
            public int FalseIntelCount;
            public int ItemsStolen;
            // #257–#266
            public int UnequippedWeaponDays;
            public int MentalBreaksCured;
            public int SoloExpeditionDays;
            public int DepressionDays;
            public bool CriticalSurgeryFailed;
            public int ScrapMechanicalParts;
            public int ScrapElectronicScrap;
            public int ScrapChemicals;
            // #267–#283
            public int ColdTurkeyCleanDays;
            public int LongNightGuardNights;
            public int FiresExtinguished;
            public int DespairToHopeConverts;
            public int PackTrainingDays;
            public int RealLeaderDirtyDays;
            public int NomadInsideDays;
            public float PrepperMreRemaining;
            public bool HungerStrikeActive;
            public bool TechTabletDead;
            public float SheriffStaminaMax;
            // #284–#298
            public int CorpsesCleaned;
            public int FalloutStormsSurvivedExposed;
            public List<string> UniqueIntelIds = new List<string>();
            public int ResourceDeficitDays;
            public bool StorageLockedByAccountant;
            public int MaestroRadAnxietySuppressUntilDay;
            public int GetawayInsideDays;
            // #299–#318
            public bool MedStudentHasPukedOnTrauma;
            public List<string> DroneMappedNodeIds = new List<string>();
            public int StarvationFoodlessDays;
            public string TwinPartnerId;
            public int PatientHydratedDays;
            public int BackdraftSaves;
            /// <summary>Ephemeral: chem use between daily ticks (#267). Not saved.</summary>
            public bool UsedChemThisDay;
            /// <summary>Ephemeral: dirty labor performed today (#279). Not saved.</summary>
            public bool DidDirtyLaborThisDay;
            /// <summary>Ephemeral: clean water consumed today (#285). Not saved.</summary>
            public bool DrankCleanWaterThisDay;

            public PersonalQuestState Clone() => new PersonalQuestState
            {
                ArchetypeId = ArchetypeId,
                LatentTraitId = LatentTraitId,
                QuestlineId = QuestlineId,
                QuestActive = QuestActive,
                TraitUnlocked = TraitUnlocked,
                Stage = Stage,
                Progress = Progress,
                DaysAlive = DaysAlive,
                MoraleHitZero = MoraleHitZero,
                VetAirlockHours = VetAirlockHours,
                VetKitsSpent = VetKitsSpent,
                VisitedNodeIds = VisitedNodeIds != null
                    ? new List<string>(VisitedNodeIds)
                    : new List<string>(),
                PerfectPlanterDays = PerfectPlanterDays,
                DeadDropSuccesses = DeadDropSuccesses,
                ManifestFound = ManifestFound,
                TeacherMourningDays = TeacherMourningDays,
                PropagandaResolutions = PropagandaResolutions,
                CrisisOfFaithActive = CrisisOfFaithActive,
                FirstStrikeIntelIds = FirstStrikeIntelIds != null
                    ? new List<string>(FirstStrikeIntelIds) : new List<string>(),
                BroadcastHours = BroadcastHours,
                HoardedFoodIds = HoardedFoodIds != null
                    ? new List<string>(HoardedFoodIds) : new List<string>(),
                ClothingScrapsDisassembled = ClothingScrapsDisassembled,
                WithdrawalCleanDays = WithdrawalCleanDays,
                ChildDeathKnown = ChildDeathKnown,
                Tier3ModulesBuilt = Tier3ModulesBuilt,
                SafeNodesCarried = SafeNodesCarried,
                SafeWasEmpty = SafeWasEmpty,
                UrgeNeed = UrgeNeed,
                MurderAttempted = MurderAttempted,
                MaskSlipsResolved = MaskSlipsResolved,
                SerialKillerEmbraced = SerialKillerEmbraced,
                FalseIntelCount = FalseIntelCount,
                ItemsStolen = ItemsStolen,
                UnequippedWeaponDays = UnequippedWeaponDays,
                MentalBreaksCured = MentalBreaksCured,
                SoloExpeditionDays = SoloExpeditionDays,
                DepressionDays = DepressionDays,
                CriticalSurgeryFailed = CriticalSurgeryFailed,
                ScrapMechanicalParts = ScrapMechanicalParts,
                ScrapElectronicScrap = ScrapElectronicScrap,
                ScrapChemicals = ScrapChemicals,
                ColdTurkeyCleanDays = ColdTurkeyCleanDays,
                LongNightGuardNights = LongNightGuardNights,
                FiresExtinguished = FiresExtinguished,
                DespairToHopeConverts = DespairToHopeConverts,
                PackTrainingDays = PackTrainingDays,
                RealLeaderDirtyDays = RealLeaderDirtyDays,
                NomadInsideDays = NomadInsideDays,
                PrepperMreRemaining = PrepperMreRemaining,
                HungerStrikeActive = HungerStrikeActive,
                TechTabletDead = TechTabletDead,
                SheriffStaminaMax = SheriffStaminaMax,
                CorpsesCleaned = CorpsesCleaned,
                FalloutStormsSurvivedExposed = FalloutStormsSurvivedExposed,
                UniqueIntelIds = UniqueIntelIds != null
                    ? new List<string>(UniqueIntelIds) : new List<string>(),
                ResourceDeficitDays = ResourceDeficitDays,
                StorageLockedByAccountant = StorageLockedByAccountant,
                MaestroRadAnxietySuppressUntilDay = MaestroRadAnxietySuppressUntilDay,
                GetawayInsideDays = GetawayInsideDays,
                MedStudentHasPukedOnTrauma = MedStudentHasPukedOnTrauma,
                DroneMappedNodeIds = DroneMappedNodeIds != null
                    ? new List<string>(DroneMappedNodeIds) : new List<string>(),
                StarvationFoodlessDays = StarvationFoodlessDays,
                TwinPartnerId = TwinPartnerId,
                PatientHydratedDays = PatientHydratedDays,
                BackdraftSaves = BackdraftSaves
            };
        }
    }

    /// <summary>
    /// Predetermined destiny for a survivor archetype (Prompt #214 SurvivorProfile).
    /// Latent trait is NOT granted until the questline completes.
    /// </summary>
    [Serializable]
    public class SurvivorProfile
    {
        public string ArchetypeId;
        public string LatentExpertTraitId;
        public string ActiveQuestlineId;

        public SurvivorProfile() { }

        public SurvivorProfile(string archetypeId, string latentTrait, string questlineId)
        {
            ArchetypeId = archetypeId;
            LatentExpertTraitId = latentTrait;
            ActiveQuestlineId = questlineId;
        }
    }

    [Serializable]
    public class PersonalQuestSave
    {
        public List<PersonalQuestEntrySave> Entries = new List<PersonalQuestEntrySave>();
        /// <summary>#250 permanent shelter repair debuff after Pillar of Atlas dies.</summary>
        public bool PillarOfAtlasDeathDebuffActive;
        /// <summary>#265 permanent bunker Inspired floor after Living Saint dies.</summary>
        public bool LivingSaintInspiredActive;
    }

    [Serializable]
    public class PersonalQuestEntrySave
    {
        public string SurvivorId;
        public string ArchetypeId;
        public string LatentTraitId;
        public string QuestlineId;
        public bool QuestActive;
        public bool TraitUnlocked;
        public int Stage;
        public float Progress;
        public int DaysAlive;
        public bool MoraleHitZero;
        public float VetAirlockHours;
        public int VetKitsSpent;
        public List<string> VisitedNodeIds = new List<string>();
        public int PerfectPlanterDays;
        public int DeadDropSuccesses;
        public bool ManifestFound;
        public int TeacherMourningDays;
        public int PropagandaResolutions;
        public bool CrisisOfFaithActive;
        public List<string> FirstStrikeIntelIds = new List<string>();
        public float BroadcastHours;
        public List<string> HoardedFoodIds = new List<string>();
        public int ClothingScrapsDisassembled;
        public int WithdrawalCleanDays;
        public bool ChildDeathKnown;
        // #249–#256
        public int Tier3ModulesBuilt;
        public int SafeNodesCarried;
        public bool SafeWasEmpty;
        public float UrgeNeed;
        public bool MurderAttempted;
        public bool MaskSlipsResolved;
        public bool SerialKillerEmbraced;
        public int FalseIntelCount;
        public int ItemsStolen;
        // #257–#266
        public int UnequippedWeaponDays;
        public int MentalBreaksCured;
        public int SoloExpeditionDays;
        public int DepressionDays;
        public bool CriticalSurgeryFailed;
        public int ScrapMechanicalParts;
        public int ScrapElectronicScrap;
        public int ScrapChemicals;
        // #267–#283
        public int ColdTurkeyCleanDays;
        public int LongNightGuardNights;
        public int FiresExtinguished;
        public int DespairToHopeConverts;
        public int PackTrainingDays;
        public int RealLeaderDirtyDays;
        public int NomadInsideDays;
        public float PrepperMreRemaining;
        public bool HungerStrikeActive;
        public bool TechTabletDead;
        public float SheriffStaminaMax;
        // #284–#298
        public int CorpsesCleaned;
        public int FalloutStormsSurvivedExposed;
        public List<string> UniqueIntelIds = new List<string>();
        public int ResourceDeficitDays;
        public bool StorageLockedByAccountant;
        public int MaestroRadAnxietySuppressUntilDay;
        public int GetawayInsideDays;
        // #299–#318
        public bool MedStudentHasPukedOnTrauma;
        public List<string> DroneMappedNodeIds = new List<string>();
        public int StarvationFoodlessDays;
        public string TwinPartnerId;
        public int PatientHydratedDays;
        public int BackdraftSaves;
    }
}
