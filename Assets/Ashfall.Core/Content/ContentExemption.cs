// SPDX-License-Identifier: MIT
// ASHFALL Core: Content Exemption System
//
// Manually exempted content that is intentionally non-consumed.
// Every exemption requires owner, classification, and rationale.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Content
{
    [Serializable]
    public sealed class ContentExemption
    {
        public string ExemptionId { get; set; } = string.Empty;
        public string ContentPath { get; set; } = string.Empty;
        public string DefinitionId { get; set; } = string.Empty;
        public string Owner { get; set; } = string.Empty;
        public string Classification { get; set; } = string.Empty; // e.g. OPTIONAL, TEST_ONLY, FUTURE
        public string Rationale { get; set; } = string.Empty;
        public string TrackingTicket { get; set; } = string.Empty;
        public string ExpiryCondition { get; set; } = string.Empty;

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(ExemptionId)
                && !string.IsNullOrEmpty(ContentPath)
                && !string.IsNullOrEmpty(Owner)
                && !string.IsNullOrEmpty(Classification)
                && !string.IsNullOrEmpty(Rationale);
        }

        public bool IsStale(ContentUtilizationGraph graph)
        {
            // Stale if the content it references no longer exists
            if (!string.IsNullOrEmpty(ContentPath))
            {
                // Check if it's a directory prefix (e.g. "narrative/")
                bool isDirectory = ContentPath.EndsWith("/");
                if (isDirectory)
                {
                    // Directory prefix exemptions are stale if no catalogs match
                    bool anyMatch = graph.Catalogs.Any(c =>
                        c.Path.StartsWith(ContentPath, StringComparison.OrdinalIgnoreCase));
                    return !anyMatch;
                }

                // Exact path match
                bool exists = graph.Catalogs.Any(c =>
                    string.Equals(c.Path, ContentPath, StringComparison.OrdinalIgnoreCase));
                if (!exists && !string.IsNullOrEmpty(DefinitionId))
                {
                    exists = graph.Definitions.Any(d =>
                        string.Equals(d.Id, DefinitionId, StringComparison.OrdinalIgnoreCase));
                }
                return !exists;
            }
            return false;
        }
    }

    [Serializable]
    public sealed class ExemptionRegistry
    {
        public string SchemaVersion { get; set; } = "1.0.0";
        public List<ContentExemption> Exemptions { get; set; } = new List<ContentExemption>();

        public bool TryGetExemption(string contentPath, out ContentExemption exemption)
        {
            exemption = Exemptions.FirstOrDefault(e =>
                string.Equals(e.ContentPath, contentPath, StringComparison.OrdinalIgnoreCase));
            return exemption != null;
        }

        public bool TryGetExemptionForDefinition(string definitionId, out ContentExemption exemption)
        {
            exemption = Exemptions.FirstOrDefault(e =>
                string.Equals(e.DefinitionId, definitionId, StringComparison.OrdinalIgnoreCase));
            return exemption != null;
        }

        public List<ContentExemption> GetInvalidExemptions()
        {
            return Exemptions.Where(e => !e.IsValid()).ToList();
        }

        public List<ContentExemption> GetStaleExemptions(ContentUtilizationGraph graph)
        {
            return Exemptions.Where(e => e.IsStale(graph)).ToList();
        }

        public List<ContentExemption> GetByOwner(string owner)
        {
            return Exemptions.Where(e =>
                string.Equals(e.Owner, owner, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }

    /// <summary>
    /// Default exemptions for known non-consumed content.
    /// These are intentionally non-consumed: narrative flavor text, future expansions,
    /// or content that is served through the codex/journal rather than gameplay systems.
    /// </summary>
    public static class DefaultExemptions
    {
        public static ExemptionRegistry CreateDefault()
        {
            var registry = new ExemptionRegistry();

            // All 68+ narrative subdirectory catalogs are codex-only — they are
            // flavor text served through the JournalCodex, not gameplay systems.
            // They are intentionally not consumed by runtime gameplay systems.
            registry.Exemptions.Add(new ContentExemption
            {
                ExemptionId = "exempt_narrative_codex",
                ContentPath = "narrative/",
                Owner = "narrative",
                Classification = "CODEX_ONLY",
                Rationale = "Narrative subdirectory files are flavor text served through the JournalCodex, not gameplay systems. They are intentionally codex-only content.",
                TrackingTicket = "TICKET-127"
            });

            // Whitelist files are infrastructure, not gameplay content
            registry.Exemptions.Add(new ContentExemption
            {
                ExemptionId = "exempt_whitelist_infra",
                ContentPath = "whitelists/",
                Owner = "content-pipeline",
                Classification = "OPTIONAL",
                Rationale = "Whitelist files are infrastructure configuration, not gameplay content.",
                TrackingTicket = "TICKET-127"
            });

            // Documents are supplementary, not core gameplay
            registry.Exemptions.Add(new ContentExemption
            {
                ExemptionId = "exempt_documents_supplementary",
                ContentPath = "documents/",
                Owner = "narrative",
                Classification = "OPTIONAL",
                Rationale = "Documents directory contains supplementary data, not core gameplay catalogs.",
                TrackingTicket = "TICKET-127"
            });

            // echoes.json is future narrative content, no loader or consumer yet
            registry.Exemptions.Add(new ContentExemption
            {
                ExemptionId = "exempt_echoes_future",
                ContentPath = "echoes.json",
                Owner = "narrative",
                Classification = "OPTIONAL",
                Rationale = "echoes.json contains future narrative echo content. No loader or consumer exists yet. Intentionally deferred.",
                TrackingTicket = "TICKET-127",
                ExpiryCondition = "When EchoSystem is implemented and wired"
            });

            return registry;
        }
    }
}