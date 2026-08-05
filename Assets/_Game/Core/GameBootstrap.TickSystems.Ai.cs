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

        private void TickAiWave(float gameHours)
        {
            // AI (evaluate per survivor, every EvaluationInterval)
            UtilityAI.Tick(gameHours * TimeSystem.SecondsPerGameHour);
            if (!UtilityAI.ShouldEvaluate()) return;

            SleepQualitySystem.ResetBedOccupancy(Shelter);
            HatchDefenseSystem?.ClearGuards();

            int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;
            var context = FillAiContextScratch(day);
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

                // Prompt #179–#181 — action-driven XP / dormant refresh / epiphany.
                if (action != null && SkillProgression != null
                    && !string.IsNullOrEmpty(action.progressionDiscipline))
                {
                    float xp = action.progressionXp > 0f
                        ? action.progressionXp
                        : SkillProgressionSystem.DefaultXpPerAction;
                    SkillProgression.RecordAction(
                        sv, action.progressionDiscipline, xp, day, _aiRng);
                }

                // Prompt #7 — track addictive chem consumption
                if (action != null && Addiction != null && action.id == "action_use_antirad")
                    Addiction.OnItemConsumed(sv, "anti_rad", day);
            }
        }

        private AIContext FillAiContextScratch(int day)
        {
            float indoorTemp = TemperatureSystem != null
                ? TemperatureSystem.GetIndoorTemperature(Shelter)
                : 15f;
            float raidThreat = ComputeAiRaidThreat(day);
            int scrapDeficit = WorkbenchSystem != null
                ? WorkbenchSystem.GetCriticalElectronicScrapDeficit()
                : 0;
            float junkUrgency = scrapDeficit > 0
                ? Mathf.Clamp01(scrapDeficit / 4f)
                : 0f;
            bool isStorm = WeatherSystem.Current == WeatherKind.FalloutStorm
                || WeatherSystem.Current == WeatherKind.BlackRain;

            var context = _aiContextScratch;
            context.Shelter = Shelter;
            context.Inventory = Inventory;
            context.Random = _aiRng;
            context.IsFalloutStorm = isStorm;
            context.AmbientRadRate = 5f;
            context.GrowLightActive = Shelter != null && Shelter.IsGrowLightActive;
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
            context.NeedsElectronicScrapForCriticalRepair = scrapDeficit > 0;
            context.JunkScavengeUrgency = junkUrgency;
            context.RadiationSystem = RadiationSystem;
            context.VerminSystem = VerminSystem;
            context.WasteSystem = WasteSystem;
            context.JuryRigSystem = JuryRigSystem;
            context.StructuralIntegrity = StructuralIntegrity;
            context.GetSurvivors = _getSurvivorsCached;
            BindAiShelterSystems(context);
            return context;
        }

        private void BindAiShelterSystems(AIContext context)
        {
            // Audit C-3: bindings for AI actions that drive C-1-wired systems.
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
            context.ScrapWeaponSystem = ScrapWeaponSystem;
            context.WeaponMaintenanceSystem = WeaponMaintenanceSystem;
            context.CombatPerks = CombatPerks;
            context.SurvivalPerks = SurvivalPerks;
            context.ShelterPerks = ShelterPerks;
            context.MedicalPerks = MedicalPerks;
            context.ExpeditionPerks = ExpeditionPerks;
            context.SocialPerks = SocialPerks;
            context.PersonalQuests = PersonalQuests;
            context.TriageSystem = TriageSystem;
            context.ResilienceSystem = ResilienceSystem;
            context.AntibioticResistSystem = AntibioticResistSystem;
            context.CartographySystem = CartographySystem;
            context.InternalLockSystem = InternalLockSystem;
            context.MentorshipSystem = MentorshipSystem;
            context.Affinity = MentalBreakSystem != null ? MentalBreakSystem.Affinity : null;
            context.GetUnchartedNodeId = ResolveUnchartedNodeId;
        }

        /// <summary>
        /// First map node that is not yet charted (for ChartMap AI action).
        /// Null when cartography has nothing left to process.
        /// </summary>
        private string ResolveUnchartedNodeId()
        {
            if (CartographySystem == null || GeneratedMap == null || GeneratedMap.Nodes == null)
                return null;
            for (int i = 0; i < GeneratedMap.Nodes.Count; i++)
            {
                var node = GeneratedMap.Nodes[i];
                if (node == null || string.IsNullOrEmpty(node.NodeId) || node.IsShelter) continue;
                if (!CartographySystem.IsCharted(node.NodeId))
                    return node.NodeId;
            }
            return null;
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

            // Prompt #180 — use-it-or-lose-it dormant perks (once per day).
            if (SkillProgression != null && TimeSystem != null)
            {
                int day = TimeSystem.CurrentDay;
                if (_lastSkillProgressionDay != day)
                {
                    _lastSkillProgressionDay = day;
                    SkillProgression.TickDaily(day, Survivors);
                    // Prompt #213 — Taskmaster: consecutive high-morale days.
                    SocialPerks?.TickDailyMorale(Survivors, day);
                }
            }

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

    }
}
