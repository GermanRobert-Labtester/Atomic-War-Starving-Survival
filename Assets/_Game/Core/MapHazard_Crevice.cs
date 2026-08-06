using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class CreviceState
    {
        public string hazardId = "map_hazard_crevice";
        public float agilityThreshold = 0.7f;
        public bool requiresBridge = false;
        public bool isBlocking = true;
        public bool bridgeBuilt = false;
    }

    public class MapHazard_Crevice
    {
        public event Action<string> OnJumpSucceeded;
        public event Action<string> OnJumpFailed;
        public event Action<string> OnBridgeBuilt;

        private CreviceState state;

        public MapHazard_Crevice()
        {
            state = new CreviceState();
        }

        public bool AttemptJump(string survivorId, float agility, System.Random rng)
        {
            if (rng == null) return false;

            if (state.bridgeBuilt)
            {
                OnJumpSucceeded?.Invoke(survivorId);
                return true;
            }

            if (agility >= state.agilityThreshold)
            {
                OnJumpSucceeded?.Invoke(survivorId);
                return true;
            }

            float roll = (float)rng.NextDouble();
            float failChance = state.agilityThreshold - agility;

            if (roll < failChance)
            {
                OnJumpFailed?.Invoke(survivorId);
                return false;
            }

            OnJumpSucceeded?.Invoke(survivorId);
            return true;
        }

        public bool BuildScrapBridge(string survivorId, int scrapCount)
        {
            if (scrapCount < 5) return false;

            state.bridgeBuilt = true;
            state.isBlocking = false;
            OnBridgeBuilt?.Invoke(survivorId);
            return true;
        }

        public bool IsBlocking() => state.isBlocking && !state.bridgeBuilt;

        public CreviceState CaptureState() => state;

        public void RestoreState(CreviceState saved)
        {
            state = saved ?? new CreviceState();
        }
    }
}
