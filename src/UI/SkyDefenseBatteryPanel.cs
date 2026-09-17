// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.SkyDefense;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Flagship Task 7 — Sky Defense Battery player surface.
    ///
    /// Presentation only: it renders the Core counter-battery read model
    /// (emplacements, magazine, heat, hydraulics, orbital tracks, crew) and
    /// forwards player intent to <see cref="SkyDefenseBatterySystem"/>
    /// (load / fire / service / crew). It never rolls an intercept and never
    /// recomputes mitigation; <c>PreviewInterceptChance</c> is a Core read
    /// model used verbatim, and every mutation is owned by the Core system.
    /// </summary>
    public partial class SkyDefenseBatteryPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Label _tracksText = null!;
        private Label _turretText = null!;
        private Label _commandResult = null!;

        private OptionButton _turretSelector = null!;
        private OptionButton _ordnanceSelector = null!;
        private OptionButton _trackSelector = null!;
        private OptionButton _crewSelector = null!;

        private Label _ordnanceOnHand = null!;
        private Label _previewText = null!;
        private Label _serviceOnHand = null!;
        private Label _crewText = null!;

        private Button _loadButton = null!;
        private Button _fireButton = null!;
        private Button _serviceButton = null!;
        private Button _assignButton = null!;
        private Button _removeButton = null!;

        private SkyDefenseBatterySystem? _system;
        private Func<IReadOnlyList<string>>? _livingCrew;
        private Func<string, string>? _crewName;
        private Func<string, int>? _itemOnHand;

        private string _selectedTurretId = string.Empty;
        private string _selectedOrdnanceId = string.Empty;
        private string _selectedTrackId = string.Empty;
        private string _selectedCrewId = string.Empty;

        private bool _syncing;

        public bool IsBound => _system != null;

        /// <summary>Last feedback line rendered by the panel (test/diagnostic surface).</summary>
        public string LastFeedback { get; private set; } = string.Empty;

        public void Bind(
            SkyDefenseBatterySystem system,
            Func<IReadOnlyList<string>> livingCrew,
            Func<string, string> crewName,
            Func<string, int> itemOnHand)
        {
            Unbind();
            _system = system;
            _livingCrew = livingCrew;
            _crewName = crewName;
            _itemOnHand = itemOnHand;

            _system.OnOrbitalTrackAcquired += HandleTrackAcquired;
            _system.OnVolleyFired += HandleVolleyFired;
            _system.OnInterceptResolved += HandleInterceptResolved;
            _system.OnServiced += HandleServiced;
            _system.OnMaintenanceDue += HandleMaintenanceDue;

            RefreshView();
        }

        public void Unbind()
        {
            if (_system != null)
            {
                _system.OnOrbitalTrackAcquired -= HandleTrackAcquired;
                _system.OnVolleyFired -= HandleVolleyFired;
                _system.OnInterceptResolved -= HandleInterceptResolved;
                _system.OnServiced -= HandleServiced;
                _system.OnMaintenanceDue -= HandleMaintenanceDue;
                _system = null;
            }
        }

        private void HandleTrackAcquired(OrbitalTrackState track) => RefreshView();
        private void HandleVolleyFired(string turretId, string ammoId, int magazineLeft) => RefreshView();
        private void HandleInterceptResolved(string trackId, string ammoId, bool success, float residual) => RefreshView();
        private void HandleServiced(string turretId) => RefreshView();
        private void HandleMaintenanceDue(string turretId) => RefreshView();

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("SKY DEFENSE // COUNTER-BATTERY", minWidth: 1180, minHeight: 700);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("emplacement", "Emplacement", "—", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("magazine", "Magazine", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
            _statusRail.AddCard("heat", "Barrel Heat", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
            _statusRail.AddCard("hydraulics", "Hydraulics", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
            _statusRail.AddCard("tracks", "Active Tracks", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
            _statusRail.AddCard("intercepts", "Interceptions", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            var scroll = new ScrollContainer
            {
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 10);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;
            scroll.AddChild(_contentStack);

            _contentStack.AddChild(_detailText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("ORBITAL TRACKS"));
            _contentStack.AddChild(_tracksText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("EMPLACEMENT"));

            var turretRow = new HBoxContainer();
            turretRow.AddThemeConstantOverride("separation", 10);
            turretRow.AddChild(AshfallUiHelpers.MakeBody("Turret:"));
            _turretSelector = new OptionButton { CustomMinimumSize = new Vector2(280, 34) };
            _turretSelector.ItemSelected += OnTurretSelected;
            turretRow.AddChild(_turretSelector);
            _contentStack.AddChild(turretRow);
            _contentStack.AddChild(_turretText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("MAGAZINE LOAD"));

            var loadRow = new HBoxContainer();
            loadRow.AddThemeConstantOverride("separation", 10);
            loadRow.AddChild(AshfallUiHelpers.MakeBody("Ordnance:"));
            _ordnanceSelector = new OptionButton { CustomMinimumSize = new Vector2(360, 34) };
            _ordnanceSelector.ItemSelected += OnOrdnanceSelected;
            loadRow.AddChild(_ordnanceSelector);
            _ordnanceOnHand = AshfallUiHelpers.MakeSmall("on hand: —");
            loadRow.AddChild(_ordnanceOnHand);
            _loadButton = AshfallUiHelpers.MakeButton("LOAD MAGAZINE", OnLoadPressed);
            loadRow.AddChild(_loadButton);
            _contentStack.AddChild(loadRow);

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("FIRE CONTROL"));

            var fireRow = new HBoxContainer();
            fireRow.AddThemeConstantOverride("separation", 10);
            fireRow.AddChild(AshfallUiHelpers.MakeBody("Track:"));
            _trackSelector = new OptionButton { CustomMinimumSize = new Vector2(360, 34) };
            _trackSelector.ItemSelected += OnTrackSelected;
            fireRow.AddChild(_trackSelector);
            _fireButton = AshfallUiHelpers.MakeButton("FIRE VOLLEY", OnFirePressed);
            fireRow.AddChild(_fireButton);
            _contentStack.AddChild(fireRow);
            _contentStack.AddChild(_previewText = AshfallUiHelpers.MakeSmall("Intercept preview: —"));

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("MAINTENANCE"));

            var serviceRow = new HBoxContainer();
            serviceRow.AddThemeConstantOverride("separation", 10);
            _serviceOnHand = AshfallUiHelpers.MakeSmall("machine oil on hand: —");
            serviceRow.AddChild(_serviceOnHand);
            _serviceButton = AshfallUiHelpers.MakeButton("SERVICE HYDRAULICS", OnServicePressed);
            serviceRow.AddChild(_serviceButton);
            _contentStack.AddChild(serviceRow);

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("CREW"));

            var crewRow = new HBoxContainer();
            crewRow.AddThemeConstantOverride("separation", 10);
            crewRow.AddChild(AshfallUiHelpers.MakeBody("Survivor:"));
            _crewSelector = new OptionButton { CustomMinimumSize = new Vector2(320, 34) };
            _crewSelector.ItemSelected += OnCrewSelected;
            crewRow.AddChild(_crewSelector);
            _assignButton = AshfallUiHelpers.MakeButton("ASSIGN GUNNER", OnAssignPressed);
            crewRow.AddChild(_assignButton);
            _removeButton = AshfallUiHelpers.MakeButton("REMOVE GUNNER", OnRemovePressed);
            crewRow.AddChild(_removeButton);
            _contentStack.AddChild(crewRow);
            _contentStack.AddChild(_crewText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _commandResult = AshfallUiHelpers.MakeSmall("—");
            _commandResult.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _contentStack.AddChild(_commandResult);

            var note = AshfallUiHelpers.MakeBody(
                "The battery consumes authored ordnance from the shelter inventory and machine oil for "
                + "hydraulic service. Firing is a real roll owned by the Core authority; a successful "
                + "intercept only reduces the incoming strike's energy, never erases it.");
            note.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            note.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            _contentStack.AddChild(note);

            _shell.SetContent(scroll);
            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }

        // ── Selection handlers (store intent only) ─────────────────────────

        private void OnTurretSelected(long index)
        {
            if (_syncing) return;
            var turrets = _system?.Turrets;
            if (turrets != null && index >= 0 && index < turrets.Count) _selectedTurretId = turrets[(int)index].turret_id;
            RefreshView();
        }

        private void OnOrdnanceSelected(long index)
        {
            if (_syncing) return;
            var ordnance = SortedOrdnance();
            if (index >= 0 && index < ordnance.Count) _selectedOrdnanceId = ordnance[(int)index].ordnance_id;
            RefreshView();
        }

        private void OnTrackSelected(long index)
        {
            if (_syncing) return;
            var tracks = _system?.Tracks;
            if (tracks != null && index >= 0 && index < tracks.Count) _selectedTrackId = tracks[(int)index].track_id;
            RefreshView();
        }

        private void OnCrewSelected(long index)
        {
            if (_syncing) return;
            var crew = LivingCrew();
            if (index >= 0 && index < crew.Count) _selectedCrewId = crew[(int)index];
            RefreshView();
        }

        // ── Command handlers (route to Core only) ──────────────────────────

        private void OnLoadPressed()
        {
            if (_system == null) return;
            string turretId = SelectedTurretId();
            if (string.IsNullOrEmpty(turretId) || string.IsNullOrEmpty(_selectedOrdnanceId)) return;
            ShowResult(_system.TryLoadMagazine(turretId, _selectedOrdnanceId));
        }

        private void OnFirePressed()
        {
            if (_system == null) return;
            string turretId = SelectedTurretId();
            if (string.IsNullOrEmpty(turretId) || string.IsNullOrEmpty(_selectedTrackId)) return;
            ShowResult(_system.TryFireVolley(turretId, _selectedTrackId));
        }

        private void OnServicePressed()
        {
            if (_system == null) return;
            string turretId = SelectedTurretId();
            if (string.IsNullOrEmpty(turretId)) return;
            ShowResult(_system.TryServiceHydraulics(turretId));
        }

        private void OnAssignPressed()
        {
            if (_system == null) return;
            string turretId = SelectedTurretId();
            if (string.IsNullOrEmpty(turretId) || string.IsNullOrEmpty(_selectedCrewId)) return;
            ShowResult(_system.TryAssignCrew(turretId, _selectedCrewId));
        }

        private void OnRemovePressed()
        {
            if (_system == null) return;
            string turretId = SelectedTurretId();
            if (string.IsNullOrEmpty(turretId) || string.IsNullOrEmpty(_selectedCrewId)) return;
            ShowResult(_system.TryRemoveCrew(turretId, _selectedCrewId));
        }

        private void ShowResult(ActionResult result)
        {
            LastFeedback = result.IsSuccess
                ? $"OK: {result.MessageKey}"
                : $"Blocked: {DescribeFailure(result.FailureCode)}";
            if (_commandResult != null)
            {
                _commandResult.Text = LastFeedback;
                _commandResult.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(
                    result.IsSuccess ? DesignTheme.Success : DesignTheme.Warning));
            }
            RefreshView();
        }

        private static string DescribeFailure(string? code) => code switch
        {
            "unknown_turret" => "no such emplacement.",
            "unknown_ordnance" => "that ordnance is not in the authored catalog.",
            "unknown_track" => "no such orbital track.",
            "track_resolved" => "that track is already resolved.",
            "magazine_full" => "the magazine is already full.",
            "no_inventory" => "no inventory authority is bound.",
            "insufficient_ammo" => "not enough rounds in the shelter inventory.",
            "barrel_hot" => "the barrel is too hot to load or fire; let it dissipate.",
            "turret_down" => "the emplacement is not operational.",
            "magazine_empty" => "the magazine is empty; load ordnance first.",
            "hydraulics_failed" => "hydraulics have failed; service the mount.",
            "missing_oil" => "machine oil is required for hydraulic service.",
            "survivor_unavailable" => "that survivor is already claimed by another institution.",
            "crew_not_assigned" => "that survivor is not assigned to this emplacement.",
            _ => string.IsNullOrEmpty(code) ? "the command was refused." : code,
        };

        // ── Read-model rendering ───────────────────────────────────────────

        public void RefreshView()
        {
            if (_system == null || _statusRail == null) return;

            _syncing = true;
            try
            {
                SyncTurrets();
                SyncOrdnance();
                SyncTracks();
                SyncCrew();
            }
            finally
            {
                _syncing = false;
            }

            var turret = CurrentTurret();
            int activeTracks = 0;
            foreach (var t in _system.Tracks)
                if (t != null && !t.resolved) activeTracks++;

            _statusRail.Set("emplacement",
                turret == null ? "NONE" : (turret.is_operational ? "OPERATIONAL" : "DOWN"),
                turret != null && !turret.is_operational ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("magazine",
                turret == null || string.IsNullOrEmpty(turret.loaded_ammo_id) ? "EMPTY" : $"{turret.magazine_count} rnd",
                turret != null && turret.magazine_count <= 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("heat",
                turret == null ? "—" : $"{turret.barrel_heat}/100",
                turret != null && turret.barrel_heat >= SkyDefenseBatterySystem.HeatSeizureThreshold
                    ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("hydraulics",
                turret == null ? "—" : $"{turret.hydraulic_condition}/100",
                turret != null && turret.hydraulic_condition <= 20 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("tracks", activeTracks.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("intercepts", _system.TotalInterceptions.ToString(), AshfallMetricCard.Criticality.Normal);

            if (_detailText != null)
            {
                _detailText.Text =
                    $"Emplacements: {_system.Turrets.Count} | Tracks: {_system.Tracks.Count} | "
                    + $"Total volleys fired: {_system.TotalVolleys} | Total interceptions: {_system.TotalInterceptions}";
            }

            RenderTracks();
            RenderTurret(turret);
            RenderLoadRow();
            RenderFireRow(turret);
            RenderServiceRow();
            RenderCrew(turret);
        }

        private void SyncTurrets()
        {
            if (_turretSelector == null || _system == null) return;
            _turretSelector.Clear();
            var turrets = _system.Turrets;
            int selected = 0;
            for (int i = 0; i < turrets.Count; i++)
            {
                var t = turrets[i];
                _turretSelector.AddItem(t.turret_id, i);
                if (t.turret_id == _selectedTurretId) selected = i;
            }
            if (string.IsNullOrEmpty(_selectedTurretId) && turrets.Count > 0) _selectedTurretId = turrets[0].turret_id;
            _turretSelector.Selected = selected;
        }

        private void SyncOrdnance()
        {
            if (_ordnanceSelector == null) return;
            _ordnanceSelector.Clear();
            var ordnance = SortedOrdnance();
            int selected = 0;
            for (int i = 0; i < ordnance.Count; i++)
            {
                _ordnanceSelector.AddItem($"{ordnance[i].display_name} [{ordnance[i].magazine_units} rnd mag]", i);
                if (ordnance[i].ordnance_id == _selectedOrdnanceId) selected = i;
            }
            if (string.IsNullOrEmpty(_selectedOrdnanceId) && ordnance.Count > 0) _selectedOrdnanceId = ordnance[0].ordnance_id;
            _ordnanceSelector.Selected = selected;
        }

        private void SyncTracks()
        {
            if (_trackSelector == null || _system == null) return;
            _trackSelector.Clear();
            var tracks = _system.Tracks;
            int selected = 0;
            for (int i = 0; i < tracks.Count; i++)
            {
                var t = tracks[i];
                _trackSelector.AddItem(string.IsNullOrEmpty(t.track_id) ? $"track {i}" : t.track_id, i);
                if (t.track_id == _selectedTrackId) selected = i;
            }
            if (string.IsNullOrEmpty(_selectedTrackId) && tracks.Count > 0) _selectedTrackId = tracks[0].track_id;
            _trackSelector.Selected = selected;
        }

        private void SyncCrew()
        {
            if (_crewSelector == null) return;
            _crewSelector.Clear();
            var crew = LivingCrew();
            int selected = 0;
            for (int i = 0; i < crew.Count; i++)
            {
                string id = crew[i];
                _crewSelector.AddItem(CrewLabel(id), i);
                if (id == _selectedCrewId) selected = i;
            }
            if (string.IsNullOrEmpty(_selectedCrewId) && crew.Count > 0) _selectedCrewId = crew[0];
            _crewSelector.Selected = selected;
        }

        private void RenderTracks()
        {
            if (_tracksText == null || _system == null) return;
            if (_system.Tracks.Count == 0)
            {
                _tracksText.Text = "No orbital tracks on file. The sky layer is currently quiet.";
                return;
            }

            var lines = new List<string>();
            foreach (var t in _system.Tracks)
            {
                if (t == null) continue;
                lines.Add(
                    $"{t.track_id} | {t.severity} | impact day {t.impact_day} | grid X {t.target_grid_x} | "
                    + $"{t.energy_mj:F0} MJ | volleys {t.volleys_fired} | {(t.resolved ? "RESOLVED" : "ACTIVE")}");
            }
            _tracksText.Text = string.Join("\n", lines);
        }

        private void RenderTurret(CounterBatteryTurretState? turret)
        {
            if (_turretText == null) return;
            if (turret == null)
            {
                _turretText.Text = "No emplacement is registered.";
                return;
            }

            _turretText.Text =
                $"State: {(turret.is_operational ? "OPERATIONAL" : "DOWN")} | "
                + $"Loaded: {(string.IsNullOrEmpty(turret.loaded_ammo_id) ? "none" : turret.loaded_ammo_id)} | "
                + $"Magazine: {turret.magazine_count} | Heat: {turret.barrel_heat}/100 | "
                + $"Hydraulics: {turret.hydraulic_condition}/100 | Radar cal: {turret.radar_calibration}/100 | "
                + $"Volleys since service: {turret.volleys_since_service} | Az/El: {turret.azimuth}/{turret.elevation}\n"
                + $"Crew: {(turret.assigned_crew_ids.Count == 0 ? "unmanned" : string.Join(", ", CrewLabels(turret.assigned_crew_ids)))}";
        }

        private void RenderLoadRow()
        {
            if (_ordnanceOnHand == null) return;
            var ordnance = SortedOrdnance();
            var selected = FindOrdnance(_selectedOrdnanceId);
            if (selected == null)
            {
                _ordnanceOnHand.Text = "on hand: —";
                if (_loadButton != null) _loadButton.Disabled = true;
                return;
            }
            int onHand = _itemOnHand?.Invoke(selected.item_id) ?? 0;
            _ordnanceOnHand.Text = $"on hand: {onHand} × {selected.item_id} (mag takes {selected.magazine_units})";
            if (_loadButton != null) _loadButton.Disabled = ordnance.Count == 0;
        }

        private void RenderFireRow(CounterBatteryTurretState? turret)
        {
            if (_previewText == null) return;
            if (turret == null)
            {
                _previewText.Text = "Intercept preview: —";
                if (_fireButton != null) _fireButton.Disabled = true;
                return;
            }

            var track = FindTrack(_selectedTrackId);
            var ordnance = FindOrdnanceByItemId(turret.loaded_ammo_id);
            if (track == null || ordnance == null)
            {
                _previewText.Text = track == null
                    ? "Intercept preview: select an active orbital track."
                    : "Intercept preview: load ordnance to compute a firing solution.";
                if (_fireButton != null) _fireButton.Disabled = true;
                return;
            }

            int chance = _system!.PreviewInterceptChance(turret, track, ordnance);
            _previewText.Text =
                $"Intercept preview: {chance}% (Core-authored; {track.severity} track, {ordnance.display_name}). "
                + $"Residual shrapnel on success: {ordnance.residual_shrapnel_severity:P0}.";
            if (_fireButton != null) _fireButton.Disabled = track.resolved;
        }

        private void RenderServiceRow()
        {
            if (_serviceOnHand == null) return;
            int oil = _itemOnHand?.Invoke(SkyDefenseBatterySystem.ServiceOilItemId) ?? 0;
            _serviceOnHand.Text = $"machine oil on hand: {oil}";
        }

        private void RenderCrew(CounterBatteryTurretState? turret)
        {
            if (_crewText == null) return;
            if (turret == null)
            {
                _crewText.Text = "No emplacement is registered.";
                return;
            }
            _crewText.Text = turret.assigned_crew_ids.Count == 0
                ? "No gunners assigned. Crew claims route through the institution ledger."
                : "Assigned gunners: " + string.Join(", ", CrewLabels(turret.assigned_crew_ids));
        }

        // ── Helpers ───────────────────────────────────────────────────────

        private string SelectedTurretId()
        {
            if (!string.IsNullOrEmpty(_selectedTurretId)) return _selectedTurretId;
            var turrets = _system?.Turrets;
            return turrets != null && turrets.Count > 0 ? turrets[0].turret_id : string.Empty;
        }

        private CounterBatteryTurretState? CurrentTurret()
        {
            if (_system == null) return null;
            string id = SelectedTurretId();
            if (string.IsNullOrEmpty(id)) return _system.Turrets.Count > 0 ? _system.Turrets[0] : null;
            return _system.GetTurret(id);
        }

        private OrbitalTrackState? FindTrack(string trackId)
        {
            if (_system == null || string.IsNullOrEmpty(trackId)) return null;
            return _system.GetTrack(trackId);
        }

        private SkyDefenseOrdnanceDefinition? FindOrdnance(string ordnanceId)
        {
            if (_system == null || string.IsNullOrEmpty(ordnanceId)) return null;
            return _system.GetOrdnance(ordnanceId);
        }

        private SkyDefenseOrdnanceDefinition? FindOrdnanceByItemId(string itemId)
        {
            if (_system == null || string.IsNullOrEmpty(itemId)) return null;
            foreach (var kv in _system.OrdnanceCatalog)
                if (kv.Value != null && string.Equals(kv.Value.item_id, itemId, StringComparison.Ordinal))
                    return kv.Value;
            return null;
        }

        private List<SkyDefenseOrdnanceDefinition> SortedOrdnance()
        {
            var list = new List<SkyDefenseOrdnanceDefinition>();
            if (_system == null) return list;
            foreach (var kv in _system.OrdnanceCatalog)
                if (kv.Value != null) list.Add(kv.Value);
            list.Sort((a, b) => string.CompareOrdinal(a.ordnance_id, b.ordnance_id));
            return list;
        }

        private IReadOnlyList<string> LivingCrew()
        {
            var crew = _livingCrew?.Invoke();
            if (crew == null) return Array.Empty<string>();
            var list = new List<string>(crew);
            list.Sort(StringComparer.Ordinal);
            return list;
        }

        private string CrewLabel(string survivorId)
        {
            string name = _crewName?.Invoke(survivorId) ?? string.Empty;
            return string.IsNullOrWhiteSpace(name) ? survivorId : name;
        }

        private List<string> CrewLabels(IEnumerable<string> ids)
        {
            var labels = new List<string>();
            foreach (var id in ids) labels.Add(CrewLabel(id));
            labels.Sort(StringComparer.Ordinal);
            return labels;
        }
    }
}