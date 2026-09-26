// SPDX-License-Identifier: MIT
// Plan 49 (C1[16]) depth passes — the four orphaned catalogs are bound
// through their existing Core loaders into constructed owners.
using System;
using System.IO;
using Xunit;

namespace Ashfall.Core.Tests.Content
{
    public sealed class Plan49DepthPassWiringTests
    {
        private static string RepoRoot()
        {
            var directory = new DirectoryInfo(Path.GetFullPath(AppContext.BaseDirectory));
            while (directory != null)
            {
                if (File.Exists(Path.Combine(directory.FullName, "src", "Main.Plan49DepthPass.cs")))
                    return directory.FullName;
                directory = directory.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate the ASHFALL repository root.");
        }

        [Fact]
        public void CoreSeams_LoadAuthoredCatalogs_AndServeQueries()
        {
            string data = Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");
            var audio = new AudioConditionSystem();
            audio.LoadAudioLogCatalog(File.ReadAllText(Path.Combine(data, "audio_logs_expansion_05.json")));
            Assert.True(audio.AudioLogCount > 0);
            Assert.Contains("Meridian Compact", audio.GetAudioLogBody("audio_log_radio_message_day_35"));

            var memorials = new Ashfall.Core.Memorial.MemorialSystem(new Ashfall.Core.Memorial.MemorialState());
            memorials.LoadMemorialTexts(File.ReadAllText(Path.Combine(data, "memorials_expansion_05.json")));
            Assert.True(memorials.MemorialTextCount > 0);
            Assert.Contains("water flowing", memorials.GetMemorialText("memorial_marcus_olejnik"));

            var heirlooms = new Ashfall.Core.Phantoms.HeirloomCatalog();
            heirlooms.Load(File.ReadAllText(Path.Combine(data, "phantom_heirlooms.json")), new Ashfall.Core.SystemTextJsonSerializer());
            Assert.True(heirlooms.AllHeirlooms.Count > 0);
            Assert.NotNull(heirlooms.GetByBaseItemId("dosimeter"));
        }

        [Fact]
        public void HostWiring_BindsAllFourCatalogs_AtSetup_NoSaveSection()
        {
            string host = File.ReadAllText(Path.Combine(RepoRoot(), "src", "Host", "Plan49DepthPassHostSession.cs"));
            string main = File.ReadAllText(Path.Combine(RepoRoot(), "src", "Main.Plan49DepthPass.cs"));
            string orchestrator = File.ReadAllText(Path.Combine(RepoRoot(), "src", "Main.SaveOrchestrator.cs"));
            string registry = File.ReadAllText(Path.Combine(RepoRoot(), "Assets", "Ashfall.Core", "Save", "SaveSectionRegistry.cs"));

            Assert.Contains("Heirlooms.Load(", host);
            Assert.Contains("TradeScreenScenarioLoader.LoadFromJson(", host);
            Assert.Contains("LoadAudioLogCatalog(", host);
            Assert.Contains("LoadMemorialTexts(", host);
            Assert.Contains("SetupPlan49DepthPass();", orchestrator);
            Assert.Contains("Plan49DepthPassHostSession.Create(_dataDir)", main);
            Assert.DoesNotContain("\"plan49_depth\"", registry); // authored data, no new save section
        }
    }
}
