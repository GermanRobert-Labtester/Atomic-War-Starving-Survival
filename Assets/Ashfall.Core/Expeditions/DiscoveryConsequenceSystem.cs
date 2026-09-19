// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Expeditions
{
    public enum DiscoveryType
    {
        ResourceDeposit = 0,
        ThreatCleared = 1,
        RuinsUncovered = 2,
        FactionContact = 3,
        StrategicLocation = 4
    }

    public enum ConsequenceStatus
    {
        Discovered = 0,
        Concealed = 1,
        Exploited = 2,
        Escalated = 3
    }

    [Serializable]
    public sealed class DiscoveryRecord
    {
        public string DiscoveryId { get; set; } = string.Empty;
        public string LocationId { get; set; } = string.Empty;
        public DiscoveryType Type { get; set; } = DiscoveryType.ResourceDeposit;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DiscoveredDay { get; set; } = 1;
        public ConsequenceStatus Status { get; set; } = ConsequenceStatus.Discovered;
        public List<string> Tags { get; set; } = new List<string>();
        public string AssociatedFactionId { get; set; } = string.Empty;
        public string ResourceYield { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class ConsequenceOutcome
    {
        public string ConsequenceId { get; set; } = string.Empty;
        public string DiscoveryId { get; set; } = string.Empty;
        public int Day { get; set; } = 1;
        public string Description { get; set; } = string.Empty;
        public float CaravanSafetyBonus { get; set; }
        public float FactionStandingDelta { get; set; }
        public string FactionId { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class DiscoveryConsequenceState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<DiscoveryRecord> Discoveries { get; set; } = new List<DiscoveryRecord>();
        public List<ConsequenceOutcome> TriggeredConsequences { get; set; } = new List<ConsequenceOutcome>();
    }

    /// <summary>
    /// Plan 133 / C1[20] — Expedition Discovery Persistent World Consequences.
    /// Tracks persistent map, economic, caravan safety, and faction ramifications
    /// originating from expedition discoveries.
    /// </summary>
    public sealed class DiscoveryConsequenceSystem
    {
        private readonly DiscoveryConsequenceState _state;

        public event Action<DiscoveryRecord>? OnDiscoveryRegistered;
        public event Action<ConsequenceOutcome>? OnConsequenceTriggered;

        public int DiscoveryCount => _state.Discoveries.Count;
        public int ConsequenceCount => _state.TriggeredConsequences.Count;

        /// <summary>
        /// Registers the canonical expedition destination-discovery fact.
        /// ExpeditionSystem already owns whether a destination is known; this
        /// method only projects that fact into consequence provenance. The
        /// stable source ID makes the projection idempotent across repeated
        /// event delivery and save/restore.
        /// </summary>
        public DiscoveryRecord? RegisterExpeditionDiscovery(string locationId, int day)
        {
            if (string.IsNullOrWhiteSpace(locationId)) return null;

            string stableId = "expedition_discovery_" + locationId.Trim();
            var existing = _state.Discoveries.FirstOrDefault(d =>
                string.Equals(d.DiscoveryId, stableId, StringComparison.Ordinal));
            if (existing != null) return existing;

            var record = new DiscoveryRecord
            {
                DiscoveryId = stableId,
                LocationId = locationId.Trim(),
                Type = DiscoveryType.StrategicLocation,
                Title = "Destination identified",
                Description = $"Expedition intelligence established a route to {locationId.Trim()}.",
                DiscoveredDay = Math.Max(1, day),
                Status = ConsequenceStatus.Discovered
            };

            _state.Discoveries.Add(record);
            OnDiscoveryRegistered?.Invoke(record);
            TriggerOutcome(record.DiscoveryId, record.DiscoveredDay,
                $"Route intelligence recorded for {record.LocationId}.");
            return record;
        }

        public DiscoveryConsequenceSystem(DiscoveryConsequenceState? state = null)
        {
            _state = state ?? new DiscoveryConsequenceState();
        }

        public DiscoveryRecord RegisterDiscovery(
            string locationId,
            DiscoveryType type,
            string title,
            string description,
            int day,
            IEnumerable<string>? tags = null,
            string associatedFactionId = "",
            string resourceYield = "")
        {
            if (string.IsNullOrEmpty(locationId)) throw new ArgumentNullException(nameof(locationId));

            var record = new DiscoveryRecord
            {
                DiscoveryId = $"disc_{locationId}_{_state.NextSequence++}",
                LocationId = locationId,
                Type = type,
                Title = title ?? string.Empty,
                Description = description ?? string.Empty,
                DiscoveredDay = Math.Max(1, day),
                Status = ConsequenceStatus.Discovered,
                Tags = tags != null ? new List<string>(tags) : new List<string>(),
                AssociatedFactionId = associatedFactionId ?? string.Empty,
                ResourceYield = resourceYield ?? string.Empty
            };

            _state.Discoveries.Add(record);
            OnDiscoveryRegistered?.Invoke(record);

            // Auto-trigger initial consequence based on type
            if (type == DiscoveryType.ThreatCleared)
            {
                TriggerOutcome(record.DiscoveryId, day, $"Route through {locationId} secured; caravan passage risk reduced.", safetyBonus: 0.15f);
            }

            return record;
        }

        public bool ConcealDiscovery(string discoveryId)
        {
            var record = _state.Discoveries.FirstOrDefault(d => string.Equals(d.DiscoveryId, discoveryId, StringComparison.OrdinalIgnoreCase));
            if (record == null) return false;

            record.Status = ConsequenceStatus.Concealed;
            return true;
        }

        public ConsequenceOutcome? ExploitDiscovery(string discoveryId, int day)
        {
            var record = _state.Discoveries.FirstOrDefault(d => string.Equals(d.DiscoveryId, discoveryId, StringComparison.OrdinalIgnoreCase));
            if (record == null) return null;

            record.Status = ConsequenceStatus.Exploited;

            float standing = 0f;
            string desc;

            if (record.Type == DiscoveryType.ResourceDeposit)
            {
                desc = $"Exploited {record.ResourceYield} at {record.LocationId}. Factions alerted to extraction.";
                standing = string.IsNullOrEmpty(record.AssociatedFactionId) ? 0f : 5f;
            }
            else if (record.Type == DiscoveryType.RuinsUncovered)
            {
                desc = $"Pre-war artifacts recovered from {record.LocationId}. Scavenger traffic increased.";
            }
            else
            {
                desc = $"Claimed operational footprint at {record.LocationId}.";
            }

            return TriggerOutcome(record.DiscoveryId, day, desc, factionStandingDelta: standing, factionId: record.AssociatedFactionId);
        }

        public ConsequenceOutcome? EscalateDiscovery(string discoveryId, int day)
        {
            var record = _state.Discoveries.FirstOrDefault(d => string.Equals(d.DiscoveryId, discoveryId, StringComparison.OrdinalIgnoreCase));
            if (record == null) return null;

            record.Status = ConsequenceStatus.Escalated;
            string desc = $"Unattended discovery at {record.LocationId} attracted local interest and opportunists.";

            return TriggerOutcome(record.DiscoveryId, day, desc);
        }

        public float GetTotalCaravanSafetyBonus()
        {
            float bonus = 0f;
            for (int i = 0; i < _state.TriggeredConsequences.Count; i++)
            {
                bonus += _state.TriggeredConsequences[i].CaravanSafetyBonus;
            }
            return Math.Clamp(bonus, 0f, 0.50f);
        }

        public IReadOnlyList<DiscoveryRecord> GetDiscoveriesAt(string locationId)
        {
            if (string.IsNullOrEmpty(locationId)) return Array.Empty<DiscoveryRecord>();

            return _state.Discoveries
                .Where(d => string.Equals(d.LocationId, locationId, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private ConsequenceOutcome TriggerOutcome(
            string discoveryId,
            int day,
            string description,
            float safetyBonus = 0f,
            float factionStandingDelta = 0f,
            string factionId = "")
        {
            var outcome = new ConsequenceOutcome
            {
                ConsequenceId = $"csq_{_state.NextSequence++}",
                DiscoveryId = discoveryId,
                Day = Math.Max(1, day),
                Description = description,
                CaravanSafetyBonus = safetyBonus,
                FactionStandingDelta = factionStandingDelta,
                FactionId = factionId
            };

            _state.TriggeredConsequences.Add(outcome);
            OnConsequenceTriggered?.Invoke(outcome);
            return outcome;
        }

        public DiscoveryConsequenceState CaptureState()
        {
            var captured = new DiscoveryConsequenceState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                Discoveries = new List<DiscoveryRecord>(_state.Discoveries.Count),
                TriggeredConsequences = new List<ConsequenceOutcome>(_state.TriggeredConsequences.Count)
            };

            for (int i = 0; i < _state.Discoveries.Count; i++)
            {
                var d = _state.Discoveries[i];
                captured.Discoveries.Add(new DiscoveryRecord
                {
                    DiscoveryId = d.DiscoveryId,
                    LocationId = d.LocationId,
                    Type = d.Type,
                    Title = d.Title,
                    Description = d.Description,
                    DiscoveredDay = d.DiscoveredDay,
                    Status = d.Status,
                    Tags = new List<string>(d.Tags),
                    AssociatedFactionId = d.AssociatedFactionId,
                    ResourceYield = d.ResourceYield
                });
            }

            for (int i = 0; i < _state.TriggeredConsequences.Count; i++)
            {
                var c = _state.TriggeredConsequences[i];
                captured.TriggeredConsequences.Add(new ConsequenceOutcome
                {
                    ConsequenceId = c.ConsequenceId,
                    DiscoveryId = c.DiscoveryId,
                    Day = c.Day,
                    Description = c.Description,
                    CaravanSafetyBonus = c.CaravanSafetyBonus,
                    FactionStandingDelta = c.FactionStandingDelta,
                    FactionId = c.FactionId
                });
            }

            return captured;
        }

        public void RestoreState(DiscoveryConsequenceState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.Discoveries.Clear();
            _state.TriggeredConsequences.Clear();

            if (state.Discoveries != null)
            {
                for (int i = 0; i < state.Discoveries.Count; i++)
                {
                    var d = state.Discoveries[i];
                    _state.Discoveries.Add(new DiscoveryRecord
                    {
                        DiscoveryId = d.DiscoveryId,
                        LocationId = d.LocationId,
                        Type = d.Type,
                        Title = d.Title,
                        Description = d.Description,
                        DiscoveredDay = d.DiscoveredDay,
                        Status = d.Status,
                        Tags = new List<string>(d.Tags ?? Enumerable.Empty<string>()),
                        AssociatedFactionId = d.AssociatedFactionId,
                        ResourceYield = d.ResourceYield
                    });
                }
            }

            if (state.TriggeredConsequences != null)
            {
                for (int i = 0; i < state.TriggeredConsequences.Count; i++)
                {
                    var c = state.TriggeredConsequences[i];
                    _state.TriggeredConsequences.Add(new ConsequenceOutcome
                    {
                        ConsequenceId = c.ConsequenceId,
                        DiscoveryId = c.DiscoveryId,
                        Day = c.Day,
                        Description = c.Description,
                        CaravanSafetyBonus = c.CaravanSafetyBonus,
                        FactionStandingDelta = c.FactionStandingDelta,
                        FactionId = c.FactionId
                    });
                }
            }
        }
    }
}
