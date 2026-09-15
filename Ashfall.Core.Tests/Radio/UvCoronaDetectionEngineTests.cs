// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radio;
using Xunit;
using InventoryModel = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.Radio;

public sealed class UvCoronaDetectionEngineTests : CatalogTestBase
{
    [Fact]
    public void CatalogLoadsEnvironmentProfiles()
    {
        var catalog = UvCoronaDetectionCatalogLoader.Load(DataDirectory);
        Assert.Single(catalog.Detectors);
        Assert.True(catalog.Environments.Count >= 3);
    }

    [Fact]
    public void StrongNearbyFaultProducesBoundedObservationAndConsumesBattery()
    {
        var inventory = new InventoryModel();
        inventory.AddById("battery", 1);
        var engine = new UvCoronaDetectionEngine(new SeededRng(119), UvCoronaDetectionCatalogLoader.Load(DataDirectory));
        engine.BindInventory(inventory);
        engine.Equip("uv_corona_camera_mk1");
        var faults = new List<ElectricalFaultInput> { new ElectricalFaultInput { fault_id = "fault_a", asset_id = "substation_a", fault_intensity = 0.9f, distance = 1f, energized = true } };

        var result = engine.Scan(faults, "clear", day: 4);

        Assert.True(result.Success);
        Assert.Single(result.Observations);
        Assert.InRange(result.Observations[0].confidence, 0f, 1f);
        Assert.True(result.Observations[0].energized);
        Assert.Equal(0, inventory.CountById("battery"));
    }

    [Fact]
    public void MissingBatteryDoesNotRecordObservation()
    {
        var engine = new UvCoronaDetectionEngine(new SeededRng(119), UvCoronaDetectionCatalogLoader.Load(DataDirectory));
        engine.Equip("uv_corona_camera_mk1");

        var result = engine.Scan(new List<ElectricalFaultInput> { new ElectricalFaultInput { fault_id = "fault_a", fault_intensity = 1f } }, "clear");

        Assert.False(result.Success);
        Assert.Equal(UvCoronaFailureCodes.BatteryDepleted, result.FailureCode);
        Assert.Empty(engine.State.observations);
    }

    [Fact]
    public void RangeAndVisibilityReduceSignalWithoutMutatingFaultInput()
    {
        var nearInventory = new InventoryModel();
        nearInventory.AddById("battery", 1);
        var near = new UvCoronaDetectionEngine(new SeededRng(22), UvCoronaDetectionCatalogLoader.Load(DataDirectory));
        near.BindInventory(nearInventory);
        near.Equip("uv_corona_camera_mk1");
        var input = new ElectricalFaultInput { fault_id = "fault_a", fault_intensity = 0.8f, distance = 1f, energized = true };
        near.Scan(new[] { input }, "clear");

        var farInventory = new InventoryModel();
        farInventory.AddById("battery", 1);
        var far = new UvCoronaDetectionEngine(new SeededRng(22), UvCoronaDetectionCatalogLoader.Load(DataDirectory));
        far.BindInventory(farInventory);
        far.Equip("uv_corona_camera_mk1");
        far.Scan(new[] { new ElectricalFaultInput { fault_id = "fault_a", fault_intensity = 0.8f, distance = 4.5f, energized = true } }, "clear");

        Assert.Equal(0.8f, input.fault_intensity);
        Assert.True(near.State.observations[0].signal > far.State.observations[0].signal);
    }

    [Fact]
    public void StateRoundTripPreservesObservation()
    {
        var inventory = new InventoryModel();
        inventory.AddById("battery", 1);
        var engine = new UvCoronaDetectionEngine(new SeededRng(7), UvCoronaDetectionCatalogLoader.Load(DataDirectory));
        engine.BindInventory(inventory);
        engine.Equip("uv_corona_camera_mk1");
        engine.Scan(new[] { new ElectricalFaultInput { fault_id = "fault_a", asset_id = "asset_a", fault_intensity = 0.9f, distance = 1f } }, "clear");

        var restored = new UvCoronaDetectionEngine(new SeededRng(7), UvCoronaDetectionCatalogLoader.Load(DataDirectory));
        restored.RestoreState(engine.CaptureState());

        Assert.Single(restored.State.observations);
        Assert.Equal(engine.State.observations[0].confidence, restored.State.observations[0].confidence);
        Assert.Equal(engine.State.observations[0].asset_id, restored.State.observations[0].asset_id);
    }
}
