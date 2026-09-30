// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Plan 253: Voluntary Register — CLI self-test probe.
    /// </summary>
    public static class HostCliVoluntaryRegister
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
                var system = new VoluntaryRegisterSystem();
                var session = new VoluntaryRegisterHostSession(system);

                // 1. Initial state
                Check("initial_empty", system.Entries.Count == 0);

                // 2. Volunteer
                bool volunteered = system.Volunteer("survivor_1", "surface_work", day: 10, "high-dose task");
                Check("volunteer_success", volunteered);
                Check("entries_count_1", system.Entries.Count == 1);

                // 3. Duplicate volunteer (same survivor + task)
                bool duplicate = system.Volunteer("survivor_1", "surface_work", day: 11);
                Check("duplicate_rejected", !duplicate);

                // 4. Complete volunteer
                bool completed = system.CompleteVolunteer("survivor_1", "surface_work", doseIncurred: 50f, day: 15);
                Check("complete_success", completed);
                Check("entry_completed", system.Entries[0].completed);
                Check("entry_dose", system.Entries[0].doseIncurred == 50f);

                // 5. Capture/restore
                var state1 = session.CaptureState();
                Check("capture_state", state1 != null);
                Check("capture_entries", state1?.entries.Count == 1);

                system.RestoreState(new VoluntaryRegisterSystemState());
                Check("reset_clears", system.Entries.Count == 0);

                session.RestoreState(state1);
                Check("restore_entries", system.Entries.Count == 1);

                Console.WriteLine($"\nVoluntary Register self-test: {passed}/{passed + failed}");
                return failed == 0 ? 0 : 1;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[VOLUNTARY_REGISTER] self-test exception: {ex.Message}");
                return 1;
            }
        }
    }
}
