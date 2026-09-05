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
using Ashfall.Core.Quests;
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
        private DynamicQuestlineSystem? _dynamicQuests;

        private bool _shelterWorkshopDirty;
        private bool _radioStationDirty;
        private bool _shelterSocialDirty;
        private bool _excavationHazardsDirty;
        private bool _dynamicQuestsDirty;

        // ── Plan 46: Precision Workshop ─────────────────────────────────

        public ShelterWorkshopSystem EnsureShelterWorkshop()
        {
            if (_shelterWorkshop != null) return _shelterWorkshop;

            SetupInventory();
            var inv = _inventory.Inventory;
            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("shelter_workshop") : new SeededRng(46);
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

            _shelterWorkshop.OnJobCompleted += (job) =>
            {
                if (job.RecipeId.Contains("weapon", StringComparison.OrdinalIgnoreCase) ||
                    job.RecipeId.Contains("repair", StringComparison.OrdinalIgnoreCase) ||
                    job.RecipeId.Contains("ammo", StringComparison.OrdinalIgnoreCase))
                {
                    _dynamicQuests?.AdvanceQuestProgress(DynamicQuestlineSystem.ArmoryMunitionsRefurbishQuestId, 1);
                }
            };

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

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("radio_station") : new SeededRng(47);
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
                _world?.WastelandMap?.Discover(locationId);
                EnsureDynamicQuests().TriggerInvestigateRadioDepotQuest(interceptId, locationId, _simDay);
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

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("shelter_social") : new SeededRng(48);
            var relations = _survivorRelations?.System;
            var needs = _survivors?.Needs;
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

            SetupInventory();
            var inv = _inventory.Inventory;
            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("excavation_hazards") : new SeededRng(49);
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
                var trapped = _excavationHazards.State.sectors.TryGetValue(sec, out var s) ? s.ActiveTrappedMiners : new List<string>();
                EnsureDynamicQuests().TriggerRescueMinersQuest($"rescue_{sec}_{_simDay}", sec, trapped, _simDay);
            };

            _excavationHazards.OnRescueSucceeded += (sec) =>
            {
                var q = _dynamicQuests?.GetActiveQuest(DynamicQuestlineSystem.RescueMinersQuestId);
                if (q != null && q.TargetLocationId == sec)
                {
                    _dynamicQuests?.CompleteQuest(q.QuestId);
                }
            };

            _excavationHazards.OnRescueFailed += (sec) =>
            {
                var q = _dynamicQuests?.GetActiveQuest(DynamicQuestlineSystem.RescueMinersQuestId);
                if (q != null && q.TargetLocationId == sec)
                {
                    _dynamicQuests?.FailQuest(q.QuestId);
                }

                if (_excavationHazards.State.sectors.TryGetValue(sec, out var s))
                {
                    foreach (var minerId in s.ActiveTrappedMiners)
                    {
                        _survivorFate?.ReportDeath(minerId, SurvivorDeathCause.Scripted, $"Died in excavation cave-in ({sec})", source: "excavation_hazard");
                    }
                }
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

        // ── Emergency Dynamic Quests ───────────────────────────────────

        public DynamicQuestlineSystem EnsureDynamicQuests()
        {
            if (_dynamicQuests != null) return _dynamicQuests;

            _dynamicQuests = new DynamicQuestlineSystem(new GodotLog());
            var saved = DynamicQuestSaveStore.TryLoad();
            if (saved != null)
            {
                _dynamicQuests.RestoreState(saved);
            }

            _dynamicQuests.OnStateChanged += () => _dynamicQuestsDirty = true;
            return _dynamicQuests;
        }

        private void SetupDynamicQuests()
        {
            EnsureDynamicQuests();
        }

        private void SaveDynamicQuests()
        {
            if (_dynamicQuests != null)
            {
                CaptureSection("dynamic_quests", DynamicQuestSaveStore.TryCapturePersisted(_dynamicQuests.CaptureState()));
                _dynamicQuestsDirty = false;
            }
        }

        // ── Daily Tick Orchestration for Plans 46-49 ───────────────────

        private void TickPlans46_49(int day, List<DayStateChangeEvent> events)
        {
            if (_shelterWorkshop != null)
            {
                _shelterWorkshop.TickDay(day);
                _shelterWorkshopDirty = true;

                foreach (var job in _shelterWorkshop.State.jobs)
                {
                    if (job.Status == WorkshopJobStatus.Completed || job.Status == WorkshopJobStatus.CompletedPendingCollection)
                    {
                        if (_shelterWorkshop.Recipes.TryGetValue(job.RecipeId, out var recipe))
                        {
                            events.Add(new DayStateChangeEvent("workshop_job_completed", "shelter_workshop", recipe.DisplayName, job.RoomId, job.YieldProduced));
                        }
                    }
                }

                foreach (var machine in _shelterWorkshop.State.machines.Values)
                {
                    if (machine.ToolingHealth <= 0.25f)
                    {
                        events.Add(new DayStateChangeEvent("workshop_machine_degraded", "shelter_workshop", machine.RoomId, null, machine.ToolingHealth));
                    }
                }
            }

            if (_radioStationSystem != null)
            {
                _radioStationSystem.TickDay(day);
                _radioStationDirty = true;

                foreach (var progress in _radioStationSystem.State.intercepts)
                {
                    if (!progress.IsExpired && progress.ExpiresOnDay.HasValue)
                    {
                        int daysLeft = progress.ExpiresOnDay.Value - day;
                        if (daysLeft <= 1)
                        {
                            events.Add(new DayStateChangeEvent("radio_distress_expiring", "radio_station", progress.InterceptId, null, daysLeft));
                        }
                        else
                        {
                            events.Add(new DayStateChangeEvent("radio_distress_active", "radio_station", progress.InterceptId, null, daysLeft));
                        }
                    }
                }
            }

            if (_shelterSocialDynamics != null)
            {
                _shelterSocialDynamics.TickDay(day);
                _shelterSocialDirty = true;

                foreach (var profile in _shelterSocialDynamics.State.privacyProfiles.Values)
                {
                    if (profile.PrivacyFatiguePermille >= 700)
                    {
                        events.Add(new DayStateChangeEvent("social_privacy_warning", "shelter_social_dynamics", profile.SurvivorId, profile.AssignedRoomId, profile.PrivacyFatiguePermille));
                    }
                }

                foreach (var inc in _shelterSocialDynamics.State.recentIncidents)
                {
                    if (inc.Day == day && !inc.Resolved)
                    {
                        events.Add(new DayStateChangeEvent("social_dispute_unresolved", "shelter_social_dynamics", inc.IncidentId, inc.RoomId, 0f));
                    }
                }
            }

            if (_excavationHazards != null)
            {
                _excavationHazards.TickDay(day);
                _excavationHazardsDirty = true;

                foreach (var sec in _excavationHazards.State.sectors.Values)
                {
                    if (sec.MethanePpm >= 2500)
                    {
                        events.Add(new DayStateChangeEvent("subterranean_methane_warning", "excavation_hazards", sec.SectorId, null, sec.MethanePpm));
                    }

                    if (sec.FloodLevelPermille >= 500)
                    {
                        events.Add(new DayStateChangeEvent("subterranean_flood_warning", "excavation_hazards", sec.SectorId, null, sec.FloodLevelPermille));
                    }

                    if (sec.ShoringHealthPermille <= 300)
                    {
                        events.Add(new DayStateChangeEvent("subterranean_shoring_warning", "excavation_hazards", sec.SectorId, null, sec.ShoringHealthPermille));
                    }

                    if (sec.ActiveTrappedMiners.Count > 0 && !sec.RescueCompleted && !sec.RescueFailed)
                    {
                        events.Add(new DayStateChangeEvent("subterranean_rescue_active", "excavation_hazards", sec.SectorId, null, sec.RescueLaborRemaining));
                    }
                    else if (sec.RescueFailed)
                    {
                        events.Add(new DayStateChangeEvent("subterranean_rescue_failed", "excavation_hazards", sec.SectorId, null, 0f));
                    }
                }
            }

            if (_dynamicQuests != null)
            {
                _dynamicQuests.TickDay(day);
                _dynamicQuestsDirty = true;
            }
        }
    }
}
