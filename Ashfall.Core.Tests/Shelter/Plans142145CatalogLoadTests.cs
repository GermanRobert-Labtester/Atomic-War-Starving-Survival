// SPDX-License-Identifier: MIT
// Wave 1 (Plans 142–145): catalog load contracts against the data authority.
// Proves the four new catalogs parse through their hardened loaders, carry
// non-empty definition content, and that every item_/room_ reference they
// author resolves against items.json / shelter_rooms.json (mirroring the
// CatalogIntegrityValidator Tier-1 rule locally).

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class Plans142145CatalogLoadTests : CatalogTestBase
    {
        private static string ReadCatalog(string fileName)
        {
            string path = Path.Combine(DataDirectory, fileName);
            Assert.True(File.Exists(path), $"Missing data-authority catalog: {path}");
            return File.ReadAllText(path);
        }

        // ── Plan 142 — cellulosic biofuel ───────────────────────────

        [Fact]
        public void CellulosicBiofuelCatalog_LoadsFromDataAuthority()
        {
            var catalog = CellulosicBiofuelCatalog.FromJson(
                ReadCatalog("cellulosic_ethanol_catalog.json"), "cellulosic_ethanol_catalog.json");

            Assert.NotNull(catalog);
            Assert.Equal(1, catalog!.schema_version);
            Assert.False(string.IsNullOrWhiteSpace(catalog.machine.machine_id));
            Assert.Equal("room_generator", catalog.machine.room_id);
            Assert.NotEmpty(catalog.machine.construction_required_items);
            Assert.NotEmpty(catalog.processes);

            var process = Assert.Single(catalog.processes);
            Assert.Equal("biofuel_cellulose_standard", process.process_id);
            Assert.NotEmpty(process.accepted_item_ids);
            Assert.True(process.fermentation_ticks > 0);
            Assert.True(process.refining_ticks > 0);
            Assert.True(process.energy_cost_kwh_per_day > 0);
            Assert.False(string.IsNullOrWhiteSpace(process.fuel_output_id));

            Assert.Equal(3, catalog.fuel_grades.Count);
            foreach (var grade in catalog.fuel_grades)
            {
                Assert.False(string.IsNullOrWhiteSpace(grade.grade_id));
                Assert.False(string.IsNullOrWhiteSpace(grade.output_item_id));
                Assert.True(grade.vehicle_wear_multiplier >= 1f, "worse fuel must never reduce vehicle wear");
            }
            Assert.NotNull(catalog.FindProcess("biofuel_cellulose_standard"));
            Assert.Null(catalog.FindProcess("biofuel_does_not_exist"));
        }

        // ── Plan 143 — radar ECM ────────────────────────────────────

        [Fact]
        public void RadarEcmCatalog_LoadsFromDataAuthority()
        {
            var catalog = RadarEcmCatalog.FromJson(
                ReadCatalog("radar_ecm_catalog.json"), "radar_ecm_catalog.json");

            Assert.NotNull(catalog);
            Assert.Equal(1, catalog!.schema_version);
            Assert.NotEmpty(catalog.ecm_profiles);

            var profile = Assert.Single(catalog.ecm_profiles);
            Assert.Equal("ecm_vehicle_deception_mk1", profile.profile_id);
            Assert.Equal(4, profile.modes.Count);
            Assert.False(string.IsNullOrWhiteSpace(profile.install_item_id));

            // Anti-immunity invariants are part of the data contract.
            Assert.True(catalog.invariants.no_immunity);
            Assert.True(catalog.invariants.single_mode_lock_break_cap_permille > 0);
            foreach (var mode in profile.modes)
            {
                var rule = catalog.FindModeRule(mode);
                Assert.NotNull(rule);
                Assert.True(rule!.lock_break_bonus_permille <= catalog.invariants.single_mode_lock_break_cap_permille,
                    $"mode '{mode}' lock-break bonus exceeds the single-mode anti-immunity cap");
            }

            Assert.NotEmpty(catalog.threat_profiles);
            Assert.NotNull(catalog.FindProfile("ecm_vehicle_deception_mk1"));
            Assert.Null(catalog.FindProfile("ecm_does_not_exist"));
        }

        // ── Plan 144 — precision broaching ──────────────────────────

        [Fact]
        public void PrecisionBroachingCatalog_LoadsFromDataAuthority()
        {
            var catalog = PrecisionBroachingCatalog.FromJson(
                ReadCatalog("precision_broaching_catalog.json"), "precision_broaching_catalog.json");

            Assert.NotNull(catalog);
            Assert.Equal(1, catalog!.schema_version);
            Assert.False(string.IsNullOrWhiteSpace(catalog.bench.bench_id));
            Assert.Equal("room_workshop", catalog.bench.room_id);
            Assert.Equal(2, catalog.operations.Count);
            Assert.Equal(3, catalog.quality_tiers.Count);

            foreach (var op in catalog.operations)
            {
                Assert.False(string.IsNullOrWhiteSpace(op.input_item_id));
                Assert.False(string.IsNullOrWhiteSpace(op.output_item_id));
                Assert.False(string.IsNullOrWhiteSpace(op.tool_class));
                Assert.True(op.labor_ticks > 0);
                Assert.True(op.tool_wear_per_job > 0);
                Assert.True(op.machine_load_permille > 0 && op.machine_load_permille <= 1000);
                Assert.NotEqual(op.input_item_id, op.output_item_id);
            }

            foreach (var tier in catalog.quality_tiers)
                Assert.InRange(tier.quality_value, 0f, 1f);

            Assert.Equal(5, catalog.failure_states.Count);
            Assert.NotNull(catalog.FindOperation("broach_internal_spline_medium"));
            Assert.Null(catalog.FindOperation("broach_does_not_exist"));
        }

        // ── Plan 145 — fog harvesting ───────────────────────────────

        [Fact]
        public void FogHarvestingCatalog_LoadsFromDataAuthority()
        {
            var catalog = FogHarvestingCatalog.FromJson(
                ReadCatalog("fog_harvesting_catalog.json"), "fog_harvesting_catalog.json");

            Assert.NotNull(catalog);
            Assert.Equal(1, catalog!.schema_version);
            Assert.Equal(2, catalog.harvester_profiles.Count);

            foreach (var profile in catalog.harvester_profiles)
            {
                Assert.False(string.IsNullOrWhiteSpace(profile.profile_id));
                Assert.True(profile.base_yield_units > 0);
                Assert.True(profile.wear_rate_per_day > 0);
                Assert.Equal("raw", profile.output_water_type);
                Assert.NotEmpty(profile.siting_tags);

                if (profile.passive)
                    Assert.Equal(0f, profile.power_draw_kwh_per_day);
                else
                    Assert.True(profile.power_draw_kwh_per_day > 0);
            }

            // Band math sanity: multipliers bounded, presence in [0,1].
            foreach (var presence in catalog.fog_bands.presence_by_weather_kind.Values)
                Assert.InRange(presence, 0f, 1f);
            foreach (var mult in catalog.fog_bands.wind_band_multipliers.Values.Concat(
                     catalog.fog_bands.humidity_band_multipliers.Values))
                Assert.InRange(mult, 0f, 2f);
            Assert.True(catalog.fog_bands.powered_assist_multiplier > 1f,
                "powered assist must strictly improve collection over passive");

            Assert.True(catalog.buffer.max_units > 0);
            Assert.True(catalog.storm_event.damage_condition_permille > 0);
            Assert.NotNull(catalog.FindProfile("fog_net_ridge_standard"));
            Assert.Null(catalog.FindProfile("fog_does_not_exist"));
        }

        // ── Cross-catalog reference resolution (local Tier-1 mirror) ──

        public static IEnumerable<object[]> ItemReferenceRows()
        {
            yield return new object[] { "cellulosic_ethanol_catalog.json", new[]
            {
                "item_crop_waste", "item_biofuel_low_grade", "item_biofuel_generator_grade",
                "item_biofuel_high_grade", "item_separation_media_cartridge"
            } };
            yield return new object[] { "radar_ecm_catalog.json", new[] { "item_ecm_jammer_module" } };
            yield return new object[] { "precision_broaching_catalog.json", new[]
            {
                "item_hydraulic_ram_assembly", "item_cutting_fluid_canister",
                "item_machined_blank_medium", "item_internal_spline_hub",
                "item_machined_blank_small", "item_keyed_actuator_collar"
            } };
            yield return new object[] { "fog_harvesting_catalog.json", new[]
            {
                "item_fog_mesh_roll", "item_reinforced_support_cable",
                "item_powered_mist_assist_module", "item_internal_spline_hub"
            } };
        }

        [Theory]
        [MemberData(nameof(ItemReferenceRows))]
        public void ItemReferences_ResolveAgainstItemsJson(string catalogFile, string[] itemIds)
        {
            Assert.True(File.Exists(Path.Combine(DataDirectory, catalogFile)), $"missing catalog {catalogFile}");
            string itemsJson = ReadCatalog("items.json");
            foreach (string itemId in itemIds)
                Assert.True(itemsJson.Contains("\"id\": \"" + itemId + "\"", StringComparison.Ordinal),
                    $"{catalogFile} references '{itemId}' which is not authored in items.json");
        }

        [Fact]
        public void RoomReferences_ResolveAgainstShelterRooms()
        {
            string roomsJson = ReadCatalog("shelter_rooms.json");
            Assert.Contains("\"id\": \"room_generator\"", roomsJson);
            Assert.Contains("\"id\": \"room_workshop\"", roomsJson);
        }

        [Fact]
        public void Catalogs_ParseIndependently_AndAreIdempotent()
        {
            // Parse each catalog twice from the same bytes — object state must
            // never leak between parses (loaders are pure functions).
            string cellulosicJson = ReadCatalog("cellulosic_ethanol_catalog.json");
            string ecmJson = ReadCatalog("radar_ecm_catalog.json");
            string broachJson = ReadCatalog("precision_broaching_catalog.json");
            string fogJson = ReadCatalog("fog_harvesting_catalog.json");

            Assert.NotNull(CellulosicBiofuelCatalog.FromJson(cellulosicJson, "cellulosic_ethanol_catalog.json"));
            Assert.NotNull(CellulosicBiofuelCatalog.FromJson(cellulosicJson, "cellulosic_ethanol_catalog.json"));
            Assert.NotNull(RadarEcmCatalog.FromJson(ecmJson, "radar_ecm_catalog.json"));
            Assert.NotNull(RadarEcmCatalog.FromJson(ecmJson, "radar_ecm_catalog.json"));
            Assert.NotNull(PrecisionBroachingCatalog.FromJson(broachJson, "precision_broaching_catalog.json"));
            Assert.NotNull(PrecisionBroachingCatalog.FromJson(broachJson, "precision_broaching_catalog.json"));
            Assert.NotNull(FogHarvestingCatalog.FromJson(fogJson, "fog_harvesting_catalog.json"));
            Assert.NotNull(FogHarvestingCatalog.FromJson(fogJson, "fog_harvesting_catalog.json"));
        }
    }
}
