using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL: THE DUTY ROSTER — a flag plus a later sentence.
    /// Not a second morale meter. Not Morale +2.
    /// Spec: docs/expansions/expansion_02_the_duty_roster_plan.md §5.3.
    /// Marks do not expire on sleep; clear only by authored quest (e.g. tag burned).
    /// </summary>
    [Serializable]
    public class MoraleMarkRecord
    {
        public string id;
        public string payload;
        public int daySet;
    }

    [Serializable]
    public class MoraleMarkSystemState
    {
        public string systemId = MoraleMarkSystem.SystemId;
        public List<MoraleMarkRecord> marks = new List<MoraleMarkRecord>();
    }

    public class MoraleMarkSystem
    {
        public const string SystemId = "morale_mark_system";

        private MoraleMarkSystemState _state = new MoraleMarkSystemState();
        private readonly Dictionary<string, MoraleMarkRecord> _byId = new Dictionary<string, MoraleMarkRecord>();
        private readonly Dictionary<string, string> _laterProse = new Dictionary<string, string>();

        public event Action<string, string> OnMarkSet;
        public event Action<string> OnMarkCleared;
        public event Action<MoraleMarkSystemState> OnStateChanged;

        public MoraleMarkSystemState State => _state;
        public int Count => _byId.Count;

        public MoraleMarkSystem()
        {
            EnsureList();
        }

        public void BindCatalog(DutyRosterCatalog catalog)
        {
            _laterProse.Clear();
            if (catalog == null) return;
            for (int i = 0; i < catalog.Marks.Count; i++)
            {
                DutyRosterMarkEntry e = catalog.Marks[i];
                if (e == null || string.IsNullOrEmpty(e.id) || string.IsNullOrEmpty(e.later))
                    continue;
                _laterProse[e.id] = e.later;
            }
        }

        public void SetMark(string id, string payload = null, int day = 0)
        {
            if (string.IsNullOrEmpty(id)) return;
            if (!_byId.TryGetValue(id, out MoraleMarkRecord rec) || rec == null)
            {
                rec = new MoraleMarkRecord { id = id };
                _byId[id] = rec;
                _state.marks.Add(rec);
            }

            rec.payload = payload ?? string.Empty;
            rec.daySet = day;
            OnMarkSet?.Invoke(id, rec.payload);
            RaiseChanged();
        }

        public bool HasMark(string id)
        {
            return !string.IsNullOrEmpty(id) && _byId.ContainsKey(id);
        }

        public string GetPayload(string id)
        {
            if (string.IsNullOrEmpty(id)) return string.Empty;
            return _byId.TryGetValue(id, out MoraleMarkRecord rec) && rec != null
                ? (rec.payload ?? string.Empty)
                : string.Empty;
        }

        /// <summary>Inspect/bark sentence from duty_roster_marks.json, else the saved payload.</summary>
        public string GetLaterProse(string id)
        {
            if (string.IsNullOrEmpty(id)) return string.Empty;
            if (_laterProse.TryGetValue(id, out string later) && !string.IsNullOrEmpty(later))
                return later;
            return GetPayload(id);
        }

        public bool ClearMark(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            if (!_byId.TryGetValue(id, out MoraleMarkRecord rec) || rec == null)
                return false;
            _byId.Remove(id);
            _state.marks.Remove(rec);
            OnMarkCleared?.Invoke(id);
            RaiseChanged();
            return true;
        }

        public MoraleMarkSystemState CaptureState()
        {
            var copy = new MoraleMarkSystemState { systemId = _state.systemId };
            copy.marks = new List<MoraleMarkRecord>();
            if (_state.marks == null) return copy;
            for (int i = 0; i < _state.marks.Count; i++)
            {
                MoraleMarkRecord m = _state.marks[i];
                if (m == null) continue;
                copy.marks.Add(new MoraleMarkRecord
                {
                    id = m.id,
                    payload = m.payload,
                    daySet = m.daySet
                });
            }

            return copy;
        }

        public void RestoreState(MoraleMarkSystemState saved)
        {
            _state = saved ?? new MoraleMarkSystemState();
            if (string.IsNullOrEmpty(_state.systemId)) _state.systemId = SystemId;
            EnsureList();
            _byId.Clear();
            for (int i = 0; i < _state.marks.Count; i++)
            {
                MoraleMarkRecord m = _state.marks[i];
                if (m == null || string.IsNullOrEmpty(m.id)) continue;
                _byId[m.id] = m;
            }

            RaiseChanged();
        }

        private void EnsureList()
        {
            if (_state.marks == null) _state.marks = new List<MoraleMarkRecord>();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
