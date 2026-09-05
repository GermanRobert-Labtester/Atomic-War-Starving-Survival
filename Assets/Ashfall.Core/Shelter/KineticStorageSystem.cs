using System;
using System.Collections.Generic;
using System.IO;
#pragma warning disable CS8618

namespace Ashfall.Core.Shelter
{
    // ─── Catalog types ───

    [Serializable]
    public sealed class KineticFlywheelCatalog
    {
        public int schema_version = 1;
        public List<FlywheelClassDef> flywheel_classes = new List<FlywheelClassDef>();
        public List<SurgeEventDef> surge_events = new List<SurgeEventDef>();
        public BlackStartDef black_start = new BlackStartDef();
        public ContainmentHazardDef containment_hazard = new ContainmentHazardDef();
    }

    [Serializable]
    public sealed class FlywheelClassDef
    {
        public string flywheel_id = string.Empty;
        public string display_name = string.Empty;
        public float rotor_mass_kg;
        public float effective_radius_m;
        public float moment_of_inertia_factor = 0.5f;
        public float max_rpm;
        public float max_safe_rpm_ratio = 0.9f;
        public float min_vacuum_torr;
        public float operational_vacuum_torr;
        public float max_bearing_temp_c;
        public float safe_bearing_temp_c;
        public float containment_rating;
        public float motor_generator_efficiency;
        public float max_charge_kw;
        public float max_discharge_kw;
        public float idle_drag_loss_percent_per_hour;
        public float vacuum_leak_rate_per_day;
        public float bearing_heat_per_charge_kw;
        public float bearing_heat_per_discharge_kw;
        public float bearing_cooling_rate_per_tick;
        public List<string> construction_required_items = new List<string>();
        public int construction_labor_ticks;
        public int maintenance_interval_days;
        public List<string> maintenance_required_items = new List<string>();
        public List<string> tags = new List<string>();
    }

    [Serializable]
    public sealed class SurgeEventDef
    {
        public string surge_id = string.Empty;
        public string display_name = string.Empty;
        public float peak_kw;
        public int duration_ticks;
        public string event_class = string.Empty;
    }

    [Serializable]
    public sealed class BlackStartDef
    {
        public float min_stored_energy_kwh = 0.5f;
        public int required_duration_ticks = 3;
        public float generator_restart_probability = 0.95f;
    }

    [Serializable]
    public sealed class ContainmentHazardDef
    {
        public float catastrophic_energy_release_mj = 4f;
        public float room_damage_per_failure = 25f;
        public int survivor_exposure_time_ticks = 1;
        public int fragment_radius_rooms = 1;
    }

    // ─── State DTOs ───

    [Serializable]
    public sealed class KineticStorageState
    {
        public string systemId = KineticStorageSystem.SystemId;
        public List<FlywheelInstance> flywheels = new List<FlywheelInstance>();
    }

    [Serializable]
    public sealed class FlywheelInstance
    {
        public string instanceId = string.Empty;
        public string flywheelClassId = string.Empty;
        public string roomId = string.Empty;

        // Rotor state
        public float rotorRpm;
        public float storedEnergyJ;
        public float rotorHealth = 1f;

        // Vacuum state
        public float vacuumPressureTorr;
        public float vacuumHealth = 1f;

        // Bearing state
        public float bearingTemperatureC;
        public float bearingHealth = 1f;

        // Containment
        public float containmentHealth = 1f;

        // Operational
        public bool isInstalled;
        public bool isOnline;
        public bool emergencyBrakeEngaged;
        public float activeChargeKw;
        public float activeDischargeKw;
        public int daysSinceMaintenance;
        public int installedDay;
        public int lastMaintenanceDay;

        // Failure tracking
        public bool hasFailed;
        public string failureReason = string.Empty;
    }

    // ─── System ───

    /// <summary>
    /// ASHFALL Kinetic Flywheel Energy Storage System (Plan 80).
    /// Owns flywheel rotor state, vacuum, bearings, containment, and energy storage.
    /// Does not own global power balance, generators, breaker state, or room integrity.
    /// </summary>
    public sealed class KineticStorageSystem
    {
        public const string SystemId = "kinetic_storage";

        private KineticStorageState _state = new KineticStorageState();
        private readonly KineticFlywheelCatalog _catalog;
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private int _currentDay;

        public KineticStorageState State => _state;
        public IReadOnlyList<FlywheelInstance> Flywheels => _state.flywheels;

        public event Action<FlywheelInstance>? OnFlywheelInstalled;
        public event Action<FlywheelInstance>? OnFlywheelOverspeed;
        public event Action<FlywheelInstance>? OnFlywheelDischarged;
        public event Action<FlywheelInstance>? OnFlywheelFailure;
        public event Action? OnStorageChanged;

        public KineticStorageSystem(KineticFlywheelCatalog catalog, ISeededRng rng, ILog? log = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        public FlywheelClassDef? FindClass(string flywheelClassId)
        {
            if (string.IsNullOrEmpty(flywheelClassId)) return null;
            foreach (var c in _catalog.flywheel_classes)
                if (c.flywheel_id == flywheelClassId) return c;
            return null;
        }

        public FlywheelInstance? FindFlywheel(string instanceId)
        {
            if (string.IsNullOrEmpty(instanceId)) return null;
            foreach (var f in _state.flywheels)
                if (f.instanceId == instanceId) return f;
            return null;
        }

        /// <summary>
        /// Compute the moment of inertia for a flywheel.
        /// I = k * m * r^2
        /// </summary>
        public static float ComputeMomentOfInertia(float massKg, float radiusM, float factorK)
        {
            return factorK * massKg * radiusM * radiusM;
        }

        /// <summary>
        /// Compute stored kinetic energy in joules.
        /// E = 0.5 * I * omega^2 where omega = rpm * 2*pi/60
        /// </summary>
        public static float ComputeStoredEnergyJ(float momentOfInertia, float rpm)
        {
            float omega = rpm * (float)(2.0 * Math.PI / 60.0);
            return 0.5f * momentOfInertia * omega * omega;
        }

        /// <summary>
        /// Compute RPM from stored energy.
        /// </summary>
        public static float ComputeRpmFromEnergy(float momentOfInertia, float energyJ)
        {
            if (energyJ <= 0 || momentOfInertia <= 0) return 0;
            float omegaSq = 2f * energyJ / momentOfInertia;
            float omega = (float)Math.Sqrt(omegaSq);
            return omega * (float)(60.0 / (2.0 * Math.PI));
        }

        /// <summary>
        /// Convert joules to kWh.
        /// </summary>
        public static float JoulesToKwh(float joules) => joules / 3_600_000f;
        public static float KwhToJoules(float kwh) => kwh * 3_600_000f;

        /// <summary>
        /// Install a flywheel in a room.
        /// </summary>
        public ActionResult InstallFlywheel(string flywheelClassId, string roomId, int day, Func<string, int, bool> consumeItems)
        {
            var fc = FindClass(flywheelClassId);
            if (fc == null)
                return ActionResult.Blocked("unknown_class", "flywheel.unknown_class");

            foreach (var itemId in fc.construction_required_items)
            {
                if (!consumeItems(itemId, 1))
                    return ActionResult.Blocked("missing_items", $"flywheel.missing_{itemId}");
            }

            var instance = new FlywheelInstance
            {
                instanceId = $"flywheel_{flywheelClassId}_{roomId}",
                flywheelClassId = flywheelClassId,
                roomId = roomId,
                rotorRpm = 0,
                storedEnergyJ = 0,
                rotorHealth = 1f,
                vacuumPressureTorr = fc.operational_vacuum_torr,
                vacuumHealth = 1f,
                bearingTemperatureC = 20f,
                bearingHealth = 1f,
                containmentHealth = 1f,
                isInstalled = true,
                isOnline = false,
                installedDay = day,
                lastMaintenanceDay = day
            };

            _state.flywheels.Add(instance);
            _log.Info($"[Flywheel] {flywheelClassId} installed in {roomId}");
            OnFlywheelInstalled?.Invoke(instance);
            OnStorageChanged?.Invoke();
            return ActionResult.Success("flywheel.installed");
        }

        /// <summary>
        /// Bring a flywheel online (start vacuum pump, energize bearings).
        /// </summary>
        public ActionResult BringOnline(string instanceId)
        {
            var f = FindFlywheel(instanceId);
            if (f == null) return ActionResult.Blocked("not_found", "flywheel.not_found");
            if (f.hasFailed) return ActionResult.Blocked("failed", "flywheel.failed");
            f.isOnline = true;
            _log.Info($"[Flywheel] {instanceId} online");
            OnStorageChanged?.Invoke();
            return ActionResult.Success("flywheel.online");
        }

        /// <summary>
        /// Charge the flywheel with surplus power. Returns energy actually stored (J).
        /// </summary>
        public float Charge(string instanceId, float powerKw, float durationSeconds)
        {
            var f = FindFlywheel(instanceId);
            if (f == null || !f.isOnline || f.hasFailed || f.emergencyBrakeEngaged) return 0;

            var fc = FindClass(f.flywheelClassId);
            if (fc == null) return 0;

            // Check vacuum and thermal limits
            if (f.vacuumPressureTorr > fc.min_vacuum_torr)
                return 0; // Vacuum too poor to charge safely
            if (f.bearingTemperatureC >= fc.max_bearing_temp_c)
                return 0; // Bearings too hot

            float maxRpm = fc.max_rpm * fc.max_safe_rpm_ratio;
            float momentOfInertia = ComputeMomentOfInertia(fc.rotor_mass_kg, fc.effective_radius_m, fc.moment_of_inertia_factor);
            float maxEnergy = ComputeStoredEnergyJ(momentOfInertia, maxRpm);

            float chargePower = Math.Min(powerKw, fc.max_charge_kw);
            float energyIn = chargePower * 1000f * durationSeconds * fc.motor_generator_efficiency;
            float newEnergy = Math.Min(f.storedEnergyJ + energyIn, maxEnergy);
            float actualStored = newEnergy - f.storedEnergyJ;

            f.storedEnergyJ = newEnergy;
            f.rotorRpm = ComputeRpmFromEnergy(momentOfInertia, newEnergy);

            // Bearing heating
            f.bearingTemperatureC += fc.bearing_heat_per_charge_kw * chargePower * durationSeconds / 3600f;

            OnStorageChanged?.Invoke();
            return actualStored;
        }

        /// <summary>
        /// Discharge the flywheel to meet a power demand. Returns energy delivered (J).
        /// </summary>
        public float Discharge(string instanceId, float powerKw, float durationSeconds)
        {
            var f = FindFlywheel(instanceId);
            if (f == null || !f.isOnline || f.hasFailed) return 0;
            if (f.storedEnergyJ <= 0) return 0;

            var fc = FindClass(f.flywheelClassId);
            if (fc == null) return 0;

            float dischargePower = Math.Min(powerKw, fc.max_discharge_kw);
            float energyOut = dischargePower * 1000f * durationSeconds / fc.motor_generator_efficiency;
            energyOut = Math.Min(energyOut, f.storedEnergyJ);

            f.storedEnergyJ -= energyOut;
            float momentOfInertia = ComputeMomentOfInertia(fc.rotor_mass_kg, fc.effective_radius_m, fc.moment_of_inertia_factor);
            f.rotorRpm = ComputeRpmFromEnergy(momentOfInertia, f.storedEnergyJ);

            // Bearing heating
            f.bearingTemperatureC += fc.bearing_heat_per_discharge_kw * dischargePower * durationSeconds / 3600f;

            if (f.storedEnergyJ <= 0)
            {
                f.rotorRpm = 0;
                OnFlywheelDischarged?.Invoke(f);
            }

            OnStorageChanged?.Invoke();
            return energyOut;
        }

        /// <summary>
        /// Handle a surge event. Returns the power (kW) the flywheel can supply.
        /// </summary>
        public float HandleSurge(string instanceId, string surgeId)
        {
            var f = FindFlywheel(instanceId);
            if (f == null || !f.isOnline || f.hasFailed || f.storedEnergyJ <= 0) return 0;

            var fc = FindClass(f.flywheelClassId);
            if (fc == null) return 0;

            SurgeEventDef? surge = null;
            foreach (var s in _catalog.surge_events)
                if (s.surge_id == surgeId) { surge = s; break; }
            if (surge == null) return 0;

            float deliverable = Math.Min(surge.peak_kw, fc.max_discharge_kw);
            float energyJ = deliverable * 1000f * surge.duration_ticks / fc.motor_generator_efficiency;

            if (energyJ > f.storedEnergyJ)
                deliverable = f.storedEnergyJ * fc.motor_generator_efficiency / (1000f * surge.duration_ticks);

            Discharge(instanceId, deliverable, surge.duration_ticks);
            return deliverable;
        }

        /// <summary>
        /// Attempt a black start of the generator.
        /// </summary>
        public bool TryBlackStart(string instanceId)
        {
            var f = FindFlywheel(instanceId);
            if (f == null || !f.isOnline || f.hasFailed) return false;

            float minEnergyJ = KwhToJoules(_catalog.black_start.min_stored_energy_kwh);
            if (f.storedEnergyJ < minEnergyJ) return false;

            // Consume the startup energy
            float energyRequired = KwhToJoules(_catalog.black_start.min_stored_energy_kwh);
            f.storedEnergyJ = Math.Max(0, f.storedEnergyJ - energyRequired);
            var fc = FindClass(f.flywheelClassId);
            if (fc != null)
            {
                float momentOfInertia = ComputeMomentOfInertia(fc.rotor_mass_kg, fc.effective_radius_m, fc.moment_of_inertia_factor);
                f.rotorRpm = ComputeRpmFromEnergy(momentOfInertia, f.storedEnergyJ);
            }

            // Deterministic success check
            bool success = _rng.NextDouble() < _catalog.black_start.generator_restart_probability;
            _log.Info($"[Flywheel] black start attempt: {(success ? "success" : "failure")}");
            OnStorageChanged?.Invoke();
            return success;
        }

        /// <summary>
        /// Tick the flywheel simulation (called once per tick).
        /// </summary>
        public void Tick(float deltaSeconds)
        {
            foreach (var f in _state.flywheels)
            {
                if (!f.isInstalled || f.hasFailed) continue;

                var fc = FindClass(f.flywheelClassId);
                if (fc == null) continue;

                float momentOfInertia = ComputeMomentOfInertia(fc.rotor_mass_kg, fc.effective_radius_m, fc.moment_of_inertia_factor);

                // Idle drag loss
                if (f.storedEnergyJ > 0 && f.activeChargeKw <= 0)
                {
                    float dragLoss = f.storedEnergyJ * (fc.idle_drag_loss_percent_per_hour / 100f) * deltaSeconds / 3600f;
                    f.storedEnergyJ = Math.Max(0, f.storedEnergyJ - dragLoss);
                    f.rotorRpm = ComputeRpmFromEnergy(momentOfInertia, f.storedEnergyJ);
                }

                // Vacuum degradation
                f.vacuumPressureTorr = Math.Min(fc.min_vacuum_torr * 10f,
                    f.vacuumPressureTorr + fc.vacuum_leak_rate_per_day * deltaSeconds / 86400f);

                // Bearing cooling (when idle)
                if (f.activeChargeKw <= 0 && f.activeDischargeKw <= 0)
                {
                    f.bearingTemperatureC = Math.Max(20f,
                        f.bearingTemperatureC - fc.bearing_cooling_rate_per_tick * deltaSeconds);
                }

                // Overspeed check
                float maxSafeRpm = fc.max_rpm * fc.max_safe_rpm_ratio;
                if (f.rotorRpm > maxSafeRpm)
                {
                    OnFlywheelOverspeed?.Invoke(f);
                    // Containment check
                    float overSpeedRatio = f.rotorRpm / fc.max_rpm;
                    if (overSpeedRatio > 1.0f)
                    {
                        float failureChance = (overSpeedRatio - 1.0f) * (1f - fc.containment_rating);
                        if (_rng.NextDouble() < failureChance)
                        {
                            TriggerCatastrophicFailure(f, fc);
                        }
                    }
                }

                // Bearing touchdown risk
                if (f.bearingTemperatureC >= fc.max_bearing_temp_c)
                {
                    f.emergencyBrakeEngaged = true;
                    f.isOnline = false;
                    f.rotorHealth -= 0.1f * deltaSeconds;
                    _log.Warn($"[Flywheel] {f.instanceId}: bearing overheat — emergency brake engaged");
                }

                // Reset active power for next tick
                f.activeChargeKw = 0;
                f.activeDischargeKw = 0;
            }
        }

        private void TriggerCatastrophicFailure(FlywheelInstance f, FlywheelClassDef fc)
        {
            f.hasFailed = true;
            f.isOnline = false;
            f.failureReason = "catastrophic_rotor_failure";
            f.storedEnergyJ = 0;
            f.rotorRpm = 0;
            f.rotorHealth = 0;
            f.containmentHealth = 0;

            _log.Error($"[Flywheel] CATASTROPHIC FAILURE: {f.instanceId} in room {f.roomId}");
            OnFlywheelFailure?.Invoke(f);
        }

        public void TickDay(int day)
        {
            _currentDay = day;
            foreach (var f in _state.flywheels)
            {
                if (!f.isInstalled) continue;
                f.daysSinceMaintenance = day - f.lastMaintenanceDay;
            }
        }

        /// <summary>
        /// Perform maintenance on a flywheel.
        /// </summary>
        public ActionResult PerformMaintenance(string instanceId, int day, Func<string, int, bool> consumeItems)
        {
            var f = FindFlywheel(instanceId);
            if (f == null) return ActionResult.Blocked("not_found", "flywheel.not_found");
            if (f.hasFailed) return ActionResult.Blocked("failed", "flywheel.failed");

            var fc = FindClass(f.flywheelClassId);
            if (fc == null) return ActionResult.Blocked("unknown_class", "flywheel.unknown_class");

            foreach (var itemId in fc.maintenance_required_items)
            {
                if (!consumeItems(itemId, 1))
                    return ActionResult.Blocked("missing_items", $"flywheel.missing_{itemId}");
            }

            f.bearingHealth = Math.Min(1f, f.bearingHealth + 0.2f);
            f.vacuumHealth = Math.Min(1f, f.vacuumHealth + 0.15f);
            f.rotorHealth = Math.Min(1f, f.rotorHealth + 0.1f);
            f.lastMaintenanceDay = day;
            f.daysSinceMaintenance = 0;
            f.emergencyBrakeEngaged = false;
            f.vacuumPressureTorr = fc.operational_vacuum_torr;

            _log.Info($"[Flywheel] {instanceId}: maintenance complete");
            OnStorageChanged?.Invoke();
            return ActionResult.Success("flywheel.maintained");
        }

        /// <summary>
        /// Get the total stored energy across all installed flywheels in kWh.
        /// </summary>
        public float TotalStoredEnergyKwh()
        {
            float total = 0;
            foreach (var f in _state.flywheels)
                if (f.isInstalled && !f.hasFailed)
                    total += JoulesToKwh(f.storedEnergyJ);
            return total;
        }

        /// <summary>
        /// Get the total available discharge capacity in kW.
        /// </summary>
        public float TotalDischargeCapacityKw()
        {
            float total = 0;
            foreach (var f in _state.flywheels)
            {
                if (!f.isInstalled || f.hasFailed || !f.isOnline || f.storedEnergyJ <= 0) continue;
                var fc = FindClass(f.flywheelClassId);
                if (fc != null) total += fc.max_discharge_kw;
            }
            return total;
        }

        public KineticStorageState CaptureState() => CloneState(_state);

        public void RestoreState(KineticStorageState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static KineticStorageState CloneState(KineticStorageState src)
        {
            if (src == null) return new KineticStorageState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<KineticStorageState>(json) ?? new KineticStorageState();
        }

        public KineticFlywheelCatalog Catalog => _catalog;
    }

    /// <summary>
    /// Loads <c>kinetic_flywheel_catalog.json</c>.
    /// </summary>
    public static class KineticFlywheelCatalogLoader
    {
        public static KineticFlywheelCatalog Load(string dataDir, IFileIO files, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(dataDir)) throw new ArgumentException("dataDir required", nameof(dataDir));
            if (files == null) throw new ArgumentNullException(nameof(files));
            if (json == null) throw new ArgumentNullException(nameof(json));

            string path = files.Combine(dataDir, "kinetic_flywheel_catalog.json");
            if (!files.FileExists(path))
                return new KineticFlywheelCatalog();

            string raw = files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return new KineticFlywheelCatalog();

            var catalog = json.Deserialize<KineticFlywheelCatalog>(raw);
            if (catalog == null)
                throw new InvalidOperationException("Failed to deserialize kinetic_flywheel_catalog.json");

            // Validate
            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var fc in catalog.flywheel_classes)
            {
                if (string.IsNullOrEmpty(fc.flywheel_id))
                    throw new InvalidOperationException("Flywheel catalog: flywheel_id is required");
                if (!seenIds.Add(fc.flywheel_id))
                    throw new InvalidOperationException($"Flywheel catalog: duplicate flywheel_id '{fc.flywheel_id}'");
                if (fc.rotor_mass_kg <= 0 || fc.effective_radius_m <= 0 || fc.max_rpm <= 0)
                    throw new InvalidOperationException($"Flywheel catalog: invalid physical params for '{fc.flywheel_id}'");
                if (fc.motor_generator_efficiency <= 0 || fc.motor_generator_efficiency > 1)
                    throw new InvalidOperationException($"Flywheel catalog: invalid efficiency for '{fc.flywheel_id}'");
            }

            return catalog;
        }
    }
}