using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompt #839 — bunker gossip: crime witness, daily spread, affinity decay, save slot.
    /// </summary>
    [TestFixture]
    public class GossipSystemTests
    {
        private const float Eps = 1e-4f;

        private static System_Gossip MakeWithRoster(params string[] ids)
        {
            var g = new System_Gossip();
            g.SetSurvivorRoster(new List<string>(ids));
            return g;
        }

        [Test]
        public void WitnessCrime_CreatesRumor_FiresEvent_SkipsSelf()
        {
            var g = MakeWithRoster("w", "c", "a");
            string gotW = null, gotC = null, gotCrime = null;
            g.OnCrimeWitnessed += (w, c, crime) =>
            {
                gotW = w;
                gotC = c;
                gotCrime = crime;
            };

            g.WitnessCrime("w", "c", "theft", 3);
            Assert.AreEqual(1, g.RumorCount);
            Assert.AreEqual("w", gotW);
            Assert.AreEqual("c", gotC);
            Assert.AreEqual("theft", gotCrime);
            Assert.IsTrue(g.HasHeardRumor("w", "c"));
            Assert.IsFalse(g.HasHeardRumor("a", "c"));

            // Duplicate same triple is ignored.
            g.WitnessCrime("w", "c", "theft", 4);
            Assert.AreEqual(1, g.RumorCount);

            // Self-witness is ignored.
            g.WitnessCrime("c", "c", "theft", 5);
            Assert.AreEqual(1, g.RumorCount);
        }

        [Test]
        public void TickDay_SpreadsToAtMostTwo_AndDecaysAffinity()
        {
            // witness + criminal + 4 uninformed = 6 roster slots
            var g = MakeWithRoster("w", "c", "a", "b", "d", "e");
            g.WitnessCrime("w", "c", "murder", 1);

            int spreads = 0;
            var decayHits = new List<(string criminal, string target, float amount)>();
            g.OnRumorSpread += (from, to, criminal) =>
            {
                spreads++;
                Assert.AreEqual("c", criminal);
            };
            g.OnAffinityDecayed += (criminal, target, amount) =>
                decayHits.Add((criminal, target, amount));

            g.TickDay();

            Assert.AreEqual(2, spreads, "MaxTellsPerDay = 2");
            Assert.AreEqual(3, g.GetInformedCount("c"), "witness + 2 new");
            // Decay = 0.02 * informedCount (3 after spread)
            float expected = System_Gossip.AffinityDecayPerInformed * 3f;
            Assert.AreEqual(expected, g.GetAffinityDecay("c"), Eps);
            // Criminal is not a decay target; every other roster member is.
            Assert.AreEqual(5, decayHits.Count);
            foreach (var hit in decayHits)
            {
                Assert.AreEqual("c", hit.criminal);
                Assert.AreNotEqual("c", hit.target);
                Assert.AreEqual(expected, hit.amount, Eps);
            }
        }

        [Test]
        public void TickDay_SecondDay_ContinuesSpread_AccumulatesDecay()
        {
            var g = MakeWithRoster("w", "c", "a", "b", "d");
            g.WitnessCrime("w", "c", "sabotage", 1);
            g.TickDay(); // informed: w + 2
            float after1 = g.GetAffinityDecay("c");
            Assert.Greater(after1, 0f);

            g.TickDay(); // remaining uninformed (minus criminal) get filled
            float after2 = g.GetAffinityDecay("c");
            Assert.Greater(after2, after1);
            // Everyone except criminal should know eventually (w,a,b,d = 4)
            Assert.AreEqual(4, g.GetInformedCount("c"));
        }

        [Test]
        public void SpreadRumor_TellsOneListener()
        {
            var g = MakeWithRoster("w", "c", "a", "b");
            g.WitnessCrime("w", "c", "theft", 1);
            string toId = null;
            g.OnRumorSpread += (from, to, criminal) => toId = to;

            g.SpreadRumor("w");
            Assert.IsNotNull(toId);
            Assert.IsTrue(g.HasHeardRumor(toId, "c"));
            Assert.AreEqual(2, g.GetInformedCount("c"));
        }

        [Test]
        public void AffinityDecayEvent_MatchesBootstrapWiringPattern()
        {
            // Mirror GameBootstrap.WireGossipSystem: OnAffinityDecayed → Affinity.Adjust(-amount)
            var affinity = new InterpersonalAffinity();
            affinity.Set("c", "a", 50f);

            var g = MakeWithRoster("w", "c", "a");
            g.OnAffinityDecayed += (criminalId, targetId, amount) =>
            {
                if (amount <= 0f) return;
                affinity.Adjust(criminalId, targetId, -amount);
            };

            g.WitnessCrime("w", "c", "theft", 1);
            g.TickDay(); // informed = 1 (only w; a may get told)

            float decay = g.GetAffinityDecay("c");
            Assert.Greater(decay, 0f);
            // After one tick, affinity(c,a) should be below the starting 50.
            Assert.Less(affinity.Get("c", "a"), 50f);
        }

        [Test]
        public void CaptureRestore_DeepCopy_PreservesRumorsAndDecay()
        {
            var a = MakeWithRoster("w", "c", "a", "b");
            a.WitnessCrime("w", "c", "theft", 2);
            a.TickDay();
            float decayAtCapture = a.GetAffinityDecay("c");
            Assert.Greater(decayAtCapture, 0f);

            var save = a.CaptureState();
            Assert.AreEqual("system_gossip", save.system_id);
            Assert.AreEqual(1, save.rumors.Count);
            Assert.Greater(save.affinity_decay_ids.Count, 0);

            // Mutating original after capture must not touch the snapshot.
            a.WitnessCrime("a", "c", "arson", 3);
            Assert.AreEqual(2, a.RumorCount);
            Assert.AreEqual(1, save.rumors.Count);

            var b = new System_Gossip();
            b.RestoreState(save);
            Assert.AreEqual(1, b.RumorCount);
            Assert.IsTrue(b.HasHeardRumor("w", "c"));
            Assert.AreEqual(decayAtCapture, b.GetAffinityDecay("c"), Eps);
            // Restored system must not include post-capture arson rumor.
            Assert.IsFalse(b.RumorCount == 2);
        }

        [Test]
        public void RestoreNull_Resets()
        {
            var g = MakeWithRoster("w", "c");
            g.WitnessCrime("w", "c", "theft", 1);
            g.TickDay();
            g.RestoreState(null);
            Assert.AreEqual(0, g.RumorCount);
            Assert.AreEqual(0f, g.GetAffinityDecay("c"), Eps);
        }

        [Test]
        public void SaveSystemAdapter_GossipSlot_RoundTrip()
        {
            string dir = SaveSystemTestFactory.TempDir("gossip");
            try
            {
                var gossipA = MakeWithRoster("w", "c", "a", "b");
                gossipA.WitnessCrime("w", "c", "theft", 5);
                gossipA.TickDay();
                float decayA = gossipA.GetAffinityDecay("c");
                Assert.Greater(decayA, 0f);

                SaveSystem Make(System_Gossip gossip) =>
                    SaveSystemTestFactory.MakeSave(dir, ss => { ss.SetGossipSystem(gossip); });

                Assert.IsTrue(Make(gossipA).Save("gossip_slot"));

                var gossipB = new System_Gossip();
                Assert.IsTrue(Make(gossipB).Load("gossip_slot"));

                Assert.AreEqual(1, gossipB.RumorCount);
                Assert.IsTrue(gossipB.HasHeardRumor("w", "c"));
                Assert.AreEqual(decayA, gossipB.GetAffinityDecay("c"), Eps);
                Assert.AreEqual(gossipA.GetInformedCount("c"), gossipB.GetInformedCount("c"));
            }
            finally
            {
                try { if (Directory.Exists(dir)) Directory.Delete(dir, true); } catch { /* best-effort */ }
            }
        }
    }
}
