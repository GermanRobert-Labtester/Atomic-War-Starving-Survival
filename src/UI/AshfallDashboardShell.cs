using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Dashboard Shell.
/// Outer chrome for a dashboard-style runtime surface. Composes:
///   • Header strip (title left)
///   • Optional ASHFALL sidebar (left rail)
///   • Optional ASHFALL status rail (top metric strip)
///   • Content slot (caller fills with the actual panel content)
///
/// The shell formalises the existing modal-style chrome into a layout where
/// a sidebar + status rail can wrap any tall content stack, without forcing
/// dashboard chrome on small confirm/prompt surfaces. Hosts construct the
/// shell, then call SetSidebar / SetStatusRail / SetContent /
/// AttachHeaderCloseButton before adding it to the scene tree.
///
/// All colors / fonts / spacing come from AshfallUiHelpers and
/// DesignTheme tokens.
/// </summary>
public partial class AshfallDashboardShell : PanelContainer
{
    private readonly VBoxContainer _outerStack;
    private readonly HBoxContainer _bodyRow;          // sidebar + content
    private readonly VBoxContainer _contentStack;     // status rail + content
    private readonly PanelContainer _headerBar;
    private readonly HBoxContainer _headerHbox;
    private readonly Label _titleLabel;

    private string _titleText = string.Empty;

    public AshfallSidebar? Sidebar { get; private set; }
    public AshfallStatusRail? StatusRail { get; private set; }
    public Node? ContentRef { get; private set; }

    public string Title
    {
        get => _titleText;
        set
        {
            _titleText = value ?? string.Empty;
            if (_titleLabel != null)
                _titleLabel.Text = _titleText.ToUpperInvariant();
        }
    }

    public AshfallDashboardShell(string title, int minWidth = 720, int minHeight = 480)
    {
        CustomMinimumSize = new Vector2(minWidth, minHeight);

        AddThemeStyleboxOverride("panel", AshfallUiHelpers.MakePanelFrameStyleBox());

        _outerStack = new VBoxContainer();
        _outerStack.AddThemeConstantOverride("separation", 0);
        _outerStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        _outerStack.SizeFlagsVertical = SizeFlags.ExpandFill;
        AddChild(_outerStack);

        _headerBar = new PanelContainer();
        _headerBar.AddThemeStyleboxOverride("panel", AshfallUiHelpers.MakeHeaderFrameStyleBox());
        _outerStack.AddChild(_headerBar);

        _headerHbox = new HBoxContainer();
        _headerHbox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _headerBar.AddChild(_headerHbox);

        // Flat bar, amber condensed title, close button pushed right by the
        // label's fill. The tab_strip plate was tried behind the title but its
        // measured width proved unreliable across title lengths.
        _titleLabel = new Label
        {
            Text = string.IsNullOrEmpty(title) ? "—" : title.ToUpperInvariant(),
            HorizontalAlignment = HorizontalAlignment.Left
        };
        _titleLabel.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeH2);
        _titleLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
        var titleFont = AshfallUiHelpers.LoadFont("res://assets/fonts/BarlowCondensed-SemiBold.ttf");
        if (titleFont != null) _titleLabel.AddThemeFontOverride("font", titleFont);
        _titleLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        _headerHbox.AddChild(_titleLabel);

        _bodyRow = new HBoxContainer();
        _bodyRow.AddThemeConstantOverride("separation", 0);
        _bodyRow.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        _bodyRow.SizeFlagsVertical = SizeFlags.ExpandFill;
        _outerStack.AddChild(_bodyRow);

        _contentStack = new VBoxContainer();
        _contentStack.AddThemeConstantOverride("separation", 0);
        _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;
        _bodyRow.AddChild(_contentStack);
    }

    /// <summary>
    /// Adds a sidebar to the left of the body row.
    /// </summary>
    public AshfallSidebar SetSidebar(AshfallSidebar.Item[] items, string headerLabel, string initialSelectedId)
    {
        Sidebar = new AshfallSidebar(items, headerLabel, initialSelectedId);
        Sidebar.SizeFlagsVertical = SizeFlags.ExpandFill;
        _bodyRow.AddChild(Sidebar);
        _bodyRow.MoveChild(Sidebar, 0); // ensure leftmost
        return Sidebar;
    }

    /// <summary>
    /// Adds a status rail at the top of the content stack.
    /// </summary>
    public AshfallStatusRail SetStatusRail()
    {
        StatusRail = new AshfallStatusRail();
        StatusRail.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        _contentStack.AddChild(StatusRail);
        return StatusRail;
    }

    /// <summary>
    /// Sets the content slot. Caller supplies any Node (VBoxContainer,
    /// ScrollContainer, etc.); it's parented and stretched to fill. This
    /// replaces any previously set content.
    /// </summary>
    public T SetContent<T>(T content) where T : Node
    {
        if (content == null) return null!;

        if (ContentRef != null && ContentRef.GetParent() == _contentStack)
            _contentStack.RemoveChild(ContentRef);

        _contentStack.AddChild(content);
        if (content is Control c)
        {
            c.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            c.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
        }
        ContentRef = content;
        return content;
    }

    /// <summary>
    /// Adds a custom button to the header bar (e.g. CLOSE [Esc]).
    /// </summary>
    public Button AttachHeaderCloseButton(string label, Action onPressed)
    {
        var btn = AshfallUiHelpers.MakeButton(label, onPressed);
        btn.CustomMinimumSize = new Vector2(110, 28);
        _headerHbox.AddChild(btn);
        return btn;
    }
}
