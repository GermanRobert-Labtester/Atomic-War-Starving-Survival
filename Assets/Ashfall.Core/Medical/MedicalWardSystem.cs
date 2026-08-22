using System;
using System.Collections.Generic;

namespace Ashfall.Core.Medical
{
    public enum MedicalBedCategory
    {
        General = 0,
        Psychiatric = 1,
        Isolation = 2,
        Surgical = 3
    }

    [Serializable]
    public sealed class MedicalBed
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public MedicalBedCategory category;
        public string occupantId = string.Empty;

        public MedicalBed() { }

        public MedicalBed(string id, string displayName, MedicalBedCategory category)
        {
            this.id = id ?? string.Empty;
            this.displayName = displayName ?? string.Empty;
            this.category = category;
        }

        public bool IsOccupied => !string.IsNullOrEmpty(occupantId);
    }

    [Serializable]
    public sealed class MedicalProcedureDef
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public string systemId = string.Empty;

        public MedicalProcedureDef() { }

        public MedicalProcedureDef(string id, string displayName, string systemId)
        {
            this.id = id ?? string.Empty;
            this.displayName = displayName ?? string.Empty;
            this.systemId = systemId ?? string.Empty;
        }
    }

    [Serializable]
    public sealed class MedicalWardState
    {
        public List<MedicalBed> beds = new List<MedicalBed>();
        public List<string> admittedSurvivorIds = new List<string>();
    }

    /// <summary>
    /// Engine-agnostic ward capacity/occupancy authority shared by medical and
    /// psychological systems. Procedure execution stays in the owning medical
    /// subsystem; this type only owns beds and admission state.
    /// </summary>
    public sealed class MedicalWardSystem
    {
        private MedicalWardState _state;
        private readonly Dictionary<string, MedicalProcedureDef> _procedures =
            new Dictionary<string, MedicalProcedureDef>(StringComparer.Ordinal);

        public MedicalWardState State => _state;
        public event Action OnWardChanged;

        public MedicalWardSystem(
            MedicalWardState state,
            IEnumerable<MedicalBed> beds,
            IEnumerable<MedicalProcedureDef> procedures)
        {
            _state = CloneState(state ?? new MedicalWardState());

            if (beds != null)
            {
                foreach (var bed in beds)
                {
                    if (bed == null || string.IsNullOrEmpty(bed.id)) continue;
                    if (_state.beds.Exists(existing => existing.id == bed.id)) continue;
                    _state.beds.Add(CloneBed(bed));
                }
            }

            if (procedures != null)
            {
                foreach (var procedure in procedures)
                {
                    if (procedure == null || string.IsNullOrEmpty(procedure.id)) continue;
                    _procedures[procedure.id] = CloneProcedure(procedure);
                }
            }
        }

        public bool HasAvailableBed(MedicalBedCategory category)
        {
            return _state.beds.Exists(b => b.category == category && !b.IsOccupied);
        }

        public bool Admit(string survivorId, MedicalBedCategory preferredCategory = MedicalBedCategory.General)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            if (_state.admittedSurvivorIds.Contains(survivorId)) return true;

            var bed = _state.beds.Find(b => b.category == preferredCategory && !b.IsOccupied)
                      ?? _state.beds.Find(b => !b.IsOccupied);
            if (bed == null) return false;

            bed.occupantId = survivorId;
            _state.admittedSurvivorIds.Add(survivorId);
            OnWardChanged?.Invoke();
            return true;
        }

        public bool Discharge(string survivorId)
        {
            bool changed = _state.admittedSurvivorIds.Remove(survivorId);
            var bed = _state.beds.Find(b => b.occupantId == survivorId);
            if (bed != null)
            {
                bed.occupantId = string.Empty;
                changed = true;
            }
            if (changed) OnWardChanged?.Invoke();
            return changed;
        }

        public MedicalProcedureDef GetProcedure(string procedureId)
        {
            _procedures.TryGetValue(procedureId ?? string.Empty, out var procedure);
            return procedure;
        }

        public MedicalWardState CaptureState() => CloneState(_state);

        public void RestoreState(MedicalWardState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
            OnWardChanged?.Invoke();
        }

        private static MedicalWardState CloneState(MedicalWardState source)
        {
            var clone = new MedicalWardState();
            if (source?.beds != null)
                foreach (var bed in source.beds)
                    if (bed != null) clone.beds.Add(CloneBed(bed));
            if (source?.admittedSurvivorIds != null)
                clone.admittedSurvivorIds.AddRange(source.admittedSurvivorIds);
            return clone;
        }

        private static MedicalBed CloneBed(MedicalBed bed) => new MedicalBed
        {
            id = bed.id ?? string.Empty,
            displayName = bed.displayName ?? string.Empty,
            category = bed.category,
            occupantId = bed.occupantId ?? string.Empty
        };

        private static MedicalProcedureDef CloneProcedure(MedicalProcedureDef procedure) => new MedicalProcedureDef
        {
            id = procedure.id ?? string.Empty,
            displayName = procedure.displayName ?? string.Empty,
            systemId = procedure.systemId ?? string.Empty
        };
    }
}
