using System;
using System.Collections.Generic;

namespace Ashfall.Core.World
{
    [Serializable]
    public sealed class WeatherHardeningState
    {
        public string systemId = "weather_hardening";
        public List<InstalledUpgrade> installedUpgrades = new List<InstalledUpgrade>();
        public List<ZoneHardeningState> zones = new List<ZoneHardeningState>();
        public float globalIntakeIce = 0f;
        public bool manifoldFrozen = false;
        public int lastProcessedDay = -1;
    }

    [Serializable]
    public sealed class InstalledUpgrade
    {
        public string upgradeId = string.Empty;
        public string zoneId = string.Empty;
        public int installDay = 0;
        public float condition = 100f;
    }

    [Serializable]
    public sealed class ZoneHardeningState
    {
        public string zoneId = string.Empty;
        public float insulationHealth = 100f;
        public float freezeRisk = 0f;
        public float pipeFreezeProgress = 0f;
        public bool auxiliaryHeatActive = false;
        public float auxiliaryHeatFuelRemaining = 0f;
        public int lastFreezeDay = -1;
    }
}
