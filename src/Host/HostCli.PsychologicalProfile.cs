// SPDX-License-Identifier: MIT
using System;

namespace AtomicWar.GodotApp
{
    public static class HostCliPsychologicalProfile
    {
        public static int RunSelfTest(string dataDir)
        {
            return PsychologicalProfileSelfTest.Run(dataDir);
        }
    }
}
