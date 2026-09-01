using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Excavation;
using Ashfall.Core.Narrative;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        /// <summary>
        /// --world-exploration-selftest / --plan11-selftest:
        /// Verifies Plan 11 deep-strata excavation catalogs, cipher decoding loops,
        /// living geography evolution triggers, route blockades, and location memory recasts.
        /// </summary>
        public static int RunWorldExplorationSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            int failures = 0;
            int totalAssertions = 0;

            void Check(bool ok, string label)
            {
                totalAssertions++;
                GD.Print($"[{(ok ? "PASS" : "FAIL")}] {label}");
                if (!ok) failures++;
            }

            GD.Print("[WorldExplorationHeadlessDemo] begin Plan 11 verification...");

            var json = new SystemTextJsonSerializer();
            var files = new FileSystemIO();

            // 1. Excavation Sites Authority
            var sites = ExcavationCatalogLoader.Load(dataDirectory, files, json);
            Check(sites != null && sites.Count >= 5, $"Excavation catalog loaded (expected >= 5, got {sites?.Count ?? 0})");

            var expectedSites = new[]
            {
                "excavation_command_vault",
                "excavation_utility_tunnels",
                "excavation_metro_interchange",
                "excavation_mine_shaft",
                "excavation_archive_bunker"
            };

            foreach (var siteId in expectedSites)
            {
                var site = sites?.FirstOrDefault(s => s.site_id == siteId);
                Check(site != null && site.depth_bands.Count >= 3, $"Excavation site {siteId} has >= 3 strata depth bands");
            }

            // 2. Excavation System Simulation & Shoring
            var rng = new SeededRng(1986);
            var excavation = new ExcavationSystem(rng);
            excavation.AddSite("excavation_command_vault", "room_command_vault", 100f, 0.4f);
            excavation.AssignWorkers("excavation_command_vault", 2);
            excavation.ApplyShoring("excavation_command_vault");
            excavation.TickDay();

            var simSite = excavation.State.sites[0];
            Check(simSite.progress > 0f, "Excavation advances progress on daily tick");
            Check(simSite.shoringApplied, "Shoring applied and risk reduced");

            // 3. Map System & Node Integration
            var (nodes, routes) = WastelandMapCatalogLoader.Load(dataDirectory, files, json);
            var mapState = new WastelandMapState();
            var map = new WastelandMapSystem(mapState, nodes, routes);

            Check(map.GetNode("loc_excavation_command_vault") != null, "Excavation Command Vault node present on map");
            Check(map.GetNode("loc_hidden_relay_bunker") != null, "Hidden Relay Bunker node present on map");
            Check(!map.IsDiscovered("loc_hidden_relay_bunker"), "Hidden Relay Bunker starts undiscovered");

            // 4. Cipher Hunt Engine & Decode Loop
            var cipherEngine = new CipherQuestChainEngine();
            cipherEngine.RecordBroadcastHeard("radio_broadcast_relay_count", map);
            Check(!map.IsDiscovered("loc_hidden_relay_bunker"), "Broadcast alone does not reveal hidden destination");

            cipherEngine.RecordKeyAcquired("item_comm_codebook_alpha", map);
            Check(map.IsDiscovered("loc_hidden_relay_bunker"), "Codebook + Broadcast decodes coordinates and reveals destination");

            // 5. Living Geography World Evolution Events
            var evolutionEngine = new WorldEvolutionEngine(dataDirectory, files, json);
            Check(evolutionEngine.Events.Count >= 10, $"World evolution catalog loaded (expected >= 10, got {evolutionEngine.Events.Count})");

            var locEvolution = new LocationEvolutionSystem(new SeededRng(42));
            var landmarks = new LandmarkDegradationSystem(new SeededRng(42));

            evolutionEngine.TickDay(20, new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "flag_faction_escalation" }, locEvolution, landmarks, map);
            Check(map.IsLocked("loc_cut_abandoned_depot"), "Checkpoint blockade locks target node");

            // 6. Route Re-planning with Blockade
            map.Discover("loc_holdfast");
            map.Discover("loc_cut_abandoned_depot");
            map.Discover("loc_cut_arsenal_ruin");
            var detourRoute = map.PlanRoute("loc_holdfast", "loc_cut_arsenal_ruin");
            Check(!detourRoute.Contains("loc_cut_abandoned_depot"), "Route planning detours around locked blockade node");

            // 7. Save and Restore
            var capturedEvolution = evolutionEngine.CaptureState();
            var restoredEngine = new WorldEvolutionEngine(dataDirectory, files, json);
            restoredEngine.RestoreState(capturedEvolution, map);
            Check(restoredEngine.TriggeredEventIds.Contains("event_evolution_checkpoint_kilo"), "Evolution state restores triggered events deterministically");

            bool pass = failures == 0;
            string status = pass ? "PASS" : "FAIL";
            GD.Print($"[WorldExplorationHeadlessDemo] {totalAssertions - failures}/{totalAssertions} {status}");
            return EmitSummary("world_exploration_selftest", pass, pass ? 0 : 1, totalAssertions - failures, failures, $"[WorldExplorationHeadlessDemo] {totalAssertions - failures}/{totalAssertions} {status}");
        }
    }
}
