using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// ASHFALL: THE HOLDFAST — one forward camp at Waystation A.
    /// Reduced vitals. Home bunker still ticks. Spec §5.4.
    /// </summary>
    [Serializable]
    public class WaystationSystemState
    {
        public string systemId = WaystationSystem.SystemId;
        public bool unlocked;
        public bool stoveLit;
        public float filterHealth = 100f;
        public int bunksOccupied;
        public string[] watchSurvivorIds = new string[0];
        public bool winteringClosedWindow;
        public int daysSinceResupply;
    }

    public class WaystationSystem
    {
        public const string SystemId = "waystation_system";
        public const string LocationId = "loc_cut_waystation_a";
        public const int MaxBunks = 4;
        public const float FilterDegradeMultiplier = 1.4f;
        public const int FilterWindowNotches = 11;

        private WaystationSystemState _state = new WaystationSystemState();

        public event Action<WaystationSystemState> OnStateChanged;
        public event Action OnStoveDied;
        public event Action OnUnlocked;

        public WaystationSystemState State => _state;
        public bool Unlocked => _state.unlocked;
        public bool StoveLit => _state.stoveLit;

        public void Unlock()
        {
            if (_state.unlocked) return;
            _state.unlocked = true;
            _state.stoveLit = true;
            OnUnlocked?.Invoke();
            RaiseChanged();
        }

        public bool AssignWatch(IList<string> survivorIds)
        {
            if (!_state.unlocked) return false;
            var ids = new List<string>(2);
            if (survivorIds != null)
            {
                for (int i = 0; i < survivorIds.Count && ids.Count < 2; i++)
                {
                    if (string.IsNullOrEmpty(survivorIds[i])) continue;
                    ids.Add(survivorIds[i]);
                }
            }
            _state.watchSurvivorIds = ids.ToArray();
            _state.bunksOccupied = Mathf.Clamp(ids.Count, 0, MaxBunks);
            RaiseChanged();
            return true;
        }

        public void SetWintering(bool wintering)
        {
            _state.winteringClosedWindow = wintering;
            if (wintering) _state.stoveLit = true;
            RaiseChanged();
        }

        public void TickDaily(bool iceRoadOpen, float filterDegradeBase = 4f)
        {
            if (!_state.unlocked) return;
            _state.daysSinceResupply++;
            float burn = filterDegradeBase * FilterDegradeMultiplier;
            if (!iceRoadOpen) burn *= 1.1f;
            _state.filterHealth = Mathf.Clamp(_state.filterHealth - burn, 0f, 100f);
            if (_state.daysSinceResupply > FilterWindowNotches && _state.stoveLit)
            {
                _state.stoveLit = false;
                OnStoveDied?.Invoke();
            }
            RaiseChanged();
        }

        public void Resupply()
        {
            _state.daysSinceResupply = 0;
            _state.stoveLit = true;
            _state.filterHealth = Mathf.Min(100f, _state.filterHealth + 40f);
            RaiseChanged();
        }

        public WaystationSystemState CaptureState()
        {
            var s = new WaystationSystemState
            {
                systemId = _state.systemId,
                unlocked = _state.unlocked,
                stoveLit = _state.stoveLit,
                filterHealth = _state.filterHealth,
                bunksOccupied = _state.bunksOccupied,
                winteringClosedWindow = _state.winteringClosedWindow,
                daysSinceResupply = _state.daysSinceResupply,
                watchSurvivorIds = _state.watchSurvivorIds != null
                    ? (string[])_state.watchSurvivorIds.Clone()
                    : new string[0]
            };
            return s;
        }

        public void RestoreState(WaystationSystemState saved)
        {
            _state = saved ?? new WaystationSystemState();
            if (_state.watchSurvivorIds == null) _state.watchSurvivorIds = new string[0];
            if (string.IsNullOrEmpty(_state.systemId)) _state.systemId = SystemId;
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
