using System;
using Ashfall.Core.Flags;
using Ashfall.Core.MoralChoice;

namespace Ashfall.Core.Factions
{
    /// <summary>
    /// Standing/alignment system for Peace Reputation Protection Forces
    /// (PRPF), the hidden third-power faction. Owns the player's standing
    /// toward PRPF (-100..+100, FactionWarSystem-shaped), PRPF's own internal
    /// alignment (positive-leaning by design, swayable once allied), the
    /// join/oppose gate (morality-band gated), and a deterministic daily
    /// influence tick once joined or opposed.
    ///
    /// Autonomous power-growth, chance-based hidden recruitment, and
    /// concealed HQ remain out of scope here.
    ///
    /// Zero engine dependencies; deterministic.
    /// </summary>
    public sealed class PrpfStandingSystem
    {
        public const string SystemId = "prpf_standing_system";

        public const int MinStanding = -100;
        public const int MaxStanding = 100;
        public const int HostileThreshold = -50;
        public const int AlliedThreshold = 50;

        public const int MinAlignment = -200;
        public const int MaxAlignment = 200;

        /// <summary>The player's own MoralChoiceSystem band must be at or above
        /// this to formally join PRPF — evil-leaning play locks out
        /// membership even with high standing.</summary>
        public const MoralPathBand JoinMinPlayerMoralBand = MoralPathBand.SlightlyPositive;

        private readonly IFlagLedger _flags;
        private readonly ILog _log;
        private PrpfSystemState _state;

        public event Action<int>? OnStandingChanged;
        public event Action<int>? OnAlignmentChanged;
        public event Action? OnJoined;
        public event Action? OnOpposed;

        public PrpfStandingSystem(IFlagLedger flags, PrpfSystemState? state = null, ILog? log = null)
        {
            _flags = flags ?? throw new ArgumentNullException(nameof(flags));
            _log = log ?? NullLog.Instance;
            _state = state ?? new PrpfSystemState();
            if (_state.standing == null) _state.standing = new PlayerFactionStandingRecord { factionId = PrpfIds.FactionId };
            if (_state.alignment == null)
                _state.alignment = new FactionAlignmentRecord { factionId = PrpfIds.FactionId, alignment = 120 };
        }

        public PrpfSystemState State => _state;
        public int Standing => _state.standing.standing;
        public bool IsHostile => _state.standing.isHostile;
        public bool IsAllied => _state.standing.isAllied;
        public int Alignment => _state.alignment.alignment;
        public bool IsJoined => _state.joined;
        public bool IsOpposed => _state.opposed;

        /// <summary>Mirrors FactionWarSystem.ModifyStanding's clamp-and-derive shape exactly.</summary>
        public void ModifyStanding(int delta)
        {
            int next = Math.Clamp(_state.standing.standing + delta, MinStanding, MaxStanding);
            _state.standing.standing = next;
            _state.standing.isHostile = next <= HostileThreshold;
            _state.standing.isAllied = next >= AlliedThreshold;
            OnStandingChanged?.Invoke(next);
        }

        /// <summary>
        /// Shifts PRPF's OWN internal alignment (not the player's morality
        /// score, not standing) as a consequence of the player's in-faction
        /// choices once allied. Clamped to -200..+200.
        /// </summary>
        public void ShiftFactionAlignment(int delta)
        {
            int next = Math.Clamp(_state.alignment.alignment + delta, MinAlignment, MaxAlignment);
            _state.alignment.alignment = next;
            OnAlignmentChanged?.Invoke(next);
        }

        /// <summary>
        /// Formally joins PRPF. Gated by the player's own MoralChoiceSystem
        /// band, not by standing — a hostile-standing player cannot join
        /// regardless, but a friendly-standing player who is playing too
        /// evil is refused too. Mutually exclusive with having opposed PRPF:
        /// once opposed, joining is not offered (matches the design's "join
        /// or oppose" Year-2 fork). Idempotent: joining twice is a no-op.
        /// </summary>
        public bool TryJoin(MoralChoiceSystem moralChoice)
        {
            if (moralChoice == null) throw new ArgumentNullException(nameof(moralChoice));
            if (_state.joined) return true;
            if (_state.opposed)
            {
                _log.Warn("Cannot join PRPF after committing to oppose them.");
                return false;
            }

            if (moralChoice.CurrentBand < JoinMinPlayerMoralBand)
            {
                _log.Warn($"PRPF membership requires morality band >= {JoinMinPlayerMoralBand}; " +
                          $"current band is {moralChoice.CurrentBand}. The encounter can still occur, " +
                          "but PRPF will not accept a player playing this evil.");
                return false;
            }

            _state.joined = true;
            _flags.Set(PrpfIds.FlagJoined);
            OnJoined?.Invoke();
            return true;
        }

        /// <summary>
        /// Commits the player to actively opposing PRPF instead of joining.
        /// Mutually exclusive with joining; idempotent.
        /// </summary>
        public void Oppose()
        {
            if (_state.opposed) return;
            if (_state.joined)
            {
                _log.Warn("Cannot oppose PRPF after having joined them.");
                return;
            }

            _state.opposed = true;
            _flags.Set(PrpfIds.FlagOpposed);
            OnOpposed?.Invoke();
        }

        /// <summary>
        /// Deterministic daily influence once the join/oppose fork is taken.
        /// Joined: slow positive alignment drift. Opposed: slow standing decay.
        /// No-op before commitment so passive observation does not mint loyalty.
        /// </summary>
        public void TickDay(int day)
        {
            if (day < 0) return;
            if (day <= _state.lastTickedDay) return;
            _state.lastTickedDay = day;

            if (_state.joined)
            {
                ShiftFactionAlignment(+1);
                return;
            }

            if (_state.opposed)
                ModifyStanding(-1);
        }

        public PrpfSystemState CaptureState() => Clone(_state);

        public void RestoreState(PrpfSystemState state)
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
                    $"Future PRPF save schema {state.schemaVersion}; supported schema is 1.");
            }
            _state = Clone(state);

            if (_state.joined) _flags.Set(PrpfIds.FlagJoined);
            if (_state.opposed) _flags.Set(PrpfIds.FlagOpposed);
        }

        private static PrpfSystemState Clone(PrpfSystemState source)
        {
            return new PrpfSystemState
            {
                systemId = source.systemId,
                schemaVersion = source.schemaVersion,
                standing = new PlayerFactionStandingRecord
                {
                    factionId = source.standing.factionId,
                    standing = source.standing.standing,
                    isHostile = source.standing.isHostile,
                    isAllied = source.standing.isAllied
                },
                alignment = new FactionAlignmentRecord
                {
                    factionId = source.alignment.factionId,
                    alignment = source.alignment.alignment
                },
                joined = source.joined,
                opposed = source.opposed,
                lastTickedDay = source.lastTickedDay
            };
        }
    }
}
