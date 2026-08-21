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
        private void FlushExpeditionIfDirty()
        {
            if (_expeditionDirty) SaveExpeditions();
        }

        private void SetupExpeditions()
        {
            if (_expeditions != null) return;
            _expeditions = ExpeditionHostSession.Create(_dataDir);
            _expeditions.StateChanged += () => _expeditionDirty = true;
            _expeditions.OnEncounterSurfaced += OnExpeditionEncounterSurfaced;
            GD.Print("[Ashfall Godot] Expedition host ready: encounters · dive instance.");
        }

        private void SaveExpeditions()
        {
            if (_expeditions == null) return;
            if (ExpeditionSaveStore.TrySave(_expeditions.CaptureSave()))
            {
                _expeditionDirty = false;
                GD.Print("[Ashfall Godot] Expedition save written.");
            }
        }

        private void SetupCombat()
        {
            if (_combat != null) return;
            SetupInventory();
            SetupSurvivors();
            _combat = CombatHostSession.Create(_dataDir);
            if (_combat != null)
            {
                _combat.Inventory = _inventory;
                _combat.Survivors = _survivors;
                _combat.WireRealState();
                _combat.StateChanged += () => _combatDirty = true;
                // Expedition encounters auto-populate a real combat encounter.
                SetupExpeditionCombatHandoff(_combat);
            }
            GD.Print("[Ashfall Godot] Combat host ready: tactical combat expansion.");
        }

        private void SaveCombat()
        {
            if (_combat == null) return;
            if (CombatSaveStore.TrySave(_combat.CaptureSave()))
            {
                _combatDirty = false;
                GD.Print("[Ashfall Godot] Combat save written.");
            }
        }

        private void FlushCombatIfDirty()
        {
            if (_combatDirty) SaveCombat();
        }

        /// <summary>
        /// Wire expedition travel encounters to spawn real combat: when an
        /// expedition triggers an encounter, populate a tactical combat at that
        /// location (if none is already active). This is the raiding/ambush
        /// hand-off from the travel loop into the Combat expansion.
        /// </summary>
        private void SetupExpeditionCombatHandoff(CombatHostSession combat)
        {
            if (combat == null) return;
            SetupExpeditions();
            if (_expeditions == null) return;
            _expeditions.Engine.OnEncounterTriggered += state =>
            {
                if (_combat == null) return;
                var cs = _combat.Engine.State;
                bool idle = string.IsNullOrEmpty(cs.EncounterId) || cs.Resolved;
                if (!idle) return;
                _combat.StartDemoCombat(state.locationId, state.displayName);
                _combatDirty = true;
                GD.Print($"[Ashfall Godot] Expedition encounter at {state.locationId} spawned combat.");
            };
        }

        private void OnExpeditionStartClicked(string locationId)
        {
            SetupExpeditions();
            _statusLabel.Text = _expeditions.StartDemoExpedition("survivor_gunner_mikhail", locationId)
                + "\n" + _expeditions.StatusLine();
        }

        private void OnExpeditionTickClicked()
        {
            SetupExpeditions();
            _statusLabel.Text = _expeditions.TickDemoHours(2f) + "\n" + _expeditions.StatusLine();
        }

        private void OnExpeditionDiveClicked()
        {
            SetupExpeditions();
            _statusLabel.Text = _expeditions.StartDiveDemo();
        }

        private void OnExpeditionAdvanceDiveClicked()
        {
            SetupExpeditions();
            _statusLabel.Text = _expeditions.AdvanceDiveDemo() + "\n" + _expeditions.DiveStatusLine();
        }

        private void SaveWastelandMap()
        {
            if (_wastelandMap == null) return;
            try
            {
                WastelandMapSaveStore.TrySave(_wastelandMap.CaptureState());
            }
            catch (Exception e)
            {
                GD.PushWarning("[Ashfall Godot] WastelandMap save failed: " + e.Message);
            }
        }

        private void SaveEncounterChoice()
        {
            if (_encounterChoice == null || !_encounterChoiceDirty) return;
            try
            {
                if (EncounterChoiceSaveStore.TrySave(_encounterChoice.CaptureState()))
                    _encounterChoiceDirty = false;
            }
            catch (Exception e)
            {
                GD.PushWarning("[Ashfall Godot] EncounterChoice save failed: " + e.Message);
            }
        }

        private void SetupWastelandMap()
        {
            if (_wastelandMap != null) return;
            var nodes = new List<Ashfall.Core.World.MapNode>
            {
                new Ashfall.Core.World.MapNode { Id = "loc_holdfast", DisplayName = "Holdfast",
                    PositionX = 500, PositionY = 300, StartingUnlocked = true },
                new Ashfall.Core.World.MapNode { Id = "loc_cut_abandoned_depot", DisplayName = "Abandoned Depot",
                    PositionX = 700, PositionY = 200, Discoverable = true },
                new Ashfall.Core.World.MapNode { Id = "loc_cut_merchant_caravanserai", DisplayName = "Merchant Caravanserai",
                    PositionX = 400, PositionY = 200, Discoverable = true, StartingUnlocked = true },
                new Ashfall.Core.World.MapNode { Id = "loc_black_flotilla_outpost", DisplayName = "Black Flotilla Outpost",
                    PositionX = 600, PositionY = 500, Discoverable = true }
            };
            var routes = new List<Ashfall.Core.World.MapRoute>
            {
                new Ashfall.Core.World.MapRoute { From = "loc_holdfast", To = "loc_cut_abandoned_depot", DistanceKm = 12 },
                new Ashfall.Core.World.MapRoute { From = "loc_holdfast", To = "loc_cut_merchant_caravanserai", DistanceKm = 8 },
                new Ashfall.Core.World.MapRoute { From = "loc_cut_merchant_caravanserai", To = "loc_black_flotilla_outpost", DistanceKm = 22 }
            };
            _wastelandMap = new Ashfall.Core.World.WastelandMapSystem(
                new Ashfall.Core.World.WastelandMapState(), nodes, routes);
        }

        private void SetupEncounterChoiceResolver()
        {
            if (_encounterChoice != null) return;
            ApplyEncounterChoiceSaveIfAny();
        }

        private void ApplyEncounterChoiceSaveIfAny()
        {
            try
            {
                var saved = EncounterChoiceSaveStore.TryLoad();
                if (saved == null) return;
                _encounterChoice = new Ashfall.Core.Expeditions.EncounterChoiceResolver(saved);
            }
            catch (Exception e)
            {
                GD.PushWarning("[Ashfall Godot] EncounterChoice load failed: " + e.Message);
            }
        }

        private void SetupEncounterChoice()
        {
            if (_encounterChoice != null) return;
            _encounterChoice = new Ashfall.Core.Expeditions.EncounterChoiceResolver(
                new Ashfall.Core.Expeditions.EncounterChoiceState());
            _encounterChoice.OnResolved += _ => _encounterChoiceDirty = true;
        }

        private void CloseExpeditionPanel()
        {
            _expeditionPanel.Visible = false;
        }

        private void OnExpeditionEncounterSurfaced(ExpeditionEncounterBridge.EncounterSurfaced surfaced)
        {
            if (_expeditionPanel != null && _expeditionPanel.Visible)
                _expeditionPanel.ShowEncounterNotice(surfaced);
            // else: panel closed/headless — encounter surfaced without a diegetic surface.
        }

        private void CloseCombatPanel()
        {
            _combatPanel.Visible = false;
        }

        private void CloseMapPanel()
        {
            _mapPanel.Visible = false;
        }

        private void CloseCombatDetailPanel()
        {
            _combatDetailPanel.Visible = false;
        }

        private void CloseCombatHistoryPanel()
        {
            _combatHistoryPanel.Visible = false;
        }

        private void CloseMapDetailPanel()
        {
            _mapDetailPanel.Visible = false;
        }

        // Debounced flush hooks for systems that mutate each frame. They run
        // every tick and bail out unless the matching dirty flag is set.
        internal void FlushEncounterChoiceIfDirty()
        {
            if (!_encounterChoiceDirty) return;
            SaveEncounterChoice();
        }

    }
}
