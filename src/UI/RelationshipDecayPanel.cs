// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;

namespace AtomicWar.GodotApp.UI
{
    public partial class RelationshipDecayPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private HBoxContainer _splitBody = null!;
        private VBoxContainer _leftColumn = null!;
        private VBoxContainer _bondsContainer = null!;
        private VBoxContainer _rightColumn = null!;
        private VBoxContainer _driftContainer = null!;

        private RelationshipDecayHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(RelationshipDecayHostSession session)
        {
            _host = session;
            if (_host != null)
            {
                _host.StateChanged += RefreshView;
            }
            RefreshView();
        }

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

            _shell = new AshfallDashboardShell("Survivor Social Ecology // Bonds & Social Drift", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("pairs", "Tracked Bonds", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("close_friends", "Close Friends", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("drift_events", "Drift Incidents", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 10);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _splitBody = new HBoxContainer();
            _splitBody.AddThemeConstantOverride("separation", 16);
            _splitBody.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _splitBody.SizeFlagsVertical = SizeFlags.ExpandFill;

            // Left Column: Survivor Pair Bonds
            _leftColumn = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill, SizeFlagsVertical = SizeFlags.ExpandFill };
            _leftColumn.AddChild(new Label { Text = "SURVIVOR PAIR BONDS & STATUS" });

            var bondScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill, SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _bondsContainer = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _bondsContainer.AddThemeConstantOverride("separation", 8);
            bondScroll.AddChild(_bondsContainer);
            _leftColumn.AddChild(bondScroll);
            _splitBody.AddChild(_leftColumn);

            // Right Column: Social Drift History
            _rightColumn = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill, SizeFlagsVertical = SizeFlags.ExpandFill };
            _rightColumn.AddChild(new Label { Text = "SOCIAL DRIFT & EROSION LOG" });

            var driftScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill, SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _driftContainer = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _driftContainer.AddThemeConstantOverride("separation", 8);
            driftScroll.AddChild(_driftContainer);
            _rightColumn.AddChild(driftScroll);
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
            if (_host == null || !IsInsideTree())
                return;

            int closeCount = _host.Pairs.Count(p => p.Bond == SurvivorBondType.CloseFriend || p.Bond == SurvivorBondType.Family);
            int driftCount = _host.DriftHistory.Count;

            _statusRail?.Set("pairs", _host.TrackedPairCount.ToString());
            _statusRail?.Set("close_friends", closeCount.ToString());
            _statusRail?.Set("drift_events", driftCount.ToString(), driftCount > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);

            // Populate Pair Bonds
            foreach (Node child in _bondsContainer.GetChildren())
                child.QueueFree();

            if (_host.Pairs.Count == 0)
            {
                var emptyLabel = new Label
                {
                    Text = "No relationship bonds tracked yet. As survivors work, dine, and bunk together, social bonds form.",
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                emptyLabel.AddThemeColorOverride("font_color", new Color(0.6f, 0.6f, 0.6f));
                _bondsContainer.AddChild(emptyLabel);
            }
            else
            {
                foreach (var p in _host.Pairs)
                {
                    var card = new PanelContainer();
                    var cardStack = new VBoxContainer();
                    cardStack.AddThemeConstantOverride("separation", 4);

                    var nameLabel = new Label { Text = $"{p.SurvivorA} & {p.SurvivorB} [{p.Bond}]" };
                    nameLabel.AddThemeFontSizeOverride("font_size", 14);
                    nameLabel.AddThemeColorOverride("font_color", new Color(0.95f, 0.85f, 0.4f));
                    cardStack.AddChild(nameLabel);

                    var statsLabel = new Label
                    {
                        Text = $"Affinity: {p.Affinity:F1} | Trust: {p.Trust:F1} | Days Since Interaction: {p.DaysWithoutInteraction}"
                    };
                    cardStack.AddChild(statsLabel);

                    card.AddChild(cardStack);
                    _bondsContainer.AddChild(card);
                }
            }

            // Populate Drift History
            foreach (Node child in _driftContainer.GetChildren())
                child.QueueFree();

            if (_host.DriftHistory.Count == 0)
            {
                var emptyLabel = new Label
                {
                    Text = "No social drift recorded. Relationships remain stable.",
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                emptyLabel.AddThemeColorOverride("font_color", new Color(0.6f, 0.6f, 0.6f));
                _driftContainer.AddChild(emptyLabel);
            }
            else
            {
                foreach (var ev in _host.DriftHistory)
                {
                    var card = new PanelContainer();
                    var cardStack = new VBoxContainer();
                    cardStack.AddThemeConstantOverride("separation", 4);

                    var hdr = new Label { Text = $"Day {ev.Day}: {ev.DriftType} ({ev.SurvivorA} & {ev.SurvivorB})" };
                    hdr.AddThemeFontSizeOverride("font_size", 14);
                    hdr.AddThemeColorOverride("font_color", new Color(0.95f, 0.5f, 0.4f));
                    cardStack.AddChild(hdr);

                    var descLabel = new Label { Text = ev.Description, AutowrapMode = TextServer.AutowrapMode.WordSmart };
                    cardStack.AddChild(descLabel);

                    card.AddChild(cardStack);
                    _driftContainer.AddChild(card);
                }
            }
        }
    }
}
