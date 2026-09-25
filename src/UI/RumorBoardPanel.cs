// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.InformationFlow;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;

namespace AtomicWar.GodotApp.UI
{
    public partial class RumorBoardPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private HBoxContainer _splitBody = null!;
        private VBoxContainer _leftColumn = null!;
        private VBoxContainer _rumorsContainer = null!;
        private VBoxContainer _rightColumn = null!;
        private VBoxContainer _hubsContainer = null!;

        private RumorNetworkHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(RumorNetworkHostSession session)
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

            _shell = new AshfallDashboardShell("Intelligence & Rumors // Wasteland Whispers & Intercepts", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("total_rumors", "Active Rumors", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("intercepted", "Intercepted", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("hubs", "Listening Hubs", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("avg_truth", "Avg Credibility", "0%", AshfallMetricCard.Criticality.Normal, minWidth: 140);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _splitBody = new HBoxContainer();
            _splitBody.AddThemeConstantOverride("separation", 16);
            _splitBody.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _splitBody.SizeFlagsVertical = SizeFlags.ExpandFill;

            // ── Left Column: Rumors List ────────────────────────────────
            _leftColumn = new VBoxContainer();
            _leftColumn.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);
            _leftColumn.CustomMinimumSize = new Vector2(500, 400);
            _leftColumn.SizeFlagsHorizontal = SizeFlags.Fill;
            _leftColumn.SizeFlagsVertical = SizeFlags.ExpandFill;

            var rumHdr = new Label();
            rumHdr.Text = "CIRCULATING WASTELAND RUMORS";
            _leftColumn.AddChild(rumHdr);

            var rumScroll = new ScrollContainer();
            rumScroll.SizeFlagsVertical = SizeFlags.ExpandFill;
            rumScroll.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            _rumorsContainer = new VBoxContainer();
            _rumorsContainer.AddThemeConstantOverride("separation", 8);
            _rumorsContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rumScroll.AddChild(_rumorsContainer);
            _leftColumn.AddChild(rumScroll);

            _splitBody.AddChild(_leftColumn);

            // ── Right Column: Information Hubs ──────────────────────────
            _rightColumn = new VBoxContainer();
            _rightColumn.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);
            _rightColumn.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _rightColumn.SizeFlagsVertical = SizeFlags.ExpandFill;

            var hubHdr = new Label();
            hubHdr.Text = "INFORMATION HUBS & OUTPOSTS";
            _rightColumn.AddChild(hubHdr);

            var hubScroll = new ScrollContainer();
            hubScroll.SizeFlagsVertical = SizeFlags.ExpandFill;
            hubScroll.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            _hubsContainer = new VBoxContainer();
            _hubsContainer.AddThemeConstantOverride("separation", 8);
            _hubsContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            hubScroll.AddChild(_hubsContainer);
            _rightColumn.AddChild(hubScroll);

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
            int total = _host.TotalRumorCount;
            int intercepted = _host.InterceptedRumorCount;
            int hubs = _host.HubCount;
            float avgTruth = total > 0 ? _host.Rumors.Average(r => r.Truthfulness) * 100f : 0f;

            _statusRail?.Set("total_rumors", total.ToString());
            _statusRail?.Set("intercepted", intercepted.ToString());
            _statusRail?.Set("hubs", hubs.ToString());
            _statusRail?.Set("avg_truth", $"{avgTruth:0}%");

            // 2. Render Rumors
            foreach (Node child in _rumorsContainer.GetChildren())
            {
                child.QueueFree();
            }

            var rumors = _host.Rumors;
            if (rumors == null || rumors.Count == 0)
            {
                var emptyLabel = new Label();
                emptyLabel.Text = "No active whispers circulating the wastes. Information will arrive via traders and radio relays.";
                _rumorsContainer.AddChild(emptyLabel);
            }
            else
            {
                foreach (var rumor in rumors)
                {
                    var card = new PanelContainer();
                    var cardBox = new VBoxContainer();
                    cardBox.AddThemeConstantOverride("separation", 4);

                    string badge = rumor.Truthfulness >= 0.75f ? "[VERIFIED]" :
                                   rumor.Truthfulness >= 0.40f ? "[UNCONFIRMED]" : "[DUBIOUS]";

                    string interceptTag = rumor.IsIntercepted ? " | [INTERCEPTED]" : "";

                    var titleLbl = new Label();
                    titleLbl.Text = $"{badge} {rumor.Headline}{interceptTag}";

                    var descLbl = new Label();
                    descLbl.Text = rumor.Description;
                    descLbl.AutowrapMode = TextServer.AutowrapMode.WordSmart;

                    var metaLbl = new Label();
                    metaLbl.Text = $"Subject: {rumor.SubjectType} ({rumor.SubjectId}) | Origin: {rumor.OriginLocationId} (Day {rumor.OriginDay}) | Hubs: {rumor.ReachedHubIds.Count}";

                    cardBox.AddChild(titleLbl);
                    cardBox.AddChild(descLbl);
                    cardBox.AddChild(metaLbl);

                    if (!rumor.IsIntercepted)
                    {
                        var interceptBtn = new Button();
                        interceptBtn.Text = "Intercept Signal";
                        interceptBtn.Pressed += () =>
                        {
                            _host.InterceptRumor(rumor.RumorId);
                            RefreshView();
                        };
                        cardBox.AddChild(interceptBtn);
                    }

                    card.AddChild(cardBox);
                    _rumorsContainer.AddChild(card);
                }
            }

            // 3. Render Hubs
            foreach (Node child in _hubsContainer.GetChildren())
            {
                child.QueueFree();
            }

            var hubList = _host.Hubs;
            if (hubList == null || hubList.Count == 0)
            {
                var emptyHub = new Label();
                emptyHub.Text = "No information hubs registered in this region.";
                _hubsContainer.AddChild(emptyHub);
            }
            else
            {
                foreach (var hub in hubList)
                {
                    var hubCard = new PanelContainer();
                    var hubBox = new VBoxContainer();
                    hubBox.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingXs);

                    var hubName = new Label();
                    hubName.Text = $"{hub.HubName} [{hub.Bias.ToUpperInvariant()}]";

                    var hubLoc = new Label();
                    hubLoc.Text = $"Location: {hub.LocationId} | Credibility: {hub.Credibility * 100f:0}%";

                    int rumorCount = _host.Rumors.Count(r => r.ReachedHubIds.Contains(hub.HubId));
                    var countLbl = new Label();
                    countLbl.Text = $"Known Rumors: {rumorCount}";

                    hubBox.AddChild(hubName);
                    hubBox.AddChild(hubLoc);
                    hubBox.AddChild(countLbl);
                    hubCard.AddChild(hubBox);
                    _hubsContainer.AddChild(hubCard);
                }
            }
        }
    }
}
