// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Partial : RadiationMutation
// Subsystem         : Plan 172 — Radiation Mutation & Genetic Instability
// ============================================================================

namespace AtomicWar.GodotApp
{
    public static class HostCliRadiationMutation
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            return RadiationMutationSelfTest.RunSelfTest(dataDir);
        }
    }
}
