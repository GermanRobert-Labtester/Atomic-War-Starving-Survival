// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Expansion 29 (The Glass — precision vitrification & optics).

using System;
using System.IO;
using Godot;
using Ashfall.Core.Optics;

namespace AtomicWar.GodotApp
{
    public static class HostCliGlassworks
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Glassworks Self-Test (Expansion 29) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: crude glass cannot hold optical precision.
                var crude = PrecisionGlassworksOpticsEngine.GrindCorrectionLens(
                    GlassPurityTier.Crude, VisionCorrectionBand.MildMyopia, 1000, 1000, 7);
                if (!crude.MeetsPrescriptionTolerance)
                {
                    GD.Print("[PASS] Check 1: Crude glass cannot satisfy a prescription.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 1: Crude glass unexpectedly satisfied a prescription.");

                // Check 2: high-purity batch anneals to PrecisionOptic at optimal temperature.
                var batch = new GlassBatchState { BatchId = "b1", SilicaPurityPermille = 980 };
                for (int i = 0; i < PrecisionGlassworksOpticsEngine.MaxAnnealingStages; i++)
                    PrecisionGlassworksOpticsEngine.AdvanceAnnealingStage(batch, 900);
                if (!batch.IsCracked && batch.ResultingTier == GlassPurityTier.PrecisionOptic)
                {
                    GD.Print($"[PASS] Check 2: 980\u2030 silica annealed to {batch.ResultingTier}.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 2: Annealing produced {batch.ResultingTier} (cracked={batch.IsCracked}).");

                // Check 3: a cold kiln introduces thermal shock and cracks the batch.
                var shocked = new GlassBatchState { BatchId = "b2", SilicaPurityPermille = 900 };
                for (int i = 0; i < PrecisionGlassworksOpticsEngine.MaxAnnealingStages; i++)
                    PrecisionGlassworksOpticsEngine.AdvanceAnnealingStage(shocked, 100);
                if (shocked.IsCracked)
                {
                    GD.Print($"[PASS] Check 3: Cold kiln cracked the batch (risk {shocked.ThermalShockRiskPermille}\u2030).");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 3: Cold kiln did not crack the batch.");

                // Check 4: purity tier mapping by silica band.
                bool tierMap = TierFor(980) == GlassPurityTier.PrecisionOptic
                    && TierFor(850) == GlassPurityTier.OpticalGrade
                    && TierFor(650) == GlassPurityTier.CommonWindow
                    && TierFor(400) == GlassPurityTier.Crude;
                if (tierMap)
                {
                    GD.Print("[PASS] Check 4: Purity tier bands map correctly.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 4: Purity tier mapping incorrect.");

                // Check 5: precision glass + skilled grinder meets a mild prescription.
                var lens = PrecisionGlassworksOpticsEngine.GrindCorrectionLens(
                    GlassPurityTier.PrecisionOptic, VisionCorrectionBand.MildMyopia, 900, 1000, 42);
                if (lens.MeetsPrescriptionTolerance && !lens.LensCracked)
                {
                    GD.Print($"[PASS] Check 5: Lens met tolerance at {lens.OpticalPrecisionPermille}\u2030.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 5: Lens did not meet tolerance ({lens.OpticalPrecisionPermille}\u2030).");

                // Check 6: severe myopia on common glass with low skill cracks the lens.
                var cracked = PrecisionGlassworksOpticsEngine.GrindCorrectionLens(
                    GlassPurityTier.CommonWindow, VisionCorrectionBand.SevereMyopia, 200, 1000, 5);
                if (cracked.LensCracked)
                {
                    GD.Print("[PASS] Check 6: Severe prescription on common glass cracked the lens.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 6: Severe prescription did not crack the lens.");

                // Check 7: theodolite is field-ready only with optical-grade or better.
                var ready = PrecisionGlassworksOpticsEngine.CalibrateTheodolite(GlassPurityTier.PrecisionOptic, 900);
                var notReady = PrecisionGlassworksOpticsEngine.CalibrateTheodolite(GlassPurityTier.CommonWindow, 900);
                if (ready.IsFieldReady && !notReady.IsFieldReady)
                {
                    GD.Print($"[PASS] Check 7: Theodolite field-ready at {ready.CalibrationAccuracyPermille}\u2030; common glass refused.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 7: Theodolite field-readiness incorrect.");

                // Check 8: minimum purity mapping.
                if (PrecisionGlassworksOpticsEngine.MinimumPurityForVision(VisionCorrectionBand.SevereMyopia) == GlassPurityTier.PrecisionOptic
                    && PrecisionGlassworksOpticsEngine.MinimumPurityForVision(VisionCorrectionBand.NoCorrectionNeeded) == GlassPurityTier.Crude)
                {
                    GD.Print("[PASS] Check 8: Minimum purity mapping matches the authored bands.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 8: Minimum purity mapping incorrect.");

                // Check 9: ledger charges a batch and reports a census.
                var ledger = new GlassworksLedger();
                ledger.AddBatch("hex_1", 980);
                var census0 = ledger.GetCensus();
                ledger.AdvanceAnnealing("hex_1", 900);
                if (census0.BatchCount == 1 && ledger.GetCensus().BatchCount == 1)
                {
                    GD.Print($"[PASS] Check 9: Ledger tracks {census0.BatchCount} batch.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 9: Ledger batch tracking incorrect.");

                // Check 10: grinding consumes grit stock and records a prescription.
                int before = ledger.GritStockPermille;
                ledger.GrindCorrectionLens("hex_1", VisionCorrectionBand.MildMyopia, 900, 42);
                ledger.SetVisionPrescription("survivor_1", VisionCorrectionBand.MildMyopia);
                if (ledger.GritStockPermille < before && ledger.GetVisionPrescription("survivor_1") == VisionCorrectionBand.MildMyopia)
                {
                    GD.Print($"[PASS] Check 10: Grit consumed ({before}\u2030 -> {ledger.GritStockPermille}\u2030) and prescription recorded.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 10: Grit consumption or prescription recording failed.");

                // Check 11: capture/restore round-trip + schema gate.
                var state = ledger.CaptureState();
                var restored = new GlassworksLedger();
                restored.RestoreState(state);
                bool roundTrip = restored.BatchCount == ledger.BatchCount
                    && restored.GetVisionPrescription("survivor_1") == VisionCorrectionBand.MildMyopia;
                bool newerRejected = false;
                try
                {
                    var newer = ledger.CaptureState();
                    newer.SchemaVersion = 99;
                    restored.RestoreState(newer);
                }
                catch (InvalidOperationException) { newerRejected = true; }
                if (roundTrip && newerRejected)
                {
                    GD.Print("[PASS] Check 11: Ledger round-trips and the schema gate rejects newer payloads.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 11: Round-trip/schema gate failed (roundTrip={roundTrip}, newerRejected={newerRejected}).");

                // Check 12: host wiring — day owner + save section.
                string main = ReadRepoFile("src", "Main.Glassworks.cs");
                string owners = ReadRepoFile("src", "Main.CampaignOwners.cs");
                string registry = ReadRepoFile("Assets", "Ashfall.Core", "Save", "SaveSectionRegistry.cs");
                if (main.Contains("TickGlassworks") && owners.Contains("GlassworksDayOwner")
                    && registry.Contains("glassworks") && registry.Contains("glassworks_save.json"))
                {
                    GD.Print("[PASS] Check 12: Host ticks kiln annealing and registers the glassworks section.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 12: Glassworks host wiring or save section missing.");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[EXCEPTION] Exception during glassworks self-test: {ex}");
            }

            GD.Print($"=== Glassworks Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }

        private static GlassPurityTier TierFor(int silicaPermille)
        {
            var batch = new GlassBatchState { BatchId = "tier", SilicaPurityPermille = silicaPermille };
            for (int i = 0; i < PrecisionGlassworksOpticsEngine.MaxAnnealingStages; i++)
                PrecisionGlassworksOpticsEngine.AdvanceAnnealingStage(batch, 900);
            return batch.ResultingTier;
        }

        private static string ReadRepoFile(params string[] parts)
        {
            try
            {
                string dir = AppContext.BaseDirectory;
                for (int i = 0; i < 8 && dir != null; i++)
                {
                    string candidate = Path.Combine(dir, Path.Combine(parts));
                    if (File.Exists(candidate)) return File.ReadAllText(candidate);
                    dir = Directory.GetParent(dir)?.FullName ?? string.Empty;
                }
            }
            catch (Exception) { /* cleanup: optional probe file is unavailable */ }
            return string.Empty;
        }
    }
}
