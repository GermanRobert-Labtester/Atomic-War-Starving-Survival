// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core.Economy;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 211 black-market counter. Reads authoritative state and routes
    /// player commands through <see cref="BlackMarketHostSession"/>. It does
    /// not calculate prices, mutate inventory, advance time, or refresh stock.
    /// </summary>
    public partial class BlackMarketPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private VBoxContainer _stockRows = null!;
        private VBoxContainer _debtRows = null!;
        private Label _outcomeText = null!;

        private readonly Dictionary<string, Control> _focusTargets =
            new Dictionary<string, Control>(StringComparer.Ordinal);
        private Control? _firstFocusTarget;
        private BlackMarketHostSession? _host;
        private string _lastOutcome = string.Empty;
        private string _pendingFocusKey = string.Empty;

        public bool IsBound => _host != null;

        public void Bind(BlackMarketHostSession session)
        {
            if (ReferenceEquals(_host, session))
            {
                RefreshView();
                return;
            }

            Unbind();
            _host = session;
            if (_host != null)
            {
                _host.StateChanged += RefreshView;
                _host.ActionCompleted += OnActionCompleted;
            }
            RefreshView();
        }

        public void Unbind()
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
                _host.ActionCompleted -= OnActionCompleted;
                _host = null;
            }
            _focusTargets.Clear();
            _firstFocusTarget = null;
        }

        public void RefreshView()
        {
            if (_host == null || _statusRail == null) return;
            var system = _host.System;
            var contacts = system.DiscoveredContacts;
            int activeDebts = system.State.debts.Count(d =>
                d != null && d.status == UnderworldDebtRecord.StatusActive);
            float heat = contacts.Count > 0
                ? system.State.syndicates.Where(s => s != null).Max(s => s!.heat)
                : 0f;

            _statusRail.Set("contacts", $"{contacts.Count}", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("funds", $"{_host.WalletValue}", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("debts", activeDebts == 0 ? "none" : $"{activeDebts} active",
                activeDebts > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("heat", heat <= 25f ? "low" : heat <= 60f ? "elevated" : "HIGH",
                heat > 60f ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("stock", $"{contacts.Sum(id => system.GetStock(id).Count)} lines",
                AshfallMetricCard.Criticality.Normal);

            _focusTargets.Clear();
            _firstFocusTarget = null;
            ClearChildren(_stockRows);
            ClearChildren(_debtRows);
            BuildStockRows(contacts);
            BuildDebtRows();
            _outcomeText.Text = _lastOutcome;

            if (!string.IsNullOrEmpty(_pendingFocusKey) &&
                _focusTargets.TryGetValue(_pendingFocusKey, out var target))
            {
                target.CallDeferred(Control.MethodName.GrabFocus);
                _pendingFocusKey = string.Empty;
            }
        }

        public void FocusFirstAction()
        {
            _firstFocusTarget?.CallDeferred(Control.MethodName.GrabFocus);
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
            FocusFirstAction();
        }

        private void BuildStockRows(IReadOnlyList<string> contacts)
        {
            if (contacts.Count == 0)
            {
                _stockRows.AddChild(AshfallUiHelpers.MakeBody(
                    "No underworld contacts known. Contacts are found through expedition and radio work."));
                return;
            }

            foreach (var contactId in contacts)
            {
                var ledger = _host!.System.FindLedger(contactId)!;
                _stockRows.AddChild(AshfallUiHelpers.MakeSectionHeader(
                    $"{contactId} · TRUST {ledger.trust:0}/100 · HEAT {ledger.heat:0}/100 · TIER {ledger.accessTier}"));

                var lines = _host.System.GetStock(contactId);
                if (lines.Count == 0)
                {
                    _stockRows.AddChild(AshfallUiHelpers.MakeBody("No stock remains at this counter today."));
                }

                foreach (var line in lines)
                    AddStockActionRow(contactId, line);

                AddLoanRow(contactId);
                _stockRows.AddChild(AshfallUiHelpers.MakeSeparator());
            }
        }

        private void AddStockActionRow(string contactId, BlackMarketStockLine line)
        {
            var entry = _host!.System.Catalog.FindEntry(line.entryId);
            if (entry == null) return;

            int held = _host.InventoryCountForEntry(line.entryId);
            var row = new VBoxContainer();
            row.AddThemeConstantOverride("separation", 4);

            var summary = AshfallUiHelpers.MakeBody(
                $"{entry.item_id} · counter {line.quantity} · Holdfast {held}");
            summary.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            row.AddChild(summary);

            var controls = new HBoxContainer();
            controls.AddThemeConstantOverride("separation", 8);
            var quantity = new SpinBox
            {
                MinValue = 1,
                MaxValue = Math.Max(1, Math.Max(line.quantity, held)),
                Step = 1,
                Value = 1,
                AllowGreater = false,
                AllowLesser = false,
                CustomMinimumSize = new Vector2(90, 0)
            };
            var buy = new Button { Text = "BUY" };
            var sell = new Button { Text = "SELL" };
            controls.AddChild(quantity);
            controls.AddChild(buy);
            controls.AddChild(sell);
            row.AddChild(controls);

            var reason = AshfallUiHelpers.MakeBody(string.Empty);
            reason.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            reason.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            row.AddChild(reason);

            void UpdateActions(double value)
            {
                int amount = Math.Max(1, (int)value);
                var buyPreview = _host.PreviewBuy(contactId, line.entryId, amount);
                var sellPreview = _host.PreviewSell(contactId, line.entryId, amount);
                ApplyPreview(buy, "BUY", buyPreview);
                ApplyPreview(sell, "SELL", sellPreview);
                reason.Text =
                    $"BUY: {buyPreview.Message}  SELL: {sellPreview.Message}";
            }

            quantity.ValueChanged += UpdateActions;
            buy.Pressed += () =>
            {
                _pendingFocusKey = FocusKey(BlackMarketSettlementService.BuyAction, contactId, line.entryId);
                _host?.Buy(contactId, line.entryId, Math.Max(1, (int)quantity.Value));
            };
            sell.Pressed += () =>
            {
                _pendingFocusKey = FocusKey(BlackMarketSettlementService.SellAction, contactId, line.entryId);
                _host?.Sell(contactId, line.entryId, Math.Max(1, (int)quantity.Value));
            };

            _firstFocusTarget ??= quantity;
            _focusTargets[FocusKey(BlackMarketSettlementService.BuyAction, contactId, line.entryId)] = quantity;
            _focusTargets[FocusKey(BlackMarketSettlementService.SellAction, contactId, line.entryId)] = quantity;
            UpdateActions(quantity.Value);
            _stockRows.AddChild(row);
        }

        private void AddLoanRow(string contactId)
        {
            var profile = _host!.System.Catalog.FindSyndicate(contactId);
            if (profile == null) return;

            var row = new VBoxContainer();
            row.AddThemeConstantOverride("separation", 4);
            row.AddChild(AshfallUiHelpers.MakeBody("Underworld credit"));

            var controls = new HBoxContainer();
            controls.AddThemeConstantOverride("separation", 8);
            var amount = new SpinBox
            {
                MinValue = 1,
                MaxValue = Math.Max(1, profile.credit_limit_units),
                Step = 1,
                Value = Math.Min(100, Math.Max(1, profile.credit_limit_units)),
                AllowGreater = false,
                AllowLesser = false,
                CustomMinimumSize = new Vector2(110, 0)
            };
            var duration = new SpinBox
            {
                MinValue = 1,
                MaxValue = 30,
                Step = 1,
                Value = 7,
                AllowGreater = false,
                AllowLesser = false,
                CustomMinimumSize = new Vector2(90, 0),
                TooltipText = "Loan duration in days."
            };
            var takeLoan = new Button { Text = "TAKE LOAN" };
            controls.AddChild(amount);
            controls.AddChild(duration);
            controls.AddChild(takeLoan);
            row.AddChild(controls);

            var reason = AshfallUiHelpers.MakeBody(string.Empty);
            reason.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            reason.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            row.AddChild(reason);

            void UpdateLoan(double _)
            {
                var preview = _host.PreviewLoan(contactId,
                    Math.Max(1L, (long)amount.Value), Math.Max(1, (int)duration.Value));
                ApplyPreview(takeLoan, "TAKE LOAN", preview);
                reason.Text = preview.Message;
            }

            amount.ValueChanged += UpdateLoan;
            duration.ValueChanged += UpdateLoan;
            takeLoan.Pressed += () =>
            {
                _pendingFocusKey = FocusKey(BlackMarketSettlementService.LoanAction, contactId, string.Empty);
                _host?.TakeLoan(contactId, Math.Max(1L, (long)amount.Value),
                    Math.Max(1, (int)duration.Value));
            };
            _firstFocusTarget ??= amount;
            _focusTargets[FocusKey(BlackMarketSettlementService.LoanAction, contactId, string.Empty)] = amount;
            UpdateLoan(0);
            _stockRows.AddChild(row);
        }

        private void BuildDebtRows()
        {
            var debts = _host!.System.State.debts.Where(d => d != null).ToList();
            if (debts.Count == 0)
            {
                _debtRows.AddChild(AshfallUiHelpers.MakeBody(
                    "No ledger entries. Loans build trust and bind tight."));
                return;
            }

            foreach (var debt in debts)
            {
                if (debt.status != UnderworldDebtRecord.StatusActive)
                {
                    string text = debt.status == UnderworldDebtRecord.StatusDefaulted
                        ? $"DEFAULTED: {debt.debtId} · {debt.syndicateId} · due day {debt.dueDay}. A bounty is out."
                        : $"REPAID: {debt.debtId} · {debt.syndicateId}";
                    _debtRows.AddChild(AshfallUiHelpers.MakeBody(text));
                    continue;
                }

                float outstanding = _host.System.OutstandingOnDebt(debt);
                var row = new VBoxContainer();
                row.AddThemeConstantOverride("separation", 4);
                row.AddChild(AshfallUiHelpers.MakeBody(
                    $"ACTIVE: {debt.debtId} · {debt.syndicateId} · {outstanding:0} owed · DUE DAY {debt.dueDay}"));

                var controls = new HBoxContainer();
                controls.AddThemeConstantOverride("separation", 8);
                var amount = new SpinBox
                {
                    MinValue = 1,
                    MaxValue = Math.Max(1, Math.Ceiling(outstanding)),
                    Step = 1,
                    Value = 1,
                    AllowGreater = false,
                    AllowLesser = false,
                    CustomMinimumSize = new Vector2(110, 0)
                };
                var repay = new Button { Text = "REPAY" };
                controls.AddChild(amount);
                controls.AddChild(repay);
                row.AddChild(controls);

                var reason = AshfallUiHelpers.MakeBody(string.Empty);
                reason.AutowrapMode = TextServer.AutowrapMode.WordSmart;
                reason.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
                row.AddChild(reason);

                void UpdateRepay(double value)
                {
                    var preview = _host.PreviewRepay(debt.debtId, Math.Max(1L, (long)value));
                    ApplyPreview(repay, "REPAY", preview);
                    reason.Text = preview.Message;
                }

                amount.ValueChanged += UpdateRepay;
                repay.Pressed += () =>
                {
                    _pendingFocusKey = FocusKey(BlackMarketSettlementService.RepayAction,
                        debt.syndicateId, debt.debtId);
                    _host?.Repay(debt.debtId, Math.Max(1L, (long)amount.Value));
                };
                _firstFocusTarget ??= amount;
                _focusTargets[FocusKey(BlackMarketSettlementService.RepayAction,
                    debt.syndicateId, debt.debtId)] = amount;
                UpdateRepay(amount.Value);
                _debtRows.AddChild(row);
            }
        }

        private void OnActionCompleted(BlackMarketActionResult result)
        {
            _lastOutcome = result.Success
                ? $"COMPLETED: {result.Message}"
                : $"UNAVAILABLE: {result.Message}";
            if (string.IsNullOrEmpty(_pendingFocusKey))
            {
                string owner = !string.IsNullOrEmpty(result.SyndicateId)
                    ? result.SyndicateId
                    : _host?.System.State.debts.FirstOrDefault(d => d?.debtId == result.DebtId)?.syndicateId ?? string.Empty;
                _pendingFocusKey = FocusKey(result.ActionId, owner,
                    !string.IsNullOrEmpty(result.EntryId) ? result.EntryId : result.DebtId);
            }
            RefreshView();
        }

        private static void ApplyPreview(Button button, string label, BlackMarketActionPreview preview)
        {
            button.Disabled = !preview.IsAvailable;
            button.TooltipText = preview.Message;
            button.Text = preview.IsAvailable && preview.SettlementUnits > 0
                ? $"{label} · {preview.SettlementUnits}"
                : label;
        }

        private static string FocusKey(string actionId, string ownerId, string subjectId) =>
            $"{actionId}:{ownerId}:{subjectId}";

        private static void ClearChildren(Node parent)
        {
            foreach (Node child in parent.GetChildren())
            {
                parent.RemoveChild(child);
                child.QueueFree();
            }
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("The Quiet Counter // Underworld Trade",
                minWidth: 920, minHeight: 600);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("contacts", "Contacts", "0", AshfallMetricCard.Criticality.Normal, minWidth: 110);
            _statusRail.AddCard("funds", "Value", "0", AshfallMetricCard.Criticality.Normal, minWidth: 110);
            _statusRail.AddCard("debts", "Debts", "none", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("heat", "Heat", "low", AshfallMetricCard.Criticality.Normal, minWidth: 100);
            _statusRail.AddCard("stock", "Stock", "0", AshfallMetricCard.Criticality.Normal, minWidth: 100);

            var scroll = new ScrollContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            _contentStack = new VBoxContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            _contentStack.AddThemeConstantOverride("separation", 12);
            scroll.AddChild(_contentStack);

            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("SYNDICATE STOCK"));
            _stockRows = new VBoxContainer();
            _stockRows.AddThemeConstantOverride("separation", 10);
            _contentStack.AddChild(_stockRows);

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("LEDGER"));
            _debtRows = new VBoxContainer();
            _debtRows.AddThemeConstantOverride("separation", 8);
            _contentStack.AddChild(_debtRows);

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _outcomeText = AshfallUiHelpers.MakeBody(string.Empty);
            _outcomeText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _outcomeText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            _contentStack.AddChild(_outcomeText);

            var note = AshfallUiHelpers.MakeBody(
                "Prices follow the legitimate market with an underworld premium. Missed due dates cost trust, raise heat, and may place a bounty.");
            note.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _contentStack.AddChild(note);

            _shell.SetContent(scroll);
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
