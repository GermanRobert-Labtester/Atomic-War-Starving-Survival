// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Tasks 9–12 Wave 4 — distress-audio contract tests.
    ///
    /// TEST CONTRACT: Distress-signal audio is a presentation projection of
    /// authoritative radio state. Playback must never determine discovery,
    /// stage progression, trust, quest state, follow-up scheduling, or
    /// encounter outcomes.
    /// </summary>
    public sealed class DistressAudioCueTests : CatalogTestBase
    {
        private static DistressSignalDefinition Fixture(
            string? signalCue = null,
            (int day, float clarity, string cue)? stage1 = null,
            (int day, float clarity, string cue)? stage2 = null)
        {
            var def = new DistressSignalDefinition
            {
                FrequencyId = "freq_test_audio",
                FrequencyMhzStr = "199.7",
                SourceName = "Audio Fixture",
                AudioCue = signalCue ?? string.Empty
            };
            def.MessageFragments.Add(new DistressMessageFragment { Day = 1, Clarity = 0.25f, Text = "weak" });
            if (stage1.HasValue)
                def.MessageFragments.Add(new DistressMessageFragment { Day = stage1.Value.day, Clarity = stage1.Value.clarity, Text = "mid", AudioCue = stage1.Value.cue });
            if (stage2.HasValue)
                def.MessageFragments.Add(new DistressMessageFragment { Day = stage2.Value.day, Clarity = stage2.Value.clarity, Text = "clear", AudioCue = stage2.Value.cue });
            return def;
        }

        // ── Required: resolution precedence + determinism ─────────────────────

        [Fact]
        public void SignalAudioCueIsDeterministic()
        {
            var def = Fixture(signalCue: "radio_static", stage1: (3, 0.5f, "radio_morse"));
            // Same signal + same stage → same cue, every call.
            for (int i = 0; i < 5; i++)
                Assert.Equal("radio_morse", DistressAudioCueResolver.Resolve(def, 1));
            // Day 3 resolves stage 1 → its override (deterministic across calls).
            Assert.Equal("radio_morse", DistressAudioCueResolver.ResolveForDay(def, 3));
            Assert.Equal("radio_morse", DistressAudioCueResolver.ResolveForDay(def, 3));
            // Day 1 resolves stage 0 → the signal default.
            Assert.Equal("radio_static", DistressAudioCueResolver.ResolveForDay(def, 1));
        }

        [Fact]
        public void AudioCueChangesAtDifferentClarityLevels()
        {
            var def = Fixture(
                signalCue: "radio_static",
                stage1: (3, 0.5f, "radio_morse"),
                stage2: (5, 0.9f, "radio_distress_beacon"));

            // Stage 0 (day 1): no override authored → signal default.
            Assert.Equal("radio_static", DistressAudioCueResolver.ResolveForDay(def, 1));
            // Stage 1 (day 3): stage override wins over the default.
            Assert.Equal("radio_morse", DistressAudioCueResolver.ResolveForDay(def, 3));
            // Stage 2 (day 5): next override.
            Assert.Equal("radio_distress_beacon", DistressAudioCueResolver.ResolveForDay(def, 5));
            // Multi-day skip: cue follows the resolved stage.
            Assert.Equal("radio_distress_beacon", DistressAudioCueResolver.ResolveForDay(def, 30));
        }

        [Fact]
        public void StageOverridePrecedenceFollowsPlanOptionA()
        {
            // Override present → wins.
            var withOverride = Fixture(signalCue: "radio_static", stage1: (3, 0.5f, "radio_morse"));
            Assert.Equal("radio_morse", DistressAudioCueResolver.Resolve(withOverride, 1));

            // Override empty → inherits the signal default.
            var withoutOverride = Fixture(signalCue: "radio_static", stage1: (3, 0.5f, ""));
            Assert.Equal("radio_static", DistressAudioCueResolver.Resolve(withoutOverride, 1));

            // No signal cue, no override → text-only fallback (empty).
            var bare = Fixture();
            Assert.Equal(string.Empty, DistressAudioCueResolver.ResolveForDay(bare, 10));
        }

        [Fact]
        public void MissingOrUnknownSignalResolvesToTextOnly()
        {
            Assert.Equal(string.Empty, DistressAudioCueResolver.Resolve(null, 0));
            Assert.Equal(string.Empty, DistressAudioCueResolver.ResolveForDay(null, 5));
            var empty = new DistressSignalDefinition { FrequencyId = "freq_test_bare_audio" };
            Assert.Equal(string.Empty, DistressAudioCueResolver.Resolve(empty, 0));
            // Out-of-range stage index falls back to the signal default, never throws.
            var def = Fixture(signalCue: "radio_static");
            Assert.Equal("radio_static", DistressAudioCueResolver.Resolve(def, 99));
            Assert.Equal("radio_static", DistressAudioCueResolver.Resolve(def, -1));
        }

        [Fact]
        public void SignalAudioCueSurvivesSaveLoad()
        {
            // The cue is DERIVED from definition + stage — nothing audio-related
            // is persisted. Save/load of the definition JSON reproduces the cue.
            string json = """
            {
              "schema_version": 1,
              "radio_broadcasts": [
                {
                  "frequency_id": "freq_test_audio_bind",
                  "frequency_mhz": "101.5",
                  "source_name": "Audio Bind",
                  "outcome_type": "survivor_isolated",
                  "revealed_location": "loc_test_validator",
                  "audio_cue": "radio_static",
                  "message_fragments": [
                    { "day": 1, "clarity": 0.2, "text": "weak" },
                    { "day": 3, "clarity": 0.6, "text": "clear", "audio_cue": "radio_morse" }
                  ],
                  "follow_up_signals": [
                    { "id": "fu_audio", "trigger_condition": "answered", "delay_days": 2, "text": "Later.", "audio_cue": "radio_distress_beacon" }
                  ]
                }
              ]
            }
            """;
            var system = new RadioDistressSystem();
            Assert.Equal(1, system.LoadFromJson(json));
            var def = system.GetDefinition("freq_test_audio_bind");
            Assert.NotNull(def);

            // Resolve → "save" (re-load the same JSON into a fresh system) → resolve again.
            string before = DistressAudioCueResolver.ResolveForDay(def, 4);
            var system2 = new RadioDistressSystem();
            system2.LoadFromJson(json);
            string after = DistressAudioCueResolver.ResolveForDay(system2.GetDefinition("freq_test_audio_bind"), 4);
            Assert.Equal(before, after);
            Assert.Equal("radio_morse", before);

            // Follow-up cue binds too.
            Assert.Equal("radio_distress_beacon", def.FollowUpSignals[0].AudioCue);
        }

        [Fact]
        public void FollowUpCarriesItsOwnCue()
        {
            var followUp = new SignalFollowUpDefinition
            {
                Id = "fu_cue",
                TriggerCondition = SignalFollowUpTriggers.Answered,
                DelayDays = 1,
                Text = "Distinct transmission.",
                AudioCue = "radio_vo_ch3_ash_road"
            };
            Assert.Equal("radio_vo_ch3_ash_road", followUp.AudioCue);
            // Default is text-only.
            var bare = new SignalFollowUpDefinition { Id = "fu_bare", TriggerCondition = "answered", DelayDays = 1, Text = "x" };
            Assert.Equal(string.Empty, bare.AudioCue);
        }

        // ── Detection gate (the host plays only on Intercept success) ─────────

        [Fact]
        public void InterceptIsTheSingleDetectionGate()
        {
            // The host plays audio only when Intercept returns true (the
            // Inactive → Intercepted transition). This test pins that gate:
            // repeat calls, already-active signals, and restored states never
            // re-cross it.
            var system = new RadioDistressSystem();
            var def = Fixture(signalCue: "radio_static");
            system.RegisterSignal(def);

            Assert.True(system.Intercept("freq_test_audio", 1));  // detection edge
            Assert.False(system.Intercept("freq_test_audio", 2)); // already active — no second edge
            Assert.False(system.Intercept("freq_test_audio", 3));
        }

        [Fact]
        public void AudioResolutionNeverMutatesGameState()
        {
            var def = Fixture(signalCue: "radio_static", stage1: (3, 0.5f, "radio_morse"));
            string before = def.AudioCue;
            var stageBefore = def.MessageFragments[1].AudioCue;
            int indexBefore = DistressStageResolver.ResolveStageIndex(def, 10);

            _ = DistressAudioCueResolver.ResolveForDay(def, 10);
            _ = DistressAudioCueResolver.Resolve(def, 1);

            Assert.Equal(before, def.AudioCue);
            Assert.Equal(stageBefore, def.MessageFragments[1].AudioCue);
            Assert.Equal(indexBefore, DistressStageResolver.ResolveStageIndex(def, 10));
            // Trust/quest/expedition state do not exist on the definition —
            // there is nothing to mutate by construction.
        }

        // ── Structural validation ─────────────────────────────────────────────

        [Fact]
        public void Validator_AcceptsWellFormedAudioCuesAndRealCatalogsStayClean()
        {
            string json = """
            {
              "schema_version": 1,
              "radio_broadcasts": [
                {
                  "frequency_id": "freq_test_cue_ok",
                  "frequency_mhz": "101.5",
                  "source_name": "Cue OK",
                  "outcome_type": "survivor_isolated",
                  "revealed_location": "loc_test_validator",
                  "audio_cue": "radio_static",
                  "message_fragments": [
                    { "day": 1, "clarity": 0.2, "text": "a", "audio_cue": "radio_morse" }
                  ],
                  "follow_up_signals": [
                    { "id": "fu_cue_ok", "trigger_condition": "answered", "delay_days": 1, "text": "x", "audio_cue": "radio_distress_beacon" }
                  ]
                }
              ]
            }
            """;
            var (dir, _) = WriteTempCatalog(json);
            try
            {
                var report = new Ashfall.Core.CatalogIntegrityReport();
                Ashfall.Core.CatalogIntegrityValidator.ValidateDistressSignalStages(dir, new Ashfall.Core.FileSystemIO(), report);
                Assert.Empty(report.Errors);
            }
            finally { DisposeTemp(dir); }

            // Real catalogs: no audio authored yet → zero audio-cue errors.
            var real = new Ashfall.Core.CatalogIntegrityReport();
            Ashfall.Core.CatalogIntegrityValidator.ValidateDistressSignalStages(DataDirectory, new Ashfall.Core.FileSystemIO(), real);
            Assert.DoesNotContain(real.Errors, e => e.Contains("audio_cue"));
        }

        [Fact]
        public void Validator_RejectsMalformedAudioCueIds()
        {
            string json = """
            {
              "schema_version": 1,
              "radio_broadcasts": [
                {
                  "frequency_id": "freq_test_cue_bad",
                  "frequency_mhz": "101.5",
                  "source_name": "Cue Bad",
                  "outcome_type": "survivor_isolated",
                  "revealed_location": "loc_test_validator",
                  "message_fragments": [
                    { "day": 1, "clarity": 0.2, "text": "a", "audio_cue": "Radio Static 2" }
                  ]
                }
              ]
            }
            """;
            var (dir, _) = WriteTempCatalog(json);
            try
            {
                var report = new Ashfall.Core.CatalogIntegrityReport();
                Ashfall.Core.CatalogIntegrityValidator.ValidateDistressSignalStages(dir, new Ashfall.Core.FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("audio_cue 'Radio Static 2'"));
            }
            finally { DisposeTemp(dir); }
        }

        private static (string Dir, string FilePath) WriteTempCatalog(string json)
        {
            string dir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "ashfall-audio-validator-" + Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(dir);
            string path = System.IO.Path.Combine(dir, "radio_distress_signals.json");
            System.IO.File.WriteAllText(path, json);
            return (dir, path);
        }

        private static void DisposeTemp(string dir)
        {
            try { System.IO.Directory.Delete(dir, recursive: true); } catch { /* best effort */ }
        }
    }
}
