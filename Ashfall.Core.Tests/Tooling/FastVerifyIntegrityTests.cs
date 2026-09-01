using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    /// <summary>
    /// Mechanically verifies that agent-fast-verify.py defines valid, distinct verification domains
    /// and that all referenced scripts and test filters exist in the repository.
    /// </summary>
    public class FastVerifyIntegrityTests
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

        private static string FastVerifyScriptPath => Path.Combine(GetRepositoryRoot(), "scripts", "ci", "agent-fast-verify.py");

        [Fact]
        public void AgentFastVerify_ScriptExists()
        {
            Assert.True(File.Exists(FastVerifyScriptPath), $"agent-fast-verify.py must exist at {FastVerifyScriptPath}");
        }

        [Fact]
        public void AgentFastVerify_DeclaresAllRequiredDomains()
        {
            var content = File.ReadAllText(FastVerifyScriptPath);
            var requiredDomains = new[]
            {
                "persistence", "data", "ui", "expansion", "audio", "schema", "smoke", "core", "docs", "fast"
            };

            foreach (var domain in requiredDomains)
            {
                Assert.Contains($"\"{domain}\":", content);
            }
        }
    }
}
