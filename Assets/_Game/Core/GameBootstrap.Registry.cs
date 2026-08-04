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

            // === Per-substep (every game-hour chunk) ===
            // Environment
            _registry.RegisterPerSubstep("weather", h => WeatherSystem?.Tick(h));
            _registry.RegisterPerSubstep("temperature", h => TemperatureSystem?.Tick(h));
            _registry.RegisterPerSubstep("photoperiod", h => PhotoperiodSystem?.Tick(h));

            // Hatch entrapment + expedition sync
            _registry.RegisterPerSubstep("hatch_entrapment", h =>
            {
                if (HatchEntrapmentSystem != null && WeatherSystem != null && TimeSystem != null)
                {
                    HatchEntrapmentSystem.Tick(h, WeatherSystem.Current, Shelter,
                        _getFactionTrustEffective, _scheduleEventCached, TimeSystem.CurrentDay);
                    SyncHatchExpeditionLock();
                }
            });

            // Shelter + power
            _registry.RegisterPerSubstep("shelter", h => Shelter?.Tick(h));
            _registry.RegisterPerSubstep("power_network", h =>
            {
                if (PowerNetwork != null)
                {
                    PowerNetwork.Tick(h, WeatherSystem != null ? WeatherNameOf(WeatherSystem.Current) : null, _tryApplyPedalCostCached);
                    PowerNetwork.ApplyToShelter(Shelter);
                }
            });

            // Hatch defense
            _registry.RegisterPerSubstep("hatch_defense", h => HatchDefenseSystem?.Tick(h, PowerNetwork));

            // Internal Horror
            _registry.RegisterPerSubstep("atmosphere", h => AtmosphereSystem?.Tick(h, PowerNetwork, Shelter));
            _registry.RegisterPerSubstep("corpses", h => CorpseSystem?.Tick(h, Survivors));
            _registry.RegisterPerSubstep("pantry", h => PantrySystem?.Tick(h, _storesRoom));

            // Structural integrity (with weather-driven damage)
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

            // Waste / vermin / jury-rig / freeze-pipe
            _registry.RegisterPerSubstep("waste", h => WasteSystem?.Tick(h, TimeSystem?.CurrentDay ?? 1));
            _registry.RegisterPerSubstep("vermin", h => VerminSystem?.Tick(h, Inventory));
            _registry.RegisterPerSubstep("jury_rig", h => JuryRigSystem?.Tick(h, TimeSystem?.CurrentDay ?? 1));
            _registry.RegisterPerSubstep("freeze_pipe", h => FreezePipeSystem?.Tick(h));

            // Trackers / dead drops / hostage / propaganda
            _registry.RegisterPerSubstep("tracker", h => TrackerSystem?.Tick(h,
                (fid, chance) => HatchDefenseSystem?.SetRaidChanceOverride(fid, chance),
                (eventId, fireDay, originFlag) => EventRunner?.ScheduleEvent(eventId, fireDay, originFlag),
                TimeSystem?.CurrentDay ?? 1));
            _registry.RegisterPerSubstep("dead_drops", h => DeadDropSystem?.Tick(h));
            _registry.RegisterPerSubstep("hostage", h => HostageSystem?.Tick(h));
            _registry.RegisterPerSubstep("propaganda", h => PropagandaSystem?.Tick(h,
                (fid, delta) => EconomySystem?.ModifyTrust(fid, delta),
                (fid, reduction) => HatchDefenseSystem?.AdjustRaidChance(fid, -reduction)));

            // Scapegoat
            _registry.RegisterPerSubstep("scapegoat", h => ScapegoatSystem?.Tick(h, WeatherSystem.Current,
                (eventId, fireDay, flag) => EventRunner?.ScheduleEvent(eventId, fireDay, flag),
                TimeSystem?.CurrentDay ?? 1));

            // Shelter tactical
            bool preDay30 = WorldPhaseSystem != null && !WorldPhaseSystem.HasTriggeredExchange;
            _registry.RegisterPerSubstep("flooding", h => FloodingSystem?.Tick(h,
                WeatherSystem.Current == WeatherKind.Rain, preDay30, Shelter,
                roomId => roomId == "cellar" || roomId == "coal_room"));
            _registry.RegisterPerSubstep("perimeter_trap", h => PerimeterTrapSystem?.Tick(h));
            _registry.RegisterPerSubstep("noise", h => NoiseSystem?.Tick(h));

            // Clothing degradation
            _registry.RegisterPerSubstep("clothing", h => TickClothing(h));

            // Needs + Medical
            _registry.RegisterPerSubstep("needs", h => NeedsSystem?.Tick(h));
            _registry.RegisterPerSubstep("medical", h => MedicalSystem?.Tick(Survivors, h));

            // Mental breaks + traits
            _registry.RegisterPerSubstep("mental_break", h =>
            {
                if (MentalBreakSystem != null && _mentalBreakRng != null)
                    MentalBreakSystem.Tick(h, Survivors, _mentalBreakRng);
            });
            _registry.RegisterPerSubstep("skill_atrophy", h => SkillAtrophy?.Tick(h, Survivors));
            _registry.RegisterPerSubstep("empath", h => EmpathSystem?.Tick(h, Survivors));
            _registry.RegisterPerSubstep("survivor_diaries", h => SurvivorDiaries?.Tick(h, Survivors, TimeSystem?.CurrentDay ?? 1, _mentalBreakRng));
            _registry.RegisterPerSubstep("spatial_psychology", h => SpatialPsychology?.Tick(h, Survivors));
            _registry.RegisterPerSubstep("hallucination", h => HallucinationSystem?.Tick(h, Survivors, _mentalBreakRng));
            _registry.RegisterPerSubstep("addiction", h => Addiction?.Tick(h, Survivors, TimeSystem?.CurrentDay ?? 1));
            _registry.RegisterPerSubstep("phantom_intruders", h => PhantomIntruders?.Tick(h, Survivors, _phantomRng));
            _registry.RegisterPerSubstep("child", h => ChildSystem?.Tick(h, Survivors));

            // Hatch / parley prompts
            _registry.RegisterPerSubstep("hatch_dilemma", h => HatchDilemmaPromptField?.Tick(h));
            _registry.RegisterPerSubstep("parley_offer", h => ParleyOfferPromptField?.Tick(h));

            // Radiation + water
            _registry.RegisterPerSubstep("radiation", h => RadiationSystem?.Tick(h));
            _registry.RegisterPerSubstep("water_economy", h => WaterEconomySystem?.Tick(h, WeatherSystem.Current, TimeSystem?.CurrentDay ?? 1, Shelter, WaterStorage));

            // Black rain
            _registry.RegisterPerSubstep("black_rain", h =>
            {
                if (BlackRainHazardSystem != null && Survivors != null)
                    BlackRainHazardSystem.TickDread(Survivors, IsSurvivorOnExpedition, IsSurvivorHatchListener, h);
            });

            // Crafting / scavenging / expeditions
            _registry.RegisterPerSubstep("crafting", h => CraftingSystem?.Tick(h));
            _registry.RegisterPerSubstep("scavenging", h => ScavengingSystem?.Tick(h));
            _registry.RegisterPerSubstep("expeditions", h => ExpeditionSystem?.Tick(h));

            // Radio tuner
            _registry.RegisterPerSubstep("radio_tuner", h =>
            {
                if (RadioTunerSystem != null && Shelter != null)
                {
                    var radioModule = Shelter.GetModule("radio");
                    if (radioModule != null && radioModule.IsOperational && radioModule.Fuel > 0f)
                        RadioTunerSystem.Tick(h, WeatherSystem.Current, TimeSystem?.CurrentDay ?? 1);
                }
            });

            // === Daily ticks (once per game-day via DayGated) ===
            _registry.RegisterPerSubstep("deserter_daily", _registry.DayGated("deserter", day =>
            {
                DeserterSystem?.TickDaily(Shelter,
                    eventId => EventRunner?.ScheduleEvent(eventId, day + 1, null));
            }));
            _registry.RegisterPerSubstep("ecosystem_daily", _registry.DayGated("ecosystem", day =>
            {
                float outdoorRad = RadiationSystem != null ? 15f : 0f;
                bool exchangeTriggered = WorldPhaseSystem != null && WorldPhaseSystem.HasTriggeredExchange;
                EcosystemSystem?.TickDaily(outdoorRad, exchangeTriggered);
            }));
            _registry.RegisterPerSubstep("amputation_daily", _registry.DayGated("amputation", day =>
                AmputationSystem?.TickDaily(Survivors)));
            _registry.RegisterPerSubstep("scurvy_daily", _registry.DayGated("scurvy", day =>
                ScurvySystem?.TickDaily(Survivors)));
            _registry.RegisterPerSubstep("mutagenesis_daily", _registry.DayGated("mutagenesis", day =>
                Mutagenesis?.Evaluate(Survivors)));
            _registry.RegisterPerSubstep("hatch_visibility_daily", _registry.DayGated("hatch_visibility", day =>
                HatchVisibilitySystem?.TickDaily()));

            // House-to-bunker: per-day artillery (day-gated, Civil-War only, pre-Day-30)
            _registry.RegisterPerSubstep("house_to_bunker", _registry.DayGated("house_to_bunker", day =>
            {
                if (HouseToBunkerSystem == null || WorldPhaseSystem == null) return;
                if (day < WorldPhaseSystem.FlashpointDay
                    && WorldPhaseSystem.CurrentPhase == AtomicWar._Game.Survivors.WorldPhase.CivilWar)
                    HouseToBunkerSystem.ApplyArtilleryDamage();
                if (day >= WorldPhaseSystem.FlashpointDay && !HouseToBunkerSystem.HouseDestroyed
                    && WorldPhaseSystem.HasTriggeredExchange)
                    HouseToBunkerSystem.CollapseHouse();
                if (ExpeditionSystem != null)
                    ExpeditionSystem.HatchBlocksExpeditions = HouseToBunkerSystem.HatchBlocked;
                if (Shelter != null)
                    Shelter.OverworldShieldingBonus = HouseToBunkerSystem.GetEffectiveShielding();
            }));

            // H-4 SystemWiring daily pass (idempotent on same day)
            _registry.RegisterPerSubstep("system_wiring", _registry.DayGated("system_wiring", day =>
            {
                if (_systemWiring != null && TimeSystem != null)
                {
                    _systemWiring.WireDaily(new SystemWiring.DailyContext
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
                        Rng = new System.Random(_worldSeed)
                    });
                }
            }));

            // Mutagenesis continuous tick (runs per-substep, not day-gated)
            _registry.RegisterPerSubstep("mutagenesis_tick", h => Mutagenesis?.Tick(h, Survivors));

            // === Event-driven systems (ticked via AI actions or EventBus, not here) ===
            _registry.RegisterEventDriven("excavation");
            _registry.RegisterEventDriven("hidden_storage");
            _registry.RegisterEventDriven("tunneling");
            _registry.RegisterEventDriven("material_shielding");
            _registry.RegisterEventDriven("airlock");
            _registry.RegisterEventDriven("escape_hatch");
            _registry.RegisterEventDriven("compost");       // also in system_wiring daily
            _registry.RegisterEventDriven("sterilization");
            _registry.RegisterEventDriven("chelation");     // also in system_wiring daily
            _registry.RegisterEventDriven("wind_turbine");
            _registry.RegisterEventDriven("antibiotic_resist");
            _registry.RegisterEventDriven("hauling");
            _registry.RegisterEventDriven("weapon_maint");
            _registry.RegisterEventDriven("triage");
            _registry.RegisterEventDriven("scrap_weapon");

            // === Save-only systems (state persisted, no live tick) ===
            _registry.RegisterSaveOnly("knowledge_map");
            _registry.RegisterSaveOnly("inventory");
            _registry.RegisterSaveOnly("belief");
            _registry.RegisterSaveOnly("world_phase");
            _registry.RegisterSaveOnly("economy");
            _registry.RegisterSaveOnly("water_storage");
            _registry.RegisterSaveOnly("generated_map");
            _registry.RegisterSaveOnly("faction_radio_intercepts");
            _registry.RegisterSaveOnly("journal");
            _registry.RegisterSaveOnly("victory_project");
            _registry.RegisterSaveOnly("event_runner");
            _registry.RegisterSaveOnly("suspicion_tracker");
            _registry.RegisterSaveOnly("lifeboat");
            _registry.RegisterSaveOnly("sabotaged_cache");
            _registry.RegisterSaveOnly("shifting_hotspot");
            _registry.RegisterSaveOnly("faction_raid_plan");
            _registry.RegisterSaveOnly("debt_collector");
            _registry.RegisterSaveOnly("ghost_station");
            _registry.RegisterSaveOnly("cartography");
            _registry.RegisterSaveOnly("bicycle");
            _registry.RegisterSaveOnly("flooded_node");
            _registry.RegisterSaveOnly("blood_transfusion");
            _registry.RegisterSaveOnly("cult_moral");
            _registry.RegisterSaveOnly("labor_camp");
            _registry.RegisterSaveOnly("location_quest");  // also in system_wiring daily
            _registry.RegisterSaveOnly("ceiling_collapse"); // also in system_wiring daily
            _registry.RegisterSaveOnly("aesthetics");       // also in system_wiring daily
            _registry.RegisterSaveOnly("ham_radio");        // also in system_wiring daily
            _registry.RegisterSaveOnly("polypharmacy");      // also in system_wiring daily
            _registry.RegisterSaveOnly("resilience");
            _registry.RegisterSaveOnly("internal_lock");
            _registry.RegisterSaveOnly("grief_keepsakes");
            _registry.RegisterSaveOnly("mentorship");
            _registry.RegisterSaveOnly("flashpoint_choreographer");

            // H-5: Verify no system is constructed but unticked.
            VerifyAllSystemsRegistered();

            // L-1: Warn on unassigned ScriptableObject references so designers
            // catch missing inspector assignments before they become silent failures.
            VerifyCriticalScriptableObjects();
        }

    }
}
