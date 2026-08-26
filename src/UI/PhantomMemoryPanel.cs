using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Phantom Memory & Relic Triggers Management Interface.
    /// Manages psychological reactions to scavenged personal artifacts, photographs,
    /// military tags, and letters, determining motivation surges vs emotional breakdowns.
    /// </summary>
    public partial class PhantomMemoryPanel : Control
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _survivorList = null!;
        private VBoxContainer _relicInspector = null!;
        private VBoxContainer _triggerLogContainer = null!;
        private Label _eventLogLabel = null!;

        private PhantomMemoryHostSession? _host;
        private string _selectedSurvivorId = "survivor_gunner_mikhail";
        private string _selectedItemCategory = "military";

        public bool IsBound => _host != null;

        public void Bind(PhantomMemoryHostSession session)
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
            }
            _host = session;
            if (_host != null)
            {
                _host.StateChanged += RefreshView;
            }
            RefreshView();
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.04f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            _shell = new AshfallDashboardShell("SYS: PHANTOM MEMORY & RELIC TRIGGERS // PSYCH MATRIX", minWidth: 1040, minHeight: 680);
            center.AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("survivors", "TRACKED SURVIVORS", "0", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("rules", "MEMORY RULES", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("triggers", "PSYCH TRIGGERS", "ACTIVE", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("nostalgia", "NOSTALGIA RISK", "ELEVATED", AshfallMetricCard.Criticality.Caution, minWidth: 130);
            _statusRail.AddCard("status", "RELIC SCANNER", "ONLINE", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _shell.AttachHeaderCloseButton("CLOSE [Esc]", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            // 3-Column Layout
            var gridRow = new HBoxContainer();
            gridRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            gridRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            gridRow.SizeFlagsVertical = SizeFlags.ExpandFill;

            // Column 1: Survivor Roster
            var leftPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            leftPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftPanel.SizeFlagsStretchRatio = 0.95f;
            var leftMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            leftPanel.AddChild(leftMargin);
            var leftVbox = new VBoxContainer();
            leftVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            leftMargin.AddChild(leftVbox);
            leftVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("COHORT BACKGROUNDS"));
            var leftScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _survivorList = new VBoxContainer();
            _survivorList.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _survivorList.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftScroll.AddChild(_survivorList);
            leftVbox.AddChild(leftScroll);
            gridRow.AddChild(leftPanel);

            // Column 2: Relic Scanner & Trigger Actions
            var centerPanel = AshfallUiHelpers.MakePanel(minWidth: 380);
            centerPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerPanel.SizeFlagsStretchRatio = 1.2f;
            var centerMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            centerPanel.AddChild(centerMargin);
            var centerVbox = new VBoxContainer();
            centerVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            centerMargin.AddChild(centerVbox);
            centerVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("SCAVENGED RELIC & PROBING"));
            var centerScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _relicInspector = new VBoxContainer();
            _relicInspector.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _relicInspector.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerScroll.AddChild(_relicInspector);
            centerVbox.AddChild(centerScroll);
            gridRow.AddChild(centerPanel);

            // Column 3: Telemetry & Memory Log
            var rightPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            rightPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightPanel.SizeFlagsStretchRatio = 0.95f;
            var rightMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            rightPanel.AddChild(rightMargin);
            var rightVbox = new VBoxContainer();
            rightVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            rightMargin.AddChild(rightVbox);
            rightVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("FLASHBACK & MOTIVATION LOG"));
            var rightScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _triggerLogContainer = new VBoxContainer();
            _triggerLogContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _triggerLogContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightScroll.AddChild(_triggerLogContainer);
            rightVbox.AddChild(rightScroll);

            rightVbox.AddChild(AshfallUiHelpers.MakeSeparator());
            _eventLogLabel = AshfallUiHelpers.MakeMetadata("No recent phantom triggers.");
            _eventLogLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            rightVbox.AddChild(_eventLogLabel);

            gridRow.AddChild(rightPanel);

            _shell.SetContent(gridRow);
            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
            QueueRedraw();
        }

        public void RefreshView()
        {
            if (_host == null || _statusRail == null) return;

            AshfallUiHelpers.EmptyChildren(_survivorList);
            AshfallUiHelpers.EmptyChildren(_relicInspector);
            AshfallUiHelpers.EmptyChildren(_triggerLogContainer);

            var survivors = _host.DemoSurvivors;
            int survivorCount = survivors.Count;

            _statusRail.Set("survivors", survivorCount.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("rules", "7 REGISTERED", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("triggers", "ACTIVE", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("nostalgia", "ELEVATED", AshfallMetricCard.Criticality.Caution);
            _statusRail.Set("status", "STANDBY", AshfallMetricCard.Criticality.Normal);

            if (!string.IsNullOrEmpty(_host.LastEvent))
            {
                _eventLogLabel.Text = _host.LastEvent;
            }

            // Populate Survivors
            foreach (var sv in survivors)
            {
                var card = AshfallUiHelpers.MakePanel();
                var cardMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingXs);
                card.AddChild(cardMargin);
                var cardVbox = new VBoxContainer();
                cardVbox.AddThemeConstantOverride("separation", 3);
                cardMargin.AddChild(cardVbox);

                var headerRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                headerRow.AddChild(AshfallUiHelpers.MakeBadgeIcon("badge_somatization", 18));
                var nameLbl = AshfallUiHelpers.MakeBody(sv.displayName);
                nameLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                headerRow.AddChild(nameLbl);
                cardVbox.AddChild(headerRow);

                var bgLbl = AshfallUiHelpers.MakeMono($"BACKGROUND: [{sv.backgroundId.ToUpperInvariant()}]");
                bgLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Lethe));
                cardVbox.AddChild(bgLbl);

                var selectBtn = AshfallUiHelpers.MakeButton($"INSPECT // {sv.survivorId}", () =>
                {
                    _selectedSurvivorId = sv.survivorId;
                    RefreshView();
                });
                selectBtn.CustomMinimumSize = new Vector2(0, 24);
                cardVbox.AddChild(selectBtn);

                _survivorList.AddChild(card);
            }

            // Relic Scanner & Actions
            var curSv = survivors.FirstOrDefault(s => s.survivorId == _selectedSurvivorId) ?? survivors[0];
            _relicInspector.AddChild(AshfallUiHelpers.MakeSectionHeader($"SURVIVOR: {curSv.displayName.ToUpperInvariant()}"));
            _relicInspector.AddChild(AshfallUiHelpers.MakeDataRow("Survivor ID", curSv.survivorId, AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            _relicInspector.AddChild(AshfallUiHelpers.MakeDataRow("Pre-War Background", curSv.backgroundId, AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
            _relicInspector.AddChild(AshfallUiHelpers.MakeDataRow("Selected Relic Item", _selectedItemCategory.ToUpperInvariant(), AshfallUiHelpers.ToColor(DesignTheme.Warm)));

            _relicInspector.AddChild(AshfallUiHelpers.MakeSeparator());
            _relicInspector.AddChild(AshfallUiHelpers.MakeSubsectionHeader("PRESENT RELIC ARTIFACT"));

            var itemOptions = new[]
            {
                new { cat = "military", name = "Dog Tags / Military Insignia" },
                new { cat = "medical", name = "Vintage Stethoscope / Surgical Tools" },
                new { cat = "correspondence", name = "Handwritten Pre-War Letter" },
                new { cat = "photograph", name = "Faded Family Photograph" },
                new { cat = "personal_item", name = "Pre-War Pocket Watch / Remains" }
            };

            foreach (var opt in itemOptions)
            {
                var btnPresent = AshfallUiHelpers.MakeButton($"SCAVENGE: {opt.name.ToUpperInvariant()}", () =>
                {
                    _selectedItemCategory = opt.cat;
                    string res = _host.ScavengeItem(curSv.survivorId, opt.cat);
                    RefreshView();
                });
                _relicInspector.AddChild(btnPresent);
            }

            // Log / Summary
            _triggerLogContainer.AddChild(AshfallUiHelpers.MakeSectionHeader("PSYCHOLOGICAL FLASHBACK LOG"));
            _triggerLogContainer.AddChild(AshfallUiHelpers.MakeBody("Scavenged artifacts carry emotional weight from before the war. Depending on background, an item may trigger a surge of resolve (+Morale) or traumatic grief (-Resolve)."));
            if (!string.IsNullOrEmpty(_host.LastEvent))
            {
                _triggerLogContainer.AddChild(AshfallUiHelpers.MakeSeparator());
                _triggerLogContainer.AddChild(AshfallUiHelpers.MakeCritical($"LATEST TRIGGER:\n{_host.LastEvent}"));
            }
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                OnClose?.Invoke();
                Visible = false;
                GetViewport().SetInputAsHandled();
            }
        }

        public override void _ExitTree()
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
            }
            base._ExitTree();
        }
    }
}
