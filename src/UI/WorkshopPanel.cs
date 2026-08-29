using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Relic Workshop & Reverse Engineering Panel.
    /// Exposes artifact examination, teardown, component reservation, and tech blueprint research.
    /// Thin presentation layer — all gameplay logic lives in WorkshopReverseEngineeringSystem.
    /// </summary>
    public partial class WorkshopPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        public bool IsBound => _workshop != null;

        private WorkshopReverseEngineeringSystem? _workshop;
        private Ashfall.Core.Inventory.Inventory? _inventory;
        private SurvivorsHostSession? _survivors;

        private VBoxContainer _relicListContainer = null!;
        private VBoxContainer _detailContainer = null!;
        private Label _activeJobHeader = null!;
        private ProgressBar _activeJobProgressBar = null!;
        private Label _activeJobDetails = null!;
        private Button _cancelJobButton = null!;

        private string _selectedRelicId = string.Empty;

        public void Bind(
            WorkshopReverseEngineeringSystem workshop,
            Ashfall.Core.Inventory.Inventory inventory,
            SurvivorsHostSession? survivors = null)
        {
            _workshop = workshop;
            _inventory = inventory;
            _survivors = survivors;

            _workshop.OnWorkshopStateChanged -= RefreshView;
            _workshop.OnWorkshopStateChanged += RefreshView;

            RefreshView();
        }

        public void Unbind()
        {
            if (_workshop != null)
            {
                _workshop.OnWorkshopStateChanged -= RefreshView;
            }
            _workshop = null;
            _inventory = null;
            _survivors = null;
        }

        public override void _Ready()
        {
            // Ticket #125 follow-up: layout chrome owned by
            // res://assets/ui/panels/WorkshopPanel.tscn. SceneBinder resolves
            // the typed unique-name slots; sibling bind logic is unchanged.
            var binder = new SceneBinder(this, typeof(WorkshopPanel));
            binder.Require<VBoxContainer>("RelicListContainer");
            binder.Require<VBoxContainer>("DetailContainer");
            binder.Require<Label>("JobHeader");
            binder.Require<ProgressBar>("JobProgressBar");
            binder.Require<Label>("JobDetails");
            binder.Require<Button>("CancelJobButton");
            binder.Require<Button>("CloseButton");

            _relicListContainer = binder.Get<VBoxContainer>("RelicListContainer");
            _detailContainer = binder.Get<VBoxContainer>("DetailContainer");
            _activeJobHeader = binder.Get<Label>("JobHeader");
            _activeJobProgressBar = binder.Get<ProgressBar>("JobProgressBar");
            _activeJobDetails = binder.Get<Label>("JobDetails");
            _cancelJobButton = binder.Get<Button>("CancelJobButton");
            binder.Get<Button>("CloseButton").Pressed += () => { Visible = false; OnClose?.Invoke(); };

            // Default state strings match the previous panel-authored defaults.
            _activeJobHeader.Text = "WORKSHOP STATUS: IDLE";
            _activeJobDetails.Text = "No active reconstruction or teardown job.";

            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_relicListContainer == null || _detailContainer == null) return;

            AshfallUiHelpers.EmptyChildren(_relicListContainer);
            AshfallUiHelpers.EmptyChildren(_detailContainer);

            if (_workshop == null || _inventory == null)
            {
                _relicListContainer.AddChild(AshfallUiHelpers.MakeMetadata("No workshop session bound."));
                return;
            }

            // Update Active Job Header & Progress
            bool busy = _workshop.IsBusy;
            _cancelJobButton.Visible = busy;

            if (busy)
            {
                var state = _workshop.State;
                string phaseName = state.workPhase switch
                {
                    2 => "DISMANTLING",
                    3 => "REPAIRING",
                    4 => "RESEARCHING BLUEPRINT",
                    _ => "WORKING"
                };

                _activeJobHeader.Text = $"WORKSHOP STATUS: {phaseName} // {state.selectedRelicId.ToUpperInvariant()}";
                _activeJobHeader.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));

                float pct = state.hoursRequired > 0f ? (state.progressHours / state.hoursRequired) * 100f : 0f;
                _activeJobProgressBar.Value = Math.Clamp(pct, 0f, 100f);

                string researcherName = !string.IsNullOrEmpty(state.assignedResearcherId) ? state.assignedResearcherId : "Staff";
                _activeJobDetails.Text = $"Assigned: {researcherName} | Progress: {state.progressHours:F1}h / {state.hoursRequired:F1}h ({pct:F0}%)";
            }
            else
            {
                _activeJobHeader.Text = "WORKSHOP STATUS: IDLE";
                _activeJobHeader.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
                _activeJobProgressBar.Value = 0;
                _activeJobDetails.Text = "Workshop benches clear. Select a recovered pre-war artifact below.";
            }

            // Populate Relic List
            var catalog = _workshop.Catalog.Values.OrderBy(r => r.relic_id).ToList();
            if (catalog.Count == 0)
            {
                _relicListContainer.AddChild(AshfallUiHelpers.MakeMetadata("No technical relics cataloged."));
            }
            else
            {
                if (string.IsNullOrEmpty(_selectedRelicId) || !_workshop.Catalog.ContainsKey(_selectedRelicId))
                {
                    _selectedRelicId = catalog[0].relic_id;
                }

                foreach (var relic in catalog)
                {
                    bool isSelected = relic.relic_id == _selectedRelicId;
                    bool isCompleted = _workshop.IsRelicCompleted(relic.relic_id);
                    _relicListContainer.AddChild(MakeRelicCard(relic, isSelected, isCompleted));
                }
            }

            // Populate Details
            if (!string.IsNullOrEmpty(_selectedRelicId) && _workshop.Catalog.TryGetValue(_selectedRelicId, out var selectedRelic))
            {
                RenderRelicDetail(selectedRelic);
            }
        }

        private Control MakeRelicCard(RelicDefinition relic, bool isSelected, bool isCompleted)
        {
            var card = new PanelContainer();
            card.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            var hbox = new HBoxContainer();
            hbox.AddThemeConstantOverride("separation", 8);
            card.AddChild(hbox);

            var selectBtn = new Button();
            string statusIcon = isCompleted ? "[COMPLETED]" : "[ARTIFACT]";
            selectBtn.Text = $"{statusIcon} {relic.display_name}";
            selectBtn.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            if (isSelected)
            {
                selectBtn.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            }
            else if (isCompleted)
            {
                selectBtn.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
            }

            selectBtn.Pressed += () =>
            {
                _selectedRelicId = relic.relic_id;
                RefreshView();
            };

            hbox.AddChild(selectBtn);
            return card;
        }

        private void RenderRelicDetail(RelicDefinition relic)
        {
            var nameLabel = new Label();
            nameLabel.Text = relic.display_name;
            nameLabel.AddThemeFontSizeOverride("font_size", 18);
            nameLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            _detailContainer.AddChild(nameLabel);

            var desc = new Label();
            desc.Text = relic.description;
            desc.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            desc.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
            _detailContainer.AddChild(desc);

            if (!string.IsNullOrEmpty(relic.restoration_text))
            {
                var memoLabel = new Label();
                memoLabel.Text = $"Field Lore: \"{relic.restoration_text}\"";
                memoLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
                memoLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Success));
                _detailContainer.AddChild(memoLabel);
            }

            // Specs & Metrics
            var specsBox = new VBoxContainer();
            specsBox.AddChild(AshfallUiHelpers.MakeSectionHeader("SPECIFICATIONS & COSTS"));

            bool canRepair = true;
            if (relic.required_components != null && relic.required_components.Count > 0)
            {
                foreach (var compId in relic.required_components)
                {
                    int held = _inventory != null ? _inventory.CountById(compId) : 0;
                    bool hasComp = held >= 1;
                    if (!hasComp) canRepair = false;

                    var compLabel = new Label();
                    compLabel.Text = $"  • Required Part: {compId} — Held: {held}/1 {(hasComp ? "[AVAILABLE]" : "[UNAVAILABLE]")}";
                    compLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(hasComp ? DesignTheme.Success : DesignTheme.Critical));
                    specsBox.AddChild(compLabel);
                }
            }
            else
            {
                specsBox.AddChild(AshfallUiHelpers.MakeMetadata("  • No additional components required for repair."));
            }

            specsBox.AddChild(AshfallUiHelpers.MakeMetadata($"  • Repair Base Time: {relic.repair_time_hours:F0} hours"));
            specsBox.AddChild(AshfallUiHelpers.MakeMetadata($"  • Morale Benefit: +{relic.morale_bonus} shelter morale"));

            if (!string.IsNullOrEmpty(relic.dismantle_yield_item))
            {
                specsBox.AddChild(AshfallUiHelpers.MakeMetadata($"  • Teardown Scrap Yield: {relic.dismantle_yield_amount}x {relic.dismantle_yield_item}"));
            }

            if (!string.IsNullOrEmpty(relic.research_unlock_id))
            {
                specsBox.AddChild(AshfallUiHelpers.MakeMetadata($"  • Research Tech Node: {relic.research_unlock_id}"));
            }

            _detailContainer.AddChild(specsBox);

            // Action Buttons
            var actionBox = new HBoxContainer();
            actionBox.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);

            bool isBusy = _workshop != null && _workshop.IsBusy;
            bool isCompleted = _workshop != null && _workshop.IsRelicCompleted(relic.relic_id);

            // Examine
            var examineBtn = new Button();
            examineBtn.Text = " EXAMINE ";
            examineBtn.Pressed += () =>
            {
                var res = _workshop?.Examine(relic.relic_id);
                RefreshView();
            };
            actionBox.AddChild(examineBtn);

            // Dismantle
            var dismantleBtn = new Button();
            dismantleBtn.Text = " DISMANTLE FOR SCRAP ";
            dismantleBtn.Disabled = isBusy || isCompleted;
            dismantleBtn.Pressed += () =>
            {
                string researcher = GetBestSurvivor("crafting");
                _workshop?.StartDismantle(relic.relic_id, researcher);
                RefreshView();
            };
            actionBox.AddChild(dismantleBtn);

            // Repair
            var repairBtn = new Button();
            repairBtn.Text = " RECONSTRUCT & RESTORE ";
            repairBtn.Disabled = isBusy || isCompleted || !canRepair;
            repairBtn.Pressed += () =>
            {
                string researcher = GetBestSurvivor("crafting");
                _workshop?.StartRepair(relic.relic_id, researcher);
                RefreshView();
            };
            actionBox.AddChild(repairBtn);

            // Research
            if (!string.IsNullOrEmpty(relic.research_unlock_id))
            {
                var researchBtn = new Button();
                researchBtn.Text = " ANALYZE BLUEPRINT ";
                researchBtn.Disabled = isBusy || isCompleted;
                researchBtn.Pressed += () =>
                {
                    string researcher = GetBestSurvivor("science");
                    _workshop?.StartResearch(relic.relic_id, researcher);
                    RefreshView();
                };
                actionBox.AddChild(researchBtn);
            }

            _detailContainer.AddChild(actionBox);
        }

        private string GetBestSurvivor(string domain)
        {
            if (_survivors?.Roster?.Roster != null)
            {
                foreach (var entry in _survivors.Roster.Roster)
                {
                    if (entry != null && entry.isAlive) return entry.survivorId;
                }
            }
            return "survivor_engineer";
        }

        private void OnCancelJobClicked()
        {
            _workshop?.CancelJob();
            RefreshView();
        }
    }
}
