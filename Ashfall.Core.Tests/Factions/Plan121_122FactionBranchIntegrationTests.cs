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
using Xunit;

namespace Ashfall.Core.Tests.Factions
{
    public sealed class Plan121_122FactionBranchIntegrationTests
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
                    Id = "quest_calib",
                    Choices = { new MoralChoiceOption { MoralDelta = delta, EmpathyDelta = 0 } }
                };
                moral.Resolve(quest, 0, "loc_test", 1);
            }
            return moral;
        }

        [Fact]
        public void Catalogs_LoadSuccessfullyFromAuthJson()
        {
            string dir = ResolveDataDir();
            var serializer = new SystemTextJsonSerializer();
            var io = new FileSystemIO();

            var independentCatalog = IndependentBranchCatalog.LoadAndRegister(dir, io, serializer);
            var militaryCatalog = MilitaryBranchCatalog.LoadAndRegister(dir, io, serializer);

            Assert.NotNull(independentCatalog);
            Assert.NotNull(militaryCatalog);

            Assert.Equal(15, independentCatalog.Count);
            Assert.Equal(15, militaryCatalog.Count);
        }

        [Fact]
        public void BranchIds_AreDisjointAndUniqueAcrossCatalogs()
        {
            var independentIds = IndependentBranchIds.AllBranches.ToList();
            var militaryIds = MilitaryBranchIds.AllBranches.ToList();

            Assert.Equal(15, independentIds.Count);
            Assert.Equal(15, militaryIds.Count);

            // Self-uniqueness
            Assert.Equal(15, independentIds.Distinct(StringComparer.OrdinalIgnoreCase).Count());
            Assert.Equal(15, militaryIds.Distinct(StringComparer.OrdinalIgnoreCase).Count());

            // Mutual disjointness
            var overlap = independentIds.Intersect(militaryIds, StringComparer.OrdinalIgnoreCase).ToList();
            Assert.Empty(overlap);
        }

        [Fact]
        public void PonrFlags_AreDisjointAcrossCatalogs()
        {
            string dir = ResolveDataDir();
            var serializer = new SystemTextJsonSerializer();
            var io = new FileSystemIO();

            var independentCatalog = IndependentBranchCatalog.LoadAndRegister(dir, io, serializer);
            var militaryCatalog = MilitaryBranchCatalog.LoadAndRegister(dir, io, serializer);

            var indepPonr = independentCatalog
                .Select(b => b.ponr_flag)
                .Where(f => !string.IsNullOrEmpty(f))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var milPonr = militaryCatalog
                .Select(b => b.ponr_flag)
                .Where(f => !string.IsNullOrEmpty(f))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            Assert.Equal(15, indepPonr.Count);
            Assert.Equal(15, milPonr.Count);

            var overlap = indepPonr.Intersect(milPonr, StringComparer.OrdinalIgnoreCase).ToList();
            Assert.Empty(overlap);
        }

        [Fact]
        public void EndingIds_AreDisjointAcrossCatalogs()
        {
            string dir = ResolveDataDir();
            var serializer = new SystemTextJsonSerializer();
            var io = new FileSystemIO();

            var independentCatalog = IndependentBranchCatalog.LoadAndRegister(dir, io, serializer);
            var militaryCatalog = MilitaryBranchCatalog.LoadAndRegister(dir, io, serializer);

            var indepEndings = independentCatalog
                .SelectMany(b => b.endings)
                .Select(e => e.ending_id)
                .Where(e => !string.IsNullOrEmpty(e))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var milEndings = militaryCatalog
                .SelectMany(b => b.endings)
                .Select(e => e.ending_id)
                .Where(e => !string.IsNullOrEmpty(e))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            Assert.NotEmpty(indepEndings);
            Assert.NotEmpty(milEndings);

            var overlap = indepEndings.Intersect(milEndings, StringComparer.OrdinalIgnoreCase).ToList();
            Assert.Empty(overlap);
        }

        [Fact]
        public void IndependentAndMilitarySystems_OperateConcurrentlyWithoutInterference()
        {
            string dir = ResolveDataDir();
            var serializer = new SystemTextJsonSerializer();
            var io = new FileSystemIO();

            var indepCatalog = IndependentBranchCatalog.LoadAndRegister(dir, io, serializer);
            var milCatalog = MilitaryBranchCatalog.LoadAndRegister(dir, io, serializer);

            var flags = new InMemoryFlagLedger();
            var indepSystem = new IndependentBranchSystem(indepCatalog, flags);
            var milSystem = new MilitaryBranchSystem(milCatalog, flags);

            var moral = MakeMoralChoice(MoralPathBand.Neutral);

            // Commit independent branch (BranchSurvivor: neutral band)
            string committedIndep = indepSystem.CommitBranch(IndependentBranchIds.BranchSurvivor, moral);
            Assert.Equal(IndependentBranchIds.BranchSurvivor, committedIndep);
            Assert.Equal(IndependentBranchIds.BranchSurvivor, indepSystem.CommittedBranchId);

            // Verify military is still uncommitted
            Assert.Null(milSystem.CommittedBranchId);

            // Commit military branch (BranchOpportunist: neutral band)
            string committedMil = milSystem.CommitBranch(MilitaryBranchIds.BranchOpportunist, moral);
            Assert.Equal(MilitaryBranchIds.BranchOpportunist, committedMil);
            Assert.Equal(MilitaryBranchIds.BranchOpportunist, milSystem.CommittedBranchId);

            // Verify independent still holds BranchSurvivor
            Assert.Equal(IndependentBranchIds.BranchSurvivor, indepSystem.CommittedBranchId);

            // Test Independent save codec roundtrip
            var indepSave = IndependentBranchSaveCodec.Capture(indepSystem);
            string indepJson = IndependentBranchSaveCodec.Encode(indepSave, serializer);
            var restoredIndepSave = IndependentBranchSaveCodec.Decode(indepJson, serializer);
            var restoredIndepSystem = new IndependentBranchSystem(indepCatalog, flags);
            IndependentBranchSaveCodec.Restore(restoredIndepSave, restoredIndepSystem);
            Assert.Equal(IndependentBranchIds.BranchSurvivor, restoredIndepSystem.CommittedBranchId);

            // Test Military save codec roundtrip
            var milSave = MilitaryBranchSaveCodec.Capture(milSystem);
            string milJson = MilitaryBranchSaveCodec.Encode(milSave, serializer);
            var restoredMilSave = MilitaryBranchSaveCodec.Decode(milJson, serializer);
            var restoredMilSystem = new MilitaryBranchSystem(milCatalog, flags);
            MilitaryBranchSaveCodec.Restore(restoredMilSave, restoredMilSystem);
            Assert.Equal(MilitaryBranchIds.BranchOpportunist, restoredMilSystem.CommittedBranchId);
        }

        [Fact]
        public void Coordinator_CoordinatesBothCatalogsWithCleanExclusivity()
        {
            string dir = ResolveDataDir();
            var serializer = new SystemTextJsonSerializer();
            var io = new FileSystemIO();

            var indepCatalog = IndependentBranchCatalog.LoadAndRegister(dir, io, serializer);
            var milCatalog = MilitaryBranchCatalog.LoadAndRegister(dir, io, serializer);
            var flags = new InMemoryFlagLedger();

            var coordinator = new FactionBranchCoordinator(
                militaryCatalog: milCatalog,
                rebelCatalog: null,
                independentCatalog: indepCatalog,
                flags: flags);

            var moral = MakeMoralChoice(MoralPathBand.Neutral);
            var options = coordinator.GetBranchOptions(moral);
            Assert.NotEmpty(options);

            var indepOptions = options.Where(o => o.FactionKind == FactionBranchKind.Independent).ToList();
            var milOptions = options.Where(o => o.FactionKind == FactionBranchKind.Military).ToList();

            Assert.Equal(15, indepOptions.Count);
            Assert.Equal(15, milOptions.Count);

            // Commit a military branch through coordinator
            var commitResult = coordinator.CommitBranch(MilitaryBranchIds.BranchOpportunist, moral);
            Assert.True(commitResult.IsSuccess, commitResult.ToString());
            Assert.True(coordinator.IsCommitted);
            Assert.Equal(MilitaryBranchIds.BranchOpportunist, coordinator.ActiveBranchId);
            Assert.Equal(FactionBranchKind.Military, coordinator.ActiveFactionKind);

            // Invariant 1: Base faction commitment is strictly mutually exclusive: committing to one faction locks out the others.
            var tryIndep = coordinator.CommitBranch(IndependentBranchIds.BranchSurvivor, moral);
            Assert.False(tryIndep.IsSuccess);
            Assert.Equal(ActionResult.StatusKind.Blocked, tryIndep.Status);
        }
    }
}
