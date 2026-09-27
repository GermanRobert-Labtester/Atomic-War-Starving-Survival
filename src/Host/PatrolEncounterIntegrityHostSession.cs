// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Patrol / travel encounter integrity host binding.
//
// PatrolEncounterValidator shipped as a 336-line authored rule set with no
// consumer at all, so the same travel_encounters.json the live expedition
// encounter owner plays from was never checked for duplicate ids, dangling
// faction / item references, malformed weights, or unwired choices.
// The validator only reports: no row is dropped, edited, or invented.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp
{
    public sealed class PatrolEncounterIntegrityHostSession : HostSessionBase
    {
        private readonly Func<TravelEncounterCatalog?> _catalogProvider;
        private readonly Func<ISet<string>?> _factionIdsProvider;
        private readonly Func<ISet<string>?> _itemIdsProvider;

        private List<string> _errors = new List<string>();

        public string LastEvent { get; private set; } = string.Empty;
        public IReadOnlyList<string> Errors => _errors;
        public int ErrorCount => _errors.Count;
        public bool IsClean => _errors.Count == 0;
        public int ValidationCount { get; private set; }
        public int EncountersValidated { get; private set; }

        /// <summary>Rows the validator actually inspected (it is patrol-specific by design).</summary>
        public int PatrolRowsValidated { get; private set; }

        public PatrolEncounterIntegrityHostSession(
            Func<TravelEncounterCatalog?> catalogProvider,
            Func<ISet<string>?>? factionIdsProvider = null,
            Func<ISet<string>?>? itemIdsProvider = null)
        {
            _catalogProvider = catalogProvider ?? throw new ArgumentNullException(nameof(catalogProvider));
            _factionIdsProvider = factionIdsProvider ?? (() => null);
            _itemIdsProvider = itemIdsProvider ?? (() => null);
        }

        /// <summary>
        /// Validates the live catalog against the real faction and item reference
        /// sets. Deterministic and read-only.
        /// </summary>
        public IReadOnlyList<string> Validate()
        {
            var catalog = _catalogProvider();
            if (catalog == null || catalog.Count == 0)
            {
                _errors = new List<string>();
                EncountersValidated = 0;
                LastEvent = "No travel encounter catalog bound; nothing validated.";
                RaiseStateChanged();
                return _errors;
            }

            var errors = PatrolEncounterValidator.Validate(
                catalog.Encounters, _factionIdsProvider(), _itemIdsProvider())
                ?? new List<string>();
            // Authored messages, sorted so a report can be diffed run to run.
            errors.Sort(StringComparer.Ordinal);
            _errors = errors;
            EncountersValidated = catalog.Count;
            // The validator is patrol-specialised and skips other rows by design;
            // report the inspected subset so the number is never over-claimed.
            int patrols = 0;
            foreach (var e in catalog.Encounters)
                if (e != null && e.Id != null && e.Id.StartsWith("enc_patrol_", StringComparison.OrdinalIgnoreCase)) patrols++;
            PatrolRowsValidated = patrols;
            ValidationCount++;
            LastEvent = errors.Count == 0
                ? $"Travel encounter integrity clean across {patrols} patrol row(s) ({catalog.Count} rows seen)."
                : $"Travel encounter integrity: {errors.Count} error(s) across {patrols} patrol row(s).";
            RaiseStateChanged();
            return _errors;
        }

        public bool HasErrorContaining(string fragment)
        {
            foreach (var e in _errors)
                if (e != null && e.Contains(fragment, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        public string StatusLine() =>
            $"encounter integrity: {(IsClean ? "clean" : $"{ErrorCount} error(s)")}"
            + $" · {PatrolRowsValidated}/{EncountersValidated} patrol row(s)";
    }
}
