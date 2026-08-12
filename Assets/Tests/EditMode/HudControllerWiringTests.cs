using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Wiring tests for the previously unbound HUD controllers:
    /// MoralChronicleBridge/MoralChronicleUI, TutorialOverlay, and the
    /// Expansion4HudController serialized reference on GameBootstrap.
    /// </summary>
    [TestFixture]
    public class HudControllerWiringTests
    {
        private GameObject _hudObject;
        private HUD _hud;
        private readonly List<GameObject> _extra = new List<GameObject>();

        [SetUp]
        public void SetUp()
        {
            _hudObject = new GameObject("TestHUD");
            _hud = _hudObject.AddComponent<HUD>();
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var go in _extra)
                if (go != null) Object.DestroyImmediate(go);
            _extra.Clear();
            if (_hudObject != null) Object.DestroyImmediate(_hudObject);
            // The bridge subscribes to the process-wide bus; leave no residue.
            EventBus.Clear();
        }

        private T Track<T>(T component) where T : Component
        {
            _extra.Add(component.gameObject);
            return component;
        }

        // ── HUD exposes the new widgets ──────────────────────────────────────

        [Test]
        public void HUD_MoralChronicleUi_IsEnsured()
        {
            Assert.NotNull(_hud.MoralChronicleUI, "HUD must expose a MoralChronicleUI widget.");
            Assert.AreSame(_hud.MoralChronicleUI, _hud.EnsureMoralChronicle());
        }

        [Test]
        public void HUD_TutorialOverlay_IsEnsured()
        {
            Assert.NotNull(_hud.TutorialOverlay, "HUD must expose a TutorialOverlay widget.");
            Assert.AreSame(_hud.TutorialOverlay, _hud.EnsureTutorialOverlay());
        }

        // ── MoralChronicleBridge ─────────────────────────────────────────────

        [Test]
        public void Bridge_AfterInitialise_CampaignEndedEvent_ShowsChronicle()
        {
            var ui = _hud.EnsureMoralChronicle();
            var bridge = Track(new GameObject("Bridge").AddComponent<MoralChronicleBridge>());
            var survivors = new List<Survivor>
            {
                new Survivor { Id = "sv_1", DisplayName = "Elena" }
            };

            bridge.Initialise(ui, survivors);
            bridge.RecordMoralEntry(7, "We turned the strangers away.", MoralChronicleEntryKind.FactionChoice);

            EventBus.Raise(new CampaignEndedEvent
            {
                Result = new CampaignResult
                {
                    Mode = GameModeKind.Story,
                    TargetDurationDays = 120,
                    DaysSurvived = 40,
                    IsDefeat = true,
                    OutcomeSummary = "All survivors perished in the bunker."
                },
                ConditionKind = EndgameConditionKind.AllSurvivorsDeceased,
                IsVictory = false,
                DaysSurvived = 40
            });

            Assert.IsTrue(ui.IsVisible, "Chronicle should show after CampaignEndedEvent.");
            Assert.IsFalse(ui.IsVictory);
            Assert.AreEqual(40, ui.DaysSurvived);
            Assert.AreEqual(1, ui.SurvivorFates.Count, "Bridge-provided survivors should appear.");
            Assert.AreEqual(1, ui.Timeline.Count, "Recorded moral entry should reach the UI.");
        }

        [Test]
        public void Bridge_JournalSnapshotRequest_ServesCachedSnapshot()
        {
            var ui = _hud.EnsureMoralChronicle();
            var bridge = Track(new GameObject("Bridge").AddComponent<MoralChronicleBridge>());
            bridge.Initialise(ui, null);
            bridge.SetJournalSnapshot("Day 1 — We sealed the door.");

            ui.ActivateJournalSnapshot();

            Assert.IsTrue(ui.JournalSnapshotVisible);
            Assert.AreEqual("Day 1 — We sealed the door.", ui.JournalSnapshotText);
        }

        [Test]
        public void Bridge_Destroyed_DoesNotThrowOnCampaignEnded()
        {
            var ui = _hud.EnsureMoralChronicle();
            var bridgeGo = new GameObject("Bridge");
            _extra.Add(bridgeGo);
            var bridge = bridgeGo.AddComponent<MoralChronicleBridge>();
            bridge.Initialise(ui, null);

            Object.DestroyImmediate(bridgeGo);
            _extra.Remove(bridgeGo);

            Assert.DoesNotThrow(() => EventBus.Raise(new CampaignEndedEvent
            {
                Result = new CampaignResult { IsDefeat = true, OutcomeSummary = "Gone." },
                IsVictory = false,
                DaysSurvived = 5
            }), "A destroyed bridge's lingering EventBus handler must no-op, not throw.");
        }

        [Test]
        public void Bridge_Reinitialise_DoesNotStackUiSubscriptions()
        {
            var ui = _hud.EnsureMoralChronicle();
            var bridge = Track(new GameObject("Bridge").AddComponent<MoralChronicleBridge>());
            bridge.Initialise(ui, null);
            bridge.Initialise(ui, null); // WireHUD re-run must not double-subscribe

            int forwarded = 0;
            bridge.OnMainMenuRequested += () => forwarded++;
            ui.ActivateMainMenu();

            Assert.AreEqual(1, forwarded, "Re-initialised bridge must forward the request exactly once.");
        }

        [Test]
        public void Bridge_SaveRoundTrip_PreservesTimelineAndSnapshot()
        {
            var ui = _hud.EnsureMoralChronicle();
            var bridge = Track(new GameObject("Bridge").AddComponent<MoralChronicleBridge>());
            bridge.Initialise(ui, null);
            bridge.RecordMoralEntry(3, "We rationed the last filters.", MoralChronicleEntryKind.DesperateChoice);
            bridge.SetJournalSnapshot("Day 3 — The filter hums.");

            var save = bridge.CaptureState();
            string json = JsonUtility.ToJson(save);
            var restored = JsonUtility.FromJson<MoralChronicleBridge.BridgeSave>(json);

            var bridge2 = Track(new GameObject("Bridge2").AddComponent<MoralChronicleBridge>());
            bridge2.RestoreState(restored);
            var save2 = bridge2.CaptureState();

            Assert.AreEqual(1, save2.Timeline.Length);
            Assert.AreEqual("We rationed the last filters.", save2.Timeline[0].Description);
            Assert.AreEqual(MoralChronicleEntryKind.DesperateChoice, save2.Timeline[0].Kind);
            Assert.AreEqual("Day 3 — The filter hums.", save2.JournalSnapshot);
        }

        // ── TutorialOverlay ──────────────────────────────────────────────────

        [Test]
        public void Tutorial_FullWalkthrough_EndsActive_FiresEndedOnce()
        {
            var tutorial = _hud.EnsureTutorialOverlay();
            int endedCount = 0;
            tutorial.OnTutorialEnded += () => endedCount++;

            tutorial.StartTutorial();
            Assert.IsTrue(tutorial.IsActive);
            Assert.AreEqual(TutorialOverlay.TutorialStep.Welcome, tutorial.CurrentStep);
            Assert.IsFalse(string.IsNullOrEmpty(tutorial.CurrentMessage));

            int guard = 0;
            while (tutorial.IsActive && guard++ < 16)
                tutorial.Advance();

            Assert.IsFalse(tutorial.IsActive, "Advancing past the last step should end the tutorial.");
            Assert.AreEqual(1, endedCount, "OnTutorialEnded must fire exactly once.");
        }

        [Test]
        public void Tutorial_DayRollover_AutoEndsOnDay3()
        {
            int day = 1;
            var tutorial = _hud.EnsureTutorialOverlay();
            tutorial.SetDayProvider(() => day);
            int endedCount = 0;
            tutorial.OnTutorialEnded += () => endedCount++;

            tutorial.StartTutorial();
            tutorial.CheckDayRollover();
            Assert.IsTrue(tutorial.IsActive, "Day 1 should not end the tutorial.");

            day = 2;
            tutorial.CheckDayRollover();
            Assert.IsTrue(tutorial.IsActive, "Day 2 should not end the tutorial.");

            day = 3;
            tutorial.CheckDayRollover();
            Assert.IsFalse(tutorial.IsActive, "Day 3 rollover should end the tutorial.");
            Assert.AreEqual(1, endedCount);
        }

        [Test]
        public void Tutorial_EndWhileInactive_DoesNotFireEnded()
        {
            var tutorial = _hud.EnsureTutorialOverlay();
            int endedCount = 0;
            tutorial.OnTutorialEnded += () => endedCount++;

            tutorial.EndTutorial();
            Assert.AreEqual(0, endedCount, "Ending an inactive tutorial is a no-op.");
        }

        // ── Expansion4HudController reference ────────────────────────────────

        [Test]
        public void GameBootstrap_DeclaresSerializedExpansion4HudController()
        {
            var field = typeof(GameBootstrap).GetField(
                "_expansion4HudController",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(field, "GameBootstrap must declare _expansion4HudController.");
            Assert.AreEqual(typeof(Expansion4HudController), field.FieldType);
            Assert.NotNull(field.GetCustomAttribute<SerializeField>(),
                "_expansion4HudController must be inspector-assignable.");
        }
    }
}
