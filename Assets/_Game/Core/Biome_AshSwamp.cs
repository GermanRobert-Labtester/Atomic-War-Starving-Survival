using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AshSwampState
    {
        public string biomeId = "biome_ash_swamp";
        public float movementSpeedMult = 0.5f;
        public bool stealthPossible = false;
        public float parasiteInfectionChance = 0.30f;
    }

    public class Biome_AshSwamp
    {
        public event Action<string> OnMovementPenalized;
        public event Action<string> OnParasiteContracted;
        public event Action<string> OnStealthFailed;

        private AshSwampState state;

        public Biome_AshSwamp()
        {
            state = new AshSwampState();
        }

        public void EnterBiome(string survivorId, System.Random rng)
        {
            OnMovementPenalized?.Invoke(survivorId);

            if (rng != null && (float)rng.NextDouble() < state.parasiteInfectionChance)
            {
                OnParasiteContracted?.Invoke(survivorId);
            }

            OnStealthFailed?.Invoke(survivorId);
        }

        public float GetMovementSpeedMult() => state.movementSpeedMult;

        public bool CanStealth() => false;

        public AshSwampState CaptureState() => state;

        public void RestoreState(AshSwampState saved)
        {
            state = saved ?? new AshSwampState();
        }
    }
}
