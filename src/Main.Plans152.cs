// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plan 152 Host Wire & Orchestration
// Subsystem    : Black Projects Intelligence Archive
// Contract     : Read-only intelligence projection over the BlackProjectsCatalog
//                (Plan 152 §4 authority firewall). Producers are EXISTING
//                deep-lore sites; discovery hooks the live ExpeditionSystem
//                location-discovery event and surfaces through the journal
//                (the game's single feedback strip). No countdown, no drones,
//                no launches, no clearance, no vault access — ever.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Maritime;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private BlackProjectsArchiveSystem? _blackProjectsArchive;
        private bool _blackProjectsArchiveDirty;

        // ── Setup ────────────────────────────────────────────────────────

        public BlackProjectsArchiveSystem EnsureBlackProjectsArchive()
        {
            if (_blackProjectsArchive != null) return _blackProjectsArchive;

            var catalog = BlackProjectsCatalog.LoadFromDirectory(_dataDir);
            var locationIds = new HashSet<string>(
                DeepLoreLocationCatalogLoader.Load(
                    _dataDir, new FileSystemIO(), new SystemTextJsonSerializer())
                    .Select(l => l.id),
                StringComparer.Ordinal);

            // Fail closed: an injected location validator rejects invented sites.
            _blackProjectsArchive = new BlackProjectsArchiveSystem(
                catalog, id => locationIds.Contains(id));

            foreach (var (recordId, producer) in BlackProjectsArchiveSystem.DefaultProducerMap())
            {
                if (!_blackProjectsArchive.TryRegisterProducer(recordId, producer))
                    GD.PrintErr($"[Main.Archive] producer registration failed: {recordId} -> {producer}");
            }

            var saved = BlackProjectsArchiveSaveStore.TryLoad();
            if (saved != null)
            {
                _blackProjectsArchive.RestoreState(saved);
            }

            // Journal is the single feedback strip: one entry per record, on
            // first discovery only (idempotent by the archive's contract).
            _blackProjectsArchive.OnRecordFirstDiscovered += (recordId, truthClass) =>
            {
                if (_blackProjectsArchive == null) return;
                var entry = DescribeRecord(recordId);
                string banner = truthClass switch
                {
                    BlackProjectsTruthClass.InstrumentTelemetry => "INSTRUMENT TELEMETRY",
                    BlackProjectsTruthClass.VehicleBlackbox => "VEHICLE BLACKBOX",
                    BlackProjectsTruthClass.ClassifiedDirective => "CLASSIFIED DIRECTIVE",
                    BlackProjectsTruthClass.ComplianceAudit => "COMPLIANCE AUDIT",
                    _ => "AMBIGUOUS INTELLIGENCE"
                };
                _journal?.TryAddRawEntry(
                    $"black_projects_archive_{recordId}",
                    $"[{banner}] Recovered record: {entry}. Archival — present status unknown.",
                    null!, _simDay);
            };

            // Producer hook: expedition location discovery drives the archive.
            // Idempotent — undiscovered records at newly found sites surface
            // here; revisits return empty and stay silent.
            if (_expeditions != null)
            {
                _expeditions.Engine.OnLocationDiscovered += locationId =>
                {
                    var discovered = _blackProjectsArchive?.DiscoverAtProducer(locationId);
                    if (discovered != null && discovered.Count > 0)
                        _blackProjectsArchiveDirty = true;
                };
            }

            return _blackProjectsArchive;
        }

        private string DescribeRecord(string recordId)
        {
            if (_blackProjectsArchive == null) return recordId;
            var catalog = _blackProjectsArchive.Catalog;
            var o = catalog.GetOrbital(recordId);
            if (o != null) return $"{o.Callsign} — {o.EntryType.Replace('_', ' ').ToLowerInvariant()}";
            var d = catalog.GetDrone(recordId);
            if (d != null) return $"{d.CarrierId} blackbox — {d.RecordType.Replace('_', ' ').ToLowerInvariant()}";
            var c = catalog.GetCobalt(recordId);
            if (c != null) return $"directive {c.DirectiveCode}";
            var v = catalog.GetVault(recordId);
            if (v != null) return $"{v.VaultId} audit — {v.AuditType.Replace('_', ' ').ToLowerInvariant()}";
            return recordId;
        }

        private void SetupBlackProjectsArchive()
        {
            EnsureBlackProjectsArchive();
        }

        // ── Save ─────────────────────────────────────────────────────────

        private void SaveBlackProjectsArchive()
        {
            if (_blackProjectsArchive != null)
            {
                CaptureSection(
                    "black_projects_archive",
                    BlackProjectsArchiveSaveStore.TryCapturePersisted(_blackProjectsArchive.CaptureState()));
            }
        }
    }
}
