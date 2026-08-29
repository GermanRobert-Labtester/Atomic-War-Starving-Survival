// SPDX-License-Identifier: MIT
// VH-3 — the three wall-clock validators must agree on their own exemption.
using System;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    /// <summary>
    /// Three validators enforce the same "no wall-clock drift in Core" policy:
    ///
    /// <list type="number">
    /// <item><description><c>Ashfall.Core.Tests/Tooling/ForbiddenCoreApiGateTests.cs</c></description></item>
    /// <item><description><c>Ashfall.Core.Tests/CoreInvariantSourceTests.cs</c></description></item>
    /// <item><description><c>scripts/ci/forbidden-api-gate.sh</c></description></item>
    /// </list>
    ///
    /// <para>They disagreed. The two xUnit validators exempted
    /// <c>IWallClock.cs</c> as "the documented single port/adapter for
    /// non-simulation wall-clock metadata"; the shell gate had no exemption
    /// mechanism at all and failed on the same file. So the canonical
    /// <c>forbidden_core_apis</c> gate was permanently red for a file two tests
    /// deliberately allowed.</para>
    ///
    /// <para>The exemption is canonical, on the evidence: <c>IWallClock.cs</c>
    /// documents itself as the wall-clock port and states that its values must
    /// never drive simulation; it ships a <c>FrozenWallClock</c> specifically so
    /// tests can control time; its only Core consumers
    /// (<c>Save/SaveSlotService.cs</c>, <c>Content/ContentUtilizationManifest.cs</c>)
    /// inject the interface rather than calling <c>DateTime</c>; and it is the
    /// only file in Core containing a real wall-clock call at all. Simulation time
    /// comes from <c>IClock</c>/<c>ISimClock</c>, which <c>Ports.cs</c> annotates
    /// "Simulation calendar. Never DateTime.Now."</para>
    ///
    /// <para>This test exists so the three cannot drift apart again. It is a
    /// source-text agreement check, not a behavioural one — that is the only way
    /// to compare a shell script against xUnit validators.</para>
    /// </summary>
    public class WallClockPolicyAgreementTests
    {
        private const string ExemptFile = "IWallClock.cs";

        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, "Assets", "Ashfall.Core"))
                    && Directory.Exists(Path.Combine(dir.FullName, "scripts", "ci")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate the repository root from the test run.");
        }

        private static string Read(params string[] parts)
        {
            string path = Path.Combine(RepoRoot(), Path.Combine(parts));
            Assert.True(File.Exists(path), $"expected validator not found: {path}");
            return File.ReadAllText(path);
        }

        [Fact]
        public void ShellGate_ExemptsTheWallClockPort()
        {
            string gate = Read("scripts", "ci", "forbidden-api-gate.sh");

            // The exemption must be attached to the wall-clock check specifically.
            var call = Regex.Match(
                gate,
                @"check_pattern\s+""Zero Wall-Clock Simulation Drift""\s+""[^""]+""\s+""(?<exempt>[^""]+)""");

            Assert.True(call.Success,
                "scripts/ci/forbidden-api-gate.sh no longer passes an exempt file to the wall-clock check. "
                + "The two xUnit validators still exempt IWallClock.cs, so the gate would be red for a file "
                + "the tests deliberately allow.");
            Assert.Equal(ExemptFile, call.Groups["exempt"].Value);
        }

        [Fact]
        public void ForbiddenCoreApiGateTests_ExemptsTheWallClockPort()
        {
            string src = Read("Ashfall.Core.Tests", "Tooling", "ForbiddenCoreApiGateTests.cs");
            Assert.Contains(ExemptFile, src);
        }

        [Fact]
        public void CoreInvariantSourceTests_ExemptsTheWallClockPort()
        {
            string src = Read("Ashfall.Core.Tests", "CoreInvariantSourceTests.cs");
            Assert.Contains(ExemptFile, src);
        }

        /// <summary>
        /// The exemption must stay narrow. The shell gate takes the exempt file as
        /// a per-check argument, so it must not be applied to the engine-coupling,
        /// RNG, GUID, serializer or Thread.Sleep checks.
        /// </summary>
        [Theory]
        [InlineData("Zero Engine Namespaces")]
        [InlineData("Zero System.Random / Nondeterministic RNG")]
        [InlineData("Zero Guid.NewGuid()")]
        [InlineData("Zero Legacy Serializer Bypasses")]
        [InlineData("Zero Thread.Sleep")]
        public void ShellGate_DoesNotExemptAnyOtherCheck(string label)
        {
            string gate = Read("scripts", "ci", "forbidden-api-gate.sh");

            var call = Regex.Match(
                gate,
                @"check_pattern\s+""" + Regex.Escape(label) + @"""\s+""(?:[^""\\]|\\.)*""(?<extra>[^\r\n]*)");

            Assert.True(call.Success, $"could not find the '{label}' check in forbidden-api-gate.sh");
            Assert.DoesNotContain(ExemptFile, call.Groups["extra"].Value);
        }

        /// <summary>
        /// The exemption is only defensible while the port really is the single
        /// wall-clock site in Core. If a second file starts calling the wall clock
        /// directly, the policy needs revisiting rather than a second exemption.
        /// </summary>
        [Fact]
        public void TheWallClockPortIsStillTheOnlyWallClockSiteInCore()
        {
            string coreDir = Path.Combine(RepoRoot(), "Assets", "Ashfall.Core");
            var lineComment = new Regex(@"//.*$", RegexOptions.Multiline);
            var blockComment = new Regex(@"/\*.*?\*/", RegexOptions.Singleline);
            var hazard = new Regex(@"\bDateTime\s*\.\s*(Now|UtcNow)\b");

            var offenders = new System.Collections.Generic.List<string>();

            foreach (string file in Directory.EnumerateFiles(coreDir, "*.cs", SearchOption.AllDirectories))
            {
                string norm = file.Replace('\\', '/');
                if (norm.Contains("/obj/") || norm.Contains("/bin/")) continue;
                if (Path.GetFileName(file).Equals(ExemptFile, StringComparison.OrdinalIgnoreCase)) continue;

                string code = blockComment.Replace(File.ReadAllText(file), string.Empty);
                code = lineComment.Replace(code, string.Empty);

                if (hazard.IsMatch(code)) offenders.Add(Path.GetFileName(file));
            }

            Assert.True(offenders.Count == 0,
                "Core files other than " + ExemptFile + " now call the wall clock directly: "
                + string.Join(", ", offenders)
                + ". The single-port exemption is no longer accurate — route these through IWallClock, "
                + "or revisit the policy deliberately rather than exempting another file.");
        }
    }
}
