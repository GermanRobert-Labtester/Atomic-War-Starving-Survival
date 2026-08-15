using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Flags;

namespace Ashfall.Core.Encounters
{
    /// <summary>
    /// Registry of door events deliberately fired with visitor_id: null.
    /// Loaded from Data/whitelists/orphan_knocks.json. Every entry must carry a
    /// mystery_web thread id so the unresolved event is tracked canonically,
    /// never accidentally. The CatalogIntegrityValidator consults this before
    /// raising an orphan-visitor violation.
    /// </summary>
    public sealed record OrphanKnockEntry(
        string knock_id,              // e.g. knock_exp07_vel_vigil
        string event_name,            // e.g. door.knock.practiced
        string gating_flag,           // the flag that must be set when it fires
        string mystery_thread_id,     // e.g. q_vel_last_knock — where it is paid off
        string resolution_expansion   // e.g. exp_12 — when the thread closes
    );

    public sealed class OrphanKnockWhitelist
    {
        private readonly Dictionary<string, OrphanKnockEntry> _entries;

        public OrphanKnockWhitelist(IEnumerable<OrphanKnockEntry> entries)
        {
            _entries = entries != null
                ? entries.ToDictionary(e => e.knock_id, e => e, StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, OrphanKnockEntry>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Called by CatalogIntegrityValidator. Returns true (validated)
        /// only if the orphan knock is deliberate, gated, and canonically tracked.
        /// </summary>
        public bool ValidateOrphan(string eventName, IFlagLedger flags, out string diagnostic)
        {
            if (flags != null)
            {
                foreach (var entry in _entries.Values)
                {
                    if (string.Equals(entry.event_name, eventName, StringComparison.OrdinalIgnoreCase)
                        && flags.IsSet(entry.gating_flag))
                    {
                        diagnostic = $"orphan_validated:{entry.knock_id}→{entry.mystery_thread_id}(resolves in {entry.resolution_expansion})";
                        return true;
                    }
                }
            }

            diagnostic = $"orphan_rejected:{eventName} — no whitelist entry or gating flag set. If deliberate, register in whitelists/orphan_knocks.json with a mystery thread.";
            return false;
        }

        public bool ContainsKnock(string knockId)
        {
            return _entries.ContainsKey(knockId);
        }
    }
}
