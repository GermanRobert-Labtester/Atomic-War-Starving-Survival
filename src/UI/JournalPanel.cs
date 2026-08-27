using System;
using System.Linq;
using Godot;
using Ashfall.Core.IO;
using Ashfall.Core.Journal;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Journal panel (wired).
/// Shows real journal entries, discovered items, survivors met, locations
/// visited, and narrative events from the live JournalSystem. Replaces the
/// previous hardcoded placeholder strings with live data binding.
/// </summary>
public partial class JournalPanel : Control
{
    public event Action? OnClose;

    private AshfallDashboardShell _shell = null!;
    private AshfallSidebar? _sidebar;
    private JournalSystem? _journal;

    private ScrollContainer _scrollRoot = null!;

    // Tab containers
    private VBoxContainer _logEntries = null!;
    private VBoxContainer _itemsList = null!;
    private VBoxContainer _peopleList = null!;
    private VBoxContainer _placesList = null!;
    private VBoxContainer _eventsList = null!;

    // Section headers (scroll targets)
    private Label _lblLogsTitle = null!;
    private Label _lblItemsTitle = null!;
    private Label _lblPeopleTitle = null!;
    private Label _lblPlacesTitle = null!;
    private Label _lblEventsTitle = null!;

    /// <summary>Bind the live JournalHostSession.</summary>
    public void Bind(JournalHostSession session) => Bind(session?.System!);

    /// <summary>Bind the live JournalSystem. Re-subscribes events and refreshes.</summary>
    public void Bind(JournalSystem journal)
    {
        if (_journal != null)
        {
            _journal.OnEntryAdded -= OnEntryAdded;
            _journal.OnTabChanged -= OnTabChanged;
            _journal.OnCodexUnlocked -= OnCodexUnlocked;
        }

        _journal = journal;

        if (_journal != null)
        {
            _journal.OnEntryAdded += OnEntryAdded;
            _journal.OnTabChanged += OnTabChanged;
            _journal.OnCodexUnlocked += OnCodexUnlocked;
        }

        RefreshView();
    }

    private void OnEntryAdded(JournalEntry _) => RefreshView();
    private void OnTabChanged(int _) => RefreshView();
    private void OnCodexUnlocked(string _) => RefreshView();

    public void RefreshView()
    {
        if (_journal == null || _logEntries == null || _itemsList == null || _peopleList == null || _placesList == null || _eventsList == null) return;

        // Clear all tab containers
        ClearContainer(_logEntries);
        ClearContainer(_itemsList);
        ClearContainer(_peopleList);
        ClearContainer(_placesList);
        ClearContainer(_eventsList);

        // ── Log tab: all entries newest-first ──
        if (_journal.Entries.Count == 0)
        {
            AddEmptyHint(_logEntries, "No entries yet. Explore the wasteland.");
        }
        else
        {
            foreach (var entry in _journal.Entries)
            {
                AddEntryLabel(_logEntries, entry);
            }
        }

        // ── Items tab: discovered items ──
        var itemKeys = _journal.Knowledge.Snapshot()
            .Where(k => k.StartsWith("item_seen_"))
            .OrderBy(k => k)
            .ToList();
        if (itemKeys.Count == 0)
        {
            AddEmptyHint(_itemsList, "No items discovered yet.");
        }
        else
        {
            foreach (var key in itemKeys)
            {
                var itemId = key.Substring("item_seen_".Length);
                AddDiscoveryLabel(_itemsList, FormatId(itemId), "ITEM");
            }
        }

        // ── People tab: survivors met ──
        var peopleKeys = _journal.Knowledge.Snapshot()
            .Where(k => k.StartsWith("survivor_met_"))
            .OrderBy(k => k)
            .ToList();
        if (peopleKeys.Count == 0)
        {
            AddEmptyHint(_peopleList, "No survivors met yet.");
        }
        else
        {
            foreach (var key in peopleKeys)
            {
                var survivorId = key.Substring("survivor_met_".Length);
                AddDiscoveryLabel(_peopleList, FormatId(survivorId), "SURVIVOR");
            }
        }

        // ── Places tab: locations visited ──
        var placeKeys = _journal.Knowledge.Snapshot()
            .Where(k => k.StartsWith("location_visited_"))
            .OrderBy(k => k)
            .ToList();
        if (placeKeys.Count == 0)
        {
            AddEmptyHint(_placesList, "No locations visited yet.");
        }
        else
        {
            foreach (var key in placeKeys)
            {
                var locId = key.Substring("location_visited_".Length);
                AddDiscoveryLabel(_placesList, FormatId(locId), "LOCATION");
            }
        }

        // ── Events tab: narrative events fired ──
        var eventKeys = _journal.Knowledge.Snapshot()
            .Where(k => k.StartsWith("event_fired_"))
            .OrderBy(k => k)
            .ToList();
        if (eventKeys.Count == 0)
        {
            AddEmptyHint(_eventsList, "No narrative events yet.");
        }
        else
        {
            foreach (var key in eventKeys)
            {
                var eventId = key.Substring("event_fired_".Length);
                AddDiscoveryLabel(_eventsList, FormatId(eventId), "EVENT");
            }
        }

        UpdateTabVisibility();
    }

    private void UpdateTabVisibility()
    {
        // All sections always visible; sidebar scrolls to the selected one.
        // Section visibility is controlled by the scroll-to logic in the
        // sidebar handler, not by hiding containers.
    }

    private static string FormatId(string snakeId)
    {
        if (string.IsNullOrEmpty(snakeId)) return "???";
        return snakeId.Replace("_", " ").Trim();
    }

    private static void ClearContainer(VBoxContainer? container)
    {
        if (container == null) return;
        AshfallUiHelpers.EmptyChildren(container);
    }

    private void AddEntryLabel(VBoxContainer container, JournalEntry entry)
    {
        var when = string.IsNullOrEmpty(entry.Timestamp) ? $"Day {entry.Day}" : entry.Timestamp;
        var who = string.IsNullOrEmpty(entry.AuthorName) ? "Someone" : entry.AuthorName;

        var hbox = new HBoxContainer();
        hbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);

        var lblWhen = new Label { Text = when };
        lblWhen.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeSmall);
        lblWhen.AddThemeFontOverride("font", AshfallUiHelpers.LoadFont("res://assets/fonts/ShareTechMono-Regular.ttf"));
        lblWhen.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
        lblWhen.CustomMinimumSize = new Vector2(90, 0);
        hbox.AddChild(lblWhen);

        var lblWho = new Label { Text = who + ":" };
        lblWho.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
        lblWho.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
        lblWho.CustomMinimumSize = new Vector2(110, 0);
        hbox.AddChild(lblWho);

        var lblText = new Label { Text = entry.Text };
        lblText.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
        lblText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
        lblText.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        lblText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
        hbox.AddChild(lblText);

        container.AddChild(hbox);
    }

    private void AddDiscoveryLabel(VBoxContainer container, string displayName, string kind)
    {
        var hbox = new HBoxContainer();
        hbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);

        var lblKind = new Label { Text = $"[{kind}]" };
        lblKind.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeSmall);
        lblKind.AddThemeFontOverride("font", AshfallUiHelpers.LoadFont("res://assets/fonts/ShareTechMono-Regular.ttf"));
        lblKind.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
        lblKind.CustomMinimumSize = new Vector2(70, 0);
        hbox.AddChild(lblKind);

        var lblName = new Label { Text = displayName };
        lblName.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
        lblName.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
        lblName.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        lblName.AutowrapMode = TextServer.AutowrapMode.WordSmart;
        hbox.AddChild(lblName);

        container.AddChild(hbox);
    }

    private void AddEmptyHint(VBoxContainer container, string hint)
    {
        var lbl = new Label { Text = hint };
        lbl.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeSmall);
        lbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Muted));
        lbl.AutowrapMode = TextServer.AutowrapMode.WordSmart;
        container.AddChild(lbl);
    }

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);
        Visible = false;

        var bg = new ColorRect { Color = new Color(0.05f, 0.05f, 0.05f, 0.92f) };
        bg.SetAnchorsPreset(LayoutPreset.FullRect);
        AddChild(bg);

        var center = new CenterContainer();
        center.SetAnchorsPreset(LayoutPreset.FullRect);
        AddChild(center);

        _shell = new AshfallDashboardShell("JOURNAL & NARRATIVE", 820, 600);
        center.AddChild(_shell);

        _sidebar = _shell.SetSidebar(new[]
        {
            new AshfallSidebar.Item { Id = "log",     Label = "Day Log",       Hint = "CHRONICLE" },
            new AshfallSidebar.Item { Id = "items",   Label = "Items",         Hint = "DISCOVERIES" },
            new AshfallSidebar.Item { Id = "people",  Label = "People",        Hint = "SURVIVORS MET" },
            new AshfallSidebar.Item { Id = "places",  Label = "Places",        Hint = "LOCATIONS" },
            new AshfallSidebar.Item { Id = "events",  Label = "Events",        Hint = "NARRATIVE" },
        }, "CHAPTERS", "log");

        _shell.AttachHeaderCloseButton("CLOSE [Esc]", () => OnClose?.Invoke());

        var scrollRoot = new ScrollContainer();
        scrollRoot.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        scrollRoot.SizeFlagsVertical = SizeFlags.ExpandFill;
        var scrollMargin = new MarginContainer();
        scrollMargin.AddThemeConstantOverride("margin_left", DesignTheme.SpacingLg);
        scrollMargin.AddThemeConstantOverride("margin_top", DesignTheme.SpacingMd);
        scrollMargin.AddThemeConstantOverride("margin_right", DesignTheme.SpacingLg);
        scrollMargin.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingMd);
        scrollRoot.AddChild(scrollMargin);
        _shell.SetContent(scrollRoot);
        _scrollRoot = scrollRoot;

        var vbox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingLg);
        vbox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        scrollMargin.AddChild(vbox);

        // Day Log section
        _lblLogsTitle = AshfallUiHelpers.MakeSectionHeader("DAY LOG");
        vbox.AddChild(_lblLogsTitle);
        _logEntries = new VBoxContainer();
        _logEntries.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _logEntries.CustomMinimumSize = new Vector2(500, 0);
        vbox.AddChild(_logEntries);

        vbox.AddChild(AshfallUiHelpers.MakeSeparator());

        // Items section
        _lblItemsTitle = AshfallUiHelpers.MakeSectionHeader("DISCOVERED ITEMS");
        vbox.AddChild(_lblItemsTitle);
        _itemsList = new VBoxContainer();
        _itemsList.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _itemsList.CustomMinimumSize = new Vector2(500, 0);
        vbox.AddChild(_itemsList);

        vbox.AddChild(AshfallUiHelpers.MakeSeparator());

        // People section
        _lblPeopleTitle = AshfallUiHelpers.MakeSectionHeader("SURVIVORS MET");
        vbox.AddChild(_lblPeopleTitle);
        _peopleList = new VBoxContainer();
        _peopleList.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _peopleList.CustomMinimumSize = new Vector2(500, 0);
        vbox.AddChild(_peopleList);

        vbox.AddChild(AshfallUiHelpers.MakeSeparator());

        // Places section
        _lblPlacesTitle = AshfallUiHelpers.MakeSectionHeader("LOCATIONS VISITED");
        vbox.AddChild(_lblPlacesTitle);
        _placesList = new VBoxContainer();
        _placesList.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _placesList.CustomMinimumSize = new Vector2(500, 0);
        vbox.AddChild(_placesList);

        vbox.AddChild(AshfallUiHelpers.MakeSeparator());

        // Events section
        _lblEventsTitle = AshfallUiHelpers.MakeSectionHeader("NARRATIVE EVENTS");
        vbox.AddChild(_lblEventsTitle);
        _eventsList = new VBoxContainer();
        _eventsList.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _eventsList.CustomMinimumSize = new Vector2(500, 0);
        vbox.AddChild(_eventsList);

        if (_sidebar != null)
        {
            _sidebar.OnSelected += id =>
            {
                Label? target = id switch
                {
                    "log"    => _lblLogsTitle,
                    "items"  => _lblItemsTitle,
                    "people" => _lblPeopleTitle,
                    "places" => _lblPlacesTitle,
                    "events" => _lblEventsTitle,
                    _        => null,
                };
                if (target != null)
                    ScrollToChild(_scrollRoot, target);
            };
        }

        RefreshView();
    }

    private static void ScrollToChild(ScrollContainer? scroll, Control? child)
    {
        if (scroll == null || child == null) return;
        try
        {
            float targetOffset = 0f;
            Node walker = child;
            while (walker != null && walker != scroll)
            {
                if (walker is Control w && walker != scroll)
                    targetOffset += w.Position.Y;
                walker = walker.GetParent();
            }
            if (targetOffset > 0)
                scroll.ScrollVertical = (int)Math.Max(0, targetOffset - 8);
        }
        catch (Exception ex)
        {
            CatalogDiagnostics.Warn("<scroll>", "ScrollToChild", ex);
        }
    }

    public void Open()
    {
        Visible = true;
        RefreshView();
        QueueRedraw();
    }

    public void Close()
    {
        Visible = false;
    }

    public void Unbind()
    {
        if (_journal != null)
        {
            _journal.OnEntryAdded -= OnEntryAdded;
            _journal.OnTabChanged -= OnTabChanged;
            _journal.OnCodexUnlocked -= OnCodexUnlocked;
            _journal = null;
        }

        if (_logEntries != null) ClearContainer(_logEntries);
        if (_itemsList != null) ClearContainer(_itemsList);
        if (_peopleList != null) ClearContainer(_peopleList);
        if (_placesList != null) ClearContainer(_placesList);
        if (_eventsList != null) ClearContainer(_eventsList);
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

    public override void _ExitTree()
    {
        base._ExitTree();
        if (_journal != null)
        {
            _journal.OnEntryAdded -= OnEntryAdded;
            _journal.OnTabChanged -= OnTabChanged;
            _journal.OnCodexUnlocked -= OnCodexUnlocked;
            _journal = null;
        }
    }
}
