// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ShelterIdentitySaveStore
// Core State : Ashfall.Core.Shelter.ShelterIdentityState
// Host Caller: Main.ShelterIdentity (SetupShelterIdentity / SaveShelterIdentity)
// Purpose    : Plan 166 — shelter name, origin, motto, emblem, infamy, and the
//              shelter's own community-action profile.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class ShelterIdentitySaveStore
    {
        public const string FileName = "shelter_identity_save.json";
        public const string SectionName = "shelter_identity";

        private static readonly SaveStore<ShelterIdentityState> s_store =
            SaveStoreHub.Checksummed<ShelterIdentityState>(FileName, nameof(ShelterIdentitySaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(ShelterIdentityState state) => s_store.CaptureBare(state);
        public static ShelterIdentityState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(ShelterIdentityState state) => s_store.TrySave(state);
        public static ShelterIdentityState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Plan 166 host session. Binds the authored origin catalog through the strict
    /// loader and offers the identity/naming/reputation-projection operations.
    /// The faction-standing authority remains the canonical owner; this session
    /// never becomes a second faction reputation store.
    /// </summary>
    public sealed class ShelterIdentityHostSession : HostSessionBase
    {
        private readonly ShelterIdentitySystem _system;
        private string _lastEvent = string.Empty;

        public ShelterIdentitySystem System => _system;
        public ShelterIdentityCensus Census => _system.GetCensus();
        public string LastEvent => _lastEvent;
        public string ShelterName => _system.ShelterName;
        public string OriginId => _system.OriginId;
        public int Infamy => _system.Infamy;

        public ShelterIdentityHostSession(string? dataDir = null, ShelterIdentitySystem? system = null)
        {
            _system = system ?? new ShelterIdentitySystem();

            _system.OnShelterNamed += name =>
            {
                _lastEvent = $"Shelter named '{name}'";
                RaiseStateChanged();
            };
            _system.OnOriginSelected += originId =>
            {
                _lastEvent = $"Origin selected '{originId}'";
                RaiseStateChanged();
            };
            _system.OnInfamyChanged += value =>
            {
                _lastEvent = $"Infamy is now {value}";
                RaiseStateChanged();
            };

            if (!string.IsNullOrEmpty(dataDir))
                LoadCatalog(dataDir);
        }

        public static ShelterIdentityHostSession Create(string dataDir, ShelterIdentitySystem? system = null) =>
            new ShelterIdentityHostSession(dataDir, system);

        public void LoadCatalog(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;

            string path = Path.Combine(dataDir, "shelter_origins.json");
            if (!File.Exists(path)) return;

            var catalog = ShelterOriginCatalogLoader.LoadFromJson(File.ReadAllText(path));
            _system.BindOrigins(catalog);
            _lastEvent = $"Loaded {catalog.origins.Count} shelter origins.";
            RaiseStateChanged();
        }

        public ActionResult SetShelterName(string name) => _system.SetShelterName(name);
        public ActionResult SetMotto(string motto) => _system.SetMotto(motto);
        public ActionResult SetEmblem(string symbol, string color) => _system.SetEmblem(symbol, color);

        public ActionResult SelectOrigin(string originId, int day, string founderSurvivorId = "") =>
            _system.SelectOrigin(originId, day, founderSurvivorId);

        public ShelterOriginDef? GetSelectedOrigin() => _system.GetSelectedOrigin();
        public void RecordCommunityAction(string actionType, int magnitude = 1) => _system.RecordCommunityAction(actionType, magnitude);
        public void RecordFactionReputation(string factionId, int delta) => _system.RecordFactionReputation(factionId, delta);
        public int GetFactionReputation(string factionId) => _system.GetFactionReputation(factionId);
        public List<string> GetKnownForTags() => _system.GetKnownForTags();
        public string FormatText(string template) => _system.FormatText(template);
        public ShelterIdentityState CaptureState() => _system.CaptureState();
        public void RestoreState(ShelterIdentityState? state) => _system.RestoreState(state);
    }
}
