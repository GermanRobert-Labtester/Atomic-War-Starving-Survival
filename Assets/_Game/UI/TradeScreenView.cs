using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Pure UI Toolkit view for the trade/barter modal. Queries UXML elements
    /// from DiegeticHud.uxml by name, paints barter lines from TradeScreenUI
    /// view-model data, and raises events for button interactions.
    /// No game logic — presentation only.
    /// </summary>
    public class TradeScreenView
    {
        // UXML element names (must match DiegeticHud.uxml).
        public const string BackdropName = "trade-backdrop";
        public const string PanelName = "trade-panel";
        public const string FactionNameLabel = "trade-faction-name";
        public const string FactionStripLabel = "trade-faction-strip";
        public const string StanceBadgeLabel = "trade-stance-badge";
        public const string PlayerLinesName = "trade-player-lines";
        public const string PlayerEmptyName = "trade-player-empty";
        public const string PlayerTotalValueName = "trade-player-total-value";
        public const string FactionLinesName = "trade-faction-lines";
        public const string FactionEmptyName = "trade-faction-empty";
        public const string FactionTotalValueName = "trade-faction-total-value";
        public const string FairIndicatorName = "trade-fair-indicator";
        public const string BalanceLabelName = "trade-balance-label";
        public const string ParleyBtnName = "trade-parley-btn";
        public const string ClearBtnName = "trade-clear-btn";
        public const string ConfirmBtnName = "trade-confirm-btn";
        public const string CloseBtnName = "trade-close-btn";
        public const string ParleyMsgName = "trade-parley-msg";
        public const string LeaderNameLabel = "trade-leader-name";
        public const string TrustLabel = "trade-trust";
        public const string AggressionLabel = "trade-aggression";
        public const string RepelsLabel = "trade-repels";
        public const string RadioTickerName = "trade-radio-ticker";

        // USS classes.
        private const string HiddenClass = "hidden";
        private const string FairClass = "trade-fair-indicator--fair";
        private const string ShortClass = "trade-fair-indicator--short";
        private const string HostileClass = "trade-stance-badge--hostile";
        private const string DisabledClass = "trade-btn--disabled";
        private const string LineNameClass = "trade-line-name";
        private const string LineQtyClass = "trade-line-qty";
        private const string LineValueClass = "trade-line-value";
        private const string LineClass = "trade-line";

        private VisualElement _backdrop;
        private VisualElement _playerLines;
        private VisualElement _factionLines;
        private Label _playerEmpty;
        private Label _factionEmpty;
        private Label _factionName;
        private Label _factionStrip;
        private Label _stanceBadge;
        private Label _playerTotalValue;
        private Label _factionTotalValue;
        private Label _fairIndicator;
        private Label _balanceLabel;
        private Button _parleyBtn;
        private Button _clearBtn;
        private Button _confirmBtn;
        private Button _closeBtn;
        private Label _parleyMsg;
        private Label _leaderName;
        private Label _trust;
        private Label _aggression;
        private Label _repels;
        private Label _radioTicker;

        public event Action OnConfirmRequested;
        public event Action OnClearRequested;
        public event Action OnParleyRequested;
        public event Action OnCloseRequested;

        /// <summary>Bind to an existing UXML tree (queries by name).</summary>
        public bool Bind(VisualElement root)
        {
            if (root == null) return false;
            _backdrop = root.Q<VisualElement>(BackdropName);
            if (_backdrop == null) return false;

            _factionName = root.Q<Label>(FactionNameLabel);
            _factionStrip = root.Q<Label>(FactionStripLabel);
            _stanceBadge = root.Q<Label>(StanceBadgeLabel);
            _playerLines = root.Q<VisualElement>(PlayerLinesName);
            _playerEmpty = root.Q<Label>(PlayerEmptyName);
            _playerTotalValue = root.Q<Label>(PlayerTotalValueName);
            _factionLines = root.Q<VisualElement>(FactionLinesName);
            _factionEmpty = root.Q<Label>(FactionEmptyName);
            _factionTotalValue = root.Q<Label>(FactionTotalValueName);
            _fairIndicator = root.Q<Label>(FairIndicatorName);
            _balanceLabel = root.Q<Label>(BalanceLabelName);
            _parleyBtn = root.Q<Button>(ParleyBtnName);
            _clearBtn = root.Q<Button>(ClearBtnName);
            _confirmBtn = root.Q<Button>(ConfirmBtnName);
            _closeBtn = root.Q<Button>(CloseBtnName);
            _parleyMsg = root.Q<Label>(ParleyMsgName);
            _leaderName = root.Q<Label>(LeaderNameLabel);
            _trust = root.Q<Label>(TrustLabel);
            _aggression = root.Q<Label>(AggressionLabel);
            _repels = root.Q<Label>(RepelsLabel);
            _radioTicker = root.Q<Label>(RadioTickerName);

            if (_clearBtn != null) _clearBtn.clicked += () => OnClearRequested?.Invoke();
            if (_confirmBtn != null) _confirmBtn.clicked += () => OnConfirmRequested?.Invoke();
            if (_parleyBtn != null) _parleyBtn.clicked += () => OnParleyRequested?.Invoke();
            if (_closeBtn != null) _closeBtn.clicked += () => OnCloseRequested?.Invoke();

            return true;
        }

        /// <summary>
        /// Paint the trade screen from view-model state. Called by
        /// DiegeticHudController.Paint() each frame the screen is open.
        /// </summary>
        public void Paint(
            bool isOpen,
            string factionName,
            string factionStrip,
            string stanceLabel,
            bool isHostile,
            IReadOnlyList<BarterLineData> playerOffers,
            float playerTotal,
            IReadOnlyList<BarterLineData> factionAsks,
            float factionTotal,
            bool isFair,
            bool canParley,
            string parleyMessage,
            string leaderName = null,
            string trustText = null,
            string aggressionText = null,
            string repelsText = null,
            string radioTickerText = null)
        {
            if (_backdrop == null) return;
            SetVisible(_backdrop, isOpen);
            if (!isOpen) return;

            if (_factionName != null) _factionName.text = factionName ?? string.Empty;
            if (_factionStrip != null) _factionStrip.text = factionStrip ?? string.Empty;

            if (_leaderName != null)
                _leaderName.text = leaderName ?? string.Empty;

            if (_stanceBadge != null)
            {
                _stanceBadge.text = stanceLabel ?? string.Empty;
                SetClass(_stanceBadge, HostileClass, isHostile);
            }

            if (_trust != null) _trust.text = trustText ?? string.Empty;
            if (_aggression != null) _aggression.text = aggressionText ?? string.Empty;
            if (_repels != null) _repels.text = repelsText ?? string.Empty;

            bool hasTicker = !string.IsNullOrEmpty(radioTickerText);
            if (_radioTicker != null)
            {
                SetVisible(_radioTicker, hasTicker);
                if (hasTicker) _radioTicker.text = radioTickerText;
            }

            PaintBarterLines(_playerLines, _playerEmpty, playerOffers);
            PaintBarterLines(_factionLines, _factionEmpty, factionAsks);

            if (_playerTotalValue != null) _playerTotalValue.text = FormatValue(playerTotal);
            if (_factionTotalValue != null) _factionTotalValue.text = FormatValue(factionTotal);

            if (_fairIndicator != null)
            {
                if (isFair)
                {
                    _fairIndicator.text = "DEAL IS FAIR";
                    SetClass(_fairIndicator, FairClass, true);
                    SetClass(_fairIndicator, ShortClass, false);
                }
                else
                {
                    _fairIndicator.text = "DEAL SHORT";
                    SetClass(_fairIndicator, FairClass, false);
                    SetClass(_fairIndicator, ShortClass, true);
                }
            }

            if (_balanceLabel != null)
                _balanceLabel.text = $"{FormatValue(playerTotal)} vs {FormatValue(factionTotal)}";

            if (_parleyBtn != null)
                SetClass(_parleyBtn, DisabledClass, !canParley);

            bool hasMsg = !string.IsNullOrEmpty(parleyMessage);
            if (_parleyMsg != null)
            {
                SetVisible(_parleyMsg, hasMsg);
                if (hasMsg) _parleyMsg.text = parleyMessage;
            }
        }

        private void PaintBarterLines(
            VisualElement container,
            Label emptyLabel,
            IReadOnlyList<BarterLineData> lines)
        {
            if (container == null) return;

            container.Clear();

            bool hasLines = lines != null && lines.Count > 0;
            if (emptyLabel != null) SetVisible(emptyLabel, !hasLines);

            if (!hasLines) return;

            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                var row = new VisualElement();
                row.AddToClassList(LineClass);

                var nameLbl = new Label { text = line.ItemName ?? string.Empty };
                nameLbl.AddToClassList(LineNameClass);
                row.Add(nameLbl);

                var qtyLbl = new Label { text = line.Quantity.ToString() };
                qtyLbl.AddToClassList(LineQtyClass);
                row.Add(qtyLbl);

                var valLbl = new Label { text = FormatValue(line.TotalValue) };
                valLbl.AddToClassList(LineValueClass);
                row.Add(valLbl);

                container.Add(row);
            }
        }

        private static string FormatValue(float value)
        {
            return value.ToString("0.#");
        }

        private static void SetVisible(VisualElement el, bool visible)
        {
            if (el == null) return;
            if (visible) el.RemoveFromClassList(HiddenClass);
            else el.AddToClassList(HiddenClass);
        }

        private static void SetClass(VisualElement el, string className, bool active)
        {
            if (el == null) return;
            if (active) el.AddToClassList(className);
            else el.RemoveFromClassList(className);
        }
    }

    /// <summary>
    /// Data for a single barter line in the trade screen view.
    /// The view-model (TradeScreenUI) converts its internal BarterLine list
    /// into these before passing to the view.
    /// </summary>
    public struct BarterLineData
    {
        public string ItemName;
        public string ItemId;
        public int Quantity;
        public float UnitValue;
        public float TotalValue;

        public BarterLineData(string itemName, string itemId, int quantity, float unitValue)
        {
            ItemName = itemName;
            ItemId = itemId;
            Quantity = quantity;
            UnitValue = unitValue;
            TotalValue = unitValue * quantity;
        }
    }
}
