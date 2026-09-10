// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plan 154 Host Wire & Orchestration
// Subsystem    : Hydrogeology Discovery & Archive System
// Contract     : Read-only scientific projection over HydroGeologyCatalog
//                (Plan 154 §4 authority firewall). Producers are existing
//                shelter rooms and expedition exploration sites; discovery
//                hooks the live ExpeditionSystem location-discovery event
//                and surfaces through the journal (the game's single feedback
//                strip). Historical assays and observations NEVER mutate water
//                stores, water purity, survivor radiation, generator power,
//                boiler temperature, or inventory ore/metals.
// ============================================================================
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private HydroGeologyDiscoverySystem? _hydroGeologyDiscovery;
        private bool _hydroGeologyDiscoveryDirty;

        // ── Setup ────────────────────────────────────────────────────────

        public HydroGeologyDiscoverySystem EnsureHydroGeologyDiscovery()
        {
            if (_hydroGeologyDiscovery != null) return _hydroGeologyDiscovery;

            var catalog = HydroGeologyCatalog.LoadFromDirectory(_dataDir);

            _hydroGeologyDiscovery = new HydroGeologyDiscoverySystem(catalog);

            var saved = HydroGeologyArchiveSaveStore.TryLoad();
            if (saved != null)
            {
                _hydroGeologyDiscovery.RestoreState(saved);
            }

            // Journal is the single feedback strip: one entry per record, on
            // first discovery only (idempotent by the archive's contract).
            _hydroGeologyDiscovery.OnRecordFirstDiscovered += (recordId, family) =>
            {
                if (_hydroGeologyDiscovery == null) return;
                var entry = DescribeHydroGeologyRecord(recordId);
                string banner = family switch
                {
                    HydroGeologyRecordFamily.ArtesianWell => "HYDROGEOLOGY - ARTESIAN WELL ASSAY",
                    HydroGeologyRecordFamily.CaveBiota => "HYDROGEOLOGY - CAVE BIOTA OBSERVATION",
                    HydroGeologyRecordFamily.GeothermalSteam => "HYDROGEOLOGY - STEAM VENT DIAGNOSTIC",
                    HydroGeologyRecordFamily.StalactiteMineral => "HYDROGEOLOGY - STALACTITE MINERAL ASSAY",
                    _ => "HYDROGEOLOGY RECORD"
                };
                _journal?.TryAddRawEntry(
                    $"hydrogeology_archive_{recordId}",
                    $"[{banner}] Recovered archival record: {entry}. Archival data recorded at sample time — present conditions unverified.",
                    null!, _simDay);
            };

            // Producer hook: expedition location discovery drives the archive.
            // Idempotent — undiscovered records at newly found sites surface
            // here; revisits return empty and stay silent.
            if (_expeditions != null)
            {
                _expeditions.Engine.OnLocationDiscovered += locationId =>
                {
                    var discovered = _hydroGeologyDiscovery?.DiscoverAtProducer(locationId, _simDay);
                    if (discovered != null && discovered.Count > 0)
                        _hydroGeologyDiscoveryDirty = true;
                };
            }

            return _hydroGeologyDiscovery;
        }

        public string DescribeHydroGeologyRecord(string recordId)
        {
            if (_hydroGeologyDiscovery == null) return recordId;
            var catalog = _hydroGeologyDiscovery.Catalog;
            var well = catalog.GetWellContamination(recordId);
            if (well != null) return $"{well.WellIdentifier} ({well.ContaminantAgent}, {well.ActivityLevelBqL:F1} Bq/L)";
            var biota = catalog.GetCaveBiota(recordId);
            if (biota != null) return $"{biota.SpeciesDesignation} ({biota.CavernLocation})";
            var steam = catalog.GetSteamVent(recordId);
            if (steam != null) return $"{steam.VentManifoldId} ({steam.SteamTemperatureCelsius:F1}°C, {steam.LinePressureBar:F1} bar)";
            var mineral = catalog.GetMineralAssay(recordId);
            if (mineral != null) return $"{mineral.SampleSpecimenId} ({mineral.MineralSpecies}, {mineral.PrimaryMetalAssayPct:F1}%)";
            return recordId;
        }

        public void CheckShelterHydroGeologyRecords(int currentDay)
        {
            if (_hydroGeologyDiscovery == null) return;
            _hydroGeologyDiscovery.DiscoverAtProducer("room_water_pump", currentDay);
            _hydroGeologyDiscovery.DiscoverAtProducer("room_filtration", currentDay);
            _hydroGeologyDiscovery.DiscoverAtProducer("room_main", currentDay);
        }

        private void SetupHydroGeologyDiscovery()
        {
            EnsureHydroGeologyDiscovery();
        }

        // ── Save ─────────────────────────────────────────────────────────

        private void SaveHydroGeologyDiscovery()
        {
            if (_hydroGeologyDiscovery != null)
            {
                CaptureSection(
                    HydroGeologyArchiveSaveStore.SectionName,
                    HydroGeologyArchiveSaveStore.TryCapturePersisted(_hydroGeologyDiscovery.CaptureState()));
            }
        }
    }
}
