// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core.IO;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Progression
{
    public sealed class SkillCatalogLoaderTests
    {
        private static string ResolveDataDir()
        {
            string baseDir = AppContext.BaseDirectory;
            string probe = Path.Combine(baseDir, "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return probe;

            string dir = baseDir;
            for (int i = 0; i < 6; i++)
            {
                probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(probe)) return probe;
                var parent = Directory.GetParent(dir);
                if (parent == null) break;
                dir = parent.FullName;
            }
            return probe;
        }

        [Fact]
        public void Load_Loads110SkillsFromCatalog()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var skills = SkillCatalogLoader.Load(dataDir, fileIO, json);
            Assert.NotEmpty(skills);
            Assert.True(skills.Count >= 110, $"Expected >= 110 skills, found {skills.Count}");
        }

        [Fact]
        public void Load_AllSkillsHaveValidIdAndDisplayName()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var skills = SkillCatalogLoader.Load(dataDir, fileIO, json);
            foreach (var s in skills)
            {
                Assert.False(string.IsNullOrWhiteSpace(s.id));
                Assert.StartsWith("skill_", s.id);
                Assert.False(string.IsNullOrWhiteSpace(s.displayName));
                Assert.False(string.IsNullOrWhiteSpace(s.disciplineId));
            }
        }

        [Fact]
        public void LoadAndRegister_PopulatesSkillProgressionSystem()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var system = new SkillProgressionSystem();

            int count = SkillCatalogLoader.LoadAndRegister(system, dataDir, fileIO, json);
            Assert.True(count >= 110);
            Assert.Equal(count, system.CatalogCount);
            Assert.NotNull(system.GetSkill("skill_field_dressing"));
            Assert.NotNull(system.GetSkill("skill_steady_hands"));
            Assert.NotNull(system.GetSkill("skill_miracle_worker"));
        }
    }
}
