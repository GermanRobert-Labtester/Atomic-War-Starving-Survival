// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Partial : BlackMarket
// Subsystem         : Plan 155 / 211 — Black Market & Underground Economy
// ============================================================================

namespace AtomicWar.GodotApp
{
    public static class HostCliBlackMarket
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            return BlackMarketSelfTest.RunSelfTest(dataDir);
        }
    }
}
