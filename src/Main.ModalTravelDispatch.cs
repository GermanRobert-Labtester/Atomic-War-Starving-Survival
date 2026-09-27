// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Modal Travel Dispatch — host wiring. Derived read model only:
// no save section, no mutation of the map, vehicle, inventory, or weather
// owners. The projection is composed from their live state on demand.
// ============================================================================

using System.Collections.Generic;
using Ashfall.Core.Weather;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ModalTravelDispatchHostSession? _modalTravelDispatch;

        public ModalTravelDispatchHostSession? ModalTravelDispatch => _modalTravelDispatch;

        public void SetupModalTravelDispatch()
        {
            if (_modalTravelDispatch != null) return;

            _modalTravelDispatch = new ModalTravelDispatchHostSession(
                () => _expeditions?.WastelandMap ?? _world?.WastelandMap,
                VehicleConditionPermille,
                TravelFuelAvailableUnits,
                FlightWeatherWindowPermille);
        }

        /// <summary>
        /// Vehicle condition in permille. Reads the live garage/vehicle owner when
        /// one is bound; otherwise the canonical "sound" default. The dispatch
        /// model never stores its own condition.
        /// </summary>
        private int VehicleConditionPermille()
        {
            var garage = _expeditions?.Garage;
            if (garage == null) return 1000;
            try
            {
                var instances = garage.GetType().GetProperty("Instances")?.GetValue(garage)
                    as System.Collections.IEnumerable;
                if (instances == null) return 1000;
                int worst = 1000;
                int count = 0;
                foreach (var inst in instances)
                {
                    if (inst == null) continue;
                    var cond = inst.GetType().GetProperty("ConditionPermille")?.GetValue(inst);
                    if (cond is int c)
                    {
                        worst = System.Math.Min(worst, c);
                        count++;
                    }
                }
                return count > 0 ? worst : 1000;
            }
            catch
            {
                return 1000;
            }
        }

        /// <summary>Fuel on hand from the canonical inventory, in units.</summary>
        private int TravelFuelAvailableUnits()
        {
            var inventory = _inventory?.Inventory;
            if (inventory == null) return 0;
            int total = 0;
            foreach (string fuelId in new[] { "item_fuel_can", "item_diesel", "item_fuel" })
                total += inventory.CountById(fuelId);
            return total;
        }

        /// <summary>
        /// Flight window in permille from the canonical weather owner. No storm
        /// forecast ledger is duplicated here.
        /// </summary>
        private int FlightWeatherWindowPermille()
        {
            // The weather owner is the sole authority for atmospheric hazard. The
            // dispatch model asks it for today's hazard and inverts it into a
            // flyable window; no storm-forecast ledger is duplicated here.
            var world = _world;
            if (world == null) return 1000;

            float severity = WeatherCascadeSeverity.SeverityFor(
                world.Weather.Current,
                world.WeatherEffects);
            int hazardPermille = (int)System.Math.Round(severity * 10.0);
            return 1000 - System.Math.Clamp(hazardPermille, 0, 1000);
        }

        /// <summary>Pre-departure feasibility for one corridor, as a status line.</summary>
        public string ModalTravelDispatchLine(string fromNodeId, string toNodeId)
        {
            SetupModalTravelDispatch();
            return _modalTravelDispatch?.StatusLine(fromNodeId, toNodeId) ?? "travel dispatch model unbound";
        }

        public List<Ashfall.Core.World.ModalTravelDispatchResult> EvaluateTravelModalities(string fromNodeId, string toNodeId)
        {
            SetupModalTravelDispatch();
            var route = _modalTravelDispatch?.FindRoute(fromNodeId, toNodeId);
            var results = new List<Ashfall.Core.World.ModalTravelDispatchResult>();
            if (_modalTravelDispatch == null) return results;
            foreach (var (_, result) in _modalTravelDispatch.EvaluateAllModalities(route))
                results.Add(result);
            return results;
        }
    }
}
