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
            if (_holdfastRuntime != null) return;

            _holdfastRuntime = HoldfastRuntimeSession.Create(_core);
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

        private void SetupDutyRoster()
        {
            if (_dutyRoster != null) return;
            SetupJournal();
            _dutyRoster = DutyRosterHostSession.Create(_dataDir, log: null, journal: _journal);
            _dutyRoster.StateChanged += () => _dutyRosterDirty = true;

            // Cross-host roundtrip: a save written here (or by the Unity host) restores
            // the chart, marks, and encounter counters instead of starting blank.
            var save = DutyRosterSaveStore.TryLoad();
            if (save != null)
            {
                _dutyRoster.RestoreSave(save);
                _dutyRosterDirty = false; // restore just raised state-change events
                GD.Print($"[Ashfall Godot] Duty Roster state restored (day {_dutyRoster.Clock.Day}).");
            }

            _dutyRoster.Unlock(_simDay);
            RefreshRosterStatus();
            GD.Print($"[Ashfall Godot] Duty Roster ready. {_dutyRoster.CatalogLine()}");
        }

        private void RefreshRosterStatus()
        {
            if (_dutyRoster == null || _statusLabel == null) return;
            _statusLabel.Text =
                $"——— DUTY ROSTER ———\n" +
                _dutyRoster.WallLine() + "\n" +
                _dutyRoster.EncountersLine() + "\n" +
                _dutyRoster.MarksLine() + "\n" +
                $"Day {_simDay} · catalog: {_dutyRoster.CatalogLine()}";
        }

        private void SetupExpansions()
        {
            if (_expansions != null) return;
            _expansions = ExpansionHostSession.Create(_dataDir);
            _expansions.StateChanged += () => _expansionHubDirty = true;
            _expansions.OnCrossingStageNarrative += OnCrossingStageNarrative;

            // Cross-host roundtrip for waystation, standing record, crossing vouch,
            // and greenhouse plots.
            var save = ExpansionHubSaveStore.TryLoad();
            if (save != null)
            {
                _expansions.RestoreSave(save);
                _expansionHubDirty = false; // restore just raised state-change events
                GD.Print($"[Ashfall Godot] Expansion hub state restored (day {save.simDay}).");
            }

            _expansions.EnsureGreenhousePlots(3);
            RefreshExpansionsStatus();
            GD.Print("[Ashfall Godot] Expansion hub ready: waystation · standing record · crossing · greenhouse");
        }

        private void OnCrossingStageNarrative(Ashfall.Core.Crossing.CrossingStageNarrativeEvent evt)
        {
            if (evt == null) return;
            string tag = evt.isCompletion ? "[CHARTER COMPLETE]" : $"[NC STAGE {evt.stageIndex + 1}]";
            string line = $"{tag} {evt.questDisplayName}: {evt.stageText}";
            GD.Print($"[Ashfall Godot] Crossing narrative: {line}");
            if (_hostEventAdapter != null)
            {
                string eventId = $"event_crossing_{evt.questId}_{evt.stageIndex}_{(evt.isCompletion ? "complete" : "stage")}";
                _hostEventAdapter.TriggerEvent(eventId, _simDay);
            }
            if (_journal != null)
            {
                _journal.TryAddRawEntry(
                    $"crossing_{evt.questId}_{evt.stageIndex}_{(evt.isCompletion ? "complete" : "stage")}",
                    line,
                    null!,
                    _simDay);
                _journalDirty = true;
            }
            _statusLabel?.SetDeferred(Label.PropertyName.Text, line);
        }

        private void RefreshExpansionsStatus()
        {
            if (_expansions == null || _statusLabel == null) return;
            _statusLabel.Text =
                $"——— EXPANSION HUB (Standing Record · Crossing · Greenhouse) ———\n" +
                _expansions.StandingRecordLine() + "\n" +
                _expansions.CrossingLine() + "\n" +
                _expansions.GreenhouseLine() + "\n" +
                _expansions.WaystationLine() + "\n" +
                _expansions.ArbitrationLine() + "\n" +
                _expansions.LedgerLine() + "\n" +
                DiseaseStatusLine();
        }

        private void OnRosterInspectWallClicked()
        {
            SetupDutyRoster();
            _statusLabel.Text = _dutyRoster.InspectWall();
        }

        private void OnRosterPencilClicked()
        {
            SetupDutyRoster();
            _statusLabel.Text = _dutyRoster.ResolveChart(DutyRosterSystem.ChoiceWritePencil)
                + "\n" + _dutyRoster.TickDay();
            RefreshRosterStatus();
        }

        private void OnRosterInkClicked()
        {
            SetupDutyRoster();
            _statusLabel.Text = _dutyRoster.ResolveInk();
            RefreshRosterStatus();
        }

        private void OnRosterBurnClicked()
        {
            SetupDutyRoster();
            _statusLabel.Text = _dutyRoster.BurnChart();
            RefreshRosterStatus();
        }

        private void OnRosterTickNightClicked()
        {
            SetupDutyRoster();
            _simDay++;
            _dutyRoster.Clock.AdvanceDays(1);
            TickSimDay(_simDay);
            _statusLabel.Text = _dutyRoster.StartEncounter(ShelterEncounterSystem.KindNightSlate);
            RefreshRosterStatus();
        }

        /// <summary>
        /// Advance every daily-bound subsystem for a new sim day. Thin host
        /// orchestration: each session owns its own rules. Weather, caravans,
        /// medical drift, crafting progress, expedition ticks, and the Verdict
        /// reckoning all move forward together so the day is consistent.
        /// </summary>
        private void TickSimDay(int day)
        {
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

            if (_holdfastRuntime != null && !_holdfastRuntime.IsDead)
                _holdfastRuntime.TickDay();

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
            _silentFoundry.Engine.TickDaily(day);
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

        private void OnRosterVisitorClicked()
        {
            SetupDutyRoster();
            _statusLabel.Text = _dutyRoster.QueueVisitor(ShelterEncounterSystem.VisitorLen);
            RefreshRosterStatus();
        }

        private void OnRosterSecondWinterClicked()
        {
            SetupDutyRoster();
            _statusLabel.Text = _dutyRoster.ActivateSecondWinter();
            RefreshRosterStatus();
        }

        private void OnStandingRecordClicked()
        {
            SetupExpansions();
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("=== STANDING RECORD (Exp 03) ===");
            sb.AppendLine(_expansions.StandingRecordLine());
            sb.AppendLine(_expansions.RecordQuestLine());
            sb.AppendLine("Walk the route: Km 19 → Transit → Archive → Ministry → Weighbridge → Grange → Bridge → Lock → 12-B → Vault.");
            _codexViewer.Text = sb.ToString().TrimEnd();
            RefreshExpansionsStatus();
        }

        private void OnRecordWalkKm19Clicked()
        {
            SetupExpansions();
            var sb = new System.Text.StringBuilder();
            _expansions.UnlockRecord();
            _expansions.ArriveAtSite("loc_cut_kilometre_19");
            _expansions.EnterSiteRoom("room_km19_post");
            _expansions.InspectSiteRoom("room_km19_post");
            _expansions.EnterSiteRoom("room_km19_seam");
            sb.AppendLine(_expansions.RoomLine("loc_cut_kilometre_19", "room_km19_post"));
            sb.AppendLine();
            sb.AppendLine(_expansions.RoomLine("loc_cut_kilometre_19", "room_km19_seam"));
            _statusLabel.Text = sb.ToString().TrimEnd();
        }

        private void OnCrossingVouchClicked()
        {
            SetupExpansions();
            bool granted = _expansions.GrantVouch("npc_osran_kell");
            _statusLabel.Text = granted
                ? "Vouch granted by Osran Kell. The Crossing gate is open."
                : "Vouch refused (already granted, burned, or last resort spent).";
            RefreshExpansionsStatus();
        }

        private void OnCrossingBurnClicked()
        {
            SetupExpansions();
            bool burned = _expansions.BurnVouch();
            _statusLabel.Text = burned
                ? "Vouch burned. The gate is closed again — last resort remains available."
                : "Nothing to burn: no active vouch.";
            RefreshExpansionsStatus();
        }

        private void OnArbitrationLoadBackersClicked()
        {
            SetupExpansions();
            _expansions.LoadDefaultBackerPool();
            _statusLabel.Text = "Backer pool loaded: Osran Kell (principled), Mattis Cray (principled), Halden Mire, Bram Ostrowski, Leva Quist, Dessa Penn.";
            _codexViewer.Text = _expansions.ArbitrationLine();
            RefreshExpansionsStatus();
        }

        private void OnArbitrationCallStandingClicked()
        {
            SetupExpansions();
            if (_expansions.Arbitration.BackerPool.Count == 0)
            {
                _expansions.LoadDefaultBackerPool();
                _statusLabel.Text = "No backer pool — loaded defaults first.";
            }
            int day = _core != null ? _core.Clock.Day : _simDay;
            string topic = "quest_crossing_the_terms";
            bool called = _expansions.Arbitration.CallStanding(topic, day);
            _expansions.Arbitration.DeclareBacker(topic, CrossingIds.NpcOsran);
            _expansions.Arbitration.DeclareBacker(topic, CrossingIds.NpcMattis);
            _expansions.Arbitration.DeclareBacker(topic, "npc_halden_mire");
            _statusLabel.Text = called
                ? $"Standing called on '{topic}' with 3 backers (Osran, Mattis, Halden). Ruling: {_expansions.Arbitration.GetRuling(topic)?.shape}"
                : "Standing already held — overturn first or call a different topic.";
            _codexViewer.Text = _expansions.ArbitrationLine();
            RefreshExpansionsStatus();
        }

        private void OnArbitrationBribeClicked()
        {
            SetupExpansions();
            if (_expansions.Arbitration.BackerPool.Count == 0)
            {
                _expansions.LoadDefaultBackerPool();
                _statusLabel.Text = "No backer pool — loaded defaults first.";
            }
            // Set up a fresh ruling on a new topic
            string topic = CrossingIds.ScaleIntegrity;
            int day = _core != null ? _core.Clock.Day : _simDay;
            _expansions.Arbitration.CallStanding(topic, day);
            _expansions.Arbitration.DeclareBacker(topic, CrossingIds.NpcOsran);
            // Try bribing a principled backer (refused) and an unprincipled one (accepted)
            var resultPrincipled = _expansions.Arbitration.TryBribeBacker(topic, CrossingIds.NpcMattis);
            var resultBought = _expansions.Arbitration.TryBribeBacker(topic, "npc_bram_ostrowski");
            _expansions.Arbitration.DeclareBacker(topic, "npc_leva_quist");
            _statusLabel.Text = $"Bribe results: Mattis={resultPrincipled}, Bram={resultBought}. Ruling: {_expansions.Arbitration.GetRuling(topic)?.shape}";
            _codexViewer.Text = _expansions.ArbitrationLine();
            RefreshExpansionsStatus();
        }

        private void OnArbitrationOverturnClicked()
        {
            SetupExpansions();
            if (_expansions.Arbitration.BackerPool.Count == 0)
            {
                _expansions.LoadDefaultBackerPool();
            }
            string topic = "quest_crossing_the_terms";
            int day = _core != null ? _core.Clock.Day : _simDay;
            // Ensure a ruling exists to overturn
            if (!_expansions.Arbitration.IsRulingActive(topic))
            {
                _expansions.Arbitration.CallStanding(topic, day);
                _expansions.Arbitration.DeclareBacker(topic, CrossingIds.NpcOsran);
                _expansions.Arbitration.DeclareBacker(topic, CrossingIds.NpcMattis);
                _expansions.Arbitration.DeclareBacker(topic, "npc_halden_mire");
            }
            bool overturned = _expansions.Arbitration.OverturnRuling(topic,
                new List<string> { "npc_bram_ostrowski", "npc_leva_quist", "npc_halden_mire" });
            _statusLabel.Text = overturned
                ? "Ruling overturned! Counter-backers (Bram, Leva, Halden) hold the Crossing now."
                : "Overturn failed — need 3+ different, living backers.";
            _codexViewer.Text = _expansions.ArbitrationLine();
            RefreshExpansionsStatus();
        }

        private void OnLedgerSignClicked()
        {
            SetupExpansions();
            string debtor = CrossingIds.NpcWyn;
            bool firstRead = _expansions.Ledger.PresentContract(debtor, 12f, 30, 0.2f, "the pledged grain");
            bool secondRead = _expansions.Ledger.PresentContract(debtor, 12f, 30, 0.2f, "the pledged grain");
            bool signed = _expansions.Ledger.SignContract(debtor, _core != null ? _core.Clock.Day : _simDay);
            _statusLabel.Text = $"Contract for {debtor}: first reading={firstRead}, second reading={secondRead}, signed={signed}.";
            _codexViewer.Text = _expansions.LedgerLine();
            RefreshExpansionsStatus();
        }

        private void OnLedgerTickClicked()
        {
            SetupExpansions();
            int day = _core != null ? _core.Clock.Day : _simDay;
            _expansions.Ledger.TickDaily(day);
            _statusLabel.Text = "Ledger day ticked. " + _expansions.LedgerLine();
            _codexViewer.Text = _expansions.LedgerLine();
            RefreshExpansionsStatus();
        }

        private void OnLedgerPayClicked()
        {
            SetupExpansions();
            string debtor = CrossingIds.NpcWyn;
            bool paid = _expansions.Ledger.PayContract(debtor, _core != null ? _core.Clock.Day : _simDay);
            _statusLabel.Text = paid
                ? $"Contract for {debtor} paid in full. The ink is history."
                : "Payment failed — no signed contract or already paid.";
            _codexViewer.Text = _expansions.LedgerLine();
            RefreshExpansionsStatus();
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

        private void SaveDutyRoster()
        {
            if (_dutyRoster == null) return;
            if (DutyRosterSaveStore.TrySave(_dutyRoster.CaptureSave()))
            {
                _dutyRosterDirty = false;
                GD.Print($"[Ashfall Godot] Duty Roster save written (day {_dutyRoster.Clock.Day}).");
            }
        }

        private void FlushDutyRosterIfDirty()
        {
            if (_dutyRosterDirty) SaveDutyRoster();
        }

        private void SaveExpansionHub()
        {
            if (_expansions == null) return;
            int day = _core != null ? _core.Clock.Day : _simDay;
            if (ExpansionHubSaveStore.TrySave(_expansions.CaptureSave(day)))
            {
                _expansionHubDirty = false;
                GD.Print($"[Ashfall Godot] Expansion hub save written (day {day}).");
            }
        }

        private void FlushExpansionHubIfDirty()
        {
            if (_expansionHubDirty || _foundryDirty) SaveExpansionHub();
        }

        private void CloseExpansionsHubPanel()
        {
            if (_expansionsHubPanel != null) _expansionsHubPanel.Visible = false;
        }

        private void CloseStandingRecordPanel()
        {
            if (_standingRecordPanel != null) _standingRecordPanel.Visible = false;
        }

        private void CloseCenturySeedPanel()
        {
            if (_centurySeedPanel != null) _centurySeedPanel.Visible = false;
        }

        private void CloseEpiloguePanel()
        {
            if (_epiloguePanel != null) _epiloguePanel.Visible = false;
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

        private void CloseDutyRosterPanel()
        {
            _dutyRosterPanel.Visible = false;
        }

        private void CloseDutyRosterDetailPanel()
        {
            _dutyRosterDetailPanel.Visible = false;
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
