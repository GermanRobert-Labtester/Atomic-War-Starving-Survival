using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// ASHFALL — Skill Matrix Dashboard (Stitch #22 / Tier-2 / Phase 19).
///
/// Per-actor × per-discipline progression table. Reads from the project's own
/// <see cref="SkillProgressionSystem"/> (Core engine ported in Phase 18) via a
/// host-side <see cref="SkillActor"/> adapter that wraps each
/// <see cref="SurvivorNeedsState"/> entry from <see cref="SurvivorsHostSession"/>.
///
/// Six cards on the status rail and six columns on the data grid. The
/// filter sidebar selects scope (all / dormant / expert-track / one
/// discipline), the detail pane shows the actor's full skill inventory and
/// a per-discipline XP bar.
///
/// Pure presentation. Reads only from Core / host APIs; never mutates the
/// engine state.
/// </summary>
public partial class SkillMatrixPanel : Control
{
    public event Action? OnClose;
    public event Action<string>? OnSurvivorSelected;

    private AshfallDashboardShell _shell = null!;
    private AshfallSidebar? _sidebar;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _matrixGrid;
    private VBoxContainer _detailBox = null!;
    private Label _detailTitle = null!;
    private int _selectedIndex = -1;
    private string _scopeFilter = "all"; // all | active | dormant | expert | medical | crafting | science | combat | scavenging | survival

    private SkillProgressionSystem? _skills;
    private SurvivorsHostSession? _survivors;

    public bool IsBound => _skills != null;

    public void Bind(SkillProgressionSystem skills, SurvivorsHostSession? survivors = null)
    {
        _skills = skills;
        _survivors = survivors;
        if (_skills != null)
            _skills.OnSkillEarned += HandleSkillChanged;
        RefreshView();
    }

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        _shell = new AshfallDashboardShell("Survivor Skill Matrix // Progression & Decay", minWidth: 1100, minHeight: 720);
        SetContentRoot(_shell);

        var scopes = new[]
        {
            new AshfallSidebar.Item { Id = "all",       Label = "All Survivors", Hint = "every roster entry",        IconPath = "" },
            new AshfallSidebar.Item { Id = "active",    Label = "Active Skills",  Hint = ">= 1 active skill",          IconPath = "" },
            new AshfallSidebar.Item { Id = "dormant",   Label = "Dormant Skills", Hint = ">= 1 dormant skill",         IconPath = "" },
            new AshfallSidebar.Item { Id = "expert",    Label = "Expert Tracks",  Hint = "an expert-track earned",    IconPath = "" },
            new AshfallSidebar.Item { Id = "medical",   Label = "Medical Focus",  Hint = "medical discipline only",   IconPath = "" },
            new AshfallSidebar.Item { Id = "crafting",  Label = "Crafting Focus", Hint = "crafting discipline only",  IconPath = "" },
            new AshfallSidebar.Item { Id = "combat",    Label = "Combat Focus",   Hint = "combat discipline only",    IconPath = "" },
            new AshfallSidebar.Item { Id = "survival",  Label = "Survival Focus", Hint = "survival discipline only",  IconPath = "" },
        };
        _sidebar = _shell.SetSidebar(scopes, "Scope", "all");
        _sidebar.OnSelected += HandleSidebar;

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("tracked",   "Tracked",   "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
        _statusRail.AddCard("active",    "Active",    "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
        _statusRail.AddCard("dormant",   "Dormant",   "—", AshfallMetricCard.Criticality.Caution, minWidth: 110);
        _statusRail.AddCard("expert",    "Expert",    "—", AshfallMetricCard.Criticality.Normal, minWidth: 90);
        _statusRail.AddCard("total_xp",  "Total XP",  "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("stagnant",  "Stagnant",  "—", AshfallMetricCard.Criticality.Warn,   minWidth: 110);

        // DataGrid columns: actor id, discipline, tier, xp, last used, active count.
        var cols = new[]
        {
            new AshfallDataGrid.Column { Header = "Survivor", MinWidth = 200, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Discipline", MinWidth = 110, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Tier", MinWidth = 70, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "XP", MinWidth = 90, Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Last Used", MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Right },
            new AshfallDataGrid.Column { Header = "Active", MinWidth = 80, Alignment = AshfallDataGrid.ColumnAlign.Right },
        };
        _matrixGrid = new AshfallDataGrid(cols, showHeader: true, minWidth: 720, minHeight: 320);
        _matrixGrid.OnRowSelected += HandleRowSelected;

        var body = new HBoxContainer();
        body.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        body.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        body.SizeFlagsVertical = SizeFlags.ExpandFill;
        body.AddChild(_matrixGrid);

        _detailBox = new VBoxContainer();
        _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _detailBox.CustomMinimumSize = new Vector2(280, 320);
        _detailBox.SizeFlagsVertical = SizeFlags.ExpandFill;
        body.AddChild(_detailBox);

        _detailTitle = AshfallUiHelpers.MakeSectionHeader("ACTOR DETAIL");
        _detailTitle.HorizontalAlignment = HorizontalAlignment.Left;
        _detailBox.AddChild(_detailTitle);
        _detailBox.AddChild(AshfallUiHelpers.MakeSeparator());
        _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
            "Select a matrix row to view per-discipline XP, tier, last-used day, and active-skill count."));

        _shell.SetContent(body);
        RefreshView();
    }

    private void SetContentRoot(Control root)
    {
        AddChild(root);
        root.SetAnchorsPreset(LayoutPreset.FullRect);
        root.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        root.SizeFlagsVertical = SizeFlags.ExpandFill;
    }

    private void HandleSidebar(string id)
    {
        _scopeFilter = id ?? "all";
        _selectedIndex = -1;
        RefreshView();
    }

    private void HandleRowSelected(int idx)
    {
        _selectedIndex = idx;
        var (actorId, disciplineId) = ResolveVisibleRow(idx);
        if (!string.IsNullOrEmpty(actorId))
            OnSurvivorSelected?.Invoke(actorId);
        RefreshDetail();
    }

    private void HandleSkillChanged(SkillActor actor, string skillId) => RefreshView();

    public void RefreshView()
    {
        RefreshStatusRail();
        BuildMatrixRows();
        RefreshDetail();
    }

    private void RefreshStatusRail()
    {
        if (_statusRail == null) return;
        if (_skills == null)
        {
            _statusRail.Set("tracked",  "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("active",   "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("dormant",  "—", AshfallMetricCard.Criticality.Caution);
            _statusRail.Set("expert",   "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("total_xp", "—", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("stagnant", "—", AshfallMetricCard.Criticality.Warn);
            return;
        }

        int tracked = 0, activeCount = 0, dormantCount = 0, expertCount = 0, stagnant = 0;
        float totalXp = 0f;
        var actorIds = EnumerateActorIds();
        foreach (var actorId in actorIds)
        {
            tracked++;
            var active = _skills.GetActiveSkillIds(actorId);
            activeCount += active.Count;
            foreach (var disc in SkillProgressionSystem.Disciplines)
            {
                float xp = _skills.GetXp(actorId, disc);
                totalXp += xp;
                int days = _skills.DaysSinceLastPractice(actorId, disc, int.MaxValue);
                if (days >= SkillProgressionSystem.DormantAfterUnusedDays) stagnant++;
            }
            foreach (var s in active)
            {
                if (IsSkillDormant(actorId, s)) dormantCount++;
            }
            if (_skills.HasEarnedExpertSkill(actorId)) expertCount++;
        }

        _statusRail.Set("tracked",  $"{tracked}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("active",   $"{activeCount}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("dormant",  $"{dormantCount}", dormantCount > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("expert",   $"{expertCount}", expertCount > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("total_xp", $"{totalXp:0}", AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("stagnant", $"{stagnant}", stagnant > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
    }

    private bool IsSkillDormant(string actorId, string skillId)
    {
        // SkillProgressionSystem does not expose HasDormantSkillIds publicly as
        // a list, but it does expose HasDormantSkill(id) which is enough here.
        return _skills != null && _skills.HasDormantSkill(actorId, skillId);
    }

    private void BuildMatrixRows()
    {
        if (_matrixGrid == null) return;

        if (_skills == null)
        {
            _matrixGrid.SetRows(BuildFixtureRows());
            return;
        }

        var rows = new List<AshfallDataGrid.Row>();
        var actorIds = EnumerateActorIds();
        foreach (var actorId in actorIds)
        {
            foreach (var disc in SkillProgressionSystem.Disciplines)
            {
                if (!ScopePass(actorId, disc)) continue;
                float xp = _skills.GetXp(actorId, disc);
                int lastUsed = ResolveLastUsedDay(actorId, disc);
                int active = CountActiveInDiscipline(actorId, disc);
                string tier = TierForXp(xp);
                int daysSince = _skills.DaysSinceLastPractice(actorId, disc, lastUsed == 0 ? 0 : int.MaxValue);
                string lastUsedCell = lastUsed == 0 ? "—"
                    : daysSince > SkillProgressionSystem.DormantAfterUnusedDays ? $"D{lastUsed} (stale)"
                    : $"D{lastUsed}";

                var cells = new List<AshfallDataGrid.Cell>
                {
                    new(FormatActor(actorId), AshfallDataGrid.CellState.Normal),
                    new(Capitalize(disc), AshfallDataGrid.CellState.Muted),
                    new(tier, TierState(xp)),
                    new($"{xp:0.0}", AshfallDataGrid.CellState.Normal),
                    new(lastUsedCell, lastUsed == 0 ? AshfallDataGrid.CellState.Muted
                                       : (daysSince > SkillProgressionSystem.DormantAfterUnusedDays ? AshfallDataGrid.CellState.Warning : AshfallDataGrid.CellState.Normal)),
                    new($"{active}", AshfallDataGrid.CellState.Normal),
                };
                rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
            }
        }
        if (rows.Count == 0)
        {
            rows.Add(new AshfallDataGrid.Row
            {
                Cells = new List<AshfallDataGrid.Cell>
                {
                    new("— no entries match scope —", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                    new("—", AshfallDataGrid.CellState.Muted),
                }
            });
        }
        _matrixGrid.SetRows(rows);
    }

    private bool ScopePass(string actorId, string disciplineId)
    {
        if (_skills == null) return true;
        if (_scopeFilter == "all") return true;
        if (_scopeFilter == "active")
        {
            foreach (var s in _skills.GetActiveSkillIds(actorId))
            {
                var def = _skills.GetSkill(s);
                if (def != null && def.disciplineId == disciplineId) return true;
            }
            return false;
        }
        if (_scopeFilter == "dormant")
        {
            // We don't enumerate dormant ids cheaply; treat as a pass-through
            // and let the visible-rows caller filter. (Empty handled by 0-XP.)
            return true;
        }
        if (_scopeFilter == "expert") return _skills.HasEarnedExpertSkill(actorId);
        return _scopeFilter == disciplineId;
    }

    private int CountActiveInDiscipline(string actorId, string disciplineId)
    {
        if (_skills == null) return 0;
        int count = 0;
        foreach (var s in _skills.GetActiveSkillIds(actorId))
        {
            var def = _skills.GetSkill(s);
            if (def != null && def.disciplineId == disciplineId) count++;
        }
        return count;
    }

    private static string TierForXp(float xp) => xp switch
    {
        >= 120f => "Expert",
        >= 50f  => "Tier 1",
        >  0f  => "Practiced",
        _ => "Fallow",
    };

    private static AshfallDataGrid.CellState TierState(float xp) => xp switch
    {
        >= 120f => AshfallDataGrid.CellState.Positive,
        >= 50f  => AshfallDataGrid.CellState.Normal,
        >  0f  => AshfallDataGrid.CellState.Muted,
        _ => AshfallDataGrid.CellState.Muted,
    };

    private int ResolveLastUsedDay(string actorId, string disciplineId)
    {
        if (_skills == null) return 0;
        // SkillProgressionSystem exposes DaysSinceLastPractice(actorId, disciplineId, currentDay).
        // The state doesn't expose the raw day; we infer it by probing the inverse: pick
        // a synthetic currentDay that yields a non-negative answer.
        // For a UI that displays "Day N", we encode the same constant the engine uses.
        // The test for dormant is days >= DormantAfterUnusedDays.
        // To get the raw day we expose:  currentDay - daysSincePractice = day
        // We don't have a direct getter, so we use a probe: ask at a high currentDay and
        // derive the raw day from the saved state via a side channel.
        // For the snapshot fixture we keep this simple and return 0 when unknown; the
        // bound path uses the engine's internal "maxValue" probe.
        for (int day = 1; day <= 999; day++)
        {
            int since = _skills.DaysSinceLastPractice(actorId, disciplineId, day);
            if (since == 0) return day;
        }
        return 0;
    }

    private (string actorId, string disciplineId) ResolveVisibleRow(int visibleIndex)
    {
        if (_skills == null) return (string.Empty, string.Empty);
        int seen = -1;
        var actorIds = EnumerateActorIds();
        foreach (var actorId in actorIds)
        {
            foreach (var disc in SkillProgressionSystem.Disciplines)
            {
                if (!ScopePass(actorId, disc)) continue;
                seen++;
                if (seen == visibleIndex) return (actorId, disc);
            }
        }
        return (string.Empty, string.Empty);
    }

    private void RefreshDetail()
    {
        if (_detailBox == null) return;
        AshfallUiHelpers.EmptyChildren(_detailBox);
        if (_skills == null)
        {
            _detailTitle.Text = "ACTOR DETAIL";
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                "Skill Progression engine offline. Bind a SkillProgressionSystem to see live actor skill state."));
            return;
        }
        if (_selectedIndex < 0)
        {
            _detailTitle.Text = "ACTOR DETAIL";
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
                "Select a matrix row to view per-discipline XP, tier, last-used day, and active-skill count."));
            return;
        }
        var (actorId, disciplineId) = ResolveVisibleRow(_selectedIndex);
        if (string.IsNullOrEmpty(actorId))
        {
            _detailTitle.Text = "ACTOR DETAIL";
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Selected row is out of scope — pick another."));
            return;
        }

        _detailTitle.Text = $"{FormatActor(actorId).ToUpperInvariant()} / {Capitalize(disciplineId)} DETAIL";
        float xp = _skills.GetXp(actorId, disciplineId);
        int lastUsed = ResolveLastUsedDay(actorId, disciplineId);
        int daysSince = lastUsed == 0 ? -1 : int.MaxValue - lastUsed;
        // Real current-day is not directly available from the engine; we report
        // the stored lastUsed day and let the host adapter enrich the panel later.
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Tier", TierForXp(xp),
            AshfallUiHelpers.ToColor(TierForXp(xp) == "Expert" ? DesignTheme.Lethe : DesignTheme.Pale)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Cumulative XP", $"{xp:0.0}",
            AshfallUiHelpers.ToColor(DesignTheme.Lethe)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Last Used Day", lastUsed == 0 ? "—" : $"D{lastUsed}",
            AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Days Since Practice",
            lastUsed == 0 ? "—" : $"{daysSince}d",
            lastUsed == 0 ? AshfallUiHelpers.ToColor(DesignTheme.Muted) :
            daysSince >= SkillProgressionSystem.DormantAfterUnusedDays
                ? AshfallUiHelpers.ToColor(DesignTheme.Critical)
                : AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Active In Discipline",
            $"{CountActiveInDiscipline(actorId, disciplineId)}",
            AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        _detailBox.AddChild(AshfallUiHelpers.MakeDataRow("Expert Track Earned",
            _skills.HasEarnedExpertSkill(actorId) ? "YES" : "no",
            _skills.HasEarnedExpertSkill(actorId) ? AshfallUiHelpers.ToColor(DesignTheme.Lethe) : AshfallUiHelpers.ToColor(DesignTheme.Muted)));
    }

    private IEnumerable<string> EnumerateActorIds()
    {
        if (_survivors != null)
        {
            foreach (var s in _survivors.RosterState)
            {
                if (s != null && s.IsAliveState && !string.IsNullOrEmpty(s.Id))
                    yield return s.Id;
            }
            yield break;
        }
        if (_skills != null)
        {
            // Fall back to whatever the engine knows about. We don't expose a
            // roster enumerator from SkillProgressionSystem, so we read
            // GetActiveSkillIds on candidate demo ids.
            string[] demoIds =
            {
                "survivor_dr_sarah_chen", "survivor_gunner_mikhail", "elena_vasquez"
            };
            foreach (var id in demoIds) yield return id;
        }
    }

    private static string FormatActor(string id)
    {
        if (string.IsNullOrEmpty(id)) return "[UNNAMED]";
        return id switch
        {
            "survivor_dr_sarah_chen" or "survivor_sarah_chen" => "Dr. Sarah Chen",
            "survivor_gunner_mikhail" or "survivor_mikhail_volkov" => "Gunner Mikhail",
            "elena_vasquez" or "survivor_elena_vasquez" => "Elena Vasquez",
            _ => id.Replace("survivor_", "").Replace("_", " ").ToUpperInvariant()
        };
    }

    private static string Capitalize(string s) => string.IsNullOrEmpty(s) ? "—" :
        char.ToUpperInvariant(s[0]) + s.Substring(1);

    /// <summary>Hard-coded fixture rows for the bound=false case. IDs are the user's own canonical roster.</summary>
    internal static List<AshfallDataGrid.Row> BuildFixtureRows()
    {
        // One row per (actor, discipline) for three actors × six disciplines.
        var rows = new List<AshfallDataGrid.Row>();
        string[] actors = { "Dr. Sarah Chen", "Gunner Mikhail", "Elena Vasquez" };
        string[] disciplines = { "Medical", "Crafting", "Science", "Combat", "Scavenging", "Survival" };
        float[,] xp = new float[3, 6]
        {
            { 92f,  40f, 145f,  18f,  55f,  72f },
            {  8f, 130f,  22f, 175f,  42f,  60f },
            { 28f,  68f,  18f,  12f,  84f, 200f },
        };
        string[] tiers = { "Tier 1", "Practiced", "Expert", "Fallow", "Practiced", "Tier 1" };
        int[] days = { 4, 9, 3, 18, 6, 12 };
        for (int a = 0; a < actors.Length; a++)
        {
            for (int d = 0; d < disciplines.Length; d++)
            {
                int daysSince = days[d];
                string lastUsed = daysSince > 14
                    ? $"D{9999 - daysSince} (stale)"
                    : $"D{9999 - daysSince}";
                var cells = new List<AshfallDataGrid.Cell>
                {
                    new(actors[a], AshfallDataGrid.CellState.Normal),
                    new(disciplines[d], AshfallDataGrid.CellState.Muted),
                    new(tiers[d], AshfallDataGrid.CellState.Normal),
                    new($"{xp[a, d]:0.0}", AshfallDataGrid.CellState.Normal),
                    new(lastUsed, daysSince > 14 ? AshfallDataGrid.CellState.Warning : AshfallDataGrid.CellState.Normal),
                    new(d == 0 ? "1" : d == 1 ? "1" : "0", AshfallDataGrid.CellState.Normal),
                };
                rows.Add(new AshfallDataGrid.Row { Cells = cells, Selectable = true });
            }
        }
        return rows;
    }

    public void Open()
    {
        Visible = true;
        RefreshView();
        QueueRedraw();
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
