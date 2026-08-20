using Ashfall.Core;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class WeatherStationSystemTests
    {
        [Fact] public void Install_WhenNotInstalled_Succeeds()
        {
            var ws = Create(out _);
            var r = ws.Install(1);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.True(ws.State.isInstalled);
        }

        [Fact] public void Install_WhenAlreadyInstalled_Blocks()
        {
            var ws = Create(out _);
            ws.Install(1);
            var r = ws.Install(2);
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void Calibrate_WhenNotInstalled_Blocks()
        {
            var ws = Create(out _);
            var r = ws.Calibrate(1);
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void IsOperational_AfterInstallAndCalibrate_True()
        {
            var ws = Create(out _);
            ws.Install(1);
            ws.Calibrate(2);
            Assert.True(ws.IsOperational);
        }

        [Fact] public void GenerateForecast_WhenOperational_ReturnsEntries()
        {
            var ws = Create(out _);
            ws.Install(1);
            ws.Calibrate(2);
            var r = ws.GenerateForecast(10);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.NotEmpty(ws.GetForecast());
        }

        [Fact] public void GenerateForecast_WhenNotOperational_Blocks()
        {
            var ws = Create(out _);
            var r = ws.GenerateForecast(1);
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void CaptureRestoreState_PreservesInstallation()
        {
            var ws = Create(out _);
            ws.Install(5);
            ws.Calibrate(6);
            var state = ws.CaptureState();
            Assert.True(state.isCalibrated);

            var ws2 = Create(out _);
            ws2.RestoreState(state);
            Assert.True(ws2.IsOperational);
        }

        private static WeatherStationSystem Create(out WeatherSystem weather)
        {
            weather = new WeatherSystem();
            weather.BindProfile(new SeasonProfileDef { id = "default" }, 42);
            return new WeatherStationSystem(weather, new SeededRng(42));
        }
    }
}
