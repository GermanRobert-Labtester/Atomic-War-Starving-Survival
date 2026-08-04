using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        private void SeedKnowledgeMap()
        {
            if (KnowledgeMap == null) return;

            // Prefer proc-gen map nodes (authoritative per-playthrough layout)
            if (GeneratedMap?.Nodes != null)
            {
                for (int i = 0; i < GeneratedMap.Nodes.Count; i++)
                {
                    var n = GeneratedMap.Nodes[i];
                    if (n == null || string.IsNullOrEmpty(n.NodeId) || n.IsShelter) continue;
                    KnowledgeMap.SeedTile(n.NodeId, n.TrueRad, n.RumoredRad, 1f);
                }
            }

            // Also seed catalog locations if present (legacy / static sites)
            if (_locationCatalog?.locations == null) return;
            var rng = new System.Random(_worldSeed + 17);
            foreach (var loc in _locationCatalog.locations)
            {
                if (loc == null || string.IsNullOrEmpty(loc.id)) continue;
                if (KnowledgeMap.GetTile(loc.id) != null) continue; // already seeded from map
                float rumorScale = 0.4f + (float)rng.NextDouble() * 0.4f;
                KnowledgeMap.SeedTile(loc.id, loc.baseRadsPerHour, loc.baseRadsPerHour * rumorScale, 1f);
            }
        }

        private void RefreshMapKnowledgeHUD()
        {
            if (_hud == null || KnowledgeMap == null) return;
            bool hasGeiger = Inventory != null && Inventory.HasWorkingGeiger();
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;
            KnowledgeMap.GetAllPlayerViews(_knowledgeViewBuffer, day, hasGeiger);
            int calAge = -1;
            var geiger = Inventory?.GetBestGeigerState();
            if (geiger != null)
            {
                calAge = InstrumentDevice.DaysSinceCalibration(geiger, day);
            }
            _hud.OnMapKnowledgeUpdated(_knowledgeViewBuffer, hasGeiger, calAge);
        }

        /// <summary>Resync the pooled inventory icon strip from live stock.</summary>
        private void RefreshInventoryStrip()
        {
            if (_hud == null) return;
            var strip = _hud.InventoryStripUI;
            if (strip != null)
                strip.Sync(Inventory);
            // Corpse / rusted-can counts also drive the Internal Horror strip.
            RefreshInternalHorrorHud();
        }

        /// <summary>
        /// Push corpse / fire / coma / contaminated-food state into InternalHorrorHUD.
        /// Safe when systems are not yet constructed.
        /// </summary>
        private void RefreshInternalHorrorHud()
        {
            if (_hud == null) return;
            var horror = _hud.EnsureInternalHorrorHud();
            if (horror == null) return;

            var snap = BuildInternalHorrorSnapshot();
            horror.ApplySnapshot(snap);

            // Auto-open fire panel once per room when a new blaze starts.
            if (snap?.Fires != null)
            {
                var live = new HashSet<string>();
                for (int i = 0; i < snap.Fires.Length; i++)
                {
                    var f = snap.Fires[i];
                    if (f == null || !f.IsOnFire || string.IsNullOrEmpty(f.RoomId)) continue;
                    live.Add(f.RoomId);
                    if (_fireAlertShownRooms.Add(f.RoomId) && !horror.IsFirePanelOpen)
                        horror.OpenFirePanel(f.RoomId);
                }
                // Drop rooms that are no longer on fire so a re-ignition re-prompts.
                _fireAlertShownRooms.RemoveWhere(id => !live.Contains(id));
            }
            else
            {
                _fireAlertShownRooms.Clear();
            }
        }

        /// <summary>Wire Internal Horror HUD action callbacks once.</summary>
        private void WireInternalHorrorHud()
        {
            if (_hud == null) return;
            var horror = _hud.EnsureInternalHorrorHud();
            if (horror == null) return;

            horror.OnBuryRequested -= HandleBuryTheDead;
            horror.OnBuryRequested += HandleBuryTheDead;
            horror.OnProcessFertilizerRequested -= HandleProcessFertilizer;
            horror.OnProcessFertilizerRequested += HandleProcessFertilizer;
            horror.OnFightFireRequested -= HandleFightFire;
            horror.OnFightFireRequested += HandleFightFire;
            horror.OnSealBulkheadRequested -= HandleSealBulkhead;
            horror.OnSealBulkheadRequested += HandleSealBulkhead;

            // Inventory corpse "click" → open dispose panel.
            var strip = _hud.InventoryStripUI;
            if (strip != null)
            {
                strip.OnIconActivated -= HandleInventoryIconActivated;
                strip.OnIconActivated += HandleInventoryIconActivated;
            }

            RefreshInternalHorrorHud();
        }

        /// <summary>
        /// Inventory strip activate (click / Enter): corpse stacks open dispose UI.
        /// </summary>
        private void HandleInventoryIconActivated(AtomicWar._Game.UI.InventoryIcon icon)
        {
            if (icon == null) return;
            if (icon.IsCorpse || icon.HasDisposeActions)
            {
                OpenCorpseDisposePanel();
            }
        }

        private void HandleBuryTheDead()
        {
            if (CorpseSystem == null) return;
            var digger = FindFirstLivingSurvivor();
            if (digger == null) return;
            float daylight = PhotoperiodSystem != null
                ? PhotoperiodSystem.EffectiveDaylightHours
                : CorpseManagementSystem.BuryHours;
            if (CorpseSystem.BuryTheDead(digger, daylight))
            {
                Debug.Log($"[Internal Horror] {digger.DisplayName} buried the dead. Four hours of light, gone.");
                RefreshInventoryStrip();
            }
        }

        private void HandleProcessFertilizer()
        {
            if (CorpseSystem == null) return;
            var processor = FindFirstLivingSurvivor();
            if (processor == null) return;
            if (CorpseSystem.ProcessForFertilizer(processor))
            {
                Debug.Log($"[Internal Horror] {processor.DisplayName} processed a body for fertilizer. Nobody speaks.");
                RefreshInventoryStrip();
            }
        }

        private void HandleFightFire(string roomId)
        {
            if (AtmosphereSystem == null || string.IsNullOrEmpty(roomId)) return;
            var fighter = FindFirstLivingSurvivor();
            if (fighter == null) return;
            bool out_ = AtmosphereSystem.FightFire(roomId, fighter, NeedsSystem);
            Debug.Log(out_
                ? $"[Internal Horror] {fighter.DisplayName} put out the fire in {roomId}."
                : $"[Internal Horror] {fighter.DisplayName} fought the fire in {roomId}. Still burning.");
            RefreshInternalHorrorHud();
        }

        private void HandleSealBulkhead(string roomId)
        {
            if (AtmosphereSystem == null || string.IsNullOrEmpty(roomId)) return;
            if (AtmosphereSystem.SealBulkhead(roomId, Shelter))
            {
                Debug.Log($"[Internal Horror] Bulkhead sealed on {roomId}. Whatever was inside stays inside.");
                RefreshInternalHorrorHud();
            }
        }

        /// <summary>Close corpse dispose and/or fire panels (Esc).</summary>
        public void CloseInternalHorrorPanels()
        {
            if (_hud == null) return;
            var horror = _hud.EnsureInternalHorrorHud();
            if (horror == null) return;
            if (horror.IsCorpsePanelOpen) horror.CloseCorpsePanel();
            if (horror.IsFirePanelOpen) horror.CloseFirePanel();
        }

        /// <summary>
        /// Apply layout-specific modifications to the shelter: rooms, starting
        /// modules, anomalies, and traits (Prompts #79-#84).
        /// </summary>
        /// <summary>
        /// Sprinkle the 10 quest location nodes onto the generated map (Prompts #85-#94).
        /// </summary>
        private void InjectQuestNodesIntoMap()
        {
            if (GeneratedMap == null) return;
            var defs = Data.LocationQuestNodeFactory.AllDefinitions();
            for (int i = 0; i < defs.Count; i++)
            {
                var existingNode = GeneratedMap.GetNode(defs[i].NodeId);
                if (existingNode != null)
                {
                    // Override existing random node with quest data.
                    existingNode.DisplayName = defs[i].DisplayName;
                    existingNode.DangerLevel = defs[i].DangerLevel;
                    existingNode.TrueRad = defs[i].TrueRad;
                    existingNode.HasUxo = defs[i].HasUxo;
                    existingNode.LootTableId = defs[i].LootTableId;
                }
                else
                {
                    // Inject new quest node into the map.
                    var node = Data.LocationQuestNodeFactory.ToMapNode(defs[i]);
                    GeneratedMap.Nodes.Add(node);
                }
            }
        }

        private void ApplyLayoutTrait(AtomicWar._Game.Shelter.ShelterLayoutTrait trait)
        {
            switch (trait)
            {
                case AtomicWar._Game.Shelter.ShelterLayoutTrait.RootCellar:
                    // High starting food, high mold risk.
                    if (_storesRoom != null) _storesRoom.HasMold = true;
                    if (Inventory != null && _itemCatalog != null)
                    {
                        var food = _itemCatalog.GetById("canned_food");
                        if (food != null) Inventory.Add(food, 8);
                    }
                    break;
                case AtomicWar._Game.Shelter.ShelterLayoutTrait.FallenBeam:
                    // Stairs blocked — hatch requires Saw to access.
                    break;
                case AtomicWar._Game.Shelter.ShelterLayoutTrait.DirtFloor:
                    // Integrity degrades faster (handled by multiplier in StructuralIntegritySystem).
                    break;
                case AtomicWar._Game.Shelter.ShelterLayoutTrait.ExposedHatch:
                    // No rubble shielding; hatch is vulnerable.
                    break;
                case AtomicWar._Game.Shelter.ShelterLayoutTrait.Flooded:
                    // Rooms start with water — must pump.
                    break;
                case AtomicWar._Game.Shelter.ShelterLayoutTrait.PrepperCache:
                    // Extra starting scrap.
                    if (Inventory != null && _itemCatalog != null)
                    {
                        var scrap = _itemCatalog.GetById("electronic_scrap");
                        if (scrap != null) Inventory.Add(scrap, 3);
                    }
                    break;
                case AtomicWar._Game.Shelter.ShelterLayoutTrait.SharedPipe:
                    // Water anomaly handled by HouseToBunkerSystem.
                    break;
            }
        }

        private void SeedStartingInventory()
        {
            if (_itemCatalog == null) return;
            foreach (var item in _itemCatalog.items)
            {
                if (item == null) continue;
                // Give a reasonable starting stock
                int amount = item.type switch
                {
                    ItemType.Food => 10,
                    ItemType.Water => 10,
                    ItemType.Iodine => 5,
                    ItemType.AntiRad => 3,
                    ItemType.Fuel => 8,
                    ItemType.Filter => 3,
                    ItemType.Material => 15,
                    _ => 1
                };
                Inventory.Add(item, amount);
            }
        }

        /// <summary>
        /// Mental-break sabotage: disable or degrade a random shelter module.
        /// Hosted in Core so Survivors does not reference Shelter.
        /// </summary>
        private void ForceMentalBreakSabotage(System.Random rng)
        {
            if (Shelter == null || Shelter.Modules == null || Shelter.Modules.Count == 0) return;
            if (rng == null) rng = new System.Random();
            int idx = rng.Next(Shelter.Modules.Count);
            var mod = Shelter.Modules[idx];
            if (mod == null) return;
            if (mod.IsEnabled)
                mod.IsEnabled = false;
            else
                mod.FilterHealth = Mathf.Max(0f, mod.FilterHealth - 25f);
        }

        private void CreateSurvivor(string id, string name)
        {
            var sv = new Survivor { Id = id, DisplayName = name };
            // Elena is the medic by default; others baseline
            if (id == "sv_elena") sv.MedicalSkill = 0.85f;
            else if (id == "sv_marcus") sv.MedicalSkill = 0.35f;
            else sv.MedicalSkill = 0.25f;
            // Default room assignment so the MentalBreakSystem has room
            // boundaries from day 1 (Prompt #29 follow-up). Elena stays
            // near the bed in quarters; Marcus watches the stores; Suki
            // is in the entry hallway (closest to the hatch).
            if (id == "sv_elena") sv.CurrentRoomId = "quarters";
            else if (id == "sv_marcus") sv.CurrentRoomId = "stores";
            else if (id == "sv_suki") sv.CurrentRoomId = "entry";
            Survivors.Add(sv);
            NeedsSystem.Register(sv);
            RadiationSystem.Register(sv);
        }

        /// <summary>
        /// One-shot: stable RNGs + non-allocating callbacks for the hourly tick.
        /// Safe to call more than once (idempotent).
        /// </summary>
        private void WarmDayTickCaches()
        {
            _mentalBreakRng ??= CreateSaltedRng(_worldSeed, "mental_break");
            _phantomRng ??= CreateSaltedRng(_worldSeed, "phantom");
            _eventCtxRng ??= CreateSaltedRng(_worldSeed, "event_ctx");
            _aiRng ??= CreateSaltedRng(_worldSeed, "ai");
            _getSurvivorsCached ??= () => Survivors;
            _getFactionTrustEffective ??= factionId =>
                EconomySystem != null ? EconomySystem.GetEffectiveTrust(factionId) : 0f;
            _getFactionTrustStored ??= factionId =>
                EconomySystem != null ? EconomySystem.GetTrust(factionId) : 0f;
            _scheduleEventCached ??= (eventId, fireDay, originFlag) =>
                EventRunner?.ScheduleEvent(eventId, fireDay, originFlag);
            _onEventFlagChangedCached ??= (flagId, value) =>
            {
                if (SaveSystem != null)
                    SaveSystem.SetWorldFlag(flagId, value);
            };
            _tryApplyPedalCostCached ??= TryApplyPedalCost;
        }

        /// <summary>Allocation-free weather name for PowerNetwork (no Enum.ToString).</summary>
        private static string WeatherNameOf(WeatherKind kind)
        {
            return kind switch
            {
                WeatherKind.Clear => "Clear",
                WeatherKind.Rain => "Rain",
                WeatherKind.Overcast => "Overcast",
                WeatherKind.Ashfall => "Ashfall",
                WeatherKind.FalloutStorm => "FalloutStorm",
                WeatherKind.Blizzard => "Blizzard",
                WeatherKind.BlackRain => "BlackRain",
                _ => "Clear"
            };
        }

        private void CheckWinLose()
        {
            if (VictoryProject == null || VictoryProject.IsTerminal) return;
            if (Survivors == null) return;

            int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;

            // Loss: all survivors dead → death-screen by cause (rads / hunger / breakdowns).
            VictoryProject.EvaluateLoss(Survivors, day);

            if (EndgameEngine != null && !EndgameEngine.Result.IsTerminal)
            {
                bool isExtractionUnlocked = VictoryProject != null && VictoryProject.ExtractionUnlocked;
                bool isHydroponicsWorking = Shelter != null && Shelter.IsGrowLightActive;
                int deadCount = 0;
                for (int i = 0; i < Survivors.Count; i++)
                {
                    if (Survivors[i] != null && !Survivors[i].IsAlive) deadCount++;
                }

                EndgameEngine.Evaluate(
                    day,
                    Survivors,
                    Shelter,
                    isExtractionUnlocked,
                    isHydroponicsWorking,
                    deadCount);
            }
        }

        /// <summary>Record a resolved moral dilemma for the endgame tally.</summary>
        public void RecordMoralChoice()
        {
            VictoryProject?.RecordMoralChoice();
        }

        /// <summary>
        /// Audit C-1: per-hour clothing degradation tick. Drives
        /// <see cref="ClothingSystem.Tick"/> for each living survivor. The
        /// humidity source is the survivor's current room (if the
        /// atmosphere system reports a humidity value) or 0.5f default.
        /// </summary>
        private void TickClothing(float gameHours)
        {
            if (ClothingSystem == null || Survivors == null) return;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                float humidity = 0.5f;
                if (!string.IsNullOrEmpty(sv.CurrentRoomId) && Shelter != null && Shelter.Rooms != null)
                {
                    for (int r = 0; r < Shelter.Rooms.Count; r++)
                    {
                        var room = Shelter.Rooms[r];
                        if (room != null && string.Equals(room.RoomId, sv.CurrentRoomId, System.StringComparison.Ordinal))
                        {
                            humidity = room.Humidity;
                            break;
                        }
                    }
                }
                ClothingSystem.Tick(sv, gameHours, humidity);
            }
        }

        private void ApplyEndgame(EndgameSummaryData summary)
        {
            if (summary == null) return;
            IsGameOver = true;
            GameOverReason = summary.Reason ?? summary.OutcomeTitle;
            if (GameState != null)
            {
                GameState.IsPaused = true;
                GameState.Phase = GamePhase.GameOver;
            }
            // Halt TimeSystem by not ticking (Update already gates on Phase/IsGameOver).
            PushEndgameSummaryToHud(summary);
            Debug.Log($"[GameBootstrap] ENDGAME ({summary.State}): {summary.OutcomeTitle} — {summary.Reason}");
        }

        private void PushEndgameSummaryToHud(EndgameSummaryData summary)
        {
            if (_hud == null || summary == null) return;
            var ui = _hud.EnsureEndgameSummary();
            if (ui == null) return;
            ui.Show(
                summary.State.ToString(),
                summary.OutcomeTitle,
                summary.OutcomeBody,
                summary.DeathScreen == DeathScreenKind.None ? string.Empty : summary.DeathScreen.ToString(),
                summary.DaysSurvived,
                summary.TotalRadiationAbsorbed,
                summary.MoralChoicesMade,
                summary.MilitaryIntelDecrypted,
                summary.ExtractionUnlocked,
                summary.VehicleEscapeUsed);
        }

        /// <summary>
        /// Runtime item defs for tests / missing catalog entries (engine, parts, fuel).
        /// </summary>
        private static ItemDefinition MakeRuntimeItem(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.stackMax = id == VictoryProjectManager.EngineItemId ? 1 : 99;
            item.weight = 0.1f;
            if (id == VictoryProjectManager.EngineItemId)
            {
                item.type = ItemType.Tool;
                item.durability = 100f;
            }
            else if (id == VictoryProjectManager.FuelItemId)
            {
                item.type = ItemType.Fuel;
            }
            else
            {
                item.type = ItemType.Material;
            }
            return item;
        }

        private void EndGame(string reason, string outcome)
        {
            // Legacy path — prefer VictoryProject triggers.
            IsGameOver = true;
            GameOverReason = reason;
            GameState.Phase = GamePhase.GameOver;
            Debug.Log($"[GameBootstrap] GAME OVER ({outcome}): {reason}");
        }

        public void PauseGame()
        {
            GameState.IsPaused = true;
            GameState.Phase = GamePhase.Paused;
        }

        public void ResumeGame()
        {
            GameState.IsPaused = false;
            GameState.Phase = GamePhase.Running;
        }

        /// <summary>Toggle fast-forward: 1x <-> 3x (keybind F). Simulation-scaled only; Unity's Time.timeScale is untouched.</summary>
        public void ToggleFastForward()
        {
            if (TimeSystem == null) return;
            TimeSystem.SetTimeScale(TimeSystem.TimeScale > 1.5f ? 1f : FastForwardScale);
        }

        /// <summary>Explicit simulation speed (clamped by TimeSystem). For UI buttons/tests.</summary>
        public void SetTimeScale(float scale)
        {
            TimeSystem?.SetTimeScale(scale);
        }

        public void SaveGame(string slotId = "quicksave")
        {
            SnapshotRadioHudToInterceptSystem();
            SaveSystem.Save(slotId);
            _diagnosticsOverlay?.NotifySave(slotId);
        }

        public void LoadGame(string slotId = "quicksave")
        {
            if (SaveSystem.Load(slotId))
            {
                // Restore endgame terminal state from VictoryProject if present.
                if (VictoryProject != null && VictoryProject.IsTerminal)
                {
                    IsGameOver = true;
                    GameOverReason = VictoryProject.TerminalReason;
                    if (GameState != null) GameState.Phase = GamePhase.GameOver;
                    if (VictoryProject.LastSummary != null)
                        PushEndgameSummaryToHud(VictoryProject.LastSummary);
                }
                else
                {
                    IsGameOver = false;
                    GameOverReason = null;
                    _hud?.EnsureEndgameSummary()?.Clear();
                }
                // Intercept log + open/unread/tuner restored — refresh HUD strip.
                SyncRadioInterceptHudFromLog();
                SyncJournalBookFromSystem();
                // Corpse counts / fire rooms / care urgency after atmosphere+inventory restore.
                RefreshInventoryStrip();
            }
        }

        /// <summary>Toggle diegetic journal book (keybind J).</summary>
        public void ToggleJournalBook()
        {
            var book = _hud?.EnsureJournalBook();
            if (book == null) return;
            book.Toggle();
            if (JournalSystem != null)
            {
                JournalSystem.HudIsOpen = book.IsOpen;
                if (book.IsOpen)
                    JournalSystem.MarkRead();
            }
        }

        /// <summary>Open journal book and clear unread / ping.</summary>
        public void OpenJournalBook()
        {
            var book = _hud?.EnsureJournalBook();
            book?.Open();
            if (JournalSystem != null)
            {
                JournalSystem.HudIsOpen = true;
                JournalSystem.MarkRead();
            }
        }

        /// <summary>
        /// Copy live radio strip presentation into the intercept system so
        /// SaveSystem.CaptureState persists open / unread / tuner index.
        /// </summary>
        public void SnapshotRadioHudToInterceptSystem()
        {
            if (FactionRadioIntercepts == null) return;
            var strip = _hud != null ? _hud.EnsureRadioInterceptHud() : null;
            if (strip == null) return;
            FactionRadioIntercepts.HudIsOpen = strip.IsOpen;
            FactionRadioIntercepts.HudHasUnread = strip.HasUnread;
            FactionRadioIntercepts.HudTunerIndex = strip.TunerIndex;
        }

        public void ConsumeItem(Survivor sv, ItemDefinition item)
        {
            if (sv == null || item == null || !sv.IsAlive) return;
            if (Inventory == null || !Inventory.Consume(item, sv, RadiationSystem, NeedsSystem))
                return;

            // Prompt #13 — poisoned iodine looks clean until swallowed.
            SabotagedCacheSystem?.TryApplyPoisonOnConsume(item, sv, MedicalSystem);
        }

        public void CraftRecipe(Recipe recipe)
        {
            if (recipe == null) return;
            CraftingSystem.StartCraft(recipe);
        }

        public void SelectEventChoice(int choiceIndex)
        {
            // Applies to the most recently triggered event context
            if (EventRunner.ActiveConsequences.Count > 0 || EventRunner.Pool.Count > 0)
            {
                // EventModalUI handles this via its own Bind
            }
        }

        /// <summary>Open the wasteland map screen (UI).</summary>
        public void OpenMapScreen()
        {
            _hud?.MapScreenUI?.Open();
        }

        /// <summary>Open the workbench disassembly / repair / hatch-install screen.</summary>
        public void OpenWorkbench()
        {
            _hud?.WorkbenchUI?.Open();
        }

        /// <summary>Toggle workbench panel (keybind B).</summary>
        public void ToggleWorkbench()
        {
            _hud?.WorkbenchUI?.Toggle();
        }

        /// <summary>Open hatch defense status panel.</summary>
        public void OpenHatchDefense()
        {
            _hud?.HatchDefenseHUD?.Open();
        }

        /// <summary>Toggle hatch defense panel (keybind H).</summary>
        public void ToggleHatchDefense()
        {
            _hud?.HatchDefenseHUD?.Toggle();
        }

        /// <summary>Open the expanded radio intercept log.</summary>
        public void OpenRadioInterceptLog()
        {
            _hud?.EnsureRadioInterceptHud()?.Open();
        }

        /// <summary>Toggle expanded radio intercept log (keybind R).</summary>
        public void ToggleRadioInterceptLog()
        {
            _hud?.EnsureRadioInterceptHud()?.Toggle();
        }

        /// <summary>Cycle radio frequency filter forward (keybind ]).</summary>
        public void CycleRadioTunerNext()
        {
            _hud?.EnsureRadioInterceptHud()?.CycleTunerNext();
        }

        /// <summary>Cycle radio frequency filter backward (keybind [).</summary>
        public void CycleRadioTunerPrev()
        {
            _hud?.EnsureRadioInterceptHud()?.CycleTunerPrev();
        }

        private void PushRadioInterceptToHud(FactionRadioInterceptSystem.InterceptEntry entry)
        {
            if (entry == null || _hud == null) return;
            var strip = _hud.EnsureRadioInterceptHud();
            strip?.Push(entry.Message, entry.Kind, entry.FactionId, entry.Day);
        }

        private void PushJournalEntryToHud(JournalEntry entry)
        {
            if (entry == null || _hud == null) return;
            var book = _hud.EnsureJournalBook();
            book?.Push(entry);
        }

        /// <summary>Rebuild journal book from JournalSystem (WireHUD / load).</summary>
        public void SyncJournalBookFromSystem()
        {
            if (_hud == null || JournalSystem == null) return;
            var book = _hud.EnsureJournalBook();
            if (book == null) return;
            book.SetEntries(JournalSystem.Entries);
            book.ApplyUiState(
                JournalSystem.HudIsOpen,
                JournalSystem.HasUnread,
                JournalSystem.NotificationPing);
        }

        /// <summary>
        /// Bind the intercept strip dial to RadioTunerSystem frequencies so
        /// [ / ] retunes intel extraction and filters faction intercepts together.
        /// Safe to call multiple times (rebinds bands + handler).
        /// </summary>
        public void WireRadioInterceptTuner()
        {
            if (_hud == null || RadioTunerSystem == null) return;
            var strip = _hud.EnsureRadioInterceptHud();
            if (strip == null) return;

            // Push band list (ALL + each registered frequency).
            var coreBands = RadioTunerSystem.BuildTunerBands();
            var uiBands = new System.Collections.Generic.List<RadioInterceptHUD.TunerBand>(coreBands.Count);
            for (int i = 0; i < coreBands.Count; i++)
            {
                var b = coreBands[i];
                uiBands.Add(RadioInterceptHUD.TunerBand.FromParts(
                    b.FrequencyId, b.Label, b.ChannelTag));
            }
            strip.SetTunerBands(uiBands);

            // Avoid stacking handlers if WireHUD / load re-runs.
            strip.OnTunerBandChanged -= HandleRadioHudTunerChanged;
            strip.OnTunerBandChanged += HandleRadioHudTunerChanged;
            RadioTunerSystem.OnFrequencyChanged -= HandleRadioTunerFrequencyChanged;
            RadioTunerSystem.OnFrequencyChanged += HandleRadioTunerFrequencyChanged;

            // Align dial with current tuner state (detuned on fresh boot).
            strip.SyncFromFrequencyId(RadioTunerSystem.State?.CurrentFrequencyId);
            PushRadioLiveStateToHud();
        }

        private void HandleRadioHudTunerChanged(string frequencyId, string channelTag)
        {
            if (RadioTunerSystem == null) return;
            if (string.IsNullOrEmpty(frequencyId))
                RadioTunerSystem.Detune();
            else
                RadioTunerSystem.TuneToFrequency(frequencyId);
        }

        private void HandleRadioTunerFrequencyChanged(string frequencyId)
        {
            if (_hud == null) return;
            var strip = _hud.EnsureRadioInterceptHud();
            // Sync HUD without re-notifying (would loop into TuneToFrequency).
            strip?.SyncFromFrequencyId(frequencyId);
            PushRadioLiveStateToHud();
        }

        private static DiaryFragmentSO CreateDefaultDiary(in DiarySeed seed)
        {
            var diary = ScriptableObject.CreateInstance<DiaryFragmentSO>();
            diary.id = seed.Id;
            diary.title = seed.Title;
            diary.text = seed.Text;
            diary.authorName = seed.Author;
            diary.foundInRoomId = seed.RoomId;
            diary.warnsAboutSystemId = seed.WarnsSystem;
            diary.pageOrder = seed.Page;
            diary.totalPages = seed.Total;
            return diary;
        }
    }
}
