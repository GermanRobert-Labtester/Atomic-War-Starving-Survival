// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    public sealed class DistressDestinationResolverTests : CatalogTestBase
    {
        private readonly DistressDestinationResolver _resolver = DistressDestinationResolver.Default;

        [Fact]
        public void CanonicalDestinations_All55PresentAndValid()
        {
            Assert.Equal(55, _resolver.TotalCanonicalDestinations);
            var all = _resolver.GetAllCanonical();
            Assert.Equal(55, all.Count);

            foreach (var dest in all)
            {
                Assert.False(string.IsNullOrWhiteSpace(dest.Id));
                Assert.False(string.IsNullOrWhiteSpace(dest.DisplayName));
                Assert.InRange(dest.DistanceTicks, 1, 30);
                Assert.InRange(dest.DangerLevel, 1, 10);
                Assert.False(string.IsNullOrWhiteSpace(dest.ScavengingTableId));
                Assert.NotEmpty(dest.LootCategories);
            }
        }

        [Theory]
        [InlineData("checkpoint_kilo_armory", 6, 4)]
        [InlineData("loc_recovery_yard", 6, 6)]
        [InlineData("rural_gas_station", 3, 3)]
        [InlineData("suburban_house", 2, 2)]
        [InlineData("loc_denial_cut_substation", 8, 4)]
        [InlineData("location_silent_observatory", 14, 8)]
        public void DirectResolution_CanonicalId_ResolvesDirectly(string destId, int expectedDistance, int expectedDanger)
        {
            var res = _resolver.Resolve(destId);
            Assert.True(res.IsValid);
            Assert.Equal(destId, res.DestinationId);
            Assert.Equal("Direct", res.ResolutionMode);
            Assert.Equal(expectedDistance, res.DistanceTicks);
            Assert.Equal(expectedDanger, res.DangerLevel);
            Assert.Null(res.ErrorCode);
        }

        [Theory]
        [InlineData("raider_ambush_site", "collapsed_building")]
        [InlineData("loc_bridge_seven", "loc_weighbridge")]
        [InlineData("location_substation_omega", "electrical_substation")]
        public void AliasedResolution_NonCanonicalLocations_ResolveToCanonical(string rawLoc, string expectedCanonical)
        {
            var res = _resolver.Resolve(rawLoc);
            Assert.True(res.IsValid);
            Assert.Equal(expectedCanonical, res.DestinationId);
            Assert.Equal("Aliased", res.ResolutionMode);
            Assert.Null(res.ErrorCode);
        }

        [Theory]
        [InlineData("loc_checkpoint_kilo", "checkpoint_kilo_armory")]
        [InlineData("loc_bunker_4_east_trap", "collapsed_building")]
        [InlineData("loc_sector_9_substation", "electrical_substation")]
        [InlineData("loc_relay_44_bunker", "loc_weighbridge")]
        [InlineData("loc_marsh_caravan_wreck", "loc_water_station")]
        [InlineData("loc_meridian_cold_store", "loc_the_allotments")]
        [InlineData("loc_river_barge_olenka", "loc_lock_gate_four")]
        [InlineData("loc_field_medic_post", "prewar_medical_cache")]
        public void BuiltinFallbackAliases_All8ResolveToCanonical(string rawLoc, string expectedCanonical)
        {
            var res = _resolver.Resolve(rawLoc);
            Assert.True(res.IsValid);
            Assert.Equal(expectedCanonical, res.DestinationId);
            Assert.Equal("Aliased", res.ResolutionMode);
            Assert.Null(res.ErrorCode);
        }

        [Theory]
        [InlineData("table_loot_farm", "loc_the_allotments")]
        [InlineData("table_loot_power_substation", "loc_denial_cut_substation")]
        [InlineData("table_loot_apartment_block", "suburban_house")]
        [InlineData("table_loot_industrial_district", "rural_gas_station")]
        [InlineData("table_loot_hospital", "hospital_pharmacy")]
        public void ScavengingTableId_ResolvesToCanonicalDestination(string tableId, string expectedDestId)
        {
            var res = _resolver.Resolve(tableId);
            Assert.True(res.IsValid);
            Assert.Equal(expectedDestId, res.DestinationId);
            Assert.Equal("AliasedTable", res.ResolutionMode);
            Assert.Null(res.ErrorCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void NullOrEmpty_ResolvesToFallbackWithErrorCode(string? emptyLocation)
        {
            var res = _resolver.Resolve(emptyLocation);
            Assert.False(res.IsValid);
            Assert.Equal("collapsed_building", res.DestinationId);
            Assert.Equal("Fallback", res.ResolutionMode);
            Assert.Equal("EMPTY_LOCATION_ID", res.ErrorCode);
        }

        [Fact]
        public void UnknownLocation_ResolvesToFallbackWithUnmappedErrorCode()
        {
            var res = _resolver.Resolve("completely_fictional_location_xyz");
            Assert.False(res.IsValid);
            Assert.Equal("collapsed_building", res.DestinationId);
            Assert.Equal("Fallback", res.ResolutionMode);
            Assert.Equal("UNMAPPED_LOCATION_completely_fictional_location_xyz", res.ErrorCode);
        }

        [Fact]
        public void AllAuthoredSignals_ResolveToCanonicalExpeditionDestinations()
        {
            var primarySys = new RadioDistressSystem();
            string primaryPath = Path.Combine(DataDirectory, "radio_distress_signals.json");
            if (File.Exists(primaryPath))
            {
                primarySys.LoadFromJson(File.ReadAllText(primaryPath));
            }

            var expSys = new RadioDistressSystem();
            string expPath = Path.Combine(DataDirectory, "radio_distress_signals_expansion.json");
            if (File.Exists(expPath))
            {
                expSys.LoadFromJson(File.ReadAllText(expPath));
            }

            var allSignals = new List<DistressSignalDefinition>();
            allSignals.AddRange(primarySys.Definitions);
            allSignals.AddRange(expSys.Definitions);

            Assert.True(allSignals.Count >= 25, $"Expected at least 25 signals, found {allSignals.Count}");

            foreach (var sig in allSignals)
            {
                var res = _resolver.ResolveSignal(sig);
                Assert.True(res.IsValid, $"Signal {sig.FrequencyId} failed to resolve to valid destination: {res.ErrorCode}");
                Assert.True(_resolver.IsCanonicalDestination(res.DestinationId),
                    $"Signal {sig.FrequencyId} resolved to unknown destination ID: {res.DestinationId}");
                Assert.InRange(res.DistanceTicks, 1, 30);
                Assert.InRange(res.DangerLevel, 1, 10);
            }
        }
    }
}
