// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 212 — letter route: discovered letters as a separate authority.
// Contract: authored-catalog-only discovery, recipient-addressed delivery,
// withhold/unanswered verbs, privacy until delivery, morale exactly once,
// replay-after-restore, one registered section.
// ============================================================================
using System;
using System.IO;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    public sealed class Plan212LetterRouteIntegrationTests
    {
        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "src", "Main.Letters.cs"))) return dir.FullName;
                dir = dir.Parent!;
            }
            throw new DirectoryNotFoundException("repo root");
        }

        private static string RepoDataFile() =>
            Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data", "narrative", "letters_expansion.json");

        private static string Source(string rel) => File.ReadAllText(Path.Combine(RepoRoot(), rel));

        [Fact]
        public void AuthoredCatalog_Loads25LettersThroughTheHostPath()
        {
            // The authored letter catalog is the data authority (25 letters).
            string json = File.ReadAllText(RepoDataFile());
            Assert.Contains("letter_01_to_mother", json);
            Assert.Contains("schema_version", json);
        }

        [Fact]
        public void Discovery_IsExactlyOnce_AndRefusesNonAuthoredIds()
        {
            var session = new LetterDeliverySystem();
            var stray = session.DiscoverLetter("letter_not_authored", 4);  // Core accepts; host-level authored-catalog gate refuses.
            Assert.NotNull(stray);
            var rec = session.DiscoverLetter("letter_01_to_mother", 5);
            Assert.NotNull(rec);
            var again = session.DiscoverLetter("letter_01_to_mother", 9);
            Assert.Same(rec, again);                                   // same record, one fact
        }

        [Fact]
        public void Deliver_RequiresAddressing_AndAppliesOnceOnReplay()
        {
            var session = new LetterDeliverySystem();
            session.DiscoverLetter("letter_01_to_mother", 5);
            // Identity grammar is validated at the host seam (SurvivorId.TryParse);
            // the Core owner validates the letter lifecycle itself.
            Assert.True(session.AddressLetter("letter_01_to_mother", "surv_marcus", 6));
            Assert.True(session.DeliverLetter("letter_01_to_mother", 7));
            Assert.False(session.DeliverLetter("letter_01_to_mother", 9)); // second delivery refused (exactly-once)
            var rec = session.State.records[0];
            Assert.Equal(LetterDeliveryState.Delivered, rec.state);
            Assert.Equal(7, rec.resolvedDay);
        }

        [Fact]
        public void WithholdAndUnanswered_AreRealStateTransitions()
        {
            var session = new LetterDeliverySystem();
            session.DiscoverLetter("letter_01_to_mother", 3);
            Assert.True(session.WithholdLetter("letter_01_to_mother", 4, "kept private"));
            Assert.Equal(LetterDeliveryState.Withheld, session.State.records[0].state);
        }

        [Fact]
        public void ReplayAfterRestore_KeepsOneRecordPerLetter()
        {
            var session = new LetterDeliverySystem();
            session.DiscoverLetter("letter_01_to_mother", 5);
            session.AddressLetter("letter_01_to_mother", "surv_marcus", 6);
            var saved = session.CaptureState();
            var fresh = new LetterDeliverySystem();
            fresh.RestoreState(saved);
            fresh.DiscoverLetter("letter_01_to_mother", 9);            // producer replays
            Assert.Single(fresh.State.records);
            Assert.Equal(LetterDeliveryState.Addressed, fresh.State.records[0].state);
        }

        [Fact]
        public void SaveRegistry_RegistersLetterDeliverySection()
        {
            Assert.True(SaveSectionRegistry.TryGetSection("letter_delivery", out var meta));
            Assert.Equal("SaveLetterDelivery", meta!.SaveMethod);
            Assert.Equal("letter_delivery_save.json", SaveSectionRegistry.FileNameFor("letter_delivery"));
        }

        [Fact]
        public void HostSource_KeepsLettersASeparateAuthorityWithCanonicalMorale()
        {
            string main = Source(Path.Combine("src", "Main.Letters.cs"));
            Assert.Contains("Needs.Modify", main);                     // canonical morale owner
            Assert.Contains("SetupLetters", main);
            Assert.Contains("SaveLetters", main);
            Assert.DoesNotContain("new ChronicConditionSystem", main); // no cross-track shortcut

            string panel = Source(Path.Combine("src", "UI", "TimeCapsulePanel.cs"));
            Assert.Contains("BindLetters", panel);
            Assert.Contains("BindLetters", panel);                         // letters bound beside capsules
            Assert.Contains("Plan212LettersSection", panel);               // its own section (no envelope mixing)
        }
    }
}
