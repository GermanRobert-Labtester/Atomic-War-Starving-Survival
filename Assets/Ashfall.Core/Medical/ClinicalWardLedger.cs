// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 38 — The Ward
// Subsystem    : Clinical Triage Priority, Surgical Roster & Sterile Supply Ledger
// Authority    : docs/expansions/wave6/expansion_38_the_ward_plan.md
//                WAVE6_INDEX.md, DEC-333
// ============================================================================
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// Persisted record of an admitted triage patient.
    /// </summary>
    [Serializable]
    public sealed class TriageAdmissionRecord
    {
        public string PatientSurvivorId { get; set; } = string.Empty;
        public int AdmissionDay { get; set; } = 1;
        public TriagePriorityTier Priority { get; set; } = TriagePriorityTier.Delayed;
        public bool IsContagious { get; set; } = false;
        public int EstimatedUrgencyMinutes { get; set; } = 60;
        public bool InIsolation { get; set; } = false;
        public int DaysInWard { get; set; } = 0;
        public bool IsDischarged { get; set; } = false;

        public TriageAdmissionRecord Clone() => new TriageAdmissionRecord
        {
            PatientSurvivorId = PatientSurvivorId,
            AdmissionDay = AdmissionDay,
            Priority = Priority,
            IsContagious = IsContagious,
            EstimatedUrgencyMinutes = EstimatedUrgencyMinutes,
            InIsolation = InIsolation,
            DaysInWard = DaysInWard,
            IsDischarged = IsDischarged
        };
    }

    /// <summary>
    /// Persisted record of a surgical procedure evaluation and outcome.
    /// </summary>
    [Serializable]
    public sealed class SurgicalProcedureRecord
    {
        public string ProcedureId { get; set; } = string.Empty;
        public string PatientSurvivorId { get; set; } = string.Empty;
        public int Day { get; set; } = 1;
        public bool WasApproved { get; set; } = false;
        public int ShockRiskPermille { get; set; } = 0;
        public int InfectionRiskPermille { get; set; } = 0;
        public int SterileSuppliesConsumedPermille { get; set; } = 0;
        public string BottleneckReason { get; set; } = string.Empty;

        public SurgicalProcedureRecord Clone() => new SurgicalProcedureRecord
        {
            ProcedureId = ProcedureId,
            PatientSurvivorId = PatientSurvivorId,
            Day = Day,
            WasApproved = WasApproved,
            ShockRiskPermille = ShockRiskPermille,
            InfectionRiskPermille = InfectionRiskPermille,
            SterileSuppliesConsumedPermille = SterileSuppliesConsumedPermille,
            BottleneckReason = BottleneckReason
        };
    }

    /// <summary>
    /// Persisted state for clinical ward triage, bed allocation, sterile supply stocks, and surgical history.
    /// </summary>
    [Serializable]
    public sealed class ClinicalWardTriageState
    {
        public int SchemaVersion { get; set; } = 1;
        public ClinicalWardState Ward { get; set; } = new ClinicalWardState();
        public Dictionary<string, TriageAdmissionRecord> ActiveAdmissions { get; set; } =
            new Dictionary<string, TriageAdmissionRecord>(StringComparer.Ordinal);
        public List<SurgicalProcedureRecord> SurgicalHistory { get; set; } = new List<SurgicalProcedureRecord>();
        public int AutoclaveCycleCount { get; set; } = 0;
        public int TotalPatientsTriaged { get; set; } = 0;
        public int TotalSurgeriesPerformed { get; set; } = 0;
        public int NosocomialInfectionsContracted { get; set; } = 0;

        public ClinicalWardTriageState Clone()
        {
            var clone = new ClinicalWardTriageState
            {
                SchemaVersion = SchemaVersion,
                Ward = Ward?.Clone() ?? new ClinicalWardState(),
                AutoclaveCycleCount = AutoclaveCycleCount,
                TotalPatientsTriaged = TotalPatientsTriaged,
                TotalSurgeriesPerformed = TotalSurgeriesPerformed,
                NosocomialInfectionsContracted = NosocomialInfectionsContracted,
                ActiveAdmissions = new Dictionary<string, TriageAdmissionRecord>(ActiveAdmissions.Count, StringComparer.Ordinal),
                SurgicalHistory = new List<SurgicalProcedureRecord>(SurgicalHistory.Count)
            };

            foreach (var kvp in ActiveAdmissions)
            {
                clone.ActiveAdmissions[kvp.Key] = kvp.Value.Clone();
            }

            foreach (var record in SurgicalHistory)
            {
                clone.SurgicalHistory.Add(record.Clone());
            }

            return clone;
        }
    }

    /// <summary>
    /// Read model census summarizing active clinical capacity, sanitation grade, and triage metrics.
    /// </summary>
    public struct ClinicalWardCensus
    {
        public int TotalBeds;
        public int OccupiedBeds;
        public int AvailableBeds;
        public int IsolationBedsTotal;
        public int IsolationBedsOccupied;
        public int AvailableIsolationBeds;
        public WardCleanlinessGrade Cleanliness;
        public int SterileSupplyStockPermille;
        public int StaffingReadinessPermille;
        public int ActiveAdmissionsCount;
        public int SurgeriesPerformedCount;
        public int ProjectedNosocomialRiskPermille;
    }

    /// <summary>
    /// Stateful Core domain authority governing clinical triage admissions, surgical suite readiness,
    /// sterile consumable utilization, and nosocomial infection risks.
    /// Wraps ClinicalWardTriageEngine without replacing MedicalWardSystem or AdvancedSurgicalWardSystem.
    /// </summary>
    public sealed class ClinicalWardLedger
    {
        public ClinicalWardTriageState State { get; private set; }

        public ClinicalWardLedger(ClinicalWardTriageState? initialState = null)
        {
            State = initialState?.Clone() ?? new ClinicalWardTriageState();
            if (State.Ward == null)
            {
                State.Ward = new ClinicalWardState();
            }
        }

        /// <summary>
        /// Triages a patient arriving at the hospital ward and reserves bed capacity if available.
        /// </summary>
        public TriageEvaluationResult TriageAndAdmitPatient(
            string patientId,
            int traumaSeverityPermille,
            int vitalStabilityPermille,
            bool isContagious,
            int currentDay)
        {
            if (string.IsNullOrWhiteSpace(patientId))
            {
                patientId = "patient_unregistered";
            }

            var eval = ClinicalWardTriageEngine.EvaluatePatientTriage(
                traumaSeverityPermille,
                vitalStabilityPermille,
                isContagious,
                State.Ward);

            if (eval.BedAvailable)
            {
                if (isContagious)
                {
                    State.Ward.IsolationBedsOccupied = Math.Min(State.Ward.IsolationBedsTotal, State.Ward.IsolationBedsOccupied + 1);
                }
                else
                {
                    State.Ward.OccupiedBeds = Math.Min(State.Ward.TotalBeds, State.Ward.OccupiedBeds + 1);
                }

                State.TotalPatientsTriaged++;

                var record = new TriageAdmissionRecord
                {
                    PatientSurvivorId = patientId,
                    AdmissionDay = Math.Max(1, currentDay),
                    Priority = eval.AssignedPriority,
                    IsContagious = isContagious,
                    EstimatedUrgencyMinutes = eval.EstimatedUrgencyMinutes,
                    InIsolation = isContagious,
                    DaysInWard = 0,
                    IsDischarged = false
                };

                State.ActiveAdmissions[patientId] = record;
            }

            return eval;
        }

        /// <summary>
        /// Preflights an operating suite and surgical team for a procedure, consuming sterile stock if approved.
        /// </summary>
        public SurgicalReadinessResult PreflightAndExecuteSurgery(
            string procedureId,
            string patientId,
            int procedureComplexityPermille,
            int patientConditionPermille,
            int currentDay,
            int surgerySeed)
        {
            var readiness = ClinicalWardTriageEngine.EvaluateSurgicalPreparation(
                State.Ward,
                procedureComplexityPermille,
                patientConditionPermille,
                surgerySeed);

            var record = new SurgicalProcedureRecord
            {
                ProcedureId = string.IsNullOrWhiteSpace(procedureId) ? "proc_general_surgery" : procedureId,
                PatientSurvivorId = string.IsNullOrWhiteSpace(patientId) ? "patient_anon" : patientId,
                Day = Math.Max(1, currentDay),
                WasApproved = readiness.IsApprovedForSurgery,
                ShockRiskPermille = readiness.ShockRiskPermille,
                InfectionRiskPermille = readiness.InfectionRiskPermille,
                SterileSuppliesConsumedPermille = readiness.SterileSuppliesConsumedPermille,
                BottleneckReason = readiness.BottleneckReason
            };

            State.SurgicalHistory.Add(record);

            if (readiness.IsApprovedForSurgery)
            {
                State.TotalSurgeriesPerformed++;
            }

            return readiness;
        }

        /// <summary>
        /// Discharges an admitted patient, freeing bed capacity.
        /// </summary>
        public bool DischargePatient(string patientId)
        {
            if (string.IsNullOrWhiteSpace(patientId) || !State.ActiveAdmissions.TryGetValue(patientId, out var record))
            {
                return false;
            }

            if (record.InIsolation)
            {
                State.Ward.IsolationBedsOccupied = Math.Max(0, State.Ward.IsolationBedsOccupied - 1);
            }
            else
            {
                State.Ward.OccupiedBeds = Math.Max(0, State.Ward.OccupiedBeds - 1);
            }

            record.IsDischarged = true;
            State.ActiveAdmissions.Remove(patientId);
            return true;
        }

        /// <summary>
        /// Adds sterile consumable stock to the clinical ward inventory.
        /// </summary>
        public void RestockSterileSupplies(int amountPermille)
        {
            if (amountPermille <= 0) return;
            State.Ward.SterileSupplyStockPermille = Math.Clamp(State.Ward.SterileSupplyStockPermille + amountPermille, 0, 1000);
        }

        /// <summary>
        /// Runs an autoclave sterilization cycle to restore the surgical theater to a sterile field.
        /// </summary>
        public void RunAutoclaveCycle()
        {
            State.Ward.Cleanliness = WardCleanlinessGrade.SterileField;
            State.AutoclaveCycleCount++;
        }

        /// <summary>
        /// Sets ward cleanliness standard (e.g. from shelter sanitation washdown).
        /// </summary>
        public void SetWardCleanliness(WardCleanlinessGrade grade)
        {
            State.Ward.Cleanliness = grade;
        }

        /// <summary>
        /// Updates the available medical staff readiness permille.
        /// </summary>
        public void SetStaffingReadiness(int staffingPermille)
        {
            State.Ward.StaffingReadinessPermille = Math.Clamp(staffingPermille, 0, 1000);
        }

        /// <summary>
        /// Advances daily clinical operations, tracking inpatient tenure, nosocomial risk, and sterile consumable drain.
        /// </summary>
        public void AdvanceDay(int currentDay, int seed)
        {
            // Daily sterile supply drain per patient
            int totalActivePatients = State.ActiveAdmissions.Count;
            if (totalActivePatients > 0)
            {
                int supplyDrain = Math.Min(State.Ward.SterileSupplyStockPermille, totalActivePatients * 5);
                State.Ward.SterileSupplyStockPermille = Math.Max(0, State.Ward.SterileSupplyStockPermille - supplyDrain);
            }

            int occupancyPermille = State.Ward.TotalBeds > 0
                ? Math.Clamp((State.Ward.OccupiedBeds * 1000) / State.Ward.TotalBeds, 0, 1000)
                : 0;

            int nosocomialRisk = ClinicalWardTriageEngine.CalculateNosocomialInfectionRisk(
                State.Ward.Cleanliness,
                occupancyPermille,
                State.Ward.SterileSupplyStockPermille);

            // Inpatient tenure progression & infection check for long-stay patients
            int patientIndex = 0;
            foreach (var kvp in State.ActiveAdmissions)
            {
                var patient = kvp.Value;
                patient.DaysInWard++;

                if (patient.DaysInWard >= 3 && nosocomialRisk > 250)
                {
                    int infectionCheck = (int)((((uint)(seed + currentDay * 37 + patientIndex * 19)) * 2654435761u) % 1000);
                    if (infectionCheck < (nosocomialRisk / 20))
                    {
                        State.NosocomialInfectionsContracted++;
                    }
                }
                patientIndex++;
            }

            // Natural bio-burden accumulation if cleanliness is not maintained with sterile supplies
            if (currentDay % 7 == 0 && State.Ward.Cleanliness > WardCleanlinessGrade.Contaminated)
            {
                if (State.Ward.SterileSupplyStockPermille < 400 || occupancyPermille > 700)
                {
                    State.Ward.Cleanliness = (WardCleanlinessGrade)((int)State.Ward.Cleanliness - 1);
                }
            }
        }

        /// <summary>
        /// Returns read model census for UI and monitoring.
        /// </summary>
        public ClinicalWardCensus GetCensus()
        {
            int totalBeds = Math.Max(0, State.Ward.TotalBeds);
            int occupiedBeds = Math.Clamp(State.Ward.OccupiedBeds, 0, totalBeds);
            int isoTotal = Math.Max(0, State.Ward.IsolationBedsTotal);
            int isoOccupied = Math.Clamp(State.Ward.IsolationBedsOccupied, 0, isoTotal);

            int occupancyPermille = totalBeds > 0 ? (occupiedBeds * 1000) / totalBeds : 0;
            int projectedRisk = ClinicalWardTriageEngine.CalculateNosocomialInfectionRisk(
                State.Ward.Cleanliness,
                occupancyPermille,
                State.Ward.SterileSupplyStockPermille);

            return new ClinicalWardCensus
            {
                TotalBeds = totalBeds,
                OccupiedBeds = occupiedBeds,
                AvailableBeds = Math.Max(0, totalBeds - occupiedBeds),
                IsolationBedsTotal = isoTotal,
                IsolationBedsOccupied = isoOccupied,
                AvailableIsolationBeds = Math.Max(0, isoTotal - isoOccupied),
                Cleanliness = State.Ward.Cleanliness,
                SterileSupplyStockPermille = State.Ward.SterileSupplyStockPermille,
                StaffingReadinessPermille = State.Ward.StaffingReadinessPermille,
                ActiveAdmissionsCount = State.ActiveAdmissions.Count,
                SurgeriesPerformedCount = State.TotalSurgeriesPerformed,
                ProjectedNosocomialRiskPermille = projectedRisk
            };
        }

        public ClinicalWardTriageState CaptureState() => State.Clone();

        public void RestoreState(ClinicalWardTriageState? state)
        {
            if (state == null)
            {
                State = new ClinicalWardTriageState();
                return;
            }

            State = state.Clone();
            if (State.Ward == null)
            {
                State.Ward = new ClinicalWardState();
            }
        }
    }
}
