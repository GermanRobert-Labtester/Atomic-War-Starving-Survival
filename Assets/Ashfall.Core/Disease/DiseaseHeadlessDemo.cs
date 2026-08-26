using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Disease;

namespace Ashfall.Core
{
    /// <summary>
    /// Disease Expansion pack-minimum smoke: exact ids resolve, the catalog
    /// loads with every countermeasure item present in items.json, an outbreak
    /// declares at the threshold, quarantine stalls the vector, protocols block
    /// it outright, outcomes resolve deterministically, and the state save
    /// round-trips. Invoked by the expansions selftest and by xUnit.
    /// </summary>
    public static class DiseaseHeadlessDemo
    {
        public const int DemoSeed = 1013;

        public static HeadlessReport Run(string? dataDirectory = null, ILog? log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new HeadlessReport();

            void Check(bool condition, string name)
            {
                report.Checks.Add(new HeadlessCheck { Name = name, Passed = condition });
                if (condition) { report.PassedCount++; log.Info("[PASS] " + name); }
                else { report.FailedCount++; log.Error("[FAIL] " + name); }
            }

            log.Info("[DiseaseHeadlessDemo] begin");

            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // Identity resolution (exact ids, never aliased).
            Check(DiseaseIds.ExpansionId == "expansion_disease_expansion", "expansion id expansion_disease_expansion");
            Check(DiseaseIds.Cholera == "disease_cholera", "disease id disease_cholera");
            Check(DiseaseIds.ZoonoticFlu == "disease_zoonotic_flu", "disease id disease_zoonotic_flu");
            Check(DiseaseIds.BloodFever == "disease_blood_fever", "disease id disease_blood_fever");
            Check(DiseaseIds.SporeBlight == "disease_spore_blight", "disease id disease_spore_blight");

            // Static catalog loads from disk.
            var catalog = DiseaseCatalogLoader.Load(dataDirectory, files, json);
            Check(catalog != null && !catalog.HasErrors, "disease_catalog.json loads with no schema errors");
            Check(catalog != null && catalog.Count >= 4, "disease_catalog.json registers >= 4 diseases");

            // Authored countermeasure items resolve in items.json (the shared
            // inventory catalog — a dangling ref would silently neuter a protocol).
            if (catalog != null && catalog.Count > 0)
            {
                var itemIds = LoadItemIds(dataDirectory, files, json);
                for (int i = 0; i < catalog.Diseases.Count; i++)
                {
                    var d = catalog.Diseases[i];
                    if (d == null || string.IsNullOrEmpty(d.countermeasure_item_id)) continue;
                    bool present = itemIds.Contains(d.countermeasure_item_id);
                    Check(present, "countermeasure item '" + d.countermeasure_item_id + "' for " + d.id + " exists in items.json");
                }
            }

            var system = new DiseaseSystem(rng: new SeededRng(DemoSeed), log: log);
            system.BindCatalog(catalog ?? new DiseaseCatalog());

            // Vectors + protocol wiring.
            Check(system.GetTransmissionVector(DiseaseIds.Cholera) == DiseaseVectorNames.Water,
                "cholera is a waterborne pathogen");
            Check(system.GetTransmissionVector(DiseaseIds.SporeBlight) == DiseaseVectorNames.Spore,
                "spore blight is a contagious spore vector");
            Check(!system.IsVectorBlocked(DiseaseVectorNames.Water), "water vector unblocked before the protocol");

            // Outbreak declaration at the threshold of 3 active infections.
            system.Infect("demo_survivor_a", DiseaseIds.Cholera, day: 10);
            system.Infect("demo_survivor_b", DiseaseIds.Cholera, day: 10);
            system.Infect("demo_survivor_c", DiseaseIds.Cholera, day: 10);
            Check(system.GetDiseaseState(DiseaseIds.Cholera) != null
                  && system.GetDiseaseState(DiseaseIds.Cholera)!.outbreak_active,
                "three active infections declare an outbreak");
            Check(system.GetSnapshot().total_outbreaks == 1, "outbreaks_total == 1");

            // Quarantine stalls the vector: all three isolated, tick with a live
            // roster — nobody new is exposed.
            foreach (var s in new[] { "demo_survivor_a", "demo_survivor_b", "demo_survivor_c" })
                system.Quarantine(s, DiseaseIds.Cholera);
            var roster = new List<string>
            {
                "demo_survivor_d", "demo_survivor_e", "demo_survivor_f",
                "demo_survivor_g", "demo_survivor_h"
            };
            int before = system.TotalInfectionsHistory;
            system.TickDaily(day: 12, candidates: roster);
            system.TickDaily(day: 13, candidates: roster);
            system.TickDaily(day: 14, candidates: roster);
            Check(system.TotalInfectionsHistory == before,
                "quarantined ward cannot seed new infections");

            // The quarantine ward contains the outbreak without loss (cholera
            // kills only on a seeded outcome roll at illness_days; here the ward
            // resolves before the days run out).
            var cholera = system.GetDiseaseState(DiseaseIds.Cholera);
            Check(cholera != null && !cholera.outbreak_active, "outbreak contained once no case is contagious");
            Check(cholera != null && cholera.outbreaks_prevented == 1, "contained outbreak counted as prevented");

            // Protocols: purifying the water blocks the water vector outright.
            system.PurifyWater();
            system.Infect("demo_survivor_i", DiseaseIds.Cholera, day: 20);
            int beforeProtocol = system.TotalInfectionsHistory;
            system.TickDaily(day: 21, candidates: roster);
            system.TickDaily(day: 22, candidates: roster);
            Check(system.IsVectorBlocked(DiseaseVectorNames.Water), "purified water blocks the water vector");
            Check(system.TotalInfectionsHistory == beforeProtocol,
                "no new cases while the water protocol holds");

            // Determinism: same seed, same candidate order ⇒ same outbreak.
            var a = SimulateOutbreak(DemoSeed, files, json, dataDirectory);
            var b = SimulateOutbreak(DemoSeed, files, json, dataDirectory);
            Check(a == b, "deterministic spread given the same seed and roster");

            // Save round-trip through the same serializer the hosts use.
            var stateA = system.CaptureState();
            string encoded = json.Serialize(stateA);
            var stateB = json.Deserialize<DiseaseSystemState>(encoded);
            var restored = new DiseaseSystem(log: log);
            restored.RestoreState(stateB!);
            restored.BindCatalog(catalog ?? new DiseaseCatalog());
            Check(restored.CaptureState().diseases.Count == system.CaptureState().diseases.Count,
                "save round-trip preserves every disease entry");
            Check(restored.GetSnapshot().total_deaths == system.GetSnapshot().total_deaths
                  && restored.GetSnapshot().total_recovered == system.GetSnapshot().total_recovered,
                "save round-trip preserves outcome history");
            // A restored system with the same seed continues the same outcome
            // sequence as a fresh twin (determinism across reload).
            var s1 = stateA;
            var twin = new DiseaseSystem(rng: new SeededRng(s1.rngSeed), log: log);
            twin.BindCatalog(catalog ?? new DiseaseCatalog());
            restored.TickDaily(day: 30, candidates: roster);
            twin.TickDaily(day: 30, candidates: roster);
            Check(restored.CaptureState().rngSeed == twin.CaptureState().rngSeed,
                "restored system resumes the same RNG stream");

            log.Info("[DiseaseHeadlessDemo] done");
            report.Passed = report.FailedCount == 0;
            report.Summary = "[DiseaseHeadlessDemo] " + report.PassedCount + "/"
                + (report.PassedCount + report.FailedCount) + " PASSED"
                + (report.Passed ? string.Empty : " (FAILED)")
                + " — disease catalog, quarantine, protocols, determinism, save round-trip";
            return report;
        }

        /// <summary>Replay the same seeded 40-day outbreak; returns a compact fingerprint.</summary>
        private static string SimulateOutbreak(int seed, IFileIO files, IJsonSerializer json, string dataDirectory)
        {
            var catalog = DiseaseCatalogLoader.Load(dataDirectory, files, json);
            var system = new DiseaseSystem(rng: new SeededRng(seed));
            system.BindCatalog(catalog);
            var roster = new List<string>();
            for (int i = 0; i < 12; i++) roster.Add("s_" + i);

            system.Infect("s_0", DiseaseIds.Cholera, day: 1);
            system.Infect("s_1", DiseaseIds.ZoonoticFlu, day: 1);
            system.Infect("s_2", DiseaseIds.BloodFever, day: 1);
            system.Infect("s_3", DiseaseIds.SporeBlight, day: 1);
            for (int day = 2; day <= 40; day++)
            {
                system.TickDaily(day, roster);
                if (day == 10) { system.Quarantine("s_3", DiseaseIds.SporeBlight); }
                if (day == 15) { system.PurifyWater(); }
            }
            var s = system.CaptureState();
            return string.Join("|",
                s.diseases[0].outbreaks_total, s.diseases[0].deaths_total,
                s.diseases[1].outbreaks_total, s.diseases[1].deaths_total,
                s.diseases[2].outbreaks_total, s.diseases[2].deaths_total,
                s.diseases[3].outbreaks_total, s.diseases[3].deaths_total,
                s.rngSeed);
        }

        private static HashSet<string> LoadItemIds(string dataDirectory, IFileIO files, IJsonSerializer json)
        {
            var ids = new HashSet<string>(StringComparer.Ordinal);
            if (string.IsNullOrEmpty(dataDirectory)) return ids;
            string path = files.Combine(dataDirectory, "items.json");
            if (!files.FileExists(path)) return ids;
            try
            {
                string raw = files.ReadAllText(path);
                List<DiseaseDemoItemRow> rows = null;
                try
                {
                    var root = json.Deserialize<DiseaseDemoItemsRoot>(raw);
                    rows = root?.items;
                }
                catch (Exception)
                {
                    // bare list fallback
                }
                if (rows == null)
                {
                    rows = CatalogLocator.LoadWrappedList<DiseaseDemoItemRow>(raw, SystemTextJsonSerializer.Options);
                }
                if (rows != null)
                {
                    for (int i = 0; i < rows.Count; i++)
                        if (rows[i] != null && !string.IsNullOrEmpty(rows[i].id))
                            ids.Add(rows[i].id);
                }
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine("[DiseaseHeadlessDemo] items.json read failed: " + e.Message);
            }
            return ids;
        }

        /// <summary>Minimal items.json row read for the countermeasure check.</summary>
        private sealed class DiseaseDemoItemRow
        {
            public string id = string.Empty;
        }

        [Serializable]
        private sealed class DiseaseDemoItemsRoot
        {
#pragma warning disable CS0649 // schema_version is deserialized for contract compliance, not read in code
            public int schema_version;
#pragma warning restore CS0649
            public List<DiseaseDemoItemRow> items = new List<DiseaseDemoItemRow>();
        }
    }
}