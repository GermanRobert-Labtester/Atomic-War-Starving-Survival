using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// Loads deep_lore_survivor_fields.json — personalised identity fields for
    /// the 4 named arc survivors — onto their Survivor instances. Runs after
    /// GameBootstrap.AssignInitialKeepsakes, so these values intentionally
    /// override that method's generic background-based keepsake fallback.
    /// </summary>
    public static class DeepLoreSurvivorFieldsLoader
    {
        [Serializable]
        private class SurvivorFieldsEntry
        {
            public string survivor_id;
            public string phantom_background_id;
            public string pre_war_profession_id;
            public string belief_profile_id;
            public string personal_keepsake_item_id;
            public string philosophical_stance;
        }

        [Serializable]
        private class SurvivorFieldsContainer
        {
            public SurvivorFieldsEntry[] entries;
        }

        public static void LoadInto(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return;
            string path = Path.Combine(Application.streamingAssetsPath, "Data/deep_lore_survivor_fields.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var wrapped = "{\"entries\":" + json + "}";
            var container = JsonUtility.FromJson<SurvivorFieldsContainer>(wrapped);
            if (container?.entries == null) return;

            for (int i = 0; i < container.entries.Length; i++)
            {
                var entry = container.entries[i];
                if (entry == null || string.IsNullOrEmpty(entry.survivor_id)) continue;

                Survivor sv = null;
                for (int s = 0; s < survivors.Count; s++)
                {
                    if (survivors[s] != null && survivors[s].Id == entry.survivor_id)
                    {
                        sv = survivors[s];
                        break;
                    }
                }
                if (sv == null) continue;

                if (!string.IsNullOrEmpty(entry.phantom_background_id))
                    sv.PhantomBackgroundId = entry.phantom_background_id;
                if (!string.IsNullOrEmpty(entry.pre_war_profession_id))
                    sv.PreWarProfessionId = entry.pre_war_profession_id;
                if (!string.IsNullOrEmpty(entry.belief_profile_id))
                    sv.BeliefProfileId = entry.belief_profile_id;
                if (!string.IsNullOrEmpty(entry.personal_keepsake_item_id))
                    sv.PersonalKeepsakeItemId = entry.personal_keepsake_item_id;
                if (!string.IsNullOrEmpty(entry.philosophical_stance))
                    sv.Stance = ParseStance(entry.philosophical_stance);
            }
        }

        private static PhilosophicalStance ParseStance(string value)
        {
            switch (value)
            {
                case "stoicism": return PhilosophicalStance.Stoicism;
                case "faith": return PhilosophicalStance.Faith;
                case "material_rationalism": return PhilosophicalStance.MaterialRationalism;
                default: return PhilosophicalStance.Undecided;
            }
        }
    }
}
