// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Economy
{
    public enum BlackMarketAttentionBand
    {
        Calm = 0,
        Raised = 1,
        Hot = 2,
        CriticalLockout = 3
    }

    [Serializable]
    public sealed class SyndicateHeatRecordSaveState
    {
        public string SyndicateId { get; set; } = string.Empty;
        public int CurrentHeat { get; set; }
        public int HeatThreshold { get; set; }
        public BlackMarketAttentionBand Band { get; set; }
        public int LastCoolingDay { get; set; }
        public int TotalHeatAccumulated { get; set; }
        public bool IsRelocating { get; set; }
        public int RelocationDaysRemaining { get; set; }
        public int RelocationCount { get; set; }
        public string CurrentLocationKey { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class BlackMarketHeatAttentionSaveState
    {
        public int schema_version { get; set; } = 1;
        public List<SyndicateHeatRecordSaveState> Syndicates { get; set; } = new();
    }

    public sealed class SyndicateHeatRecord
    {
        public string SyndicateId { get; }
        public int CurrentHeat { get; set; }
        public int HeatThreshold { get; set; }
        public BlackMarketAttentionBand Band { get; set; }
        public int LastCoolingDay { get; set; }
        public int TotalHeatAccumulated { get; set; }
        public bool IsRelocating { get; set; }
        public int RelocationDaysRemaining { get; set; }
        public int RelocationCount { get; set; }
        public string CurrentLocationKey { get; set; }

        public SyndicateHeatRecord(
            string syndicateId,
            int heatThreshold = 100,
            string initialLocation = "underground_hideout")
        {
            SyndicateId = syndicateId ?? throw new ArgumentNullException(nameof(syndicateId));
            CurrentHeat = 0;
            HeatThreshold = Math.Max(1, heatThreshold);
            Band = BlackMarketAttentionBand.Calm;
            LastCoolingDay = 0;
            TotalHeatAccumulated = 0;
            IsRelocating = false;
            RelocationDaysRemaining = 0;
            RelocationCount = 0;
            CurrentLocationKey = initialLocation ?? "underground_hideout";
        }

        internal SyndicateHeatRecord(SyndicateHeatRecordSaveState state)
        {
            SyndicateId = state.SyndicateId;
            CurrentHeat = state.CurrentHeat;
            HeatThreshold = state.HeatThreshold;
            Band = state.Band;
            LastCoolingDay = state.LastCoolingDay;
            TotalHeatAccumulated = state.TotalHeatAccumulated;
            IsRelocating = state.IsRelocating;
            RelocationDaysRemaining = state.RelocationDaysRemaining;
            RelocationCount = state.RelocationCount;
            CurrentLocationKey = state.CurrentLocationKey;
        }

        public SyndicateHeatRecordSaveState CaptureState()
        {
            return new SyndicateHeatRecordSaveState
            {
                SyndicateId = SyndicateId,
                CurrentHeat = CurrentHeat,
                HeatThreshold = HeatThreshold,
                Band = Band,
                LastCoolingDay = LastCoolingDay,
                TotalHeatAccumulated = TotalHeatAccumulated,
                IsRelocating = IsRelocating,
                RelocationDaysRemaining = RelocationDaysRemaining,
                RelocationCount = RelocationCount,
                CurrentLocationKey = CurrentLocationKey
            };
        }
    }

    /// <summary>
    /// XP-04-F4 / UNBLOCK-02 §5.4 / §5.10 / §660:
    /// Black Market Heat & Attention Band Engine.
    /// Coordinates heat deltas, attention bands (Calm, Raised, Hot, CriticalLockout),
    /// daily transaction cooling, patrol sweep risks, and deterministic relocation anti-reroll persistence.
    /// </summary>
    public sealed class BlackMarketHeatAttentionEngine
    {
        public const int DefaultDailyCooling = 5;
        public const int RelocationThresholdHeat = 150;
        public const int RelocationDurationDays = 7;

        private static readonly string[] PossibleLocations = new[]
        {
            "hideout_north_tunnel",
            "hideout_abandoned_subway",
            "hideout_flooded_cistern",
            "hideout_collapsed_vault",
            "hideout_sewer_junction",
            "hideout_ventilation_shaft"
        };

        private readonly Dictionary<string, SyndicateHeatRecord> _records = new(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyDictionary<string, SyndicateHeatRecord> Records => _records;

        public event Action<string, int, BlackMarketAttentionBand>? OnHeatUpdated;
        public event Action<string, string>? OnRelocationTriggered;
        public event Action<string>? OnRelocationCompleted;

        public SyndicateHeatRecord GetOrCreateRecord(
            string syndicateId,
            int heatThreshold = 100,
            string initialLocation = "underground_hideout")
        {
            if (string.IsNullOrWhiteSpace(syndicateId))
                throw new ArgumentException("SyndicateId cannot be empty.", nameof(syndicateId));

            if (!_records.TryGetValue(syndicateId, out var record))
            {
                record = new SyndicateHeatRecord(syndicateId, heatThreshold, initialLocation);
                _records[syndicateId] = record;
            }

            return record;
        }

        public BlackMarketAttentionBand EvaluateBand(int currentHeat, int threshold)
        {
            if (currentHeat >= RelocationThresholdHeat)
                return BlackMarketAttentionBand.CriticalLockout;
            if (currentHeat >= threshold)
                return BlackMarketAttentionBand.Hot;
            if (currentHeat >= threshold / 2)
                return BlackMarketAttentionBand.Raised;
            return BlackMarketAttentionBand.Calm;
        }

        public int AddHeat(string syndicateId, int delta, string reason, int currentDay)
        {
            if (delta <= 0) return 0;
            var record = GetOrCreateRecord(syndicateId);

            record.CurrentHeat += delta;
            record.TotalHeatAccumulated += delta;
            record.Band = EvaluateBand(record.CurrentHeat, record.HeatThreshold);
            if (record.LastCoolingDay == 0)
            {
                record.LastCoolingDay = currentDay;
            }

            OnHeatUpdated?.Invoke(syndicateId, record.CurrentHeat, record.Band);
            return record.CurrentHeat;
        }

        public void ProcessDailyTick(int currentDay, int campaignSeed = 0, int dailyCooling = DefaultDailyCooling)
        {
            foreach (var kvp in _records)
            {
                var record = kvp.Value;
                if (currentDay <= record.LastCoolingDay) continue;

                int daysElapsed = currentDay - record.LastCoolingDay;

                for (int d = 0; d < daysElapsed; d++)
                {
                    // Relocation countdown
                    if (record.IsRelocating)
                    {
                        record.RelocationDaysRemaining--;
                        if (record.RelocationDaysRemaining <= 0)
                        {
                            record.IsRelocating = false;
                            record.RelocationDaysRemaining = 0;
                            record.CurrentHeat = Math.Max(0, record.CurrentHeat - 50); // Heat drops significantly upon relocation
                            record.Band = EvaluateBand(record.CurrentHeat, record.HeatThreshold);
                            OnRelocationCompleted?.Invoke(record.SyndicateId);
                        }
                    }
                    else
                    {
                        // Daily cooling
                        record.CurrentHeat = Math.Max(0, record.CurrentHeat - dailyCooling);
                        record.Band = EvaluateBand(record.CurrentHeat, record.HeatThreshold);

                        // If heat hits critical threshold, trigger relocation
                        if (record.CurrentHeat >= RelocationThresholdHeat)
                        {
                            TriggerRelocation(record.SyndicateId, currentDay, campaignSeed);
                        }
                    }
                }

                record.LastCoolingDay = currentDay;
                OnHeatUpdated?.Invoke(record.SyndicateId, record.CurrentHeat, record.Band);
            }
        }

        public bool TriggerRelocation(string syndicateId, int currentDay, int campaignSeed)
        {
            var record = GetOrCreateRecord(syndicateId);
            if (record.IsRelocating) return false;

            record.IsRelocating = true;
            record.RelocationDaysRemaining = RelocationDurationDays;
            record.RelocationCount++;
            record.Band = BlackMarketAttentionBand.CriticalLockout;

            // Deterministic anti-reroll location selection. Do not use
            // HashCode/string.GetHashCode here: both vary across processes.
            int seed = StableHash.Combine(campaignSeed, record.RelocationCount);
            seed = StableHash.Combine(seed, record.SyndicateId);
            int locationIndex = StableHash.NonNegativeRemainder(seed, PossibleLocations.Length);
            record.CurrentLocationKey = PossibleLocations[locationIndex];

            OnRelocationTriggered?.Invoke(syndicateId, record.CurrentLocationKey);
            return true;
        }

        public int GetRaidRiskPermille(string syndicateId)
        {
            if (!_records.TryGetValue(syndicateId, out var record)) return 0;
            if (record.IsRelocating) return 0; // In transit / dispersed

            return record.Band switch
            {
                BlackMarketAttentionBand.CriticalLockout => 600, // 60%
                BlackMarketAttentionBand.Hot => 250,            // 25%
                BlackMarketAttentionBand.Raised => 50,          // 5%
                _ => 0
            };
        }

        public bool CheckRaidTrigger(string syndicateId, int rollPermille)
        {
            int risk = GetRaidRiskPermille(syndicateId);
            return risk > 0 && rollPermille >= 0 && rollPermille < risk;
        }

        public BlackMarketHeatAttentionSaveState CaptureState()
        {
            var save = new BlackMarketHeatAttentionSaveState
            {
                schema_version = 1,
                Syndicates = new List<SyndicateHeatRecordSaveState>(_records.Count)
            };

            foreach (var kvp in _records)
            {
                save.Syndicates.Add(kvp.Value.CaptureState());
            }

            return save;
        }

        public void RestoreState(BlackMarketHeatAttentionSaveState? state)
        {
            _records.Clear();
            if (state?.Syndicates == null) return;

            for (int i = 0; i < state.Syndicates.Count; i++)
            {
                var s = state.Syndicates[i];
                if (s != null && !string.IsNullOrWhiteSpace(s.SyndicateId))
                {
                    _records[s.SyndicateId] = new SyndicateHeatRecord(s);
                }
            }
        }
    }
}
