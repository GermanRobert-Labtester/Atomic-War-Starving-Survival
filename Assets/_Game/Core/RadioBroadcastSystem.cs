using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Data;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Drives radio broadcast playback based on the current day. Picks the first
    /// broadcast whose day window matches, plays it once, and advances. Wired to
    /// the RadioModuleSO in the shelter.
    /// </summary>
    public class RadioBroadcastSystem
    {
        private readonly List<RadioBroadcastSO> _broadcasts = new List<RadioBroadcastSO>();
        private readonly HashSet<string> _played = new HashSet<string>();
        private RadioBroadcastSO _currentBroadcast;

        public event Action<RadioBroadcastSO> OnBroadcastStarted;
        public event Action<RadioBroadcastSO> OnBroadcastEnded;

        public RadioBroadcastSO CurrentBroadcast => _currentBroadcast;

        public void SetCatalog(RadioCatalogSO catalog)
        {
            _broadcasts.Clear();
            if (catalog != null && catalog.broadcasts != null)
            {
                _broadcasts.AddRange(catalog.broadcasts);
            }
        }

        /// <summary>Check for new broadcasts based on current day. Call once per day tick.</summary>
        public void CheckForBroadcast(int currentDay)
        {
            for (int i = 0; i < _broadcasts.Count; i++)
            {
                var bc = _broadcasts[i];
                if (bc == null || _played.Contains(bc.id)) continue;

                if (currentDay >= bc.minDay && (bc.maxDay < 0 || currentDay <= bc.maxDay))
                {
                    PlayBroadcast(bc);
                    return;
                }
            }
        }

        private void PlayBroadcast(RadioBroadcastSO broadcast)
        {
            _currentBroadcast = broadcast;
            _played.Add(broadcast.id);
            OnBroadcastStarted?.Invoke(broadcast);
            Debug.Log($"[Radio] Broadcast: {broadcast.message}");
        }

        public void StopBroadcast()
        {
            if (_currentBroadcast != null)
            {
                OnBroadcastEnded?.Invoke(_currentBroadcast);
                _currentBroadcast = null;
            }
        }

        /// <summary>Reset played state (for new game or load).</summary>
        public void Reset()
        {
            _played.Clear();
            _currentBroadcast = null;
        }

        // -----------------------------------------------------------------
        // Save / Load (audit wiring fix)
        // -----------------------------------------------------------------
        public RadioSave CaptureState()
        {
            var ids = new string[_played.Count];
            _played.CopyTo(ids);
            return new RadioSave { PlayedBroadcastIds = ids };
        }

        public void RestoreState(RadioSave save)
        {
            _played.Clear();
            _currentBroadcast = null;
            if (save?.PlayedBroadcastIds == null) return;
            for (int i = 0; i < save.PlayedBroadcastIds.Length; i++)
                if (!string.IsNullOrEmpty(save.PlayedBroadcastIds[i]))
                    _played.Add(save.PlayedBroadcastIds[i]);
        }
    }

    [Serializable]
    public class RadioSave
    {
        public string[] PlayedBroadcastIds;
    }
}
