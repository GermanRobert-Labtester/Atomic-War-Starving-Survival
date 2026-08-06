using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Shelter
{
    [Serializable]
    public class SubPenLayoutData
    {
        public string layoutId = "layout_sub_pen";
        public string layoutName = "The Flooded Sub-Pen";
        public int roomCount = 8;
        public List<string> submergedRoomIds = new List<string>
        {
            "storage_a",
            "storage_b",
            "utility_room",
            "lower_corridor"
        };
        public float waterSupplyBonus = 200f;
        public float humidityOverride = 1.0f;
        public float moldOverride = 1.0f;
        public bool requiresInternalBoat = true;
    }

    /// <summary>
    /// Prompt #576: Layout 11 — The Flooded Sub-Pen.
    /// Massive bunker start. Half the rooms are permanently submerged.
    /// Player must use a rowboat inside the bunker to access storage rooms.
    /// Massive base Water supply, but Humidity and Mold are permanently maxed
    /// out. Save/load safe. Plain C#.
    /// </summary>
    public class ShelterLayout_SubPen
    {
        private SubPenLayoutData _data = new SubPenLayoutData();
        private readonly HashSet<string> _submergedSet;
        private bool _initialized;

        // -- Events --
        public event Action<SubPenLayoutData> OnSubPenLayoutInitialized;
        public event Action<string> OnRoomAccessedByBoat;

        public SubPenLayoutData Data => _data;

        public ShelterLayout_SubPen()
        {
            _submergedSet = new HashSet<string>(StringComparer.Ordinal);
            SyncSubmergedSet();
        }

        /// <summary>
        /// Returns a ShelterMapSO-compatible layout definition with all rooms,
        /// traits, shielding, and anomalies configured for the Sub-Pen.
        /// </summary>
        public ShelterMapDefinition GetLayoutDefinition()
        {
            var def = new ShelterMapDefinition
            {
                LayoutId = _data.layoutId,
                LayoutName = _data.layoutName,
                RoomCount = _data.roomCount,
                RoomIds = new[]
                {
                    "command_center",
                    "living_quarters",
                    "hydroponics_bay",
                    "airlock",
                    "storage_a",
                    "storage_b",
                    "utility_room",
                    "lower_corridor"
                },
                RoomNames = new[]
                {
                    "Command Center",
                    "Living Quarters",
                    "Hydroponics Bay",
                    "Airlock",
                    "Storage A (Flooded)",
                    "Storage B (Flooded)",
                    "Utility Room (Flooded)",
                    "Lower Corridor (Flooded)"
                },
                RoomSizes = new[] { 3f, 2f, 2f, 1f, 2f, 2f, 1f, 1f },
                InherentShielding = 0.75f,
                StartingCleanWater = _data.waterSupplyBonus,
                StartingIntegrity = 80f,
                Traits = new[]
                {
                    ShelterLayoutTrait.Flooded,
                    ShelterLayoutTrait.DeepUnderground
                },
                Anomalies = new[] { "flooded_sub_pen", "internal_rowboat_required" }
            };

            if (!_initialized)
            {
                _initialized = true;
                OnSubPenLayoutInitialized?.Invoke(_data);
            }

            return def;
        }

        /// <summary>Returns true if the given room is permanently submerged.</summary>
        public bool IsRoomSubmerged(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return false;
            return _submergedSet.Contains(roomId);
        }

        /// <summary>
        /// Returns true if the room can be accessed. Submerged rooms require
        /// a rowboat; dry rooms are always accessible.
        /// </summary>
        public bool CanAccessRoom(string roomId, bool hasRowboat)
        {
            if (string.IsNullOrEmpty(roomId)) return false;

            if (!IsRoomSubmerged(roomId))
                return true;

            if (!hasRowboat)
                return false;

            OnRoomAccessedByBoat?.Invoke(roomId);
            return true;
        }

        /// <summary>Returns the starting water supply bonus (200 units).</summary>
        public float GetStartingWaterBonus() => _data.waterSupplyBonus;

        /// <summary>
        /// Returns the permanent atmosphere overrides: humidity and mold both
        /// maxed at 1.0.
        /// </summary>
        public (float humidity, float mold) GetAtmosphereOverrides()
        {
            return (_data.humidityOverride, _data.moldOverride);
        }

        private void SyncSubmergedSet()
        {
            _submergedSet.Clear();
            if (_data.submergedRoomIds != null)
            {
                for (int i = 0; i < _data.submergedRoomIds.Count; i++)
                {
                    if (!string.IsNullOrEmpty(_data.submergedRoomIds[i]))
                        _submergedSet.Add(_data.submergedRoomIds[i]);
                }
            }
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public SubPenLayoutData GetState() => _data;

        public void RestoreState(SubPenLayoutData state)
        {
            _data = state ?? new SubPenLayoutData();
            SyncSubmergedSet();
            _initialized = false;
        }
    }

    /// <summary>
    /// ShelterMapSO-compatible definition DTO for the Sub-Pen layout.
    /// Mirrors the fields of <see cref="ShelterMapSO"/> for runtime construction
    /// without requiring a ScriptableObject asset.
    /// </summary>
    [Serializable]
    public class ShelterMapDefinition
    {
        public string LayoutId;
        public string LayoutName;
        public int RoomCount;
        public string[] RoomIds;
        public string[] RoomNames;
        public float[] RoomSizes;
        public float InherentShielding;
        public float StartingCleanWater;
        public float StartingIntegrity;
        public ShelterLayoutTrait[] Traits;
        public string[] Anomalies;
    }
}
