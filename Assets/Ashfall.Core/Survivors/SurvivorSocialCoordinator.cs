using System;
using System.Collections.Generic;
#pragma warning disable CS8618

using Ashfall.Core;
using Ashfall.Core.StartingLevel;

namespace Ashfall.Core.Survivors
{
    // ── Aggregate save DTO (one section, not five loose files) ─────────────

    [Serializable]
    public sealed class SurvivorSocialSaveState
    {
        public LeadershipSaveState leadership = new LeadershipSaveState();
        public IdeologicalFrictionSaveState friction = new IdeologicalFrictionSaveState();
        public RationConflictSaveState ration = new RationConflictSaveState();
        public TraumaBondSaveState trauma = new TraumaBondSaveState();
        public SkillAtrophySaveState atrophy = new SkillAtrophySaveState();
    }

    // ── Read model (consumed by survivor-relations and duty-roster panels) ─

    [Serializable]
    public sealed class SurvivorSocialReadModel
    {
        public string leaderId = string.Empty;
        public float leaderStress;
        public List<Entry> entries = new List<Entry>();

        [Serializable]
        public sealed class Entry
        {
            public string survivorId = string.Empty;
            public string belief = string.Empty;
            public int bondCount;
            public string strongestBondPartnerId = string.Empty;
            public float strongestBondStrength;
            public string resentmentTargetId = string.Empty;
            public float resentmentLevel;
            public List<string> atrophiedSkills = new List<string>();
            public float rationAllocation;
            public float perceivedFairness;
        }
    }

    /// <summary>
    /// Survivor-social coordinator — the single wiring point for the five
    /// dormant social-mechanics systems (Leadership, IdeologicalFriction,
    /// RationConflict, TraumaBond, SkillAtrophy). Owns the five systems,
    /// wires their delegate hooks to the real Needs / Relations / Roster
    /// APIs, feeds them real data each day, and persists as ONE section
    /// inside the campaign envelope.
    ///
    /// Engine-agnostic (Core). The host constructs it with real system
    /// references and calls <see cref="TickDay"/> from the daily tick.
    /// </summary>
    public sealed class SurvivorSocialCoordinator
    {
        public LeadershipSystem Leadership { get; }
        public IdeologicalFrictionSystem Friction { get; }
        public RationConflictSystem Ration { get; }
        public TraumaBondSystem TraumaBond { get; }
        public SkillAtrophySystem Atrophy { get; }

        /// <summary>
        /// Plan 60 / D7 — the relationship authority this coordinator already owns.
        /// Exposed read-only so the memorial/grief path reaches the same ledger the
        /// social systems mutate, instead of a second instance.
        /// </summary>
        public SurvivorRelationsSystem Relations => _relations;

        private readonly NeedsSystem _needs;
        private readonly SurvivorRelationsSystem _relations;
        private readonly DutyRosterSystem _roster;
        private readonly ISeededRng _rng;
        private readonly Func<int> _getDay;

        /// <summary>
        /// Current ration policy, set by the host from StartingLevelSystem.
        /// Drives per-survivor allocations fed to RationConflictSystem.
        /// </summary>
        public RationPolicy RationPolicy { get; set; }

        /// <summary>survivorId → belief profile id (registered by the host).</summary>
        private readonly Dictionary<string, string> _beliefs =
            new Dictionary<string, string>(StringComparer.Ordinal);

        private readonly List<string> _aliveIds = new List<string>();
        private readonly List<SkillActorAdapter> _actorAdapters = new List<SkillActorAdapter>();

        public SurvivorSocialCoordinator(
            ISeededRng rng,
            NeedsSystem needs,
            SurvivorRelationsSystem relations,
            DutyRosterSystem roster,
            Func<int> getDay,
            ILog? log = null)
        {
            _rng = rng ?? new SeededRng(31415);
            _needs = needs ?? new NeedsSystem();
            _relations = relations;
            _roster = roster;
            _getDay = getDay ?? (() => 1);

            Leadership = new LeadershipSystem();
            Friction = new IdeologicalFrictionSystem();
            Ration = new RationConflictSystem(_rng);
            TraumaBond = new TraumaBondSystem();
            Atrophy = new SkillAtrophySystem();

            WireHooks();
        }

        // ── Hook wiring ───────────────────────────────────────────────

        private void WireHooks()
        {
            // Leadership → Needs (morale)
            Leadership.ApplyMoraleDelta = (id, delta) =>
            {
                if (_needs != null) _needs.Modify(id, NeedKind.Morale, delta);
            };
            Leadership.ApplyShelterMoraleDelta = delta =>
            {
                if (_needs == null) return;
                for (int i = 0; i < _aliveIds.Count; i++)
                    _needs.Modify(_aliveIds[i], NeedKind.Morale, delta);
            };
            Leadership.GetAliveSurvivorIds = () => _aliveIds;

            // TraumaBond → Relations (affinity) + Roster (same-shift)
            TraumaBond.AdjustAffinity = (a, b, delta) =>
            {
                _relations?.ModifyAffinity(a, b, delta);
            };
            TraumaBond.AreOnSameShift = (a, b) =>
            {
                if (_roster == null) return false;
                string ra = _roster.GetRoleOf(a);
                if (string.IsNullOrEmpty(ra)) return false;
                return string.Equals(ra, _roster.GetRoleOf(b), StringComparison.Ordinal);
            };
            TraumaBond.GetDay = () => MathfCompat.Max(1f, _getDay());

            // IdeologicalFriction → Relations (affinity)
            Friction.OnAffinityChanged += (a, b, delta) =>
            {
                _relations?.ModifyAffinity(a, b, delta);
            };

            // RationConflict → Needs (morale)
            Ration.OnMoraleDelta += (id, delta) =>
            {
                if (_needs != null) _needs.Modify(id, NeedKind.Morale, delta);
            };
        }

        // ── Registration ──────────────────────────────────────────────

        /// <summary>
        /// Register a survivor's belief profile for ideological friction.
        /// Call once per survivor during setup; idempotent.
        /// </summary>
        public void RegisterBelief(string survivorId, string beliefProfileId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            _beliefs[survivorId] = beliefProfileId ?? string.Empty;
            Friction.RegisterBelief(survivorId, beliefProfileId ?? string.Empty);
        }

        /// <summary>
        /// Refresh the alive-survivor list from the host roster. Called
        /// each tick before the social systems advance.
        /// </summary>
        public void SetAliveSurvivors(IReadOnlyList<string> aliveIds)
        {
            _aliveIds.Clear();
            if (aliveIds == null) return;
            for (int i = 0; i < aliveIds.Count; i++)
            {
                if (!string.IsNullOrEmpty(aliveIds[i]))
                    _aliveIds.Add(aliveIds[i]);
            }

            // Ensure every alive survivor is registered in the per-survivor systems.
            for (int i = 0; i < _aliveIds.Count; i++)
            {
                Ration.RegisterSurvivor(_aliveIds[i]);
            }
        }

        // ── Daily tick ────────────────────────────────────────────────

        /// <summary>
        /// Advance all five social systems by one day. Feeds real needs,
        /// duty-shift pairs, ration policy, and skill-actor morale to the
        /// subsystems. Outputs route through Needs (morale), Relations
        /// (affinity), and the atrophy event surface.
        /// </summary>
        public void TickDay(int day, IReadOnlyList<SurvivorNeedsState> survivors)
        {
            if (survivors == null) return;

            // Rebuild alive list + actor adapters from real needs state.
            _aliveIds.Clear();
            _actorAdapters.Clear();
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s == null || !s.IsAliveState) continue;
                _aliveIds.Add(s.Id);
                Ration.RegisterSurvivor(s.Id);
                _actorAdapters.Add(new SkillActorAdapter(s));
            }

            // 1. Ration allocations from policy + leader priority.
            ApplyRationAllocations();

            // 2. Ration conflict — per survivor.
            for (int i = 0; i < _aliveIds.Count; i++)
            {
                Ration.Tick(_aliveIds[i], 24f);
            }

            // 3. Ideological friction — roommate pairs from duty-roster shifts.
            TickFrictionPairs();

            // 4. Trauma bonds — decay.
            for (int i = 0; i < _aliveIds.Count; i++)
            {
                TraumaBond.Tick(_aliveIds[i], 24f);
            }

            // 5. Leadership — stress decay + cooldown.
            Leadership.Tick(24f);

            // 6. Skill atrophy — from real morale.
            if (_actorAdapters.Count > 0)
                Atrophy.Tick(24f, _actorAdapters);
        }

        private void ApplyRationAllocations()
        {
            if (_aliveIds.Count == 0) return;

            string leader = Leadership.CurrentLeaderId;
            float baseAlloc = RationPolicy switch
            {
                RationPolicy.Half => 0.35f,
                RationPolicy.Irradiated => 0.40f,
                _ => 0.50f,
            };
            // When a leader is designated, they receive priority rations
            // while everyone else gets less — a gap large enough to cross
            // the resentment threshold (0.20) even with a small roster.
            float leaderAlloc = MathfCompat.Min(1f, baseAlloc + 0.45f);
            float nonLeaderAlloc = MathfCompat.Max(0f, baseAlloc - 0.15f);

            for (int i = 0; i < _aliveIds.Count; i++)
            {
                string id = _aliveIds[i];
                float alloc = string.IsNullOrEmpty(leader)
                    ? baseAlloc
                    : (string.Equals(id, leader, StringComparison.Ordinal)
                        ? leaderAlloc : nonLeaderAlloc);
                Ration.SetAllocation(id, alloc);
            }
        }

        private void TickFrictionPairs()
        {
            if (_roster == null || _aliveIds.Count < 2) return;

            // Group survivors by duty-roster role; tick each pair on the same role.
            var byRole = new Dictionary<string, List<string>>(StringComparer.Ordinal);
            for (int i = 0; i < _aliveIds.Count; i++)
            {
                string id = _aliveIds[i];
                string role = _roster.GetRoleOf(id);
                if (string.IsNullOrEmpty(role)) role = "unassigned";
                if (!byRole.TryGetValue(role, out var list))
                {
                    list = new List<string>();
                    byRole[role] = list;
                }
                list.Add(id);
            }

            foreach (var kv in byRole)
            {
                var list = kv.Value;
                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = i + 1; j < list.Count; j++)
                    {
                        Friction.TickRoommates(list[i], list[j], 24f);
                    }
                }
            }
        }

        // ── Event forwarders (host calls these when events occur) ──────

        public void OnSharedHazardEndured(List<string> participantIds, string hazardId) =>
            TraumaBond.OnSharedHazardEndured(participantIds, hazardId);

        public void OnSurvivorDied(string survivorId) => Leadership.OnSurvivorDied(survivorId);
        public void OnSurvivorInjured(string survivorId) => Leadership.OnSurvivorInjured(survivorId);
        public void OnCrisisEvent() => Leadership.OnCrisisEvent();

        public bool DesignateLeader(string survivorId) => Leadership.DesignateLeader(survivorId);
        public bool StepDown(string survivorId) => Leadership.StepDown(survivorId);

        // ── Read model ────────────────────────────────────────────────

        public SurvivorSocialReadModel BuildReadModel()
        {
            var rm = new SurvivorSocialReadModel
            {
                leaderId = Leadership.CurrentLeaderId ?? string.Empty,
                leaderStress = string.IsNullOrEmpty(Leadership.CurrentLeaderId)
                    ? 0f : Leadership.GetLeaderStress(Leadership.CurrentLeaderId),
            };

            for (int i = 0; i < _aliveIds.Count; i++)
            {
                string id = _aliveIds[i];
                var entry = new SurvivorSocialReadModel.Entry
                {
                    survivorId = id,
                    belief = Friction.GetBelief(id),
                    bondCount = TraumaBond.GetBondCount(id),
                    resentmentTargetId = Ration.GetState(id)?.resentmentTargetId ?? string.Empty,
                    resentmentLevel = Ration.GetState(id)?.resentmentLevel ?? 0f,
                    atrophiedSkills = new List<string>(Atrophy.GetAtrophiedSkillIds(id)),
                    rationAllocation = Ration.GetAllocation(id),
                    perceivedFairness = Ration.GetState(id)?.perceivedFairness ?? 1f,
                };

                // Strongest bond partner.
                float strongest = 0f;
                string partner = string.Empty;
                for (int j = 0; j < _aliveIds.Count; j++)
                {
                    if (i == j) continue;
                    float bs = TraumaBond.GetBondStrength(id, _aliveIds[j]);
                    if (bs > strongest)
                    {
                        strongest = bs;
                        partner = _aliveIds[j];
                    }
                }
                entry.strongestBondPartnerId = partner;
                entry.strongestBondStrength = strongest;

                rm.entries.Add(entry);
            }

            return rm;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public SurvivorSocialSaveState CaptureState()
        {
            return new SurvivorSocialSaveState
            {
                leadership = Leadership.CaptureState(),
                friction = Friction.CaptureState(),
                ration = Ration.CaptureState(),
                trauma = TraumaBond.CaptureState(),
                atrophy = Atrophy.CaptureState(),
            };
        }

        public void RestoreState(SurvivorSocialSaveState save)
        {
            if (save == null) return;
            Leadership.RestoreState(save.leadership);
            Friction.RestoreState(save.friction);
            Ration.RestoreState(save.ration);
            TraumaBond.RestoreState(save.trauma);
            Atrophy.RestoreState(save.atrophy);
        }

        // ── SkillActor adapter (wraps SurvivorNeedsState) ─────────────

        private sealed class SkillActorAdapter : SkillActor
        {
            private readonly SurvivorNeedsState _state;
            public SkillActorAdapter(SurvivorNeedsState state) { _state = state; }
            public string Id => _state.Id;
            public bool IsAlive => _state.IsAliveState;
            public float Morale => _state.Morale;
            public float Health => _state.Health;
            public string ExpertDisciplineId => string.Empty;
            public void SetSkillBonus(string disciplineId, float bonus) { }
        }
    }
}
