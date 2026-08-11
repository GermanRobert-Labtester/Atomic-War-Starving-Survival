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

        private void InitNarrativeDependentSystems()
        {
            InitPhantomIntruders();
            InitChildDependentSystem();
        }

        private void InitPhantomIntruders()
        {
            // Prompt #6 — Phantom Intruders System
            // ───────────────────────────────────────────────────────────
            PhantomIntruders = new PhantomIntruderSystem();
            PhantomIntruders.SetNeedsSystem(NeedsSystem);
            PhantomIntruders.ConsumeAmmoHandler = amount =>
            {
                if (Inventory == null || _itemCatalog == null) return false;
                // Try common ammo types
                var ammoTypes = new[] { "ammo_9mm", "ammo_shotgun", "ammo_rifle" };
                foreach (var ammoId in ammoTypes)
                {
                    var def = _itemCatalog.GetById(ammoId);
                    if (def != null && Inventory.Remove(def, amount)) return true;
                }
                return false;
            };
            PhantomIntruders.OnWeaponFiredHandler = () =>
            {
                GameLog.Log("[Phantom Intruder] Weapon fired at the hatch door!");
            };
            Action<Survivor> onPhantomIntruderTriggered = paranoid =>
            {
                GameLog.Log($"[Phantom Intruder] {paranoid.DisplayName} sees a Hatch Breach that isn't there!");
            };
            PhantomIntruders.OnPhantomIntruderTriggered += onPhantomIntruderTriggered;
            _subscriptions.Track(() => PhantomIntruders.OnPhantomIntruderTriggered -= onPhantomIntruderTriggered);

            Action<Survivor> onPhantomIntruderResolved = paranoid =>
            {
                GameLog.Log($"[Phantom Intruder] {paranoid.DisplayName} realizes nothing was out there.");
            };
            PhantomIntruders.OnPhantomIntruderResolved += onPhantomIntruderResolved;
            _subscriptions.Track(() => PhantomIntruders.OnPhantomIntruderResolved -= onPhantomIntruderResolved);

            // ───────────────────────────────────────────────────────────
            // Prompt #9 — The Child Dependent System
            // ───────────────────────────────────────────────────────────
            
        }

        private void InitChildDependentSystem()
        {ChildSystem = new ChildDependentSystem();
            ChildSystem.SetNeedsSystem(NeedsSystem);
            ChildSystem.ConsumeChildRationsHandler = (food, water) =>
            {
                if (Inventory == null || _itemCatalog == null) return;
                var foodItem = _itemCatalog.GetById("canned_food");
                if (foodItem != null) Inventory.Remove(foodItem, Mathf.CeilToInt(food / 20f));
                var waterItem = _itemCatalog.GetById("clean_water");
                if (waterItem != null) Inventory.Remove(waterItem, Mathf.CeilToInt(water / 20f));
            };
            Action<Survivor> onChildFound = child =>
            {
                if (Survivors != null)
                {
                    Survivors.Add(child);
                    NeedsSystem.Register(child);
                }
                GameLog.Log("[Child] The child has been found and brought into the bunker. Hope rises.");
            };
            ChildSystem.OnChildFound += onChildFound;
            _subscriptions.Track(() => ChildSystem.OnChildFound -= onChildFound);

            Action<Survivor> onChildDied = _ =>
            {
                GameLog.Log("[Child] The child has died. The bunker's hope shatters.");
                if (SaveSystem != null)
                    SaveSystem.SetWorldFlag(ChildDependentSystem.ChildDiedFlag, true);
            };
            ChildSystem.OnChildDied += onChildDied;
            _subscriptions.Track(() => ChildSystem.OnChildDied -= onChildDied);

            // ───────────────────────────────────────────────────────────
        
        }



    }
}
