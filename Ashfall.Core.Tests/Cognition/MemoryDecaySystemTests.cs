// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Cognition;
using Xunit;

namespace Ashfall.Core.Tests.Cognition
{
    public sealed class MemoryDecaySystemTests
    {
        [Fact]
        public void RegisterOrUpdate_CreatesRecord_WithExpectedDefaults()
        {
            var system = new MemoryDecaySystem();

            var record = system.RegisterOrUpdate("dweller_1", MemoryDomain.Knowledge, "radiation_filtration_theory", day: 1, initialStrength: 90f);

            Assert.NotNull(record);
            Assert.Equal("dweller_1", record.SurvivorId);
            Assert.Equal(MemoryDomain.Knowledge, record.Domain);
            Assert.Equal("radiation_filtration_theory", record.ReferenceId);
            Assert.Equal(90f, record.Strength);
            Assert.Equal(MemoryClarity.Vivid, record.Clarity);
            Assert.Equal(1, system.RecordCount);
        }

        [Fact]
        public void Reinforce_IncreasesStrength_AndUpdatesClarity()
        {
            var system = new MemoryDecaySystem();
            system.RegisterOrUpdate("dweller_1", MemoryDomain.Skill, "surgery_technique", day: 1, initialStrength: 45f);

            ReinforcementRecord? firedReinf = null;
            system.OnMemoryReinforced += (rec, r) => firedReinf = r;

            bool ok = system.Reinforce("dweller_1", MemoryDomain.Skill, "surgery_technique", day: 10, ReinforcementType.Practice, boostAmount: 20f);

            Assert.True(ok);
            Assert.NotNull(firedReinf);
            Assert.Equal(ReinforcementType.Practice, firedReinf.Type);

            var mem = system.GetMemory("dweller_1", MemoryDomain.Skill, "surgery_technique");
            Assert.NotNull(mem);
            // 45 + 20 * 1.2 = 45 + 24 = 69
            Assert.Equal(69f, mem.Strength);
            Assert.Equal(MemoryClarity.Clear, mem.Clarity);
            Assert.Equal(10, mem.LastReinforcedDay);
        }

        [Fact]
        public void TickDay_DecaysStrengthAndClarity_FiresOnMemoryFaded()
        {
            var system = new MemoryDecaySystem();
            system.RegisterOrUpdate("dweller_2", MemoryDomain.EventMemory, "bunker_breach_anniversary", day: 1, initialStrength: 52f);

            ForgettingEvent? fadedEvent = null;
            system.OnMemoryFaded += (rec, ev) => fadedEvent = ev;

            // EventMemory base decay is 4.0 / day.
            // After 1 day, 52 - 4 = 48 (< 50, so clarity drops from Clear to Vague)
            system.TickDay(currentDay: 2);

            var mem = system.GetMemory("dweller_2", MemoryDomain.EventMemory, "bunker_breach_anniversary");
            Assert.NotNull(mem);
            Assert.Equal(48f, mem.Strength);
            Assert.Equal(MemoryClarity.Vague, mem.Clarity);

            Assert.NotNull(fadedEvent);
            Assert.Equal(MemoryClarity.Vague, fadedEvent.NewClarity);
        }

        [Fact]
        public void TickDay_CertifiedMemory_DecaysAtOneTenthRate()
        {
            var system = new MemoryDecaySystem();
            system.RegisterOrUpdate("dweller_3", MemoryDomain.Skill, "master_welder", day: 1, initialStrength: 100f, isCertified: true);

            // Skill base decay is 1.0f / day. Certified is 0.1x -> 0.1f / day.
            // After 10 days unreinforced: 10 * 0.1 = 1.0 lost -> 99.0
            for (int d = 2; d <= 11; d++)
            {
                system.TickDay(currentDay: d);
            }

            var mem = system.GetMemory("dweller_3", MemoryDomain.Skill, "master_welder");
            Assert.NotNull(mem);
            Assert.Equal(99.0f, mem.Strength, 2);
        }

        [Fact]
        public void TickDay_PreservedMemory_DoesNotDecay()
        {
            var system = new MemoryDecaySystem();
            system.RegisterOrUpdate("dweller_4", MemoryDomain.EventMemory, "first_successful_expedition", day: 1, initialStrength: 100f, isPreserved: true);

            for (int d = 2; d <= 20; d++)
            {
                system.TickDay(currentDay: d);
            }

            var mem = system.GetMemory("dweller_4", MemoryDomain.EventMemory, "first_successful_expedition");
            Assert.NotNull(mem);
            Assert.Equal(100f, mem.Strength);
            Assert.Equal(MemoryClarity.Vivid, mem.Clarity);
        }

        [Fact]
        public void TickDay_HaltAllDecay_SkipsAllDecay()
        {
            var system = new MemoryDecaySystem();
            system.RegisterOrUpdate("dweller_5", MemoryDomain.Knowledge, "geological_fault_map", day: 1, initialStrength: 80f);

            system.TickDay(currentDay: 5, haltAllDecay: () => true);

            var mem = system.GetMemory("dweller_5", MemoryDomain.Knowledge, "geological_fault_map");
            Assert.NotNull(mem);
            Assert.Equal(80f, mem.Strength);
        }

        [Fact]
        public void CaptureState_And_RestoreState_RoundTripsAccurately()
        {
            var system1 = new MemoryDecaySystem();
            system1.RegisterOrUpdate("dweller_6", MemoryDomain.Procedural, "lockpicking_touch", day: 1, initialStrength: 65f);
            system1.Reinforce("dweller_6", MemoryDomain.Procedural, "lockpicking_touch", day: 2, ReinforcementType.Review, boostAmount: 10f);

            var state = system1.CaptureState();
            Assert.Single(state.Records);
            Assert.Single(state.RecentReinforcements);

            var system2 = new MemoryDecaySystem();
            system2.RestoreState(state);

            Assert.Equal(1, system2.RecordCount);
            var mem = system2.GetMemory("dweller_6", MemoryDomain.Procedural, "lockpicking_touch");
            Assert.NotNull(mem);
            Assert.Equal(75f, mem.Strength);
            Assert.Equal(MemoryClarity.Vivid, mem.Clarity);
        }

        [Fact]
        public void ProjectCanonicalSources_IsDeterministicAndDoesNotOwnSourceState()
        {
            var system = new MemoryDecaySystem();
            system.RegisterOrUpdate("dweller_1", MemoryDomain.Skill, "canonical_skill", day: 1, initialStrength: 80f);
            var facts = new[]
            {
                new CanonicalMemoryFact
                {
                    SurvivorId = "dweller_2",
                    Domain = MemoryDomain.EventMemory,
                    SourceId = "journal:event_b",
                    Strength = 60f,
                    LastReinforcedDay = 4
                },
                new CanonicalMemoryFact
                {
                    SurvivorId = "dweller_1",
                    Domain = MemoryDomain.Skill,
                    SourceId = "skill:medical",
                    Strength = 80f,
                    LastReinforcedDay = 9,
                    IsCertified = true
                }
            };

            var projected = MemoryDecaySystem.ProjectCanonicalSources(facts, currentDay: 10);

            Assert.Equal(2, projected.Count);
            Assert.Equal("dweller_1", projected[0].SurvivorId);
            Assert.Equal("skill:medical", projected[0].RecordId);
            Assert.Equal(79.9f, projected[0].Strength, 2);
            Assert.Equal(36f, projected[1].Strength, 2);
            Assert.Equal(1, system.RecordCount);
            Assert.Equal(80f, system.GetMemory("dweller_1", MemoryDomain.Skill, "canonical_skill")!.Strength);
        }

        [Fact]
        public void ProjectCanonicalSources_PreservesCertifiedAndPreservedFacts()
        {
            var projected = MemoryDecaySystem.ProjectCanonicalSources(
                new[]
                {
                    new CanonicalMemoryFact
                    {
                        SurvivorId = "dweller_1",
                        Domain = MemoryDomain.Knowledge,
                        SourceId = "certified",
                        Strength = 100f,
                        LastReinforcedDay = 1,
                        IsCertified = true
                    },
                    new CanonicalMemoryFact
                    {
                        SurvivorId = "dweller_1",
                        Domain = MemoryDomain.EventMemory,
                        SourceId = "preserved",
                        Strength = 100f,
                        LastReinforcedDay = 1,
                        IsPreserved = true
                    }
                },
                currentDay: 11);

            Assert.Equal(97.5f, projected[0].Strength, 2);
            Assert.Equal(100f, projected[1].Strength);
        }
    }
}
