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
        /// <summary>
        /// H-5: Register every constructed system with the SystemRegistry.
        /// This is called at the end of InitializeSystems after all systems
        /// are created. Each registration identifies the system by name and
        /// its tick category (per-substep, daily, event-driven, or save-only).
        ///
        /// After this method, the registry's IsSystemTicked() can verify
        /// that every system appears in at least one category — a missing
        /// system here IS a C-1 class bug.
        ///
        /// The registration mirrors the TickSystems() method order for
        /// auditability: compare this list against the TickSystems body to
        /// verify every ticked system is registered and vice versa.
        /// </summary>
        private void RegisterSystemsInRegistry()
        {
            if (_registry == null) return;

            RegisterEnvironmentSubsteps();
            RegisterShelterAndHorrorSubsteps();
            RegisterNeedsPsycheSubsteps();
            RegisterRadiationCraftSubsteps();
            RegisterDailyAndHouseSubsteps();
            RegisterEventDrivenAndSaveOnlySystems();

            // H-5: Verify no system is constructed but unticked.
            VerifyAllSystemsRegistered();

            // L-1: Warn on unassigned ScriptableObject references so designers
            // catch missing inspector assignments before they become silent failures.
            VerifyCriticalScriptableObjects();
        }

        private void RegisterEnvironmentSubsteps()
        {
            _registry.RegisterPerSubstep("weather", h => WeatherSystem?.Tick(h));
            _registry.RegisterPerSubstep("temperature", h => TemperatureSystem?.Tick(h));
            _registry.RegisterPerSubstep("photoperiod", h => PhotoperiodSystem?.Tick(h));
            _registry.RegisterPerSubstep("hatch_entrapment", h =>
            {
                if (HatchEntrapmentSystem == null || WeatherSystem == null || TimeSystem == null) return;
                HatchEntrapmentSystem.Tick(h, WeatherSystem.Current, Shelter,
                    _getFactionTrustEffective, _scheduleEventCached, TimeSystem.CurrentDay);
                SyncHatchExpeditionLock();
            });
        }

        private void RegisterShelterAndHorrorSubsteps()
        {
            _registry.RegisterPerSubstep("shelter", h => Shelter?.Tick(h));
            _registry.RegisterPerSubstep("power_network", h =>
            {
                if (PowerNetwork == null) return;
                PowerNetwork.Tick(h, WeatherSystem != null ? WeatherNameOf(WeatherSystem.Current) : null, _tryApplyPedalCostCached);
                PowerNetwork.ApplyToShelter(Shelter);
            });
            _registry.RegisterPerSubstep("hatch_defense", h => HatchDefenseSystem?.Tick(h, PowerNetwork));
            _registry.RegisterPerSubstep("atmosphere", h =>
            {
                if (AtmosphereSystem == null) return;
                if (ShelterPerks != null)
                    AtmosphereSystem.VentilationClearMultiplier =
                        ShelterPerks.GetVentilationClearMultiplier(Survivors);
                AtmosphereSystem.Tick(h, PowerNetwork, Shelter);
            });
            _registry.RegisterPerSubstep("corpses", h => CorpseSystem?.Tick(h, Survivors));
            _registry.RegisterPerSubstep("pantry", h => PantrySystem?.Tick(h, _storesRoom));
            _registry.RegisterPerSubstep("structural_integrity", h =>
            {
                if (StructuralIntegrity == null || WeatherSystem == null) return;
                var weather = WeatherSystem.Current;
                if (weather == WeatherKind.FalloutStorm)
                    StructuralIntegrity.ApplyDamage(StructuralIntegritySystem.FalloutStormDamagePerHour * h, "fallout_storm");
                else if (weather == WeatherKind.Blizzard && WorldPhaseSystem != null
                    && WorldPhaseSystem.CurrentPhase == AtomicWar._Game.Survivors.WorldPhase.CivilWar)
                    StructuralIntegrity.ApplyDamage(StructuralIntegritySystem.MortarStrikeDamage * 0.3f * h, "mortar_strike");
                StructuralIntegrity.Tick(h, Shelter);
            });
            _registry.RegisterPerSubstep("waste", h => WasteSystem?.Tick(h, TimeSystem?.CurrentDay ?? 1));
            _registry.RegisterPerSubstep("vermin", h => VerminSystem?.Tick(h, Inventory));
            _registry.RegisterPerSubstep("jury_rig", h => JuryRigSystem?.Tick(h, TimeSystem?.CurrentDay ?? 1));
            _registry.RegisterPerSubstep("freeze_pipe", h => FreezePipeSystem?.Tick(h));
            _registry.RegisterPerSubstep("tracker", h => TrackerSystem?.Tick(h,
                (fid, chance) => HatchDefenseSystem?.SetRaidChanceOverride(fid, chance),
                (eventId, fireDay, originFlag) => EventRunner?.ScheduleEvent(eventId, fireDay, originFlag),
                TimeSystem?.CurrentDay ?? 1));
            _registry.RegisterPerSubstep("dead_drops", h => DeadDropSystem?.Tick(h));
            _registry.RegisterPerSubstep("hostage", h => HostageSystem?.Tick(h));
            _registry.RegisterPerSubstep("propaganda", h => PropagandaSystem?.Tick(h,
                (fid, delta) => EconomySystem?.ModifyTrust(fid, delta),
                (fid, reduction) => HatchDefenseSystem?.AdjustRaidChance(fid, -reduction)));
            _registry.RegisterPerSubstep("scapegoat", h => ScapegoatSystem?.Tick(h, WeatherSystem.Current,
                (eventId, fireDay, flag) => EventRunner?.ScheduleEvent(eventId, fireDay, flag),
                TimeSystem?.CurrentDay ?? 1));

            bool preDay30 = WorldPhaseSystem != null && !WorldPhaseSystem.HasTriggeredExchange;
            _registry.RegisterPerSubstep("flooding", h => FloodingSystem?.Tick(h,
                WeatherSystem.Current == WeatherKind.Rain, preDay30, Shelter,
                roomId => roomId == "cellar" || roomId == "coal_room"));
            // Prompt #806 — daily bilge harvest while rooms stay flooded.
            _registry.RegisterPerSubstep("bilge_pumps_daily",
                _registry.DayGated("bilge_pumps", day => TickBilgePumpsDaily()));
            // Prompt #658 — carrion birds arrive/depart + hatch/map/morale pressure.
            _registry.RegisterPerSubstep("carrion_birds_daily",
                _registry.DayGated("carrion_birds", day => TickCarrionBirdsDaily()));
            _registry.RegisterPerSubstep("perimeter_trap", h => PerimeterTrapSystem?.Tick(h));
            _registry.RegisterPerSubstep("noise", h =>
            {
                if (NoiseSystem == null) return;
                // #268 Restless night pacing: night gate via TimeSystem hour.
                bool isNight = TimeSystem != null
                    && (TimeSystem.CurrentHour < 6 || TimeSystem.CurrentHour >= 22);
                if (isNight)
                    NoiseSystem.TickPersonalQuestNoise(h, isNight: true);
                else
                    NoiseSystem.Tick(h);
            });
            _registry.RegisterPerSubstep("clothing", h => TickClothing(h));
            _registry.RegisterPerSubstep("compost", h => TickCompost(h));
            _registry.RegisterPerSubstep("pets", h => PetSystem?.Tick(Survivors, h));
        }

        private void RegisterNeedsPsycheSubsteps()
        {
            _registry.RegisterPerSubstep("needs", h => NeedsSystem?.Tick(h));
            _registry.RegisterPerSubstep("medical", h => MedicalSystem?.Tick(Survivors, h));
            _registry.RegisterPerSubstep("mental_break", h =>
            {
                if (MentalBreakSystem != null && _mentalBreakRng != null)
                    MentalBreakSystem.Tick(h, Survivors, _mentalBreakRng);
            });
            // Prompts #469-#478 — social: auras every substep; daily transitions day-gated internally.
            _registry.RegisterPerSubstep("bunker_social", h =>
                BunkerSocial?.Tick(h, TimeSystem?.CurrentDay ?? 1, Survivors, _mentalBreakRng));
            // Prompt #839 — gossip spread + affinity rot once per day.
            _registry.RegisterPerSubstep("gossip_daily",
                _registry.DayGated("gossip", day =>
                {
                    RefreshGossipRoster();
                    Gossip?.TickDay();
                }));
            _registry.RegisterPerSubstep("skill_atrophy", h => SkillAtrophy?.Tick(h, Survivors));
            _registry.RegisterPerSubstep("skill_progression_daily",
                _registry.DayGated("skill_progression", day => SkillProgression?.TickDaily(day, Survivors)));
            // Prompt #213 — Taskmaster high-morale streak.
            _registry.RegisterPerSubstep("social_perks_daily",
                _registry.DayGated("social_perks", day => SocialPerks?.TickDailyMorale(Survivors, day)));
            // Prompts #214–#219 — personal quest days-alive + Anchor lock.
            _registry.RegisterPerSubstep("personal_quests_daily",
                _registry.DayGated("personal_quests", day => PersonalQuests?.TickDaily(Survivors, day)));
            // Morale 0→100 quest trigger + Anchor room floor each needs substep.
            _registry.RegisterPerSubstep("personal_quests_morale", h =>
            {
                if (PersonalQuests == null || Survivors == null) return;
                int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;
                PersonalQuests.WatchMoraleAll(Survivors, day);
                for (int i = 0; i < Survivors.Count; i++)
                    PersonalQuests.ApplyRoomMoraleFloor(Survivors[i], Survivors);
            });
            // #249–#256: hoarder internal theft from main storage into personal stash.
            _registry.RegisterPerSubstep("personal_quests_hoarder_theft", h =>
            {
                if (PersonalQuests == null || Survivors == null || Inventory == null || h <= 0f) return;
                // ~5% chance per game-hour per Selfish hoarder to steal one unit.
                var rng = _mentalBreakRng ?? new System.Random(256);
                for (int i = 0; i < Survivors.Count; i++)
                {
                    var sv = Survivors[i];
                    if (sv == null || !sv.IsAlive) continue;
                    if (!PersonalQuests.HasSelfish(sv)) continue;
                    if (rng.NextDouble() > 0.05 * h) continue;
                    if (Inventory.Slots == null || Inventory.Slots.Count == 0) continue;
                    int start = rng.Next(0, Inventory.Slots.Count);
                    for (int k = 0; k < Inventory.Slots.Count; k++)
                    {
                        var slot = Inventory.Slots[(start + k) % Inventory.Slots.Count];
                        if (slot?.Item == null || slot.Amount <= 0) continue;
                        string itemId = slot.Item.id;
                        if (string.IsNullOrEmpty(itemId)) continue;
                        if (!Inventory.Remove(slot.Item, 1)) continue;
                        PersonalQuests.TryStealToPersonalInventory(sv, itemId);
                        break;
                    }
                }
            });
            _registry.RegisterPerSubstep("empath", h => EmpathSystem?.Tick(h, Survivors));
            _registry.RegisterPerSubstep("survivor_diaries", h => SurvivorDiaries?.Tick(h, Survivors, TimeSystem?.CurrentDay ?? 1, _mentalBreakRng));
            _registry.RegisterPerSubstep("spatial_psychology", h => SpatialPsychology?.Tick(h, Survivors));
            _registry.RegisterPerSubstep("hallucination", h => HallucinationSystem?.Tick(h, Survivors, _mentalBreakRng));
            _registry.RegisterPerSubstep("addiction", h => Addiction?.Tick(h, Survivors, TimeSystem?.CurrentDay ?? 1));
            _registry.RegisterPerSubstep("pheromone_masking", TickPheromoneMasking);
            _registry.RegisterPerSubstep("phantom_intruders", h => PhantomIntruders?.Tick(h, Survivors, _phantomRng));
            _registry.RegisterPerSubstep("child", h => ChildSystem?.Tick(h, Survivors));
            _registry.RegisterPerSubstep("hatch_dilemma", h => HatchDilemmaPromptField?.Tick(h));
            _registry.RegisterPerSubstep("parley_offer", h => ParleyOfferPromptField?.Tick(h));
        }

        private void TickPheromoneMasking(float hours)
        {
            if (PheromoneMasking == null || hours <= 0f) return;
            // Snapshot keys — TickHour may expire and mutate the dict.
            var ids = new List<string>(PheromoneMasking.States.Keys);
            for (int i = 0; i < ids.Count; i++)
                PheromoneMasking.TickHour(ids[i], hours);
        }

        private void RegisterRadiationCraftSubsteps()
        {
            _registry.RegisterPerSubstep("radiation", h => RadiationSystem?.Tick(h));
            _registry.RegisterPerSubstep("water_economy", h => WaterEconomySystem?.Tick(h, WeatherSystem.Current, TimeSystem?.CurrentDay ?? 1, Shelter, WaterStorage));
            _registry.RegisterPerSubstep("black_rain", h =>
            {
                if (BlackRainHazardSystem != null && Survivors != null)
                    BlackRainHazardSystem.TickDread(Survivors, IsSurvivorOnExpedition, IsSurvivorHatchListener, h);
            });
            _registry.RegisterPerSubstep("crafting", h => CraftingSystem?.Tick(h));
            _registry.RegisterPerSubstep("scavenging", h => ScavengingSystem?.Tick(h));
            _registry.RegisterPerSubstep("expeditions", h => ExpeditionSystem?.Tick(h));
            _registry.RegisterPerSubstep("radio_tuner", h =>
            {
                if (RadioTunerSystem == null || Shelter == null) return;
                var radioModule = Shelter.GetModule("radio");
                if (radioModule != null && radioModule.IsOperational && radioModule.Fuel > 0f)
                    RadioTunerSystem.Tick(h, WeatherSystem.Current, TimeSystem?.CurrentDay ?? 1);
            });
            _registry.RegisterPerSubstep("mutagenesis_tick", h => Mutagenesis?.Tick(h, Survivors));
        }

        private void RegisterDailyAndHouseSubsteps()
        {
            _registry.RegisterPerSubstep("deserter_daily",
                _registry.DayGated("deserter", RegisterTickDeserterDaily));
            _registry.RegisterPerSubstep("ecosystem_daily",
                _registry.DayGated("ecosystem", RegisterTickEcosystemDaily));
            _registry.RegisterPerSubstep("amputation_daily",
                _registry.DayGated("amputation", day => AmputationSystem?.TickDaily(Survivors)));
            _registry.RegisterPerSubstep("scurvy_daily",
                _registry.DayGated("scurvy", day => ScurvySystem?.TickDaily(Survivors)));
            _registry.RegisterPerSubstep("graft_rejection_daily",
                _registry.DayGated("graft_rejection", RegisterTickGraftRejectionDaily));
            _registry.RegisterPerSubstep("mutagenesis_daily",
                _registry.DayGated("mutagenesis", day => Mutagenesis?.Evaluate(Survivors)));
            _registry.RegisterPerSubstep("hatch_visibility_daily",
                _registry.DayGated("hatch_visibility", day => HatchVisibilitySystem?.TickDaily()));
            // Reuse TickHouseToBunkerDaily from TickSystems (same behavior; DayGated once/day).
            _registry.RegisterPerSubstep("house_to_bunker",
                _registry.DayGated("house_to_bunker", TickHouseToBunkerDaily));
            _registry.RegisterPerSubstep("system_wiring",
                _registry.DayGated("system_wiring", RegisterTickSystemWiringDaily));
            // #267–#283 chemistry/titles daily host helpers (pyro fire, sheriff guard, journal spam).
            _registry.RegisterPerSubstep("chemistry_titles_daily",
                _registry.DayGated("chemistry_titles", RegisterTickChemistryTitlesDaily));
            // #284–#298 rebuilders: accountant capacity quest + storage lock pressure.
            _registry.RegisterPerSubstep("rebuilders_daily",
                _registry.DayGated("rebuilders", RegisterTickRebuildersDaily));
            _registry.RegisterPerSubstep("fuel_decay_daily",
                _registry.DayGated("fuel_decay", RegisterTickFuelDecayDaily));
        }

        private void RegisterTickFuelDecayDaily(int day)
        {
            if (FuelDecaySystem == null) return;
            FuelDecaySystem.TickDaily(day);
            ApplyFuelDecayToPowerNetwork();
        }

        private void RegisterTickGraftRejectionDaily(int day)
        {
            if (GraftRejection == null) return;
            var ids = new List<string>(GraftRejection.States.Keys);
            for (int i = 0; i < ids.Count; i++)
                GraftRejection.TickDay(ids[i]);
        }

        /// <summary>
        /// Prompt #380 — map gasoline varnish efficiency into diesel burn cost.
        /// Lower efficiency → higher burn multiplier (more fuel for same watts).
        /// </summary>
        private void ApplyFuelDecayToPowerNetwork()
        {
            if (PowerNetwork == null || FuelDecaySystem?.State == null) return;
            float eff = FuelDecaySystem.State.fuelEfficiencyMultiplier;
            if (eff < 0.01f) eff = 0.01f;
            PowerNetwork.FuelDegradationBurnMultiplier = 1f / eff;
        }

        private void RegisterTickChemistryTitlesDaily(int day)
        {
            var rng = _mentalBreakRng ?? new System.Random(day + 270);
            AtmosphereSystem?.TryPyromaniacDeliberateFire(rng);
            HatchDefenseSystem?.TryAutoAssignSheriffGuard();
            JournalSystem?.TickNewsAnchorJournalSpam(day);
        }

        private void RegisterTickRebuildersDaily(int day)
        {
            if (PersonalQuests == null || Survivors == null || Inventory == null) return;
            float food = Inventory.FoodFillRatio();
            float water = Inventory.WaterFillRatio();
            float fuel = Inventory.FuelFillRatio();
            bool deficit = food < 0.5f || water < 0.5f || fuel < 0.5f;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                if (string.Equals(sv.ArchetypeId, PersonalQuestSystem.AccountantId,
                        System.StringComparison.Ordinal)
                    || PersonalQuests.HasPennyPincher(sv)
                    || PersonalQuests.HasAuditor(sv))
                {
                    PersonalQuests.RecordResourceDeficitDay(sv, deficit, day);
                    PersonalQuests.NotifyResourceCapacities(sv, food, water, fuel, day);
                }
            }
        }

        private void RegisterTickDeserterDaily(int day)
        {
            DeserterSystem?.TickDaily(Shelter,
                eventId => EventRunner?.ScheduleEvent(eventId, day + 1, null));
        }

        private void RegisterTickEcosystemDaily(int day)
        {
            float outdoorRad = RadiationSystem != null ? 15f : 0f;
            bool exchangeTriggered = WorldPhaseSystem != null && WorldPhaseSystem.HasTriggeredExchange;
            EcosystemSystem?.TickDaily(outdoorRad, exchangeTriggered);
        }

        private void RegisterTickSystemWiringDaily(int day)
        {
            if (_systemWiring == null || TimeSystem == null) return;
            _systemWiring.WireDaily(BuildDailyWiringContext(day));
        }

        private SystemWiring.DailyContext BuildDailyWiringContext(int day)
        {
            float indoor = TemperatureSystem != null
                ? TemperatureSystem.GetIndoorTemperature(Shelter)
                : 15f;
            return new SystemWiring.DailyContext
            {
                CurrentDay = day,
                Compost = CompostSystem,
                Chelation = ChelationSystem,
                Aesthetics = AestheticsSystem,
                HamRadio = HamRadioSystem,
                Polypharmacy = PolypharmacySystem,
                CeilingCollapse = CeilingCollapseSystem,
                LocationQuest = LocationQuestSystem,
                Shelter = Shelter,
                Inventory = Inventory,
                Survivors = Survivors,
                Rooms = Shelter?.Rooms != null
                    ? new System.Collections.Generic.List<Shelter.ShelterRoom>(Shelter.Rooms)
                    : null,
                Rng = CreateSaltedRng(_worldSeed, "registry_ctx"),
                ShelterPerks = ShelterPerks,
                IndoorTemperatureC = indoor
            };
        }

        private void RegisterEventDrivenAndSaveOnlySystems()
        {
            string[] eventDriven =
            {
                "excavation", "hidden_storage", "tunneling", "material_shielding", "airlock",
                "escape_hatch", "sterilization", "chelation", "wind_turbine",
                "antibiotic_resist", "hauling", "weapon_maint", "triage", "scrap_weapon",
                // Chem abuse bookkeeping (ticks only on treatment item consume):
                "blood_toxicity",
                "tolerance",
                // Rogue-lite grave (fires on wipe) + legacy bunker start + river crossings:
                "last_will",
                "legacy_start",
                "blood_types",
                "epilogue_stats",
                "river_nodes",
                // Infrastructure / AI / UI systems that are not hour-ticked:
                "workbench", "utility_ai", "radio", "time", "save", "game_state",
                "endgame", "shelter_layout", "sleep_quality", "skill_progression",
                // Player/AI action systems (no autonomous hour tick):
                "cooking", "mentorship", "combat_perks", "survival_perks", "shelter_perks",
                "medical_perks", "expedition_perks", "night_scavenge"
            };
            for (int i = 0; i < eventDriven.Length; i++)
                _registry.RegisterEventDriven(eventDriven[i]);

            string[] saveOnly =
            {
                "knowledge_map", "inventory", "belief", "world_phase", "economy", "water_storage",
                "generated_map", "faction_radio_intercepts", "journal", "victory_project",
                "event_runner", "suspicion_tracker", "lifeboat", "sabotaged_cache",
                "shifting_hotspot", "faction_raid_plan", "debt_collector", "ghost_station",
                "cartography", "bicycle", "flooded_node", "blood_transfusion", "cult_moral",
                "labor_camp", "location_quest", "ceiling_collapse", "aesthetics", "ham_radio",
                "polypharmacy", "resilience", "internal_lock", "grief_keepsakes", "mentorship",
                "flashpoint_choreographer", "diary_catalog"
            };
            for (int i = 0; i < saveOnly.Length; i++)
                _registry.RegisterSaveOnly(saveOnly[i]);
        }

    }
}
