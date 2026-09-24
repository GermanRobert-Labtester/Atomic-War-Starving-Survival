#nullable enable
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Launch
{

    /// <summary>
    /// An individual factual claim presented on public store pages or marketing copy.
    /// </summary>
    public sealed class StoreCapabilityClaim
    {
        public string ClaimId { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Statement { get; set; } = string.Empty;
        public string BackingSystem { get; set; } = string.Empty;
        public string VerificationGate { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
    }

    /// <summary>
    /// Catalog data declaring all public store capability claims.
    /// </summary>
    public sealed class StoreCapabilityManifestData
    {
        public int SchemaVersion { get; set; } = 1;
        public string ManifestId { get; set; } = string.Empty;
        public string ProductTitle { get; set; } = "ASHFALL";
        public string ProjectVersion { get; set; } = "1.1.0";
        public string GeneratedDate { get; set; } = string.Empty;
        public List<StoreCapabilityClaim> Claims { get; set; } = new List<StoreCapabilityClaim>();
    }

    /// <summary>
    /// Outcome of validating an individual store capability claim against repository truth.
    /// </summary>
    public sealed class StoreClaimValidationResult
    {
        public string ClaimId { get; set; } = string.Empty;
        public bool Passed { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// Summary audit report of all store capability claims.
    /// </summary>
    public sealed class StoreStatementReport
    {
        public string ManifestId { get; set; } = string.Empty;
        public int TotalClaims { get; set; }
        public int VerifiedClaims { get; set; }
        public int RejectedClaims { get; set; }
        public bool AllClaimsVerified { get; set; }
        public List<StoreClaimValidationResult> Results { get; set; } = new List<StoreClaimValidationResult>();
    }

    /// <summary>
    /// Pure domain authority for Plan 57: The Shop Window.
    /// Ensures that store page claims and marketing statements cannot run ahead of shipped code,
    /// rejecting unbacked or unverifiable assertions.
    /// Zero engine references (Godot/UnityEngine free).
    /// </summary>
    public sealed class StoreCapabilityManifest
    {
        private readonly StoreCapabilityManifestData _data;

        // Seam delegates
        public Action<StoreCapabilityClaim, bool>? OnClaimAuditedSeam { get; set; }
        public Action<StoreStatementReport>? OnStoreManifestAuditedSeam { get; set; }
        public Action<string, string>? OnUnsubstantiatedClaimDetectedSeam { get; set; }

        public StoreCapabilityManifestData Data => _data;

        public StoreCapabilityManifest(StoreCapabilityManifestData data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
        }

        /// <summary>
        /// Validates an individual claim against known system and gate predicates.
        /// </summary>
        public StoreClaimValidationResult ValidateClaim(
            StoreCapabilityClaim claim,
            Func<string, bool> isSystemAvailable,
            Func<string, bool> isGatePassing)
        {
            if (claim == null)
            {
                return new StoreClaimValidationResult
                {
                    ClaimId = "null",
                    Passed = false,
                    Message = "Claim reference was null"
                };
            }

            if (string.IsNullOrWhiteSpace(claim.BackingSystem))
            {
                var failure = new StoreClaimValidationResult
                {
                    ClaimId = claim.ClaimId,
                    Passed = false,
                    Message = $"Claim '{claim.ClaimId}' has no declared backing system"
                };
                OnUnsubstantiatedClaimDetectedSeam?.Invoke(claim.ClaimId, failure.Message);
                OnClaimAuditedSeam?.Invoke(claim, false);
                return failure;
            }

            if (!isSystemAvailable(claim.BackingSystem))
            {
                var failure = new StoreClaimValidationResult
                {
                    ClaimId = claim.ClaimId,
                    Passed = false,
                    Message = $"Declared backing system '{claim.BackingSystem}' is not available in production runtime"
                };
                OnUnsubstantiatedClaimDetectedSeam?.Invoke(claim.ClaimId, failure.Message);
                OnClaimAuditedSeam?.Invoke(claim, false);
                return failure;
            }

            if (!string.IsNullOrWhiteSpace(claim.VerificationGate) && !isGatePassing(claim.VerificationGate))
            {
                var failure = new StoreClaimValidationResult
                {
                    ClaimId = claim.ClaimId,
                    Passed = false,
                    Message = $"Required verification gate '{claim.VerificationGate}' is not passing"
                };
                OnUnsubstantiatedClaimDetectedSeam?.Invoke(claim.ClaimId, failure.Message);
                OnClaimAuditedSeam?.Invoke(claim, false);
                return failure;
            }

            var success = new StoreClaimValidationResult
            {
                ClaimId = claim.ClaimId,
                Passed = true,
                Message = $"Claim '{claim.ClaimId}' verified by {claim.BackingSystem} and gate {claim.VerificationGate}"
            };

            OnClaimAuditedSeam?.Invoke(claim, true);
            return success;
        }

        /// <summary>
        /// Audits all declared claims against system availability and verification gate predicates.
        /// </summary>
        public StoreStatementReport AuditAllClaims(
            Func<string, bool> isSystemAvailable,
            Func<string, bool> isGatePassing)
        {
            var results = new List<StoreClaimValidationResult>();
            int verified = 0;
            int rejected = 0;

            foreach (var claim in _data.Claims)
            {
                var res = ValidateClaim(claim, isSystemAvailable, isGatePassing);
                results.Add(res);
                if (res.Passed) verified++;
                else rejected++;
            }

            var report = new StoreStatementReport
            {
                ManifestId = _data.ManifestId,
                TotalClaims = _data.Claims.Count,
                VerifiedClaims = verified,
                RejectedClaims = rejected,
                AllClaimsVerified = rejected == 0 && verified > 0,
                Results = results
            };

            OnStoreManifestAuditedSeam?.Invoke(report);
            return report;
        }
    }
}
