// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    public sealed class UiWave4SourceContractTests
    {
        [Fact]
        public void FactionsPanel_RebindsAndRemovesNamedCollaboratorHandlers()
        {
            string source = ReadSource("src/UI/FactionsPanel.cs");

            Assert.Contains(
                "Unbind(clearPresentation: false);\n            _informantNetwork = informantNetwork;",
                source);
            Assert.Contains("_yearOfAsh.Warlord.OnTributeSettled += HandleWarlordTributeSettled;", source);
            Assert.Contains("_yearOfAsh.Warlord.OnTributeSettled -= HandleWarlordTributeSettled;", source);
            Assert.Contains("_yearOfAsh.Warlord.OnTributeDemanded += HandleWarlordTributeDemanded;", source);
            Assert.Contains("_yearOfAsh.Warlord.OnTributeDemanded -= HandleWarlordTributeDemanded;", source);
            Assert.Contains("_informantNetwork.StateChanged -= RefreshView;", source);
            Assert.Contains("_branchCoordinator.OnStateChanged -= RefreshView;", source);
            Assert.Contains("_muster.StateChanged -= RefreshView;", source);
            Assert.Contains("_expansions.StateChanged -= RefreshView;", source);
            Assert.Contains("AshfallUiHelpers.EmptyChildren(_overviewContainer);", source);
            Assert.DoesNotContain("OnTributeSettled += (", source);
            Assert.DoesNotContain("OnTributeDemanded += (", source);
        }

        [Fact]
        public void EventDrivenPanelsUnbindOnPredelete()
        {
            string[] paths =
            {
                "src/UI/FactionsPanel.cs",
                "src/UI/WeatherPanel.cs",
                "src/UI/FeedbackPanel.cs",
                "src/UI/ShelterDecorPanel.cs",
                "src/Journal/JournalBookUI.cs"
            };

            foreach (string path in paths)
            {
                string source = ReadSource(path);
                Assert.Contains("public override void _Notification(int what)", source);
                Assert.Contains(
                    "if (what == NotificationPredelete)\n                Unbind(",
                    source);
            }
        }

        [Fact]
        public void WeatherPanelUnbindsBeforeSwitchingHostsAndUsesSharedBackdrop()
        {
            string source = ReadSource("src/UI/WeatherPanel.cs");

            Assert.Contains(
                "public void Bind(WeatherHostSession weather)\n        {\n            Unbind(refresh: false);",
                source);
            Assert.Contains(
                "public void Bind(WorldHostSession weather)\n        {\n            Unbind(refresh: false);",
                source);
            Assert.Contains("AddChild(AshfallUiHelpers.MakeBackdropOverlay());", source);
            Assert.Contains("_worldHost.Weather.OnWeatherChanged -= HandleWeatherChanged;", source);
            Assert.Contains("_weatherHost.System.OnWeatherChanged -= HandleWeatherChanged;", source);
        }

        [Fact]
        public void DashboardContentReplacementFreesThePreviousOwnedContent()
        {
            string source = ReadSource("src/UI/AshfallDashboardShell.cs");

            Assert.Contains("if (ReferenceEquals(ContentRef, content)", source);
            Assert.Contains("_contentStack.RemoveChild(previousContent);\n            previousContent.Free();", source);
        }

        [Fact]
        public void FlatSurfacePanelsUseThemeRadiusAndBackdropTokens()
        {
            string confirmation = ReadSource("src/UI/ConfirmationModal.cs");
            string feedback = ReadSource("src/UI/FeedbackPanel.cs");
            string journal = ReadSource("src/Journal/JournalBookUI.cs");

            Assert.Contains("styleBox.SetCornerRadiusAll(DesignTheme.RadiusSm);", confirmation);
            Assert.Contains("style.SetCornerRadiusAll(DesignTheme.RadiusSm);", feedback);
            Assert.Contains("bg.SetCornerRadiusAll(Ashfall.Core.UI.Theme.RadiusSm);", journal);
            Assert.Contains("DesignTheme.Ink.r", confirmation);
            Assert.Contains("DesignTheme.SurfaceCard.r", feedback);
            Assert.DoesNotMatch(@"CornerRadius(?:BottomLeft|BottomRight|TopLeft|TopRight)\s*=\s*[1-9]", confirmation);
            Assert.DoesNotMatch(@"CornerRadius(?:BottomLeft|BottomRight|TopLeft|TopRight)\s*=\s*[1-9]", feedback);
            Assert.DoesNotMatch(@"SetCornerRadiusAll\s*\(\s*[1-9]", journal);

            foreach (string path in new[]
            {
                "src/UI/FactionsPanel.cs",
                "src/UI/WeatherPanel.cs",
                "src/UI/ShelterPanel.cs",
                "src/UI/MedicalPanel.cs"
            })
                Assert.Contains("MakeBackdropOverlay()", ReadSource(path));
        }

        [Fact]
        public void RefreshingPanelsUseSharedChildAndActionHelpers()
        {
            string[] childPanels =
            {
                "src/UI/BlackMarketPanel.cs",
                "src/UI/ShelterDecorPanel.cs",
                "src/UI/ShelterPanel.cs",
                "src/UI/CraftingPanel.cs",
                "src/UI/MedicalPanel.cs",
                "src/UI/CryogenicPermafrostCorePanel.cs",
                "src/UI/BoreholeSeismographPanel.cs",
                "src/UI/BasalRadonMigrationPanel.cs"
            };

            foreach (string path in childPanels)
            {
                string source = ReadSource(path);
                Assert.DoesNotContain("ClearChildren(", source);
                Assert.Contains("AshfallUiHelpers.EmptyChildren(", source);
            }

            string helpers = ReadSource("src/UI/AshfallUiHelpers.cs");
            Assert.Contains("public static Button? AddActionButton(", helpers);

            string[] actionPanels =
            {
                "src/UI/EbPvdCoatingPanel.cs",
                "src/UI/MicrofluidicDiagnosticPanel.cs",
                "src/UI/RailGrindingPanel.cs",
                "src/UI/MineFlailPanel.cs"
            };
            foreach (string path in actionPanels)
            {
                string source = ReadSource(path);
                Assert.Contains("AshfallUiHelpers.EmptyChildren(_actionsBox);", source);
                Assert.Contains("AshfallUiHelpers.AddActionButton(", source);
            }
        }

        [Fact]
        public void FactionSignatureQuoteWrapsToItsAvailableWidth()
        {
            string source = ReadSource("src/UI/FactionsPanel.cs");

            Assert.Contains("MakeSmall(f.SignatureQuote, autowrap: true)", source);
            Assert.Contains("quoteLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;", source);
        }

        private static string ReadSource(string relativePath) =>
            File.ReadAllText(Path.Combine(FindRepositoryRoot(), relativePath));

        private static string FindRepositoryRoot()
        {
            foreach (string start in new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory })
            {
                var directory = new DirectoryInfo(Path.GetFullPath(start));
                while (directory != null)
                {
                    if (File.Exists(Path.Combine(directory.FullName, "project.godot")) &&
                        Directory.Exists(Path.Combine(directory.FullName, "src")))
                        return directory.FullName;
                    directory = directory.Parent!;
                }
            }

            throw new DirectoryNotFoundException(
                "Could not locate the ASHFALL repository root from the current test directory.");
        }
    }
}
