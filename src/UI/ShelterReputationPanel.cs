// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Reputation;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;

namespace AtomicWar.GodotApp.UI
{
    public partial class ShelterReputationPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private HBoxContainer _splitBody = null!;
        private VBoxContainer _leftColumn = null!;
        private VBoxContainer _dimensionsContainer = null!;
        private VBoxContainer _tagsContainer = null!;
        private VBoxContainer _rightColumn = null!;
        private Label _perceptionSummary = null!;
        private VBoxContainer _evidenceListContainer = null!;

        private ShelterReputationHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(ShelterReputationHostSession session)
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

            _shell = new AshfallDashboardShell("Shelter Reputation // External Perception & Notoriety", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("notoriety", "Notoriety", "0%", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("standing", "Dominant Trait", "Neutral", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("tags", "Active Tags", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("evidence", "Evidence Events", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _splitBody = new HBoxContainer();
            _splitBody.AddThemeConstantOverride("separation", 16);
            _splitBody.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _splitBody.SizeFlagsVertical = SizeFlags.ExpandFill;

            // ── Left Column: Dimensions & Tags ─────────────────────────
            _leftColumn = new VBoxContainer();
            _leftColumn.AddThemeConstantOverride("separation", 12);
            _leftColumn.CustomMinimumSize = new Vector2(440, 400);
            _leftColumn.SizeFlagsHorizontal = SizeFlags.Fill;
            _leftColumn.SizeFlagsVertical = SizeFlags.ExpandFill;

            var dimTitle = new Label();
            dimTitle.Text = "REPUTATION DIMENSIONS";
            _leftColumn.AddChild(dimTitle);

            _dimensionsContainer = new VBoxContainer();
            _dimensionsContainer.AddThemeConstantOverride("separation", 8);
            _leftColumn.AddChild(_dimensionsContainer);

            var tagTitle = new Label();
            tagTitle.Text = "PUBLIC TAGS & WASTELAND DESIGNATIONS";
            _leftColumn.AddChild(tagTitle);

            _tagsContainer = new VBoxContainer();
            _tagsContainer.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _leftColumn.AddChild(_tagsContainer);

            _splitBody.AddChild(_leftColumn);

            // ── Right Column: Perception Summary & Evidence Log ────────
            _rightColumn = new VBoxContainer();
            _rightColumn.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);
            _rightColumn.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _rightColumn.SizeFlagsVertical = SizeFlags.ExpandFill;

            var percTitle = new Label();
            percTitle.Text = "EXTERNAL PERCEPTION BRIEFING";
            _rightColumn.AddChild(percTitle);

            _perceptionSummary = new Label();
            _perceptionSummary.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _rightColumn.AddChild(_perceptionSummary);

            var logTitle = new Label();
            logTitle.Text = "RECENT EVIDENCE & WITNESS REPORTS";
            _rightColumn.AddChild(logTitle);

            var rightScroll = new ScrollContainer();
            rightScroll.CustomMinimumSize = new Vector2(450, 260);
            rightScroll.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightScroll.SizeFlagsVertical = SizeFlags.ExpandFill;

            _evidenceListContainer = new VBoxContainer();
            _evidenceListContainer.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _evidenceListContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightScroll.AddChild(_evidenceListContainer);
            _rightColumn.AddChild(rightScroll);

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
            if (_host == null || _statusRail == null)
            {
                if (_perceptionSummary != null)
                {
                    _perceptionSummary.Text = "Shelter Reputation system is offline or uninitialized.";
                }
                return;
            }

            float notoriety = _host.Notoriety;
            var activeTags = _host.ActiveTags;
            var evidenceHistory = _host.GetEvidenceHistory();

            // Status Rail
            var notoCrit = notoriety > 75f ? AshfallMetricCard.Criticality.Critical
                : (notoriety > 40f ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("notoriety", $"{notoriety:F0}%", notoCrit);

            // Find dominant dimension
            ReputationDimension? dominantDim = null;
            float maxScore = -1f;
            foreach (ReputationDimension dim in Enum.GetValues(typeof(ReputationDimension)))
            {
                float score = Math.Abs(_host.GetScore(dim));
                if (score > maxScore)
                {
                    maxScore = score;
                    dominantDim = dim;
                }
            }

            string domName = dominantDim.HasValue ? dominantDim.Value.ToString() : "Neutral";
            _statusRail.Set("standing", domName, AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("tags", activeTags.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("evidence", evidenceHistory.Count.ToString(), AshfallMetricCard.Criticality.Normal);

            // Dimensions Container
            foreach (Node child in _dimensionsContainer.GetChildren())
            {
                child.QueueFree();
            }

            foreach (ReputationDimension dim in Enum.GetValues(typeof(ReputationDimension)))
            {
                float val = _host.GetScore(dim);
                var row = new HBoxContainer();
                row.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);

                var lbl = new Label();
                lbl.CustomMinimumSize = new Vector2(130, 24);
                lbl.Text = $"{dim}:";
                row.AddChild(lbl);

                var valLbl = new Label();
                valLbl.CustomMinimumSize = new Vector2(70, 24);
                valLbl.Text = $"{val:+0.0;-0.0;0.0}";
                row.AddChild(valLbl);

                var bar = new ProgressBar();
                bar.CustomMinimumSize = new Vector2(200, 18);
                bar.MinValue = -100;
                bar.MaxValue = 100;
                bar.Value = val;
                bar.ShowPercentage = false;
                row.AddChild(bar);

                _dimensionsContainer.AddChild(row);
            }

            // Tags Container
            foreach (Node child in _tagsContainer.GetChildren())
            {
                child.QueueFree();
            }

            if (activeTags.Count == 0)
            {
                var noTags = new Label();
                noTags.Text = "No prominent public designations established yet.";
                _tagsContainer.AddChild(noTags);
            }
            else
            {
                foreach (var tag in activeTags)
                {
                    var tagCard = new PanelContainer();
                    var tagVBox = new VBoxContainer();
                    tagVBox.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingXs);

                    var tagLabel = new Label();
                    tagLabel.Text = $"[{tag.ToString().ToUpperInvariant()}]";

                    var descLabel = new Label();
                    descLabel.Text = GetTagDescription(tag);
                    descLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;

                    tagVBox.AddChild(tagLabel);
                    tagVBox.AddChild(descLabel);
                    tagCard.AddChild(tagVBox);
                    _tagsContainer.AddChild(tagCard);
                }
            }

            // Perception Summary
            string summaryText = $"Notoriety: {notoriety:F1} / 100\n";
            if (notoriety < 15f)
            {
                summaryText += "The shelter is virtually unknown across the wasteland. Passing scouts view it as an abandoned bunker or low-priority salvage site.\n";
            }
            else if (notoriety < 50f)
            {
                summaryText += "Word is spreading across local trade routes. Regional survivors and scavenging parties are taking notice of shelter activities.\n";
            }
            else if (notoriety < 80f)
            {
                summaryText += "The shelter is widely known throughout the sector. Major factions maintain intelligence files on your actions and resources.\n";
            }
            else
            {
                summaryText += "WARNING: High notoriety. Your shelter is a prominent landmark and priority target for wasteland raiders and military factions alike.\n";
            }

            if (activeTags.Count > 0)
            {
                summaryText += "\nActive Perceptions: " + string.Join(", ", activeTags.Select(t => t.ToString()));
            }

            _perceptionSummary.Text = summaryText;

            // Evidence History
            foreach (Node child in _evidenceListContainer.GetChildren())
            {
                child.QueueFree();
            }

            if (evidenceHistory.Count == 0)
            {
                var noEvidence = new Label();
                noEvidence.Text = "No recorded evidence events in the public log.";
                _evidenceListContainer.AddChild(noEvidence);
            }
            else
            {
                // Show latest evidence first (up to 20)
                var recent = evidenceHistory.TakeLast(20).Reverse();
                foreach (var ev in recent)
                {
                    var evRow = new PanelContainer();
                    var evBox = new VBoxContainer();
                    evBox.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingXs);

                    var hdr = new Label();
                    hdr.Text = $"Day {ev.DayRecorded} | {ev.Medium} | {ev.Dimension} ({ev.Delta:+0.0;-0.0;0.0})";

                    var dsc = new Label();
                    dsc.Text = string.IsNullOrEmpty(ev.Description) ? $"Event: {ev.SourceEventId}" : $"{ev.Description} ({ev.SourceEventId})";
                    dsc.AutowrapMode = TextServer.AutowrapMode.WordSmart;

                    evBox.AddChild(hdr);
                    evBox.AddChild(dsc);
                    evRow.AddChild(evBox);
                    _evidenceListContainer.AddChild(evRow);
                }
            }
        }

        private static string GetTagDescription(ReputationTag tag) => tag switch
        {
            ReputationTag.Sanctuary => "Known across the wastes as an open haven for refugees and the wounded.",
            ReputationTag.TradingPost => "Renowned for fair trade, solvent barter, and dependable market exchange.",
            ReputationTag.Fortress => "Perceived as heavily armed and fortified; discouraging casual incursions.",
            ReputationTag.Dangerous => "Whispered about with caution; scouts warn of violent resistance and traps.",
            ReputationTag.Treacherous => "Wastelanders caution each other against trusting pacts or trade agreements.",
            ReputationTag.Honorable => "Recognized for keeping pledges, honoring treaties, and respecting truces.",
            ReputationTag.Desperate => "Rumored to be low on provisions; may attract opportunists and extortion.",
            ReputationTag.RaiderBane => "Feared by bandit clans and outlaws as relentless punishers of violence.",
            _ => "Public designation."
        };

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
