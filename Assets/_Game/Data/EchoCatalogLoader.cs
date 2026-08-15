using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using AtomicWar._Game.Events;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// Runtime pipeline for StreamingAssets/Data/echoes.json.
    ///
    /// echoes.json is authored with the full interactive schema (conditions,
    /// choices, effects, delayed consequences) but historically nothing read
    /// it at runtime — the editor importer skips it and the journal only
    /// received C#-authored echoes. This loader materialises every echo into
    /// a GameEvent so EventPoolBuilder can register them into the event pool,
    /// turning found-things vignettes into playable events.
    ///
    /// Mapping notes:
    ///  - conditions.MinDay wins; top-level minDay is the fallback.
    ///  - choice/effect keys mirror GameEvent / EventChoice / EventEffect
    ///    field names exactly (JsonUtility matching).
    ///  - delayedConsequence → DelayedConsequence (delayHours/title/description/effects).
    /// </summary>
    public static class EchoCatalogLoader
    {
        [Serializable]
        private class EchoEffectEntry
        {
            public string targetNeed;
            public float needDelta;
            public string itemId;
            public int itemAmount;
            public string setWorldFlag;
            public bool worldFlagValue;
        }

        [Serializable]
        private class EchoDelayedConsequenceEntry
        {
            public float delayHours;
            public string title;
            public string description;
            public EchoEffectEntry[] effects;
        }

        [Serializable]
        private class EchoChoiceEntry
        {
            public string choiceId;
            public string text;
            public float moraleDelta;
            public EchoEffectEntry[] effects;
            public EchoDelayedConsequenceEntry delayedConsequence;
        }

        [Serializable]
        private class EchoConditionEntry
        {
            public int MinDay;
            public string RequiredFlagId;
        }

        [Serializable]
        private class EchoEntry
        {
            public string id;
            public string title;
            public string bodyText;
            public float weight;
            public int minDay;
            public EchoConditionEntry conditions;
            public EchoChoiceEntry[] choices;
        }

        [Serializable]
        private class EchoContainer
        {
            public EchoEntry[] entries;
        }

        /// <summary>Materialise every echo in echoes.json as a GameEvent (file order).</summary>
        public static List<GameEvent> Load()
        {
            var result = new List<GameEvent>();
            string path = Path.Combine(Application.streamingAssetsPath, "Data/echoes.json");
            if (!File.Exists(path)) return result;

            string json = File.ReadAllText(path);
            var wrapped = "{\"entries\":" + json + "}";
            var container = JsonUtility.FromJson<EchoContainer>(wrapped);
            if (container?.entries == null) return result;

            for (int i = 0; i < container.entries.Length; i++)
            {
                var e = container.entries[i];
                if (e == null || string.IsNullOrEmpty(e.id)) continue;

                var ev = ScriptableObject.CreateInstance<GameEvent>();
                ev.id = e.id;
                ev.title = e.title;
                ev.bodyText = e.bodyText;
                ev.weight = e.weight > 0f ? e.weight : 1f;
                ev.conditions = new EventConditions
                {
                    MinDay = e.conditions != null && e.conditions.MinDay > 0
                        ? e.conditions.MinDay
                        : (e.minDay > 0 ? e.minDay : 1),
                    RequiredFlagId = e.conditions != null ? e.conditions.RequiredFlagId : null
                };

                if (e.choices != null)
                {
                    for (int c = 0; c < e.choices.Length; c++)
                    {
                        var ce = e.choices[c];
                        if (ce == null) continue;

                        var choice = new EventChoice
                        {
                            ChoiceId = ce.choiceId,
                            Text = ce.text,
                            MoraleDelta = ce.moraleDelta,
                            Effects = new List<EventEffect>()
                        };

                        if (ce.effects != null)
                        {
                            for (int f = 0; f < ce.effects.Length; f++)
                            {
                                var ef = ce.effects[f];
                                if (ef == null) continue;
                                choice.Effects.Add(new EventEffect
                                {
                                    TargetNeed = ef.targetNeed,
                                    NeedDelta = ef.needDelta,
                                    ItemId = ef.itemId,
                                    ItemAmount = ef.itemAmount,
                                    SetWorldFlag = ef.setWorldFlag,
                                    WorldFlagValue = ef.worldFlagValue
                                });
                            }
                        }

                        if (ce.delayedConsequence != null)
                        {
                            var dc = new DelayedConsequence
                            {
                                DelayHours = ce.delayedConsequence.delayHours,
                                Title = ce.delayedConsequence.title,
                                Description = ce.delayedConsequence.description,
                                Effects = new List<EventEffect>()
                            };
                            if (ce.delayedConsequence.effects != null)
                            {
                                for (int f = 0; f < ce.delayedConsequence.effects.Length; f++)
                                {
                                    var ef = ce.delayedConsequence.effects[f];
                                    if (ef == null) continue;
                                    dc.Effects.Add(new EventEffect
                                    {
                                        TargetNeed = ef.targetNeed,
                                        NeedDelta = ef.needDelta,
                                        ItemId = ef.itemId,
                                        ItemAmount = ef.itemAmount,
                                        SetWorldFlag = ef.setWorldFlag,
                                        WorldFlagValue = ef.worldFlagValue
                                    });
                                }
                            }
                            choice.DelayedConsequence = dc;
                        }

                        ev.choices.Add(choice);
                    }
                }

                result.Add(ev);
            }
            return result;
        }
    }
}
