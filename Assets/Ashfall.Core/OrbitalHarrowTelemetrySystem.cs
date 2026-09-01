using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core.Shelter;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class OrbitalSalvageOpportunity
    {
        public string eventId = string.Empty;
        public string itemId = string.Empty;
        public int quantity = 1;
        public int targetGridX;
        public int spawnDay;
        public int expiresDay;
        public bool isClaimed;
    }

    [Serializable]
    public sealed class OrbitalTelemetryState
    {
        public string systemId = OrbitalHarrowTelemetrySystem.SystemId;
        public bool telemetryActive;
        public int lastImpactDay = -1;
        public int nextImpactDay = -1;
        public int warningLeadDays = 3;
        public int targetGridX = -1;
        public int affectedCellSpread = 1;
        public float impactEnergyMj = 10f;
        public string scheduledEventId = string.Empty;
        public string scheduledEventName = string.Empty;
        public string revealedSiteId = string.Empty;
        public bool isBraced;
        public bool braceUsed;
        public List<int> impactHistory = new List<int>();
        public List<OrbitalWarningEntry> warnings = new List<OrbitalWarningEntry>();
        public List<OrbitalSalvageOpportunity> activeSalvage = new List<OrbitalSalvageOpportunity>();
        public List<string> revealedSites = new List<string>();
    }

    [Serializable]
    public sealed class OrbitalWarningEntry
    {
        public int day;
        public int targetGridX;
        public float energyMj;
        public string eventId = string.Empty;
        public string telemetryText = string.Empty;
        public string severity = "Minor";
    }

    public sealed class OrbitalImpactReport
    {
        public int Day;
        public string EventId = string.Empty;
        public int TargetGridX;
        public int CellsAffected;
        public float TotalEnergyMj;
        public bool AnyBreached;
        public float TotalPenetrationDamage;
        public float PowerGridDisruption;
        public string SalvageItemId = string.Empty;
        public int SalvageQuantity;
        public string RevealedSiteId = string.Empty;
    }

    public sealed class OrbitalHarrowTelemetrySystem
    {
        public const string SystemId = "orbital_harrow_telemetry";

        private OrbitalTelemetryState _state = new OrbitalTelemetryState();
        private readonly SkyLayerArmorSystem _armor;
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private int _currentDay;

        public OrbitalTelemetryState State => _state;
        public bool HasPendingImpact => _state.nextImpactDay > _currentDay;
        public IReadOnlyList<OrbitalSalvageOpportunity> ActiveSalvage => _state.activeSalvage.AsReadOnly();
        public IReadOnlyList<string> RevealedSites => _state.revealedSites.AsReadOnly();

        public event Action<OrbitalWarningEntry> OnImpactWarning;
        public event Action<int, float> OnImpactResolved; // day, energy
        public event Action<OrbitalImpactReport> OnImpactDetailed;
        public event Action OnTelemetryChanged;

        public OrbitalHarrowTelemetrySystem(SkyLayerArmorSystem armor, ISeededRng rng, ILog? log = null)
        {
            _armor = armor ?? throw new ArgumentNullException(nameof(armor));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        public void ActivateTelemetry(int day)
        {
            _state.telemetryActive = true;
            _log.Info($"[OrbitalHarrow] telemetry activated on day {day}");
            OnTelemetryChanged?.Invoke();
        }

        public void ScheduleImpact(int day, int gridX, float energyMj)
        {
            ScheduleImpactInternal(day, gridX, energyMj, spread: 1, eventId: "custom_impact", eventName: "Kinetic Debris Impact", siteId: string.Empty, severity: "Moderate");
        }

        public void ScheduleEventDef(OrbitalEventDef def, int day, int gridX)
        {
            if (def == null) return;
            ScheduleImpactInternal(
                day: day,
                gridX: gridX,
                energyMj: def.impact_energy_mj,
                spread: Math.Max(1, def.affected_cell_spread),
                eventId: def.id,
                eventName: def.name,
                siteId: def.revealed_site_id,
                severity: def.severity);
        }

        private void ScheduleImpactInternal(
            int day,
            int gridX,
            float energyMj,
            int spread,
            string eventId,
            string eventName,
            string siteId,
            string severity)
        {
            _state.nextImpactDay = day;
            _state.targetGridX = gridX;
            _state.impactEnergyMj = energyMj;
            _state.affectedCellSpread = Math.Max(1, spread);
            _state.scheduledEventId = eventId;
            _state.scheduledEventName = eventName;
            _state.revealedSiteId = siteId ?? string.Empty;
            _state.isBraced = false;
            _state.braceUsed = false;

            var warning = new OrbitalWarningEntry
            {
                day = day,
                targetGridX = gridX,
                energyMj = energyMj,
                eventId = eventId,
                severity = severity,
                telemetryText = $"ORBITAL WARNING [Day {day}]: {eventName} over Grid {gridX} (Energy {energyMj:F1} MJ, Spread {spread} cells)"
            };
            _state.warnings.Add(warning);
            OnImpactWarning?.Invoke(warning);
            OnTelemetryChanged?.Invoke();
        }

        public ActionResult Brace(string materialId, int amount)
        {
            if (!HasPendingImpact)
                return ActionResult.Blocked("no_impact", "orbital.no_impact");
            if (_state.braceUsed)
                return ActionResult.Blocked("already_braced", "orbital.already_braced");

            _state.isBraced = true;
            _state.braceUsed = true;
            _log.Info($"[OrbitalHarrow] braced with {amount}x {materialId}");
            OnTelemetryChanged?.Invoke();
            return ActionResult.Success("orbital.braced",
                new Dictionary<string, double> { { "mitigation", _state.isBraced ? 0.5 : 0.0 } });
        }

        public void TickDay(int day)
        {
            _currentDay = day;

            // Prune expired salvage opportunities
            _state.activeSalvage.RemoveAll(s => s.expiresDay < day && !s.isClaimed);

            if (_state.nextImpactDay == day)
            {
                ResolveImpact();
            }
        }

        private void ResolveImpact()
        {
            float totalEnergy = _state.isBraced ? _state.impactEnergyMj * 0.5f : _state.impactEnergyMj;
            int spread = Math.Max(1, _state.affectedCellSpread);
            float perCellEnergy = totalEnergy / spread;

            bool anyBreached = false;
            float totalDamage = 0f;

            for (int offset = 0; offset < spread; offset++)
            {
                int cellX = _state.targetGridX + offset;
                bool breached = _armor.EvaluateKineticImpact(cellX, perCellEnergy, out float cellDamage);
                if (breached)
                {
                    anyBreached = true;
                    totalDamage += cellDamage;
                }
            }

            // Downstream shelter cascading power impact
            float powerDisruption = anyBreached ? Math.Min(100f, totalDamage * 2.5f) : 0f;

            // Spawn salvage aftermath if item yield exists
            string salvageItem = !string.IsNullOrEmpty(_state.scheduledEventId)
                ? GetSalvageItemForEvent(_state.scheduledEventId)
                : "scrap_mechanical";
            int salvageQty = Math.Max(1, (int)Math.Round(totalEnergy / 6f));

            var salvage = new OrbitalSalvageOpportunity
            {
                eventId = _state.scheduledEventId,
                itemId = salvageItem,
                quantity = salvageQty,
                targetGridX = _state.targetGridX,
                spawnDay = _state.nextImpactDay,
                expiresDay = _state.nextImpactDay + 7,
                isClaimed = false
            };
            _state.activeSalvage.Add(salvage);

            // Reveal hidden site if present
            if (!string.IsNullOrEmpty(_state.revealedSiteId) && !_state.revealedSites.Contains(_state.revealedSiteId))
            {
                _state.revealedSites.Add(_state.revealedSiteId);
            }

            int resolvedDay = _state.nextImpactDay;
            _state.impactHistory.Add(resolvedDay);
            _state.lastImpactDay = resolvedDay;
            _state.nextImpactDay = -1;
            _state.isBraced = false;

            var report = new OrbitalImpactReport
            {
                Day = resolvedDay,
                EventId = _state.scheduledEventId,
                TargetGridX = _state.targetGridX,
                CellsAffected = spread,
                TotalEnergyMj = totalEnergy,
                AnyBreached = anyBreached,
                TotalPenetrationDamage = totalDamage,
                PowerGridDisruption = powerDisruption,
                SalvageItemId = salvageItem,
                SalvageQuantity = salvageQty,
                RevealedSiteId = _state.revealedSiteId
            };

            _log.Info($"[OrbitalHarrow] impact resolved: breached={anyBreached}, damage={totalDamage:F1}, powerDisruption={powerDisruption:F1}");
            OnImpactResolved?.Invoke(resolvedDay, totalEnergy);
            OnImpactDetailed?.Invoke(report);
            OnTelemetryChanged?.Invoke();
        }

        public ActionResult ClaimSalvage(string eventId)
        {
            var opp = _state.activeSalvage.Find(s => s.eventId == eventId && !s.isClaimed);
            if (opp == null)
                return ActionResult.Blocked("not_found", "orbital.salvage_not_found");

            opp.isClaimed = true;
            OnTelemetryChanged?.Invoke();
            return ActionResult.Success("orbital.salvage_claimed",
                new Dictionary<string, double> { { "quantity", opp.quantity } });
        }

        private static string GetSalvageItemForEvent(string eventId)
        {
            return eventId switch
            {
                "event_orbital_kinetic_early_track" or "event_orbital_fragmented_kinetic_rod" or "event_orbital_small_debris_shower" => "scrap_mechanical",
                "event_orbital_kinetic_thermal_descent" or "event_orbital_standard_kinetic_strike" or "event_orbital_heavy_kinetic_impact" => "scrap_electronic",
                "event_orbital_kinetic_seismic_precursor" or "event_orbital_heavy_penetrator_impact" or "event_orbital_low_warning_strike" => "heavy_industrial_motor",
                "event_orbital_kinetic_fragmented_track" => "scrap_mechanical",
                "event_orbital_cluster_multiple_returns" or "event_orbital_telemetry_station_cluster" or "event_orbital_clustered_impact" => "copper_wire_10m_of_10m",
                "event_orbital_cluster_split_track" or "event_orbital_defense_submunitions_spread" => "mechanical_parts",
                "event_orbital_emp_radio_blackout" or "event_orbital_airburst_emp_detonation" => "battery",
                "event_orbital_emp_signature_mismatch" or "event_orbital_ionized_plasma_shockwave" or "event_orbital_near_miss_shockwave" => "fuel",
                "event_orbital_dead_hand_repeating_ping" => "scrap_electronic",
                "event_orbital_dead_hand_broken_checksum" => "scrap_electronic",
                "event_orbital_radar_ducting_false_alarm" => "scrap_mechanical",
                "event_orbital_debris_misclassification" => "scrap_metal",
                "event_orbital_catastrophic_kinetic_lance" => "scrap_electronic",
                "event_orbital_solar_array_debris_shower" => "scrap_mechanical",
                "event_orbital_booster_casing_decay" => "scrap_metal",
                _ => "scrap_mechanical"
            };
        }

        public OrbitalTelemetryState CaptureState() => CloneState(_state);

        public void RestoreState(OrbitalTelemetryState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static OrbitalTelemetryState CloneState(OrbitalTelemetryState src)
        {
            if (src == null) return new OrbitalTelemetryState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<OrbitalTelemetryState>(json) ?? new OrbitalTelemetryState();
        }
    }
}
