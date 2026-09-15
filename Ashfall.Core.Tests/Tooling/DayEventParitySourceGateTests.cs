// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    /// <summary>
    /// C2 / Plan 17A-S — producer/consumer semantic parity gate.
    ///
    /// Scans the live producer sources (host day owners + Core emitters) for
    /// emitted <c>DayStateChangeEvent</c> kinds and asserts that every one is
    /// consumed by the briefing builder (explicit case or generic default) or
    /// classified as an internal heartbeat. Prevents the original Plan 17A
    /// defect class — silent vocabulary drift — from ever re-occurring
    /// silently. Plan 31 may replace this gate with a typed vocabulary.
    /// </summary>
    public sealed class DayEventParitySourceGateTests
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

        [Fact]
        public void EveryEmittedKind_IsHandledOrClassified_NoSilentDrop()
        {
            var handled = ExtractHandledKinds();
            var emitted = ExtractEmittedKinds();

            Assert.NotEmpty(emitted);

            // Structural no-silent-drop: the builder MUST contain a default
            // case routing unhandled kinds to the generic section (C2 §6.3).
            // With that default in place, EVERY emitted kind is consumed by
            // construction: explicit case → tailored text; heartbeat →
            // intentionally classified; anything else → generic visible row.
            string builder = File.ReadAllText(Path.Combine(RepoRoot, "Assets", "Ashfall.Core",
                "Campaign", "DailyBriefingReportBuilder.cs"));
            Assert.Matches(new Regex("default:\\s*// C2 / Plan 17A-S", RegexOptions.Multiline), builder);
            Assert.Contains("DayEventVocabulary.IsInternalHeartbeat", builder);

            // Per-kind coherence: an unhandled, non-heartbeat kind must be a
            // deliberate generic-render (present in the parity matrix), never
            // an accidental addition nobody classified.
            string matrix = File.ReadAllText(Path.Combine(RepoRoot, "docs", "campaign",
                "EVENT_SEMANTIC_PARITY_MATRIX.md"));
            foreach ((string kind, _) in emitted)
            {
                Assert.True(
                    handled.Contains(kind) || Ashfall.Core.Campaign.DayEventVocabulary.IsInternalHeartbeat(kind)
                        || matrix.Contains(kind, StringComparison.Ordinal),
                    $"emitted kind '{kind}' is unhandled and unclassified — add it to the " +
                    "parity matrix or the vocabulary (C2 §6.3/§6.4)");
            }
        }

        [Fact]
        public void ParityMatrix_IsCurrent_AtMeasuredShape()
        {
            var handled = ExtractHandledKinds();
            var emitted = ExtractEmittedKinds();
            string matrix = File.ReadAllText(Path.Combine(RepoRoot, "docs", "campaign",
                "EVENT_SEMANTIC_PARITY_MATRIX.md"));

            // Every emitted kind must appear in the parity matrix document.
            foreach ((string kind, _) in emitted)
                Assert.Contains(kind, matrix, StringComparison.Ordinal);

            // Every handled kind must appear too (catches builder-case removals
            // leaving the matrix stale).
            foreach (string kind in handled)
                Assert.Contains(kind, matrix, StringComparison.Ordinal);
        }
    }
}
