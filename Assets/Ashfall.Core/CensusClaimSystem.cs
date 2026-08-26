using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL: THE HOLDFAST — Office relationship as named claims, not hegemony.
    /// Spec: docs/expansions/expansion_the_holdfast_plan.md §5.3.
    /// Engine-agnostic extract of Assets/_Game/Core/CensusClaimSystem.cs
    /// (Mathf.Clamp → Math.Clamp).
    /// </summary>
    [Serializable]
    public class CensusLedgerEntry
    {
        public string survivorId;
        public string displayName;
        public string occupationGuess;
        public string occupationObserved;
        public bool listed;
        public float scoreIfKnown = -1f;
        public bool assignedAway;
    }

    [Serializable]
    public class LevyOrder
    {
        public string[] survivorIds = Array.Empty<string>();
        public int durationDays = 30;
        public string destinationNodeId = "loc_cluster_office";
        public int issuedDay;
        public int remainingDays;
        public bool active;
    }

    [Serializable]
    public class CensusClaimSystemState
    {
        public string systemId = CensusClaimSystem.SystemId;
        public List<CensusLedgerEntry> ledger = new List<CensusLedgerEntry>();
        public LevyOrder levy = new LevyOrder();
        public bool levyHonour;
        public bool levySubstitute;
        public bool levyRefuse;
        public bool order12cActive;
        public bool edorWaitingAtHatch;
        public bool clerkInterviewDone;
        public float officeTrust;
        public int levyRefuseDay = -1;
    }

    public class CensusClaimSystem
    {
        public const string SystemId = "census_claim_system";
        public const string FlagLevyHonour = "holdfast_levy_honour";
        public const string FlagLevySubstitute = "holdfast_levy_substitute";
        public const string FlagLevyRefuse = "holdfast_levy_refuse";
        public const string FlagOrder12c = "holdfast_order_12c";
        public const string FlagEdorWait = "holdfast_edor_wait_hatch";
        public const int MaxLevyCount = 3;
        public const int DefaultLevyDays = 30;
        public const int QuietIntervalDays = 40;

        private CensusClaimSystemState _state = new CensusClaimSystemState();

        public event Action OnCensusUpdated;
        public event Action<LevyOrder> OnLevyIssued;
        public event Action<string> OnLevyResolved;
        public event Action On12CActivated;
        public event Action<CensusClaimSystemState> OnStateChanged;

        public CensusClaimSystemState State => _state;
        public bool LevyHonour => _state.levyHonour;
        public bool LevySubstitute => _state.levySubstitute;
        public bool LevyRefuse => _state.levyRefuse;
        public bool Order12CActive => _state.order12cActive;
        public LevyOrder ActiveLevy => _state.levy;

        public void UpsertLedger(string survivorId, string displayName, string occupationGuess, bool listed)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            var row = Find(survivorId);
            if (row == null)
            {
                row = new CensusLedgerEntry { survivorId = survivorId };
                _state.ledger.Add(row);
            }
            row.displayName = displayName ?? "";
            row.occupationGuess = occupationGuess;
            row.listed = listed;
            OnCensusUpdated?.Invoke();
            RaiseChanged();
        }

        public void CorrectOccupation(string survivorId, string observed)
        {
            var row = Find(survivorId);
            if (row == null) return;
            row.occupationObserved = observed ?? "";
            OnCensusUpdated?.Invoke();
            RaiseChanged();
        }

        public void SetEdorWaitingAtHatch(bool waiting)
        {
            _state.edorWaitingAtHatch = waiting;
            _state.clerkInterviewDone = true;
            RaiseChanged();
        }

        /// <summary>Issue a levy. Always at most three named ids. Never the whole roster.</summary>
        public bool IssueLevy(IList<string> survivorIds, int day, int durationDays = DefaultLevyDays, string destination = "loc_cluster_office")
        {
            if (survivorIds == null || survivorIds.Count == 0) return false;
            if (_state.levy != null && _state.levy.active) return false;

            var ids = new List<string>(MaxLevyCount);
            for (int i = 0; i < survivorIds.Count && ids.Count < MaxLevyCount; i++)
            {
                string id = survivorIds[i];
                if (string.IsNullOrEmpty(id)) continue;
                if (ids.Contains(id)) continue;
                ids.Add(id);
            }
            if (ids.Count == 0) return false;

            _state.levy = new LevyOrder
            {
                survivorIds = ids.ToArray(),
                durationDays = durationDays > 0 ? durationDays : DefaultLevyDays,
                destinationNodeId = string.IsNullOrEmpty(destination) ? "loc_cluster_office" : destination,
                issuedDay = day,
                remainingDays = durationDays > 0 ? durationDays : DefaultLevyDays,
                active = false
            };
            OnLevyIssued?.Invoke(_state.levy);
            RaiseChanged();
            return true;
        }

        public bool HonourLevy()
        {
            if (_state.levy == null || _state.levy.survivorIds == null || _state.levy.survivorIds.Length == 0)
                return false;
            ClearLevyFlags();
            _state.levyHonour = true;
            _state.levy.active = true;
            _state.levy.remainingDays = _state.levy.durationDays;
            MarkAssigned(_state.levy.survivorIds, true);
            OnLevyResolved?.Invoke(FlagLevyHonour);
            RaiseChanged();
            return true;
        }

        /// <summary>Send three *other* people. Named cap still three. Uses assignedAway only — never an expedition flag.</summary>
        public bool SubstituteLevy(IList<string> substituteIds)
        {
            if (_state.levy == null) return false;
            if (substituteIds == null || substituteIds.Count == 0) return false;

            var ids = new List<string>(MaxLevyCount);
            for (int i = 0; i < substituteIds.Count && ids.Count < MaxLevyCount; i++)
            {
                string id = substituteIds[i];
                if (string.IsNullOrEmpty(id)) continue;
                if (ids.Contains(id)) continue;
                ids.Add(id);
            }
            if (ids.Count == 0) return false;

            if (_state.levy != null && _state.levy.survivorIds != null)
                MarkAssigned(_state.levy.survivorIds, false);

            ClearLevyFlags();
            if (_state.levy == null) return false;
            _state.levySubstitute = true;
            _state.levy.survivorIds = ids.ToArray();
            _state.levy.active = true;
            _state.levy.remainingDays = _state.levy.durationDays;
            MarkAssigned(_state.levy.survivorIds, true);
            OnLevyResolved?.Invoke(FlagLevySubstitute);
            RaiseChanged();
            return true;
        }

        public bool RefuseLevy(int day)
        {
            ClearLevyFlags();
            _state.levyRefuse = true;
            _state.levyRefuseDay = day;
            _state.edorWaitingAtHatch = true;
            if (_state.levy != null)
            {
                MarkAssigned(_state.levy.survivorIds, false);
                _state.levy.active = false;
            }
            OnLevyResolved?.Invoke(FlagLevyRefuse);
            Activate12C();
            RaiseChanged();
            return true;
        }

        public void Activate12C()
        {
            if (_state.order12cActive) return;
            _state.order12cActive = true;
            On12CActivated?.Invoke();
            RaiseChanged();
        }

        public void TickDaily(int day)
        {
            if (_state.levy != null && _state.levy.active)
            {
                _state.levy.remainingDays--;
                if (_state.levy.remainingDays <= 0)
                {
                    MarkAssigned(_state.levy.survivorIds, false);
                    _state.levy.active = false;
                    RaiseChanged();
                }
            }
        }

        public bool IsAssignedAway(string survivorId)
        {
            var row = Find(survivorId);
            return row != null && row.assignedAway;
        }

        public IReadOnlyList<string> AssignedAwayIds()
        {
            var list = new List<string>();
            for (int i = 0; i < _state.ledger.Count; i++)
                if (_state.ledger[i] != null && _state.ledger[i].assignedAway)
                    list.Add(_state.ledger[i].survivorId);
            if (_state.levy != null && _state.levy.active && _state.levy.survivorIds != null)
            {
                for (int i = 0; i < _state.levy.survivorIds.Length; i++)
                {
                    string id = _state.levy.survivorIds[i];
                    if (!string.IsNullOrEmpty(id) && !list.Contains(id))
                        list.Add(id);
                }
            }
            return list;
        }

        public void AdjustOfficeTrust(float delta)
        {
            _state.officeTrust = Math.Clamp(_state.officeTrust + delta, -100f, 100f);
            RaiseChanged();
        }

        public CensusClaimSystemState CaptureState()
        {
            return Clone(_state);
        }

        public void RestoreState(CensusClaimSystemState saved)
        {
            // Deep-copy: the deserialized DTO must not become the live state.
            // Otherwise the caller's save object and the running system alias
            // the same ledger and a later mutation corrupts the envelope.
            _state = Clone(saved ?? new CensusClaimSystemState());
            if (_state.ledger == null) _state.ledger = new List<CensusLedgerEntry>();
            if (_state.levy == null) _state.levy = new LevyOrder();
            if (string.IsNullOrEmpty(_state.systemId)) _state.systemId = SystemId;
            RaiseChanged();
        }

        private void ClearLevyFlags()
        {
            _state.levyHonour = false;
            _state.levySubstitute = false;
            _state.levyRefuse = false;
        }

        private void MarkAssigned(string[] ids, bool away)
        {
            if (ids == null) return;
            for (int i = 0; i < ids.Length; i++)
            {
                var row = Find(ids[i]);
                if (row == null && !string.IsNullOrEmpty(ids[i]))
                {
                    row = new CensusLedgerEntry { survivorId = ids[i] };
                    _state.ledger.Add(row);
                }
                if (row != null) row.assignedAway = away;
            }
        }

        private CensusLedgerEntry? Find(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            for (int i = 0; i < _state.ledger.Count; i++)
                if (_state.ledger[i] != null && _state.ledger[i].survivorId == survivorId)
                    return _state.ledger[i];
            return null;
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);

        private static CensusClaimSystemState Clone(CensusClaimSystemState src)
        {
            var dst = new CensusClaimSystemState
            {
                systemId = src.systemId,
                levyHonour = src.levyHonour,
                levySubstitute = src.levySubstitute,
                levyRefuse = src.levyRefuse,
                order12cActive = src.order12cActive,
                edorWaitingAtHatch = src.edorWaitingAtHatch,
                clerkInterviewDone = src.clerkInterviewDone,
                officeTrust = src.officeTrust,
                levyRefuseDay = src.levyRefuseDay,
                ledger = new List<CensusLedgerEntry>(),
                levy = new LevyOrder()
            };
            if (src.ledger != null)
            {
                for (int i = 0; i < src.ledger.Count; i++)
                {
                    var e = src.ledger[i];
                    if (e == null) continue;
                    dst.ledger.Add(new CensusLedgerEntry
                    {
                        survivorId = e.survivorId,
                        displayName = e.displayName,
                        occupationGuess = e.occupationGuess,
                        occupationObserved = e.occupationObserved,
                        listed = e.listed,
                        scoreIfKnown = e.scoreIfKnown,
                        assignedAway = e.assignedAway
                    });
                }
            }
            if (src.levy != null)
            {
                dst.levy.durationDays = src.levy.durationDays;
                dst.levy.destinationNodeId = src.levy.destinationNodeId;
                dst.levy.issuedDay = src.levy.issuedDay;
                dst.levy.remainingDays = src.levy.remainingDays;
                dst.levy.active = src.levy.active;
                if (src.levy.survivorIds != null)
                {
                    dst.levy.survivorIds = new string[src.levy.survivorIds.Length];
                    Array.Copy(src.levy.survivorIds, dst.levy.survivorIds, src.levy.survivorIds.Length);
                }
            }
            return dst;
        }
    }
}
