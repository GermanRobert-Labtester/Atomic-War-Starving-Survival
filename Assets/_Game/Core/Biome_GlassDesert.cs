using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class GlassDesertState
    {
        public string biomeId = "biome_glass_desert";
        public float coverAvailable = 0f;
        public float sniperAccuracy = 1.0f;
        public float temperatureCelsius = 40f;
        public bool yieldsVitrifiedGlass = true;
    }

    public class Biome_GlassDesert
    {
        public event Action<string> OnSniperExposed;
        public event Action<string> OnHeatStress;
        public event Action<string, int> OnVitrifiedGlassFound;

        private GlassDesertState state;

        public Biome_GlassDesert()
        {
            state = new GlassDesertState();
        }

        public void EnterBiome(string survivorId)
        {
            OnSniperExposed?.Invoke(survivorId);
            OnHeatStress?.Invoke(survivorId);
        }

        public float GetCover() => state.coverAvailable;

        public float GetTemperature() => state.temperatureCelsius;

        public int Scavenge(string survivorId, System.Random rng)
        {
            if (rng == null) return 0;

            int glassCount = 0;
            float roll = (float)rng.NextDouble();
            if (roll < 0.15f)
            {
                glassCount = rng.Next(1, 4);
            }

            if (glassCount > 0)
            {
                OnVitrifiedGlassFound?.Invoke(survivorId, glassCount);
            }

            return glassCount;
        }

        public GlassDesertState CaptureState() => state;

        public void RestoreState(GlassDesertState saved)
        {
            state = saved ?? new GlassDesertState();
        }
    }
}
