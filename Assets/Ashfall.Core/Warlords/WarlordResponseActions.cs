using System;
using System.Collections.Generic;
using Ashfall.Core.Warlords;

namespace Ashfall.Core.Warlords
{
    /// <summary>
    /// ASHFALL Warlord Radar response actions (item 14).
    ///
    /// Atomic, idempotent command surface over WarlordDoctrineSystem so
    /// the WarlordRadarPanel can issue Pay / Contest / Submit responses
    /// without double-rewarding the player.
    /// </summary>
    public sealed class WarlordResponseActions
    {
        private readonly WarlordResponseState _state;
        private readonly WarlordDoctrineSystem _system;

        public event Action<WarlordResponseRecord>? OnResponded;

        public WarlordResponseActions(WarlordResponseState state, WarlordDoctrineSystem system)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
            _system = system ?? throw new ArgumentNullException(nameof(system));
        }

        public bool IsResponded(string tributeId)
        {
            if (string.IsNullOrEmpty(tributeId)) return false;
            for (int i = 0; i < _state.Responses.Count; i++)
                if (_state.Responses[i].TributeId == tributeId) return true;
            return false;
        }

        public WarlordResponseResult Pay(string tributeId, int amountPaid, int day)
        {
            if (string.IsNullOrEmpty(tributeId))
                return WarlordResponseResult.Fail("missing_tribute_id");
            if (amountPaid < 0)
                return WarlordResponseResult.Fail("invalid_amount");
            if (IsResponded(tributeId))
                return WarlordResponseResult.Fail("already_responded");

            int nextAsk;
            bool paidFull = _system.SettleTribute(amountPaid, day, out nextAsk);

            var rec = new WarlordResponseRecord
            {
                TributeId = tributeId,
                Kind = WarlordResponseKind.Pay,
                AmountPaid = amountPaid,
                Day = day,
                SettledFully = paidFull
            };
            _state.Responses.Add(rec);
            OnResponded?.Invoke(rec);
            return WarlordResponseResult.Ok(rec);
        }

        public WarlordResponseResult Contest(string tributeId, int day)
        {
            if (string.IsNullOrEmpty(tributeId))
                return WarlordResponseResult.Fail("missing_tribute_id");
            if (IsResponded(tributeId))
                return WarlordResponseResult.Fail("already_responded");
            var rec = new WarlordResponseRecord
            {
                TributeId = tributeId,
                Kind = WarlordResponseKind.Contest,
                AmountPaid = 0,
                Day = day,
                SettledFully = false
            };
            _state.Responses.Add(rec);
            OnResponded?.Invoke(rec);
            return WarlordResponseResult.Ok(rec);
        }

        public WarlordResponseResult Submit(string tributeId, int day)
        {
            if (string.IsNullOrEmpty(tributeId))
                return WarlordResponseResult.Fail("missing_tribute_id");
            if (IsResponded(tributeId))
                return WarlordResponseResult.Fail("already_responded");
            var rec = new WarlordResponseRecord
            {
                TributeId = tributeId,
                Kind = WarlordResponseKind.Submit,
                AmountPaid = 0,
                Day = day,
                SettledFully = true
            };
            _state.Responses.Add(rec);
            OnResponded?.Invoke(rec);
            return WarlordResponseResult.Ok(rec);
        }

        public WarlordResponseState CaptureState() => _state.Capture();

        public void RestoreState(WarlordResponseState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            _state.RestoreInto(state);
        }
    }

    public enum WarlordResponseKind
    {
        Pay,
        Contest,
        Submit
    }

    [Serializable]
    public sealed class WarlordResponseRecord
    {
        public string TributeId;
        public WarlordResponseKind Kind;
        public int AmountPaid;
        public int Day;
        public bool SettledFully;
    }

    [Serializable]
    public sealed class WarlordResponseState
    {
        public List<WarlordResponseRecord> Responses = new List<WarlordResponseRecord>();

        public WarlordResponseState Capture() => new WarlordResponseState
        {
            Responses = new List<WarlordResponseRecord>(Responses)
        };

        public void RestoreInto(WarlordResponseState state)
        {
            Responses = state.Responses ?? new List<WarlordResponseRecord>();
        }
    }

    [Serializable]
    public sealed class WarlordResponseResult
    {
        public bool Succeeded;
        public string ReasonCode;
        public WarlordResponseRecord Record;

        public static WarlordResponseResult Ok(WarlordResponseRecord r)
            => new WarlordResponseResult { Succeeded = true, ReasonCode = "ok", Record = r };

        public static WarlordResponseResult Fail(string reason)
            => new WarlordResponseResult { Succeeded = false, ReasonCode = reason ?? "fail", Record = null! };
    }
}
