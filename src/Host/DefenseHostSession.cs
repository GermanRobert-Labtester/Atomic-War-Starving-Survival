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
