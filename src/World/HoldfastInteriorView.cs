using Godot;
using System;
using System.Collections.Generic;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp.World
{
    public partial class HoldfastInteriorView : Node2D
    {
        [Signal]
        public delegate void RoomSelectedEventHandler(string roomId);

        private SurvivorsHostSession _survivors = null!;
        private List<SurvivorActorView> _survivorActors = new List<SurvivorActorView>();

        public HoldfastInteriorView()
        {
            // Self-contained: create the container nodes so this view works both
            // when instanced from HoldfastInterior.tscn and when created in code
            // (e.g. embedded in the ShelterPanel viewport).
            if (!HasNode("SurvivorActors"))
                AddChild(new Node2D { Name = "SurvivorActors" });
            if (!HasNode("RoomHotspots"))
                AddChild(new Node2D { Name = "RoomHotspots" });
        }

        public void Initialize(SurvivorsHostSession survivors)
        {
            _survivors = survivors;

            ClearExistingSurvivors();
            PopulateSurvivors();
            PopulateRoomHotspots();
            UpdateSurvivorPositions();
        }

        public void UpdateSurvivorPositions()
        {
            if (_survivors == null)
                return;

            foreach (var actor in _survivorActors)
            {
                UpdateSurvivorActor(actor);
            }
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

            // Only show first 4 survivors to prevent overcrowding and "stacked pizza" effect
            int maxSurvivors = Math.Min(_survivors.RosterState.Count, 4);
            for (int i = 0; i < maxSurvivors; i++)
            {
                var survivorState = _survivors.RosterState[i];
                if (survivorState == null || string.IsNullOrEmpty(survivorState.Id) || !survivorState.IsAliveState)
                    continue;

                var actor = new SurvivorActorView();
                actor.SurvivorId = survivorState.Id;
                actor.Label.Text = FormatSurvivorName(survivorState.Id);
                actor.Position = GetStartingPositionForSurvivor(i);
                survivorActorsNode.AddChild(actor);
                _survivorActors.Add(actor);
            }
        }

        private Vector2 GetStartingPositionForSurvivor(int index)
        {
            // Distribute survivors horizontally across the viewport with proper spacing
            // Viewport is 760x420, so distribute from left to right with margin
            int x = 100 + (index * 120);  // More compact spacing
            int y = 320;  // Center vertically in the viewport
            return new Vector2(x, y);
        }

        private string FormatSurvivorName(string id)
        {
            if (string.IsNullOrEmpty(id)) return "Unknown";
            return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(id.Replace('_', ' '));
        }

        private void PopulateRoomHotspots()
        {
            var roomHotspotsNode = GetNode<Node2D>("RoomHotspots");
            foreach (Node child in roomHotspotsNode.GetChildren())
            {
                roomHotspotsNode.RemoveChild(child);
                child.QueueFree();
            }

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
                roomHotspotsNode.AddChild(hotspot);
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
    }
}
