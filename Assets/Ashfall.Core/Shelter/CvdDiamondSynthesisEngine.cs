// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Shelter
{
    /// <summary>CVD reactor operating modes (Plan 124). Batches are industrial jobs.</summary>
    public enum CvdReactorMode
    {
        Offline,
        Idle,
        Growing,
        AwaitingCertification,
        Faulted
    }

    /// <summary>Typed failure codes (plan §12). Host supplies localized prose.</summary>
    public static class CvdFailureCodes
    {
        public const string ReactorUnavailable = "reactor_unavailable";
        public const string InputMissing = "input_missing";
        public const string SubstrateInvalid = "substrate_invalid";
        public const string PlasmaUnstable = "plasma_unstable";
        public const string EquipmentDegraded = "equipment_degraded";
        public const string BatchRejected = "batch_rejected";
        public const string CertificationUnavailable = "certification_unavailable";
    }

    /// <summary>Authoritative CVD reactor state. Owned by this engine only.</summary>
    public sealed class CvdDiamondReactorState
    {
        public string ReactorProfileId { get; set; } = string.Empty;
        public CvdReactorMode Mode { get; set; } = CvdReactorMode.Offline;
        /// <summary>Chamber condition, bp: 10000 = pristine.</summary>
        public int ChamberConditionBp { get; set; } = 10000;
        /// <summary>Magnetron condition, bp: 10000 = pristine.</summary>
        public int MagnetronConditionBp { get; set; } = 10000;
        /// <summary>Current plasma stability, bp. Drifts with magnetron health and noise.</summary>
        public int PlasmaStabilityBp { get; set; } = 7000;
        public DiamondGrowthBatch? ActiveBatch { get; set; }
        public string FaultCode { get; set; } = string.Empty;
    }

    /// <summary>One industrial growth batch. Progress is bp-of-completion.</summary>
    public sealed class DiamondGrowthBatch
    {
        public string BatchId { get; set; } = string.Empty;
        public string ComponentId { get; set; } = string.Empty;
        public string FeedProfileId { get; set; } = string.Empty;
        public string SubstrateProfileId { get; set; } = string.Empty;
        public int GrowthProgressBp { get; set; }
        public int ConformityScoreBp { get; set; }
        public List<string> DefectIds { get; } = new List<string>();
        public float OperatorSkillLevel { get; set; }
        public bool Certified { get; set; }
        public bool Consumed { get; set; }
        public string? AchievedGradeId { get; set; }
        public bool Complete => GrowthProgressBp >= 10000;
    }

    /// <summary>Typed result of consuming a finished batch into a tool component.</summary>
    public sealed class DiamondToolResult
    {
        public string BatchId { get; set; } = string.Empty;
        public string ComponentId { get; set; } = string.Empty;
        public string GradeId { get; set; } = string.Empty;
        public int GradeRank { get; set; }
        /// <summary>Wear factor vs baseline (bp: lower = longer service life). 0 only for rejected.</summary>
        public int WearFactorBp { get; set; }
        public bool RequiresCertification { get; set; }
        public bool Certified { get; set; }
        public List<string> AcceptedConsumerIds { get; } = new List<string>();
    }

    /// <summary>
    /// Synthetic diamond tooling plant (Plan 124 Phase 3-equivalent Core).
    ///
    /// Gameplay-abstract CVD chain: bounded multiplicative growth model over
    /// feed purity, plasma stability, substrate quality, operator skill, and
    /// equipment condition; seeded defect rolls; a strictly ordered grade
    /// ladder; metrology certification for master-grade outputs; and a typed
    /// high-wear consumer registry — only registered consumers that accept a
    /// component receive its wear factor. No global durability buff, no
    /// zero-wear output, no real deposition procedures.
    ///
    /// Deterministic: defect and instability rolls consume ISeededRng only.
    /// The engine owns reactor/magnetron/chamber condition (the machine is
    /// not an inventory item) and never duplicates inventory, power, or
    /// excavation truth; wear benefits are returned as typed values for the
    /// owning systems to apply.
    /// </summary>
    public sealed class CvdDiamondSynthesisEngine
    {
        public const string SystemId = "cvd_diamond";

        // ── Grade thresholds (conformity score bp → grade) ───────────────
        public const int MasterGradeThresholdBp = 8500;
        public const int IndustrialGradeThresholdBp = 6500;
        public const int UtilityGradeThresholdBp = 3500;

        // ── Equipment gates ──────────────────────────────────────────────
        public const int MagnetronStartThresholdBp = 2500;
        public const int ChamberStartThresholdBp = 2500;
        /// <summary>Below this plasma stability a growth tick destabilizes (bp).</summary>
        public const int PlasmaInstabilityThresholdBp = 1500;
        /// <summary>Plasma noise band per tick, bp (seeded).</summary>
        public const int PlasmaNoiseBp = 600;
        /// <summary>Maintenance restores this fraction of missing condition (bp of 10000 scale).</summary>
        public const int MaintenanceRestoreBp = 4000;

        private CvdDiamondReactorState _state = new CvdDiamondReactorState();
        private readonly CvdDiamondCatalog _catalog;
        private readonly ILog _log;
        private readonly HashSet<string> _registeredConsumers = new(StringComparer.Ordinal);
        private readonly Dictionary<string, DiamondGrowthBatch> _finishedBatches = new(StringComparer.Ordinal);
        private int _batchCounter;

        public ISeededRng? Rng { get; set; }

        public CvdDiamondReactorState State => _state;
        public CvdDiamondCatalog Catalog => _catalog;

        public CvdDiamondSynthesisEngine(CvdDiamondCatalog catalog, ILog? log = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _log = log ?? NullLog.Instance;
        }

        public CvdReactorProfile? Profile =>
            _catalog.GetReactor(string.IsNullOrEmpty(_state.ReactorProfileId) ? "" : _state.ReactorProfileId);

        // ── Setup ────────────────────────────────────────────────────────

        public ActionResult Install(string reactorProfileId, bool partsAvailable)
        {
            var profile = _catalog.GetReactor(reactorProfileId ?? string.Empty);
            if (profile == null)
                return ActionResult.Blocked(CvdFailureCodes.ReactorUnavailable, "cvd.reactor_profile_unknown");
            if (!partsAvailable)
                return ActionResult.Blocked("cvd_install_parts_missing", "cvd.install_parts_missing");
            if (_state.Mode != CvdReactorMode.Offline && _state.Mode != CvdReactorMode.Faulted)
                return ActionResult.Blocked("cvd_install_not_offline", "cvd.install_not_offline");

            _state = new CvdDiamondReactorState
            {
                ReactorProfileId = profile.id,
                Mode = CvdReactorMode.Idle,
                PlasmaStabilityBp = profile.plasma_stability_baseline_bp
            };
            return ActionResult.Success("cvd_installed", eventId: "cvd.install");
        }

        /// <summary>Registers a typed high-wear consumer. Must exist in the catalog.</summary>
        public ActionResult RegisterConsumer(string consumerId)
        {
            if (_catalog.GetConsumer(consumerId ?? string.Empty) == null)
                return ActionResult.Blocked("cvd_consumer_unknown", "cvd.consumer_unknown");
            _registeredConsumers.Add(consumerId);
            return ActionResult.Success("cvd_consumer_registered", eventId: "cvd.register_consumer");
        }

        public bool IsConsumerRegistered(string consumerId)
            => !string.IsNullOrEmpty(consumerId) && _registeredConsumers.Contains(consumerId);

        // ── Batch lifecycle ──────────────────────────────────────────────

        public ActionResult StartBatch(
            string batchId, string componentId, string feedProfileId, string substrateProfileId,
            bool powerAvailable, bool coolingAvailable, bool feedstockAvailable, bool substrateItemAvailable,
            float operatorSkillLevel)
        {
            var profile = Profile;
            if (profile == null || _state.Mode == CvdReactorMode.Faulted || _state.Mode == CvdReactorMode.Offline)
                return ActionResult.Blocked(CvdFailureCodes.ReactorUnavailable, "cvd.reactor_unavailable");
            if (_state.ActiveBatch != null)
                return ActionResult.Blocked("cvd_batch_already_active", "cvd.batch_already_active");

            var component = _catalog.GetComponent(componentId ?? string.Empty);
            if (component == null)
                return ActionResult.Blocked(CvdFailureCodes.InputMissing, "cvd.component_unknown");
            var feed = _catalog.GetFeed(feedProfileId ?? string.Empty);
            if (feed == null)
                return ActionResult.Blocked(CvdFailureCodes.InputMissing, "cvd.feed_profile_unknown");
            var substrate = _catalog.GetSubstrate(substrateProfileId ?? string.Empty);
            if (substrate == null)
                return ActionResult.Blocked(CvdFailureCodes.SubstrateInvalid, "cvd.substrate_profile_unknown");
            if (!feedstockAvailable)
                return ActionResult.Blocked(CvdFailureCodes.InputMissing, "cvd.feedstock_missing");
            if (!substrateItemAvailable)
                return ActionResult.Blocked(CvdFailureCodes.InputMissing, "cvd.substrate_item_missing");
            if (!powerAvailable)
                return ActionResult.Blocked(CvdFailureCodes.ReactorUnavailable, "cvd.power_unavailable");
            if (!coolingAvailable)
                return ActionResult.Blocked(CvdFailureCodes.PlasmaUnstable, "cvd.cooling_unavailable");
            if (_state.MagnetronConditionBp < MagnetronStartThresholdBp || _state.ChamberConditionBp < ChamberStartThresholdBp)
                return ActionResult.Blocked(CvdFailureCodes.EquipmentDegraded, "cvd.equipment_degraded");
            if (string.IsNullOrWhiteSpace(batchId))
                return ActionResult.Blocked(CvdFailureCodes.InputMissing, "cvd.batch_id_missing");

            _state.ActiveBatch = new DiamondGrowthBatch
            {
                BatchId = batchId,
                ComponentId = component.id,
                FeedProfileId = feed.id,
                SubstrateProfileId = substrate.id,
                OperatorSkillLevel = Math.Clamp(operatorSkillLevel, 0f, 100f)
            };
            _state.Mode = CvdReactorMode.Growing;
            return ActionResult.Success("cvd_batch_started", eventId: "cvd.start_batch");
        }

        /// <summary>
        /// Advances one industrial tick. Growth progress, plasma drift,
        /// equipment wear, and defect rolls are all deterministic under the
        /// seeded RNG. Completes the batch when progress reaches 10000 bp.
        /// </summary>
        public ActionResult AdvanceBatch()
        {
            var profile = Profile;
            if (profile == null || _state.Mode != CvdReactorMode.Growing || _state.ActiveBatch == null)
                return ActionResult.Blocked("cvd_no_active_batch", "cvd.no_active_batch");

            var batch = _state.ActiveBatch;
            var feed = _catalog.GetFeed(batch.FeedProfileId.Length == 0 ? FirstFeedId() : batch.FeedProfileId)
                       ?? _catalog.GetFeed(FirstFeedId())!;
            var substrate = _catalog.GetSubstrate(batch.SubstrateProfileId)!;

            // ── Plasma stability drift (magnetron health + seeded noise) ─
            int noiseBp = Rng != null ? Rng.Next(-PlasmaNoiseBp, PlasmaNoiseBp + 1) : 0;
            int stabilityTarget = (profile.plasma_stability_baseline_bp * _state.MagnetronConditionBp) / 10000;
            _state.PlasmaStabilityBp = Math.Clamp(stabilityTarget + noiseBp, 0, 10000);

            // Plasma collapse below the instability threshold faults the
            // reactor: batch lost, condition damaged, growth halted.
            if (_state.PlasmaStabilityBp < PlasmaInstabilityThresholdBp
                && Rng != null && Rng.Next(0, 10000) < 4000)
            {
                return FaultReactor(CvdFailureCodes.PlasmaUnstable);
            }

            // ── Bounded multiplicative growth model (plan §6.5) ─────────
            float feedModifier = feed.purity_bp / 10000f;
            float substrateModifier = substrate.quality_bp / 10000f;
            float plasmaModifier = 0.5f + 0.5f * _state.PlasmaStabilityBp / 10000f;
            float operatorModifier = 0.75f + 0.25f * Math.Clamp(batch.OperatorSkillLevel, 0f, 100f) / 100f;
            float equipmentModifier = 0.5f + 0.25f * ((_state.MagnetronConditionBp + _state.ChamberConditionBp) / 2) / 10000f;

            float factor = feedModifier * substrateModifier * plasmaModifier
                * operatorModifier * equipmentModifier;
            int progress = Math.Max(1, (int)MathF.Round(profile.growth_rate_per_tick_bp * factor));
            progress = Math.Min(progress, profile.growth_rate_per_tick_bp);
            batch.GrowthProgressBp = Math.Min(10000, batch.GrowthProgressBp + progress);

            // ── Equipment wear (catalog rates; never negative) ───────────
            _state.MagnetronConditionBp = Math.Clamp(_state.MagnetronConditionBp - profile.magnetron_wear_per_tick_bp, 0, 10000);
            _state.ChamberConditionBp = Math.Clamp(_state.ChamberConditionBp - profile.chamber_wear_per_tick_bp, 0, 10000);

            // ── Seeded defect roll (one attempt per tick) ────────────────
            // ── Seeded defect roll (one attempt per tick) ────────────
            // Risk scales with feed/substrate quality but never floors to
            // zero: even the best inputs carry a bounded defect chance.
            int defectRiskBp = Math.Clamp(1500
                - (feed.purity_bp + substrate.quality_bp) / 16, 100, 3000);
            if (Rng != null && Rng.Next(0, 10000) < defectRiskBp)
            {
                var defect = RollDefect();
                if (defect != null && !batch.DefectIds.Contains(defect.id))
                    batch.DefectIds.Add(defect.id);
            }

            if (!batch.Complete)
                return ActionResult.Success("cvd_batch_growing", eventId: "cvd.advance_batch");

            // ── QA: conformity score → grade ─────────────────────────────
            int score = ComputeConformityScore(batch, feed, substrate);
            var grade = ResolveGrade(score);
            batch.AchievedGradeId = grade?.id;
            batch.ConformityScoreBp = score;

            _state.ActiveBatch = null;
            if (grade == null || string.Equals(grade.id, "grade_rejected", StringComparison.Ordinal))
            {
                _state.Mode = CvdReactorMode.Idle;
                _finishedBatches[batch.BatchId] = batch;
                return ActionResult.Blocked(CvdFailureCodes.BatchRejected, "cvd.batch_rejected");
            }

            _state.Mode = grade.requires_certification
                ? CvdReactorMode.AwaitingCertification
                : CvdReactorMode.Idle;
            _finishedBatches[batch.BatchId] = batch;
            return ActionResult.Success("cvd_batch_complete", eventId: "cvd.batch_complete");
        }

        /// <summary>
        /// Certifies a finished batch through the precision-metrology seam.
        /// Only master-grade outputs require certification (plan §6.7);
        /// lower grades produce without any metrology dependency.
        /// </summary>
        public ActionResult CertifyBatch(string batchId, bool metrologyPassed)
        {
            if (!_finishedBatches.TryGetValue(batchId ?? string.Empty, out var batch))
                return ActionResult.Blocked("cvd_batch_unknown", "cvd.batch_unknown");
            var grade = batch.AchievedGradeId != null ? _catalog.GetGrade(batch.AchievedGradeId) : null;
            if (grade == null || !grade.requires_certification)
                return ActionResult.Blocked("cvd_certification_not_required", "cvd.certification_not_required");
            if (batch.Certified)
                return ActionResult.Blocked("cvd_already_certified", "cvd.already_certified");
            if (!metrologyPassed)
                return ActionResult.Blocked(CvdFailureCodes.CertificationUnavailable, "cvd.certification_unavailable");

            batch.Certified = true;
            if (_state.Mode == CvdReactorMode.AwaitingCertification)
                _state.Mode = CvdReactorMode.Idle;
            return ActionResult.Success("cvd_batch_certified", eventId: "cvd.certify_batch");
        }

        /// <summary>
        /// Consumes a finished batch into its tool component. Idempotent:
        /// a second consume returns the same failure, never a second reward.
        /// Master-grade components require certification before release.
        /// </summary>
        public ActionResult<DiamondToolResult>? ConsumeOutput(string batchId)
        {
            if (!_finishedBatches.TryGetValue(batchId ?? string.Empty, out var batch))
                return ActionResult<DiamondToolResult>.Blocked(CvdFailureCodes.BatchRejected, "cvd.batch_unknown");
            if (batch.Consumed)
                return ActionResult<DiamondToolResult>.Blocked(CvdFailureCodes.BatchRejected, "cvd.batch_already_consumed");

            var grade = batch.AchievedGradeId != null ? _catalog.GetGrade(batch.AchievedGradeId) : null;
            var component = _catalog.GetComponent(batch.ComponentId);
            if (grade == null || component == null
                || string.Equals(grade.id, "grade_rejected", StringComparison.Ordinal)
                || grade.wear_factor_bp <= 0)
                return ActionResult<DiamondToolResult>.Blocked(CvdFailureCodes.BatchRejected, "cvd.batch_rejected");

            // Master-grade output without certification cannot ship at all:
            // the precision path stays closed without the metrology gate.
            if (grade.requires_certification && !batch.Certified)
                return ActionResult<DiamondToolResult>.Blocked(CvdFailureCodes.CertificationUnavailable, "cvd.certification_unavailable");

            batch.Consumed = true;
            var result = new DiamondToolResult
            {
                BatchId = batch.BatchId,
                ComponentId = component.id,
                GradeId = grade.id,
                GradeRank = grade.rank,
                WearFactorBp = grade.wear_factor_bp,
                RequiresCertification = grade.requires_certification,
                Certified = batch.Certified
            };
            foreach (var consumer in _catalog.Consumers)
                foreach (var accepted in consumer.accepted_component_ids)
                    if (string.Equals(accepted, component.id, StringComparison.Ordinal))
                        result.AcceptedConsumerIds.Add(consumer.id);
            return ActionResult<DiamondToolResult>.Success(result);
        }

        /// <summary>
        /// Typed wear benefit: registered consumers that accept the component
        /// receive the grade's wear factor (lower = longer service life).
        /// Unregistered consumers or unaccepted components get NO benefit.
        /// </summary>
        public bool TryGetWearFactor(string consumerId, string componentId, string gradeId, out int wearFactorBp)
        {
            wearFactorBp = 10000;
            if (!IsConsumerRegistered(consumerId))
                return false;
            var consumer = _catalog.GetConsumer(consumerId);
            var grade = _catalog.GetGrade(gradeId);
            if (consumer == null || grade == null || grade.wear_factor_bp <= 0)
                return false;
            foreach (var accepted in consumer.accepted_component_ids)
            {
                var component = _catalog.GetComponent(accepted);
                if (component == null) continue;
                var minGrade = _catalog.GetGrade(component.min_grade_id);
                if (minGrade == null) continue;
                // The component's grade must meet its own minimum grade, and
                // the consumer must accept this component.
                if (grade.rank < minGrade.rank)
                    continue;
                if (string.Equals(accepted, componentId, StringComparison.Ordinal))
                {
                    wearFactorBp = grade.wear_factor_bp;
                    return true;
                }
            }
            return false;
        }

        public ActionResult PerformMaintenance(bool partsAvailable)
        {
            if (_state.Mode == CvdReactorMode.Offline)
                return ActionResult.Blocked("cvd_maintenance_not_needed", "cvd.maintenance_not_needed");
            if (!partsAvailable)
                return ActionResult.Blocked("cvd_maintenance_parts_missing", "cvd.maintenance_parts_missing");

            _state.ChamberConditionBp = Math.Min(10000, _state.ChamberConditionBp + MaintenanceRestoreBp);
            _state.MagnetronConditionBp = Math.Min(10000, _state.MagnetronConditionBp + MaintenanceRestoreBp);
            _state.PlasmaStabilityBp = Profile?.plasma_stability_baseline_bp ?? _state.PlasmaStabilityBp;
            if (_state.Mode == CvdReactorMode.Faulted)
            {
                _state.FaultCode = string.Empty;
                _state.Mode = CvdReactorMode.Idle;
            }
            return ActionResult.Success("cvd_maintained", eventId: "cvd.maintenance");
        }

        public bool HasFinishedBatch(string batchId) => _finishedBatches.ContainsKey(batchId);

        public DiamondGrowthBatch? GetFinishedBatch(string batchId)
            => string.IsNullOrEmpty(batchId) ? null : _finishedBatches.TryGetValue(batchId, out var b) ? b : null;

        /// <summary>
        /// Deterministic next batch id (batch_001, batch_002, …). Presentation
        /// callers mint the label here so it is unique within the live batch
        /// history — no wall-clock entropy in simulation-affecting state.
        /// </summary>
        public string NextBatchId()
        {
            int n = _finishedBatches.Count + 1;
            string id;
            do
            {
                id = $"batch_{n:D3}";
                n++;
            }
            while (_finishedBatches.ContainsKey(id)
                || string.Equals(_state.ActiveBatch?.BatchId, id, StringComparison.Ordinal));
            return id;
        }

        // ── Save round-trip (Plan 124 Phase 8): deep-clone via typed JSON. ──
        public CvdDiamondReactorState CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            return s.Deserialize<CvdDiamondReactorState>(s.Serialize(_state)) ?? new CvdDiamondReactorState();
        }

        public void RestoreState(CvdDiamondReactorState? saved)
        {
            if (saved == null) return; // old saves: no section → fresh reactor
            var s = new SystemTextJsonSerializer();
            _state = s.Deserialize<CvdDiamondReactorState>(s.Serialize(saved)) ?? new CvdDiamondReactorState();
            if (_state.Mode < CvdReactorMode.Offline || _state.Mode > CvdReactorMode.Faulted)
                _state.Mode = CvdReactorMode.Idle;
            _state.ChamberConditionBp = Math.Clamp(_state.ChamberConditionBp, 0, 10000);
            _state.MagnetronConditionBp = Math.Clamp(_state.MagnetronConditionBp, 0, 10000);
            _state.PlasmaStabilityBp = Math.Clamp(_state.PlasmaStabilityBp, 0, 10000);
        }


        // ── internals ────────────────────────────────────────────────────

        private ActionResult FaultReactor(string code)
        {
            _state.Mode = CvdReactorMode.Faulted;
            _state.FaultCode = code;
            _state.ActiveBatch = null; // batch lost with the reactor
            _state.ChamberConditionBp = Math.Clamp(_state.ChamberConditionBp - 2500, 0, 10000);
            return ActionResult.Blocked(code, "cvd.reactor_faulted");
        }

        private DiamondDefectProfile RollDefect()
        {
            var defects = _catalog.defect_profiles;
            if (defects.Count == 0)
                return new DiamondDefectProfile { id = "defect_none", display_name = "None", grade_penalty_bp = 0 };
            int index = Rng != null ? Rng.Next(0, defects.Count) : 0;
            return defects[index];
        }

        private int ComputeConformityScore(DiamondGrowthBatch batch, DiamondFeedProfile feed, DiamondSubstrateProfile substrate)
        {
            // Bounded additive conformity: process inputs + average plasma
            // stability during growth − defect penalties. Deterministic.
            int baseScore = (feed.purity_bp + substrate.quality_bp) / 2;
            int plasmaContribution = _state.PlasmaStabilityBp / 4;
            int operatorContribution = (int)(Math.Clamp(batch.OperatorSkillLevel, 0f, 100f) * 15f);
            int score = baseScore + plasmaContribution + operatorContribution - 2500;
            foreach (var defectId in batch.DefectIds)
            {
                var defect = _catalog.GetDefect(defectId);
                if (defect != null)
                    score -= defect.grade_penalty_bp;
            }
            return Math.Clamp(score, 0, 10000);
        }

        private DiamondGrowthGrade? ResolveGrade(int scoreBp)
        {
            DiamondGrowthGrade? best = null;
            foreach (var grade in _catalog.Grades)
            {
                if (string.Equals(grade.id, "grade_rejected", StringComparison.Ordinal)) continue;
                int threshold = grade.rank switch
                {
                    1 => UtilityGradeThresholdBp,
                    2 => IndustrialGradeThresholdBp,
                    _ => MasterGradeThresholdBp
                };
                if (scoreBp >= threshold && (best == null || grade.rank > best.rank))
                    best = grade;
            }
            return best ?? _catalog.GetGrade("grade_rejected");
        }

        private string FirstFeedId() => _catalog.feed_profiles.Count > 0 ? _catalog.feed_profiles[0].id : string.Empty;
    }

    /// <summary>Generic typed result wrapper for value-carrying outcomes.</summary>
    public sealed class ActionResult<T>
    {
        public bool IsSuccess { get; private init; }
        public bool IsFailure => !IsSuccess;
        public T? Value { get; private init; }
        public string? FailureCode { get; private init; }

        public static ActionResult<T> Success(T value) => new() { IsSuccess = true, Value = value };
        public static ActionResult<T> Blocked(string failureCode, string messageKey)
            => new() { IsSuccess = false, FailureCode = failureCode };
    
}
}
