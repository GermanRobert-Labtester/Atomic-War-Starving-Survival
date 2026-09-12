// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Quests;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 104 runtime wiring — contract tests for the survivor narrative
    /// questline catalog and progression system.
    ///
    /// Covers: authoritative catalog shape (12 arcs, uniform four-stage machine),
    /// loader hardening (missing / malformed / future schema / duplicate ids),
    /// progression rules (one arc per survivor, owed-item-only delivery, single
    /// irreversible crisis fork), trait and morale reporting, save round-trip,
    /// orphan-arc drop on restore, and determinism via SaveChecksum.
    /// </summary>
    public sealed class NarrativeQuestlineSystemTests
    {
        private static string? FindDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dir)) return dir;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir)) return dir;
            return null;
        }

        private static List<NarrativeQuestlineDef> LoadAuthoritative()
        {
            string? dir = FindDataDir();
            Assert.False(dir == null, "StreamingAssets/Data directory not found");
            return NarrativeQuestlineCatalogLoader.LoadEntries(dir!, new FileSystemIO(), new SystemTextJsonSerializer());
        }

        private static NarrativeQuestlineDef SyntheticDef(
            string questId = "quest_test_arc",
            string survivorId = "survivor_test",
            string item0 = "item_a",
            string item1a = "item_b",
            string item1b = "item_c")
        {
            return new NarrativeQuestlineDef
            {
                questId = questId,
                survivorId = survivorId,
                title = "Test Arc",
                targetLocationId = "loc_test",
                stages = new List<NarrativeQuestlineStageDef>
                {
                    new NarrativeQuestlineStageDef
                    {
                        stage = 0, name = "Discovery", description = "Opening.",
                        objectiveItems = new List<string> { item0 }
                    },
                    new NarrativeQuestlineStageDef
                    {
                        stage = 1, name = "Investigation", description = "Middle.",
                        objectiveItems = new List<string> { item1a, item1b }
                    },
                    new NarrativeQuestlineStageDef
                    {
                        stage = 2, name = "Crisis", description = "Fork.",
                        branchA = new NarrativeQuestlineBranchDef
                        {
                            id = "branch_left", label = "Left", description = "Take the left.",
                            traitGranted = "trait_left", moraleDelta = 5
                        },
                        branchB = new NarrativeQuestlineBranchDef
                        {
                            id = "branch_right", label = "Right", description = "Take the right.",
                            traitGranted = "trait_right", moraleDelta = -15
                        }
                    },
                    new NarrativeQuestlineStageDef { stage = 3, name = "Resolution", description = "Ending." }
                }
            };
        }

        private static NarrativeQuestlineSystem SyntheticSystem()
            => new NarrativeQuestlineSystem(new[] { SyntheticDef() });

        // ── authoritative catalog ─────────────────────────────────────────────

        [Fact]
        public void Catalog_LoadsTwelveAuthoredArcs()
        {
            var defs = LoadAuthoritative();
            Assert.Equal(12, defs.Count);
        }

        [Fact]
        public void Catalog_ArcIdsAndSurvivorIdsAreUnique()
        {
            var defs = LoadAuthoritative();
            Assert.Equal(defs.Count, defs.Select(d => d.questId).Distinct(StringComparer.Ordinal).Count());
            Assert.Equal(defs.Count, defs.Select(d => d.survivorId).Distinct(StringComparer.Ordinal).Count());
        }

        [Fact]
        public void Catalog_EveryArcHasTheUniformFourStageMachine()
        {
            foreach (var def in LoadAuthoritative())
            {
                Assert.Equal(4, def.stages.Count);
                Assert.Equal(new[] { 0, 1, 2, 3 }, def.stages.Select(s => s.stage).ToArray());
                Assert.Equal("Discovery", def.stages[0].name);
                Assert.Equal("Investigation", def.stages[1].name);
                Assert.Equal("Crisis", def.stages[2].name);
                Assert.Equal("Resolution", def.stages[3].name);

                Assert.NotEmpty(def.stages[0].objectiveItems);
                Assert.NotEmpty(def.stages[1].objectiveItems);
                Assert.Empty(def.stages[2].objectiveItems);
                Assert.Empty(def.stages[3].objectiveItems);

                Assert.True(def.stages[2].HasBranch, $"{def.questId}: crisis stage lacks a binary fork");
                Assert.False(def.stages[0].HasBranch);
                Assert.False(def.stages[3].HasBranch);
            }
        }

        [Fact]
        public void Catalog_EveryBranchCarriesADistinctTraitAndLabel()
        {
            var traits = new List<string>();
            foreach (var def in LoadAuthoritative())
            {
                var crisis = def.stages[2];
                Assert.NotEqual(crisis.branchA!.id, crisis.branchB!.id);
                Assert.False(string.IsNullOrWhiteSpace(crisis.branchA.label));
                Assert.False(string.IsNullOrWhiteSpace(crisis.branchB.label));
                Assert.False(string.IsNullOrWhiteSpace(crisis.branchA.description));
                Assert.False(string.IsNullOrWhiteSpace(crisis.branchB.description));
                Assert.False(string.IsNullOrWhiteSpace(crisis.branchA.traitGranted),
                    $"{def.questId}: branch A grants no trait");
                Assert.False(string.IsNullOrWhiteSpace(crisis.branchB.traitGranted),
                    $"{def.questId}: branch B grants no trait");
                Assert.NotEqual(crisis.branchA.traitGranted, crisis.branchB.traitGranted);
                traits.Add(crisis.branchA.traitGranted);
                traits.Add(crisis.branchB.traitGranted);
            }
            Assert.Equal(24, traits.Count);
            Assert.Equal(24, traits.Distinct(StringComparer.Ordinal).Count());
        }

        [Fact]
        public void Catalog_BranchIdsAreGloballyUnique()
        {
            var ids = LoadAuthoritative()
                .SelectMany(d => d.stages)
                .Where(s => s.HasBranch)
                .SelectMany(s => new[] { s.branchA!.id, s.branchB!.id })
                .ToList();
            Assert.Equal(ids.Count, ids.Distinct(StringComparer.Ordinal).Count());
        }

        [Fact]
        public void Catalog_AllStagesHaveNonEmptyProse()
        {
            foreach (var def in LoadAuthoritative())
            {
                Assert.False(string.IsNullOrWhiteSpace(def.title), $"{def.questId}: empty title");
                foreach (var stage in def.stages)
                {
                    Assert.False(string.IsNullOrWhiteSpace(stage.name), $"{def.questId} stage {stage.stage}: empty name");
                    Assert.True(stage.description.Length >= 80,
                        $"{def.questId} stage {stage.stage}: description too thin ({stage.description.Length})");
                }
            }
        }

        // ── loader hardening ──────────────────────────────────────────────────

        [Fact]
        public void Loader_MissingDirectoryYieldsEmptyListWithoutThrowing()
        {
            var defs = NarrativeQuestlineCatalogLoader.LoadEntries(
                Path.Combine(Path.GetTempPath(), "ashfall_nql_absent_" + Guid.NewGuid().ToString("N")),
                new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Empty(defs);
        }

        [Fact]
        public void Loader_MalformedJsonYieldsEmptyListWithoutThrowing()
        {
            var defs = NarrativeQuestlineCatalogLoader.Parse(
                "{\"schema_version\":1,\"questlines\":[{\"quest_id\":", new SystemTextJsonSerializer());
            Assert.Empty(defs);
        }

        [Fact]
        public void Loader_FutureSchemaIsRefusedWholeNotPartially()
        {
            string raw = "{\"schema_version\":99,\"questlines\":[{" +
                         "\"quest_id\":\"quest_a\",\"survivor_id\":\"s_a\",\"title\":\"A\"," +
                         "\"target_location_id\":\"loc_a\",\"stages\":[{" +
                         "\"stage\":0,\"name\":\"Discovery\",\"description\":\"d\"," +
                         "\"objective_items\":[\"item_x\"]}]}]}";
            Assert.Empty(NarrativeQuestlineCatalogLoader.Parse(raw, new SystemTextJsonSerializer()));
        }

        [Fact]
        public void Loader_DropsIncompleteEntriesAndDuplicateIds()
        {
            string raw = "{\"schema_version\":1,\"questlines\":[" +
                         // no stages -> dropped
                         "{\"quest_id\":\"quest_a\",\"survivor_id\":\"s_a\",\"stages\":[]}," +
                         // no survivor -> dropped
                         "{\"quest_id\":\"quest_b\",\"stages\":[{\"stage\":0,\"name\":\"n\",\"description\":\"d\"}]}," +
                         // valid
                         "{\"quest_id\":\"quest_c\",\"survivor_id\":\"s_c\",\"stages\":[" +
                         "{\"stage\":0,\"name\":\"n\",\"description\":\"d\",\"objective_items\":[\"item_x\",\"item_x\"]}]}," +
                         // duplicate quest id -> dropped
                         "{\"quest_id\":\"quest_c\",\"survivor_id\":\"s_d\",\"stages\":[" +
                         "{\"stage\":0,\"name\":\"n\",\"description\":\"d\"}]}," +
                         // duplicate survivor id -> dropped
                         "{\"quest_id\":\"quest_e\",\"survivor_id\":\"s_c\",\"stages\":[" +
                         "{\"stage\":0,\"name\":\"n\",\"description\":\"d\"}]}]}";

            var defs = NarrativeQuestlineCatalogLoader.Parse(raw, new SystemTextJsonSerializer());
            Assert.Single(defs);
            Assert.Equal("quest_c", defs[0].questId);
            // duplicate objective items collapse to one
            Assert.Single(defs[0].stages[0].objectiveItems);
        }

        [Fact]
        public void Loader_BranchWithoutIdIsDropped()
        {
            string raw = "{\"schema_version\":1,\"questlines\":[{" +
                         "\"quest_id\":\"quest_a\",\"survivor_id\":\"s_a\",\"stages\":[" +
                         "{\"stage\":0,\"name\":\"n\",\"description\":\"d\",\"objective_items\":[\"i\"]}," +
                         "{\"stage\":1,\"name\":\"Crisis\",\"description\":\"d\"," +
                         "\"branch_a\":{\"label\":\"A\",\"trait_granted\":\"trait_a\"}," +
                         "\"branch_b\":{\"id\":\"b\",\"label\":\"B\",\"trait_granted\":\"trait_b\"}}]}]}";
            var defs = NarrativeQuestlineCatalogLoader.Parse(raw, new SystemTextJsonSerializer());
            Assert.Single(defs);
            Assert.False(defs[0].stages[1].HasBranch, "a branch with no id must not form a fork");
        }

        // ── progression rules ─────────────────────────────────────────────────

        [Fact]
        public void Begin_OpensOneArcPerSurvivorAndNeverTwice()
        {
            var sys = SyntheticSystem();
            Assert.True(sys.TryBegin("survivor_test", 10));
            Assert.False(sys.TryBegin("survivor_test", 11));

            var arc = sys.GetArc("survivor_test");
            Assert.NotNull(arc);
            Assert.Equal(NarrativeArcStatus.Active, arc!.status);
            Assert.Equal(0, arc.currentStage);
            Assert.Equal(10, arc.startedDay);
            Assert.Equal("quest_test_arc", arc.questId);
        }

        [Fact]
        public void Begin_UnknownSurvivorFails()
        {
            var sys = SyntheticSystem();
            Assert.False(sys.TryBegin("survivor_nobody", 1));
            Assert.Null(sys.GetArc("survivor_nobody"));
        }

        [Fact]
        public void Delivery_RefusesItemsTheStageDoesNotOwe()
        {
            var sys = SyntheticSystem();
            Assert.True(sys.TryBegin("survivor_test", 1));

            Assert.False(sys.TryDeliverItem("survivor_test", "item_not_owed", 1));
            var arc = sys.GetArc("survivor_test")!;
            Assert.Equal(0, arc.currentStage);
            Assert.Empty(arc.deliveredItems);
        }

        [Fact]
        public void Delivery_RefusesDuplicatesAndAdvancesOnlyWhenStageIsClear()
        {
            var sys = SyntheticSystem();
            Assert.True(sys.TryBegin("survivor_test", 1));

            Assert.True(sys.TryDeliverItem("survivor_test", "item_a", 2));
            Assert.Equal(1, sys.GetArc("survivor_test")!.currentStage);

            // stage 1 owes two items; one delivery must not advance it
            Assert.True(sys.TryDeliverItem("survivor_test", "item_b", 3));
            Assert.Equal(1, sys.GetArc("survivor_test")!.currentStage);
            Assert.Equal(NarrativeArcStatus.Active, sys.GetArc("survivor_test")!.status);

            // a repeat of an already-delivered item is refused, not double-counted
            Assert.False(sys.TryDeliverItem("survivor_test", "item_b", 3));

            Assert.True(sys.TryDeliverItem("survivor_test", "item_c", 4));
            var arc = sys.GetArc("survivor_test")!;
            Assert.Equal(2, arc.currentStage);
            Assert.Equal(NarrativeArcStatus.AwaitingBranch, arc.status);
            Assert.True(sys.IsAwaitingBranch("survivor_test"));
        }

        [Fact]
        public void OutstandingObjectives_TrackTheCurrentStageOnly()
        {
            var sys = SyntheticSystem();
            Assert.True(sys.TryBegin("survivor_test", 1));
            Assert.Equal(new[] { "item_a" }, sys.GetOutstandingObjectives("survivor_test"));

            Assert.True(sys.TryDeliverItem("survivor_test", "item_a", 1));
            Assert.Equal(new[] { "item_b", "item_c" }, sys.GetOutstandingObjectives("survivor_test"));

            Assert.True(sys.TryDeliverItem("survivor_test", "item_b", 1));
            Assert.Equal(new[] { "item_c" }, sys.GetOutstandingObjectives("survivor_test"));

            // once the fork is reached nothing is owed any more
            Assert.True(sys.TryDeliverItem("survivor_test", "item_c", 1));
            Assert.Empty(sys.GetOutstandingObjectives("survivor_test"));
        }

        [Fact]
        public void Branch_RefusedBeforeTheCrisisAndForForeignIds()
        {
            var sys = SyntheticSystem();
            Assert.True(sys.TryBegin("survivor_test", 1));

            Assert.False(sys.TryChooseBranch("survivor_test", "branch_left", 1, out var early));
            Assert.Null(early);

            Assert.True(sys.TryDeliverItem("survivor_test", "item_a", 1));
            Assert.True(sys.TryDeliverItem("survivor_test", "item_b", 1));
            Assert.True(sys.TryDeliverItem("survivor_test", "item_c", 1));

            Assert.False(sys.TryChooseBranch("survivor_test", "branch_does_not_exist", 2, out var foreign));
            Assert.Null(foreign);
            Assert.Equal(NarrativeArcStatus.AwaitingBranch, sys.GetArc("survivor_test")!.status);
        }

        [Fact]
        public void Branch_RecordsTraitAndMoraleThenClosesTheArc()
        {
            var sys = SyntheticSystem();
            Assert.True(sys.TryBegin("survivor_test", 1));
            Assert.True(sys.TryDeliverItem("survivor_test", "item_a", 1));
            Assert.True(sys.TryDeliverItem("survivor_test", "item_b", 1));
            Assert.True(sys.TryDeliverItem("survivor_test", "item_c", 1));

            Assert.True(sys.TryChooseBranch("survivor_test", "branch_right", 7, out var branch));
            Assert.NotNull(branch);
            Assert.Equal("branch_right", branch!.id);
            Assert.Equal(-15, branch.moraleDelta);
            Assert.Equal("trait_right", branch.traitGranted);

            var arc = sys.GetArc("survivor_test")!;
            Assert.Equal(NarrativeArcStatus.Resolved, arc.status);
            Assert.Equal("branch_right", arc.chosenBranchId);
            Assert.Equal("trait_right", arc.grantedTraitId);
            Assert.Equal(3, arc.currentStage);
            Assert.Equal(7, arc.resolvedDay);
        }

        [Fact]
        public void Branch_CanNeverBeChosenTwice()
        {
            var sys = SyntheticSystem();
            Assert.True(sys.TryBegin("survivor_test", 1));
            Assert.True(sys.TryDeliverItem("survivor_test", "item_a", 1));
            Assert.True(sys.TryDeliverItem("survivor_test", "item_b", 1));
            Assert.True(sys.TryDeliverItem("survivor_test", "item_c", 1));
            Assert.True(sys.TryChooseBranch("survivor_test", "branch_left", 5, out _));

            Assert.False(sys.TryChooseBranch("survivor_test", "branch_right", 6, out var second));
            Assert.Null(second);
            var arc = sys.GetArc("survivor_test")!;
            Assert.Equal("branch_left", arc.chosenBranchId);
            Assert.Equal("trait_left", arc.grantedTraitId);
        }

        [Fact]
        public void Commands_RefuseUnknownSurvivors()
        {
            var sys = SyntheticSystem();
            Assert.False(sys.TryDeliverItem("nobody", "item_a", 1));
            Assert.False(sys.TryChooseBranch("nobody", "branch_left", 1, out _));
            Assert.Empty(sys.GetOutstandingObjectives("nobody"));
            Assert.False(sys.IsAwaitingBranch("nobody"));
        }

        [Fact]
        public void Events_FireOncePerTransition()
        {
            var sys = SyntheticSystem();
            int started = 0, advanced = 0, chosen = 0, resolved = 0;
            sys.OnArcStarted += _ => started++;
            sys.OnStageAdvanced += (_, _) => advanced++;
            sys.OnBranchChosen += (_, _) => chosen++;
            sys.OnArcResolved += _ => resolved++;

            Assert.True(sys.TryBegin("survivor_test", 1));
            Assert.True(sys.TryDeliverItem("survivor_test", "item_a", 1));
            Assert.True(sys.TryDeliverItem("survivor_test", "item_b", 1));
            Assert.True(sys.TryDeliverItem("survivor_test", "item_c", 1));
            Assert.True(sys.TryChooseBranch("survivor_test", "branch_left", 2, out _));

            Assert.Equal(1, started);
            Assert.Equal(3, advanced);
            Assert.Equal(1, chosen);
            Assert.Equal(1, resolved);
        }

        [Fact]
        public void ArcsAreIndependentPerSurvivor()
        {
            var sys = new NarrativeQuestlineSystem(new[]
            {
                SyntheticDef("quest_one", "survivor_one", "item_1"),
                SyntheticDef("quest_two", "survivor_two", "item_2")
            });

            Assert.True(sys.TryBegin("survivor_one", 1));
            Assert.True(sys.TryDeliverItem("survivor_one", "item_1", 1));
            Assert.Equal(1, sys.GetArc("survivor_one")!.currentStage);
            Assert.Null(sys.GetArc("survivor_two"));

            // survivor_two's stage 0 owes item_2, not item_1
            Assert.True(sys.TryBegin("survivor_two", 1));
            Assert.False(sys.TryDeliverItem("survivor_two", "item_1", 1));
            Assert.True(sys.TryDeliverItem("survivor_two", "item_2", 1));
            Assert.Equal(2, sys.Arcs.Count);
        }

        // ── persistence ───────────────────────────────────────────────────────

        [Fact]
        public void SaveRoundTrip_PreservesArcProgressExactly()
        {
            var sys = SyntheticSystem();
            Assert.True(sys.TryBegin("survivor_test", 3));
            Assert.True(sys.TryDeliverItem("survivor_test", "item_a", 4));
            Assert.True(sys.TryDeliverItem("survivor_test", "item_b", 5));

            var captured = sys.CaptureState();
            var restored = SyntheticSystem();
            restored.RestoreState(captured);

            var arc = restored.GetArc("survivor_test");
            Assert.NotNull(arc);
            Assert.Equal(1, arc!.currentStage);
            Assert.Equal(NarrativeArcStatus.Active, arc.status);
            Assert.Equal(new[] { "item_a", "item_b" }, arc.deliveredItems);
            Assert.Equal(3, arc.startedDay);
            Assert.Equal(new[] { "item_c" }, restored.GetOutstandingObjectives("survivor_test"));

            // the restored system keeps playing from the same position
            Assert.True(restored.TryDeliverItem("survivor_test", "item_c", 6));
            Assert.True(restored.IsAwaitingBranch("survivor_test"));
            Assert.True(restored.TryChooseBranch("survivor_test", "branch_left", 7, out var branch));
            Assert.Equal("trait_left", branch!.traitGranted);
        }

        [Fact]
        public void SaveRoundTrip_PreservesResolvedArcs()
        {
            var sys = SyntheticSystem();
            Assert.True(sys.TryBegin("survivor_test", 1));
            Assert.True(sys.TryDeliverItem("survivor_test", "item_a", 1));
            Assert.True(sys.TryDeliverItem("survivor_test", "item_b", 1));
            Assert.True(sys.TryDeliverItem("survivor_test", "item_c", 1));
            Assert.True(sys.TryChooseBranch("survivor_test", "branch_right", 9, out _));

            var restored = SyntheticSystem();
            restored.RestoreState(sys.CaptureState());

            var arc = restored.GetArc("survivor_test")!;
            Assert.Equal(NarrativeArcStatus.Resolved, arc.status);
            Assert.Equal("branch_right", arc.chosenBranchId);
            Assert.Equal("trait_right", arc.grantedTraitId);
            Assert.Equal(9, arc.resolvedDay);

            // a resolved arc cannot be reopened or re-decided
            Assert.False(restored.TryBegin("survivor_test", 10));
            Assert.False(restored.TryDeliverItem("survivor_test", "item_a", 10));
            Assert.False(restored.TryChooseBranch("survivor_test", "branch_left", 10, out _));
        }

        [Fact]
        public void Restore_NullOrEmptyClearsArcsWithoutThrowing()
        {
            var sys = SyntheticSystem();
            Assert.True(sys.TryBegin("survivor_test", 1));
            Assert.Single(sys.Arcs);

            sys.RestoreState(null);
            Assert.Empty(sys.Arcs);
            Assert.Null(sys.GetArc("survivor_test"));

            Assert.True(sys.TryBegin("survivor_test", 2));
            sys.RestoreState(new NarrativeQuestlineSaveState());
            Assert.Empty(sys.Arcs);
        }

        [Fact]
        public void Restore_DropsArcsWhoseDefinitionNoLongerExists()
        {
            var state = new NarrativeQuestlineSaveState
            {
                arcs = new List<NarrativeQuestlineArcState>
                {
                    new NarrativeQuestlineArcState
                    {
                        questId = "quest_retired", survivorId = "survivor_old",
                        currentStage = 1, status = NarrativeArcStatus.Active, startedDay = 4
                    },
                    new NarrativeQuestlineArcState
                    {
                        questId = "quest_test_arc", survivorId = "survivor_test",
                        currentStage = 0, status = NarrativeArcStatus.Active, startedDay = 5
                    }
                }
            };

            var sys = SyntheticSystem();
            sys.RestoreState(state);

            Assert.Single(sys.Arcs);
            Assert.Null(sys.GetArc("survivor_old"));
            Assert.NotNull(sys.GetArc("survivor_test"));
        }

        [Fact]
        public void Restore_SuppressesEventsSoLoadingIsNotMistakenForPlay()
        {
            var sys = SyntheticSystem();
            Assert.True(sys.TryBegin("survivor_test", 1));
            var captured = sys.CaptureState();

            var restored = SyntheticSystem();
            int started = 0, advanced = 0, resolved = 0;
            restored.OnArcStarted += _ => started++;
            restored.OnStageAdvanced += (_, _) => advanced++;
            restored.OnArcResolved += _ => resolved++;

            restored.RestoreState(captured);

            Assert.Equal(0, started);
            Assert.Equal(0, advanced);
            Assert.Equal(0, resolved);
            Assert.NotNull(restored.GetArc("survivor_test"));
        }

        [Fact]
        public void Checksum_CoversArcFieldsSoMutationIsActuallyDetected()
        {
            // Regression guard for the property-vs-field trap: SaveChecksum
            // canonicalizes BindingFlags.Public|Instance FIELDS only, so a
            // property-based save DTO hashes to an empty object and every state
            // looks identical — the envelope would certify corrupt saves as clean.
            var sys = SyntheticSystem();
            Assert.True(sys.TryBegin("survivor_test", 1));

            string canonical = SaveChecksum.Canonicalize(sys.CaptureState());
            Assert.Contains("questId", canonical);
            Assert.Contains("survivorId", canonical);
            Assert.Contains("survivor_test", canonical);
            Assert.Contains("deliveredItems", canonical);

            string baseline = SaveChecksum.Compute(sys.CaptureState());
            Assert.NotEqual(SaveChecksum.Compute(new NarrativeQuestlineSaveState()), baseline);

            Assert.True(sys.TryDeliverItem("survivor_test", "item_a", 2));
            Assert.NotEqual(baseline, SaveChecksum.Compute(sys.CaptureState()));
        }

        [Fact]
        public void Determinism_SameInputsProduceIdenticalChecksums()
        {
            static string Play()
            {
                var sys = SyntheticSystem();
                sys.TryBegin("survivor_test", 1);
                sys.TryDeliverItem("survivor_test", "item_a", 2);
                sys.TryDeliverItem("survivor_test", "item_b", 3);
                sys.TryDeliverItem("survivor_test", "item_c", 4);
                sys.TryChooseBranch("survivor_test", "branch_left", 5, out _);
                return SaveChecksum.Compute(sys.CaptureState());
            }

            string a = Play();
            string b = Play();
            Assert.False(string.IsNullOrEmpty(a));
            Assert.Equal(a, b);
        }

        [Fact]
        public void Determinism_DifferentBranchChoiceChangesTheChecksum()
        {
            var left = SyntheticSystem();
            left.TryBegin("survivor_test", 1);
            left.TryDeliverItem("survivor_test", "item_a", 1);
            left.TryDeliverItem("survivor_test", "item_b", 1);
            left.TryDeliverItem("survivor_test", "item_c", 1);
            left.TryChooseBranch("survivor_test", "branch_left", 2, out _);

            var right = SyntheticSystem();
            right.TryBegin("survivor_test", 1);
            right.TryDeliverItem("survivor_test", "item_a", 1);
            right.TryDeliverItem("survivor_test", "item_b", 1);
            right.TryDeliverItem("survivor_test", "item_c", 1);
            right.TryChooseBranch("survivor_test", "branch_right", 2, out _);

            Assert.NotEqual(
                SaveChecksum.Compute(left.CaptureState()),
                SaveChecksum.Compute(right.CaptureState()));
        }

        [Fact]
        public void Definitions_AreExposedSortedAndStable()
        {
            var sys = new NarrativeQuestlineSystem(new[]
            {
                SyntheticDef("quest_zulu", "survivor_z"),
                SyntheticDef("quest_alpha", "survivor_a"),
                SyntheticDef("quest_mike", "survivor_m")
            });

            Assert.Equal(3, sys.DefinitionCount);
            Assert.Equal(
                new[] { "quest_alpha", "quest_mike", "quest_zulu" },
                sys.Definitions.Select(d => d.questId).ToArray());
            Assert.Equal(sys.Definitions.Select(d => d.questId).ToArray(),
                         sys.Definitions.Select(d => d.questId).ToArray());
        }

        [Fact]
        public void NullAndEmptyDefinitions_ProduceAnEmptyButUsableSystem()
        {
            var sys = new NarrativeQuestlineSystem(null);
            Assert.Equal(0, sys.DefinitionCount);
            Assert.Empty(sys.Definitions);
            Assert.False(sys.TryBegin("anyone", 1));
            Assert.Null(sys.GetDefinitionForSurvivor("anyone"));
            Assert.Empty(sys.CaptureState().arcs);
        }

        [Fact]
        public void AuthoritativeCatalog_DrivesACompleteArcToResolution()
        {
            var defs = LoadAuthoritative();
            var sys = new NarrativeQuestlineSystem(defs);
            Assert.Equal(12, sys.DefinitionCount);

            // Play the first authored arc end to end through its real data.
            var def = defs.OrderBy(d => d.questId, StringComparer.Ordinal).First();
            Assert.True(sys.TryBegin(def.survivorId, 1));

            foreach (var stage in def.stages.Where(s => s.objectiveItems.Count > 0))
            {
                var arc = sys.GetArc(def.survivorId)!;
                Assert.Equal(stage.stage, arc.currentStage);
                foreach (var item in stage.objectiveItems)
                    Assert.True(sys.TryDeliverItem(def.survivorId, item, 2),
                        $"{def.questId}: stage {stage.stage} should accept {item}");
            }

            Assert.True(sys.IsAwaitingBranch(def.survivorId));
            var crisis = def.FindBranchStage()!;
            Assert.True(sys.TryChooseBranch(def.survivorId, crisis.branchB!.id, 3, out var branch));
            Assert.Equal(crisis.branchB.traitGranted, branch!.traitGranted);

            var resolved = sys.GetArc(def.survivorId)!;
            Assert.Equal(NarrativeArcStatus.Resolved, resolved.status);
            Assert.Equal(def.FinalStageIndex, resolved.currentStage);
        }
    }
}
