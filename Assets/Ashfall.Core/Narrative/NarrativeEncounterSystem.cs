using System;
using System.Collections.Generic;
#pragma warning disable CS8618

using Ashfall.Core.IO;
using Ashfall.Core.Content;
namespace Ashfall.Core.Narrative
{
    /// <summary>
    /// F1/F2/F3/F4 — structured consequence payload returned by a successful
    /// encounter resolution. Narrative Core decides what the choice meant; the
    /// Host applies each effect through the subsystem that owns it (expedition
    /// loot / shelter inventory / journal / location authority). Deterministic:
    /// derived entirely from the catalog, state, and input — no RNG.
    /// </summary>
    public sealed class NarrativeEncounterResolutionResult
    {
        public string EncounterId = string.Empty;
        public string ChoiceId = string.Empty;
        public string LocationId = string.Empty;
        public int Day;

        public int MoraleDelta;
        public int GuiltDelta;

        /// <summary>True when the resolved choice marks the encounter depleted
        /// (one-time micro-location). The whole encounter is exhausted.</summary>
        public bool DepletesEncounter;

        /// <summary>Signed item delta: &gt;0 grant, &lt;0 removal (offerings), 0 none.
        /// Empty item id with a zero quantity means "no item effect".</summary>
        public string GrantItemId = string.Empty;
        public int GrantItemQuantity;

        /// <summary>Journal/codex knowledge key to unlock. Empty = none.</summary>
        public string JournalUnlockId = string.Empty;

        /// <summary>Location ID to discover. Empty = none.</summary>
        public string DiscoverLocationId = string.Empty;

        /// <summary>Campaign flag to set. Empty = none.</summary>
        public string SetWorldFlagId = string.Empty;

        /// <summary>Stable resolution identity for exactly-once Host application:
        /// encounterId:choiceId:day:locationId. Not a hash — deterministic text.</summary>
        public string ResolutionId =>
            $"{EncounterId}:{ChoiceId}:{Day}:{LocationId}";
    }

    /// <summary>
    /// Engine-agnostic port of the Unity narrative-encounter layer
    /// (EncounterSO selection math + NarrativeEncounters factories, now
    /// data-driven). Owns the catalog, weighted encounter selection per the
    /// Unity formula (stance multipliers, danger/location filters), depletion
    /// (F1), and a save/load-safe resolution history. All rolls through
    /// ISeededRng. Cross-system consequences (items, journal, locations) are
    /// returned to the Host as a structured payload — never mutated here.
    /// </summary>
    public class NarrativeEncounterSystem
    {
        public const string SystemId = "narrative_encounter_system";

        private readonly NarrativeEncounterState _state;
        private readonly List<EncounterDefinition> _catalog = new List<EncounterDefinition>();

        /// <summary>F1 — depleted encounter IDs. O(1) lookup; bounded by the
        /// authored depletable encounter set. Never exposed for mutation.</summary>
        private readonly HashSet<string> _depletedEncounters = new HashSet<string>(StringComparer.Ordinal);

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
        /// Plan 52 — optional expansion-quest link. When set, a resolved
        /// choice carrying <c>completesQuestId</c> records the decision as
        /// expansion-quest progress (including the recorded choice id), which
        /// is the persisted memory recurring-NPC arcs resolve from. Null
        /// leaves encounter resolution exactly as before.
        /// </summary>
        public ExpansionQuestSystem? QuestLink { get; set; }

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
            if (_state.depletedEncounterIds != null)
            {
                for (int i = 0; i < _state.depletedEncounterIds.Count; i++)
                {
                    var id = _state.depletedEncounterIds[i];
                    if (!string.IsNullOrEmpty(id)) _depletedEncounters.Add(id);
                }
            }
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

        /// <summary>F1 — true when the encounter's depleting choice has
        /// already been resolved: the micro-location is exhausted and the
        /// encounter must not reappear.</summary>
        public bool IsDepleted(string encounterId)
        {
            if (string.IsNullOrEmpty(encounterId)) return false;
            return _depletedEncounters.Contains(encounterId);
        }

        /// <summary>Number of depleted encounters (diagnostics; bounded by the
        /// authored depletable set).</summary>
        public int DepletedCount => _depletedEncounters.Count;

        /// <summary>
        /// Returns all eligible narrative encounter candidates and their effective weights
        /// for the given stance, danger level, and location without consuming any RNG.
        /// </summary>
        public List<(EncounterDefinition def, double weight)> GetEligibleCandidates(
            string stance, float dangerLevel, string locationId)
        {
            var candidates = new List<(EncounterDefinition, double)>();
            for (int i = 0; i < _catalog.Count; i++)
            {
                var def = _catalog[i];
                if (IsDepleted(def.id)) continue;
                double w = def.GetEffectiveWeight(stance, dangerLevel, locationId);
                if (w <= 0d) continue;
                if (WeatherGateFilter != null && WeatherGateFilter(def.id)) continue;
                candidates.Add((def, w));
            }
            return candidates;
        }

        public void RecordEncounterSelected(EncounterDefinition def)
        {
            if (def == null) return;
            OnEncounterSelected?.Invoke(def);
            Instrumentation?.RecordDefinitionSelected(
                NarrativeEncounterCatalogLoader.FileName, def.id, nameof(NarrativeEncounterSystem));
        }

        /// <summary>Pick an eligible encounter by weight, or null when none
        /// qualify for this stance/danger/location. F1: depleted encounters are
        /// excluded before weighting, so they neither distort the weight sum
        /// nor consume deterministic RNG rolls as zero-weight candidates.</summary>
        public EncounterDefinition? SelectEncounter(
            string stance, float dangerLevel, string locationId, ISeededRng rng)
        {
            if (rng == null) return null;

            var candidates = GetEligibleCandidates(stance, dangerLevel, locationId);
            if (candidates.Count == 0) return null;

            double total = 0d;
            for (int i = 0; i < candidates.Count; i++) total += candidates[i].weight;
            if (total <= 0d) return null;

            double roll = rng.NextDouble() * total;
            double acc = 0d;
            for (int i = 0; i < candidates.Count; i++)
            {
                acc += candidates[i].weight;
                if (roll < acc)
                {
                    RecordEncounterSelected(candidates[i].def);
                    return candidates[i].def;
                }
            }
            return null;
        }

        // ── Resolution ─────────────────────────────────────────────────

        /// <summary>Morale/guilt-only resolution retained for existing callers.
        /// Returns true when the resolution committed. New Host flows should
        /// prefer <see cref="TryResolve"/> for the full consequence payload.</summary>
        public bool Resolve(string encounterId, string choiceId, string locationId, int day)
            => TryResolve(encounterId, choiceId, locationId, day) != null;

        /// <summary>
        /// F1–F4 — validate the encounter and choice, append exactly one
        /// resolution record, apply depletion when the choice depletes, and
        /// return the full consequence payload. Validation precedes mutation:
        /// an unknown encounter or choice never touches state. The Host applies
        /// the returned item/journal/location effects through their owning
        /// subsystems; this method never does.
        /// </summary>
        public NarrativeEncounterResolutionResult? TryResolve(
            string encounterId, string choiceId, string locationId, int day)
        {
            var def = Find(encounterId);
            if (def == null) return null;
            var choice = FindChoice(def, choiceId);
            if (choice == null) return null;

            bool depletes = choice.depletesOnResolve;

            // F1: a depleting choice marks the whole encounter exhausted —
            // even if loot capacity later rejects the grant (the site was
            // searched; that it happened is not undone by a full pack).
            if (depletes) _depletedEncounters.Add(encounterId);

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
            ApplyQuestLink(choice, day);

            var result = new NarrativeEncounterResolutionResult
            {
                EncounterId = encounterId,
                ChoiceId = choiceId,
                LocationId = locationId ?? string.Empty,
                Day = day,
                MoraleDelta = choice.moraleDelta,
                GuiltDelta = choice.guiltDelta,
                DepletesEncounter = depletes,
                GrantItemId = choice.grantItemId ?? string.Empty,
                GrantItemQuantity = choice.grantItemQuantity,
                JournalUnlockId = choice.journalUnlockId ?? string.Empty,
                DiscoverLocationId = choice.discoverLocationId ?? string.Empty,
                SetWorldFlagId = choice.setWorldFlag ?? string.Empty
            };

            OnEncounterResolved?.Invoke(record);
            Instrumentation?.RecordDefinitionConsumed(
                NarrativeEncounterCatalogLoader.FileName, encounterId, nameof(NarrativeEncounterSystem),
                $"morale{(choice.moraleDelta >= 0 ? "+" : "")}{choice.moraleDelta:0.##} guilt{(choice.guiltDelta >= 0 ? "+" : "")}{choice.guiltDelta:0.##}",
                day);
            RaiseChanged();
            return result;
        }

        /// <summary>
        /// Plan 52 — land an arc decision into the expansion-quest ledger.
        /// Idempotent and order-safe: starts the quest if its day window has
        /// not auto-started it yet, records the authored choice, then completes
        /// it unless a choice effect already did. Quest progress is the
        /// persisted arc-memory authority — this bridge writes nothing else.
        /// </summary>
        private void ApplyQuestLink(EncounterChoiceDefinition choice, int day)
        {
            if (QuestLink == null || string.IsNullOrEmpty(choice.completesQuestId)) return;

            string questId = choice.completesQuestId;
            if (!QuestLink.IsStarted(questId))
                QuestLink.StartQuest(questId, day);

            if (!string.IsNullOrEmpty(choice.completesQuestChoiceId))
                QuestLink.MakeChoice(questId, choice.completesQuestChoiceId, day);

            if (!QuestLink.IsCompleted(questId))
                QuestLink.CompleteQuest(questId, day);
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
                pending = new List<PendingSurfacedEncounter>(),
                depletedEncounterIds = CaptureDepletedIds()
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

        /// <summary>F1 — ordinal-sorted depletion snapshot. HashSet iteration
        /// order is not a cross-host guarantee and the checksum walks the array.</summary>
        private List<string> CaptureDepletedIds()
        {
            var ids = new List<string>(_depletedEncounters.Count);
            ids.AddRange(_depletedEncounters);
            ids.Sort(string.CompareOrdinal);
            return ids;
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

            // F1 restore: clear, then rebuild deterministically. A present list
            // (even empty) is authoritative — a campaign that resolved nothing
            // depleting must not drift as the catalog evolves. A null list
            // marks a legacy save that predates depletion: reconstruct the set
            // from history so pre-feature depleting choices stay exhausted.
            _depletedEncounters.Clear();
            if (saved.depletedEncounterIds != null)
            {
                var ids = new List<string>(saved.depletedEncounterIds);
                ids.Sort(string.CompareOrdinal);
                for (int i = 0; i < ids.Count; i++)
                {
                    if (!string.IsNullOrEmpty(ids[i])) _depletedEncounters.Add(ids[i]);
                }
            }
            else
            {
                ReconstructDepletionFromHistory();
            }

            _state.depletedEncounterIds = CaptureDepletedIds();
            RaiseChanged();
        }

        /// <summary>
        /// F1 legacy migration (§48 of the integration plan): walk the saved
        /// resolution history, resolve each recorded choice against the current
        /// catalog, and re-mark every encounter whose recorded choice depletes.
        /// Deterministic. Unknown historical encounters/choices are skipped —
        /// never guessed. Runs only when a legacy save carries no depletion
        /// list; never on ordinary restore.
        /// </summary>
        private void ReconstructDepletionFromHistory()
        {
            if (_state.history == null) return;
            for (int i = 0; i < _state.history.Count; i++)
            {
                var r = _state.history[i];
                if (r == null || string.IsNullOrEmpty(r.encounterId)) continue;
                var def = Find(r.encounterId);
                if (def == null) continue; // unknown historical encounter — skip, do not guess
                var choice = FindChoice(def, r.choiceId);
                if (choice == null) continue; // unknown historical choice — skip, do not guess
                if (choice.depletesOnResolve) _depletedEncounters.Add(r.encounterId);
            }
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }

    /// <summary>Engine-agnostic loader for narrative_encounters.json.</summary>
    public static class NarrativeEncounterCatalogLoader
    {
        public const string FileName = "narrative_encounters.json";

        /// <summary>Plan 52 — recurring-NPC arc encounters load after the
        /// base catalog (duplicate ids are dropped by RegisterEncounter).</summary>
        public const string ArcFileName = "narrative_encounters_npc_arcs.json";

        /// <summary>GAP-49B — destination-bound approach micro-locations
        /// (Plan 76 §35) load last through the same schema; their
        /// `requiredLocationId` makes them weight-zero off-destination.</summary>
        public const string MicroLocationsFileName = "micro_locations.json";

        public static List<EncounterDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new List<EncounterDefinition>();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return result;

            result.AddRange(LoadFile(dataDir, FileName, fileIO, json));
            result.AddRange(LoadFile(dataDir, ArcFileName, fileIO, json));
            result.AddRange(LoadFile(dataDir, MicroLocationsFileName, fileIO, json));
            return result;
        }

        private static List<EncounterDefinition> LoadFile(
            string dataDir, string fileName, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new List<EncounterDefinition>();

            string path = fileIO.Combine(dataDir, fileName);
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

                    // F6 §6.3 seal — the dedicated MicroLocationEncounterLoader stamps
                    // marker + source file, but production loads through this shared
                    // LoadFile. Apply the same stamp here so every definition from
                    // micro_locations.json carries the metadata its consumers expect;
                    // other catalog files keep their defaults.
                    if (fileName == MicroLocationsFileName)
                    {
                        parsed[i].isMicroLocation = true;
                        parsed[i].sourceFile = MicroLocationsFileName;
                    }
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
