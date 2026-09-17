// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Wave 8 B2 — Sky Defense Battery player route
// Subsystems   : presentation glue only. The Core authority
//                (SkyDefenseBatterySystem), its host driver
//                (Main.FlagshipInstitutions), and its save section already
//                exist; this file adds the missing player-facing panel and
//                binds it to the campaign-owned system. No new authority.
// ============================================================================
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.SkyDefense;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private UI.SkyDefenseBatteryPanel? _skyDefensePanel;
        private SkyDefenseBatterySystem? _skyDefensePanelBoundSystem;

        /// <summary>
        /// Creates (once) and binds the sky-defense surface to the
        /// campaign-owned system. Re-binds when a load-game reset has swapped
        /// the system instance; never constructs a second authority.
        /// </summary>
        private void EnsureSkyDefensePanel()
        {
            var system = EnsureSkyDefense();

            if (_skyDefensePanel == null)
            {
                _skyDefensePanel = new UI.SkyDefenseBatteryPanel();
                _skyDefensePanel.Bind(system, LivingSkyDefenseCrew, ResolveSurvivorArcName, SkyDefenseItemOnHand);
                _skyDefensePanelBoundSystem = system;
                _skyDefensePanel.Visible = false;
                AddChild(_skyDefensePanel);
                return;
            }

            if (!ReferenceEquals(_skyDefensePanelBoundSystem, system))
            {
                _skyDefensePanel.Bind(system, LivingSkyDefenseCrew, ResolveSurvivorArcName, SkyDefenseItemOnHand);
                _skyDefensePanelBoundSystem = system;
            }
        }

        private void OpenSkyDefenseBatteryPanel()
        {
            // The system consumes the canonical inventory and institution
            // ledger; both must be composed before the battery is built.
            SetupInventory();
            SetupSurvivors();
            SetupEnrichment();
            EnsureSkyDefensePanel();
            if (_skyDefensePanel != null)
            {
                _skyDefensePanel.Visible = true;
                _skyDefensePanel.RefreshView();
            }
        }

        /// <summary>Living survivors eligible for a gunner claim, from the roster authority.</summary>
        private IReadOnlyList<string> LivingSkyDefenseCrew()
        {
            SetupSurvivors();
            var ids = new List<string>();
            var roster = _survivors?.RosterState;
            if (roster == null) return ids;
            foreach (var s in roster)
            {
                if (s != null && s.IsAlive && !s.IsDead) ids.Add(s.Id);
            }
            return ids;
        }

        /// <summary>Canonical inventory count for the panel's read-only ordnance/oil rows.</summary>
        private int SkyDefenseItemOnHand(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return 0;
            SetupInventory();
            return _inventory?.Inventory?.CountById(itemId) ?? 0;
        }
    }
}