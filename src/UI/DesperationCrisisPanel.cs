using System;
using Godot;
using Ashfall.Core.Survivors;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    public partial class DesperationCrisisPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _btnHarvest = null!;
        private Button _btnBurial = null!;

        private DesperationSystem? _system;

        public bool IsBound => _system != null;

        public void Bind(DesperationSystem system)
        {
            _system = system;
            RefreshView();
        }

        public void Unbind()
        {
            _system = null;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("SANCTUARY CRISIS // DESPERATION & TABOO MONITOR", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("starvation", "Starvation Crisis", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("mutiny", "Mutiny Pressure", "0%", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("corpses", "Unburied Remains", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("taboo", "Taboo State", "INTACT", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 16);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.Text = "Sanctuary is stable. No severe desperation protocols required.";
            _contentStack.AddChild(_detailText);

            var buttonRow = new HBoxContainer();
            buttonRow.AddThemeConstantOverride("separation", 16);

            _btnHarvest = new Button { Text = "EMERGENCY RATION HARVEST" };
            _btnHarvest.CustomMinimumSize = new Vector2(300, 40);
            _btnHarvest.Pressed += () => {
                if (_system != null && _system.State.unburiedCorpseIds.Count > 0)
                {
                    /* _system.HarvestCorpse(actorId, corpseId, "desperation_consume_corpse", 1); */
                    RefreshView();
                }
            };
            buttonRow.AddChild(_btnHarvest);

            _btnBurial = new Button { Text = "SANCTIFIED BURIAL" };
            _btnBurial.CustomMinimumSize = new Vector2(300, 40);
            _btnBurial.Pressed += () => {
                if (_system != null && _system.State.unburiedCorpseIds.Count > 0)
                {
                    /* _system.State.unburiedCorpseIds.RemoveAt(0); // Mocked burial */
                    RefreshView();
                }
            };
            buttonRow.AddChild(_btnBurial);

            _contentStack.AddChild(buttonRow);
            _shell.SetContent(_contentStack);
            _shell.AttachHeaderCloseButton("CLOSE (ESC)", () => OnClose?.Invoke());

            Visible = false;
            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public void Close()
        {
            Visible = false;
            OnClose?.Invoke();
        }

        public void RefreshView()
        {
            if (_statusRail == null || _detailText == null) return;
            if (_system == null)
            {
                _detailText.Text = "Desperation monitor offline.";
                _btnHarvest.Disabled = true;
                _btnBurial.Disabled = true;
                return;
            }

            int starving = 0; // Requires cross-system querying in UI
            _statusRail.Set("starvation", starving.ToString(), starving > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);

            float mutiny = _system.MutinyPressure * 100f;
            _statusRail.Set("mutiny", $"{mutiny:F0}%", mutiny > 70 ? AshfallMetricCard.Criticality.Critical : (mutiny > 40 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal));

            int corpses = _system.State.unburiedCorpseIds.Count;
            _statusRail.Set("corpses", corpses.ToString(), corpses > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);

            bool tabooBroken = (_system.State.actsHistory.Count > 0);
            _statusRail.Set("taboo", tabooBroken ? "BROKEN" : "INTACT", tabooBroken ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);

            _btnHarvest.Disabled = corpses == 0;
            _btnBurial.Disabled = corpses == 0;

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("=== CRISIS LOG ===");
            if (corpses == 0 && starving == 0 && mutiny < 20)
            {
                sb.AppendLine("  Order maintained.");
            }
            else
            {
                sb.AppendLine($"  {starving} dwellers are critically starving.");
                sb.AppendLine($"  {corpses} bodies await processing or burial.");
                if (tabooBroken)
                {
                    sb.AppendLine("  WARNING: Survival taboo has been broken. Trust is shattered.");
                }
            }
            _detailText.Text = sb.ToString();
        }
    }
}
