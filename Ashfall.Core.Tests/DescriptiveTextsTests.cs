using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.IO;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Tests for item description texts, medical texts, and trade texts JSON files.
    /// Verifies that the new descriptive text files load correctly and have valid structure.
    /// </summary>
    public class DescriptiveTextsTests : CatalogTestBase
    {
        private static readonly SystemTextJsonSerializer s_serializer = new SystemTextJsonSerializer();

        // ─── Item Description Texts ──────────────────────────────────────────────

        [Fact]
        public void ItemDescriptionTexts_LoadsCorrectly()
        {
            string path = Path.Combine(DataDirectory, "item_description_texts.json");
            if (!File.Exists(path))
            {
                // File doesn't exist yet — skip
                return;
            }

            string json = File.ReadAllText(path);
            Assert.False(string.IsNullOrWhiteSpace(json), "item_description_texts.json is empty");

            var root = s_serializer.Deserialize<ItemDescriptionTextsRoot>(json);
            Assert.NotNull(root);
            Assert.Equal(1, root.schema_version);
            Assert.Equal("item_description_texts", root.collection_id);
            Assert.NotEmpty(root.descriptions);
        }

        [Fact]
        public void ItemDescriptionTexts_HasRequiredFields()
        {
            string path = Path.Combine(DataDirectory, "item_description_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<ItemDescriptionTextsRoot>(json);

            foreach (var desc in root.descriptions)
            {
                Assert.False(string.IsNullOrWhiteSpace(desc.item_id), "Item description has empty item_id");
                Assert.False(string.IsNullOrWhiteSpace(desc.category), $"Item {desc.item_id} has empty category");
                Assert.False(string.IsNullOrWhiteSpace(desc.base_description), $"Item {desc.item_id} has empty base_description");
                Assert.False(string.IsNullOrWhiteSpace(desc.current_state), $"Item {desc.item_id} has empty current_state");
                Assert.False(string.IsNullOrWhiteSpace(desc.visual_indicators), $"Item {desc.item_id} has empty visual_indicators");
                Assert.False(string.IsNullOrWhiteSpace(desc.functional_description), $"Item {desc.item_id} has empty functional_description");
                Assert.False(string.IsNullOrWhiteSpace(desc.sensory_details), $"Item {desc.item_id} has empty sensory_details");
                Assert.False(string.IsNullOrWhiteSpace(desc.emotional_weight), $"Item {desc.item_id} has empty emotional_weight");
            }
        }

        [Fact]
        public void ItemDescriptionTexts_HasMinimumCount()
        {
            string path = Path.Combine(DataDirectory, "item_description_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<ItemDescriptionTextsRoot>(json);

            Assert.True(root.descriptions.Count >= 175, $"Expected at least 175 item descriptions, got {root.descriptions.Count}");
        }

        [Fact]
        public void ItemDescriptionTexts_CoversAllCategories()
        {
            string path = Path.Combine(DataDirectory, "item_description_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<ItemDescriptionTextsRoot>(json);

            var categories = root.descriptions.Select(d => d.category).Distinct().ToList();
            Assert.Contains("device", categories);
            Assert.Contains("medical", categories);
            Assert.Contains("protective", categories);
            Assert.Contains("tool", categories);
            Assert.Contains("water", categories);
            Assert.Contains("food", categories);
            Assert.Contains("fuel", categories);
            Assert.Contains("material", categories);
        }

        // ─── Medical Texts ───────────────────────────────────────────────────────

        [Fact]
        public void MedicalTexts_LoadsCorrectly()
        {
            string path = Path.Combine(DataDirectory, "medical_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            Assert.False(string.IsNullOrWhiteSpace(json), "medical_texts.json is empty");

            var root = s_serializer.Deserialize<MedicalTextsRoot>(json);
            Assert.NotNull(root);
            Assert.Equal(1, root.schema_version);
            Assert.Equal("medical_texts", root.collection_id);
            Assert.NotEmpty(root.conditions);
        }

        [Fact]
        public void MedicalTexts_HasRequiredFields()
        {
            string path = Path.Combine(DataDirectory, "medical_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<MedicalTextsRoot>(json);

            foreach (var condition in root.conditions)
            {
                Assert.False(string.IsNullOrWhiteSpace(condition.id), "Medical condition has empty id");
                Assert.False(string.IsNullOrWhiteSpace(condition.category), $"Condition {condition.id} has empty category");
                Assert.False(string.IsNullOrWhiteSpace(condition.display_name), $"Condition {condition.id} has empty display_name");
                Assert.False(string.IsNullOrWhiteSpace(condition.diagnosis_text), $"Condition {condition.id} has empty diagnosis_text");
                Assert.NotEmpty(condition.symptom_descriptions);
                Assert.NotEmpty(condition.treatment_steps);
                Assert.NotEmpty(condition.pain_descriptions);
            }
        }

        [Fact]
        public void MedicalTexts_HasMinimumCount()
        {
            string path = Path.Combine(DataDirectory, "medical_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<MedicalTextsRoot>(json);

            Assert.True(root.conditions.Count >= 80, $"Expected at least 80 medical conditions, got {root.conditions.Count}");
        }

        [Fact]
        public void MedicalTexts_CoversAllCategories()
        {
            string path = Path.Combine(DataDirectory, "medical_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<MedicalTextsRoot>(json);

            var categories = root.conditions.Select(c => c.category).Distinct().ToList();
            Assert.Contains("injury", categories);
            Assert.Contains("illness", categories);
            Assert.Contains("mental", categories);
            Assert.Contains("complication", categories);
            Assert.Contains("emergency", categories);
        }

        // ─── Trade Texts ─────────────────────────────────────────────────────────

        [Fact]
        public void TradeTexts_LoadsCorrectly()
        {
            string path = Path.Combine(DataDirectory, "trade_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            Assert.False(string.IsNullOrWhiteSpace(json), "trade_texts.json is empty");

            var root = s_serializer.Deserialize<TradeTextsRoot>(json);
            Assert.NotNull(root);
            Assert.Equal(1, root.schema_version);
            Assert.Equal("trade_texts", root.collection_id);
            Assert.NotEmpty(root.traders);
        }

        [Fact]
        public void TradeTexts_HasRequiredFields()
        {
            string path = Path.Combine(DataDirectory, "trade_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<TradeTextsRoot>(json);

            foreach (var trader in root.traders)
            {
                Assert.False(string.IsNullOrWhiteSpace(trader.id), "Trader has empty id");
                Assert.False(string.IsNullOrWhiteSpace(trader.display_name), $"Trader {trader.id} has empty display_name");
                Assert.False(string.IsNullOrWhiteSpace(trader.profile), $"Trader {trader.id} has empty profile");
                Assert.NotNull(trader.greetings);
                Assert.NotNull(trader.item_examinations);
                Assert.NotNull(trader.offers);
                Assert.NotNull(trader.counter_offers);
                Assert.NotNull(trader.acceptance);
                Assert.NotNull(trader.rejection);
            }
        }

        [Fact]
        public void TradeTexts_HasMinimumTraders()
        {
            string path = Path.Combine(DataDirectory, "trade_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<TradeTextsRoot>(json);

            Assert.True(root.traders.Count >= 4, $"Expected at least 4 traders, got {root.traders.Count}");
        }

        [Fact]
        public void TradeTexts_HasTradeScenarios()
        {
            string path = Path.Combine(DataDirectory, "trade_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<TradeTextsRoot>(json);

            Assert.NotNull(root.trade_scenarios);
            Assert.True(root.trade_scenarios.Count >= 8, $"Expected at least 8 trade scenarios, got {root.trade_scenarios.Count}");
        }

        [Fact]
        public void TradeTexts_HasItemValuations()
        {
            string path = Path.Combine(DataDirectory, "trade_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<TradeTextsRoot>(json);

            Assert.NotNull(root.item_valuations);
            Assert.True(root.item_valuations.Count >= 5, $"Expected at least 5 item valuations, got {root.item_valuations.Count}");
        }

        [Fact]
        public void TradeTexts_HasSeasonalEffects()
        {
            string path = Path.Combine(DataDirectory, "trade_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<TradeTextsRoot>(json);

            Assert.NotNull(root.seasonal_effects);
            Assert.True(root.seasonal_effects.Count >= 3, $"Expected at least 3 seasonal effects, got {root.seasonal_effects.Count}");
        }

        [Fact]
        public void TradeTexts_HasFactionPreferences()
        {
            string path = Path.Combine(DataDirectory, "trade_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<TradeTextsRoot>(json);

            Assert.NotNull(root.faction_preferences);
            Assert.True(root.faction_preferences.Count >= 5, $"Expected at least 5 faction preferences, got {root.faction_preferences.Count}");
        }

        [Fact]
        public void TradeTexts_HasNegotiationTactics()
        {
            string path = Path.Combine(DataDirectory, "trade_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<TradeTextsRoot>(json);

            Assert.NotNull(root.negotiation_tactics);
            Assert.True(root.negotiation_tactics.Count >= 10, $"Expected at least 10 negotiation tactics, got {root.negotiation_tactics.Count}");
        }

        [Fact]
        public void TradeTexts_HasBlackMarket()
        {
            string path = Path.Combine(DataDirectory, "trade_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<TradeTextsRoot>(json);

            Assert.NotNull(root.black_market);
            Assert.NotNull(root.black_market.items);
            Assert.True(root.black_market.items.Count >= 3, $"Expected at least 3 black market item types, got {root.black_market.items.Count}");
        }

        [Fact]
        public void TradeTexts_HasBarterSystem()
        {
            string path = Path.Combine(DataDirectory, "trade_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<TradeTextsRoot>(json);

            Assert.NotNull(root.barter_system);
            Assert.NotNull(root.barter_system.rules);
            Assert.True(root.barter_system.rules.Count >= 10, $"Expected at least 10 barter rules, got {root.barter_system.rules.Count}");
        }

        [Fact]
        public void TradeTexts_HasTradeEtiquette()
        {
            string path = Path.Combine(DataDirectory, "trade_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<TradeTextsRoot>(json);

            Assert.NotNull(root.trade_etiquette);
            Assert.NotNull(root.trade_etiquette.rules);
            Assert.True(root.trade_etiquette.rules.Count >= 10, $"Expected at least 10 trade etiquette rules, got {root.trade_etiquette.rules.Count}");
        }

        [Fact]
        public void TradeTexts_HasTradeGuilds()
        {
            string path = Path.Combine(DataDirectory, "trade_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<TradeTextsRoot>(json);

            Assert.NotNull(root.trade_guilds);
            Assert.NotNull(root.trade_guilds.guilds);
            Assert.True(root.trade_guilds.guilds.Count >= 5, $"Expected at least 5 trade guilds, got {root.trade_guilds.guilds.Count}");
        }

        [Fact]
        public void TradeTexts_HasTradeLaws()
        {
            string path = Path.Combine(DataDirectory, "trade_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<TradeTextsRoot>(json);

            Assert.NotNull(root.trade_laws);
            Assert.NotNull(root.trade_laws.laws);
            Assert.True(root.trade_laws.laws.Count >= 10, $"Expected at least 10 trade laws, got {root.trade_laws.laws.Count}");
        }

        [Fact]
        public void TradeTexts_HasTradeOrganizations()
        {
            string path = Path.Combine(DataDirectory, "trade_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<TradeTextsRoot>(json);

            Assert.NotNull(root.trade_organizations);
            Assert.NotNull(root.trade_organizations.organizations);
            Assert.True(root.trade_organizations.organizations.Count >= 5, $"Expected at least 5 trade organizations, got {root.trade_organizations.organizations.Count}");
        }

        [Fact]
        public void TradeTexts_HasTradeDisputes()
        {
            string path = Path.Combine(DataDirectory, "trade_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<TradeTextsRoot>(json);

            Assert.NotNull(root.trade_disputes);
            Assert.NotNull(root.trade_disputes.disputes);
            Assert.True(root.trade_disputes.disputes.Count >= 5, $"Expected at least 5 trade disputes, got {root.trade_disputes.disputes.Count}");
        }

        [Fact]
        public void TradeTexts_HasTradeSkills()
        {
            string path = Path.Combine(DataDirectory, "trade_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<TradeTextsRoot>(json);

            Assert.NotNull(root.trade_skills);
            Assert.NotNull(root.trade_skills.skills);
            Assert.True(root.trade_skills.skills.Count >= 10, $"Expected at least 10 trade skills, got {root.trade_skills.skills.Count}");
        }

        [Fact]
        public void TradeTexts_HasTradeTraining()
        {
            string path = Path.Combine(DataDirectory, "trade_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<TradeTextsRoot>(json);

            Assert.NotNull(root.trade_training);
            Assert.NotNull(root.trade_training.training);
            Assert.True(root.trade_training.training.Count >= 5, $"Expected at least 5 trade training methods, got {root.trade_training.training.Count}");
        }

        [Fact]
        public void TradeTexts_HasTradeStrategies()
        {
            string path = Path.Combine(DataDirectory, "trade_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<TradeTextsRoot>(json);

            Assert.NotNull(root.trade_strategies);
            Assert.NotNull(root.trade_strategies.strategies);
            Assert.True(root.trade_strategies.strategies.Count >= 5, $"Expected at least 5 trade strategies, got {root.trade_strategies.strategies.Count}");
        }

        [Fact]
        public void TradeTexts_HasTradeRelationships()
        {
            string path = Path.Combine(DataDirectory, "trade_texts.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var root = s_serializer.Deserialize<TradeTextsRoot>(json);

            Assert.NotNull(root.trade_relationships);
            Assert.NotNull(root.trade_relationships.relationships);
            Assert.True(root.trade_relationships.relationships.Count >= 5, $"Expected at least 5 trade relationships, got {root.trade_relationships.relationships.Count}");
        }

        // ─── DTOs ────────────────────────────────────────────────────────────────

        [Serializable]
        public class ItemDescriptionTextsRoot
        {
            public int schema_version;
            public string collection_id;
            public List<ItemDescriptionEntry> descriptions;
        }

        [Serializable]
        public class ItemDescriptionEntry
        {
            public string item_id;
            public string category;
            public string base_description;
            public string current_state;
            public string visual_indicators;
            public string functional_description;
            public string sensory_details;
            public string emotional_weight;
            public string hazards;
            public string dependencies;
            public string contamination_status;
            public string preservation_state;
            public string makeshift_utility;
            public string alternates;
            public string system_integration;
        }

        [Serializable]
        public class MedicalTextsRoot
        {
            public int schema_version;
            public string collection_id;
            public List<MedicalConditionEntry> conditions;
        }

        [Serializable]
        public class MedicalConditionEntry
        {
            public string id;
            public string category;
            public string display_name;
            public string diagnosis_text;
            public List<string> symptom_descriptions;
            public List<string> treatment_steps;
            public List<string> required_items;
            public Dictionary<string, double> success_chances;
            public List<string> failure_consequences;
            public List<string> recovery_descriptions;
            public List<string> complication_warnings;
            public List<string> prevention_advice;
            public List<string> long_term_effects;
            public List<string> pain_descriptions;
            public string mental_state;
            public string physical_state;
            public string emotional_impact;
            public string system_integration;
        }

        [Serializable]
        public class TradeTextsRoot
        {
            public int schema_version;
            public string collection_id;
            public List<TraderEntry> traders;
            public Dictionary<string, TradeScenarioEntry> trade_scenarios;
            public Dictionary<string, ItemValuationEntry> item_valuations;
            public Dictionary<string, PriceShockEntry> price_shock_texts;
            public Dictionary<string, ScarcityEntry> scarcity_texts;
            public Dictionary<string, SeasonalEffectEntry> seasonal_effects;
            public Dictionary<string, FactionPreferenceEntry> faction_preferences;
            public Dictionary<string, TradeEventEntry> trade_events;
            public Dictionary<string, TradeTipEntry> trade_tips;
            public Dictionary<string, NegotiationTacticEntry> negotiation_tactics;
            public BlackMarketEntry black_market;
            public Dictionary<string, TradeRouteEntry> trade_routes;
            public TradeReputationEntry trade_reputation;
            public Dictionary<string, TradeDisasterEntry> trade_disasters;
            public BarterSystemEntry barter_system;
            public TradeEtiquetteEntry trade_etiquette;
            public MarketAnalysisEntry market_analysis;
            public TradePsychologyEntry trade_psychology;
            public TradeSecretsEntry trade_secrets;
            public TradeMythsEntry trade_myths;
            public TradeLegendsEntry trade_legends;
            public TradeGuildsEntry trade_guilds;
            public TradeLawsEntry trade_laws;
            public TradeHistoryEntry trade_history;
            public TradeTechnologyEntry trade_technology;
            public TradeCultureEntry trade_culture;
            public TradePhilosophyEntry trade_philosophy;
            public TradeFutureEntry trade_future;
            public TradeOrganizationsEntry trade_organizations;
            public TradeDisputesEntry trade_disputes;
            public TradeAgreementsEntry trade_agreements;
            public TradeCeremoniesEntry trade_ceremonies;
            public TradeSuperstitionsEntry trade_superstitions;
            public TradeProverbsEntry trade_proverbs;
            public TradeJokesEntry trade_jokes;
            public TradeRiddlesEntry trade_riddles;
            public TradeSkillsEntry trade_skills;
            public TradeTrainingEntry trade_training;
            public TradeCertificationsEntry trade_certifications;
            public TradeMarketsEntry trade_markets;
            public TradeCompetitionEntry trade_competition;
            public TradeInnovationEntry trade_innovation;
            public TradeSustainabilityEntry trade_sustainability;
            public TradeStrategiesEntry trade_strategies;
            public TradeRelationshipsEntry trade_relationships;
            public TradeHistoryEventsEntry trade_history_events;
            public TradeWisdomEntry trade_wisdom;
            public TradeChallengesEntry trade_challenges;
            public TradeSuccessStoriesEntry trade_success_stories;
        }

        [Serializable]
        public class TraderEntry
        {
            public string id;
            public string display_name;
            public string profile;
            public Dictionary<string, string> greetings;
            public Dictionary<string, string> item_examinations;
            public Dictionary<string, string> offers;
            public Dictionary<string, string> counter_offers;
            public Dictionary<string, string> acceptance;
            public Dictionary<string, string> rejection;
            public Dictionary<string, string> regret;
            public Dictionary<string, string> insult;
            public Dictionary<string, string> flattery;
            public Dictionary<string, string> threat;
        }

        [Serializable]
        public class TradeScenarioEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
        }

        [Serializable]
        public class ItemValuationEntry
        {
            public List<string> items;
            public string description;
            public string trader_text;
        }

        [Serializable]
        public class PriceShockEntry
        {
            public string description;
            public string trader_text;
        }

        [Serializable]
        public class ScarcityEntry
        {
            public string description;
            public string trader_text;
        }

        [Serializable]
        public class SeasonalEffectEntry
        {
            public string description;
            public double price_multiplier;
            public List<string> high_demand_items;
            public List<string> low_demand_items;
            public string trader_text;
        }

        [Serializable]
        public class FactionPreferenceEntry
        {
            public string display_name;
            public string description;
            public List<string> buys_at_premium;
            public List<string> refuses;
            public string trade_currency;
            public string trader_text;
        }

        [Serializable]
        public class TradeEventEntry
        {
            public string description;
            public int duration_days;
            public double price_multiplier;
            public List<string> high_demand_items;
            public string trader_text;
        }

        [Serializable]
        public class TradeTipEntry
        {
            public string description;
            public string trader_text;
        }

        [Serializable]
        public class NegotiationTacticEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
        }

        [Serializable]
        public class BlackMarketEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, BlackMarketItemEntry> items;
            public Dictionary<string, string> risks;
        }

        [Serializable]
        public class BlackMarketItemEntry
        {
            public string description;
            public string trader_text;
        }

        [Serializable]
        public class TradeRouteEntry
        {
            public string description;
            public string trader_text;
            public List<string> dangers;
            public List<string> rewards;
        }

        [Serializable]
        public class TradeReputationEntry
        {
            public Dictionary<string, TradeReputationLevel> levels;
            public Dictionary<string, string> effects;
        }

        [Serializable]
        public class TradeReputationLevel
        {
            public string description;
            public string trader_text;
        }

        [Serializable]
        public class TradeDisasterEntry
        {
            public string description;
            public string trader_text;
            public string effect;
        }

        [Serializable]
        public class BarterSystemEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, string> rules;
            public Dictionary<string, string> common_goods;
        }

        [Serializable]
        public class TradeEtiquetteEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, string> rules;
        }

        [Serializable]
        public class MarketAnalysisEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, string> factors;
        }

        [Serializable]
        public class TradePsychologyEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, string> trader_types;
        }

        [Serializable]
        public class TradeSecretsEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, string> secrets;
        }

        [Serializable]
        public class TradeMythsEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, string> myths;
        }

        [Serializable]
        public class TradeLegendsEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, string> legends;
        }

        [Serializable]
        public class TradeGuildsEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, TradeGuildEntry> guilds;
        }

        [Serializable]
        public class TradeGuildEntry
        {
            public string description;
            public string trader_text;
            public string requirements;
            public string benefits;
        }

        [Serializable]
        public class TradeLawsEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, string> laws;
            public Dictionary<string, string> punishments;
        }

        [Serializable]
        public class TradeHistoryEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, TradeHistoryPeriod> periods;
        }

        [Serializable]
        public class TradeHistoryPeriod
        {
            public string description;
            public string trader_text;
            public string characteristics;
        }

        [Serializable]
        public class TradeTechnologyEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, string> technologies;
        }

        [Serializable]
        public class TradeCultureEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, string> aspects;
        }

        [Serializable]
        public class TradePhilosophyEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, string> philosophies;
        }

        [Serializable]
        public class TradeFutureEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, string> predictions;
        }

        [Serializable]
        public class TradeOrganizationsEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, TradeOrganizationEntry> organizations;
        }

        [Serializable]
        public class TradeOrganizationEntry
        {
            public string description;
            public string trader_text;
            public string requirements;
            public string benefits;
        }

        [Serializable]
        public class TradeDisputesEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, TradeDisputeEntry> disputes;
        }

        [Serializable]
        public class TradeDisputeEntry
        {
            public string description;
            public string trader_text;
            public string resolution;
        }

        [Serializable]
        public class TradeAgreementsEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, TradeAgreementEntry> agreements;
        }

        [Serializable]
        public class TradeAgreementEntry
        {
            public string description;
            public string trader_text;
            public string terms;
        }

        [Serializable]
        public class TradeCeremoniesEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, TradeCeremonyEntry> ceremonies;
        }

        [Serializable]
        public class TradeCeremonyEntry
        {
            public string description;
            public string trader_text;
            public string meaning;
        }

        [Serializable]
        public class TradeSuperstitionsEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, TradeSuperstitionEntry> superstitions;
        }

        [Serializable]
        public class TradeSuperstitionEntry
        {
            public string description;
            public string trader_text;
            public string meaning;
        }

        [Serializable]
        public class TradeProverbsEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, string> proverbs;
        }

        [Serializable]
        public class TradeJokesEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, string> jokes;
        }

        [Serializable]
        public class TradeRiddlesEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, string> riddles;
        }

        [Serializable]
        public class TradeSkillsEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, TradeSkillEntry> skills;
        }

        [Serializable]
        public class TradeSkillEntry
        {
            public string description;
            public string trader_text;
            public string importance;
            public string difficulty;
        }

        [Serializable]
        public class TradeTrainingEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, TradeTrainingMethodEntry> training;
        }

        [Serializable]
        public class TradeTrainingMethodEntry
        {
            public string description;
            public string trader_text;
            public string duration;
            public string cost;
            public string benefits;
        }

        [Serializable]
        public class TradeCertificationsEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, TradeCertificationEntry> certifications;
        }

        [Serializable]
        public class TradeCertificationEntry
        {
            public string description;
            public string trader_text;
            public string requirements;
            public string benefits;
        }

        [Serializable]
        public class TradeMarketsEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, TradeMarketEntry> markets;
        }

        [Serializable]
        public class TradeMarketEntry
        {
            public string description;
            public string trader_text;
            public string advantages;
            public string disadvantages;
        }

        [Serializable]
        public class TradeCompetitionEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, TradeCompetitionTypeEntry> competition;
        }

        [Serializable]
        public class TradeCompetitionTypeEntry
        {
            public string description;
            public string trader_text;
            public string impact;
            public string resolution;
        }

        [Serializable]
        public class TradeInnovationEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, TradeInnovationTypeEntry> innovations;
        }

        [Serializable]
        public class TradeInnovationTypeEntry
        {
            public string description;
            public string trader_text;
            public string impact;
            public string examples;
        }

        [Serializable]
        public class TradeSustainabilityEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, TradeSustainabilityPracticeEntry> sustainability;
        }

        [Serializable]
        public class TradeSustainabilityPracticeEntry
        {
            public string description;
            public string trader_text;
            public string importance;
            public string practices;
        }

        [Serializable]
        public class TradeStrategiesEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, TradeStrategyEntry> strategies;
        }

        [Serializable]
        public class TradeStrategyEntry
        {
            public string description;
            public string trader_text;
            public string risk;
            public string reward;
        }

        [Serializable]
        public class TradeRelationshipsEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, TradeRelationshipEntry> relationships;
        }

        [Serializable]
        public class TradeRelationshipEntry
        {
            public string description;
            public string trader_text;
            public string importance;
            public string maintenance;
        }

        [Serializable]
        public class TradeHistoryEventsEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, TradeHistoryEventEntry> events;
        }

        [Serializable]
        public class TradeHistoryEventEntry
        {
            public string description;
            public string trader_text;
            public string impact;
            public string lesson;
        }

        [Serializable]
        public class TradeWisdomEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, string> wisdom;
        }

        [Serializable]
        public class TradeChallengesEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, TradeChallengeEntry> challenges;
        }

        [Serializable]
        public class TradeChallengeEntry
        {
            public string description;
            public string trader_text;
            public string solution;
        }

        [Serializable]
        public class TradeSuccessStoriesEntry
        {
            public string description;
            public string trader_text;
            public string player_text;
            public Dictionary<string, TradeSuccessStoryEntry> stories;
        }

        [Serializable]
        public class TradeSuccessStoryEntry
        {
            public string description;
            public string trader_text;
            public string lesson;
            public string outcome;
        }
    }
}
