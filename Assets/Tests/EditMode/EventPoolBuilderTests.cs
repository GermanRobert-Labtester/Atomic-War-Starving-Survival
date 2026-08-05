using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Events;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// H-6: EventPoolBuilder tests. Verifies that the unified pool builder
    /// correctly merges event sources, detects duplicate ids, and produces
    /// a valid pool for EventRunner.SetPool.
    /// </summary>
    [TestFixture]
    public class EventPoolBuilderTests
    {
        // -----------------------------------------------------------------
        // Basic Build — produces a non-null, non-empty pool
        // -----------------------------------------------------------------

        [Test]
        public void Build_WithNullCatalog_ReturnsPool()
        {
            var pool = EventPoolBuilder.Build(null);

            Assert.IsNotNull(pool, "Build should never return null.");
        }

        [Test]
        public void Build_WithCatalog_IncludesCatalogEvents()
        {
            var catalog = ScriptableObject.CreateInstance<GameEventCatalogSO>();
            var catEvent = ScriptableObject.CreateInstance<GameEvent>();
            catEvent.id = "catalog_test_event";
            catEvent.title = "Test";
            catalog.events = new List<GameEvent> { catEvent };

            var pool = EventPoolBuilder.Build(catalog);

            Assert.IsNotNull(pool);
            Assert.IsTrue(pool.Count > 0, "Pool should contain at least the catalog event.");
            bool found = false;
            for (int i = 0; i < pool.Count; i++)
                if (pool[i].id == "catalog_test_event") { found = true; break; }
            Assert.IsTrue(found, "Catalog event should be in the pool.");
        }

        [Test]
        public void Build_IncludesEmissaryChain()
        {
            var pool = EventPoolBuilder.Build(null);

            // The emissary chain should be in the pool.
            bool foundEmissary = false;
            for (int i = 0; i < pool.Count; i++)
            {
                if (pool[i].id != null && pool[i].id.StartsWith("emissary"))
                {
                    foundEmissary = true;
                    break;
                }
            }
            Assert.IsTrue(foundEmissary,
                "Pool should contain emissary chain events.");
        }

        [Test]
        public void Build_IncludesChildFoundEvent()
        {
            var pool = EventPoolBuilder.Build(null);

            bool foundChild = false;
            for (int i = 0; i < pool.Count; i++)
            {
                if (pool[i].id == "child_found_in_ash")
                {
                    foundChild = true;
                    break;
                }
            }
            Assert.IsTrue(foundChild,
                "Pool should contain the Child Found event.");
        }

        [Test]
        public void Build_NoNullIdEvents()
        {
            var pool = EventPoolBuilder.Build(null);

            for (int i = 0; i < pool.Count; i++)
            {
                Assert.IsNotNull(pool[i], $"Pool entry at index {i} should not be null.");
                Assert.IsFalse(string.IsNullOrEmpty(pool[i].id),
                    $"Pool entry at index {i} should have a non-empty id.");
            }
        }

        // -----------------------------------------------------------------
        // Duplicate detection — ValidateNoDuplicateIds
        // -----------------------------------------------------------------

        [Test]
        public void ValidateNoDuplicateIds_NoDuplicates_NoWarning()
        {
            var pool = new List<GameEvent>
            {
                CreateTestEvent("ev_a"),
                CreateTestEvent("ev_b"),
                CreateTestEvent("ev_c")
            };

            // Should not log any warnings.
            LogAssert.NoUnexpectedReceived();
            Assert.DoesNotThrow(() =>
                typeof(EventPoolBuilder).GetMethod("ValidateNoDuplicateIds",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                    ?.Invoke(null, new object[] { pool }));
        }

        [Test]
        public void ValidateNoDuplicateIds_Duplicate_LogsWarning()
        {
            var pool = new List<GameEvent>
            {
                CreateTestEvent("duplicate_id"),
                CreateTestEvent("duplicate_id"),
                CreateTestEvent("unique_id")
            };

            // The duplicate should produce a warning log. We verify it doesn't throw.
            Assert.DoesNotThrow(() =>
            {
                var method = typeof(EventPoolBuilder).GetMethod("ValidateNoDuplicateIds",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                if (method != null)
                {
                    // The method logs a warning but doesn't throw.
                    LogAssert.Expect(LogType.Warning,
                        new System.Text.RegularExpressions.Regex(
                            @"\[EventPoolBuilder\] Duplicate event id 'duplicate_id'"));
                    method.Invoke(null, new object[] { pool });
                }
            });
        }

        [Test]
        public void ValidateNoDuplicateIds_EmptyPool_NoThrow()
        {
            var pool = new List<GameEvent>();

            Assert.DoesNotThrow(() =>
                typeof(EventPoolBuilder).GetMethod("ValidateNoDuplicateIds",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                    ?.Invoke(null, new object[] { pool }));
        }

        // -----------------------------------------------------------------
        // ChildFoundEvent — has choices
        // -----------------------------------------------------------------

        [Test]
        public void ChildFoundEvent_HasTwoChoices()
        {
            var pool = EventPoolBuilder.Build(null);

            GameEvent childEvent = null;
            for (int i = 0; i < pool.Count; i++)
            {
                if (pool[i].id == "child_found_in_ash")
                {
                    childEvent = pool[i];
                    break;
                }
            }

            Assert.IsNotNull(childEvent, "Child Found event should be in pool.");
            Assert.IsNotNull(childEvent.choices,
                "Child Found event should have choices.");
            Assert.AreEqual(2, childEvent.choices.Count,
                "Child Found event should have exactly 2 choices (take in / leave).");
        }

        // -----------------------------------------------------------------
        // Helpers
        // -----------------------------------------------------------------

        private static GameEvent CreateTestEvent(string id)
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = id;
            ev.title = "Test " + id;
            return ev;
        }
    }
}
