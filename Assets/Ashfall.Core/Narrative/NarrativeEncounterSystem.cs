using System;
using System.Collections.Generic;
#pragma warning disable CS8618

using Ashfall.Core.IO;
using Ashfall.Core.Content;
namespace Ashfall.Core.Narrative
{
    /// <summary>
    /// Engine-agnostic port of the Unity narrative-encounter layer
    /// (EncounterSO selection math + NarrativeEncounters factories, now
    /// data-driven). Owns the catalog, weighted encounter selection per the
    /// Unity formula (stance multipliers, danger/location filters), and a
    /// save/load-safe resolution history. All rolls through ISeededRng.
    /// </summary>
    public class NarrativeEncounterSystem
    {
        public const string SystemId = "narrative_encounter_system";

        private readonly NarrativeEncounterState _state;
        private readonly List<EncounterDefinition> _catalog = new List<EncounterDefinition>();

        public event Action<EncounterDefinition> OnEncounterSelected;
        public event Action<EncounterResolutionRecord> OnEncounterResolved;
        public event Action<NarrativeEncounterState> OnStateChanged;

        /// <summary>
        /// Optional weather gate filter. When set, encounters whose IDs match
        /// blocked weather gates are excluded from selection. The delegate
        /// receives an encounter ID and returns true if the encounter is
        /// blocked by current weather conditions. Plan 48 integration point.
        /// </summary>
        public Func<string, bool>? WeatherGateFilter { get; set; }

        /// <summary>
        /// Optional content-utilization instrumentation (Ticket #127). Null
        /// during normal gameplay (side-effect free, zero overhead); set by
        /// diagnostic/self-test harnesses that want SELECTED/EFFECT_PRODUCED
        /// evidence sourced from this system's own real selection and
        /// resolution logic rather than a hand-authored literal.
        /// </summary>
        public ContentUtilizationInstrumentation? Instrumentation { get; set; }

        public NarrativeEncounterSystem(NarrativeEncounterState? state = null)
        {
            _state = state ?? new NarrativeEncounterState();
            if (_state.history == null) _state.history = new List<EncounterResolutionRecord>();
        }

        public NarrativeEncounterState State => _state;
        public IReadOnlyList<EncounterDefinition> Catalog => _catalog;
        public int TotalResolved => _state.totalResolved;

        // ── Catalog ────────────────────────────────────────────────────

        public void RegisterEncounter(EncounterDefinition def)
        {
            if (def == null || string.IsNullOrEmpty(def.id)) return;
            if (_catalog.Exists(e => e.id == def.id)) return;
            _catalog.Add(def);
        }

        public void RegisterRange(IEnumerable<EncounterDefinition> defs)
        {
            if (defs == null) return;
            foreach (var def in defs) RegisterEncounter(def);
        }

        public EncounterDefinition? Find(string encounterId)
        {
            for (int i = 0; i < _catalog.Count; i++)
                if (_catalog[i].id == encounterId) return _catalog[i];
            return null;
        }

        // ── Weighted selection (Unity formula) ─────────────────────────

        /// <summary>Pick an eligible encounter by weight, or null when none
        /// qualify for this stance/danger/location.</summary>
        public EncounterDefinition? SelectEncounter(
            string stance, float dangerLevel, string locationId, ISeededRng rng)
        {
            if (rng == null) return null;

            // Pass 1: sum only eligible, non-filtered weights
            double total = 0d;
            for (int i = 0; i < _catalog.Count; i++)
            {
                double w = _catalog[i].GetEffectiveWeight(stance, dangerLevel, locationId);
                if (w <= 0d) continue;
                if (WeatherGateFilter != null && WeatherGateFilter(_catalog[i].id)) continue;
                total += w;
            }
            if (total <= 0d) return null;

            // Pass 2: roll against filtered total
            double roll = rng.NextDouble() * total;
            double acc = 0d;
            for (int i = 0; i < _catalog.Count; i++)
            {
                var def = _catalog[i];
                double weight = def.GetEffectiveWeight(stance, dangerLevel, locationId);
                if (weight <= 0d) continue;
                if (WeatherGateFilter != null && WeatherGateFilter(def.id)) continue;
                acc += weight;
                if (roll < acc)
                {
                    OnEncounterSelected?.Invoke(def);
                    Instrumentation?.RecordDefinitionSelected(
                        NarrativeEncounterCatalogLoader.FileName, def.id, nameof(NarrativeEncounterSystem));
                    return def;
                }
            }
            return null;
        }

        // ── Resolution ─────────────────────────────────────────────────

        public bool Resolve(string encounterId, string choiceId, string locationId, int day)
        {
            var def = Find(encounterId);
            if (def == null) return false;
            var choice = FindChoice(def, choiceId);
            if (choice == null) return false;

            var record = new EncounterResolutionRecord
            {
                encounterId = encounterId,
                choiceId = choiceId,
                locationId = locationId ?? string.Empty,
                day = day,
                moraleDelta = choice.moraleDelta,
                guiltDelta = choice.guiltDelta
            };
            _state.history.Add(record);
            _state.totalResolved++;
            _state.cumulativeMorale += choice.moraleDelta;
            _state.cumulativeGuilt += choice.guiltDelta;
            OnEncounterResolved?.Invoke(record);
            Instrumentation?.RecordDefinitionConsumed(
                NarrativeEncounterCatalogLoader.FileName, encounterId, nameof(NarrativeEncounterSystem),
                $"morale{(choice.moraleDelta >= 0 ? "+" : "")}{choice.moraleDelta:0.##} guilt{(choice.guiltDelta >= 0 ? "+" : "")}{choice.guiltDelta:0.##}",
                day);
            RaiseChanged();
            return true;
        }

        // ── Pending surfaced queue ─────────────────────────────────────
        // The host enqueues on surface and clears after the player has
        // acknowledged a choice. Resolve() deliberately does NOT auto-clear:
        // the pending list must mirror what the player actually acknowledged,
        // not what Core happened to record.

        /// <summary>Append a surfaced-but-unresolved encounter to the pending queue.</summary>
        public void EnqueuePending(string encounterId, string locationId, int legIndex, int day)
        {
            if (string.IsNullOrEmpty(encounterId)) return;
            if (_state.pending == null) _state.pending = new List<PendingSurfacedEncounter>();
            _state.pending.Add(new PendingSurfacedEncounter
            {
                encounterId = encounterId,
                locationId = locationId ?? string.Empty,
                legIndex = legIndex,
                day = day
            });
            RaiseChanged();
        }

        /// <summary>Remove every pending entry for this encounter id. No-op when absent.</summary>
        public void ClearPending(string encounterId)
        {
            if (_state.pending == null || string.IsNullOrEmpty(encounterId)) return;
            bool removed = false;
            for (int i = _state.pending.Count - 1; i >= 0; i--)
            {
                if (_state.pending[i] != null && _state.pending[i].encounterId == encounterId)
                {
                    _state.pending.RemoveAt(i);
                    removed = true;
                }
            }
            if (removed) RaiseChanged();
        }

        /// <summary>Drop the whole pending queue without resolving anything.</summary>
        public void ClearAllPending()
        {
            if (_state.pending == null || _state.pending.Count == 0) return;
            _state.pending.Clear();
            RaiseChanged();
        }

        private static EncounterChoiceDefinition? FindChoice(EncounterDefinition def, string choiceId)
        {
            for (int i = 0; i < def.choices.Count; i++)
                if (def.choices[i].choiceId == choiceId) return def.choices[i];
            return null;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public NarrativeEncounterState CaptureState()
        {
            var copy = new NarrativeEncounterState
            {
                systemId = _state.systemId,
                totalResolved = _state.totalResolved,
                cumulativeMorale = _state.cumulativeMorale,
                cumulativeGuilt = _state.cumulativeGuilt,
                pending = new List<PendingSurfacedEncounter>()
            };
            var ordered = new List<EncounterResolutionRecord>(_state.history);
            ordered.Sort((a, b) =>
            {
                int byDay = a.day.CompareTo(b.day);
                if (byDay != 0) return byDay;
                int byEnc = string.CompareOrdinal(a.encounterId, b.encounterId);
                return byEnc != 0 ? byEnc : string.CompareOrdinal(a.choiceId, b.choiceId);
            });
            for (int i = 0; i < ordered.Count; i++)
            {
                var r = ordered[i];
                copy.history.Add(new EncounterResolutionRecord
                {
                    encounterId = r.encounterId,
                    choiceId = r.choiceId,
                    locationId = r.locationId,
                    day = r.day,
                    moraleDelta = r.moraleDelta,
                    guiltDelta = r.guiltDelta
                });
            }
            if (_state.pending != null)
            {
                for (int i = 0; i < _state.pending.Count; i++)
                    copy.pending.Add(_state.pending[i]);
            }
            return copy;
        }

        public void RestoreState(NarrativeEncounterState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _state.totalResolved = saved.totalResolved;
            _state.cumulativeMorale = saved.cumulativeMorale;
            _state.cumulativeGuilt = saved.cumulativeGuilt;
            _state.history.Clear();
            if (saved.history != null)
            {
                for (int i = 0; i < saved.history.Count; i++)
                {
                    var r = saved.history[i];
                    if (r == null || string.IsNullOrEmpty(r.encounterId)) continue;
                    _state.history.Add(new EncounterResolutionRecord
                    {
                        encounterId = r.encounterId,
                        choiceId = r.choiceId,
                        locationId = r.locationId,
                        day = r.day,
                        moraleDelta = r.moraleDelta,
                        guiltDelta = r.guiltDelta
                    });
                }
            }
            _state.pending = saved.pending != null
                ? new List<PendingSurfacedEncounter>(saved.pending)
                : new List<PendingSurfacedEncounter>();
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }

    /// <summary>Engine-agnostic loader for narrative_encounters.json.</summary>
    public static class NarrativeEncounterCatalogLoader
    {
        public const string FileName = "narrative_encounters.json";

        public static List<EncounterDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new List<EncounterDefinition>();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return result;

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
                return result;

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return result;

            try
            {
                var parsed = CatalogLocator.LoadWrappedList<EncounterDefinition>(raw, SystemTextJsonSerializer.Options).ToArray();
                if (parsed == null) return result;
                for (int i = 0; i < parsed.Length; i++)
                {
                    if (parsed[i] == null || string.IsNullOrEmpty(parsed[i].id)) continue;
                    if (parsed[i].choices == null) parsed[i].choices = new List<EncounterChoiceDefinition>();
                    result.Add(parsed[i]);
                }
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn(path, "EncounterDefinition list", ex_CATDIAG);
                return result;
            }
            return result;
        }
    }
}
