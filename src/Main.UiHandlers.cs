using Godot;
using System;
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
            _craftingPanel.Bind(_crafting, _inventory);
            _craftingPanel.Open();
        }
        public void OpenRadioPanel() => _radioPanel?.Open();
        public void OpenMedicalPanel() => _medicalPanel?.Open();

        public void OpenPhase0Panel()
        {
            SetupSurvivors();
            SetupPhase0();
            _phase0Panel.Bind(_phase0, _survivors);
            _phase0Panel.Open();
        }
        public void OpenDutyRosterPanel() => _dutyRosterPanel?.Open();
        public void OpenExpeditionPanel() => _expeditionPanel?.Open();
        public void OpenWeatherPanel() => _weatherPanel?.Open();
        public void OpenWeatherForecastPanel()
        {
            _weatherForecastPanel?.Bind(_world.Weather);
            _weatherForecastPanel?.Open();
        }
        public void OpenWeatherHistoryPanel()
        {
            _weatherHistoryPanel?.Bind(_world.Weather);
            _weatherHistoryPanel?.Open();
        }
        public void OpenQuestsPanel()
        {
            SetupHoldfastRuntime();
            SetupExpansions();
            SetupDutyRoster();
            _questsPanel.Bind(_core.Quests, _expansions?.CrossingQuests, _dutyRoster, _holdfastRuntime?.Day ?? _simDay);
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
            _factionsPanel.Bind(_core.Catalog.Factions, _holdfastRuntime?.Trade, _muster, _expansions, _yearOfAsh, _factionBranch?.Coordinator, _moralChoice);
            _factionsPanel.OnWarlordTributePay -= PayWarlordTribute;
            _factionsPanel.OnWarlordTributePay += PayWarlordTribute;
            _factionsPanel.OnWarlordTributeRefuse -= RefuseWarlordTribute;
            _factionsPanel.OnWarlordTributeRefuse += RefuseWarlordTribute;
            _factionsPanel.Open();
        }


        public void OpenShelterPanel() => _shelterPanel?.Open();
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
            _mapDetailPanel.Bind(holdfastLoc, journalLoc);
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
            var holdfastDef = _core?.Quests?.GetDef(questId);
            var holdfastProgress = _core?.Quests?.GetProgress(questId);
            if (holdfastDef != null)
            {
                _questDetailPanel.Bind(holdfastDef, holdfastProgress);
            }
            else if (_expansions?.CrossingQuests != null)
            {
                var crossingDef = _expansions.CrossingQuests.GetDef(questId);
                var crossingProgress = _expansions.CrossingQuests.GetProgress(questId);
                if (crossingDef != null)
                    _questDetailPanel.Bind(crossingDef, crossingProgress);
            }
            _questDetailPanel.Open();
        }
        public void OpenSaveLoadPanel() => _saveLoadPanel?.Open();
        public void OpenCrossingQuestPanel()
        {
            SetupExpansions();
            _crossingQuestPanel.Bind(_expansions, _expansions.Vouch, _simDay);
            _crossingQuestPanel.Open();
        }
        public void OnExitGameClicked() { SaveAll(); GetTree().Quit(); }









        private string _selectedApproachQuestlineId = "quest_the_rate_card_war";







        // ── ASHFALL: THE VERDICT (Expansion 08) ────────────────────────────────



        // Chain 1 tracking: previous-tick living-count snapshot held in host
        // state. Day boundary resets so we do not attribute today's losses
        // to last week. Threshold is observed but the doctrine check lives
        // in ReckoningSystem.
        private int _previousLivingCount = -1;
        private int _previousLivingDay = -1;





        // ── District 8 deep-coast route (Exp 01 sibling layer) ─────────




        // ── ASHFALL: THE BLACK FLOTILLA (Expansion 09 — maritime salvage) ──────







        // ── EXPEDITIONS (Encounters port) ─────────────────────────────────────



        // ── COMBAT (Expansion 06) ───────────────────────────────────────────









        // ── NARRATIVE · MEDICAL · WORLD · CRAFTING ────────────────────────────



















        // ── TRAVELING CARAVANS (Exp V spec §3.3) ─────────────────────────────





        // ── STARTING LEVEL & HOLDFAST DIRECTIVES ───────────────────────



        // ── POWER GRID (item 13) ────────────────────────────────────────────





        // ── MEDICAL WARD (item 11) ─────────────────────────────────────




        // ── MEMORIAL (item 15) ──────────────────────────────────────────




        // ── STATE-LOSS TRIAD REPAIR (audit fix) ─────────────────────────────
        // The four SaveXxx methods below close the 12 Setup-without-Save gaps
        // called out in the forensic audit. They each persist a single Core
        // envelope to user:// via a dedicated save store. The matching load
        // step runs at the corresponding SetupXxx entry-point (see the audit
        // reference at the top of this file for the full mapping).





        // ── TRAVEL MAP (item 4) ─────────────────────────────────────────


        // ── ENCOUNTER CHOICE (item 5) ──────────────────────────────────




        // ── PHASE 0 / CAMPAIGN DAY COORDINATOR ───────────────────────────

        private const string DailyBriefingSaveKey = "daily_briefing_v1";








        // ── GREENHOUSE / THE GLASS ORCHARD (Exp 05 / XI) ───────────────




        // ── THE SILENT FOUNDRY (Exp 10) ─────────────────────────────────




































        /// <summary>
        /// UI smoke tests create and queue-free a large widget tree. Give Godot one
        /// process frame to flush queued frees before shutting down, otherwise the
        /// test can pass while reporting false-positive node/RID/resource leaks.
        /// </summary>
        private async void QuitUiTestAfterFrame(int exitCode)
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

        /// <summary>Headless smoke test for the player-facing Godot shell.</summary>
    }
}
