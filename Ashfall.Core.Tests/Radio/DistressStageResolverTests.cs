// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Tasks 9–12 Wave 1 — distress-signal stage contract tests.
    ///
    /// TEST CONTRACT: A message stage is a deterministic presentation/intelligence
    /// view of one canonical distress signal. It is not quest progression and must
    /// not mutate quest, expedition, trust, or encounter state.
    ///
    /// Stage authority is <c>message_fragments</c> (Wave 0 decision gate PC-2 —
    /// no redundant <c>stages</c> field). Fragment <c>day</c> is an ABSOLUTE
    /// campaign-day threshold; selection is purely derivable, so no stage index
    /// is persisted. Stage days are strictly ascending and clarity
    /// non-decreasing (validator-enforced), so stages can never regress.
    /// </summary>
    public sealed class DistressStageResolverTests : CatalogTestBase
    {
        private static DistressSignalDefinition Fixture(int[] days, float[] clarities, string[]? hints = null)
        {
            var fragments = new List<DistressMessageFragment>();
            for (int i = 0; i < days.Length; i++)
            {
                fragments.Add(new DistressMessageFragment
                {
                    Day = days[i],
                    Clarity = clarities[i],
                    Text = $"stage {i} transmission",
                    OutcomeHint = hints != null && i < hints.Length ? hints[i] : string.Empty
                });
            }
            return new DistressSignalDefinition
            {
                FrequencyId = "freq_test_stage_fixture",
                FrequencyMhzStr = "199.9",
                SourceName = "Stage Fixture",
                MessageFragments = fragments
            };
        }

        // ── Task 9 required tests ──────────────────────────────────────────────

        [Fact]
        public void SignalProgressesThroughAllStages()
        {
            var signal = Fixture(new[] { 1, 3, 5 }, new[] { 0.25f, 0.5f, 0.9f });

            // Before first threshold: legacy fallback keeps stage 0 audible.
            Assert.Equal(0, DistressStageResolver.ResolveStageIndex(signal, 0));
            Assert.Equal(0, DistressStageResolver.ResolveStageIndex(signal, 1));
            Assert.Equal(1, DistressStageResolver.ResolveStageIndex(signal, 3));
            Assert.Equal(2, DistressStageResolver.ResolveStageIndex(signal, 5));
            // Multi-day skip advances directly; never selects a future stage.
            Assert.Equal(2, DistressStageResolver.ResolveStageIndex(signal, 10));
            Assert.Equal(2, DistressStageResolver.ResolveStageIndex(signal, 1000));
        }

        [Fact]
        public void SignalClarityIncreasesOverTime()
        {
            var system = LoadAuthoritativeCatalog();
            var jsonBacked = SignalsWithFragments(system);
            Assert.True(jsonBacked.Count >= 43, $"expected >= 43 fragment-backed signals, got {jsonBacked.Count}");

            var failures = new List<string>();
            foreach (var signal in jsonBacked)
            {
                float? previous = null;
                for (int day = 0; day <= 60; day++)
                {
                    var stage = DistressStageResolver.Resolve(signal, day);
                    if (stage == null) { failures.Add($"{signal.FrequencyId}: no stage resolved"); break; }
                    float clarity = stage.Value.Fragment.Clarity;
                    if (previous.HasValue && clarity < previous.Value)
                        failures.Add($"{signal.FrequencyId}: clarity regressed {previous.Value} -> {clarity} at day {day}");
                    previous = clarity;
                }
            }
            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void SignalTextChangesAtEachStage()
        {
            var system = LoadAuthoritativeCatalog();
            var failures = new List<string>();
            int changingSignals = 0;
            foreach (var signal in SignalsWithFragments(system))
            {
                string? previous = null;
                bool changed = false;
                for (int day = 0; day <= 60; day++)
                {
                    var stage = DistressStageResolver.Resolve(signal, day);
                    if (stage == null) { failures.Add($"{signal.FrequencyId}: no stage resolved"); break; }
                    string text = stage.Value.Fragment.Text;
                    // Resolver fidelity: the selected text is always the authored
                    // fragment text for the resolved index.
                    Assert.Equal(signal.MessageFragments[stage.Value.StageIndex].Text, text);
                    if (previous != null && text != previous) changed = true;
                    previous = text;
                }
                if (changed) changingSignals++;
            }
            // All but the one authored automated-loop signal (freq_distress_367_9,
            // repeated content structure with rising clarity) change text over time.
            Assert.True(changingSignals >= jsonBackedCount(system) - 1,
                $"expected all but one signal to change text, got {changingSignals}/{jsonBackedCount(system)}");
        }

        [Fact]
        public void OutcomeHintsRevealAtExpectedStages()
        {
            var signal = Fixture(
                new[] { 1, 3, 5 },
                new[] { 0.25f, 0.5f, 0.9f },
                new[] { string.Empty, string.Empty, "The coordinates repeat every 30 seconds." });

            // Early stages: hint absent.
            Assert.Equal(string.Empty, DistressStageResolver.Resolve(signal, 1)!.Value.Fragment.OutcomeHint);
            Assert.Equal(string.Empty, DistressStageResolver.Resolve(signal, 3)!.Value.Fragment.OutcomeHint);
            // Authored later hint appears exactly at its threshold.
            Assert.Equal("The coordinates repeat every 30 seconds.",
                DistressStageResolver.Resolve(signal, 5)!.Value.Fragment.OutcomeHint);
            Assert.Equal("The coordinates repeat every 30 seconds.",
                DistressStageResolver.Resolve(signal, 9)!.Value.Fragment.OutcomeHint);
        }

        [Fact]
        public void MultiStageProgressionIsDeterministic()
        {
            var signal = Fixture(new[] { 1, 3, 5, 8 }, new[] { 0.2f, 0.45f, 0.7f, 0.95f });

            int[] Trace()
            {
                var trace = new int[64];
                for (int day = 0; day < trace.Length; day++)
                    trace[day] = DistressStageResolver.ResolveStageIndex(signal, day);
                return trace;
            }

            var a = Trace();
            var b = Trace();
            Assert.Equal(a, b);
        }

        [Fact]
        public void NoFragmentSignalReturnsLegacyFallbackSentinel()
        {
            var bare = new DistressSignalDefinition { FrequencyId = "freq_test_bare", SourceName = "Bare" };
            Assert.Null(DistressStageResolver.Resolve(bare, 5));
            Assert.Equal(-1, DistressStageResolver.ResolveStageIndex(bare, 5));
        }

        // ── Parity with the legacy duplicated consumers (Wave 0 R13) ──────────

        /// <summary>
        /// Reference oracle: the exact selection logic that was duplicated in
        /// RadioTuner.EvaluateFrequency and RadioPropagation.Evaluate before the
        /// Wave 1 consolidation. The resolver must match it byte-for-byte for
        /// every authored signal across the day sweep.
        /// </summary>
        private static int LegacySelectionIndex(DistressSignalDefinition signal, int day)
        {
            int fragIdx = -1;
            for (int i = 0; i < signal.MessageFragments.Count; i++)
            {
                var frag = signal.MessageFragments[i];
                if (frag.Day <= day)
                {
                    if (fragIdx == -1 || frag.Day > signal.MessageFragments[fragIdx].Day)
                    {
                        fragIdx = i;
                    }
                }
            }
            if (fragIdx == -1) fragIdx = 0;
            return fragIdx;
        }

        [Fact]
        public void StageSelectionMatchesLegacyConsumersForAllAuthoredSignals()
        {
            var system = LoadAuthoritativeCatalog();
            var failures = new List<string>();
            foreach (var signal in SignalsWithFragments(system))
            {
                for (int day = 0; day <= 60; day++)
                {
                    int expected = LegacySelectionIndex(signal, day);
                    int actual = DistressStageResolver.ResolveStageIndex(signal, day);
                    if (expected != actual)
                    {
                        failures.Add($"{signal.FrequencyId} day {day}: legacy={expected} resolver={actual}");
                    }
                }
            }
            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        private static List<DistressSignalDefinition> SignalsWithFragments(RadioDistressSystem system)
            => system.Definitions.Where(d => d.MessageFragments.Count > 0).ToList();

        private static int jsonBackedCount(RadioDistressSystem system) => SignalsWithFragments(system).Count;

        // ── Catalog load + primary-wins authority ─────────────────────────────

        /// <summary>
        /// Host-order load (expansion first, primary Plan 50 authority last) —
        /// the documented primary-wins pattern from RadioHostSession.
        /// </summary>
        private static RadioDistressSystem LoadAuthoritativeCatalog()
        {
            var system = new RadioDistressSystem();
            string expPath = Path.Combine(DataDirectory, "radio_distress_signals_expansion.json");
            if (File.Exists(expPath))
                system.LoadFromJson(File.ReadAllText(expPath));
            string basePath = Path.Combine(DataDirectory, "radio_distress_signals.json");
            Assert.True(File.Exists(basePath), "primary distress catalog missing");
            system.LoadFromJson(File.ReadAllText(basePath));
            return system;
        }

        [Fact]
        public void AllAuthoredSignalsLoadWithMultiStageFragments()
        {
            var system = LoadAuthoritativeCatalog();
            // 25 primary + 23 expansion rows − 5 documented cross-file overrides
            // = 43 unique JSON identities, PLUS 4 builtin-only fragment-less
            // fallback signals (108_9, 134_5, 162_1, 124_7) = 47 registered.
            Assert.Equal(47, system.TotalRegisteredSignals);
            Assert.Equal(43, jsonBackedCount(system));
            foreach (var signal in SignalsWithFragments(system))
            {
                Assert.True(signal.MessageFragments.Count >= 2,
                    $"{signal.FrequencyId}: expected >= 2 authored stages, got {signal.MessageFragments.Count}");
            }

            // Builtin-only compatibility fallbacks stay fragment-less; the
            // resolver's null contract keeps their legacy SourceName behavior.
            foreach (var builtinOnly in new[] { "freq_distress_108_9", "freq_distress_134_5", "freq_distress_162_1", "freq_distress_124_7" })
            {
                var def = system.GetDefinition(builtinOnly);
                Assert.NotNull(def);
                Assert.Empty(def!.MessageFragments);
                Assert.Null(DistressStageResolver.Resolve(def, 10));
            }
        }

        [Fact]
        public void PrimaryAuthorityWinsForCrossFileDuplicateIds()
        {
            var system = LoadAuthoritativeCatalog();
            // freq_distress_148_2 exists in both files; the primary Plan 50
            // definition is the canonical bait authority (bait_trap with a
            // raiders deceptive_faction_id). The expansion row must have been
            // overridden by the primary definition, not vice versa.
            var def = system.GetDefinition("freq_distress_148_2");
            Assert.NotNull(def);
            Assert.Equal("bait_trap", def!.OutcomeTypeStr);
            Assert.Equal("raiders", def.DeceptiveFactionId);
        }

        [Fact]
        public void OutcomeHintFieldBindsFromJson()
        {
            string json = """
            {
              "schema_version": 1,
              "radio_broadcasts": [
                {
                  "frequency_id": "freq_test_hint_bind",
                  "frequency_mhz": "101.5",
                  "source_name": "Hint Bind Fixture",
                  "outcome_type": "survivor_isolated",
                  "days_to_trace": 4,
                  "revealed_location": "loc_test_hint",
                  "message_fragments": [
                    { "day": 1, "clarity": 0.2, "text": "weak carrier" },
                    { "day": 3, "clarity": 0.6, "text": "clearer plea", "outcome_hint": "Multiple voices in background." }
                  ]
                }
              ]
            }
            """;
            var system = new RadioDistressSystem();
            Assert.Equal(1, system.LoadFromJson(json));
            var def = system.GetDefinition("freq_test_hint_bind");
            Assert.NotNull(def);
            Assert.Equal(string.Empty, def!.MessageFragments[0].OutcomeHint);
            Assert.Equal("Multiple voices in background.", def.MessageFragments[1].OutcomeHint);
        }

        // ── Stage-contract validation ─────────────────────────────────────────

        private static (string Dir, string FilePath) WriteTempCatalog(string json)
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall-stage-validator-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            string path = Path.Combine(dir, "radio_distress_signals.json");
            File.WriteAllText(path, json);
            return (dir, path);
        }

        private static void DisposeTemp(string dir)
        {
            try { Directory.Delete(dir, recursive: true); } catch { /* best effort */ }
        }

        private static string ValidBroadcast(string id, string fragmentsJson) => $$"""
            {
              "schema_version": 1,
              "radio_broadcasts": [
                {
                  "frequency_id": "{{id}}",
                  "frequency_mhz": "101.5",
                  "source_name": "Validator Fixture",
                  "outcome_type": "survivor_isolated",
                  "days_to_trace": 4,
                  "revealed_location": "loc_test_validator",
                  "message_fragments": {{fragmentsJson}}
                }
              ]
            }
            """;

        [Fact]
        public void Validator_AcceptsWellFormedStages()
        {
            var (dir, _) = WriteTempCatalog(ValidBroadcast("freq_test_valid",
                """[{"day":1,"clarity":0.2,"text":"a"},{"day":3,"clarity":0.6,"text":"b","outcome_hint":"hint"}]"""));
            try
            {
                var report = new Ashfall.Core.CatalogIntegrityReport();
                Ashfall.Core.CatalogIntegrityValidator.ValidateDistressSignalStages(dir, new Ashfall.Core.FileSystemIO(), report);
                Assert.Empty(report.Errors);
            }
            finally { DisposeTemp(dir); }
        }

        [Fact]
        public void Validator_RejectsDuplicateStageDay()
        {
            var (dir, _) = WriteTempCatalog(ValidBroadcast("freq_test_dupday",
                """[{"day":2,"clarity":0.2,"text":"a"},{"day":2,"clarity":0.5,"text":"b"}]"""));
            try
            {
                var report = new Ashfall.Core.CatalogIntegrityReport();
                Ashfall.Core.CatalogIntegrityValidator.ValidateDistressSignalStages(dir, new Ashfall.Core.FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("duplicate stage day") && e.Contains("freq_test_dupday"));
            }
            finally { DisposeTemp(dir); }
        }

        [Fact]
        public void Validator_RejectsDescendingStageDay()
        {
            var (dir, _) = WriteTempCatalog(ValidBroadcast("freq_test_descday",
                """[{"day":3,"clarity":0.2,"text":"a"},{"day":2,"clarity":0.5,"text":"b"}]"""));
            try
            {
                var report = new Ashfall.Core.CatalogIntegrityReport();
                Ashfall.Core.CatalogIntegrityValidator.ValidateDistressSignalStages(dir, new Ashfall.Core.FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("must be greater than previous stage day=3") && e.Contains("freq_test_descday"));
            }
            finally { DisposeTemp(dir); }
        }

        [Fact]
        public void Validator_RejectsDecreasingClarity()
        {
            var (dir, _) = WriteTempCatalog(ValidBroadcast("freq_test_descclar",
                """[{"day":1,"clarity":0.6,"text":"a"},{"day":3,"clarity":0.3,"text":"b"}]"""));
            try
            {
                var report = new Ashfall.Core.CatalogIntegrityReport();
                Ashfall.Core.CatalogIntegrityValidator.ValidateDistressSignalStages(dir, new Ashfall.Core.FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("must be greater than or equal to previous stage clarity") && e.Contains("freq_test_descclar"));
            }
            finally { DisposeTemp(dir); }
        }

        [Fact]
        public void Validator_RejectsEmptyStageText()
        {
            var (dir, _) = WriteTempCatalog(ValidBroadcast("freq_test_emptytext",
                """[{"day":1,"clarity":0.2,"text":""}]"""));
            try
            {
                var report = new Ashfall.Core.CatalogIntegrityReport();
                Ashfall.Core.CatalogIntegrityValidator.ValidateDistressSignalStages(dir, new Ashfall.Core.FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("missing or empty stage text") && e.Contains("freq_test_emptytext"));
            }
            finally { DisposeTemp(dir); }
        }

        [Fact]
        public void Validator_RejectsMissingFragmentsAndOutOfRangeClarity()
        {
            string json = """
            {
              "schema_version": 1,
              "radio_broadcasts": [
                {
                  "frequency_id": "freq_test_nofrag",
                  "frequency_mhz": "101.5",
                  "source_name": "No Fragments",
                  "outcome_type": "survivor_isolated",
                  "revealed_location": "loc_test_validator"
                },
                {
                  "frequency_id": "freq_test_bigclarity",
                  "frequency_mhz": "102.5",
                  "source_name": "Big Clarity",
                  "outcome_type": "survivor_isolated",
                  "revealed_location": "loc_test_validator",
                  "message_fragments": [{"day":1,"clarity":1.5,"text":"a"}]
                }
              ]
            }
            """;
            var (dir, _) = WriteTempCatalog(json);
            try
            {
                var report = new Ashfall.Core.CatalogIntegrityReport();
                Ashfall.Core.CatalogIntegrityValidator.ValidateDistressSignalStages(dir, new Ashfall.Core.FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("freq_test_nofrag") && e.Contains("'message_fragments' missing or empty"));
                Assert.Contains(report.Errors, e => e.Contains("freq_test_bigclarity") && e.Contains("outside the valid range [0,1]"));
            }
            finally { DisposeTemp(dir); }
        }

        [Fact]
        public void Validator_RealCatalogsPassWithPrimaryWinsWarningsOnly()
        {
            var report = new Ashfall.Core.CatalogIntegrityReport();
            Ashfall.Core.CatalogIntegrityValidator.ValidateDistressSignalStages(DataDirectory, new Ashfall.Core.FileSystemIO(), report);
            Assert.Empty(report.Errors);
            // The 5 documented cross-file overrides surface as warnings.
            Assert.Equal(5, report.Warnings.Count(w => w.Contains("overridden by primary authority")));
        }
    }
}
