// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plan 152 Host Wire & Orchestration
// Subsystem    : Black Projects Intelligence Archive
// Contract     : Read-only intelligence projection over the BlackProjectsCatalog
//                (Plan 152 §4 authority firewall). Producers are EXISTING
//                deep-lore sites bridged into the expedition destination
//                registry so DiscoverLocation can fire. Discovery hooks:
//                (1) OnLocationDiscovered, (2) expedition arrival at producer,
//                (3) map-detail inspection. No countdown, no drones, no
//                launches, no clearance, no vault access — ever.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Maritime;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private BlackProjectsArchiveSystem? _blackProjectsArchive;
        private bool _blackProjectsArchiveDirty;
        private bool _blackProjectsProducerHookBound;
        private bool _blackProjectsArrivalHookBound;
        private HashSet<string>? _blackProjectsProducerLocationIds;

        // ── Setup ────────────────────────────────────────────────────────

        public BlackProjectsArchiveSystem EnsureBlackProjectsArchive()
        {
            if (_blackProjectsArchive != null)
            {
                TryBindBlackProjectsProducerHooks();
                return _blackProjectsArchive;
            }

            var deepLoreEntries = DeepLoreLocationCatalogLoader.Load(
                _dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var locationIds = new HashSet<string>(
                deepLoreEntries.Select(l => l.id),
                StringComparer.Ordinal);

            // Fail closed: an injected location validator rejects invented sites.
            _blackProjectsArchive = new BlackProjectsArchiveSystem(
                BlackProjectsCatalog.LoadFromDirectory(_dataDir),
                id => locationIds.Contains(id));

            foreach (var (recordId, producer) in BlackProjectsArchiveSystem.DefaultProducerMap())
            {
                if (!_blackProjectsArchive.TryRegisterProducer(recordId, producer))
                    GD.PrintErr($"[Main.Archive] producer registration failed: {recordId} -> {producer}");
            }

            RegisterBlackProjectsProducerDestinations(deepLoreEntries);

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

            TryBindBlackProjectsProducerHooks();
            return _blackProjectsArchive;
        }

        /// <summary>
        /// Bridge Plan 152 deep-lore producer sites into the expedition
        /// destination registry + host Definitions list so DiscoverLocation
        /// and player travel can name the same IDs DefaultProducerMap uses.
        /// </summary>
        private void RegisterBlackProjectsProducerDestinations(
            IReadOnlyList<DeepLoreLocationEntry> deepLoreEntries)
        {
            _blackProjectsProducerLocationIds ??= new HashSet<string>(StringComparer.Ordinal);
            var byId = new Dictionary<string, DeepLoreLocationEntry>(StringComparer.Ordinal);
            for (int i = 0; i < deepLoreEntries.Count; i++)
            {
                var entry = deepLoreEntries[i];
                if (entry == null || string.IsNullOrEmpty(entry.id)) continue;
                byId[entry.id] = entry;
            }

            var producers = new HashSet<string>(StringComparer.Ordinal);
            foreach (var (_, producer) in BlackProjectsArchiveSystem.DefaultProducerMap())
            {
                if (!string.IsNullOrEmpty(producer))
                    producers.Add(producer);
            }

            foreach (var producerId in producers)
            {
                _blackProjectsProducerLocationIds.Add(producerId);
                if (ExpeditionDefinitionRegistry.Get(producerId) != null)
                {
                    EnsureProducerInHostDefinitions(producerId);
                    continue;
                }

                if (!byId.TryGetValue(producerId, out var lore) || lore == null)
                {
                    GD.PrintErr($"[Main.Archive] missing deep-lore producer destination: {producerId}");
                    continue;
                }

                int ticks = lore.travelHours > 0f
                    ? Math.Max(1, (int)Math.Round(lore.travelHours * 2f))
                    : 8;
                int danger = lore.dangerLevel > 0 ? lore.dangerLevel : 1;
                var def = new ExpeditionDefinition
                {
                    id = producerId,
                    displayName = !string.IsNullOrEmpty(lore.displayName) ? lore.displayName : producerId,
                    distanceTicks = ticks,
                    dangerLevel = danger,
                    encounterChancePerTick = Math.Clamp(0.10f + danger * 0.02f, 0.05f, 0.50f),
                    baseStaminaDrainPerHour = Math.Clamp(1.5f + danger * 0.25f, 1.0f, 5.0f),
                    lootCategories = new List<string>(),
                    scavenging_table_id = string.Empty,
                    requiresDiscovery = false
                };
                ExpeditionDefinitionRegistry.Register(def);
                EnsureProducerInHostDefinitions(def);
            }
        }

        private void EnsureProducerInHostDefinitions(string producerId)
        {
            var existing = ExpeditionDefinitionRegistry.Get(producerId);
            if (existing != null)
                EnsureProducerInHostDefinitions(existing);
        }

        private void EnsureProducerInHostDefinitions(ExpeditionDefinition def)
        {
            if (_expeditions == null || def == null || string.IsNullOrEmpty(def.id)) return;
            for (int i = 0; i < _expeditions.Definitions.Count; i++)
            {
                var d = _expeditions.Definitions[i];
                if (d != null && string.Equals(d.id, def.id, StringComparison.Ordinal))
                    return;
            }
            _expeditions.Definitions.Add(def);
        }

        private void TryBindBlackProjectsProducerHooks()
        {
            if (_expeditions?.Engine == null || _blackProjectsArchive == null) return;

            if (!_blackProjectsProducerHookBound)
            {
                _expeditions.Engine.OnLocationDiscovered += locationId =>
                {
                    var discovered = _blackProjectsArchive?.DiscoverAtProducer(locationId);
                    if (discovered != null && discovered.Count > 0)
                        _blackProjectsArchiveDirty = true;
                };
                _blackProjectsProducerHookBound = true;
            }

            // Arrival path: Outbound → Looting marks the destination known so
            // the producer hook above recovers archive records. Idempotent.
            if (!_blackProjectsArrivalHookBound)
            {
                _expeditions.Engine.OnExpeditionTick += exp =>
                {
                    if (exp == null || string.IsNullOrEmpty(exp.locationId)) return;
                    if ((ExpeditionPhase)exp.phase != ExpeditionPhase.Looting) return;
                    if (_blackProjectsProducerLocationIds == null
                        || !_blackProjectsProducerLocationIds.Contains(exp.locationId))
                        return;
                    _expeditions?.Engine.DiscoverLocation(exp.locationId);
                };
                _blackProjectsArrivalHookBound = true;
            }
        }

        /// <summary>
        /// Plan 152 map-detail path: inspecting a producer site recovers its
        /// archive records without inventing discovery theater for unrelated IDs.
        /// </summary>
        private void NotifyBlackProjectsProducerInspected(string locationId)
        {
            if (string.IsNullOrEmpty(locationId)) return;
            var archive = EnsureBlackProjectsArchive();
            if (_blackProjectsProducerLocationIds == null
                || !_blackProjectsProducerLocationIds.Contains(locationId))
                return;

            // Prefer the shared DiscoverLocation event path when the registry
            // knows the site; otherwise fall through to DiscoverAtProducer.
            if (_expeditions?.Engine != null
                && ExpeditionDefinitionRegistry.Get(locationId) != null)
            {
                _expeditions.Engine.DiscoverLocation(locationId);
            }

            var discovered = archive.DiscoverAtProducer(locationId);
            if (discovered != null && discovered.Count > 0)
                _blackProjectsArchiveDirty = true;
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

        // ── UI Panel (Plan 152 Follow-up) ─────────────────────────────────

        private UI.BlackProjectsArchivePanel? _blackProjectsArchivePanel;

        public UI.BlackProjectsArchivePanel EnsureBlackProjectsArchivePanel()
        {
            if (_blackProjectsArchivePanel != null) return _blackProjectsArchivePanel;

            _blackProjectsArchivePanel = new UI.BlackProjectsArchivePanel();
            _blackProjectsArchivePanel.Visible = false;
            _blackProjectsArchivePanel.OnClose += () => _blackProjectsArchivePanel.Visible = false;
            AddChild(_blackProjectsArchivePanel);
            return _blackProjectsArchivePanel;
        }

        public void OpenBlackProjectsArchivePanel()
        {
            var panel = EnsureBlackProjectsArchivePanel();
            var archive = EnsureBlackProjectsArchive();
            SetupJournal();
            panel.Bind(archive, _journal);
            panel.Open();
        }

        public void CloseBlackProjectsArchivePanel()
        {
            if (_blackProjectsArchivePanel != null)
                _blackProjectsArchivePanel.Close();
        }
    }
}
