using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// Per-day environment inputs for <see cref="LocationEvolutionSystem.TickDay"/>.
    /// The host derives these from live weather so contamination drifts with
    /// real hazard conditions instead of a constant.
    /// </summary>
    public readonly struct LocationEvolutionInputs
    {
        /// <summary>WeatherSystem.OutdoorRadModifier for the day (1.0 = clear baseline).</summary>
        public readonly float OutdoorRadModifier;
        /// <summary>True while hazard weather (fallout storm / black rain) holds over the region.</summary>
        public readonly bool HazardWeather;

        public LocationEvolutionInputs(float outdoorRadModifier, bool hazardWeather)
        {
            OutdoorRadModifier = outdoorRadModifier;
            HazardWeather = hazardWeather;
        }

        public static LocationEvolutionInputs Clear => new LocationEvolutionInputs(1f, false);
    }

    public partial class LocationEvolutionSystem
    {
        // ── Tunables (named so designers can tune without touching math) ──
        /// <summary>Contamination gained per day under hazard weather, scaled by rad modifier.</summary>
        public const float ContaminationHazardGain = 0.02f;
        /// <summary>Contamination decay per day toward zero in clear weather.</summary>
        public const float ContaminationClearDecay = 0.01f;
        /// <summary>Contamination at or above which a location is written off as ruined.</summary>
        public const float RuinedContaminationThreshold = 1f;
        /// <summary>Daily chance a dormant threat appears at an unvisited, uncleared location.</summary>
        public const float ThreatSproutChance = 0.02f;
        /// <summary>Daily chance each active threat decays away on its own.</summary>
        public const float ThreatDecayChance = 0.05f;

        public const string ThreatSquatters = "threat_rad_squatters";
        public const string ThreatWildBeasts = "threat_wild_beasts";

        /// <summary>
        /// Read a record without creating one. Returns null for locations the
        /// world has never touched — callers must not treat that as blank
        /// authority to write over.
        /// </summary>
        public LocationMutationRecord? TryGetRecord(string locationId)
        {
            if (string.IsNullOrEmpty(locationId)) return null;
            return _state.mutations.Find(m => string.Equals(m.locationId, locationId, StringComparison.Ordinal));
        }

        /// <summary>Record a passage through a location (expedition visit, trade stop).</summary>
        public ActionResult MarkVisited(string locationId, int day)
        {
            var record = GetOrCreateRecord(locationId);
            if (record == null) return ActionResult.Failed("invalid_location", "location.invalid");
            record.lastVisitedDay = day;
            return ActionResult.Success("location.visited");
        }

        /// <summary>Scavenging pressure: raises depletion (0..1). Recovery is TickDay's job.</summary>
        public ActionResult MarkDepleted(string locationId, float amount)
        {
            var record = GetOrCreateRecord(locationId);
            if (record == null) return ActionResult.Failed("invalid_location", "location.invalid");
            record.lootDepletionFactor = Math.Clamp(record.lootDepletionFactor + Math.Max(0f, amount), 0f, 1f);
            OnLocationMutated?.Invoke(locationId);
            return ActionResult.Success("location.depleted");
        }

        /// <summary>Encounters, faction sweeps, or Collapse leftovers can threaten a location.</summary>
        public ActionResult AddThreat(string locationId, string threatId)
        {
            var record = GetOrCreateRecord(locationId);
            if (record == null) return ActionResult.Failed("invalid_location", "location.invalid");
            if (string.IsNullOrEmpty(threatId)) return ActionResult.Failed("invalid_threat", "location.invalid_threat");
            if (!record.activeThreats.Contains(threatId))
            {
                record.activeThreats.Add(threatId);
                OnLocationMutated?.Invoke(locationId);
            }
            return ActionResult.Success("location.threat_added");
        }

        public ActionResult RemoveThreat(string locationId, string threatId)
        {
            var record = GetOrCreateRecord(locationId);
            if (record == null) return ActionResult.Failed("invalid_location", "location.invalid");
            if (record.activeThreats.Remove(threatId))
            {
                OnLocationMutated?.Invoke(locationId);
                return ActionResult.Success("location.threat_removed");
            }
            return ActionResult.Blocked("no_such_threat", "location.no_such_threat");
        }

        /// <summary>
        /// Live-world daily tick: contamination drifts with the day's real
        /// radiation weather, contamination can ruin a location, and dormant
        /// threats sprout or decay on seeded rolls. The legacy
        /// <see cref="TickDay(int)"/> overload delegates here with clear-weather
        /// inputs and the constructor rng.
        /// </summary>
        public void TickDay(int day, in LocationEvolutionInputs inputs, ISeededRng? dayRng = null)
        {
            var rng = dayRng ?? _rng;
            _state.lastEvolutionDay = day;
            foreach (var rec in _state.mutations)
            {
                // Loot recovery for cleared locations left alone (existing rule).
                if (rec.isCleared && day - rec.lastVisitedDay > 20)
                {
                    rec.lootDepletionFactor = Math.Max(0f, rec.lootDepletionFactor - 0.05f);
                }

                // Contamination follows the sky: hazard weather pushes it up
                // (scaled by how hot the outdoors is), clear weather lets it settle.
                if (inputs.HazardWeather)
                {
                    rec.contaminationLevel = Math.Min(1f,
                        rec.contaminationLevel + ContaminationHazardGain * Math.Max(1f, inputs.OutdoorRadModifier / 150f));
                }
                else if (rec.contaminationLevel > 0f)
                {
                    rec.contaminationLevel = Math.Max(0f, rec.contaminationLevel - ContaminationClearDecay);
                }

                // Sustained contamination writes the location off. Ruin is sticky.
                if (!rec.isRuined && rec.contaminationLevel >= RuinedContaminationThreshold)
                {
                    rec.isRuined = true;
                    OnLocationMutated?.Invoke(rec.locationId);
                }

                // Threats: dormant ground sprouts trouble, watched trouble fades.
                if (rng != null)
                {
                    if (!rec.isCleared && rec.activeThreats.Count == 0
                        && rec.lastVisitedDay >= 0
                        && rng.NextDouble() < ThreatSproutChance)
                    {
                        rec.activeThreats.Add(rng.NextDouble() < 0.5 ? ThreatSquatters : ThreatWildBeasts);
                        OnLocationMutated?.Invoke(rec.locationId);
                    }
                    for (int i = rec.activeThreats.Count - 1; i >= 0; i--)
                    {
                        if (rng.NextDouble() < ThreatDecayChance)
                        {
                            rec.activeThreats.RemoveAt(i);
                            OnLocationMutated?.Invoke(rec.locationId);
                        }
                    }
                }
            }
        }
    }
}
