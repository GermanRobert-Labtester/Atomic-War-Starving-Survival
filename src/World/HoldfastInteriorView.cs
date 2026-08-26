using AtomicWar.GodotApp.UI;
using Godot;
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp.World
{
    public partial class HoldfastInteriorView : Node2D
    {
        [Signal]
        public delegate void RoomSelectedEventHandler(string roomId);

        private SurvivorsHostSession _survivors = null!;
        private DutyRosterHostSession? _dutyRoster;
        private List<SurvivorActorView> _survivorActors = new List<SurvivorActorView>();
        private Dictionary<string, RoomHotspotView> _roomHotspots = new Dictionary<string, RoomHotspotView>();
        private CanvasModulate? _lightingModulate;

        public HoldfastInteriorView()
        {
            if (!HasNode("SurvivorActors"))
                AddChild(new Node2D { Name = "SurvivorActors" });
            if (!HasNode("RoomHotspots"))
                AddChild(new Node2D { Name = "RoomHotspots" });

            _lightingModulate = new CanvasModulate
            {
                Name = "InteriorLighting",
                Color = new Color(0.95f, 0.95f, 0.95f, 1.0f)
            };
            AddChild(_lightingModulate);
        }

        public void Initialize(SurvivorsHostSession survivors, DutyRosterHostSession? dutyRoster = null)
        {
            _survivors = survivors;
            _dutyRoster = dutyRoster;

            ClearExistingSurvivors();
            PopulateRoomHotspots();
            PopulateSurvivors();
            UpdateSurvivorPositions();
        }

        public void SetLightingPhase(string phase)
        {
            if (_lightingModulate == null) return;

            _lightingModulate.Color = phase.ToLowerInvariant() switch
            {
                "morning" => new Color(1.0f, 0.96f, 0.88f, 1.0f),
                "midday" => new Color(1.0f, 1.0f, 1.0f, 1.0f),
                "evening" => new Color(0.85f, 0.82f, 0.88f, 1.0f),
                "night" => new Color(0.55f, 0.58f, 0.70f, 1.0f),
                "emergency" => new Color(0.95f, 0.40f, 0.35f, 1.0f),
                _ => new Color(0.95f, 0.95f, 0.95f, 1.0f)
            };
        }

        public void UpdateSurvivorPositions()
        {
            if (_survivors == null)
                return;

            var roomCounts = new Dictionary<string, int>
            {
                { "room_storage_bay", 0 },
                { "room_bunker_corridor", 0 },
                { "room_bunks", 0 },
                { "room_filtration", 0 },
                { "room_airlock", 0 }
            };

            foreach (var actor in _survivorActors)
            {
                UpdateSurvivorActor(actor);
                string assignedRoom = GetAssignedRoomForSurvivor(actor.SurvivorId);
                if (roomCounts.ContainsKey(assignedRoom))
                {
                    roomCounts[assignedRoom]++;
                    int offset = (roomCounts[assignedRoom] - 1) * 35;
                    actor.Position = GetRoomBasePosition(assignedRoom) + new Vector2(offset, 25);
                }
            }

            // Update hotspot occupant counters
            foreach (var kvp in _roomHotspots)
            {
                int count = roomCounts.TryGetValue(kvp.Key, out int c) ? c : 0;
                string status = kvp.Key switch
                {
                    "room_filtration" => "Filtration Stack: Active · Attenuation: 99%",
                    "room_airlock" => "Airlock: Sealed · Outer Decon Ready",
                    "room_bunks" => "Living Quarters: Warmth 100%",
                    "room_storage_bay" => "Tool & Supply Depot",
                    _ => "Access Concourse"
                };
                kvp.Value.SetRoomInfo(GetRoomDisplayName(kvp.Key), status, count);
            }
        }

        private string GetAssignedRoomForSurvivor(string survivorId)
        {
            if (_dutyRoster != null && _dutyRoster.Roster != null)
            {
                var role = _dutyRoster.Roster.GetRoleOf(survivorId);
                if (!string.IsNullOrEmpty(role))
                {
                    if (role == DutyRosterIds.RoleIntakeSleeper)
                        return "room_filtration";
                    if (role == DutyRosterIds.RoleNightWatch || role == DutyRosterIds.RoleHatchOpener)
                        return "room_airlock";
                    if (role == DutyRosterIds.RoleMess)
                        return "room_bunker_corridor";
                }
            }

            return "room_bunks";
        }

        private Vector2 GetRoomBasePosition(string roomId)
        {
            return roomId switch
            {
                "room_storage_bay" => new Vector2(160, 290),
                "room_bunker_corridor" => new Vector2(340, 290),
                "room_bunks" => new Vector2(510, 290),
                "room_filtration" => new Vector2(680, 290),
                "room_airlock" => new Vector2(860, 290),
                _ => new Vector2(510, 290)
            };
        }

        private string GetRoomDisplayName(string roomId)
        {
            return roomId switch
            {
                "room_storage_bay" => "Storage Bay",
                "room_bunker_corridor" => "Central Corridor",
                "room_bunks" => "Bunk Living",
                "room_filtration" => "Filtration Stack",
                "room_airlock" => "Airlock Hatch",
                _ => "Shelter Area"
            };
        }

        private void ClearExistingSurvivors()
        {
            var survivorActorsNode = GetNode<Node2D>("SurvivorActors");
            foreach (var actor in _survivorActors)
            {
                if (actor == null || !GodotObject.IsInstanceValid(actor))
                    continue;

                if (actor.GetParent() == survivorActorsNode)
                    survivorActorsNode.RemoveChild(actor);
                actor.QueueFree();
            }
            _survivorActors.Clear();
        }

        private void PopulateSurvivors()
        {
            if (_survivors == null)
                return;

            var survivorActorsNode = GetNode<Node2D>("SurvivorActors");

            int maxSurvivors = Math.Min(_survivors.RosterState.Count, 6);
            for (int i = 0; i < maxSurvivors; i++)
            {
                var survivorState = _survivors.RosterState[i];
                if (survivorState == null || string.IsNullOrEmpty(survivorState.Id) || !survivorState.IsAliveState)
                    continue;

                var actor = new SurvivorActorView();
                actor.SurvivorId = survivorState.Id;
                actor.Label.Text = FormatSurvivorName(survivorState.Id);
                survivorActorsNode.AddChild(actor);
                _survivorActors.Add(actor);
            }
        }

        private string FormatSurvivorName(string id)
        {
            if (string.IsNullOrEmpty(id)) return "Unknown";
            return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(id.Replace('_', ' '));
        }

        private void PopulateRoomHotspots()
        {
            var roomHotspotsNode = GetNode<Node2D>("RoomHotspots");
            AshfallUiHelpers.EmptyChildren(roomHotspotsNode);
            _roomHotspots.Clear();

            var rooms = new[]
            {
                new { Id = "room_storage_bay", DisplayName = "Storage Bay", PositionX = 160, PositionY = 240 },
                new { Id = "room_bunker_corridor", DisplayName = "Central Corridor", PositionX = 340, PositionY = 240 },
                new { Id = "room_bunks", DisplayName = "Bunk Living", PositionX = 510, PositionY = 240 },
                new { Id = "room_filtration", DisplayName = "Filtration Stack", PositionX = 680, PositionY = 240 },
                new { Id = "room_airlock", DisplayName = "Airlock Hatch", PositionX = 860, PositionY = 240 }
            };

            foreach (var room in rooms)
            {
                var hotspot = new RoomHotspotView();
                hotspot.RoomId = room.Id;
                hotspot.Position = new Vector2(room.PositionX, room.PositionY);
                hotspot.SetRoomInfo(room.DisplayName, "Holding status...", 0);
                hotspot.Connect(RoomHotspotView.SignalName.Clicked, Callable.From<string>(OnRoomClicked));
                roomHotspotsNode.AddChild(hotspot);
                _roomHotspots[room.Id] = hotspot;
            }
        }

        private void UpdateSurvivorActor(SurvivorActorView actor)
        {
            if (_survivors == null || string.IsNullOrEmpty(actor.SurvivorId))
                return;

            var survivorState = _survivors.Find(actor.SurvivorId);
            if (survivorState == null || !survivorState.IsAliveState)
            {
                actor.Visible = false;
                return;
            }

            var rad = _survivors.RadStateFor(actor.SurvivorId);
            actor.Visible = true;
            actor.UpdateFromSurvivor(survivorState, rad);
        }

        private void OnRoomClicked(string roomId)
        {
            EmitSignal(SignalName.RoomSelected, roomId);
            GD.Print($"[HoldfastInterior] Room clicked: {roomId}");
        }

        public override void _ExitTree()
        {
            ClearExistingSurvivors();
            base._ExitTree();
        }
    }
}
