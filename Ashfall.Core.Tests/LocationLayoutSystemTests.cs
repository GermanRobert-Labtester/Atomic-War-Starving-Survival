using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    public class LocationLayoutSystemTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static LocationLayoutSystem Loaded()
        {
            var sys = new LocationLayoutSystem(new FileSystemIO(), new SystemTextJsonSerializer());
            sys.Load(DataDir());
            return sys;
        }

        [Fact]
        public void CatalogLoadsAllFeaturedParents()
        {
            var sys = Loaded();
            Assert.Equal(14, sys.LayoutCount);
            Assert.NotNull(sys.GetLayout(LocationLayoutSystem.LocKilometre19));
            Assert.NotNull(sys.GetLayout(LocationLayoutSystem.LocTransitHq));
            Assert.Equal(4, sys.GetLayout(LocationLayoutSystem.LocKilometre19).RoomCount);
            Assert.Equal(5, sys.GetLayout(LocationLayoutSystem.LocTransitHq).RoomCount);
        }

        [Fact]
        public void RoomIdsUniqueSnakeCase()
        {
            var sys = Loaded();
            var set = new HashSet<string>();
            for (int i = 0; i < sys.Layouts.Count; i++)
            {
                var layout = sys.Layouts[i];
                Assert.False(string.IsNullOrEmpty(layout.parentLocationId));
                Assert.Equal(layout.parentLocationId, layout.parentLocationId.ToLowerInvariant());
                Assert.NotNull(layout.rooms);
                for (int r = 0; r < layout.rooms.Length; r++)
                {
                    var room = layout.rooms[r];
                    Assert.False(string.IsNullOrEmpty(room.id));
                    Assert.True(set.Add(room.id), "duplicate " + room.id);
                    Assert.Equal(room.id, room.id.ToLowerInvariant());
                    Assert.StartsWith("room_", room.id);
                    Assert.False(string.IsNullOrEmpty(room.inspect));
                    Assert.False(string.IsNullOrEmpty(room.description));
                    Assert.NotNull(room.adjacent);
                }
            }
            Assert.Equal(66, set.Count);
        }

        [Fact]
        public void Kilometre19InspectAndDescriptionAreVerbatim()
        {
            var km = Loaded().GetLayout(LocationLayoutSystem.LocKilometre19);
            var post = km.GetRoom(LocationLayoutSystem.RoomKm19Post);
            Assert.Equal("The Post", post.displayName);
            Assert.Equal("Ivy's stencil. Overlay's plate. Four screws, one of them the wrong metal.", post.inspect);
            Assert.Contains("CUT-19 / LAMP", post.description);
            Assert.Contains("The lamp will still be Ivy's.", post.description);

            var seam = km.GetRoom(LocationLayoutSystem.RoomKm19Seam);
            Assert.Equal("Ash on one side of a survey nail. Salt-white on the other. The nail is new.", seam.inspect);
            Assert.Contains("SEAM / DO NOT TREAT AS ONE DISTRICT", seam.description);

            var oil = km.GetRoom(LocationLayoutSystem.RoomKm19OilTin);
            Assert.Equal("The Oil Cache", oil.displayName);
            Assert.Equal("Ivy's wick tin. Beside it, a pigment pot with a thumbprint in lampblack.", oil.inspect);
            Assert.Contains("PIGMENT / LAMPBLACK / CUT", oil.description);

            var crate = km.GetRoom(LocationLayoutSystem.RoomKm19PlateCrate);
            Assert.Equal("Spares. Tissue between plates. The numbering runs past the post you are standing at.", crate.inspect);
            Assert.Contains("Dark until the post has been inspected.", crate.description);
            Assert.Contains("does not unlock from the bunker menu", crate.description);
        }

        [Fact]
        public void TransitInspectAndDescriptionAreVerbatim()
        {
            var hq = Loaded().GetLayout(LocationLayoutSystem.LocTransitHq);
            Assert.Equal(
                "A disc dispenser with nothing left to dispense. Queue paint on a floor that no longer has a queue.",
                hq.GetRoom(LocationLayoutSystem.RoomTransitLobby).inspect);
            Assert.Equal(
                "Wall-sized routes. Grease pencil. A trestle of printed plates waiting to become the truth.",
                hq.GetRoom(LocationLayoutSystem.RoomTransitMapGlass).inspect);
            Assert.Contains("HELD — DOB QUERY", hq.GetRoom(LocationLayoutSystem.RoomTransitMapGlass).description);
            Assert.Equal(
                "A telephone with the cord cut clean. A blotter that did the holding.",
                hq.GetRoom(LocationLayoutSystem.RoomTransitDobDesk).inspect);
            Assert.Equal("Typeset slots. A font without a word for held.", hq.GetRoom(LocationLayoutSystem.RoomTransitOverlayBench).inspect);
            Assert.Contains("Dark until map glass inspected.", hq.GetRoom(LocationLayoutSystem.RoomTransitOverlayBench).description);
            Assert.Equal(
                "The last order to turn the buses around, still on a spindle.",
                hq.GetRoom(LocationLayoutSystem.RoomTransitRadioGallery).inspect);
            Assert.Contains("EVACUATION COMPLETE", hq.GetRoom(LocationLayoutSystem.RoomTransitRadioGallery).description);
        }

        [Fact]
        public void DarkUntilExpansionUnlock()
        {
            var sys = Loaded();
            Assert.False(sys.IsUnlocked);
            Assert.False(sys.ArriveAtParent(LocationLayoutSystem.LocKilometre19));
            Assert.False(sys.CanEnter(LocationLayoutSystem.RoomKm19Post));
            Assert.True(sys.IsRoomDark(LocationLayoutSystem.LocKilometre19, LocationLayoutSystem.RoomKm19Post));
        }

        [Fact]
        public void ArrivalLightsOnlyEntryRooms()
        {
            var sys = Loaded();
            sys.Unlock();
            Assert.True(sys.ArriveAtParent(LocationLayoutSystem.LocKilometre19));
            Assert.True(sys.CanEnter(LocationLayoutSystem.RoomKm19Post));
            Assert.False(sys.CanEnter(LocationLayoutSystem.RoomKm19Seam));
            Assert.False(sys.CanEnter(LocationLayoutSystem.RoomKm19OilTin));
            Assert.False(sys.CanEnter(LocationLayoutSystem.RoomKm19PlateCrate));
            Assert.True(sys.IsRoomDark(LocationLayoutSystem.LocKilometre19, LocationLayoutSystem.RoomKm19PlateCrate));
        }

        [Fact]
        public void InspectPostUnlocksAdjacentAndPlateCrate()
        {
            var sys = Loaded();
            sys.Unlock();
            sys.ArriveAtParent(LocationLayoutSystem.LocKilometre19);

            var unlocked = new List<string>();
            sys.OnRoomUnlocked += (_, roomId) => unlocked.Add(roomId);

            Assert.True(sys.EnterRoom(LocationLayoutSystem.RoomKm19Post));
            Assert.False(sys.CanEnter(LocationLayoutSystem.RoomKm19Seam));
            Assert.False(sys.InspectRoom(LocationLayoutSystem.RoomKm19Seam));

            Assert.True(sys.InspectRoom(LocationLayoutSystem.RoomKm19Post));
            Assert.True(sys.CanEnter(LocationLayoutSystem.RoomKm19Seam));
            Assert.True(sys.CanEnter(LocationLayoutSystem.RoomKm19OilTin));
            Assert.True(sys.CanEnter(LocationLayoutSystem.RoomKm19PlateCrate));
            Assert.Contains(LocationLayoutSystem.RoomKm19Seam, unlocked);
            Assert.Contains(LocationLayoutSystem.RoomKm19OilTin, unlocked);
            Assert.Contains(LocationLayoutSystem.RoomKm19PlateCrate, unlocked);
            Assert.Equal("lore_sr_seam", sys.GetInspectKey(LocationLayoutSystem.LocKilometre19, LocationLayoutSystem.RoomKm19Post));
        }

        [Fact]
        public void CrateDoesNotUnlockFromBunkerMenu()
        {
            var sys = Loaded();
            sys.Unlock();
            Assert.False(sys.EnterRoom(LocationLayoutSystem.RoomKm19Post));
            Assert.False(sys.InspectRoom(LocationLayoutSystem.RoomKm19Post));
            Assert.False(sys.CanEnter(LocationLayoutSystem.RoomKm19PlateCrate));
        }

        [Fact]
        public void TransitAdjacencyUnlocksBenchAndGalleryByInspectRules()
        {
            var sys = Loaded();
            sys.Unlock();
            sys.ArriveAtParent(LocationLayoutSystem.LocTransitHq);
            Assert.True(sys.CanEnter(LocationLayoutSystem.RoomTransitLobby));
            Assert.False(sys.CanEnter(LocationLayoutSystem.RoomTransitMapGlass));
            Assert.False(sys.CanEnter(LocationLayoutSystem.RoomTransitOverlayBench));
            Assert.False(sys.CanEnter(LocationLayoutSystem.RoomTransitRadioGallery));

            Assert.True(sys.EnterRoom(LocationLayoutSystem.RoomTransitLobby));
            Assert.True(sys.InspectRoom(LocationLayoutSystem.RoomTransitLobby));
            Assert.True(sys.CanEnter(LocationLayoutSystem.RoomTransitMapGlass));
            Assert.False(sys.CanEnter(LocationLayoutSystem.RoomTransitDobDesk));
            Assert.False(sys.CanEnter(LocationLayoutSystem.RoomTransitOverlayBench));

            Assert.True(sys.EnterRoom(LocationLayoutSystem.RoomTransitMapGlass));
            Assert.True(sys.InspectRoom(LocationLayoutSystem.RoomTransitMapGlass));
            Assert.True(sys.CanEnter(LocationLayoutSystem.RoomTransitDobDesk));
            Assert.True(sys.CanEnter(LocationLayoutSystem.RoomTransitOverlayBench));
            Assert.False(sys.CanEnter(LocationLayoutSystem.RoomTransitRadioGallery));

            Assert.True(sys.EnterRoom(LocationLayoutSystem.RoomTransitDobDesk));
            Assert.True(sys.InspectRoom(LocationLayoutSystem.RoomTransitDobDesk));
            Assert.True(sys.CanEnter(LocationLayoutSystem.RoomTransitRadioGallery));
        }

        [Fact]
        public void EnterRaisesEventOnce()
        {
            var sys = Loaded();
            sys.Unlock();
            sys.ArriveAtParent(LocationLayoutSystem.LocKilometre19);
            int entered = 0;
            sys.OnRoomEntered += (_, roomId) =>
            {
                if (roomId == LocationLayoutSystem.RoomKm19Post) entered++;
            };
            Assert.True(sys.EnterRoom(LocationLayoutSystem.RoomKm19Post));
            Assert.True(sys.EnterRoom(LocationLayoutSystem.RoomKm19Post));
            Assert.Equal(1, entered);
            Assert.True(sys.HasEntered(LocationLayoutSystem.LocKilometre19, LocationLayoutSystem.RoomKm19Post));
        }

        [Fact]
        public void MutateRaisesLayoutMutated()
        {
            var sys = Loaded();
            string mutated = "";
            sys.OnLayoutMutated += parent => mutated = parent;
            sys.MutateLayout(LocationLayoutSystem.LocKilometre19, "mutation_km19_plated");
            Assert.Equal(LocationLayoutSystem.LocKilometre19, mutated);
            Assert.True(sys.HasFlag(LocationLayoutSystem.LocKilometre19, "mutation_km19_plated"));
        }

        [Fact]
        public void SaveRoundtripKeepsUnlockedRooms()
        {
            var json = new SystemTextJsonSerializer();
            var sys = Loaded();
            sys.Unlock();
            sys.ArriveAtParent(LocationLayoutSystem.LocKilometre19);
            sys.EnterRoom(LocationLayoutSystem.RoomKm19Post);
            sys.InspectRoom(LocationLayoutSystem.RoomKm19Post);
            sys.MutateLayout(LocationLayoutSystem.LocKilometre19, "mutation_km19_plated");
            string blob = json.Serialize(sys.CaptureState());

            var restored = Loaded();
            restored.RestoreState(json.Deserialize<LocationLayoutState>(blob));
            Assert.True(restored.IsUnlocked);
            Assert.True(restored.CanEnter(LocationLayoutSystem.LocKilometre19, LocationLayoutSystem.RoomKm19PlateCrate));
            Assert.True(restored.HasInspected(LocationLayoutSystem.LocKilometre19, LocationLayoutSystem.RoomKm19Post));
            Assert.True(restored.HasFlag(LocationLayoutSystem.LocKilometre19, "mutation_km19_plated"));
            Assert.Equal(LocationLayoutSystem.LocKilometre19, restored.CurrentParentId);
        }

        [Fact]
        public void OverlayCurrentLivesInStandingRecordFileNotASeventhPower()
        {
            string path = Path.Combine(DataDir(), "standing_record_factions.json");
            Assert.True(File.Exists(path));
            var json = new SystemTextJsonSerializer();
            var factions = CatalogLocator.LoadWrappedList<StandingRecordFactionStub>(File.ReadAllText(path), SystemTextJsonSerializer.Options);
            Assert.NotNull(factions);
            Assert.Single(factions);
            Assert.Equal("faction_the_overlay", factions[0].id);
            Assert.Equal("The Overlay", factions[0].display_name);
            Assert.Contains("Ground does not argue", factions[0].signature_quote);
        }

        private class StandingRecordFactionStub
        {
            public string id = "";
            public string display_name = "";
            public string signature_quote = "";
        }
    }
}
