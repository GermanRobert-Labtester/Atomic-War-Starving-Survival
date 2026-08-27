using System;
using System.Collections.Generic;
using Ashfall.Core.YearOfAsh;

using Ashfall.Core.IO;
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

    // ── Schema-versioned wrapper DTOs ──────────────────────────────────
    // The dose files are wrapped in {"schema_version": N, "locations"/"items": [...]}
    // so the loader can distinguish schema_version from bare-list fallback.

    [Serializable]
    internal sealed class DoseLocationsRoot
    {
#pragma warning disable CS0649 // schema_version is deserialized for contract compliance, not read in code
        public int schema_version;
#pragma warning restore CS0649
        public List<DoseLocationDef> locations = new List<DoseLocationDef>();
    }

    [Serializable]
    internal sealed class DoseItemsRoot
    {
#pragma warning disable CS0649
        public int schema_version;
#pragma warning restore CS0649
        public List<DoseItemDef> items = new List<DoseItemDef>();
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

            // Locations (wrapped: {"schema_version":1, "locations":[...]})
            string locPath = fileIO.Combine(dataDir, LocationsFile);
            if (fileIO.FileExists(locPath))
            {
                try
                {
                    var root = json.Deserialize<DoseLocationsRoot>(fileIO.ReadAllText(locPath));
                    if (root?.locations != null && root.locations.Count > 0)
                    {
                        catalog.locations.AddRange(root.locations);
                    }
                }
                catch (Exception ex_CATDIAG)
                {
                    CatalogDiagnostics.Warn(locPath, "DoseLocationsRoot", ex_CATDIAG);
                }
            }

            // Items (wrapped: {"schema_version":1, "items":[...]})
            string itemPath = fileIO.Combine(dataDir, ItemsFile);
            if (fileIO.FileExists(itemPath))
            {
                try
                {
                    var root = json.Deserialize<DoseItemsRoot>(fileIO.ReadAllText(itemPath));
                    if (root?.items != null && root.items.Count > 0)
                    {
                        catalog.items.AddRange(root.items);
                    }
                }
                catch (Exception ex_CATDIAG)
                {
                    CatalogDiagnostics.Warn(itemPath, "DoseItemsRoot", ex_CATDIAG);
                }
            }

            // Quests (authored to the live DAG DTO)
            string questPath = fileIO.Combine(dataDir, QuestsFile);
            if (fileIO.FileExists(questPath))
            {
                try
                {
                    var rows = CatalogLocator.LoadWrappedList<DoseQuestDef>(fileIO.ReadAllText(questPath), SystemTextJsonSerializer.Options);
                    if (rows != null)
                    {
                        foreach (var rq in rows)
                        {
                            var def = ToQuestlineDefinition(rq);
                            if (def != null) catalog.quests.Add(def);
                        }
                    }
                }
                catch (Exception ex_CATDIAG)
                {
                    CatalogDiagnostics.Warn(questPath, "DoseQuestDef list", ex_CATDIAG);
                }
            }

            return catalog;
        }

        public static QuestlineDefinition? ToQuestlineDefinition(DoseQuestDef rq)
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
