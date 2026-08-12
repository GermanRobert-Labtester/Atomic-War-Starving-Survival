using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
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

            hud.HypervigilanceIndicator.UpdateLevel("sv_test", 0.55f);
            yield return null;
            Assert.AreEqual(0.55f, hud.HypervigilanceIndicator.CaptureState().focusedLevel, 0.01f);
        }

        [UnityTest]
        public IEnumerator MoralBranchDisplay_SetBranch_Persists()
        {
            var hud = FindHud();
            yield return null;

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

            hud.TerminalPrognosisBanner.Show("sv_test", 3f, "deliver_letter");
            yield return null;
            Assert.AreEqual(3f, hud.TerminalPrognosisBanner.CaptureState().focused.daysRemaining, 0.01f);
        }
    }
}
