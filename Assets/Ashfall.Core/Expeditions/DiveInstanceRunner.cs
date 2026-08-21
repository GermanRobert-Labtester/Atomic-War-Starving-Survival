using System;
using Ashfall.Core.Events;
using Ashfall.Core.Flags;

namespace Ashfall.Core.Expeditions
{
    public enum DiveRoom { deckhouse, companionway, hold_approach, the_hold }
    public enum SovereignChoice { undecided, flood_the_market, burn_the_hold }

    public sealed record DiveSiteDefinition(
        string site_id,              // site_exp09_ss_sovereign
        int oxygen_budget_ticks,      // hand-crank compressor capacity; upgrades raise it
        double base_noise_floor,      // storm masking; acid squall LOWERS detection
        string keeper_thread_id       // q_keeper_of_logs — observation props check this
    );

    /// <summary>
    /// Driven by the existing stealth/noise model, with the final-room choice expressed
    /// as locomotion (walk to crane lever OR flare rack) rather than a UI menu.
    /// </summary>
    public sealed class DiveInstanceRunner
    {
        private readonly IEventBus _bus;
        private readonly IFlagLedger _flags;
        private readonly ISeededRng _diveRng; // stream: "dive"
        private readonly DiveSiteDefinition _site;

        public DiveRoom CurrentRoom { get; private set; } = DiveRoom.deckhouse;
        public int OxygenRemaining { get; private set; }
        public SovereignChoice Choice { get; private set; } = SovereignChoice.undecided;

        public DiveInstanceRunner(
            IEventBus bus,
            IFlagLedger flags,
            ISeededRng diveRng,
            DiveSiteDefinition site)
        {
            _bus = bus;
            _flags = flags;
            _diveRng = diveRng;
            _site = site;
            OxygenRemaining = site != null ? site.oxygen_budget_ticks : 120;
        }

        /// <summary>
        /// Noise model: the acid squall is cover. Detection risk rises in the
        /// companionway where the hull stops hissing. The 432 Hz pipes harmonize with
        /// diver breath-rate — breathing faster (fear state) raises anomaly resonance.
        /// </summary>
        public double DetectionRisk(double diverNoise, bool fearState)
        {
            if (_site == null) return 0.0;

            double stormMask = CurrentRoom == DiveRoom.deckhouse
                ? _site.base_noise_floor
                : _site.base_noise_floor * 0.35; // companionway: the hull quietens the rain

            double resonance = fearState && CurrentRoom == DiveRoom.hold_approach ? 0.15 : 0.0;
            double raw = diverNoise - stormMask + resonance;
            return MathfCompat.Clamp((float)raw, 0.0f, 1.0f);
        }

        /// <summary>
        /// Room advance is one-way. There is no backtrack — the flood does not retreat.
        /// </summary>
        public bool Advance()
        {
            if (OxygenRemaining <= 0 || CurrentRoom == DiveRoom.the_hold) return false;

            CurrentRoom++;
            _bus.Publish($"dive.room.{CurrentRoom}", _site?.site_id!);

            // The Keeper is always one bulkhead ahead: observation props fire here,
            // feeding q_keeper_of_logs without ever rendering a figure.
            if (CurrentRoom == DiveRoom.hold_approach && _site != null)
            {
                _bus.Publish("anomaly.keeper_trace", _site.keeper_thread_id);
            }

            return true;
        }

        /// <summary>
        /// The hold choice is locomotion: the player walks the dweller to the
        /// crane lever or the flare rack. This method is called by the room's interaction
        /// volumes — NOT by a dialogue menu. Consequences detonate two expansions of flags.
        /// </summary>
        public void CommitChoice(SovereignChoice choice)
        {
            if (CurrentRoom != DiveRoom.the_hold || Choice != SovereignChoice.undecided)
                return;

            Choice = choice;
            if (choice == SovereignChoice.flood_the_market)
            {
                _flags.Set("flag_exp09_iodine_released");
                _bus.Publish("economy.serum_market.crash", "faction_the_syndicate");
            }
            else if (choice == SovereignChoice.burn_the_hold)
            {
                _flags.Set("flag_exp09_iodine_burned");
                _bus.Publish("census.denial", "the_tempest_directorate");
            }

            _bus.Publish("journal.autowrite", "jrnl_sovereign_one_line"); // "We were never poor. We were robbed."
        }

        public void TickOxygen()
        {
            OxygenRemaining--;
            if (OxygenRemaining == 30) _bus.Publish("dive.oxygen.low", OxygenRemaining);
            if (OxygenRemaining <= 0) _bus.Publish("dive.abort.forced", _site?.site_id!);
        }
    }
}
