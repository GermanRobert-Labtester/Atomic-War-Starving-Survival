using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Expansion III — Mega-Location forward operating base. Survivors set up a
    /// temporary camp at a staging point before pushing into a multi-day mega-location.
    /// Camp must be supplied with food, water, fuel, and medical supplies before
    /// the expedition departs. The bunker is vulnerable while the team is away.
    ///
    /// Uses string survivor IDs — the host (GameBootstrap) resolves actual Survivor
    /// references. This keeps the World assembly free of Survivors dependency.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class MegaExpeditionCampSystem
    {
        // ── Camp supply requirements ──────────────────────────────────
        public const int FoodPerSurvivorPerDay = 3;
        public const int WaterPerSurvivorPerDay = 3;
        public const float FuelPerDay = 5f;
        public const int MedicalSupplyUnits = 2;

        // ── Camp states ───────────────────────────────────────────────
        public enum CampState
        {
            None,
            Preparing,
            Deployed,
            Returning,
            Disbanded
        }

        // ── Events ────────────────────────────────────────────────────
        public event Action<string> OnCampEstablished;
        public event Action<string> OnCampDeployed;
        public event Action<string, int> OnTeamDeparted;
        public event Action<string> OnTeamReturned;
        public event Action<string, string> OnSupplyShortage;

        private CampState _state = CampState.None;
        private string _locationId;
        private readonly List<string> _deployedSurvivorIds = new List<string>();
        private readonly Dictionary<string, float> _supplies = new Dictionary<string, float>();
        private float _hoursDeployed;
        private float _totalHoursRequired;
        private int _returnTravelHours;

        public CampState State => _state;
        public string LocationId => _locationId;
        public IReadOnlyList<string> DeployedSurvivorIds => _deployedSurvivorIds;
        public float HoursDeployed => _hoursDeployed;
        public float TotalHoursRequired => _totalHoursRequired;
        public float Progress => _totalHoursRequired > 0f ? Mathf.Clamp01(_hoursDeployed / _totalHoursRequired) : 0f;
        public bool IsDeployed => _state == CampState.Deployed || _state == CampState.Returning;

        // ── Supply tracking ───────────────────────────────────────────

        public float GetSupply(string supplyId)
        {
            return _supplies.TryGetValue(supplyId, out var v) ? v : 0f;
        }

        public void AddSupply(string supplyId, float amount)
        {
            if (string.IsNullOrEmpty(supplyId) || amount <= 0f) return;
            _supplies.TryGetValue(supplyId, out var current);
            _supplies[supplyId] = current + amount;
        }

        public static ExpeditionSupplyNeeds CalculateNeeds(
            int survivorCount, int missionHours, int returnHours)
        {
            float missionDays = missionHours / 24f;
            float returnDays = returnHours / 24f;
            float totalDays = missionDays + returnDays + 0.5f;

            return new ExpeditionSupplyNeeds
            {
                FoodUnits = Mathf.CeilToInt(FoodPerSurvivorPerDay * survivorCount * totalDays),
                WaterUnits = Mathf.CeilToInt(WaterPerSurvivorPerDay * survivorCount * totalDays),
                FuelUnits = Mathf.CeilToInt(FuelPerDay * totalDays),
                MedicalUnits = MedicalSupplyUnits,
                TotalMissionHours = missionHours,
                ReturnTravelHours = returnHours
            };
        }

        // ── Camp lifecycle ────────────────────────────────────────────

        public bool BeginPreparation(string locationId, List<string> survivorIds,
            int missionHours, int returnHours)
        {
            if (_state != CampState.None && _state != CampState.Disbanded) return false;
            if (string.IsNullOrEmpty(locationId) || survivorIds == null || survivorIds.Count < 2)
                return false;

            _locationId = locationId;
            _deployedSurvivorIds.Clear();
            _deployedSurvivorIds.AddRange(survivorIds);
            _totalHoursRequired = missionHours;
            _returnTravelHours = returnHours;
            _hoursDeployed = 0f;
            _state = CampState.Preparing;

            OnCampEstablished?.Invoke(locationId);
            return true;
        }

        public bool Deploy()
        {
            if (_state != CampState.Preparing) return false;

            var needs = CalculateNeeds(_deployedSurvivorIds.Count,
                Mathf.CeilToInt(_totalHoursRequired), _returnTravelHours);

            if (GetSupply("food") < needs.FoodUnits)
            {
                OnSupplyShortage?.Invoke(_locationId, "food");
                return false;
            }
            if (GetSupply("water") < needs.WaterUnits)
            {
                OnSupplyShortage?.Invoke(_locationId, "water");
                return false;
            }
            if (GetSupply("fuel") < needs.FuelUnits)
            {
                OnSupplyShortage?.Invoke(_locationId, "fuel");
                return false;
            }

            _supplies["food"] = GetSupply("food") - needs.FoodUnits;
            _supplies["water"] = GetSupply("water") - needs.WaterUnits;
            _supplies["fuel"] = GetSupply("fuel") - needs.FuelUnits;

            _state = CampState.Deployed;
            OnCampDeployed?.Invoke(_locationId);
            OnTeamDeparted?.Invoke(_locationId, _deployedSurvivorIds.Count);
            return true;
        }

        /// <summary>
        /// Tick the camp forward. Returns true when mission phase is complete.
        /// Host is responsible for applying injury/radiation to actual Survivor objects.
        /// </summary>
        public bool Tick(float gameHours)
        {
            if (_state != CampState.Deployed) return false;
            _hoursDeployed += gameHours;
            if (_hoursDeployed >= _totalHoursRequired)
            {
                _state = CampState.Returning;
                return true;
            }
            return false;
        }

        public bool BeginReturn()
        {
            if (_state != CampState.Returning) return false;
            return true;
        }

        /// <summary>
        /// Complete the return. Returns list of survivor IDs that made it back alive.
        /// Host resolves actual alive/dead status.
        /// </summary>
        public List<string> CompleteReturn()
        {
            if (_state != CampState.Returning) return null;
            OnTeamReturned?.Invoke(_locationId);
            _state = CampState.Disbanded;
            return new List<string>(_deployedSurvivorIds);
        }

        public void Disband()
        {
            _state = CampState.Disbanded;
        }

        public int DeployedCount => _deployedSurvivorIds.Count;
        public bool BunkerUnderstaffed => IsDeployed && _deployedSurvivorIds.Count >= 2;

        // ── Save / Load ───────────────────────────────────────────────

        public MobileCampSave CaptureState()
        {
            var supplyEntries = new SupplySave[_supplies.Count];
            int i = 0;
            foreach (var kv in _supplies)
                supplyEntries[i++] = new SupplySave { SupplyId = kv.Key, Amount = kv.Value };

            return new MobileCampSave
            {
                State = _state,
                LocationId = _locationId,
                DeployedSurvivorIds = _deployedSurvivorIds.ToArray(),
                Supplies = supplyEntries,
                HoursDeployed = _hoursDeployed,
                TotalHoursRequired = _totalHoursRequired,
                ReturnTravelHours = _returnTravelHours
            };
        }

        public void RestoreState(MobileCampSave save)
        {
            _state = CampState.None;
            _locationId = null;
            _deployedSurvivorIds.Clear();
            _supplies.Clear();
            _hoursDeployed = 0f;
            _totalHoursRequired = 0f;
            _returnTravelHours = 0;
            if (save == null) return;
            _state = save.State;
            _locationId = save.LocationId;
            if (save.DeployedSurvivorIds != null)
                _deployedSurvivorIds.AddRange(save.DeployedSurvivorIds);
            if (save.Supplies != null)
                for (int i = 0; i < save.Supplies.Length; i++)
                    if (save.Supplies[i] != null)
                        _supplies[save.Supplies[i].SupplyId] = save.Supplies[i].Amount;
            _hoursDeployed = save.HoursDeployed;
            _totalHoursRequired = save.TotalHoursRequired;
            _returnTravelHours = save.ReturnTravelHours;
        }
    }

    [Serializable]
    public class ExpeditionSupplyNeeds
    {
        public int FoodUnits;
        public int WaterUnits;
        public int FuelUnits;
        public int MedicalUnits;
        public int TotalMissionHours;
        public int ReturnTravelHours;
    }

    [Serializable]
    public class MobileCampSave
    {
        public MegaExpeditionCampSystem.CampState State;
        public string LocationId;
        public string[] DeployedSurvivorIds;
        public SupplySave[] Supplies;
        public float HoursDeployed;
        public float TotalHoursRequired;
        public int ReturnTravelHours;
    }

    [Serializable]
    public class SupplySave
    {
        public string SupplyId;
        public float Amount;
    }
}
