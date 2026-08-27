using System;
using Ashfall.Core;

namespace Ashfall.Core.Maritime
{
    /// <summary>
    /// ASHFALL: THE BLACK FLOTILLA (Expansion 09) — Stealth Dive Instance.
    /// Thin legacy subclass maintaining 100% binary and source compatibility
    /// with all existing callers while delegating directly to the authoritative MaritimeDiveSystem.
    /// </summary>
    public sealed class StealthDiveInstance : MaritimeDiveSystem
    {
        public StealthDiveInstance(ISeededRng? rng = null, ILog? log = null)
            : base(rng, log)
        {
        }
    }
}
