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
        private CeremonySystem? _ceremonySystem;
        private bool _ceremonyDirty;

        // ── Plan 200: Wasteland Festivals & Ceremonies ───────────────────

        public CeremonySystem EnsureCeremonySystem()
        {
            if (_ceremonySystem != null) return _ceremonySystem;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("ceremony_system") : new SeededRng(200);
            _ceremonySystem = new CeremonySystem(rng, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("ceremonies.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    _ceremonySystem.LoadCatalog(json);
                }
            }

            var saved = CeremonySaveStore.TryLoad();
            if (saved != null)
            {
                _ceremonySystem.RestoreState(saved);
            }

            _ceremonySystem.OnTruceRequested += (factionId, days) =>
            {
                _journal?.TryAddRawEntry("ceremony_truce_declared", $"Festival truce negotiated with {factionId} for {days} days.", null!, _simDay);
            };

            _ceremonySystem.OnCeremonyDisaster += (ceremonyId, disasterId) =>
            {
                _journal?.TryAddRawEntry("ceremony_disaster_event", $"Incident during festival celebration: {disasterId}!", null!, _simDay);
            };

            _ceremonySystem.OnStateChanged += () => _ceremonyDirty = true;
            return _ceremonySystem;
        }

        private void SetupCeremony()
        {
            EnsureCeremonySystem();
        }

        private void SaveCeremony()
        {
            if (_ceremonySystem != null)
            {
                CaptureSection("ceremony", CeremonySaveStore.TryCapturePersisted(_ceremonySystem.CaptureState()));
                _ceremonyDirty = false;
            }
        }

        // ── Plan 200: ceremony console commands ───────────────────────────

        private void HandleCeremonyAction(string action, string param = "")
        {
            if (action == "CLOSE") { CloseCeremonyFestivalPanel(); return; }
            if (_ceremonyFestivalPanel == null || _ceremonySystem == null) return;

            switch (action)
            {
                case "schedule":
                {
                    int population = _survivors?.RosterState.Count ?? 0;
                    if (_ceremonySystem.ScheduleCeremony(param, _simDay, population, out string error))
                    {
                        _ceremonyFestivalPanel.ShowFeedback("Preparation begins. The feast stores must be filled before the day comes.", false);
                    }
                    else
                    {
                        _ceremonyFestivalPanel.ShowFeedback(error, true);
                    }
                    break;
                }
                case "contribute":
                {
                    // param: itemId:quantity — atomic pay-then-commit.
                    var parts = param.Split(':');
                    if (parts.Length != 2 || !int.TryParse(parts[1], out int qty) || qty <= 0)
                        break;
                    var bill = new InventoryBill();
                    bill.AddCost(parts[0], qty);
                    if (TryPayBill(bill))
                    {
                        if (!_ceremonySystem.ContributeResource(parts[0], qty))
                        {
                            RefundBill(bill);
                            _ceremonyFestivalPanel.ShowFeedback("The feast stores would not take that.", true);
                        }
                        else
                        {
                            _ceremonyFestivalPanel.ShowFeedback("Stores committed to the ceremony.", false);
                        }
                    }
                    else
                    {
                        _ceremonyFestivalPanel.ShowFeedback("Not enough in storage for that commitment.", true);
                    }
                    break;
                }
                case "invite":
                {
                    float trust = EnsureSharedFactionStance().GetTrust(param);
                    if (_ceremonySystem.InviteFaction(param, (int)trust))
                    {
                        bool accepted = trust >= -10f;
                        _ceremonyFestivalPanel.ShowFeedback(
                            accepted ? $"{UI.FactionDisplay.Name(param)} has accepted the invitation."
                                     : $"An envoy was sent to {UI.FactionDisplay.Name(param)}.",
                            !accepted);
                    }
                    else
                    {
                        _ceremonyFestivalPanel.ShowFeedback("That faction cannot be invited again.", true);
                    }
                    break;
                }
            }
            _ceremonyFestivalPanel.RefreshView();
        }

    }
}
