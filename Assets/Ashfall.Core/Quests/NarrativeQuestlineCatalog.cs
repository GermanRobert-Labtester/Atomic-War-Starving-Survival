// SPDX-License-Identifier: MIT
// ASHFALL Core: survivor narrative questline catalog (Plan 104 data authority).

using System.Collections.Generic;
#pragma warning disable CS0649
#pragma warning disable CS8618

namespace Ashfall.Core.Quests
{
    /// <summary>
    /// One binary crisis branch (Plan 104 stage 2). Choosing a branch grants
    /// <see cref="traitGranted"/> to the arc's survivor and applies
    /// <see cref="moraleDelta"/> to shelter morale. The two branches of a
    /// questline are mutually exclusive and never both granted.
    /// </summary>
    public class NarrativeQuestlineBranchDef
    {
        public string id = string.Empty;
        public string label = string.Empty;
        public string description = string.Empty;
        public string traitGranted = string.Empty;
        public int moraleDelta;
    }

    /// <summary>
    /// One stage of a survivor arc. Stages 0-1 carry <see cref="objectiveItems"/>
    /// (delivered to advance); the crisis stage carries
    /// <see cref="branchA"/>/<see cref="branchB"/> instead; the resolution stage
    /// carries neither and is epilogue text.
    /// </summary>
    public class NarrativeQuestlineStageDef
    {
        public int stage;
        public string name = string.Empty;
        public string description = string.Empty;
        public List<string> objectiveItems = new List<string>();
        public NarrativeQuestlineBranchDef? branchA;
        public NarrativeQuestlineBranchDef? branchB;

        /// <summary>True only when both halves of the binary crisis fork are present.</summary>
        public bool HasBranch => branchA != null && branchB != null;

        public NarrativeQuestlineBranchDef? FindBranch(string branchId)
        {
            if (string.IsNullOrEmpty(branchId)) return null;
            if (branchA != null && string.Equals(branchA.id, branchId, System.StringComparison.Ordinal)) return branchA;
            if (branchB != null && string.Equals(branchB.id, branchId, System.StringComparison.Ordinal)) return branchB;
            return null;
        }
    }

    /// <summary>
    /// One authored survivor arc: a named survivor, a target location, and a
    /// linear stage ladder terminating in a binary crisis fork.
    /// </summary>
    public class NarrativeQuestlineDef
    {
        public string questId = string.Empty;
        public string survivorId = string.Empty;
        public string title = string.Empty;
        public string targetLocationId = string.Empty;
        public List<NarrativeQuestlineStageDef> stages = new List<NarrativeQuestlineStageDef>();

        public NarrativeQuestlineStageDef? FindStage(int stageIndex)
        {
            for (int i = 0; i < stages.Count; i++)
                if (stages[i] != null && stages[i].stage == stageIndex) return stages[i];
            return null;
        }

        /// <summary>The crisis stage carrying the binary fork, or null when absent.</summary>
        public NarrativeQuestlineStageDef? FindBranchStage()
        {
            for (int i = 0; i < stages.Count; i++)
                if (stages[i] != null && stages[i].HasBranch) return stages[i];
            return null;
        }

        /// <summary>Highest authored stage index, or -1 for a stageless definition.</summary>
        public int FinalStageIndex
        {
            get
            {
                int max = -1;
                for (int i = 0; i < stages.Count; i++)
                    if (stages[i] != null && stages[i].stage > max) max = stages[i].stage;
                return max;
            }
        }
    }

    /// <summary>
    /// Engine-agnostic loader for narrative_questlines.json. Missing file → empty
    /// list; future schema → empty list (never partially parsed); malformed entry
    /// → skipped, never thrown. Duplicate quest ids: first definition wins, later
    /// duplicates are dropped so the catalog can never hold two arcs under one id.
    /// </summary>
    public static class NarrativeQuestlineCatalogLoader
    {
        public const string FileName = "narrative_questlines.json";
        public const int CurrentSchemaVersion = 1;

        public static List<NarrativeQuestlineDef> LoadEntries(
            string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<NarrativeQuestlineDef>();

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
                return new List<NarrativeQuestlineDef>();

            string raw = fileIO.ReadAllText(path);
            return Parse(raw, json, path);
        }

        /// <summary>Parse an already-read payload. Exposed for tests and headless demos.</summary>
        public static List<NarrativeQuestlineDef> Parse(string raw, IJsonSerializer json, string? pathForDiagnostics = null)
        {
            var result = new List<NarrativeQuestlineDef>();
            if (json == null || string.IsNullOrWhiteSpace(raw))
                return result;

            try
            {
                var root = json.Deserialize<QuestlineRoot>(raw);
                if (root == null) return result;
                if (root.schema_version > CurrentSchemaVersion) return result;
                if (root.questlines == null) return result;

                var seenQuest = new HashSet<string>(System.StringComparer.Ordinal);
                var seenSurvivor = new HashSet<string>(System.StringComparer.Ordinal);

                for (int i = 0; i < root.questlines.Count; i++)
                {
                    var e = root.questlines[i];
                    if (e == null) continue;
                    if (string.IsNullOrEmpty(e.quest_id) || string.IsNullOrEmpty(e.survivor_id)) continue;
                    if (e.stages == null || e.stages.Count == 0) continue;
                    if (!seenQuest.Add(e.quest_id)) continue;
                    if (!seenSurvivor.Add(e.survivor_id)) continue;

                    var def = new NarrativeQuestlineDef
                    {
                        questId = e.quest_id,
                        survivorId = e.survivor_id,
                        title = e.title ?? string.Empty,
                        targetLocationId = e.target_location_id ?? string.Empty
                    };

                    for (int s = 0; s < e.stages.Count; s++)
                    {
                        var st = e.stages[s];
                        if (st == null) continue;
                        var stage = new NarrativeQuestlineStageDef
                        {
                            stage = st.stage,
                            name = st.name ?? string.Empty,
                            description = st.description ?? string.Empty
                        };
                        if (st.objective_items != null)
                        {
                            for (int oi = 0; oi < st.objective_items.Count; oi++)
                            {
                                var item = st.objective_items[oi];
                                if (!string.IsNullOrEmpty(item) && !stage.objectiveItems.Contains(item))
                                    stage.objectiveItems.Add(item);
                            }
                        }
                        stage.branchA = MapBranch(st.branch_a);
                        stage.branchB = MapBranch(st.branch_b);
                        def.stages.Add(stage);
                    }

                    if (def.stages.Count == 0) continue;
                    result.Add(def);
                }
            }
            catch (System.Exception ex_CATDIAG)
            {
                Ashfall.Core.IO.CatalogDiagnostics.Warn(
                    pathForDiagnostics ?? FileName, "NarrativeQuestlineRoot", ex_CATDIAG);
                return result;
            }

            return result;
        }

        private static NarrativeQuestlineBranchDef? MapBranch(Branch? b)
        {
            if (b == null || string.IsNullOrEmpty(b.id)) return null;
            return new NarrativeQuestlineBranchDef
            {
                id = b.id,
                label = b.label ?? string.Empty,
                description = b.description ?? string.Empty,
                traitGranted = b.trait_granted ?? string.Empty,
                moraleDelta = b.morale_delta
            };
        }

        private class QuestlineRoot
        {
            public int schema_version = 1;
            public List<Questline> questlines = new List<Questline>();
        }

        private class Questline
        {
            public string quest_id;
            public string survivor_id;
            public string title;
            public string target_location_id;
            public List<Stage> stages;
        }

        private class Stage
        {
            public int stage;
            public string name;
            public string description;
            public List<string> objective_items;
            public Branch branch_a;
            public Branch branch_b;
        }

        private class Branch
        {
            public string id;
            public string label;
            public string description;
            public string trait_granted;
            public int morale_delta;
        }
    }
}
