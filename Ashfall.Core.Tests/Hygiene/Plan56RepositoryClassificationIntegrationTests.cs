// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Hygiene;
using Xunit;

namespace Ashfall.Core.Tests.Hygiene
{
    public sealed class Plan56RepositoryClassificationIntegrationTests
    {
        [Fact]
        public void RepositoryClassificationPolicy_ClassifiesPathsAcrossCategories()
        {
            var policy = new RepositoryClassificationPolicy();

            Assert.Equal(RepositoryArtifactCategory.DataAuthority,
                policy.ClassifyPath("Assets/StreamingAssets/Data/items.json"));

            Assert.Equal(RepositoryArtifactCategory.SourceCode,
                policy.ClassifyPath("Assets/Ashfall.Core/Records/RetentionPolicy.cs"));

            Assert.Equal(RepositoryArtifactCategory.SourceCode,
                policy.ClassifyPath("src/Host/AssetRegistry.cs"));

            Assert.Equal(RepositoryArtifactCategory.TestProject,
                policy.ClassifyPath("Ashfall.Core.Tests/Hygiene/Plan56RepositoryClassificationIntegrationTests.cs"));

            Assert.Equal(RepositoryArtifactCategory.ShippedAssets,
                policy.ClassifyPath("assets/sprites/Items/pistol_cz75_9x19.png"));

            Assert.Equal(RepositoryArtifactCategory.DesignMockups,
                policy.ClassifyPath("assets/ui/Screens/01_terminal_mockup.png"));

            Assert.Equal(RepositoryArtifactCategory.GeneratedArtifacts,
                policy.ClassifyPath("artifacts/asset_registry.json"));

            Assert.Equal(RepositoryArtifactCategory.Ephemera,
                policy.ClassifyPath(".godot/imported/sample.png"));
        }

        [Fact]
        public void RepositoryClassificationPolicy_DetectsProhibitedRootFiles()
        {
            var policy = new RepositoryClassificationPolicy();

            Assert.Equal(RepositoryArtifactCategory.ProhibitedOrphan,
                policy.ClassifyPath("batch20-playmode-results.xml"));

            Assert.Equal(RepositoryArtifactCategory.ProhibitedOrphan,
                policy.ClassifyPath("art-wiring-results.xml"));

            Assert.Equal(RepositoryArtifactCategory.ProhibitedOrphan,
                policy.ClassifyPath("fix_queuefree.py"));

            Assert.Equal(RepositoryArtifactCategory.ProhibitedOrphan,
                policy.ClassifyPath("safe_fix.py"));

            Assert.Equal(RepositoryArtifactCategory.ProhibitedOrphan,
                policy.ClassifyPath("test_parse.py"));
        }

        [Fact]
        public void RepositoryClassificationPolicy_AssetFamilyBudgetsEnforceLimits()
        {
            var policy = new RepositoryClassificationPolicy();

            Assert.True(policy.TryGetBudget("art", out var artBudget));
            Assert.NotNull(artBudget);
            Assert.Equal(85L * 1024 * 1024, artBudget!.MaxBudgetBytes);

            Assert.True(policy.TryGetBudget("ui", out var uiBudget));
            Assert.NotNull(uiBudget);
            Assert.Equal(35L * 1024 * 1024, uiBudget!.MaxBudgetBytes);

            // Test usage calculation
            var sampleBudget = new AssetFamilyBudget("sprites", 10 * 1024 * 1024, 8 * 1024 * 1024);
            Assert.True(sampleBudget.IsWithinBudget);
            Assert.Equal(80.0, sampleBudget.UsagePercentage);

            var overBudget = new AssetFamilyBudget("sprites", 10 * 1024 * 1024, 12 * 1024 * 1024);
            Assert.False(overBudget.IsWithinBudget);
            Assert.Equal(120.0, overBudget.UsagePercentage);
        }

        [Fact]
        public void RepositoryClassificationPolicy_FiresSeamsOnClassificationAndViolation()
        {
            var policy = new RepositoryClassificationPolicy();

            string? classifiedPath = null;
            RepositoryArtifactCategory? reportedCategory = null;
            string? violationPath = null;
            string? violationReason = null;

            policy.OnPathClassifiedSeam = (p, cat) =>
            {
                classifiedPath = p;
                reportedCategory = cat;
            };

            policy.OnHygieneViolationSeam = (p, reason) =>
            {
                violationPath = p;
                violationReason = reason;
            };

            var catResult = policy.ClassifyPath("batch20-playmode-results.xml");

            Assert.Equal(RepositoryArtifactCategory.ProhibitedOrphan, catResult);
            Assert.Equal("batch20-playmode-results.xml", classifiedPath);
            Assert.Equal(RepositoryArtifactCategory.ProhibitedOrphan, reportedCategory);

            Assert.Equal("batch20-playmode-results.xml", violationPath);
            Assert.NotNull(violationReason);
        }
    }
}
