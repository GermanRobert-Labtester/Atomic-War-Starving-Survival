// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : PalliativeCareSelfTest
// Subsystem          : Palliative Care & Dignity (Expansion 24 — The Long Goodbye)
// ============================================================================

using System;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    public static class HostCliPalliativeCare
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Palliative Care Self-Test ===");
            int passed = 0;
            const int total = 7;
            const int Seed = 1337;

            try
            {
                var session = PalliativeCareHostSession.Create();

                if (session.PatientCount == 0 && session.MemorialEchoes.Count == 0)
                {
                    Console.WriteLine("[PASS] Check 1: Palliative ward starts empty.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 1: Ward was not empty.");
                }

                var patient = session.AdmitPatient("surv_a", 3, PalliativeCareProtocol.BalancedAnalgesia);
                if (patient != null && session.PatientCount == 1)
                {
                    Console.WriteLine("[PASS] Check 2: Terminal survivor admitted to the ward.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 2: Admission failed.");
                }

                // No medicine + no skill: dignity must decay, not silently stay flat.
                session.AdmitPatient("surv_b", 5, PalliativeCareProtocol.MinimalSedation);
                int dignityBefore = session.FindPatient("surv_a")!.DignityIndexPermille;
                session.AdvanceDay(1, 0, 0, Seed);
                int dignityAfter = session.FindPatient("surv_a")!.DignityIndexPermille;
                if (dignityAfter < dignityBefore)
                {
                    Console.WriteLine($"[PASS] Check 3: Without supplies dignity decays ({dignityBefore} -> {dignityAfter}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 3: Dignity did not decay ({dignityBefore} -> {dignityAfter}).");
                }

                // Full medicine + full caregiver skill must recover dignity.
                session.AdvanceDay(2, 1000, 1000, Seed);
                int dignityRecovered = session.FindPatient("surv_a")!.DignityIndexPermille;
                if (dignityRecovered > dignityAfter)
                {
                    Console.WriteLine($"[PASS] Check 4: Full care recovers dignity ({dignityAfter} -> {dignityRecovered}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 4: Dignity did not recover under full care.");
                }

                // Deep symptom control relieves pain fastest but costs lucidity.
                session.SetProtocol("surv_a", PalliativeCareProtocol.DeepSymptomControl);
                int painBefore = session.FindPatient("surv_a")!.PainLevelPermille;
                session.AdvanceDay(3, 1000, 1000, Seed);
                int painAfter = session.FindPatient("surv_a")!.PainLevelPermille;
                if (painAfter < painBefore)
                {
                    Console.WriteLine($"[PASS] Check 5: Deep symptom control relieves pain ({painBefore} -> {painAfter}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 5: Pain was not relieved ({painBefore} -> {painAfter}).");
                }

                // Final wish + sustained care should reach the high-dignity threshold.
                session.FulfillFinalWish("surv_a", "quest_final_wish");
                for (int d = 4; d <= 20; d++) session.AdvanceDay(d, 1000, 1000, Seed);
                var g = session.FindPatient("surv_a")!.CurrentGriefStage;
                if (g == GriefStage.Acceptance && session.HighDignityPatientCount >= 1)
                {
                    Console.WriteLine("[PASS] Check 6: Sustained care reaches Acceptance and high dignity.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: Grief stage {g}, high-dignity patients {session.HighDignityPatientCount}.");
                }

                var echo = session.RecordPassing("surv_a");
                bool saved = session.TrySave();
                var reloaded = PalliativeCareHostSession.Create();
                bool loaded = reloaded.TryLoad();
                if (echo.DiedInDignity && saved && loaded
                    && reloaded.MemorialEchoes.Count == 1 && reloaded.PatientCount == session.PatientCount
                    && PalliativeCareSaveStore.SectionName.Equals("palliative_care", StringComparison.Ordinal)
                    && PalliativeCareSaveStore.FileName.Equals("palliative_care_save.json", StringComparison.Ordinal))
                {
                    Console.WriteLine("[PASS] Check 7: Dignified death, memorial echo, and save/restore verified.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 7: echo={echo.DiedInDignity}, saved={saved}, loaded={loaded}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Self-test crashed: {ex.Message}");
            }

            Console.WriteLine($"=== Palliative Care Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
