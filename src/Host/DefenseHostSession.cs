// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : DefenseHostSession (Plan 163)
// Core System  : DefenseSystem (trap layer + pre-combat raid resolution)
// Purpose      : LastEvent feedback, dirty-flag save flush, inventory-gated
//      trap commands. Capture intake hands off to PrisonerSystem via an
//      injected callback (the captive authority stays single).
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Defense;

namespace AtomicWar.GodotApp
{
    public sealed class DefenseHostSession : HostSessionBase
    {
        public DefenseSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        /// <summary>Raised when a raid produces captives; Main calls
        /// PrisonerSystem.TakePrisoner (single captive authority).</summary>
        public event Action<int, int>? OnCaptivesToHandOff;

        /// <summary>Plan 203: attached perimeter authority (optional — sector grid,
        /// alarms, weather wear). Attached by Main once both sessions exist.</summary>
        public PerimeterDefenseSystem? Perimeter { get; private set; }

        /// <summary>
        /// CORE-MECH W5 — whether an emplacement is actually powered. Wired by
        /// Main from the grid/armory circuit so the defense panel's strength
        /// projection is the same truth the raid resolver uses (it previously
        /// projected with no perimeter at all, under-reporting what the player
        /// built). Unbound = every emplacement treated as unpowered/neutral,
        /// which is the fail-closed reading.
        /// </summary>
        public Func<string, bool>? EmplacementPoweredProvider { get; set; }

        /// <summary>
        /// CORE-MECH W5 — the most recent pre-combat engagement, so the panel can
        /// report what the defenses actually did (repelled / breached, raiders
        /// neutralized, captured) instead of only a static score.
        /// </summary>
        public DefenseEngagementResult? LastEngagement { get; private set; }

        /// <summary>Record an engagement resolved elsewhere (Main owns the call).</summary>
        public void RecordEngagement(DefenseEngagementResult engagement)
        {
            LastEngagement = engagement;
            RaiseStateChanged();
        }

        public void AttachPerimeter(PerimeterDefenseSystem? perimeter)
        {
            if (Perimeter != null) Perimeter.OnEventRaised -= _ => RaiseStateChanged();
            Perimeter = perimeter;
            if (Perimeter != null) Perimeter.OnEventRaised += _ => RaiseStateChanged();
            RaiseStateChanged();
        }

        public ActionResult ResetSectorAlarm(string sectorId)
        {
            if (Perimeter == null) return ActionResult.Failed("no_perimeter", "defense.no_perimeter");
            var res = Perimeter.ResetSectorAlarm(sectorId);
            if (res.IsSuccess) LastEvent = $"{sectorId} sector alarm device reset.";
            else LastEvent = "Alarm reset blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public ActionResult ToggleSectorArm(string sectorId)
        {
            if (Perimeter == null) return ActionResult.Failed("no_perimeter", "defense.no_perimeter");
            var res = Perimeter.DisarmSector(sectorId);
            if (res.IsSuccess) LastEvent = $"{sectorId} sector alarm {(Perimeter.FindSector(sectorId)?.alarm_armed == true ? "armed" : "disarmed")}.";
            RaiseStateChanged();
            return res;
        }

        public DefenseHostSession(DefenseSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnTrapSprung += (installationId, trapId) =>
            {
                LastEvent = $"{trapId} sprung ({installationId}).";
                RaiseStateChanged();
            };
            System.OnTrapBroken += inst =>
            {
                LastEvent = $"{inst.installation_id} broke — repair before reset.";
                RaiseStateChanged();
            };
            System.OnRaiderCaptured += (day, count) => OnCaptivesToHandOff?.Invoke(day, count);
            System.OnRaidResolved += result =>
            {
                LastEvent = result.Repelled
                    ? $"Raid repelled by static defenses ({result.RaidersNeutralizedByTraps} trapped, {result.RaidersCaptured} captured)."
                    : $"Defenses breached — {result.RemainingRaiders} raiders through. Traps took {result.RaidersNeutralizedByTraps}, captured {result.RaidersCaptured}.";
                RaiseStateChanged();
            };
        }

        public void MarkDirty(string reason)
        {
            LastEvent = reason;
            RaiseStateChanged();
        }

        public override void Save()
        {
            if (!IsDirty) return;
            DefenseSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
