using Godot;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AtomicWar.Journal;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Economy;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Foundry;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.Radio;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.Economy;
using AtomicWar.GodotApp.YearOfAsh;
using AtomicWar.GodotApp.Muster;
using AtomicWar.GodotApp.Dose;
using AtomicWar.GodotApp.UtilityAI;
using AtomicWar.GodotApp.Radio;
using AtomicWar.GodotApp.Audio;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        // ── Economy fields (GAP-ARCH-01 Phase 1) ──
        private EconomyHostSession _economy = null!;
        private bool _economyDirty;
        private TravelingCaravanHostSession _caravans = null!;
        private bool _caravansDirty;
        private AtomicWar.GodotApp.Economy.TradeScreenGodotPanel _tradePanel = null!;
        private Ashfall.Core.Radio.FactionRadioEngine _tradeRadio = null!;

        private void FlushCaravanIfDirty()
        {
            if (_caravansDirty) SaveCaravans();
        }

        private void SetupEconomy()
        {
            if (_economy != null) return;
            _economy = EconomyHostSession.Create(_dataDir);
            _economy.StateChanged += () => _economyDirty = true;
            var save = EconomySaveStore.TryLoad();
            if (save != null)
            {
                _economy.Market.RestoreState(save);
                _economyDirty = false; // restore just raised state-change events
                GD.Print("[Ashfall Godot] Economy state restored.");
            }

            if (_economyPanel == null && _rightColumn != null)
            {
                _economyPanel = new EconomyMarketPanel();
                _rightColumn.AddChild(_economyPanel);
            }
            if (_economyPanel != null)
            {
                _economyPanel.BindSession(_economy);
                _economyPanel.RefreshView();
            }
        }

        private void OnEconomyOpenClicked()
        {
            SetupEconomy();
            _statusLabel.Text = _economy.StatusLine();
            _codexViewer.Text = _economy.StatusLine();
        }

        private void OnEconomySaveClicked()
        {
            SetupEconomy();
            SaveEconomy();
        }

        private void SaveEconomy()
        {
            if (_economy == null) return;
            if (CaptureSection("economy", EconomySaveStore.TryCapturePersisted(_economy.CaptureSave())))
            {
                _economyDirty = false;
                GD.Print("[Ashfall Godot] Economy save written.");
            }
        }

        private void FlushEconomyIfDirty()
        {
            if (_economyDirty) SaveEconomy();
        }

        private void SetupCaravans()
        {
            if (_caravans != null) return;
            _caravans = TravelingCaravanHostSession.Create(_dataDir);
            _caravans.StateChanged += () => _caravansDirty = true;
            GD.Print("[Ashfall Godot] Caravan host ready.");
        }

        private void SaveCaravans()
        {
            if (_caravans == null) return;
            if (CaptureSection("caravan", CaravanSaveStore.TryCapturePersisted(_caravans.CaptureSave())))
            {
            _caravansDirty = false;
            _yearOfAshDirty = false;
                GD.Print("[Ashfall Godot] Caravan save written.");
            }
        }

        private void SaveSilentFoundry()
        {
            if (_silentFoundry == null) return;
            try
            {
                CaptureSection("silent_foundry", SilentFoundrySaveStore.TryCapturePersisted(_silentFoundry.Engine.CaptureState()));
            }
            catch (Exception e)
            {
                GD.PushWarning("[Ashfall Godot] SilentFoundry save failed: " + e.Message);
            }
        }

        private void SetupSilentFoundry()
        {
            if (_silentFoundry != null) return;
            SetupExpansions();
            SetupInventory();
            SetupJournal();
            SetupEconomy();
            SetupPowerGrid();
            _silentFoundry = AtomicWar.GodotApp.SilentFoundryHostSession.Create(
                _dataDir, _expansions, _inventory, _journal, market: _economy.Market);
            _silentFoundry.BindPowerAndThermal(_powerGrid?.System, _shelterThermal?.System);
            // GAP-STUB-03: wire the remaining FactionStanceEngine providers
            // from Main state so SilentFoundry guild trust reflects actual
            // campaign day, radiation, and military-survivor presence.
            _silentFoundry.BindStanceProviders(_simDay, _holdfastRuntime?.Radiation ?? 0f, _survivors);
            // Foundry state rides the expansion-hub save (already restored above);
            // state-change events mark the hub save dirty so nothing is lost.
            _silentFoundry.StateChanged += () =>
            {
                _foundryDirty = true;
                _silentFoundryPanel?.RefreshView();
                _factionsPanel?.RefreshView();
                _economyPanel?.RefreshView();
                if (_state == GameState.Playing) UpdateHud();
            };
            if (_silentFoundryPanel != null)
                _silentFoundryPanel.Bind(_silentFoundry, _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay);
            // Live market strip: show the guild's real trade access at all times.
            if (_economyPanel != null)
                _economyPanel.BindStance(_silentFoundry.GuildStanceEngine, Ashfall.Core.Foundry.SilentFoundryIds.FactionId);
            GD.Print("[Ashfall Godot] Silent Foundry host ready (exp_10_the_silent_foundry).");
        }

        private void CloseSilentFoundryPanel()
        {
            _silentFoundryPanel.Visible = false;
        }

        private void CloseTradePanel()
        {
            if (_silentFoundry != null)
                _silentFoundry.StateChanged -= _tradePanel.RefreshView;
            _tradePanel.Visible = false;
        }

        /// <summary>
        /// Open the live trade screen bound to the Foundry Guild's real stance
        /// engine (derived from the durable consequence ledger). The panel's
        /// confirm gate follows TradeStance: below Trade the stall is blocked.
        /// </summary>
        private void OpenTradeScreen()
        {
            if (_tradePanel == null) return;
            if (_tradeRadio == null)
            {
                string radioPath = Path.Combine(_dataDir, "faction_radio_corpus.json");
                _tradeRadio = Ashfall.Core.Radio.FactionRadioEngine.LoadFromJson(
                    System.IO.File.Exists(radioPath) ? System.IO.File.ReadAllText(radioPath) : "{}");
            }
            var tuning = new Ashfall.Core.Economy.HardcoreEconomyTuning();
            tuning.Apply(new Ashfall.Core.Economy.HardcoreEconomyTuningBundle(
                Array.Empty<Ashfall.Core.Economy.ScarcityEntry>(),
                Array.Empty<Ashfall.Core.Economy.FactionTradePreference>(),
                Array.Empty<Ashfall.Core.Economy.PriceShockRule>()));
            SetupCampaignDay();
            _tradePanel.BindSession(_economy, _silentFoundry.GuildStanceEngine, tuning, _tradeRadio, _campaignDay.Rng.GetStream(Ashfall.Core.Random.CampaignStreamIds.Economy).Rng);
            _tradePanel.SetActiveFaction(Ashfall.Core.Foundry.SilentFoundryIds.FactionId);
            // Live refresh when a treaty consequence moves the guild's standing
            // (subscribe once per open; CloseTradePanel removes it).
            _silentFoundry.StateChanged -= _tradePanel.RefreshView;
            _silentFoundry.StateChanged += _tradePanel.RefreshView;
            _tradePanel.Open();
            GD.Print($"[Ashfall Godot] Trade screen open — Foundry Guild stance {_silentFoundry.GuildStance} · trust {_silentFoundry.GuildTrust:F0}");
        }

        private void CloseEconomyPanel()
        {
            _economyPanel.Visible = false;
        }

        private void CloseEconomyDetailPanel()
        {
            _economyDetailPanel.Visible = false;
        }

    }
}
