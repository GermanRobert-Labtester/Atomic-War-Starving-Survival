using Ashfall.Core;
using Ashfall.Core.StartingLevel;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class VentilationSystemTests
    {
        private static (VentilationSystem vent, StartingLevelSystem sl) Create()
        {
            var sl = new StartingLevelSystem();
            var vent = new VentilationSystem(sl);
            return (vent, sl);
        }

        [Fact] public void RegisterSource_AddsSource()
        {
            var (v, _) = Create();
            v.RegisterSource(new VentilationSource { sourceId = "foundry", smokeOutputPerDay = 10f, coOutputPerDay = 5f, requiresExhaust = true });
            Assert.Single(v.State.activeSources);
        }

        [Fact] public void SetValve_UpdatesState()
        {
            var (v, _) = Create();
            v.SetValve("foundry", true);
            Assert.True(v.State.valveToFoundryOpen);
        }

        [Fact] public void TickDay_WithNoSources_NoAccumulation()
        {
            var (v, _) = Create();
            v.TickDay(1);
            Assert.Equal(0, v.SmokeSoot);
        }

        [Fact] public void TickDay_WithActiveSourceNoExhaust_Accumulates()
        {
            var (v, sl) = Create();
            sl.State.airFilterHealthPercent = 20f; // degraded filter for test
            v.RegisterSource(new VentilationSource { sourceId = "foundry", smokeOutputPerDay = 10f, coOutputPerDay = 20f, requiresExhaust = true });
            v.SetSourceActive("foundry", true);
            v.SetValve("foundry", false);
            v.TickDay(1);
            Assert.True(v.SmokeSoot > 0);
            Assert.True(v.CarbonMonoxide > 0);
        }

        [Fact] public void ServiceFilter_ReducesSaturation()
        {
            var (v, _) = Create();
            v.State.exhaustFilterSaturation = 80f;
            v.ServiceFilter();
            Assert.True(v.State.exhaustFilterSaturation < 80f);
        }

        [Fact] public void ReplaceFilter_ResetsSaturation()
        {
            var (v, _) = Create();
            v.State.exhaustFilterSaturation = 90f;
            v.ReplaceFilter();
            Assert.Equal(0, v.State.exhaustFilterSaturation);
        }

        [Fact] public void CaptureRestoreState_PreservesState()
        {
            var (v, _) = Create();
            v.RegisterSource(new VentilationSource { sourceId = "gen", smokeOutputPerDay = 5f, coOutputPerDay = 3f, requiresExhaust = true });
            v.SetSourceActive("gen", true);
            var state = v.CaptureState();
            Assert.Single(state.activeSources);

            var (v2, _) = Create();
            v2.RestoreState(state);
            Assert.Single(v2.State.activeSources);
            Assert.True(v2.State.activeSources[0].isActive);
        }
    }
}
