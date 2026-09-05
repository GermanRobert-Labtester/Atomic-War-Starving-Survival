using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public sealed class WeatherGateCatalogLoaderTests
    {
        private readonly string _dataDir;
        private readonly IFileIO _fileIO;
        private readonly IJsonSerializer _jsonSerializer;

        public WeatherGateCatalogLoaderTests()
        {
            _dataDir = WeatherGateAuditSimulator.FindDataDir();
            _fileIO = new FileSystemIO();
            _jsonSerializer = new SystemTextJsonSerializer();
        }

        [Fact]
        public void ProductionJson_LoadsExpectedGates_WithZeroErrors()
        {
            var catalog = WeatherGateCatalogLoader.LoadFromDirectory(_dataDir, _fileIO, _jsonSerializer);

            Assert.True(catalog.IsValid, $"Expected valid catalog, but had errors: {string.Join("; ", catalog.Errors)}");
            Assert.Empty(catalog.Errors);
            Assert.Equal(18, catalog.Count);
            Assert.Equal(15, System.Linq.Enumerable.Count(catalog.GetAll(), g => g.GateType == "route"));
            Assert.Equal(3, System.Linq.Enumerable.Count(catalog.GetAll(), g => g.GateType == "destination"));
        }

        [Fact]
        public void DuplicateGateId_CollectsValidationError_DoesNotThrow()
        {
            string json = @"{
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
                        ""target"": ""route_02"",
                        ""blocked_weather"": [""Rain""]
                    }
                ]
            }";

            var catalog = WeatherGateCatalogLoader.LoadFromJson(json);

            Assert.False(catalog.IsValid);
            Assert.Contains(catalog.Errors, e => e.Contains("duplicate gate id 'gate_dup'"));
            Assert.Single(catalog.GetAll());
        }

        [Fact]
        public void InvalidWeatherId_CollectsValidationError()
        {
            string json = @"{
                ""schema_version"": 1,
                ""gates"": [
                    {
                        ""id"": ""gate_invalid_weather"",
                        ""gate_type"": ""route"",
                        ""target"": ""route_01"",
                        ""blocked_weather"": [""SolarFlareSuperstorm""]
                    }
                ]
            }";

            var catalog = WeatherGateCatalogLoader.LoadFromJson(json);

            Assert.False(catalog.IsValid);
            Assert.Contains(catalog.Errors, e => e.Contains("references unknown weather kind 'SolarFlareSuperstorm'"));
        }

        [Fact]
        public void RequiredAndBlockedOverlap_CollectsValidationError()
        {
            string json = @"{
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
            }";

            var catalog = WeatherGateCatalogLoader.LoadFromJson(json);

            Assert.False(catalog.IsValid);
            Assert.Contains(catalog.Errors, e => e.Contains("both required and blocked"));
        }

        [Fact]
        public void TargetLookup_MountainPassRoute_ReturnsExpectedGate()
        {
            var catalog = WeatherGateCatalogLoader.LoadFromDirectory(_dataDir, _fileIO, _jsonSerializer);

            var gate = catalog.GetByTarget("route_12_the_cloud_eyrie_meteorological_ascent");

            Assert.NotNull(gate);
            Assert.Equal("gate_mountain_pass_blizzard", gate.Id);
            Assert.Equal("route_12_the_cloud_eyrie_meteorological_ascent", gate.TargetId);
            Assert.Contains("Blizzard", gate.BlockedWeather);
        }

        [Fact]
        public void TargetLookup_NonexistentRoute_ReturnsNull()
        {
            var catalog = WeatherGateCatalogLoader.LoadFromDirectory(_dataDir, _fileIO, _jsonSerializer);

            var gate = catalog.GetByTarget("route_999_nonexistent_corridor");

            Assert.Null(gate);
        }

        [Fact]
        public void DeterministicCatalogLoad_IdenticalInputsProduceIdenticalStateAndErrorOrder()
        {
            string json = @"{
                ""schema_version"": 1,
                ""gates"": [
                    {
                        ""id"": ""gate_b"",
                        ""gate_type"": ""route"",
                        ""target"": ""route_02"",
                        ""blocked_weather"": [""Blizzard""]
                    },
                    {
                        ""id"": ""gate_a"",
                        ""gate_type"": ""route"",
                        ""target"": ""route_01"",
                        ""blocked_weather"": [""UnknownWeatherToken""]
                    }
                ]
            }";

            var cat1 = WeatherGateCatalogLoader.LoadFromJson(json);
            var cat2 = WeatherGateCatalogLoader.LoadFromJson(json);

            Assert.Equal(cat1.Errors, cat2.Errors);
            var gates1 = cat1.GetAll();
            var gates2 = cat2.GetAll();
            Assert.Equal(gates1.Count, gates2.Count);
            for (int i = 0; i < gates1.Count; i++)
            {
                Assert.Equal(gates1[i].Id, gates2[i].Id);
                Assert.Equal(gates1[i].TargetId, gates2[i].TargetId);
                Assert.Equal(gates1[i].BlockedWeather, gates2[i].BlockedWeather);
            }
        }

        [Fact]
        public void NewerSchemaVersion_SafelyRejected_WithDiagnostic()
        {
            string json = @"{
                ""schema_version"": 999,
                ""gates"": []
            }";

            var catalog = WeatherGateCatalogLoader.LoadFromJson(json);

            Assert.False(catalog.IsValid);
            Assert.Contains(catalog.Errors, e => e.Contains("is newer than supported version"));
            Assert.Empty(catalog.GetAll());
        }

        [Fact]
        public void MalformedJson_CollectsValidationError_DoesNotThrow()
        {
            string json = @"{ ""schema_version"": 1, ""gates"": [ { bad json";

            var catalog = WeatherGateCatalogLoader.LoadFromJson(json);

            Assert.False(catalog.IsValid);
            Assert.NotEmpty(catalog.Errors);
            Assert.Contains(catalog.Errors, e => e.Contains("malformed JSON"));
        }

        [Fact]
        public void AbsentOptionalArrays_NormalizesToEmptyDeterministicLists()
        {
            string json = @"{
                ""schema_version"": 1,
                ""gates"": [
                    {
                        ""id"": ""gate_sparse"",
                        ""gate_type"": ""route"",
                        ""target"": ""route_sparse"",
                        ""blocked_weather"": [""Rain""]
                    }
                ]
            }";

            var catalog = WeatherGateCatalogLoader.LoadFromJson(json);

            var gate = catalog.GetById("gate_sparse");
            Assert.NotNull(gate);
            Assert.NotNull(gate.RequiredWeather);
            Assert.Empty(gate.RequiredWeather);
            Assert.Equal(string.Empty, gate.OverrideItem);
            Assert.Equal(string.Empty, gate.OverrideSkill);
            Assert.Equal(string.Empty, gate.ConsequenceOnForce);
        }
    }
}
