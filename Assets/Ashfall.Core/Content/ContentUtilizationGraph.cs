// SPDX-License-Identifier: MIT
// ASHFALL Core: Content Utilization Graph
//
// Ticket #127 — Evidence-backed content-runtime utilization graph.
// Models content pipeline stages: DISCOVERED → LOADED → DESERIALIZED →
// REGISTERED → QUERIED → SELECTED → EFFECT_PRODUCED.
//
// Each stage, edge, and classification carries explicit evidence.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Content
{
    // ── Utilization Stages ──────────────────────────────────────────

    public enum UtilizationStage
    {
        /// <summary>File exists on disk.</summary>
        DISCOVERED,

        /// <summary>Loader opened the file.</summary>
        LOADED,

        /// <summary>Content deserialized from JSON.</summary>
        DESERIALIZED,

        /// <summary>Definitions registered in a lookup structure.</summary>
        REGISTERED,

        /// <summary>At least one production runtime query targets this content.</summary>
        QUERIED,

        /// <summary>Definition was selected/activated/consumed by gameplay.</summary>
        SELECTED,

        /// <summary>Definition produced a measurable gameplay/UI/narrative effect.</summary>
        EFFECT_PRODUCED
    }

    // ── Node Kinds ──────────────────────────────────────────────────

    public enum ContentNodeKind
    {
        ContentRoot,
        ContentFile,
        Definition,
        Loader,
        Registry,
        Query,
        RuntimeSystem,
        UiSurface,
        CodexSurface,
        Test,
        Exemption
    }

    // ── Edge Kinds ──────────────────────────────────────────────────

    public enum ContentEdgeKind
    {
        CONTAINS,
        LOADED_BY,
        DESERIALIZED_BY,
        REGISTERED_IN,
        QUERIED_BY,
        SELECTED_BY,
        CONSUMED_BY,
        DISPLAYED_BY,
        UNLOCKED_BY,
        GATED_BY,
        REFERENCES,
        SPAWNS,
        ACTIVATES,
        TESTED_BY,
        FALLBACK_FOR,
        SUPERSEDES,
        EXEMPTED_BY
    }

    // ── Evidence Tiers ──────────────────────────────────────────────

    public enum EvidenceTier
    {
        /// <summary>Static code analysis — architectural connectivity.</summary>
        STATIC,

        /// <summary>Runtime observation — actually observed during execution.</summary>
        RUNTIME,

        /// <summary>Test fixture exercised the path.</summary>
        TEST,

        /// <summary>Configuration or registration file.</summary>
        CONFIG,

        /// <summary>Explicit manual exemption.</summary>
        MANUAL_EXEMPTION,

        /// <summary>Derived from other evidence (transitive).</summary>
        DERIVED
    }

    // ── Classification ──────────────────────────────────────────────

    public enum ContentClassification
    {
        /// <summary>Observed or proven reachable through production runtime consumers.</summary>
        GAMEPLAY_CONSUMED,

        /// <summary>Consumed only by UI/presentation, not gameplay rules.</summary>
        UI_ONLY,

        /// <summary>Consumed only by codex/journal/encyclopedia.</summary>
        CODEX_ONLY,

        /// <summary>Used only by test fixtures.</summary>
        TEST_ONLY,

        /// <summary>Intentionally optional (expansion, scenario, mod).</summary>
        OPTIONAL,

        /// <summary>No legitimate production consumer can be demonstrated.</summary>
        ORPHANED,

        /// <summary>Not yet classified.</summary>
        UNRESOLVED
    }

    // ── Reachability Status ─────────────────────────────────────────

    public enum ReachabilityStatus
    {
        REACHABLE,
        REACHABLE_BUT_NOT_OBSERVED,
        GATED,
        UNREACHABLE,
        BROKEN_GATE,
        UNKNOWN
    }

    // ── Confidence ──────────────────────────────────────────────────

    public enum ConfidenceLevel
    {
        OBSERVED_RUNTIME,
        PROVEN_STATIC_REACHABILITY,
        POTENTIAL_STATIC_REACHABILITY,
        TEST_ONLY_OBSERVED,
        UNVERIFIED
    }

    // ── Graph Node ──────────────────────────────────────────────────

    [Serializable]
    public sealed class ContentNode
    {
        /// <summary>Stable deterministic identity (e.g. "file:data/quests/foo.json").</summary>
        public string Id { get; set; } = string.Empty;

        public ContentNodeKind Kind { get; set; }

        /// <summary>Human-readable label.</summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>Additional metadata as key-value pairs.</summary>
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

        public ContentNode() { }

        public ContentNode(string id, ContentNodeKind kind, string label)
        {
            Id = id;
            Kind = kind;
            Label = label;
        }
    }

    // ── Graph Edge ──────────────────────────────────────────────────

    [Serializable]
    public sealed class ContentEdge
    {
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public ContentEdgeKind Kind { get; set; }
        public EvidenceTier Evidence { get; set; }

        /// <summary>Optional context (e.g. method name, condition).</summary>
        public string Context { get; set; } = string.Empty;

        public ContentEdge() { }

        public ContentEdge(string from, string to, ContentEdgeKind kind, EvidenceTier evidence, string context = "")
        {
            From = from;
            To = to;
            Kind = kind;
            Evidence = evidence;
            Context = context;
        }
    }

    // ── Catalog Entry ───────────────────────────────────────────────

    [Serializable]
    public sealed class CatalogEntry
    {
        /// <summary>Relative path from data root (e.g. "data/quests/foo.json").</summary>
        public string Path { get; set; } = string.Empty;

        public ContentClassification Classification { get; set; } = ContentClassification.UNRESOLVED;

        public string Loader { get; set; } = string.Empty;

        public int DefinitionCount { get; set; }

        public int RegisteredCount { get; set; }

        public int QueriedCount { get; set; }

        public int ReachableCount { get; set; }

        public UtilizationStage MaxStage { get; set; } = UtilizationStage.DISCOVERED;

        public EvidenceTier BestEvidence { get; set; } = EvidenceTier.STATIC;

        public string ExemptionId { get; set; } = string.Empty;

        public List<string> ConsumerSystems { get; set; } = new List<string>();

        public List<string> Findings { get; set; } = new List<string>();
    }

    // ── Definition Entry ────────────────────────────────────────────

    [Serializable]
    public sealed class DefinitionEntry
    {
        public string Id { get; set; } = string.Empty;

        public string Catalog { get; set; } = string.Empty;

        public ContentClassification Classification { get; set; } = ContentClassification.UNRESOLVED;

        public ReachabilityStatus Reachability { get; set; } = ReachabilityStatus.UNKNOWN;

        public ConfidenceLevel Confidence { get; set; } = ConfidenceLevel.UNVERIFIED;

        public List<string> Consumers { get; set; } = new List<string>();

        public List<string> Gates { get; set; } = new List<string>();

        public List<EvidenceTier> Evidence { get; set; } = new List<EvidenceTier>();

        public string Notes { get; set; } = string.Empty;
    }

    // ── Content Family Summary ──────────────────────────────────────

    [Serializable]
    public sealed class ContentFamilySummary
    {
        public string Family { get; set; } = string.Empty;
        public int Catalogs { get; set; }
        public int Definitions { get; set; }
        public int GameplayConsumed { get; set; }
        public int UiOnly { get; set; }
        public int CodexOnly { get; set; }
        public int Optional { get; set; }
        public int TestOnly { get; set; }
        public int Orphaned { get; set; }
        public int Unresolved { get; set; }
        public int TestCovered { get; set; }
    }

    // ── Hardcoded Authority Finding ─────────────────────────────────

    [Serializable]
    public sealed class HardcodedAuthorityFinding
    {
        public string RuntimeSystem { get; set; } = string.Empty;
        public string HardcodedSource { get; set; } = string.Empty;
        public string AvailableCatalog { get; set; } = string.Empty;
        public bool JsonLoaded { get; set; }
        public bool RuntimeUsesJson { get; set; }
        public string RecommendedStatus { get; set; } = string.Empty;
    }

    // ── Disconnect Finding ──────────────────────────────────────────

    [Serializable]
    public sealed class DisconnectFinding
    {
        public string Catalog { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // NO_LOADER, LOADED_NOT_REGISTERED, etc.
        public UtilizationStage LastStage { get; set; }
        public string MissingLink { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }

    // ── Reachability Finding ────────────────────────────────────────

    [Serializable]
    public sealed class ReachabilityFinding
    {
        public string DefinitionId { get; set; } = string.Empty;
        public ReachabilityStatus Status { get; set; }
        public List<string> Gates { get; set; } = new List<string>();
        public string BrokenChain { get; set; } = string.Empty;
    }

    // ── Full Utilization Graph ──────────────────────────────────────

    [Serializable]
    public sealed class ContentUtilizationGraph
    {
        public string SchemaVersion { get; set; } = "1.0.0";
        public string GeneratedFromCommit { get; set; } = string.Empty;
        public string GeneratedAt { get; set; } = string.Empty;

        public List<string> ContentRoots { get; set; } = new List<string>();

        public List<ContentNode> Nodes { get; set; } = new List<ContentNode>();
        public List<ContentEdge> Edges { get; set; } = new List<ContentEdge>();

        public List<CatalogEntry> Catalogs { get; set; } = new List<CatalogEntry>();
        public List<DefinitionEntry> Definitions { get; set; } = new List<DefinitionEntry>();

        public List<ContentFamilySummary> FamilySummaries { get; set; } = new List<ContentFamilySummary>();

        public List<HardcodedAuthorityFinding> HardcodedAuthorities { get; set; } = new List<HardcodedAuthorityFinding>();
        public List<DisconnectFinding> Disconnects { get; set; } = new List<DisconnectFinding>();
        public List<ReachabilityFinding> ReachabilityFindings { get; set; } = new List<ReachabilityFinding>();

        // Summary counters
        public int TotalCatalogs { get; set; }
        public int TotalDefinitions { get; set; }
        public int GameplayConsumedCatalogs { get; set; }
        public int UiOnlyCatalogs { get; set; }
        public int CodexOnlyCatalogs { get; set; }
        public int OptionalCatalogs { get; set; }
        public int TestOnlyCatalogs { get; set; }
        public int OrphanedCatalogs { get; set; }
        public int UnresolvedCatalogs { get; set; }
        public int ExemptedCatalogs { get; set; }

        public void ComputeSummaries()
        {
            TotalCatalogs = Catalogs.Count;
            TotalDefinitions = Definitions.Count;
            GameplayConsumedCatalogs = Catalogs.Count(c => c.Classification == ContentClassification.GAMEPLAY_CONSUMED);
            UiOnlyCatalogs = Catalogs.Count(c => c.Classification == ContentClassification.UI_ONLY);
            CodexOnlyCatalogs = Catalogs.Count(c => c.Classification == ContentClassification.CODEX_ONLY);
            OptionalCatalogs = Catalogs.Count(c => c.Classification == ContentClassification.OPTIONAL);
            TestOnlyCatalogs = Catalogs.Count(c => c.Classification == ContentClassification.TEST_ONLY);
            OrphanedCatalogs = Catalogs.Count(c => c.Classification == ContentClassification.ORPHANED);
            UnresolvedCatalogs = Catalogs.Count(c => c.Classification == ContentClassification.UNRESOLVED);
            ExemptedCatalogs = Catalogs.Count(c => !string.IsNullOrEmpty(c.ExemptionId));
        }

        /// <summary>Stable sort for deterministic output.</summary>
        public void Stabilize()
        {
            Catalogs.Sort((a, b) => string.CompareOrdinal(a.Path, b.Path));
            Definitions.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
            Nodes.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
            Edges.Sort((a, b) =>
            {
                int c = string.CompareOrdinal(a.From, b.From);
                if (c != 0) return c;
                c = string.CompareOrdinal(a.To, b.To);
                if (c != 0) return c;
                return string.CompareOrdinal(a.Kind.ToString(), b.Kind.ToString());
            });
            Disconnects.Sort((a, b) => string.CompareOrdinal(a.Catalog, b.Catalog));
            ReachabilityFindings.Sort((a, b) => string.CompareOrdinal(a.DefinitionId, b.DefinitionId));
            HardcodedAuthorities.Sort((a, b) => string.CompareOrdinal(a.RuntimeSystem, b.RuntimeSystem));
            FamilySummaries.Sort((a, b) => string.CompareOrdinal(a.Family, b.Family));

            foreach (var cat in Catalogs)
            {
                cat.ConsumerSystems.Sort(StringComparer.Ordinal);
                cat.Findings.Sort(StringComparer.Ordinal);
            }
            foreach (var def in Definitions)
            {
                def.Consumers.Sort(StringComparer.Ordinal);
                def.Gates.Sort(StringComparer.Ordinal);
                def.Evidence.Sort();
            }
        }
    }
}