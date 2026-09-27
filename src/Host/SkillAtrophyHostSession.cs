// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : SkillAtrophySaveStore
// Core State : Ashfall.Core.Survivors.SkillAtrophySaveState
// Host Caller: Main.SkillAtrophy
// Purpose    : Skill atrophy host session & persistence. The Core system remains
//              the sole decay authority: it reads each survivor's practice hours
//              and morale (via the SkillActor abstraction) and marks a skill
//              atrophied exactly once.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class SkillAtrophySaveStore
    {
        public const string FileName = "skill_atrophy_save.json";
        public const string SectionName = "skill_atrophy";

        private static readonly SaveStore<SkillAtrophySaveState> s_store =
            SaveStoreHub.Checksummed<SkillAtrophySaveState>(FileName, nameof(SkillAtrophySaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(SkillAtrophySaveState state) => s_store.CaptureBare(state);
        public static SkillAtrophySaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(SkillAtrophySaveState state) => s_store.TrySave(state);
        public static SkillAtrophySaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Host-side actor view. The host supplies survivor ids, morale, health, and
    /// practice hours; the Core system decides atrophy. This adapter is the only
    /// place the survivor roster shape enters the atrophy authority.
    /// </summary>
    public sealed class HostedSkillActor : SimpleSkillActor
    {
        public HostedSkillActor(string id, float morale, float health, float _unusedPractice = 0f, bool isAlive = true)
            : base(id)
        {
            Morale = morale;
            Health = health;
            IsAlive = isAlive;
        }
    }

    /// <summary>Host session composing the Core <see cref="SkillAtrophySystem"/>.</summary>
    public sealed class SkillAtrophyHostSession : HostSessionBase
    {
        private readonly SkillAtrophySystem _system;

        public SkillAtrophySystem System => _system;
        public string LastEvent { get; private set; } = string.Empty;

        public SkillAtrophyHostSession(SkillAtrophySaveState? state = null)
        {
            _system = new SkillAtrophySystem();
            if (state != null) _system.RestoreState(state);
        }

        public static SkillAtrophyHostSession Create(SkillAtrophySaveState? state = null) =>
            new SkillAtrophyHostSession(state);

        public void Tick(float gameHours, IReadOnlyList<HostedSkillActor> actors)
        {
            if (actors == null || actors.Count == 0) return;
            _system.Tick(gameHours, actors.Cast<SkillActor>().ToList());
            LastEvent = $"Skill atrophy ticked over {gameHours:0.#} game hours.";
            RaiseStateChanged();
        }

        public bool IsAtrophied(string actorId, string skillId) => _system.IsAtrophied(actorId, skillId);
        public IReadOnlyList<string> GetAtrophiedSkillIds(string actorId) => _system.GetAtrophiedSkillIds(actorId);

        public int TrackedActorCount => _system.CaptureState().survivorIds.Count;
        public int TrackedCount => TrackedActorCount;

        public SkillAtrophySaveState CaptureState() => _system.CaptureState();

        public void RestoreState(SkillAtrophySaveState state)
        {
            _system.RestoreState(state);
            LastEvent = "Restored skill atrophy state.";
            RaiseStateChanged();
        }

        public bool TrySave() => SkillAtrophySaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = SkillAtrophySaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
