using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Trade Specialty System — pre-war professions unlock specialized perk
    /// trees as survivors craft related items, turning basic tasks into
    /// narrative milestones.
    ///
    /// Professions: electrician, nurse, machinist, teacher.
    /// Each has 3 milestone tiers; completing all 3 masters the trade.
    ///
    /// Plain C#, leaf assembly. Host injects crafting event listener.
    /// </summary>
    public class TradeSpecialtySystem
    {
        // ── Constants ──────────────────────────────────────────────────
        public const int MilestonesToMaster = 3;
        public const float MasterySkillBonus = 0.15f;
        public const float MasteryMoraleBonus = 10f;

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
        public event Action<Survivor, string, int> OnSpecialtyMilestone;
        // sv, professionId, milestoneTier (1-3)
        public event Action<Survivor, string> OnSpecialtyMastered;
        // sv, professionId

        // ── Host hooks ─────────────────────────────────────────────────
        public Action<Survivor, string, float> GrantSkillBonus;
        public Action<Survivor, float> ApplyMoraleDelta;
        public Func<string> GetNarrativeEventId;
        // professionId → narrativeEventId
        public Action<string, Survivor> FireNarrativeEvent;

        /// <summary>
        /// Called when a survivor crafts an item. Checks if the item
        /// matches their profession's specialty tree.
        /// </summary>
        public void OnItemCrafted(Survivor sv, string itemId)
        {
            if (sv == null || !sv.IsAlive) return;
            if (string.IsNullOrEmpty(sv.PreWarProfessionId)) return;
            if (sv.CraftMilestonesCompleted.Count >= MilestonesToMaster) return;

            if (!ProfessionItemCategories.TryGetValue(sv.PreWarProfessionId,
                out var categories))
                return;

            // Check if item matches profession
            bool matches = false;
            for (int i = 0; i < categories.Count; i++)
            {
                if (itemId != null && itemId.IndexOf(categories[i],
                    StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    matches = true;
                    break;
                }
            }
            if (!matches) return;

            // Check this item hasn't already been counted as a milestone
            string milestoneId = $"{sv.PreWarProfessionId}_{itemId}";
            if (sv.CraftMilestonesCompleted.Contains(milestoneId))
                return;

            sv.CraftMilestonesCompleted.Add(milestoneId);
            int milestoneTier = sv.CraftMilestonesCompleted.Count;
            OnSpecialtyMilestone?.Invoke(sv, sv.PreWarProfessionId,
                Math.Min(milestoneTier, MilestonesToMaster));

            if (milestoneTier >= MilestonesToMaster)
            {
                MasterTrade(sv);
            }
            else
            {
                // Intermediate milestone — small boost
                GrantSkillBonus?.Invoke(sv, sv.PreWarProfessionId,
                    MasterySkillBonus * 0.3f);
            }
        }

        private void MasterTrade(Survivor sv)
        {
            GrantSkillBonus?.Invoke(sv, sv.PreWarProfessionId, MasterySkillBonus);
            ApplyMoraleDelta?.Invoke(sv, MasteryMoraleBonus);
            OnSpecialtyMastered?.Invoke(sv, sv.PreWarProfessionId);

            // Fire narrative event
            string narrativeId = GetNarrativeEventId?.Invoke(sv.PreWarProfessionId);
            if (!string.IsNullOrEmpty(narrativeId))
                FireNarrativeEvent?.Invoke(narrativeId, sv);
        }

        /// <summary>
        /// Get the mastery tier for a survivor's profession (0-3).
        /// </summary>
        public int GetMasteryTier(Survivor sv)
        {
            if (sv == null || sv.CraftMilestonesCompleted == null) return 0;
            return Math.Min(sv.CraftMilestonesCompleted.Count, MilestonesToMaster);
        }

        /// <summary>
        /// True if survivor has mastered their pre-war trade.
        /// </summary>
        public bool HasMasteredTrade(Survivor sv)
        {
            return GetMasteryTier(sv) >= MilestonesToMaster;
        }
    }
}
