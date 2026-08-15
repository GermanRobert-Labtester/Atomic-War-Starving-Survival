namespace Ashfall.Core.Economy
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;

    /// <summary>
    /// One data-defined Negotiation Table scenario (fair deal, offer short,
    /// empty table). JSON in StreamingAssets is the authority; nothing about
    /// a scenario is hardcoded in hosts.
    /// </summary>
    public sealed class TradeScreenScenario
    {
        public string Id { get; set; } = string.Empty;
        public string FactionId { get; set; } = string.Empty;
        public string FactionName { get; set; } = string.Empty;
        public string LeaderName { get; set; } = string.Empty;
        public int SuccessionGeneration { get; set; } = 1;
        public TradeStance Stance { get; set; } = TradeStance.Refuse;
        public float Trust { get; set; }
        public float Aggression { get; set; }
        public int ConsecutiveRepels { get; set; }
        public bool HasSurrendered { get; set; }
        public bool CanDemandParley { get; set; }
        public string WorldPhase { get; set; } = string.Empty;
        public int WorldDay { get; set; } = 1;
        public List<ShockBadgeData> PriceShocks { get; set; } = new();
        public List<ScarcityBandData> Scarcity { get; set; } = new();
        public List<TradeLineData> PlayerOffers { get; set; } = new();
        public List<TradeLineData> FactionDemands { get; set; } = new();
        public Dictionary<BiologicalTradeItem, int> BiologicalOffers { get; set; } = new();
        public TradeFairness ExpectedFairness { get; set; } = TradeFairness.EmptyTable;
        public bool ConfirmSucceeds { get; set; }
        public string RadioTicker { get; set; } = string.Empty;
    }

    /// <summary>Records intent routed through the mock sink (skin-track assertions).</summary>
    public sealed class MockTradeIntentSink : ITradeIntentSink
    {
        public int ConfirmCalls { get; private set; }
        public int ParleyCalls { get; private set; }
        public int CloseCalls { get; private set; }
        public bool? LastCloseWasTraded { get; private set; }
        public bool ConfirmResult { get; set; } = true;

        public bool TryConfirmTrade()
        {
            ConfirmCalls++;
            return ConfirmResult;
        }

        public bool TryDemandParley()
        {
            ParleyCalls++;
            return true;
        }

        public void Close(bool traded)
        {
            CloseCalls++;
            LastCloseWasTraded = traded;
        }
    }

    /// <summary>A mock binding: view-model + intent sink built from a scenario.</summary>
    public sealed class MockTradeScreenBinding
    {
        public TradeScreenScenario Scenario { get; }
        public TradeScreenViewModel ViewModel { get; }
        public MockTradeIntentSink Intents { get; }

        public MockTradeScreenBinding(TradeScreenScenario scenario, TradeScreenViewModel viewModel, MockTradeIntentSink intents)
        {
            Scenario = scenario;
            ViewModel = viewModel;
            Intents = intents;
        }
    }

    /// <summary>
    /// Loads trade screen scenarios from JSON and builds mock bindings for the
    /// skin track. Both tracks build against the same seam.
    /// </summary>
    public static class TradeScreenScenarioLoader
    {
        /// <summary>
        /// Parses { "scenarios": [ { id, faction_id, stance, trust, player_offers: [{item_id,
        /// display_name, quantity, unit_price}], biological_offers: {PintOfBlood: n},
        /// faction_demands: [...], expected_fairness, confirm_succeeds, ... } ] }.
        /// </summary>
        public static IReadOnlyList<TradeScreenScenario> LoadFromJson(string json)
        {
            var result = new List<TradeScreenScenario>();
            if (string.IsNullOrWhiteSpace(json)) return result;

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            if (!root.TryGetProperty("scenarios", out var scenariosEl) || scenariosEl.ValueKind != JsonValueKind.Array)
            {
                return result;
            }

            foreach (var s in scenariosEl.EnumerateArray())
            {
                var scenario = new TradeScreenScenario
                {
                    Id = GetString(s, "id"),
                    FactionId = GetString(s, "faction_id"),
                    FactionName = GetString(s, "faction_name"),
                    LeaderName = GetString(s, "leader_name"),
                    SuccessionGeneration = GetInt(s, "succession_generation", 1),
                    Stance = ParseStance(GetString(s, "stance")),
                    Trust = GetFloat(s, "trust", 0f),
                    Aggression = GetFloat(s, "aggression", 0f),
                    ConsecutiveRepels = GetInt(s, "consecutive_repels", 0),
                    HasSurrendered = GetBool(s, "has_surrendered", false),
                    CanDemandParley = GetBool(s, "can_demand_parley", false),
                    WorldPhase = GetString(s, "world_phase"),
                    WorldDay = GetInt(s, "world_day", 1),
                    ExpectedFairness = ParseFairness(GetString(s, "expected_fairness")),
                    ConfirmSucceeds = GetBool(s, "confirm_succeeds", false),
                    RadioTicker = GetString(s, "radio_ticker")
                };

                if (s.TryGetProperty("price_shocks", out var shocksEl) && shocksEl.ValueKind == JsonValueKind.Array)
                {
                    foreach (var sh in shocksEl.EnumerateArray())
                    {
                        scenario.PriceShocks.Add(new ShockBadgeData(
                            ParseShockKind(GetString(sh, "kind")),
                            GetFloat(sh, "multiplier", 1f),
                            GetString(sh, "note")));
                    }
                }

                if (s.TryGetProperty("scarcity", out var scarEl) && scarEl.ValueKind == JsonValueKind.Array)
                {
                    foreach (var sc in scarEl.EnumerateArray())
                    {
                        scenario.Scarcity.Add(new ScarcityBandData(
                            GetString(sc, "item_id"),
                            GetString(sc, "display_name"),
                            GetFloat(sc, "multiplier", 1f)));
                    }
                }

                if (s.TryGetProperty("player_offers", out var offersEl) && offersEl.ValueKind == JsonValueKind.Array)
                {
                    foreach (var o in offersEl.EnumerateArray())
                    {
                        scenario.PlayerOffers.Add(new TradeLineData(
                            GetString(o, "item_id"),
                            GetString(o, "display_name"),
                            GetInt(o, "quantity", 0),
                            GetFloat(o, "unit_price", 0f)));
                    }
                }

                if (s.TryGetProperty("faction_demands", out var demandsEl) && demandsEl.ValueKind == JsonValueKind.Array)
                {
                    foreach (var d in demandsEl.EnumerateArray())
                    {
                        scenario.FactionDemands.Add(new TradeLineData(
                            GetString(d, "item_id"),
                            GetString(d, "display_name"),
                            GetInt(d, "quantity", 0),
                            GetFloat(d, "unit_price", 0f)));
                    }
                }

                if (s.TryGetProperty("biological_offers", out var bioEl) && bioEl.ValueKind == JsonValueKind.Object)
                {
                    foreach (var b in bioEl.EnumerateObject())
                    {
                        if (Enum.TryParse<BiologicalTradeItem>(b.Name, ignoreCase: true, out var kind) && b.Value.ValueKind == JsonValueKind.Number)
                        {
                            scenario.BiologicalOffers[kind] = b.Value.GetInt32();
                        }
                    }
                }

                result.Add(scenario);
            }

            return result;
        }

        /// <summary>Builds the skin-track binding: a populated view-model + recording mock sink.</summary>
        public static MockTradeScreenBinding CreateBinding(TradeScreenScenario scenario, ITradeTellProvider tells, ISeededRng rng)
        {
            if (scenario == null) throw new ArgumentNullException(nameof(scenario));

            var vm = new TradeScreenViewModel();
            vm.SetOpen(true);
            vm.SetFaction(scenario.FactionId, scenario.FactionName, scenario.LeaderName, scenario.SuccessionGeneration);
            vm.SetStance(scenario.Stance);
            vm.SetMeters(scenario.Trust, scenario.Aggression);
            vm.SetFactionPresence(scenario.ConsecutiveRepels, scenario.HasSurrendered, scenario.CanDemandParley);
            vm.SetWorld(scenario.WorldPhase, scenario.WorldDay);
            vm.SetShockBadges(scenario.PriceShocks);
            vm.SetScarcityBands(scenario.Scarcity);

            bool willTrade = scenario.Stance == TradeStance.Trade || scenario.Stance == TradeStance.ShareIntel;
            vm.SetTable(scenario.PlayerOffers, scenario.FactionDemands, scenario.BiologicalOffers, willTrade);

            if (tells != null && tells.TrySelectTell(scenario.Stance, scenario.Trust, rng, out var tell))
            {
                vm.SetTell(tell.Id, tell.Line);
            }

            vm.SetRadioTicker(scenario.RadioTicker);

            var sink = new MockTradeIntentSink { ConfirmResult = scenario.ConfirmSucceeds };
            return new MockTradeScreenBinding(scenario, vm, sink);
        }

        // ── JSON helpers (invariant culture only) ────────────────────

        private static string GetString(JsonElement el, string prop)
        {
            return el.TryGetProperty(prop, out var v) && v.ValueKind == JsonValueKind.String
                ? v.GetString() ?? string.Empty
                : string.Empty;
        }

        private static int GetInt(JsonElement el, string prop, int fallback)
        {
            return el.TryGetProperty(prop, out var v) && v.ValueKind == JsonValueKind.Number
                ? v.GetInt32()
                : fallback;
        }

        private static float GetFloat(JsonElement el, string prop, float fallback)
        {
            return el.TryGetProperty(prop, out var v) && v.ValueKind == JsonValueKind.Number
                ? (float)v.GetDouble()
                : fallback;
        }

        private static bool GetBool(JsonElement el, string prop, bool fallback)
        {
            if (!el.TryGetProperty(prop, out var v)) return fallback;
            if (v.ValueKind == JsonValueKind.True) return true;
            if (v.ValueKind == JsonValueKind.False) return false;
            return fallback;
        }

        private static TradeStance ParseStance(string key)
        {
            switch ((key ?? string.Empty).Trim())
            {
                case "hostile_raid":
                case "HostileRaid": return TradeStance.HostileRaid;
                case "rob":
                case "Rob": return TradeStance.Rob;
                case "trade":
                case "Trade": return TradeStance.Trade;
                case "share_intel":
                case "ShareIntel": return TradeStance.ShareIntel;
                default: return TradeStance.Refuse;
            }
        }

        private static TradeFairness ParseFairness(string key)
        {
            switch ((key ?? string.Empty).Trim())
            {
                case "fair": return TradeFairness.Fair;
                case "short": return TradeFairness.Short;
                default: return TradeFairness.EmptyTable;
            }
        }

        private static PriceShockKind ParseShockKind(string key)
        {
            switch ((key ?? string.Empty).Trim())
            {
                case "ConvoyAmbush": return PriceShockKind.ConvoyAmbush;
                case "FactionWar": return PriceShockKind.FactionWar;
                case "WinterDeepens": return PriceShockKind.WinterDeepens;
                default: return PriceShockKind.PlumePassing;
            }
        }
    }
}
