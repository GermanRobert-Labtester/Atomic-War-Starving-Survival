// SPDX-License-Identifier: MIT
// ASHFALL Core: Content Utilization Instrumentation
//
// Lightweight runtime instrumentation for content utilization tracking.
// Side-effect free — does not consume RNG, alter state, or change timing.
// Designed for diagnostic/self-test mode; disabled during normal gameplay.

using System;
using System.Collections.Generic;

namespace Ashfall.Core.Content
{
    /// <summary>
    /// Event raised when content is utilized at runtime.
    /// </summary>
    [Serializable]
    public sealed class UtilizationEvent
    {
        public UtilizationStage Stage { get; set; }
        public string Catalog { get; set; } = string.Empty;
        public string DefinitionId { get; set; } = string.Empty;
        public string Loader { get; set; } = string.Empty;
        public string Registry { get; set; } = string.Empty;
        public string Query { get; set; } = string.Empty;
        public string ConsumerSystem { get; set; } = string.Empty;
        public int CampaignDay { get; set; }
        public string Context { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
    }

    /// <summary>
    /// Lightweight instrumentation hub for content utilization tracking.
    /// Disabled by default; enabled for diagnostic/self-test runs.
    /// </summary>
    public sealed class ContentUtilizationInstrumentation
    {
        private bool _enabled;
        private readonly List<UtilizationEvent> _events = new List<UtilizationEvent>();
        private readonly HashSet<string> _queriedCatalogs = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> _queriedDefinitions = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> _selectedDefinitions = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> _consumedDefinitions = new HashSet<string>(StringComparer.Ordinal);

        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                if (!_enabled)
                {
                    _events.Clear();
                    _queriedCatalogs.Clear();
                    _queriedDefinitions.Clear();
                    _selectedDefinitions.Clear();
                    _consumedDefinitions.Clear();
                }
            }
        }

        public IReadOnlyList<UtilizationEvent> Events => _events;
        public IReadOnlyCollection<string> QueriedCatalogs => _queriedCatalogs;
        public IReadOnlyCollection<string> QueriedDefinitions => _queriedDefinitions;
        public IReadOnlyCollection<string> SelectedDefinitions => _selectedDefinitions;
        public IReadOnlyCollection<string> ConsumedDefinitions => _consumedDefinitions;

        public int EventCount => _events.Count;

        /// <summary>Record a catalog open event.</summary>
        public void RecordCatalogOpened(string catalog, string loader)
        {
            if (!_enabled) return;
            _events.Add(new UtilizationEvent
            {
                Stage = UtilizationStage.LOADED,
                Catalog = catalog,
                Loader = loader
            });
        }

        /// <summary>Record a catalog deserialization event.</summary>
        public void RecordCatalogDeserialized(string catalog, int definitionCount)
        {
            if (!_enabled) return;
            _events.Add(new UtilizationEvent
            {
                Stage = UtilizationStage.DESERIALIZED,
                Catalog = catalog,
                Context = $"definitions={definitionCount}"
            });
        }

        /// <summary>Record definitions registered into a registry.</summary>
        public void RecordDefinitionsRegistered(string catalog, string registry, int count)
        {
            if (!_enabled) return;
            _events.Add(new UtilizationEvent
            {
                Stage = UtilizationStage.REGISTERED,
                Catalog = catalog,
                Registry = registry,
                Context = $"count={count}"
            });
        }

        /// <summary>Record a definition query.</summary>
        public void RecordDefinitionQueried(string catalog, string definitionId, string query, string consumerSystem, int campaignDay = 0)
        {
            if (!_enabled) return;
            _queriedCatalogs.Add(catalog);
            if (!string.IsNullOrEmpty(definitionId)) _queriedDefinitions.Add(definitionId);
            _events.Add(new UtilizationEvent
            {
                Stage = UtilizationStage.QUERIED,
                Catalog = catalog,
                DefinitionId = definitionId,
                Query = query,
                ConsumerSystem = consumerSystem,
                CampaignDay = campaignDay
            });
        }

        /// <summary>Record a definition selection/activation (e.g. quest started, encounter chosen).</summary>
        public void RecordDefinitionSelected(string catalog, string definitionId, string consumerSystem, int campaignDay = 0)
        {
            if (!_enabled) return;
            _selectedDefinitions.Add(definitionId);
            _events.Add(new UtilizationEvent
            {
                Stage = UtilizationStage.SELECTED,
                Catalog = catalog,
                DefinitionId = definitionId,
                ConsumerSystem = consumerSystem,
                CampaignDay = campaignDay
            });
        }

        /// <summary>Record a definition that produced a measurable effect.</summary>
        public void RecordDefinitionConsumed(string catalog, string definitionId, string consumerSystem, string result, int campaignDay = 0)
        {
            if (!_enabled) return;
            _consumedDefinitions.Add(definitionId);
            _events.Add(new UtilizationEvent
            {
                Stage = UtilizationStage.EFFECT_PRODUCED,
                Catalog = catalog,
                DefinitionId = definitionId,
                ConsumerSystem = consumerSystem,
                Result = result,
                CampaignDay = campaignDay
            });
        }

        /// <summary>Check if a catalog was queried at runtime.</summary>
        public bool WasCatalogQueried(string catalog) => _queriedCatalogs.Contains(catalog);

        /// <summary>Check if a definition was queried at runtime.</summary>
        public bool WasDefinitionQueried(string definitionId) => _queriedDefinitions.Contains(definitionId);

        /// <summary>Check if a definition was selected/activated.</summary>
        public bool WasDefinitionSelected(string definitionId) => _selectedDefinitions.Contains(definitionId);

        /// <summary>Check if a definition produced an effect.</summary>
        public bool WasDefinitionConsumed(string definitionId) => _consumedDefinitions.Contains(definitionId);

        /// <summary>Clear all events (for deterministic replay).</summary>
        public void Clear()
        {
            _events.Clear();
            _queriedCatalogs.Clear();
            _queriedDefinitions.Clear();
            _selectedDefinitions.Clear();
            _consumedDefinitions.Clear();
        }

        /// <summary>Merge instrumentation data into a utilization graph.</summary>
        public void MergeInto(ContentUtilizationGraph graph)
        {
            // Per-catalog: which definition ids were selected/consumed, so a
            // catalog's MaxStage reflects the highest stage actually observed
            // for content that belongs to it (not just "was queried at all").
            var selectedByCatalog = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
            var consumedByCatalog = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
            foreach (var ev in _events)
            {
                if (string.IsNullOrEmpty(ev.Catalog) || string.IsNullOrEmpty(ev.DefinitionId)) continue;
                if (ev.Stage == UtilizationStage.SELECTED)
                {
                    if (!selectedByCatalog.TryGetValue(ev.Catalog, out var set))
                        selectedByCatalog[ev.Catalog] = set = new HashSet<string>(StringComparer.Ordinal);
                    set.Add(ev.DefinitionId);
                }
                else if (ev.Stage == UtilizationStage.EFFECT_PRODUCED)
                {
                    if (!consumedByCatalog.TryGetValue(ev.Catalog, out var set))
                        consumedByCatalog[ev.Catalog] = set = new HashSet<string>(StringComparer.Ordinal);
                    set.Add(ev.DefinitionId);
                }
            }

            foreach (var cat in graph.Catalogs)
            {
                if (WasCatalogQueried(cat.Path))
                {
                    cat.MaxStage = UtilizationStage.QUERIED;
                    cat.BestEvidence = EvidenceTier.RUNTIME;
                    cat.QueriedCount = 1;
                }
                if (selectedByCatalog.TryGetValue(cat.Path, out var selectedIds) && selectedIds.Count > 0)
                {
                    cat.MaxStage = UtilizationStage.SELECTED;
                    cat.BestEvidence = EvidenceTier.RUNTIME;
                }
                if (consumedByCatalog.TryGetValue(cat.Path, out var consumedIds) && consumedIds.Count > 0)
                {
                    cat.MaxStage = UtilizationStage.EFFECT_PRODUCED;
                    cat.BestEvidence = EvidenceTier.RUNTIME;
                }
            }
        }
    }
}
