using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class Phase11UiWidgetTests
    {
        [Test]
        public void RadiationPhaseIndicator_AllPhases_SetCorrectColor()
        {
            var healthy = RadiationPhaseIndicator.GetColorForPhase(RadiationSicknessPhase.Healthy);
            var prodromal = RadiationPhaseIndicator.GetColorForPhase(RadiationSicknessPhase.Prodromal);
            var manifest = RadiationPhaseIndicator.GetColorForPhase(RadiationSicknessPhase.ManifestIllness);

            Assert.AreEqual(new Color(0.298f, 0.686f, 0.314f), healthy);
            Assert.AreEqual(new Color(1f, 0.757f, 0.027f), prodromal);
            Assert.AreEqual(new Color(0.957f, 0.263f, 0.212f), manifest);
        }

        [Test]
        public void HypervigilanceIndicator_LevelChange_UpdatesVisibility()
        {
            var go = new GameObject("hyper");
            var widget = go.AddComponent<HypervigilanceIndicator>();
            widget.UpdateLevel("sv_1", 0.2f);
            var captureLow = widget.CaptureState();
            Assert.AreEqual(0.2f, captureLow.focusedLevel, 0.01f);

            widget.UpdateLevel("sv_1", 0.5f);
            var captureMid = widget.CaptureState();
            Assert.AreEqual(0.5f, captureMid.focusedLevel, 0.01f);

            widget.UpdateLevel("sv_1", 0.8f);
            var captureHigh = widget.CaptureState();
            Assert.AreEqual(0.8f, captureHigh.focusedLevel, 0.01f);

            Object.DestroyImmediate(go);
        }

        [Test]
        public void MoralBranchDisplay_SetBranch_ShowsCorrectIcon()
        {
            var go = new GameObject("moral");
            var widget = go.AddComponent<MoralBranchDisplay>();
            widget.SetBranch("sv_1", MoralBranchDirection.NumbedResilience);
            var numbed = widget.CaptureState();
            Assert.AreEqual(MoralBranchDirection.NumbedResilience, numbed.focusedBranch);

            widget.SetBranch("sv_1", MoralBranchDirection.BurdenedCompassion);
            var compassion = widget.CaptureState();
            Assert.AreEqual(MoralBranchDirection.BurdenedCompassion, compassion.focusedBranch);

            Object.DestroyImmediate(go);
        }

        [Test]
        public void KeepsakeSlotUI_LostKeepsake_ShowsCrackedOverlay()
        {
            var go = new GameObject("keepsake");
            var widget = go.AddComponent<KeepsakeSlotUI>();
            widget.SetKeepsake("sv_1", "wedding_ring", true, 0.75f);
            var state = widget.CaptureState();
            Assert.IsTrue(state.focused.lost);
            Assert.AreEqual(0.75f, state.focused.griefLevel, 0.01f);

            Object.DestroyImmediate(go);
        }

        [Test]
        public void MemorialWallUI_AddEntry_RendersNamePlate()
        {
            var go = new GameObject("memorial");
            var widget = go.AddComponent<MemorialWallUI>();
            widget.AddEntry(new MemorialEntry
            {
                SurvivorId = "sv_dead",
                DisplayName = "Marcus",
                DeathDay = 42,
                HasDogTag = true
            });
            var state = widget.CaptureState();
            Assert.AreEqual(1, state.entryCount);

            Object.DestroyImmediate(go);
        }

        [Test]
        public void TerminalPrognosisBanner_Show_CountdownVisible()
        {
            var go = new GameObject("terminal");
            var widget = go.AddComponent<TerminalPrognosisBanner>();
            widget.Show("sv_1", 5f, "retrieve_heirloom");
            var state = widget.CaptureState();
            Assert.AreEqual("sv_1", state.focusedSurvivorId);
            Assert.AreEqual(5f, state.focused.daysRemaining, 0.01f);

            Object.DestroyImmediate(go);
        }

        [Test]
        public void AddictionDetoxIndicator_Withdrawal_ShowsShakingIcon()
        {
            var go = new GameObject("addiction");
            var widget = go.AddComponent<AddictionDetoxIndicator>();
            widget.ShowDependency("sv_1", "item_opioid_painkillers",
                AddictionDetoxIndicator.DetoxState.Withdrawal);
            var state = widget.CaptureState();
            Assert.AreEqual(AddictionDetoxIndicator.DetoxState.Withdrawal, state.focused.state);

            Object.DestroyImmediate(go);
        }

        [Test]
        public void PhantomMemoryVignette_Trigger_FiresAnimation()
        {
            var go = new GameObject("phantom");
            var widget = go.AddComponent<PhantomMemoryVignette>();
            widget.Trigger("Elena", "A memory surfaces.", true);
            var mot = widget.CaptureState();
            Assert.IsTrue(mot.isMotivation);

            widget.Trigger("Elena", "A memory surfaces.", false);
            var breakdown = widget.CaptureState();
            Assert.IsFalse(breakdown.isMotivation);

            Object.DestroyImmediate(go);
        }

        [Test]
        public void Phase11Widgets_BindDocument_AcceptsNullSafe()
        {
            var go = new GameObject("phase11_bind");
            go.AddComponent<RadiationPhaseIndicator>().BindDocument(null);
            go.AddComponent<PhantomMemoryVignette>().BindDocument(null);
            go.AddComponent<HypervigilanceIndicator>().BindDocument(null);
            go.AddComponent<MoralBranchDisplay>().BindDocument(null);
            go.AddComponent<KeepsakeSlotUI>().BindDocument(null);
            go.AddComponent<MemorialWallUI>().BindDocument(null);
            go.AddComponent<TerminalPrognosisBanner>().BindDocument(null);
            go.AddComponent<AddictionDetoxIndicator>().BindDocument(null);
            Assert.Pass();
            Object.DestroyImmediate(go);
        }

        [Test]
        public void PlanApiAliases_TerminalAndAddictionAndKeepsake()
        {
            var go = new GameObject("aliases");
            var terminal = go.AddComponent<TerminalPrognosisBanner>();
            terminal.Show("sv_1", 3f, "wish_a");
            terminal.MarkWishCompleted("sv_1");
            Assert.AreEqual(TerminalPrognosisBanner.WishOutcome.Completed, terminal.CaptureState().focused.outcome);
            terminal.MarkWishFailed("sv_1");
            Assert.AreEqual(TerminalPrognosisBanner.WishOutcome.Failed, terminal.CaptureState().focused.outcome);

            var detox = go.AddComponent<AddictionDetoxIndicator>();
            detox.ShowWithdrawal("sv_1");
            Assert.AreEqual(AddictionDetoxIndicator.DetoxState.Withdrawal, detox.CaptureState().focused.state);
            detox.Hide("sv_1");
            Assert.AreEqual(AddictionDetoxIndicator.DetoxState.Clean, detox.CaptureState().focused.state);

            var keepsake = go.AddComponent<KeepsakeSlotUI>();
            keepsake.SetKeepsake("sv_1", "ring", true, 0.5f);
            keepsake.ClearKeepsake("sv_1");
            Assert.IsTrue(string.IsNullOrEmpty(keepsake.CaptureState().focused.itemId));

            Object.DestroyImmediate(go);
        }
    }
}
