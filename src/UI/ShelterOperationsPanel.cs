// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using System.Text;
using Godot;
using Ashfall.Core.Events;
using Ashfall.Core.Shelter;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Routed command surface over the existing construction, assignment,
    /// outpost, inventory, survivor-needs, and holiday authorities.
    /// </summary>
    public partial class ShelterOperationsPanel : Control, IBindablePanel
    {
        public event Action? OnClose;
        public bool IsBound => _host != null;

        private ShelterOperationsHostSession? _host;
        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _content = null!;
        private Label _summary = null!;
        private Label _feedback = null!;

        private OptionButton _blueprints = null!;
        private OptionButton _rooms = null!;
        private OptionButton _upgrades = null!;
        private OptionButton _projects = null!;
        private OptionButton _survivors = null!;
        private OptionButton _outposts = null!;
        private OptionButton _holidays = null!;
        private OptionButton _scales = null!;
        private SpinBox _gridX = null!;
        private SpinBox _gridY = null!;
        private SpinBox _depth = null!;
        private SpinBox _supplyAmount = null!;

        public void Bind(ShelterOperationsHostSession host)
        {
            Unbind();
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _host.StateChanged += RefreshView;
            RebuildSelectors();
            RefreshView();
        }

        public void Unbind()
        {
            if (_host != null)
                _host.StateChanged -= RefreshView;
            _host = null;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            _shell = new AshfallDashboardShell("Shelter Operations // Works, Crews & Outposts",
                minWidth: 1120, minHeight: 720);
            AddChild(_shell);
            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("stability", "Structural Stability", "—", AshfallMetricCard.Criticality.Normal, 170);
            _statusRail.AddCard("depth", "Open Depth", "—", AshfallMetricCard.Criticality.Normal, 130);
            _statusRail.AddCard("projects", "Active Projects", "0", AshfallMetricCard.Criticality.Normal, 130);
            _statusRail.AddCard("outposts", "Outposts", "0", AshfallMetricCard.Criticality.Normal, 130);

            _content = new VBoxContainer();
            _content.AddThemeConstantOverride("separation", 8);
            _content.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _content.SizeFlagsVertical = SizeFlags.ExpandFill;

            _content.AddChild(AshfallUiHelpers.MakeSectionHeader("SHELTER WORKS"));
            _summary = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _content.AddChild(_summary);
            _feedback = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _feedback.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            _content.AddChild(_feedback);

            AddConstructionControls();
            AddCrewControls();
            AddOutpostControls();
            AddHolidayControls();

            var scroll = new ScrollContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            scroll.AddChild(_content);
            _shell.SetContent(scroll);
            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });
            RefreshView();
        }

        public override void _ExitTree() => Unbind();

        private void AddConstructionControls()
        {
            _content.AddChild(AshfallUiHelpers.MakeSectionHeader("CONSTRUCTION / REPAIR / EXCAVATION"));
            var blueprintRow = new HBoxContainer();
            _blueprints = MakeSelector(290, "Blueprint");
            _gridX = MakeSpin(-50, 50, 0);
            _gridY = MakeSpin(-50, 50, 1);
            _depth = MakeSpin(1, 5, 1);
            blueprintRow.AddChild(_blueprints);
            blueprintRow.AddChild(MakeLabeled("X", _gridX));
            blueprintRow.AddChild(MakeLabeled("Y", _gridY));
            blueprintRow.AddChild(MakeLabeled("DEPTH", _depth));
            blueprintRow.AddChild(AshfallUiHelpers.MakeButton("START BUILD", StartBuild));
            blueprintRow.AddChild(AshfallUiHelpers.MakeButton("EXCAVATE NEXT", StartDepthExcavation));
            _content.AddChild(blueprintRow);

            var upgradeRow = new HBoxContainer();
            _rooms = MakeSelector(300, "Target room");
            _upgrades = MakeSelector(280, "Upgrade");
            upgradeRow.AddChild(_rooms);
            upgradeRow.AddChild(_upgrades);
            upgradeRow.AddChild(AshfallUiHelpers.MakeButton("RENOVATE", StartRenovation));
            upgradeRow.AddChild(AshfallUiHelpers.MakeButton("INSTALL", StartUpgrade));
            _content.AddChild(upgradeRow);
        }

        private void AddCrewControls()
        {
            _content.AddChild(AshfallUiHelpers.MakeSectionHeader("PROJECT CREWS"));
            var row = new HBoxContainer();
            _projects = MakeSelector(330, "Active project");
            _survivors = MakeSelector(300, "Eligible survivor");
            row.AddChild(_projects);
            row.AddChild(_survivors);
            row.AddChild(AshfallUiHelpers.MakeButton("ASSIGN CREW", AssignCrew));
            row.AddChild(AshfallUiHelpers.MakeButton("RELIEVE CREW", RelieveCrew));
            _content.AddChild(row);
            var note = AshfallUiHelpers.MakeBody(
                "Assigned workers advance projects once per campaign day. Crafting practice and fatigue return through the existing survivor authorities.");
            note.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            note.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
            _content.AddChild(note);
        }

        private void AddOutpostControls()
        {
            _content.AddChild(AshfallUiHelpers.MakeSectionHeader("OUTPOST NETWORK"));
            var row = new HBoxContainer();
            _outposts = MakeSelector(330, "Authored outpost");
            _supplyAmount = MakeSpin(1, 999, 10);
            row.AddChild(_outposts);
            row.AddChild(AshfallUiHelpers.MakeButton("ESTABLISH", EstablishOutpost));
            row.AddChild(MakeLabeled("RATIONS", _supplyAmount));
            row.AddChild(AshfallUiHelpers.MakeButton("SUPPLY", SupplyOutpost));
            row.AddChild(AshfallUiHelpers.MakeButton("ABANDON", AbandonOutpost));
            row.AddChild(AshfallUiHelpers.MakeButton("GARRISON", AssignGarrison));
            row.AddChild(AshfallUiHelpers.MakeButton("RELIEVE", RelieveGarrison));
            _content.AddChild(row);
        }

        private void AddHolidayControls()
        {
            _content.AddChild(AshfallUiHelpers.MakeSectionHeader("HOLIDAY / REMEMBRANCE"));
            var row = new HBoxContainer();
            _holidays = MakeSelector(330, "Today's observance");
            _scales = MakeSelector(190, "Scale");
            row.AddChild(_holidays);
            row.AddChild(_scales);
            row.AddChild(AshfallUiHelpers.MakeButton("HOLD", HoldHoliday));
            row.AddChild(AshfallUiHelpers.MakeButton("SKIP", SkipHoliday));
            _content.AddChild(row);
        }

        public void RefreshView()
        {
            if (_host == null || _statusRail == null) return;
            RebuildSelectors();
            var projects = _host.Projects;
            var active = projects.Where(p => p.Status == ProjectStatus.Active).ToList();
            _statusRail.Set("stability", $"{_host.Construction.System.StabilityRating:F0}%",
                _host.Construction.System.StabilityRating >= ShelterExpansionSystem.MinimumSafeStability
                    ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Critical);
            _statusRail.Set("depth", $"D{_host.Construction.System.MaxDepthUnlocked}");
            _statusRail.Set("projects", active.Count.ToString());
            _statusRail.Set("outposts",
                $"{_host.OutpostInstances.Count(x => x.IsEstablished)}/{_host.OutpostDefinitions.Count}");

            var lines = new StringBuilder();
            lines.AppendLine($"DAY {_host.CurrentDay} · structural threshold {ShelterExpansionSystem.MinimumSafeStability:F0}%");
            foreach (var project in projects.OrderBy(p => p.ProjectId, StringComparer.Ordinal))
            {
                lines.AppendLine($"{project.Status,-9} {project.ProjectId} · {project.ProjectType} · " +
                    $"{project.LaborInvestedDays:F1}/{project.LaborRequiredDays:F1} workdays · " +
                    $"{project.CrewSurvivorIds.Count} crew");
            }
            if (projects.Count == 0) lines.AppendLine("No active or completed projects recorded.");
            lines.AppendLine();
            lines.AppendLine("COMPLETED CAPACITY BONUSES");
            foreach (var bonus in _host.Construction.System.GetCompletedCapacityBonuses().OrderBy(x => x.Key, StringComparer.Ordinal))
                lines.AppendLine($"  {bonus.Key}: +{bonus.Value}");
            lines.AppendLine();
            lines.AppendLine("OUTPOST STATUS");
            foreach (var instance in _host.OutpostInstances.OrderBy(x => x.OutpostId, StringComparer.Ordinal))
            {
                var definition = _host.Outposts.System.GetDefinition(instance.OutpostId);
                lines.AppendLine($"  {definition?.Name ?? instance.OutpostId}: " +
                    $"{(instance.IsEstablished ? "ESTABLISHED" : "NOT BUILT")} · " +
                    $"{instance.GarrisonSurvivorIds.Count}/{definition?.MaxGarrisonBunks ?? 0} garrison · " +
                    $"{instance.RationReserve} reserve · {instance.ConditionPermille}‰ condition" +
                    (instance.IsStarving ? " · STARVING" : string.Empty) +
                    (instance.IsOverrun ? " · OVERRUN" : string.Empty));
            }

            var holiday = _host.HolidayForCurrentDay();
            lines.AppendLine();
            lines.AppendLine(holiday == null
                ? "No authored holiday is due today."
                : $"{holiday.Name}: {holiday.Description} · food {holiday.FoodCost}, fuel {holiday.FuelCost} · " +
                    (_host.Celebrations.System.IsHolidayOccurrenceResolved(holiday.HolidayId, _host.CurrentDay)
                        ? "RESOLVED THIS CYCLE" : "AWAITING DECISION"));
            _summary.Text = lines.ToString();
        }

        private void RebuildSelectors()
        {
            if (_host == null || _blueprints == null) return;
            Fill(_blueprints, _host.Blueprints, x => x.BlueprintId, x => $"{x.Name} · cap +{x.CapacityBonus}");
            Fill(_rooms, _host.Rooms, x => x.RoomId, x => $"{x.Name} ({x.RoomId}) · {x.Condition:F0}%");
            Fill(_upgrades, _host.Upgrades, x => x.UpgradeId, x => x.Name);
            Fill(_projects,
                _host.Projects.Where(x => x.Status == ProjectStatus.Active),
                x => x.ProjectId,
                x => $"{x.ProjectId} · {x.ProjectType}");
            Fill(_survivors, _host.CrewCandidates, x => x, x => x);
            Fill(_outposts, _host.OutpostDefinitions, x => x.Id, x => x.Name);
            var holiday = _host.HolidayForCurrentDay();
            var todaysHoliday = new System.Collections.Generic.List<HolidayDef>();
            if (holiday != null) todaysHoliday.Add(holiday);
            Fill(_holidays, todaysHoliday, x => x.HolidayId, x => x.Name);
            if (_scales.ItemCount == 0)
            {
                AddOption(_scales, "Small", "small");
                AddOption(_scales, "Large", "large");
            }
        }

        private static void Fill<T>(OptionButton selector, System.Collections.Generic.IEnumerable<T> items,
            Func<T, string> id, Func<T, string> label)
        {
            string selected = Selected(selector);
            selector.Clear();
            foreach (var item in items)
                AddOption(selector, label(item), id(item));
            Select(selector, selected);
        }

        private static OptionButton MakeSelector(float width, string placeholder)
        {
            var selector = new OptionButton { CustomMinimumSize = new Vector2(width, 36) };
            selector.AddItem(placeholder);
            selector.SetItemMetadata(0, string.Empty);
            return selector;
        }

        private static SpinBox MakeSpin(double min, double max, double value)
            => new() { MinValue = min, MaxValue = max, Value = value, Step = 1, CustomMinimumSize = new Vector2(72, 36) };

        private static Control MakeLabeled(string text, Control input)
        {
            var row = new HBoxContainer();
            row.AddChild(AshfallUiHelpers.MakeBody(text));
            row.AddChild(input);
            return row;
        }

        private static void AddOption(OptionButton selector, string label, string id)
        {
            selector.AddItem(label);
            selector.SetItemMetadata(selector.ItemCount - 1, id);
        }

        private static string Selected(OptionButton? selector)
        {
            if (selector == null || selector.Selected < 0) return string.Empty;
            return selector.GetItemMetadata(selector.Selected).AsString();
        }

        private static void Select(OptionButton selector, string id)
        {
            for (int i = 0; i < selector.ItemCount; i++)
                if (string.Equals(selector.GetItemMetadata(i).AsString(), id, StringComparison.Ordinal))
                {
                    selector.Select(i);
                    return;
                }
            if (selector.ItemCount > 0) selector.Select(0);
        }

        private void StartBuild()
        {
            if (_host == null) return;
            var result = _host.StartRoom(Selected(_blueprints), (int)_gridX.Value, (int)_gridY.Value, (int)_depth.Value);
            SetFeedback(result.Succeeded
                ? $"Started {result.Project?.ProjectId}."
                : $"Construction refused: {result.Code}.");
        }

        private void StartDepthExcavation()
        {
            if (_host == null) return;
            var result = _host.StartDepthExcavation();
            SetFeedback(result.Succeeded
                ? $"Started shaft project {result.Project?.ProjectId}."
                : $"Excavation refused: {result.Code}.");
        }

        private void StartRenovation()
        {
            if (_host == null) return;
            var result = _host.StartRenovation(Selected(_rooms));
            SetFeedback(result.Succeeded ? "Renovation started." : $"Renovation refused: {result.Code}.");
        }

        private void StartUpgrade()
        {
            if (_host == null) return;
            var result = _host.StartUpgrade(Selected(_rooms), Selected(_upgrades));
            SetFeedback(result.Succeeded ? "Upgrade started." : $"Upgrade refused: {result.Code}.");
        }

        private void AssignCrew()
        {
            bool assigned = _host?.AssignCrew(Selected(_projects), Selected(_survivors)) == true;
            SetFeedback(assigned ? "Survivor assigned to project." : "Crew assignment refused.");
        }

        private void RelieveCrew()
        {
            bool relieved = _host?.RelieveCrew(Selected(_projects), Selected(_survivors)) == true;
            SetFeedback(relieved ? "Survivor relieved from project." : "Crew relief refused.");
        }

        private void EstablishOutpost()
        {
            bool established = _host?.EstablishOutpost(Selected(_outposts)) == true;
            SetFeedback(established ? "Outpost established from the authored inventory bill." : "Outpost establishment refused.");
        }

        private void SupplyOutpost()
        {
            bool supplied = _host?.SupplyOutpost(Selected(_outposts), (int)_supplyAmount.Value) == true;
            SetFeedback(supplied ? "Rations transferred to outpost reserve." : "Outpost supply refused.");
        }

        private void AbandonOutpost()
        {
            bool abandoned = _host?.AbandonOutpost(Selected(_outposts)) == true;
            SetFeedback(abandoned ? "Outpost abandoned; garrison relieved." : "Outpost abandonment refused.");
        }

        private void AssignGarrison()
        {
            bool assigned = _host?.AssignGarrison(Selected(_outposts), Selected(_survivors)) == true;
            SetFeedback(assigned ? "Survivor assigned to outpost garrison." : "Garrison assignment refused.");
        }

        private void RelieveGarrison()
        {
            bool relieved = _host?.RelieveGarrison(Selected(_outposts), Selected(_survivors)) == true;
            SetFeedback(relieved ? "Survivor relieved from outpost garrison." : "Garrison relief refused.");
        }

        private void HoldHoliday()
        {
            if (_host == null) return;
            bool held = _host.HoldHoliday(Selected(_holidays), Selected(_scales),
                Math.Max(1, _host.CrewCandidates.Count), out var record);
            SetFeedback(held ? record?.Summary ?? "Holiday held." : "Holiday refused: check cycle, fuel, and ration stock.");
        }

        private void SkipHoliday()
        {
            if (_host == null) return;
            bool skipped = _host.SkipHoliday(Selected(_holidays), out float penalty);
            SetFeedback(skipped ? $"Holiday skipped; morale {penalty:F0}." : "No unresolved holiday is due today.");
        }

        private void SetFeedback(string message)
        {
            _feedback.Text = message;
            RefreshView();
        }
    }
}
