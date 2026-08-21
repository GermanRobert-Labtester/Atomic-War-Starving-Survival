using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Godot;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Shelter Assignment host session (item 3).
    ///
    /// Thin Godot-side glue: holds the Core ShelterAssignmentSystem, loads
    /// and saves through ShelterAssignmentSaveStore, exposes the room list
    /// to the existing HoldfastInteriorView/RoomHotspotView, and registers
    /// with the Campaign Day Coordinator so per-day cleanup (decommissioned
    /// assignments) runs at the right seam.
    /// </summary>
    public sealed class ShelterAssignmentHostSession
    {
        public ShelterAssignmentSystem System { get; private set; }

        private readonly ISeededRng _rng;

        public static ShelterAssignmentHostSession CreateDefault(ISeededRng rng)
        {
            var rooms = new List<ShelterRoom>
            {
                new ShelterRoom("room_bunker_corridor", "Central Access Corridor", 0),
                new ShelterRoom("room_bunks", "Bunks", 4),
                new ShelterRoom("room_kitchen", "Kitchen", 2, "skill_cooking"),
                new ShelterRoom("room_clinic", "Clinic", 2, "skill_medic"),
                new ShelterRoom("room_workshop", "Workshop", 2, "skill_crafting"),
                new ShelterRoom("room_filtration", "Filtration Stack", 1, "skill_technician")
            };
            return new ShelterAssignmentHostSession(rooms, new ShelterAssignmentState(), rng);
        }

        public ShelterAssignmentHostSession(List<ShelterRoom> rooms,
            ShelterAssignmentState state, ISeededRng rng)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            System = new ShelterAssignmentSystem(state, rooms, rng);
        }

        public bool TrySave()
        {
            var save = new ShelterAssignmentSave
            {
                simDay = 0,
                Rooms = new List<ShelterRoomSave>(),
                State = System.CaptureState()
            };
            foreach (var r in System.Rooms)
                save.Rooms.Add(new ShelterRoomSave
                {
                    RoomId = r.RoomId,
                    DisplayName = r.DisplayName,
                    Capacity = r.Capacity,
                    RequiredSkillId = r.RequiredSkillId,
                    WorkstationId = r.WorkstationId
                });
            return ShelterAssignmentSaveStore.TrySave(save);
        }

        public bool TryLoad()
        {
            var loaded = ShelterAssignmentSaveStore.TryLoad();
            if (loaded == null) return false;
            System.State.RestoreInto(loaded.State, System.Rooms);
            return true;
        }
    }

    /// <summary>
    /// Save store for ShelterAssignmentSave (mirrors the other expansion stores).
    /// </summary>
    public static class ShelterAssignmentSaveStore
    {
        public const string FileName = "shelter_assignment_save.json";
        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();
        private static readonly ILog s_log = new GodotLog();

        public static string SavePath =>
            System.IO.Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);

        public static bool TrySave(ShelterAssignmentSave save)
        {
            if (save == null) return false;
            try
            {
                s_files.WriteAllText(SavePath, ShelterAssignmentSaveCodec.EncodeToString(save, s_json));
                return true;
            }
            catch (Exception e)
            {
                s_log.Error("[ShelterAssignmentSaveStore] save failed: " + e.Message);
                return false;
            }
        }

        public static ShelterAssignmentSave? TryLoad()
        {
            try
            {
                if (!s_files.FileExists(SavePath)) return null;
                return ShelterAssignmentSaveCodec.Decode(s_files.ReadAllText(SavePath), s_json);
            }
            catch (Exception e)
            {
                s_log.Error("[ShelterAssignmentSaveStore] load failed: " + e.Message);
                return null;
            }
        }
    }
}
