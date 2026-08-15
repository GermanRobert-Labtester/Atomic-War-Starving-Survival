using System;
using Ashfall.Core.Clock;
using Ashfall.Core.Events;
using Ashfall.Core.Flags;

namespace Ashfall.Core.Radio
{
    /// <summary>Live world census — implemented against the flag ledger and dweller registry.</summary>
    public interface IWorldCensus
    {
        /// <summary>
        /// Dwellers + named NPCs alive + traders on-schedule. Includes exp_09 hook:
        /// the flotilla's rescued are NOT counted (they are beyond the ash-sea) — which means
        /// the discrepancy equals the number of the player's "dead." The machine is right
        /// for the wrong reason, and that wrongness is the exp_09 S3 detonation.
        /// </summary>
        long LivingRegisteredSouls();
    }

    /// <summary>
    /// Schedules the 99.0 MHz "Census Window" broadcasts for exp_08.
    /// The observed count is computed LIVE from world state — the machine reads
    /// your save. Broadcast structure is fixed: carrier(4s) → header → pause(1.7s)
    /// → count → footer → carrier(4s). The pause duration is load-bearing canon
    /// ("precisely 1.7s — the duration of a held breath") and must not be randomized.
    /// </summary>
    public sealed class CensusBroadcastScheduler
    {
        public const double CarrierSeconds = 4.0;
        public const double HeldBreathPauseSeconds = 1.7; // canon constant — do not tune
        public const long ExpectedProvincialCount = 211004;

        private readonly ISimClock _clock;
        private readonly IEventBus _bus;
        private readonly IFlagLedger _flags;
        private readonly ISeededRng _radioRng; // stream: "radio" — never contaminates sim RNG
        private readonly IWorldCensus _census;

        public CensusBroadcastScheduler(
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

        /// <summary>
        /// Broadcast windows open every 7 in-game days at 03:00 — the hour
        /// the valley is most asleep. Deterministic from the day index alone.
        /// </summary>
        public bool IsWindowOpen()
            => _clock.DayIndex % 7 == 0 && _clock.HourOfDay == 3;

        public void BroadcastIfDue()
        {
            if (!IsWindowOpen()) return;

            long observed = _census != null ? _census.LivingRegisteredSouls() : 0;
            long discrepancy = ExpectedProvincialCount - observed;

            _bus.Publish("radio.carrier.open", 99.0);

            // The machine never says the full expected number after exp_08's midpoint;
            // it starts saying "minus" instead. Grief degrades its grammar.
            bool degradation = _flags.IsSet("flag_exp08_machine_degraded");
            string header = degradation
                ? "Window open. Count follows."
                : $"Census window open. Provincial count: expected {ExpectedProvincialCount:n0}.";

            _bus.Publish("radio.census.header", header);
            _bus.Publish("radio.census.pause", HeldBreathPauseSeconds);
            _bus.Publish("radio.census.count", observed);

            string footer = _flags.IsSet("flag_exp08_signed_reckoning")
                ? $"Discrepancy: {discrepancy:n0}. Signature received. Window closes."
                : "Discrepancy: pending signature. Window closes.";

            _bus.Publish("radio.census.footer", footer);

            // exp_08 S1 hook: if the player is teaching the judge wrong, the count
            // quietly poisons exp_12 reconciliation eligibility.
            if (_flags.IsSet("flag_exp08_teaching_the_judge"))
                _flags.Increment("counter_exp12_reconciliation_debt");

            _bus.Publish("radio.carrier.close", 99.0);
        }
    }
}
