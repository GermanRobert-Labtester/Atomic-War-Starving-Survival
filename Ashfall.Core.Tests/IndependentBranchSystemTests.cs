using System;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.Flags;
using Ashfall.Core.MoralChoice;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class IndependentBranchSystemTests
    {
        private static IndependentBranchCatalog LoadCatalog()
        {
            string start = System.IO.Directory.GetCurrentDirectory();
            string dir;
            if (!CatalogLocator.TryFindDataDirectory(start, out dir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir);
            Assert.False(string.IsNullOrEmpty(dir), "StreamingAssets/Data must be findable from the test run");
            return IndependentBranchCatalog.LoadAndRegister(dir, new FileSystemIO(), new SystemTextJsonSerializer());
        }

        private static MoralChoiceSystem MakeMoralChoice(int seed = 1) =>
            new MoralChoiceSystem(new StubRng(seed));

        private static MoralChoiceSystem MakePositiveMoralChoice()
        {
            var moral = new MoralChoiceSystem(new StubRng(1));
            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_share_child",
                Choices = { new MoralChoiceOption { MoralDelta = 60, EmpathyDelta = 1 } }
            };
            moral.Resolve(quest, 0, "loc_test", 1);
            return moral;
        }

        private static MoralChoiceSystem MakeEvilMoralChoice()
        {
            var moral = new MoralChoiceSystem(new StubRng(1));
            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_share_child",
                Choices = { new MoralChoiceOption { MoralDelta = -60, EmpathyDelta = 0 } }
            };
            moral.Resolve(quest, 0, "loc_test", 1);
            return moral;
        }

        [Fact]
        public void Catalog_LoadsAllEightBranchesWithThreeEndingsEach()
        {
            var catalog = LoadCatalog();
            Assert.Equal(IndependentBranchIds.BranchCount, catalog.Count);
            foreach (var branchId in IndependentBranchIds.AllBranches)
            {
                var entry = catalog.GetById(branchId);
                Assert.NotNull(entry);
                Assert.Equal(3, entry!.endings.Count);
            }
        }

        [Fact]
        public void Catalog_Ind3HasPrpfGate_Ind4HasDualHostilityGate()
        {
            var catalog = LoadCatalog();

            var ind3 = catalog.GetById(IndependentBranchIds.BranchPeacekeeperDiplomat)!;
            Assert.True(ind3.requires_prpf_standing_min.HasValue);
            Assert.Equal(20, ind3.requires_prpf_standing_min.Value);

            var ind4 = catalog.GetById(IndependentBranchIds.BranchExile)!;
            Assert.True(ind4.requires_hostile_to_military == true);
            Assert.True(ind4.requires_hostile_to_rebel == true);

            // No other branch should carry these Independent-only gates.
            var ind1 = catalog.GetById(IndependentBranchIds.BranchSurvivor)!;
            Assert.False(ind1.requires_prpf_standing_min.HasValue);
            Assert.Null(ind1.requires_hostile_to_military);
        }

        [Fact]
        public void CommitBranch_WithoutIndependentGates_Succeeds()
        {
            var catalog = LoadCatalog();
            var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
            var moral = MakeMoralChoice();

            string committed = system.CommitBranch(IndependentBranchIds.BranchSurvivor, moral);

            Assert.Equal(IndependentBranchIds.BranchSurvivor, committed);
        }

        [Fact]
        public void CommitBranch_Ind3_WithoutPrpfSystem_Throws()
        {
            var catalog = LoadCatalog();
            var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
            var moral = MakePositiveMoralChoice();

            // IND-3 declares requires_prpf_standing_min but no PrpfStandingSystem
            // is supplied — must fail loud, not silently skip the gate.
            Assert.Throws<InvalidOperationException>(() =>
                system.CommitBranch(IndependentBranchIds.BranchPeacekeeperDiplomat, moral));
        }

        [Fact]
        public void CommitBranch_Ind3_WithInsufficientPrpfStanding_Throws()
        {
            var catalog = LoadCatalog();
            var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
            var moral = MakePositiveMoralChoice();
            var prpf = new PrpfStandingSystem(new InMemoryFlagLedger());
            prpf.ModifyStanding(5); // below the branch's requires_prpf_standing_min of 20

            Assert.Throws<InvalidOperationException>(() =>
                system.CommitBranch(IndependentBranchIds.BranchPeacekeeperDiplomat, moral, prpf));
        }

        [Fact]
        public void CommitBranch_Ind3_WithSufficientPrpfStanding_Succeeds()
        {
            var catalog = LoadCatalog();
            var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
            var moral = MakePositiveMoralChoice();
            var prpf = new PrpfStandingSystem(new InMemoryFlagLedger());
            prpf.ModifyStanding(30);

            string committed = system.CommitBranch(IndependentBranchIds.BranchPeacekeeperDiplomat, moral, prpf);

            Assert.Equal(IndependentBranchIds.BranchPeacekeeperDiplomat, committed);
        }

        [Fact]
        public void CommitBranch_Ind4_WithoutHostilityToEither_Throws()
        {
            var catalog = LoadCatalog();
            var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
            var moral = MakeEvilMoralChoice();

            Assert.Throws<InvalidOperationException>(() =>
                system.CommitBranch(IndependentBranchIds.BranchExile, moral));
        }

        [Fact]
        public void CommitBranch_Ind4_HostileToOnlyOneFaction_Throws()
        {
            var catalog = LoadCatalog();
            var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
            var moral = MakeEvilMoralChoice();

            system.ModifyMilitaryStanding(-80); // hostile to Military only
            Assert.True(system.IsHostileToMilitary);
            Assert.False(system.IsHostileToRebel);

            Assert.Throws<InvalidOperationException>(() =>
                system.CommitBranch(IndependentBranchIds.BranchExile, moral));
        }

        [Fact]
        public void CommitBranch_Ind4_HostileToBothFactions_Succeeds()
        {
            var catalog = LoadCatalog();
            var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
            var moral = MakeEvilMoralChoice();

            system.ModifyMilitaryStanding(-80);
            system.ModifyRebelStanding(-80);

            string committed = system.CommitBranch(IndependentBranchIds.BranchExile, moral);

            Assert.Equal(IndependentBranchIds.BranchExile, committed);
        }

        [Fact]
        public void ModifyMilitaryStanding_ClampsAndDerivesHostileAllied()
        {
            var catalog = LoadCatalog();
            var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());

            system.ModifyMilitaryStanding(1000);
            Assert.Equal(IndependentBranchSystem.MaxStanding, system.MilitaryStanding);

            system.ModifyMilitaryStanding(-2000);
            Assert.Equal(IndependentBranchSystem.MinStanding, system.MilitaryStanding);
            Assert.True(system.IsHostileToMilitary);
        }

        [Fact]
        public void ModifyRebelStanding_ClampsAndDerivesHostileAllied()
        {
            var catalog = LoadCatalog();
            var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());

            system.ModifyRebelStanding(1000);
            Assert.Equal(IndependentBranchSystem.MaxStanding, system.RebelStanding);
            Assert.True(system.State.rebelStanding.isAllied);
        }

        [Fact]
        public void LockPointOfNoReturn_SetsDurableAndRuntimeFlag()
        {
            var catalog = LoadCatalog();
            var flags = new InMemoryFlagLedger();
            var system = new IndependentBranchSystem(catalog, flags);
            var moral = MakeMoralChoice();

            system.CommitBranch(IndependentBranchIds.BranchSurvivor, moral);
            system.AdvanceDay(90);
            system.LockPointOfNoReturn();

            Assert.True(system.IsPonrLocked);
            Assert.True(flags.IsSet(IndependentBranchIds.FlagPonrSurvivor));
            Assert.Equal(90, system.State.branch.ponrLockedDay);
        }

        [Fact]
        public void ResolveEnding_NeutralBand_ResolvesExpectedEnding()
        {
            var catalog = LoadCatalog();
            var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
            var moral = MakeMoralChoice(); // Neutral band

            system.CommitBranch(IndependentBranchIds.BranchSurvivor, moral);
            system.LockPointOfNoReturn();
            string ending = system.ResolveEnding(moral);

            Assert.Equal(IndependentBranchIds.EndingSurvivorA, ending);
        }

        [Fact]
        public void ResolveEnding_IsIdempotent()
        {
            var catalog = LoadCatalog();
            var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
            var moral = MakeMoralChoice();

            system.CommitBranch(IndependentBranchIds.BranchSurvivor, moral);
            system.LockPointOfNoReturn();
            string first = system.ResolveEnding(moral);
            string second = system.ResolveEnding(moral);

            Assert.Equal(first, second);
        }

        [Fact]
        public void IsGameOver_ZeroOrNegativeSurvivors_IsTrue()
        {
            Assert.True(IndependentBranchSystem.IsGameOver(0));
            Assert.False(IndependentBranchSystem.IsGameOver(1));
        }

        [Fact]
        public void SaveRoundTrip_PreservesBranchStandingsAndFlags()
        {
            var catalog = LoadCatalog();
            var flagsA = new InMemoryFlagLedger();
            var systemA = new IndependentBranchSystem(catalog, flagsA);
            var moral = MakeMoralChoice();

            systemA.ModifyMilitaryStanding(-30);
            systemA.ModifyRebelStanding(15);
            systemA.CommitBranch(IndependentBranchIds.BranchSurvivor, moral);
            systemA.AdvanceDay(50);
            systemA.LockPointOfNoReturn();
            systemA.ResolveEnding(moral);

            var save = IndependentBranchSaveCodec.Capture(systemA);
            var jsonSerializer = new SystemTextJsonSerializer();
            string jsonText = IndependentBranchSaveCodec.Encode(save, jsonSerializer);
            var loaded = IndependentBranchSaveCodec.Decode(jsonText, jsonSerializer);

            var flagsB = new InMemoryFlagLedger();
            var systemB = new IndependentBranchSystem(catalog, flagsB);
            IndependentBranchSaveCodec.Restore(loaded, systemB);

            Assert.Equal(-30, systemB.MilitaryStanding);
            Assert.Equal(15, systemB.RebelStanding);
            Assert.Equal(IndependentBranchIds.BranchSurvivor, systemB.CommittedBranchId);
            Assert.True(systemB.IsPonrLocked);
            Assert.Equal(systemA.ResolvedEndingId, systemB.ResolvedEndingId);
            Assert.True(flagsB.IsSet(IndependentBranchIds.FlagPonrSurvivor));
        }

        [Fact]
        public void Decode_TamperedChecksum_Throws()
        {
            var catalog = LoadCatalog();
            var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
            var moral = MakeMoralChoice();
            system.CommitBranch(IndependentBranchIds.BranchSurvivor, moral);

            var save = IndependentBranchSaveCodec.Capture(system);
            var jsonSerializer = new SystemTextJsonSerializer();
            string jsonText = IndependentBranchSaveCodec.Encode(save, jsonSerializer);
            string tampered = jsonText.Replace(IndependentBranchIds.BranchSurvivor, IndependentBranchIds.BranchGhost);

            Assert.Throws<InvalidOperationException>(() => IndependentBranchSaveCodec.Decode(tampered, jsonSerializer));
        }

        [Fact]
        public void RestoreState_WrongSystemId_Throws()
        {
            var catalog = LoadCatalog();
            var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
            var badState = new IndependentBranchSystemState { systemId = "not_the_right_system" };

            Assert.Throws<ArgumentException>(() => system.RestoreState(badState));
        }
    }
}
