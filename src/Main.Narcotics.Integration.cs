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
        private NarcoticsSystem? _narcotics;

        // ── Plan 184: Chemical Engineering & Narcotics ────────────────────

        public NarcoticsSystem EnsureNarcotics()
        {
            if (_narcotics != null) return _narcotics;

            _narcotics = new NarcoticsSystem();

            string catalogPath = CatalogPath.ResolveCatalog("narcotics.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    _narcotics.LoadCatalog(json, new SystemTextJsonSerializer());
                }
            }

            var saved = NarcoticsSaveStore.TryLoad();
            if (saved != null)
            {
                _narcotics.RestoreState(saved);
            }

            _narcotics.OnOverdoseEmergency += (survivorId, msg) =>
            {
                _journal?.TryAddRawEntry("chem_overdose", $"MEDICAL EMERGENCY: Survivor {survivorId} suffered an overdose! {msg}", null!, _simDay);
            };

            return _narcotics;
        }

        private void SetupNarcotics()
        {
            EnsureNarcotics();
        }

        private void SaveNarcotics()
        {
            if (_narcotics != null)
            {
                CaptureSection("narcotics", NarcoticsSaveStore.TryCapturePersisted(_narcotics.CaptureState()));
            }
        }

    }
}
