// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 215 — Crisis Rationing overlay completion: the authored protocol
// catalog reaches the canonical system through a strict loader, the player
// protocol route is lawful, restore never replays tier events, and the
// duplicate market restore stays removed.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public sealed class Plan215RationingOverlayCompletionTests
    {
        private static string RepoRoot()
        {
            var directory = new DirectoryInfo(Path.GetFullPath(AppContext.BaseDirectory));
            while (directory != null)
            {
                if (File.Exists(Path.Combine(directory.FullName, "src", "Main.Economy.cs")))
                    return directory.FullName;
                directory = directory.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate the ASHFALL repository root.");
        }

        private static string Source(string relativePath) =>
            File.ReadAllText(Path.Combine(RepoRoot(), relativePath));

        private static string DataDirectory => Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");

        private static List<RationingProtocolDefinition> LoadAuthoredProtocols()
        {
            var load = RationingProtocolCatalogLoader.Load(
                DataDirectory, new Ashfall.Core.FileSystemIO());
            Assert.False(load.HasErrors, string.Join("; ", load.Errors));
            Assert.Equal(4, load.Protocols.Count);
            return load.Protocols;
        }

        // ── Strict loader ──

        [Fact]
        public void StrictLoader_LoadsAllFourAuthoredProtocols_WithBoundedTiers()
        {
            var protocols = LoadAuthoredProtocols();
            Assert.Contains(protocols, p => p.Id == "protocol_standard_distribution");
            Assert.Contains(protocols, p => p.Id == "protocol_triage_survival" && p.DefaultTier == "minimal");
            foreach (var proto in protocols)
            {
                Assert.False(float.IsNaN(proto.MoralePenaltyScale));
                Assert.True(proto.MoralePenaltyScale >= 0f);
            }
        }

        [Fact]
        public void StrictLoader_RejectsUnknownTier_NegativeScale_AndDuplicates()
        {
            var bad = RationingProtocolCatalogLoader.Validate(new RationingProtocolDocument
            {
                schema_version = 1,
                protocols = new List<RationingProtocolRow>
                {
                    new() { id = "protocol_bad_tier", name = "Bad Tier", default_tier = "eleventy" },
                    new() { id = "protocol_bad_scale", name = "Bad Scale", morale_penalty_scale = -1f },
                    new() { id = "protocol_dup", name = "Dup" },
                    new() { id = "protocol_dup", name = "Dup again" }
                }
            });
            Assert.True(bad.HasErrors);
            Assert.Empty(bad.Protocols);
            Assert.Contains(bad.Errors, e => e.Contains("unknown default_tier"));
            Assert.Contains(bad.Errors, e => e.Contains("non-finite or negative"));
            Assert.Contains(bad.Errors, e => e.Contains("duplicate protocol id"));
        }

        // ── Canonical route semantics ──

        [Fact]
        public void ApplyProtocol_UnknownIdRefused_WhenCatalogLoaded()
        {
            var rationing = new ResourceRationingSystem();
            rationing.LoadCatalog(new RationingProtocolCatalogData
            {
                SchemaVersion = 1,
                Protocols = LoadAuthoredProtocols()
            });

            Assert.False(rationing.ApplyProtocol("protocol_nonexistent", Array.Empty<string>(), 1));
            Assert.Equal("protocol_standard_distribution", rationing.ActiveProtocolId);
        }

        [Fact]
        public void ApplyProtocol_SetsDefaultTier_AndEmitsExactlyOneTierEvent()
        {
            var rationing = new ResourceRationingSystem();
            rationing.LoadCatalog(new RationingProtocolCatalogData
            {
                SchemaVersion = 1,
                Protocols = LoadAuthoredProtocols()
            });
            rationing.BindResourceValidator(id => id == "item_canned_beans");
            rationing.SetRationTier("item_canned_beans", RationingTier.Full, 1);
            int before = rationing.EventCount;

            Assert.True(rationing.ApplyProtocol("protocol_emergency_crisis", new[] { "item_canned_beans" }, 2));

            Assert.Equal("protocol_emergency_crisis", rationing.ActiveProtocolId);
            Assert.Equal(RationingTier.Half, rationing.GetRationTier("item_canned_beans"));
            Assert.Equal(before + 1, rationing.EventCount);
        }

        [Fact]
        public void RestoreState_RoundTripsPolicy_WithoutRefiringTierEvents()
        {
            var rationing = new ResourceRationingSystem();
            rationing.LoadCatalog(new RationingProtocolCatalogData
            {
                SchemaVersion = 1,
                Protocols = LoadAuthoredProtocols()
            });
            rationing.BindResourceValidator(id => id == "item_canned_beans");
            rationing.ApplyProtocol("protocol_triage_survival", new[] { "item_canned_beans" }, 1);
            var snapshot = rationing.CaptureState();
            int eventsAtCapture = snapshot.Events.Count;

            var restored = new ResourceRationingSystem();
            restored.LoadCatalog(new RationingProtocolCatalogData
            {
                SchemaVersion = 1,
                Protocols = LoadAuthoredProtocols()
            });
            restored.BindResourceValidator(id => id == "item_canned_beans");
            restored.RestoreState(snapshot);

            Assert.Equal("protocol_triage_survival", restored.ActiveProtocolId);
            Assert.Equal(RationingTier.Minimal, restored.GetRationTier("item_canned_beans"));
            Assert.Equal(eventsAtCapture, restored.EventCount);
            Assert.NotNull(restored.GetProtocol("protocol_triage_survival"));
        }

        [Fact]
        public void RestoreState_IgnoresSavedRowsThatFailTheBoundValidator()
        {
            var tampered = new ResourceRationingState
            {
                ActiveProtocolId = "protocol_standard_distribution",
                Targets = new List<RationTarget>
                {
                    new() { ResourceId = "item_canned_beans", Tier = RationingTier.Half, BaseMultiplier = 0.5f },
                    new() { ResourceId = "item_not_in_catalog", Tier = RationingTier.None, BaseMultiplier = 0f }
                }
            };
            var filtered = new ResourceRationingSystem();
            filtered.BindResourceValidator(id => id == "item_canned_beans");
            filtered.RestoreState(tampered);

            Assert.Equal(1, filtered.TargetCount);
            Assert.Equal("item_canned_beans", filtered.RationTargets[0].ResourceId);
        }

        // ── Production wiring gate (source-text evidence) ──

        [Fact]
        public void HostWiring_LoadsCatalogBeforeRestore_RemovesDuplicateRestore_BindsPlayerRoute()
        {
            string hostSession = Source("src/Host/EconomyHostSession.cs");
            string mainEconomy = Source("src/Main.Economy.cs");
            string panel = Source("src/Economy/EconomyMarketPanel.cs");

            // The authored catalog feeds the canonical system BEFORE the saved
            // policy id is restored (host Create path).
            int loadIdx = hostSession.IndexOf("RationingProtocolCatalogLoader.Load", StringComparison.Ordinal);
            int restoreIdx = hostSession.IndexOf("session.Rationing.RestoreState(save.rationing", StringComparison.Ordinal);
            Assert.True(loadIdx >= 0, "host must load the authored protocol catalog");
            Assert.True(restoreIdx > loadIdx, "protocol definitions must load before the saved policy id is restored");

            // Duplicate market restore removed from Main.SetupEconomy.
            Assert.DoesNotContain("_economy.Market.RestoreState(save);", mainEconomy);
            Assert.Contains("EconomyHostSession.Create already", mainEconomy);

            // One lawful player protocol route through the canonical owner.
            Assert.Contains("ApplyRationingProtocolCommand", mainEconomy);
            Assert.Contains("_economy.ApplyRationingProtocol(protocolId, day)", mainEconomy);
            Assert.Contains("_economyPanel.RationingProtocolCommand = id => ApplyRationingProtocolCommand(id);", mainEconomy);

            // Panel readout distinguishes policy from stock and never mutates
            // policy on refresh.
            Assert.Contains("RefreshRationing", panel);
            Assert.Contains("OnRationingApplyPressed", panel);
            Assert.Contains("RationingProtocolCommand", panel);
        }

        [Fact]
        public void RationingSnapshotStaysNestedInTheEconomySection_NoSecondStore()
        {
            // DEC-200: the rationing snapshot is nested inside the canonical
            // economy save (MarketState.rationing) — no separate ration
            // section may exist.
            string registry = Source("Assets/Ashfall.Core/Save/SaveSectionRegistry.cs");
            Assert.DoesNotContain("\"rationing\"", registry);
            Assert.Contains("\"economy\"", registry);

            string market = Source("Assets/Ashfall.Core/Economy/MarketSystem.cs");
            Assert.Contains("public ResourceRationingState? rationing;", market);
        }
    }
}
