using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Candidate-A adapter probes (Phase E loop): the core-side contract the
    /// Unity DynamicEconomySystem delegates to. Verifies delegation semantics,
    /// sentinel behavior, clamps, and save/restore mapping at the core level.
    /// </summary>
    public class MarketAdapterProbeTests
    {
        private static GoodsCatalog Catalog()
        {
            var result = new GoodsCatalogLoadResult();
            result.Goods.Add(new GoodDefinition
            {
                id = "probe_water", displayName = "Water", category = "water",
                basePrice = 8f, volatility = 0.1f, elasticity = 1f
            });
            return GoodsCatalogLoader.ToCatalog(result);
        }

        [Fact]
        public void Probe_DemandDelegationContract_MirrorsCore()
        {
            var core = new MarketSystem();
            core.BindCatalog(Catalog());
            core.AdjustDemand("probe_water", 0.5f);

            // The Unity adapter reads exactly what the core holds.
            Assert.Equal(1.5f, core.GetDemandMultiplier("probe_water"));
            Assert.True(core.IsSuppliesShort()); // 1.5 >= 1.35
            Assert.Equal(8f * 1.5f, core.GetPrice("probe_water"));
        }

        [Fact]
        public void Probe_AdjustDemandClamps_AtCoreBounds()
        {
            var core = new MarketSystem();
            core.AdjustDemand("x", 100f);
            Assert.Equal(MarketSystem.MaxDemandMult, core.GetDemandMultiplier("x"));
            core.AdjustDemand("x", -1000f);
            Assert.Equal(MarketSystem.MinDemandMult, core.GetDemandMultiplier("x"));
            // Untracked items read neutral (Unity parity).
            Assert.Equal(1f, core.GetDemandMultiplier("untracked"));
        }

        [Fact]
        public void Probe_Sentinel_ZeroAndNegativeClamped()
        {
            var core = new MarketSystem();
            // Zero/negative demand must never surface (the Unity adapter maps
            // through Mathf.Clamp before RestoreState, and the core re-clamps).
            core.RestoreState(new MarketState
            {
                version = MarketState.Version,
                demand = new List<DemandEntry>
                {
                    new DemandEntry { itemId = "x", multiplier = -3f },
                    new DemandEntry { itemId = "y", multiplier = 0f }
                }
            });
            Assert.Equal(MarketSystem.MinDemandMult, core.GetDemandMultiplier("x"));
            Assert.Equal(MarketSystem.MinDemandMult, core.GetDemandMultiplier("y"));
        }

        [Fact]
        public void Probe_SaveRestoreMapping_StableChecksum()
        {
            var core = new MarketSystem();
            core.AdjustDemand("a", 0.3f);
            core.AdjustDemand("b", -0.2f);
            string before = SaveChecksum.Compute(core.CaptureState());

            // Simulate the Unity adapter's mapping: rows out, rows back in.
            var snapshot = core.CaptureState();
            var mapped = new MarketState { version = MarketState.Version };
            mapped.demand.AddRange(snapshot.demand);
            var restored = new MarketSystem();
            restored.RestoreState(mapped);
            string after = SaveChecksum.Compute(restored.CaptureState());

            Assert.Equal(before, after);
            Assert.Equal(core.GetDemandMultiplier("a"), restored.GetDemandMultiplier("a"));
            Assert.Equal(core.GetDemandMultiplier("b"), restored.GetDemandMultiplier("b"));
        }

        [Fact]
        public void Probe_TuningOverlay_DayGatesAndFallback()
        {
            // Critical tier (days 1-15) from the sample JSON shape.
            var load = HardcoreEconomyTuningLoader.Load(
                "{\"version\":1,\"scarcity_tiers\":[" +
                "{\"tier\":\"Critical\",\"multiplier\":2.5,\"day_range_label\":\"Days 1-15\"," +
                "\"affected_item_ids\":[\"clean_water\"],\"rationale\":\"t\"}]," +
                "\"faction_preferences\":[],\"price_shock_rules\":[]}");
            Assert.True(load.IsValid);

            var overlay = new HardcoreEconomyTuning();
            overlay.Apply(load.Bundle);
            Assert.Equal(2.5f, overlay.GetScarcityMultiplier(5, "clean_water"));
            Assert.Equal(1.0f, overlay.GetScarcityMultiplier(16, "clean_water")); // outside window
            Assert.Equal(1.0f, overlay.GetScarcityMultiplier(5, "other_item"));   // not affected

            // Empty overlay (no JSON) = Unity parity (no scarcity).
            var empty = new HardcoreEconomyTuning();
            Assert.False(empty.IsActive);
            Assert.Equal(1.0f, empty.GetScarcityMultiplier(5, "clean_water"));
        }

        [Fact]
        public void Probe_TuningLoader_MalformedAndUnknownTierRejected()
        {
            var malformed = HardcoreEconomyTuningLoader.Load("{ not json");
            Assert.False(malformed.IsValid);
            Assert.NotNull(malformed.Errors);
            Assert.NotEmpty(malformed.Errors);

            var badTier = HardcoreEconomyTuningLoader.Load(
                "{\"version\":1,\"scarcity_tiers\":[" +
                "{\"tier\":\"Mythic\",\"multiplier\":2.5,\"day_range_label\":\"Days 1-15\"," +
                "\"affected_item_ids\":[\"x\"],\"rationale\":\"t\"}]," +
                "\"faction_preferences\":[],\"price_shock_rules\":[]}");
            Assert.False(badTier.IsValid);
            Assert.Contains(badTier.Errors, e => e.Contains("invalid tier"));
        }

        [Fact]
        public void Probe_ForwardVersionRejected_Loudly()
        {
            var overlay = new HardcoreEconomyTuning();
            var future = HardcoreEconomyTuningLoader.Load(
                "{\"version\":99,\"scarcity_tiers\":[],\"faction_preferences\":[],\"price_shock_rules\":[]}");
            Assert.False(future.IsValid);
            Assert.Contains(future.Errors, e => e.Contains("Unsupported tuning version"));
        }
    }
}
