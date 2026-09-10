// SPDX-License-Identifier: MIT
// ASHFALL Core: Deep content-utilization chain gate (Plan 49 / Task 15).
//
// Upgrades utilization from "the file is consumed somewhere" to "the authored
// content has a complete, evidence-carrying chain from producer to
// player-observable surface". An offline graph verifier over the
// <see cref="ContentUtilizationGraph"/> emitted by the static scanner —
// never a startup/runtime component.
//
// Grammar:  Producer (authored catalog)
//        → Authority (loader / registry / domain system)
//        → Consumer  (gameplay system)
//        → Player-observable surface (panel / route / briefing).
//
// A flagship chain is healthy only when every mandatory edge exists, backed by
// the scanner's evidence (LOADED_BY / CONSUMED_BY / DISPLAYED_BY).

using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Content
{
    /// <summary>One mandatory hop inside a chain spec.</summary>
    [Serializable]
    public sealed class DeepChainHopSpec
    {
        /// <summary>Stable hop identifier (used in diagnostics).</summary>
        public string HopId { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        /// <summary>Catalog file that must exist as a producer node.</summary>
        public string RequiredFile { get; set; } = string.Empty;

        /// <summary>Loader the file must be LOADED_BY (optional).</summary>
        public string? RequiredLoader { get; set; }

        /// <summary>Consumer systems the file must be CONSUMED_BY (optional, any-of).</summary>
        public string[]? RequiredSystems { get; set; }

        /// <summary>UI surface the file must be DISPLAYED_BY (optional).</summary>
        public string? RequiredSurface { get; set; }
    }

    /// <summary>A flagship (or warn-tier) chain specification.</summary>
    [Serializable]
    public sealed class DeepChainSpec
    {
        public string ChainId { get; set; } = string.Empty;
        public string Narrative { get; set; } = string.Empty;

        /// <summary>Hard gates fail CI; warn-tier chains only warn (never block).</summary>
        public bool IsHardGate { get; set; }

        public DeepChainHopSpec[] Hops { get; set; } = Array.Empty<DeepChainHopSpec>();
    }

    [Serializable]
    public sealed class DeepChainFinding
    {
        public string ChainId { get; set; } = string.Empty;
        public string HopId { get; set; } = string.Empty;

        /// <summary>
        /// Missing-hop category: DATA_WITHOUT_LOADER, LOADER_WITHOUT_SYSTEM,
        /// CONSUMER_WITHOUT_SURFACE, PRODUCER_MISSING.
        /// </summary>
        public string MissingCategory { get; set; } = string.Empty;

        public string Details { get; set; } = string.Empty;
        public string Severity { get; set; } = "HARD"; // HARD | WARN
        public string RecommendedFix { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class DeepChainReport
    {
        public string SchemaVersion { get; set; } = "1.0.0";
        public List<DeepChainFinding> Findings { get; set; } = new();
        public int ChainsEvaluated { get; set; }
        public int HardFailures => Findings.Count(f => f.Severity == "HARD");
        public int Warnings => Findings.Count(f => f.Severity == "WARN");
        public bool HardGatePassed => HardFailures == 0;

        /// <summary>Deterministic ordinal ordering for artifacts.</summary>
        public void Stabilize() =>
            Findings.Sort((a, b) =>
            {
                int c = string.CompareOrdinal(a.ChainId, b.ChainId);
                if (c != 0) return c;
                c = string.CompareOrdinal(a.HopId, b.HopId);
                if (c != 0) return c;
                return string.CompareOrdinal(a.MissingCategory, b.MissingCategory);
            });
    }

    public static class ContentDeepChainGate
    {
        // ── Flagship chains (hard gates) ────────────────────────────

        /// <summary>
        /// Chain 1: Research → Breakthrough → Recipe → Craft.
        /// Required hops: research authored; loader; research system; recipe
        /// authored; crafting acceptance; inventory result; player surface.
        /// </summary>
        public static readonly DeepChainSpec ResearchToCraft = new()
        {
            ChainId = "research-breakthrough-recipe-craft",
            Narrative = "Authored research resolves through the knowledge catalog loader, " +
                        "the research system consumes it, unlockable recipes resolve through " +
                        "the recipe authority into crafting, results land in inventory, and " +
                        "a player surface displays the workflow.",
            IsHardGate = true,
            Hops = new[]
            {
                new DeepChainHopSpec
                {
                    HopId = "research_authored",
                    Description = "research_knowledge.json exists (producer)",
                    RequiredFile = "research_knowledge.json",
                },
                new DeepChainHopSpec
                {
                    HopId = "research_loaded",
                    Description = "research_knowledge.json is loaded by ResearchKnowledgeCatalogLoader (authority)",
                    RequiredFile = "research_knowledge.json",
                    RequiredLoader = "ResearchKnowledgeCatalogLoader",
                },
                new DeepChainHopSpec
                {
                    HopId = "research_system_consumes",
                    Description = "ResearchSystem consumes research_knowledge.json (consumer)",
                    RequiredFile = "research_knowledge.json",
                    RequiredSystems = new[] { "ResearchSystem" },
                },
                new DeepChainHopSpec
                {
                    HopId = "recipe_authored",
                    Description = "recipes.json exists (producer for the craft leg)",
                    RequiredFile = "recipes.json",
                },
                new DeepChainHopSpec
                {
                    HopId = "recipe_loaded",
                    Description = "recipes.json is loaded by RecipeCatalogLoader (authority)",
                    RequiredFile = "recipes.json",
                    RequiredLoader = "RecipeCatalogLoader",
                },
                new DeepChainHopSpec
                {
                    HopId = "crafting_accepts",
                    Description = "CraftingSystem consumes recipes.json (consumer)",
                    RequiredFile = "recipes.json",
                    RequiredSystems = new[] { "CraftingSystem" },
                },
                new DeepChainHopSpec
                {
                    HopId = "inventory_result",
                    Description = "InventorySystem consumes items.json — craft results land somewhere usable (consumer)",
                    RequiredFile = "items.json",
                    RequiredSystems = new[] { "InventorySystem" },
                },
                new DeepChainHopSpec
                {
                    HopId = "player_surface",
                    Description = "ResearchPanel displays research_knowledge.json (player-observable surface)",
                    RequiredFile = "research_knowledge.json",
                    RequiredSurface = "ResearchPanel",
                },
            },
        };

        /// <summary>
        /// Chain 2: Expedition → Loot → Inventory → Use.
        /// </summary>
        public static readonly DeepChainSpec ExpeditionToUse = new()
        {
            ChainId = "expedition-loot-inventory-use",
            Narrative = "Expedition content resolves through its loader into the expedition " +
                        "system, encounter/loot resolution is bridged, loot lands in the " +
                        "inventory authority, and the expedition briefing surface is reachable.",
            IsHardGate = true,
            Hops = new[]
            {
                new DeepChainHopSpec
                {
                    HopId = "expedition_authored",
                    Description = "expeditions.json exists (producer)",
                    RequiredFile = "expeditions.json",
                },
                new DeepChainHopSpec
                {
                    HopId = "expedition_loaded",
                    Description = "expeditions.json is loaded by ExpeditionCatalogLoader (authority)",
                    RequiredFile = "expeditions.json",
                    RequiredLoader = "ExpeditionCatalogLoader",
                },
                new DeepChainHopSpec
                {
                    HopId = "expedition_system_consumes",
                    Description = "ExpeditionSystem consumes expeditions.json (consumer)",
                    RequiredFile = "expeditions.json",
                    RequiredSystems = new[] { "ExpeditionSystem" },
                },
                new DeepChainHopSpec
                {
                    HopId = "loot_resolution_bridged",
                    Description = "ExpeditionEncounterBridge participates in loot resolution (consumer)",
                    RequiredFile = "expeditions.json",
                    RequiredSystems = new[] { "ExpeditionEncounterBridge" },
                },
                new DeepChainHopSpec
                {
                    HopId = "inventory_insertion",
                    Description = "InventorySystem consumes items.json — loot insertion authority (consumer)",
                    RequiredFile = "items.json",
                    RequiredSystems = new[] { "InventorySystem" },
                },
                new DeepChainHopSpec
                {
                    HopId = "player_surface",
                    Description = "ExpeditionPanel displays expeditions.json (player-observable surface)",
                    RequiredFile = "expeditions.json",
                    RequiredSurface = "ExpeditionPanel",
                },
            },
        };

        /// <summary>
        /// Chain 3: Faction → Treaty → World Flag/Consequence → Briefing.
        /// </summary>
        public static readonly DeepChainSpec FactionTreatyBriefing = new()
        {
            ChainId = "faction-treaty-flag-briefing",
            Narrative = "Authored treaties resolve through the treaty loader into the summit " +
                        "system, consequences flow into the foundry consequence policy (world " +
                        "state), and an observable surface reacts.",
            IsHardGate = true,
            Hops = new[]
            {
                new DeepChainHopSpec
                {
                    HopId = "treaty_authored",
                    Description = "diplomatic_treaties.json exists (producer)",
                    RequiredFile = "diplomatic_treaties.json",
                },
                new DeepChainHopSpec
                {
                    HopId = "treaty_loaded",
                    Description = "diplomatic_treaties.json is loaded by DiplomaticTreatyCatalogLoader (authority)",
                    RequiredFile = "diplomatic_treaties.json",
                    RequiredLoader = "DiplomaticTreatyCatalogLoader",
                },
                new DeepChainHopSpec
                {
                    HopId = "treaty_state_consumed",
                    Description = "DiplomaticSummitSystem consumes diplomatic_treaties.json (consumer)",
                    RequiredFile = "diplomatic_treaties.json",
                    RequiredSystems = new[] { "DiplomaticSummitSystem" },
                },
                new DeepChainHopSpec
                {
                    HopId = "consequence_authority",
                    Description = "foundry_treaty_consequences.json resolves through SilentFoundryConsequencePolicy (world-state authority)",
                    RequiredFile = "foundry_treaty_consequences.json",
                    RequiredLoader = "SilentFoundryConsequencePolicy",
                },
                new DeepChainHopSpec
                {
                    HopId = "consequence_system",
                    Description = "SilentFoundrySystem consumes foundry_treaty_consequences.json (consumer)",
                    RequiredFile = "foundry_treaty_consequences.json",
                    RequiredSystems = new[] { "SilentFoundrySystem" },
                },
                new DeepChainHopSpec
                {
                    HopId = "briefing_surface",
                    Description = "FoundryPanel displays foundry_treaty_consequences.json (player-observable surface)",
                    RequiredFile = "foundry_treaty_consequences.json",
                    RequiredSurface = "FoundryPanel",
                },
            },
        };

        // ── Warn-tier chains (never hard-fail) ──────────────────────

        /// <summary>
        /// Plan 49 Phase 15L warn-tier expansion: radio, shelter schedules
        /// (duty roster), foundry, greenhouse. Warn-tier findings never block CI.
        /// </summary>
        public static readonly DeepChainSpec[] WarnTierChains = new[]
        {
            new DeepChainSpec
            {
                ChainId = "warn-radio-broadcast",
                Narrative = "Radio broadcasts load, resolve through the radio runtime and surface to the player.",
                IsHardGate = false,
                Hops = new[]
                {
                    new DeepChainHopSpec { HopId = "authored", Description = "radio.json exists", RequiredFile = "radio.json" },
                    new DeepChainHopSpec { HopId = "consumed", Description = "a radio runtime system consumes radio.json", RequiredFile = "radio.json", RequiredSystems = new[] { "RadioHostSession", "RadioBroadcastSystem", "RadioSystem" } },
                },
            },
            new DeepChainSpec
            {
                ChainId = "warn-duty-roster-schedules",
                Narrative = "Duty roster locations load into the roster catalog and surface in the roster UI.",
                IsHardGate = false,
                Hops = new[]
                {
                    new DeepChainHopSpec { HopId = "authored", Description = "duty_roster_locations.json exists", RequiredFile = "duty_roster_locations.json" },
                    new DeepChainHopSpec { HopId = "consumed", Description = "DutyRosterSystem consumes duty_roster_locations.json", RequiredFile = "duty_roster_locations.json", RequiredSystems = new[] { "DutyRosterSystem" } },
                },
            },
            new DeepChainSpec
            {
                ChainId = "warn-foundry-production",
                Narrative = "Foundry production catalogs load into SilentFoundrySystem and surface on FoundryPanel.",
                IsHardGate = false,
                Hops = new[]
                {
                    new DeepChainHopSpec { HopId = "authored", Description = "foundry_treaty_consequences.json exists", RequiredFile = "foundry_treaty_consequences.json" },
                    new DeepChainHopSpec { HopId = "consumed", Description = "SilentFoundrySystem consumes the consequence policy", RequiredFile = "foundry_treaty_consequences.json", RequiredSystems = new[] { "SilentFoundrySystem" } },
                    new DeepChainHopSpec { HopId = "surface", Description = "FoundryPanel displays it", RequiredFile = "foundry_treaty_consequences.json", RequiredSurface = "FoundryPanel" },
                },
            },
            new DeepChainSpec
            {
                ChainId = "warn-greenhouse-crops",
                Narrative = "Greenhouse crop catalogs load into the greenhouse runtime and surface in the greenhouse UI.",
                IsHardGate = false,
                Hops = new[]
                {
                    new DeepChainHopSpec { HopId = "authored", Description = "hydroponic_crops.json exists", RequiredFile = "hydroponic_crops.json" },
                    new DeepChainHopSpec { HopId = "consumed", Description = "HydroponicBiomeSystem consumes hydroponic_crops.json", RequiredFile = "hydroponic_crops.json", RequiredSystems = new[] { "HydroponicBiomeSystem" } },
                },
            },
            new DeepChainSpec
            {
                ChainId = "warn-shelter-schedules",
                Narrative = "Shelter schedule content (duty roster seasons) loads and is consumed by the schedule runtime.",
                IsHardGate = false,
                Hops = new[]
                {
                    new DeepChainHopSpec { HopId = "authored", Description = "duty_roster_seasons.json exists", RequiredFile = "duty_roster_seasons.json" },
                    new DeepChainHopSpec { HopId = "consumed", Description = "DutyRosterSystem consumes duty_roster_seasons.json", RequiredFile = "duty_roster_seasons.json", RequiredSystems = new[] { "DutyRosterSystem" } },
                },
            },
        };

        /// <summary>All gated chains (flagship + warn-tier).</summary>
        public static IEnumerable<DeepChainSpec> AllChains =>
            new[] { ResearchToCraft, ExpeditionToUse, FactionTreatyBriefing }.Concat(WarnTierChains);

        // ── Evaluation ──────────────────────────────────────────────

        /// <summary>
        /// Verifies every chain hop against the scanner's evidence graph.
        /// Diagnostic always names the exact missing hop.
        /// </summary>
        public static DeepChainReport Evaluate(ContentUtilizationGraph graph)
        {
            var report = new DeepChainReport();

            foreach (var chain in AllChains)
            {
                report.ChainsEvaluated++;
                foreach (var hop in chain.Hops)
                {
                    string severity = chain.IsHardGate ? "HARD" : "WARN";
                    string fileId = "file:" + hop.RequiredFile;

                    bool fileNode = graph.Nodes.Any(n => n.Id == fileId)
                                    || graph.Catalogs.Any(c => c.Path == hop.RequiredFile);
                    if (!fileNode)
                    {
                        report.Findings.Add(new DeepChainFinding
                        {
                            ChainId = chain.ChainId,
                            HopId = hop.HopId,
                            MissingCategory = "PRODUCER_MISSING",
                            Details = $"{hop.RequiredFile} is not present in the scanned graph",
                            Severity = severity,
                            RecommendedFix = $"Restore or re-register the producer catalog {hop.RequiredFile}.",
                        });
                        continue;
                    }

                    if (!string.IsNullOrEmpty(hop.RequiredLoader) &&
                        !graph.Edges.Any(e => e.From == fileId && e.Kind == ContentEdgeKind.LOADED_BY &&
                                              e.To == "loader:" + hop.RequiredLoader))
                    {
                        report.Findings.Add(new DeepChainFinding
                        {
                            ChainId = chain.ChainId,
                            HopId = hop.HopId,
                            MissingCategory = "DATA_WITHOUT_LOADER",
                            Details = $"{hop.RequiredFile} has no LOADED_BY edge to loader:{hop.RequiredLoader}",
                            Severity = severity,
                            RecommendedFix = $"Wire {hop.RequiredLoader} into the loader evidence map for {hop.RequiredFile}.",
                        });
                        continue;
                    }

                    if (hop.RequiredSystems != null &&
                        !hop.RequiredSystems.Any(sys =>
                            graph.Edges.Any(e => e.From == fileId && e.Kind == ContentEdgeKind.CONSUMED_BY &&
                                                 e.To == "system:" + sys)))
                    {
                        report.Findings.Add(new DeepChainFinding
                        {
                            ChainId = chain.ChainId,
                            HopId = hop.HopId,
                            MissingCategory = "LOADER_WITHOUT_SYSTEM",
                            Details = $"{hop.RequiredFile} has no CONSUMED_BY edge to any of: {string.Join(", ", hop.RequiredSystems.Select(s => "system:" + s))}",
                            Severity = severity,
                            RecommendedFix = $"Register {string.Join(" or ", hop.RequiredSystems)} as a live consumer of {hop.RequiredFile}.",
                        });
                        continue;
                    }

                    if (!string.IsNullOrEmpty(hop.RequiredSurface) &&
                        !graph.Edges.Any(e => e.From == fileId && e.Kind == ContentEdgeKind.DISPLAYED_BY &&
                                              e.To == "ui:" + hop.RequiredSurface))
                    {
                        report.Findings.Add(new DeepChainFinding
                        {
                            ChainId = chain.ChainId,
                            HopId = hop.HopId,
                            MissingCategory = "CONSUMER_WITHOUT_SURFACE",
                            Details = $"{hop.RequiredFile} has no DISPLAYED_BY edge to ui:{hop.RequiredSurface}",
                            Severity = severity,
                            RecommendedFix = $"Give {hop.RequiredFile} a registered route through {hop.RequiredSurface} (never a registry-only promotion).",
                        });
                    }
                }
            }

            report.Stabilize();
            return report;
        }
    }
}
