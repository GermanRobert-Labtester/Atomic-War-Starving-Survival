// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plan 157 Host Wire & Orchestration
// Subsystem    : Grain Milling Discovery & Archive System
// Contract     : Read-only industrial food-processing projection over GrainMillingCatalog
//                (Plan 157 §4 authority firewall). Producers are existing
//                shelter rooms and expedition exploration sites; discovery
//                hooks the live ExpeditionSystem location-discovery event
//                and surfaces through the journal (the game's single feedback
//                strip). Historical milling observations NEVER mutate grain
//                inventory, flour yields, recipe outputs, nutrition, or
//                silo spoilage rates.
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
        private GrainMillingDiscoverySystem? _grainMillingDiscovery;
        private bool _grainMillingDiscoveryDirty;

        // ── Setup ────────────────────────────────────────────────────────

        public GrainMillingDiscoverySystem EnsureGrainMillingDiscovery()
        {
            if (_grainMillingDiscovery != null) return _grainMillingDiscovery;

            var catalog = GrainMillingCatalog.LoadFromDirectory(_dataDir);

            _grainMillingDiscovery = new GrainMillingDiscoverySystem(catalog);

            var saved = GrainMillingArchiveSaveStore.TryLoad();
            if (saved != null)
            {
                _grainMillingDiscovery.RestoreState(saved);
            }

            // Journal is the single feedback strip: one entry per record, on
            // first discovery only (idempotent by the archive's contract).
            _grainMillingDiscovery.OnRecordFirstDiscovered += (recordId, family) =>
            {
                if (_grainMillingDiscovery == null) return;
                var entry = DescribeGrainMillingRecord(recordId);
                string banner = family switch
                {
                    GrainMillingRecordFamily.MillstoneDressing => "GRAIN MILLING - BURR DRESSING LOG",
                    GrainMillingRecordFamily.BoltingSilk => "GRAIN MILLING - BOLTING SILK REPORT",
                    GrainMillingRecordFamily.SiloWeevil => "GRAIN MILLING - SILO WEEVIL AUDIT",
                    GrainMillingRecordFamily.DampenerTempering => "GRAIN MILLING - TEMPERING ASSAY",
                    _ => "GRAIN MILLING RECORD"
                };
                _journal?.TryAddRawEntry(
                    $"grain_milling_archive_{recordId}",
                    $"[{banner}] Recovered archival record: {entry}. Historical milling observations recorded at operation time — current machinery uncalibrated.",
                    null!, _simDay);
            };

            // Producer hook: expedition location discovery drives the archive.
            // Idempotent — undiscovered records at newly found sites surface
            // here; revisits return empty and stay silent.
            if (_expeditions != null)
            {
                _expeditions.Engine.OnLocationDiscovered += locationId =>
                {
                    var discovered = _grainMillingDiscovery?.DiscoverAtProducer(locationId, _simDay);
                    if (discovered != null && discovered.Count > 0)
                        _grainMillingDiscoveryDirty = true;
                };
            }

            return _grainMillingDiscovery;
        }

        public string DescribeGrainMillingRecord(string recordId)
        {
            if (_grainMillingDiscovery == null) return recordId;
            var catalog = _grainMillingDiscovery.Catalog;
            var mill = catalog.GetMillstone(recordId);
            if (mill != null) return $"{mill.MillstonePairId} ({mill.StoneMaterialType}, {mill.CracksPerInchCount:F1} cracks/in, {mill.RunnerRotationalRpm:F1} RPM)";
            var silk = catalog.GetSilk(recordId);
            if (silk != null) return $"{silk.SifterReelId} ({silk.SilkGauzeGrade}, {silk.MeshApertureMicrons:F0}µm, {silk.FlourExtractionYieldPct:F1}% yield)";
            var silo = catalog.GetSilo(recordId);
            if (silo != null) return $"{silo.GrainSiloBinId} ({silo.GrainCropSpecies}, {silo.GrainMoistureContentPct:F1}% moisture, {silo.GrainTemperatureCelsius:F1}°C)";
            var temper = catalog.GetTemper(recordId);
            if (temper != null) return $"{temper.ConditioningBinId} (+{temper.TemperingWaterAdditionPct:F1}%, target {temper.TargetMillingMoisturePct:F1}%, {temper.ConditioningDwellHours:F1}h dwell)";
            return recordId;
        }

        public void CheckShelterGrainMillingRecords(int currentDay)
        {
            if (_grainMillingDiscovery == null) return;
            _grainMillingDiscovery.DiscoverAtProducer("room_workshop", currentDay);
            _grainMillingDiscovery.DiscoverAtProducer("room_common_mess_hall", currentDay);
            _grainMillingDiscovery.DiscoverAtProducer("room_greenhouse", currentDay);
        }

        private void SetupGrainMillingArchive()
        {
            EnsureGrainMillingDiscovery();
        }

        // ── Save ─────────────────────────────────────────────────────────

        private void SaveGrainMillingArchive()
        {
            if (_grainMillingDiscovery != null)
            {
                CaptureSection(
                    GrainMillingArchiveSaveStore.SectionName,
                    GrainMillingArchiveSaveStore.TryCapturePersisted(_grainMillingDiscovery.CaptureState()));
            }
        }
    }
}
