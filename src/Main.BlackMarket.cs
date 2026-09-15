// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plan 211 — Black Market & Underworld Syndicates host wire
// Subsystems   : inventory catalog + canonical market/bounty binding,
//                daily stock refresh + debt/heat tick, save section.
// ============================================================================
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private BlackMarketHostSession? _blackMarket;
        private bool _blackMarketDirty;

        public BlackMarketHostSession EnsureBlackMarketSession()
        {
            SetupBlackMarket();
            return _blackMarket!;
        }

        private void SetupBlackMarket()
        {
            if (_blackMarket != null) return;

            SetupEconomy();
            _blackMarket = BlackMarketHostSession.Create(_dataDir, _economy.Market);
            _blackMarket.StateChanged += () =>
            {
                _blackMarketDirty = true;
                _economyPanel?.RefreshView();
                if (_state == GameState.Playing) UpdateHud();
            };
        }

        private void SaveBlackMarket()
        {
            if (_blackMarket == null) return;
            CaptureSection("black_market", BlackMarketSaveStore.TryCapturePersisted(_blackMarket.CaptureSave()));
        }

        private void FlushBlackMarketIfDirty()
        {
            if (_blackMarketDirty) SaveBlackMarket();
        }

        // ── Panel (Plan 211 Phase 9): created hidden; opened via the
        //    expanded-panel route. Presentation only.
        private UI.BlackMarketPanel? _blackMarketPanel;

        private void EnsureBlackMarketPanel()
        {
            if (_blackMarket == null) return;
            if (_blackMarketPanel != null) return;
            _blackMarketPanel = new UI.BlackMarketPanel();
            _blackMarketPanel.Bind(_blackMarket);
            _blackMarketPanel.Visible = false;
            AddChild(_blackMarketPanel);
        }

        private void OpenBlackMarketPanel()
        {
            SetupBlackMarket();
            EnsureBlackMarketPanel();
            if (_blackMarketPanel != null) { _blackMarketPanel.Visible = true; _blackMarketPanel.RefreshView(); }
        }
    }
}
