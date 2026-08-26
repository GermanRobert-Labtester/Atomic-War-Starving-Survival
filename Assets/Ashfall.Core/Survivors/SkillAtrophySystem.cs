using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// ASHFALL — Skill Atrophy Engine (engine-agnostic Core port).
    ///
    /// Replaces <c>Assets/_Game/Survivors/SkillAtrophySystem.cs</c>. When an
    /// actor's morale stays below the atrophy threshold for the configured
    /// window, their Medical / Crafting skills permanently degrade. The actor
    /// hasn't forgotten how — they've lost the will to care about the details.
    ///
    /// Engine-agnostic: NO <see cref="UnityEngine.Random"/>, NO
    /// <c>UnityEngine.Mathf</c>; actors come in via the same <see cref="SkillActor"/>
    /// interface that the progression engine uses. The host adapter is
    /// responsible for monitoring atrophied-skill entries and re-applying the
    /// display-side penalty (e.g. EffectiveMedicalSkill *= 0.5).
    /// </summary>
    public sealed class SkillAtrophySystem
    {
        /// <summary>Morale must stay below this value for the full window to fire atrophy.</summary>
        public const float AtrophyMoraleThreshold = 20f;

        /// <summary>Consecutive days below the threshold before atrophy fires.</summary>
        public const float AtrophyWindowDays = 14f;

        /// <summary>Multiplier applied to the skill when atrophy triggers (0.5 = halved).</summary>
        public const float AtrophyMultiplier = 0.5f;

        /// <summary>Fired when a skill atrophies. Args: (actorId, skillName).</summary>
        public event Action<string, string> OnSkillAtrophied;

        /// <summary>Fired when an actor's morale recovers above threshold after a low streak.</summary>
        public event Action<string> OnAtrophyDangerPassed;

        /// <summary>Per-actor cyclic bookkeeping keyed by actor id. Pure data; safe to serialize.</summary>
        private readonly Dictionary<string, AtrophyState> _byActor =
            new Dictionary<string, AtrophyState>(StringComparer.Ordinal);

        /// <summary>
        /// Advance the system by <paramref name="gameHours"/>. Called from the
        /// host's per-day tick alongside other per-actor systems.
        /// </summary>
        public void Tick(float gameHours, IReadOnlyList<SkillActor> actors)
        {
            if (gameHours <= 0f || actors == null) return;
            float gameDays = gameHours / 24f;

            for (int i = 0; i < actors.Count; i++)
            {
                var actor = actors[i];
                if (actor == null || !actor.IsAlive) continue;
                TickActor(actor.Id, actor.Morale, gameDays);
            }
        }

        private void TickActor(string actorId, float morale, float gameDays)
        {
            if (string.IsNullOrEmpty(actorId)) return;
            if (!_byActor.TryGetValue(actorId, out var state))
            {
                state = new AtrophyState();
                _byActor[actorId] = state;
            }

            if (morale < AtrophyMoraleThreshold)
            {
                float previousDays = state.consecutiveLowMoraleDays;
                state.consecutiveLowMoraleDays += gameDays;

                if (previousDays < AtrophyWindowDays && state.consecutiveLowMoraleDays >= AtrophyWindowDays)
                {
                    ApplyAtrophy(actorId, state);
                }
            }
            else
            {
                if (state.consecutiveLowMoraleDays > 0f)
                {
                    state.consecutiveLowMoraleDays = 0f;
                    OnAtrophyDangerPassed?.Invoke(actorId);
                }
            }
        }

        private void ApplyAtrophy(string actorId, AtrophyState state)
        {
            EnsureAtrophied(state, "medical");
            EnsureAtrophied(state, "crafting");
        }

        private void EnsureAtrophied(AtrophyState state, string skillId)
        {
            if (state.atrophiedSkillIds == null)
                state.atrophiedSkillIds = new List<string>();
            if (!state.atrophiedSkillIds.Contains(skillId))
            {
                state.atrophiedSkillIds.Add(skillId);
                OnSkillAtrophied?.Invoke(state.survivorId, skillId);
            }
        }

        public bool IsAtrophied(string actorId, string skillId)
        {
            if (string.IsNullOrEmpty(actorId) || string.IsNullOrEmpty(skillId)) return false;
            if (!_byActor.TryGetValue(actorId, out var state)) return false;
            return state.atrophiedSkillIds != null && state.atrophiedSkillIds.Contains(skillId);
        }

        public IReadOnlyList<string> GetAtrophiedSkillIds(string actorId)
        {
            if (string.IsNullOrEmpty(actorId)) return Array.Empty<string>();
            if (!_byActor.TryGetValue(actorId, out var state)) return Array.Empty<string>();
            return state.atrophiedSkillIds ?? (IReadOnlyList<string>)Array.Empty<string>();
        }

        /// <summary>Pure data carrier; safe for save/load (parallel arrays because JsonUtility cannot serialize Dictionary directly).</summary>
        [Serializable]
        public sealed class AtrophyState
        {
            public string survivorId = string.Empty;
            public float consecutiveLowMoraleDays = 0f;
            public List<string> atrophiedSkillIds = new List<string>();
        }

        /// <summary>Capture save envelope (mirrors the legacy SkillAtrophySystem shape).</summary>
        public SkillAtrophySaveState CaptureState()
        {
            var save = new SkillAtrophySaveState();
            foreach (var kv in _byActor)
            {
                if (kv.Value == null) continue;
                save.survivorIds.Add(kv.Key);
                save.consecutiveLowMoraleDays.Add(kv.Value.consecutiveLowMoraleDays);
                save.atrophiedSkillIds.Add(kv.Value.atrophiedSkillIds != null
                    ? new List<string>(kv.Value.atrophiedSkillIds) : new List<string>());
            }
            return save;
        }

        public void RestoreState(SkillAtrophySaveState save)
        {
            _byActor.Clear();
            if (save == null || save.survivorIds == null) return;
            for (int i = 0; i < save.survivorIds.Count; i++)
            {
                var actorId = save.survivorIds[i];
                if (string.IsNullOrEmpty(actorId)) continue;
                float days = i < save.consecutiveLowMoraleDays.Count ? save.consecutiveLowMoraleDays[i] : 0f;
                var atrophied = i < save.atrophiedSkillIds.Count && save.atrophiedSkillIds[i] != null
                    ? save.atrophiedSkillIds[i] : new List<string>();

                _byActor[actorId] = new AtrophyState
                {
                    survivorId = actorId,
                    consecutiveLowMoraleDays = days,
                    atrophiedSkillIds = new List<string>(atrophied),
                };
            }
        }
    }
}
