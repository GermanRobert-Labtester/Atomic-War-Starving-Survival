// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Narrative;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 200: Wasteland festivals and ceremonies workflow.
    /// Presentation-only — every mutation is emitted via
    /// <see cref="OnActionRequested"/> and performed by the host through the
    /// bound <see cref="CeremonySystem"/> plus the canonical inventory and
    /// faction-standing authorities. State → blocker → cost → consequence.
    /// </summary>
    public partial class CeremonyFestivalPanel : Control, IBindablePanel
    {
        public event Action? OnClose;
        public event Action<string, string>? OnActionRequested;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private CeremonySystem? _system;
        private ItemList _ceremonyList = null!;
        private VBoxContainer _detail = null!;
        private int _selectedCeremonyIndex = -1;
        private string _feedbackText = string.Empty;
        private bool _feedbackIsFailure;

        public bool IsBound => _system != null;

        public void Bind(CeremonySystem system) { _system = system; _feedbackText = string.Empty; RefreshView(); }
        public void Unbind() { _system = null; }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("COMMON HEARTH // WASTELAND FESTIVALS", minWidth: 1000, minHeight: 650);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("phase", "Preparation", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("prep", "Days Remaining", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
            _statusRail.AddCard("truce", "Truce Days", "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
            _statusRail.AddCard("held", "Ceremonies Held", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);

            _ceremonyList = new ItemList
            {
                CustomMinimumSize = new Vector2(280, 0),
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.Fill
            };
            _ceremonyList.ItemSelected += OnCeremonySelected;

            _detail = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);

            var bodyRow = new HBoxContainer();
            bodyRow.AddChild(_ceremonyList);

            var detailScroll = new ScrollContainer
            {
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            detailScroll.AddChild(_detail);
            bodyRow.AddChild(detailScroll);

            _shell.SetContent(bodyRow);
            _shell.AttachHeaderCloseButton("CLOSE", () => OnClose?.Invoke());
            AddChild(_shell);
            Visible = false;
        }

        public void Open() { Visible = true; RefreshView(); }
        public void Close() { Visible = false; OnClose?.Invoke(); }

        private void OnCeremonySelected(long index)
        {
            _selectedCeremonyIndex = (int)index;
            RefreshView();
        }

        /// <summary>Last feedback line rendered by the panel (test/diagnostic surface).</summary>
        public string LastFeedback { get; private set; } = string.Empty;

        /// <summary>Host feedback strip — tied to the actual command result.</summary>
        public void ShowFeedback(string message, bool isFailure)
        {
            _feedbackText = message;
            _feedbackIsFailure = isFailure;
            LastFeedback = message;
            RefreshView();
        }

        private CeremonyDefinition? SelectedDefinition()
        {
            if (_system == null) return null;
            int idx = _selectedCeremonyIndex;
            var defs = new List<CeremonyDefinition>(_system.CeremonyCatalog.Values);
            defs.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
            if (idx < 0 || idx >= defs.Count) return null;
            return defs[idx];
        }

        public void RefreshView()
        {
            if (_system == null || _detail == null || _ceremonyList == null) return;
            AshfallUiHelpers.EmptyChildren(_detail);

            var active = _system.ActiveCeremony;

            // ── Status rail ──
            if (_statusRail != null)
            {
                if (active != null)
                {
                    bool preparing = active.Phase == CeremonyPhase.Preparing;
                    _statusRail.Set("phase", PhaseText(active.Phase),
                        active.Phase == CeremonyPhase.Failed ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
                    _statusRail.Set("prep", preparing ? $"{active.PreparationDaysRemaining} d" : "—", AshfallMetricCard.Criticality.Normal);
                    _statusRail.Set("truce", active.ActiveTruceDaysRemaining > 0 ? $"{active.ActiveTruceDaysRemaining} d" : "none",
                        active.ActiveTruceDaysRemaining > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Normal);
                }
                else
                {
                    _statusRail.Set("phase", "NONE", AshfallMetricCard.Criticality.Normal);
                    _statusRail.Set("prep", "—", AshfallMetricCard.Criticality.Normal);
                    _statusRail.Set("truce", "none", AshfallMetricCard.Criticality.Normal);
                }
                _statusRail.Set("held", _system.State.TotalCeremoniesHeld.ToString(), AshfallMetricCard.Criticality.Normal);
            }

            // ── Ceremony catalog list ──
            _ceremonyList.Clear();
            var defs = new List<CeremonyDefinition>(_system.CeremonyCatalog.Values);
            defs.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
            _selectedCeremonyIndex = Math.Clamp(_selectedCeremonyIndex, -1, Math.Max(0, defs.Count - 1));
            for (int i = 0; i < defs.Count; i++)
            {
                string label = defs[i].DisplayName;
                if (active != null && string.Equals(defs[i].Id, active.CeremonyId, StringComparison.Ordinal))
                    label += $" — {PhaseText(active.Phase)}";
                _ceremonyList.AddItem(label, null, false);
                if (i == _selectedCeremonyIndex)
                    _ceremonyList.Select(i);
            }

            var def = SelectedDefinition();
            if (def == null)
            {
                _detail.AddChild(AshfallUiHelpers.MakeEmptyState(
                    "No ceremony traditions are recorded. The shelter keeps no feast days.",
                    title: "NO CEREMONIES KNOWN"));
                return;
            }

            // ── Detail: definition ──
            _detail.AddChild(AshfallUiHelpers.MakeSectionHeader(def.DisplayName.ToUpperInvariant()));
            _detail.AddChild(AshfallUiHelpers.MakeMetadata(def.Description));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Preparation", $"{def.PreparationDays} days", AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Minimum people", def.MinPopulation.ToString(), AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Morale lift", $"+{def.MoraleBoost:0}", AshfallUiHelpers.ColorSuccess));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Stress relief", $"+{def.StressRelief:0}", AshfallUiHelpers.ColorSuccess));
            if (def.TruceEligible && def.TruceDurationDays > 0)
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Truce offered", $"{def.TruceDurationDays} days to invited factions", AshfallUiHelpers.ColorInfo));

            // ── Active ceremony state ──
            if (active != null && string.Equals(active.CeremonyId, def.Id, StringComparison.Ordinal))
            {
                _detail.AddChild(AshfallUiHelpers.MakeSeparator());
                _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("ACTIVE PREPARATION"));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Phase", PhaseText(active.Phase), AshfallUiHelpers.ColorText));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Prep days left", active.PreparationDaysRemaining.ToString(), AshfallUiHelpers.ColorText));

                if (def.RequiredItems.Count > 0)
                {
                    _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("MATERIALS"));
                    foreach (var req in def.RequiredItems)
                    {
                        active.CommittedItems.TryGetValue(req.ItemId, out int committed);
                        bool satisfied = committed >= req.Quantity;
                        _detail.AddChild(AshfallUiHelpers.MakeDataRow(
                            ItemDisplay.Name(req.ItemId),
                            $"{Math.Min(committed, req.Quantity)} / {req.Quantity}",
                            satisfied ? AshfallUiHelpers.ColorSuccess : AshfallUiHelpers.ColorWarning));
                    }
                }

                if (active.InvitedFactions.Count > 0)
                {
                    _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("INVITATIONS"));
                    foreach (var f in active.InvitedFactions)
                    {
                        bool accepted = active.AcceptedFactions.Contains(f);
                        _detail.AddChild(AshfallUiHelpers.MakeDataRow(
                            FactionDisplay.Name(f),
                            accepted ? "accepted" : "no answer",
                            accepted ? AshfallUiHelpers.ColorSuccess : AshfallUiHelpers.ColorWarning));
                    }
                }

                if (!string.IsNullOrEmpty(active.OccurredDisasterId))
                    _detail.AddChild(AshfallUiHelpers.MakeDataRow("Incident", DisasterDisplay.Name(active.OccurredDisasterId), AshfallUiHelpers.ColorCritical));
                if (active.ActiveTruceDaysRemaining > 0)
                    _detail.AddChild(AshfallUiHelpers.MakeDataRow("Truce days left", active.ActiveTruceDaysRemaining.ToString(), AshfallUiHelpers.ColorInfo));
            }

            // ── Feedback strip ──
            if (!string.IsNullOrEmpty(_feedbackText))
            {
                _detail.AddChild(AshfallUiHelpers.MakeSeparator());
                _detail.AddChild(_feedbackIsFailure
                    ? AshfallUiHelpers.MakeWarning(_feedbackText)
                    : AshfallUiHelpers.MakeSuccess(_feedbackText));
            }

            // ── Actions ──
            _detail.AddChild(AshfallUiHelpers.MakeSeparator());
            _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("ACTIONS"));
            var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);

            if (active == null || active.Phase >= CeremonyPhase.Completed)
            {
                var scheduleBtn = AshfallUiHelpers.MakeButton("SCHEDULE CEREMONY", () =>
                    OnActionRequested?.Invoke("schedule", def.Id));
                scheduleBtn.TooltipText = "Begins preparation. Requires enough people in the shelter.";
                row.AddChild(scheduleBtn);
            }
            else if (string.Equals(active.CeremonyId, def.Id, StringComparison.Ordinal))
            {
                if (active.Phase == CeremonyPhase.Preparing)
                {
                    foreach (var req in def.RequiredItems)
                    {
                        active.CommittedItems.TryGetValue(req.ItemId, out int committed);
                        if (committed < req.Quantity)
                        {
                            int remaining = req.Quantity - committed;
                            var btn = AshfallUiHelpers.MakeButton(
                                $"COMMIT {remaining}× {ItemDisplay.Name(req.ItemId).ToUpperInvariant()}",
                                () => OnActionRequested?.Invoke("contribute", $"{req.ItemId}:{remaining}"));
                            btn.TooltipText = $"Moves {remaining} {ItemDisplay.Name(req.ItemId)} from storage into the feast stores.";
                            row.AddChild(btn);
                        }
                    }

                    foreach (var f in FactionDisplay.KnownFactions)
                    {
                        if (active.InvitedFactions.Contains(f.id)) continue;
                        var btn = AshfallUiHelpers.MakeButton(
                            $"INVITE {f.display.ToUpperInvariant()}",
                            () => OnActionRequested?.Invoke("invite", f.id));
                        btn.TooltipText = "Sends an envoy. Standing decides whether they come — and whether a truce holds.";
                        row.AddChild(btn);
                    }
                }
            }

            if (row.GetChildCount() == 0)
                row.AddChild(AshfallUiHelpers.MakeMetadata("Nothing to do for this ceremony right now."));
            _detail.AddChild(row);
        }

        private static string PhaseText(CeremonyPhase phase) => phase switch
        {
            CeremonyPhase.Planned => "PLANNED",
            CeremonyPhase.Preparing => "PREPARING",
            CeremonyPhase.Ready => "READY",
            CeremonyPhase.Active => "UNDERWAY",
            CeremonyPhase.Completed => "COMPLETED",
            CeremonyPhase.Failed => "FAILED",
            CeremonyPhase.Cancelled => "CANCELLED",
            _ => phase.ToString().ToUpperInvariant()
        };

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
}
