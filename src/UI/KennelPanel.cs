// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using System.Text;
using Godot;
using Ashfall.Core.Ecology;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 174 presentation — Kennel // Companion Animals. Presentation only:
    /// state → need/blocker → benefit → risk → consequence per §19. All bonus
    /// math stays in the Core authority; this panel reads it truthfully.
    /// </summary>
    public partial class KennelPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _rosterText = null!;

        private Func<CompanionAnimalSystem?>? _source;

        public bool IsBound => _source != null;

        public void Bind(Func<CompanionAnimalSystem?> source)
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

            var alive = system.State.companions.Where(c => c != null && c.alive).ToList();
            int hungry = alive.Count(c => c.hunger >= CompanionAnimalSystem.HungerCritical);
            int sick = alive.Count(c => c.sickness != (int)CompanionSicknessState.Healthy);
            float avgBond = alive.Count > 0 ? (float)alive.Average(c => c.bond) : 0f;

            _statusRail.Set("roster", $"{alive.Count} animals",
                alive.Count == 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("bond", $"{avgBond:0}/100", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("hungry", hungry > 0 ? $"{hungry} STARVING" : "none",
                hungry > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("sick", sick > 0 ? $"{sick} need care" : "none",
                sick > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);

            var lines = new StringBuilder();
            if (alive.Count == 0)
                lines.AppendLine("No companion animals yet. Tamed wildlife joins the pens through the bestiary's tame route.");
            foreach (var c in alive)
            {
                var profile = system.Profile(c.species_id);
                string role = ((CompanionRole)c.role).ToString();
                float guard = system.GetGuardModifier(c.companion_id);
                float pack = system.GetPackCapacityBonus(c.companion_id);
                int support = system.GetMoraleSupportBp(c.companion_id);
                int feed = Math.Max(1, profile?.base_food_per_day ?? 1);
                bool fedToday = c.last_fed_day >= 0;

                lines.AppendLine($"{c.name} ({profile?.display_name ?? c.species_id}) — {role} duty");
                lines.AppendLine($"  health {c.health}/{profile?.max_health ?? 50} · hunger {c.hunger}/100 · bond {c.bond}/100 · training {(CompanionTrainingLevel)c.training_level}");
                lines.AppendLine($"  handler: {(string.IsNullOrEmpty(c.assigned_survivor_id) ? "none" : c.assigned_survivor_id)} · upkeep {feed} feed/day · fed day {c.last_fed_day}");
                string benefit = ((CompanionRole)c.role) switch
                {
                    CompanionRole.Guard => $"guard rating {guard:0} (fades when hurt or starving)",
                    CompanionRole.Pack => $"carries +{pack:0} kg on expeditions",
                    CompanionRole.Morale => $"morale support {support} bp for the bonded survivor",
                    _ => "unassigned — no benefit while idle"
                };
                lines.AppendLine($"  benefit: {benefit}");
                if (c.hunger >= CompanionAnimalSystem.HungerCritical)
                    lines.AppendLine("  BLOCKER: starving — health drains daily until fed.");
                else if (c.sickness != (int)CompanionSicknessState.Healthy)
                    lines.AppendLine("  RISK: sick — treat with a veterinary kit or wait out recovery.");
                if (!string.IsNullOrEmpty(c.assigned_survivor_id) && c.on_expedition)
                    lines.AppendLine("  away with the handler's expedition.");
                lines.AppendLine();
            }
            _rosterText.Text = lines.ToString().TrimEnd();
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            _shell = new AshfallDashboardShell("Kennel // Companion Animals", minWidth: 900, minHeight: 560);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("roster", "Companions", "0", AshfallMetricCard.Criticality.Normal, minWidth: 160);
            _statusRail.AddCard("bond", "Average Bond", "—", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("hungry", "Starving", "none", AshfallMetricCard.Criticality.Normal, minWidth: 150);
            _statusRail.AddCard("sick", "Under Care", "none", AshfallMetricCard.Criticality.Normal, minWidth: 150);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("ANIMAL ROSTER"));
            _rosterText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _contentStack.AddChild(_rosterText);

            var note = AshfallUiHelpers.MakeBody(
                "Animals need daily feed from the stores, a handler for training, and care when sick. Guard animals sharpen night watch; pack animals carry extra weight on expeditions; small companions steady the people bonded to them. A companion's death weighs on its bonded survivor — care is cheaper than grief.");
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
