// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core.Survivors;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 210 — Survivor Personal Belongings &amp; Effects surface.
    /// Claims an existing shared-inventory item definition as a survivor's
    /// keepsake, grants authored keepsake templates, toggles a favorite,
    /// transfers a claim by gift, and reports theft/loss. Physical stacks and
    /// storage capacity stay with Inventory; inheritance stays with the death
    /// orchestrator.
    /// </summary>
    public partial class PersonalBelongingsPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private HBoxContainer _splitBody = null!;
        private VBoxContainer _leftColumn = null!;
        private VBoxContainer _rightColumn = null!;
        private VBoxContainer _belongingsContainer = null!;
        private OptionButton _survivorSelect = null!;
        private OptionButton _itemSelect = null!;
        private OptionButton _categorySelect = null!;
        private OptionButton _templateSelect = null!;
        private OptionButton _giftTargetSelect = null!;
        private Label _feedback = null!;

        private PersonalBelongingsHostSession? _host;
        private string _selectedSurvivorId = string.Empty;

        public bool IsBound => _host != null;

        public void Bind(PersonalBelongingsHostSession session)
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

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Personal Effects // Keepsakes, Favorites & Gifts", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("claims", "Total Claims", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("favorites", "Favorites", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("templates", "Authored Keepsakes", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 10);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _splitBody = new HBoxContainer();
            _splitBody.AddThemeConstantOverride("separation", 16);
            _splitBody.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _splitBody.SizeFlagsVertical = SizeFlags.ExpandFill;

            _leftColumn = new VBoxContainer();
            _leftColumn.AddThemeConstantOverride("separation", 8);
            _leftColumn.CustomMinimumSize = new Vector2(420, 400);
            _leftColumn.SizeFlagsHorizontal = SizeFlags.Fill;
            _leftColumn.SizeFlagsVertical = SizeFlags.ExpandFill;
            _leftColumn.AddChild(new Label { Text = "ASSIGN A KEEPSAKE" });

            _survivorSelect = new OptionButton();
            _survivorSelect.ItemSelected += _ => OnSurvivorSelected();
            _leftColumn.AddChild(LabeledRow("Survivor", _survivorSelect));

            _itemSelect = new OptionButton();
            _leftColumn.AddChild(LabeledRow("Inventory item", _itemSelect));

            _categorySelect = new OptionButton();
            foreach (var category in Enum.GetValues(typeof(BelongingCategory)))
                _categorySelect.AddItem(category.ToString());
            _leftColumn.AddChild(LabeledRow("Category", _categorySelect));

            var claimButton = new Button { Text = "CLAIM AS PERSONAL EFFECT" };
            claimButton.Pressed += OnClaimPressed;
            _leftColumn.AddChild(claimButton);

            _leftColumn.AddChild(AshfallUiHelpers.MakeSeparator());

            _leftColumn.AddChild(new Label { Text = "GRANT AN AUTHORED KEEPSAKE" });
            _templateSelect = new OptionButton();
            _leftColumn.AddChild(LabeledRow("Template", _templateSelect));
            var templateButton = new Button { Text = "GRANT TEMPLATE TO SURVIVOR" };
            templateButton.Pressed += OnGrantTemplatePressed;
            _leftColumn.AddChild(templateButton);

            _feedback = new Label { Text = string.Empty, AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _leftColumn.AddChild(_feedback);

            _splitBody.AddChild(_leftColumn);

            _rightColumn = new VBoxContainer();
            _rightColumn.AddThemeConstantOverride("separation", 8);
            _rightColumn.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _rightColumn.SizeFlagsVertical = SizeFlags.ExpandFill;
            _rightColumn.AddChild(new Label { Text = "HELD PERSONAL EFFECTS" });

            _giftTargetSelect = new OptionButton();
            _rightColumn.AddChild(LabeledRow("Gift target", _giftTargetSelect));

            var scroll = new ScrollContainer();
            scroll.SizeFlagsVertical = SizeFlags.ExpandFill;
            scroll.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _belongingsContainer = new VBoxContainer();
            _belongingsContainer.AddThemeConstantOverride("separation", 8);
            _belongingsContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(_belongingsContainer);
            _rightColumn.AddChild(scroll);

            _splitBody.AddChild(_rightColumn);

            _contentStack.AddChild(_splitBody);
            _shell.SetContent(_contentStack);

            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        private static HBoxContainer LabeledRow(string label, Control control)
        {
            var row = new HBoxContainer();
            row.AddThemeConstantOverride("separation", 8);
            row.AddChild(new Label { Text = label, CustomMinimumSize = new Vector2(120, 0) });
            control.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            row.AddChild(control);
            return row;
        }

        private void OnSurvivorSelected()
        {
            if (_survivorSelect.Selected >= 0)
                _selectedSurvivorId = _survivorSelect.GetItemMetadata(_survivorSelect.Selected).AsString();
            RefreshView();
        }

        private void OnClaimPressed()
        {
            if (_host == null || string.IsNullOrEmpty(_selectedSurvivorId)) return;
            if (_itemSelect.Selected < 0) return;
            string itemId = _itemSelect.GetItemMetadata(_itemSelect.Selected).AsString();
            var category = (BelongingCategory)Math.Max(0, _categorySelect.Selected);
            bool ok = _host.Claim(_selectedSurvivorId, itemId, category);
            _feedback.Text = ok ? $"Claimed {itemId}." : $"Could not claim {itemId} (missing, already claimed, or unknown survivor).";
            RefreshView();
        }

        private void OnGrantTemplatePressed()
        {
            if (_host == null || string.IsNullOrEmpty(_selectedSurvivorId)) return;
            if (_templateSelect.Selected < 0) return;
            string templateId = _templateSelect.GetItemMetadata(_templateSelect.Selected).AsString();
            bool ok = _host.ClaimTemplate(_selectedSurvivorId, templateId);
            _feedback.Text = ok ? $"Granted keepsake template {templateId}." : $"Could not grant {templateId}.";
            RefreshView();
        }

        private void OnFavoritePressed(PersonalBelonging belonging)
        {
            if (_host == null) return;
            _host.SetFavorite(_selectedSurvivorId, belonging.BelongingId, !belonging.IsFavorite);
            RefreshView();
        }

        private void OnGiftPressed(PersonalBelonging belonging)
        {
            if (_host == null || _giftTargetSelect.Selected < 0) return;
            string target = _giftTargetSelect.GetItemMetadata(_giftTargetSelect.Selected).AsString();
            bool ok = _host.Gift(_selectedSurvivorId, target, belonging.BelongingId);
            _feedback.Text = ok ? $"Gifted {belonging.ItemName} to {target}." : "Gift refused.";
            RefreshView();
        }

        private void OnLossPressed(PersonalBelonging belonging)
        {
            if (_host == null) return;
            bool ok = _host.ReportLoss(_selectedSurvivorId, belonging.BelongingId, stolen: true);
            _feedback.Text = ok ? $"Reported {belonging.ItemName} lost/stolen." : "Loss report failed.";
            RefreshView();
        }

        public void RefreshView()
        {
            if (_host == null || !IsInsideTree()) return;

            var survivors = _host.SurvivorIdsProvider?.Invoke() ?? new List<string>();
            var items = _host.InventoryItemIdsProvider?.Invoke() ?? new List<string>();
            var templates = _host.Templates.ToList();

            RebuildOptions(_survivorSelect, survivors, _host.SurvivorNameProvider);
            if (string.IsNullOrEmpty(_selectedSurvivorId) && survivors.Count > 0)
                _selectedSurvivorId = survivors[0];
            SelectByMetadata(_survivorSelect, _selectedSurvivorId);
            RebuildOptions(_itemSelect, items, null);
            RebuildOptions(_giftTargetSelect, survivors.Where(s => s != _selectedSurvivorId).ToList(), _host.SurvivorNameProvider);
            RebuildOptions(_templateSelect, templates.Select(t => t.Id).ToList(), id => templates.FirstOrDefault(t => t.Id == id)?.Name);

            int totalClaims = 0;
            int favorites = 0;
            foreach (string survivor in survivors)
            {
                var held = _host.GetBelongingsFor(survivor);
                totalClaims += held.Count;
                favorites += held.Count(b => b.IsFavorite);
            }
            _statusRail?.Set("claims", totalClaims.ToString());
            _statusRail?.Set("favorites", favorites.ToString());
            _statusRail?.Set("templates", templates.Count.ToString());

            foreach (Node child in _belongingsContainer.GetChildren()) child.QueueFree();

            var selectedHeld = string.IsNullOrEmpty(_selectedSurvivorId)
                ? new List<PersonalBelonging>()
                : _host.GetBelongingsFor(_selectedSurvivorId).ToList();

            if (selectedHeld.Count == 0)
            {
                _belongingsContainer.AddChild(new Label { Text = "No personal effects held by this survivor." });
                return;
            }

            foreach (var belonging in selectedHeld)
            {
                var card = new PanelContainer();
                var box = new VBoxContainer();
                box.AddThemeConstantOverride("separation", 3);
                box.AddChild(new Label { Text = $"{belonging.ItemName} ({belonging.ItemId})" });
                box.AddChild(new Label
                {
                    Text = $"{belonging.Category} | sentiment {belonging.SentimentalValue:0} | condition {belonging.Condition:0} | acquired day {belonging.AcquiredDay} from {belonging.AcquiredFrom}"
                });
                box.AddChild(new Label { Text = belonging.IsFavorite ? "FAVORITE" : (belonging.IsInherited ? "inherited" : "not favorite") });

                var actions = new HBoxContainer();
                actions.AddThemeConstantOverride("separation", 6);
                var captured = belonging;

                var favorite = new Button { Text = belonging.IsFavorite ? "Unfavorite" : "Favorite" };
                favorite.Pressed += () => OnFavoritePressed(captured);
                actions.AddChild(favorite);

                var gift = new Button { Text = "Gift" };
                gift.Pressed += () => OnGiftPressed(captured);
                actions.AddChild(gift);

                var loss = new Button { Text = "Report loss" };
                loss.Pressed += () => OnLossPressed(captured);
                actions.AddChild(loss);

                box.AddChild(actions);
                card.AddChild(box);
                _belongingsContainer.AddChild(card);
            }
        }

        private static void RebuildOptions(OptionButton select, IReadOnlyList<string> ids, Func<string, string>? nameProvider)
        {
            int previous = select.Selected;
            string previousId = previous >= 0 ? select.GetItemMetadata(previous).AsString() : string.Empty;
            select.Clear();
            foreach (string id in ids)
            {
                string label = nameProvider?.Invoke(id) ?? id;
                select.AddItem(string.IsNullOrWhiteSpace(label) ? id : $"{label} ({id})");
                select.SetItemMetadata(select.ItemCount - 1, id);
            }
            if (!string.IsNullOrEmpty(previousId))
                SelectByMetadata(select, previousId);
        }

        private static void SelectByMetadata(OptionButton select, string id)
        {
            if (string.IsNullOrEmpty(id)) return;
            for (int i = 0; i < select.ItemCount; i++)
            {
                if (string.Equals(select.GetItemMetadata(i).AsString(), id, StringComparison.Ordinal))
                {
                    select.Select(i);
                    return;
                }
            }
        }
    }
}
