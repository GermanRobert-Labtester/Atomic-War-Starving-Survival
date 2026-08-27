using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    public class InputMapGateTests
    {
        private static string FindRepoRoot()
        {
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                if (File.Exists(Path.Combine(search, "project.godot")))
                    return search;
                string parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return AppContext.BaseDirectory;
        }

        private static readonly string[] CanonicalActions = new[]
        {
            "ashfall_close",
            "ashfall_confirm",
            "ashfall_next_tab",
            "ashfall_nav_up",
            "ashfall_nav_down",
            "ashfall_nav_left",
            "ashfall_nav_right",
            "ashfall_journal",
            "ashfall_help",
            "ashfall_forecast",
            "ashfall_weather_history",
            "ashfall_events",
            "ashfall_expeditions",
            "ashfall_holdfast",
            "ashfall_journal_tab_1",
            "ashfall_journal_tab_2",
            "ashfall_journal_tab_3",
            "ashfall_journal_tab_4",
            "ashfall_journal_tab_5",
            "ashfall_holdfast_build",
            "ashfall_holdfast_status"
        };

        [Fact]
        public void ProjectGodot_ContainsInputSection_WithAllCanonicalActions()
        {
            string repoRoot = FindRepoRoot();
            string projectGodotPath = Path.Combine(repoRoot, "project.godot");
            Assert.True(File.Exists(projectGodotPath), $"project.godot must exist at {projectGodotPath}");

            string content = File.ReadAllText(projectGodotPath);
            Assert.Contains("[input]", content);

            foreach (string action in CanonicalActions)
            {
                Assert.True(
                    content.Contains($"{action}="),
                    $"project.godot [input] section is missing canonical action '{action}'");
            }
        }

        [Fact]
        public void GlobalShortcutHandlers_UseInputMapActions_NotRawKeycodes()
        {
            string repoRoot = FindRepoRoot();
            string mainAppPath = Path.Combine(repoRoot, "src", "Main.Application.cs");
            string mainFlowPath = Path.Combine(repoRoot, "src", "Main.GameFlow.cs");
            string holdfastPath = Path.Combine(repoRoot, "src", "Host", "HoldfastTerminalPanel.cs");
            string briefingPath = Path.Combine(repoRoot, "src", "UI", "DailyBriefingModal.cs");
            string gameOverPath = Path.Combine(repoRoot, "src", "UI", "GameOverPanel.cs");

            var files = new[] { mainAppPath, mainFlowPath, holdfastPath, briefingPath, gameOverPath };

            foreach (var path in files)
            {
                if (!File.Exists(path)) continue;
                string content = File.ReadAllText(path);

                // These core dispatch files must use AshfallInputActions rather than raw Key. checks
                Assert.Contains("AshfallInputActions", content);
                Assert.DoesNotMatch(@"key\.Keycode\s*==\s*Key\.(Escape|Enter|Space|Tab|F|H|J|F1|E|B|S|Key1|Key2|Key3|Key4|Key5)", content);
            }
        }

        [Fact]
        public void AshfallInputActions_ExposesAllCanonicalActionConstants()
        {
            string repoRoot = FindRepoRoot();
            string actionsFile = Path.Combine(repoRoot, "src", "Host", "AshfallInputActions.cs");
            Assert.True(File.Exists(actionsFile), $"AshfallInputActions.cs must exist at {actionsFile}");

            string content = File.ReadAllText(actionsFile);
            foreach (string action in CanonicalActions)
            {
                Assert.Contains($"\"{action}\"", content);
            }
        }
    }
}
