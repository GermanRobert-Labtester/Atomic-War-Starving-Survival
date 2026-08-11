using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Encounters
{
    [Serializable]
    public class FriendlyFireState
    {
        public string hazardId = "hazard_friendly_fire";
        public float friendlyFireChance = 0.15f;
        public float anxietyThreshold = 0.8f;
    }

    public class Hazard_FriendlyFire
    {
        public event Action<string, string> OnFriendlyFireHit;

        private FriendlyFireState _state;

        public Hazard_FriendlyFire()
        {
            _state = new FriendlyFireState();
        }

        public Hazard_FriendlyFire(FriendlyFireState state)
        {
            _state = state ?? new FriendlyFireState();
        }

        public FriendlyFireState CaptureState() => _state;

        public void RestoreState(FriendlyFireState state)
        {
            _state = state ?? new FriendlyFireState();
        }

        /// <summary>
        /// Checks whether a missed shot hits a friendly survivor in an adjacent lane.
        /// Triggers when the shooter's anxiety exceeds the threshold or they are concussed.
        /// Returns the hit ally's survivorId, or null if no friendly fire occurs.
        /// </summary>
        /// <param name="shooterId">ID of the survivor who fired.</param>
        /// <param name="anxiety">Shooter's current anxiety level (0-1 range).</param>
        /// <param name="isConcussed">Whether the shooter is concussed.</param>
        /// <param name="missedTargetId">ID of the intended target that was missed.</param>
        /// <param name="allies">List of allied survivors with their combat lanes (as strings).</param>
        /// <param name="rng">Random number generator for deterministic rolls.</param>
        /// <returns>The survivorId of the hit ally, or null.</returns>
        public string CheckFriendlyFire(
            string shooterId,
            float anxiety,
            bool isConcussed,
            string missedTargetId,
            List<(string survivorId, string lane)> allies,
            Random rng)
        {
            if (allies == null || allies.Count == 0) return null;
            if (string.IsNullOrEmpty(shooterId)) return null;

            bool highAnxiety = anxiety > _state.anxietyThreshold;
            if (!highAnxiety && !isConcussed) return null;

            // 15% chance of hitting a friendly in an adjacent lane
            float roll = (float)rng.NextDouble();
            if (roll > _state.friendlyFireChance) return null;

            // Find the shooter's lane to determine adjacent allies
            string shooterLane = null;
            for (int i = 0; i < allies.Count; i++)
            {
                if (allies[i].survivorId == shooterId)
                {
                    shooterLane = allies[i].lane;
                    break;
                }
            }

            // Collect eligible allies (adjacent lane, not the shooter)
            List<string> eligible = new List<string>();
            for (int i = 0; i < allies.Count; i++)
            {
                var ally = allies[i];
                if (ally.survivorId == shooterId) continue;
                if (IsAdjacentLane(shooterLane, ally.lane))
                {
                    eligible.Add(ally.survivorId);
                }
            }

            if (eligible.Count == 0) return null;

            // Pick a random eligible ally
            string hitAllyId = eligible[rng.Next(eligible.Count)];
            OnFriendlyFireHit?.Invoke(shooterId, hitAllyId);
            return hitAllyId;
        }

        /// <summary>
        /// Determines if two lanes are adjacent. Lanes are expected to be
        /// string identifiers; adjacency means they differ by one position.
        /// If either lane is null/unknown, treat as adjacent (worst case).
        /// </summary>
        private bool IsAdjacentLane(string laneA, string laneB)
        {
            if (string.IsNullOrEmpty(laneA) || string.IsNullOrEmpty(laneB))
                return true; // unknown lane = assume adjacent (conservative)

            // Attempt numeric parse for positional adjacency
            if (int.TryParse(laneA, out int a) && int.TryParse(laneB, out int b))
            {
                return Math.Abs(a - b) <= 1;
            }

            // For non-numeric lane ids, compare for inequality (any different lane is "adjacent")
            return laneA != laneB;
        }
    }
}
