using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// Wildlife Migration System (#57) — irradiated fauna (ash crows,
    /// feral pack dogs) migrate according to weather patterns and fallout
    /// storm paths, altering danger ratings along expedition routes.
    ///
    /// Plain C#, save-safe.
    /// </summary>
    public class WildlifeMigrationSystem
    {
        public enum WildlifeActivity { None, Low, Medium, High, Swarm }

        [Serializable]
        public class ZoneDanger
        {
            public string ZoneId;
            public WildlifeActivity CurrentActivity;
            public float DangerModifier; // multiplier on expedition encounter chance
            public int DaysUntilChange;
        }

        public event Action<string, WildlifeActivity> OnZoneDangerChanged;
        public event Action<string> OnSwarmWarning;

        private readonly Dictionary<string, ZoneDanger> _zones =
            new Dictionary<string, ZoneDanger>();
        private int _lastMigrationDay = -1;

        public void RegisterZone(string zoneId)
        {
            if (!_zones.ContainsKey(zoneId))
            {
                _zones[zoneId] = new ZoneDanger
                {
                    ZoneId = zoneId,
                    CurrentActivity = WildlifeActivity.Low,
                    DangerModifier = 1f,
                    DaysUntilChange = 5
                };
            }
        }

        public float GetZoneDangerModifier(string zoneId)
        {
            return _zones.TryGetValue(zoneId, out var zone)
                ? zone.DangerModifier : 1f;
        }

        public void Tick(int currentDay, string currentWeatherId,
            bool isFalloutStormActive, System.Random rng)
        {
            if (currentDay - _lastMigrationDay < 3) return;
            _lastMigrationDay = currentDay;

            foreach (var kv in _zones)
            {
                var zone = kv.Value;
                zone.DaysUntilChange--;

                if (zone.DaysUntilChange <= 0)
                {
                    float roll = (float)(rng?.NextDouble() ?? 0.5);
                    WildlifeActivity oldActivity = zone.CurrentActivity;

                    if (isFalloutStormActive)
                    {
                        // Storms drive wildlife toward shelter
                        zone.CurrentActivity = roll < 0.6f
                            ? WildlifeActivity.High : WildlifeActivity.Swarm;
                    }
                    else if (currentWeatherId == "clear" || currentWeatherId == "mild")
                    {
                        zone.CurrentActivity = roll < 0.5f
                            ? WildlifeActivity.Low : WildlifeActivity.Medium;
                    }
                    else
                    {
                        zone.CurrentActivity = roll < 0.4f
                            ? WildlifeActivity.Low : WildlifeActivity.Medium;
                    }

                    zone.DangerModifier = zone.CurrentActivity switch
                    {
                        WildlifeActivity.None => 0.5f,
                        WildlifeActivity.Low => 1.0f,
                        WildlifeActivity.Medium => 1.5f,
                        WildlifeActivity.High => 2.0f,
                        WildlifeActivity.Swarm => 3.0f,
                        _ => 1.0f
                    };

                    zone.DaysUntilChange = 3 + (rng?.Next(5) ?? 2);

                    if (oldActivity != zone.CurrentActivity)
                        OnZoneDangerChanged?.Invoke(zone.ZoneId,
                            zone.CurrentActivity);

                    if (zone.CurrentActivity == WildlifeActivity.Swarm)
                        OnSwarmWarning?.Invoke(zone.ZoneId);
                }
            }
        }
    }
}
