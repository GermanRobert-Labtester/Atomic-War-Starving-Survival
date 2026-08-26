using System;
using Ashfall.Core.Flags;
using Ashfall.Core.MoralChoice;

namespace Ashfall.Core.Factions
{
    /// <summary>
    /// Resolution engine for the Rebel faction slice of "The Weight of
    /// Choices" branching system. Structural mirror of MilitaryBranchSystem —
    /// same method set, same idempotency and clamping rules — so the two
    /// faction branch systems stay behaviorally identical even though their
    /// branch/ending data differs. See MilitaryBranchSystem's remarks for the
    /// full design rationale (morality as a gate, not a judgment; faction
    /// alignment as a value distinct from the player's own morality score).
    ///
    /// Zero engine dependencies; deterministic; runs on its own local day
    /// counter (RebelBranchTimelineState), independent of the global IClock,
    /// of Year of Ash's day 180-360 window, and of MilitaryBranchSystem's own
    /// timeline (a player is only ever on one of the two in a playthrough,
    /// but the systems do not share a clock instance).
    /// </summary>
    public sealed class RebelBranchSystem
    {
        public const string SystemId = "rebel_branch_system";

        /// <summary>Faction alignment is clamped to the same -200..+200 range as
        /// MoralChoiceSystem purely for player-facing scale consistency.</summary>
        public const int MinAlignment = -200;
        public const int MaxAlignment = 200;

        private readonly RebelBranchCatalog _catalog;
        private readonly IFlagLedger _flags;
        private readonly ILog _log;
        private RebelBranchSystemState _state;

        public event Action<string>? OnBranchCommitted;
        public event Action<string, int>? OnPonrLocked;
        public event Action<string>? OnEndingResolved;
        public event Action<int>? OnAlignmentChanged;

        public RebelBranchSystem(RebelBranchCatalog catalog, IFlagLedger flags, RebelBranchSystemState? state = null, ILog? log = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _flags = flags ?? throw new ArgumentNullException(nameof(flags));
            _log = log ?? NullLog.Instance;
            _state = state ?? new RebelBranchSystemState();
            if (_state.timeline == null) _state.timeline = new RebelBranchTimelineState();
            if (_state.branch == null) _state.branch = new RebelBranchRecord();
            if (_state.rebelAlignment == null)
                _state.rebelAlignment = new FactionAlignmentRecord { factionId = RebelBranchIds.FactionId, alignment = -80 };
            if (_state.setFlags == null) _state.setFlags = new System.Collections.Generic.List<string>();
        }

        public RebelBranchSystemState State => _state;
        public int CurrentDay => _state.timeline.currentDay;
        public string? CommittedBranchId => string.IsNullOrEmpty(_state.branch.branchId) ? null : _state.branch.branchId;
        public bool IsPonrLocked => _state.branch.ponrLocked;
        public int RebelAlignment => _state.rebelAlignment.alignment;
        public string? ResolvedEndingId => string.IsNullOrEmpty(_state.branch.resolvedEndingId) ? null : _state.branch.resolvedEndingId;

        public void AdvanceDay(int day)
        {
            if (day < 0) throw new ArgumentOutOfRangeException(nameof(day));
            if (day < _state.timeline.currentDay) return; // never rewind
            _state.timeline.currentDay = day;
        }

        /// <summary>
        /// Commits the player to a base Rebel branch. Soft-gated by the
        /// branch's entry band range read from the catalog. Committing twice
        /// is a no-op that returns the already-committed branch (first
        /// commit wins), matching MilitaryBranchSystem.CommitBranch.
        /// </summary>
        public string CommitBranch(string branchId, MoralChoiceSystem moralChoice)
        {
            if (string.IsNullOrEmpty(branchId)) throw new ArgumentNullException(nameof(branchId));
            if (moralChoice == null) throw new ArgumentNullException(nameof(moralChoice));

            if (_state.branch.committed)
            {
                _log.Warn($"Rebel branch already committed to '{_state.branch.branchId}'; ignoring commit to '{branchId}'.");
                return _state.branch.branchId;
            }

            var def = _catalog.GetById(branchId);
            if (def == null)
                throw new ArgumentException($"Unknown Rebel branch id '{branchId}'.", nameof(branchId));

            var band = moralChoice.CurrentBand;
            var min = ParseBand(def.entry_band_min);
            var max = ParseBand(def.entry_band_max);
            if (band < min || band > max)
            {
                throw new InvalidOperationException(
                    $"Branch '{branchId}' requires a morality band between {min} and {max}; " +
                    $"current band is {band}. Morality is a gate, not a judgment — a different " +
                    "branch is accessible at this band.");
            }

            _state.branch.branchId = branchId;
            _state.branch.committed = true;
            OnBranchCommitted?.Invoke(branchId);
            return branchId;
        }

        /// <summary>
        /// Fires the committed branch's point-of-no-return. Irreversible: once
        /// locked, the flag is set in both the runtime IFlagLedger and the
        /// save-durable setFlags list. Locking twice is a no-op.
        /// </summary>
        public void LockPointOfNoReturn()
        {
            if (!_state.branch.committed)
                throw new InvalidOperationException("Cannot lock a point-of-no-return before a branch is committed.");
            if (_state.branch.ponrLocked) return;

            string flagId = RebelBranchIds.PonrFlagFor(_state.branch.branchId);
            _state.branch.ponrLocked = true;
            _state.branch.ponrLockedDay = _state.timeline.currentDay;
            SetDurableFlag(flagId);
            OnPonrLocked?.Invoke(flagId, _state.timeline.currentDay);
        }

        /// <summary>
        /// Shifts the Rebel faction's OWN internal alignment (not the
        /// player's MoralChoiceSystem score) toward good or evil as a
        /// consequence of the player's in-faction choices. Clamped to
        /// -200..+200, mirroring FactionStandingRecord's clamp-on-write shape.
        /// </summary>
        public void ShiftFactionAlignment(int delta)
        {
            int next = Math.Clamp(_state.rebelAlignment.alignment + delta, MinAlignment, MaxAlignment);
            _state.rebelAlignment.alignment = next;
            OnAlignmentChanged?.Invoke(next);
        }

        /// <summary>
        /// Resolves the ending for the committed, PoNR-locked branch using the
        /// player's current morality band. Idempotent: resolving twice
        /// returns the first-resolved ending.
        /// </summary>
        public string ResolveEnding(MoralChoiceSystem moralChoice)
        {
            if (moralChoice == null) throw new ArgumentNullException(nameof(moralChoice));
            if (!_state.branch.ponrLocked)
                throw new InvalidOperationException("Cannot resolve an ending before the point-of-no-return has locked the branch.");

            if (!string.IsNullOrEmpty(_state.branch.resolvedEndingId))
                return _state.branch.resolvedEndingId;

            var def = _catalog.GetById(_state.branch.branchId)
                ?? throw new InvalidOperationException($"Committed branch '{_state.branch.branchId}' is missing from the catalog.");

            var band = moralChoice.CurrentBand;
            foreach (var ending in def.endings)
            {
                var min = ParseBand(ending.band_min);
                var max = ParseBand(ending.band_max);
                if (band >= min && band <= max)
                {
                    _state.branch.resolvedEndingId = ending.ending_id;
                    OnEndingResolved?.Invoke(ending.ending_id);
                    return ending.ending_id;
                }
            }

            // No row matched (a gap in authored ranges) — fall back to the
            // middle-listed ending rather than throwing, so a data gap never
            // hard-crashes a playthrough's final resolution.
            var fallback = def.endings.Count > 0 ? def.endings[def.endings.Count / 2].ending_id : string.Empty;
            _state.branch.resolvedEndingId = fallback;
            if (!string.IsNullOrEmpty(fallback)) OnEndingResolved?.Invoke(fallback);
            return fallback;
        }

        /// <summary>
        /// Full-party-wipe loss check. Deliberately a pure function over a
        /// caller-supplied living-survivor count, identical to
        /// MilitaryBranchSystem.IsGameOver — no SurvivorRosterSystem exists
        /// in Core yet, and the game-over check is faction-agnostic, so it is
        /// not duplicated logic drift risk, just the same pure function
        /// mirrored for symmetry with the Military slice.
        /// </summary>
        public static bool IsGameOver(int livingSurvivorCount) => livingSurvivorCount <= 0;

        private void SetDurableFlag(string flagId)
        {
            _flags.Set(flagId);
            if (!_state.setFlags.Contains(flagId))
                _state.setFlags.Add(flagId);
        }

        private static MoralPathBand ParseBand(string band) => band switch
        {
            "very_evil" => MoralPathBand.VeryEvil,
            "evil" => MoralPathBand.Evil,
            "slightly_evil" => MoralPathBand.SlightlyEvil,
            "neutral" => MoralPathBand.Neutral,
            "slightly_positive" => MoralPathBand.SlightlyPositive,
            "positive" => MoralPathBand.Positive,
            "very_positive" => MoralPathBand.VeryPositive,
            _ => throw new ArgumentException($"Unknown morality band token '{band}'.", nameof(band))
        };

        public RebelBranchSystemState CaptureState() => Clone(_state);

        public void RestoreState(RebelBranchSystemState state)
        {
            if (state == null) return;
            if (!string.Equals(state.systemId, SystemId, StringComparison.Ordinal))
            {
                throw new ArgumentException(
                    $"State belongs to system '{state.systemId}', expected '{SystemId}'.", nameof(state));
            }
            if (state.schemaVersion > 1)
            {
                throw new NotSupportedException(
                    $"Future Rebel branch save schema {state.schemaVersion}; supported schema is 1.");
            }
            _state = Clone(state);

            // Replay durable flags into the runtime ledger so same-session
            // gating checks (IFlagLedger.IsSet) agree with the restored save
            // immediately — IFlagLedger itself is not persisted.
            foreach (var flagId in _state.setFlags)
                _flags.Set(flagId);
        }

        private static RebelBranchSystemState Clone(RebelBranchSystemState source)
        {
            return new RebelBranchSystemState
            {
                systemId = source.systemId,
                schemaVersion = source.schemaVersion,
                timeline = new RebelBranchTimelineState { currentDay = source.timeline.currentDay },
                branch = new RebelBranchRecord
                {
                    branchId = source.branch.branchId,
                    committed = source.branch.committed,
                    ponrLocked = source.branch.ponrLocked,
                    ponrLockedDay = source.branch.ponrLockedDay,
                    resolvedEndingId = source.branch.resolvedEndingId
                },
                rebelAlignment = new FactionAlignmentRecord
                {
                    factionId = source.rebelAlignment.factionId,
                    alignment = source.rebelAlignment.alignment
                },
                setFlags = new System.Collections.Generic.List<string>(source.setFlags ?? new System.Collections.Generic.List<string>())
            };
        }
    }
}
