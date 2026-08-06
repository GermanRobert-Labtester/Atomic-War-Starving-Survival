using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class GraveSiteData
    {
        public string locationId;
        public int originalSeed;
        public int dayOfDeath;
        public List<string> deadSurvivorNames = new List<string>();
        public List<string> diaryEntries = new List<string>();
        public List<string> remainingLootIds = new List<string>();
        public string causeOfDeath = "";
    }

    [Serializable]
    public class LastWillSave
    {
        public GraveSiteData currentGraveSite;
        public bool hasGraveSite;
    }

    /// <summary>
    /// Prompt #563: System: The Last Will (Rogue-lite Integration).
    /// If the player loses (all survivors die), the Save File is converted into a
    /// GraveSiteData record. In the next playthrough, the player can discover their
    /// old bunker with dead survivors' corpses, diaries, and remaining loot.
    /// </summary>
    public class LastWillSystem
    {
        private GraveSiteData _currentGraveSite;
        private bool _hasGraveSite;

        public event Action<GraveSiteData> OnGraveSiteGenerated;
        public event Action<string, GraveSiteData> OnGraveSiteDiscovered; // (visitingSurvivorId, site)

        public bool HasGraveSite => _hasGraveSite;
        public GraveSiteData CurrentGraveSite => _currentGraveSite;

        /// <summary>
        /// Convert a completed (lost) save into a grave site record for future
        /// playthroughs. Call this when all survivors have died.
        /// </summary>
        public void GenerateGraveSite(
            string locationId,
            int originalSeed,
            int dayOfDeath,
            List<string> deadSurvivorNames,
            List<string> diaryEntries,
            List<string> remainingLootIds,
            string causeOfDeath)
        {
            _currentGraveSite = new GraveSiteData
            {
                locationId = locationId ?? "grave_unknown",
                originalSeed = originalSeed,
                dayOfDeath = dayOfDeath,
                causeOfDeath = causeOfDeath ?? "unknown"
            };

            if (deadSurvivorNames != null)
            {
                for (int i = 0; i < deadSurvivorNames.Count; i++)
                    _currentGraveSite.deadSurvivorNames.Add(deadSurvivorNames[i]);
            }

            if (diaryEntries != null)
            {
                for (int i = 0; i < diaryEntries.Count; i++)
                    _currentGraveSite.diaryEntries.Add(diaryEntries[i]);
            }

            if (remainingLootIds != null)
            {
                for (int i = 0; i < remainingLootIds.Count; i++)
                    _currentGraveSite.remainingLootIds.Add(remainingLootIds[i]);
            }

            _hasGraveSite = true;
            OnGraveSiteGenerated?.Invoke(_currentGraveSite);
        }

        /// <summary>
        /// Returns the grave site location data for map-node discovery integration.
        /// Returns null if no grave site exists.
        /// </summary>
        public GraveSiteData GetGraveSiteLocationNode()
        {
            if (!_hasGraveSite) return null;
            return _currentGraveSite;
        }

        /// <summary>
        /// Attempt to discover the grave site. Returns the grave data if the node
        /// matches; otherwise returns null.
        /// </summary>
        public GraveSiteData TryDiscoverGrave(string visitingSurvivorId, string nodeId)
        {
            if (!_hasGraveSite || _currentGraveSite == null) return null;
            if (string.IsNullOrEmpty(nodeId) || string.IsNullOrEmpty(visitingSurvivorId)) return null;

            if (string.Equals(_currentGraveSite.locationId, nodeId, StringComparison.Ordinal))
            {
                OnGraveSiteDiscovered?.Invoke(visitingSurvivorId, _currentGraveSite);
                return _currentGraveSite;
            }

            return null;
        }

        /// <summary>
        /// Generate player-facing narrative text describing the discovered grave site.
        /// </summary>
        public string GetNarrativeText(GraveSiteData site)
        {
            if (site == null) return "";

            var sb = new StringBuilder();
            sb.Append("You found the remains of a previous group. ");
            sb.Append("They survived ");
            sb.Append(site.dayOfDeath);
            sb.Append(" days before ");
            sb.Append(site.causeOfDeath);
            sb.Append(" claimed them.");

            if (site.deadSurvivorNames.Count > 0)
            {
                sb.Append(" Among the dead: ");
                for (int i = 0; i < site.deadSurvivorNames.Count; i++)
                {
                    if (i > 0) sb.Append(i == site.deadSurvivorNames.Count - 1 ? ", and " : ", ");
                    sb.Append(site.deadSurvivorNames[i]);
                }
                sb.Append(".");
            }

            if (site.diaryEntries.Count > 0)
            {
                sb.Append(" A tattered diary reads: \"");
                sb.Append(site.diaryEntries[0]);
                sb.Append("\"");
            }

            if (site.remainingLootIds.Count > 0)
            {
                sb.Append(" Some supplies were left behind.");
            }

            return sb.ToString();
        }

        public LastWillSave CaptureState()
        {
            var save = new LastWillSave
            {
                hasGraveSite = _hasGraveSite,
                currentGraveSite = _hasGraveSite && _currentGraveSite != null
                    ? new GraveSiteData
                    {
                        locationId = _currentGraveSite.locationId,
                        originalSeed = _currentGraveSite.originalSeed,
                        dayOfDeath = _currentGraveSite.dayOfDeath,
                        causeOfDeath = _currentGraveSite.causeOfDeath,
                        deadSurvivorNames = new List<string>(_currentGraveSite.deadSurvivorNames),
                        diaryEntries = new List<string>(_currentGraveSite.diaryEntries),
                        remainingLootIds = new List<string>(_currentGraveSite.remainingLootIds)
                    }
                    : null
            };
            return save;
        }

        public void RestoreState(LastWillSave save)
        {
            if (save == null || !save.hasGraveSite || save.currentGraveSite == null)
            {
                _hasGraveSite = false;
                _currentGraveSite = null;
                return;
            }

            _hasGraveSite = true;
            _currentGraveSite = new GraveSiteData
            {
                locationId = save.currentGraveSite.locationId,
                originalSeed = save.currentGraveSite.originalSeed,
                dayOfDeath = save.currentGraveSite.dayOfDeath,
                causeOfDeath = save.currentGraveSite.causeOfDeath,
                deadSurvivorNames = new List<string>(save.currentGraveSite.deadSurvivorNames),
                diaryEntries = new List<string>(save.currentGraveSite.diaryEntries),
                remainingLootIds = new List<string>(save.currentGraveSite.remainingLootIds)
            };
        }
    }
}
