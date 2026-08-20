using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ArchiveDeskSystemTests
    {
        [Fact] public void QueueTranscription_UnknownInk_Blocks()
        {
            var a = Create(out _, out _, out _, out _);
            var r = a.QueueTranscription("evidence_1", "archivist_1", "unknown_ink");
            Assert.Equal(ActionResult.StatusKind.Failed, r.Status);
        }

        [Fact] public void QueueTranscription_MissingInkMaterial_Blocks()
        {
            var a = Create(out var inv, out _, out _, out _);
            a.LoadInkCatalog(new System.Collections.Generic.List<InkMaterialDefinition>
            {
                new InkMaterialDefinition { ink_id = "iron_gall", requiredItemId = "iron_gall_ink", requiredAmount = 1, legibilityScore = 0.8f }
            });
            var r = a.QueueTranscription("evidence_1", "archivist_1", "iron_gall");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void QueueTranscription_Valid_QueuesJob()
        {
            var a = Create(out var inv, out _, out _, out _);
            a.LoadInkCatalog(new System.Collections.Generic.List<InkMaterialDefinition>
            {
                new InkMaterialDefinition { ink_id = "iron_gall", requiredItemId = "iron_gall_ink", requiredAmount = 1, legibilityScore = 0.8f }
            });
            inv.AddById("iron_gall_ink", 2);
            var r = a.QueueTranscription("evidence_1", "archivist_1", "iron_gall");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(a.State.queue);
        }

        [Fact] public void TickDay_CompletesTranscription()
        {
            var a = Create(out var inv, out _, out var journal, out _);
            a.LoadInkCatalog(new System.Collections.Generic.List<InkMaterialDefinition>
            {
                new InkMaterialDefinition { ink_id = "iron_gall", requiredItemId = "iron_gall_ink", requiredAmount = 1, legibilityScore = 0.8f }
            });
            inv.AddById("iron_gall_ink", 2);
            a.QueueTranscription("evidence_1", "archivist_1", "iron_gall");
            a.TickDay(1);
            Assert.True(a.State.queue[0].isComplete);
            Assert.Contains("evidence_1", a.State.unlockedEvidenceIds);
        }

        [Fact] public void CompleteTranscription_AddsJournalEntry()
        {
            var a = Create(out var inv, out _, out var journal, out _);
            a.LoadInkCatalog(new System.Collections.Generic.List<InkMaterialDefinition>
            {
                new InkMaterialDefinition { ink_id = "iron_gall", requiredItemId = "iron_gall_ink", requiredAmount = 1, legibilityScore = 0.8f }
            });
            inv.AddById("iron_gall_ink", 2);
            a.QueueTranscription("evidence_1", "archivist_1", "iron_gall");
            a.TickDay(1);
            Assert.True(journal.EntryCount > 0);
        }

        [Fact] public void IsEvidenceUnlocked_ReturnsTrue()
        {
            var a = Create(out var inv, out _, out _, out _);
            a.LoadInkCatalog(new System.Collections.Generic.List<InkMaterialDefinition>
            {
                new InkMaterialDefinition { ink_id = "iron_gall", requiredItemId = "iron_gall_ink", requiredAmount = 1, legibilityScore = 0.8f }
            });
            inv.AddById("iron_gall_ink", 2);
            a.QueueTranscription("evidence_1", "archivist_1", "iron_gall");
            a.TickDay(1);
            Assert.True(a.IsEvidenceUnlocked("evidence_1"));
        }

        [Fact] public void CancelJob_RefundsInk()
        {
            var a = Create(out var inv, out _, out _, out _);
            a.LoadInkCatalog(new System.Collections.Generic.List<InkMaterialDefinition>
            {
                new InkMaterialDefinition { ink_id = "iron_gall", requiredItemId = "iron_gall_ink", requiredAmount = 1, legibilityScore = 0.8f }
            });
            inv.AddById("iron_gall_ink", 2);
            a.QueueTranscription("evidence_1", "archivist_1", "iron_gall");
            int before = inv.CountById("iron_gall_ink");
            a.CancelJob(a.State.queue[0].jobId);
            Assert.Equal(before + 1, inv.CountById("iron_gall_ink"));
        }

        [Fact] public void CaptureRestoreState_PreservesQueue()
        {
            var a = Create(out var inv, out _, out _, out _);
            a.LoadInkCatalog(new System.Collections.Generic.List<InkMaterialDefinition>
            {
                new InkMaterialDefinition { ink_id = "iron_gall", requiredItemId = "iron_gall_ink", requiredAmount = 1, legibilityScore = 0.8f }
            });
            inv.AddById("iron_gall_ink", 2);
            a.QueueTranscription("evidence_1", "archivist_1", "iron_gall");
            var state = a.CaptureState();
            Assert.Single(state.queue);

            var a2 = Create(out _, out _, out _, out _);
            a2.LoadInkCatalog(new System.Collections.Generic.List<InkMaterialDefinition>
            {
                new InkMaterialDefinition { ink_id = "iron_gall", requiredItemId = "iron_gall_ink", requiredAmount = 1, legibilityScore = 0.8f }
            });
            a2.RestoreState(state);
            Assert.Single(a2.State.queue);
        }

        private static ArchiveDeskSystem Create(out Inventory.Inventory inv, out KnowledgeBase knowledge, out JournalSystem journal, out DutyRosterSystem roster)
        {
            inv = new Inventory.Inventory();
            knowledge = new KnowledgeBase();
            journal = new JournalSystem();
            roster = new DutyRosterSystem();
            return new ArchiveDeskSystem(journal, knowledge, inv, roster);
        }
    }
}
