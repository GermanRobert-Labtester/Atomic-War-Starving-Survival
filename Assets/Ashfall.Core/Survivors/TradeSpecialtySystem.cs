// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS8618

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

    // ── Authored profession content (trade_specialties.json) ─────────
    // Presentation/reference data keyed by milestone tier. Content only, never
    // per-run state, so none of it belongs in TradeSpecialtySaveState.

    [Serializable]
    public sealed class TradeSpecialtyMilestoneInfo
    {
        public int Tier = 1;
        public string Title = string.Empty;
        public string NarrativeId = string.Empty;

        /// <summary>Authored per-milestone bonus. Retained so it is queryable and no
        /// longer silently dropped; the runtime still applies the constants below.</summary>
        public float SkillBonus;
    }

    [Serializable]
    public sealed class TradeSpecialtyProfessionInfo
    {
        public string ProfessionId = string.Empty;
        public string DisplayName = string.Empty;
        public string MasteryNarrativeId = string.Empty;
        public string MasteryBonusText = string.Empty;
        public List<string> Aliases = new List<string>();
        public Dictionary<int, TradeSpecialtyMilestoneInfo> Milestones =
            new Dictionary<int, TradeSpecialtyMilestoneInfo>();
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

        public static void RegisterProfessionPatterns(string professionId, IEnumerable<string> patterns)
        {
            if (string.IsNullOrEmpty(professionId) || patterns == null) return;
            if (!ProfessionItemCategories.TryGetValue(professionId, out var list))
            {
                list = new List<string>();
                ProfessionItemCategories[professionId] = list;
            }
            foreach (var p in patterns)
            {
                if (!string.IsNullOrEmpty(p) && !list.Contains(p))
                    list.Add(p);
            }
        }

        /// <summary>
        /// True when the item matches the profession's authored crafting patterns.
        /// This is the same rule OnItemCrafted applies, exposed so a host can
        /// attribute an unassigned craft without duplicating the match logic.
        /// </summary>
        public static bool ProfessionMatchesItem(string professionId, string itemId)
        {
            if (string.IsNullOrEmpty(professionId) || string.IsNullOrEmpty(itemId)) return false;
            if (!ProfessionItemCategories.TryGetValue(professionId, out var categories)) return false;
            for (int i = 0; i < categories.Count; i++)
            {
                if (itemId.IndexOf(categories[i], StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }
            return false;
        }

        // ── Profession content registry (fed by TradeSpecialtyCatalogLoader) ──
        public static readonly Dictionary<string, TradeSpecialtyProfessionInfo> ProfessionInfo =
            new Dictionary<string, TradeSpecialtyProfessionInfo>(StringComparer.Ordinal);

        /// <summary>Registers authored profession content. Re-registering the same
        /// profession replaces it, so reloading the catalog cannot duplicate state.</summary>
        public static void RegisterProfessionInfo(TradeSpecialtyProfessionInfo? info)
        {
            if (info == null || string.IsNullOrEmpty(info.ProfessionId)) return;
            ProfessionInfo[info.ProfessionId] = info;
            RebuildProfessionLabelIndex();
        }

        public static TradeSpecialtyProfessionInfo? GetProfessionInfo(string professionId)
        {
            if (string.IsNullOrEmpty(professionId)) return null;
            return ProfessionInfo.TryGetValue(professionId, out var info) ? info : null;
        }

        public static TradeSpecialtyMilestoneInfo? GetMilestone(string professionId, int tier)
        {
            var info = GetProfessionInfo(professionId);
            if (info == null || info.Milestones == null) return null;
            return info.Milestones.TryGetValue(tier, out var milestone) ? milestone : null;
        }

        public static string GetDisplayName(string professionId)
            => GetProfessionInfo(professionId)?.DisplayName ?? string.Empty;

        /// <summary>Authored mastery narrative event id, or empty when the profession
        /// has no catalog entry. Empty means MasterTrade fires nothing rather than
        /// inventing an id that events.json cannot resolve.</summary>
        public static string GetMasteryNarrativeId(string professionId)
            => GetProfessionInfo(professionId)?.MasteryNarrativeId ?? string.Empty;

        public static string GetMasteryBonusText(string professionId)
            => GetProfessionInfo(professionId)?.MasteryBonusText ?? string.Empty;

        public static string GetMilestoneTitle(string professionId, int tier)
            => GetMilestone(professionId, tier)?.Title ?? string.Empty;

        public static string GetMilestoneNarrativeId(string professionId, int tier)
            => GetMilestone(professionId, tier)?.NarrativeId ?? string.Empty;

        // ── Profession label → specialty id resolution ─────────────────
        // Survivor rosters author display labels ("Trauma Surgeon") while
        // specialty trees are keyed by id (bone_setter). The index is rebuilt
        // from the registry in ordinal profession-id order, so resolution never
        // depends on catalog load order or registration sequence.
        private static readonly Dictionary<string, string> s_professionLabelIndex =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public static IReadOnlyDictionary<string, string> ProfessionLabelIndex => s_professionLabelIndex;

        private static void RebuildProfessionLabelIndex()
        {
            s_professionLabelIndex.Clear();
            var ids = new List<string>(ProfessionInfo.Keys);
            ids.Sort(StringComparer.Ordinal);
            for (int i = 0; i < ids.Count; i++)
            {
                var info = ProfessionInfo[ids[i]];
                if (info == null || info.Aliases == null) continue;
                for (int a = 0; a < info.Aliases.Count; a++)
                {
                    string alias = info.Aliases[a]?.Trim() ?? string.Empty;
                    if (alias.Length == 0) continue;
                    if (!s_professionLabelIndex.ContainsKey(alias))
                        s_professionLabelIndex[alias] = info.ProfessionId;
                }
            }
        }

        /// <summary>
        /// Resolve a survivor's specialty id. An explicit authored
        /// pre_war_profession_id always wins; otherwise the roster display label
        /// is matched against authored profession_aliases. Empty means the
        /// survivor has no trade specialty tree.
        /// </summary>
        public static string ResolveProfessionId(string? explicitProfessionId, string? professionLabel)
        {
            if (!string.IsNullOrWhiteSpace(explicitProfessionId))
                return explicitProfessionId.Trim();
            return ResolveProfessionIdFromLabel(professionLabel);
        }

        public static string ResolveProfessionIdFromLabel(string? professionLabel)
        {
            if (string.IsNullOrWhiteSpace(professionLabel)) return string.Empty;
            return s_professionLabelIndex.TryGetValue(professionLabel.Trim(), out var id) ? id : string.Empty;
        }

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

            if (!ProfessionMatchesItem(professionId, itemId)) return;

            string milestoneId = professionId + "_" + itemId;
            if (state.craftMilestonesCompleted.Contains(milestoneId))
                return;

            state.craftMilestonesCompleted.Add(milestoneId);
            int milestoneTier = Math.Min(state.craftMilestonesCompleted.Count, MilestonesToMaster);
            OnSpecialtyMilestone?.Invoke(survivorId, professionId, milestoneTier);

            // Authored per-tier narrative (milestones[].narrative). Fires on every tier
            // including the third, so a mastery craft emits both its tier narrative and
            // the profession-level mastery_narrative from MasterTrade below.
            string tierNarrativeId = GetMilestoneNarrativeId(professionId, milestoneTier);
            if (!string.IsNullOrEmpty(tierNarrativeId))
                FireNarrativeEvent?.Invoke(tierNarrativeId, survivorId);

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

            // Authored mastery_narrative from trade_specialties.json is the authority.
            // GetNarrativeEventId remains only as a fallback for professions the catalog
            // does not cover, so the host hook cannot override authored content.
            string narrativeId = GetMasteryNarrativeId(state.professionId);
            if (string.IsNullOrEmpty(narrativeId))
                narrativeId = GetNarrativeEventId?.Invoke(state.professionId) ?? string.Empty;
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
