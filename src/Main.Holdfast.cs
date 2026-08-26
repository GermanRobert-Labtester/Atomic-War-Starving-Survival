using Godot;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AtomicWar.Journal;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Economy;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Foundry;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.Radio;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.Economy;
using AtomicWar.GodotApp.YearOfAsh;
using AtomicWar.GodotApp.Muster;
using AtomicWar.GodotApp.Dose;
using AtomicWar.GodotApp.UtilityAI;
using AtomicWar.GodotApp.Radio;
using AtomicWar.GodotApp.Audio;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        // ── Holdfast runtime fields (GAP-ARCH-01 Phase 2) ──
        private CoreDemoSession _core = null!;
        private HoldfastRuntimeSession _holdfastRuntime = null!;
        private HoldfastTerminalPanel _holdfastTerminal = null!;
        private bool _holdfastDirty;

        private void SetupIceRoad()
        {
            if (_core != null) return;
            _core = CoreDemoSession.Create(_dataDir);
            _core.IceRoad.OnStateChanged += _ => { _holdfastDirty = true; RefreshIceRoadLabel(); };
            _core.Census.OnStateChanged += _ => { _holdfastDirty = true; RefreshIceRoadLabel(); };
            _core.Brine.OnStateChanged += _ => { _holdfastDirty = true; RefreshIceRoadLabel(); };
            _core.Quests.OnStateChanged += _ => { _holdfastDirty = true; RefreshIceRoadLabel(); };

            // Cross-host roundtrip: a save written here (or by the Unity host) restores
            // the S1 gate instead of starting dark again. Codec validates the checksum.
            var save = HoldfastSaveStore.TryLoad();
            if (save != null)
            {
                _core.RestoreSave(save);
                _simDay = _core.Clock.Day;
                _holdfastDirty = false; // restore just raised state-change events
                GD.Print($"[Ashfall Godot] Holdfast S1 state restored (day {_core.Clock.Day}).");
            }

            RefreshIceRoadLabel();
            GD.Print($"[Ashfall Godot] Ice road ready. {_core.CatalogLine()}");
        }

        private void SetupHoldfastRuntime()
        {
            SetupIceRoad();
            SetupSurvivors();
            if (_holdfastRuntime != null)
            {
                _holdfastRuntime.Survivors = _survivors;
                return;
            }

            _holdfastRuntime = HoldfastRuntimeSession.Create(_core);
            _holdfastRuntime.Survivors = _survivors;
            if (_holdfastTerminal == null || !_holdfastTerminal.IsInsideTree())
            {
                _holdfastTerminal = new HoldfastTerminalPanel();
                AddChild(_holdfastTerminal);
            }
            _holdfastTerminal.BindSession(_holdfastRuntime);

            // ── Wire death event ──
            _holdfastRuntime.OnPlayerDied += OnPlayerDied;
            _holdfastRuntime.OnGameWon += OnGameWon;
        }

        /// <summary>
        /// Advance every daily-bound subsystem for a new sim day. Thin host
        /// orchestration: each session owns its own rules. Weather, caravans,
        /// medical drift, crafting progress, expedition ticks, and the Verdict
        /// reckoning all move forward together so the day is consistent.
        /// </summary>
        private void TickSimDay(int day)
        {
            // Moral choice: overnight settlement — pending legend overflow and
            // band-crossing faction events land here, after the campaign day
            // advance in CommitAdvance and never mid-scene.
            SetupMoralChoice();
            _moralChoice.Reconcile(day);

            SetupWorld();
            _world.TickDemo(24f);

            SetupCaravans();
            _caravans.TickDemo();

            SetupMedical();
            _medical.TickDemo(24f);

            SetupExpeditions();
            _expeditions.TickDemoHours(24f);

            // Hatch-return bridge (Exp 02): a returning expedition crosses the
            // hatch as a staged shelter scene. Expedition magnitudes are owned by
            // ExpeditionSystem and never changed here; the bridge only stages.
            SetupDutyRoster();
            var expeditions = _expeditions.Engine.CaptureState();
            if (expeditions != null && _dutyRoster != null)
            {
                for (int i = 0; i < expeditions.Count; i++)
                {
                    var ex = expeditions[i];
                    if (ex == null) continue;
                    if (ex.phase == (int)ExpeditionPhase.Completed && !string.IsNullOrEmpty(ex.survivorId))
                    {
                        // quest_roster_window opens the crisis window: multiple scenes allowed.
                        bool crisis = _dutyRoster.Quests.IsCrisisQuestActive();
                        _dutyRoster.BridgeHatchReturn(ex.survivorId, crisis: crisis);
                        break; // one hatch scene per night unless the window quest is active
                    }
                }
            }

            SetupCrafting();
            _crafting.CompleteAll(24f);

            SetupMaritime();
            if (_maritime.Dive.IsActive)
                _maritime.TickDiveDemo(60f);
            SetupDeepCoast();
            _deepCoast.TickDaily(day, _core.Weather);
            _deepCoastPanel?.SetSimDay(day);

            SetupSurvivors();
            _survivors.TickHour(24f);

            if (_holdfastRuntime != null && !_holdfastRuntime.IsDead)
            {
                _holdfastRuntime.Survivors = _survivors;
                _holdfastRuntime.TickDay();
            }

            SetupStartingLevel();
            _startingLevel.TickDay();

            SetupInventory();
            int foodToConsume = _startingLevel.System.State.rationPolicy == Ashfall.Core.StartingLevel.RationPolicy.Half ? 2 : 3;
            int waterToConsume = _startingLevel.System.State.rationPolicy == Ashfall.Core.StartingLevel.RationPolicy.Irradiated ? 0 : (_startingLevel.System.State.rationPolicy == Ashfall.Core.StartingLevel.RationPolicy.Half ? 2 : 3);
            _inventory.Remove("canned_food", foodToConsume);
            if (waterToConsume > 0)
                _inventory.Remove("clean_water", waterToConsume);
            else
                _inventory.Remove("irradiated_water", 2);

            TickVerdict(day, LivingDwellerCountEstimate());

            // Year of Ash (Days 180–360): advance the timeline + faction war +
            // deep-freeze + radon when the sim is inside the expansion window.
            if (day >= 180 && day <= 360)
            {
                SetupYearOfAsh();
                _yearOfAsh.TickDay(day);
            }

            // Muster (Exp 06) opens Day 260; escalate idempotently each day past it.
            if (day >= 260)
            {
                SetupMuster();
                _muster.Escalate(day);
            }

            SetupExpansions();
            if (_expansions.Greenhouse.PlotCount > 0)
                _expansions.TickGreenhouse(day);
            _expansions.Ledger.TickDaily(day);
            _expansions.TickCrossingQuests(day);

            SetupExpansionQuests();
            _expansionQuests.TickDay(day);

            // The Duty Roster (Exp 02) advances on the real day clock: the morning
            // snapshot comes from the REAL home occupants, and Holdfast state
            // (levy, membrane, waystation, ice road) feeds the chart's marks.
            SetupDutyRoster();
            _dutyRoster!.TickDay(BuildHomeOccupantSnapshot());
            SetupIceRoad(); // owns _core (IceRoad, Census, Brine)
            _dutyRoster.SyncHoldfastToDuty(_core.Census, _core.IceRoad, _expansions.Waystation, _core.Brine, day);
            _dutyRosterPanel?.RefreshView();
            if (_dutyRosterDirty) SaveDutyRoster();

            // The Silent Foundry (Exp 10) advances on the real day clock.
            SetupSilentFoundry();
            _silentFoundry.TickDaily(day);
            _silentFoundryPanel?.RefreshView();
            if (_foundryDirty) SaveExpansionHub();

            // The Disease Expansion advances on the real day clock: the exposure
            // pool is the duty-roster home occupants (threats among the people
            // actually in the shelter tonight). Outcome-only advance otherwise.
            SetupDisease();
            _disease.TickDaily(day);
            if (_expansionHubDirty) SaveExpansionHub();

            SetupGreenhouse();
            _greenhouse.TickDay(day, growLightHours: 6f, ashContaminationRate: 0.04f);

            TickPowerGrid(day);
            TickAllExpandedShelterSystems(day);

            // Phase 0 (psychological/medical effects) advances on the real day clock:
            // refresh environment signals from the world/shelter hosts, then tick all
            // ten systems for a full day.
            SetupPhase0();
            _phase0.CurrentDay = day;
            _phase0.IsInFalloutStorm = _world != null && _world.Weather.Current == Ashfall.Core.WeatherKind.FalloutStorm;
            _phase0.IsNightTime = day % 2 == 0; // night signal for trauma false-alarm rolls
            _phase0.TickDay(day);

            SetupEventAdapter();
            bool hydroAudit = _muster?.HydroBarons?.AdminReform ?? false;
            bool hydroSeized = _muster?.HydroBarons?.PlantSeized ?? false;
            bool osteophageInquiry = (_yearOfAsh != null && _yearOfAsh.Timeline.CurrentDay >= 205) || day >= 205;
            bool coldCountBroadcast = _muster?.ColdCount?.BroadcastSent ?? false;
            _hostEventAdapter?.EvaluateTriggers(day, hydroAudit, hydroSeized, osteophageInquiry, coldCountBroadcast);

            UpdateHud();
            SaveAll();
        }

        private void SaveHoldfast()
        {
            if (_core == null) return;
            if (HoldfastSaveStore.TrySave(_core.CaptureSave()))
            {
                _holdfastDirty = false;
                GD.Print($"[Ashfall Godot] Holdfast S1 save written (day {_core.Clock.Day}).");
            }
        }

        private void SaveHoldfastRuntime()
        {
            if (_holdfastRuntime == null) return;
            if (_holdfastRuntime.TrySave())
                GD.Print("[Ashfall Godot] Holdfast player/trade state written.");
        }

        /// <summary>Writes the S1 save only when a system changed since the last flush.</summary>
        private void FlushHoldfastIfDirty()
        {
            if (_holdfastDirty) SaveHoldfast();
        }

        private void RefreshIceRoadLabel()
        {
            if (_core == null) return;
            if (_iceRoadLabel != null)
                _iceRoadLabel.Text = _core.StatusLine() + "\n" + _core.BrineLine() + "\n" +
                    _core.QuestLine() + "\n" + _core.EndingLine();
            if (_catalogLabel != null)
                _catalogLabel.Text = _core.CatalogLine() + "\n" + _core.CensusLine();
            if (_briefingPreviewLabel != null)
                _briefingPreviewLabel.Text = HoldfastBriefingView.PreviewLine(_core.CurrentQuest);
        }

        private void OnHoldfastNewLedgerClicked()
        {
            SetupHoldfastRuntime();
            if (_holdfastTerminal != null)
            {
                _holdfastTerminal.PressNewLedger();
                _statusLabel.Text = _holdfastRuntime?.LastPersistenceMessage ?? "New ledger failed.";
            }
        }

        private void OnHoldfastOpenClicked()
        {
            SetupHoldfastRuntime();
            _holdfastTerminal.OpenTerminal();
            _statusLabel.Text = "Holdfast terminal open. Factions, supplies, inventory, trade, and save/load are live.";
        }

        private void OnTickIceRoadClicked()
        {
            if (_advanceTimerRemaining > 0) return; // already counting down

            var settings = AtomicWar.GodotApp.Settings.UserSettingsStore.Current;
            if (settings.ConfirmEndDay && !_advanceConfirmed)
            {
                _advanceTimerRemaining = AdvanceCountdownDefaultSeconds;
                _advanceCancelled = false;
                _statusLabel.Text = "Sleep in progress … press ESC or MENU to cancel";
                return;
            }

            CommitAdvance();
        }

        /// <summary>Cancel a pending sleep advance. Called from _UnhandledKeyInput
        /// when the player hits Escape, and from ReturnToMenu to prevent stale ticks.</summary>
        private void CancelAdvanceConfirmation()
        {
            if (!_advanceTimerRemaining.Equals(0))
            {
                _advanceCancelled = true;
                _advanceTimerRemaining = 0;
                _advanceConfirmed = false;
                if (_statusLabel != null)
                    _statusLabel.Text = "Advance cancelled.";
            }
        }

        /// <summary>Fully tick the simulation forward one day: advance every subsystem
        /// exactly once, then auto-save per settings.</summary>
        private void CommitAdvance()
        {
            SetupIceRoad();
            SetupCampaignDay();

            int targetDay = _core.Clock.Day + 1;

            // Re-entrance guard: if a previous CommitAdvance is still in flight,
            // or if the player hammered the button for an already-completed day,
            // refuse the second call. This is the only place that owns the gate.
            if (!_campaignDay.TryBegin(targetDay))
            {
                _statusLabel.Text = $"Day {targetDay} re-entrant guard tripped (skipped duplicate).";
                return;
            }

            try
            {
                string delta = _core.TickDay();
                _simDay = _core.Clock.Day;
                TickSimDay(_simDay);

                // Notify the coordinator (it tracks the last-advanced day and
                // lets the host build a typed report from owner results).
                _campaignDay.Advance(targetDay, new CampaignDayPersistenceAdapter(this));
                _campaignDayDirty = true;

                _audio?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.DayTransition);
                _statusLabel.Text = $"Day {_core.Clock.Day} advanced ({delta})";
                UpdateHud();

                ShowBriefingForDay(_simDay);

                var settings = AtomicWar.GodotApp.Settings.UserSettingsStore.Current;
                if (settings.AutoSaveOnDay) SaveAll();
            }
            finally
            {
                _advanceConfirmed = false;
                _advanceCancelled = false;
                _advanceTimerRemaining = 0;
                // Release the coordinator's gate so the next click can advance.
                _campaignDay.EndAdvance();
            }
        }

        private void OnCycleWeatherClicked()
        {
            SetupIceRoad();
            _core.CycleWeather();
            _statusLabel.Text =
                $"Weather set to {_core.Weather} ({_core.OutdoorCelsius:0}°C). Next tick uses this.";
            RefreshIceRoadLabel();
        }

        private void OnShowBriefingClicked()
        {
            SetupIceRoad();
            if (_core.QuestCount == 0)
            {
                _statusLabel.Text = "No Holdfast quests in catalog.";
                _codexViewer.Text = _core.CatalogLine();
                RefreshIceRoadLabel();
                return;
            }

            _codexViewer.Text =
                "=== HOLDFAST QUEST BRIEFING ===\n" +
                $"{_core.CatalogLine()}\n" +
                $"Showing {(_core.QuestIndex + 1)}/{_core.QuestCount}\n\n" +
                HoldfastBriefingView.FormatQuest(_core.CurrentQuest, _core.Catalog);
            _statusLabel.Text = HoldfastBriefingView.PreviewLine(_core.CurrentQuest);
            RefreshIceRoadLabel();
            _core.AdvanceQuest();
        }

        private void OnCensusLevyClicked()
        {
            SetupIceRoad();
            string result = _core.HonourDemoLevy();
            _statusLabel.Text = result;
            _codexViewer.Text =
                "=== CENSUS (Ashfall.Core) ===\n" +
                _core.CensusLine() + "\n" +
                "Named cap is three. Honour assigns them away until the levy days run out.\n";
            RefreshIceRoadLabel();
        }

        private void OnOrder12CClicked()
        {
            SetupIceRoad();
            bool wasActive = _core.Census.Order12CActive;
            _core.Activate12C();
            _statusLabel.Text = wasActive
                ? "Order 12-C already published. The unlisted are a reserve. The office will come south when the ice allows."
                : "Order 12-C published. Unlisted occupants of Allocation 12 are a labour reserve.";
            _codexViewer.Text =
                "=== ORDER 12-C (Ashfall.Core) ===\n" +
                _core.QuestLine() + "\n" +
                "\"You are living in a facility that authenticated for fourteen. " +
                "The fourteen did not arrive. I am not collecting you. I am scheduling you.\"\n" +
                "The Second List quest gates on the refuse branch or the membrane resolution.\n";
            RefreshIceRoadLabel();
        }

        private void OnCycleEndingClicked()
        {
            SetupIceRoad();
            string current = _core.Quests.State.endingId;
            // Cycle: none → schedule → reserve → dark road → tender → white → none.
            int index = -1;
            if (!string.IsNullOrEmpty(current))
                for (int i = 0; i < HoldfastEndings.All.Length; i++)
                    if (HoldfastEndings.All[i] == current) { index = i; break; }
            string next = index >= 0 && index + 1 < HoldfastEndings.All.Length
                ? HoldfastEndings.All[index + 1]
                : HoldfastEndings.None;

            if (string.IsNullOrEmpty(next))
            {
                _core.Quests.SetEnding(HoldfastEndings.None);
                _statusLabel.Text = "Ending disarmed. No ending armed — the road stays open.";
            }
            else
            {
                bool armed = _core.SetEnding(next);
                _statusLabel.Text = armed
                    ? $"Ending armed: {HoldfastEndings.DisplayName(next)} [{next}]. " +
                      "Arming a second ending overwrites the first — endings are exclusive."
                    : "Ending rejected: id not in the master list.";
            }
            _codexViewer.Text =
                "=== ENDINGS (Sprint 4) ===\n" +
                _core.EndingLine() + "\n" +
                "Five endings, mutually exclusive. The ice takes a column south and a column north.\n" +
                "Receipts in triplicate. Nobody is shot.\n";
            RefreshIceRoadLabel();
        }

        private void OnSaveHoldfastClicked()
        {
            SetupIceRoad();
            SaveHoldfast();
            SetupHoldfastRuntime();
            SaveHoldfastRuntime();
            _statusLabel.Text =
                $"Holdfast state saved (day {_core.Clock.Day}) → {HoldfastSaveStore.FileName} + {HoldfastTradeSaveStore.FileName}\n" +
                _core.StatusLine();
        }

        private void OnUnlockPlantClicked()
        {
            SetupIceRoad();
            bool wasUnlocked = _core.Brine.Unlocked;
            _core.UnlockPlant();
            _statusLabel.Text = wasUnlocked
                ? "Plant already unlocked. Salt trade is open."
                : "Plant unlocked. Steam rises from Membrane Hall. The Office has noticed the water.";
            _codexViewer.Text =
                "=== BRINE WATER (Ashfall.Core) ===\n" +
                _core.BrineLine() + "\n" +
                "Sector 4 dies of thirst; District 8 drowns in brine. Potability needs resin, iodine, heat.\n" +
                "Tick days to watch the membrane degrade.\n";
            RefreshIceRoadLabel();
        }

        private void OnRepairMembraneClicked()
        {
            SetupIceRoad();
            bool repaired = _core.RepairMembrane(4);
            _statusLabel.Text = repaired
                ? "Four resin drums rolled into the hall. " + _core.BrineLine()
                : "Repair rejected (resin drums must be positive).";
            _codexViewer.Text =
                "=== MEMBRANE CRISIS ===\n" +
                _core.BrineLine() + "\n" +
                "Resin above 40% restores steam; the Cluster rewarms to 14°C.\n";
            RefreshIceRoadLabel();
        }

        private void OnToggleOutfallClicked()
        {
            SetupIceRoad();
            _core.ToggleOutfallShift();
            _statusLabel.Text = _core.OutfallShifted
                ? "Outfall shift on — brine load cut to 55%."
                : "Outfall shift off — full brine load resumes.";
            _codexViewer.Text =
                "=== OUTFALL SHIFT ===\n" +
                _core.BrineLine() + "\n" +
                "Shifting the outfall costs bodies on the yard. It halves what the membrane eats.\n";
            RefreshIceRoadLabel();
        }

        private void OnViewCodexClicked()
        {
            // One surface: the bunker ledger is the lore archive.
            if (_journalBook != null) _journalBook.Open();
            LoadGameCatalogs();
            UpdateStatus();
        }

        private void OnDiagnosticsClicked()
        {
            var diag = new System.Text.StringBuilder();
            diag.AppendLine("=== ASHFALL SYSTEM DIAGNOSTICS (GODOT .NET) ===");
            diag.AppendLine($"Engine: Godot {Engine.GetVersionInfo()["string"]}");
            diag.AppendLine($"Target FPS: {Engine.MaxFps}");
            diag.AppendLine($"Current FPS: {Engine.GetFramesPerSecond():F1}");
            diag.AppendLine($"Static Memory: {OS.GetStaticMemoryUsage() / (1024 * 1024.0):F2} MB");
            diag.AppendLine($"GC Heap Memory: {GC.GetTotalMemory(false) / (1024 * 1024.0):F2} MB");
            diag.AppendLine($"Operating System: {OS.GetName()} ({OS.GetDistributionName()})");
            diag.AppendLine($"Architecture: {Engine.GetArchitectureName()}");
            diag.AppendLine($"Processors: {OS.GetProcessorCount()} cores");
            diag.AppendLine($"Video Adapter: {RenderingServer.GetVideoAdapterName()}");
            if (_journal != null)
            {
                diag.AppendLine();
                diag.AppendLine("=== JOURNAL STATE ===");
                diag.AppendLine($"Entries: {_journal.EntryCount}/64 · Unlocks: {_journal.CodexUnlockCount}");
                diag.AppendLine($"Unread: {_journal.HasUnread} · Ping: {_journal.NotificationPing} · Tab: {_journal.ActiveTab}");
                diag.AppendLine($"Open: {_journal.HudIsOpen} · Save: {JournalSaveStore.Exists}");
            }
            if (_core != null)
            {
                diag.AppendLine();
                diag.AppendLine("=== ICE ROAD (Ashfall.Core) ===");
                diag.AppendLine($"Unlocked: {_core.IceRoad.IsUnlocked}  Open: {_core.IceRoad.IsOpen}");
                diag.AppendLine($"Thickness: {_core.IceRoad.IceThicknessM:0.000} m  Window: {_core.IceRoad.WindowDaysRemaining}/{_core.IceRoad.State.windowLengthDays}");
                diag.AppendLine($"Weather: {_core.Weather}  Outdoor: {_core.OutdoorCelsius:0}°C");
                diag.AppendLine($"Gate blocked: {_core.GateBlocked}  Clerk: {_core.IceRoad.State.clerkStarted}");
                diag.AppendLine(_core.CatalogLine());
                diag.AppendLine(_core.CensusLine());
                diag.AppendLine(_core.BrineLine());
                diag.AppendLine(_core.QuestLine());
                diag.AppendLine(_core.EndingLine());
                diag.AppendLine($"Data: {_dataDir}");
                diag.AppendLine($"S1 save: {(HoldfastSaveStore.Exists ? HoldfastSaveStore.SavePath : "none")} · dirty: {_holdfastDirty}");
                diag.AppendLine();
                diag.AppendLine("=== HOLDFAST BRIEFING ===");
                diag.AppendLine(HoldfastBriefingView.FormatQuest(_core.CurrentQuest, _core.Catalog));
            }
            if (_yearOfAsh != null)
            {
                diag.AppendLine();
                diag.AppendLine("=== YEAR OF ASH (Ashfall.Core) ===");
                diag.AppendLine(_yearOfAsh.GetStatusSummary());
            }
            if (_dutyRoster != null)
            {
                diag.AppendLine();
                diag.AppendLine("=== DUTY ROSTER (Ashfall.Core) ===");
                diag.AppendLine(_dutyRoster.WallLine());
                diag.AppendLine(_dutyRoster.EncountersLine());
                diag.AppendLine("Save: " + (DutyRosterSaveStore.Exists ? DutyRosterSaveStore.SavePath : "none")
                    + " · dirty: " + _dutyRosterDirty);
            }
            if (_expansions != null)
            {
                diag.AppendLine();
                diag.AppendLine("=== EXPANSION HUB (Ashfall.Core) ===");
                diag.AppendLine("Save: " + (ExpansionHubSaveStore.Exists ? ExpansionHubSaveStore.SavePath : "none")
                    + " · dirty: " + _expansionHubDirty);
            }
            if (_doseLedger != null)
            {
                diag.AppendLine();
                diag.AppendLine("=== THE DOSE (Ashfall.Core) ===");
                diag.AppendLine(_doseLedger.DoseStatusLine());
                diag.AppendLine("Save: " + (DoseLedgerSaveStore.Exists ? DoseLedgerSaveStore.SavePath : "none")
                    + " · dirty: " + _doseLedgerDirty);
            }
            _codexViewer.Text = diag.ToString();
        }

    }
}
