using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Expansion VIII — Location: Quarantine Mile (Highway 9 Checkpoint Delta).
    /// On Day -2, the military stopped 4,000 civilian vehicles to "process" them.
    /// Then the EMP hit. The military fled. The refugees waited in their cars
    /// for the four hours to end. They waited until they died.
    /// </summary>
    public class Location_QuarantineMile
    {
        public const string LocationId = "location_quarantine_mile";
        public const string DisplayName = "Quarantine Mile";
        public const int TravelHours = 4;  // 4.5h round trip
        public const int DangerLevel = 7;
        public const float BaseRads = 15f;

        public const int TotalVehicles = 4000;
        public const float VehicleSearchTimeHours = 0.5f;
        public const float IceCollapseChance = 0.05f;
        public const float ExplosiveCollapseChance = 0.80f;

        public event Action<string> OnPASystemActivated;
        public event Action<string> OnIceCollapse;
        public event Action<string, int> OnVehicleSearched;

        private readonly System.Random _rng;
        private int _vehiclesSearched;
        private bool _paSystemActivated;
        private bool _iceCollapsed;

        public int VehiclesSearched => _vehiclesSearched;
        public bool IsIceCollapsed => _iceCollapsed;

        public Location_QuarantineMile(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(12000);
        }

        public List<string> SearchVehicle(string survivorId)
        {
            if (_iceCollapsed) return null;
            _vehiclesSearched++;

            var loot = new List<string>();
            float r = (float)_rng.NextDouble();

            if (r < 0.60f) loot.Add("family_photograph");
            if (r < 0.40f) loot.Add("currency");
            if (r < 0.30f) loot.Add("winter_coat");
            if (r < 0.20f) loot.Add("canned_food");
            if (r < 0.05f) loot.Add("jewelry");

            OnVehicleSearched?.Invoke(survivorId, _vehiclesSearched);

            // Ice collapse check
            if (_rng.NextDouble() < IceCollapseChance)
            {
                _iceCollapsed = true;
                OnIceCollapse?.Invoke(survivorId);
            }

            return loot;
        }

        public bool ActivatePASystem(string survivorId, bool hasFoleyOrElectrician)
        {
            if (!hasFoleyOrElectrician || _paSystemActivated) return false;
            _paSystemActivated = true;
            OnPASystemActivated?.Invoke(survivorId);
            return true;
        }

        public bool CheckExplosiveCollapse(bool usedExplosive)
        {
            if (!usedExplosive) return false;
            if (_rng.NextDouble() < ExplosiveCollapseChance)
            {
                _iceCollapsed = true;
                return true;
            }
            return false;
        }

        public QuarantineSave CaptureState()
        {
            return new QuarantineSave
            {
                VehiclesSearched = _vehiclesSearched,
                PASystemActivated = _paSystemActivated,
                IceCollapsed = _iceCollapsed
            };
        }

        public void RestoreState(QuarantineSave save)
        {
            _vehiclesSearched = 0;
            _paSystemActivated = false;
            _iceCollapsed = false;
            if (save == null) return;
            _vehiclesSearched = save.VehiclesSearched;
            _paSystemActivated = save.PASystemActivated;
            _iceCollapsed = save.IceCollapsed;
        }
    }

    [Serializable]
    public class QuarantineSave
    {
        public int VehiclesSearched;
        public bool PASystemActivated;
        public bool IceCollapsed;
    }
}
