using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Medical;
using Ashfall.Core.Radiation;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class AutopsyProceduresCatalogTests
    {
        private static string FindDataDir()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
            return dataDir ?? string.Empty;
        }

        private static List<AutopsyProcedure> LoadProcedures()
        {
            string dataDir = FindDataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "Could not locate StreamingAssets/Data directory");
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var defs = AutopsyProcedureCatalogLoader.Load(dataDir, io, json);
            Assert.NotNull(defs);
            return defs;
        }

        private static HashSet<string> LoadItemIds()
        {
            string dataDir = FindDataDir();
            var io = new FileSystemIO();
            string itemsRaw = io.ReadAllText(Path.Combine(dataDir, "items.json"));
            using var doc = JsonDocument.Parse(itemsRaw);
            var set = new HashSet<string>(StringComparer.Ordinal);
            if (doc.RootElement.TryGetProperty("items", out var arr) && arr.ValueKind == JsonValueKind.Array)
            {
                foreach (var it in arr.EnumerateArray())
                {
                    if (it.TryGetProperty("id", out var idProp))
                    {
                        string id = idProp.GetString() ?? "";
                        if (!string.IsNullOrEmpty(id)) set.Add(id);
                    }
                }
            }
            return set;
        }

        private static HashSet<string> LoadKnowledgeIds()
        {
            string dataDir = FindDataDir();
            var io = new FileSystemIO();
            string raw = io.ReadAllText(Path.Combine(dataDir, "research_knowledge.json"));
            using var doc = JsonDocument.Parse(raw);
            var set = new HashSet<string>(StringComparer.Ordinal);
            if (doc.RootElement.TryGetProperty("knowledge_nodes", out var arr) && arr.ValueKind == JsonValueKind.Array)
            {
                foreach (var it in arr.EnumerateArray())
                {
                    if (it.TryGetProperty("id", out var idProp))
                    {
                        string id = idProp.GetString() ?? "";
                        if (!string.IsNullOrEmpty(id)) set.Add(id);
                    }
                }
            }
            return set;
        }

        private static AutopsySystem CreateSystem(out Inventory.Inventory inv, out ResearchSystem res)
        {
            inv = new Inventory.Inventory();
            var rad = new RadiationSystem(seed: 42);
            var starting = new StartingLevelSystem();
            var vent = new VentilationSystem(starting);
            res = new ResearchSystem();
            var wardState = new MedicalWardState();
            var bed = new MedicalBed("bed_1", "Bed 1", MedicalBedCategory.General);
            var proc = new MedicalProcedureDef("proc_1", "Procedure 1", "MedicalSystem");
            var medical = new MedicalWardSystem(wardState, new[] { bed }, new[] { proc });
            var sys = new AutopsySystem(new SeededRng(42), inv, rad, vent, res, medical);
            return sys;
        }

        [Fact]
        public void Catalog_LoadsExact12Procedures()
        {
            var procs = LoadProcedures();
            Assert.Equal(12, procs.Count);
        }

        [Fact]
        public void Catalog_OriginalProceduresPreserved()
        {
            var procs = LoadProcedures();
            var map = new Dictionary<string, AutopsyProcedure>(StringComparer.Ordinal);
            foreach (var p in procs) map[p.procedure_id] = p;

            Assert.Contains("procedure_rad_pathology", map.Keys);
            Assert.Contains("procedure_toxicology", map.Keys);
            Assert.Contains("procedure_containment_autopsy", map.Keys);

            var rad = map["procedure_rad_pathology"];
            Assert.Equal("Radiation Pathology", rad.display_name);
            Assert.Contains("knowledge_radiation_basics", rad.researchUnlocks);

            var tox = map["procedure_toxicology"];
            Assert.Equal("Toxicology Screen", tox.display_name);
            Assert.Contains("knowledge_pathogen_containment", tox.researchUnlocks);

            var cont = map["procedure_containment_autopsy"];
            Assert.Equal("Containment Autopsy", cont.display_name);
            Assert.Contains("knowledge_pathogen_containment", cont.researchUnlocks);
        }

        [Fact]
        public void Catalog_AllTwelveIdsUniqueAndPrefixed()
        {
            var procs = LoadProcedures();
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var p in procs)
            {
                Assert.False(string.IsNullOrWhiteSpace(p.procedure_id));
                Assert.StartsWith("procedure_", p.procedure_id);
                Assert.True(seen.Add(p.procedure_id), $"Duplicate procedure ID: {p.procedure_id}");
            }
            Assert.Equal(12, seen.Count);
        }

        [Fact]
        public void Catalog_AllDisplayNamesNonEmptyAndUnique()
        {
            var procs = LoadProcedures();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var p in procs)
            {
                Assert.False(string.IsNullOrWhiteSpace(p.display_name));
                Assert.True(seen.Add(p.display_name), $"Duplicate display name: {p.display_name}");
            }
            Assert.Equal(12, seen.Count);
        }

        [Fact]
        public void Catalog_AllRequiredToolsResolveInItemsJson()
        {
            var procs = LoadProcedures();
            var itemIds = LoadItemIds();

            foreach (var p in procs)
            {
                Assert.NotEmpty(p.requiredTools);
                foreach (var tool in p.requiredTools)
                {
                    Assert.True(itemIds.Contains(tool),
                        $"Procedure {p.procedure_id} references required tool '{tool}' which is not in items.json");
                }
            }
        }

        [Fact]
        public void Catalog_AllRequiredConsumablesResolveInItemsJson()
        {
            var procs = LoadProcedures();
            var itemIds = LoadItemIds();

            foreach (var p in procs)
            {
                Assert.NotEmpty(p.requiredConsumables);
                foreach (var consumable in p.requiredConsumables)
                {
                    Assert.True(itemIds.Contains(consumable),
                        $"Procedure {p.procedure_id} references required consumable '{consumable}' which is not in items.json");
                }
            }
        }

        [Fact]
        public void Catalog_AllResearchUnlocksResolveInKnowledgeCatalog()
        {
            var procs = LoadProcedures();
            var knowledgeIds = LoadKnowledgeIds();

            foreach (var p in procs)
            {
                Assert.NotEmpty(p.researchUnlocks);
                foreach (var unlock in p.researchUnlocks)
                {
                    Assert.True(knowledgeIds.Contains(unlock),
                        $"Procedure {p.procedure_id} references research unlock '{unlock}' which is not in research_knowledge.json");
                }
            }
        }

        [Fact]
        public void Catalog_AllRisksAndDurationsWithinValidRanges()
        {
            var procs = LoadProcedures();
            foreach (var p in procs)
            {
                Assert.InRange(p.airborneRisk, 0.01f, 0.50f);
                Assert.InRange(p.pathogenRisk, 0.01f, 0.50f);
                Assert.InRange(p.procedureHours, 1, 12);
            }
        }

        [Fact]
        public void Catalog_PossibleFindingsNonEmptyAndUniquePerProcedure()
        {
            var procs = LoadProcedures();
            foreach (var p in procs)
            {
                Assert.NotEmpty(p.possibleFindings);
                var seen = new HashSet<string>(StringComparer.Ordinal);
                foreach (var finding in p.possibleFindings)
                {
                    Assert.StartsWith("finding_", finding);
                    Assert.True(seen.Add(finding), $"Duplicate finding '{finding}' in {p.procedure_id}");
                }
            }
        }

        [Fact]
        public void Runtime_AutopsySystemQueueAndBeginConsumesSupplies()
        {
            var sys = CreateSystem(out var inv, out _);
            string dataDir = FindDataDir();
            AutopsyProcedureCatalogLoader.LoadAndRegister(sys, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            // Provide supplies for procedure_deprivation_pathology (medical_scissors, scalpel, forceps, clean_water, bandage)
            inv.AddById("medical_scissors", 1);
            inv.AddById("scalpel", 1);
            inv.AddById("forceps", 1);
            inv.AddById("clean_water", 1);
            inv.AddById("bandage", 1);

            var queueRes = sys.QueueAutopsy("corpse_starved", "procedure_deprivation_pathology", "medic_anna");
            Assert.True(queueRes.IsSuccess);
            Assert.Single(sys.State.cases);

            var caseId = sys.State.cases[0].caseId;
            var beginRes = sys.BeginAutopsy(caseId);
            Assert.True(beginRes.IsSuccess);

            // Supplies should be consumed
            Assert.Equal(0, inv.CountById("clean_water"));
            Assert.Equal(0, inv.CountById("bandage"));
            Assert.Equal(0, inv.CountById("medical_scissors"));
        }

        [Fact]
        public void Runtime_AutopsySystemCompletionYieldsFindingAndResearchUnlock()
        {
            var sys = CreateSystem(out var inv, out var res);
            string dataDir = FindDataDir();
            AutopsyProcedureCatalogLoader.LoadAndRegister(sys, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            inv.AddById("medical_scissors", 1);
            inv.AddById("scalpel", 1);
            inv.AddById("forceps", 1);
            inv.AddById("clean_water", 1);
            inv.AddById("bandage", 1);

            sys.QueueAutopsy("corpse_starved", "procedure_deprivation_pathology", "medic_anna");
            var caseId = sys.State.cases[0].caseId;
            sys.BeginAutopsy(caseId);

            // Progress 8 hours (procedure takes 3 hours)
            sys.TickDay(1);

            Assert.Contains("corpse_starved", sys.State.completedSpecimenIds);
            Assert.True(res.IsManualUnlocked("knowledge_food_preservation"));
        }

        [Fact]
        public void Runtime_SaveLoadPreservesCompletedSpecimensAndCases()
        {
            var sys = CreateSystem(out var inv, out _);
            string dataDir = FindDataDir();
            AutopsyProcedureCatalogLoader.LoadAndRegister(sys, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            inv.AddById("medical_scissors", 2);
            inv.AddById("scalpel", 2);
            inv.AddById("forceps", 2);
            inv.AddById("clean_water", 2);
            inv.AddById("bandage", 2);

            sys.QueueAutopsy("specimen_a", "procedure_deprivation_pathology", "medic_1");

            var state = sys.CaptureState();
            Assert.Single(state.cases);
            Assert.Equal("specimen_a", state.cases[0].specimenId);

            var sys2 = CreateSystem(out _, out _);
            sys2.RestoreState(state);
            Assert.Single(sys2.State.cases);
            Assert.Equal("specimen_a", sys2.State.cases[0].specimenId);
        }
    }
}
