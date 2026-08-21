using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for WaystationSystem.
    /// Manages the forward camp at Waystation A, watch assignments, stove heating, and filter maintenance.
    /// </summary>
    public sealed class WaystationHostSession
    {
        public WaystationSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public event Action? StateChanged;

        public WaystationHostSession(WaystationSystem system)
        {
            System = system ?? new WaystationSystem();

            System.OnStateChanged += state =>
            {
                StateChanged?.Invoke();
            };

            System.OnUnlocked += () =>
            {
                LastEvent = "[Waystation] Forward camp Waystation A unlocked!";
                StateChanged?.Invoke();
            };

            System.OnStoveDied += () =>
            {
                LastEvent = "[Waystation] WARNING: Waystation stove has died!";
                StateChanged?.Invoke();
            };
        }

        public void Unlock()
        {
            System.Unlock();
            LastEvent = "Unlocked Waystation A outpost";
            StateChanged?.Invoke();
        }

        public bool AssignWatch(IList<string> survivorIds)
        {
            bool ok = System.AssignWatch(survivorIds);
            if (ok)
            {
                LastEvent = "Assigned watch sentries to Waystation A";
                StateChanged?.Invoke();
            }
            return ok;
        }

        public void Resupply()
        {
            System.Resupply();
            LastEvent = "Resupplied Waystation A forward camp.";
            StateChanged?.Invoke();
        }

        public void TickDaily(bool iceRoadOpen)
        {
            System.TickDaily(iceRoadOpen);
            StateChanged?.Invoke();
        }
    }
}
