// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Presentation
{
    /// <summary>
    /// Overall visual and lighting crisis band of the Holdfast shelter interior.
    /// Used for camera grading, lighting palette swaps, and ambient soundscape modulation.
    /// </summary>
    public enum ShelterVisualCrisisBand
    {
        /// <summary>Nominal operations, powered life support, controlled atmosphere.</summary>
        Calm = 0,

        /// <summary>Generator offline or partial grid failure; emergency amber lighting.</summary>
        Brownout = 1,

        /// <summary>Subterranean sump or drainage breach; rising cold water in lower decks.</summary>
        Flooding = 2,

        /// <summary>Compound failures; critical radiation, cold, blackout, or cascading destruction.</summary>
        Crisis = 3
    }

    /// <summary>
    /// Visual stance/posture of a survivor actor in a shelter interior room.
    /// </summary>
    public enum SurvivorVisualStance
    {
        Idling = 0,
        Working = 1,
        Sleeping = 2,
        Grieving = 3,
        Incapacitated = 4,
        Perished = 5
    }

    /// <summary>
    /// Visual hazard classification for a specific interior room.
    /// </summary>
    public enum RoomHazardVisualBand
    {
        Nominal = 0,
        UnpoweredDark = 1,
        FloodingSubmerged = 2,
        FreezingCold = 3,
        ThermalHazard = 4
    }

    /// <summary>
    /// Visual knowledge tier of a wasteland map location.
    /// </summary>
    public enum MapKnowledgeVisualRung
    {
        Unknown = 0,
        Rumored = 1,
        Scouted = 2,
        Mapped = 3
    }

    /// <summary>
    /// Snapshot of an interior room's presentation state.
    /// </summary>
    public sealed class RoomPresentationSnapshot
    {
        public string RoomId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public bool IsPowered { get; set; } = true;
        public bool IsFlooding { get; set; }
        public float TemperatureCelsius { get; set; } = 18.0f;
        public List<string> OccupantIds { get; } = new List<string>();
        public string? ActiveJobTitle { get; set; }
        public RoomHazardVisualBand PrimaryHazard { get; set; } = RoomHazardVisualBand.Nominal;
        public string BoundPanelRoute { get; set; } = string.Empty;

        public RoomPresentationSnapshot() { }

        public RoomPresentationSnapshot(
            string roomId,
            string displayName,
            bool isPowered = true,
            bool isFlooding = false,
            float temperatureCelsius = 18.0f,
            string boundPanelRoute = "")
        {
            RoomId = roomId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            IsPowered = isPowered;
            IsFlooding = isFlooding;
            TemperatureCelsius = temperatureCelsius;
            BoundPanelRoute = boundPanelRoute ?? string.Empty;

            UpdatePrimaryHazard();
        }

        public void UpdatePrimaryHazard()
        {
            if (IsFlooding)
            {
                PrimaryHazard = RoomHazardVisualBand.FloodingSubmerged;
            }
            else if (!IsPowered)
            {
                PrimaryHazard = RoomHazardVisualBand.UnpoweredDark;
            }
            else if (TemperatureCelsius <= 2.0f)
            {
                PrimaryHazard = RoomHazardVisualBand.FreezingCold;
            }
            else if (TemperatureCelsius >= 38.0f)
            {
                PrimaryHazard = RoomHazardVisualBand.ThermalHazard;
            }
            else
            {
                PrimaryHazard = RoomHazardVisualBand.Nominal;
            }
        }
    }

    /// <summary>
    /// Snapshot of a survivor actor in the shelter interior.
    /// Reflects schedule, assignment, grief, and health.
    /// </summary>
    public sealed class SurvivorActorPresentationSnapshot
    {
        public string SurvivorId { get; set; } = string.Empty;
        public string CurrentRoomId { get; set; } = string.Empty;
        public SurvivorVisualStance Stance { get; set; } = SurvivorVisualStance.Idling;
        public bool HasGriefMarker { get; set; }
        public string MoraleExpression { get; set; } = "content";
        public string? CurrentPairId { get; set; }
        public string? LostPairGriefId { get; set; }

        public SurvivorActorPresentationSnapshot() { }

        public SurvivorActorPresentationSnapshot(
            string survivorId,
            string currentRoomId,
            SurvivorVisualStance stance = SurvivorVisualStance.Idling,
            bool hasGriefMarker = false,
            string moraleExpression = "content")
        {
            SurvivorId = survivorId ?? string.Empty;
            CurrentRoomId = currentRoomId ?? string.Empty;
            Stance = stance;
            HasGriefMarker = hasGriefMarker;
            MoraleExpression = moraleExpression ?? "content";
        }
    }

    /// <summary>
    /// Snapshot of a wasteland map location node.
    /// </summary>
    public sealed class MapNodePresentationSnapshot
    {
        public string NodeId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public MapKnowledgeVisualRung Knowledge { get; set; } = MapKnowledgeVisualRung.Unknown;
        public int DangerTier { get; set; } = 1;
        public float AmbientDoseRate { get; set; }
        public int CoordX { get; set; }
        public int CoordY { get; set; }

        public MapNodePresentationSnapshot() { }

        public MapNodePresentationSnapshot(
            string nodeId,
            string displayName,
            MapKnowledgeVisualRung knowledge = MapKnowledgeVisualRung.Unknown,
            int dangerTier = 1,
            float ambientDoseRate = 0.0f,
            int coordX = 0,
            int coordY = 0)
        {
            NodeId = nodeId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            Knowledge = knowledge;
            DangerTier = Math.Clamp(dangerTier, 1, 6);
            AmbientDoseRate = Math.Max(0.0f, ambientDoseRate);
            CoordX = coordX;
            CoordY = coordY;
        }
    }

    /// <summary>
    /// Snapshot of a wasteland travel route edge between two map nodes.
    /// </summary>
    public sealed class MapRoutePresentationSnapshot
    {
        public string FromNodeId { get; set; } = string.Empty;
        public string ToNodeId { get; set; } = string.Empty;
        public float DistanceKm { get; set; } = 1.0f;
        public float HazardRating { get; set; }
        public bool IsTraversable { get; set; } = true;

        public MapRoutePresentationSnapshot() { }

        public MapRoutePresentationSnapshot(
            string fromNodeId,
            string toNodeId,
            float distanceKm = 1.0f,
            float hazardRating = 0.0f,
            bool isTraversable = true)
        {
            FromNodeId = fromNodeId ?? string.Empty;
            ToNodeId = toNodeId ?? string.Empty;
            DistanceKm = Math.Max(0.1f, distanceKm);
            HazardRating = Math.Max(0.0f, hazardRating);
            IsTraversable = isTraversable;
        }
    }

    /// <summary>
    /// Motion, animation timing, and accessibility profile for UI / world transitions.
    /// Enforces zero-duration instant switches when ReduceMotion is enabled.
    /// </summary>
    public readonly struct PresentationMotionProfile
    {
        public bool ReduceMotion { get; }
        public float TransitionDurationSeconds => ReduceMotion ? 0.0f : 0.25f;
        public string EasingCurve => ReduceMotion ? "instant" : "cubic_ease_in_out";
        public float ScreenFadeAlpha => 1.0f;

        public PresentationMotionProfile(bool reduceMotion = false)
        {
            ReduceMotion = reduceMotion;
        }
    }

    /// <summary>
    /// Complete Holdfast Presentation Slate providing pure domain projection
    /// for shelter interior and wasteland map views without engine references.
    /// </summary>
    public sealed class HoldfastPresentationSlate
    {
        public List<RoomPresentationSnapshot> Rooms { get; } = new List<RoomPresentationSnapshot>();
        public List<SurvivorActorPresentationSnapshot> Actors { get; } = new List<SurvivorActorPresentationSnapshot>();
        public List<MapNodePresentationSnapshot> MapNodes { get; } = new List<MapNodePresentationSnapshot>();
        public List<MapRoutePresentationSnapshot> MapRoutes { get; } = new List<MapRoutePresentationSnapshot>();

        public ShelterVisualCrisisBand OverallShelterCrisisBand { get; set; } = ShelterVisualCrisisBand.Calm;
        public PresentationMotionProfile MotionProfile { get; set; } = new PresentationMotionProfile(false);

        public int TotalRooms => Rooms.Count;

        public int PoweredRoomsCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < Rooms.Count; i++)
                    if (Rooms[i].IsPowered) count++;
                return count;
            }
        }

        public int FloodedRoomsCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < Rooms.Count; i++)
                    if (Rooms[i].IsFlooding) count++;
                return count;
            }
        }

        public int GrievingSurvivorsCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < Actors.Count; i++)
                    if (Actors[i].HasGriefMarker || Actors[i].Stance == SurvivorVisualStance.Grieving) count++;
                return count;
            }
        }

        /// <summary>Delegate seam fired whenever a room state is projected.</summary>
        public Action<string, RoomPresentationSnapshot>? OnRoomProjectedSeam { get; set; }

        /// <summary>Delegate seam fired whenever the overall shelter crisis band changes.</summary>
        public Action<ShelterVisualCrisisBand>? OnCrisisBandChangedSeam { get; set; }

        public void AddRoom(RoomPresentationSnapshot room)
        {
            if (room == null) return;
            Rooms.Add(room);
            OnRoomProjectedSeam?.Invoke(room.RoomId, room);
        }

        public void AddActor(SurvivorActorPresentationSnapshot actor)
        {
            if (actor != null)
                Actors.Add(actor);
        }

        public void AddMapNode(MapNodePresentationSnapshot node)
        {
            if (node != null)
                MapNodes.Add(node);
        }

        public void AddMapRoute(MapRoutePresentationSnapshot route)
        {
            if (route != null)
                MapRoutes.Add(route);
        }

        /// <summary>
        /// Evaluates and updates the overall shelter visual crisis band based on room conditions.
        /// </summary>
        public ShelterVisualCrisisBand ReevaluateCrisisBand()
        {
            int total = Rooms.Count;
            if (total == 0)
            {
                OverallShelterCrisisBand = ShelterVisualCrisisBand.Calm;
                return OverallShelterCrisisBand;
            }

            int unpowered = total - PoweredRoomsCount;
            int flooded = FloodedRoomsCount;

            ShelterVisualCrisisBand newBand;
            if (flooded >= 2 || (unpowered >= 2 && flooded >= 1))
            {
                newBand = ShelterVisualCrisisBand.Crisis;
            }
            else if (flooded >= 1)
            {
                newBand = ShelterVisualCrisisBand.Flooding;
            }
            else if (unpowered >= 1)
            {
                newBand = ShelterVisualCrisisBand.Brownout;
            }
            else
            {
                newBand = ShelterVisualCrisisBand.Calm;
            }

            if (newBand != OverallShelterCrisisBand)
            {
                OverallShelterCrisisBand = newBand;
                OnCrisisBandChangedSeam?.Invoke(OverallShelterCrisisBand);
            }

            return OverallShelterCrisisBand;
        }
    }
}
