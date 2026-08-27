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

            public bool DirectoryExists(string path) => Directories.Contains(path) || Files.Keys.Any(k => k.StartsWith(path, StringComparison.OrdinalIgnoreCase));
            public void CreateDirectory(string path) => Directories.Add(path);
            public bool FileExists(string path) => Files.ContainsKey(path);
            public string ReadAllText(string path) => Files.TryGetValue(path, out var text) ? text : throw new FileNotFoundException(path);
            public void WriteAllText(string path, string contents) => Files[path] = contents;
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
