// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>Tasks 5–8 §6.8 — raised when a manual/collectible reveal adds a
        /// node to unlockedIds. Panels observe this; collectible code never
        /// touches panel rows directly.</summary>
        public event Action<string>? OnManualUnlocked;

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
            bool isNew = !State.unlockedIds.Contains(id);
            if (isNew) State.unlockedIds.Add(id);
            if (_catalog.TryGetValue(id, out var def)) def.isUnlocked = true;
            if (isNew) OnManualUnlocked?.Invoke(id);
        }

        public bool IsManualUnlocked(string id) => !string.IsNullOrEmpty(id) && State.unlockedIds.Contains(id);

        /// <summary>
        /// Stable capability-query contract (B5–B8 Phase 1). Consumer systems
        /// (greenhouse, power builds, defense builds, water sources) ask this
        /// single question instead of reaching into <see cref="IsManualUnlocked"/>
        /// or caching unlock truth locally.
        ///
        /// Capability ≠ infrastructure: a <c>true</c> here gates what a player
        /// may BUILD — it never grants the built instance, never applies an
        /// invisible multiplier, and never substitutes for a canonical
        /// item/recipe cost. Behaviorally identical to <see cref="IsManualUnlocked"/>.
        /// </summary>
        public bool HasCapability(string knowledgeId) => IsManualUnlocked(knowledgeId);

        /// <summary>
        /// Evaluates research eligibility for a given knowledge ID under the current state.
        /// Pure projection without state mutations.
        /// </summary>
        public ResearchEligibility GetEligibility(string id)
        {
            if (string.IsNullOrEmpty(id) || !_catalog.TryGetValue(id, out var def))
            {
                return new ResearchEligibility(id, canStart: false, isDiscovered: false, isCompleted: false, isActive: false, null, null, ResearchEligibilityCode.UnknownNode);
            }

            bool isCompleted = def.isCompleted || State.completedIds.Contains(id);
            if (isCompleted)
            {
                return new ResearchEligibility(id, canStart: false, isDiscovered: true, isCompleted: true, isActive: false, null, null, ResearchEligibilityCode.AlreadyCompleted);
            }

            bool isActive = string.Equals(State.activeResearchId, id, StringComparison.Ordinal);
            if (isActive)
            {
                return new ResearchEligibility(id, canStart: false, isDiscovered: true, isCompleted: false, isActive: true, null, null, ResearchEligibilityCode.AlreadyActive);
            }

            // Prerequisite gate
            var missing = new List<string>();
            if (def.prerequisites != null)
            {
                for (int i = 0; i < def.prerequisites.Length; i++)
                {
                    string p = def.prerequisites[i];
                    if (!_catalog.TryGetValue(p, out var pdef) || !pdef.isCompleted)
                    {
                        missing.Add(p);
                    }
                }
            }

            if (missing.Count > 0)
            {
                return new ResearchEligibility(id, canStart: false, isDiscovered: true, isCompleted: false, isActive: false, missing, null, ResearchEligibilityCode.MissingPrerequisites);
            }

            // Prereqs are satisfied. Check if another node is currently active.
            if (!string.IsNullOrEmpty(State.activeResearchId) && !string.Equals(State.activeResearchId, id, StringComparison.Ordinal))
            {
                return new ResearchEligibility(id, canStart: false, isDiscovered: true, isCompleted: false, isActive: false, null, State.activeResearchId, ResearchEligibilityCode.AnotherResearchActive);
            }

            return new ResearchEligibility(id, canStart: true, isDiscovered: true, isCompleted: false, isActive: false, null, null, ResearchEligibilityCode.Eligible);
        }

        /// <summary>
        /// Begin researching a knowledge node. Fails if the node is not
        /// registered, already completed, active, or its prerequisites are unmet.
        /// </summary>
        public bool StartResearch(string id, int day)
        {
            var eligibility = GetEligibility(id);
            if (!eligibility.CanStart)
            {
                _log.Warn($"[Research] cannot start '{id}': {eligibility.Code}");
                return false;
            }

            var def = _catalog[id];
            // Mark the node as unlocked the first time it is queued.
            if (!def.isUnlocked)
            {
                def.isUnlocked = true;
                if (!State.unlockedIds.Contains(id))
                    State.unlockedIds.Add(id);
            }

            State.activeResearchId = id;
            State.activeResearchDays = 0;
            State.currentDay = day;
            _log.Info($"[Research] started '{id}' on day {day}");
            return true;
        }

        /// <summary>Remaining days to complete research for a node.</summary>
        public int GetDaysRemaining(string id)
        {
            if (string.IsNullOrEmpty(id) || !_catalog.TryGetValue(id, out var def)) return 0;
            if (def.isCompleted || State.completedIds.Contains(id)) return 0;
            if (string.Equals(State.activeResearchId, id, StringComparison.Ordinal))
            {
                return Math.Max(0, def.daysToComplete - State.activeResearchDays);
            }
            return def.daysToComplete;
        }

        /// <summary>Returns eligible research nodes currently unstarted with prerequisites met.</summary>
        public IReadOnlyList<ResearchKnowledgeDef> GetAvailableNodes()
        {
            var list = new List<ResearchKnowledgeDef>();
            foreach (var kvp in _catalog)
            {
                var def = kvp.Value;
                if (def.isCompleted || State.completedIds.Contains(def.id)) continue;
                if (string.Equals(State.activeResearchId, def.id, StringComparison.Ordinal)) continue;

                bool prereqsMet = true;
                if (def.prerequisites != null)
                {
                    for (int i = 0; i < def.prerequisites.Length; i++)
                    {
                        string p = def.prerequisites[i];
                        if (!_catalog.TryGetValue(p, out var pd) || (!pd.isCompleted && !State.completedIds.Contains(p)))
                        {
                            prereqsMet = false;
                            break;
                        }
                    }
                }
                if (prereqsMet)
                    list.Add(def);
            }
            list.Sort((a, b) =>
            {
                int cat = string.Compare(a.category, b.category, StringComparison.Ordinal);
                return cat != 0 ? cat : string.Compare(a.id, b.id, StringComparison.Ordinal);
            });
            return list;
        }

        /// <summary>Returns locked research nodes that are missing prerequisites.</summary>
        public IReadOnlyList<ResearchKnowledgeDef> GetLockedNodes()
        {
            var list = new List<ResearchKnowledgeDef>();
            foreach (var kvp in _catalog)
            {
                var def = kvp.Value;
                if (def.isCompleted || State.completedIds.Contains(def.id)) continue;
                if (string.Equals(State.activeResearchId, def.id, StringComparison.Ordinal)) continue;

                bool missingPrereqs = false;
                if (def.prerequisites != null)
                {
                    for (int i = 0; i < def.prerequisites.Length; i++)
                    {
                        string p = def.prerequisites[i];
                        if (!_catalog.TryGetValue(p, out var pd) || (!pd.isCompleted && !State.completedIds.Contains(p)))
                        {
                            missingPrereqs = true;
                            break;
                        }
                    }
                }
                if (missingPrereqs)
                    list.Add(def);
            }
            list.Sort((a, b) =>
            {
                int cat = string.Compare(a.category, b.category, StringComparison.Ordinal);
                return cat != 0 ? cat : string.Compare(a.id, b.id, StringComparison.Ordinal);
            });
            return list;
        }

        /// <summary>Returns all completed research nodes.</summary>
        public IReadOnlyList<ResearchKnowledgeDef> GetCompletedNodes()
        {
            var list = new List<ResearchKnowledgeDef>();
            foreach (var kvp in _catalog)
            {
                var def = kvp.Value;
                if (def.isCompleted || State.completedIds.Contains(def.id))
                    list.Add(def);
            }
            list.Sort((a, b) =>
            {
                int cat = string.Compare(a.category, b.category, StringComparison.Ordinal);
                return cat != 0 ? cat : string.Compare(a.id, b.id, StringComparison.Ordinal);
            });
            return list;
        }

        /// <summary>
        /// Finds all downstream dependent nodes that require <paramref name="id"/> as a prerequisite
        /// directly or indirectly. Handles diamond dependencies without duplicates and returns
        /// sorted ordinally.
        /// </summary>
        public IReadOnlyList<string> GetDependents(string id)
        {
            if (string.IsNullOrEmpty(id) || !_catalog.ContainsKey(id))
                return Array.Empty<string>();

            var forwardMap = new Dictionary<string, List<string>>(StringComparer.Ordinal);
            foreach (var kvp in _catalog)
            {
                if (kvp.Value.prerequisites != null)
                {
                    foreach (var p in kvp.Value.prerequisites)
                    {
                        if (!forwardMap.TryGetValue(p, out var children))
                        {
                            children = new List<string>();
                            forwardMap[p] = children;
                        }
                        children.Add(kvp.Key);
                    }
                }
            }

            var visited = new HashSet<string>(StringComparer.Ordinal);
            var queue = new Queue<string>();

            if (forwardMap.TryGetValue(id, out var directChildren))
            {
                foreach (var child in directChildren)
                {
                    if (visited.Add(child))
                        queue.Enqueue(child);
                }
            }

            while (queue.Count > 0)
            {
                string curr = queue.Dequeue();
                if (forwardMap.TryGetValue(curr, out var nextChildren))
                {
                    foreach (var child in nextChildren)
                    {
                        if (visited.Add(child))
                            queue.Enqueue(child);
                    }
                }
            }

            var result = new List<string>(visited);
            result.Sort(StringComparer.Ordinal);
            return result;
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

        /// <summary>
        /// Adds research points from a named producer. The source is retained
        /// in logs/events by the caller; the wallet itself remains owned by
        /// this system. Invalid or non-positive grants are rejected.
        /// </summary>
        public bool TryAddResearchPoints(int amount, string sourceId)
        {
            if (amount <= 0) return false;
            if (State.researchPointsAvailable > int.MaxValue - amount)
                return false;

            State.researchPointsAvailable += amount;
            State.researchPointsLifetimeEarned += amount;
            return true;
        }

        public bool TrySpendResearchPoints(int amount, string purposeId)
        {
            if (amount <= 0 || amount > State.researchPointsAvailable)
                return false;

            State.researchPointsAvailable -= amount;
            return true;
        }

        public int GetResearchPoints() => Math.Max(0, State.researchPointsAvailable);

        public BlueprintProgressState? GetBlueprintProgress(string blueprintId)
        {
            if (string.IsNullOrWhiteSpace(blueprintId) || State.blueprintProgress == null)
                return null;

            return State.blueprintProgress.Find(p =>
                p != null && string.Equals(p.blueprintId, blueprintId, StringComparison.Ordinal));
        }

        public bool IsBlueprintUnlocked(string blueprintId)
        {
            var progress = GetBlueprintProgress(blueprintId);
            return progress != null && string.Equals(progress.discoveryState, "unlocked", StringComparison.Ordinal);
        }

        /// <summary>
        /// Applies bounded progress to an authored blueprint. Completion is a
        /// transition and raises once; restore never calls this method.
        /// </summary>
        public bool TryAddBlueprintProgress(
            string blueprintId,
            int amount,
            int requiredPoints,
            int completedDay,
            string sourceTechId = "")
        {
            if (string.IsNullOrWhiteSpace(blueprintId) || amount <= 0 || requiredPoints <= 0)
                return false;

            State.blueprintProgress ??= new List<BlueprintProgressState>();
            var progress = GetBlueprintProgress(blueprintId);
            if (progress == null)
            {
                progress = new BlueprintProgressState
                {
                    blueprintId = blueprintId,
                    requiredPoints = requiredPoints,
                    discoveryState = "identified"
                };
                State.blueprintProgress.Add(progress);
            }

            progress.requiredPoints = Math.Max(progress.requiredPoints, requiredPoints);
            if (!string.IsNullOrWhiteSpace(sourceTechId)
                && !ContainsOrdinal(progress.sourceTechIds, sourceTechId))
            {
                progress.sourceTechIds.Add(sourceTechId);
            }

            if (IsBlueprintUnlocked(blueprintId)) return false;

            long next = (long)Math.Max(0, progress.progressPoints) + amount;
            progress.progressPoints = (int)Math.Min(progress.requiredPoints, next);
            progress.discoveryState = progress.progressPoints >= progress.requiredPoints
                ? "unlocked"
                : "in_progress";

            if (progress.progressPoints >= progress.requiredPoints)
            {
                progress.completedDay = completedDay;
                OnBlueprintUnlocked?.Invoke(progress);
            }

            return true;
        }

        private static bool ContainsOrdinal(List<string> values, string value)
        {
            if (values == null) return false;
            for (int i = 0; i < values.Count; i++)
                if (string.Equals(values[i], value, StringComparison.Ordinal)) return true;
            return false;
        }

        public event Action<BlueprintProgressState>? OnBlueprintUnlocked;

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

            var copy = new ResearchState
            {
                systemId = State.systemId,
                expansionUnlocked = State.expansionUnlocked,
                currentDay = State.currentDay,
                activeResearchId = State.activeResearchId ?? string.Empty,
                activeResearchDays = Math.Max(0, State.activeResearchDays),
                researchPointsAvailable = Math.Max(0, State.researchPointsAvailable),
                researchPointsLifetimeEarned = Math.Max(0, State.researchPointsLifetimeEarned),
                unlockedIds = new List<string>(State.unlockedIds ?? new List<string>()),
                completedIds = new List<string>(State.completedIds ?? new List<string>()),
                blueprintProgress = new List<BlueprintProgressState>()
            };
            if (State.blueprintProgress != null)
            {
                foreach (var progress in State.blueprintProgress)
                {
                    if (progress != null && !string.IsNullOrWhiteSpace(progress.blueprintId))
                        copy.blueprintProgress.Add(progress.Clone());
                }
            }
            return copy;
        }

        public void RestoreState(ResearchState saved)
        {
            if (saved == null) return;
            State = new ResearchState
            {
                systemId = saved.systemId ?? ResearchSystem.SystemId,
                expansionUnlocked = saved.expansionUnlocked,
                currentDay = saved.currentDay,
                activeResearchId = saved.activeResearchId ?? string.Empty,
                activeResearchDays = Math.Max(0, saved.activeResearchDays),
                researchPointsAvailable = Math.Max(0, saved.researchPointsAvailable),
                researchPointsLifetimeEarned = Math.Max(0, saved.researchPointsLifetimeEarned),
                unlockedIds = saved.unlockedIds != null ? new List<string>(saved.unlockedIds) : new List<string>(),
                completedIds = saved.completedIds != null ? new List<string>(saved.completedIds) : new List<string>(),
                blueprintProgress = new List<BlueprintProgressState>()
            };
            if (saved.blueprintProgress != null)
            {
                foreach (var progress in saved.blueprintProgress)
                {
                    if (progress != null && !string.IsNullOrWhiteSpace(progress.blueprintId))
                        State.blueprintProgress.Add(progress.Clone());
                }
            }
            // Push saved flags back into the catalog.
            foreach (var kv in _catalog)
            {
                kv.Value.isUnlocked = State.unlockedIds.Contains(kv.Key);
                kv.Value.isCompleted = State.completedIds.Contains(kv.Key);
            }
        }
    }
}
