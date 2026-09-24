// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 187 — Bestiary & Creature Encounter Tracking UI
// Pure domain authority for creature discovery progression, encounter tracking,
// kill counts, sighting logs, and multi-tier lore unlocks.
// Integrates with WastelandBestiaryCatalog for static fauna reference data.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Ashfall.Core.Narrative;

namespace Ashfall.Core.Bestiary
{
    // ── Persistent State DTOs ───────────────────────────────────────────────

    [Serializable]
    public sealed class CreatureDiscoveryRecord
    {
        public string CreatureId { get; set; } = string.Empty;
        public int DiscoveredDay { get; set; } = 1;
        public int EncounterCount { get; set; }
        public int KillCount { get; set; }
        public int ButcherCount { get; set; }
        public string FirstLocationId { get; set; } = string.Empty;
        public int LastEncounterDay { get; set; } = 1;
        public List<string> UnlockedNoteKeys { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class CreatureSightingRecord
    {
        public string SightingId { get; set; } = string.Empty;
        public string CreatureId { get; set; } = string.Empty;
        public int Day { get; set; }
        public string LocationId { get; set; } = string.Empty;
        public string WitnessSurvivorId { get; set; } = string.Empty;
        public string SightingType { get; set; } = "spotted"; // spotted, attacked, fled, tracks
    }

    [Serializable]
    public sealed class BestiaryState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<CreatureDiscoveryRecord> Discoveries { get; set; } = new List<CreatureDiscoveryRecord>();
        public List<CreatureSightingRecord> Sightings { get; set; } = new List<CreatureSightingRecord>();
    }

    // ── Domain System ───────────────────────────────────────────────────────

    public sealed class BestiarySystem
    {
        public const int TotalCanonicalFaunaCount = 24;

        private readonly BestiaryState _state;
        private readonly WastelandBestiaryCatalog _catalog = new WastelandBestiaryCatalog();

        public event Action<string>? OnCreatureDiscovered;
        public event Action<string, int>? OnCreatureEncountered; // (creatureId, count)
        public event Action<string, int>? OnCreatureKilled;      // (creatureId, count)
        public event Action<string, string>? OnNoteUnlocked;     // (creatureId, noteKey)

        public int DiscoveredCount => _state.Discoveries.Count;
        public WastelandBestiaryCatalog Catalog => _catalog;

        public BestiarySystem()
        {
            _state = new BestiaryState();
        }

        public BestiarySystem(BestiaryState state)
        {
            _state = state ?? new BestiaryState();
        }

        // ── Catalog Loading ────────────────────────────────────────────────

        public void LoadCatalog(string json, IJsonSerializer? serializer = null)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            serializer ??= new SystemTextJsonSerializer();
            _catalog.Load(json, serializer);
        }

        // ── Encounter & Sighting Tracking ──────────────────────────────────

        public CreatureDiscoveryRecord RecordEncounter(string creatureId, int day, string locationId = "", string witnessId = "")
        {
            if (string.IsNullOrWhiteSpace(creatureId)) throw new ArgumentException("creatureId is required");

            var record = _state.Discoveries.FirstOrDefault(d =>
                string.Equals(d.CreatureId, creatureId, StringComparison.OrdinalIgnoreCase));

            bool isNewDiscovery = false;
            if (record == null)
            {
                isNewDiscovery = true;
                record = new CreatureDiscoveryRecord
                {
                    CreatureId = creatureId,
                    DiscoveredDay = day,
                    FirstLocationId = locationId,
                    LastEncounterDay = day
                };
                _state.Discoveries.Add(record);
            }

            record.EncounterCount++;
            record.LastEncounterDay = day;

            // Log sighting record (keep last 100)
            _state.Sightings.Add(new CreatureSightingRecord
            {
                SightingId = $"sight_{_state.NextSequence++}",
                CreatureId = creatureId,
                Day = day,
                LocationId = locationId,
                WitnessSurvivorId = witnessId,
                SightingType = "spotted"
            });
            if (_state.Sightings.Count > 100)
                _state.Sightings.RemoveAt(0);

            if (isNewDiscovery)
                OnCreatureDiscovered?.Invoke(creatureId);

            OnCreatureEncountered?.Invoke(creatureId, record.EncounterCount);

            // Tier unlocks based on encounters
            CheckNoteUnlocks(record);

            return record;
        }

        public void RecordKill(string creatureId, int day, string locationId = "")
        {
            var record = RecordEncounter(creatureId, day, locationId);
            record.KillCount++;
            OnCreatureKilled?.Invoke(creatureId, record.KillCount);
            CheckNoteUnlocks(record);
        }

        public void RecordButcher(string creatureId, int day)
        {
            var record = _state.Discoveries.FirstOrDefault(d =>
                string.Equals(d.CreatureId, creatureId, StringComparison.OrdinalIgnoreCase));
            if (record != null)
            {
                record.ButcherCount++;
                CheckNoteUnlocks(record);
            }
        }

        private void CheckNoteUnlocks(CreatureDiscoveryRecord record)
        {
            void TryUnlock(string key)
            {
                if (!record.UnlockedNoteKeys.Contains(key))
                {
                    record.UnlockedNoteKeys.Add(key);
                    OnNoteUnlocked?.Invoke(record.CreatureId, key);
                }
            }

            // 1 encounter: Discovery
            TryUnlock("discovery");

            // 3 encounters: Basic Stats & Threat Level
            if (record.EncounterCount >= 3)
                TryUnlock("basic_stats");

            // 5 encounters: Behavior and Habitat
            if (record.EncounterCount >= 5)
                TryUnlock("behavior");

            // 10 encounters or 1 kill: Combat & Weakness
            if (record.EncounterCount >= 10 || record.KillCount >= 1)
                TryUnlock("combat_tactics");

            // 1 butcher: Harvestable Resources
            if (record.ButcherCount >= 1)
                TryUnlock("harvest_yields");
        }

        // ── Status & Completion Queries ────────────────────────────────────

        public CreatureDiscoveryRecord? GetDiscovery(string creatureId)
        {
            return _state.Discoveries.FirstOrDefault(d =>
                string.Equals(d.CreatureId, creatureId, StringComparison.OrdinalIgnoreCase));
        }

        public IReadOnlyList<CreatureDiscoveryRecord> GetAllDiscoveries() => _state.Discoveries;

        public IReadOnlyList<CreatureSightingRecord> GetRecentSightings(int limit = 20)
        {
            return _state.Sightings.TakeLast(limit).Reverse().ToList();
        }

        public float GetCompletionPercentage()
        {
            int total = _catalog.AllCreatures.Count > 0 ? _catalog.AllCreatures.Count : TotalCanonicalFaunaCount;
            return Math.Clamp((float)DiscoveredCount / total * 100.0f, 0.0f, 100.0f);
        }

        public bool IsBasicStatsUnlocked(string creatureId)
        {
            var d = GetDiscovery(creatureId);
            return d != null && d.UnlockedNoteKeys.Contains("basic_stats");
        }

        public bool IsBehaviorUnlocked(string creatureId)
        {
            var d = GetDiscovery(creatureId);
            return d != null && d.UnlockedNoteKeys.Contains("behavior");
        }

        public bool IsCombatTacticsUnlocked(string creatureId)
        {
            var d = GetDiscovery(creatureId);
            return d != null && d.UnlockedNoteKeys.Contains("combat_tactics");
        }

        // ── Save / Restore ─────────────────────────────────────────────────

        public BestiaryState CaptureState()
        {
            return new BestiaryState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                Discoveries = _state.Discoveries.Select(d => new CreatureDiscoveryRecord
                {
                    CreatureId = d.CreatureId,
                    DiscoveredDay = d.DiscoveredDay,
                    EncounterCount = d.EncounterCount,
                    KillCount = d.KillCount,
                    ButcherCount = d.ButcherCount,
                    FirstLocationId = d.FirstLocationId,
                    LastEncounterDay = d.LastEncounterDay,
                    UnlockedNoteKeys = new List<string>(d.UnlockedNoteKeys)
                }).ToList(),
                Sightings = _state.Sightings.Select(s => new CreatureSightingRecord
                {
                    SightingId = s.SightingId,
                    CreatureId = s.CreatureId,
                    Day = s.Day,
                    LocationId = s.LocationId,
                    WitnessSurvivorId = s.WitnessSurvivorId,
                    SightingType = s.SightingType
                }).ToList()
            };
        }

        public void RestoreState(BestiaryState? saved)
        {
            if (saved == null) return;
            _state.SchemaVersion = saved.SchemaVersion;
            _state.NextSequence = saved.NextSequence > 0 ? saved.NextSequence : 1;
            _state.Discoveries = saved.Discoveries?.Select(d => new CreatureDiscoveryRecord
            {
                CreatureId = d.CreatureId,
                DiscoveredDay = d.DiscoveredDay,
                EncounterCount = d.EncounterCount,
                KillCount = d.KillCount,
                ButcherCount = d.ButcherCount,
                FirstLocationId = d.FirstLocationId,
                LastEncounterDay = d.LastEncounterDay,
                UnlockedNoteKeys = new List<string>(d.UnlockedNoteKeys ?? new List<string>())
            }).ToList() ?? new List<CreatureDiscoveryRecord>();
            _state.Sightings = saved.Sightings?.Select(s => new CreatureSightingRecord
            {
                SightingId = s.SightingId,
                CreatureId = s.CreatureId,
                Day = s.Day,
                LocationId = s.LocationId,
                WitnessSurvivorId = s.WitnessSurvivorId,
                SightingType = s.SightingType
            }).ToList() ?? new List<CreatureSightingRecord>();
        }

        public BestiaryCensus GetCensus()
        {
            int kills = 0, butchered = 0;
            for (int i = 0; i < _state.Discoveries.Count; i++)
            {
                kills += _state.Discoveries[i].KillCount;
                butchered += _state.Discoveries[i].ButcherCount;
            }
            int total = _catalog.AllCreatures.Count > 0 ? _catalog.AllCreatures.Count : TotalCanonicalFaunaCount;
            return new BestiaryCensus(DiscoveredCount, total, GetCompletionPercentage(), _state.Sightings.Count, kills, butchered);
        }
    }

    public struct BestiaryCensus
    {
        public readonly int TotalDiscovered;
        public readonly int TotalCanonicalFauna;
        public readonly float CompletionPercentage;
        public readonly int TotalSightings;
        public readonly int TotalKills;
        public readonly int TotalButchered;

        public BestiaryCensus(int totalDiscovered, int totalCanonical, float completion, int sightings, int kills, int butchered)
        {
            TotalDiscovered = totalDiscovered;
            TotalCanonicalFauna = totalCanonical;
            CompletionPercentage = completion;
            TotalSightings = sightings;
            TotalKills = kills;
            TotalButchered = butchered;
        }
    }
}
