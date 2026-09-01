using System;
using System.IO;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests;

public class TradeCommandTests
{
    private static readonly string DataDir = Path.GetFullPath(
        Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));

    private static HoldfastCatalog LoadCatalog()
    {
        var files = new FileSystemIO();
        var json = new SystemTextJsonSerializer();
        var loader = new HoldfastCatalogLoader(files, json);
        return loader.Load(DataDir);
    }

    [Fact]
    public void PreviewBuy_Available_WhenItemInStockAndAffordable()
    {
        var catalog = LoadCatalog();
        var sys = new HoldfastTradeSession(catalog, 50L);
        sys.SetStock("item_map_sheet_ice_road", 5);
        sys.SelectFaction("faction_the_office");

        var preview = sys.PreviewBuy("item_map_sheet_ice_road", 1, "faction_the_office", stateVersion: 10L);

        Assert.True(preview.IsAvailable);
        Assert.Equal(10L, preview.StateVersion);
        Assert.Equal("trade.preview_buy", preview.MessageKey);
    }

    [Fact]
    public void PreviewBuy_Unavailable_WhenOutOfStock()
    {
        var catalog = LoadCatalog();
        var sys = new HoldfastTradeSession(catalog, 50L);
        sys.SetStock("item_map_sheet_ice_road", 0);
        sys.SelectFaction("faction_the_office");

        var preview = sys.PreviewBuy("item_map_sheet_ice_road", 1, "faction_the_office", stateVersion: 10L);

        Assert.False(preview.IsAvailable);
    }

    [Fact]
    public void PreviewBuy_Unavailable_WhenInsufficientFunds()
    {
        var catalog = LoadCatalog();
        var sys = new HoldfastTradeSession(catalog, 1L);
        sys.SetStock("item_map_sheet_ice_road", 5);
        sys.SelectFaction("faction_the_office");

        var preview = sys.PreviewBuy("item_map_sheet_ice_road", 1, "faction_the_office", stateVersion: 10L);

        Assert.False(preview.IsAvailable);
    }

    [Fact]
    public void ExecuteBuy_StalePreview_RejectsWithoutMutation()
    {
        var catalog = LoadCatalog();
        var sys = new HoldfastTradeSession(catalog, 50L);
        sys.SetStock("item_map_sheet_ice_road", 5);
        sys.SelectFaction("faction_the_office");

        var result = sys.ExecuteBuy("item_map_sheet_ice_road", 1, "faction_the_office", expectedStateVersion: 99L, currentStateVersion: 100L);

        Assert.False(result.IsSuccess);
        Assert.Equal("stale_preview", result.FailureCode);
    }

    [Fact]
    public void ExecuteBuy_MatchingVersions_BuysItem()
    {
        var catalog = LoadCatalog();
        var sys = new HoldfastTradeSession(catalog, 50L);
        sys.SetStock("item_map_sheet_ice_road", 5);
        sys.SelectFaction("faction_the_office");

        int heldBefore = sys.GetHeld("item_map_sheet_ice_road");
        var result = sys.ExecuteBuy("item_map_sheet_ice_road", 1, "faction_the_office", expectedStateVersion: 10L, currentStateVersion: 10L);

        Assert.True(result.IsSuccess);
        Assert.Equal("trade.bought", result.MessageKey);
        Assert.Equal(heldBefore + 1, sys.GetHeld("item_map_sheet_ice_road"));
    }

    [Fact]
    public void PreviewSell_Available_WhenItemHeld()
    {
        var catalog = LoadCatalog();
        var sys = new HoldfastTradeSession(catalog, 50L);
        sys.SetStock("item_map_sheet_ice_road", 5);
        sys.SelectFaction("faction_the_office");
        sys.SeedInventory("item_map_sheet_ice_road", 10);

        var preview = sys.PreviewSell("item_map_sheet_ice_road", 2, "faction_the_office", stateVersion: 10L);

        Assert.True(preview.IsAvailable);
        Assert.Equal(10L, preview.StateVersion);
    }

    [Fact]
    public void PreviewSell_Unavailable_WhenInsufficientInventory()
    {
        var catalog = LoadCatalog();
        var sys = new HoldfastTradeSession(catalog, 50L);
        sys.SetStock("item_map_sheet_ice_road", 5);
        sys.SelectFaction("faction_the_office");
        sys.SeedInventory("item_map_sheet_ice_road", 1);

        var preview = sys.PreviewSell("item_map_sheet_ice_road", 2, "faction_the_office", stateVersion: 10L);

        Assert.False(preview.IsAvailable);
    }

    [Fact]
    public void ExecuteSell_StalePreview_RejectsWithoutMutation()
    {
        var catalog = LoadCatalog();
        var sys = new HoldfastTradeSession(catalog, 50L);
        sys.SetStock("item_map_sheet_ice_road", 5);
        sys.SelectFaction("faction_the_office");
        sys.SeedInventory("item_map_sheet_ice_road", 10);

        var result = sys.ExecuteSell("item_map_sheet_ice_road", 2, "faction_the_office", expectedStateVersion: 99L, currentStateVersion: 100L);

        Assert.False(result.IsSuccess);
        Assert.Equal("stale_preview", result.FailureCode);
    }

    [Fact]
    public void ExecuteSell_MatchingVersions_SellsItem()
    {
        var catalog = LoadCatalog();
        var sys = new HoldfastTradeSession(catalog, 50L);
        sys.SetStock("item_map_sheet_ice_road", 5);
        sys.SelectFaction("faction_the_office");
        sys.SeedInventory("item_map_sheet_ice_road", 10);

        int heldBefore = sys.GetHeld("item_map_sheet_ice_road");
        var result = sys.ExecuteSell("item_map_sheet_ice_road", 2, "faction_the_office", expectedStateVersion: 10L, currentStateVersion: 10L);

        Assert.True(result.IsSuccess);
        Assert.Equal("trade.sold", result.MessageKey);
        Assert.Equal(heldBefore - 2, sys.GetHeld("item_map_sheet_ice_road"));
    }
}
