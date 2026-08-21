using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class WaterTreatmentIntegrationTests
    {
        [Fact]
        public void AddWater_AndStartCharcoalFiltration_ProducesCleanOutput()
        {
            var sys = new WaterTreatmentSystem();
            sys.AddWater(WaterType.Raw, 50f);
            sys.State.charcoalSupply = 20f;
            Assert.Equal(50f, sys.RawWater);

            var start = sys.StartTreatment(TreatmentMode.CharcoalFiltration, 20f);
            Assert.True(start.IsSuccess);
            Assert.True(sys.IsProcessing);

            sys.TickDay(1);
            Assert.False(sys.IsProcessing);
            Assert.True(sys.CleanWater > 0f);
            Assert.True(sys.State.totalWaterProcessed > 0f);
        }

        [Fact]
        public void ReplaceFilter_RestoresIntegrity()
        {
            var sys = new WaterTreatmentSystem();
            sys.AddWater(WaterType.Raw, 50f);
            sys.State.charcoalSupply = 20f;
            sys.StartTreatment(TreatmentMode.CharcoalFiltration, 30f);
            sys.TickDay(1);

            Assert.True(sys.FilterIntegrity < 100f);
            var rep = sys.ReplaceFilter();
            Assert.True(rep.IsSuccess);
            Assert.Equal(100f, sys.FilterIntegrity);
        }

        [Fact]
        public void SaveAndRestore_PreservesAllReservesAndJobs()
        {
            var sys1 = new WaterTreatmentSystem();
            sys1.AddWater(WaterType.Raw, 100f);
            sys1.AddWater(WaterType.Clean, 25f);
            sys1.State.charcoalSupply = 20f;
            sys1.StartTreatment(TreatmentMode.CharcoalFiltration, 20f);
            sys1.TickDay(1);

            var state = sys1.CaptureState();
            var sys2 = new WaterTreatmentSystem();
            sys2.RestoreState(state);

            Assert.Equal(sys1.CleanWater, sys2.CleanWater);
            Assert.Equal(sys1.RawWater, sys2.RawWater);
            Assert.Equal(sys1.FilterIntegrity, sys2.FilterIntegrity);
        }
    }
}
