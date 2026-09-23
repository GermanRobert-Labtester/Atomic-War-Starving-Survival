// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Relations;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    [Serializable]
    public sealed class SurvivorRelationsState
    {
        public string systemId = SurvivorRelationsSystem.SystemId;
        public List<RelationshipEntry> relationships = new List<RelationshipEntry>();
        public List<ConflictEntry> activeConflicts = new List<ConflictEntry>();
        public List<MediationEntry> mediationHistory = new List<MediationEntry>();
    }

    [Serializable]
    public sealed class RelationshipEntry
    {
        public string dwellerA = string.Empty;
        public string dwellerB = string.Empty;
        public float affinity;      // -100 to 100
        public float trust;         // 0 to 100
        public float resentment;    // 0 to 100
        public float grief;         // 0 to 100
        /// <summary>Plan 24C (A3) — campaign day the relationship's grief was
        /// last applied by a memorialized death (−1 = never). Persisted canonical
        /// fact: the derived grief-to-needs projection decays from this onset;
        /// legacy saves default to −1 (no needs-grief effect).</summary>
        public int grief_since_day = -1;
        public string bondType = string.Empty; // "friendship", "rivalry", "mentor", "caregiver", etc.
        public List<string> recentCauses = new List<string>();
        /// <summary>
        /// Plan 182 — campaign day of the last real interaction between this pair
        /// (-1 = never). Stamped by the pair-affinity producers; it is the
        /// precondition for any future neglect rule. No decay is applied yet, and
        /// time passing alone never moves it.
        /// </summary>
        public int lastInteractionDay = -1;
        /// <summary>
        /// Plan 44 / C2[19] — Bounded traceable history entries for this pair.
        /// </summary>
        public List<PairRelationHistoryEntry> history = new List<PairRelationHistoryEntry>();
    }

    [Serializable]
    public sealed class ConflictEntry
    {
        public string conflictId = string.Empty;
        public string dwellerA = string.Empty;
        public string dwellerB = string.Empty;
        public string cause = string.Empty;
        public int dayStarted;
        public bool isResolved;
        public string resolution = string.Empty;
    }

    [Serializable]
    public sealed class MediationEntry
    {
        public string conflictId = string.Empty;
        public int day;
        public string mediatorId = string.Empty;
        public string outcome = string.Empty;
        public float affinityChange;
    }

    public sealed class SurvivorRelationsSystem
    {
        public const string SystemId = "survivor_relations";
        private SurvivorRelationsState _state = new SurvivorRelationsState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private int _currentDay;
        private readonly List<RelationshipBandDefinition> _bands = new List<RelationshipBandDefinition>();

        public SurvivorRelationsState State => _state;
        public IReadOnlyList<RelationshipBandDefinition> Bands => _bands;

        public event Action<ConflictEntry> OnConflictStarted;
        public event Action<MediationEntry> OnConflictResolved;
        public event Action OnRelationsChanged;

        /// <summary>
        /// Plan 44 / C2[19] — Delegate seam invoked whenever a pair relation effect is evaluated.
        /// </summary>
        public Action<string, string, RelationEffect>? RelationEffectAppliedSeam { get; set; }

        /// <summary>
        /// Plan 44 / C2[19] — Delegate seam invoked whenever a pair relation history entry is recorded.
        /// </summary>
        public Action<PairRelationHistoryEntry>? HistoryEntryRecordedSeam { get; set; }

        public const int MaxHistoryPerPair = 10;

        public SurvivorRelationsSystem(ISeededRng rng, ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
            InitializeDefaultBands();
        }

        private void InitializeDefaultBands()
        {
            _bands.Clear();
            _bands.Add(new RelationshipBandDefinition
            {
                BandId = "hostile",
                DisplayName = "Hostile",
                MinAffinity = -100f,
                MaxAffinity = -40f,
                WorkingModifier = -0.25f,
                MoraleModifier = -0.15f,
                ErrorRiskModifier = 0.25f,
                ExpeditionRiskModifier = 0.20f,
                SeparationModifier = 0.0f,
                CaregivingModifier = -0.20f,
                TrainingModifier = -0.25f,
                NoteKey = "relations.band.hostile"
            });
            _bands.Add(new RelationshipBandDefinition
            {
                BandId = "strained",
                DisplayName = "Strained",
                MinAffinity = -40f,
                MaxAffinity = -10f,
                WorkingModifier = -0.10f,
                MoraleModifier = -0.05f,
                ErrorRiskModifier = 0.10f,
                ExpeditionRiskModifier = 0.08f,
                SeparationModifier = 0.0f,
                CaregivingModifier = -0.10f,
                TrainingModifier = -0.10f,
                NoteKey = "relations.band.strained"
            });
            _bands.Add(new RelationshipBandDefinition
            {
                BandId = "cordial",
                DisplayName = "Cordial",
                MinAffinity = -10f,
                MaxAffinity = 25f,
                WorkingModifier = 0.0f,
                MoraleModifier = 0.0f,
                ErrorRiskModifier = 0.0f,
                ExpeditionRiskModifier = 0.0f,
                SeparationModifier = 0.0f,
                CaregivingModifier = 0.0f,
                TrainingModifier = 0.0f,
                NoteKey = "relations.band.cordial"
            });
            _bands.Add(new RelationshipBandDefinition
            {
                BandId = "close",
                DisplayName = "Close",
                MinAffinity = 25f,
                MaxAffinity = 60f,
                WorkingModifier = 0.10f,
                MoraleModifier = 0.08f,
                ErrorRiskModifier = -0.08f,
                ExpeditionRiskModifier = -0.06f,
                SeparationModifier = -0.05f,
                CaregivingModifier = 0.15f,
                TrainingModifier = 0.15f,
                NoteKey = "relations.band.close"
            });
            _bands.Add(new RelationshipBandDefinition
            {
                BandId = "bonded",
                DisplayName = "Bonded",
                MinAffinity = 60f,
                MaxAffinity = 100f,
                WorkingModifier = 0.20f,
                MoraleModifier = 0.15f,
                ErrorRiskModifier = -0.15f,
                ExpeditionRiskModifier = -0.12f,
                SeparationModifier = -0.10f,
                CaregivingModifier = 0.25f,
                TrainingModifier = 0.25f,
                NoteKey = "relations.band.bonded"
            });
        }

        public void LoadBandsCatalog(RelationshipBandsCatalog catalog)
        {
            if (catalog?.Bands == null || catalog.Bands.Count == 0) return;
            _bands.Clear();
            _bands.AddRange(catalog.Bands);
        }

        public RelationshipEntry GetOrCreateRelationship(string a, string b)
        {
            var key = MakeKey(a, b);
            var existing = _state.relationships.Find(r => MakeKey(r.dwellerA, r.dwellerB) == key);
            if (existing != null) return existing;
            var rel = new RelationshipEntry { dwellerA = a, dwellerB = b };
            _state.relationships.Add(rel);
            return rel;
        }

        private static string MakeKey(string a, string b) =>
            string.CompareOrdinal(a, b) <= 0 ? $"{a}|{b}" : $"{b}|{a}";

        public void ModifyAffinity(string a, string b, float delta)
        {
            var rel = GetOrCreateRelationship(a, b);
            rel.affinity = Math.Max(-100, Math.Min(100, rel.affinity + delta));
            if (delta < 0) rel.resentment = Math.Min(100, rel.resentment - delta);
            rel.recentCauses.Add($"affinity_{delta:F0} on day {_currentDay}");
            // Plan 182: a real interaction happened — stamp it. This is the only
            // place pair affinity is produced, so every existing producer stamps
            // without touching its own call site.
            rel.lastInteractionDay = _currentDay;

            // Plan 44 / C2[19]: Record explainable pair history
            RecordPairHistory(a, b, $"affinity_delta_{delta:F0}", "affinity_change", delta, "system", "");
            OnRelationsChanged?.Invoke();
        }

        public RelationEffect GetRelationEffect(string a, string b)
        {
            if (!TryGetRelationship(a, b, out var rel) || rel == null)
            {
                return ResolveBandForAffinity(0f);
            }

            var effect = ResolveBandForAffinity(rel.affinity);
            RelationEffectAppliedSeam?.Invoke(a, b, effect);
            return effect;
        }

        public RelationEffect EffectOf(string a, string b) => GetRelationEffect(a, b);

        public RelationEffect ResolveBandForAffinity(float affinity)
        {
            for (int i = 0; i < _bands.Count; i++)
            {
                var b = _bands[i];
                if (affinity >= b.MinAffinity && (affinity <= b.MaxAffinity || (i == _bands.Count - 1 && affinity >= b.MaxAffinity)))
                {
                    return new RelationEffect(
                        b.BandId,
                        b.DisplayName,
                        b.WorkingModifier,
                        b.MoraleModifier,
                        b.ErrorRiskModifier,
                        b.ExpeditionRiskModifier,
                        b.SeparationModifier,
                        b.CaregivingModifier,
                        b.TrainingModifier,
                        b.NoteKey
                    );
                }
            }

            return RelationEffect.NeutralFallback;
        }

        public PairRelationHistoryEntry RecordPairHistory(
            string a,
            string b,
            string causeId,
            string kind,
            float delta,
            string sourceOwner = "system",
            string noteKey = "")
        {
            var rel = GetOrCreateRelationship(a, b);
            var effect = ResolveBandForAffinity(rel.affinity);

            var entry = new PairRelationHistoryEntry
            {
                EventId = $"rel_hist_{_currentDay}_{a}_{b}_{rel.history.Count}",
                Day = _currentDay,
                CauseId = causeId ?? "general",
                Kind = kind ?? "interaction",
                Delta = delta,
                ResultingBand = effect.BandId,
                SourceOwner = sourceOwner ?? "system",
                NoteKey = string.IsNullOrEmpty(noteKey) ? effect.NoteKey : noteKey
            };

            rel.history.Add(entry);
            if (rel.history.Count > MaxHistoryPerPair)
            {
                rel.history.RemoveAt(0); // prune oldest
            }

            rel.lastInteractionDay = _currentDay;
            HistoryEntryRecordedSeam?.Invoke(entry);
            return entry;
        }

        public TeamRelationAggregate AggregateTeamEffect(IEnumerable<string> memberIds)
        {
            if (memberIds == null) return TeamRelationAggregate.Neutral;
            var list = new List<string>(memberIds);
            if (list.Count < 2) return TeamRelationAggregate.Neutral;

            int totalPairs = 0;
            int hostileCount = 0;
            int bondedCount = 0;
            float totalWorkMod = 0f;
            float totalMoraleMod = 0f;
            float maxRiskMod = 0f;
            string dominantNoteKey = "relations.team.neutral";

            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    totalPairs++;
                    var effect = GetRelationEffect(list[i], list[j]);
                    totalWorkMod += effect.WorkingModifier;
                    totalMoraleMod += effect.MoraleModifier;
                    if (effect.ErrorRiskModifier > maxRiskMod)
                    {
                        maxRiskMod = effect.ErrorRiskModifier;
                        dominantNoteKey = effect.NoteKey;
                    }
                    if (effect.BandId == "hostile" || effect.BandId == "strained")
                    {
                        hostileCount++;
                    }
                    else if (effect.BandId == "bonded")
                    {
                        bondedCount++;
                    }
                }
            }

            if (totalPairs == 0) return TeamRelationAggregate.Neutral;

            float avgWorkMod = totalWorkMod / totalPairs;
            float avgMoraleMod = totalMoraleMod / totalPairs;

            return new TeamRelationAggregate(
                totalPairs,
                hostileCount,
                bondedCount,
                avgWorkMod,
                maxRiskMod,
                avgMoraleMod,
                dominantNoteKey
            );
        }

        public void ModifyTrust(string a, string b, float delta)
        {
            var rel = GetOrCreateRelationship(a, b);
            rel.trust = Math.Max(0, Math.Min(100, rel.trust + delta));
            // Plan 182: trust is also a real interaction between the pair.
            rel.lastInteractionDay = _currentDay;
        }

        public void ApplyGrief(string survivorId, float amount)
        {
            foreach (var rel in _state.relationships)
            {
                if (rel.dwellerA == survivorId || rel.dwellerB == survivorId)
                {
                    rel.grief = Math.Min(100, rel.grief + amount);
                    rel.affinity = Math.Max(-100, rel.affinity - amount * 0.5f);
                }
            }
            OnRelationsChanged?.Invoke();
        }

        /// <summary>Plan 24C (A3) — non-creating relationship lookup for the
        /// grief bridge: returns the pair's existing entry, or false. Unlike
        /// <see cref="GetOrCreateRelationship"/> it never mutates state.</summary>
        public bool TryGetRelationship(string a, string b, out RelationshipEntry? entry)
        {
            entry = null;
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return false;
            for (int i = 0; i < _state.relationships.Count; i++)
            {
                var rel = _state.relationships[i];
                if (rel == null) continue;
                if ((rel.dwellerA == a && rel.dwellerB == b)
                    || (rel.dwellerA == b && rel.dwellerB == a))
                {
                    entry = rel;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Plan 60 / D7 — the other half of every relationship this survivor is in,
        /// ordinal-sorted and de-duplicated. Used to tell the memorial pipeline who
        /// actually mourns whom, so grief is applied to the living rather than to a
        /// whole-shelter average. Returns an empty list for unknown survivors.
        /// </summary>
        public IReadOnlyList<string> RelatedIds(string survivorId)
        {
            var result = new List<string>();
            if (string.IsNullOrEmpty(survivorId)) return result;

            for (int i = 0; i < _state.relationships.Count; i++)
            {
                var rel = _state.relationships[i];
                if (rel == null) continue;
                string other = null;
                if (rel.dwellerA == survivorId) other = rel.dwellerB;
                else if (rel.dwellerB == survivorId) other = rel.dwellerA;
                if (string.IsNullOrEmpty(other) || other == survivorId) continue;
                if (!result.Contains(other)) result.Add(other);
            }

            result.Sort(StringComparer.Ordinal);
            return result;
        }

        public ConflictEntry? TryTriggerConflict()
        {
            if (_state.activeConflicts.Exists(c => !c.isResolved)) return null;
            if (_rng.NextDouble() > 0.1f) return null; // 10% chance per day

            var stressed = _state.relationships.FindAll(r => r.resentment > 50f || r.affinity < -30f);
            if (stressed.Count == 0) return null;

            var rel = stressed[_rng.Next(0, stressed.Count)];
            var conflict = new ConflictEntry
            {
                conflictId = $"conflict_{_currentDay}_{rel.dwellerA}_{rel.dwellerB}",
                dwellerA = rel.dwellerA, dwellerB = rel.dwellerB,
                cause = $"resentment {rel.resentment:F0}/affinity {rel.affinity:F0}",
                dayStarted = _currentDay
            };
            _state.activeConflicts.Add(conflict);
            _log.Info($"[Relations] conflict: {rel.dwellerA} vs {rel.dwellerB}");
            OnConflictStarted?.Invoke(conflict);
            OnRelationsChanged?.Invoke();
            return conflict;
        }

        public ActionResult Mediate(string conflictId, string mediatorId, MediationStyle style)
        {
            var conflict = _state.activeConflicts.Find(c => c.conflictId == conflictId);
            if (conflict == null) return ActionResult.Failed("unknown_conflict", "relations.unknown_conflict");
            if (conflict.isResolved) return ActionResult.Blocked("already_resolved", "relations.already_resolved");

            float affinityDelta = style switch
            {
                MediationStyle.Apology => 15f,
                MediationStyle.ResourceSettlement => 20f,
                MediationStyle.Discipline => -10f,
                MediationStyle.Refusal => -25f,
                _ => 5f
            };

            ModifyAffinity(conflict.dwellerA, conflict.dwellerB, affinityDelta);
            conflict.isResolved = true;
            conflict.resolution = style.ToString();

            var entry = new MediationEntry
            {
                conflictId = conflictId, day = _currentDay,
                mediatorId = mediatorId ?? string.Empty,
                outcome = style.ToString(), affinityChange = affinityDelta
            };
            _state.mediationHistory.Add(entry);
            _log.Info($"[Relations] mediated {conflictId}: {style} ({affinityDelta:F0} affinity)");
            OnConflictResolved?.Invoke(entry);
            OnRelationsChanged?.Invoke();
            return ActionResult.Success("relations.mediated",
                new Dictionary<string, double> { { "affinity_change", affinityDelta } });
        }

        public void TickDay(int day)
        {
            _currentDay = day;
            TryTriggerConflict();
        }

        public SurvivorRelationsState CaptureState() => CloneState(_state);

        public void RestoreState(SurvivorRelationsState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static SurvivorRelationsState CloneState(SurvivorRelationsState src)
        {
            if (src == null) return new SurvivorRelationsState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<SurvivorRelationsState>(json) ?? new SurvivorRelationsState();
        }
    }

    public enum MediationStyle { Apology, ResourceSettlement, Discipline, Refusal }
}
