// SPDX-License-Identifier: MIT
// ASHFALL Core — Plan 157 Grain Milling Discovery & Archive System
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Narrative
{
    /// <summary>Serialized discovery bookkeeping for the grain milling archive. Record IDs only.</summary>
    [Serializable]
    public sealed class GrainMillingArchiveState
    {
        public string systemId = GrainMillingDiscoverySystem.SystemId;
        public List<string> discoveredRecordIds = new List<string>();
    }

    /// <summary>Deterministic cross-record industrial relation between discovered milling records.</summary>
    public sealed class GrainMillingRelatedRecord
    {
        public string RecordId { get; }
        public string RelationKind { get; }
        public string Detail { get; }

        public GrainMillingRelatedRecord(string recordId, string relationKind, string detail)
        {
            RecordId = recordId;
            RelationKind = relationKind;
            Detail = detail;
        }
    }

    /// <summary>
    /// Plan 157: Read-only industrial food-processing discovery projection over GrainMillingCatalog
    /// (30 records across: millstone dressing, bolting silk, silo weevil, dampener tempering).
    ///
    /// Authority Firewall (Plan 157 §4 &amp; Invariant 5):
    /// - Exposes ONLY: producer registration, discovery tracking, and derived read-only relations.
    /// - Contains NO methods to mutate grain inventory, flour yields, meal nutrition, silo spoilage rates,
    ///   granary temperature, or commodity barter values.
    /// - Discovery is idempotent; first discovery triggers events; repeat discovery is a no-op.
    /// - Related records only link already-discovered records — never spoiling undiscovered entries.
    /// </summary>
    public sealed class GrainMillingDiscoverySystem
    {
        public const string SystemId = "grain_milling_archive";

        private readonly GrainMillingCatalog _catalog;
        private readonly Func<string, bool>? _locationExists;
        private readonly Dictionary<string, string> _producerByRecordId =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, List<string>> _recordsByProducer =
            new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        private GrainMillingArchiveState _state = new GrainMillingArchiveState();

        /// <summary>Raised once per record on its initial discovery.</summary>
        public event Action<string, GrainMillingRecordFamily>? OnRecordFirstDiscovered;
        public event Action? OnStateChanged;

        public GrainMillingDiscoverySystem(GrainMillingCatalog catalog, Func<string, bool>? locationExists = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _locationExists = locationExists;
            InitializeDefaultProducers();
        }

        public GrainMillingCatalog Catalog => _catalog;
        public GrainMillingArchiveState State => _state;

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
            foreach (var meta in GrainMillingProjection.GetAllMetadata())
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

            if (_catalog.GetAny(recordId) == null)
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
            foreach (var meta in GrainMillingProjection.GetAllMetadata())
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

            var meta = GrainMillingProjection.GetMetadata(recordId);
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

        public IReadOnlyList<GrainMillingRelatedRecord> GetRelated(string recordId)
        {
            var related = new List<GrainMillingRelatedRecord>();
            if (!IsDiscovered(recordId)) return related;

            var meta = GrainMillingProjection.GetMetadata(recordId);
            if (meta == null) return related;

            // Epistemic Privacy Firewall: ONLY surface relations to records that are ALREADY discovered.
            foreach (var otherId in DiscoveredRecordIds)
            {
                if (string.Equals(otherId, recordId, StringComparison.OrdinalIgnoreCase)) continue;

                var otherMeta = GrainMillingProjection.GetMetadata(otherId);
                if (otherMeta == null) continue;

                // 1. Explicit cross-reference defined in projection
                if (meta.RelatedRecordIds.Contains(otherId, StringComparer.OrdinalIgnoreCase) ||
                    otherMeta.RelatedRecordIds.Contains(recordId, StringComparer.OrdinalIgnoreCase))
                {
                    related.Add(new GrainMillingRelatedRecord(
                        otherId,
                        "process_dependency",
                        $"Cross-process dependency: {meta.NamedSubject} <-> {otherMeta.NamedSubject}"));
                }
                // 2. Co-located producer site
                else if (string.Equals(meta.ProducerId, otherMeta.ProducerId, StringComparison.OrdinalIgnoreCase))
                {
                    related.Add(new GrainMillingRelatedRecord(
                        otherId,
                        "co_located_facility",
                        $"Co-located milling equipment observation at {meta.ProducerId}"));
                }
                // 3. Shared grain/crop material
                else if (!string.IsNullOrWhiteSpace(meta.CropOrMaterial) &&
                         string.Equals(meta.CropOrMaterial, otherMeta.CropOrMaterial, StringComparison.OrdinalIgnoreCase))
                {
                    related.Add(new GrainMillingRelatedRecord(
                        otherId,
                        "shared_crop_specimen",
                        $"Shared grain specimen lineage ({meta.CropOrMaterial})"));
                }
            }

            return related.OrderBy(r => r.RecordId, StringComparer.Ordinal).ToList();
        }

        public GrainMillingArchiveState CaptureState()
        {
            var copy = new GrainMillingArchiveState
            {
                systemId = _state.systemId,
                discoveredRecordIds = new List<string>(_state.discoveredRecordIds)
            };
            copy.discoveredRecordIds.Sort(StringComparer.Ordinal);
            return copy;
        }

        public void RestoreState(GrainMillingArchiveState? state)
        {
            _state = new GrainMillingArchiveState();
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
