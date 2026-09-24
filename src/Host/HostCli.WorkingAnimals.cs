// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Partial : WorkingAnimals
// Subsystem         : Plan 151 / 174 — Working Animals & Companion System
// ============================================================================

namespace AtomicWar.GodotApp
{
    public static class HostCliWorkingAnimals
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            return WorkingAnimalsSelfTest.RunSelfTest(dataDir);
        }
    }
}
