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

            string catalogPath = "res://Assets/StreamingAssets/Data/recreation.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
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

            string catalogPath = "res://Assets/StreamingAssets/Data/naval_vessels.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
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
    }
}
