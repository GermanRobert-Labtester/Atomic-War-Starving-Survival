// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Archaeology;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Farming;
using Ashfall.Core.Medical;
using Ashfall.Core.Narrative;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private FungiCultivationSystem? _fungi;

        // ── Plan 192: Subterranean Fungi Cultivation ─────────────────────

        public FungiCultivationSystem EnsureFungi()
        {
            if (_fungi != null) return _fungi;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("fungi") : new SeededRng(192);
            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();

            _fungi = new FungiCultivationSystem(rng, inv, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("underground_flora.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    try
                    {
                        var catalog = System.Text.Json.JsonSerializer.Deserialize<UndergroundFloraCatalog>(json);
                        if (catalog != null)
                        {
                            _fungi.RegisterCatalog(catalog);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Fungi] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            var saved = FungiSaveStore.TryLoad();
            if (saved != null)
            {
                _fungi.RestoreState(saved);
            }

            _fungi.OnToxicBloom += (plotId, roomId) =>
            {
                _journal?.TryAddRawEntry("fungi_toxic_bloom", $"Toxic mold outbreak detected at plot {plotId} in {roomId}!", null!, _simDay);
            };

            _fungi.OnFungiHarvested += (plotId, strain, count) =>
            {
                _journal?.TryAddRawEntry("fungi_harvest", $"Harvested {count} units of {strain} from subterranean bed {plotId}.", null!, _simDay);
            };

            _fungi.OnSubstratePrepared += (plotId, preparation, usedHeat) =>
            {
                _journal?.TryAddRawEntry("fungi_substrate_prepared", $"Bed {plotId} substrate prepared ({preparation}{(usedHeat ? ", heated" : "")}).", null!, _simDay);
            };

            _fungi.OnSubstrateDisposed += (plotId, method) =>
            {
                _journal?.TryAddRawEntry("fungi_substrate_disposed", $"Contaminated substrate from bed {plotId} disposed ({method}).", null!, _simDay);
            };

            _fungi.OnContaminationSpread += (sourcePlotId, roomId) =>
            {
                _journal?.TryAddRawEntry("fungi_contamination_spread", $"Mold contamination is spreading from bed {sourcePlotId} to neighbouring beds in {roomId}.", null!, _simDay);
            };

            return _fungi;
        }

        private void SetupFungi()
        {
            EnsureFungi();
        }

        private void SaveFungi()
        {
            if (_fungi != null)
            {
                CaptureSection("fungi_cultivation", FungiSaveStore.TryCapturePersisted(_fungi.CaptureState()));
            }
        }

    }
}
