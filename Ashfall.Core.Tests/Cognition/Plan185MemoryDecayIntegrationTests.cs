// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 185: Memory & Knowledge Decay System — Integration Tests
// Verifies memory decay catalog loading, domain-specific decay rates,
// clarity level transitions, practice/review reinforcement, certification protection,
// and state save/restore persistence.
// ============================================================================
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core.Cognition;

namespace Ashfall.Core.Tests.Cognition
{
    public sealed class Plan185MemoryDecayIntegrationTests
    {
        private static string ResolveDataPath(string filename)
        {
            var candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates) { if (File.Exists(c)) return Path.GetFullPath(c); }
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename));
        }

        [Fact]
        public void LoadCatalog_LoadsDomainRatesAndClarityThresholds()
        {
            var system = new MemoryDecaySystem();
            string path = ResolveDataPath("memory_decay_rates.json");
            Assert.True(File.Exists(path), $"memory_decay_rates.json must exist at {path}");

            system.LoadCatalog(File.ReadAllText(path));

            var rates = system.GetAllDomainRates();
            Assert.NotEmpty(rates);
            Assert.Contains(rates, r => r.domain == "Skill");
            Assert.Contains(rates, r => r.domain == "Knowledge");
            Assert.Contains(rates, r => r.domain == "EventMemory");

            var skillRate = system.GetDomainRate(MemoryDomain.Skill);
            Assert.NotNull(skillRate);
            Assert.Equal(1.0f, skillRate!.base_daily_decay_rate);
        }

        [Fact]
        public void RegisterAndDecay_ReducesStrengthAndTransitionsClarity()
        {
            var system = new MemoryDecaySystem();
            system.RegisterOrUpdate("survivor_1", MemoryDomain.Knowledge, "radiation_theory", day: 1, initialStrength: 60f);

            // 60 strength is Clear (50-74).
            var initialMem = system.GetMemory("survivor_1", MemoryDomain.Knowledge, "radiation_theory");
            Assert.NotNull(initialMem);
            Assert.Equal(MemoryClarity.Clear, initialMem!.Clarity);

            // Tick 6 days: Knowledge base decay is 2.5/day -> 6 * 2.5 = 15 decay -> strength 45 (Vague)
            for (int d = 2; d <= 7; d++)
            {
                system.TickDay(d);
            }

            var decayedMem = system.GetMemory("survivor_1", MemoryDomain.Knowledge, "radiation_theory");
            Assert.NotNull(decayedMem);
            Assert.Equal(45f, decayedMem!.Strength);
            Assert.Equal(MemoryClarity.Vague, decayedMem.Clarity);
        }

        [Fact]
        public void PracticeReinforcement_RestoresStrengthAndFiresEvent()
        {
            var system = new MemoryDecaySystem();
            system.RegisterOrUpdate("survivor_2", MemoryDomain.Skill, "lockpicking", day: 1, initialStrength: 40f);

            ReinforcementRecord? record = null;
            system.OnMemoryReinforced += (mem, r) => record = r;

            bool success = system.Reinforce("survivor_2", MemoryDomain.Skill, "lockpicking", day: 10, ReinforcementType.Practice, boostAmount: 20f);

            Assert.True(success);
            Assert.NotNull(record);
            Assert.Equal(ReinforcementType.Practice, record!.Type);

            var mem = system.GetMemory("survivor_2", MemoryDomain.Skill, "lockpicking");
            Assert.NotNull(mem);
            Assert.True(mem!.Strength > 40f);
            Assert.Equal(10, mem.LastReinforcedDay);
        }

        [Fact]
        public void CertifiedSkills_DecayAtReducedScale()
        {
            var facts = new[]
            {
                new CanonicalMemoryFact
                {
                    SurvivorId = "survivor_cert",
                    Domain = MemoryDomain.Skill,
                    SourceId = "skill_medicine",
                    Strength = 100f,
                    LastReinforcedDay = 1,
                    IsCertified = true
                },
                new CanonicalMemoryFact
                {
                    SurvivorId = "survivor_uncert",
                    Domain = MemoryDomain.Skill,
                    SourceId = "skill_medicine",
                    Strength = 100f,
                    LastReinforcedDay = 1,
                    IsCertified = false
                }
            };

            // Project 10 days unreinforced
            var projected = MemoryDecaySystem.ProjectCanonicalSources(facts, currentDay: 11);

            var cert = projected.First(p => p.SurvivorId == "survivor_cert");
            var uncert = projected.First(p => p.SurvivorId == "survivor_uncert");

            // Uncertified decays at 1.0/day * 10 days = 10 lost -> 90
            // Certified decays at 0.1/day * 10 days = 1 lost -> 99
            Assert.True(cert.Strength > uncert.Strength);
            Assert.Equal(99f, cert.Strength, 1);
            Assert.Equal(90f, uncert.Strength, 1);
        }

        [Fact]
        public void ResolveClarity_MapsStrengthAcrossThresholds()
        {
            Assert.Equal(MemoryClarity.Forgotten, MemoryDecaySystem.ResolveClarity(0f));
            Assert.Equal(MemoryClarity.Fragmentary, MemoryDecaySystem.ResolveClarity(10f));
            Assert.Equal(MemoryClarity.Vague, MemoryDecaySystem.ResolveClarity(35f));
            Assert.Equal(MemoryClarity.Clear, MemoryDecaySystem.ResolveClarity(60f));
            Assert.Equal(MemoryClarity.Vivid, MemoryDecaySystem.ResolveClarity(85f));
        }

        [Fact]
        public void SaveRestoreState_PreservesRecordsAndRecentEvents()
        {
            var system = new MemoryDecaySystem();
            system.RegisterOrUpdate("survivor_saved", MemoryDomain.EventMemory, "first_winter", day: 1, initialStrength: 80f);
            system.TickDay(currentDay: 2);

            var state = system.CaptureState();

            var restored = new MemoryDecaySystem();
            restored.RestoreState(state);

            Assert.Equal(1, restored.RecordCount);
            var mem = restored.GetMemory("survivor_saved", MemoryDomain.EventMemory, "first_winter");
            Assert.NotNull(mem);
            Assert.Equal(MemoryDomain.EventMemory, mem!.Domain);
        }
    }
}
