// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.PlayerCommand;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.YearOfAsh;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for ShelterThermalSystem.
    /// Manages boiler fuel, heating zones, radiator valves, pipe freeze/burst risks, and thermal incidents.
    /// </summary>
    public sealed class ShelterThermalHostSession
    : HostSessionBase{
        public ShelterThermalSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public ShelterThermalHostSession(ShelterThermalSystem? system = null,
            Ashfall.Core.Shelter.ShelterAssignmentSystem? assignment = null)
        {
            if (system == null)
            {
                var rng = new SeededRng(1986);
                var needs = new NeedsSystem();
                var starting = new StartingLevelSystem();
                var deepFreeze = new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState());
                system = new ShelterThermalSystem(rng, needs, starting, deepFreeze,
                    new GodotLog(), assignment);
            }
            System = system;

            System.OnIncident += inc =>
            {
                LastEvent = $"[Thermal] INCIDENT: {inc.kind} in {inc.roomId} (Pipe {inc.pipeId})";
                RaiseStateChanged();
            };

            System.OnThermalChanged += () =>
            {
                RaiseStateChanged();
            };

            System.OnFrostbiteRisk += (roomId, survivorId) =>
            {
                LastEvent = $"[Thermal] FROSTBITE RISK: {survivorId} in {roomId} (<5°C)";
                RaiseStateChanged();
            };
        }

        public ActionResult SetBoilerActive(bool active)
        {
            var res = System.SetBoilerActive(active);
            if (res.IsSuccess)
            {
                LastEvent = $"Boiler status set to: {(active ? "ACTIVE" : "OFF")}";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult SetRadiatorValve(string roomId, float openRatio)
        {
            var res = System.SetRadiatorValve(roomId, openRatio);
            if (res.IsSuccess)
            {
                RaiseStateChanged();
            }
            return res;
        }

        public CommandResult RepairPipe(string pipeId, float repairAmount = 20f)
        {
            var result = System.ExecuteRepairPipe(pipeId, repairAmount, expectedStateVersion: StateVersion, currentStateVersion: StateVersion);
            if (result.IsSuccess)
            {
                LastEvent = $"Pipe repaired: {result.FailureCode}";
                RaiseStateChanged();
            }
            return result;
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
            RaiseStateChanged();
        }

        /// <summary>
        /// Storm sealing (alpha feature): retrofit the authored storm-sealing
        /// insulation on every room that does not have it yet, paying each
        /// room's authored cost through the canonical inventory. Backed by the
        /// existing thermal retrofit engine + thermal save store; owns no new
        /// state.
        /// </summary>
        public ActionResult RetrofitStormSealing(Ashfall.Core.Inventory.Inventory? inventory)
        {
            const string stormSealingId = "insul_storm_sealing";
            int applied = 0;
            int blocked = 0;
            string lastReason = string.Empty;
            foreach (var room in System.State.rooms)
            {
                if (room == null) continue;
                if (System.State.roomInstalledInsulation.TryGetValue(room.roomId, out var installed)
                    && installed == stormSealingId)
                {
                    continue;
                }
                var result = System.RetrofitInsulation(room.roomId, stormSealingId, inventory);
                if (result.Status == ActionResult.StatusKind.Success) applied++;
                else { blocked++; lastReason = string.IsNullOrEmpty(result.FailureCode) ? "retrofit_failed" : result.FailureCode; }
            }

            LastEvent = blocked == 0
                ? $"Storm sealing fitted in {applied} room(s)."
                : $"Storm sealing fitted in {applied} room(s); {blocked} blocked ({lastReason}).";
            if (applied > 0) RaiseStateChanged();
            return applied > 0
                ? ActionResult.Success("storm_sealing")
                : ActionResult.Blocked(lastReason.Length > 0 ? lastReason : "already_sealed", "storm_sealing_none");
        }

        public void SetAssignments(Ashfall.Core.Shelter.ShelterAssignmentSystem? assignment)
        {
            System.SetAssignments(assignment);
        }

        public override void Save()
        {
            if (!IsDirty) return;
            ShelterThermalSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
