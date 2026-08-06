using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class MagmaTapState
    {
        public string moduleId;
        public float powerOutput = 1000f;
        public float heatOutput = 30f;
        public int requiredDepth = 10;
        public bool isInstalled;
        public bool isVented;
    }

    public class MagmaTapSystem
    {
        private readonly MagmaTapState _state;

        public MagmaTapState State => _state;

        public event Action<string> OnInstalled;             // moduleId
        public event Action<string, float> OnHeatChanged;    // moduleId, tempIncrease

        public MagmaTapSystem(string moduleId)
        {
            _state = new MagmaTapState
            {
                moduleId = moduleId,
                powerOutput = 1000f,
                heatOutput = 30f,
                requiredDepth = 10,
                isInstalled = false,
                isVented = false
            };
        }

        /// <summary>
        /// Install at required depth with optional venting. Without venting → Heatstroke risk.
        /// </summary>
        public bool Install(int currentDepth, bool hasVenting)
        {
            if (currentDepth < _state.requiredDepth)
                return false;

            _state.isInstalled = true;
            _state.isVented = hasVenting;
            OnInstalled?.Invoke(_state.moduleId);
            OnHeatChanged?.Invoke(_state.moduleId, GetTemperatureIncrease());
            return true;
        }

        public float GetPowerOutput()
        {
            return _state.isInstalled ? _state.powerOutput : 0f;
        }

        /// <summary>
        /// Returns ambient temperature increase. Unvented = full heat output.
        /// </summary>
        public float GetTemperatureIncrease()
        {
            if (!_state.isInstalled)
                return 0f;
            return _state.isVented ? _state.heatOutput * 0.3f : _state.heatOutput;
        }
    }
}
