using System;
using AtomicWar._Game.Shelter.Modules;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>Loads supervised by the bunker climate terminal.</summary>
    public enum AirHeatLoad
    {
        AirFiltration,
        Heater
    }

    /// <summary>
    /// Read-only climate telemetry plus narrow controls for the filter and heater.
    /// Priority/request state remains owned and persisted by <see cref="PowerNetwork"/>,
    /// so this adapter never creates a second source of truth for the grid.
    /// </summary>
    public sealed class AirHeatManagementSystem : IDisposable
    {
        public const string SystemId = "air_heat_management";
        public const string AirFiltrationModuleId = "air_filtration";
        public const string HeaterModuleId = "heater";
        public const float FallbackFilterDegradationPerHour = 2f;
        public const float FallbackHeaterFuelBurnPerHour = 1f;

        private readonly Shelter _shelter;
        private readonly PowerNetwork _powerNetwork;
        private readonly Func<float> _getIndoorTemperature;
        private readonly Func<float> _getAmbientTemperature;
        private Func<AirHeatLoad, float> _getResourceConsumptionMultiplier;
        private AirHeatManagementSnapshot _lastSnapshot;

        /// <summary>Raised when any climate-terminal telemetry or controlled load state changes.</summary>
        public event Action OnChanged;

        public AirHeatManagementSystem(
            Shelter shelter,
            PowerNetwork powerNetwork,
            Func<float> getIndoorTemperature = null,
            Func<float> getAmbientTemperature = null)
        {
            _shelter = shelter;
            _powerNetwork = powerNetwork;
            _getIndoorTemperature = getIndoorTemperature;
            _getAmbientTemperature = getAmbientTemperature;
            if (_powerNetwork != null)
                _powerNetwork.OnPowerStateChanged += Refresh;
            _lastSnapshot = GetSnapshot();
        }

        /// <summary>
        /// Bind transient staffing/perk effects without making the climate terminal
        /// own another fuel or durability state. Values are clamped at read time.
        /// </summary>
        public void SetResourceConsumptionMultiplierProvider(Func<AirHeatLoad, float> provider)
        {
            _getResourceConsumptionMultiplier = provider;
            Refresh();
        }

        /// <summary>
        /// Build a presentation-only snapshot. Runtime estimates are operating
        /// hours at the current module rate; an unpowered module preserves its
        /// remaining service life but cannot provide climate protection.
        /// </summary>
        public AirHeatManagementSnapshot GetSnapshot()
        {
            var filter = _shelter != null ? _shelter.GetModule(AirFiltrationModuleId) : null;
            var heater = _shelter != null ? _shelter.GetModule(HeaterModuleId) : null;
            var filterLoad = _powerNetwork != null ? _powerNetwork.GetConsumer(AirFiltrationModuleId) : null;
            var heaterLoad = _powerNetwork != null ? _powerNetwork.GetConsumer(HeaterModuleId) : null;

            float filterRate = GetFilterDegradationRate(filter);
            float heaterBurnRate = GetHeaterFuelBurnRate(heater);
            float filterHealth = filter != null ? Mathf.Clamp(filter.FilterHealth, 0f, 100f) : 0f;
            float heaterFuel = heater != null ? Mathf.Max(0f, heater.Fuel) : 0f;

            return new AirHeatManagementSnapshot
            {
                IndoorTemperatureCelsius = _getIndoorTemperature != null ? _getIndoorTemperature() : 0f,
                AmbientTemperatureCelsius = _getAmbientTemperature != null ? _getAmbientTemperature() : 0f,
                AirQuality = _shelter != null ? _shelter.AirQuality : 0f,
                GridGenerationWatts = _powerNetwork != null ? _powerNetwork.TotalGeneration : 0f,
                GridDrawWatts = _powerNetwork != null ? _powerNetwork.TotalDraw : 0f,
                GridRequestedWatts = _powerNetwork != null ? _powerNetwork.RequestedDraw : 0f,
                IsBlackout = _powerNetwork != null && _powerNetwork.IsBlackout,
                IsLoadShedding = _powerNetwork != null && _powerNetwork.IsLoadShedding,
                FilterInstalled = filter != null && filter.Level > 0,
                FilterOperational = filter != null && filter.IsOperational && filter.FilterHealth > 0f,
                FilterHealth = filterHealth,
                FilterDegradationPerHour = filterRate,
                FilterRuntimeHours = ProjectRuntime(filterHealth, filterRate),
                FilterLoad = BuildLoadSnapshot(filterLoad),
                HeaterInstalled = heater != null && heater.Level > 0,
                HeaterOperational = heater != null && heater.IsOperational && heater.Fuel > 0f,
                HeaterFuel = heaterFuel,
                HeaterFuelBurnPerHour = heaterBurnRate,
                HeaterRuntimeHours = ProjectRuntime(heaterFuel, heaterBurnRate),
                HeaterLoad = BuildLoadSnapshot(heaterLoad)
            };
        }

        /// <summary>Move one climate load's priority, 1 = critical and 5 = shed first.</summary>
        public bool AdjustPriority(AirHeatLoad load, int direction)
        {
            if (_powerNetwork == null || direction == 0) return false;
            var consumer = _powerNetwork.GetConsumer(ModuleIdFor(load));
            if (consumer == null) return false;

            int next = Mathf.Clamp(consumer.Priority + (direction > 0 ? 1 : -1), 1, 5);
            if (next == consumer.Priority) return false;
            _powerNetwork.SetPriority(consumer.ModuleId, next);
            return true;
        }

        /// <summary>Request or unrequest power for a climate load through the grid.</summary>
        public bool ToggleRequested(AirHeatLoad load)
        {
            if (_powerNetwork == null) return false;
            var consumer = _powerNetwork.GetConsumer(ModuleIdFor(load));
            if (consumer == null) return false;
            _powerNetwork.SetRequested(consumer.ModuleId, !consumer.IsRequested);
            return true;
        }

        /// <summary>
        /// Compare live shelter/module telemetry after a simulation step. This is
        /// deliberately separate from grid events because fuel and filter wear
        /// change in Shelter.Tick before the next grid balance.
        /// </summary>
        public void Refresh()
        {
            var snapshot = GetSnapshot();
            if (SnapshotsEqual(_lastSnapshot, snapshot)) return;
            _lastSnapshot = snapshot;
            OnChanged?.Invoke();
        }

        public void Dispose()
        {
            if (_powerNetwork != null)
                _powerNetwork.OnPowerStateChanged -= Refresh;
        }

        private static string ModuleIdFor(AirHeatLoad load)
        {
            return load == AirHeatLoad.Heater ? HeaterModuleId : AirFiltrationModuleId;
        }

        private static ClimateLoadSnapshot BuildLoadSnapshot(PowerConsumer consumer)
        {
            return new ClimateLoadSnapshot
            {
                IsRegistered = consumer != null,
                IsRequested = consumer != null && consumer.IsRequested,
                IsPowered = consumer != null && consumer.IsPowered,
                IsShed = consumer != null && consumer.IsShed,
                Priority = consumer != null ? consumer.Priority : 0,
                Watts = consumer != null ? consumer.Watts : 0f
            };
        }

        private float GetFilterDegradationRate(ShelterModuleInstance module)
        {
            if (module == null) return 0f;
            float multiplier = GetResourceConsumptionMultiplier(AirHeatLoad.AirFiltration);
            if (module.Definition is AirFiltrationModuleSO airDefinition)
                return Mathf.Max(0f, airDefinition.DegradationRatePerHour) * multiplier;
            return FallbackFilterDegradationPerHour * multiplier;
        }

        private float GetHeaterFuelBurnRate(ShelterModuleInstance module)
        {
            if (module == null) return 0f;
            float baseRate = module.Definition is HeaterModuleSO heaterDefinition
                ? Mathf.Max(0f, heaterDefinition.FuelConsumptionRatePerHour)
                : FallbackHeaterFuelBurnPerHour;
            return baseRate * module.EffectiveFuelBurnMultiplier
                * GetResourceConsumptionMultiplier(AirHeatLoad.Heater);
        }

        private float GetResourceConsumptionMultiplier(AirHeatLoad load)
        {
            float multiplier = _getResourceConsumptionMultiplier != null
                ? _getResourceConsumptionMultiplier(load)
                : 1f;
            return Mathf.Clamp(multiplier, 0.01f, 1f);
        }

        private static float ProjectRuntime(float remaining, float ratePerHour)
        {
            return ratePerHour > 0f ? Mathf.Max(0f, remaining) / ratePerHour : 0f;
        }

        private static bool SnapshotsEqual(AirHeatManagementSnapshot left, AirHeatManagementSnapshot right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left == null || right == null) return false;
            return Mathf.Approximately(left.IndoorTemperatureCelsius, right.IndoorTemperatureCelsius)
                && Mathf.Approximately(left.AmbientTemperatureCelsius, right.AmbientTemperatureCelsius)
                && Mathf.Approximately(left.AirQuality, right.AirQuality)
                && Mathf.Approximately(left.GridGenerationWatts, right.GridGenerationWatts)
                && Mathf.Approximately(left.GridDrawWatts, right.GridDrawWatts)
                && Mathf.Approximately(left.GridRequestedWatts, right.GridRequestedWatts)
                && Mathf.Approximately(left.FilterHealth, right.FilterHealth)
                && Mathf.Approximately(left.HeaterFuel, right.HeaterFuel)
                && Mathf.Approximately(left.FilterRuntimeHours, right.FilterRuntimeHours)
                && Mathf.Approximately(left.HeaterRuntimeHours, right.HeaterRuntimeHours)
                && LoadEqual(left.FilterLoad, right.FilterLoad)
                && LoadEqual(left.HeaterLoad, right.HeaterLoad)
                && left.IsBlackout == right.IsBlackout
                && left.IsLoadShedding == right.IsLoadShedding;
        }

        private static bool LoadEqual(ClimateLoadSnapshot left, ClimateLoadSnapshot right)
        {
            if (left == null || right == null) return left == right;
            return left.IsRegistered == right.IsRegistered
                && left.IsRequested == right.IsRequested
                && left.IsPowered == right.IsPowered
                && left.IsShed == right.IsShed
                && left.Priority == right.Priority
                && Mathf.Approximately(left.Watts, right.Watts);
        }
    }

    /// <summary>Serializable presentation snapshot for the air/heat terminal.</summary>
    [Serializable]
    public sealed class AirHeatManagementSnapshot
    {
        public float IndoorTemperatureCelsius;
        public float AmbientTemperatureCelsius;
        public float AirQuality;
        public float GridGenerationWatts;
        public float GridDrawWatts;
        public float GridRequestedWatts;
        public bool IsBlackout;
        public bool IsLoadShedding;
        public bool FilterInstalled;
        public bool FilterOperational;
        public float FilterHealth;
        public float FilterDegradationPerHour;
        public float FilterRuntimeHours;
        public ClimateLoadSnapshot FilterLoad;
        public bool HeaterInstalled;
        public bool HeaterOperational;
        public float HeaterFuel;
        public float HeaterFuelBurnPerHour;
        public float HeaterRuntimeHours;
        public ClimateLoadSnapshot HeaterLoad;
    }

    /// <summary>Grid state for one climate load, copied so UI never mutates the consumer directly.</summary>
    [Serializable]
    public sealed class ClimateLoadSnapshot
    {
        public bool IsRegistered;
        public bool IsRequested;
        public bool IsPowered;
        public bool IsShed;
        public int Priority;
        public float Watts;
    }
}
