using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — §5.2 CrossingArbitrationSystem.
    /// The Standing. A ruling is real for as long as three backers hold it.
    /// Plain C#, event-driven, save/load safe.
    ///
    /// Pool of named stallholders; 3 declared backers to hold a ruling;
    /// 3+ can overturn. Principled backers cap pure bribery.
    /// Not a political sim. Not agent-based. Scripted micro-disputes only.
    /// </summary>

    // ── Data types ─────────────────────────────────────────────────────

    [Serializable]
    public class BackerDef
    {
        public string id;
        public string displayName;
        public string wants;        // what motivates this backer
        public string willNot;      // hard limit — will not cross
        public bool principled;     // cannot be bribed; caps pure-buy rulings
        public bool isAlive = true; // dead backers lose their hold
    }

    [Serializable]
    public class StandingRuling
    {
        public string topic;       // dispute subject (quest id or freeform)
        public List<string> backers = new List<string>(); // backer ids holding this
        public RulingShape shape;
        public int dayCalled;
    }

    public enum RulingShape
    {
        Pending,    // called but not yet 3 backers
        Honest,     // 3+ backers, no bribes
        Rigged,     // 3+ backers, but bribery detected
        Overturned  // 3+ counter-backers
    }

    [Serializable]
    public class CrossingArbitrationState
    {
        public string systemId = CrossingArbitrationSystem.SystemId;
        public List<BackerDef> backerPool = new List<BackerDef>();
        public List<StandingRuling> rulings = new List<StandingRuling>();
        public int rulingsCalled;
        public int rulingsOverturned;
    }

    // ── System ─────────────────────────────────────────────────────────

    public class CrossingArbitrationSystem
    {
        public const string SystemId = "crossing_arbitration_system";
        public const int BackersToHold = 3;

        private CrossingArbitrationState _state = new CrossingArbitrationState();

        public event Action<string> OnStandingCalled;          // topic
        public event Action<StandingRuling> OnRulingMade;      // the ruling that now holds
        public event Action<StandingRuling> OnRulingOverturned;

        public CrossingArbitrationState State => _state;
        public IReadOnlyList<BackerDef> BackerPool => _state.backerPool;
        public IReadOnlyList<StandingRuling> Rulings => _state.rulings;

        // ── Initialisation ─────────────────────────────────────────────

        public void LoadBackerPool(IReadOnlyList<BackerDef> defs)
        {
            _state.backerPool.Clear();
            if (defs == null) return;
            for (int i = 0; i < defs.Count; i++)
                _state.backerPool.Add(defs[i]);
        }

        // ── Queries ────────────────────────────────────────────────────

        public BackerDef GetBacker(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < _state.backerPool.Count; i++)
            {
                var b = _state.backerPool[i];
                if (b != null && b.id == id) return b;
            }
            return null;
        }

        public StandingRuling GetRuling(string topic)
        {
            if (string.IsNullOrEmpty(topic)) return null;
            for (int i = 0; i < _state.rulings.Count; i++)
            {
                var r = _state.rulings[i];
                if (r != null && r.topic == topic) return r;
            }
            return null;
        }

        /// <summary>All living backers not already committed to this topic.</summary>
        public List<BackerDef> GetAvailableBackers(string topic)
        {
            var result = new List<BackerDef>();
            var existing = GetRuling(topic);
            for (int i = 0; i < _state.backerPool.Count; i++)
            {
                var b = _state.backerPool[i];
                if (b == null || !b.isAlive) continue;
                if (existing != null && existing.backers.Contains(b.id)) continue;
                result.Add(b);
            }
            return result;
        }

        public bool IsRulingHeld(string topic)
        {
            var r = GetRuling(topic);
            return r != null && r.shape == RulingShape.Honest && r.backers.Count >= BackersToHold;
        }

        public bool IsRulingOverturned(string topic)
        {
            var r = GetRuling(topic);
            return r != null && r.shape == RulingShape.Overturned;
        }

        // ── Actions ────────────────────────────────────────────────────

        /// <summary>
        /// Call a Standing on a topic. Creates a pending ruling if none exists.
        /// Returns false if the topic already has a held or overturned ruling.
        /// </summary>
        public bool CallStanding(string topic, int currentDay)
        {
            if (string.IsNullOrEmpty(topic)) return false;
            var existing = GetRuling(topic);
            if (existing != null && existing.shape != RulingShape.Pending) return false;

            if (existing == null)
            {
                existing = new StandingRuling
                {
                    topic = topic,
                    shape = RulingShape.Pending,
                    dayCalled = currentDay
                };
                _state.rulings.Add(existing);
            }

            _state.rulingsCalled++;
            OnStandingCalled?.Invoke(topic);
            return true;
        }

        /// <summary>
        /// A backer declares support for the topic's ruling.
        /// Returns false if the backer is dead, already committed, or the
        /// ruling is already final (held / overturned).
        /// </summary>
        public bool DeclareBacker(string topic, string backerId)
        {
            if (string.IsNullOrEmpty(topic) || string.IsNullOrEmpty(backerId)) return false;

            var backer = GetBacker(backerId);
            if (backer == null || !backer.isAlive) return false;

            var ruling = GetRuling(topic);
            if (ruling == null) return false; // CallStanding first
            if (ruling.shape == RulingShape.Overturned) return false;
            if (ruling.backers.Contains(backerId)) return false;

            ruling.backers.Add(backerId);

            if (ruling.backers.Count >= BackersToHold && ruling.shape == RulingShape.Pending)
            {
                // Determine shape: honest vs rigged based on principled backer presence
                ruling.shape = HasPrincipledMajority(ruling)
                    ? RulingShape.Honest
                    : RulingShape.Rigged;
                OnRulingMade?.Invoke(ruling);
            }

            return true;
        }

        /// <summary>
        /// Overturn an existing ruling by bringing 3+ counter-backers.
        /// The ruling's shape becomes Overturned; backers are cleared.
        /// </summary>
        public bool OverturnRuling(string topic, IReadOnlyList<string> counterBackerIds)
        {
            if (string.IsNullOrEmpty(topic) || counterBackerIds == null) return false;

            var ruling = GetRuling(topic);
            if (ruling == null) return false;
            if (ruling.shape != RulingShape.Honest && ruling.shape != RulingShape.Rigged)
                return false;

            if (counterBackerIds.Count < BackersToHold) return false;

            ruling.shape = RulingShape.Overturned;
            ruling.backers.Clear();
            _state.rulingsOverturned++;
            OnRulingOverturned?.Invoke(ruling);
            return true;
        }

        /// <summary>
        /// Kill a backer (death, exile, departure). Their held rulings
        /// lose one backer; if a held ruling drops below 3, it reverts
        /// to Pending.
        /// </summary>
        public bool RemoveBacker(string backerId)
        {
            var backer = GetBacker(backerId);
            if (backer == null || !backer.isAlive) return false;

            backer.isAlive = false;

            // Check all rulings this backer held
            for (int i = 0; i < _state.rulings.Count; i++)
            {
                var r = _state.rulings[i];
                if (r == null || !r.backers.Contains(backerId)) continue;
                r.backers.Remove(backerId);
                if (r.shape != RulingShape.Overturned && r.backers.Count < BackersToHold)
                    r.shape = RulingShape.Pending;
            }
            return true;
        }

        // ── Helpers ─────────────────────────────────────────────────────

        /// <summary>
        /// A principled majority means most backers cannot be bribed.
        /// If a majority are principled, the ruling is honest even if
        /// some non-principled backers were bought.
        /// </summary>
        private bool HasPrincipledMajority(StandingRuling ruling)
        {
            int principled = 0;
            for (int i = 0; i < ruling.backers.Count; i++)
            {
                var b = GetBacker(ruling.backers[i]);
                if (b != null && b.principled) principled++;
            }
            return principled > ruling.backers.Count / 2;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public CrossingArbitrationState CaptureState() => _state;

        public void RestoreState(CrossingArbitrationState saved)
        {
            if (saved == null) return;
            _state = saved;
        }
    }
}
