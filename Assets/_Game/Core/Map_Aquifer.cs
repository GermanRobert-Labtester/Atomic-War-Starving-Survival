using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AquiferState
    {
        public string locationId;
        public bool hasPumpInstalled;
        public float caveInRiskIncrease = 0.25f;
        public float floodingRiskIncrease = 0.30f;
        public bool providesInfiniteWater;
    }

    public class AquiferSystem
    {
        private readonly AquiferState _state;

        public AquiferState State => _state;

        public event Action<string> OnPumpInstalled;   // locationId
        public event Action<string> OnRiskChanged;     // locationId

        public AquiferSystem(string locationId)
        {
            _state = new AquiferState
            {
                locationId = locationId,
                hasPumpInstalled = false,
                caveInRiskIncrease = 0.25f,
                floodingRiskIncrease = 0.30f,
                providesInfiniteWater = false
            };
        }

        /// <summary>
        /// Install PumpModule. Grants infinite CleanWater immune to weather.
        /// </summary>
        public void InstallPump()
        {
            _state.hasPumpInstalled = true;
            _state.providesInfiniteWater = true;
            OnPumpInstalled?.Invoke(_state.locationId);
            OnRiskChanged?.Invoke(_state.locationId);
        }

        /// <summary>
        /// Returns water output per hour. Infinite when pump installed.
        /// </summary>
        public float GetWaterOutput()
        {
            return _state.hasPumpInstalled ? float.MaxValue : 0f;
        }

        /// <summary>
        /// Returns (caveInRiskMultiplier, floodingRiskMultiplier) after digging into aquifer.
        /// </summary>
        public (float caveInRisk, float floodingRisk) GetRiskMultipliers()
        {
            return (_state.caveInRiskIncrease, _state.floodingRiskIncrease);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public AquiferState CaptureState()
        {
            return new AquiferState
            {
                locationId = _state.locationId,
                hasPumpInstalled = _state.hasPumpInstalled,
                caveInRiskIncrease = _state.caveInRiskIncrease,
                floodingRiskIncrease = _state.floodingRiskIncrease,
                providesInfiniteWater = _state.providesInfiniteWater,
            };
        }

        public void RestoreState(AquiferState saved)
        {
            if (saved == null) return;
            _state.locationId = saved.locationId;
            _state.hasPumpInstalled = saved.hasPumpInstalled;
            _state.caveInRiskIncrease = saved.caveInRiskIncrease;
            _state.floodingRiskIncrease = saved.floodingRiskIncrease;
            _state.providesInfiniteWater = saved.providesInfiniteWater;
        }

}
}
