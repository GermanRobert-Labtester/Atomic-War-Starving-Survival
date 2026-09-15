// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ashfall.Core.Expeditions
{
    /// <summary>Authored run-flat wheel profile (Plan 141 Phase 1). Abstract mobility data.</summary>
    public sealed class RunFlatProfileDef
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public List<string> compatible_vehicle_tags { get; set; } = new List<string>();
        public int puncture_resistance_bp { get; set; }
        public int sidewall_damage_resistance_bp { get; set; }
        public int rolling_resistance_modifier_bp { get; set; }
        public int unsprung_mass_modifier_bp { get; set; }
        public int heat_generation_rate_bp { get; set; }
        public int safe_speed_profile_kph { get; set; } = 70;
        public int terrain_modifier_bp { get; set; }
        public int repairability_bp { get; set; }
        public List<string> required_item_ids { get; set; } = new List<string>();
        public List<string> tags { get; set; } = new List<string>();
    }

    public sealed class RunFlatTireCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<RunFlatProfileDef> runflat_profiles { get; set; } = new List<RunFlatProfileDef>();

        private readonly Dictionary<string, RunFlatProfileDef> _byId = new Dictionary<string, RunFlatProfileDef>(StringComparer.Ordinal);

        public void Index()
        {
            _byId.Clear();
            foreach (var p in runflat_profiles)
                if (p != null && !string.IsNullOrEmpty(p.id)) _byId[p.id] = p;
        }

        public RunFlatProfileDef? Get(string id) { _byId.TryGetValue(id ?? string.Empty, out var d); return d; }
        public IReadOnlyCollection<RunFlatProfileDef> All => _byId.Values;
    }

    public static class RunFlatTireCatalogLoader
    {
        public const string DefaultFileName = "runflat_tire_catalog.json";

        public static RunFlatTireCatalog Load(string dataDir, IFileIO fileIo)
        {
            if (fileIo == null || string.IsNullOrEmpty(dataDir)) return Empty();
            string path = Path.Combine(dataDir, DefaultFileName);
            if (!fileIo.FileExists(path)) return Empty();
            string json = fileIo.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json)) return Empty();
            var catalog = JsonSerializer.Deserialize<RunFlatTireCatalog>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? Empty();
            catalog.Index();
            return catalog;
        }

        private static RunFlatTireCatalog Empty() { var c = new RunFlatTireCatalog(); c.Index(); return c; }
    }

    [Serializable]
    public sealed class WheelSetState
    {
        public string VehicleId { get; set; } = string.Empty;
        public string ProfileId { get; set; } = string.Empty;
        public int IntegrityBp { get; set; } = 100;
        public int PunctureCount { get; set; }
        public int HeatC { get; set; } = 20;
        public int ImbalanceBp { get; set; }
        public int RimBeadBp { get; set; } = 100;
        public int WearBp { get; set; }
        public int LastHazardDay { get; set; }
    }

    [Serializable]
    public sealed class RunFlatTireState
    {
        public string SystemId { get; set; } = RunFlatTireEngine.SystemId;
        public int SchemaVersion { get; set; } = 1;
        public List<WheelSetState> WheelSets { get; set; } = new List<WheelSetState>();
        public int TotalInstalls { get; set; }
        public int TotalHazards { get; set; }
    }

    /// <summary>
    /// Plan 141 Phase 1 — run-flat wheel component. A persistent upgrade that
    /// trades puncture resilience for mass, rolling resistance, heat, and
    /// maintenance burden. Severity is reduced, never eliminated; severe
    /// hazards and overheating can still disable or damage a wheel.
    /// </summary>
    public sealed class RunFlatTireEngine
    {
        public const string SystemId = "runflat_tire";

        public const int OverheatThresholdC = 120;
        public const int MaxHeatC = 200;
        public const int AmbientC = 20;
        public const int MinSafeSpeedKph = 10;

        private RunFlatTireState _state = new RunFlatTireState();
        private readonly RunFlatTireCatalog _catalog;
        private readonly ILog _log;

        public ISeededRng? Rng { get; set; }

        public RunFlatTireState State => _state;
        public RunFlatTireCatalog Catalog => _catalog;

        public RunFlatTireEngine(RunFlatTireCatalog catalog, ILog? log = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _log = log ?? NullLog.Instance;
        }

        public WheelSetState? FindWheelSet(string vehicleId)
            => _state.WheelSets.Find(w => string.Equals(w.VehicleId, vehicleId, StringComparison.Ordinal));

        /// <summary>
        /// Installs a run-flat profile. Requires a compatible vehicle tag, the
        /// authored install kit, and a workshop. Skill improves balance/integrity.
        /// </summary>
        public ActionResult Install(string vehicleId, string vehicleTag, string profileId, bool workshopAvailable, bool partsAvailable, double skill)
        {
            var profile = _catalog.Get(profileId);
            if (profile == null)
                return ActionResult.Blocked("upgrade_parts_missing", "runflat.upgrade_parts_missing");
            if (string.IsNullOrEmpty(vehicleId))
                return ActionResult.Blocked("vehicle_incompatible", "runflat.vehicle_incompatible");
            if (!IsCompatible(profile, vehicleTag))
                return ActionResult.Blocked("vehicle_incompatible", "runflat.vehicle_incompatible");
            if (!partsAvailable)
                return ActionResult.Blocked("upgrade_parts_missing", "runflat.upgrade_parts_missing");
            if (!workshopAvailable)
                return ActionResult.Blocked("workshop_unavailable", "runflat.workshop_unavailable");

            int integrity = Math.Max(70, Math.Min(100, 80 + (int)Math.Round(Clamp01(skill) * 20)));
            int imbalance = Math.Max(5, 25 - (int)Math.Round(Clamp01(skill) * 20));

            var existing = FindWheelSet(vehicleId);
            if (existing != null)
            {
                existing.ProfileId = profileId;
                existing.IntegrityBp = integrity;
                existing.ImbalanceBp = imbalance;
                existing.RimBeadBp = 100;
                existing.HeatC = AmbientC;
            }
            else
            {
                _state.WheelSets.Add(new WheelSetState
                {
                    VehicleId = vehicleId,
                    ProfileId = profileId,
                    IntegrityBp = integrity,
                    ImbalanceBp = imbalance
                });
            }

            _state.TotalInstalls++;
            return ActionResult.Success("runflat.installed");
        }

        private static bool IsCompatible(RunFlatProfileDef profile, string vehicleTag)
        {
            if (string.IsNullOrEmpty(vehicleTag)) return false;
            foreach (var tag in profile.compatible_vehicle_tags)
                if (string.Equals(tag, vehicleTag, StringComparison.Ordinal)) return true;
            return false;
        }

        /// <summary>
        /// Applies a road-hazard event. Run-flat resistance reduces severity but
        /// never to zero; severe classes can still damage the rim/bead and even
        /// destroy an already-weak wheel.
        /// </summary>
        public ActionResult ApplyHazard(string vehicleId, string hazardClass, int speedKph, int ambientTempC)
        {
            var wheel = FindWheelSet(vehicleId);
            if (wheel == null)
                return ActionResult.Blocked("maintenance_required", "runflat.no_runflat_fitted");
            var profile = _catalog.Get(wheel.ProfileId);
            if (profile == null)
                return ActionResult.Blocked("maintenance_required", "runflat.unknown_profile");

            int baseLoss = hazardClass switch
            {
                "glass" => 4,
                "wire" => 6,
                "scrap" => 9,
                "rubble" => 12,
                "spike" => 22,
                "severe" => 35,
                _ => 8
            };

            bool severe = hazardClass == "spike" || hazardClass == "severe";
            int resistance = severe ? profile.sidewall_damage_resistance_bp : profile.puncture_resistance_bp;
            int loss = (int)Math.Round(baseLoss * (10000 - resistance) / 10000.0);
            // No immunity: even the best profile loses at least 1 point to a hazard.
            loss = Math.Max(1, loss);

            wheel.IntegrityBp = Math.Max(0, wheel.IntegrityBp - loss);
            wheel.PunctureCount++;
            wheel.LastHazardDay++;
            _state.TotalHazards++;

            // Hazard friction heats the carcass.
            wheel.HeatC = Math.Min(MaxHeatC, wheel.HeatC + Math.Max(1, baseLoss / 2) + speedKph / 20);

            // Severe impacts can bend the rim/bead even when the carcass holds.
            if (severe)
            {
                bool rimHit = Rng != null
                    ? Rng.Next(0, 10000) < Math.Max(500, 4000 - profile.sidewall_damage_resistance_bp / 4)
                    : StableRoll(wheel.VehicleId, Math.Max(500, 4000 - profile.sidewall_damage_resistance_bp / 4));
                if (rimHit)
                {
                    wheel.RimBeadBp = Math.Max(0, wheel.RimBeadBp - 15);
                    wheel.ImbalanceBp = Math.Min(100, wheel.ImbalanceBp + 10);
                }
            }

            _log.Info($"[RunFlat] {vehicleId} hazard {hazardClass}: -{loss} integrity, heat {wheel.HeatC}C");

            if (wheel.IntegrityBp <= 0)
            {
                return ActionResult.Blocked("wheel_damaged", "runflat.wheel_destroyed");
            }
            if (wheel.HeatC >= OverheatThresholdC)
            {
                return ActionResult.Blocked("wheel_overheated", "runflat.wheel_overheated");
            }
            return ActionResult.Success("runflat.hazard_applied");
        }

        /// <summary>
        /// Ticks heat for a leg of travel. Heat rises with speed, load, ambient,
        /// and the profile's heat generation; it falls when stopped.
        /// </summary>
        public void TickHeat(string vehicleId, int speedKph, int loadBp, int ambientTempC)
        {
            var wheel = FindWheelSet(vehicleId);
            if (wheel == null) return;
            var profile = _catalog.Get(wheel.ProfileId);
            if (profile == null) return;

            double speedFactor = Math.Max(0, speedKph) / 100.0;
            double loadFactor = 1.0 + Math.Max(0, loadBp) / 200.0;
            double ambientFactor = Math.Max(0, ambientTempC - AmbientC) * 0.5;

            double gain = profile.heat_generation_rate_bp / 100.0 * speedFactor * loadFactor + ambientFactor;
            double cooling = speedKph < 5 ? 8.0 : 2.0;

            int heat = (int)Math.Round(wheel.HeatC + gain - cooling);
            wheel.HeatC = Math.Max(0, Math.Min(MaxHeatC, heat));

            if (wheel.HeatC >= OverheatThresholdC)
            {
                wheel.WearBp = Math.Min(100, wheel.WearBp + 2);
                wheel.IntegrityBp = Math.Max(0, wheel.IntegrityBp - 1);
            }
        }

        /// <summary>Recommended safe speed band; heat, imbalance, wear and damage reduce it.</summary>
        public int GetSafeSpeedKph(string vehicleId)
        {
            var wheel = FindWheelSet(vehicleId);
            if (wheel == null) return 0;
            var profile = _catalog.Get(wheel.ProfileId);
            if (profile == null) return 0;

            int speed = profile.safe_speed_profile_kph;
            if (wheel.HeatC > OverheatThresholdC - 20) speed -= 15;
            speed -= wheel.WearBp / 10;
            speed -= wheel.ImbalanceBp / 5;
            speed -= (100 - wheel.IntegrityBp) / 8;
            speed -= (100 - wheel.RimBeadBp) / 10;
            return Math.Max(MinSafeSpeedKph, speed);
        }

        /// <summary>Rolling-resistance fuel penalty, percent. Real cost, never free resilience.</summary>
        public int GetFuelPenaltyPct(string vehicleId)
        {
            var wheel = FindWheelSet(vehicleId);
            var profile = wheel == null ? null : _catalog.Get(wheel.ProfileId);
            return profile == null ? 0 : profile.rolling_resistance_modifier_bp / 100;
        }

        public int GetRollingResistanceModifierBp(string vehicleId)
        {
            var wheel = FindWheelSet(vehicleId);
            var profile = wheel == null ? null : _catalog.Get(wheel.ProfileId);
            return profile?.rolling_resistance_modifier_bp ?? 0;
        }

        /// <summary>Field maintenance. Some profiles are hard to restore; never a full factory reset.</summary>
        public ActionResult Repair(string vehicleId, bool workshopAvailable, double skill)
        {
            var wheel = FindWheelSet(vehicleId);
            if (wheel == null)
                return ActionResult.Blocked("maintenance_required", "runflat.no_runflat_fitted");
            var profile = _catalog.Get(wheel.ProfileId);
            if (profile == null)
                return ActionResult.Blocked("maintenance_required", "runflat.unknown_profile");
            if (!workshopAvailable)
                return ActionResult.Blocked("workshop_unavailable", "runflat.workshop_unavailable");

            int restored = (int)Math.Round(profile.repairability_bp / 100.0 * (0.5 + Clamp01(skill) * 0.5));
            wheel.IntegrityBp = Math.Min(100, wheel.IntegrityBp + restored);
            wheel.RimBeadBp = Math.Min(100, wheel.RimBeadBp + restored / 2);
            wheel.WearBp = Math.Max(0, wheel.WearBp - 10);
            wheel.HeatC = Math.Max(AmbientC, wheel.HeatC - 30);
            return ActionResult.Success("runflat.repaired");
        }

        private static bool StableRoll(string vehicleId, int riskBp)
        {
            if (riskBp <= 0) return false;
            uint h = 2166136261u;
            foreach (char c in vehicleId) { h ^= c; h *= 16777619u; }
            return (h % 10000u) < (uint)riskBp;
        }

        private static double Clamp01(double v) => Math.Max(0.0, Math.Min(1.0, v));

        public RunFlatTireState CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            return s.Deserialize<RunFlatTireState>(s.Serialize(_state)) ?? new RunFlatTireState();
        }

        public void RestoreState(RunFlatTireState? saved)
        {
            if (saved == null) return;
            var s = new SystemTextJsonSerializer();
            _state = s.Deserialize<RunFlatTireState>(s.Serialize(saved)) ?? new RunFlatTireState();
            if (string.IsNullOrEmpty(_state.SystemId)) _state.SystemId = SystemId;
        }
    }
}
