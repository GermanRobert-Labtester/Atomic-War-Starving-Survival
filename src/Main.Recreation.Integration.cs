// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Recreation;
using Godot;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private SurvivorDowntimeSystem? _recreation;
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

    }
}
