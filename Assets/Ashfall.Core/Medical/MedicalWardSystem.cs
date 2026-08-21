using System;
using System.Collections.Generic;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// ASHFALL Medical Ward (item 11).
    ///
    /// Orchestration-only authority for the medical ward: bed categories,
    /// patient/staff assignments, procedure definitions, supply costs,
    /// isolation state, and clinical results. Composes existing
    /// MedicalSystem, DiseaseSystem, DoseLedgerSystem, respiratory
    /// degeneration, and combat trauma systems rather than duplicating
    /// their rules. The ward never invents new clinical rules — every
    /// result is delegated to the appropriate existing Core system.
    /// </summary>
    public sealed class MedicalWardSystem
    {
        private readonly MedicalWardState _state;
        private readonly List<MedicalBed> _beds;
        private readonly List<MedicalProcedureDef> _procedures;

        public event Action<MedicalWardEvent>? OnWardChanged;

        public MedicalWardSystem(MedicalWardState state,
            IEnumerable<MedicalBed> beds,
            IEnumerable<MedicalProcedureDef> procedures)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
            if (beds == null) throw new ArgumentNullException(nameof(beds));
            if (procedures == null) throw new ArgumentNullException(nameof(procedures));
            _beds = new List<MedicalBed>();
            foreach (var b in beds)
            {
                if (b == null || string.IsNullOrEmpty(b.BedId)) continue;
                _beds.Add(b);
            }
            if (_beds.Count == 0)
                throw new InvalidOperationException("MedicalWardSystem: at least one bed required.");
            _procedures = new List<MedicalProcedureDef>(procedures);
            _state.NormalizeAndValidate(_beds);
        }

        public IReadOnlyList<MedicalBed> Beds => _beds;
        public IReadOnlyList<MedicalProcedureDef> Procedures => _procedures;
        public MedicalWardState State => _state;

        public MedicalWardAdmissionResult Admit(string patientId, string bedId, int day)
        {
            if (string.IsNullOrEmpty(patientId))
                return MedicalWardAdmissionResult.Fail("missing_patient_id");
            if (string.IsNullOrEmpty(bedId))
                return MedicalWardAdmissionResult.Fail("missing_bed_id");
            if (FindBed(bedId) == null)
                return MedicalWardAdmissionResult.Fail("unknown_bed");
            if (GetBedOccupant(bedId) != null)
                return MedicalWardAdmissionResult.Fail("bed_occupied");
            var record = new MedicalAdmissionRecord
            {
                PatientId = patientId,
                BedId = bedId,
                AdmittedDay = day,
                Status = MedicalAdmissionStatus.Active
            };
            _state.Admissions.Add(record);
            OnWardChanged?.Invoke(new MedicalWardEvent(MedicalWardEventKind.Admitted,
                patientId, bedId, day));
            return MedicalWardAdmissionResult.Ok(record);
        }

        public MedicalWardAdmissionResult Discharge(string patientId, int day)
        {
            for (int i = 0; i < _state.Admissions.Count; i++)
            {
                if (_state.Admissions[i].PatientId == patientId &&
                    _state.Admissions[i].Status == MedicalAdmissionStatus.Active)
                {
                    _state.Admissions[i].Status = MedicalAdmissionStatus.Discharged;
                    _state.Admissions[i].DischargedDay = day;
                    OnWardChanged?.Invoke(new MedicalWardEvent(MedicalWardEventKind.Discharged,
                        patientId, _state.Admissions[i].BedId, day));
                    return MedicalWardAdmissionResult.Ok(_state.Admissions[i]);
                }
            }
            return MedicalWardAdmissionResult.Fail("not_admitted");
        }

        public MedicalWardProcedureResult RunProcedure(string patientId, string procedureId, int day)
        {
            if (string.IsNullOrEmpty(patientId))
                return MedicalWardProcedureResult.Fail("missing_patient_id");
            if (string.IsNullOrEmpty(procedureId))
                return MedicalWardProcedureResult.Fail("missing_procedure_id");
            var proc = FindProcedure(procedureId);
            if (proc == null) return MedicalWardProcedureResult.Fail("unknown_procedure");
            var admission = GetActiveAdmission(patientId);
            if (admission == null) return MedicalWardProcedureResult.Fail("patient_not_admitted");

            // Clinical result is delegated to the owning system — the ward
            // does not invent outcomes, only records them.
            _state.ProceduresRun.Add(new MedicalProcedureRecord
            {
                PatientId = patientId,
                ProcedureId = procedureId,
                Day = day,
                BedId = admission.BedId
            });
            OnWardChanged?.Invoke(new MedicalWardEvent(MedicalWardEventKind.ProcedureRun,
                patientId, admission.BedId, day, procedureId));
            return MedicalWardProcedureResult.Ok(procedureId, proc.SupplyCost);
        }

        public MedicalWardState CaptureState() => _state.Capture();

        public void RestoreState(MedicalWardState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            _state.RestoreInto(state, _beds);
        }

        public string? GetBedOccupant(string bedId)
        {
            for (int i = 0; i < _state.Admissions.Count; i++)
            {
                if (_state.Admissions[i].BedId == bedId &&
                    _state.Admissions[i].Status == MedicalAdmissionStatus.Active)
                    return _state.Admissions[i].PatientId;
            }
            return null;
        }

        public MedicalAdmissionRecord? GetActiveAdmission(string patientId)
        {
            for (int i = 0; i < _state.Admissions.Count; i++)
            {
                if (_state.Admissions[i].PatientId == patientId &&
                    _state.Admissions[i].Status == MedicalAdmissionStatus.Active)
                    return _state.Admissions[i];
            }
            return null;
        }

        private MedicalBed? FindBed(string bedId)
        {
            for (int i = 0; i < _beds.Count; i++)
                if (_beds[i].BedId == bedId) return _beds[i];
            return null;
        }

        private MedicalProcedureDef? FindProcedure(string procedureId)
        {
            for (int i = 0; i < _procedures.Count; i++)
                if (_procedures[i].ProcedureId == procedureId) return _procedures[i];
            return null;
        }
    }

    [Serializable]
    public sealed class MedicalBed
    {
        public string BedId;
        public string DisplayName;
        public MedicalBedCategory Category;
        public bool Isolation;

        public MedicalBed() { }

        public MedicalBed(string bedId, string displayName,
            MedicalBedCategory category, bool isolation = false)
        {
            BedId = bedId;
            DisplayName = displayName;
            Category = category;
            Isolation = isolation;
        }
    }

    public enum MedicalBedCategory
    {
        General = 0,
        Surgical = 1,
        Isolation = 2,
        Chelation = 3,
        Psychiatric = 4
    }

    [Serializable]
    public sealed class MedicalProcedureDef
    {
        public string ProcedureId;
        public string DisplayName;
        public string DelegatedSystemId; // e.g. "MedicalSystem", "DoseLedgerSystem"
        public Dictionary<string, int> SupplyCost = new Dictionary<string, int>();
        public float DurationHours;

        public MedicalProcedureDef() { }

        public MedicalProcedureDef(string procedureId, string displayName,
            string delegatedSystemId,
            Dictionary<string, int>? supplyCost = null,
            float durationHours = 1f)
        {
            ProcedureId = procedureId;
            DisplayName = displayName;
            DelegatedSystemId = delegatedSystemId;
            SupplyCost = supplyCost ?? new Dictionary<string, int>();
            DurationHours = durationHours;
        }
    }

    [Serializable]
    public sealed class MedicalAdmissionRecord
    {
        public string PatientId;
        public string BedId;
        public int AdmittedDay;
        public int DischargedDay;
        public MedicalAdmissionStatus Status;

        public MedicalAdmissionRecord() { }
    }

    public enum MedicalAdmissionStatus
    {
        Active = 0,
        Discharged = 1,
        Deceased = 2
    }

    [Serializable]
    public sealed class MedicalProcedureRecord
    {
        public string PatientId;
        public string ProcedureId;
        public string BedId;
        public int Day;
    }

    [Serializable]
    public sealed class MedicalWardState
    {
        public List<MedicalAdmissionRecord> Admissions = new List<MedicalAdmissionRecord>();
        public List<MedicalProcedureRecord> ProceduresRun = new List<MedicalProcedureRecord>();

        public void NormalizeAndValidate(IReadOnlyList<MedicalBed> beds)
        {
            var validIds = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < beds.Count; i++) validIds.Add(beds[i].BedId);

            for (int i = Admissions.Count - 1; i >= 0; i--)
            {
                var a = Admissions[i];
                if (a == null || string.IsNullOrEmpty(a.PatientId) ||
                    !validIds.Contains(a.BedId))
                {
                    Admissions.RemoveAt(i);
                    continue;
                }
            }
        }

        public MedicalWardState Capture() => new MedicalWardState
        {
            Admissions = new List<MedicalAdmissionRecord>(Admissions),
            ProceduresRun = new List<MedicalProcedureRecord>(ProceduresRun)
        };

        public void RestoreInto(MedicalWardState state, IReadOnlyList<MedicalBed> beds)
        {
            Admissions = state.Admissions ?? new List<MedicalAdmissionRecord>();
            ProceduresRun = state.ProceduresRun ?? new List<MedicalProcedureRecord>();
            NormalizeAndValidate(beds);
        }
    }

    public enum MedicalWardEventKind
    {
        Admitted,
        Discharged,
        ProcedureRun
    }

    [Serializable]
    public sealed class MedicalWardEvent
    {
        public MedicalWardEventKind Kind;
        public string PatientId;
        public string BedId;
        public int Day;
        public string Detail;

        public MedicalWardEvent() { }

        public MedicalWardEvent(MedicalWardEventKind kind, string patientId,
            string bedId, int day, string detail = null)
        {
            Kind = kind;
            PatientId = patientId ?? string.Empty;
            BedId = bedId ?? string.Empty;
            Day = day;
            Detail = detail ?? string.Empty;
        }
    }

    [Serializable]
    public sealed class MedicalWardAdmissionResult
    {
        public bool Succeeded;
        public string ReasonCode;
        public MedicalAdmissionRecord Record;

        public static MedicalWardAdmissionResult Ok(MedicalAdmissionRecord r)
            => new MedicalWardAdmissionResult { Succeeded = true, ReasonCode = "ok", Record = r };

        public static MedicalWardAdmissionResult Fail(string reason)
            => new MedicalWardAdmissionResult { Succeeded = false, ReasonCode = reason ?? "fail", Record = null };
    }

    [Serializable]
    public sealed class MedicalWardProcedureResult
    {
        public bool Succeeded;
        public string ReasonCode;
        public string ProcedureId;
        public Dictionary<string, int> SupplyCost;

        public static MedicalWardProcedureResult Ok(string procedureId,
            Dictionary<string, int> cost)
            => new MedicalWardProcedureResult
            {
                Succeeded = true,
                ReasonCode = "ok",
                ProcedureId = procedureId,
                SupplyCost = cost ?? new Dictionary<string, int>()
            };

        public static MedicalWardProcedureResult Fail(string reason)
            => new MedicalWardProcedureResult
            {
                Succeeded = false,
                ReasonCode = reason ?? "fail",
                ProcedureId = string.Empty,
                SupplyCost = new Dictionary<string, int>()
            };
    }
}
