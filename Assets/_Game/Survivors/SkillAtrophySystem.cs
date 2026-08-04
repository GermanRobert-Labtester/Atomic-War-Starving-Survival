using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Skill Atrophy (Prompt #10): when a survivor's Morale stays below the
    /// atrophy threshold for the configured window, their MedicalSkill and/or
    /// CraftingSkill permanently degrades. They haven't forgotten how — they've
    /// lost the will to care about the details.
    ///
    /// Plain C# system; EditMode-testable. Owns and mutates Survivor fields:
    /// ConsecutiveLowMoraleDays, AtrophiedSkills, MedicalSkill, CraftingSkill.
    /// </summary>
    public class SkillAtrophySystem
    {
        /// <summary>Morale must stay below this value for the full window.</summary>
        public const float AtrophyMoraleThreshold = 20f;

        /// <summary>Consecutive days below the threshold before atrophy fires.</summary>
        public const float AtrophyWindowDays = 14f;

        /// <summary>Multiplier applied to the skill when atrophy triggers (0.5 = halved).</summary>
        public const float AtrophyMultiplier = 0.5f;

        /// <summary>Fired when a skill atrophies. Args: (survivor, skillName).</summary>
        public event Action<Survivor, string> OnSkillAtrophied;

        /// <summary>Fired when a survivor's morale recovers above threshold after
        /// being below it (resets the countdown).</summary>
        public event Action<Survivor> OnAtrophyDangerPassed;

        // -----------------------------------------------------------------
        // Tick
        // -----------------------------------------------------------------

        /// <summary>
        /// Advance the system by <paramref name="gameHours"/>. Called from
        /// GameBootstrap.TickSystems alongside other per-survivor systems.
        /// </summary>
        public void Tick(float gameHours, IReadOnlyList<Survivor> survivors)
        {
            if (gameHours <= 0f || survivors == null) return;
            float gameDays = gameHours / 24f;

            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                TickSurvivor(sv, gameDays);
            }
        }

        private void TickSurvivor(Survivor sv, float gameDays)
        {
            if (sv.Needs.Morale < AtrophyMoraleThreshold)
            {
                float previousDays = sv.ConsecutiveLowMoraleDays;
                sv.ConsecutiveLowMoraleDays += gameDays;

                // Check for atrophy trigger — one per skill
                if (previousDays < AtrophyWindowDays && sv.ConsecutiveLowMoraleDays >= AtrophyWindowDays)
                {
                    ApplyAtrophy(sv);
                }
            }
            else
            {
                if (sv.ConsecutiveLowMoraleDays > 0f)
                {
                    sv.ConsecutiveLowMoraleDays = 0f;
                    OnAtrophyDangerPassed?.Invoke(sv);
                }
            }
        }

        private void ApplyAtrophy(Survivor sv)
        {
            // Ensure list is initialized (JsonUtility leaves it null on empty save)
            if (sv.AtrophiedSkills == null)
                sv.AtrophiedSkills = new System.Collections.Generic.List<string>();

            // Atrophy both Medical and Crafting skills if not already atrophied.
            // The skill value itself stays unchanged; EffectiveXSkill applies the
            // penalty at read time so we don't double-multiply.
            if (!sv.AtrophiedSkills.Contains("medical"))
            {
                sv.AtrophiedSkills.Add("medical");
                OnSkillAtrophied?.Invoke(sv, "medical");
            }

            if (!sv.AtrophiedSkills.Contains("crafting"))
            {
                sv.AtrophiedSkills.Add("crafting");
                OnSkillAtrophied?.Invoke(sv, "crafting");
            }
        }
    }
}
