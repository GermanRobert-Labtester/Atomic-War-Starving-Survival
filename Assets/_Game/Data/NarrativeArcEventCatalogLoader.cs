using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using AtomicWar._Game.Events;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// Loads narrative_arc_events.json into GameEvent instances for the
    /// EventRunner pool. Milestone/branch advancement is driven entirely by
    /// GameBootstrap.WireNarrativeArcTriggers matching on event id / choice id
    /// substrings, so the JSON's "effects" array (author-facing intent notes)
    /// is not translated into EventEffect data here.
    /// </summary>
    public static class NarrativeArcEventCatalogLoader
    {
        [Serializable]
        private class ArcChoiceEntry
        {
            public string choiceId;
            public string text;
            public float moraleDelta;
        }

        [Serializable]
        private class ArcEventEntry
        {
            public string id;
            public string title;
            public string bodyText;
            public float weight;
            public int minDay;
            public ArcChoiceEntry[] choices;
        }

        [Serializable]
        private class ArcEventContainer
        {
            public ArcEventEntry[] entries;
        }

        public static List<GameEvent> Load()
        {
            var result = new List<GameEvent>();
            string path = Path.Combine(Application.streamingAssetsPath, "Data/narrative_arc_events.json");
            if (!File.Exists(path)) return result;

            string json = File.ReadAllText(path);
            var wrapped = "{\"entries\":" + json + "}";
            var container = JsonUtility.FromJson<ArcEventContainer>(wrapped);
            if (container?.entries == null) return result;

            for (int i = 0; i < container.entries.Length; i++)
            {
                var entry = container.entries[i];
                if (entry == null || string.IsNullOrEmpty(entry.id)) continue;

                var ev = ScriptableObject.CreateInstance<GameEvent>();
                ev.id = entry.id;
                ev.title = entry.title;
                ev.bodyText = entry.bodyText;
                ev.weight = entry.weight > 0f ? entry.weight : 1f;
                ev.minDay = entry.minDay;
                ev.choices = new List<EventChoice>();

                if (entry.choices != null)
                {
                    for (int c = 0; c < entry.choices.Length; c++)
                    {
                        var choiceEntry = entry.choices[c];
                        if (choiceEntry == null || string.IsNullOrEmpty(choiceEntry.choiceId)) continue;
                        ev.choices.Add(new EventChoice
                        {
                            ChoiceId = choiceEntry.choiceId,
                            Text = choiceEntry.text,
                            MoraleDelta = choiceEntry.moraleDelta
                        });
                    }
                }

                result.Add(ev);
            }

            return result;
        }
    }
}
