using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Radio;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.Economy;
using DesignTheme = Ashfall.Core.UI.Theme;

using Ashfall.Core.IO;
namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Caravan Barter Ledger (#35 Stitch).
///
/// Dashboard HYBRID wrapper around the existing TradeScreenGodotPanel.
/// The "ledger components" — trader card, two-column offer/ask table,
/// arbitrator, radio ticker — are already implemented inside
/// TradeScreenGodotPanel and would be regressed by any wholesale refactor.
/// This wrapper adds the Stitch dashboard chrome (sidebar nav for trade-flow
/// actions + status rail of faction stance / trust / repels counters) and
/// hosts TradeScreenGodotPanel inside the dashboard shell content slot.
///
/// The trade engine (EconomyHostSession / IFactionStanceProvider /
/// IPriceShockProvider / IFactionRadioProvider) remains the authoritative
/// source. The wrapper reads stance / trust / aggression / repels strictly
/// from existing session APIs to populate the status rail — no fake
/// metrics, no derived values not already exposed by the engine.
///
/// Sub-section nav (sidebar) lets the user jump between:
///   • Caravan context (faction profile, headline stance)
///   • Player offer column (open the existing player offer list)
///   • Faction stock column (open the existing faction asks)
///   • Arbitrator scale (open the existing fair-deal strip)
///
/// The existing "ACCEPT BARTER" / "DEMAND PARLEY" buttons still live inside
/// TradeScreenGodotPanel. This wrapper does not re-implement the wiring; it
/// only routes the user toward the right sub-section via sidebar selection.
/// </summary>
public partial class CaravanBarterLedgerPanel : Control
{
    public event Action? OnClose;
    public event Action<string>? OnSetActiveFaction;

    private AshfallDashboardShell _shell = null!;
    private AshfallSidebar? _sidebar;
    private AshfallStatusRail? _statusRail;
    private TradeScreenGodotPanel _tradeInner = null!;

    private EconomyHostSession? _session;
    private IFactionStanceProvider? _stance;
    private string _activeFactionId = "scavenger_camp";

    public bool IsBound => _tradeInner != null && _session != null;

    public void Bind(
        EconomyHostSession session,
        IFactionStanceProvider? stanceProvider = null,
        IPriceShockProvider? priceShockProvider = null,
        IFactionRadioProvider? radioProvider = null,
        ISeededRng? rng = null)
    {
        _session = session;
        _stance = stanceProvider;
        if (_tradeInner != null)
        {
            _tradeInner.BindSession(session, stanceProvider!, priceShockProvider!, radioProvider!, rng!);
        }
        RefreshView();
    }

    public void BindViewModel(ITradeScreenViewModel viewModel, ITradeIntentSink intentSink)
    {
        if (_tradeInner != null)
        {
            _tradeInner.BindViewModel(viewModel, intentSink);
        }
    }

    public void SetActiveFaction(string factionId)
    {
        _activeFactionId = factionId ?? "scavenger_camp";
        if (_tradeInner != null)
        {
            _tradeInner.SetActiveFaction(_activeFactionId);
        }
        RefreshView();
    }

    public void RefreshView()
    {
        if (_statusRail == null || _tradeInner == null) return;
        if (_session == null || _stance == null)
        {
            _statusRail.Set("faction",  "—",            AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("stance",   "—",            AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("trust",    "0",            AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("aggress",  "0.00",         AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("repels",   "0",            AshfallMetricCard.Criticality.Normal);
            return;
        }

        var stance = _stance.GetStance(_activeFactionId);
        float trust = _stance.GetEffectiveTrust(_activeFactionId);
        float aggression = _stance.GetRaidAggression(_activeFactionId);
        int consecutiveRepels = SafeGetConsecutiveRepels(_stance, _activeFactionId);

        string stanceLabel = stance.ToString().ToUpperInvariant();
        var stanceCrit = stance.ToString() switch
        {
            "Trade" => AshfallMetricCard.Criticality.Normal,
            "Rob" => AshfallMetricCard.Criticality.Warn,
            "HostileRaid" => AshfallMetricCard.Criticality.Critical,
            _ => AshfallMetricCard.Criticality.Caution,
        };

        var trustCrit = trust >= 50 ? AshfallMetricCard.Criticality.Normal
            : trust >= 0 ? AshfallMetricCard.Criticality.Caution
            : trust >= -25 ? AshfallMetricCard.Criticality.Warn
            : AshfallMetricCard.Criticality.Critical;

        _statusRail.Set("faction",  _activeFactionId.Replace('_', ' ').ToUpperInvariant(), AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("stance",   $"[{stanceLabel}]", stanceCrit);
        _statusRail.Set("trust",    trust > 0 ? $"+{trust:0}" : $"{trust:0}",       trustCrit);
        _statusRail.Set("aggress",  $"{aggression:0.00}",                            AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("repels",   $"{consecutiveRepels}",                          AshfallMetricCard.Criticality.Normal);
    }

    private static int SafeGetConsecutiveRepels(IFactionStanceProvider stance, string factionId)
    {
        try
        {
            // The stance engine exposes a negotiated-count accessor in HoldfastTradeSession,
            // but the IFactionStanceProvider abstraction does not. We probe via a known
            // property pattern, otherwise fall back to zero — never invent data.
            var t = stance.GetType();
            var prop = t.GetProperty("ConsecutiveRepels");
            if (prop != null)
            {
                var raw = prop.GetValue(stance);
                if (raw is int i) return i;
            }
            return 0;
        }
        catch (Exception ex_CATDIAG)
                                {
                                    CatalogDiagnostics.Warn("<unknown>", "unknown", ex_CATDIAG);
                                    return 0;
                                }
    }

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);
        Visible = false;

        var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.90f) };
        bg.SetAnchorsPreset(LayoutPreset.FullRect);
        AddChild(bg);

        var center = new CenterContainer();
        center.SetAnchorsPreset(LayoutPreset.FullRect);
        AddChild(center);

        _shell = new AshfallDashboardShell(
            "CARAVAN BARTER LEDGER — OPEN_TRADE_TABLE",
            1100, 720);
        center.AddChild(_shell);

        _sidebar = _shell.SetSidebar(new[]
        {
            new AshfallSidebar.Item { Id = "context",      Label = "Context",       Hint = "Faction profile" },
            new AshfallSidebar.Item { Id = "your_offers",  Label = "Your Offers",   Hint = "Player edge" },
            new AshfallSidebar.Item { Id = "their_asks",   Label = "Their Asks",    Hint = "Faction edge" },
            new AshfallSidebar.Item { Id = "fairness",     Label = "Fairness",      Hint = "DEAL IS FAIR indicator" },
            new AshfallSidebar.Item { Id = "biology",      Label = "Biology",       Hint = "Biological drawer" },
        }, "LEDGER OPS", "context");
        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("faction", "FACTION",   "—",        AshfallMetricCard.Criticality.Normal, 180);
        _statusRail.AddCard("stance",  "STANCE",    "—",        AshfallMetricCard.Criticality.Normal, 140);
        _statusRail.AddCard("trust",   "TRUST",     "0",        AshfallMetricCard.Criticality.Normal, 110);
        _statusRail.AddCard("aggress", "AGGRESSION","0.00",     AshfallMetricCard.Criticality.Normal, 130);
        _statusRail.AddCard("repels",  "REPELS",    "0",        AshfallMetricCard.Criticality.Normal, 110);

        _shell.AttachHeaderCloseButton("CLOSE [Esc]", () => OnClose?.Invoke());

        // TradeScreenGodotPanel builds its own internal UI when added to the tree.
        // We reparent it into the shell's content slot so the existing chrome
        // plots inside the dashboard frame.
        _tradeInner = new TradeScreenGodotPanel();
        _tradeInner.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        _tradeInner.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
        _shell.SetContent(_tradeInner);

        if (_session != null)
        {
            _tradeInner.BindSession(_session, _stance!);
        }

        // Sidebar nav highlights the relevant sub-section by changing
        // the active faction (default already at the top). Selecting
        // "Your Offers" / "Their Asks" / "Fairness" / "Biology" broadcasts
        // a hint to the host — the host can drive secondary UI if needed.
        // The actual arbitration/confirm action lives inside TradeScreen.
        if (_sidebar != null)
        {
            _sidebar.OnSelected += id =>
            {
                OnSetActiveFaction?.Invoke(id);
                RefreshView();
            };
        }
        RefreshView();
    }

    public void Open()
    {
        Visible = true;
        _tradeInner.Visible = true;
        _tradeInner.Open();
        RefreshView();
    }

    public void Close()
    {
        _tradeInner.Close();
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
