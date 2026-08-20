using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class OrbitalHarrowTelemetrySystemTests
    {
        [Fact] public void ActivateTelemetry_EnablesSystem()
        {
            var oh = Create(out _);
            oh.ActivateTelemetry(1);
            Assert.True(oh.State.telemetryActive);
        }

        [Fact] public void ScheduleImpact_CreatesWarning()
        {
            var oh = Create(out _);
            oh.ScheduleImpact(10, 5, 25f);
            Assert.Single(oh.State.warnings);
            Assert.True(oh.HasPendingImpact);
        }

        [Fact] public void Brace_MitigatesImpact()
        {
            var oh = Create(out _);
            oh.ScheduleImpact(10, 5, 25f);
            var r = oh.Brace("concrete", 5);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.True(oh.State.isBraced);
        }

        [Fact] public void TickDay_OnImpactDay_Resolves()
        {
            var oh = Create(out _);
            oh.ScheduleImpact(10, 5, 25f);
            bool resolved = false;
            oh.OnImpactResolved += (_, _) => resolved = true;
            oh.TickDay(10);
            Assert.True(resolved);
            Assert.False(oh.HasPendingImpact);
        }

        [Fact] public void Brace_WhenNoImpact_Blocks()
        {
            var oh = Create(out _);
            var r = oh.Brace("concrete", 5);
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void CaptureRestoreState_PreservesImpact()
        {
            var oh = Create(out _);
            oh.ScheduleImpact(10, 5, 25f);
            var state = oh.CaptureState();
            Assert.Equal(10, state.nextImpactDay);

            var oh2 = Create(out _);
            oh2.RestoreState(state);
            Assert.True(oh2.HasPendingImpact);
        }

        private static OrbitalHarrowTelemetrySystem Create(out SkyLayerArmorSystem armor)
        {
            armor = new SkyLayerArmorSystem();
            armor.SetCellArmor(5, CeilingMaterialTier.ReinforcedConcrete, 0.5f);
            return new OrbitalHarrowTelemetrySystem(armor, new SeededRng(42));
        }
    }
}
