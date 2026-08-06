using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SuburbsState
    {
        public string biomeId = "biome_suburbs";
        public float lootDensity = 0.8f;
        public float banditSpawnDensity = 0.9f;
        public int fogOfWarRadius = 1;
    }

    public enum HouseResult
    {
        Empty,
        Loot,
        Ambush,
        Trap
    }

    public class Biome_Suburbs
    {
        public event Action<string, string> OnAmbushTriggered;
        public event Action<string> OnFogOfWarRevealed;
        public event Action<string, int> OnLootFound;

        private SuburbsState state;

        public Biome_Suburbs()
        {
            state = new SuburbsState();
        }

        public HouseResult ScoutHouse(string survivorId, string houseId, System.Random rng)
        {
            if (rng == null) return HouseResult.Empty;

            float roll = (float)rng.NextDouble();

            if (roll < state.banditSpawnDensity * 0.4f)
            {
                OnAmbushTriggered?.Invoke(survivorId, houseId);
                return HouseResult.Ambush;
            }

            if (roll < state.banditSpawnDensity * 0.4f + 0.1f)
            {
                return HouseResult.Trap;
            }

            if (roll < state.banditSpawnDensity * 0.4f + 0.1f + state.lootDensity * 0.4f)
            {
                int count = rng.Next(1, 5);
                OnLootFound?.Invoke(survivorId, count);
                return HouseResult.Loot;
            }

            return HouseResult.Empty;
        }

        public void RevealFog(string tileId)
        {
            OnFogOfWarRevealed?.Invoke(tileId);
        }

        public List<string> GetAdjacentTileIds(string currentTileId)
        {
            var adjacent = new List<string>();
            if (string.IsNullOrEmpty(currentTileId)) return adjacent;

            for (int dx = -state.fogOfWarRadius; dx <= state.fogOfWarRadius; dx++)
            {
                for (int dy = -state.fogOfWarRadius; dy <= state.fogOfWarRadius; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    adjacent.Add($"{currentTileId}_{dx}_{dy}");
                }
            }

            return adjacent;
        }

        public SuburbsState CaptureState() => state;

        public void RestoreState(SuburbsState saved)
        {
            state = saved ?? new SuburbsState();
        }
    }
}
