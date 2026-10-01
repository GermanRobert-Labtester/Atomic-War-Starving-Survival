// SPDX-License-Identifier: MIT
// Source-level hygiene gate: every expedition phase label rendered by src/
// must go through ExpeditionPhaseText — the single owner of phase wording —
// so a new panel or host file cannot reintroduce a raw (ExpeditionPhase)….ToString()
// label that drifts from the localized ui.expedition.phase.* catalog rows.
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    public sealed class ExpeditionPhaseTextSourceGateTests
    {
        // A raw enum-to-display conversion of a phase value, e.g.
        // ((ExpeditionPhase)exp.phase).ToString().ToUpperInvariant().
        private static readonly Regex RawPhaseToString = new(
            @"ExpeditionPhase\)[^\n;]*?\.ToString\(\)",
            RegexOptions.Compiled);

        [Fact]
        public void Src_DoesNotRenderRawExpeditionPhaseStrings()
        {
            string srcRoot = FindSrcRoot();
            var offenders = new List<string>();
            foreach (string file in Directory.EnumerateFiles(srcRoot, "*.cs", SearchOption.AllDirectories))
            {
                string name = Path.GetFileName(file);
                if (name == "ExpeditionPhaseText.cs") continue;
                string text = File.ReadAllText(file);
                foreach (Match match in RawPhaseToString.Matches(text))
                    offenders.Add($"{Path.GetRelativePath(srcRoot, file)}: {match.Value}");
            }

            Assert.True(offenders.Count == 0,
                "src/ must render expedition phase labels through ExpeditionPhaseText, not a raw " +
                "(ExpeditionPhase)….ToString() conversion:\n" + string.Join("\n", offenders));
        }

        [Fact]
        public void ExpeditionPanels_UseTheSharedPhaseTextHelper()
        {
            string uiDir = Path.Combine(FindSrcRoot(), "UI");
            foreach (string name in new[] { "ExpeditionPanel.cs", "ExpeditionRadarPanel.cs" })
            {
                string path = Path.Combine(uiDir, name);
                Assert.True(File.Exists(path), $"Could not find {name} under {uiDir}");
                Assert.Contains("ExpeditionPhaseText.Label(", File.ReadAllText(path), StringComparison.Ordinal);
            }
        }

        private static string FindSrcRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string candidate = Path.Combine(dir.FullName, "src");
                if (Directory.Exists(candidate) && File.Exists(Path.Combine(dir.FullName, "project.godot")))
                    return candidate;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate src/ from " + AppContext.BaseDirectory);
        }
    }
}
