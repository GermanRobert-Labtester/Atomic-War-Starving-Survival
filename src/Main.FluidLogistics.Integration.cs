// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Factions;
using Ashfall.Core.Narrative;
using Ashfall.Core.Quests;
using Ashfall.Core.Random;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private FluidLogisticsHostSession? _fluidLogistics168;
        private bool _fluidLogistics168Dirty;

        private void SaveFluidLogistics()
        {
            if (_fluidLogistics168 != null)
                CaptureSection(FluidLogisticsSaveStore.SectionName,
                    FluidLogisticsSaveStore.TryCapturePersisted(_fluidLogistics168.CaptureState()));
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

    }
}
