// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.Progression
{
    public sealed class AutopsyProcedureCatalogExpansionTests
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
        public void Load_Loads9ProceduresFromCatalog()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var procedures = AutopsyProcedureCatalogLoader.Load(dataDir, fileIO, json);
            Assert.Equal(9, procedures.Count);
        }

        [Fact]
        public void Load_AllProceduresHaveValidToolsRisksAndFindings()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var procedures = AutopsyProcedureCatalogLoader.Load(dataDir, fileIO, json);
            foreach (var p in procedures)
            {
                Assert.False(string.IsNullOrWhiteSpace(p.procedure_id));
                Assert.StartsWith("procedure_", p.procedure_id);
                Assert.False(string.IsNullOrWhiteSpace(p.display_name));
                Assert.NotEmpty(p.requiredTools);
                Assert.NotEmpty(p.requiredConsumables);
                Assert.NotEmpty(p.possibleFindings);
                Assert.True(p.procedureHours > 0, $"{p.procedure_id} procedureHours should be > 0");
                Assert.True(p.airborneRisk >= 0f, $"{p.procedure_id} airborneRisk should be >= 0");
                Assert.True(p.pathogenRisk >= 0f, $"{p.procedure_id} pathogenRisk should be >= 0");
            }
        }
    }
}
