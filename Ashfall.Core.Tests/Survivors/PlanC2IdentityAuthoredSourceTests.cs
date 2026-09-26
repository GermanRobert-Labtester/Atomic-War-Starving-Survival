// SPDX-License-Identifier: MIT
// ============================================================================
// C2[17] — authored survivor identity: runtime reads the authored belief
// field or registers nothing; the trait-keyword shadow inference is retired.
// ============================================================================
using System;
using System.IO;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class PlanC2IdentityAuthoredSourceTests
    {
        private static string RepoRoot()
        {
            var directory = new DirectoryInfo(Path.GetFullPath(AppContext.BaseDirectory));
            while (directory != null)
            {
                if (File.Exists(Path.Combine(directory.FullName, "src", "Main.SurvivorSocial.cs")))
                    return directory.FullName;
                directory = directory.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate the ASHFALL repository root.");
        }

        [Fact]
        public void BeliefRegistration_IsAuthoredOnly_NoTraitKeywordInference()
        {
            string source = File.ReadAllText(Path.Combine(RepoRoot(), "src", "Main.SurvivorSocial.cs"));
            Assert.DoesNotContain("InferBeliefProfile", source);
            Assert.DoesNotContain("military_discipline\";", source);
            Assert.Contains("fields?.belief_profile_id ?? string.Empty", source);
            Assert.Contains("RegisterBelief(entry.survivorId, belief)", source);
        }

        [Fact]
        public void AuthoredBeliefFields_ExistInData_AndUseCanonicalVocabulary()
        {
            string data = File.ReadAllText(Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data", "expansion_survivor_fields.json"));
            Assert.Contains("\"belief_profile_id\"", data);
            foreach (var canonical in new[] { "military_discipline", "religious_faith", "atheist_rationalist", "collectivist_solidarity" })
            {
                Assert.Contains(canonical, data);
            }
        }
    }
}
