using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AtomicWar._Game.Core;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Data; // ItemCatalogSO, RecipeCatalogSO, etc.
using AtomicWar._Game.Flashpoint; // FlashpointSequenceSO
// Aliases for class-vs-namespace collisions.
using ShelterClass = AtomicWar._Game.Shelter.Shelter;
using InventoryClass = AtomicWar._Game.Inventory.Inventory;

namespace AtomicWar.Tests.PlayMode
{
    /// <summary>
    /// Audit H-1: TimeSystem substep watchdog. The Update loop in
    /// GameBootstrap drives the per-frame game-time substep budget. When the
    /// budget is exceeded, leftover time rolls into the next frame (no time
    /// is lost) but the player should be warned. These tests verify the
    /// overflow detection, the carry-over correctness, and the no-false-
    /// positive case.
    ///
    /// The tests construct a real GameBootstrap in a test scene, drive the
    /// public TickFrame(float) method with controlled dt, and inspect the
    /// read-only counter properties.
    /// </summary>
    [TestFixture]
    public class TimeSystemWatchdogTests
    {
        private GameObject _bootstrapGo;
        private GameBootstrap _bootstrap;

        [SetUp]
        public void SetUp()
        {
            // Build the bootstrap with the SerializeField dependencies
            // injected BEFORE Awake() runs. The trick: instantiate the
            // GameObject as disabled, AddComponent (which doesn't fire
            // Awake on a disabled GO), inject the fields, then enable.
            _bootstrapGo = new GameObject("TestBootstrap");
            _bootstrapGo.SetActive(false);
            _bootstrap = _bootstrapGo.AddComponent<GameBootstrap>();
            InjectBootstrapFields(_bootstrap);
            _bootstrapGo.SetActive(true);
        }

        [TearDown]
        public void TearDown()
        {
            if (_bootstrapGo != null) Object.DestroyImmediate(_bootstrapGo);
        }

        // -----------------------------------------------------------------
        // H-1: No false positive
        // -----------------------------------------------------------------

        [Test]
        public void TickFrame_SmallDelta_NoOverflowCounters()
        {
            // 10ms real time × 3x fast-forward = 30ms game time = ~0.03 game-hours.
            // With MaxGameHoursPerStep=1, this is well under the 128-substep budget.
            int before = _bootstrap.DropEventCount;
            float totalBefore = _bootstrap.TotalDroppedGameHours;

            // Tick 60 times at 10ms each (simulating 0.6 seconds of real time).
            for (int i = 0; i < 60; i++)
            {
                _bootstrap.TickFrame(0.01f);
            }

            Assert.AreEqual(before, _bootstrap.DropEventCount, "Small deltas must not trigger the watchdog.");
            Assert.AreEqual(totalBefore, _bootstrap.TotalDroppedGameHours, 0.001f);
        }

        // -----------------------------------------------------------------
        // H-1: Overflow detection
        // -----------------------------------------------------------------

        [Test]
        public void TickFrame_LargeDelta_TriggersWatchdog()
        {
            // 30 minutes real time at default scale (1x) and 10s/game-hour
            // = 180 game-hours. With MaxGameHoursPerStep=1 and
            // MaxSubstepsPerFrame=128, the frame processes 128 hours
            // and carries 52 hours into the next frame.
            int before = _bootstrap.DropEventCount;
            float totalBefore = _bootstrap.TotalDroppedGameHours;

            // Single huge frame.
            _bootstrap.TickFrame(1800f); // 30 minutes real time

            Assert.AreEqual(before + 1, _bootstrap.DropEventCount, "Overflow must increment DropEventCount.");
            Assert.Greater(_bootstrap.TotalDroppedGameHours, totalBefore + 10f, "TotalDroppedGameHours must grow by the carry amount.");
            Assert.Greater(_bootstrap.LastFrameDroppedGameHours, 0f, "LastFrameDroppedGameHours must be > 0 after overflow.");
        }

        [Test]
        public void TickFrame_MediumDelta_NotDroppedWhenFitsInBudget()
        {
            // 100 seconds real time at 3x = 300 game-seconds = 5 game-hours.
            // 5 substeps to process (under the 128 budget).
            int before = _bootstrap.DropEventCount;
            _bootstrap.TickFrame(100f);
            // The watchdog only fires when the carry > 0 AFTER hitting the
            // substep cap. 5 substeps is well under 128, so no drop.
            Assert.AreEqual(before, _bootstrap.DropEventCount, "5 substeps must not trigger the watchdog.");
        }

        // -----------------------------------------------------------------
        // H-1: Carry-over correctness
        // -----------------------------------------------------------------

        [Test]
        public void TickFrame_Overflow_CarriesOverToNextFrame()
        {
            // First frame: 1 hour real time (3600s) at default 1x scale →
            // 360 game-hours. With substep cap 128 and 1h/step, the frame
            // processes 128 hours and carries 232 hours.
            _bootstrap.TickFrame(3600f);
            float carried1 = _bootstrap.LastFrameDroppedGameHours;
            Assert.Greater(carried1, 200f, "First frame must carry > 200 game-hours.");

            // The pending time stays in _pendingGameHours and is consumed
            // on the next frame. Tick a small frame and assert that the
            // pending time decreased but is still > 0 (the carry is too
            // large to consume in one follow-up frame at default 0.05h).
            float pendingBefore = (float)GetPrivateField(_bootstrap, "_pendingGameHours");
            Assert.Greater(pendingBefore, 200f, "Pending time should be > 200 hours after 1h overflow.");
            _bootstrap.TickFrame(0.5f); // 0.5s × 1x = 0.05 game-hours added; substep loop processes 128 hours.
            float pendingAfter = (float)GetPrivateField(_bootstrap, "_pendingGameHours");
            // The carry should have decreased (some game-time was processed).
            Assert.Less(pendingAfter, pendingBefore, "Pending time should decrease on the next frame.");
            // But not all carry should be consumed: 232 + 0.05 - 128 = ~104 hours remain.
            Assert.Greater(pendingAfter, 100f, "But not all carry should be consumed in one normal frame.");
            // And the watchdog must fire AGAIN because the carry exceeded the cap.
            Assert.GreaterOrEqual(_bootstrap.DropEventCount, 2, "Carry-over must trigger another drop event.");
        }

        [Test]
        public void TickFrame_RepeatedOverflow_AccumulatesTotalDroppedHours()
        {
            // Three back-to-back overflow frames. Each carries 52 game-hours
            // (180 hours - 128 hours processed), so the total accumulates
            // by ~156 hours across three frames.
            float before = _bootstrap.TotalDroppedGameHours;
            _bootstrap.TickFrame(1800f);
            _bootstrap.TickFrame(1800f);
            _bootstrap.TickFrame(1800f);
            float delta = _bootstrap.TotalDroppedGameHours - before;
            Assert.Greater(delta, 100f, "Three overflow frames must accumulate >100 game-hours of carry.");
            // Each frame should have produced a drop event.
            Assert.AreEqual(3, _bootstrap.DropEventCount, "Three overflow frames must each increment DropEventCount.");
        }

        [Test]
        public void TickFrame_OverflowThenNormal_DoesNotDoubleCountNormalFrame()
        {
            // The first frame overflows; the second is small. The
            // follow-up frame is "normal" only if the carry from the first
            // frame is fully consumed in one substep budget.
            // First frame: 1800s (30 min) → 180 game-hours → 128 processed,
            // 52 carry. The carry is under 128 so the next frame can
            // consume it in one budget.
            _bootstrap.TickFrame(1800f);
            int dropsAfterOverflow = _bootstrap.DropEventCount;
            Assert.AreEqual(1, dropsAfterOverflow);

            // 100s = 10 game-hours, well under 128. Plus the 52-hour carry
            // totals 62 — still under 128. So no overflow this frame.
            _bootstrap.TickFrame(100f);
            Assert.AreEqual(dropsAfterOverflow, _bootstrap.DropEventCount,
                "A normal frame must not increment DropEventCount.");
            // LastFrameDroppedGameHours DOES reset to 0 on a normal frame.
            Assert.AreEqual(0f, _bootstrap.LastFrameDroppedGameHours, 0.001f);
        }

        [Test]
        public void TickFrame_Overflow_LogsWarningOnFirstEvent()
        {
            // The first overflow must log a warning. LogAssert.Expect catches
            // the expected message before the warning becomes a test failure.
            UnityEngine.TestTools.LogAssert.Expect(LogType.Warning,
                new System.Text.RegularExpressions.Regex(@"\[TimeSystemWatchdog\] Frame exceeded \d+ substeps.*"));
            _bootstrap.TickFrame(1800f);
        }

        // -----------------------------------------------------------------
        // H-1: Peak substep tracking
        // -----------------------------------------------------------------

        [Test]
        public void TickFrame_Overflow_RecordsPeakSubsteps()
        {
            Assert.AreEqual(0, _bootstrap.PeakSubstepsInOneFrame);
            _bootstrap.TickFrame(1800f);
            Assert.GreaterOrEqual(_bootstrap.PeakSubstepsInOneFrame, 128, "PeakSubstepsInOneFrame must hit the budget on overflow.");
        }

        [Test]
        public void TickFrame_NormalFrame_UpdatesPeakToActualCount()
        {
            // Even a normal frame updates the peak to its actual substep count.
            int beforePeak = _bootstrap.PeakSubstepsInOneFrame;
            _bootstrap.TickFrame(0.01f);
            Assert.GreaterOrEqual(_bootstrap.PeakSubstepsInOneFrame, beforePeak,
                "PeakSubstepsInOneFrame is monotonic — it must not decrease.");
        }

        // -----------------------------------------------------------------
        // H-1: Reset behavior
        // -----------------------------------------------------------------

        [Test]
        public void TickFrame_Overflow_DoesNotResetOnNormalFrame()
        {
            // Once DropEventCount increments, it must stay incremented even
            // after a normal frame. TotalDroppedGameHours is also cumulative.
            _bootstrap.TickFrame(1800f);
            int dropsAtPeak = _bootstrap.DropEventCount;
            float totalAtPeak = _bootstrap.TotalDroppedGameHours;
            Assert.Greater(dropsAtPeak, 0);
            Assert.Greater(totalAtPeak, 0f);

            // A normal frame must not reset the counters.
            for (int i = 0; i < 100; i++) _bootstrap.TickFrame(0.01f);
            Assert.AreEqual(dropsAtPeak, _bootstrap.DropEventCount);
            Assert.AreEqual(totalAtPeak, _bootstrap.TotalDroppedGameHours, 0.001f);
            // LastFrameDroppedGameHours DOES reset on a normal frame.
            Assert.AreEqual(0f, _bootstrap.LastFrameDroppedGameHours, 0.001f);
        }

        // -----------------------------------------------------------------
        // Helpers
        // -----------------------------------------------------------------

        /// <summary>
        /// Reflective accessor for private fields. Used by tests to verify
        /// internal state without breaking encapsulation in production code.
        /// </summary>
        private static object GetPrivateField(object instance, string fieldName)
        {
            var f = instance.GetType().GetField(fieldName,
                BindingFlags.NonPublic | BindingFlags.Instance);
            return f?.GetValue(instance);
        }

        /// <summary>
        /// Inject empty ScriptableObject instances for every [SerializeField]
        /// dependency the bootstrap requires. Without this, Awake() throws
        /// NullReferenceException on the first system that needs a profile.
        /// </summary>
        private static void InjectBootstrapFields(GameBootstrap bs)
        {
            // Profile ScriptableObjects.
            SetPrivate(bs, "_needsProfile", ScriptableObject.CreateInstance<NeedsProfile>());
            SetPrivate(bs, "_lightProfile", ScriptableObject.CreateInstance<LightProfile>());
            SetPrivate(bs, "_seasonProfile", ScriptableObject.CreateInstance<SeasonProfile>());
            // Catalog ScriptableObjects.
            SetPrivate(bs, "_itemCatalog", ScriptableObject.CreateInstance<ItemCatalogSO>());
            SetPrivate(bs, "_recipeCatalog", ScriptableObject.CreateInstance<RecipeCatalogSO>());
            SetPrivate(bs, "_eventCatalog", ScriptableObject.CreateInstance<GameEventCatalogSO>());
            SetPrivate(bs, "_locationCatalog", ScriptableObject.CreateInstance<LocationCatalogSO>());
            SetPrivate(bs, "_radioCatalog", ScriptableObject.CreateInstance<RadioCatalogSO>());
            SetPrivate(bs, "_worldPhaseConfig", ScriptableObject.CreateInstance<WorldPhaseConfigSO>());
            SetPrivate(bs, "_flashpointSequence", ScriptableObject.CreateInstance<FlashpointSequenceSO>());
            SetPrivate(bs, "_mentalBreakCatalog", ScriptableObject.CreateInstance<MentalBreakCatalogSO>());
            SetPrivate(bs, "_lootTable", ScriptableObject.CreateInstance<LootTableSO>());
            // HUD reference is null-safe (the bootstrap nulls-checks _hud).
            // Don't create one — it's a MonoBehaviour and would need a canvas.
        }

        private static void SetPrivate(object instance, string fieldName, object value)
        {
            var f = instance.GetType().GetField(fieldName,
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (f == null) throw new System.MissingFieldException(
                $"{instance.GetType().Name} has no field '{fieldName}'.");
            f.SetValue(instance, value);
        }
    }
}
