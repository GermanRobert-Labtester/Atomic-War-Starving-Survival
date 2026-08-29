// SPDX-License-Identifier: MIT
// ASHFALL — UiPanelContractTests.cs
//
// Mechanically asserts that every scene-backed UI panel (.tscn) in assets/ui/
// contains all required unique_name_in_owner nodes with their declared Godot
// control types, guaranteeing 0 runtime binding exceptions before headless boot.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    public sealed class UiPanelContractTests
    {
        private static string GetRepositoryRoot()
        {
            string current = AppContext.BaseDirectory;
            while (!string.IsNullOrEmpty(current))
            {
                if (File.Exists(Path.Combine(current, "project.godot")))
                    return current;
                current = Directory.GetParent(current)?.FullName ?? string.Empty;
            }
            throw new InvalidOperationException("Could not locate repository root from BaseDirectory: " + AppContext.BaseDirectory);
        }

        private sealed class NodeInfo
        {
            public string Name { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public bool IsUnique { get; set; }
        }

        private static Dictionary<string, NodeInfo> ParseTscnUniqueNodes(string tscnPath)
        {
            var nodes = new Dictionary<string, NodeInfo>(StringComparer.Ordinal);
            if (!File.Exists(tscnPath))
                return nodes;

            string[] lines = File.ReadAllLines(tscnPath);
            NodeInfo? current = null;
            var nodeRegex = new Regex(@"^\[node\s+name=""([^""]+)""\s+type=""([^""]+)""", RegexOptions.Compiled);

            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                var match = nodeRegex.Match(trimmed);
                if (match.Success)
                {
                    if (current != null && current.IsUnique)
                    {
                        nodes[current.Name] = current;
                    }
                    current = new NodeInfo
                    {
                        Name = match.Groups[1].Value,
                        Type = match.Groups[2].Value,
                        IsUnique = false
                    };
                }
                else if (trimmed == "unique_name_in_owner = true" && current != null)
                {
                    current.IsUnique = true;
                }
            }

            if (current != null && current.IsUnique)
            {
                nodes[current.Name] = current;
            }

            return nodes;
        }

        [Theory]
        // InventoryDetailPanel
        [InlineData("assets/ui/panels/InventoryDetailPanel.tscn", "Backdrop", "ColorRect")]
        [InlineData("assets/ui/panels/InventoryDetailPanel.tscn", "Info", "VBoxContainer")]
        [InlineData("assets/ui/panels/InventoryDetailPanel.tscn", "Stats", "VBoxContainer")]
        [InlineData("assets/ui/panels/InventoryDetailPanel.tscn", "Actions", "VBoxContainer")]
        [InlineData("assets/ui/panels/InventoryDetailPanel.tscn", "CloseButton", "Button")]
        // AfflictionsPanel
        [InlineData("assets/ui/panels/AfflictionsPanel.tscn", "Backdrop", "ColorRect")]
        [InlineData("assets/ui/panels/AfflictionsPanel.tscn", "ActiveList", "VBoxContainer")]
        [InlineData("assets/ui/panels/AfflictionsPanel.tscn", "ChronicList", "VBoxContainer")]
        [InlineData("assets/ui/panels/AfflictionsPanel.tscn", "TreatmentList", "VBoxContainer")]
        [InlineData("assets/ui/panels/AfflictionsPanel.tscn", "CloseButton", "Button")]
        // SurvivorDetailPanel
        [InlineData("assets/ui/panels/SurvivorDetailPanel.tscn", "Backdrop", "ColorRect")]
        [InlineData("assets/ui/panels/SurvivorDetailPanel.tscn", "SurvivorInfo", "VBoxContainer")]
        [InlineData("assets/ui/panels/SurvivorDetailPanel.tscn", "NeedsList", "VBoxContainer")]
        [InlineData("assets/ui/panels/SurvivorDetailPanel.tscn", "TraitsList", "VBoxContainer")]
        [InlineData("assets/ui/panels/SurvivorDetailPanel.tscn", "StatusList", "VBoxContainer")]
        [InlineData("assets/ui/panels/SurvivorDetailPanel.tscn", "CloseButton", "Button")]
        // WeatherDetailPanel
        [InlineData("assets/ui/panels/WeatherDetailPanel.tscn", "Backdrop", "ColorRect")]
        [InlineData("assets/ui/panels/WeatherDetailPanel.tscn", "CurrentList", "VBoxContainer")]
        [InlineData("assets/ui/panels/WeatherDetailPanel.tscn", "ForecastList", "VBoxContainer")]
        [InlineData("assets/ui/panels/WeatherDetailPanel.tscn", "WindList", "VBoxContainer")]
        [InlineData("assets/ui/panels/WeatherDetailPanel.tscn", "TrendList", "VBoxContainer")]
        [InlineData("assets/ui/panels/WeatherDetailPanel.tscn", "CloseButton", "Button")]
        // QuestDetailPanel
        [InlineData("assets/ui/panels/QuestDetailPanel.tscn", "Backdrop", "ColorRect")]
        [InlineData("assets/ui/panels/QuestDetailPanel.tscn", "InfoContainer", "VBoxContainer")]
        [InlineData("assets/ui/panels/QuestDetailPanel.tscn", "StagesContainer", "VBoxContainer")]
        [InlineData("assets/ui/panels/QuestDetailPanel.tscn", "ChoicesContainer", "VBoxContainer")]
        [InlineData("assets/ui/panels/QuestDetailPanel.tscn", "RewardsContainer", "VBoxContainer")]
        [InlineData("assets/ui/panels/QuestDetailPanel.tscn", "Title", "Label")]
        [InlineData("assets/ui/panels/QuestDetailPanel.tscn", "CloseButton", "Button")]
        // MapDetailPanel
        [InlineData("assets/ui/panels/MapDetailPanel.tscn", "Backdrop", "ColorRect")]
        [InlineData("assets/ui/panels/MapDetailPanel.tscn", "InfoContainer", "VBoxContainer")]
        [InlineData("assets/ui/panels/MapDetailPanel.tscn", "HazardsContainer", "VBoxContainer")]
        [InlineData("assets/ui/panels/MapDetailPanel.tscn", "LayoutsContainer", "VBoxContainer")]
        [InlineData("assets/ui/panels/MapDetailPanel.tscn", "SalvageContainer", "VBoxContainer")]
        [InlineData("assets/ui/panels/MapDetailPanel.tscn", "Title", "Label")]
        [InlineData("assets/ui/panels/MapDetailPanel.tscn", "CloseButton", "Button")]
        // RadiationDetailPanel
        [InlineData("assets/ui/panels/RadiationDetailPanel.tscn", "Backdrop", "ColorRect")]
        [InlineData("assets/ui/panels/RadiationDetailPanel.tscn", "CurrentData", "VBoxContainer")]
        [InlineData("assets/ui/panels/RadiationDetailPanel.tscn", "DosimeterData", "VBoxContainer")]
        [InlineData("assets/ui/panels/RadiationDetailPanel.tscn", "ProtectionData", "VBoxContainer")]
        [InlineData("assets/ui/panels/RadiationDetailPanel.tscn", "EventsList", "VBoxContainer")]
        [InlineData("assets/ui/panels/RadiationDetailPanel.tscn", "CloseButton", "Button")]
        // EconomyDetailPanel
        [InlineData("assets/ui/panels/EconomyDetailPanel.tscn", "Backdrop", "ColorRect")]
        [InlineData("assets/ui/panels/EconomyDetailPanel.tscn", "ResourcesList", "VBoxContainer")]
        [InlineData("assets/ui/panels/EconomyDetailPanel.tscn", "TradeList", "VBoxContainer")]
        [InlineData("assets/ui/panels/EconomyDetailPanel.tscn", "MarketList", "VBoxContainer")]
        [InlineData("assets/ui/panels/EconomyDetailPanel.tscn", "DebtList", "VBoxContainer")]
        [InlineData("assets/ui/panels/EconomyDetailPanel.tscn", "CloseButton", "Button")]
        // CombatDetailPanel
        [InlineData("assets/ui/panels/CombatDetailPanel.tscn", "Backdrop", "ColorRect")]
        [InlineData("assets/ui/panels/CombatDetailPanel.tscn", "BattleInfo", "VBoxContainer")]
        [InlineData("assets/ui/panels/CombatDetailPanel.tscn", "TacticsData", "VBoxContainer")]
        [InlineData("assets/ui/panels/CombatDetailPanel.tscn", "CasualtyData", "VBoxContainer")]
        [InlineData("assets/ui/panels/CombatDetailPanel.tscn", "OutcomesData", "VBoxContainer")]
        [InlineData("assets/ui/panels/CombatDetailPanel.tscn", "CloseButton", "Button")]
        // FactionDetailPanel
        [InlineData("assets/ui/panels/FactionDetailPanel.tscn", "Backdrop", "ColorRect")]
        [InlineData("assets/ui/panels/FactionDetailPanel.tscn", "InfoContainer", "VBoxContainer")]
        [InlineData("assets/ui/panels/FactionDetailPanel.tscn", "DiplomacyContainer", "VBoxContainer")]
        [InlineData("assets/ui/panels/FactionDetailPanel.tscn", "TradeContainer", "VBoxContainer")]
        [InlineData("assets/ui/panels/FactionDetailPanel.tscn", "EventsContainer", "VBoxContainer")]
        [InlineData("assets/ui/panels/FactionDetailPanel.tscn", "Title", "Label")]
        [InlineData("assets/ui/panels/FactionDetailPanel.tscn", "CloseButton", "Button")]
        // JournalDetailPanel
        [InlineData("assets/ui/panels/JournalDetailPanel.tscn", "Backdrop", "ColorRect")]
        [InlineData("assets/ui/panels/JournalDetailPanel.tscn", "EntriesList", "VBoxContainer")]
        [InlineData("assets/ui/panels/JournalDetailPanel.tscn", "CodexList", "VBoxContainer")]
        [InlineData("assets/ui/panels/JournalDetailPanel.tscn", "TabsList", "VBoxContainer")]
        [InlineData("assets/ui/panels/JournalDetailPanel.tscn", "CloseButton", "Button")]
        // EventDetailPanel
        [InlineData("assets/ui/panels/EventDetailPanel.tscn", "Backdrop", "ColorRect")]
        [InlineData("assets/ui/panels/EventDetailPanel.tscn", "EventInfoList", "VBoxContainer")]
        [InlineData("assets/ui/panels/EventDetailPanel.tscn", "HistoryList", "VBoxContainer")]
        [InlineData("assets/ui/panels/EventDetailPanel.tscn", "NarrativeList", "VBoxContainer")]
        [InlineData("assets/ui/panels/EventDetailPanel.tscn", "CloseButton", "Button")]
        // DutyRosterDetailPanel
        [InlineData("assets/ui/panels/DutyRosterDetailPanel.tscn", "Backdrop", "ColorRect")]
        [InlineData("assets/ui/panels/DutyRosterDetailPanel.tscn", "AssignmentsList", "VBoxContainer")]
        [InlineData("assets/ui/panels/DutyRosterDetailPanel.tscn", "ShiftsList", "VBoxContainer")]
        [InlineData("assets/ui/panels/DutyRosterDetailPanel.tscn", "PerformanceList", "VBoxContainer")]
        [InlineData("assets/ui/panels/DutyRosterDetailPanel.tscn", "CloseButton", "Button")]
        // SurvivalDetailPanel
        [InlineData("assets/ui/panels/SurvivalDetailPanel.tscn", "Backdrop", "ColorRect")]
        [InlineData("assets/ui/panels/SurvivalDetailPanel.tscn", "HealthData", "VBoxContainer")]
        [InlineData("assets/ui/panels/SurvivalDetailPanel.tscn", "NeedsData", "VBoxContainer")]
        [InlineData("assets/ui/panels/SurvivalDetailPanel.tscn", "RadiationData", "VBoxContainer")]
        [InlineData("assets/ui/panels/SurvivalDetailPanel.tscn", "StatusData", "VBoxContainer")]
        [InlineData("assets/ui/panels/SurvivalDetailPanel.tscn", "CloseButton", "Button")]
        // WorkshopPanel
        [InlineData("assets/ui/panels/WorkshopPanel.tscn", "RelicListContainer", "VBoxContainer")]
        [InlineData("assets/ui/panels/WorkshopPanel.tscn", "DetailContainer", "VBoxContainer")]
        [InlineData("assets/ui/panels/WorkshopPanel.tscn", "JobHeader", "Label")]
        [InlineData("assets/ui/panels/WorkshopPanel.tscn", "JobProgressBar", "ProgressBar")]
        [InlineData("assets/ui/panels/WorkshopPanel.tscn", "JobDetails", "Label")]
        [InlineData("assets/ui/panels/WorkshopPanel.tscn", "CancelJobButton", "Button")]
        [InlineData("assets/ui/panels/WorkshopPanel.tscn", "CloseButton", "Button")]
        // CraftingPanel
        [InlineData("assets/ui/panels/CraftingPanel.tscn", "RecipeList", "VBoxContainer")]
        [InlineData("assets/ui/panels/CraftingPanel.tscn", "QueueList", "VBoxContainer")]
        [InlineData("assets/ui/panels/CraftingPanel.tscn", "QueueHeader", "Label")]
        [InlineData("assets/ui/panels/CraftingPanel.tscn", "FilterStatus", "Label")]
        [InlineData("assets/ui/panels/CraftingPanel.tscn", "CloseButton", "Button")]
        [InlineData("assets/ui/panels/CraftingPanel.tscn", "FilterAllButton", "Button")]
        [InlineData("assets/ui/panels/CraftingPanel.tscn", "FilterCraftableButton", "Button")]
        [InlineData("assets/ui/panels/CraftingPanel.tscn", "RelicWorkshopButton", "Button")]
        [InlineData("assets/ui/panels/CraftingPanel.tscn", "PharmaLabButton", "Button")]
        // KitchenNutritionPanel
        [InlineData("assets/ui/panels/KitchenNutritionPanel.tscn", "RecipeList", "VBoxContainer")]
        [InlineData("assets/ui/panels/KitchenNutritionPanel.tscn", "PrepStation", "VBoxContainer")]
        [InlineData("assets/ui/panels/KitchenNutritionPanel.tscn", "ServiceLogContainer", "VBoxContainer")]
        [InlineData("assets/ui/panels/KitchenNutritionPanel.tscn", "EventLogLabel", "Label")]
        // WaterTreatmentPanel
        [InlineData("assets/ui/panels/WaterTreatmentPanel.tscn", "ContentStack", "VBoxContainer")]
        [InlineData("assets/ui/panels/WaterTreatmentPanel.tscn", "DetailText", "Label")]
        [InlineData("assets/ui/panels/WaterTreatmentPanel.tscn", "CharcoalButton", "Button")]
        [InlineData("assets/ui/panels/WaterTreatmentPanel.tscn", "DistillButton", "Button")]
        [InlineData("assets/ui/panels/WaterTreatmentPanel.tscn", "OsmosisButton", "Button")]
        [InlineData("assets/ui/panels/WaterTreatmentPanel.tscn", "ReplaceFilterButton", "Button")]
        // PharmaLabPanel
        [InlineData("assets/ui/panels/PharmaLabPanel.tscn", "RecipeListContainer", "VBoxContainer")]
        [InlineData("assets/ui/panels/PharmaLabPanel.tscn", "DetailContainer", "VBoxContainer")]
        [InlineData("assets/ui/panels/PharmaLabPanel.tscn", "LabStatusHeader", "Label")]
        // OpeningProtocolModal
        [InlineData("assets/ui/modals/OpeningProtocolModal.tscn", "Backdrop", "ColorRect")]
        [InlineData("assets/ui/modals/OpeningProtocolModal.tscn", "Title", "Label")]
        [InlineData("assets/ui/modals/OpeningProtocolModal.tscn", "CloseButton", "Button")]
        // SafeCrackModal
        [InlineData("assets/ui/modals/SafeCrackModal.tscn", "Margin", "MarginContainer")]
        [InlineData("assets/ui/modals/SafeCrackModal.tscn", "Root", "VBoxContainer")]
        [InlineData("assets/ui/modals/SafeCrackModal.tscn", "HeaderLabel", "Label")]
        // DailyBriefingModal
        [InlineData("assets/ui/modals/DailyBriefingModal.tscn", "TitleLabel", "Label")]
        [InlineData("assets/ui/modals/DailyBriefingModal.tscn", "BodyLabel", "RichTextLabel")]
        [InlineData("assets/ui/modals/DailyBriefingModal.tscn", "AckButton", "Button")]
        public void SceneBackedPanel_ContainsDeclaredUniqueNodeContract(string relativeTscnPath, string nodeName, string expectedType)
        {
            string root = GetRepositoryRoot();
            string fullPath = Path.Combine(root, relativeTscnPath);
            Assert.True(File.Exists(fullPath), $"Scene file {relativeTscnPath} does not exist on disk.");

            var uniqueNodes = ParseTscnUniqueNodes(fullPath);
            Assert.True(uniqueNodes.ContainsKey(nodeName),
                $"Scene {relativeTscnPath} is missing required unique node '%{nodeName}' (unique_name_in_owner = true).");

            Assert.Equal(expectedType, uniqueNodes[nodeName].Type);
        }
    }
}
