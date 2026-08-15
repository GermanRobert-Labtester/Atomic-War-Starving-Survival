using System;
using Ashfall.Core.Clock;
using Ashfall.Core.Events;
using Ashfall.Core.Flags;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// Drives the exp_07 flagship vigil scene ("The Dictation") and generalizes to all
    /// terminal-vigil content. Unskippable by UI; the only "skip" is diegetic —
    /// reaching for the water glass — which is itself a logged moral event.
    /// </summary>
    public enum VigilPhase
    {
        bedside,    // ambient; dwellers may enter/leave the room; needs still simulate
        dictation,  // the dying dweller speaks the name list; one name per beat_ticks
        silence,    // post-list stillness; audio ducks; the phantom knock fires here
        aftermath   // grief timers start; journal auto-entry writes in witness's hand
    }

    public sealed record VigilDefinition(
        string vigil_id,
        string dying_dweller_id,
        string witness_dweller_id,      // whose hand writes the journal entry
        string[] name_list,              // the thirty-one names; one per dictation beat
        int beat_ticks,                  // master-clock ticks per name (default 12 = 4 min total)
        string phantom_knock_flag,       // flag_exp07_vel_vigil_knock or null
        string journal_entry_template    // diary template id, e.g. jrnl_vel_vigil
    );

    public sealed class VigilStateMachine
    {
        private readonly ISimClock _clock;
        private readonly IEventBus _bus;
        private readonly IFlagLedger _flags;
        private readonly VigilDefinition _def;

        public VigilPhase Phase { get; private set; } = VigilPhase.bedside;
        public int NamesSpoken { get; private set; }
        private long _phaseStartTick;

        public VigilStateMachine(ISimClock clock, IEventBus bus, IFlagLedger flags, VigilDefinition def)
        {
            _clock = clock;
            _bus = bus;
            _flags = flags;
            _def = def;
            _phaseStartTick = clock.CurrentTick;
        }

        /// <summary>
        /// Diegetic "skip": the dying dweller reaches for the water glass.
        /// This is a choice, not a fast-forward. It sets a flag and the scene ends early.
        /// </summary>
        public void ReachForWaterGlass()
        {
            if (Phase != VigilPhase.dictation) return;
            _flags.Set($"flag_vigil_witness_refused_{_def.dying_dweller_id}");
            _bus.Publish("vigil.witness_refused", _def.dying_dweller_id);
            Transition(VigilPhase.aftermath);
        }

        public void Tick()
        {
            long elapsed = _clock.CurrentTick - _phaseStartTick;
            switch (Phase)
            {
                case VigilPhase.bedside:
                    // 30 ticks of ambient room tone before the dying begin to speak
                    if (elapsed >= 30) Transition(VigilPhase.dictation);
                    break;

                case VigilPhase.dictation:
                    if (_def.name_list != null && _def.name_list.Length > 0)
                    {
                        if (elapsed >= (long)_def.beat_ticks * (NamesSpoken + 1))
                        {
                            _bus.Publish("vigil.name_spoken", _def.name_list[NamesSpoken]);
                            NamesSpoken++;
                            if (NamesSpoken >= _def.name_list.Length)
                            {
                                Transition(VigilPhase.silence);
                            }
                        }
                    }
                    else
                    {
                        Transition(VigilPhase.silence);
                    }
                    break;

                case VigilPhase.silence:
                    // The candle gutters at 60% of the silence window; the knock fires at tick 24
                    if (elapsed >= 24 && _def.phantom_knock_flag != null
                        && !_flags.IsSet(_def.phantom_knock_flag))
                    {
                        _flags.Set(_def.phantom_knock_flag);
                        _bus.Publish("door.knock.practiced", payload: null); // visitor_id: null
                    }
                    if (elapsed >= 48) Transition(VigilPhase.aftermath);
                    break;

                case VigilPhase.aftermath:
                    _bus.Publish("vigil.complete", _def.vigil_id);
                    _bus.Publish("journal.autowrite", _def.journal_entry_template);
                    break;
            }
        }

        private void Transition(VigilPhase next)
        {
            _bus.Publish($"vigil.phase.{next}", _def.vigil_id);
            Phase = next;
            _phaseStartTick = _clock.CurrentTick;
        }
    }
}
