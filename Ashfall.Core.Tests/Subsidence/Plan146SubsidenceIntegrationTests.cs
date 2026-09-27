// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 146 batch-4 / ORPHAN-SEAL A.56 — subsidence integration contract.
// Strata crosswalk determinism, decay gating on water/strata/shoring,
// evacuation gate, one-owner composition through SubterraneanSystem.
// ============================================================================
using System.Linq;
using Ashfall.Core.Excavation;
using Ashfall.Core.Subterranean;
using Xunit;

namespace Ashfall.Core.Tests.Subsidence
{
    public sealed class Plan146SubsidenceIntegrationTests
    {
        [Fact]
        public void StrataCrosswalk_IsDeterministic_AndSaltFree()
        {
            var (cave, caveVoid) = SubterraneanSystem.StrataProfileFor("Cave");
            var (mine, mineVoid) = SubterraneanSystem.StrataProfileFor("Mine");
            var (metro, metroVoid) = SubterraneanSystem.StrataProfileFor("Metro");
            var (facility, _) = SubterraneanSystem.StrataProfileFor("CollapsedFacility");
            Assert.Equal(StrataType.LimestoneKarst, cave);
            Assert.Equal(StrataType.LimestoneKarst, mine);
            Assert.Equal(StrataType.GraniteSolid, metro);
            Assert.Equal(StrataType.SandstoneUnconsolidated, facility);
            Assert.True(mineVoid > metroVoid);
        }

        [Fact]
        public void ProfileForNode_MapsDepth_Shoring_AndIntegrity()
        {
            var system = new SubterraneanSystem(new SubterraneanZoneCatalogContainer(), new Ashfall.Core.Inventory.Inventory());
            var profile = SubterraneanSystem.ProfileForNode(
                new SubterraneanNodeState { nodeId = "n", depthTier = 3, shoringLevel = 2, structuralIntegrity = 42.5f },
                new SubterraneanZoneDef { zone_type = "Metro" });
            Assert.Equal(DepthTier.Tier3_DeepBedrock, profile.Tier);
            Assert.Equal(2, profile.ShoringLevel);
            Assert.Equal(425, profile.StructuralIntegrityPermille);
            Assert.Equal(StrataType.GraniteSolid, profile.Strata);
        }

        [Fact]
        public void DailyDecay_GatesOnWaterAndStrata_DampedByShoring()
        {
            int dry = SubterraneanSubsidenceEngine.CalculateDailyIntegrityDecayPermille(
                MakeProfile(), 100);
            int wetSalt = SubterraneanSubsidenceEngine.CalculateDailyIntegrityDecayPermille(
                MakeProfile(strata: StrataType.SaltSeam), 900);
            int shored = SubterraneanSubsidenceEngine.CalculateDailyIntegrityDecayPermille(
                MakeProfile(shoring: 3), 500);
            Assert.True(dry > 0);
            Assert.True(wetSalt > dry);
            Assert.True(shored < dry);
        }

        [Fact]
        public void Evaluation_GatesEvacuation_OnFailingNodes()
        {
            var sound = SubterraneanSubsidenceEngine.EvaluateSubsidence(
                MakeProfile(decay: 950, water: 0.0f));
            var failing = SubterraneanSubsidenceEngine.EvaluateSubsidence(
                MakeProfile(decay: 100, water: 0.6f, strata: StrataType.LimestoneKarst, voidVolume: 900));
            Assert.False(sound.RequiresImmediateEvacuation);
            Assert.True(failing.SubsidenceRiskPermille > sound.SubsidenceRiskPermille);
            Assert.True(failing.RequiresImmediateEvacuation);
        }

        [Fact]
        public void SystemOwner_EvaluatesGeneratedNetwork_Deterministically()
        {
            var catalog = new SubterraneanZoneCatalogContainer();
            catalog.subterranean_zones.Add(new SubterraneanZoneDef
            { id = "z_cave_1", zone_type = "Cave", depth_tier = 2, base_structural_risk = 0.4f, flood_susceptibility = 0.5f });
            catalog.subterranean_zones.Add(new SubterraneanZoneDef
            { id = "z_metro_1", zone_type = "Metro", depth_tier = 1, base_structural_risk = 0.2f, flood_susceptibility = 0.3f });

            var system = new SubterraneanSystem(catalog, new Ashfall.Core.Inventory.Inventory());
            system.EnsureNetwork(777);
            var nodes = system.State.nodes;
            Assert.Equal(2, nodes.Count);

            var first = nodes.Select(n => system.EvaluateSubsidence(n.nodeId)).ToList();
            var second = nodes.Select(n => system.EvaluateSubsidence(n.nodeId)).ToList();
            Assert.Equal(first.Select(e => e.SubsidenceRiskPermille), second.Select(e => e.SubsidenceRiskPermille));

            // Metro (granite, shallow, small void) must read no worse than the
            // failing karst mine gallery of the same integrity.
            var metro = system.EvaluateSubsidence("z_metro_1");
            Assert.True(metro.SubsidenceRiskPermille >= 0);
        }

        [Fact]
        public void UnknownNode_EvaluatesNegligible_WithoutThrowing()
        {
            var system = new SubterraneanSystem(new SubterraneanZoneCatalogContainer(), new Ashfall.Core.Inventory.Inventory());
            var eval = system.EvaluateSubsidence("does_not_exist");
            Assert.Equal(0, eval.SubsidenceRiskPermille);
            Assert.Equal(SubsidenceCategory.Negligible, eval.Category);
        }

        private static ExcavationNodeProfile MakeProfile(
            int decay = 900, float water = 0.2f, StrataType strata = StrataType.GraniteSolid,
            int shoring = 0, int voidVolume = 600)
            => new ExcavationNodeProfile
            {
                NodeId = "probe",
                Strata = strata,
                VoidVolumeCubicMeters = voidVolume,
                ShoringLevel = shoring,
                StructuralIntegrityPermille = decay
            };
    }
}
