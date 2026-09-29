// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Combat;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Ashfall.Core.Narrative;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using Godot;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private CommsArraySystem? _commsArray;
        private bool _commsArrayDirty;

        // ── Plan 199: Communications Arrays & Distant Contact ───────────

        public CommsArraySystem EnsureCommsArray()
        {
            if (_commsArray != null) return _commsArray;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("comms_array") : new SeededRng(199);
            _commsArray = new CommsArraySystem(rng, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("comms_targets.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    _commsArray.LoadCatalog(json);
                }
            }

            var saved = CommsArraySaveStore.TryLoad();
            if (saved != null)
            {
                _commsArray.RestoreState(saved);
            }

            _commsArray.OnContactEstablished += (target, lockState) =>
            {
                _journal?.TryAddRawEntry("comms_contact_locked", $"Long-range carrier lock established: {target.DisplayName} ({target.FrequencyKhz} kHz)", null!, _simDay);
            };

            _commsArray.OnStrategicStrikeRequested += (targetId, code) =>
            {
                _journal?.TryAddRawEntry("strategic_strike_uplink", $"CRITICAL: Strategic orbital uplink transmission authorized! Target: {targetId} [AUTH: {code}]", null!, _simDay);
            };

            _commsArray.OnStateChanged += () => _commsArrayDirty = true;
            return _commsArray;
        }

        private void SetupCommsArray()
        {
            EnsureCommsArray();
        }

        private void SaveCommsArray()
        {
            if (_commsArray != null)
            {
                CaptureSection("comms_array", CommsArraySaveStore.TryCapturePersisted(_commsArray.CaptureState()));
                _commsArrayDirty = false;
            }
        }

        // ── Plan 199: comms array console commands ──────────────────────────

        private void HandleCommsArrayAction(string action, string param = "")
        {
            if (action == "CLOSE") { CloseCommsArrayTransceiverPanel(); return; }
            if (_commsArrayTransceiverPanel == null || _commsArray == null) return;

            switch (action)
            {
                case "tune":
                {
                    if (_commsArray.TargetCatalog.TryGetValue(param, out var target))
                    {
                        _commsArray.TuneFrequency(target.FrequencyKhz, target.Band);
                        _commsArrayTransceiverPanel.ShowFeedback(
                            $"Carrier moved to {target.FrequencyKhz} kHz ({target.Band}). Scanning.", false);
                    }
                    break;
                }
                case "upgrade_tier":
                {
                    // Strategic investment: each tier costs rare electronics,
                    // paid atomically from the canonical inventory authority.
                    var bill = new InventoryBill();
                    bill.AddCost("scrap_electronic", 5);
                    if (TryPayBill(bill))
                    {
                        _commsArray.SetArrayTier(_commsArray.State.ArrayTier + 1);
                        _commsArrayTransceiverPanel.ShowFeedback(
                            $"Array raised to tier {_commsArray.State.ArrayTier}. The antenna hears farther now.", false);
                    }
                    else
                    {
                        _commsArrayTransceiverPanel.ShowFeedback(
                            "Upgrade needs 5 salvaged electronics.", true);
                    }
                    break;
                }
                case "request_strike":
                {
                    // Endgame fictional capability: the intercepted code is
                    // authoritative and single-use; the Core request path owns
                    // every gate (tier, power, strategic target, code).
                    var lockState = _commsArray.GetOrCreateLock(param);
                    if (_commsArray.RequestStrategicStrike(param, lockState.InterceptedData, out string error))
                    {
                        _commsArrayTransceiverPanel.ShowFeedback(
                            "Uplink accepted. The request has left our hands.", false);
                    }
                    else
                    {
                        _commsArrayTransceiverPanel.ShowFeedback(error, true);
                    }
                    break;
                }
            }
            _commsArrayTransceiverPanel.RefreshView();
        }

    }
}
