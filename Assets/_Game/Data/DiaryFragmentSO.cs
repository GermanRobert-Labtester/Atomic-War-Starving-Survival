using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// A diary fragment found while clearing rubble in sealed bunker rooms.
    /// These serve as diegetic warnings about the bunker's systems — the
    /// previous tenants' last words, found too late.
    ///
    /// Authored as ScriptableObject assets or imported from JSON.
    /// Displayed via JournalSystem as a discovery entry.
    /// </summary>
    [CreateAssetMenu(fileName = "NewDiaryFragment", menuName = "ASHFALL/Diary Fragment")]
    public class DiaryFragmentSO : ScriptableObject
    {
        [Header("Identity")]
        /// <summary>Snake_case unique id (e.g. "diary_filter_is_a_lie").</summary>
        public string id;

        /// <summary>Display title shown in the journal (e.g. "Torn Notebook Page").</summary>
        public string title;

        [Header("Content")]
        /// <summary>The diary text. Keep it restrained — cold, exhausted, human.
        /// Show, don't preach. This is a diegetic warning about a specific system.</summary>
        [TextArea(4, 12)]
        public string text;

        [Header("Meta")]
        /// <summary>Who wrote this? (e.g. "Unknown", "Elena's handwriting").</summary>
        public string authorName;

        /// <summary>Room id where this fragment is found. Links to the sealed room.</summary>
        public string foundInRoomId;

        /// <summary>Optional: the system this fragment warns about
        /// (e.g. "air_filtration", "water_purifier", "radiation_shielding").</summary>
        public string warnsAboutSystemId;

        /// <summary>Order in a multi-page sequence (0 = standalone).</summary>
        public int pageOrder;

        /// <summary>Total pages in this sequence (1 = standalone).</summary>
        public int totalPages;

        /// <summary>True if this fragment has been found by the player.
        /// Runtime state; set by the diary discovery system.</summary>
        [System.NonSerialized]
        public bool IsFound;

        /// <summary>Formatted journal entry key (snake_case, used for dedup).</summary>
        public string JournalKey => $"diary_{id}";
    }
}
