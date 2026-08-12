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

using AtomicWar._Game.Encounters;

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
            RegisterSection7Systems();
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
            // Validate staff before resource systems consume the current substep.
            _registry.RegisterPerSubstep("survivor_work_shifts", h => SurvivorWorkShiftSystem?.Tick(h));
            _registry.RegisterPerSubstep("shelter", h => Shelter?.Tick(h));
            _registry.RegisterPerSubstep("power_network", h =>
            {
                if (PowerNetwork == null) return;
                PowerNetwork.Tick(h, WeatherSystem != null ? WeatherNameOf(WeatherSystem.Current) : null, _tryApplyPedalCostCached);
                PowerNetwork.ApplyToShelter(Shelter);
            });
            // C-1: both depend on the power state applied just above, so they stay
            // registered immediately after power_network to preserve tick order.
            // Previously they were only refreshed as a side effect nested inside
            // that closure, invisible to the registry's own C-1 gap diagnostic.
            _registry.RegisterPerSubstep("air_heat_management", h => AirHeatManagementSystem?.Refresh());
            _registry.RegisterPerSubstep("bunker_maintenance", h => BunkerMaintenanceSystem?.Refresh());
            _registry.RegisterPerSubstep("repair_work_order", h => RepairWorkOrderSystem?.Tick(h));
            // Prompt #799 — evaluate IF/THEN rules after power rebalance.
            _registry.RegisterPerSubstep("logic_gates", h => TickLogicGates());
            _registry.RegisterPerSubstep("hatch_defense", h => HatchDefenseSystem?.Tick(h, PowerNetwork));
            RegisterShelterAirSubsteps();
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
            // Batch systems: hourly disease/sheriff/true-ending; daily scapegoat/blockade/speedrun day.
            _registry.RegisterPerSubstep("batch_systems_hourly", h => TickBatchSystemsHourly(h));
            _registry.RegisterPerSubstep("batch_systems_daily",
                _registry.DayGated("batch_systems", day => TickBatchSystemsDaily(day)));
            // Victory paths: broadcast upload minutes; daily lone-survivor check.
            _registry.RegisterPerSubstep("victory_paths_hourly", h => TickVictoryPathsHourly(h));
            _registry.RegisterPerSubstep("victory_paths_daily",
                _registry.DayGated("victory_paths", day => TickVictoryPathsDaily(day)));
            // Map hazards: acid geyser hourly clock; ashlanche suffocation minutes.
            _registry.RegisterPerSubstep("map_hazards_hourly", h => TickMapHazardsHourly(h));
            // Weather_* special-event trackers: countdown/expiry for triggered storms.
            // Without these an active event never expired and its debuff never lifted.
            _registry.RegisterPerSubstep("weather_events_hourly", h => TickWeatherEventsHourly(h));
            // Prompts #319–#325 — Section X new weather events (5 additions).
            _registry.RegisterPerSubstep("new_weather_hourly", h => TickNewWeatherSystemsHourly(h));
            _registry.RegisterPerSubstep("weather_events_daily",
                _registry.DayGated("weather_events", day => TickWeatherEventsDaily(day)));
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

        private void RegisterShelterAirSubsteps()
        {
            _registry.RegisterPerSubstep("atmosphere", h =>
            {
                if (AtmosphereSystem == null) return;
                if (ShelterPerks != null)
                    AtmosphereSystem.VentilationClearMultiplier =
                        ShelterPerks.GetVentilationClearMultiplier(Survivors);
                AtmosphereSystem.Tick(h, PowerNetwork, Shelter);
            });
            // REPROMOTE-Hazard-001 — read current shelter air only after atmosphere advances.
            _registry.RegisterPerSubstep("hazard_methane", TickHazardMethaneFromShelterAir);
        }

        private void RegisterNeedsPsycheSubsteps()
        {
            _registry.RegisterPerSubstep("needs", h => NeedsSystem?.Tick(h));
            _registry.RegisterDaily("bunker_rationing", day =>
                BunkerRationingSystem?.ApplyDailyRations(day, Survivors, NeedsSystem));
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
            _registry.RegisterPerSubstep("personal_quests_morale", TickPersonalQuestsMorale);
            // #249–#256: hoarder internal theft from main storage into personal stash.
            _registry.RegisterPerSubstep("personal_quests_hoarder_theft", TickHoarderTheft);
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

        private void TickPersonalQuestsMorale(float h)
        {
            if (PersonalQuests == null || Survivors == null) return;
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;
            PersonalQuests.WatchMoraleAll(Survivors, day);
            for (int i = 0; i < Survivors.Count; i++)
                PersonalQuests.ApplyRoomMoraleFloor(Survivors[i], Survivors);
        }

        /// <summary>#249–#256: ~5% chance per game-hour per Selfish hoarder to steal one unit into their personal stash.</summary>
        private void TickHoarderTheft(float h)
        {
            if (PersonalQuests == null || Survivors == null || Inventory == null || h <= 0f) return;

            var rng = _mentalBreakRng ?? new System.Random(256);
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                if (!PersonalQuests.HasSelfish(sv)) continue;
                if (rng.NextDouble() > 0.05 * h) continue;
                TryStealOneUnit(sv, rng);
            }
        }

        private void TryStealOneUnit(Survivor sv, System.Random rng)
        {
            if (Inventory.Slots == null || Inventory.Slots.Count == 0) return;

            int start = rng.Next(0, Inventory.Slots.Count);
            for (int k = 0; k < Inventory.Slots.Count; k++)
            {
                var slot = Inventory.Slots[(start + k) % Inventory.Slots.Count];
                if (slot?.Item == null || slot.Amount <= 0) continue;
                string itemId = slot.Item.id;
                if (string.IsNullOrEmpty(itemId)) continue;
                if (!Inventory.Remove(slot.Item, 1)) continue;
                PersonalQuests.TryStealToPersonalInventory(sv, itemId);
                return;
            }
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
            // "Into the Ash" quest chains — evaluates time-gated quest stage triggers each day.
            _registry.RegisterPerSubstep("expansion_quest_daily",
                _registry.DayGated("expansion_quests", day => ExpansionQuests?.TickDaily()));
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
            // Prompt #904 — word of the bunker spreading is the only part of the
            // pianist encounter that happens on the clock rather than on a choice.
            _registry.RegisterPerSubstep("pianist_daily",
                _registry.DayGated("pianist", RegisterTickPianistDaily));
            // Audit H-6e: PetSystem.TryTameWastelandAnimal existed but nothing ever
            // called it — a Zoonotic Expert could never actually tame an animal.
            _registry.RegisterPerSubstep("pet_taming_daily",
                _registry.DayGated("pet_taming", RegisterTickPetTamingDaily));
            // New-content quests: evaluates day-gate / roster / resource trigger
            // conditions once per day and drives the quest daily counters.
            // Outer and DayGated keys share the canonical name so the C-1
            // audit resolves the GameBootstrap.QuestRegistry property.
            _registry.RegisterPerSubstep("quest_registry",
                _registry.DayGated("quest_registry", TickQuestTriggersDaily));

            // ── Protocol Zero — permafrost expansion systems ─────────
            _registry.RegisterPerSubstep("thermal_grid", h =>
                ThermalGrid?.Tick(h, NeedsSystem, Survivors));
            _registry.RegisterPerSubstep("ash_tide", h =>
                AshTideScheduler?.Tick(h, TimeSystem?.CurrentDay ?? 1));
            _registry.RegisterPerSubstep("atmosphere_toxicity", h =>
            {
                int aliveCount = 0;
                if (Survivors != null)
                    for (int i = 0; i < Survivors.Count; i++)
                        if (Survivors[i] != null && Survivors[i].IsAlive) aliveCount++;
                bool heaterOn = Shelter?.GetModule("heater")?.IsOperational ?? false;
                AtmosphereToxicity?.Tick(h, aliveCount, heaterOn);
            });
            _registry.RegisterPerSubstep("convoy_logistics", h =>
            {
                bool isBlizzard = WeatherSystem?.Current == WeatherKind.Blizzard;
                ConvoyLogistics?.Tick(h, TimeSystem?.CurrentDay ?? 1, isBlizzard,
                    NeedsSystem,
                    id => Survivors?.Find(s => s.Id == id));
            });

            // ── Expansion II Addendum: Black Aquifer systems ─────────
            _registry.RegisterPerSubstep("hydrostatic_pressure", h =>
                HydrostaticPressure?.Tick(h));
            _registry.RegisterPerSubstep("hydrostatic_daily",
                _registry.DayGated("hydrostatic_daily", day =>
                    HydrostaticPressure?.TickDaily(day)));
            _registry.RegisterPerSubstep("tunneling_stress", h =>
                TunnelingStress?.Tick(h, _mentalBreakRng));
            _registry.RegisterPerSubstep("mycelium_network", h =>
                MyceliumNetwork?.Tick(h));
            _registry.RegisterPerSubstep("mycelium_daily",
                _registry.DayGated("mycelium_daily", day =>
                    MyceliumNetwork?.TickDaily(Survivors)));

            // ── Expansion III: Dead Hand systems ─────────────────────
            _registry.RegisterPerSubstep("uxo_field", h =>
                UXOField?.Tick(h, _mentalBreakRng));
            _registry.RegisterPerSubstep("automated_threats", h =>
            {
                float sig = UXOField?.GlobalAcousticSignature ?? 0f;
                AutomatedThreats?.Tick(h, sig, _mentalBreakRng);
            });
            _registry.RegisterPerSubstep("automated_threats_daily",
                _registry.DayGated("automated_threats_daily", day =>
                {
                    float sig = UXOField?.GlobalAcousticSignature ?? 0f;
                    AutomatedThreats?.TickDaily(sig, null, _mentalBreakRng);
                }));
            _registry.RegisterPerSubstep("electromagnetic_decay_daily",
                _registry.DayGated("electromagnetic_decay", day =>
                    ElectromagneticDecay?.TickDaily(_mentalBreakRng)));

            // ── Expansion IV: Chronos Decay & Lethe Protocol ─────────
            // Structural entropy: hour tick for ongoing carbonation/corrosion.
            // DailyTickExpansion4 wraps the 24h bulk tick + generational DailyTick.
            _registry.RegisterPerSubstep("expansion4_daily",
                _registry.DayGated("expansion4", DailyTickExpansion4));
            // OzoneScourgeSystem is event/query-driven via its WeatherSystem reference;
            // no separate hourly tick registration needed.
        }

        /// <summary>
        /// Section VII new-content batch. Grief, shelter degradation and ash
        /// accumulation carry their own LastSimulatedDay guards, but they are
        /// still routed through DayGated so the once-per-day cadence is
        /// explicit and auditable. Sleep deprivation and calorie accounting
        /// loop the roster once per day. Disease mutation and noise discipline
        /// are purely host-driven (pill administered / source toggled).
        /// </summary>
        private void RegisterSection7Systems()
        {
            // Outer and DayGated keys share the canonical snake_case name (same
            // convention as "house_to_bunker") so the C-1 audit resolves each
            // GameBootstrap property without an alias-table entry.
            _registry.RegisterPerSubstep("sleep_deprivation",
                _registry.DayGated("sleep_deprivation", day => TickSleepDeprivationDaily(day)));
            _registry.RegisterPerSubstep("grief",
                _registry.DayGated("grief", day => Grief?.Tick()));
            _registry.RegisterPerSubstep("shelter_degradation",
                _registry.DayGated("shelter_degradation", day => ShelterDegradation?.Tick()));
            _registry.RegisterPerSubstep("ash_accumulation",
                _registry.DayGated("ash_accumulation", day => AshAccumulation?.Tick()));
            _registry.RegisterPerSubstep("calorie_accounting",
                _registry.DayGated("calorie_accounting", day => TickCalorieAccountingDaily(day)));
            _registry.RegisterEventDriven("disease_mutation");
            _registry.RegisterEventDriven("noise_discipline");
        }

        private void TickSleepDeprivationDaily(int day)
        {
            if (SleepDeprivation == null || Survivors == null) return;
            float cycleHour = TimeSystem != null ? TimeSystem.CurrentHour : 8f;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                // No explicit hours-slept tracker exists yet; derive last
                // night's rest from end-of-day fatigue (0 rested .. 100 spent).
                float hoursSlept = Mathf.Clamp01(1f - sv.Needs.Fatigue / 100f) * 8f;
                SleepDeprivation.OnEndOfDay(sv, day, cycleHour, hoursSlept);
            }
        }

        private void TickCalorieAccountingDaily(int day)
        {
            if (CalorieAccounting == null || Survivors == null) return;
            for (int i = 0; i < Survivors.Count; i++)
                CalorieAccounting.Tick(Survivors[i], day);
        }

        /// <summary>
        /// Prompt #904 — once Matej has been told about the bunker, a small daily
        /// chance that someone he passed it on to walks up to the hatch. Nothing
        /// drove this roll before, so telling him set a flag and stopped there.
        /// </summary>
        private void RegisterTickPianistDaily(int day)
        {
            if (Pianist == null || Survivors == null) return;
            if (!Pianist.RollRefugeeArrival(CreateSaltedRng(_worldSeed, "pianist_refugee"))) return;

            var refugee = new Survivor
            {
                Id = "sv_pianist_refugee",
                DisplayName = "Refugee",
                CurrentRoomId = "entry"
            };
            Survivors.Add(refugee);
            NeedsSystem?.Register(refugee);
            RadiationSystem?.Register(refugee);
            GameLog.Log(
                $"[Pianist] Someone {Encounter_Pianist.PianistName} told about the bunker " +
                $"has found the hatch on day {day}.");
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

        private void RegisterTickPetTamingDaily(int day)
        {
            if (PetSystem == null || Survivors == null) return;
            var rng = _mentalBreakRng ?? new System.Random(day + 217);
            var tamed = PetSystem.TryTameWastelandAnimalDaily(Survivors, rng);
            if (tamed == null) return;

            var rooms = Shelter?.Rooms;
            if (rooms != null && rooms.Count > 0 && rooms[0] != null)
                PetSystem.SetPetRoom(tamed, rooms[0].RoomId);

            GameLog.Log($"[Zoonotic Expert] A {tamed.DisplayName.ToLowerInvariant()} has been tamed. It calls the shelter home now.");
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
                IndoorTemperatureC = indoor,
                NeedsSystem = NeedsSystem
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
                "medical_perks", "expedition_perks", "night_scavenge",
                // REPROMOTE-001 — caravan weather pricing fires on trade quote, not hour tick:
                "npc_passive_trader",
                // REPROMOTE-Encounter-001 — class roadblock resolves from expedition psychology:
                "encounter_roadblock",
                "map_hazard_venus_trap",
                // Prompt #902 — frozen survivor resolves on expedition looting arrival:
                "map_hazard_frozen_survivor",
                // REPROMOTE-Item-001 — keycard find/unlock on expedition looting:
                "item_keycards",
                // Prompts #901/#903 — these resolve from expedition encounter choices.
                // (The pianist also has a daily refugee roll, so it is tick-registered.)
                "dead_letter_office",
                "weather_station",
                // REPROMOTE-Pet-001 — guard dog alert fires on hatch raid resolve start:
                "pet_guard_dog",
                // REPROMOTE-Weapon-001 — HMG defense power read on GetWeaponPower:
                "weapon_hmg",
                // C-1: reactive-only, no autonomous tick -- driven entirely by UI
                // actions (TryEquip/TryUnequip, TryTransferOne).
                "field_gear_loadout", "overflow_crate",
                // Expansion IV: event/query-driven systems (no autonomous tick).
                "lethe_protocol", "ozone_scourge"
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
                "labor_camp", "location_quest", "expansion_quest_chains", "ceiling_collapse", "aesthetics", "ham_radio",
                "polypharmacy", "resilience", "internal_lock", "grief_keepsakes", "mentorship",
                "flashpoint_choreographer", "diary_catalog",
                // C-1: secondary inventory bucket, mirrors "inventory" above.
                "overflow_stash"
            };
            for (int i = 0; i < saveOnly.Length; i++)
                _registry.RegisterSaveOnly(saveOnly[i]);
        }

    }
}
