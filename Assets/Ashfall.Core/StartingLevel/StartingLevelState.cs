using System;
using System.Collections.Generic;

namespace Ashfall.Core.StartingLevel
{
    /// <summary>
    /// Ration triage policies chosen by the player in the opening protocol.
    /// </summary>
    public enum RationPolicy
    {
        Standard = 0,    // Full rations: normal consumption, stable morale
        Half = 1,        // Half rations: 50% consumption, +hunger/thirst buildup, -morale
        Irradiated = 2   // Irradiated supplement: saves clean water, +rad dose
    }

    /// <summary>
    /// Maintenance priorities chosen for shelter infrastructure on Day 1.
    /// </summary>
    public enum MaintenanceDirective
    {
        ServiceFilterStack = 0,  // Service HEPA filtration stack (-1 mechanical scrap, 0 radon leak)
        FortifyBunksLead = 1,    // Fortify bunk ceiling with lead (-2 scrap, -1 lead plate, upgrades ceiling to 99%)
        CalibrateMonitors = 2    // Calibrate dosimeters & geiger counter (accurate weather/fallout warnings)
    }

    /// <summary>
    /// Radio protocol chosen for the opening transmission on 142.850 MHz.
    /// </summary>
    public enum RadioProtocol
    {
        AcknowledgeHydroBarons = 0, // Acknowledge Coastal Hydro-Barons (reveals crossing notice & trade)
        MaintainSilence = 1,        // Maintain radio silence (prevents raider detection, +security)
        BroadcastBeacon = 2         // Broadcast Holdfast beacon (signals wandering traders, slight risk)
    }

    /// <summary>
    /// DTO representing the state of an individual shelter room.
    /// </summary>
    [Serializable]
    public class ShelterRoomState
    {
        public string roomId = string.Empty;
        public string displayName = string.Empty;
        public string material = "Concrete";
        public float attenuation = 0.80f;
        public bool isInspected;
    }

    /// <summary>
    /// Serializable snapshot for the starting level holdfast simulation.
    /// </summary>
    [Serializable]
    public class StartingLevelSaveState
    {
        public int day = 1;
        public string locationId = "loc_bunker_holdfast";
        public RationPolicy rationPolicy = RationPolicy.Standard;
        public MaintenanceDirective maintenanceDirective = MaintenanceDirective.ServiceFilterStack;
        public RadioProtocol radioProtocol = RadioProtocol.AcknowledgeHydroBarons;

        public bool morningTriageResolved;
        public bool middayMaintenanceResolved;
        public bool eveningRadioResolved;

        // Shelter Air & Filtration System
        public float airFilterHealthPercent = 100.0f;
        public float airQualityPercent = 100.0f;
        public float radonLevelBqm3 = 12.0f;
        public bool airHazardWarning;
        public int filterSparesCount = 1;
        public int mechanicalScrapCount = 6;

        public List<ShelterRoomState> rooms = new List<ShelterRoomState>();
        public List<string> journalDirectives = new List<string>();
        public int daysSurvived = 1;
    }
}
