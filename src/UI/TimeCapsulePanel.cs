// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Communication;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class TimeCapsulePanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private HBoxContainer _splitBody = null!;
        private VBoxContainer _leftColumn = null!;
        private VBoxContainer _capsulesContainer = null!;
        private VBoxContainer _rightColumn = null!;
        private VBoxContainer _messagesContainer = null!;

        private TimeCapsuleHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(TimeCapsuleHostSession session)
        {
            _host = session;
            if (_host != null)
            {
                _host.StateChanged += RefreshView;
            }
            RefreshView();
        }

        /// <summary>
        /// Plan 212 letter route — a separate discovered-letters authority bound
        /// beside (never inside) the capsule state. Optional; existing binds
        /// unchanged.
        /// </summary>
        public void BindLetters(LetterDeliveryHostSession? letters)
        {
            // Detach any previous subscription before rebinding.
            if (_letters != null && _lettersBound)
            {
                _letters.StateChanged -= RefreshView;
            }
            _letters = letters;
            _lettersBound = _letters != null;
            if (_letters != null)
            {
                _letters.StateChanged += RefreshView;
            }
            RefreshView();
        }

        /// <summary>Discovered-letters privacy-safe section under the right column.</summary>
        private void RefreshLettersSection()
        {
            if (!IsInsideTree() || _messagesContainer == null) return;

            // Drop the previous letters section (tracked by name) and re-render.
            var previous = GetNodeOrNull("Plan212LettersSection");
            previous?.QueueFree();

            if (_letters == null) return;

            var section = new VBoxContainer { Name = "Plan212LettersSection", SizeFlagsHorizontal = SizeFlags.ExpandFill };
            section.AddThemeConstantOverride("separation", 6);

            var hdr = new Label { Text = "DISCOVERED LETTERS (Plan 212 letter route)" };
            hdr.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
            section.AddChild(hdr);

            var visible = _letters.VisibleLetters();
            if (visible.Count == 0)
            {
                var empty = new Label { Text = "No letters discovered yet. Expeditions may surface authored letters." };
                empty.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
                section.AddChild(empty);
            }
            else
            {
                foreach (var (rec, body) in visible)
                {
                    var row = new Label
                    {
                        Text = $"• {rec.letterId} [{rec.state}] found day {rec.foundDay}\n   {body}",
                        AutowrapMode = TextServer.AutowrapMode.WordSmart,
                    };
                    row.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(
                        rec.state == Ashfall.Core.Narrative.LetterDeliveryState.Delivered
                            ? Ashfall.Core.UI.Theme.Success
                            : Ashfall.Core.UI.Theme.Dim));
                    section.AddChild(row);
                }
            }

            _messagesContainer.AddChild(section);
        }

        private LetterDeliveryHostSession? _letters;
        private bool _lettersBound;

        public void Unbind()
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
                _host = null;
            }
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Shelter Heritage // Time Capsules & Legacy Messages", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("unopened", "Sealed Capsules", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("total", "Total Capsules", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("pending_msgs", "Pending Messages", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _splitBody = new HBoxContainer();
            _splitBody.AddThemeConstantOverride("separation", 16);
            _splitBody.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _splitBody.SizeFlagsVertical = SizeFlags.ExpandFill;

            // Left Column: Time Capsules
            _leftColumn = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill, SizeFlagsVertical = SizeFlags.ExpandFill };
            var capHdr = new Label { Text = "SEALED & RECOVERED TIME CAPSULES" };
            _leftColumn.AddChild(capHdr);

            var capScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill, SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _capsulesContainer = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _capsulesContainer.AddThemeConstantOverride("separation", 8);
            capScroll.AddChild(_capsulesContainer);
            _leftColumn.AddChild(capScroll);
            _splitBody.AddChild(_leftColumn);

            // Right Column: Legacy Messages
            _rightColumn = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill, SizeFlagsVertical = SizeFlags.ExpandFill };
            var msgHdr = new Label { Text = "CROSS-GENERATIONAL MESSAGES" };
            _rightColumn.AddChild(msgHdr);

            var msgScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill, SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _messagesContainer = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _messagesContainer.AddThemeConstantOverride("separation", 8);
            msgScroll.AddChild(_messagesContainer);
            _rightColumn.AddChild(msgScroll);
            _splitBody.AddChild(_rightColumn);

            _contentStack.AddChild(_splitBody);
            _shell.SetContent(_contentStack);

            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        public void RefreshView()
        {
            if (!IsInsideTree())
                return;
            if (_host == null)
            {
                // Letters-only bind (Plan 212 letter route) still refreshes.
                RefreshLettersSection();
                return;
            }

            _statusRail?.Set("unopened", _host.UnopenedCapsuleCount.ToString());
            _statusRail?.Set("total", _host.TotalCapsuleCount.ToString());
            _statusRail?.Set("pending_msgs", _host.PendingMessageCount.ToString());

            // Populate Capsules
            foreach (Node child in _capsulesContainer.GetChildren())
                child.QueueFree();

            if (_host.Capsules.Count == 0)
            {
                var emptyLabel = new Label
                {
                    Text = "No time capsules buried or cached yet. Survivors can cache artifacts, letters, and memories for future generations.",
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                emptyLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
                _capsulesContainer.AddChild(emptyLabel);
            }
            else
            {
                foreach (var c in _host.Capsules)
                {
                    var card = new PanelContainer();
                    var cardStack = new VBoxContainer();
                    cardStack.AddThemeConstantOverride("separation", 4);

                    var nameLabel = new Label { Text = $"{c.CapsuleName} (By {c.CreatorId}, Day {c.CreatedDay})" };
                    nameLabel.AddThemeColorOverride("font_color", c.IsOpen ? AshfallUiHelpers.ToColor(DesignTheme.Info) : AshfallUiHelpers.ToColor(DesignTheme.Warning));
                    cardStack.AddChild(nameLabel);

                    string statusStr = c.IsOpen ? $"OPENED on Day {c.OpenedDay} by {c.OpenedBy}" : $"SEALED [{c.ConditionType}]";
                    if (!c.IsOpen && c.OpenDay > 0) statusStr += $" (Target Day: {c.OpenDay})";
                    if (!string.IsNullOrEmpty(c.Location)) statusStr += $" | Loc: {c.Location}";

                    var statusLabel = new Label { Text = statusStr };
                    cardStack.AddChild(statusLabel);

                    if (!string.IsNullOrEmpty(c.Message))
                    {
                        var msgLabel = new Label { Text = $"Note: \"{c.Message}\"", AutowrapMode = TextServer.AutowrapMode.WordSmart };
                        msgLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
                        cardStack.AddChild(msgLabel);
                    }

                    if (c.Contents.Count > 0)
                    {
                        var itemsLabel = new Label { Text = $"Contents: {string.Join(", ", c.Contents.Select(cnt => $"{cnt.ContentType}: {cnt.ItemId} {cnt.Text}"))}" };
                        cardStack.AddChild(itemsLabel);
                    }

                    if (!c.IsOpen)
                    {
                        string capId = c.CapsuleId;
                        var openBtn = AshfallUiHelpers.MakeButton("UNSEAL CAPSULE", () =>
                        {
                            // Plan 212: truthful unseal — validates the capsule's
                            // own condition at the live campaign day; the
                            // blocked reason surfaces when it refuses.
                            var (opened, reason) = _host.TryOpenCapsule(capId, "Overseer", 1);
                            if (!opened) GD.PushWarning($"[TimeCapsule] unseal refused: {reason}");
                        });
                        cardStack.AddChild(openBtn);
                    }

                    card.AddChild(cardStack);
                    _capsulesContainer.AddChild(card);
                }
            }

            // Populate Messages
            foreach (Node child in _messagesContainer.GetChildren())
                child.QueueFree();

            if (_host.Messages.Count == 0)
            {
                var emptyLabel = new Label
                {
                    Text = "No legacy messages queued. Survivors can write sealed letters to deliver on specific dates, death, or milestones.",
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                emptyLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
                _messagesContainer.AddChild(emptyLabel);
            }
            else
            {
                foreach (var m in _host.VisibleMessages()) // Plan 212: pending content stays sealed
                {
                    var card = new PanelContainer();
                    var cardStack = new VBoxContainer();
                    cardStack.AddThemeConstantOverride("separation", 4);

                    var hdr = new Label { Text = $"From {m.AuthorId} to {m.RecipientId} (Day {m.CreatedDay})" };
                    hdr.AddThemeColorOverride("font_color", m.IsDelivered ? AshfallUiHelpers.ToColor(DesignTheme.Success) : AshfallUiHelpers.ToColor(DesignTheme.Warning));
                    cardStack.AddChild(hdr);

                    var contentLabel = new Label { Text = $"\"{m.Content}\"", AutowrapMode = TextServer.AutowrapMode.WordSmart };
                    cardStack.AddChild(contentLabel);

                    var deliveryLabel = new Label { Text = $"Condition: {m.Condition} | Status: {(m.IsDelivered ? "DELIVERED" : "PENDING")}" };
                    cardStack.AddChild(deliveryLabel);

                    card.AddChild(cardStack);
                    _messagesContainer.AddChild(card);
                }
            }

            RefreshLettersSection();
        }
    }
}
