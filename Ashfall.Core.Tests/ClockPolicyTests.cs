using Ashfall.Core.Clock;
using Ashfall.Core.Events;
using Ashfall.Core.Flags;
using Ashfall.Core.Verdict;
using Ashfall.Core.Warlords;
using Xunit;

namespace Ashfall.Core.Tests
{
    using SimClock = Ashfall.Core.Clock.SimClock;
    public class ClockPolicyTests
    {
        private sealed class TestWorldCensus : IWorldCensus
        {
            public long LivingCount { get; set; } = 150000;
            public long LivingRegisteredSouls() => LivingCount;
        }

        [Fact]
        public void VerdictCadence_WindowOpensStrictlyEverySevenDaysAtThreeAm_AndIsIdempotent()
        {
            var clock = new SimClock(0);
            var bus = new SimpleEventBus();
            var flags = new InMemoryFlagLedger();
            var rng = new SeededRng(2026);
            var census = new TestWorldCensus();

            var broadcast = new VerdictCensusBroadcast(clock, bus, flags, rng, census);

            // Day 0: 00:00 (tick 0) -> Window closed
            Assert.False(broadcast.IsWindowOpen());

            // Day 0: 03:00 (tick 180) -> Window open (Day 0 % 7 == 0 && Hour 3)
            clock.AdvanceHours(3);
            Assert.Equal(0, clock.DayIndex);
            Assert.Equal(3, clock.HourOfDay);
            Assert.True(broadcast.IsWindowOpen());

            // Day 0: 04:00 -> Window closed
            clock.AdvanceHours(1);
            Assert.False(broadcast.IsWindowOpen());

            // Day 1: 03:00 -> Window closed (Day 1 % 7 != 0)
            clock.SetTick(SimClock.TicksPerDay + 3 * SimClock.TicksPerHour);
            Assert.Equal(1, clock.DayIndex);
            Assert.Equal(3, clock.HourOfDay);
            Assert.False(broadcast.IsWindowOpen());

            // Day 6: 03:00 -> Window closed (Day 6 % 7 != 0)
            clock.SetTick(6 * SimClock.TicksPerDay + 3 * SimClock.TicksPerHour);
            Assert.Equal(6, clock.DayIndex);
            Assert.Equal(3, clock.HourOfDay);
            Assert.False(broadcast.IsWindowOpen());

            // Day 7: 03:00 -> Window open (Day 7 % 7 == 0 && Hour 3)
            clock.SetTick(7 * SimClock.TicksPerDay + 3 * SimClock.TicksPerHour);
            Assert.Equal(7, clock.DayIndex);
            Assert.Equal(3, clock.HourOfDay);
            Assert.True(broadcast.IsWindowOpen());

            // Broadcast execution and idempotency test
            Assert.Empty(bus.PublishedEvents);

            broadcast.BroadcastIfDue();
            // Should publish: carrier.open, header, pause, count, footer, carrier.close (6 events)
            Assert.Equal(6, bus.PublishedEvents.Count);
            Assert.Equal("radio.carrier.open", bus.PublishedEvents[0].name);
            Assert.Equal("radio.carrier.close", bus.PublishedEvents[5].name);
            Assert.Equal(7, broadcast.LastWindowDay);

            // Repeat call in the same window must be idempotent (no duplicate broadcast)
            broadcast.BroadcastIfDue();
            Assert.Equal(6, bus.PublishedEvents.Count);
        }

        [Fact]
        public void WarlordCadence_TickDaily_IsIdempotentAgainstDoubleAdvance()
        {
            var sys = new WarlordDoctrineSystem(seedSalt: 42);
            var rng = new SeededRng(42);
            var context = new WarlordContext();

            // First daily tick on Day 1
            sys.TickDaily(1, rng, context);
            int ops = sys.State.totalOperations;
            int actionDay = sys.State.lastActionDay;
            int askDay = sys.State.lastAskDay;
            int supply = sys.State.supply;

            // Second daily tick on the same day (simulating double-advance or repeated render call)
            sys.TickDaily(1, rng, context);

            Assert.Equal(ops, sys.State.totalOperations);
            Assert.Equal(actionDay, sys.State.lastActionDay);
            Assert.Equal(askDay, sys.State.lastAskDay);
            Assert.Equal(supply, sys.State.supply);
        }

        [Fact]
        public void TickToDayConversion_ArithmeticConstants_AndProgression()
        {
            Assert.Equal(60, SimClock.TicksPerHour);
            Assert.Equal(1440, SimClock.TicksPerDay);

            var clock = new SimClock(0);
            Assert.Equal(0, clock.CurrentTick);
            Assert.Equal(0, clock.DayIndex);
            Assert.Equal(0, clock.HourOfDay);
            Assert.Equal(0, clock.Day);

            // 1 hour
            clock.SetTick(60);
            Assert.Equal(0, clock.DayIndex);
            Assert.Equal(1, clock.HourOfDay);

            // 1 full day (1440 ticks)
            clock.SetTick(1440);
            Assert.Equal(1, clock.DayIndex);
            Assert.Equal(0, clock.HourOfDay);
            Assert.Equal(1, clock.Day);

            // Day 5, 17:45 (5 * 1440 + 17 * 60 + 45 = 7200 + 1020 + 45 = 8265)
            clock.SetTick(8265);
            Assert.Equal(5, clock.DayIndex);
            Assert.Equal(17, clock.HourOfDay);
            Assert.Equal(5, clock.Day);

            // Advances
            clock.AdvanceHours(2);
            Assert.Equal(8265 + 120, clock.CurrentTick);

            clock.AdvanceDays(3);
            Assert.Equal(8265 + 120 + 3 * 1440, clock.CurrentTick);
        }

        [Fact]
        public void TickClock_DeterministicReplay_AndStatePreservation()
        {
            var clockA = new SimClock(0);
            var clockB = new SimClock(0);

            // Exact same sequence of advances
            clockA.AdvanceTicks(45);
            clockA.AdvanceHours(14);
            clockA.AdvanceDays(4);

            clockB.AdvanceTicks(45);
            clockB.AdvanceHours(14);
            clockB.AdvanceDays(4);

            Assert.Equal(clockA.CurrentTick, clockB.CurrentTick);
            Assert.Equal(clockA.DayIndex, clockB.DayIndex);
            Assert.Equal(clockA.HourOfDay, clockB.HourOfDay);

            // State preservation across reload
            long savedTick = clockA.CurrentTick;
            var clockC = new SimClock(savedTick);

            Assert.Equal(clockA.CurrentTick, clockC.CurrentTick);
            Assert.Equal(clockA.DayIndex, clockC.DayIndex);
            Assert.Equal(clockA.HourOfDay, clockC.HourOfDay);

            // Continuation yields identical future state
            clockA.AdvanceTicks(500);
            clockC.AdvanceTicks(500);

            Assert.Equal(clockA.CurrentTick, clockC.CurrentTick);
        }
    }
}
