using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.World
{
    [Serializable]
    public sealed class SettlementEconomy
    {
        [JsonPropertyName("primary_export")]
        public string PrimaryExport { get; set; } = string.Empty;

        [JsonPropertyName("primary_import")]
        public string PrimaryImport { get; set; } = string.Empty;

        [JsonPropertyName("trade_specialty")]
        public string TradeSpecialty { get; set; } = string.Empty;

        [JsonPropertyName("price_modifier_exports")]
        public float PriceModifierExports { get; set; } = 1.0f;

        [JsonPropertyName("price_modifier_imports")]
        public float PriceModifierImports { get; set; } = 1.0f;

        [JsonPropertyName("stock_item_ids")]
        public List<string> StockItemIds { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class SettlementSociety
    {
        [JsonPropertyName("governance")]
        public string Governance { get; set; } = string.Empty;

        [JsonPropertyName("population")]
        public int Population { get; set; } = 50;

        [JsonPropertyName("core_value")]
        public string CoreValue { get; set; } = string.Empty;

        [JsonPropertyName("internal_tension")]
        public string InternalTension { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class SettlementFactionRelation
    {
        [JsonPropertyName("primary_faction")]
        public string PrimaryFaction { get; set; } = string.Empty;

        [JsonPropertyName("standing_gate_faction")]
        public string StandingGateFaction { get; set; } = string.Empty;

        [JsonPropertyName("min_standing_to_enter")]
        public int MinStandingToEnter { get; set; } = 0;

        [JsonPropertyName("hostile_standing_threshold")]
        public int HostileStandingThreshold { get; set; } = -40;
    }

    [Serializable]
    public sealed class SettlementDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("archetype")]
        public string Archetype { get; set; } = string.Empty;

        [JsonPropertyName("region")]
        public string Region { get; set; } = string.Empty;

        [JsonPropertyName("location_id")]
        public string LocationId { get; set; } = string.Empty;

        [JsonPropertyName("route_node")]
        public string RouteNode { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("survival_adaptation")]
        public string SurvivalAdaptation { get; set; } = string.Empty;

        [JsonPropertyName("economy")]
        public SettlementEconomy Economy { get; set; } = new SettlementEconomy();

        [JsonPropertyName("society")]
        public SettlementSociety Society { get; set; } = new SettlementSociety();

        [JsonPropertyName("faction_relation")]
        public SettlementFactionRelation FactionRelation { get; set; } = new SettlementFactionRelation();

        [JsonPropertyName("location_link")]
        public string LocationLink { get; set; } = string.Empty;

        [JsonPropertyName("population")]
        public int Population { get; set; } = 0;

        [JsonPropertyName("allegiance")]
        public string Allegiance { get; set; } = string.Empty;

        [JsonPropertyName("threat_level")]
        public int ThreatLevel { get; set; } = 2;

        [JsonPropertyName("attitude")]
        public string Attitude { get; set; } = "neutral";

        [JsonPropertyName("trade_goods")]
        public List<string> TradeGoods { get; set; } = new List<string>();

        [JsonPropertyName("trade_needs")]
        public List<string> TradeNeeds { get; set; } = new List<string>();

        [JsonPropertyName("keeper_npc_id")]
        public string KeeperNpcId { get; set; } = string.Empty;

        [JsonPropertyName("trader_npc_id")]
        public string TraderNpcId { get; set; } = string.Empty;

        [JsonPropertyName("fixture_npc_id")]
        public string FixtureNpcId { get; set; } = string.Empty;

        [JsonPropertyName("sidework_quest_id")]
        public string SideworkQuestId { get; set; } = string.Empty;

        public string GetEffectiveLocationId() => !string.IsNullOrEmpty(LocationLink) ? LocationLink : LocationId;
        public int GetEffectivePopulation() => Population > 0 ? Population : (Society?.Population ?? 50);
        public string GetEffectiveAllegiance() => !string.IsNullOrEmpty(Allegiance) ? Allegiance : (FactionRelation?.PrimaryFaction ?? "none");
    }

    [Serializable]
    public sealed class SettlementNpcGreeting
    {
        [JsonPropertyName("low_standing")]
        public string LowStanding { get; set; } = string.Empty;

        [JsonPropertyName("neutral")]
        public string Neutral { get; set; } = string.Empty;

        [JsonPropertyName("high_standing")]
        public string HighStanding { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class SettlementNpcEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("settlement_id")]
        public string SettlementId { get; set; } = string.Empty;

        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty; // "Keeper", "Trader", "Fixture"

        [JsonPropertyName("profession")]
        public string Profession { get; set; } = string.Empty;

        [JsonPropertyName("faction")]
        public string Faction { get; set; } = "none";

        [JsonPropertyName("trade_specialty")]
        public string TradeSpecialty { get; set; } = string.Empty;

        [JsonPropertyName("physical_anchor")]
        public string PhysicalAnchor { get; set; } = string.Empty;

        [JsonPropertyName("value")]
        public string Value { get; set; } = string.Empty;

        [JsonPropertyName("fear")]
        public string Fear { get; set; } = string.Empty;

        [JsonPropertyName("contradiction")]
        public string Contradiction { get; set; } = string.Empty;

        [JsonPropertyName("personal_thread")]
        public string PersonalThread { get; set; } = string.Empty;

        [JsonPropertyName("greetings")]
        public SettlementNpcGreeting Greetings { get; set; } = new SettlementNpcGreeting();

        [JsonPropertyName("trade_tells")]
        public List<string> TradeTells { get; set; } = new List<string>();

        [JsonPropertyName("sidework_quest_id")]
        public string SideworkQuestId { get; set; } = string.Empty;

        [JsonPropertyName("portrait_id")]
        public string PortraitId { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class RepeatableQuestStage
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class RepeatableQuestEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("provider_npc_id")]
        public string ProviderNpcId { get; set; } = string.Empty;

        [JsonPropertyName("settlement_id")]
        public string SettlementId { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("briefing")]
        public string Briefing { get; set; } = string.Empty;

        [JsonPropertyName("prereq_quest_id")]
        public string PrereqQuestId { get; set; } = string.Empty;

        [JsonPropertyName("target_location_id")]
        public string TargetLocationId { get; set; } = string.Empty;

        [JsonPropertyName("cooldown_days")]
        public int CooldownDays { get; set; } = 7;

        [JsonPropertyName("reward_item_id")]
        public string RewardItemId { get; set; } = string.Empty;

        [JsonPropertyName("reward_count")]
        public int RewardCount { get; set; } = 1;

        [JsonPropertyName("standing_delta")]
        public int StandingDelta { get; set; } = 5;

        [JsonPropertyName("stages")]
        public List<RepeatableQuestStage> Stages { get; set; } = new List<RepeatableQuestStage>();
    }

    [Serializable]
    public sealed class SettlementState
    {
        [JsonPropertyName("cooldowns")]
        public Dictionary<string, int> QuestAvailableDay { get; set; } = new Dictionary<string, int>();

        [JsonPropertyName("completed_quest_counts")]
        public Dictionary<string, int> CompletedQuestCounts { get; set; } = new Dictionary<string, int>();
    }

    public sealed class SettlementCatalog
    {
        private readonly Dictionary<string, SettlementDefinition> _settlementsById = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, SettlementNpcEntry> _npcsById = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, RepeatableQuestEntry> _questsById = new(StringComparer.OrdinalIgnoreCase);
        private readonly List<SettlementDefinition> _allSettlements = new();
        private readonly List<SettlementNpcEntry> _allNpcs = new();
        private readonly List<RepeatableQuestEntry> _allQuests = new();

        private readonly Dictionary<string, int> _questAvailableDay = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, int> _completedQuestCounts = new(StringComparer.OrdinalIgnoreCase);

        public int SettlementCount => _allSettlements.Count;
        public int NpcCount => _allNpcs.Count;
        public int QuestCount => _allQuests.Count;

        public IReadOnlyList<SettlementDefinition> Settlements => _allSettlements;
        public IReadOnlyList<SettlementNpcEntry> Npcs => _allNpcs;
        public IReadOnlyList<RepeatableQuestEntry> Quests => _allQuests;

        public static SettlementCatalog LoadFromDirectory(string directoryPath, IFileIO fileIO)
        {
            var catalog = new SettlementCatalog();
            if (string.IsNullOrEmpty(directoryPath) || fileIO == null) return catalog;

            string settlementsPath = Path.Combine(directoryPath, "settlements.json");
            if (fileIO.FileExists(settlementsPath))
            {
                catalog.LoadSettlementsJson(fileIO.ReadAllText(settlementsPath));
            }

            string npcsPath = Path.Combine(directoryPath, "wasteland_settlement_npcs.json");
            if (fileIO.FileExists(npcsPath))
            {
                catalog.LoadNpcsJson(fileIO.ReadAllText(npcsPath));
            }

            string questsPath = Path.Combine(directoryPath, "repeatable_quests.json");
            if (fileIO.FileExists(questsPath))
            {
                catalog.LoadQuestsJson(fileIO.ReadAllText(questsPath));
            }

            return catalog;
        }

        public void LoadSettlementsJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("settlements", out var settlementsElem) && settlementsElem.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in settlementsElem.EnumerateArray())
                {
                    var settlement = JsonSerializer.Deserialize<SettlementDefinition>(item.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (settlement != null && !string.IsNullOrEmpty(settlement.Id))
                    {
                        _settlementsById[settlement.Id] = settlement;
                        _allSettlements.Add(settlement);
                    }
                }
            }
        }

        public void LoadNpcsJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("npcs", out var npcsElem) && npcsElem.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in npcsElem.EnumerateArray())
                {
                    var npc = JsonSerializer.Deserialize<SettlementNpcEntry>(item.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (npc != null && !string.IsNullOrEmpty(npc.Id))
                    {
                        _npcsById[npc.Id] = npc;
                        _allNpcs.Add(npc);
                    }
                }
            }
        }

        public void LoadQuestsJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("quests", out var questsElem) && questsElem.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in questsElem.EnumerateArray())
                {
                    var quest = JsonSerializer.Deserialize<RepeatableQuestEntry>(item.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (quest != null && !string.IsNullOrEmpty(quest.Id))
                    {
                        _questsById[quest.Id] = quest;
                        _allQuests.Add(quest);
                    }
                }
            }
        }

        public bool TryGetSettlement(string id, out SettlementDefinition settlement)
        {
            if (string.IsNullOrEmpty(id))
            {
                settlement = null!;
                return false;
            }
            return _settlementsById.TryGetValue(id, out settlement!);
        }

        public bool TryGetNpc(string id, out SettlementNpcEntry npc)
        {
            if (string.IsNullOrEmpty(id))
            {
                npc = null!;
                return false;
            }
            return _npcsById.TryGetValue(id, out npc!);
        }

        public bool TryGetQuest(string id, out RepeatableQuestEntry quest)
        {
            if (string.IsNullOrEmpty(id))
            {
                quest = null!;
                return false;
            }
            return _questsById.TryGetValue(id, out quest!);
        }

        public string GetNpcGreeting(string npcId, float standing)
        {
            if (!TryGetNpc(npcId, out var npc)) return string.Empty;
            if (standing <= -15f) return npc.Greetings.LowStanding;
            if (standing >= 25f) return npc.Greetings.HighStanding;
            return npc.Greetings.Neutral;
        }

        public bool IsQuestAvailable(string questId, int currentDay)
        {
            if (!_questsById.ContainsKey(questId)) return false;
            if (_questAvailableDay.TryGetValue(questId, out int nextAvailable))
            {
                return currentDay >= nextAvailable;
            }
            return true;
        }

        public void CompleteQuest(string questId, int currentDay)
        {
            if (!TryGetQuest(questId, out var quest)) return;

            _questAvailableDay[questId] = currentDay + Math.Max(1, quest.CooldownDays);
            if (!_completedQuestCounts.ContainsKey(questId))
            {
                _completedQuestCounts[questId] = 0;
            }
            _completedQuestCounts[questId]++;
        }

        public int GetCompletedQuestCount(string questId)
        {
            return _completedQuestCounts.TryGetValue(questId, out int count) ? count : 0;
        }

        public SettlementState CaptureState()
        {
            return new SettlementState
            {
                QuestAvailableDay = new Dictionary<string, int>(_questAvailableDay, StringComparer.OrdinalIgnoreCase),
                CompletedQuestCounts = new Dictionary<string, int>(_completedQuestCounts, StringComparer.OrdinalIgnoreCase)
            };
        }

        public void RestoreState(SettlementState? state)
        {
            _questAvailableDay.Clear();
            _completedQuestCounts.Clear();
            if (state == null) return;

            if (state.QuestAvailableDay != null)
            {
                foreach (var kvp in state.QuestAvailableDay)
                {
                    _questAvailableDay[kvp.Key] = kvp.Value;
                }
            }

            if (state.CompletedQuestCounts != null)
            {
                foreach (var kvp in state.CompletedQuestCounts)
                {
                    _completedQuestCounts[kvp.Key] = kvp.Value;
                }
            }
        }
    }
}
