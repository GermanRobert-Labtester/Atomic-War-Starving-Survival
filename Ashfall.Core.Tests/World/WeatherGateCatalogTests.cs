// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Gate catalog validation (section 16 / Q2). Load-time rules only;
    /// runtime precedence lives in the evaluator tests.
    /// </summary>
    public class WeatherGateCatalogTests
    {
        private static WeatherGate ValidGate(string id) => new WeatherGate
        {
            Id = id,
            TargetId = "route_01",
            BlockedWeather = { "Blizzard" }
        };

        [Fact]
        public void Register_NonEmptyId_Succeeds()
        {
            var catalog = new WeatherGateCatalog();
            catalog.Register(ValidGate("gate_a"));
            Assert.Equal(1, catalog.Count);
        }

        [Fact]
        public void Register_EmptyIdTable_Throws()
        {
            var failures = new List<string>();

            foreach (string? id in new[] { null, "", "   " })
            {
                var catalog = new WeatherGateCatalog();
                var gate = ValidGate("gate_a");
                gate.Id = id!;
                var exception = Record.Exception(() => catalog.Register(gate));
                if (exception is not WeatherGateCatalogException)
                {
                    failures.Add(
                        $"id '{id ?? "<null>"}' expected {nameof(WeatherGateCatalogException)}, got {exception?.GetType().Name ?? "no exception"}");
                }
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void Register_DuplicateId_Throws()
        {
            var catalog = new WeatherGateCatalog();
            catalog.Register(ValidGate("gate_a"));
            Assert.Throws<WeatherGateCatalogException>(() => catalog.Register(ValidGate("gate_a")));
        }

        [Fact]
        public void Register_EmptyTarget_Throws()
        {
            var catalog = new WeatherGateCatalog();
            var gate = ValidGate("gate_a");
            gate.TargetId = "";
            Assert.Throws<WeatherGateCatalogException>(() => catalog.Register(gate));
        }

        [Fact]
        public void Register_KnownWeatherKindTable_Succeeds()
        {
            var failures = new List<string>();

            foreach (string kind in new[]
            {
                "Blizzard", "BlackRain", "FalloutStorm", "BioFog", "EMPStorm", "IceStorm"
            })
            {
                var catalog = new WeatherGateCatalog();
                var gate = new WeatherGate
                {
                    Id = "gate_kind_" + kind,
                    TargetId = "route_01",
                    BlockedWeather = { kind }
                };
                var exception = Record.Exception(() => catalog.Register(gate));
                if (exception != null)
                {
                    failures.Add($"weather kind '{kind}' unexpectedly threw: {exception.Message}");
                }
                else if (catalog.Count != 1)
                {
                    failures.Add($"weather kind '{kind}' registered {catalog.Count} gates instead of 1");
                }
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void Register_UnknownWeatherKind_Throws()
        {
            var catalog = new WeatherGateCatalog();
            var gate = new WeatherGate
            {
                Id = "gate_unknown_kind",
                TargetId = "route_01",
                BlockedWeather = { "MeteorShower" }
            };
            Assert.Throws<WeatherGateCatalogException>(() => catalog.Register(gate));
        }

        [Fact]
        public void Register_DuplicateWeatherWithinGate_Throws()
        {
            var catalog = new WeatherGateCatalog();
            var gate = new WeatherGate
            {
                Id = "gate_dup_weather",
                TargetId = "route_01",
                BlockedWeather = { "Blizzard", "Blizzard" }
            };
            Assert.Throws<WeatherGateCatalogException>(() => catalog.Register(gate));
        }

        [Fact]
        public void Register_PositiveGateWithoutRequiredWeather_Throws()
        {
            var catalog = new WeatherGateCatalog();
            var gate = new WeatherGate
            {
                Id = "gate_positive_no_required",
                TargetId = "route_06",
                BlockedWeather = { "Blizzard" },
                RequiredWeather = { }
            };
            // positive gate ⇒ must define at least one required weather
            Assert.Throws<WeatherGateCatalogException>(() => catalog.Register(gate));
        }

        [Fact]
        public void Register_NegativeGateWithoutBlockedWeather_Throws()
        {
            var catalog = new WeatherGateCatalog();
            var gate = new WeatherGate
            {
                Id = "gate_negative_no_blocked",
                TargetId = "route_12",
                BlockedWeather = { },
                RequiredWeather = { "Blizzard" }
            };
            // negative gate ⇒ must define at least one blocked weather
            Assert.Throws<WeatherGateCatalogException>(() => catalog.Register(gate));
        }

        [Fact]
        public void Register_Contradiction_ThrowsWithExplicitPrecedence()
        {
            var catalog = new WeatherGateCatalog();
            var gate = new WeatherGate
            {
                Id = "gate_contradiction",
                TargetId = "route_12",
                BlockedWeather = { "Blizzard" },
                RequiredWeather = { "Blizzard" }
            };
            var ex = Assert.Throws<WeatherGateCatalogException>(() => catalog.Register(gate));
            // Documented precedence: blocked wins (fail-closed). The error
            // names the contradiction explicitly — never silently resolved.
            Assert.Contains("contradiction", ex.Message);
            Assert.Contains("blocked_weather wins", ex.Message);
        }

        [Fact]
        public void GetAll_EmptyCatalog_ReturnsEmpty()
        {
            var catalog = new WeatherGateCatalog();
            Assert.Empty(catalog.GetAll());
        }

        [Fact]
        public void GetAll_StableOrdering_RegistrationThenOrdinalId()
        {
            var catalog = new WeatherGateCatalog();
            catalog.Register(ValidGate("gate_b"));
            catalog.Register(ValidGate("gate_a"));
            catalog.Register(ValidGate("gate_c"));

            var ids = catalog.GetAll().Select(g => g.Id).ToList();

            // registration order preserved; ties broken by ordinal id sort
            Assert.Equal(new[] { "gate_a", "gate_b", "gate_c" }, ids);
        }
    }
}
