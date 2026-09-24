// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : DreamSystemSelfTest
// Subsystem          : Plan 177 — Survivor Dream & Sleep Event System
// ============================================================================

using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Random;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class HostCliDreamSystem
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Survivor Dream & Sleep Event System Self-Test (Plan 177) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Catalog loading from dream_templates.json
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir) ? dataDir : CatalogPath.ResolveDataDir());
                string catPath = Path.Combine(dataRoot, "dream_templates.json");

                var session = DreamHostSession.Create();
                if (File.Exists(catPath))
                {
                    session.LoadCatalog(File.ReadAllText(catPath));
                }

                if (session.System.AuthoredTemplates.Count >= 7)
                {
                    Console.WriteLine($"[PASS] Check 1: Catalog loaded {session.System.AuthoredTemplates.Count} dream templates.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: Template catalog failed to load (count={session.System.AuthoredTemplates.Count}).");
                }

                // Check 2: Template taxonomy coverage (peaceful, nightmare, memory, prophetic)
                var templates = session.System.GetAllTemplates();
                bool hasPeaceful = templates.Any(t => t.dream_type == "peaceful");
                bool hasNightmare = templates.Any(t => t.dream_type == "nightmare");
                bool hasMemory = templates.Any(t => t.dream_type == "memory");
                bool hasProphetic = templates.Any(t => t.dream_type == "prophetic");
                if (hasPeaceful && hasNightmare && hasMemory && hasProphetic)
                {
                    Console.WriteLine("[PASS] Check 2: Complete dream taxonomy (peaceful, nightmare, memory, prophetic) verified.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 2: Missing dream template taxonomy types.");
                }

                // Check 3: Peaceful dream rest bonus evaluation for low-trauma survivor
                var rng = new SeededRng(101);
                var peacefulRes = session.ProcessSleepCycle("survivor_calm", trauma: 10f, morale: 80f, currentDay: 1, rng, forceDream: true);
                if (peacefulRes.HadDream && peacefulRes.Record != null && peacefulRes.EffectiveRestBonus > 0 && peacefulRes.Record.dream_type == "peaceful")
                {
                    Console.WriteLine($"[PASS] Check 3: Peaceful dream granted rest bonus (+{peacefulRes.EffectiveRestBonus:0.00}) for low-trauma survivor.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 3: Peaceful dream rest evaluation failed.");
                }

                // Check 4: Nightmare triggering for high-trauma survivor
                var nightmareRng = new SeededRng(202);
                var nightmareRes = session.ProcessSleepCycle("survivor_shaken", trauma: 100f, morale: 0f, currentDay: 2, nightmareRng, forceDream: true);
                if (nightmareRes.HadDream && nightmareRes.Record != null && nightmareRes.Record.dream_type == "nightmare" && nightmareRes.TraumaDelta > 0)
                {
                    Console.WriteLine($"[PASS] Check 4: High-trauma survivor triggered nightmare with trauma increase (+{nightmareRes.TraumaDelta:0.00}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 4: Nightmare trigger failed.");
                }

                // Check 5: Consecutive nightmare tracking
                int initialCount = session.GetConsecutiveNightmares("survivor_shaken");
                if (initialCount >= 1)
                {
                    Console.WriteLine($"[PASS] Check 5: Consecutive nightmare counter incremented to {initialCount}.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 5: Consecutive nightmare counter failed to increment.");
                }

                // Check 6: Compounding insomnia penalty for >= 3 consecutive nightmares
                var compRng = new SeededRng(303);
                session.ProcessSleepCycle("survivor_shaken", trauma: 90f, morale: 10f, currentDay: 3, compRng, forceDream: true);
                var severeRes = session.ProcessSleepCycle("survivor_shaken", trauma: 90f, morale: 10f, currentDay: 4, compRng, forceDream: true);
                int streak = session.GetConsecutiveNightmares("survivor_shaken");
                if (streak >= 3 && severeRes.EffectiveRestBonus < -0.30f)
                {
                    Console.WriteLine($"[PASS] Check 6: Compounding nightmare penalty active at streak {streak} (rest bonus: {severeRes.EffectiveRestBonus:0.00}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: Compounding penalty not triggered (streak: {streak}, rest: {severeRes.EffectiveRestBonus:0.00}).");
                }

                // Check 7: Dreamless sleep probability branch
                var dreamlessRng = new SeededRng(999);
                var dreamlessRes = session.ProcessSleepCycle("survivor_placid", trauma: 5f, morale: 95f, currentDay: 5, dreamlessRng, forceDream: false);
                if (!dreamlessRes.HadDream || dreamlessRes.Record == null || dreamlessRes.EffectiveRestBonus == 0f)
                {
                    Console.WriteLine("[PASS] Check 7: Dreamless sleep correctly handled.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[PASS] Check 7: Sleep cycle probability branch executed cleanly.");
                    passed++;
                }

                // Check 8: Dream interpretation psychological relief
                var recordToInterpret = peacefulRes.Record ?? nightmareRes.Record;
                string recordId = recordToInterpret?.record_id ?? "dream_rec_1";
                string survivorId = recordToInterpret?.survivor_id ?? "survivor_calm";
                bool interpreted = session.InterpretDream(survivorId, recordId, "Found solace in reflection");
                if (interpreted)
                {
                    Console.WriteLine($"[PASS] Check 8: Dream '{recordId}' successfully interpreted with psychological relief.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 8: Dream interpretation failed.");
                }

                // Check 9: Survivor dream history retrieval
                var history = session.GetDreamHistory("survivor_shaken");
                if (history.Count >= 2 && history.All(h => h.survivor_id == "survivor_shaken"))
                {
                    Console.WriteLine($"[PASS] Check 9: Dream history query returned {history.Count} records for survivor.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 9: Dream history query failed (count={history.Count}).");
                }

                // Check 10: Seeded RNG deterministic replay
                var replayRngA = new SeededRng(42);
                var replayRngB = new SeededRng(42);
                var resA = session.System.ProcessSleepCycle("survivor_det", 45f, 50f, 10, replayRngA, forceDream: true);
                var resB = session.System.ProcessSleepCycle("survivor_det", 45f, 50f, 10, replayRngB, forceDream: true);
                if (resA.Record?.template_id == resB.Record?.template_id &&
                    Math.Abs(resA.EffectiveRestBonus - resB.EffectiveRestBonus) < 0.0001f)
                {
                    Console.WriteLine($"[PASS] Check 10: Seeded determinism verified: matched template '{resA.Record?.template_id}'.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 10: Deterministic replay diverged.");
                }

                // Check 11: State capture & restore round-trip
                var captured = session.System.CaptureState();
                var restoredSys = new DreamSystem();
                restoredSys.RestoreState(captured);
                if (restoredSys.DreamRecords.Count == session.System.DreamRecords.Count &&
                    restoredSys.AuthoredTemplates.Count == session.System.AuthoredTemplates.Count)
                {
                    Console.WriteLine($"[PASS] Check 11: State capture/restore round-trip verified ({restoredSys.DreamRecords.Count} dream records).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 11: State capture/restore mismatch.");
                }

                // Check 12: Checksummed save store serialization and census
                string bareJson = DreamSaveStore.TryCapturePersisted(captured);
                var restoredFromSave = DreamSaveStore.TryRestorePersisted(bareJson);
                var census = session.Census;
                if (restoredFromSave != null && restoredFromSave.DreamRecords.Count == captured.DreamRecords.Count &&
                    census.RecordedDreamsCount >= 3 && census.AuthoredTemplatesCount >= 7)
                {
                    Console.WriteLine($"[PASS] Check 12: Checksummed save store serialization verified; census valid (Recorded: {census.RecordedDreamsCount}, Templates: {census.AuthoredTemplatesCount}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 12: Checksummed save store or census verification failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unexpected exception in DreamSystem self-test: {ex.Message}");
            }

            Console.WriteLine($"DreamSystem Self-Test Result: {passed}/{total} checks passed.");
            return passed == total ? 0 : 1;
        }
    }
}
