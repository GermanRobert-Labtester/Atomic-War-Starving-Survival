using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    public partial class LandmarkDegradationSystem
    {
        public LandmarkStatusRecord? TryGetLandmark(string landmarkId)
        {
            if (string.IsNullOrEmpty(landmarkId)) return null;
            return _state.landmarks.Find(l => string.Equals(l.landmarkId, landmarkId, StringComparison.Ordinal));
        }

        /// <summary>
        /// Scavengers picked the landmark clean. Recorded so the world shows
        /// the difference between a tower that fell and one that was taken.
        /// </summary>
        public ActionResult MarkScavenged(string landmarkId)
        {
            var landmark = TryGetLandmark(landmarkId);
            if (landmark == null) return ActionResult.Failed("unknown_landmark", "landmark.unknown");
            if (landmark.isScavenged) return ActionResult.Blocked("already_scavenged", "landmark.already_scavenged");
            landmark.isScavenged = true;
            return ActionResult.Success("landmark.scavenged");
        }

        /// <summary>Mean structural integrity of all registered landmarks, 0..100.</summary>
        public float MeanIntegrity()
        {
            if (_state.landmarks.Count == 0) return 100f;
            float sum = 0f;
            int count = 0;
            foreach (var l in _state.landmarks)
            {
                if (l == null) continue;
                sum += l.structuralIntegrity;
                count++;
            }
            return count > 0 ? sum / count : 100f;
        }
    }
}
