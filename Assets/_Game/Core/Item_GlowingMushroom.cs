using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class GlowingMushroomState
    {
        public string itemId;
        public float lightOutput = 0.3f;
        public float radiationPerHour = 2f;
        public int chemicalScrapYield = 5;
        public bool isHarvestable;
        public string roomId = string.Empty;
    }

    public class GlowingMushroomSystem
    {
        private readonly GlowingMushroomState _state;

        public GlowingMushroomState State => _state;

        public event Action<string, string> OnSpawned;    // itemId, roomId
        public event Action<string, int> OnHarvested;     // itemId, scrapYield

        public GlowingMushroomSystem(string itemId)
        {
            _state = new GlowingMushroomState
            {
                itemId = itemId,
                lightOutput = 0.3f,
                radiationPerHour = 2f,
                chemicalScrapYield = 5,
                isHarvestable = false,
                roomId = string.Empty
            };
        }

        /// <summary>
        /// Spawns in dark, humid deep rooms. Provides passive light.
        /// </summary>
        public bool SpawnInRoom(string roomId, float humidity, float lightLevel)
        {
            if (humidity < 0.6f || lightLevel > 0.2f)
                return false;

            _state.roomId = roomId;
            _state.isHarvestable = true;
            OnSpawned?.Invoke(_state.itemId, roomId);
            return true;
        }

        public float GetLightOutput()
        {
            return _state.isHarvestable ? _state.lightOutput : 0f;
        }

        public float GetRadiation()
        {
            return _state.isHarvestable ? _state.radiationPerHour : 0f;
        }

        /// <summary>
        /// Harvest for ChemicalScrap. Returns yield amount.
        /// </summary>
        public int Harvest()
        {
            if (!_state.isHarvestable)
                return 0;

            int yield = _state.chemicalScrapYield;
            _state.isHarvestable = false;
            OnHarvested?.Invoke(_state.itemId, yield);
            return yield;
        }
    }
}
