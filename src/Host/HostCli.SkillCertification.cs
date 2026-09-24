// SPDX-License-Identifier: MIT
using System;

namespace AtomicWar.GodotApp
{
    public static class HostCliSkillCertification
    {
        public static int RunSelfTest(string dataDir)
        {
            return SkillCertificationSelfTest.Run(dataDir);
        }
    }
}
