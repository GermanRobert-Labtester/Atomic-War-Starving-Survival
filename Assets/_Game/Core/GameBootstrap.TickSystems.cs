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
        private void TickSystems(float gameHours)
        {
            if (gameHours <= 0f) return;
            if (_mentalBreakRng == null) WarmDayTickCaches();

            TickEnvironmentAndHatch(gameHours);
            TickNeedsMedicalAndPsyche(gameHours);
            TickRadiationWaterAndCraft(gameHours);
            TickAiWave(gameHours);
            TickEventsAndJournal(gameHours);
        }

        private void TickEnvironmentAndHatch(float gameHours)
        {
            int currentDay = TimeSystem != null ? TimeSystem.CurrentDay : 1;

            // Environment
            WeatherSystem.Tick(gameHours);
            TemperatureSystem.Tick(gameHours);
            PhotoperiodSystem.Tick(gameHours);

            // Prompt #48 — continuous extreme weather seals the hatch.
            if (HatchEntrapmentSystem != null && WeatherSystem != null)
            {
                int day = currentDay;
                HatchEntrapmentSystem.Tick(
                    gameHours,
                    WeatherSystem.Current,
                    Shelter,
                    _getFactionTrustEffective,
                    _scheduleEventCached,
                    day);
                SyncHatchExpeditionLock();
            }

            // Shelter
            Shelter.Tick(gameHours);

            // Power grid (fuel burn, CO, pedaling, load-shed) then push to modules
            if (PowerNetwork != null)
            {
                string weatherName = WeatherSystem != null
                    ? WeatherNameOf(WeatherSystem.Current)
                    : null;
                PowerNetwork.Tick(gameHours, weatherName, _tryApplyPedalCostCached);
                PowerNetwork.ApplyToShelter(Shelter);
            }

            // Hatch defense: outdoor generator noise + periodic post-Day-30 raid rolls
            HatchDefenseSystem?.Tick(gameHours, PowerNetwork);

            // Internal Horror — fire/O2/CO/humidity, corpse rot, pantry rust
            AtmosphereSystem?.Tick(gameHours, PowerNetwork, Shelter);
            CorpseSystem?.Tick(gameHours, Survivors);
            PantrySystem?.Tick(gameHours, _storesRoom);

            // Prompt #49 — structural integrity dust leaks + cave-in checks.
            // Apply damage from severe weather: FalloutStorms (post-Day30) and
            // Blizzards (pre-Day30 mortar strikes are weather-driven too).
            if (StructuralIntegrity != null && WeatherSystem != null)
            {
                var weather = WeatherSystem.Current;
                if (weather == WeatherKind.FalloutStorm)
                {
                    StructuralIntegrity.ApplyDamage(
                        StructuralIntegritySystem.FalloutStormDamagePerHour * gameHours,
                        "fallout_storm");
                }
                else if (weather == WeatherKind.Blizzard && WorldPhaseSystem != null
                    && WorldPhaseSystem.CurrentPhase == AtomicWar._Game.Survivors.WorldPhase.CivilWar)
                {
                    // Pre-Day30: blizzards include mortar shelling that shakes the ceiling.
                    StructuralIntegrity.ApplyDamage(
                        StructuralIntegritySystem.MortarStrikeDamage * 0.3f * gameHours,
                        "mortar_strike");
                }
                StructuralIntegrity.Tick(gameHours, Shelter);
            }

            // Prompt #50 — waste generation + hygiene decay
            WasteSystem?.Tick(gameHours, currentDay);

            // Prompt #51 — vermin growth + food theft + contamination
            VerminSystem?.Tick(gameHours, Inventory);

            // Prompt #52 — jury-rig catastrophic failure rolls
            JuryRigSystem?.Tick(gameHours, currentDay);

            // Prompt #53 — freeze pipe checks
            FreezePipeSystem?.Tick(gameHours);

            // Prompt #71 — tracker: footprints in ash countdown.
            TrackerSystem?.Tick(gameHours,
                setFactionRaidChance: (factionId, chance) =>
                {
                    if (HatchDefenseSystem != null)
                        HatchDefenseSystem.SetRaidChanceOverride(factionId, chance);
                },
                scheduleEvent: (eventId, fireDay, originFlag) =>
                    EventRunner?.ScheduleEvent(eventId, fireDay, originFlag),
                currentDay: TimeSystem != null ? TimeSystem.CurrentDay : 1);

            // Prompt #72 — dead drops: resolve timers.
            DeadDropSystem?.Tick(gameHours);

            // Prompt #73 — hostage expiration countdown.
            HostageSystem?.Tick(gameHours);

            // Prompt #74 — propaganda effect fade.
            PropagandaSystem?.Tick(gameHours,
                modifyFactionTrust: (fid, delta) => EconomySystem?.ModifyTrust(fid, delta),
                reduceRaidChance: (fid, reduction) =>
                {
                    if (HatchDefenseSystem != null)
                        HatchDefenseSystem.AdjustRaidChance(fid, -reduction);
                });

            // Prompt #75 — spy sabotage countdown (daily).
            if (DeserterSystem != null && TimeSystem != null)
            {
                int day = TimeSystem.CurrentDay;
                if (_lastDeserterDay != day)
                {
                    _lastDeserterDay = day;
                    DeserterSystem.TickDaily(Shelter,
                        scheduleEvent: (eventId) =>
                            EventRunner?.ScheduleEvent(eventId, day + 1, null));
                }
            }

            // Prompt #76 — weather scapegoat tracking.
            ScapegoatSystem?.Tick(gameHours, WeatherSystem.Current,
                scheduleEvent: (eventId, fireDay, flag) =>
                    EventRunner?.ScheduleEvent(eventId, fireDay, flag),
                currentDay: TimeSystem != null ? TimeSystem.CurrentDay : 1);

            // Prompt #79 — mutated ecosystem daily advance.
            if (EcosystemSystem != null && TimeSystem != null)
            {
                int day = TimeSystem.CurrentDay;
                if (_lastEcosystemDay != day)
                {
                    _lastEcosystemDay = day;
                    float outdoorRad = RadiationSystem != null ? 15f : 0f;
                    bool exchangeTriggered = WorldPhaseSystem != null
                        && WorldPhaseSystem.HasTriggeredExchange;
                    EcosystemSystem.TickDaily(outdoorRad, exchangeTriggered);
                }
            }

            // Prompt #79–#84 — house-to-bunker: artillery during Civil War (Day 1-29).
            if (HouseToBunkerSystem != null && TimeSystem != null)
            {
                int day = TimeSystem.CurrentDay;
                if (_lastHouseDay != day && day < WorldPhaseSystem.FlashpointDay)
                {
                    _lastHouseDay = day;
                    // Only during Civil War phase, pre-Day 30.
                    if (WorldPhaseSystem != null
                        && WorldPhaseSystem.CurrentPhase == AtomicWar._Game.Survivors.WorldPhase.CivilWar)
                    {
                        HouseToBunkerSystem.ApplyArtilleryDamage();
                    }
                }

                // Day 30 Flashpoint: collapse the house.
                if (day >= WorldPhaseSystem.FlashpointDay && !HouseToBunkerSystem.HouseDestroyed
                    && WorldPhaseSystem != null && WorldPhaseSystem.HasTriggeredExchange)
                {
                    HouseToBunkerSystem.CollapseHouse();
                }

                // Block expeditions if hatch is blocked by debris.
                if (ExpeditionSystem != null)
                {
                    ExpeditionSystem.HatchBlocksExpeditions = HouseToBunkerSystem.HatchBlocked;
                }

                // Sync overworld shielding to Shelter.
                if (Shelter != null)
                {
                    Shelter.OverworldShieldingBonus = HouseToBunkerSystem.GetEffectiveShielding();
                }
            }

            // Prompts #119–#128 — Shelter tactical systems ticks.
            bool preDay30 = WorldPhaseSystem != null && !WorldPhaseSystem.HasTriggeredExchange;
            FloodingSystem?.Tick(gameHours, WeatherSystem.Current == WeatherKind.Rain, preDay30, Shelter,
                roomId => roomId == "cellar" || roomId == "coal_room");
            PerimeterTrapSystem?.Tick(gameHours);
            NoiseSystem?.Tick(gameHours);
            if (HatchVisibilitySystem != null && TimeSystem != null && _lastHatchVisDay != TimeSystem.CurrentDay)
            { _lastHatchVisDay = TimeSystem.CurrentDay; HatchVisibilitySystem.TickDaily(); }

            // Prompt #119–#178 — audit C-1 fix. Wired daily pass for systems
            // added in the most recent push that previously sat dead-state.
            // Idempotent on the same day; no per-tick allocations.
            if (TimeSystem != null && _systemWiring != null)
            {
                _systemWiring.WireDaily(new SystemWiring.DailyContext
                {
                    CurrentDay = TimeSystem.CurrentDay,
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

            // Clothing degradation: per-hour, driven by AtmosphericSystem humidity.
            // Bind once so the wiring is visible from this file (audit C-1).
            TickClothing(gameHours);

        }

        private void TickNeedsMedicalAndPsyche(float gameHours)
        {
            int currentDay = TimeSystem != null ? TimeSystem.CurrentDay : 1;

            // Needs
            NeedsSystem.Tick(gameHours);

            // Medical triage — Health pressure from active afflictions
            MedicalSystem?.Tick(Survivors, gameHours);

            // Prompt #55 — blood typing (no per-tick work; transfusion is event-driven).
            // Prompt #56 — phantom pain daily roll.
            if (AmputationSystem != null && TimeSystem != null)
            {
                // Roll once per day for phantom pain.
                int day = TimeSystem.CurrentDay;
                if (_lastPhantomPainDay != day)
                {
                    _lastPhantomPainDay = day;
                    AmputationSystem.TickDaily(Survivors);
                }
            }

            // Prompt #57 — scurvy daily advance.
            if (ScurvySystem != null && TimeSystem != null)
            {
                int day = TimeSystem.CurrentDay;
                if (_lastScurvyDay != day)
                {
                    _lastScurvyDay = day;
                    ScurvySystem.TickDaily(Survivors);
                }
            }

            // Prompt #60 — mutagenesis evaluate + tick.
            if (Mutagenesis != null && TimeSystem != null)
            {
                int day = TimeSystem.CurrentDay;
                if (_lastMutagenesisDay != day)
                {
                    _lastMutagenesisDay = day;
                    Mutagenesis.Evaluate(Survivors);
                }
                Mutagenesis.Tick(gameHours, Survivors);
            }

            // Mental breaks: low-morale tracking, break rolls, BingeEater
            // consumption, ViolentParanoia sabotage, passive morale drain
            // to other survivors, and natural cure progress.
            if (MentalBreakSystem != null)
            {
                MentalBreakSystem.Tick(gameHours, Survivors, _mentalBreakRng);
            }

            // Prompt #10 — Skill Atrophy: morale < 20 for 14 days → skill downgrade.
            SkillAtrophy?.Tick(gameHours, Survivors);

            // Prompt #8 — Empath coupling: Empath's morale tracks bunker average.
            EmpathSystem?.Tick(gameHours, Survivors);

            // Prompt #61 — Survivor Diaries & Privacy Violations.
            SurvivorDiaries?.Tick(gameHours, Survivors, currentDay, _mentalBreakRng);

            // Prompt #63 — Spatial Psychology Traits (Claustrophobia / Agoraphobia).
            SpatialPsychology?.Tick(gameHours, Survivors);

            // Prompt #65 — UI Hallucinations & Phantom Utility Actions.
            HallucinationSystem?.Tick(gameHours, Survivors, _mentalBreakRng);

            // Prompt #7 — Addiction & Withdrawal: dose counting, withdrawal drains, panic destruction.
            Addiction?.Tick(gameHours, Survivors, currentDay);

            // Prompt #6 — Phantom Intruders: fake hatch breach when Anxiety+Fatigue max out.
            PhantomIntruders?.Tick(gameHours, Survivors, _phantomRng);

            // Prompt #9 — Child: Hope buff, rations consumption, death check.
            ChildSystem?.Tick(gameHours, Survivors);

            // Hatch-dilemma prompt: advance the timeout. On expiry the
            // prompt auto-resolves with ForceDeconOutside.
            HatchDilemmaPromptField?.Tick(gameHours);
            ParleyOfferPromptField?.Tick(gameHours);

        }

        private void TickRadiationWaterAndCraft(float gameHours)
        {
            // Radiation
            RadiationSystem.Tick(gameHours);
            // Cult of the Glow: rad drop across healthy ceiling → hatch raid cascade.
            EconomySystem?.NotifyPartyRadiationChanged();

            // Water economy: catchment collection + purifier conversion queue.
            WaterEconomySystem?.Tick(gameHours, WeatherSystem.Current, TimeSystem.CurrentDay, Shelter, WaterStorage);

            // Prompt #11 — Black Rain dread for outdoor scavengers + hatch listeners.
            if (BlackRainHazardSystem != null && Survivors != null)
            {
                BlackRainHazardSystem.TickDread(
                    Survivors,
                    isOutdoor: IsSurvivorOnExpedition,
                    isHatchListener: IsSurvivorHatchListener,
                    gameHours);
            }

            // Crafting
            CraftingSystem.Tick(gameHours);

            // Scavenging & Expeditions
            ScavengingSystem?.Tick(gameHours);
            ExpeditionSystem?.Tick(gameHours);
            
            // Radio Tuner (intel extraction)
            if (RadioTunerSystem != null && Shelter != null)
            {
                var radioModule = Shelter.GetModule("radio");
                if (radioModule != null && radioModule.IsOperational && radioModule.Fuel > 0f)
                {
                    RadioTunerSystem.Tick(gameHours, WeatherSystem.Current, TimeSystem.CurrentDay);
                }
            }

        }

        private void TickAiWave(float gameHours)
        {
            float indoorTemp = TemperatureSystem != null
                ? TemperatureSystem.GetIndoorTemperature(Shelter)
                : 15f;

            // AI (evaluate per survivor, every EvaluationInterval)
            UtilityAI.Tick(gameHours * TimeSystem.SecondsPerGameHour);
            if (UtilityAI.ShouldEvaluate())
            {
                // Fresh sleep-wave occupancy so capacity is per evaluation pass.
                SleepQualitySystem.ResetBedOccupancy(Shelter);
                // Guards re-assigned each AI wave (stale posts clear).
                HatchDefenseSystem?.ClearGuards();
                int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;
                float raidThreat = ComputeAiRaidThreat(day);

                int scrapDeficit = WorkbenchSystem != null
                    ? WorkbenchSystem.GetCriticalElectronicScrapDeficit()
                    : 0;
                float junkUrgency = scrapDeficit > 0
                    ? Mathf.Clamp01(scrapDeficit / 4f)
                    : 0f;
                bool needsScrap = scrapDeficit > 0;
                bool isStorm = WeatherSystem.Current == WeatherKind.FalloutStorm
                    || WeatherSystem.Current == WeatherKind.BlackRain;
                bool growLight = Shelter != null && Shelter.IsGrowLightActive;

                // Reuse one AIContext + shared RNG across the whole AI wave.
                var context = _aiContextScratch;
                context.Shelter = Shelter;
                context.Inventory = Inventory;
                context.Random = _aiRng;
                context.IsFalloutStorm = isStorm;
                context.AmbientRadRate = 5f;
                context.GrowLightActive = growLight;
                context.OnRequestSurvey = RequestSurveyForSurvivor;
                context.BeliefSystem = BeliefSystem;
                context.MedicalSystem = MedicalSystem;
                context.MentalBreak = MentalBreakSystem;
                context.PowerNetwork = PowerNetwork;
                context.HatchDefense = HatchDefenseSystem;
                context.RaidThreatLevel = raidThreat;
                context.CurrentDay = day;
                context.IndoorTemperatureC = indoorTemp;
                context.SleepRoomId = SleepQualitySystem.DefaultSleepRoomId;
                context.AreRoomsAdjacent = Shelter != null ? Shelter.AreRoomsAdjacent : null;
                context.WaterStorage = WaterStorage;
                context.NeedsElectronicScrapForCriticalRepair = needsScrap;
                context.JunkScavengeUrgency = junkUrgency;
                context.RadiationSystem = RadiationSystem;
                context.VerminSystem = VerminSystem;
                context.WasteSystem = WasteSystem;
                context.JuryRigSystem = JuryRigSystem;
                context.StructuralIntegrity = StructuralIntegrity;
                context.GetSurvivors = _getSurvivorsCached;
                // Audit C-3: bindings for the AI actions that drive the
                // C-1-wired systems. None of these allocate per substep
                // (they are simple field assignments on the scratch
                // AIContext) so the day-tick GC profile is preserved.
                context.ExcavationSystem = ExcavationSystem;
                context.FloodingSystem = FloodingSystem;
                context.HiddenStorageSystem = HiddenStorageSystem;
                context.CeilingCollapseSystem = CeilingCollapseSystem;
                context.PerimeterTrapSystem = PerimeterTrapSystem;
                context.TunnelingSystem = TunnelingSystem;
                context.MaterialShieldingSystem = MaterialShieldingSystem;
                context.EscapeHatchSystem = EscapeHatchSystem;
                context.AirlockSystem = AirlockSystem;
                context.CompostSystem = CompostSystem;
                context.SterilizationSystem = SterilizationSystem;
                context.ChelationSystem = ChelationSystem;
                context.WindTurbineSystem = WindTurbineSystem;
                context.HaulingSystem = HaulingSystem;

                for (int si = 0; si < Survivors.Count; si++)
                {
                    var sv = Survivors[si];
                    if (sv == null || !sv.IsAlive) continue;
                    float mapUncertainty = GetMapUncertaintyFor(sv);
                    BeliefSystem.Tick(sv, mapUncertainty, gameHours);

                    context.Survivor = sv;
                    context.IsListless = sv.IsListless;
                    context.MapUncertainty = mapUncertainty;
                    context.IsAnxious = sv.HasRadiationAnxietyStatus;
                    context.IsNumb = sv.IsNumb;

                    var action = UtilityAI.SelectAction(context, Actions);
                    action?.Execute(context);

                    // Prompt #7 — track addictive chem consumption
                    if (action != null && Addiction != null)
                    {
                        if (action.id == "action_use_antirad")
                            Addiction.OnItemConsumed(sv, "anti_rad", day);
                    }
                }
            }

        }

        private void TickEventsAndJournal(float gameHours)
        {
            float indoorTemp = TemperatureSystem != null
                ? TemperatureSystem.GetIndoorTemperature(Shelter)
                : 15f;

            // Events (chance per hour)
            var eventContext = BuildEventContext(
                TimeSystem != null ? TimeSystem.CurrentDay : 1,
                TimeSystem != null ? TimeSystem.CurrentHourFloat : 12f,
                indoorTemp);
            EventRunner.Tick(gameHours, eventContext);

            // Internal mysteries: resource-starved Missing Rations pressure.
            SuspicionTracker?.Tick(gameHours, eventContext, EventRunner);

            // Diegetic journal discoveries (first-time atmosphere / rad / storm / etc.)
            if (JournalSystem != null)
                EventRunner.ObserveDiscoveries(JournalSystem, eventContext);

            // Try to trigger an event occasionally
            if (UnityEngine.Random.value < 0.05f) // ~5% chance per hour
            {
                var selectedEvent = EventRunner.SelectEvent(eventContext);
                if (selectedEvent != null)
                {
                    EventRunner.Run(selectedEvent, eventContext);
                }
            }
        }


    }
}
