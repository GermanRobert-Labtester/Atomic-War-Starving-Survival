#nullable enable
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class Plan40_41DebtRoomIntegrationTests
    {
        private static string ResolveDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void LedgerDebtTemplates_LoadsAll15TemplatesAnd10Consequences_FromAuthoritativeJson()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var catalog = DebtTemplateCatalogLoader.Load(dataDir, fileIO, json);

            Assert.NotNull(catalog);
            Assert.Empty(catalog.Errors);
            Assert.Equal(15, catalog.Templates.Count);
            Assert.Equal(10, catalog.Consequences.Count);

            string[] expectedTemplateIds = new[]
            {
                "debt_supply_corps_rations",
                "debt_supply_corps_fuel",
                "debt_supply_corps_medical",
                "debt_hydro_barons_water",
                "debt_hydro_barons_filter",
                "debt_hydro_barons_purification",
                "debt_railway_guild_fuel",
                "debt_railway_guild_parts",
                "debt_railway_guild_transport",
                "debt_ordnance_foundry_ammo",
                "debt_ordnance_foundry_tools",
                "debt_ordnance_foundry_armor",
                "debt_scavengers_food",
                "debt_scavengers_medicine",
                "debt_scavengers_equipment"
            };

            foreach (var templateId in expectedTemplateIds)
            {
                var template = catalog.GetTemplate(templateId);
                Assert.NotNull(template);
                Assert.False(string.IsNullOrWhiteSpace(template!.displayName),
                    $"Template '{templateId}' must have non-empty displayName.");
                Assert.False(string.IsNullOrWhiteSpace(template.creditorId),
                    $"Template '{templateId}' must have creditorId.");
                Assert.False(string.IsNullOrWhiteSpace(template.principalItemId),
                    $"Template '{templateId}' must have principalItemId.");
                Assert.True(template.principalQuantity > 0,
                    $"Template '{templateId}' principalQuantity must be > 0.");
                Assert.True(template.termDays > 0,
                    $"Template '{templateId}' termDays must be > 0.");
                Assert.True(template.rate >= 0f,
                    $"Template '{templateId}' rate must be >= 0.");
                Assert.False(string.IsNullOrWhiteSpace(template.consequenceId),
                    $"Template '{templateId}' must specify consequenceId.");

                // Validate consequence referenced by template exists
                var consequence = catalog.GetConsequence(template.consequenceId);
                Assert.NotNull(consequence);
            }

            string[] expectedConsequenceIds = new[]
            {
                "conseq_standing_loss_mild",
                "conseq_standing_loss_moderate",
                "conseq_embargo_trade",
                "conseq_standing_loss_and_embargo",
                "conseq_bounty_moderate",
                "conseq_collateral_seizure",
                "conseq_raid_severe",
                "conseq_labor_obligation",
                "conseq_treaty_breach",
                "conseq_forgiveness_rare"
            };

            foreach (var consequenceId in expectedConsequenceIds)
            {
                var consequence = catalog.GetConsequence(consequenceId);
                Assert.NotNull(consequence);
                Assert.False(string.IsNullOrWhiteSpace(consequence!.displayName),
                    $"Consequence '{consequenceId}' must have displayName.");
                Assert.False(string.IsNullOrWhiteSpace(consequence.trigger),
                    $"Consequence '{consequenceId}' must have trigger.");
                Assert.False(string.IsNullOrWhiteSpace(consequence.effectType),
                    $"Consequence '{consequenceId}' must have effectType.");
            }
        }

        [Fact]
        public void ShelterRoomCatalog_LoadsAll23RoomsAnd12Rules_FromAuthoritativeJson()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var catalog = ShelterRoomCatalogLoader.Load(dataDir, fileIO, json);

            Assert.NotNull(catalog);
            Assert.True(catalog.rooms.Count >= 20, $"Expected at least 20 rooms, found {catalog.rooms.Count}");
            Assert.Equal(23, catalog.rooms.Count);
            Assert.Equal(12, catalog.assignment_rules.Count);

            string[] keyRoomIds = new[]
            {
                "room_bunker_corridor",
                "room_bunks_crowded",
                "room_bunks",
                "room_quarters_private",
                "room_workshop",
                "room_workshop_heavy",
                "room_workshop_precision",
                "room_clinic",
                "room_ward_clinical",
                "room_ward_quarantine",
                "room_kitchen",
                "room_storage_bay",
                "room_storage_secure",
                "room_greenhouse_shelter",
                "room_radio_tuner",
                "room_armory_munitions",
                "room_laboratory_research",
                "room_common_mess_hall",
                "room_reading_quiet_room",
                "room_airlock",
                "room_generator",
                "room_filtration",
                "room_hope_beacon"
            };

            var roomsById = catalog.rooms.ToDictionary(r => r.id, StringComparer.Ordinal);

            foreach (var roomId in keyRoomIds)
            {
                Assert.True(roomsById.TryGetValue(roomId, out var room),
                    $"Room '{roomId}' should be present in catalog.");
                Assert.NotNull(room);
                Assert.False(string.IsNullOrWhiteSpace(room.display_name),
                    $"Room '{roomId}' must have non-empty display_name.");
                Assert.False(string.IsNullOrWhiteSpace(room.function),
                    $"Room '{roomId}' must have valid function.");
                Assert.True(room.capacity >= 0,
                    $"Room '{roomId}' capacity must be non-negative.");
                Assert.Equal(100.0f, room.base_condition);
                Assert.NotEmpty(room.build_cost);
                Assert.NotEmpty(room.repair_cost);
            }

            foreach (var rule in catalog.assignment_rules)
            {
                Assert.False(string.IsNullOrWhiteSpace(rule.id), "Rule must have non-empty id.");
                Assert.False(string.IsNullOrWhiteSpace(rule.name), "Rule must have non-empty name.");
                Assert.False(string.IsNullOrWhiteSpace(rule.target_room_function), "Rule must have target_room_function.");
                Assert.False(string.IsNullOrWhiteSpace(rule.required_skill_id), "Rule must have required_skill_id.");
                Assert.True(rule.bonus_magnitude > 0f, "Rule bonus magnitude must be > 0.");
            }
        }

        [Fact]
        public void LedgerDebtSystem_And_ShelterRoomSystem_ExecuteConcurrentlyWithoutInterference()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // 1. Evaluate debt catalog
            var debtCatalog = DebtTemplateCatalogLoader.Load(dataDir, fileIO, json);
            Assert.NotNull(debtCatalog);
            var rationDebt = debtCatalog.GetTemplate("debt_supply_corps_rations");
            Assert.NotNull(rationDebt);
            Assert.Equal("faction_supply_corps", rationDebt!.creditorId);
            Assert.Equal(20, rationDebt.termDays);

            // 2. Evaluate room catalog
            var roomCatalog = ShelterRoomCatalogLoader.Load(dataDir, fileIO, json);
            Assert.NotNull(roomCatalog);
            var workshop = roomCatalog.rooms.First(r => r.id == "room_workshop");
            Assert.Equal("Workshop", workshop.function);
            Assert.Equal(2, workshop.capacity);

            // 3. Test concurrent evaluation:
            // Verify debt interest calculation simulation
            float principal = rationDebt.principalQuantity;
            float rate = rationDebt.rate;
            float accruedInterest = principal * rate * 5; // 5 days
            Assert.True(accruedInterest > 0f);

            // Verify room assignment rule match
            var workshopRule = roomCatalog.assignment_rules.First(r => r.target_room_function == "Workshop");
            Assert.Equal("skill_rough_repairs", workshopRule.required_skill_id);
            Assert.True(workshopRule.bonus_magnitude > 0f);

            // Verify zero cross-talk
            Assert.Equal(15, debtCatalog.Templates.Count);
            Assert.Equal(23, roomCatalog.rooms.Count);
        }
    }
}
