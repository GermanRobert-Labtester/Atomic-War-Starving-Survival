using System.Collections.Generic;
using AtomicWar._Game.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI.MainMenu
{
    /// <summary>
    /// Drives the LAST STATIC main menu: builds the rows from
    /// <see cref="MainMenuModel"/>, handles selection and keyboard/gamepad
    /// navigation, and dispatches the five actions.
    ///
    /// Split across partial files: this one owns lifecycle, the menu list and
    /// navigation; MainMenuController.Dialogs.cs owns the shared dialog shell
    /// and its focus trap; MainMenuController.Settings.cs owns the settings
    /// bindings.
    ///
    /// Replaces StartScreenController, whose only behaviour (Escape quits, and
    /// an unlocked cursor) is preserved here — Escape now opens the quit
    /// confirmation rather than killing the process without asking.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    [DisallowMultipleComponent]
    public sealed partial class MainMenuController : MonoBehaviour
    {
        [Header("Scene flow")]
        [Tooltip("Scene loaded by Continue and New Expedition. Must be in Build Settings.")]
        [SerializeField] private string _gameplaySceneName = "SampleScene";

        [Header("Continue")]
        [Tooltip("Slots probed for Continue. The most recently written one wins.")]
        [SerializeField] private string[] _continueSlotIds = { "autosave", "quicksave" };

        /// <summary>
        /// Panel-space width below which the menu column tightens up. USS has
        /// no @media, so the breakpoints are classes toggled from geometry.
        /// The panel scales from a 1920x1080 reference matched on height, so
        /// these are effectively aspect-ratio thresholds: 1500 is a little
        /// narrower than 4:3, 2200 a little wider than 2:1.
        /// </summary>
        private const float NarrowWidth = 1500f;
        private const float UltrawideWidth = 2200f;

        private UIDocument _document;
        private VisualElement _root;
        private VisualElement _shell;
        private VisualElement _menuList;

        private readonly List<Button> _rows = new List<Button>();
        private int _selectedIndex;

        /// <summary>Slot Continue will resume, or null when no save exists.</summary>
        private string _continueSlotId;

        private bool _built;

        // -----------------------------------------------------------------
        // Lifecycle
        // -----------------------------------------------------------------

        private void OnEnable()
        {
            // A menu the player must be able to click. Fully qualified:
            // UnityEngine.UIElements also defines a Cursor type.
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;

            _document = GetComponent<UIDocument>();
            Build();
        }

        /// <summary>
        /// UIDocument creates its visual tree in its own OnEnable. Script
        /// execution order between two components on the same GameObject is
        /// not guaranteed, so if we ran first there is no tree to query yet;
        /// Start always runs after every OnEnable, so it is a safe second
        /// chance. <see cref="_built"/> keeps this idempotent.
        /// </summary>
        private void Start()
        {
            if (!_built) Build();
        }

        private void OnDisable()
        {
            if (_root != null)
            {
                _root.UnregisterCallback<NavigationMoveEvent>(OnNavigationMove, TrickleDown.TrickleDown);
                _root.UnregisterCallback<KeyDownEvent>(OnKeyDown, TrickleDown.TrickleDown);
            }
            if (_shell != null)
            {
                _shell.UnregisterCallback<GeometryChangedEvent>(OnShellGeometryChanged);
            }

            UnbindSettings();
            _rows.Clear();
            _animationsArmed = false;
            _sceneImage = null;
            _root = null;
            _shell = null;
            _menuList = null;
            _built = false;
        }

        private void Build()
        {
            _root = _document != null ? _document.rootVisualElement : null;
            if (_root == null) return;

            _shell = _root.Q<VisualElement>("game-shell");
            _menuList = _root.Q<VisualElement>("menu-list");
            if (_shell == null || _menuList == null)
            {
                Debug.LogError(
                    "[MainMenuController] MainMenu.uxml is missing #game-shell or #menu-list. " +
                    "Check the UIDocument's Source Asset.");
                return;
            }

            _built = true;

            SetLabel("footer-credit", MainMenuModel.FooterCredit);

            _continueSlotId = ProbeContinueSlot();
            BuildRows();
            BuildDialogs();
            BindSettings();
            ArmAnimations();

            _root.RegisterCallback<NavigationMoveEvent>(OnNavigationMove, TrickleDown.TrickleDown);
            _root.RegisterCallback<KeyDownEvent>(OnKeyDown, TrickleDown.TrickleDown);
            _shell.RegisterCallback<GeometryChangedEvent>(OnShellGeometryChanged);

            SelectFirstEnabled();
        }

        // -----------------------------------------------------------------
        // Menu list
        // -----------------------------------------------------------------

        /// <summary>
        /// Which slot Continue resumes, or null. Probed with plain file checks
        /// rather than through SaveSystem: SaveSystem needs a fully constructed
        /// world, which does not exist on the start screen.
        /// </summary>
        private string ProbeContinueSlot() =>
            SaveSlotPaths.NewestExistingSlot(SaveSlotPaths.DefaultSavesDir, _continueSlotIds);

        private void BuildRows()
        {
            _menuList.Clear();
            _rows.Clear();

            for (int i = 0; i < MainMenuModel.Entries.Length; i++)
            {
                MainMenuModel.Entry entry = MainMenuModel.Entries[i];
                bool isContinue = entry.Id == MainMenuModel.EntryId.Continue;
                string detail = isContinue ? MainMenuModel.ContinueDetail(_continueSlotId) : entry.Detail;

                Button row = CreateRow(i, entry.Label, detail);
                if (isContinue) row.SetEnabled(_continueSlotId != null);

                MainMenuModel.EntryId id = entry.Id;
                row.clicked += () => Activate(id);

                // Clicking or tabbing to a row must move the highlight too, or
                // the arrow keys would then jump back to wherever the keyboard
                // selection had been left.
                int index = i;
                row.RegisterCallback<FocusInEvent>(_ => SetSelected(index, focus: false));

                _menuList.Add(row);
                _rows.Add(row);
            }
        }

        private static Button CreateRow(int index, string label, string detail)
        {
            var row = new Button { name = "menu-" + index, text = string.Empty };
            row.AddToClassList("menu-button");

            var inner = new VisualElement();
            inner.AddToClassList("menu-button__inner");
            inner.pickingMode = PickingMode.Ignore;

            var glow = new VisualElement();
            glow.AddToClassList("menu-button__glow");
            glow.pickingMode = PickingMode.Ignore;
            inner.Add(glow);

            inner.Add(MakeLabel(MainMenuModel.IndexLabel(index), "menu-index"));

            var labelColumn = new VisualElement();
            labelColumn.AddToClassList("menu-label");
            labelColumn.pickingMode = PickingMode.Ignore;
            labelColumn.Add(MakeLabel(label, "menu-label__main"));
            labelColumn.Add(MakeLabel(detail, "menu-label__detail"));
            inner.Add(labelColumn);

            inner.Add(MakeLabel("▶", "menu-arrow"));

            row.Add(inner);
            return row;
        }

        private static Label MakeLabel(string text, string className)
        {
            var label = new Label(text) { pickingMode = PickingMode.Ignore };
            label.AddToClassList(className);
            return label;
        }

        // -----------------------------------------------------------------
        // Selection & navigation
        // -----------------------------------------------------------------

        private void SelectFirstEnabled()
        {
            for (int i = 0; i < _rows.Count; i++)
            {
                if (!_rows[i].enabledSelf) continue;
                SetSelected(i, focus: true);
                return;
            }
        }

        private void SetSelected(int index, bool focus)
        {
            if (index < 0 || index >= _rows.Count) return;

            for (int i = 0; i < _rows.Count; i++)
            {
                _rows[i].EnableInClassList("is-selected", i == index);
            }

            _selectedIndex = index;
            if (focus) _rows[index].Focus();
        }

        /// <summary>
        /// Step the highlight, skipping rows that are disabled so a save-less
        /// Continue is passed over instead of trapping the cursor on a row that
        /// cannot be activated.
        /// </summary>
        private void MoveSelection(int delta)
        {
            if (_rows.Count == 0) return;

            int index = _selectedIndex;
            for (int step = 0; step < _rows.Count; step++)
            {
                index = (index + delta + _rows.Count) % _rows.Count;
                if (!_rows[index].enabledSelf) continue;
                SetSelected(index, focus: true);
                return;
            }
        }

        /// <summary>
        /// Arrow keys and gamepad D-pad/stick both arrive as NavigationMoveEvent,
        /// so one handler covers both. Taken in the trickle-down phase because
        /// the focused Button would otherwise consume it first and move focus
        /// itself, bypassing the disabled-row skip.
        /// </summary>
        private void OnNavigationMove(NavigationMoveEvent evt)
        {
            if (IsDialogOpen) return;

            if (evt.direction == NavigationMoveEvent.Direction.Down) MoveSelection(1);
            else if (evt.direction == NavigationMoveEvent.Direction.Up) MoveSelection(-1);
            else return;

            evt.StopPropagation();
        }

        private void OnKeyDown(KeyDownEvent evt)
        {
            if (IsDialogOpen)
            {
                if (evt.keyCode == KeyCode.Escape)
                {
                    CloseDialog();
                    evt.StopPropagation();
                }
                return;
            }

            switch (evt.keyCode)
            {
                case KeyCode.S:
                    MoveSelection(1);
                    break;
                case KeyCode.W:
                    MoveSelection(-1);
                    break;
                case KeyCode.Escape:
                    OpenDialog(DialogKind.Quit);
                    break;
                default:
                    return;
            }

            evt.StopPropagation();
        }

        // Enter and Space need no handling: Button turns NavigationSubmitEvent
        // into its own click, which is already wired to Activate.

        // -----------------------------------------------------------------
        // Actions
        // -----------------------------------------------------------------

        private void Activate(MainMenuModel.EntryId id)
        {
            switch (id)
            {
                case MainMenuModel.EntryId.Continue:
                    ContinueGame();
                    break;
                case MainMenuModel.EntryId.NewExpedition:
                    OpenDialog(DialogKind.NewExpedition);
                    break;
                case MainMenuModel.EntryId.Settings:
                    OpenDialog(DialogKind.Settings);
                    break;
                case MainMenuModel.EntryId.Credits:
                    OpenDialog(DialogKind.Credits);
                    break;
                case MainMenuModel.EntryId.Exit:
                    OpenDialog(DialogKind.Quit);
                    break;
            }
        }

        private void ContinueGame()
        {
            if (_continueSlotId == null) return;
            PendingGameLoad.SlotId = _continueSlotId;
            LoadGameplayScene();
        }

        private void StartNewExpedition(ExpeditionDifficulty difficulty)
        {
            // Clear first: a stale pending slot would make "new game" silently
            // resume a save instead.
            PendingGameLoad.Clear();
            PendingGameLoad.Difficulty = difficulty;
            LoadGameplayScene();
        }

        private void LoadGameplayScene()
        {
            if (string.IsNullOrEmpty(_gameplaySceneName))
            {
                Debug.LogError("[MainMenuController] No gameplay scene name configured.");
                return;
            }
            SceneManager.LoadScene(_gameplaySceneName);
        }

        /// <summary>Leave the application. Stops play mode instead in the Editor.</summary>
        private void Quit()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        // -----------------------------------------------------------------
        // Responsive classes
        // -----------------------------------------------------------------

        private void OnShellGeometryChanged(GeometryChangedEvent evt)
        {
            float width = evt.newRect.width;
            _shell.EnableInClassList("narrow", width < NarrowWidth);
            _shell.EnableInClassList("ultrawide", width > UltrawideWidth);
        }

        // -----------------------------------------------------------------

        private void SetLabel(string elementName, string text)
        {
            var label = _root.Q<Label>(elementName);
            if (label != null) label.text = text;
        }
    }
}
