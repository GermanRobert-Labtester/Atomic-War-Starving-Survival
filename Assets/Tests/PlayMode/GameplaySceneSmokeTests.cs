using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using AtomicWar._Game.Core;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.PlayMode
{
    /// <summary>
    /// Loads the real gameplay scene and asserts the simulation actually runs.
    ///
    /// Every other test in the suite constructs systems directly in C# and never
    /// loads a scene, so all ~1,100 of them stayed green while the shipped player
    /// booted into an empty SampleScene. This fixture is the only thing that can
    /// catch a broken scene or a null Inspector reference.
    /// </summary>
    [TestFixture]
    public class GameplaySceneSmokeTests
    {
        const string SceneName = "Gameplay";

        [UnitySetUp]
        public IEnumerator LoadGameplayScene()
        {
            yield return SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);
            yield return null; // let Awake/Start run
        }

        static GameBootstrap Bootstrap()
        {
            var bootstrap = Object.FindAnyObjectByType<GameBootstrap>();
            Assert.IsNotNull(bootstrap, "Gameplay scene must contain a GameBootstrap");
            return bootstrap;
        }

        [UnityTest]
        public IEnumerator Scene_BootsWithItsCoreSystemsConstructed()
        {
            var bootstrap = Bootstrap();

            Assert.IsNotNull(bootstrap.TimeSystem,  "TimeSystem should be constructed in Awake");
            Assert.IsNotNull(bootstrap.NeedsSystem, "NeedsSystem should be constructed in Awake");
            Assert.IsNotNull(bootstrap.SaveSystem,  "SaveSystem should be constructed in Awake");
            Assert.IsNotNull(Object.FindAnyObjectByType<HUD>(), "Gameplay scene must contain a HUD");

            Assert.IsNotNull(bootstrap.Survivors, "Survivors list should exist");
            Assert.Greater(bootstrap.Survivors.Count, 0, "a new game should start with survivors");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Scene_ConstructsTheSystemsBuiltFromCatalogs()
        {
            var bootstrap = Bootstrap();

            // GameBootstrap exposes no public catalog accessor, so assert on the
            // systems built from them instead: these stay null if Awake bailed
            // partway through initialization.
            Assert.IsNotNull(bootstrap.CraftingSystem,   "CraftingSystem is built from the recipe catalog");
            Assert.IsNotNull(bootstrap.EventRunner,      "EventRunner is built from the event catalog");
            Assert.IsNotNull(bootstrap.ScavengingSystem, "ScavengingSystem is built from the location catalog");

            yield return null;
        }

        /// <summary>
        /// PlayerInputHandler was fully implemented, covered by EditMode tests that
        /// AddComponent it onto a throwaway GameObject, and present in no scene at
        /// all -- so no key the player pressed reached the simulation. Its Awake
        /// does GetComponent&lt;GameBootstrap&gt;(), so being in the scene is not
        /// enough: it has to be on the bootstrap's own GameObject.
        /// </summary>
        [UnityTest]
        public IEnumerator Input_IsWiredToTheBootstrapItDrives()
        {
            var bootstrap = Bootstrap();

            var input = Object.FindAnyObjectByType<PlayerInputHandler>();
            Assert.IsNotNull(input, "Gameplay scene must contain a PlayerInputHandler");

            Assert.AreSame(bootstrap.gameObject, input.gameObject,
                "PlayerInputHandler resolves its bootstrap with GetComponent, so it " +
                "must share the GameBootstrap GameObject");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Clock_AdvancesOverFrames()
        {
            var bootstrap = Bootstrap();
            float before = bootstrap.TimeSystem.TotalElapsedHours;

            for (int i = 0; i < 120; i++)
                yield return null;

            Assert.Greater(bootstrap.TimeSystem.TotalElapsedHours, before,
                "the clock must advance while the scene is playing");
        }

        [UnityTest]
        public IEnumerator Needs_DecayOverFrames()
        {
            var bootstrap = Bootstrap();
            var survivor = bootstrap.Survivors[0];
            float thirstBefore = survivor.Needs.Thirst;

            for (int i = 0; i < 120; i++)
                yield return null;

            Assert.Greater(survivor.Needs.Thirst, thirstBefore,
                "thirst accumulates upward as time passes");
        }

        /// <summary>
        /// The vitals panel is the only on-screen report of the core loop. Its
        /// repaint hangs off need/dose events rather than the discrete actions
        /// that drive the other panels, so this asserts the labels actually hold
        /// live values after some frames -- a panel wired but never repainted
        /// looks identical to one that works, until you watch it.
        /// </summary>
        [UnityTest]
        public IEnumerator Vitals_ShowLiveValuesAfterFrames()
        {
            var hud = Object.FindAnyObjectByType<HUD>();
            Assert.IsNotNull(hud, "Gameplay scene must contain a HUD");

            for (int i = 0; i < 120; i++)
                yield return null;

            var view = hud.DiegeticHud != null ? hud.DiegeticHud.View : null;
            Assert.IsNotNull(view, "diegetic view should exist");
            Assert.IsNotNull(view.VitalsClock, "vitals clock label should be bound");

            // Not asserted on the clock: the UXML ships "DAY 1   00:00" as
            // placeholder text, so a "DAY" check passes even when nothing ever
            // repaints. The dose label is empty in the UXML and the rows are
            // built at paint time, so both can only hold after a real paint.
            Assert.IsNotEmpty(view.VitalsDose.text,
                "dose label is empty in the UXML, so text here proves a real paint");
            Assert.AreEqual(DiegeticHudView.CoreNeedIds.Length, view.VitalsNeeds.childCount,
                "one row per core need should be painted");

            // The dose label is only useful if it holds the DosimeterHUD's
            // current cumulative dose, not the pre-init zero. "0.00 Sv" was
            // the symptom of a wiring bug that the IsNotEmpty check above
            // silently passed, so we assert the value matches the live HUD.
            // We compare against the DosimeterHUD (not the survivor) because
            // the HUD is what the vitals panel mirrors; if those drift, this
            // test fires.
            var dosimeter = hud.DosimeterHUD;
            Assert.IsNotNull(dosimeter, "HUD must hold a DosimeterHUD widget");
            string expectedDose = dosimeter.CumulativeDose.ToString("0.00");
            StringAssert.Contains(expectedDose, view.VitalsDose.text,
                $"dose label should reflect DosimeterHUD.CumulativeDose ({expectedDose} Sv); " +
                "0.00 here means the first RepaintVitals ran before the dose was pushed");
        }

        /// <summary>
        /// No event is guaranteed to fire inside a smoke test, so this asserts
        /// the panel is bound and that its visibility tracks EventModalUI.IsOpen
        /// -- the wiring, without depending on the event schedule.
        /// </summary>
        [UnityTest]
        public IEnumerator EventPanel_VisibilityTracksTheModalState()
        {
            var hud = Object.FindAnyObjectByType<HUD>();
            Assert.IsNotNull(hud, "Gameplay scene must contain a HUD");

            for (int i = 0; i < 30; i++)
                yield return null;

            var view = hud.DiegeticHud != null ? hud.DiegeticHud.View : null;
            Assert.IsNotNull(view, "diegetic view should exist");
            Assert.IsNotNull(view.EventPanel, "event panel should be bound from the UXML");

            bool modalOpen = hud.EventModalUI != null && hud.EventModalUI.IsOpen;
            Assert.AreEqual(
                modalOpen ? UnityEngine.UIElements.DisplayStyle.Flex
                          : UnityEngine.UIElements.DisplayStyle.None,
                view.EventPanel.style.display.value,
                "panel visibility must track EventModalUI.IsOpen");
        }

        [UnityTest]
        public IEnumerator SaveAndLoad_RoundTripsClockAndNeeds()
        {
            var bootstrap = Bootstrap();

            for (int i = 0; i < 60; i++)
                yield return null;

            Assert.IsTrue(bootstrap.SaveSystem.Save("smoke_test"), "save should succeed");

            int dayAtSave = bootstrap.TimeSystem.CurrentDay;
            float hoursAtSave = bootstrap.TimeSystem.TotalElapsedHours;
            float thirstAtSave = bootstrap.Survivors[0].Needs.Thirst;

            for (int i = 0; i < 60; i++)
                yield return null;

            Assert.IsTrue(bootstrap.SaveSystem.Load("smoke_test"), "load should succeed");

            Assert.AreEqual(dayAtSave, bootstrap.TimeSystem.CurrentDay);
            Assert.AreEqual(hoursAtSave, bootstrap.TimeSystem.TotalElapsedHours, 0.001f);
            Assert.AreEqual(thirstAtSave, bootstrap.Survivors[0].Needs.Thirst, 0.001f);

            bootstrap.SaveSystem.Delete("smoke_test");
        }
    }
}
