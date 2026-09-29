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
        private ShelterAcousticDirector? _shelterAcousticDirector;
        private ShelterAcousticBridge? _shelterAcousticBridge;
        public ShelterAcousticDirector? ShelterAcousticDirector => _shelterAcousticDirector;
        public ShelterAcousticBridge? ShelterAcousticBridge => _shelterAcousticBridge;

        // ── Plan 53: Shelter Acoustic Director ────────────────────────────

        public ShelterAcousticDirector EnsureShelterAcoustics()
        {
            if (_shelterAcousticDirector != null) return _shelterAcousticDirector;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("shelter_acoustics") : new SeededRng(53);
            _shelterAcousticDirector = new ShelterAcousticDirector(null, rng);

            string path = Path.Combine(_dataDir, "shelter_audio_cues.json");
            if (System.IO.File.Exists(path))
            {
                try
                {
                    string json = System.IO.File.ReadAllText(path);
                    var catalog = ShelterAudioCueCatalogLoader.Load(json, new SystemTextJsonSerializer());
                    _shelterAcousticDirector.LoadCatalog(catalog);
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"[Ashfall Godot] Failed to load shelter_audio_cues.json: {ex.Message}");
                }
            }

            if (_audio != null)
            {
                _shelterAcousticBridge = new ShelterAcousticBridge(_audio);
                _shelterAcousticBridge.BindDirector(_shelterAcousticDirector);
            }

            return _shelterAcousticDirector;
        }

        public void SetupShelterAcoustics() => EnsureShelterAcoustics();

    }
}
