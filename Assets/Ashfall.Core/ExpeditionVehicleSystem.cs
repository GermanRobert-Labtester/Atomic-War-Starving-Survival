using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class ExpeditionVehicleState
    {
        public string systemId = ExpeditionVehicleSystem.SystemId;
        public Dictionary<string, VehicleInstance> ownedVehicles = new Dictionary<string, VehicleInstance>();
        public string activeExpeditionVehicleId = string.Empty;
    }

    [Serializable]
    public sealed class VehicleInstance
    {
        public string vehicleId = string.Empty;
        public string displayName = string.Empty;
        public float condition = 100f;
        public float fuel;
        public float maxFuel = 50f;
        public float cargoCapacity = 100f;
        public float speedMultiplier = 1f;
        public string terrainType = "road";
        public bool isBrokenDown;
        public string breakdownCause = string.Empty;
        public List<string> attachments = new List<string>();
    }

    [Serializable]
    public sealed class VehicleDefinition
    {
        public string vehicle_id = string.Empty;
        public string display_name = string.Empty;
        public float max_fuel = 50f;
        public float cargo_capacity = 100f;
        public float speed_multiplier = 1f;
        public string terrain_type = "road";
        public float condition_max = 100f;
        public float fuel_consumption_per_km = 0.5f;
        public float breakdown_threshold = 0.2f;
        public List<string> default_attachments = new List<string>();
    }

    [Serializable]
    public sealed class VehicleCatalog
    {
        public string schema_version = "1.0";
        public List<VehicleDefinition> vehicles = new List<VehicleDefinition>();
    }

    public sealed class ExpeditionVehicleSystem
    {
        public const string SystemId = "expedition_vehicle";
        private ExpeditionVehicleState _state = new ExpeditionVehicleState();
        private readonly Dictionary<string, VehicleDefinition> _catalog = new Dictionary<string, VehicleDefinition>(StringComparer.Ordinal);
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        public ExpeditionVehicleState State => _state;
        public event Action OnVehicleStateChanged;

        public ExpeditionVehicleSystem(ISeededRng rng, ILog log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        public void LoadCatalog(VehicleCatalog catalog)
        {
            if (catalog?.vehicles == null) return;
            _catalog.Clear();
            foreach (var v in catalog.vehicles)
                if (!string.IsNullOrEmpty(v.vehicle_id))
                    _catalog[v.vehicle_id] = v;
        }

        public VehicleDefinition? GetDefinition(string id)
        {
            _catalog.TryGetValue(id, out var def);
            return def;
        }

        public ActionResult AcquireVehicle(string vehicleId)
        {
            if (_state.ownedVehicles.ContainsKey(vehicleId))
                return ActionResult.Blocked("already_owned", "vehicle.already_owned");
            if (!_catalog.TryGetValue(vehicleId, out var def))
                return ActionResult.Failed("unknown_vehicle", "vehicle.unknown");

            _state.ownedVehicles[vehicleId] = new VehicleInstance
            {
                vehicleId = vehicleId,
                displayName = def.display_name,
                condition = def.condition_max,
                fuel = def.max_fuel * 0.5f,
                maxFuel = def.max_fuel,
                cargoCapacity = def.cargo_capacity,
                speedMultiplier = def.speed_multiplier,
                terrainType = def.terrain_type,
                attachments = new List<string>(def.default_attachments)
            };
            OnVehicleStateChanged?.Invoke();
            return ActionResult.Success("vehicle.acquired",
                new Dictionary<string, double> { { "fuel", def.max_fuel * 0.5f } });
        }

        public VehicleInstance? GetVehicle(string vehicleId)
        {
            _state.ownedVehicles.TryGetValue(vehicleId, out var v);
            return v;
        }

        public ActionResult Refuel(string vehicleId, float amount)
        {
            if (!_state.ownedVehicles.TryGetValue(vehicleId, out var v))
                return ActionResult.Failed("unknown_vehicle", "vehicle.unknown");
            float added = Math.Min(amount, v.maxFuel - v.fuel);
            v.fuel += added;
            OnVehicleStateChanged?.Invoke();
            return ActionResult.Success("vehicle.refueled",
                new Dictionary<string, double> { { "fuel_added", added }, { "fuel_total", v.fuel } });
        }

        public ActionResult Repair(string vehicleId, float amount)
        {
            if (!_state.ownedVehicles.TryGetValue(vehicleId, out var v))
                return ActionResult.Failed("unknown_vehicle", "vehicle.unknown");
            v.condition = Math.Min(100f, v.condition + amount);
            v.isBrokenDown = false;
            v.breakdownCause = string.Empty;
            OnVehicleStateChanged?.Invoke();
            return ActionResult.Success("vehicle.repaired",
                new Dictionary<string, double> { { "condition", v.condition } });
        }

        public ActionResult AttachEquipment(string vehicleId, string equipmentId)
        {
            if (!_state.ownedVehicles.TryGetValue(vehicleId, out var v))
                return ActionResult.Failed("unknown_vehicle", "vehicle.unknown");
            if (v.attachments.Contains(equipmentId))
                return ActionResult.Blocked("already_attached", "vehicle.already_attached");
            v.attachments.Add(equipmentId);
            OnVehicleStateChanged?.Invoke();
            return ActionResult.Success("vehicle.attached",
                new Dictionary<string, double> { { "attachments", v.attachments.Count } });
        }

        public (float fuelCost, float travelTimeMod, bool breakdown) PrepareForExpedition(string vehicleId, float distanceKm)
        {
            if (!_state.ownedVehicles.TryGetValue(vehicleId, out var v))
                return (0, 1f, false);

            float fuelNeeded = distanceKm * 0.5f; // 0.5 fuel per km
            if (_catalog.TryGetValue(vehicleId, out var def))
                fuelNeeded = distanceKm * def.fuel_consumption_per_km;

            if (v.fuel < fuelNeeded) return (fuelNeeded, 1f, false);

            v.fuel -= fuelNeeded;
            float wear = distanceKm * 0.5f;
            v.condition = Math.Max(0, v.condition - wear);

            bool breakdown = false;
            if (v.condition < 20f && _rng.NextDouble() < 0.3f)
            {
                breakdown = true;
                v.isBrokenDown = true;
                v.breakdownCause = $"vehicle broke down at {distanceKm}km (condition={v.condition:F1})";
            }

            OnVehicleStateChanged?.Invoke();
            return (fuelNeeded, v.speedMultiplier, breakdown);
        }

        public ExpeditionVehicleState CaptureState() => _state;
        public void RestoreState(ExpeditionVehicleState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnVehicleStateChanged?.Invoke();
        }
    }
}
