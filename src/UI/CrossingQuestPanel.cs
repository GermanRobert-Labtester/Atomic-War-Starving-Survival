using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Crossing;
using Ashfall.Core.UI;
using CoreTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Nobody's Charter (Exp 04) Crossing quest panel.
    /// Presents the active Crossing quest, stage objectives, narrative briefing,
    /// available interaction choices, and gate/lock reasons. All state mutations go
    /// through ExpansionHostSession; this panel is purely presentational.
    /// </summary>
    public partial class CrossingQuestPanel : Control
    {
        public event Action? OnClose;

        private ExpansionHostSession? _expansions;
        private VouchAccessSystem? _vouch;
        private int _currentDay = 1;

        // ── UI nodes ──────────────────────────────────────────────────
        private Label _gateStatus = null!;
        private VBoxContainer _activeQuestContainer = null!;
        private VBoxContainer _availableQuestsContainer = null!;
        private VBoxContainer _completedQuestsContainer = null!;
        private Label _emptyState = null!;

        public bool IsBound => _expansions != null;

        // ── Bind ──────────────────────────────────────────────────────

        public void Bind(ExpansionHostSession expansions, VouchAccessSystem? vouch, int currentDay)
        {
            if (_expansions != null)
            {
                _expansions.CrossingQuests.OnStateChanged -= OnStateChangedHandler;
                if (_vouch != null)
                    _vouch.OnStateChanged -= OnVouchChangedHandler;
            }

            _expansions = expansions;
            _vouch = vouch ?? _expansions?.Vouch;
            _currentDay = currentDay;

            if (_expansions != null)
            {
                _expansions.CrossingQuests.OnStateChanged += OnStateChangedHandler;
                if (_vouch != null)
                    _vouch.OnStateChanged += OnVouchChangedHandler;
            }

            RefreshView();
        }

        private void OnStateChangedHandler(CrossingQuestSystemState state) => RefreshView();
        private void OnVouchChangedHandler(VouchAccessSystemState state) => RefreshView();

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        // ── Godot lifecycle ───────────────────────────────────────────

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

            var rootBox = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingMd);
            rootBox.CustomMinimumSize = new Vector2(760, 0);
            center.AddChild(rootBox);

            // ── Header ─────────────────────────────────────────────
            var header = AshfallUiHelpers.MakeTitle("NOBODY'S CHARTER // CROSSING PROTOCOLS", CoreTheme.FontSizeH1);
            header.HorizontalAlignment = HorizontalAlignment.Center;
            rootBox.AddChild(header);

            var subtitle = AshfallUiHelpers.MakeSmall("Active obligations under the Crossing. The ledger is honest. The gate is patient.");
            subtitle.HorizontalAlignment = HorizontalAlignment.Center;
            subtitle.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
            rootBox.AddChild(subtitle);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Gate status bar ─────────────────────────────────────
            _gateStatus = AshfallUiHelpers.MakeMono("GATE: —");
            _gateStatus.HorizontalAlignment = HorizontalAlignment.Center;
            _gateStatus.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Entropy));
            rootBox.AddChild(_gateStatus);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Active Quest Container ──────────────────────────────
            _activeQuestContainer = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingSm);
            rootBox.AddChild(_activeQuestContainer);

            // ── Empty State ─────────────────────────────────────────
            _emptyState = AshfallUiHelpers.MakeBody("No active Crossing protocol. Return when you hold a vouch or the gate day arrives.");
            _emptyState.HorizontalAlignment = HorizontalAlignment.Center;
            _emptyState.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
            _emptyState.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            rootBox.AddChild(_emptyState);

            // ── Available Quests Container ──────────────────────────
            _availableQuestsContainer = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingSm);
            rootBox.AddChild(_availableQuestsContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Completed Quests Container ──────────────────────────
            _completedQuestsContainer = AshfallUiHelpers.MakeVBox(CoreTheme.SpacingSm);
            rootBox.AddChild(_completedQuestsContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Footer buttons ──────────────────────────────────────
            var btnRow = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingMd);
            btnRow.Alignment = BoxContainer.AlignmentMode.Center;

            var btnClose = AshfallUiHelpers.MakeButton("RETURN TO DASHBOARD [Esc]", () => OnClose?.Invoke(), false);
            btnClose.CustomMinimumSize = new Vector2(260, 42);
            btnRow.AddChild(btnClose);

            rootBox.AddChild(btnRow);

            var hint = AshfallUiHelpers.MakeSmall("Press [Esc] to return");
            hint.HorizontalAlignment = HorizontalAlignment.Center;
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
            rootBox.AddChild(hint);
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

        // ── View refresh ──────────────────────────────────────────────

        public void RefreshView()
        {
            if (_activeQuestContainer == null) return; // _Ready not yet called

            ClearContainer(_activeQuestContainer);
            ClearContainer(_availableQuestsContainer);
            ClearContainer(_completedQuestsContainer);

            if (_expansions == null)
            {
                _emptyState.Text = "Panel not bound to session.";
                _emptyState.Visible = true;
                return;
            }

            // Gate status
            bool gateOpen = _vouch?.HasAccess ?? (_expansions.Vouch?.HasAccess ?? false);
            string vouchedBy = _vouch?.VouchedBy ?? (_expansions.Vouch?.VouchedBy ?? "");
            string vouchDesc = string.IsNullOrEmpty(vouchedBy) ? "" : $" (Vouched by {vouchedBy})";
            string gateLabel = gateOpen
                ? $"GATE: OPEN — Vouch on ledger{vouchDesc}"
                : "GATE: CLOSED — Vouch required to cross the viaduct";

            _gateStatus.Text = gateLabel;
            _gateStatus.AddThemeColorOverride("font_color",
                gateOpen
                    ? AshfallUiHelpers.ToColor(CoreTheme.Warm)
                    : AshfallUiHelpers.ToColor(CoreTheme.Entropy));

            var catalog = _expansions.CrossingQuests.Catalog;
            CrossingQuestDef? activeDef = null;
            CrossingQuestProgress? activeProgress = null;
            var availableList = new List<(CrossingQuestDef def, bool locked, string reason)>();
            var completedList = new List<(CrossingQuestDef def, CrossingQuestProgress prog)>();

            for (int i = 0; i < catalog.Count; i++)
            {
                var def = catalog[i];
                if (def == null) continue;
                var prog = _expansions.CrossingQuests.GetProgress(def.id);

                if (prog != null && prog.completed)
                {
                    completedList.Add((def, prog));
                }
                else if (prog != null && prog.started && !prog.failed)
                {
                    if (activeDef == null)
                    {
                        activeDef = def;
                        activeProgress = prog;
                    }
                }
                else
                {
                    // Evaluate availability and lock reasons
                    bool locked = false;
                    string reason = "";

                    if (prog != null && prog.failed)
                    {
                        locked = true;
                        reason = "Protocol failed and closed on the record.";
                    }
                    else if (def.min_day > _currentDay)
                    {
                        locked = true;
                        reason = $"Available on Day {def.min_day} (Current: Day {_currentDay})";
                    }
                    else if (!string.IsNullOrEmpty(def.prereq_quest_id) && !_expansions.IsCrossingQuestCompleted(def.prereq_quest_id))
                    {
                        var prereqDef = _expansions.CrossingQuests.GetDef(def.prereq_quest_id);
                        string prereqName = prereqDef?.display_name ?? def.prereq_quest_id;
                        locked = true;
                        reason = $"Requires completion of: {prereqName}";
                    }
                    else if (def.id != CrossingQuestSystem.OpeningQuest && !gateOpen && !_expansions.IsCrossingQuestCompleted(CrossingQuestSystem.OpeningQuest))
                    {
                        locked = true;
                        reason = "Requires gate vouch access or opening charter resolution.";
                    }

                    availableList.Add((def, locked, reason));
                }
            }

            // ── Render Active Quest ──────────────────────────────────
            if (activeDef != null && activeProgress != null)
            {
                _emptyState.Visible = false;
                var activeCard = BuildActiveQuestCard(activeDef, activeProgress);
                _activeQuestContainer.AddChild(activeCard);
            }
            else
            {
                _emptyState.Text = "No active Crossing protocol in progress. Review available charters below.";
                _emptyState.Visible = availableList.Count == 0;
            }

            // ── Render Available Protocols ───────────────────────────
            if (availableList.Count > 0)
            {
                var availHeader = AshfallUiHelpers.MakeSectionHeader("AVAILABLE CROSSING PROTOCOLS");
                _availableQuestsContainer.AddChild(availHeader);

                foreach (var item in availableList)
                {
                    var card = BuildAvailableQuestCard(item.def, item.locked, item.reason);
                    _availableQuestsContainer.AddChild(card);
                }
            }

            // ── Render Completed Protocols ───────────────────────────
            if (completedList.Count > 0)
            {
                var compCard = AshfallUiHelpers.MakeCardFrame("RESOLVED CROSSING CHARTERS", "LEDGER ARCHIVE");
                var compBox = compCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

                foreach (var item in completedList)
                {
                    string resolution = string.IsNullOrEmpty(item.prog.chosenChoiceId)
                        ? "Charter resolved and recorded."
                        : $"Resolved · Choice recorded [{item.prog.chosenChoiceId}]";
                    compBox.AddChild(AshfallUiHelpers.MakeDataRow($"✓ {item.def.display_name}", resolution, AshfallUiHelpers.ToColor(CoreTheme.Pale)));
                }

                _completedQuestsContainer.AddChild(compCard);
            }
        }

        // ── Active Quest Card ─────────────────────────────────────────

        private Control BuildActiveQuestCard(CrossingQuestDef def, CrossingQuestProgress prog)
        {
            int totalStages = def.stages?.Count ?? 0;
            int currentStageIdx = prog.currentStage;
            string subtitle = $"TYPE: {def.type?.ToUpperInvariant() ?? "EXPEDITION"} · STAGE {currentStageIdx + 1}/{Math.Max(1, totalStages)} · TARGET: {def.target_location_id ?? "—"}";

            var card = AshfallUiHelpers.MakeCardFrame(def.display_name, subtitle);
            var cardBox = card.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            // Briefing
            if (!string.IsNullOrEmpty(def.briefing))
            {
                var briefHeader = AshfallUiHelpers.MakeSubsectionHeader("DIRECTIVE BRIEFING");
                cardBox.AddChild(briefHeader);

                var briefLbl = AshfallUiHelpers.MakeBody(def.briefing);
                briefLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Pale));
                cardBox.AddChild(briefLbl);
                cardBox.AddChild(AshfallUiHelpers.MakeSeparator());
            }

            // Stage Objectives
            var stageHeader = AshfallUiHelpers.MakeSubsectionHeader("STAGE OBJECTIVES");
            cardBox.AddChild(stageHeader);

            if (def.stages != null && def.stages.Count > 0)
            {
                for (int i = 0; i < def.stages.Count; i++)
                {
                    var stage = def.stages[i];
                    string marker;
                    Color markerColor;

                    if (i < currentStageIdx)
                    {
                        marker = "[✓]";
                        markerColor = AshfallUiHelpers.ToColor(CoreTheme.Warm);
                    }
                    else if (i == currentStageIdx)
                    {
                        marker = "[►]";
                        markerColor = AshfallUiHelpers.ToColor(CoreTheme.Hot);
                    }
                    else
                    {
                        marker = "[ ]";
                        markerColor = AshfallUiHelpers.ToColor(CoreTheme.Dim);
                    }

                    var stageRow = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingSm);
                    var stageLbl = AshfallUiHelpers.MakeMono($"{marker} {stage.text ?? "—"}");
                    stageLbl.AddThemeColorOverride("font_color", markerColor);
                    stageLbl.AutowrapMode = TextServer.AutowrapMode.WordSmart;
                    stageLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    stageRow.AddChild(stageLbl);
                    cardBox.AddChild(stageRow);
                }
            }

            cardBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Choices / Interactions
            var choicesHeader = AshfallUiHelpers.MakeSubsectionHeader("AVAILABLE INTERACTIONS & DIRECTIVES");
            cardBox.AddChild(choicesHeader);

            bool hasChoices = def.choices != null && def.choices.Count > 0;
            bool choiceMade = !string.IsNullOrEmpty(prog.chosenChoiceId);

            if (hasChoices)
            {
                foreach (var choice in def.choices!)
                {
                    bool isThisChoice = prog.chosenChoiceId == choice.id;
                    var choiceBox = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingSm);

                    var choiceText = AshfallUiHelpers.MakeBody($"• {choice.text ?? "—"}");
                    choiceText.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    choiceText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
                    if (isThisChoice)
                        choiceText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Hot));
                    choiceBox.AddChild(choiceText);

                    if (isThisChoice)
                    {
                        var chosenTag = AshfallUiHelpers.MakeSmall("[CHOSEN]");
                        chosenTag.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Warm));
                        choiceBox.AddChild(chosenTag);
                    }
                    else if (!choiceMade)
                    {
                        string cId = choice.id;
                        string qId = def.id;
                        var btnChoose = AshfallUiHelpers.MakeButton("SELECT", () =>
                        {
                            _expansions?.MakeCrossingChoice(qId, cId);
                            RefreshView();
                        });
                        btnChoose.CustomMinimumSize = new Vector2(100, 32);
                        choiceBox.AddChild(btnChoose);
                    }

                    cardBox.AddChild(choiceBox);
                }
            }
            else
            {
                var noChoiceLbl = AshfallUiHelpers.MakeMetadata("No choice decisions at this stage. Proceed with stage execution.");
                noChoiceLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Dim));
                cardBox.AddChild(noChoiceLbl);
            }

            // Action row: Advance stage
            var actRow = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingSm);
            actRow.Alignment = BoxContainer.AlignmentMode.End;

            string advanceText = currentStageIdx + 1 >= totalStages
                ? "RESOLVE CHARTER PROTOCOL"
                : $"ADVANCE TO STAGE {currentStageIdx + 2}";

            string questId = def.id;
            var btnAdvance = AshfallUiHelpers.MakeButton(advanceText, () =>
            {
                _expansions?.AdvanceCrossingQuestStage(questId);
                RefreshView();
            });
            btnAdvance.CustomMinimumSize = new Vector2(220, 36);
            actRow.AddChild(btnAdvance);
            cardBox.AddChild(actRow);

            return card;
        }

        // ── Available Quest Card ──────────────────────────────────────

        private Control BuildAvailableQuestCard(CrossingQuestDef def, bool locked, string lockReason)
        {
            string reqs = locked ? "LOCKED" : $"ELIGIBLE · MIN DAY {def.min_day}";
            var card = AshfallUiHelpers.MakeCardFrame(def.display_name, reqs);
            var cardBox = card.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            if (!string.IsNullOrEmpty(def.briefing))
            {
                var briefLbl = AshfallUiHelpers.MakeSmall(def.briefing);
                briefLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Muted));
                cardBox.AddChild(briefLbl);
            }

            if (locked)
            {
                var lockLbl = AshfallUiHelpers.MakeSmall($"⚠ {lockReason}");
                lockLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Critical));
                cardBox.AddChild(lockLbl);
            }
            else
            {
                var btnRow = AshfallUiHelpers.MakeHBox(CoreTheme.SpacingSm);
                string qId = def.id;
                var btnStart = AshfallUiHelpers.MakeButton($"INITIATE PROTOCOL // [{def.display_name}]", () =>
                {
                    _expansions?.StartCrossingQuest(qId, _currentDay);
                    RefreshView();
                });
                btnStart.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                btnRow.AddChild(btnStart);
                cardBox.AddChild(btnRow);
            }

            return card;
        }

        private static void ClearContainer(VBoxContainer container)
        {
            AshfallUiHelpers.EmptyChildren(container);
        }

        public override void _ExitTree()
        {
            if (_expansions?.CrossingQuests != null)
            {
                _expansions.CrossingQuests.OnStateChanged -= OnStateChangedHandler;
            }
            if (_vouch != null)
            {
                _vouch.OnStateChanged -= OnVouchChangedHandler;
            }
            base._ExitTree();
        }
    }
}
