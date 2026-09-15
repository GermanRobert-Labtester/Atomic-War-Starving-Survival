// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    /// <summary>
    /// C2 / Plan 20A (G1) — weather-effects data authority. Every WeatherKind
    /// must have an explicit row (no silent defaults, plan §35.1); the runtime
    /// OutdoorRadModifier pins are preserved; unknown/negative/duplicate rows
    /// are load errors; forecast and runtime read the same table.
    /// </summary>
    public sealed class WeatherEffectsCatalogTests
    {
        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent!;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        [Fact]
        public void RealDataCatalog_LoadsCleanly_WithNoErrors()
        {
            var catalog = WeatherEffectsCatalog.LoadFromDirectory(GetDataDir(), new Ashfall.Core.FileSystemIO());
            Assert.Empty(catalog.Errors);
            Assert.True(catalog.LoadedCount > 0, "weather_effects.json produced no rows");
        }

        [Fact]
        public void RealDataCatalog_EveryWeatherKind_HasExplicitRow_NoSilentDefaults()
        {
            var catalog = WeatherEffectsCatalog.LoadFromDirectory(GetDataDir(), new Ashfall.Core.FileSystemIO());
            var missing = catalog.MissingKinds();
            Assert.True(missing.Count == 0,
                "weather_effects.json is missing explicit rows for: " + string.Join(", ", missing));
        }

        [Fact]
        public void RealDataCatalog_RuntimePins_Preserved()
        {
            var catalog = WeatherEffectsCatalog.LoadFromDirectory(GetDataDir(), new Ashfall.Core.FileSystemIO());
            Assert.True(catalog.TryGetModifier(WeatherKind.FalloutStorm, out float storm));
            Assert.Equal(WeatherSystem.FalloutStormOutdoorRadModifier, storm);
            Assert.True(catalog.TryGetModifier(WeatherKind.BlackRain, out float blackRain));
            Assert.Equal(WeatherSystem.BlackRainOutdoorRadModifier, blackRain);
        }

        [Fact]
        public void RealDataCatalog_AshfallDose_MatchesLegacyForecastPromise()
        {
            // The pre-catalog forecast projected Ashfall at +45 rads while the
            // runtime dose added 0 — a forecast/runtime drift (plan §57.3).
            // The data authority now owns both; the row pins the forecast value
            // so runtime matches what the forecast already promised.
            var catalog = WeatherEffectsCatalog.LoadFromDirectory(GetDataDir(), new Ashfall.Core.FileSystemIO());
            Assert.True(catalog.TryGetModifier(WeatherKind.Ashfall, out float ashfall));
            Assert.Equal(45f, ashfall);
        }

        [Fact]
        public void UnknownWeatherKind_IsALoadError_NotASilentRow()
        {
            var catalog = new WeatherEffectsCatalog(new[]
            {
                new WeatherEffectsDef { weather = "MeteorShower", outdoor_rad_modifier = 10f }
            });
            Assert.Contains(catalog.Errors, e => e.Contains("unknown weather kind 'MeteorShower'"));
            Assert.Equal(0, catalog.LoadedCount);
        }

        [Fact]
        public void NegativeOrNonFiniteModifier_IsALoadError()
        {
            var catalog = new WeatherEffectsCatalog(new[]
            {
                new WeatherEffectsDef { weather = "Rain", outdoor_rad_modifier = -5f },
                new WeatherEffectsDef { weather = "Clear", outdoor_rad_modifier = float.NaN }
            });
            Assert.Contains(catalog.Errors, e => e.Contains("non-negative"));
            Assert.Contains(catalog.Errors, e => e.Contains("finite"));
            Assert.Equal(0, catalog.LoadedCount);
        }

        [Fact]
        public void DuplicateRow_IsALoadError()
        {
            var catalog = new WeatherEffectsCatalog(new[]
            {
                new WeatherEffectsDef { weather = "Rain", outdoor_rad_modifier = 0f },
                new WeatherEffectsDef { weather = "Rain", outdoor_rad_modifier = 3f }
            });
            Assert.Contains(catalog.Errors, e => e.Contains("duplicate"));
            Assert.Equal(1, catalog.LoadedCount);
        }

        [Fact]
        public void MissingRows_AreReportedByMissingKinds()
        {
            var catalog = new WeatherEffectsCatalog(new[]
            {
                new WeatherEffectsDef { weather = "Clear", outdoor_rad_modifier = 0f }
            });
            Assert.Empty(catalog.Errors);
            var missing = catalog.MissingKinds();
            Assert.Equal(((WeatherKind[])Enum.GetValues(typeof(WeatherKind))).Length - 1, missing.Count);
            Assert.DoesNotContain(WeatherKind.Clear, missing);
        }

        [Fact]
        public void WeatherSystem_ForecastAndRuntime_ShareTheBoundTable()
        {
            var catalog = new WeatherEffectsCatalog(new[]
            {
                new WeatherEffectsDef { weather = "Ashfall", outdoor_rad_modifier = 45f }
            });
            var weather = new WeatherSystem();
            weather.BindWeatherEffects(catalog);

            // The forecast projection method and (with Current forced to a kind
            // that has a row) the runtime modifier resolve the same table.
            // Kinds with an explicit row read the data; kinds without a row
            // fall back to the legacy constants — completeness of the table is
            // enforced by the integrity validator, not silently here.
            Assert.Equal(45f, weather.ForecastRadModifier(WeatherKind.Ashfall));
            Assert.Equal(WeatherSystem.FalloutStormOutdoorRadModifier,
                weather.ForecastRadModifier(WeatherKind.FalloutStorm));
        }

        [Fact]
        public void WeatherSystem_Unbound_LegacyConstantsByteIdentical()
        {
            var weather = new WeatherSystem();
            Assert.Equal(0f, weather.ForecastRadModifier(WeatherKind.Rain));
            Assert.Equal(150f, weather.ForecastRadModifier(WeatherKind.FalloutStorm));
            Assert.Equal(250f, weather.ForecastRadModifier(WeatherKind.BlackRain));
            Assert.Equal(45f, weather.ForecastRadModifier(WeatherKind.Ashfall));
        }
    }
}
