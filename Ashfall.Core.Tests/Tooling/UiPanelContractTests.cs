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

        private static readonly (string RelativeTscnPath, string NodeName, string ExpectedType)[] DeclaredContracts =
        {
            ("assets/ui/panels/InventoryDetailPanel.tscn", "Backdrop", "ColorRect"),
            ("assets/ui/panels/InventoryDetailPanel.tscn", "Info", "VBoxContainer"),
            ("assets/ui/panels/InventoryDetailPanel.tscn", "Stats", "VBoxContainer"),
            ("assets/ui/panels/InventoryDetailPanel.tscn", "Actions", "VBoxContainer"),
            ("assets/ui/panels/InventoryDetailPanel.tscn", "CloseButton", "Button"),
            ("assets/ui/panels/AfflictionsPanel.tscn", "Backdrop", "ColorRect"),
            ("assets/ui/panels/AfflictionsPanel.tscn", "ActiveList", "VBoxContainer"),
            ("assets/ui/panels/AfflictionsPanel.tscn", "ChronicList", "VBoxContainer"),
            ("assets/ui/panels/AfflictionsPanel.tscn", "TreatmentList", "VBoxContainer"),
            ("assets/ui/panels/AfflictionsPanel.tscn", "CloseButton", "Button"),
            ("assets/ui/panels/SurvivorDetailPanel.tscn", "Backdrop", "ColorRect"),
            ("assets/ui/panels/SurvivorDetailPanel.tscn", "SurvivorInfo", "VBoxContainer"),
            ("assets/ui/panels/SurvivorDetailPanel.tscn", "NeedsList", "VBoxContainer"),
            ("assets/ui/panels/SurvivorDetailPanel.tscn", "TraitsList", "VBoxContainer"),
            ("assets/ui/panels/SurvivorDetailPanel.tscn", "StatusList", "VBoxContainer"),
            ("assets/ui/panels/SurvivorDetailPanel.tscn", "CloseButton", "Button"),
            ("assets/ui/panels/WeatherDetailPanel.tscn", "Backdrop", "ColorRect"),
            ("assets/ui/panels/WeatherDetailPanel.tscn", "CurrentList", "VBoxContainer"),
            ("assets/ui/panels/WeatherDetailPanel.tscn", "ForecastList", "VBoxContainer"),
            ("assets/ui/panels/WeatherDetailPanel.tscn", "WindList", "VBoxContainer"),
            ("assets/ui/panels/WeatherDetailPanel.tscn", "TrendList", "VBoxContainer"),
            ("assets/ui/panels/WeatherDetailPanel.tscn", "CloseButton", "Button"),
            ("assets/ui/panels/QuestDetailPanel.tscn", "Backdrop", "ColorRect"),
            ("assets/ui/panels/QuestDetailPanel.tscn", "InfoContainer", "VBoxContainer"),
            ("assets/ui/panels/QuestDetailPanel.tscn", "StagesContainer", "VBoxContainer"),
            ("assets/ui/panels/QuestDetailPanel.tscn", "ChoicesContainer", "VBoxContainer"),
            ("assets/ui/panels/QuestDetailPanel.tscn", "RewardsContainer", "VBoxContainer"),
            ("assets/ui/panels/QuestDetailPanel.tscn", "Title", "Label"),
            ("assets/ui/panels/QuestDetailPanel.tscn", "CloseButton", "Button"),
            ("assets/ui/panels/MapDetailPanel.tscn", "Backdrop", "ColorRect"),
            ("assets/ui/panels/MapDetailPanel.tscn", "InfoContainer", "VBoxContainer"),
            ("assets/ui/panels/MapDetailPanel.tscn", "HazardsContainer", "VBoxContainer"),
            ("assets/ui/panels/MapDetailPanel.tscn", "LayoutsContainer", "VBoxContainer"),
            ("assets/ui/panels/MapDetailPanel.tscn", "SalvageContainer", "VBoxContainer"),
            ("assets/ui/panels/MapDetailPanel.tscn", "Title", "Label"),
            ("assets/ui/panels/MapDetailPanel.tscn", "CloseButton", "Button"),
            ("assets/ui/panels/RadiationDetailPanel.tscn", "Backdrop", "ColorRect"),
            ("assets/ui/panels/RadiationDetailPanel.tscn", "CurrentData", "VBoxContainer"),
            ("assets/ui/panels/RadiationDetailPanel.tscn", "DosimeterData", "VBoxContainer"),
            ("assets/ui/panels/RadiationDetailPanel.tscn", "ProtectionData", "VBoxContainer"),
            ("assets/ui/panels/RadiationDetailPanel.tscn", "EventsList", "VBoxContainer"),
            ("assets/ui/panels/RadiationDetailPanel.tscn", "CloseButton", "Button"),
            ("assets/ui/panels/EconomyDetailPanel.tscn", "Backdrop", "ColorRect"),
            ("assets/ui/panels/EconomyDetailPanel.tscn", "ResourcesList", "VBoxContainer"),
            ("assets/ui/panels/EconomyDetailPanel.tscn", "TradeList", "VBoxContainer"),
            ("assets/ui/panels/EconomyDetailPanel.tscn", "MarketList", "VBoxContainer"),
            ("assets/ui/panels/EconomyDetailPanel.tscn", "DebtList", "VBoxContainer"),
            ("assets/ui/panels/EconomyDetailPanel.tscn", "CloseButton", "Button"),
            ("assets/ui/panels/CombatDetailPanel.tscn", "Backdrop", "ColorRect"),
            ("assets/ui/panels/CombatDetailPanel.tscn", "BattleInfo", "VBoxContainer"),
            ("assets/ui/panels/CombatDetailPanel.tscn", "TacticsData", "VBoxContainer"),
            ("assets/ui/panels/CombatDetailPanel.tscn", "CasualtyData", "VBoxContainer"),
            ("assets/ui/panels/CombatDetailPanel.tscn", "OutcomesData", "VBoxContainer"),
            ("assets/ui/panels/CombatDetailPanel.tscn", "CloseButton", "Button"),
            ("assets/ui/panels/FactionDetailPanel.tscn", "Backdrop", "ColorRect"),
            ("assets/ui/panels/FactionDetailPanel.tscn", "InfoContainer", "VBoxContainer"),
            ("assets/ui/panels/FactionDetailPanel.tscn", "DiplomacyContainer", "VBoxContainer"),
            ("assets/ui/panels/FactionDetailPanel.tscn", "TradeContainer", "VBoxContainer"),
            ("assets/ui/panels/FactionDetailPanel.tscn", "EventsContainer", "VBoxContainer"),
            ("assets/ui/panels/FactionDetailPanel.tscn", "Title", "Label"),
            ("assets/ui/panels/FactionDetailPanel.tscn", "CloseButton", "Button"),
            ("assets/ui/panels/JournalDetailPanel.tscn", "Backdrop", "ColorRect"),
            ("assets/ui/panels/JournalDetailPanel.tscn", "EntriesList", "VBoxContainer"),
            ("assets/ui/panels/JournalDetailPanel.tscn", "CodexList", "VBoxContainer"),
            ("assets/ui/panels/JournalDetailPanel.tscn", "TabsList", "VBoxContainer"),
            ("assets/ui/panels/JournalDetailPanel.tscn", "CloseButton", "Button"),
            ("assets/ui/panels/EventDetailPanel.tscn", "Backdrop", "ColorRect"),
            ("assets/ui/panels/EventDetailPanel.tscn", "EventInfoList", "VBoxContainer"),
            ("assets/ui/panels/EventDetailPanel.tscn", "HistoryList", "VBoxContainer"),
            ("assets/ui/panels/EventDetailPanel.tscn", "NarrativeList", "VBoxContainer"),
            ("assets/ui/panels/EventDetailPanel.tscn", "CloseButton", "Button"),
            ("assets/ui/panels/DutyRosterDetailPanel.tscn", "Backdrop", "ColorRect"),
            ("assets/ui/panels/DutyRosterDetailPanel.tscn", "AssignmentsList", "VBoxContainer"),
            ("assets/ui/panels/DutyRosterDetailPanel.tscn", "ShiftsList", "VBoxContainer"),
            ("assets/ui/panels/DutyRosterDetailPanel.tscn", "PerformanceList", "VBoxContainer"),
            ("assets/ui/panels/DutyRosterDetailPanel.tscn", "CloseButton", "Button"),
            ("assets/ui/panels/SurvivalDetailPanel.tscn", "Backdrop", "ColorRect"),
            ("assets/ui/panels/SurvivalDetailPanel.tscn", "HealthData", "VBoxContainer"),
            ("assets/ui/panels/SurvivalDetailPanel.tscn", "NeedsData", "VBoxContainer"),
            ("assets/ui/panels/SurvivalDetailPanel.tscn", "RadiationData", "VBoxContainer"),
            ("assets/ui/panels/SurvivalDetailPanel.tscn", "StatusData", "VBoxContainer"),
            ("assets/ui/panels/SurvivalDetailPanel.tscn", "CloseButton", "Button"),
            ("assets/ui/panels/WorkshopPanel.tscn", "RelicListContainer", "VBoxContainer"),
            ("assets/ui/panels/WorkshopPanel.tscn", "DetailContainer", "VBoxContainer"),
            ("assets/ui/panels/WorkshopPanel.tscn", "JobHeader", "Label"),
            ("assets/ui/panels/WorkshopPanel.tscn", "JobProgressBar", "ProgressBar"),
            ("assets/ui/panels/WorkshopPanel.tscn", "JobDetails", "Label"),
            ("assets/ui/panels/WorkshopPanel.tscn", "CancelJobButton", "Button"),
            ("assets/ui/panels/WorkshopPanel.tscn", "CloseButton", "Button"),
            ("assets/ui/panels/CraftingPanel.tscn", "RecipeList", "VBoxContainer"),
            ("assets/ui/panels/CraftingPanel.tscn", "QueueList", "VBoxContainer"),
            ("assets/ui/panels/CraftingPanel.tscn", "QueueHeader", "Label"),
            ("assets/ui/panels/CraftingPanel.tscn", "FilterStatus", "Label"),
            ("assets/ui/panels/CraftingPanel.tscn", "CloseButton", "Button"),
            ("assets/ui/panels/CraftingPanel.tscn", "FilterAllButton", "Button"),
            ("assets/ui/panels/CraftingPanel.tscn", "FilterCraftableButton", "Button"),
            ("assets/ui/panels/CraftingPanel.tscn", "RelicWorkshopButton", "Button"),
            ("assets/ui/panels/CraftingPanel.tscn", "PharmaLabButton", "Button"),
            ("assets/ui/panels/KitchenNutritionPanel.tscn", "RecipeList", "VBoxContainer"),
            ("assets/ui/panels/KitchenNutritionPanel.tscn", "PrepStation", "VBoxContainer"),
            ("assets/ui/panels/KitchenNutritionPanel.tscn", "ServiceLogContainer", "VBoxContainer"),
            ("assets/ui/panels/KitchenNutritionPanel.tscn", "EventLogLabel", "Label"),
            ("assets/ui/panels/WaterTreatmentPanel.tscn", "ContentStack", "VBoxContainer"),
            ("assets/ui/panels/WaterTreatmentPanel.tscn", "DetailText", "Label"),
            ("assets/ui/panels/WaterTreatmentPanel.tscn", "CharcoalButton", "Button"),
            ("assets/ui/panels/WaterTreatmentPanel.tscn", "DistillButton", "Button"),
            ("assets/ui/panels/WaterTreatmentPanel.tscn", "OsmosisButton", "Button"),
            ("assets/ui/panels/WaterTreatmentPanel.tscn", "ReplaceFilterButton", "Button"),
            ("assets/ui/panels/PharmaLabPanel.tscn", "RecipeListContainer", "VBoxContainer"),
            ("assets/ui/panels/PharmaLabPanel.tscn", "DetailContainer", "VBoxContainer"),
            ("assets/ui/panels/PharmaLabPanel.tscn", "LabStatusHeader", "Label"),
            ("assets/ui/modals/OpeningProtocolModal.tscn", "Backdrop", "ColorRect"),
            ("assets/ui/modals/OpeningProtocolModal.tscn", "Title", "Label"),
            ("assets/ui/modals/OpeningProtocolModal.tscn", "CloseButton", "Button"),
            ("assets/ui/modals/SafeCrackModal.tscn", "Margin", "MarginContainer"),
            ("assets/ui/modals/SafeCrackModal.tscn", "Root", "VBoxContainer"),
            ("assets/ui/modals/SafeCrackModal.tscn", "HeaderLabel", "Label"),
            ("assets/ui/modals/DailyBriefingModal.tscn", "TitleLabel", "Label"),
            ("assets/ui/modals/DailyBriefingModal.tscn", "BodyLabel", "RichTextLabel"),
            ("assets/ui/modals/DailyBriefingModal.tscn", "AckButton", "Button"),
        };

        [Fact]
        public void SceneBackedPanel_ContainsDeclaredUniqueNodeContracts()
        {
            string root = GetRepositoryRoot();
            var failures = new List<string>();
            var parsedScenes = new Dictionary<string, Dictionary<string, NodeInfo>>(StringComparer.Ordinal);
            var missingScenes = new HashSet<string>(StringComparer.Ordinal);

            foreach (var (relativeTscnPath, nodeName, expectedType) in DeclaredContracts)
            {
                string fullPath = Path.Combine(root, relativeTscnPath);
                if (missingScenes.Contains(relativeTscnPath))
                    continue;

                if (!File.Exists(fullPath))
                {
                    failures.Add($"Scene file {relativeTscnPath} does not exist on disk.");
                    missingScenes.Add(relativeTscnPath);
                    continue;
                }

                if (!parsedScenes.TryGetValue(relativeTscnPath, out var uniqueNodes))
                {
                    uniqueNodes = ParseTscnUniqueNodes(fullPath);
                    parsedScenes[relativeTscnPath] = uniqueNodes;
                }

                if (!uniqueNodes.TryGetValue(nodeName, out var node))
                {
                    failures.Add(
                        $"Scene {relativeTscnPath} is missing required unique node '%{nodeName}' " +
                        "(unique_name_in_owner = true).");
                    continue;
                }

                if (!string.Equals(expectedType, node.Type, StringComparison.Ordinal))
                {
                    failures.Add(
                        $"Scene {relativeTscnPath} node %{nodeName}: expected type {expectedType}, " +
                        $"got {node.Type}.");
                }
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }
    }
}
