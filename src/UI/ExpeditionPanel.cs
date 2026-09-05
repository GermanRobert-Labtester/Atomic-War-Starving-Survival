using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.UI;

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

        private string _selectedTargetId = "loc_the_allotments";
        private string _selectedSurvivorId = "survivor_gunner_mikhail";
        private ExpeditionStance _selectedStance = ExpeditionStance.Stealth;

        // ── Dispatch preparation (vehicle + weapon loadout) ──────────
        private OptionButton? _vehicleSelect;
        private OptionButton? _weaponSelect;
        private readonly List<string> _vehicleIds = new();
        private readonly List<string> _weaponInstanceIds = new();
        private Ashfall.Core.EquipmentConditionSystem? _equipment;

        /// <summary>The vehicle chosen in the dispatch-preparation selector, or "" for foot.</summary>
        private string SelectedVehicleId =>
            _vehicleSelect != null && _vehicleSelect.Selected > 0 && _vehicleSelect.Selected - 1 < _vehicleIds.Count
                ? _vehicleIds[_vehicleSelect.Selected - 1]
                : string.Empty;

        private string SelectedWeaponInstanceId =>
            _weaponSelect != null && _weaponSelect.Selected > 0 && _weaponSelect.Selected - 1 < _weaponInstanceIds.Count
                ? _weaponInstanceIds[_weaponSelect.Selected - 1]
                : string.Empty;

        // ── Encounter surface (modal default / autoplay flag) ────────
        private readonly Queue<ExpeditionEncounterBridge.EncounterSurfaced> _encounterQueue = new();
        private Control? _encounterModal;
        private Label? _encounterTitle;
        private Label? _encounterBody;
        private Control? _encounterBanner;
        private Label? _encounterBannerLabel;
        private bool _modalActive;
        private float _bannerTimer;
        private const float BannerDuration = 3f;
        private ExpeditionEncounterBridge.EncounterSurfaced? _lastSurfaced;
        private VBoxContainer? _choicesContainer;
        private bool _pendingBatchMode;

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
                _expeditionHost.StateChanged += RefreshView;

                RefreshView();
            }
        }

        public void Unbind()
        {
            if (_expeditionHost != null)
            {
                _expeditionHost.Engine.OnExpeditionCompleted -= OnExpeditionCompleted;
                _expeditionHost.StateChanged -= RefreshView;
                _expeditionHost = null;
            }
            _survivorsHost = null;
            _inventoryHost = null;
            RefreshView();
        }

        private void OnExpeditionCompleted(ExpeditionState state)
        {
            if (state != null && state.loot != null && state.loot.Count > 0)
            {
                if (_inventoryHost != null)
                {
                    foreach (var item in state.loot)
                    {
                        if (string.IsNullOrEmpty(item.itemId) || item.quantity <= 0) continue;
                        _inventoryHost.Add(item.itemId, item.quantity);
                    }
                }
                OnLootDeposited?.Invoke(state.loot);
            }
            OnExpeditionUpdated?.Invoke();
            RefreshView();
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.95f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

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

            var header = AshfallUiHelpers.MakeTitle("WASTELAND EXPEDITIONS // SORTIE PLANNER", Ashfall.Core.UI.Theme.FontSizeH1);
            header.HorizontalAlignment = HorizontalAlignment.Center;
            rootBox.AddChild(header);

            _statusSummary = AshfallUiHelpers.MakeMetadata("Plan reconnaissance and scavenging sorties. Monitor radiation risk, distance, and survivor stamina.");
            _statusSummary.HorizontalAlignment = HorizontalAlignment.Center;
            _statusSummary.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            rootBox.AddChild(_statusSummary);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Active Expeditions ──
            var activeTitle = AshfallUiHelpers.MakeSectionHeader("ACTIVE SORTIES IN THE FIELD");
            rootBox.AddChild(activeTitle);

            _activeContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_activeContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Pending Surfaced Encounters ──
            _pendingHeader = AshfallUiHelpers.MakeSectionHeader("PENDING SURFACED ENCOUNTERS");
            _pendingHeader.Visible = false;
            rootBox.AddChild(_pendingHeader);

            _pendingContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            _pendingContainer.Visible = false;
            rootBox.AddChild(_pendingContainer);

            // ── Dispatch Preparation (vehicle + weapon loadout) ──
            var prepTitle = AshfallUiHelpers.MakeSectionHeader("DISPATCH PREPARATION // MOTOR POOL & ARMORY");
            rootBox.AddChild(prepTitle);

            var prepRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingMd);

            prepRow.AddChild(AshfallUiHelpers.MakeBody("VEHICLE:"));
            _vehicleSelect = new OptionButton();
            _vehicleSelect.CustomMinimumSize = new Vector2(230, 30);
            _vehicleSelect.ItemSelected += _ => UpdateEstimateLine();
            prepRow.AddChild(_vehicleSelect);

            prepRow.AddChild(AshfallUiHelpers.MakeBody("WEAPON:"));
            _weaponSelect = new OptionButton();
            _weaponSelect.CustomMinimumSize = new Vector2(230, 30);
            _weaponSelect.ItemSelected += _ => UpdateEstimateLine();
            prepRow.AddChild(_weaponSelect);

            var btnRefuel = AshfallUiHelpers.MakeButton("REFUEL TOP-UP", () =>
            {
                string vehicleId = SelectedVehicleId;
                if (_expeditionHost == null || _inventoryHost == null || string.IsNullOrEmpty(vehicleId)) return;
                int have = _inventoryHost.Inventory.CountById("fuel");
                if (have <= 0) return;
                int spend = Math.Min(have, 10);
                _inventoryHost.Remove("fuel", spend);
                _expeditionHost.RefuelVehicle(vehicleId, spend);
                RefreshView();
            });
            btnRefuel.TooltipText = "Burn 10 carried fuel items into the selected tank.";
            prepRow.AddChild(btnRefuel);

            var btnTrackGear = AshfallUiHelpers.MakeButton("FIT TRACK GEAR", () =>
            {
                string vehicleId = SelectedVehicleId;
                if (_expeditionHost == null || string.IsNullOrEmpty(vehicleId)) return;
                _expeditionHost.InstallTrackGear(vehicleId, "vehicle_track_gear_standard");
                RefreshView();
            });
            btnTrackGear.TooltipText = "Install the authored track-gear package, improving rough-terrain traction and reducing breakdown risk.";
            prepRow.AddChild(btnTrackGear);

            rootBox.AddChild(prepRow);

            _estimateLabel = AshfallUiHelpers.MakeMono("");
            _estimateLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            rootBox.AddChild(_estimateLabel);

            // ── Target Destinations ──
            var targetsTitle = AshfallUiHelpers.MakeSectionHeader("KNOWN WASTELAND DESTINATIONS");
            rootBox.AddChild(targetsTitle);

            _targetsContainer = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            rootBox.AddChild(_targetsContainer);

            rootBox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingMd);
            btnRow.Alignment = BoxContainer.AlignmentMode.Center;

            var btnTick = AshfallUiHelpers.MakeButton("ADVANCE SORTIES (2 HOURS)", () =>
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

            var btnClose = AshfallUiHelpers.MakeButton("RETURN TO DASHBOARD [Esc]", () => OnClose?.Invoke(), true);
            btnClose.CustomMinimumSize = new Vector2(220, 42);
            btnRow.AddChild(btnClose);
            rootBox.AddChild(btnRow);

            var hint = AshfallUiHelpers.MakeSmall("Press [Esc] to return");
            hint.HorizontalAlignment = HorizontalAlignment.Center;
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            rootBox.AddChild(hint);
        }

        private static string FormatSurvivorName(string id)
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

        public void RefreshView()
        {
            if (_activeContainer == null || _targetsContainer == null || _expeditionHost == null) return;

            // Clear Containers
            AshfallUiHelpers.EmptyChildren(_activeContainer);
            AshfallUiHelpers.EmptyChildren(_targetsContainer);

            // 1. Render Active Expeditions
            if (_expeditionHost.Engine.ActiveCount == 0)
            {
                _activeContainer.AddChild(AshfallUiHelpers.MakeMetadata("No active scavenging sorties currently deployed."));
            }
            else
            {
                foreach (var kv in _expeditionHost.Engine.Active)
                {
                    var exp = kv.Value;
                    if (exp == null) continue;

                    var card = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
                    var topRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);

                    var phaseName = ((ExpeditionPhase)exp.phase).ToString().ToUpperInvariant();
                    var lblPhase = AshfallUiHelpers.MakeMono($"[{phaseName}] {exp.displayName}");
                    lblPhase.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                    lblPhase.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                    topRow.AddChild(lblPhase);

                    var lblScout = AshfallUiHelpers.MakeSmall($"SCOUT: {FormatSurvivorName(exp.survivorId)}");
                    topRow.AddChild(lblScout);

                    var lblStamina = AshfallUiHelpers.MakeMono($"STAMINA {exp.stamina:0}%");
                    lblStamina.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(exp.stamina < 30 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Hot));
                    topRow.AddChild(lblStamina);
                    card.AddChild(topRow);

                    var midRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                    var progress = AshfallUiHelpers.MakeSmall($"Travel Progress: {exp.travelTicksCompleted}/{exp.distanceTicks} legs · Encounters: {exp.encounterCount} · Loot: {exp.loot.Count} items ({exp.currentWeightKg:F1}/{exp.maxLootCapacityKg:F0} kg)");
                    midRow.AddChild(progress);
                    card.AddChild(midRow);

                    // Action Controls for Active Expedition
                    var actionRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                    string scoutId = exp.survivorId;

                    if (exp.phase == (int)ExpeditionPhase.Looting)
                    {
                        var btnPush = AshfallUiHelpers.MakeButton("PUSH LUCK (SCAVENGE DEEPER)", () =>
                        {
                            _expeditionHost.PushLuck(scoutId);
                            OnExpeditionUpdated?.Invoke();
                            RefreshView();
                        });
                        btnPush.CustomMinimumSize = new Vector2(230, 30);
                        actionRow.AddChild(btnPush);

                        var btnRetreat = AshfallUiHelpers.MakeButton("ORDER INBOUND RETURN", () =>
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
                            ? "In transit toward objective..."
                            : "Returning to shelter with salvage...");
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
            UpdateEstimateLine(livingSurvivors.Count > 0 ? livingSurvivors[0] : null);

            foreach (var def in _expeditionHost.Definitions)
            {
                if (def == null) continue;

                var card = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
                var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);

                var title = AshfallUiHelpers.MakeSectionHeader(def.displayName);
                title.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                row.AddChild(title);

                var danger = AshfallUiHelpers.MakeMono($"DANGER: LVL {def.dangerLevel} · DISTANCE: {def.distanceTicks} LEGS");
                danger.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(def.dangerLevel >= 3 ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Warm));
                row.AddChild(danger);
                card.AddChild(row);

                var lootCategories = string.Join(", ", def.lootCategories);
                var desc = AshfallUiHelpers.MakeBody($"Potential Salvage: {lootCategories} · Encounter Risk: {def.encounterChancePerTick:P0}/hr");
                desc.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                card.AddChild(desc);

                // Task 122: live world state — ownership, spoilage, ruin, threats.
                var worldLine = BuildWorldStateLine(def.id);
                if (!string.IsNullOrEmpty(worldLine))
                {
                    var worldLabel = AshfallUiHelpers.MakeMono(worldLine);
                    worldLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(
                        worldLine.Contains("RUINED") || worldLine.Contains("threat(s)")
                            ? Ashfall.Core.UI.Theme.Critical
                            : Ashfall.Core.UI.Theme.Pale));
                    card.AddChild(worldLabel);
                }

                // Dispatch Bar
                var dispatchRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                string defId = def.id;
                bool blocked = _expeditionHost.IsLocationBlocked(defId);

                if (blocked)
                {
                    var gateLabel = AshfallUiHelpers.MakeMono("[CROSSING GATE CLOSED — no vouch]");
                    gateLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
                    dispatchRow.AddChild(gateLabel);
                }

                var btnDispatchStealth = AshfallUiHelpers.MakeButton("DISPATCH STEALTH SORTIE", () =>
                {
                    if (livingSurvivors.Count > 0)
                    {
                        _expeditionHost.DispatchSortie(livingSurvivors[0], defId, ExpeditionStance.Stealth, 1, SelectedVehicleId);
                        OnExpeditionUpdated?.Invoke();
                        RefreshView();
                    }
                });
                btnDispatchStealth.Disabled = blocked || livingSurvivors.Count == 0 || _expeditionHost.Engine.Active.ContainsKey(livingSurvivors[0]);
                btnDispatchStealth.CustomMinimumSize = new Vector2(200, 32);
                dispatchRow.AddChild(btnDispatchStealth);

                var btnDispatchSpeed = AshfallUiHelpers.MakeButton("DISPATCH SPEED SORTIE (1.5x)", () =>
                {
                    if (livingSurvivors.Count > 0)
                    {
                        _expeditionHost.DispatchSortie(livingSurvivors[0], defId, ExpeditionStance.Speed, 1, SelectedVehicleId);
                        OnExpeditionUpdated?.Invoke();
                        RefreshView();
                    }
                });
                btnDispatchSpeed.Disabled = blocked || livingSurvivors.Count == 0 || _expeditionHost.Engine.Active.ContainsKey(livingSurvivors[0]);
                btnDispatchSpeed.CustomMinimumSize = new Vector2(220, 32);
                dispatchRow.AddChild(btnDispatchSpeed);

                card.AddChild(dispatchRow);

                var panel = AshfallUiHelpers.MakePanel();
                panel.AddChild(card);
                _targetsContainer.AddChild(panel);
            }
        }

        /// <summary>Live evolving-world line for a target location, or null when untouched ground.</summary>
        private string? BuildWorldStateLine(string locationId)
        {
            if (_worldHost == null || string.IsNullOrEmpty(locationId)) return null;
            var rec = _worldHost.LocationEvolution?.TryGetRecord(locationId);
            string? flavor = _worldHost.FlavorTextForLocation(locationId, _worldHost.Weather?.Current.ToString());

            if (rec == null)
            {
                return string.IsNullOrEmpty(flavor) ? null : TruncateFlavor(flavor);
            }

            string owner = rec.currentOwner == "none" ? "unclaimed" : rec.currentOwner.Replace("faction_", "");
            string state = rec.isRuined ? " · RUINED" : string.Empty;
            string threats = rec.activeThreats.Count > 0 ? $" · {rec.activeThreats.Count} threat(s)" : string.Empty;
            string line = $"WORLD: {owner} · {rec.lootDepletionFactor:P0} spoilage{state}{threats}";
            if (!string.IsNullOrEmpty(flavor))
                line += "\n" + TruncateFlavor(flavor);
            return line;
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

            if (_vehicleSelect != null)
            {
                string previous = SelectedVehicleId;
                _vehicleSelect.Clear();
                _vehicleIds.Clear();
                _vehicleSelect.AddItem("On foot", 0);
                foreach (var v in _expeditionHost.Vehicles.State.ownedVehicles.Values)
                {
                    if (v == null || string.IsNullOrEmpty(v.vehicleId)) continue;
                    _vehicleIds.Add(v.vehicleId);
                    _vehicleSelect.AddItem(
                        $"{v.displayName} · fuel {v.fuel:F0}/{v.maxFuel:F0} · cond {v.condition:F0}%" + (v.isBrokenDown ? " · BROKEN" : ""),
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
                _weaponSelect.AddItem("Sidearm only", 0);
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

            var preview = _expeditionHost.EstimateExpedition(def.id, ExpeditionStance.Stealth, SelectedVehicleId, readiness, jam);
            if (preview == null)
            {
                _estimateLabel.Text = "NO ROUTE DATA.";
                return;
            }

            var (est, fuelOk) = preview.Value;
            string vehiclePart = est.usingVehicle
                ? $"by {_expeditionHost.Vehicles.GetVehicle(SelectedVehicleId)?.displayName ?? est.locationId}"
                : "on foot";
            _estimateLabel.Text =
                $"ESTIMATE [{def.displayName} · {vehiclePart}] ticks {est.totalTicks:F0} " +
                $"(out {est.outboundTicks:F0} / loot {est.lootingTicks:F0} / in {est.inboundTicks:F0}) · " +
                $"cargo {est.cargoCapacityKg:F0} kg · fuel need {est.fuelRequired:F1}{(fuelOk ? "" : " — TANK LOW")} · " +
                $"breakdown {est.breakdownRiskTotal:P0} · encounter {est.encounterRiskPerTick:P0}/hr · " +
                $"weapon readiness {est.weaponReadiness:P0}{(jam > 0f ? $" (jam {jam:P0})" : "")}";
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
                    : $"ENCOUNTER #{p.legIndex}";

                var card = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
                var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);

                var lbl = AshfallUiHelpers.MakeMono($"[#{p.legIndex}] {label}");
                lbl.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                row.AddChild(lbl);

                var meta = AshfallUiHelpers.MakeSmall($"{p.locationId} · DAY {p.day}");
                row.AddChild(meta);

                string pendingId = p.encounterId;
                string pendingLocation = p.locationId;
                int pendingLeg = p.legIndex;
                var btnResolve = AshfallUiHelpers.MakeButton("RESOLVE", () =>
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
            var btnDismissAll = AshfallUiHelpers.MakeButton("DISMISS ALL", () =>
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
            }
            else
            {
                dto.title = def.title;
                dto.description = def.description;
                dto.category = def.category;
                dto.choices = def.choices ?? new List<Ashfall.Core.Narrative.EncounterChoiceDefinition>();
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
                return;
            }

            _modalActive = true;
            _lastSurfaced = _encounterQueue.Dequeue();
            BuildEncounterModal();
            if (_encounterModal == null) return;

            if (_encounterTitle != null) _encounterTitle.Text = _lastSurfaced!.title;
            if (_encounterBody != null && _lastSurfaced != null)
            {
                if (_lastSurfaced!.resolved_at_lead == false)
                {
                    // Bare notice: honest text, no invented outcome.
                    _encounterBody.Text = _lastSurfaced!.description;
                }
                else
                {
                    string phase = ((ExpeditionPhase)_lastSurfaced!.trigger.phase).ToString().ToUpperInvariant();
                    _encounterBody.Text = string.Join("\n",
                        FormatSurvivorName(_lastSurfaced!.trigger.survivorId) + " at " + _lastSurfaced!.trigger.displayName,
                        $"{_lastSurfaced!.category} · {phase} · encounter #{_lastSurfaced!.trigger.encounterCount}",
                        "",
                        _lastSurfaced!.description);
                }
            }

            RenderChoiceButtons();
            _encounterModal.Visible = true;
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
            _choicesContainer.AddChild(AshfallUiHelpers.MakeSectionHeader("TACTICAL APPROACH SELECTION"));

            var choiceRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            choiceRow.Alignment = BoxContainer.AlignmentMode.Center;

            float danger = _lastSurfaced?.trigger?.dangerLevel ?? 5f;
            string stance = _lastSurfaced?.trigger?.stance ?? "Balanced";

            var inv = _expeditionHost?.ShelterInventory ?? _inventoryHost?.Inventory;

            foreach (var c in _lastSurfaced!.choices)
            {
                string choiceId = c.choiceId;
                string choiceText = c.text;

                // Tactical assessment derivation
                string riskTag = danger >= 8 ? "EXTREME RISK" : danger >= 5 ? "HIGH RISK" : danger >= 3 ? "MODERATE RISK" : "LOW RISK";
                var riskColor = danger >= 8 ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical)
                    : danger >= 5 ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Entropy)
                    : danger >= 3 ? AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.LetheAmber)
                    : AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe);

                var choiceCard = AshfallUiHelpers.MakeVBox(2);
                choiceCard.CustomMinimumSize = new Vector2(210, 100);

                var headerLabel = AshfallUiHelpers.MakeSmall($"{riskTag} · {stance.ToUpperInvariant()}");
                headerLabel.Modulate = riskColor;
                choiceCard.AddChild(headerLabel);

                string previewText = c.moraleDelta != 0 ? $"Morale: {(c.moraleDelta > 0 ? "+" : "")}{c.moraleDelta}" : "Stamina: -15 · Ammo: 0";
                if (c.guiltDelta > 0) previewText += $" · Guilt: +{c.guiltDelta}";
                if (c.factionStandingDelta != 0) previewText += $" · Standing: {(c.factionStandingDelta > 0 ? "+" : "")}{c.factionStandingDelta}";
                choiceCard.AddChild(AshfallUiHelpers.MakeMetadata(previewText));

                bool canAfford = true;
                bool meetsRequirement = true;
                string requirementText = string.Empty;

                if (!string.IsNullOrWhiteSpace(c.requiredItemId) && c.requiredItemQuantity > 0)
                {
                    int held = inv?.CountById(c.requiredItemId) ?? 0;
                    string itemName = _expeditionHost?.Items?.Get(c.requiredItemId)?.displayName ?? c.requiredItemId;
                    requirementText = $"Req: {itemName} x{c.requiredItemQuantity} ({held}/{c.requiredItemQuantity})";
                    if (held < c.requiredItemQuantity)
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
                        string itemName = _expeditionHost?.Items?.Get(kvp.Key)?.displayName ?? kvp.Key;
                        costParts.Add($"{itemName} x{needed} ({held}/{needed})");
                        if (held < needed)
                        {
                            canAfford = false;
                        }
                    }
                    if (costParts.Count > 0)
                    {
                        costText = "Cost: " + string.Join(", ", costParts);
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
                btn.CustomMinimumSize = new Vector2(210, 32);
                if (!canAfford)
                {
                    btn.Disabled = true;
                }
                choiceCard.AddChild(btn);

                choiceRow.AddChild(choiceCard);
            }

            _choicesContainer.AddChild(choiceRow);
            vbox.AddChild(_choicesContainer);
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

            var backdrop = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.85f) };
            backdrop.SetAnchorsPreset(LayoutPreset.FullRect);
            _encounterModal.AddChild(backdrop);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            _encounterModal.AddChild(center);

            var card = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            card.CustomMinimumSize = new Vector2(420, 0);
            center.AddChild(card);

            _encounterTitle = AshfallUiHelpers.MakeTitle("ENCOUNTER", Ashfall.Core.UI.Theme.FontSizeH2);
            _encounterTitle.HorizontalAlignment = HorizontalAlignment.Center;
            card.AddChild(_encounterTitle);

            _encounterBody = AshfallUiHelpers.MakeBody("", true);
            _encounterBody.HorizontalAlignment = HorizontalAlignment.Center;
            card.AddChild(_encounterBody);

            var btnRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            btnRow.Alignment = BoxContainer.AlignmentMode.Center;

            var btnOk = AshfallUiHelpers.MakeButton("OK", DismissEncounter, false);
            btnOk.CustomMinimumSize = new Vector2(140, 36);
            btnRow.AddChild(btnOk);

            var btnLater = AshfallUiHelpers.MakeButton("DECIDE LATER", DeferEncounter, false);
            btnLater.CustomMinimumSize = new Vector2(160, 36);
            btnRow.AddChild(btnLater);

            card.AddChild(btnRow);
        }

        private void ShowAutoplayBanner(ExpeditionEncounterBridge.EncounterSurfaced surfaced)
        {
            BuildAutoplayBanner();
            if (_encounterBanner == null || _encounterBannerLabel == null) return;

            string phase = ((ExpeditionPhase)surfaced.trigger.phase).ToString().ToUpperInvariant();
            _encounterBannerLabel.Text = surfaced.resolved_at_lead == false
                ? $"[!] ENCOUNTER — {FormatSurvivorName(surfaced.trigger.survivorId)} at {surfaced.trigger.displayName} [{phase}] # {surfaced.trigger.encounterCount}"
                : $"[!] {surfaced.title} — {FormatSurvivorName(surfaced.trigger.survivorId)} [{phase}] # {surfaced.trigger.encounterCount}";
            _encounterBanner.Visible = true;
            _bannerTimer = BannerDuration;
        }

        private void BuildAutoplayBanner()
        {
            if (_encounterBanner != null) return;

            _encounterBanner = new Control();
            _encounterBanner.SetAnchorsPreset(LayoutPreset.TopWide);
            _encounterBanner.CustomMinimumSize = new Vector2(0, 52);
            _encounterBanner.Visible = false;
            AddChild(_encounterBanner);

            var bg = new ColorRect { Color = new Color(0.10f, 0.07f, 0.04f, 0.94f) };
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
            if (!Visible) return;
            if (!ExpeditionHostSession.UseEncounterModal && _encounterBanner != null && _encounterBanner.Visible)
            {
                _bannerTimer -= (float)delta;
                if (_bannerTimer <= 0f)
                {
                    _encounterBanner.Visible = false;
                }
            }
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;

            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
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
