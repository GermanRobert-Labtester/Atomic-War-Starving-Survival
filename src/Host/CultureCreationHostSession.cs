// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Culture;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class CultureCreationSaveStore
    {
        public const string SectionName = "culture_creation";
        public const string FileName = "culture_creation_save.json";

        private static readonly SaveStore<CultureCreationState> s_store =
            SaveStoreHub.Checksummed<CultureCreationState>(FileName, nameof(CultureCreationSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string? TryCapturePersisted(CultureCreationState state) => s_store.CaptureBare(state);
        public static CultureCreationState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(CultureCreationState state) => s_store.TrySave(state);
        public static CultureCreationState? TryLoad() => s_store.TryLoad();
    }

    public sealed class CultureCreationHostSession : HostSessionBase
    {
        public CultureCreationSystem System { get; }

        public CultureCreationHostSession(CultureCreationSystem? system = null)
        {
            System = system ?? new CultureCreationSystem();
        }

        public static CultureCreationHostSession Create(string dataDir, CultureCreationState? restoredState = null)
        {
            var system = new CultureCreationSystem(restoredState);
            string catalogPath = Path.Combine(dataDir, "art_forms.json");
            if (File.Exists(catalogPath))
            {
                system.LoadCatalog(File.ReadAllText(catalogPath));
            }
            return new CultureCreationHostSession(system);
        }

        public ArtworkRecord CreateArtwork(
            string creatorSurvivorId,
            string title,
            ArtMedium medium,
            ArtTheme theme,
            int day,
            float artistSkill = 50f,
            ISeededRng? rng = null)
        {
            var record = System.CreateArtwork(creatorSurvivorId, title, medium, theme, day, artistSkill, rng);
            RaiseStateChanged();
            return record;
        }

        public bool DisplayArtwork(string artworkId, string locationId)
        {
            bool ok = System.DisplayArtwork(artworkId, locationId);
            if (ok) RaiseStateChanged();
            return ok;
        }

        public float GetShelterCultureMoraleBonus() => System.GetShelterCultureMoraleBonus();

        public CultureCreationCensus GetCensus() => System.GetCensus();

        public CultureCreationState CaptureState() => System.CaptureState();

        public void RestoreState(CultureCreationState state)
        {
            System.RestoreState(state);
            RaiseStateChanged();
        }
    }
}
