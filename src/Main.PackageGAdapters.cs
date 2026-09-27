// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Package G adapters — three authored-policy/authority bindings onto
// owners that are already live. None of these adds a save section: the state
// they act on is already persisted by its existing owner.
//
//   • GuiltSourceHostSession          → live GuiltInsomniaSystem (Phase-0 host)
//   • BlackFlotillaStandingHostSession → live FactionStanceEngine (Deep Coast)
//   • PatientRecordIntegrityHostSession → live MedicalPipelineSaveState
//
// Each is a thin seam: it supplies the authored data or the missing policy
// registration and routes consequences to the owner that already owns them.
// ============================================================================

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private GuiltSourceHostSession? _guiltSources;
        private BlackFlotillaStandingHostSession? _flotillaStanding;

        // ── Authored guilt sources ───────────────────────────────────────

        public GuiltSourceHostSession? GuiltSources => _guiltSources;

        public void SetupGuiltSources()
        {
            if (_guiltSources != null) return;
            _guiltSources = new GuiltSourceHostSession(
                () => _phase0?.Guilt,
                () => _core?.Clock.Day ?? _simDay);
            _guiltSources.LoadCatalog(_dataDir ?? CatalogPath.ResolveDataDir());
        }

        /// <summary>
        /// Records guilt from a resolved choice using the AUTHORED severity.
        /// Replaces the hardcoded floats the previous call sites passed.
        /// </summary>
        public string RecordGuiltFromChoice(string survivorId, string choicePattern)
        {
            SetupGuiltSources();
            if (_guiltSources == null) return "guilt source catalog unavailable";
            bool ok = _guiltSources.RecordGuiltFromChoice(survivorId, choicePattern);
            return ok ? _guiltSources.LastEvent : $"Guilt refused: {_guiltSources.LastEvent}";
        }

        public string GuiltSourceStatusLine() => _guiltSources?.StatusLine() ?? "guilt catalog unbound";

        // ── Black Flotilla authored standing ─────────────────────────────

        public BlackFlotillaStandingHostSession? FlotillaStanding => _flotillaStanding;

        public void SetupFlotillaStanding()
        {
            if (_flotillaStanding != null) return;
            _flotillaStanding = new BlackFlotillaStandingHostSession(() => _deepCoast?.Stances);
            _flotillaStanding.Register();
        }

        public string FlotillaStandingStatusLine() =>
            _flotillaStanding?.StatusLine() ?? "flotilla standing unbound";

        /// <summary>Authored flotilla gate verdicts for the live maritime trust.</summary>
        public bool FlotillaWillTrade() { SetupFlotillaStanding(); return _flotillaStanding?.CanTrade() ?? false; }
        public bool FlotillaWillShareIntel() { SetupFlotillaStanding(); return _flotillaStanding?.CanShareIntel() ?? false; }

        public void ResetPackageGGuards()
        {
            _guiltSources = null;
            _flotillaStanding = null;
        }
    }
}
