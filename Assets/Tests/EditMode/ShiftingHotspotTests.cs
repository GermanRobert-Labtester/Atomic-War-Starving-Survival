using System.Collections.Generic;
using NUnit.Framework;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using Random = System.Random;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompt #14 — Shifting Hotspots: death zones, two-hop windstorm migration,
    /// knowledge invalidation, deterministic seed, save/load replay.
    /// </summary>
    [TestFixture]
    public class ShiftingHotspotTests
    {
        private const float Eps = 1e-3f;

        [Test]
        public void MapGenerator_TagsDeathZones_Deterministically_ShelterNever()
        {
            var a = MapGenerator.Generate(12345);
            var b = MapGenerator.Generate(12345);

            int deathCount = 0;
            for (int i = 0; i < a.Nodes.Count; i++)
            {
                var na = a.Nodes[i];
                var nb = b.GetNode(na.NodeId);
                Assert.That(nb.IsDeathZone, Is.EqualTo(na.IsDeathZone), na.NodeId);
                if (na.IsShelter)
                    Assert.That(na.IsDeathZone, Is.False, "Shelter must never be a death zone");
                else if (na.IsDeathZone)
                {
                    deathCount++;
                    Assert.That(na.TrueRad, Is.GreaterThanOrEqualTo(MapGenerator.DeathZoneRadThreshold));
                }
                else
                {
                    Assert.That(na.TrueRad, Is.LessThan(MapGenerator.DeathZoneRadThreshold));
                }
            }
            Assert.That(deathCount, Is.GreaterThan(0),
                "Seed should produce at least one high-rad death zone (Ground Zero)");
        }

        [Test]
        public void SameSeed_IsDeathZone_InLayoutFingerprint()
        {
            var a = MapGenerator.Generate(42);
            var b = MapGenerator.Generate(42);
            Assert.That(a.ComputeLayoutFingerprint(), Is.EqualTo(b.ComputeLayoutFingerprint()));
        }

        [Test]
        public void CollectNodesAtHopDistance_ReturnsExactlyTwoHops_ExcludesShelter()
        {
            var map = MapGenerator.Generate(7);
            MapNode death = FindFirstDeathZone(map);
            Assert.That(death, Is.Not.Null);

            var sys = new ShiftingHotspotSystem(new Random(1));
            sys.Bind(map);

            var hops2 = sys.CollectNodesAtHopDistance(death.NodeId, 2);
            Assert.That(hops2.Count, Is.GreaterThan(0),
                "Death zone should have at least one node two hops away");

            // FindPath is hour-weighted (can prefer more hops); hop distance is BFS.
            // Re-query hop-1 set and assert none of hops2 appear there (strict distance 2).
            var hops1 = sys.CollectNodesAtHopDistance(death.NodeId, 1);
            var hop1Ids = new HashSet<string>();
            for (int i = 0; i < hops1.Count; i++)
                hop1Ids.Add(hops1[i].NodeId);

            for (int i = 0; i < hops2.Count; i++)
            {
                Assert.That(hops2[i].IsShelter, Is.False);
                Assert.That(hop1Ids.Contains(hops2[i].NodeId), Is.False,
                    $"{hops2[i].NodeId} must not also be a 1-hop neighbor");
            }

            // Every 2-hop node must have a neighbor that is 1 hop from origin
            for (int i = 0; i < hops2.Count; i++)
            {
                bool linkedViaOneHop = false;
                for (int j = 0; j < hops1.Count; j++)
                {
                    if (map.GetPath(hops1[j].NodeId, hops2[i].NodeId) != null)
                    {
                        linkedViaOneHop = true;
                        break;
                    }
                }
                Assert.That(linkedViaOneHop, Is.True,
                    $"{hops2[i].NodeId} must connect through a 1-hop neighbor of {death.NodeId}");
            }
        }

        [Test]
        public void TryShift_MovesLethalRad_OldNodeNoLongerDeathZone()
        {
            var map = MapGenerator.Generate(99);
            MapNode death = FindFirstDeathZone(map);
            Assert.That(death, Is.Not.Null);
            float lethal = death.TrueRad;
            string fromId = death.NodeId;

            var knowledge = new RadiationKnowledgeMap();
            SeedKnowledge(map, knowledge);
            knowledge.RecordSurvey(fromId, lethal, 1f, day: 10);
            Assert.That(knowledge.GetTile(fromId).Surveyed, Is.True);

            var sys = new ShiftingHotspotSystem(new AlwaysShiftRng());
            sys.Bind(map, knowledge);

            var result = sys.TryShift(day: 35, preferFromId: fromId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.FromNodeId, Is.EqualTo(fromId));
            Assert.That(result.ToNodeId, Is.Not.EqualTo(fromId));
            Assert.That(result.MovedRad, Is.EqualTo(lethal).Within(Eps));

            var oldNode = map.GetNode(fromId);
            var newNode = map.GetNode(result.ToNodeId);
            Assert.That(oldNode.IsDeathZone, Is.False, "Previous death zone cools off");
            Assert.That(oldNode.TrueRad, Is.EqualTo(ShiftingHotspotSystem.ResidualRadAfterShift).Within(Eps));
            Assert.That(oldNode.TrueRad, Is.LessThan(MapGenerator.DeathZoneRadThreshold));
            Assert.That(newNode.TrueRad, Is.EqualTo(lethal).Within(Eps));
            Assert.That(newNode.IsDeathZone, Is.True);

            // Destination must be in the 2-hop BFS set (hour-weighted FindPath may differ)
            var hops2 = sys.CollectNodesAtHopDistance(fromId, 2);
            bool inHop2 = false;
            for (int i = 0; i < hops2.Count; i++)
            {
                if (hops2[i].NodeId == result.ToNodeId) { inHop2 = true; break; }
            }
            Assert.That(inHop2, Is.True, "Shift target must be exactly two path-hops away");

            // Knowledge invalidated on both tiles
            Assert.That(knowledge.GetTile(fromId).Surveyed, Is.False);
            Assert.That(knowledge.GetTile(result.ToNodeId).Surveyed, Is.False);
            Assert.That(knowledge.GetTrueRad(fromId),
                Is.EqualTo(ShiftingHotspotSystem.ResidualRadAfterShift).Within(Eps));
            Assert.That(knowledge.GetTrueRad(result.ToNodeId), Is.EqualTo(lethal).Within(Eps));
        }

        [Test]
        public void TickDay_BeforeDay30_NeverShifts()
        {
            var map = MapGenerator.Generate(11);
            var sys = new ShiftingHotspotSystem(new AlwaysShiftRng());
            sys.Bind(map);

            for (int day = 1; day < ShiftingHotspotSystem.MinDayForShift; day++)
                Assert.That(sys.TickDay(day), Is.False, $"Day {day} must not shift");

            Assert.That(sys.ShiftCount, Is.EqualTo(0));
        }

        [Test]
        public void TickDay_AfterDay30_WithAlwaysRng_CanShift()
        {
            var map = MapGenerator.Generate(11);
            var sys = new ShiftingHotspotSystem(new AlwaysShiftRng());
            sys.Bind(map);

            bool shifted = sys.TickDay(ShiftingHotspotSystem.MinDayForShift);
            Assert.That(shifted, Is.True);
            Assert.That(sys.ShiftCount, Is.EqualTo(1));
            Assert.That(sys.LastShiftDay, Is.EqualTo(ShiftingHotspotSystem.MinDayForShift));
        }

        [Test]
        public void TickDay_RespectsMinDaysBetweenShifts()
        {
            var map = MapGenerator.Generate(11);
            var sys = new ShiftingHotspotSystem(new AlwaysShiftRng());
            sys.Bind(map);

            Assert.That(sys.TickDay(30), Is.True);
            Assert.That(sys.TickDay(31), Is.False, "Too soon after previous shift");
            Assert.That(sys.TickDay(30 + ShiftingHotspotSystem.MinDaysBetweenShifts - 1), Is.False);
            Assert.That(sys.TickDay(30 + ShiftingHotspotSystem.MinDaysBetweenShifts), Is.True);
        }

        [Test]
        public void CaptureRestore_ReplaysHistory_Idempotent()
        {
            var map = MapGenerator.Generate(55);
            var knowledge = new RadiationKnowledgeMap();
            SeedKnowledge(map, knowledge);

            var sys = new ShiftingHotspotSystem(new AlwaysShiftRng());
            sys.Bind(map, knowledge);
            var r1 = sys.TryShift(40);
            Assert.That(r1, Is.Not.Null);
            // Allow second shift after cooldown window via force
            sys.TryShift(50);

            var save = sys.CaptureState();
            Assert.That(save.History.Count, Is.GreaterThanOrEqualTo(1));

            // Snapshot rads after shifts
            var radSnapshot = new Dictionary<string, float>();
            var deathSnapshot = new Dictionary<string, bool>();
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                var n = map.Nodes[i];
                if (n == null) continue;
                radSnapshot[n.NodeId] = n.TrueRad;
                deathSnapshot[n.NodeId] = n.IsDeathZone;
            }

            // Fresh map + restore
            var map2 = MapGenerator.Generate(55);
            var knowledge2 = new RadiationKnowledgeMap();
            SeedKnowledge(map2, knowledge2);
            var sys2 = new ShiftingHotspotSystem(new Random(999));
            sys2.Bind(map2, knowledge2);
            sys2.RestoreState(save);

            Assert.That(sys2.ShiftCount, Is.EqualTo(save.ShiftCount));
            Assert.That(sys2.History.Count, Is.EqualTo(save.History.Count));

            for (int i = 0; i < map2.Nodes.Count; i++)
            {
                var n = map2.Nodes[i];
                if (n == null) continue;
                Assert.That(n.TrueRad, Is.EqualTo(radSnapshot[n.NodeId]).Within(Eps), n.NodeId);
                Assert.That(n.IsDeathZone, Is.EqualTo(deathSnapshot[n.NodeId]), n.NodeId);
            }

            // Second restore must not double-apply
            sys2.RestoreState(save);
            for (int i = 0; i < map2.Nodes.Count; i++)
            {
                var n = map2.Nodes[i];
                if (n == null) continue;
                Assert.That(n.TrueRad, Is.EqualTo(radSnapshot[n.NodeId]).Within(Eps), n.NodeId);
            }
        }

        [Test]
        public void InvalidateKnowledge_ClearsSurvey_MaxUncertainty()
        {
            var map = new RadiationKnowledgeMap();
            map.SeedTile("loc_a", 100f, 50f, 0.2f);
            map.RecordSurvey("loc_a", 100f, 1f, 5);
            Assert.That(map.GetTile("loc_a").Surveyed, Is.True);

            Assert.That(map.InvalidateKnowledge("loc_a", rumoredRad: 40f), Is.True);
            var tile = map.GetTile("loc_a");
            Assert.That(tile.Surveyed, Is.False);
            Assert.That(tile.MeasuredAtDay, Is.EqualTo(-1));
            Assert.That(tile.RumorUncertainty, Is.EqualTo(RadiationKnowledgeMap.MaxUncertainty));
            Assert.That(tile.RumoredRad, Is.EqualTo(40f).Within(Eps));
        }

        [Test]
        public void DeterministicShift_SameSeedSameSequence()
        {
            HotspotShiftResult Run(int mapSeed, int rngSeed)
            {
                var map = MapGenerator.Generate(mapSeed);
                var sys = new ShiftingHotspotSystem(new Random(rngSeed));
                sys.Bind(map);
                return sys.TryShift(35);
            }

            var a = Run(77, 3);
            var b = Run(77, 3);
            Assert.That(a, Is.Not.Null);
            Assert.That(b, Is.Not.Null);
            Assert.That(a.FromNodeId, Is.EqualTo(b.FromNodeId));
            Assert.That(a.ToNodeId, Is.EqualTo(b.ToNodeId));
            Assert.That(a.MovedRad, Is.EqualTo(b.MovedRad).Within(Eps));
        }

        // -----------------------------------------------------------------

        private sealed class AlwaysShiftRng : Random
        {
            public override double NextDouble() => 0.0;
            public override int Next(int maxValue) => 0;
        }

        private static MapNode FindFirstDeathZone(GeneratedMap map)
        {
            if (map?.Nodes == null) return null;
            var list = new List<MapNode>();
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                if (map.Nodes[i] != null && map.Nodes[i].IsDeathZone)
                    list.Add(map.Nodes[i]);
            }
            list.Sort((a, b) => string.CompareOrdinal(a.NodeId, b.NodeId));
            return list.Count > 0 ? list[0] : null;
        }

        private static void SeedKnowledge(GeneratedMap map, RadiationKnowledgeMap knowledge)
        {
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                var n = map.Nodes[i];
                if (n == null || n.IsShelter) continue;
                knowledge.SeedTile(n.NodeId, n.TrueRad, n.RumoredRad, 1f);
            }
        }
    }
}
