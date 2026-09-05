using System;
using Ashfall.Core.Balance;
using Xunit;
using Xunit.Abstractions;

namespace Ashfall.Core.Tests;

/// <summary>
/// Mass-balance simulation tests validating that the integrated survival loop
/// (water, power, nutrition, trapping, greenhouse) remains stable over 30-day
/// and 200-day horizons without resource inflation or starvation deadlocks.
///
/// The simulator itself enforces strict daily invariants:
///   - Water discrepancy per day must be &lt; 0.05 L (absolute, not relative).
///   - Food inventory must not exceed 300 units (inflation gate).
/// The Success + InvariantViolations fields are the authoritative result.
/// </summary>
public class ResourceMassBalanceSimulationTests
{
    private readonly ITestOutputHelper _out;

    public ResourceMassBalanceSimulationTests(ITestOutputHelper output)
    {
        _out = output;
    }

    // ── Config helpers ────────────────────────────────────────────────────────

    /// <summary>
    /// "Test" scenario bypasses the simulator's built-in Baseline assertions
    /// so individual tests can control which conditions they assert.
    /// </summary>
    private static ResourceMassBalanceConfig Cfg(int seed = 42, int days = 30) => new()
    {
        Seed              = seed,
        Days              = days,
        CrewSize          = 4,
        InitialCleanWater = 200f,
        InitialRawWater   = 500f,
        InitialFuel       = 300f,
        InitialCannedFood = 120,
        InitialRawMeat    = 10,
        EnableTrapping    = true,
        EnableGreenhouse  = true,
        ScenarioName      = "Test"
    };

    private string Dump(ResourceMassBalanceResult r)
        => $"alive={r.SurvivorsAlive}  health={r.AvgSurvivorHealth:F1}  " +
           $"maxDisc={r.MaxWaterDiscrepancy:F4}L  meals={r.TotalMealsServed}  " +
           $"meat={r.TotalMeatProduced}  crops={r.TotalCropsHarvested}  " +
           $"brownout={r.TotalBrownoutHours:F1}h  fuel={r.TotalFuelBurned:F1}  " +
           $"violations=[{string.Join("; ", r.InvariantViolations)}]";

    // ── 30-Day Baseline ───────────────────────────────────────────────────────

    [Fact]
    public void Baseline30Day_BuiltInInvariantsPass()
    {
        // The "Baseline" scenario name enables the simulator's own gate:
        // 100% survival + avg health >= 60 required.
        var cfg = Cfg(42, 30);
        cfg.ScenarioName = "Baseline";

        var res = ResourceMassBalanceSimulator.Run(cfg);
        _out.WriteLine(Dump(res));

        Assert.True(res.Success,
            $"Baseline violated built-in invariants: {string.Join("; ", res.InvariantViolations)}");
        Assert.Empty(res.InvariantViolations);
    }

    [Fact]
    public void Baseline30Day_AllSurvivorsSurvive()
    {
        var res = ResourceMassBalanceSimulator.Run(Cfg(42, 30));
        _out.WriteLine(Dump(res));
        Assert.Equal(4, res.SurvivorsAlive);
    }

    [Fact]
    public void Baseline30Day_WaterMassConservation_MaxDiscrepancyUnder0p05()
    {
        var res = ResourceMassBalanceSimulator.Run(Cfg(42, 30));
        _out.WriteLine(Dump(res));

        // Simulator gates at 0.05 L/day absolute — any violation appears in InvariantViolations.
        // Verify no water invariant violations occurred.
        foreach (var v in res.InvariantViolations)
            Assert.False(v.Contains("Water mass balance"),
                $"Water mass balance violation: {v}");

        Assert.True(res.MaxWaterDiscrepancy < 0.05,
            $"Max daily water discrepancy {res.MaxWaterDiscrepancy:F4}L >= 0.05L tolerance");
    }

    [Fact]
    public void Baseline30Day_FoodInflationGateRespected()
    {
        var res = ResourceMassBalanceSimulator.Run(Cfg(42, 30));
        _out.WriteLine(Dump(res));

        // Simulator gates at food > 300 units. No inflation violation should appear.
        foreach (var v in res.InvariantViolations)
            Assert.False(v.Contains("Resource inflation"),
                $"Food inflation invariant violated: {v}");
    }

    [Fact]
    public void Baseline30Day_PowerRemainsStable()
    {
        var res = ResourceMassBalanceSimulator.Run(Cfg(42, 30));
        _out.WriteLine(Dump(res));

        // Brownout hours < 25% of total (30 days * 24h = 720h; 25% = 180h)
        Assert.True(res.TotalBrownoutHours < 180f,
            $"Excessive brownout: {res.TotalBrownoutHours:F1}h / 720h total");
    }

    [Fact]
    public void Baseline30Day_FoodAndMealsNonNegative()
    {
        var res = ResourceMassBalanceSimulator.Run(Cfg(42, 30));
        Assert.True(res.TotalMealsServed  >= 0, "Meals served must be non-negative");
        Assert.True(res.TotalMeatProduced >= 0, "Meat produced must be non-negative");
        Assert.True(res.TotalFoodSpoiled  >= 0, "Spoiled food must be non-negative");
        Assert.True(res.TotalCropsHarvested >= 0, "Crop harvests must be non-negative");
    }

    // ── 200-Day Long Horizon ──────────────────────────────────────────────────

    [Fact]
    public void LongHorizon200Day_CrewSurvivesWithoutInflation()
    {
        var cfg = Cfg(7, 200);
        cfg.InitialFuel = 3500f;  // fuel for the full 200-day haul (16.8/day)
        cfg.InitialCannedFood = 250; // buffer below 300-item inflation ceiling
        var res = ResourceMassBalanceSimulator.Run(cfg);
        _out.WriteLine(Dump(res));

        // At least 50% crew survival rate
        Assert.True(res.SurvivorsAlive >= 2,
            $"Expected >= 2 survivors at day 200, got {res.SurvivorsAlive}");

        // Meat inflation ceiling: 2 trappers, 200 days, max realistic 8 catches/day = 1600
        Assert.True(res.TotalMeatProduced < 2000,
            $"Meat inflation: {res.TotalMeatProduced} units in 200 days exceeds 2000-unit ceiling");

        // No food inflation invariant violations
        foreach (var v in res.InvariantViolations)
            Assert.False(v.Contains("Resource inflation"),
                $"200-day food inflation violation: {v}");
    }

    [Fact]
    public void LongHorizon200Day_WaterConservationHolds()
    {
        var cfg = Cfg(7, 200);
        cfg.InitialFuel = 3500f;
        var res = ResourceMassBalanceSimulator.Run(cfg);
        _out.WriteLine(Dump(res));

        // Max daily discrepancy still must be < 0.05 L
        Assert.True(res.MaxWaterDiscrepancy < 0.05,
            $"200-day max water discrepancy {res.MaxWaterDiscrepancy:F4}L >= 0.05L");

        foreach (var v in res.InvariantViolations)
            Assert.False(v.Contains("Water mass balance"),
                $"200-day water violation: {v}");
    }

    // ── Determinism ───────────────────────────────────────────────────────────

    [Fact]
    public void Determinism_SameSeedProducesIdenticalResults()
    {
        var cfg  = Cfg(999, 30);
        var runA = ResourceMassBalanceSimulator.Run(cfg);
        var runB = ResourceMassBalanceSimulator.Run(cfg);

        Assert.Equal(runA.SurvivorsAlive,     runB.SurvivorsAlive);
        Assert.Equal(runA.TotalMealsServed,   runB.TotalMealsServed);
        Assert.Equal(runA.TotalMeatProduced,  runB.TotalMeatProduced);
        Assert.Equal(runA.TotalCropsHarvested, runB.TotalCropsHarvested);
        Assert.Equal(runA.MaxWaterDiscrepancy, runB.MaxWaterDiscrepancy);
        Assert.Equal(runA.TotalFuelBurned,     runB.TotalFuelBurned);
    }

    [Fact]
    public void Determinism_DifferentSeedsProduceDifferentResults()
    {
        var runA = ResourceMassBalanceSimulator.Run(Cfg(1, 30));
        var runB = ResourceMassBalanceSimulator.Run(Cfg(2, 30));

        // At least one stochastic metric must differ across seeds
        bool anyDifference =
            runA.TotalMeatProduced    != runB.TotalMeatProduced
            || runA.TotalBrownoutHours != runB.TotalBrownoutHours
            || runA.TotalCropsHarvested != runB.TotalCropsHarvested;

        Assert.True(anyDifference,
            "Different seeds produced identical stochastic results -- check RNG seeding");
    }

    // ── Stress Scenarios ──────────────────────────────────────────────────────

    [Fact]
    public void StressScenario_LowResources_NoDeadlockOrException()
    {
        var cfg = Cfg(42, 30);
        cfg.InitialCleanWater = 20f;
        cfg.InitialRawWater   = 40f;
        cfg.InitialFuel       = 30f;
        cfg.InitialCannedFood = 8;
        cfg.InitialRawMeat    = 2;

        // Must not throw — sim runs to completion even if crew starves
        var res = ResourceMassBalanceSimulator.Run(cfg);
        _out.WriteLine(Dump(res));
        Assert.Equal(30, res.DaysSimulated);
    }

    [Fact]
    public void StressScenario_TrappingDisabled_StillRunsToCompletion()
    {
        var cfg = Cfg(42, 30);
        cfg.EnableTrapping    = false;
        cfg.InitialCannedFood = 120;  // compensate for no trapping output

        var res = ResourceMassBalanceSimulator.Run(cfg);
        _out.WriteLine(Dump(res));

        Assert.Equal(30, res.DaysSimulated);
        Assert.True(res.SurvivorsAlive >= 2,
            $"Expected >= 2 survivors with no trapping, got {res.SurvivorsAlive}");
        Assert.Equal(0, res.TotalMeatProduced);  // trapping off = no meat
    }

    [Fact]
    public void StressScenario_GreenhouseDisabled_ZeroCropTranspiration()
    {
        var cfg = Cfg(42, 30);
        cfg.EnableGreenhouse = false;

        var res      = ResourceMassBalanceSimulator.Run(cfg);
        var resGreen = ResourceMassBalanceSimulator.Run(Cfg(42, 30));

        Assert.Equal(0.0, res.TotalWaterCropTranspiration);
        _out.WriteLine($"Greenhouse OFF transpiration: {res.TotalWaterCropTranspiration:F1}L");
        _out.WriteLine($"Greenhouse ON transpiration: {resGreen.TotalWaterCropTranspiration:F1}L");
    }

    [Fact]
    public void StressScenario_PowerGridDisabled_NoBrownout()
    {
        var cfg = Cfg(42, 30);
        cfg.EnablePowerGrid = false;

        var res = ResourceMassBalanceSimulator.Run(cfg);
        _out.WriteLine(Dump(res));

        Assert.Equal(0f, res.TotalBrownoutHours);
        Assert.Equal(0f, res.TotalFuelBurned);
    }
}
