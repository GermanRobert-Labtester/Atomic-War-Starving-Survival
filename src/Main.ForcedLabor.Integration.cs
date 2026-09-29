// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Factions;
using Ashfall.Core.Medical;
using Ashfall.Core.Narrative;
using Godot;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ForcedLaborSystem? _forcedLabor;

        // ── Plan 183: Forced Labor, Captivity & Rebellion ─────────────────

        public ForcedLaborSystem EnsureForcedLabor()
        {
            if (_forcedLabor != null) return _forcedLabor;

            _forcedLabor = new ForcedLaborSystem();

            string catalogPath = CatalogPath.ResolveCatalog("labor_camps.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    _forcedLabor.LoadCatalog(json, new SystemTextJsonSerializer());
                }
            }

            var saved = ForcedLaborSaveStore.TryLoad();
            if (saved != null)
            {
                _forcedLabor.RestoreState(saved);
            }

            _forcedLabor.OnRebellionTriggered += (msg) =>
            {
                _journal?.TryAddRawEntry("penal_rebellion", $"ALERT: Captive rebellion erupted! {msg}", null!, _simDay);
            };

            return _forcedLabor;
        }

        private void SetupForcedLabor()
        {
            EnsureForcedLabor();
        }

        private void SaveForcedLabor()
        {
            if (_forcedLabor != null)
            {
                CaptureSection("forced_labor", ForcedLaborSaveStore.TryCapturePersisted(_forcedLabor.CaptureState()));
            }
        }

    }
}
