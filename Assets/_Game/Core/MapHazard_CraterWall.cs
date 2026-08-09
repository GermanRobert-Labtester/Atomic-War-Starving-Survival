using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class CraterWallState
    {
        public string hazardId = "map_hazard_crater_wall";
        public float climbHours = 4f;
        public float fatigueDrainPerHour = 0.25f;
        public bool requiresClimbingGear = true;
        public bool blocksVehicles = true;
        // Parallel arrays — JsonUtility-safe climb progress.
        public string[] climbProgressIds = Array.Empty<string>();
        public float[] climbProgressHours = Array.Empty<float>();
    }

    /// <summary>DEMOTE-MapHazard-batch — dormant ghost. Re-promote with Boot+Save+host.</summary>
    public class MapHazard_CraterWall
    {
        public event Action<string> OnClimbStarted; // survivorId
        public event Action<string> OnClimbCompleted; // survivorId
        public event Action<string> OnClimbFailed; // survivorId

        private CraterWallState _state;
        private Dictionary<string, float> _climbProgress = new Dictionary<string, float>();

        public MapHazard_CraterWall()
        {
            _state = new CraterWallState();
        }

        public MapHazard_CraterWall(CraterWallState state)
        {
            _state = state ?? new CraterWallState();
        }

        public CraterWallState State => _state;

        public bool AttemptClimb(string survivorId, bool hasClimbingGear, float currentFatigue, float hours)
        {
            if (_state.requiresClimbingGear && !hasClimbingGear)
            {
                OnClimbFailed?.Invoke(survivorId);
                return false;
            }

            OnClimbStarted?.Invoke(survivorId);

            float fatigueAccumulated = _state.fatigueDrainPerHour * hours;
            float totalFatigue = currentFatigue + fatigueAccumulated;

            if (totalFatigue >= 1.0f)
            {
                OnClimbFailed?.Invoke(survivorId);
                return false;
            }

            if (hours < _state.climbHours)
            {
                // Partial progress
                if (!_climbProgress.ContainsKey(survivorId))
                    _climbProgress[survivorId] = 0f;
                _climbProgress[survivorId] += hours;

                if (_climbProgress[survivorId] >= _state.climbHours)
                {
                    _climbProgress.Remove(survivorId);
                    OnClimbCompleted?.Invoke(survivorId);
                    return true;
                }

                return false;
            }

            OnClimbCompleted?.Invoke(survivorId);
            return true;
        }

        public bool CanVehiclePass()
        {
            return false;
        }

        public float GetClimbDuration()
        {
            return _state.climbHours;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public CraterWallState CaptureState()
        {
            var ids = new string[_climbProgress.Count];
            var hours = new float[_climbProgress.Count];
            int i = 0;
            foreach (var kvp in _climbProgress)
            {
                ids[i] = kvp.Key;
                hours[i] = kvp.Value;
                i++;
            }

            return new CraterWallState
            {
                hazardId = string.IsNullOrEmpty(_state.hazardId) ? "map_hazard_crater_wall" : _state.hazardId,
                climbHours = _state.climbHours,
                fatigueDrainPerHour = _state.fatigueDrainPerHour,
                requiresClimbingGear = _state.requiresClimbingGear,
                blocksVehicles = _state.blocksVehicles,
                climbProgressIds = ids,
                climbProgressHours = hours
            };
        }

        public void RestoreState(CraterWallState data)
        {
            if (data == null)
            {
                _state = new CraterWallState();
                _climbProgress = new Dictionary<string, float>();
                return;
            }

            _state = new CraterWallState
            {
                hazardId = string.IsNullOrEmpty(data.hazardId) ? "map_hazard_crater_wall" : data.hazardId,
                climbHours = data.climbHours,
                fatigueDrainPerHour = data.fatigueDrainPerHour,
                requiresClimbingGear = data.requiresClimbingGear,
                blocksVehicles = data.blocksVehicles,
                climbProgressIds = data.climbProgressIds ?? Array.Empty<string>(),
                climbProgressHours = data.climbProgressHours ?? Array.Empty<float>()
            };

            _climbProgress = new Dictionary<string, float>();
            if (_state.climbProgressIds != null && _state.climbProgressHours != null)
            {
                int n = Math.Min(_state.climbProgressIds.Length, _state.climbProgressHours.Length);
                for (int i = 0; i < n; i++)
                {
                    if (!string.IsNullOrEmpty(_state.climbProgressIds[i]))
                        _climbProgress[_state.climbProgressIds[i]] = _state.climbProgressHours[i];
                }
            }
        }
    }
}
