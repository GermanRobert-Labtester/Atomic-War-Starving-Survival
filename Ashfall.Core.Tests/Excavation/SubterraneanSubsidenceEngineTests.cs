// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Excavation;
using Xunit;

namespace Ashfall.Core.Tests.Excavation
{
    public sealed class SubterraneanSubsidenceEngineTests
    {
        [Fact]
        public void EvaluateSubsidence_ReturnsExpectedCategory_ForShallowUnshored()
        {
            var profile = new ExcavationNodeProfile
            {
                NodeId = "subnode_metro_tunnel",
                Tier = DepthTier.Tier1_ShallowBasement,
                Strata = StrataType.SandstoneUnconsolidated,
                VoidVolumeCubicMeters = 800,
                ShoringLevel = 0,
                StructuralIntegrityPermille = 400 // Damaged
            };

            var result = SubterraneanSubsidenceEngine.EvaluateSubsidence(profile);

            Assert.Equal("subnode_metro_tunnel", result.NodeId);
            Assert.True(result.SubsidenceRiskPermille >= 600, $"Expected high risk, got {result.SubsidenceRiskPermille}");
            Assert.True(result.Category == SubsidenceCategory.Severe || result.Category == SubsidenceCategory.CatastrophicCollapse);
            Assert.True(result.SurfaceDistortionMm > 250);
            Assert.True(result.RequiresImmediateEvacuation);
        }

        [Fact]
        public void EvaluateSubsidence_ShoringMitigatesRisk_Monotonically()
        {
            var baseProfile = new ExcavationNodeProfile
            {
                NodeId = "subnode_quarry",
                Tier = DepthTier.Tier2_ServiceTunnels,
                Strata = StrataType.LimestoneKarst,
                VoidVolumeCubicMeters = 600,
                StructuralIntegrityPermille = 700
            };

            baseProfile.ShoringLevel = 0;
            var r0 = SubterraneanSubsidenceEngine.EvaluateSubsidence(baseProfile);

            baseProfile.ShoringLevel = 1;
            var r1 = SubterraneanSubsidenceEngine.EvaluateSubsidence(baseProfile);

            baseProfile.ShoringLevel = 2;
            var r2 = SubterraneanSubsidenceEngine.EvaluateSubsidence(baseProfile);

            baseProfile.ShoringLevel = 3;
            var r3 = SubterraneanSubsidenceEngine.EvaluateSubsidence(baseProfile);

            Assert.True(r0.SubsidenceRiskPermille > r1.SubsidenceRiskPermille);
            Assert.True(r1.SubsidenceRiskPermille > r2.SubsidenceRiskPermille);
            Assert.True(r2.SubsidenceRiskPermille > r3.SubsidenceRiskPermille);
        }

        [Fact]
        public void EvaluateSubsidence_StrataResilience_ReducesRisk()
        {
            var saltProfile = new ExcavationNodeProfile
            {
                NodeId = "subnode_salt",
                Tier = DepthTier.Tier3_DeepBedrock,
                Strata = StrataType.SaltSeam,
                VoidVolumeCubicMeters = 500,
                ShoringLevel = 1,
                StructuralIntegrityPermille = 800
            };

            var graniteProfile = new ExcavationNodeProfile
            {
                NodeId = "subnode_granite",
                Tier = DepthTier.Tier3_DeepBedrock,
                Strata = StrataType.GraniteSolid,
                VoidVolumeCubicMeters = 500,
                ShoringLevel = 1,
                StructuralIntegrityPermille = 800
            };

            var rSalt = SubterraneanSubsidenceEngine.EvaluateSubsidence(saltProfile);
            var rGranite = SubterraneanSubsidenceEngine.EvaluateSubsidence(graniteProfile);

            Assert.True(rSalt.SubsidenceRiskPermille > rGranite.SubsidenceRiskPermille);
        }

        [Fact]
        public void TryTriggerInducedSeismicEvent_DeterministicSeededRoll()
        {
            var deepVoid = new ExcavationNodeProfile
            {
                NodeId = "subnode_deep_abyss",
                Tier = DepthTier.Tier5_AbyssalCrystalline,
                Strata = StrataType.BasaltShield,
                VoidVolumeCubicMeters = 900,
                ShoringLevel = 0,
                StructuralIntegrityPermille = 300 // Compromised deep void
            };

            const int worldSeed = 42981;
            const long simTick = 10500;

            var outcome1 = SubterraneanSubsidenceEngine.TryTriggerInducedSeismicEvent(deepVoid, simTick, worldSeed);
            var outcome2 = SubterraneanSubsidenceEngine.TryTriggerInducedSeismicEvent(deepVoid, simTick, worldSeed);

            Assert.Equal(outcome1.TremorTriggered, outcome2.TremorTriggered);
            Assert.Equal(outcome1.TremorMagnitude, outcome2.TremorMagnitude);
            Assert.Equal(outcome1.IntegrityDamagePermille, outcome2.IntegrityDamagePermille);
            Assert.Equal(outcome1.NewIntegrityPermille, outcome2.NewIntegrityPermille);
            Assert.Equal(outcome1.CaveInOccurred, outcome2.CaveInOccurred);
        }

        [Fact]
        public void CalculateDailyIntegrityDecay_WaterAcceleratesSaltDissolution()
        {
            var saltProfile = new ExcavationNodeProfile
            {
                NodeId = "salt_drift",
                Strata = StrataType.SaltSeam,
                ShoringLevel = 0
            };

            var graniteProfile = new ExcavationNodeProfile
            {
                NodeId = "granite_drift",
                Strata = StrataType.GraniteSolid,
                ShoringLevel = 0
            };

            int dryDecay = SubterraneanSubsidenceEngine.CalculateDailyIntegrityDecayPermille(saltProfile, 0);
            int floodedSaltDecay = SubterraneanSubsidenceEngine.CalculateDailyIntegrityDecayPermille(saltProfile, 800);
            int floodedGraniteDecay = SubterraneanSubsidenceEngine.CalculateDailyIntegrityDecayPermille(graniteProfile, 800);

            Assert.True(floodedSaltDecay > dryDecay, "Flooding should accelerate decay");
            Assert.True(floodedSaltDecay > floodedGraniteDecay, "Salt should dissolve much faster than granite under water");
        }
    }
}
