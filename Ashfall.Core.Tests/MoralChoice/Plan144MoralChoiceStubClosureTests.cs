// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.MoralChoice;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 144 — moral choice quest stub promotion, canonicalization and
    /// integrity closure.
    ///
    /// `moral_choice_quest_stubs.json` was an integrity shim: ten full quest
    /// definitions that NO runtime loader ever read, nine of which were
    /// divergent duplicates of canonical quests in
    /// moral_choice_quests_branching.json, plus one quest authored under the
    /// bare `merge_quest_prefix` token. These tests pin the closed state:
    ///   - the stub catalog is retired (no integrity-only placeholders);
    ///   - every chain entry/gate quest resolves to exactly one executable
    ///     definition in the branching catalog;
    ///   - the merge prefix is a validated grammar token, never a quest;
    ///   - the validator gates duplicate executable quest definitions
    ///     across files (the historical stub failure mode) while keeping
    ///     identity-only registry acknowledgement legal;
    ///   - saves round-trip resolved quest ids against canonical definitions
    ///     only, with no source-file dependence.
    /// </summary>
    public sealed class Plan144MoralChoiceStubClosureTests : CatalogTestBase
    {
        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();

        // ── Classification / retirement ──────────────────────────────────

        [Fact]
        public void StubCatalogIsRetired_NoIntegrityOnlyPlaceholderFileRemains()
        {
            string stubPath = Path.Combine(DataDirectory, "moral_choice_quest_stubs.json");
            Assert.False(File.Exists(stubPath),
                "moral_choice_quest_stubs.json must not exist: all ten rows were classified " +
                "(9 divergent duplicates of canonical branching quests + 1 prefix token) and " +
                "the prefix is now validated as a pattern, not fed a placeholder quest");
        }

        [Fact]
        public void AllTwelveBranchEntryQuestsResolveToExecutableBranchingDefinitions()
        {
            var chains = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
            var branching = MoralChoiceBranchQuestCatalogLoader.Load(DataDirectory, s_files, s_json);
            var byId = branching.ToDictionary(q => q.Id, StringComparer.Ordinal);

            Assert.Equal(4, chains.Branches.Count);
            foreach (var branch in chains.Branches)
            {
                Assert.Equal(3, branch.EntryQuests.Count);
                foreach (string entryId in branch.EntryQuests)
                {
                    Assert.Contains(entryId, byId.Keys);
                    var q = byId[entryId];
                    Assert.False(string.IsNullOrWhiteSpace(q.DisplayName),
                        entryId + " must have a display name");
                    Assert.True(q.Choices.Count >= 2,
                        entryId + " must be executable (choices), not a placeholder");
                }
            }
        }

        [Fact]
        public void CanonicalIronEntrySurvivesAsTheWeightOfFilters()
        {
            var branching = MoralChoiceBranchQuestCatalogLoader.Load(DataDirectory, s_files, s_json);
            var iron01 = branching.Single(q => q.Id == "quest_moral_chain_iron_01");
            Assert.Equal("The Weight of Filters", iron01.DisplayName);
            // The retired stub version was "The Triage Doctrine" — same id,
            // divergent content. The canonical runtime authorship wins.
            Assert.DoesNotContain(branching, q => q.DisplayName == "The Triage Doctrine");
        }

        [Fact]
        public void AllQuestGateIdsResolveInTheBranchingCatalog()
        {
            var chains = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
            var branching = MoralChoiceBranchQuestCatalogLoader.Load(DataDirectory, s_files, s_json);
            var byId = branching.Select(q => q.Id).ToHashSet(StringComparer.Ordinal);

            Assert.True(chains.QuestGates.Count >= 88, "4 branches × 22 gated quests expected");
            foreach (var gate in chains.QuestGates)
                Assert.Contains(gate.QuestId, byId);
        }

        // ── Merge-prefix contract (Workstream 144B) ─────────────────────

        [Fact]
        public void MergePrefixIsAGrammarToken_NoCataloguedQuestBearsTheBarePrefixId()
        {
            var chains = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
            Assert.Equal("quest_moral_merge_", chains.MergeRules.MergeQuestPrefix, StringComparer.Ordinal);

            var allQuests = MoralChoiceCatalogLoader.Load(DataDirectory, s_files, s_json)
                .Concat(MoralChoiceBranchQuestCatalogLoader.Load(DataDirectory, s_files, s_json))
                .Concat(MoralChoiceExpansionQuestCatalogLoader.Load(DataDirectory, s_files, s_json))
                .Concat(MoralChoiceCatalogLoader.LoadFrom(
                    DataDirectory, "moral_choice_quests_distress.json", s_files, s_json));

            foreach (var q in allQuests)
            {
                Assert.NotEqual(chains.MergeRules.MergeQuestPrefix, q.Id);
                Assert.False(q.Id.StartsWith(chains.MergeRules.MergeQuestPrefix, StringComparison.Ordinal),
                    "no merge quest exists yet; when one is authored it must be a full " +
                    "definition under a prefix-matching id, never the bare prefix: " + q.Id);
            }
        }

        [Fact]
        public void BarePrefixIdIsNotASelectableOrResolvableQuest()
        {
            var sys = new MoralChoiceSystem(new SeededRng(1441));
            sys.RegisterQuests(MoralChoiceBranchQuestCatalogLoader.Load(DataDirectory, s_files, s_json));

            Assert.Null(sys.GetQuest("quest_moral_merge_"));
            Assert.False(sys.TryResolve("quest_moral_merge_", 0, "loc_shelter_gate", 100,
                out _), "the prefix token must never resolve as a quest");
        }

        // ── Integrity validator contract (Workstreams 144B/144C/144D) ────

        [Fact]
        public void ShippedDataPassesIntegrityWithoutAnyStubDefinitions()
        {
            var report = CatalogIntegrityValidator.Validate(DataDirectory, new FileSystemIO());
            Assert.True(report.Clean,
                "shipped data must be clean with the stub file retired:\n"
                + string.Join("\n", report.Errors));
        }

        [Fact]
        public void PrefixPatternValueNeedsNoQuestDefinition()
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "chains.json"),
                    "{\"schema_version\":1," +
                    "\"branches\":[{\"id\":\"branch_test_road\",\"display_name\":\"Road\",\"description\":\"d\"," +
                    "\"lock_threshold\":3,\"entry_quests\":[\"quest_chain_alpha_01\"]}]," +
                    "\"merge_rules\":{\"merge_quest_prefix\":\"quest_moral_merge_\"," +
                    "\"merge_quests_require_min_progress\":5}}");
                File.WriteAllText(Path.Combine(scratch, "quests.json"),
                    "{\"schema_version\":1,\"quests\":[{\"id\":\"quest_chain_alpha_01\"," +
                    "\"display_name\":\"Alpha\",\"choices\":[{\"label\":\"a\",\"outcome_text\":\"x\"}]}]}");
            });

            Assert.True(report.Clean,
                "a prefix grammar token must not require a placeholder quest definition:\n"
                + string.Join("\n", report.Errors));
        }

        [Fact]
        public void MalformedPrefixPatternIsAnError()
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "chains.json"),
                    "{\"schema_version\":1," +
                    "\"merge_rules\":{\"merge_quest_prefix\":\"quest_moral_merge\"}}");
            });

            Assert.False(report.Clean, "a prefix without trailing underscore must be rejected");
            Assert.Contains(report.Errors, line =>
                line.Contains("prefix-pattern key") && line.Contains("quest_moral_merge"));
        }

        [Fact]
        public void DuplicateExecutableQuestDefinitionAcrossTwoFilesIsAnError()
        {
            // The exact historical failure mode: quest_moral_chain_iron_01 was a
            // full (divergent) definition in both moral_choice_quest_stubs.json
            // and moral_choice_quests_branching.json.
            string questA = "{\"schema_version\":1,\"quests\":[{\"id\":\"quest_chain_dup_01\"," +
                "\"display_name\":\"Stub Body\",\"choices\":[{\"label\":\"s\",\"outcome_text\":\"s\"}]}]}";
            string questB = "{\"schema_version\":1,\"quests\":[{\"id\":\"quest_chain_dup_01\"," +
                "\"display_name\":\"Canonical Body\",\"choices\":[{\"label\":\"c\",\"outcome_text\":\"c\"}]}]}";

            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "quests_one.json"), questA);
                File.WriteAllText(Path.Combine(scratch, "quests_two.json"), questB);
            });

            Assert.False(report.Clean, "two executable definitions for one quest id must fail");
            Assert.Contains(report.Errors, line =>
                line.Contains("duplicate executable quest definition 'quest_chain_dup_01'")
                && line.Contains("quests_one.json") && line.Contains("quests_two.json"));
        }

        [Fact]
        public void RegistryOnlyAcknowledgementBesideExecutableDefinitionIsClean()
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "quests_real.json"),
                    "{\"schema_version\":1,\"quests\":[{\"id\":\"quest_chain_reg_01\"," +
                    "\"display_name\":\"Real\",\"choices\":[{\"label\":\"a\",\"outcome_text\":\"x\"}]}]}");
                // questline_master.json-style identity-only registry row: an id
                // with no playable grammar acknowledges, never redefines.
                File.WriteAllText(Path.Combine(scratch, "questline_master.json"),
                    "{\"schema_version\":1,\"entries\":[{\"id\":\"quest_chain_reg_01\"}]}");
            });

            Assert.True(report.Clean,
                "identity-only registry acknowledgement next to the single executable " +
                "definition must stay legal:\n" + string.Join("\n", report.Errors));
        }

        [Fact]
        public void ChainEntryQuestWithoutDefinitionIsUnresolved()
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "chains.json"),
                    "{\"schema_version\":1," +
                    "\"branches\":[{\"id\":\"branch_test_road\",\"display_name\":\"Road\"," +
                    "\"description\":\"d\",\"lock_threshold\":3," +
                    "\"entry_quests\":[\"quest_missing_entry_01\"]}]}");
            });

            Assert.False(report.Clean, "a chain entry with no executable definition must fail");
            Assert.Contains(report.Errors, line =>
                line.Contains("unresolved id 'quest_missing_entry_01'")
                && line.Contains("chains.json"));
        }

        [Fact]
        public void PrefixMatchingMergeQuestIdWithoutDefinitionIsNeverAutoBlessed()
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "chains.json"),
                    "{\"schema_version\":1," +
                    "\"merge_rules\":{\"merge_quest_prefix\":\"quest_moral_merge_\"}," +
                    "\"branches\":[{\"id\":\"branch_test_road\",\"display_name\":\"Road\"," +
                    "\"description\":\"d\",\"lock_threshold\":3," +
                    "\"entry_quests\":[\"quest_moral_merge_phantom\"]}]}");
            });

            Assert.False(report.Clean,
                "a prefix-matching id must still resolve to a full definition; " +
                "matching the prefix grammar alone must not bless it");
            Assert.Contains(report.Errors, line =>
                line.Contains("unresolved id 'quest_moral_merge_phantom'"));
        }

        [Fact]
        public void UnresolvedBranchLockFlagIsReported()
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "chains.json"),
                    "{\"schema_version\":1," +
                    "\"branches\":[{\"id\":\"branch_test_road\",\"display_name\":\"Road\"," +
                    "\"description\":\"d\",\"lock_threshold\":3," +
                    "\"entry_quests\":[\"quest_chain_flag_01\"]," +
                    "\"locked_flag\":\"flag_branch_missing_locked\"}]}");
                File.WriteAllText(Path.Combine(scratch, "quests.json"),
                    "{\"schema_version\":1,\"quests\":[{\"id\":\"quest_chain_flag_01\"," +
                    "\"display_name\":\"F\",\"choices\":[{\"label\":\"a\",\"outcome_text\":\"x\"}]}]}");
            });

            Assert.False(report.Clean, "a branch lock flag with no definition must fail");
            Assert.Contains(report.Errors, line =>
                line.Contains("flag_branch_missing_locked"));
        }

        // ── Save compatibility (Workstream 144 §14) ──────────────────────

        [Fact]
        public void SaveRoundTripsResolvedBranchQuest_AgainstCanonicalDefinitionsOnly()
        {
            var chains = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
            var branching = MoralChoiceBranchQuestCatalogLoader.Load(DataDirectory, s_files, s_json);

            // System A resolves a canonical branch entry quest.
            var a = new MoralChoiceSystem(new SeededRng(1442));
            a.RegisterQuests(branching);
            a.InitializeChainData(chains);
            Assert.True(a.TryResolve("quest_moral_chain_iron_01", 0, "loc_shelter_gate", 1, out _));

            // Capture → serialize (the save pipeline) → deserialize → restore.
            var captured = a.CaptureState();
            var wire = s_json.Serialize(captured);
            var restoredDto = s_json.Deserialize<MoralChoiceState>(wire);
            Assert.NotNull(restoredDto);

            // System B has only canonical catalogs (no stub source exists at
            // all anymore) and restores the save by stable quest id.
            var b = new MoralChoiceSystem(new SeededRng(1442));
            b.RegisterQuests(branching);
            b.InitializeChainData(chains);
            b.RestoreState(restoredDto!);

            Assert.True(b.IsResolved("quest_moral_chain_iron_01"),
                "a save from before cleanup must resolve against the canonical definition");
            Assert.Equal(1, b.QuestsResolved);
            Assert.True(b.GetBranchProgress("branch_iron_way") >= 1,
                "branch progress must survive the round-trip");
            Assert.Equal("The Weight of Filters", b.GetQuest("quest_moral_chain_iron_01")!.DisplayName);
            Assert.DoesNotContain(b.State.resolutions,
                r => r.questId == "quest_moral_merge_");
        }

        [Fact]
        public void SaveDtosCarryStableQuestIdsOnly_NoSourceFileDependence()
        {
            // Structural pin: the resolution DTO has exactly one identity
            // field for the quest — its id. No field can record which
            // catalog file supplied a definition, so retiring the stub
            // file required no save migration.
            var resolutionFields = typeof(MoralChoiceResolution).GetFields();
            Assert.Contains(resolutionFields, f => f.Name == "questId");
            Assert.DoesNotContain(resolutionFields, f =>
                f.Name.Contains("file", StringComparison.OrdinalIgnoreCase)
                || f.Name.Contains("source", StringComparison.OrdinalIgnoreCase)
                || f.Name.Contains("catalog", StringComparison.OrdinalIgnoreCase));
        }

        // ── helpers ──────────────────────────────────────────────────────

        private static CatalogIntegrityReport ValidateScratch(Action<string> seed)
        {
            string scratch = Path.Combine(Path.GetTempPath(),
                "ashfall_plan144_" + Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(scratch);
                IntegrityScratchFixture.SeedMandatoryCatalogs(scratch);
                seed(scratch);
                return CatalogIntegrityValidator.Validate(scratch, new FileSystemIO());
            }
            finally
            {
                if (Directory.Exists(scratch))
                    Directory.Delete(scratch, true);
            }
        }
    }
}
