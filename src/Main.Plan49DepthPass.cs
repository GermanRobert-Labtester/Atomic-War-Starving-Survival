// SPDX-License-Identifier: MIT
// ASHFALL Plan 49 (C1[16]) — depth passes: the four remaining orphaned
// authored catalogs are bound through their existing Core loaders into
// constructed owners at campaign setup. Authored data, no mutable state, no
// save section; re-derived from the data authority on every setup.

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private Plan49DepthPassHostSession? _depthPass;

        public Plan49DepthPassHostSession? DepthPass => _depthPass;

        public void SetupPlan49DepthPass()
        {
            if (_depthPass != null) return;
            _depthPass = Plan49DepthPassHostSession.Create(_dataDir);
        }

        public void ResetPlan49DepthPass()
        {
            _depthPass = null;
        }
    }
}
