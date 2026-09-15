// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    /// <summary>
    /// C2 / Plan 20A Gap G3 — weather-gate forced-entry radiation contract.
    ///
    /// `ExpeditionHostSession.OnWeatherGateForced` documents that "the radiation
    /// owner applies block.ForceRadDose to the survivor", and `force_rad_dose`
    /// is authored in weather_route_gates.json — but the event historically had
    /// zero subscribers, silently dropping the dose (plan §49/§57 silent-failure
    /// class). The tests project references Core only, so the host subscription
    /// itself is guarded by a source-level gate (DayEventParitySourceGateTests
    /// pattern) plus data validation of the authored dose values.
    /// </summary>
    public sealed class WeatherGateRadDoseTests
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

        private static string ReadHostSource(string relativePath)
        {
            string path = Path.Combine(RepoRoot, relativePath.Replace('/', Path.DirectorySeparatorChar));
            return File.ReadAllText(path);
        }

        /// <summary>Subscription body tail for assertions; empty when absent so
        /// failures are clean assertion messages, not index crashes.</summary>
        private static string SubscriptionTail(string src)
        {
            int idx = src.IndexOf("OnWeatherGateForced", StringComparison.Ordinal);
            if (idx < 0) return string.Empty;
            return src.Substring(idx, Math.Min(src.Length - idx, 2000));
        }

        [Fact]
        public void OnWeatherGateForced_HasASubscriber_InLiveHostWiring()
        {
            string src = ReadHostSource("src/Main.Expeditions.cs");
            Assert.Contains("OnWeatherGateForced", src, StringComparison.Ordinal);
        }

        [Fact]
        public void ForcedGateSubscriber_AppliesAuthoredForceRadDose_NotAHardcodedLiteral()
        {
            string src = ReadHostSource("src/Main.Expeditions.cs");
            Assert.Contains("OnWeatherGateForced", src, StringComparison.Ordinal);
            // The subscription body must consume block.ForceRadDose through the
            // radiation owner rather than a hardcoded number.
            Assert.Matches(new Regex(@"ForceRadDose"), SubscriptionTail(src));
        }

        [Fact]
        public void ForcedGateSubscriber_DoesNotRouteThroughAmbientExposureContext()
        {
            // Exactly-once rule (plan §13.3/§49): the forced-gate dose is an
            // acute discrete event — it must not feed the ambient zone-rate
            // providers, or a deployed survivor would double-count.
            string src = ReadHostSource("src/Main.Expeditions.cs");
            string tail = SubscriptionTail(src);
            Assert.DoesNotContain("ZoneRadLevel", tail);
            Assert.DoesNotContain("LocationRadRateProvider", tail);
            Assert.DoesNotContain("WeatherRadModifierProvider", tail);
        }

        [Fact]
        public void WeatherRouteGateData_ForceRadDose_ValuesAreNonNegativeIntegers()
        {
            string path = Path.Combine(RepoRoot, "Assets", "StreamingAssets", "Data",
                "weather_route_gates.json");
            Assert.True(File.Exists(path), "weather_route_gates.json missing");
            string src = File.ReadAllText(path);
            var matches = Regex.Matches(src, "\"force_rad_dose\"\\s*:\\s*(-?\\d+)");
            Assert.True(matches.Count > 0, "no force_rad_dose rows authored");
            foreach (Match m in matches)
                Assert.True(int.TryParse(m.Groups[1].Value, out int v) && v >= 0,
                    $"force_rad_dose must be a non-negative integer, found {m.Groups[1].Value}");
        }
    }
}
