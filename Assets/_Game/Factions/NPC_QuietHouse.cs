using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_QuietHouseState
    {
        public string id = "faction_quiet_house";
        public string displayName = "The Quiet House";
        public bool isActive;
        /// <summary>Intakes recorded: name, the true thing, and whether the player lied.</summary>
        public List<QuietHouseIntake> intakes = new List<QuietHouseIntake>();
    }

    [Serializable]
    public class QuietHouseIntake
    {
        public string personName;
        public string trueThing;
        /// <summary>The House writes the tag exactly as given. The game never adjudicates.</summary>
        public bool toldTrue;
        public int day;
    }

    /// <summary>
    /// Lore bible 05_FACTIONS §3 — The Quiet House (peaceful Current).
    /// They take the dying that nobody else can care for. No payment, no
    /// sermon. They ask for exactly two things: the person's name, and one
    /// true thing about them. The room at the back is never revealed,
    /// never confirmed, never denied.
    /// </summary>
    public class NPC_QuietHouse
    {
        private NPC_QuietHouseState _state = new NPC_QuietHouseState();

        public event Action<NPC_QuietHouseState, QuietHouseIntake> OnIntakeRecorded;

        public NPC_QuietHouseState State => _state;

        public void Initialise(string displayName)
        {
            if (!string.IsNullOrEmpty(displayName)) _state.displayName = displayName;
            _state.isActive = true;
        }

        /// <summary>
        /// Accept a dying survivor into care. The true thing is recorded
        /// exactly as given — including the lie, which the House accepts
        /// without comment. Returns the catalogued intake.
        /// </summary>
        public QuietHouseIntake AcceptTheDying(string personName, string trueThing, int day)
        {
            var intake = new QuietHouseIntake
            {
                personName = personName ?? "",
                trueThing = trueThing ?? "",
                toldTrue = true,
                day = day
            };
            _state.intakes.Add(intake);
            OnIntakeRecorded?.Invoke(_state, intake);
            return intake;
        }

        /// <summary>Overload the player uses when choosing to lie.</summary>
        public QuietHouseIntake AcceptTheDyingWithLie(string personName, string falseThing, int day)
        {
            var intake = AcceptTheDying(personName, falseThing, day);
            intake.toldTrue = false;
            return intake;
        }

        public NPC_QuietHouseState CaptureState() => _state;
        public void RestoreState(NPC_QuietHouseState saved) { _state = saved ?? new NPC_QuietHouseState(); }
    }
}
