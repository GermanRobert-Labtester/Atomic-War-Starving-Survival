// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Audio;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Factions;
using Ashfall.Core.Needs;
using AtomicWar.GodotApp.Audio;
using Godot;
using System;
using System.IO;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ShelterEspionageSystem? _shelterEspionage;
        private bool _shelterEspionageDirty;
        public ShelterEspionageSystem? ShelterEspionageSystem => _shelterEspionage;

        // ── Plan 51: Faction Espionage ────────────────────────────────────

        public ShelterEspionageSystem EnsureShelterEspionage()
        {
            if (_shelterEspionage != null) return _shelterEspionage;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("shelter_espionage") : new SeededRng(51);
            _shelterEspionage = new ShelterEspionageSystem(null, rng);

            string path = Path.Combine(_dataDir, "faction_intelligence.json");
            if (System.IO.File.Exists(path))
            {
                try
                {
                    string json = System.IO.File.ReadAllText(path);
                    var catalog = FactionIntelligenceCatalogLoader.Load(json, new SystemTextJsonSerializer());
                    _shelterEspionage.LoadCatalog(catalog);
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"[Ashfall Godot] Failed to load faction_intelligence.json: {ex.Message}");
                }
            }

            var saved = ShelterEspionageSaveStore.TryLoad();
            if (saved != null)
            {
                _shelterEspionage.RestoreState(saved);
            }

            return _shelterEspionage;
        }

        private void SetupShelterEspionage() => EnsureShelterEspionage();

        private void SaveShelterEspionage()
        {
            if (_shelterEspionage == null) return;
            var state = _shelterEspionage.CaptureState();
            string payload = ShelterEspionageSaveStore.TryCapturePersisted(state);
            if (CaptureSection(ShelterEspionageSaveStore.SectionName, payload))
            {
                _shelterEspionageDirty = false;
            }
        }

    }
}
