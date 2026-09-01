// SPDX-License-Identifier: MIT
// Plan 12C - Shelter interior decor & memorial wall (White Space 3)
// regression coverage. All tests below are original C# test code that
// pins observable runtime behaviour of the ShelterDecorSystem, its save
// section registration, and the data authority that the host wires in.

using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using Ashfall.Core.UI;

namespace Ashfall.Core.Tests
{
    public class Plan12CDecorTests : CatalogTestBase
    {
        private static string DataDir => DataDirectory;

        // ---- Assign / Remove / GetSlot ----

        [Fact]
        public void DecorSystem_Assign_PersistsRoundTrip()
        {
            var sys = new ShelterDecorSystem();
            int events = 0;
            sys.OnDecorChanged += _ => events++;

            Assert.True(sys.Assign(
                roomId: "loc_lobby",
                slotId: "north_wall",
                itemId: "item_decor_poster_ration",
                dayInstalled: 30));
            Assert.Equal(1, events);

            var slot = sys.GetSlot("loc_lobby", "north_wall");
            Assert.NotNull(slot);
            Assert.Equal("item_decor_poster_ration", slot!.ItemId);
            Assert.Equal(30, slot.DayInstalled);
            Assert.False(slot.IsMemorialPlaque);
        }

        [Fact]
        public void DecorSystem_Assign_RejectsBadInput()
        {
            var sys = new ShelterDecorSystem();
            Assert.False(sys.Assign("", "x", "item_decor_poster_ration", 1));
            Assert.False(sys.Assign("loc_lobby", "", "item_decor_poster_ration", 1));
            Assert.Empty(sys.State.Placements);
        }

        [Fact]
        public void DecorSystem_MemorialAssign_RoundTripsPlaqueMetadata()
        {
            var sys = new ShelterDecorSystem();
            Assert.True(sys.Assign(
                roomId: "loc_memorial_wall",
                slotId: "peg_27",
                itemId: "item_decor_memorial_plaque_carving",
                dayInstalled: 412,
                isMemorialPlaque: true,
                memorialSurvivorId: "sv_eli_p",
                plaqueSourceHeirloomId: "item_personal_keepsake_eli_pewter_carving"));
            var slot = sys.GetSlot("loc_memorial_wall", "peg_27");
            Assert.NotNull(slot);
            Assert.True(slot!.IsMemorialPlaque);
            Assert.Equal("sv_eli_p", slot.MemorialSurvivorId);
            Assert.Equal("item_personal_keepsake_eli_pewter_carving", slot.PlaqueSourceHeirloomId);
        }

        [Fact]
        public void DecorSystem_AssignOverwritesPriorPlacementAtSameSlot()
        {
            var sys = new ShelterDecorSystem();
            sys.Assign("loc_lobby", "north_wall", "item_decor_poster_ration", 10);
            sys.Assign("loc_lobby", "north_wall", "item_decor_poster_warning", 14);
            var slots = sys.ListRoomPlacements("loc_lobby");
            Assert.Single(slots);
            Assert.Equal("item_decor_poster_warning", slots[0].ItemId);
            Assert.Equal(14, slots[0].DayInstalled);
        }

        [Fact]
        public void DecorSystem_Remove_DeletesSpecifiedSlotOnly()
        {
            var sys = new ShelterDecorSystem();
            sys.Assign("loc_lobby", "north_wall", "item_decor_poster_ration", 10);
            sys.Assign("loc_lobby", "south_pegs", "item_decor_poster_warning", 11);
            sys.Assign("loc_corridor", "main_panel", "item_decor_signal_log", 12);
            Assert.True(sys.Remove("loc_lobby", "north_wall"));
            Assert.Equal(2, sys.State.Placements.Count);
            Assert.Null(sys.GetSlot("loc_lobby", "north_wall"));
            Assert.NotNull(sys.GetSlot("loc_lobby", "south_pegs"));
            Assert.NotNull(sys.GetSlot("loc_corridor", "main_panel"));
            Assert.False(sys.Remove("loc_lobby", "north_wall"));
        }

        [Fact]
        public void DecorSystem_ListRoomPlacements_OrdinalSortsBySlotId()
        {
            var sys = new ShelterDecorSystem();
            sys.Assign("loc_lobby", "z_late_slot", "item_decor_signal_log", 1);
            sys.Assign("loc_lobby", "a_early_slot", "item_decor_poster_ration", 1);
            sys.Assign("loc_lobby", "m_middle_slot", "item_decor_pressed_flower", 1);
            var slots = sys.ListRoomPlacements("loc_lobby");
            Assert.Equal(3, slots.Count);
            Assert.Equal("a_early_slot", slots[0].SlotId);
            Assert.Equal("m_middle_slot", slots[1].SlotId);
            Assert.Equal("z_late_slot", slots[2].SlotId);
        }

        // ---- GetRoomMoraleDelta ----

        [Fact]
        public void DecorSystem_GetRoomMoraleDelta_SumsRegisteredModifiers()
        {
            var sys = new ShelterDecorSystem();
            sys.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = "item_decor_poster_ration",
                LocalizedMoraleDelta = 1.5f,
                Category = "poster"
            });
            sys.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = "item_decor_locomotive_nameplate",
                LocalizedMoraleDelta = 2.0f,
                Category = "trophy"
            });
            sys.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = "item_decor_chalk_drawing",
                LocalizedMoraleDelta = 1.0f,
                Category = "drawing"
            });
            sys.Assign("loc_lobby", "north_wall", "item_decor_poster_ration", 1);
            sys.Assign("loc_lobby", "south_panel", "item_decor_locomotive_nameplate", 1);
            sys.Assign("loc_lobby", "north_table", "item_decor_chalk_drawing", 1);
            float sum = sys.GetRoomMoraleDelta("loc_lobby");
            Assert.Equal(4.5f, sum, 3);
        }

        [Fact]
        public void DecorSystem_GetRoomMoraleDelta_EmptyRoomReturnsZero()
        {
            var sys = new ShelterDecorSystem();
            Assert.Equal(0f, sys.GetRoomMoraleDelta("loc_empty"));
        }

        [Fact]
        public void DecorSystem_GetRoomMoraleDelta_ItemWithoutModifierDoesNotCrash()
        {
            var sys = new ShelterDecorSystem();
            sys.Assign("loc_lobby", "east_wall", "item_decor_unregistered_thing", 1);
            Assert.Equal(0f, sys.GetRoomMoraleDelta("loc_lobby"));
        }

        // ---- Memorial plaque bridge ----

        [Fact]
        public void DecorSystem_ResolvePlaqueItemId_ReturnsCanonicalId()
        {
            var sys = new ShelterDecorSystem();
            sys.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = ShelterDecorSystem.MemorialPlaquePrefix + "_generic",
                LocalizedMoraleDelta = 1.6f
            });
            var resolved = sys.ResolvePlaqueItemId("item_personal_keepsake_eli_pewter_default");
            Assert.Equal(ShelterDecorSystem.MemorialPlaquePrefix + "_generic", resolved);
        }

        [Fact]
        public void DecorSystem_ResolvePlaqueItemId_FindsKindSpecific_WhenRegistered()
        {
            var sys = new ShelterDecorSystem();
            sys.RegisterItemModifier(new ShelterDecorItemModifier { ItemId = ShelterDecorSystem.MemorialPlaquePrefix + "_drawing", LocalizedMoraleDelta = 2.0f });
            sys.RegisterItemModifier(new ShelterDecorItemModifier { ItemId = ShelterDecorSystem.MemorialPlaquePrefix + "_generic", LocalizedMoraleDelta = 1.5f });
            var resolved = sys.ResolvePlaqueItemId("item_personal_keepsake_eli_drawing");
            Assert.Equal(ShelterDecorSystem.MemorialPlaquePrefix + "_drawing", resolved);
        }

        [Fact]
        public void DecorSystem_ResolvePlaqueSlot_PopulatesMemorialPlaqueMetadata()
        {
            var sys = new ShelterDecorSystem();
            sys.RegisterItemModifier(new ShelterDecorItemModifier { ItemId = ShelterDecorSystem.MemorialPlaquePrefix + "_carving", LocalizedMoraleDelta = 1.8f });
            var slot = sys.ResolvePlaqueSlot(
                memorialSurvivorId: "sv_eli_p",
                heirloomItemId: "item_personal_keepsake_eli_pewter_carving",
                memorialWallRoom: "loc_memorial_wall",
                plaqueSlotId: "peg_27",
                dayInstalled: 412);
            Assert.NotNull(slot);
            Assert.Equal("loc_memorial_wall", slot!.RoomId);
            Assert.Equal("peg_27", slot.SlotId);
            Assert.Equal(ShelterDecorSystem.MemorialPlaquePrefix + "_carving", slot.ItemId);
            Assert.True(slot.IsMemorialPlaque);
            Assert.Equal("sv_eli_p", slot.MemorialSurvivorId);
            Assert.Equal("item_personal_keepsake_eli_pewter_carving", slot.PlaqueSourceHeirloomId);
        }

        [Fact]
        public void DecorSystem_ResolvePlaqueItemId_EmptyString_BypassesFallback()
        {
            var sys = new ShelterDecorSystem();
            sys.RegisterItemModifier(new ShelterDecorItemModifier { ItemId = ShelterDecorSystem.MemorialPlaquePrefix + "_generic", LocalizedMoraleDelta = 1.5f });
            Assert.Equal(string.Empty, sys.ResolvePlaqueItemId(""));
        }

        // ---- Save round-trip ----

        [Fact]
        public void DecorSystem_CaptureRestore_IsolatesSnapshot()
        {
            var sys = new ShelterDecorSystem();
            sys.Assign("loc_lobby", "north_wall", "item_decor_poster_ration", 12);
            sys.Assign("loc_lobby", "south_panel", "item_decor_locomotive_nameplate", 13);

            var snap = sys.CaptureState();
            sys.Assign("loc_lobby", "east_wall", "item_decor_signal_log", 14);
            sys.Remove("loc_lobby", "south_panel");

            Assert.Equal(2, snap.Placements.Count);
            Assert.Contains(snap.Placements, p => p.SlotId == "north_wall" && p.ItemId == "item_decor_poster_ration");
            Assert.Contains(snap.Placements, p => p.SlotId == "south_panel" && p.ItemId == "item_decor_locomotive_nameplate");
        }

        [Fact]
        public void DecorSystem_Restore_PreservesMemorialPlaqueMetadata()
        {
            var sys = new ShelterDecorSystem();
            sys.Assign("loc_wall", "peg_5", "item_decor_memorial_plaque_generic", 270,
                isMemorialPlaque: true,
                memorialSurvivorId: "sv_jenny_t",
                plaqueSourceHeirloomId: "item_personal_keepsake_jenny_default");

            var snap = sys.CaptureState();
            var dest = new ShelterDecorSystem();
            dest.RestoreState(snap);
            var slot = dest.GetSlot("loc_wall", "peg_5");
            Assert.NotNull(slot);
            Assert.True(slot!.IsMemorialPlaque);
            Assert.Equal("sv_jenny_t", slot.MemorialSurvivorId);
            Assert.Equal("item_personal_keepsake_jenny_default", slot.PlaqueSourceHeirloomId);
        }

        // ---- SaveSectionRegistry exposes shelter_decor ----

        [Fact]
        public void SaveSectionRegistry_ShelterDecor_Section_IsRegistered()
        {
            var entries = Ashfall.Core.Save.SaveSectionRegistry.All.ToList();
            var entry = entries.FirstOrDefault(s => s.SectionKey == "shelter_decor");
            Assert.NotNull(entry);
            Assert.Equal("SaveShelterDecor", entry!.SaveMethod);
            Assert.Equal("shelter", entry.Owner);
            Assert.Equal("expanded_shelter", entry.LifecycleGroup);
        }

        [Fact]
        public void SaveSectionRegistry_ShelterDecor_HasCanonicalFileMap()
        {
            var names = Ashfall.Core.Save.SaveSectionRegistry.SectionFileNames;
            Assert.True(names.ContainsKey("shelter_decor"));
            Assert.Equal("shelter_decor_save.json", names["shelter_decor"]);
            // And the same key lives in the metadata registry so SaveStore
            // routing (which scans both) picks it up consistently.
            var meta = Ashfall.Core.Save.SaveSectionRegistry.All.First(s => s.SectionKey == "shelter_decor");
            Assert.Equal("expanded_shelter", meta.LifecycleGroup);
        }

        // ---- items.json decor entries ----

        [Fact]
        public void Items_Plan12CDecor_AllTwelveAuthored()
        {
            using var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(
                Path.Combine(DataDir, "items.json")));
            var items = doc.RootElement.GetProperty("items");
            string[] required =
            {
                "item_decor_poster_ration",
                "item_decor_poster_warning",
                "item_decor_locomotive_nameplate",
                "item_decor_carved_memorial",
                "item_decor_chalk_drawing",
                "item_decor_pressed_flower",
                "item_decor_medal_civic",
                "item_decor_classroom_chart",
                "item_decor_signal_log",
                "item_decor_memorial_plaque_generic",
                "item_decor_memorial_plaque_carving",
                "item_decor_memorial_plaque_drawing"
            };
            var ids = items.EnumerateArray().Select(i => i.GetProperty("id").GetString()!).ToHashSet();
            foreach (var r in required)
                Assert.True(ids.Contains(r), "decor item " + r + " missing from items.json");
        }

        [Fact]
        public void Items_Plan12CDecor_CarryDecorModifierField()
        {
            using var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(
                Path.Combine(DataDir, "items.json")));
            var items = doc.RootElement.GetProperty("items");
            int withModifier = 0;
            foreach (var it in items.EnumerateArray())
            {
                var id = it.GetProperty("id").GetString();
                if (id == null || !id.StartsWith("item_decor_", StringComparison.Ordinal)) continue;
                Assert.True(it.TryGetProperty("decorLocalizedMoraleDelta", out var m),
                    "decor item " + id + " must expose decorLocalizedMoraleDelta for host boot to read");
                Assert.True(m.GetDouble() > 0f,
                    "decor item " + id + " must carry a positive morale delta");
                withModifier++;
            }
            Assert.True(withModifier >= 12,
                "expected at least 12 decor items with decor modifier; got " + withModifier);
        }

        [Fact]
        public void Items_Plan12CDecor_LoadedCatalogPreservesLocalizedMoraleModifier()
        {
            var catalog = ItemCatalogLoader.LoadCatalog(
                DataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            var poster = catalog.Get("item_decor_poster_ration");
            Assert.NotNull(poster);
            Assert.Equal(1.5f, poster!.decorLocalizedMoraleDelta, 3);

            int registered = catalog.Ids
                .Select(catalog.Get)
                .Count(item => item != null
                    && item.id.StartsWith("item_decor_", StringComparison.Ordinal)
                    && item.decorLocalizedMoraleDelta > 0f);
            Assert.Equal(12, registered);
        }

        [Fact]
        public void PlayerSurfaceManifest_ExposesShelterDecorAsInteractiveSnapshotSurface()
        {
            PanelRegistryBootstrap.RegisterAll();
            var contract = PlayerSurfaceManifest.Generate().Contracts
                .Single(c => c.PanelId == "shelter_decor");

            Assert.Equal(SurfaceRouteKind.ExpandedShelter, contract.RouteKind);
            Assert.Equal(SurfaceActionCoverage.InteractiveCommands, contract.ActionCoverage);
            Assert.True(contract.HasSnapshotCoverage);
        }
    }
}
