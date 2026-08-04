using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Tracker System — Footprints in Ash (Prompt #71). When a survivor flees
    /// an encounter, there's a 20% chance they're tracked back to the bunker.
    /// The next day, an event fires: "Footprints in the ash leading to our hatch."
    /// Increases Raid chance to 90% for the tracking faction.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class TrackerSystem
    {
        /// <summary>Chance (0..1) of being tracked when fleeing an encounter.</summary>
        public const float TrackChanceOnFlee = 0.20f;

        /// <summary>Raid chance multiplier when tracked (90% = 0.9).</summary>
        public const float TrackedRaidChance = 0.90f;

        /// <summary>Event id for the "Footprints in the ash" narrative event.</summary>
        public const string TrackedEventId = "tracked_footprints_ash";

        /// <summary>Hours delay before the tracking event fires.</summary>
        public const float TrackedEventDelayHours = 24f;

        /// <summary>Tracking entry per faction.</summary>
        public class ActiveTrack
        {
            public string FactionId;
            public float HoursUntilEvent;
            public string TrackedSurvivorId;
        }

        private readonly List<ActiveTrack> _activeTracks = new List<ActiveTrack>();
        private readonly System.Random _rng;

        // -- Events --
        public event Action<ActiveTrack> OnTracked;    // survivor was followed
        public event Action<ActiveTrack> OnTrackEventFired; // delayed event triggered

        public IReadOnlyList<ActiveTrack> ActiveTracks => _activeTracks;

        public TrackerSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(71);
        }

        /// <summary>
        /// Roll for tracking after fleeing an encounter. Returns true if tracked.
        /// </summary>
        public bool TryTrackAfterFlee(string factionId, string survivorId)
        {
            if (string.IsNullOrEmpty(factionId)) return false;
            if (_rng.NextDouble() >= TrackChanceOnFlee) return false;

            var track = new ActiveTrack
            {
                FactionId = factionId,
                HoursUntilEvent = TrackedEventDelayHours,
                TrackedSurvivorId = survivorId ?? string.Empty
            };
            _activeTracks.Add(track);
            OnTracked?.Invoke(track);
            return true;
        }

        /// <summary>
        /// Tick tracking timers. When a timer expires, fire the "Footprints in ash"
        /// event and boost raid chance for that faction.
        /// </summary>
        public void Tick(float gameHours, Action<string, float> setFactionRaidChance = null,
            Action<string, int, string> scheduleEvent = null, int currentDay = 1)
        {
            if (gameHours <= 0f || _activeTracks.Count == 0) return;

            for (int i = _activeTracks.Count - 1; i >= 0; i--)
            {
                var track = _activeTracks[i];
                track.HoursUntilEvent -= gameHours;

                if (track.HoursUntilEvent <= 0f)
                {
                    // Boost raid chance for this faction to 90%.
                    setFactionRaidChance?.Invoke(track.FactionId, TrackedRaidChance);

                    // Fire the narrative event.
                    scheduleEvent?.Invoke(TrackedEventId, currentDay + 1, track.FactionId);

                    OnTrackEventFired?.Invoke(track);
                    _activeTracks.RemoveAt(i);
                }
            }
        }

        /// <summary>Whether any active tracks are pending for a faction.</summary>
        public bool IsTracked(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return false;
            for (int i = 0; i < _activeTracks.Count; i++)
                if (_activeTracks[i].FactionId == factionId) return true;
            return false;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public TrackerSave CaptureState()
        {
            var tracks = new TrackedEntrySave[_activeTracks.Count];
            for (int i = 0; i < _activeTracks.Count; i++)
            {
                tracks[i] = new TrackedEntrySave
                {
                    FactionId = _activeTracks[i].FactionId,
                    HoursUntilEvent = _activeTracks[i].HoursUntilEvent,
                    TrackedSurvivorId = _activeTracks[i].TrackedSurvivorId
                };
            }
            return new TrackerSave { Tracks = tracks };
        }

        public void RestoreState(TrackerSave save)
        {
            _activeTracks.Clear();
            if (save?.Tracks == null) return;
            for (int i = 0; i < save.Tracks.Length; i++)
            {
                var t = save.Tracks[i];
                if (t == null || string.IsNullOrEmpty(t.FactionId)) continue;
                _activeTracks.Add(new ActiveTrack
                {
                    FactionId = t.FactionId,
                    HoursUntilEvent = t.HoursUntilEvent,
                    TrackedSurvivorId = t.TrackedSurvivorId ?? string.Empty
                });
            }
        }
    }

    [Serializable]
    public class TrackerSave
    {
        public TrackedEntrySave[] Tracks;
    }

    [Serializable]
    public class TrackedEntrySave
    {
        public string FactionId;
        public float HoursUntilEvent;
        public string TrackedSurvivorId;
    }
}
