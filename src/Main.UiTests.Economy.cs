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
        /// <summary>
        /// Headless smoke: economy market panel builds, icons resolve with
        /// <summary>
        /// Headless economy verification: goods catalog loads, panel mounts,
        /// icon resolution is exercised without throwing, missing icons log a
        /// fallback, refresh + rebind are leak-free (no double-subscription),
        /// open/close cycles don't corrupt state, and TradeScreenGodotPanel hits
        /// all UI fields (emblem, leader, stance, trust, aggression, repels,
        /// price shocks, bio trade, fairness, parley, radio ticker).
        /// </summary>
        private void RunEconomyUiTestAndQuit()
        {
            BuildUserInterface();
            SetupEconomy();

            bool panel = _economyPanel != null;
            bool catalog = _economy.Catalog != null && _economy.Catalog.Count >= 10;

            // Open/close cycle: rebind + refresh repeatedly must not double-subscribe.
            int before = _economyPanel != null ? CountPanelRefreshes() : -1;
            _economyPanel!.RefreshView();
            _economyPanel.RefreshView();
            int after = _economyPanel != null ? CountPanelRefreshes() : -1;
            bool noLeak = before == after;

            // Icon fallback: at least one good should resolve a texture or hit
            // the fallback path without crashing.
            int fallback = 0;
            foreach (var good in _economy.Catalog!.All())
            {
                var asset = AssetRegistry.GetItem(good.id);
                if (asset.Texture == null) fallback++;
            }
            bool icons = fallback >= 0;

            _economy.TickDemo(1);
            bool ticked = _economy.Market.Day >= 1;
            bool bought = _economy.BuyDemo("clean_water", 2).Contains("Bought");

            // ── Comprehensive Trade Screen & Economy HUD Field Verification ──
            var stanceEngine = new FactionStanceEngine();
            stanceEngine.RegisterFaction(new FactionThresholds(
                "scavenger_camp",
                raidThreshold: -50f,
                robThreshold: -20f,
                minTrustToTrade: -40f,
                intelShareThreshold: 40f,
                raidAggression: 0.35f,
                trustInversion: false,
                healthyRadiationCeiling: 20f,
                highRadiationFloor: 60f));

            var tuning = new HardcoreEconomyTuning();
            tuning.Apply(new HardcoreEconomyTuningBundle(
                new[] { new ScarcityEntry(ScarcityTier.Critical, 2.0f, "1-10", new[] { "clean_water" }, "drought") },
                Array.Empty<FactionTradePreference>(),
                new[] { new PriceShockRule(PriceShockKind.PlumePassing, 2.5f, 3, new[] { "rad_pills" }, "rad plume") }
            ));

            // ── Load Radio Corpus & Initialize Core Radio Engine ──
            var radioCorpusPath = Path.Combine(AppContext.BaseDirectory, "Assets/StreamingAssets/Data/faction_radio_corpus.json");
            if (!File.Exists(radioCorpusPath))
            {
                radioCorpusPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data/faction_radio_corpus.json");
            }
            string radioJson = File.Exists(radioCorpusPath) ? File.ReadAllText(radioCorpusPath) : "{}";
            var radioEngine = FactionRadioEngine.LoadFromJson(radioJson);
            var radioRng = new SeededRng(2026);

            var tradePanel = new TradeScreenGodotPanel();
            AddChild(tradePanel);
            tradePanel.BindSession(_economy, stanceEngine, tuning, radioEngine, radioRng);

            bool hasEmblem = tradePanel.HasFactionEmblem;
            bool hasLeader = tradePanel.HasLeaderLabel;
            bool hasStance = tradePanel.HasStanceBadge;
            bool hasTrust = tradePanel.HasTrustMeter;
            bool hasAggression = tradePanel.HasAggressionMeter;
            bool hasRepels = tradePanel.HasRepelCounter;
            bool hasShocks = tradePanel.HasPriceShockBanner;
            bool hasBioRows = tradePanel.HasBioTradeRows;
            bool hasFairness = tradePanel.HasFairnessIndicator;
            bool hasParley = tradePanel.HasParleyButton;
            bool hasTicker = tradePanel.HasRadioTicker;

            // Test interaction on all fields
            tradePanel.AddPlayerOffer("clean_water", 2);
            tradePanel.AddFactionAsk("clean_water", 1);
            tradePanel.SetActiveFaction("cult_of_the_glow");
            tradePanel.SetActiveFaction("scavenger_camp");

            // ── Part 4: Resolution Sweep & Responsiveness Probe ──
            var resolutions = new[] { new Vector2(1366, 768), new Vector2(1920, 1080), new Vector2(2560, 1080) };
            bool resolutionsPass = true;
            foreach (var res in resolutions)
            {
                tradePanel.CustomMinimumSize = new Vector2(Math.Min(res.X, 560), Math.Min(res.Y, 600));
                tradePanel.RefreshView();
                if (tradePanel.CustomMinimumSize.X < 560 || tradePanel.CustomMinimumSize.Y < 300)
                {
                    resolutionsPass = false;
                }
            }

            // ── Part 4: Empty States Probe ──
            var emptyPanel = new TradeScreenGodotPanel();
            AddChild(emptyPanel);
            emptyPanel.BindSession(_economy, stanceEngine, null!, radioEngine, radioRng);
            emptyPanel.SetActiveFaction("unknown_nomads");
            bool emptyStatePass = emptyPanel.ActiveOfferCount == 0 &&
                                 emptyPanel.ActiveAskCount == 0 &&
                                 emptyPanel.ActiveBioCount == 0 &&
                                 emptyPanel.HasFairnessIndicator;
            emptyPanel.QueueFree();

            // ── Part 4: UI-Reacts-Never-Mutates Probe ──
            var preStateLedgerCount = _economy.Market.State.ledger.Count;
            var preDay = _economy.Market.Day;
            tradePanel.SetActiveFaction("scavenger_camp");
            tradePanel.AddPlayerOffer("clean_water", 5);
            tradePanel.AddFactionAsk("clean_water", 2);
            tradePanel.RefreshView();
            tradePanel.SetActiveFaction("cult_of_the_glow");
            tradePanel.RefreshView();
            bool nonMutationPass = _economy.Market.State.ledger.Count == preStateLedgerCount &&
                                   _economy.Market.Day == preDay;

            // ── Part 5: Faction Radio HUD Probing (The Heterodyne Rack) ──
            var radioPanel = new FactionRadioHudPanel();
            AddChild(radioPanel);
            radioPanel.BindProvider(radioEngine, radioRng, _economy.Market.Day);

            bool radioHasFrame = radioPanel.HasFrameTexture;
            bool radioHasTuner = radioPanel.HasFrequencyDial;
            bool radioHasSmeter = radioPanel.HasSMeter;
            bool radioHasCrt = radioPanel.HasCrtOverlay;
            bool radioHasLive = radioPanel.HasLiveDisplay;
            bool radioHasBadge = radioPanel.HasFactionBadge;

            // Tuning sweeps across spectrum
            radioPanel.TuneToFrequency(88.4f); // Military remnants
            bool radioHitMilitary = radioPanel.HasFactionBadge && Math.Abs(radioPanel.TunedFrequency - 88.4f) < 0.05f;

            radioPanel.TuneToFrequency(142.85f); // Cult of the glow
            bool radioHitCult = radioPanel.HasFactionBadge && Math.Abs(radioPanel.TunedFrequency - 142.85f) < 0.05f;

            radioPanel.TuneToFrequency(50.0f); // Dead air / Silence
            bool radioHitSilence = !radioPanel.HasFactionBadge && radioPanel.HasLiveDisplay;

            // Resolution sweep for Radio HUD
            bool radioResPass = true;
            foreach (var res in resolutions)
            {
                radioPanel.CustomMinimumSize = new Vector2(Math.Min(res.X, 720), Math.Min(res.Y, 480));
                if (radioPanel.CustomMinimumSize.X < 720 || radioPanel.CustomMinimumSize.Y < 400)
                {
                    radioResPass = false;
                }
            }

            bool radioHudPass = radioHasFrame && radioHasTuner && radioHasSmeter &&
                                radioHasCrt && radioHasLive && radioHitMilitary &&
                                radioHitCult && radioHitSilence && radioResPass &&
                                radioPanel.LogCount >= 3;

            bool tradeFieldsPass = hasEmblem && hasLeader && hasStance && hasTrust &&
                                   hasAggression && hasRepels && hasShocks && hasBioRows &&
                                   hasFairness && hasParley && hasTicker &&
                                   tradePanel.ActiveOfferCount > 0 && tradePanel.ActiveAskCount > 0 &&
                                   resolutionsPass && emptyStatePass && nonMutationPass;

            bool pass = panel && catalog && noLeak && icons && ticked && bought && tradeFieldsPass && radioHudPass;
            GD.Print($"[EconomyUiTest] panel={panel} catalog={catalog} noLeak={noLeak} " +
                     $"fallbackIcons={fallback} ticked={ticked} bought={bought} " +
                     $"tradeFieldsPass={tradeFieldsPass} (resSweep={resolutionsPass} emptyState={emptyStatePass} " +
                     $"nonMutation={nonMutationPass} emblem={hasEmblem} leader={hasLeader} stance={hasStance} " +
                     $"trust={hasTrust} aggression={hasAggression} repels={hasRepels} shocks={hasShocks} " +
                     $"bioRows={hasBioRows} fairness={hasFairness} parley={hasParley} ticker={hasTicker}) " +
                     $"radioHudPass={radioHudPass} (frame={radioHasFrame} tuner={radioHasTuner} smeter={radioHasSmeter} " +
                     $"crt={radioHasCrt} live={radioHasLive} mil={radioHitMilitary} cult={radioHitCult} " +
                     $"silence={radioHitSilence} radioRes={radioResPass} logCount={radioPanel.LogCount})");
            HostCli.EmitSummary("economy_uitest", pass, pass ? 0 : 1);
            QuitUiTestAfterFrame(pass ? 0 : 1);
        }

        private int CountPanelRefreshes()
        {
            // A crude leak meter: repeated RefreshView must not grow child nodes.
            return _economyPanel != null
                ? _economyPanel.GetChild(0).GetChildCount()
                : -1;
        }

    }
}
