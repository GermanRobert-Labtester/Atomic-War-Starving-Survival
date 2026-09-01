using System;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Memorial;
using Ashfall.Core.Shelter;
using Godot;
using AtomicWar.GodotApp;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Deterministic, disposable fixture for the Plan 12C visual contract.
    /// It deliberately travels through the production host seam: catalog
    /// loader → InventoryHostSession → shelter assignment → NeedsSystem →
    /// ShelterDecorHostSession → memorial-plaque projection. No fake panel
    /// rows or independent morale counters are used here.
    /// </summary>
    internal static class ShelterDecorSnapshotFixture
    {
        public static IDisposable? Bind(Node node)
        {
            if (node is not ShelterDecorPanel panel)
                return null;

            string dataDir = CatalogPath.ResolveDataDir();
            var catalog = ItemCatalogLoader.LoadCatalog(
                dataDir,
                CatalogPath.CreateFileIOForDataDir(dataDir),
                new SystemTextJsonSerializer());
            var inventory = new InventoryHostSession(new InventoryContainer(), catalog);
            var survivors = new SurvivorsHostSession();
            survivors.SeedDemoRoster();
            var assignment = ShelterAssignmentHostSession.CreateDefault(new SeededRng(12012));
            assignment.System.Assign("survivor_gunner_mikhail", "room_bunks", day: 12);
            assignment.System.Assign("survivor_dr_sarah_chen", "room_bunks", day: 12);

            var session = new ShelterDecorHostSession(
                new ShelterDecorSystem(), assignment.System, survivors.Needs, inventory);
            session.SetCurrentDay(12);
            session.LoadCatalogModifiers();

            Mount(inventory, session, "item_decor_poster_ration", count: 2, slotId: "north_wall");
            Mount(inventory, session, "item_decor_carved_memorial", count: 1, slotId: "shelf_1");
            session.TryMountMemorialPlaque(new MemorialEntry
            {
                SurvivorId = "survivor_memorial_fixture",
                HeirloomItemId = "item_personal_keepsake_fixture_carving",
                Day = 12
            }, out _);

            panel.Bind(session);
            return new FixtureOwner(panel, session, assignment, inventory, survivors);
        }

        private static void Mount(
            InventoryHostSession inventory,
            ShelterDecorHostSession session,
            string itemId,
            int count,
            string slotId)
        {
            var item = inventory.Catalog.Get(itemId)
                ?? throw new InvalidOperationException("Plan 12C snapshot item missing: " + itemId);
            if (!inventory.Inventory.Add(item, count))
                throw new InvalidOperationException("Could not seed snapshot decor item: " + itemId);
            if (!session.TryMount("room_bunks", slotId, itemId, 12, out var reason))
                throw new InvalidOperationException("Could not mount snapshot decor item: " + reason);
        }

        private sealed class FixtureOwner : IDisposable
        {
            private ShelterDecorPanel? _panel;
            private ShelterDecorHostSession? _session;
            private ShelterAssignmentHostSession? _assignment;
            private InventoryHostSession? _inventory;
            private SurvivorsHostSession? _survivors;

            public FixtureOwner(
                ShelterDecorPanel panel,
                ShelterDecorHostSession session,
                ShelterAssignmentHostSession assignment,
                InventoryHostSession inventory,
                SurvivorsHostSession survivors)
            {
                _panel = panel;
                _session = session;
                _assignment = assignment;
                _inventory = inventory;
                _survivors = survivors;
            }

            public void Dispose()
            {
                _panel?.Unbind();
                _session?.Dispose();
                _assignment?.Dispose();
                _inventory?.Dispose();
                _survivors?.Dispose();
                _panel = null;
                _session = null;
                _assignment = null;
                _inventory = null;
                _survivors = null;
            }
        }
    }
}
