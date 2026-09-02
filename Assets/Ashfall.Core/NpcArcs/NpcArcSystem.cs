using System;
using System.Collections.Generic;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.NpcArcs
{
    /// <summary>
    /// ASHFALL — Plan 52 recurring-NPC arc resolver.
    ///
    /// Pure, deterministic projection of persisted campaign facts onto the
    /// authored arc catalog. Owns nothing: it does not tick, does not mutate
    /// quest/flag/roster state, and holds no per-NPC branches. The resolved
    /// state is a pure function of (campaign day, expansion-quest progress,
    /// survivor roster), so any save that round-trips those authorities
    /// resolves identical arc states.
    ///
    /// Precedence contract (documented conventions, enforced by authored
    /// data): roster death &gt; recruited &gt; authored terminal branch &gt;
    /// late/evolved branch flags &gt; day-gated fallback &gt; initial. Day
    /// thresholds can therefore never outrank a player decision or a
    /// terminal state.
    /// </summary>
    public sealed class NpcArcSystem
    {
        private readonly NpcArcCatalog _catalog;
        private readonly Func<int> _dayProvider;
        private readonly ExpansionQuestSystem? _quests;
        private readonly SurvivorRosterSystem? _roster;

        public NpcArcSystem(
            NpcArcCatalog catalog,
            Func<int> dayProvider,
            ExpansionQuestSystem? quests = null,
            SurvivorRosterSystem? roster = null)
        {
            _catalog = catalog ?? new NpcArcCatalog();
            _dayProvider = dayProvider ?? (() => 1);
            _quests = quests;
            _roster = roster;
        }

        public NpcArcCatalog Catalog => _catalog;

        /// <summary>Resolved view of one NPC's current arc state.</summary>
        public sealed class Resolution
        {
            public string NpcId = string.Empty;
            public string DisplayName = string.Empty;
            public bool ArcFound;
            public string StateId = string.Empty;
            public string Kind = string.Empty;
            public string Role = string.Empty;
            public string LocationId = string.Empty;
            public string Summary = string.Empty;
            public bool Terminal;
            public bool Recruited;
            public bool Dead;
            public int Day;
        }

        /// <summary>
        /// True while the NPC is a living shelter resident. Recruitment uses
        /// the existing survivor-roster authority: the recruited NPC's
        /// survivor definition id IS the npc_* id (stable mapping, no second
        /// identity).
        /// </summary>
        public bool IsRecruited(string npcId)
        {
            var entry = _roster?.Find(npcId);
            return entry != null && entry.isAlive;
        }

        /// <summary>True when the roster records this NPC as dead.</summary>
        public bool IsRosterDead(string npcId)
        {
            var entry = _roster?.Find(npcId);
            return entry != null && !entry.isAlive;
        }

        /// <summary>
        /// Resolve the current arc state for one NPC. Deterministic: the
        /// winning state is the highest-precedence match; ties resolve in
        /// authored order. No match yields an out-of-window "unavailable"
        /// resolution (ArcFound=true, StateId empty) so callers can treat the
        /// NPC as simply not present today.
        /// </summary>
        public Resolution Resolve(string npcId)
        {
            var result = new Resolution
            {
                NpcId = npcId ?? string.Empty,
                Day = _dayProvider()
            };
            if (string.IsNullOrEmpty(npcId)) return result;

            var arc = _catalog.Find(npcId);
            if (arc == null) return result;

            result.ArcFound = true;
            result.DisplayName = arc.display_name;

            bool recruited = IsRecruited(npcId);
            bool rosterDead = IsRosterDead(npcId);
            result.Recruited = recruited;
            result.Dead = rosterDead;

            NpcArcStateDefinition? best = null;
            for (int i = 0; i < arc.states.Count; i++)
            {
                var state = arc.states[i];
                if (state == null) continue;
                if (!Matches(state, result.Day, recruited, rosterDead)) continue;
                // Strictly-greater keeps the earliest authored state on ties.
                if (best == null || state.precedence > best.precedence) best = state;
            }

            if (best == null) return result;

            result.StateId = best.id;
            result.Kind = best.kind;
            result.Role = best.role;
            result.LocationId = best.location_id;
            result.Summary = best.summary;
            result.Terminal = best.terminal;
            if (best.when_dead) result.Dead = true;
            if (best.when_recruited) result.Recruited = true;
            return result;
        }

        /// <summary>Resolve every authored arc, ordered by npc id (ordinal).</summary>
        public List<Resolution> ResolveAll()
        {
            var ids = new List<string>(_catalog.Arcs.Count);
            foreach (var arc in _catalog.Arcs) ids.Add(arc.npc_id);
            ids.Sort(StringComparer.Ordinal);

            var result = new List<Resolution>(ids.Count);
            foreach (var id in ids) result.Add(Resolve(id));
            return result;
        }

        /// <summary>
        /// Plan 52 distress contract: a dead, recruited, or otherwise
        /// terminal NPC must not emit a fresh distress signal. Used by the
        /// radio host as a suppression filter keyed on the signal's npc_id.
        /// </summary>
        public bool IsSignalSuppressed(string npcId)
        {
            if (string.IsNullOrEmpty(npcId)) return false;
            var resolution = Resolve(npcId);
            if (!resolution.ArcFound) return false;
            return resolution.Dead || resolution.Recruited || resolution.Terminal;
        }

        private bool Matches(NpcArcStateDefinition state, int day, bool recruited, bool rosterDead)
        {
            if (state.when_recruited && !recruited) return false;
            if (state.when_dead && !rosterDead) return false;
            if (!state.when_recruited && !state.when_dead)
            {
                // External states must not resolve for someone standing in the
                // shelter or in the ground: recruitment/death outrank roles.
                if (recruited || rosterDead) return false;
            }

            if (state.min_day > 0 && day < state.min_day) return false;
            if (state.max_day > 0 && day > state.max_day) return false;

            if (_quests == null)
            {
                // Without a quest authority, completion/choice memory cannot
                // be proven — only unconditional states may resolve.
                if (state.requires_completed.Count > 0) return false;
                if (state.requires_choice.Count > 0) return false;
            }
            else
            {
                foreach (var questId in state.requires_completed)
                    if (!_quests.IsCompleted(questId)) return false;

                foreach (var condition in state.requires_choice)
                {
                    var progress = _quests.GetProgress(condition.quest_id);
                    if (progress == null ||
                        !string.Equals(progress.currentChoiceId, condition.choice_id, StringComparison.Ordinal))
                        return false;
                }

                foreach (var questId in state.excludes_completed)
                    if (_quests.IsCompleted(questId)) return false;
            }

            return true;
        }
    }
}
