// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Survivors
{
    [Serializable]
    public sealed class LatentAwakeningRecord
    {
        public string survivorId = string.Empty;
        public string traitId = string.Empty;
        public string skillId = string.Empty;
        public int awakenedDay = -1;
        public string contextReason = string.Empty;
        public int progressCount = 0;
    }

    [Serializable]
    public sealed class LatentAwakeningSaveState
    {
        public string systemId = LatentExpertAwakeningSystem.SystemId;
        public List<LatentAwakeningRecord> records = new List<LatentAwakeningRecord>();
    }

    public sealed class LatentAwakeningDefinition
    {
        public string TraitId { get; }
        public string SkillId { get; }
        public string DisplayName { get; }
        public string Discipline { get; }
        public int RequiredCount { get; }
        public string ConditionDescription { get; }

        public LatentAwakeningDefinition(
            string traitId,
            string skillId,
            string displayName,
            string discipline,
            int requiredCount,
            string conditionDescription)
        {
            TraitId = traitId;
            SkillId = skillId;
            DisplayName = displayName;
            Discipline = discipline;
            RequiredCount = requiredCount;
            ConditionDescription = conditionDescription;
        }
    }

    /// <summary>
    /// Coordinates deterministic in-game awakening of dormant survivor expert traits.
    /// When survivors with latent traits perform relevant high-stress, master-level tasks,
    /// their hidden expertise awakens into active skills in <see cref="SkillProgressionSystem"/>.
    /// </summary>
    public sealed class LatentExpertAwakeningSystem
    {
        public const string SystemId = "latent_expert_awakening_system";

        private readonly Dictionary<string, LatentAwakeningDefinition> _definitions =
            new Dictionary<string, LatentAwakeningDefinition>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, LatentAwakeningRecord> _recordsByKey =
            new Dictionary<string, LatentAwakeningRecord>(StringComparer.OrdinalIgnoreCase);

        private readonly SkillProgressionSystem? _skillSystem;
        private readonly ILog _log;

        public event Action<string, string, string, int>? OnTraitAwakened;
        public event Action? OnStateChanged;

        public LatentExpertAwakeningSystem(
            SkillProgressionSystem? skillSystem = null,
            ILog? log = null)
        {
            _skillSystem = skillSystem;
            _log = log ?? NullLog.Instance;
            RegisterDefaultAwakeningDefinitions();
        }

        private void RegisterDefaultAwakeningDefinitions()
        {
            RegisterDefinition(new LatentAwakeningDefinition("trait_miracle_worker", "skill_miracle_worker", "Miracle Worker", "medical", 1, "Complete emergency surgery on a critically injured patient."));
            RegisterDefinition(new LatentAwakeningDefinition("trait_alchemist", "skill_alchemist", "Alchemist", "science", 5, "Synthesize 5 clean chemical or medical reagents."));
            RegisterDefinition(new LatentAwakeningDefinition("trait_grease_monkey", "skill_grease_monkey", "Grease Monkey", "crafting", 3, "Repair 3 generator or vehicle breakdowns."));
            RegisterDefinition(new LatentAwakeningDefinition("trait_grid_walker", "skill_grid_walker", "Grid Walker", "crafting", 3, "Restore or stabilize 3 high-voltage power conduits."));
            RegisterDefinition(new LatentAwakeningDefinition("trait_iron_chef", "skill_iron_chef", "Iron Chef", "survival", 10, "Prepare 10 morale-boosting meals during food pressure."));
            RegisterDefinition(new LatentAwakeningDefinition("trait_armorer", "skill_armorer", "Armorer", "crafting", 3, "Fabricate or reinforce 3 ballistic armor plates."));
            RegisterDefinition(new LatentAwakeningDefinition("trait_tinkerer", "skill_tinkerer", "Tinkerer", "crafting", 2, "Reverse engineer or optimize 2 electronic relics."));
            RegisterDefinition(new LatentAwakeningDefinition("trait_wasteland_scout", "skill_wasteland_scout", "Wasteland Scout", "scavenging", 5, "Complete 5 sector reconnaissance expeditions."));
            RegisterDefinition(new LatentAwakeningDefinition("trait_demolitions_expert", "skill_demolitions_expert", "Demolitions Expert", "combat", 2, "Safely breach or clear 2 collapsed blast zones."));
            RegisterDefinition(new LatentAwakeningDefinition("trait_supply_chain_master", "skill_supply_chain_master", "Supply Chain Master", "scavenging", 5, "Execute 5 zero-loss trade convoy transactions."));
            RegisterDefinition(new LatentAwakeningDefinition("trait_forge_master", "skill_forge_master", "Forge Master", "crafting", 5, "Smelt 5 high-grade tool steel or alloy ingots."));
            RegisterDefinition(new LatentAwakeningDefinition("trait_sanitization_expert", "skill_sanitization_expert", "Sanitization Expert", "medical", 3, "Decontaminate 3 severe bio/rad infection hotspots."));
        }

        public void RegisterDefinition(LatentAwakeningDefinition def)
        {
            if (def == null || string.IsNullOrEmpty(def.TraitId)) return;
            _definitions[def.TraitId] = def;
        }

        public LatentAwakeningDefinition? GetDefinition(string traitId)
        {
            if (string.IsNullOrEmpty(traitId)) return null;
            return _definitions.TryGetValue(traitId, out var def) ? def : null;
        }

        private static string MakeKey(string survivorId, string traitId) => $"{survivorId}:{traitId}";

        public bool IsAwakened(string survivorId, string traitId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(traitId)) return false;
            string key = MakeKey(survivorId, traitId);
            return _recordsByKey.TryGetValue(key, out var rec) && rec.awakenedDay >= 0;
        }

        public int GetProgress(string survivorId, string traitId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(traitId)) return 0;
            string key = MakeKey(survivorId, traitId);
            return _recordsByKey.TryGetValue(key, out var rec) ? rec.progressCount : 0;
        }

        /// <summary>
        /// Record incremental progress toward awakening a latent expert trait.
        /// If the progress meets the required threshold, the trait awakens into an active skill.
        /// </summary>
        public bool RecordProgress(
            string survivorId,
            string traitId,
            int increment,
            int currentDay,
            string contextReason,
            SkillActor? actor = null)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(traitId) || increment <= 0)
                return false;

            if (!_definitions.TryGetValue(traitId, out var def))
                return false;

            string key = MakeKey(survivorId, traitId);
            if (!_recordsByKey.TryGetValue(key, out var rec))
            {
                rec = new LatentAwakeningRecord
                {
                    survivorId = survivorId,
                    traitId = traitId,
                    skillId = def.SkillId,
                    progressCount = 0,
                    awakenedDay = -1
                };
                _recordsByKey[key] = rec;
            }

            if (rec.awakenedDay >= 0)
                return false; // Already awakened

            rec.progressCount += increment;

            if (rec.progressCount >= def.RequiredCount)
            {
                rec.awakenedDay = currentDay;
                rec.contextReason = contextReason;

                if (_skillSystem != null && actor != null)
                {
                    _skillSystem.TryGrantSkill(actor, def.SkillId, currentDay);
                }

                _log.Info($"[LatentAwakening] Survivor '{survivorId}' awakened '{def.DisplayName}' ({def.TraitId} -> {def.SkillId}) on Day {currentDay} via '{contextReason}'");
                OnTraitAwakened?.Invoke(survivorId, traitId, def.SkillId, currentDay);
                OnStateChanged?.Invoke();
                return true;
            }

            OnStateChanged?.Invoke();
            return false;
        }

        /// <summary>
        /// Directly awaken a latent trait (e.g. Narrative milestone, quest choice, or immediate epiphany).
        /// </summary>
        public bool TryAwaken(
            string survivorId,
            string traitId,
            int currentDay,
            string contextReason,
            SkillActor? actor = null)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(traitId)) return false;
            if (!_definitions.TryGetValue(traitId, out var def)) return false;

            return RecordProgress(survivorId, traitId, def.RequiredCount, currentDay, contextReason, actor);
        }

        public LatentAwakeningSaveState CaptureState()
        {
            var state = new LatentAwakeningSaveState { systemId = SystemId };
            foreach (var kvp in _recordsByKey)
            {
                state.records.Add(new LatentAwakeningRecord
                {
                    survivorId = kvp.Value.survivorId,
                    traitId = kvp.Value.traitId,
                    skillId = kvp.Value.skillId,
                    awakenedDay = kvp.Value.awakenedDay,
                    contextReason = kvp.Value.contextReason,
                    progressCount = kvp.Value.progressCount
                });
            }
            return state;
        }

        public void RestoreState(LatentAwakeningSaveState? state)
        {
            _recordsByKey.Clear();
            if (state?.records == null) return;

            foreach (var r in state.records)
            {
                if (r != null && !string.IsNullOrEmpty(r.survivorId) && !string.IsNullOrEmpty(r.traitId))
                {
                    string key = MakeKey(r.survivorId, r.traitId);
                    _recordsByKey[key] = new LatentAwakeningRecord
                    {
                        survivorId = r.survivorId,
                        traitId = r.traitId,
                        skillId = r.skillId,
                        awakenedDay = r.awakenedDay,
                        contextReason = r.contextReason,
                        progressCount = r.progressCount
                    };
                }
            }
            OnStateChanged?.Invoke();
        }
    }
}
