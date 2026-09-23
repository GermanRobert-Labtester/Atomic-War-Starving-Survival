using System;
using System.Collections.Generic;

namespace Ashfall.Core.Voice
{
    /// <summary>
    /// Contextual facts about the speaking survivor.
    /// </summary>
    public readonly struct SurvivorVoiceContext
    {
        public string SurvivorId { get; }
        public string VoiceProfileId { get; }
        public int MoralePermille { get; }
        public int StressPermille { get; }
        public IReadOnlyList<string> TraumaTags { get; }
        public bool IsIncapacitated { get; }

        public SurvivorVoiceContext(
            string survivorId,
            string voiceProfileId,
            int moralePermille,
            int stressPermille,
            IReadOnlyList<string> traumaTags = null,
            bool isIncapacitated = false)
        {
            SurvivorId = survivorId ?? string.Empty;
            VoiceProfileId = voiceProfileId ?? string.Empty;
            MoralePermille = Math.Max(0, Math.Min(1000, moralePermille));
            StressPermille = Math.Max(0, Math.Min(1000, stressPermille));
            TraumaTags = traumaTags ?? Array.Empty<string>();
            IsIncapacitated = isIncapacitated;
        }

        public bool HasTag(string tag)
        {
            if (TraumaTags == null || string.IsNullOrWhiteSpace(tag)) return false;
            for (int i = 0; i < TraumaTags.Count; i++)
            {
                if (string.Equals(TraumaTags[i], tag, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Candidate voice line specification authored in game data.
    /// </summary>
    public sealed class VoiceLineCandidate
    {
        public string LineId { get; set; }
        public string VoiceKey { get; set; }
        public string TextKey { get; set; }
        public string AudioCueKey { get; set; }
        public string EventKind { get; set; }
        public int MinStressPermille { get; set; }
        public int MaxStressPermille { get; set; } = 1000;
        public int MinMoralePermille { get; set; }
        public int MaxMoralePermille { get; set; } = 1000;
        public string RequiredTraumaTag { get; set; }
        public int PriorityWeight { get; set; } = 100;
    }

    /// <summary>
    /// Immutable selection result emitted by <see cref="VoiceLineSelectionEngine"/>.
    /// </summary>
    public readonly struct VoiceLineSelectionResult
    {
        public bool HasLine { get; }
        public string LineId { get; }
        public string VoiceKey { get; }
        public string TextKey { get; }
        public string AudioCueKey { get; }
        public string SpeakerSurvivorId { get; }

        public VoiceLineSelectionResult(
            bool hasLine,
            string lineId,
            string voiceKey,
            string textKey,
            string audioCueKey,
            string speakerSurvivorId)
        {
            HasLine = hasLine;
            LineId = lineId ?? string.Empty;
            VoiceKey = voiceKey ?? string.Empty;
            TextKey = textKey ?? string.Empty;
            AudioCueKey = audioCueKey ?? string.Empty;
            SpeakerSurvivorId = speakerSurvivorId ?? string.Empty;
        }

        public static VoiceLineSelectionResult None => new VoiceLineSelectionResult(false, null, null, null, null, null);
    }

    /// <summary>
    /// Pure domain engine for deterministic survivor voice line selection (Plan 42 / C2[18]).
    /// Evaluates mood, stress thresholds, trauma tags, and event kinds without unseeded RNG or engine dependencies.
    /// </summary>
    public static class VoiceLineSelectionEngine
    {
        public const int PermilleScale = 1000;

        /// <summary>
        /// Selects the most appropriate voice line candidate deterministically.
        /// </summary>
        /// <param name="survivor">Speaker survivor state context.</param>
        /// <param name="eventKind">Triggered event identifier.</param>
        /// <param name="candidates">Candidate lines to evaluate.</param>
        /// <param name="deterministicSeed">Integer seed for reproducible tie-breaks.</param>
        /// <returns>Selected <see cref="VoiceLineSelectionResult"/> or None if no match.</returns>
        public static VoiceLineSelectionResult SelectLine(
            SurvivorVoiceContext survivor,
            string eventKind,
            IEnumerable<VoiceLineCandidate> candidates,
            int deterministicSeed = 0)
        {
            if (survivor.IsIncapacitated || string.IsNullOrWhiteSpace(survivor.SurvivorId) || candidates == null)
            {
                return VoiceLineSelectionResult.None;
            }

            var qualifying = new List<VoiceLineCandidate>();

            foreach (var line in candidates)
            {
                if (line == null) continue;

                // Event kind match
                if (!string.Equals(line.EventKind, eventKind, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Stress bounds
                if (survivor.StressPermille < line.MinStressPermille || survivor.StressPermille > line.MaxStressPermille)
                {
                    continue;
                }

                // Morale bounds
                if (survivor.MoralePermille < line.MinMoralePermille || survivor.MoralePermille > line.MaxMoralePermille)
                {
                    continue;
                }

                // Required tag check
                if (!string.IsNullOrWhiteSpace(line.RequiredTraumaTag) && !survivor.HasTag(line.RequiredTraumaTag))
                {
                    continue;
                }

                qualifying.Add(line);
            }

            if (qualifying.Count == 0)
            {
                return VoiceLineSelectionResult.None;
            }

            // Deterministic priority and weighted selection
            // Sort primary by PriorityWeight descending, then stable tie-break by LineId hash + seed
            qualifying.Sort((a, b) =>
            {
                int cmp = b.PriorityWeight.CompareTo(a.PriorityWeight);
                if (cmp != 0) return cmp;

                int hashA = (a.LineId?.GetHashCode() ?? 0) ^ deterministicSeed;
                int hashB = (b.LineId?.GetHashCode() ?? 0) ^ deterministicSeed;
                return hashA.CompareTo(hashB);
            });

            var selected = qualifying[0];

            return new VoiceLineSelectionResult(
                hasLine: true,
                lineId: selected.LineId,
                voiceKey: selected.VoiceKey,
                textKey: selected.TextKey,
                audioCueKey: selected.AudioCueKey,
                speakerSurvivorId: survivor.SurvivorId
            );
        }
    }
}
