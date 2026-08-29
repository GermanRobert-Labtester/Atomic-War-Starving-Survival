using Xunit;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests;

public class CaregivingCommandTests
{
    private static CaregivingSystem CreateSystem()
    {
        var sys = new CaregivingSystem();
        sys.IsAlive = id => id != "sv_dead";
        sys.CanProvideCare = id => id != "sv_incap";
        sys.NeedsCare = id => id != "sv_healthy";
        return sys;
    }

    [Fact]
    public void PreviewAssignCaregiver_ValidIds_ShowsAvailable()
    {
        var sys = CreateSystem();

        var preview = sys.PreviewAssignCaregiver("sv_caregiver", "sv_patient", stateVersion: 5L);

        Assert.True(preview.IsAvailable);
        Assert.Equal(5L, preview.StateVersion);
        Assert.Equal("caregiving.preview_assign", preview.MessageKey);
    }

    [Fact]
    public void ExecuteAssignCaregiver_StalePreview_RejectsWithoutMutation()
    {
        var sys = CreateSystem();

        var result = sys.ExecuteAssignCaregiver("sv_caregiver", "sv_patient", expectedStateVersion: 99L, currentStateVersion: 100L);

        Assert.False(result.IsSuccess);
        Assert.Equal("stale_preview", result.FailureCode);
    }

    [Fact]
    public void ExecuteAssignCaregiver_FreshPreview_Assigns()
    {
        var sys = CreateSystem();

        var result = sys.ExecuteAssignCaregiver("sv_caregiver", "sv_patient", expectedStateVersion: 1L, currentStateVersion: 1L);

        Assert.True(result.IsSuccess);
        Assert.Equal("sv_caregiver", sys.GetCaregiverForPatient("sv_patient"));
        Assert.Equal(1L, result.ExpectedStateVersion);
        Assert.Equal(2L, result.ActualStateVersion);
    }
}
