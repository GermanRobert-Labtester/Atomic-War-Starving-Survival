// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Progression
{
    public sealed class Plan33SkillCatalogExternalizationTests
    {
        private readonly string _dataDir;
        private readonly IFileIO _fileIO;
        private readonly IJsonSerializer _serializer;

        public Plan33SkillCatalogExternalizationTests()
        {
            _dataDir = ResolveDataDir();
            _fileIO = new FileSystemIO();
            _serializer = new SystemTextJsonSerializer();
        }

        private static string ResolveDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new InvalidOperationException("StreamingAssets/Data directory not found");
        }

        private sealed class TestSkillActor : SkillActor
        {
            private readonly Dictionary<string, float> _bonuses = new(StringComparer.Ordinal);
            public string Id { get; }
            public bool IsAlive => true;
            public float Morale => 100f;
            public float Health => 100f;
            public string ExpertDisciplineId { get; set; } = string.Empty;

            public TestSkillActor(string id, string expertDiscipline = "")
            {
                Id = id;
                ExpertDisciplineId = expertDiscipline;
            }

            public void SetSkillBonus(string disciplineId, float bonus)
            {
                if (string.IsNullOrEmpty(disciplineId)) return;
                _bonuses[disciplineId] = bonus;
            }
        }

        [Fact]
        public void Catalog_LoadsExactExpectedCount_FromAuthoritativeJson()
        {
            var defs = SkillCatalogLoader.Load(_dataDir, _fileIO, _serializer);
            Assert.NotEmpty(defs);
            Assert.Equal(148, defs.Count);
        }

        [Fact]
        public void AllSkills_HaveValidIdsPrefixAndNonNegativeThresholds()
        {
            var defs = SkillCatalogLoader.Load(_dataDir, _fileIO, _serializer);
            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            var validDisciplines = new HashSet<string>(StringComparer.Ordinal)
            {
                "medical", "crafting", "science", "combat", "scavenging", "survival", ""
            };

            foreach (var def in defs)
            {
                Assert.NotNull(def);
                Assert.False(string.IsNullOrWhiteSpace(def.id));
                Assert.StartsWith("skill_", def.id);
                Assert.True(seenIds.Add(def.id), $"Duplicate skill id: {def.id}");
                Assert.False(string.IsNullOrWhiteSpace(def.displayName));
                Assert.Contains(def.disciplineId, validDisciplines);
                Assert.True(def.xpThreshold >= 0f);
                Assert.True(def.skillBonus >= 0f && def.skillBonus <= 1f);
            }
        }

        [Fact]
        public void BaselineSkills_MatchExpectedValues()
        {
            var defs = SkillCatalogLoader.Load(_dataDir, _fileIO, _serializer);
            var byId = defs.ToDictionary(d => d.id, StringComparer.Ordinal);

            // Action-driven medical skills
            Assert.True(byId.TryGetValue("skill_field_dressing", out var fieldDressing));
            Assert.Equal("Field Dressing", fieldDressing.displayName);
            Assert.Equal("medical", fieldDressing.disciplineId);
            Assert.Equal(50f, fieldDressing.xpThreshold);
            Assert.Equal(0.10f, fieldDressing.skillBonus, 3);
            Assert.False(fieldDressing.isExpertSkill);

            Assert.True(byId.TryGetValue("skill_steady_hands", out var steadyHands));
            Assert.Equal("Steady Hands", steadyHands.displayName);
            Assert.Equal("medical", steadyHands.disciplineId);
            Assert.Equal(120f, steadyHands.xpThreshold);
            Assert.Equal(0.20f, steadyHands.skillBonus, 3);
            Assert.True(steadyHands.isExpertSkill);

            // Action-driven crafting skills
            Assert.True(byId.TryGetValue("skill_rough_repairs", out var roughRepairs));
            Assert.Equal("crafting", roughRepairs.disciplineId);
            Assert.Equal(50f, roughRepairs.xpThreshold);
            Assert.Equal(0.10f, roughRepairs.skillBonus, 3);

            Assert.True(byId.TryGetValue("skill_workshop_sense", out var workshopSense));
            Assert.Equal("crafting", workshopSense.disciplineId);
            Assert.Equal(120f, workshopSense.xpThreshold);
            Assert.True(workshopSense.isExpertSkill);
        }

        [Fact]
        public void NewGroundedSkills_PresentAndConfigured()
        {
            var defs = SkillCatalogLoader.Load(_dataDir, _fileIO, _serializer);
            var byId = defs.ToDictionary(d => d.id, StringComparer.Ordinal);

            Assert.True(byId.TryGetValue("skill_field_surgery", out var surgery));
            Assert.Equal("Field Surgery", surgery.displayName);
            Assert.Equal("medical", surgery.disciplineId);
            Assert.Equal(0.15f, surgery.skillBonus, 3);
            Assert.Equal(999999f, surgery.xpThreshold);

            Assert.True(byId.TryGetValue("skill_water_filtration", out var water));
            Assert.Equal("Water Filtration", water.displayName);
            Assert.Equal("survival", water.disciplineId);
            Assert.Equal(0.10f, water.skillBonus, 3);

            Assert.True(byId.TryGetValue("skill_radio_repair", out var radio));
            Assert.Equal("Radio Repair", radio.displayName);
            Assert.Equal("science", radio.disciplineId);
            Assert.Equal(0.10f, radio.skillBonus, 3);
        }

        [Fact]
        public void Loader_HandlesMissingOrCorruptedPathGracefully()
        {
            var emptyList1 = SkillCatalogLoader.Load("nonexistent/path/nowhere", _fileIO, _serializer);
            Assert.Empty(emptyList1);

            var sys = new SkillProgressionSystem();
            int registered = SkillCatalogLoader.LoadAndRegister(sys, "nonexistent/path/nowhere", _fileIO, _serializer);
            Assert.Equal(0, registered);
        }

        [Fact]
        public void Progression_ActionDrivenXp_UnlocksTierAndAppliesBonus()
        {
            var sys = new SkillProgressionSystem();
            SkillCatalogLoader.LoadAndRegister(sys, _dataDir, _fileIO, _serializer);

            var actor = new TestSkillActor("surv_test_medic", "medical");

            for (int i = 0; i < 11; i++)
            {
                sys.RecordAction(actor, "medical", 5f, 1);
            }

            Assert.True(sys.GetXp("surv_test_medic", "medical") >= 50f);
            Assert.True(sys.HasActiveSkill("surv_test_medic", "skill_field_dressing"));
            Assert.Equal(0.10f, sys.GetCachedBonus("surv_test_medic", "medical"), 3);

            // Advance past 120 XP to unlock expert tier
            for (int i = 0; i < 15; i++)
            {
                sys.RecordAction(actor, "medical", 5f, 2);
            }

            Assert.True(sys.GetXp("surv_test_medic", "medical") >= 120f);
            Assert.True(sys.HasActiveSkill("surv_test_medic", "skill_steady_hands"));
            Assert.Equal(0.30f, sys.GetCachedBonus("surv_test_medic", "medical"), 3);
        }

        [Fact]
        public void MilestoneGranting_WorksForNewGroundedSkills()
        {
            var sys = new SkillProgressionSystem();
            SkillCatalogLoader.LoadAndRegister(sys, _dataDir, _fileIO, _serializer);

            var actor = new TestSkillActor("surv_specialist");

            bool granted1 = sys.TryGrantSkill(actor, "skill_field_surgery", 5);
            Assert.True(granted1);
            Assert.True(sys.HasActiveSkill("surv_specialist", "skill_field_surgery"));

            bool granted2 = sys.TryGrantSkill(actor, "skill_water_filtration", 5);
            Assert.True(granted2);
            Assert.True(sys.HasActiveSkill("surv_specialist", "skill_water_filtration"));

            bool granted3 = sys.TryGrantSkill(actor, "skill_radio_repair", 5);
            Assert.True(granted3);
            Assert.True(sys.HasActiveSkill("surv_specialist", "skill_radio_repair"));
        }

        [Fact]
        public void SaveAndRestore_RoundTripsWithLoadedCatalog()
        {
            var sys = new SkillProgressionSystem();
            SkillCatalogLoader.LoadAndRegister(sys, _dataDir, _fileIO, _serializer);

            var actor = new TestSkillActor("surv_veteran", "science");
            for (int i = 0; i < 12; i++)
            {
                sys.RecordAction(actor, "science", 5f, 3);
            }
            sys.TryGrantSkill(actor, "skill_radio_repair", 3);

            var state = sys.CaptureState();

            var restoredSys = new SkillProgressionSystem();
            SkillCatalogLoader.LoadAndRegister(restoredSys, _dataDir, _fileIO, _serializer);
            restoredSys.RestoreState(state, new List<SkillActor> { actor });

            Assert.True(restoredSys.HasActiveSkill("surv_veteran", "skill_signal_ear"));
            Assert.True(restoredSys.HasActiveSkill("surv_veteran", "skill_radio_repair"));
            Assert.Equal(sys.GetCachedBonus("surv_veteran", "science"), restoredSys.GetCachedBonus("surv_veteran", "science"), 3);
        }
    }
}
