// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Muster;
using Xunit;

namespace Ashfall.Core.Tests.Factions
{
    public sealed class Plan25FactionEcologyTests
    {
        private static string DataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("StreamingAssets/Data was not found");
        }

        [Fact]
        public void T01_RegionalTreatyCatalogIntegratesWithHostSession()
        {
            var treaties = RegionalTreatyCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Equal(5, treaties.Count);

            var system = new RegionalTreatySystem();
            system.LoadCatalog(treaties);

            var proposeRes = system.Propose("road_iron_charter");
            Assert.True(proposeRes.IsSuccess, proposeRes.FailureCode);

            var ratifyRes = system.Ratify("road_iron_charter", 30);
            Assert.True(ratifyRes.IsSuccess, ratifyRes.FailureCode);

            Assert.True(system.IsActive("road_iron_charter"));
            Assert.Equal(1, system.CountByStatus(TreatyStatus.Ratified));
        }

        [Fact]
        public void T02_UnifiedStandingReadModelMapsAllBands()
        {
            Assert.Equal(FactionActionBands.Hostile, FactionActionBoard.BandForTrust(0f));
            Assert.Equal(FactionActionBands.Hostile, FactionActionBoard.BandForTrust(-5f));
            Assert.Equal(FactionActionBands.Poor, FactionActionBoard.BandForTrust(2f));
            Assert.Equal(FactionActionBands.Neutral, FactionActionBoard.BandForTrust(6f));
            Assert.Equal(FactionActionBands.Good, FactionActionBoard.BandForTrust(12f));
            Assert.Equal(FactionActionBands.Allied, FactionActionBoard.BandForTrust(20f));

            var board = new FactionActionBoard();
            Assert.Equal(FactionActionBands.Hostile, board.ComputeBand(FactionActionBoard.FactionScavengerGuild));
            Assert.Equal(FactionActionBands.Hostile, board.ComputeBand(FactionActionBoard.FactionHydroBarons));
            Assert.Equal(FactionActionBands.Hostile, board.ComputeBand(FactionActionBoard.FactionDeserterCoalition));
        }

        private sealed class TestLivingGate : IWitnessEligibility
        {
            private readonly HashSet<string> _alive;
            public TestLivingGate(IEnumerable<string> alive) => _alive = new HashSet<string>(alive, StringComparer.Ordinal);
            public bool IsFlagSet(string flagId) => true;
            public bool IsSubjectAlive(string subjectId) => _alive.Contains(subjectId);
            public bool IsFactionPresent(string factionId) => true;
        }

        [Fact]
        public void T03_WitnessSelectionFiltersDeadSubjects()
        {
            var witness = new WitnessDefinition
            {
                id = "witness_test_subject",
                subjectId = "survivor_dr_chen",
                dayMin = 10,
                priority = 50,
                testimonies = new List<WitnessTestimony>
                {
                    new WitnessTestimony
                    {
                        variantId = "v1_default",
                        body = "I saw what happened."
                    }
                }
            };
            var catalog = new List<WitnessDefinition> { witness };

            // When alive, candidate is eligible
            var livingGate = new TestLivingGate(new[] { "survivor_dr_chen" });
            var livingResult = WitnessSelector.Select(catalog, 15, livingGate, maxCount: 1);
            Assert.Single(livingResult);
            Assert.Equal("witness_test_subject", livingResult[0].Witness.id);

            // When deceased, witness is filtered out (dead subjects never testify)
            var deadGate = new TestLivingGate(Array.Empty<string>());
            var deadResult = WitnessSelector.Select(catalog, 15, deadGate, maxCount: 1);
            Assert.Empty(deadResult);
        }

        private sealed class MockItemSink : IFactionActionItemSink
        {
            public readonly List<(string ItemId, int Amount)> Deliveries = new List<(string, int)>();
            public bool Deliver(string itemId, int amount)
            {
                Deliveries.Add((itemId, amount));
                return true;
            }
        }

        [Fact]
        public void T04_FactionActionBoardItemSinkDeliversGoods()
        {
            var board = new FactionActionBoard();
            var action = new FactionActionDefinition
            {
                id = "act_water_filter_exchange",
                factionId = FactionActionBoard.FactionHydroBarons,
                minDay = 1,
                variants = new List<FactionActionVariant>
                {
                    new FactionActionVariant
                    {
                        band = FactionActionBands.Neutral,
                        text = "Filter trade",
                        choices = new List<FactionActionChoice>
                        {
                            new FactionActionChoice
                            {
                                choiceId = "choice_receive_filter",
                                text = "Accept filter",
                                effects = new FactionActionEffects
                                {
                                    itemId = "item_water_filter_advanced",
                                    itemAmount = 2
                                }
                            }
                        }
                    }
                }
            };
            board.SetCatalog(new[] { action });

            var mockSink = new MockItemSink();
            bool resolved = board.Resolve("act_water_filter_exchange", "choice_receive_filter", day: 5, itemSink: mockSink);
            Assert.True(resolved);

            Assert.Single(mockSink.Deliveries);
            Assert.Equal("item_water_filter_advanced", mockSink.Deliveries[0].ItemId);
            Assert.Equal(2, mockSink.Deliveries[0].Amount);
        }

        [Fact]
        public void T05_TreatyTransitionsDriveEscalationAndRaidPressure()
        {
            var system = new RegionalTreatySystem();
            system.LoadCatalog(new List<TreatyDefinition>
            {
                new TreatyDefinition
                {
                    treaty_id = "non_aggression_pact",
                    display_name = "Non-Aggression Pact",
                    faction_id = "iron_raiders",
                    ratification_cost_scrap = 10f,
                    effects = new List<TreatyEffect>
                    {
                        new TreatyEffect
                        {
                            effect_type = "raid_pressure_relief",
                            target_id = "shelter_perimeter",
                            value = 0.25f
                        }
                    }
                }
            });

            // Baseline raid pressure modifier is 0
            Assert.Equal(0f, system.GetRaidPressureModifier());

            // Ratifying relief pact lowers raid pressure
            system.Propose("non_aggression_pact");
            system.Ratify("non_aggression_pact", 10);
            Assert.Equal(-0.25f, system.GetRaidPressureModifier());

            // Breaking the treaty removes relief and incurs breach penalty
            system.BreakTreaty("non_aggression_pact");
            Assert.Equal(TreatyEffectTable.BreachRaidPressure, system.GetRaidPressureModifier());
            Assert.Equal(1, system.CountByStatus(TreatyStatus.Violated));
        }
    }
}
