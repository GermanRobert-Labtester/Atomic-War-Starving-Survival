#nullable enable
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.Flags;
using Ashfall.Core.MoralChoice;
using Ashfall.Core.Verdict;
using Xunit;

namespace Ashfall.Core.Tests.Factions
{
    public sealed class Plan123_127RebelVerdictIntegrationTests
    {
        private static string ResolveDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static MoralChoiceSystem MakeMoralChoice(MoralPathBand band = MoralPathBand.Neutral)
        {
            var moral = new MoralChoiceSystem(new StubRng(1));
            int delta = band switch
            {
                MoralPathBand.VeryEvil => -120,
                MoralPathBand.Evil => -60,
                MoralPathBand.SlightlyEvil => -20,
                MoralPathBand.Neutral => 0,
                MoralPathBand.SlightlyPositive => 20,
                MoralPathBand.Positive => 60,
                MoralPathBand.VeryPositive => 120,
                _ => 0
            };
            if (delta != 0)
            {
                var quest = new MoralChoiceQuestDefinition
                {
                    Id = "quest_calib_rebel",
                    Choices = { new MoralChoiceOption { MoralDelta = delta, EmpathyDelta = 0 } }
                };
                moral.Resolve(quest, 0, "loc_test", 1);
            }
            return moral;
        }

        [Fact]
        public void RebelBranchCatalog_LoadsAll15BranchesFromAuthJson()
        {
            string dir = ResolveDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var catalog = RebelBranchCatalog.LoadAndRegister(dir, io, json);

            Assert.NotNull(catalog);
            Assert.Equal(15, catalog.Count);

            foreach (var branch in catalog)
            {
                Assert.False(string.IsNullOrWhiteSpace(branch.id));
                Assert.False(string.IsNullOrWhiteSpace(branch.display_name));
                Assert.False(string.IsNullOrWhiteSpace(branch.ponr_flag));
                Assert.NotEmpty(branch.endings);
            }
        }

        [Fact]
        public void ThreeWayFactionBranchIds_AreCompletelyDisjoint()
        {
            var rebelIds = RebelBranchIds.AllBranches.ToList();
            var militaryIds = MilitaryBranchIds.AllBranches.ToList();
            var independentIds = IndependentBranchIds.AllBranches.ToList();

            Assert.Equal(15, rebelIds.Count);
            Assert.Equal(15, militaryIds.Count);
            Assert.Equal(15, independentIds.Count);

            // Total unique branch count across all three factions must be exactly 45
            var all45 = rebelIds.Concat(militaryIds).Concat(independentIds).ToList();
            Assert.Equal(45, all45.Distinct(StringComparer.OrdinalIgnoreCase).Count());

            // Mutual pairwise disjointness
            Assert.Empty(rebelIds.Intersect(militaryIds, StringComparer.OrdinalIgnoreCase));
            Assert.Empty(rebelIds.Intersect(independentIds, StringComparer.OrdinalIgnoreCase));
            Assert.Empty(militaryIds.Intersect(independentIds, StringComparer.OrdinalIgnoreCase));
        }

        [Fact]
        public void ThreeWayPonrFlags_AreCompletelyDisjoint()
        {
            string dir = ResolveDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var rebelCatalog = RebelBranchCatalog.LoadAndRegister(dir, io, json);
            var militaryCatalog = MilitaryBranchCatalog.LoadAndRegister(dir, io, json);
            var independentCatalog = IndependentBranchCatalog.LoadAndRegister(dir, io, json);

            var rebelPonr = rebelCatalog.Select(b => b.ponr_flag).Where(f => !string.IsNullOrEmpty(f)).ToList();
            var militaryPonr = militaryCatalog.Select(b => b.ponr_flag).Where(f => !string.IsNullOrEmpty(f)).ToList();
            var independentPonr = independentCatalog.Select(b => b.ponr_flag).Where(f => !string.IsNullOrEmpty(f)).ToList();

            Assert.Equal(15, rebelPonr.Count);
            Assert.Equal(15, militaryPonr.Count);
            Assert.Equal(15, independentPonr.Count);

            var all45Flags = rebelPonr.Concat(militaryPonr).Concat(independentPonr).ToList();
            Assert.Equal(45, all45Flags.Distinct(StringComparer.OrdinalIgnoreCase).Count());
        }

        [Fact]
        public void RebelSystem_And_VerdictCorpusLadder_ExecuteConcurrentlyWithoutInterference()
        {
            string dir = ResolveDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var rebelCatalog = RebelBranchCatalog.LoadAndRegister(dir, io, json);
            var corpus = VerdictCatalogLoader.LoadCorruptionCorpus(dir, io, json);
            var ladder = VerdictCatalogLoader.LoadWorldHistoryLadder(dir, io, json);

            Assert.Equal(15, rebelCatalog.Count);
            Assert.Equal(25, corpus.Count);
            Assert.Equal(12, ladder.Count);

            var flags = new InMemoryFlagLedger();
            var rebelSystem = new RebelBranchSystem(rebelCatalog, flags);
            var machineLog = new MachineLogSystem();
            var moral = MakeMoralChoice(MoralPathBand.Neutral);

            // Commit a rebel branch (branch_rebel_1_true_rebel: neutral band)
            string committedRebel = rebelSystem.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);
            Assert.Equal(RebelBranchIds.BranchTrueRebel, committedRebel);
            Assert.Equal(RebelBranchIds.BranchTrueRebel, rebelSystem.CommittedBranchId);

            // Machine log inserts a corruption marker from the expanded 25-line corpus
            bool logResult = machineLog.InsertCorruptionMarker(day: 15, rng: new StubRng(3), corpus: corpus);
            Assert.True(logResult);

            // Verify Rebel state is preserved
            Assert.Equal(RebelBranchIds.BranchTrueRebel, rebelSystem.CommittedBranchId);

            // Roundtrip Rebel state through codec
            var rebelSave = RebelBranchSaveCodec.Capture(rebelSystem);
            string rebelJson = RebelBranchSaveCodec.Encode(rebelSave, json);
            var restoredSave = RebelBranchSaveCodec.Decode(rebelJson, json);
            var restoredSystem = new RebelBranchSystem(rebelCatalog, flags);
            RebelBranchSaveCodec.Restore(restoredSave, restoredSystem);
            Assert.Equal(RebelBranchIds.BranchTrueRebel, restoredSystem.CommittedBranchId);

            // Machine log entries intact
            Assert.Single(machineLog.Entries);
            Assert.Contains(machineLog.Entries[0].bodyShort, corpus);
        }

        [Fact]
        public void Coordinator_CoordinatesRebelBranchesWithExclusivity()
        {
            string dir = ResolveDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var rebelCatalog = RebelBranchCatalog.LoadAndRegister(dir, io, json);
            var militaryCatalog = MilitaryBranchCatalog.LoadAndRegister(dir, io, json);
            var independentCatalog = IndependentBranchCatalog.LoadAndRegister(dir, io, json);
            var flags = new InMemoryFlagLedger();

            var coordinator = new FactionBranchCoordinator(
                militaryCatalog: militaryCatalog,
                rebelCatalog: rebelCatalog,
                independentCatalog: independentCatalog,
                flags: flags);

            var moral = MakeMoralChoice(MoralPathBand.Neutral);
            var options = coordinator.GetBranchOptions(moral);

            // Total 45 options across the 3 factions!
            Assert.Equal(45, options.Count);
            Assert.Equal(15, options.Count(o => o.FactionKind == FactionBranchKind.Rebel));
            Assert.Equal(15, options.Count(o => o.FactionKind == FactionBranchKind.Military));
            Assert.Equal(15, options.Count(o => o.FactionKind == FactionBranchKind.Independent));

            // Commit a rebel branch
            var result = coordinator.CommitBranch(RebelBranchIds.BranchTrueRebel, moral);
            Assert.True(result.IsSuccess);
            Assert.True(coordinator.IsCommitted);
            Assert.Equal(FactionBranchKind.Rebel, coordinator.ActiveFactionKind);
            Assert.Equal(RebelBranchIds.BranchTrueRebel, coordinator.ActiveBranchId);

            // Exclusivity: attempting to commit military or independent must be blocked
            var tryMil = coordinator.CommitBranch(MilitaryBranchIds.BranchOpportunist, moral);
            Assert.False(tryMil.IsSuccess);
            Assert.Equal(ActionResult.StatusKind.Blocked, tryMil.Status);

            var tryIndep = coordinator.CommitBranch(IndependentBranchIds.BranchSurvivor, moral);
            Assert.False(tryIndep.IsSuccess);
            Assert.Equal(ActionResult.StatusKind.Blocked, tryIndep.Status);
        }
    }
}
