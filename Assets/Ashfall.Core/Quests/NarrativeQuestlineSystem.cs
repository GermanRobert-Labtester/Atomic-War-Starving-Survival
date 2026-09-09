// SPDX-License-Identifier: MIT
// ASHFALL Core: survivor narrative questline progression (Plan 104 runtime).

using System;
using System.Collections.Generic;

namespace Ashfall.Core.Quests
{
    /// <summary>
    /// Lifecycle of one survivor arc. <see cref="AwaitingBranch"/> is the crisis
    /// stage: objectives are exhausted and the arc cannot advance until the player
    /// picks one of the two authored branches.
    /// </summary>
    public enum NarrativeArcStatus
    {
        NotStarted = 0,
        Active = 1,
        AwaitingBranch = 2,
        Resolved = 3
    }

    /// <summary>
    /// Persisted state of one survivor arc. Plain serializable DTO.
    /// <para>
    /// Public fields rather than properties on purpose: <see cref="SaveChecksum"/>
    /// canonicalizes <c>BindingFlags.Public | BindingFlags.Instance</c> fields only,
    /// so a property-based DTO hashes to an empty object and its envelope checksum
    /// would silently stop detecting mutation. <c>SystemTextJsonSerializer</c> sets
    /// <c>IncludeFields = true</c>, so fields round-trip through JSON unchanged.
    /// </para>
    /// </summary>
    [Serializable]
    public sealed class NarrativeQuestlineArcState
    {
        public string questId = string.Empty;
        public string survivorId = string.Empty;
        public int currentStage;
        public NarrativeArcStatus status = NarrativeArcStatus.NotStarted;
        public List<string> deliveredItems = new List<string>();
        public string chosenBranchId = string.Empty;
        public string grantedTraitId = string.Empty;
        public int startedDay;
        public int resolvedDay;

        public NarrativeQuestlineArcState Clone()
        {
            return new NarrativeQuestlineArcState
            {
                questId = questId,
                survivorId = survivorId,
                currentStage = currentStage,
                status = status,
                deliveredItems = new List<string>(deliveredItems ?? new List<string>()),
                chosenBranchId = chosenBranchId,
                grantedTraitId = grantedTraitId,
                startedDay = startedDay,
                resolvedDay = resolvedDay
            };
        }
    }

    /// <summary>Aggregate persisted state for the narrative questline system.</summary>
    [Serializable]
    public sealed class NarrativeQuestlineSaveState
    {
        public int schema_version = 1;
        public string systemId = NarrativeQuestlineSystem.SystemId;
        public List<NarrativeQuestlineArcState> arcs = new List<NarrativeQuestlineArcState>();
    }

    /// <summary>
    /// Progression engine for the authored survivor arcs in
    /// <c>narrative_questlines.json</c>. One arc per survivor, a linear ladder of
    /// objective-item stages, then a single binary crisis fork whose choice grants
    /// a trait and a morale delta and ends the arc on its resolution epilogue.
    /// <para>
    /// Deterministic: the system owns no RNG. Advancement is driven entirely by
    /// authored data and player input, so a replayed input sequence over the same
    /// catalog produces an identical arc state and an identical checksum.
    /// </para>
    /// <para>
    /// Trait granting and morale application are deliberately NOT performed here:
    /// Core reports the granted trait id and morale delta, and the host applies
    /// them through the systems that already own survivor traits and shelter
    /// morale. This keeps one authority per mechanic.
    /// </para>
    /// </summary>
    public sealed class NarrativeQuestlineSystem
    {
        public const string SystemId = "narrative_questlines";

        private readonly Dictionary<string, NarrativeQuestlineDef> _byQuestId =
            new Dictionary<string, NarrativeQuestlineDef>(StringComparer.Ordinal);
        private readonly Dictionary<string, NarrativeQuestlineDef> _bySurvivorId =
            new Dictionary<string, NarrativeQuestlineDef>(StringComparer.Ordinal);
        private readonly Dictionary<string, NarrativeQuestlineArcState> _arcsBySurvivor =
            new Dictionary<string, NarrativeQuestlineArcState>(StringComparer.Ordinal);

        private readonly ILog? _log;
        private NarrativeQuestlineSaveState _state = new NarrativeQuestlineSaveState();
        private bool _restoring;

        public event Action<NarrativeQuestlineArcState>? OnArcStarted;
        public event Action<NarrativeQuestlineArcState, int>? OnStageAdvanced;
        public event Action<NarrativeQuestlineArcState, NarrativeQuestlineBranchDef>? OnBranchChosen;
        public event Action<NarrativeQuestlineArcState>? OnArcResolved;

        public NarrativeQuestlineSystem(IReadOnlyList<NarrativeQuestlineDef>? definitions, ILog? log = null)
        {
            _log = log;
            if (definitions != null)
            {
                for (int i = 0; i < definitions.Count; i++)
                {
                    var def = definitions[i];
                    if (def == null || string.IsNullOrEmpty(def.questId) || string.IsNullOrEmpty(def.survivorId))
                        continue;
                    if (!_byQuestId.ContainsKey(def.questId))
                        _byQuestId[def.questId] = def;
                    if (!_bySurvivorId.ContainsKey(def.survivorId))
                        _bySurvivorId[def.survivorId] = def;
                }
            }
        }

        public int DefinitionCount => _byQuestId.Count;

        public IReadOnlyList<NarrativeQuestlineDef> Definitions
        {
            get
            {
                var list = new List<NarrativeQuestlineDef>(_byQuestId.Count);
                foreach (var kv in _byQuestId) list.Add(kv.Value);
                list.Sort((a, b) => string.CompareOrdinal(a.questId, b.questId));
                return list;
            }
        }

        public NarrativeQuestlineDef? GetDefinition(string questId)
            => !string.IsNullOrEmpty(questId) && _byQuestId.TryGetValue(questId, out var d) ? d : null;

        public NarrativeQuestlineDef? GetDefinitionForSurvivor(string survivorId)
            => !string.IsNullOrEmpty(survivorId) && _bySurvivorId.TryGetValue(survivorId, out var d) ? d : null;

        public NarrativeQuestlineArcState? GetArc(string survivorId)
            => !string.IsNullOrEmpty(survivorId) && _arcsBySurvivor.TryGetValue(survivorId, out var a) ? a : null;

        public IReadOnlyList<NarrativeQuestlineArcState> Arcs
        {
            get
            {
                var list = new List<NarrativeQuestlineArcState>(_arcsBySurvivor.Values);
                list.Sort((a, b) => string.CompareOrdinal(a.questId, b.questId));
                return list;
            }
        }

        public bool HasArc(string survivorId) => GetArc(survivorId) != null;

        public bool IsAwaitingBranch(string survivorId)
            => GetArc(survivorId)?.status == NarrativeArcStatus.AwaitingBranch;

        /// <summary>Objective items still owed on the arc's current stage.</summary>
        public List<string> GetOutstandingObjectives(string survivorId)
        {
            var outstanding = new List<string>();
            var arc = GetArc(survivorId);
            if (arc == null || arc.status != NarrativeArcStatus.Active) return outstanding;
            var def = GetDefinition(arc.questId);
            var stage = def?.FindStage(arc.currentStage);
            if (stage == null) return outstanding;
            for (int i = 0; i < stage.objectiveItems.Count; i++)
            {
                var item = stage.objectiveItems[i];
                if (!arc.deliveredItems.Contains(item)) outstanding.Add(item);
            }
            return outstanding;
        }

        /// <summary>
        /// Open the arc for a survivor. Fails when the survivor has no authored
        /// questline or already has an arc in any state (an arc is never restarted).
        /// </summary>
        public bool TryBegin(string survivorId, int day)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            if (_arcsBySurvivor.ContainsKey(survivorId)) return false;
            if (!_bySurvivorId.TryGetValue(survivorId, out var def)) return false;

            int firstStage = def.stages.Count > 0 ? def.stages[0].stage : 0;
            var arc = new NarrativeQuestlineArcState
            {
                questId = def.questId,
                survivorId = survivorId,
                currentStage = firstStage,
                status = NarrativeArcStatus.Active,
                startedDay = day
            };

            // A questline whose opening stage is already the crisis fork has no
            // objectives to deliver, so it opens straight into the branch.
            var stage = def.FindStage(firstStage);
            if (stage != null && stage.HasBranch)
                arc.status = NarrativeArcStatus.AwaitingBranch;

            _arcsBySurvivor[survivorId] = arc;
            RebuildStateList();
            if (!_restoring) OnArcStarted?.Invoke(arc);
            return true;
        }

        /// <summary>
        /// Record one objective item delivered for a survivor's arc. Returns true
        /// only when the item was owed on the current stage and was accepted;
        /// duplicates and items belonging to other stages are refused without
        /// mutating state. Accepting the last owed item advances the stage.
        /// </summary>
        public bool TryDeliverItem(string survivorId, string itemId, int day)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(itemId)) return false;
            if (!_arcsBySurvivor.TryGetValue(survivorId, out var arc)) return false;
            if (arc.status != NarrativeArcStatus.Active) return false;

            var def = GetDefinition(arc.questId);
            if (def == null) return false;
            var stage = def.FindStage(arc.currentStage);
            if (stage == null) return false;
            if (!stage.objectiveItems.Contains(itemId)) return false;
            if (arc.deliveredItems.Contains(itemId)) return false;

            arc.deliveredItems.Add(itemId);

            bool stageComplete = true;
            for (int i = 0; i < stage.objectiveItems.Count; i++)
            {
                if (!arc.deliveredItems.Contains(stage.objectiveItems[i])) { stageComplete = false; break; }
            }

            if (stageComplete)
                AdvanceToNextStage(arc, def, day);

            if (!_restoring) OnStageAdvanced?.Invoke(arc, arc.currentStage);
            return true;
        }

        /// <summary>
        /// Resolve the crisis fork. Succeeds only while the arc awaits a branch and
        /// only for an id authored on that arc's branch stage. On success the arc
        /// records the chosen branch and granted trait, moves to the resolution
        /// stage, and reports the branch so the host can apply morale and the trait
        /// through their owning systems.
        /// </summary>
        public bool TryChooseBranch(
            string survivorId, string branchId, int day,
            out NarrativeQuestlineBranchDef? chosenBranch)
        {
            chosenBranch = null;
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(branchId)) return false;
            if (!_arcsBySurvivor.TryGetValue(survivorId, out var arc)) return false;
            if (arc.status != NarrativeArcStatus.AwaitingBranch) return false;
            if (!string.IsNullOrEmpty(arc.chosenBranchId)) return false;

            var def = GetDefinition(arc.questId);
            if (def == null) return false;
            var branchStage = def.FindBranchStage();
            if (branchStage == null) return false;
            if (branchStage.stage != arc.currentStage) return false;

            var branch = branchStage.FindBranch(branchId);
            if (branch == null) return false;

            arc.chosenBranchId = branch.id;
            arc.grantedTraitId = branch.traitGranted;
            chosenBranch = branch;

            AdvanceToNextStage(arc, def, day);
            arc.status = NarrativeArcStatus.Resolved;
            arc.resolvedDay = day;

            if (!_restoring)
            {
                OnBranchChosen?.Invoke(arc, branch);
                OnArcResolved?.Invoke(arc);
            }
            return true;
        }

        private void AdvanceToNextStage(NarrativeQuestlineArcState arc, NarrativeQuestlineDef def, int day)
        {
            int next = arc.currentStage;
            bool foundNext = false;
            for (int i = 0; i < def.stages.Count; i++)
            {
                if (def.stages[i].stage > arc.currentStage)
                {
                    if (!foundNext || def.stages[i].stage < next) { next = def.stages[i].stage; foundNext = true; }
                }
            }

            if (!foundNext)
            {
                // No later stage: the arc ends where it stands.
                arc.status = NarrativeArcStatus.Resolved;
                arc.resolvedDay = day;
                return;
            }

            arc.currentStage = next;
            var stage = def.FindStage(next);
            arc.status = stage != null && stage.HasBranch
                ? NarrativeArcStatus.AwaitingBranch
                : NarrativeArcStatus.Active;
        }

        public NarrativeQuestlineSaveState CaptureState()
        {
            var snapshot = new NarrativeQuestlineSaveState
            {
                schema_version = 1,
                systemId = SystemId
            };
            foreach (var arc in Arcs)
                snapshot.arcs.Add(arc.Clone());
            return snapshot;
        }

        public void RestoreState(NarrativeQuestlineSaveState? saved)
        {
            _arcsBySurvivor.Clear();
            if (saved == null || saved.arcs == null)
            {
                RebuildStateList();
                return;
            }

            _restoring = true;
            try
            {
                for (int i = 0; i < saved.arcs.Count; i++)
                {
                    var arc = saved.arcs[i];
                    if (arc == null || string.IsNullOrEmpty(arc.survivorId)) continue;
                    // Arcs whose definition is no longer authored are dropped rather
                    // than resurrected as orphans.
                    if (!_byQuestId.ContainsKey(arc.questId)) continue;
                    if (_arcsBySurvivor.ContainsKey(arc.survivorId)) continue;

                    var clone = arc.Clone();
                    clone.deliveredItems = clone.deliveredItems ?? new List<string>();
                    _arcsBySurvivor[clone.survivorId] = clone;
                }
            }
            finally
            {
                _restoring = false;
            }

            RebuildStateList();
        }

        private void RebuildStateList()
        {
            var arcs = new List<NarrativeQuestlineArcState>(_arcsBySurvivor.Count);
            foreach (var arc in Arcs) arcs.Add(arc);
            _state = new NarrativeQuestlineSaveState
            {
                schema_version = 1,
                systemId = SystemId,
                arcs = arcs
            };
        }

        /// <summary>Live aggregate state. Use <see cref="CaptureState"/> for persistence.</summary>
        public NarrativeQuestlineSaveState State => _state;
    }
}
