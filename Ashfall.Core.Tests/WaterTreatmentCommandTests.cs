using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests;

public class WaterTreatmentCommandTests
{
    [Fact]
    public void PreviewStartTreatment_Available_ShowsProjectedDeltas()
    {
        var wt = new WaterTreatmentSystem();
        wt.AddWater(WaterType.Raw, 10f);
        wt.AddCharcoal(1f);

        var preview = wt.PreviewStartTreatment(TreatmentMode.CharcoalFiltration, 5f, stateVersion: 7L);

        Assert.True(preview.IsAvailable);
        Assert.Equal(7L, preview.StateVersion);
        Assert.Equal(5, preview.ProjectedDeltas["input"]);
        Assert.Equal(1, preview.ProjectedDeltas["mode"]);
    }

    [Fact]
    public void ExecuteStartTreatment_StalePreview_RejectsWithoutMutation()
    {
        var wt = new WaterTreatmentSystem();
        wt.AddWater(WaterType.Raw, 10f);
        wt.AddCharcoal(1f);

        var result = wt.ExecuteStartTreatment(TreatmentMode.CharcoalFiltration, 5f, expectedStateVersion: 99L, currentStateVersion: 100L);

        Assert.False(result.IsSuccess);
        Assert.Equal("stale_preview", result.FailureCode);
        Assert.False(wt.IsProcessing);
    }

    [Fact]
    public void ExecuteStartTreatment_FreshPreview_Executes()
    {
        var wt = new WaterTreatmentSystem();
        wt.AddWater(WaterType.Raw, 10f);
        wt.AddCharcoal(1f);

        var result = wt.ExecuteStartTreatment(TreatmentMode.CharcoalFiltration, 5f, expectedStateVersion: 5L, currentStateVersion: 5L);

        Assert.True(result.IsSuccess);
        Assert.Equal(5L, result.ExpectedStateVersion);
        Assert.Equal(6L, result.ActualStateVersion);
        Assert.True(wt.IsProcessing);
    }

    [Fact]
    public void PreviewCancelTreatment_Unavailable_WhenNotProcessing()
    {
        var wt = new WaterTreatmentSystem();

        var preview = wt.PreviewCancelTreatment(stateVersion: 1L);

        Assert.False(preview.IsAvailable);
        Assert.Equal("not_processing", preview.FailureCode);
    }

    [Fact]
    public void ExecuteCancelTreatment_StalePreview_RejectsWithoutMutation()
    {
        var wt = new WaterTreatmentSystem();
        wt.AddWater(WaterType.Raw, 10f);
        wt.AddCharcoal(1f);
        wt.StartTreatment(TreatmentMode.CharcoalFiltration, 5f);

        var result = wt.ExecuteCancelTreatment(expectedStateVersion: 99L, currentStateVersion: 100L);

        Assert.False(result.IsSuccess);
        Assert.Equal("stale_preview", result.FailureCode);
        Assert.True(wt.IsProcessing);
    }
}
