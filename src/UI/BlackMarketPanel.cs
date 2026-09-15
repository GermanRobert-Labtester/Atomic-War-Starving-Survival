// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Godot;
using Ashfall.Core.Economy;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 211 Phase 9 — black market counter panel. Presents discovered
    /// syndicates, their daily-persistent stock with canonical-derived
    /// prices, debt status with explicit due dates, and heat/trust standing.
    /// Hidden internals (stock weights, RNG) are never shown.
    /// </summary>
    public partial class BlackMarketPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _stockText = null!;
        private Label _debtText = null!;

        private BlackMarketHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(BlackMarketHostSession session)
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
            var contacts = system.DiscoveredContacts;
            int activeDebts = system.State.debts.Count(d => d != null && d.status == UnderworldDebtRecord.StatusActive);
            float heat = contacts.Count > 0 ? system.State.syndicates.Max(s => s!.heat) : 0f;

            _statusRail.Set("contacts", $"{contacts.Count}", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("debts", activeDebts == 0 ? "none" : $"{activeDebts} active",
                activeDebts > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("heat", heat <= 25f ? "low" : heat <= 60f ? "elevated" : "HIGH",
                heat > 60f ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("stock", $"{system.GetStock(contacts.FirstOrDefault() ?? string.Empty).Count} lines",
                AshfallMetricCard.Criticality.Normal);

            var stock = new System.Text.StringBuilder();
            if (contacts.Count == 0)
                stock.Append("No underworld contacts known. Contacts are found, not bought — expedition and radio work may introduce you.");
            foreach (var contactId in contacts)
            {
                var ledger = system.FindLedger(contactId)!;
                stock.AppendLine($"{contactId} · trust {ledger.trust:0}/100 · heat {ledger.heat:0}/100 · access tier {ledger.accessTier}");
                foreach (var line in system.GetStock(contactId))
                {
                    var entry = system.Catalog.FindEntry(line.entryId);
                    if (entry == null) continue;
                    float buy = system.GetBuyPrice(contactId, entry);
                    float sell = system.GetSellPrice(contactId, entry);
                    stock.AppendLine($"  {line.entryId} ×{line.quantity} — buy {buy:0.00} / sells for {sell:0.00}");
                }
                stock.AppendLine();
            }
            _stockText.Text = stock.ToString().TrimEnd();

            var debts = system.State.debts.Where(d => d != null).ToList();
            _debtText.Text = debts.Count == 0
                ? "No ledger entries. Loans build trust and bind tight."
                : string.Join("\n", debts.Select(d =>
                    d.status == UnderworldDebtRecord.StatusActive
                        ? $"  ACTIVE: {d.debtId} · {d.syndicateId} · {d.principalUnits - d.repaidUnits:0} owed · DUE DAY {d.dueDay}"
                    : d.status == UnderworldDebtRecord.StatusDefaulted
                        ? $"  DEFAULTED: {d.debtId} · {d.syndicateId} — a bounty is out. The due date was day {d.dueDay}."
                        : $"  repaid: {d.debtId} · {d.syndicateId}"));
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("The Quiet Counter // Underworld Trade", minWidth: 920, minHeight: 600);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("contacts", "Contacts", "0", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("debts", "Debts", "none", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("heat", "Heat", "low", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("stock", "Stock Lines", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("SYNDICATE STOCK"));
            _stockText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _contentStack.AddChild(_stockText);

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("LEDGER"));
            _debtText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _contentStack.AddChild(_debtText);

            var note = AshfallUiHelpers.MakeBody(
                "Illicit prices ride the same market as the legitimate trade, with a risk premium that never drops below the honest counter. Selling contraband here always loses value against the market. Missed due dates cost trust, raise heat, and put a bounty out.");
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
