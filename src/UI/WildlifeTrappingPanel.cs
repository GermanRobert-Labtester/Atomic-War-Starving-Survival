using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class WildlifeTrappingPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _setTrapBtn = null!;
        private Button _checkTrapBtn = null!;
        private Button _repairBtn = null!;

        private WildlifeTrappingHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(WildlifeTrappingHostSession session)
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

            _shell = new AshfallDashboardShell("Wildlife Trapping // Snare Network", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("traps_active", "Active Snares", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("total_catch", "Total Harvest", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("trap_type", "Trap Type", "—", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("trap_condition", "Condition", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _contentStack.AddChild(_detailText);

            var buttonRow = new HBoxContainer();
            buttonRow.AddThemeConstantOverride("separation", 10);

            _setTrapBtn = new Button { Text = "Set Snare at Perimeter", CustomMinimumSize = new Vector2(180, 36) };
            _setTrapBtn.Pressed += () =>
            {
                if (_host?.Catalog != null)
                    _host.TrySetTrap("snare_perimeter_north", "trap_snare", "bait_grain_lure", "Hunter");
                else
                    _host?.SetTrap("snare_perimeter_north", "bait_grain_lure", "Hunter");
            };
            buttonRow.AddChild(_setTrapBtn);

            _checkTrapBtn = new Button { Text = "Check & Harvest Snares", CustomMinimumSize = new Vector2(180, 36) };
            _checkTrapBtn.Pressed += () => _host?.CheckTraps();
            buttonRow.AddChild(_checkTrapBtn);

            _repairBtn = new Button { Text = "Repair Trap", CustomMinimumSize = new Vector2(140, 36) };
            _repairBtn.Pressed += () =>
            {
                if (_host != null)
                {
                    // Repair first broken catalog-linked trap
                    foreach (var site in _host.System.State.trapSites)
                    {
                        if (site.isBroken && !string.IsNullOrEmpty(site.trapId))
                        {
                            _host.TryRepairTrap(site.siteId);
                            break;
                        }
                    }
                }
            };
            _repairBtn.Visible = false;
            buttonRow.AddChild(_repairBtn);

            _contentStack.AddChild(buttonRow);
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

            var s = _host.System.State;
            _statusRail.Set("traps_active", s.trapSites.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("total_catch", s.totalCatch.ToString(), AshfallMetricCard.Criticality.Normal);

            // Show first trap's type and condition in status rail
            bool hasBroken = false;
            if (s.trapSites.Count > 0)
            {
                var first = s.trapSites[0];
                string trapName = first.trapType;
                if (_host.Catalog != null && !string.IsNullOrEmpty(first.trapId)
                    && _host.Catalog.Traps.TryGetValue(first.trapId, out var trapDef))
                    trapName = trapDef.displayName;
                _statusRail.Set("trap_type", trapName, AshfallMetricCard.Criticality.Normal);

                if (first.isBroken)
                {
                    _statusRail.Set("trap_condition", "BROKEN", AshfallMetricCard.Criticality.Critical);
                    hasBroken = true;
                }
                else if (first.remainingDurability > 0)
                {
                    int max = first.remainingDurability; // approximate; actual max from catalog
                    if (_host.Catalog != null && !string.IsNullOrEmpty(first.trapId)
                        && _host.Catalog.Traps.TryGetValue(first.trapId, out var def))
                        max = def.durabilityChecks;
                    _statusRail.Set("trap_condition", $"{first.remainingDurability}/{max}",
                        first.remainingDurability <= max / 3 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
                }
                else
                {
                    _statusRail.Set("trap_condition", "—", AshfallMetricCard.Criticality.Normal);
                }
            }
            else
            {
                _statusRail.Set("trap_type", "—", AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("trap_condition", "—", AshfallMetricCard.Criticality.Normal);
            }

            // Show repair button only when a broken catalog-linked trap exists
            if (_repairBtn != null)
                _repairBtn.Visible = hasBroken;

            if (_detailText != null)
            {
                string text = $"Wildlife Trapping Network ({s.trapSites.Count} sites):\n";
                foreach (var t in s.trapSites)
                {
                    string name = t.trapType;
                    if (_host.Catalog != null && !string.IsNullOrEmpty(t.trapId)
                        && _host.Catalog.Traps.TryGetValue(t.trapId, out var td))
                        name = td.displayName;
                    string condition = t.isBroken ? "BROKEN"
                        : t.remainingDurability > 0 ? $"{t.remainingDurability} checks left"
                        : "—";
                    text += $"  • {name} at {t.siteId} — {condition}" +
                        (t.hasCatch ? $" — CATCH READY ({t.catchSpecies})" : " — Armed") + "\n";
                }
                text += $"\nTotal Toxins Neutralized: {s.totalToxicRemoved} | Last Event: {_host.LastEvent}";
                _detailText.Text = text;
            }
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
