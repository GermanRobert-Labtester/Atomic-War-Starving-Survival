// SPDX-License-Identifier: MIT
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using Xunit;
using InventoryModel = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.Shelter;

public sealed class FischerTropschSynthesisEngineTests : CatalogTestBase
{
    [Fact]
    public void CatalogLoadsAndResolvesAllProductKinds()
    {
        var catalog = FischerTropschCatalogLoader.Load(DataDirectory);
        Assert.Single(catalog.Reactors);
        Assert.Contains(catalog.Products, x => x.output_kind == "lubricant");
        Assert.Contains(catalog.Products, x => x.output_kind == "wax");
        Assert.Contains(catalog.Products, x => x.output_kind == "light_fraction");
    }

    [Fact]
    public void MissingFeedstockDoesNotStartOrConsumeAnything()
    {
        var inventory = new InventoryModel();
        var engine = new FischerTropschSynthesisEngine(new SeededRng(118), FischerTropschCatalogLoader.Load(DataDirectory));
        engine.BindInventory(inventory);

        var result = engine.StartBatch("ft_reactor_mk1");

        Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
        Assert.False(engine.HasActiveBatch);
        Assert.Equal(0, inventory.CountById("synthetic_fuel_canister"));
    }

    [Fact]
    public void ProductionUsesFeedstockAndClaimsBoundedOutputs()
    {
        var inventory = new InventoryModel();
        inventory.AddById("synthetic_fuel_canister", 2);
        var engine = new FischerTropschSynthesisEngine(new SeededRng(118), FischerTropschCatalogLoader.Load(DataDirectory));
        engine.BindInventory(inventory);

        Assert.Equal(ActionResult.StatusKind.Success, engine.StartBatch("ft_reactor_mk1").Status);
        engine.Tick(0.6f, 0.6f, 1f);
        var completed = engine.Tick(0.6f, 0.6f, 1f);

        Assert.NotNull(completed.CompletedBatch);
        Assert.InRange(completed.CompletedBatch!.lubricant_units, 0, 2);
        Assert.InRange(completed.CompletedBatch.wax_units, 0, 2);
        Assert.InRange(completed.CompletedBatch.light_fraction_units, 0, 2);
        Assert.Equal(ActionResult.StatusKind.Success, engine.ClaimOutputs().Status);
        Assert.True(inventory.CountById("machine_oil") + inventory.CountById("carbon_black_powder") + inventory.CountById("fuel_1l") > 0);
    }

    [Fact]
    public void ActiveBatchCaptureRestoreKeepsCompletionDeterministic()
    {
        var catalog = FischerTropschCatalogLoader.Load(DataDirectory);
        var firstInventory = new InventoryModel();
        firstInventory.AddById("synthetic_fuel_canister", 2);
        var first = new FischerTropschSynthesisEngine(new SeededRng(991), catalog);
        first.BindInventory(firstInventory);
        first.StartBatch("ft_reactor_mk1");
        first.Tick(0.6f, 0.6f, 1f);

        var second = new FischerTropschSynthesisEngine(new SeededRng(991), catalog);
        second.RestoreState(first.CaptureState());
        var a = first.Tick(0.6f, 0.6f, 1f).CompletedBatch;
        var b = second.Tick(0.6f, 0.6f, 1f).CompletedBatch;

        Assert.NotNull(a);
        Assert.NotNull(b);
        Assert.Equal(a!.lubricant_units, b!.lubricant_units);
        Assert.Equal(a.wax_units, b.wax_units);
        Assert.Equal(a.light_fraction_units, b.light_fraction_units);
        Assert.Equal(a.lubricant_grade, b.lubricant_grade);
    }

    [Fact]
    public void RegisteredConsumerGetsOnlyItsExplicitServiceMultiplier()
    {
        var inventory = new InventoryModel();
        inventory.AddById("machine_oil", 1);
        var engine = new FischerTropschSynthesisEngine(new SeededRng(118), FischerTropschCatalogLoader.Load(DataDirectory));
        engine.BindInventory(inventory);
        engine.RegisterLubricantConsumer(new MechanicalLubricantConsumer { consumer_id = "generator_a", accepted_grades = new System.Collections.Generic.List<string> { "synthetic" }, wear_multiplier = 0.75f });

        Assert.Equal(1f, engine.GetWearMultiplier("generator_a"));
        Assert.Equal(ActionResult.StatusKind.Success, engine.ServiceLubricantConsumer("generator_a").Status);
        Assert.Equal(0.75f, engine.GetWearMultiplier("generator_a"));
        Assert.Equal(1f, engine.GetWearMultiplier("unregistered"));
    }
}
