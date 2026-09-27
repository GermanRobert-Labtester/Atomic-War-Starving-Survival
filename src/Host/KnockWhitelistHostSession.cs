// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : KnockWhitelistHostSession
// Purpose      : PLAN-KNOCK-WHITELIST-TRUTH-155 — load the authored orphan-knock
//                whitelist and expose deliberate/gated door-arrival validation.
//                Derived from authored data; no save section.
// ============================================================================
using System.IO;
using Ashfall.Core.Encounters;
using Ashfall.Core.Flags;

namespace AtomicWar.GodotApp
{
    public sealed class KnockWhitelistHostSession
    {
        public OrphanKnockWhitelist Whitelist { get; private set; } = new(null);
        public int Count => Whitelist.Count;
        public string LastEvent { get; private set; } = string.Empty;

        public void Load(string dataRoot)
        {
            string path = Path.Combine(dataRoot, "whitelists", "orphan_knocks.json");
            Whitelist = File.Exists(path)
                ? OrphanKnockWhitelist.LoadFromJson(File.ReadAllText(path))
                : new OrphanKnockWhitelist(null);
            LastEvent = $"Loaded {Whitelist.Count} deliberate orphan knock(s).";
        }

        public bool Contains(string knockId) => Whitelist.ContainsKnock(knockId);

        public bool Validate(string eventName, IFlagLedger flags, out string diagnostic)
        {
            bool ok = Whitelist.ValidateOrphan(eventName, flags, out diagnostic);
            LastEvent = diagnostic;
            return ok;
        }
    }
}
