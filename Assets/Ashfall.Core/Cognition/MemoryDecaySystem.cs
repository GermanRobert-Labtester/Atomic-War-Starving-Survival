// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Cognition
{
    public enum MemoryDomain
    {
        Skill = 0,
        Knowledge = 1,
        EventMemory = 2,
        Relationship = 3,
        Procedural = 4
    }

    public enum MemoryClarity
    {
        Forgotten = 0,
        Fragmentary = 1,
        Vague = 2,
        Clear = 3,
        Vivid = 4
    }

    public enum ReinforcementType
    {
        Practice = 0,
        Review = 1,
        Reminder = 2,
        Experience = 3
    }

    [Serializable]
    public sealed class MemoryRecord
    {
        public string RecordId { get; set; } = string.Empty;
        public string SurvivorId { get; set; } = string.Empty;
        public MemoryDomain Domain { get; set; } = MemoryDomain.Skill;
        public string ReferenceId { get; set; } = string.Empty;
        public float Strength { get; set; } = 100f;
        public MemoryClarity Clarity { get; set; } = MemoryClarity.Vivid;
        public int LastReinforcedDay { get; set; } = 1;
        public bool IsCertified { get; set; }
        public bool IsPreserved { get; set; }
    }

    [Serializable]
    public sealed class ForgettingEvent
    {
        public string EventId { get; set; } = string.Empty;
        public string SurvivorId { get; set; } = string.Empty;
        public string RecordId { get; set; } = string.Empty;
        public MemoryDomain Domain { get; set; }
        public int Day { get; set; }
        public float StrengthLost { get; set; }
        public MemoryClarity NewClarity { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class ReinforcementRecord
    {
        public string ReinforcementId { get; set; } = string.Empty;
        public string SurvivorId { get; set; } = string.Empty;
        public string RecordId { get; set; } = string.Empty;
        public MemoryDomain Domain { get; set; }
        public int Day { get; set; }
        public ReinforcementType Type { get; set; }
        public float StrengthRestored { get; set; }
    }

    [Serializable]
    public sealed class MemoryDecayState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public float GlobalDecayModifier { get; set; } = 1.0f;
        public List<MemoryRecord> Records { get; set; } = new List<MemoryRecord>();
        public List<ForgettingEvent> RecentForgettingEvents { get; set; } = new List<ForgettingEvent>();
        public List<ReinforcementRecord> RecentReinforcements { get; set; } = new List<ReinforcementRecord>();
    }

    [Serializable]
    public sealed class DomainDecayRateDef
    {
        public string domain { get; set; } = string.Empty;
        public float base_daily_decay_rate { get; set; } = 1.0f;
        public float reinforcement_multiplier { get; set; } = 1.0f;
        public float certified_decay_scale { get; set; } = 0.1f;
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class MemoryDecayCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<DomainDecayRateDef> domain_rates { get; set; } = new List<DomainDecayRateDef>();
    }

    /// <summary>
    /// A detached fact supplied by an existing canonical owner (for example
    /// skill progression or the journal). Memory decay does not persist or
    /// mutate these facts; it only projects their current clarity.
    /// </summary>
    [Serializable]
    public sealed class CanonicalMemoryFact
    {
        public string SurvivorId { get; set; } = string.Empty;
        public MemoryDomain Domain { get; set; } = MemoryDomain.Skill;
        public string SourceId { get; set; } = string.Empty;
        public float Strength { get; set; } = 100f;
        public int LastReinforcedDay { get; set; } = 1;
        public bool IsCertified { get; set; }
        public bool IsPreserved { get; set; }
    }

    /// <summary>
    /// Plan 185 / C1[33] — Memory & Skill Decay System.
    /// Manages gradual, deterministic degradation and reinforcement across cognitive,
    /// skill, factual, and experiential domains without duplicating master authorities.
    /// </summary>
    public sealed class MemoryDecaySystem
    {
        private readonly MemoryDecayState _state;
        private readonly List<DomainDecayRateDef> _domainRates = new List<DomainDecayRateDef>();

        public event Action<MemoryRecord, ReinforcementRecord>? OnMemoryReinforced;
        public event Action<MemoryRecord, ForgettingEvent>? OnMemoryFaded;
        public event Action<MemoryRecord, ForgettingEvent>? OnMemoryForgotten;

        public int RecordCount => _state.Records.Count;
        public float GlobalDecayModifier
        {
            get => _state.GlobalDecayModifier;
            set => _state.GlobalDecayModifier = Math.Max(0f, value);
        }

        public MemoryDecaySystem(MemoryDecayState? state = null)
        {
            _state = state ?? new MemoryDecayState();
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var catalog = JsonSerializer.Deserialize<MemoryDecayCatalog>(json, options);
                if (catalog?.domain_rates == null) return;

                _domainRates.Clear();
                foreach (var r in catalog.domain_rates)
                {
                    if (!string.IsNullOrWhiteSpace(r.domain))
                        _domainRates.Add(r);
                }
            }
            catch (Exception) { /* malformed catalog falls back to built-in defaults; authoring errors are enforced by the data-integrity gate */ }
        }

        public IReadOnlyList<DomainDecayRateDef> GetAllDomainRates() => _domainRates;

        public DomainDecayRateDef? GetDomainRate(MemoryDomain domain)
        {
            string name = domain.ToString();
            return _domainRates.FirstOrDefault(r => string.Equals(r.domain, name, StringComparison.OrdinalIgnoreCase));
        }

        public static MemoryClarity ResolveClarity(float strength)
        {
            if (strength <= 0f) return MemoryClarity.Forgotten;
            if (strength < 25f) return MemoryClarity.Fragmentary;
            if (strength < 50f) return MemoryClarity.Vague;
            if (strength < 75f) return MemoryClarity.Clear;
            return MemoryClarity.Vivid;
        }

        public static float GetBaseDailyDecayRate(MemoryDomain domain)
        {
            return domain switch
            {
                MemoryDomain.Skill => 1.0f,
                MemoryDomain.Knowledge => 2.5f,
                MemoryDomain.EventMemory => 4.0f,
                MemoryDomain.Relationship => 1.5f,
                MemoryDomain.Procedural => 0.5f,
                _ => 1.5f
            };
        }

        /// <summary>
        /// Builds a deterministic, read-only projection over canonical facts.
        /// The returned records are detached read models: callers must keep
        /// source ownership, mutation, and persistence in the originating
        /// system rather than treating this projection as a second ledger.
        /// </summary>
        public static IReadOnlyList<MemoryRecord> ProjectCanonicalSources(
            IEnumerable<CanonicalMemoryFact>? facts,
            int currentDay,
            float globalDecayModifier = 1f)
        {
            var projected = new List<MemoryRecord>();
            if (facts == null) return projected;

            float modifier = Math.Max(0f, globalDecayModifier);
            foreach (var fact in facts)
            {
                if (fact == null || string.IsNullOrEmpty(fact.SurvivorId) || string.IsNullOrEmpty(fact.SourceId))
                    continue;

                float strength = Math.Clamp(fact.Strength, 0f, 100f);
                int lastDay = Math.Max(1, fact.LastReinforcedDay);
                int daysUnreinforced = Math.Max(0, currentDay - lastDay);
                if (!fact.IsPreserved && strength > 0f && modifier > 0f)
                {
                    float dailyRate = GetBaseDailyDecayRate(fact.Domain) * modifier;
                    if (fact.IsCertified) dailyRate *= 0.1f;
                    strength = Math.Max(0f, strength - daysUnreinforced * dailyRate);
                }

                projected.Add(new MemoryRecord
                {
                    RecordId = fact.SourceId,
                    SurvivorId = fact.SurvivorId,
                    Domain = fact.Domain,
                    ReferenceId = fact.SourceId,
                    Strength = strength,
                    Clarity = ResolveClarity(strength),
                    LastReinforcedDay = lastDay,
                    IsCertified = fact.IsCertified,
                    IsPreserved = fact.IsPreserved
                });
            }

            return projected
                .OrderBy(r => r.SurvivorId, StringComparer.Ordinal)
                .ThenBy(r => r.Domain)
                .ThenBy(r => r.ReferenceId, StringComparer.Ordinal)
                .ToList();
        }

        public MemoryRecord RegisterOrUpdate(
            string survivorId,
            MemoryDomain domain,
            string referenceId,
            int day,
            float initialStrength = 100f,
            bool isCertified = false,
            bool isPreserved = false)
        {
            if (string.IsNullOrEmpty(survivorId)) throw new ArgumentNullException(nameof(survivorId));
            if (string.IsNullOrEmpty(referenceId)) throw new ArgumentNullException(nameof(referenceId));

            var record = _state.Records.FirstOrDefault(r =>
                string.Equals(r.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase) &&
                r.Domain == domain &&
                string.Equals(r.ReferenceId, referenceId, StringComparison.OrdinalIgnoreCase));

            if (record == null)
            {
                record = new MemoryRecord
                {
                    RecordId = $"mem_{_state.NextSequence++}",
                    SurvivorId = survivorId,
                    Domain = domain,
                    ReferenceId = referenceId,
                    Strength = Math.Clamp(initialStrength, 0f, 100f),
                    Clarity = ResolveClarity(initialStrength),
                    LastReinforcedDay = Math.Max(1, day),
                    IsCertified = isCertified,
                    IsPreserved = isPreserved
                };
                _state.Records.Add(record);
            }
            else
            {
                record.Strength = Math.Clamp(initialStrength, 0f, 100f);
                record.Clarity = ResolveClarity(record.Strength);
                record.LastReinforcedDay = Math.Max(record.LastReinforcedDay, day);
                if (isCertified) record.IsCertified = true;
                if (isPreserved) record.IsPreserved = true;
            }

            return record;
        }

        public bool Reinforce(
            string survivorId,
            MemoryDomain domain,
            string referenceId,
            int day,
            ReinforcementType type,
            float boostAmount = 25f)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(referenceId)) return false;

            var record = _state.Records.FirstOrDefault(r =>
                string.Equals(r.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase) &&
                r.Domain == domain &&
                string.Equals(r.ReferenceId, referenceId, StringComparison.OrdinalIgnoreCase));

            if (record == null) return false;

            float effectiveBoost = boostAmount * (type switch
            {
                ReinforcementType.Practice => 1.2f,
                ReinforcementType.Review => 1.0f,
                ReinforcementType.Reminder => 0.8f,
                ReinforcementType.Experience => 1.5f,
                _ => 1.0f
            });

            record.Strength = Math.Clamp(record.Strength + effectiveBoost, 0f, 100f);
            record.Clarity = ResolveClarity(record.Strength);
            record.LastReinforcedDay = Math.Max(record.LastReinforcedDay, day);

            var reinforcement = new ReinforcementRecord
            {
                ReinforcementId = $"reinf_{_state.NextSequence++}",
                SurvivorId = survivorId,
                RecordId = record.RecordId,
                Domain = domain,
                Day = day,
                Type = type,
                StrengthRestored = effectiveBoost
            };

            _state.RecentReinforcements.Add(reinforcement);
            if (_state.RecentReinforcements.Count > 100)
            {
                _state.RecentReinforcements.RemoveAt(0);
            }

            OnMemoryReinforced?.Invoke(record, reinforcement);
            return true;
        }

        public void TickDay(int currentDay, Func<bool>? haltAllDecay = null)
        {
            if (haltAllDecay != null && haltAllDecay()) return;
            if (_state.GlobalDecayModifier <= 0f) return;

            for (int i = 0; i < _state.Records.Count; i++)
            {
                var record = _state.Records[i];
                if (record.IsPreserved || record.Strength <= 0f) continue;

                int daysUnreinforced = currentDay - record.LastReinforcedDay;
                if (daysUnreinforced <= 0) continue;

                float dailyRate = GetBaseDailyDecayRate(record.Domain) * _state.GlobalDecayModifier;
                if (record.IsCertified)
                {
                    dailyRate *= 0.1f; // Certified skills are 90% decay-resistant (Plan 180)
                }

                float oldStrength = record.Strength;
                MemoryClarity oldClarity = record.Clarity;

                record.Strength = Math.Max(0f, record.Strength - dailyRate);
                record.Clarity = ResolveClarity(record.Strength);

                if (record.Clarity != oldClarity)
                {
                    var ev = new ForgettingEvent
                    {
                        EventId = $"fg_{_state.NextSequence++}",
                        SurvivorId = record.SurvivorId,
                        RecordId = record.RecordId,
                        Domain = record.Domain,
                        Day = currentDay,
                        StrengthLost = oldStrength - record.Strength,
                        NewClarity = record.Clarity,
                        Description = $"{record.Domain} {record.ReferenceId} clarity degraded to {record.Clarity}."
                    };

                    _state.RecentForgettingEvents.Add(ev);
                    if (_state.RecentForgettingEvents.Count > 100)
                    {
                        _state.RecentForgettingEvents.RemoveAt(0);
                    }

                    if (record.Clarity == MemoryClarity.Forgotten)
                    {
                        OnMemoryForgotten?.Invoke(record, ev);
                    }
                    else
                    {
                        OnMemoryFaded?.Invoke(record, ev);
                    }
                }
            }
        }

        public MemoryRecord? GetMemory(string survivorId, MemoryDomain domain, string referenceId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(referenceId)) return null;

            return _state.Records.FirstOrDefault(r =>
                string.Equals(r.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase) &&
                r.Domain == domain &&
                string.Equals(r.ReferenceId, referenceId, StringComparison.OrdinalIgnoreCase));
        }

        public IReadOnlyList<MemoryRecord> GetSurvivorMemories(string survivorId, MemoryDomain? domainFilter = null)
        {
            if (string.IsNullOrEmpty(survivorId)) return Array.Empty<MemoryRecord>();

            var query = _state.Records.Where(r => string.Equals(r.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));
            if (domainFilter.HasValue)
            {
                query = query.Where(r => r.Domain == domainFilter.Value);
            }

            return query.ToList();
        }

        public IReadOnlyList<MemoryRecord> GetFadedMemories(string survivorId, MemoryClarity maxClarity = MemoryClarity.Vague)
        {
            if (string.IsNullOrEmpty(survivorId)) return Array.Empty<MemoryRecord>();

            return _state.Records
                .Where(r => string.Equals(r.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase) && r.Clarity <= maxClarity)
                .ToList();
        }

        public MemoryDecayState CaptureState()
        {
            var captured = new MemoryDecayState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                GlobalDecayModifier = _state.GlobalDecayModifier,
                Records = new List<MemoryRecord>(_state.Records.Count),
                RecentForgettingEvents = new List<ForgettingEvent>(_state.RecentForgettingEvents.Count),
                RecentReinforcements = new List<ReinforcementRecord>(_state.RecentReinforcements.Count)
            };

            for (int i = 0; i < _state.Records.Count; i++)
            {
                var r = _state.Records[i];
                captured.Records.Add(new MemoryRecord
                {
                    RecordId = r.RecordId,
                    SurvivorId = r.SurvivorId,
                    Domain = r.Domain,
                    ReferenceId = r.ReferenceId,
                    Strength = r.Strength,
                    Clarity = r.Clarity,
                    LastReinforcedDay = r.LastReinforcedDay,
                    IsCertified = r.IsCertified,
                    IsPreserved = r.IsPreserved
                });
            }

            for (int i = 0; i < _state.RecentForgettingEvents.Count; i++)
            {
                var e = _state.RecentForgettingEvents[i];
                captured.RecentForgettingEvents.Add(new ForgettingEvent
                {
                    EventId = e.EventId,
                    SurvivorId = e.SurvivorId,
                    RecordId = e.RecordId,
                    Domain = e.Domain,
                    Day = e.Day,
                    StrengthLost = e.StrengthLost,
                    NewClarity = e.NewClarity,
                    Description = e.Description
                });
            }

            for (int i = 0; i < _state.RecentReinforcements.Count; i++)
            {
                var rf = _state.RecentReinforcements[i];
                captured.RecentReinforcements.Add(new ReinforcementRecord
                {
                    ReinforcementId = rf.ReinforcementId,
                    SurvivorId = rf.SurvivorId,
                    RecordId = rf.RecordId,
                    Domain = rf.Domain,
                    Day = rf.Day,
                    Type = rf.Type,
                    StrengthRestored = rf.StrengthRestored
                });
            }

            return captured;
        }

        public void RestoreState(MemoryDecayState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.GlobalDecayModifier = state.GlobalDecayModifier;
            _state.Records.Clear();
            _state.RecentForgettingEvents.Clear();
            _state.RecentReinforcements.Clear();

            if (state.Records != null)
            {
                for (int i = 0; i < state.Records.Count; i++)
                {
                    var r = state.Records[i];
                    _state.Records.Add(new MemoryRecord
                    {
                        RecordId = r.RecordId,
                        SurvivorId = r.SurvivorId,
                        Domain = r.Domain,
                        ReferenceId = r.ReferenceId,
                        Strength = r.Strength,
                        Clarity = r.Clarity,
                        LastReinforcedDay = r.LastReinforcedDay,
                        IsCertified = r.IsCertified,
                        IsPreserved = r.IsPreserved
                    });
                }
            }

            if (state.RecentForgettingEvents != null)
            {
                _state.RecentForgettingEvents.AddRange(state.RecentForgettingEvents);
            }

            if (state.RecentReinforcements != null)
            {
                _state.RecentReinforcements.AddRange(state.RecentReinforcements);
            }
        }

        public MemoryDecayCensus GetCensus()
        {
            int vivid = 0, clear = 0, vague = 0, fragmentary = 0, forgotten = 0;
            int certified = 0, preserved = 0;
            for (int i = 0; i < _state.Records.Count; i++)
            {
                var r = _state.Records[i];
                switch (r.Clarity)
                {
                    case MemoryClarity.Vivid: vivid++; break;
                    case MemoryClarity.Clear: clear++; break;
                    case MemoryClarity.Vague: vague++; break;
                    case MemoryClarity.Fragmentary: fragmentary++; break;
                    case MemoryClarity.Forgotten: forgotten++; break;
                }
                if (r.IsCertified) certified++;
                if (r.IsPreserved) preserved++;
            }
            return new MemoryDecayCensus(
                _state.Records.Count,
                vivid,
                clear,
                vague,
                fragmentary,
                forgotten,
                certified,
                preserved,
                _domainRates.Count);
        }
    }

    public struct MemoryDecayCensus
    {
        public int TotalRecords { get; }
        public int VividCount { get; }
        public int ClearCount { get; }
        public int VagueCount { get; }
        public int FragmentaryCount { get; }
        public int ForgottenCount { get; }
        public int CertifiedCount { get; }
        public int PreservedCount { get; }
        public int DomainRatesCount { get; }

        public MemoryDecayCensus(
            int totalRecords,
            int vividCount,
            int clearCount,
            int vagueCount,
            int fragmentaryCount,
            int forgottenCount,
            int certifiedCount,
            int preservedCount,
            int domainRatesCount)
        {
            TotalRecords = totalRecords;
            VividCount = vividCount;
            ClearCount = clearCount;
            VagueCount = vagueCount;
            FragmentaryCount = fragmentaryCount;
            ForgottenCount = forgottenCount;
            CertifiedCount = certifiedCount;
            PreservedCount = preservedCount;
            DomainRatesCount = domainRatesCount;
        }
    }
}
