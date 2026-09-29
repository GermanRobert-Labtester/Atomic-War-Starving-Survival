// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Save
{
    public class ActiveSaveSlotPersistenceTests
    {
        private sealed class MemoryFileIO : IFileIO
        {
            public readonly Dictionary<string, string> Files = new(StringComparer.OrdinalIgnoreCase);
            public readonly HashSet<string> Directories = new(StringComparer.OrdinalIgnoreCase);
            public string? FailWritePath;

            public bool DirectoryExists(string path) => Directories.Contains(path) || Files.Keys.Any(k => k.StartsWith(path, StringComparison.OrdinalIgnoreCase));
            public void CreateDirectory(string path) => Directories.Add(path);
            public bool FileExists(string path) => Files.ContainsKey(path);
            public string ReadAllText(string path) => Files.TryGetValue(path, out var text) ? text : throw new FileNotFoundException(path);
            public void WriteAllText(string path, string contents)
            {
                if (path == FailWritePath) throw new IOException("Injected interrupted write");
                Files[path] = contents;
            }
            public void DeleteFile(string path) => Files.Remove(path);
            public string Combine(params string[] paths) => Path.Combine(paths);
            public string[] GetFiles(string path, string searchPattern = "*") => Files.Keys.Where(k => k.StartsWith(path, StringComparison.OrdinalIgnoreCase)).ToArray();
            public string[] GetDirectories(string path) => Directories.Where(k => k.StartsWith(path, StringComparison.OrdinalIgnoreCase) && k != path).ToArray();
        }

        private sealed class TestLog : ILog
        {
            public readonly List<string> Messages = new();
            public void Info(string message) => Messages.Add("INFO: " + message);
            public void Warn(string message) => Messages.Add("WARN: " + message);
            public void Error(string message) => Messages.Add("ERROR: " + message);
        }

        private static AggregateSaveEnvelope RecoveryEnvelope(SaveProfileId profile, SaveSlotId slot,
            int day, int manifestVersion = SaveManifest.CurrentManifestVersion)
        {
            return CampaignEnvelopeBuilder.Build(new Dictionary<string, string>
            {
                ["inventory"] = "{\"Credits\":" + day + "}"
            }, new SaveManifest
            {
                manifestVersion = manifestVersion,
                profileId = profile, slotId = slot, currentDay = day,
                lastSaveTick = day, generationId = "generation_" + day,
                difficultyPresetId = "standard", lastPanelId = manifestVersion >= 3 ? "inventory" : string.Empty
            });
        }

        [Fact]
        [Trait("Category", "fast")]
        public void RollingBackups_SkipCorruptNewest_AndExplicitlyRecoverOlderCampaign()
        {
            var files = new MemoryFileIO();
            var json = new SystemTextJsonSerializer();
            var service = new SaveSlotService(files, json, new TestLog(), "recovery_memory");
            var profile = new SaveProfileId("default");
            var slot = new SaveSlotId("slot_1");
            for (int day = 1; day <= 5; day++)
                Assert.True(service.WriteAggregateAtomically(profile, slot, RecoveryEnvelope(profile, slot, day)));
            for (int generation = 1; generation <= 3; generation++)
                Assert.Equal(5 - generation, json.Deserialize<AggregateSaveEnvelope>(
                    files.ReadAllText(service.GetBackupPath(profile, slot, generation)))!.manifest.currentDay);

            // A valid primary cannot be replaced by an older generation.
            Assert.False(service.FindRecoverableBackup(profile, slot).IsSuccess);
            string primary = service.GetAggregatePath(profile, slot);
            files.WriteAllText(primary, "{ interrupted");
            string newest = service.GetBackupPath(profile, slot, 1);
            files.WriteAllText(newest, files.ReadAllText(newest).Replace("inventory", "tampered"));
            string previousBackup = files.ReadAllText(service.GetBackupPath(profile, slot, 2));

            var preview = service.FindRecoverableBackup(profile, slot);
            Assert.True(preview.IsSuccess);
            Assert.Equal(3, preview.Envelope!.manifest.currentDay);
            Assert.Equal("{ interrupted", files.ReadAllText(primary)); // preview is read-only
            var recovered = service.RecoverBackup(profile, slot);
            Assert.True(recovered.IsSuccess);
            Assert.Equal(3, recovered.Envelope!.manifest.currentDay);
            Assert.Equal("inventory", recovered.Envelope.manifest.lastPanelId);
            Assert.Equal("{\"Credits\":3}", recovered.Envelope.sections[0].payloadJson);
            Assert.Equal(previousBackup, files.ReadAllText(service.GetBackupPath(profile, slot, 2)));
        }

        [Fact]
        [Trait("Category", "fast")]
        public void BackupRotationFailure_LeavesPrimaryLoadable()
        {
            var files = new MemoryFileIO();
            var service = new SaveSlotService(files, new SystemTextJsonSerializer(), new TestLog(), "failure_memory");
            var profile = new SaveProfileId("default");
            var slot = new SaveSlotId("slot_1");
            Assert.True(service.WriteAggregateAtomically(profile, slot, RecoveryEnvelope(profile, slot, 1)));
            files.FailWritePath = service.GetBackupPath(profile, slot, 1) + ".tmp";
            Assert.False(service.WriteAggregateAtomically(profile, slot, RecoveryEnvelope(profile, slot, 2)));
            Assert.Equal(1, service.TryLoadAggregate(profile, slot).Envelope!.manifest.currentDay);
        }

        [Fact]
        [Trait("Category", "fast")]
        public void Recovery_RejectsForeignIdentity_AndTerminalCampaign()
        {
            var files = new MemoryFileIO();
            var json = new SystemTextJsonSerializer();
            var service = new SaveSlotService(files, json, new TestLog(), "policy_memory");
            var profile = new SaveProfileId("default");
            var slot = new SaveSlotId("slot_1");
            string backup = service.GetBackupPath(profile, slot, 1);
            files.WriteAllText(backup, json.Serialize(RecoveryEnvelope(profile, new SaveSlotId("foreign"), 1)));
            Assert.False(service.RecoverBackup(profile, slot).IsSuccess);
            files.WriteAllText(backup, json.Serialize(RecoveryEnvelope(profile, slot, 1)));
            service.SaveManifest(profile, slot, new SaveManifest
            {
                profileId = profile, slotId = slot, ironManTerminalState = IronManTerminalState.TerminalLoss
            });
            Assert.Equal(SaveLoadStatus.IronManBlocked, service.RecoverBackup(profile, slot).Status);
            Assert.False(files.FileExists(service.GetAggregatePath(profile, slot)));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [Trait("Category", "fast")]
        public void PanelMetadata_RoundTrips_WithVersionedChecksumCompatibility(int version)
        {
            var files = new MemoryFileIO();
            var json = new SystemTextJsonSerializer();
            var service = new SaveSlotService(files, json, new TestLog(), "panel_memory_" + version);
            var profile = new SaveProfileId("default");
            var slot = new SaveSlotId("slot_1");
            var envelope = RecoveryEnvelope(profile, slot, 12, version);
            string originalChecksum = envelope.aggregateChecksum;
            // v2 does not checksum the new field; v3 does. Difficulty remains
            // checksummed in both versions after the manifest version bump.
            envelope.manifest.lastPanelId = "journal";
            Assert.Equal(version == 2, originalChecksum == SaveSlotService.ComputeAggregateChecksum(envelope));
            envelope.manifest.lastPanelId = version >= 3 ? "inventory" : string.Empty;
            Assert.True(service.WriteAggregateAtomically(profile, slot, envelope));
            var loaded = service.TryLoadAggregate(profile, slot);
            Assert.True(loaded.IsSuccess);
            Assert.Equal(12, loaded.Envelope!.manifest.currentDay);
            Assert.Equal(version >= 3 ? "inventory" : string.Empty, loaded.Envelope.manifest.lastPanelId);
            loaded.Envelope.manifest.difficultyPresetId = "changed";
            Assert.False(service.ValidateAggregate(loaded.Envelope).IsValid);
        }

        [Fact]
        public void ActiveSlot_TwoSlotsInOneProcess_RemainCompletelyIsolated()
        {
            var files = new MemoryFileIO();
            var json = new SystemTextJsonSerializer();
            var log = new TestLog();
            var slotService = new SaveSlotService(files, json, log, "user://");

            var profileId = new SaveProfileId("default");
            var slot1 = new SaveSlotId("slot_1");
            var slot2 = new SaveSlotId("slot_2");

            slotService.CreateSlot(profileId, slot1);
            slotService.CreateSlot(profileId, slot2);

            var manifest1 = slotService.LoadManifest(profileId, slot1)!;
            manifest1.currentDay = 10;
            manifest1.generationId = "gen_slot1_day10";
            var payloads1 = new Dictionary<string, string>
            {
                ["inventory"] = "{\"Credits\":500}",
                ["survivors"] = "{\"Count\":4}"
            };
            var env1 = CampaignEnvelopeBuilder.Build(payloads1, manifest1);
            slotService.WriteAggregateAtomically(profileId, slot1, env1);

            var manifest2 = slotService.LoadManifest(profileId, slot2)!;
            manifest2.currentDay = 25;
            manifest2.generationId = "gen_slot2_day25";
            var payloads2 = new Dictionary<string, string>
            {
                ["inventory"] = "{\"Credits\":9999}",
                ["survivors"] = "{\"Count\":12}"
            };
            var env2 = CampaignEnvelopeBuilder.Build(payloads2, manifest2);
            slotService.WriteAggregateAtomically(profileId, slot2, env2);

            // Load Slot 1
            var load1 = slotService.TryLoadAggregate(profileId, slot1);
            Assert.True(load1.IsSuccess);
            Assert.Equal(10, load1.Envelope!.manifest.currentDay);
            Assert.Equal("gen_slot1_day10", load1.Envelope.manifest.generationId);
            Assert.Equal("{\"Credits\":500}", load1.Envelope.sections.Find(s => s.sectionName == "inventory")!.payloadJson);

            // Load Slot 2
            var load2 = slotService.TryLoadAggregate(profileId, slot2);
            Assert.True(load2.IsSuccess);
            Assert.Equal(25, load2.Envelope!.manifest.currentDay);
            Assert.Equal("gen_slot2_day25", load2.Envelope.manifest.generationId);
            Assert.Equal("{\"Credits\":9999}", load2.Envelope.sections.Find(s => s.sectionName == "inventory")!.payloadJson);
        }

        [Fact]
        public void MixedGeneration_Sections_AreRejectedByValidation()
        {
            var files = new MemoryFileIO();
            var json = new SystemTextJsonSerializer();
            var log = new TestLog();
            var slotService = new SaveSlotService(files, json, log, "user://");

            var manifest = new SaveManifest
            {
                generationId = "gen_alpha_100",
                slotId = new SaveSlotId("slot_1"),
                profileId = new SaveProfileId("default"),
                currentDay = 5
            };

            var payloads = new Dictionary<string, string>
            {
                ["inventory"] = "{\"Credits\":100}"
            };

            var env = CampaignEnvelopeBuilder.Build(payloads, manifest);

            // Inject foreign generation section
            var alienSection = new SaveSectionEnvelope
            {
                sectionName = "world",
                generationId = "gen_stale_beta_99",
                schemaVersion = 1,
                payloadJson = "{\"Weather\":\"Clear\"}"
            };
            alienSection.checksum = SaveSlotService.ComputeSectionChecksum(alienSection);
            env.sections.Add(alienSection);
            env.aggregateChecksum = SaveSlotService.ComputeAggregateChecksum(env);

            var validation = slotService.ValidateAggregate(env);
            Assert.False(validation.IsValid);
            Assert.Contains(validation.SectionErrors, e => e.Contains("generation mismatch"));
        }

        [Fact]
        public void CorruptedEnvelope_FailsValidation_AndLeavesLiveStateUntouched()
        {
            var files = new MemoryFileIO();
            var json = new SystemTextJsonSerializer();
            var log = new TestLog();
            var slotService = new SaveSlotService(files, json, log, "user://");

            var profileId = new SaveProfileId("default");
            var slotId = new SaveSlotId("slot_1");
            slotService.CreateSlot(profileId, slotId);

            // Write corrupt JSON to campaign.json
            string aggregatePath = slotService.GetAggregatePath(profileId, slotId);
            files.WriteAllText(aggregatePath, "{ malformed json: not valid }");

            var result = slotService.TryLoadAggregate(profileId, slotId);
            Assert.False(result.IsSuccess);
            Assert.Equal(SaveLoadStatus.CorruptData, result.Status);
            Assert.Contains("corrupted", result.UserMessage);
        }

        [Fact]
        public void ChecksumMismatch_FailsValidation_WithExplicitStatus()
        {
            var files = new MemoryFileIO();
            var json = new SystemTextJsonSerializer();
            var log = new TestLog();
            var slotService = new SaveSlotService(files, json, log, "user://");

            var profileId = new SaveProfileId("default");
            var slotId = new SaveSlotId("slot_1");
            slotService.CreateSlot(profileId, slotId);

            var manifest = slotService.LoadManifest(profileId, slotId)!;
            var payloads = new Dictionary<string, string> { ["inventory"] = "{\"Gold\":10}" };
            var env = CampaignEnvelopeBuilder.Build(payloads, manifest);

            // Tamper with payload without updating checksum
            env.sections[0].payloadJson = "{\"Gold\":999999}";
            string aggregatePath = slotService.GetAggregatePath(profileId, slotId);
            files.WriteAllText(aggregatePath, json.Serialize(env));

            var result = slotService.TryLoadAggregate(profileId, slotId);
            Assert.False(result.IsSuccess);
            Assert.Equal(SaveLoadStatus.ChecksumMismatch, result.Status);
        }
    }
}
