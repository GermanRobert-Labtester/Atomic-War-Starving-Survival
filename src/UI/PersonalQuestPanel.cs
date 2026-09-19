// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Quests;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;

namespace AtomicWar.GodotApp.UI
{
    public partial class PersonalQuestPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private HBoxContainer _splitBody = null!;
        private VBoxContainer _leftColumn = null!;
        private VBoxContainer _activeQuestsContainer = null!;
        private VBoxContainer _rightColumn = null!;
        private VBoxContainer _completedQuestsContainer = null!;

        private PersonalQuestHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(PersonalQuestHostSession session)
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

            _shell = new AshfallDashboardShell("Survivor Character Arcs // Personal Quests", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("active_quests", "Active Quests", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("completed_quests", "Completed", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("catalog_count", "Available Arcs", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 10);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _splitBody = new HBoxContainer();
            _splitBody.AddThemeConstantOverride("separation", 16);
            _splitBody.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _splitBody.SizeFlagsVertical = SizeFlags.ExpandFill;

            // Left Column: Active Character Quests
            _leftColumn = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill, SizeFlagsVertical = SizeFlags.ExpandFill };
            var activeHdr = new Label { Text = "ACTIVE CHARACTER ARCS" };
            _leftColumn.AddChild(activeHdr);

            var activeScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill, SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _activeQuestsContainer = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _activeQuestsContainer.AddThemeConstantOverride("separation", 8);
            activeScroll.AddChild(_activeQuestsContainer);
            _leftColumn.AddChild(activeScroll);
            _splitBody.AddChild(_leftColumn);

            // Right Column: Completed & Catalog
            _rightColumn = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill, SizeFlagsVertical = SizeFlags.ExpandFill };
            var completedHdr = new Label { Text = "RESOLVED ARCS & REPERCUSSIONS" };
            _rightColumn.AddChild(completedHdr);

            var completedScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill, SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _completedQuestsContainer = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _completedQuestsContainer.AddThemeConstantOverride("separation", 8);
            completedScroll.AddChild(_completedQuestsContainer);
            _rightColumn.AddChild(completedScroll);
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

            int activeCount = _host.System.ActiveQuests.Count;
            int completedCount = _host.System.CompletedQuests.Count;
            int catalogCount = _host.System.Catalog.Count;

            _statusRail?.Set("active_quests", activeCount.ToString());
            _statusRail?.Set("completed_quests", completedCount.ToString());
            _statusRail?.Set("catalog_count", catalogCount.ToString());

            // Populate Active Quests
            foreach (Node child in _activeQuestsContainer.GetChildren())
                child.QueueFree();

            if (activeCount == 0)
            {
                var emptyLabel = new Label
                {
                    Text = "No active personal quests. Survivors unlock personal arcs as their traits and shelter bonds develop.",
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                emptyLabel.AddThemeColorOverride("font_color", new Color(0.6f, 0.6f, 0.6f));
                _activeQuestsContainer.AddChild(emptyLabel);
            }
            else
            {
                foreach (var q in _host.System.ActiveQuests)
                {
                    var card = new PanelContainer();
                    var cardStack = new VBoxContainer();
                    cardStack.AddThemeConstantOverride("separation", 4);

                    string title = q.questId;
                    string stageDesc = $"Stage {q.currentStage + 1}";
                    if (_host.System.Catalog.TryGetValue(q.questId, out var def))
                    {
                        title = $"{def.title} ({q.survivorId})";
                        if (q.currentStage >= 0 && q.currentStage < def.stages.Count)
                        {
                            var st = def.stages[q.currentStage];
                            stageDesc = $"{st.title} — {st.description} [Progress: {q.progressCount}/{st.target_count}]";
                        }
                    }

                    var titleLabel = new Label { Text = title };
                    titleLabel.AddThemeColorOverride("font_color", new Color(0.95f, 0.85f, 0.4f));
                    cardStack.AddChild(titleLabel);

                    var descLabel = new Label { Text = stageDesc, AutowrapMode = TextServer.AutowrapMode.WordSmart };
                    cardStack.AddChild(descLabel);

                    // Render choices if available
                    if (def != null && q.currentStage >= 0 && q.currentStage < def.stages.Count)
                    {
                        var st = def.stages[q.currentStage];
                        foreach (var choice in st.choices)
                        {
                            string cId = choice.choice_id;
                            string sId = q.survivorId;
                            var choiceBtn = AshfallUiHelpers.MakeButton($"{choice.label} (+{choice.morale_delta} Morale)", () =>
                            {
                                _host.ChooseOption(sId, cId, 1, out _);
                            });
                            cardStack.AddChild(choiceBtn);
                        }
                    }

                    card.AddChild(cardStack);
                    _activeQuestsContainer.AddChild(card);
                }
            }

            // Populate Completed Quests
            foreach (Node child in _completedQuestsContainer.GetChildren())
                child.QueueFree();

            if (completedCount == 0)
            {
                var emptyLabel = new Label
                {
                    Text = "No completed character arcs recorded yet.",
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                };
                emptyLabel.AddThemeColorOverride("font_color", new Color(0.6f, 0.6f, 0.6f));
                _completedQuestsContainer.AddChild(emptyLabel);
            }
            else
            {
                foreach (var cq in _host.System.CompletedQuests)
                {
                    var card = new PanelContainer();
                    var cardStack = new VBoxContainer();
                    cardStack.AddThemeConstantOverride("separation", 4);

                    string title = cq.questId;
                    if (_host.System.Catalog.TryGetValue(cq.questId, out var def))
                    {
                        title = $"{def.title} ({cq.survivorId})";
                    }

                    var titleLabel = new Label { Text = title };
                    titleLabel.AddThemeColorOverride("font_color", new Color(0.4f, 0.9f, 0.5f));
                    cardStack.AddChild(titleLabel);

                    var statusLabel = new Label
                    {
                        Text = $"Status: {cq.status} | Resolved Day: {cq.resolvedDay} | Choices: {string.Join(", ", cq.selectedChoices)}",
                        AutowrapMode = TextServer.AutowrapMode.WordSmart
                    };
                    cardStack.AddChild(statusLabel);

                    card.AddChild(cardStack);
                    _completedQuestsContainer.AddChild(card);
                }
            }
        }
    }
}
