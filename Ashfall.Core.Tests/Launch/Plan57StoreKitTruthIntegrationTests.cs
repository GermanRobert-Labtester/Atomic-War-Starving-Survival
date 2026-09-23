// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core.Launch;
using Xunit;

namespace Ashfall.Core.Tests.Launch
{
    public sealed class Plan57StoreKitTruthIntegrationTests
    {
        private static StoreCapabilityManifestData LoadAuthoredManifestData()
        {
            string catalogPath = Path.Combine(AppContext.BaseDirectory, "Data", "store_capability_claims.json");
            if (!File.Exists(catalogPath))
            {
                catalogPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "store_capability_claims.json"));
            }
            if (!File.Exists(catalogPath))
            {
                catalogPath = Path.Combine("Assets", "StreamingAssets", "Data", "store_capability_claims.json");
            }

            string json = File.ReadAllText(catalogPath);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var data = new StoreCapabilityManifestData
            {
                SchemaVersion = root.GetProperty("schema_version").GetInt32(),
                ManifestId = root.GetProperty("manifest_id").GetString() ?? string.Empty,
                ProductTitle = root.GetProperty("product_title").GetString() ?? "ASHFALL",
                ProjectVersion = root.GetProperty("project_version").GetString() ?? "1.1.0",
                GeneratedDate = root.GetProperty("generated_date").GetString() ?? string.Empty
            };

            if (root.TryGetProperty("claims", out var claimsElem))
            {
                foreach (var c in claimsElem.EnumerateArray())
                {
                    data.Claims.Add(new StoreCapabilityClaim
                    {
                        ClaimId = c.GetProperty("claim_id").GetString() ?? string.Empty,
                        Category = c.GetProperty("category").GetString() ?? string.Empty,
                        Statement = c.GetProperty("statement").GetString() ?? string.Empty,
                        BackingSystem = c.GetProperty("backing_system").GetString() ?? string.Empty,
                        VerificationGate = c.GetProperty("verification_gate").GetString() ?? string.Empty,
                        IsVerified = c.GetProperty("is_verified").GetBoolean()
                    });
                }
            }

            return data;
        }

        [Fact]
        public void StoreManifest_DeclaresCanonicalVerifiedClaims()
        {
            var data = LoadAuthoredManifestData();

            Assert.Equal(1, data.SchemaVersion);
            Assert.Equal("ashfall_store_capability_manifest_v1", data.ManifestId);
            Assert.Equal("ASHFALL", data.ProductTitle);
            Assert.Equal("1.1.0", data.ProjectVersion);
            Assert.True(data.Claims.Count >= 10);

            foreach (var claim in data.Claims)
            {
                Assert.False(string.IsNullOrWhiteSpace(claim.ClaimId));
                Assert.False(string.IsNullOrWhiteSpace(claim.Category));
                Assert.False(string.IsNullOrWhiteSpace(claim.Statement));
                Assert.False(string.IsNullOrWhiteSpace(claim.BackingSystem));
                Assert.False(string.IsNullOrWhiteSpace(claim.VerificationGate));
                Assert.True(claim.IsVerified);
            }
        }

        [Fact]
        public void StoreCapabilityManifest_ValidateClaim_PassesWhenBackingSystemAndGateAreValid()
        {
            var data = LoadAuthoredManifestData();
            var manifest = new StoreCapabilityManifest(data);

            var sampleClaim = data.Claims[0];

            var result = manifest.ValidateClaim(
                sampleClaim,
                isSystemAvailable: sys => sys == sampleClaim.BackingSystem,
                isGatePassing: gate => gate == sampleClaim.VerificationGate);

            Assert.True(result.Passed);
            Assert.Contains("verified", result.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void StoreCapabilityManifest_ValidateClaim_RejectsMissingBackingSystemOrFailedGate()
        {
            var data = LoadAuthoredManifestData();
            var manifest = new StoreCapabilityManifest(data);

            var sampleClaim = data.Claims[0];

            // 1. System not available
            var resNoSystem = manifest.ValidateClaim(
                sampleClaim,
                isSystemAvailable: sys => false,
                isGatePassing: gate => true);

            Assert.False(resNoSystem.Passed);
            Assert.Contains("not available", resNoSystem.Message, StringComparison.OrdinalIgnoreCase);

            // 2. Gate not passing
            var resNoGate = manifest.ValidateClaim(
                sampleClaim,
                isSystemAvailable: sys => true,
                isGatePassing: gate => false);

            Assert.False(resNoGate.Passed);
            Assert.Contains("not passing", resNoGate.Message, StringComparison.OrdinalIgnoreCase);

            // 3. Null claim
            var resNull = manifest.ValidateClaim(null!, _ => true, _ => true);
            Assert.False(resNull.Passed);
        }

        [Fact]
        public void StoreCapabilityManifest_AuditAllClaims_GeneratesAccurateReport()
        {
            var data = LoadAuthoredManifestData();
            var manifest = new StoreCapabilityManifest(data);

            // All systems and gates pass
            var allPassReport = manifest.AuditAllClaims(
                isSystemAvailable: _ => true,
                isGatePassing: _ => true);

            Assert.Equal("ashfall_store_capability_manifest_v1", allPassReport.ManifestId);
            Assert.Equal(data.Claims.Count, allPassReport.TotalClaims);
            Assert.Equal(data.Claims.Count, allPassReport.VerifiedClaims);
            Assert.Equal(0, allPassReport.RejectedClaims);
            Assert.True(allPassReport.AllClaimsVerified);

            // One system missing
            var oneMissingReport = manifest.AuditAllClaims(
                isSystemAvailable: sys => sys != "SessionDurabilityManager",
                isGatePassing: _ => true);

            Assert.Equal(1, oneMissingReport.RejectedClaims);
            Assert.False(oneMissingReport.AllClaimsVerified);
        }

        [Fact]
        public void StoreCapabilityManifest_SeamDelegates_TriggerOnAuditAndRejection()
        {
            var data = LoadAuthoredManifestData();
            var manifest = new StoreCapabilityManifest(data);

            StoreCapabilityClaim? auditedClaim = null;
            bool? auditedClaimPassed = null;
            StoreStatementReport? auditedReport = null;
            string? unsubstantiatedClaimId = null;

            manifest.OnClaimAuditedSeam = (c, pass) =>
            {
                auditedClaim = c;
                auditedClaimPassed = pass;
            };
            manifest.OnStoreManifestAuditedSeam = r => auditedReport = r;
            manifest.OnUnsubstantiatedClaimDetectedSeam = (id, reason) => unsubstantiatedClaimId = id;

            var fakeClaim = new StoreCapabilityClaim
            {
                ClaimId = "fake_unbacked_claim",
                Category = "GameplayCore",
                Statement = "Fictitious feature",
                BackingSystem = "NonExistentSystem",
                VerificationGate = "FakeGate"
            };

            var res = manifest.ValidateClaim(fakeClaim, _ => false, _ => true);

            Assert.False(res.Passed);
            Assert.Equal("fake_unbacked_claim", unsubstantiatedClaimId);
            Assert.Equal(fakeClaim, auditedClaim);
            Assert.False(auditedClaimPassed);

            var report = manifest.AuditAllClaims(_ => true, _ => true);
            Assert.NotNull(auditedReport);
            Assert.Equal(report, auditedReport);
        }
    }
}
