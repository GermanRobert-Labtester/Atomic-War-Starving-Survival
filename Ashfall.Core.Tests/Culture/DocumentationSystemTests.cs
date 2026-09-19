// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Culture;
using Xunit;

namespace Ashfall.Core.Tests.Culture
{
    public sealed class DocumentationSystemTests
    {
        [Fact]
        public void CreatePhotograph_InitializesPhotoAndEvent()
        {
            var system = new DocumentationSystem();
            var photo = system.CreatePhotograph(
                authorId: "photographer_1",
                title: "Sunset over Wasteland",
                cameraUsed: "item_camera_film",
                subjects: new[] { "survivor_a", "survivor_b" },
                locationId: "shelter_rooftop",
                composition: 85f,
                lighting: 75f,
                currentDay: 10,
                description: "Evening sky with amber haze."
            );

            Assert.NotNull(photo);
            Assert.Equal(DocumentationType.Photograph, photo.Type);
            Assert.Equal("photographer_1", photo.AuthorId);
            Assert.Equal(80f, photo.Quality); // (85 + 75) / 2
            Assert.NotNull(photo.Photo);
            Assert.Equal("item_camera_film", photo.Photo.CameraUsed);
            Assert.Equal(2, photo.Photo.Subjects.Count);
            Assert.Equal(1, system.TotalItemsCount);
        }

        [Fact]
        public void CreateSketch_CalculatesQualityAndSentimentalValue()
        {
            var system = new DocumentationSystem();
            var sketch = system.CreateSketch(
                authorId: "artist_1",
                title: "Hydroponic Basin Study",
                subject: "hydroponics",
                medium: "charcoal",
                artisticQuality: 90f,
                accuracy: 70f,
                hoursSpent: 3.5f,
                currentDay: 5
            );

            Assert.NotNull(sketch);
            Assert.Equal(DocumentationType.Sketch, sketch.Type);
            Assert.Equal(82f, sketch.Quality); // 90*0.6 + 70*0.4 = 54 + 28 = 82
            Assert.NotNull(sketch.Sketch);
            Assert.Equal("charcoal", sketch.Sketch.Medium);
            Assert.Equal(3.5f, sketch.Sketch.TimeSpentHours);
        }

        [Fact]
        public void CreateWrittenRecord_WordCountAndMoraleImpact()
        {
            var system = new DocumentationSystem();
            var record = system.CreateWrittenRecord(
                authorId: "writer_1",
                title: "Day 20 Expedition Log",
                recordType: "chronicle",
                content: "We discovered an intact storage bay beneath the ruins today.",
                writingQuality: 80f,
                currentDay: 20
            );

            Assert.NotNull(record);
            Assert.Equal(DocumentationType.WrittenRecord, record.Type);
            Assert.NotNull(record.Record);
            Assert.Equal(10, record.Record.WordCount);
            Assert.Equal(80f, record.Quality);
        }

        [Fact]
        public void CreateAlbum_And_AddPhotoToAlbum_MaintainsReferences()
        {
            var system = new DocumentationSystem();
            var photo1 = system.CreatePhotograph("dweller_1", "Photo 1", "camera", null, "loc_1", 70f, 70f, 1);
            var photo2 = system.CreatePhotograph("dweller_1", "Photo 2", "camera", null, "loc_1", 75f, 75f, 2);

            var album = system.CreateAlbum("dweller_1", "Shelter Chronicle", "history", currentDay: 2);
            Assert.NotNull(album);
            Assert.Equal(1, system.TotalAlbumsCount);

            bool added1 = system.AddPhotoToAlbum(album.AlbumId, photo1.DocumentationId, currentDay: 3);
            bool added2 = system.AddPhotoToAlbum(album.AlbumId, photo2.DocumentationId, currentDay: 3);

            Assert.True(added1);
            Assert.True(added2);
            Assert.Equal(2, album.PhotoIds.Count);

            // Duplicate prevention
            bool addedDup = system.AddPhotoToAlbum(album.AlbumId, photo1.DocumentationId, currentDay: 3);
            Assert.False(addedDup);
        }

        [Fact]
        public void ShareDocumentation_UpdatesIsPublicAndReturnsMoraleBoost()
        {
            var system = new DocumentationSystem();
            var doc = system.CreateSketch("dweller_2", "Secret Sketch", "garden", "pencil", 80f, 80f, 2f, 1, isPublic: false);
            Assert.False(doc.IsPublic);

            float boost = system.ShareDocumentation(doc.DocumentationId, currentDay: 4);

            Assert.True(doc.IsPublic);
            Assert.True(boost > 0f);
        }

        [Fact]
        public void CaptureState_And_RestoreState_RoundTripsAccurately()
        {
            var system1 = new DocumentationSystem();
            var p = system1.CreatePhotograph("alice", "Alice Portrait", "camera", new[] { "alice" }, "room_a", 90f, 90f, 10);
            var album = system1.CreateAlbum("alice", "Alice Life", "portraits", 10);
            system1.AddPhotoToAlbum(album.AlbumId, p.DocumentationId, 10);

            var state = system1.CaptureState();
            Assert.Single(state.Items);
            Assert.Single(state.Albums);

            var system2 = new DocumentationSystem();
            system2.RestoreState(state);

            Assert.Equal(1, system2.TotalItemsCount);
            Assert.Equal(1, system2.TotalAlbumsCount);

            var restoredAlbum = system2.GetAlbum(album.AlbumId);
            Assert.NotNull(restoredAlbum);
            Assert.Equal("Alice Life", restoredAlbum.AlbumName);
            Assert.Single(restoredAlbum.PhotoIds);
            Assert.Equal(p.DocumentationId, restoredAlbum.PhotoIds[0]);
        }
    }
}
