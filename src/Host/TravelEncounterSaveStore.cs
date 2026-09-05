// SPDX-License-Identifier: MIT
// ASHFALL Travel encounter & cooldown save store facade.

using System;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Travel encounters persistence facade delegating to the Core SaveStore service.
    /// Manages active encounter cooldowns and chain stages across the campaign.
    /// </summary>
    public static class TravelEncounterSaveStore
    {
        public const string FileName = "travel_encounters_save.json";
        public const string SectionName = "travel_encounters";

        private static readonly SaveStore<TravelEncounterState> s_store =
            SaveStoreHub.Checksummed<TravelEncounterState>(
                FileName,
                nameof(TravelEncounterSaveStore),
                allowLegacyBareState: true);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static bool TrySave(TravelEncounterState state) => s_store.TrySave(state);
        public static TravelEncounterState? TryLoad() => s_store.TryLoad();

        public static string TryCapturePersisted(TravelEncounterState state) =>
            s_store.CapturePersisted(state);

        public static TravelEncounterState FromSystem(TravelEncounterSystem system)
        {
            if (system == null) throw new ArgumentNullException(nameof(system));
            return system.CaptureState();
        }

        public static void ApplyToSystem(TravelEncounterSystem system, TravelEncounterState? state)
        {
            if (system == null) throw new ArgumentNullException(nameof(system));
            if (state == null) return;
            system.RestoreState(state);
        }
    }
}
