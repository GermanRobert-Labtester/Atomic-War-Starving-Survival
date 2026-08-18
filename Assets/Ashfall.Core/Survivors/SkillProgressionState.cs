using System;
using System.Collections.Generic;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// Minimal engine-agnostic survival-actor surface needed by the progression
    /// and atrophy systems. Replaces the legacy <c>Survivor</c> class fields that
    /// the upgrade copied directly; Core does not import the Unity-typed class.
    ///
    /// Each call site in the runtime supplies a thin adapter that maps an engine
    /// survivor into this interface (e.g. a <c>Godot SurvivorsHostSession</c>
    /// adapter). The Core engine never touches engine types.
    /// </summary>
    public interface SkillActor
    {
        string Id { get; }
        bool IsAlive { get; }

        /// <summary>
        /// Morale 0..100. The legacy system reads <c>NeedsSystem.Morale</c>; the
        /// Core engine treats this as a 0..100 black box returned by the host.
        /// </summary>
        float Morale { get; }

        /// <summary>
        /// Health 0..100. Same shape as morale — engine agnostic.
        /// </summary>
        float Health { get; }

        /// <summary>
        /// Predetermined expert-track discipline id (snake_case). Empty when this
        /// survivor has no expert gate.
        /// </summary>
        string ExpertDisciplineId { get; }

        /// <summary>
        /// Backing store for the survivor's individual skill bonus values. Each
        /// discipline reads its <c>{Discipline}SkillBonus</c>; the host applies them
        /// to its own survivor model. Core sets them through this interface.
        /// </summary>
        void SetSkillBonus(string disciplineId, float bonus);
    }

    /// <summary>
    /// Per-survivor per-discipline progression bookkeeping. Save/load safe —
    /// parallel arrays because <c>JsonUtility</c> cannot serialize
    /// <see cref="Dictionary{TKey,TValue}"/> directly.
    ///
    /// Always used by <see cref="SkillProgressionSystem"/> via lookup; never
    /// constructed directly outside that system.
    /// </summary>
    [Serializable]
    public sealed class SkillProgressionState
    {
        public bool expertSkillEarned = false;

        /// <summary>Active skill ids (always-on at runtime; not yet decayed to dormant).</summary>
        public List<string> activeSkillIds = new List<string>();

        /// <summary>Dormant skill ids (lost mechanical benefit until practiced again).</summary>
        public List<string> dormantSkillIds = new List<string>();

        /// <summary>Tracking discipline -> cumulative XP. Lists rather than Dictionary for save-safety.</summary>
        public List<string> disciplineIds = new List<string>();
        public List<float> xpValues = new List<float>();

        /// <summary>Tracking discipline -> last day this discipline was practiced.</summary>
        public List<int> lastUsedDays = new List<int>();
    }

    /// <summary>
    /// Top-level save envelope for <see cref="SkillProgressionSystem"/>.
    /// Mirrors the legacy <c>SkillProgressionSave</c> shape — one entry per survivor.
    /// </summary>
    [Serializable]
    public sealed class SkillProgressionSaveState
    {
        public List<SkillProgressionState> entries = new List<SkillProgressionState>();
        public List<string> survivorIds = new List<string>();
    }

    /// <summary>
    /// Save envelope for <see cref="SkillAtrophySystem"/>.
    /// </summary>
    [Serializable]
    public sealed class SkillAtrophySaveState
    {
        public List<string> survivorIds = new List<string>();
        public List<float> consecutiveLowMoraleDays = new List<float>();
        public List<List<string>> atrophiedSkillIds = new List<List<string>>();
    }
}
