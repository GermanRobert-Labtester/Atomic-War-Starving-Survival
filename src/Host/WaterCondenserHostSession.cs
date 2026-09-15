// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// B5–B8 expansion: thin host session over <see cref="AtmosphericCondenserSystem"/>
    /// — build (capability + canonical bill checked by the Main route),
    /// membrane replacement, enable/disable, daily tick, save capture.
    /// </summary>
    public sealed class WaterCondenserHostSession
    {
        public AtmosphericCondenserSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public bool IsDirty { get; private set; }

        public WaterCondenserHostSession(AtmosphericCondenserSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnStateChanged += _ => { IsDirty = true; };
        }

        public bool TryBuild(bool hasRequiredCapability, out string reason)
        {
            bool ok = System.TryBuild(hasRequiredCapability, out reason);
            if (ok)
            {
                LastEvent = "Peltier condensation array installed. Registered as a grid load.";
                IsDirty = true;
            }
            else
            {
                LastEvent = "Condenser build blocked: " + reason;
            }
            return ok;
        }

        public bool SetEnabled(bool enabled, out string reason)
        {
            bool ok = System.SetEnabled(enabled, out reason);
            if (ok)
            {
                LastEvent = enabled ? "Condenser array engaged." : "Condenser array disengaged.";
                IsDirty = true;
            }
            return ok;
        }

        /// <summary>Caller consumes the canonical membrane item first.</summary>
        public bool ReplaceMembrane(out string reason)
        {
            bool ok = System.ReplaceMembrane(out reason);
            if (ok)
            {
                LastEvent = "Condensation membrane replaced to full integrity.";
                IsDirty = true;
            }
            return ok;
        }

        public void TickDay(int day) => System.TickDay(day);

        public void ClearDirty() => IsDirty = false;
    }
}
