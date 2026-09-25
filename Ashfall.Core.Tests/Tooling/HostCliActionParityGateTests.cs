// SPDX-License-Identifier: MIT
// ASHFALL gate: host CLI action catalog parity (ERR-01 recurrence guard).
//
// Defect class (docs/health/CODEHEALTH_SWEEP_2026-09-25.md ERR-01): a rewrite of
// src/Host/HostCli.cs dropped 24 HostCliAction members that src/Main.Application.cs
// still dispatched, which also left 24+ --*-selftest probes unreachable after the
// compile error was repaired. The compiler guards the enum/switch half; nothing
// guarded the "probe silently dropped from the catalog" half.
//
// This gate joins the three existing authorities instead of inventing a fourth:
//   docs/ci/SELFTEST_MANIFEST.json  (generated from Ashfall.Core.HostCliRegistry)
//   src/Host/HostCli.cs             (AtomicWar.GodotApp.HostCliAction + Parse)
//   src/Main.Application.cs         (dispatch switch)
//
// Debt reference: DEBT-HOSTCLI-PROBE-MANIFEST-GAP (KNOWN_DEBT.md).
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    public class HostCliActionParityGateTests
    {
        /// <summary>
        /// Host probes dispatched by the CLI but not yet cataloged in
        /// docs/ci/SELFTEST_MANIFEST.json. Shrink-only contract: cataloging one of
        /// these requires removing it here; adding a name requires a foreman-signed
        /// catalog decision. Measured 2026-09-25 on claim
        /// claim-wholegame-p0-build-green-2026-09-25.
        /// </summary>
        private static readonly string[] DocumentedUnmanifestedSelfTests =
        {
            "CampaignFuzzSelfTest",
            "CartographySelfTest",
            "ChemicalReconUiTest",
            "CompositionRootSelfTest",
            "ContentUtilizationSelfTest",
            "DeconAirlockUiTest",
            "DynamicWorldSelfTest",
            "ExpansionDepthSelfTest",
            "ExportParitySelfTest",
            "GeodeticSurveyUiTest",
            "GeothermalAquiferSelfTest",
            "KineticStorageUiTest",
            "ModSelfTest",
            "NarrativeContinuitySelfTest",
            "OralLoreSelfTest",
            "Plans198To201UiTest",
            "PowerGridCatalogSelfTest",
            "ReconTelemetrySelfTest",
            "SceneBindingSelfTest",
            "StartingCohortLifecycleSelfTest",
            "StartingSuppliesSelfTest",
            "TrappingHostSelfTest",
            "WastelandInhabitantsSelfTest",
            "WorkshopRelicUiTest",
            "WorldExplorationSelfTest"
        };

        /// <summary>No-argument default action; it has no flag and no switch case.</summary>
        private const string NoArgDefaultMember = "Interactive";

        private static readonly Regex LineComment = new Regex("//.*", RegexOptions.Compiled);
        private static readonly Regex BlockComment =
            new Regex("/\\*.*?\\*/", RegexOptions.Compiled | RegexOptions.Singleline);

        private sealed class SelftestManifest
        {
            public string schema_version { get; set; } = string.Empty;
            public int total_tests { get; set; }
            public List<SelftestEntry> tests { get; set; } = new List<SelftestEntry>();
        }

        private sealed class SelftestEntry
        {
            public string test_id { get; set; } = string.Empty;
            public string action { get; set; } = string.Empty;
            public string primary_flag { get; set; } = string.Empty;
            public List<string> aliases { get; set; } = new List<string>();
        }

        private static string RepoRoot()
        {
            string[] candidates =
            {
                Directory.GetCurrentDirectory(),
                AppContext.BaseDirectory
            };
            foreach (string start in candidates)
            {
                var dir = new DirectoryInfo(Path.GetFullPath(start));
                while (dir != null)
                {
                    string probe = Path.Combine(dir.FullName, "src", "Host", "HostCli.cs");
                    if (File.Exists(probe))
                        return dir.FullName;
                    dir = dir.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate src/Host/HostCli.cs from the test run.");
        }

        private static string StripComments(string text)
        {
            text = BlockComment.Replace(text, string.Empty);
            return LineComment.Replace(text, string.Empty);
        }

        private static SelftestManifest LoadManifest(out string root)
        {
            root = RepoRoot();
            string path = Path.Combine(root, "docs", "ci", "SELFTEST_MANIFEST.json");
            Assert.True(File.Exists(path), $"SELFTEST_MANIFEST.json missing at {path}");
            var doc = JsonSerializer.Deserialize<SelftestManifest>(
                File.ReadAllText(path),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(doc);
            Assert.True(doc!.tests.Count >= 200,
                $"Expected at least 200 cataloged tests, parsed {doc.tests.Count}. The manifest or its schema drifted.");
            return doc;
        }
        private static List<string> HostEnumMembers(string root)
        {
            string code = StripComments(File.ReadAllText(Path.Combine(root, "src", "Host", "HostCli.cs")));
            int enumIdx = code.IndexOf("enum HostCliAction", StringComparison.Ordinal);
            Assert.True(enumIdx >= 0, "Could not find 'enum HostCliAction' in src/Host/HostCli.cs");
            int open = code.IndexOf('{', enumIdx);
            int close = code.IndexOf('}', open);
            Assert.True(open > 0 && close > open, "Could not delimit the HostCliAction enum body");
            return code.Substring(open + 1, close - open - 1)
                .Split(',')
                .Select(m => m.Trim())
                .Where(m => m.Length > 0)
                .ToList();
        }

        [Fact]
        public void EveryManifestAction_ExistsInHostEnumAndIsDispatched()
        {
            var manifest = LoadManifest(out string root);
            var members = new HashSet<string>(HostEnumMembers(root), StringComparer.Ordinal);
            string dispatch = StripComments(File.ReadAllText(Path.Combine(root, "src", "Main.Application.cs")));

            var missingFromEnum = new List<string>();
            var missingFromDispatch = new List<string>();
            foreach (var entry in manifest.tests)
            {
                if (string.IsNullOrWhiteSpace(entry.action))
                    continue;
                if (!members.Contains(entry.action))
                    missingFromEnum.Add($"{entry.test_id} -> {entry.action}");
                else if (!Regex.IsMatch(dispatch, "HostCliAction\\." + Regex.Escape(entry.action) + "\\b"))
                    missingFromDispatch.Add($"{entry.test_id} -> {entry.action}");
            }

            Assert.True(missingFromEnum.Count == 0,
                $"Cataloged tests name {missingFromEnum.Count} host actions that do not exist in " +
                "AtomicWar.GodotApp.HostCliAction (the probe is cataloged but cannot be dispatched):\n  " +
                string.Join("\n  ", missingFromEnum));

            Assert.True(missingFromDispatch.Count == 0,
                $"Cataloged tests name {missingFromDispatch.Count} host actions with no dispatch case in " +
                "src/Main.Application.cs (the probe is cataloged but has no runtime path):\n  " +
                string.Join("\n  ", missingFromDispatch));
        }

        [Fact]
        public void EveryManifestFlag_IsParsedByHostCli()
        {
            var manifest = LoadManifest(out string root);
            string code = StripComments(File.ReadAllText(Path.Combine(root, "src", "Host", "HostCli.cs")));

            int parseIdx = code.IndexOf("Parse(", StringComparison.Ordinal);
            int helpIdx = code.IndexOf("PrintHelp(", StringComparison.Ordinal);
            Assert.True(parseIdx >= 0 && helpIdx > parseIdx, "Could not delimit HostCli.Parse body");
            string parseBody = code.Substring(parseIdx, helpIdx - parseIdx);

            var unparsed = new List<string>();
            foreach (var entry in manifest.tests)
            {
                var flags = new List<string>();
                if (!string.IsNullOrWhiteSpace(entry.primary_flag))
                    flags.Add(entry.primary_flag);
                flags.AddRange(entry.aliases ?? new List<string>());
                if (flags.Count == 0)
                    continue;
                if (!flags.Any(f => parseBody.Contains("\"" + f + "\"", StringComparison.Ordinal)))
                    unparsed.Add($"{entry.test_id} ({string.Join(" | ", flags)})");
            }

            Assert.True(unparsed.Count == 0,
                $"Cataloged tests expose {unparsed.Count} flag sets that HostCli.Parse never recognizes " +
                "(undeclared probes in the machine-readable catalog):\n  " +
                string.Join("\n  ", unparsed));
        }
        [Fact]
        public void EveryHostEnumMember_IsDispatched()
        {
            string root = RepoRoot();
            var members = HostEnumMembers(root);
            string dispatch = StripComments(File.ReadAllText(Path.Combine(root, "src", "Main.Application.cs")));
            string host = StripComments(File.ReadAllText(Path.Combine(root, "src", "Host", "HostCli.cs")));

            var dead = members
                .Where(m => m != NoArgDefaultMember)
                .Where(m => !Regex.IsMatch(dispatch, "HostCliAction\\." + Regex.Escape(m) + "\\b"))
                .Where(m => !Regex.IsMatch(host, "HostCliAction\\." + Regex.Escape(m) + "\\b"))
                .ToList();

            Assert.True(dead.Count == 0,
                $"HostCliAction declares {dead.Count} members nothing dispatches " +
                $"(neither src/Main.Application.cs nor src/Host/HostCli.cs references them; '{NoArgDefaultMember}' " +
                "is the documented no-arg default):\n  " + string.Join("\n  ", dead));
        }

        [Fact]
        public void UnmanifestedHostSelfTests_MatchDocumentedBaseline()
        {
            var manifest = LoadManifest(out string root);
            var cataloged = new HashSet<string>(manifest.tests.Select(t => t.action), StringComparer.Ordinal);
            var baseline = new HashSet<string>(DocumentedUnmanifestedSelfTests, StringComparer.Ordinal);

            var actual = HostEnumMembers(root)
                .Where(m => Regex.IsMatch(m, "(SelfTest|UiTest|SelfCheck)$"))
                .Where(m => !cataloged.Contains(m))
                .ToHashSet(StringComparer.Ordinal);

            var added = actual.Where(m => !baseline.Contains(m)).OrderBy(m => m, StringComparer.Ordinal).ToList();
            var removed = baseline.Where(m => !actual.Contains(m)).OrderBy(m => m, StringComparer.Ordinal).ToList();

            Assert.True(added.Count == 0,
                "New host probes are dispatched but absent from docs/ci/SELFTEST_MANIFEST.json " +
                $"(they would never run in manifest-driven shard smokes):\n  {string.Join("\n  ", added)}\n" +
                "Fix: author the Core HostCliRegistry descriptor, regenerate the manifest, then remove the name " +
                "from DocumentedUnmanifestedSelfTests. Debt: DEBT-HOSTCLI-PROBE-MANIFEST-GAP.");

            Assert.True(removed.Count == 0,
                $"DocumentedUnmanifestedSelfTests lists {removed.Count} names that are now cataloged or gone:\n  " +
                string.Join("\n  ", removed) + "\nThe baseline is shrink-only: delete each resolved name.");
        }


    }
}
