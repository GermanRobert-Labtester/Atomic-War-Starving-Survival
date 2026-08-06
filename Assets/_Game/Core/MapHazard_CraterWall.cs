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
    }

    public class MapHazard_CraterWall
    {
        public event Action<string> OnClimbStarted; // survivorId
        public event Action<string> OnClimbCompleted; // survivorId
        public event Action<string> OnClimbFailed; // survivorId
        public event Action<string> OnVehicleBlocked; // vehicleId

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

        public Dictionary<string, float> CaptureState()
        {
            return new Dictionary<string, float>(_climbProgress);
        }

        public void RestoreState(Dictionary<string, float> data)
        {
            _climbProgress = data != null
                ? new Dictionary<string, float>(data)
                : new Dictionary<string, float>();
        }
    }
}
