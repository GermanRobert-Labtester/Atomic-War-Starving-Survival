// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: UI Panel Accessibility Gate Tests.
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    [IntegrationTest]
    [UnitTest]
    public class UiAccessibilityGateTests
    {
        private static string RepoRoot()
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
                    string probeProps = Path.Combine(dir.FullName, "Directory.Packages.props");
                    if (File.Exists(probeProps))
                        return dir.FullName;
                    dir = dir.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate repository root from test execution context.");
        }

        [Fact]
        public void HostCliRegistry_RegistersUiAccessibilitySelfTest()
        {
            var desc = HostCliRegistry.AllDescriptors.FirstOrDefault(d => d.Action == HostCliAction.UiAccessibilitySelfTest);
            Assert.NotNull(desc);
            Assert.Equal("--ui-accessibility-selftest", desc.PrimaryFlag);
            Assert.True(desc.IsSelfTest);
            Assert.True(desc.IsTest);
            Assert.True(desc.HeadlessCompatible);
            Assert.Equal("ui_accessibility_selftest", desc.TestId);
            Assert.Contains("--accessibility-selftest", desc.Aliases);
        }

        [Fact]
        public void HostCliRegistry_ResolvesAccessibilityFlags()
        {
            Assert.Equal(HostCliAction.UiAccessibilitySelfTest, HostCliRegistry.Resolve(new[] { "--ui-accessibility-selftest" }));
            Assert.Equal(HostCliAction.UiAccessibilitySelfTest, HostCliRegistry.Resolve(new[] { "--accessibility-selftest" }));
            Assert.Equal(HostCliAction.UiAccessibilitySelfTest, HostCliRegistry.Resolve(new[] { "--ui-a11y-selftest" }));
        }

        [Fact]
        public void UiPanelSourceFiles_DoNotContainCorruptPlaceholders()
        {
            string root = RepoRoot();
            string uiDir = Path.Combine(root, "src", "UI");

            if (!Directory.Exists(uiDir)) return;

            var panelFiles = Directory.EnumerateFiles(uiDir, "*Panel*.cs", SearchOption.AllDirectories)
                .Where(f => !f.EndsWith("Test.cs") && !f.EndsWith("Tests.cs"))
                .ToList();

            foreach (var file in panelFiles)
            {
                string text = File.ReadAllText(file);
                Assert.DoesNotContain("\"[PLACEHOLDER]\"", text);
                Assert.DoesNotContain("\"[MISSING]\"", text);
            }
        }
    }
}
