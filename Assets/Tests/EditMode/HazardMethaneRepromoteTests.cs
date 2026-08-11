using NUnit.Framework;
using AtomicWar._Game.Core;

using AtomicWar._Game.Encounters;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// REPROMOTE-Hazard-001 — breached shelter methane responds to live air quality.
    /// </summary>
    [TestFixture]
    public class HazardMethaneRepromoteTests
    {
        [Test]
        public void ActivePocket_RoundTrips_AndVentsOnlyWithGoodAir()
        {
            var source = new MethaneSystem("hazard_methane");
            source.RestoreState(new MethaneState
            {
                hazardId = "hazard_methane",
                breachChance = 1f,
                isGasPresent = false,
                isDetonated = false
            });
            Assert.That(source.TryExcavate(4, new System.Random(7)), Is.True);

            var restored = new MethaneSystem("hazard_methane");
            restored.RestoreState(source.CaptureState());
            int clearedEvents = 0;
            restored.OnMethaneCleared += id =>
            {
                Assert.That(id, Is.EqualTo("hazard_methane"));
                clearedEvents++;
            };

            restored.Tick(1f, MethaneSystem.VentilationClearAirQuality);
            Assert.That(restored.State.isGasPresent, Is.True,
                "Fair/poor air must not clear an active pocket");
            Assert.That(clearedEvents, Is.Zero);

            restored.Tick(1f, MethaneSystem.VentilationClearAirQuality + 1f);
            Assert.That(restored.State.isGasPresent, Is.False);
            Assert.That(clearedEvents, Is.EqualTo(1));
        }

        [Test]
        public void InactiveState_AirTicksAreNoOp()
        {
            var methane = new MethaneSystem("hazard_methane");
            MethaneState before = methane.CaptureState();
            int clearedEvents = 0;
            methane.OnMethaneCleared += _ => clearedEvents++;

            methane.Tick(24f, 100f);

            MethaneState after = methane.CaptureState();
            Assert.That(after.hazardId, Is.EqualTo(before.hazardId));
            Assert.That(after.breachChance, Is.EqualTo(before.breachChance));
            Assert.That(after.isGasPresent, Is.EqualTo(before.isGasPresent));
            Assert.That(after.isDetonated, Is.EqualTo(before.isDetonated));
            Assert.That(clearedEvents, Is.Zero);
        }
    }
}
