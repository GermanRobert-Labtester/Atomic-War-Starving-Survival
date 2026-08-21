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

        [Fact] public void QueueTranscription_ArchivistOnDuty_DoesNotConsumeInk()
        {
            // CR3-04 regression: QueueTranscription previously consumed
            // `_inventory.RemoveById(ink.requiredItemId, ink.requiredAmount)`
            // BEFORE checking `_roster.GetAssignment(archivistId) != null`.
            // If the archivist was on a duty shift, ink was consumed and never
            // refunded. The fix pre-checks all gating predicates (ink availability
            // AND roster state) before any inventory mutation. Twin to
            // CR3-02 / CR3-03 atomicity pattern; same drain-by-block pattern.
            var a = Create(out var inv, out _, out _, out var roster);
            a.LoadInkCatalog(new System.Collections.Generic.List<InkMaterialDefinition>
            {
                new InkMaterialDefinition { ink_id = "iron_gall", requiredItemId = "iron_gall_ink", requiredAmount = 1, legibilityScore = 0.8f }
            });
            inv.AddById("iron_gall_ink", 2);
            // Place archivist on a duty shift.
            roster.Unlock(0);
            roster.WriteName("archivist_busy", displayName: "Archivist Busy",
                occupationObserved: "scribe", script: DutyRosterSystem.ScriptPencil,
                day: 1, sleptHere: true);
            Assert.True(roster.Assign(DutyRosterSystem.AssignmentRoles[0], "archivist_busy"));
            int inkBefore = inv.CountById("iron_gall_ink");
            var r = a.QueueTranscription("evidence_1", "archivist_busy", "iron_gall");
            // Block on busy archivist; ink inventory must be UNCHANGED.
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
            Assert.Equal(inkBefore, inv.CountById("iron_gall_ink"));
            Assert.Empty(a.State.queue);
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

        [Fact] public void CancelJob_AfterCompletion_IsBlocked()
        {
            // BUG-13 latent guard pin: CancelJob is currently guarded against
            // already-completed/cancelled jobs (returns Blocked "no_job").
            // Without this regression test, a future refactor could loosen
            // the guard and silently allow cancellation after completion.
            var a = Create(out var inv, out _, out _, out _);
            a.LoadInkCatalog(new System.Collections.Generic.List<InkMaterialDefinition>
            {
                new InkMaterialDefinition { ink_id = "iron_gall", requiredItemId = "iron_gall_ink", requiredAmount = 1, legibilityScore = 0.8f }
            });
            inv.AddById("iron_gall_ink", 2);
            a.QueueTranscription("evidence_1", "archivist_1", "iron_gall");
            a.TickDay(1); // job completes; ink spent; evidence unlocked.
            Assert.True(a.IsEvidenceUnlocked("evidence_1"));
            int inkAfterComplete = inv.CountById("iron_gall_ink");
            // Late cancel — must be blocked, must NOT refund ink.
            var lateCancel = a.CancelJob(a.State.queue[0].jobId);
            Assert.Equal(ActionResult.StatusKind.Blocked, lateCancel.Status);
            Assert.Equal(inkAfterComplete, inv.CountById("iron_gall_ink"));
            Assert.True(a.IsEvidenceUnlocked("evidence_1"));
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
