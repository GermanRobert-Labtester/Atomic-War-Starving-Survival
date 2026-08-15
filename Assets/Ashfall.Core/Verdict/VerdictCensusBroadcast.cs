using System;
using Ashfall.Core.Clock;
using Ashfall.Core.Events;
using Ashfall.Core.Flags;

namespace Ashfall.Core.Verdict
{
    /// <summary>Live world census — the machine reads the player's save.</summary>
    public interface IWorldCensus
    {
        /// <summary>Dwellers + named NPCs alive + traders on-schedule.</summary>
        long LivingRegisteredSouls();
    }

    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — the unified Census Broadcast
    /// engine on 99.0 MHz. This is the single authoritative carrier: fixed
    /// structure (carrier 4s → header → pause 1.7s → count → footer → carrier
    /// 4s), the 1.7 s held-breath pause is canon and load-bearing. Windows open
    /// every 7 in-game days at 03:00, deterministic from the clock.
    /// Adopts the (previously dead) Radio/CensusBroadcastScheduler canon and
    /// deletes the duplicate math-only class.
    /// </summary>
    public sealed class VerdictCensusBroadcast
    {
        public const double CarrierSeconds = 4.0;
        public const double HeldBreathPauseSeconds = 1.7; // canon — do not tune
        public const long ExpectedProvincialCount = 211004;

        private readonly ISimClock _clock;
        private readonly IEventBus _bus;
        private readonly IFlagLedger _flags;
        private readonly ISeededRng _radioRng;
        private readonly IWorldCensus _census;
        private int _lastWindowDay = -1;

        public VerdictCensusBroadcast(
            ISimClock clock,
            IEventBus bus,
            IFlagLedger flags,
            ISeededRng radioRng,
            IWorldCensus census)
        {
            _clock = clock;
            _bus = bus;
            _flags = flags;
            _radioRng = radioRng;
            _census = census;
        }

        /// <summary>Windows open every 7 in-game days at 03:00 — the hour the valley is most asleep.</summary>
        public bool IsWindowOpen()
            => _clock.DayIndex % 7 == 0 && _clock.HourOfDay == 3;

        /// <summary>Fire the broadcast once per window. Idempotent per day.</summary>
        public void BroadcastIfDue()
        {
            if (!IsWindowOpen()) return;
            if (_lastWindowDay == _clock.DayIndex) return; // already broadcast this window
            if (_flags.IsSet("flag_exp08_signed_reckoning")) return; // post-ending: the woman is silent

            long observed = _census != null ? _census.LivingRegisteredSouls() : 0;
            long discrepancy = ExpectedProvincialCount - observed;

            _bus.Publish("radio.carrier.open", 99.0);

            bool degraded = _flags.IsSet("flag_exp08_machine_degraded");
            string header = degraded
                ? "Window open. Count follows."
                : $"Census window open. Provincial count: expected {ExpectedProvincialCount:n0}.";

            _bus.Publish("radio.census.header", header);
            _bus.Publish("radio.census.pause", HeldBreathPauseSeconds);
            _bus.Publish("radio.census.count", observed);

            string footer = _flags.IsSet("flag_exp08_signed_reckoning")
                ? $"Discrepancy: {discrepancy:n0}. Signature received. Window closes."
                : "Discrepancy: pending signature. Window closes.";

            _bus.Publish("radio.census.footer", footer);

            _lastWindowDay = _clock.DayIndex;
            _bus.Publish("radio.carrier.close", 99.0);
        }

        /// <summary>Reset the per-window latch (used by tests/reloads).</summary>
        public void ResetWindowLatch() => _lastWindowDay = -1;

        /// <summary>Total windows elapsed (test observability).</summary>
        public int LastWindowDay => _lastWindowDay;
    }
}
