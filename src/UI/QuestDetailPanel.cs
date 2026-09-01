using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Crossing;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Quest Detail panel.
    /// Shows comprehensive mission dossier, narrative briefing, step-by-step stage objectives,
    /// branching decisions, target location coordinates, and operational rewards.
    /// </summary>
    public partial class QuestDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _infoContainer = null!;
        private VBoxContainer _stagesContainer = null!;
        private VBoxContainer _choicesContainer = null!;
        private VBoxContainer _rewardsContainer = null!;
        private Label _titleLabel = null!;

        public void Bind(
            string questId,
            string displayName,
            string type,
            string briefing,
            int currentStage,
            List<string> stages,
            List<string>? choices = null,
            string? targetLocation = null,
            string? rewards = null,
            bool isCompleted = false)
        {
            if (_titleLabel != null)
                _titleLabel.Text = $"OPERATION DOSSIER // {displayName.ToUpperInvariant()}";

            if (_infoContainer == null || _stagesContainer == null ||
                _choicesContainer == null || _rewardsContainer == null)
                return;

            AshfallUiHelpers.EmptyChildren(_infoContainer);
            AshfallUiHelpers.EmptyChildren(_stagesContainer);
            AshfallUiHelpers.EmptyChildren(_choicesContainer);
            AshfallUiHelpers.EmptyChildren(_rewardsContainer);

            // ── 1. Quest Profile Card ──
            var infoCard = AshfallUiHelpers.MakeCardFrame("MISSION OVERVIEW & DIRECTIVE", questId);
            var infoBox = infoCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            infoBox.AddChild(AshfallUiHelpers.MakeDataRow("Classification", type, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
            infoBox.AddChild(AshfallUiHelpers.MakeDataRow("Operational Status", isCompleted ? "COMPLETED & VERIFIED" : "ACTIVE DIRECTIVE", AshfallUiHelpers.ToColor(isCompleted ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Hot)));
            infoBox.AddChild(AshfallUiHelpers.MakeDataRow("Target Sector", string.IsNullOrEmpty(targetLocation) ? "District 8 / Sector Grid" : targetLocation, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));

            infoBox.AddChild(AshfallUiHelpers.MakeSeparator());

            string fullBriefing = string.IsNullOrEmpty(briefing) ? "Standard survival protocol directive. Complete the assigned tasks to secure shelter infrastructure and ensure cohort continuity." : briefing;
            var bodyLbl = AshfallUiHelpers.MakeBody(fullBriefing);
            infoBox.AddChild(bodyLbl);
            _infoContainer.AddChild(infoCard);

            // ── 2. Step-by-Step Stage Breakdown ──
            var stageCard = AshfallUiHelpers.MakeCardFrame("OPERATIONAL STAGES & OBJECTIVES", $"PROGRESS: {Math.Min(currentStage + 1, stages.Count)} / {stages.Count}");
            var stageBox = stageCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            for (int i = 0; i < stages.Count; i++)
            {
                bool done = isCompleted || i < currentStage;
                bool active = !isCompleted && i == currentStage;

                string prefix = done ? "[✓]" : (active ? "[►]" : "[ ]");
                string stageText = $"{prefix} Stage {i + 1}: {stages[i]}";

                var stageLbl = AshfallUiHelpers.MakeBody(stageText);
                if (active)
                {
                    stageLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                    stageLbl.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeH3);
                }
                else if (done)
                {
                    stageLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                }
                else
                {
                    stageLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
                }
                stageBox.AddChild(stageLbl);
            }
            _stagesContainer.AddChild(stageCard);

            // ── 3. Branching Decisions & Choices ──
            var choiceCard = AshfallUiHelpers.MakeCardFrame("CRITICAL DECISION GATES & BRANCHES", "TACTICAL CHOICE");
            var choiceBox = choiceCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            if (choices != null && choices.Count > 0)
            {
                foreach (var choice in choices)
                {
                    choiceBox.AddChild(AshfallUiHelpers.MakeDataRow("Available Path", choice, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
                }
            }
            else
            {
                choiceBox.AddChild(AshfallUiHelpers.MakeDataRow("Linear Protocol", "Standard sequential execution required. No divergent ideological branches.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            }
            _choicesContainer.AddChild(choiceCard);

            // ── 4. Rewards & Unlocks ──
            var rewardCard = AshfallUiHelpers.MakeCardFrame("COMPLETION REWARDS & INTEL UNLOCKS", "YIELD");
            var rewardBox = rewardCard.GetChild<MarginContainer>(0).GetChild<VBoxContainer>(0);

            string rewText = string.IsNullOrEmpty(rewards) ? "+10 Morale, +20 Survival Supplies, +1 Codex Knowledge Node" : rewards;
            rewardBox.AddChild(AshfallUiHelpers.MakeDataRow("Cohort Compensation", rewText, AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
            rewardBox.AddChild(AshfallUiHelpers.MakeDataRow("Strategic Impact", "Advances Holdfast survival timeline and strengthens shelter structural security.", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale)));
            _rewardsContainer.AddChild(rewardCard);
        }

        public void Bind(HoldfastQuestEntry? holdfastDef, HoldfastQuestProgress? progress = null)
        {
            if (holdfastDef == null) return;

            var stages = new List<string>();
            if (holdfastDef.stages != null && holdfastDef.stages.Length > 0)
            {
                foreach (var s in holdfastDef.stages)
                    stages.Add(s?.text ?? "Operational step");
            }
            else
            {
                stages.Add("Perform preliminary survey");
                stages.Add("Secure primary site objective");
                stages.Add("Report findings to leadership");
            }

            var choices = new List<string>();
            if (holdfastDef.choices != null)
            {
                foreach (var c in holdfastDef.choices)
                    choices.Add(c?.text ?? "Choice");
            }

            int curStage = progress?.stage ?? 0;
            bool isComp = progress?.completed ?? false;

            Bind(
                holdfastDef.id,
                holdfastDef.display_name ?? holdfastDef.id,
                "Main Story // Holdfast Protocol",
                holdfastDef.briefing ?? "",
                curStage,
                stages,
                choices,
                holdfastDef.target_location_id,
                "+20 Food Rations, +10 Water, +5 Morale",
                isComp);
        }

        public void Bind(CrossingQuestDef? crossingDef, CrossingQuestProgress? progress = null)
        {
            if (crossingDef == null) return;

            var stages = new List<string>();
            if (crossingDef.stages != null && crossingDef.stages.Count > 0)
            {
                foreach (var s in crossingDef.stages)
                    stages.Add(s?.text ?? "Crossing objective");
            }
            else
            {
                stages.Add("Negotiate transit rights");
                stages.Add("Fulfill arbitration terms");
            }

            var choices = new List<string>();
            if (crossingDef.choices != null)
            {
                foreach (var c in crossingDef.choices)
                    choices.Add(c?.text ?? "Choice");
            }

            int curStage = progress?.currentStage ?? 0;
            bool isComp = progress?.completed ?? false;

            Bind(
                crossingDef.id,
                crossingDef.display_name,
                "Nobody's Charter // Crossing Quest",
                crossingDef.briefing,
                curStage,
                stages,
                choices,
                crossingDef.target_location_id,
                "Vouch Access Authorization, Crossing Transit Rights",
                isComp);
        }

        public override void _Ready()
        {
            // Ticket #125: layout chrome owned by
            // res://assets/ui/panels/QuestDetailPanel.tscn. SceneBinder resolves
            // typed unique-name nodes; sibling bind logic in this file is
            // unchanged.
            var binder = new SceneBinder(this, typeof(QuestDetailPanel));
            binder.Require<VBoxContainer>("InfoContainer");
            binder.Require<VBoxContainer>("StagesContainer");
            binder.Require<VBoxContainer>("ChoicesContainer");
            binder.Require<VBoxContainer>("RewardsContainer");
            binder.Require<Label>("Title");
            binder.Require<Button>("CloseButton");

            _infoContainer = binder.Get<VBoxContainer>("InfoContainer");
            _stagesContainer = binder.Get<VBoxContainer>("StagesContainer");
            _choicesContainer = binder.Get<VBoxContainer>("ChoicesContainer");
            _rewardsContainer = binder.Get<VBoxContainer>("RewardsContainer");
            _titleLabel = binder.Get<Label>("Title");
            binder.Get<Button>("CloseButton").Pressed += () => OnClose?.Invoke();

            Visible = false;
        }

        public void Open()
        {
            Visible = true;
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
