using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Shelter
{
    // ── Fire zone ───────────────────────────────────────────────────

    /// <summary>State of one shelter zone during a fire incident.</summary>
    [Serializable]
    public class FireZoneState
    {
        public string zoneId = string.Empty;
        public string displayName = string.Empty;
        public float fireLevel = 0f;        // 0..1, intensity
        public float smokeLevel = 0f;       // 0..1, particulate
        public float coLevel = 0f;          // 0..1, carbon monoxide
        public float heatLevel = 0f;        // 0..1, temperature
        public bool damperOpen = true;      // true = airflow through zone
        public bool isEvacuated = false;
        public List<string> adjacentZoneIds = new List<string>();
    }

    /// <summary>Fire incident state (save DTO).</summary>
    [Serializable]
    public class FireIncidentState
    {
        public string systemId = ShelterFireHazardSystem.SystemId;
        public string incidentId = string.Empty;
        public string sourceZoneId = string.Empty;
        public int ignitionDay = 0;
        public int ticksElapsed = 0;
        public bool alarmRaised = false;
        public bool isSuppressed = false;
        public bool isResolved = false;
        public string resolution = string.Empty; // suppressed, burned_out, contained
        public List<FireZoneState> zones = new List<FireZoneState>();
        public List<string> brigadeWorkers = new List<string>();
        public int extinguisherChargesUsed = 0;
        public float structuralDamage = 0f;
    }

    // ── System ──────────────────────────────────────────────────────

    /// <summary>
    /// ASHFALL — Bunker Ventilation Fire and Smoke Asphyxiation system.
    /// Industrial, kitchen, electrical, or maintenance fires propagate
    /// through configured shelter zones. Players can raise an alarm,
    /// assign a brigade, close dampers, deploy extinguishers, isolate
    /// smoke, and repair damage before smoke and carbon monoxide cause
    /// lethal consequences.
    ///
    /// Fire advances on simulation ticks, not frame time.
    /// Smoke and CO are emitted into the existing VentilationSystem.
    /// </summary>
    public class ShelterFireHazardSystem
    {
        public const string SystemId = "shelter_fire_hazard";

        // Fire propagation constants
        public const float FireSpreadRate = 0.05f;       // per tick to adjacent zone
        public const float FireDecayRate = 0.02f;        // natural decay per tick
        public const float SmokePerFireUnit = 0.3f;      // smoke per fire level per tick
        public const float CoPerFireUnit = 0.1f;         // CO per fire level per tick
        public const float HeatPerFireUnit = 0.4f;       // heat per fire level per tick
        public const float SmokeDecayRate = 0.05f;       // natural dissipation
        public const float CoDecayRate = 0.01f;          // natural dissipation
        public const float HeatDecayRate = 0.1f;         // cooling rate

        // Suppression constants
        public const float BrigadeSuppressionPerWorker = 0.08f;
        public const float ExtinguisherSuppression = 0.25f;
        public const int ExtinguisherMaxCharges = 4;
        public const float DamperSmokeReduction = 0.5f;  // dampers reduce smoke spread
        public const float StructuralDamagePerTick = 0.02f;

        // Thresholds
        public const float CriticalSmokeLevel = 0.6f;
        public const float CriticalCoLevel = 0.4f;
        public const float LethalCoLevel = 0.8f;

        private readonly Dictionary<string, FireIncidentState> _incidents = new Dictionary<string, FireIncidentState>();

        // Events
        public event Action<string> OnAlarmRaised;            // incidentId
        public event Action<string, string> OnFireIgnited;    // incidentId, zoneId
        public event Action<string, string> OnSmokeZoneChanged; // incidentId, zoneId
        public event Action<string, string> OnDamperChanged;  // incidentId, zoneId
        public event Action<string> OnBrigadeDispatched;      // incidentId
        public event Action<string, float> OnEquipmentDamaged; // incidentId, damage
        public event Action<string, string> OnSurvivorExposed; // incidentId, zoneId
        public event Action<string> OnIncidentSuppressed;     // incidentId
        public event Action<string> OnIncidentResolved;       // incidentId
        public event Action<Dictionary<string, FireIncidentState>> OnStateChanged;

        public IReadOnlyDictionary<string, FireIncidentState> Incidents => _incidents;

        public ShelterFireHazardSystem()
        {
        }

        // ── Ignition ────────────────────────────────────────────────

        /// <summary>Start a fire incident in a zone.</summary>
        public bool Ignite(string incidentId, string sourceZoneId, int day, List<FireZoneState> zones)
        {
            if (string.IsNullOrEmpty(incidentId) || string.IsNullOrEmpty(sourceZoneId)) return false;
            if (_incidents.ContainsKey(incidentId)) return false;
            if (zones == null || zones.Count == 0) return false;

            var incident = new FireIncidentState
            {
                incidentId = incidentId,
                sourceZoneId = sourceZoneId,
                ignitionDay = day,
                ticksElapsed = 0,
                alarmRaised = false,
                isSuppressed = false,
                isResolved = false,
                zones = new List<FireZoneState>(),
                brigadeWorkers = new List<string>(),
                extinguisherChargesUsed = 0,
                structuralDamage = 0f
            };

            // Deep copy zones
            foreach (var z in zones)
            {
                incident.zones.Add(new FireZoneState
                {
                    zoneId = z.zoneId,
                    displayName = z.displayName,
                    fireLevel = z.zoneId == sourceZoneId ? 0.3f : 0f,
                    smokeLevel = z.zoneId == sourceZoneId ? 0.1f : 0f,
                    coLevel = 0f,
                    heatLevel = z.zoneId == sourceZoneId ? 0.1f : 0f,
                    damperOpen = z.damperOpen,
                    isEvacuated = false,
                    adjacentZoneIds = new List<string>(z.adjacentZoneIds)
                });
            }

            _incidents[incidentId] = incident;
            OnFireIgnited?.Invoke(incidentId, sourceZoneId);
            RaiseChanged();
            return true;
        }

        // ── Alarm ───────────────────────────────────────────────────

        /// <summary>Raise the fire alarm.</summary>
        public bool RaiseAlarm(string incidentId)
        {
            if (!_incidents.TryGetValue(incidentId, out var incident)) return false;
            if (incident.alarmRaised) return false;
            incident.alarmRaised = true;
            OnAlarmRaised?.Invoke(incidentId);
            RaiseChanged();
            return true;
        }

        // ── Brigade ─────────────────────────────────────────────────

        /// <summary>Assign workers to the fire brigade.</summary>
        public bool AssignBrigade(string incidentId, List<string> workerIds)
        {
            if (!_incidents.TryGetValue(incidentId, out var incident)) return false;
            if (incident.isResolved) return false;
            incident.brigadeWorkers.Clear();
            incident.brigadeWorkers.AddRange(workerIds);
            OnBrigadeDispatched?.Invoke(incidentId);
            RaiseChanged();
            return true;
        }

        // ── Dampers ─────────────────────────────────────────────────

        /// <summary>Toggle a damper in a zone.</summary>
        public bool SetDamper(string incidentId, string zoneId, bool open)
        {
            if (!_incidents.TryGetValue(incidentId, out var incident)) return false;
            foreach (var z in incident.zones)
            {
                if (z.zoneId == zoneId)
                {
                    z.damperOpen = open;
                    OnDamperChanged?.Invoke(incidentId, zoneId);
                    RaiseChanged();
                    return true;
                }
            }
            return false;
        }

        // ── Extinguisher ────────────────────────────────────────────

        /// <summary>Deploy an extinguisher charge in a zone.</summary>
        public bool DeployExtinguisher(string incidentId, string zoneId)
        {
            if (!_incidents.TryGetValue(incidentId, out var incident)) return false;
            if (incident.isResolved) return false;
            if (incident.extinguisherChargesUsed >= ExtinguisherMaxCharges) return false;

            foreach (var z in incident.zones)
            {
                if (z.zoneId == zoneId)
                {
                    z.fireLevel = Math.Max(0f, z.fireLevel - ExtinguisherSuppression);
                    incident.extinguisherChargesUsed++;
                    RaiseChanged();
                    return true;
                }
            }
            return false;
        }

        // ── Evacuation ──────────────────────────────────────────────

        /// <summary>Evacuate a zone.</summary>
        public bool EvacuateZone(string incidentId, string zoneId)
        {
            if (!_incidents.TryGetValue(incidentId, out var incident)) return false;
            foreach (var z in incident.zones)
            {
                if (z.zoneId == zoneId)
                {
                    z.isEvacuated = true;
                    RaiseChanged();
                    return true;
                }
            }
            return false;
        }

        // ── Tick ────────────────────────────────────────────────────

        /// <summary>
        /// Advance one simulation tick. Propagates fire, generates smoke/CO,
        /// applies brigade suppression, and checks resolution.
        /// </summary>
        public void Tick(string incidentId, ISeededRng rng)
        {
            if (!_incidents.TryGetValue(incidentId, out var incident)) return;
            if (incident.isResolved) return;

            incident.ticksElapsed++;

            // Phase 1: Fire propagation
            foreach (var z in incident.zones)
            {
                if (z.fireLevel <= 0f) continue;

                // Spread to adjacent zones
                foreach (var adjId in z.adjacentZoneIds)
                {
                    var adj = FindZone(incident, adjId);
                    if (adj == null || adj.fireLevel > 0f) continue;

                    float spreadChance = FireSpreadRate * z.fireLevel;
                    if (!z.damperOpen) spreadChance *= 0.3f; // closed damper reduces spread
                    if (rng != null && rng.NextDouble() < spreadChance)
                    {
                        adj.fireLevel = 0.1f; // ignition
                    }
                }
            }

            // Phase 2: Fire growth and decay
            foreach (var z in incident.zones)
            {
                if (z.fireLevel <= 0f) continue;

                // Growth (fire feeds itself)
                z.fireLevel = Math.Min(1f, z.fireLevel * 1.05f);

                // Natural decay
                z.fireLevel = Math.Max(0f, z.fireLevel - FireDecayRate);

                // Smoke and CO generation
                float smokeGen = z.fireLevel * SmokePerFireUnit;
                float coGen = z.fireLevel * CoPerFireUnit;
                float heatGen = z.fireLevel * HeatPerFireUnit;

                // Dampers affect spread but not local generation
                z.smokeLevel = Math.Min(1f, z.smokeLevel + smokeGen);
                z.coLevel = Math.Min(1f, z.coLevel + coGen);
                z.heatLevel = Math.Min(1f, z.heatLevel + heatGen);

                // Check survivor exposure
                if (!z.isEvacuated && (z.smokeLevel > CriticalSmokeLevel || z.coLevel > CriticalCoLevel))
                {
                    OnSmokeZoneChanged?.Invoke(incidentId, z.zoneId);
                    OnSurvivorExposed?.Invoke(incidentId, z.zoneId);
                }
            }

            // Phase 3: Smoke/CO spread through open dampers
            foreach (var z in incident.zones)
            {
                if (z.smokeLevel <= 0.1f && z.coLevel <= 0.1f) continue;

                foreach (var adjId in z.adjacentZoneIds)
                {
                    var adj = FindZone(incident, adjId);
                    if (adj == null) continue;

                    // Smoke spreads through open dampers
                    if (z.damperOpen && adj.damperOpen)
                    {
                        float smokeTransfer = z.smokeLevel * 0.1f;
                        float coTransfer = z.coLevel * 0.1f;
                        adj.smokeLevel = Math.Min(1f, adj.smokeLevel + smokeTransfer);
                        adj.coLevel = Math.Min(1f, adj.coLevel + coTransfer);
                    }
                }
            }

            // Phase 4: Natural dissipation
            foreach (var z in incident.zones)
            {
                z.smokeLevel = Math.Max(0f, z.smokeLevel - SmokeDecayRate);
                z.coLevel = Math.Max(0f, z.coLevel - CoDecayRate);
                z.heatLevel = Math.Max(0f, z.heatLevel - HeatDecayRate);
            }

            // Phase 5: Brigade suppression
            if (incident.brigadeWorkers.Count > 0)
            {
                float suppression = BrigadeSuppressionPerWorker * incident.brigadeWorkers.Count;
                // Apply to hottest zone
                FireZoneState? hottest = null;
                foreach (var z in incident.zones)
                {
                    if (hottest == null || z.fireLevel > hottest.fireLevel)
                        hottest = z;
                }
                if (hottest != null && hottest.fireLevel > 0f)
                {
                    hottest.fireLevel = Math.Max(0f, hottest.fireLevel - suppression);
                }
            }

            // Phase 6: Structural damage
            float totalFire = 0f;
            foreach (var z in incident.zones) totalFire += z.fireLevel;
            if (totalFire > 0f)
            {
                float damage = StructuralDamagePerTick * totalFire;
                incident.structuralDamage = Math.Min(1f, incident.structuralDamage + damage);
                OnEquipmentDamaged?.Invoke(incidentId, damage);
            }

            // Phase 7: Check resolution
            float remainingFire = 0f;
            foreach (var z in incident.zones) remainingFire += z.fireLevel;

            if (remainingFire <= 0.01f)
            {
                incident.isResolved = true;
                incident.resolution = "suppressed";
                incident.isSuppressed = true;
                OnIncidentSuppressed?.Invoke(incidentId);
                OnIncidentResolved?.Invoke(incidentId);
            }
            else if (incident.ticksElapsed > 50 && remainingFire < 0.1f)
            {
                incident.isResolved = true;
                incident.resolution = "contained";
                OnIncidentResolved?.Invoke(incidentId);
            }

            RaiseChanged();
        }

        // ── Queries ──────────────────────────────────────────────────

        public FireIncidentState? GetIncident(string incidentId)
        {
            return _incidents.TryGetValue(incidentId, out var incident) ? incident : null;
        }

        public bool IsResolved(string incidentId)
        {
            return _incidents.TryGetValue(incidentId, out var incident) && incident.isResolved;
        }

        public float GetMaxCoLevel(string incidentId)
        {
            if (!_incidents.TryGetValue(incidentId, out var incident)) return 0f;
            float max = 0f;
            foreach (var z in incident.zones)
                if (z.coLevel > max) max = z.coLevel;
            return max;
        }

        public float GetMaxSmokeLevel(string incidentId)
        {
            if (!_incidents.TryGetValue(incidentId, out var incident)) return 0f;
            float max = 0f;
            foreach (var z in incident.zones)
                if (z.smokeLevel > max) max = z.smokeLevel;
            return max;
        }

        // ── Helpers ──────────────────────────────────────────────────

        private static FireZoneState? FindZone(FireIncidentState incident, string zoneId)
        {
            foreach (var z in incident.zones)
                if (z.zoneId == zoneId) return z;
            return null;
        }

        // ── Save / Load ──────────────────────────────────────────────

        public Dictionary<string, FireIncidentState> CaptureState()
        {
            var copy = new Dictionary<string, FireIncidentState>();
            foreach (var kv in _incidents)
            {
                var src = kv.Value;
                var dst = new FireIncidentState
                {
                    systemId = src.systemId,
                    incidentId = src.incidentId,
                    sourceZoneId = src.sourceZoneId,
                    ignitionDay = src.ignitionDay,
                    ticksElapsed = src.ticksElapsed,
                    alarmRaised = src.alarmRaised,
                    isSuppressed = src.isSuppressed,
                    isResolved = src.isResolved,
                    resolution = src.resolution,
                    extinguisherChargesUsed = src.extinguisherChargesUsed,
                    structuralDamage = src.structuralDamage,
                    brigadeWorkers = new List<string>(src.brigadeWorkers),
                    zones = new List<FireZoneState>()
                };
                foreach (var z in src.zones)
                {
                    dst.zones.Add(new FireZoneState
                    {
                        zoneId = z.zoneId,
                        displayName = z.displayName,
                        fireLevel = z.fireLevel,
                        smokeLevel = z.smokeLevel,
                        coLevel = z.coLevel,
                        heatLevel = z.heatLevel,
                        damperOpen = z.damperOpen,
                        isEvacuated = z.isEvacuated,
                        adjacentZoneIds = new List<string>(z.adjacentZoneIds)
                    });
                }
                copy[kv.Key] = dst;
            }
            return copy;
        }

        public void RestoreState(Dictionary<string, FireIncidentState> saved)
        {
            _incidents.Clear();
            if (saved == null) return;
            foreach (var kv in saved)
            {
                if (kv.Value == null || string.IsNullOrEmpty(kv.Value.incidentId)) continue;
                _incidents[kv.Key] = kv.Value;
            }
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_incidents);
    }
}
