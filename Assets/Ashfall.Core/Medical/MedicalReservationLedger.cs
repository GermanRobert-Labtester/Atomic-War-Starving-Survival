// SPDX-License-Identifier: MIT
// Task #133 — Medical reservations: claims overlay, inventory stays quantity truth.
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Medical
{
    public enum MedicalReservationKind
    {
        /// <summary>Units of one inventory item claimed by a treatment.</summary>
        Medicine = 0,
        /// <summary>A ward bed claimed by an admission-linked procedure.</summary>
        Bed = 1
    }

    /// <summary>Serialized reservation row.</summary>
    [Serializable]
    public sealed class MedicalReservation
    {
        public int reservationId;
        public string survivorId = string.Empty;
        /// <summary>"medicine" | "bed" — snake_case on the wire.</summary>
        public string kind = "medicine";
        /// <summary>Item id for medicine reservations, bed id for bed reservations.</summary>
        public string targetId = string.Empty;
        public int quantity;
        public string treatmentId = string.Empty;
        public int procedureId = -1;

        public MedicalReservation Clone() => new MedicalReservation
        {
            reservationId = reservationId,
            survivorId = survivorId,
            kind = kind,
            targetId = targetId,
            quantity = quantity,
            treatmentId = treatmentId,
            procedureId = procedureId
        };
    }

    /// <summary>Versioned save snapshot of all reservations.</summary>
    [Serializable]
    public sealed class MedicalReservationSaveState
    {
        public const int CurrentVersion = 1;
        public int version = CurrentVersion;
        public int nextReservationId = 1;
        public List<MedicalReservation> reservations = new List<MedicalReservation>();
    }

    /// <summary>
    /// Claims against shared medical inputs so two procedures cannot promise the
    /// same inhaler. The ledger stores <b>claims only</b>: actual quantities stay
    /// in the authoritative inventory (<c>IPlayerInventoryPort</c>). Availability
    /// is <c>inventory count − sum of active claims</c>.
    ///
    /// <para>Reservation ids are a persisted monotonic counter — never a Guid,
    /// never a hashcode (determinism invariant). Reservations survive save/load;
    /// on restore, rows referencing unknown ids are repairable by the host's
    /// integrity pass.</para>
    /// </summary>
    public sealed class MedicalReservationLedger
    {
        private readonly Dictionary<int, MedicalReservation> _byId =
            new Dictionary<int, MedicalReservation>();
        private int _nextReservationId = 1;

        public event Action? OnStateChanged;

        public IReadOnlyCollection<MedicalReservation> Reservations
        {
            get
            {
                var keys = new List<int>(_byId.Keys);
                keys.Sort();
                var list = new List<MedicalReservation>(keys.Count);
                foreach (var k in keys) list.Add(_byId[k].Clone());
                return list;
            }
        }

        /// <summary>Total active claims against one item/bed id (any survivor).</summary>
        public int ReservedQuantity(string targetId)
        {
            int total = 0;
            foreach (var kv in _byId)
            {
                if (string.Equals(kv.Value.targetId, targetId, StringComparison.Ordinal))
                    total += kv.Value.quantity;
            }
            return total;
        }

        public bool TryGet(int reservationId, out MedicalReservation reservation)
        {
            if (_byId.TryGetValue(reservationId, out var r))
            {
                reservation = r.Clone();
                return true;
            }
            reservation = null!;
            return false;
        }

        /// <summary>
        /// Record a claim. Caller must have verified availability against
        /// inventory − claims; the ledger only refuses structurally invalid rows.
        /// Returns the reservation id, or -1 when the row is invalid.
        /// </summary>
        public int Reserve(Survivors.SurvivorId survivor, MedicalReservationKind kind,
            string targetId, int quantity, string treatmentId, int procedureId = -1)
        {
            if (survivor.IsEmpty || string.IsNullOrEmpty(targetId) || quantity <= 0)
                return -1;
            var row = new MedicalReservation
            {
                reservationId = _nextReservationId++,
                survivorId = survivor.Value,
                kind = kind == MedicalReservationKind.Bed ? "bed" : "medicine",
                targetId = targetId,
                quantity = quantity,
                treatmentId = treatmentId ?? string.Empty,
                procedureId = procedureId
            };
            _byId[row.reservationId] = row;
            RaiseChanged();
            return row.reservationId;
        }

        /// <summary>Release a claim; returns true when a reservation was removed.</summary>
        public bool Release(int reservationId)
        {
            bool removed = _byId.Remove(reservationId);
            if (removed) RaiseChanged();
            return removed;
        }

        /// <summary>Release every claim for one survivor (death, departure, cancel-all).</summary>
        public int ReleaseAllForSurvivor(Survivors.SurvivorId survivor)
        {
            if (survivor.IsEmpty) return 0;
            var doomed = new List<int>();
            foreach (var kv in _byId)
                if (string.Equals(kv.Value.survivorId, survivor.Value, StringComparison.Ordinal))
                    doomed.Add(kv.Key);
            foreach (var id in doomed) _byId.Remove(id);
            if (doomed.Count > 0) RaiseChanged();
            return doomed.Count;
        }

        public MedicalReservationSaveState CaptureState()
        {
            var copy = new MedicalReservationSaveState { nextReservationId = _nextReservationId };
            var keys = new List<int>(_byId.Keys);
            keys.Sort();
            foreach (var k in keys) copy.reservations.Add(_byId[k].Clone());
            return copy;
        }

        public void RestoreState(MedicalReservationSaveState? saved)
        {
            _byId.Clear();
            if (saved != null)
            {
                _nextReservationId = Math.Max(1, saved.nextReservationId);
                for (int i = 0; i < saved.reservations.Count; i++)
                {
                    var r = saved.reservations[i];
                    if (r == null || r.reservationId <= 0) continue;
                    if (string.IsNullOrEmpty(r.survivorId) || string.IsNullOrEmpty(r.targetId)) continue;
                    if (r.quantity <= 0) continue;
                    // Keep the highest persisted id ahead of the counter so a
                    // restored ledger never re-issues an id.
                    _nextReservationId = Math.Max(_nextReservationId, r.reservationId + 1);
                    _byId[r.reservationId] = r.Clone();
                }
            }
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke();
    }
}
