// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.Inventory;
using Ashfall.Core.Combat;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class WorkshopPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        public bool IsBound => _shelterWorkshop != null || _legacyWorkshop != null;

        private ShelterWorkshopSystem? _shelterWorkshop;
        private WorkshopReverseEngineeringSystem? _legacyWorkshop;
        private Ashfall.Core.Inventory.Inventory? _inventory;
        private EquipmentConditionSystem? _equipment;
        private ExpeditionVehicleSystem? _vehicles;
        private SurvivorsHostSession? _survivors;

        private VBoxContainer _relicListContainer = null!;
        private VBoxContainer _detailContainer = null!;
        private Label _activeJobHeader = null!;
        private ProgressBar _activeJobProgressBar = null!;
        private Label _activeJobDetails = null!;
        private Button _cancelJobButton = null!;
        private Button _closeButton = null!;
        private VBoxContainer _machineConditionContainer = null!;
        private OptionButton _categoryFilter = null!;

        private string _selectedRecipeId = string.Empty;
        private string _selectedRelicId = string.Empty;
        private ItemCatalog? _itemCatalog;
        private string _currentRoomId = "room_workshop";

        public void Bind(
            ShelterWorkshopSystem workshop,
            Ashfall.Core.Inventory.Inventory inventory,
            EquipmentConditionSystem? equipment = null,
            ExpeditionVehicleSystem? vehicles = null,
            SurvivorsHostSession? survivors = null)
        {
            _shelterWorkshop = workshop;
            _inventory = inventory;
            _equipment = equipment;
            _vehicles = vehicles;
            _survivors = survivors;

            _shelterWorkshop.OnWorkshopChanged -= RefreshView;
            _shelterWorkshop.OnWorkshopChanged += RefreshView;

            RefreshView();
        }

        // Backward-compatible overload
        public void Bind(
            WorkshopReverseEngineeringSystem workshop,
            Ashfall.Core.Inventory.Inventory inventory,
            SurvivorsHostSession? survivors = null)
        {
            BindRelicWorkshop(workshop, inventory, itemCatalog: null, survivors);
        }

        /// <summary>
        /// Plan 87 follow-up: bind the relic restoration catalog alongside (or
        /// instead of) the shelter crafting system. The panel renders crafting
        /// recipes when <see cref="Bind(ShelterWorkshopSystem, ...)"/> was used,
        /// and always renders the restoration section when a relic workshop is
        /// bound. <paramref name="itemCatalog"/> supplies item display names.
        /// </summary>
        public void BindRelicWorkshop(
            WorkshopReverseEngineeringSystem workshop,
            Ashfall.Core.Inventory.Inventory inventory,
            ItemCatalog? itemCatalog = null,
            SurvivorsHostSession? survivors = null)
        {
            if (_legacyWorkshop != null)
            {
                _legacyWorkshop.OnWorkshopStateChanged -= RefreshView;
            }
            _legacyWorkshop = workshop;
            _inventory = inventory;
            _itemCatalog = itemCatalog;
            if (survivors != null) _survivors = survivors;

            if (_legacyWorkshop != null)
            {
                _legacyWorkshop.OnWorkshopStateChanged += RefreshView;
            }

            RefreshView();
        }

        public void Unbind()
        {
            if (_shelterWorkshop != null) _shelterWorkshop.OnWorkshopChanged -= RefreshView;
            if (_legacyWorkshop != null)
            {
                _legacyWorkshop.OnWorkshopStateChanged -= RefreshView;
            }
            _shelterWorkshop = null;
            _legacyWorkshop = null;
            _inventory = null;
            _equipment = null;
            _vehicles = null;
            _survivors = null;
        }

        public override void _Ready()
        {
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
            _closeButton = binder.Get<Button>("CloseButton");

            _closeButton.Pressed += () => { Visible = false; OnClose?.Invoke(); };

            // Optional new nodes for the new system
            if (HasNode("%MachineConditionContainer")) _machineConditionContainer = GetNode<VBoxContainer>("%MachineConditionContainer");
            if (HasNode("%CategoryFilter"))
            {
                _categoryFilter = GetNode<OptionButton>("%CategoryFilter");
                _categoryFilter.ItemSelected += (idx) => RefreshView();
            }

            RefreshView();
        }

        public override void _ExitTree()
        {
            Unbind();
        }

        public void Open() { Visible = true; RefreshView(); }
        public void RefreshView()
        {
            if (_relicListContainer == null) return;
            AshfallUiHelpers.EmptyChildren(_relicListContainer);
            AshfallUiHelpers.EmptyChildren(_detailContainer);
            if (_machineConditionContainer != null) AshfallUiHelpers.EmptyChildren(_machineConditionContainer);

            // Legacy-only layout (no shelter crafting system bound).
            if (_shelterWorkshop == null)
            {
                if (_legacyWorkshop != null) RenderLegacy();
                return;
            }

            var state = _shelterWorkshop.State;
            var activeJob = state.jobs.FirstOrDefault(j => j.Status == WorkshopJobStatus.Active || j.Status == WorkshopJobStatus.CompletedPendingCollection);

            _cancelJobButton.Visible = (activeJob != null && activeJob.Status == WorkshopJobStatus.Active);

            if (activeJob != null)
            {
                _activeJobHeader.Text = $"WORKSHOP STATUS: {(activeJob.Status == WorkshopJobStatus.CompletedPendingCollection ? "COMPLETED" : "WORKING")} // {activeJob.RecipeId}";
                _activeJobProgressBar.Value = activeJob.TotalLaborTicks > 0 ? ((activeJob.TotalLaborTicks - activeJob.RemainingLaborTicks) / (float)activeJob.TotalLaborTicks) * 100f : 0;
                _activeJobDetails.Text = $"Progress: {_activeJobProgressBar.Value:F0}%";
                if (_cancelJobButton.IsConnected("pressed", new Callable(this, MethodName.OnCancelJobClicked))) _cancelJobButton.Disconnect("pressed", new Callable(this, MethodName.OnCancelJobClicked));
                _cancelJobButton.Pressed += OnCancelJobClicked;

                if (activeJob.Status == WorkshopJobStatus.CompletedPendingCollection)
                {
                    _cancelJobButton.Visible = true;
                    _cancelJobButton.Text = "COLLECT JOB";
                    if (_cancelJobButton.IsConnected("pressed", new Callable(this, MethodName.OnCancelJobClicked))) _cancelJobButton.Disconnect("pressed", new Callable(this, MethodName.OnCancelJobClicked));
                    _cancelJobButton.Pressed += () => { _shelterWorkshop.TryCollectCompletedJob(activeJob.JobId); RefreshView(); };
                }
                else
                {
                    _cancelJobButton.Text = "ABORT JOB";
                }
            }
            else if (_legacyWorkshop != null && _legacyWorkshop.IsBusy)
            {
                // Shelter bench idle but a relic restoration is on the bench.
                var st = _legacyWorkshop.State;
                var busyDef = _legacyWorkshop.GetRelic(st.selectedRelicId);
                _activeJobHeader.Text = $"RESTORATION: {busyDef?.display_name ?? st.selectedRelicId}";
                _activeJobProgressBar.Value = st.hoursRequired > 0 ? (st.progressHours / st.hoursRequired) * 100f : 0f;
                _activeJobDetails.Text = $"Progress: {st.progressHours:F1} / {st.hoursRequired:F0} h";
            }
            else
            {
                _activeJobHeader.Text = "WORKSHOP STATUS: IDLE";
                _activeJobProgressBar.Value = 0;
                _activeJobDetails.Text = "No active job.";
            }

            var machine = _shelterWorkshop.GetOrCreateMachineState(_currentRoomId);
            if (_machineConditionContainer != null)
            {
                var gaugeBox = new HBoxContainer();

                var healthGauge = new AnalogConditionGauge { LabelText = "TOOLING", Value = machine.ToolingHealth * 100f, WarningThreshold = 20f };
                var calGauge = new AnalogConditionGauge { LabelText = "CALIBRATION", Value = machine.Calibration * 100f, WarningThreshold = 20f };

                gaugeBox.AddChild(healthGauge);
                gaugeBox.AddChild(calGauge);
                _machineConditionContainer.AddChild(gaugeBox);

                var overhaulBtn = new Button { Text = "OVERHAUL TOOLING" };
                overhaulBtn.Pressed += () => { _shelterWorkshop.TryOverhaulTooling(_currentRoomId); RefreshView(); };
                _machineConditionContainer.AddChild(overhaulBtn);

                // C2 / Plan 22 Phase 4 — protective-gear repair through the
                // authored repairRecipe bills: atomic TryConsumeBill → the
                // canonical EquippedItem durability the simulation reads.
                // Text state + percentages (never color-only); standard
                // keyboard-focusable buttons.
                if (_inventory?.Equipped != null)
                {
                    foreach (var equipped in _inventory.Equipped)
                    {
                        var gearRecipe = equipped?.Item?.repairRecipe;
                        if (equipped?.Item == null || gearRecipe == null || gearRecipe.costs.Count == 0) continue;

                    float cap = equipped.Item.durability
                        * Math.Clamp(gearRecipe.MaxRepairConditionFraction, 0f, 1f);
                    bool failed = equipped.CurrentDurability <= 0f;
                    bool repairableNow = !failed && equipped.CurrentDurability < cap;

                    string stateText = failed
                        ? "FAILED — REPLACE (repair refuses dead gear)"
                        : repairableNow ? "repairable" : "serviceable";

                    var gearRow = new HBoxContainer();
                    var gearLabel = new Label
                    {
                        Text = $"{equipped.Item.displayName} · condition {equipped.CurrentDurability:0}/{equipped.Item.durability:0} · {stateText}"
                    };
                    gearLabel.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
                    gearRow.AddChild(gearLabel);

                    var repairBtn = new Button { Text = "REPAIR" };
                    repairBtn.Disabled = !repairableNow;
                    var equippedRef = equipped;
                    repairBtn.Pressed += () =>
                    {
                        _inventory.TryRepairEquippedGear(equippedRef); // refusal is silent-free: state text already explains
                        RefreshView();
                    };
                    gearRow.AddChild(repairBtn);
                    _machineConditionContainer.AddChild(gearRow);
                }
            }
            }

            var recipes = _shelterWorkshop.GetAvailableRecipes(_currentRoomId);
            foreach (var r in recipes)
            {
                var btn = new Button { Text = r.DisplayName };
                if (r.Id == _selectedRecipeId) btn.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
                btn.Pressed += () => { _selectedRecipeId = r.Id; _selectedRelicId = string.Empty; RefreshView(); };
                _relicListContainer.AddChild(btn);
            }

            if (!string.IsNullOrEmpty(_selectedRecipeId) && _shelterWorkshop.Recipes.TryGetValue(_selectedRecipeId, out var recipe))
            {
                var title = new Label { Text = recipe.DisplayName };
                title.AddThemeFontSizeOverride("font_size", 18);
                _detailContainer.AddChild(title);

                _detailContainer.AddChild(new Label { Text = $"Labor Ticks: {recipe.BaseLaborTicks}\nTooling Wear: {recipe.ToolWearPermille/10f}%\nBase Scrap Waste: {recipe.BaseScrapWastePermille/10f}%" });

                var startBtn = new Button { Text = "START JOB" };
                startBtn.Pressed += () =>
                {
                    string? targetId = null;
                    if (recipe.Kind == WorkshopJobKind.WeaponService && _equipment?.State?.items?.Count > 0) targetId = _equipment.State.items.First().instanceId;
                    _shelterWorkshop.TryStartJob(recipe.Id, _currentRoomId, targetId, new List<string>(), out var _);
                    RefreshView();
                };
                _detailContainer.AddChild(startBtn);
            }
            else if (!string.IsNullOrEmpty(_selectedRelicId))
            {
                RenderRelicDetail(_selectedRelicId);
            }

            // Plan 87 follow-up: the restoration catalog renders beside the
            // crafting recipes when both systems are bound.
            RenderRelicSection();
        }

        private void RenderLegacy()
        {
            if (_legacyWorkshop == null) return;

            var state = _legacyWorkshop.State;
            if (_legacyWorkshop.IsBusy)
            {
                var def = _legacyWorkshop.GetRelic(state.selectedRelicId);
                _activeJobHeader.Text = $"RESTORATION: {def?.display_name ?? state.selectedRelicId}";
                _activeJobProgressBar.Value = state.hoursRequired > 0 ? (state.progressHours / state.hoursRequired) * 100f : 0f;
                _activeJobDetails.Text = $"Progress: {state.progressHours:F1} / {state.hoursRequired:F0} h";
                _cancelJobButton.Visible = true;
                _cancelJobButton.Text = "ABANDON RESTORATION";
                if (_cancelJobButton.IsConnected("pressed", new Callable(this, MethodName.OnCancelJobClicked))) _cancelJobButton.Disconnect("pressed", new Callable(this, MethodName.OnCancelJobClicked));
                _cancelJobButton.Pressed += OnLegacyCancelClicked;
            }
            else
            {
                _activeJobHeader.Text = "WORKSHOP STATUS: IDLE";
                _activeJobProgressBar.Value = 0;
                _activeJobDetails.Text = "No active job.";
                _cancelJobButton.Visible = false;
            }

            RenderRelicSection();

            if (!string.IsNullOrEmpty(_selectedRelicId))
                RenderRelicDetail(_selectedRelicId);
        }

        private void RenderRelicSection()
        {
            if (_legacyWorkshop == null || _relicListContainer == null) return;

            AppendSectionHeader("RESTORATION — PRE-WAR ARTIFACTS");

            if (_legacyWorkshop.IsBusy)
            {
                var st = _legacyWorkshop.State;
                var busyDef = _legacyWorkshop.GetRelic(st.selectedRelicId);
                var busyRow = new HBoxContainer();
                busyRow.AddChild(new Label
                {
                    Text = $"On the bench: {busyDef?.display_name ?? st.selectedRelicId} ({st.progressHours:F1}/{st.hoursRequired:F0} h)",
                    SizeFlagsHorizontal = SizeFlags.ExpandFill,
                    AutowrapMode = TextServer.AutowrapMode.WordSmart
                });
                var abandonBtn = new Button { Text = "ABANDON" };
                abandonBtn.Pressed += OnLegacyCancelClicked;
                busyRow.AddChild(abandonBtn);
                _relicListContainer.AddChild(busyRow);
            }

            foreach (var relic in SortedRelics())
            {
                var label = relic.display_name;
                if (_legacyWorkshop.IsRelicCompleted(relic.relic_id)) label += "  [RESTORED]";
                else if (_legacyWorkshop.IsBusy && _legacyWorkshop.State.selectedRelicId == relic.relic_id) label += "  [IN PROGRESS]";

                var btn = new Button { Text = label };
                if (relic.relic_id == _selectedRelicId) btn.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
                var capturedId = relic.relic_id;
                btn.Pressed += () => { _selectedRelicId = capturedId; _selectedRecipeId = string.Empty; RefreshView(); };
                _relicListContainer.AddChild(btn);
            }
        }

        private void RenderRelicDetail(string relicId)
        {
            if (_legacyWorkshop == null || _detailContainer == null) return;
            var relic = _legacyWorkshop.GetRelic(relicId);
            if (relic == null) return;

            var title = new Label { Text = relic.display_name };
            title.AddThemeFontSizeOverride("font_size", 18);
            _detailContainer.AddChild(title);

            if (!string.IsNullOrEmpty(relic.description))
                _detailContainer.AddChild(MakeWrappedLabel(relic.description));

            if (_legacyWorkshop.IsRelicCompleted(relicId))
            {
                _detailContainer.AddChild(new Label { Text = "STATUS: RESTORED" });
                if (!string.IsNullOrEmpty(relic.restoration_text))
                    _detailContainer.AddChild(MakeWrappedLabel(relic.restoration_text));
                return;
            }

            bool allAvailable = true;
            if (relic.required_components.Count > 0)
            {
                var lines = new System.Text.StringBuilder("COMPONENTS:\n");
                for (int i = 0; i < relic.required_components.Count; i++)
                {
                    var compId = relic.required_components[i];
                    int held = _inventory?.CountById(compId) ?? 0;
                    bool ok = held >= 1;
                    if (!ok) allAvailable = false;
                    lines.Append(ok ? "  [OK] " : "  [!!] ")
                         .Append(DisplayName(compId))
                         .Append($" (held {held})\n");
                }
                _detailContainer.AddChild(new Label { Text = lines.ToString().TrimEnd() });
            }

            _detailContainer.AddChild(new Label { Text = $"Workshop time: {relic.repair_time_hours:F0} h" + (relic.morale_bonus > 0 ? $"\nMorale: +{relic.morale_bonus} once restored" : string.Empty) });

            if (_legacyWorkshop.IsBusy)
            {
                _detailContainer.AddChild(new Label { Text = "The bench is occupied. Finish or abandon the current restoration first." });
                return;
            }

            var startBtn = new Button { Text = "START RESTORATION", Disabled = !allAvailable };
            startBtn.Pressed += () =>
            {
                _legacyWorkshop.StartRepair(relicId, PickResearcherId());
                RefreshView();
            };
            _detailContainer.AddChild(startBtn);
        }

        private void OnLegacyCancelClicked()
        {
            _legacyWorkshop?.CancelJob();
            RefreshView();
        }

        private string PickResearcherId()
        {
            var roster = _survivors?.RosterState;
            if (roster != null)
            {
                for (int i = 0; i < roster.Count; i++)
                {
                    var s = roster[i];
                    if (s != null && s.IsAliveState && !string.IsNullOrEmpty(s.Id)) return s.Id;
                }
            }
            return string.Empty;
        }

        private string DisplayName(string itemId)
        {
            var def = _itemCatalog?.Get(itemId);
            return def != null && !string.IsNullOrEmpty(def.displayName) ? def.displayName : itemId;
        }

        private System.Collections.Generic.IEnumerable<RelicDefinition> SortedRelics()
        {
            var list = new System.Collections.Generic.List<RelicDefinition>();
            foreach (var kvp in _legacyWorkshop!.Catalog)
            {
                if (kvp.Value != null) list.Add(kvp.Value);
            }
            // Cultural relics first, then technical; alphabetical within group.
            list.Sort((a, b) =>
            {
                int cat = string.Compare(CategoryGroup(a), CategoryGroup(b), System.StringComparison.Ordinal);
                if (cat != 0) return cat;
                return string.Compare(a.display_name, b.display_name, System.StringComparison.CurrentCultureIgnoreCase);
            });
            return list;
        }

        private static string CategoryGroup(RelicDefinition r) =>
            r.category != null && r.category.StartsWith("relic_tech", System.StringComparison.Ordinal) ? "1" : "0";

        private void AppendSectionHeader(string text)
        {
            var header = new Label { Text = text };
            header.AddThemeFontSizeOverride("font_size", 14);
            _relicListContainer.AddChild(header);
        }

        private Label MakeWrappedLabel(string text) =>
            new Label { Text = text, AutowrapMode = TextServer.AutowrapMode.WordSmart, CustomMinimumSize = new Vector2(0, 0) };

        private void OnCancelJobClicked()
        {
            if (_shelterWorkshop != null)
            {
                var active = _shelterWorkshop.State.jobs.FirstOrDefault(j => j.Status == WorkshopJobStatus.Active);
                if (active != null) _shelterWorkshop.TryCancelJob(active.JobId);
            }
            RefreshView();
        }
    }
}
