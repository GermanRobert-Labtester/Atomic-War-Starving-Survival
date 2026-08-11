using System;

namespace AtomicWar._Game.World
{
    [Serializable]
    public class SkyscraperTopsState
    {
        public string biomeId = "biome_skyscraper_tops";
        public bool requiresRope = true;
        public float fallDeathChance = 0.15f;
        public float coldExposure = 0.7f;
        public bool windTurbineScavengeable = true;
    }

    public class Biome_SkyscraperTops
    {
        public event Action<string> OnFallHazard;
        public event Action<string> OnColdExposure;
        public event Action<string, string> OnWindTurbineScavenged;

        private SkyscraperTopsState state;

        public Biome_SkyscraperTops()
        {
            state = new SkyscraperTopsState();
        }

        public bool Traverse(string survivorId, bool hasRope, System.Random rng)
        {
            OnColdExposure?.Invoke(survivorId);

            if (rng == null) return true;

            float deathChance = hasRope ? state.fallDeathChance * 0.25f : state.fallDeathChance;

            if ((float)rng.NextDouble() < deathChance)
            {
                OnFallHazard?.Invoke(survivorId);
                return false;
            }

            return true;
        }

        public string ScavengeTurbine(string survivorId, System.Random rng)
        {
            if (rng == null || !state.windTurbineScavengeable) return null;

            float roll = (float)rng.NextDouble();
            if (roll < 0.35f)
            {
                string[] parts = { "turbine_blade", "copper_coil", "bearing_assembly", "charge_controller" };
                string partId = parts[rng.Next(parts.Length)];
                OnWindTurbineScavenged?.Invoke(survivorId, partId);
                return partId;
            }

            return null;
        }

        public float GetColdExposure() => state.coldExposure;

        public SkyscraperTopsState CaptureState() => state;

        public void RestoreState(SkyscraperTopsState saved)
        {
            state = saved ?? new SkyscraperTopsState();
        }
    }
}
