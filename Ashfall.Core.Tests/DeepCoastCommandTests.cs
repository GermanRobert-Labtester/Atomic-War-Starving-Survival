using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests;

public class DeepCoastCommandTests
{
    [Fact]
    public void PreviewRepairDeepBerth_Available_WhenDockAccessible()
    {
        var sys = new District8DeepCoastSystem(1);
        sys.State.stage = (int)DeepCoastStage.DockAccessible;

        var preview = sys.PreviewRepairDeepBerth(day: 100, stateVersion: 5L);

        Assert.True(preview.IsAvailable);
        Assert.Equal(5L, preview.StateVersion);
        Assert.Equal("deepcoast.preview_repair_berth", preview.MessageKey);
    }

    [Fact]
    public void ExecuteRepairDeepBerth_StalePreview_RejectsWithoutMutation()
    {
        var sys = new District8DeepCoastSystem(1);
        sys.State.stage = (int)DeepCoastStage.DockAccessible;

        bool consumed = false;
        var result = sys.ExecuteRepairDeepBerth(day: 100, tryConsumeBill: bill => consumed = true, expectedStateVersion: 99L, currentStateVersion: 100L);

        Assert.False(result.IsSuccess);
        Assert.Equal("stale_preview", result.FailureCode);
        Assert.False(consumed);
    }
}
