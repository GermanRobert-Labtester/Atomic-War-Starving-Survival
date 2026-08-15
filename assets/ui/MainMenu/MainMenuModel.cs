using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.UI.MainMenu
{
    /// <summary>
    /// Every string and every menu entry the main menu shows.
    ///
    /// Kept free of UnityEngine so it can be asserted against directly in
    /// EditMode tests, and kept separate from the controller so that changing
    /// a label is a data edit rather than a change to focus-trap or
    /// scene-loading logic.
    /// </summary>
    public static class MainMenuModel
    {
        /// <summary>Which row was activated. Order here is display order.</summary>
        public enum EntryId
        {
            Continue,
            NewExpedition,
            Settings,
            Credits,
            Exit,
        }

        /// <summary>One row of the menu.</summary>
        public readonly struct Entry
        {
            public readonly EntryId Id;
            public readonly string Label;
            public readonly string Detail;

            public Entry(EntryId id, string label, string detail)
            {
                Id = id;
                Label = label;
                Detail = detail;
            }
        }

        /// <summary>
        /// The rows, in display order. Continue's detail line is a placeholder:
        /// the controller replaces it with <see cref="ContinueDetail"/> once it
        /// knows whether a save exists.
        /// </summary>
        public static readonly Entry[] Entries =
        {
            new Entry(EntryId.Continue, "CONTINUE", ContinueDetailNoSave),
            new Entry(EntryId.NewExpedition, "NEW EXPEDITION", "INITIALIZE A FRESH OPERATION"),
            new Entry(EntryId.Settings, "SETTINGS", "DISPLAY · AUDIO · RELAY"),
            new Entry(EntryId.Credits, "CREDITS", "THE PEOPLE BEHIND THE SIGNAL"),
            new Entry(EntryId.Exit, "EXIT", "TERMINATE SESSION"),
        };

        public const string ContinueDetailNoSave = "NO ACTIVE FIELD LOG";

        /// <summary>
        /// Detail line for an enabled Continue row. Naming the slot matters:
        /// autosave and quicksave can be hours apart, and the player is about
        /// to lose whichever one they did not pick.
        /// </summary>
        public static string ContinueDetail(string slotId) =>
            string.IsNullOrEmpty(slotId)
                ? ContinueDetailNoSave
                : "RESUME FIELD LOG // " + slotId.ToUpperInvariant();

        /// <summary>Two-digit row number, e.g. "03" for index 2.</summary>
        public static string IndexLabel(int index) => (index + 1).ToString("00");

        // -----------------------------------------------------------------
        // Dialogs
        // -----------------------------------------------------------------

        /// <summary>Copy for one dialog. All three share a single shell.</summary>
        public readonly struct DialogCopy
        {
            public readonly string Eyebrow;
            public readonly string Title;
            public readonly string Body;
            public readonly string Confirm;
            public readonly string Back;

            public DialogCopy(string eyebrow, string title, string body, string confirm, string back)
            {
                Eyebrow = eyebrow;
                Title = title;
                Body = body;
                Confirm = confirm;
                Back = back;
            }
        }

        /// <summary>
        /// The prototype promised "existing field progress will be retained",
        /// which is false in this codebase: a new game's first autosave
        /// overwrites save_autosave.json. The copy warns instead.
        /// </summary>
        public static readonly DialogCopy NewExpeditionDialog = new DialogCopy(
            "OPERATIONS / 01",
            "BEGIN A NEW SIGNAL",
            "Starting a new expedition overwrites your existing field log. "
            + "Select a difficulty before entering the exclusion zone.",
            "START EXPEDITION",
            "BACK");

        public static readonly DialogCopy SettingsDialog = new DialogCopy(
            "SYSTEM CONFIGURATION",
            "FIELD SETTINGS",
            "Changes apply immediately and are stored on this machine.",
            "DONE",
            "BACK");

        public static readonly DialogCopy CreditsDialog = new DialogCopy(
            "TRANSMISSION LOG",
            "CREDITS",
            "Made by " + AuthorName + ".\nBuilt with Unity.",
            "CLOSE",
            "BACK");

        public static readonly DialogCopy QuitDialog = new DialogCopy(
            "SESSION CONTROL",
            "RETURN TO DESKTOP?",
            "Your settings have been stored locally. The radio will remain on standby.",
            "QUIT TO DESKTOP",
            "BACK");

        // -----------------------------------------------------------------
        // Difficulty
        // -----------------------------------------------------------------

        public const string DifficultyOperativeLabel = "OPERATIVE";
        public const string DifficultyOperativeDetail = "STANDARD";
        public const string DifficultyVeteranLabel = "VETERAN";
        public const string DifficultyVeteranDetail = "SCARCE RESOURCES";

        public static string DifficultyLabel(ExpeditionDifficulty difficulty) =>
            difficulty == ExpeditionDifficulty.Veteran
                ? DifficultyVeteranLabel
                : DifficultyOperativeLabel;

        // -----------------------------------------------------------------
        // Chrome
        // -----------------------------------------------------------------

        public const string AuthorName = "Roberts the Atomic-war_Dev";

        /// <summary>Replaces the prototype's placeholder publisher line.</summary>
        public const string FooterCredit = "MADE BY ROBERTS THE ATOMIC-WAR_DEV";
    }
}
