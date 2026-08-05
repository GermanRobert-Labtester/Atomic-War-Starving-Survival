using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Proc-gen node maps (#23): seed determinism, danger rings, weather pathing,
    /// silhouette fog-of-war, and MapScreenUI expedition pathing.
    /// Acceptance: seed 12345 twice → identical layout, loot tables, distances.
    /// </summary>
    [TestFixture]
    public class MapGeneratorTests
    {
        private const int AcceptanceSeed = 12345;
        private const float Eps = 1e-4f;

        [TearDown]
        public void TearDown()
        {
            // ExpeditionSystem subscribes to the static EventBus on construction
            // and is only unsubscribed by UnsubscribeAll(), which tests cannot
            // call for instances local to a test method. Clear between tests so
            // handlers bound to dead systems don't fire on later fixtures.
            EventBus.Clear();
        }

        [Test]
        public void Seed_12345_Twice_ProducesIdenticalLayout_LootTables_AndDistances()
        {
            var a = MapGenerator.Generate(AcceptanceSeed);
            var b = MapGenerator.Generate(AcceptanceSeed);

            Assert.That(a.ComputeLayoutFingerprint(), Is.EqualTo(b.ComputeLayoutFingerprint()),
                "Same seed must produce identical layout fingerprint");

            Assert.That(a.Nodes.Count, Is.EqualTo(b.Nodes.Count));
            Assert.That(a.Paths.Count, Is.EqualTo(b.Paths.Count));

            for (int i = 0; i < a.Nodes.Count; i++)
            {
                var na = a.Nodes[i];
                var nb = b.GetNode(na.NodeId);
                Assert.That(nb, Is.Not.Null, $"Missing node {na.NodeId} in second map");
                Assert.That(nb.LootTableId, Is.EqualTo(na.LootTableId), na.NodeId);
                Assert.That(nb.DistanceFromShelter, Is.EqualTo(na.DistanceFromShelter).Within(Eps), na.NodeId);
                Assert.That(nb.TrueRad, Is.EqualTo(na.TrueRad).Within(Eps), na.NodeId);
                Assert.That(nb.RumoredRad, Is.EqualTo(na.RumoredRad).Within(Eps), na.NodeId);
                Assert.That(nb.RadZoneProfileId, Is.EqualTo(na.RadZoneProfileId), na.NodeId);
                Assert.That(nb.DisplayName, Is.EqualTo(na.DisplayName), na.NodeId);
                Assert.That(nb.Ring, Is.EqualTo(na.Ring), na.NodeId);
                Assert.That(nb.EncounterDeckIds.Count, Is.EqualTo(na.EncounterDeckIds.Count), na.NodeId);
                for (int e = 0; e < na.EncounterDeckIds.Count; e++)
                    Assert.That(nb.EncounterDeckIds[e], Is.EqualTo(na.EncounterDeckIds[e]));
            }

            // Edges
            for (int i = 0; i < a.Paths.Count; i++)
            {
                var pa = a.Paths[i];
                var pb = b.GetPath(pa.FromNodeId, pa.ToNodeId);
                Assert.That(pb, Is.Not.Null, $"Missing path {pa.FromNodeId}-{pa.ToNodeId}");
                Assert.That(pb.BaseTravelHours, Is.EqualTo(pa.BaseTravelHours).Within(Eps));
            }
        }

        [Test]
        public void DifferentSeeds_ProduceDifferentLayouts()
        {
            var a = MapGenerator.Generate(12345);
            var b = MapGenerator.Generate(99999);
            Assert.That(a.ComputeLayoutFingerprint(), Is.Not.EqualTo(b.ComputeLayoutFingerprint()));
        }

        [Test]
        public void Generate_CreatesThreeDangerRings_AndShelterHub()
        {
            var map = MapGenerator.Generate(AcceptanceSeed);

            Assert.That(map.ShelterNode, Is.Not.Null);
            Assert.That(map.ShelterNode.IsShelter, Is.True);
            Assert.That(map.ShelterNode.IsRevealed, Is.True);

            int suburbs = 0, outskirts = 0, gz = 0;
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                switch (map.Nodes[i].Ring)
                {
                    case DangerRing.Suburbs: suburbs++; break;
                    case DangerRing.CityOutskirts: outskirts++; break;
                    case DangerRing.GroundZero: gz++; break;
                }
            }

            Assert.That(suburbs, Is.EqualTo(MapGenerator.SuburbsNodeCount));
            Assert.That(outskirts, Is.EqualTo(MapGenerator.OutskirtsNodeCount));
            Assert.That(gz, Is.EqualTo(MapGenerator.GroundZeroNodeCount));

            // Loot / rad zone ids by ring
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                var n = map.Nodes[i];
                if (n.IsShelter) continue;
                switch (n.Ring)
                {
                    case DangerRing.Suburbs:
                        Assert.That(n.LootTableId, Is.EqualTo(MapGenerator.LootSuburbs));
                        Assert.That(n.RadZoneProfileId, Is.EqualTo(MapGenerator.RadSuburbs));
                        Assert.That(n.TrueRad, Is.LessThan(40f));
                        break;
                    case DangerRing.CityOutskirts:
                        Assert.That(n.LootTableId, Is.EqualTo(MapGenerator.LootOutskirts));
                        Assert.That(n.RadZoneProfileId, Is.EqualTo(MapGenerator.RadOutskirts));
                        break;
                    case DangerRing.GroundZero:
                        Assert.That(n.LootTableId, Is.EqualTo(MapGenerator.LootGroundZero));
                        Assert.That(n.RadZoneProfileId, Is.EqualTo(MapGenerator.RadGroundZero));
                        Assert.That(n.TrueRad, Is.GreaterThan(100f));
                        break;
                }
            }
        }

        [Test]
        public void AllNodes_ReachableFromShelter_ViaPaths()
        {
            var map = MapGenerator.Generate(AcceptanceSeed);
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                var n = map.Nodes[i];
                if (n.IsShelter) continue;
                var path = map.FindPath(GeneratedMap.ShelterNodeId, n.NodeId);
                Assert.That(path, Is.Not.Null, $"No path to {n.NodeId}");
                Assert.That(path.Count, Is.GreaterThanOrEqualTo(2));
                Assert.That(n.DistanceFromShelter, Is.GreaterThan(0f));
            }
        }

        [Test]
        public void Blizzard_DoublesTravelTime()
        {
            var map = MapGenerator.Generate(AcceptanceSeed);
            MapNode target = null;
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                if (map.Nodes[i].Ring == DangerRing.Suburbs)
                {
                    target = map.Nodes[i];
                    break;
                }
            }
            Assert.That(target, Is.Not.Null);

            float clear = map.GetTravelHoursFromShelter(target.NodeId, WeatherKind.Clear);
            float blizzard = map.GetTravelHoursFromShelter(target.NodeId, WeatherKind.Blizzard);

            Assert.That(clear, Is.GreaterThan(0f));
            Assert.That(blizzard, Is.EqualTo(clear * 2f).Within(Eps));
            Assert.That(GeneratedMap.WeatherTravelMultiplier(WeatherKind.Blizzard), Is.EqualTo(2f));
        }

        [Test]
        public void UnsurveyedNode_ShowsSilhouetteAndRumoredRadOnly()
        {
            var map = MapGenerator.Generate(AcceptanceSeed);
            MapNode target = null;
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                if (!map.Nodes[i].IsShelter)
                {
                    target = map.Nodes[i];
                    break;
                }
            }

            Assert.That(target.IsRevealed, Is.False);
            var view = map.GetPlayerView(target.NodeId);
            Assert.That(view.IsSilhouette, Is.True);
            Assert.That(view.Label, Does.Contain("Silhouette"));
            Assert.That(view.DisplayedRad, Is.EqualTo(target.RumoredRad).Within(Eps));
            Assert.That(view.LootTableId, Is.Empty);

            map.RevealNode(target.NodeId);
            view = map.GetPlayerView(target.NodeId);
            Assert.That(view.IsSilhouette, Is.False);
            Assert.That(view.Label, Is.EqualTo(target.DisplayName));
            Assert.That(view.LootTableId, Is.EqualTo(target.LootTableId));
        }

        [Test]
        public void MarkVisited_RevealsNode()
        {
            var map = MapGenerator.Generate(7);
            string id = null;
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                if (!map.Nodes[i].IsShelter) { id = map.Nodes[i].NodeId; break; }
            }
            map.MarkVisited(id);
            var n = map.GetNode(id);
            Assert.That(n.IsVisited, Is.True);
            Assert.That(n.IsRevealed, Is.True);
        }

        [Test]
        public void MapScreenUI_Pathing_UsesWeatherMultiplier()
        {
            var map = MapGenerator.Generate(AcceptanceSeed);
            WeatherKind weather = WeatherKind.Clear;
            var go = new GameObject("MapScreenTest");
            try
            {
                var ui = go.AddComponent<MapScreenUI>();
                ui.Bind(map, () => weather);

                string nodeId = null;
                for (int i = 0; i < map.Nodes.Count; i++)
                {
                    if (map.Nodes[i].Ring == DangerRing.Suburbs)
                    {
                        nodeId = map.Nodes[i].NodeId;
                        break;
                    }
                }

                Assert.That(ui.SelectNode(nodeId), Is.True);
                float clearHours = ui.GetSelectedTravelHours();
                Assert.That(clearHours, Is.GreaterThan(0f));
                Assert.That(ui.SelectedPath.Count, Is.GreaterThanOrEqualTo(2));
                Assert.That(ui.SelectedPath[0], Is.EqualTo(GeneratedMap.ShelterNodeId));

                weather = WeatherKind.Blizzard;
                ui.SelectNode(nodeId); // rebuild path under blizzard
                Assert.That(ui.GetSelectedTravelHours(), Is.EqualTo(clearHours * 2f).Within(Eps));

                // Silhouette until revealed
                Assert.That(ui.DetailSummary, Does.Contain("silhouette").IgnoreCase
                    .Or.Contain("Silhouette").Or.Contain("unsurveyed").IgnoreCase
                    .Or.Contain("Intel"));
            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }

        [Test]
        public void ExpeditionSystem_StartFromMapNode_UsesWeatherScaledDistance_AndMarksVisited()
        {
            var map = MapGenerator.Generate(AcceptanceSeed);
            MapNode target = null;
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                if (map.Nodes[i].Ring == DangerRing.Suburbs)
                {
                    target = map.Nodes[i];
                    break;
                }
            }

            var weatherProfile = ScriptableObject.CreateInstance<SeasonProfile>();
            weatherProfile.campaignLengthDays = 90;
            weatherProfile.weatherCheckIntervalHours = 24f;
            weatherProfile.seasons = new[]
            {
                new SeasonWindow
                {
                    id = "test", displayName = "Test", startDay = 0,
                    clearWeight = 0f, blizzardWeight = 1f
                }
            };
            var weather = new WeatherSystem(weatherProfile, seed: 1);
            weather.ForceWeather(WeatherKind.Blizzard);

            var needsProfile = ScriptableObject.CreateInstance<NeedsProfile>();
            needsProfile.hungerPerHour = 1f;
            needsProfile.thirstPerHour = 1f;
            needsProfile.fatiguePerHour = 1f;
            var needs = new NeedsSystem(needsProfile, sv => true);
            var survivor = new Survivor { Id = "sv_map", DisplayName = "Scout" };
            survivor.Needs.Health = 100f;
            needs.Register(survivor);
            var inv = new Inventory { Capacity = 50, MaxWeight = 200f };
            var rad = new RadiationSystem(needs);
            rad.Register(survivor);

            // Encounters disabled: a random encounter can resolve to "flee",
            // which flips the expedition to Inbound mid-travel so it never
            // arrives. This test is about arrival marking a node visited, so
            // arrival must be guaranteed rather than left to the RNG stream.
            var exp = new ExpeditionSystem(rad, inv, null, new ExpeditionSystem.Config
            {
                WeatherSystem = weather,
                Seed = 1,
                CreateDefaultEncounters = false
            });
            exp.SetGeneratedMap(map);

            float expectedHours = map.GetTravelHoursFromShelter(target.NodeId, WeatherKind.Blizzard);
            Assert.That(exp.StartExpedition(survivor, target), Is.True);
            Assert.That(exp.ActiveExpeditions.Count, Is.EqualTo(1));
            var state = exp.ActiveExpeditions[0];
            Assert.That(state.TotalDistanceTicks, Is.EqualTo(Mathf.Max(1, Mathf.RoundToInt(expectedHours))));
            Assert.That(state.TargetLocationId, Is.EqualTo(target.NodeId));

            // Tick until looting (arrival marks visited)
            int safety = 200;
            while (state.Phase == ExpeditionPhase.Outbound && safety-- > 0)
                exp.Tick(1f);

            // Assert arrival explicitly. Phase alone is not sufficient evidence:
            // an aborted trip also leaves the expedition in Inbound, so a phase-only
            // check cannot distinguish "arrived, then turned back" from
            // "fled before ever arriving".
            Assert.That(state.TravelTicksCompleted, Is.GreaterThanOrEqualTo(state.TotalDistanceTicks),
                "Expedition must actually cover the full outbound distance");
            Assert.That(state.Phase, Is.EqualTo(ExpeditionPhase.Looting),
                "Reaching the node begins the Looting phase");
            Assert.That(map.GetNode(target.NodeId).IsVisited, Is.True,
                "Arriving at a node must mark it visited/revealed");

            Object.DestroyImmediate(weatherProfile);
            Object.DestroyImmediate(needsProfile);
        }

        [Test]
        public void CaptureRestore_PreservesRevealFlags_AcrossRegenerate()
        {
            var map = MapGenerator.Generate(AcceptanceSeed);
            string id = null;
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                if (!map.Nodes[i].IsShelter) { id = map.Nodes[i].NodeId; break; }
            }
            map.MarkVisited(id);
            map.SetRumoredRad(id, 77f);
            var save = map.CaptureState();

            var reloaded = MapGenerator.Generate(AcceptanceSeed);
            Assert.That(reloaded.GetNode(id).IsVisited, Is.False);
            reloaded.RestoreRevealState(save);
            Assert.That(reloaded.GetNode(id).IsVisited, Is.True);
            Assert.That(reloaded.GetNode(id).IsRevealed, Is.True);
            Assert.That(reloaded.GetNode(id).RumoredRad, Is.EqualTo(77f).Within(Eps));
            // Layout structure (ids, loot, base distances) still matches a fresh generate
            var fresh = MapGenerator.Generate(AcceptanceSeed);
            Assert.That(reloaded.Nodes.Count, Is.EqualTo(fresh.Nodes.Count));
            Assert.That(reloaded.Paths.Count, Is.EqualTo(fresh.Paths.Count));
            for (int i = 0; i < fresh.Nodes.Count; i++)
            {
                var f = fresh.Nodes[i];
                var r = reloaded.GetNode(f.NodeId);
                Assert.That(r.LootTableId, Is.EqualTo(f.LootTableId));
                Assert.That(r.DistanceFromShelter, Is.EqualTo(f.DistanceFromShelter).Within(Eps));
            }
        }

        [Test]
        public void RadioPlume_SetsRumoredRad_AndCanRevealNode()
        {
            var map = MapGenerator.Generate(AcceptanceSeed);
            string id = null;
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                if (!map.Nodes[i].IsShelter) { id = map.Nodes[i].NodeId; break; }
            }

            var knowledge = new RadiationKnowledgeMap();
            knowledge.SeedTile(id, map.GetNode(id).TrueRad, map.GetNode(id).RumoredRad, 1f);

            var radio = new RadioTunerSystem(new System.Random(1));
            var intel = AtomicWar._Game.Data.IntelNode.CreatePlumeReport(
                id, rumoredRad: 88f, confidence: 0.8f,
                extractedDay: 1, expirationDay: 10, text: "Plume over site");

            Assert.That(radio.ApplyPlumeReportToMap(intel, knowledge, map), Is.True);
            Assert.That(map.GetNode(id).RumoredRad, Is.EqualTo(88f).Within(Eps));
            Assert.That(map.GetNode(id).IsRevealed, Is.True, "High-confidence radio intel reveals node");
            Assert.That(knowledge.GetTile(id).RumoredRad, Is.EqualTo(88f).Within(Eps));
        }
    }
}
