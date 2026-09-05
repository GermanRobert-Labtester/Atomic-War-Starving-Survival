using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Narrative;
using Ashfall.Core.Random;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Task F14 / Section 21: 100-expedition deterministic simulation tests.
    /// Proves route-affinity filtering, replay determinism, eligibility bounds,
    /// and selection diversity across multi-destination expedition sorties.
    /// </summary>
    public class MicroLocationSimulationTests
    {
        private static string DataDir()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent!;
            return Path.Combine(dir!.FullName, "Assets", "StreamingAssets", "Data");
        }

        private static NarrativeEncounterSystem CreateSystem()
        {
            var sys = new NarrativeEncounterSystem();
            string dataDir = DataDir();
            var defs = NarrativeEncounterCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            foreach (var d in defs)
            {
                if (d.id.StartsWith("micro_", StringComparison.Ordinal))
                    sys.RegisterEncounter(d);
            }
            return sys;
        }

        private static List<ExpeditionDefinition> LoadExpeditions()
        {
            string dataDir = DataDir();
            return ExpeditionCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
        }

        [Fact]
        public void Simulation_100Expeditions_DeterministicAndAffinityCompliant()
        {
            var expeditions = LoadExpeditions();
            Assert.True(expeditions.Count >= 20, "Expected at least 20 authored expeditions");

            List<string> RunSimulation(int masterSeed)
            {
                var sys = CreateSystem();
                var results = new List<string>();
                var rng = new SeededRng(masterSeed);

                for (int sortie = 0; sortie < 100; sortie++)
                {
                    // Rotate expeditions deterministically
                    var exp = expeditions[sortie % expeditions.Count];
                    int legSeed = masterSeed + sortie * 37;
                    var legRng = new SeededRng(legSeed);

                    var picked = sys.SelectEncounter("Normal", exp.dangerLevel, exp.id, legRng, exp.lootCategories);
                    if (picked != null)
                    {
                        results.Add($"{sortie}:{exp.id}:{picked.id}");

                        // Verify route affinity compliance:
                        if (picked.routeAffinity != null && picked.routeAffinity.Count > 0)
                        {
                            bool matches = false;
                            foreach (var token in picked.routeAffinity)
                            {
                                if (exp.lootCategories != null && exp.lootCategories.Contains(token))
                                {
                                    matches = true;
                                    break;
                                }
                            }
                            Assert.True(matches,
                                $"Selected specialized micro-location '{picked.id}' does not match route categories on '{exp.id}'");
                        }
                    }
                }
                return results;
            }

            var runA = RunSimulation(2026);
            var runB = RunSimulation(2026);

            // 1. Determinism
            Assert.Equal(runA.Count, runB.Count);
            for (int i = 0; i < runA.Count; i++)
            {
                Assert.Equal(runA[i], runB[i]);
            }

            // 2. Selection count & diversity
            Assert.True(runA.Count >= 80, $"Expected at least 80 successful selections across 100 sorties, got {runA.Count}");

            var uniqueEncounterIds = new HashSet<string>();
            foreach (var record in runA)
            {
                string encounterId = record.Split(':')[2];
                uniqueEncounterIds.Add(encounterId);
            }

            // Across 100 sorties over diverse destinations, multiple distinct micro-locations must be chosen
            Assert.True(uniqueEncounterIds.Count >= 10,
                $"Expected at least 10 distinct micro-locations chosen in 100 expeditions, got {uniqueEncounterIds.Count}");
        }

        [Fact]
        public void Simulation_DepletionPrunesSelectedCandidates()
        {
            var sys = CreateSystem();
            var rng = new SeededRng(777);
            var cats = new List<string> { "fuel", "scrap_metal", "mechanical_parts", "canned_food" };

            // Pick an encounter
            var firstPick = sys.SelectEncounter("Normal", 1f, "rural_gas_station", rng, cats);
            Assert.NotNull(firstPick);

            var depletingChoice = firstPick!.choices.Find(c => c.depletesOnResolve);
            Assert.NotNull(depletingChoice);

            // Deplete it
            sys.TryResolve(firstPick.id, depletingChoice!.choiceId, "rural_gas_station", 1);
            Assert.True(sys.IsDepleted(firstPick.id));

            // Run 50 subsequent selections — the depleted encounter must NEVER be chosen
            for (int i = 0; i < 50; i++)
            {
                var pick = sys.SelectEncounter("Normal", 1f, "rural_gas_station", new SeededRng(i * 13), cats);
                if (pick != null)
                {
                    Assert.NotEqual(firstPick.id, pick.id);
                }
            }
        }
    }
}
