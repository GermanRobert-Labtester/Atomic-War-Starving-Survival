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
        private SurvivorMentalHealthSystem? _survivorMentalHealth;
        private bool _survivorMentalHealthDirty;
        public SurvivorMentalHealthSystem? SurvivorMentalHealthSystem => _survivorMentalHealth;

        // ── Plan 52: Survivor Mental Health ───────────────────────────────

        public SurvivorMentalHealthSystem EnsureSurvivorMentalHealth()
        {
            if (_survivorMentalHealth != null) return _survivorMentalHealth;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("survivor_mental_health") : new SeededRng(52);
            _survivorMentalHealth = new SurvivorMentalHealthSystem(null, rng);

            string path = Path.Combine(_dataDir, "psychological_trauma.json");
            if (System.IO.File.Exists(path))
            {
                try
                {
                    string json = System.IO.File.ReadAllText(path);
                    var catalog = PsychologicalTraumaCatalogLoader.Load(json, new SystemTextJsonSerializer());
                    _survivorMentalHealth.LoadCatalog(catalog);
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"[Ashfall Godot] Failed to load psychological_trauma.json: {ex.Message}");
                }
            }

            var saved = SurvivorMentalHealthSaveStore.TryLoad();
            if (saved != null)
            {
                _survivorMentalHealth.RestoreState(saved);
            }

            return _survivorMentalHealth;
        }

        private void SetupSurvivorMentalHealth() => EnsureSurvivorMentalHealth();

        private void SaveSurvivorMentalHealth()
        {
            if (_survivorMentalHealth == null) return;
            var state = _survivorMentalHealth.CaptureState();
            string payload = SurvivorMentalHealthSaveStore.TryCapturePersisted(state);
            if (CaptureSection(SurvivorMentalHealthSaveStore.SectionName, payload))
            {
                _survivorMentalHealthDirty = false;
            }
        }

    }
}
