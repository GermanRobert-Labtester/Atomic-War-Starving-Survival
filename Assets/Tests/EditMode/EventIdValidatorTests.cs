using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Events;
using AtomicWar._Game.Editor;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// H-3: EventIdValidator regression tests. These tests assert that the
    /// production event source tree has no duplicate ids, no empty ids,
    /// and conforms to the snake_case naming convention. If a future change
    /// introduces a collision or a malformed id, these tests will fail
    /// with a clear diagnostic listing the source of the problem.
    ///
    /// The tests are CI-friendly (run in <100ms in EditMode) and don't
    /// require any scene setup. They invoke the validator's public API
    /// and check the diagnostics.
    /// </summary>
    [TestFixture]
    public class EventIdValidatorTests
    {
        [Test]
        public void Validator_ProductionCode_NoDuplicates()
        {
            // The validator's main contract: across all production event
            // sources (EncounterEventFactory, GameBootstrap.Ensure*,
            // StreamingAssets catalog), there should be no duplicate ids.
            // If this fails, find the source in the diagnostic string and
            // rename the duplicate id.
            var diagnostics = EventIdValidator.Validate();
            foreach (var d in diagnostics)
            {
                Debug.LogWarning("[EventIdValidator] " + d);
            }
            Assert.AreEqual(0, diagnostics.Count,
                "EventIdValidator.Validate() returned diagnostics:\n" +
                string.Join("\n", diagnostics));
        }

        [Test]
        public void Validator_CatchesKnownDuplicatePattern_ManualSmoke()
        {
            // Sanity test: confirm the regex rejects malformed ids.
            // (We can't easily inject duplicates into the production
            // validator's collection without touching private state, so
            // this test focuses on the regex behavior.)
            Assert.IsTrue(EventIdValidator.SnakeCasePattern.IsMatch("the_emissary"));
            Assert.IsFalse(EventIdValidator.SnakeCasePattern.IsMatch("The_Emissary"));
        }

        [Test]
        public void Validator_NamingConvention_SnakeCasePattern_IsPublished()
        {
            // The pattern used by the validator is published so designers
            // can name new events correctly. The regex is ^[a-z][a-z0-9_]*$.
            string pattern = EventIdValidator.SnakeCasePattern.ToString();
            // Strip the ^ and $ anchors for the assertion (the regex
            // includes them but the assertion just needs the body).
            string body = pattern;
            if (body.StartsWith("^")) body = body.Substring(1);
            if (body.EndsWith("$")) body = body.Substring(0, body.Length - 1);
            Assert.AreEqual("[a-z][a-z0-9_]*", body);
            // Spot-check a few sample ids.
            Assert.IsTrue(EventIdValidator.SnakeCasePattern.IsMatch("the_emissary"));
            Assert.IsTrue(EventIdValidator.SnakeCasePattern.IsMatch("enc_child_sniper"));
            Assert.IsTrue(EventIdValidator.SnakeCasePattern.IsMatch("a"));
            Assert.IsTrue(EventIdValidator.SnakeCasePattern.IsMatch("a1_b2_c3"));
            // Negative cases.
            Assert.IsFalse(EventIdValidator.SnakeCasePattern.IsMatch("The_Emissary"));
            Assert.IsFalse(EventIdValidator.SnakeCasePattern.IsMatch("the-emissary"));
            Assert.IsFalse(EventIdValidator.SnakeCasePattern.IsMatch("1starts_with_digit"));
            Assert.IsFalse(EventIdValidator.SnakeCasePattern.IsMatch(""));
        }

        [Test]
        public void Validator_CollectAllEvents_ReturnsNonEmptyList()
        {
            // The production code has many events. The validator must
            // find at least the 45+ encounter events plus the emissary
            // chain plus the catalog entries.
            var all = EventIdValidator.CollectAllEvents();
            Assert.GreaterOrEqual(all.Count, 50,
                $"Validator should find at least 50 events; found {all.Count}. " +
                "This may indicate a reflection regression in CollectAllEvents.");
        }

        [Test]
        public void Validator_AllIdsAreNonEmpty()
        {
            // Cross-check: the validator's "all events" list must have
            // non-empty ids (the production code passes this, but the
            // validator itself is a single point of failure).
            var all = EventIdValidator.CollectAllEvents();
            for (int i = 0; i < all.Count; i++)
            {
                Assert.IsFalse(string.IsNullOrEmpty(all[i].id),
                    $"Event at index {i} has empty id (source: {all[i].source}).");
            }
        }

        [Test]
        public void Validator_AllIdsPassSnakeCaseConvention()
        {
            var all = EventIdValidator.CollectAllEvents();
            int bad = 0;
            for (int i = 0; i < all.Count; i++)
            {
                if (string.IsNullOrEmpty(all[i].id)) continue;
                if (!EventIdValidator.SnakeCasePattern.IsMatch(all[i].id))
                {
                    Debug.LogWarning($"[EventIdValidator] Bad naming: '{all[i].id}' (source {all[i].source})");
                    bad++;
                }
            }
            Assert.AreEqual(0, bad,
                $"{bad} event ids violate the snake_case convention. " +
                "See the warnings above for the offenders.");
        }
    }
}
