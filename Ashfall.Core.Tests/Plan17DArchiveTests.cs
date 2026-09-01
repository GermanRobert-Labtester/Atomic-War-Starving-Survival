// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: Plan 17D — Archive desk and ink system tests.
// Validates ArchiveInkCatalogLoader loading, ArchiveDeskSystem transcription
// pipeline (queue, tick, cancel/refund, completion), and save/load round-trip.

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class Plan17DArchiveTests
    {
        // -----------------------------------------------------------------
        // Helpers
        // -----------------------------------------------------------------

        private static string FindDataDir()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
            return dataDir ?? string.Empty;
        }

        private static ArchiveDeskSystem CreateSystem(
            out Ashfall.Core.Inventory.Inventory inv, out KnowledgeBase knowledge,
            out JournalSystem journal, out DutyRosterSystem roster)
        {
            inv = new Ashfall.Core.Inventory.Inventory();
            knowledge = new KnowledgeBase();
            journal = new JournalSystem();
            roster = new DutyRosterSystem();
            return new ArchiveDeskSystem(journal, knowledge, inv, roster);
        }

        private static List<InkMaterialDefinition> SampleInks()
        {
            return new List<InkMaterialDefinition>
            {
                new InkMaterialDefinition
                {
                    ink_id = "iron_gall",
                    requiredItemId = "charcoal",
                    requiredAmount = 2,
                    legibilityScore = 0.9f
                },
                new InkMaterialDefinition
                {
                    ink_id = "soot_lamp",
                    requiredItemId = "charcoal",
                    requiredAmount = 1,
                    legibilityScore = 0.7f
                }
            };
        }

        // -----------------------------------------------------------------
        // ArchiveInkCatalogLoader — loads inks from JSON
        // -----------------------------------------------------------------

        [Fact]
        public void ArchiveInkCatalogLoader_LoadsInksFromJson()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var defs = ArchiveInkCatalogLoader.Load(dataDir, io, json);

            Assert.NotNull(defs);
            Assert.True(defs.Count >= 3, $"Expected >= 3 inks, got {defs.Count}");
            Assert.Contains(defs, d => d.ink_id == "ink_iron_gall");
            Assert.Contains(defs, d => d.ink_id == "ink_soot_lamp");
            Assert.Contains(defs, d => d.ink_id == "ink_plant_dye");
        }

        [Fact]
        public void ArchiveInkCatalogLoader_LoadAndRegister_PopulatesSystem()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var system = CreateSystem(out _, out _, out _, out _);
            int count = ArchiveInkCatalogLoader.LoadAndRegister(
                system, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.True(count >= 3, $"Expected >= 3 registered, got {count}");
        }

        // -----------------------------------------------------------------
        // ArchiveDeskSystem — LoadInkCatalog populates catalog
        // -----------------------------------------------------------------

        [Fact]
        public void ArchiveDeskSystem_LoadInkCatalog_PopulatesCatalog()
        {
            var system = CreateSystem(out var inv, out _, out _, out _);
            system.LoadInkCatalog(SampleInks());
            inv.AddById("charcoal", 10);

            // Verify by queuing a transcription with a known ink
            var result = system.QueueTranscription("evidence_test", "archivist_1", "iron_gall");
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
        }

        // -----------------------------------------------------------------
        // QueueTranscription — consumes ink
        // -----------------------------------------------------------------

        [Fact]
        public void ArchiveDeskSystem_QueueTranscription_ConsumesInk()
        {
            var system = CreateSystem(out var inv, out _, out _, out _);
            system.LoadInkCatalog(SampleInks());
            inv.AddById("charcoal", 10);

            int before = inv.CountById("charcoal");
            var result = system.QueueTranscription("evidence_1", "archivist_1", "iron_gall");

            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal(before - 2, inv.CountById("charcoal")); // iron_gall costs 2 charcoal
        }

        // -----------------------------------------------------------------
        // QueueTranscription — fails with insufficient ink
        // -----------------------------------------------------------------

        [Fact]
        public void ArchiveDeskSystem_QueueTranscription_FailsWithInsufficientInk()
        {
            var system = CreateSystem(out var inv, out _, out _, out _);
            system.LoadInkCatalog(SampleInks());
            inv.AddById("charcoal", 1); // only 1, but iron_gall needs 2

            var result = system.QueueTranscription("evidence_1", "archivist_1", "iron_gall");

            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
            Assert.Empty(system.State.queue);
            Assert.Equal(1, inv.CountById("charcoal")); // ink not consumed
        }

        // -----------------------------------------------------------------
        // TickDay — progresses transcription
        // -----------------------------------------------------------------

        [Fact]
        public void ArchiveDeskSystem_TickDay_ProgressesTranscription()
        {
            var system = CreateSystem(out var inv, out _, out _, out _);
            system.LoadInkCatalog(SampleInks());
            inv.AddById("charcoal", 10);

            system.QueueTranscription("evidence_1", "archivist_1", "iron_gall");
            Assert.Single(system.State.queue);
            Assert.False(system.State.queue[0].isComplete);

            system.TickDay(1);

            // After one tick, progress should have advanced
            Assert.True(system.State.queue[0].progressHours > 0);
        }

        // -----------------------------------------------------------------
        // Transcription completion creates journal entry
        // -----------------------------------------------------------------

        [Fact]
        public void ArchiveDeskSystem_TranscriptionCompletion_CreatesJournalEntry()
        {
            var system = CreateSystem(out var inv, out _, out var journal, out _);
            system.LoadInkCatalog(SampleInks());
            inv.AddById("charcoal", 10);

            system.QueueTranscription("evidence_complete", "archivist_1", "iron_gall");
            system.TickDay(1);

            Assert.True(system.State.queue[0].isComplete);
            Assert.True(journal.EntryCount > 0,
                "Transcription completion should create a journal entry");
            Assert.True(system.IsEvidenceUnlocked("evidence_complete"));
        }

        // -----------------------------------------------------------------
        // CancelJob — refunds ink
        // -----------------------------------------------------------------

        [Fact]
        public void ArchiveDeskSystem_CancelJob_RefundsInk()
        {
            var system = CreateSystem(out var inv, out _, out _, out _);
            system.LoadInkCatalog(SampleInks());
            inv.AddById("charcoal", 10);

            system.QueueTranscription("evidence_cancel", "archivist_1", "iron_gall");
            int afterQueue = inv.CountById("charcoal");

            string jobId = system.State.queue[0].jobId;
            var cancelResult = system.CancelJob(jobId);

            Assert.Equal(ActionResult.StatusKind.Success, cancelResult.Status);
            Assert.Equal(afterQueue + 2, inv.CountById("charcoal")); // refunded 2 charcoal
            // Cancelled job remains in queue with isCancelled flag (not removed)
            Assert.Single(system.State.queue);
            Assert.True(system.State.queue[0].isCancelled);
        }

        // -----------------------------------------------------------------
        // Save/load round-trip
        // -----------------------------------------------------------------

        [Fact]
        public void ArchiveDeskSystem_SaveLoad_RoundTrip_PreservesState()
        {
            var system = CreateSystem(out var inv, out _, out _, out _);
            system.LoadInkCatalog(SampleInks());
            inv.AddById("charcoal", 10);

            system.QueueTranscription("evidence_save", "archivist_1", "iron_gall");
            Assert.Single(system.State.queue);

            // Capture state
            var state = system.CaptureState();
            Assert.NotNull(state);
            Assert.Single(state.queue);

            // Serialize → deserialize
            var ser = new SystemTextJsonSerializer();
            string json = ser.Serialize(state);
            var restoredState = ser.Deserialize<ArchiveDeskState>(json);
            Assert.NotNull(restoredState);

            // Restore into fresh system
            var system2 = CreateSystem(out _, out _, out _, out _);
            system2.LoadInkCatalog(SampleInks());
            system2.RestoreState(restoredState);

            Assert.Single(system2.State.queue);
            Assert.Equal("evidence_save", system2.State.queue[0].evidenceId);
            Assert.Equal("iron_gall", system2.State.queue[0].inkId);
        }

        // -----------------------------------------------------------------
        // Unknown ink is rejected
        // -----------------------------------------------------------------

        [Fact]
        public void ArchiveDeskSystem_QueueTranscription_UnknownInk_Fails()
        {
            var system = CreateSystem(out var inv, out _, out _, out _);
            system.LoadInkCatalog(SampleInks());
            inv.AddById("charcoal", 10);

            var result = system.QueueTranscription("evidence_x", "archivist_1", "nonexistent_ink");
            Assert.Equal(ActionResult.StatusKind.Failed, result.Status);
        }
    }
}
