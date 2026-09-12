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

namespace Ashfall.Core.Tests
{
    public sealed class RebelBranchExpansionTests
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

        private static RebelBranchCatalog LoadCatalog() =>
            RebelBranchCatalog.LoadAndRegister(
                ResolveDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

        private static MoralChoiceSystem MakeMoralChoice(MoralPathBand band)
        {
            var moral = new MoralChoiceSystem(new StubRng(123));
            int delta = band switch
            {
                MoralPathBand.VeryEvil => -120,
                MoralPathBand.Evil => -60,
                MoralPathBand.SlightlyEvil => -20,
                MoralPathBand.Neutral => 0,
                MoralPathBand.SlightlyPositive => 20,
                MoralPathBand.Positive => 60,
                MoralPathBand.VeryPositive => 120,
                _ => throw new ArgumentOutOfRangeException(nameof(band))
            };

            if (delta != 0)
            {
                var calibration = new MoralChoiceQuestDefinition
                {
                    Id = "quest_moral_rebel_branch_calibration",
                    Choices = { new MoralChoiceOption { MoralDelta = delta } }
                };
                moral.Resolve(calibration, 0, "loc_test", 1);
            }

            Assert.Equal(band, moral.CurrentBand);
            return moral;
        }

        private static readonly string[] ValidBands =
        {
            "very_evil", "evil", "slightly_evil", "neutral",
            "slightly_positive", "positive", "very_positive"
        };

        private static MoralPathBand ParseBand(string token) => token switch
        {
            "very_evil" => MoralPathBand.VeryEvil,
            "evil" => MoralPathBand.Evil,
            "slightly_evil" => MoralPathBand.SlightlyEvil,
            "neutral" => MoralPathBand.Neutral,
            "slightly_positive" => MoralPathBand.SlightlyPositive,
            "positive" => MoralPathBand.Positive,
            "very_positive" => MoralPathBand.VeryPositive,
            _ => throw new ArgumentException("Unknown band " + token)
        };

        [Fact]
        public void Catalog_ContainsExactlyFifteenBranches()
        {
            var catalog = LoadCatalog();

            Assert.Equal(15, catalog.Count);
            Assert.Equal(RebelBranchIds.BranchCount, catalog.Count);
            Assert.Equal(RebelBranchIds.AllBranches, catalog.Select(branch => branch.id).ToArray());
        }

        [Fact]
        public void OriginalEightBranches_PreserveOrderAndShape()
        {
            var catalog = LoadCatalog();
            var expected = new[]
            {
                ("branch_rebel_1_true_rebel", "The True Rebel", "flag_branch_rebel_1_ponr"),
                ("branch_rebel_2_defector", "The Defector", "flag_branch_rebel_2_ponr"),
                ("branch_rebel_3_opportunist", "The Opportunist", "flag_branch_rebel_3_ponr"),
                ("branch_rebel_4_martyr", "The Martyr", "flag_branch_rebel_4_ponr"),
                ("branch_rebel_5_warlord", "The Warlord", "flag_branch_rebel_5_ponr"),
                ("branch_rebel_6_reformer", "The Reformer", "flag_branch_rebel_6_ponr"),
                ("branch_rebel_7_lone_wolf", "The Lone Wolf", "flag_branch_rebel_7_ponr"),
                ("branch_rebel_8_revolution", "The Revolution", "flag_branch_rebel_8_ponr")
            };

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i].Item1, catalog[i].id);
                Assert.Equal(expected[i].Item2, catalog[i].display_name);
                Assert.Equal(expected[i].Item3, catalog[i].ponr_flag);
                Assert.Equal(3, catalog[i].endings.Count);
            }
        }

        [Fact]
        public void SevenNewBranches_AreAppendedWithCanonicalIds()
        {
            var catalog = LoadCatalog();
            var expected = new[]
            {
                (RebelBranchIds.BranchBombmaker, "The Bombmaker", RebelBranchIds.FlagPonrBombmaker),
                (RebelBranchIds.BranchCourier, "The Courier", RebelBranchIds.FlagPonrCourier),
                (RebelBranchIds.BranchPropagandist, "The Propagandist", RebelBranchIds.FlagPonrPropagandist),
                (RebelBranchIds.BranchDissident, "The Dissident", RebelBranchIds.FlagPonrDissident),
                (RebelBranchIds.BranchProtector, "The Protector", RebelBranchIds.FlagPonrProtector),
                (RebelBranchIds.BranchSaboteur, "The Saboteur", RebelBranchIds.FlagPonrSaboteur),
                (RebelBranchIds.BranchNegotiator, "The Negotiator", RebelBranchIds.FlagPonrNegotiator)
            };

            for (int i = 0; i < expected.Length; i++)
            {
                var branch = catalog[8 + i];
                Assert.Equal(expected[i].Item1, branch.id);
                Assert.Equal(expected[i].Item2, branch.display_name);
                Assert.Equal(expected[i].Item3, branch.ponr_flag);
                Assert.Equal(3, branch.endings.Count);
            }
        }

        [Fact]
        public void BranchesFlagsAndEndings_AreUniqueAndUseCanonicalPrefixes()
        {
            var catalog = LoadCatalog();
            var branches = new HashSet<string>(StringComparer.Ordinal);
            var flags = new HashSet<string>(StringComparer.Ordinal);
            var endings = new HashSet<string>(StringComparer.Ordinal);

            foreach (var branch in catalog)
            {
                Assert.StartsWith("branch_rebel_", branch.id);
                Assert.StartsWith("flag_branch_rebel_", branch.ponr_flag);
                Assert.True(branches.Add(branch.id), "Duplicate branch " + branch.id);
                Assert.True(flags.Add(branch.ponr_flag), "Duplicate flag " + branch.ponr_flag);
                Assert.Equal(RebelBranchIds.PonrFlagFor(branch.id), branch.ponr_flag);

                foreach (var ending in branch.endings)
                {
                    Assert.StartsWith("ending_rebel_", ending.ending_id);
                    Assert.False(string.IsNullOrWhiteSpace(ending.display_name));
                    Assert.True(endings.Add(ending.ending_id), "Duplicate ending " + ending.ending_id);
                }
            }

            Assert.Equal(15, branches.Count);
            Assert.Equal(15, flags.Count);
            Assert.Equal(45, endings.Count);
        }

        [Fact]
        public void AllBandsAndEndingRanges_AreValid()
        {
            var catalog = LoadCatalog();

            foreach (var branch in catalog.Skip(8))
            {
                Assert.Contains(branch.entry_band_min, ValidBands);
                Assert.Contains(branch.entry_band_max, ValidBands);
                Assert.True(
                    Array.IndexOf(ValidBands, branch.entry_band_min) <=
                    Array.IndexOf(ValidBands, branch.entry_band_max),
                    "Inverted entry range for " + branch.id);

                foreach (var ending in branch.endings)
                {
                    Assert.Contains(ending.band_min, ValidBands);
                    Assert.Contains(ending.band_max, ValidBands);
                    Assert.True(
                        Array.IndexOf(ValidBands, ending.band_min) <=
                        Array.IndexOf(ValidBands, ending.band_max),
                        "Inverted ending range for " + ending.ending_id);
                }
            }
        }

        [Fact]
        public void SevenNewBranches_HaveExactlyOneEndingForEveryMoralBand()
        {
            var catalog = LoadCatalog();
            var bands = Enum.GetValues(typeof(MoralPathBand)).Cast<MoralPathBand>().ToArray();

            foreach (var branch in catalog.Skip(8))
            {
                foreach (var band in bands)
                {
                    int matches = branch.endings.Count(ending =>
                    {
                        MoralPathBand min = ParseBand(ending.band_min);
                        MoralPathBand max = ParseBand(ending.band_max);
                        return band >= min && band <= max;
                    });
                    Assert.Equal(1, matches);
                }
            }
        }

        [Fact]
        public void SevenNewBranches_CanCommitAtNeutralAndLockTheirOwnPonrFlags()
        {
            var catalog = LoadCatalog();

            foreach (var branch in catalog.Skip(8))
            {
                var flags = new InMemoryFlagLedger();
                var system = new RebelBranchSystem(catalog, flags);
                system.CommitBranch(branch.id, MakeMoralChoice(MoralPathBand.Neutral));
                system.AdvanceDay(123);
                system.LockPointOfNoReturn();

                Assert.True(system.IsPonrLocked);
                Assert.Equal(123, system.State.branch.ponrLockedDay);
                Assert.True(flags.IsSet(branch.ponr_flag));
                Assert.Contains(branch.ponr_flag, system.State.setFlags);
            }
        }

        [Fact]
        public void Coordinator_ExposesAndResolvesANewRebelBranch()
        {
            var coordinator = FactionBranchCoordinator.LoadFromData(
                ResolveDataDir(),
                new FileSystemIO(),
                new SystemTextJsonSerializer(),
                new InMemoryFlagLedger());

            var options = coordinator.GetBranchOptions(MakeMoralChoice(MoralPathBand.Neutral));
            Assert.Contains(options, option =>
                option.BranchId == RebelBranchIds.BranchNegotiator &&
                option.FactionKind == FactionBranchKind.Rebel);

            var moral = MakeMoralChoice(MoralPathBand.Neutral);
            var committed = coordinator.CommitBranch(RebelBranchIds.BranchNegotiator, moral);
            Assert.True(committed.IsSuccess);
            Assert.Equal(RebelBranchIds.BranchNegotiator, coordinator.ActiveBranchId);

            Assert.True(coordinator.LockPonr(90).IsSuccess);
            Assert.True(coordinator.ResolveEnding(moral).IsSuccess);
            Assert.Contains("ending_rebel_15", coordinator.ResolvedEndingId);
        }

        [Fact]
        public void SevenNewBranches_ResolveAnAuthoredEndingAtEveryMoralBand()
        {
            var catalog = LoadCatalog();
            var bands = Enum.GetValues(typeof(MoralPathBand)).Cast<MoralPathBand>().ToArray();

            foreach (var branch in catalog.Skip(8))
            {
                foreach (var band in bands)
                {
                    var system = new RebelBranchSystem(catalog, new InMemoryFlagLedger());
                    system.State.branch.branchId = branch.id;
                    system.State.branch.committed = true;
                    system.LockPointOfNoReturn();

                    string ending = system.ResolveEnding(MakeMoralChoice(band));
                    Assert.Contains(branch.endings, row => row.ending_id == ending);
                }
            }
        }

        [Fact]
        public void NewBranch_SaveRoundTripPreservesPonrAndResolvedEnding()
        {
            var catalog = LoadCatalog();
            var flagsA = new InMemoryFlagLedger();
            var systemA = new RebelBranchSystem(catalog, flagsA);
            var moral = MakeMoralChoice(MoralPathBand.Positive);

            systemA.CommitBranch(RebelBranchIds.BranchNegotiator, moral);
            systemA.AdvanceDay(88);
            systemA.LockPointOfNoReturn();
            string ending = systemA.ResolveEnding(moral);

            var serializer = new SystemTextJsonSerializer();
            string encoded = RebelBranchSaveCodec.Encode(
                RebelBranchSaveCodec.Capture(systemA), serializer);
            var loaded = RebelBranchSaveCodec.Decode(encoded, serializer);
            var flagsB = new InMemoryFlagLedger();
            var systemB = new RebelBranchSystem(catalog, flagsB);
            RebelBranchSaveCodec.Restore(loaded, systemB);

            Assert.Equal(RebelBranchIds.BranchNegotiator, systemB.CommittedBranchId);
            Assert.Equal(88, systemB.State.branch.ponrLockedDay);
            Assert.True(flagsB.IsSet(RebelBranchIds.FlagPonrNegotiator));
            Assert.Equal(ending, systemB.ResolvedEndingId);
        }

        [Fact]
        public void LegacyStateWithoutNewFieldsStillRestores()
        {
            var catalog = LoadCatalog();
            var legacy = new RebelBranchSystemState
            {
                timeline = new RebelBranchTimelineState { currentDay = 17 },
                branch = new RebelBranchRecord
                {
                    branchId = RebelBranchIds.BranchTrueRebel,
                    committed = true
                }
            };

            var flags = new InMemoryFlagLedger();
            var system = new RebelBranchSystem(catalog, flags);
            system.RestoreState(legacy);

            Assert.Equal(RebelBranchIds.BranchTrueRebel, system.CommittedBranchId);
            Assert.Equal(17, system.CurrentDay);
            Assert.False(system.IsPonrLocked);
        }

        [Fact]
        public void HighRiskArcsRemainNonOperationalInTheirNarrativeText()
        {
            var catalog = LoadCatalog();
            string text = string.Join(
                " ",
                catalog.Where(branch =>
                    branch.id == RebelBranchIds.BranchBombmaker ||
                    branch.id == RebelBranchIds.BranchSaboteur ||
                    branch.id == RebelBranchIds.BranchPropagandist)
                    .SelectMany(branch => new[] { branch.ponr_trigger }
                        .Concat(branch.endings.Select(ending => ending.display_name))));

            Assert.DoesNotContain("recipe", text, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("fuse", text, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("detonator", text, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("chemical formula", text, StringComparison.OrdinalIgnoreCase);
        }
    }
}
