using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    [Serializable]
    public sealed class LandmarkSaveState
    {
        public int schema_version = 1;
        public string systemId = LandmarkDegradationSystem.SystemId;
        public int lastDegradationDay = -1;
        public List<LandmarkStatusRecord> landmarks = new List<LandmarkStatusRecord>();
    }

    [Serializable]
    public sealed class LandmarkStatusRecord
    {
        public string landmarkId = string.Empty;
        public string locationId = string.Empty;
        public float structuralIntegrity = 100f;
        public float ashBurialCm;
        public bool isCollapsed;
        public bool isScavenged;
        public int collapseDay = -1;
    }

    public sealed class LandmarkDegradationSystem
    {
        public const string SystemId = "landmark_degradation";
        private LandmarkSaveState _state = new LandmarkSaveState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        public LandmarkSaveState State => _state;
        public event Action<LandmarkStatusRecord> OnLandmarkCollapsed;

        public LandmarkDegradationSystem(ISeededRng rng = null!, ILog log = null!)
        {
            _rng = rng ?? new SeededRng(42);
            _log = log ?? NullLog.Instance;
        }

        public ActionResult RegisterLandmark(string landmarkId, string locationId, float integrity = 100f)
        {
            if (_state.landmarks.Exists(l => string.Equals(l.landmarkId, landmarkId, StringComparison.Ordinal)))
                return ActionResult.Blocked("landmark_exists", "landmark.landmark_exists");

            var landmark = new LandmarkStatusRecord
            {
                landmarkId = landmarkId,
                locationId = locationId,
                structuralIntegrity = integrity
            };
            _state.landmarks.Add(landmark);
            return ActionResult.Success("landmark.registered");
        }

        public ActionResult DamageLandmark(string landmarkId, float damage, int day)
        {
            var landmark = _state.landmarks.Find(l => string.Equals(l.landmarkId, landmarkId, StringComparison.Ordinal));
            if (landmark == null) return ActionResult.Failed("unknown_landmark", "landmark.unknown");

            landmark.structuralIntegrity = Math.Max(0f, landmark.structuralIntegrity - damage);
            if (landmark.structuralIntegrity <= 0f && !landmark.isCollapsed)
            {
                landmark.isCollapsed = true;
                landmark.collapseDay = day;
                OnLandmarkCollapsed?.Invoke(landmark);
            }
            return ActionResult.Success("landmark.damaged");
        }

        public void TickDay(int day, float weatherAshfallMm = 0f)
        {
            _state.lastDegradationDay = day;
            foreach (var l in _state.landmarks)
            {
                if (l.isCollapsed) continue;
                l.ashBurialCm += weatherAshfallMm * 0.1f;
                // Minor structural decay from weathering
                l.structuralIntegrity = Math.Max(0f, l.structuralIntegrity - 0.2f);
                if (l.structuralIntegrity <= 0f && !l.isCollapsed)
                {
                    l.isCollapsed = true;
                    l.collapseDay = day;
                    OnLandmarkCollapsed?.Invoke(l);
                }
            }
        }

        public LandmarkSaveState CaptureState() => _state;

        public void RestoreState(LandmarkSaveState saved)
        {
            if (saved == null) return;
            _state = saved;
        }
    }
}
