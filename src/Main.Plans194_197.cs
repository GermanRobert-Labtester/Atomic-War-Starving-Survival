// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plans 194-197 Host Wire & Orchestration
// Subsystems   : Naval & River Exploration, Scrap Economy & Item Degradation,
//                Survivor Hobbies & Downtime, Winter Freeze & Hypothermia
// ============================================================================
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Recreation;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private SurvivorDowntimeSystem? _recreation;
        private ExpeditionNavalSystem? _navalSystem;
        private bool _recreationDirty;

        // ── Plan 196: Survivor Hobbies & Downtime ─────────────────────────

        public SurvivorDowntimeSystem EnsureRecreation()
        {
            if (_recreation != null) return _recreation;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("recreation") : new SeededRng(196);
            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();
            var needs = _survivors?.Needs ?? new Ashfall.Core.Survivors.NeedsSystem();
            var social = _shelterSocialDynamics;

            _recreation = new SurvivorDowntimeSystem(rng, inv, needs, social, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("recreation.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    _recreation.LoadCatalog(json);
                }
            }

            var saved = RecreationSaveStore.TryLoad();
            if (saved != null)
            {
                _recreation.RestoreState(saved);
            }

            _recreation.OnHobbyCompleted += (session, relief) =>
            {
                _recreationDirty = true;
                _journal?.TryAddRawEntry("recreation_session", $"Recreation session {session.hobbyId} completed, relieving {relief:F0} stress.", null!, _simDay);
            };

            _recreation.OnHobbyBrawl += (session, p1, p2) =>
            {
                _recreationDirty = true;
                _journal?.TryAddRawEntry("recreation_brawl", $"Dispute erupted between {p1} and {p2} during {session.hobbyId}!", null!, _simDay);
            };

            return _recreation;
        }

        private void SetupRecreation()
        {
            EnsureRecreation();
        }

        private void SaveRecreation()
        {
            if (_recreation != null)
            {
                CaptureSection("recreation", RecreationSaveStore.TryCapturePersisted(_recreation.CaptureState()));
                _recreationDirty = false;
            }
        }

        // ── Plan 194: Naval & River Exploration ───────────────────────────

        public ExpeditionNavalSystem EnsureNavalSystem()
        {
            if (_navalSystem != null) return _navalSystem;

            _navalSystem = new ExpeditionNavalSystem(new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("naval_vessels.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    _navalSystem.LoadCatalog(json);
                }
            }

            return _navalSystem;
        }

        // ── Daily Tick Coordination ──────────────────────────────────────

        public void TickPlans194_197(int day, List<Ashfall.Core.Campaign.DayStateChangeEvent>? events = null)
        {
            _recreation?.TickDay(day);
        }

        // ── Plan 196: downtime console commands ───────────────────────────

        private void HandleDowntimeAction(string action, string param = "")
        {
            if (action == "CLOSE") { CloseSurvivorDowntimePanel(); return; }
            if (_survivorDowntimePanel == null || _recreation == null) return;

            switch (action)
            {
                case "start":
                {
                    if (_recreation.GetHobby(param) == null) break;
                    var hobby = _recreation.GetHobby(param)!;

                    // Participants come from the canonical roster authority —
                    // up to the hobby's social maximum, at least its minimum.
                    var participants = new List<string>();
                    if (_survivors != null)
                    {
                        foreach (var s in _survivors.RosterState)
                        {
                            if (!string.IsNullOrEmpty(s.Id)) participants.Add(s.Id);
                            if (participants.Count >= Math.Max(1, hobby.social_max)) break;
                        }
                    }
                    if (participants.Count < hobby.social_min)
                    {
                        _survivorDowntimePanel.ShowFeedback("Not enough free hands for that pastime.", true);
                        break;
                    }

                    var res = _recreation.StartSession(param, roomId: "room_common_mess_hall", participants);
                    _survivorDowntimePanel.ShowFeedback(
                        res.IsSuccess ? "The session is underway. It completes at the end of the day."
                                      : "That session could not start — check the company and the gear it needs.",
                        !res.IsSuccess);
                    break;
                }
            }
            _survivorDowntimePanel.RefreshView();
        }

        // ── Plan 197: deep-freeze console commands ───────────────────────

        private void HandleWinterFreezeAction(string action, string param = "")
        {
            if (action == "CLOSE") { CloseWinterFreezePanel(); return; }
            if (_winterFreezePanel == null || _yearOfAsh == null) return;

            var deepFreeze = _yearOfAsh.DeepFreeze;
            if (deepFreeze == null) return;

            switch (action)
            {
                case "clear_ice":
                {
                    deepFreeze.ClearIntakeIce();
                    _winterFreezePanel.ShowFeedback("Cold-work party reports the intake cowling clear.", false);
                    break;
                }
                case "insulate":
                {
                    // Materials transact atomically through the canonical
                    // inventory authority; insulation boost goes through Core.
                    var bill = new InventoryBill();
                    bill.AddCost("scrap_wood", 4);
                    bill.AddCost("cloth", 1);
                    if (TryPayBill(bill))
                    {
                        deepFreeze.UpgradeThermalInsulation(0.10f);
                        _winterFreezePanel.ShowFeedback(
                            $"Insulation reinforced — quality now {deepFreeze.State.thermalInsulationQuality * 100f:0}%.", false);
                    }
                    else
                    {
                        _winterFreezePanel.ShowFeedback("Insulation work needs 4 scrap wood and 1 cloth.", true);
                    }
                    break;
                }
            }
            _winterFreezePanel.RefreshView();
        }
    }
}
