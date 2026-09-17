// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Ashfall.Core.Campaign;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Plan20CWarnings
{
    /// <summary>
    /// C2 / Plan 20C (§39/§43) — forecast-miss attribution + audio alert parity.
    /// Host-side emission is source-gated (the tests project references Core
    /// only); the builder rendering + classification rules are tested directly.
    /// </summary>
    public sealed class Plan20CWarningTests
    {
        private static DayStateChangeEvent Evt(string kind, string owner,
            string? primary = null, string? secondary = null, float numeric = 0f)
            => new(kind, owner, primary, secondary, numeric);

        [Fact]
        public void ForecastMiss_RendersWarning_NamingTheCause()
        {
            var report = DailyBriefingReportBuilder.BuildFromDayEvents(20, 20, new[]
            {
                Evt("weather_forecast_miss", "weather_world", "FalloutStorm", "station_predicted_other", 20)
            });

            var section = report.Sections.FirstOrDefault(s => s.Title == "Warnings");
            Assert.NotNull(section);
            var entry = Assert.Single(section!.Entries);
            Assert.Contains("FalloutStorm", entry.Text);
            Assert.Contains("station predicted otherwise", entry.Text);
        }

        [Fact]
        public void UnexpectedStorm_RendersWarning_NamingTheAbsence()
        {
            var report = DailyBriefingReportBuilder.BuildFromDayEvents(20, 20, new[]
            {
                Evt("weather_unexpected_storm", "weather_world", "BlackRain", "no_station_forecast", 20)
            });

            var section = report.Sections.FirstOrDefault(s => s.Title == "Warnings");
            Assert.NotNull(section);
            var entry = Assert.Single(section!.Entries);
            Assert.Contains("BlackRain", entry.Text);
            Assert.Contains("no station forecast", entry.Text);
        }

        [Fact]
        public void SevereWeatherRendering_IsDeterministic()
        {
            var events = new[]
            {
                Evt("weather_forecast_miss", "weather_world", "GlassStorm", "station_predicted_other", 20),
                Evt("weather_unexpected_storm", "weather_world", "RadHail", "no_station_forecast", 20)
            };
            string Render()
            {
                var report = DailyBriefingReportBuilder.BuildFromDayEvents(9, 9, events);
                return string.Join("|", report.Sections
                    .SelectMany(s => s.Entries.Select(e => $"{s.Title}::{e.Text}")));
            }
            Assert.Equal(Render(), Render());
        }
    }

    /// <summary>
    /// §43 alert-parity rules, source-gated: the host classifies severity from
    /// the effects table (data-driven — GlassStorm/RadHail/IceStorm now alert
    /// like Blizzard), with the legacy static as the unbound fallback.
    /// </summary>
    public sealed class Plan20CAudioParityGateTests
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

        private static string Read(string relativePath)
            => File.ReadAllText(Path.Combine(RepoRoot, relativePath.Replace('/', Path.DirectorySeparatorChar)));

        [Fact]
        public void AlertClassification_IsDataDriven_FromTheEffectsTable()
        {
            string session = Read("src/Host/WorldHostSession.cs");
            Assert.Contains("IsSevereWeather", session, StringComparison.Ordinal);
            Assert.Contains("WeatherEffects.TryGetEffects", session, StringComparison.Ordinal);
            // The alert edge must consume the severity classifier, not the
            // legacy three-kind literal.
            Assert.Contains("IsSevereWeather(kind)", session, StringComparison.Ordinal);
        }

        [Fact]
        public void AmbienceBeds_CoverEverySevereKind_BySemanticFamily()
        {
            // The ambience resolver must map every severe kind to a bed (no
            // silence where a bed is expected). Severe set per the authored
            // table: rad ≥ 60 OR visibility ≤ 0.5 OR thermal ≤ −10.
            string audio = Read("src/Audio/SurfaceAmbienceController.cs");
            foreach (string kind in new[]
            {
                "FalloutStorm", "Blizzard", "BlackRain", "GlassStorm", "Ashfall",
                "AcidSnow", "BlackSnow", "IceStorm", "BloodRain", "EMPStorm"
            })
            {
                Assert.True(
                    audio.Contains($"WeatherKind.{kind}", StringComparison.Ordinal)
                        || audio.Contains($"AudioCueCatalog.Weather{kind}", StringComparison.Ordinal),
                    $"ambience resolver does not reference severe kind {kind}");
            }
        }
    }
}
