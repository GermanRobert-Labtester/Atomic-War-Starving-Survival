using Xunit;
using Ashfall.Core.Medical;

namespace Ashfall.Core.Tests;

public class ChemicalDependencyCommandTests
{
    [Fact]
    public void PreviewBeginManagedDetox_Available_WhenDependencyExists()
    {
        var sys = Engine();
        sys.OnSubstanceConsumed("sv_1", "painkillers", ChemicalDependencyKind.Opioid);
        sys.OnSubstanceConsumed("sv_1", "painkillers", ChemicalDependencyKind.Opioid);

        var preview = sys.PreviewBeginManagedDetox("sv_1", "painkillers", stateVersion: 10L);

        Assert.True(preview.IsAvailable);
        Assert.Equal(10L, preview.StateVersion);
        Assert.Equal("medical.preview_managed_detox", preview.MessageKey);
    }

    [Fact]
    public void PreviewBeginManagedDetox_Unavailable_WhenNoDependency()
    {
        var sys = Engine();

        var preview = sys.PreviewBeginManagedDetox("sv_1", "painkillers", stateVersion: 10L);

        Assert.False(preview.IsAvailable);
        Assert.Equal("missing_dependency", preview.FailureCode);
    }

    [Fact]
    public void ExecuteBeginManagedDetox_StalePreview_RejectsWithoutMutation()
    {
        var sys = Engine();
        sys.OnSubstanceConsumed("sv_1", "painkillers", ChemicalDependencyKind.Opioid);
        sys.OnSubstanceConsumed("sv_1", "painkillers", ChemicalDependencyKind.Opioid);

        var result = sys.ExecuteBeginManagedDetox("sv_1", "painkillers", expectedStateVersion: 99L, currentStateVersion: 100L);

        Assert.False(result.IsSuccess);
        Assert.Equal("stale_preview", result.FailureCode);
    }

    [Fact]
    public void ExecuteBeginManagedDetox_MatchingVersions_StartsDetox()
    {
        var sys = Engine();
        sys.OnSubstanceConsumed("sv_1", "painkillers", ChemicalDependencyKind.Opioid);
        sys.OnSubstanceConsumed("sv_1", "painkillers", ChemicalDependencyKind.Opioid);

        var result = sys.ExecuteBeginManagedDetox("sv_1", "painkillers", expectedStateVersion: 10L, currentStateVersion: 10L);

        Assert.True(result.IsSuccess);
        Assert.Equal("medical.managed_detox_started", result.MessageKey);
    }

    [Fact]
    public void PreviewBeginColdTurkey_Available_WhenDependencyExists()
    {
        var sys = Engine();
        sys.OnSubstanceConsumed("sv_1", "painkillers", ChemicalDependencyKind.Opioid);
        sys.OnSubstanceConsumed("sv_1", "painkillers", ChemicalDependencyKind.Opioid);

        var preview = sys.PreviewBeginColdTurkey("sv_1", "painkillers", stateVersion: 10L);

        Assert.True(preview.IsAvailable);
        Assert.Equal(10L, preview.StateVersion);
        Assert.Equal("medical.preview_cold_turkey", preview.MessageKey);
    }

    [Fact]
    public void ExecuteBeginColdTurkey_StalePreview_RejectsWithoutMutation()
    {
        var sys = Engine();
        sys.OnSubstanceConsumed("sv_1", "painkillers", ChemicalDependencyKind.Opioid);
        sys.OnSubstanceConsumed("sv_1", "painkillers", ChemicalDependencyKind.Opioid);

        var result = sys.ExecuteBeginColdTurkey("sv_1", "painkillers", expectedStateVersion: 99L, currentStateVersion: 100L);

        Assert.False(result.IsSuccess);
        Assert.Equal("stale_preview", result.FailureCode);
    }

    [Fact]
    public void ExecuteBeginColdTurkey_MatchingVersions_StartsColdTurkey()
    {
        var sys = Engine();
        sys.OnSubstanceConsumed("sv_1", "painkillers", ChemicalDependencyKind.Opioid);
        sys.OnSubstanceConsumed("sv_1", "painkillers", ChemicalDependencyKind.Opioid);

        var result = sys.ExecuteBeginColdTurkey("sv_1", "painkillers", expectedStateVersion: 10L, currentStateVersion: 10L);

        Assert.True(result.IsSuccess);
        Assert.Equal("medical.cold_turkey_started", result.MessageKey);
    }

    private static ChemicalDependencySystem Engine()
    {
        return new ChemicalDependencySystem();
    }
}
