// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Godot;
using Ashfall.Core.Shelter;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 210 Phase 9 — sanitation overlay panel. Presents the derived
    /// hygiene picture truthfully: waste burden per room, spill state,
    /// compost queue, and cleaning priority. Severity is text + labels, never
    /// color-only (accessibility contract).
    /// </summary>
    public partial class SanitationPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Label _compostText = null!;

        private SanitationHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(SanitationHostSession session)
        {
            _host = session;
            if (_host != null) _host.StateChanged += RefreshView;
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

        public void RefreshView()
        {
            if (_host == null || _statusRail == null) return;
            var system = _host.System;
            int hygiene = system.GetShelterHygienePermille();
            var band = system.GetShelterHygieneBand();
            var spill = system.ActiveSpill;

            _statusRail.Set("hygiene", $"{hygiene}/1000 · {band}",
                band == HygieneBand.Hazardous ? AshfallMetricCard.Criticality.Critical
                : band >= HygieneBand.Squalid ? AshfallMetricCard.Criticality.Warn
                : band >= HygieneBand.Poor ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("spill", spill != null ? $"ACTIVE · {spill.roomId}" : "none",
                spill != null ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("compost", $"{system.CompostQueue.Count} batches", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("rooms", $"{system.State.rooms.Count}", AshfallMetricCard.Criticality.Normal);

            var lines = new System.Text.StringBuilder();
            lines.AppendLine($"Shelter hygiene: {hygiene}/1000 ({band}).");
            if (spill != null)
                lines.AppendLine($"ACTIVE SPILL: {WasteLabel(spill.type)} in {spill.roomId} since day {spill.startDay} "
                    + $"(severity {spill.severity:0.00}). Critical cleaning resolves it.");
            else
                lines.AppendLine("No active spill. Spills trigger only when a room overflows its safe capacity.");
            lines.AppendLine();
            foreach (var room in system.State.rooms.Take(8))
            {
                int roomHygiene = system.GetRoomHygienePermille(room.roomId);
                lines.AppendLine($"  {room.roomId}: organic {room.organic:0.0} · chemical {room.chemical:0.0} · "
                    + $"radioactive {room.radioactive:0.0} → hygiene {roomHygiene}/1000");
            }
            if (system.State.rooms.Count > 8)
                lines.AppendLine($"  … and {system.State.rooms.Count - 8} more rooms.");
            _detailText.Text = lines.ToString().TrimEnd();

            _compostText.Text = system.CompostQueue.Count == 0
                ? "No compost batches processing. The compost unit converts organic waste into fertilizer over six days."
                : string.Join("\n", system.CompostQueue.Take(5).Select(b =>
                    $"  {b.batchId}: {b.inputUnits:0.0} units → {b.outputUnits:0.0} × {b.outputItemId} (ready day {b.readyDay})"));
        }

        private static string WasteLabel(WasteType type) => type switch
        {
            WasteType.Chemical => "chemical",
            WasteType.Radioactive => "radioactive",
            _ => "organic"
        };

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Waste & Sanitation // Shelter Hygiene", minWidth: 900, minHeight: 580);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("hygiene", "Shelter Hygiene", "—", AshfallMetricCard.Criticality.Normal, minWidth: 180);
            _statusRail.AddCard("spill", "Spill Status", "none", AshfallMetricCard.Criticality.Normal, minWidth: 170);
            _statusRail.AddCard("compost", "Compost Batches", "0", AshfallMetricCard.Criticality.Normal, minWidth: 150);
            _statusRail.AddCard("rooms", "Rooms Tracked", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("ROOM WASTE LEDGER"));
            _detailText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _contentStack.AddChild(_detailText);

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("COMPOST QUEUE"));
            _compostText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _contentStack.AddChild(_compostText);

            var note = AshfallUiHelpers.MakeBody(
                "Waste accumulates from daily life and industrial work. Facilities process what they are powered and staffed for; cleaning duty trades labor for hygiene. Chemical and radioactive waste never enters the compost path.");
            note.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            note.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            _contentStack.AddChild(note);

            _shell.SetContent(_contentStack);
            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        public override void _ExitTree() => Unbind();
    }
}
