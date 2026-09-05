using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// F6 / Section 6.12 — Tests for registration of micro_locations.json into NarrativeEncounterSystem catalog.
    /// Verifies source separation, metadata stamping (isMicroLocation=true), field fidelity,
    /// duplicate detection with source diagnostics, eligibility filters, and weight preservation.
    /// </summary>
    public class MicroLocationCatalogLoaderTests
    {
        private static string DataDir()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent!;
            return Path.Combine(dir!.FullName, "Assets", "StreamingAssets", "Data");
        }

        [Fact]
        public void F6_01_MicroLoader_ReturnsAuthoredDefinitions()
        {
            var defs = MicroLocationEncounterLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotNull(defs);
            Assert.True(defs.Count >= 25, $"Expected at least 25 micro-locations, got {defs.Count}");
        }

        [Fact]
        public void F6_02_AllMicroDefinitions_HaveIsMicroLocationTrue()
        {
            var defs = MicroLocationEncounterLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotEmpty(defs);
            foreach (var def in defs)
            {
                Assert.True(def.isMicroLocation, $"Encounter {def.id} should have isMicroLocation = true");
                Assert.True(def.IsMicroLocation, $"Encounter {def.id} should have IsMicroLocation = true");
            }
        }

        [Fact]
        public void F6_03_CategoryValues_AreValidEncounterCategories()
        {
            var defs = MicroLocationEncounterLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var validCategories = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Discovery", "Hazard", "Social", "Trade"
            };

            foreach (var def in defs)
            {
                Assert.True(validCategories.Contains(def.category),
                    $"Encounter {def.id} has invalid category '{def.category}'");
            }
        }

        [Fact]
        public void F6_04_BaseWeights_PreservedFromAuthoring_PositiveAndFinite()
        {
            var defs = MicroLocationEncounterLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            foreach (var def in defs)
            {
                Assert.True(def.baseWeight > 0f, $"Encounter {def.id} base weight should be > 0");
                Assert.False(float.IsNaN(def.baseWeight), $"Encounter {def.id} base weight is NaN");
                Assert.False(float.IsInfinity(def.baseWeight), $"Encounter {def.id} base weight is Infinity");
            }

            // Spot check specific known authored weights
            var memorial = defs.Find(d => d.id == "micro_roadside_memorial");
            Assert.NotNull(memorial);
            Assert.Equal(0.8f, memorial!.baseWeight, 3);

            var truck = defs.Find(d => d.id == "micro_crashed_truck");
            Assert.NotNull(truck);
            Assert.Equal(0.6f, truck!.baseWeight, 3);
        }

        [Fact]
        public void F6_05_MinDangerLevels_AreNonNegativeAndFinite()
        {
            var defs = MicroLocationEncounterLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            foreach (var def in defs)
            {
                Assert.True(def.minDangerLevel >= 0f, $"Encounter {def.id} minDangerLevel should be >= 0");
                Assert.False(float.IsNaN(def.minDangerLevel));
                Assert.False(float.IsInfinity(def.minDangerLevel));
            }
        }

        [Fact]
        public void F6_06_RequiredLocationIds_PreservedAccurately()
        {
            var defs = MicroLocationEncounterLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

            // Destination-bound encounters must carry their required location
            var hospitalLedger = defs.Find(d => d.id == "micro_hospital_chapel_ledger");
            Assert.NotNull(hospitalLedger);
            Assert.Equal("abandoned_hospital", hospitalLedger!.requiredLocationId);

            var depotRaft = defs.Find(d => d.id == "micro_depot_undertow_raft_line");
            Assert.NotNull(depotRaft);
            Assert.Equal("location_flooded_subway_depot", depotRaft!.requiredLocationId);

            var gammaBoard = defs.Find(d => d.id == "micro_gamma_levy_board");
            Assert.NotNull(gammaBoard);
            Assert.Equal("loc_garrison_checkpoint_gamma", gammaBoard!.requiredLocationId);
        }

        [Fact]
        public void F6_07_DuplicateId_FailsCatalogLoadWithSourceDiagnostics()
        {
            var builder = new EncounterCatalogBuilder();
            var coreDef = new EncounterDefinition { id = "duplicate_test_encounter", baseWeight = 1f, sourceFile = "narrative_encounters.json" };
            var microDef = new EncounterDefinition { id = "duplicate_test_encounter", baseWeight = 0.5f, sourceFile = "micro_locations.json", isMicroLocation = true };

            builder.Add(coreDef);

            var ex = Assert.Throws<InvalidOperationException>(() => builder.Add(microDef));
            Assert.Contains("duplicate_test_encounter", ex.Message);
            Assert.Contains("narrative_encounters.json", ex.Message);
            Assert.Contains("micro_locations.json", ex.Message);
        }

        [Fact]
        public void F6_08_EligibleMicroLocation_HasPositiveEffectiveWeight()
        {
            var defs = MicroLocationEncounterLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var memorial = defs.Find(d => d.id == "micro_roadside_memorial")!;
            Assert.NotNull(memorial);

            float weight = memorial.GetEffectiveWeight("Normal", 1f, "any_location");
            Assert.True(weight > 0f, $"Expected positive effective weight, got {weight}");
        }

        [Fact]
        public void F6_09_LowDangerContext_GivesZeroWeight()
        {
            var defs = MicroLocationEncounterLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            // Crashed truck requires minDangerLevel = 1
            var truck = defs.Find(d => d.id == "micro_crashed_truck")!;
            Assert.NotNull(truck);
            Assert.True(truck.minDangerLevel >= 1f);

            float weightInDangerZero = truck.GetEffectiveWeight("Normal", 0f, "any_location");
            Assert.Equal(0f, weightInDangerZero);

            float weightInDangerOne = truck.GetEffectiveWeight("Normal", 1f, "any_location");
            Assert.True(weightInDangerOne > 0f);
        }

        [Fact]
        public void F6_10_WrongLocationContext_GivesZeroWeight()
        {
            var defs = MicroLocationEncounterLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var hospitalLedger = defs.Find(d => d.id == "micro_hospital_chapel_ledger")!;
            Assert.NotNull(hospitalLedger);
            Assert.Equal("abandoned_hospital", hospitalLedger.requiredLocationId);

            float weightWrongLoc = hospitalLedger.GetEffectiveWeight("Normal", 1f, "suburban_ruins");
            Assert.Equal(0f, weightWrongLoc);

            float weightRightLoc = hospitalLedger.GetEffectiveWeight("Normal", 1f, "abandoned_hospital");
            Assert.True(weightRightLoc > 0f);
        }

        [Fact]
        public void F6_11_CoreEncounterBaseWeights_UnchangedByMicroRegistration()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string dataDir = DataDir();

            var coreOnly = NarrativeEncounterCatalogLoader.LoadCoreEncounters(dataDir, fileIO, json);
            var composed = NarrativeEncounterCatalogLoader.Load(dataDir, fileIO, json);

            Assert.NotEmpty(coreOnly);
            Assert.True(composed.Count > coreOnly.Count);

            foreach (var coreDef in coreOnly)
            {
                var matchInComposed = composed.Find(c => c.id == coreDef.id);
                Assert.NotNull(matchInComposed);
                Assert.Equal(coreDef.baseWeight, matchInComposed!.baseWeight);
                Assert.Equal(coreDef.stealthWeightMultiplier, matchInComposed.stealthWeightMultiplier);
                Assert.Equal(coreDef.speedWeightMultiplier, matchInComposed.speedWeightMultiplier);
                Assert.False(matchInComposed.isMicroLocation, $"Core encounter {coreDef.id} should NOT be marked micro");
            }
        }

        [Fact]
        public void F6_12_ComposedCatalog_IsDeterministic()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string dataDir = DataDir();

            var catalog1 = NarrativeEncounterCatalogLoader.Load(dataDir, fileIO, json);
            var catalog2 = NarrativeEncounterCatalogLoader.Load(dataDir, fileIO, json);

            Assert.Equal(catalog1.Count, catalog2.Count);
            for (int i = 0; i < catalog1.Count; i++)
            {
                Assert.Equal(catalog1[i].id, catalog2[i].id);
                Assert.Equal(catalog1[i].baseWeight, catalog2[i].baseWeight);
                Assert.Equal(catalog1[i].isMicroLocation, catalog2[i].isMicroLocation);
                Assert.Equal(catalog1[i].sourceFile, catalog2[i].sourceFile);
            }
        }

        [Fact]
        public void F6_13_AllChoiceExtendedFields_SurviveParse()
        {
            var defs = MicroLocationEncounterLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

            // 1. grantItemId / grantItemQuantity / depletesOnResolve
            var truck = defs.Find(d => d.id == "micro_crashed_truck")!;
            var cargoChoice = truck.choices.Find(c => c.choiceId == "search_truck_cargo")!;
            Assert.Equal("canned_food", cargoChoice.grantItemId);
            Assert.Equal(2, cargoChoice.grantItemQuantity);
            Assert.True(cargoChoice.depletesOnResolve);

            // 2. setWorldFlag
            var generator = defs.Find(d => d.id == "micro_abandoned_generator")!;
            var markChoice = generator.choices.Find(c => c.choiceId == "mark_generator")!;
            Assert.Equal("micro_generator_marked", markChoice.setWorldFlag);

            // 3. journalUnlockId
            var clinic = defs.Find(d => d.id == "micro_makeshift_clinic")!;
            var triageChoice = clinic.choices.Find(c => c.choiceId == "read_triage_list")!;
            Assert.Equal("micro_clinic_triage", triageChoice.journalUnlockId);

            // 4. discoverLocationId
            var post = defs.Find(d => d.id == "micro_observation_post")!;
            var mapChoice = post.choices.Find(c => c.choiceId == "read_grid_references")!;
            Assert.Equal("rural_gas_station", mapChoice.discoverLocationId);
        }

        [Fact]
        public void F6_14_ExpeditionSelector_CanSeeMicroEntries()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string dataDir = DataDir();

            var sys = new NarrativeEncounterSystem();
            sys.RegisterRange(NarrativeEncounterCatalogLoader.Load(dataDir, fileIO, json));

            // Verify micro entries are in the catalog and can be retrieved
            var microEntries = sys.Catalog.Where(e => e.isMicroLocation).ToList();
            Assert.True(microEntries.Count >= 25);

            var memorial = sys.Find("micro_roadside_memorial");
            Assert.NotNull(memorial);
            Assert.True(memorial!.isMicroLocation);
        }

        [Fact]
        public void F6_15_SourceAwareDiagnostics_IdentifySourceFile()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string dataDir = DataDir();

            var composed = NarrativeEncounterCatalogLoader.Load(dataDir, fileIO, json);
            var micro = composed.Find(e => e.id == "micro_roadside_memorial");
            Assert.NotNull(micro);
            Assert.Equal(MicroLocationEncounterLoader.FileName, micro!.sourceFile);

            var core = composed.Find(e => e.id == "enc_dead_letter_office");
            Assert.NotNull(core);
            Assert.Equal(NarrativeEncounterCatalogLoader.FileName, core!.sourceFile);
        }
    }
}
