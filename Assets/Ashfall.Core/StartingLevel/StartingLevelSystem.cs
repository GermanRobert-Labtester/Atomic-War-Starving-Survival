using System;
using System.Collections.Generic;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.StartingLevel
{
    /// <summary>
    /// Engine-agnostic simulation system for ASHFALL's first playable starting level:
    /// The Holdfast (Day 1). Manages starting layout, initial decisions, ration policy,
    /// maintenance directives, and day-advance consequences.
    /// </summary>
    public class StartingLevelSystem
    {
        public const string HoldfastLocationId = "loc_bunker_holdfast";

        public StartingLevelSaveState State { get; private set; } = new StartingLevelSaveState();

        public event Action? OnStateChanged;
        public event Action<string>? OnDirectiveLogged;

        public StartingLevelSystem()
        {
            InitializeDefaultHoldfast();
        }

        public void InitializeDefaultHoldfast()
        {
            State = new StartingLevelSaveState
            {
                day = 1,
                locationId = HoldfastLocationId,
                rationPolicy = RationPolicy.Standard,
                maintenanceDirective = MaintenanceDirective.ServiceFilterStack,
                radioProtocol = RadioProtocol.AcknowledgeHydroBarons,
                morningTriageResolved = false,
                middayMaintenanceResolved = false,
                eveningRadioResolved = false,
                daysSurvived = 1,
                rooms = new List<ShelterRoomState>
                {
                    new ShelterRoomState
                    {
                        roomId = "room_bunker_corridor",
                        displayName = "Central Access Corridor",
                        material = "Concrete",
                        attenuation = 0.80f,
                        isInspected = true
                    },
                    new ShelterRoomState
                    {
                        roomId = "room_filtration_stack",
                        displayName = "Air Filtration & HEPA Bay",
                        material = "Lead",
                        attenuation = 0.99f,
                        isInspected = false
                    },
                    new ShelterRoomState
                    {
                        roomId = "room_storage_bay",
                        displayName = "Ration & Supply Locker",
                        material = "Concrete",
                        attenuation = 0.80f,
                        isInspected = true
                    },
                    new ShelterRoomState
                    {
                        roomId = "room_bunks_living",
                        displayName = "Survivor Bunk Quarters",
                        material = "Wood",
                        attenuation = 0.10f,
                        isInspected = false
                    },
                    new ShelterRoomState
                    {
                        roomId = "room_radio_tuner",
                        displayName = "142.850 MHz Tuner Station",
                        material = "Concrete",
                        attenuation = 0.80f,
                        isInspected = false
                    }
                },
                journalDirectives = new List<string>
                {
                    "Day 1: The ash has settled over the blast berm. The Holdfast airlocks are dogged down.",
                    "Standing Order: Reconcile clean water and canned food stores before the morning shift."
                }
            };
        }

        public void InspectRoom(string roomId)
        {
            var room = State.rooms.Find(r => r.roomId == roomId);
            if (room != null && !room.isInspected)
            {
                room.isInspected = true;
                LogDirective($"Inspected {room.displayName} (Ceiling: {room.material}, {room.attenuation:P0} rad attenuation).");
                OnStateChanged?.Invoke();
            }
        }

        public void UpgradeRoomShielding(string roomId, string material, float attenuation)
        {
            var room = State.rooms.Find(r => r.roomId == roomId);
            if (room != null)
            {
                room.material = material;
                room.attenuation = attenuation;
                LogDirective($"Upgraded {room.displayName} shielding to {material} ({attenuation:P0} attenuation).");
                OnStateChanged?.Invoke();
            }
        }

        public void ResolveMorningRationTriage(RationPolicy policy)
        {
            State.rationPolicy = policy;
            State.morningTriageResolved = true;

            string policyDesc = policy switch
            {
                RationPolicy.Standard => "Standard Rations (Full food & clean water). Survivor morale steady.",
                RationPolicy.Half => "Half Rations (50% consumption). Preserving stores; mild fatigue accumulation.",
                RationPolicy.Irradiated => "Emergency Irradiated Supplement. Conserving clean water; radiation exposure watch.",
                _ => "Standard Rations"
            };

            LogDirective($"[MORNING TRIAGE] Set ration policy: {policyDesc}");
            OnStateChanged?.Invoke();
        }

        public void ResolveMiddayMaintenance(MaintenanceDirective directive)
        {
            State.maintenanceDirective = directive;
            State.middayMaintenanceResolved = true;

            string directiveDesc = directive switch
            {
                MaintenanceDirective.ServiceFilterStack => "Serviced HEPA filtration stack. Filter pressure stabilized at 100%.",
                MaintenanceDirective.FortifyBunksLead => "Fortified Bunk Quarters with lead plating (99% ceiling attenuation).",
                MaintenanceDirective.CalibrateMonitors => "Calibrated quartz dosimeters & Geiger M3 instrumentation.",
                _ => "Serviced filtration stack"
            };

            if (directive == MaintenanceDirective.FortifyBunksLead)
            {
                UpgradeRoomShielding("room_bunks_living", "Lead", 0.99f);
            }

            LogDirective($"[MIDDAY MAINTENANCE] {directiveDesc}");
            OnStateChanged?.Invoke();
        }

        public void ResolveEveningRadio(RadioProtocol protocol)
        {
            State.radioProtocol = protocol;
            State.eveningRadioResolved = true;

            string radioDesc = protocol switch
            {
                RadioProtocol.AcknowledgeHydroBarons => "Acknowledged Coastal Hydro-Barons on 142.850 MHz. Rate card and crossing notice recorded.",
                RadioProtocol.MaintainSilence => "Maintained radio silence. Kept holdfast location dark from raiding patrols.",
                RadioProtocol.BroadcastBeacon => "Transmitted low-power Holdfast emergency beacon. Wandering merchants notified.",
                _ => "Acknowledged frequency"
            };

            LogDirective($"[EVENING RADIO] {radioDesc}");
            OnStateChanged?.Invoke();
        }

        public bool ServiceAirFilter()
        {
            if (State.mechanicalScrapCount <= 0) return false;
            State.mechanicalScrapCount--;
            State.airFilterHealthPercent = Math.Min(100.0f, State.airFilterHealthPercent + 25.0f);
            State.airQualityPercent = Math.Clamp(State.airFilterHealthPercent * 0.9f + 10f, 0f, 100f);
            State.radonLevelBqm3 = Math.Max(12.0f, State.radonLevelBqm3 - 15.0f);
            State.airHazardWarning = State.airFilterHealthPercent < 50.0f || State.radonLevelBqm3 > 30.0f;
            LogDirective($"[MAINTENANCE] Serviced HEPA air filtration stack (-1 scrap, integrity now {State.airFilterHealthPercent:0}%).");
            OnStateChanged?.Invoke();
            return true;
        }

        public bool ReplaceAirFilter()
        {
            if (State.filterSparesCount <= 0) return false;
            State.filterSparesCount--;
            State.airFilterHealthPercent = 100.0f;
            State.airQualityPercent = 100.0f;
            State.radonLevelBqm3 = 12.0f;
            State.airHazardWarning = false;
            LogDirective("[MAINTENANCE] Replaced HEPA air filter core with fresh cartridge (100% integrity restored).");
            OnStateChanged?.Invoke();
            return true;
        }

        public void TickDay() => TickDay(false, WeatherKind.Clear);

        public void TickDay(bool isFilterDutyAssigned, WeatherKind outdoorWeather)
        {
            State.day++;
            State.daysSurvived++;
            State.morningTriageResolved = false;
            State.middayMaintenanceResolved = false;
            State.eveningRadioResolved = false;

            // ── Air Filtration Degradation ──
            float baseDegrade = 5.0f;
            bool isHazardWeather = outdoorWeather == WeatherKind.FalloutStorm ||
                                   outdoorWeather == WeatherKind.BlackRain ||
                                   outdoorWeather == WeatherKind.Ashfall;
            if (isHazardWeather)
            {
                baseDegrade += 4.0f; // Heavy particulate / fallout storm clogging
            }

            if (isFilterDutyAssigned)
            {
                baseDegrade *= 0.5f; // Duty Roster intake maintenance halves degradation
            }

            State.airFilterHealthPercent = Math.Max(0.0f, State.airFilterHealthPercent - baseDegrade);
            State.airQualityPercent = Math.Clamp(State.airFilterHealthPercent * 0.9f + 10f, 0f, 100f);

            if (State.airFilterHealthPercent < 50.0f)
            {
                float radonInflow = (50.0f - State.airFilterHealthPercent) * 0.4f;
                State.radonLevelBqm3 = Math.Min(150.0f, State.radonLevelBqm3 + radonInflow);
            }
            else
            {
                State.radonLevelBqm3 = Math.Max(12.0f, State.radonLevelBqm3 - 2.0f);
            }

            State.airHazardWarning = State.airFilterHealthPercent < 50.0f || State.radonLevelBqm3 > 30.0f;

            string airStatus = State.airHazardWarning
                ? $"WARNING: Air quality degraded ({State.airQualityPercent:0}%, Radon {State.radonLevelBqm3:0} Bq/m³)."
                : $"Atmosphere holding (Filter: {State.airFilterHealthPercent:0}%, Radon: {State.radonLevelBqm3:0} Bq/m³).";

            LogDirective($"Day {State.day:00}: Holdfast cycle begun. {airStatus}");
            OnStateChanged?.Invoke();
        }

        public void LogDirective(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            State.journalDirectives.Add(message);
            OnDirectiveLogged?.Invoke(message);
        }

        public StartingLevelSaveState CaptureState()
        {
            var save = new StartingLevelSaveState
            {
                day = State.day,
                locationId = State.locationId,
                rationPolicy = State.rationPolicy,
                maintenanceDirective = State.maintenanceDirective,
                radioProtocol = State.radioProtocol,
                morningTriageResolved = State.morningTriageResolved,
                middayMaintenanceResolved = State.middayMaintenanceResolved,
                eveningRadioResolved = State.eveningRadioResolved,
                airFilterHealthPercent = State.airFilterHealthPercent,
                airQualityPercent = State.airQualityPercent,
                radonLevelBqm3 = State.radonLevelBqm3,
                airHazardWarning = State.airHazardWarning,
                filterSparesCount = State.filterSparesCount,
                mechanicalScrapCount = State.mechanicalScrapCount,
                daysSurvived = State.daysSurvived,
                rooms = new List<ShelterRoomState>(),
                journalDirectives = new List<string>(State.journalDirectives)
            };

            foreach (var r in State.rooms)
            {
                save.rooms.Add(new ShelterRoomState
                {
                    roomId = r.roomId,
                    displayName = r.displayName,
                    material = r.material,
                    attenuation = r.attenuation,
                    isInspected = r.isInspected
                });
            }

            return save;
        }

        public void RestoreState(StartingLevelSaveState save)
        {
            if (save == null) return;
            State = new StartingLevelSaveState
            {
                day = save.day,
                locationId = string.IsNullOrWhiteSpace(save.locationId) ? HoldfastLocationId : save.locationId,
                rationPolicy = save.rationPolicy,
                maintenanceDirective = save.maintenanceDirective,
                radioProtocol = save.radioProtocol,
                morningTriageResolved = save.morningTriageResolved,
                middayMaintenanceResolved = save.middayMaintenanceResolved,
                eveningRadioResolved = save.eveningRadioResolved,
                airFilterHealthPercent = save.airFilterHealthPercent > 0f ? save.airFilterHealthPercent : 100.0f,
                airQualityPercent = save.airQualityPercent > 0f ? save.airQualityPercent : 100.0f,
                radonLevelBqm3 = save.radonLevelBqm3 > 0f ? save.radonLevelBqm3 : 12.0f,
                airHazardWarning = save.airHazardWarning,
                filterSparesCount = save.filterSparesCount >= 0 ? save.filterSparesCount : 1,
                mechanicalScrapCount = save.mechanicalScrapCount >= 0 ? save.mechanicalScrapCount : 6,
                daysSurvived = Math.Max(1, save.daysSurvived),
                rooms = new List<ShelterRoomState>(),
                journalDirectives = new List<string>(save.journalDirectives ?? new List<string>())
            };

            if (save.rooms != null && save.rooms.Count > 0)
            {
                foreach (var r in save.rooms)
                {
                    State.rooms.Add(new ShelterRoomState
                    {
                        roomId = r.roomId,
                        displayName = r.displayName,
                        material = r.material,
                        attenuation = r.attenuation,
                        isInspected = r.isInspected
                    });
                }
            }
            else
            {
                InitializeDefaultHoldfast();
            }

            OnStateChanged?.Invoke();
        }
    }
}
