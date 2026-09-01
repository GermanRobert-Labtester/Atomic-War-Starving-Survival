using System;
using System.Collections.Generic;

#pragma warning disable CS8618

namespace Ashfall.Core
{
    /// <summary>One active trade embargo against a faction. Expiry is derived
    /// from the day, never ticked — restoring state cannot drift the window.</summary>
    [Serializable]
    public class FactionEmbargoRecord
    {
        public string factionId = string.Empty;
        /// <summary>Scope of the suspension (authored consequence embargoScope,
        /// e.g. "creditor_faction" or "trade_offers").</summary>
        public string scope = string.Empty;
        public int startDay;
        public int endDay;
        /// <summary>Stable source identity (e.g. debt:{contract}:{consequence}).
        /// Re-requesting the same source is a no-op — one logical embargo per
        /// consequence, defended here in depth as well as in the dispatcher.</summary>
        public string sourceId = string.Empty;
    }

    [Serializable]
    public class FactionEmbargoLedgerState
    {
        public List<FactionEmbargoRecord> embargoes = new List<FactionEmbargoRecord>();
    }

    /// <summary>
    /// Canonical embargo authority for the shelter's faction trade. Debt
    /// defaults request suspensions through <see cref="TryAddEmbargo"/>; trade
    /// surfaces and credit eligibility query <see cref="IsEmbargoed"/>. The
    /// ledger owns start/end days, persistence and dedupe — requesters never
    /// maintain a parallel blacklist.
    /// </summary>
    public class FactionEmbargoLedger
    {
        private FactionEmbargoLedgerState _state = new FactionEmbargoLedgerState();

        public event Action<FactionEmbargoRecord> OnEmbargoAdded;
        public event Action OnStateChanged;

        public IReadOnlyList<FactionEmbargoRecord> Embargoes => _state.embargoes;

        /// <summary>
        /// Suspend trade with a faction for a bounded window. The same
        /// sourceId is idempotent (returns false, nothing changes); a different
        /// source may coexist. Purely day-derived: no ticking required.
        /// </summary>
        public bool TryAddEmbargo(string factionId, string scope, int startDay, int durationDays, string sourceId)
        {
            if (string.IsNullOrEmpty(factionId) || durationDays <= 0) return false;
            if (!string.IsNullOrEmpty(sourceId))
            {
                for (int i = 0; i < _state.embargoes.Count; i++)
                {
                    if (_state.embargoes[i].sourceId == sourceId) return false;
                }
            }

            var record = new FactionEmbargoRecord
            {
                factionId = factionId,
                scope = scope ?? string.Empty,
                startDay = startDay,
                endDay = startDay + durationDays,
                sourceId = sourceId ?? string.Empty
            };
            _state.embargoes.Add(record);
            OnEmbargoAdded?.Invoke(record);
            RaiseChanged();
            return true;
        }

        /// <summary>Whether trade with the faction is suspended on the given day.
        /// The end day itself is already open again (window is [startDay, endDay)).</summary>
        public bool IsEmbargoed(string factionId, int day)
        {
            if (string.IsNullOrEmpty(factionId)) return false;
            for (int i = 0; i < _state.embargoes.Count; i++)
            {
                var r = _state.embargoes[i];
                if (r != null && r.factionId == factionId && day >= r.startDay && day < r.endDay)
                    return true;
            }
            return false;
        }

        /// <summary>Active (unexpired) embargo records on the given day.</summary>
        public List<FactionEmbargoRecord> ActiveEmbargoes(int day)
        {
            var active = new List<FactionEmbargoRecord>();
            for (int i = 0; i < _state.embargoes.Count; i++)
            {
                var r = _state.embargoes[i];
                if (r != null && day >= r.startDay && day < r.endDay) active.Add(r);
            }
            return active;
        }

        public FactionEmbargoLedgerState CaptureState()
        {
            var copy = new FactionEmbargoLedgerState();
            for (int i = 0; i < _state.embargoes.Count; i++)
            {
                var r = _state.embargoes[i];
                if (r == null) continue;
                copy.embargoes.Add(new FactionEmbargoRecord
                {
                    factionId = r.factionId ?? string.Empty,
                    scope = r.scope ?? string.Empty,
                    startDay = r.startDay,
                    endDay = r.endDay,
                    sourceId = r.sourceId ?? string.Empty
                });
            }
            return copy;
        }

        public void RestoreState(FactionEmbargoLedgerState saved)
        {
            _state = new FactionEmbargoLedgerState();
            if (saved?.embargoes == null)
            {
                RaiseChanged();
                return;
            }
            for (int i = 0; i < saved.embargoes.Count; i++)
            {
                var r = saved.embargoes[i];
                if (r == null || string.IsNullOrEmpty(r.factionId)) continue;
                _state.embargoes.Add(new FactionEmbargoRecord
                {
                    factionId = r.factionId,
                    scope = r.scope ?? string.Empty,
                    startDay = r.startDay,
                    endDay = r.endDay,
                    sourceId = r.sourceId ?? string.Empty
                });
            }
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke();
    }
}
