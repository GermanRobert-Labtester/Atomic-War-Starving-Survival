using System;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.Flags;
using Ashfall.Core.MoralChoice;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class PrpfStandingSystemTests
    {
        private static MoralChoiceSystem MakeMoralChoice(int seed = 1) =>
            new MoralChoiceSystem(new StubRng(seed));

        [Fact]
        public void DefaultState_StartsNeutralStandingAndPositiveAlignment()
        {
            var system = new PrpfStandingSystem(new InMemoryFlagLedger());

            Assert.Equal(0, system.Standing);
            Assert.False(system.IsHostile);
            Assert.False(system.IsAllied);
            Assert.Equal(120, system.Alignment); // PRPF starts positive-leaning by design
            Assert.False(system.IsJoined);
            Assert.False(system.IsOpposed);
        }

        [Fact]
        public void ModifyStanding_ClampsAndDerivesHostileAllied()
        {
            var system = new PrpfStandingSystem(new InMemoryFlagLedger());

            system.ModifyStanding(1000);
            Assert.Equal(PrpfStandingSystem.MaxStanding, system.Standing);
            Assert.True(system.IsAllied);

            system.ModifyStanding(-1000);
            Assert.Equal(PrpfStandingSystem.MinStanding, system.Standing);
            Assert.True(system.IsHostile);
        }

        [Fact]
        public void ShiftFactionAlignment_ClampsToRange()
        {
            var system = new PrpfStandingSystem(new InMemoryFlagLedger());

            system.ShiftFactionAlignment(1000);
            Assert.Equal(PrpfStandingSystem.MaxAlignment, system.Alignment);

            system.ShiftFactionAlignment(-1000);
            Assert.Equal(PrpfStandingSystem.MinAlignment, system.Alignment);
        }

        [Fact]
        public void TryJoin_NeutralBand_SucceedsBecauseNeutralMeetsThreshold()
        {
            // JoinMinPlayerMoralBand is SlightlyPositive; a fresh MoralChoiceSystem
            // starts at Neutral, which is BELOW SlightlyPositive.
            var system = new PrpfStandingSystem(new InMemoryFlagLedger());
            var moral = MakeMoralChoice();

            bool joined = system.TryJoin(moral);

            Assert.False(joined);
            Assert.False(system.IsJoined);
        }

        [Fact]
        public void TryJoin_PositiveBand_Succeeds()
        {
            var system = new PrpfStandingSystem(new InMemoryFlagLedger());
            var flags = new InMemoryFlagLedger();
            var moral = new MoralChoiceSystem(new StubRng(1));

            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_share_child",
                Choices = { new MoralChoiceOption { MoralDelta = 60, EmpathyDelta = 1 } }
            };
            moral.Resolve(quest, 0, "loc_test", 1);
            Assert.True(moral.CurrentBand >= MoralPathBand.SlightlyPositive);

            bool joined = system.TryJoin(moral);

            Assert.True(joined);
            Assert.True(system.IsJoined);
        }

        [Fact]
        public void TryJoin_AfterOpposed_Fails()
        {
            var system = new PrpfStandingSystem(new InMemoryFlagLedger());
            var moral = new MoralChoiceSystem(new StubRng(1));
            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_share_child",
                Choices = { new MoralChoiceOption { MoralDelta = 60, EmpathyDelta = 1 } }
            };
            moral.Resolve(quest, 0, "loc_test", 1);

            system.Oppose();
            bool joined = system.TryJoin(moral);

            Assert.False(joined);
            Assert.True(system.IsOpposed);
        }

        [Fact]
        public void Oppose_AfterJoined_IsRejected()
        {
            var system = new PrpfStandingSystem(new InMemoryFlagLedger());
            var moral = new MoralChoiceSystem(new StubRng(1));
            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_share_child",
                Choices = { new MoralChoiceOption { MoralDelta = 60, EmpathyDelta = 1 } }
            };
            moral.Resolve(quest, 0, "loc_test", 1);
            system.TryJoin(moral);

            system.Oppose();

            Assert.False(system.IsOpposed);
            Assert.True(system.IsJoined);
        }

        [Fact]
        public void TryJoin_SetsDurableFlag()
        {
            var flags = new InMemoryFlagLedger();
            var system = new PrpfStandingSystem(flags);
            var moral = new MoralChoiceSystem(new StubRng(1));
            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_share_child",
                Choices = { new MoralChoiceOption { MoralDelta = 60, EmpathyDelta = 1 } }
            };
            moral.Resolve(quest, 0, "loc_test", 1);

            system.TryJoin(moral);

            Assert.True(flags.IsSet(PrpfIds.FlagJoined));
        }

        [Fact]
        public void SaveRoundTrip_PreservesStandingAlignmentAndJoinState()
        {
            var flagsA = new InMemoryFlagLedger();
            var systemA = new PrpfStandingSystem(flagsA);
            var moral = new MoralChoiceSystem(new StubRng(1));
            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_share_child",
                Choices = { new MoralChoiceOption { MoralDelta = 60, EmpathyDelta = 1 } }
            };
            moral.Resolve(quest, 0, "loc_test", 1);

            systemA.ModifyStanding(70);
            systemA.ShiftFactionAlignment(-20);
            systemA.TryJoin(moral);

            var save = PrpfSaveCodec.Capture(systemA);
            var jsonSerializer = new SystemTextJsonSerializer();
            string jsonText = PrpfSaveCodec.Encode(save, jsonSerializer);
            var loaded = PrpfSaveCodec.Decode(jsonText, jsonSerializer);

            var flagsB = new InMemoryFlagLedger();
            var systemB = new PrpfStandingSystem(flagsB);
            PrpfSaveCodec.Restore(loaded, systemB);

            Assert.Equal(70, systemB.Standing);
            Assert.True(systemB.IsAllied);
            Assert.Equal(100, systemB.Alignment); // 120 default - 20 shift
            Assert.True(systemB.IsJoined);
            Assert.True(flagsB.IsSet(PrpfIds.FlagJoined));
        }

        [Fact]
        public void Decode_TamperedChecksum_Throws()
        {
            var system = new PrpfStandingSystem(new InMemoryFlagLedger());
            system.ModifyStanding(10);

            var save = PrpfSaveCodec.Capture(system);
            var jsonSerializer = new SystemTextJsonSerializer();
            string jsonText = PrpfSaveCodec.Encode(save, jsonSerializer);
            string tampered = jsonText.Replace("\"standing\":10", "\"standing\":99");

            Assert.Throws<InvalidOperationException>(() => PrpfSaveCodec.Decode(tampered, jsonSerializer));
        }
    }
}
