using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Catalogs;
using Ashfall.Core.Diplomacy;
using Ashfall.Core.Institutions;
using Ashfall.Core.Inventory;
using Ashfall.Core.SkyDefense;

namespace Ashfall.Core.SkyDefense
{
    // ---------------------------------------------------------------------
    // Persisted state
    // ---------------------------------------------------------------------

    /// <summary>
    /// One counter-battery emplacement. Angles are authored game-abstraction
    /// integers normalized to [0,360) / [0,90]; no real-world ballistics.
    /// </summary>
    [Serializable]
    public sealed class CounterBatteryTurretState
    {
        public string turret_id = string.Empty;
        public int azimuth;                                // 0..359
        public int elevation;                              // 0..90
        public int barrel_heat;                            // 0..100
        public string loaded_ammo_id = string.Empty;
        public int magazine_count;                         // rounds in the loaded magazine
        public int radar_calibration = 70;                 // 0..100
        public int hydraulic_condition = 100;              // 0..100
        public int volleys_since_service;
        public bool is_operational = true;
        public List<string> assigned_crew_ids = new();
    }

    /// <summary>An active engagement opportunity derived from an orbital warning.</summary>
    [Serializable]
    public sealed class OrbitalTrackState
    {
        public string track_id = string.Empty;             // telemetry event id (dedup key)
        public int warning_day = -1;
        public int impact_day = -1;
        public int target_grid_x;
        public float energy_mj;
        public string severity = "Moderate";
        public int volleys_fired;
        public bool resolved;
    }

    [Serializable]
    public sealed class SkyDefenseBatterySave
    {
        public int schema_version = 1;
        public List<CounterBatteryTurretState> turrets = new();
        public List<OrbitalTrackState> tracks = new();
        public int total_interceptions;
        public int total_volleys;
    }

    // ---------------------------------------------------------------------
    // System
    // ---------------------------------------------------------------------

    /// <summary>
    /// Kinetic sky-layer counter-battery (flagship Task 7). Consumes
    /// authoritative <see cref="OrbitalHarrowTelemetrySystem"/> warnings,
    /// resolves authored game-scale interception probability, and reduces the
    /// pending strike's energy through
    /// <see cref="OrbitalHarrowTelemetrySystem.ApplyInterceptionMitigation"/> —
    /// residual severity always flows through the existing
    /// SkyLayerArmorSystem pipeline, never around it.
    ///
    /// Ammo authority: loaded-magazine model. Loading atomically transfers
    /// rounds from global inventory into the turret magazine; volleys consume
    /// magazine rounds. The same round is never counted in both places.
    ///
    /// Determinism: keyed RNG stream per (masterSeed, track, volley); no
    /// persisted RNG continuation.
    /// </summary>
    public sealed class SkyDefenseBatterySystem
    {
        public const string SystemId = "sky_defense_battery";
        public const string InstitutionId = "institution_sky_defense";

        public const int BaseInterceptChance = 45;
        public const int MinInterceptChance = 15;
        public const int MaxInterceptChance = 85;
        public const int HeatSeizureThreshold = 90;
        public const int DailyHeatDissipation = 30;
        public const int DailyRadarDrift = 2;
        public const int VolleysPerService = 10;
        public const string ServiceOilItemId = "machine_oil";
        public const string DefaultTurretId = "turret_main_battery";

        private readonly Inventory.Inventory? _inventory;
        private readonly OrbitalHarrowTelemetrySystem? _telemetry;
        private readonly ILog _log;
        private readonly int _masterSeed;
        private readonly IInstitutionAvailability? _availability;
        private readonly ISurvivorSkillsPort? _skills;

        private readonly Dictionary<string, SkyDefenseOrdnanceDefinition> _ordnance = new(StringComparer.Ordinal);
        private SkyDefenseBatterySave _state = new();

        public SkyDefenseBatterySystem(
            int masterSeed,
            Inventory.Inventory? inventory = null,
            OrbitalHarrowTelemetrySystem? telemetry = null,
            ILog? log = null,
            IInstitutionAvailability? availability = null,
            ISurvivorSkillsPort? skills = null)
        {
            _masterSeed = masterSeed;
            _inventory = inventory;
            _telemetry = telemetry;
            _log = log ?? new ConsoleLog();
            _availability = availability;
            _skills = skills;

            if (_telemetry != null)
                _telemetry.OnImpactWarning += HandleImpactWarning;
        }

        // -----------------------------------------------------------------
        // Events
        // -----------------------------------------------------------------

        public event Action<OrbitalTrackState>? OnOrbitalTrackAcquired;
        public event Action<string, string, int>? OnVolleyFired;             // turret, ammo, magazine left
        public event Action<string, string, bool, float>? OnInterceptResolved; // track, ammo, success, residual fraction
        public event Action<string>? OnMaintenanceDue;                       // turret
        public event Action<string>? OnServiced;                             // turret

        // -----------------------------------------------------------------
        // Catalog + setup
        // -----------------------------------------------------------------

        public void LoadOrdnanceCatalog(List<SkyDefenseOrdnanceDefinition> ordnance)
        {
            if (ordnance == null) return;
            _ordnance.Clear();
            foreach (var o in ordnance)
                if (!string.IsNullOrEmpty(o.ordnance_id))
                    _ordnance[o.ordnance_id] = o;
        }

        /// <summary>Ensures the authored default emplacement exists.</summary>
        public CounterBatteryTurretState EnsureDefaultTurret()
        {
            var turret = _state.turrets.FirstOrDefault(t => t.turret_id == DefaultTurretId);
            if (turret == null)
            {
                turret = new CounterBatteryTurretState { turret_id = DefaultTurretId };
                _state.turrets.Add(turret);
            }
            return turret;
        }

        public IReadOnlyList<CounterBatteryTurretState> Turrets => _state.turrets.AsReadOnly();
        public IReadOnlyList<OrbitalTrackState> Tracks => _state.tracks.AsReadOnly();
        public int TotalInterceptions => _state.total_interceptions;
        public SkyDefenseOrdnanceDefinition? GetOrdnance(string ordnanceId) =>
            _ordnance.GetValueOrDefault(ordnanceId);
        public CounterBatteryTurretState? GetTurret(string turretId) =>
            _state.turrets.FirstOrDefault(t => t.turret_id == turretId);
        public OrbitalTrackState? GetTrack(string trackId) =>
            _state.tracks.FirstOrDefault(t => t.track_id == trackId);

        // -----------------------------------------------------------------
        // Telemetry intake (the UI is never the relay — plan §9.7)
        // -----------------------------------------------------------------

        private void HandleImpactWarning(OrbitalWarningEntry warning)
        {
            if (warning == null || string.IsNullOrEmpty(warning.eventId)) return;
            if (_state.tracks.Any(t => t.track_id == warning.eventId))
                return; // repeated same warning does not duplicate a track

            var track = new OrbitalTrackState
            {
                track_id = warning.eventId,
                warning_day = warning.day - Math.Max(1, _telemetry?.State.warningLeadDays ?? 3),
                impact_day = warning.day,
                target_grid_x = warning.targetGridX,
                energy_mj = warning.energyMj,
                severity = string.IsNullOrEmpty(warning.severity) ? "Moderate" : warning.severity,
            };
            _state.tracks.Add(track);
            _log.Info($"[SkyDefense] track acquired '{track.track_id}' → impact day {track.impact_day}");
            OnOrbitalTrackAcquired?.Invoke(track);
        }

        // -----------------------------------------------------------------
        // Magazine logistics (atomic inventory ↔ turret transfer)
        // -----------------------------------------------------------------

        public ActionResult TryLoadMagazine(string turretId, string ordnanceId)
        {
            var turret = GetTurret(turretId);
            if (turret == null)
                return ActionResult.Blocked("unknown_turret", "sky.unknown_turret");
            if (!_ordnance.TryGetValue(ordnanceId, out var ordnance))
                return ActionResult.Blocked("unknown_ordnance", "sky.unknown_ordnance");
            if (ordnance.item_id == turret.loaded_ammo_id && turret.magazine_count >= ordnance.magazine_units)
                return ActionResult.Blocked("magazine_full", "sky.magazine_full");
            if (_inventory == null)
                return ActionResult.Blocked("no_inventory", "sky.no_inventory");
            if (turret.barrel_heat >= HeatSeizureThreshold)
                return ActionResult.Blocked("barrel_hot", "sky.barrel_hot");

            int space = ordnance.magazine_units - (turret.loaded_ammo_id == ordnance.item_id ? turret.magazine_count : 0);
            if (space <= 0)
                return ActionResult.Blocked("magazine_full", "sky.magazine_full");

            // Atomic transfer: unload the old type back to inventory, load the new.
            var bill = new InventoryBill();
            if (!string.IsNullOrEmpty(turret.loaded_ammo_id) && turret.magazine_count > 0)
                bill.AddGrant(turret.loaded_ammo_id, turret.magazine_count);
            bill.AddCost(ordnance.item_id, space);
            if (!_inventory.TryExecuteTransaction(bill))
                return ActionResult.Blocked("insufficient_ammo", "sky.insufficient_ammo");

            turret.loaded_ammo_id = ordnance.item_id;
            turret.magazine_count = ordnance.magazine_units;
            _log.Info($"[SkyDefense] '{turretId}' loaded {turret.magazine_count}x {ordnanceId}");
            return ActionResult.Success("sky.magazine_loaded",
                new Dictionary<string, double> { { "rounds", turret.magazine_count } });
        }

        // -----------------------------------------------------------------
        // Crew
        // -----------------------------------------------------------------

        public ActionResult TryAssignCrew(string turretId, string survivorId)
        {
            var turret = GetTurret(turretId);
            if (turret == null)
                return ActionResult.Blocked("unknown_turret", "sky.unknown_turret");
            if (turret.assigned_crew_ids.Contains(survivorId))
                return ActionResult.Success("sky.crew_already_assigned");
            if (_availability != null && !_availability.TryClaim(survivorId, InstitutionId, "gunner"))
                return ActionResult.Blocked("survivor_unavailable", "sky.survivor_unavailable");
            turret.assigned_crew_ids.Add(survivorId);
            return ActionResult.Success("sky.crew_assigned");
        }

        public ActionResult TryRemoveCrew(string turretId, string survivorId)
        {
            var turret = GetTurret(turretId);
            if (turret == null || !turret.assigned_crew_ids.Contains(survivorId))
                return ActionResult.Blocked("crew_not_assigned", "sky.crew_not_assigned");
            turret.assigned_crew_ids.Remove(survivorId);
            _availability?.Release(survivorId, InstitutionId, "gunner");
            return ActionResult.Success("sky.crew_removed");
        }

        // -----------------------------------------------------------------
        // Firing
        // -----------------------------------------------------------------

        /// <summary>Authored intercept chance preview for UI (clamped, deterministic).</summary>
        public int PreviewInterceptChance(CounterBatteryTurretState turret, OrbitalTrackState track, SkyDefenseOrdnanceDefinition ordnance) =>
            ComputeInterceptChance(turret, track, ordnance);

        public ActionResult TryFireVolley(string turretId, string trackId)
        {
            var turret = GetTurret(turretId);
            if (turret == null)
                return ActionResult.Blocked("unknown_turret", "sky.unknown_turret");
            var track = GetTrack(trackId);
            if (track == null)
                return ActionResult.Blocked("unknown_track", "sky.unknown_track");
            if (track.resolved)
                return ActionResult.Blocked("track_resolved", "sky.track_resolved");
            if (!turret.is_operational)
                return ActionResult.Blocked("turret_down", "sky.turret_down");
            if (string.IsNullOrEmpty(turret.loaded_ammo_id) || turret.magazine_count <= 0)
                return ActionResult.Blocked("magazine_empty", "sky.magazine_empty");
            if (turret.barrel_heat >= HeatSeizureThreshold)
                return ActionResult.Blocked("barrel_hot", "sky.barrel_hot");
            if (turret.hydraulic_condition <= 0)
                return ActionResult.Blocked("hydraulics_failed", "sky.hydraulics_failed");

            var ordnance = _ordnance.Values.FirstOrDefault(o => o.item_id == turret.loaded_ammo_id);
            if (ordnance == null)
                return ActionResult.Blocked("unknown_ordnance", "sky.unknown_ordnance");

            // Author the firing solution: deterministic angles from the track.
            turret.azimuth = ((track.target_grid_x * 17) % 360 + 360) % 360;
            turret.elevation = Math.Clamp(30 + track.target_grid_x % 20, 0, 90);

            int chance = ComputeInterceptChance(turret, track, ordnance);
            var rng = StreamFor(track.track_id, track.volleys_fired);
            int roll = rng.Next(0, 100);
            bool intercepted = roll < chance;

            // State costs — magazine + heat + hydraulics (magazine is the countable authority).
            track.volleys_fired++;
            turret.magazine_count--;
            turret.barrel_heat = Math.Min(100, turret.barrel_heat + ordnance.heat_per_volley);
            turret.volleys_since_service++;
            turret.hydraulic_condition = Math.Max(0, turret.hydraulic_condition - ordnance.recoil_load / 2);
            _state.total_volleys++;

            float residual = intercepted ? ordnance.residual_shrapnel_severity : 1f;
            bool mitigated = intercepted
                && _telemetry != null
                && _telemetry.ApplyInterceptionMitigation(track.track_id, residual);

            _log.Info($"[SkyDefense] volley {track.volleys_fired} at '{track.track_id}': " +
                      $"chance {chance}, roll {roll}, intercepted={intercepted} (mitigated={mitigated})");

            OnVolleyFired?.Invoke(turretId, turret.loaded_ammo_id, turret.magazine_count);
            OnInterceptResolved?.Invoke(track.track_id, turret.loaded_ammo_id, intercepted, intercepted ? residual : 1f);

            if (turret.volleys_since_service >= VolleysPerService)
                OnMaintenanceDue?.Invoke(turretId);

            if (intercepted)
                _state.total_interceptions++;

            return ActionResult.Success("sky.volley_resolved",
                new Dictionary<string, double>
                {
                    { "intercept_chance", chance },
                    { "roll", roll },
                    { "intercepted", intercepted ? 1 : 0 },
                    { "residual_fraction", intercepted ? residual : 1f },
                });
        }

        private int ComputeInterceptChance(
            CounterBatteryTurretState turret, OrbitalTrackState track, SkyDefenseOrdnanceDefinition ordnance)
        {
            float chance = BaseInterceptChance;
            chance += (turret.radar_calibration - 50) * 0.2f;                       // -10..+10
            chance += (turret.hydraulic_condition - 50) * 0.1f;                     // -5..+5
            chance += ordnance.tracking_modifier * 5f;                              // -10..+10
            chance += ordnance.interception_modifier * 100f;                        // -20..+40

            int crewBonus = 0;
            foreach (var crew in turret.assigned_crew_ids)
            {
                if (_skills == null) break;
                if (_skills.HasSkill(crew, "skill_steady_hands") || _skills.HasSkill(crew, "skill_cold_analysis"))
                    crewBonus += 5;
            }
            chance += Math.Min(10, crewBonus);

            // Authored target difficulty by warning severity.
            chance += track.severity switch
            {
                "Minor" => 10,
                "Moderate" => 0,
                "Major" => -8,
                "Severe" => -12,
                _ => 0,
            };

            return (int)Math.Clamp(chance, MinInterceptChance, MaxInterceptChance);
        }

        // -----------------------------------------------------------------
        // Maintenance
        // -----------------------------------------------------------------

        /// <summary>Machine-oil hydraulic service. Atomic; resets the service counter.</summary>
        public ActionResult TryServiceHydraulics(string turretId)
        {
            var turret = GetTurret(turretId);
            if (turret == null)
                return ActionResult.Blocked("unknown_turret", "sky.unknown_turret");
            if (_inventory == null)
                return ActionResult.Blocked("no_inventory", "sky.no_inventory");

            var bill = new InventoryBill();
            bill.AddCost(ServiceOilItemId, 1);
            if (!_inventory.TryExecuteTransaction(bill))
                return ActionResult.Blocked("missing_oil", "sky.missing_oil");

            turret.volleys_since_service = 0;
            turret.hydraulic_condition = Math.Min(100, turret.hydraulic_condition + 40);
            turret.is_operational = turret.hydraulic_condition > 0;
            _log.Info($"[SkyDefense] '{turretId}' serviced (hydraulics {turret.hydraulic_condition})");
            OnServiced?.Invoke(turretId);
            return ActionResult.Success("sky.serviced",
                new Dictionary<string, double> { { "hydraulics", turret.hydraulic_condition } });
        }

        // -----------------------------------------------------------------
        // Daily tick
        // -----------------------------------------------------------------

        private int _currentDay;

        public void TickDay(int day)
        {
            _currentDay = day;

            foreach (var turret in _state.turrets)
            {
                turret.barrel_heat = Math.Max(0, turret.barrel_heat - DailyHeatDissipation);
                turret.radar_calibration = Math.Max(0, turret.radar_calibration - DailyRadarDrift);
                if (turret.hydraulic_condition <= 0)
                    turret.is_operational = false;
            }

            // Tracks whose impact day has passed (telemetry resolved them).
            _state.tracks.RemoveAll(t => day > t.impact_day && !t.resolved);
        }

        // -----------------------------------------------------------------
        // Keyed RNG streams
        // -----------------------------------------------------------------

        private SeededRng StreamFor(string trackId, int volley)
        {
            ulong h = 1469598103934665603UL;
            foreach (char c in trackId)
            {
                h ^= c;
                h *= 1099511628211UL;
            }
            h ^= (uint)volley;
            h *= 1099511628211UL;
            h ^= (uint)_masterSeed;
            h *= 1099511628211UL;
            return new SeededRng(unchecked((int)(h ^ (h >> 32))));
        }

        // -----------------------------------------------------------------
        // Save / restore
        // -----------------------------------------------------------------

        public SkyDefenseBatterySave CaptureState() => Clone(_state);

        public void RestoreState(SkyDefenseBatterySave? saved)
        {
            if (saved == null) return;
            _state = Clone(saved);
        }

        private static SkyDefenseBatterySave Clone(SkyDefenseBatterySave src)
        {
            var json = new SystemTextJsonSerializer();
            return json.Deserialize<SkyDefenseBatterySave>(json.Serialize(src)) ?? new SkyDefenseBatterySave();
        }
    }
}
