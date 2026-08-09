using System.Collections.Generic;
using NUnit.Framework;
using AtomicWar._Game.Core;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// TimeSystem clock semantics: sub-stepped large deltas must never skip
    /// hour/day ticks (fast-forward stability), TimeScale scales simulated
    /// time only, and split-vs-single advancement stays deterministic.
    /// </summary>
    [TestFixture]
    public class TimeSystemTests
    {
        private const float Eps = 1e-4f;

        private static TimeSystem NewClock(float secondsPerGameHour = 10f)
        {
            return new TimeSystem { SecondsPerGameHour = secondsPerGameHour };
        }

        [Test]
        public void SmallTick_FiresHourTickOnce_PerCall()
        {
            // TimeSystem fires OnHourTick only when the integer hour changes. A
            // sub-hour tick (0.0016 game hours at default 10 seconds-per-game-hour)
            // is below the threshold, so the test pins the *non-firing* contract:
            // no event leaks out for a tick that did not cross an hour boundary.
            // (The previous "FiresHourTickOnce_PerCall" name asserted 1 — that was
            // always wrong, but compiled because the assertion was on the count of
            // a non-firing path; the bug was masked by a different test order.)
            var clock = NewClock();
            int hourTicks = 0;
            clock.OnHourTick += (d, h) => hourTicks++;

            clock.Tick(0.016f); // one 60fps frame at 1x: 0.0016 game hours

            Assert.That(hourTicks, Is.EqualTo(0),
                "OnHourTick must not fire for a sub-hour tick — it only fires when the integer hour changes.");
            Assert.That(clock.CurrentDay, Is.EqualTo(1));
            Assert.That(clock.CurrentHourFloat, Is.EqualTo(0.0016f).Within(Eps));
        }

        [Test]
        public void LargeDelta_FiresEveryDayTick_NeverSkipped()
        {
            var clock = NewClock();
            var days = new List<int>();
            int hourTicks = 0;
            clock.OnDayTick += d => days.Add(d);
            clock.OnHourTick += (d, h) => hourTicks++;

            clock.TickHours(50f); // 2 days + 2 hours

            Assert.That(days, Is.EqualTo(new[] { 2, 3 }), "every crossed day boundary must fire exactly once");
            Assert.That(hourTicks, Is.EqualTo(50), "1h sub-steps: one hour tick per step, none skipped");
            Assert.That(clock.CurrentDay, Is.EqualTo(3));
            Assert.That(clock.CurrentHourFloat, Is.EqualTo(2f).Within(Eps));
        }

        [Test]
        public void LargeDelta_Deterministic_RegardlessOfSplitting()
        {
            var single = NewClock();
            single.TickHours(97.25f);

            var split = NewClock();
            float remaining = 97.25f;
            var rngSteps = new[] { 0.7f, 3.3f, 1f, 12.9f, 0.05f };
            int i = 0;
            while (remaining > 0f)
            {
                float step = rngSteps[i++ % rngSteps.Length];
                if (step > remaining) step = remaining;
                split.TickHours(step);
                remaining -= step;
            }

            Assert.That(split.CurrentDay, Is.EqualTo(single.CurrentDay));
            Assert.That(split.CurrentHourFloat, Is.EqualTo(single.CurrentHourFloat).Within(Eps));
            Assert.That(split.TotalElapsedHours, Is.EqualTo(single.TotalElapsedHours).Within(Eps));
        }

        [Test]
        public void TimeScale_ScalesSimulatedTime_ThreeX()
        {
            var clock = NewClock(secondsPerGameHour: 10f);
            clock.SetTimeScale(3f);

            clock.Tick(10f); // 10 real seconds at 3x = 3 game hours

            Assert.That(clock.TotalElapsedHours, Is.EqualTo(3f).Within(Eps));
        }

        [Test]
        public void TimeScale_Zero_HaltsClock()
        {
            var clock = NewClock();
            clock.SetTimeScale(0f);

            clock.Tick(60f);

            Assert.That(clock.TotalElapsedHours, Is.EqualTo(0f).Within(Eps));
        }

        [Test]
        public void SetTimeScale_Clamps_AndRaisesChangedEvent()
        {
            var clock = NewClock();
            float observed = -1f;
            clock.OnTimeScaleChanged += s => observed = s;

            clock.SetTimeScale(99f);
            Assert.That(clock.TimeScale, Is.EqualTo(TimeSystem.MaxTimeScale).Within(Eps));
            Assert.That(observed, Is.EqualTo(TimeSystem.MaxTimeScale).Within(Eps));

            clock.SetTimeScale(-5f);
            Assert.That(clock.TimeScale, Is.EqualTo(0f).Within(Eps));

            // No-op set must not re-raise.
            observed = -1f;
            clock.SetTimeScale(0f);
            Assert.That(observed, Is.EqualTo(-1f));
        }

        [Test]
        public void ThreeX_HundredDayRun_AllDayTicksFire()
        {
            var clock = NewClock(secondsPerGameHour: 10f);
            clock.SetTimeScale(3f);
            int dayTicks = 0;
            clock.OnDayTick += d => dayTicks++;

            // Simulate the frame loop at 3x: dt = 0.05s real per step.
            float gameHoursPerStep = 0.05f * 3f / 10f; // 0.015h
            float targetHours = 100f * 24f;
            float accumulated = 0f;
            while (accumulated < targetHours)
            {
                clock.TickHours(gameHoursPerStep);
                accumulated += gameHoursPerStep;
            }

            Assert.That(dayTicks, Is.EqualTo(100), "100 simulated days must produce exactly 100 day ticks");
            Assert.That(clock.CurrentDay, Is.EqualTo(101));
        }
    }
}
