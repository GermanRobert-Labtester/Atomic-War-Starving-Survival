using System;
using System.Collections.Generic;
using Ashfall.Core.Campaign;
using Ashfall.Core.Expeditions;
using Godot;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private void RegisterProductionCampaignOwners()
        {
            if (_campaignDay == null) return;

            // Phase 1: Environment, Weather & Base Core
            _campaignDay.Register("holdfast_core", new HoldfastCoreDayOwner(this), phase: 1);
            _campaignDay.Register("maritime_deep_coast", new DeepCoastMaritimeDayOwner(this), phase: 1);
            _campaignDay.Register("power_grid", new PowerGridDayOwner(this), phase: 1);
            _campaignDay.Register("weather_world", new WeatherWorldDayOwner(this), phase: 1);

            // Phase 2: Production, Infrastructure & Survival Basics
            _campaignDay.Register("crafting_production", new CraftingProductionDayOwner(this), phase: 2);
            _campaignDay.Register("greenhouse_foundry", new GreenhouseFoundryDayOwner(this), phase: 2);
            _campaignDay.Register("shelter_facilities", new ShelterFacilitiesDayOwner(this), phase: 2);
            _campaignDay.Register("starting_level_rations", new StartingLevelRationsDayOwner(this), phase: 2);

            // Phase 3: Survivors, Medical, Disease & Social
            _campaignDay.Register("duty_roster", new DutyRosterDayOwner(this), phase: 3);
            _campaignDay.Register("medical_disease", new MedicalDiseaseDayOwner(this), phase: 3);
            _campaignDay.Register("phase0_psychology", new Phase0PsychologyDayOwner(this), phase: 3);
            _campaignDay.Register("survivor_social", new SurvivorSocialDayOwner(this), phase: 3);
            _campaignDay.Register("survivors_needs", new SurvivorsNeedsDayOwner(this), phase: 3);

            // Phase 4: Expeditions, World, Factions & Quests
            _campaignDay.Register("expeditions_caravans", new ExpeditionsCaravansDayOwner(this), phase: 4);
            _campaignDay.Register("narrative_quests_verdict", new NarrativeQuestsVerdictDayOwner(this), phase: 4);

            // Phase 5: Events, Memorial & Final Evaluation
            _campaignDay.Register("host_events", new HostEventsDayOwner(this), phase: 5);
            _campaignDay.Register("memorial", new MemorialDayOwner(this), phase: 5);
        }

        // ── Phase 1 Owners ───────────────────────────────────────────────

        private sealed class WeatherWorldDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public WeatherWorldDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupWorld();
                _m._world.TickHours(24f);
                _m._world.WeatherIntelligence.TickDay(day);
                events.Add(new DayStateChangeEvent("weather_ticked", "weather_world",
                    _m._world.Weather.Current.ToString(), null, _m._world.OutdoorRadModifier));
            }
        }

        private sealed class DeepCoastMaritimeDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public DeepCoastMaritimeDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupMaritime();
                if (_m._maritime.Dive.IsActive)
                    _m._maritime.TickDive(60f);
                _m.SetupDeepCoast();
                _m._deepCoast.TickDaily(day, _m._core.Weather);
                _m._deepCoastPanel?.SetSimDay(day);
                events.Add(new DayStateChangeEvent("maritime_ticked", "maritime_deep_coast", null, null, day));
            }
        }

        private sealed class PowerGridDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public PowerGridDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.TickPowerGrid(day);
                events.Add(new DayStateChangeEvent("power_ticked", "power_grid", null, null, day));
            }
        }

        private sealed class HoldfastCoreDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public HoldfastCoreDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupIceRoad();
                string delta = _m._core.TickDay();
                if (_m._holdfastRuntime != null && !_m._holdfastRuntime.IsDead)
                {
                    _m._holdfastRuntime.Survivors = _m._survivors;
                    _m._holdfastRuntime.TickDay();
                }
                events.Add(new DayStateChangeEvent("holdfast_ticked", "holdfast_core", delta, null, _m._core.Clock.Day));
            }
        }

        // ── Phase 2 Owners ───────────────────────────────────────────────

        private sealed class StartingLevelRationsDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public StartingLevelRationsDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupStartingLevel();
                _m._startingLevel.TickDay();

                _m.SetupInventory();
                int foodToConsume = _m._startingLevel.System.State.rationPolicy == Ashfall.Core.StartingLevel.RationPolicy.Half ? 2 : 3;
                int waterToConsume = _m._startingLevel.System.State.rationPolicy == Ashfall.Core.StartingLevel.RationPolicy.Irradiated ? 0 : (_m._startingLevel.System.State.rationPolicy == Ashfall.Core.StartingLevel.RationPolicy.Half ? 2 : 3);
                _m._inventory.Remove("canned_food", foodToConsume);
                if (waterToConsume > 0)
                    _m._inventory.Remove("clean_water", waterToConsume);
                else
                    _m._inventory.Remove("irradiated_water", 2);

                events.Add(new DayStateChangeEvent("consumed_rations", "starting_level_rations", "canned_food", null, foodToConsume));
            }
        }

        private sealed class CraftingProductionDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public CraftingProductionDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupCrafting();
                _m._crafting.CompleteAll(24f);
                events.Add(new DayStateChangeEvent("crafting_completed", "crafting_production", null, null, 24f));
            }
        }

        private sealed class GreenhouseFoundryDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public GreenhouseFoundryDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupExpansions();
                if (_m._expansions.Greenhouse.PlotCount > 0)
                    _m._expansions.TickGreenhouse(day);

                _m.SetupGreenhouse();
                _m._greenhouse.TickDay(day, growLightHours: 6f, ashContaminationRate: 0.04f);

                _m.SetupSilentFoundry();
                _m._silentFoundry.TickDaily(day);
                _m._silentFoundryPanel?.RefreshView();
                if (_m._foundryDirty) _m.SaveExpansionHub();

                events.Add(new DayStateChangeEvent("greenhouse_foundry_ticked", "greenhouse_foundry", null, null, day));
            }
        }

        private sealed class ShelterFacilitiesDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public ShelterFacilitiesDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.TickAllExpandedShelterSystems(day);
                events.Add(new DayStateChangeEvent("shelter_facilities_ticked", "shelter_facilities", null, null, day));
            }
        }

        // ── Phase 3 Owners ───────────────────────────────────────────────

        private sealed class SurvivorsNeedsDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public SurvivorsNeedsDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupSurvivors();
                _m._survivors.TickHour(24f);
                events.Add(new DayStateChangeEvent("survivors_ticked", "survivors_needs", null, null, _m._survivors.RosterState.Count));
            }
        }

        private sealed class MedicalDiseaseDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public MedicalDiseaseDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupMedical();
                _m._medical.TickHours(24f);

                _m.SetupDisease();
                _m._disease.TickDaily(day);
                if (_m._expansionHubDirty) _m.SaveExpansionHub();

                events.Add(new DayStateChangeEvent("medical_disease_ticked", "medical_disease", null, null, day));
            }
        }

        private sealed class DutyRosterDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public DutyRosterDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupDutyRoster();
                _m._dutyRoster!.SyncDay(day);
                _m._dutyRoster!.TickDay(_m.BuildHomeOccupantSnapshot());
                _m.SetupIceRoad();
                _m._dutyRoster.SyncHoldfastToDuty(_m._core.Census, _m._core.IceRoad, _m._expansions.Waystation, _m._core.Brine, day);
                _m._dutyRosterPanel?.RefreshView();
                if (_m._dutyRosterDirty) _m.SaveDutyRoster();

                events.Add(new DayStateChangeEvent("duty_roster_ticked", "duty_roster", null, null, day));
            }
        }

        private sealed class SurvivorSocialDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public SurvivorSocialDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.TickSurvivorSocial(day);
                events.Add(new DayStateChangeEvent("survivor_social_ticked", "survivor_social", null, null, day));
            }
        }

        private sealed class Phase0PsychologyDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public Phase0PsychologyDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupPhase0();
                _m._phase0.CurrentDay = day;
                _m._phase0.IsInFalloutStorm = _m._world != null && _m._world.Weather.Current == Ashfall.Core.WeatherKind.FalloutStorm;
                _m._phase0.IsNightTime = day % 2 == 0;
                _m._phase0.TickDay(day);

                events.Add(new DayStateChangeEvent("phase0_ticked", "phase0_psychology", null, null, day));
            }
        }

        // ── Phase 4 Owners ───────────────────────────────────────────────

        private sealed class ExpeditionsCaravansDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public ExpeditionsCaravansDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupExpeditions();
                _m._expeditions.TickHours(24f);

                _m.SetupDutyRoster();
                var expeditions = _m._expeditions.Engine.CaptureState();
                if (expeditions != null && _m._dutyRoster != null)
                {
                    for (int i = 0; i < expeditions.Count; i++)
                    {
                        var ex = expeditions[i];
                        if (ex == null) continue;
                        if (ex.phase == (int)ExpeditionPhase.Completed && !string.IsNullOrEmpty(ex.survivorId))
                        {
                            bool crisis = _m._dutyRoster.Quests.IsCrisisQuestActive();
                            _m._dutyRoster.BridgeHatchReturn(ex.survivorId, crisis: crisis);
                            break;
                        }
                    }
                }

                _m.SetupCaravans();
                _m._caravans.TickRoute();

                events.Add(new DayStateChangeEvent("expeditions_caravans_ticked", "expeditions_caravans", null, null, day));
            }
        }

        private sealed class NarrativeQuestsVerdictDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public NarrativeQuestsVerdictDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupMoralChoice();
                _m._moralChoice.Reconcile(day);

                _m.TickVerdict(day, _m.LivingDwellerCountEstimate());

                if (day >= 180 && day <= 360)
                {
                    _m.SetupYearOfAsh();
                    _m._yearOfAsh.TickDay(day);
                }

                if (day >= 260)
                {
                    _m.SetupMuster();
                    _m._muster.Escalate(day);
                }

                _m.SetupExpansions();
                _m._expansions.Ledger.TickDaily(day);
                _m._expansions.TickCrossingQuests(day);

                _m.SetupExpansionQuests();
                _m._expansionQuests.TickDay(day);

                events.Add(new DayStateChangeEvent("narrative_ticked", "narrative_quests_verdict", null, null, day));
            }
        }

        // ── Phase 5 Owners ───────────────────────────────────────────────

        private sealed class HostEventsDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public HostEventsDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupEventAdapter();
                bool hydroAudit = _m._muster?.HydroBarons?.AdminReform ?? false;
                bool hydroSeized = _m._muster?.HydroBarons?.PlantSeized ?? false;
                bool osteophageInquiry = (_m._yearOfAsh != null && _m._yearOfAsh.Timeline.CurrentDay >= 205) || day >= 205;
                bool coldCountBroadcast = _m._muster?.ColdCount?.BroadcastSent ?? false;
                _m._hostEventAdapter?.EvaluateTriggers(day, hydroAudit, hydroSeized, osteophageInquiry, coldCountBroadcast);

                events.Add(new DayStateChangeEvent("events_evaluated", "host_events", null, null, day));
            }
        }

        private sealed class MemorialDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public MemorialDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupMemorial();
                events.Add(new DayStateChangeEvent("memorial_checked", "memorial", null, null, day));
            }
        }
    }
}
