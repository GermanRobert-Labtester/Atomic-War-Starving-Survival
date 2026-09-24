// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Partial : RadioProduction
// Subsystem         : Plan 173 — Radio Program Production & Audience Response
// ============================================================================

namespace AtomicWar.GodotApp
{
    public static class HostCliRadioProduction
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            return RadioProgramProductionSelfTest.RunSelfTest(dataDir);
        }
    }
}
