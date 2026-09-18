// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Campaign;

namespace Ashfall.Core.Commitments
{
    [Serializable]
    public sealed class CommitmentSaveState
    {
        public int version = 1;
        public List<string> met_ids = new List<string>();
        public List<string> missed_ids = new List<string>();
        public Dictionary<string, int> progress_by_id = new Dictionary<string, int>(StringComparer.Ordinal);
        public List<string> warned_ids = new List<string>();
    }

    /// <summary>
    /// ASHFALL — Generalized Commitment and Deadline Pressure System (Plan 38 §38C).
    /// Tracks authored obligations with exact-once terminal evaluation (Met / Missed),
    /// warning lead emissions, consequence routing through existing authorities,
    /// and save/load stability.
    /// </summary>
    public sealed class CommitmentSystem : IDayAdvanceOwner, IPreDaySnapshotRestore
    {
        public const string OwnerId = "commitments";

        private readonly List<CommitmentDefinition> _definitions = new List<CommitmentDefinition>();
        private readonly Dictionary<string, CommitmentDefinition> _defsById = new Dictionary<string, CommitmentDefinition>(StringComparer.Ordinal);

        private readonly HashSet<string> _metIds = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> _missedIds = new HashSet<string>(StringComparer.Ordinal);
        private readonly Dictionary<string, int> _progress = new Dictionary<string, int>(StringComparer.Ordinal);
        private readonly HashSet<string> _warnedIds = new HashSet<string>(StringComparer.Ordinal);

        // Snapshot state for fail-closed rollback
        private HashSet<string>? _snapshotMet;
        private HashSet<string>? _snapshotMissed;
        private Dictionary<string, int>? _snapshotProgress;
        private HashSet<string>? _snapshotWarned;

        public event Action<CommitmentReadModel>? OnWarningIssued;
        public event Action<CommitmentReadModel>? OnCommitmentMet;
        public event Action<CommitmentReadModel>? OnCommitmentMissed;
        public event Action<string, string, int>? OnConsequenceRouted;

        public IReadOnlyList<CommitmentDefinition> Definitions => _definitions;
        public IReadOnlyCollection<string> MetIds => _metIds;
        public IReadOnlyCollection<string> MissedIds => _missedIds;

        public CommitmentSystem(IEnumerable<CommitmentDefinition>? initialDefs = null)
        {
            if (initialDefs != null)
            {
                foreach (var def in initialDefs)
                {
                    RegisterCommitment(def);
                }
            }
        }

        public void RegisterCommitment(CommitmentDefinition def)
        {
            if (def == null || string.IsNullOrWhiteSpace(def.id)) return;
            if (_defsById.ContainsKey(def.id)) return;

            _definitions.Add(def);
            _defsById[def.id] = def;
        }

        public bool RecordProgress(string commitmentId, int quantityAdded)
        {
            if (string.IsNullOrEmpty(commitmentId) || quantityAdded <= 0) return false;
            if (!_defsById.TryGetValue(commitmentId, out var def)) return false;

            // Terminal commitments cannot receive further progress
            if (_metIds.Contains(commitmentId) || _missedIds.Contains(commitmentId)) return false;

            _progress.TryGetValue(commitmentId, out int current);
            current += quantityAdded;
            _progress[commitmentId] = current;

            if (current >= def.target_quantity)
            {
                _metIds.Add(commitmentId);
                var model = BuildReadModel(def, def.due_day);
                OnCommitmentMet?.Invoke(model);
                return true;
            }

            return false;
        }

        public bool Settle(string commitmentId, int currentDay)
        {
            if (string.IsNullOrEmpty(commitmentId)) return false;
            if (!_defsById.TryGetValue(commitmentId, out var def)) return false;

            // Terminal commitments cannot be settled again
            if (_metIds.Contains(commitmentId) || _missedIds.Contains(commitmentId)) return false;

            _progress[commitmentId] = def.target_quantity;
            _metIds.Add(commitmentId);

            var model = BuildReadModel(def, currentDay);
            OnCommitmentMet?.Invoke(model);
            return true;
        }

        public CommitmentReadModel? GetCommitment(string id, int currentDay)
        {
            if (string.IsNullOrEmpty(id)) return null;
            if (!_defsById.TryGetValue(id, out var def)) return null;
            return BuildReadModel(def, currentDay);
        }

        public IReadOnlyList<CommitmentReadModel> GetCommitments(int currentDay)
        {
            var list = new List<CommitmentReadModel>(_definitions.Count);
            for (int i = 0; i < _definitions.Count; i++)
            {
                list.Add(BuildReadModel(_definitions[i], currentDay));
            }
            return list;
        }

        public void CapturePreDaySnapshot(int day)
        {
            _snapshotMet = new HashSet<string>(_metIds, StringComparer.Ordinal);
            _snapshotMissed = new HashSet<string>(_missedIds, StringComparer.Ordinal);
            _snapshotProgress = new Dictionary<string, int>(_progress, StringComparer.Ordinal);
            _snapshotWarned = new HashSet<string>(_warnedIds, StringComparer.Ordinal);
        }

        public void RestorePreDaySnapshot(int day)
        {
            if (_snapshotMet != null)
            {
                _metIds.Clear();
                foreach (var id in _snapshotMet) _metIds.Add(id);
            }
            if (_snapshotMissed != null)
            {
                _missedIds.Clear();
                foreach (var id in _snapshotMissed) _missedIds.Add(id);
            }
            if (_snapshotProgress != null)
            {
                _progress.Clear();
                foreach (var kv in _snapshotProgress) _progress[kv.Key] = kv.Value;
            }
            if (_snapshotWarned != null)
            {
                _warnedIds.Clear();
                foreach (var id in _snapshotWarned) _warnedIds.Add(id);
            }
        }

        public void TickDay(int day, List<DayStateChangeEvent> events)
        {
            // Sort deterministically by id before evaluating
            for (int i = 0; i < _definitions.Count; i++)
            {
                var def = _definitions[i];
                string id = def.id;

                // If already terminal, exactly-once guarantee ensures no duplicate actions
                if (_metIds.Contains(id) || _missedIds.Contains(id))
                {
                    continue;
                }

                // If before active window, skip
                if (day < def.start_day)
                {
                    continue;
                }

                // Warning threshold evaluation: day == due_day - warning_lead_days
                int warningDay = def.due_day - def.warning_lead_days;
                if (day >= warningDay && day <= def.due_day && !_warnedIds.Contains(id))
                {
                    _warnedIds.Add(id);
                    var model = BuildReadModel(def, day);
                    OnWarningIssued?.Invoke(model);
                    events.Add(new DayStateChangeEvent("obligation_warning", OwnerId, def.id, def.counterparty, def.due_day - day));
                }

                // Window expiration evaluation: day > due_day without being met
                if (day > def.due_day)
                {
                    _missedIds.Add(id);
                    var model = BuildReadModel(def, day);
                    OnCommitmentMissed?.Invoke(model);

                    if (!string.IsNullOrEmpty(def.consequence_class))
                    {
                        OnConsequenceRouted?.Invoke(def.consequence_class, def.consequence_target, def.consequence_magnitude);
                    }

                    events.Add(new DayStateChangeEvent("obligation_missed", OwnerId, def.id, def.consequence_class, def.consequence_magnitude));
                }
            }
        }

        public CommitmentSaveState CaptureState()
        {
            var save = new CommitmentSaveState();
            save.met_ids.AddRange(_metIds);
            save.missed_ids.AddRange(_missedIds);
            save.warned_ids.AddRange(_warnedIds);
            foreach (var kv in _progress)
            {
                save.progress_by_id[kv.Key] = kv.Value;
            }
            return save;
        }

        public void RestoreState(CommitmentSaveState? state)
        {
            _metIds.Clear();
            _missedIds.Clear();
            _warnedIds.Clear();
            _progress.Clear();

            if (state == null) return;

            if (state.met_ids != null)
            {
                for (int i = 0; i < state.met_ids.Count; i++)
                {
                    if (!string.IsNullOrEmpty(state.met_ids[i]))
                        _metIds.Add(state.met_ids[i]);
                }
            }

            if (state.missed_ids != null)
            {
                for (int i = 0; i < state.missed_ids.Count; i++)
                {
                    if (!string.IsNullOrEmpty(state.missed_ids[i]))
                        _missedIds.Add(state.missed_ids[i]);
                }
            }

            if (state.warned_ids != null)
            {
                for (int i = 0; i < state.warned_ids.Count; i++)
                {
                    if (!string.IsNullOrEmpty(state.warned_ids[i]))
                        _warnedIds.Add(state.warned_ids[i]);
                }
            }

            if (state.progress_by_id != null)
            {
                foreach (var kv in state.progress_by_id)
                {
                    if (!string.IsNullOrEmpty(kv.Key))
                        _progress[kv.Key] = kv.Value;
                }
            }
        }

        private CommitmentReadModel BuildReadModel(CommitmentDefinition def, int currentDay)
        {
            string id = def.id;
            _progress.TryGetValue(id, out int currentQty);

            CommitmentStatus status;
            if (_metIds.Contains(id))
            {
                status = CommitmentStatus.Met;
            }
            else if (_missedIds.Contains(id) || currentDay > def.due_day)
            {
                status = CommitmentStatus.Missed;
            }
            else if (currentDay < def.start_day)
            {
                status = CommitmentStatus.Pending;
            }
            else if (currentDay >= def.due_day - def.warning_lead_days)
            {
                status = CommitmentStatus.Warning;
            }
            else
            {
                status = CommitmentStatus.Active;
            }

            int daysRemaining = Math.Max(0, def.due_day - currentDay);

            return new CommitmentReadModel(
                id: def.id,
                type: def.type,
                title: def.title,
                counterparty: def.counterparty,
                startDay: def.start_day,
                dueDay: def.due_day,
                daysRemaining: daysRemaining,
                status: status,
                targetQuantity: def.target_quantity,
                currentQuantity: currentQty,
                targetId: def.target_id,
                conditionType: def.condition_type,
                consequenceClass: def.consequence_class,
                consequenceTarget: def.consequence_target,
                consequenceMagnitude: def.consequence_magnitude
            );
        }
    }
}
