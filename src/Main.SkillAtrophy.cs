// SPDX-License-Identifier: MIT
// ============================================================================
// Skill atrophy host composition. The Core SkillAtrophySystem remains the sole
// decay authority; the host supplies survivor morale and health from the needs
// owner. The Core system decides which unused skills atrophy and records each
// atrophy exactly once.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private SkillAtrophyHostSession? _skillAtrophy;
        private bool _skillAtrophyDirty;

        public SkillAtrophyHostSession? SkillAtrophySession => _skillAtrophy;

        public void SetupSkillAtrophy()
        {
            if (_skillAtrophy != null) return;
            var saved = SkillAtrophySaveStore.TryLoad();
            _skillAtrophy = SkillAtrophyHostSession.Create(saved);
            _skillAtrophy.StateChanged += () => _skillAtrophyDirty = true;
        }

        /// <summary>
        /// Advances skill atrophy over <paramref name="gameHours"/>. The activity data are derived
        /// from the canonical survivors roster (morale/health) in the host; the
        /// Core system decides which skills go unused.
        /// </summary>
        public void TickSkillAtrophy(float gameHours = 24f)
        {
            SetupSkillAtrophy();
            if (_skillAtrophy == null) return;

            var actors = new List<HostedSkillActor>();
            var roster = _survivors?.RosterState;
            if (roster != null)
            {
                foreach (var survivor in roster)
                {
                    if (survivor == null || string.IsNullOrEmpty(survivor.Id) || survivor.IsAlive == false) continue;
                    float morale = 50f;
                    float health = 100f;
                    var needs = _survivors?.Needs;
                    if (needs != null)
                    {
                        var needsState = needs.Get(survivor.Id);
                        if (needsState != null)
                        {
                            morale = Math.Clamp(needsState.Morale, 0f, 100f);
                            health = Math.Clamp(needsState.Health, 0f, 100f);
                        }
                    }
                    actors.Add(new HostedSkillActor(survivor.Id, morale, health, 0f));
                }
            }

            if (actors.Count > 0)
            {
                _skillAtrophy.Tick(gameHours, actors);
                _skillAtrophyDirty = true;
            }
        }

        public bool IsSkillAtrophied(string survivorId, string skillId)
        {
            SetupSkillAtrophy();
            return _skillAtrophy?.IsAtrophied(survivorId, skillId) ?? false;
        }

        public IReadOnlyList<string> GetAtrophiedSkillIds(string survivorId)
        {
            SetupSkillAtrophy();
            return _skillAtrophy?.GetAtrophiedSkillIds(survivorId) ?? Array.Empty<string>();
        }

        public (int Actors, int Atrophied) GetSkillAtrophyReadout()
        {
            SetupSkillAtrophy();
            if (_skillAtrophy == null) return (0, 0);
            int atrophied = _skillAtrophy.CaptureState().atrophiedSkillIds.Sum(x => x?.Count ?? 0);
            return (_skillAtrophy.TrackedActorCount, atrophied);
        }

        public void SaveSkillAtrophy()
        {
            if (_skillAtrophy == null) return;
            var state = _skillAtrophy.CaptureState();
            SkillAtrophySaveStore.TrySave(state);
            if (CaptureSection(SkillAtrophySaveStore.SectionName, SkillAtrophySaveStore.TryCapturePersisted(state)))
                _skillAtrophyDirty = false;
        }

        public void FlushSkillAtrophyIfDirty()
        {
            if (_skillAtrophyDirty) SaveSkillAtrophy();
        }

        public void ResetSkillAtrophy()
        {
            _skillAtrophy = null;
            _skillAtrophyDirty = false;
        }
    }
}
