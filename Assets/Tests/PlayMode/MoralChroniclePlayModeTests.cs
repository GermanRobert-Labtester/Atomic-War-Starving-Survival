using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AtomicWar._Game.Core;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.PlayMode
{
    /// <summary>
    /// PlayMode tests for Prompt #42: Post-Game Campaign Summary and Moral Chronicle UI.
    ///
    /// Tests:
    /// 1. CampaignEndedEvent → MoralChronicleUI becomes visible with correct metadata.
    /// 2. Survivor fates populate correctly (alive vs. dead).
    /// 3. Moral timeline entries appear in chronological order.
    /// 4. "Main Menu" button flag is set and event fires.
    /// 5. "View Final Journal Snapshot" button shows snapshot text.
    /// 6. MoralChronicleBridge auto-populates chronicle when EndgameEngine fires.
    /// </summary>
    [TestFixture]
    public class MoralChroniclePlayModeTests
    {
        // ─────────────────────────────────────────────────────────────────────
        // 1. Chronicle shows victory metadata when CampaignEndedEvent fires
        // ─────────────────────────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator VictoryEvent_ShowsChronicle_WithCorrectMetadata()
        {
            var go = new GameObject("ChronicleUI");
            var ui = go.AddComponent<MoralChronicleUI>();

            var fates = new List<SurvivorFateSummary>
            {
                new SurvivorFateSummary { DisplayName = "Elena", FateDescription = "Alive", Survived = true },
                new SurvivorFateSummary { DisplayName = "Marcus", FateDescription = "Dead — starvation", Survived = false }
            };
            var timeline = new List<MoralChronicleEntry>
            {
                new MoralChronicleEntry { Day = 10, Description = "We chose to endure.", Kind = MoralChronicleEntryKind.DesperateChoice }
            };

            ui.Show(
                isVictory: true,
                outcomeLabel: "VICTORY",
                outcomeSummary: "Military extraction completed successfully after broadcast contact.",
                gameModeLabel: "Story (120 days)",
                daysSurvived: 62,
                targetDurationDays: 120,
                campaignStartDate: "August 25, 2026",
                survivorFates: fates,
                timeline: timeline
            );

            yield return null;

            Assert.IsTrue(ui.IsVisible, "Chronicle should be visible after Show().");
            Assert.IsTrue(ui.IsVictory, "IsVictory should be true for victory outcome.");
            Assert.AreEqual(62, ui.DaysSurvived);
            Assert.AreEqual(120, ui.TargetDurationDays);
            Assert.AreEqual("August 25, 2026", ui.CampaignStartDate);
            Assert.AreEqual("Story (120 days)", ui.GameModeLabel);
            Assert.IsTrue(ui.StatusLine.Contains("VICTORY"), "StatusLine should contain VICTORY.");
            Assert.IsTrue(ui.DetailSummary.Contains("Military extraction"), "Detail should contain outcome summary.");

            UnityEngine.Object.DestroyImmediate(go);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 2. Defeat metadata renders correctly
        // ─────────────────────────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator DefeatEvent_ShowsChronicle_WithCorrectMetadata()
        {
            var go = new GameObject("ChronicleUI");
            var ui = go.AddComponent<MoralChronicleUI>();

            ui.Show(
                isVictory: false,
                outcomeLabel: "DEFEAT",
                outcomeSummary: "All survivors perished in the bunker.",
                gameModeLabel: "Expert (180 days)",
                daysSurvived: 23,
                targetDurationDays: 180,
                campaignStartDate: "August 25, 2026",
                survivorFates: null,
                timeline: null
            );

            yield return null;

            Assert.IsTrue(ui.IsVisible);
            Assert.IsFalse(ui.IsVictory);
            Assert.AreEqual(23, ui.DaysSurvived);
            Assert.IsTrue(ui.StatusLine.Contains("DEFEAT"), "StatusLine should contain DEFEAT.");
            Assert.IsTrue(ui.DetailSummary.Contains("All survivors perished"), "Detail should contain defeat summary.");

            UnityEngine.Object.DestroyImmediate(go);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 3. Survivor fates in the chronicle list (alive + dead)
        // ─────────────────────────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator SurvivorFates_DisplayBothAliveAndDead()
        {
            var go = new GameObject("ChronicleUI");
            var ui = go.AddComponent<MoralChronicleUI>();

            var fates = new List<SurvivorFateSummary>
            {
                new SurvivorFateSummary { DisplayName = "Elena", FateDescription = "Alive", Survived = true, TotalRadiationAbsorbed = 120f },
                new SurvivorFateSummary { DisplayName = "Marcus", FateDescription = "Dead — radiation", Survived = false, TraumaCount = 2 }
            };

            ui.Show(false, "DEFEAT", "Two survivors entered; none left standing.", "Custom (60 days)",
                60, 60, "August 25, 2026", fates, null);

            yield return null;

            Assert.AreEqual(2, ui.SurvivorFates.Count);
            Assert.IsTrue(ui.SurvivorFates[0].Survived, "Elena should be alive.");
            Assert.IsFalse(ui.SurvivorFates[1].Survived, "Marcus should be dead.");
            Assert.IsTrue(ui.DetailSummary.Contains("Elena"), "Detail should name Elena.");
            Assert.IsTrue(ui.DetailSummary.Contains("Marcus"), "Detail should name Marcus.");

            UnityEngine.Object.DestroyImmediate(go);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 4. Moral timeline entries appear in chronological order
        // ─────────────────────────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator MoralTimeline_IsSortedChronologically()
        {
            var go = new GameObject("ChronicleUI");
            var ui = go.AddComponent<MoralChronicleUI>();

            var timeline = new List<MoralChronicleEntry>
            {
                new MoralChronicleEntry { Day = 50, Description = "Late choice.", Kind = MoralChronicleEntryKind.DesperateChoice },
                new MoralChronicleEntry { Day = 10, Description = "Early choice.", Kind = MoralChronicleEntryKind.DesperateChoice },
                new MoralChronicleEntry { Day = 30, Description = "Mid choice.", Kind = MoralChronicleEntryKind.SurvivorLost }
            };

            ui.Show(true, "VICTORY", "Self-sufficient.", "Story (120 days)",
                100, 120, "August 25, 2026", null, timeline);

            yield return null;

            Assert.AreEqual(3, ui.Timeline.Count);
            Assert.AreEqual(10, ui.Timeline[0].Day, "First entry should be Day 10 (earliest).");
            Assert.AreEqual(30, ui.Timeline[1].Day, "Second entry should be Day 30.");
            Assert.AreEqual(50, ui.Timeline[2].Day, "Third entry should be Day 50 (latest).");
            Assert.IsTrue(ui.DetailSummary.Contains("Early choice."), "Detail should contain early choice.");

            UnityEngine.Object.DestroyImmediate(go);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 5. Main Menu button sets flag and fires event
        // ─────────────────────────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator MainMenuButton_SetsFlag_AndFiresEvent()
        {
            var go = new GameObject("ChronicleUI");
            var ui = go.AddComponent<MoralChronicleUI>();

            ui.Show(false, "DEFEAT", "Everyone is gone.", "Story (120 days)",
                5, 120, "August 25, 2026", null, null);

            bool eventFired = false;
            ui.OnMainMenuRequested += () => eventFired = true;

            yield return null;

            ui.ActivateMainMenu();

            Assert.IsTrue(ui.MainMenuRequested, "MainMenuRequested should be true after button activation.");
            Assert.IsTrue(eventFired, "OnMainMenuRequested event should have fired.");

            ui.ConsumeMainMenuRequest();
            Assert.IsFalse(ui.MainMenuRequested, "MainMenuRequested should be false after consuming.");

            UnityEngine.Object.DestroyImmediate(go);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 6. Journal snapshot button shows snapshot text
        // ─────────────────────────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator JournalSnapshotButton_ShowsSnapshotText()
        {
            var go = new GameObject("ChronicleUI");
            var ui = go.AddComponent<MoralChronicleUI>();

            ui.Show(true, "VICTORY", "We made it.", "Custom (100 days)",
                100, 100, "August 25, 2026", null, null);

            yield return null;

            bool snapshotRequested = false;
            ui.OnJournalSnapshotRequested += () => snapshotRequested = true;

            ui.ActivateJournalSnapshot();

            Assert.IsTrue(ui.JournalSnapshotRequested, "JournalSnapshotRequested should be true.");
            Assert.IsTrue(snapshotRequested, "OnJournalSnapshotRequested event should have fired.");

            const string journalText = "Day 1 — Elena: The bunker doors are sealed. Outside is silence.\nDay 60 — Marcus: A voice on the radio. They are coming.";
            ui.ConsumeJournalSnapshotRequest(journalText);

            Assert.IsFalse(ui.JournalSnapshotRequested, "JournalSnapshotRequested should be false after consuming.");
            Assert.IsTrue(ui.JournalSnapshotVisible, "Journal snapshot should be visible.");
            Assert.AreEqual(journalText, ui.JournalSnapshotText, "Snapshot text should match.");
            Assert.IsTrue(ui.DetailSummary.Contains("Day 60"), "Detail should contain journal text.");

            ui.CloseJournalSnapshot();
            Assert.IsFalse(ui.JournalSnapshotVisible, "Journal snapshot should be hidden after closing.");

            UnityEngine.Object.DestroyImmediate(go);
        }

        // ─────────────────────────────────────────────────────────────────────
        // 7. MoralChronicleBridge auto-populates UI when EndgameEngine fires
        // ─────────────────────────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator Bridge_OnCampaignEndedEvent_PopulatesChronicleUI()
        {
            // Arrange — create bridge and UI GameObjects
            var bridgeGO = new GameObject("Bridge");
            var bridge = bridgeGO.AddComponent<MoralChronicleBridge>();

            var uiGO = new GameObject("ChronicleUI");
            var ui = uiGO.AddComponent<MoralChronicleUI>();

            var survivors = new List<Survivor>
            {
                new Survivor { Id = "sv_1", DisplayName = "Elena" },
                new Survivor { Id = "sv_2", DisplayName = "Marcus" }
            };
            survivors[1].State = SurvivorState.Dead;

            bridge.Initialise(ui, survivors);

            // Pre-populate a moral entry
            bridge.RecordMoralEntry(12, "We abandoned the weakest among us to the cold.", MoralChronicleEntryKind.DesperateChoice);
            bridge.SetJournalSnapshot("Day 12 — Elena: We had no choice. Or so we told ourselves.");

            // Act — fire endgame via EndgameEngine
            var engine = new EndgameEngine(GameModeKind.Story, 120);
#pragma warning disable CS0219 // engineFired is used for debugging in PlayMode; keep the assignment.
            bool engineFired = false;
#pragma warning restore CS0219
            engine.OnCampaignEnded += _ => engineFired = true;

            // Build minimal passing state (rescue victory, two days past the
            // chopper calendar so the day the UI reports is distinguishable
            // from the threshold itself).
            int endDay = VictoryProjectManager.ChopperArrivalDay + 2;
            engine.Evaluate(
                currentDay: endDay,
                survivors: new List<Survivor> { new Survivor { Id = "sv_alive" } }, // alive survivor list (engine check)
                shelter: null,
                isExtractionUnlocked: true,
                isHydroponicsOperational: false,
                totalDeathsRecorded: 0
            );

            yield return null;

            // Assert — Bridge is subscribed to EventBus, engine raises there.
            // After 1 frame, UI should be visible with data.
            Assert.IsTrue(ui.IsVisible, "MoralChronicleUI should be visible after CampaignEndedEvent.");
            Assert.IsTrue(ui.IsVictory, "Chronicle should reflect victory.");
            Assert.AreEqual(endDay, ui.DaysSurvived);
            Assert.AreEqual(2, ui.SurvivorFates.Count, "Both bridge-provided survivors should appear.");
            Assert.AreEqual(1, ui.Timeline.Count, "The moral entry should be in the timeline.");
            Assert.AreEqual(12, ui.Timeline[0].Day);

            // Clean up EventBus subscribers
            EventBus.Clear();
            UnityEngine.Object.DestroyImmediate(bridgeGO);
            UnityEngine.Object.DestroyImmediate(uiGO);
        }
    }
}
