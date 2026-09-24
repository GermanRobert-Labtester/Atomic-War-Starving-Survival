// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Needs
{
    /// <summary>Presentation class for a projected sleep beat. Never persisted.</summary>
    public enum SleepBeatKind
    {
        None = 0,
        Restful = 1,
        Troubled = 2,
        Nightmare = 3,
        PhantomPain = 4
    }

    /// <summary>One projected sleep/nightmare presentation beat.</summary>
    public readonly struct SleepBeat
    {
        public readonly SleepBeatKind Kind;
        public readonly string SurvivorId;
        public readonly string JournalKey;
        public readonly string Text;

        public SleepBeat(SleepBeatKind kind, string survivorId, string journalKey, string text)
        {
            Kind = kind;
            SurvivorId = survivorId ?? string.Empty;
            JournalKey = journalKey ?? string.Empty;
            Text = text ?? string.Empty;
        }

        /// <summary>Presentation should be written when a non-restful beat was projected.</summary>
        public bool Emitted => Kind == SleepBeatKind.Troubled || Kind == SleepBeatKind.Nightmare || Kind == SleepBeatKind.PhantomPain;
    }

    /// <summary>
    /// Plans 177/179 — read-only sleep/nightmare projection (DEBT-177-SLEEP-EVENT-CONSUMER).
    ///
    /// This is a <b>read model</b>, not a dream engine. It derives a sleep beat from
    /// the existing <see cref="SurvivorMentalHealthRecord"/> (insomnia, active
    /// trauma, stress, crisis) and a deterministic authored phrase. It writes no
    /// state anywhere: no `DreamSystem`, no dream templates, no profile ledger,
    /// and it never mutates the mental-health record or the treatment authority.
    /// The host turns an emitted beat into a journal line only.
    /// </summary>
    public sealed class SleepNarrativeProjection
    {
        /// <summary>Stress (permille) at or above which sleep reads as troubled.</summary>
        public const int TroubledStressPermille = 450;
        /// <summary>Stress (permille) at or above which sleep reads as a nightmare.</summary>
        public const int NightmareStressPermille = 700;

        public const string NightmareJournalKey = "sleep_nightmare";
        public const string TroubledJournalKey = "sleep_troubled";
        public const string RestfulJournalKey = "sleep_restful";
        public const string PhantomPainJournalKey = "sleep_phantom_pain";

        private readonly SurvivorMentalHealthSystem _mentalHealth;
        private readonly ISeededRng? _rng;

        public SleepNarrativeProjection(SurvivorMentalHealthSystem mentalHealth, ISeededRng? rng = null)
        {
            _mentalHealth = mentalHealth ?? throw new ArgumentNullException(nameof(mentalHealth));
            _rng = rng;
        }

        private static readonly string[] NightmareLines =
        {
            "Woke before dawn certain the walls had been closer in the dream. Sat up until the lamps came on.",
            "Slept badly. Something in the dark kept counting, and would not be talked out of it.",
            "Came awake with a shout held behind the teeth. Walked the corridor twice before lying back down."
        };

        private static readonly string[] TroubledLines =
        {
            "Tossed most of the night. Sleep came in short pieces and none of them held.",
            "Slept light, listening. Nothing came, but the listening did not stop.",
            "A restless night — woke at every pipe knock and every shift change."
        };

        private static readonly string[] RestfulLines =
        {
            "Slept through. Woke with the watch change and no memory of the hours between.",
            "A quiet night, for once. Slept deep and woke slow."
        };

        private static readonly string[] PhantomPainLines =
        {
            "Woke reaching for fingers that were not there. Took half an hour for the phantom cramp to ease.",
            "A sharp ache where the limb used to be kept sleep away until dawn.",
            "Dreamt of walking on two whole legs; woke to the cold reminder of the missing limb."
        };

        /// <summary>
        /// Projects the sleep beat for one survivor from existing state. Deterministic:
        /// the same record + same rng position yields the same line. Never mutates.
        /// </summary>
        public SleepBeat Project(string survivorId, int day) => Project(survivorId, day, hasPhantomPain: false);

        /// <summary>
        /// UNBLOCK-01 / F14-F: Projects the sleep beat with optional phantom-pain awareness.
        /// </summary>
        public SleepBeat Project(string survivorId, int day, bool hasPhantomPain)
        {
            if (string.IsNullOrEmpty(survivorId))
                return new SleepBeat(SleepBeatKind.None, string.Empty, string.Empty, string.Empty);

            var record = _mentalHealth.GetOrCreateRecord(survivorId);
            SleepBeatKind kind = Classify(record, hasPhantomPain);
            string[] lines = kind switch
            {
                SleepBeatKind.Nightmare => NightmareLines,
                SleepBeatKind.PhantomPain => PhantomPainLines,
                SleepBeatKind.Troubled => TroubledLines,
                _ => RestfulLines
            };
            string key = kind switch
            {
                SleepBeatKind.Nightmare => NightmareJournalKey,
                SleepBeatKind.PhantomPain => PhantomPainJournalKey,
                SleepBeatKind.Troubled => TroubledJournalKey,
                _ => RestfulJournalKey
            };

            int index = _rng != null
                ? _rng.Next(0, lines.Length)
                : StableHash.NonNegativeRemainder(day, lines.Length);

            // Per-survivor key: the journal dedupes raw entries by knowledge key,
            // so this records each survivor's sleep trouble once instead of
            // repeating the same line every night.
            string perSurvivorKey = $"{key}:{survivorId}";
            return new SleepBeat(kind, survivorId, perSurvivorKey, lines[index]);
        }

        /// <summary>
        /// Derives the sleep class from the record. Insomnia and an active crisis
        /// dominate trauma; trauma dominates stress.
        /// </summary>
        public static SleepBeatKind Classify(SurvivorMentalHealthRecord? record) => Classify(record, hasPhantomPain: false);

        /// <summary>
        /// UNBLOCK-01 / F14-F: Derives sleep class with optional phantom-pain awareness.
        /// </summary>
        public static SleepBeatKind Classify(SurvivorMentalHealthRecord? record, bool hasPhantomPain)
        {
            if (record == null) return SleepBeatKind.None;

            bool insomnia = record.insomniaDaysRemaining > 0;
            bool crisis = !string.IsNullOrEmpty(record.currentCrisisId) && record.crisisDaysRemaining > 0;
            bool trauma = record.activeTraumaIds != null && record.activeTraumaIds.Count > 0;

            // Nightmare needs a stronger signal than mere sleeplessness:
            // an active crisis, trauma plus insomnia together, or extreme stress.
            if (crisis || (trauma && insomnia) || record.stressPermille >= NightmareStressPermille)
                return SleepBeatKind.Nightmare;
            if (hasPhantomPain)
                return SleepBeatKind.PhantomPain;
            if (insomnia || trauma || record.stressPermille >= TroubledStressPermille)
                return SleepBeatKind.Troubled;
            return SleepBeatKind.Restful;
        }
    }
}
