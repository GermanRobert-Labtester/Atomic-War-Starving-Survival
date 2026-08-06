using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class HighwayTunnelState
    {
        public string biomeId = "biome_highway_tunnel";
        public bool isWeatherImmune = true;
        public bool isPitchBlack = true;
        public bool requiresLight = true;
    }

    public class Biome_HighwayTunnel
    {
        public event Action<string> OnDarknessPenalty;
        public event Action<string, string> OnGasSiphoned;
        public event Action<string, string> OnMutantEncounter;

        private HighwayTunnelState state;

        public Biome_HighwayTunnel()
        {
            state = new HighwayTunnelState();
        }

        public void EnterBiome(string survivorId, bool hasFlashlight, bool hasNVG)
        {
            if (!hasFlashlight && !hasNVG)
            {
                OnDarknessPenalty?.Invoke(survivorId);
            }
        }

        public float SiphonGas(string survivorId, string vehicleId, System.Random rng)
        {
            if (rng == null) return 0f;

            float fuelGained = (float)(rng.NextDouble() * 3.0 + 0.5);
            OnGasSiphoned?.Invoke(survivorId, vehicleId);
            return fuelGained;
        }

        public string CheckMutantEncounter(string survivorId, System.Random rng)
        {
            if (rng == null) return null;

            float roll = (float)rng.NextDouble();
            if (roll < 0.25f)
            {
                string[] types = { "mutant_rat", "mutant_dog", "mutant_crawler" };
                string mutantType = types[rng.Next(types.Length)];
                OnMutantEncounter?.Invoke(survivorId, mutantType);
                return mutantType;
            }

            return null;
        }

        public bool IsWeatherImmune() => true;

        public HighwayTunnelState CaptureState() => state;

        public void RestoreState(HighwayTunnelState saved)
        {
            state = saved ?? new HighwayTunnelState();
        }
    }
}
