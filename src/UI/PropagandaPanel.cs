// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Propaganda;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;

namespace AtomicWar.GodotApp.UI
{
    public partial class PropagandaPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private HBoxContainer _splitBody = null!;
        private VBoxContainer _leftColumn = null!;
        private VBoxContainer _campaignsContainer = null!;
        private VBoxContainer _rightColumn = null!;
        private VBoxContainer _messagesContainer = null!;

        private PropagandaHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(PropagandaHostSession session)
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

            _shell = new AshfallDashboardShell("Information Warfare // Propaganda & Influence", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("credibility", "Credibility", "75%", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("active_ops", "Active Ops", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("messages", "Messages", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("impact", "Faction Impact", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 10);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _splitBody = new HBoxContainer();
            _splitBody.AddThemeConstantOverride("separation", 16);
            _splitBody.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _splitBody.SizeFlagsVertical = SizeFlags.ExpandFill;

            // ── Left Column: Operations / Campaigns ─────────────────────
            _leftColumn = new VBoxContainer();
            _leftColumn.AddThemeConstantOverride("separation", 10);
            _leftColumn.CustomMinimumSize = new Vector2(480, 400);
            _leftColumn.SizeFlagsHorizontal = SizeFlags.Fill;
            _leftColumn.SizeFlagsVertical = SizeFlags.ExpandFill;

            var campHdr = new Label();
            campHdr.Text = "PROPAGANDA CAMPAIGNS & OPERATIONS";
            _leftColumn.AddChild(campHdr);

            var campScroll = new ScrollContainer();
            campScroll.SizeFlagsVertical = SizeFlags.ExpandFill;
            campScroll.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            _campaignsContainer = new VBoxContainer();
            _campaignsContainer.AddThemeConstantOverride("separation", 8);
            _campaignsContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            campScroll.AddChild(_campaignsContainer);
            _leftColumn.AddChild(campScroll);

            _splitBody.AddChild(_leftColumn);

            // ── Right Column: Messages & Intel ──────────────────────────
            _rightColumn = new VBoxContainer();
            _rightColumn.AddThemeConstantOverride("separation", 10);
            _rightColumn.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _rightColumn.SizeFlagsVertical = SizeFlags.ExpandFill;

            var msgHdr = new Label();
            msgHdr.Text = "AUTHORED DISPATCHES & LEAFLETS";
            _rightColumn.AddChild(msgHdr);

            var msgScroll = new ScrollContainer();
            msgScroll.SizeFlagsVertical = SizeFlags.ExpandFill;
            msgScroll.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            _messagesContainer = new VBoxContainer();
            _messagesContainer.AddThemeConstantOverride("separation", 8);
            _messagesContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
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
            if (_host == null || !IsInsideTree()) return;

            // 1. Update Status Rail
            float credibility = _host.ShelterCredibility;
            var credCrit = credibility < 30f ? AshfallMetricCard.Criticality.Critical :
                           credibility < 60f ? AshfallMetricCard.Criticality.Warn :
                           AshfallMetricCard.Criticality.Normal;
            _statusRail?.Set("credibility", $"{credibility:0}%", credCrit);
            _statusRail?.Set("active_ops", _host.ActiveCampaignCount.ToString());
            _statusRail?.Set("messages", _host.MessageCount.ToString());

            int affectedFactions = _host.System.State.FactionMoraleImpacts.Count;
            _statusRail?.Set("impact", $"{affectedFactions} Factions");

            // 2. Render Campaigns
            foreach (Node child in _campaignsContainer.GetChildren())
            {
                child.QueueFree();
            }

            var campaigns = _host.Campaigns;
            if (campaigns == null || campaigns.Count == 0)
            {
                var emptyLabel = new Label();
                emptyLabel.Text = "No propaganda campaigns launched. Author dispatches to initiate an operation.";
                _campaignsContainer.AddChild(emptyLabel);
            }
            else
            {
                foreach (var cmp in campaigns)
                {
                    var card = new PanelContainer();
                    var cardBox = new VBoxContainer();
                    cardBox.AddThemeConstantOverride("separation", 4);

                    var titleLbl = new Label();
                    string statusTag = cmp.WasDetected ? "[COMPROMISED]" : $"[{cmp.Status.ToString().ToUpperInvariant()}]";
                    titleLbl.Text = $"{statusTag} {cmp.CampaignName} -> {cmp.TargetFactionId}";

                    var detailsLbl = new Label();
                    detailsLbl.Text = $"Objective: {cmp.Objective} | Duration: {cmp.DurationDays}d | Messages: {cmp.MessageIds.Count}";

                    var bar = new ProgressBar();
                    bar.MinValue = 0;
                    bar.MaxValue = 100;
                    bar.Value = Math.Clamp(cmp.AccumulatedEffectiveness, 0, 100);
                    bar.CustomMinimumSize = new Vector2(0, 16);

                    cardBox.AddChild(titleLbl);
                    cardBox.AddChild(detailsLbl);
                    cardBox.AddChild(bar);
                    card.AddChild(cardBox);
                    _campaignsContainer.AddChild(card);
                }
            }

            // 3. Render Messages
            foreach (Node child in _messagesContainer.GetChildren())
            {
                child.QueueFree();
            }

            var messages = _host.Messages;
            if (messages == null || messages.Count == 0)
            {
                var emptyMsg = new Label();
                emptyMsg.Text = "No propaganda messages drafted. Compose radio broadcasts or printed leaflets.";
                _messagesContainer.AddChild(emptyMsg);
            }
            else
            {
                foreach (var msg in messages.TakeLast(20).Reverse())
                {
                    var msgCard = new PanelContainer();
                    var msgBox = new VBoxContainer();
                    msgBox.AddThemeConstantOverride("separation", 2);

                    var header = new Label();
                    header.Text = $"Day {msg.CreationDay} | {msg.Medium} | {msg.Theme} ({msg.Truthfulness})";

                    var body = new Label();
                    body.Text = string.IsNullOrWhiteSpace(msg.Content) ? "(Classified Content)" : $"\"{msg.Content}\"";
                    body.AutowrapMode = TextServer.AutowrapMode.WordSmart;

                    var target = new Label();
                    target.Text = $"Target: {msg.TargetFactionId} ({msg.TargetAudience}) | Quality: {msg.Quality:0.0}";

                    msgBox.AddChild(header);
                    msgBox.AddChild(body);
                    msgBox.AddChild(target);
                    msgCard.AddChild(msgBox);
                    _messagesContainer.AddChild(msgCard);
                }
            }
        }
    }
}
