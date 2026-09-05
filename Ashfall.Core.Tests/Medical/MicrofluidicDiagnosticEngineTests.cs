// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.AdvancedMachinery;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class MicrofluidicDiagnosticEngineTests
    {
        private static string FindDataDir()
        {
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir);
            return dir ?? string.Empty;
        }

        private static Func<IReadOnlyList<InventoryDemand>, bool> MakeConsumer(Dictionary<string, int> inventory)
        {
            return demands =>
            {
                foreach (var d in demands)
                {
                    if (!inventory.TryGetValue(d.ItemId, out int current) || current < d.Quantity)
                        return false;
                }
                foreach (var d in demands)
                {
                    inventory[d.ItemId] -= d.Quantity;
                }
                return true;
            };
        }

        [Fact]
        public void CartridgeManufacturing_ConsumesInputsAtomically()
        {
            var engine = new MicrofluidicDiagnosticEngine();
            var inv = new Dictionary<string, int>
            {
                { "item_pdms_silicone_kit", 1 },
                { "item_assay_reagent_pack", 1 }
            };

            bool ok = engine.StartCartridgeManufacturing("job_01", "microfluidic_assay_cholera", "op_1", MakeConsumer(inv), out string err);
            Assert.True(ok, err);
            Assert.Equal(0, inv["item_pdms_silicone_kit"]);
            Assert.Equal(0, inv["item_assay_reagent_pack"]);
            Assert.NotNull(engine.StateDto.ActiveManufacturingJob);
        }

        [Fact]
        public void CartridgeManufacturing_MissingReagent_ConsumesNothing()
        {
            var engine = new MicrofluidicDiagnosticEngine();
            var inv = new Dictionary<string, int>
            {
                { "item_pdms_silicone_kit", 2 }
                // reagent pack absent
            };

            bool ok = engine.StartCartridgeManufacturing("job_01", "microfluidic_assay_cholera", "op_1", MakeConsumer(inv), out string err);
            Assert.False(ok);
            Assert.Equal("insufficient_manufacturing_inputs", err);
            Assert.Equal(2, inv["item_pdms_silicone_kit"]);
            Assert.Null(engine.StateDto.ActiveManufacturingJob);
        }

        [Fact]
        public void UnpoweredTick_PausesAllProcesses_AndResumesWithPower()
        {
            var engine = new MicrofluidicDiagnosticEngine();
            engine.StartDiagnosticRun("run_01", "survivor_01", "microfluidic_assay_cholera", "op_1", 2, 100f, (_, _) => true, out _);

            // Total blackout: no progress at all, run pauses
            engine.Tick(60f, PowerSupplyContext.Unpowered(), null, new SeededRng(1), (_, _) => true);
            Assert.Single(engine.StateDto.ActiveRuns);
            Assert.Equal(ProcessState.Paused, engine.StateDto.ActiveRuns[0].Status);
            Assert.Equal(0f, engine.StateDto.ActiveRuns[0].ProgressMinutes);

            // Severe brownout below 50% nominal draw: still paused
            engine.Tick(60f, PowerSupplyContext.Throttled(0.5f, 2.5f), null, new SeededRng(2), (_, _) => true);
            Assert.Equal(ProcessState.Paused, engine.StateDto.ActiveRuns[0].Status);
            Assert.Equal(0f, engine.StateDto.ActiveRuns[0].ProgressMinutes);

            // Power returns: the paused run auto-resumes and completes
            engine.Tick(35f, PowerSupplyContext.Full(5f), null, new SeededRng(3), (_, _) => true);
            Assert.Empty(engine.StateDto.ActiveRuns);
            Assert.Single(engine.StateDto.CompletedResults);
        }

        [Fact]
        public void DiagnosticRun_ResolvesPositiveForInfectedPatient()
        {
            var engine = new MicrofluidicDiagnosticEngine();
            bool runStarted = engine.StartDiagnosticRun("run_01", "survivor_01", "microfluidic_assay_cholera", "op_1", 2, 100f, (_, _) => true, out _);
            Assert.True(runStarted);
            Assert.Single(engine.StateDto.ActiveRuns);

            var power = PowerSupplyContext.Full(5.0f);
            var rng = new SeededRng(100);

            // Tick for 35 minutes (assay duration is 30 mins)
            engine.Tick(35f, power, null, rng, (patientId, diseaseId) =>
            {
                return patientId == "survivor_01" && diseaseId == "disease_cholera";
            });

            Assert.Empty(engine.StateDto.ActiveRuns);
            Assert.Single(engine.StateDto.CompletedResults);
            var result = engine.StateDto.CompletedResults[0];
            Assert.Equal("survivor_01", result.PatientId);
            Assert.Equal(DiagnosticResultKind.Positive, result.ResultKind);
            Assert.InRange(result.Confidence01, 0.5f, 0.99f);
        }

        [Fact]
        public void DiagnosticRun_ResolvesNegativeForHealthyPatient()
        {
            var engine = new MicrofluidicDiagnosticEngine();
            engine.StartDiagnosticRun("run_02", "survivor_02", "microfluidic_assay_zoonotic_flu", "op_1", 2, 100f, (_, _) => true, out _);

            var power = PowerSupplyContext.Full(5.0f);
            var rng = new SeededRng(200);

            // Healthy patient (returns false for all diseases)
            engine.Tick(50f, power, null, rng, (_, _) => false);

            Assert.Single(engine.StateDto.CompletedResults);
            var result = engine.StateDto.CompletedResults[0];
            Assert.Equal(DiagnosticResultKind.Negative, result.ResultKind);
            Assert.InRange(result.Confidence01, 0.5f, 0.99f);
        }

        [Fact]
        public void NullClinicalPredicate_YieldsIndeterminate_AndRecordsNoEvidence()
        {
            var engine = new MicrofluidicDiagnosticEngine();
            engine.StartDiagnosticRun("run_01", "survivor_01", "microfluidic_assay_cholera", "op_1", 2, 100f, (_, _) => true, out _);

            // Without a clinical truth source the assay must not guess.
            engine.Tick(35f, PowerSupplyContext.Full(5f), null, new SeededRng(300), null);

            Assert.Empty(engine.StateDto.ActiveRuns);
            Assert.Empty(engine.StateDto.CompletedResults);
        }

        [Fact]
        public void RegisterAssay_ClampsProbabilitiesToUnitInterval()
        {
            var engine = new MicrofluidicDiagnosticEngine();
            engine.RegisterAssay(new MicrofluidicAssayDef
            {
                Id = "assay_out_of_range",
                Sensitivity = 1.5f,
                Specificity = -0.2f,
                EarlyDetectionModifier = 2.0f,
                InvalidRunBaseChance = -1.0f
            });

            var def = engine.GetAssay("assay_out_of_range");
            Assert.NotNull(def);
            Assert.Equal(1.0f, def!.Sensitivity);
            Assert.Equal(0.0f, def.Specificity);
            Assert.Equal(1.0f, def.EarlyDetectionModifier);
            Assert.Equal(0.0f, def.InvalidRunBaseChance);
        }

        [Fact]
        public void SeededSweep_TruePositiveRateTracksAuthoredSensitivity()
        {
            var engine = new MicrofluidicDiagnosticEngine();
            var rng = new SeededRng(4242);
            const int Runs = 200;
            int positives = 0;

            for (int i = 0; i < Runs; i++)
            {
                engine.StartDiagnosticRun($"run_{i}", "survivor_01", "microfluidic_assay_cholera", "op_1", 1, 0f, (_, _) => true, out _);
                engine.Tick(35f, PowerSupplyContext.Full(5f), null, rng, (_, _) => true);
                if (engine.StateDto.CompletedResults.Count > 0 &&
                    engine.StateDto.CompletedResults[^1].ResultKind == DiagnosticResultKind.Positive)
                {
                    positives++;
                }
                engine.StateDto.CompletedResults.Clear();
            }

            // Authored sensitivity is 0.94; invalid runs (0.04) remove some samples.
            double observedRate = positives / (double)Runs;
            Assert.InRange(observedRate, 0.84, 1.0);
        }

        [Fact]
        public void ConfidenceDistribution_OverlapsBetweenTrueAndFalsePositives()
        {
            // If confidence ranges were branch-specific, a Positive above/below a
            // threshold would reveal whether the patient is truly infected. The
            // shared distribution must make the two populations indistinguishable.
            var engine = new MicrofluidicDiagnosticEngine();
            var rng = new SeededRng(777);
            var truePositiveConfidences = new List<float>();
            var falsePositiveConfidences = new List<float>();

            for (int i = 0; i < 400; i++)
            {
                bool infected = i % 2 == 0;
                engine.StartDiagnosticRun($"run_{i}", "survivor_01", "microfluidic_assay_cholera", "op_1", 1, 0f, (_, _) => true, out _);
                engine.Tick(35f, PowerSupplyContext.Full(5f), null, rng, (_, _) => infected);
                var last = engine.StateDto.CompletedResults.Count > 0 ? engine.StateDto.CompletedResults[^1] : null;
                if (last != null && last.ResultKind == DiagnosticResultKind.Positive)
                {
                    (infected ? truePositiveConfidences : falsePositiveConfidences).Add(last.Confidence01);
                }
                engine.StateDto.CompletedResults.Clear();
            }

            Assert.True(truePositiveConfidences.Count > 10, "sweep produced too few true positives");
            Assert.True(falsePositiveConfidences.Count > 0, "sweep produced no false positives (raise iterations or seed)");

            // With a specificity of 0.98, false positives are rare — but with 200
            // healthy draws at least one appears; the ranges must overlap.
            Assert.True(
                truePositiveConfidences.Min() <= falsePositiveConfidences.Max() + 0.01f,
                $"confidence leak: TP min {truePositiveConfidences.Min():F3} must overlap FP max {falsePositiveConfidences.Max():F3}");
        }

        [Fact]
        public void StateCaptureAndRestore_RoundtripsAccurately()
        {
            var engine = new MicrofluidicDiagnosticEngine();
            engine.StartDiagnosticRun("run_save", "survivor_09", "microfluidic_assay_blood_fever", "op_1", 5, 50f, (_, _) => true, out _);

            var captured = engine.CaptureState();
            Assert.Single(captured.ActiveRuns);

            var restored = new MicrofluidicDiagnosticEngine(captured);
            Assert.Single(restored.StateDto.ActiveRuns);
            Assert.Equal("run_save", restored.StateDto.ActiveRuns[0].RunId);
        }

        [Fact]
        public void Catalog_LoadsAllEightAssays_AndMatchesEngineAuthority()
        {
            string dataDir = FindDataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "Could not locate StreamingAssets/Data directory");

            var catalog = MicrofluidicDiagnosticCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Equal(8, catalog.assays.Count);
            Assert.Equal(2.5f, catalog.machine.nominal_power_kw);

            var engine = new MicrofluidicDiagnosticEngine();
            var catalogIds = catalog.ToAssayDefs().Select(d => d.Id).OrderBy(x => x, StringComparer.Ordinal).ToList();
            var engineIds = engine.Assays.Select(d => d.Id).OrderBy(x => x, StringComparer.Ordinal).ToList();
            Assert.Equal(catalogIds, engineIds);

            var zoonotic = catalog.ToAssayDefs().First(d => d.Id == "microfluidic_assay_zoonotic_flu");
            Assert.Equal(0.05f, zoonotic.InvalidRunBaseChance);
            Assert.Equal(45.0f, zoonotic.BaseDurationMinutes);
        }
    }
}
