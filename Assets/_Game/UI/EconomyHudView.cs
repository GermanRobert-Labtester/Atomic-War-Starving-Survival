using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Pure UI Toolkit view for the economy HUD: persistent mini-strip
    /// (day/supply/price) and toggleable detail panel (full goods catalog).
    /// Queries UXML elements from DiegeticHud.uxml by name. No game logic.
    /// </summary>
    public class EconomyHudView
    {
        // UXML element names (must match DiegeticHud.uxml).
        public const string StripName = "economy-strip";
        public const string StripDayName = "economy-strip-day";
        public const string StripSupplyName = "economy-strip-supply";
        public const string StripPriceName = "economy-strip-price";
        public const string PanelName = "economy-panel";
        public const string PanelSummaryName = "economy-panel-summary";
        public const string GoodsListName = "economy-goods-list";
        public const string EmptyLabelName = "economy-empty";

        // USS classes.
        private const string HiddenClass = "hidden";
        private const string SupplyNormalClass = "economy-strip-supply--normal";
        private const string SupplyShortClass = "economy-strip-supply--short";
        private const string DemandNormalClass = "economy-good-demand--normal";
        private const string DemandElevatedClass = "economy-good-demand--elevated";
        private const string DemandHighClass = "economy-good-demand--high";
        private const string DemandCriticalClass = "economy-good-demand--critical";

        private VisualElement _strip;
        private Label _stripDay;
        private Label _stripSupply;
        private Label _stripPrice;
        private VisualElement _panel;
        private Label _panelSummary;
        private VisualElement _goodsList;
        private Label _emptyLabel;

        /// <summary>Bind to an existing UXML tree (queries by name).</summary>
        public bool Bind(VisualElement root)
        {
            if (root == null) return false;
            _strip = root.Q<VisualElement>(StripName);
            if (_strip == null) return false;

            _stripDay = root.Q<Label>(StripDayName);
            _stripSupply = root.Q<Label>(StripSupplyName);
            _stripPrice = root.Q<Label>(StripPriceName);
            _panel = root.Q<VisualElement>(PanelName);
            _panelSummary = root.Q<Label>(PanelSummaryName);
            _goodsList = root.Q<VisualElement>(GoodsListName);
            _emptyLabel = root.Q<Label>(EmptyLabelName);

            return true;
        }

        /// <summary>
        /// Paint the persistent mini-strip. Always visible when bound.
        /// </summary>
        public void PaintStrip(int day, bool suppliesShort, string anchorPrice)
        {
            if (_stripDay != null) _stripDay.text = $"DAY {day}";

            if (_stripSupply != null)
            {
                _stripSupply.text = suppliesShort ? "SHORT" : "NOMINAL";
                SetClass(_stripSupply, SupplyShortClass, suppliesShort);
                SetClass(_stripSupply, SupplyNormalClass, !suppliesShort);
            }

            if (_stripPrice != null)
                _stripPrice.text = anchorPrice ?? string.Empty;
        }

        /// <summary>
        /// Paint the toggleable detail panel with the full goods catalog.
        /// </summary>
        public void PaintPanel(bool isOpen, string summary, IReadOnlyList<GoodRowData> goods)
        {
            if (_panel == null) return;
            SetVisible(_panel, isOpen);
            if (!isOpen) return;

            if (_panelSummary != null)
                _panelSummary.text = summary ?? string.Empty;

            bool hasGoods = goods != null && goods.Count > 0;
            if (_emptyLabel != null) SetVisible(_emptyLabel, !hasGoods);

            if (_goodsList != null)
            {
                _goodsList.Clear();
                if (hasGoods)
                {
                    for (int i = 0; i < goods.Count; i++)
                        _goodsList.Add(BuildGoodRow(goods[i]));
                }
            }
        }

        private VisualElement BuildGoodRow(GoodRowData good)
        {
            var row = new VisualElement();
            row.AddToClassList("economy-good-row");

            var name = new Label { text = good.DisplayName ?? string.Empty };
            name.AddToClassList("economy-good-name");
            row.Add(name);

            var category = new Label { text = good.Category ?? string.Empty };
            category.AddToClassList("economy-good-category");
            row.Add(category);

            var price = new Label { text = good.Price.ToString("0.#") };
            price.AddToClassList("economy-good-price");
            row.Add(price);

            var demand = new Label { text = good.DemandMultiplier.ToString("0.00") };
            demand.AddToClassList("economy-good-demand");
            ApplyDemandClass(demand, good.DemandMultiplier);
            row.Add(demand);

            return row;
        }

        private static void ApplyDemandClass(Label el, float demand)
        {
            if (demand >= 2.5f)
                el.AddToClassList(DemandCriticalClass);
            else if (demand >= 1.75f)
                el.AddToClassList(DemandHighClass);
            else if (demand >= 1.25f)
                el.AddToClassList(DemandElevatedClass);
            else
                el.AddToClassList(DemandNormalClass);
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
    /// Data for a single goods row in the economy detail panel.
    /// The controller converts economy data into these.
    /// </summary>
    public struct GoodRowData
    {
        public string Id;
        public string DisplayName;
        public string Category;
        public float Price;
        public float DemandMultiplier;

        public GoodRowData(string id, string displayName, string category, float price, float demand)
        {
            Id = id;
            DisplayName = displayName;
            Category = category;
            Price = price;
            DemandMultiplier = demand;
        }
    }
}
