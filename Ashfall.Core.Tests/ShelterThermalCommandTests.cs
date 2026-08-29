using Xunit;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core.Tests;

public class ShelterThermalCommandTests
{
    [Fact]
    public void PreviewRepairPipe_Available_WhenPipeIsBurst()
    {
        var sys = Engine();
        sys.AddPipe("pipe_main", "room_a", "room_b");
        sys.State.pipes[0].condition = 20f;
        sys.State.pipes[0].hasBurst = true;

        var preview = sys.PreviewRepairPipe("pipe_main", 10f, stateVersion: 10L);

        Assert.True(preview.IsAvailable);
        Assert.Equal(10L, preview.StateVersion);
        Assert.Equal("thermal.preview_repair", preview.MessageKey);
    }

    [Fact]
    public void PreviewRepairPipe_Unavailable_WhenPipeNotBurst()
    {
        var sys = Engine();
        sys.AddPipe("pipe_main", "room_a", "room_b");
        sys.State.pipes[0].condition = 80f;
        sys.State.pipes[0].hasBurst = false;

        var preview = sys.PreviewRepairPipe("pipe_main", 10f, stateVersion: 10L);

        Assert.False(preview.IsAvailable);
        Assert.Equal("not_burst", preview.FailureCode);
    }

    [Fact]
    public void ExecuteRepairPipe_StalePreview_RejectsWithoutMutation()
    {
        var sys = Engine();
        sys.AddPipe("pipe_main", "room_a", "room_b");
        sys.State.pipes[0].condition = 20f;
        sys.State.pipes[0].hasBurst = true;

        var result = sys.ExecuteRepairPipe("pipe_main", 10f, expectedStateVersion: 99L, currentStateVersion: 100L);

        Assert.False(result.IsSuccess);
        Assert.Equal("stale_preview", result.FailureCode);
    }

    [Fact]
    public void ExecuteRepairPipe_MatchingVersions_RepairsPipe()
    {
        var sys = Engine();
        sys.AddPipe("pipe_main", "room_a", "room_b");
        sys.State.pipes[0].condition = 20f;
        sys.State.pipes[0].hasBurst = true;

        var result = sys.ExecuteRepairPipe("pipe_main", 10f, expectedStateVersion: 10L, currentStateVersion: 10L);

        Assert.True(result.IsSuccess);
        Assert.Equal("thermal.pipe_repaired", result.MessageKey);
        Assert.Equal(30f, sys.State.pipes[0].condition);
    }

    private static ShelterThermalSystem Engine()
    {
        var rng = new SeededRng(42);
        var needs = new NeedsSystem();
        var starting = new StartingLevelSystem();
        var deepFreeze = new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState());
        return new ShelterThermalSystem(rng, needs, starting, deepFreeze);
    }
}
