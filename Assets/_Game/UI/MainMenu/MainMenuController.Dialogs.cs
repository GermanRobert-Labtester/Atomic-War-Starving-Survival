using AtomicWar._Game.Utilities;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI.MainMenu
{
    /// <summary>
    /// The shared dialog shell: one backdrop and one panel reused by all three
    /// dialogs, so there is a single focus trap and a single set of transitions
    /// rather than three near-identical copies.
    /// </summary>
    public sealed partial class MainMenuController
    {
        private enum DialogKind
        {
            None,
            NewExpedition,
            Settings,
            Credits,
            Quit,
        }

        private VisualElement _dialogBackdrop;
        private VisualElement _dialogPanel;
        private Label _dialogEyebrow;
        private Label _dialogTitle;
        private Label _dialogBody;
        private Label _dialogConfirmLabel;
        private Label _dialogBackLabel;
        private Button _dialogConfirm;
        private Button _dialogBack;

        private VisualElement _difficultyRow;
        private Button _difficultyOperative;
        private Button _difficultyVeteran;

        private DialogKind _openDialog = DialogKind.None;
        private ExpeditionDifficulty _chosenDifficulty = ExpeditionDifficulty.Operative;

        private bool IsDialogOpen => _openDialog != DialogKind.None;

        private void BuildDialogs()
        {
            _dialogBackdrop = _root.Q<VisualElement>("dialog-backdrop");
            _dialogPanel = _root.Q<VisualElement>("dialog-panel");
            _dialogEyebrow = _root.Q<Label>("dialog-eyebrow");
            _dialogTitle = _root.Q<Label>("dialog-title");
            _dialogBody = _root.Q<Label>("dialog-body");
            _dialogConfirm = _root.Q<Button>("dialog-confirm");
            _dialogBack = _root.Q<Button>("dialog-back");
            _dialogConfirmLabel = _root.Q<Label>("dialog-confirm-label");
            _dialogBackLabel = _root.Q<Label>("dialog-back-label");

            _difficultyRow = _root.Q<VisualElement>("difficulty-row");
            _difficultyOperative = _root.Q<Button>("difficulty-operative");
            _difficultyVeteran = _root.Q<Button>("difficulty-veteran");

            if (_dialogBackdrop == null || _dialogPanel == null)
            {
                Debug.LogError("[MainMenuController] MainMenu.uxml is missing the dialog shell.");
                return;
            }

            // Click anywhere on the dimmed backdrop closes; clicks that land on
            // the panel itself must not bubble up and close it again.
            _dialogBackdrop.RegisterCallback<PointerDownEvent>(OnBackdropPointerDown);
            _dialogPanel.RegisterCallback<PointerDownEvent>(evt => evt.StopPropagation());

            if (_dialogBack != null) _dialogBack.clicked += CloseDialog;
            if (_dialogConfirm != null) _dialogConfirm.clicked += ConfirmDialog;

            SetLabelText(_root.Q<Label>("difficulty-operative-label"), MainMenuModel.DifficultyOperativeLabel);
            SetLabelText(_root.Q<Label>("difficulty-operative-detail"), MainMenuModel.DifficultyOperativeDetail);
            SetLabelText(_root.Q<Label>("difficulty-veteran-label"), MainMenuModel.DifficultyVeteranLabel);
            SetLabelText(_root.Q<Label>("difficulty-veteran-detail"), MainMenuModel.DifficultyVeteranDetail);

            if (_difficultyOperative != null)
                _difficultyOperative.clicked += () => SetDifficulty(ExpeditionDifficulty.Operative);
            if (_difficultyVeteran != null)
                _difficultyVeteran.clicked += () => SetDifficulty(ExpeditionDifficulty.Veteran);

            SetDifficulty(ExpeditionDifficulty.Operative);
            ApplyDialogVisibility(DialogKind.None);
        }

        private void OnBackdropPointerDown(PointerDownEvent evt)
        {
            CloseDialog();
            evt.StopPropagation();
        }

        private void OpenDialog(DialogKind kind)
        {
            if (kind == DialogKind.None || _dialogBackdrop == null) return;

            _openDialog = kind;
            MainMenuModel.DialogCopy copy = CopyFor(kind);

            SetLabelText(_dialogEyebrow, copy.Eyebrow);
            SetLabelText(_dialogTitle, copy.Title);
            SetLabelText(_dialogBody, copy.Body);
            SetLabelText(_dialogConfirmLabel, copy.Confirm);
            SetLabelText(_dialogBackLabel, copy.Back);

            if (kind == DialogKind.Settings) RefreshSettingsControls();

            ApplyDialogVisibility(kind);

            // Modal trap. The rows are made unfocusable rather than disabled:
            // SetEnabled(false) would grey them out through :disabled, which
            // reads as "these are gone" instead of "these are behind a dialog".
            SetMenuFocusable(false);
            _dialogBackdrop.AddToClassList("is-open");
            _dialogConfirm?.Focus();
        }

        private void CloseDialog()
        {
            if (!IsDialogOpen) return;

            _openDialog = DialogKind.None;
            _dialogBackdrop.RemoveFromClassList("is-open");
            ApplyDialogVisibility(DialogKind.None);
            SetMenuFocusable(true);

            // Put the player back where they were, not at the top of the list.
            SetSelected(_selectedIndex, focus: true);
        }

        private void ConfirmDialog()
        {
            switch (_openDialog)
            {
                case DialogKind.NewExpedition:
                    StartNewExpedition(_chosenDifficulty);
                    return;
                case DialogKind.Quit:
                    Quit();
                    return;
                case DialogKind.Settings:
                    PersistSettings();
                    break;
            }

            CloseDialog();
        }

        private static MainMenuModel.DialogCopy CopyFor(DialogKind kind)
        {
            switch (kind)
            {
                case DialogKind.NewExpedition: return MainMenuModel.NewExpeditionDialog;
                case DialogKind.Settings: return MainMenuModel.SettingsDialog;
                case DialogKind.Credits: return MainMenuModel.CreditsDialog;
                default: return MainMenuModel.QuitDialog;
            }
        }

        /// <summary>
        /// Show only the extra block the current dialog needs. These use
        /// display rather than visibility on purpose: unlike the backdrop they
        /// are not transitioned, and they must not reserve layout space in the
        /// dialogs that do not use them.
        /// </summary>
        private void ApplyDialogVisibility(DialogKind kind)
        {
            SetDisplayed(_difficultyRow, kind == DialogKind.NewExpedition);
            SetDisplayed(_settingsBody, kind == DialogKind.Settings);
        }

        private void SetMenuFocusable(bool focusable)
        {
            for (int i = 0; i < _rows.Count; i++)
            {
                _rows[i].focusable = focusable;
            }
        }

        private void SetDifficulty(ExpeditionDifficulty difficulty)
        {
            _chosenDifficulty = difficulty;
            _difficultyOperative?.EnableInClassList("is-selected", difficulty == ExpeditionDifficulty.Operative);
            _difficultyVeteran?.EnableInClassList("is-selected", difficulty == ExpeditionDifficulty.Veteran);
        }

        private static void SetDisplayed(VisualElement element, bool displayed)
        {
            if (element == null) return;
            element.style.display = displayed ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private static void SetLabelText(Label label, string text)
        {
            if (label != null) label.text = text;
        }
    }
}
