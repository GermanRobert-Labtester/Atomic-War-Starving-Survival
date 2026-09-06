// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plans 50-53 Host Wire & Orchestration
// Subsystems   : Vehicle Garage, Faction Espionage, Survivor Mental Health, Shelter Acoustic Director
// ============================================================================
using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Audio;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Factions;
using Ashfall.Core.Needs;
using AtomicWar.GodotApp.Audio;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private VehicleGarageSystem? _vehicleGarage;
        private ShelterEspionageSystem? _shelterEspionage;
        private SurvivorMentalHealthSystem? _survivorMentalHealth;
        private ShelterAcousticDirector? _shelterAcousticDirector;
        private ShelterAcousticBridge? _shelterAcousticBridge;

        private bool _vehicleGarageDirty;
        private bool _shelterEspionageDirty;
        private bool _survivorMentalHealthDirty;

        public VehicleGarageSystem? VehicleGarageSystem => _vehicleGarage;
        public ShelterEspionageSystem? ShelterEspionageSystem => _shelterEspionage;
        public SurvivorMentalHealthSystem? SurvivorMentalHealthSystem => _survivorMentalHealth;
        public ShelterAcousticDirector? ShelterAcousticDirector => _shelterAcousticDirector;
        public ShelterAcousticBridge? ShelterAcousticBridge => _shelterAcousticBridge;

        // ── Plan 50: Vehicle Garage ───────────────────────────────────────

        public VehicleGarageSystem EnsureVehicleGarage()
        {
            if (_vehicleGarage != null) return _vehicleGarage;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("vehicle_garage") : new SeededRng(50);
            _vehicleGarage = new VehicleGarageSystem(null, rng);

            string path = Path.Combine(_dataDir, "vehicle_modifications.json");
            if (System.IO.File.Exists(path))
            {
                try
                {
                    string json = System.IO.File.ReadAllText(path);
                    var catalog = VehicleGarageCatalogLoader.Load(json, new SystemTextJsonSerializer());
                    _vehicleGarage.LoadCatalog(catalog);
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"[Ashfall Godot] Failed to load vehicle_modifications.json: {ex.Message}");
                }
            }

            var saved = VehicleGarageSaveStore.TryLoad();
            if (saved != null)
            {
                _vehicleGarage.RestoreState(saved);
            }

            return _vehicleGarage;
        }

        private void SetupVehicleGarage() => EnsureVehicleGarage();

        private void SaveVehicleGarage()
        {
            if (_vehicleGarage == null) return;
            var state = _vehicleGarage.CaptureState();
            string payload = VehicleGarageSaveStore.TryCapturePersisted(state);
            if (CaptureSection(VehicleGarageSaveStore.SectionName, payload))
            {
                _vehicleGarageDirty = false;
            }
        }

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

        // ── Combined Lifecycle ───────────────────────────────────────────

        public void SetupPlans50To53()
        {
            EnsureVehicleGarage();
            EnsureShelterEspionage();
            EnsureSurvivorMentalHealth();
            EnsureShelterAcoustics();
        }

        public void FlushPlans50To53()
        {
            if (_vehicleGarageDirty) SaveVehicleGarage();
            if (_shelterEspionageDirty) SaveShelterEspionage();
            if (_survivorMentalHealthDirty) SaveSurvivorMentalHealth();
        }

        public void TickPlans50To53(int currentDay)
        {
            if (_shelterEspionage != null)
            {
                _shelterEspionage.TickDay(currentDay, _inventory?.Inventory);
                _shelterEspionageDirty = true;
            }

            if (_survivorMentalHealth != null)
            {
                _survivorMentalHealth.TickDay(currentDay);
                _survivorMentalHealthDirty = true;
            }

            _shelterAcousticBridge?.SyncAcoustics();
        }
    }
}
