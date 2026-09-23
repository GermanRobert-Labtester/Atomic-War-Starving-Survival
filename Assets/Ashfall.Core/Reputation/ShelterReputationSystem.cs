// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Reputation
{
    public enum ReputationDimension
    {
        Reliability = 0,   // Reliable (+100) vs Treacherous (-100)
        Strength = 1,      // Formidable (+100) vs Vulnerable (-100)
        Generosity = 2,    // Sanctuary (+100) vs Selfish (-100)
        Ruthlessness = 3,  // Fearsome (+100) vs Lenient (-100)
        Wealth = 4         // Prosperous (+100) vs Impoverished (-100)
    }

    public enum ReputationTag
    {
        Sanctuary = 0,
        TradingPost = 1,
        Fortress = 2,
        Dangerous = 3,
        Treacherous = 4,
        Honorable = 5,
        Desperate = 6,
        RaiderBane = 7
    }

    public enum InformationMedium
    {
        Witness = 0,
        RadioBroadcast = 1,
        TraderWord = 2,
        RefugeeReport = 3,
        Propaganda = 4
    }

    [Serializable]
    public sealed class ReputationEvidence
    {
        public string EvidenceId { get; set; } = string.Empty;
        public ReputationDimension Dimension { get; set; }
        public float Delta { get; set; }
        public float Confidence { get; set; } = 1.0f; // 0.0 to 1.0
        public float Salience { get; set; } = 1.0f;   // 0.0 to 1.0
        public InformationMedium Medium { get; set; }
        public string SourceEventId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DayRecorded { get; set; } = 1;
        public string? AudienceFactionId { get; set; }
    }

    [Serializable]
    public sealed class ReputationDimensionDef
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string positive_title { get; set; } = string.Empty;
        public string negative_title { get; set; } = string.Empty;
        public float daily_decay { get; set; } = 0.25f;
    }

    [Serializable]
    public sealed class ReputationTagDef
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int trade_discount_permille { get; set; }
    }

    [Serializable]
    public sealed class ReputationCatalogData
    {
        public int schema_version { get; set; } = 1;
        public List<ReputationDimensionDef> dimensions { get; set; } = new List<ReputationDimensionDef>();
        public List<ReputationTagDef> tags { get; set; } = new List<ReputationTagDef>();
    }

    [Serializable]
    public sealed class ShelterReputationState
    {
        public int SchemaVersion { get; set; } = 1;
        public float Notoriety { get; set; } = 0f; // 0 to 100
        public Dictionary<string, float> DimensionScores { get; set; } = new Dictionary<string, float>();
        public List<string> ActiveTags { get; set; } = new List<string>();
        public List<ReputationEvidence> EvidenceHistory { get; set; } = new List<ReputationEvidence>();
        public int NextSequence { get; set; } = 1;
    }

    /// <summary>
    /// Plan 207 (C1[39]) — Shelter Reputation & External Perception System.
    /// Manages publicly knowable external perceptions across dimensions (Reliability, Strength,
    /// Generosity, Ruthlessness, Wealth), notoriety, evidence provenance, decay, and active public tags.
    /// </summary>
    public sealed class ShelterReputationSystem
    {
        private readonly ShelterReputationState _state;
        private readonly Dictionary<string, ReputationTagDef> _tagDefs = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>Plan 207 — authored perception-dimension definitions keyed by
        /// canonical id (normalized) and enum name. Daily decay reads from here so
        /// the JSON catalog stays authoritative instead of a hardcoded constant.</summary>
        private readonly Dictionary<string, ReputationDimensionDef> _dimensionDefs = new(StringComparer.OrdinalIgnoreCase);
        private const float DefaultDimensionDailyDecay = 0.25f;

        public event Action<ReputationEvidence>? OnEvidenceRecorded;
        public event Action<float>? OnNotorietyChanged;
        public event Action<ReputationTag>? OnTagGranted;
        public event Action<ReputationTag>? OnTagRevoked;
        public event Action? OnStateChanged;

        public event Action<ReputationTag>? OnTagAcquired
        {
            add => OnTagGranted += value;
            remove => OnTagGranted -= value;
        }

        public event Action<ReputationTag>? OnTagLost
        {
            add => OnTagRevoked += value;
            remove => OnTagRevoked -= value;
        }

        public float Notoriety => _state.Notoriety;
        public int TotalEvidenceCount => _state.EvidenceHistory.Count;
        public int EvidenceCount => _state.EvidenceHistory.Count;
        public IReadOnlyList<ReputationEvidence> EvidenceHistory => _state.EvidenceHistory;
        public IReadOnlyList<ReputationTag> ActiveTags => GetActiveTags();
        public ShelterReputationState State => _state;

        public ShelterReputationSystem(ShelterReputationState? state = null)
        {
            _state = state ?? new ShelterReputationState();
            InitializeDimensionsIfMissing();
        }

        private void InitializeDimensionsIfMissing()
        {
            foreach (ReputationDimension dim in Enum.GetValues(typeof(ReputationDimension)))
            {
                string key = dim.ToString();
                if (!_state.DimensionScores.ContainsKey(key))
                {
                    _state.DimensionScores[key] = 0f;
                }
            }
        }

        public float GetScore(ReputationDimension dimension)
        {
            string key = dimension.ToString();
            return _state.DimensionScores.TryGetValue(key, out float val) ? val : 0f;
        }

        public bool HasTag(ReputationTag tag)
        {
            return _state.ActiveTags.Contains(tag.ToString());
        }

        public IReadOnlyList<ReputationTag> GetActiveTags()
        {
            var list = new List<ReputationTag>();
            foreach (var tagStr in _state.ActiveTags)
            {
                if (Enum.TryParse<ReputationTag>(tagStr, out var tag))
                {
                    list.Add(tag);
                }
            }
            return list;
        }

        public IReadOnlyList<ReputationEvidence> GetAllEvidence()
        {
            return _state.EvidenceHistory;
        }

        public ReputationEvidence? RecordEvidence(
            string sourceEventId,
            ReputationDimension dimension,
            float delta,
            InformationMedium medium,
            int day,
            string description = "")
        {
            return RecordEvidence(dimension, delta, 1.0f, 1.0f, medium, sourceEventId, description, day);
        }

        public ReputationEvidence? RecordEvidence(
            ReputationDimension dimension,
            float delta,
            float confidence,
            float salience,
            InformationMedium medium,
            string sourceEventId,
            string description,
            int day,
            string? audienceFactionId = null)
        {
            if (string.IsNullOrWhiteSpace(sourceEventId)) throw new ArgumentNullException(nameof(sourceEventId));

            // Anti-farming check: prevent duplicate evidence from identical source event, medium, and audience
            bool exists = _state.EvidenceHistory.Any(e =>
                string.Equals(e.SourceEventId, sourceEventId, StringComparison.OrdinalIgnoreCase) &&
                e.Medium == medium &&
                string.Equals(e.AudienceFactionId, audienceFactionId, StringComparison.OrdinalIgnoreCase));

            if (exists)
            {
                return null;
            }

            float safeConfidence = Math.Clamp(confidence, 0.05f, 1.0f);
            float safeSalience = Math.Clamp(salience, 0.05f, 1.0f);
            float effectiveDelta = delta * safeConfidence * safeSalience;

            var evidence = new ReputationEvidence
            {
                EvidenceId = $"rep_{_state.NextSequence++}",
                Dimension = dimension,
                Delta = delta,
                Confidence = safeConfidence,
                Salience = safeSalience,
                Medium = medium,
                SourceEventId = sourceEventId.Trim(),
                Description = description ?? string.Empty,
                DayRecorded = Math.Max(1, day),
                AudienceFactionId = audienceFactionId
            };

            _state.EvidenceHistory.Add(evidence);

            // Update dimension score
            string dimKey = dimension.ToString();
            float currentScore = _state.DimensionScores.TryGetValue(dimKey, out float s) ? s : 0f;
            _state.DimensionScores[dimKey] = Math.Clamp(currentScore + effectiveDelta, -100f, 100f);

            // Increase notoriety based on salience and medium reach
            float mediumMultiplier = medium switch
            {
                InformationMedium.RadioBroadcast => 2.0f,
                InformationMedium.Propaganda => 1.5f,
                InformationMedium.TraderWord => 1.2f,
                InformationMedium.RefugeeReport => 1.0f,
                InformationMedium.Witness => 0.8f,
                _ => 1.0f
            };

            float notorietyGain = Math.Abs(effectiveDelta) * 0.15f * mediumMultiplier;
            float prevNotoriety = _state.Notoriety;
            _state.Notoriety = Math.Clamp(_state.Notoriety + notorietyGain, 0f, 100f);

            if (Math.Abs(_state.Notoriety - prevNotoriety) > 0.001f)
            {
                OnNotorietyChanged?.Invoke(_state.Notoriety);
            }

            OnEvidenceRecorded?.Invoke(evidence);

            EvaluateTags();
            OnStateChanged?.Invoke();
            return evidence;
        }

        public void TickDay(int currentDay)
        {
            // Apply subtle daily decay toward zero for dimension scores (0.5% per day)
            foreach (ReputationDimension dim in Enum.GetValues(typeof(ReputationDimension)))
            {
                string key = dim.ToString();
                if (_state.DimensionScores.TryGetValue(key, out float score))
                {
                    if (Math.Abs(score) > 0.01f)
                    {
                        float magnitude = GetAuthoredDailyDecay(dim);
                        float decay = score > 0 ? -magnitude : magnitude;
                        if (Math.Abs(score) < magnitude)
                        {
                            _state.DimensionScores[key] = 0f;
                        }
                        else
                        {
                            _state.DimensionScores[key] = score + decay;
                        }
                    }
                }
            }

            // Notoriety also very slowly decays if no new evidence (0.1 per day)
            if (_state.Notoriety > 0f)
            {
                _state.Notoriety = Math.Max(0f, _state.Notoriety - 0.1f);
                OnNotorietyChanged?.Invoke(_state.Notoriety);
            }

            EvaluateTags();
            OnStateChanged?.Invoke();
        }

        private void EvaluateTags()
        {
            float reliability = GetScore(ReputationDimension.Reliability);
            float strength = GetScore(ReputationDimension.Strength);
            float generosity = GetScore(ReputationDimension.Generosity);
            float ruthlessness = GetScore(ReputationDimension.Ruthlessness);
            float wealth = GetScore(ReputationDimension.Wealth);

            UpdateTag(ReputationTag.Sanctuary, generosity >= 40f && ruthlessness <= 10f);
            UpdateTag(ReputationTag.Fortress, strength >= 40f);
            UpdateTag(ReputationTag.TradingPost, wealth >= 30f && reliability >= 20f);
            UpdateTag(ReputationTag.Dangerous, ruthlessness >= 40f);
            UpdateTag(ReputationTag.Treacherous, reliability <= -40f);
            UpdateTag(ReputationTag.Honorable, reliability >= 40f);
            UpdateTag(ReputationTag.Desperate, wealth <= -40f && strength <= -20f);
            UpdateTag(ReputationTag.RaiderBane, ruthlessness >= 30f && strength >= 30f);
        }

        private void UpdateTag(ReputationTag tag, bool condition)
        {
            string tagStr = tag.ToString();
            bool hasTag = _state.ActiveTags.Contains(tagStr);

            if (condition && !hasTag)
            {
                _state.ActiveTags.Add(tagStr);
                OnTagGranted?.Invoke(tag);
            }
            else if (!condition && hasTag)
            {
                _state.ActiveTags.Remove(tagStr);
                OnTagRevoked?.Invoke(tag);
            }
        }

        public void LoadCatalog(ReputationCatalogData catalog)
        {
            if (catalog?.tags == null) return;
            foreach (var t in catalog.tags)
            {
                if (!string.IsNullOrEmpty(t.id))
                {
                    _tagDefs[t.id] = t;
                    _tagDefs[t.id.Replace("_", "")] = t;
                }
            }

            if (catalog.dimensions != null)
            {
                foreach (var d in catalog.dimensions)
                {
                    if (string.IsNullOrWhiteSpace(d.id)) continue;
                    _dimensionDefs[d.id] = d;
                    _dimensionDefs[d.id.Replace("_", "")] = d;
                }
            }
        }

        /// <summary>Plan 207 — authored dimension definitions (display names, titles, decay).</summary>
        public IReadOnlyCollection<ReputationDimensionDef> DimensionDefinitions => _dimensionDefs.Values;

        public ReputationDimensionDef? GetDimensionDefinition(ReputationDimension dimension)
        {
            _dimensionDefs.TryGetValue(dimension.ToString(), out var def);
            return def;
        }

        private float GetAuthoredDailyDecay(ReputationDimension dimension)
        {
            var def = GetDimensionDefinition(dimension);
            return def != null && def.daily_decay > 0f ? def.daily_decay : DefaultDimensionDailyDecay;
        }

        public float GetTradePriceMultiplier()
        {
            int netDiscountPermille = 0;
            foreach (var tagStr in _state.ActiveTags)
            {
                string normalizedKey = tagStr.ToLowerInvariant();
                if (_tagDefs.TryGetValue(normalizedKey, out var def))
                {
                    netDiscountPermille += def.trade_discount_permille;
                }
                else
                {
                    if (Enum.TryParse<ReputationTag>(tagStr, out var tag))
                    {
                        netDiscountPermille += tag switch
                        {
                            ReputationTag.TradingPost => 100,
                            ReputationTag.Honorable => 80,
                            ReputationTag.Sanctuary => 50,
                            ReputationTag.Dangerous => -150,
                            ReputationTag.Treacherous => -250,
                            ReputationTag.Desperate => -100,
                            ReputationTag.RaiderBane => 20,
                            _ => 0
                        };
                    }
                }
            }

            float multiplier = 1.0f - (netDiscountPermille / 1000f);
            return Math.Clamp(multiplier, 0.70f, 1.50f);
        }

        public string GetDominantPerceptionTitle()
        {
            if (HasTag(ReputationTag.Treacherous)) return "Treacherous Holdfast";
            if (HasTag(ReputationTag.Sanctuary)) return "Sanctuary for the Stricken";
            if (HasTag(ReputationTag.TradingPost)) return "Wasteland Trading Post";
            if (HasTag(ReputationTag.Fortress)) return "Impenetrable Fortress";
            if (HasTag(ReputationTag.Dangerous)) return "Dangerous Territory";
            if (HasTag(ReputationTag.RaiderBane)) return "Raider Bane";
            if (HasTag(ReputationTag.Honorable)) return "Honorable Bastion";
            if (HasTag(ReputationTag.Desperate)) return "Desperate Outpost";

            if (_state.Notoriety < 10f) return "Unknown Holdfast";
            return "Known Wasteland Settlement";
        }

        public ShelterReputationState CaptureState()
        {
            return new ShelterReputationState
            {
                SchemaVersion = _state.SchemaVersion,
                Notoriety = _state.Notoriety,
                DimensionScores = new Dictionary<string, float>(_state.DimensionScores),
                ActiveTags = new List<string>(_state.ActiveTags),
                EvidenceHistory = new List<ReputationEvidence>(_state.EvidenceHistory),
                NextSequence = _state.NextSequence
            };
        }

        public void RestoreState(ShelterReputationState state)
        {
            if (state == null) return;

            _state.SchemaVersion = state.SchemaVersion;
            _state.Notoriety = state.Notoriety;
            _state.DimensionScores.Clear();
            foreach (var kvp in state.DimensionScores)
            {
                _state.DimensionScores[kvp.Key] = kvp.Value;
            }
            InitializeDimensionsIfMissing();

            _state.ActiveTags.Clear();
            _state.ActiveTags.AddRange(state.ActiveTags);

            _state.EvidenceHistory.Clear();
            _state.EvidenceHistory.AddRange(state.EvidenceHistory);

            _state.NextSequence = state.NextSequence;

            EvaluateTags();
            OnStateChanged?.Invoke();
        }
    }
}
