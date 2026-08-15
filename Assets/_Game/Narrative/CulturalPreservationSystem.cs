using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Narrative
{
    /// <summary>
    /// Cultural Preservation System (#73) — preserving books, music
    /// records, artwork, and historical seeds unlocks permanent
    /// psychological resilience for current and future bunker
    /// generations.
    ///
    /// Owns: Survivor.CulturalArtifactsPreserved, Survivor.CulturalResilience.
    /// </summary>
    public class CulturalPreservationSystem
    {
        public const int ArtifactsForTier1 = 3;
        public const int ArtifactsForTier2 = 7;
        public const int ArtifactsForTier3 = 12;
        public const float Tier1Resilience = 0.15f;
        public const float Tier2Resilience = 0.30f;
        public const float Tier3Resilience = 0.50f;
        public const float ResilienceMoraleDecayReduction = 0.5f;

        public event Action<int, float> OnResilienceTierReached;
        // tier (1-3), resilienceValue
        public event Action<string> OnArtifactPreserved;

        private readonly HashSet<string> _preservedArtifactIds = new HashSet<string>();
        private float _bunkerCulturalResilience;
        private int _currentTier;

        public float BunkerCulturalResilience => _bunkerCulturalResilience;
        public int CurrentTier => _currentTier;

        public bool PreserveArtifact(string artifactId, Survivor preserver)
        {
            if (!_preservedArtifactIds.Add(artifactId)) return false;

            if (preserver != null)
            {
                preserver.CulturalArtifactsPreserved++;
                preserver.CulturalResilience = _bunkerCulturalResilience;
            }

            OnArtifactPreserved?.Invoke(artifactId);
            RecalculateResilience();
            return true;
        }

        private void RecalculateResilience()
        {
            int count = _preservedArtifactIds.Count;
            int newTier;
            float resilience;

            if (count >= ArtifactsForTier3)
            {
                newTier = 3;
                resilience = Tier3Resilience;
            }
            else if (count >= ArtifactsForTier2)
            {
                newTier = 2;
                resilience = Tier2Resilience;
            }
            else if (count >= ArtifactsForTier1)
            {
                newTier = 1;
                resilience = Tier1Resilience;
            }
            else
            {
                newTier = 0;
                resilience = 0f;
            }

            if (newTier != _currentTier)
            {
                _currentTier = newTier;
                _bunkerCulturalResilience = resilience;
                if (newTier > 0)
                    OnResilienceTierReached?.Invoke(newTier, resilience);
            }
        }

        public float GetMoraleDecayMultiplier()
        {
            return 1f - (_bunkerCulturalResilience * ResilienceMoraleDecayReduction);
        }
    }
}
