using System;

namespace AtomicWar._Game.Shelter.Modules
{
    [Serializable]
    public class LatheState
    {
        public string moduleId = "shelter_module_lathe";
        public float powerRequired = 100f;
        public float noiseLevel = 1.0f;
        public bool isActive;
    }

    public class ShelterModule_Lathe
    {
        public event Action<string, int> OnPartsProduced;
        public event Action<string> OnNoisePollution;

        private LatheState _state;

        public ShelterModule_Lathe()
        {
            _state = new LatheState();
        }

        public ShelterModule_Lathe(LatheState state)
        {
            _state = state ?? new LatheState();
        }

        public LatheState CaptureState() => _state;

        public void RestoreState(LatheState state)
        {
            _state = state ?? new LatheState();
        }

        public int ConvertRawMetal(string roomId, int rawMetalCount, float availablePower)
        {
            if (availablePower < _state.powerRequired)
                return 0;

            int partsProduced = rawMetalCount;
            _state.isActive = true;

            OnPartsProduced?.Invoke(roomId, partsProduced);
            OnNoisePollution?.Invoke(roomId);

            return partsProduced;
        }

        public void Tick(float deltaTime, float availablePower)
        {
            _state.isActive = availablePower >= _state.powerRequired;
        }
    }
}
