using System;
using System.Collections.Generic;

namespace Ashfall.Core.Institutions
{
    /// <summary>
    /// Shared survivor-availability authority for the flagship institutions.
    /// ONE live claim per survivor across ALL institutions — a sanatorium
    /// patient cannot simultaneously attend a summit, crew a battery or
    /// transcribe tomes (plan §9.1). Claims are runtime-derived: each
    /// institution persists its own assignments inside its own save section
    /// and re-claims them on restore (host rebinds after restore), so the
    /// ledger itself needs no save section (plan §10).
    /// </summary>
    public sealed class InstitutionAssignmentLedger : IInstitutionAvailability
    {
        private readonly object _gate = new();
        private readonly Dictionary<string, string> _claims = new(StringComparer.Ordinal); // survivor -> institution|role

        /// <summary>survivor id → "institution|role" of the live claim.</summary>
        public IReadOnlyDictionary<string, string> Claims
        {
            get { lock (_gate) return new Dictionary<string, string>(_claims); }
        }

        public bool IsAvailable(string survivorId)
        {
            lock (_gate) return !_claims.ContainsKey(survivorId);
        }

        public bool TryClaim(string survivorId, string institutionId, string roleId)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            lock (_gate)
            {
                var claim = $"{institutionId}|{roleId}";
                if (_claims.TryGetValue(survivorId, out var existing))
                    return existing == claim; // idempotent for the identical triple
                _claims[survivorId] = claim;
                return true;
            }
        }

        public void Release(string survivorId, string institutionId, string roleId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            lock (_gate)
            {
                if (_claims.TryGetValue(survivorId, out var existing)
                    && existing == $"{institutionId}|{roleId}")
                    _claims.Remove(survivorId);
            }
        }
    }
}
