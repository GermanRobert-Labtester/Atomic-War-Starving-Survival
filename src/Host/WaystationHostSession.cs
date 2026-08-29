using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.PlayerCommand;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for WaystationSystem.
    /// Manages the forward camp at Waystation A, watch assignments, stove heating, and filter maintenance.
    /// </summary>
    public sealed class WaystationHostSession
    : HostSessionBase{
        public WaystationSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public WaystationHostSession(WaystationSystem system)
        {
            System = system ?? new WaystationSystem();

            System.OnStateChanged += state =>
            {
                RaiseStateChanged();
            };

            System.OnUnlocked += () =>
            {
                LastEvent = "[Waystation] Forward camp Waystation A unlocked!";
                RaiseStateChanged();
            };

            System.OnStoveDied += () =>
            {
                LastEvent = "[Waystation] WARNING: Waystation stove has died!";
                RaiseStateChanged();
            };
        }

        public void Unlock()
        {
            System.Unlock();
            LastEvent = "Unlocked Waystation A outpost";
            RaiseStateChanged();
        }

        public CommandResult AssignWatch(IList<string> survivorIds)
        {
            var result = System.ExecuteAssignWatch(survivorIds, expectedStateVersion: StateVersion, currentStateVersion: StateVersion);
            if (result.IsSuccess)
            {
                LastEvent = "Assigned watch sentries to Waystation A";
                RaiseStateChanged();
            }
            else
            {
                LastEvent = $"Watch assignment refused: {result.FailureCode}.";
            }
            return result;
        }

        public void Resupply()
        {
            System.Resupply();
            LastEvent = "Resupplied Waystation A forward camp.";
            RaiseStateChanged();
        }

        public void TickDaily(bool iceRoadOpen)
        {
            System.TickDaily(iceRoadOpen);
            RaiseStateChanged();
        }

        public override void Save()
        {
            if (!IsDirty) return;
            WaystationSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
