// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.Progression
{
    public sealed class LibraryStudyCatalogExpansionTests
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
        public void Load_Loads12ManualsFromCatalog()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var manuals = LibraryManualCatalogLoader.Load(dataDir, fileIO, json);
            Assert.True(manuals.Count >= 24, $"Expected >= 24 manuals, got {manuals.Count}");
        }

        [Fact]
        public void Load_AllManualsHaveValidFieldsAndRequirements()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var manuals = LibraryManualCatalogLoader.Load(dataDir, fileIO, json);
            foreach (var m in manuals)
            {
                Assert.False(string.IsNullOrWhiteSpace(m.manual_id));
                Assert.StartsWith("manual_", m.manual_id);
                Assert.False(string.IsNullOrWhiteSpace(m.display_name));
                Assert.False(string.IsNullOrWhiteSpace(m.category));
                Assert.True(m.studyHoursRequired > 0, $"{m.manual_id} studyHoursRequired should be > 0");
                Assert.True(m.fatiguePerHour > 0f, $"{m.manual_id} fatiguePerHour should be > 0");
            }
        }
    }
}
