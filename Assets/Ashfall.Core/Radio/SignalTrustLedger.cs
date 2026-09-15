// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// Tasks 9–12 Wave 2 — centralized signal-trust delta policy.
    ///
    /// <para>Signal trust is a RADIO-SPECIFIC credibility/history metric (the
    /// player's accumulated track record with distress calls), NOT a second
    /// general reputation system. It must never be confused with faction
    /// standing (<see cref="Ashfall.Core.YearOfAsh.FactionWarSystem"/> is
    /// faction-keyed only and keeps its own semantics), NPC relationships,
    /// quest reputation, or morality. Deltas exist ONLY here — no event
    /// handler may scatter numeric trust changes.</para>
    ///
    /// <para>Scale: integer [0, 100], neutral default 50. All deltas are
    /// integers; the availability modifiers are integer permille — no
    /// floating-point drift anywhere.</para>
    /// </summary>
    public static class SignalTrustPolicy
    {
        public const int MinScore = 0;
        public const int MaxScore = 100;
        public const int NeutralScore = 50;

        /// <summary>Player dispatched an expedition to an actionable signal.</summary>
        public const int DeltaAnswered = +2;

        /// <summary>The rescue authority resolved a rescue as successful.</summary>
        public const int DeltaRescueSuccessful = +5;

        /// <summary>A discovered, actionable signal expired unanswered or was explicitly declined.</summary>
        public const int DeltaIgnored = -2;

        /// <summary>The player dispatched into an authored trap and the ambush was encountered.</summary>
        public const int DeltaAmbushEncountered = -5;

        public static int Clamp(int score) => Math.Clamp(score, MinScore, MaxScore);
    }

    /// <summary>Save DTO for the signal-trust ledger (rides the radio save section).</summary>
    [Serializable]
    public sealed class SignalTrustSaveEntry
    {
        public int signalsAnswered;
        public int signalsIgnored;
        public int trapsFallenFor;
        public int rescuesSuccessful;
        public int score = SignalTrustPolicy.NeutralScore;
        public List<string> answeredSignalIds = new List<string>();
        public List<string> ignoredSignalIds = new List<string>();
        public List<string> trapSignalIds = new List<string>();
        public List<string> rescueSignalIds = new List<string>();
    }

    /// <summary>
    /// Tasks 9–12 Wave 2 — radio-owned signal-trust ledger (fallback path B of
    /// the plan's storage decision gate: the existing standing store is
    /// faction-keyed only, so a minimal scoped data state is correct).
    ///
    /// <para>Properties: data-only, deterministic, engine-free, owned by the
    /// radio/distress runtime, free of UI/AudioManager dependencies. Every
    /// record method is exactly-once PER SIGNAL (first event wins, mirroring
    /// the DistressMissionSaveState.ClaimedReceipts pattern) so path overlaps
    /// (e.g. an explicit moral-choice ignore followed by a deadline expiry)
    /// can never double-count.</para>
    ///
    /// <para>Event definitions (plan §10B — normative):</para>
    /// <list type="bullet">
    /// <item><b>Answered</b>: the player performed the authoritative response
    /// action — an expedition dispatch through
    /// <see cref="DistressRescueMissionManager.RecordExpeditionDispatched"/>.
    /// Tuning, panel opens, or discovery never count.</item>
    /// <item><b>Ignored</b>: the signal was discovered and actionable and the
    /// player explicitly declined (moral-choice ignore) OR the authored
    /// response window expired unanswered (mission-manager daily tick).
    /// Undiscovered signals never count.</item>
    /// <item><b>Trap fallen for</b>: the player dispatched into an authored
    /// trap and the arrival actually resolved the ambush
    /// (TerminalAmbush). Merely detecting or analyzing a trap never counts;
    /// surviving the ambush afterwards adds no second event.</item>
    /// <item><b>Rescue successful</b>: the rescue authority resolved the
    /// mission TerminalRescued (arrival in time). Not inferred from arrival
    /// dispatch alone; late arrivals (TerminalFailed) count only as answered.</item>
    /// </list>
    /// </summary>
    public sealed class SignalTrustLedger
    {
        private readonly HashSet<string> _answered = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _ignored = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _traps = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _rescues = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public int SignalsAnswered { get; private set; }
        public int SignalsIgnored { get; private set; }
        public int TrapsFallenFor { get; private set; }
        public int RescuesSuccessful { get; private set; }
        public int Score { get; private set; } = SignalTrustPolicy.NeutralScore;

        public SignalTrustLedger() { }

        /// <summary>Records the player answering a signal (authoritative dispatch). Exactly once per signal.</summary>
        public bool RecordAnswered(string signalId)
        {
            if (string.IsNullOrWhiteSpace(signalId) || !_answered.Add(signalId)) return false;
            SignalsAnswered++;
            Score = SignalTrustPolicy.Clamp(Score + SignalTrustPolicy.DeltaAnswered);
            return true;
        }

        /// <summary>Records an ignored signal (explicit decline or actionable expiry). Exactly once per signal.</summary>
        public bool RecordIgnored(string signalId)
        {
            if (string.IsNullOrWhiteSpace(signalId) || !_ignored.Add(signalId)) return false;
            SignalsIgnored++;
            Score = SignalTrustPolicy.Clamp(Score + SignalTrustPolicy.DeltaIgnored);
            return true;
        }

        /// <summary>Records falling for a trap (ambush actually encountered). Exactly once per signal.</summary>
        public bool RecordAmbushEncountered(string signalId)
        {
            if (string.IsNullOrWhiteSpace(signalId) || !_traps.Add(signalId)) return false;
            TrapsFallenFor++;
            Score = SignalTrustPolicy.Clamp(Score + SignalTrustPolicy.DeltaAmbushEncountered);
            return true;
        }

        /// <summary>Records a successful rescue resolution. Exactly once per signal.</summary>
        public bool RecordRescueSuccessful(string signalId)
        {
            if (string.IsNullOrWhiteSpace(signalId) || !_rescues.Add(signalId)) return false;
            RescuesSuccessful++;
            Score = SignalTrustPolicy.Clamp(Score + SignalTrustPolicy.DeltaRescueSuccessful);
            return true;
        }

        public SignalTrustSaveEntry CaptureState()
        {
            return new SignalTrustSaveEntry
            {
                signalsAnswered = SignalsAnswered,
                signalsIgnored = SignalsIgnored,
                trapsFallenFor = TrapsFallenFor,
                rescuesSuccessful = RescuesSuccessful,
                score = Score,
                answeredSignalIds = new List<string>(_answered),
                ignoredSignalIds = new List<string>(_ignored),
                trapSignalIds = new List<string>(_traps),
                rescueSignalIds = new List<string>(_rescues)
            };
        }

        public void RestoreState(SignalTrustSaveEntry? entry)
        {
            if (entry == null) return; // old save / absent section → neutral default
            SignalsAnswered = Math.Max(0, entry.signalsAnswered);
            SignalsIgnored = Math.Max(0, entry.signalsIgnored);
            TrapsFallenFor = Math.Max(0, entry.trapsFallenFor);
            RescuesSuccessful = Math.Max(0, entry.rescuesSuccessful);
            Score = SignalTrustPolicy.Clamp(entry.score);
            Replace(_answered, entry.answeredSignalIds);
            Replace(_ignored, entry.ignoredSignalIds);
            Replace(_traps, entry.trapSignalIds);
            Replace(_rescues, entry.rescueSignalIds);
        }

        private static void Replace(HashSet<string> set, List<string>? ids)
        {
            set.Clear();
            if (ids == null) return;
            foreach (var id in ids)
                if (!string.IsNullOrWhiteSpace(id)) set.Add(id);
        }
    }
}
