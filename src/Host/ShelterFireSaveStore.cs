// SPDX-License-Identifier: MIT
// ASHFALL shelter fire hazard save store facade.

using System;
using System.Collections.Generic;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persistable wrapper for <see cref="ShelterFireHazardSystem"/> incident ledger.
    /// </summary>
    [Serializable]
    public sealed class ShelterFireSaveState
    {
        public Dictionary<string, FireIncidentState> Incidents { get; set; }
            = new Dictionary<string, FireIncidentState>();
    }

    /// <summary>
    /// Shelter fire persistence facade delegating to the Core SaveStore service.
    /// </summary>
    public static class ShelterFireSaveStore
    {
        public const string FileName = "shelter_fire_save.json";
        public const string SectionName = "shelter_fire";

        private static readonly SaveStore<ShelterFireSaveState> s_store =
            SaveStoreHub.Checksummed<ShelterFireSaveState>(
                FileName,
                nameof(ShelterFireSaveStore),
                allowLegacyBareState: false);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static bool TrySave(ShelterFireSaveState state) => s_store.TrySave(state);
        public static ShelterFireSaveState? TryLoad() => s_store.TryLoad();

        public static string TryCapturePersisted(ShelterFireSaveState state) =>
            s_store.CapturePersisted(state);

        public static ShelterFireSaveState FromSystem(ShelterFireHazardSystem system)
        {
            if (system == null) throw new ArgumentNullException(nameof(system));
            return new ShelterFireSaveState { Incidents = system.CaptureState() };
        }

        public static void ApplyToSystem(ShelterFireHazardSystem system, ShelterFireSaveState? state)
        {
            if (system == null) throw new ArgumentNullException(nameof(system));
            if (state?.Incidents == null) return;
            system.RestoreState(state.Incidents);
        }
    }
}
