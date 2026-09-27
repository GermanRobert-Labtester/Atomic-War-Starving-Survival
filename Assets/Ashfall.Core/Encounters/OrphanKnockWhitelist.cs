// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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

        /// <summary>Number of authored deliberate orphan knocks.</summary>
        public int Count => _entries.Count;

        /// <summary>Look up an authored entry by knock id (null when absent).</summary>
        public OrphanKnockEntry? GetEntry(string knockId) =>
            _entries.TryGetValue(knockId, out var entry) ? entry : null;

        private sealed class WhitelistDto
        {
            public int schema_version { get; set; }
            public List<WhitelistEntryDto> orphan_knocks { get; set; } = new List<WhitelistEntryDto>();
        }

        private sealed class WhitelistEntryDto
        {
            public string knock_id { get; set; } = string.Empty;
            public string event_name { get; set; } = string.Empty;
            public string gating_flag { get; set; } = string.Empty;
            public string mystery_thread_id { get; set; } = string.Empty;
            public string resolution_expansion { get; set; } = string.Empty;
        }

        /// <summary>PLAN-KNOCK-WHITELIST-TRUTH-155 — strict authored-loader for
        /// <c>Data/whitelists/orphan_knocks.json</c>. Returns an empty whitelist
        /// on malformed input rather than a half-parsed one.</summary>
        public static OrphanKnockWhitelist LoadFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return new OrphanKnockWhitelist(null);
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var dto = JsonSerializer.Deserialize<WhitelistDto>(json, options);
                var entries = new List<OrphanKnockEntry>();
                if (dto?.orphan_knocks != null)
                {
                    foreach (var e in dto.orphan_knocks)
                    {
                        if (string.IsNullOrWhiteSpace(e.knock_id) || string.IsNullOrWhiteSpace(e.event_name))
                            continue;
                        entries.Add(new OrphanKnockEntry(
                            e.knock_id.Trim(), e.event_name.Trim(), e.gating_flag ?? string.Empty,
                            e.mystery_thread_id ?? string.Empty, e.resolution_expansion ?? string.Empty));
                    }
                }
                return new OrphanKnockWhitelist(entries);
            }
            catch (JsonException)
            {
                return new OrphanKnockWhitelist(null);
            }
        }
    }
}
