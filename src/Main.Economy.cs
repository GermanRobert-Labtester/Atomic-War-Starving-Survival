// SPDX-License-Identifier: MIT
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
        private TradeVoiceResolver _tradeVoiceResolver = null!;

        private void FlushCaravanIfDirty()
        {
            if (_caravansDirty) SaveCaravans();
        }

        private void SetupEconomy()
        {
            if (_economy != null) return;
            _economy = EconomyHostSession.Create(_dataDir);
            _economy.BindRationingResourceValidator(IsCanonicalRationingResource);
            // Plan 42 / Plan 46 — the rationing owner's tier change reaches the
            // journal voice trigger and the session telemetry through this one
            // forwarder; no second ration model is created in the host.
            _economy.RationTierChangedSeam += target =>
            {
                RecordPlayMetricRationPolicyChanged(target);
                TriggerSurvivorVoiceRationCut(target);
            };
            BindRationingToInventory();
            _economy.StateChanged += () => _economyDirty = true;

            // Plan 212 follow-up — trade rumors from real market state: a shock
            // the canonical market applies (or expires) becomes one band item.
            // The text is Core-projected; this adapter only relays it once per
            // event. Restore never re-fires the events, so no replay.
            _economy.Market.OnShockStarted += shock =>
            {
                if (shock == null) return;
                SetupRadio();
                _radio?.RecordMarketRumor(
                    Ashfall.Core.Economy.EconomyMarketRumorRules.ShockStartedLine(shock), shock.startDay);
            };
            _economy.Market.OnShockExpired += shock =>
            {
                if (shock == null) return;
                SetupRadio();
                _radio?.RecordMarketRumor(
                    Ashfall.Core.Economy.EconomyMarketRumorRules.ShockExpiredLine(shock), shock.startDay);
            };

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

        private bool IsCanonicalRationingResource(string resourceId)
        {
            if (string.IsNullOrWhiteSpace(resourceId)) return false;
            string id = resourceId.Trim();
            return (_inventory?.Catalog?.Get(id) != null)
                || (_economy?.Catalog?.Find(id) != null)
                || GoodCategories.IsKnown(id);
        }

        private void BindRationingToInventory()
        {
            if (_inventory == null || _economy == null) return;
            _inventory.RationingAuthorizer = (resourceId, consumerId, demand, day, available) =>
                _economy.AuthorizeAllocation(resourceId, consumerId, demand, available, day);
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

        /// <summary>
        /// Plan 212 — weather→market shock bridge. The weather authority owns
        /// weather; the market owns its indices; the band mapping is Core
        /// policy (<see cref="EconomyWeatherShockRules"/>). This adapter only
        /// reads the canonical weather state and applies the bounded,
        /// idempotent shock. Deterministic: pure function of current weather.
        /// Plan 14A — the embargo authority FIRST advances its decay state with
        /// the same authoritative weather (activation + decay are Core state);
        /// the market then applies the decay-aware multiplier as one embargo
        /// factor. No embargo shock ever enters ApplyShock — the two shock
        /// paths stay separate factors in the canonical price equation.
        /// </summary>
        private void TickEconomyWeatherBridge(int day)
        {
            if (_economy == null) return;
            if (_world?.Weather == null) return;
            _economy.EmbargoSystem?.NotifyWeather(day, _world.Weather.Current);
            var band = EconomyWeatherShockRules.TryGetWeatherShock(_world.Weather.Current);
            if (band == null) return;
            _economy.Market.ApplyShock(
                band.CategoryId, band.IsShortage, band.SeverityBp,
                startDay: day, durationDays: band.DurationDays, sourceId: band.SourceId);
        }

        private void SetupCaravans()
        {
            if (_caravans != null) return;
            SetupEconomy();
            _caravans = TravelingCaravanHostSession.Create(_dataDir);
            // Plan 14A — caravans share the campaign's ONE embargo authority
            // (rules from trade_embargoes.json); route blocking evaluates the
            // same rules the market prices from.
            _caravans.Engine.Embargoes = _economy.EmbargoSystem;
            SetupWorld();
            _caravans.Engine.Map = _world?.WastelandMap;
            // C2 / Plan 20C (§41) — weather availability from the ONE effects
            // table, combined with (never mixed into) the embargo multiplier.
            _caravans.Engine.WeatherAvailabilityProvider = weather =>
            {
                var effects = _world?.WeatherEffects;
                if (effects != null && effects.TryGetEffects(weather, out var fx) && fx != null)
                    return fx.caravan_availability_multiplier;
                return 1f;
            };
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
            // Plan B66: heavy batches emit smoke/CO through the canonical
            // ventilation authority (register/deactivate around each batch).
            _silentFoundry.Engine.BindVentilation(_ventilation);
            // GAP-STUB-03 (resolved): wire the remaining FactionStanceEngine
            // providers as live accessors into Main state, not one-time
            // captured values, so guild trust reflects the campaign's actual
            // current day, radiation, and military-survivor presence on every
            // future read — including after the values change post-bind.
            _silentFoundry.BindStanceProviders(
                campaignDayProvider: () => _simDay,
                partyRadiationProvider: () => _holdfastRuntime?.Radiation ?? 0f,
                survivorsProvider: () => _survivors);
            // v6 SaltMine: hub envelope already restored into expansions/foundry
            // Core systems; SaltMine lives on this host session — restore it
            // from the same hub payload when present.
            var hubSave = ExpansionHubSaveStore.TryLoad();
            if (hubSave?.saltMine != null)
                _silentFoundry.SaltMine.RestoreState(hubSave.saltMine);
            // Foundry + SaltMine ride the expansion-hub save; state-change
            // events mark the hub save dirty so nothing is lost.
            _silentFoundry.StateChanged += () =>
            {
                _foundryDirty = true;
                _silentFoundryPanel?.RefreshView();
                _factionsPanel?.RefreshView();
                _economyPanel?.RefreshView();
                if (_state == GameState.Playing) UpdateHud();
            };
            if (_silentFoundryPanel != null)
            {
                _silentFoundryPanel.Bind(_silentFoundry, _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay);
                _silentFoundryPanel.SetMachineTellCatalog(GetMachineTellCatalog());
            }
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

        private TradeVoiceResolver GetTradeVoiceResolver()
        {
            if (_tradeVoiceResolver != null) return _tradeVoiceResolver;

            var load = TradeTextCatalogLoader.Load(
                _dataDir,
                new FileSystemIO(),
                new SystemTextJsonSerializer());
            if (!load.IsValid)
            {
                GD.PushWarning("[Ashfall Godot] Trade voice catalog using fallback: "
                    + (load.Errors.Count == 0
                        ? "catalog missing"
                        : string.Join("; ", load.Errors)));
            }

            _tradeVoiceResolver = new TradeVoiceResolver(load.Catalog);
            return _tradeVoiceResolver;
        }

        private HardcoreEconomyTuning LoadHardcoreEconomyTuning()
        {
            var tuning = new HardcoreEconomyTuning();
            string path = Path.Combine(_dataDir, "hardcore_economy_tuning.json");
            if (!File.Exists(path))
            {
                GD.PushWarning($"[Ashfall Godot] Hardcore economy tuning missing: {path}");
                return tuning;
            }

            var result = HardcoreEconomyTuningLoader.Load(File.ReadAllText(path));
            if (!result.IsValid || result.Bundle == null)
            {
                GD.PushWarning("[Ashfall Godot] Hardcore economy tuning rejected: "
                    + string.Join("; ", result.Errors));
                return tuning;
            }

            tuning.Apply(result.Bundle);
            return tuning;
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
            var tuning = LoadHardcoreEconomyTuning();
            SetupCampaignDay();
            _tradePanel.BindSession(
                _economy,
                _silentFoundry.GuildStanceEngine,
                tuning,
                _tradeRadio,
                _campaignDay.Rng.GetStream(Ashfall.Core.Random.CampaignStreamIds.Economy).Rng,
                GetTradeVoiceResolver());
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
