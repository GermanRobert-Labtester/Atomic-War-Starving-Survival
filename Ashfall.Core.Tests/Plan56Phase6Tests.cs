// SPDX-License-Identifier: MIT
// Plan 56 phase 6 — provenance-aware waystation resupply in Core:
// lapse semantics, shortage-policy resupply, and save round-trip.

using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core.Economy;
using Ashfall.Core.IO;
using Ashfall.Core.Waystation;

namespace Ashfall.Core.Tests;

public class Plan56Phase6Tests
{
    private static GoodsCatalog LoadCatalog()
    {
        var cwd = System.IO.Directory.GetCurrentDirectory();
        CatalogLocator.TryFindDataDirectory(cwd, out var dataDir);
        var dir = string.IsNullOrEmpty(dataDir) ? cwd : dataDir;
        var load = GoodsCatalogLoader.Load(dir, new FileSystemIO(), new SystemTextJsonSerializer());
        Assert.False(load.HasErrors, string.Join("\n", load.Errors));
        return GoodsCatalogLoader.ToCatalog(load);
    }

    /// <summary>A two-station network: industrial_belt (foundry pool) + ash_flats (greenhouse pool).</summary>
    private static List<WaystationDef> TwoStationCatalog()
    {
        return new List<WaystationDef>
        {
            new WaystationDef
            {
                id = "ws_industrial", name = "Industrial Depot", node_id = "n1",
                region = "industrial_belt", specialty = "tools",
                stock_item_ids = new List<string> { "water_filter", "mechanical_parts", "seed_packets" },
            },
            new WaystationDef
            {
                id = "ws_greenhouse", name = "Glass Orchard Post", node_id = "n2",
                region = "ash_flats", specialty = "provisions",
                stock_item_ids = new List<string> { "seed_packets", "duct_tape", "rope" },
            },
        };
    }

    private static WaystationNetworkSystem NetworkWithPolicy(bool marketShort)
    {
        var network = new WaystationNetworkSystem(TwoStationCatalog());
        network.BindShortagePolicy(LoadCatalog(), () => marketShort);
        return network;
    }

    [Fact]
    public void ShortageResupply_KeepsLocalAndGeneral_LapsesPureImports()
    {
        var network = NetworkWithPolicy(marketShort: true);

        // Force the 7-day resupply on the next tick.
        foreach (var s in network.State.stations)
            s.daysSinceResupply = 7;
        network.TickDay();

        // Industrial station: water_filter (foundry — local) and
        // mechanical_parts (unannotated — general rides through) survive;
        // seed_packets (greenhouse) is a pure import here and lapses.
        var industrial = network.GetStation("ws_industrial")!;
        Assert.Contains("water_filter", industrial.availableStockItemIds);
        Assert.Contains("mechanical_parts", industrial.availableStockItemIds);
        Assert.DoesNotContain("seed_packets", industrial.availableStockItemIds);

        // Greenhouse station: its own pool + general survive.
        var greenhouse = network.GetStation("ws_greenhouse")!;
        Assert.Contains("seed_packets", greenhouse.availableStockItemIds);
        Assert.Contains("duct_tape", greenhouse.availableStockItemIds);
        Assert.Contains("rope", greenhouse.availableStockItemIds);
    }

    [Fact]
    public void LapsedImports_ReportsDefinitionMinusAvailability()
    {
        var network = NetworkWithPolicy(marketShort: true);
        foreach (var s in network.State.stations)
            s.daysSinceResupply = 7;
        network.TickDay();

        var def = TwoStationCatalog().First(d => d.id == "ws_industrial");
        var station = network.GetStation("ws_industrial")!;
        var lapsed = WaystationNetworkSystem.LapsedImports(def, station);
        Assert.Equal(new List<string> { "seed_packets" }, lapsed);
    }

    [Fact]
    public void NormalMarket_ResuppliesFullStock_NoLapse()
    {
        var network = NetworkWithPolicy(marketShort: false);
        foreach (var s in network.State.stations)
            s.daysSinceResupply = 7;
        network.TickDay();

        foreach (var def in TwoStationCatalog())
        {
            var station = network.GetStation(def.id)!;
            Assert.Equal(def.stock_item_ids, station.availableStockItemIds);
            Assert.Empty(WaystationNetworkSystem.LapsedImports(def, station));
        }
    }

    [Fact]
    public void UnboundNetwork_LegacyResupplyExact()
    {
        // No BindShortagePolicy → the resupply copies the definition exactly
        // (byte-for-byte legacy behavior for unwired hosts).
        var network = new WaystationNetworkSystem(TwoStationCatalog());
        foreach (var s in network.State.stations)
            s.daysSinceResupply = 7;
        network.TickDay();
        foreach (var def in TwoStationCatalog())
            Assert.Equal(def.stock_item_ids, network.GetStation(def.id)!.availableStockItemIds);
    }

    [Fact]
    public void StateCapture_Restore_PreservesFilteredStock()
    {
        var network = NetworkWithPolicy(marketShort: true);
        foreach (var s in network.State.stations)
            s.daysSinceResupply = 7;
        network.TickDay();

        var saved = network.CaptureState();
        var restored = new WaystationNetworkSystem(TwoStationCatalog());
        restored.RestoreState(saved);

        var industrial = restored.GetStation("ws_industrial")!;
        Assert.DoesNotContain("seed_packets", industrial.availableStockItemIds);
        Assert.Contains("water_filter", industrial.availableStockItemIds);
    }
}
