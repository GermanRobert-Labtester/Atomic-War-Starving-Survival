// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 51 — Holdfast presentation slate in the running game.
//
// The slate is a derived read model: no save section, no parallel state. Each
// refresh composes it from the current canonical owners (assignment, power,
// thermal, sump, roster, duty, memorial, wasteland map) and publishes it to the
// shelter interior view, which is the player-observable surface.
// ============================================================================
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Presentation;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private HoldfastPresentationHostSession? _presentation;
        private bool _presentationDirty;

        public HoldfastPresentationHostSession? Presentation => _presentation;

        private HoldfastPresentationHostSession EnsurePresentation()
        {
            if (_presentation != null) return _presentation;
            _presentation = new HoldfastPresentationHostSession();
            return _presentation;
        }

        /// <summary>
        /// Compose and publish the current slate. Owners that do not exist yet
        /// are passed as null; their projections stay empty rather than
        /// invented. Called on campaign setup, on every canonical day advance,
        /// and whenever the interior view (re)binds.
        /// </summary>
        internal void RefreshHoldfastPresentation()
        {
            var session = EnsurePresentation();
            var sources = new PresentationSources
            {
                Survivors = _survivors,
                Assignments = _shelterAssignment,
                Power = _powerGrid?.System,
                Thermal = _shelterThermal?.System,
                Sump = _sumpFlooding?.System,
                Map = _world?.WastelandMap,
                Memorial = _memorial,
                DutyRoster = _dutyRoster,
                OccupationFor = ResolveSurvivorOccupation,
                CurrentDay = Math.Max(1, _simDay),
                ReduceMotion = Settings.UserSettingsStore.Current.ReducedMotion
            };

            var slate = session.Compose(sources);
            _presentationDirty = true;
            PublishPresentationSlate(slate);
        }

        private void PublishPresentationSlate(HoldfastPresentationSlate slate)
        {
            if (slate == null) return;
            // The shelter panel owns the interior view instance and is the
            // refresh seam the settings screen already uses.
            _shelterPanel?.SetPresentationSlate(slate);
        }

        private void SetupPresentation()
        {
            EnsurePresentation();
            RefreshHoldfastPresentation();
        }

        private void SavePresentation()
        {
            // Derived read model: nothing is persisted. The canonical owners
            // (assignment, power, thermal, sump, map) own their own sections.
            _presentationDirty = false;
        }

        private void ResetPresentation()
        {
            _presentation?.Dispose();
            _presentation = null;
            _presentationDirty = false;
        }

        /// <summary>
        /// Occupation label for the slate's room job line, read from the
        /// survivor's authored definition (never a second roster store).
        /// </summary>
        private string ResolveSurvivorOccupation(string survivorId)
        {
            if (string.IsNullOrWhiteSpace(survivorId) || _survivors == null) return string.Empty;
            var entry = _survivors.Roster.Find(survivorId);
            if (entry == null) return string.Empty;
            var definition = _survivors.Roster.FindDefinition(entry.definitionId);
            return definition?.profession ?? string.Empty;
        }
    }
}
