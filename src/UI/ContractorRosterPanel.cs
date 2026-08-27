using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Contractor Guild & Mercenary Roster Management Interface.
    /// Manages specialist hiring, daily wage payroll, contracts, and expedition mercenaries.
    /// </summary>
    public partial class ContractorRosterPanel : Control
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contractorList = null!;
        private VBoxContainer _contractDesk = null!;
        private VBoxContainer _payrollLogContainer = null!;
        private Label _eventLogLabel = null!;

        private ContractorRosterHostSession? _host;
        private string? _selectedOfferId;

        public bool IsBound => _host != null;

        public void Bind(ContractorRosterHostSession session)
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
            }
            _host = session;
            if (_host != null)
            {
                _host.StateChanged += RefreshView;
            }
            RefreshView();
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.04f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            _shell = new AshfallDashboardShell("SYS: CONTRACTOR GUILD & MERCENARY ROSTER // CONTRACT MATRIX", minWidth: 1040, minHeight: 680);
            center.AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("contractors", "ACTIVE GUILD", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("offers", "OFFERS PENDING", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("payroll", "DAILY PAYROLL", "0 RATIONS", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("morale", "GUILD LOYALTY", "HIGH", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("status", "CONTRACT DESK", "READY", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _shell.AttachHeaderCloseButton("CLOSE [Esc]", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            // 3-Column Layout
            var gridRow = new HBoxContainer();
            gridRow.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            gridRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            gridRow.SizeFlagsVertical = SizeFlags.ExpandFill;

            // Column 1: Active Contractors & Offers List
            var leftPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            leftPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftPanel.SizeFlagsStretchRatio = 0.95f;
            var leftMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            leftPanel.AddChild(leftMargin);
            var leftVbox = new VBoxContainer();
            leftVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            leftMargin.AddChild(leftVbox);
            leftVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("CONTRACTOR ROSTER & OFFERS"));
            var leftScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _contractorList = new VBoxContainer();
            _contractorList.AddThemeConstantOverride("separation", DesignTheme.SpacingXs);
            _contractorList.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            leftScroll.AddChild(_contractorList);
            leftVbox.AddChild(leftScroll);
            gridRow.AddChild(leftPanel);

            // Column 2: Contract Terms Inspector & Hiring
            var centerPanel = AshfallUiHelpers.MakePanel(minWidth: 380);
            centerPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerPanel.SizeFlagsStretchRatio = 1.2f;
            var centerMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            centerPanel.AddChild(centerMargin);
            var centerVbox = new VBoxContainer();
            centerVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            centerMargin.AddChild(centerVbox);
            centerVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("CONTRACT TERMS & DISPATCH"));
            var centerScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _contractDesk = new VBoxContainer();
            _contractDesk.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _contractDesk.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            centerScroll.AddChild(_contractDesk);
            centerVbox.AddChild(centerScroll);
            gridRow.AddChild(centerPanel);

            // Column 3: Payroll Log & History
            var rightPanel = AshfallUiHelpers.MakePanel(minWidth: 310);
            rightPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightPanel.SizeFlagsStretchRatio = 0.95f;
            var rightMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            rightPanel.AddChild(rightMargin);
            var rightVbox = new VBoxContainer();
            rightVbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            rightMargin.AddChild(rightVbox);
            rightVbox.AddChild(AshfallUiHelpers.MakeSectionHeader("PAYROLL & LOGS"));
            var rightScroll = new ScrollContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _payrollLogContainer = new VBoxContainer();
            _payrollLogContainer.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            _payrollLogContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            rightScroll.AddChild(_payrollLogContainer);
            rightVbox.AddChild(rightScroll);

            rightVbox.AddChild(AshfallUiHelpers.MakeSeparator());
            _eventLogLabel = AshfallUiHelpers.MakeMetadata("No recent contractor events.");
            _eventLogLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            rightVbox.AddChild(_eventLogLabel);

            gridRow.AddChild(rightPanel);

            _shell.SetContent(gridRow);
            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
            QueueRedraw();
        }

        public void RefreshView()
        {
            if (_contractorList == null || _contractDesk == null || _payrollLogContainer == null) return;

            AshfallUiHelpers.EmptyChildren(_contractorList);
            AshfallUiHelpers.EmptyChildren(_contractDesk);
            AshfallUiHelpers.EmptyChildren(_payrollLogContainer);

            if (_host == null || _statusRail == null)
            {
                _contractorList.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("No contractor roster session bound", "offline"));
                _contractDesk.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("Contract desk offline", "offline"));
                _payrollLogContainer.AddChild(AshfallUiHelpers.MakeEmptyStateLabel("Payroll log unavailable", "offline"));
                return;
            }

            var s = _host.System.State;
            int activeContractors = s.contractors.Count(c => c.status == ContractStatus.Active);
            int pendingOffers = s.activeOffers.Count(o => o.status == ContractStatus.Available);
            int dailyPayroll = s.activeOffers.Where(o => o.status == ContractStatus.Active).Sum(o => o.dailyHazardPay);

            _statusRail.Set("contractors", activeContractors.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("offers", pendingOffers.ToString(), pendingOffers > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("payroll", $"{dailyPayroll} RATIONS/DAY", dailyPayroll > 5 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("morale", "NOMINAL", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("status", activeContractors > 0 ? "EMPLOYING" : "STANDBY", AshfallMetricCard.Criticality.Normal);

            if (!string.IsNullOrEmpty(_host.LastEvent))
            {
                _eventLogLabel.Text = _host.LastEvent;
            }

            // Populate Contractors and Offers
            if (s.contractors.Count == 0 && s.activeOffers.Count == 0)
            {
                _contractorList.AddChild(AshfallUiHelpers.MakeMetadata("No active contractors or pending offers."));
                var btnGenOffer = AshfallUiHelpers.MakeButton("POST RECRUITMENT NOTICE", () =>
                {
                    _host.GenerateOffer("mercenary_kane", "Point Scout & Sentry", new List<string> { "Combat", "Stealth" }, 5, 2, 7);
                    _host.GenerateOffer("scavenger_tora", "Tunnel Breacher", new List<string> { "Engineering" }, 4, 1, 5);
                    RefreshView();
                });
                _contractorList.AddChild(btnGenOffer);
            }
            else
            {
                if (_selectedOfferId == null && s.activeOffers.Count > 0)
                {
                    _selectedOfferId = s.activeOffers[0].offerId;
                }

                // Active Contractors
                foreach (var c in s.contractors)
                {
                    var card = AshfallUiHelpers.MakePanel();
                    var cardMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingXs);
                    card.AddChild(cardMargin);
                    var cardVbox = new VBoxContainer();
                    cardVbox.AddThemeConstantOverride("separation", 3);
                    cardMargin.AddChild(cardVbox);

                    var headerRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                    headerRow.AddChild(AshfallUiHelpers.MakeBadgeIcon("badge_corneal_burn", 18));
                    var nameLbl = AshfallUiHelpers.MakeBody($"{c.displayName.ToUpperInvariant()} // {c.role}");
                    nameLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    headerRow.AddChild(nameLbl);
                    cardVbox.AddChild(headerRow);

                    var payLbl = AshfallUiHelpers.MakeMono($"LOYALTY: {c.loyalty:F0}% | TRUST: {c.trust:F0}% [{c.status}]");
                    payLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Lethe));
                    cardVbox.AddChild(payLbl);

                    var btnDismiss = AshfallUiHelpers.MakeButton($"DISMISS // {c.contractorId}", () =>
                    {
                        _host.Dismiss(c.contractorId);
                        RefreshView();
                    });
                    btnDismiss.CustomMinimumSize = new Vector2(0, 24);
                    cardVbox.AddChild(btnDismiss);

                    _contractorList.AddChild(card);
                }

                // Pending Offers
                foreach (var offer in s.activeOffers)
                {
                    var card = AshfallUiHelpers.MakePanel();
                    var cardMargin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingXs);
                    card.AddChild(cardMargin);
                    var cardVbox = new VBoxContainer();
                    cardVbox.AddThemeConstantOverride("separation", 3);
                    cardMargin.AddChild(cardVbox);

                    var headerRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                    headerRow.AddChild(AshfallUiHelpers.MakeBadgeIcon("badge_crossing_terms", 18));
                    var nameLbl = AshfallUiHelpers.MakeBody($"OFFER: {offer.candidateId.ToUpperInvariant()}");
                    nameLbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    headerRow.AddChild(nameLbl);
                    cardVbox.AddChild(headerRow);

                    var payLbl = AshfallUiHelpers.MakeMono($"FEE: {offer.initialFee} Rations upfront, {offer.dailyHazardPay}/day ({offer.termDays}d)");
                    payLbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
                    cardVbox.AddChild(payLbl);

                    var btnSelect = AshfallUiHelpers.MakeButton($"REVIEW OFFER // {offer.offerId}", () =>
                    {
                        _selectedOfferId = offer.offerId;
                        RefreshView();
                    });
                    btnSelect.CustomMinimumSize = new Vector2(0, 24);
                    cardVbox.AddChild(btnSelect);

                    _contractorList.AddChild(card);
                }
            }

            // Selected Offer / Contract Desk Inspector
            var curOffer = s.activeOffers.FirstOrDefault(o => o.offerId == _selectedOfferId);
            if (curOffer != null)
            {
                _contractDesk.AddChild(AshfallUiHelpers.MakeSectionHeader($"OFFER: {curOffer.candidateId.ToUpperInvariant()}"));
                _contractDesk.AddChild(AshfallUiHelpers.MakeDataRow("Designated Role", curOffer.role, AshfallUiHelpers.ToColor(DesignTheme.Warm)));
                _contractDesk.AddChild(AshfallUiHelpers.MakeDataRow("Contract Term", $"{curOffer.termDays} Simulation Days", AshfallUiHelpers.ToColor(DesignTheme.Pale)));
                _contractDesk.AddChild(AshfallUiHelpers.MakeDataRow("Signing Retainer", $"{curOffer.initialFee} Rations (One-time)", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
                _contractDesk.AddChild(AshfallUiHelpers.MakeDataRow("Daily Wage Rate", $"{curOffer.dailyHazardPay} Rations/Day", AshfallUiHelpers.ToColor(DesignTheme.Lethe)));

                _contractDesk.AddChild(AshfallUiHelpers.MakeSeparator());
                _contractDesk.AddChild(AshfallUiHelpers.MakeSubsectionHeader("EXECUTE CONTRACT"));

                var btnAccept = AshfallUiHelpers.MakeButton("SIGN CONTRACT & DISPATCH RETAINER", () =>
                {
                    _host.AcceptOffer(curOffer.offerId);
                    RefreshView();
                });
                _contractDesk.AddChild(btnAccept);
            }
            else
            {
                _contractDesk.AddChild(AshfallUiHelpers.MakeMetadata("Select a candidate offer to review contract terms and wages."));
            }

            // History / Log
            _payrollLogContainer.AddChild(AshfallUiHelpers.MakeSectionHeader("ACTIVE SERVICE ROSTER"));
            if (s.contractors.Count == 0)
            {
                _payrollLogContainer.AddChild(AshfallUiHelpers.MakeMetadata("No outside contractors currently under payroll."));
            }
            else
            {
                foreach (var c in s.contractors)
                {
                    _payrollLogContainer.AddChild(AshfallUiHelpers.MakeMono($"[DEPLOYED] {c.displayName} ({c.role})"));
                }
            }
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                OnClose?.Invoke();
                Visible = false;
                GetViewport().SetInputAsHandled();
            }
        }

        public override void _ExitTree()
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
            }
            base._ExitTree();
        }
    }
}
