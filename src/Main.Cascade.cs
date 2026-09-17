// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Godot;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// C2[6] 23C — host adapter for the fact-reading cascade authority.
    ///
    /// Builds <see cref="CascadeFacts"/> from the existing subsystem owners, ticks
    /// <see cref="CascadeCoordinator"/>, and forwards exactly-once warning/recovery
    /// transitions into the canonical day-event/briefing path. It owns no gameplay
    /// state and writes no subsystem fields; the physical consequences stay with
    /// the systems that already model them.
    /// </summary>
    public partial class Main
    {
        private CascadeCoordinator? _cascadeCoordinator;
        private bool _cascadeSetupDone;

        private void SetupCascade()
        {
            if (_cascadeSetupDone) return;
            _cascadeSetupDone = true;
            var catalog = CascadeRuleCatalogLoader.TryLoad(_dataDir, new FileSystemIO());
            if (catalog == null)
            {
                GD.Print("[Cascade] cascade_rules.json not present/unreadable — cascade layer inactive.");
                return;
            }
            if (!CascadeRuleCatalogLoader.Validate(catalog, out string error))
            {
                GD.PrintErr($"[Cascade] invalid cascade_rules.json: {error}");
                return;
            }
            _cascadeCoordinator = new CascadeCoordinator(catalog);
        }

        /// <summary>
        /// Evaluate the cascade rules for the day and append attribution events.
        /// Called by the power day owner after the grid allocation is resolved, so
        /// every fact reflects the day's real served/shed outcome.
        /// </summary>
        private void TickCascade(int day, List<DayStateChangeEvent> events)
        {
            SetupCascade();
            if (_cascadeCoordinator == null) return;

            var assessment = _cascadeCoordinator.Evaluate(BuildCascadeFacts(day));
            foreach (var transition in assessment.Transitions)
            {
                events.Add(new DayStateChangeEvent(
                    transition.Kind, "cascade_coordinator", transition.RuleId, transition.DisplayName, day));
                if (transition.Kind == "cascade_warning")
                    _journal?.TryAddRawEntry($"cascade_warning_{transition.RuleId}",
                        $"STRAIN: {transition.DisplayName}. Estimated warning window {assessment.NextWarningHours:0} h.",
                        null!, day);
                else
                    _journal?.TryAddRawEntry($"cascade_recovered_{transition.RuleId}",
                        string.IsNullOrEmpty(transition.OffRamp)
                            ? $"RECOVERED: {transition.DisplayName} cleared."
                            : $"RECOVERED: {transition.DisplayName} resolved via {transition.OffRamp}.",
                        null!, day);
            }
        }

        private CascadeFacts BuildCascadeFacts(int day)
        {
            var facts = new CascadeFacts { Day = day };
            var grid = _powerGrid?.System;
            if (grid != null)
            {
                facts.PowerDeficit = grid.IsBrownout || grid.DeficitWatts > 0f;
                var summary = _powerGrid!.LastTickSummary;
                facts.CriticalDeficit = summary?.HasCriticalDeficit ?? false;
                facts.PumpingUnserved = SumpPumpingUnserved(summary?.ShedRoomIds);
                facts.HeatingUnserved = !grid.IsRoomServed("room_heating");
                facts.FiltrationUnserved = !grid.IsRoomServed("room_air_filtration");
                facts.Darkness = !grid.IsRoomServed("room_lighting_main");
            }

            facts.SumpRising = SumpIsRising();
            facts.FalloutStorm = _world?.Weather?.Current == WeatherKind.FalloutStorm;
            facts.OutbreakActive = _disease?.Engine?.State?.diseases != null
                && _disease.Engine.State.diseases.Exists(d => d != null && d.outbreak_active);
            facts.ColdStorageUnpowered = _foodPreservation64 != null && !_foodPreservation64.IsPowerOnline;
            facts.FireActive = _shelterFireHazard != null
                && _shelterFireHazard.Incidents.Values.Any(i => i != null && !i.isResolved && !i.isSuppressed);
            facts.HatchUnsealed = _airlockSecurity?.System?.State != null
                && _airlockSecurity.System.State.doorState != AirlockDoorState.Secure;
            return facts;
        }

        private bool SumpPumpingUnserved(List<string>? shedRoomIds)
        {
            var sump = _sumpFlooding?.System;
            var grid = _powerGrid?.System;
            if (sump == null || grid == null) return false;
            foreach (var node in sump.State.nodes)
            {
                if (node == null || !node.hasSumpPump || !node.pumpPowered) continue;
                if (shedRoomIds != null && shedRoomIds.Contains(node.nodeId)) return true;
                if (!grid.IsRoomServed(node.nodeId)) return true;
            }
            return false;
        }

        private bool SumpIsRising()
        {
            var sump = _sumpFlooding?.System;
            if (sump == null) return false;
            foreach (var node in sump.State.nodes)
            {
                if (node == null || node.isFlooded) continue;
                var risk = sump.GetRisk(node.nodeId);
                if (risk.NodeExists && !float.IsPositiveInfinity(risk.HoursToThreshold)) return true;
            }
            return false;
        }
    }
}