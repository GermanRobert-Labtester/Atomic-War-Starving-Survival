using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class FactionWarDialogueExpansionTests : CatalogTestBase
    {
        private static FactionWarContentCatalog LoadCatalog()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new FactionWarContentCatalogLoader(files, json);
            return loader.Load(DataDirectory);
        }

        [Fact]
        public void Catalog_Loads_Exactly_40_Snippets()
        {
            var catalog = LoadCatalog();
            Assert.Equal(40, catalog.DialogueSnippetCount);
            Assert.Equal(40, catalog.DialogueSnippets.Count);
        }

        [Fact]
        public void All_Dialogue_IDs_Are_Unique()
        {
            var catalog = LoadCatalog();
            var ids = catalog.DialogueSnippets.Select(s => s.id).ToList();
            var distinctIds = ids.Distinct(StringComparer.Ordinal).ToList();
            Assert.Equal(ids.Count, distinctIds.Count);
        }

        [Fact]
        public void All_Dialogue_IDs_Have_Dlg_Prefix()
        {
            var catalog = LoadCatalog();
            foreach (var snippet in catalog.DialogueSnippets)
            {
                Assert.True(snippet.id.StartsWith("dlg_"), $"Snippet {snippet.id} must start with dlg_ prefix");
            }
        }

        [Fact]
        public void All_18_Baseline_Snippets_Preserved_With_Original_Keys_And_Bodies()
        {
            var catalog = LoadCatalog();
            var baselineIds = new[]
            {
                "dlg_d482_checkpoint_quartermasters",
                "dlg_d483_exchange_lean_pool",
                "dlg_d488_understory_relay_move",
                "dlg_d490_switchback_pilgrims",
                "dlg_d493_weighbridge_toll_grumble",
                "dlg_d497_scavengers_clean_crater",
                "dlg_d505_conscription_office_clerks",
                "dlg_d512_weighbridge_reroute",
                "dlg_d526_exchange_roster_kid",
                "dlg_d538_checkpoint_awkward_small_talk",
                "dlg_d552_deserter_hunters",
                "dlg_d549_children_after_the_plaza",
                "dlg_d580_shrine_keepers_doubt",
                "dlg_d568_toll_syndicate_cynicism",
                "dlg_d571_forward_roster_checkpoint",
                "dlg_d573_forward_roster_identity",
                "dlg_d584_d9_cell_debate",
                "dlg_d591_switchback_waystation_doubt"
            };

            var byId = catalog.DialogueSnippets.ToDictionary(s => s.id, StringComparer.Ordinal);
            foreach (var bId in baselineIds)
            {
                Assert.True(byId.ContainsKey(bId), $"Baseline id {bId} must be preserved");
                Assert.False(string.IsNullOrWhiteSpace(byId[bId].body), $"Body for {bId} must be non-empty");
            }
        }

        [Fact]
        public void All_22_New_Snippets_Present()
        {
            var catalog = LoadCatalog();
            var newIds = new[]
            {
                // Garrison (5)
                "dlg_d486_garrison_crate_seal",
                "dlg_d494_garrison_boot_leather",
                "dlg_d516_garrison_kerosene_stove",
                "dlg_d542_garrison_sick_list_billet",
                "dlg_d562_garrison_fuel_drum_tare",
                // Exchange (4)
                "dlg_d485_exchange_wet_grain_scale",
                "dlg_d508_exchange_axle_grease_delay",
                "dlg_d530_exchange_stamped_chits",
                "dlg_d489_exchange_drum_bung_dispute",
                // Understory (4)
                "dlg_d492_understory_porcelain_insulator",
                "dlg_d518_understory_log_overrun",
                "dlg_d546_understory_smudged_pad_entry",
                "dlg_d576_understory_copper_splice_tale",
                // Independent (3)
                "dlg_d498_independent_chalk_boundary",
                "dlg_d534_independent_tripwire_slack",
                "dlg_d566_independent_blanket_tally",
                // Foundry (3)
                "dlg_d502_foundry_cracked_flask_sand",
                "dlg_d528_foundry_crucible_heat_window",
                "dlg_d556_foundry_slag_billet_reject",
                // Civilian (3)
                "dlg_d487_civilian_parsnip_stew_scrap",
                "dlg_d520_civilian_valve_handle_toy",
                "dlg_d574_civilian_kettle_scouring_mutter"
            };

            var byId = catalog.DialogueSnippets.ToDictionary(s => s.id, StringComparer.Ordinal);
            foreach (var nId in newIds)
            {
                Assert.True(byId.ContainsKey(nId), $"New snippet id {nId} must be present");
                Assert.False(string.IsNullOrWhiteSpace(byId[nId].body), $"Body for {nId} must be non-empty");
                Assert.False(string.IsNullOrWhiteSpace(byId[nId].speakerTag), $"Speaker tag for {nId} must be non-empty");
            }
        }

        [Fact]
        public void All_Snippets_Have_NonEmpty_Fields_And_Valid_MinDay()
        {
            var catalog = LoadCatalog();
            foreach (var s in catalog.DialogueSnippets)
            {
                Assert.False(string.IsNullOrWhiteSpace(s.id), "Snippet id must not be empty");
                Assert.False(string.IsNullOrWhiteSpace(s.locationId), $"locationId for {s.id} must not be empty");
                Assert.True(s.locationId.StartsWith("loc_"), $"locationId for {s.id} must start with loc_");
                Assert.False(string.IsNullOrWhiteSpace(s.speakerTag), $"speakerTag for {s.id} must not be empty");
                Assert.False(string.IsNullOrWhiteSpace(s.body), $"body for {s.id} must not be empty");
                Assert.True(s.minDay >= 480 && s.minDay <= 605, $"minDay {s.minDay} for {s.id} must be in Faction War range [480, 605]");
            }
        }

        [Fact]
        public void GetDialogueForLocation_Filters_Correctly_At_Day_Boundaries()
        {
            var catalog = LoadCatalog();
            // Test with a new snippet: dlg_d502_foundry_cracked_flask_sand at loc_granite_arsenal_foundry, minDay 502
            const string loc = "loc_granite_arsenal_foundry";
            const int onsetDay = 502;

            var before = catalog.GetDialogueForLocation(loc, onsetDay - 1);
            Assert.DoesNotContain(before, s => s.id == "dlg_d502_foundry_cracked_flask_sand");

            var at = catalog.GetDialogueForLocation(loc, onsetDay);
            Assert.Contains(at, s => s.id == "dlg_d502_foundry_cracked_flask_sand");

            var after = catalog.GetDialogueForLocation(loc, onsetDay + 50);
            Assert.Contains(after, s => s.id == "dlg_d502_foundry_cracked_flask_sand");
        }

        [Fact]
        public void GetDialogueForLocation_Wrong_Location_Returns_No_Snippets_For_That_Location()
        {
            var catalog = LoadCatalog();
            var list = catalog.GetDialogueForLocation("loc_nonexistent_location_xyz", 600);
            Assert.Empty(list);
        }

        [Fact]
        public void Faction_Context_Distribution_Satisfied()
        {
            var catalog = LoadCatalog();
            var baselineIds = new HashSet<string>
            {
                "dlg_d482_checkpoint_quartermasters", "dlg_d483_exchange_lean_pool",
                "dlg_d488_understory_relay_move", "dlg_d490_switchback_pilgrims",
                "dlg_d493_weighbridge_toll_grumble", "dlg_d497_scavengers_clean_crater",
                "dlg_d505_conscription_office_clerks", "dlg_d512_weighbridge_reroute",
                "dlg_d526_exchange_roster_kid", "dlg_d538_checkpoint_awkward_small_talk",
                "dlg_d552_deserter_hunters", "dlg_d549_children_after_the_plaza",
                "dlg_d580_shrine_keepers_doubt", "dlg_d568_toll_syndicate_cynicism",
                "dlg_d571_forward_roster_checkpoint", "dlg_d573_forward_roster_identity",
                "dlg_d584_d9_cell_debate", "dlg_d591_switchback_waystation_doubt"
            };

            var newSnippets = catalog.DialogueSnippets.Where(s => !baselineIds.Contains(s.id)).ToList();
            Assert.Equal(22, newSnippets.Count);

            var newGarrison = newSnippets.Count(s => s.id.Contains("_garrison_"));
            var newExchange = newSnippets.Count(s => s.id.Contains("_exchange_"));
            var newUnderstory = newSnippets.Count(s => s.id.Contains("_understory_"));
            var newIndependent = newSnippets.Count(s => s.id.Contains("_independent_"));
            var newFoundry = newSnippets.Count(s => s.id.Contains("_foundry_"));
            var newCivilian = newSnippets.Count(s => s.id.Contains("_civilian_"));

            Assert.Equal(5, newGarrison);
            Assert.Equal(4, newExchange);
            Assert.Equal(4, newUnderstory);
            Assert.Equal(3, newIndependent);
            Assert.Equal(3, newFoundry);
            Assert.Equal(3, newCivilian);
        }
    }
}
