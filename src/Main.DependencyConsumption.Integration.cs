// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using Ashfall.Core.Medical;
using Ashfall.Core.Narrative;
using Godot;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private bool _dependencyConsumeHookWired;
        private Dictionary<string, ChemicalDependencyKind>? _dependencyKindsByItemId;

        // ── Narcotics slice: canonical consumption → dependency authority ──

        /// <summary>
        /// Loads chemical_dependency_items.json (item_id → dependency_kind).
        /// Parse failure leaves the map null — consumption then simply carries
        /// no dependency routing (fail closed, no invented kinds).
        /// </summary>
        private Dictionary<string, ChemicalDependencyKind>? EnsureDependencyKindMap()
        {
            if (_dependencyKindsByItemId != null) return _dependencyKindsByItemId;

            string catalogPath = CatalogPath.ResolveCatalog("chemical_dependency_items.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (!_catalogIo.FileExists(catalogPath)) return null;
            string _depJson = _catalogIo.ReadAllText(catalogPath);

            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(_depJson);
                var map = new Dictionary<string, ChemicalDependencyKind>(StringComparer.Ordinal);
                foreach (var it in doc.RootElement.GetProperty("items").EnumerateArray())
                {
                    if (!it.TryGetProperty("item_id", out var idEl)) continue;
                    if (!it.TryGetProperty("dependency_kind", out var kindEl)) continue;
                    if (map.ContainsKey(idEl.GetString() ?? string.Empty)) continue;
                    map[idEl.GetString()!] = kindEl.GetString()?.ToLowerInvariant() switch
                    {
                        "opioid" => ChemicalDependencyKind.Opioid,
                        "alcohol" => ChemicalDependencyKind.Alcohol,
                        "stimulant" => ChemicalDependencyKind.Stimulant,
                        "sedative" => ChemicalDependencyKind.Sedative,
                        _ => ChemicalDependencyKind.Opioid // catalog rows are all dependency substances
                    };
                }
                _dependencyKindsByItemId = map;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Main.Contraband] failed to parse dependency catalog: {ex.Message}");
            }
            return _dependencyKindsByItemId;
        }

        /// <summary>
        /// Routes committed inventory consumptions of dependency-catalog items
        /// (morphine, alcohol, …) into the ChemicalDependencySystem — exactly
        /// once per committed consumption, because InventoryHostSession.
        /// OnConsumed fires exactly once after the transaction commits. The
        /// dependency system owns dose math and events; nothing here reads any
        /// contraband mechanics field.
        /// </summary>
        private void WireDependencyConsumeHook()
        {
            if (_dependencyConsumeHookWired || _inventory == null) return;
            _dependencyConsumeHookWired = true;

            _inventory.OnConsumed += (survivorId, itemId) =>
            {
                if (_chemicalDependency == null || string.IsNullOrEmpty(itemId)) return;
                var kinds = EnsureDependencyKindMap();
                if (kinds == null || !kinds.TryGetValue(itemId, out var kind)) return;

                _chemicalDependency.System.OnSubstanceConsumed(survivorId ?? string.Empty, itemId, kind);
            };
        }

    }
}
