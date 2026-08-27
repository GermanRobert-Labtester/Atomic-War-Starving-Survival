using System;
using Godot;
using Ashfall.Core.Maritime;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Safe Cracking modal.
    /// Thin presentation layer for safe/container interaction.
    /// Shows safe condition, lock profile, tool condition, noise level,
    /// attempt feedback, and loot transfer.
    /// All gameplay logic delegates to MaritimeHostSession → SafeCrackingSystem.
    /// </summary>
    public partial class SafeCrackModal : Control
    {
        public event Action? OnClose;
        public event Action? OnSafeOpened;

        private MaritimeHostSession? _maritimeHost;
        private string _safeId = string.Empty;
        private float _toolCondition = 1.0f;

        private Label _headerLabel = null!;
        private Label _safeInfoLabel = null!;
        private Label _difficultyLabel = null!;
        private Label _attemptsLabel = null!;
        private Label _noiseLabel = null!;
        private Label _toolLabel = null!;
        private Label _feedbackLabel = null!;
        private Label _lootLabel = null!;
        private HBoxContainer _guessRow = null!;
        private SpinBox[] _tumblers = null!;
        private Button _attemptButton = null!;
        private Button _accessibleButton = null!;
        private Button _transferLootButton = null!;
        private Button _abandonButton = null!;

        public bool IsBound => _maritimeHost != null;

        public void Bind(MaritimeHostSession maritimeHost, string safeId)
        {
            _maritimeHost = maritimeHost;
            _safeId = safeId;
            RefreshView();
        }

        public override void _Ready()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            var margin = AshfallUiHelpers.MakeMargins(16);
            AddChild(margin);

            var root = new VBoxContainer();
            margin.AddChild(root);

            _headerLabel = AshfallUiHelpers.MakeLabel("SAFE CRACKING", 20, true);
            root.AddChild(_headerLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            _safeInfoLabel = AshfallUiHelpers.MakeBody("Safe: —");
            root.AddChild(_safeInfoLabel);

            _difficultyLabel = AshfallUiHelpers.MakeBody("Difficulty: —");
            root.AddChild(_difficultyLabel);

            _attemptsLabel = AshfallUiHelpers.MakeBody("Attempts: —");
            root.AddChild(_attemptsLabel);

            _noiseLabel = AshfallUiHelpers.MakeBody("Noise: —");
            root.AddChild(_noiseLabel);

            _toolLabel = AshfallUiHelpers.MakeBody("Tool Condition: —");
            root.AddChild(_toolLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            // Guess input row
            _guessRow = new HBoxContainer();
            root.AddChild(_guessRow);
            _guessRow.AddChild(AshfallUiHelpers.MakeBody("Guess: "));

            // We'll create up to 6 tumblers, hide unused ones
            _tumblers = new SpinBox[6];
            for (int i = 0; i < 6; i++)
            {
                var spin = new SpinBox
                {
                    MinValue = 0,
                    MaxValue = 9,
                    Value = 0,
                    SizeFlagsHorizontal = SizeFlags.ExpandFill
                };
                _guessRow.AddChild(spin);
                _tumblers[i] = spin;
            }

            _feedbackLabel = AshfallUiHelpers.MakeBody("");
            root.AddChild(_feedbackLabel);

            _lootLabel = AshfallUiHelpers.MakeBody("");
            root.AddChild(_lootLabel);

            root.AddChild(AshfallUiHelpers.MakeSeparator());

            var buttonRow = new HBoxContainer();
            root.AddChild(buttonRow);

            _attemptButton = AshfallUiHelpers.MakeButton("Attempt", OnAttemptPressed);
            buttonRow.AddChild(_attemptButton);

            _accessibleButton = AshfallUiHelpers.MakeButton("Accessible Attempt", OnAccessibleAttempt);
            buttonRow.AddChild(_accessibleButton);

            _transferLootButton = AshfallUiHelpers.MakeButton("Transfer Loot", OnTransferLoot);
            _transferLootButton.Visible = false;
            buttonRow.AddChild(_transferLootButton);

            _abandonButton = AshfallUiHelpers.MakeButton("Abandon", () => OnClose?.Invoke());
            buttonRow.AddChild(_abandonButton);
        }

        private void OnAttemptPressed()
        {
            if (_maritimeHost == null) return;
            var safe = _maritimeHost.SafeCrack.GetSafe(_safeId);
            if (safe == null) return;

            int[] guess = new int[safe.difficulty];
            for (int i = 0; i < safe.difficulty; i++)
                guess[i] = (int)_tumblers[i].Value;

            string result = _maritimeHost.AttemptSafe(_safeId, guess, _toolCondition);
            _feedbackLabel.Text = result;

            if (_maritimeHost.SafeCrack.IsOpened(_safeId))
            {
                _transferLootButton.Visible = true;
                OnSafeOpened?.Invoke();
            }
            RefreshView();
        }

        private void OnAccessibleAttempt()
        {
            if (_maritimeHost == null) return;
            string result = _maritimeHost.AttemptSafeAccessible(_safeId, 0.5f, _toolCondition, 0.3f);
            _feedbackLabel.Text = result;

            if (_maritimeHost.SafeCrack.IsOpened(_safeId))
            {
                _transferLootButton.Visible = true;
                OnSafeOpened?.Invoke();
            }
            RefreshView();
        }

        private void OnTransferLoot()
        {
            if (_maritimeHost == null) return;
            string result = _maritimeHost.TransferSafeLoot(_safeId);
            _lootLabel.Text = result;
            RefreshView();
        }

        private void RefreshView()
        {
            if (_maritimeHost == null) return;

            var safe = _maritimeHost.SafeCrack.GetSafe(_safeId);
            if (safe == null)
            {
                _safeInfoLabel.Text = "Safe: Not found";
                return;
            }

            _safeInfoLabel.Text = $"Safe: {safe.safeId} ({safe.roomId})";
            _difficultyLabel.Text = $"Difficulty: {safe.difficulty} tumblers";
            _attemptsLabel.Text = $"Attempts: {safe.attemptsUsed}/{safe.maxAttempts}";

            _noiseLabel.Text = safe.cumulativeNoise >= safe.alarmThreshold
                ? $"Noise: {safe.cumulativeNoise:F2} [ALARM!]"
                : $"Noise: {safe.cumulativeNoise:F2}";
            _noiseLabel.Modulate = safe.cumulativeNoise >= safe.alarmThreshold ? Colors.Red : Colors.White;

            _toolLabel.Text = $"Tool Condition: {_toolCondition:P0}";

            // Show/hide tumbler spinboxes based on difficulty
            for (int i = 0; i < 6; i++)
                _tumblers[i].Visible = i < safe.difficulty;

            // Disable controls based on state
            bool canAttempt = !safe.isOpened && !safe.isJammed && _toolCondition > 0.05f;
            _attemptButton.Disabled = !canAttempt;
            _accessibleButton.Disabled = !canAttempt;
            _transferLootButton.Visible = safe.isOpened && !safe.lootTransferred;

            if (safe.isOpened)
                _feedbackLabel.Text = "Safe is OPEN";
            else if (safe.isJammed)
                _feedbackLabel.Text = "Safe is JAMMED";
        }

        public override void _ExitTree()
        {
        }
    }
}
