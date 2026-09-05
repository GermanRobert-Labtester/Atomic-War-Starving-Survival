// SPDX-License-Identifier: MIT
// ASHFALL input verb routing and rebinding architecture gate (Plan 81 / Task B22).

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using Xunit;
using Ashfall.Core.Settings;

namespace Ashfall.Core.Tests.UI
{
    public class InputVerbRoutingTests
    {
        private static string FindRepoRoot()
        {
            string[] candidates =
            {
                Directory.GetCurrentDirectory(),
                AppContext.BaseDirectory
            };
            foreach (string start in candidates)
            {
                var dir = new DirectoryInfo(Path.GetFullPath(start));
                while (dir != null)
                {
                    string probeSrc = Path.Combine(dir.FullName, "src");
                    string probeAssets = Path.Combine(dir.FullName, "Assets");
                    if (Directory.Exists(probeSrc) && Directory.Exists(probeAssets))
                        return dir.FullName;
                    dir = dir.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate repo root from the test run");
        }

        [Fact]
        public void UserSettingsData_KeyBindings_RoundTripsThroughCodec()
        {
            var original = new UserSettingsData();
            original.KeyBindings["ashfall_confirm"] = "Space";
            original.KeyBindings["ashfall_journal"] = "K";
            original.KeyBindings["ashfall_help"] = "F2";

            string json = UserSettingsCodec.Serialize(original);
            var (deserialized, diag) = UserSettingsCodec.DeserializeWithRecovery(json);

            Assert.Null(diag);
            Assert.NotNull(deserialized.KeyBindings);
            Assert.Equal("Space", deserialized.KeyBindings["ashfall_confirm"]);
            Assert.Equal("K", deserialized.KeyBindings["ashfall_journal"]);
            Assert.Equal("F2", deserialized.KeyBindings["ashfall_help"]);
        }

        [Fact]
        public void UserSettingsCodec_RecoversGracefullyFromCorruptedKeyBindings()
        {
            string badJson = "{\"schema_version\": 1, \"key_bindings\": null}";
            var (data, _) = UserSettingsCodec.DeserializeWithRecovery(badJson);
            Assert.NotNull(data);
            Assert.NotNull(data.KeyBindings);

            string invalidJson = "{\"schema_version\": 1, \"key_bindings\": 12345}";
            var (data2, _) = UserSettingsCodec.DeserializeWithRecovery(invalidJson);
            Assert.NotNull(data2);
            Assert.NotNull(data2.KeyBindings);
        }

        [Fact]
        public void InputMapAuditArtifact_ExistsAndHasCanonicalActions()
        {
            string root = FindRepoRoot();
            string auditPath = Path.Combine(root, "artifacts", "input-map-audit.json");
            Assert.True(File.Exists(auditPath), "artifacts/input-map-audit.json must exist");

            string json = File.ReadAllText(auditPath);
            using var doc = JsonDocument.Parse(json);
            var rootEl = doc.RootElement;

            Assert.True(rootEl.TryGetProperty("schema_version", out var sv) && sv.GetInt32() == 1);
            Assert.True(rootEl.TryGetProperty("total_actions", out var total) && total.GetInt32() >= 20);
            Assert.True(rootEl.TryGetProperty("canonical_verbs", out var verbs) && verbs.GetArrayLength() >= 20);
        }

        [Fact]
        public void CoreDashboardPanels_DoNotUseRawEscapeOutsideSettingsPanel()
        {
            string root = FindRepoRoot();
            string uiDir = Path.Combine(root, "src", "UI");

            string[] auditedPanels =
            {
                "InventoryPanel.cs",
                "ResearchAtlasPanel.cs",
                "ResearchPanel.cs",
                "CraftingPanel.cs",
                "MedicalPanel.cs",
                "StatusPanel.cs"
            };

            foreach (var panel in auditedPanels)
            {
                string fullPath = Path.Combine(uiDir, panel);
                Assert.True(File.Exists(fullPath), $"Panel file {panel} must exist");
                string content = File.ReadAllText(fullPath);

                // None of these panels should have raw Key.Escape
                Assert.DoesNotContain("Key.Escape", content);
            }
        }
    }
}
