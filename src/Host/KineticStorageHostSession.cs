using System;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot adapter for the Core kinetic storage authority (Plan 80).
    /// Presentation (KineticStoragePanel) is a Wave 6 google-stitch deliverable —
    /// see the "Missing UI panels" registry in AGENTS.md. The UI must route
    /// the emergency brake through Core commands, never bypass them.
    /// </summary>
    public sealed class KineticStorageHostSession : HostSessionBase
    {
        public KineticStorageSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public KineticStorageHostSession(KineticStorageSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnFlywheelInstalled += f =>
            {
                LastEvent = $"Flywheel installed in {f.roomId}.";
                RaiseStateChanged();
            };
            System.OnFlywheelOverspeed += f =>
            {
                LastEvent = $"OVERSPEED: {f.instanceId} at {f.rotorRpm:F0} RPM.";
                RaiseStateChanged();
            };
            System.OnFlywheelFailure += f =>
            {
                LastEvent = $"CONTAINMENT FAILURE: {f.instanceId} ({f.failureReason}).";
                RaiseStateChanged();
            };
            System.OnStorageChanged += () => { RaiseStateChanged(); };
        }

        public ActionResult Install(string flywheelClassId, string roomId, int day, Func<string, int, bool> consumeItems)
        {
            var res = System.InstallFlywheel(flywheelClassId, roomId, day, consumeItems);
            if (res.IsFailure) LastEvent = "Flywheel install blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public ActionResult BringOnline(string instanceId)
        {
            var res = System.BringOnline(instanceId);
            if (res.IsFailure) LastEvent = "Flywheel start blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public ActionResult PerformMaintenance(string instanceId, int day, Func<string, int, bool> consumeItems)
        {
            var res = System.PerformMaintenance(instanceId, day, consumeItems);
            if (res.IsFailure) LastEvent = "Flywheel maintenance blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public override void Save()
        {
            if (!IsDirty) return;
            KineticStorageSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
