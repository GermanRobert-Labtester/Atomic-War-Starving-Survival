using System;
using Godot;
using Ashfall.Core.UI;
using Ashfall.Core;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Inventory Detail panel. Shows item info, stats, and available
/// actions for a specific item — bound to the live InventoryHostSession.
///
/// Ticket #125: layout is owned by <c>res://assets/ui/panels/InventoryDetailPanel.tscn</c>
/// (backdrop, dialog frame, sections, separators, close button). This class
/// is a typed binder: it discovers the scene's required nodes, then projects
/// presentation data into them. Refresh/RefreshView stays C# because the
/// inventory row contents are dynamic; the surrounding chrome stays in
/// the scene.
/// </summary>
public partial class InventoryDetailPanel : Control
{
    public event Action? OnClose;
    public event Action<string>? OnConsume;
    public event Action<string>? OnEquip;

    private SceneBinder? _binder;

    // Cached dynamic-content containers the binder fills at Refresh time.
    private VBoxContainer _itemInfo = null!;
    private VBoxContainer _itemStats = null!;
    private VBoxContainer _itemActions = null!;
    private Button _closeButton = null!;
    private ColorRect _backdrop = null!;

    private InventoryHostSession? _inventory;
    private string _itemId = string.Empty;

    public bool IsBound => _inventory != null && !string.IsNullOrEmpty(_itemId);
    public int RenderedRowCount { get; private set; }

    public void Bind(InventoryHostSession? inventory, string itemId)
    {
        _inventory = inventory;
        _itemId = itemId ?? string.Empty;
        RefreshView();
    }

    public override void _Ready()
    {
        // Scene composition lives entirely in InventoryDetailPanel.tscn.
        // The C# binder resolves typed unique-name nodes via SceneBinder
        // and never re-creates layout primitives at runtime.
        _binder = new SceneBinder(this, typeof(InventoryDetailPanel));
        _binder.Require<ColorRect>("Backdrop");
        _binder.Require<VBoxContainer>("Info");
        _binder.Require<VBoxContainer>("Stats");
        _binder.Require<VBoxContainer>("Actions");
        _binder.Require<Button>("CloseButton");

        _itemInfo = _binder.Get<VBoxContainer>("Info");
        _itemStats = _binder.Get<VBoxContainer>("Stats");
        _itemActions = _binder.Get<VBoxContainer>("Actions");
        _closeButton = _binder.Get<Button>("CloseButton");
        _backdrop = _binder.Get<ColorRect>("Backdrop");
        _closeButton.Pressed += () => OnClose?.Invoke();

        Visible = false;
    }

    public void RefreshView()
    {
        if (_itemInfo == null || _itemStats == null || _itemActions == null) return;

        AshfallUiHelpers.EmptyChildren(_itemInfo);
        AshfallUiHelpers.EmptyChildren(_itemStats);
        AshfallUiHelpers.EmptyChildren(_itemActions);

        RenderedRowCount = 0;

        if (_inventory?.Inventory == null || string.IsNullOrEmpty(_itemId))
        {
            _itemInfo.AddChild(MakeDimLine("No item selected."));
            return;
        }

        var slot = _inventory.Inventory.FindSlot(_itemId);
        if (slot == null)
        {
            _itemInfo.AddChild(MakeDimLine($"Item '{_itemId}' not in inventory."));
            return;
        }

        var def = slot.Item;
        int count = _inventory.Inventory.CountById(_itemId);

        // ── Item info ──
        AddRow(_itemInfo, $"Name: {def.displayName}", Ashfall.Core.UI.Theme.Pale);
        AddRow(_itemInfo, $"ID: {def.id}", Ashfall.Core.UI.Theme.Dim);
        AddRow(_itemInfo, $"Type: {def.type}", Ashfall.Core.UI.Theme.Lethe);
        AddRow(_itemInfo, $"In Stock: {count}", count > 0 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim);
        RenderedRowCount += 4;

        if (!string.IsNullOrEmpty(def.description))
        {
            AddRow(_itemInfo, def.description, Ashfall.Core.UI.Theme.Dim);
            RenderedRowCount++;
        }

        // ── Stats ──
        if (def.radProtection > 0) { AddRow(_itemStats, $"Rad Protection: {def.radProtection * 100f:0}%", Ashfall.Core.UI.Theme.Lethe); RenderedRowCount++; }
        if (def.durability > 0) { AddRow(_itemStats, $"Durability: {def.durability:0}", Ashfall.Core.UI.Theme.Pale); RenderedRowCount++; }
        if (def.hungerRestore > 0) { AddRow(_itemStats, $"Hunger Restore: {def.hungerRestore:0}", Ashfall.Core.UI.Theme.Warm); RenderedRowCount++; }
        if (def.thirstRestore > 0) { AddRow(_itemStats, $"Thirst Restore: {def.thirstRestore:0}", Ashfall.Core.UI.Theme.Warm); RenderedRowCount++; }
        if (def.healthEffect > 0) { AddRow(_itemStats, $"Health Effect: +{def.healthEffect:0}", Ashfall.Core.UI.Theme.Lethe); RenderedRowCount++; }
        if (def.radCleanse > 0) { AddRow(_itemStats, $"Rad Cleanse: −{def.radCleanse:0} mSv", Ashfall.Core.UI.Theme.Lethe); RenderedRowCount++; }
        if (def.moraleEffect > 0) { AddRow(_itemStats, $"Morale Effect: +{def.moraleEffect:0}", Ashfall.Core.UI.Theme.Warm); RenderedRowCount++; }
        if (def.tradeValue > 0) { AddRow(_itemStats, $"Trade Value: {def.tradeValue:0} (tier {def.tradeTier})", Ashfall.Core.UI.Theme.Pale); RenderedRowCount++; }
        if (def.isEquipable) { AddRow(_itemStats, $"Equipable: {def.equipSlot}", Ashfall.Core.UI.Theme.Lethe); RenderedRowCount++; }
        if (RenderedRowCount == 4)
            _itemStats.AddChild(MakeDimLine("No special stats."));

        // ── Actions (contextual) ──
        if (def.IsConsumable())
        {
            var btn = new Button { Text = count > 0 ? $"CONSUME {def.displayName.ToUpperInvariant()}" : "CONSUME (OUT OF STOCK)", Disabled = count <= 0 };
            btn.CustomMinimumSize = new Vector2(240, 32);
            string capturedId = def.id;
            btn.Pressed += () => OnConsume?.Invoke(capturedId);
            _itemActions.AddChild(btn);
        }
        else
        {
            AddRow(_itemActions, "Consume: not consumable", Ashfall.Core.UI.Theme.Dim);
        }

        if (def.isEquipable)
        {
            var btn = new Button { Text = count > 0 ? $"EQUIP ({def.equipSlot})" : $"EQUIP ({def.equipSlot}) (OUT OF STOCK)", Disabled = count <= 0 };
            btn.CustomMinimumSize = new Vector2(240, 32);
            string capturedId = def.id;
            btn.Pressed += () => OnEquip?.Invoke(capturedId);
            _itemActions.AddChild(btn);
        }
        else
        {
            AddRow(_itemActions, "Equip: not equipable", Ashfall.Core.UI.Theme.Dim);
        }
        RenderedRowCount += 2;
    }

    private void AddRow(VBoxContainer parent, string text, (float r, float g, float b, float a) col)
    {
        var label = new Label { Text = text };
        label.CustomMinimumSize = new Vector2(400, 0);
        label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
        label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(col));
        parent.AddChild(label);
    }

    private Label MakeDimLine(string text)
    {
        var l = new Label { Text = text };
        l.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
        l.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
        return l;
    }

    public void Open()
    {
        Visible = true;
        QueueRedraw();
    }

    public override void _GuiInput(InputEvent @event)
    {
        // Scene-owned backdrop sits inside the Control; swallow clicks so
        // the panel does not pass input through to game-world UI beneath it.
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!Visible) return;
        if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
        {
            OnClose?.Invoke();
            GetViewport().SetInputAsHandled();
        }
    }
}
