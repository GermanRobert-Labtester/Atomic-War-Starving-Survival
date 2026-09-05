// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : PsychologyArcHostSession (Plan 164)
// Core System  : PsychologicalArcSystem
// Purpose      : LastEvent feedback + dirty-flag save flush. The system's own
//      events reach journal/needs/fire authorities through Main wiring; this
//      session is presentation feedback only.
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public sealed class PsychologyArcHostSession : HostSessionBase
    {
        public PsychologicalArcSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public PsychologyArcHostSession(PsychologicalArcSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnBreakdownArcStarted += (id, arcId) =>
            {
                LastEvent = $"{id}: {arcId.Replace("arc_", "").Replace('_', ' ')} taking hold.";
                RaiseStateChanged();
            };
            System.OnBreakdownEscalated += (id, _, from, to) =>
            {
                LastEvent = $"{id}'s crisis deepened: {from} → {to}.";
                RaiseStateChanged();
            };
            System.OnBreakdownBehaviorOccurred += (id, behavior) =>
            {
                LastEvent = $"{id}: {behavior}.";
                RaiseStateChanged();
            };
            System.OnStashTransferred += (id, item, count) =>
            {
                LastEvent = $"{count}× {item} missing from stores."; // hidden cause stays hidden
                RaiseStateChanged();
            };
            System.OnBreakdownResolved += (id, arcId) =>
            {
                LastEvent = $"{id} worked through the {arcId.Replace("arc_", "").Replace('_', ' ')} arc.";
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
            PsychologyArcSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
