// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plans 46-49 Host Wire & Orchestration
// Subsystems   : Precision Workshop, Radio Intelligence, Shelter Social Dynamics, Subterranean Hazards
// ============================================================================
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Excavation;
using Ashfall.Core.IO;
using Ashfall.Core.Radio;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ShelterWorkshopSystem? _shelterWorkshop;
        private ShelterRadioStationSystem? _radioStationSystem;
        private ShelterSocialDynamicsSystem? _shelterSocialDynamics;
        private ExcavationHazardSystem? _excavationHazards;

        private bool _shelterWorkshopDirty;
        private bool _radioStationDirty;
        private bool _shelterSocialDirty;
        private bool _excavationHazardsDirty;

        // ── Plan 46: Precision Workshop ─────────────────────────────────

        public ShelterWorkshopSystem EnsureShelterWorkshop()
        {
            if (_shelterWorkshop != null) return _shelterWorkshop;

            var inv = _inventoryHost?.Inventory ?? new Ashfall.Core.Inventory.Inventory { Capacity = 100, MaxWeight = 500f };
            var rng = _campaignDay?.Rng ?? new SeededRng(46);
            var equip = _equipmentCondition?.System;
            var veh = _expeditions?.Vehicles;

            _shelterWorkshop = new ShelterWorkshopSystem(inv, rng, equip, veh, new GodotLog());

            // Load authoritative catalog
            string catalogPath = "res://Assets/StreamingAssets/Data/workshop_recipes.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
                    _shelterWorkshop.LoadCatalog(json);
                }
            }

            var saved = ShelterWorkshopSaveStore.TryLoad();
            if (saved != null)
            {
                _shelterWorkshop.RestoreState(saved);
            }

            _shelterWorkshop.OnWorkshopChanged += () => _shelterWorkshopDirty = true;
            return _shelterWorkshop;
        }

        private void SetupWorkshop()
        {
            EnsureShelterWorkshop();
        }

        private void SaveWorkshop()
        {
            if (_shelterWorkshop != null)
            {
                CaptureSection("shelter_workshop", ShelterWorkshopSaveStore.TryCapturePersisted(_shelterWorkshop.CaptureState()));
                _shelterWorkshopDirty = false;
            }
        }

        // ── Plan 47: Wasteland Radio Intelligence ───────────────────────

        public ShelterRadioStationSystem EnsureRadioStation()
        {
            if (_radioStationSystem != null) return _radioStationSystem;

            var rng = _campaignDay?.Rng ?? new SeededRng(47);
            _radioStationSystem = new ShelterRadioStationSystem(rng, null, new GodotLog());

            string catalogPath = "res://Assets/StreamingAssets/Data/radio_intercepts.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
                    _radioStationSystem.LoadCatalog(json);
                }
            }

            var saved = RadioStationSaveStore.TryLoad();
            if (saved != null)
            {
                _radioStationSystem.RestoreState(saved);
            }

            _radioStationSystem.OnLocationTriangulated += (interceptId, locationId) =>
            {
                _journal?.TryAddRawEntry("radio_triangulation", $"Triangulated coordinates for {locationId} via intercept {interceptId}", null!, _simDay);
            };

            _radioStationSystem.OnRadioStateChanged += () => _radioStationDirty = true;
            return _radioStationSystem;
        }

        private void SetupRadioStation()
        {
            EnsureRadioStation();
        }

        private void SaveRadioStation()
        {
            if (_radioStationSystem != null)
            {
                CaptureSection("radio_station", RadioStationSaveStore.TryCapturePersisted(_radioStationSystem.CaptureState()));
                _radioStationDirty = false;
            }
        }

        // ── Plan 48: Shelter Social Dynamics ────────────────────────────

        public ShelterSocialDynamicsSystem EnsureShelterSocialDynamics()
        {
            if (_shelterSocialDynamics != null) return _shelterSocialDynamics;

            var rng = _campaignDay?.Rng ?? new SeededRng(48);
            var relations = _survivorRelations?.System;
            var needs = _survivors?.HostSession?.Needs;
            var memorial = _memorial;

            _shelterSocialDynamics = new ShelterSocialDynamicsSystem(rng, relations, needs, memorial, new GodotLog());

            string catalogPath = "res://Assets/StreamingAssets/Data/shelter_social_events.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
                    _shelterSocialDynamics.LoadCatalog(json);
                }
            }

            var saved = ShelterSocialSaveStore.TryLoad();
            if (saved != null)
            {
                _shelterSocialDynamics.RestoreState(saved);
            }

            _shelterSocialDynamics.OnIncidentTriggered += inc =>
            {
                _journal?.TryAddRawEntry("shelter_social_incident", $"Social incident occurred in {inc.RoomId} (event: {inc.EventId})", null!, _simDay);
            };

            _shelterSocialDynamics.OnSocialStateChanged += () => _shelterSocialDirty = true;
            return _shelterSocialDynamics;
        }

        private void SetupShelterSocial()
        {
            EnsureShelterSocialDynamics();
        }

        private void SaveShelterSocial()
        {
            if (_shelterSocialDynamics != null)
            {
                CaptureSection("shelter_social_dynamics", ShelterSocialSaveStore.TryCapturePersisted(_shelterSocialDynamics.CaptureState()));
                _shelterSocialDirty = false;
            }
        }

        // ── Plan 49: Subterranean Hazard Operations ─────────────────────

        public ExcavationHazardSystem EnsureExcavationHazards()
        {
            if (_excavationHazards != null) return _excavationHazards;

            var inv = _inventoryHost?.Inventory ?? new Ashfall.Core.Inventory.Inventory { Capacity = 100, MaxWeight = 500f };
            var rng = _campaignDay?.Rng ?? new SeededRng(49);
            var excavation = _excavation?.System;

            _excavationHazards = new ExcavationHazardSystem(inv, rng, excavation, null, new GodotLog());

            string catalogPath = "res://Assets/StreamingAssets/Data/excavation_hazard_mitigation.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
                    _excavationHazards.LoadCatalog(json);
                }
            }

            var saved = ExcavationHazardSaveStore.TryLoad();
            if (saved != null)
            {
                _excavationHazards.RestoreState(saved);
            }

            _excavationHazards.OnRescueStarted += (sec, count) =>
            {
                _journal?.TryAddRawEntry("excavation_rescue_started", $"Cave-in emergency! {count} miner(s) trapped in {sec}", null!, _simDay);
            };

            _excavationHazards.OnHazardStateChanged += () => _excavationHazardsDirty = true;
            return _excavationHazards;
        }

        private void SetupExcavationHazards()
        {
            EnsureExcavationHazards();
        }

        private void SaveExcavationHazards()
        {
            if (_excavationHazards != null)
            {
                CaptureSection("excavation_hazards", ExcavationHazardSaveStore.TryCapturePersisted(_excavationHazards.CaptureState()));
                _excavationHazardsDirty = false;
            }
        }

        // ── Daily Tick Orchestration for Plans 46-49 ───────────────────

        private void TickPlans46_49(int day, List<DayStateChangeEvent> events)
        {
            if (_shelterWorkshop != null)
            {
                _shelterWorkshop.TickDay(day);
                _shelterWorkshopDirty = true;
            }

            if (_radioStationSystem != null)
            {
                _radioStationSystem.TickDay(day);
                _radioStationDirty = true;
            }

            if (_shelterSocialDynamics != null)
            {
                _shelterSocialDynamics.TickDay(day);
                _shelterSocialDirty = true;
            }

            if (_excavationHazards != null)
            {
                _excavationHazards.TickDay(day);
                _excavationHazardsDirty = true;
            }
        }
    }
}
