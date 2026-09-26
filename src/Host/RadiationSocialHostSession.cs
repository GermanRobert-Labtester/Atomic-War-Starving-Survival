// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : RadiationSocialSaveStore
// Core State : Ashfall.Core.Radiation.RadiationSocialSaveState
// Host Caller: Main.RadiationSocial
// Purpose    : Radiation social bridge host session & persistence. The Core
//              bridge owns dose brackets, social penalties, discrimination
//              incidents, and faction standing adjustments. Policy remains the
//              faction owner's concern; survivor dose remains the dose ledger's.
// ============================================================================

using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Radiation;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class RadiationSocialSaveStore
    {
        public const string FileName = "radiation_social_save.json";
        public const string SectionName = "radiation_social";

        private static readonly SaveStore<RadiationSocialSaveState> s_store =
            SaveStoreHub.Checksummed<RadiationSocialSaveState>(FileName, nameof(RadiationSocialSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(RadiationSocialSaveState state) => s_store.CaptureBare(state);
        public static RadiationSocialSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(RadiationSocialSaveState state) => s_store.TrySave(state);
        public static RadiationSocialSaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>Host session composing the Core <see cref="RadiationSocialBridge"/>.</summary>
    public sealed class RadiationSocialHostSession : HostSessionBase
    {
        private readonly RadiationSocialBridge _bridge;

        public RadiationSocialBridge Bridge => _bridge;
        public bool CatalogReady { get; private set; }
        public string LastEvent { get; private set; } = string.Empty;

        public RadiationSocialHostSession(RadiationSocialSaveState? state = null)
        {
            _bridge = new RadiationSocialBridge();
            if (state != null) _bridge.RestoreState(state);
        }

        public static RadiationSocialHostSession Create(RadiationSocialSaveState? state = null) =>
            new RadiationSocialHostSession(state);

        public bool LoadCatalog(string dataDirectory)
        {
            if (string.IsNullOrWhiteSpace(dataDirectory)) return false;
            string path = Path.Combine(dataDirectory, "radiation_economy_social.json");
            if (!File.Exists(path)) return false;
            try
            {
                _bridge.LoadCatalog(File.ReadAllText(path));
                CatalogReady = _bridge.Brackets.Count > 0;
                return CatalogReady;
            }
            catch (Exception)
            {
                CatalogReady = false;
                return false;
            }
        }

        public SurvivorRadiationSocialProfile EvaluateSocialStance(
            string survivorId, float doseMsv, ISeededRng? rng = null, string? encounteringFactionId = null)
        {
            var profile = _bridge.EvaluateSocialStance(survivorId, doseMsv, rng, encounteringFactionId);
            LastEvent = $"Evaluated {survivorId} at {doseMsv:0.0} mSv -> {profile.bracketId}.";
            RaiseStateChanged();
            return profile;
        }

        public RadiationSocialSaveState CaptureState() => _bridge.CaptureState();

        public void RestoreState(RadiationSocialSaveState state)
        {
            _bridge.RestoreState(state);
            LastEvent = "Restored radiation social state.";
            RaiseStateChanged();
        }

        public bool TrySave() => RadiationSocialSaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = RadiationSocialSaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
