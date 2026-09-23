// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 38 / C1[11] — commitment authority host-integration gate.
//
// Pins the production wiring contract added by the host integration:
//   * the `commitment` save section is registered with its projection file,
//   * the authored commitments.json loads through the strict loader,
//   * the day owner is registered on the campaign coordinator,
//   * Setup/Save are enrolled on the expanded-shelter lifecycle,
//   * every authored consequence_class has an explicit host route,
//   * `obligation_met` is classified by the day-event vocabulary.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Commitments;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public sealed class Plan38CommitmentHostIntegrationTests
    {
        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (File.Exists(Path.Combine(dir, "Ashfall.csproj")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        private static string ReadRepoFile(params string[] parts)
            => File.ReadAllText(Path.Combine(new[] { RepoRoot() }.Concat(parts).ToArray()));

        private static CommitmentLoadResult LoadAuthoredCatalog()
        {
            string dataDir = Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");
            return CommitmentCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
        }

        [Fact]
        public void CommitmentSection_IsRegisteredWithProjectionFile()
        {
            Assert.True(SaveSectionRegistry.TryGetSection("commitment", out var section));
            Assert.NotNull(section);
            Assert.Equal("SaveCommitments", section!.SaveMethod);
            Assert.Equal("SetupCommitments", section.SetupMethod);
            Assert.Equal("commitment_save.json", SaveSectionRegistry.FileNameFor("commitment"));
        }

        [Fact]
        public void AuthoredCommitments_LoadThroughStrictLoader()
        {
            var result = LoadAuthoredCatalog();

            Assert.False(result.HasErrors, string.Join("; ", result.Errors));
            Assert.Equal(3, result.Commitments.Count);
            Assert.Contains(result.Commitments, c => c.id == "commitment_warlord_grain_tribute");
            Assert.Contains(result.Commitments, c => c.id == "commitment_water_filter_treaty");
            Assert.Contains(result.Commitments, c => c.id == "commitment_shelter_census_filing");
        }

        [Fact]
        public void DayOwner_IsRegisteredOnCampaignCoordinator()
        {
            string owners = ReadRepoFile("src", "Main.CampaignOwners.cs");
            Assert.Contains("_campaignDay.Register(\"shelter_commitments\", new CommitmentDayOwner(this), phase: 4);", owners);
            Assert.Contains("private sealed class CommitmentDayOwner : IDayAdvanceOwner, IPreDaySnapshotRestore", owners);
        }

        [Fact]
        public void SetupAndSave_AreEnrolledOnExpandedShelterLifecycle()
        {
            string expanded = ReadRepoFile("src", "Main.ExpandedShelterSystems.cs");
            Assert.Contains("SetupCommitments();", expanded);
            Assert.Contains("SaveCommitments();", expanded);
            Assert.Contains("ResetCommitments();", expanded);
        }

        [Fact]
        public void EveryAuthoredConsequenceClass_HasAnExplicitHostRoute()
        {
            var result = LoadAuthoredCatalog();
            string host = ReadRepoFile("src", "Main.Commitments.cs");

            foreach (string consequenceClass in result.Commitments.Select(c => c.consequence_class).Distinct())
            {
                Assert.Contains($"case \"{consequenceClass}\":", host);
            }
        }

        [Fact]
        public void MetKind_IsClassifiedByDayEventVocabulary()
        {
            Assert.False(DayEventVocabulary.IsInternalHeartbeat("obligation_met"));
            Assert.Equal(SemanticKind.Narrative, DayEventVocabulary.GetSemanticKind("obligation_met"));
        }

        [Fact]
        public void HostSession_ExposesTheCanonicalCommandAndStateSeams()
        {
            string session = ReadRepoFile("src", "Host", "CommitmentHostSession.cs");
            Assert.Contains("public bool RecordProgress(", session);
            Assert.Contains("public bool Settle(", session);
            Assert.Contains("public void TickDay(int day)", session);
            Assert.Contains("public void DrainDayEvents(List<DayStateChangeEvent> events)", session);
            Assert.Contains("CommitmentSaveStore.TrySave", session);
        }
    }
}
