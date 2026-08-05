using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Excavation System (Prompt #119). Unused rooms start filled with Debris.
    /// Survivors clear rubble with Fatigue cost, requiring a Shovel. Rubble must
    /// be carried to the surface hatch. If the hatch is blocked (#48/#79),
    /// excavation is impossible. Shelter expansion as a logistical loop.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class ExcavationSystem
    {
        public const string ShovelItemId = "shovel";

        /// <summary>Hours to clear one unit of rubble (base, before tool/skill).</summary>
        public const float BaseClearHoursPerUnit = 4f;

        /// <summary>Fatigue cost per hour of clearing rubble.</summary>
        public const float FatiguePerHour = 8f;

        /// <summary>Rubble units that can be stored before requiring a surface dump run.</summary>
        public const int MaxRubblePileUnits = 10;

        /// <summary>Hours to carry one rubble dump to the surface.</summary>
        public const float SurfaceDumpHours = 2f;

        /// <summary>Fatigue cost for a surface dump run.</summary>
        public const float SurfaceDumpFatigue = 12f;

        /// <summary>Room debris state per room.</summary>
        public class RoomExcavation
        {
            public string RoomId;
            public float RubbleUnitsRemaining;
            public float RubbleUnitsTotal;
            public bool IsCleared;
        }

        private readonly Dictionary<string, RoomExcavation> _rooms = new Dictionary<string, RoomExcavation>();
        private int _pendingRubbleUnits; // Rubble waiting at hatch for surface dump
        private readonly System.Random _rng;
        private ShelterPerkSystem _shelterPerks;
        private PersonalQuestSystem _personalQuests;
        private Func<int> _getDay;

        // -- Events --
        public event Action<string> OnRoomCleared;
        public event Action<int> OnRubbleDumped; // units dumped to surface
        public event Action OnRubblePileFull;
        /// <summary>Prompt #199 — cave-in mini-event while digging (roomId).</summary>
        public event Action<string> OnCaveInWhileDigging;

        public IReadOnlyDictionary<string, RoomExcavation> Rooms => _rooms;
        public int PendingRubbleUnits => _pendingRubbleUnits;
        public bool NeedsSurfaceDump => _pendingRubbleUnits >= MaxRubblePileUnits;

        public ExcavationSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(119);
        }

        /// <summary>Prompt #199 — Sandhog fatigue / cave-in immunity.</summary>
        public void BindShelterPerks(ShelterPerkSystem perks, Func<int> getDay = null)
        {
            _shelterPerks = perks;
            _getDay = getDay ?? (() => 0);
        }

        /// <summary>Register a room with rubble to clear.</summary>
        public void SealRoom(string roomId, float rubbleUnits)
        {
            if (string.IsNullOrEmpty(roomId) || rubbleUnits <= 0f) return;
            _rooms[roomId] = new RoomExcavation
            {
                RoomId = roomId,
                RubbleUnitsRemaining = rubbleUnits,
                RubbleUnitsTotal = rubbleUnits
            };
        }

        /// <summary>Whether a room has rubble remaining.</summary>
        public bool HasRubble(string roomId) =>
            _rooms.TryGetValue(roomId, out var r) && !r.IsCleared && r.RubbleUnitsRemaining > 0f;

        /// <summary>True if any registered room still has rubble (used by AI to score actions).</summary>
        public bool HasAnyRubble()
        {
            foreach (var kv in _rooms)
            {
                var r = kv.Value;
                if (r != null && !r.IsCleared && r.RubbleUnitsRemaining > 0f) return true;
            }
            return false;
        }

        /// <summary>
        /// Clear rubble for <paramref name="workHours"/> of work (default 1).
        /// Returns units cleared. Requires Shovel for full speed.
        /// Prompt #213 — Taskmaster passes workHours &gt; 1 for Pacing Aura.
        /// </summary>
        /// <summary>Prompt #227 — Vault Builder room build fatigue/materials.</summary>
        public void BindPersonalQuests(PersonalQuestSystem personalQuests) =>
            _personalQuests = personalQuests;

        public float ClearRubble(string roomId, Survivors.Survivor worker,
            bool hasShovel, bool hatchBlocked, float workHours = 1f)
        {
            // #314 Amputee/Cyber-Arm: punches through a blocked hatch instead of stalling.
            bool cyberArmBypass = hatchBlocked && _personalQuests != null
                && _personalQuests.CanPunchThroughBlockedDoors(worker);
            if (hatchBlocked && !cyberArmBypass) return 0f; // Can't dump rubble if hatch is blocked.
            if (worker == null || !worker.IsAlive) return 0f;
            if (!HasRubble(roomId)) return 0f;
            if (workHours <= 0f) return 0f;

            var room = _rooms[roomId];

            // #251 Wasteland Scout: crawl through debris instantly (full clear, no fatigue).
            if (_personalQuests != null && _personalQuests.CanCrawlDebrisInstantly(worker))
            {
                float instant = room.RubbleUnitsRemaining;
                room.RubbleUnitsRemaining = 0f;
                _pendingRubbleUnits = Mathf.Min(
                    _pendingRubbleUnits + Mathf.CeilToInt(instant), MaxRubblePileUnits * 2);
                if (!room.IsCleared)
                {
                    room.IsCleared = true;
                    _shelterPerks?.RecordRoomCleared(worker, _getDay?.Invoke() ?? 0);
                    OnRoomCleared?.Invoke(roomId);
                }
                if (NeedsSurfaceDump) OnRubblePileFull?.Invoke();
                return instant;
            }

            float shovelMult = hasShovel ? 1.5f : 0.5f;
            float skillMult = 1f + worker.EffectiveCraftingSkill * 0.5f;
            // #284 Deep Delver: excavating rubble takes 75% less time (0.25× duration → 4× rate).
            float delverMult = 1f;
            if (_personalQuests != null)
            {
                float durationMult = _personalQuests.GetDeepDelverExcavationDurationMultiplier(worker);
                if (durationMult > 0f && durationMult < 1f)
                    delverMult = 1f / durationMult;
            }
            float cleared = (workHours / BaseClearHoursPerUnit) * shovelMult * skillMult * delverMult;
            cleared = Mathf.Min(cleared, room.RubbleUnitsRemaining);

            room.RubbleUnitsRemaining -= cleared;
            _pendingRubbleUnits = Mathf.Min(_pendingRubbleUnits + Mathf.CeilToInt(cleared), MaxRubblePileUnits * 2);

            float fatMult = _shelterPerks != null
                ? _shelterPerks.GetExcavationFatigueMultiplier(worker)
                : 1f;
            // Prompt #227 — Vault Builder: 50% less fatigue when excavating/building rooms.
            if (_personalQuests != null)
                fatMult *= _personalQuests.GetRoomBuildCostMultiplier(worker);
            worker.Needs.Fatigue = Mathf.Clamp(
                worker.Needs.Fatigue + FatiguePerHour * workHours * fatMult, 0f, 100f);

            // Prompt #199 — cave-in mini-event while digging (Sandhog never triggers).
            if (_shelterPerks != null
                && _shelterPerks.RollDigCaveIn(worker, _rng))
            {
                OnCaveInWhileDigging?.Invoke(roomId);
            }
            else if (_shelterPerks == null
                     && _rng.NextDouble() < ShelterPerkSystem.BaseExcavationCaveInChance)
            {
                OnCaveInWhileDigging?.Invoke(roomId);
            }

            if (room.RubbleUnitsRemaining <= 0f && !room.IsCleared)
            {
                room.IsCleared = true;
                _shelterPerks?.RecordRoomCleared(worker, _getDay?.Invoke() ?? 0);
                OnRoomCleared?.Invoke(roomId);
            }

            if (NeedsSurfaceDump) OnRubblePileFull?.Invoke();
            return cleared;
        }

        /// <summary>Carry accumulated rubble to the surface. Costs time and fatigue.</summary>
        public int DumpRubbleToSurface(Survivors.Survivor worker)
        {
            if (worker == null || !worker.IsAlive || _pendingRubbleUnits <= 0) return 0;
            int dumped = Mathf.Min(_pendingRubbleUnits, MaxRubblePileUnits);
            _pendingRubbleUnits -= dumped;
            worker.Needs.Fatigue = Mathf.Clamp(worker.Needs.Fatigue + SurfaceDumpFatigue, 0f, 100f);
            OnRubbleDumped?.Invoke(dumped);
            return dumped;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------
        public ExcavationSave CaptureState()
        {
            var entries = new ExcavationRoomSave[_rooms.Count];
            int i = 0;
            foreach (var kv in _rooms)
            {
                entries[i++] = new ExcavationRoomSave
                {
                    RoomId = kv.Value.RoomId,
                    RubbleUnitsRemaining = kv.Value.RubbleUnitsRemaining,
                    RubbleUnitsTotal = kv.Value.RubbleUnitsTotal,
                    IsCleared = kv.Value.IsCleared
                };
            }
            return new ExcavationSave { Rooms = entries, PendingRubbleUnits = _pendingRubbleUnits };
        }

        public void RestoreState(ExcavationSave save)
        {
            _rooms.Clear();
            _pendingRubbleUnits = 0;
            if (save == null) return;
            _pendingRubbleUnits = save.PendingRubbleUnits;
            if (save.Rooms != null)
                for (int i = 0; i < save.Rooms.Length; i++)
                {
                    var e = save.Rooms[i];
                    if (e == null || string.IsNullOrEmpty(e.RoomId)) continue;
                    _rooms[e.RoomId] = new RoomExcavation
                    {
                        RoomId = e.RoomId,
                        RubbleUnitsRemaining = e.RubbleUnitsRemaining,
                        RubbleUnitsTotal = e.RubbleUnitsTotal,
                        IsCleared = e.IsCleared
                    };
                }
        }
    }

    [Serializable] public class ExcavationSave
    {
        public ExcavationRoomSave[] Rooms;
        public int PendingRubbleUnits;
    }
    [Serializable] public class ExcavationRoomSave
    {
        public string RoomId;
        public float RubbleUnitsRemaining;
        public float RubbleUnitsTotal;
        public bool IsCleared;
    }
}
