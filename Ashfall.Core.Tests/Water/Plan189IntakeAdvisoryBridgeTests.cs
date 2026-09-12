// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Plan189Water
{
    /// <summary>
    /// Plan 189 implement package — piezometer advisory → water-treatment intake
    /// gate. Proves a blocked source grants zero liters, the advisory round-trip
    /// works end to end through public APIs, and the two calls are consumed once.
    /// </summary>
    public sealed class Plan189IntakeAdvisoryBridgeTests
    {
        private const string DeepSourceId = "source_deep_borehole";
        private const string ShallowSourceId = "source_shallow_well";
        private const string AdvisoryId = "advisory_aquifer_network_primary_12";

        private static WaterTreatmentSystem CreateWater() => new WaterTreatmentSystem();

        [Fact]
        public void Blocked_source_grants_zero_liters()
        {
            var water = CreateWater();
            water.RegisterContaminationAdvisory(AdvisoryId, "warning", new List<string> { DeepSourceId });

            float before = water.RawWater;
            var result = water.TryAddWaterFromSource(DeepSourceId, WaterType.Raw, 250f);

            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
            Assert.Equal("intake_source_blocked", result.FailureCode);
            Assert.Equal(before, water.RawWater);
        }

        [Fact]
        public void Unblocked_source_still_grants()
        {
            var water = CreateWater();
            water.RegisterContaminationAdvisory(AdvisoryId, "warning", new List<string> { DeepSourceId });

            float before = water.RawWater;
            var result = water.TryAddWaterFromSource(ShallowSourceId, WaterType.Raw, 120f);

            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal(before + 120f, water.RawWater, 3);
        }

        [Fact]
        public void Untagged_legacy_producer_is_always_admitted()
        {
            var water = CreateWater();
            water.RegisterContaminationAdvisory(AdvisoryId, "warning", new List<string> { DeepSourceId });

            float before = water.CleanWater;
            var result = water.TryAddWaterFromSource(string.Empty, WaterType.Clean, 40f);

            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal(before + 40f, water.CleanWater, 3);
        }

        [Fact]
        public void Advisory_is_consumed_once()
        {
            var water = CreateWater();

            Assert.True(water.RegisterContaminationAdvisory(AdvisoryId, "warning", new List<string> { DeepSourceId }));
            Assert.False(water.RegisterContaminationAdvisory(AdvisoryId, "warning", new List<string> { DeepSourceId }));
        }

        [Fact]
        public void Clear_advisory_does_not_block_and_type_mapping_is_unchanged()
        {
            var water = CreateWater();
            water.RegisterContaminationAdvisory(AdvisoryId, "none", new List<string>());

            Assert.False(water.IsIntakeSourceBlocked(DeepSourceId));
            Assert.Equal(ActionResult.StatusKind.Success,
                water.TryAddWaterFromSource(DeepSourceId, WaterType.Brackish, 10f).Status);
            Assert.Equal(10f, water.BrackishWater, 3);
        }

        [Fact]
        public void Advisory_round_trip_from_live_piezometer_gates_the_named_source()
        {
            // Real engine → real advisory → real intake gate (the bridge's two calls).
            var engine = new AquiferPiezometerEngine();
            engine.BindCatalog(CreateCatalog());
            engine.RestoreState(CreateContaminatedNetwork());

            var advisory = engine.BuildAdvisory();
            Assert.NotEqual("none", advisory.advisory_level);
            Assert.Contains(DeepSourceId, advisory.contaminated_source_ids);

            var water = CreateWater();
            bool accepted = water.RegisterContaminationAdvisory(
                advisory.advisory_id, advisory.advisory_level, advisory.contaminated_source_ids);
            Assert.True(accepted);

            Assert.True(water.IsIntakeSourceBlocked(DeepSourceId));
            Assert.False(water.IsIntakeSourceBlocked(ShallowSourceId));
        }

        [Fact]
        public void Unconstructed_network_issues_no_advisory()
        {
            var engine = new AquiferPiezometerEngine();
            engine.BindCatalog(CreateCatalog());
            // Old-save baseline: unbuilt network (invariant 14).
            engine.RestoreState(new HydrogeologyNetworkState
            {
                network_id = "aquifer_network_primary",
                constructed = false
            });

            var advisory = engine.BuildAdvisory();
            Assert.Equal("none", advisory.advisory_level);
            Assert.Empty(advisory.contaminated_source_ids);
        }

        [Fact]
        public void Sump_source_id_is_a_documented_synthetic_id_not_a_strata_source()
        {
            // Guards the map's §3.1 rule: producers name documented synthetic ids,
            // never a piezometer strata id.
            Assert.Equal("source_sump_greywater", SumpFloodingSystem.SumpGreywaterSourceId);
            Assert.DoesNotContain(SumpFloodingSystem.SumpGreywaterSourceId,
                CreateCatalog().strata[0].monitored_source_ids);
        }

        private static AquiferPiezometerCatalog CreateCatalog() => new AquiferPiezometerCatalog
        {
            network_id = "aquifer_network_primary",
            strata = new List<PiezometerStrataDef>
            {
                new PiezometerStrataDef
                {
                    strata_id = "strata_deep_fractured_bedrock",
                    monitored_source_ids = new List<string> { DeepSourceId }
                },
                new PiezometerStrataDef
                {
                    strata_id = "strata_shallow_gravel",
                    monitored_source_ids = new List<string> { ShallowSourceId }
                }
            },
            forecast = new PiezometerForecastTuning
            {
                warning_signal_threshold = 0.45f,
                minimum_warning_confidence = 0.05f
            }
        };

        private static HydrogeologyNetworkState CreateContaminatedNetwork() => new HydrogeologyNetworkState
        {
            network_id = "aquifer_network_primary",
            constructed = true,
            contamination_risk = 0.8f,
            monitoring_confidence = 0.9f,
            drawdown_state = "stable",
            nodes = new List<PiezometerNodeState>
            {
                new PiezometerNodeState
                {
                    node_id = "node_strata_deep_fractured_bedrock",
                    strata_id = "strata_deep_fractured_bedrock",
                    condition = 100f,
                    calibration = 1f,
                    head_index = 0.8f,
                    contamination_signal = 0.9f
                }
            }
        };
    }
}
