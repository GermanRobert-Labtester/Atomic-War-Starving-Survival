using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Ashfall.Core.Tests.Tooling;

public sealed class PlanGovernanceContractTests
{
    private static string RepositoryRoot
    {
        get
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "AGENTS.md")))
            {
                directory = directory.Parent;
            }

            return directory?.FullName ?? throw new DirectoryNotFoundException("ASHFALL repository root was not found");
        }
    }

    [Fact]
    public void GeneratedRegisterHasStableSchemaAndCorpusCoverage()
    {
        var path = Path.Combine(RepositoryRoot, "docs", "roadmap", "PLAN_REGISTER.json");
        Assert.True(File.Exists(path), $"Missing generated plan register: {path}");

        using var document = JsonDocument.Parse(File.ReadAllText(path));
        var root = document.RootElement;
        Assert.Equal(1, root.GetProperty("register_schema_version").GetInt32());
        Assert.Equal("scripts/ci/generate-plan-register.py", root.GetProperty("generated_by").GetString());

        var plans = root.GetProperty("plans");
        Assert.True(plans.GetArrayLength() >= 500, "The register must cover the current multi-namespace corpus");
        Assert.True(root.GetProperty("counts").GetProperty("plans").GetInt32() == plans.GetArrayLength());

        var namespaces = plans.EnumerateArray()
            .Select(plan => plan.GetProperty("namespace").GetString())
            .Where(value => value is not null)
            .ToHashSet(StringComparer.Ordinal);
        Assert.Contains("next_steps", namespaces);
        Assert.Contains("piagents", namespaces);
        Assert.Contains("integration", namespaces);
    }

    [Fact]
    public void LegacyPlansRemainVisibleAsMetadataMissing()
    {
        using var document = JsonDocument.Parse(File.ReadAllText(Path.Combine(RepositoryRoot, "docs", "roadmap", "PLAN_REGISTER.json")));
        var legacy = document.RootElement.GetProperty("plans")
            .EnumerateArray()
            .Where(plan => plan.GetProperty("metadata_state").GetString() == "METADATA_MISSING")
            .ToList();

        Assert.NotEmpty(legacy);
        Assert.All(legacy, plan =>
        {
            Assert.Equal("METADATA_MISSING", plan.GetProperty("status").GetString());
            Assert.StartsWith("LEGACY-", plan.GetProperty("plan_id").GetString());
        });
    }

    [Fact]
    public void RegisterRowsExposeRequiredMetadataAndGeneratedHealthFields()
    {
        using var document = JsonDocument.Parse(File.ReadAllText(Path.Combine(RepositoryRoot, "docs", "roadmap", "PLAN_REGISTER.json")));
        foreach (var plan in document.RootElement.GetProperty("plans").EnumerateArray().Take(20))
        {
            foreach (var field in new[]
                     {
                         "plan_id", "status", "category", "wave", "premise_verified_at", "supersedes",
                         "superseded_by", "owner", "rails_required", "metric_moved", "acceptance_tier",
                         "source_authority", "reference_health", "overlap_cluster", "path", "file_sha256"
                     })
            {
                Assert.True(plan.TryGetProperty(field, out _), $"Missing register field {field}");
            }
        }
    }
}
