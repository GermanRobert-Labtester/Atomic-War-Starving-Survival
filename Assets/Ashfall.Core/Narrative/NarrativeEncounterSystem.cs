using System;
using System.Collections.Generic;

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

        public NarrativeEncounterSystem(NarrativeEncounterState state = null)
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

        public EncounterDefinition Find(string encounterId)
        {
            for (int i = 0; i < _catalog.Count; i++)
                if (_catalog[i].id == encounterId) return _catalog[i];
            return null;
        }

        // ── Weighted selection (Unity formula) ─────────────────────────

        /// <summary>Pick an eligible encounter by weight, or null when none
        /// qualify for this stance/danger/location.</summary>
        public EncounterDefinition SelectEncounter(
            string stance, float dangerLevel, string locationId, ISeededRng rng)
        {
            if (rng == null) return null;
            double total = 0d;
            for (int i = 0; i < _catalog.Count; i++)
                total += _catalog[i].GetEffectiveWeight(stance, dangerLevel, locationId);
            if (total <= 0d) return null;

            double roll = rng.NextDouble() * total;
            double acc = 0d;
            for (int i = 0; i < _catalog.Count; i++)
            {
                var def = _catalog[i];
                double weight = def.GetEffectiveWeight(stance, dangerLevel, locationId);
                if (weight <= 0d) continue;
                acc += weight;
                if (roll < acc)
                {
                    OnEncounterSelected?.Invoke(def);
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
            RaiseChanged();
            return true;
        }

        private static EncounterChoiceDefinition FindChoice(EncounterDefinition def, string choiceId)
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
                cumulativeGuilt = _state.cumulativeGuilt
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
                var parsed = json.Deserialize<EncounterDefinition[]>(raw);
                if (parsed == null) return result;
                for (int i = 0; i < parsed.Length; i++)
                {
                    if (parsed[i] == null || string.IsNullOrEmpty(parsed[i].id)) continue;
                    if (parsed[i].choices == null) parsed[i].choices = new List<EncounterChoiceDefinition>();
                    result.Add(parsed[i]);
                }
            }
            catch
            {
                return result;
            }
            return result;
        }
    }
}
