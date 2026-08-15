using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using AtomicWar._Game.Core;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.PlayMode
{
    [TestFixture]
    public class Phase11UiTests
    {
        const string SceneName = "Gameplay";

        [UnitySetUp]
        public IEnumerator LoadGameplayScene()
        {
            yield return SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);
            yield return null;
        }

        private static HUD FindHud()
        {
            var hud = Object.FindAnyObjectByType<HUD>();
            Assert.IsNotNull(hud, "Gameplay scene must contain HUD");
            return hud;
        }

        private static void FocusPhase11(HUD hud, string survivorId)
        {
            hud.RadiationPhaseIndicator.SetFocusedSurvivor(survivorId);
            hud.HypervigilanceIndicator.SetFocusedSurvivor(survivorId);
            hud.MoralBranchDisplay.SetFocusedSurvivor(survivorId);
            hud.KeepsakeSlotUi.SetFocusedSurvivor(survivorId);
            hud.TerminalPrognosisBanner.SetFocusedSurvivor(survivorId);
            hud.AddictionDetoxIndicator.SetFocusedSurvivor(survivorId);
        }

        [UnityTest]
        public IEnumerator AllWidgets_ExposedAndNonNullAfterBoot()
        {
            var hud = FindHud();
            yield return null;

            Assert.IsNotNull(hud.RadiationPhaseIndicator);
            Assert.IsNotNull(hud.PhantomMemoryVignette);
            Assert.IsNotNull(hud.HypervigilanceIndicator);
            Assert.IsNotNull(hud.MoralBranchDisplay);
            Assert.IsNotNull(hud.KeepsakeSlotUi);
            Assert.IsNotNull(hud.MemorialWallUi);
            Assert.IsNotNull(hud.TerminalPrognosisBanner);
            Assert.IsNotNull(hud.AddictionDetoxIndicator);
        }

        [UnityTest]
        public IEnumerator RadiationPhaseIndicator_SetPhase_UpdatesState()
        {
            var hud = FindHud();
            yield return null;

            FocusPhase11(hud, "test_sv");
            hud.RadiationPhaseIndicator.SetPhase("test_sv", RadiationSicknessPhase.Prodromal);
            yield return null;
            var state = hud.RadiationPhaseIndicator.CaptureState();
            Assert.AreEqual(RadiationSicknessPhase.Prodromal, state.focusedPhase);
        }

        [UnityTest]
        public IEnumerator PhantomMemoryVignette_Trigger_UpdatesCaptureState()
        {
            var hud = FindHud();
            yield return null;

            hud.PhantomMemoryVignette.Trigger("Test", "Memory text", true);
            yield return null;
            Assert.IsTrue(hud.PhantomMemoryVignette.CaptureState().isMotivation);
        }

        [UnityTest]
        public IEnumerator HypervigilanceIndicator_UpdateLevel_Persists()
        {
            var hud = FindHud();
            yield return null;

            FocusPhase11(hud, "sv_test");
            hud.HypervigilanceIndicator.UpdateLevel("sv_test", 0.55f);
            yield return null;
            Assert.AreEqual(0.55f, hud.HypervigilanceIndicator.CaptureState().focusedLevel, 0.01f);
        }

        [UnityTest]
        public IEnumerator MoralBranchDisplay_SetBranch_Persists()
        {
            var hud = FindHud();
            yield return null;

            FocusPhase11(hud, "sv_test");
            hud.MoralBranchDisplay.SetBranch("sv_test", MoralBranchDirection.BurdenedCompassion);
            yield return null;
            Assert.AreEqual(MoralBranchDirection.BurdenedCompassion,
                hud.MoralBranchDisplay.CaptureState().focusedBranch);
        }

        [UnityTest]
        public IEnumerator TerminalPrognosisBanner_Show_Persists()
        {
            var hud = FindHud();
            yield return null;

            FocusPhase11(hud, "sv_test");
            hud.TerminalPrognosisBanner.Show("sv_test", 3f, "deliver_letter");
            yield return null;
            Assert.AreEqual(3f, hud.TerminalPrognosisBanner.CaptureState().focused.daysRemaining, 0.01f);
        }

        [UnityTest]
        public IEnumerator AllNineteenExpansionWidgets_ExposedAndNonNullAfterBoot()
        {
            var hud = FindHud();
            yield return null;

            Assert.IsNotNull(hud.RadiationPhaseIndicator, "RadiationPhaseIndicator");
            Assert.IsNotNull(hud.PhantomMemoryVignette, "PhantomMemoryVignette");
            Assert.IsNotNull(hud.HypervigilanceIndicator, "HypervigilanceIndicator");
            Assert.IsNotNull(hud.MoralBranchDisplay, "MoralBranchDisplay");
            Assert.IsNotNull(hud.KeepsakeSlotUi, "KeepsakeSlotUi");
            Assert.IsNotNull(hud.MemorialWallUi, "MemorialWallUi");
            Assert.IsNotNull(hud.TerminalPrognosisBanner, "TerminalPrognosisBanner");
            Assert.IsNotNull(hud.AddictionDetoxIndicator, "AddictionDetoxIndicator");
            Assert.IsNotNull(hud.LocationDetailPanel, "LocationDetailPanel");
            Assert.IsNotNull(hud.ItemConditionBadge, "ItemConditionBadge");
            Assert.IsNotNull(hud.QuestlineProgressTracker, "QuestlineProgressTracker");
            Assert.IsNotNull(hud.SiegeStatusHud, "SiegeStatusHud");
            Assert.IsNotNull(hud.FactionIntelligencePanel, "FactionIntelligencePanel");
            Assert.IsNotNull(hud.VehicleStatusPanel, "VehicleStatusPanel");
            Assert.IsNotNull(hud.TacticalCommandBar, "TacticalCommandBar");
            Assert.IsNotNull(hud.QuestlineStageTracker, "QuestlineStageTracker");
            Assert.IsNotNull(hud.LoreCodexPanel, "LoreCodexPanel");
            Assert.IsNotNull(hud.FactionRelationshipMap, "FactionRelationshipMap");
            Assert.IsNotNull(hud.CharacterArcProgressPanel, "CharacterArcProgressPanel");
        }

        [UnityTest]
        public IEnumerator DiegeticRoot_ExposesNineteenNamedWidgets()
        {
            var hud = FindHud();
            var diegetic = hud.EnsureDiegeticHud();
            Assert.IsNotNull(diegetic);
            diegetic.EnsureDocumentMounted();
            diegetic.EnsureBuilt();
            yield return null;

            var root = diegetic.Document.rootVisualElement;
            Assert.IsNotNull(root);
            Assert.IsNotNull(root.Q("diegetic-root"), "diegetic-root");
            Assert.AreEqual(19, DiegeticHudController.ExpansionWidgetRootNames.Length);
            for (int i = 0; i < DiegeticHudController.ExpansionWidgetRootNames.Length; i++)
            {
                string name = DiegeticHudController.ExpansionWidgetRootNames[i];
                Assert.IsNotNull(root.Q(name), "live HUD must keep " + name);
            }
        }

        [UnityTest]
        public IEnumerator ExpansionWidgets_ProvokeUpdatesVisibleTree()
        {
            var hud = FindHud();
            var diegetic = hud.EnsureDiegeticHud();
            diegetic.EnsureDocumentMounted();
            diegetic.EnsureBuilt();
            var doc = diegetic.Document;
            var root = doc.rootVisualElement;
            yield return null;

            hud.LocationDetailPanel.BindDocument(doc);
            hud.LocationDetailPanel.ShowLocation(
                "ruined_clinic", "Ruined Clinic", 0.8f, 0.12f, 0.4f, "none",
                new List<LootPreviewEntry>
                {
                    new LootPreviewEntry { ItemId = "iodine_pills", DisplayName = "Iodine pills", DropChance = 0.5f }
                });
            yield return null;
            Assert.AreEqual("Ruined Clinic", root.Q<Label>("location-name").text);
            Assert.AreNotEqual(DisplayStyle.None, root.Q("location-detail-panel").resolvedStyle.display);

            hud.SiegeStatusHud.BindDocument(doc);
            hud.SiegeStatusHud.ShowSiege(0.72f, 2, 0.35f);
            yield return null;
            Assert.AreNotEqual(DisplayStyle.None, root.Q("siege-status").resolvedStyle.display);
            Assert.AreEqual(72f, root.Q<ProgressBar>("hatch-integrity-bar").value, 0.5f);

            int commandClicks = 0;
            hud.TacticalCommandBar.BindDocument(doc);
            hud.TacticalCommandBar.ShowCommands(
                new[] { true, true, true, true, true },
                new[] { 0f, 0f, 0f, 0f, 0f },
                _ => commandClicks++);
            yield return null;
            var hold = root.Q<Button>("cmd-hold-line");
            Assert.IsNotNull(hold);
            Assert.IsTrue(hold.enabledSelf);
            hud.TacticalCommandBar.IssueCommand(0);
            yield return null;
            Assert.AreEqual(1, commandClicks, "tactical command click must issue once");

            hud.QuestlineProgressTracker.BindDocument(doc);
            hud.QuestlineProgressTracker.ShowQuest(
                "deliver_letter", "Deliver the letter",
                new[] { "Find the office", "Hand it over", "Return" }, 1);
            yield return null;
            Assert.AreEqual("Deliver the letter", root.Q<Label>("quest-title").text);

            hud.LoreCodexPanel.BindDocument(doc);
            hud.LoreCodexPanel.Show();
            hud.FactionRelationshipMap.BindDocument(doc);
            hud.FactionRelationshipMap.Show();
            hud.VehicleStatusPanel.BindDocument(doc);
            hud.VehicleStatusPanel.ShowVehicle("Scavenger cart", 0.6f, 8f, 20f, 12f, 40f);
            hud.CharacterArcProgressPanel.BindDocument(doc);
            hud.CharacterArcProgressPanel.ShowCharacter(
                "sv_test", "Elena", "nurse", "Keeps the ward lit.",
                new[] { "steady_hands" }, 1, "");
            hud.ItemConditionBadge.BindDocument(doc);
            hud.ItemConditionBadge.SetCondition(0.2f);
            hud.FactionIntelligencePanel.BindDocument(doc);
            hud.FactionIntelligencePanel.SetFaction("ash_militia", "Ash Militia", 12f, false);
            hud.QuestlineStageTracker.BindDocument(doc);
            hud.QuestlineStageTracker.AddQuest("deliver_letter", "Deliver the letter", 1, 3, "Find the office", 0.4f);
            yield return null;

            Assert.AreNotEqual(DisplayStyle.None, root.Q("lore-codex-panel").resolvedStyle.display);
            Assert.AreNotEqual(DisplayStyle.None, root.Q("faction-relationship-map").resolvedStyle.display);
            Assert.AreNotEqual(DisplayStyle.None, root.Q("vehicle-status-panel").resolvedStyle.display);
            Assert.AreEqual("Elena", root.Q<Label>("character-name").text);
            Assert.AreEqual("DAMAGED", root.Q<Label>("condition-text").text);
            Assert.AreEqual("Ash Militia", root.Q("faction-intelligence-panel").Q<Label>("faction-name").text);
            Assert.IsNotNull(root.Q("quest-deliver_letter"));

            hud.KeepsakeSlotUi.BindDocument(doc);
            FocusPhase11(hud, "sv_test");
            hud.KeepsakeSlotUi.SetKeepsake("sv_test", "family_photograph", false, 0f);
            hud.MemorialWallUi.BindDocument(doc);
            hud.MemorialWallUi.AddEntry(new MemorialEntry
            {
                SurvivorId = "sv_fallen",
                DisplayName = "Marcus Reed",
                DeathDay = 4,
                HasDogTag = true
            });
            hud.MemorialWallUi.Show();
            hud.AddictionDetoxIndicator.BindDocument(doc);
            hud.AddictionDetoxIndicator.ShowWithdrawal("sv_test");
            yield return null;

            Assert.IsFalse(root.Q("keepsake-slot-root").ClassListContains("hidden"));
            Assert.IsFalse(root.Q("memorial-wall-root").ClassListContains("hidden"));
            Assert.Greater(root.Q("memorial-entry-list").childCount, 0);
            Assert.IsFalse(root.Q("addiction-detox-root").ClassListContains("hidden"));
        }

        [UnityTest]
        public IEnumerator Phase11Widgets_CaptureRestore_RoundTrip()
        {
            var hud = FindHud();
            yield return null;

            FocusPhase11(hud, "sv_round");
            hud.RadiationPhaseIndicator.SetPhase("sv_round", RadiationSicknessPhase.ManifestIllness);
            hud.HypervigilanceIndicator.UpdateLevel("sv_round", 0.62f);
            hud.MoralBranchDisplay.SetBranch("sv_round", MoralBranchDirection.NumbedResilience);
            hud.KeepsakeSlotUi.SetKeepsake("sv_round", "worn_stethoscope", true, 0.4f);
            hud.TerminalPrognosisBanner.Show("sv_round", 2.5f, "deliver_letter");
            hud.AddictionDetoxIndicator.ShowDependency(
                "sv_round", "morphine_ampoule", AddictionDetoxIndicator.DetoxState.Withdrawal);
            hud.PhantomMemoryVignette.Trigger("Elena", "The ward lights flicker.", false);
            yield return null;

            var rad = hud.RadiationPhaseIndicator.CaptureState();
            var vig = hud.HypervigilanceIndicator.CaptureState();
            var moral = hud.MoralBranchDisplay.CaptureState();
            var keep = hud.KeepsakeSlotUi.CaptureState();
            var term = hud.TerminalPrognosisBanner.CaptureState();
            var detox = hud.AddictionDetoxIndicator.CaptureState();
            var phantom = hud.PhantomMemoryVignette.CaptureState();

            hud.RadiationPhaseIndicator.SetPhase("sv_round", RadiationSicknessPhase.Healthy);
            hud.HypervigilanceIndicator.UpdateLevel("sv_round", 0f);
            hud.MoralBranchDisplay.SetBranch("sv_round", MoralBranchDirection.Neutral);
            hud.KeepsakeSlotUi.ClearKeepsake("sv_round");
            hud.AddictionDetoxIndicator.Hide("sv_round");
            yield return null;

            hud.RadiationPhaseIndicator.RestoreState(rad);
            hud.HypervigilanceIndicator.RestoreState(vig);
            hud.MoralBranchDisplay.RestoreState(moral);
            hud.KeepsakeSlotUi.RestoreState(keep);
            hud.TerminalPrognosisBanner.RestoreState(term);
            hud.AddictionDetoxIndicator.RestoreState(detox);
            hud.PhantomMemoryVignette.RestoreState(phantom);
            yield return null;

            Assert.AreEqual(RadiationSicknessPhase.ManifestIllness,
                hud.RadiationPhaseIndicator.CaptureState().focusedPhase);
            Assert.AreEqual(0.62f, hud.HypervigilanceIndicator.CaptureState().focusedLevel, 0.01f);
            Assert.AreEqual(MoralBranchDirection.NumbedResilience,
                hud.MoralBranchDisplay.CaptureState().focusedBranch);
            Assert.AreEqual("worn_stethoscope", hud.KeepsakeSlotUi.CaptureState().focused.itemId);
            Assert.AreEqual(2.5f, hud.TerminalPrognosisBanner.CaptureState().focused.daysRemaining, 0.01f);
            Assert.AreEqual(AddictionDetoxIndicator.DetoxState.Withdrawal,
                hud.AddictionDetoxIndicator.CaptureState().focused.state);
            Assert.IsFalse(hud.PhantomMemoryVignette.CaptureState().isMotivation);
        }

        [UnityTest]
        public IEnumerator SaveLoad_RestoresPhase11HudFromSurvivorState()
        {
            var hud = FindHud();
            var bootstrap = Object.FindAnyObjectByType<GameBootstrap>();
            Assert.IsNotNull(bootstrap, "Gameplay scene must contain GameBootstrap");
            Assert.IsNotNull(bootstrap.Survivors);
            Assert.Greater(bootstrap.Survivors.Count, 0);
            yield return null;

            var sv = bootstrap.Survivors[0];
            sv.SicknessPhase = RadiationSicknessPhase.Prodromal;
            sv.PrognosisStage = PrognosisStage.Prodromal;
            sv.HypervigilanceLevel = 0.55f;
            sv.BranchDirection = MoralBranchDirection.BurdenedCompassion;
            sv.PersonalKeepsakeItemId = "family_photograph";
            sv.HasLostKeepsake = false;
            sv.KeepsakeGriefLevel = 0.1f;
            sv.HasTerminalPrognosis = true;
            sv.TerminalPrognosisDaysRemaining = 4f;
            sv.Needs.Hunger = 41f;
            sv.QuestStage = 2;

            FocusPhase11(hud, sv.Id);
            hud.RadiationPhaseIndicator.SetPhase(sv.Id, sv.SicknessPhase);
            hud.HypervigilanceIndicator.UpdateLevel(sv.Id, sv.HypervigilanceLevel);
            hud.MoralBranchDisplay.SetBranch(sv.Id, sv.BranchDirection);
            hud.KeepsakeSlotUi.SetKeepsake(sv.Id, sv.PersonalKeepsakeItemId, sv.HasLostKeepsake, sv.KeepsakeGriefLevel);
            hud.TerminalPrognosisBanner.Show(sv.Id, sv.TerminalPrognosisDaysRemaining, "");
            yield return null;

            const string slot = "wave_b_hud_roundtrip";
            bootstrap.SaveGame(slot);

            sv.SicknessPhase = RadiationSicknessPhase.Healthy;
            sv.PrognosisStage = PrognosisStage.Healthy;
            sv.HypervigilanceLevel = 0f;
            sv.BranchDirection = MoralBranchDirection.Neutral;
            sv.PersonalKeepsakeItemId = "";
            sv.HasTerminalPrognosis = false;
            sv.Needs.Hunger = 90f;
            sv.QuestStage = 0;
            hud.RadiationPhaseIndicator.SetPhase(sv.Id, RadiationSicknessPhase.Healthy);
            hud.HypervigilanceIndicator.UpdateLevel(sv.Id, 0f);
            hud.MoralBranchDisplay.SetBranch(sv.Id, MoralBranchDirection.Neutral);
            yield return null;

            Assert.IsTrue(bootstrap.LoadGame(slot), "LoadGame must restore the wave B slot");
            yield return null;
            yield return null;

            var restored = bootstrap.Survivors[0];
            Assert.AreEqual(PrognosisStage.Prodromal, restored.PrognosisStage);
            Assert.AreEqual(RadiationSicknessPhase.Prodromal, restored.SicknessPhase);
            Assert.AreEqual(0.55f, restored.HypervigilanceLevel, 0.01f);
            Assert.AreEqual(MoralBranchDirection.BurdenedCompassion, restored.BranchDirection);
            Assert.AreEqual("family_photograph", restored.PersonalKeepsakeItemId);
            Assert.AreEqual(4f, restored.TerminalPrognosisDaysRemaining, 0.01f);
            Assert.AreEqual(41f, restored.Needs.Hunger, 0.5f);
            Assert.AreEqual(2, restored.QuestStage);

            Assert.AreEqual(RadiationSicknessPhase.Prodromal,
                hud.RadiationPhaseIndicator.CaptureState().focusedPhase);
            Assert.AreEqual(0.55f, hud.HypervigilanceIndicator.CaptureState().focusedLevel, 0.01f);
            Assert.AreEqual(MoralBranchDirection.BurdenedCompassion,
                hud.MoralBranchDisplay.CaptureState().focusedBranch);
            Assert.AreEqual("family_photograph", hud.KeepsakeSlotUi.CaptureState().focused.itemId);
            Assert.AreEqual(4f, hud.TerminalPrognosisBanner.CaptureState().focused.daysRemaining, 0.01f);
        }
    }
}
