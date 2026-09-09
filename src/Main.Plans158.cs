// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plan 158 Host Wire & Orchestration
// Subsystem    : Cordage, Cable & Technical Textiles — material archive
// Contract     : Read-only technical evidence (Plan 158 §5). Producers are
//                EXISTING deep-lore sites; discovery rides the live
//                ExpeditionSystem location-discovery event; the journal is
//                the single first-discovery feedback strip. No durability,
//                armor, vehicle, power, fire or crafting state is touched.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Maritime;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private TechnicalMaterialArchiveSystem? _technicalMaterialArchive;
        private bool _technicalMaterialArchiveDirty;

        // ── Setup ────────────────────────────────────────────────────────

        public TechnicalMaterialArchiveSystem EnsureTechnicalMaterialArchive()
        {
            if (_technicalMaterialArchive != null) return _technicalMaterialArchive;

            var (cordage, polymer) = (
                CordageCableCatalog.LoadFromDirectory(_dataDir),
                PolymerTextileCatalog.LoadFromDirectory(_dataDir));
            var itemCatalog = _inventory?.Catalog;
            var locationIds = new HashSet<string>(
                DeepLoreLocationCatalogLoader.Load(_dataDir, new FileSystemIO(), new SystemTextJsonSerializer())
                    .Select(l => l.id),
                StringComparer.Ordinal);

            _technicalMaterialArchive = new TechnicalMaterialArchiveSystem(
                cordage, polymer,
                itemLookup: id => itemCatalog?.Get(id),
                locationExists: id => locationIds.Contains(id));

            foreach (var (recordId, itemId) in TechnicalMaterialArchiveSystem.DefaultCanonicalLinks())
            {
                if (!_technicalMaterialArchive.TryRegisterCanonicalLink(recordId, itemId))
                    GD.PrintErr($"[Main.Materials] canonical link failed: {recordId} -> {itemId}");
            }
            foreach (var (recordId, producer) in TechnicalMaterialArchiveSystem.DefaultProducerMap())
            {
                if (!_technicalMaterialArchive.TryRegisterProducer(recordId, producer))
                    GD.PrintErr($"[Main.Materials] producer registration failed: {recordId} -> {producer}");
            }

            var saved = TechnicalMaterialArchiveSaveStore.TryLoad();
            if (saved != null)
            {
                _technicalMaterialArchive.RestoreState(saved);
            }

            // Journal is the single first-discovery feedback strip (deduped).
            _technicalMaterialArchive.OnRecordFirstDiscovered += (recordId, family) =>
            {
                if (_technicalMaterialArchive == null) return;
                var record = _technicalMaterialArchive.GetRecord(recordId);
                if (record == null) return;
                string familyLabel = family switch
                {
                    TechnicalMaterialFamily.HempFiberHackling => "FIBER PROCESS RECORD",
                    TechnicalMaterialFamily.WireRopeStranding => "CABLE ASSAY",
                    TechnicalMaterialFamily.ManilaHawserBreakage => "HAWSER FAILURE REPORT",
                    TechnicalMaterialFamily.TransmissionRopeSplicing => "DRIVE-LINE AUDIT",
                    TechnicalMaterialFamily.NeopreneGasketDegradation => "SEAL DEGRADATION LOG",
                    TechnicalMaterialFamily.AramidFiberRot => "ARMOR CLOTH FAILURE REPORT",
                    TechnicalMaterialFamily.TireRetreading => "WORKSHOP RETREAD LOG",
                    _ => "ARCHIVE MATERIAL HAZARD ASSESSMENT"
                };
                string summary = string.IsNullOrEmpty(record.FailureSummary)
                    ? record.MeasurementSummary
                    : $"{record.MeasurementSummary} — {record.FailureSummary.Replace('_', ' ').ToLowerInvariant()}";
                _journal?.TryAddRawEntry(
                    $"technical_material_record_{recordId}",
                    $"[{familyLabel}] {record.ObjectLabel.Replace('_', ' ')}: {summary}. Archival — present status unknown.",
                    null!, _simDay);
            };

            // Producer hook: expedition location discovery surfaces records.
            if (_expeditions != null)
            {
                _expeditions.Engine.OnLocationDiscovered += locationId =>
                {
                    var discovered = _technicalMaterialArchive?.DiscoverAtProducer(locationId);
                    if (discovered != null && discovered.Count > 0)
                        _technicalMaterialArchiveDirty = true;
                };
            }

            return _technicalMaterialArchive;
        }

        private void SetupTechnicalMaterialArchive()
        {
            EnsureTechnicalMaterialArchive();
        }

        // ── Save ─────────────────────────────────────────────────────────

        private void SaveTechnicalMaterialArchive()
        {
            if (_technicalMaterialArchive != null)
            {
                CaptureSection(
                    "technical_material_archive",
                    TechnicalMaterialArchiveSaveStore.TryCapturePersisted(_technicalMaterialArchive.CaptureState()));
            }
        }
    }
}
