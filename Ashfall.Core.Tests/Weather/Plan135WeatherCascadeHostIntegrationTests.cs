// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 135 — Weather → Deep Gameplay Cascade: host-integration gate.
//
// Pins the production wiring contract for the cascade:
//   * the authored template table loads strictly and rejects a bad row,
//   * severity is derived from the canonical weather-effects authority only,
//     never invented and never fabricated when the authority is unbound,
//   * the engine exposes a validated-template binding path and a schema-gated
//     restore (so a wrong version cannot be half-applied),
//   * the cascade section and the CLI probe are registered.
//
// Route/owner behaviour itself is covered by the LIVING probe
// (--weather-cascade-selftest) and by Plan135WeatherCascadeIntegrationTests.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Weather;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Weather
{
    public sealed class Plan135WeatherCascadeHostIntegrationTests
    {
        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent;
            return dir?.FullName ?? throw new InvalidOperationException("repo root not found");
        }

        private static string DataDir() =>
            Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");

        // ── 1 — the authored cascade table is real, strict, and complete ────

        [Fact]
        public void AuthoredCascadeTable_LoadsStrictly_ThroughTheCoreLoader()
        {
            var load = Ashfall.Core.Records.WeatherCascadeCatalogLoader.Load(
                DataDir(), new Ashfall.Core.FileSystemIO());

            Assert.False(load.HasErrors, string.Join("; ", load.Errors));
            Assert.NotEmpty(load.Templates);
        }

        [Fact]
        public void AuthoredCascadeTable_NamesRealWeatherKinds_AndKnownSystems()
        {
            var load = Ashfall.Core.Records.WeatherCascadeCatalogLoader.Load(
                DataDir(), new Ashfall.Core.FileSystemIO());
            Assert.False(load.HasErrors, string.Join("; ", load.Errors));

            foreach (var template in load.Templates)
            {
                Assert.True(
                    Enum.TryParse<WeatherKind>(template.weather_kind, ignoreCase: true, out _),
                    $"cascade template '{template.id}' names weather_kind '{template.weather_kind}', "
                    + "which is not a WeatherKind.");
                Assert.Contains(template.target_system,
                    Ashfall.Core.Records.WeatherCascadeCatalogLoader.KnownTargetSystems);
                Assert.Contains(template.effect_type,
                    Ashfall.Core.Records.WeatherCascadeCatalogLoader.KnownEffectTypes);
                Assert.True(template.duration_days >= 1);
                Assert.False(float.IsNaN(template.magnitude) || float.IsInfinity(template.magnitude));
            }
        }

        [Fact]
        public void StrictLoader_RejectsAnUnknownWeatherKind()
        {
            const string bad = "{\"schema_version\":1,\"cascade_templates\":[{\"id\":\"bad\","
                + "\"weather_kind\":\"NotAWeatherKind\",\"target_system\":\"shelter\","
                + "\"effect_type\":\"damage\",\"magnitude\":1.0,\"duration_days\":1}]}";

            var load = Ashfall.Core.Records.WeatherCascadeCatalogLoader.LoadFromJson(bad);

            Assert.True(load.HasErrors);
        }

        [Fact]
        public void StrictLoader_RejectsAnEmptyTable_RatherThanFallingBackToGeneratedEffects()
        {
            const string bad = "{\"schema_version\":1,\"cascade_templates\":[]}";
            var load = Ashfall.Core.Records.WeatherCascadeCatalogLoader.LoadFromJson(bad);
            Assert.True(load.HasErrors);
        }

        [Fact]
        public void StrictLoader_RejectsASubDayDuration()
        {
            const string bad = "{\"schema_version\":1,\"cascade_templates\":[{\"id\":\"bad\","
                + "\"weather_kind\":\"Blizzard\",\"target_system\":\"shelter\","
                + "\"effect_type\":\"damage\",\"magnitude\":1.0,\"duration_days\":0}]}";

            Assert.True(Ashfall.Core.Records.WeatherCascadeCatalogLoader.LoadFromJson(bad).HasErrors);
        }

        // ── 2 — severity is the canonical weather authority, not an invention ──

        [Fact]
        public void Severity_ReadsOnlyTheCanonicalWeatherEffectsTable()
        {
            var effects = WeatherEffectsCatalog.LoadFromDirectory(
                DataDir(), new Ashfall.Core.FileSystemIO());
            Assert.NotNull(effects);
            Assert.Empty(effects!.MissingKinds());

            // Every kind gets exactly the same value twice: no RNG, no cache,
            // no per-call drift.
            foreach (WeatherKind kind in Enum.GetValues(typeof(WeatherKind)))
            {
                float a = WeatherCascadeSeverity.SeverityFor(kind, effects);
                float b = WeatherCascadeSeverity.SeverityFor(kind, effects);
                Assert.Equal(a, b);
                Assert.InRange(a, 0f, 100f);
            }
        }

        [Fact]
        public void Severity_FollowsTheAuthoredHazardOrdering()
        {
            var effects = WeatherEffectsCatalog.LoadFromDirectory(
                DataDir(), new Ashfall.Core.FileSystemIO());

            float clear = WeatherCascadeSeverity.SeverityFor(WeatherKind.Clear, effects);
            float falseSpring = WeatherCascadeSeverity.SeverityFor(WeatherKind.FalseSpring, effects);
            float iceStorm = WeatherCascadeSeverity.SeverityFor(WeatherKind.IceStorm, effects);
            float blizzard = WeatherCascadeSeverity.SeverityFor(WeatherKind.Blizzard, effects);
            float fallout = WeatherCascadeSeverity.SeverityFor(WeatherKind.FalloutStorm, effects);
            float blackRain = WeatherCascadeSeverity.SeverityFor(WeatherKind.BlackRain, effects);

            Assert.Equal(0f, clear, 3);
            Assert.True(falseSpring < iceStorm, "false_spring must be the gentler front");
            Assert.True(iceStorm < blizzard, "ice_storm must be gentler than blizzard");
            Assert.True(blizzard < fallout, "blizzard must be gentler than fallout_storm");
            Assert.True(fallout < blackRain, "fallout_storm must be gentler than black_rain");
        }

        [Fact]
        public void Severity_IsNotFabricatedWhenTheWeatherAuthorityIsUnbound()
        {
            // An unbound catalog means "no cascade", not "average cascade".
            Assert.Equal(0f, WeatherCascadeSeverity.SeverityFor(WeatherKind.BlackRain, null));
        }

        [Fact]
        public void Severity_AxisWeightsAreCompletePermille()
        {
            int total = WeatherCascadeSeverity.VisibilityWeightPermille
                + WeatherCascadeSeverity.ThermalWeightPermille
                + WeatherCascadeSeverity.RadWeightPermille
                + WeatherCascadeSeverity.TravelDelayWeightPermille
                + WeatherCascadeSeverity.EncounterWeightPermille;
            Assert.Equal(1000, total);
        }

        // ── 3 — the engine's strict binding path and schema gate ────────────

        [Fact]
        public void Engine_BindsOnlyValidatedTemplates()
        {
            var engine = new WeatherGameplayCascadeEngine();
            Assert.Equal(0, engine.BindValidatedTemplates(null));

            var row = new Ashfall.Core.Weather.WeatherEffectDef
            {
                id = "probe_row", weather_kind = "Blizzard",
                target_system = "shelter", effect_type = "damage",
                magnitude = 10f, duration_days = 2
            };
            Assert.Equal(1, engine.BindValidatedTemplates(new[] { row }));
            Assert.Single(engine.Templates);

            // An id-less row is never bound: the host's strict loader is the
            // only accepted source of a template.
            Assert.Equal(0, engine.BindValidatedTemplates(
                new[] { new Ashfall.Core.Weather.WeatherEffectDef { id = "" } }));
        }

        [Fact]
        public void Engine_RejectsAnUnknownStateSchemaVersion()
        {
            var engine = new WeatherGameplayCascadeEngine();
            var wrong = new WeatherCascadeState { schema_version = 99 };
            Assert.Throws<InvalidOperationException>(() => engine.RestoreState(wrong));
        }

        [Fact]
        public void Engine_RoundTripsACapturedState()
        {
            var engine = new WeatherGameplayCascadeEngine();
            var row = new Ashfall.Core.Weather.WeatherEffectDef
            {
                id = "probe_row", weather_kind = "Blizzard",
                target_system = "shelter", effect_type = "damage",
                magnitude = 25f, duration_days = 3
            };
            engine.BindValidatedTemplates(new[] { row });
            engine.EvaluateWeatherCascade(WeatherKind.Blizzard, 60f, 4);

            var captured = engine.CaptureState();
            var restored = new WeatherGameplayCascadeEngine();
            restored.RestoreState(captured);

            Assert.Equal(captured.activeEvents.Count, restored.State.activeEvents.Count);
            Assert.Equal(captured.activeEffects.Count, restored.State.activeEffects.Count);
        }

        [Fact]
        public void Engine_TickDayExpiresAnEvent_AndArchivesItToHistory()
        {
            var engine = new WeatherGameplayCascadeEngine();
            engine.BindValidatedTemplates(new[]
            {
                new Ashfall.Core.Weather.WeatherEffectDef
                {
                    id = "probe_row", weather_kind = "Blizzard",
                    target_system = "shelter", effect_type = "damage",
                    magnitude = 25f, duration_days = 1
                }
            });
            engine.EvaluateWeatherCascade(WeatherKind.Blizzard, 60f, 4);

            engine.TickDay(5);
            Assert.Empty(engine.State.activeEvents);
            Assert.Single(engine.State.eventHistory);
            Assert.Empty(engine.State.activeEffects);
        }

        // ── 4 — the cascade is registered as a first-class campaign section ──

        [Fact]
        public void CascadeSection_IsRegistered_WithAFilenameAndASetupMethod()
        {
            var section = Ashfall.Core.Save.SaveSectionRegistry.All
                .FirstOrDefault(s => s.SectionKey == "weather_cascade");

            Assert.NotNull(section);
            Assert.Equal("SaveWeatherCascade", section!.SaveMethod);
            Assert.Equal("SetupWeatherCascade", section.SetupMethod);
            Assert.Contains("Plan 135", section.Description, StringComparison.Ordinal);
        }

        [Fact]
        public void CascadeSection_HasAPersistentFilename()
        {
            var filenames = Ashfall.Core.Save.SaveSectionRegistry.SectionFileNames;
            Assert.True(filenames.TryGetValue("weather_cascade", out string? fileName),
                "the registered section must resolve a persistent filename");
            Assert.Equal("weather_cascade_save.json", fileName);
        }

        [Fact]
        public void CascadeProbe_IsRegisteredInTheHostCliRegistry()
        {
            var action = Ashfall.Core.HostCliRegistry.AllDescriptors
                .FirstOrDefault(a => a.Action == Ashfall.Core.HostCliAction.WeatherCascadeSelfTest);

            Assert.NotNull(action);
            Assert.Contains("--weather-cascade-selftest", action!.AllFlags);
            Assert.Contains("Plan 135", action.Description, StringComparison.Ordinal);
        }
    }
}
