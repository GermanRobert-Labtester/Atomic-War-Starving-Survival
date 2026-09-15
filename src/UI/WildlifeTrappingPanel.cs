// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Localization;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using AtomicWar.GodotApp.Localization;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class WildlifeTrappingPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        /// <summary>The single perimeter site this panel manages.</summary>
        private const string ManagedSiteId = "snare_perimeter_north";
        /// <summary>Catalog trap deployed by the Set/Replace action when the catalog is bound.</summary>
        private const string DefaultTrapId = "trap_snare";

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _setTrapBtn = null!;
        private Button _checkTrapBtn = null!;
        private Button _butcherBtn = null!;
        private Button _preserveHideBtn = null!;
        private Button _repairBtn = null!;
        private Button _removeBtn = null!;
        private OptionButton _repairSiteDropdown = null!;

        private readonly List<TrapSite> _brokenSites = new();
        private readonly List<string> _knownSiteCardKeys = new();
        private string? _selectedRepairSiteId;

        private WildlifeTrappingHostSession? _host;

        public bool IsBound => _host != null;

        // Test / inspection accessors
        public string? SelectedRepairSiteId => _selectedRepairSiteId;
        public Button? RepairButton => _repairBtn;
        public OptionButton? RepairSiteDropdown => _repairSiteDropdown;
        public Button? SetTrapButton => _setTrapBtn;
        public AshfallStatusRail? StatusRail => _statusRail;

        public void Bind(WildlifeTrappingHostSession session)
        {
            _host = session;
            if (_host != null)
            {
                _host.StateChanged += RefreshView;
                LocalizationService.Instance.OnLocaleChanged += OnLocaleChanged;
            }
            RefreshView();
        }

        public void Unbind()
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
                LocalizationService.Instance.OnLocaleChanged -= OnLocaleChanged;
                _host = null;
            }
        }

        private void OnLocaleChanged(string _) => RefreshView();

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Wildlife Trapping // Snare Network", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("traps_active", "Active Snares", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("total_catch", "Total Harvest", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);

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
                    _host.TrySetTrap(ManagedSiteId, DefaultTrapId, "bait_grain_lure", "Hunter");
                else
                    _host?.SetTrap(ManagedSiteId, "bait_grain_lure", "Hunter");
                RefreshView();
            };
            buttonRow.AddChild(_setTrapBtn);

            _checkTrapBtn = new Button { Text = "Check & Harvest Snares", CustomMinimumSize = new Vector2(180, 36) };
            _checkTrapBtn.Pressed += () => _host?.CheckTraps();
            buttonRow.AddChild(_checkTrapBtn);

            _butcherBtn = new Button { Text = "Butcher Catch", CustomMinimumSize = new Vector2(140, 36) };
            _butcherBtn.Pressed += () =>
            {
                if (_host == null) return;
                _host.Butcher(ManagedSiteId, "Hunter");
                RefreshView();
            };
            _butcherBtn.Visible = false;
            buttonRow.AddChild(_butcherBtn);

            _preserveHideBtn = new Button { Text = "Preserve Hide", CustomMinimumSize = new Vector2(140, 36) };
            _preserveHideBtn.Pressed += () =>
            {
                if (_host == null) return;
                _host.PreserveHide(ManagedSiteId);
                RefreshView();
            };
            _preserveHideBtn.Visible = false;
            buttonRow.AddChild(_preserveHideBtn);

            _repairSiteDropdown = new OptionButton { CustomMinimumSize = new Vector2(200, 36), Visible = false };
            _repairSiteDropdown.ItemSelected += (index) =>
            {
                if (index >= 0 && index < _brokenSites.Count)
                {
                    _selectedRepairSiteId = _brokenSites[(int)index].siteId;
                    UpdateRepairButtonState();
                }
            };
            buttonRow.AddChild(_repairSiteDropdown);

            _repairBtn = new Button { Text = "Repair Trap", CustomMinimumSize = new Vector2(140, 36) };
            _repairBtn.Pressed += () =>
            {
                if (_host != null && !string.IsNullOrEmpty(_selectedRepairSiteId))
                {
                    _host.TryRepairTrap(_selectedRepairSiteId);
                    RefreshView();
                }
            };
            _repairBtn.Visible = false;
            buttonRow.AddChild(_repairBtn);

            _removeBtn = new Button { Text = "Remove Trap", CustomMinimumSize = new Vector2(140, 36) };
            _removeBtn.Pressed += () =>
            {
                if (_host != null && _host.System.State.trapSites.Find(t => t.siteId == ManagedSiteId) != null)
                    _host.RemoveTrap(ManagedSiteId);
                RefreshView();
            };
            buttonRow.AddChild(_removeBtn);

            _contentStack.AddChild(buttonRow);
            _shell.SetContent(_contentStack);

            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        public void SelectRepairSite(string siteId)
        {
            if (string.IsNullOrEmpty(siteId)) return;
            for (int i = 0; i < _brokenSites.Count; i++)
            {
                if (_brokenSites[i].siteId == siteId)
                {
                    _selectedRepairSiteId = siteId;
                    if (_repairSiteDropdown != null && _repairSiteDropdown.Visible)
                        _repairSiteDropdown.Select(i);
                    UpdateRepairButtonState();
                    return;
                }
            }
        }

        public void RefreshView()
        {
            if (_host == null || _statusRail == null) return;

            var s = _host.System.State;
            _statusRail.Set("traps_active", s.trapSites.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("total_catch", s.totalCatch.ToString(), AshfallMetricCard.Criticality.Normal);

            // Multi-site dynamic cards
            var activeKeys = new HashSet<string>(StringComparer.Ordinal);
            if (s.trapSites.Count == 0)
            {
                // Clean up any site cards
                for (int i = _knownSiteCardKeys.Count - 1; i >= 0; i--)
                {
                    _statusRail.RemoveCard(_knownSiteCardKeys[i]);
                }
                _knownSiteCardKeys.Clear();

                if (!_statusRail.HasCard("empty"))
                {
                    _statusRail.AddCard("empty", "NO SITES", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
                }
                _statusRail.ReorderCard("empty", 2);
            }
            else
            {
                // Remove placeholder empty card
                if (_statusRail.HasCard("empty"))
                {
                    _statusRail.RemoveCard("empty");
                }

                for (int i = 0; i < s.trapSites.Count; i++)
                {
                    var site = s.trapSites[i];
                    string cardKey = "site_" + site.siteId;
                    activeKeys.Add(cardKey);

                    string trapName = site.trapType;
                    int maxDurability = 0;
                    if (_host?.Catalog != null && !string.IsNullOrEmpty(site.trapId)
                        && _host.Catalog.Traps.TryGetValue(site.trapId, out var trapDef))
                    {
                        trapName = AshfallLocalization.Tr(
                            WildlifeTrappingLocalization.TrapNameKey(site.trapId), trapDef.displayName);
                        maxDurability = trapDef.durabilityChecks;
                    }
                    else if (site.remainingDurability > 0)
                    {
                        maxDurability = site.remainingDurability;
                    }

                    string condition;
                    AshfallMetricCard.Criticality crit;

                    if (site.isBroken)
                    {
                        condition = "BROKEN";
                        crit = AshfallMetricCard.Criticality.Critical;
                    }
                    else if (site.remainingDurability > 0)
                    {
                        condition = maxDurability > 0
                            ? $"{site.remainingDurability}/{maxDurability}"
                            : $"{site.remainingDurability}";
                        crit = (maxDurability > 0 && (float)site.remainingDurability / maxDurability <= 0.334f)
                            ? AshfallMetricCard.Criticality.Warn
                            : AshfallMetricCard.Criticality.Normal;
                    }
                    else if (site.remainingDurability == 0)
                    {
                        // Zero durability with isBroken=false: malformed or depleted trap
                        condition = maxDurability > 0 ? $"0/{maxDurability}" : "0";
                        crit = AshfallMetricCard.Criticality.Critical;
                    }
                    else
                    {
                        // Sentinel durability value (-1): legacy/untracked
                        condition = "—";
                        crit = AshfallMetricCard.Criticality.Normal;
                    }

                    string valueText = site.hasCatch ? $"{condition} • CATCH" : condition;

                    if (_statusRail.HasCard(cardKey))
                    {
                        _statusRail.Set(cardKey, valueText, crit);
                        _statusRail.SetLabel(cardKey, trapName);
                    }
                    else
                    {
                        _statusRail.AddCard(cardKey, trapName, valueText, crit, minWidth: 130);
                    }

                    _statusRail.ReorderCard(cardKey, i + 2);
                }

                // Remove stale site cards
                for (int i = _knownSiteCardKeys.Count - 1; i >= 0; i--)
                {
                    string k = _knownSiteCardKeys[i];
                    if (!activeKeys.Contains(k))
                    {
                        _statusRail.RemoveCard(k);
                        _knownSiteCardKeys.RemoveAt(i);
                    }
                }

                _knownSiteCardKeys.Clear();
                _knownSiteCardKeys.AddRange(activeKeys);
            }

            // Update broken sites list for targeted repair
            _brokenSites.Clear();
            foreach (var site in s.trapSites)
            {
                if (site.isBroken && !string.IsNullOrEmpty(site.trapId))
                {
                    _brokenSites.Add(site);
                }
            }

            if (_brokenSites.Count == 0)
            {
                _selectedRepairSiteId = null;
                if (_repairSiteDropdown != null) _repairSiteDropdown.Visible = false;
                if (_repairBtn != null) _repairBtn.Visible = false;
            }
            else if (_brokenSites.Count == 1)
            {
                _selectedRepairSiteId = _brokenSites[0].siteId;
                if (_repairSiteDropdown != null) _repairSiteDropdown.Visible = false;
                if (_repairBtn != null)
                {
                    _repairBtn.Visible = true;
                    UpdateRepairButtonState();
                }
            }
            else
            {
                if (_repairSiteDropdown != null)
                {
                    _repairSiteDropdown.Visible = true;
                    _repairSiteDropdown.Clear();

                    int selectedIdx = -1;
                    for (int i = 0; i < _brokenSites.Count; i++)
                    {
                        var bSite = _brokenSites[i];
                        string trapName = bSite.trapType;
                        if (_host?.Catalog != null && !string.IsNullOrEmpty(bSite.trapId)
                            && _host.Catalog.Traps.TryGetValue(bSite.trapId, out var td))
                        {
                            trapName = AshfallLocalization.Tr(
                                WildlifeTrappingLocalization.TrapNameKey(bSite.trapId), td.displayName);
                        }
                        _repairSiteDropdown.AddItem($"{trapName} ({bSite.siteId})", i);
                        if (_selectedRepairSiteId == bSite.siteId)
                        {
                            selectedIdx = i;
                        }
                    }

                    if (selectedIdx == -1)
                    {
                        selectedIdx = 0;
                        _selectedRepairSiteId = _brokenSites[0].siteId;
                    }

                    _repairSiteDropdown.Select(selectedIdx);
                }

                if (_repairBtn != null)
                {
                    _repairBtn.Visible = true;
                    UpdateRepairButtonState();
                }
            }

            // Deploy/replace control state
            var managedSite = s.trapSites.Find(t => t.siteId == ManagedSiteId);
            if (_setTrapBtn != null)
            {
                string failureCode = string.Empty;
                bool replaceable = _host != null
                    && _host.CanSetTrapAtSite(ManagedSiteId, out failureCode);
                _setTrapBtn.Text = managedSite != null && replaceable
                    ? "Replace Trap at Perimeter"
                    : "Set Snare at Perimeter";
                _setTrapBtn.Disabled = !replaceable;
                if (!replaceable)
                {
                    _setTrapBtn.TooltipText = failureCode == "trap_active"
                        ? "Trap active — check the snare or wait for it to break."
                        : "Trap deployment unavailable.";
                }
                else if (_host != null && _host.TryGetSetupBill(DefaultTrapId, out var setupBill, out _))
                {
                    string costStr = FormatBill(setupBill);
                    bool canAfford = _host.CanAffordSetup(DefaultTrapId, out _, out _);
                    _setTrapBtn.TooltipText = canAfford
                        ? $"Cost: {costStr}"
                        : $"Cost: {costStr} (Insufficient materials)";
                }
                else
                {
                    _setTrapBtn.TooltipText = string.Empty;
                }
            }

            if (_detailText != null)
            {
                string text = $"Wildlife Trapping Network ({s.trapSites.Count} sites):\n";
                foreach (var t in s.trapSites)
                {
                    string name = t.trapType;
                    string description = string.Empty;
                    if (_host?.Catalog != null && !string.IsNullOrEmpty(t.trapId)
                        && _host.Catalog.Traps.TryGetValue(t.trapId, out var td))
                    {
                        name = AshfallLocalization.Tr(
                            WildlifeTrappingLocalization.TrapNameKey(t.trapId), td.displayName);
                        description = AshfallLocalization.Tr(
                            WildlifeTrappingLocalization.TrapDescriptionKey(t.trapId), td.description);
                    }
                    string condition = t.isBroken ? "BROKEN"
                        : t.remainingDurability > 0 ? $"{t.remainingDurability} checks left"
                        : "—";
                    string baitName = t.baitType;
                    if (_host?.Catalog != null && !string.IsNullOrEmpty(t.baitType)
                        && _host.Catalog.Baits.TryGetValue(t.baitType, out var bait))
                        baitName = AshfallLocalization.Tr(
                            WildlifeTrappingLocalization.BaitNameKey(t.baitType), bait.displayName);
                    string catchName = "Uncatalogued catch";
                    string catchDescription = string.Empty;
                    if (_host?.Catalog != null && !string.IsNullOrEmpty(t.catchSpecies)
                        && _host.Catalog.Prey.TryGetValue(t.catchSpecies, out var preyDef))
                    {
                        catchName = AshfallLocalization.Tr(
                            WildlifeTrappingLocalization.PreyNameKey(t.catchSpecies), preyDef.displayName);
                        catchDescription = AshfallLocalization.Tr(
                            WildlifeTrappingLocalization.PreyDescriptionKey(t.catchSpecies), preyDef.description);
                    }
                    text += $"  • {name} at {t.siteId} — {condition} · Bait: {baitName}" +
                        (t.hasCatch ? $" — CATCH READY ({catchName})" : " — Armed") + "\n";
                    if (!string.IsNullOrEmpty(description)) text += $"    {description}\n";
                    if (t.hasCatch && !string.IsNullOrEmpty(catchDescription)) text += $"    {catchDescription}\n";
                }
                text += $"\nTotal Toxins Neutralized: {s.totalToxicRemoved} | Last Event: {_host?.LastEvent ?? string.Empty}";
                _detailText.Text = text;
            }

            if (_removeBtn != null)
                _removeBtn.Visible = managedSite != null;

            bool catchReady = managedSite != null && managedSite.hasCatch && !managedSite.isMeatProcessed;
            bool hideReady = managedSite != null && managedSite.hasCatch
                && managedSite.isMeatProcessed && !managedSite.hidePreserved;
            if (_butcherBtn != null)
                _butcherBtn.Visible = catchReady;
            if (_preserveHideBtn != null)
                _preserveHideBtn.Visible = hideReady;
        }

        private void UpdateRepairButtonState()
        {
            if (_repairBtn == null) return;

            if (_host == null || string.IsNullOrEmpty(_selectedRepairSiteId))
            {
                _repairBtn.Disabled = true;
                _repairBtn.TooltipText = string.Empty;
                return;
            }

            bool hasBill = _host.TryGetRepairBill(_selectedRepairSiteId, out var bill, out string reason);
            if (!hasBill)
            {
                _repairBtn.Disabled = true;
                _repairBtn.TooltipText = string.IsNullOrEmpty(reason) ? "Repair unavailable." : $"Repair unavailable: {reason}";
                return;
            }

            string formattedCost = FormatBill(bill);
            bool canAfford = _host.CanAffordRepair(_selectedRepairSiteId, out _, out string failureReason);

            if (canAfford)
            {
                _repairBtn.Disabled = false;
                _repairBtn.TooltipText = $"Repair: {formattedCost}";
            }
            else
            {
                _repairBtn.Disabled = true;
                string failMsg = failureReason == "trapping.insufficient_materials"
                    ? "Insufficient materials"
                    : failureReason;
                _repairBtn.TooltipText = $"Repair: {formattedCost}\n({failMsg})";
            }
        }

        private string FormatBill(InventoryBill bill)
        {
            if (bill == null || bill.Costs.Count == 0) return "Free";
            var parts = new List<string>();
            foreach (var cost in bill.Costs)
            {
                string name = ResolveItemDisplayName(cost.ItemId);
                parts.Add($"{name} x{cost.Amount}");
            }
            return string.Join(", ", parts);
        }

        private string ResolveItemDisplayName(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return string.Empty;
            if (_host?.Inventory?.Catalog != null)
            {
                var def = _host.Inventory.Catalog.Get(itemId);
                if (def != null && !string.IsNullOrEmpty(def.displayName))
                    return def.displayName;
            }
            return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(itemId.Replace('_', ' '));
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
