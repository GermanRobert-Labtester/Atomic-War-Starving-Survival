// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Godot;
using Ashfall.Core.Radio;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Flagship XI — Plan 157 host wiring: constructs the psyops layer, gates
    /// broadcasts on the comms array (tier + power — the existing transmitter
    /// authority), routes loyalty shifts to the canonical faction authority that
    /// owns each target, persists the "psyops" envelope section, and ticks a
    /// phase-4 day owner. Holdfast trade factions have no runtime trust
    /// aggregate in this host; their influence accrues in the psyops pressure
    /// ledger only (implementation log D7).
    /// </summary>
    public partial class Main : Control
    {
        private PsyOpsHostSession? _psyops;

        private void SetupPsyOps()
        {
            if (_psyops != null) return;

            var catalog = PsyOpsCatalogLoader.Load(_dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var system = new PsyOpsSystem(catalog)
            {
                // The comms array is the transmitter authority: no array or no
                // grid power means broadcasts stall at residual reach.
                TransmitterReady = () =>
                    _commsArray != null && _commsArray.State.ArrayTier >= 1 && _commsArray.State.IsPowered
                    && _powerGrid?.System != null && !_powerGrid.System.IsBrownout,
                LoyaltyShiftRequested = RouteLoyaltyShift
            };

            _psyops = new PsyOpsHostSession(system);
            _psyops.BroadcastIntercepted += (campaignId, factionId, confidence, day) =>
            {
                // Typed intel fact: narrative/host systems decide presentation.
                GD.Print($"[Ashfall Godot] Broadcast intercepted: {campaignId} -> {factionId} (confidence {confidence:F2}, day {day}).");
            };

            var save = PsyOpsSaveStore.TryLoad();
            if (save != null)
            {
                _psyops.RestoreSave(save);
                GD.Print("[Ashfall Godot] PsyOps state restored.");
            }
        }

        /// <summary>
        /// Faction-scoped loyalty routing: each target goes to the authority that
        /// owns its aggregate, and only the targeted faction is touched.
        /// </summary>
        private void RouteLoyaltyShift(string factionId, float delta)
        {
            int whole = (int)MathF.Round(delta);
            if (whole == 0) return;

            if (string.Equals(factionId, "faction_central_garrison", StringComparison.Ordinal) ||
                string.Equals(factionId, "warlords_sector_4", StringComparison.Ordinal))
            {
                _yearOfAsh?.FactionWar?.ModifyStanding(factionId, whole);
                return;
            }
            if (string.Equals(factionId, "faction_prpf", StringComparison.Ordinal) ||
                string.Equals(factionId, "faction_military", StringComparison.Ordinal) ||
                string.Equals(factionId, "faction_rebel", StringComparison.Ordinal))
            {
                _factionBranch?.Coordinator?.ModifyStanding(factionId, whole);
                return;
            }
            // Holdfast trade factions: no runtime trust aggregate exists in this
            // host; influence stays in the psyops ideological-pressure ledger.
        }

        // ------------------------------------------------------------- commands

        public string StartPsyOpsCampaign(string campaignId, int day)
        {
            SetupPsyOps();
            if (_psyops == null) return "Broadcast systems are unavailable.";
            return _psyops.StartCampaignById(campaignId, day);
        }

        public string JamPsyOpsTarget(string factionId, float strength, int days, int day)
        {
            SetupPsyOps();
            if (_psyops == null) return "Broadcast systems are unavailable.";
            return _psyops.StartJammingOn(factionId, strength, days, day);
        }

        public string CounterPsyOpsCampaign(string campaignId, int days, int day)
        {
            SetupPsyOps();
            if (_psyops == null) return "Broadcast systems are unavailable.";
            return _psyops.CounterPropagandaOn(campaignId, days, day);
        }

        private void SavePsyOps()
        {
            if (_psyops == null) return;
            CaptureSection("psyops",
                PsyOpsSaveStore.TryCapturePersisted(_psyops.CaptureSave()));
        }
    }
}
