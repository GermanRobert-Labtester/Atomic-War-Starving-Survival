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
                new WeatherEffectsDef { weather = "Rain", visibility_modifier = 0.85f },
                new WeatherEffectsDef { weather = "Rain", visibility_modifier = 0.85f, outdoor_rad_modifier = 3f }
            });
            Assert.Contains(catalog.Errors, e => e.Contains("duplicate"));
            Assert.Equal(1, catalog.LoadedCount);
        }

        [Fact]
        public void MissingRows_AreReportedByMissingKinds()
        {
            var catalog = new WeatherEffectsCatalog(new[]
            {
                new WeatherEffectsDef { weather = "Clear", outdoor_rad_modifier = 0f, explicitly_neutral = true }
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

    /// <summary>
    /// C2 / Plan 20C (§35/§44) — full WeatherEffects record: every kind has an
    /// explicit entry with at least one mechanical effect, unless it declares
    /// itself explicitly neutral (no silent identity rows). Thermal + visibility
    /// consumers read the bound table; the legacy constants survive only as the
    /// unbound fallback.
    /// </summary>
    public sealed class WeatherEffectsCoverageGateTests
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

        private static WeatherEffectsCatalog Real()
            => WeatherEffectsCatalog.LoadFromDirectory(GetDataDir(), new Ashfall.Core.FileSystemIO());

        [Fact]
        public void EveryKind_HasMechanicalEffect_OrIsExplicitlyNeutral()
        {
            var catalog = Real();
            Assert.Empty(catalog.Errors);
            var neutral = new System.Collections.Generic.List<WeatherKind>();
            foreach (WeatherKind kind in Enum.GetValues(typeof(WeatherKind)))
            {
                Assert.True(catalog.TryGetEffects(kind, out var row),
                    $"missing effects row for {kind}");
                Assert.NotNull(row);
                bool hasEffect = row!.outdoor_rad_modifier != 0f
                    || row.visibility_modifier != 1.0f
                    || row.thermal_load_additive_c != 0f
                    || row.travel_speed_multiplier != 1.0f
                    || row.travel_encounter_multiplier != 1.0f
                    || row.trap_yield_multiplier != 1.0f
                    || row.caravan_availability_multiplier != 1.0f;
                if (!hasEffect)
                {
                    Assert.True(row.explicitly_neutral,
                        $"{kind} is all-identity without declaring explicitly_neutral (silent default)");
                    neutral.Add(kind);
                }
                else
                {
                    Assert.False(row.explicitly_neutral,
                        $"{kind} declares explicitly_neutral but has mechanical effects");
                }
            }
            // D3 (revised for trapping parity): Clear and Silence are the
            // intentionally neutral states — Silence's trapping penalty was
            // always 0 and it carries no other authored effect.
            Assert.Equal(new[] { WeatherKind.Clear, WeatherKind.Silence }, neutral);
        }

        [Fact]
        public void NewFields_ValidateRanges_RejectBadRows()
        {
            var bad = new WeatherEffectsCatalog(new[]
            {
                new WeatherEffectsDef { weather = "Rain", visibility_modifier = 1.5f },
                new WeatherEffectsDef { weather = "Blizzard", thermal_load_additive_c = -99f },
                new WeatherEffectsDef { weather = "Clear", explicitly_neutral = true, trap_yield_multiplier = 2f }
            });
            Assert.Contains(bad.Errors, e => e.Contains("out of range"));
            Assert.Contains(bad.Errors, e => e.Contains("declared explicitly_neutral but has mechanical effects"));
        }

        [Fact]
        public void ThermalAndVisibility_ReadTheBoundTable_LegacyFallbackWhenUnbound()
        {
            var catalog = new WeatherEffectsCatalog(new[]
            {
                new WeatherEffectsDef { weather = "IceStorm", visibility_modifier = 0.25f, thermal_load_additive_c = -20f }
            });
            var weather = new WeatherSystem();
            weather.BindWeatherEffects(catalog);

            // Parameterized consumers read the bound row…
            Assert.Equal(-20f, weather.TemperaturePenaltyC(WeatherKind.IceStorm), 3);
            Assert.Equal(0.25f, weather.VisibilityModifier(WeatherKind.IceStorm), 3);

            // …while an unbound system keeps the legacy constants (IceStorm
            // was absent from the legacy thermal switch → 0).
            var unbound = new WeatherSystem();
            Assert.Equal(0f, unbound.TemperaturePenaltyC(WeatherKind.IceStorm), 3);
            Assert.Equal(1f, unbound.VisibilityModifier(WeatherKind.IceStorm), 3);
        }
    }
}
