// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Authored Guilt Sources — host adapter over the ALREADY-LIVE
// Ashfall.Core.Survivors.GuiltInsomniaSystem.
//
// GuiltInsomniaSystem remains the sole guilt/insomnia authority. This adapter
// adds only what was missing: the authored guilt_sources.json table, so a
// choice pattern resolves to its authored severity and description instead of a
// float hardcoded at the call site.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Read-only host adapter that binds <see cref="GuiltSourceCatalog"/> to the
    /// live <see cref="GuiltInsomniaSystem"/>. No state of its own — guilt records
    /// persist through the Phase-0 aggregate that already owns them.
    /// </summary>
    public sealed class GuiltSourceHostSession : HostSessionBase
    {
        private readonly Func<GuiltInsomniaSystem?> _guiltProvider;
        private readonly Func<int> _dayProvider;
        private GuiltSourceCatalog _catalog = new GuiltSourceCatalog();

        public string LastEvent { get; private set; } = string.Empty;
        public GuiltSourceCatalog Catalog => _catalog;
        public int SourceCount => _catalog.Count;
        public bool IsBound => _catalog.Count > 0;

        public GuiltSourceHostSession(
            Func<GuiltInsomniaSystem?> guiltProvider,
            Func<int> dayProvider)
        {
            _guiltProvider = guiltProvider ?? throw new ArgumentNullException(nameof(guiltProvider));
            _dayProvider = dayProvider ?? throw new ArgumentNullException(nameof(dayProvider));
        }

        /// <summary>Loads the authored guilt-source table. Returns the row count.</summary>
        public int LoadCatalog(string dataDir)
        {
            _catalog = GuiltSourceCatalog.LoadFromDirectory(dataDir);
            LastEvent = _catalog.Count > 0
                ? $"Loaded {_catalog.Count} authored guilt source(s)."
                : "No authored guilt source catalog found.";
            RaiseStateChanged();
            return _catalog.Count;
        }

        public GuiltSourceDefinition? Resolve(string choicePattern) => _catalog.GetByPattern(choicePattern);

        /// <summary>
        /// Records guilt for a resolved choice pattern using the AUTHORED severity.
        /// An unknown pattern is refused without touching the live owner — the host
        /// never invents a number to keep a call site working.
        /// </summary>
        public bool RecordGuiltFromChoice(string survivorId, string choicePattern)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) return Refuse("missing_survivor_id");
            var def = _catalog.GetByPattern(choicePattern);
            if (def == null) return Refuse($"unauthored_guilt_source:{choicePattern}");

            var guilt = _guiltProvider();
            if (guilt == null) return Refuse("guilt_owner_unavailable");

            guilt.RecordGuilt(survivorId, choicePattern, def.Severity, _dayProvider());
            LastEvent = $"{survivorId} carries '{def.Title}' (severity {def.Severity:0.00}).";
            RaiseStateChanged();
            return true;
        }

        /// <summary>
        /// Authored description for a survivor, or empty when the pattern is not
        /// authored. Never falls back to a generic string.
        /// </summary>
        public string Describe(string survivorId, string choicePattern, string? survivorName = null)
        {
            var def = _catalog.GetByPattern(choicePattern);
            if (def == null) return string.Empty;
            return def.FormatDescription(string.IsNullOrEmpty(survivorName) ? survivorId : survivorName);
        }

        /// <summary>Every authored severity, ordinal-sorted by pattern (deterministic).</summary>
        public IReadOnlyList<(string Pattern, float Severity, string Title)> AuthoredSeverities()
        {
            var rows = new List<(string, float, string)>();
            foreach (var item in _catalog.Items)
            {
                if (item == null || string.IsNullOrEmpty(item.ChoicePattern)) continue;
                rows.Add((item.ChoicePattern, item.Severity, item.Title));
            }
            rows.Sort((a, b) => string.CompareOrdinal(a.Item1, b.Item1));
            return rows;
        }

        private bool Refuse(string reason)
        {
            LastEvent = $"Guilt recording refused: {reason}";
            return false;
        }

        /// <summary>Truthful projection for a status readout.</summary>
        public string StatusLine() =>
            _catalog.Count > 0 ? $"{_catalog.Count} authored guilt source(s)" : "guilt catalog unbound";
    }
}
