using System;
using System.Collections.Generic;

namespace Ashfall.Core.MoralChoice
{
    /// <summary>
    /// Runtime gossip processor: given loaded gossip data and the current
    /// moral choice state, returns the appropriate camp chatter, NPC greeting,
    /// and whisper lines for the player's current band. Handles gossip decay
    /// (intensity fades after N days without a new resolution).
    ///
    /// Pure function of (data, state, currentDay) — no mutable state of its
    /// own, so it needs no save/load. The host calls it when rendering NPC
    /// interactions or camp ambience.
    /// </summary>
    public sealed class MoralChoiceGossipRuntime
    {
        private readonly MoralChoiceGossipData _data;
        private readonly ISeededRng _rng;

        public MoralChoiceGossipRuntime(MoralChoiceGossipData data, ISeededRng rng)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
        }

        public MoralChoiceGossipData Data => _data;

        /// <summary>
        /// Get camp chatter lines appropriate for the player's current moral
        /// band. Returns an empty list if the band has no chatter data.
        /// </summary>
        public List<string> GetCampChatter(MoralPathBand band)
        {
            return band switch
            {
                MoralPathBand.VeryPositive => _data.CampChatter.VeryPositive,
                MoralPathBand.Positive => _data.CampChatter.Positive,
                MoralPathBand.SlightlyPositive => _data.CampChatter.SlightlyPositive,
                MoralPathBand.Neutral => _data.CampChatter.Neutral,
                MoralPathBand.SlightlyEvil => _data.CampChatter.SlightlyEvil,
                MoralPathBand.Evil => _data.CampChatter.Evil,
                MoralPathBand.VeryEvil => _data.CampChatter.VeryEvil,
                _ => new List<string>()
            };
        }

        /// <summary>
        /// Get a random camp chatter line for the band, or empty string if none.
        /// </summary>
        public string PickCampChatter(MoralPathBand band)
        {
            var lines = GetCampChatter(band);
            if (lines.Count == 0) return string.Empty;
            return lines[_rng.Next(0, lines.Count)];
        }

        /// <summary>Get NPC greeting lines for the player's current moral band.</summary>
        public List<string> GetNpcGreetings(MoralPathBand band)
        {
            return band switch
            {
                MoralPathBand.VeryPositive => _data.NpcGreetingShifts.VeryPositive,
                MoralPathBand.Positive => _data.NpcGreetingShifts.Positive,
                MoralPathBand.SlightlyPositive => _data.NpcGreetingShifts.SlightlyPositive,
                MoralPathBand.Neutral => _data.NpcGreetingShifts.Neutral,
                MoralPathBand.SlightlyEvil => _data.NpcGreetingShifts.SlightlyEvil,
                MoralPathBand.Evil => _data.NpcGreetingShifts.Evil,
                MoralPathBand.VeryEvil => _data.NpcGreetingShifts.VeryEvil,
                _ => new List<string>()
            };
        }

        /// <summary>Pick a random NPC greeting line for the band.</summary>
        public string PickNpcGreeting(MoralPathBand band)
        {
            var lines = GetNpcGreetings(band);
            if (lines.Count == 0) return string.Empty;
            return lines[_rng.Next(0, lines.Count)];
        }

        /// <summary>Get whisper lines (fragments the player catches as they pass).</summary>
        public List<string> GetWhisperLines(MoralPathBand band)
        {
            return band switch
            {
                MoralPathBand.VeryPositive => _data.WhisperLines.VeryPositive,
                MoralPathBand.Positive => _data.WhisperLines.Positive,
                MoralPathBand.Neutral => _data.WhisperLines.Neutral,
                MoralPathBand.SlightlyEvil => _data.WhisperLines.SlightlyEvil,
                MoralPathBand.Evil => _data.WhisperLines.Evil,
                MoralPathBand.VeryEvil => _data.WhisperLines.VeryEvil,
                _ => new List<string>()
            };
        }

        /// <summary>Pick a random whisper line for the band.</summary>
        public string PickWhisper(MoralPathBand band)
        {
            var lines = GetWhisperLines(band);
            if (lines.Count == 0) return string.Empty;
            return lines[_rng.Next(0, lines.Count)];
        }

        /// <summary>
        /// Compute the effective gossip intensity given the time since the
        /// most recently PROPAGATED moral resolution. Returns the band to use
        /// for gossip display (may be neutral if gossip has fully decayed, or
        /// if the most recent act hasn't left the witnessing circle yet).
        ///
        /// A resolution's consequences do not reach camp chatter the instant
        /// it happens — MoralChoiceResolution.propagatesOnDay (set in
        /// MoralChoiceSystem.Resolve as resolvedDay + 1..3) is the day gossip
        /// about it actually starts circulating. This mirrors Reconcile()'s
        /// overnight settlement of band-crossing events: an act's social
        /// consequences land on their own delay, never mid-scene. The decay
        /// clock below is anchored to propagatesOnDay, not resolvedDay — a
        /// choice that hasn't propagated yet contributes nothing to the
        /// currently effective gossip, even if it already changed the band.
        ///
        /// Decay rules: after decay_interval_days without a newly propagated
        /// resolution, gossip downgrades one intensity level. After
        /// full_decay_days, it reaches neutral baseline. A dramatic act
        /// (moral_delta >= threshold) resets the clock.
        /// </summary>
        public MoralPathBand GetEffectiveGossipBand(MoralChoiceSystem system, int currentDay)
        {
            var actualBand = system.CurrentBand;

            if (system.QuestsResolved == 0) return MoralPathBand.Neutral;

            int lastPropagatedDay = -1;
            bool wasDramatic = false;
            foreach (var r in system.Resolutions)
            {
                // A resolution that hasn't propagated yet is invisible to gossip:
                // the wasteland hasn't heard about it, so it cannot anchor the
                // decay clock or register as this cycle's dramatic act.
                if (r.propagatesOnDay > currentDay) continue;

                if (r.propagatesOnDay > lastPropagatedDay)
                {
                    lastPropagatedDay = r.propagatesOnDay;
                    wasDramatic = Math.Abs(r.moralDelta) >= _data.GossipDecay.DramaticResetThreshold;
                }
            }

            // Nothing has propagated to the camp yet — gossip has no material
            // to work with, so it stays at neutral baseline.
            if (lastPropagatedDay < 0) return MoralPathBand.Neutral;

            int daysSincePropagation = currentDay - lastPropagatedDay;

            // Full decay → neutral baseline regardless of dramatic status.
            if (daysSincePropagation >= _data.GossipDecay.FullDecayDays)
                return MoralPathBand.Neutral;

            // Within the first decay interval, gossip holds at full intensity
            // (a dramatic act resets the clock).
            if (daysSincePropagation < _data.GossipDecay.DecayIntervalDays || wasDramatic)
                return actualBand;

            // Past one interval, not dramatic: decay one level toward neutral.
            return DecayOneLevel(actualBand);
        }

        /// <summary>Decay one intensity level toward neutral.</summary>
        private static MoralPathBand DecayOneLevel(MoralPathBand band)
        {
            return band switch
            {
                MoralPathBand.VeryEvil => MoralPathBand.Evil,
                MoralPathBand.Evil => MoralPathBand.SlightlyEvil,
                MoralPathBand.SlightlyEvil => MoralPathBand.Neutral,
                MoralPathBand.SlightlyPositive => MoralPathBand.Neutral,
                MoralPathBand.Positive => MoralPathBand.SlightlyPositive,
                MoralPathBand.VeryPositive => MoralPathBand.Positive,
                _ => MoralPathBand.Neutral
            };
        }
    }
}
