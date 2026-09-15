// SPDX-License-Identifier: MIT
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using Xunit;
using InventoryModel = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.Shelter;

public sealed class CarbonCompositeEngineTests : CatalogTestBase
{
    [Fact]
    public void CatalogLoadsExplicitComponentsAndCureProfiles()
    {
        var catalog = CarbonCompositeCatalogLoader.Load(DataDirectory);
        Assert.True(catalog.Components.Count >= 2);
        Assert.NotEmpty(catalog.Cures);
    }

    [Fact]
    public void MissingMaterialDoesNotCreateJobOrConsumeInventory()
    {
        var inventory = new InventoryModel();
        var engine = new CarbonCompositeEngine(new SeededRng(120), CarbonCompositeCatalogLoader.Load(DataDirectory));
        engine.BindInventory(inventory);

        var result = engine.StartJob("composite_sensor_housing", "prepreg_standard");

        Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
        Assert.Null(engine.State.active_job);
        Assert.Equal(0, inventory.CountById("prepreg_standard"));
    }

    [Fact]
    public void CureProducesExplicitComponentProjection()
    {
        var inventory = new InventoryModel();
        inventory.AddById("prepreg_standard", 2);
        var engine = new CarbonCompositeEngine(new SeededRng(120), CarbonCompositeCatalogLoader.Load(DataDirectory));
        engine.BindInventory(inventory);
        Assert.Equal(ActionResult.StatusKind.Success, engine.StartJob("composite_sensor_housing", "prepreg_standard").Status);
        engine.Tick(0.70f, 0.65f, 1f, 1f);
        engine.Tick(0.70f, 0.65f, 1f, 1f);
        var completed = engine.Tick(0.70f, 0.65f, 1f, 1f);

        Assert.NotNull(completed.CompletedOutput);
        Assert.InRange(completed.CompletedOutput!.quality, CompositeQualityGrade.Reject, CompositeQualityGrade.Certified);
        var projection = engine.ProjectComponent("composite_sensor_housing");
        Assert.NotNull(projection);
        Assert.InRange(projection!.MassFactor, 0.01f, 1.5f);
        Assert.InRange(projection.DurabilityFactor, 0.01f, 2f);
    }

    [Fact]
    public void ActiveCureCaptureRestoreKeepsQualityDeterministic()
    {
        var catalog = CarbonCompositeCatalogLoader.Load(DataDirectory);
        var firstInventory = new InventoryModel();
        firstInventory.AddById("prepreg_standard", 2);
        var first = new CarbonCompositeEngine(new SeededRng(500), catalog);
        first.BindInventory(firstInventory);
        first.StartJob("composite_sensor_housing", "prepreg_standard", 1);
        first.Tick(0.7f, 0.65f, 1f, 1f);

        var second = new CarbonCompositeEngine(new SeededRng(500), catalog);
        second.RestoreState(first.CaptureState());
        var a = first.Tick(0.7f, 0.65f, 1f, 1f);
        var b = second.Tick(0.7f, 0.65f, 1f, 1f);
        Assert.Equal(a.StatusCode, b.StatusCode);
        Assert.Equal(a.CompletedOutput?.quality, b.CompletedOutput?.quality);
        Assert.Equal(a.CompletedOutput?.mass_factor, b.CompletedOutput?.mass_factor);
    }
}
