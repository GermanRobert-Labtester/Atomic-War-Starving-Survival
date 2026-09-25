// SPDX-License-Identifier: MIT
// ============================================================================
// UI Panel      : RecruitmentPanel
// Core System   : Ashfall.Core.Survivors.RecruitmentSystem
// Plan Reference: Plan 204 — Survivor Recruitment & Defection System
// Purpose       : Dedicated recruitment and defection command desk.
//                 Exposes active campaigns, discovered candidates, defection offers,
//                 and shelter roster admission with full controller/keyboard support.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core.Survivors;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI;

public partial class RecruitmentPanel : Control
{
    public event Action? OnClose;
    public event Action<string, string>? OnActionRequested;

    private AshfallDashboardShell _shell = null!;
    private AshfallStatusRail? _statusRail;
    private AshfallDataGrid? _grid;
    private VBoxContainer _detailBox = null!;
    private Label _detailTitle = null!;

    private int _selectedIndex = -1;
    private readonly List<string> _rowIds = new List<string>();

    private RecruitmentHostSession? _session;

    public bool IsBound => _session != null;

    public void Bind(RecruitmentHostSession session)
    {
        _session = session;
        RefreshView();
    }

    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        _shell = new AshfallDashboardShell("Survivor Recruitment & Defection Desk // Operations", minWidth: 1000, minHeight: 660);
        AddChild(_shell);
        _shell.SetAnchorsPreset(LayoutPreset.FullRect);
        _shell.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        _shell.SizeFlagsVertical = SizeFlags.ExpandFill;

        _statusRail = _shell.SetStatusRail();
        _statusRail.AddCard("active_campaigns", "Active Campaigns", "0", AshfallMetricCard.Criticality.Normal, minWidth: 130);
        _statusRail.AddCard("known_candidates", "Discovered", "0", AshfallMetricCard.Criticality.Normal, minWidth: 110);
        _statusRail.AddCard("pending_offers", "Pending Offers", "0", AshfallMetricCard.Criticality.Caution, minWidth: 120);
        _statusRail.AddCard("total_recruited", "Recruited", "0", AshfallMetricCard.Criticality.Normal, minWidth: 100);

        var cols = new[]
        {
            new AshfallDataGrid.Column { Header = "Candidate / Campaign", MinWidth = 180, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Type / Role",          MinWidth = 130, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Faction / Loc",       MinWidth = 120, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Willingness / Days",  MinWidth = 120, Alignment = AshfallDataGrid.ColumnAlign.Left },
            new AshfallDataGrid.Column { Header = "Status",              MinWidth = 100, Alignment = AshfallDataGrid.ColumnAlign.Left },
        };
        _grid = new AshfallDataGrid(cols, showHeader: true, minWidth: 640, minHeight: 300);
        _grid.OnRowSelected += idx => { _selectedIndex = idx; RefreshDetail(); };

        var body = new HBoxContainer();
        body.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
        body.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        body.SizeFlagsVertical = SizeFlags.ExpandFill;
        body.AddChild(_grid);

        _detailBox = new VBoxContainer();
        _detailBox.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
        _detailBox.CustomMinimumSize = new Vector2(330, 300);
        _detailBox.SizeFlagsVertical = SizeFlags.ExpandFill;
        body.AddChild(_detailBox);

        _shell.SetContent(body);
        RefreshView();
    }

    public void RefreshView()
    {
        RefreshStatusRail();
        BuildGridRows();
        RefreshDetail();
    }

    private void RefreshStatusRail()
    {
        if (_statusRail == null || _session == null) return;
        var census = _session.GetCensus();
        _statusRail.Set("active_campaigns", census.ActiveCampaigns.ToString(), AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("known_candidates", census.KnownCandidates.ToString(), AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("pending_offers", census.PendingOffers.ToString(), census.PendingOffers > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
        _statusRail.Set("total_recruited", census.TotalRecruited.ToString(), AshfallMetricCard.Criticality.Normal);
    }

    private void BuildGridRows()
    {
        if (_grid == null) return;
        _rowIds.Clear();
        var rows = new List<AshfallDataGrid.Row>();

        if (_session != null)
        {
            // First list active campaigns
            foreach (var campaign in _session.GetActiveCampaigns())
            {
                _rowIds.Add($"camp_{campaign.CampaignId}");
                rows.Add(new AshfallDataGrid.Row
                {
                    Cells = new List<AshfallDataGrid.Cell>
                    {
                        new($"Campaign: {campaign.CampaignTypeId}", AshfallDataGrid.CellState.Normal),
                        new("Campaign", AshfallDataGrid.CellState.Normal),
                        new(campaign.TargetFaction, AshfallDataGrid.CellState.Normal),
                        new($"Duration: {campaign.DurationDays}d", AshfallDataGrid.CellState.Normal),
                        new(campaign.Status.ToUpperInvariant(), AshfallDataGrid.CellState.Normal)
                    },
                    Selectable = true
                });
            }

            // Next list known candidates
            foreach (var candidate in _session.GetKnownCandidates())
            {
                _rowIds.Add($"cand_{candidate.CandidateId}");
                rows.Add(new AshfallDataGrid.Row
                {
                    Cells = new List<AshfallDataGrid.Cell>
                    {
                        new(candidate.CandidateId, AshfallDataGrid.CellState.Normal),
                        new(candidate.CandidateType, AshfallDataGrid.CellState.Normal),
                        new(string.IsNullOrEmpty(candidate.CurrentFaction) ? candidate.LocationId : candidate.CurrentFaction, AshfallDataGrid.CellState.Normal),
                        new($"{candidate.Willingness}%", AshfallDataGrid.CellState.Normal),
                        new(candidate.IsRecruited ? "RECRUITED" : "AVAILABLE", candidate.IsRecruited ? AshfallDataGrid.CellState.Positive : AshfallDataGrid.CellState.Normal)
                    },
                    Selectable = true
                });
            }
        }

        _grid.SetRows(rows);
    }

    private void RefreshDetail()
    {
        if (_detailBox == null) return;
        foreach (Node child in _detailBox.GetChildren())
            child.QueueFree();

        _detailTitle = AshfallUiHelpers.MakeSectionHeader("RECRUITMENT OPERATIONS");
        _detailBox.AddChild(_detailTitle);

        if (_selectedIndex < 0 || _selectedIndex >= _rowIds.Count || _session == null)
        {
            _detailBox.AddChild(AshfallUiHelpers.MakeMetadata("Select a candidate or campaign from the registry to review operations."));

            var launchBtn = AshfallUiHelpers.MakeButton("LAUNCH SEARCH PARTY", () =>
            {
                OnActionRequested?.Invoke("START_CAMPAIGN", "active_recruitment");
            });
            launchBtn.CustomMinimumSize = new Vector2(180, 32);
            _detailBox.AddChild(launchBtn);
            return;
        }

        string selectedKey = _rowIds[_selectedIndex];
        if (selectedKey.StartsWith("camp_"))
        {
            string campId = selectedKey.Substring(5);
            var campaign = _session.GetActiveCampaigns().FirstOrDefault(c => c.CampaignId == campId);
            if (campaign != null)
            {
                _detailBox.AddChild(AshfallUiHelpers.MakeBody($"Campaign ID: {campaign.CampaignId}"));
                _detailBox.AddChild(AshfallUiHelpers.MakeBody($"Type: {campaign.CampaignTypeId}"));
                _detailBox.AddChild(AshfallUiHelpers.MakeBody($"Target: {campaign.TargetFaction}"));
                _detailBox.AddChild(AshfallUiHelpers.MakeBody($"Duration: {campaign.DurationDays} days (Started Day {campaign.StartedDay})"));
                _detailBox.AddChild(AshfallUiHelpers.MakeBody($"Success Chance: {campaign.SuccessChance}%"));
            }
        }
        else if (selectedKey.StartsWith("cand_"))
        {
            string candId = selectedKey.Substring(5);
            var candidate = _session.GetKnownCandidates().FirstOrDefault(c => c.CandidateId == candId);
            if (candidate != null)
            {
                _detailBox.AddChild(AshfallUiHelpers.MakeBody($"Candidate: {candidate.CandidateId}"));
                _detailBox.AddChild(AshfallUiHelpers.MakeBody($"Type: {candidate.CandidateType}"));
                _detailBox.AddChild(AshfallUiHelpers.MakeBody($"Template: {candidate.CandidateTemplateId}"));
                _detailBox.AddChild(AshfallUiHelpers.MakeBody($"Willingness: {candidate.Willingness}%"));
                _detailBox.AddChild(AshfallUiHelpers.MakeBody($"Status: {(candidate.IsRecruited ? "Recruited" : "Available")}"));

                var btnBox = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);

                if (!candidate.IsRecruited)
                {
                    var admitBtn = AshfallUiHelpers.MakeButton("ADMIT TO ROSTER", () =>
                    {
                        OnActionRequested?.Invoke("ADMIT_CANDIDATE", candidate.CandidateId);
                    });
                    admitBtn.CustomMinimumSize = new Vector2(140, 30);
                    btnBox.AddChild(admitBtn);

                    var offerBtn = AshfallUiHelpers.MakeButton("DEFECTION OFFER", () =>
                    {
                        OnActionRequested?.Invoke("MAKE_OFFER", candidate.CandidateId);
                    });
                    offerBtn.CustomMinimumSize = new Vector2(140, 30);
                    btnBox.AddChild(offerBtn);
                }

                _detailBox.AddChild(btnBox);
            }
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
        if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(this))
            Visible = false;
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

    public void Unbind()
    {
        _session = null;
        RefreshView();
    }

    public override void _ExitTree()
    {
        Unbind();
        base._ExitTree();
    }
}
