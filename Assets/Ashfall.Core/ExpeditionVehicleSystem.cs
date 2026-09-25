// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Expeditions;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    /// <summary>CORE-MECH W4 — what a vehicle breakdown costs the crew.</summary>
    public enum VehicleBreakdownKind
    {
        None = 0,
        Injury = 1,
        RadiationExposure = 2,
        Contamination = 3
    }

    /// <summary>
    /// CORE-MECH W4 — typed breakdown consequence. Pure data: the owner computes
    /// the band and raises it; the host routes it into the medical, dose, and
    /// disease owners (AA.2/AA.4 receiver contracts). Nothing here is persisted
    /// beyond the vehicle's own isBrokenDown / breakdownCause state.
    /// </summary>
    public sealed class VehicleBreakdownOutcome
    {
        /// <summary>True when the vehicle actually broke (independent of crew band).</summary>
        public bool BrokeDown { get; }

        public VehicleBreakdownKind Kind { get; }
        public float Severity01 { get; }
        public float NominalMsV { get; }
        public string Cause { get; }
        public string VehicleId { get; }

        /// <summary>
        /// CORE-MECH W4 — survivor the crew consequence applies to; filled by the
        /// host at dispatch (empty when the caller supplied no survivor).
        /// </summary>
        public string SurvivorId { get; set; } = string.Empty;

        public VehicleBreakdownOutcome(
            VehicleBreakdownKind kind, float severity01, float nominalMsV, string cause, string vehicleId,
            bool brokeDown = false)
        {
            Kind = kind;
            Severity01 = severity01;
            NominalMsV = nominalMsV;
            Cause = cause ?? string.Empty;
            VehicleId = vehicleId ?? string.Empty;
            BrokeDown = brokeDown;
        }

        /// <summary>No breakdown occurred (unknown vehicle, already broken, clean dispatch).</summary>
        public static VehicleBreakdownOutcome None(string vehicleId, string reason)
            => new VehicleBreakdownOutcome(VehicleBreakdownKind.None, 0f, 0f, reason, vehicleId);
    }

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
        public VehicleTrackGearState trackGear = new VehicleTrackGearState();
    }

    /// <summary>
    /// Persisted, normalized track-gear facts for one vehicle. Track gear is
    /// an attachment effect, not a second vehicle or terrain simulator.
    /// </summary>
    [Serializable]
    public sealed class VehicleTrackGearState
    {
        public string gearId = string.Empty;
        public float condition = 100f;
        public float tractionMultiplier = 1f;
        public float breakdownRiskMultiplier = 1f;

        public bool IsInstalled => !string.IsNullOrEmpty(gearId);

        public float EffectiveTractionMultiplier()
        {
            float condition01 = Math.Clamp(condition / 100f, 0f, 1f);
            float configured = Math.Clamp(tractionMultiplier, 0.5f, 2f);
            return 1f + (configured - 1f) * condition01;
        }

        public float EffectiveBreakdownRiskMultiplier()
        {
            float condition01 = Math.Clamp(condition / 100f, 0f, 1f);
            float configured = Math.Clamp(breakdownRiskMultiplier, 0f, 1f);
            return 1f + (configured - 1f) * condition01;
        }
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
    public sealed class VehicleTrackGearDefinition
    {
        public string gear_id = string.Empty;
        public string display_name = string.Empty;
        public float traction_multiplier = 1.1f;
        public float breakdown_risk_multiplier = 0.9f;
    }

    [Serializable]
    public sealed class VehicleCatalog
    {
        public int schema_version = 1;
        public List<VehicleDefinition> vehicles = new List<VehicleDefinition>();
        public List<VehicleTrackGearDefinition> track_gear = new List<VehicleTrackGearDefinition>();
    }

    public sealed class ExpeditionVehicleSystem
    {
        public const string SystemId = "expedition_vehicle";
        private ExpeditionVehicleState _state = new ExpeditionVehicleState();
        private readonly Dictionary<string, VehicleDefinition> _catalog = new Dictionary<string, VehicleDefinition>(StringComparer.Ordinal);
        private readonly Dictionary<string, VehicleTrackGearDefinition> _trackGearCatalog = new Dictionary<string, VehicleTrackGearDefinition>(StringComparer.Ordinal);
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        public ExpeditionVehicleState State => _state;
        public event Action OnVehicleStateChanged;

        public ExpeditionVehicleSystem(ISeededRng rng, ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        public void LoadCatalog(VehicleCatalog catalog)
        {
            if (catalog?.vehicles == null) return;
            _catalog.Clear();
            _trackGearCatalog.Clear();
            foreach (var v in catalog.vehicles)
                if (!string.IsNullOrEmpty(v.vehicle_id))
                    _catalog[v.vehicle_id] = v;
            foreach (var gear in catalog.track_gear)
                if (!string.IsNullOrEmpty(gear.gear_id))
                    _trackGearCatalog[gear.gear_id] = gear;
        }

        public VehicleDefinition? GetDefinition(string id)
        {
            _catalog.TryGetValue(id, out var def);
            return def;
        }

        public VehicleTrackGearDefinition? GetTrackGearDefinition(string id)
        {
            _trackGearCatalog.TryGetValue(id, out var def);
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

        /// <summary>
        /// Install normalized track gear on a vehicle. The host supplies the
        /// authored gear facts; this authority owns the resulting condition
        /// and projects it into expedition mobility.
        /// </summary>
        public ActionResult InstallTrackGear(
            string vehicleId,
            string gearId,
            float tractionMultiplier,
            float breakdownRiskMultiplier,
            float condition = 100f)
        {
            if (!_state.ownedVehicles.TryGetValue(vehicleId, out var v))
                return ActionResult.Failed("unknown_vehicle", "vehicle.unknown");
            if (string.IsNullOrEmpty(gearId))
                return ActionResult.Blocked("invalid_track_gear", "vehicle.invalid_track_gear");
            if (tractionMultiplier < 0.5f || tractionMultiplier > 2f
                || breakdownRiskMultiplier < 0f || breakdownRiskMultiplier > 1f)
                return ActionResult.Blocked("invalid_track_gear", "vehicle.invalid_track_gear");

            v.trackGear = new VehicleTrackGearState
            {
                gearId = gearId,
                condition = Math.Clamp(condition, 0f, 100f),
                tractionMultiplier = tractionMultiplier,
                breakdownRiskMultiplier = breakdownRiskMultiplier
            };
            OnVehicleStateChanged?.Invoke();
            return ActionResult.Success("vehicle.track_gear_installed",
                new Dictionary<string, double>
                {
                    { "traction_multiplier", v.trackGear.EffectiveTractionMultiplier() },
                    { "breakdown_risk_multiplier", v.trackGear.EffectiveBreakdownRiskMultiplier() }
                });
        }

        public ActionResult InstallTrackGear(string vehicleId, string gearId, float condition = 100f)
        {
            if (!_trackGearCatalog.TryGetValue(gearId, out var def))
                return ActionResult.Failed("unknown_track_gear", "vehicle.unknown_track_gear");
            return InstallTrackGear(
                vehicleId,
                gearId,
                def.traction_multiplier,
                def.breakdown_risk_multiplier,
                condition);
        }

        public ActionResult RemoveTrackGear(string vehicleId)
        {
            if (!_state.ownedVehicles.TryGetValue(vehicleId, out var v))
                return ActionResult.Failed("unknown_vehicle", "vehicle.unknown");
            if (!v.trackGear.IsInstalled)
                return ActionResult.Blocked("no_track_gear", "vehicle.no_track_gear");

            v.trackGear = new VehicleTrackGearState();
            OnVehicleStateChanged?.Invoke();
            return ActionResult.Success("vehicle.track_gear_removed");
        }

        public ActionResult RepairTrackGear(string vehicleId, float amount)
        {
            if (!_state.ownedVehicles.TryGetValue(vehicleId, out var v))
                return ActionResult.Failed("unknown_vehicle", "vehicle.unknown");
            if (!v.trackGear.IsInstalled)
                return ActionResult.Blocked("no_track_gear", "vehicle.no_track_gear");

            v.trackGear.condition = Math.Clamp(v.trackGear.condition + Math.Max(0f, amount), 0f, 100f);
            OnVehicleStateChanged?.Invoke();
            return ActionResult.Success("vehicle.track_gear_repaired",
                new Dictionary<string, double> { { "condition", v.trackGear.condition } });
        }

        /// <summary>
        /// Project the garage facts into the existing expedition profile.
        /// This keeps vehicle preparation and travel as separate authorities.
        /// </summary>
        public ExpeditionVehicleProfile? CreateExpeditionProfile(
            string vehicleId,
            float kmPerTravelTick = 2.5f)
        {
            if (!_state.ownedVehicles.TryGetValue(vehicleId, out var v))
                return null;

            var def = _catalog.TryGetValue(vehicleId, out var catalogDef) ? catalogDef : null;
            float consumption = def?.fuel_consumption_per_km ?? 0.5f;
            var gear = v.trackGear ?? new VehicleTrackGearState();
            return new ExpeditionVehicleProfile
            {
                vehicleId = vehicleId,
                speedMultiplier = Math.Max(0.01f, v.speedMultiplier * gear.EffectiveTractionMultiplier()),
                cargoCapacityKg = v.cargoCapacity,
                fuelPerTravelTick = Math.Max(0f, consumption * Math.Max(0f, kmPerTravelTick)),
                breakdownChancePerTick = Math.Clamp(
                    (100f - v.condition) / 100f * 0.15f * gear.EffectiveBreakdownRiskMultiplier(),
                    0f,
                    1f)
            };
        }

        public (float fuelCost, float travelTimeMod, bool breakdown) PrepareForExpedition(string vehicleId, float distanceKm)
        {
            return PrepareForExpeditionCore(vehicleId, distanceKm, null);
        }

        /// <summary>
        /// CORE-MECH W4 — the consuming preparation travel, with an optional rng for
        /// the breakdown roll. Null keeps the legacy path (the system's own stream)
        /// byte-for-byte; the outcome-aware caller passes a seed so the whole
        /// resolution (breakdown + crew band) is one deterministic draw sequence.
        /// </summary>
        private (float fuelCost, float travelTimeMod, bool breakdown) PrepareForExpeditionCore(
            string vehicleId, float distanceKm, ISeededRng? rng)
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
            if (v.trackGear != null && v.trackGear.IsInstalled)
                v.trackGear.condition = Math.Max(0f, v.trackGear.condition - distanceKm * 0.25f);

            bool breakdown = false;
            if (v.condition < 20f && (rng ?? _rng).NextDouble() < 0.3f)
            {
                breakdown = true;
                v.isBrokenDown = true;
                v.breakdownCause = $"vehicle broke down at {distanceKm}km (condition={v.condition:F1})";
            }

            OnVehicleStateChanged?.Invoke();
            return (fuelNeeded, v.speedMultiplier, breakdown);
        }

        /// <summary>
        /// CORE-MECH W4 — a prep breakdown now names what it costs the crew.
        /// Raised once per breakdown resolution; not raised for clean dispatches
        /// or when the vehicle is already broken (the repair gate is the
        /// exactly-once guard, AC.3).
        /// </summary>
        public event Action<VehicleBreakdownOutcome>? OnBreakdownResolved;

        /// <summary>
        /// CORE-MECH W4 — consume the same preparation travel as
        /// <see cref="PrepareForExpedition"/> and, when the vehicle breaks down,
        /// resolve a typed crew consequence (injury / exposure / contamination).
        /// Deterministic: the band is a pure function of the vehicle's condition
        /// and the injected rng. The existing tuple API is untouched; this is the
        /// consequence-aware call the host uses.
        /// </summary>
        public VehicleBreakdownOutcome ResolvePrepBreakdown(string vehicleId, float distanceKm, ISeededRng? rng = null)
        {
            if (!_state.ownedVehicles.TryGetValue(vehicleId, out var v))
                return VehicleBreakdownOutcome.None(vehicleId, "unknown_vehicle");

            if (v.isBrokenDown)
                return VehicleBreakdownOutcome.None(vehicleId, "already_broken");

            var (_, _, broke) = PrepareForExpeditionCore(vehicleId, distanceKm, rng);
            if (!broke)
                return VehicleBreakdownOutcome.None(vehicleId, "clean_dispatch");

            // Severity rises as the vehicle is closer to failure; the band is a
            // pure function so the same seed always yields the same outcome.
            float severity = Math.Clamp(1f - v.condition / 100f, 0.05f, 1f);
            double roll = (rng ?? _rng).NextDouble();

            VehicleBreakdownKind kind;
            if (roll < 0.55) kind = VehicleBreakdownKind.None;
            else if (roll < 0.80) kind = VehicleBreakdownKind.Injury;
            else if (roll < 0.92) kind = VehicleBreakdownKind.RadiationExposure;
            else kind = VehicleBreakdownKind.Contamination;

            // Nominal exposure scales with severity and stays inside the ledger's
            // own band discipline; magnitude is authored data pending W12 (BE.2).
            float nominalMsV = kind == VehicleBreakdownKind.RadiationExposure
                ? 0.15f + severity * 0.85f
                : 0f;

            var outcome = new VehicleBreakdownOutcome(
                kind, severity, nominalMsV,
                v.breakdownCause ?? string.Empty, vehicleId, brokeDown: true);

            OnBreakdownResolved?.Invoke(outcome);
            OnVehicleStateChanged?.Invoke();
            return outcome;
        }

        public ExpeditionVehicleState CaptureState() => CloneState(_state);

        public void RestoreState(ExpeditionVehicleState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static ExpeditionVehicleState CloneState(ExpeditionVehicleState src)
        {
            if (src == null) return new ExpeditionVehicleState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<ExpeditionVehicleState>(json) ?? new ExpeditionVehicleState();
        }
    }
}
