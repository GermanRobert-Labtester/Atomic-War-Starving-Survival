using System;
using System.Collections.Generic;
using Ashfall.Core.Medical;

#pragma warning disable CS8618

namespace Ashfall.Core.Disease
{
    /// <summary>
    /// Plan 63 / B4 — Disease Quarantine Coordinator.
    ///
    /// Orchestrates isolation policy across:
    /// - MedicalWardSystem (authoritative bed allocation & admissions)
    /// - DutyRosterSystem (duty separation & external availability reservation)
    /// - DiseaseSystem (pathogen simulation, quarantine shedding reduction, immunity)
    /// - Research / ContainmentCapability (bounded projection)
    /// - Resource consumption (water, food, medical supplies)
    ///
    /// Invariants:
    /// 1. No bed ownership in DiseaseSystem: MedicalWardSystem owns beds.
    /// 2. Duty roster separation: isolated survivors cannot work communal shifts.
    /// 3. Bed capacity honesty: if no isolation bed is available, assignment is blocked.
    /// 4. Real care burden: care supplies are consumed daily; neglected care degrades isolation quality.
    /// 5. Bounded transmission: quarantine reduces transmission by 85%–95%, never magic 0%.
    /// </summary>
    public sealed class DiseaseQuarantineCoordinator
    {
        private readonly MedicalWardSystem _medicalWard;
        private readonly DiseaseSystem _diseaseSystem;
        private readonly DutyRosterSystem? _dutyRoster;
        private readonly Func<string, int, bool>? _tryConsumeItem;
        private readonly Func<ContainmentCapability>? _containmentProvider;

        private readonly Dictionary<string, float> _isolationQualities =
            new Dictionary<string, float>(StringComparer.Ordinal);

        public event Action<string, string, int>? OnQuarantineAssigned;
        public event Action<string, string, int>? OnQuarantineReleased;
        public event Action<int, int, float>? OnDailyBurdenProcessed;

        public DiseaseQuarantineCoordinator(
            MedicalWardSystem medicalWard,
            DiseaseSystem diseaseSystem,
            DutyRosterSystem? dutyRoster = null,
            Func<string, int, bool>? tryConsumeItem = null,
            Func<ContainmentCapability>? containmentProvider = null)
        {
            _medicalWard = medicalWard ?? throw new ArgumentNullException(nameof(medicalWard));
            _diseaseSystem = diseaseSystem ?? throw new ArgumentNullException(nameof(diseaseSystem));
            _dutyRoster = dutyRoster;
            _tryConsumeItem = tryConsumeItem;
            _containmentProvider = containmentProvider;

            // Wire isolation quality provider into DiseaseSystem
            _diseaseSystem.GetIsolationQuality = GetIsolationQuality;

            // Wire external reservation into DutyRosterSystem
            if (_dutyRoster != null)
            {
                var existingReserved = _dutyRoster.IsSurvivorReservedExternally;
                _dutyRoster.IsSurvivorReservedExternally = id =>
                    (existingReserved != null && existingReserved(id)) || IsIsolated(id);
            }
        }

        public MedicalWardSystem Ward => _medicalWard;
        public DiseaseSystem Disease => _diseaseSystem;
        public DutyRosterSystem? Roster => _dutyRoster;

        public ContainmentCapability Containment =>
            _containmentProvider?.Invoke() ?? _diseaseSystem.Containment ?? ContainmentCapability.None;

        /// <summary>
        /// True if the survivor is actively admitted to an isolation bed in the medical ward.
        /// </summary>
        public bool IsIsolated(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            var admission = _medicalWard.GetActiveAdmission(survivorId);
            if (admission == null) return false;

            var bed = FindBed(admission.BedId);
            return bed != null && IsIsolationBed(bed);
        }

        public float GetIsolationQuality(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId) || !IsIsolated(survivorId))
                return 0f;

            if (_isolationQualities.TryGetValue(survivorId, out float quality))
                return quality;

            return 1.0f; // Default full quality when newly admitted
        }

        public MedicalBed? FindAvailableIsolationBed()
        {
            var beds = _medicalWard.Beds;
            for (int i = 0; i < beds.Count; i++)
            {
                var b = beds[i];
                if (b != null && IsIsolationBed(b) && _medicalWard.GetBedOccupant(b.BedId) == null)
                    return b;
            }
            return null;
        }

        private static bool IsIsolationBed(MedicalBed bed)
        {
            return bed.Isolation || bed.Category == MedicalBedCategory.Isolation;
        }

        private MedicalBed? FindBed(string bedId)
        {
            var beds = _medicalWard.Beds;
            for (int i = 0; i < beds.Count; i++)
            {
                if (beds[i] != null && string.Equals(beds[i].BedId, bedId, StringComparison.Ordinal))
                    return beds[i];
            }
            return null;
        }

        // -----------------------------------------------------------------
        // Command API: Preview & Execute Pattern
        // -----------------------------------------------------------------

        public QuarantineCommandPreview PreviewAssignIsolation(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId))
                return QuarantineCommandPreview.Blocked("missing_survivor_id", survivorId ?? "");

            if (IsIsolated(survivorId))
                return QuarantineCommandPreview.Blocked("already_isolated", survivorId);

            var bed = FindAvailableIsolationBed();
            if (bed == null)
                return QuarantineCommandPreview.Blocked("no_isolation_beds_available", survivorId);

            string? conflictingRole = _dutyRoster?.GetRoleOf(survivorId);

            var costs = new Dictionary<string, int>
            {
                { "clean_water", 1 },
                { "canned_food", 1 },
                { "medical_kit", 1 }
            };

            float projectedQuality = 1.0f;

            return QuarantineCommandPreview.Success(survivorId, bed.BedId, conflictingRole, projectedQuality, costs);
        }

        public QuarantineCommandResult ExecuteAssignIsolation(string survivorId, int day)
        {
            var preview = PreviewAssignIsolation(survivorId);
            if (!preview.CanExecute)
                return QuarantineCommandResult.Fail(preview.Reason, survivorId);

            // 1. Admit to MedicalWard
            var admitResult = _medicalWard.Admit(survivorId, preview.TargetBedId, day);
            if (!admitResult.Succeeded)
                return QuarantineCommandResult.Fail(admitResult.ReasonCode, survivorId, preview.TargetBedId, day);

            // 2. Clear conflicting duty role
            if (_dutyRoster != null && !string.IsNullOrEmpty(preview.ConflictingRole))
            {
                _dutyRoster.Assign(preview.ConflictingRole, null!);
            }

            // 3. Set quarantine in DiseaseSystem for all active infections
            SetDiseaseQuarantine(survivorId, true);

            // 4. Record baseline isolation quality
            _isolationQualities[survivorId] = 1.0f;

            OnQuarantineAssigned?.Invoke(survivorId, preview.TargetBedId, day);
            return QuarantineCommandResult.Ok(survivorId, preview.TargetBedId, day);
        }

        public QuarantineCommandPreview PreviewReleaseIsolation(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId))
                return QuarantineCommandPreview.Blocked("missing_survivor_id", survivorId ?? "");

            if (!IsIsolated(survivorId))
                return QuarantineCommandPreview.Blocked("not_isolated", survivorId);

            var admission = _medicalWard.GetActiveAdmission(survivorId);
            return QuarantineCommandPreview.Success(survivorId, admission?.BedId ?? "", null, 0f, new Dictionary<string, int>());
        }

        public QuarantineCommandResult ExecuteReleaseIsolation(string survivorId, int day)
        {
            var preview = PreviewReleaseIsolation(survivorId);
            if (!preview.CanExecute)
                return QuarantineCommandResult.Fail(preview.Reason, survivorId);

            string bedId = preview.TargetBedId;

            // 1. Discharge from MedicalWard
            var dischargeResult = _medicalWard.Discharge(survivorId, day);
            if (!dischargeResult.Succeeded)
                return QuarantineCommandResult.Fail(dischargeResult.ReasonCode, survivorId, bedId, day);

            // 2. Lift quarantine in DiseaseSystem
            SetDiseaseQuarantine(survivorId, false);

            // 3. Clear quality tracker
            _isolationQualities.Remove(survivorId);

            OnQuarantineReleased?.Invoke(survivorId, bedId, day);
            return QuarantineCommandResult.Ok(survivorId, bedId, day);
        }

        private void SetDiseaseQuarantine(string survivorId, bool quarantined)
        {
            var diseases = _diseaseSystem.State.diseases;
            if (diseases == null) return;

            for (int i = 0; i < diseases.Count; i++)
            {
                var d = diseases[i];
                if (d == null || d.infected == null) continue;
                for (int j = 0; j < d.infected.Count; j++)
                {
                    if (string.Equals(d.infected[j].survivor_id, survivorId, StringComparison.Ordinal))
                    {
                        if (quarantined)
                            _diseaseSystem.Quarantine(survivorId, d.disease_id);
                        else
                            _diseaseSystem.EndQuarantine(survivorId, d.disease_id);
                    }
                }
            }
        }

        // -----------------------------------------------------------------
        // Daily Simulation Loop (TickDaily)
        // -----------------------------------------------------------------

        public void TickDaily(int day)
        {
            var admissions = _medicalWard.State.Admissions;
            int isolatedCount = 0;
            float totalQuality = 0f;

            var containment = Containment;
            _diseaseSystem.Containment = containment;

            for (int i = 0; i < admissions.Count; i++)
            {
                var adm = admissions[i];
                if (adm == null || adm.Status != MedicalAdmissionStatus.Active) continue;

                var bed = FindBed(adm.BedId);
                if (bed == null || !IsIsolationBed(bed)) continue;

                isolatedCount++;
                string patientId = adm.PatientId;

                // Daily care resource drain
                float quality = 0.10f; // Baseline chamber isolation

                if (_tryConsumeItem != null)
                {
                    bool waterOk = _tryConsumeItem("clean_water", 1);
                    bool foodOk = _tryConsumeItem("canned_food", 1);
                    bool medOk = _tryConsumeItem("medical_kit", 1) || _tryConsumeItem("bandage", 1);

                    if (waterOk) quality += 0.35f;
                    if (foodOk) quality += 0.35f;
                    if (medOk) quality += 0.20f;
                }
                else
                {
                    // Full care assumed when no consumption sink provided
                    quality = 1.0f;
                }

                quality += containment.EfficacyBonus;
                quality = Math.Clamp(quality, 0.10f, 1.0f);

                _isolationQualities[patientId] = quality;
                totalQuality += quality;

                // Sync quarantine flag in DiseaseSystem
                SetDiseaseQuarantine(patientId, true);
            }

            float avgQuality = isolatedCount > 0 ? (totalQuality / isolatedCount) : 1.0f;
            OnDailyBurdenProcessed?.Invoke(day, isolatedCount, avgQuality);
        }

        /// <summary>
        /// Reconcile state after loading a save. Ensures DiseaseSystem quarantine flags
        /// match active MedicalWard isolation beds.
        /// </summary>
        public void Rehydrate()
        {
            var admissions = _medicalWard.State.Admissions;
            for (int i = 0; i < admissions.Count; i++)
            {
                var adm = admissions[i];
                if (adm != null && adm.Status == MedicalAdmissionStatus.Active)
                {
                    var bed = FindBed(adm.BedId);
                    if (bed != null && IsIsolationBed(bed))
                    {
                        SetDiseaseQuarantine(adm.PatientId, true);
                        _isolationQualities[adm.PatientId] = 1.0f;
                    }
                }
            }
        }
    }

    public sealed class QuarantineCommandPreview
    {
        public bool CanExecute { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string SurvivorId { get; set; } = string.Empty;
        public string TargetBedId { get; set; } = string.Empty;
        public string? ConflictingRole { get; set; }
        public float ProjectedIsolationQuality { get; set; } = 1.0f;
        public Dictionary<string, int> DailySupplyCost { get; set; } = new Dictionary<string, int>();

        public static QuarantineCommandPreview Success(string survivorId, string bedId, string? conflictingRole, float projectedQuality, Dictionary<string, int> costs) =>
            new QuarantineCommandPreview
            {
                CanExecute = true,
                Reason = "ok",
                SurvivorId = survivorId,
                TargetBedId = bedId,
                ConflictingRole = conflictingRole,
                ProjectedIsolationQuality = projectedQuality,
                DailySupplyCost = costs
            };

        public static QuarantineCommandPreview Blocked(string reason, string survivorId) =>
            new QuarantineCommandPreview
            {
                CanExecute = false,
                Reason = reason,
                SurvivorId = survivorId
            };
    }

    public sealed class QuarantineCommandResult
    {
        public bool Success { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string SurvivorId { get; set; } = string.Empty;
        public string BedId { get; set; } = string.Empty;
        public int Day { get; set; }

        public static QuarantineCommandResult Ok(string survivorId, string bedId, int day) =>
            new QuarantineCommandResult { Success = true, Reason = "ok", SurvivorId = survivorId, BedId = bedId, Day = day };

        public static QuarantineCommandResult Fail(string reason, string survivorId, string bedId = "", int day = 0) =>
            new QuarantineCommandResult { Success = false, Reason = reason, SurvivorId = survivorId, BedId = bedId, Day = day };
    }
}
