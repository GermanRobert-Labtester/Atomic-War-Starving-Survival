// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using System.Text;
using Godot;
using Ashfall.Core.Survivors;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 175 presentation — Beliefs // Doctrinal Climate (setting-neutral
    /// naming, §7.18). Presents the fictional movements present in the shelter:
    /// adherence, fervor, dissent, tensions, ritual demands. No real-world
    /// iconography; no gameplay math here — the Core authority owns it.
    /// </summary>
    public partial class BeliefsPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _climateText = null!;

        private Func<ZealotrySystem?>? _source;

        public bool IsBound => _source != null;

        public void Bind(Func<ZealotrySystem?> source)
        {
            _source = source;
            RefreshView();
        }

        public void Unbind() => _source = null;

        public void RefreshView()
        {
            if (_source == null || _statusRail == null) return;
            var system = _source();
            if (system == null) return;

            var believers = system.State.believers.Where(b => b != null && !string.IsNullOrEmpty(b.belief_id)).ToList();
            float avgFervor = believers.Count > 0 ? (float)believers.Average(b => b.fervor) : 0f;
            int inCrisis = believers.Count(b => b.in_crisis);
            var stage = (ZealotEscalationStage)system.State.escalation_stage;

            _statusRail.Set("adherents", $"{believers.Count}", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("fervor", $"{avgFervor:0}/100",
                avgFervor >= 70 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("tension", stage == ZealotEscalationStage.None ? "calm" : stage.ToString(),
                stage >= ZealotEscalationStage.PropertyDamage ? AshfallMetricCard.Criticality.Critical
                : stage >= ZealotEscalationStage.Ostracism ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("crisis", inCrisis > 0 ? $"{inCrisis} shaken" : "none",
                inCrisis > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);

            var lines = new StringBuilder();
            if (believers.Count == 0)
                lines.AppendLine("No held doctrines in the shelter. Movements find recruits when stress runs high and a voice speaks for them.");
            foreach (var b in believers)
            {
                var profile = system.Profile(b.belief_id);
                string role = ((BeliefRole)b.role) switch
                {
                    BeliefRole.Leader => "speaks for the movement",
                    BeliefRole.Devout => "devout",
                    _ => "adherent"
                };
                lines.AppendLine($"{b.survivor_id} — {profile?.display_name ?? b.belief_id} ({role})");
                lines.AppendLine($"  conviction {b.conviction}/100 · fervor {b.fervor}/100 · doubt {b.dissent}/100 · joined day {b.converted_day}");
                int despair = system.GetDespairResistanceBp(b.survivor_id);
                int cohesion = system.GetCohesionBonusBp(b.belief_id);
                lines.AppendLine($"  steadies against despair by {despair} bp; group pull {cohesion} bp");
                if (system.HasShrine(b.belief_id))
                    lines.AppendLine("  a meeting space is kept for this movement.");
                if (b.in_crisis)
                    lines.AppendLine("  CRISIS: belief shaken — the person is raw, and morale is paying for it.");
                else if (b.fervor >= (profile?.fanaticism_threshold ?? 100))
                    lines.AppendLine("  RISK: fervor has passed the movement's danger line — expect friction with dissenters.");
                lines.AppendLine();
            }

            lines.AppendLine($"Doctrinal tension: {(stage == ZealotEscalationStage.None ? "calm" : stage.ToString())}.");
            if (stage >= ZealotEscalationStage.PropertyDamage)
                lines.AppendLine("  Escalating disagreements are being watched. Any real violence is handled through the shelter's own justice and defenses — never by doctrine alone.");
            if (system.State.unresolved_demands.Count > 0)
            {
                lines.AppendLine("Ritual demands awaiting offerings:");
                foreach (var demandId in system.State.unresolved_demands)
                {
                    var p = system.Profile(demandId);
                    if (p != null)
                        lines.AppendLine($"  {p.display_name}: {string.Join(", ", p.ritual_resource_item_ids)}");
                }
            }
            else
                lines.AppendLine("No ritual demands pending.");
            _climateText.Text = lines.ToString().TrimEnd();
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            _shell = new AshfallDashboardShell("Beliefs // Doctrinal Climate", minWidth: 900, minHeight: 560);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("adherents", "Adherents", "0", AshfallMetricCard.Criticality.Normal, minWidth: 150);
            _statusRail.AddCard("fervor", "Average Fervor", "—", AshfallMetricCard.Criticality.Normal, minWidth: 160);
            _statusRail.AddCard("tension", "Tension", "calm", AshfallMetricCard.Criticality.Normal, minWidth: 160);
            _statusRail.AddCard("crisis", "Belief Crises", "none", AshfallMetricCard.Criticality.Normal, minWidth: 150);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("HELD DOCTRINES"));
            _climateText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _contentStack.AddChild(_climateText);

            var note = AshfallUiHelpers.MakeBody(
                "Belief gives people steadiness and a reason to work together — and, at high fervor, sharp edges against anyone who dissents. Rites cost real stores. These movements are the shelter's own; the world outside has its own religions and none of them are ours to caricature.");
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
