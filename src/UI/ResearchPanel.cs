// SPDX-License-Identifier: MIT
using System;
using System.Linq;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.Localization;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Research panel (Plan 35).
    /// Interactive research queue action surface. Shows active research with daily progress,
    /// available research with focusable START action, locked nodes with structured prerequisite reasons,
    /// completed nodes, and discovered knowledge. Wraps full 56-node catalog in a scroll container.
    /// </summary>
    public partial class ResearchPanel : Control
    {
        public event Action? OnClose;
        public event Action<string>? OnResearchStarted;

        private Label _lblActiveTitle;
        private VBoxContainer _activeContainer;
        private Label _lblAvailableTitle;
        private VBoxContainer _availableList;
        private Label _lblLockedTitle;
        private VBoxContainer _lockedList;
        private Label _lblCompletedTitle;
        private VBoxContainer _completedList;
        private Label _lblKnowledgeTitle;
        private VBoxContainer _knowledgeList;
        private Label _statusFeedbackLabel;

        private ResearchSystem? _research;
        private ResearchHostSession? _host;

        public bool IsBound => _research != null || _host != null;
        public int RenderedRowCount { get; private set; }

        public void Bind(ResearchSystem? research)
        {
            Bind(research, null);
        }

        public void Bind(ResearchHostSession? host)
        {
            Bind(host?.Engine, host);
        }

        public void Bind(ResearchSystem? research, ResearchHostSession? host)
        {
            if (_host != null)
                _host.StateChanged -= RefreshView;

            _research = research ?? host?.Engine;
            _host = host;

            if (_host != null)
                _host.StateChanged += RefreshView;

            RefreshView();
        }

        public void RefreshView()
        {
            if (_activeContainer == null || _availableList == null || _lockedList == null || _completedList == null || _knowledgeList == null)
                return;

            AshfallUiHelpers.EmptyChildren(_activeContainer);
            AshfallUiHelpers.EmptyChildren(_availableList);
            AshfallUiHelpers.EmptyChildren(_lockedList);
            AshfallUiHelpers.EmptyChildren(_completedList);
            AshfallUiHelpers.EmptyChildren(_knowledgeList);

            RenderedRowCount = 0;

            if (_research == null)
            {
                string offline = T("research.status.offline", "Offline — no research engine connected.");
                _activeContainer.AddChild(MakeDimLine(offline));
                _availableList.AddChild(MakeDimLine(offline));
                _lockedList.AddChild(MakeDimLine(offline));
                _completedList.AddChild(MakeDimLine(offline));
                _knowledgeList.AddChild(MakeDimLine(offline));
                if (_statusFeedbackLabel != null)
                    _statusFeedbackLabel.Text = T("research.status.offline", "Offline — no research engine connected.");
                return;
            }

            if (_statusFeedbackLabel != null)
            {
                _statusFeedbackLabel.Text = !string.IsNullOrEmpty(_host?.LastEvent)
                    ? _host.LastEvent
                    : F("research.status.active", "Research system active. Day {0}.", _research.State.currentDay);
            }

            // ── 1. ACTIVE RESEARCH ──
            var active = _research.GetActiveResearch();
            if (active != null)
            {
                int remaining = _research.GetDaysRemaining(active.id);
                var activeBox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);

                var title = new Label
                {
                    Text = $"{active.displayName} [{active.category.ToUpperInvariant()}]"
                };
                title.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeH2);
                title.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Hot));
                activeBox.AddChild(title);

                var desc = AshfallUiHelpers.MakeSmall(active.description);
                activeBox.AddChild(desc);

                string progressText = F("research.progress",
                    "Progress: Day {0} of {1} ({2} days remaining)",
                    _research.State.activeResearchDays, active.daysToComplete, remaining);
                var progressLabel = new Label { Text = progressText };
                progressLabel.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
                progressLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
                activeBox.AddChild(progressLabel);

                if (!string.IsNullOrEmpty(active.breakthroughItem))
                {
                    var btLabel = AshfallUiHelpers.MakeSmall(F("research.breakthrough_award",
                        "Breakthrough Award: {0}", active.breakthroughItem));
                    btLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
                    activeBox.AddChild(btLabel);
                }

                _activeContainer.AddChild(activeBox);
            }
            else
            {
                _activeContainer.AddChild(MakeDimLine("No active research in progress. Select an available node below to begin."));
            }

            // ── 2. AVAILABLE RESEARCH (Prerequisites met, unstarted) ──
            var availableNodes = _research.GetAvailableNodes();
            if (availableNodes.Count == 0)
            {
                _availableList.AddChild(MakeDimLine(active != null
                    ? T("research.empty.available_busy", "No other research available (queue slot occupied or all unlocked).")
                    : T("research.empty.available", "No research available to start.")));
            }
            else
            {
                foreach (var node in availableNodes)
                {
                    var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingMd);
                    row.SizeFlagsHorizontal = SizeFlags.ExpandFill;

                    var infoBox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
                    infoBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;

                    var nameLbl = new Label
                    {
                        Text = F("research.node.summary", "{0} [{1}] — {2} days",
                            node.displayName, node.category, node.daysToComplete)
                    };
                    nameLbl.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
                    nameLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
                    infoBox.AddChild(nameLbl);

                    if (!string.IsNullOrEmpty(node.breakthroughItem))
                    {
                        var btLbl = AshfallUiHelpers.MakeSmall(F("research.breakthrough",
                            "Breakthrough: {0}", node.breakthroughItem));
                        btLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
                        infoBox.AddChild(btLbl);
                    }
                    row.AddChild(infoBox);

                    string nodeId = node.id;
                    var startBtn = AshfallUiHelpers.MakeButton(T("research.action.start", "START RESEARCH"), () =>
                    {
                        HandleStartResearch(nodeId);
                    });
                    startBtn.CustomMinimumSize = new Vector2(160, 36);
                    row.AddChild(startBtn);

                    _availableList.AddChild(row);
                }
            }

            // ── 3. LOCKED / UPCOMING RESEARCH ──
            var lockedNodes = _research.GetLockedNodes();
            if (lockedNodes.Count == 0)
            {
                _lockedList.AddChild(MakeDimLine(T("research.empty.locked", "No locked research nodes remaining.")));
            }
            else
            {
                foreach (var node in lockedNodes)
                {
                    var itemBox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingXs);
                    itemBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;

                    var nameLbl = new Label
                    {
                        Text = F("research.node.summary", "{0} [{1}] — {2} days",
                            node.displayName, node.category, node.daysToComplete)
                    };
                    nameLbl.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
                    nameLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
                    itemBox.AddChild(nameLbl);

                    var elig = _research.GetEligibility(node.id);
                    var prereqNames = elig.MissingPrerequisites
                        .Select(p => _research.GetKnowledge(p)?.displayName ?? p)
                        .ToList();

                    string reason = prereqNames.Count > 0
                        ? F("research.requirements", "Requires: {0}", string.Join(", ", prereqNames))
                        : T("research.locked.prerequisites", "Locked: prerequisites incomplete");

                    var reasonLbl = AshfallUiHelpers.MakeSmall(reason);
                    reasonLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warning));
                    itemBox.AddChild(reasonLbl);

                    _lockedList.AddChild(itemBox);
                }
            }

            // ── 4. COMPLETED RESEARCH ──
            var completedNodes = _research.GetCompletedNodes();
            if (completedNodes.Count == 0)
            {
                _completedList.AddChild(MakeDimLine(T("research.empty.completed", "No completed research.")));
            }
            else
            {
                foreach (var node in completedNodes)
                {
                    string text = F("research.completed", "{0} [{1}] — Complete",
                        node.displayName, node.category);
                    if (!string.IsNullOrEmpty(node.breakthroughItem))
                        text += " " + F("research.completed.breakthrough",
                            "(Breakthrough: {0})", node.breakthroughItem);

                    AddRow(_completedList, text, DesignTheme.Pale);
                }
            }

            // ── 5. DISCOVERED KNOWLEDGE (Maintains Gate 5 lifecycle count parity) ──
            int unlockedCount = 0;
            foreach (var kv in _research.Catalog.OrderBy(k => k.Key, StringComparer.Ordinal))
            {
                if (!_research.IsManualUnlocked(kv.Key)) continue;
                AddRow(_knowledgeList, F("research.knowledge.row", "{0} — {1}",
                    kv.Value.displayName, kv.Value.category), DesignTheme.Lethe);
                unlockedCount++;
            }
            RenderedRowCount = unlockedCount;
            if (RenderedRowCount == 0)
                _knowledgeList.AddChild(MakeDimLine(T("research.empty.knowledge", "No knowledge unlocked yet.")));
        }

        private void HandleStartResearch(string nodeId)
        {
            if (_research == null) return;

            bool ok;
            if (_host != null)
            {
                ok = _host.TryStart(nodeId);
            }
            else
            {
                ok = _research.StartResearch(nodeId, _research.State.currentDay);
            }

            if (!ok)
            {
                var elig = _research.GetEligibility(nodeId);
                string failMsg = _host != null
                    ? _host.FormatFailureCode(elig)
                    : $"Cannot start: {elig.Code}";
                if (_statusFeedbackLabel != null)
                    _statusFeedbackLabel.Text = failMsg;
            }
            else
            {
                OnResearchStarted?.Invoke(nodeId);
            }
            RefreshView();
        }

        private void AddRow(VBoxContainer parent, string text, (float r, float g, float b, float a) col)
        {
            var label = new Label { Text = text };
            label.CustomMinimumSize = new Vector2(400, 0);
            label.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
            label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(col));
            parent.AddChild(label);
        }

        private Label MakeDimLine(string text)
        {
            var l = new Label { Text = text };
            l.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
            l.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
            return l;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.04f, 0.05f, 0.94f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var container = new CenterContainer();
            container.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(container);

            var panelCard = AshfallUiHelpers.MakeCardFrame(
                T("research.card.title", "RESEARCH & TECHNOLOGY"),
                T("research.card.subtitle", "R&D QUEUE · DISCOVERED KNOWLEDGE · BREAKTHROUGHS"));
            panelCard.CustomMinimumSize = new Vector2(960, 720);
            container.AddChild(panelCard);

            var margin = panelCard.GetChild<MarginContainer>(0);
            var mainVBox = margin.GetChild<VBoxContainer>(0);

            var title = AshfallUiHelpers.MakeTitle(T("research.title",
                "RESEARCH & TECHNOLOGY // QUEUE"), DesignTheme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            mainVBox.AddChild(title);

            mainVBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Scroll container wrapping all research sections
            var scroll = new ScrollContainer
            {
                CustomMinimumSize = new Vector2(900, 480),
                SizeFlagsVertical = SizeFlags.ExpandFill,
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
            };
            mainVBox.AddChild(scroll);

            var scrollContent = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
            scrollContent.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(scrollContent);

            // 1. Active research
            _lblActiveTitle = AshfallUiHelpers.MakeSectionHeader(T("research.section.active", "ACTIVE RESEARCH"));
            scrollContent.AddChild(_lblActiveTitle);
            _activeContainer = new VBoxContainer();
            _activeContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            scrollContent.AddChild(_activeContainer);

            scrollContent.AddChild(AshfallUiHelpers.MakeSeparator());

            // 2. Available research
            _lblAvailableTitle = AshfallUiHelpers.MakeSectionHeader(T("research.section.available", "AVAILABLE RESEARCH"));
            scrollContent.AddChild(_lblAvailableTitle);
            _availableList = new VBoxContainer();
            _availableList.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            scrollContent.AddChild(_availableList);

            scrollContent.AddChild(AshfallUiHelpers.MakeSeparator());

            // 3. Locked / Upcoming
            _lblLockedTitle = AshfallUiHelpers.MakeSectionHeader(T("research.section.locked", "LOCKED / UPCOMING RESEARCH"));
            scrollContent.AddChild(_lblLockedTitle);
            _lockedList = new VBoxContainer();
            _lockedList.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            scrollContent.AddChild(_lockedList);

            scrollContent.AddChild(AshfallUiHelpers.MakeSeparator());

            // 4. Completed
            _lblCompletedTitle = AshfallUiHelpers.MakeSectionHeader(T("research.section.completed", "COMPLETED RESEARCH"));
            scrollContent.AddChild(_lblCompletedTitle);
            _completedList = new VBoxContainer();
            _completedList.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            scrollContent.AddChild(_completedList);

            scrollContent.AddChild(AshfallUiHelpers.MakeSeparator());

            // 5. Discovered knowledge
            _lblKnowledgeTitle = AshfallUiHelpers.MakeSectionHeader(T("research.section.knowledge", "DISCOVERED KNOWLEDGE"));
            scrollContent.AddChild(_lblKnowledgeTitle);
            _knowledgeList = new VBoxContainer();
            _knowledgeList.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            scrollContent.AddChild(_knowledgeList);

            mainVBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Status feedback strip
            _statusFeedbackLabel = AshfallUiHelpers.MakeSmall(T("research.status.ready", "Ready."));
            _statusFeedbackLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            mainVBox.AddChild(_statusFeedbackLabel);

            var bottomHBox = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingMd);
            bottomHBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            var btnClose = AshfallUiHelpers.MakeButton(T("ui.common.close_short",
                "CLOSE [Esc]"), () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            btnClose.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            bottomHBox.AddChild(btnClose);

            mainVBox.AddChild(bottomHBox);
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
            QueueRedraw();
        }

        public void Close()
        {
            Visible = false;
        }

        public void Unbind()
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
                _host = null;
            }
            _research = null;
            RefreshView();
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;

            if (AshfallInputActions.IsCloseOrCancel(@event))
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }

        private static string T(string key, string fallback) =>
            AshfallLocalization.Tr(key, fallback);

        private static string F(string key, string fallback, params object[] args)
        {
            AshfallLocalization.Initialize();
            string translated = AshfallLocalization.Tr(key, fallback);
            try
            {
                return string.Format(System.Globalization.CultureInfo.InvariantCulture, translated, args);
            }
            catch (FormatException)
            {
                return fallback;
            }
        }
    }
}
