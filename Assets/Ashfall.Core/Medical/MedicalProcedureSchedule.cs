// SPDX-License-Identifier: MIT
// Task #133 — Scheduled medical procedures: a duration ledger, not a second clock.
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Medical
{
    public enum MedicalProcedureStatus
    {
        Active = 0,
        Completed = 1,
        Cancelled = 2,
        Failed = 3
    }

    /// <summary>Serialized scheduled-procedure row.</summary>
    [Serializable]
    public sealed class MedicalProcedureRow
    {
        public int procedureId;
        public string survivorId = string.Empty;
        public string treatmentId = string.Empty;
        public string afflictionEpisodeId = string.Empty;
        public int startDay;
        /// <summary>Remaining game-hours until completion. Decremented only by the campaign day owner.</summary>
        public float remainingHours;
        public float totalHours;
        /// <summary>Reservation ids held by this procedure.</summary>
        public List<int> reservationIds = new List<int>();
        /// <summary>"active" | "completed" | "cancelled" | "failed" — snake_case on the wire.</summary>
        public string status = "active";
        public int endDay = -1;

        public MedicalProcedureRow Clone()
        {
            var clone = new MedicalProcedureRow
            {
                procedureId = procedureId,
                survivorId = survivorId,
                treatmentId = treatmentId,
                afflictionEpisodeId = afflictionEpisodeId,
                startDay = startDay,
                remainingHours = remainingHours,
                totalHours = totalHours,
                status = status,
                endDay = endDay
            };
            clone.reservationIds.AddRange(reservationIds);
            return clone;
        }
    }

    /// <summary>Outcome of one advance pass.</summary>
    public sealed class MedicalProcedureCompletion
    {
        public int ProcedureId;
        public string SurvivorId = string.Empty;
        public string TreatmentId = string.Empty;
        public string AfflictionEpisodeId = string.Empty;
        public int CompletionDay;
    }

    /// <summary>Versioned save snapshot of the procedure schedule.</summary>
    [Serializable]
    public sealed class MedicalProcedureScheduleSaveState
    {
        public const int CurrentVersion = 1;
        public int version = CurrentVersion;
        public int nextProcedureId = 1;
        public List<MedicalProcedureRow> procedures = new List<MedicalProcedureRow>();
    }

    /// <summary>
    /// The medical procedure duration ledger. This is <b>not</b> a second
    /// campaign clock: rows carry remaining hours, and those hours decrease only
    /// when the campaign day owner (or an explicit production hour path) calls
    /// <see cref="Advance"/>. No timers, no UI-driven ticks, no hard-coded 24f
    /// inside this class.
    ///
    /// <para>If a canonical simulation scheduler lands later, this ledger
    /// becomes an adapter over it. Until then it is the single authoritative
    /// list of in-flight medical procedures.</para>
    /// </summary>
    public sealed class MedicalProcedureSchedule
    {
        private readonly List<MedicalProcedureRow> _active = new List<MedicalProcedureRow>();
        private readonly List<MedicalProcedureRow> _history = new List<MedicalProcedureRow>();
        private int _nextProcedureId = 1;

        public event Action? OnStateChanged;

        /// <summary>Active procedures in deterministic order (by procedure id).</summary>
        public IReadOnlyList<MedicalProcedureRow> Active
        {
            get
            {
                var list = new List<MedicalProcedureRow>(_active);
                list.Sort(static (a, b) => a.procedureId.CompareTo(b.procedureId));
                return list;
            }
        }

        /// <summary>Finished procedures (completed/cancelled/failed), newest last.</summary>
        public IReadOnlyList<MedicalProcedureRow> History => _history.ToArray();

        public bool TryGetActive(int procedureId, out MedicalProcedureRow row)
        {
            for (int i = 0; i < _active.Count; i++)
            {
                if (_active[i].procedureId == procedureId)
                {
                    row = _active[i].Clone();
                    return true;
                }
            }
            row = null!;
            return false;
        }

        /// <summary>True when the survivor already has an active procedure for this treatment.</summary>
        public bool HasActiveProcedure(Survivors.SurvivorId survivor, string treatmentId)
        {
            for (int i = 0; i < _active.Count; i++)
            {
                var r = _active[i];
                if (string.Equals(r.survivorId, survivor.Value, StringComparison.Ordinal) &&
                    string.Equals(r.treatmentId, treatmentId, StringComparison.Ordinal))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Schedule a procedure. Caller must have validated and reserved resources;
        /// the schedule only refuses structurally invalid rows. Returns the
        /// procedure id or -1.
        /// </summary>
        public int Schedule(Survivors.SurvivorId survivor, string treatmentId,
            AfflictionEpisodeId episode, int startDay, float durationHours, IReadOnlyList<int> reservationIds)
        {
            if (survivor.IsEmpty || string.IsNullOrEmpty(treatmentId) || durationHours <= 0f)
                return -1;
            var row = new MedicalProcedureRow
            {
                procedureId = _nextProcedureId++,
                survivorId = survivor.Value,
                treatmentId = treatmentId,
                afflictionEpisodeId = episode.Value,
                startDay = startDay,
                remainingHours = durationHours,
                totalHours = durationHours,
                status = "active"
            };
            if (reservationIds != null) row.reservationIds.AddRange(reservationIds);
            _active.Add(row);
            RaiseChanged();
            return row.procedureId;
        }

        /// <summary>
        /// Advance every active procedure by <paramref name="hours"/> (game hours,
        /// driven by the campaign clock). Returns the procedures that completed
        /// in deterministic id order. Completed rows move to history.
        /// </summary>
        public IReadOnlyList<MedicalProcedureCompletion> Advance(float hours, int currentDay)
        {
            var completions = new List<MedicalProcedureCompletion>();
            if (hours <= 0f) return completions;

            // Deterministic: advance in id order, not list order.
            var order = new List<MedicalProcedureRow>(_active);
            order.Sort(static (a, b) => a.procedureId.CompareTo(b.procedureId));
            foreach (var row in order)
            {
                row.remainingHours -= hours;
                if (row.remainingHours > 0f) continue;
                row.remainingHours = 0f;
                row.status = "completed";
                row.endDay = currentDay;
                _active.Remove(row);
                _history.Add(row);
                completions.Add(new MedicalProcedureCompletion
                {
                    ProcedureId = row.procedureId,
                    SurvivorId = row.survivorId,
                    TreatmentId = row.treatmentId,
                    AfflictionEpisodeId = row.afflictionEpisodeId,
                    CompletionDay = currentDay
                });
            }
            if (completions.Count > 0) RaiseChanged();
            return completions;
        }

        /// <summary>
        /// Cancel one procedure deterministically. Returns the row (moved to
        /// history) or null when no such active procedure exists.
        /// </summary>
        public MedicalProcedureRow? Cancel(int procedureId, int currentDay, bool failed = false)
        {
            for (int i = 0; i < _active.Count; i++)
            {
                if (_active[i].procedureId != procedureId) continue;
                var row = _active[i];
                row.status = failed ? "failed" : "cancelled";
                row.endDay = currentDay;
                _active.RemoveAt(i);
                _history.Add(row);
                RaiseChanged();
                return row.Clone();
            }
            return null;
        }

        /// <summary>Cancel everything for a survivor (death reconciliation). Returns cancelled ids.</summary>
        public List<int> CancelAllForSurvivor(Survivors.SurvivorId survivor, int currentDay)
        {
            var cancelled = new List<int>();
            for (int i = _active.Count - 1; i >= 0; i--)
            {
                var row = _active[i];
                if (!string.Equals(row.survivorId, survivor.Value, StringComparison.Ordinal)) continue;
                row.status = "cancelled";
                row.endDay = currentDay;
                _history.Add(row);
                cancelled.Add(row.procedureId);
                _active.RemoveAt(i);
            }
            if (cancelled.Count > 0) RaiseChanged();
            return cancelled;
        }

        public MedicalProcedureScheduleSaveState CaptureState()
        {
            var copy = new MedicalProcedureScheduleSaveState { nextProcedureId = _nextProcedureId };
            foreach (var row in Active) copy.procedures.Add(row.Clone());
            foreach (var row in _history) copy.procedures.Add(row.Clone());
            return copy;
        }

        public void RestoreState(MedicalProcedureScheduleSaveState? saved)
        {
            _active.Clear();
            _history.Clear();
            if (saved != null)
            {
                _nextProcedureId = Math.Max(1, saved.nextProcedureId);
                for (int i = 0; i < saved.procedures.Count; i++)
                {
                    var r = saved.procedures[i];
                    if (r == null || r.procedureId <= 0) continue;
                    if (string.IsNullOrEmpty(r.survivorId) || string.IsNullOrEmpty(r.treatmentId)) continue;
                    _nextProcedureId = Math.Max(_nextProcedureId, r.procedureId + 1);
                    var clone = r.Clone();
                    if (string.Equals(clone.status, "active", StringComparison.Ordinal))
                        _active.Add(clone);
                    else
                        _history.Add(clone);
                }
            }
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke();
    }
}
