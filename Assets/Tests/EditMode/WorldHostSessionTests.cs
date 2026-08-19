using System.Collections.Generic;
using NUnit.Framework;
using AtomicWar.GodotApp;
using Ashfall.Core;
using Ashfall.Core.World;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class WorldHostSessionTests
    {
        private WorldHostSession _session = null!;

        [SetUp]
        public void Setup()
        {
            var system = new WeatherSystem();
            system.BindProfile(new SeasonProfileDef()
            {
                id = "test",
                seasons = new List<SeasonWindowDef>()
                {
                    new SeasonWindowDef()
                }
            }, 1234);

            _session = new WorldHostSession(system);
        }

        [Test]
        public void TickDemo_AdvancesWeatherAndReturnsFormattedString()
        {
            string output = _session.TickDemo(5f);

            Assert.AreEqual(5f, _session.Weather.State.totalElapsedHours);
            Assert.IsTrue(output.StartsWith("Tick 5h:"));
            Assert.IsTrue(output.Contains("(rolls"));
        }

        [Test]
        public void ForceDemo_ForcesWeatherKindAndReturnsFormattedString()
        {
            string output = _session.ForceDemo(WeatherKind.BlackRain);

            Assert.AreEqual(WeatherKind.BlackRain, _session.Weather.Current);
            Assert.AreEqual("Weather forced to BlackRain.", output);
        }

        [Test]
        public void StatusLine_ReturnsFormattedWeatherStatus()
        {
            _session.ForceDemo(WeatherKind.Clear);
            string output = _session.StatusLine();

            Assert.IsTrue(output.Contains("Weather: Clear"));
            Assert.IsTrue(output.Contains("visibility"));
            Assert.IsTrue(output.Contains("outdoor rad"));
            Assert.IsTrue(output.Contains("temp penalty"));
        }
    }
}
