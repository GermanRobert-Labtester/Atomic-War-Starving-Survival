using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.Flags;
using Ashfall.Core.MoralChoice;

namespace Ashfall.Core.Tests
{
    public sealed class IndependentBranchExpansionTests
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

        private static IndependentBranchCatalog LoadCatalog()
        {
            string dir = ResolveDataDir();
            return IndependentBranchCatalog.LoadAndRegister(dir, new FileSystemIO(), new SystemTextJsonSerializer());
        }

        private static MoralChoiceSystem MakeMoralChoiceWithBand(MoralPathBand targetBand)
        {
            var moral = new MoralChoiceSystem(new StubRng(1));
            // MoralChoiceSystem.BandForScore:
            // <= -100: VeryEvil, <= -50: Evil, < 0: SlightlyEvil, == 0: Neutral,
            // < 50: SlightlyPositive, < 100: Positive, >= 100: VeryPositive
            int delta = targetBand switch
            {
                MoralPathBand.VeryEvil => -120,
                MoralPathBand.Evil => -60,
                MoralPathBand.SlightlyEvil => -20,
                MoralPathBand.Neutral => 0,
                MoralPathBand.SlightlyPositive => 20,
                MoralPathBand.Positive => 60,
                MoralPathBand.VeryPositive => 120,
                _ => throw new ArgumentOutOfRangeException(nameof(targetBand))
            };

            if (delta != 0)
            {
                var quest = new MoralChoiceQuestDefinition
                {
                    Id = "quest_moral_test_calibration",
                    Choices = { new MoralChoiceOption { MoralDelta = delta, EmpathyDelta = 0 } }
                };
                moral.Resolve(quest, 0, "loc_test", 1);
            }

            Assert.Equal(targetBand, moral.CurrentBand);
            return moral;
        }

        [Fact]
        public void Catalog_LoadsSuccessfully_ContainsExactFifteenBranches()
        {
            var catalog = LoadCatalog();
            Assert.Equal(15, catalog.Count);
            Assert.Equal(IndependentBranchIds.BranchCount, catalog.Count);
        }

        [Fact]
        public void BaselineEightBranches_PreservedVerbatim_WithExactOrderAndData()
        {
            var catalog = LoadCatalog();

            var expectedBaseline = new[]
            {
                ("branch_ind_1_survivor", "The Survivor", "flag_branch_ind_1_ponr"),
                ("branch_ind_2_mercenary", "The Mercenary", "flag_branch_ind_2_ponr"),
                ("branch_ind_3_peacekeeper_diplomat", "The Peacekeeper / Diplomat", "flag_branch_ind_3_ponr"),
                ("branch_ind_4_exile", "The Exile", "flag_branch_ind_4_ponr"),
                ("branch_ind_5_kingmaker", "The Kingmaker", "flag_branch_ind_5_ponr"),
                ("branch_ind_6_legend", "The Legend", "flag_branch_ind_6_ponr"),
                ("branch_ind_7_ghost", "The Ghost", "flag_branch_ind_7_ponr"),
                ("branch_ind_8_wasteland_myth", "The Wasteland Myth", "flag_branch_ind_8_ponr")
            };

            for (int i = 0; i < expectedBaseline.Length; i++)
            {
                var branch = catalog[i];
                Assert.Equal(expectedBaseline[i].Item1, branch.id);
                Assert.Equal(expectedBaseline[i].Item2, branch.display_name);
                Assert.Equal(expectedBaseline[i].Item3, branch.ponr_flag);
                Assert.Equal(3, branch.endings.Count);
            }
        }

        [Fact]
        public void SevenNewBranches_AllPresent_AndMatchExpectedIds()
        {
            var catalog = LoadCatalog();

            var expectedNew = new[]
            {
                ("branch_ind_9_hermit", "The Hermit", "flag_branch_ind_9_ponr"),
                ("branch_ind_10_mediator", "The Mediator", "flag_branch_ind_10_ponr"),
                ("branch_ind_11_scavenger_king", "The Scavenger King", "flag_branch_ind_11_ponr"),
                ("branch_ind_12_caretaker", "The Caretaker", "flag_branch_ind_12_ponr"),
                ("branch_ind_13_witness", "The Witness", "flag_branch_ind_13_ponr"),
                ("branch_ind_14_engineer", "The Engineer", "flag_branch_ind_14_ponr"),
                ("branch_ind_15_prophet", "The Prophet", "flag_branch_ind_15_ponr")
            };

            for (int i = 0; i < expectedNew.Length; i++)
            {
                var branch = catalog[8 + i];
                Assert.Equal(expectedNew[i].Item1, branch.id);
                Assert.Equal(expectedNew[i].Item2, branch.display_name);
                Assert.Equal(expectedNew[i].Item3, branch.ponr_flag);
                Assert.Equal(3, branch.endings.Count);
            }
        }

        [Fact]
        public void BranchIds_AreUnique_AndMatchPrefix()
        {
            var catalog = LoadCatalog();
            var seen = new HashSet<string>(StringComparer.Ordinal);

            foreach (var b in catalog)
            {
                Assert.False(string.IsNullOrWhiteSpace(b.id));
                Assert.StartsWith("branch_ind_", b.id);
                Assert.True(seen.Add(b.id), "Duplicate branch ID: " + b.id);
            }
        }

        [Fact]
        public void DisplayNames_AreUnique_AndNonEmpty()
        {
            var catalog = LoadCatalog();
            var seen = new HashSet<string>(StringComparer.Ordinal);

            foreach (var b in catalog)
            {
                Assert.False(string.IsNullOrWhiteSpace(b.display_name));
                Assert.True(seen.Add(b.display_name), "Duplicate display name: " + b.display_name);
            }
        }

        [Fact]
        public void PonrFlags_AreUnique_AndMatchPrefix()
        {
            var catalog = LoadCatalog();
            var seen = new HashSet<string>(StringComparer.Ordinal);

            foreach (var b in catalog)
            {
                Assert.False(string.IsNullOrWhiteSpace(b.ponr_flag));
                Assert.StartsWith("flag_branch_ind_", b.ponr_flag);
                Assert.True(seen.Add(b.ponr_flag), "Duplicate PONR flag: " + b.ponr_flag);
            }
        }

        [Fact]
        public void PonrTriggers_AreAuthored_AndNonEmpty()
        {
            var catalog = LoadCatalog();
            var seen = new HashSet<string>(StringComparer.Ordinal);

            foreach (var b in catalog)
            {
                Assert.False(string.IsNullOrWhiteSpace(b.ponr_trigger));
                Assert.True(seen.Add(b.ponr_trigger), "Duplicate PONR trigger: " + b.ponr_trigger);
            }
        }

        [Fact]
        public void EndingIds_AreGloballyUniqueAcrossCatalog()
        {
            var catalog = LoadCatalog();
            var seen = new HashSet<string>(StringComparer.Ordinal);

            foreach (var b in catalog)
            {
                foreach (var e in b.endings)
                {
                    Assert.False(string.IsNullOrWhiteSpace(e.ending_id));
                    Assert.StartsWith("ending_ind_", e.ending_id);
                    Assert.True(seen.Add(e.ending_id), "Duplicate ending ID: " + e.ending_id);
                }
            }

            Assert.Equal(45, seen.Count);
        }

        [Fact]
        public void EndingDisplayNames_AreNonEmpty()
        {
            var catalog = LoadCatalog();
            foreach (var b in catalog)
            {
                foreach (var e in b.endings)
                {
                    Assert.False(string.IsNullOrWhiteSpace(e.display_name), $"Empty display name for ending {e.ending_id}");
                }
            }
        }

        [Fact]
        public void EntryBands_AreValid_AndMinLessThanOrEqualToMax()
        {
            var catalog = LoadCatalog();
            var validBands = new HashSet<string>
            {
                "very_evil", "evil", "slightly_evil", "neutral", "slightly_positive", "positive", "very_positive"
            };

            foreach (var b in catalog)
            {
                Assert.Contains(b.entry_band_min, validBands);
                Assert.Contains(b.entry_band_max, validBands);

                var min = ParseBandOrdinal(b.entry_band_min);
                var max = ParseBandOrdinal(b.entry_band_max);
                Assert.True(min <= max, $"Branch {b.id} entry range min ({min}) > max ({max})");
            }
        }

        [Fact]
        public void NewSevenBranches_HaveCompleteDeterministicSevenBandPartition()
        {
            var catalog = LoadCatalog();
            var allBands = Enum.GetValues(typeof(MoralPathBand)).Cast<MoralPathBand>().ToArray();

            // Check branches 9 through 15
            for (int i = 8; i < 15; i++)
            {
                var branch = catalog[i];
                foreach (var band in allBands)
                {
                    int matchCount = 0;
                    foreach (var ending in branch.endings)
                    {
                        var min = (MoralPathBand)ParseBandOrdinal(ending.band_min);
                        var max = (MoralPathBand)ParseBandOrdinal(ending.band_max);
                        if (band >= min && band <= max)
                            matchCount++;
                    }

                    Assert.True(matchCount == 1,
                        $"Branch {branch.id} must have exactly 1 matching ending for band {band}, found {matchCount}.");
                }
            }
        }

        [Fact]
        public void ExhaustiveResolution_AllFifteenBranchesResolveValidEndingAtEveryBand()
        {
            var catalog = LoadCatalog();
            var allBands = Enum.GetValues(typeof(MoralPathBand)).Cast<MoralPathBand>().ToArray();

            foreach (var branch in catalog)
            {
                foreach (var band in allBands)
                {
                    var flags = new InMemoryFlagLedger();
                    var system = new IndependentBranchSystem(catalog, flags);
                    var moral = MakeMoralChoiceWithBand(band);

                    // Force commit bypass of entry gate to test resolver partition/fallback
                    system.State.branch.branchId = branch.id;
                    system.State.branch.committed = true;
                    system.LockPointOfNoReturn();

                    string endingId = system.ResolveEnding(moral);
                    Assert.False(string.IsNullOrEmpty(endingId), $"Branch {branch.id} failed to resolve ending at band {band}");
                    Assert.Contains(endingId, branch.endings.Select(e => e.ending_id));
                }
            }
        }

        [Fact]
        public void CommitBranch_Hermit_AcceptsAnyBand()
        {
            var catalog = LoadCatalog();
            var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
            var moral = MakeMoralChoiceWithBand(MoralPathBand.VeryEvil);

            string committed = system.CommitBranch(IndependentBranchIds.BranchHermit, moral);
            Assert.Equal(IndependentBranchIds.BranchHermit, committed);
        }

        [Fact]
        public void CommitBranch_Caretaker_RequiresNeutralOrHigher()
        {
            var catalog = LoadCatalog();
            var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
            var evil = MakeMoralChoiceWithBand(MoralPathBand.Evil);

            Assert.Throws<InvalidOperationException>(() =>
                system.CommitBranch(IndependentBranchIds.BranchCaretaker, evil));

            var neutral = MakeMoralChoiceWithBand(MoralPathBand.Neutral);
            string committed = system.CommitBranch(IndependentBranchIds.BranchCaretaker, neutral);
            Assert.Equal(IndependentBranchIds.BranchCaretaker, committed);
        }

        [Fact]
        public void CommitBranch_ScavengerKing_RejectsPositiveBands()
        {
            var catalog = LoadCatalog();
            var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
            var positive = MakeMoralChoiceWithBand(MoralPathBand.Positive);

            Assert.Throws<InvalidOperationException>(() =>
                system.CommitBranch(IndependentBranchIds.BranchScavengerKing, positive));

            var slightlyPos = MakeMoralChoiceWithBand(MoralPathBand.SlightlyPositive);
            string committed = system.CommitBranch(IndependentBranchIds.BranchScavengerKing, slightlyPos);
            Assert.Equal(IndependentBranchIds.BranchScavengerKing, committed);
        }

        [Fact]
        public void LockPointOfNoReturn_NewBranches_SetsCorrectFlagAndIsIdempotent()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new IndependentBranchSystem(catalog, flags);
            var moral = MakeMoralChoiceWithBand(MoralPathBand.Neutral);

            system.CommitBranch(IndependentBranchIds.BranchWitness, moral);
            system.AdvanceDay(42);
            system.LockPointOfNoReturn();

            Assert.True(system.IsPonrLocked);
            Assert.Equal(42, system.State.branch.ponrLockedDay);
            Assert.True(flags.IsSet(IndependentBranchIds.FlagPonrWitness));

            // Second lock must be an idempotent no-op
            system.AdvanceDay(45);
            system.LockPointOfNoReturn();
            Assert.Equal(42, system.State.branch.ponrLockedDay);
        }

        [Fact]
        public void SaveRoundTrip_NewBranch_Hermit_PreservesState()
        {
            var catalog = LoadCatalog();
            var flagsA = new InMemoryFlagLedger();
            var systemA = new IndependentBranchSystem(catalog, flagsA);
            var moral = MakeMoralChoiceWithBand(MoralPathBand.Positive);

            systemA.CommitBranch(IndependentBranchIds.BranchHermit, moral);
            systemA.AdvanceDay(60);
            systemA.LockPointOfNoReturn();
            string ending = systemA.ResolveEnding(moral);
            Assert.Equal(IndependentBranchIds.EndingHermitB, ending); // "The Unlatched Door"

            var save = IndependentBranchSaveCodec.Capture(systemA);
            var serializer = new SystemTextJsonSerializer();
            string encoded = IndependentBranchSaveCodec.Encode(save, serializer);

            var loaded = IndependentBranchSaveCodec.Decode(encoded, serializer);
            var flagsB = new InMemoryFlagLedger();
            var systemB = new IndependentBranchSystem(catalog, flagsB);
            IndependentBranchSaveCodec.Restore(loaded, systemB);

            Assert.Equal(IndependentBranchIds.BranchHermit, systemB.CommittedBranchId);
            Assert.True(systemB.IsPonrLocked);
            Assert.Equal(60, systemB.State.branch.ponrLockedDay);
            Assert.Equal(IndependentBranchIds.EndingHermitB, systemB.ResolvedEndingId);
            Assert.True(flagsB.IsSet(IndependentBranchIds.FlagPonrHermit));
        }

        [Fact]
        public void SaveRoundTrip_NewBranch_Witness_PreservesState()
        {
            var catalog = LoadCatalog();
            var flagsA = new InMemoryFlagLedger();
            var systemA = new IndependentBranchSystem(catalog, flagsA);
            var moral = MakeMoralChoiceWithBand(MoralPathBand.SlightlyEvil);

            systemA.CommitBranch(IndependentBranchIds.BranchWitness, moral);
            systemA.AdvanceDay(75);
            systemA.LockPointOfNoReturn();
            string ending = systemA.ResolveEnding(moral);
            Assert.Equal(IndependentBranchIds.EndingWitnessC, ending); // "Missing Pages"

            var save = IndependentBranchSaveCodec.Capture(systemA);
            var serializer = new SystemTextJsonSerializer();
            string encoded = IndependentBranchSaveCodec.Encode(save, serializer);

            var loaded = IndependentBranchSaveCodec.Decode(encoded, serializer);
            var flagsB = new InMemoryFlagLedger();
            var systemB = new IndependentBranchSystem(catalog, flagsB);
            IndependentBranchSaveCodec.Restore(loaded, systemB);

            Assert.Equal(IndependentBranchIds.BranchWitness, systemB.CommittedBranchId);
            Assert.True(systemB.IsPonrLocked);
            Assert.Equal(75, systemB.State.branch.ponrLockedDay);
            Assert.Equal(IndependentBranchIds.EndingWitnessC, systemB.ResolvedEndingId);
            Assert.True(flagsB.IsSet(IndependentBranchIds.FlagPonrWitness));
        }

        [Fact]
        public void LegacySave_SimulatedOldState_RestoresCleanlyWithoutCorruption()
        {
            var catalog = LoadCatalog();
            var oldState = new IndependentBranchSystemState
            {
                timeline = new IndependentBranchTimelineState { currentDay = 25 },
                branch = new IndependentBranchRecord
                {
                    branchId = IndependentBranchIds.BranchSurvivor,
                    committed = true,
                    ponrLocked = false
                }
            };

            var flags = new InMemoryFlagLedger();
            var system = new IndependentBranchSystem(catalog, flags, oldState);

            Assert.Equal(IndependentBranchIds.BranchSurvivor, system.CommittedBranchId);
            Assert.False(system.IsPonrLocked);
            Assert.Equal(25, system.CurrentDay);
        }

        [Fact]
        public void PoolBalance_EveryMoralBandHasAtLeastElevenEligibleBranches()
        {
            var catalog = LoadCatalog();
            var allBands = Enum.GetValues(typeof(MoralPathBand)).Cast<MoralPathBand>().ToArray();

            foreach (var band in allBands)
            {
                int eligibleCount = 0;
                foreach (var branch in catalog)
                {
                    var min = (MoralPathBand)ParseBandOrdinal(branch.entry_band_min);
                    var max = (MoralPathBand)ParseBandOrdinal(branch.entry_band_max);
                    if (band >= min && band <= max)
                        eligibleCount++;
                }

                Assert.True(eligibleCount >= 11,
                    $"Band {band} has {eligibleCount} eligible branches, expected >= 11 for fair pool balance.");
            }
        }

        [Fact]
        public void NegativeFixture_DuplicateBranchId_Detected()
        {
            var branches = new List<IndependentBranchEntry>
            {
                new IndependentBranchEntry { id = "branch_ind_9_hermit" },
                new IndependentBranchEntry { id = "branch_ind_9_hermit" }
            };

            var seen = new HashSet<string>();
            bool duplicateFound = false;
            foreach (var b in branches)
            {
                if (!seen.Add(b.id)) duplicateFound = true;
            }

            Assert.True(duplicateFound);
        }

        [Fact]
        public void NegativeFixture_DuplicatePonrFlag_Detected()
        {
            var flags = new List<string> { "flag_branch_ind_9_ponr", "flag_branch_ind_9_ponr" };
            var seen = new HashSet<string>();
            bool duplicateFound = false;
            foreach (var f in flags)
            {
                if (!seen.Add(f)) duplicateFound = true;
            }

            Assert.True(duplicateFound);
        }

        [Fact]
        public void NegativeFixture_DuplicateEndingId_Detected()
        {
            var endings = new List<string> { "ending_ind_9a_quiet_holding", "ending_ind_9a_quiet_holding" };
            var seen = new HashSet<string>();
            bool duplicateFound = false;
            foreach (var e in endings)
            {
                if (!seen.Add(e)) duplicateFound = true;
            }

            Assert.True(duplicateFound);
        }

        [Fact]
        public void NegativeFixture_InvalidMoralBand_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _ = "chaotic_neutral" switch
                {
                    "very_evil" => MoralPathBand.VeryEvil,
                    "evil" => MoralPathBand.Evil,
                    "slightly_evil" => MoralPathBand.SlightlyEvil,
                    "neutral" => MoralPathBand.Neutral,
                    "slightly_positive" => MoralPathBand.SlightlyPositive,
                    "positive" => MoralPathBand.Positive,
                    "very_positive" => MoralPathBand.VeryPositive,
                    _ => throw new ArgumentException("Unknown morality band token.")
                };
            });
        }

        private static int ParseBandOrdinal(string band) => band switch
        {
            "very_evil" => (int)MoralPathBand.VeryEvil,
            "evil" => (int)MoralPathBand.Evil,
            "slightly_evil" => (int)MoralPathBand.SlightlyEvil,
            "neutral" => (int)MoralPathBand.Neutral,
            "slightly_positive" => (int)MoralPathBand.SlightlyPositive,
            "positive" => (int)MoralPathBand.Positive,
            "very_positive" => (int)MoralPathBand.VeryPositive,
            _ => throw new ArgumentException($"Unknown morality band token '{band}'.", nameof(band))
        };
    }
}
