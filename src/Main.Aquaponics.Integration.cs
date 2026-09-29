// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Random;
using Ashfall.Core.Shelter;
using Godot;
using System;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {

        private AquaponicsSystem? _aquaponics;
        private bool _aquaponicsDirty;

        // ── Plan B87 — closed-loop aquaponics ────────────────────────

        private void SetupAquaponics()
        {
            if (_aquaponics != null) return;

            SetupCampaignDay();
            SetupInventory();

            var rng = _campaignDay != null
                ? _campaignDay.Rng.GetStream(CampaignStreamIds.AquaponicsDisease).Rng
                : new SeededRng(87);

            _aquaponics = new AquaponicsSystem(
                rng,
                _inventory?.Inventory,
                roomId => ResolveAquaponicsPowerAvailability(roomId),
                new GodotLog());

            string catalogPath = System.IO.Path.Combine(_dataDir, AquaponicsCatalogLoader.DefaultFileName);
            if (System.IO.File.Exists(catalogPath))
            {
                var files = new FileSystemIO();
                var json = new SystemTextJsonSerializer();
                _aquaponics.LoadCatalog(
                    AquaponicsCatalogLoader.Load(_dataDir, files, json, new GodotLog()));
            }

            var saved = AquaponicsSaveStore.TryLoad();
            if (saved != null) _aquaponics.RestoreState(saved);

            _aquaponics.OnStateChanged += () => _aquaponicsDirty = true;
            _aquaponics.OnDiseaseMilestone += _ => _aquaponicsDirty = true;
            _aquaponics.OnHarvested += _ => _aquaponicsDirty = true;

            GD.Print("[Ashfall Godot] Aquaponics host ready (plan B87, " +
                     _aquaponics.TankClasses.Count + " tank classes).");
        }

        private float ResolveAquaponicsPowerAvailability(string roomId)
        {
            // Power truth stays on the grid. Aquaponics only queries availability.
            _ = roomId;
            if (_powerGrid?.System == null) return 1f;
            return _powerGrid.System.IsBrownout ? 0f : 1f;
        }

        private void SaveAquaponics()
        {
            if (_aquaponics == null) return;
            if (CaptureSection(AquaponicsSaveStore.SectionName,
                    AquaponicsSaveStore.TryCapturePersisted(_aquaponics.CaptureState())))
                _aquaponicsDirty = false;
        }

        private void TickAquaponics(int day)
        {
            SetupAquaponics();
            if (_aquaponics == null) return;

            // Temperature modifier stays a host projection; Core never owns room thermal truth.
            float temperatureModifier = 1f;
            _aquaponics.TickDay(day, temperatureModifier);
            if (_aquaponicsDirty) SaveAquaponics();
        }

        /// <summary>
        /// Explicit nutrient export for greenhouse consumers. Aquaponics never
        /// mutates greenhouse plots; callers pull this projection.
        /// </summary>
        private AquaponicNutrientSource? QueryAquaponicNutrientExport()
        {
            SetupAquaponics();
            return _aquaponics?.GetNutrientExport();
        }

    }
}
