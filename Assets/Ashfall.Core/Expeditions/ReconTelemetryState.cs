using System;
using System.Collections.Generic;

namespace Ashfall.Core.Expeditions
{
    [Serializable]
    public sealed class ReconTelemetryState
    {
        public string systemId = ReconTelemetrySystem.SystemId;
        public List<ActiveReconMissionState> activeMissions = new List<ActiveReconMissionState>();
        public List<string> launchedPlatformIds = new List<string>();
        public List<string> surveyedSectorIds = new List<string>();
        public List<ReconForecastRecord> forecasts = new List<ReconForecastRecord>();
        public List<RouteScoutRecord> scoutedRoutes = new List<RouteScoutRecord>();
        public int lastProcessedDay = -1;
    }

    [Serializable]
    public sealed class ActiveReconMissionState
    {
        public string missionId = string.Empty;
        public string platformId = string.Empty;
        public string targetSectorId = string.Empty;
        public float linkQuality = 1f;
        public float batteryRemaining = 0f;
        public int intelligencePoints = 0;
        public string status = "prepared"; // prepared, launched, surveying, returning, recovered, lost, crashed
        public int launchDay = 0;
        public int expectedRecoveryDay = 0;
        public List<string> surveyedSectorIds = new List<string>();
        public string lossReason = string.Empty;
    }

    [Serializable]
    public sealed class ReconForecastRecord
    {
        public string forecastId = string.Empty;
        public string platformId = string.Empty;
        public int generatedDay = 0;
        public int expiryDay = 0;
        public string frontType = string.Empty;
        public float confidence = 0f;
        public List<string> affectedSectorIds = new List<string>();
    }

    [Serializable]
    public sealed class RouteScoutRecord
    {
        public string routeId = string.Empty;
        public string missionId = string.Empty;
        public float speedMultiplier = 0.75f;
        public int expiryDay = 0;
        public bool isActive = false;
    }
}
