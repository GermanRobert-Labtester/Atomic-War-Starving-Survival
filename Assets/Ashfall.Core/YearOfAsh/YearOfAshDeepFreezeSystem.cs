using System;

namespace Ashfall.Core.YearOfAsh
{
    [Serializable]
    public class YearOfAshDeepFreezeState
    {
        public float indoorTemperatureCelsius = 18.0f; // Target 18-22C
        public float thermalInsulationQuality = 0.80f; // 0.0 to 1.0
        public float geothermalFlowRatePercent = 75.0f; // 0 to 100%
        public float intakeIceThicknessMm = 0.0f; // Intake icing (critical > 50mm)
        public bool isIntakeBlocked = false;
        public int daysFrozenPipelinesExperienced = 0;
    }

    /// <summary>
    /// Engine-agnostic Deep Freeze thermal management simulation for Phase IV (Days 180–240).
    /// Simulates surface heat loss at -38°C, geothermal glycol circulation, and intake icing.
    /// Pure C#; zero engine dependencies.
    /// </summary>
    public class YearOfAshDeepFreezeSystem
    {
        private readonly YearOfAshDeepFreezeState _state;

        public YearOfAshDeepFreezeState State => _state;
        public float IndoorTempCelsius => _state.indoorTemperatureCelsius;
        public float IntakeIceMm => _state.intakeIceThicknessMm;
        public bool IsIntakeBlocked => _state.isIntakeBlocked;

        public event Action<float> OnTemperatureChanged;
        public event Action<string> OnFreezeAlarmTriggered;

        public YearOfAshDeepFreezeSystem(YearOfAshDeepFreezeState state = null!)
        {
            _state = state ?? new YearOfAshDeepFreezeState();
        }

        public void TickDailyThermal(int day, float surfaceTempCelsius)
        {
            if (day > 240)
            {
                // Transition out of deep freeze
                _state.intakeIceThicknessMm = Math.Max(0.0f, _state.intakeIceThicknessMm - 5.0f);
                _state.isIntakeBlocked = false;
                _state.indoorTemperatureCelsius = Math.Min(21.0f, _state.indoorTemperatureCelsius + 0.5f);
                return;
            }

            // Heat balance equation
            float heatGainFromGeothermal = (_state.geothermalFlowRatePercent / 100.0f) * 26.0f;
            float heatLossToSurface = (20.0f - surfaceTempCelsius) * (1.0f - (_state.thermalInsulationQuality * 0.7f));
            float targetTemp = heatGainFromGeothermal - (heatLossToSurface * 0.35f);

            _state.indoorTemperatureCelsius = (_state.indoorTemperatureCelsius * 0.7f) + (targetTemp * 0.3f);

            // Intake icing accumulation in sub-zero weather
            if (surfaceTempCelsius < -15.0f)
            {
                float iceGain = Math.Abs(surfaceTempCelsius + 15.0f) * 0.8f;
                _state.intakeIceThicknessMm += iceGain;
            }
            else
            {
                _state.intakeIceThicknessMm = Math.Max(0.0f, _state.intakeIceThicknessMm - 2.0f);
            }

            if (_state.intakeIceThicknessMm >= 50.0f)
            {
                _state.isIntakeBlocked = true;
                OnFreezeAlarmTriggered?.Invoke("WARNING: Ventilation intake cowling is choked with rime ice!");
            }
            else
            {
                _state.isIntakeBlocked = false;
            }

            if (_state.indoorTemperatureCelsius <= 0.0f)
            {
                _state.daysFrozenPipelinesExperienced++;
            }

            OnTemperatureChanged?.Invoke(_state.indoorTemperatureCelsius);
        }

        public void ClearIntakeIce()
        {
            _state.intakeIceThicknessMm = 0.0f;
            _state.isIntakeBlocked = false;
        }

        public void UpgradeThermalInsulation(float boost)
        {
            _state.thermalInsulationQuality = Math.Min(1.0f, _state.thermalInsulationQuality + boost);
        }

        /// <summary>
        /// Snapshot for the save envelope. Returns a copy so later ticks cannot mutate
        /// a capture that is still on its way to disk.
        /// </summary>
        public YearOfAshDeepFreezeState CaptureState()
        {
            return new YearOfAshDeepFreezeState
            {
                indoorTemperatureCelsius = _state.indoorTemperatureCelsius,
                thermalInsulationQuality = _state.thermalInsulationQuality,
                geothermalFlowRatePercent = _state.geothermalFlowRatePercent,
                intakeIceThicknessMm = _state.intakeIceThicknessMm,
                isIntakeBlocked = _state.isIntakeBlocked,
                daysFrozenPipelinesExperienced = _state.daysFrozenPipelinesExperienced
            };
        }

        /// <summary>
        /// Rebuilds live state from a snapshot. Tolerates a null section so a save
        /// written before this section existed restores at the thermal defaults.
        /// </summary>
        public void RestoreState(YearOfAshDeepFreezeState state)
        {
            if (state == null) return;
            _state.indoorTemperatureCelsius = state.indoorTemperatureCelsius;
            _state.thermalInsulationQuality = state.thermalInsulationQuality;
            _state.geothermalFlowRatePercent = state.geothermalFlowRatePercent;
            _state.intakeIceThicknessMm = state.intakeIceThicknessMm;
            _state.isIntakeBlocked = state.isIntakeBlocked;
            _state.daysFrozenPipelinesExperienced = state.daysFrozenPipelinesExperienced;
            OnTemperatureChanged?.Invoke(_state.indoorTemperatureCelsius);
        }
    }
}
