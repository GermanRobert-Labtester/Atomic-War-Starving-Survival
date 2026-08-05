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



        private void RefreshInventoryStrip()
        {
            if (_hud == null) return;
            var strip = _hud.InventoryStripUI;
            if (strip != null)
                strip.Sync(Inventory);
            // Corpse / rusted-can counts also drive the Internal Horror strip.
            RefreshInternalHorrorHud();
        }



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



        public void CloseInternalHorrorPanels()
        {
            if (_hud == null) return;
            var horror = _hud.EnsureInternalHorrorHud();
            if (horror == null) return;
            if (horror.IsCorpsePanelOpen) horror.CloseCorpsePanel();
            if (horror.IsFirePanelOpen) horror.CloseFirePanel();
        }

    }
}
