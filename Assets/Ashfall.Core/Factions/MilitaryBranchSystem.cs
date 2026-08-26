using System;
using Ashfall.Core.Flags;
using Ashfall.Core.MoralChoice;

namespace Ashfall.Core.Factions
{
    /// <summary>
    /// Resolution engine for the Military faction slice of "The Weight of
    /// Choices" branching system.
    ///
    /// Deliberately does NOT own morality or faction-standing math: it reads
    /// MoralChoiceSystem.CurrentBand for the player's morality axis (morality
    /// is a gate key here, never a "correct/incorrect" judgment — evil-leaning
    /// and positive-leaning play lock out different, equally valid quest
    /// pools and endings) and it can read player-relationship standing from a
    /// caller-supplied FactionWarSystem-shaped source when that wiring exists.
    /// What this system DOES own: which base branch (of the 8 Military
    /// branches) the player has committed to, the branch's point-of-no-return
    /// lock, the Military faction's own internal alignment (a value distinct
    /// from the player's morality score, per design: "the player has the
    /// chance to sway the faction to the positive side"), and morality-gated
    /// ending resolution once the branch is locked.
    ///
    /// Zero engine dependencies; deterministic; runs on its own local day
    /// counter (MilitaryBranchTimelineState), independent of the global
    /// IClock and of Year of Ash's day 180-360 window.
    /// </summary>
    public sealed class MilitaryBranchSystem
    {
        public const string SystemId = "military_branch_system";

        /// <summary>Faction alignment is clamped to the same -200..+200 range as
        /// MoralChoiceSystem purely for player-facing scale consistency.</summary>
        public const int MinAlignment = -200;
        public const int MaxAlignment = 200;

        private readonly MilitaryBranchCatalog _catalog;
        private readonly IFlagLedger _flags;
        private readonly ILog _log;
        private MilitaryBranchSystemState _state;

        public event Action<string>? OnBranchCommitted;
        public event Action<string, int>? OnPonrLocked;
        public event Action<string>? OnEndingResolved;
        public event Action<int>? OnAlignmentChanged;

        public MilitaryBranchSystem(MilitaryBranchCatalog catalog, IFlagLedger flags, MilitaryBranchSystemState? state = null, ILog? log = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _flags = flags ?? throw new ArgumentNullException(nameof(flags));
            _log = log ?? NullLog.Instance;
            _state = state ?? new MilitaryBranchSystemState();
            if (_state.timeline == null) _state.timeline = new MilitaryBranchTimelineState();
            if (_state.branch == null) _state.branch = new MilitaryBranchRecord();
            if (_state.militaryAlignment == null)
                _state.militaryAlignment = new FactionAlignmentRecord { factionId = MilitaryBranchIds.FactionId, alignment = -80 };
            if (_state.setFlags == null) _state.setFlags = new System.Collections.Generic.List<string>();
        }

        public MilitaryBranchSystemState State => _state;
        public int CurrentDay => _state.timeline.currentDay;
        public string? CommittedBranchId => string.IsNullOrEmpty(_state.branch.branchId) ? null : _state.branch.branchId;
        public bool IsPonrLocked => _state.branch.ponrLocked;
        public int MilitaryAlignment => _state.militaryAlignment.alignment;
        public string? ResolvedEndingId => string.IsNullOrEmpty(_state.branch.resolvedEndingId) ? null : _state.branch.resolvedEndingId;

        public void AdvanceDay(int day)
        {
            if (day < 0) throw new ArgumentOutOfRangeException(nameof(day));
            if (day < _state.timeline.currentDay) return; // never rewind
            _state.timeline.currentDay = day;
        }

        /// <summary>
        /// Commits the player to a base branch. Soft-gated by the branch's
        /// entry band range read from the catalog (a design "starting
        /// morality range" is an entry gate, never a re-definition of the
        /// morality tiers themselves) — attempting to commit outside that
        /// range throws, since the design doc treats these as hard entry
        /// requirements, not just flavor. Committing twice is a no-op that
        /// returns the already-committed branch (first commit wins, matching
        /// MoralChoiceSystem.Resolve's "one resolution per quest" idempotency
        /// convention) rather than silently overwriting it.
        /// </summary>
        public string CommitBranch(string branchId, MoralChoiceSystem moralChoice)
        {
            if (string.IsNullOrEmpty(branchId)) throw new ArgumentNullException(nameof(branchId));
            if (moralChoice == null) throw new ArgumentNullException(nameof(moralChoice));

            if (_state.branch.committed)
            {
                _log.Warn($"Military branch already committed to '{_state.branch.branchId}'; ignoring commit to '{branchId}'.");
                return _state.branch.branchId;
            }

            var def = _catalog.GetById(branchId);
            if (def == null)
                throw new ArgumentException($"Unknown Military branch id '{branchId}'.", nameof(branchId));

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
        /// locked, the flag is set in both the runtime IFlagLedger (for
        /// same-session gating checks elsewhere) and the save-durable
        /// setFlags list (since IFlagLedger itself is not persisted). Locking
        /// twice is a no-op.
        /// </summary>
        public void LockPointOfNoReturn()
        {
            if (!_state.branch.committed)
                throw new InvalidOperationException("Cannot lock a point-of-no-return before a branch is committed.");
            if (_state.branch.ponrLocked) return;

            string flagId = MilitaryBranchIds.PonrFlagFor(_state.branch.branchId);
            _state.branch.ponrLocked = true;
            _state.branch.ponrLockedDay = _state.timeline.currentDay;
            SetDurableFlag(flagId);
            OnPonrLocked?.Invoke(flagId, _state.timeline.currentDay);
        }

        /// <summary>
        /// Shifts the Military faction's OWN internal alignment (not the
        /// player's MoralChoiceSystem score) toward good or evil as a
        /// consequence of the player's in-faction choices. Clamped to
        /// -200..+200, mirroring FactionStandingRecord's clamp-on-write shape.
        /// </summary>
        public void ShiftFactionAlignment(int delta)
        {
            int next = Math.Clamp(_state.militaryAlignment.alignment + delta, MinAlignment, MaxAlignment);
            _state.militaryAlignment.alignment = next;
            OnAlignmentChanged?.Invoke(next);
        }

        /// <summary>
        /// Resolves the ending for the committed, PoNR-locked branch using the
        /// player's current morality band. Each branch's ending table is
        /// checked in catalog order; the first row whose band_min..band_max
        /// contains the current band wins. Idempotent: resolving twice
        /// returns the first-resolved ending, matching the "PoNR locks the
        /// path" design intent — the ending is not re-rolled if morality
        /// keeps drifting after the branch is already locked.
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
        /// caller-supplied living-survivor count rather than an owned roster
        /// — no SurvivorRosterSystem exists in Core yet (confirmed absent),
        /// and building one is out of scope for the Military branch slice.
        /// Mirrors EpilogueEvaluationContext's caller-supplied-count pattern.
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

        public MilitaryBranchSystemState CaptureState() => Clone(_state);

        public void RestoreState(MilitaryBranchSystemState state)
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
                    $"Future Military branch save schema {state.schemaVersion}; supported schema is 1.");
            }
            _state = Clone(state);

            // Replay durable flags into the runtime ledger so same-session
            // gating checks (IFlagLedger.IsSet) agree with the restored save
            // immediately — IFlagLedger itself is not persisted.
            foreach (var flagId in _state.setFlags)
                _flags.Set(flagId);
        }

        private static MilitaryBranchSystemState Clone(MilitaryBranchSystemState source)
        {
            return new MilitaryBranchSystemState
            {
                systemId = source.systemId,
                schemaVersion = source.schemaVersion,
                timeline = new MilitaryBranchTimelineState { currentDay = source.timeline.currentDay },
                branch = new MilitaryBranchRecord
                {
                    branchId = source.branch.branchId,
                    committed = source.branch.committed,
                    ponrLocked = source.branch.ponrLocked,
                    ponrLockedDay = source.branch.ponrLockedDay,
                    resolvedEndingId = source.branch.resolvedEndingId
                },
                militaryAlignment = new FactionAlignmentRecord
                {
                    factionId = source.militaryAlignment.factionId,
                    alignment = source.militaryAlignment.alignment
                },
                setFlags = new System.Collections.Generic.List<string>(source.setFlags ?? new System.Collections.Generic.List<string>())
            };
        }
    }
}
