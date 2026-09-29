// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Recreation;
using Godot;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ExpeditionNavalSystem? _navalSystem;

        // ── Plan 194: Naval & River Exploration ───────────────────────────

        public ExpeditionNavalSystem EnsureNavalSystem()
        {
            if (_navalSystem != null) return _navalSystem;

            _navalSystem = new ExpeditionNavalSystem(new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("naval_vessels.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    _navalSystem.LoadCatalog(json);
                }
            }

            return _navalSystem;
        }

    }
}
