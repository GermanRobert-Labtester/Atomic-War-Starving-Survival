// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.UI;
using Ashfall.Core;
using Ashfall.Core.Inventory;

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
    private ItemDescriptionCatalog? _descriptions;
    private ExpansionEnrichmentCatalog? _enrichment;
    private string _itemId = string.Empty;
    private System.Collections.Generic.IReadOnlyList<Ashfall.Core.Narrative.TechnicalMaterialRecord>? _technicalProvenance;
    private System.Collections.Generic.IReadOnlyList<Ashfall.Core.Narrative.LeatherworkRecord>? _leatherProvenance;

    public bool IsBound => _inventory != null && !string.IsNullOrEmpty(_itemId);
    public int RenderedRowCount { get; private set; }
    public ItemInspectionModel? CurrentInspection { get; private set; }

    public void Bind(InventoryHostSession? inventory, string itemId, ItemDescriptionCatalog? descriptions = null, ExpansionEnrichmentCatalog? enrichment = null)
        => Bind(inventory, itemId, descriptions, enrichment, technicalProvenance: null);

    /// <summary>
    /// Plan 158 — optional technical-material provenance for canonical items
    /// (rope, masks, film goods). Display-only: archived records render as
    /// dim provenance lines; no condition/durability state is read or
    /// changed. Null (default) renders nothing — existing callers untouched.
    /// </summary>
    public void Bind(InventoryHostSession? inventory, string itemId, ItemDescriptionCatalog? descriptions, ExpansionEnrichmentCatalog? enrichment, System.Collections.Generic.IReadOnlyList<Ashfall.Core.Narrative.TechnicalMaterialRecord>? technicalProvenance)
        => Bind(inventory, itemId, descriptions, enrichment, technicalProvenance, leatherProvenance: null);

    /// <summary>
    /// Plan 159 — optional leather provenance for canonical items (masks,
    /// curing salt, strap leather). Display-only: archived tanning/currying
    /// records render as dim provenance lines describing one historical
    /// production batch; no condition/durability/trade state is read or
    /// changed. Null (default) renders nothing — existing callers untouched.
    /// </summary>
    public void Bind(InventoryHostSession? inventory, string itemId, ItemDescriptionCatalog? descriptions, ExpansionEnrichmentCatalog? enrichment, System.Collections.Generic.IReadOnlyList<Ashfall.Core.Narrative.TechnicalMaterialRecord>? technicalProvenance, System.Collections.Generic.IReadOnlyList<Ashfall.Core.Narrative.LeatherworkRecord>? leatherProvenance)
    {
        _inventory = inventory;
        _itemId = itemId ?? string.Empty;
        _descriptions = descriptions ?? inventory?.DescriptionCatalog;
        _enrichment = enrichment ?? inventory?.EnrichmentCatalog;
        _technicalProvenance = technicalProvenance;
        _leatherProvenance = leatherProvenance;
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
        CurrentInspection = null;

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
        var inspection = ItemInspectionModel.Create(def, _descriptions ?? _inventory?.DescriptionCatalog, _enrichment ?? _inventory?.EnrichmentCatalog);
        CurrentInspection = inspection;

        // ── Item info ──
        AddRow(_itemInfo, $"Name: {inspection.DisplayName}", Ashfall.Core.UI.Theme.Pale);
        AddRow(_itemInfo, $"ID: {inspection.ItemId}", Ashfall.Core.UI.Theme.Dim);
        AddRow(_itemInfo, $"Type: {inspection.Type}", Ashfall.Core.UI.Theme.Lethe);
        AddRow(_itemInfo, $"In Stock: {count}", count > 0 ? Ashfall.Core.UI.Theme.Warm : Ashfall.Core.UI.Theme.Dim);
        RenderedRowCount += 4;

        if (inspection.IsKeepsakeCandidate)
        {
            AddRow(_itemInfo, "Keepsake: can be kept", Ashfall.Core.UI.Theme.Warm);
            RenderedRowCount++;
        }

        if (!string.IsNullOrEmpty(inspection.BaseDescription))
        {
            AddRow(_itemInfo, inspection.BaseDescription, Ashfall.Core.UI.Theme.Dim);
            RenderedRowCount++;
        }

        // ── Technical provenance (Plan 158, display-only) ──
        if (_technicalProvenance != null && _technicalProvenance.Count > 0)
        {
            foreach (var record in _technicalProvenance)
            {
                string summary = string.IsNullOrEmpty(record.FailureSummary)
                    ? record.MeasurementSummary
                    : $"{record.MeasurementSummary} — {record.FailureSummary.Replace('_', ' ').ToLowerInvariant()}";
                AddRow(_itemInfo, $"Archive [{record.Family}]: {record.ObjectLabel.Replace('_', ' ')} — {summary} (ARCHIVAL — PRESENT STATUS UNKNOWN)", Ashfall.Core.UI.Theme.Lethe);
                RenderedRowCount++;
            }
        }

        // ── Leather provenance (Plan 159, display-only) ──
        if (_leatherProvenance != null && _leatherProvenance.Count > 0)
        {
            foreach (var record in _leatherProvenance)
            {
                string summary = string.IsNullOrEmpty(record.FailureSummary)
                    ? record.MeasurementSummary
                    : $"{record.MeasurementSummary} — {record.FailureSummary.Replace('_', ' ').ToLowerInvariant()}";
                AddRow(_itemInfo, $"Tanning record [{record.Family}]: {record.FacilityLabel.Replace('_', ' ')} — {summary} (ARCHIVAL BATCH — NOT THIS ITEM'S MEASURED QUALITY)", Ashfall.Core.UI.Theme.Lethe);
                RenderedRowCount++;
            }
        }

        if (inspection.HasEnhancedDescription)
        {
            if (!string.IsNullOrEmpty(inspection.VisualIndicators))
            {
                AddRow(_itemInfo, $"Visual: {inspection.VisualIndicators}", Ashfall.Core.UI.Theme.Pale);
                RenderedRowCount++;
            }
            if (!string.IsNullOrEmpty(inspection.SensoryDetails))
            {
                AddRow(_itemInfo, $"Sensory: {inspection.SensoryDetails}", Ashfall.Core.UI.Theme.Dim);
                RenderedRowCount++;
            }
            if (!string.IsNullOrEmpty(inspection.CurrentState))
            {
                AddRow(_itemInfo, $"Condition: {inspection.CurrentState}", Ashfall.Core.UI.Theme.Warm);
                RenderedRowCount++;
            }
            if (!string.IsNullOrEmpty(inspection.Hazards))
            {
                AddRow(_itemInfo, $"Hazards: {inspection.Hazards}", Ashfall.Core.UI.Theme.Warm);
                RenderedRowCount++;
            }
            if (!string.IsNullOrEmpty(inspection.PreservationState))
            {
                AddRow(_itemInfo, $"Preservation: {inspection.PreservationState}", Ashfall.Core.UI.Theme.Pale);
                RenderedRowCount++;
            }
            if (!string.IsNullOrEmpty(inspection.MakeshiftUtility))
            {
                AddRow(_itemInfo, $"Makeshift: {inspection.MakeshiftUtility}", Ashfall.Core.UI.Theme.Lethe);
                RenderedRowCount++;
            }
        }

        // ── Stats ──
        int statCount = 0;
        if (def.radProtection > 0) { AddRow(_itemStats, $"Rad Protection: {def.radProtection * 100f:0}%", Ashfall.Core.UI.Theme.Lethe); RenderedRowCount++; statCount++; }
        if (def.durability > 0) { AddRow(_itemStats, $"Durability: {def.durability:0}", Ashfall.Core.UI.Theme.Pale); RenderedRowCount++; statCount++; }
        if (def.hungerRestore > 0) { AddRow(_itemStats, $"Hunger Restore: {def.hungerRestore:0}", Ashfall.Core.UI.Theme.Warm); RenderedRowCount++; statCount++; }
        if (def.thirstRestore > 0) { AddRow(_itemStats, $"Thirst Restore: {def.thirstRestore:0}", Ashfall.Core.UI.Theme.Warm); RenderedRowCount++; statCount++; }
        if (def.healthEffect > 0) { AddRow(_itemStats, $"Health Effect: +{def.healthEffect:0}", Ashfall.Core.UI.Theme.Lethe); RenderedRowCount++; statCount++; }
        if (def.radCleanse > 0) { AddRow(_itemStats, $"Rad Cleanse: −{def.radCleanse:0} mSv", Ashfall.Core.UI.Theme.Lethe); RenderedRowCount++; statCount++; }
        if (def.moraleEffect > 0) { AddRow(_itemStats, $"Morale Effect: +{def.moraleEffect:0}", Ashfall.Core.UI.Theme.Warm); RenderedRowCount++; statCount++; }
        if (def.tradeValue > 0) { AddRow(_itemStats, $"Trade Value: {def.tradeValue:0} (tier {def.tradeTier})", Ashfall.Core.UI.Theme.Pale); RenderedRowCount++; statCount++; }
        if (def.isEquipable) { AddRow(_itemStats, $"Equipable: {def.equipSlot}", Ashfall.Core.UI.Theme.Lethe); RenderedRowCount++; statCount++; }
        if (statCount == 0)
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
        var label = new Label { Text = text, AutowrapMode = TextServer.AutowrapMode.WordSmart };
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
        if (AshfallInputActions.IsCloseOrCancel(@event))
        {
            OnClose?.Invoke();
            GetViewport().SetInputAsHandled();
        }
    }
}
