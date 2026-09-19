// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Text.RegularExpressions;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests.Release
{
    public class ReleaseVersionContractTests
    {
        private static string FindRepoRoot()
        {
            var dir = new DirectoryInfo(Path.GetFullPath(AppContext.BaseDirectory));
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            throw new FileNotFoundException(
                "Could not locate the repository root (Assets/StreamingAssets/Data) from the test run");
        }

        [Theory]
        [InlineData("1.2.3", "1.2.3")]
        [InlineData("0.0.1", "0.0.1")]
        [InlineData("10.20.30", "10.20.30")]
        [InlineData("1.1.0", "1.1.0")]
        [InlineData("  1.2.3  ", "1.2.3")]
        public void TryParse_ValidSemver_ReturnsTrueAndNormalized(string input, string expected)
        {
            bool ok = ReleaseVersion.TryParse(input, out string normalized);
            Assert.True(ok);
            Assert.Equal(expected, normalized);
        }

        [Theory]
        [InlineData("1.2")]
        [InlineData("v1.2.3")]
        [InlineData("1.2.3.4")]
        [InlineData("1.2.3-rc")]
        [InlineData("1.2.3+build")]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        [InlineData("01.2.3")]
        [InlineData("1.02.3")]
        [InlineData("1.2.03")]
        [InlineData("abc")]
        [InlineData("-1.0.0")]
        public void TryParse_InvalidSemver_ReturnsFalse(string? input)
        {
            bool ok = ReleaseVersion.TryParse(input, out string normalized);
            Assert.False(ok);
            Assert.Equal(string.Empty, normalized);
        }

        [Fact]
        public void TryParse_Components_ExtractsIntegersCorrectly()
        {
            bool ok = ReleaseVersion.TryParse("12.34.56", out int major, out int minor, out int patch);
            Assert.True(ok);
            Assert.Equal(12, major);
            Assert.Equal(34, minor);
            Assert.Equal(56, patch);
        }

        [Theory]
        [InlineData("1.1.0", "1.1.1", ReleaseBumpKind.Patch)]
        [InlineData("1.1.0", "1.2.0", ReleaseBumpKind.Minor)]
        [InlineData("1.1.0", "2.0.0", ReleaseBumpKind.Major)]
        [InlineData("1.1.0", "1.0.9", ReleaseBumpKind.Invalid)]
        [InlineData("1.1.0", "1.1.0", ReleaseBumpKind.Invalid)]
        [InlineData("1.1.0", "garbage", ReleaseBumpKind.Invalid)]
        [InlineData(null, "1.1.0", ReleaseBumpKind.Invalid)]
        [InlineData("1.1.0", null, ReleaseBumpKind.Invalid)]
        public void ClassifyBump_ExpectedCategories(string? oldVer, string? newVer, ReleaseBumpKind expected)
        {
            ReleaseBumpKind kind = ReleaseVersion.ClassifyBump(oldVer, newVer);
            Assert.Equal(expected, kind);
        }

        [Fact]
        public void Format_ComposesNormalizedString()
        {
            string formatted = ReleaseVersion.Format(1, 2, 3);
            Assert.Equal("1.2.3", formatted);
        }

        [Fact]
        public void ProjectGodot_HasStrictSemverVersion()
        {
            string repoRoot = FindRepoRoot();
            string projectGodotPath = Path.Combine(repoRoot, "project.godot");
            string text = File.ReadAllText(projectGodotPath);

            var match = Regex.Match(text, @"config/version=""([^""]+)""");
            Assert.True(match.Success, "config/version not found in project.godot");

            string rawVersion = match.Groups[1].Value;
            bool ok = ReleaseVersion.TryParse(rawVersion, out string normalized);
            Assert.True(ok, $"project.godot version '{rawVersion}' must be strict semver");
            Assert.Equal(rawVersion, normalized);
        }

        [Fact]
        public void DirectoryBuildProps_VersionPrefix_MatchesProjectGodot()
        {
            string repoRoot = FindRepoRoot();
            string godotPath = Path.Combine(repoRoot, "project.godot");
            string propsPath = Path.Combine(repoRoot, "Directory.Build.props");

            string godotText = File.ReadAllText(godotPath);
            string propsText = File.ReadAllText(propsPath);

            var godotMatch = Regex.Match(godotText, @"config/version=""([^""]+)""");
            var propsMatch = Regex.Match(propsText, @"<VersionPrefix>([^<]+)</VersionPrefix>");

            Assert.True(godotMatch.Success, "config/version not found in project.godot");
            Assert.True(propsMatch.Success, "<VersionPrefix> not found in Directory.Build.props");

            Assert.Equal(godotMatch.Groups[1].Value, propsMatch.Groups[1].Value);
        }

        [Fact]
        public void ExportPresets_WindowsDesktop_Versions_MatchProjectGodot()
        {
            string repoRoot = FindRepoRoot();
            string godotPath = Path.Combine(repoRoot, "project.godot");
            string exportPresetsPath = Path.Combine(repoRoot, "export_presets.cfg");

            string godotText = File.ReadAllText(godotPath);
            string presetsText = File.ReadAllText(exportPresetsPath);

            var godotMatch = Regex.Match(godotText, @"config/version=""([^""]+)""");
            Assert.True(godotMatch.Success, "config/version not found in project.godot");
            string godotVersion = godotMatch.Groups[1].Value;

            var fileVerMatch = Regex.Match(presetsText, @"application/file_version=""([^""]+)""");
            var prodVerMatch = Regex.Match(presetsText, @"application/product_version=""([^""]+)""");

            Assert.True(fileVerMatch.Success, "application/file_version not found in export_presets.cfg");
            Assert.True(prodVerMatch.Success, "application/product_version not found in export_presets.cfg");

            Assert.Equal(godotVersion, fileVerMatch.Groups[1].Value);
            Assert.Equal(godotVersion, prodVerMatch.Groups[1].Value);
        }

        [Fact]
        public void VersionReport_Compose_WithInvalidMarker_RendersInvalidLine()
        {
            const string invalidMarker = "INVALID (config/version missing or not semver)";
            string report = VersionReport.Compose(invalidMarker, dataDir: null);

            Assert.Contains($"game         : {invalidMarker}", report);
        }
    }
}
