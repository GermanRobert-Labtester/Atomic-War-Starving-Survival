using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.YearOfAsh
{
    /// <summary>
    /// Plan 59 — loader for <c>dynamic_questlines.json</c> (the shelter campaign
    /// questline catalog). Parses the same <see cref="YearOfAshQuestContainer"/>
    /// choice-graph schema the runtime already consumes from
    /// <c>year_of_ash_questlines.json</c> and registers each questline into the
    /// supplied <see cref="QuestlineSystem"/> via its existing
    /// <c>RegisterQuestline</c> API.
    ///
    /// Justification (Plan 59 §72): the file existed with a schema no loader
    /// consumed; this adds typed reference resolution only — no new quest
    /// logic, stage interpreter, or reward engine.
    /// </summary>
    public static class DynamicQuestlineCatalogLoader
    {
        public const string FileName = "dynamic_questlines.json";

        public static int LoadAndRegister(
            QuestlineSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (system == null || fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return 0;

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
                return 0;

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return 0;

            List<QuestlineDefinition> quests;
            try
            {
                var container = json.Deserialize<YearOfAshQuestContainer>(raw);
                if (container == null || container.quests == null || container.quests.Count == 0)
                    return 0;
                quests = container.quests;
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn(path, "YearOfAshQuestContainer (dynamic questlines)", ex_CATDIAG);
                return 0;
            }

            int count = 0;
            foreach (var q in quests)
            {
                if (q == null || string.IsNullOrEmpty(q.questlineId)) continue;
                system.RegisterQuestline(q);
                count++;
            }
            return count;
        }
    }
}
