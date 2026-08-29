using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests;

public class GreenhouseCommandTests
{
    [Fact]
    public void PreviewTreatBlight_Available_WhenPlotHasBlight()
    {
        var gh = new GreenhouseSystem(seed: 1);
        gh.EnsurePlots(2);
        gh.TickDay(1, 6f, 0.5f);
        gh.TickDay(2, 6f, 0.5f);

        var plot = gh.Plots[0];
        if (plot.blight <= 0f)
            plot.blight = 0.5f;

        var preview = gh.PreviewTreatBlight(0, stateVersion: 3L);

        Assert.True(preview.IsAvailable);
        Assert.Equal(3L, preview.StateVersion);
        Assert.Equal("greenhouse.preview_treat_blight", preview.MessageKey);
    }

    [Fact]
    public void ExecuteTreatBlight_StalePreview_RejectsWithoutMutation()
    {
        var gh = new GreenhouseSystem(seed: 1);
        gh.EnsurePlots(2);
        gh.TickDay(1, 6f, 0.5f);
        gh.TickDay(2, 6f, 0.5f);
        var plot = gh.Plots[0];
        if (plot.blight <= 0f) plot.blight = 0.5f;

        var result = gh.ExecuteTreatBlight(0, expectedStateVersion: 99L, currentStateVersion: 100L);

        Assert.False(result.IsSuccess);
        Assert.Equal("stale_preview", result.FailureCode);
        Assert.True(plot.blight > 0f);
    }

    [Fact]
    public void ExecuteTreatBlight_FreshPreview_CuresBlight()
    {
        var gh = new GreenhouseSystem(seed: 1);
        gh.EnsurePlots(2);
        gh.TickDay(1, 6f, 0.5f);
        gh.TickDay(2, 6f, 0.5f);
        var plot = gh.Plots[0];
        if (plot.blight <= 0f) plot.blight = 0.5f;

        var result = gh.ExecuteTreatBlight(0, expectedStateVersion: 3L, currentStateVersion: 3L);

        Assert.True(result.IsSuccess);
        Assert.Equal(0f, plot.blight);
        Assert.Equal(3L, result.ExpectedStateVersion);
        Assert.Equal(4L, result.ActualStateVersion);
    }
}
