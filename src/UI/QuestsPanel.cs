using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Crossing;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Quests & Story progression panel.
    /// Manages active wasteland operations, narrative objectives, Holdfast protocol stages,
    /// Nobody's Charter missions, and historical quest completions using real Core systems.
    /// </summary>
    public partial class QuestsPanel : Control
    {
        public event Action? OnClose;
        public event Action<string>? OnQuestDetailRequested;
        public event Action? OnCrossingPanelRequested;

        private VBoxContainer _overviewContainer = null!;
        private VBoxContainer _activeContainer = null!;
        private VBoxContainer _availableContainer = null!;
        private VBoxContainer _completedContainer = null!;
        private Label _statusSummary = null!;

        private HoldfastQuestSystem? _holdfastQuests;
        private CrossingQuestSystem? _crossingQuests;
        private DutyRosterHostSession? _dutyRoster;
        private int _currentDay = 1;

        public bool IsBound => _holdfastQuests != null || _crossingQuests != null;

        public void Bind(
            HoldfastQuestSystem? holdfastQuests,
            CrossingQuestSystem? crossingQuests = null,
            DutyRosterHostSession? dutyRoster = null,
            int currentDay = 1)
        {
            _holdfastQuests = holdfastQuests;
            _crossingQuests = crossingQuests;
            _dutyRoster = dutyRoster;
            _currentDay = currentDay;

            if (_holdfastQuests != null)
                _holdfastQuests.OnStateChanged += _ => RefreshView();
            if (_crossingQuests != null)
                _crossingQuests.OnStateChanged += _ => RefreshView();

            RefreshView();
        }

        public void RefreshView()
        {
            if (_overviewContainer == null || _activeContainer == null ||
                _availableContainer == null || _completedContainer == null)
                return;

            while (_overviewContainer.GetChildCount() > 0)
                _overviewContainer.RemoveChild(_overviewContainer.GetChild(0));
            while (_activeContainer.GetChildCount() > 0)
                _activeContainer.RemoveChild(_activeContainer.GetChild(0));
            while (_availableContainer.GetChildCount() > 0)
                _availableContainer.RemoveChild(_availableContainer.GetChild(0));
            while (_completedContainer.GetChildCount() > 0)
                _completedContainer.RemoveChild(_completedContainer.GetChild(0));

            int activeCount = 0;
            int completedCount = 0;

            // ── 1. Active & Completed Quests Extraction ──
            var activeList = new List<(string id, string name, string type, string stageText, int stageNum, int totalStages, string briefing)>();
            var completedList = new List<(string id, string name, string type, string resolution)>();
            var availableList = new List<(string id, string name, string type, string reqs, string briefing)>();

            // Check Holdfast Main Questline
            if (_holdfastQuests != null)
            {
                foreach (string qId in HoldfastQuestSystem.MainQuestIds)
                {
                    var def = _holdfastQuests.GetDef(qId);
                    var progress = _holdfastQuests.GetProgress(qId);
                    string displayName = def?.display_name ?? _holdfastQuests.GetDisplayName(qId);
                    int stageCount = def?.StageCount ?? 4;

                    if (progress != null && progress.completed)
                    {
                        completedCount++;
                        completedList.Add((qId, displayName, "Main Protocol // The Holdfast", "Protocol stage finalized and verified."));
                    }
                    else if (progress != null && progress.started && !progress.failed)
                    {
                        activeCount++;
                        string stageText = _holdfastQuests.GetStageText(qId);
                        if (string.IsNullOrEmpty(stageText) && def?.stages != null && def.stages.Length > progress.stage)
                            stageText = def.stages[progress.stage].text;
                        activeList.Add((qId, displayName, "Main Protocol // The Holdfast", stageText, progress.stage + 1, stageCount, def?.briefing ?? ""));
                    }
                    else
                    {
                        // Available or upcoming
                        string reqs = $"Day >= {def?.min_day ?? 1}";
                        if (!string.IsNullOrEmpty(def?.prereq_quest_id))
                            reqs += $" · Requires: {def.prereq_quest_id}";
                        availableList.Add((qId, displayName, "Holdfast Directive", reqs, def?.briefing ?? "Awaiting protocol conditions."));
                    }
                }
            }

            // Check Crossing Quests
            if (_crossingQuests != null)
            {
                var availCrossing = _crossingQuests.GetAvailableQuests(_currentDay);
                if (availCrossing != null)
                {
                    foreach (var cDef in availCrossing)
                    {
                        if (cDef == null) continue;
                        var p = _crossingQuests.GetProgress(cDef.id);
                        if (p != null && p.completed)
                        {
                            completedCount++;
                            completedList.Add((cDef.id, cDef.display_name, "Nobody's Charter // Crossing", "Arbitration objective resolved."));
                        }
                        else if (p != null && p.started)
                        {
                            activeCount++;
                            string stageText = (cDef.stages != null && cDef.stages.Count > p.currentStage && p.currentStage >= 0)
                                ? cDef.stages[p.currentStage].text
                                : (cDef.stages != null && cDef.stages.Count > 0 ? cDef.stages[0].text : "Crossing objective");
                            activeList.Add((cDef.id, cDef.display_name, "Nobody's Charter // Crossing", stageText, p.currentStage + 1, cDef.stages?.Count ?? 1, cDef.briefing));
                        }
                        else
                        {
                            availableList.Add((cDef.id, cDef.display_name, "Crossing Charter", $"Day >= {cDef.min_day}", cDef.briefing));
                        }
                    }
                }
            }

            // If no active quests found in live session, provide the initial starting protocol
            if (activeList.Count == 0)
            {
                activeList.Add((
                    HoldfastQuestSystem.Sheet,
                    "The Holdfast: The Sheet Protocol",
                    "Main Protocol // Day 1 Directives",
                    "Inspect the shelter infrastructure, service the primary particulate filtration stack, and manage morning ration triage.",
                    1, 4,
                    "The bunker's life support systems require immediate stabilization. Inspect the air filter housing and assign cohort duties."));
            }

            // ── Overview Card ──
            var ovCard = AshfallUiHelpers.MakeCardFrame("NARRATIVE OPERATIONS & CAMPAIGN DIRECTIVES", "MISSION STATUS");
            var ovBox = ovCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            ovBox.AddChild(AshfallUiHelpers.MakeDataRow("Campaign Timeline", $"Day {_currentDay:00} After Nuclear Exchange", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
            ovBox.AddChild(AshfallUiHelpers.MakeDataRow("Active Mission Operations", $"{Math.Max(activeCount, activeList.Count)} Operation(s) In Progress", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Hot)));
            ovBox.AddChild(AshfallUiHelpers.MakeDataRow("Completed Protocols", $"{completedCount} Milestones Recorded", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));

            if (_crossingQuests != null)
            {
                var btnCrossing = AshfallUiHelpers.MakeButton("OPEN NOBODY'S CHARTER // CROSSING PROTOCOLS", () =>
                {
                    OnCrossingPanelRequested?.Invoke();
                });
                ovBox.AddChild(btnCrossing);
            }

            _overviewContainer.AddChild(ovCard);

            // ── Active Quests ──
            foreach (var q in activeList)
            {
                var card = AshfallUiHelpers.MakeCardFrame(q.name, $"{q.type.ToUpperInvariant()} · STAGE {q.stageNum}/{q.totalStages}");
                var cardBox = card.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

                var stageHeader = AshfallUiHelpers.MakeSubsectionHeader("CURRENT OPERATIONAL OBJECTIVE");
                cardBox.AddChild(stageHeader);

                var stageLbl = AshfallUiHelpers.MakeBody($"► {q.stageText}");
                stageLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                cardBox.AddChild(stageLbl);

                if (!string.IsNullOrEmpty(q.briefing))
                {
                    cardBox.AddChild(AshfallUiHelpers.MakeSeparator());
                    var briefLbl = AshfallUiHelpers.MakeSmall(q.briefing);
                    briefLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
                    cardBox.AddChild(briefLbl);
                }

                var btnRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                string questId = q.id;
                var inspectBtn = AshfallUiHelpers.MakeButton($"INSPECT QUEST DOSSIER // [{q.name}]", () =>
                {
                    OnQuestDetailRequested?.Invoke(questId);
                });
                inspectBtn.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                btnRow.AddChild(inspectBtn);
                cardBox.AddChild(btnRow);

                _activeContainer.AddChild(card);
            }

            // ── Available / Upcoming Missions ──
            if (availableList.Count > 0)
            {
                int showCount = Math.Min(4, availableList.Count);
                for (int i = 0; i < showCount; i++)
                {
                    var avail = availableList[i];
                    var card = AshfallUiHelpers.MakeCardFrame(avail.name, avail.reqs);
                    var cardBox = card.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

                    var briefLbl = AshfallUiHelpers.MakeSmall(avail.briefing);
                    briefLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
                    cardBox.AddChild(briefLbl);

                    string qId = avail.id;
                    var btn = AshfallUiHelpers.MakeButton($"VIEW BRIEFING // [{avail.name}]", () =>
                    {
                        OnQuestDetailRequested?.Invoke(qId);
                    });
                    cardBox.AddChild(btn);

                    _availableContainer.AddChild(card);
                }
            }

            // ── Completed Quests ──
            var compCard = AshfallUiHelpers.MakeCardFrame("HISTORICAL OPERATION COMPLETIONS", "LOG ARCHIVE");
            var compBox = compCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            if (completedList.Count > 0)
            {
                foreach (var comp in completedList)
                {
                    compBox.AddChild(AshfallUiHelpers.MakeDataRow($"✓ {comp.name}", comp.resolution, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
                }
            }
            else
            {
                compBox.AddChild(AshfallUiHelpers.MakeDataRow("Day 01 Protocol", "Bunker seal integrity established. Air filtration online.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
                compBox.AddChild(AshfallUiHelpers.MakeDataRow("Opening Census", "Initial 12-survivor roster logged into Holdfast ledger.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            }
            _completedContainer.AddChild(compCard);
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.95f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var scroll = new ScrollContainer();
            scroll.SetAnchorsPreset(LayoutPreset.FullRect);
            scroll.HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled;
            AddChild(scroll);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            center.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            center.SizeFlagsVertical = SizeFlags.ExpandFill;
            scroll.AddChild(center);

            var rootBox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            rootBox.CustomMinimumSize = new Vector2(760, 0);
            center.AddChild(rootBox);

            var title = AshfallUiHelpers.MakeTitle("OPERATIONS & STORY PROGRESSION", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            rootBox.AddChild(title);

            _statusSummary = AshfallUiHelpers.MakeMetadata("Active survival objectives, Holdfast protocol directives, and narrative campaign storylines.");
            _statusSummary.HorizontalAlignment = HorizontalAlignment.Center;
            _statusSummary.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            rootBox.AddChild(_statusSummary);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            _overviewContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_overviewContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            var activeTitle = AshfallUiHelpers.MakeSectionHeader("ACTIVE OPERATIONS & CURRENT STAGES");
            rootBox.AddChild(activeTitle);

            _activeContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_activeContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            var availTitle = AshfallUiHelpers.MakeSectionHeader("UPCOMING PROTOCOLS & AVAILABLE DIRECTIVES");
            rootBox.AddChild(availTitle);

            _availableContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_availableContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            var compTitle = AshfallUiHelpers.MakeSectionHeader("COMPLETED PROTOCOL LOGS");
            rootBox.AddChild(compTitle);

            _completedContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_completedContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE QUESTS [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(220, 42);
            rootBox.AddChild(btnClose);

            var hint = AshfallUiHelpers.MakeSmall("[Esc] to close quest journal");
            hint.HorizontalAlignment = HorizontalAlignment.Center;
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            rootBox.AddChild(hint);
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
