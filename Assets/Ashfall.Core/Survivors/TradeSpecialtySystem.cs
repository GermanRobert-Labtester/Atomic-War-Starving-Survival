using System;
using System.Collections.Generic;

namespace Ashfall.Core.Survivors
{
    // ── Save/load DTOs ───────────────────────────────────────────────
    [Serializable]
    public sealed class TradeSpecialtySurvivorState
    {
        public string survivorId = string.Empty;
        public string professionId = string.Empty;
        public List<string> craftMilestonesCompleted = new List<string>();
        public bool mastered;
    }

    [Serializable]
    public sealed class TradeSpecialtySaveState
    {
        public string systemId = TradeSpecialtySystem.SystemId;
        public List<TradeSpecialtySurvivorState> survivors = new List<TradeSpecialtySurvivorState>();
    }

    /// <summary>
    /// Trade Specialty System — pre-war professions unlock specialized perk
    /// trees as survivors craft related items, turning basic tasks into
    /// narrative milestones.
    ///
    /// Professions: electrician, nurse, machinist, teacher. Each has 3
    /// milestone tiers; completing all 3 masters the trade.
    ///
    /// Engine-agnostic port: operates on string survivor ids (no engine
    /// Survivor object), raises C# events on milestone/mastery, and is
    /// save/load safe via CaptureState/RestoreState (deep copy). Host injects
    /// the skill-bonus / morale / narrative hooks in its own domain.
    /// All constants match the Unity source 1:1.
    /// </summary>
    public class TradeSpecialtySystem
    {
        public const string SystemId = "trade_specialty_system";

        // ── Constants (match Unity source 1:1) ────────────────────────
        public const int MilestonesToMaster = 3;
        public const float MasterySkillBonus = 0.15f;
        public const float MasteryMoraleBonus = 10f;
        public const float MilestoneSkillBonusFactor = 0.3f;

        // ── Profession crafting categories (item id prefixes) ──────────
        public static readonly Dictionary<string, List<string>> ProfessionItemCategories =
            new Dictionary<string, List<string>>
            {
                { "electrician", new List<string> { "battery", "generator", "circuit", "wire", "power", "solar", "turbine" } },
                { "nurse", new List<string> { "bandage", "splint", "antiseptic", "saline", "stitch", "tourniquet", "medical" } },
                { "machinist", new List<string> { "wrench", "tool", "gear", "spring", "lever", "blade", "mechanism", "lathe" } },
                { "teacher", new List<string> { "book", "chalk", "slate", "journal", "ink", "lesson", "diagram" } }
            };

        // ── Events ─────────────────────────────────────────────────────
        /// <summary>SurvivorId, professionId, milestoneTier (1-3).</summary>
        public event Action<string, string, int> OnSpecialtyMilestone;
        /// <summary>SurvivorId, professionId — all 3 tiers completed.</summary>
        public event Action<string, string> OnSpecialtyMastered;
        public event Action OnStateChanged;

        // ── Host hooks (hosts apply the effects in their own domain) ──
        public Action<string, string, float> GrantSkillBonus;
        // survivorId, professionId, bonus
        public Action<string, float> ApplyMoraleDelta;
        // survivorId, delta
        public Func<string, string> GetNarrativeEventId;
        // professionId → narrativeEventId
        public Action<string, string> FireNarrativeEvent;
        // narrativeEventId, survivorId

        // ── State ──────────────────────────────────────────────────────
        private readonly Dictionary<string, TradeSpecialtySurvivorState> _bySurvivor =
            new Dictionary<string, TradeSpecialtySurvivorState>(StringComparer.Ordinal);

        private TradeSpecialtySurvivorState GetOrCreate(string survivorId, string professionId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var state))
            {
                state = new TradeSpecialtySurvivorState
                {
                    survivorId = survivorId,
                    professionId = professionId ?? string.Empty
                };
                _bySurvivor[survivorId] = state;
            }
            return state;
        }

        /// <summary>
        /// Called when a survivor crafts an item. Checks if the item matches
        /// their profession's specialty tree.
        /// </summary>
        public void OnItemCrafted(string survivorId, string professionId, string itemId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(professionId)) return;
            if (string.IsNullOrEmpty(itemId)) return;

            var state = GetOrCreate(survivorId, professionId);
            if (state.mastered || state.craftMilestonesCompleted.Count >= MilestonesToMaster)
                return; // matches Unity: count drives the guard, not just the flag
            if (!string.Equals(state.professionId, professionId, StringComparison.Ordinal))
                return; // profession changed — do not count toward an old tree

            if (!ProfessionItemCategories.TryGetValue(professionId, out var categories))
                return;

            bool matches = false;
            for (int i = 0; i < categories.Count; i++)
            {
                if (itemId.IndexOf(categories[i], StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    matches = true;
                    break;
                }
            }
            if (!matches) return;

            string milestoneId = professionId + "_" + itemId;
            if (state.craftMilestonesCompleted.Contains(milestoneId))
                return;

            state.craftMilestonesCompleted.Add(milestoneId);
            int milestoneTier = Math.Min(state.craftMilestonesCompleted.Count, MilestonesToMaster);
            OnSpecialtyMilestone?.Invoke(survivorId, professionId, milestoneTier);

            if (state.craftMilestonesCompleted.Count >= MilestonesToMaster)
            {
                MasterTrade(survivorId, state);
            }
            else
            {
                // Intermediate milestone — small boost
                GrantSkillBonus?.Invoke(survivorId, professionId,
                    MasterySkillBonus * MilestoneSkillBonusFactor);
            }
            RaiseChanged();
        }

        private void MasterTrade(string survivorId, TradeSpecialtySurvivorState state)
        {
            state.mastered = true;
            GrantSkillBonus?.Invoke(survivorId, state.professionId, MasterySkillBonus);
            ApplyMoraleDelta?.Invoke(survivorId, MasteryMoraleBonus);
            OnSpecialtyMastered?.Invoke(survivorId, state.professionId);

            // Fire narrative event
            string narrativeId = GetNarrativeEventId?.Invoke(state.professionId);
            if (!string.IsNullOrEmpty(narrativeId))
                FireNarrativeEvent?.Invoke(narrativeId, survivorId);
        }

        /// <summary>
        /// Get the mastery tier for a survivor's profession (0-3).
        /// </summary>
        public int GetMasteryTier(string survivorId)
        {
            return _bySurvivor.TryGetValue(survivorId, out var state)
                ? Math.Min(state.craftMilestonesCompleted.Count, MilestonesToMaster) : 0;
        }

        /// <summary>
        /// True if survivor has mastered their pre-war trade.
        /// </summary>
        public bool HasMasteredTrade(string survivorId)
        {
            return _bySurvivor.TryGetValue(survivorId, out var state) && state.mastered;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public TradeSpecialtySaveState CaptureState()
        {
            var save = new TradeSpecialtySaveState { systemId = SystemId };
            var keys = new List<string>(_bySurvivor.Keys);
            keys.Sort(string.CompareOrdinal);
            for (int i = 0; i < keys.Count; i++)
            {
                var s = _bySurvivor[keys[i]];
                save.survivors.Add(new TradeSpecialtySurvivorState
                {
                    survivorId = s.survivorId,
                    professionId = s.professionId,
                    craftMilestonesCompleted = new List<string>(s.craftMilestonesCompleted),
                    mastered = s.mastered
                });
            }
            return save;
        }

        public void RestoreState(TradeSpecialtySaveState save)
        {
            _bySurvivor.Clear();
            if (save?.survivors == null) return;
            for (int i = 0; i < save.survivors.Count; i++)
            {
                var s = save.survivors[i];
                if (s == null || string.IsNullOrEmpty(s.survivorId)) continue;
                _bySurvivor[s.survivorId] = new TradeSpecialtySurvivorState
                {
                    survivorId = s.survivorId,
                    professionId = s.professionId ?? string.Empty,
                    craftMilestonesCompleted = s.craftMilestonesCompleted != null
                        ? new List<string>(s.craftMilestonesCompleted) : new List<string>(),
                    mastered = s.mastered
                };
            }
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke();
    }
}
