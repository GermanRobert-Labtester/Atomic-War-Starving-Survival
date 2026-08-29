using Xunit;
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Tests;

public class AirlockSecurityCommandTests
{
    [Fact]
    public void PreviewRepairDoor_Available_WithPositiveAmount()
    {
        var sys = Engine();
        sys.State.blastDoorIntegrity = 60f;

        var preview = sys.PreviewRepairDoor(10f, stateVersion: 10L);

        Assert.True(preview.IsAvailable);
        Assert.Equal(10L, preview.StateVersion);
        Assert.Equal("airlock.preview_repair", preview.MessageKey);
    }

    [Fact]
    public void PreviewRepairDoor_Unavailable_WithNonPositiveAmount()
    {
        var sys = Engine();
        sys.State.blastDoorIntegrity = 60f;

        var preview = sys.PreviewRepairDoor(0f, stateVersion: 10L);

        Assert.False(preview.IsAvailable);
        Assert.Equal("invalid_amount", preview.FailureCode);
    }

    [Fact]
    public void ExecuteRepairDoor_StalePreview_RejectsWithoutMutation()
    {
        var sys = Engine();
        sys.State.blastDoorIntegrity = 60f;

        var result = sys.ExecuteRepairDoor(10f, expectedStateVersion: 99L, currentStateVersion: 100L);

        Assert.False(result.IsSuccess);
        Assert.Equal("stale_preview", result.FailureCode);
    }

    [Fact]
    public void ExecuteRepairDoor_MatchingVersions_RepairsDoor()
    {
        var sys = Engine();
        sys.State.blastDoorIntegrity = 60f;

        var result = sys.ExecuteRepairDoor(10f, expectedStateVersion: 10L, currentStateVersion: 10L);

        Assert.True(result.IsSuccess);
        Assert.Equal("airlock.door_repaired", result.MessageKey);
        Assert.Equal(70f, sys.State.blastDoorIntegrity);
    }

    private static AirlockSecuritySystem Engine()
    {
        return new AirlockSecuritySystem(new SeededRng(42));
    }
}
