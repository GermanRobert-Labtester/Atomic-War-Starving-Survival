// SPDX-License-Identifier: MIT
using Ashfall.Core.Inventory;
using Ashfall.Core.World;
using Xunit;
using InventoryModel = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.World;

public sealed class GroundPenetratingRadarEngineTests : CatalogTestBase
{
    [Fact]
    public void CatalogRequiresMeaningfulModeTradeoff()
    {
        var catalog = GroundPenetratingRadarCatalogLoader.Load(DataDirectory);
        Assert.Contains(catalog.Modes, x => x.penetration_rating > x.resolution_rating);
        Assert.Contains(catalog.Modes, x => x.resolution_rating > x.penetration_rating);
    }

    [Fact]
    public void SurveyConsumesPowerAndProducesBoundedUncertainObservation()
    {
        var inventory = new InventoryModel();
        inventory.AddById("battery_pack", 2);
        var engine = new GroundPenetratingRadarEngine(new SeededRng(121), GroundPenetratingRadarCatalogLoader.Load(DataDirectory));
        engine.BindInventory(inventory);
        engine.Equip("gpr_cart_mk1");
        Assert.Equal(ActionResult.StatusKind.Success, engine.BeginSurvey("sector_a", "gpr_buried_structure", "dry_soil", "gpr_deep_scan").Status);
        engine.Tick();
        engine.Tick();
        var result = engine.Tick();

        Assert.True(result.Success);
        Assert.NotNull(result.Observation);
        Assert.InRange(result.Observation!.confidence, 0f, 1f);
        Assert.InRange(result.Observation.depth_band, 0f, 100f);
        Assert.Equal(0, inventory.CountById("battery_pack"));
    }

    [Fact]
    public void MissingPowerDoesNotCreateActiveSurvey()
    {
        var engine = new GroundPenetratingRadarEngine(new SeededRng(121), GroundPenetratingRadarCatalogLoader.Load(DataDirectory));
        engine.Equip("gpr_cart_mk1");

        var result = engine.BeginSurvey("sector_a", "gpr_buried_structure", "dry_soil", "gpr_deep_scan");

        Assert.Equal(GprFailureCodes.PowerUnavailable, result.FailureCode);
        Assert.Null(engine.State.active_survey);
    }

    [Fact]
    public void RepeatedObservationCanCreateOneIdempotentLead()
    {
        var inventory = new InventoryModel();
        inventory.AddById("battery_pack", 12);
        var engine = new GroundPenetratingRadarEngine(new SeededRng(121), GroundPenetratingRadarCatalogLoader.Load(DataDirectory));
        engine.BindInventory(inventory);
        engine.Equip("gpr_cart_mk1");
        for (int scan = 0; scan < 4; scan++)
        {
            Assert.Equal(ActionResult.StatusKind.Success, engine.BeginSurvey("sector_a", "gpr_buried_structure", "dry_soil", "gpr_deep_scan").Status);
            engine.Tick(); engine.Tick(); engine.Tick();
        }

        bool created = engine.TryCreateLead("sector_a", out var lead);
        bool duplicate = engine.TryCreateLead("sector_a", out var sameLead);

        Assert.True(created);
        Assert.NotNull(lead);
        Assert.False(duplicate);
        Assert.Same(lead, sameLead);
        Assert.Single(engine.State.leads);
    }

    [Fact]
    public void ActiveSurveyCaptureRestoreKeepsResultDeterministic()
    {
        var catalog = GroundPenetratingRadarCatalogLoader.Load(DataDirectory);
        var inventory = new InventoryModel();
        inventory.AddById("battery_pack", 2);
        var first = new GroundPenetratingRadarEngine(new SeededRng(121), catalog);
        first.BindInventory(inventory);
        first.Equip("gpr_cart_mk1");
        Assert.Equal(ActionResult.StatusKind.Success, first.BeginSurvey("sector_a", "gpr_buried_structure", "dry_soil", "gpr_deep_scan").Status);
        first.Tick();
        var second = new GroundPenetratingRadarEngine(new SeededRng(121), catalog);
        second.RestoreState(first.CaptureState());
        first.Tick();
        second.Tick();
        var a = first.Tick();
        var b = second.Tick();
        Assert.Equal(a.Success, b.Success);
        Assert.Equal(a.Observation?.confidence, b.Observation?.confidence);
        Assert.Equal(a.Observation?.anomaly_class, b.Observation?.anomaly_class);
    }
}
