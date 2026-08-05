using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Personal Quest Engine &amp; Latent Expert Traits (Prompts #214–#234).
    /// Survivors do not start with their Expert Trait. After 30 days alive OR
    /// a Morale 0→100 recovery, their assigned questline begins. Completing
    /// the final stage permanently unlocks the latent expert trait.
    /// Plain C#, save/load safe, inventory-free (Survivors leaf assembly).
    /// </summary>
    public class PersonalQuestSystem
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

        private SkillProgressionSystem _progression;
        private readonly Dictionary<string, QuestlineSO> _questlines =
            new Dictionary<string, QuestlineSO>();
        private readonly Dictionary<string, PersonalQuestState> _bySurvivor =
            new Dictionary<string, PersonalQuestState>();

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

        public void Bind(SkillProgressionSystem progression)
        {
            _progression = progression;
            _progression?.RegisterLatentExpertTraits();
            EnsureDefaultQuestlines();
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
        /// Does NOT grant the trait — only stores the destiny.
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
                _ => archetypeId
            };
            string discipline = "survival";
            if (archetypeId == SurgeonId || archetypeId == PharmacistId
                || archetypeId == VetId || archetypeId == TherapistId
                || archetypeId == ChemistId)
                discipline = "medical";
            else if (archetypeId == VeteranId || archetypeId == CopId || archetypeId == BouncerId)
                discipline = "combat";
            else if (archetypeId == HunterId || archetypeId == CourierId
                || archetypeId == BurglarId || archetypeId == MechanicId)
                discipline = "scavenging";
            var sv = new Survivor
            {
                Id = runtimeId ?? archetypeId,
                DisplayName = name,
                State = SurvivorState.Idle,
                ExpertDisciplineId = discipline
            };
            sv.Needs.Morale = 60f;
            sv.Needs.Health = 100f;
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
            }
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
                undertaker.RadiationDose = Mathf.Clamp(
                    undertaker.RadiationDose + MassGraveRadHit, 0f, 100f);
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

            plumber.RadiationDose = Mathf.Clamp(
                plumber.RadiationDose + PipeBurstIrradiatedRadSpike, 0f, 100f);
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
            rng ??= new System.Random();
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

        /// <summary>#219 — zero morale penalty from corpses / murder / butchering.</summary>
        public bool IsImmuneToDeathMorale(Survivor sv) => HasDeathBlind(sv);

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

        /// <summary>#222 — effective carry capacity (unlimited sentinel when Juggernaut).</summary>
        public float GetExpeditionCarryCapacity(Survivor sv, float baseCapacity)
        {
            if (HasJuggernaut(sv)) return 99999f;
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
                    DeadDropSuccesses = s.DeadDropSuccesses
                });
            }
            return save;
        }

        public void RestoreState(PersonalQuestSave save)
        {
            _bySurvivor.Clear();
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
                    DeadDropSuccesses = e.DeadDropSuccesses
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
                DeadDropSuccesses = DeadDropSuccesses
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
    }
}
