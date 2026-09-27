// SPDX-License-Identifier: MIT
using System;
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
                // Coordination repair 2026-09-27 (rumor/memorial seal session):
                // mapped the draft's guessed API to the live Core API —
                // engine ctor is (dataDir, …) and loads its authored catalog;
                // state truth is lastEvaluatedDay / triggeredEventIds.
                var session = new WorldEvolutionHostSession();

                // 1. Initial state
                Check("initial_state", session.State.triggeredEventIds.Count == 0);
                Check("initial_day", session.State.lastEvaluatedDay == -1);

                // 2. Tick
                session.Tick(1);
                Check("tick_advances", session.State.lastEvaluatedDay == 1);

                // 3. Capture/restore
                var state1 = session.CaptureState();
                Check("capture_state", state1 != null);
                Check("capture_day", state1 != null && state1.lastEvaluatedDay == 1);

                session.Tick(2);
                Check("tick_advances_2", session.State.lastEvaluatedDay == 2);

                session.RestoreState(state1!);
                Check("restore_day", session.State.lastEvaluatedDay == 1);

                // 4. Reset
                session.Reset();
                Check("reset_clears", session.State.lastEvaluatedDay == -1);

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
