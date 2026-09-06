// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : WildlifeEcosystemHostSession (Plan 165)
// Core System  : WildlifeEcosystemSystem (ecology over WildlifeMigrationSystem)
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public sealed class WildlifeEcosystemHostSession : HostSessionBase
    {
        public WildlifeEcosystemSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public WildlifeEcosystemHostSession(WildlifeEcosystemSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnWildlifeObserved += (species, sector) => RaiseStateChanged();
            System.OnWildlifePopulationShifted += (_, _, _) => RaiseStateChanged();
            System.OnLocalExtinction += (species, sector) =>
            {
                LastEvent = $"{species.Replace("species_", "").Replace('_', ' ')} locally extinct around {sector}.";
                RaiseStateChanged();
            };
            System.OnApexPredatorSpotted += (species, sector) =>
            {
                LastEvent = $"Apex predator active: {species.Replace("species_", "").Replace('_', ' ')} near {sector}.";
                RaiseStateChanged();
            };
            System.OnWildlifeTamed += a =>
            {
                LastEvent = $"Tamed a {a.species_id.Replace("species_", "").Replace('_', ' ')} ({a.animal_id}).";
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
            WildlifeEcosystemSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
