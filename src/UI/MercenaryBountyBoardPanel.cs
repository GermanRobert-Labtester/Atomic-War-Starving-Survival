// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core.Economy;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plans 186–189: mercenary bounty board workflow.
    /// Presentation-only — accept/claim commands are emitted via
    /// <see cref="OnActionRequested"/> and resolved by the host through the
    /// bound <see cref="MercenarySystem"/>, the canonical inventory (posting
    /// costs, proof items) and the journal. Urgency and faction consequences
    /// are labeled in text, never by color alone.
    /// </summary>
    public partial class MercenaryBountyBoardPanel : Control, IBindablePanel
    {
        public event Action? OnClose;
        public event Action<string, string>? OnActionRequested;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private MercenarySystem? _system;
        private ItemList _contractList = null!;
        private VBoxContainer _detail = null!;
        private int _selectedContractIndex = -1;
        private string _feedbackText = string.Empty;
        private bool _feedbackIsFailure;
        private int _displayDay;

        public bool IsBound => _system != null;

        public void Bind(MercenarySystem system) { _system = system; _feedbackText = string.Empty; RefreshView(); }
        public void Unbind() { _system = null; }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            _shell = new AshfallDashboardShell("WARLORD CONTRACTS // MERCENARY BOUNTY BOARD", minWidth: 1000, minHeight: 650);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("open", "Open Offers", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
            _statusRail.AddCard("accepted", "Accepted", "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
            _statusRail.AddCard("claimable", "Ready to Claim", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("posted", "Posted by Us", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contractList = new ItemList
            {
                CustomMinimumSize = new Vector2(320, 0),
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.Fill
            };
            _contractList.ItemSelected += index => { _selectedContractIndex = (int)index; RefreshView(); };

            _detail = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);

            var bodyRow = new HBoxContainer();
            bodyRow.AddChild(_contractList);

            var detailScroll = new ScrollContainer
            {
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            detailScroll.AddChild(_detail);
            bodyRow.AddChild(detailScroll);

            _shell.SetContent(bodyRow);
            _shell.AttachHeaderCloseButton("CLOSE", () => OnClose?.Invoke());
            AddChild(_shell);
            Visible = false;
        }

        public void Open() { Visible = true; RefreshView(); }
        public void Close()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(this))
                Visible = false;
            OnClose?.Invoke();
        }

        /// <summary>Last feedback line rendered by the panel (test/diagnostic surface).</summary>
        public string LastFeedback { get; private set; } = string.Empty;

        /// <summary>Campaign clock for expiry urgency display (host-provided).</summary>
        public void SetDisplayClock(int day) { _displayDay = day; }

        public void ShowFeedback(string message, bool isFailure)
        {
            _feedbackText = message;
            _feedbackIsFailure = isFailure;
            LastFeedback = message;
            RefreshView();
        }

        private List<BountyContract> SortedContracts()
        {
            if (_system == null) return new List<BountyContract>();
            // Sort: accepted first, then expiring soonest, then the rest.
            return _system.State.contracts
                .OrderBy(c => c.status == BountyContractStatus.Accepted ? 0 :
                              c.status == BountyContractStatus.Open ? 1 : 2)
                .ThenBy(c => c.expiryDay)
                .ThenBy(c => c.contractId, StringComparer.Ordinal)
                .ToList();
        }

        public void RefreshView()
        {
            if (_system == null || _detail == null || _contractList == null || _statusRail == null) return;
            AshfallUiHelpers.EmptyChildren(_detail);

            var contracts = SortedContracts();
            _selectedContractIndex = Math.Clamp(_selectedContractIndex, -1, Math.Max(0, contracts.Count - 1));

            int open = contracts.Count(c => c.status == BountyContractStatus.Open);
            int accepted = contracts.Count(c => c.status == BountyContractStatus.Accepted);
            int claimable = contracts.Count(c => c.status == BountyContractStatus.Completed && !c.rewardClaimed);
            int claimed = contracts.Count(c => c.rewardClaimed);

            _statusRail.Set("open", open.ToString(), open > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Caution);
            _statusRail.Set("accepted", accepted.ToString(),
                accepted > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("claimable", claimable.ToString(),
                claimable > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("posted", claimed.ToString(), AshfallMetricCard.Criticality.Normal);

            _contractList.Clear();
            for (int i = 0; i < contracts.Count; i++)
            {
                var c = contracts[i];
                string label = $"{ContractStatusText(c)} — {c.contractType} — {c.rewardAmount:0} scrap";
                if (_displayDay >= c.expiryDay && c.status == BountyContractStatus.Open)
                    label += " [EXPIRED]";
                _contractList.AddItem(label, null, false);
                if (i == _selectedContractIndex)
                    _contractList.Select(i);
            }
            if (contracts.Count == 0)
                _contractList.AddItem("No contracts on the board", null, false);

            var selected = _selectedContractIndex >= 0 && _selectedContractIndex < contracts.Count
                ? contracts[_selectedContractIndex] : null;

            if (selected == null)
            {
                _detail.AddChild(AshfallUiHelpers.MakeEmptyState(
                    "The board is empty. Contracts appear as factions post them; posting our own costs scrap up front.",
                    title: "NO CONTRACTS"));
                return;
            }

            _detail.AddChild(AshfallUiHelpers.MakeSectionHeader($"{selected.contractType.ToUpperInvariant()} CONTRACT"));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Issued by", FactionDisplayName(selected.issuerFactionId), AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Target", ItemDisplay.Prettify(selected.targetId), AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Target faction", FactionDisplayName(selected.targetFactionId),
                IsHostileFaction(selected.targetFactionId) ? AshfallUiHelpers.ColorDim : AshfallUiHelpers.ColorWarning));
            if (!IsHostileFaction(selected.targetFactionId))
                _detail.AddChild(AshfallUiHelpers.MakeWarning(
                    "Target belongs to a faction we are not at war with — acting on this contract will cost standing."));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Payout", $"{selected.rewardAmount:0} scrap", AshfallUiHelpers.ColorSuccess));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Status", ContractStatusText(selected), AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Proof required", ItemDisplay.Name(selected.requiredProofItemId), AshfallUiHelpers.ColorDim));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Expires", $"day {selected.expiryDay}",
                _displayDay >= selected.expiryDay - 2 ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorDim));

            if (!string.IsNullOrEmpty(_feedbackText))
            {
                _detail.AddChild(AshfallUiHelpers.MakeSeparator());
                _detail.AddChild(_feedbackIsFailure
                    ? AshfallUiHelpers.MakeWarning(_feedbackText)
                    : AshfallUiHelpers.MakeSuccess(_feedbackText));
            }

            // ── Actions ──
            _detail.AddChild(AshfallUiHelpers.MakeSeparator());
            _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("ACTIONS"));
            var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);

            switch (selected.status)
            {
                case BountyContractStatus.Open:
                    var acceptBtn = AshfallUiHelpers.MakeButton("ACCEPT CONTRACT", () =>
                        OnActionRequested?.Invoke("accept", selected.contractId));
                    acceptBtn.TooltipText = "Takes the contract. The tribunal of factions will remember where we stood.";
                    row.AddChild(acceptBtn);
                    break;
                case BountyContractStatus.Completed when !selected.rewardClaimed:
                    var claimBtn = AshfallUiHelpers.MakeButton("CLAIM REWARD", () =>
                        OnActionRequested?.Invoke("claim", selected.contractId));
                    claimBtn.TooltipText = "Proof verified — collect the payout at the board.";
                    row.AddChild(claimBtn);
                    break;
                default:
                    row.AddChild(AshfallUiHelpers.MakeMetadata(
                        selected.rewardClaimed || selected.status == BountyContractStatus.Claimed ? "Reward already collected."
                        : selected.status == BountyContractStatus.Failed ? "This contract failed. The board remembers."
                        : "Nothing to do for this contract right now."));
                    break;
            }
            _detail.AddChild(row);
        }

        private static string ContractStatusText(BountyContract c) => c.status switch
        {
            BountyContractStatus.Open => "OPEN OFFER",
            BountyContractStatus.Accepted => "ACCEPTED — IN THE FIELD",
            BountyContractStatus.Completed => c.rewardClaimed ? "CLAIMED" : "COMPLETED — REWARD READY",
            BountyContractStatus.Claimed => "CLAIMED",
            BountyContractStatus.Failed => "FAILED",
            _ => c.status.ToString().ToUpperInvariant()
        };

        private static bool IsHostileFaction(string factionId) =>
            string.Equals(factionId, "faction_hostile_raiders", StringComparison.OrdinalIgnoreCase)
            || factionId.Contains("raider", StringComparison.OrdinalIgnoreCase)
            || factionId.Contains("hostile", StringComparison.OrdinalIgnoreCase);

        private static string FactionDisplayName(string factionId) =>
            string.IsNullOrEmpty(factionId) ? "unknown" : ItemDisplay.Prettify(factionId.Replace("faction_", ""));

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (AshfallInputActions.IsCloseOrCancel(@event))
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
