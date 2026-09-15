// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;
using Ashfall.Core.PlayerCommand;
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
        public const string HoldfastLocationId = "loc_holdfast";
        /// <summary>Pre-Plan-29 save value for the Holdfast location id; migrated on restore (data authority: locations.json).</summary>
        public const string LegacyHoldfastLocationId = "loc_bunker_holdfast";

        public StartingLevelSaveState State { get; private set; } = new StartingLevelSaveState();

        public const string AirFilterServiceItemId = "scrap_mechanical";
        public const string AirFilterReplacementItemId = "item_air_filter_hepa";
        public const string AirFilterResearchId = "knowledge_air_filtration";

        private IPlayerInventoryPort? _maintenanceInventory;
        private Func<string, bool>? _maintenanceCapability;

        public event Action? OnStateChanged;
        public event Action<string>? OnDirectiveLogged;

        public StartingLevelSystem()
        {
            InitializeDefaultHoldfast();
        }

        /// <summary>
        /// Bind the campaign-owned inventory and research capability query.
        /// The starting-level system owns the air state, while the campaign
        /// roots own item and research truth. This keeps maintenance actions
        /// transactional without introducing shelter-local counters or flags.
        /// </summary>
        public void BindMaintenance(
            IPlayerInventoryPort inventory,
            Func<string, bool>? hasCapability = null)
        {
            _maintenanceInventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _maintenanceCapability = hasCapability;
        }

        public bool HasMaintenanceDependencies => _maintenanceInventory != null;

        public string AirFilterConditionBand
        {
            get
            {
                if (State.airFilterHealthPercent >= 75f) return "healthy";
                if (State.airFilterHealthPercent >= 50f) return "degraded";
                if (State.airFilterHealthPercent > 0f) return "critical";
                return "failed";
            }
        }

        /// <summary>Side-effect-free preview for the real inventory-backed path.</summary>
        public CommandPreview PreviewMaintainAirFilter(bool replace = false)
        {
            const string command = "shelter.maintain_air_filter";
            if (_maintenanceInventory == null)
                return CommandPreview.Unavailable(command, "maintenance_dependencies_missing", "shelter.maintenance_dependencies_missing", 0);

            string itemId = replace ? AirFilterReplacementItemId : AirFilterServiceItemId;
            if (replace && (_maintenanceCapability == null || !_maintenanceCapability(AirFilterResearchId)))
                return CommandPreview.Unavailable(command, "research_required", "shelter.research_required", 0);
            if (State.airFilterHealthPercent >= 100f)
                return CommandPreview.Unavailable(command, "maintenance_not_needed", "shelter.maintenance_not_needed", 0);
            if (_maintenanceInventory.CountById(itemId) < 1)
                return CommandPreview.Unavailable(command, "missing_maintenance_item", "shelter.missing_maintenance_item", 0);

            return CommandPreview.Available(
                command,
                0,
                new Dictionary<string, double> { [itemId] = -1, ["air_filter_health_percent"] = replace ? 100f - State.airFilterHealthPercent : Math.Min(25f, 100f - State.airFilterHealthPercent) },
                messageKey: replace ? "shelter.air_filter_replacement_available" : "shelter.air_filter_service_available");
        }

        /// <summary>
        /// Consume a real maintenance part and restore the owning air state.
        /// Validation completes before the atomic inventory bill is consumed.
        /// </summary>
        public ActionResult MaintainAirFilter(bool replace = false)
        {
            var preview = PreviewMaintainAirFilter(replace);
            if (!preview.IsAvailable)
                return ActionResult.Blocked(preview.FailureCode, preview.MessageKey);

            string itemId = replace ? AirFilterReplacementItemId : AirFilterServiceItemId;
            if (!_maintenanceInventory!.TryConsumeBill(new Dictionary<string, int> { [itemId] = 1 }))
                return ActionResult.Blocked("missing_maintenance_item", "shelter.missing_maintenance_item");

            float before = State.airFilterHealthPercent;
            if (replace)
            {
                State.airFilterHealthPercent = 100f;
                State.airQualityPercent = 100f;
                State.radonLevelBqm3 = 12f;
                State.airHazardWarning = false;
            }
            else
            {
                State.airFilterHealthPercent = Math.Min(100f, before + 25f);
                State.airQualityPercent = Math.Clamp(State.airFilterHealthPercent * 0.9f + 10f, 0f, 100f);
                State.radonLevelBqm3 = Math.Max(12f, State.radonLevelBqm3 - 15f);
                State.airHazardWarning = State.airFilterHealthPercent < 50f || State.radonLevelBqm3 > 30f;
            }
            LogDirective(replace
                ? "[MAINTENANCE] Replaced HEPA air filter core from inventory (100% integrity restored)."
                : $"[MAINTENANCE] Serviced HEPA air filtration stack (-1 {AirFilterServiceItemId}, integrity now {State.airFilterHealthPercent:0}%).");
            OnStateChanged?.Invoke();
            return ActionResult.Success(
                replace ? "shelter.air_filter_replaced" : "shelter.air_filter_serviced",
                new Dictionary<string, double>
                {
                    [itemId] = -1,
                    ["air_filter_health_percent"] = State.airFilterHealthPercent - before
                });
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

        /// <summary>
        /// Inspect a room of the Day-1 roster. Returns true when the room id is
        /// known (already-inspected rooms return true without re-logging).
        /// </summary>
        public bool InspectRoom(string roomId)
        {
            var room = State.rooms.Find(r => r.roomId == roomId);
            if (room == null) return false;
            if (!room.isInspected)
            {
                room.isInspected = true;
                LogDirective($"Inspected {room.displayName} (Ceiling: {room.material}, {room.attenuation:P0} rad attenuation).");
                OnStateChanged?.Invoke();
            }
            return true;
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
            => TickDay(isFilterDutyAssigned, outdoorWeather, powerAvailability01: 1f);

        /// <summary>
        /// Advance one shelter day. <paramref name="powerAvailability01"/> is the
        /// grid-derived power fraction for the filtration plant (canonical room
        /// <c>room_air_filtration</c>, failure effect <c>fx_filtration_off</c>).
        /// With zero power the active scrubbing stack is offline: the filter
        /// clogs at hazard-weather rate regardless of duty roster, and the
        /// powered +10 quality offset disappears (the stack stops pushing
        /// clean air). Defaults to 1 for legacy callers.
        /// </summary>
        public void TickDay(bool isFilterDutyAssigned, WeatherKind outdoorWeather, float powerAvailability01)
        {
            State.day++;
            State.daysSurvived++;
            State.morningTriageResolved = false;
            State.middayMaintenanceResolved = false;
            State.eveningRadioResolved = false;

            // ── Air Filtration Degradation ──
            bool filtrationPowered = powerAvailability01 > 0f;
            float baseDegrade = 5.0f;
            bool isHazardWeather = outdoorWeather == WeatherKind.FalloutStorm ||
                                   outdoorWeather == WeatherKind.BlackRain ||
                                   outdoorWeather == WeatherKind.Ashfall;
            if (isHazardWeather)
            {
                baseDegrade += 4.0f; // Heavy particulate / fallout storm clogging
            }
            if (!filtrationPowered)
            {
                // fx_filtration_off: the powered scrubbing stack is offline —
                // the filter clogs at hazard-weather rate regardless of duty
                // roster (intake maintenance cannot run without power).
                baseDegrade += 4.0f;
                isFilterDutyAssigned = false;
            }

            if (_maintenanceCapability != null && _maintenanceCapability(AirFilterResearchId))
            {
                // The authored “+50% lifespan” claim is represented as a
                // 2/3 degradation rate. It is applied once in the owner of
                // air-filter degradation, not cached as shadow state.
                baseDegrade *= 2f / 3f;
            }

            if (isFilterDutyAssigned)
            {
                baseDegrade *= 0.5f; // Duty Roster intake maintenance halves degradation
            }

            State.airFilterHealthPercent = Math.Max(0.0f, State.airFilterHealthPercent - baseDegrade);
            // Powered scrubbing adds a +10 quality offset; an offline stack loses it.
            float poweredOffset = filtrationPowered ? 10f : 0f;
            State.airQualityPercent = Math.Clamp(State.airFilterHealthPercent * 0.9f + poweredOffset, 0f, 100f);

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
                locationId = string.IsNullOrWhiteSpace(save.locationId) || string.Equals(save.locationId, LegacyHoldfastLocationId, StringComparison.Ordinal)
                    ? HoldfastLocationId
                    : save.locationId,
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
