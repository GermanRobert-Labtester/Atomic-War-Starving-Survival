using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// A recurring non-player character (lore bible 04_ENCOUNTERS Part I).
    /// Faces without institutions: each wants something the player can
    /// actually give, and none of them will ask twice. Not recruitable —
    /// these are world characters, not bunker survivors (see survivors.json).
    /// </summary>
    [Serializable]
    public class CharacterEntry
    {
        public string id;
        public string display_name;
        public string profession;
        public string bio;
        /// <summary>Economy-namespace faction id (FactionSO.Ids) or "none".</summary>
        public string faction;
        public string region;
        public int first_day;
        /// <summary>Location id where the character is found ("" = roaming).</summary>
        public string location_id;
        public string[] wants;
        public string[] offers;
        public string[] will_not;
        public string signature_quote;
    }

    /// <summary>
    /// Loads characters.json (flat, JsonUtility-safe). Follows the
    /// PhantomTriggerCatalogLoader pattern.
    /// </summary>
    public static class CharactersCatalogLoader
    {
        [Serializable]
        private class CharactersContainer
        {
            public CharacterEntry[] entries;
        }

        private static List<CharacterEntry> _cache;

        /// <summary>All character entries in file order. Cached after first load.</summary>
        public static List<CharacterEntry> Load()
        {
            if (_cache != null) return _cache;
            _cache = new List<CharacterEntry>();

            string path = Path.Combine(Application.streamingAssetsPath, "Data/characters.json");
            if (!File.Exists(path)) return _cache;

            string json = File.ReadAllText(path);
            var wrapped = "{\"entries\":" + json + "}";
            var container = JsonUtility.FromJson<CharactersContainer>(wrapped);
            if (container?.entries == null) return _cache;

            for (int i = 0; i < container.entries.Length; i++)
            {
                var e = container.entries[i];
                if (e == null || string.IsNullOrEmpty(e.id)) continue;
                _cache.Add(e);
            }
            return _cache;
        }

        /// <summary>Look up a character by snake_case id (null-safe).</summary>
        public static CharacterEntry GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var all = Load();
            for (int i = 0; i < all.Count; i++)
                if (all[i].id == id) return all[i];
            return null;
        }

        /// <summary>
        /// Characters whose location_id matches the given location — the
        /// people the player meets by going there (lore bible wiring).
        /// </summary>
        public static List<CharacterEntry> AtLocation(string locationId)
        {
            var result = new List<CharacterEntry>();
            if (string.IsNullOrEmpty(locationId)) return result;
            var all = Load();
            for (int i = 0; i < all.Count; i++)
                if (all[i].location_id == locationId)
                    result.Add(all[i]);
            return result;
        }
    }
}
