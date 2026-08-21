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
        private SurvivorsHostSession? _survivorsHost;
        private InventoryHostSession? _inventoryHost;

        private VBoxContainer _targetsContainer = null!;
        private VBoxContainer _activeContainer = null!;
        private VBoxContainer _pendingContainer = null!;
        private Label _pendingHeader = null!;
        private Label _statusSummary = null!;

        private string _selectedTargetId = "loc_the_allotments";
        private string _selectedSurvivorId = "survivor_gunner_mikhail";
        private ExpeditionStance _selectedStance = ExpeditionStance.Stealth;

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
            InventoryHostSession? inventoryHost = null)
        {
            if (_expeditionHost != null)
            {
                _expeditionHost.Engine.OnExpeditionCompleted -= OnExpeditionCompleted;
                _expeditionHost.StateChanged -= RefreshView;
            }

            _expeditionHost = expeditionHost;
            _survivorsHost = survivorsHost;
            _inventoryHost = inventoryHost;

            if (_expeditionHost != null)
            {
                _expeditionHost.Engine.OnExpeditionCompleted += OnExpeditionCompleted;
                _expeditionHost.StateChanged += RefreshView;

                RefreshView();
            }
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
                    _expeditionHost.TickDemoHours(2f);
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

            // Clear Active Container
            while (_activeContainer.GetChildCount() > 0)
            {
                var child = _activeContainer.GetChild(0);
                _activeContainer.RemoveChild(child);
                child.QueueFree();
            }

            // Clear Targets Container
            while (_targetsContainer.GetChildCount() > 0)
            {
                var child = _targetsContainer.GetChild(0);
                _targetsContainer.RemoveChild(child);
                child.QueueFree();
            }

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
                            _expeditionHost.PushLuckDemo(scoutId);
                            OnExpeditionUpdated?.Invoke();
                            RefreshView();
                        });
                        btnPush.CustomMinimumSize = new Vector2(230, 30);
                        actionRow.AddChild(btnPush);

                        var btnRetreat = AshfallUiHelpers.MakeButton("ORDER INBOUND RETURN", () =>
                        {
                            _expeditionHost.RetreatDemo(scoutId);
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

            foreach (var def in _expeditionHost.DemoDefinitions)
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
                        _expeditionHost.Engine.Start(def, livingSurvivors[0], 1, ExpeditionStance.Stealth);
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
                        _expeditionHost.Engine.Start(def, livingSurvivors[0], 1, ExpeditionStance.Speed);
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

            while (_pendingContainer.GetChildCount() > 0)
            {
                var child = _pendingContainer.GetChild(0);
                _pendingContainer.RemoveChild(child);
                child.QueueFree();
            }

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
            var choiceRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            choiceRow.Alignment = BoxContainer.AlignmentMode.Center;

            foreach (var c in _lastSurfaced!.choices)
            {
                string choiceId = c.choiceId;
                string choiceText = c.text;
                var btn = AshfallUiHelpers.MakeButton(choiceText.ToUpperInvariant(), () =>
                {
                    if (_expeditionHost != null && _lastSurfaced != null)
                    {
                        // Location comes from this DTO's own trigger, which for a
                        // backlog row is the pending entry's recorded location.
                        bool ok = _expeditionHost.EncounterApplyChoice(
                            _lastSurfaced!.encounter_id,
                            choiceId,
                            _expeditionHost.CurrentDay,
                            _lastSurfaced!.trigger?.locationId ?? string.Empty);
                        if (ok)
                        {
                            GD.Print($"[Expedition] Resolved {_lastSurfaced!.encounter_id} via {choiceId}.");
                        }
                    }
                    DismissEncounter();
                }, false);
                btn.CustomMinimumSize = new Vector2(180, 34);
                choiceRow.AddChild(btn);
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
    }
}
