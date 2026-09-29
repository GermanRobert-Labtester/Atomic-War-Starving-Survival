// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Excavation;
using Ashfall.Core.IO;
using Ashfall.Core.Quests;
using Ashfall.Core.Radio;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Godot;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ShelterWorkshopSystem? _shelterWorkshop;

        private bool _shelterWorkshopDirty;

        // ── Plan 46: Precision Workshop ─────────────────────────────────

        public ShelterWorkshopSystem EnsureShelterWorkshop()
        {
            if (_shelterWorkshop != null) return _shelterWorkshop;

            SetupInventory();
            var inv = _inventory.Inventory;
            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("shelter_workshop") : new SeededRng(46);
            var equip = _equipmentCondition?.System;
            var veh = _expeditions?.Vehicles;

            _shelterWorkshop = new ShelterWorkshopSystem(inv, rng, equip, veh, new GodotLog());

            // Load authoritative catalog
            string catalogPath = CatalogPath.ResolveCatalog("workshop_recipes.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    _shelterWorkshop.LoadCatalog(json);
                }
            }

            var saved = ShelterWorkshopSaveStore.TryLoad();
            if (saved != null)
            {
                _shelterWorkshop.RestoreState(saved);
            }

            _shelterWorkshop.OnJobCompleted += (job) =>
            {
                if (job.RecipeId.Contains("weapon", StringComparison.OrdinalIgnoreCase) ||
                    job.RecipeId.Contains("repair", StringComparison.OrdinalIgnoreCase) ||
                    job.RecipeId.Contains("ammo", StringComparison.OrdinalIgnoreCase))
                {
                    _dynamicQuests?.AdvanceQuestProgress(DynamicQuestlineSystem.ArmoryMunitionsRefurbishQuestId, 1);
                }
            };

            _shelterWorkshop.OnWorkshopChanged += () =>
            {
                _shelterWorkshopDirty = true;
                SyncCraftingStationsFromShelter();
            };
            return _shelterWorkshop;
        }

        private void SetupWorkshop()
        {
            EnsureShelterWorkshop();
        }

        private void SaveWorkshop()
        {
            if (_shelterWorkshop != null)
            {
                CaptureSection("shelter_workshop", ShelterWorkshopSaveStore.TryCapturePersisted(_shelterWorkshop.CaptureState()));
                _shelterWorkshopDirty = false;
            }
        }

    }
}
