// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    public sealed class HoldfastNpcCatalogTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            var dir = new DirectoryInfo(start);
            while (dir != null)
            {
                string probe = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(probe))
                    return probe;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate Assets/StreamingAssets/Data from test run");
        }

        [Fact]
        public void HoldfastNpcCatalog_LoadsAuthoritativeJson_AllTenNpcsPresent()
        {
            string dataDir = FindDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var catalog = HoldfastNpcCatalogLoader.Load(dataDir, files, json);

            Assert.True(catalog.IsValid);
            Assert.Equal(10, catalog.Count);

            var ormund = catalog.GetById("npc_cael_ormund");
            Assert.NotNull(ormund);
            Assert.Equal("Registrar-General Cael Ormund", ormund!.DisplayName);
            Assert.Equal("faction_the_office", ormund.FactionId);
            Assert.Equal("Registrar", ormund.Role);
            Assert.Contains("The discrepancy is noted.", ormund.DialogueFragments);
            Assert.Contains("Complete census forms accurately", ormund.TrustBuildingRequirements);
            Assert.False(ormund.IsCompanion);

            var edor = catalog.GetById("npc_edor_vale");
            Assert.NotNull(edor);
            Assert.True(edor!.IsCompanion);
            Assert.Contains("companion_edor", edor.CompanionFlags);

            var leva = catalog.GetById("npc_leva_quist");
            Assert.NotNull(leva);
            Assert.True(leva!.IsCompanion);
            Assert.Equal("hydro_barons", leva.FactionId);

            var yara = catalog.GetById("npc_yara_holm");
            Assert.NotNull(yara);
            Assert.True(yara!.IsCompanion);
            Assert.Equal("faction_the_cutters", yara.FactionId);

            var mire = catalog.GetById("npc_halden_mire");
            Assert.NotNull(mire);
            Assert.True(mire!.IsCompanion);

            var sela = catalog.GetById("npc_sela_renn");
            Assert.NotNull(sela);
            Assert.True(sela!.IsCompanion);
            Assert.Equal(0.8f, sela.BaseTrust);

            Assert.NotNull(catalog.GetById("npc_ivy_corrigan"));
            Assert.NotNull(catalog.GetById("npc_margit_sole"));
            Assert.NotNull(catalog.GetById("npc_colonel_voss"));
            Assert.NotNull(catalog.GetById("npc_wren"));
        }

        [Fact]
        public void HoldfastCatalog_Integration_NpcsLoadedWithHoldfastCatalog()
        {
            string dataDir = FindDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var loader = new HoldfastCatalogLoader(files, json);
            var holdfastCatalog = loader.Load(dataDir);

            Assert.NotNull(holdfastCatalog.Npcs);
            Assert.Equal(10, holdfastCatalog.Npcs.Count);
            Assert.NotNull(holdfastCatalog.GetNpc("npc_cael_ormund"));
        }

        [Fact]
        public void HoldfastNpcCatalog_ZeroHardcodedNpcFallbacksInCore_AuthorityGate()
        {
            string start = Directory.GetCurrentDirectory();
            var dir = new DirectoryInfo(start);
            string? sourceFile = null;
            while (dir != null)
            {
                string probe = Path.Combine(dir.FullName, "Assets", "Ashfall.Core", "Narrative", "HoldfastNpcCatalog.cs");
                if (File.Exists(probe))
                {
                    sourceFile = probe;
                    break;
                }
                dir = dir.Parent;
            }
            Assert.NotNull(sourceFile);

            string content = File.ReadAllText(sourceFile!);
            Assert.DoesNotContain("CreateDefaultNpcs", content);
            Assert.DoesNotContain("Registrar-General Cael Ormund", content);
            Assert.DoesNotContain("Colonel Rurik Voss", content);
            Assert.DoesNotContain("Clerk Edor Vale", content);
        }
    }
}
