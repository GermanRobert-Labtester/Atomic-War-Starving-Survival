// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using System.Text;
using Godot;
using Ashfall.Core.Medical;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 177 presentation — Cybernetics // Prosthetic Care. Presents the
    /// limb picture truthfully (from the limb authority) and implant component
    /// state (condition, integration, power, maintenance, complications).
    /// No surgical procedural guidance; commands stay in the Core authorities.
    /// </summary>
    public partial class CyberneticsPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _bodyText = null!;
        private Func<BionicsSystem?>? _source;
        private Func<AmputationSystem?>? _limbSource;

        public bool IsBound => _source != null;

        public void Bind(Func<BionicsSystem?> source, Func<AmputationSystem?>? limbSource = null)
        {
            _source = source;
            _limbSource = limbSource;
            RefreshView();
        }

        public void Unbind() { _source = null; _limbSource = null; }

        public void RefreshView()
        {
            if (_source == null || _statusRail == null) return;
            var system = _source();
            if (system == null) return;

            var instances = system.State.implants.Where(i => i != null).ToList();
            var active = instances.Where(i => !i.destroyed).ToList();
            float avgCondition = active.Count > 0 ? active.Average(i => i.condition) : 0f;
            int overdue = active.Count(i =>
            {
                var def = system.Definition(i.implant_id);
                return def != null && camp_lastMaintenanceOverdue(i, def, system.State.last_tick_day);
            });
            bool charging = active.Any(i => i.is_charging);

            _statusRail.Set("implants", $"{active.Count}", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("condition", active.Count > 0 ? $"{avgCondition:0}%" : "—",
                avgCondition < BionicsCaps.ConditionFunctionFloor ? AshfallMetricCard.Criticality.Critical
                : avgCondition < 50f ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("power", active.Count == 0 ? "—" : charging ? "charging" : "on battery",
                active.Count > 0 && !charging ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("faults", $"{active.Count(i => i.malfunction != 0)} active",
                active.Any(i => i.malfunction != 0) ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);

            var lines = new StringBuilder();
            if (instances.Count == 0)
                lines.AppendLine("No bionic implants installed. Lost limbs regain function through prosthetic fittings first, then surgical upgrades.");
            foreach (var i in instances)
            {
                var def = system.Definition(i.implant_id);
                var limb = (LimbId)i.body_slot_limb;
                lines.AppendLine($"{i.survivor_id} — {def?.display_name ?? i.implant_id} ({limb})");
                lines.AppendLine($"  condition {i.condition:0}/100 · installed day {i.installed_day}");
                if (i.destroyed)
                    lines.AppendLine("  DESTROYED — the socket is bare again; a new implant is needed.");
                else
                {
                    if (i.integration_status == (int)ImplantIntegrationStatus.Integrating)
                        lines.AppendLine($"  integrating — {i.integration_days_left} rehab days left (partial function until then)");
                    float bonus = system.GetLimbCapabilityBonusBp(i.survivor_id, limb);
                    lines.AppendLine($"  functional bonus {bonus:0} bp (bounded; the limb authority carries the base)");
                    if (def?.power_profile != "passive")
                        lines.AppendLine($"  power: battery {i.battery_days_remaining:0.0}/{def?.battery_days ?? 0} days · {(i.is_charging ? "charging at the clinic bench" : "drawing internal cell")}");
                    int daysSince = system.State.last_tick_day - i.last_maintenance_day;
                    if (def != null && daysSince > def.maintenance_interval_days)
                        lines.AppendLine($"  MAINTENANCE OVERDUE — {daysSince - def.maintenance_interval_days} days past interval; malfunction risk rises daily");
                    else
                        lines.AppendLine($"  maintenance clock: {Math.Max(0, (def?.maintenance_interval_days ?? 0) - daysSince)} days left");
                    var complication = (ImplantComplication)i.complication;
                    if (complication != ImplantComplication.None)
                        lines.AppendLine($"  complication: {complication.ToString().ToLowerInvariant()} — treatment through the medical pipeline");
                    var malfunction = (ImplantMalfunction)i.malfunction;
                    if (malfunction != ImplantMalfunction.None)
                        lines.AppendLine($"  MALFUNCTION: {malfunction.ToString().ToLowerInvariant()} ({i.malfunction_days_left} days)");
                    if (def?.electrical_vulnerability == "high")
                        lines.AppendLine("  RISK: electrostatic fields and surges hit this model hard — power drains, actuators can lock.");
                    lines.AppendLine();
                }
            }

            // Limb picture from the limb authority (single body model).
            var limbSource = _limbSource?.Invoke();
            if (limbSource != null)
            {
                var limbs = limbSource.EnsureSurvivorLimbs("s_amputee");
                var worn = limbs.Where(l => l.condition != LimbCondition.Intact).ToList();
                if (worn.Count > 0)
                {
                    lines.AppendLine("Limb ledger (from the amputation authority):");
                    foreach (var l in worn)
                        lines.AppendLine($"  {l.limb}: {l.condition} · recovery {l.recoveryDaysLeft}d · socket: {l.prostheticId ?? "none"}");
                }
            }
            _bodyText.Text = lines.ToString().TrimEnd();
        }

        private static bool camp_lastMaintenanceOverdue(ImplantInstanceState i, ImplantDefinition def, int today)
            => today - i.last_maintenance_day > def.maintenance_interval_days + BionicsCaps.OverdueMalfunctionGraceDays;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            _shell = new AshfallDashboardShell("Cybernetics // Prosthetic Care", minWidth: 900, minHeight: 580);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("implants", "Implants", "0", AshfallMetricCard.Criticality.Normal, minWidth: 150);
            _statusRail.AddCard("condition", "Avg Condition", "—", AshfallMetricCard.Criticality.Normal, minWidth: 160);
            _statusRail.AddCard("power", "Power", "—", AshfallMetricCard.Criticality.Normal, minWidth: 150);
            _statusRail.AddCard("faults", "Faults", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("IMPLANT REGISTRY"));
            _bodyText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _contentStack.AddChild(_bodyText);

            var note = AshfallUiHelpers.MakeBody(
                "Implants restore what trauma took — and add upkeep the body never asked for. They need scheduled maintenance, charge from the clinic bench, and time to integrate; they also give storms and surges new ways to hurt the person carrying them. Some people find that a fair trade; some never do.");
            note.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            note.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            _contentStack.AddChild(note);

            _shell.SetContent(_contentStack);
            _shell.AttachHeaderCloseButton("CLOSE", () => { Visible = false; OnClose?.Invoke(); });
            RefreshView();
        }

        public override void _ExitTree() => Unbind();
    }
}
