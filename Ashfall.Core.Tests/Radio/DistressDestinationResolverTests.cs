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

        [Fact]
        public void DirectResolution_CanonicalId_ResolvesDirectly()
        {
            var cases = new[]
            {
                (DestinationId: "checkpoint_kilo_armory", Distance: 6, Danger: 4),
                (DestinationId: "loc_recovery_yard", Distance: 6, Danger: 6),
                (DestinationId: "rural_gas_station", Distance: 3, Danger: 3),
                (DestinationId: "suburban_house", Distance: 2, Danger: 2),
                (DestinationId: "loc_denial_cut_substation", Distance: 8, Danger: 4),
                (DestinationId: "location_silent_observatory", Distance: 14, Danger: 8)
            };
            var failures = new List<string>();

            foreach (var test in cases)
            {
                var res = _resolver.Resolve(test.DestinationId);
                if (!res.IsValid)
                    failures.Add($"{test.DestinationId}: expected valid resolution");
                if (res.DestinationId != test.DestinationId)
                    failures.Add($"{test.DestinationId}: resolved to {res.DestinationId}");
                if (res.ResolutionMode != "Direct")
                    failures.Add($"{test.DestinationId}: expected Direct mode, got {res.ResolutionMode}");
                if (res.DistanceTicks != test.Distance)
                    failures.Add($"{test.DestinationId}: expected distance {test.Distance}, got {res.DistanceTicks}");
                if (res.DangerLevel != test.Danger)
                    failures.Add($"{test.DestinationId}: expected danger {test.Danger}, got {res.DangerLevel}");
                if (res.ErrorCode != null)
                    failures.Add($"{test.DestinationId}: unexpected error {res.ErrorCode}");
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void AliasedResolution_NonCanonicalLocations_ResolveToCanonical()
        {
            var cases = new[]
            {
                (RawLocation: "raider_ambush_site", ExpectedCanonical: "collapsed_building"),
                (RawLocation: "loc_bridge_seven", ExpectedCanonical: "loc_weighbridge"),
                (RawLocation: "location_substation_omega", ExpectedCanonical: "electrical_substation")
            };
            var failures = new List<string>();

            foreach (var test in cases)
            {
                var res = _resolver.Resolve(test.RawLocation);
                if (!res.IsValid)
                    failures.Add($"{test.RawLocation}: expected valid resolution");
                if (res.DestinationId != test.ExpectedCanonical)
                    failures.Add($"{test.RawLocation}: expected {test.ExpectedCanonical}, got {res.DestinationId}");
                if (res.ResolutionMode != "Aliased")
                    failures.Add($"{test.RawLocation}: expected Aliased mode, got {res.ResolutionMode}");
                if (res.ErrorCode != null)
                    failures.Add($"{test.RawLocation}: unexpected error {res.ErrorCode}");
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void BuiltinFallbackAliases_All8ResolveToCanonical()
        {
            var cases = new[]
            {
                (RawLocation: "loc_checkpoint_kilo", ExpectedCanonical: "checkpoint_kilo_armory"),
                (RawLocation: "loc_bunker_4_east_trap", ExpectedCanonical: "collapsed_building"),
                (RawLocation: "loc_sector_9_substation", ExpectedCanonical: "electrical_substation"),
                (RawLocation: "loc_relay_44_bunker", ExpectedCanonical: "loc_weighbridge"),
                (RawLocation: "loc_marsh_caravan_wreck", ExpectedCanonical: "loc_water_station"),
                (RawLocation: "loc_meridian_cold_store", ExpectedCanonical: "loc_the_allotments"),
                (RawLocation: "loc_river_barge_olenka", ExpectedCanonical: "loc_lock_gate_four"),
                (RawLocation: "loc_field_medic_post", ExpectedCanonical: "prewar_medical_cache")
            };
            var failures = new List<string>();

            foreach (var test in cases)
            {
                var res = _resolver.Resolve(test.RawLocation);
                if (!res.IsValid)
                    failures.Add($"{test.RawLocation}: expected valid resolution");
                if (res.DestinationId != test.ExpectedCanonical)
                    failures.Add($"{test.RawLocation}: expected {test.ExpectedCanonical}, got {res.DestinationId}");
                if (res.ResolutionMode != "Aliased")
                    failures.Add($"{test.RawLocation}: expected Aliased mode, got {res.ResolutionMode}");
                if (res.ErrorCode != null)
                    failures.Add($"{test.RawLocation}: unexpected error {res.ErrorCode}");
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void ScavengingTableId_ResolvesToCanonicalDestination()
        {
            var cases = new[]
            {
                (TableId: "table_loot_farm", ExpectedDestination: "loc_the_allotments"),
                (TableId: "table_loot_power_substation", ExpectedDestination: "loc_denial_cut_substation"),
                (TableId: "table_loot_apartment_block", ExpectedDestination: "suburban_house"),
                (TableId: "table_loot_industrial_district", ExpectedDestination: "rural_gas_station"),
                (TableId: "table_loot_hospital", ExpectedDestination: "hospital_pharmacy")
            };
            var failures = new List<string>();

            foreach (var test in cases)
            {
                var res = _resolver.Resolve(test.TableId);
                if (!res.IsValid)
                    failures.Add($"{test.TableId}: expected valid resolution");
                if (res.DestinationId != test.ExpectedDestination)
                    failures.Add($"{test.TableId}: expected {test.ExpectedDestination}, got {res.DestinationId}");
                if (res.ResolutionMode != "AliasedTable")
                    failures.Add($"{test.TableId}: expected AliasedTable mode, got {res.ResolutionMode}");
                if (res.ErrorCode != null)
                    failures.Add($"{test.TableId}: unexpected error {res.ErrorCode}");
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void NullOrEmpty_ResolvesToFallbackWithErrorCode()
        {
            string?[] locations = { null, string.Empty, "   " };
            var failures = new List<string>();

            foreach (var location in locations)
            {
                var res = _resolver.Resolve(location);
                string label = location == null ? "<null>" : $"'{location}'";
                if (res.IsValid)
                    failures.Add($"{label}: expected invalid resolution");
                if (res.DestinationId != "collapsed_building")
                    failures.Add($"{label}: expected fallback collapsed_building, got {res.DestinationId}");
                if (res.ResolutionMode != "Fallback")
                    failures.Add($"{label}: expected Fallback mode, got {res.ResolutionMode}");
                if (res.ErrorCode != "EMPTY_LOCATION_ID")
                    failures.Add($"{label}: expected EMPTY_LOCATION_ID, got {res.ErrorCode}");
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
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
