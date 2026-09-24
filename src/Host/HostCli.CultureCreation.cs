// SPDX-License-Identifier: MIT
using System;

namespace AtomicWar.GodotApp
{
    public static class HostCliCultureCreation
    {
        public static int RunSelfTest(string dataDir)
        {
            return CultureCreationSelfTest.Run(dataDir);
        }
    }
}
