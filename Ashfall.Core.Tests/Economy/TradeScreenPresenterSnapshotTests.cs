using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Snapshot and verification tests for TradeScreenPresenter (Item 77):
    /// 1. Barter totals & qualitative worth calculations
    /// 2. Disabled actions & stance/fairness gating
    /// 3. Selection capture, round-trip, and deterministic restoration
    /// 4. Biological offerings pricing
    /// 5. Radio ticker resolution
    /// </summary>
    public class TradeScreenPresenterSnapshotTests
    {
        private static FactionStanceEngine CreateTestStanceEngine(float trust = 0f, float raidAggression = 0.35f)
        {
            var engine = new FactionStanceEngine();
            engine.RegisterFaction(new FactionThresholds(
                "scavenger_camp",
                raidThreshold: -50f,
                robThreshold: -20f,
                minTrustToTrade: -40f,
                intelShareThreshold: 40f,
                raidAggression: raidAggression,
                trustInversion: false,
                healthyRadiationCeiling: 20f,
                highRadiationFloor: 60f));
            engine.SetTrust("scavenger_camp", trust);
            return engine;
        }

        [Fact]
        public void BarterTotals_CalculatesPlayerAndFactionWorthCorrectly()
        {
            var stance = CreateTestStanceEngine(trust: 10f);
            var presenter = new TradeScreenPresenter(
                stance,
                unitPriceLookup: id => id switch
                {
                    "clean_water" => 20f,
                    "canned_food" => 15f,
                    "medical_kit" => 40f,
                    _ => 10f
                },
                displayNameLookup: id => id switch
                {
                    "clean_water" => "Clean Water",
                    "canned_food" => "Canned Food",
                    "medical_kit" => "Medical Kit",
                    _ => id
                });

            presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);

            // 1. Initial state (empty)
            Assert.Equal(0f, presenter.ViewModel.PlayerOfferValue);
            Assert.Equal(0f, presenter.ViewModel.FactionAskValue);
            Assert.Equal("None", TradeWorthLabels.Format(presenter.ViewModel.PlayerOfferValue));
            Assert.Equal("None", TradeWorthLabels.Format(presenter.ViewModel.FactionAskValue));
            Assert.Equal(TradeFairness.EmptyTable, presenter.ViewModel.Fairness);

            // 2. Add player item offers: 2x canned_food (30) + 1x clean_water (20) = 50
            presenter.SetPlayerOffer("canned_food", 2);
            presenter.SetPlayerOffer("clean_water", 1);
            Assert.Equal(50f, presenter.ViewModel.PlayerOfferValue);
            Assert.Equal("Modest", TradeWorthLabels.Format(presenter.ViewModel.PlayerOfferValue));
            Assert.Equal(2, presenter.ActiveOfferCount);

            // 3. Add biological offering: 1x PintOfBlood (25) + 1x BoneMarrow (50) -> +75 = 125 total
            presenter.SetBiologicalOffer(BiologicalTradeItem.PintOfBlood, 1);
            presenter.SetBiologicalOffer(BiologicalTradeItem.BoneMarrow, 1);
            Assert.Equal(125f, presenter.ViewModel.PlayerOfferValue);
            Assert.Equal("Substantial", TradeWorthLabels.Format(presenter.ViewModel.PlayerOfferValue));
            Assert.Equal(2, presenter.ActiveBioCount);

            // 4. Add faction ask: 2x medical_kit (80) + 1x clean_water (20) = 100
            presenter.SetFactionAsk("medical_kit", 2);
            presenter.SetFactionAsk("clean_water", 1);
            Assert.Equal(100f, presenter.ViewModel.FactionAskValue);
            Assert.Equal("Substantial", TradeWorthLabels.Format(presenter.ViewModel.FactionAskValue));
            Assert.Equal(2, presenter.ActiveAskCount);

            // 5. Check scale comparison: 125 >= 100 -> Fair
            Assert.Equal(TradeFairness.Fair, presenter.ViewModel.Fairness);
            Assert.Equal("DEAL IS FAIR", presenter.ViewModel.FairnessLabel);
            Assert.True(presenter.ViewModel.CanConfirm);
        }

        [Theory]
        [InlineData(0f, "None")]
        [InlineData(10f, "Sparse")]
        [InlineData(19.9f, "Sparse")]
        [InlineData(20f, "Modest")]
        [InlineData(59.9f, "Modest")]
        [InlineData(60f, "Substantial")]
        [InlineData(149.9f, "Substantial")]
        [InlineData(150f, "Generous")]
        [InlineData(500f, "Generous")]
        public void BarterTotals_QualitativeThresholds_Snapshot(float value, string expectedLabel)
        {
            Assert.Equal(expectedLabel, TradeWorthLabels.Format(value));
        }

        [Fact]
        public void DisabledActions_EmptyTable_ConfirmDisabled()
        {
            var stance = CreateTestStanceEngine(trust: 20f);
            var presenter = new TradeScreenPresenter(stance);
            presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);

            Assert.False(presenter.ViewModel.CanConfirm);
            Assert.Equal(TradeFairness.EmptyTable, presenter.ViewModel.Fairness);
            Assert.Equal("EMPTY TABLE", presenter.ViewModel.FairnessLabel);
            Assert.False(presenter.TryConfirmTrade());
        }

        [Fact]
        public void DisabledActions_OfferShort_ConfirmDisabled()
        {
            var stance = CreateTestStanceEngine(trust: 20f);
            var presenter = new TradeScreenPresenter(
                stance,
                unitPriceLookup: id => id == "clean_water" ? 30f : 10f);
            presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);

            presenter.SetPlayerOffer("canned_food", 1); // 10
            presenter.SetFactionAsk("clean_water", 1);  // 30

            Assert.False(presenter.ViewModel.CanConfirm);
            Assert.Equal(TradeFairness.Short, presenter.ViewModel.Fairness);
            Assert.Equal("OFFER SHORT", presenter.ViewModel.FairnessLabel);
            Assert.False(presenter.TryConfirmTrade());
        }

        [Theory]
        [InlineData(-60f, false)] // Below raid threshold (-50): HostileRaid -> willTrade = false
        [InlineData(-30f, false)] // Below rob threshold (-20): Rob -> willTrade = false
        [InlineData(10f, true)]   // Between -20 and 40: Trade -> willTrade = true
        [InlineData(50f, true)]   // Above intelShare (40): ShareIntel -> willTrade = true
        public void DisabledActions_StanceGating_WillTradeControlsConfirm(float trust, bool expectCanConfirmWhenFair)
        {
            var stance = CreateTestStanceEngine(trust: trust);
            var presenter = new TradeScreenPresenter(
                stance,
                unitPriceLookup: _ => 20f);
            presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);

            // Offer 2x (40) vs Ask 1x (20) -> mathematically fair
            presenter.SetPlayerOffer("canned_food", 2);
            presenter.SetFactionAsk("clean_water", 1);

            Assert.Equal(TradeFairness.Fair, presenter.ViewModel.Fairness);
            Assert.Equal(expectCanConfirmWhenFair, presenter.ViewModel.CanConfirm);
            Assert.Equal(expectCanConfirmWhenFair, presenter.TryConfirmTrade());
        }

        [Fact]
        public void SelectionRestoration_CaptureAndRestore_PreservesAllOffersTotalsAndActions()
        {
            var stance = CreateTestStanceEngine(trust: 15f);
            var presenter = new TradeScreenPresenter(
                stance,
                unitPriceLookup: id => id switch
                {
                    "canned_food" => 12f,
                    "clean_water" => 25f,
                    "ammo" => 5f,
                    _ => 10f
                },
                displayNameLookup: id => id.Replace('_', ' '));

            presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);

            // 1. Configure complex selections
            presenter.SetPlayerOffer("canned_food", 3); // 36
            presenter.SetPlayerOffer("ammo", 10);        // 50
            presenter.SetBiologicalOffer(BiologicalTradeItem.PintOfBlood, 2); // 50
            presenter.SetBiologicalOffer(BiologicalTradeItem.Plasma, 1);      // 75
            // Total player value: 36 + 50 + 50 + 75 = 211

            presenter.SetFactionAsk("clean_water", 4);   // 100
            // Total faction value: 100

            float initialPlayerVal = presenter.ViewModel.PlayerOfferValue;
            float initialFactionVal = presenter.ViewModel.FactionAskValue;
            var initialFairness = presenter.ViewModel.Fairness;
            bool initialCanConfirm = presenter.ViewModel.CanConfirm;
            string initialSummary = presenter.BuildQuoteSummary();

            Assert.Equal(211f, initialPlayerVal);
            Assert.Equal(100f, initialFactionVal);
            Assert.Equal(TradeFairness.Fair, initialFairness);
            Assert.True(initialCanConfirm);

            // 2. Capture selection snapshot
            var selectionSnapshot = presenter.CaptureSelection();
            Assert.Equal(2, selectionSnapshot.PlayerOffers.Count);
            Assert.Equal(3, selectionSnapshot.PlayerOffers["canned_food"]);
            Assert.Equal(10, selectionSnapshot.PlayerOffers["ammo"]);
            Assert.Equal(1, selectionSnapshot.FactionAsks.Count);
            Assert.Equal(4, selectionSnapshot.FactionAsks["clean_water"]);
            Assert.Equal(2, selectionSnapshot.BiologicalOffers.Count);
            Assert.Equal(2, selectionSnapshot.BiologicalOffers[BiologicalTradeItem.PintOfBlood]);
            Assert.Equal(1, selectionSnapshot.BiologicalOffers[BiologicalTradeItem.Plasma]);

            // 3. Clear selections and assert empty state
            presenter.ClearOffers();
            Assert.Equal(0, presenter.ActiveOfferCount);
            Assert.Equal(0, presenter.ActiveAskCount);
            Assert.Equal(0, presenter.ActiveBioCount);
            Assert.Equal(0f, presenter.ViewModel.PlayerOfferValue);
            Assert.Equal(0f, presenter.ViewModel.FactionAskValue);
            Assert.Equal(TradeFairness.EmptyTable, presenter.ViewModel.Fairness);
            Assert.False(presenter.ViewModel.CanConfirm);

            // 4. Restore selection snapshot
            presenter.RestoreSelection(selectionSnapshot);

            // 5. Assert all counts and state restored with exact fidelity
            Assert.Equal(2, presenter.ActiveOfferCount);
            Assert.Equal(1, presenter.ActiveAskCount);
            Assert.Equal(2, presenter.ActiveBioCount);
            Assert.Equal(3, presenter.GetPlayerOfferCount("canned_food"));
            Assert.Equal(10, presenter.GetPlayerOfferCount("ammo"));
            Assert.Equal(4, presenter.GetFactionAskCount("clean_water"));
            Assert.Equal(2, presenter.GetBiologicalOfferCount(BiologicalTradeItem.PintOfBlood));
            Assert.Equal(1, presenter.GetBiologicalOfferCount(BiologicalTradeItem.Plasma));

            Assert.Equal(initialPlayerVal, presenter.ViewModel.PlayerOfferValue);
            Assert.Equal(initialFactionVal, presenter.ViewModel.FactionAskValue);
            Assert.Equal(initialFairness, presenter.ViewModel.Fairness);
            Assert.Equal(initialCanConfirm, presenter.ViewModel.CanConfirm);
            Assert.Equal(initialSummary, presenter.BuildQuoteSummary());
        }

        [Fact]
        public void SelectionRestoration_FactionSwitching_PreservesIndependence()
        {
            var stance = CreateTestStanceEngine(trust: 10f);
            stance.RegisterFaction(new FactionThresholds(
                "nomads",
                raidThreshold: -60f,
                robThreshold: -30f,
                minTrustToTrade: -20f,
                intelShareThreshold: 50f));
            stance.SetTrust("nomads", 25f);

            var presenter = new TradeScreenPresenter(
                stance,
                unitPriceLookup: id => id == "bread" ? 8f : 15f);

            // Setup Scavenger Camp trade
            presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);
            presenter.SetPlayerOffer("bread", 5); // 40
            presenter.SetFactionAsk("clean_water", 2); // 30
            var scavSnapshot = presenter.CaptureSelection();

            // Switch to Nomads (which resets offers on Open)
            presenter.Open("nomads", "Nomads", "Zara", 2);
            Assert.Equal(0, presenter.ActiveOfferCount);
            Assert.Equal(0, presenter.ActiveAskCount);

            presenter.SetPlayerOffer("bread", 2); // 16
            var nomadSnapshot = presenter.CaptureSelection();
            Assert.Equal(2, nomadSnapshot.PlayerOffers["bread"]);

            // Switch back to Scavenger Camp and restore selection
            presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);
            presenter.RestoreSelection(scavSnapshot);

            Assert.Equal(5, presenter.GetPlayerOfferCount("bread"));
            Assert.Equal(2, presenter.GetFactionAskCount("clean_water"));
            Assert.Equal(40f, presenter.ViewModel.PlayerOfferValue);
            Assert.Equal(30f, presenter.ViewModel.FactionAskValue);
        }

        [Fact]
        public void BiologicalOfferings_PricingCalculations()
        {
            Assert.Equal(25f, TradePricing.BioUnitValue(BiologicalTradeItem.PintOfBlood));
            Assert.Equal(50f, TradePricing.BioUnitValue(BiologicalTradeItem.BoneMarrow));
            Assert.Equal(75f, TradePricing.BioUnitValue(BiologicalTradeItem.Plasma));
            Assert.Equal(100f, TradePricing.BioUnitValue(BiologicalTradeItem.Organ));

            var stance = CreateTestStanceEngine(trust: 10f);
            var presenter = new TradeScreenPresenter(stance);
            presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);

            presenter.SetBiologicalOffer(BiologicalTradeItem.PintOfBlood, 2); // 50
            presenter.SetBiologicalOffer(BiologicalTradeItem.BoneMarrow, 1);  // 50
            presenter.SetBiologicalOffer(BiologicalTradeItem.Plasma, 2);      // 150
            presenter.SetBiologicalOffer(BiologicalTradeItem.Organ, 1);       // 100

            // 50 + 50 + 150 + 100 = 350
            Assert.Equal(350f, presenter.ViewModel.PlayerOfferValue);
            Assert.Equal(4, presenter.ActiveBioCount);

            // Removing count clears it
            presenter.SetBiologicalOffer(BiologicalTradeItem.Organ, 0);
            Assert.Equal(3, presenter.ActiveBioCount);
            Assert.Equal(250f, presenter.ViewModel.PlayerOfferValue);
        }

        [Fact]
        public void RadioTicker_UpdatesOnRecalculateAndExecution()
        {
            var stance = CreateTestStanceEngine(trust: 10f);
            var radio = FactionRadioEngine.LoadFromJson(@"{
                ""factions"": {
                    ""scavenger_camp"": {
                        ""frequency_mhz"": 104.7,
                        ""callsign"": ""SCAV-1"",
                        ""intercept_chatter"": [ ""Listening on wire."" ],
                        ""trade_reaction"": [ ""Deal accepted."" ],
                        ""parley_resolution"": [ ""Parley acknowledged."" ]
                    }
                }
            }");

            var presenter = new TradeScreenPresenter(
                stance,
                unitPriceLookup: _ => 20f,
                radioProvider: radio,
                rng: new SeededRng(2026));

            presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);
            Assert.Contains("Listening on wire.", presenter.ViewModel.RadioTickerLine);

            presenter.SetPlayerOffer("food", 2);
            presenter.SetFactionAsk("water", 1);
            Assert.True(presenter.TryConfirmTrade());
            Assert.Contains("Deal accepted.", presenter.ViewModel.RadioTickerLine);

            presenter.TryDemandParley();
            Assert.Contains("Parley acknowledged.", presenter.ViewModel.RadioTickerLine);
        }
    }
}
