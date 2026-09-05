using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Flagship XI — Plan 154 host wiring: constructs the morale-contagion system
    /// against the canonical authorities (needs, relations, assignments, roster,
    /// crisis), persists it as the "morale_contagion" envelope section, and ticks
    /// it from the survivors-needs day owner after decor morale.
    /// </summary>
    public partial class Main : Control
    {
        private MoraleContagionHostSession? _moraleContagion;

        private void SetupMoraleContagion()
        {
            if (_moraleContagion != null) return;

            SetupSurvivors();
            SetupSurvivorSocial();
            SetupShelterAssignment();
            SetupMentalHealthCrisis();
            SetupDutyRoster();
            SetupPowerGrid();

            var catalog = ContagionEventCatalogLoader.Load(_dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            var ports = new MoraleContagionPorts
            {
                AliveSurvivors = () => _survivors.RosterState
                    .Where(s => s != null && s.IsAliveState)
                    .Select(s => s.Id)
                    .ToList(),
                GetMorale = id => _survivors.Needs.Get(id)?.Morale ?? 50f,
                ApplyMoraleDelta = (id, delta) => _survivors.Needs.Modify(id, NeedKind.Morale, delta),
                AreInSameRoom = (a, b) => _shelterAssignment?.AreInSameRoom(a, b) ?? false,
                GetDutyRole = id => _dutyRoster?.Roster?.GetRoleOf(id) ?? string.Empty,
                GetBondStrength = (a, b) => _survivorSocial?.TraumaBond?.GetBondStrength(a, b) ?? 0f,
                IsHopeBeaconActive = () => _moraleContagion != null && _moraleContagion.IsHopeBeaconOperating(),
                UnassignSurvivor = (id, day) => _shelterAssignment?.System?.Unassign(id, day),
                ClearDutyRole = id => _dutyRoster?.Roster?.RemoveAssignmentsFor(id),
                TriggerBreakdown = (id, stress) =>
                    _mentalHealthCrisis?.TriggerCrisis(id, stress, Ashfall.Core.CrisisProfile.AcuteStress)
            };

            var system = new MoraleContagionSystem(catalog, ports);
            _moraleContagion = new MoraleContagionHostSession(system);
            _moraleContagion.ConfigureHopeBeacon(
                powerQuery: () => _powerGrid?.System != null && !_powerGrid.System.IsBrownout,
                occupancyQuery: () => _shelterAssignment?.System?.GetRoomOccupancy(MoraleContagionHostSession.HopeBeaconRoomId) ?? 0);

            var save = MoraleContagionSaveStore.TryLoad();
            if (save != null)
            {
                _moraleContagion.RestoreSave(save);
                GD.Print("[Ashfall Godot] Morale contagion state restored.");
            }

            _moraleContagion.StateChanged += () =>
            {
                _survivorRelationsPanel?.RefreshView();
            };
            _survivorRelationsPanel?.BindContagion(_moraleContagion);
        }

        /// <summary>
        /// Player-facing beacon installation. Charges the build bill atomically;
        /// ongoing operation requires a staffed room and grid power.
        /// </summary>
        public string InstallHopeBeacon(int day)
        {
            SetupMoraleContagion();
            if (_moraleContagion == null) return "Settlement mood systems are unavailable.";
            if (_moraleContagion.IsHopeBeaconInstalled)
                return "The beacon lamp already stands in the common room.";

            var bill = new InventoryBill();
            bill.AddCost("scrap_metal", 4);
            bill.AddCost("cloth", 2);
            bill.AddCost("battery", 1);

            using var tx = _inventory.Inventory.BeginTransaction(bill);
            if (!tx.Validation.IsValid)
            {
                tx.Cancel();
                return "Not enough materials: 4 scrap metal, 2 cloth, 1 battery.";
            }

            _moraleContagion.InstallHopeBeacon(day);
            tx.TryCommit();
            return "The beacon lamp is assembled. It needs a light-keeper and power to matter.";
        }

        public string IsolateSurvivorForContagion(string survivorId, int day, int durationDays)
        {
            SetupMoraleContagion();
            if (_moraleContagion == null) return "Settlement mood systems are unavailable.";
            return _moraleContagion.IsolateSurvivor(survivorId, day, durationDays);
        }

        public string ReleaseSurvivorFromIsolation(string survivorId, int day)
        {
            SetupMoraleContagion();
            if (_moraleContagion == null) return "Settlement mood systems are unavailable.";
            return _moraleContagion.ReleaseSurvivor(survivorId, day);
        }

        private void SaveMoraleContagion()
        {
            if (_moraleContagion == null) return;
            CaptureSection("morale_contagion",
                MoraleContagionSaveStore.TryCapturePersisted(_moraleContagion.CaptureSave()));
        }
    }
}
