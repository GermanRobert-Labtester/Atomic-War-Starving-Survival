using System;

namespace AtomicWar._Game.Events
{
    /// <summary>
    /// Describes the broad category of a moral timeline event logged in the chronicle.
    /// </summary>
    public enum MoralChronicleEntryKind
    {
        Unknown,
        /// <summary>Desperate survival choice: butchering, abandonment, or rationing under starvation.</summary>
        DesperateChoice,
        /// <summary>Survivor death — natural, radiation, or violence.</summary>
        SurvivorLost,
        /// <summary>Bunker module critically failed (filter, shielding, power).</summary>
        BunkerCriticalFailure,
        /// <summary>Scavenging expedition returned with morally weighted cargo or evidence.</summary>
        ExpeditionMoralFind,
        /// <summary>Radio/radio contact with strangers — trust decision made.</summary>
        RadioContactDecision,
        /// <summary>Player chose to help, betray, or ignore an outside faction or refugee.</summary>
        FactionChoice,
    }

    /// <summary>
    /// One entry in the moral timeline displayed by <see cref="AtomicWar._Game.UI.MoralChronicleUI"/>.
    /// Populated by MoralChronicleBridge when significant events occur. (Prompt #42)
    /// </summary>
    [Serializable]
    public class MoralChronicleEntry
    {
        /// <summary>Campaign day on which this event occurred.</summary>
        public int Day;
        /// <summary>Short human-readable description in first-person plural ("We chose…").</summary>
        public string Description;
        /// <summary>Category for icon/colour coding in the chronicle UI.</summary>
        public MoralChronicleEntryKind Kind;
        /// <summary>Optional name of the survivor involved, or empty.</summary>
        public string SurvivorName;

        public override string ToString()
        {
            string who = string.IsNullOrEmpty(SurvivorName) ? string.Empty : $" [{SurvivorName}]";
            return $"Day {Day}{who} — {Description}";
        }
    }
}
