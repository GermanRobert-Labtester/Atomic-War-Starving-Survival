// SPDX-License-Identifier: MIT
using Ashfall.Core.Combat;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using Xunit;
using InventoryStore = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests;

public sealed class Plans74To77SystemsTests
{
    [Fact]
    public void GeothermalOrc_OperatesAndRestoresDeterministically()
    {
        var definition = new GeothermalStratumDefinition
        {
            StratumId = "stratum_test",
            DisplayName = "Test Reservoir",
            DepthM = 500f,
            BaseRockTemperatureC = 180f,
            EnthalpyKjPerKg = 2800f,
            ThermalCapacityFactor = 1f,
            MaxSafeExtractionKw = 1000f,
            RecoveryRatePerDay = 1f
        };
        var system = new GeothermalOrcSystem(new SeededRng(74));
        system.RegisterStratum(definition);

        Assert.True(system.AddLoop("loop_test", definition.StratumId));
        Assert.True(system.CommissionLoop("loop_test").IsSuccess);
        Assert.True(system.SetFlow("loop_test", 200f).IsSuccess);

        var first = system.OperateDay(1);
        Assert.True(first.Active);
        Assert.True(first.ElectricalOutputKw > 0f);
        Assert.True(first.WasteHeatKw > 0f);

        var restored = new GeothermalOrcSystem(new SeededRng(999));
        restored.RegisterStratum(definition);
        restored.RestoreState(system.CaptureState());
        var second = restored.Snapshot();

        Assert.Equal(first.LoopId, second.LoopId);
        Assert.Equal(first.ElectricalOutputKw, second.ElectricalOutputKw);
        Assert.Equal(first.ThermalReservePct, second.ThermalReservePct);
    }

    [Fact]
    public void BallisticsWorkbench_UsesEventIdForIdempotentWear()
    {
        var system = new BallisticsWorkbenchSystem(new SeededRng(75));
        system.RegisterDefinition(new BallisticsWorkbenchDefinition
        {
            ProfileId = "profile_test",
            WeaponTag = "rifle",
            BaseDispersionMoa = 3f,
            WearPerShotFactor = 0.1f,
            MaxCalibrationBonus = 0.2f,
            HeadspaceWarningThreshold = 0.5f,
            HeadspaceFailureThreshold = 0.9f
        });
        var profile = system.EnsureProfile("weapon_test", "profile_test");

        var first = system.RecordFiring("weapon_test", "combat_event_1", 10, false, false);
        var afterFirst = profile.TotalRounds;
        var repeated = system.RecordFiring("weapon_test", "combat_event_1", 10, false, false);

        Assert.True(first.Accepted);
        Assert.True(repeated.Accepted);
        Assert.Equal(afterFirst, profile.TotalRounds);
        Assert.True(system.Calibrate("weapon_test", 1f, 1f, 2).IsSuccess);
        Assert.True(system.GetCombatModifier("weapon_test").AccuracyMultiplier > 0f);
    }

    [Fact]
    public void BallisticsWorkbench_AttachOptic_ImprovesBoundedAccuracyProjection()
    {
        var system = new BallisticsWorkbenchSystem(new SeededRng(7501));
        system.RegisterDefinition(new BallisticsWorkbenchDefinition
        {
            ProfileId = "profile_test",
            BaseDispersionMoa = 3f,
            MaxOpticBonus = 0.15f,
            HeadspaceWarningThreshold = 0.5f,
            HeadspaceFailureThreshold = 0.9f
        });
        system.EnsureProfile("weapon_test", "profile_test");
        var before = system.GetCombatModifier("weapon_test");

        var attached = system.AttachOptic("weapon_test", 0.9f);
        var withOptic = system.GetCombatModifier("weapon_test");

        Assert.True(attached.IsSuccess);
        Assert.Equal(0.9f, system.FindProfile("weapon_test")!.OpticQuality, 3);
        Assert.True(withOptic.AccuracyMultiplier >= before.AccuracyMultiplier);
        Assert.InRange(withOptic.AccuracyMultiplier, 0.5f, 1.2f);
        Assert.Equal(before.MalfunctionMultiplier, withOptic.MalfunctionMultiplier, 3);
    }

    [Fact]
    public void BallisticsWorkbench_ApplyToCombatWeapon_WritesProjectionTokens()
    {
        var system = new BallisticsWorkbenchSystem(new SeededRng(7502));
        system.RegisterDefinition(new BallisticsWorkbenchDefinition
        {
            ProfileId = "profile_test",
            BaseDispersionMoa = 3f,
            MaxOpticBonus = 0.15f,
            HeadspaceWarningThreshold = 0.5f,
            HeadspaceFailureThreshold = 0.9f
        });
        system.EnsureProfile("weapon_test", "profile_test");
        Assert.True(system.AttachOptic("weapon_test", 1f).IsSuccess);

        var token = new WeaponInstanceState { InstanceId = "weapon_test" };
        system.ApplyToCombatWeapon(token);
        var modifier = system.GetCombatModifier("weapon_test");

        Assert.Equal(modifier.AccuracyMultiplier, token.BallisticsAccuracyMultiplier, 3);
        Assert.Equal(modifier.RangeMultiplier, token.BallisticsRangeMultiplier, 3);
        Assert.Equal(modifier.PenetrationMultiplier, token.BallisticsPenetrationMultiplier, 3);
        Assert.Equal(modifier.CriticalMultiplier, token.BallisticsCriticalMultiplier, 3);
        Assert.Equal(modifier.MalfunctionMultiplier, token.BallisticsMalfunctionMultiplier, 3);
    }

    [Fact]
    public void BallisticsWorkbench_OpticQuality_RoundTripsAndClamps()
    {
        var system = new BallisticsWorkbenchSystem(new SeededRng(7503));
        system.RegisterDefinition(new BallisticsWorkbenchDefinition
        {
            ProfileId = "profile_test",
            BaseDispersionMoa = 3f,
            HeadspaceWarningThreshold = 0.5f,
            HeadspaceFailureThreshold = 0.9f
        });
        system.EnsureProfile("weapon_test", "profile_test");
        Assert.True(system.AttachOptic("weapon_test", 4f).IsSuccess);

        var restored = new BallisticsWorkbenchSystem(new SeededRng(999));
        restored.RegisterDefinition(new BallisticsWorkbenchDefinition
        {
            ProfileId = "profile_test",
            BaseDispersionMoa = 3f,
            HeadspaceWarningThreshold = 0.5f,
            HeadspaceFailureThreshold = 0.9f
        });
        restored.RestoreState(system.CaptureState());

        Assert.Equal(1f, restored.FindProfile("weapon_test")!.OpticQuality, 3);
    }

    [Fact]
    public void Aeroponics_GrowsAndReturnsHarvestToCanonicalInventory()
    {
        var inventory = new InventoryStore();
        var system = new AeroponicsSystem(new SeededRng(76), inventory);
        system.RegisterProfile(new AeroponicNutrientDefinition
        {
            NutrientProfileId = "profile_test",
            CropTag = "food_leaf",
            OptimalEcMin = 1f,
            OptimalEcMax = 2f,
            OptimalPhMin = 5.5f,
            OptimalPhMax = 6.5f,
            YieldItemId = "item_aeroponic_food_leaf",
            YieldAmount = 2,
            BaseGrowthPerDay = 20f
        });

        Assert.True(system.AddChamber("chamber_test", "room_greenhouse").IsSuccess);
        Assert.True(system.Plant("chamber_test", "cycle_test", "profile_test", 1).IsSuccess);
        Assert.True(system.AddWater("chamber_test", 40f, 1f).IsSuccess);
        for (int day = 1; day <= 6; day++)
            system.TickDay(day);

        var harvest = system.Harvest("chamber_test", 7);
        Assert.True(harvest.Success);
        Assert.Equal("item_aeroponic_food_leaf", harvest.ItemId);
        Assert.Equal(harvest.Amount, inventory.CountById(harvest.ItemId));
    }

    [Fact]
    public void PneumaticDispatch_PreservesCargoAndBlocksDuringBlackout()
    {
        var source = new InventoryStore();
        var destination = new InventoryStore();
        source.AddById("item_pneumatic_capsule_50mm", 2);
        var system = new PneumaticDispatchSystem(new SeededRng(77));
        system.LoadCatalog(new PneumaticNetworkCatalog
        {
            Stations =
            {
                new PneumaticStationDefinition
                {
                    StationId = "station_a",
                    MaxCargoMassKg = 5f,
                    MaxCargoVolumeLitres = 5f
                },
                new PneumaticStationDefinition
                {
                    StationId = "station_b",
                    MaxCargoMassKg = 5f,
                    MaxCargoVolumeLitres = 5f
                }
            },
            Links =
            {
                new PneumaticLinkDefinition
                {
                    LinkId = "link_ab",
                    FromStationId = "station_a",
                    ToStationId = "station_b",
                    LengthM = 10f,
                    CapsuleStandard = "capsule_50mm",
                    BaseSealCondition = 100f
                }
            },
            CapsuleStandards =
            {
                new PneumaticCapsuleStandardDefinition
                {
                    StandardId = "capsule_50mm",
                    DiameterMm = 50f,
                    BaseSpeedKmh = 30f
                }
            }
        });
        system.RegisterEndpoint("station_a", source);
        system.RegisterEndpoint("station_b", destination);

        var blockedBeforeDispatch = system.Dispatch(
            "station_a", "station_b", "item_pneumatic_capsule_50mm",
            1, 0.5f, 1f, PneumaticDispatchPriority.Normal, 1);
        Assert.True(blockedBeforeDispatch.Success);
        Assert.Equal(1, source.CountById("item_pneumatic_capsule_50mm"));

        system.SetBlackout(true);
        var blocked = system.Dispatch(
            "station_a", "station_b", "item_pneumatic_capsule_50mm",
            1, 0.5f, 1f, PneumaticDispatchPriority.Normal, 1);
        Assert.False(blocked.Success);
        Assert.Equal("blower_unpowered", blocked.FailureCode);

        system.SetBlackout(false);
        system.TickDay(1);
        Assert.Equal(1, destination.CountById("item_pneumatic_capsule_50mm"));
        Assert.Equal(1, source.CountById("item_pneumatic_capsule_50mm"));
    }
}
