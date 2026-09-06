using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// ASHFALL Power Grid — Core system (item 13).
    ///
    /// Single authority for shelter electrical state. The Core owns the
    /// deterministic math (generation, draw, battery reserve, brownout
    /// effects); the host presents a panel and applies the consequences
    /// to air filtration, water, clinic, greenhouse, foundry, and lighting
    /// via adapters.
    ///
    /// State is captured/restored through <see cref="PowerGridState"/> and
    /// the envelope in <see cref="PowerGridSave"/>. Every mutation emits a
    /// typed <see cref="PowerGridEvent"/> that the host listens to.
    /// </summary>
    public sealed class PowerGridSystem
    {
        private readonly PowerGridState _state;
        private readonly List<PowerGridRoom> _rooms;
        private readonly ISeededRng _rng;
        // Runtime generation sources are projections from their owning systems.
        // They are deliberately not persisted here: the owning save section
        // restores first, then the host republishes the contribution.
        private readonly Dictionary<string, float> _generationContributions =
            new Dictionary<string, float>(StringComparer.Ordinal);

        /// <summary>Raised whenever a room's powered state changes.</summary>
        public event Action<PowerGridEvent>? OnPowerChanged;

        /// <summary>Raised at end of every tick with the day summary.</summary>
        public event Action<PowerGridTickSummary>? OnTickSummary;

        public PowerGridSystem(PowerGridState state, IEnumerable<PowerGridRoom> rooms, ISeededRng rng)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
            if (rooms == null) throw new ArgumentNullException(nameof(rooms));
            _rooms = new List<PowerGridRoom>();
            foreach (var r in rooms)
            {
                if (r == null || string.IsNullOrEmpty(r.RoomId)) continue;
                _rooms.Add(r);
            }
            if (_rooms.Count == 0)
                throw new InvalidOperationException("PowerGridSystem: at least one room required.");
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _state.NormalizeAndValidate(_rooms);
        }

        public PowerGridState State => _state;
        public IReadOnlyList<PowerGridRoom> Rooms => _rooms;

        /// <summary>
        /// Base generator output plus current runtime contributions (kW is
        /// represented as watts at this authority). External contributions are
        /// fuel-free; the base generator remains the only fuel-burning source.
        /// </summary>
        public float GenerationWatts
        {
            get
            {
                float total = _state.GenerationWatts;
                foreach (var contribution in _generationContributions.Values)
                    total += Math.Max(0f, contribution);
                return total;
            }
        }

        public float BaseGenerationWatts => _state.GenerationWatts;
        public IReadOnlyDictionary<string, float> GenerationContributions => _generationContributions;
        public float FuelUnits => _state.FuelUnits;
        public float BatteryReserveWh => _state.BatteryReserveWh;
        public float BatteryCapacityWh => _state.BatteryCapacityWh;
        public float TotalDrawWatts => ComputeTotalDraw();
        public float NetWatts => GenerationWatts - TotalDrawWatts;
        public bool IsBrownout => TotalDrawWatts > GenerationWatts && BatteryReserveWh <= 0;

        /// <summary>
        /// Publish one external generation source. The source ID is stable and
        /// replacing a value is idempotent, so a host can republish after every
        /// campaign tick without accumulating duplicate output.
        /// </summary>
        public bool SetGenerationContribution(string sourceId, float watts)
        {
            if (string.IsNullOrWhiteSpace(sourceId)) return false;
            if (float.IsNaN(watts) || float.IsInfinity(watts)) return false;
            float normalized = Math.Max(0f, watts);
            if (normalized <= 0f)
                _generationContributions.Remove(sourceId);
            else
                _generationContributions[sourceId] = normalized;

            OnPowerChanged?.Invoke(new PowerGridEvent(
                PowerGridEventKind.GenerationChanged,
                sourceId,
                _state.SimDay,
                normalized > 0f ? "generation_contribution_set" : "generation_contribution_removed",
                normalized));
            return true;
        }

        public bool RemoveGenerationContribution(string sourceId)
        {
            if (string.IsNullOrWhiteSpace(sourceId)) return false;
            return SetGenerationContribution(sourceId, 0f);
        }

        public PowerGridSnapshot Snapshot()
        {
            var snapshot = new PowerGridSnapshot
            {
                Day = _state.SimDay,
                GenerationWatts = GenerationWatts,
                FuelUnits = FuelUnits,
                BatteryReserveWh = BatteryReserveWh,
                BatteryCapacityWh = BatteryCapacityWh,
                TotalDrawWatts = TotalDrawWatts,
                NetWatts = NetWatts,
                IsBrownout = IsBrownout,
                RoomIds = new List<string>(RoomPoweredStates())
            };
            foreach (var contribution in _generationContributions)
                snapshot.GenerationContributions[contribution.Key] = contribution.Value;
            return snapshot;
        }

        public bool IsRoomPowered(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return false;
            var r = FindRoom(roomId);
            if (r == null) return false;
            return _state.IsBreakerClosed(roomId) && !_state.IsRoomTripped(roomId) &&
                   !IsBrownout;
        }

        public PowerGridRoomPriority EffectivePriority(string roomId)
        {
            var r = FindRoom(roomId);
            if (r == null) return PowerGridRoomPriority.Disabled;
            return _state.GetRoomPriority(roomId);
        }

        /// <summary>
        /// Toggle a breaker. Returns false if the room id is unknown.
        /// Idempotent: re-closing an already-closed breaker is a no-op.
        /// </summary>
        public bool ToggleBreaker(string roomId)
        {
            var r = FindRoom(roomId);
            if (r == null) return false;
            bool wasClosed = _state.IsBreakerClosed(roomId);
            _state.SetBreaker(roomId, !wasClosed);
            OnPowerChanged?.Invoke(new PowerGridEvent(PowerGridEventKind.BreakerToggled,
                roomId, _state.SimDay, wasClosed ? "closed_to_open" : "open_to_closed"));
            return true;
        }

        public bool SetBreaker(string roomId, bool closed)
        {
            var r = FindRoom(roomId);
            if (r == null) return false;
            bool wasClosed = _state.IsBreakerClosed(roomId);
            if (wasClosed == closed) return true;
            _state.SetBreaker(roomId, closed);
            OnPowerChanged?.Invoke(new PowerGridEvent(PowerGridEventKind.BreakerToggled,
                roomId, _state.SimDay, closed ? "open_to_closed" : "closed_to_open"));
            return true;
        }

        public void MarkTripped(string roomId, int day)
        {
            _state.MarkTripped(roomId, day);
            OnPowerChanged?.Invoke(new PowerGridEvent(PowerGridEventKind.Tripped,
                roomId, day, "manual_trip"));
        }

        public void ClearTripped(string roomId) => _state.ClearTripped(roomId);

        public bool IsRoomTripped(string roomId) => _state.IsRoomTripped(roomId);

        public bool SetPriority(string roomId, PowerGridRoomPriority priority)
        {
            var r = FindRoom(roomId);
            if (r == null) return false;
            _state.SetRoomPriority(roomId, priority);
            OnPowerChanged?.Invoke(new PowerGridEvent(PowerGridEventKind.PriorityChanged,
                roomId, _state.SimDay, priority.ToString()));
            return true;
        }

        public void AddFuel(float units)
        {
            if (units <= 0) return;
            _state.FuelUnits += units;
            OnPowerChanged?.Invoke(new PowerGridEvent(PowerGridEventKind.FuelAdded, null!,
                _state.SimDay, "fuel_added", units));
        }

        /// <summary>EMP storm severity applied on weather onset (balance constant;</summary>
        /// orbital surges are data-driven via OrbitalImpactReport.PowerGridDisruption).
        public const float EmpStormSurgeSeverity = 0.6f;

        /// <summary>Fraction of battery capacity drained at full surge severity.</summary>
        public const float SurgeBatteryDrainFraction = 0.15f;

        /// <summary>
        /// Apply an external electrical surge (EMP storm onset, orbital impact).
        /// Deterministic: trips the lowest-priority non-tripped circuits first
        /// (priority tier ascending, then RoomId ordinal), draining the battery
        /// proportionally to severity. Critical rooms are exempt below 0.9
        /// severity so a surge can wound the grid without destroying it.
        /// Deduped per day: a second surge event on the same day is a no-op.
        /// The surge day persists via <see cref="PowerGridState.LastSurgeDay"/>.
        /// </summary>
        public IReadOnlyList<string> ApplySurgeDay(int day, float severity01)
        {
            float severity = Math.Clamp(severity01, 0f, 1f);
            if (severity <= 0f) return Array.Empty<string>();
            if (day <= _state.LastSurgeDay) return Array.Empty<string>();

            bool criticalEligible = severity >= 0.9f;
            var candidates = new List<(string RoomId, int Tier)>();
            foreach (var r in _rooms)
            {
                if (_state.IsRoomTripped(r.RoomId)) continue;
                // Effective tier: player override wins; catalog default otherwise.
                bool hasOverride = false;
                for (int p = 0; p < _state.Priorities.Count; p++)
                {
                    if (_state.Priorities[p].RoomId == r.RoomId) { hasOverride = true; break; }
                }
                var tier = (int)(hasOverride
                    ? _state.GetRoomPriority(r.RoomId)
                    : r.DefaultPriority);
                if (tier == (int)PowerGridRoomPriority.Disabled) continue;
                if (tier == (int)PowerGridRoomPriority.Critical && !criticalEligible) continue;
                candidates.Add((r.RoomId, tier));
            }
            candidates.Sort(static (a, b) =>
            {
                int byTier = a.Tier.CompareTo(b.Tier);
                return byTier != 0 ? byTier : string.CompareOrdinal(a.RoomId, b.RoomId);
            });

            int tripCount = (int)(severity * candidates.Count);
            var tripped = new List<string>(tripCount);
            for (int i = 0; i < tripCount; i++)
            {
                _state.MarkTripped(candidates[i].RoomId, day);
                tripped.Add(candidates[i].RoomId);
            }

            float drain = (float)Math.Floor(_state.BatteryCapacityWh * SurgeBatteryDrainFraction * severity);
            _state.BatteryReserveWh = Math.Max(0f, _state.BatteryReserveWh - drain);
            _state.LastSurgeDay = day;

            OnPowerChanged?.Invoke(new PowerGridEvent(PowerGridEventKind.SurgeApplied,
                tripped.Count > 0 ? string.Join(",", tripped) : "none",
                day, "surge_applied", severity));
            return tripped;
        }

        /// <summary>
        /// Tick one full day. Deterministic: given the same fuel/battery state
        /// and RNG, the result is identical across hosts and runs.
        /// </summary>
        public PowerGridTickSummary TickDay(int day, ISeededRng tickRng)
        {
            var rng = tickRng ?? _rng;
            _state.SimDay = day;
            float draw = ComputeTotalDraw();
            float gen = GenerationWatts;
            float net = gen - draw; // Wh per hour assumed; full day = 24 units
            float fuelConsumed = 0f;
            float brownoutHours = 0f;

            // Burn fuel proportional to generation.
            float externalGeneration = 0f;
            foreach (var contribution in _generationContributions.Values)
                externalGeneration += Math.Max(0f, contribution);
            float fuelNeed = Math.Max(0f, gen - externalGeneration) * 24f * 0.001f;
            if (_state.FuelUnits >= fuelNeed)
            {
                _state.FuelUnits -= fuelNeed;
                fuelConsumed = fuelNeed;
            }
            else
            {
                fuelConsumed = _state.FuelUnits;
                _state.FuelUnits = 0;
                gen *= 0.5f; // partial generation when fuel-starved.
            }

            if (net >= 0)
            {
                float spareWh = net * 24f;
                _state.BatteryReserveWh = Math.Min(_state.BatteryCapacityWh,
                    _state.BatteryReserveWh + spareWh);
            }
            else
            {
                float demandWh = -net * 24f;
                if (_state.BatteryReserveWh >= demandWh)
                {
                    _state.BatteryReserveWh -= demandWh;
                }
                else
                {
                    float unmet = demandWh - _state.BatteryReserveWh;
                    _state.BatteryReserveWh = 0;
                    brownoutHours = Math.Min(24f, unmet / Math.Max(1f, draw));
                }
            }

            // Random load spike (deterministic via injected rng).
            if (rng.NextDouble() < 0.05 && _state.BatteryReserveWh > 0)
            {
                float spike = (float)(rng.NextDouble() * 30.0);
                _state.BatteryReserveWh = Math.Max(0, _state.BatteryReserveWh - spike);
                brownoutHours += 0.5f;
            }

            // Trip breakers that overload for more than 4 hours of brownout.
            if (brownoutHours >= 4f)
            {
                foreach (var r in _rooms)
                {
                    if (_state.GetRoomPriority(r.RoomId) == PowerGridRoomPriority.Disabled)
                        continue;
                    if (!_state.IsBreakerClosed(r.RoomId)) continue;
                    if (rng.NextDouble() < 0.10)
                    {
                        _state.MarkTripped(r.RoomId, day);
                        OnPowerChanged?.Invoke(new PowerGridEvent(PowerGridEventKind.Tripped,
                            r.RoomId, day, "brownout_overload"));
                    }
                }
            }

            var summary = new PowerGridTickSummary
            {
                Day = day,
                FuelConsumed = fuelConsumed,
                BatteryEndWh = _state.BatteryReserveWh,
                BrownoutHours = brownoutHours,
                IsBrownout = IsBrownout
            };
            OnTickSummary?.Invoke(summary);
            return summary;
        }

        public PowerGridState CaptureState() => _state.Capture();

        public void RestoreState(PowerGridState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            _state.RestoreInto(state, _rooms);
        }

        private PowerGridRoom? FindRoom(string roomId)
        {
            for (int i = 0; i < _rooms.Count; i++)
                if (_rooms[i].RoomId == roomId) return _rooms[i];
            return null;
        }

        private float ComputeTotalDraw()
        {
            // Phase 6: this now returns the *intent* draw — the sum of all rooms'
            // logical DrawWatts that are eligible (closed breaker, not tripped,
            // not disabled). The brownout suppression number lives on the new
            // EffectiveTotalDrawWatts property. Splitting intent from effect
            // stops IsBrownout from flipping false the same tick brownout fires:
            // a 0-returning TotalDrawWatts made NetWatts positive, which made
            // the battery charge UP during brownout (opposite of intent).
            float draw = 0f;
            for (int i = 0; i < _rooms.Count; i++)
            {
                var r = _rooms[i];
                if (!_state.IsBreakerClosed(r.RoomId)) continue;
                if (_state.IsRoomTripped(r.RoomId)) continue;
                var pri = _state.GetRoomPriority(r.RoomId);
                if (pri == PowerGridRoomPriority.Disabled) continue;
                draw += r.DrawWatts;
            }
            return draw;
        }

        /// <summary>
        /// Number that actually drains the battery during a brownout. When the
        /// grid is in brownout state, every room drops to 0 effective draw,
        /// which means the unmet-deficit (and thus the brownout-hours
        /// calculation) collapses to whatever the *previous-tick* steady-state
        /// looked like. Returns 0 during brownout to preserve the legacy
        /// TickDay brownout-hours math that downstream code relies on.
        /// </summary>
        public float EffectiveTotalDrawWatts =>
            IsBrownout ? 0f : TotalDrawWatts;

        private List<string> RoomPoweredStates()
        {
            var list = new List<string>(_rooms.Count);
            for (int i = 0; i < _rooms.Count; i++)
            {
                var r = _rooms[i];
                bool powered = IsRoomPowered(r.RoomId);
                list.Add(r.RoomId + "=" + (powered ? "on" : "off"));
            }
            return list;
        }
    }

    /// <summary>Priority used by the grid when total draw exceeds generation.</summary>
    public enum PowerGridRoomPriority
    {
        Disabled = 0,
        Low = 1,
        Standard = 2,
        Critical = 3
    }

    [Serializable]
    public sealed class PowerGridRoom
    {
        public string RoomId;
        public string DisplayName;
        public float DrawWatts;
        public PowerGridRoomPriority DefaultPriority;
        public string FailureEffectId; // semantic id the host looks up.

        public PowerGridRoom() { }

        public PowerGridRoom(string roomId, string displayName, float drawWatts,
            PowerGridRoomPriority defaultPriority = PowerGridRoomPriority.Standard,
string? failureEffectId = null)
        {
            RoomId = roomId;
            DisplayName = displayName;
            DrawWatts = drawWatts;
            DefaultPriority = defaultPriority;
            FailureEffectId = failureEffectId;
        }
    }

    [Serializable]
    public sealed class PowerGridState
    {
        public int SimDay;
        public float GenerationWatts;
        public float FuelUnits;
        public float BatteryReserveWh;
        public float BatteryCapacityWh;
        public List<string> ClosedBreakers = new List<string>();
        public List<string> TrippedRooms = new List<string>();
        public List<RoomPriorityRecord> Priorities = new List<RoomPriorityRecord>();

        /// <summary>
        /// Day of the last applied surge (EMP/orbital). Optional field: saves
        /// written before the field existed restore as 0 ("no surge yet") per
        /// the repository's optional-field migration tolerance.
        /// </summary>
        public int LastSurgeDay;

        public bool IsBreakerClosed(string roomId) => !ClosedBreakers.Contains(roomId);
        public bool IsRoomTripped(string roomId) => TrippedRooms.Contains(roomId);

        public void SetBreaker(string roomId, bool closed)
        {
            if (closed) ClosedBreakers.Remove(roomId);
            else if (!ClosedBreakers.Contains(roomId)) ClosedBreakers.Add(roomId);
        }

        public void MarkTripped(string roomId, int day)
        {
            if (!TrippedRooms.Contains(roomId)) TrippedRooms.Add(roomId);
        }

        public void ClearTripped(string roomId) => TrippedRooms.Remove(roomId);

        public PowerGridRoomPriority GetRoomPriority(string roomId)
        {
            for (int i = 0; i < Priorities.Count; i++)
                if (Priorities[i].RoomId == roomId) return Priorities[i].Priority;
            return PowerGridRoomPriority.Standard;
        }

        public void SetRoomPriority(string roomId, PowerGridRoomPriority priority)
        {
            for (int i = 0; i < Priorities.Count; i++)
            {
                if (Priorities[i].RoomId == roomId)
                {
                    Priorities[i].Priority = priority;
                    return;
                }
            }
            Priorities.Add(new RoomPriorityRecord { RoomId = roomId, Priority = priority });
        }

        public void NormalizeAndValidate(IReadOnlyList<PowerGridRoom> rooms)
        {
            if (BatteryCapacityWh < 0) BatteryCapacityWh = 0;
            if (BatteryReserveWh < 0) BatteryReserveWh = 0;
            if (BatteryReserveWh > BatteryCapacityWh) BatteryReserveWh = BatteryCapacityWh;
            if (FuelUnits < 0) FuelUnits = 0;
            if (GenerationWatts < 0) GenerationWatts = 0;
            var validIds = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < rooms.Count; i++) validIds.Add(rooms[i].RoomId);

            for (int i = ClosedBreakers.Count - 1; i >= 0; i--)
                if (!validIds.Contains(ClosedBreakers[i])) ClosedBreakers.RemoveAt(i);
            for (int i = TrippedRooms.Count - 1; i >= 0; i--)
                if (!validIds.Contains(TrippedRooms[i])) TrippedRooms.RemoveAt(i);
            for (int i = Priorities.Count - 1; i >= 0; i--)
                if (!validIds.Contains(Priorities[i].RoomId)) Priorities.RemoveAt(i);

            // Default breakers: every room starts closed unless explicitly opened.
            for (int i = 0; i < rooms.Count; i++)
            {
                if (!validIds.Contains(rooms[i].RoomId)) continue;
                if (!ClosedBreakers.Contains(rooms[i].RoomId) &&
                    !IsBreakerClosed(rooms[i].RoomId))
                {
                    // first-time init: leave ClosedBreakers empty (closed-by-default)
                }
            }
        }

        public PowerGridState Capture() => new PowerGridState
        {
            SimDay = SimDay,
            GenerationWatts = GenerationWatts,
            FuelUnits = FuelUnits,
            BatteryReserveWh = BatteryReserveWh,
            BatteryCapacityWh = BatteryCapacityWh,
            ClosedBreakers = new List<string>(ClosedBreakers),
            TrippedRooms = new List<string>(TrippedRooms),
            Priorities = new List<RoomPriorityRecord>(Priorities),
            LastSurgeDay = LastSurgeDay
        };

        public void RestoreInto(PowerGridState state, IReadOnlyList<PowerGridRoom> rooms)
        {
            SimDay = state.SimDay;
            GenerationWatts = state.GenerationWatts;
            FuelUnits = state.FuelUnits;
            BatteryReserveWh = state.BatteryReserveWh;
            BatteryCapacityWh = state.BatteryCapacityWh;
            LastSurgeDay = state.LastSurgeDay;
            ClosedBreakers = state.ClosedBreakers ?? new List<string>();
            TrippedRooms = state.TrippedRooms ?? new List<string>();
            Priorities = state.Priorities ?? new List<RoomPriorityRecord>();
            NormalizeAndValidate(rooms);
        }
    }

    [Serializable]
    public sealed class RoomPriorityRecord
    {
        public string RoomId;
        public PowerGridRoomPriority Priority;
    }

    [Serializable]
    public sealed class PowerGridSystemState
    {
        public int SimDay;
        public float GenerationWatts;
        public float FuelUnits;
        public float BatteryReserveWh;
        public float BatteryCapacityWh;
        public List<string> ClosedBreakers = new List<string>();
        public List<string> TrippedRooms = new List<string>();
        public List<RoomPriorityRecord> Priorities = new List<RoomPriorityRecord>();
    }

    [Serializable]
    public sealed class PowerGridEvent
    {
        public PowerGridEventKind Kind;
        public string RoomId;
        public int Day;
        public string Detail;
        public float Numeric;

        public PowerGridEvent() { }

        public PowerGridEvent(PowerGridEventKind kind, string roomId, int day,
string? detail = null, float numeric = 0f)
        {
            Kind = kind;
            RoomId = roomId ?? string.Empty;
            Day = day;
            Detail = detail ?? string.Empty;
            Numeric = numeric;
        }
    }

    public enum PowerGridEventKind
    {
        BreakerToggled,
        PriorityChanged,
        FuelAdded,
        Tripped,
        SurgeApplied,
        GenerationChanged,
        TickSummary
    }

    [Serializable]
    public sealed class PowerGridTickSummary
    {
        public int Day;
        public float FuelConsumed;
        public float BatteryEndWh;
        public float BrownoutHours;
        public bool IsBrownout;
    }

    [Serializable]
    public sealed class PowerGridSnapshot
    {
        public int Day;
        public float GenerationWatts;
        public float FuelUnits;
        public float BatteryReserveWh;
        public float BatteryCapacityWh;
        public float TotalDrawWatts;
        public float NetWatts;
        public bool IsBrownout;
        public List<string> RoomIds = new List<string>();
        public Dictionary<string, float> GenerationContributions =
            new Dictionary<string, float>(StringComparer.Ordinal);
    }
}
