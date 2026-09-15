// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Factions;
using Ashfall.Core.Narrative;
using Ashfall.Core.Quests;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private EspionageHostSession? _espionage166;
        private FluidLogisticsHostSession? _fluidLogistics168;
        private ProceduralNarrativeHostSession? _proceduralNarrative169;
        private readonly HashSet<string> _espionageAgentsAway = new HashSet<string>(StringComparer.Ordinal);
        private bool _espionage166Dirty;
        private bool _fluidLogistics168Dirty;
        private bool _proceduralNarrative169Dirty;
        private bool _espionageConsequenceBound;

        /// <summary>
        /// Composes the Plans 166–169 campaign seams. Existing ResearchSystem,
        /// WaterTreatmentSystem, faction authorities, and domain quest systems
        /// remain owners of their facts; these sessions only add the new
        /// progression, distribution, intelligence, and shared quest edges.
        /// </summary>
        private void SetupPlans166To169()
        {
            SetupCampaignDay();
            SetupSurvivors();
            SetupInventory();
            SetupWorld();
            SetupPowerGrid();
            SetupCrafting();
            SetupWaterTreatment();
            SetupDisease();
            SetupPsyOps();
            SetupYearOfAsh();
            EnsureCaravanTrade();
            _sharedResearch = EnsureSharedResearch();

            if (_espionage166 == null)
            {
                var system = new EspionageSystem(new GodotLog());
                system.BindRng(_campaignDay.Rng.Fork("espionage"));
                system.BindAgentAvailability(
                    id => _survivors?.Find(id)?.IsAliveState == true,
                    id => _survivors?.Find(id)?.IsAliveState == true && !_espionageAgentsAway.Contains(id),
                    (id, away) =>
                    {
                        if (away) _espionageAgentsAway.Add(id);
                        else _espionageAgentsAway.Remove(id);
                    });
                system.BindAgentCapability(_ => 1f);
                system.BindResearchGate(id =>
                    _sharedResearch?.GetKnowledge(id)?.isCompleted == true
                    || _sharedResearch?.State.completedIds?.Contains(id) == true);
                _espionage166 = EspionageHostSession.Create(_dataDir, system);
                _espionage166.StateChanged += () => _espionage166Dirty = true;
                var saved = EspionageSaveStore.TryLoad();
                if (saved != null) _espionage166.RestoreState(saved);
            }

            BindEspionageConsequenceRouting();

            if (_fluidLogistics168 == null)
            {
                var system = new FluidLogisticsSystem(new GodotLog());
                system.BindRng(_campaignDay.Rng.Fork("fluid_logistics"));
                system.EnsureDefaultShelterTopology();
                _fluidLogistics168 = FluidLogisticsHostSession.Create(_dataDir, system);
                _fluidLogistics168.StateChanged += () => _fluidLogistics168Dirty = true;
                var saved = FluidLogisticsSaveStore.TryLoad();
                if (saved != null)
                {
                    _fluidLogistics168.RestoreState(saved);
                    // Reload may restore a partial/empty graph; re-seed defaults.
                    _fluidLogistics168.System.EnsureDefaultShelterTopology();
                }
            }

            if (_proceduralNarrative169 == null)
            {
                _proceduralNarrative169 = ProceduralNarrativeHostSession.Create(_dataDir);
                _proceduralNarrative169.StateChanged += () => _proceduralNarrative169Dirty = true;
                var saved = ProceduralNarrativeSaveStore.TryLoad();
                if (saved != null) _proceduralNarrative169.RestoreState(saved.narrative, saved.quests);
            }
        }

        private void BindEspionageConsequenceRouting()
        {
            if (_espionage166 == null || _espionageConsequenceBound) return;

            var caravan = EnsureCaravanTrade();
            SetupPsyOps();
            SetupYearOfAsh();

            _espionage166.BindConsequenceConsumers(
                applySupply: intent => caravan.TryApplySupplyDisruption(
                    intent.targetFactionId,
                    intent.magnitude,
                    intent.startDay,
                    intent.expiryDay,
                    intent.incidentId),
                applyComms: intent =>
                {
                    if (_psyops?.System == null) return false;
                    int days = Math.Max(1, intent.expiryDay - intent.startDay);
                    return _psyops.System.StartJamming(
                        intent.targetFactionId,
                        intent.magnitude,
                        days,
                        intent.startDay);
                },
                applyDefense: intent =>
                {
                    if (_yearOfAsh?.FactionWar == null) return false;
                    return _yearOfAsh.FactionWar.TryApplyDefenseReadinessPressure(
                        intent.targetFactionId,
                        intent.magnitude,
                        intent.startDay,
                        intent.expiryDay,
                        intent.incidentId);
                });
            _espionageConsequenceBound = true;
        }

        private void SaveEspionage()
        {
            if (_espionage166 != null)
                CaptureSection(EspionageSaveStore.SectionName,
                    EspionageSaveStore.TryCapturePersisted(_espionage166.CaptureState()));
        }

        private void SaveFluidLogistics()
        {
            if (_fluidLogistics168 != null)
                CaptureSection(FluidLogisticsSaveStore.SectionName,
                    FluidLogisticsSaveStore.TryCapturePersisted(_fluidLogistics168.CaptureState()));
        }

        private void SaveProceduralNarrative()
        {
            if (_proceduralNarrative169 == null) return;
            var payload = new ProceduralNarrativeSaveState
            {
                narrative = _proceduralNarrative169.CaptureNarrativeState(),
                quests = _proceduralNarrative169.CaptureQuestState()
            };
            CaptureSection(ProceduralNarrativeSaveStore.SectionName,
                ProceduralNarrativeSaveStore.TryCapturePersisted(payload));
        }

        private void TickPlan166Research(int day)
        {
            SetupPlans166To169();
            _sharedResearch?.Tick(day);
        }

        private void TickPlan167Espionage(int day)
        {
            SetupPlans166To169();
            BindEspionageConsequenceRouting();
            _espionage166?.AdvanceDay(day);
        }

        private void TickPlan168Fluid(int day)
        {
            SetupPlans166To169();
            float temperature = _world?.Weather?.GetTemperaturePenaltyCelsius() ?? 0f;
            // G1 (SHELTER_GRID_CATALOG_SEAL): power the fluid network from the
            // canonical catalog room room_water_pump. The previous literal
            // room_water_treatment matched no grid room, so IsRoomPowered
            // always returned false and the network ran fully depowered.
            float power = _powerGrid?.System?.IsRoomPowered("room_water_pump") == false ? 0f : 1f;
            string? PickLivingSurvivor()
            {
                if (_survivors?.RosterState == null) return null;
                for (int i = 0; i < _survivors.RosterState.Count; i++)
                {
                    var s = _survivors.RosterState[i];
                    if (s != null && s.IsAliveState)
                        return s.Id;
                }
                return null;
            }

            _fluidLogistics168?.AdvanceDay(
                day,
                temperature,
                power,
                _waterTreatment?.System,
                _greenhouse?.System,
                _disease?.Engine,
                PickLivingSurvivor);
            if (_fluidLogistics168 != null)
                _fluidLogistics168Dirty = true;
        }

        private void TickPlan169Narrative(int day)
        {
            SetupPlans166To169();
            _proceduralNarrative169?.AdvanceDay(day);
        }

        private sealed class Plan166ResearchDayOwner : Ashfall.Core.Campaign.IDayAdvanceOwner
        {
            private readonly Main _main;
            public Plan166ResearchDayOwner(Main main) => _main = main;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _main.TickPlan166Research(day);
                events.Add(new DayStateChangeEvent("research_ticked", "plan_166", null, null, day));
            }
        }

        private sealed class Plan168FluidDayOwner : Ashfall.Core.Campaign.IDayAdvanceOwner
        {
            private readonly Main _main;
            public Plan168FluidDayOwner(Main main) => _main = main;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _main.TickPlan168Fluid(day);
                events.Add(new DayStateChangeEvent("fluid_logistics_ticked", "plan_168", null, null, day));
            }
        }

        private sealed class Plan167EspionageDayOwner : Ashfall.Core.Campaign.IDayAdvanceOwner
        {
            private readonly Main _main;
            public Plan167EspionageDayOwner(Main main) => _main = main;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _main.TickPlan167Espionage(day);
                events.Add(new DayStateChangeEvent("espionage_ticked", "plan_167", null, null, day));
            }
        }

        private sealed class Plan169NarrativeDayOwner : Ashfall.Core.Campaign.IDayAdvanceOwner
        {
            private readonly Main _main;
            public Plan169NarrativeDayOwner(Main main) => _main = main;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _main.TickPlan169Narrative(day);
                events.Add(new DayStateChangeEvent("procedural_narrative_ticked", "plan_169", null, null, day));
            }
        }
    }
}
