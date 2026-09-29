// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Maritime;
using Ashfall.Core.Narrative;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private LeatherworkArchiveSystem? _leatherworkArchive;
        // ── Setup ────────────────────────────────────────────────────────

        public LeatherworkArchiveSystem EnsureLeatherworkArchive()
        {
            if (_leatherworkArchive != null) return _leatherworkArchive;

            var catalog = TanningLeatherCatalog.LoadFromDirectory(_dataDir);
            var itemCatalog = _inventory?.Catalog;
            var locationIds = new HashSet<string>(
                DeepLoreLocationCatalogLoader.Load(_dataDir, new FileSystemIO(), new SystemTextJsonSerializer())
                    .Select(l => l.id),
                StringComparer.Ordinal);

            _leatherworkArchive = new LeatherworkArchiveSystem(
                catalog,
                itemLookup: id => itemCatalog?.Get(id),
                locationExists: id => locationIds.Contains(id));

            foreach (var (recordId, itemId) in LeatherworkArchiveSystem.DefaultCanonicalLinks())
            {
                if (!_leatherworkArchive.TryRegisterCanonicalLink(recordId, itemId))
                    GD.PrintErr($"[Main.Leatherwork] canonical link failed: {recordId} -> {itemId}");
            }
            foreach (var (recordId, producer) in LeatherworkArchiveSystem.DefaultProducerMap())
            {
                if (!_leatherworkArchive.TryRegisterProducer(recordId, producer))
                    GD.PrintErr($"[Main.Leatherwork] producer registration failed: {recordId} -> {producer}");
            }

            var saved = LeatherworkArchiveSaveStore.TryLoad();
            if (saved != null)
            {
                _leatherworkArchive.RestoreState(saved);
            }

            // Journal is the single first-discovery feedback strip (deduped).
            _leatherworkArchive.OnRecordFirstDiscovered += (recordId, family) =>
            {
                if (_leatherworkArchive == null) return;
                var record = _leatherworkArchive.GetRecord(recordId);
                if (record == null) return;
                string familyLabel = family switch
                {
                    LeatherProcessFamily.OakBarkVegetableTan => "TANNERY PIT LOG",
                    LeatherProcessFamily.MineralTanLiquor => "TANNING LIQUOR ASSAY",
                    LeatherProcessFamily.RawhideBatingFailure => "BEAMHOUSE FAILURE REPORT",
                    LeatherProcessFamily.HarnessCurrying => "HARNESS CURRYING AUDIT",
                    _ => "LEATHERWORK RECORD"
                };
                string summary = string.IsNullOrEmpty(record.FailureSummary)
                    ? record.MeasurementSummary
                    : $"{record.MeasurementSummary} — {record.FailureSummary.Replace('_', ' ').ToLowerInvariant()}";
                _journal?.TryAddRawEntry(
                    $"leatherwork_archive_{recordId}",
                    $"[{familyLabel}] {record.FacilityLabel.Replace('_', ' ')}: {summary}. Archival process record — present status unknown.",
                    null!, _simDay);
            };

            // Producer hook: expedition location discovery surfaces records.
            if (_expeditions != null)
            {
                _expeditions.Engine.OnLocationDiscovered += locationId =>
                {
                    var discovered = _leatherworkArchive?.DiscoverAtProducer(locationId);
                    // Discovery ledger is captured unconditionally on save;
                    // nothing further to flag here. Reads stay side-effect free.
                    _ = discovered;
                };
            }

            return _leatherworkArchive;
        }

        private void SetupLeatherworkArchive()
        {
            EnsureLeatherworkArchive();
        }

        // ── Save ─────────────────────────────────────────────────────────

        private void SaveLeatherworkArchive()
        {
            if (_leatherworkArchive != null)
            {
                CaptureSection(
                    LeatherworkArchiveSaveStore.SectionName,
                    LeatherworkArchiveSaveStore.TryCapturePersisted(_leatherworkArchive.CaptureState()));
            }
        }

    }
}
