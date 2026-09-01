// SPDX-License-Identifier: MIT
// ASHFALL CI Gate: Composition Root & StartNewGame Invariant Gate (Task 131).
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    public sealed class CompositionRootArchitectureGateTests
    {
        private static string FindRepoRoot()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            while (!string.IsNullOrEmpty(dir))
            {
                if (File.Exists(Path.Combine(dir, "project.godot")))
                    return dir;
                dir = Path.GetDirectoryName(dir);
            }
            throw new InvalidOperationException("Could not locate repository root from test execution directory.");
        }

        [Fact]
        public void StartNewGame_CallsComposeCampaign_BeforeOpeningPanels()
        {
            var repoRoot = FindRepoRoot();
            var gameFlowPath = Path.Combine(repoRoot, "src", "Main.GameFlow.cs");
            Assert.True(File.Exists(gameFlowPath), $"Main.GameFlow.cs must exist at {gameFlowPath}");

            var code = File.ReadAllText(gameFlowPath);
            int startNewGameIdx = code.IndexOf("private void StartNewGame()", StringComparison.Ordinal);
            Assert.True(startNewGameIdx >= 0, "StartNewGame() method must exist in Main.GameFlow.cs");

            int composeIdx = code.IndexOf("ComposeCampaign();", startNewGameIdx, StringComparison.Ordinal);
            Assert.True(composeIdx > startNewGameIdx, "StartNewGame() must call ComposeCampaign()");

            int openProtocolIdx = code.IndexOf("_openingProtocolModal.Open();", startNewGameIdx, StringComparison.Ordinal);
            if (openProtocolIdx >= 0)
            {
                Assert.True(composeIdx < openProtocolIdx, "ComposeCampaign() must be called BEFORE opening the opening protocol modal");
            }
        }

        [Fact]
        public void MainCampaignServices_DefinesAuthoritativeComposeCampaign()
        {
            var repoRoot = FindRepoRoot();
            var campaignServicesPath = Path.Combine(repoRoot, "src", "Main.CampaignServices.cs");
            Assert.True(File.Exists(campaignServicesPath), $"Main.CampaignServices.cs must exist at {campaignServicesPath}");

            var code = File.ReadAllText(campaignServicesPath);
            Assert.Contains("public void ComposeCampaign()", code);
            Assert.Contains("_composeCampaignCallCount++", code);
            Assert.Contains("SetupCampaignDay()", code);
            Assert.Contains("SetupHoldfastRuntime()", code);
            Assert.Contains("SetupInventory()", code);
            Assert.Contains("SetupSurvivors()", code);
            Assert.Contains("SetupWorld()", code);
            Assert.Contains("SetupMedical()", code);
            Assert.Contains("SetupExpandedShelterSystems()", code);
        }

        [Fact]
        public void AllShelterAndSocialSetupMethods_HaveNullGuards()
        {
            var repoRoot = FindRepoRoot();
            string[] sourceFiles = new[]
            {
                Path.Combine(repoRoot, "src", "Main.ShelterInfrastructure.cs"),
                Path.Combine(repoRoot, "src", "Main.ShelterSocial.cs"),
                Path.Combine(repoRoot, "src", "Main.ShelterBatch3.cs"),
                Path.Combine(repoRoot, "src", "Main.Quests.cs")
            };

            var methodRegex = new Regex(@"private\s+void\s+(Setup\w+)\s*\([^)]*\)\s*\{([^}]+)\}", RegexOptions.Singleline);
            var guardRegex = new Regex(@"if\s*\(\s*_\w+\s*!=\s*null\s*\)\s*return\s*;", RegexOptions.Singleline);

            foreach (var file in sourceFiles)
            {
                if (!File.Exists(file)) continue;
                var code = File.ReadAllText(file);
                var matches = methodRegex.Matches(code);

                foreach (Match match in matches)
                {
                    string methodName = match.Groups[1].Value;
                    string methodBody = match.Groups[2].Value;

                    // Skip methods that are orchestrators or composite wrappers
                    if (methodName == "SetupEventsHost" ||
                        methodName.StartsWith("SetupWaterTreatment") ||
                        methodName.StartsWith("SetupAirlockSecurity") ||
                        methodName.StartsWith("SetupShelterThermal") ||
                        methodName.StartsWith("SetupAutopsy") ||
                        methodName.StartsWith("SetupWaystation") ||
                        methodName.StartsWith("SetupRegionalTreaty") ||
                        methodName.StartsWith("SetupVinylMorale") ||
                        methodName.StartsWith("SetupWildlifeTrapping") ||
                        methodName.StartsWith("SetupExcavation") ||
                        methodName.StartsWith("SetupApprenticeship") ||
                        methodName.StartsWith("SetupCaregiving") ||
                        methodName.StartsWith("SetupMentalHealthCrisis") ||
                        methodName.StartsWith("SetupSumpFlooding") ||
                        methodName.StartsWith("SetupDecontamination") ||
                        methodName.StartsWith("SetupKitchenNutrition") ||
                        methodName.StartsWith("SetupEquipmentCondition") ||
                        methodName.StartsWith("SetupLibraryStudy") ||
                        methodName.StartsWith("SetupArchiveDesk") ||
                        methodName.StartsWith("SetupContractorRoster") ||
                        methodName.StartsWith("SetupShelterAssignment"))
                    {
                        bool hasGuard = guardRegex.IsMatch(methodBody);
                        Assert.True(hasGuard, $"Method {methodName} in {Path.GetFileName(file)} must contain an 'if (_system != null) return;' null guard.");
                    }
                }
            }
        }
    }
}
