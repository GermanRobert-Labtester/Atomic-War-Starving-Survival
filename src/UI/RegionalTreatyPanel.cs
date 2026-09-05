using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class RegionalTreatyPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;

        private RegionalTreatyHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(RegionalTreatyHostSession session)
        {
            _host = session;
            if (_host != null)
            {
                _host.StateChanged += RefreshView;
            }
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



        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Regional Treaty // Diplomatic Accords", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("active_treaties", "Active Accords", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("compliance", "Compliance Rate", "100%", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = AshfallUiHelpers.MakeBody("", autowrap: true);
            _contentStack.AddChild(_detailText);

            _shell.SetContent(_contentStack);

            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        public void RefreshView()
        {
            if (_host == null || _statusRail == null) return;

            var sys = _host.System;
            var s = sys.State;
            int active = s.treaties.FindAll(t => t.status == TreatyStatus.Active || t.status == TreatyStatus.Ratified).Count;
            _statusRail.Set("active_treaties", active.ToString(), AshfallMetricCard.Criticality.Normal);

            // Plan VIII · Task 21.12 — honest compliance: the mean obligation
            // score of live accords, not a hardcoded constant.
            float compliance = 0f;
            if (active > 0)
            {
                float total = 0f;
                foreach (var t in s.treaties)
                    if (t.status == TreatyStatus.Active || t.status == TreatyStatus.Ratified)
                        total += t.complianceScore;
                compliance = total / active;
            }
            var criticality = active == 0 || compliance >= 0.8f
                ? AshfallMetricCard.Criticality.Normal
                : compliance >= 0.5f ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Warn;
            _statusRail.Set("compliance", active > 0 ? $"{compliance:P0}" : "—", criticality);

            if (_detailText == null) return;

            if (s.treaties.Count == 0)
            {
                _detailText.Text = "No regional treaties or diplomatic accords currently on record.\nDiplomatic envoys and faction emissaries will negotiate regional accords during diplomatic outreach.\n\nLast Event: " + (string.IsNullOrEmpty(_host.LastEvent) ? "None recorded" : _host.LastEvent);
                return;
            }

            // Task 21.12 — consequences read authoritative effect state; the
            // panel never recomputes hidden modifiers independently.
            var descriptors = sys.GetActiveEffectDescriptors();
            float raidMod = sys.GetRaidPressureModifier();

            string text = $"Regional Diplomatic Treaties ({s.treaties.Count} total):\n";
            foreach (var t in s.treaties)
            {
                var def = sys.GetDefinition(t.treatyId);
                string name = def != null && !string.IsNullOrEmpty(def.display_name) ? def.display_name : t.treatyId;
                text += $"  • [{t.status}] {name} (Compliance: {t.complianceScore:P0}";
                if (t.ratifiedDay >= 0) text += $", Ratified Day: {t.ratifiedDay}";
                if (t.violatedDay >= 0) text += $", Violated Day: {t.violatedDay}";
                text += ")\n";
                foreach (var d in descriptors)
                {
                    if (d.TreatyId != t.treatyId) continue;
                    text += "      — " + DescribeEffect(d) + "\n";
                }
                if (t.status == TreatyStatus.Violated)
                    text += "      — benefits lost; raid pressure raised\n";
                if (t.status == TreatyStatus.Expired)
                    text += "      — term served; effects ended\n";
            }

            if (descriptors.Count > 0 || System.MathF.Abs(raidMod) > 0.0005f)
            {
                text += "\nActive consequences:\n";
                if (System.MathF.Abs(raidMod) > 0.0005f)
                    text += $"  • Raid pressure {raidMod:+0%;-0%} from treaty standing\n";
            }

            text += $"\nLast Event: {_host.LastEvent}";
            _detailText.Text = text;
        }

        private static string DescribeEffect(TreatyActiveEffect d) => d.Kind switch
        {
            TreatyEffectKind.TradeDiscount => $"Caravan prices −{d.Value * 100f:0}% while ratified",
            TreatyEffectKind.SupplyPriceRelief => $"Supply prices −{d.Value * 100f:0}% while ratified",
            TreatyEffectKind.RaidPressureRelief => $"Raid pressure reduced ({d.Value * 100f:0}) while ratified",
            TreatyEffectKind.WaterQuota => $"Water quota: {d.Value:0} L/day secured",
            TreatyEffectKind.PowerQuota => $"Power quota: {d.Value:0} kW secured",
            _ => d.SourceId
        };

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
