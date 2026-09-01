using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests;

public class WaystationCommandTests
{
    [Fact]
    public void PreviewAssignWatch_Available_WhenUnlocked()
    {
        var sys = new WaystationSystem();
        sys.Unlock();

        var preview = sys.PreviewAssignWatch(new[] { "scout_a" }, stateVersion: 2L);

        Assert.True(preview.IsAvailable);
        Assert.Equal(2L, preview.StateVersion);
        Assert.Equal("waystation.preview_assign_watch", preview.MessageKey);
    }

    [Fact]
    public void ExecuteAssignWatch_StalePreview_RejectsWithoutMutation()
    {
        var sys = new WaystationSystem();
        sys.Unlock();

        var result = sys.ExecuteAssignWatch(new[] { "scout_a" }, expectedStateVersion: 99L, currentStateVersion: 100L);

        Assert.False(result.IsSuccess);
        Assert.Equal("stale_preview", result.FailureCode);
        Assert.Empty(sys.State.watchSurvivorIds);
    }

    [Fact]
    public void ExecuteAssignWatch_FreshPreview_Assigns()
    {
        var sys = new WaystationSystem();
        sys.Unlock();

        var result = sys.ExecuteAssignWatch(new[] { "scout_a" }, expectedStateVersion: 2L, currentStateVersion: 2L);

        Assert.True(result.IsSuccess);
        Assert.Single(sys.State.watchSurvivorIds);
        Assert.Equal(2L, result.ExpectedStateVersion);
        Assert.Equal(3L, result.ActualStateVersion);
    }
}
