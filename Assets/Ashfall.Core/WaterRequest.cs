// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core
{
    /// <summary>
    /// A typed cross-system request for shelter water (B5–B8 Phase 1 shared
    /// contract, flagship brief §6.4). Greenhouse irrigation, kitchen, medical
    /// and decontamination consumers express their demand through this shape
    /// instead of decrementing private counters.
    ///
    /// Quality is expressed with the LIVE <see cref="WaterType"/> vocabulary —
    /// the treatment authority's own quality classes are canonical; no second
    /// quality taxonomy is introduced. Full source-network routing remains
    /// Plan 189's scope; this contract is only the consumer-facing seam.
    /// </summary>
    public readonly struct WaterRequest
    {
        /// <summary>Stable consumer identity, e.g. <c>greenhouse</c>, <c>kitchen</c>, <c>decon</c>. Diagnostic only.</summary>
        public string ConsumerId { get; }

        /// <summary>Quality class requested, in the live treatment vocabulary.</summary>
        public WaterType Quality { get; }

        /// <summary>Exact amount requested. All-or-nothing: commit never delivers partial water.</summary>
        public float RequestedAmount { get; }

        /// <summary>Stable purpose code for diagnostics/briefing, e.g. <c>irrigation</c>, <c>cooking</c>. Diagnostic only.</summary>
        public string Purpose { get; }

        public WaterRequest(string consumerId, WaterType quality, float requestedAmount, string purpose)
        {
            ConsumerId = consumerId ?? string.Empty;
            Quality = quality;
            RequestedAmount = requestedAmount;
            Purpose = purpose ?? string.Empty;
        }
    }

    /// <summary>Side-effect-free result of previewing a <see cref="WaterRequest"/> against the treatment authority.</summary>
    public readonly struct WaterRequestPreview
    {
        public bool CanCommit { get; }
        public float AvailableAmount { get; }
        public float RequestedAmount { get; }
        public WaterType Quality { get; }

        /// <summary>Stable reason code when unavailable: <c>invalid_amount</c>, <c>unknown_type</c>, <c>insufficient_water</c>; empty on success.</summary>
        public string ReasonCode { get; }

        public WaterRequestPreview(bool canCommit, float availableAmount, float requestedAmount, WaterType quality, string reasonCode)
        {
            CanCommit = canCommit;
            AvailableAmount = availableAmount;
            RequestedAmount = requestedAmount;
            Quality = quality;
            ReasonCode = reasonCode ?? string.Empty;
        }
    }

    /// <summary>
    /// Preview/commit behavior for the shared water contract. Preview mutates
    /// nothing; commit consumes the exact requested amount exactly once and is
    /// all-or-nothing (a failed commit never partially drains a pool).
    /// Commit routes through <see cref="WaterTreatmentSystem.RemoveWater"/>,
    /// the same mutation path every existing consumer uses, so the spendable
    /// authority remains exactly one.
    /// </summary>
    public static class WaterRequestContracts
    {
        /// <summary>Pure projection: what would a commit do right now? Never mutates state, never rolls RNG.</summary>
        public static WaterRequestPreview PreviewWaterRequest(this WaterTreatmentSystem system, in WaterRequest request)
        {
            if (system == null)
                return new WaterRequestPreview(false, 0f, request.RequestedAmount, request.Quality, "no_authority");
            if (request.RequestedAmount <= 0f)
                return new WaterRequestPreview(false, system.GetWater(request.Quality), request.RequestedAmount, request.Quality, "invalid_amount");
            if (request.Quality != WaterType.Clean && request.Quality != WaterType.Raw
                && request.Quality != WaterType.Brackish && request.Quality != WaterType.Irradiated)
                return new WaterRequestPreview(false, 0f, request.RequestedAmount, request.Quality, "unknown_type");

            float available = system.GetWater(request.Quality);
            bool canCommit = available >= request.RequestedAmount;
            return new WaterRequestPreview(canCommit, available, request.RequestedAmount, request.Quality,
                canCommit ? string.Empty : "insufficient_water");
        }

        /// <summary>
        /// Authoritative commit: consumes the exact requested amount exactly once,
        /// all-or-nothing. Returns <c>water.committed</c> on success or a stable
        /// failure code with zero mutation on failure.
        /// </summary>
        public static ActionResult CommitWaterRequest(this WaterTreatmentSystem system, in WaterRequest request)
        {
            var preview = system.PreviewWaterRequest(request);
            if (!preview.CanCommit)
                return ActionResult.Failed(preview.ReasonCode, "water.request_failed");

            var removal = system.RemoveWater(request.Quality, request.RequestedAmount);
            if (!removal.IsSuccess)
                return removal; // Defensive: pool moved between preview and commit; nothing was drained by us.

            return ActionResult.Success("water.committed",
                new System.Collections.Generic.Dictionary<string, double>
                {
                    { "amount", request.RequestedAmount },
                    { "quality", (int)request.Quality }
                });
        }
    }
}
