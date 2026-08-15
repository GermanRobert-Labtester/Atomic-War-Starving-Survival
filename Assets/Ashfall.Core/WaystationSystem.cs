using System;
using System.Collections.Generic;

namespace Ashfall.Core
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
        public string[] watchSurvivorIds = Array.Empty<string>();
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
            _state.bunksOccupied = Math.Clamp(ids.Count, 0, MaxBunks);
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
            _state.filterHealth = Math.Clamp(_state.filterHealth - burn, 0f, 100f);
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
            _state.filterHealth = Math.Min(100f, _state.filterHealth + 40f);
            RaiseChanged();
        }

        public WaystationSystemState CaptureState()
        {
            return new WaystationSystemState
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
                    : Array.Empty<string>()
            };
        }

        public void RestoreState(WaystationSystemState saved)
        {
            if (saved == null) _state = new WaystationSystemState();
            else
            {
                // Deep-copy: the live system must never alias the envelope's array.
                _state = new WaystationSystemState
                {
                    systemId = saved.systemId,
                    unlocked = saved.unlocked,
                    stoveLit = saved.stoveLit,
                    filterHealth = saved.filterHealth,
                    bunksOccupied = saved.bunksOccupied,
                    winteringClosedWindow = saved.winteringClosedWindow,
                    daysSinceResupply = saved.daysSinceResupply,
                    watchSurvivorIds = saved.watchSurvivorIds != null
                        ? (string[])saved.watchSurvivorIds.Clone()
                        : Array.Empty<string>()
                };
            }
            if (_state.watchSurvivorIds == null) _state.watchSurvivorIds = Array.Empty<string>();
            if (string.IsNullOrEmpty(_state.systemId)) _state.systemId = SystemId;
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
