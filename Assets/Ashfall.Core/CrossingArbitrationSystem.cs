using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — §5.1 CrossingArbitrationSystem.
    /// The Standing. A ruling is real for as long as three backers hold it.
    /// Engine-agnostic extract of Assets/_Game/Core/CrossingArbitrationSystem.cs.
    /// Core additions over the Unity original (house pattern): OnStateChanged
    /// on every mutation, defensive CaptureState copy, null-safe RestoreState.
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
        public List<string> bribedBackers = new List<string>(); // bought, not earned
        public int bribeMarks; // public refusals / known-bought marks on this ruling
        public List<string> refusedBribes = new List<string>(); // principled backers who refused publicly
    }

    public enum RulingShape
    {
        Pending,    // called but not yet 3 backers
        Honest,     // 3+ backers, none bought, principled majority behind it
        Rigged,     // 3+ backers, but bought (or no principled majority to vouch for it)
        Overturned  // 3+ counter-backers
    }

    /// <summary>
    /// Outcome of attempting to buy a backer. A principled backer refuses
    /// outright and says so publicly — the refusal is itself a mark on the
    /// ruling (bible §5.1: "some backers refuse a bribe outright and will
    /// say so publicly if pushed, which itself becomes a mark").
    /// </summary>
    public enum BribeResult
    {
        Invalid,          // no pending ruling / dead / unknown / committed backer
        Accepted,          // a non-principled backer took the bribe
        RefusedPrincipled  // a principled backer refused, publicly (a mark)
    }

    [Serializable]
    public class CrossingArbitrationState
    {
        public string systemId = CrossingArbitrationSystem.SystemId;
        public List<BackerDef> backerPool = new List<BackerDef>();
        public List<StandingRuling> rulings = new List<StandingRuling>();
        public int rulingsCalled;
        public int rulingsOverturned;
        public int standingRepeats; // re-Standings called after an overturn
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
        public event Action<string, string> OnBribeRefused;    // backerId, topic (public mark)
        public event Action<CrossingArbitrationState> OnStateChanged;

        public CrossingArbitrationState State => _state;
        public IReadOnlyList<BackerDef> BackerPool => _state.backerPool;
        public IReadOnlyList<StandingRuling> Rulings => _state.rulings;

        // ── Initialisation ─────────────────────────────────────────────

        public void LoadBackerPool(IReadOnlyList<BackerDef> defs)
        {
            _state.backerPool.Clear();
            if (defs == null) return;
            for (int i = 0; i < defs.Count; i++)
            {
                var d = defs[i];
                if (d == null) continue;
                // Deep copy: the pool owns its backers, so a caller mutating
                // (or reusing) the source list cannot change live rulings.
                _state.backerPool.Add(new BackerDef
                {
                    id = d.id,
                    displayName = d.displayName,
                    wants = d.wants,
                    willNot = d.willNot,
                    principled = d.principled,
                    isAlive = d.isAlive
                });
            }
            RaiseChanged();
        }

        // ── Queries ────────────────────────────────────────────────────

        public BackerDef? GetBacker(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < _state.backerPool.Count; i++)
            {
                var b = _state.backerPool[i];
                if (b != null && b.id == id) return b;
            }
            return null;
        }

        public StandingRuling? GetRuling(string topic)
        {
            if (string.IsNullOrEmpty(topic)) return null;
            // Latest match wins: an overturned ruling can be re-Stood, so a
            // topic may carry history. The active ruling is the most recent.
            StandingRuling? result = null;
            for (int i = 0; i < _state.rulings.Count; i++)
            {
                var r = _state.rulings[i];
                if (r != null && r.topic == topic) result = r;
            }
            return result;
        }

        /// <summary>Every ruling ever called on this topic, oldest first (history).</summary>
        public List<StandingRuling> GetRulingHistory(string topic)
        {
            var result = new List<StandingRuling>();
            if (string.IsNullOrEmpty(topic)) return result;
            for (int i = 0; i < _state.rulings.Count; i++)
            {
                var r = _state.rulings[i];
                if (r != null && r.topic == topic) result.Add(r);
            }
            return result;
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

        /// <summary>
        /// True when the topic's ruling is held honestly (3+ backers, none
        /// bought, principled majority). A rigged ruling is on the board but
        /// is not "held honestly" — use IsRulingActive for control queries.
        /// </summary>
        public bool IsRulingHeld(string topic)
        {
            var r = GetRuling(topic);
            return r != null && r.shape == RulingShape.Honest && r.backers.Count >= BackersToHold;
        }

        /// <summary>
        /// True when the topic's ruling is currently on the board — held
        /// honestly or held bought (rigged). Quest/mutation logic reads this
        /// for "who currently controls X at the Crossing" (bible §5.1).
        /// </summary>
        public bool IsRulingActive(string topic)
        {
            var r = GetRuling(topic);
            return r != null && (r.shape == RulingShape.Honest || r.shape == RulingShape.Rigged);
        }

        public bool IsRulingOverturned(string topic)
        {
            var r = GetRuling(topic);
            return r != null && r.shape == RulingShape.Overturned;
        }

        // ── Actions ────────────────────────────────────────────────────

        /// <summary>
        /// Call a Standing on a topic. Creates a pending ruling if none exists.
        /// A held (honest/rigged) ruling must be challenged via OverturnRuling,
        /// not re-called. An overturned ruling may be re-Stood — nothing is
        /// permanently settled (bible §5.1).
        /// </summary>
        public bool CallStanding(string topic, int currentDay)
        {
            if (string.IsNullOrEmpty(topic)) return false;
            var existing = GetRuling(topic);

            if (existing != null)
            {
                if (existing.shape == RulingShape.Pending)
                {
                    // Idempotent re-call on a pending ruling.
                    _state.rulingsCalled++;
                    OnStandingCalled?.Invoke(topic);
                    RaiseChanged();
                    return true;
                }
                if (existing.shape == RulingShape.Honest || existing.shape == RulingShape.Rigged)
                    return false; // held — challenge via OverturnRuling
                // Overturned: re-Standing. Fall through to a fresh pending ruling.
                _state.standingRepeats++;
            }

            var ruling = new StandingRuling
            {
                topic = topic,
                shape = RulingShape.Pending,
                dayCalled = currentDay
            };
            _state.rulings.Add(ruling);

            _state.rulingsCalled++;
            OnStandingCalled?.Invoke(topic);
            RaiseChanged();
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
                ruling.shape = ResolveHoldShape(ruling);
                OnRulingMade?.Invoke(ruling);
            }

            RaiseChanged();
            return true;
        }

        /// <summary>
        /// Attempt to buy a backer's support (bible §5.1 principled cap).
        /// A principled backer refuses outright and the refusal is a public
        /// mark on the ruling; a non-principled backer accepts and the ruling
        /// is then known-bought — it will hold Rigged, never Honest.
        /// Only valid while the ruling is Pending; held/overturned/missing
        /// rulings and dead or already-committed backers return Invalid.
        /// </summary>
        public BribeResult TryBribeBacker(string topic, string backerId)
        {
            if (string.IsNullOrEmpty(topic) || string.IsNullOrEmpty(backerId))
                return BribeResult.Invalid;

            var ruling = GetRuling(topic);
            if (ruling == null || ruling.shape != RulingShape.Pending)
                return BribeResult.Invalid;

            var backer = GetBacker(backerId);
            if (backer == null || !backer.isAlive || ruling.backers.Contains(backerId))
                return BribeResult.Invalid;

            if (backer.principled)
            {
                if (ruling.refusedBribes == null) ruling.refusedBribes = new List<string>();
                // A principled backer refuses once, publicly. Pushing again
                // yields nothing new — the refusal is already a mark.
                if (ruling.refusedBribes.Contains(backerId)) return BribeResult.Invalid;
                ruling.refusedBribes.Add(backerId);
                ruling.bribeMarks++;
                OnBribeRefused?.Invoke(backerId, topic);
                RaiseChanged();
                return BribeResult.RefusedPrincipled;
            }

            ruling.backers.Add(backerId);
            if (ruling.bribedBackers == null) ruling.bribedBackers = new List<string>();
            ruling.bribedBackers.Add(backerId);

            if (ruling.backers.Count >= BackersToHold && ruling.shape == RulingShape.Pending)
            {
                ruling.shape = ResolveHoldShape(ruling);
                OnRulingMade?.Invoke(ruling);
            }

            RaiseChanged();
            return BribeResult.Accepted;
        }

        /// <summary>
        /// Overturn an existing ruling by bringing 3+ counter-backers.
        /// Counters must be distinct, living backers and a *different* set
        /// from the current holders (bible §5.1: "a different 3+ backers").
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

            // Counters must be distinct, living backers, and not the same
            // set that currently holds the ruling.
            var seen = new HashSet<string>();
            bool differsFromHolders = false;
            for (int i = 0; i < counterBackerIds.Count; i++)
            {
                var id = counterBackerIds[i];
                if (string.IsNullOrEmpty(id)) return false;
                var b = GetBacker(id);
                if (b == null || !b.isAlive) return false;
                if (!seen.Add(id)) return false; // duplicate counter
                if (!ruling.backers.Contains(id)) differsFromHolders = true;
            }
            if (!differsFromHolders) return false;

            ruling.shape = RulingShape.Overturned;
            ruling.backers.Clear();
            if (ruling.bribedBackers != null) ruling.bribedBackers.Clear();
            _state.rulingsOverturned++;
            OnRulingOverturned?.Invoke(ruling);
            RaiseChanged();
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
                if (r.bribedBackers != null) r.bribedBackers.Remove(backerId);
                if (r.shape != RulingShape.Overturned && r.backers.Count < BackersToHold)
                    r.shape = RulingShape.Pending;
            }
            RaiseChanged();
            return true;
        }

        // ── Helpers ─────────────────────────────────────────────────────

        /// <summary>
        /// A ruling that reaches three backers holds. It is Honest only when
        /// no backer was bought and a principled majority stands behind it; a
        /// bought ruling is Rigged even if principled backers also hold it —
        /// the purchase is public knowledge (bible §5.1).
        /// </summary>
        private RulingShape ResolveHoldShape(StandingRuling ruling)
        {
            if (ruling.bribedBackers != null && ruling.bribedBackers.Count > 0)
                return RulingShape.Rigged;
            return HasPrincipledMajority(ruling) ? RulingShape.Honest : RulingShape.Rigged;
        }

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

        public CrossingArbitrationState CaptureState()
        {
            return CloneState(_state);
        }

        /// <summary>
        /// Deep copy: the live system and the serialized envelope must never
        /// alias the same lists, or a later mutation corrupts the save.
        /// </summary>
        private static CrossingArbitrationState CloneState(CrossingArbitrationState from)
        {
            var copy = new CrossingArbitrationState
            {
                systemId = from.systemId,
                rulingsCalled = from.rulingsCalled,
                rulingsOverturned = from.rulingsOverturned,
                standingRepeats = from.standingRepeats,
                backerPool = new List<BackerDef>(),
                rulings = new List<StandingRuling>()
            };
            if (from.backerPool != null)
            {
                for (int i = 0; i < from.backerPool.Count; i++)
                {
                    var b = from.backerPool[i];
                    if (b == null) continue;
                    copy.backerPool.Add(new BackerDef
                    {
                        id = b.id,
                        displayName = b.displayName,
                        wants = b.wants,
                        willNot = b.willNot,
                        principled = b.principled,
                        isAlive = b.isAlive
                    });
                }
            }
            if (from.rulings != null)
            {
                for (int i = 0; i < from.rulings.Count; i++)
                {
                    var r = from.rulings[i];
                    if (r == null) continue;
                    copy.rulings.Add(new StandingRuling
                    {
                        topic = r.topic,
                        shape = r.shape,
                        dayCalled = r.dayCalled,
                        bribeMarks = r.bribeMarks,
                        backers = r.backers != null ? new List<string>(r.backers) : new List<string>(),
                        bribedBackers = r.bribedBackers != null ? new List<string>(r.bribedBackers) : new List<string>(),
                        refusedBribes = r.refusedBribes != null ? new List<string>(r.refusedBribes) : new List<string>()
                    });
                }
            }
            return copy;
        }

        public void RestoreState(CrossingArbitrationState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
            if (string.IsNullOrEmpty(_state.systemId)) _state.systemId = SystemId;
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
