// SPDX-License-Identifier: MIT
// Plan 56 phase 4 — accessibility-safe provenance labels for market rows.

using Xunit;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.IO;

namespace Ashfall.Core.Tests;

public class Plan56Phase4Tests
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

    [Fact]
    public void Label_LocallyMade_WhenOriginProducesTheGood()
    {
        var catalog = LoadCatalog();
        // water_filter is annotated foundry; the foundry region makes it.
        Assert.Equal("locally made",
            RegionalSupplyRouter.ProvenanceLabel(catalog, "industrial_belt", "water_filter"));
        // Seed packets are greenhouse-supplied.
        Assert.Equal("locally made",
            RegionalSupplyRouter.ProvenanceLabel(catalog, "ash_flats", "seed_packets"));
    }

    [Fact]
    public void Label_Imported_WhenOriginDoesNotProduceTheGood()
    {
        var catalog = LoadCatalog();
        Assert.Equal("imported",
            RegionalSupplyRouter.ProvenanceLabel(catalog, "flotilla", "water_filter"));
        Assert.Equal("imported",
            RegionalSupplyRouter.ProvenanceLabel(catalog, "ash_flats", "mechanical_parts"));
    }

    [Fact]
    public void Label_GeneralSupply_ForUniversalGoods()
    {
        var catalog = LoadCatalog();
        Assert.Equal("general supply",
            RegionalSupplyRouter.ProvenanceLabel(catalog, "industrial_belt", "duct_tape"));
        Assert.Equal("general supply",
            RegionalSupplyRouter.ProvenanceLabel(catalog, "flotilla", "duct_tape"));
    }

    [Fact]
    public void Label_EmptyForUnknownOrUnannotatedGoods_NeverThrows()
    {
        var catalog = LoadCatalog();
        Assert.Equal(string.Empty, RegionalSupplyRouter.ProvenanceLabel(catalog, "settlement", "does_not_exist"));
        Assert.NotNull(RegionalSupplyRouter.ProvenanceLabel(null!, "settlement", "clean_water"));
        Assert.Empty(RegionalSupplyRouter.ProvenanceLabel(null!, "settlement", "clean_water"));
    }

    [Fact]
    public void Label_IsTextFirst_NeverColorOnly()
    {
        // The labels are plain words usable by screen readers and readable
        // without color — pin the exact vocabulary so UI copy stays stable.
        var catalog = LoadCatalog();
        var allowed = new[] { "locally made", "imported", "general supply", "" };
        foreach (var good in catalog.All())
        {
            foreach (var region in new[] { "settlement", "industrial_belt", "deep_coast", "ash_flats", "traplines" })
            {
                var label = RegionalSupplyRouter.ProvenanceLabel(catalog, region, good.id);
                Assert.Contains(label, allowed);
            }
        }
    }
}
