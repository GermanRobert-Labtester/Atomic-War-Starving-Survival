using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public sealed class CatalogIntegrityWeatherGateTests
    {
        private readonly string _dataDir;

        public CatalogIntegrityWeatherGateTests()
        {
            _dataDir = WeatherGateAuditSimulator.FindDataDir();
        }

        private static CatalogIntegrityReport ValidateScratch(Action<string> seed)
        {
            string scratch = Path.Combine(Path.GetTempPath(), "ashfall_weather_gate_test_" + Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(scratch);
                seed(scratch);
                return CatalogIntegrityValidator.Validate(scratch, new FileSystemIO());
            }
            finally
            {
                if (Directory.Exists(scratch))
                    Directory.Delete(scratch, true);
            }
        }

        [Fact]
        public void ProductionCatalog_ReportsZeroWeatherGateErrors()
        {
            Assert.False(string.IsNullOrEmpty(_dataDir), "StreamingAssets/Data directory must exist");
            var report = CatalogIntegrityValidator.Validate(_dataDir, new FileSystemIO());

            var weatherGateErrors = report.Errors
                .Where(e => e.Contains("weather_route_gates.json") || e.Contains("gate_"))
                .ToList();

            Assert.True(weatherGateErrors.Count == 0, string.Join("\n", weatherGateErrors));
        }

        [Fact]
        public void InvalidWeatherId_EmitsIntegrityError()
        {
            var report = ValidateScratch(dir =>
            {
                File.WriteAllText(Path.Combine(dir, "routes.json"),
                    "{\"schema_version\":1,\"routes\":[{\"route_id\":\"route_01\"}]}");
                File.WriteAllText(Path.Combine(dir, "weather_route_gates.json"), @"{
                    ""schema_version"": 1,
                    ""gates"": [
                        {
                            ""id"": ""gate_bad_weather"",
                            ""gate_type"": ""route"",
                            ""target"": ""route_01"",
                            ""blocked_weather"": [""SuperstormXYZ""]
                        }
                    ]
                }");
            });

            Assert.Contains(report.Errors, e => e.Contains("references unknown weather kind 'SuperstormXYZ'"));
        }

        [Fact]
        public void NonexistentTarget_EmitsIntegrityError()
        {
            var report = ValidateScratch(dir =>
            {
                File.WriteAllText(Path.Combine(dir, "weather_route_gates.json"), @"{
                    ""schema_version"": 1,
                    ""gates"": [
                        {
                            ""id"": ""gate_bad_target"",
                            ""gate_type"": ""route"",
                            ""target"": ""route_nowhere_99999"",
                            ""blocked_weather"": [""Blizzard""]
                        }
                    ]
                }");
            });

            Assert.Contains(report.Errors, e => e.Contains("unresolved target 'route_nowhere_99999'"));
        }

        [Fact]
        public void NonexistentOverrideItem_EmitsIntegrityError()
        {
            var report = ValidateScratch(dir =>
            {
                File.WriteAllText(Path.Combine(dir, "routes.json"),
                    "{\"schema_version\":1,\"routes\":[{\"route_id\":\"route_01\"}]}");
                File.WriteAllText(Path.Combine(dir, "weather_route_gates.json"), @"{
                    ""schema_version"": 1,
                    ""gates"": [
                        {
                            ""id"": ""gate_bad_item"",
                            ""gate_type"": ""route"",
                            ""target"": ""route_01"",
                            ""blocked_weather"": [""BioFog""],
                            ""override_item"": ""item_missing_laser_cannon""
                        }
                    ]
                }");
            });

            Assert.Contains(report.Errors, e => e.Contains("unresolved override_item 'item_missing_laser_cannon'"));
        }

        [Fact]
        public void RequiredAndBlockedOverlap_EmitsIntegrityError()
        {
            var report = ValidateScratch(dir =>
            {
                File.WriteAllText(Path.Combine(dir, "routes.json"),
                    "{\"schema_version\":1,\"routes\":[{\"route_id\":\"route_01\"}]}");
                File.WriteAllText(Path.Combine(dir, "weather_route_gates.json"), @"{
                    ""schema_version"": 1,
                    ""gates"": [
                        {
                            ""id"": ""gate_overlap"",
                            ""gate_type"": ""route"",
                            ""target"": ""route_01"",
                            ""blocked_weather"": [""Blizzard""],
                            ""required_weather"": [""Blizzard""]
                        }
                    ]
                }");
            });

            Assert.Contains(report.Errors, e => e.Contains("both required and blocked"));
        }

        [Fact]
        public void DuplicateGateId_EmitsIntegrityError()
        {
            var report = ValidateScratch(dir =>
            {
                File.WriteAllText(Path.Combine(dir, "routes.json"),
                    "{\"schema_version\":1,\"routes\":[{\"route_id\":\"route_01\"}]}");
                File.WriteAllText(Path.Combine(dir, "weather_route_gates.json"), @"{
                    ""schema_version"": 1,
                    ""gates"": [
                        {
                            ""id"": ""gate_dup"",
                            ""gate_type"": ""route"",
                            ""target"": ""route_01"",
                            ""blocked_weather"": [""Blizzard""]
                        },
                        {
                            ""id"": ""gate_dup"",
                            ""gate_type"": ""route"",
                            ""target"": ""route_01"",
                            ""blocked_weather"": [""Rain""]
                        }
                    ]
                }");
            });

            Assert.Contains(report.Errors, e => e.Contains("duplicate gate id 'gate_dup'"));
        }

        [Fact]
        public void InvalidGateType_EmitsIntegrityError()
        {
            var report = ValidateScratch(dir =>
            {
                File.WriteAllText(Path.Combine(dir, "weather_route_gates.json"), @"{
                    ""schema_version"": 1,
                    ""gates"": [
                        {
                            ""id"": ""gate_bad_type"",
                            ""gate_type"": ""hyperspace"",
                            ""target"": ""route_01"",
                            ""blocked_weather"": [""Blizzard""]
                        }
                    ]
                }");
            });

            Assert.Contains(report.Errors, e => e.Contains("invalid gate_type 'hyperspace'"));
        }

        [Fact]
        public void NullOrEmptyOptionalOverrideItem_ProducesNoFalsePositiveReferenceError()
        {
            var report = ValidateScratch(dir =>
            {
                File.WriteAllText(Path.Combine(dir, "routes.json"),
                    "{\"schema_version\":1,\"routes\":[{\"route_id\":\"route_01\"}]}");
                File.WriteAllText(Path.Combine(dir, "weather_route_gates.json"), @"{
                    ""schema_version"": 1,
                    ""gates"": [
                        {
                            ""id"": ""gate_empty_override"",
                            ""gate_type"": ""route"",
                            ""target"": ""route_01"",
                            ""blocked_weather"": [""Blizzard""],
                            ""override_item"": """"
                        }
                    ]
                }");
            });

            var overrideErrors = report.Errors
                .Where(e => e.Contains("override_item"))
                .ToList();

            Assert.Empty(overrideErrors);
        }

        [Fact]
        public void DeterministicErrorOrdering_AcrossRuns()
        {
            void Seed(string dir)
            {
                File.WriteAllText(Path.Combine(dir, "weather_route_gates.json"), @"{
                    ""schema_version"": 1,
                    ""gates"": [
                        {
                            ""id"": ""gate_z"",
                            ""gate_type"": ""invalid_type_z"",
                            ""target"": ""route_missing_z"",
                            ""blocked_weather"": [""UnknownZ""]
                        },
                        {
                            ""id"": ""gate_a"",
                            ""gate_type"": ""invalid_type_a"",
                            ""target"": ""route_missing_a"",
                            ""blocked_weather"": [""UnknownA""]
                        }
                    ]
                }");
            }

            var report1 = ValidateScratch(Seed);
            var report2 = ValidateScratch(Seed);

            Assert.NotEmpty(report1.Errors);
            Assert.Equal(report1.Errors, report2.Errors);
        }

        [Fact]
        public void GenericTargetKey_NoUnrelatedCatalogRegressions()
        {
            var report = ValidateScratch(dir =>
            {
                // A quest or narrative catalog using target: "food" (plain string, not ID)
                File.WriteAllText(Path.Combine(dir, "quests_sample.json"), @"{
                    ""schema_version"": 1,
                    ""quests"": [
                        {
                            ""id"": ""quest_sample_01"",
                            ""target"": ""food""
                        }
                    ]
                }");
            });

            var targetErrors = report.Errors
                .Where(e => e.Contains("food") || e.Contains("unresolved reference 'food'"))
                .ToList();

            Assert.Empty(targetErrors);
        }
    }
}
