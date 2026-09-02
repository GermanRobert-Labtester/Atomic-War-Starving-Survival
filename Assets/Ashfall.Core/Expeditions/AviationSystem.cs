using System;
using System.Collections.Generic;
using Ashfall.Core.Save;
#pragma warning disable CS8618

namespace Ashfall.Core.Expeditions
{
    public enum FlightPhase
    {
        Preparing,
        AirborneOutbound,
        OnStation,
        AirborneReturn,
        Landed,
        ForcedLanding,
        Crashed,
        Rescued
    }

    [Serializable]
    public class AircraftDefinition
    {
        public string aircraft_id = string.Empty;
        public string name = string.Empty;
        public string category = "Balloon"; // Balloon, Glider, Ultralight
        public int crew_requirement = 1;
        public float payload_mass = 80f;
        public string fuel_type = "none";
        public float base_fuel_burn = 0f;
        public float cruise_range = 30f;
        public string speed_class = "Low";
        public float wind_tolerance = 18f;
        public float visibility_tolerance = 0.35f;
        public float cold_tolerance = -10f;
        public float structural_reliability = 0.85f;
        public int discovery_radius = 4;
        public int cargo_slots = 2;
        public float anti_air_exposure = 0.45f;
        public List<string> tags = new List<string>();
    }

    [Serializable]
    public class AircraftPartsCatalog
    {
        public int schema_version = 1;
        public List<AircraftDefinition> aircraft = new List<AircraftDefinition>();
    }

    [Serializable]
    public class AircraftRuntimeState
    {
        public string aircraftId = string.Empty;
        public string definitionId = string.Empty;
        public float airworthiness = 100f; // 0..100
        public float totalHoursFlown = 0f;
        public bool isCommitted = false;
    }

    [Serializable]
    public class FlightRiskBreakdown
    {
        public float windShearRisk;
        public float visibilityRisk;
        public float icingRisk;
        public float mechanicalRisk;
        public float antiAirRisk;
        public float totalRisk; // 0..1
    }

    [Serializable]
    public class FlightPlan
    {
        public string flightId = string.Empty;
        public string aircraftId = string.Empty;
        public List<string> pilotIds = new List<string>();
        public string originNodeId = string.Empty;
        public string destinationNodeId = string.Empty;
        public float payloadMass = 0f;
        public float fuelLoaded = 0f;
        public float routeDistanceKm = 30f;
        public FlightPhase phase = FlightPhase.Preparing;
        public float progressKm = 0f;
        public int mapCellsRevealed = 0;
        public bool rescueRequired = false;
        public string incidentLog = string.Empty;
    }

    [Serializable]
    public class AviationState
    {
        public string systemId = "aviation_system";
        public List<AircraftRuntimeState> aircraft = new List<AircraftRuntimeState>();
        public List<FlightPlan> activeFlights = new List<FlightPlan>();
        public List<FlightPlan> flightHistory = new List<FlightPlan>();
        public int totalMissionsLaunched = 0;
        public int totalSuccessfulLandings = 0;
        public int totalCrashes = 0;
    }

    public class AviationSystem
    {
        public const string SystemId = "aviation_system";

        private readonly Dictionary<string, AircraftDefinition> _definitions = new Dictionary<string, AircraftDefinition>(StringComparer.Ordinal);
        private readonly Dictionary<string, AircraftRuntimeState> _aircraft = new Dictionary<string, AircraftRuntimeState>(StringComparer.Ordinal);
        private readonly Dictionary<string, FlightPlan> _activeFlights = new Dictionary<string, FlightPlan>(StringComparer.Ordinal);
        private readonly List<FlightPlan> _history = new List<FlightPlan>();

        private int _totalLaunched;
        private int _totalLanded;
        private int _totalCrashes;

        public event Action<FlightPlan>? OnFlightLaunched;
        public event Action<FlightPlan, float>? OnFlightProgressed;
        public event Action<FlightPlan, int>? OnAerialMappingPerformed;
        public event Action<FlightPlan, string>? OnForcedLanding;
        public event Action<FlightPlan, string>? OnFlightCrashed;
        public event Action<FlightPlan>? OnFlightReturned;

        public IReadOnlyCollection<AircraftRuntimeState> Aircraft => _aircraft.Values;
        public IReadOnlyCollection<FlightPlan> ActiveFlights => _activeFlights.Values;
        public IReadOnlyList<FlightPlan> History => _history;

        public int TotalLaunched => _totalLaunched;
        public int TotalLanded => _totalLanded;
        public int TotalCrashes => _totalCrashes;

        public void LoadCatalog(string jsonText, IJsonSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(jsonText) || serializer == null) return;
            try
            {
                var catalog = serializer.Deserialize<AircraftPartsCatalog>(jsonText);
                if (catalog?.aircraft != null)
                {
                    _definitions.Clear();
                    foreach (var def in catalog.aircraft)
                    {
                        if (!string.IsNullOrEmpty(def.aircraft_id))
                            _definitions[def.aircraft_id] = def;
                    }
                }
            }
            catch
            {
                // Graceful fallback
            }
        }

        public AircraftDefinition? GetDefinition(string defId)
        {
            return _definitions.TryGetValue(defId, out var def) ? def : null;
        }

        public AircraftRuntimeState RegisterAircraft(string aircraftId, string definitionId)
        {
            if (string.IsNullOrEmpty(aircraftId)) throw new ArgumentException("aircraftId required", nameof(aircraftId));
            var state = new AircraftRuntimeState
            {
                aircraftId = aircraftId,
                definitionId = definitionId,
                airworthiness = 100f,
                totalHoursFlown = 0f,
                isCommitted = false
            };
            _aircraft[aircraftId] = state;
            return state;
        }

        public float CalculateFlightRange(AircraftDefinition def, float payloadMass, float fuelAmount, float windFactor)
        {
            if (def == null) return 0f;
            float massPenalty = Math.Max(0f, (payloadMass - (def.payload_mass * 0.5f)) / Math.Max(1f, def.payload_mass));
            float effRange = def.cruise_range * Math.Max(0.4f, 1f - (massPenalty * 0.5f));

            if (def.base_fuel_burn > 0.001f)
            {
                float fuelBonus = (fuelAmount / def.base_fuel_burn) * 10f;
                effRange = Math.Min(effRange * 1.5f, fuelBonus);
            }

            effRange *= Math.Max(0.2f, windFactor);
            return Math.Max(5f, effRange);
        }

        public FlightRiskBreakdown CalculateFlightRisk(AircraftDefinition def, AircraftRuntimeState? plane, float windSpeedKnots, float visibility, float temperatureC, float hostileAaExposure)
        {
            var risk = new FlightRiskBreakdown();
            if (def == null)
            {
                risk.totalRisk = 1f;
                return risk;
            }

            // Wind shear
            if (windSpeedKnots > def.wind_tolerance)
            {
                risk.windShearRisk = Math.Min(0.5f, (windSpeedKnots - def.wind_tolerance) * 0.03f);
            }

            // Visibility
            if (visibility < def.visibility_tolerance)
            {
                risk.visibilityRisk = Math.Min(0.4f, (def.visibility_tolerance - visibility) * 1.2f);
            }

            // Icing / Cold
            if (temperatureC < def.cold_tolerance)
            {
                risk.icingRisk = Math.Min(0.35f, (def.cold_tolerance - temperatureC) * 0.02f);
            }

            // Mechanical condition
            float condition = plane != null ? plane.airworthiness / 100f : def.structural_reliability;
            risk.mechanicalRisk = Math.Max(0.02f, (1f - condition) * 0.4f);

            // Anti-Air exposure
            risk.antiAirRisk = Math.Clamp(def.anti_air_exposure * hostileAaExposure * 0.5f, 0f, 0.6f);

            risk.totalRisk = Math.Clamp(risk.windShearRisk + risk.visibilityRisk + risk.icingRisk + risk.mechanicalRisk + risk.antiAirRisk, 0.05f, 0.95f);
            return risk;
        }

        public bool ValidateFlightPlan(string aircraftId, List<string> pilots, float payload, float fuel, out string failureReason)
        {
            if (!_aircraft.TryGetValue(aircraftId, out var plane))
            {
                failureReason = "Aircraft not found in hangar";
                return false;
            }
            if (plane.isCommitted)
            {
                failureReason = "Aircraft is already committed to an active flight";
                return false;
            }
            if (plane.airworthiness < 30f)
            {
                failureReason = "Airworthiness critically degraded; repairs required";
                return false;
            }
            if (!_definitions.TryGetValue(plane.definitionId, out var def))
            {
                failureReason = "Aircraft definition missing";
                return false;
            }
            if (pilots == null || pilots.Count < def.crew_requirement)
            {
                failureReason = $"Insufficient crew; requires at least {def.crew_requirement} pilot(s)";
                return false;
            }
            if (payload > def.payload_mass * 1.5f)
            {
                failureReason = "Payload exceeds maximum structural airframe limit";
                return false;
            }
            if (def.base_fuel_burn > 0.001f && fuel < def.base_fuel_burn)
            {
                failureReason = "Insufficient fuel for minimum flight envelope";
                return false;
            }

            failureReason = string.Empty;
            return true;
        }

        public FlightPlan LaunchFlight(string flightId, string aircraftId, List<string> pilots, string origin, string dest, float distanceKm, float payload, float fuel)
        {
            if (!ValidateFlightPlan(aircraftId, pilots, payload, fuel, out var blocker))
                throw new InvalidOperationException($"Cannot launch flight: {blocker}");

            var plane = _aircraft[aircraftId];
            plane.isCommitted = true;

            var plan = new FlightPlan
            {
                flightId = flightId,
                aircraftId = aircraftId,
                pilotIds = new List<string>(pilots),
                originNodeId = origin,
                destinationNodeId = dest,
                routeDistanceKm = Math.Max(5f, distanceKm),
                payloadMass = payload,
                fuelLoaded = fuel,
                phase = FlightPhase.AirborneOutbound,
                progressKm = 0f,
                mapCellsRevealed = 0,
                rescueRequired = false
            };

            _activeFlights[flightId] = plan;
            _totalLaunched++;
            OnFlightLaunched?.Invoke(plan);
            return plan;
        }

        public void AdvanceFlightTick(string flightId, float deltaHours, float windSpeed, float visibility, float tempC, float hostileAa, ISeededRng rng)
        {
            if (!_activeFlights.TryGetValue(flightId, out var plan)) return;
            if (plan.phase != FlightPhase.AirborneOutbound && plan.phase != FlightPhase.AirborneReturn && plan.phase != FlightPhase.OnStation)
                return;

            if (!_aircraft.TryGetValue(plan.aircraftId, out var plane)) return;
            _definitions.TryGetValue(plane.definitionId, out var def);

            // Speed approximation
            float speedKmH = 25f;
            if (def != null)
            {
                speedKmH = def.speed_class switch
                {
                    "High" => 75f,
                    "Medium" => 45f,
                    _ => 20f
                };
            }

            float kmTraveled = speedKmH * deltaHours;
            plan.progressKm += kmTraveled;
            plane.totalHoursFlown += deltaHours;
            plane.airworthiness = Math.Max(0f, plane.airworthiness - (deltaHours * 1.5f));

            // Aerial discovery
            int radius = def != null ? def.discovery_radius : 3;
            int newCells = (int)(radius * 1.5f * deltaHours);
            if (newCells > 0)
            {
                plan.mapCellsRevealed += newCells;
                OnAerialMappingPerformed?.Invoke(plan, newCells);
            }

            OnFlightProgressed?.Invoke(plan, plan.progressKm);

            // Hazard Check
            var risk = CalculateFlightRisk(def!, plane, windSpeed, visibility, tempC, hostileAa);
            double roll = rng.NextDouble();

            if (roll < risk.totalRisk * 0.15f) // incident trigger threshold
            {
                // Incident roll
                double severity = rng.NextDouble();
                if (severity < 0.35)
                {
                    // Forced Landing
                    plan.phase = FlightPhase.ForcedLanding;
                    plan.rescueRequired = true;
                    plan.incidentLog = "Forced landing due to violent wind shear and airframe fatigue.";
                    plane.isCommitted = false;
                    _history.Add(plan);
                    _activeFlights.Remove(flightId);
                    OnForcedLanding?.Invoke(plan, plan.incidentLog);
                    return;
                }
                else if (severity < 0.65 && risk.antiAirRisk > 0.1f)
                {
                    // Shot Down by Anti-Air
                    plan.phase = FlightPhase.Crashed;
                    plan.rescueRequired = true;
                    plan.incidentLog = "Intercepted and crippled by hostile surface anti-air flak.";
                    plane.isCommitted = false;
                    _totalCrashes++;
                    _history.Add(plan);
                    _activeFlights.Remove(flightId);
                    OnFlightCrashed?.Invoke(plan, plan.incidentLog);
                    return;
                }
                else if (severity < 0.90)
                {
                    // Mechanical failure crash
                    plan.phase = FlightPhase.Crashed;
                    plan.rescueRequired = true;
                    plan.incidentLog = "Catastrophic structural failure in flight; emergency crash landing.";
                    plane.isCommitted = false;
                    _totalCrashes++;
                    _history.Add(plan);
                    _activeFlights.Remove(flightId);
                    OnFlightCrashed?.Invoke(plan, plan.incidentLog);
                    return;
                }
            }

            // Normal flight phase advance
            if (plan.progressKm >= plan.routeDistanceKm * 0.5f && plan.phase == FlightPhase.AirborneOutbound)
            {
                plan.phase = FlightPhase.AirborneReturn;
            }
            else if (plan.progressKm >= plan.routeDistanceKm)
            {
                // Completed flight!
                plan.phase = FlightPhase.Landed;
                plane.isCommitted = false;
                _totalLanded++;
                _history.Add(plan);
                _activeFlights.Remove(flightId);
                OnFlightReturned?.Invoke(plan);
            }
        }

        public bool ResolveCrashRescue(string flightId, bool success)
        {
            var plan = _history.Find(p => p.flightId == flightId);
            if (plan == null || !plan.rescueRequired) return false;

            plan.rescueRequired = false;
            plan.phase = success ? FlightPhase.Rescued : FlightPhase.Crashed;
            return true;
        }

        public AviationState CaptureState()
        {
            var state = new AviationState
            {
                systemId = SystemId,
                totalMissionsLaunched = _totalLaunched,
                totalSuccessfulLandings = _totalLanded,
                totalCrashes = _totalCrashes
            };
            foreach (var kv in _aircraft) state.aircraft.Add(kv.Value);
            foreach (var kv in _activeFlights) state.activeFlights.Add(kv.Value);
            state.flightHistory.AddRange(_history);
            return state;
        }

        public void RestoreState(AviationState? state)
        {
            _aircraft.Clear();
            _activeFlights.Clear();
            _history.Clear();
            _totalLaunched = 0;
            _totalLanded = 0;
            _totalCrashes = 0;

            if (state == null) return;

            _totalLaunched = state.totalMissionsLaunched;
            _totalLanded = state.totalSuccessfulLandings;
            _totalCrashes = state.totalCrashes;

            if (state.aircraft != null)
            {
                foreach (var p in state.aircraft)
                {
                    if (!string.IsNullOrEmpty(p.aircraftId))
                        _aircraft[p.aircraftId] = p;
                }
            }
            if (state.activeFlights != null)
            {
                foreach (var f in state.activeFlights)
                {
                    if (!string.IsNullOrEmpty(f.flightId))
                        _activeFlights[f.flightId] = f;
                }
            }
            if (state.flightHistory != null)
            {
                _history.AddRange(state.flightHistory);
            }
        }
    }
}
