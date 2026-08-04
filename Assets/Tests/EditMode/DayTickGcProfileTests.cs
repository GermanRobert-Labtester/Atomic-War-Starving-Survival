using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Medical;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Day-tick GC profile: measures managed allocations across a warm 100-day
    /// pure-C# tick loop (EventRunner + MentalBreak + Phantom + Addiction +
    /// Empath + SkillAtrophy + ShiftingHotspot). Guards regressions that re-
    /// introduce per-tick <c>new System.Random</c> / list churn on the hot path.
    ///
    /// Absolute budgets are editor-friendly; the primary signal is that the
    /// steady window is not dramatically worse than the early window.
    /// </summary>
    [TestFixture]
    public class DayTickGcProfileTests
    {
        private const int TotalDays = 100;
        private const int WarmupDays = 10;
        private const int EarlyEndDay = 30;
        private const int SteadyStartDay = 70;
        private const int SteadyEndDay = 100;

        /// <summary>Hard ceiling per steady day (bytes). Catches MB-scale leaks.</summary>
        private const long MaxBytesPerSteadyDay = 256L * 1024;

        /// <summary>Steady window may not grow more than 3x early + 1 MB slack.</summary>
        private const long GrowthSlackBytes = 1024L * 1024;

        [Test]
        public void HundredDayTickLoop_SteadyAlloc_WithinBudget()
        {
            var survivors = BuildCrew();
            var rng = new System.Random(42);
            var mental = new MentalBreakSystem();
            var phantom = new PhantomIntruderSystem();
            var addiction = new AddictionSystem(rng);
            addiction.RegisterAddictiveItem("anti_rad");
            var empath = new EmpathSystem();
            var atrophy = new SkillAtrophySystem();
            var map = MapGenerator.Generate(9001);
            var hotspots = new ShiftingHotspotSystem(new System.Random(7));
            hotspots.Bind(map, null);

            var runner = new EventRunner();
            var pool = new List<GameEvent>
            {
                MakeEvent("evt_noise", 1.2f),
                MakeEvent("evt_quiet", 0.8f),
                MakeEvent("evt_chain_only", 0f) // scheduled-only — must never random-pick
            };
            runner.SetPool(pool);

            var ctx = new EventContext(survivors[0], null, null, rng)
            {
                AllSurvivors = survivors,
                CurrentWeather = WeatherKind.Clear
            };

            // Warm-up: fill buffers / JIT so GC measurement is steady-state.
            for (int d = 1; d <= WarmupDays; d++)
            {
                TickOneDay(d, survivors, mental, phantom, addiction, empath, atrophy,
                    hotspots, runner, ctx, rng);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long earlyBefore = GC.GetTotalMemory(false);
            for (int d = WarmupDays + 1; d <= EarlyEndDay; d++)
            {
                TickOneDay(d, survivors, mental, phantom, addiction, empath, atrophy,
                    hotspots, runner, ctx, rng);
            }
            long earlyAfter = GC.GetTotalMemory(false);
            long earlyBytes = Math.Max(0, earlyAfter - earlyBefore);
            int earlyDays = EarlyEndDay - WarmupDays;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long steadyBefore = GC.GetTotalMemory(false);
            for (int d = SteadyStartDay; d <= SteadyEndDay; d++)
            {
                TickOneDay(d, survivors, mental, phantom, addiction, empath, atrophy,
                    hotspots, runner, ctx, rng);
            }
            long steadyAfter = GC.GetTotalMemory(false);
            long steadyBytes = Math.Max(0, steadyAfter - steadyBefore);
            int steadyDays = SteadyEndDay - SteadyStartDay + 1;

            long earlyPerDay = earlyBytes / Math.Max(1, earlyDays);
            long steadyPerDay = steadyBytes / Math.Max(1, steadyDays);

            Debug.Log(
                $"[DayTickGcProfile] early={earlyBytes}B ({earlyPerDay} B/day over {earlyDays}d) " +
                $"steady={steadyBytes}B ({steadyPerDay} B/day over {steadyDays}d)");

            Assert.That(steadyPerDay, Is.LessThan(MaxBytesPerSteadyDay),
                $"steady-state day tick GC too high: {steadyPerDay} B/day " +
                $"(budget {MaxBytesPerSteadyDay} B/day). early={earlyPerDay} B/day");

            Assert.That(steadyBytes, Is.LessThanOrEqualTo(earlyBytes * 3 + GrowthSlackBytes),
                $"allocation growth: early={earlyBytes}B steady={steadyBytes}B");

            // SelectEvent must ignore weight-0 chain events (regression for pool leak).
            for (int i = 0; i < 50; i++)
            {
                var picked = runner.SelectEvent(ctx);
                if (picked != null)
                    Assert.That(picked.weight, Is.GreaterThan(0f),
                        "SelectEvent must never return scheduled-only (weight<=0) events");
            }
        }

        [Test]
        public void EventRunner_SelectEvent_ReusesValidBuffer_NoGrowthAcrossCalls()
        {
            var runner = new EventRunner();
            var pool = new List<GameEvent>();
            for (int i = 0; i < 40; i++)
                pool.Add(MakeEvent($"evt_{i}", 1f + (i % 3) * 0.1f));
            runner.SetPool(pool);
            var ctx = new EventContext(null, null, null, new System.Random(1))
            {
                CurrentDay = 10,
                CurrentWeather = WeatherKind.Clear
            };

            // Warm
            for (int i = 0; i < 20; i++)
                runner.SelectEvent(ctx);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long before = GC.GetTotalMemory(false);
            for (int i = 0; i < 500; i++)
                runner.SelectEvent(ctx);
            long after = GC.GetTotalMemory(false);
            long delta = Math.Max(0, after - before);

            Debug.Log($"[DayTickGcProfile] SelectEvent x500 delta={delta}B");
            // 500 calls should be near-zero if the valid-events list is reused.
            Assert.That(delta, Is.LessThan(64L * 1024),
                $"SelectEvent buffer reuse regression: {delta}B over 500 calls");
        }

        private static void TickOneDay(
            int day,
            List<Survivor> survivors,
            MentalBreakSystem mental,
            PhantomIntruderSystem phantom,
            AddictionSystem addiction,
            EmpathSystem empath,
            SkillAtrophySystem atrophy,
            ShiftingHotspotSystem hotspots,
            EventRunner runner,
            EventContext ctx,
            System.Random rng)
        {
            ctx.CurrentDay = day;
            // 24 hourly slices of pure systems (mirrors day advance without MonoBehaviour).
            for (int h = 0; h < 24; h++)
            {
                const float hours = 1f;
                mental.Tick(hours, survivors, rng);
                phantom.Tick(hours, survivors, rng);
                addiction.Tick(hours, survivors, day);
                empath.Tick(hours, survivors);
                atrophy.Tick(hours, survivors);
                runner.Tick(hours, ctx);
                // Rare SelectEvent (matches bootstrap ~5%/hour)
                if (rng.NextDouble() < 0.05)
                    runner.SelectEvent(ctx);
            }
            hotspots.TickDay(day);
            runner.TickDay(day, ctx);
        }

        private static List<Survivor> BuildCrew()
        {
            var list = new List<Survivor>();
            for (int i = 0; i < 4; i++)
            {
                var s = new Survivor
                {
                    Id = $"s_{i}",
                    DisplayName = $"S{i}",
                    State = SurvivorState.Idle,
                    RiskBias = i == 0 ? RiskBiasTrait.Empath
                        : i == 1 ? RiskBiasTrait.Paranoid
                        : RiskBiasTrait.Realist
                };
                s.Needs.Morale = 55f + i * 5f;
                s.Needs.Fatigue = 30f;
                s.Needs.Health = 90f;
                list.Add(s);
            }
            return list;
        }

        private static GameEvent MakeEvent(string id, float weight)
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = id;
            ev.title = id;
            ev.bodyText = "profile";
            ev.weight = weight;
            ev.minDay = 0;
            return ev;
        }
    }
}
