// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Plan 227: World Evolution — CLI self-test probe.
    /// </summary>
    public static class HostCliWorldEvolution
    {
        public static int RunSelfTest()
        {
            int passed = 0;
            int failed = 0;

            void Check(string name, bool condition)
            {
                if (condition) { passed++; Console.WriteLine($"[PASS] {name}"); }
                else { failed++; Console.WriteLine($"[FAIL] {name}"); }
            }

            try
            {
                string dataDir = System.IO.Path.Combine("Assets", "StreamingAssets", "Data");
                var engine = new WorldEvolutionEngine(dataDir);
                var session = new WorldEvolutionHostSession(dataDir, engine);

                // 1. Authored catalog reached the engine
                Check("catalog_loaded", engine.Events.Count >= 10);
                Check("no_events_triggered", engine.TriggeredEventIds.Count == 0);

                // 2. Tick to the authored trigger day of the first UNGATED event.
                // (The earliest authored event is flag-gated, so it must NOT
                // fire from an empty flag set — the flag is the gate.)
                int firstUngatedDay = int.MaxValue;
                foreach (var evt in engine.Events)
                    if (string.IsNullOrEmpty(evt.required_flag))
                        firstUngatedDay = Math.Min(firstUngatedDay, evt.trigger_day);
                Check("authored_ungated_day_found", firstUngatedDay < int.MaxValue);

                session.Tick(firstUngatedDay, new HashSet<string>(StringComparer.OrdinalIgnoreCase), null);
                Check("event_triggered", engine.TriggeredEventIds.Count >= 1);

                // 3. Capture/restore is exact and does not re-trigger
                var state = session.CaptureState();
                Check("capture_state", state != null);
                int triggeredCount = engine.TriggeredEventIds.Count;

                session.RestoreState(state, null);
                Check("restore_triggered_count", engine.TriggeredEventIds.Count == triggeredCount);

                session.Tick(firstUngatedDay + 1, new HashSet<string>(StringComparer.OrdinalIgnoreCase), null);
                Check("no_duplicate_trigger", engine.TriggeredEventIds.Count == triggeredCount);

                // 4. Reset clears triggered events
                session.Reset();
                Check("reset_clears", engine.TriggeredEventIds.Count == 0);

                Console.WriteLine($"\nWorld Evolution self-test: {passed}/{passed + failed}");
                return failed == 0 ? 0 : 1;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[WORLD_EVOLUTION] self-test exception: {ex.Message}");
                return 1;
            }
        }
    }
}
