// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Communication;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Communication
{
    /// <summary>
    /// Plan 211 host/save/route contract checks that do not require launching
    /// Godot. The headless probe covers the live host behavior; these checks
    /// keep the production seams from silently disappearing during a rebuild.
    /// </summary>
    public sealed class Plan211InternalCommunicationHostWiringTests
    {
        private static string RepoFile(string relativePath)
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", relativePath);
            if (File.Exists(candidate)) return Path.GetFullPath(candidate);

            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory != null)
            {
                string check = Path.Combine(directory.FullName, relativePath);
                if (File.Exists(check)) return check;
                directory = directory.Parent;
            }

            throw new FileNotFoundException($"Repository file not found: {relativePath}");
        }

        [Fact]
        public void Registry_UsesOneDistinctInternalCommunicationSection()
        {
            var row = Assert.Single(SaveSectionRegistry.All, section =>
                section.SectionKey == "internal_communication");
            Assert.Equal("SaveInternalCommunication", row.SaveMethod);
            Assert.Equal("SetupInternalCommunication", row.SetupMethod);
            Assert.Equal("internal_communication_save.json", SaveSectionRegistry.FileNameFor(row.SectionKey));
            Assert.Equal("communications_save.json", SaveSectionRegistry.FileNameFor("communications"));
            Assert.NotEqual(
                SaveSectionRegistry.FileNameFor("internal_communication"),
                SaveSectionRegistry.FileNameFor("communications"));
        }

        [Fact]
        public void CoreRestore_AcceptsLegacyBaselineAndRejectsFutureSchema()
        {
            var legacy = new InternalCommunicationState
            {
                SchemaVersion = 0,
                NextSequence = 0,
                Messages = new List<CommunicationMessage>(),
                Boards = new List<BulletinBoard>(),
                IntercomBroadcasts = new List<IntercomAnnouncement>()
            };
            var system = new InternalCommunicationSystem();
            system.RestoreState(legacy);

            Assert.Equal(InternalCommunicationSystem.CurrentSchemaVersion, system.CaptureState().SchemaVersion);
            Assert.Equal(1, system.CaptureState().NextSequence);
            Assert.Single(system.Boards);

            var future = new InternalCommunicationState { SchemaVersion = 99 };
            Assert.Throws<InvalidOperationException>(() => system.RestoreState(future));
        }

        [Fact]
        public void HostAndMainSources_ContainTheLiveSaveDayAndPanelSeams()
        {
            string host = File.ReadAllText(RepoFile("src/Host/InternalCommunicationHostSession.cs"));
            string main = File.ReadAllText(RepoFile("src/Main.InternalCommunication.cs"));
            string plans = File.ReadAllText(RepoFile("src/Main.Plans46_49.cs"));
            string campaign = File.ReadAllText(RepoFile("src/Main.CampaignServices.cs"));
            string lifecycle = File.ReadAllText(RepoFile("src/Main.ExpandedShelterSystems.cs"));

            Assert.Contains("TryLoadCatalog", host, StringComparison.Ordinal);
            Assert.Contains("PostWaterAdvisory", host, StringComparison.Ordinal);
            Assert.Contains("CreateBulletinBoard", host, StringComparison.Ordinal);
            Assert.Contains("author_not_authorized", host, StringComparison.Ordinal);
            Assert.Contains("MarkRead", host, StringComparison.Ordinal);
            Assert.Contains("Acknowledge", host, StringComparison.Ordinal);
            Assert.Contains("SetupInternalCommunication", main, StringComparison.Ordinal);
            Assert.Contains("SetupInternalCommunication", campaign, StringComparison.Ordinal);
            Assert.Contains("SaveInternalCommunication", main, StringComparison.Ordinal);
            Assert.Contains("OnMessagePosted", main, StringComparison.Ordinal);
            Assert.Contains("TickInternalCommunication", plans, StringComparison.Ordinal);
            Assert.Contains("SaveInternalCommunication();", plans, StringComparison.Ordinal);
            Assert.Contains("ResetInternalCommunication();", lifecycle, StringComparison.Ordinal);
        }

        [Fact]
        public void ShelterSocialPanel_UsesHostProjectionWithoutPrivateMailLeak()
        {
            string panel = File.ReadAllText(RepoFile("src/UI/ShelterSocialPanel.cs"));
            Assert.Contains("BindCommunications", panel, StringComparison.Ordinal);
            Assert.Contains("PublicNotices", panel, StringComparison.Ordinal);
            Assert.Contains("MarkRead", panel, StringComparison.Ordinal);
            Assert.Contains("Acknowledge", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("GetInbox", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void CliAndRegistry_ExposeTheInternalCommunicationProbe()
        {
            string coreRegistry = File.ReadAllText(RepoFile("Assets/Ashfall.Core/HostCliRegistry.cs"));
            string hostCli = File.ReadAllText(RepoFile("src/Host/HostCli.cs"));
            string application = File.ReadAllText(RepoFile("src/Main.Application.cs"));
            string selftest = File.ReadAllText(RepoFile("src/Host/InternalCommunicationSelfTest.cs"));

            Assert.Contains("InternalCommunicationSelfTest", coreRegistry, StringComparison.Ordinal);
            Assert.Contains("--internal-communication-selftest", coreRegistry, StringComparison.Ordinal);
            Assert.Contains("--internal-communication-selftest", hostCli, StringComparison.Ordinal);
            Assert.Contains("HostCliAction.InternalCommunicationSelfTest", application, StringComparison.Ordinal);
            Assert.Contains("InternalCommunicationSelfTest", selftest, StringComparison.Ordinal);
            Assert.Contains("return failures == 0 ? 0 : 1;", selftest, StringComparison.Ordinal);
        }
    }
}
