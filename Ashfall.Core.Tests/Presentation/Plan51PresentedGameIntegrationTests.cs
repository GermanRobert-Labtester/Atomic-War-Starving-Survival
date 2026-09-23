// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Presentation;
using Xunit;

namespace Ashfall.Core.Tests.Presentation
{
    public sealed class Plan51PresentedGameIntegrationTests
    {
        [Fact]
        public void HoldfastPresentationSlate_EvaluatesCrisisBandsCorrectly()
        {
            var slate = new HoldfastPresentationSlate();

            // All nominal -> Calm
            var r1 = new RoomPresentationSnapshot("room_bunks", "Dormitory", isPowered: true, isFlooding: false);
            var r2 = new RoomPresentationSnapshot("room_gen", "Generator Vault", isPowered: true, isFlooding: false);
            var r3 = new RoomPresentationSnapshot("room_sump", "Sump Drainage", isPowered: true, isFlooding: false);

            slate.AddRoom(r1);
            slate.AddRoom(r2);
            slate.AddRoom(r3);

            Assert.Equal(3, slate.TotalRooms);
            Assert.Equal(3, slate.PoweredRoomsCount);
            Assert.Equal(0, slate.FloodedRoomsCount);
            Assert.Equal(ShelterVisualCrisisBand.Calm, slate.ReevaluateCrisisBand());

            // 1 unpowered -> Brownout
            r1.IsPowered = false;
            r1.UpdatePrimaryHazard();
            Assert.Equal(RoomHazardVisualBand.UnpoweredDark, r1.PrimaryHazard);
            Assert.Equal(ShelterVisualCrisisBand.Brownout, slate.ReevaluateCrisisBand());

            // 1 flooding -> Flooding
            r3.IsFlooding = true;
            r3.UpdatePrimaryHazard();
            Assert.Equal(RoomHazardVisualBand.FloodingSubmerged, r3.PrimaryHazard);
            Assert.Equal(ShelterVisualCrisisBand.Flooding, slate.ReevaluateCrisisBand());

            // 2 unpowered + 1 flooding -> Crisis
            r2.IsPowered = false;
            r2.UpdatePrimaryHazard();
            Assert.Equal(ShelterVisualCrisisBand.Crisis, slate.ReevaluateCrisisBand());
        }

        [Fact]
        public void HoldfastPresentationSlate_TracksActorStanceAndGrief()
        {
            var slate = new HoldfastPresentationSlate();

            var a1 = new SurvivorActorPresentationSnapshot("survivor_elena", "room_bunks", SurvivorVisualStance.Working, hasGriefMarker: false, moraleExpression: "resolute");
            var a2 = new SurvivorActorPresentationSnapshot("survivor_marcus", "room_bunks", SurvivorVisualStance.Grieving, hasGriefMarker: true, moraleExpression: "despairing")
            {
                LostPairGriefId = "survivor_suki"
            };

            slate.AddActor(a1);
            slate.AddActor(a2);

            Assert.Equal(2, slate.Actors.Count);
            Assert.Equal(1, slate.GrievingSurvivorsCount);
            Assert.Equal("survivor_suki", a2.LostPairGriefId);
            Assert.Equal(SurvivorVisualStance.Grieving, a2.Stance);
        }

        [Fact]
        public void HoldfastPresentationSlate_ProjectsWastelandMapNodesAndRoutes()
        {
            var slate = new HoldfastPresentationSlate();

            var node1 = new MapNodePresentationSnapshot("loc_holdfast", "Holdfast Sanctuary", MapKnowledgeVisualRung.Mapped, dangerTier: 1, ambientDoseRate: 0.05f, coordX: 500, coordY: 500);
            var node2 = new MapNodePresentationSnapshot("loc_thermal", "Geothermal Plant Ruins", MapKnowledgeVisualRung.Scouted, dangerTier: 4, ambientDoseRate: 2.5f, coordX: 750, coordY: 320);

            var route = new MapRoutePresentationSnapshot("loc_holdfast", "loc_thermal", distanceKm: 8.5f, hazardRating: 0.35f, isTraversable: true);

            slate.AddMapNode(node1);
            slate.AddMapNode(node2);
            slate.AddMapRoute(route);

            Assert.Equal(2, slate.MapNodes.Count);
            Assert.Equal(1, slate.MapRoutes.Count);

            Assert.Equal(4, node2.DangerTier);
            Assert.Equal(MapKnowledgeVisualRung.Scouted, node2.Knowledge);
            Assert.True(route.IsTraversable);
            Assert.Equal(8.5f, route.DistanceKm);
        }

        [Fact]
        public void PresentationMotionProfile_RespectsReduceMotionSetting()
        {
            var standard = new PresentationMotionProfile(reduceMotion: false);
            Assert.False(standard.ReduceMotion);
            Assert.Equal(0.25f, standard.TransitionDurationSeconds);
            Assert.Equal("cubic_ease_in_out", standard.EasingCurve);

            var accessible = new PresentationMotionProfile(reduceMotion: true);
            Assert.True(accessible.ReduceMotion);
            Assert.Equal(0.0f, accessible.TransitionDurationSeconds);
            Assert.Equal("instant", accessible.EasingCurve);
        }

        [Fact]
        public void HoldfastPresentationSlate_FiresSeamsOnRoomAddAndCrisisBandChange()
        {
            var slate = new HoldfastPresentationSlate();

            string? projectedRoomId = null;
            RoomPresentationSnapshot? projectedSnapshot = null;
            ShelterVisualCrisisBand? changedBand = null;

            slate.OnRoomProjectedSeam = (id, snap) =>
            {
                projectedRoomId = id;
                projectedSnapshot = snap;
            };

            slate.OnCrisisBandChangedSeam = band =>
            {
                changedBand = band;
            };

            var room = new RoomPresentationSnapshot("room_infirmary", "Medical Ward", isPowered: true, isFlooding: false);
            slate.AddRoom(room);

            Assert.Equal("room_infirmary", projectedRoomId);
            Assert.NotNull(projectedSnapshot);

            // Change room state and reevaluate crisis
            room.IsFlooding = true;
            room.UpdatePrimaryHazard();
            slate.ReevaluateCrisisBand();

            Assert.Equal(ShelterVisualCrisisBand.Flooding, changedBand);
            Assert.Equal(ShelterVisualCrisisBand.Flooding, slate.OverallShelterCrisisBand);
        }
    }
}
