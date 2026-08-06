using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Room Flooding & Sump Pumps (Prompt #120). During heavy Rain (pre-Day 30),
    /// lowest rooms flood. Flooded rooms disable electronics and ruin ground
    /// items. Craft a SumpPump (powered) or manually bucket water out.
    /// Save/load safe.
    /// </summary>
    public class RoomFloodingSystem
    {
        public const string SumpPumpModuleId = "sump_pump";
        /// <summary>Per-24h-rain-period chance a lowest room floods (~62% = 1-(1-0.04)^24).</summary>
        public const float FloodChancePer24hRain = 0.62f;
        public const float BucketClearHoursPerUnit = 1f;
        public const float BucketFatiguePerHour = 10f;

        private readonly HashSet<string> _floodedRooms = new HashSet<string>();
        private float _floodAccumulator;
        private System.Random _rng;

        public IReadOnlyCollection<string> FloodedRooms => _floodedRooms;
        public bool IsFlooded(string roomId) => _floodedRooms.Contains(roomId);

        public event Action<string> OnRoomFlooded;
        public event Action<string> OnRoomDrained;

        /// <summary>Inject a seeded RNG for deterministic save/load replay (audit bugfix #1).</summary>
        public void SetRng(System.Random rng) => _rng = rng ?? new System.Random(120);

        public void Tick(float gameHours, bool isRaining, bool preDay30,
            Shelter shelter, Func<string, bool> isLowestRoom)
        {
            if (!preDay30 || !isRaining) { _floodAccumulator = 0f; return; }
            _floodAccumulator += gameHours;
            if (_floodAccumulator < 24f) return;

            // Consume one 24h period; remainder stays in accumulator for
            // correctness under large substeps (3× fast-forward, catch-up).
            _floodAccumulator -= 24f;

            var candidates = new List<string>();
            if (shelter?.Rooms != null)
                for (int i = 0; i < shelter.Rooms.Count; i++)
                {
                    var r = shelter.Rooms[i];
                    if (r != null && isLowestRoom(r.RoomId) && !_floodedRooms.Contains(r.RoomId))
                        candidates.Add(r.RoomId);
                }

            if (candidates.Count > 0)
            {
                // Use seeded System.Random so save/load replays are deterministic.
                var rng = _rng ?? new System.Random(120);
                if (rng.NextDouble() < FloodChancePer24hRain)
                {
                    string flooded = candidates[rng.Next(candidates.Count)];
                    _floodedRooms.Add(flooded);
                    OnRoomFlooded?.Invoke(flooded);
                }
            }
        }

        public bool DrainRoom(string roomId, Survivors.Survivor worker, bool hasSumpPump)
        {
            if (!_floodedRooms.Contains(roomId)) return false;
            if (worker == null || !worker.IsAlive) return false;
            if (hasSumpPump) { _floodedRooms.Remove(roomId); OnRoomDrained?.Invoke(roomId); return true; }
            worker.Needs.Fatigue = Mathf.Clamp(worker.Needs.Fatigue + BucketFatiguePerHour, 0f, 100f);
            _floodedRooms.Remove(roomId);
            OnRoomDrained?.Invoke(roomId);
            return true;
        }

        public FloodingSave CaptureState()
        {
            var ids = new string[_floodedRooms.Count]; _floodedRooms.CopyTo(ids);
            return new FloodingSave { FloodedRoomIds = ids, FloodAccumulator = _floodAccumulator };
        }
        public void RestoreState(FloodingSave save)
        {
            _floodedRooms.Clear(); _floodAccumulator = 0f;
            if (save == null) return;
            _floodAccumulator = save.FloodAccumulator;
            if (save.FloodedRoomIds != null)
                for (int i = 0; i < save.FloodedRoomIds.Length; i++)
                    if (!string.IsNullOrEmpty(save.FloodedRoomIds[i])) _floodedRooms.Add(save.FloodedRoomIds[i]);
        }
    }
    [Serializable] public class FloodingSave { public string[] FloodedRoomIds; public float FloodAccumulator; }
}
