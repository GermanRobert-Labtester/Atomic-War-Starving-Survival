// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class TradeTextCatalogTests
    {
        private static string DataDir
        {
            get
            {
                string[] candidates =
                {
                    Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data"),
                    Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data")
                };
                foreach (string candidate in candidates)
                    if (Directory.Exists(candidate)) return candidate;
                throw new DirectoryNotFoundException("Could not locate Assets/StreamingAssets/Data.");
            }
        }

        [Fact]
        public void AuthoredCatalog_LoadsNineProfilesAndScenarioPresentation()
        {
            var result = TradeTextCatalogLoader.Load(
                DataDir,
                new FileSystemIO(),
                new SystemTextJsonSerializer());

            Assert.True(result.IsValid, string.Join("; ", result.Errors));
            Assert.True(result.LoadedFromFile);
            Assert.False(result.UsedFallback);
            Assert.Equal(9, result.Catalog.TraderCount);
            Assert.Equal(8, result.Catalog.ScenarioCount);
            Assert.Contains("trader_foundry_broker", result.Catalog.Traders.Keys);
            Assert.Contains("fair_deal", result.Catalog.Scenarios.Keys);
        }

        [Fact]
        public void AuthoredCatalog_HasAllRequiredFamiliesAndNoUnknownPlaceholders()
        {
            var catalog = Load().Catalog;
            var required = new Dictionary<string, string[]>
            {
                ["greetings"] = new[] { "hostile", "wary", "neutral", "warm" },
                ["item_examinations"] = new[] { "valuable", "worthless", "interesting", "dangerous" },
                ["offers"] = new[] { "fair", "generous", "stingy", "desperate" },
                ["counter_offers"] = new[] { "accept", "reject", "negotiate", "insulted" },
                ["acceptance"] = new[] { "pleased", "relieved", "indifferent", "suspicious" },
                ["rejection"] = new[] { "polite", "annoyed", "angry", "sad" },
                ["regret"] = new[] { "buyer", "seller", "mutual" },
                ["insult"] = new[] { "mild", "moderate", "severe" },
                ["flattery"] = new[] { "subtle", "obvious", "excessive" },
                ["threat"] = new[] { "veiled", "direct", "desperate" }
            };

            foreach (var trader in catalog.Traders.Values)
            {
                foreach (var family in required)
                {
                    var lines = Family(trader, family.Key);
                    Assert.NotNull(lines);
                    foreach (string key in family.Value)
                    {
                        Assert.True(lines!.TryGetValue(key, out string text), $"{trader.id}: {family.Key}.{key}");
                        Assert.False(string.IsNullOrWhiteSpace(text));
                        Assert.DoesNotContain("{", text);
                        Assert.DoesNotContain("}", text);
                        Assert.DoesNotContain("[item]", text.Replace("[item]", string.Empty, StringComparison.Ordinal));
                    }
                }
            }
        }

        [Fact]
        public void Resolver_TrustBoundaryTable_UsesCanonicalBands()
        {
            var resolver = new TradeVoiceResolver(Load().Catalog);
            var failures = new List<string>();

            foreach (var testCase in new[]
            {
                (Trust: -40f, ExpectedBand: "hostile"),
                (Trust: -39f, ExpectedBand: "wary"),
                (Trust: 0f, ExpectedBand: "wary"),
                (Trust: 1f, ExpectedBand: "neutral"),
                (Trust: 40f, ExpectedBand: "neutral"),
                (Trust: 41f, ExpectedBand: "warm"),
            })
            {
                var result = resolver.ResolveGreeting(new TradeVoiceContext
                {
                    TraderProfileId = "trader_foundry_broker",
                    Trust = testCase.Trust
                });

                var exception = Record.Exception(() =>
                {
                    Assert.Equal("trader_foundry_broker", result.ProfileId);
                    Assert.Equal(testCase.ExpectedBand, result.Band);
                    Assert.False(string.IsNullOrWhiteSpace(result.Text));
                    Assert.False(result.UsedFallback);
                });
                if (exception != null)
                {
                    failures.Add($"trust={testCase.Trust}: {exception.Message}");
                }
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void Resolver_PrefersExplicitProfileThenScenarioCaravanFactionAndRegion()
        {
            var resolver = new TradeVoiceResolver(Load().Catalog);

            Assert.Equal(
                "trader_medical_supplier",
                resolver.ResolveProfileId(new TradeVoiceContext
                {
                    TraderProfileId = "trader_medical_supplier",
                    FactionId = "faction_silent_foundry"
                }));
            Assert.Equal(
                "trader_quartermaster",
                resolver.ResolveProfileId(new TradeVoiceContext { ScenarioId = "depot_window" }));
            Assert.Equal(
                "trader_medical_supplier",
                resolver.ResolveProfileId(new TradeVoiceContext { CaravanId = "caravan_medic_syndicate" }));
            Assert.Equal(
                "trader_medical_supplier",
                resolver.ResolveProfileId(new TradeVoiceContext { FactionId = "faction_wandering_menders" }));
            Assert.Equal(
                "trader_bulk_dealer",
                resolver.ResolveProfileId(new TradeVoiceContext { CaravanId = "caravan_permafrost_traders" }));
            Assert.Equal(
                "trader_foundry_broker",
                resolver.ResolveProfileId(new TradeVoiceContext { FactionId = "faction_silent_foundry" }));
            Assert.Equal(
                "trader_flotilla_salvager",
                resolver.ResolveProfileId(new TradeVoiceContext { CaravanOriginRegion = "deep_coast" }));
            Assert.Equal(
                TradeVoiceResolver.DefaultProfileId,
                resolver.ResolveProfileId(new TradeVoiceContext { FactionId = "unknown_faction" }));
        }

        [Fact]
        public void Resolver_FormatsCanonicalDisplayNamesAndRejectsUnsafeTokens()
        {
            var resolver = new TradeVoiceResolver(Load().Catalog);
            var result = resolver.ResolveLine(
                new TradeVoiceContext { TraderProfileId = "trader_merchant" },
                TradeVoiceLineFamily.Offer,
                "fair",
                "Clean [Water]",
                "Fuel {sealed}");

            Assert.Contains("Clean  Water", result.Text);
            Assert.Contains("Fuel  sealed", result.Text);
            Assert.DoesNotContain("[item]", result.Text);
            Assert.DoesNotContain("{", result.Text);
            Assert.DoesNotContain("}", result.Text);
        }

        [Fact]
        public void Resolver_ReadsScenarioPresentationWithoutChangingScenarioAuthority()
        {
            var resolver = new TradeVoiceResolver(Load().Catalog);
            var result = resolver.ResolveScenarioTraderText(
                new TradeVoiceContext { ScenarioId = "fair_deal", Trust = 20f },
                "fair_deal");

            Assert.False(result.UsedFallback);
            Assert.Contains("fair deal", result.Text, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Presenter_VoiceProjectionDoesNotChangeTradeReadModel()
        {
            var plainSink = new RecordingExecutionSink();
            var voicedSink = new RecordingExecutionSink();
            var withoutVoice = new TradeScreenPresenter(
                new StaticStanceProvider(12f),
                unitPriceLookup: _ => 10f,
                executionSink: plainSink);
            var withVoice = new TradeScreenPresenter(
                new StaticStanceProvider(12f),
                unitPriceLookup: _ => 10f,
                executionSink: voicedSink,
                voiceResolver: new TradeVoiceResolver(Load().Catalog));

            withoutVoice.Open("faction_silent_foundry", "Foundry", "Broker", 1);
            withVoice.Open("faction_silent_foundry", "Foundry", "Broker", 1);
            withVoice.SetVoiceContext(new TradeVoiceContext
            {
                FactionId = "faction_silent_foundry",
                StableContextKey = "trade-rebind"
            });
            withoutVoice.SetPlayerOffer("item_clean_water", 2);
            withoutVoice.SetFactionAsk("item_canned_food", 1);
            withVoice.SetPlayerOffer("item_clean_water", 2);
            withVoice.SetFactionAsk("item_canned_food", 1);

            Assert.Equal(withoutVoice.ViewModel.PlayerOfferValue, withVoice.ViewModel.PlayerOfferValue);
            Assert.Equal(withoutVoice.ViewModel.FactionAskValue, withVoice.ViewModel.FactionAskValue);
            Assert.Equal(withoutVoice.ViewModel.Fairness, withVoice.ViewModel.Fairness);
            Assert.Equal(withoutVoice.ViewModel.CanConfirm, withVoice.ViewModel.CanConfirm);
            Assert.Equal("trader_foundry_broker", withVoice.ViewModel.TraderProfileId);
            Assert.False(string.IsNullOrWhiteSpace(withVoice.ViewModel.TraderVoiceLine));

            Assert.True(withoutVoice.TryConfirmTrade());
            Assert.True(withVoice.TryConfirmTrade());
            Assert.Equal(plainSink.LastPlayerOffers["item_clean_water"], voicedSink.LastPlayerOffers["item_clean_water"]);
            Assert.Equal(plainSink.LastFactionAsks["item_canned_food"], voicedSink.LastFactionAsks["item_canned_food"]);
            Assert.Equal(plainSink.ExecuteCalls, voicedSink.ExecuteCalls);
        }

        [Fact]
        public void Resolver_RebindingTheSameCaravanContextKeepsTheSameProfileAndLine()
        {
            var resolver = new TradeVoiceResolver(Load().Catalog);
            var context = new TradeVoiceContext
            {
                CaravanId = "caravan_medic_syndicate",
                CaravanOriginRegion = "settlement",
                FactionId = "faction_wandering_menders",
                StableContextKey = "caravan_medic_syndicate",
                Trust = 0f
            };

            var first = resolver.ResolveGreeting(context);
            var second = resolver.ResolveGreeting(context);

            Assert.Equal("trader_medical_supplier", first.ProfileId);
            Assert.Equal(first.ProfileId, second.ProfileId);
            Assert.Equal(first.Text, second.Text);
        }

        [Fact]
        public void MissingCatalog_UsesGenericFallbackWithoutTradeFailure()
        {
            var result = TradeTextCatalogLoader.Load(
                "memory",
                new MemoryFileIO(),
                new SystemTextJsonSerializer());

            Assert.False(result.IsValid);
            Assert.False(result.LoadedFromFile);
            Assert.True(result.UsedFallback);
            Assert.Single(result.Catalog.Traders);

            var voice = new TradeVoiceResolver(result.Catalog).ResolveGreeting(
                new TradeVoiceContext { Trust = 41f });
            Assert.True(voice.UsedFallback);
            Assert.False(string.IsNullOrWhiteSpace(voice.Text));
        }

        [Fact]
        public void MalformedOrDuplicateCatalog_FallsBackAndReportsErrors()
        {
            const string duplicate = """
            {
              "schema_version": 1,
              "collection_id": "trade_texts",
              "traders": [
                { "id": "trader_one", "display_name": "One" },
                { "id": "trader_one", "display_name": "Duplicate" }
              ],
              "trade_scenarios": {}
            }
            """;

            var result = TradeTextCatalogLoader.LoadFromJson(duplicate);

            Assert.True(result.UsedFallback);
            Assert.Contains(result.Errors, error => error.Contains("Duplicate", StringComparison.Ordinal));
            Assert.Single(result.Catalog.Traders);
        }

        private static TradeTextCatalogLoadResult Load() =>
            TradeTextCatalogLoader.Load(
                DataDir,
                new FileSystemIO(),
                new SystemTextJsonSerializer());

        private static Dictionary<string, string> Family(TradeTextTraderDefinition trader, string family) =>
            family switch
            {
                "greetings" => trader.greetings,
                "item_examinations" => trader.item_examinations,
                "offers" => trader.offers,
                "counter_offers" => trader.counter_offers,
                "acceptance" => trader.acceptance,
                "rejection" => trader.rejection,
                "regret" => trader.regret,
                "insult" => trader.insult,
                "flattery" => trader.flattery,
                "threat" => trader.threat,
                _ => new Dictionary<string, string>()
            };

        private sealed class MemoryFileIO : IFileIO
        {
            public bool DirectoryExists(string path) => true;
            public bool FileExists(string path) => false;
            public string ReadAllText(string path) => string.Empty;
            public void WriteAllText(string path, string contents) { }
            public string Combine(params string[] parts) => string.Join("/", parts);
        }

        private sealed class StaticStanceProvider : IFactionStanceProvider
        {
            private readonly float _trust;

            public StaticStanceProvider(float trust) => _trust = trust;

            public TradeStance GetStance(string factionId) => TradeStance.Trade;
            public bool WillTrade(string factionId) => true;
            public bool WillShareIntel(string factionId) => false;
            public float GetTrust(string factionId) => _trust;
            public float GetEffectiveTrust(string factionId) => _trust;
            public float ModifyTrust(string factionId, float delta) => _trust + delta;
            public void SetTrust(string factionId, float value) { }
            public float GetRaidAggression(string factionId) => 0.1f;
            public void SetRaidAggression(string factionId, float value) { }
            public bool IsFactionActive(string factionId) => true;
        }

        private sealed class RecordingExecutionSink : ITradeExecutionSink
        {
            public int ExecuteCalls { get; private set; }
            public IReadOnlyDictionary<string, int> LastPlayerOffers { get; private set; } =
                new Dictionary<string, int>();
            public IReadOnlyDictionary<string, int> LastFactionAsks { get; private set; } =
                new Dictionary<string, int>();

            public bool WillTrade(string factionId) => true;

            public bool TryExecuteTrade(
                string factionId,
                IReadOnlyDictionary<string, int> playerOffers,
                IReadOnlyDictionary<string, int> factionAsks,
                IReadOnlyDictionary<BiologicalTradeItem, int> biologicalOffers)
            {
                ExecuteCalls++;
                LastPlayerOffers = new Dictionary<string, int>(playerOffers);
                LastFactionAsks = new Dictionary<string, int>(factionAsks);
                return true;
            }

            public bool TryDemandParley(string factionId) => true;
        }
    }
}
