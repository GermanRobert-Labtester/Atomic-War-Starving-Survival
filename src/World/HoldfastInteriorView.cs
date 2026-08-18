using Godot;
using System.Collections.Generic;

namespace AtomicWar.GodotApp.World
{
    public partial class HoldfastInteriorView : Node2D
    {
        [Signal]
        public delegate void RoomSelectedEventHandler(string roomId);

        public void Initialize()
        {
            PopulateSurvivors();
            PopulateRoomHotspots();
        }

        private void PopulateSurvivors()
        {
            var survivorData = new[]
            {
                new { Id = "survivor_dr_sarah_chen", DisplayName = "Dr. Sarah Chen", PositionX = 200, PositionY = 500 },
                new { Id = "survivor_gunner_mikhail", DisplayName = "Gunner Mikhail", PositionX = 400, PositionY = 500 },
                new { Id = "elena_vasquez", DisplayName = "Elena Vasquez", PositionX = 600, PositionY = 500 }
            };

            foreach (var survivor in survivorData)
            {
                var actor = new SurvivorActorView();
                actor.SurvivorId = survivor.Id;
                actor.Label.Text = survivor.DisplayName;
                actor.Position = new Vector2(survivor.PositionX, survivor.PositionY);
                GetNode<Node2D>("SurvivorActors").AddChild(actor);
            }
        }

        private void PopulateRoomHotspots()
        {
            var rooms = new[]
            {
                new { Id = "room_bunker_corridor", DisplayName = "Central Access Corridor", PositionX = 300, PositionY = 300 },
                new { Id = "room_bunks", DisplayName = "Bunks", PositionX = 500, PositionY = 300 },
                new { Id = "room_filtration", DisplayName = "Filtration Stack", PositionX = 700, PositionY = 300 }
            };

            foreach (var room in rooms)
            {
                var hotspot = new RoomHotspotView();
                hotspot.RoomId = room.Id;
                hotspot.Label.Text = room.DisplayName;
                hotspot.Position = new Vector2(room.PositionX, room.PositionY);
                hotspot.Connect(RoomHotspotView.SignalName.Clicked, Callable.From<string>(OnRoomClicked));
                GetNode<Node2D>("RoomHotspots").AddChild(hotspot);
            }
        }

        private void OnRoomClicked(string roomId)
        {
            EmitSignal(SignalName.RoomSelected, roomId);
        }
    }
}