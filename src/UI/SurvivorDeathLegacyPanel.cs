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
    public partial class SurvivorDeathLegacyPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private HBoxContainer _splitBody = null!;
        private VBoxContainer _leftColumn = null!;
        private VBoxContainer _deathsContainer = null!;
        private VBoxContainer _rightColumn = null!;
        private VBoxContainer _willsContainer = null!;

        private SurvivorDeathLegacyHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(SurvivorDeathLegacyHostSession session)
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

            _shell = new AshfallDashboardShell("Survivor Memorial & Wills // Death Records & Estates", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("deaths", "Deceased", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("wills", "Registered Wills", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("disputes", "Active Disputes", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 10);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _splitBody = new HBoxContainer();
            _splitBody.AddThemeConstantOverride("separation", 16);
            _splitBody.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _splitBody.SizeFlagsVertical = SizeFlags.ExpandFill;

            // Left Column: Death Records
            _leftColumn = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill, SizeFlagsVertical = SizeFlags.ExpandFill };
            var deathHdr = new Label { Text = "MEMORIAL DEATH RECORDS" };
            _leftColumn.AddChild(deathHdr);

            var deathScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill, SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _deathsContainer = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _deathsContainer.AddThemeConstantOverride("separation", 8);
            deathScroll.AddChild(_deathsContainer);
            _leftColumn.AddChild(deathScroll);
            _splitBody.AddChild(_leftColumn);

            // Right Column: Wills & Inheritance
            _rightColumn = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill, SizeFlagsVertical = SizeFlags.ExpandFill };
            var willHdr = new Label { Text = "LAST WILLS & ESTATE DISTRIBUTION" };
            _rightColumn.AddChild(willHdr);

            var willScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill, SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _willsContainer = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _willsContainer.AddThemeConstantOverride("separation", 8);
            willScroll.AddChild(_willsContainer);
            _rightColumn.AddChild(willScroll);
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

            _statusRail?.Set("deaths", _host.DeathCount.ToString(), _host.DeathCount > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail?.Set("wills", _host.WillCount.ToString());
            _statusRail?.Set("disputes", _host.ActiveDisputeCount.ToString(), _host.ActiveDisputeCount > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);

            // Populate Death Records
            foreach (Node child in _deathsContainer.GetChildren())
                child.QueueFree();

            if (_host.DeathRecords.Count == 0)
            {
                var emptyLabel = new Label
                {
                    Text = "No deceased survivors recorded. The memorial wall remains unwritten.",
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                emptyLabel.AddThemeColorOverride("font_color", new Color(0.6f, 0.6f, 0.6f));
                _deathsContainer.AddChild(emptyLabel);
            }
            else
            {
                foreach (var d in _host.DeathRecords)
                {
                    var card = new PanelContainer();
                    var cardStack = new VBoxContainer();
                    cardStack.AddThemeConstantOverride("separation", 4);

                    var nameLabel = new Label { Text = $"{d.SurvivorName} (Day {d.DeathDay})" };
                    nameLabel.AddThemeColorOverride("font_color", new Color(0.95f, 0.5f, 0.5f));
                    cardStack.AddChild(nameLabel);

                    var causeLabel = new Label { Text = $"Cause: {d.Cause} | Loc: {(string.IsNullOrEmpty(d.LocationAtDeath) ? "Shelter" : d.LocationAtDeath)}" };
                    cardStack.AddChild(causeLabel);

                    if (!string.IsNullOrEmpty(d.LastWords))
                    {
                        var wordsLabel = new Label { Text = $"Last Words: \"{d.LastWords}\"", AutowrapMode = TextServer.AutowrapMode.WordSmart };
                        wordsLabel.AddThemeColorOverride("font_color", new Color(0.85f, 0.85f, 0.85f));
                        cardStack.AddChild(wordsLabel);
                    }

                    if (!string.IsNullOrEmpty(d.Circumstances))
                    {
                        var circLabel = new Label { Text = $"Note: {d.Circumstances}", AutowrapMode = TextServer.AutowrapMode.WordSmart };
                        circLabel.AddThemeColorOverride("font_color", new Color(0.7f, 0.7f, 0.7f));
                        cardStack.AddChild(circLabel);
                    }

                    card.AddChild(cardStack);
                    _deathsContainer.AddChild(card);
                }
            }

            // Populate Wills
            foreach (Node child in _willsContainer.GetChildren())
                child.QueueFree();

            if (_host.Wills.Count == 0)
            {
                var emptyLabel = new Label
                {
                    Text = "No last wills on file. Survivors can record wills to designate heirs for their personal gear and valuables.",
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                emptyLabel.AddThemeColorOverride("font_color", new Color(0.6f, 0.6f, 0.6f));
                _willsContainer.AddChild(emptyLabel);
            }
            else
            {
                foreach (var w in _host.Wills)
                {
                    var card = new PanelContainer();
                    var cardStack = new VBoxContainer();
                    cardStack.AddThemeConstantOverride("separation", 4);

                    var hdr = new Label { Text = $"Will of {w.SurvivorId} (Registered Day {w.CreatedDay})" };
                    hdr.AddThemeColorOverride("font_color", w.IsValid ? new Color(0.4f, 0.85f, 0.95f) : new Color(0.6f, 0.6f, 0.6f));
                    cardStack.AddChild(hdr);

                    if (w.Beneficiaries.Count > 0)
                    {
                        var benLabel = new Label { Text = $"Beneficiaries: {string.Join(", ", w.Beneficiaries.Select(b => $"{b.BeneficiaryId} ({b.Percentage}% {b.Category})"))}" };
                        cardStack.AddChild(benLabel);
                    }

                    if (w.SpecialBequests.Count > 0)
                    {
                        var beqLabel = new Label { Text = $"Bequests: {string.Join(", ", w.SpecialBequests.Select(sb => $"{sb.ItemId} -> {sb.RecipientId}"))}" };
                        cardStack.AddChild(beqLabel);
                    }

                    if (!string.IsNullOrEmpty(w.ResiduaryBeneficiary))
                    {
                        var resLabel = new Label { Text = $"Residuary: {w.ResiduaryBeneficiary}" };
                        cardStack.AddChild(resLabel);
                    }

                    card.AddChild(cardStack);
                    _willsContainer.AddChild(card);
                }
            }
        }
    }
}
