using System;
using System.Collections.Generic;
using Ashfall.Core.YearOfAsh;

using Ashfall.Core.IO;
namespace Ashfall.Core.Verdict
{
    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — quest registration into the live
    /// QuestlineSystem. `verdict_questlines.json` is authored directly in the
    /// runtime QuestlineDefinition schema (stageId-DAG-with-choices) so the
    /// quests are playable end-to-end, unlike the legacy flat stage catalog.
    /// Loads via the existing YearOfAshCatalogLoader's third fallback (a raw
    /// List&lt;QuestlineDefinition&gt;) — no parallel quest evaluator.
    /// </summary>
    public static class VerdictQuestCatalogLoader
    {
        public const string FileName = "verdict_questlines.json";

        public static int LoadAndRegister(
            QuestlineSystem system,
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer json)
        {
            if (system == null || fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return 0;

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
                return 0;

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return 0;

            try
            {
                var container = json.Deserialize<YearOfAshQuestContainer>(raw);
                var quests = container?.quests;
                if (quests == null || quests.Count == 0)
                {
                    quests = json.Deserialize<List<QuestlineDefinition>>(raw);
                }
                if (quests == null) return 0;
                int count = 0;
                foreach (var def in quests)
                {
                    if (def == null || string.IsNullOrEmpty(def.questlineId)) continue;
                    system.RegisterQuestline(def);
                    count++;
                }
                return count;
            }
            catch (Exception ex_CATDIAG)
                                {
                                    CatalogDiagnostics.Warn("<unknown>", "unknown", ex_CATDIAG);
                                    return 0;
                                }
        }
    }
}
