using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;

namespace Ashfall.Core
{
    public enum CryogenicPlantBand
    {
        Offline,
        Ready,
        Running,
        Degraded,
        Failed
    }

    [Serializable]
    public sealed class CryogenicGasProduct
    {
        public string product_id = string.Empty;
        public int units_per_cycle;
    }

    [Serializable]
    public sealed class CryogenicAirSeparationState
    {
        public string system_id = CryogenicAirSeparationSystem.SystemId;
        public float plant_integrity = 100f;
        public float filter_condition = 100f;
        public float required_power_watts = 600f;
        public bool is_running;
        public int last_tick_day = -1;
        public int cycles_completed;
        public int cycles_blocked;
        public int failure_events;
    }

    /// <summary>
    /// Abstract cryogenic air-separation authority. It exposes only bounded
    /// plant bands, power gating, deterministic wear/failure, and inventory
    /// product grants. It does not model cryogenic thermodynamics or provide
    /// real-world operating instructions.
    /// </summary>
    public sealed class CryogenicAirSeparationSystem
    {
        public const string SystemId = "cryogenic_air_separation";
        public const float MinimumPowerWatts = 1f;
        public const float DailyIntegrityWear = 1.5f;
        public const float DailyFilterWear = 2f;

        private readonly Inventory.Inventory _inventory;
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly Func<float>? _availablePowerWatts;
        private CryogenicAirSeparationState _state = new CryogenicAirSeparationState();
        private readonly List<CryogenicGasProduct> _products = new List<CryogenicGasProduct>();

        public CryogenicAirSeparationState State => _state;
        public IReadOnlyList<CryogenicGasProduct> Products => _products;
        public CryogenicPlantBand Band => ResolveBand();

        public event Action<CryogenicPlantBand>? OnBandChanged;
        public event Action? OnCycleCompleted;
        public event Action<string>? OnFailure;
        public event Action? OnStateChanged;

        public CryogenicAirSeparationSystem(
            Inventory.Inventory inventory,
            ISeededRng rng,
            Func<float>? availablePowerWatts = null,
            ILog? log = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _availablePowerWatts = availablePowerWatts;
            _log = log ?? NullLog.Instance;
        }

        public void ConfigureProducts(IEnumerable<CryogenicGasProduct> products)
        {
            _products.Clear();
            if (products == null) return;
            foreach (var product in products)
            {
                if (product == null || string.IsNullOrEmpty(product.product_id) || product.units_per_cycle <= 0)
                    continue;
                _products.Add(new CryogenicGasProduct
                {
                    product_id = product.product_id,
                    units_per_cycle = product.units_per_cycle
                });
            }
        }

        public bool SetRunning(bool running)
        {
            var before = Band;
            if (running && ResolveBand() == CryogenicPlantBand.Failed)
                return false;
            _state.is_running = running;
            RaiseBandIfChanged(before);
            OnStateChanged?.Invoke();
            return true;
        }

        public bool Repair(float plantIntegrity, float filterCondition)
        {
            if (Band != CryogenicPlantBand.Failed && plantIntegrity <= 0f && filterCondition <= 0f)
                return false;

            var before = Band;
            _state.plant_integrity = Math.Clamp(_state.plant_integrity + Math.Max(0f, plantIntegrity), 0f, 100f);
            _state.filter_condition = Math.Clamp(_state.filter_condition + Math.Max(0f, filterCondition), 0f, 100f);
            RaiseBandIfChanged(before);
            OnStateChanged?.Invoke();
            return true;
        }

        public void TickDay(int day)
        {
            if (day < _state.last_tick_day) return;
            int elapsed = _state.last_tick_day < 0 ? 1 : Math.Max(0, day - _state.last_tick_day);
            _state.last_tick_day = day;
            if (elapsed == 0)
            {
                OnStateChanged?.Invoke();
                return;
            }

            var before = Band;
            if (!_state.is_running)
            {
                RaiseBandIfChanged(before);
                OnStateChanged?.Invoke();
                return;
            }

            if (ResolveAvailablePower() < Math.Max(MinimumPowerWatts, _state.required_power_watts))
            {
                _state.cycles_blocked += elapsed;
                OnFailure?.Invoke("insufficient_power");
                OnStateChanged?.Invoke();
                return;
            }

            for (int i = 0; i < elapsed; i++)
            {
                if (ResolveBand() == CryogenicPlantBand.Failed) break;
                if (!TryCompleteCycle()) break;
            }

            RaiseBandIfChanged(before);
            OnStateChanged?.Invoke();
        }

        public CryogenicAirSeparationState CaptureState() => CloneState(_state);

        public void RestoreState(CryogenicAirSeparationState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private bool TryCompleteCycle()
        {
            float failureChance = Math.Clamp(
                (100f - _state.plant_integrity) / 250f
                + (100f - _state.filter_condition) / 300f,
                0f,
                0.8f);
            if (failureChance > 0f && _rng.NextDouble() < failureChance)
            {
                _state.plant_integrity = Math.Max(0f, _state.plant_integrity - 8f);
                _state.failure_events++;
                OnFailure?.Invoke("normalized_plant_fault");
                _log.Warn("[Cryogenic] normalized plant fault");
                return false;
            }

            var bill = new InventoryBill();
            for (int i = 0; i < _products.Count; i++)
                bill.AddGrant(_products[i].product_id, _products[i].units_per_cycle);

            bool committed = _inventory.TryExecuteTransaction(bill, () =>
            {
                _state.plant_integrity = Math.Max(0f, _state.plant_integrity - DailyIntegrityWear);
                _state.filter_condition = Math.Max(0f, _state.filter_condition - DailyFilterWear);
                _state.cycles_completed++;
                OnCycleCompleted?.Invoke();
            });
            if (!committed)
            {
                _state.cycles_blocked++;
                OnFailure?.Invoke("product_storage_unavailable");
                return false;
            }
            return true;
        }

        private float ResolveAvailablePower()
            => _availablePowerWatts == null ? float.PositiveInfinity : Math.Max(0f, _availablePowerWatts());

        private CryogenicPlantBand ResolveBand()
        {
            if (_state.plant_integrity <= 0f || _state.filter_condition <= 0f)
                return CryogenicPlantBand.Failed;
            if (!_state.is_running) return CryogenicPlantBand.Offline;
            if (_state.plant_integrity < 35f || _state.filter_condition < 25f)
                return CryogenicPlantBand.Degraded;
            if (ResolveAvailablePower() < Math.Max(MinimumPowerWatts, _state.required_power_watts))
                return CryogenicPlantBand.Ready;
            return CryogenicPlantBand.Running;
        }

        private void RaiseBandIfChanged(CryogenicPlantBand before)
        {
            var after = ResolveBand();
            if (after != before) OnBandChanged?.Invoke(after);
        }

        private static CryogenicAirSeparationState CloneState(CryogenicAirSeparationState src)
        {
            if (src == null) return new CryogenicAirSeparationState();
            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(src);
            return serializer.Deserialize<CryogenicAirSeparationState>(json)
                ?? new CryogenicAirSeparationState();
        }
    }
}
