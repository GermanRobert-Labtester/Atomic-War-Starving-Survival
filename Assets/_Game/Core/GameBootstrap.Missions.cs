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
        /// <summary>
        /// Pick the survivor to treat as the "leader" of the bunker for
        /// affinity bookkeeping. Defaults to the first living survivor
        /// (matches the convention used by MentalBreakSystem); falls back
        /// to the donor if they are the only living survivor.
        /// </summary>
        private Survivor ResolveBunkerLeader()
        {
            if (Survivors == null) return null;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var s = Survivors[i];
                if (s != null && s.IsAlive) return s;
            }
            return null;
        }

        private Survivor FindFirstLivingSurvivor()
        {
            if (Survivors == null) return null;
            for (int i = 0; i < Survivors.Count; i++)
            {
                if (Survivors[i] != null && Survivors[i].IsAlive)
                    return Survivors[i];
            }
            return null;
        }

        /// <summary>
        /// Player API: open corpse dispose panel (bury / fertilizer) from inventory body slot.
        /// </summary>
        public bool OpenCorpseDisposePanel()
        {
            if (_hud == null) return false;
            RefreshInternalHorrorHud();
            var horror = _hud.EnsureInternalHorrorHud();
            if (horror == null || horror.CorpseCount <= 0) return false;
            horror.OpenCorpsePanel();
            return true;
        }

        /// <summary>Player API: choose bury or fertilizer on the open corpse panel.</summary>
        public bool SelectCorpseDispose(AtomicWar._Game.UI.CorpseDisposeChoice choice)
        {
            if (_hud == null) return false;
            var horror = _hud.EnsureInternalHorrorHud();
            return horror != null && horror.SelectCorpseDispose(choice);
        }

        /// <summary>Player API: fight fire in a room (or active fire room).</summary>
        public bool SelectFightFire(string roomId = null)
        {
            if (_hud == null) return false;
            var horror = _hud.EnsureInternalHorrorHud();
            return horror != null && horror.SelectFightFire(roomId);
        }

        /// <summary>Player API: seal bulkhead on a burning room.</summary>
        public bool SelectSealBulkhead(string roomId = null)
        {
            if (_hud == null) return false;
            var horror = _hud.EnsureInternalHorrorHud();
            return horror != null && horror.SelectSealBulkhead(roomId);
        }

        /// <summary>
        /// Simulate inventory strip click at index. Corpse icons open dispose panel.
        /// </summary>
        public bool ActivateInventoryIcon(int index)
        {
            if (_hud == null) return false;
            var strip = _hud.InventoryStripUI;
            return strip != null && strip.ActivateIndex(index);
        }

        /// <summary>Cycle inventory strip focus forward (keyboard path [I]).</summary>
        public bool SelectNextInventoryIcon()
        {
            if (_hud == null) return false;
            var strip = _hud.InventoryStripUI;
            bool ok = strip != null && strip.SelectNext();
            if (ok) _hud.RefreshDiegeticHud();
            return ok;
        }

        /// <summary>Cycle inventory strip focus backward (keyboard path [Shift+I]).</summary>
        public bool SelectPrevInventoryIcon()
        {
            if (_hud == null) return false;
            var strip = _hud.InventoryStripUI;
            bool ok = strip != null && strip.SelectPrev();
            if (ok) _hud.RefreshDiegeticHud();
            return ok;
        }

        /// <summary>Clear strip focus and hide stores tooltip ([Esc] when no other panel).</summary>
        public bool ClearInventorySelection()
        {
            if (_hud == null) return false;
            var strip = _hud.InventoryStripUI;
            bool cleared = strip != null && strip.ClearSelection();
            _hud.DiegeticHud?.ClearStoresFocus();
            _hud.RefreshDiegeticHud();
            return cleared;
        }

        /// <summary>Confirm focused inventory icon (Enter/E). Corpses open dispose.</summary>
        public bool ActivateSelectedInventoryIcon()
        {
            if (_hud == null) return false;
            var strip = _hud.InventoryStripUI;
            return strip != null && strip.ActivateSelected();
        }

        /// <summary>Click first corpse stack in inventory (if any).</summary>
        public bool ActivateFirstCorpseInInventory()
        {
            if (_hud == null) return false;
            RefreshInventoryStrip();
            var strip = _hud.InventoryStripUI;
            return strip != null && strip.ActivateFirstCorpse();
        }

        /// <summary>True when corpse dispose panel is open (input priority).</summary>
        public bool IsCorpseDisposePanelOpen()
        {
            var horror = _hud != null ? _hud.InternalHorrorHUD : null;
            return horror != null && horror.IsCorpsePanelOpen;
        }

        /// <summary>True when fire fight/seal panel is open (input priority).</summary>
        public bool IsFirePanelOpen()
        {
            var horror = _hud != null ? _hud.InternalHorrorHUD : null;
            return horror != null && horror.IsFirePanelOpen;
        }

        /// <summary>
        /// Mental-break binge: consume highest-value food × multiplier from bunker stock.
        /// Hosted in Core so Survivors does not reference Inventory.
        /// </summary>

        /// <summary>
        /// Mental-break comfort cure: pick a Comfort item from the
        /// inventory, consume one, and return true. Returns false if
        /// no Comfort item is available. Hosted in Core so Survivors
        /// does not reference Inventory.
        /// </summary>

        /// <summary>
        /// High-tier radio/antenna operational for inter-faction wiretaps.
        /// Requires powered radio with remaining fuel and EMP damage below destroy.
        /// </summary>

        /// <summary>
        /// Vehicle escape project: 50 mechanical_parts + 10 fuel + repaired engine.
        /// Explicit player action (not auto each frame).
        /// </summary>

        /// <summary>Start expedition to a proc-gen map node (weather-scaled travel).</summary>

        /// <summary>Execute a workbench line by 0-based index (keybinds 1-9).</summary>

        /// <summary>
        /// Open trade with a faction stockpile (UI). Hostile factions still open
        /// so the player can demand parley after a hatch repel.
        /// </summary>

        /// <summary>
        /// Open trade using an ephemeral faction stock (created on first use).
        /// Used by the post-repel parley modal so the player need not hunt UI.
        /// </summary>

        /// <summary>Demand parley / surrender on the open trade screen (keybind P).</summary>

        /// <summary>
        /// Demand parley for a faction. Opens trade when HUD is present so the
        /// strip shows STOOD DOWN; falls back to economy-only when headless.
        /// Used by the post-repel modal.
        /// </summary>

        /// <summary>Send a survivor to survey a location with a working geiger.</summary>

        /// <summary>
        /// AI/UI hook: survey the least-known location (unsurveyed first, then oldest measure).
        /// </summary>

    }
}
