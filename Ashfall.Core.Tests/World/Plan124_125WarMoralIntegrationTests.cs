// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.MoralChoice;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public sealed class Plan124_125WarMoralIntegrationTests
    {
        private static string DataDirectory
        {
            get
            {
                string start = Directory.GetCurrentDirectory();
                if (CatalogLocator.TryFindDataDirectory(start, out string found))
                    return found;
                if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                    return found;
                throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
            }
        }

        private static readonly string[] s_plan124ExpansionOverrideIds = new[]
        {
            "loc_override_checkpoint_occupied",
            "loc_override_granary_burned",
            "loc_override_well_contaminated",
            "loc_override_rail_yard_fortified",
            "loc_override_village_abandoned",
            "loc_override_factory_occupied",
            "loc_override_bridge_destroyed",
            "loc_override_roadblock_liberated",
            "loc_override_camp_overrun",
            "loc_override_station_reclaimed",
            "loc_override_field_scorched"
        };

        private static readonly string[] s_plan125ExpansionFlagIds = new[]
        {
            "flag_spared_raider",
            "flag_executed_prisoner",
            "flag_shared_rations",
            "flag_hoarded_medicine",
            "flag_sheltered_refugee",
            "flag_expelled_survivor",
            "flag_repaired_infrastructure",
            "flag_sabotaged_rival",
            "flag_broke_treaty",
            "flag_honored_debt",
            "flag_ignored_distress",
            "flag_responded_distress",
            "flag_forged_record",
            "flag_preserved_archive",
            "flag_chosen_faction_side"
        };

        [Fact]
        public void Plan124_FactionWarLocationOverrides_LoadsExactTwentyOverridesAndVerifiesExpansionTier()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new FactionWarContentCatalogLoader(files, json);
            var catalog = loader.Load(DataDirectory);

            Assert.NotNull(catalog);
            Assert.Equal(20, catalog.LocationOverrideCount);

            var overrideMap = catalog.LocationOverrides.ToDictionary(o => o.id, StringComparer.Ordinal);

            foreach (string expectedId in s_plan124ExpansionOverrideIds)
            {
                Assert.True(overrideMap.ContainsKey(expectedId), $"Faction war overrides missing Plan 124 override '{expectedId}'");
                var ov = overrideMap[expectedId];

                Assert.NotNull(ov);
                Assert.False(string.IsNullOrWhiteSpace(ov.locationId), $"Override '{expectedId}' missing locationId");
                Assert.False(string.IsNullOrWhiteSpace(ov.displayName), $"Override '{expectedId}' missing displayName");
                Assert.False(string.IsNullOrWhiteSpace(ov.description), $"Override '{expectedId}' missing description");
                Assert.False(string.IsNullOrWhiteSpace(ov.overrideType), $"Override '{expectedId}' missing overrideType");

                // Day window checks
                Assert.True(ov.activeFromDay > 0, $"Override '{expectedId}' activeFromDay must be positive");
                if (ov.activeUntilDay > 0)
                {
                    Assert.True(ov.activeUntilDay > ov.activeFromDay, $"Override '{expectedId}' activeUntilDay must be greater than activeFromDay");
                }
            }
        }

        [Fact]
        public void Plan125_MoralChoiceFlags_LoadsExactTwentyFiveFlagsAndValidatesAllExpansionFlags()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var definitions = MoralChoiceFlagCatalogLoader.Load(DataDirectory, files, json);

            Assert.NotNull(definitions);
            Assert.Equal(25, definitions.Flags.Count);
            Assert.Equal(25, definitions.Flags.Select(f => f.Id).Distinct(StringComparer.Ordinal).Count());

            var flagMap = definitions.Flags.ToDictionary(f => f.Id, StringComparer.Ordinal);

            foreach (string expectedFlag in s_plan125ExpansionFlagIds)
            {
                Assert.True(flagMap.ContainsKey(expectedFlag), $"Moral choice flag definitions missing Plan 125 flag '{expectedFlag}'");
                var def = flagMap[expectedFlag];

                Assert.NotNull(def);
                Assert.StartsWith("flag_", def.Id, StringComparison.Ordinal);
                Assert.False(string.IsNullOrWhiteSpace(def.DisplayName), $"Flag '{expectedFlag}' has empty display name");
            }

            // Verify compatibility with static definitions in MoralChoiceIds
            var staticFlags = MoralChoiceIds.AllFlags.ToHashSet(StringComparer.Ordinal);
            foreach (string expectedFlag in s_plan125ExpansionFlagIds)
            {
                Assert.Contains(expectedFlag, staticFlags);
            }
        }

        [Fact]
        public void Plan124_125_CrossDomainCoherence_WarDevastationAndMoralReactivityLinkages()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var warCatalog = new FactionWarContentCatalogLoader(files, json).Load(DataDirectory);
            var flagDefs = MoralChoiceFlagCatalogLoader.Load(DataDirectory, files, json);

            // Faction war location overrides introduce tangible crises:
            var granaryBurned = warCatalog.LocationOverrides.FirstOrDefault(o => o.id == "loc_override_granary_burned");
            Assert.NotNull(granaryBurned);
            Assert.Equal("loc_crossing_granary_pledge", granaryBurned!.locationId);

            var wellContaminated = warCatalog.LocationOverrides.FirstOrDefault(o => o.id == "loc_override_well_contaminated");
            Assert.NotNull(wellContaminated);
            Assert.Equal("location_municipal_water_reservoir", wellContaminated!.locationId);

            var campOverrun = warCatalog.LocationOverrides.FirstOrDefault(o => o.id == "loc_override_camp_overrun");
            Assert.NotNull(campOverrun);
            Assert.Equal("loc_crossing_petition_tent", campOverrun!.locationId);

            // Moral choice flags allow player responses to wasteland crises:
            Assert.Contains(flagDefs.Flags, f => f.Id == "flag_shared_rations");
            Assert.Contains(flagDefs.Flags, f => f.Id == "flag_sheltered_refugee");
            Assert.Contains(flagDefs.Flags, f => f.Id == "flag_hoarded_medicine");
            Assert.Contains(flagDefs.Flags, f => f.Id == "flag_sabotaged_rival");
            Assert.Contains(flagDefs.Flags, f => f.Id == "flag_broke_treaty");

            // Rule 5: Ensure zero domain pollution between war content catalog and moral flags
            Assert.All(warCatalog.LocationOverrides, o => Assert.StartsWith("loc_override_", o.id));
            Assert.All(flagDefs.Flags, f => Assert.StartsWith("flag_", f.Id));
        }

        [Fact]
        public void Plan124_125_DeterministicDayProgressionAndFlagResolution()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var warCatalog = new FactionWarContentCatalogLoader(files, json).Load(DataDirectory);

            // Test active override selection at specific campaign milestones
            int day230 = 230; // Day 230 falls in granary burned window (220-260) and checkpoint occupied window (200-250)
            var activeAt230 = warCatalog.LocationOverrides
                .Where(o => o.activeFromDay <= day230 && (o.activeUntilDay == 0 || o.activeUntilDay >= day230))
                .Select(o => o.id)
                .ToList();

            Assert.Contains("loc_override_granary_burned", activeAt230);
            Assert.Contains("loc_override_checkpoint_occupied", activeAt230);
            Assert.DoesNotContain("loc_override_field_scorched", activeAt230); // starts day 360

            int day370 = 370; // Day 370 falls in field scorched (360-400) and roadblock liberated (330-370)
            var activeAt370 = warCatalog.LocationOverrides
                .Where(o => o.activeFromDay <= day370 && (o.activeUntilDay == 0 || o.activeUntilDay >= day370))
                .Select(o => o.id)
                .ToList();

            Assert.Contains("loc_override_field_scorched", activeAt370);
            Assert.Contains("loc_override_roadblock_liberated", activeAt370);
            Assert.DoesNotContain("loc_override_granary_burned", activeAt370);

            // Test deterministic flag resolution in MoralChoiceSystem
            var rng = new SeededRng(124125);
            var moralSystem = new MoralChoiceSystem(rng);

            Assert.False(moralSystem.HasFlag("flag_sheltered_refugee"));
            Assert.False(moralSystem.HasFlag("flag_spared_raider"));

            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_refugee_crossroads",
                Choices = new List<MoralChoiceOption>
                {
                    new MoralChoiceOption { Label = "Shelter Refugee", SetFlag = "flag_sheltered_refugee" },
                    new MoralChoiceOption { Label = "Turn Away", SetFlag = "flag_expelled_survivor" }
                }
            };
            moralSystem.RegisterQuest(quest);

            // Resolve choice 0 deterministically
            moralSystem.Resolve(quest, 0, "shelter_bunk", day230);

            Assert.True(moralSystem.HasFlag("flag_sheltered_refugee"));
            Assert.False(moralSystem.HasFlag("flag_expelled_survivor"));
        }
    }
}
