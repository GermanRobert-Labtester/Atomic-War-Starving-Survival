// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Culture;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Culture
{
    public sealed class Plan219DocumentationIntegrationTests
    {
        private static Inventory.Inventory CreateTestInventory(int filmCount = 0)
        {
            var inv = new Inventory.Inventory();
            if (filmCount > 0)
            {
                inv.TryProduce("photographic_film", filmCount);
            }
            return inv;
        }

        [Fact]
        public void CulturalArchiveVaultSystem_InitializesDocumentationSystem()
        {
            var inv = CreateTestInventory();
            var vault = new CulturalArchiveVaultSystem(inv);

            Assert.NotNull(vault.Documentation);
            Assert.Equal(0, vault.Documentation.TotalItemsCount);
            Assert.Equal(0, vault.Documentation.TotalAlbumsCount);
        }

        [Fact]
        public void TryCreatePhotograph_ConsumesFilm_WhenRequireFilmTrue()
        {
            var inv = CreateTestInventory(filmCount: 3);
            var vault = new CulturalArchiveVaultSystem(inv);

            bool dirtyRaised = false;
            vault.OnDocumentationChanged += () => dirtyRaised = true;

            var result = vault.TryCreatePhotograph(
                authorId: "survivor_reporter",
                title: "Dawn on the Perimeter",
                cameraUsed: "vintage_rangefinder",
                subjects: new[] { "survivor_scout" },
                locationId: "loc_holdfast",
                composition: 80f,
                lighting: 70f,
                currentDay: 5,
                description: "Sun rising over the eastern watchtower.",
                isPublic: true,
                requireFilm: true,
                out var photo);

            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.NotNull(photo);
            Assert.Equal(2, inv.CountById("photographic_film")); // 1 consumed
            Assert.Equal(75f, photo.Quality); // (80 + 70) / 2
            Assert.Equal("survivor_reporter", photo.AuthorId);
            Assert.True(dirtyRaised);

            var save = vault.CaptureState();
            Assert.Contains(save.chronicle_entries, c => c.event_type == "documentation_photo");
            Assert.Single(save.documentation.Items);
        }

        [Fact]
        public void TryCreatePhotograph_Blocks_WhenFilmMissing_AndRequireFilmTrue()
        {
            var inv = CreateTestInventory(filmCount: 0);
            var vault = new CulturalArchiveVaultSystem(inv);

            var result = vault.TryCreatePhotograph(
                authorId: "survivor_reporter",
                title: "Failed Shot",
                cameraUsed: "camera",
                subjects: null,
                locationId: "loc_holdfast",
                composition: 50f,
                lighting: 50f,
                currentDay: 2,
                description: "",
                isPublic: true,
                requireFilm: true,
                out var photo);

            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
            Assert.Equal("missing_film", result.FailureCode);
            Assert.Null(photo);
            Assert.Equal(0, vault.Documentation.TotalItemsCount);
        }

        [Fact]
        public void TryCreateSketch_CalculatesQuality_AndRecordsChronicle()
        {
            var inv = CreateTestInventory();
            var vault = new CulturalArchiveVaultSystem(inv);

            var result = vault.TryCreateSketch(
                authorId: "survivor_artist",
                title: "Generator Room Architecture",
                subject: "generator",
                medium: "pencil",
                artisticQuality: 85f,
                accuracy: 75f,
                hoursSpent: 3f,
                currentDay: 12,
                description: "Detailed rendering of turbine housing.",
                isPublic: true,
                out var sketch);

            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.NotNull(sketch);
            Assert.Equal(81f, sketch.Quality); // 85*0.6 + 75*0.4 = 51 + 30 = 81
            Assert.Equal(DocumentationType.Sketch, sketch.Type);

            var save = vault.CaptureState();
            Assert.Contains(save.chronicle_entries, c => c.event_type == "documentation_sketch");
        }

        [Fact]
        public void TryCreateWrittenRecord_WordCount_AndChronicle()
        {
            var inv = CreateTestInventory();
            var vault = new CulturalArchiveVaultSystem(inv);

            var result = vault.TryCreateWrittenRecord(
                authorId: "survivor_scribe",
                title: "First Winter Chronicle",
                recordType: "chronicle",
                content: "The frost set in early, yet the hydroponic beds remained green.",
                writingQuality: 90f,
                currentDay: 15,
                isPublic: true,
                out var record);

            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.NotNull(record);
            Assert.Equal(DocumentationType.WrittenRecord, record.Type);
            Assert.Equal(11, record.Record?.WordCount);

            var save = vault.CaptureState();
            Assert.Contains(save.chronicle_entries, c => c.event_type == "documentation_record");
        }

        [Fact]
        public void TryShareDocumentation_BoostsMorale_AndMarksPublic()
        {
            var inv = CreateTestInventory(filmCount: 1);
            var vault = new CulturalArchiveVaultSystem(inv);

            vault.TryCreatePhotograph(
                authorId: "survivor_reporter",
                title: "Shelter Common Hall",
                cameraUsed: "camera",
                subjects: null,
                locationId: "loc_holdfast",
                composition: 80f,
                lighting: 80f,
                currentDay: 8,
                description: "Evening meal gathering.",
                isPublic: false,
                requireFilm: false,
                out var photo);

            Assert.NotNull(photo);
            Assert.False(photo.IsPublic);

            var shareResult = vault.TryShareDocumentation(photo.DocumentationId, currentDay: 9, out float moraleBoost);
            Assert.Equal(ActionResult.StatusKind.Success, shareResult.Status);
            Assert.True(moraleBoost > 0f);
            Assert.True(photo.IsPublic);

            var save = vault.CaptureState();
            Assert.Contains(save.chronicle_entries, c => c.event_type == "documentation_shared");
        }

        [Fact]
        public void PhotoAlbum_Creation_And_AddPhoto()
        {
            var inv = CreateTestInventory(filmCount: 2);
            var vault = new CulturalArchiveVaultSystem(inv);

            vault.TryCreatePhotograph("survivor_a", "Photo 1", "camera", null, "loc_a", 70f, 70f, 1, "", true, false, out var p1);
            vault.TryCreatePhotograph("survivor_a", "Photo 2", "camera", null, "loc_b", 75f, 75f, 2, "", true, false, out var p2);

            var album = vault.Documentation.CreateAlbum("survivor_a", "Scouting Memoirs", "exploration", currentDay: 3);
            Assert.NotNull(album);

            bool added1 = vault.Documentation.AddPhotoToAlbum(album.AlbumId, p1!.DocumentationId, currentDay: 3);
            bool added2 = vault.Documentation.AddPhotoToAlbum(album.AlbumId, p2!.DocumentationId, currentDay: 3);
            bool addedDup = vault.Documentation.AddPhotoToAlbum(album.AlbumId, p1.DocumentationId, currentDay: 3);

            Assert.True(added1);
            Assert.True(added2);
            Assert.False(addedDup);
            Assert.Equal(2, album.PhotoIds.Count);

            var userAlbums = vault.Documentation.GetAlbumsForOwner("survivor_a");
            Assert.Single(userAlbums);
        }

        [Fact]
        public void SaveRestore_RoundTrips_DocumentationState_ThroughCulturalArchiveVaultSave()
        {
            var inv1 = CreateTestInventory(filmCount: 5);
            var vault1 = new CulturalArchiveVaultSystem(inv1);

            vault1.TryCreatePhotograph("survivor_x", "Sunset", "camera", null, "loc_1", 80f, 80f, 10, "Nice view", true, false, out var photo);
            vault1.TryCreateSketch("survivor_y", "Tool Study", "wrench", "charcoal", 70f, 70f, 2f, 11, "", true, out var sketch);
            var album = vault1.Documentation.CreateAlbum("survivor_x", "Album Alpha", "history", 12);
            vault1.Documentation.AddPhotoToAlbum(album.AlbumId, photo!.DocumentationId, 12);

            var saved = vault1.CaptureState();
            Assert.Equal(2, saved.documentation.Items.Count);
            Assert.Single(saved.documentation.Albums);

            var inv2 = CreateTestInventory(filmCount: 0);
            var vault2 = new CulturalArchiveVaultSystem(inv2);
            vault2.RestoreState(saved);

            Assert.Equal(2, vault2.Documentation.TotalItemsCount);
            Assert.Equal(1, vault2.Documentation.TotalAlbumsCount);

            var restoredDocsX = vault2.Documentation.GetDocumentationByAuthor("survivor_x");
            Assert.Single(restoredDocsX);
            Assert.Equal("Sunset", restoredDocsX[0].Title);

            var restoredAlbum = vault2.Documentation.GetAlbum(album.AlbumId);
            Assert.NotNull(restoredAlbum);
            Assert.Single(restoredAlbum.PhotoIds);
            Assert.Equal(photo.DocumentationId, restoredAlbum.PhotoIds[0]);
        }
    }
}
