using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Events
{
    /// <summary>
    /// Expansion III — The Radio Dark. Multi-night audio puzzles that the player
    /// must actively decrypt. Each ghost station broadcasts on a specific frequency
    /// and requires specific survivors or items to decode.
    /// </summary>
    public class RadioDarkPuzzleSystem
    {
        // ── Station ids ───────────────────────────────────────────────
        public const string Station_BombersLighthouse = "ghost_station_bombers_lighthouse";
        public const string Station_DeadMansSwitch = "ghost_station_dead_mans_switch";
        public const string Station_ChildrensChorus = "ghost_station_childrens_chorus";

        // ── Frequencies ───────────────────────────────────────────────
        public const float Freq_BombersLighthouse = 114.5f;
        public const float Freq_DeadMansSwitch_Scatter1 = 87.3f;
        public const float Freq_DeadMansSwitch_Scatter2 = 91.7f;
        public const string Freq_ChildrensChorus = "99.0";

        // ── Decrypt requirements ──────────────────────────────────────
        public const string DecryptRole_Meteorologist = "the_meteorologist";
        public const string DecryptRole_RadioHost = "the_radio_host";
        public const string DecryptRole_TechBro = "the_tech_bro";

        // ── Puzzle states ─────────────────────────────────────────────
        public enum PuzzleState
        {
            Undiscovered,
            Listening,       // Signal detected, not yet decoded
            Decrypting,      // Player is working on the puzzle
            Solved,          // Puzzle completed
            Expired,         // Window closed or countdown reached zero
            ConsequenceApplied
        }

        // ── The Bomber's Lighthouse ───────────────────────────────────
        public const int LighthouseDisableDeadlineDay = 60;
        public const string Reward_RTGBattery = "rtg_battery";
        public const string Reward_PlutoniumCore = "plutonium_core";
        public const string Anomaly_UXONuke = "map_anomaly_uxo_nuke";

        // ── The Dead Man's Switch ─────────────────────────────────────
        public const string Anomaly_CollapsedBunker = "map_anomaly_collapsed_bunker";
        public const string Reward_MedicalCacheGuide = "medical_cache_guide";
        public const int DeadManBroadcastRange = 12; // hours walk to medical cache

        // ── The Children's Chorus ─────────────────────────────────────
        public const string RaidTarget_MilitiaGrainExchange = "location_militia_grain_exchange";
        public const string Reward_MilitiaFavor = "militia_favor_massive";

        // ── Events ────────────────────────────────────────────────────
        public event Action<string> OnStationDiscovered;         // stationId
        public event Action<string, PuzzleState> OnPuzzleStateChanged;
        public event Action<string> OnPuzzleSolved;              // stationId
        public event Action<string> OnSignalDecoded;             // stationId
        public event Action<string> OnConsequenceApplied;        // stationId
        public event Action<string> OnCountdownWarning;          // stationId

        private readonly System.Random _rng;
        private readonly Dictionary<string, GhostStationState> _stations = new Dictionary<string, GhostStationState>();
        private int _currentDay;

        public RadioDarkPuzzleSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(5000);
            InitializeStations();
        }

        private void InitializeStations()
        {
            _stations[Station_BombersLighthouse] = new GhostStationState
            {
                StationId = Station_BombersLighthouse,
                Frequency = Freq_BombersLighthouse,
                State = PuzzleState.Undiscovered,
                DeadlineDay = LighthouseDisableDeadlineDay,
                Description = "A looping automated beacon meant to guide high-altitude bombers. " +
                    "Long and short tones. The coordinates point to something buried.",
                DecryptRequirement = DecryptRole_Meteorologist + "|" + DecryptRole_RadioHost,
                RewardIds = new List<string> { Reward_RTGBattery, Reward_PlutoniumCore },
                AnomalyId = Anomaly_UXONuke
            };

            _stations[Station_DeadMansSwitch] = new GhostStationState
            {
                StationId = Station_DeadMansSwitch,
                Frequency = 0f, // Scattered across bands
                State = PuzzleState.Undiscovered,
                Description = "A man's breathing. Occasionally, a voice reads numbers. " +
                    "The numbers are a countdown. Someone is dying.",
                DecryptRequirement = "action_decrypt",
                RewardIds = new List<string> { Anomaly_CollapsedBunker },
                AnomalyId = null
            };

            _stations[Station_ChildrensChorus] = new GhostStationState
            {
                StationId = Station_ChildrensChorus,
                Frequency = 99f,
                State = PuzzleState.Undiscovered,
                Description = "A school choir singing a folk song, cut off by a siren. " +
                    "Then a child's voice reads the cipher. The feral packs are communicating.",
                DecryptRequirement = DecryptRole_RadioHost,
                RewardIds = new List<string> { Reward_MilitiaFavor },
                AnomalyId = null
            };
        }

        /// <summary>Update the current campaign day (called by host).</summary>
        public void SetCurrentDay(int day)
        {
            _currentDay = day;
            CheckDeadlines();
        }

        /// <summary>
        /// Discover a ghost station by tuning to its frequency.
        /// </summary>
        public bool DiscoverStation(string stationId)
        {
            if (!_stations.TryGetValue(stationId, out var station)) return false;
            if (station.State != PuzzleState.Undiscovered) return false;

            station.State = PuzzleState.Listening;
            OnStationDiscovered?.Invoke(stationId);
            OnPuzzleStateChanged?.Invoke(stationId, PuzzleState.Listening);
            return true;
        }

        /// <summary>
        /// Begin decryption of a discovered station. Requires specific survivor role.
        /// </summary>
        public bool BeginDecryption(string stationId, string survivorRole)
        {
            if (!_stations.TryGetValue(stationId, out var station)) return false;
            if (station.State != PuzzleState.Listening) return false;

            // Check decrypt requirement
            if (!string.IsNullOrEmpty(station.DecryptRequirement))
            {
                if (!station.DecryptRequirement.Contains(survivorRole))
                    return false;
            }

            station.State = PuzzleState.Decrypting;
            station.DecryptProgress = 0f;
            OnPuzzleStateChanged?.Invoke(stationId, PuzzleState.Decrypting);
            return true;
        }

        /// <summary>
        /// Advance decryption progress. Returns true when puzzle is solved.
        /// </summary>
        public bool AdvanceDecryption(string stationId, float hoursWorked)
        {
            if (!_stations.TryGetValue(stationId, out var station)) return false;
            if (station.State != PuzzleState.Decrypting) return false;

            station.DecryptProgress += hoursWorked;

            // Each puzzle requires a different amount of work
            float required = station.StationId switch
            {
                Station_BombersLighthouse => 8f,
                Station_DeadMansSwitch => 6f,
                Station_ChildrensChorus => 4f,
                _ => 6f
            };

            if (station.DecryptProgress >= required)
            {
                station.State = PuzzleState.Solved;
                OnPuzzleSolved?.Invoke(stationId);
                OnPuzzleStateChanged?.Invoke(stationId, PuzzleState.Solved);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Apply the consequence of a solved puzzle.
        /// </summary>
        public bool ApplyConsequence(string stationId, string choice = null)
        {
            if (!_stations.TryGetValue(stationId, out var station)) return false;
            if (station.State != PuzzleState.Solved) return false;

            station.State = PuzzleState.ConsequenceApplied;
            station.ChoiceMade = choice;
            OnConsequenceApplied?.Invoke(stationId);
            OnPuzzleStateChanged?.Invoke(stationId, PuzzleState.ConsequenceApplied);
            return true;
        }

        /// <summary>
        /// For Dead Man's Switch: choose to ignore (let timer expire) or
        /// broadcast abort code (save the stranger).
        /// </summary>
        public enum DeadManChoice
        {
            Ignore,
            BroadcastAbort
        }

        public DeadManChoice? GetDeadManChoice()
        {
            if (!_stations.TryGetValue(Station_DeadMansSwitch, out var station)) return null;
            if (station.ChoiceMade == "ignore") return DeadManChoice.Ignore;
            if (station.ChoiceMade == "broadcast_abort") return DeadManChoice.BroadcastAbort;
            return null;
        }

        /// <summary>
        /// For Children's Chorus: choose to tip off militia (reward) or
        /// let the raid happen (scavenge aftermath).
        /// </summary>
        public enum ChorusChoice
        {
            TipOffMilitia,
            LetRaidHappen
        }

        public ChorusChoice? GetChorusChoice()
        {
            if (!_stations.TryGetValue(Station_ChildrensChorus, out var station)) return null;
            if (station.ChoiceMade == "tip_off") return ChorusChoice.TipOffMilitia;
            if (station.ChoiceMade == "let_raid") return ChorusChoice.LetRaidHappen;
            return null;
        }

        /// <summary>Get the state of a specific station.</summary>
        public GhostStationState GetStation(string stationId)
        {
            return _stations.TryGetValue(stationId, out var s) ? s : null;
        }

        /// <summary>Get all stations.</summary>
        public IReadOnlyDictionary<string, GhostStationState> AllStations => _stations;

        // ── Deadline checks ───────────────────────────────────────────

        private void CheckDeadlines()
        {
            // Bomber's Lighthouse: if not disabled by day 60, artillery targets bunker
            if (_stations.TryGetValue(Station_BombersLighthouse, out var lighthouse))
            {
                if (lighthouse.State != PuzzleState.Solved
                    && lighthouse.State != PuzzleState.ConsequenceApplied
                    && _currentDay >= LighthouseDisableDeadlineDay)
                {
                    lighthouse.State = PuzzleState.Expired;
                    OnPuzzleStateChanged?.Invoke(Station_BombersLighthouse, PuzzleState.Expired);
                    OnConsequenceApplied?.Invoke(Station_BombersLighthouse);
                    // Host must trigger Siege_Artillery event
                }
                else if (lighthouse.State == PuzzleState.Listening
                         && _currentDay >= LighthouseDisableDeadlineDay - 10)
                {
                    OnCountdownWarning?.Invoke(Station_BombersLighthouse);
                }
            }
        }

        /// <summary>
        /// Check if the lighthouse artillery should fire.
        /// Called by host on Day 60+.
        /// </summary>
        public bool ShouldArtilleryFire()
        {
            if (!_stations.TryGetValue(Station_BombersLighthouse, out var s)) return false;
            return s.State == PuzzleState.Expired && _currentDay >= LighthouseDisableDeadlineDay;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public GhostStationSave CaptureState()
        {
            var entries = new GhostStationStateSave[_stations.Count];
            int i = 0;
            foreach (var kv in _stations)
            {
                var s = kv.Value;
                entries[i++] = new GhostStationStateSave
                {
                    StationId = s.StationId,
                    State = s.State,
                    DecryptProgress = s.DecryptProgress,
                    ChoiceMade = s.ChoiceMade,
                    DiscoveredDay = s.DiscoveredDay
                };
            }
            return new GhostStationSave { Stations = entries, CurrentDay = _currentDay };
        }

        public void RestoreState(GhostStationSave save)
        {
            if (save == null) return;
            _currentDay = save.CurrentDay;
            if (save.Stations == null) return;
            for (int i = 0; i < save.Stations.Length; i++)
            {
                var e = save.Stations[i];
                if (e == null || string.IsNullOrEmpty(e.StationId)) continue;
                if (_stations.TryGetValue(e.StationId, out var station))
                {
                    station.State = e.State;
                    station.DecryptProgress = e.DecryptProgress;
                    station.ChoiceMade = e.ChoiceMade;
                    station.DiscoveredDay = e.DiscoveredDay;
                }
            }
        }
    }

    [Serializable]
    public class GhostStationState
    {
        public string StationId;
        public float Frequency;
        public RadioDarkPuzzleSystem.PuzzleState State;
        public string Description;
        public string DecryptRequirement;
        public float DecryptProgress;
        public int DeadlineDay;
        public List<string> RewardIds;
        public string AnomalyId;
        public string ChoiceMade;
        public int DiscoveredDay;
    }

    [Serializable]
    public class GhostStationSave
    {
        public GhostStationStateSave[] Stations;
        public int CurrentDay;
    }

    [Serializable]
    public class GhostStationStateSave
    {
        public string StationId;
        public RadioDarkPuzzleSystem.PuzzleState State;
        public float DecryptProgress;
        public string ChoiceMade;
        public int DiscoveredDay;
    }
}
