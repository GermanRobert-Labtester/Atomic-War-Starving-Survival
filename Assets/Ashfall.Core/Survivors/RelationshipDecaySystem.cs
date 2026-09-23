// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Survivors
{
    public enum SurvivorBondType
    {
        Acquaintance = 0,
        Friend = 1,
        CloseFriend = 2,
        Family = 3,
        Mentor = 4,
        Rival = 5
    }

    public enum SocialDriftType
    {
        GrewApart = 0,
        TrustEroded = 1,
        FriendshipFaded = 2,
        ResentmentBuilt = 3
    }

    [Serializable]
    public sealed class PairBondState
    {
        public string SurvivorA { get; set; } = string.Empty;
        public string SurvivorB { get; set; } = string.Empty;
        public SurvivorBondType Bond { get; set; } = SurvivorBondType.Acquaintance;
        public float Affinity { get; set; } = 20f;
        public float Trust { get; set; } = 30f;
        public int LastInteractionDay { get; set; } = 1;
        public int DaysWithoutInteraction { get; set; } = 0;
    }

    [Serializable]
    public sealed class SocialDriftEvent
    {
        public string EventId { get; set; } = string.Empty;
        public string SurvivorA { get; set; } = string.Empty;
        public string SurvivorB { get; set; } = string.Empty;
        public SocialDriftType DriftType { get; set; } = SocialDriftType.GrewApart;
        public int Day { get; set; } = 1;
        public string Description { get; set; } = string.Empty;
        public float AffinityDelta { get; set; } = 0f;
    }

    [Serializable]
    public sealed class RelationshipDecayProfileDefinition
    {
        [JsonPropertyName("profile_id")]
        public string ProfileId { get; set; } = string.Empty;

        [JsonPropertyName("bond_type")]
        public string BondType { get; set; } = string.Empty;

        [JsonPropertyName("decay_rate_per_day")]
        public float DecayRatePerDay { get; set; } = 0.80f;

        [JsonPropertyName("neglect_threshold_days")]
        public int NeglectThresholdDays { get; set; } = 3;

        [JsonPropertyName("drift_threshold_affinity")]
        public float DriftThresholdAffinity { get; set; } = 0f;

        [JsonPropertyName("reconnection_bonus_multiplier")]
        public float ReconnectionBonusMultiplier { get; set; } = 1.0f;

        [JsonPropertyName("shared_duty_mitigation")]
        public float SharedDutyMitigation { get; set; } = 0.50f;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        public SurvivorBondType GetBondType() => BondType.ToLowerInvariant() switch
        {
            "acquaintance" => SurvivorBondType.Acquaintance,
            "friend" => SurvivorBondType.Friend,
            "closefriend" or "close_friend" => SurvivorBondType.CloseFriend,
            "family" => SurvivorBondType.Family,
            "mentor" => SurvivorBondType.Mentor,
            "rival" => SurvivorBondType.Rival,
            _ => SurvivorBondType.Acquaintance
        };
    }

    [Serializable]
    public sealed class RelationshipDecayCatalogData
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("profiles")]
        public List<RelationshipDecayProfileDefinition> Profiles { get; set; } = new List<RelationshipDecayProfileDefinition>();
    }

    [Serializable]
    public sealed class RelationshipDecayState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<PairBondState> Pairs { get; set; } = new List<PairBondState>();
        public List<SocialDriftEvent> DriftHistory { get; set; } = new List<SocialDriftEvent>();
    }

    /// <summary>
    /// Plan 182 — Relationship Decay & Drift System.
    /// Simulates social bond decay over time without interaction; friends drift apart,
    /// trust erodes from neglect, while regular interactions and shared quarters reinforce bonds.
    /// </summary>
    public sealed class RelationshipDecaySystem
    {
        private readonly RelationshipDecayState _state;
        private readonly Dictionary<SurvivorBondType, RelationshipDecayProfileDefinition> _profilesByBond = new Dictionary<SurvivorBondType, RelationshipDecayProfileDefinition>();
        private readonly Dictionary<string, RelationshipDecayProfileDefinition> _profilesById = new Dictionary<string, RelationshipDecayProfileDefinition>(StringComparer.OrdinalIgnoreCase);

        public event Action<SocialDriftEvent>? OnBondDrifted;
        public event Action<PairBondState>? OnBondBroken;

        public Func<string /*survivorA*/, string /*survivorB*/, float /*mitigationFactor*/>? BondingMitigationProvider { get; set; }
        public Action<PairBondState, SocialDriftType>? BondDriftBridge { get; set; }

        public int TrackedPairCount => _state.Pairs.Count;
        public IReadOnlyList<PairBondState> Pairs => _state.Pairs;
        public IReadOnlyList<SocialDriftEvent> DriftHistory => _state.DriftHistory;
        public IReadOnlyCollection<RelationshipDecayProfileDefinition> Profiles => _profilesById.Values;

        public RelationshipDecaySystem(RelationshipDecayState? state = null)
        {
            _state = state ?? new RelationshipDecayState();
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var data = JsonSerializer.Deserialize<RelationshipDecayCatalogData>(json, options);
            if (data != null)
            {
                LoadCatalog(data);
            }
        }

        public void LoadCatalog(RelationshipDecayCatalogData catalog)
        {
            if (catalog?.Profiles == null) return;
            foreach (var p in catalog.Profiles)
            {
                if (!string.IsNullOrWhiteSpace(p.ProfileId))
                {
                    _profilesById[p.ProfileId] = p;
                }
                var bond = p.GetBondType();
                _profilesByBond[bond] = p;
            }
        }

        public RelationshipDecayProfileDefinition? GetProfile(SurvivorBondType bond)
        {
            return _profilesByBond.TryGetValue(bond, out var p) ? p : null;
        }

        public RelationshipDecayProfileDefinition? GetProfile(string profileId)
        {
            if (string.IsNullOrWhiteSpace(profileId)) return null;
            return _profilesById.TryGetValue(profileId, out var p) ? p : null;
        }

        private static string OrderKey(string a, string b) =>
            string.CompareOrdinal(a, b) <= 0 ? $"{a}|{b}" : $"{b}|{a}";

        public PairBondState RegisterOrUpdatePair(
            string survivorA,
            string survivorB,
            SurvivorBondType bond,
            float initialAffinity = 25f,
            float initialTrust = 30f,
            int currentDay = 1)
        {
            if (string.IsNullOrWhiteSpace(survivorA)) throw new ArgumentNullException(nameof(survivorA));
            if (string.IsNullOrWhiteSpace(survivorB)) throw new ArgumentNullException(nameof(survivorB));

            string a = survivorA.Trim();
            string b = survivorB.Trim();
            if (string.CompareOrdinal(a, b) > 0)
            {
                (a, b) = (b, a);
            }

            var pair = _state.Pairs.FirstOrDefault(p =>
                string.Equals(p.SurvivorA, a, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(p.SurvivorB, b, StringComparison.OrdinalIgnoreCase));

            if (pair == null)
            {
                pair = new PairBondState
                {
                    SurvivorA = a,
                    SurvivorB = b,
                    Bond = bond,
                    Affinity = Math.Clamp(initialAffinity, -100f, 100f),
                    Trust = Math.Clamp(initialTrust, 0f, 100f),
                    LastInteractionDay = currentDay,
                    DaysWithoutInteraction = 0
                };
                _state.Pairs.Add(pair);
            }
            else
            {
                pair.Bond = bond;
                pair.Affinity = Math.Clamp(initialAffinity, -100f, 100f);
                pair.Trust = Math.Clamp(initialTrust, 0f, 100f);
            }

            return pair;
        }

        public PairBondState? GetPair(string survivorA, string survivorB)
        {
            return _state.Pairs.FirstOrDefault(p =>
                (string.Equals(p.SurvivorA, survivorA, StringComparison.OrdinalIgnoreCase) && string.Equals(p.SurvivorB, survivorB, StringComparison.OrdinalIgnoreCase)) ||
                (string.Equals(p.SurvivorA, survivorB, StringComparison.OrdinalIgnoreCase) && string.Equals(p.SurvivorB, survivorA, StringComparison.OrdinalIgnoreCase)));
        }

        public bool RecordInteraction(string survivorA, string survivorB, string interactionType, float affinityBonus, int currentDay)
        {
            var pair = GetPair(survivorA, survivorB) ?? RegisterOrUpdatePair(survivorA, survivorB, SurvivorBondType.Acquaintance, currentDay: currentDay);

            float effectiveBonus = affinityBonus;
            var profile = GetProfile(pair.Bond);
            if (profile != null && pair.DaysWithoutInteraction > profile.NeglectThresholdDays && affinityBonus > 0f)
            {
                effectiveBonus *= profile.ReconnectionBonusMultiplier;
            }

            pair.LastInteractionDay = currentDay;
            pair.DaysWithoutInteraction = 0;
            pair.Affinity = Math.Clamp(pair.Affinity + effectiveBonus, -100f, 100f);

            if (effectiveBonus > 0f)
            {
                pair.Trust = Math.Clamp(pair.Trust + (effectiveBonus * 0.4f), 0f, 100f);
            }

            // Upgrade bond if affinity crosses threshold
            if (pair.Bond == SurvivorBondType.Acquaintance && pair.Affinity >= 40f && pair.Trust >= 30f)
            {
                pair.Bond = SurvivorBondType.Friend;
            }
            else if (pair.Bond == SurvivorBondType.Friend && pair.Affinity >= 75f && pair.Trust >= 60f)
            {
                pair.Bond = SurvivorBondType.CloseFriend;
            }

            return true;
        }

        public void TickDay(int currentDay)
        {
            for (int i = 0; i < _state.Pairs.Count; i++)
            {
                var pair = _state.Pairs[i];
                pair.DaysWithoutInteraction++;

                int neglectThreshold = 3;
                float decayRate = pair.Bond switch
                {
                    SurvivorBondType.Family => 0.15f,
                    SurvivorBondType.CloseFriend => 0.35f,
                    SurvivorBondType.Friend => 0.60f,
                    SurvivorBondType.Mentor => 0.25f,
                    SurvivorBondType.Rival => 0.10f,
                    _ => 0.80f // Acquaintance
                };

                var profile = GetProfile(pair.Bond);
                if (profile != null)
                {
                    neglectThreshold = profile.NeglectThresholdDays;
                    decayRate = profile.DecayRatePerDay;
                }

                if (pair.DaysWithoutInteraction > neglectThreshold)
                {
                    if (BondingMitigationProvider != null)
                    {
                        float mitigation = Math.Clamp(BondingMitigationProvider(pair.SurvivorA, pair.SurvivorB), 0f, 1f);
                        decayRate *= (1f - mitigation);
                    }

                    float oldAffinity = pair.Affinity;
                    pair.Affinity = Math.Clamp(pair.Affinity - decayRate, -100f, 100f);
                    pair.Trust = Math.Clamp(pair.Trust - (decayRate * 0.5f), 0f, 100f);

                    // Check for drift thresholds
                    if (pair.Bond == SurvivorBondType.CloseFriend && pair.Affinity < 50f)
                    {
                        pair.Bond = SurvivorBondType.Friend;
                        var ev = new SocialDriftEvent
                        {
                            EventId = $"dev_{_state.NextSequence++}",
                            SurvivorA = pair.SurvivorA,
                            SurvivorB = pair.SurvivorB,
                            DriftType = SocialDriftType.FriendshipFaded,
                            Day = currentDay,
                            Description = $"{pair.SurvivorA} and {pair.SurvivorB} have drifted from close friends to casual friends.",
                            AffinityDelta = pair.Affinity - oldAffinity
                        };
                        _state.DriftHistory.Add(ev);
                        OnBondDrifted?.Invoke(ev);
                        BondDriftBridge?.Invoke(pair, SocialDriftType.FriendshipFaded);
                    }
                    else if (pair.Bond == SurvivorBondType.Friend && pair.Affinity <= 0f)
                    {
                        pair.Bond = SurvivorBondType.Acquaintance;
                        var ev = new SocialDriftEvent
                        {
                            EventId = $"dev_{_state.NextSequence++}",
                            SurvivorA = pair.SurvivorA,
                            SurvivorB = pair.SurvivorB,
                            DriftType = SocialDriftType.GrewApart,
                            Day = currentDay,
                            Description = $"{pair.SurvivorA} and {pair.SurvivorB} have completely grown apart.",
                            AffinityDelta = pair.Affinity - oldAffinity
                        };
                        _state.DriftHistory.Add(ev);
                        OnBondDrifted?.Invoke(ev);
                        OnBondBroken?.Invoke(pair);
                        BondDriftBridge?.Invoke(pair, SocialDriftType.GrewApart);
                    }
                }
            }
        }

        public RelationshipDecayState CaptureState()
        {
            var state = new RelationshipDecayState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                Pairs = new List<PairBondState>(_state.Pairs.Count),
                DriftHistory = new List<SocialDriftEvent>(_state.DriftHistory.Count)
            };

            foreach (var p in _state.Pairs)
            {
                state.Pairs.Add(new PairBondState
                {
                    SurvivorA = p.SurvivorA,
                    SurvivorB = p.SurvivorB,
                    Bond = p.Bond,
                    Affinity = p.Affinity,
                    Trust = p.Trust,
                    LastInteractionDay = p.LastInteractionDay,
                    DaysWithoutInteraction = p.DaysWithoutInteraction
                });
            }

            foreach (var d in _state.DriftHistory)
            {
                state.DriftHistory.Add(new SocialDriftEvent
                {
                    EventId = d.EventId,
                    SurvivorA = d.SurvivorA,
                    SurvivorB = d.SurvivorB,
                    DriftType = d.DriftType,
                    Day = d.Day,
                    Description = d.Description,
                    AffinityDelta = d.AffinityDelta
                });
            }

            return state;
        }

        public void RestoreState(RelationshipDecayState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.Pairs.Clear();
            _state.DriftHistory.Clear();

            if (state.Pairs != null)
            {
                foreach (var p in state.Pairs)
                {
                    _state.Pairs.Add(new PairBondState
                    {
                        SurvivorA = p.SurvivorA,
                        SurvivorB = p.SurvivorB,
                        Bond = p.Bond,
                        Affinity = p.Affinity,
                        Trust = p.Trust,
                        LastInteractionDay = p.LastInteractionDay,
                        DaysWithoutInteraction = p.DaysWithoutInteraction
                    });
                }
            }

            if (state.DriftHistory != null)
            {
                foreach (var d in state.DriftHistory)
                {
                    _state.DriftHistory.Add(new SocialDriftEvent
                    {
                        EventId = d.EventId,
                        SurvivorA = d.SurvivorA,
                        SurvivorB = d.SurvivorB,
                        DriftType = d.DriftType,
                        Day = d.Day,
                        Description = d.Description,
                        AffinityDelta = d.AffinityDelta
                    });
                }
            }
        }
    }
}
