using System;
using System.Collections.Generic;

namespace Ashfall.Core.MoralChoice
{
    // ── JSON wire records ──

    [Serializable]
    public sealed class MoralChoiceGossipContainer
    {
        public int schema_version = 1;
        public string description = string.Empty;
        public MoralCampChatterRecord camp_chatter = new MoralCampChatterRecord();
        public MoralNpcGreetingShiftsRecord npc_greeting_shifts = new MoralNpcGreetingShiftsRecord();
        public MoralWhisperLinesRecord whisper_lines = new MoralWhisperLinesRecord();
        public MoralGossipDecayRecord gossip_decay = new MoralGossipDecayRecord();
    }

    [Serializable]
    public sealed class MoralCampChatterRecord
    {
        public string description = string.Empty;
        public List<string> very_positive = new List<string>();
        public List<string> positive = new List<string>();
        public List<string> slightly_positive = new List<string>();
        public List<string> neutral = new List<string>();
        public List<string> slightly_evil = new List<string>();
        public List<string> evil = new List<string>();
        public List<string> very_evil = new List<string>();
    }

    [Serializable]
    public sealed class MoralNpcGreetingShiftsRecord
    {
        public string description = string.Empty;
        public List<string> very_positive = new List<string>();
        public List<string> positive = new List<string>();
        public List<string> slightly_positive = new List<string>();
        public List<string> neutral = new List<string>();
        public List<string> slightly_evil = new List<string>();
        public List<string> evil = new List<string>();
        public List<string> very_evil = new List<string>();
    }

    [Serializable]
    public sealed class MoralWhisperLinesRecord
    {
        public string description = string.Empty;
        public List<string> very_positive = new List<string>();
        public List<string> positive = new List<string>();
        public List<string> neutral = new List<string>();
        public List<string> slightly_evil = new List<string>();
        public List<string> evil = new List<string>();
        public List<string> very_evil = new List<string>();
    }

    [Serializable]
    public sealed class MoralGossipDecayRecord
    {
        public string description = string.Empty;
        public int decay_interval_days = 30;
        public int full_decay_days = 60;
        public int dramatic_reset_threshold = 10;
    }

    /// <summary>
    /// Loads moral_choice_gossip.json — camp chatter, NPC greeting shifts,
    /// whisper lines, and decay rules. Engine-agnostic.
    /// </summary>
    public static class MoralChoiceGossipCatalogLoader
    {
        public const string DefaultFileName = "moral_choice_gossip.json";

        public static MoralChoiceGossipData Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new MoralChoiceGossipData();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new MoralChoiceGossipData();

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return new MoralChoiceGossipData();

            var container = json.Deserialize<MoralChoiceGossipContainer>(raw);
            if (container == null)
                return new MoralChoiceGossipData();

            return Map(container);
        }

        private static MoralChoiceGossipData Map(MoralChoiceGossipContainer c)
        {
            var chatter = c.camp_chatter ?? new MoralCampChatterRecord();
            var greetings = c.npc_greeting_shifts ?? new MoralNpcGreetingShiftsRecord();
            var whispers = c.whisper_lines ?? new MoralWhisperLinesRecord();
            var decay = c.gossip_decay ?? new MoralGossipDecayRecord();

            return new MoralChoiceGossipData
            {
                CampChatter = new MoralCampChatter
                {
                    VeryPositive = chatter.very_positive ?? new List<string>(),
                    Positive = chatter.positive ?? new List<string>(),
                    SlightlyPositive = chatter.slightly_positive ?? new List<string>(),
                    Neutral = chatter.neutral ?? new List<string>(),
                    SlightlyEvil = chatter.slightly_evil ?? new List<string>(),
                    Evil = chatter.evil ?? new List<string>(),
                    VeryEvil = chatter.very_evil ?? new List<string>()
                },
                NpcGreetingShifts = new MoralNpcGreetingShifts
                {
                    VeryPositive = greetings.very_positive ?? new List<string>(),
                    Positive = greetings.positive ?? new List<string>(),
                    SlightlyPositive = greetings.slightly_positive ?? new List<string>(),
                    Neutral = greetings.neutral ?? new List<string>(),
                    SlightlyEvil = greetings.slightly_evil ?? new List<string>(),
                    Evil = greetings.evil ?? new List<string>(),
                    VeryEvil = greetings.very_evil ?? new List<string>()
                },
                WhisperLines = new MoralWhisperLines
                {
                    VeryPositive = whispers.very_positive ?? new List<string>(),
                    Positive = whispers.positive ?? new List<string>(),
                    Neutral = whispers.neutral ?? new List<string>(),
                    SlightlyEvil = whispers.slightly_evil ?? new List<string>(),
                    Evil = whispers.evil ?? new List<string>(),
                    VeryEvil = whispers.very_evil ?? new List<string>()
                },
                GossipDecay = new MoralGossipDecay
                {
                    DecayIntervalDays = decay.decay_interval_days,
                    FullDecayDays = decay.full_decay_days,
                    DramaticResetThreshold = decay.dramatic_reset_threshold
                }
            };
        }
    }
}
