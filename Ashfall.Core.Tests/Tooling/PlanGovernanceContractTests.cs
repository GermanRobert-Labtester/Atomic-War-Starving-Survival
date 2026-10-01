using System;
using System.Collections.Generic;
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
        Assert.Equal(2, root.GetProperty("register_schema_version").GetInt32());
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
    public void E1CMigrationCompletesCurrentInventoryWithoutDuplicateIds()
    {
        var registerPath = Path.Combine(RepositoryRoot, "docs", "roadmap", "PLAN_REGISTER.json");
        var baselinePath = Path.Combine(RepositoryRoot, "docs", "roadmap", "e1", "E1C_EXECUTION_BASELINE.json");
        using var document = JsonDocument.Parse(File.ReadAllText(registerPath));
        using var baseline = JsonDocument.Parse(File.ReadAllText(baselinePath));
        var plans = document.RootElement.GetProperty("plans").EnumerateArray().ToList();

        Assert.Equal(baseline.RootElement.GetProperty("plan_count").GetInt32(), plans.Count);
        Assert.All(plans, plan => Assert.Equal("COMPLETE", plan.GetProperty("metadata_state").GetString()));

        var planIds = plans.Select(plan => plan.GetProperty("plan_id").GetString()).ToList();
        Assert.DoesNotContain(planIds, string.IsNullOrWhiteSpace);
        Assert.Equal(planIds.Count, planIds.Distinct(StringComparer.Ordinal).Count());
        Assert.Empty(document.RootElement.GetProperty("validation").GetProperty("errors").EnumerateArray());
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
                         "source_authority", "reference_health", "overlap_cluster", "overlap_clusters", "path", "file_sha256"
                     })
            {
                Assert.True(plan.TryGetProperty(field, out _), $"Missing register field {field}");
            }
        }
    }

    [Fact]
    public void E1EClustersHaveReviewedEvidenceAndDoNotChangePlanStatuses()
    {
        var root = RepositoryRoot;
        using var register = JsonDocument.Parse(File.ReadAllText(Path.Combine(root, "docs", "roadmap", "PLAN_REGISTER.json")));
        using var clusters = JsonDocument.Parse(File.ReadAllText(Path.Combine(root, "docs", "roadmap", "e1", "E1E_CAPABILITY_CLUSTERS.json")));
        var clusterRows = clusters.RootElement.GetProperty("clusters").EnumerateArray().ToList();
        Assert.Equal(5, clusterRows.Count);

        var plans = register.RootElement.GetProperty("plans").EnumerateArray()
            .ToDictionary(plan => plan.GetProperty("plan_id").GetString()!, plan => plan, StringComparer.Ordinal);
        using var claims = JsonDocument.Parse(File.ReadAllText(Path.Combine(root, "docs", "architecture", "CLAIMS.json")));
        var claimIds = claims.RootElement.GetProperty("claims").EnumerateArray()
            .Select(claim => claim.GetProperty("claim_id").GetString()).ToHashSet(StringComparer.Ordinal);
        int candidateReceipts = 0;
        foreach (var cluster in clusterRows)
        {
            var clusterId = cluster.GetProperty("id").GetString()!;
            Assert.Equal("REVIEWED", cluster.GetProperty("review_status").GetString());
            Assert.False(cluster.TryGetProperty("status", out _), $"Cluster {clusterId} must not assign a plan status");
            foreach (var member in cluster.GetProperty("members").EnumerateArray())
            {
                var planId = member.GetProperty("plan_id").GetString()!;
                Assert.True(plans.TryGetValue(planId, out var plan), $"Unknown E1E member {planId}");
                Assert.Contains(clusterId, plan.GetProperty("overlap_clusters").EnumerateArray().Select(value => value.GetString()));
                if (member.TryGetProperty("receipt_path", out var receiptPathElement))
                {
                    candidateReceipts++;
                    var receiptPath = receiptPathElement.GetString()!;
                    var receiptFullPath = Path.Combine(root, receiptPath);
                    Assert.True(File.Exists(receiptFullPath), $"Missing candidate duplicate-search receipt: {receiptPath}");
                    var receiptText = File.ReadAllText(receiptFullPath);
                    Assert.Contains(planId, receiptText);
                    foreach (var field in new[] { "Queries used", "Files and registries inspected", "Live Core authority found", "Host route and save owner found", "Tests or runtime evidence inspected", "Semantic overlap versus implementation duplicate", "Recommended action", "Reviewer conclusion and date" })
                    {
                        Assert.Contains(field, receiptText);
                    }
                }
            }

            foreach (var claimId in cluster.GetProperty("claim_ids").EnumerateArray().Select(value => value.GetString()))
            {
                Assert.Contains(claimId, claimIds);
            }

            foreach (var authority in cluster.GetProperty("live_authorities").EnumerateArray())
            {
                var path = authority.GetProperty("path").GetString()!;
                Assert.True(File.Exists(Path.Combine(root, path)) || Directory.Exists(Path.Combine(root, path)), $"Missing authority evidence: {path}");
            }
        }

        Assert.Equal(5, candidateReceipts);
        Assert.True(File.Exists(Path.Combine(root, "docs", "roadmap", "CAPABILITY_CLUSTERS.md")));
        Assert.True(File.Exists(Path.Combine(root, "docs", "roadmap", "e1", "E1E_INDEPENDENT_REVIEW.md")));
    }

    [Fact]
    public void CanonicalRailsRegistryHasStableSchemaAndEvidenceOnDisk()
    {
        var jsonPath = Path.Combine(RepositoryRoot, "docs", "roadmap", "rails.json");
        var mdPath = Path.Combine(RepositoryRoot, "docs", "roadmap", "RAILS.md");
        var registryPath = Path.Combine(RepositoryRoot, "docs", "roadmap", "rails.registry.json");

        Assert.True(File.Exists(jsonPath), $"Missing rails.json: {jsonPath}");
        Assert.True(File.Exists(mdPath), $"Missing RAILS.md: {mdPath}");
        Assert.True(File.Exists(registryPath), $"Missing rails.registry.json: {registryPath}");

        using var document = JsonDocument.Parse(File.ReadAllText(jsonPath));
        var root = document.RootElement;
        Assert.Equal(1, root.GetProperty("schema_version").GetInt32());
        Assert.Equal(10, root.GetProperty("total_rails").GetInt32());

        var rails = root.GetProperty("rails").EnumerateArray().ToList();
        Assert.Equal(10, rails.Count);

        var validStates = new HashSet<string> { "NOT_STARTED", "IN_FLIGHT", "CODE_READY", "RUNTIME_VERIFIED", "PRESENTED", "DONE" };
        foreach (var rail in rails)
        {
            var id = rail.GetProperty("id").GetString();
            Assert.False(string.IsNullOrWhiteSpace(id));
            var state = rail.GetProperty("state").GetString();
            Assert.Contains(state, validStates);

            // If state is >= CODE_READY, evidence must exist on disk
            if (state is "CODE_READY" or "RUNTIME_VERIFIED" or "PRESENTED" or "DONE")
            {
                var evidence = rail.GetProperty("evidence").EnumerateArray().Select(e => e.GetString()).ToList();
                Assert.NotEmpty(evidence);
                foreach (var ep in evidence)
                {
                    Assert.NotNull(ep);
                    var fullPath = Path.Combine(RepositoryRoot, ep);
                    Assert.True(File.Exists(fullPath) || Directory.Exists(fullPath), $"Evidence path does not exist for rail '{id}': {ep}");
                }
            }
        }
    }

    [Fact]
    public void PlanGovernanceToolingScriptsExistAndAreConfigured()
    {
        var scripts = new[]
        {
            Path.Combine(RepositoryRoot, "scripts", "ci", "verify-plan-freshness.py"),
            Path.Combine(RepositoryRoot, "scripts", "ci", "plan-intake-check.py"),
            Path.Combine(RepositoryRoot, "scripts", "ci", "plan-intake-check.sh"),
            Path.Combine(RepositoryRoot, "scripts", "ci", "generate-rails-registry.py"),
            Path.Combine(RepositoryRoot, "scripts", "ci", "generate-capability-clusters.py"),
        };

        foreach (var script in scripts)
        {
            Assert.True(File.Exists(script), $"Missing governance script: {script}");
            Assert.True(new FileInfo(script).Length > 0, $"Empty governance script: {script}");
        }
    }
}
