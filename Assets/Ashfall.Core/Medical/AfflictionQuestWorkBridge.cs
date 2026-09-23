// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 143 — Medical Afflictions → Quest & Work Bridge
// Pure domain bridge: reads affliction state and exposes quest gating and
// work efficiency modifiers to DutyRosterSystem and quest systems.
// No RNG. No save section — bridge is stateless; it projects affliction facts.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Medical
{
    // ── Catalog DTOs ────────────────────────────────────────────────────────

    [Serializable]
    public sealed class AfflictionWorkModifierDef
    {
        public string affliction_id { get; set; } = string.Empty;
        public float work_speed_multiplier { get; set; } = 1.0f;
        public float work_quality_multiplier { get; set; } = 1.0f;
        public List<string> excluded_duty_types { get; set; } = new List<string>();
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class AfflictionQuestGateDef
    {
        public string affliction_id { get; set; } = string.Empty;
        /// <summary>blocks | unlocks | modifies</summary>
        public string gate_type { get; set; } = "blocks";
        public string quest_tag { get; set; } = string.Empty;
        public string severity { get; set; } = "mild";
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class AfflictionBridgeCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<AfflictionWorkModifierDef> affliction_work_modifiers { get; set; } = new List<AfflictionWorkModifierDef>();
        public List<AfflictionQuestGateDef> affliction_quest_gates { get; set; } = new List<AfflictionQuestGateDef>();
    }

    // ── Result types ────────────────────────────────────────────────────────

    public sealed class WorkModifierResult
    {
        public float SpeedMultiplier { get; set; } = 1.0f;
        public float QualityMultiplier { get; set; } = 1.0f;
        public List<string> ExcludedDutyTypes { get; set; } = new List<string>();
        public List<string> ActiveAfflictionIds { get; set; } = new List<string>();

        public bool IsDutyExcluded(string dutyType)
            => ExcludedDutyTypes.Any(d => string.Equals(d, dutyType, StringComparison.OrdinalIgnoreCase));
    }

    public sealed class QuestGateCheckResult
    {
        public bool IsBlocked { get; set; }
        public List<string> UnlockedQuestTags { get; set; } = new List<string>();
        public List<string> BlockingAfflictionIds { get; set; } = new List<string>();
        public List<string> UnlockingAfflictionIds { get; set; } = new List<string>();
    }

    // ── Bridge ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Plan 143 — Stateless bridge that projects affliction IDs into
    /// work-efficiency penalties and quest gate answers.
    /// Rule 5: reads from affliction authority; does not own affliction state.
    /// </summary>
    public sealed class AfflictionQuestWorkBridge
    {
        private readonly List<AfflictionWorkModifierDef> _workModifiers = new List<AfflictionWorkModifierDef>();
        private readonly List<AfflictionQuestGateDef> _questGates = new List<AfflictionQuestGateDef>();

        public event Action<string, string>? OnQuestUnlocked;   // (survivorId, questTag)
        public event Action<string, string>? OnQuestBlocked;    // (survivorId, questTag)

        // ── Catalog loading ────────────────────────────────────────────────

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var catalog = JsonSerializer.Deserialize<AfflictionBridgeCatalog>(json, options);
                if (catalog == null) return;

                if (catalog.affliction_work_modifiers != null)
                {
                    foreach (var m in catalog.affliction_work_modifiers)
                        if (!string.IsNullOrWhiteSpace(m.affliction_id))
                            _workModifiers.Add(m);
                }

                if (catalog.affliction_quest_gates != null)
                {
                    foreach (var g in catalog.affliction_quest_gates)
                        if (!string.IsNullOrWhiteSpace(g.affliction_id))
                            _questGates.Add(g);
                }
            }
            catch (Exception) { /* malformed catalog falls back to built-in defaults; authoring errors are enforced by the data-integrity gate */ }
        }

        public IReadOnlyList<AfflictionWorkModifierDef> GetAllWorkModifiers() => _workModifiers;
        public IReadOnlyList<AfflictionQuestGateDef> GetAllQuestGates() => _questGates;

        // ── Work efficiency projection ─────────────────────────────────────

        /// <summary>
        /// Given a list of active affliction IDs, returns the stacked
        /// work speed/quality multipliers and union of excluded duties.
        /// Multiple afflictions multiply (compound) — worst-case stacking.
        /// Pure function: no side-effects, deterministic.
        /// </summary>
        public WorkModifierResult CalculateWorkModifiers(IEnumerable<string> activeAfflictionIds)
        {
            var result = new WorkModifierResult();
            if (activeAfflictionIds == null) return result;

            foreach (var afflictionId in activeAfflictionIds)
            {
                if (string.IsNullOrWhiteSpace(afflictionId)) continue;

                var def = _workModifiers.FirstOrDefault(m =>
                    string.Equals(m.affliction_id, afflictionId, StringComparison.OrdinalIgnoreCase));
                if (def == null) continue;

                result.SpeedMultiplier *= def.work_speed_multiplier;
                result.QualityMultiplier *= def.work_quality_multiplier;
                result.ActiveAfflictionIds.Add(afflictionId);

                foreach (var duty in def.excluded_duty_types)
                    if (!result.ExcludedDutyTypes.Contains(duty))
                        result.ExcludedDutyTypes.Add(duty);
            }

            // Clamp to sane floor
            result.SpeedMultiplier = Math.Max(0.1f, result.SpeedMultiplier);
            result.QualityMultiplier = Math.Max(0.1f, result.QualityMultiplier);
            return result;
        }

        // ── Quest gate projection ──────────────────────────────────────────

        /// <summary>
        /// Given a survivor's active affliction IDs and quest tag, determines
        /// whether the quest is blocked or unlocked by any affliction.
        /// Fires events as a side-effect for host adapters.
        /// </summary>
        public QuestGateCheckResult CheckQuestGates(
            string survivorId,
            string questTag,
            IEnumerable<string> activeAfflictionIds)
        {
            var result = new QuestGateCheckResult();
            if (activeAfflictionIds == null || string.IsNullOrWhiteSpace(questTag))
                return result;

            foreach (var afflictionId in activeAfflictionIds)
            {
                if (string.IsNullOrWhiteSpace(afflictionId)) continue;

                var gates = _questGates.Where(g =>
                    string.Equals(g.affliction_id, afflictionId, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(g.quest_tag, questTag, StringComparison.OrdinalIgnoreCase));

                foreach (var gate in gates)
                {
                    if (string.Equals(gate.gate_type, "blocks", StringComparison.OrdinalIgnoreCase))
                    {
                        result.IsBlocked = true;
                        result.BlockingAfflictionIds.Add(afflictionId);
                        OnQuestBlocked?.Invoke(survivorId, questTag);
                    }
                    else if (string.Equals(gate.gate_type, "unlocks", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!result.UnlockedQuestTags.Contains(gate.quest_tag))
                            result.UnlockedQuestTags.Add(gate.quest_tag);
                        result.UnlockingAfflictionIds.Add(afflictionId);
                        OnQuestUnlocked?.Invoke(survivorId, gate.quest_tag);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Returns all quest tags unlocked by the given affliction IDs,
        /// regardless of a specific quest being queried.
        /// </summary>
        public List<string> GetUnlockedQuestTags(IEnumerable<string> activeAfflictionIds)
        {
            var result = new List<string>();
            if (activeAfflictionIds == null) return result;

            foreach (var afflictionId in activeAfflictionIds)
            {
                if (string.IsNullOrWhiteSpace(afflictionId)) continue;

                foreach (var gate in _questGates.Where(g =>
                    string.Equals(g.affliction_id, afflictionId, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(g.gate_type, "unlocks", StringComparison.OrdinalIgnoreCase)))
                {
                    if (!result.Contains(gate.quest_tag))
                        result.Add(gate.quest_tag);
                }
            }
            return result;
        }

        /// <summary>
        /// True if any active affliction blocks the given duty type.
        /// </summary>
        public bool IsDutyBlocked(IEnumerable<string> activeAfflictionIds, string dutyType)
        {
            if (activeAfflictionIds == null || string.IsNullOrWhiteSpace(dutyType)) return false;
            var modifiers = CalculateWorkModifiers(activeAfflictionIds);
            return modifiers.IsDutyExcluded(dutyType);
        }
    }
}
