using System;

namespace AtomicWar._Game.Shelter.Modules
{
    [Serializable]
    public class ReloaderState
    {
        public string moduleId = "shelter_module_reloader";
        public float dudChance = 0.05f;
    }

    public class ShelterModule_Reloader
    {
        public event Action<string, int> OnAmmoReloaded;
        public event Action<string, int> OnDudProduced;

        private ReloaderState _state;

        public ShelterModule_Reloader()
        {
            _state = new ReloaderState();
        }

        public ShelterModule_Reloader(ReloaderState state)
        {
            _state = state ?? new ReloaderState();
        }

        public ReloaderState CaptureState() => _state;

        public void RestoreState(ReloaderState state)
        {
            _state = state ?? new ReloaderState();
        }

        public (int liveRounds, int duds) ReloadAmmo(string survivorId, int spentBrassCount, Random rng)
        {
            int liveRounds = 0;
            int duds = 0;

            for (int i = 0; i < spentBrassCount; i++)
            {
                double roll = rng.NextDouble();
                if (roll < _state.dudChance)
                {
                    duds++;
                }
                else
                {
                    liveRounds++;
                }
            }

            if (liveRounds > 0)
                OnAmmoReloaded?.Invoke(survivorId, liveRounds);

            if (duds > 0)
                OnDudProduced?.Invoke(survivorId, duds);

            return (liveRounds, duds);
        }
    }
}
