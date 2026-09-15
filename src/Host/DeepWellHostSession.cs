// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// B5–B8 Phase 6: thin host session over <see cref="DeepWellSystem"/> —
    /// builds (capability + canonical bill checked by the Main route),
    /// maintenance, enable/disable, daily tick, save capture. The session
    /// never mutates water or power state directly; it only drives the Core
    /// system's own seam methods.
    /// </summary>
    public sealed class DeepWellHostSession
    {
        public DeepWellSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public bool IsDirty { get; private set; }

        public DeepWellHostSession(DeepWellSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnStateChanged += _ => { IsDirty = true; };
        }

        /// <summary>
        /// Build the well. The Main route has already verified the live
        /// capability query and consumed the canonical build bill — this only
        /// commits the physical state.
        /// </summary>
        public bool TryBuild(bool hasRequiredCapability, out string reason)
        {
            bool ok = System.TryBuild(hasRequiredCapability, out reason);
            if (ok)
            {
                LastEvent = "Deep-well pump installed. Registered as a critical grid load.";
                IsDirty = true;
            }
            else
            {
                LastEvent = "Deep-well build blocked: " + reason;
            }
            return ok;
        }

        public bool SetEnabled(bool enabled, out string reason)
        {
            bool ok = System.SetEnabled(enabled, out reason);
            if (ok)
            {
                LastEvent = enabled ? "Deep-well pump engaged." : "Deep-well pump disengaged.";
                IsDirty = true;
            }
            return ok;
        }

        /// <summary>Caller consumes the canonical maintenance item first.</summary>
        public bool PerformMaintenance(out string reason)
        {
            bool ok = System.PerformMaintenance(out reason);
            if (ok)
            {
                LastEvent = "Deep-well pump serviced to full condition.";
                IsDirty = true;
            }
            return ok;
        }

        public void TickDay(int day) => System.TickDay(day);

        public void ClearDirty() => IsDirty = false;
    }
}
