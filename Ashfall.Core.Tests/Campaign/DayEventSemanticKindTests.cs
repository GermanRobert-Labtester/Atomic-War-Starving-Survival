// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Ashfall.Core.Campaign;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    /// <summary>
    /// Plan 31 / Task B1 — SemanticKind Totality and Domain Classification Gate.
    ///
    /// Asserts that the semantic kind taxonomy over the DayEventVocabulary is total,
    /// deterministic, non-empty, and provides a strict zero-silent-drop domain classification
    /// over all emitted, handled, and documented day events.
    /// </summary>
    public sealed class DayEventSemanticKindTests
    {
        private static string RepoRoot
        {
            get
            {
                string dir = AppContext.BaseDirectory;
                for (int i = 0; i < 8; i++)
                {
                    if (File.Exists(Path.Combine(dir, "Ashfall.csproj"))) return dir;
                    dir = Path.GetDirectoryName(dir)!;
                }
                throw new InvalidOperationException("repository root not found");
            }
        }

        private static HashSet<string> ExtractHandledKinds()
        {
            string path = Path.Combine(RepoRoot, "Assets", "Ashfall.Core", "Campaign",
                "DailyBriefingReportBuilder.cs");
            string src = File.ReadAllText(path);
            return new HashSet<string>(
                Regex.Matches(src, "case \"([a-z_]+)\":").Select(m => m.Groups[1].Value),
                StringComparer.Ordinal);
        }

        private static Dictionary<string, List<string>> ExtractEmittedKinds()
        {
            var emitted = new Dictionary<string, List<string>>(StringComparer.Ordinal);
            var roots = new[]
            {
                Path.Combine(RepoRoot, "src"),
                Path.Combine(RepoRoot, "Assets", "Ashfall.Core")
            };
            foreach (string root in roots)
            {
                foreach (string file in Directory.EnumerateFiles(root, "*.cs", SearchOption.AllDirectories))
                {
                    string src = File.ReadAllText(file);
                    foreach (Match m in Regex.Matches(src, "new DayStateChangeEvent\\(\\s*\"([a-z_]+)\""))
                    {
                        string kind = m.Groups[1].Value;
                        if (!emitted.TryGetValue(kind, out var list))
                            emitted[kind] = list = new List<string>();
                        list.Add(Path.GetFileName(file));
                    }
                }
            }
            return emitted;
        }

        private static HashSet<string> ExtractParityMatrixKinds()
        {
            string matrixPath = Path.Combine(RepoRoot, "docs", "campaign", "EVENT_SEMANTIC_PARITY_MATRIX.md");
            string text = File.ReadAllText(matrixPath);
            return new HashSet<string>(
                Regex.Matches(text, @"\|\s*`([a-z0-9_]+)`\s*\|").Select(m => m.Groups[1].Value),
                StringComparer.Ordinal);
        }

        [Fact]
        public void SemanticKindMap_IsNotEmpty_AndContainsZeroUnknowns()
        {
            var mappings = DayEventVocabulary.AllSemanticMappings;
            Assert.NotEmpty(mappings);
            Assert.True(mappings.Count >= 110, $"Expected >= 110 mappings, found {mappings.Count}");

            foreach (var (kind, semKind) in mappings)
            {
                Assert.False(string.IsNullOrWhiteSpace(kind), "Event kind key must not be blank.");
                Assert.NotEqual(SemanticKind.Unknown, semKind);
            }
        }

        [Fact]
        public void TotalityGate_AllEmittedKinds_MapToValidSemanticKind()
        {
            var emitted = ExtractEmittedKinds();
            Assert.NotEmpty(emitted);

            foreach (var (kind, sourceFiles) in emitted)
            {
                var semKind = DayEventVocabulary.GetSemanticKind(kind);
                Assert.NotEqual(SemanticKind.Unknown, semKind);
                Assert.True(DayEventVocabulary.TryGetSemanticKind(kind, out var semKind2));
                Assert.Equal(semKind, semKind2);
            }
        }

        [Fact]
        public void TotalityGate_AllBriefingHandledKinds_MapToValidSemanticKind()
        {
            var handled = ExtractHandledKinds();
            Assert.NotEmpty(handled);

            foreach (string kind in handled)
            {
                var semKind = DayEventVocabulary.GetSemanticKind(kind);
                Assert.NotEqual(SemanticKind.Unknown, semKind);
                Assert.True(DayEventVocabulary.TryGetSemanticKind(kind, out var semKind2));
                Assert.Equal(semKind, semKind2);
            }
        }

        [Fact]
        public void TotalityGate_AllParityMatrixKinds_MapToValidSemanticKind()
        {
            var matrixKinds = ExtractParityMatrixKinds();
            Assert.NotEmpty(matrixKinds);

            foreach (string kind in matrixKinds)
            {
                var semKind = DayEventVocabulary.GetSemanticKind(kind);
                Assert.NotEqual(SemanticKind.Unknown, semKind);
                Assert.True(DayEventVocabulary.TryGetSemanticKind(kind, out var semKind2));
                Assert.Equal(semKind, semKind2);
            }
        }

        [Theory]
        [InlineData("survivor_perished", SemanticKind.Casualty)]
        [InlineData("child_lost", SemanticKind.Casualty)]
        [InlineData("hazard_warning", SemanticKind.Hazard)]
        [InlineData("cascade_warning", SemanticKind.Hazard)]
        [InlineData("power_critical_deficit", SemanticKind.Hazard)]
        [InlineData("power_brownout_began", SemanticKind.Hazard)]
        [InlineData("shelter_filter_degraded", SemanticKind.Hazard)]
        [InlineData("ate", SemanticKind.Survivor)]
        [InlineData("drank", SemanticKind.Survivor)]
        [InlineData("med_taken", SemanticKind.Survivor)]
        [InlineData("meal_served", SemanticKind.Survivor)]
        [InlineData("duty_vacated", SemanticKind.Survivor)]
        [InlineData("medical_admitted", SemanticKind.Survivor)]
        [InlineData("power_shed_automatic", SemanticKind.Shelter)]
        [InlineData("shelter_decon_completed", SemanticKind.Shelter)]
        [InlineData("crafting_completed", SemanticKind.Production)]
        [InlineData("trapping_harvest", SemanticKind.Production)]
        [InlineData("expedition_milestone", SemanticKind.Expedition)]
        [InlineData("subterranean_rescue_completed", SemanticKind.Expedition)]
        [InlineData("radio_intercept", SemanticKind.Communication)]
        [InlineData("radio_transmission", SemanticKind.Communication)]
        [InlineData("weather_condition", SemanticKind.Weather)]
        [InlineData("weather_forecast_miss", SemanticKind.Weather)]
        [InlineData("echo_surfaced", SemanticKind.Narrative)]
        [InlineData("personal_quest_progressed", SemanticKind.Narrative)]
        [InlineData("obligation_warning", SemanticKind.Narrative)]
        [InlineData("obligation_missed", SemanticKind.Narrative)]
        [InlineData("events_evaluated", SemanticKind.Heartbeat)]
        [InlineData("world_ticked", SemanticKind.Heartbeat)]
        [InlineData("power_ticked", SemanticKind.Heartbeat)]
        public void RepresentativeKinds_ResolveToExpectedSemanticDomain(string kind, SemanticKind expected)
        {
            Assert.Equal(expected, DayEventVocabulary.GetSemanticKind(kind));
        }

        [Fact]
        public void UnregisteredKind_ReturnsUnknown()
        {
            Assert.Equal(SemanticKind.Unknown, DayEventVocabulary.GetSemanticKind((string?)null));
            Assert.Equal(SemanticKind.Unknown, DayEventVocabulary.GetSemanticKind(string.Empty));
            Assert.Equal(SemanticKind.Unknown, DayEventVocabulary.GetSemanticKind("unregistered_bogus_kind_999"));

            Assert.False(DayEventVocabulary.TryGetSemanticKind("unregistered_bogus_kind_999", out var sem));
            Assert.Equal(SemanticKind.Unknown, sem);
        }

        [Fact]
        public void DynamicTickedSuffix_ClassifiesAsHeartbeat()
        {
            // Verifies open-ended dynamic heartbeats follow convention even if not statically in map
            string syntheticHeartbeat = "quantum_stabilizer_ticked";
            Assert.Equal(SemanticKind.Heartbeat, DayEventVocabulary.GetSemanticKind(syntheticHeartbeat));
            Assert.True(DayEventVocabulary.TryGetSemanticKind(syntheticHeartbeat, out var sem));
            Assert.Equal(SemanticKind.Heartbeat, sem);
        }

        [Fact]
        public void ExtensionMethod_OnDayStateChangeEvent_MatchesStaticMethod()
        {
            DayStateChangeEvent? nullEvt = null;
            Assert.Equal(SemanticKind.Unknown, nullEvt.GetSemanticKind());

            var evt = new DayStateChangeEvent("hazard_warning", "system", "rad_storm", null, 42f);
            Assert.Equal(SemanticKind.Hazard, evt.GetSemanticKind());
        }

        [Fact]
        public void HeartbeatClassification_MatchesIsInternalHeartbeatPredicate()
        {
            foreach (var (kind, semKind) in DayEventVocabulary.AllSemanticMappings)
            {
                bool isHeartbeat = DayEventVocabulary.IsInternalHeartbeat(kind);
                if (semKind == SemanticKind.Heartbeat)
                {
                    Assert.True(isHeartbeat, $"Kind '{kind}' mapped to Heartbeat but IsInternalHeartbeat returned false.");
                }
                else
                {
                    Assert.False(isHeartbeat, $"Kind '{kind}' mapped to {semKind} but IsInternalHeartbeat returned true.");
                }
            }
        }

        [Fact]
        public void ConsumerGroupingAndFiltering_BehavesDeterministically()
        {
            var events = new List<DayStateChangeEvent>
            {
                new("ate", "crew", "mara"),
                new("hazard_warning", "sensors", "storm"),
                new("power_ticked", "grid"),
                new("drank", "crew", "mara"),
                new("survivor_perished", "ward", "colin"),
                new("echo_surfaced", "narrative", "echo_1"),
                new("crafting_completed", "workshop", "filter"),
                new("events_evaluated", "narrative")
            };

            var grouped = events.GroupBy(e => e.GetSemanticKind())
                                .ToDictionary(g => g.Key, g => g.Count());

            Assert.Equal(2, grouped[SemanticKind.Survivor]);
            Assert.Equal(1, grouped[SemanticKind.Hazard]);
            Assert.Equal(2, grouped[SemanticKind.Heartbeat]);
            Assert.Equal(1, grouped[SemanticKind.Casualty]);
            Assert.Equal(1, grouped[SemanticKind.Narrative]);
            Assert.Equal(1, grouped[SemanticKind.Production]);

            // Filter non-heartbeats
            var playerFacing = events.Where(e => e.GetSemanticKind() != SemanticKind.Heartbeat).ToList();
            Assert.Equal(6, playerFacing.Count);
        }
    }
}
