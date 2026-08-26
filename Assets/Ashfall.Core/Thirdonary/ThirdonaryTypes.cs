using System;
using System.Collections.Generic;

namespace Ashfall.Core.Thirdonary
{
    public enum ThirdonaryCategory
    {
        Environmental,
        Crafting,
        Medical,
        Combat,
        Lore,
        Social
    }

    [Serializable]
    public sealed class ThirdonaryQuestDef
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
        public string category = string.Empty;
        public string trigger = string.Empty;
        public string discovery = string.Empty;
        public int min_day = 0;
        public int max_day = 0;
        public int cooldown_days = 0;
        public string difficulty = "easy";
        public string moral_weight = "none";
        public string location_id = string.Empty;
        public List<string> trigger_flags = new List<string>();
        public List<ThirdonaryChoice> choices = new List<ThirdonaryChoice>();
    }

    [Serializable]
    public sealed class ThirdonaryChoice
    {
        public string id = string.Empty;
        public string label = string.Empty;
        public string outcome_text = string.Empty;
        public string epitaph = string.Empty;
        public int moral_delta = 0;
        public int empathy_delta = 0;
        public List<ThirdonaryEffect> effects = new List<ThirdonaryEffect>();
    }

    [Serializable]
    public sealed class ThirdonaryEffect
    {
        public string type = string.Empty;
        public string target = string.Empty;
        public int value = 0;
    }

    [Serializable]
    public sealed class ThirdonaryProgress
    {
        public string quest_id = string.Empty;
        public bool started = false;
        public bool completed = false;
        public bool failed = false;
        public int day_started = 0;
        public int day_resolved = -1;
        public string chosen_choice_id = string.Empty;
        public int last_completed_day = -1;
    }

    [Serializable]
    public sealed class ThirdonaryState
    {
        public string system_id = ThirdonaryQuestSystem.SystemId;
        public int schema_version = 1;
        public List<ThirdonaryProgress> quests = new List<ThirdonaryProgress>();
        public List<string> completed_quest_ids = new List<string>();
        public List<string> failed_quest_ids = new List<string>();
    }

    /// <summary>
    /// Host-computed world state for trigger evaluation.
    /// The host fills this from live game state; Core only reads it.
    /// </summary>
    public sealed class ThirdonaryWorldState
    {
        public int CurrentDay { get; set; }
        public bool PlayerHasFood { get; set; }
        public bool PlayerHasWater { get; set; }
        public bool PlayerHasMedicine { get; set; }
        public bool PlayerHasTools { get; set; }
        public bool PlayerInjured { get; set; }
        public bool ShelterDamaged { get; set; }
        public bool IsNight { get; set; }
        public bool IsStorming { get; set; }
        public bool RaidersNearby { get; set; }
        public string CurrentLocationId { get; set; } = string.Empty;
        public HashSet<string> ActiveFlags { get; set; } = new HashSet<string>(StringComparer.Ordinal);
    }

    [Serializable]
    public sealed class ThirdonaryCatalogRoot
    {
        public int schema_version = 1;
        public List<ThirdonaryQuestDef> quests = new List<ThirdonaryQuestDef>();
    }
}
