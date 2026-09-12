// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// SHELTER_GRID_CATALOG_SEAL Phase 1: loader contract for power_grid.json —
    /// strict validation for tests, fallback-defaults policy for the host boot path.
    /// </summary>
    public sealed class ShelterPowerGridCatalogLoaderTests
    {
        private static string FindDataDir()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
            return dataDir ?? string.Empty;
        }

        private sealed class StaticFileIO : IFileIO
        {
            private readonly string _path;
            private readonly string _content;
            public StaticFileIO(string path, string content) { _path = path; _content = content; }
            public bool DirectoryExists(string path) => true;
            public bool FileExists(string path) => path == _path;
            public string ReadAllText(string path) => path == _path ? _content : throw new FileNotFoundException(path);
            public void WriteAllText(string path, string contents) { }
            public string Combine(params string[] parts) => string.Join("/", parts);
        }

        // ── Shipped catalog ─────────────────────────────────────────────

        [Fact]
        public void ShippedCatalog_StrictLoad_Succeeds()
        {
            var ok = ShelterPowerGridCatalogLoader.TryLoad(FindDataDir(), new FileSystemIO(),
                new SystemTextJsonSerializer(), out var catalog, out var error);
            Assert.True(ok, error);
            Assert.NotNull(catalog);
        }

        [Fact]
        public void ShippedCatalog_HostLoad_MatchesStrictLoad()
        {
            var dataDir = FindDataDir();
            var strict = ShelterPowerGridCatalogLoader.TryLoad(dataDir, new FileSystemIO(),
                new SystemTextJsonSerializer(), out var a, out _);
            Assert.True(strict);
            var host = ShelterPowerGridCatalogLoader.LoadOrDefault(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Equal(a!.Rooms.Count, host.Rooms.Count);
            for (int i = 0; i < a.Rooms.Count; i++)
                Assert.Equal(a.Rooms[i].Id, host.Rooms[i].Id);
        }

        // ── Fallback policy ─────────────────────────────────────────────

        [Fact]
        public void MissingFile_LoadOrDefault_FallsBackToDefaults()
        {
            var catalog = ShelterPowerGridCatalogLoader.LoadOrDefault("/nonexistent/dir", new FileSystemIO(), new SystemTextJsonSerializer());
            var fallback = ShelterPowerGridCatalogLoader.FallbackDefault();
            Assert.Equal(fallback.Rooms.Count, catalog.Rooms.Count);
            Assert.Equal(fallback.GenerationWattsDefault, catalog.GenerationWattsDefault);
        }

        [Fact]
        public void MissingFile_TryLoad_ReportsFileNotFound()
        {
            var ok = ShelterPowerGridCatalogLoader.TryLoad("/nonexistent/dir", new FileSystemIO(),
                new SystemTextJsonSerializer(), out _, out var error);
            Assert.False(ok);
            Assert.Contains("file not found", error, StringComparison.Ordinal);
        }

        [Fact]
        public void MalformedJson_TryLoad_FailsWithFileAndReason()
        {
            var io = new StaticFileIO("/mem/power_grid.json", "{ not valid json !!!");
            var ok = ShelterPowerGridCatalogLoader.TryLoad("/mem", io, new SystemTextJsonSerializer(), out _, out var error);
            Assert.False(ok);
            Assert.Contains("power_grid.json", error, StringComparison.Ordinal);
            Assert.Contains("malformed", error, StringComparison.Ordinal);
        }

        [Fact]
        public void MalformedJson_LoadOrDefault_FallsBack()
        {
            var io = new StaticFileIO("/mem/power_grid.json", "{ not valid json !!!");
            var catalog = ShelterPowerGridCatalogLoader.LoadOrDefault("/mem", io, new SystemTextJsonSerializer());
            Assert.Equal(ShelterPowerGridCatalogLoader.FallbackDefault().Rooms.Count, catalog.Rooms.Count);
        }

        // ── Validation rules ────────────────────────────────────────────

        [Fact]
        public void DuplicateRoomId_FailsWithRoomId()
        {
            var catalog = ShelterPowerGridCatalogLoader.FallbackDefault();
            catalog.Rooms.Add(new ShelterPowerGridRoomDef { Id = "room_clinic", DisplayName = "Dup", DrawWatts = 10f, DefaultPriority = "low" });
            var ok = ShelterPowerGridCatalogLoader.Validate(catalog, out var error);
            Assert.False(ok);
            Assert.Contains("room_clinic", error, StringComparison.Ordinal);
        }

        [Fact]
        public void NegativeDraw_FailsWithRoomId()
        {
            var catalog = ShelterPowerGridCatalogLoader.FallbackDefault();
            catalog.Rooms[0].DrawWatts = -5f;
            var ok = ShelterPowerGridCatalogLoader.Validate(catalog, out var error);
            Assert.False(ok);
            Assert.Contains(catalog.Rooms[0].Id, error, StringComparison.Ordinal);
        }

        [Fact]
        public void UnknownPriority_FailsWithRoomId()
        {
            var catalog = ShelterPowerGridCatalogLoader.FallbackDefault();
            catalog.Rooms[0].DefaultPriority = "ultra";
            var ok = ShelterPowerGridCatalogLoader.Validate(catalog, out var error);
            Assert.False(ok);
            Assert.Contains("ultra", error, StringComparison.Ordinal);
        }

        [Fact]
        public void WrongSchemaVersion_Fails()
        {
            var catalog = ShelterPowerGridCatalogLoader.FallbackDefault();
            catalog.SchemaVersion = 99;
            var ok = ShelterPowerGridCatalogLoader.Validate(catalog, out var error);
            Assert.False(ok);
            Assert.Contains("schema_version", error, StringComparison.Ordinal);
        }

        [Fact]
        public void EmptyDisplayName_FailsWithRoomId()
        {
            var catalog = ShelterPowerGridCatalogLoader.FallbackDefault();
            catalog.Rooms[0].DisplayName = "";
            var ok = ShelterPowerGridCatalogLoader.Validate(catalog, out var error);
            Assert.False(ok);
            Assert.Contains(catalog.Rooms[0].Id, error, StringComparison.Ordinal);
        }

        // ── Determinism ─────────────────────────────────────────────────

        [Fact]
        public void Load_IsDeterministicInOrder()
        {
            var dataDir = FindDataDir();
            var a = ShelterPowerGridCatalogLoader.LoadOrDefault(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var b = ShelterPowerGridCatalogLoader.LoadOrDefault(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Equal(a.Rooms.Count, b.Rooms.Count);
            for (int i = 0; i < a.Rooms.Count; i++)
            {
                Assert.Equal(a.Rooms[i].Id, b.Rooms[i].Id);
                Assert.Equal(a.Rooms[i].DrawWatts, b.Rooms[i].DrawWatts);
                Assert.Equal(a.Rooms[i].DefaultPriority, b.Rooms[i].DefaultPriority);
            }
        }

        [Fact]
        public void FallbackDefault_Validates()
        {
            var ok = ShelterPowerGridCatalogLoader.Validate(ShelterPowerGridCatalogLoader.FallbackDefault(), out var error);
            Assert.True(ok, error);
        }
    }
}
