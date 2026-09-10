// SPDX-License-Identifier: MIT
// ASHFALL Core — Plan 154 Hydrogeology Discovery & Archive System
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Narrative
{
    /// <summary>Serialized discovery bookkeeping for the hydrogeology archive. Record IDs only.</summary>
    [Serializable]
    public sealed class HydroGeologyArchiveState
    {
        public string systemId = HydroGeologyDiscoverySystem.SystemId;
        public List<string> discoveredRecordIds = new List<string>();
    }

    /// <summary>Deterministic cross-record scientific relation between discovered records.</summary>
    public sealed class HydroGeologyRelatedRecord
    {
        public string RecordId { get; }
        public string RelationKind { get; }
        public string Detail { get; }

        public HydroGeologyRelatedRecord(string recordId, string relationKind, string detail)
        {
            RecordId = recordId;
            RelationKind = relationKind;
            Detail = detail;
        }
    }

    /// <summary>
    /// Plan 154: Read-only scientific discovery projection over HydroGeologyCatalog
    /// (30 records across: artesian wells, cave biota, steam vents, stalactite assays).
    ///
    /// Authority Firewall:
    /// - Exposes ONLY: producer registration, discovery tracking, and derived read-only relations.
    /// - Contains NO methods to mutate water stores, water purity, survivor radiation, generator state,
    ///   boiler temperature, or inventory ore/metals.
    /// - Discovery is idempotent; first discovery triggers events; repeat discovery is a no-op.
    /// - Related records only link already-discovered records — never spoiling undiscovered entries.
    /// </summary>
    public sealed class HydroGeologyDiscoverySystem
    {
        public const string SystemId = "hydrogeology_archive";

        private readonly HydroGeologyCatalog _catalog;
        private readonly Func<string, bool>? _locationExists;
        private readonly Dictionary<string, string> _producerByRecordId =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, List<string>> _recordsByProducer =
            new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        private HydroGeologyArchiveState _state = new HydroGeologyArchiveState();

        /// <summary>Raised once per record on its initial discovery.</summary>
        public event Action<string, HydroGeologyRecordFamily>? OnRecordFirstDiscovered;
        public event Action? OnStateChanged;

        public HydroGeologyDiscoverySystem(HydroGeologyCatalog catalog, Func<string, bool>? locationExists = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _locationExists = locationExists;
            InitializeDefaultProducers();
        }

        public HydroGeologyCatalog Catalog => _catalog;
        public HydroGeologyArchiveState State => _state;

        public IReadOnlyList<string> DiscoveredRecordIds
        {
            get
            {
                var list = new List<string>(_state.discoveredRecordIds);
                list.Sort(StringComparer.Ordinal);
                return list;
            }
        }

        private void InitializeDefaultProducers()
        {
            foreach (var meta in HydroGeologyProjection.GetAllMetadata())
            {
                if (meta.IsActivated && !string.IsNullOrWhiteSpace(meta.ProducerId))
                {
                    TryRegisterProducer(meta.RecordId, meta.ProducerId);
                }
            }
        }

        public bool TryRegisterProducer(string recordId, string producerLocationId)
        {
            if (string.IsNullOrWhiteSpace(recordId) || string.IsNullOrWhiteSpace(producerLocationId))
                return false;

            if (_catalog.GetWellContamination(recordId) == null
                && _catalog.GetCaveBiota(recordId) == null
                && _catalog.GetSteamVent(recordId) == null
                && _catalog.GetMineralAssay(recordId) == null)
            {
                return false; // unknown record
            }

            if (_locationExists != null && !_locationExists(producerLocationId))
                return false; // invalid producer location

            if (_producerByRecordId.ContainsKey(recordId))
                return false; // one primary producer per record

            _producerByRecordId[recordId] = producerLocationId;
            if (!_recordsByProducer.TryGetValue(producerLocationId, out var list))
            {
                list = new List<string>();
                _recordsByProducer[producerLocationId] = list;
            }
            list.Add(recordId);
            return true;
        }

        public bool IsDiscovered(string recordId)
            => !string.IsNullOrEmpty(recordId) && _state.discoveredRecordIds.Contains(recordId, StringComparer.OrdinalIgnoreCase);

        public bool HasProducer(string recordId) => _producerByRecordId.ContainsKey(recordId);

        public string? GetProducer(string recordId)
            => _producerByRecordId.TryGetValue(recordId, out var p) ? p : null;

        public IReadOnlyList<string> DeferredRecordIds()
        {
            var result = new List<string>();
            foreach (var meta in HydroGeologyProjection.GetAllMetadata())
            {
                if (!HasProducer(meta.RecordId))
                    result.Add(meta.RecordId);
            }
            result.Sort(StringComparer.Ordinal);
            return result;
        }

        public bool TryDiscoverRecord(string recordId, int currentDay = 1)
        {
            if (string.IsNullOrWhiteSpace(recordId)) return false;

            var meta = HydroGeologyProjection.ResolveMetadata(recordId);
            if (meta == null || !meta.IsActivated) return false;
            if (currentDay < meta.MinDay) return false;

            if (IsDiscovered(recordId)) return false;

            _state.discoveredRecordIds.Add(recordId);
            _state.discoveredRecordIds.Sort(StringComparer.Ordinal);

            OnRecordFirstDiscovered?.Invoke(recordId, meta.Family);
            OnStateChanged?.Invoke();
            return true;
        }

        public IReadOnlyList<string> DiscoverAtProducer(string producerLocationId, int currentDay = 1)
        {
            var discovered = new List<string>();
            if (string.IsNullOrWhiteSpace(producerLocationId)) return discovered;

            if (_recordsByProducer.TryGetValue(producerLocationId, out var recordIds))
            {
                foreach (var id in recordIds)
                {
                    if (TryDiscoverRecord(id, currentDay))
                    {
                        discovered.Add(id);
                    }
                }
            }
            return discovered;
        }

        public IReadOnlyList<HydroGeologyRelatedRecord> GetRelated(string recordId)
        {
            var related = new List<HydroGeologyRelatedRecord>();
            if (!IsDiscovered(recordId)) return related;

            var meta = HydroGeologyProjection.ResolveMetadata(recordId);
            if (meta == null) return related;

            // Strict privacy: only surface relations to records that are ALREADY discovered.
            foreach (var otherId in DiscoveredRecordIds)
            {
                if (string.Equals(otherId, recordId, StringComparison.OrdinalIgnoreCase)) continue;

                var otherMeta = HydroGeologyProjection.ResolveMetadata(otherId);
                if (otherMeta == null) continue;

                // 1. Same producer / subterranean sector
                if (string.Equals(meta.ProducerId, otherMeta.ProducerId, StringComparison.OrdinalIgnoreCase))
                {
                    related.Add(new HydroGeologyRelatedRecord(
                        otherId,
                        "shared_producer_site",
                        $"Co-located observation at {meta.ProducerId}"));
                }
                // 2. Radionuclide linkage (e.g. Tritium/Strontium/Radium wells and Uranophane/Autunite assays)
                else if (meta.ContaminantClass == HydroGeologyContaminantClassification.Radionuclide &&
                         (otherMeta.ContaminantClass == HydroGeologyContaminantClassification.Radionuclide ||
                          otherMeta.Family == HydroGeologyRecordFamily.StalactiteMineral && otherId.Contains("uran")))
                {
                    related.Add(new HydroGeologyRelatedRecord(
                        otherId,
                        "radionuclide_migration",
                        "Shared hydrological radionuclide dispersal pathway"));
                }
                // 3. Geothermal circuit linkage (steam vents and geothermal borehole reinjection/minerals)
                else if ((meta.Family == HydroGeologyRecordFamily.GeothermalSteam || meta.RecordId.Contains("geothermal")) &&
                         (otherMeta.Family == HydroGeologyRecordFamily.GeothermalSteam || otherMeta.RecordId.Contains("geothermal")))
                {
                    related.Add(new HydroGeologyRelatedRecord(
                        otherId,
                        "geothermal_circuit",
                        "Coupled deep hydrothermal and steam manifold telemetry"));
                }
                // 4. Cavern transit ecology & mineral precipitation
                else if (meta.Family == HydroGeologyRecordFamily.CaveBiota && otherMeta.Family == HydroGeologyRecordFamily.StalactiteMineral)
                {
                    related.Add(new HydroGeologyRelatedRecord(
                        otherId,
                        "speleological_matrix",
                        "Troglobitic habitat supported by subterranean mineral seepage"));
                }
            }

            return related.OrderBy(r => r.RecordId, StringComparer.Ordinal).ToList();
        }

        public HydroGeologyArchiveState CaptureState()
        {
            var copy = new HydroGeologyArchiveState
            {
                systemId = _state.systemId,
                discoveredRecordIds = new List<string>(_state.discoveredRecordIds)
            };
            copy.discoveredRecordIds.Sort(StringComparer.Ordinal);
            return copy;
        }

        public void RestoreState(HydroGeologyArchiveState? state)
        {
            _state = new HydroGeologyArchiveState();
            if (state?.discoveredRecordIds != null)
            {
                foreach (var id in state.discoveredRecordIds)
                {
                    if (!string.IsNullOrWhiteSpace(id) && !_state.discoveredRecordIds.Contains(id, StringComparer.OrdinalIgnoreCase))
                    {
                        _state.discoveredRecordIds.Add(id);
                    }
                }
            }
            _state.discoveredRecordIds.Sort(StringComparer.Ordinal);
            OnStateChanged?.Invoke();
        }
    }
}
