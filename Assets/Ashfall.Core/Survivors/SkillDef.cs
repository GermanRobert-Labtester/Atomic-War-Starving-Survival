using System;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// ASHFALL — Skill Definition (engine-agnostic port of <c>PerkSO</c>).
    /// Plain C# skill / perk definition consumed by <see cref="SkillProgressionSystem"/>.
    /// Survives save/load (declared in the per-skill catalog JSON); the canonical
    /// ids live in <c>skills.json</c> in <c>Assets/StreamingAssets/Data/</c>.
    ///
    /// Engine-agnostic means: NO <c>UnityEngine.*</c>, NO <c>Godot.*</c>, NO
    /// <c>JsonUtility</c>. This file replaces the legacy
    /// <c>Assets/_Game/Survivors/PerkSO.cs</c> ScriptableObject — a 1:1 mapping
    /// of public field set with a deterministic, finite-flag initialization.
    /// </summary>
    [Serializable]
    public sealed class SkillDef
    {
        /// <summary>snake_case id (e.g. "skill_field_dressing").</summary>
        public string id = string.Empty;

        /// <summary>Display name (e.g. "Field Dressing").</summary>
        public string displayName = string.Empty;

        /// <summary>Prose description. May be empty.</summary>
        public string description = string.Empty;

        /// <summary>snake_case discipline id (one of the known disciplines).</summary>
        public string disciplineId = string.Empty;

        /// <summary>XP needed to unlock this skill. Action-driven skills sit at moderate thresholds (50..200); milestone-only skills hold an unreachable sentinel (999999).</summary>
        public float xpThreshold = 0f;

        /// <summary>Mechanical bonus applied when this skill is active (0..1 range). Multiplicative on the survivor's effective skill.</summary>
        public float skillBonus = 0f;

        /// <summary>
        /// If true, this skill is an expert-track gate: only one expert skill per
        /// survivor, and only when the survivor's <see cref="SkillActor.ExpertDisciplineId"/>
        /// matches <see cref="disciplineId"/>.
        /// </summary>
        public bool isExpertSkill = false;
    }
}
