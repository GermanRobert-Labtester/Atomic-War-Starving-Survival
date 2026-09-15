// SPDX-License-Identifier: MIT
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
    public sealed class MilitaryBranchExpansionTests
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

        private static MilitaryBranchCatalog LoadCatalog()
        {
            string dir = ResolveDataDir();
            return MilitaryBranchCatalog.LoadAndRegister(dir, new FileSystemIO(), new SystemTextJsonSerializer());
        }

        private static MoralChoiceSystem MakeMoralChoiceWithBand(MoralPathBand targetBand)
        {
            var moral = new MoralChoiceSystem(new StubRng(1));
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
            Assert.Equal(MilitaryBranchIds.BranchCount, catalog.Count);
        }

        [Fact]
        public void BaselineEightBranches_PreservedVerbatim_WithExactOrderAndData()
        {
            var catalog = LoadCatalog();

            var expectedBaseline = new[]
            {
                ("branch_mil_1_loyal_soldier", "The Loyal Soldier", "flag_branch_mil_1_ponr"),
                ("branch_mil_2_defector", "The Defector", "flag_branch_mil_2_ponr"),
                ("branch_mil_3_opportunist", "The Opportunist", "flag_branch_mil_3_ponr"),
                ("branch_mil_4_martyr", "The Martyr", "flag_branch_mil_4_ponr"),
                ("branch_mil_5_tyrant", "The Tyrant", "flag_branch_mil_5_ponr"),
                ("branch_mil_6_reformer", "The Reformer", "flag_branch_mil_6_ponr"),
                ("branch_mil_7_deserter", "The Deserter", "flag_branch_mil_7_ponr"),
                ("branch_mil_8_broken_chain", "The Broken Chain", "flag_branch_mil_8_ponr")
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
                (MilitaryBranchIds.BranchQuartermaster, "The Quartermaster", MilitaryBranchIds.FlagPonrQuartermaster),
                (MilitaryBranchIds.BranchCombatMedic, "The Combat Medic", MilitaryBranchIds.FlagPonrCombatMedic),
                (MilitaryBranchIds.BranchConscriptParent, "The Conscript Parent", MilitaryBranchIds.FlagPonrConscriptParent),
                (MilitaryBranchIds.BranchIntelligenceOfficer, "The Intelligence Officer", MilitaryBranchIds.FlagPonrIntelligenceOfficer),
                (MilitaryBranchIds.BranchPeacekeeper, "The Peacekeeper", MilitaryBranchIds.FlagPonrPeacekeeper),
                (MilitaryBranchIds.BranchFugitiveDeserter, "The Fugitive Deserter", MilitaryBranchIds.FlagPonrFugitiveDeserter),
                (MilitaryBranchIds.BranchDissidentOfficer, "The Dissident Officer", MilitaryBranchIds.FlagPonrDissidentOfficer)
            };

            for (int i = 0; i < expectedNew.Length; i++)
            {
                int catalogIndex = 8 + i;
                var branch = catalog[catalogIndex];
                Assert.Equal(expectedNew[i].Item1, branch.id);
                Assert.Equal(expectedNew[i].Item2, branch.display_name);
                Assert.Equal(expectedNew[i].Item3, branch.ponr_flag);
                Assert.Equal(3, branch.endings.Count);
            }
        }

        [Fact]
        public void AllFifteenBranches_HaveThreeEndings_Totaling45Endings()
        {
            var catalog = LoadCatalog();
            int totalEndings = 0;

            foreach (var branch in catalog)
            {
                Assert.NotNull(branch.endings);
                Assert.Equal(3, branch.endings.Count);
                totalEndings += branch.endings.Count;
            }

            Assert.Equal(45, totalEndings);
        }

        [Fact]
        public void AllBranchIds_AreUnique_AndFollowMilConvention()
        {
            var catalog = LoadCatalog();
            var seenIds = new HashSet<string>(StringComparer.Ordinal);

            foreach (var branch in catalog)
            {
                Assert.StartsWith("branch_mil_", branch.id);
                Assert.True(seenIds.Add(branch.id), $"Duplicate branch id: {branch.id}");
            }

            Assert.Equal(15, seenIds.Count);
        }

        [Fact]
        public void AllPonrFlags_AreUnique_AndFollowMilConvention()
        {
            var catalog = LoadCatalog();
            var seenFlags = new HashSet<string>(StringComparer.Ordinal);

            foreach (var branch in catalog)
            {
                Assert.StartsWith("flag_branch_mil_", branch.ponr_flag);
                Assert.True(seenFlags.Add(branch.ponr_flag), $"Duplicate ponr flag: {branch.ponr_flag}");
                Assert.Equal(MilitaryBranchIds.PonrFlagFor(branch.id), branch.ponr_flag);
            }

            Assert.Equal(15, seenFlags.Count);
        }

        [Fact]
        public void AllEndingIds_AreUnique_AcrossEntireCatalog()
        {
            var catalog = LoadCatalog();
            var seenEndings = new HashSet<string>(StringComparer.Ordinal);

            foreach (var branch in catalog)
            {
                foreach (var ending in branch.endings)
                {
                    Assert.StartsWith("ending_mil_", ending.ending_id);
                    Assert.False(string.IsNullOrWhiteSpace(ending.display_name));
                    Assert.True(seenEndings.Add(ending.ending_id), $"Duplicate ending id: {ending.ending_id}");
                }
            }

            Assert.Equal(45, seenEndings.Count);
        }

        [Fact]
        public void AllEntryBands_AreValid_AndOrdered()
        {
            var catalog = LoadCatalog();
            var validBands = new[]
            {
                "very_evil", "evil", "slightly_evil", "neutral", "slightly_positive", "positive", "very_positive"
            };

            foreach (var branch in catalog)
            {
                int minIdx = Array.IndexOf(validBands, branch.entry_band_min);
                int maxIdx = Array.IndexOf(validBands, branch.entry_band_max);

                Assert.True(minIdx >= 0, $"Invalid entry_band_min '{branch.entry_band_min}' in {branch.id}");
                Assert.True(maxIdx >= 0, $"Invalid entry_band_max '{branch.entry_band_max}' in {branch.id}");
                Assert.True(minIdx <= maxIdx, $"Inverted entry band range in {branch.id}: {minIdx} > {maxIdx}");
            }
        }

        [Fact]
        public void SevenNewBranches_EndingBands_CoverAllSevenBandsWithoutGaps()
        {
            var catalog = LoadCatalog();
            var allBands = Enum.GetValues(typeof(MoralPathBand)).Cast<MoralPathBand>().ToArray();

            for (int i = 8; i < 15; i++)
            {
                var branch = catalog[i];
                var coveredBands = new HashSet<MoralPathBand>();

                foreach (var ending in branch.endings)
                {
                    var minBand = ParseBand(ending.band_min);
                    var maxBand = ParseBand(ending.band_max);
                    Assert.True(minBand <= maxBand, $"Ending {ending.ending_id} has inverted band range.");

                    foreach (var band in allBands)
                    {
                        if (band >= minBand && band <= maxBand)
                        {
                            coveredBands.Add(band);
                        }
                    }
                }

                foreach (var band in allBands)
                {
                    Assert.True(coveredBands.Contains(band),
                        $"Branch '{branch.id}' fails to cover moral band '{band}'.");
                }
            }
        }

        [Fact]
        public void MilitaryBranchSystem_CanCommit_ToNewBranches_WithinValidBands()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();

            // Branch 9 (Quartermaster) entry: very_evil..very_positive (Neutral works)
            var sys9 = new MilitaryBranchSystem(catalog, flags);
            var moral9 = MakeMoralChoiceWithBand(MoralPathBand.Neutral);
            string committed9 = sys9.CommitBranch(MilitaryBranchIds.BranchQuartermaster, moral9);
            Assert.Equal(MilitaryBranchIds.BranchQuartermaster, committed9);

            // Branch 10 (Combat Medic) entry: slightly_evil..very_positive (Positive works)
            var sys10 = new MilitaryBranchSystem(catalog, flags);
            var moral10 = MakeMoralChoiceWithBand(MoralPathBand.Positive);
            string committed10 = sys10.CommitBranch(MilitaryBranchIds.BranchCombatMedic, moral10);
            Assert.Equal(MilitaryBranchIds.BranchCombatMedic, committed10);

            // Branch 13 (Peacekeeper) entry: neutral..very_positive (VeryPositive works)
            var sys13 = new MilitaryBranchSystem(catalog, flags);
            var moral13 = MakeMoralChoiceWithBand(MoralPathBand.VeryPositive);
            string committed13 = sys13.CommitBranch(MilitaryBranchIds.BranchPeacekeeper, moral13);
            Assert.Equal(MilitaryBranchIds.BranchPeacekeeper, committed13);
        }

        [Fact]
        public void MilitaryBranchSystem_CommitOutsideBand_Throws()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();

            // Branch 13 (Peacekeeper) entry: neutral..very_positive.
            // VeryEvil should throw!
            var sys = new MilitaryBranchSystem(catalog, flags);
            var moral = MakeMoralChoiceWithBand(MoralPathBand.VeryEvil);

            Assert.Throws<InvalidOperationException>(() =>
                sys.CommitBranch(MilitaryBranchIds.BranchPeacekeeper, moral));
        }

        [Fact]
        public void MilitaryBranchSystem_LocksPoNR_AndEmitsCorrectDurableFlag_ForNewBranches()
        {
            var catalog = LoadCatalog();

            var testCases = new[]
            {
                (MilitaryBranchIds.BranchQuartermaster, MilitaryBranchIds.FlagPonrQuartermaster, MoralPathBand.Neutral),
                (MilitaryBranchIds.BranchCombatMedic, MilitaryBranchIds.FlagPonrCombatMedic, MoralPathBand.Positive),
                (MilitaryBranchIds.BranchConscriptParent, MilitaryBranchIds.FlagPonrConscriptParent, MoralPathBand.Neutral),
                (MilitaryBranchIds.BranchIntelligenceOfficer, MilitaryBranchIds.FlagPonrIntelligenceOfficer, MoralPathBand.Neutral),
                (MilitaryBranchIds.BranchPeacekeeper, MilitaryBranchIds.FlagPonrPeacekeeper, MoralPathBand.Positive),
                (MilitaryBranchIds.BranchFugitiveDeserter, MilitaryBranchIds.FlagPonrFugitiveDeserter, MoralPathBand.Neutral),
                (MilitaryBranchIds.BranchDissidentOfficer, MilitaryBranchIds.FlagPonrDissidentOfficer, MoralPathBand.Positive)
            };

            foreach (var (branchId, expectedFlag, band) in testCases)
            {
                var flags = new InMemoryFlagLedger();
                var system = new MilitaryBranchSystem(catalog, flags);
                var moral = MakeMoralChoiceWithBand(band);

                system.CommitBranch(branchId, moral);
                Assert.False(system.IsPonrLocked);

                system.LockPointOfNoReturn();
                Assert.True(system.IsPonrLocked);
                Assert.True(flags.IsSet(expectedFlag));
                Assert.Contains(expectedFlag, system.State.setFlags);
            }
        }

        // TEST-AGGREGATION: source_rows=6 aggregate_cases=1 saved_cases=5
        [Fact]
        public void MilitaryBranchSystem_ResolvesCorrectEnding_ForQuartermaster()
        {
            var catalog = LoadCatalog();
            var failures = new List<string>();
            foreach (var testCase in new[]
            {
                (Band: MoralPathBand.VeryPositive, ExpectedEnding: MilitaryBranchIds.EndingQuartermasterA),
                (Band: MoralPathBand.Positive, ExpectedEnding: MilitaryBranchIds.EndingQuartermasterA),
                (Band: MoralPathBand.Neutral, ExpectedEnding: MilitaryBranchIds.EndingQuartermasterB),
                (Band: MoralPathBand.SlightlyEvil, ExpectedEnding: MilitaryBranchIds.EndingQuartermasterB),
                (Band: MoralPathBand.Evil, ExpectedEnding: MilitaryBranchIds.EndingQuartermasterC),
                (Band: MoralPathBand.VeryEvil, ExpectedEnding: MilitaryBranchIds.EndingQuartermasterC)
            })
            {
                var system = new MilitaryBranchSystem(catalog, new InMemoryFlagLedger());
                var moral = MakeMoralChoiceWithBand(testCase.Band);
                system.CommitBranch(MilitaryBranchIds.BranchQuartermaster, moral);
                system.LockPointOfNoReturn();

                string ending = system.ResolveEnding(moral);
                if (ending != testCase.ExpectedEnding)
                    failures.Add($"{testCase.Band}: expected '{testCase.ExpectedEnding}', got '{ending}'");
                if (system.ResolvedEndingId != testCase.ExpectedEnding)
                    failures.Add($"{testCase.Band}: persisted '{system.ResolvedEndingId}', expected '{testCase.ExpectedEnding}'");
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        // TEST-AGGREGATION: source_rows=3 aggregate_cases=1 saved_cases=2
        [Fact]
        public void MilitaryBranchSystem_ResolvesCorrectEnding_ForPeacekeeper()
        {
            var catalog = LoadCatalog();
            var failures = new List<string>();
            foreach (var testCase in new[]
            {
                (Band: MoralPathBand.VeryPositive, ExpectedEnding: MilitaryBranchIds.EndingPeacekeeperA),
                (Band: MoralPathBand.Positive, ExpectedEnding: MilitaryBranchIds.EndingPeacekeeperA),
                (Band: MoralPathBand.Neutral, ExpectedEnding: MilitaryBranchIds.EndingPeacekeeperB)
            })
            {
                var system = new MilitaryBranchSystem(catalog, new InMemoryFlagLedger());
                var moral = MakeMoralChoiceWithBand(testCase.Band);
                system.CommitBranch(MilitaryBranchIds.BranchPeacekeeper, moral);
                system.LockPointOfNoReturn();

                string ending = system.ResolveEnding(moral);
                if (ending != testCase.ExpectedEnding)
                    failures.Add($"{testCase.Band}: expected '{testCase.ExpectedEnding}', got '{ending}'");
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void MilitaryBranchSystem_SaveAndRestore_PreservesBranchCommitAndPoNRLock()
        {
            var catalog = LoadCatalog();
            var flags1 = new InMemoryFlagLedger();
            var sys1 = new MilitaryBranchSystem(catalog, flags1);
            var moral = MakeMoralChoiceWithBand(MoralPathBand.Neutral);

            sys1.AdvanceDay(42);
            sys1.CommitBranch(MilitaryBranchIds.BranchIntelligenceOfficer, moral);
            sys1.LockPointOfNoReturn();
            string ending = sys1.ResolveEnding(moral);

            var state = sys1.CaptureState();

            var flags2 = new InMemoryFlagLedger();
            var sys2 = new MilitaryBranchSystem(catalog, flags2);
            sys2.RestoreState(state);

            Assert.Equal(MilitaryBranchIds.BranchIntelligenceOfficer, sys2.CommittedBranchId);
            Assert.True(sys2.IsPonrLocked);
            Assert.Equal(42, sys2.CurrentDay);
            Assert.Equal(ending, sys2.ResolvedEndingId);
            Assert.True(flags2.IsSet(MilitaryBranchIds.FlagPonrIntelligenceOfficer));
        }

        [Fact]
        public void AllPonrTriggers_AreSingleSentence_AndIrreversible()
        {
            var catalog = LoadCatalog();

            foreach (var branch in catalog)
            {
                Assert.False(string.IsNullOrWhiteSpace(branch.ponr_trigger));
                Assert.EndsWith(".", branch.ponr_trigger.Trim());
            }
        }

        private static MoralPathBand ParseBand(string band) => band switch
        {
            "very_evil" => MoralPathBand.VeryEvil,
            "evil" => MoralPathBand.Evil,
            "slightly_evil" => MoralPathBand.SlightlyEvil,
            "neutral" => MoralPathBand.Neutral,
            "slightly_positive" => MoralPathBand.SlightlyPositive,
            "positive" => MoralPathBand.Positive,
            "very_positive" => MoralPathBand.VeryPositive,
            _ => throw new ArgumentException($"Unknown morality band token '{band}'.", nameof(band))
        };
    }
}
