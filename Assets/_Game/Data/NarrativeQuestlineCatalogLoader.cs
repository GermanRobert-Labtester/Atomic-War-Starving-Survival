using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using AtomicWar._Game.Quests;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// Loads narrative_questlines.json (the 4 named survivors' arc quests)
    /// into DynamicQuestlineSystem so their stage progression is trackable
    /// via QuestlineProgressTracker/QuestlineStageTracker. Branch text and
    /// trait rewards for these quests are owned by SurvivorNarrativeArcSystem's
    /// CharacterBranches and are not duplicated here.
    /// </summary>
    public static class NarrativeQuestlineCatalogLoader
    {
        [Serializable]
        private class Stage
        {
            public int stage;
            public string name;
            public string description;
            public string[] objective_items;
        }

        [Serializable]
        private class QuestEntry
        {
            public string quest_id;
            public string survivor_id;
            public string title;
            public string target_location_id;
            public Stage[] stages;
        }

        [Serializable]
        private class QuestContainer
        {
            public QuestEntry[] entries;
        }

        // DynamicQuestState hardcodes TotalStages = 4 (DynamicQuestlineSystem.StartQuest);
        // stage arrays must match that contract regardless of how many stages a
        // given JSON entry authors, or later stages become unreachable.
        private const int FixedStageCount = 4;

        public static void LoadInto(DynamicQuestlineSystem system)
        {
            if (system == null) return;
            string path = Path.Combine(Application.streamingAssetsPath, "Data/narrative_questlines.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var wrapped = "{\"entries\":" + json + "}";
            var container = JsonUtility.FromJson<QuestContainer>(wrapped);
            if (container?.entries == null) return;

            for (int i = 0; i < container.entries.Length; i++)
            {
                var entry = container.entries[i];
                if (entry == null || string.IsNullOrEmpty(entry.quest_id) || entry.stages == null) continue;
                if (entry.stages.Length != FixedStageCount)
                    Debug.LogWarning($"[NarrativeQuestlineCatalogLoader] '{entry.quest_id}' has " +
                        $"{entry.stages.Length} stages; DynamicQuestlineSystem expects {FixedStageCount}.");

                var descriptions = new string[FixedStageCount];
                var objectiveItems = new List<string>();
                for (int s = 0; s < entry.stages.Length; s++)
                {
                    var stage = entry.stages[s];
                    if (stage == null) continue;
                    if (stage.stage >= 0 && stage.stage < descriptions.Length)
                        descriptions[stage.stage] = stage.description;
                    if (stage.objective_items != null)
                        objectiveItems.AddRange(stage.objective_items);
                }

                system.StartQuest(
                    entry.quest_id,
                    entry.title,
                    descriptions,
                    entry.target_location_id,
                    objectiveItems.ToArray());
            }
        }
    }
}
