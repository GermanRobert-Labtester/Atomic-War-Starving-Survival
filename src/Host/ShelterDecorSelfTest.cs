using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Memorial;
using Ashfall.Core.Shelter;
using AtomicWar.GodotApp.UI;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Narrow production seam gate for Plan 12C. It intentionally uses the
    /// live item catalog, inventory container, assignment map, survivor needs,
    /// save façade, and real Control construction—not hand-authored test DTOs.
    /// </summary>
    internal static class ShelterDecorSelfTest
    {
        public static int Run(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string message)
            {
                if (condition) GD.Print("[PASS] " + message);
                else
                {
                    GD.PrintErr("[FAIL] " + message);
                    failures++;
                }
            }

            try
            {
                GD.Print("[ShelterDecorSelfTest] Starting catalog → inventory → room morale → memorial wall verification...");
                var catalog = ItemCatalogLoader.LoadCatalog(dataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
                var inventory = new InventoryHostSession(new InventoryContainer(), catalog);
                var poster = catalog.Get("item_decor_poster_ration");
                Check(poster != null && poster.decorLocalizedMoraleDelta > 0f,
                    "items.json decor modifier loaded through ItemCatalog");
                if (poster == null)
                    return HostCli.EmitSummary("shelter_decor_selftest", false, 1, 0, 1, "missing item_decor_poster_ration");
                Check(inventory.Inventory.Add(poster, 1), "decor item seeded into real inventory");

                var survivors = new SurvivorsHostSession();
                survivors.SeedDemoRoster();
                const string survivorId = "survivor_gunner_mikhail";
                var assignment = ShelterAssignmentHostSession.CreateDefault(new SeededRng(12012));
                Check(assignment.System.Assign(survivorId, "room_bunks", day: 3).Succeeded,
                    "alive survivor assigned to a real shelter room");

                var session = new ShelterDecorHostSession(
                    new ShelterDecorSystem(), assignment.System, survivors.Needs, inventory);
                session.SetCurrentDay(3);
                Check(session.LoadCatalogModifiers() == 12,
                    "all twelve item_decor_* modifiers registered from the live catalog");
                int beforeInventory = inventory.Inventory.CountById(poster.id);
                Check(session.TryMount("room_bunks", "north_wall", poster.id, 3, out _),
                    "mount consumes the selected real inventory item");
                Check(inventory.Inventory.CountById(poster.id) == beforeInventory - 1,
                    "mount decremented Holdfast storage exactly once");

                float moraleBefore = survivors.Find(survivorId)?.Morale ?? -1f;
                Check(session.ApplyDailyMorale(4) == 1,
                    "daily decorator pass finds the active room occupant");
                float moraleAfter = survivors.Find(survivorId)?.Morale ?? -1f;
                Check(System.Math.Abs(moraleAfter - (moraleBefore + poster.decorLocalizedMoraleDelta)) < 0.001f,
                    "daily decorator pass writes only through NeedsSystem morale");

                Check(session.TryRemoveMount("room_bunks", "north_wall", out _),
                    "player-mounted decor can return to storage");
                Check(inventory.Inventory.CountById(poster.id) == beforeInventory,
                    "remove returned the original item to Holdfast storage");

                var memorial = new MemorialEntry
                {
                    SurvivorId = "survivor_memorial_probe",
                    HeirloomItemId = "item_personal_keepsake_probe_carving",
                    Day = 4
                };
                Check(session.TryMountMemorialPlaque(memorial, out _),
                    "memorial entry projects to a canonical plaque without fabricating inventory");
                Check(session.System.GetSlot(ShelterDecorHostSession.MemorialWallRoomId, "plaque_survivor_memorial_probe")?.IsMemorialPlaque == true,
                    "memorial wall retains plaque provenance metadata");

                string serialized = ShelterDecorSaveStore.TryCapture(session.System.State);
                var restored = ShelterDecorSaveStore.TryRestore(serialized);
                Check(restored != null && restored.Placements.Count == session.System.State.Placements.Count,
                    "decor save façade round-trips mounted memorial state");

                var panel = new ShelterDecorPanel();
                panel._Ready();
                panel.Bind(session);
                Check(panel.SelectRoom(ShelterDecorHostSession.MemorialWallRoomId)
                    && panel.IsBound && panel.RenderedPlacementCount == 1,
                    "memorial wall panel constructs and renders the live plaque placement");
                panel.Unbind();
                panel.QueueFree();
                session.Dispose();
                assignment.Dispose();
                inventory.Dispose();
                survivors.Dispose();
            }
            catch (System.Exception ex)
            {
                GD.PrintErr("[FAIL] Shelter decor self-test threw: " + ex.Message);
                failures++;
            }

            return HostCli.EmitSummary(
                "shelter_decor_selftest",
                failures == 0,
                failures == 0 ? 0 : 1,
                -1,
                failures,
                failures == 0 ? "PASS" : $"FAIL ({failures})");
        }
    }
}
