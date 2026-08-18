using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Verdict Dashboard (#15 Stitch).
/// Hosts the existing VerdictPanel inside the dashboard shell so the screen
/// gains the Phase-12 sidebar + status rail chrome without rewriting the
/// bespoke per-record interaction (NPC "hear" buttons, log list, evidence list).
///
/// Narrative verdict content is deliberately NOT converted into a DataGrid —
/// per the brief, "Do not convert narrative verdict content into a
/// spreadsheet merely for architectural consistency."
///
/// Reads headline metrics from the bound VerdictHostSession directly; the
/// inner VerdictPanel is mounted as the content slot.
/// </summary>
public partial class VerdictDashboardPanel : Control
{
    public event Action? OnClose;
    public event Action<string>? OnFactionDetailRequested;

    private AshfallDashboardShell _shell = null!;
    private AshfallStatusRail? _statusRail;
    private VerdictPanel? _verdictInner;
    private VerdictHostSession? _session;

    public bool IsBound => _session != null;

    public void Bind(VerdictPanel verdict, VerdictHostSession session)
    {
        _verdictInner = verdict;
        _session = session;
        _verdictInner.Bind(session);
        MountInner();
        RefreshView();
    }

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);
        Visible = false;

        var bg = new ColorRect { Color = new Color(0.04f, 0.04f, 0.05f, 0.95f) };
        bg.SetAnchorsPreset(LayoutPreset.FullRect);
        AddChild(bg);

        _shell = new AshfallDashboardShell(
            "VERDICT — THE MACHINE'S REGISTER",
            1100, 720);

        var hostContainer = new MarginContainer();
        hostContainer.AddThemeConstantOverride("margin_left", DesignTheme.HudEdge);
        hostContainer.AddThemeConstantOverride("margin_top", DesignTheme.SpacingLg);
        hostContainer.AddThemeConstantOverride("margin_right", DesignTheme.HudEdge);
        hostContainer.AddThemeConstantOverride("margin_bottom", DesignTheme.SpacingMd);
        hostContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        hostContainer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
        hostContainer.AddChild(_shell);
        AddChild(hostContainer);

        _shell.SetSidebar(new[]
        {
            new AshfallSidebar.Item { Id = "phase",     Label = "Phase",          Hint = "reckoning state" },
            new AshfallSidebar.Item { Id = "figures",   Label = "Figures",        Hint = "of the record" },
            new AshfallSidebar.Item { Id = "places",    Label = "Places",         Hint = "& evidence" },
            new AshfallSidebar.Item { Id = "transmits", Label = "Transmissions",   Hint = "radio log" },
        }, "VERDICT OPS", "phase");

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("phase",     "PHASE",    "—", AshfallMetricCard.Criticality.Normal, 130);
        _statusRail.AddCard("evidence",  "EVIDENCE", "0", AshfallMetricCard.Criticality.Normal, 110);
        _statusRail.AddCard("call",      "CALL",     "—", AshfallMetricCard.Criticality.Normal, 110);
        _statusRail.AddCard("figs",      "FIGURES",  "0", AshfallMetricCard.Criticality.Normal, 100);
        _statusRail.AddCard("places",    "PLACES",   "0", AshfallMetricCard.Criticality.Normal, 100);
        _statusRail.AddCard("transmits", "TRANSMITS","0", AshfallMetricCard.Criticality.Normal, 120);

        _shell.AttachHeaderCloseButton("RETURN TO EXPANSION HUB [Esc]", () => OnClose?.Invoke());
    }

    private void MountInner()
    {
        if (_verdictInner == null || _shell == null) return;
        _verdictInner.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        _verdictInner.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
        _shell.SetContent(_verdictInner);
    }

    public void RefreshView()
    {
        if (_statusRail == null) return;
        if (_session == null)
        {
            _statusRail.Set("phase",    "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("evidence", "0", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("call",     "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("figs",     "0", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("places",   "0", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("transmits","0", AshfallMetricCard.Criticality.Normal);
            return;
        }
        try
        {
            int evidenceCount = _session.Evidence?.Count ?? 0;
            int readCount = _session.MachineLog?.ReadCount() ?? 0;
            int readTotal = _session.MachineLog?.Entries?.Count ?? 0;
            string phaseName = _session.Reckoning?.Phase.ToString() ?? "—";
            bool callResolved = _session.Reckoning?.State.callResolved ?? false;
            int figs = _session.AvailableNpcs()?.Count ?? 0;
            int places = _session.Locations?.Count ?? 0;
            int transmits = _session.RadioEntries?.Count ?? 0;

            var phaseInt = (int)_session.Reckoning.Phase;
            var phaseCrit = phaseInt >= 3 ? AshfallMetricCard.Criticality.Critical
                : phaseInt >= 2 ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Normal;

            _statusRail.Set("phase",    phaseName.ToUpperInvariant(), phaseCrit);
            _statusRail.Set("evidence", $"{evidenceCount}", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("call",     callResolved ? "RESOLVED" : "OPEN",
                callResolved ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Caution);
            _statusRail.Set("figs",     $"{figs}",     AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("places",   $"{places}",   AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("transmits",$"{transmits}",AshfallMetricCard.Criticality.Normal);
        }
        catch
        {
            // bound session with quirky state should not break the dashboard
            _statusRail.Set("phase", "—", AshfallMetricCard.Criticality.Normal);
        }
    }

    public void Open()
    {
        Visible = true;
        _verdictInner?.Open();
        RefreshView();
    }

    public void Close()
    {
        Visible = false;
        OnClose?.Invoke();
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
