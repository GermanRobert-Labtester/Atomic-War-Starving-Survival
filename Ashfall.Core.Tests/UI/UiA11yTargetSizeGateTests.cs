// SPDX-License-Identifier: MIT
// A11Y-P2.10-TARGET-SIZES-2026-09-29 — static drift gate for the UI
// accessibility target-size package (plan .ai/plans/ui-a11y-target-sizes-2026-09-29.md,
// findings docs/ui/ACCESSIBILITY_REPORT_2026-09-29.md §5d/§9.10).
// Pattern: MainTriadDriftGateTests / UiA11yP1InputGateTests.
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class UiA11yTargetSizeGateTests
    {
        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (Directory.Exists(Path.Combine(dir, "src")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        private static string ReadSrc(params string[] parts)
            => File.ReadAllText(Path.Combine(new[] { RepoRoot() }.Concat(parts).ToArray()));

        [Fact]
        public void Theme_DefinesTheInteractiveFloor()
        {
            string src = ReadSrc("Assets", "Ashfall.Core", "UI", "Theme.cs");
            Assert.Matches(@"public const int MinInteractiveHeight = 28;", src);
        }

        [Fact]
        public void MakeButton_UsesTheInteractiveFloor_NotFontCoupledHeight()
        {
            string src = ReadSrc("src", "UI", "AshfallUiHelpers.cs");
            Assert.Contains("CustomMinimumSize = new Vector2(0, Theme.MinInteractiveHeight)", src);
            Assert.DoesNotContain("new Vector2(0, Theme.FontSizeBody + Theme.SpacingMd)", src);
        }

        // Every interactive control swept in the 2026-09-29 package must keep
        // its height at >= 28. Theory input: (file, old literal that must not return).
        [Theory]
        [InlineData("ShelterBarterPanel", "Vector2(24, 22)")]
        [InlineData("FeedbackPanel", "Vector2(24, 24)")]
        [InlineData("InventoryPanel", "Vector2(64, 24)")]
        [InlineData("InventoryPanel", "Vector2(82, 24)")]
        [InlineData("PowerGridPanel", "Vector2(56, 24)")]
        [InlineData("PowerGridPanel", "Vector2(60, 24)")]
        [InlineData("PowerGridPanel", "Vector2(220, 26)")]
        [InlineData("PowerGridPanel", "Vector2(280, 26)")]
        [InlineData("PowerGridPanel", "Vector2(180, 26)")]
        [InlineData("RadioPanel", "Vector2(110, 26)")]
        [InlineData("RadioPanel", "Vector2(90, 26)")]
        [InlineData("DefenseGridPanel", "Vector2(0, 26)")]
        [InlineData("RoboticsWorkshopPanel", "Vector2(0, 26)")]
        [InlineData("FungiCultivationBedPanel", "Vector2(0, 26)")]
        [InlineData("BioFermentationPanel", "CustomMinimumSize = new Vector2(0, 26) }")]
        [InlineData("JusticeTribunalPanel", "Vector2(0, 26)")]
        [InlineData("ArchiveDeskPanel", "Vector2(0, 24)")]
        [InlineData("ChemicalDependencyPanel", "Vector2(0, 24)")]
        [InlineData("DecontaminationPanel", "Vector2(0, 24)")]
        [InlineData("ContractorRosterPanel", "Vector2(0, 24)")]
        [InlineData("MedicalWardPanel", "Vector2(0, 24)")]
        [InlineData("LibraryStudyPanel", "Vector2(0, 24)")]
        [InlineData("KitchenNutritionPanel", "Vector2(0, 24)")]
        [InlineData("PhantomMemoryPanel", "Vector2(0, 24)")]
        [InlineData("EquipmentConditionPanel", "Vector2(0, 24)")]
        [InlineData("MentalHealthCrisisPanel", "Vector2(0, 24)")]
        [InlineData("GreenhousePanel", "Vector2(0, 24)")]
        [InlineData("SumpFloodingPanel", "Vector2(0, 24)")]
        [InlineData("TravelingCaravanPanel", "Vector2(0, 24)")]
        public void SweptInteractiveControls_StayAtOrAbove28(string fileName, string forbiddenLiteral)
        {
            string src = ReadSrc("src", "UI", $"{fileName}.cs");
            Assert.DoesNotContain(forbiddenLiteral, src);
        }
    }
}
