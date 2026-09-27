// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Mercenary System — CLI self-test probe.
    /// </summary>
    public static class HostCliMercenary
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
                // MercenarySystem(rng, inventory); day truth is
                // State.lastGenerationDay (advanced by GenerateBoard); the
                // contract list is system.ActiveContracts.
                var system = new MercenarySystem(
                    new SeededRng(123), new Ashfall.Core.Inventory.Inventory());
                var session = new MercenaryHostSession(system);

                // 1. Initial state
                Check("initial_state", system.ActiveContracts.Count == 0);
                Check("initial_day", system.State.lastGenerationDay == -1);

                // 2. Tick
                system.GenerateBoard(1, Array.Empty<string>());
                Check("tick_advances", system.State.lastGenerationDay == 1);

                // 3. Capture/restore
                var state1 = session.CaptureState();
                Check("capture_state", state1 != null);
                Check("capture_day", state1 != null && state1.lastGenerationDay == 1);

                system.GenerateBoard(2, Array.Empty<string>());
                Check("tick_advances_2", system.State.lastGenerationDay == 2);

                session.RestoreState(state1!);
                Check("restore_day", system.State.lastGenerationDay == 1);

                // 4. Reset
                session.Reset();
                Check("reset_clears", system.State.lastGenerationDay == -1);

                Console.WriteLine($"\nMercenary System self-test: {passed}/{passed + failed}");
                return failed == 0 ? 0 : 1;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[MERCENARY] self-test exception: {ex.Message}");
                return 1;
            }
        }
    }
}
