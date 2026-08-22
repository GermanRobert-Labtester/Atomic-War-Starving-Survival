using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class AirlockVisitor
    {
        public string visitorId = string.Empty;
        public string visitorKind = string.Empty;
        public int arrivalSequence;
    }

    [Serializable]
    public sealed class AirlockSecurityState
    {
        public List<AirlockVisitor> visitors = new List<AirlockVisitor>();
        public int totalArrivals;
    }

    /// <summary>
    /// Minimal engine-agnostic airlock authority used by decontamination and
    /// future access-control systems. Arrivals are deterministic and save-safe;
    /// callers decide admission/release policy at the higher domain layer.
    /// </summary>
    public sealed class AirlockSecuritySystem
    {
        private AirlockSecurityState _state = new AirlockSecurityState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        public AirlockSecurityState State => _state;
        public event Action OnAirlockChanged;

        public AirlockSecuritySystem(ISeededRng rng, ILog log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        public AirlockVisitor VisitorArrives(string visitorId, string visitorKind)
        {
            if (string.IsNullOrEmpty(visitorId))
                throw new ArgumentException("Visitor id is required.", nameof(visitorId));

            var existing = _state.visitors.Find(v => v.visitorId == visitorId);
            if (existing != null) return existing;

            var visitor = new AirlockVisitor
            {
                visitorId = visitorId,
                visitorKind = visitorKind ?? string.Empty,
                arrivalSequence = ++_state.totalArrivals
            };
            _state.visitors.Add(visitor);
            _log.Info($"[Airlock] arrival {visitor.visitorId} ({visitor.visitorKind})");
            OnAirlockChanged?.Invoke();
            return visitor;
        }

        public bool ReleaseVisitor(string visitorId)
        {
            int removed = _state.visitors.RemoveAll(v => v.visitorId == visitorId);
            if (removed == 0) return false;
            OnAirlockChanged?.Invoke();
            return true;
        }

        public bool ContainsVisitor(string visitorId) =>
            _state.visitors.Exists(v => v.visitorId == visitorId);

        public void TickDay(int day)
        {
            // No passive mutation yet. Kept explicit for deterministic host ticking.
        }

        public AirlockSecurityState CaptureState()
        {
            var clone = new AirlockSecurityState { totalArrivals = _state.totalArrivals };
            foreach (var visitor in _state.visitors)
            {
                clone.visitors.Add(new AirlockVisitor
                {
                    visitorId = visitor.visitorId,
                    visitorKind = visitor.visitorKind,
                    arrivalSequence = visitor.arrivalSequence
                });
            }
            return clone;
        }

        public void RestoreState(AirlockSecurityState saved)
        {
            if (saved == null) return;
            _state = new AirlockSecurityState { totalArrivals = saved.totalArrivals };
            if (saved.visitors != null)
            {
                foreach (var visitor in saved.visitors)
                {
                    if (visitor == null) continue;
                    _state.visitors.Add(new AirlockVisitor
                    {
                        visitorId = visitor.visitorId ?? string.Empty,
                        visitorKind = visitor.visitorKind ?? string.Empty,
                        arrivalSequence = visitor.arrivalSequence
                    });
                }
            }
            OnAirlockChanged?.Invoke();
        }
    }
}
