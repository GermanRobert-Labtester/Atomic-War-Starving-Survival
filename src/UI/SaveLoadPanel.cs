using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Save/Load panel.
    /// Real slot management bound to SaveLoadHostSession.
    /// </summary>
    public partial class SaveLoadPanel : Control
    {
        public event Action? OnClose;
        public event Action<SaveSlotId>? OnSlotSelected;
        public event Action<SaveSlotId>? OnLoadRequested;
        public event Action? OnSaveRequested;
        public event Action<SaveSlotId>? OnDeleteRequested;
        public event Action<string>? OnImportRequested;

        /// <summary>Plan VIII · Task 22.9 — new-game reset request for the
        /// selected slot (routed through SaveLoadHostSession.ResetSlotForNewGame).</summary>
        public event Action<SaveSlotId>? OnResetRequested;

        // Task 22.6 — destructive actions are two-step: the first click arms,
        // the second click (same selection) confirms. Selecting anything else
        // disarms. No typed-confirmation convention exists in this UI.
        private SaveSlotId? _pendingDeleteSlot;
        private bool _pendingReset;

        private SaveLoadHostSession? _session;
        private VBoxContainer _contentVBox = null!;
        private Label _lblSlotsTitle = null!;
        private VBoxContainer _slotsList = null!;
        private Label _lblInfoTitle = null!;
        private VBoxContainer _infoList = null!;
        private VBoxContainer _actionButtons = null!;
        private Label _statusMessageLabel = null!;
        private SaveSlotId? _selectedSlotId;

        public string LastStatusMessage { get; private set; } = string.Empty;
        public bool IsLastError { get; private set; }

        public void Bind(SaveLoadHostSession session)
        {
            if (_session != null)
            {
                _session.SlotsChanged -= RefreshView;
                _session.ActiveSlotChanged -= OnActiveSlotChanged;
                _session.OnLoadCompleted -= OnSessionLoadCompleted;
            }
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _session.SlotsChanged += RefreshView;
            _session.ActiveSlotChanged += OnActiveSlotChanged;
            _session.OnLoadCompleted += OnSessionLoadCompleted;
            RefreshView();
        }

        private void OnSessionLoadCompleted(SaveLoadResult result)
        {
            ShowStatusMessage(result.UserMessage, !result.IsSuccess);
        }

        public void ShowStatusMessage(string message, bool isError = false)
        {
            LastStatusMessage = message;
            IsLastError = isError;
            if (_statusMessageLabel != null)
            {
                _statusMessageLabel.Text = message;
                _statusMessageLabel.Visible = !string.IsNullOrWhiteSpace(message);
                var color = isError ? new Color(1f, 0.4f, 0.4f) : new Color(0.53f, 1f, 0.67f);
                _statusMessageLabel.AddThemeColorOverride("font_color", color);
            }
        }

        public void ShowError(string error) => ShowStatusMessage(error, isError: true);
        public void ShowSuccess(string message) => ShowStatusMessage(message, isError: false);
        public void ClearStatusMessage() => ShowStatusMessage(string.Empty, isError: false);

        private void OnActiveSlotChanged(SaveSlotId? slotId)
        {
            _selectedSlotId = slotId;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_slotsList == null || _infoList == null || _actionButtons == null) return;
            if (_session == null) return;

            AshfallUiHelpers.EmptyChildren(_slotsList);
            AshfallUiHelpers.EmptyChildren(_infoList);
            AshfallUiHelpers.EmptyChildren(_actionButtons);

            var slots = _session.GetSlots();
            if (slots.Count == 0)
            {
                var emptyLabel = AshfallUiHelpers.MakeSmall("No save slots. Create a new slot to begin.");
                emptyLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
                _slotsList.AddChild(emptyLabel);
            }
            else
            {
                foreach (SaveSlotId slotId in slots)
                {
                    SlotCard card = _session.BuildSlotCard(slotId);
                    bool isSelected = _selectedSlotId.HasValue && _selectedSlotId.Value == slotId;

                    string status = card.IsTerminalIronMan ? " [TERMINAL]" :
                                    card.HasValidSave ? $" [Day {card.CurrentDay}]" : " [empty]";

                    var hbox = new HBoxContainer();
                    var label = new Label
                    {
                        Text = $"{card.CampaignName}{status} — {card.Mode}",
                        CustomMinimumSize = new Vector2(380, 36)
                    };
                    label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                    hbox.AddChild(label);

                    if (isSelected)
                    {
                        var selected = new Label { Text = "< active" };
                        selected.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeLabel);
                        selected.AddThemeColorOverride("font_color", new Color(0.53f, 1f, 0.67f));
                        hbox.AddChild(selected);
                    }

                    var btnSelect = AshfallUiHelpers.MakeButton("SELECT", () =>
                    {
                        _selectedSlotId = slotId;
                        _pendingDeleteSlot = null;
                        _pendingReset = false;
                        OnSlotSelected?.Invoke(slotId);
                        RefreshView();
                    });
                    btnSelect.CustomMinimumSize = new Vector2(90, 32);
                    hbox.AddChild(btnSelect);

                    if (!card.IsTerminalIronMan)
                    {
                        bool armed = _pendingDeleteSlot.HasValue && _pendingDeleteSlot.Value == slotId;
                        var btnDelete = AshfallUiHelpers.MakeButton(armed ? "SURE?" : "DEL", () =>
                        {
                            if (_pendingDeleteSlot.HasValue && _pendingDeleteSlot.Value == slotId)
                            {
                                _pendingDeleteSlot = null;
                                _pendingReset = false;
                                OnDeleteRequested?.Invoke(slotId);
                            }
                            else
                            {
                                _pendingDeleteSlot = slotId;
                                _pendingReset = false;
                            }
                            RefreshView();
                        });
                        if (armed)
                            btnDelete.AddThemeColorOverride("font_color", new Color(1f, 0.45f, 0.4f));
                        btnDelete.CustomMinimumSize = new Vector2(90, 32);
                        hbox.AddChild(btnDelete);
                    }

                    _slotsList.AddChild(hbox);
                }
            }

            // Info section.
            if (_selectedSlotId.HasValue)
            {
                SlotCard card = _session.BuildSlotCard(_selectedSlotId.Value);
                var manifest = _session.GetManifest(_selectedSlotId.Value);

                var infoLines = new List<string>
                {
                    $"Slot: {card.SlotId}",
                    $"Campaign: {card.CampaignName}",
                    $"Mode: {card.Mode}",
                    $"Day: {card.CurrentDay}",
                    $"Terminal: {(card.IsTerminalIronMan ? "Yes" : "No")}",
                    $"Last Save: {card.LastSaveTimestamp}"
                };

                if (manifest != null)
                {
                    infoLines.Add($"Profile: {manifest.profileId}");
                    infoLines.Add($"Game Version: {manifest.gameVersion}");
                    infoLines.Add($"Build: {manifest.buildId}");
                    infoLines.Add($"Seed: {manifest.seed}");
                }

                // Task 22.4 — envelope health from the last persisted save.
                var health = _session.GetEnvelopeHealth(_selectedSlotId.Value);
                if (health == null || !health.EnvelopePresent)
                {
                    infoLines.Add(health != null && health.LoadFailed
                        ? "Envelope: LOAD FAILED (corrupt — keep for recovery)"
                        : "Envelope: none (empty slot)");
                }
                else
                {
                    infoLines.Add($"Envelope: v{health.ManifestVersion} — aggregate checksum "
                        + (health.AggregateChecksumPresent ? "present" : "MISSING")
                        + (health.MigratedFromLegacy ? " — migrated from legacy save" : string.Empty));
                    infoLines.Add($"Sections saved: {health.SectionCount}");
                    foreach (string line in health.SectionLines)
                        infoLines.Add("  · " + line);
                }

                foreach (string line in infoLines)
                {
                    var label = new Label { Text = line };
                    label.CustomMinimumSize = new Vector2(350, 28);
                    label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                    _infoList.AddChild(label);
                }
            }
            else
            {
                var hint = AshfallUiHelpers.MakeSmall("Select a slot to view details.");
                hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
                _infoList.AddChild(hint);
            }

            // Action buttons.
            var btnCreate = AshfallUiHelpers.MakeButton("NEW SLOT", () =>
            {
                // The line below already guarded _session; CreateSlot did not
                // (CS8602). An unbound panel firing this handler would have thrown.
                if (_session == null) return;
                var existing = _session.GetSlots() ?? new List<SaveSlotId>();
                int nextIdx = 1;
                while (existing.Any(s => s.Value == $"slot_{nextIdx}"))
                    nextIdx++;
                string newId = $"slot_{nextIdx}";
                var newSlotId = new SaveSlotId(newId);
                if (_session.CreateSlot(newSlotId))
                {
                    _selectedSlotId = newSlotId;
                    OnSlotSelected?.Invoke(newSlotId);
                    RefreshView();
                }
            });
            btnCreate.CustomMinimumSize = new Vector2(160, 40);
            _actionButtons.AddChild(btnCreate);

            var btnSave = AshfallUiHelpers.MakeButton("SAVE CURRENT", () => OnSaveRequested?.Invoke());
            btnSave.CustomMinimumSize = new Vector2(160, 40);
            btnSave.Disabled = !_selectedSlotId.HasValue;
            _actionButtons.AddChild(btnSave);

            var btnLoad = AshfallUiHelpers.MakeButton("LOAD SELECTED", () =>
            {
                if (_selectedSlotId.HasValue)
                    OnLoadRequested?.Invoke(_selectedSlotId.Value);
            });
            btnLoad.CustomMinimumSize = new Vector2(160, 40);
            btnLoad.Disabled = !_selectedSlotId.HasValue;
            _actionButtons.AddChild(btnLoad);

            // Task 22.9 — new-game reset via the save authority, two-step confirm.
            var btnReset = AshfallUiHelpers.MakeButton(_pendingReset ? "SURE? ERASE SLOT" : "NEW GAME (RESET)", () =>
            {
                if (_pendingReset)
                {
                    _pendingReset = false;
                    if (_selectedSlotId.HasValue)
                        OnResetRequested?.Invoke(_selectedSlotId.Value);
                }
                else
                {
                    _pendingReset = true;
                    _pendingDeleteSlot = null;
                }
                RefreshView();
            });
            btnReset.CustomMinimumSize = new Vector2(160, 40);
            btnReset.Disabled = !_selectedSlotId.HasValue;
            if (_pendingReset)
                btnReset.AddThemeColorOverride("font_color", new Color(1f, 0.45f, 0.4f));
            _actionButtons.AddChild(btnReset);

            var btnImport = AshfallUiHelpers.MakeButton("IMPORT LEGACY", () =>
            {
                // Host should present a file dialog; this button signals the intent.
                OnImportRequested?.Invoke(_session.CurrentProfileId.Value);
            });
            btnImport.CustomMinimumSize = new Vector2(160, 40);
            _actionButtons.AddChild(btnImport);
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.05f, 0.05f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var container = new CenterContainer();
            container.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(container);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(600, 0);
            container.AddChild(vbox);

            var title = AshfallUiHelpers.MakeTitle("SAVE & LOAD", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            _statusMessageLabel = new Label
            {
                Visible = false,
                AutowrapMode = TextServer.AutowrapMode.WordSmart,
                HorizontalAlignment = HorizontalAlignment.Center,
                CustomMinimumSize = new Vector2(500, 36)
            };
            _statusMessageLabel.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            vbox.AddChild(_statusMessageLabel);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblSlotsTitle = AshfallUiHelpers.MakeSectionHeader("SAVE SLOTS");
            vbox.AddChild(_lblSlotsTitle);

            _slotsList = new VBoxContainer();
            _slotsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _slotsList.CustomMinimumSize = new Vector2(500, 0);
            vbox.AddChild(_slotsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblInfoTitle = AshfallUiHelpers.MakeSectionHeader("SLOT INFORMATION");
            vbox.AddChild(_lblInfoTitle);

            _infoList = new VBoxContainer();
            _infoList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _infoList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_infoList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _actionButtons = new VBoxContainer();
            _actionButtons.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);
            _actionButtons.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_actionButtons);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(160, 40);
            vbox.AddChild(btnClose);

            var hint = AshfallUiHelpers.MakeSmall("[Esc] to close");
            hint.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeLabel);
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            vbox.AddChild(hint);
        }

        public void Unbind()
        {
            if (_session != null)
            {
                _session.SlotsChanged -= RefreshView;
                _session.ActiveSlotChanged -= OnActiveSlotChanged;
                _session.OnLoadCompleted -= OnSessionLoadCompleted;
                _session = null;
            }
            RefreshView();
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
            QueueRedraw();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;

            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
