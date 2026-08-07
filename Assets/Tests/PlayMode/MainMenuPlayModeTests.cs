using System.Collections;
using System.IO;
using AtomicWar._Game.UI.MainMenu;
using AtomicWar._Game.Utilities;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace AtomicWar.Tests.PlayMode
{
    /// <summary>
    /// The gate for the main menu: builds the real StartScreen scene, in play
    /// mode, and asserts the things that are invisible until something is on
    /// screen — that the UXML resolved, that the rows exist, that the fonts and
    /// theme actually bound, and that navigation steps over a disabled
    /// Continue instead of getting stuck on it.
    ///
    /// The menu instantiates a UIDocument, so these have to be play-mode tests;
    /// none of it exists as data an EditMode test could inspect.
    /// </summary>
    [TestFixture]
    public class MainMenuPlayModeTests
    {
        private const string ScenePath = "Assets/Scenes/StartScreen.unity";
        private const string UxmlPath = "Assets/_Game/UI/MainMenu/MainMenu.uxml";
        private const string PanelSettingsPath = "Assets/_Game/UI/MainMenu/MainMenuPanelSettings.asset";

        private GameObject _menuGo;
        private UIDocument _document;
        private MainMenuController _controller;

        [SetUp]
        public void SetUp()
        {
            PendingGameLoad.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            if (_menuGo != null) Object.DestroyImmediate(_menuGo);
            _menuGo = null;
            _document = null;
            _controller = null;
            PendingGameLoad.Clear();
        }

        /// <summary>
        /// Build the menu the same way the generated scene does, rather than
        /// loading the scene itself: the test runner's own scene is already
        /// loaded, and swapping it out mid-run fights the runner.
        /// </summary>
        private IEnumerator BuildMenu()
        {
#if UNITY_EDITOR
            var uxml = UnityEditor.AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath);
            var panelSettings = UnityEditor.AssetDatabase.LoadAssetAtPath<PanelSettings>(PanelSettingsPath);
            Assert.That(uxml, Is.Not.Null, $"Missing {UxmlPath}");
            Assert.That(panelSettings, Is.Not.Null, $"Missing {PanelSettingsPath}");

            _menuGo = new GameObject("MainMenu (test)");
            _menuGo.SetActive(false);
            _document = _menuGo.AddComponent<UIDocument>();
            _document.panelSettings = panelSettings;
            _document.visualTreeAsset = uxml;
            _controller = _menuGo.AddComponent<MainMenuController>();
            _menuGo.SetActive(true);

            // One frame for UIDocument to build the tree and for the panel to
            // resolve styles; transitions and layout are not needed here.
            yield return null;
            yield return null;
#else
            yield break;
#endif
        }

        private VisualElement Root => _document.rootVisualElement;

        [Test]
        public void GeneratedScene_HasNoEventSystem()
        {
            // An active EventSystem with no PanelRaycaster silently starves a
            // UI Toolkit panel of input, so its absence is load-bearing.
            Assert.That(File.Exists(ScenePath), Is.True, $"Missing {ScenePath}");
            string scene = File.ReadAllText(ScenePath);
            Assert.That(scene, Does.Not.Contain("EventSystem"));
            Assert.That(scene, Does.Contain("UIDocument"));
        }

        [UnityTest]
        public IEnumerator Menu_BuildsTheShellAndFiveRows()
        {
            yield return BuildMenu();

            Assert.That(Root.Q<VisualElement>("game-shell"), Is.Not.Null, "#game-shell");
            var list = Root.Q<VisualElement>("menu-list");
            Assert.That(list, Is.Not.Null, "#menu-list");
            Assert.That(list.childCount, Is.EqualTo(MainMenuModel.Entries.Length));
        }

        [UnityTest]
        public IEnumerator Menu_ResolvesTheCustomFont()
        {
            yield return BuildMenu();

            var label = Root.Q<Label>(className: "menu-label__main");
            Assert.That(label, Is.Not.Null, "no menu label was built");

            // If -unity-font-definition failed to resolve, this is null and the
            // menu silently falls back to the engine default font.
            Assert.That(label.resolvedStyle.unityFontDefinition.fontAsset, Is.Not.Null,
                "Barlow SDF font asset did not bind; check the USS url() paths.");
        }

        [UnityTest]
        public IEnumerator Menu_ContinueRowIsDisabledWhenNoSaveExists()
        {
            // Only meaningful when this machine has no save; otherwise the row
            // is legitimately enabled and there is nothing to assert.
            string existing = SaveSlotPaths.NewestExistingSlot(
                SaveSlotPaths.DefaultSavesDir, "autosave", "quicksave");

            yield return BuildMenu();

            var list = Root.Q<VisualElement>("menu-list");
            var continueRow = list[0] as Button;
            Assert.That(continueRow, Is.Not.Null);

            Assert.That(continueRow.enabledSelf, Is.EqualTo(existing != null),
                "Continue's enabled state must match whether a save file exists.");

            var detail = continueRow.Q<Label>(className: "menu-label__detail");
            Assert.That(detail.text, Is.EqualTo(MainMenuModel.ContinueDetail(existing)));
        }

        [UnityTest]
        public IEnumerator Menu_StartsWithAnEnabledRowSelected()
        {
            yield return BuildMenu();

            var list = Root.Q<VisualElement>("menu-list");
            VisualElement selected = null;
            for (int i = 0; i < list.childCount; i++)
            {
                if (list[i].ClassListContains("is-selected")) selected = list[i];
            }

            Assert.That(selected, Is.Not.Null, "nothing was selected on open");
            Assert.That(selected.enabledSelf, Is.True, "a disabled row must never start selected");
        }

        [UnityTest]
        public IEnumerator Dialogs_StartClosedAndOpenOnActivate()
        {
            yield return BuildMenu();

            var backdrop = Root.Q<VisualElement>("dialog-backdrop");
            Assert.That(backdrop, Is.Not.Null, "#dialog-backdrop");
            Assert.That(backdrop.ClassListContains("is-open"), Is.False, "dialog started open");

            // EXIT is the last row and always enabled.
            var list = Root.Q<VisualElement>("menu-list");
            var exitRow = list[list.childCount - 1] as Button;
            Assert.That(exitRow, Is.Not.Null);

            yield return Click(exitRow);

            Assert.That(backdrop.ClassListContains("is-open"), Is.True,
                "activating EXIT should open the quit dialog");
            Assert.That(Root.Q<Label>("dialog-title").text,
                Is.EqualTo(MainMenuModel.QuitDialog.Title));
        }

        [UnityTest]
        public IEnumerator SettingsDialog_HasItsThreeControls()
        {
            yield return BuildMenu();

            Assert.That(Root.Q<Slider>("setting-volume"), Is.Not.Null, "volume slider");
            Assert.That(Root.Q<Toggle>("setting-fullscreen"), Is.Not.Null, "fullscreen toggle");

            var dropdown = Root.Q<DropdownField>("setting-resolution");
            Assert.That(dropdown, Is.Not.Null, "resolution dropdown");
            Assert.That(dropdown.choices, Is.Not.Empty,
                "the resolution dropdown must never be empty, even headless");
        }

        [UnityTest]
        public IEnumerator Escape_ClosesAnOpenDialog()
        {
            yield return BuildMenu();

            var list = Root.Q<VisualElement>("menu-list");
            var exitRow = (Button)list[list.childCount - 1];
            yield return Click(exitRow);

            var backdrop = Root.Q<VisualElement>("dialog-backdrop");
            Assert.That(backdrop.ClassListContains("is-open"), Is.True, "dialog did not open");

            using (var evt = KeyDownEvent.GetPooled('\u001b', KeyCode.Escape, EventModifiers.None))
            {
                evt.target = Root;
                Root.SendEvent(evt);
            }
            yield return null;

            Assert.That(backdrop.ClassListContains("is-open"), Is.False,
                "Escape should close an already-open dialog rather than re-opening Quit");
        }

        [UnityTest]
        public IEnumerator OpenDialog_MakesMenuRowsUnfocusable_ClosingRestoresThem()
        {
            yield return BuildMenu();

            var list = Root.Q<VisualElement>("menu-list");
            var exitRow = (Button)list[list.childCount - 1];

            for (int i = 0; i < list.childCount; i++)
            {
                Assert.That(((Button)list[i]).focusable, Is.True, $"row {i} should start focusable");
            }

            yield return Click(exitRow);

            for (int i = 0; i < list.childCount; i++)
            {
                Assert.That(((Button)list[i]).focusable, Is.False,
                    $"row {i} must be unfocusable while a dialog is open, or Tab escapes the modal");
            }

            var dialogBack = Root.Q<Button>("dialog-back");
            yield return Click(dialogBack);

            for (int i = 0; i < list.childCount; i++)
            {
                Assert.That(((Button)list[i]).focusable, Is.True, $"row {i} should be focusable again after close");
            }
        }

        [UnityTest]
        public IEnumerator NewExpeditionDialog_DifficultyDefaultsToOperative_AndClickSwitchesSelection()
        {
            yield return BuildMenu();

            var list = Root.Q<VisualElement>("menu-list");
            var newExpeditionRow = (Button)list[1];
            yield return Click(newExpeditionRow);

            var operative = Root.Q<Button>("difficulty-operative");
            var veteran = Root.Q<Button>("difficulty-veteran");
            Assert.That(operative.ClassListContains("is-selected"), Is.True, "Operative should be the default");
            Assert.That(veteran.ClassListContains("is-selected"), Is.False);

            yield return Click(veteran);

            Assert.That(veteran.ClassListContains("is-selected"), Is.True, "clicking Veteran should select it");
            Assert.That(operative.ClassListContains("is-selected"), Is.False,
                "selecting Veteran must deselect Operative");
        }

        [UnityTest]
        public IEnumerator VolumeSlider_ChangingValue_AppliesToAudioListenerImmediately()
        {
            float originalVolume = AudioListener.volume;
            try
            {
                yield return BuildMenu();

                var list = Root.Q<VisualElement>("menu-list");
                var settingsRow = (Button)list[2];
                yield return Click(settingsRow);

                var slider = Root.Q<Slider>("setting-volume");
                Assert.That(slider, Is.Not.Null);

                slider.value = 0.3f;
                yield return null;

                Assert.That(AudioListener.volume, Is.EqualTo(0.3f).Within(0.001f),
                    "moving the slider should apply master volume live, with no Apply button");
            }
            finally
            {
                AudioListener.volume = originalVolume;
            }
        }

        /// <summary>
        /// Button.clicked is wired through the Clickable manipulator, which
        /// tracks a PointerDown/PointerUp pair rather than a raw ClickEvent —
        /// sending ClickEvent directly does not fire it. position has no
        /// public setter on the pooled event, so the click location comes
        /// from a raw UnityEngine.Event instead — the documented way to seed
        /// a pointer event with a screen position.
        /// </summary>
        private static IEnumerator Click(VisualElement element)
        {
            Vector2 center = element.worldBound.center;
            var downSource = new Event { type = EventType.MouseDown, mousePosition = center, button = 0 };
            using (var down = PointerDownEvent.GetPooled(downSource))
            {
                down.target = element;
                element.SendEvent(down);
            }
            var upSource = new Event { type = EventType.MouseUp, mousePosition = center, button = 0 };
            using (var up = PointerUpEvent.GetPooled(upSource))
            {
                up.target = element;
                element.SendEvent(up);
            }
            yield return null;
        }
    }
}
