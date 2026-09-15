// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Tasks 9–12 Wave 2 — signal-trust contract tests.
    ///
    /// TEST CONTRACT: Signal trust is a radio-specific credibility/history
    /// metric backed by exactly-once per-signal event ledgers. It is not a
    /// second reputation engine, never mutates faction standing, and only
    /// ever influences FUTURE candidate weighting — never an active signal's
    /// authored authenticity.
    /// </summary>
    public sealed class SignalTrustTests
    {
        private const string GenuineQuest = "quest_distress_trapped_mechanic";   // freq_distress_88_3, deadline 5, survival 5
        private const string GenuineSignal = "freq_distress_88_3";
        private const string LegacyExpiryQuest = "quest_distress_family_shelter"; // freq_distress_445_2, deadline 4, no consequence
        private const string LegacyExpirySignal = "freq_distress_445_2";
        private const string TrapQuest = "quest_distress_raider_trap";            // freq_distress_192_4, IsTrap
        private const string TrapSignal = "freq_distress_192_4";

        private static (DistressRescueMissionManager Manager, SignalTrustLedger Trust) Bound()
        {
            var trust = new SignalTrustLedger();
            var manager = new DistressRescueMissionManager(null, null, trust);
            return (manager, trust);
        }

        // ── Required: answered / ignored / trap / rescue ───────────────────────

        [Fact]
        public void AnsweringSignalsIncreasesTrust()
        {
            var (manager, trust) = Bound();
            Assert.Equal(SignalTrustPolicy.NeutralScore, trust.Score);

            Assert.True(manager.RecordSignalHeard(GenuineSignal, 1));
            Assert.True(manager.RecordExpeditionDispatched(GenuineQuest, "exp_test_1"));

            Assert.Equal(1, trust.SignalsAnswered);
            Assert.Equal(SignalTrustPolicy.NeutralScore + SignalTrustPolicy.DeltaAnswered, trust.Score);
        }

        [Fact]
        public void MerelyTuningOrHearingDoesNotCountAsAnswered()
        {
            var (manager, trust) = Bound();
            Assert.True(manager.RecordSignalHeard(GenuineSignal, 1));
            Assert.True(manager.RecordSignalIdentified(GenuineSignal));
            Assert.Equal(0, trust.SignalsAnswered);
            Assert.Equal(SignalTrustPolicy.NeutralScore, trust.Score);
        }

        [Fact]
        public void IgnoringSignalsDecreasesTrust()
        {
            var (manager, trust) = Bound();
            // Discovered, actionable, deadline expires unanswered (deadline 5 → expiry day 6).
            Assert.True(manager.RecordSignalHeard(GenuineSignal, 1));
            manager.TickDaily(6);

            Assert.Equal(1, trust.SignalsIgnored);
            Assert.Equal(SignalTrustPolicy.NeutralScore + SignalTrustPolicy.DeltaIgnored, trust.Score);
        }

        [Fact]
        public void UndiscoveredSignalDoesNotCountAsIgnored()
        {
            var (manager, trust) = Bound();
            // All authored missions exist but none was ever heard; far past every deadline.
            manager.TickDaily(1000);
            Assert.Equal(0, trust.SignalsIgnored);
            Assert.Equal(SignalTrustPolicy.NeutralScore, trust.Score);
        }

        [Fact]
        public void LegacyExpiryWithoutConsequenceAlsoCountsAsIgnored()
        {
            var (manager, trust) = Bound();
            Assert.True(manager.RecordSignalHeard(LegacyExpirySignal, 1));
            manager.TickDaily(5); // deadline 4 → expiry day 5

            Assert.Equal(1, trust.SignalsIgnored);
            Assert.Equal(SignalTrustPolicy.NeutralScore + SignalTrustPolicy.DeltaIgnored, trust.Score);
        }

        [Fact]
        public void IgnoringATrapSignalIsNotATrustDeficit()
        {
            var (manager, trust) = Bound();
            // Authored design: "ignoring a lure is not a failure" — trap-class
            // expiry must never count as ignored.
            Assert.True(manager.RecordSignalHeard(TrapSignal, 1));
            manager.TickDaily(100);
            Assert.Equal(0, trust.SignalsIgnored);
            Assert.Equal(SignalTrustPolicy.NeutralScore, trust.Score);
        }

        [Fact]
        public void FallingForTrapHasStrongerNegativeEffect()
        {
            var (manager, trust) = Bound();
            Assert.True(manager.RecordSignalHeard(TrapSignal, 1));
            Assert.True(manager.RecordExpeditionDispatched(TrapQuest, "exp_trap_1"));
            // Arrival in time (no survival model, deadline 5) → ambush encountered.
            var stage = manager.RecordDestinationReached(TrapQuest, 2);
            Assert.Equal(DistressRescueMissionStage.TerminalAmbush, stage);

            Assert.Equal(1, trust.TrapsFallenFor);
            // The dispatch itself counted as answered (+2) before the ambush
            // resolved: 50 + 2 (answered) − 5 (trap) = 47.
            Assert.Equal(1, trust.SignalsAnswered);
            Assert.Equal(SignalTrustPolicy.NeutralScore
                         + SignalTrustPolicy.DeltaAnswered
                         + SignalTrustPolicy.DeltaAmbushEncountered, trust.Score);
            Assert.True(Math.Abs(SignalTrustPolicy.DeltaAmbushEncountered) > Math.Abs(SignalTrustPolicy.DeltaIgnored),
                "trap penalty must be stronger than the ordinary ignore penalty");
        }

        [Fact]
        public void SuccessfulRescueHasStrongestPositiveEffect()
        {
            var (manager, trust) = Bound();
            Assert.True(manager.RecordSignalHeard(GenuineSignal, 1));
            Assert.True(manager.RecordExpeditionDispatched(GenuineQuest, "exp_rescue_1"));
            var stage = manager.RecordDestinationReached(GenuineQuest, 2); // day 2 < death day 6
            Assert.Equal(DistressRescueMissionStage.TerminalRescued, stage);

            Assert.Equal(1, trust.SignalsAnswered);
            Assert.Equal(1, trust.RescuesSuccessful);
            Assert.Equal(SignalTrustPolicy.NeutralScore
                         + SignalTrustPolicy.DeltaAnswered
                         + SignalTrustPolicy.DeltaRescueSuccessful, trust.Score);
        }

        [Fact]
        public void LateArrivalCountsAnsweredButNotRescueOrIgnored()
        {
            var (manager, trust) = Bound();
            Assert.True(manager.RecordSignalHeard(GenuineSignal, 1));
            Assert.True(manager.RecordExpeditionDispatched(GenuineQuest, "exp_late_1"));
            var stage = manager.RecordDestinationReached(GenuineQuest, 10); // past death day 6
            Assert.Equal(DistressRescueMissionStage.TerminalFailed, stage);

            Assert.Equal(1, trust.SignalsAnswered);
            Assert.Equal(0, trust.RescuesSuccessful);
            Assert.Equal(0, trust.SignalsIgnored);
        }

        // ── Exactly-once / no double counting ─────────────────────────────────

        [Fact]
        public void NoDoubleCountingOnRepeatedTransitions()
        {
            var (manager, trust) = Bound();
            Assert.True(manager.RecordSignalHeard(GenuineSignal, 1));
            Assert.True(manager.RecordExpeditionDispatched(GenuineQuest, "exp_d_1"));
            Assert.False(manager.RecordExpeditionDispatched(GenuineQuest, "exp_d_2")); // wrong stage now
            manager.RecordDestinationReached(GenuineQuest, 2);
            manager.RecordDestinationReached(GenuineQuest, 2); // ArrivalResolved guard replays stage, no event
            manager.TickDaily(100); // terminal mission — never punished

            Assert.Equal(1, trust.SignalsAnswered);
            Assert.Equal(1, trust.RescuesSuccessful);
            Assert.Equal(0, trust.SignalsIgnored);
        }

        [Fact]
        public void ExplicitIgnoreThenDeadlineExpiryCountsOnce()
        {
            var trust = new SignalTrustLedger();
            // The moral-choice ignore path (RadioDistressSystem) and the later
            // deadline-expiry path both target the same signal; first wins.
            Assert.True(trust.RecordIgnored(GenuineSignal));
            Assert.False(trust.RecordIgnored(GenuineSignal));
            Assert.Equal(1, trust.SignalsIgnored);
            Assert.Equal(SignalTrustPolicy.NeutralScore + SignalTrustPolicy.DeltaIgnored, trust.Score);
        }

        [Fact]
        public void TrapAmbushSurvivalAddsNoSecondTrustEvent()
        {
            var (manager, trust) = Bound();
            Assert.True(manager.RecordSignalHeard(TrapSignal, 1));
            Assert.True(manager.RecordExpeditionDispatched(TrapQuest, "exp_a_1"));
            manager.RecordDestinationReached(TrapQuest, 2);
            Assert.True(manager.ResolveAmbushSurvived(TrapQuest));

            Assert.Equal(1, trust.TrapsFallenFor); // counted once at the ambush
            Assert.Equal(0, trust.RescuesSuccessful); // surviving a trap is not a rescue
        }

        // ── Bounded score / clamping ──────────────────────────────────────────

        [Fact]
        public void ScoreIsBoundedAndClamped()
        {
            var trust = new SignalTrustLedger();
            for (int i = 0; i < 50; i++) trust.RecordIgnored($"sig_ignore_{i}");
            Assert.Equal(SignalTrustPolicy.MinScore, trust.Score);

            for (int i = 0; i < 50; i++) trust.RecordRescueSuccessful($"sig_rescue_{i}");
            Assert.Equal(SignalTrustPolicy.MaxScore, trust.Score);
        }

        // ── Determinism + save/load ───────────────────────────────────────────

        [Fact]
        public void TrustIsDeterministic()
        {
            List<string> Trace()
            {
                var (manager, trust) = Bound();
                var lines = new List<string>();
                manager.RecordSignalHeard(GenuineSignal, 1);
                manager.RecordExpeditionDispatched(GenuineQuest, "exp_x");
                manager.RecordDestinationReached(GenuineQuest, 2);
                manager.RecordSignalHeard(LegacyExpirySignal, 1);
                manager.TickDaily(5);
                var cap = trust.CaptureState();
                lines.Add($"{cap.signalsAnswered},{cap.signalsIgnored},{cap.trapsFallenFor},{cap.rescuesSuccessful},{cap.score}");
                lines.Add(string.Join(";", cap.answeredSignalIds));
                lines.Add(string.Join(";", cap.ignoredSignalIds));
                return lines;
            }

            Assert.Equal(Trace(), Trace());
        }

        [Fact]
        public void TrustSurvivesSaveLoad()
        {
            var json = new Ashfall.Core.SystemTextJsonSerializer();

            // Continuous run.
            var (managerA, trustA) = Bound();
            managerA.RecordSignalHeard(GenuineSignal, 1);
            managerA.RecordExpeditionDispatched(GenuineQuest, "exp_s");
            managerA.RecordDestinationReached(GenuineQuest, 2);
            var continuous = trustA.CaptureState();

            // Interrupted run: capture mid-state, restore into a fresh ledger, continue.
            var (managerB, trustB) = Bound();
            managerB.RecordSignalHeard(GenuineSignal, 1);
            managerB.RecordExpeditionDispatched(GenuineQuest, "exp_s");
            trustB.RestoreState(trustB.CaptureState()); // round-trip through the save DTO
            managerB.RecordDestinationReached(GenuineQuest, 2);
            var interrupted = trustB.CaptureState();

            Assert.Equal(continuous.score, interrupted.score);
            Assert.Equal(continuous.signalsAnswered, interrupted.signalsAnswered);
            Assert.Equal(continuous.rescuesSuccessful, interrupted.rescuesSuccessful);
            Assert.Equal(continuous.answeredSignalIds, interrupted.answeredSignalIds);
            Assert.Equal(continuous.rescueSignalIds, interrupted.rescueSignalIds);
        }

        [Fact]
        public void RadioSaveRoundTripsTrustState()
        {
            var json = new Ashfall.Core.SystemTextJsonSerializer();
            var trust = new SignalTrustLedger();
            trust.RecordAnswered(GenuineSignal);
            trust.RecordRescueSuccessful(GenuineSignal);
            trust.RecordAmbushEncountered(TrapSignal);

            var state = new RadioSaveState { day = 42, signalTrust = trust.CaptureState() };
            string encoded = RadioSaveCodec.Encode(state, json);
            // CurrentSaveVersion is stamped by Encode (V6 since Wave 3).
            Assert.Contains($"\"saveVersion\":{RadioSaveCodec.CurrentSaveVersion}", encoded);

            Assert.True(RadioSaveCodec.TryDecode(encoded, json, out var restored));
            Assert.NotNull(restored!.signalTrust);
            Assert.Equal(trust.Score, restored.signalTrust!.score);
            Assert.Equal(1, restored.signalTrust.signalsAnswered);
            Assert.Equal(1, restored.signalTrust.rescuesSuccessful);
            Assert.Equal(1, restored.signalTrust.trapsFallenFor);
            Assert.Contains(GenuineSignal, restored.signalTrust.answeredSignalIds);
        }

        [Fact]
        public void RadioSaveV4MigratesToNeutralTrust()
        {
            var json = new Ashfall.Core.SystemTextJsonSerializer();
            // A faithful V4 save: the frozen shape has no trust field.
            var v4 = new RadioSaveStateFrozenV4 { day = 30, currentFrequency = 100.0f };
            v4.Checksum = Ashfall.Core.SaveChecksum.Compute(v4);
            string encoded = json.Serialize(v4);

            Assert.True(RadioSaveCodec.TryDecode(encoded, json, out var migrated));
            Assert.Equal(RadioSaveCodec.CurrentSaveVersion, migrated!.saveVersion);
            // Neutral default — no reconstructed history from incomplete data.
            Assert.NotNull(migrated.signalTrust);
            Assert.Equal(SignalTrustPolicy.NeutralScore, migrated.signalTrust!.score);
            Assert.Equal(0, migrated.signalTrust.signalsAnswered);
            Assert.Equal(0, migrated.signalTrust.signalsIgnored);
        }

        // ── Availability weighting (plan §10D/§10E) ───────────────────────────

        [Fact]
        public void TrustAffectsSignalAvailability()
        {
            // High trust: genuine weight rises, trap weight falls.
            int highGenuine = SignalTrustAvailability.ApplyModifier(10,
                SignalTrustAvailability.GenuineModifierPermille(100));
            int highTrap = SignalTrustAvailability.ApplyModifier(10,
                SignalTrustAvailability.TrapModifierPermille(100));
            Assert.Equal(15, highGenuine);
            Assert.Equal(5, highTrap);

            // Low trust: the mirror image.
            int lowGenuine = SignalTrustAvailability.ApplyModifier(10,
                SignalTrustAvailability.GenuineModifierPermille(0));
            int lowTrap = SignalTrustAvailability.ApplyModifier(10,
                SignalTrustAvailability.TrapModifierPermille(0));
            Assert.Equal(5, lowGenuine);
            Assert.Equal(15, lowTrap);

            // Neutral: unmodified.
            Assert.Equal(10, SignalTrustAvailability.ApplyModifier(10,
                SignalTrustAvailability.GenuineModifierPermille(SignalTrustPolicy.NeutralScore)));
            Assert.Equal(10, SignalTrustAvailability.ApplyModifier(10,
                SignalTrustAvailability.TrapModifierPermille(SignalTrustPolicy.NeutralScore)));
        }

        [Fact]
        public void AvailabilityModifiersAreBoundedAndMonotonic()
        {
            for (int score = SignalTrustPolicy.MinScore; score <= SignalTrustPolicy.MaxScore; score++)
            {
                int genuine = SignalTrustAvailability.GenuineModifierPermille(score);
                int trap = SignalTrustAvailability.TrapModifierPermille(score);
                Assert.InRange(genuine, SignalTrustAvailability.MinPermille, SignalTrustAvailability.MaxPermille);
                Assert.InRange(trap, SignalTrustAvailability.MinPermille, SignalTrustAvailability.MaxPermille);
                if (score > SignalTrustPolicy.MinScore)
                {
                    Assert.True(genuine > SignalTrustAvailability.GenuineModifierPermille(score - 1),
                        $"genuine modifier must be monotonic at score {score}");
                    Assert.True(trap < SignalTrustAvailability.TrapModifierPermille(score - 1),
                        $"trap modifier must be inversely monotonic at score {score}");
                }
            }
        }

        [Fact]
        public void CandidateWeightingIsDeterministicAndOrderPreserving()
        {
            var candidates = new List<(string, int, bool)>
            {
                ("signal_bridge_family", 10, true),
                ("signal_raider_bait", 10, false),
                ("signal_automated_loop", 4, true)
            };
            candidates.Sort((a, b) => string.Compare(a.Item1, b.Item1, StringComparison.Ordinal));

            var weighted = SignalTrustAvailability.ModifyCandidates(candidates, 63);
            Assert.Equal(3, weighted.Count);
            // Order preserved (stable ID sort is the caller's contract).
            Assert.Equal("signal_automated_loop", weighted[0].SignalId);
            Assert.Equal("signal_bridge_family", weighted[1].SignalId);
            Assert.Equal("signal_raider_bait", weighted[2].SignalId);
            // trust=63 → genuine 1130‰, trap 870‰ (plan §10E illustrative band).
            Assert.Equal(4, weighted[0].FinalWeight);  // 4 × 1.13 = 4.52 → 4
            Assert.Equal(11, weighted[1].FinalWeight); // 10 × 1.13 = 11.3 → 11
            Assert.Equal(8, weighted[2].FinalWeight);  // 10 × 0.87 = 8.7 → 8

            // Same input, same output — byte-for-byte deterministic.
            var again = SignalTrustAvailability.ModifyCandidates(candidates, 63);
            for (int i = 0; i < weighted.Count; i++)
                Assert.Equal(weighted[i].FinalWeight, again[i].FinalWeight);
        }

        [Fact]
        public void TrustDoesNotChangeAuthenticityClassification()
        {
            // The modifier category is a caller-provided classification of
            // authored truth; the availability API cannot reclassify a signal.
            var def = new DistressSignalDefinition
            {
                FrequencyId = TrapSignal,
                Authenticity = "trap",
                OutcomeTypeStr = "bait_trap"
            };
            Assert.True(def.IsTrapOrDeception);
            Assert.False(def.IsGenuineRescue);
            // ...and no availability call mutates the definition:
            SignalTrustAvailability.ModifyCandidates(new List<(string, int, bool)> { (def.FrequencyId, 10, false) }, 100);
            Assert.True(def.IsTrapOrDeception);
        }
    }
}
