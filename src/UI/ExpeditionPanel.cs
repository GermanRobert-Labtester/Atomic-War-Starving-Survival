// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Factions;
using Ashfall.Core.UI;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.Localization;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Expedition panel.
    /// Manages wasteland scavenging sorties, target selection, squad deployment,
    /// push-your-luck looting, and salvage recovery.
    /// </summary>
    public partial class ExpeditionPanel : Control
    {
        public event Action? OnClose;
        public event Action? OnExpeditionUpdated;
        public event Action<List<ExpeditionLootEntry>>? OnLootDeposited;
        // Console deep links (T14): the panel only raises the request; the
        // host owns the routing decision for the registered
        // expedition_radar / expedition_camp PanelRegistry routes.
        public event Action? OnOpenRadarRequested;
        public event Action? OnOpenCampConsoleRequested;
        public event Action? OnOpenRailwayTerminalRequested;

        private ExpeditionHostSession? _expeditionHost;
        private WorldHostSession? _worldHost;
        private SurvivorsHostSession? _survivorsHost;
        private InventoryHostSession? _inventoryHost;

        private VBoxContainer _targetsContainer = null!;
        private VBoxContainer _activeContainer = null!;
        private VBoxContainer _pendingContainer = null!;
        private Label _pendingHeader = null!;
        private Label _statusSummary = null!;
        private Label _estimateLabel = null!;
        private Label _dispatchStatusLabel = null!;
        private Label? _prepLabel;
        private Label? _returnSummaryLabel;

        /// <summary>P107 — the last return-loot ceremony or failure aftermath text
        /// rendered by the panel (empty until a sortie ends). Observability only.</summary>
        public string LastReturnSummary { get; private set; } = string.Empty;

        private string _selectedTargetId = "loc_the_allotments";
        private ExpeditionStance _selectedStance = ExpeditionStance.Stealth;

        // ── Dispatch preparation (survivor + vehicle + weapon loadout) ──
        private OptionButton? _survivorSelect;
        private OptionButton? _vehicleSelect;
        private OptionButton? _weaponSelect;
        private readonly List<string> _survivorIds = new();
        private readonly List<string> _vehicleIds = new();
        private readonly List<string> _weaponInstanceIds = new();
        private Ashfall.Core.EquipmentConditionSystem? _equipment;
        private ConfirmationDialog? _fitnessWarningDialog;
        private Action? _pendingFitnessDispatch;

        /// <summary>The vehicle chosen in the dispatch-preparation selector, or "" for foot.</summary>
        private string SelectedVehicleId =>
            _vehicleSelect != null && _vehicleSelect.Selected > 0 && _vehicleSelect.Selected - 1 < _vehicleIds.Count
                ? _vehicleIds[_vehicleSelect.Selected - 1]
                : string.Empty;

        private string SelectedWeaponInstanceId =>
            _weaponSelect != null && _weaponSelect.Selected > 0 && _weaponSelect.Selected - 1 < _weaponInstanceIds.Count
                ? _weaponInstanceIds[_weaponSelect.Selected - 1]
                : string.Empty;

        /// <summary>P105 follow-up — the survivor chosen in the dispatch selector, or "".</summary>
        private string SelectedSurvivorId =>
            _survivorSelect != null && _survivorSelect.Selected >= 0 && _survivorSelect.Selected < _survivorIds.Count
                ? _survivorIds[_survivorSelect.Selected]
                : string.Empty;

        /// <summary>The survivor the estimate/dispatch targets: the selector's
        /// choice when still dispatchable, otherwise the first living survivor.</summary>
        private string? ChosenSurvivor(List<string> living)
        {
            string selected = SelectedSurvivorId;
            if (!string.IsNullOrEmpty(selected) && living.Contains(selected)) return selected;
            return living.Count > 0 ? living[0] : null;
        }

        // ── Encounter surface (modal default / autoplay flag) ────────
        private readonly Queue<ExpeditionEncounterBridge.EncounterSurfaced> _encounterQueue = new();
        private Control? _encounterModal;
        private Label? _encounterTitle;
        private Label? _encounterContext;
        private TextureRect? _encounterFactionEmblem;
        private Label? _encounterBody;
        private Button? _encounterBtnOk;
        private Control? _encounterBanner;
        private Label? _encounterBannerLabel;
        private bool _modalActive;
        private float _bannerTimer;
        private const float BannerDuration = 3f;
        private ExpeditionEncounterBridge.EncounterSurfaced? _lastSurfaced;
        private VBoxContainer? _choicesContainer;
        private bool _pendingBatchMode;

        public Label? EncounterTitleLabel => _encounterTitle;
        public Label? EncounterContextLabel => _encounterContext;
        public Label? EncounterBodyLabel => _encounterBody;
        public Control? EncounterModal => _encounterModal;
        public VBoxContainer? ChoicesContainer => _choicesContainer;
        public ExpeditionEncounterBridge.EncounterSurfaced? LastSurfaced => _lastSurfaced;

        /// <summary>P105/P106 — the rendered pre-dispatch prep projection (empty
        /// until the estimate line runs). Observability for the UI gate.</summary>
        public string PrepSummaryText => _prepLabel?.Text ?? string.Empty;

        /// <summary>Task 10 — clear the ceremony/aftermath surface on a campaign
        /// reset so a new run never inherits the previous run's return text.</summary>
        public void ClearReturnSummary() => SetReturnSummary(string.Empty, success: true);

        /// <summary>Test seam (P107): render a returned sortie through the shared
        /// Core formatter without a live tick loop.</summary>
        public void PresentReturnForTest(ExpeditionState state)
            => SetReturnSummary(
                ExpeditionReturnReport.BuildCeremony(state, id => _expeditionHost?.Items?.Get(id)?.displayName ?? ExpeditionReturnReport.HumanizeToken(id)),
                success: true);

        /// <summary>Test seam (P108): render a failed sortie through the shared
        /// Core formatter without a live tick loop.</summary>
        public void PresentFailureForTest(ExpeditionState state, string reason)
            => SetReturnSummary(ExpeditionReturnReport.BuildAftermath(state, reason), success: false);

        public bool IsBound => _expeditionHost != null;

        public void Bind(
            ExpeditionHostSession expeditionHost,
            SurvivorsHostSession? survivorsHost = null,
            InventoryHostSession? inventoryHost = null,
            Ashfall.Core.EquipmentConditionSystem? equipment = null,
            WorldHostSession? world = null)
        {
            if (_expeditionHost != null)
            {
                _expeditionHost.Engine.OnExpeditionCompleted -= OnExpeditionCompleted;
                _expeditionHost.Engine.OnExpeditionFailed -= OnExpeditionFailed;
                _expeditionHost.StateChanged -= RefreshView;
            }

            _expeditionHost = expeditionHost;
            _survivorsHost = survivorsHost;
            _inventoryHost = inventoryHost;
            _equipment = equipment;
            _worldHost = world;

            if (_expeditionHost != null)
            {
                _expeditionHost.Engine.OnExpeditionCompleted += OnExpeditionCompleted;
                _expeditionHost.Engine.OnExpeditionFailed += OnExpeditionFailed;
                _expeditionHost.StateChanged += RefreshView;

                RefreshView();
            }
        }

        public void Unbind()
        {
            if (_expeditionHost != null)
            {
                _expeditionHost.Engine.OnExpeditionCompleted -= OnExpeditionCompleted;
                _expeditionHost.Engine.OnExpeditionFailed -= OnExpeditionFailed;
                _expeditionHost.StateChanged -= RefreshView;
                _expeditionHost = null;
            }
            _survivorsHost = null;
            _inventoryHost = null;
            RefreshView();
        }

        private void OnExpeditionCompleted(ExpeditionState state)
        {
            // P107 — Main is the single depositor; the host session owns the
            // summary text so it survives an unbound panel. This handler only
            // surfaces it (and raises the deposit notification).
            SetReturnSummary(_expeditionHost?.LastReturnSummary ?? string.Empty, success: true);
            if (state?.loot != null && state.loot.Count > 0)
                OnLootDeposited?.Invoke(state.loot);
            OnExpeditionUpdated?.Invoke();
            RefreshView();
        }

        /// <summary>P108 — failure consequence surfaced once the host has applied
        /// it through the existing owners (health, fate, journal).</summary>
        private void OnExpeditionFailed(ExpeditionState state, string reason)
        {
            SetReturnSummary(_expeditionHost?.LastReturnSummary ?? string.Empty, success: false);
            OnExpeditionUpdated?.Invoke();
            RefreshView();
        }

        private void SetReturnSummary(string text, bool success)
        {
            LastReturnSummary = text ?? string.Empty;
            if (_returnSummaryLabel == null) return;
            _returnSummaryLabel.Text = LastReturnSummary;
            _returnSummaryLabel.Visible = !string.IsNullOrEmpty(LastReturnSummary);
            _returnSummaryLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(
                success ? Ashfall.Core.UI.Theme.Lethe : Ashfall.Core.UI.Theme.Critical));
        }

        /// <summary>Swap the departure backdrop to the phase variant (dawn/day/dusk/night).</summary>
        public void SetLightingPhase(string phase)
            => BackdropArt.SetTexture(this, BackdropArt.ExpeditionDepartureFor(phase));

        public override void _Ready()
        {
            SetProcess(false);
            VisibilityChanged += RefreshBannerProcessing;
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            // Load faction display names from lore catalog (idempotent)
            try
            {
                string lorePath = CatalogPath.ResolveCatalog("faction_lore.json");
                if (File.Exists(lorePath))
                    FactionDisplayNameCatalog.LoadFromJson(File.ReadAllText(lorePath));
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[ExpeditionPanel] Failed to load faction lore display names: {ex.Message}");
            }

            // Placeholder expedition-departure backdrop (days 1–7, intact shelter mouth).
            BackdropArt.Apply(this, BackdropArt.ExpeditionDeparture, 0.90f); // A11Y §3: dim 0.82 let bright art under the Dim summary fall to ~3.8:1

            var scroll = new ScrollContainer();
            scroll.SetAnchorsPreset(LayoutPreset.FullRect);
            scroll.HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled;
            AddChild(scroll);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            center.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            center.SizeFlagsVertical = SizeFlags.ExpandFill;
            scroll.AddChild(center);

            var rootBox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            rootBox.CustomMinimumSize = new Vector2(760, 0);
            center.AddChild(rootBox);

            var header = AshfallUiHelpers.MakeTitle(AshfallLocalization.Tr("ui.expedition.shell_title", "WASTELAND EXPEDITIONS // SORTIE PLANNER"), Ashfall.Core.UI.Theme.FontSizeH1);
            header.HorizontalAlignment = HorizontalAlignment.Center;
            rootBox.AddChild(header);

            _statusSummary = AshfallUiHelpers.MakeMetadata(AshfallLocalization.Tr("ui.expedition.summary", "Plan reconnaissance and scavenging sorties. Monitor radiation risk, distance, and survivor stamina."));
            _statusSummary.HorizontalAlignment = HorizontalAlignment.Center;
            _statusSummary.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            rootBox.AddChild(_statusSummary);

            // P107 — persistent return ceremony / failure aftermath block.
            _returnSummaryLabel = AshfallUiHelpers.MakeBody("", true);
            _returnSummaryLabel.Visible = false;
            _returnSummaryLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _returnSummaryLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe));
            rootBox.AddChild(_returnSummaryLabel);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Active Expeditions ──
            var activeTitle = AshfallUiHelpers.MakeSectionHeader(
                AshfallLocalization.Tr("ui.expedition.section.active", "ACTIVE SORTIES IN THE FIELD"));
            rootBox.AddChild(activeTitle);

            _activeContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_activeContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Pending Surfaced Encounters ──
            _pendingHeader = AshfallUiHelpers.MakeSectionHeader(
                AshfallLocalization.Tr("ui.expedition.section.pending", "PENDING SURFACED ENCOUNTERS"));
            _pendingHeader.Visible = false;
            rootBox.AddChild(_pendingHeader);

            _pendingContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            _pendingContainer.Visible = false;
            rootBox.AddChild(_pendingContainer);

            // ── Dispatch Preparation (vehicle + weapon loadout) ──
            var prepTitle = AshfallUiHelpers.MakeSectionHeader(
                AshfallLocalization.Tr("ui.expedition.section.prep", "DISPATCH PREPARATION // MOTOR POOL & ARMORY"));
            rootBox.AddChild(prepTitle);

            var prepRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingMd);

            prepRow.AddChild(AshfallUiHelpers.MakeBody(AshfallLocalization.Tr("ui.expedition.prep_survivor", "SURVIVOR:")));
            _survivorSelect = new OptionButton();
            _survivorSelect.CustomMinimumSize = new Vector2(200, 30);
            _survivorSelect.ItemSelected += _ => UpdateEstimateLine();
            prepRow.AddChild(_survivorSelect);

            prepRow.AddChild(AshfallUiHelpers.MakeBody(AshfallLocalization.Tr("ui.expedition.prep_vehicle", "VEHICLE:")));
            _vehicleSelect = new OptionButton();
            _vehicleSelect.CustomMinimumSize = new Vector2(230, 30);
            _vehicleSelect.ItemSelected += _ => UpdateEstimateLine();
            prepRow.AddChild(_vehicleSelect);

            prepRow.AddChild(AshfallUiHelpers.MakeBody(AshfallLocalization.Tr("ui.expedition.prep_weapon", "WEAPON:")));
            _weaponSelect = new OptionButton();
            _weaponSelect.CustomMinimumSize = new Vector2(230, 30);
            _weaponSelect.ItemSelected += _ => UpdateEstimateLine();
            prepRow.AddChild(_weaponSelect);

            var btnRefuel = AshfallUiHelpers.MakeButton(AshfallLocalization.Tr("ui.expedition.refuel", "REFUEL TOP-UP"), () =>
            {
                string vehicleId = SelectedVehicleId;
                if (_expeditionHost == null || _inventoryHost == null || string.IsNullOrEmpty(vehicleId)) return;
                int have = _inventoryHost.Inventory.CountById("fuel");
                if (have <= 0) return;
                int spend = Math.Min(have, 10);
                _inventoryHost.Remove("fuel", spend);
                var refuelResult = _expeditionHost.RefuelVehicle(vehicleId, spend);
                SurfaceCommandRefusal(refuelResult, AshfallLocalization.Tr("ui.expedition.refuel_refused", "REFUEL REFUSED"));
                RefreshView();
            });
            btnRefuel.TooltipText = AshfallLocalization.Tr("ui.expedition.refuel_tooltip", "Burn 10 carried fuel items into the selected tank.");
            prepRow.AddChild(btnRefuel);

            var btnTrackGear = AshfallUiHelpers.MakeButton(AshfallLocalization.Tr("ui.expedition.fit_track_gear", "FIT TRACK GEAR"), () =>
            {
                string vehicleId = SelectedVehicleId;
                if (_expeditionHost == null || string.IsNullOrEmpty(vehicleId)) return;
                var trackResult = _expeditionHost.InstallTrackGear(vehicleId, "vehicle_track_gear_standard");
                SurfaceCommandRefusal(trackResult, AshfallLocalization.Tr("ui.expedition.track_gear_refused", "TRACK GEAR REFUSED"));
                RefreshView();
            });
            btnTrackGear.TooltipText = AshfallLocalization.Tr("ui.expedition.track_gear_tooltip", "Install the authored track-gear package, improving rough-terrain traction and reducing breakdown risk.");
            prepRow.AddChild(btnTrackGear);

            rootBox.AddChild(prepRow);

            // Expedition sub-consoles (T14): deep links to the registered
            // radar-sweep and overnight-camp surfaces. Presentation only —
            // the host resolves the routes.
            var consoleRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingMd);
            consoleRow.Alignment = BoxContainer.AlignmentMode.Center;
            var btnRadar = AshfallUiHelpers.MakeButton(AshfallLocalization.Tr("ui.expedition.radar_console", "RADAR SWEEP CONSOLE"), () => OnOpenRadarRequested?.Invoke());
            btnRadar.TooltipText = AshfallLocalization.Tr("ui.expedition.radar_tooltip", "Open the expedition radar sweep — every cataloged destination, danger band, and active sortie on one grid.");
            consoleRow.AddChild(btnRadar);
            var btnCamp = AshfallUiHelpers.MakeButton(AshfallLocalization.Tr("ui.expedition.camp_console", "OVERNIGHT CAMP CONSOLE"), () => OnOpenCampConsoleRequested?.Invoke());
            btnCamp.TooltipText = AshfallLocalization.Tr("ui.expedition.camp_tooltip", "Manage an expedition's overnight camp: firewood, rations, sentry shifts, and night segments.");
            consoleRow.AddChild(btnCamp);
            var btnRail = AshfallUiHelpers.MakeButton(AshfallLocalization.Tr("ui.expedition.rail_console", "RAILWAY LOGISTICS TERMINAL"), () => OnOpenRailwayTerminalRequested?.Invoke());
            btnRail.TooltipText = AshfallLocalization.Tr("ui.expedition.railway_tooltip", "Railway logistics terminal — branch lines, rolling stock, and evacuation capacity.");
            consoleRow.AddChild(btnRail);
            rootBox.AddChild(consoleRow);

            _estimateLabel = AshfallUiHelpers.MakeMono("");
            _estimateLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            rootBox.AddChild(_estimateLabel);

            // P105/P106 — advisory pre-dispatch checklist, supply burn, and risk.
            rootBox.AddChild(AshfallUiHelpers.MakeSectionHeader(
                AshfallLocalization.Tr("ui.expedition.prep_header", "EXPEDITION PREP // PROJECTED LOADOUT")));
            _prepLabel = AshfallUiHelpers.MakeMono("");
            _prepLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _prepLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _prepLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            rootBox.AddChild(_prepLabel);

            _dispatchStatusLabel = AshfallUiHelpers.MakeMetadata("");
            _dispatchStatusLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _dispatchStatusLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
            rootBox.AddChild(_dispatchStatusLabel);

            // ── Target Destinations ──
            var targetsTitle = AshfallUiHelpers.MakeSectionHeader(
                AshfallLocalization.Tr("ui.expedition.section.targets", "KNOWN WASTELAND DESTINATIONS"));
            rootBox.AddChild(targetsTitle);

            _targetsContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_targetsContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingMd);
            btnRow.Alignment = BoxContainer.AlignmentMode.Center;

            var btnTick = AshfallUiHelpers.MakeButton(
                AshfallLocalization.Tr("ui.expedition.advance", "ADVANCE SORTIES (2 HOURS)"), () =>
            {
                if (_expeditionHost != null)
                {
                    _expeditionHost.TickHours(2f);
                    OnExpeditionUpdated?.Invoke();
                    RefreshView();
                }
            });
            btnTick.CustomMinimumSize = new Vector2(220, 42);
            btnRow.AddChild(btnTick);

            var btnClose = AshfallUiHelpers.MakeButton(
                AshfallLocalization.Tr("ui.expedition.return_dashboard", "RETURN TO DASHBOARD [Esc]"), () => OnClose?.Invoke(), true);
            btnClose.CustomMinimumSize = new Vector2(220, 42);
            btnRow.AddChild(btnClose);
            rootBox.AddChild(btnRow);

            var hint = AshfallUiHelpers.MakeSmall(AshfallLocalization.Tr("ui.expedition.esc_hint", "Press [Esc] to return"));
            hint.HorizontalAlignment = HorizontalAlignment.Center;
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            rootBox.AddChild(hint);

            _fitnessWarningDialog = new ConfirmationDialog
            {
                Title = AshfallLocalization.Tr("ui.expedition.fitness_title", "FITNESS WARNING — EXPEDITION"),
                DialogText = string.Empty
            };
            _fitnessWarningDialog.Confirmed += ConfirmFitnessDispatch;
            _fitnessWarningDialog.Canceled += () => _pendingFitnessDispatch = null;
            AddChild(_fitnessWarningDialog);
        }

        private void DispatchWithFitnessCheck(
            string survivorId,
            string locationId,
            ExpeditionStance stance,
            int day,
            RoleFitnessVerdict? fitness)
        {
            if (_expeditionHost == null) return;

            Action dispatch = () =>
            {
                var result = _expeditionHost.DispatchSortie(
                    survivorId,
                    locationId,
                    stance,
                    day,
                    SelectedVehicleId,
                    confirmFitnessWarning: fitness?.RequiresConfirmation == true);
                if (!result.IsSuccess && _dispatchStatusLabel != null)
                {
                    // A refused dispatch must reach the screen, not vanish as a
                    // button no-op. The typed code names the actual gate.
                    _dispatchStatusLabel.Text =
                        TrFmt("ui.expedition.dispatch_refused", "DISPATCH REFUSED — {0}", FormatDispatchRefusal(result.FailureCode));
                }
                else if (_dispatchStatusLabel != null)
                {
                    _dispatchStatusLabel.Text = string.Empty;
                }
                OnExpeditionUpdated?.Invoke();
                RefreshView();
            };

            if (fitness?.RequiresConfirmation == true)
            {
                string reasons = string.Join(", ", fitness.WarningReasons).Replace('_', ' ');
                _pendingFitnessDispatch = dispatch;
                _fitnessWarningDialog!.DialogText =
                    $"{FormatSurvivorName(survivorId)} is impaired. Dispatch is allowed with explicit confirmation. " +
                    $"Recommended maximum duty: {fitness.RecommendedMaxHours:0} hours. " +
                    (string.IsNullOrEmpty(reasons) ? string.Empty : TrFmt("ui.expedition.fitness_factors", "Fitness factors: {0}", reasons + "."));
                _fitnessWarningDialog.PopupCentered();
                return;
            }

            dispatch();
        }

        private void ConfirmFitnessDispatch()
        {
            var dispatch = _pendingFitnessDispatch;
            _pendingFitnessDispatch = null;
            dispatch?.Invoke();
        }

        private static string FormatSurvivorName(string id)
        {
            if (string.IsNullOrEmpty(id)) return AshfallLocalization.Tr("ui.expedition.unnamed", "[UNNAMED]");
            return id switch
            {
                "survivor_dr_sarah_chen" or "survivor_sarah_chen" => "Dr. Sarah Chen",
                "survivor_gunner_mikhail" or "survivor_mikhail_volkov" => "Gunner Mikhail",
                "elena_vasquez" or "survivor_elena_vasquez" => "Elena Vasquez",
                _ => id.Replace("survivor_", "").Replace("_", " ").ToUpperInvariant()
            };
        }

        private static string FormatFactionName(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return AshfallLocalization.Tr("ui.expedition.unknown_faction", "UNKNOWN FACTION");
            // Presentation retains the authored lore ID. Canonicalization is
            // for standing/state authorities; mapping it here would turn an
            // Iron Garrison patrol into the unrelated display era name.
            string resolved = FactionDisplayNameCatalog.Resolve(id);
            return resolved.ToUpperInvariant();
        }

        private static string HumanizeDisplayToken(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return string.Empty;
            string[] parts = id.Split('_', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Length == 0) continue;
                parts[i] = char.ToUpperInvariant(parts[i][0]) + (parts[i].Length > 1 ? parts[i][1..] : string.Empty);
            }
            return string.Join(" ", parts).ToUpperInvariant();
        }

        private static string FormatPatrolToken(string id)
        {
            return string.IsNullOrWhiteSpace(id) ? AshfallLocalization.Tr("ui.expedition.patrol_token", "PATROL") : HumanizeDisplayToken(id);
        }

        /// <summary>Localize + format a string without throwing on a malformed
        /// catalog row (falls back to the raw template).</summary>
        private static string TrFmt(string key, string fallback, params object[] args)
        {
            string template = AshfallUiText.Tr(key, fallback);
            try { return string.Format(template, args); }
            catch (FormatException) { return template; }
        }

        private static string FormatUnavailableReason(string code)
        {
            return code switch
            {
                "cost_unavailable" => AshfallLocalization.Tr("ui.expedition.unavailable.cost", "Cost unavailable"),
                "required_item_missing" => AshfallLocalization.Tr("ui.expedition.unavailable.required_item", "Required item unavailable"),
                "required_condition_missing" => AshfallLocalization.Tr("ui.expedition.unavailable.required_condition", "Required condition unmet"),
                "recognition_requirement_unmet" => AshfallLocalization.Tr("ui.expedition.unavailable.recognition", "Recognition requirement unmet"),
                "standing_requirement_unmet" => AshfallLocalization.Tr("ui.expedition.unavailable.standing", "Standing requirement unmet"),
                _ => AshfallLocalization.Tr("ui.expedition.unavailable.generic", "Unavailable")
            };
        }

        /// <summary>Player-facing wording for a refused sortie, keyed by the
        /// stable Core failure code (see ACTION_RESULT_SURFACING_MATRIX.md).</summary>
        private static string FormatDispatchRefusal(string code)
        {
            return code switch
            {
                "unmapped" => AshfallLocalization.Tr("ui.expedition.refusal.unmapped", "no route knowledge for this sector. Survey or chart it first."),
                "crossing_closed" => AshfallLocalization.Tr("ui.expedition.refusal.crossing_closed", "crossing gate closed. No vouch authorization to traverse."),
                "route_blocked" => AshfallLocalization.Tr("ui.expedition.refusal.route_blocked", "route blocked by a weather or standing gate."),
                "vehicle_unready" => AshfallLocalization.Tr("ui.expedition.refusal.vehicle_unready", "vehicle unready. Check fuel and condition in the garage."),
                "fitness_blocked" => AshfallLocalization.Tr("ui.expedition.refusal.fitness_blocked", "survivor is unfit for expedition duty."),
                "fitness_warning_confirmation_required" => AshfallLocalization.Tr("ui.expedition.refusal.fitness_warning_confirmation_required", "survivor is impaired and needs explicit confirmation."),
                "unknown_target" => AshfallLocalization.Tr("ui.expedition.refusal.unknown_target", "unknown destination."),
                "stale_preview" => AshfallLocalization.Tr("ui.expedition.refusal.stale_preview", "planning data went stale. Retry the dispatch."),
                // P003 + unknown codes route through the shared refusal
                // formatter, so the tutorial-ordering gate names its pending
                // step instead of printing a bare de-underscored code.
                _ => ActionRefusalText.Describe(code)
            };
        }

        /// <summary>T10 — surface a refused vehicle-prep command through the same
        /// status line the dispatch refusal uses. An abandoned CommandResult must
        /// not look like a successful click.</summary>
        private void SurfaceCommandRefusal(Ashfall.Core.PlayerCommand.CommandResult result, string prefix)
        {
            if (_dispatchStatusLabel == null) return;
            _dispatchStatusLabel.Text = result.IsSuccess
                ? string.Empty
                : $"{prefix} — {ActionRefusalText.Describe(result.FailureCode)}";
        }

        public void RefreshView()
        {
            if (_activeContainer == null || _targetsContainer == null || _expeditionHost == null) return;
            var host = _expeditionHost;

            // P107 follow-up — the host session retains the last ceremony/aftermath,
            // so a panel never bound when the sortie ended (or rebound after a
            // newer sortie) still shows the current text, not a stale one.
            if (!string.IsNullOrEmpty(host.LastReturnSummary) &&
                !string.Equals(LastReturnSummary, host.LastReturnSummary, StringComparison.Ordinal))
                SetReturnSummary(host.LastReturnSummary, success: !host.LastReturnWasFailure);

            // Clear Containers
            AshfallUiHelpers.EmptyChildren(_activeContainer);
            AshfallUiHelpers.EmptyChildren(_targetsContainer);

            // 1. Render Active Expeditions
            if (host.Engine.ActiveCount == 0)
            {
                _activeContainer.AddChild(AshfallUiHelpers.MakeMetadata(
                    AshfallLocalization.Tr("ui.expedition.no_active", "No active scavenging sorties currently deployed.")));
            }
            else
            {
                foreach (var kv in _expeditionHost.Engine.Active)
                {
                    var exp = kv.Value;
                    if (exp == null) continue;

                    var card = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
                    var topRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);

                    var phaseName = ExpeditionPhaseText.Label((ExpeditionPhase)exp.phase);
                    var lblPhase = AshfallUiHelpers.MakeMono($"[{phaseName}] {exp.displayName}");
                    lblPhase.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                    lblPhase.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    topRow.AddChild(lblPhase);

                    var lblScout = AshfallUiHelpers.MakeSmall(TrFmt(
                        "ui.expedition.scout", "SCOUT: {0}", FormatSurvivorName(exp.survivorId)));
                    topRow.AddChild(lblScout);

                    var lblStamina = AshfallUiHelpers.MakeMono(TrFmt(
                        "ui.expedition.stamina", "STAMINA {0:0}%", exp.stamina));
                    lblStamina.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(exp.stamina < 30 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Hot));
                    topRow.AddChild(lblStamina);
                    card.AddChild(topRow);

                    var midRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                    var progress = AshfallUiHelpers.MakeSmall(TrFmt(
                        "ui.expedition.progress_line",
                        "Travel Progress: {0}/{1} legs · Encounters: {2} · Loot: {3} items ({4}/{5} kg)",
                        exp.travelTicksCompleted, exp.distanceTicks, exp.encounterCount, exp.loot?.Count ?? 0,
                        exp.currentWeightKg.ToString("F1"), exp.maxLootCapacityKg.ToString("F0")));
                    midRow.AddChild(progress);
                    card.AddChild(midRow);

                    // Action Controls for Active Expedition
                    var actionRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                    string scoutId = exp.survivorId;

                    if (exp.phase == (int)ExpeditionPhase.Looting)
                    {
                        var btnPush = AshfallUiHelpers.MakeButton(
                            AshfallLocalization.Tr("ui.expedition.push_luck", "PUSH LUCK (SCAVENGE DEEPER)"), () =>
                        {
                            _expeditionHost.PushLuck(scoutId);
                            OnExpeditionUpdated?.Invoke();
                            RefreshView();
                        });
                        btnPush.CustomMinimumSize = new Vector2(230, 30);
                        actionRow.AddChild(btnPush);

                        var btnRetreat = AshfallUiHelpers.MakeButton(
                            AshfallLocalization.Tr("ui.expedition.order_return", "ORDER INBOUND RETURN"), () =>
                        {
                            _expeditionHost.Retreat(scoutId);
                            OnExpeditionUpdated?.Invoke();
                            RefreshView();
                        });
                        btnRetreat.CustomMinimumSize = new Vector2(200, 30);
                        actionRow.AddChild(btnRetreat);
                    }
                    else
                    {
                        var lblTransit = AshfallUiHelpers.MakeMetadata(exp.phase == (int)ExpeditionPhase.Outbound
                            ? AshfallLocalization.Tr("ui.expedition.transit_outbound", "In transit toward objective...")
                            : AshfallLocalization.Tr("ui.expedition.transit_inbound", "Returning to shelter with salvage..."));
                        actionRow.AddChild(lblTransit);
                    }

                    card.AddChild(actionRow);

                    var panel = AshfallUiHelpers.MakePanel();
                    panel.AddChild(card);
                    _activeContainer.AddChild(panel);
                }
            }

            // 2. Render Pending Surfaced Encounters
            RenderPendingList();

            // 3. Render Available Targets
            var livingSurvivors = new List<string>();
            if (_survivorsHost != null)
            {
                foreach (var s in _survivorsHost.RosterState)
                {
                    if (s != null && s.IsAliveState && !_expeditionHost.Engine.Active.ContainsKey(s.Id))
                        livingSurvivors.Add(s.Id);
                }
            }
            if (livingSurvivors.Count == 0 && _survivorsHost != null)
            {
                livingSurvivors.Add("survivor_gunner_mikhail");
            }

            RebuildDispatchSelectors();
            string? chosen = ChosenSurvivor(livingSurvivors);
            UpdateEstimateLine(chosen);

            foreach (var def in _expeditionHost.Definitions)
            {
                if (def == null) continue;

                var card = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
                var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);

                var title = AshfallUiHelpers.MakeSectionHeader(def.displayName);
                title.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                row.AddChild(title);

                var danger = AshfallUiHelpers.MakeMono(TrFmt(
                    "ui.expedition.danger", "DANGER: LVL {0} · DISTANCE: {1} LEGS",
                    def.dangerLevel, def.distanceTicks));
                danger.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(def.dangerLevel >= 3 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Warm));
                row.AddChild(danger);
                card.AddChild(row);

                var lootCategories = string.Join(", ", def.lootCategories);
                var desc = AshfallUiHelpers.MakeBody(TrFmt(
                    "ui.expedition.target_line",
                    "Potential Salvage: {0} · Encounter Risk: {1}/hr",
                    lootCategories, def.encounterChancePerTick.ToString("P0")));
                desc.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                card.AddChild(desc);

                // Task 122: live world state — ownership, spoilage, ruin, threats.
                var (worldLine, worldSevere) = BuildWorldStateLine(def.id);
                if (!string.IsNullOrEmpty(worldLine))
                {
                    var worldLabel = AshfallUiHelpers.MakeMono(worldLine);
                    worldLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(
                        worldSevere
                            ? Ashfall.Core.UI.Theme.Critical
                            : Ashfall.Core.UI.Theme.Pale));
                    card.AddChild(worldLabel);
                }

                string defId = def.id;
                bool blocked = _expeditionHost.IsLocationBlocked(defId);
                RoleFitnessVerdict? dispatchRoleFitness = chosen != null
                    ? _expeditionHost.GetExpeditionFitness(chosen)
                    : null;
                FitnessVerdict? dispatchFitness = chosen != null
                    ? _expeditionHost.GetSurvivorFitness(chosen)
                    : null;
                bool fitnessBlocked = dispatchRoleFitness != null
                    ? !dispatchRoleFitness.Allowed
                    : dispatchFitness != null && dispatchFitness.Level >= FitnessLevel.Unfit;

                if (dispatchRoleFitness != null || dispatchFitness != null)
                {
                    var level = dispatchRoleFitness?.BaseVerdict.Level ?? dispatchFitness!.Level;
                    float maximumHours = dispatchRoleFitness?.RecommendedMaxHours
                        ?? dispatchFitness!.RecommendedMaxHours;
                    string fitnessState = fitnessBlocked
                        ? AshfallLocalization.Tr("ui.expedition.fitness.unfit", "FITNESS: UNFIT — dispatch blocked")
                        : level == FitnessLevel.Fit
                        ? AshfallLocalization.Tr("ui.expedition.fitness.fit", "FITNESS: FIT")
                        : TrFmt("ui.expedition.fitness.impaired", "FITNESS: IMPAIRED — allowed, max {0:0}h", maximumHours);
                    var fitnessLabel = AshfallUiHelpers.MakeMono(fitnessState);
                    fitnessLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(
                        fitnessBlocked ? Ashfall.Core.UI.Theme.Critical :
                        level == FitnessLevel.Impaired ? Ashfall.Core.UI.Theme.Warm :
                        Ashfall.Core.UI.Theme.Lethe));
                    fitnessLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
                    fitnessLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    card.AddChild(fitnessLabel);
                    var reasons = fitnessBlocked
                        ? dispatchRoleFitness?.BlockingReasons
                        : dispatchRoleFitness?.WarningReasons;
                    if (reasons != null && reasons.Count > 0)
                    {
                        var reasonLabel = AshfallUiHelpers.MakeSmall(
                            TrFmt("ui.expedition.fitness_factors", "Fitness factors: {0}",
                                string.Join(", ", reasons).Replace('_', ' ')), autowrap: true);
                        reasonLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                        card.AddChild(reasonLabel);
                    }
                }

                // Dispatch Bar
                var dispatchRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);

                string? blockReason = blocked ? _expeditionHost.GetBlockReason(defId) : null;
                if (blocked)
                {
                    // Name the actual gate (fog, clues, map, weather) instead of one
                    // hardcoded crossing message — a fogged node must say so.
                    var gateLabel = AshfallUiHelpers.MakeMono(
                        $"[{(blockReason ?? "dispatch blocked").ToUpperInvariant()}]");
                    gateLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
                    gateLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
                    gateLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    dispatchRow.AddChild(gateLabel);
                }

                var btnEstimate = AshfallUiHelpers.MakeButton(
                    AshfallLocalization.Tr("ui.expedition.estimate", "ESTIMATE"), () =>
                {
                    // Focus the risk estimate on this card's destination; the
                    // global estimate line otherwise describes a default target.
                    _selectedTargetId = defId;
                    UpdateEstimateLine(chosen);
                });
                btnEstimate.TooltipText = AshfallLocalization.Tr("ui.expedition.estimate_tooltip", "Show fuel, dose, breakdown, and encounter estimates for this destination.");
                btnEstimate.CustomMinimumSize = new Vector2(110, 32);
                dispatchRow.AddChild(btnEstimate);

                var btnDispatchStealth = AshfallUiHelpers.MakeButton(
                    AshfallLocalization.Tr("ui.expedition.dispatch_stealth", "DISPATCH STEALTH SORTIE"), () =>
                {
                    if (chosen != null)
                        DispatchWithFitnessCheck(
                            chosen, defId, ExpeditionStance.Stealth, 1, dispatchRoleFitness);
                });
                btnDispatchStealth.Disabled = blocked || fitnessBlocked || chosen == null || _expeditionHost.Engine.Active.ContainsKey(chosen);
                if (blocked && blockReason != null)
                    btnDispatchStealth.TooltipText = TrFmt("ui.expedition.dispatch_blocked", "Dispatch blocked: {0}", blockReason);
                btnDispatchStealth.CustomMinimumSize = new Vector2(200, 32);
                dispatchRow.AddChild(btnDispatchStealth);

                var btnDispatchSpeed = AshfallUiHelpers.MakeButton(
                    AshfallLocalization.Tr("ui.expedition.dispatch_speed", "DISPATCH SPEED SORTIE (1.5x)"), () =>
                {
                    if (chosen != null)
                        DispatchWithFitnessCheck(
                            chosen, defId, ExpeditionStance.Speed, 1, dispatchRoleFitness);
                });
                btnDispatchSpeed.Disabled = blocked || fitnessBlocked || chosen == null || _expeditionHost.Engine.Active.ContainsKey(chosen);
                if (blocked && blockReason != null)
                    btnDispatchSpeed.TooltipText = TrFmt("ui.expedition.dispatch_blocked", "Dispatch blocked: {0}", blockReason);
                btnDispatchSpeed.CustomMinimumSize = new Vector2(220, 32);
                dispatchRow.AddChild(btnDispatchSpeed);

                card.AddChild(dispatchRow);

                var panel = AshfallUiHelpers.MakePanel();
                panel.AddChild(card);
                _targetsContainer.AddChild(panel);
            }
        }

        /// <summary>Live evolving-world line for a target location, or null when
        /// untouched ground. <c>Severe</c> is derived from the record flags, not
        /// from the rendered text, so localization cannot change the color.</summary>
        private (string? Text, bool Severe) BuildWorldStateLine(string locationId)
        {
            if (_worldHost == null || string.IsNullOrEmpty(locationId)) return (null, false);
            var rec = _worldHost.LocationEvolution?.TryGetRecord(locationId);
            string? flavor = _worldHost.FlavorTextForLocation(locationId, _worldHost.Weather?.Current.ToString());

            if (rec == null)
            {
                return string.IsNullOrEmpty(flavor) ? (null, false) : (TruncateFlavor(flavor), false);
            }

            string owner = rec.currentOwner == "none"
                ? AshfallLocalization.Tr("ui.expedition.world_unclaimed", "unclaimed")
                : rec.currentOwner.Replace("faction_", "");
            string state = rec.isRuined ? " " + AshfallLocalization.Tr("ui.expedition.world_ruined", "· RUINED") : string.Empty;
            string threats = rec.activeThreats.Count > 0
                ? " " + AshfallLocalization.TrFormat("ui.expedition.world_threats", rec.activeThreats.Count)
                : string.Empty;
            string line = TrFmt("ui.expedition.world_line", "WORLD: {0} · {1} spoilage{2}{3}",
                owner, rec.lootDepletionFactor.ToString("P0"), state, threats);
            if (!string.IsNullOrEmpty(flavor))
                line += "\n" + TruncateFlavor(flavor);
            return (line, rec.isRuined || rec.activeThreats.Count > 0);
        }

        private static string TruncateFlavor(string flavor)
        {
            const int max = 160;
            if (flavor.Length <= max) return flavor;
            return flavor.Substring(0, max - 1) + "…";
        }

        // ── Dispatch preparation helpers ──────────────────────────────

        /// <summary>
        /// Rebuild the vehicle/weapon selectors from the garage and the
        /// equipment authority, preserving the current choice when possible.
        /// </summary>
        private void RebuildDispatchSelectors()
        {
            if (_expeditionHost == null) return;

            // P105 follow-up — the survivor selector lists living, non-active
            // survivors so the checklist reflects who is actually going.
            if (_survivorSelect != null)
            {
                string previous = SelectedSurvivorId;
                _survivorSelect.Clear();
                _survivorIds.Clear();
                if (_survivorsHost != null)
                {
                    foreach (var s in _survivorsHost.RosterState)
                    {
                        if (s == null || !s.IsAliveState) continue;
                        if (_expeditionHost.Engine.Active.ContainsKey(s.Id)) continue;
                        _survivorIds.Add(s.Id);
                        _survivorSelect.AddItem(FormatSurvivorName(s.Id), _survivorSelect.ItemCount);
                    }
                }
                if (_survivorSelect.ItemCount > 0)
                {
                    int restoreIdx = _survivorIds.IndexOf(previous);
                    _survivorSelect.Select(restoreIdx >= 0 ? restoreIdx : 0);
                }
            }

            if (_vehicleSelect != null)
            {
                string previous = SelectedVehicleId;
                _vehicleSelect.Clear();
                _vehicleIds.Clear();
                _vehicleSelect.AddItem(AshfallLocalization.Tr("ui.expedition.vehicle_foot", "On foot"), 0);
                foreach (var v in _expeditionHost.Vehicles.State.ownedVehicles.Values)
                {
                    if (v == null || string.IsNullOrEmpty(v.vehicleId)) continue;
                    _vehicleIds.Add(v.vehicleId);
                    _vehicleSelect.AddItem(
                        TrFmt("ui.expedition.vehicle_entry", "{0} · fuel {1:0}/{2:0} · cond {3:0}%",
                            v.displayName, v.fuel, v.maxFuel, v.condition) +
                        (v.isBrokenDown ? " " + AshfallLocalization.Tr("ui.expedition.vehicle_broken", "BROKEN") : ""),
                        _vehicleSelect.ItemCount);
                }
                int restoreIdx = _vehicleIds.IndexOf(previous);
                _vehicleSelect.Select(restoreIdx >= 0 ? restoreIdx + 1 : 0);
            }

            if (_weaponSelect != null)
            {
                string previous = SelectedWeaponInstanceId;
                _weaponSelect.Clear();
                _weaponInstanceIds.Clear();
                _weaponSelect.AddItem(AshfallLocalization.Tr("ui.expedition.weapon_sidearm", "Sidearm only"), 0);
                if (_equipment?.State?.items != null)
                {
                    foreach (var item in _equipment.State.items)
                    {
                        if (item == null || item.family != Ashfall.Core.EquipmentFamily.Weapon) continue;
                        if (!Ashfall.Core.Combat.WeaponEquipmentBridge.Readiness(_equipment, item.instanceId).Equals(0f))
                        {
                            _weaponInstanceIds.Add(item.instanceId);
                            _weaponSelect.AddItem($"{item.itemId} · cond {item.condition:F0}%", _weaponSelect.ItemCount);
                        }
                    }
                }
                int restoreW = _weaponInstanceIds.IndexOf(previous);
                _weaponSelect.Select(restoreW >= 0 ? restoreW + 1 : 0);
            }
        }

        /// <summary>Live estimate line for the current selection (first dispatchable survivor).</summary>
        private void UpdateEstimateLine(string? survivorId = null)
        {
            if (_estimateLabel == null || _expeditionHost == null) return;
            if (_vehicleSelect == null || _weaponSelect == null) return;
            if (_expeditionHost.Definitions.Count == 0) return;

            string targetId = _selectedTargetId;
            var def = _expeditionHost.Definitions.Find(d => d.id == targetId) ?? _expeditionHost.Definitions[0];

            string weaponInstance = SelectedWeaponInstanceId;
            float readiness = Ashfall.Core.Combat.WeaponEquipmentBridge.Readiness(_equipment, weaponInstance);
            float jam = Ashfall.Core.Combat.WeaponEquipmentBridge.JamRisk(_equipment, weaponInstance);

            var preview = _expeditionHost.EstimateExpedition(def.id, ExpeditionStance.Stealth, SelectedVehicleId, readiness, jam,
                survivorId: survivorId ?? SelectedSurvivorId);
            if (preview == null)
            {
                _estimateLabel.Text = AshfallLocalization.Tr("ui.expedition.no_route", "NO ROUTE DATA.");
                return;
            }

            var (est, fuelOk) = preview.Value;
            string vehiclePart = est.usingVehicle
                ? TrFmt("ui.expedition.estimate_vehicle", "by {0}", _expeditionHost.Vehicles.GetVehicle(SelectedVehicleId)?.displayName ?? est.locationId)
                : AshfallLocalization.Tr("ui.expedition.estimate_on_foot", "on foot");
            _estimateLabel.Text = TrFmt("ui.expedition.estimate_line",
                "ESTIMATE [{0} · {1}] ticks {2:F0} (out {3:F0} / loot {4:F0} / in {5:F0}) · cargo {6:F0} kg · fuel need {7:F1}{8} · breakdown {9:P0} · encounter {10:P0}/hr · weapon readiness {11:P0}{12}",
                def.displayName,
                vehiclePart,
                est.totalTicks,
                est.outboundTicks,
                est.lootingTicks,
                est.inboundTicks,
                est.cargoCapacityKg,
                est.fuelRequired,
                fuelOk ? string.Empty : " " + AshfallLocalization.Tr("ui.expedition.estimate_tank_low", "— TANK LOW"),
                est.breakdownRiskTotal,
                est.encounterRiskPerTick,
                est.weaponReadiness,
                jam > 0f ? " " + TrFmt("ui.expedition.estimate_jam", "(jam {0:P0})", jam) : string.Empty);

            // C2 / Plan 21C (P6/§34) — warn, do not silently block: projected
            // dose and mid-route gear failure are displayed from the canonical
            // estimate; the player keeps full dispatch agency.
            if (est.projectedDoseTotal > 0f || est.unprotectedCount > 0)
            {
                string protection = est.partyProtection > 0f
                    ? TrFmt("ui.expedition.estimate_protection", "protection {0:0.#}", est.partyProtection)
                    : AshfallLocalization.Tr("ui.expedition.estimate_no_protection", "NO WORKING PROTECTION");
                string unprotected = est.unprotectedCount > 0
                    ? TrFmt("ui.expedition.estimate_unprotected", ", {0} unprotected", est.unprotectedCount)
                    : string.Empty;
                _estimateLabel.Text += " " + TrFmt("ui.expedition.estimate_dose",
                    "· dose ~{0:F0} mSv ({1}{2})", est.projectedDoseTotal, protection, unprotected);
            }
            if (est.predictsMidRouteFailure)
            {
                _estimateLabel.Text += " " + TrFmt("ui.expedition.estimate_gear_fail",
                    "· GEAR FAILS MID-ROUTE (~{0:F0} h < {1:F0} h trip)", est.protectiveLifeHours, est.projectedTripHours);
            }

            // P105/P106 — advisory prep projection over the live shelter inventory.
            var prepLabel = _prepLabel;
            if (prepLabel != null)
            {
                int Count(string itemId) => _inventoryHost?.Inventory?.CountById(itemId) ?? 0;
                var prep = ExpeditionPrepPlanner.Build(
                    def, est, Count,
                    weaponReady: readiness > 0f,
                    hasLight: Count(ExpeditionPrepPlanner.LightItemId) > 0);
                string overnight = prep.OvernightLine();
                prepLabel.Text = string.IsNullOrEmpty(overnight)
                    ? prep.Summary()
                    : prep.Summary() + "\n" + overnight;
                prepLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(
                    prep.Ready ? Ashfall.Core.UI.Theme.Lethe : Ashfall.Core.UI.Theme.LetheAmber));
            }
        }

        // ── Pending surfaced encounters ────────────────────────────────

        /// <summary>
        /// Renders NarrativeEncounterState.pending as selectable rows so a stack
        /// of surfaced encounters from one trip can be worked through without
        /// modal-spam. Hidden when the queue is empty. Readable without colour:
        /// [#N] prefix + uppercase label + panel border.
        /// </summary>
        private void RenderPendingList()
        {
            if (_pendingContainer == null || _expeditionHost == null) return;

            AshfallUiHelpers.EmptyChildren(_pendingContainer);

            var pending = _expeditionHost.Pending;
            bool any = pending != null && pending.Count > 0;
            _pendingContainer.Visible = any;
            if (_pendingHeader != null) _pendingHeader.Visible = any;
            if (!any) return;

            for (int i = 0; i < pending!.Count; i++)
            {
                var p = pending[i];
                if (p == null || string.IsNullOrEmpty(p.encounterId)) continue;

                var def = _expeditionHost.FindEncounter(p.encounterId);
                string label = def != null && !string.IsNullOrEmpty(def.title)
                    ? def.title.ToUpperInvariant()
                    : TrFmt("ui.expedition.encounter_pending", "ENCOUNTER #{0}", p.legIndex);

                var card = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
                var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);

                var lbl = AshfallUiHelpers.MakeMono($"[#{p.legIndex}] {label}");
                lbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                row.AddChild(lbl);

                var meta = AshfallUiHelpers.MakeSmall($"{p.locationId} · {TrFmt("ui.expedition.pending_day", "DAY {0}", p.day)}");
                row.AddChild(meta);

                string pendingId = p.encounterId;
                string pendingLocation = p.locationId;
                int pendingLeg = p.legIndex;
                var btnResolve = AshfallUiHelpers.MakeButton(
                    AshfallLocalization.Tr("ui.expedition.resolve", "RESOLVE"), () =>
                {
                    OpenPendingEncounter(pendingId, pendingLocation, pendingLeg);
                });
                btnResolve.CustomMinimumSize = new Vector2(120, 30);
                row.AddChild(btnResolve);

                card.AddChild(row);

                var panel = AshfallUiHelpers.MakePanel();
                panel.AddChild(card);
                _pendingContainer.AddChild(panel);
            }

            var footer = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            var btnDismissAll = AshfallUiHelpers.MakeButton(
                AshfallLocalization.Tr("ui.expedition.dismiss_all", "DISMISS ALL"), () =>
            {
                int count = _expeditionHost.Pending?.Count ?? 0;
                _expeditionHost.ClearAllPending();
                GD.Print($"[Expedition] Dismissed {count} pending encounter(s) without resolving.");
                RefreshView();
            }, true);
            btnDismissAll.CustomMinimumSize = new Vector2(160, 30);
            footer.AddChild(btnDismissAll);
            _pendingContainer.AddChild(footer);
        }

        /// <summary>
        /// Batch mode: queue exactly this pending encounter into the existing
        /// modal and show it. Text is verbatim from the catalog; when the catalog
        /// has no record we say so rather than inventing one.
        /// </summary>
        private void OpenPendingEncounter(string encounterId, string locationId, int legIndex)
        {
            if (_expeditionHost == null || string.IsNullOrEmpty(encounterId)) return;

            var def = _expeditionHost.FindEncounter(encounterId);
            var trigger = new ExpeditionState
            {
                survivorId = string.Empty,
                locationId = locationId ?? string.Empty,
                displayName = locationId ?? string.Empty,
                phase = (int)ExpeditionPhase.Outbound,
                encounterCount = legIndex
            };

            var dto = new ExpeditionEncounterBridge.EncounterSurfaced
            {
                encounter_id = encounterId,
                trigger = trigger,
                resolved_at_lead = null,
                encounter_record_resolution_id = null!
            };

            if (def == null)
            {
                dto.title = "Encounter #" + legIndex;
                dto.description = "This encounter is pending, but the catalog holds no record of it.";
                dto.category = string.Empty;
                dto.choices = new List<Ashfall.Core.Narrative.EncounterChoiceDefinition>();
                dto.resolved_at_lead = false;
                dto.is_micro_location = false;
            }
            else
            {
                dto.title = def.title;
                dto.description = def.description;
                dto.category = def.category;
                dto.choices = def.choices ?? new List<Ashfall.Core.Narrative.EncounterChoiceDefinition>();
                dto.is_micro_location = def.isMicroLocation;
            }

            _encounterQueue.Clear();
            _encounterQueue.Enqueue(dto);
            _pendingBatchMode = true;
            ShowNextModal();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
            QueueRedraw();
        }
        public void Close()
        {
            _encounterQueue.Clear();
            _modalActive = false;
            _bannerTimer = 0f;
            if (_encounterModal != null) _encounterModal.Visible = false;
            if (_encounterBanner != null) _encounterBanner.Visible = false;
            _lastSurfaced = null;
            _pendingBatchMode = false;
            Visible = false;
            OnClose?.Invoke();
        }

        // ── Encounter surface ──────────────────────────────────────────

        /// <summary>
        /// Total encounter notices delivered to this panel (observability / UI
        /// tests). Incremented exactly once per <see cref="ShowEncounterNotice"/>
        /// call regardless of modal vs banner mode, so a double-subscribed host
        /// handler shows up as a count above the surfaced-encounter total.
        /// </summary>
        public int TotalEncounterNotices { get; private set; }

        /// <summary>True when the current resolvable encounter's choice buttons were
        /// rendered into the modal card (observability / UI tests for the modal
        /// card-index fix).</summary>
        public bool ChoiceButtonsRendered => _choicesContainer != null;

        /// <summary>Entry point from Main when Core rolls an encounter.</summary>
        public void ShowEncounterNotice(ExpeditionEncounterBridge.EncounterSurfaced surfaced)
        {
            if (surfaced == null) return;
            TotalEncounterNotices++;
            if (!Visible)
            {
                return; // headless/closed panel: diegetic notice surfaced but not shown
            }

            if (ExpeditionHostSession.UseEncounterModal)
            {
                _encounterQueue.Enqueue(surfaced);
                if (!_modalActive) ShowNextModal();
            }
            else
            {
                ShowAutoplayBanner(surfaced);
            }
        }

        private void ShowNextModal()
        {
            if (_encounterQueue.Count == 0)
            {
                _modalActive = false;
                if (_encounterModal != null) _encounterModal.Visible = false;
                AshfallFocusPolicy.FocusFirstDeferred(this);
                return;
            }

            _modalActive = true;
            _lastSurfaced = _encounterQueue.Dequeue();
            BuildEncounterModal();
            if (_encounterModal == null) return;

            string encounterId = _lastSurfaced!.encounter_id ?? string.Empty;
            string titleKey = $"discovery.{encounterId}.title";
            string localizedTitle = AshfallLocalization.Tr(titleKey, _lastSurfaced!.title);

            if (_encounterTitle != null) _encounterTitle.Text = localizedTitle;
            if (_encounterContext != null && _encounterFactionEmblem != null)
            {
                bool isPatrol = _lastSurfaced!.is_patrol;
                bool isMicro = _lastSurfaced!.is_micro_location;
                if (isPatrol)
                {
                    _encounterContext.Visible = true;
                    _encounterFactionEmblem.Visible = true;
                    string faction = FormatFactionName(_lastSurfaced!.faction_id);
                    string archetype = FormatPatrolToken(_lastSurfaced!.patrol_archetype);
                    string territory = FormatPatrolToken(_lastSurfaced!.territory_state);
                    string recognition = string.IsNullOrWhiteSpace(_lastSurfaced!.recognition_label)
                        ? AshfallLocalization.Tr("ui.expedition.no_prior_contact", "No prior contact")
                        : _lastSurfaced!.recognition_label;
                    _encounterContext.Text = $"{faction} · {archetype} · {territory} · {recognition}";
                    _encounterFactionEmblem.Texture = AshfallUiHelpers.MakeFactionEmblem(_lastSurfaced!.faction_id, 42).Texture;
                }
                else if (isMicro)
                {
                    _encounterContext.Visible = true;
                    _encounterFactionEmblem.Visible = false;
                    _encounterContext.Text = AshfallLocalization.Tr("ui.expedition.micro_location", "DISCOVERY · MICRO-LOCATION");
                }
                else
                {
                    _encounterContext.Visible = false;
                    _encounterFactionEmblem.Visible = false;
                }
            }
            if (_encounterBody != null && _lastSurfaced != null)
            {
                string descKey = $"discovery.{encounterId}.description";
                string localizedDesc = AshfallLocalization.Tr(descKey, _lastSurfaced!.description);

                if (_lastSurfaced!.resolved_at_lead == false)
                {
                    // Bare notice: honest text, no invented outcome.
                    _encounterBody.Text = localizedDesc;
                }
                else
                {
                    string phase = ExpeditionPhaseText.Label((ExpeditionPhase)_lastSurfaced!.trigger.phase);
                    string categoryLine = _lastSurfaced!.is_micro_location
                        ? "DISCOVERY · MICRO-LOCATION"
                        : $"{_lastSurfaced!.category} · {phase} · encounter #{_lastSurfaced!.trigger.encounterCount}";

                    _encounterBody.Text = string.Join("\n",
                        FormatSurvivorName(_lastSurfaced!.trigger.survivorId) + " at " + _lastSurfaced!.trigger.displayName,
                        categoryLine,
                        "",
                        localizedDesc);
                }
            }

            RenderChoiceButtons();
            _encounterModal.Visible = true;
            // Nested overlay: never routed through ShowPanelLifecycle, so it
            // needs its own initial focus (a11y pkg 19 — pkg-12 regression
            // class; keyboard players previously had to click first).
            AshfallFocusPolicy.OpenWithFocus(_encounterModal, opener: this);
        }

        private void RenderChoiceButtons()
        {
            if (_choicesContainer != null)
            {
                _choicesContainer.QueueFree();
                _choicesContainer = null;
            }

            if (_lastSurfaced == null || _lastSurfaced!.resolved_at_lead == false || _lastSurfaced!.choices == null || _lastSurfaced!.choices.Count == 0)
            {
                // Bare notice or no choices: OK / Decide Later only.
                return;
            }

            if (_encounterModal == null) return;
            // Modal layout: child 0 = backdrop (ColorRect, no children), child 1 =
            // center (CenterContainer) whose child 0 is the card (VBoxContainer).
            var card = _encounterModal.GetChild(1)?.GetChild(0); // center -> card
            if (card is not VBoxContainer vbox) return;

            _choicesContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            _choicesContainer.AddChild(AshfallUiHelpers.MakeSeparator());
            _choicesContainer.AddChild(AshfallUiHelpers.MakeSectionHeader(
                AshfallLocalization.Tr("ui.expedition.tactical_header", "TACTICAL APPROACH SELECTION")));

            var choiceScroll = new ScrollContainer
            {
                CustomMinimumSize = new Vector2(0, 190),
                SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
                SizeFlagsVertical = Control.SizeFlags.ExpandFill,
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
                VerticalScrollMode = ScrollContainer.ScrollMode.Auto
            };
            var choiceList = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            choiceList.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            choiceScroll.AddChild(choiceList);

            float danger = _lastSurfaced?.trigger?.dangerLevel ?? 5f;
            string stance = _lastSurfaced?.trigger?.stance ?? "Balanced";

            var inv = _expeditionHost?.ShelterInventory ?? _inventoryHost?.Inventory;

            foreach (var c in _lastSurfaced!.choices)
            {
                string choiceId = c.choiceId;
                string choiceKey = $"discovery.{_lastSurfaced!.encounter_id}.choice.{choiceId}";
                string choiceText = AshfallLocalization.Tr(choiceKey, c.text);

                // Tactical assessment derivation
                string riskTag = danger >= 8 ? AshfallLocalization.Tr("ui.expedition.risk.extreme", "EXTREME RISK")
                    : danger >= 5 ? AshfallLocalization.Tr("ui.expedition.risk.high", "HIGH RISK")
                    : danger >= 3 ? AshfallLocalization.Tr("ui.expedition.risk.moderate", "MODERATE RISK")
                    : AshfallLocalization.Tr("ui.expedition.risk.low", "LOW RISK");
                var riskColor = danger >= 8 ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical)
                    : danger >= 5 ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Entropy)
                    : danger >= 3 ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.LetheAmber)
                    : AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe);

                var choiceCard = AshfallUiHelpers.MakeVBox(2);
                choiceCard.CustomMinimumSize = new Vector2(210, 100);

                var headerLabel = AshfallUiHelpers.MakeSmall($"{riskTag} · {stance.ToUpperInvariant()}");
                headerLabel.Modulate = riskColor;
                choiceCard.AddChild(headerLabel);

                string previewText = c.moraleDelta != 0
                    ? TrFmt("ui.expedition.preview.morale", "Morale: {0}", $"{(c.moraleDelta > 0 ? "+" : "")}{c.moraleDelta}")
                    : AshfallLocalization.Tr("ui.expedition.preview.default", "Stamina: -15 · Ammo: 0");
                if (c.guiltDelta > 0) previewText += " · " + TrFmt("ui.expedition.preview.guilt", "Guilt: +{0}", c.guiltDelta);
                if (c.factionStandingDelta != 0) previewText += " · " + TrFmt("ui.expedition.preview.standing", "Standing: {0}", $"{(c.factionStandingDelta > 0 ? "+" : "")}{c.factionStandingDelta}");
                choiceCard.AddChild(AshfallUiHelpers.MakeMetadata(previewText));

                bool authoritativePatrol = _lastSurfaced!.is_patrol && c.isPatrolChoice;
                bool canAfford = authoritativePatrol ? c.enabled : true;
                bool meetsRequirement = true;
                string requirementText = string.Empty;

                if (!string.IsNullOrWhiteSpace(c.requiredItemId) && c.requiredItemQuantity > 0)
                {
                    int held = inv?.CountById(c.requiredItemId) ?? 0;
                    string itemName = _expeditionHost?.Items?.Get(c.requiredItemId)?.displayName ?? HumanizeDisplayToken(c.requiredItemId);
                    requirementText = TrFmt("ui.expedition.badge.req", "Req: {0} x{1} ({2}/{3})",
                        itemName, c.requiredItemQuantity, held, c.requiredItemQuantity);
                    if (!authoritativePatrol && held < c.requiredItemQuantity)
                    {
                        canAfford = false;
                        meetsRequirement = false;
                    }
                }

                // costItems is List<string> of item ids; duplicates encode quantity
                // (same aggregation as TravelEncounterChoice.GetNormalizedCosts).
                string costText = string.Empty;
                if (c.costItems != null && c.costItems.Count > 0)
                {
                    var aggregated = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                    foreach (var itemId in c.costItems)
                    {
                        if (string.IsNullOrWhiteSpace(itemId)) continue;
                        string clean = itemId.Trim();
                        aggregated[clean] = aggregated.TryGetValue(clean, out int qty) ? qty + 1 : 1;
                    }

                    var costParts = new List<string>();
                    foreach (var kvp in aggregated)
                    {
                        int needed = kvp.Value;
                        if (needed <= 0) continue;
                        int held = inv?.CountById(kvp.Key) ?? 0;
                        string itemName = _expeditionHost?.Items?.Get(kvp.Key)?.displayName ?? HumanizeDisplayToken(kvp.Key);
                        costParts.Add($"{itemName} x{needed} ({held}/{needed})");
                        if (!authoritativePatrol && held < needed)
                        {
                            canAfford = false;
                        }
                    }
                    if (costParts.Count > 0)
                    {
                        costText = TrFmt("ui.expedition.badge.cost_list", "Cost: {0}", string.Join(", ", costParts));
                    }
                }

                if (!string.IsNullOrEmpty(requirementText))
                {
                    var reqLabel = AshfallUiHelpers.MakeSmall(requirementText);
                    reqLabel.Modulate = meetsRequirement
                        ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe)
                        : AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical);
                    choiceCard.AddChild(reqLabel);
                }

                if (!string.IsNullOrEmpty(costText))
                {
                    var costLabel = AshfallUiHelpers.MakeSmall(costText);
                    costLabel.Modulate = canAfford
                        ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.LetheAmber)
                        : AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical);
                    choiceCard.AddChild(costLabel);
                }

                // Reward badge: Gain: {itemName} ×{qty} (Theme.Lethe)
                if (!string.IsNullOrWhiteSpace(c.grantItemId) && c.grantItemQuantity > 0)
                {
                    string grantName = _expeditionHost?.Items?.Get(c.grantItemId)?.displayName ?? HumanizeDisplayToken(c.grantItemId);
                    var rewardLabel = AshfallUiHelpers.MakeSmall(TrFmt("ui.expedition.badge.gain", "Gain: {0} ×{1}", grantName, c.grantItemQuantity));
                    rewardLabel.Modulate = AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe);
                    choiceCard.AddChild(rewardLabel);
                }
                // Offering / Cost badge: Cost: {itemName} ×{-qty} (Theme.LetheAmber) with shelter inventory affordability check
                else if (!string.IsNullOrWhiteSpace(c.grantItemId) && c.grantItemQuantity < 0)
                {
                    int neededOffering = -c.grantItemQuantity;
                    int heldOffering = inv?.CountById(c.grantItemId) ?? 0;
                    string offeringName = _expeditionHost?.Items?.Get(c.grantItemId)?.displayName ?? HumanizeDisplayToken(c.grantItemId);
                    var offeringLabel = AshfallUiHelpers.MakeSmall(TrFmt("ui.expedition.badge.cost", "Cost: {0} ×{1} ({2}/{3})",
                        offeringName, neededOffering, heldOffering, neededOffering));
                    offeringLabel.Modulate = heldOffering >= neededOffering
                        ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.LetheAmber)
                        : AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical);
                    choiceCard.AddChild(offeringLabel);
                    if (heldOffering < neededOffering)
                    {
                        canAfford = false;
                    }
                }

                // Journal clue badge: [CODEX] Clue Unlocked: {journalName} (Theme.Cyan)
                if (!string.IsNullOrWhiteSpace(c.journalUnlockId))
                {
                    string journalName = HumanizeDisplayToken(c.journalUnlockId);
                    var journalLabel = AshfallUiHelpers.MakeSmall(TrFmt("ui.expedition.badge.codex", "[CODEX] Clue Unlocked: {0}", journalName));
                    journalLabel.Modulate = AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Cyan);
                    choiceCard.AddChild(journalLabel);
                }

                // Map discovery badge: [MAP] Discovers: {locName} (Theme.Cyan)
                if (!string.IsNullOrWhiteSpace(c.discoverLocationId))
                {
                    string locName = _expeditionHost?.Definitions?.Find(d => d.id == c.discoverLocationId)?.displayName ?? HumanizeDisplayToken(c.discoverLocationId);
                    var mapLabel = AshfallUiHelpers.MakeSmall(TrFmt("ui.expedition.badge.map", "[MAP] Discovers: {0}", locName));
                    mapLabel.Modulate = AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Cyan);
                    choiceCard.AddChild(mapLabel);
                }

                // One-time badge: [ONE-TIME] (Theme.LetheAmber)
                if (c.depletesOnResolve)
                {
                    var oneTimeLabel = AshfallUiHelpers.MakeSmall(
                        AshfallLocalization.Tr("ui.expedition.one_time", "[ONE-TIME]"));
                    oneTimeLabel.Modulate = AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.LetheAmber);
                    choiceCard.AddChild(oneTimeLabel);
                }

                if (!canAfford)
                {
                    string reason = authoritativePatrol
                        ? FormatUnavailableReason(c.disabledReason)
                        : AshfallLocalization.Tr("ui.expedition.badge.cost_unavailable", "Cost or requirement unavailable");
                    var unavailable = AshfallUiHelpers.MakeSmall(
                        TrFmt("ui.expedition.badge.unavailable", "UNAVAILABLE — {0}", reason), autowrap: true);
                    unavailable.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
                    choiceCard.AddChild(unavailable);
                }

                var btn = AshfallUiHelpers.MakeButton(choiceText.ToUpperInvariant(), () =>
                {
                    if (_expeditionHost != null && _lastSurfaced != null)
                    {
                        bool ok = _expeditionHost.EncounterApplyChoice(
                            _lastSurfaced!.encounter_id,
                            choiceId,
                            _expeditionHost.CurrentDay,
                            _lastSurfaced!.trigger?.locationId ?? string.Empty);
                        if (ok)
                        {
                            GD.Print($"[Expedition] Resolved {_lastSurfaced!.encounter_id} via {choiceId}.");
                            DismissEncounter();
                        }
                        else if (_expeditionHost.LastChoiceWasDuplicate)
                        {
                            // Already decided (e.g. before a reload) — never reapply.
                            DismissEncounter();
                        }
                        else
                        {
                            if (_encounterBody != null)
                            {
                                _encounterBody.Text += "\n\n[Action cannot be taken: requirements or costs not met.]";
                            }
                        }
                    }
                    else
                    {
                        DismissEncounter();
                    }
                }, false);
                btn.CustomMinimumSize = new Vector2(0, 46);
                btn.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                btn.Disabled = !canAfford;
                choiceCard.AddChild(btn);

                choiceList.AddChild(choiceCard);
            }

            _choicesContainer.AddChild(choiceScroll);
            vbox.AddChild(_choicesContainer);
            for (int i = 0; i < choiceList.GetChildCount(); i++)
            {
                if (choiceList.GetChild(i) is VBoxContainer cardNode)
                {
                    for (int j = 0; j < cardNode.GetChildCount(); j++)
                    {
                        if (cardNode.GetChild(j) is Button button && !button.Disabled)
                        {
                            button.GrabFocus();
                            return;
                        }
                    }
                }
            }
            _encounterBtnOk?.GrabFocus();
        }

        /// <summary>Close the current modal and advance the queue. Acknowledged.</summary>
        private void DismissEncounter() => CloseCurrentEncounter();

        /// <summary>
        /// Close the current modal without deciding. The encounter stays in the
        /// host's pending list — only EncounterApplyChoice clears it — so it
        /// reappears in the pending rows for later.
        /// </summary>
        private void DeferEncounter() => CloseCurrentEncounter();

        private void CloseCurrentEncounter()
        {
            if (_encounterModal != null) _encounterModal.Visible = false;
            _modalActive = false;
            if (_choicesContainer != null)
            {
                _choicesContainer.QueueFree();
                _choicesContainer = null;
            }
            _lastSurfaced = null;
            ShowNextModal();
            FinishPendingBatchIfDone();
        }

        /// <summary>After a batch-mode modal closes, re-read pending so resolved rows disappear.</summary>
        private void FinishPendingBatchIfDone()
        {
            if (!_pendingBatchMode || _modalActive) return;
            _pendingBatchMode = false;
            RenderPendingList();
        }

        private void BuildEncounterModal()
        {
            if (_encounterModal != null) return;

            _encounterModal = new Control();
            _encounterModal.SetAnchorsPreset(LayoutPreset.FullRect);
            _encounterModal.MouseFilter = Control.MouseFilterEnum.Stop;
            _encounterModal.Visible = false;
            AddChild(_encounterModal);

            var backdrop = new ColorRect { Color = AshfallUiHelpers.PanelScrim() };
            backdrop.SetAnchorsPreset(LayoutPreset.FullRect);
            _encounterModal.AddChild(backdrop);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            _encounterModal.AddChild(center);

            var card = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            card.CustomMinimumSize = new Vector2(480, 0);
            center.AddChild(card);

            _encounterTitle = AshfallUiHelpers.MakeTitle(AshfallLocalization.Tr("ui.expedition.encounter_title", "ENCOUNTER"), Ashfall.Core.UI.Theme.FontSizeH2);
            _encounterTitle.HorizontalAlignment = HorizontalAlignment.Center;
            card.AddChild(_encounterTitle);

            var metaRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            metaRow.Alignment = BoxContainer.AlignmentMode.Center;
            _encounterFactionEmblem = AshfallUiHelpers.MakeFactionEmblem(string.Empty, 42);
            _encounterFactionEmblem.Visible = false;
            metaRow.AddChild(_encounterFactionEmblem);
            _encounterContext = AshfallUiHelpers.MakeSmall(string.Empty, autowrap: true);
            _encounterContext.HorizontalAlignment = HorizontalAlignment.Center;
            _encounterContext.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            _encounterContext.Visible = false;
            metaRow.AddChild(_encounterContext);
            card.AddChild(metaRow);

            var bodyScroll = new ScrollContainer
            {
                CustomMinimumSize = new Vector2(0, 140),
                SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
                VerticalScrollMode = ScrollContainer.ScrollMode.Auto
            };
            _encounterBody = AshfallUiHelpers.MakeBody("", true);
            _encounterBody.HorizontalAlignment = HorizontalAlignment.Center;
            _encounterBody.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            bodyScroll.AddChild(_encounterBody);
            card.AddChild(bodyScroll);

            var btnRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            btnRow.Alignment = BoxContainer.AlignmentMode.Center;

            _encounterBtnOk = AshfallUiHelpers.MakeButton(
                AshfallLocalization.Tr("ui.common.ok", "OK"), DismissEncounter, false);
            _encounterBtnOk.CustomMinimumSize = new Vector2(140, 36);
            btnRow.AddChild(_encounterBtnOk);

            var btnLater = AshfallUiHelpers.MakeButton(
                AshfallLocalization.Tr("ui.expedition.decide_later", "DECIDE LATER"), DeferEncounter, false);
            btnLater.CustomMinimumSize = new Vector2(160, 36);
            btnRow.AddChild(btnLater);

            card.AddChild(btnRow);
        }

        private void ShowAutoplayBanner(ExpeditionEncounterBridge.EncounterSurfaced surfaced)
        {
            BuildAutoplayBanner();
            if (_encounterBanner == null || _encounterBannerLabel == null) return;

            string phase = ExpeditionPhaseText.Label((ExpeditionPhase)surfaced.trigger.phase);
            _encounterBannerLabel.Text = surfaced.resolved_at_lead == false
                ? TrFmt("ui.expedition.banner_encounter", "[!] ENCOUNTER — {0} at {1} [{2}] # {3}",
                    FormatSurvivorName(surfaced.trigger.survivorId), surfaced.trigger.displayName, phase, surfaced.trigger.encounterCount)
                : TrFmt("ui.expedition.banner_specific", "[!] {0} — {1} [{2}] # {3}",
                    surfaced.title, FormatSurvivorName(surfaced.trigger.survivorId), phase, surfaced.trigger.encounterCount);
            _encounterBanner.Visible = true;
            _bannerTimer = BannerDuration;
            RefreshBannerProcessing();
        }

        private void BuildAutoplayBanner()
        {
            if (_encounterBanner != null) return;

            _encounterBanner = new Control();
            _encounterBanner.SetAnchorsPreset(LayoutPreset.TopWide);
            _encounterBanner.CustomMinimumSize = new Vector2(0, 52);
            _encounterBanner.Visible = false;
            AddChild(_encounterBanner);

            var bg = new ColorRect { Color = new Color(Ashfall.Core.UI.Theme.Entropy.r * 0.12f, Ashfall.Core.UI.Theme.Entropy.g * 0.12f, Ashfall.Core.UI.Theme.Entropy.b * 0.12f, 0.94f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            _encounterBanner.AddChild(bg);

            var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            row.SetAnchorsPreset(LayoutPreset.FullRect);
            _encounterBanner.AddChild(row);

            var icon = AshfallUiHelpers.MakeMono("[!]");
            icon.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Entropy));
            row.AddChild(icon);

            _encounterBannerLabel = AshfallUiHelpers.MakeMono("");
            _encounterBannerLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
            _encounterBannerLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            row.AddChild(_encounterBannerLabel);
        }

        public override void _Process(double delta)
        {
            using var measurement = Host.FrameStartupProfiler.MeasureProcess(nameof(ExpeditionPanel));
            if (!Visible) return;
            if (!ExpeditionHostSession.UseEncounterModal && _encounterBanner != null && _encounterBanner.Visible)
            {
                _bannerTimer -= (float)delta;
                if (_bannerTimer <= 0f)
                {
                    _encounterBanner.Visible = false;
                    SetProcess(false);
                }
            }
        }

        private void RefreshBannerProcessing()
        {
            SetProcess(IsVisibleInTree() && Visible && _bannerTimer > 0f &&
                _encounterBanner != null && _encounterBanner.Visible);
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;

            if (AshfallInputActions.IsCloseOrCancel(@event))
            {
                Close();
                GetViewport().SetInputAsHandled();
            }
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
