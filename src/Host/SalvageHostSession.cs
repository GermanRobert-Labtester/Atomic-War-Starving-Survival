// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Inventory;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host composition for the salvage teardown bench (alpha feature).
    ///
    /// Loads the authored <c>salvage_teardown.json</c> and applies teardowns to
    /// the canonical <see cref="InventoryHostSession"/> — the single inventory
    /// authority. It owns no state of its own: consumed items, yielded
    /// components and tool-slot durability all persist through the inventory
    /// session's existing save path.
    /// </summary>
    public sealed class SalvageHostSession : HostSessionBase
    {
        private readonly InventoryHostSession _inventory;

        public SalvageTeardownSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public SalvageHostSession(InventoryHostSession inventory, SalvageTeardownSystem system)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        /// <summary>
        /// Load the authored teardown catalog. A missing or empty catalog is a
        /// hard failure: an unauthored bench is not invented here. Yielded
        /// components resolve through the inventory catalog so they keep their
        /// authored display name and weight.
        /// </summary>
        public static SalvageHostSession Load(string dataDirectory, IFileIO files, InventoryHostSession inventory)
        {
            if (inventory == null) throw new ArgumentNullException(nameof(inventory));
            string path = Path.Combine(dataDirectory ?? string.Empty, SalvageTeardownCatalog.FileName);
            if (files == null || !files.FileExists(path))
                throw new InvalidOperationException(
                    $"SalvageHostSession: {SalvageTeardownCatalog.FileName} does not exist at '{path}'.");

            var catalog = SalvageTeardownCatalog.Load(files.ReadAllText(path));
            if (catalog.Count == 0)
                throw new InvalidOperationException(
                    $"SalvageHostSession: {SalvageTeardownCatalog.FileName} defines no teardown recipes.");

            var system = new SalvageTeardownSystem(catalog, id => inventory.Catalog.Get(id));
            return new SalvageHostSession(inventory, system);
        }

        /// <summary>True when the item has an authored teardown recipe.</summary>
        public bool CanTeardown(string itemId) => System.CanTeardown(itemId);

        /// <summary>
        /// Run one teardown through the canonical inventory. Returns the Core
        /// result and records the message for presentation.
        /// </summary>
        public SalvageResult TryTeardown(string itemId, int count = 1)
        {
            var result = System.TryTeardown(_inventory.Inventory, itemId, count);
            LastEvent = result.Message;
            if (result.IsSuccess) RaiseStateChanged();
            return result;
        }
    }
}
