// SPDX-License-Identifier: MIT
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Inventory;
using Ashfall.Core.Expeditions;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        public void OpenSettingsPanel() => _settingsPanel?.Open();
        public void OpenCraftingPanel()
        {
            SetupCrafting();
            SetupInventory();
            SetupSurvivors();
            SetupPhase0();
            SyncCraftingStationsFromShelter();
            _craftingPanel.Bind(_crafting, _inventory, _survivors,
                _phase0?.TradeSpecialty, sid => ResolveSurvivorProfessionId(sid));
            _craftingPanel.Open();
        }
        public void OpenRadioPanel()
        {
            SetupRadio();
            if (_radioPanel != null && _radio != null)
            {
                _radioPanel.Bind(_radio);
                _radioPanel.BindProduction(EnsureRadioProgramProductionSession());
            }
            _radioPanel?.Open();
        }
        public void OpenMedicalPanel()
        {
            SetupJournal();
            DiscoverBureaucraticDocuments("medical_office");
            _medicalPanel?.Open();
        }

        public void OpenPhase0Panel()
        {
            SetupSurvivors();
            SetupPhase0();
            // Task #133 P1c: bind the medical pipeline so the panel's inhaler
            // action consumes inventory through the pipeline like MedicalPanel.
            EnsureMedicalPipeline();
            _phase0Panel.Bind(_phase0, _survivors, _medical.Pipeline);
            _phase0Panel.Open();
        }
        public void OpenDutyRosterPanel()
        {
            SetupJournal();
            DiscoverBureaucraticDocuments("duty_roster");
            _dutyRosterPanel?.Open();
        }
        public void OpenExpeditionPanel() => _expeditionPanel?.Open();
        public void OpenWeatherPanel()
        {
            _weatherPanel?.Open();
            ObserveSigil("weather.read");
        }
        public void OpenWeatherForecastPanel()
        {
            _weatherForecastPanel?.Bind(_world.Weather);
            _weatherForecastPanel?.Open();
            ObserveSigil("weather.read");
        }
        public void OpenWeatherHistoryPanel()
        {
            _weatherHistoryPanel?.Bind(_world.Weather);
            _weatherHistoryPanel?.Open();
            ObserveSigil("weather.read");
        }
        public void OpenQuestsPanel()
        {
            SetupHoldfastRuntime();
            SetupExpansions();
            SetupDutyRoster();
            SetupFactionBranch();
            SetupMoralChoice();
            BindQuestsPanel();
            _questsPanel.Open();
        }
        public void OpenJournalPanel()
        {
            _journalPanel?.Bind(_journal);
            _journalPanel?.Open();
        }
        public void OpenFactionsPanel()
        {
            SetupHoldfastRuntime();
            SetupMuster();
            SetupExpansions();
            SetupYearOfAsh();
            SetupFactionBranch();
            SetupMoralChoice();
            _factionsPanel.Bind(_core.Catalog.Factions, _holdfastRuntime?.Trade, _muster, _expansions, _yearOfAsh, _factionBranch?.Coordinator, _moralChoice, _informantNetwork);
            _factionsPanel.OnWarlordTributePay -= PayWarlordTribute;
            _factionsPanel.OnWarlordTributePay += PayWarlordTribute;
            _factionsPanel.OnWarlordTributeRefuse -= RefuseWarlordTribute;
            _factionsPanel.OnWarlordTributeRefuse += RefuseWarlordTribute;
            _factionsPanel.OnWarlordTributeContest -= RefuseWarlordTribute;
            _factionsPanel.OnWarlordTributeContest += RefuseWarlordTribute;
            _factionsPanel.OnWarlordTributeSubmit -= SubmitToWarlordTribute;
            _factionsPanel.OnWarlordTributeSubmit += SubmitToWarlordTribute;
            // The panel asks this projection whether the current ask already has a
            // response; it is the only source for the disabled-button state.
            _factionsPanel.WarlordResponseStateProvider = () =>
            {
                try { SetupWarlordResponse(); } catch { /* unbound owner */ }
                return WarlordResponseStatusLine();
            };
            _factionsPanel.Open();
        }

        private void SubmitToWarlordTribute()
        {
            string line = SubmitToWarlordGuarded();
            GD.Print($"[warlord] {line}");
            _statusLabel.Text = line;
        }


        public void OpenShelterPanel()
        {
            SetupJournal();
            DiscoverBureaucraticDocuments("shelter_records");
            _shelterPanel?.Open();
        }
        public void OpenCombatPanel()
        {
            SetupCombat();
            _combatPanel.Bind(_combat);
            _combatPanel.Open();
        }
        public void OpenMapPanel()
        {
            SetupHoldfastRuntime();
            SetupExpeditions();
            SetupExpansions();
            SetupWorld();
            SetupJournal();
            SetupDeepCoast();
            SetupYearOfAsh();
            _mapPanel.Bind(_core, _expeditions, _expansions, _world, _journalCodex?.Catalogs, _deepCoast, _yearOfAsh);
            _mapPanel.Open();
        }
        public void OpenMapDetailPanel(string locationId)
        {
            SetupHoldfastRuntime();
            SetupExpeditions();
            SetupJournal();
            DiscoverFringeCultRecords(locationId);
            DiscoverPaperPrintingRecords(locationId);
            DiscoverBoneHornRecords(locationId);
            DiscoverPersonalLetterRecords(locationId);
            DiscoverAbyssalAnomalyRecords(locationId);
            // Plan 152 — inspecting a registered producer site recovers archive
            // records (arrival path also discovers via ExpeditionSystem).
            NotifyBlackProjectsProducerInspected(locationId);
            var holdfastLoc = _core?.Catalog?.GetLocation(locationId);
            AtomicWar.Journal.LocationDefinitionData? journalLoc = null;
            if (_journalCodex?.Catalogs?.Locations != null)
            {
                foreach (var l in _journalCodex.Catalogs.Locations)
                {
                    if (l != null && l.id == locationId)
                    {
                        journalLoc = l;
                        break;
                    }
                }
            }
            int currentDay = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
            // Plan 32 fog gate, same predicate as MapPanel's location list: a
            // node on the graph that is neither discovered nor surveyed has no
            // verified dosimetry, so the detail panel must not show hazard
            // numbers for it.
            bool uncharted = false;
            var canonicalMap = _world?.WastelandMap;
            if (canonicalMap != null)
            {
                uncharted = canonicalMap.GetNode(locationId) != null
                    && !canonicalMap.IsDiscovered(locationId)
                    && canonicalMap.GetFogState(locationId) == Ashfall.Core.World.MapFogState.Unknown;
            }
            // Standing-record sub-layouts (R4): only rooms the player has
            // actually unlocked surface here — authored-but-sealed rooms
            // never leak through the detail view.
            List<string>? subLayouts = null;
            var layouts = _expansions?.Layouts;
            if (layouts != null)
            {
                var layoutDef = layouts.GetLayoutDefinition(locationId);
                Ashfall.Core.LocationLayoutParentSave? parentState = null;
                foreach (var candidate in layouts.State.parents)
                {
                    if (candidate != null && candidate.parentLocationId == locationId)
                    {
                        parentState = candidate;
                        break;
                    }
                }
                if (layoutDef != null && parentState?.unlockedRoomIds != null && parentState.unlockedRoomIds.Count > 0)
                {
                    subLayouts = new List<string>();
                    foreach (var roomId in parentState.unlockedRoomIds)
                    {
                        var room = layoutDef.GetRoom(roomId);
                        if (room != null && !string.IsNullOrEmpty(room.displayName))
                            subLayouts.Add(room.displayName);
                    }
                    if (subLayouts.Count == 0) subLayouts = null;
                }
            }
            // Salvage survey (route-reachability wave, S2): the node's authored
            // loot table resolved through the live Plan 46 scavenging catalog.
            // Fog-gated like the hazard rows — Surveyed shows the table
            // category, Visited adds the yield tiers actually rolled against;
            // Rumored/Unknown sectors keep the truthful empty state.
            List<string>? salvageSurvey = null;
            var salvageTableId = canonicalMap?.GetNode(locationId)?.LootTableId ?? string.Empty;
            if (canonicalMap != null && !string.IsNullOrEmpty(salvageTableId))
            {
                var salvageFog = canonicalMap.GetFogState(locationId);
                if (salvageFog == Ashfall.Core.World.MapFogState.Surveyed
                    || salvageFog == Ashfall.Core.World.MapFogState.Visited)
                {
                    var salvageTables = _expeditions?.Engine?.ScavengingCatalog;
                    if (salvageTables != null
                        && salvageTables.TryGetTable(salvageTableId, out var salvageTable)
                        && !string.IsNullOrEmpty(salvageTable.display_name))
                    {
                        salvageSurvey = new List<string> { salvageTable.display_name };
                        if (salvageFog == Ashfall.Core.World.MapFogState.Visited
                            && salvageTable.entries != null && salvageTable.entries.Count > 0)
                        {
                            var tiers = new SortedSet<string>();
                            foreach (var entry in salvageTable.entries)
                                tiers.Add(entry.rarity_tier);
                            salvageSurvey.Add("Yield tiers: " + string.Join(", ", tiers).ToUpperInvariant());
                        }
                    }
                }
            }
            _mapDetailPanel.Bind(holdfastLoc, journalLoc, GetBunkerGraffitiCatalog(), currentDay, uncharted, subLayouts, salvageSurvey);
            _mapDetailPanel.Open();
        }
        public void OpenFactionDetailPanel(string factionId)
        {
            SetupHoldfastRuntime();
            SetupMuster();
            SetupExpansions();
            var faction = _core?.Catalog?.Factions?.GetById(factionId);
            if (faction != null)
            {
                _factionDetailPanel.Bind(faction, _holdfastRuntime?.Trade, _muster, _expansions);
            }
            _factionDetailPanel.Open();
        }
        public void OpenQuestDetailPanel(string questId)
        {
            SetupHoldfastRuntime();
            SetupExpansions();
            SetupMoralChoice();
            var holdfastDef = _core?.Quests?.GetDef(questId);
            var holdfastProgress = _core?.Quests?.GetProgress(questId);
            if (holdfastDef != null)
            {
                _questDetailPanel.Bind(holdfastDef, holdfastProgress);
            }
            else if (_expansions?.CrossingQuests != null && _expansions.CrossingQuests.GetDef(questId) != null)
            {
                var crossingDef = _expansions.CrossingQuests.GetDef(questId);
                var crossingProgress = _expansions.CrossingQuests.GetProgress(questId);
                _questDetailPanel.Bind(crossingDef, crossingProgress);
            }
            else
            {
                var moralDef = GetMoralChoiceDef(questId);
                if (moralDef != null)
                {
                    _questDetailPanel.Bind(moralDef, _moralChoice, (qId, idx) =>
                    {
                        TryResolveMoralChoice(qId, idx);
                        OpenQuestDetailPanel(qId);
                    });
                }
            }
            _questDetailPanel.Open();
        }

        public void OpenMoralChoiceModal(string? questId = null)
        {
            // Exclusive-open seam: raw keys 1–5 are shared with CombatPanel and
            // the moral-choice quick-select must never be live over an open
            // combat surface (a11y audit 2026-09-29, §9.1).
            CloseAllOverlayPanels();
            SetupMoralChoice();
            Ashfall.Core.MoralChoice.MoralChoiceQuestDefinition? targetDef = null;
            if (!string.IsNullOrEmpty(questId))
            {
                targetDef = GetMoralChoiceDef(questId);
            }
            else
            {
                var available = GetAvailableMoralChoices();
                if (available.Count > 0)
                    targetDef = available[0];
                else
                {
                    var resolved = GetResolvedMoralChoices();
                    if (resolved.Count > 0)
                        targetDef = resolved[0];
                    else if (_moralChoiceDefs.Count > 0)
                        targetDef = _moralChoiceDefs[0];
                }
            }

            if (targetDef != null)
            {
                _moralChoiceModal.Bind(targetDef, _moralChoice, (qId, idx) =>
                {
                    TryResolveMoralChoice(qId, idx);
                });
                _moralChoiceModal.Open();
                return;
            }

            if (_statusLabel != null)
                _statusLabel.Text = "No moral choices are available right now.";
            GD.Print("[Ashfall] OpenMoralChoiceModal: no available/resolved/catalog moral choice to present.");
        }
        public void OpenFireIncidentPanel(string? incidentId = null)
        {
            CloseAllOverlayPanels();
            SetupShelterFireHazard();
            SetupSurvivors();
            _fireIncidentPanel.RosterWorkerProvider = () =>
                _survivors?.RosterState?.Where(s => s.IsAlive && !s.IsDead)
                    .Select(s => s.Id).Take(3).ToList() ?? new List<string>();
            _fireIncidentPanel.Rng = _campaignDay?.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 15);
            _fireIncidentPanel.Bind(_shelterFireSession!, incidentId);
            _fireIncidentPanel.Open();
        }
        public void OpenSaveLoadPanel() => _saveLoadPanel?.Open();
        public void OpenCrossingQuestPanel()
        {
            SetupExpansions();
            _crossingQuestPanel.Bind(_expansions, _expansions.Vouch, _simDay);
            _crossingQuestPanel.Open();
        }
        public void OnExitGameClicked()
        {
            SaveAll();
            RecordPlayMetricsSessionEnded();
            ShutdownDebtConsequenceIntegration();
            GetTree().Quit();
        }

        private string _selectedApproachQuestlineId = "quest_the_rate_card_war";

        // ── ASHFALL: THE VERDICT (Expansion 08) ────────────────────────────────

        // Chain 1 tracking: previous-tick living-count snapshot held in host
        // state. Day boundary resets so we do not attribute today's losses
        // to last week. Threshold is observed but the doctrine check lives
        // in ReckoningSystem.
        private int _previousLivingCount = -1;
        private int _previousLivingDay = -1;

        // ── PHASE 0 / CAMPAIGN DAY COORDINATOR ───────────────────────────

        private const string DailyBriefingSaveKey = "daily_briefing_v1";

#if ASHFALL_SELFTEST
        /// <summary>
        /// UI smoke tests create and queue-free a large widget tree. Give Godot one
        /// process frame to flush queued frees before shutting down, otherwise the
        /// test can pass while reporting false-positive node/RID/resource leaks.
        /// </summary>
        private async void QuitUiTestAfterFrame(int exitCode)
        {
            // Audit #46 — async void must not silently swallow exceptions on the
            // headless quit path; log and force a non-zero exit on failure.
            try
            {
                var tree = GetTree();
                await ToSignal(tree, SceneTree.SignalName.ProcessFrame);

                // The UI smoke tests construct the shell directly under Main rather
                // than loading a disposable child scene. Free those test-owned roots
                // explicitly so Godot does not leave their controls in ObjectDB at
                // process exit (normal gameplay never calls this path).
                AshfallUiHelpers.EmptyChildren(this);

                await ToSignal(tree, SceneTree.SignalName.ProcessFrame);
                await ToSignal(tree, SceneTree.SignalName.ProcessFrame);
                tree.Quit(exitCode);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[QuitUiTestAfterFrame] {ex.GetType().Name}: {ex.Message}");
                try { GetTree()?.Quit(exitCode == 0 ? 1 : exitCode); }
                catch { /* last-resort: process may already be tearing down */ }
            }
        }

#endif
    }
}
