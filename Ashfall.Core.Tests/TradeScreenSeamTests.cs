using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Negotiation Table seam: Act 0 scenarios (fair / short / empty),
    /// Track B presenter mapping + zero-mutation invariant + TradeScreenUI
    /// API parity.
    /// </summary>
    public class TradeScreenSeamTests
    {
        private static string ReadDataFile(string fileName)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", fileName);
            if (!File.Exists(path))
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", fileName);
            }
            Assert.True(File.Exists(path), $"Data file not found at {path}");
            return File.ReadAllText(path);
        }

        private static IReadOnlyList<TradeScreenScenario> LoadScenarios()
        {
            return TradeScreenScenarioLoader.LoadFromJson(ReadDataFile("trade_screen_scenarios.json"));
        }

        private static TradeTellEngine LoadTells()
        {
            return TradeTellEngine.LoadFromJson(ReadDataFile("trade_tell_lines.json"));
        }

        private static FactionStanceEngine CreateStanceEngine()
        {
            var engine = new FactionStanceEngine();
            engine.RegisterFaction(new FactionThresholds(
                "scavenger_camp",
                raidThreshold: -50f,
                robThreshold: -20f,
                minTrustToTrade: -40f,
                intelShareThreshold: 40f,
                raidAggression: 0.35f,
                trustInversion: false,
                healthyRadiationCeiling: 20f,
                highRadiationFloor: 60f));
            return engine;
        }

        // ── Act 0: mock scenarios ────────────────────────────────────

        [Fact]
        public void Scenarios_LoadAllThreeFromData()
        {
            var scenarios = LoadScenarios();

            Assert.Equal(3, scenarios.Count);
            Assert.Contains(scenarios, s => s.Id == "fair_deal");
            Assert.Contains(scenarios, s => s.Id == "offer_short");
            Assert.Contains(scenarios, s => s.Id == "empty_table");
        }

        [Fact]
        public void Scenario_FairDeal_ComputedFairnessMatchesDataExpectation()
        {
            var binding = TradeScreenScenarioLoader.CreateBinding(
                GetScenario("fair_deal"), LoadTells(), new SeededRng(2026));

            var vm = binding.ViewModel;
            Assert.True(vm.IsOpen);
            Assert.Equal(TradeFairness.Fair, vm.Fairness);
            Assert.Equal("DEAL IS FAIR", vm.FairnessLabel);
            Assert.True(vm.CanConfirm);
            Assert.False(string.IsNullOrWhiteSpace(vm.StanceTellLine));

            // 3x18 + 1x15 + 1 pint of blood (25) = 94 vs 2x22 = 44.
            Assert.Equal(94f, vm.PlayerOfferValue, 2);
            Assert.Equal(44f, vm.FactionAskValue, 2);

            // Intent routing through the seam.
            Assert.True(binding.Intents.TryConfirmTrade());
            Assert.Equal(1, binding.Intents.ConfirmCalls);
        }

        [Fact]
        public void Scenario_OfferShort_BlocksConfirmAndKeepsStanceLegible()
        {
            var binding = TradeScreenScenarioLoader.CreateBinding(
                GetScenario("offer_short"), LoadTells(), new SeededRng(2026));

            var vm = binding.ViewModel;
            Assert.Equal(TradeFairness.Short, vm.Fairness);
            Assert.Equal("OFFER SHORT", vm.FairnessLabel);
            Assert.False(vm.CanConfirm);
            Assert.True(vm.ConsecutiveRepels > 0);

            // The mock sink still routes and records intent; the verdict is data-defined.
            Assert.False(binding.Intents.TryConfirmTrade());
            Assert.Equal(1, binding.Intents.ConfirmCalls);
        }

        [Fact]
        public void Scenario_EmptyTable_IsDeliberateNotBroken()
        {
            var binding = TradeScreenScenarioLoader.CreateBinding(
                GetScenario("empty_table"), LoadTells(), new SeededRng(2026));

            var vm = binding.ViewModel;
            Assert.Equal(TradeFairness.EmptyTable, vm.Fairness);
            Assert.Equal("EMPTY TABLE", vm.FairnessLabel);
            Assert.False(vm.CanConfirm);
            Assert.Empty(vm.PlayerOffers);
            Assert.Empty(vm.FactionDemands);
            Assert.Empty(vm.BiologicalOffers);

            // A deliberate posture: stance, tell, and radio all speak.
            Assert.Equal(TradeStance.Refuse, vm.Stance);
            Assert.False(string.IsNullOrWhiteSpace(vm.StanceTellLine));
            Assert.False(string.IsNullOrWhiteSpace(vm.RadioTickerLine));
        }

        [Fact]
        public void Scenario_IntentSink_CloseRecordsTradedFlag()
        {
            var binding = TradeScreenScenarioLoader.CreateBinding(
                GetScenario("fair_deal"), LoadTells(), new SeededRng(2026));

            binding.Intents.Close(traded: true);
            Assert.Equal(1, binding.Intents.CloseCalls);
            Assert.True(binding.Intents.LastCloseWasTraded);
        }

        private static TradeScreenScenario GetScenario(string id)
        {
            var scenarios = LoadScenarios();
            foreach (var s in scenarios)
            {
                if (s.Id == id) return s;
            }
            Assert.Fail($"Scenario {id} not found");
            return null;
        }

        // ── Track B: presenter ───────────────────────────────────────

        /// <summary>Counting decorator proving the presenter never mutates providers.</summary>
        private sealed class MutationCountingStanceProvider : IFactionStanceProvider
        {
            private readonly IFactionStanceProvider _inner;
            public int Mutations { get; private set; }

            public MutationCountingStanceProvider(IFactionStanceProvider inner) { _inner = inner; }

            public TradeStance GetStance(string factionId) => _inner.GetStance(factionId);
            public bool WillTrade(string factionId) => _inner.WillTrade(factionId);
            public bool WillShareIntel(string factionId) => _inner.WillShareIntel(factionId);
            public float GetTrust(string factionId) => _inner.GetTrust(factionId);
            public float GetEffectiveTrust(string factionId) => _inner.GetEffectiveTrust(factionId);
            public float ModifyTrust(string factionId, float delta) { Mutations++; return _inner.ModifyTrust(factionId, delta); }
            public void SetTrust(string factionId, float value) { Mutations++; _inner.SetTrust(factionId, value); }
            public float GetRaidAggression(string factionId) => _inner.GetRaidAggression(factionId);
            public void SetRaidAggression(string factionId, float value) { Mutations++; _inner.SetRaidAggression(factionId, value); }
            public bool IsFactionActive(string factionId) => _inner.IsFactionActive(factionId);
        }

        [Fact]
        public void Presenter_MapsProvidersOntoViewModel()
        {
            var stance = CreateStanceEngine();
            var tuning = new HardcoreEconomyTuning();
            tuning.Apply(new HardcoreEconomyTuningBundle(
                new[] { new ScarcityEntry(ScarcityTier.Critical, 2.0f, "1-50", new[] { "clean_water" }, "drought") },
                Array.Empty<FactionTradePreference>(),
                new[] { new PriceShockRule(PriceShockKind.PlumePassing, 2.5f, 10, new[] { "rad_pills" }, "rad plume") }
            ));

            var presenter = new TradeScreenPresenter(
                stance, tuning, LoadTells(), new SeededRng(2026),
                unitPriceLookup: id => id == "clean_water" ? 22f : 18f);
            presenter.SetWorldContext("CivilWar", 5);
            presenter.SetWatchedItems(new[] { "clean_water" });

            Assert.True(presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1));

            var vm = presenter.ViewModel;
            Assert.True(vm.IsOpen);
            Assert.Equal(TradeStance.Trade, vm.Stance);
            Assert.Equal(0f, vm.Trust, 2);
            Assert.Equal(0.35f, vm.Aggression, 2);
            Assert.False(string.IsNullOrWhiteSpace(vm.StanceTellLine));
            Assert.Single(vm.ShockBadges);
            Assert.Equal(PriceShockKind.PlumePassing, vm.ShockBadges[0].Kind);
            Assert.Single(vm.ScarcityMultipliers);
            Assert.Equal(2.0f, vm.ScarcityMultipliers[0].Multiplier, 2);
        }

        [Fact]
        public void Presenter_ZeroMutation_InvariantHolds()
        {
            var counting = new MutationCountingStanceProvider(CreateStanceEngine());
            var presenter = new TradeScreenPresenter(counting, null, LoadTells(), new SeededRng(2026));

            presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);
            presenter.SetPlayerOffer("canned_food", 3);
            presenter.SetFactionAsk("clean_water", 2);
            presenter.SetBiologicalOffer(BiologicalTradeItem.PintOfBlood, 1);
            presenter.Recalculate();
            presenter.TryConfirmTrade();
            presenter.TryDemandParley();
            presenter.BuildQuoteSummary();
            presenter.Close(traded: false);

            Assert.Equal(0, counting.Mutations);
        }

        [Fact]
        public void Presenter_ApiParity_TradeScreenUISurface()
        {
            var presenter = new TradeScreenPresenter(
                CreateStanceEngine(), null, LoadTells(), new SeededRng(2026),
                unitPriceLookup: id => id == "clean_water" ? 30f : 18f,
                displayNameLookup: id => id == "canned_food" ? "Canned Food" : "Clean Water");

            // Open rejects inactive factions like TradeScreenUI.Open.
            Assert.False(presenter.Open("unknown_nomads", "Nomads", "None", 1));
            Assert.True(presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1));

            // SetPlayerOffer / SetFactionAsk drive the fairness verdict.
            presenter.SetPlayerOffer("canned_food", 1);   // 18
            presenter.SetFactionAsk("clean_water", 1);    // 30
            Assert.Equal(TradeFairness.Short, presenter.ViewModel.Fairness);
            Assert.False(presenter.TryConfirmTrade());

            presenter.SetPlayerOffer("canned_food", 3);   // 54 >= 30
            Assert.Equal(TradeFairness.Fair, presenter.ViewModel.Fairness);
            Assert.True(presenter.TryConfirmTrade());

            // Successful confirm clears the table, like TradeScreenUI.
            Assert.Equal(TradeFairness.EmptyTable, presenter.ViewModel.Fairness);
            Assert.Empty(presenter.ViewModel.PlayerOffers);

            // BuildQuoteSummary is qualitative — no raw digit totals.
            presenter.SetPlayerOffer("canned_food", 2);
            presenter.SetFactionAsk("clean_water", 1);
            string summary = presenter.BuildQuoteSummary();
            Assert.Contains("DEAL IS FAIR", summary);
            Assert.Contains("Canned Food", summary);
            Assert.Contains("Clean Water", summary);
            Assert.DoesNotContain("36.0", summary);

            // Close collapses the open state.
            presenter.Close(traded: true);
            Assert.False(presenter.ViewModel.IsOpen);
        }

        [Fact]
        public void Presenter_BioOffersPricedByCoreRule()
        {
            var presenter = new TradeScreenPresenter(
                CreateStanceEngine(), null, LoadTells(), new SeededRng(2026),
                unitPriceLookup: _ => 0f);

            presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);
            presenter.SetBiologicalOffer(BiologicalTradeItem.Organ, 1);   // 4*25 = 100
            presenter.SetFactionAsk("clean_water", 3);                    // 0

            // Bio-only offer still counts toward the scale; demands worth nothing => fair.
            Assert.Equal(TradeFairness.Fair, presenter.ViewModel.Fairness);
            Assert.Equal(100f, presenter.ViewModel.PlayerOfferValue, 2);
        }

        [Fact]
        public void Presenter_RoutesExecutionThroughSink()
        {
            var recorder = new RecordingExecutionSink();
            var presenter = new TradeScreenPresenter(
                CreateStanceEngine(), null, LoadTells(), new SeededRng(2026),
                unitPriceLookup: _ => 10f,
                executionSink: recorder);

            presenter.Open("scavenger_camp", "Scavenger Camp", "Varek", 1);
            presenter.SetPlayerOffer("canned_food", 2);
            presenter.SetFactionAsk("clean_water", 1);
            Assert.True(presenter.TryConfirmTrade());

            Assert.Equal(1, recorder.ExecuteCalls);
            Assert.Equal("scavenger_camp", recorder.LastFactionId);
            Assert.Equal(2, recorder.LastPlayerOffers["canned_food"]);

            presenter.TryDemandParley();
            Assert.Equal(1, recorder.ParleyCalls);
        }

        private sealed class RecordingExecutionSink : ITradeExecutionSink
        {
            public int ExecuteCalls { get; private set; }
            public int ParleyCalls { get; private set; }
            public string LastFactionId { get; private set; }
            public IReadOnlyDictionary<string, int> LastPlayerOffers { get; private set; }

            public bool WillTrade(string factionId) => true;

            public bool TryExecuteTrade(
                string factionId,
                IReadOnlyDictionary<string, int> playerOffers,
                IReadOnlyDictionary<string, int> factionAsks,
                IReadOnlyDictionary<BiologicalTradeItem, int> biologicalOffers)
            {
                ExecuteCalls++;
                LastFactionId = factionId;
                LastPlayerOffers = playerOffers;
                return true;
            }

            public bool TryDemandParley(string factionId)
            {
                ParleyCalls++;
                return true;
            }
        }
    }
}
