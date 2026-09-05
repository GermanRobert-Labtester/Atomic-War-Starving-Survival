using System;
using Ashfall.Core.Flags;
using Ashfall.Core.MoralChoice;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// F5 / Section 5.7 &amp; 37: Integration tests verifying that micro-location world flags
    /// set after encounter resolution can be observed and consumed cleanly across
    /// various systems that read IFlagLedger.
    /// </summary>
    public class WorldFlagConsumerIntegrationTests
    {
        [Fact]
        public void MoralChoiceSystem_ReadsMicroLocationWorldFlags()
        {
            var flags = new CampaignConsequenceLedger();
            flags.Set("micro_generator_marked");

            // MoralChoiceSystem with flags injected
            var moralRng = new SeededRng(100);
            var moral = new MoralChoiceSystem(moralRng, flags: flags);

            Assert.True(moral.HasFlag("micro_generator_marked"));
            Assert.False(moral.HasFlag("micro_contamination_exposure"));
        }

        [Fact]
        public void VigilCare_WithMicroLocationFlags_DoesNotCollide()
        {
            var flags = new CampaignConsequenceLedger();
            flags.Set("micro_contamination_exposure");

            // Verify that vigil care namespace queries do not false-positive on micro flags
            Assert.False(global::Ashfall.Core.Medical.VigilCare.IsKept(flags, "micro_contamination_exposure"));
            Assert.True(flags.IsSet("micro_contamination_exposure"));
        }

        [Fact]
        public void CustomPredicatePipeline_FiltersByMicroWorldFlags()
        {
            var flags = new CampaignConsequenceLedger();

            // Gated predicate: e.g. sick bay admission or decontamination gate
            Func<IFlagLedger, bool> requiresDecon = f => f.IsSet("micro_contamination_exposure");
            Assert.False(requiresDecon(flags));

            flags.Set("micro_contamination_exposure");
            Assert.True(requiresDecon(flags));
        }
    }
}
