// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 39 / C2[16] — session durability host-integration gate.
//
// Pins the production wiring contract:
//   * the `session_durability` section is registered with its projection file,
//   * save/load/day-advance hooks call the durability audit,
//   * Setup/Save/Reset are enrolled on the expanded-shelter lifecycle,
//   * the canonical save service remains the only recovery authority.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Save
{
    public sealed class Plan39SessionDurabilityHostIntegrationTests
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

        [Fact]
        public void SessionDurabilitySection_IsRegisteredWithProjectionFile()
        {
            Assert.True(SaveSectionRegistry.TryGetSection("session_durability", out var section));
            Assert.NotNull(section);
            Assert.Equal("SaveSessionDurability", section!.SaveMethod);
            Assert.Equal("SetupSessionDurability", section.SetupMethod);
            Assert.Equal("session_durability_save.json", SaveSectionRegistry.FileNameFor("session_durability"));
        }

        [Fact]
        public void SavePipeline_MirrorsCommittedSaveAndAuditsLoadFailure()
        {
            string orchestrator = ReadRepoFile("src", "Main.SaveOrchestrator.cs");
            Assert.Contains("RecordSessionSaveResult(", orchestrator);
            Assert.Contains("ComputeSessionChecksum(_sectionPayloads)", orchestrator);
            Assert.Contains("RecordSessionLoadFailure(slotId.Value, message);", orchestrator);
        }

        [Fact]
        public void DayAdvance_RecordsOneSoakSamplePerSuccessfulAdvance()
        {
            string holdfast = ReadRepoFile("src", "Main.Holdfast.cs");
            Assert.Contains("Stopwatch.StartNew()", holdfast);
            Assert.Contains("RecordSessionDaySample(day, (float)soakWatch.Elapsed.TotalMilliseconds", holdfast);
        }

        [Fact]
        public void SetupSaveReset_AreEnrolledOnExpandedShelterLifecycle()
        {
            string expanded = ReadRepoFile("src", "Main.ExpandedShelterSystems.cs");
            Assert.Contains("SetupSessionDurability();", expanded);
            Assert.Contains("SaveSessionDurability();", expanded);
            Assert.Contains("ResetSessionDurability();", expanded);
        }

        [Fact]
        public void DurabilitySession_IsAnAuditLayer_NotARecoveryAuthority()
        {
            string session = ReadRepoFile("src", "Host", "SessionDurabilityHostSession.cs");
            // The audit layer records recovery facts; it never calls the
            // canonical service's recovery path itself.
            Assert.Contains("RecordSuccessfulSave", session);
            Assert.Contains("RecordLoadFailure", session);
            Assert.Contains("RecordDaySample", session);
            // The audit layer never exposes or invokes the manager's recovery path;
            // the canonical save service remains the only recovery authority.
            Assert.DoesNotContain("TryRecoverBackup(", session);
        }
    }
}
