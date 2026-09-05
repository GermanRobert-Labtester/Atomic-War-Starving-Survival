using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    /// <summary>
    /// Plan 85 — damaged-map treasure-hunt layer: catalog contract, fragment
    /// lifecycle, idempotent completion/reveal, destination gating, old-save
    /// compatibility, and expedition scavenging integration.
    /// </summary>
    public sealed class DamagedMapSystemTests : IDisposable
    {
        private readonly string _dataDir;
        private readonly IFileIO _files;
        private readonly IJsonSerializer _json;

        public DamagedMapSystemTests()
        {
            _dataDir = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (!Directory.Exists(_dataDir))
                _dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data");
            _files = new FileSystemIO();
            _json = new SystemTextJsonSerializer();
        }

        public void Dispose()
        {
            ExpeditionDefinitionRegistry.Clear();
        }

        private (DamagedMapSystem system, WastelandMapSystem map, List<DamagedMapZone> zones) CreateLive()
        {
            var (nodes, routes) = WastelandMapCatalogLoader.Load(_dataDir, _files, _json);
            var map = new WastelandMapSystem(new WastelandMapState(), nodes, routes);
            var (zones, errors) = DamagedMapCatalogLoader.LoadWithValidation(_dataDir, _files, _json);
            Assert.Empty(errors);
            return (new DamagedMapSystem(zones, map), map, zones);
        }

        // ── Catalog contract ─────────────────────────────────────────

        [Fact]
        public void Catalog_LoadsTwelveZones_WithUniqueIdsAndConsistentCounts()
        {
            var (zones, errors) = DamagedMapCatalogLoader.LoadWithValidation(_dataDir, _files, _json);
            Assert.Empty(errors);
            Assert.True(zones.Count >= 12, $"expected >= 12 zones, got {zones.Count}");

            var zoneIds = zones.Select(z => z.ZoneId).ToHashSet();
            var fragmentIds = zones.SelectMany(z => z.Fragments).Select(f => f.fragment_id).ToHashSet();
            var installationIds = zones.Select(z => z.InstallationId).ToHashSet();
            Assert.Equal(zones.Count, zoneIds.Count);
            Assert.Equal(zones.SelectMany(z => z.Fragments).Count(), fragmentIds.Count);
            Assert.Equal(zones.Count, installationIds.Count);
            foreach (var zone in zones)
            {
                Assert.Equal(zone.TotalFragments, zone.Fragments.Count);
                Assert.InRange(zone.TotalFragments, 2, 4);
                Assert.NotEmpty(zone.RevealedItems);
            }
        }

        [Fact]
        public void Catalog_AllRevealedItems_ResolveAgainstItemAuthority()
        {
            string itemsPath = Path.Combine(_dataDir, "items.json");
            Assert.True(_files.FileExists(itemsPath));
            var raw = _files.ReadAllText(itemsPath);
            var doc = System.Text.Json.JsonDocument.Parse(raw);
            var ids = new HashSet<string>(StringComparer.Ordinal);
            if (doc.RootElement.ValueKind == System.Text.Json.JsonValueKind.Array)
            {
                foreach (var e in doc.RootElement.EnumerateArray())
                    if (e.TryGetProperty("id", out var id)) ids.Add(id.GetString()!);
            }
            else
            {
                foreach (var e in doc.RootElement.GetProperty("items").EnumerateArray())
                    if (e.TryGetProperty("id", out var id)) ids.Add(id.GetString()!);
            }

            var (zones, errors) = DamagedMapCatalogLoader.LoadWithValidation(_dataDir, _files, _json);
            Assert.Empty(errors);
            foreach (var zone in zones)
            foreach (var item in zone.RevealedItems)
                Assert.True(ids.Contains(item), $"zone '{zone.ZoneId}' revealed_item '{item}' does not resolve in items.json");
        }

        [Fact]
        public void Catalog_EveryFragment_HasScavengingProducer()
        {
            var tables = ScavengingTableCatalog.LoadFromDirectory(_dataDir, _files, _json);
            var produced = new HashSet<string>(StringComparer.Ordinal);
            foreach (var table in tables.Tables)
            foreach (var entry in table.entries)
                if (!string.IsNullOrEmpty(entry.map_fragment_id))
                    produced.Add(entry.map_fragment_id);

            var (zones, _) = DamagedMapCatalogLoader.LoadWithValidation(_dataDir, _files, _json);
            foreach (var zone in zones)
            foreach (var fragment in zone.Fragments)
                Assert.True(produced.Contains(fragment.fragment_id),
                    $"fragment '{fragment.fragment_id}' (zone '{zone.ZoneId}') has no scavenging producer");
        }

        [Fact]
        public void Catalog_EveryInstallation_HasWastelandMapNodeAndRoute()
        {
            var (nodes, routes) = WastelandMapCatalogLoader.Load(_dataDir, _files, _json);
            var nodeIds = nodes.Select(n => n.Id).ToHashSet();
            var (zones, _) = DamagedMapCatalogLoader.LoadWithValidation(_dataDir, _files, _json);
            foreach (var zone in zones)
            {
                string nodeId = DamagedMapSystem.ResolveRevealNodeId(zone.InstallationId)!;
                Assert.True(nodeIds.Contains(nodeId),
                    $"zone '{zone.ZoneId}' installation '{zone.InstallationId}' has no map node '{nodeId}'");
                Assert.Contains(routes, r => r.To == nodeId || r.From == nodeId);
            }
        }

        // ── Lifecycle: registration, completion, idempotence ─────────

        [Fact]
        public void RegisterFragment_ProgressesAndCompletes_EdgeTriggeredOnce()
        {
            var (system, map, zones) = CreateLive();
            var zone = zones.First(z => z.Fragments.Count == 3);
            var fragments = zone.Fragments.Select(f => f.fragment_id).ToList();
            string nodeId = DamagedMapSystem.ResolveRevealNodeId(zone.InstallationId)!;

            int completions = 0;
            int reveals = 0;
            system.OnZoneCompleted += _ => completions++;
            system.OnInstallationRevealed += (_, _) => reveals++;

            Assert.Equal(0, system.RegisteredCount(zone.ZoneId));
            Assert.False(map.IsDiscovered(nodeId));
            Assert.True(map.IsLocked(nodeId)); // hidden until reveal

            Assert.True(system.RegisterFragment(fragments[0]));
            Assert.Equal(1, system.RegisteredCount(zone.ZoneId));
            Assert.Equal(0, completions);
            Assert.False(system.IsZoneComplete(zone.ZoneId));

            Assert.True(system.RegisterFragment(fragments[1]));
            Assert.Equal(2, system.RegisteredCount(zone.ZoneId));
            Assert.Equal(0, completions);

            Assert.True(system.RegisterFragment(fragments[2]));
            Assert.Equal(3, system.RegisteredCount(zone.ZoneId));
            Assert.Equal(1, completions);
            Assert.Equal(1, reveals);
            Assert.True(system.IsZoneComplete(zone.ZoneId));

            // Reveal reached the authoritative world map — discovered AND unlocked.
            Assert.True(map.IsDiscovered(nodeId));
            Assert.False(map.IsLocked(nodeId));
            Assert.True(system.IsInstallationRevealed(zone.ZoneId));

            // Idempotence: re-registering the final fragment changes nothing.
            Assert.False(system.RegisterFragment(fragments[2]));
            Assert.Equal(1, completions);
            Assert.Equal(1, reveals);
        }

        [Fact]
        public void RegisterFragment_DuplicatesAndUnknowns_NeverDoubleCount()
        {
            var (system, _, zones) = CreateLive();
            var zone = zones[0];
            var fragment = zone.Fragments[0].fragment_id;

            Assert.True(system.RegisterFragment(fragment));
            for (int i = 0; i < 5; i++)
                Assert.False(system.RegisterFragment(fragment));
            Assert.Equal(1, system.RegisteredCount(zone.ZoneId));

            Assert.False(system.RegisterFragment("damaged_map_does_not_exist"));
            Assert.False(system.RegisterFragment(string.Empty));
        }

        [Fact]
        public void Reveal_PersistsThroughCaptureRestore_AndSurvivesReload()
        {
            var (system, map, zones) = CreateLive();
            var zone = zones[0];
            string nodeId = DamagedMapSystem.ResolveRevealNodeId(zone.InstallationId)!;
            foreach (var f in zone.Fragments)
                system.RegisterFragment(f.fragment_id);
            Assert.True(map.IsDiscovered(nodeId));

            // Save boundary: capture, then restore into a fresh map + system.
            var captured = map.CaptureState();
            var (nodes, routes) = WastelandMapCatalogLoader.Load(_dataDir, _files, _json);
            var restoredMap = new WastelandMapSystem(captured, nodes, routes);
            var restoredSystem = new DamagedMapSystem(zones, restoredMap);

            Assert.True(restoredMap.IsDiscovered(nodeId));
            Assert.False(restoredMap.IsLocked(nodeId));
            Assert.True(restoredSystem.IsInstallationRevealed(zone.ZoneId));
            Assert.True(restoredSystem.IsZoneComplete(zone.ZoneId));

            // Reload cannot re-fire completion or duplicate the reveal.
            int completions = 0;
            restoredSystem.OnZoneCompleted += _ => completions++;
            foreach (var f in zone.Fragments)
                restoredSystem.RegisterFragment(f.fragment_id);
            Assert.Equal(0, completions);
        }

        [Fact]
        public void DestinationGate_BlocksUntilRevealed_NeverGatesOtherLocations()
        {
            var (system, _, zones) = CreateLive();
            var zone = zones[0];
            string nodeId = DamagedMapSystem.ResolveRevealNodeId(zone.InstallationId)!;

            Assert.True(system.IsDestinationLocked(nodeId));
            Assert.False(system.IsDestinationLocked("loc_holdfast"));
            Assert.False(system.IsDestinationLocked("loc_cut_abandoned_depot"));
            Assert.False(system.IsDestinationLocked("some_random_place"));

            foreach (var f in zone.Fragments)
                system.RegisterFragment(f.fragment_id);
            Assert.False(system.IsDestinationLocked(nodeId));
        }

        // ── Old-save compatibility (§85E.2) ──────────────────────────

        [Fact]
        public void OldSave_OriginalZoneProgress_LoadsAndPreserves_UnderExpandedCatalog()
        {
            // Simulate a pre-expansion save: partial progress on one original
            // zone, another original zone untouched, no new-zone knowledge.
            var originalZoneIds = new[] { "industrial_district", "suburban_heights", "military_corridor" };
            var (nodes, routes) = WastelandMapCatalogLoader.Load(_dataDir, _files, _json);
            var oldState = new WastelandMapState();
            oldState.Discovered.Add("loc_holdfast");
            oldState.Discovered.Add("loc_cut_merchant_caravanserai");
            oldState.RegisteredMapFragments.Add("damaged_map_industrial_1");
            oldState.RegisteredMapFragments.Add("damaged_map_industrial_2");

            var map = new WastelandMapSystem(oldState, nodes, routes);
            var (zones, _) = DamagedMapCatalogLoader.LoadWithValidation(_dataDir, _files, _json);
            var system = new DamagedMapSystem(zones, map);

            // Original progress preserved exactly.
            Assert.Equal(2, system.RegisteredCount("industrial_district"));
            Assert.False(system.IsZoneComplete("industrial_district"));
            Assert.Equal(0, system.RegisteredCount("suburban_heights"));

            // New zones initialize cleanly as undiscovered.
            foreach (var zone in zones)
            {
                if (originalZoneIds.Contains(zone.ZoneId) && zone.ZoneId != "industrial_district")
                    Assert.Equal(0, system.RegisteredCount(zone.ZoneId));
                Assert.False(system.IsInstallationRevealed(zone.ZoneId));
            }

            // Collecting the final original fragment completes and reveals.
            Assert.True(system.RegisterFragment("damaged_map_industrial_3"));
            Assert.True(system.IsZoneComplete("industrial_district"));
            Assert.True(map.IsDiscovered("loc_underground_fuel_depot"));

            // Round-trip keeps everything.
            var captured = map.CaptureState();
            var restored = new WastelandMapSystem(captured, nodes, routes);
            Assert.Contains("damaged_map_industrial_1", restored.State.RegisteredMapFragments);
            Assert.Contains("damaged_map_industrial_3", restored.State.RegisteredMapFragments);
            Assert.True(restored.IsDiscovered("loc_underground_fuel_depot"));
        }

        // ── Determinism (§85E.4) ─────────────────────────────────────

        [Fact]
        public void CatalogOrder_DoesNotAffectProgression()
        {
            var (zones, errors) = DamagedMapCatalogLoader.LoadWithValidation(_dataDir, _files, _json);
            Assert.Empty(errors);
            var (nodes, routes) = WastelandMapCatalogLoader.Load(_dataDir, _files, _json);

            var shuffled = zones.ToList();
            shuffled.Reverse(); // reverse rather than RNG: pure order test

            var forward = new DamagedMapSystem(zones, new WastelandMapSystem(new WastelandMapState(), nodes, routes));
            var backward = new DamagedMapSystem(shuffled, new WastelandMapSystem(new WastelandMapState(), nodes, routes));

            var zone = zones[0];
            foreach (var f in zone.Fragments)
            {
                forward.RegisterFragment(f.fragment_id);
                backward.RegisterFragment(f.fragment_id);
            }
            Assert.Equal(forward.IsZoneComplete(zone.ZoneId), backward.IsZoneComplete(zone.ZoneId));
            Assert.Equal(
                forward.IsDestinationLocked(DamagedMapSystem.ResolveRevealNodeId(zone.InstallationId)!),
                backward.IsDestinationLocked(DamagedMapSystem.ResolveRevealNodeId(zone.InstallationId)!));
        }

        // ── Expedition integration (§10.3) ───────────────────────────

        [Fact]
        public void ExpeditionSystem_FragmentRoll_RegistersDiscovery_AndFragmentOnlyRollYieldsNoItem()
        {
            var (nodes, routes) = WastelandMapCatalogLoader.Load(_dataDir, _files, _json);
            var map = new WastelandMapSystem(new WastelandMapState(), nodes, routes);
            var (zones, _) = DamagedMapCatalogLoader.LoadWithValidation(_dataDir, _files, _json);
            var damagedMap = new DamagedMapSystem(zones, map);

            // Deterministic plumbing probe: a single-entry table where every
            // roll resolves the fragment token (no physical item attached).
            var probeTable = new ScavengingTableDef
            {
                id = "table_probe_fragment",
                location_type = "probe",
                entries = new List<ScavengingLootEntryDef>
                {
                    new ScavengingLootEntryDef { item_id = string.Empty, weight = 10, map_fragment_id = "damaged_map_industrial_1" }
                }
            };
            var system = new ExpeditionSystem
            {
                ScavengingCatalog = new ScavengingTableCatalog(new[] { probeTable }),
                DamagedMap = damagedMap
            };

            var def = new ExpeditionDefinition
            {
                id = "exp_plan85_probe",
                displayName = "Plan 85 Fragment Probe",
                distanceTicks = 1,
                dangerLevel = 1,
                scavenging_table_id = "table_probe_fragment"
            };
            ExpeditionDefinitionRegistry.Register(def);
            Assert.True(system.Start(def, "surv_plan85_probe", 1));
            var state = system.Active["surv_plan85_probe"];
            state.phase = (int)ExpeditionPhase.Looting;
            state.maxLootCapacityKg = 100f;

            var rng = new SeededRng(185);
            for (int i = 0; i < 10; i++)
                system.TickHours(1.0f, rng);

            Assert.Equal(1, damagedMap.RegisteredCount("industrial_district"));
            Assert.Contains("damaged_map_industrial_1", map.State.RegisteredMapFragments);

            // Fragment-only rolls yield no physical loot line...
            Assert.DoesNotContain(state.loot, l => l.itemId == "damaged_map_industrial_1");

            // ...and duplicate rolls cannot double-count.
            Assert.False(damagedMap.RegisterFragment("damaged_map_industrial_1"));
            Assert.Equal(1, damagedMap.RegisteredCount("industrial_district"));
        }

        [Fact]
        public void LiveCatalog_FragmentTokens_SurfaceUnderSeededSoak_AndResolve()
        {
            var tables = ScavengingTableCatalog.LoadFromDirectory(_dataDir, _files, _json);
            var (zones, _) = DamagedMapCatalogLoader.LoadWithValidation(_dataDir, _files, _json);
            var validFragments = zones.SelectMany(z => z.Fragments).Select(f => f.fragment_id).ToHashSet();

            var rng = new SeededRng(20260903);
            var surfaced = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < 20000; i++)
            {
                var roll = tables.RollLoot("table_loot_industrial_district", rng);
                Assert.NotNull(roll);
                if (!string.IsNullOrEmpty(roll.MapFragmentId))
                {
                    Assert.True(validFragments.Contains(roll.MapFragmentId),
                        $"rolled fragment '{roll.MapFragmentId}' is not a catalog fragment");
                    surfaced.Add(roll.MapFragmentId);
                    if (surfaced.Count >= 2) break;
                }
            }
            Assert.True(surfaced.Count >= 2,
                "seeded soak of the live industrial table never surfaced two distinct fragment tokens");
        }

        [Fact]
        public void ExpeditionSystem_StartRefused_WhileDestinationLocked()
        {
            var (nodes, routes) = WastelandMapCatalogLoader.Load(_dataDir, _files, _json);
            var map = new WastelandMapSystem(new WastelandMapState(), nodes, routes);
            var (zones, _) = DamagedMapCatalogLoader.LoadWithValidation(_dataDir, _files, _json);
            var damagedMap = new DamagedMapSystem(zones, map);

            var system = new ExpeditionSystem { DamagedMap = damagedMap };
            var def = new ExpeditionDefinition
            {
                id = "loc_sealed_triage_annex",
                displayName = "Sealed Triage Annex",
                distanceTicks = 8,
                dangerLevel = 3
            };
            ExpeditionDefinitionRegistry.Register(def);

            Assert.True(damagedMap.IsDestinationLocked(def.id));
            Assert.False(system.Start(def, "surv_plan85_gate", 1));
            Assert.Empty(system.Active);
        }

        // ── Negative fixtures (§10.4) ────────────────────────────────

        [Fact]
        public void Validator_DuplicateZoneId_IsReported()
        {
            var container = new DamagedMapCatalogContainer
            {
                zones = new List<DamagedMapZoneDef>
                {
                    MakeZone("zone_a", "frag_a1"),
                    MakeZone("zone_a", "frag_b1")
                }
            };
            var errors = DamagedMapCatalogLoader.Validate(container);
            Assert.Contains(errors, e => e.ErrorMessage.Contains("Duplicate zone_id"));
        }

        [Fact]
        public void Validator_FragmentCountMismatch_IsReported()
        {
            var zone = MakeZone("zone_a", "frag_a1");
            zone.total_fragments = 3; // fragments.Count == 1
            var errors = DamagedMapCatalogLoader.Validate(new DamagedMapCatalogContainer { zones = new List<DamagedMapZoneDef> { zone } });
            Assert.Contains(errors, e => e.ErrorMessage.Contains("total_fragments"));
        }

        [Fact]
        public void Validator_DuplicateFragmentAcrossZones_IsReported()
        {
            var container = new DamagedMapCatalogContainer
            {
                zones = new List<DamagedMapZoneDef>
                {
                    MakeZone("zone_a", "frag_shared"),
                    MakeZone("zone_b", "frag_shared")
                }
            };
            var errors = DamagedMapCatalogLoader.Validate(container);
            Assert.Contains(errors, e => e.ErrorMessage.Contains("across zones"));
        }

        [Fact]
        public void Validator_UnresolvedAndDuplicateRewards_AreReported()
        {
            var zone = MakeZone("zone_a", "frag_a1");
            zone.revealed_items = new List<string> { "bandage", "bandage" };
            var errors = DamagedMapCatalogLoader.Validate(new DamagedMapCatalogContainer { zones = new List<DamagedMapZoneDef> { zone } });
            Assert.Contains(errors, e => e.ErrorMessage.Contains("duplicate"));

            zone.revealed_items = new List<string> { "bandage", "" };
            errors = DamagedMapCatalogLoader.Validate(new DamagedMapCatalogContainer { zones = new List<DamagedMapZoneDef> { zone } });
            Assert.Contains(errors, e => e.ErrorMessage.Contains("empty entry"));
        }

        [Fact]
        public void Validator_MissingZoneName_IsReported()
        {
            var zone = MakeZone("zone_a", "frag_a1");
            zone.zone_name = string.Empty;
            var errors = DamagedMapCatalogLoader.Validate(new DamagedMapCatalogContainer { zones = new List<DamagedMapZoneDef> { zone } });
            Assert.Contains(errors, e => e.ErrorMessage.Contains("zone_name"));
        }

        private static DamagedMapZoneDef MakeZone(string zoneId, string fragmentId)
        {
            return new DamagedMapZoneDef
            {
                zone_id = zoneId,
                zone_name = zoneId,
                total_fragments = 1,
                hidden_installation_id = "loc_install_" + zoneId,
                hidden_installation_name = "Install " + zoneId,
                installation_description = "Test installation description.",
                revealed_items = new List<string> { "bandage" },
                fragments = new List<DamagedMapFragmentDef>
                {
                    new DamagedMapFragmentDef { fragment_id = fragmentId, label = "F", description = "D" }
                }
            };
        }
    }
}
