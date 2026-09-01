using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// Research / R&D / Breakthrough engine. Coordinates a catalog of
    /// research knowledge nodes with prerequisite gating, day-progress
    /// ticks, and breakthrough-item awards. Engine-agnostic; mirrors
    /// the Phase-18 <see cref="Survivors.SkillProgressionSystem"/> shape.
    /// </summary>
    public sealed class ResearchSystem
    {
        public const string SystemId = "research_system";

        public ResearchState State { get; private set; }

        private readonly Dictionary<string, ResearchKnowledgeDef> _catalog =
            new Dictionary<string, ResearchKnowledgeDef>();
        private readonly ILog _log;

        public int CatalogCount => _catalog.Count;
        public IReadOnlyDictionary<string, ResearchKnowledgeDef> Catalog => _catalog;

        /// <summary>
        /// Raised once per node on the completed transition (never on restore,
        /// never for an already-completed node). Hosts use this to award the
        /// node's <see cref="ResearchKnowledgeDef.breakthroughItem"/>.
        /// </summary>
        public event Action<ResearchKnowledgeDef>? OnResearchCompleted;

        public ResearchSystem(ILog? log = null, ResearchState? state = null)
        {
            _log = log ?? NullLog.Instance;
            State = state ?? new ResearchState();
        }

        /// <summary>Register a knowledge node in the catalog.</summary>
        public void Register(ResearchKnowledgeDef def)
        {
            if (def == null || string.IsNullOrEmpty(def.id)) return;
            if (_catalog.ContainsKey(def.id))
            {
                _log.Warn("[Research] duplicate registration: " + def.id);
                return;
            }
            // Mirror state flags from save.
            if (State.unlockedIds.Contains(def.id)) def.isUnlocked = true;
            if (State.completedIds.Contains(def.id)) def.isCompleted = true;
            _catalog[def.id] = def;
        }

        public void UnlockManual(string id)
        {
            if (string.IsNullOrEmpty(id)) return;
            if (!State.unlockedIds.Contains(id)) State.unlockedIds.Add(id);
            if (_catalog.TryGetValue(id, out var def)) def.isUnlocked = true;
        }

        public bool IsManualUnlocked(string id) => !string.IsNullOrEmpty(id) && State.unlockedIds.Contains(id);

        /// <summary>
        /// Begin researching a knowledge node. Fails if the node is not
        /// registered, already completed, or its prerequisites are unmet.
        /// </summary>
        public bool StartResearch(string id, int day)
        {
            if (string.IsNullOrEmpty(id)) return false;
            if (!_catalog.TryGetValue(id, out var def)) return false;
            if (def.isCompleted) return false;

            // Prerequisite gate.
            if (def.prerequisites != null)
            {
                for (int i = 0; i < def.prerequisites.Length; i++)
                {
                    string prereq = def.prerequisites[i];
                    if (!_catalog.TryGetValue(prereq, out var pdef) || !pdef.isCompleted)
                    {
                        _log.Warn($"[Research] prerequisite '{prereq}' not completed for '{id}'");
                        return false;
                    }
                }
            }

            // Mark the node as unlocked the first time it is queued.
            if (!def.isUnlocked)
            {
                def.isUnlocked = true;
                State.unlockedIds.Add(id);
            }

            State.activeResearchId = id;
            State.activeResearchDays = 0;
            State.currentDay = day;
            _log.Info($"[Research] started '{id}' on day {day}");
            return true;
        }

        /// <summary>
        /// Day-step hook. Advances the active research by
        /// (<paramref name="newDay"/> - currentDay) days and completes
        /// the node if the budget is exhausted.
        /// </summary>
        public void Tick(int newDay)
        {
            if (string.IsNullOrEmpty(State.activeResearchId)) return;
            int delta = newDay - State.currentDay;
            if (delta <= 0) return;
            State.currentDay = newDay;
            State.activeResearchDays += delta;

            if (!_catalog.TryGetValue(State.activeResearchId, out var def)) return;
            if (State.activeResearchDays >= def.daysToComplete)
            {
                CompleteResearch(def.id);
            }
        }

        /// <summary>Force-complete a research node (bypasses day budget).</summary>
        public bool CompleteResearch(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            if (!_catalog.TryGetValue(id, out var def)) return false;
            if (def.isCompleted) return false;

            def.isCompleted = true;
            State.completedIds.Add(id);
            if (State.activeResearchId == id)
            {
                State.activeResearchId = string.Empty;
                State.activeResearchDays = 0;
            }

            _log.Info($"[Research] completed '{id}' — breakthrough: {def.breakthroughItem ?? "(none)"}");
            OnResearchCompleted?.Invoke(def);
            return true;
        }

        /// <summary>Read-only: get the current active research def, or null if idle.</summary>
        public ResearchKnowledgeDef? GetActiveResearch()
        {
            if (string.IsNullOrEmpty(State.activeResearchId)) return null;
            _catalog.TryGetValue(State.activeResearchId, out var def);
            return def;
        }

        /// <summary>Read-only: get any registered knowledge node.</summary>
        public ResearchKnowledgeDef? GetKnowledge(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            _catalog.TryGetValue(id, out var def);
            return def;
        }

        public ResearchState CaptureState()
        {
            // Mirror flags back into state lists for the save envelope.
            // IDs present in the state lists but absent from the loaded catalog
            // (progress from a save whose catalog has since changed, or unlocks
            // granted by producers outside this catalog) are preserved verbatim:
            // Plan 34 §34D.5 — never silently discard unknown saved research.
            var unlocked = new List<string>();
            var completed = new List<string>();
            var catalogIds = new HashSet<string>();
            foreach (var kv in _catalog)
            {
                catalogIds.Add(kv.Key);
                if (kv.Value.isUnlocked) unlocked.Add(kv.Key);
                if (kv.Value.isCompleted) completed.Add(kv.Key);
            }
            foreach (var id in State.unlockedIds)
                if (!catalogIds.Contains(id) && !unlocked.Contains(id)) unlocked.Add(id);
            foreach (var id in State.completedIds)
                if (!catalogIds.Contains(id) && !completed.Contains(id)) completed.Add(id);
            State.unlockedIds.Clear();
            State.unlockedIds.AddRange(unlocked);
            State.completedIds.Clear();
            State.completedIds.AddRange(completed);
            return State;
        }

        public void RestoreState(ResearchState saved)
        {
            if (saved == null) return;
            State = saved;
            // Push saved flags back into the catalog.
            foreach (var kv in _catalog)
            {
                kv.Value.isUnlocked = State.unlockedIds.Contains(kv.Key);
                kv.Value.isCompleted = State.completedIds.Contains(kv.Key);
            }
        }
    }
}
