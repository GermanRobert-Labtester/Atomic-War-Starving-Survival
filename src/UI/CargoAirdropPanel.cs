// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core.World;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 205: cargo airdrop tracking & recovery console. Presentation-only —
    /// every mutation routes through the bound <see cref="CargoAirdropHostSession"/>;
    /// recovery itself runs a standard expedition sortie to the drop-site
    /// destination. Communicates state → blocker → cost → consequence.
    /// </summary>
    public partial class CargoAirdropPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private CargoAirdropHostSession? _session;
        private ItemList _dropList = null!;
        private VBoxContainer _detail = null!;
        private string _selectedEventId = string.Empty;
        private string _feedbackText = string.Empty;
        private bool _feedbackIsFailure;

        public bool IsBound => _session != null;

        public void Bind(CargoAirdropHostSession session) { _session = session; _selectedEventId = string.Empty; RefreshView(); }
        public void Unbind() { _session = null; }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("AIRDROP WATCH // CARGO RECOVERY", minWidth: 1000, minHeight: 650);

            _dropList = new ItemList
            {
                CustomMinimumSize = new Vector2(280, 0),
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.Fill
            };
            _dropList.ItemSelected += OnDropSelected;

            _detail = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);

            var bodyRow = new HBoxContainer();
            bodyRow.AddChild(_dropList);

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

        private void OnDropSelected(long index)
        {
            if (_session == null) return;
            var drops = _session.System.State.drops;
            if (index >= 0 && index < drops.Count)
            {
                _selectedEventId = drops[(int)index].event_id;
                RefreshView();
            }
        }

        private AirdropEventState? SelectedDrop()
        {
            if (_session == null) return null;
            if (string.IsNullOrEmpty(_selectedEventId))
                return _session.System.State.drops.Count > 0 ? _session.System.State.drops[0] : null;
            return _session.System.State.drops.Find(d => d.event_id == _selectedEventId);
        }

        private void SetFeedback(string message, bool failure)
        {
            _feedbackText = message;
            _feedbackIsFailure = failure;
        }

        private Ashfall.Core.ActionResult RunCommand(Func<Ashfall.Core.ActionResult> action, string successText)
        {
            if (_session == null) return Ashfall.Core.ActionResult.Failed("unbound", "airdrop.unbound");
            var result = action();
            SetFeedback(result.IsSuccess ? successText : $"Blocked: {DescribeFailure(result.FailureCode)}", !result.IsSuccess);
            RefreshView();
            return result;
        }

        private static string DescribeFailure(string? code) => code switch
        {
            "airdrop.profile_invalid" => "Unknown drop profile.",
            "airdrop.missing_signal" => "No radio contact linked to this drop.",
            "airdrop.max_active" => "Too many active drops — recover or lose one first.",
            "airdrop.event_not_found" => "Unknown airdrop event.",
            "airdrop.wrong_phase" => "Wrong drop phase for that action.",
            "airdrop.not_landed" => "The canister has not landed yet.",
            "airdrop.beacon_active" => "The beacon is already active.",
            "airdrop.empty_pool" => "Drop profile has an empty cargo pool.",
            _ => code ?? "unknown error"
        };

        private static string PhaseText(AirdropEventState ev) => ev.phase switch
        {
            "scheduled" => "SCHEDULED",
            "descending" => $"DESCENDING (band {ev.current_band})",
            "landed" => ev.beacon_active ? "LANDED — BEACON LIVE" : "LANDED — BEACON DARK",
            "recovered" => "RECOVERED",
            "intercepted" => "LOST TO HOSTILES",
            "expired" => "EXPIRED",
            _ => ev.phase.ToUpperInvariant()
        };

        public void RefreshView()
        {
            if (_session == null || _detail == null) return;
            var sys = _session.System;
            AshfallUiHelpers.EmptyChildren(_detail);
            _dropList.Clear();

            var drops = sys.State.drops;
            for (int i = 0; i < drops.Count; i++)
            {
                var d = drops[i];
                _dropList.AddItem($"Drop {i + 1} — {PhaseText(d)}", null, false);
                if (d.event_id == _selectedEventId || (string.IsNullOrEmpty(_selectedEventId) && i == 0))
                {
                    _dropList.Select(i);
                    if (string.IsNullOrEmpty(_selectedEventId)) _selectedEventId = d.event_id;
                }
            }

            _detail.AddChild(AshfallUiHelpers.MakeSectionHeader("DROP STATUS"));

            if (!string.IsNullOrEmpty(_feedbackText))
            {
                _detail.AddChild(_feedbackIsFailure
                    ? AshfallUiHelpers.MakeWarning(_feedbackText)
                    : AshfallUiHelpers.MakeSuccess(_feedbackText));
            }

            var drop = SelectedDrop();
            if (drop == null)
            {
                _detail.AddChild(AshfallUiHelpers.MakeEmptyState(
                    "No airdrop events. Resolved radio contacts may offer scheduled supply canisters.",
                    title: "NO DROPS INBOUND",
                    actionHint: "Watch the radio for resolved contacts."));
                return;
            }

            var profile = sys.FindProfile(drop.drop_profile_id);
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Event", $"Drop {sys.State.drops.IndexOf(drop) + 1} — {profile?.display_name ?? drop.drop_profile_id}", AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Phase", PhaseText(drop),
                drop.phase == "intercepted" || drop.phase == "expired" ? AshfallUiHelpers.ColorCritical : AshfallUiHelpers.ColorText));

            if (drop.phase == "descending")
            {
                double driftKm = drop.wind_speed_kph * (profile?.wind_sensitivity ?? 1f) * Math.Max(1, sys.Catalog.descent_bands);
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Wind", $"{drop.wind_speed_kph:F0} kph from {drop.wind_direction_deg:F0}° — drift ≈ {driftKm:0.#} km", AshfallUiHelpers.ColorText));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Target / predicted landing", $"({drop.target_x},{drop.target_y}) → ({drop.landing_x},{drop.landing_y})", AshfallUiHelpers.ColorText));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Lands", $"day {drop.landing_day}", AshfallUiHelpers.ColorText));
            }

            if (drop.phase == "landed")
            {
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Landing site", $"grid ({drop.landing_x},{drop.landing_y})", AshfallUiHelpers.ColorText));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Beacon", drop.beacon_active
                    ? $"LIVE — expires day {drop.beacon_expires_day}"
                    : $"DARK — expired day {drop.beacon_expires_day}",
                    drop.beacon_active ? AshfallUiHelpers.ColorSuccess : AshfallUiHelpers.ColorWarning));
                int cargoUnits = drop.remaining_contents.Sum(c => c.quantity);
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Cargo integrity", $"{drop.cargo_integrity_pct}%", AshfallUiHelpers.ColorText));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Crated cargo", $"{cargoUnits} units remaining", AshfallUiHelpers.ColorText));

                int hostileEta = (10000 - drop.interception_progress_bp) / Math.Max(1, profile?.interception_rate_per_day_bp ?? 900);
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Hostile scouts", $"reach the crate in ≈ {hostileEta}d",
                    hostileEta <= 1 ? AshfallUiHelpers.ColorCritical : AshfallUiHelpers.ColorWarning));

                _detail.AddChild(AshfallUiHelpers.MakeBody(
                    "Dispatch a standard expedition to the drop-site destination shown on the map. On arrival, cargo transfers into the sortie subject to its carry capacity — what does not fit stays crated."));
            }

            if (drop.phase == "recovered" || drop.phase == "intercepted" || drop.phase == "expired")
            {
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Outcome", PhaseText(drop), AshfallUiHelpers.ColorMuted));
            }

            _detail.AddChild(AshfallUiHelpers.MakeSeparator());
            _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("ACTIONS"));

            var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            if (drop != null && drop.phase == "landed" && !drop.beacon_active)
            {
                row.AddChild(MakeAction("REACTIVATE BEACON", "Free action while the crate is intact and on schedule.",
                    () => RunCommand(() => _session!.ReactivateBeacon(_selectedEventId), "Beacon reactivated.")));
            }
            if (row.GetChildCount() == 0)
            {
                var note = AshfallUiHelpers.MakeMetadata("Recovery runs through the expedition console — dispatch a sortie to the drop-site destination.");
                row.AddChild(note);
            }
            _detail.AddChild(row);
        }

        private Control MakeAction(string label, string consequence, Func<Ashfall.Core.ActionResult> command)
        {
            var btn = AshfallUiHelpers.MakeButton(label, () => command());
            btn.TooltipText = consequence;
            btn.CustomMinimumSize = new Vector2(0, 30);
            return btn;
        }
    }
}
