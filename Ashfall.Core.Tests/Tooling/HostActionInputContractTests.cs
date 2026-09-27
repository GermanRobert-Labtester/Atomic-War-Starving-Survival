// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    public sealed class HostActionInputContractTests
    {
        private static string RepoRoot()
        {
            string? directory = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 12 && directory != null; i++)
            {
                if (File.Exists(Path.Combine(directory, "Ashfall.csproj")))
                    return directory;
                directory = Directory.GetParent(directory)?.FullName;
            }

            throw new DirectoryNotFoundException("Could not locate the repository root from the test output directory.");
        }

        [Fact]
        public void GeothermalCasingInput_IsInvariantFiniteAndUserVisibleOnFailure()
        {
            string source = File.ReadAllText(Path.Combine(RepoRoot(), "src", "Main.World.cs"));

            Assert.Contains(
                "float.TryParse(casingInput, NumberStyles.Float, CultureInfo.InvariantCulture",
                source);
            Assert.Contains("|| !float.IsFinite(casingDepth)", source);
            Assert.Contains("Invalid casing depth. Enter a positive finite number", source);
            Assert.DoesNotContain("InstallCasing(float.Parse(param ?? \"100\"))", source);
        }
    }
}
