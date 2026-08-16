using System;
using System.Collections.Generic;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core
{
    /// <summary>One Dose location row (dose_locations.json).</summary>
    [Serializable]
    public class DoseLocationDef
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public string sector = string.Empty;
        public int riskLevel;
        public float radiationUsv;
        public string description = string.Empty;
    }

    /// <summary>One Dose item row (dose_items.json).</summary>
    [Serializable]
    public class DoseItemDef
    {
        public string id = string.Empty;
        public string name = string.Empty;
        public float weightKg;
        public float tradeValue;
        public string category = string.Empty;
        public string description = string.Empty;
    }

    /// <summary>One Dose quest line (dose_quests.json), authored to the live
    /// QuestlineDefinition graph DTO (stages + choices).</summary>
    [Serializable]
    public class DoseQuestDef
    {
        public string questlineId = string.Empty;
        public string title = string.Empty;
        public string synopsis = string.Empty;
        public string factionTag = string.Empty;
        public int minDay = 40;
        public int maxDay = 360;
        public List<DoseQuestStage> stages = new List<DoseQuestStage>();
    }

    [Serializable]
    public class DoseQuestStage
    {
        public string stageId = string.Empty;
        public string title = string.Empty;
        public string narrativePrompt = string.Empty;
        public bool isTerminal;
        public List<DoseQuestChoice> choices = new List<DoseQuestChoice>();
    }

    [Serializable]
    public class DoseQuestChoice
    {
        public string choiceId = string.Empty;
        public string text = string.Empty;
        public string nextStageId = string.Empty;
        public int moraleDelta;
        public int guiltDelta;
        public string grantItemId = string.Empty;
        public int grantItemQuantity;
        public string outcomeNarrative = string.Empty;
    }

    /// <summary>The Expansion 07 content bundle — locations, items and quest
    /// lines the four dose registers were written to serve (plan §IV/VI/VII).
    /// Display reads through Registers; gameplay reads through the systems.
    /// Engine-agnostic.</summary>
    public class DoseContentCatalog
    {
        public List<DoseLocationDef> locations = new List<DoseLocationDef>();
        public List<DoseItemDef> items = new List<DoseItemDef>();
        public List<QuestlineDefinition> quests = new List<QuestlineDefinition>();
    }

    /// <summary>Engine-agnostic loader for the dose_content bundle (three files).</summary>
    public static class DoseContentCatalogLoader
    {
        public const string LocationsFile = "dose_locations.json";
        public const string ItemsFile = "dose_items.json";
        public const string QuestsFile = "dose_quests.json";

        public static DoseContentCatalog Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var catalog = new DoseContentCatalog();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return catalog;

            // Locations
            string locPath = fileIO.Combine(dataDir, LocationsFile);
            if (fileIO.FileExists(locPath))
            {
                try
                {
                    var rows = json.Deserialize<List<DoseLocationDef>>(fileIO.ReadAllText(locPath));
                    if (rows != null) catalog.locations.AddRange(rows);
                }
                catch { /* tolerate a malformed editorial file; integrity gate flags it */ }
            }

            // Items
            string itemPath = fileIO.Combine(dataDir, ItemsFile);
            if (fileIO.FileExists(itemPath))
            {
                try
                {
                    var rows = json.Deserialize<List<DoseItemDef>>(fileIO.ReadAllText(itemPath));
                    if (rows != null) catalog.items.AddRange(rows);
                }
                catch { }
            }

            // Quests (authored to the live DAG DTO)
            string questPath = fileIO.Combine(dataDir, QuestsFile);
            if (fileIO.FileExists(questPath))
            {
                try
                {
                    var rows = json.Deserialize<List<DoseQuestDef>>(fileIO.ReadAllText(questPath));
                    if (rows != null)
                    {
                        foreach (var rq in rows)
                        {
                            var def = ToQuestlineDefinition(rq);
                            if (def != null) catalog.quests.Add(def);
                        }
                    }
                }
                catch { }
            }

            return catalog;
        }

        public static QuestlineDefinition ToQuestlineDefinition(DoseQuestDef rq)
        {
            if (rq == null || string.IsNullOrEmpty(rq.questlineId)) return null;
            var def = new QuestlineDefinition
            {
                questlineId = rq.questlineId,
                title = rq.title,
                synopsis = rq.synopsis,
                factionTag = rq.factionTag,
                minDay = rq.minDay,
                maxDay = rq.maxDay
            };
            foreach (var rs in rq.stages)
            {
                if (rs == null || string.IsNullOrEmpty(rs.stageId)) continue;
                var stage = new QuestStage
                {
                    stageId = rs.stageId,
                    title = rs.title,
                    narrativePrompt = rs.narrativePrompt,
                    isTerminal = rs.isTerminal
                };
                if (rs.choices != null)
                {
                    foreach (var rc in rs.choices)
                    {
                        if (rc == null || string.IsNullOrEmpty(rc.choiceId)) continue;
                        stage.choices.Add(new QuestChoice
                        {
                            choiceId = rc.choiceId,
                            text = rc.text,
                            nextStageId = rc.nextStageId,
                            moraleDelta = rc.moraleDelta,
                            guiltDelta = rc.guiltDelta,
                            grantItemId = rc.grantItemId,
                            grantItemQuantity = rc.grantItemQuantity,
                            outcomeNarrative = rc.outcomeNarrative
                        });
                    }
                }
                def.stages.Add(stage);
                if (string.IsNullOrEmpty(def.firstStageId)) def.firstStageId = rs.stageId;
            }
            return def.stages.Count > 0 ? def : null;
        }
    }
}
