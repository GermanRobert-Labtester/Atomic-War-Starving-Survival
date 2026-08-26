using System;
using Ashfall.Core.Flags;
using Ashfall.Core.MoralChoice;

namespace Ashfall.Core.Factions
{
    /// <summary>
    /// Resolution engine for the Independent faction slice of "The Weight of
    /// Choices" branching system. Reuses the branch-commit / PoNR-lock /
    /// ending-resolution lifecycle shape from MilitaryBranchSystem and
    /// RebelBranchSystem, but the gating logic is genuinely different:
    ///
    /// - CommitBranch still checks the player's MoralChoiceSystem band
    ///   (same as Military/Rebel), but ALSO checks two gate types neither of
    ///   those systems has: a minimum PRPF standing (IND-3
    ///   Peacekeeper/Diplomat — "the faction is a positive leveled faction",
    ///   reachable through Independent without ever touching Military or
    ///   Rebel, i.e. a cold join path already possible via
    ///   PrpfStandingSystem.TryJoin with zero branch commitment) and a dual
    ///   hostile-to-both-factions requirement (IND-4 Exile — "hostile
    ///   towards all of them" made concrete as one specific branch rather
    ///   than a generic mechanic every branch shares).
    /// - There is no ShiftFactionAlignment here — Independent has no faction
    ///   of its own to sway. ModifyMilitaryStanding/ModifyRebelStanding
    ///   exist instead, tracking the player's relationship to the two
    ///   factions they never joined (a value those factions' own systems
    ///   have no reason to track for an uncommitted player).
    ///
    /// Zero engine dependencies; deterministic; runs on its own local day
    /// counter, independent of every other expansion's and every other
    /// branch system's clock.
    /// </summary>
    public sealed class IndependentBranchSystem
    {
        public const string SystemId = "independent_branch_system";

        public const int MinStanding = -100;
        public const int MaxStanding = 100;
        public const int HostileThreshold = -50;
        public const int AlliedThreshold = 50;

        private readonly IndependentBranchCatalog _catalog;
        private readonly IFlagLedger _flags;
        private readonly ILog _log;
        private IndependentBranchSystemState _state;

        public event Action<string>? OnBranchCommitted;
        public event Action<string, int>? OnPonrLocked;
        public event Action<string>? OnEndingResolved;
        public event Action<int>? OnMilitaryStandingChanged;
        public event Action<int>? OnRebelStandingChanged;

        public IndependentBranchSystem(IndependentBranchCatalog catalog, IFlagLedger flags, IndependentBranchSystemState? state = null, ILog? log = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _flags = flags ?? throw new ArgumentNullException(nameof(flags));
            _log = log ?? NullLog.Instance;
            _state = state ?? new IndependentBranchSystemState();
            if (_state.timeline == null) _state.timeline = new IndependentBranchTimelineState();
            if (_state.branch == null) _state.branch = new IndependentBranchRecord();
            if (_state.militaryStanding == null)
                _state.militaryStanding = new PlayerFactionStandingRecord { factionId = MilitaryBranchIds.FactionId };
            if (_state.rebelStanding == null)
                _state.rebelStanding = new PlayerFactionStandingRecord { factionId = RebelBranchIds.FactionId };
            if (_state.setFlags == null) _state.setFlags = new System.Collections.Generic.List<string>();
        }

        public IndependentBranchSystemState State => _state;
        public int CurrentDay => _state.timeline.currentDay;
        public string? CommittedBranchId => string.IsNullOrEmpty(_state.branch.branchId) ? null : _state.branch.branchId;
        public bool IsPonrLocked => _state.branch.ponrLocked;
        public int MilitaryStanding => _state.militaryStanding.standing;
        public int RebelStanding => _state.rebelStanding.standing;
        public bool IsHostileToMilitary => _state.militaryStanding.isHostile;
        public bool IsHostileToRebel => _state.rebelStanding.isHostile;
        public string? ResolvedEndingId => string.IsNullOrEmpty(_state.branch.resolvedEndingId) ? null : _state.branch.resolvedEndingId;

        public void AdvanceDay(int day)
        {
            if (day < 0) throw new ArgumentOutOfRangeException(nameof(day));
            if (day < _state.timeline.currentDay) return; // never rewind
            _state.timeline.currentDay = day;
        }

        /// <summary>Mirrors FactionWarSystem.ModifyStanding's clamp-and-derive shape for the
        /// player's relationship to Military, tracked here because MilitaryBranchSystem
        /// only models a player who committed to it.</summary>
        public void ModifyMilitaryStanding(int delta)
        {
            int next = Math.Clamp(_state.militaryStanding.standing + delta, MinStanding, MaxStanding);
            _state.militaryStanding.standing = next;
            _state.militaryStanding.isHostile = next <= HostileThreshold;
            _state.militaryStanding.isAllied = next >= AlliedThreshold;
            OnMilitaryStandingChanged?.Invoke(next);
        }

        /// <summary>Same as ModifyMilitaryStanding, for Rebel.</summary>
        public void ModifyRebelStanding(int delta)
        {
            int next = Math.Clamp(_state.rebelStanding.standing + delta, MinStanding, MaxStanding);
            _state.rebelStanding.standing = next;
            _state.rebelStanding.isHostile = next <= HostileThreshold;
            _state.rebelStanding.isAllied = next >= AlliedThreshold;
            OnRebelStandingChanged?.Invoke(next);
        }

        /// <summary>
        /// Commits the player to a base Independent branch. Checks the same
        /// morality-band soft gate as Military/Rebel, plus any of this
        /// branch's Independent-specific gates: a minimum PRPF standing
        /// (checked only if the branch declares requires_prpf_standing_min
        /// AND a PrpfStandingSystem is supplied — passing null when a branch
        /// needs the check throws rather than silently skipping it) and a
        /// dual hostile-to-both-factions requirement read from this system's
        /// own militaryStanding/rebelStanding records.
        /// </summary>
        public string CommitBranch(string branchId, MoralChoiceSystem moralChoice, PrpfStandingSystem? prpf = null)
        {
            if (string.IsNullOrEmpty(branchId)) throw new ArgumentNullException(nameof(branchId));
            if (moralChoice == null) throw new ArgumentNullException(nameof(moralChoice));

            if (_state.branch.committed)
            {
                _log.Warn($"Independent branch already committed to '{_state.branch.branchId}'; ignoring commit to '{branchId}'.");
                return _state.branch.branchId;
            }

            var def = _catalog.GetById(branchId);
            if (def == null)
                throw new ArgumentException($"Unknown Independent branch id '{branchId}'.", nameof(branchId));

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

            if (def.requires_prpf_standing_min.HasValue)
            {
                if (prpf == null)
                {
                    throw new InvalidOperationException(
                        $"Branch '{branchId}' requires a minimum PRPF standing of {def.requires_prpf_standing_min.Value}, " +
                        "but no PrpfStandingSystem was supplied to evaluate it.");
                }
                if (prpf.Standing < def.requires_prpf_standing_min.Value)
                {
                    throw new InvalidOperationException(
                        $"Branch '{branchId}' requires PRPF standing >= {def.requires_prpf_standing_min.Value}; " +
                        $"current standing is {prpf.Standing}.");
                }
            }

            if (def.requires_hostile_to_military == true && !IsHostileToMilitary)
            {
                throw new InvalidOperationException(
                    $"Branch '{branchId}' requires hostile standing toward Military " +
                    $"(<= {HostileThreshold}); current standing is {MilitaryStanding}.");
            }

            if (def.requires_hostile_to_rebel == true && !IsHostileToRebel)
            {
                throw new InvalidOperationException(
                    $"Branch '{branchId}' requires hostile standing toward Rebel " +
                    $"(<= {HostileThreshold}); current standing is {RebelStanding}.");
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

            string flagId = IndependentBranchIds.PonrFlagFor(_state.branch.branchId);
            _state.branch.ponrLocked = true;
            _state.branch.ponrLockedDay = _state.timeline.currentDay;
            SetDurableFlag(flagId);
            OnPonrLocked?.Invoke(flagId, _state.timeline.currentDay);
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

            var fallback = def.endings.Count > 0 ? def.endings[def.endings.Count / 2].ending_id : string.Empty;
            _state.branch.resolvedEndingId = fallback;
            if (!string.IsNullOrEmpty(fallback)) OnEndingResolved?.Invoke(fallback);
            return fallback;
        }

        /// <summary>Full-party-wipe loss check, identical to Military/Rebel's IsGameOver.</summary>
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

        public IndependentBranchSystemState CaptureState() => Clone(_state);

        public void RestoreState(IndependentBranchSystemState state)
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
                    $"Future Independent branch save schema {state.schemaVersion}; supported schema is 1.");
            }
            _state = Clone(state);

            foreach (var flagId in _state.setFlags)
                _flags.Set(flagId);
        }

        private static IndependentBranchSystemState Clone(IndependentBranchSystemState source)
        {
            return new IndependentBranchSystemState
            {
                systemId = source.systemId,
                schemaVersion = source.schemaVersion,
                timeline = new IndependentBranchTimelineState { currentDay = source.timeline.currentDay },
                branch = new IndependentBranchRecord
                {
                    branchId = source.branch.branchId,
                    committed = source.branch.committed,
                    ponrLocked = source.branch.ponrLocked,
                    ponrLockedDay = source.branch.ponrLockedDay,
                    resolvedEndingId = source.branch.resolvedEndingId
                },
                militaryStanding = new PlayerFactionStandingRecord
                {
                    factionId = source.militaryStanding.factionId,
                    standing = source.militaryStanding.standing,
                    isHostile = source.militaryStanding.isHostile,
                    isAllied = source.militaryStanding.isAllied
                },
                rebelStanding = new PlayerFactionStandingRecord
                {
                    factionId = source.rebelStanding.factionId,
                    standing = source.rebelStanding.standing,
                    isHostile = source.rebelStanding.isHostile,
                    isAllied = source.rebelStanding.isAllied
                },
                setFlags = new System.Collections.Generic.List<string>(source.setFlags ?? new System.Collections.Generic.List<string>())
            };
        }
    }
}
