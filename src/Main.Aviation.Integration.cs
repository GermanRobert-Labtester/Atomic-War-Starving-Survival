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
        private AviationSystem? _aviation;

        // ── Plan 182: Aviation & Jury-Rigged Aircraft ─────────────────────

        public AviationSystem EnsureAviation()
        {
            if (_aviation != null) return _aviation;

            _aviation = new AviationSystem();

            string catalogPath = CatalogPath.ResolveCatalog("aircraft_parts.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    _aviation.LoadCatalog(json, new SystemTextJsonSerializer());
                }
            }

            var saved = AviationSaveStore.TryLoad();
            if (saved != null)
            {
                _aviation.RestoreState(saved);
            }

            _aviation.OnFlightCrashed += (plan, reason) =>
            {
                _journal?.TryAddRawEntry("aviation_crash", $"CRITICAL: Flight {plan.flightId} has crashed! Reason: {reason}. Rescue requested.", null!, _simDay);
            };

            return _aviation;
        }

        private void SetupAviation()
        {
            EnsureAviation();
        }

        private void SaveAviation()
        {
            if (_aviation != null)
            {
                CaptureSection("aviation", AviationSaveStore.TryCapturePersisted(_aviation.CaptureState()));
            }
        }

    }
}
