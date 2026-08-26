using System.Collections.Generic;

namespace Ashfall.Core.MoralChoice
{
    /// <summary>
    /// Wire shape for moral_choice_gossip.json — camp chatter, NPC greeting
    /// shifts, whisper lines, and decay rules. The player never sees their
    /// score; these reactions are the world's UI.
    /// </summary>
    public sealed class MoralChoiceGossipData
    {
        public MoralCampChatter CampChatter { get; set; } = new MoralCampChatter();
        public MoralNpcGreetingShifts NpcGreetingShifts { get; set; } = new MoralNpcGreetingShifts();
        public MoralWhisperLines WhisperLines { get; set; } = new MoralWhisperLines();
        public MoralGossipDecay GossipDecay { get; set; } = new MoralGossipDecay();
    }

    public sealed class MoralCampChatter
    {
        public List<string> VeryPositive { get; set; } = new List<string>();
        public List<string> Positive { get; set; } = new List<string>();
        public List<string> SlightlyPositive { get; set; } = new List<string>();
        public List<string> Neutral { get; set; } = new List<string>();
        public List<string> SlightlyEvil { get; set; } = new List<string>();
        public List<string> Evil { get; set; } = new List<string>();
        public List<string> VeryEvil { get; set; } = new List<string>();
    }

    public sealed class MoralNpcGreetingShifts
    {
        public List<string> VeryPositive { get; set; } = new List<string>();
        public List<string> Positive { get; set; } = new List<string>();
        public List<string> SlightlyPositive { get; set; } = new List<string>();
        public List<string> Neutral { get; set; } = new List<string>();
        public List<string> SlightlyEvil { get; set; } = new List<string>();
        public List<string> Evil { get; set; } = new List<string>();
        public List<string> VeryEvil { get; set; } = new List<string>();
    }

    public sealed class MoralWhisperLines
    {
        public List<string> VeryPositive { get; set; } = new List<string>();
        public List<string> Positive { get; set; } = new List<string>();
        public List<string> Neutral { get; set; } = new List<string>();
        public List<string> SlightlyEvil { get; set; } = new List<string>();
        public List<string> Evil { get; set; } = new List<string>();
        public List<string> VeryEvil { get; set; } = new List<string>();
    }

    public sealed class MoralGossipDecay
    {
        public int DecayIntervalDays { get; set; } = 30;
        public int FullDecayDays { get; set; } = 60;
        public int DramaticResetThreshold { get; set; } = 10;
    }
}
