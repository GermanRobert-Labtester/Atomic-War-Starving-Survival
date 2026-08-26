using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public class WastelandMapRouteValidationTests
    {
        private static List<MapNode> GetTestNodes() => new List<MapNode>
        {
            new MapNode { Id = "loc_alpha", DisplayName = "Alpha Station", StartingUnlocked = true },
            new MapNode { Id = "loc_beta", DisplayName = "Beta Bunker", StartingUnlocked = false },
            new MapNode { Id = "loc_gamma", DisplayName = "Gamma Mine", StartingUnlocked = false }
        };

        private static string GetDataDir()
        {
            return Path.Combine("..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
        }

        [Fact]
        public void ValidateRoutes_PassesOnCanonicalCatalog()
        {
            string dataDir = GetDataDir();
            var (nodes, routes, errors) = WastelandMapCatalogLoader.LoadWithValidation(dataDir);

            Assert.NotEmpty(nodes);
            Assert.NotEmpty(routes);
            Assert.Empty(errors);
        }

        [Fact]
        public void ValidateRoutes_DetectsDuplicateRoutes()
        {
            var nodes = GetTestNodes();
            var routes = new List<MapRoute>
            {
                new MapRoute { From = "loc_alpha", To = "loc_beta", DistanceKm = 10 },
                new MapRoute { From = "loc_alpha", To = "loc_beta", DistanceKm = 12 } // duplicate
            };

            var errors = WastelandMapCatalogLoader.ValidateRoutes(nodes, routes);

            Assert.Single(errors);
            Assert.Equal(MapRouteErrorKind.DuplicateRoute, errors[0].Kind);
            Assert.Contains("Duplicate route detected", errors[0].ErrorMessage);
        }

        [Fact]
        public void ValidateRoutes_DetectsDanglingFromEndpoint()
        {
            var nodes = GetTestNodes();
            var routes = new List<MapRoute>
            {
                new MapRoute { From = "loc_non_existent", To = "loc_beta", DistanceKm = 10 }
            };

            var errors = WastelandMapCatalogLoader.ValidateRoutes(nodes, routes);

            Assert.Contains(errors, e => e.Kind == MapRouteErrorKind.DanglingEndpoint && e.ErrorMessage.Contains("'from'"));
        }

        [Fact]
        public void ValidateRoutes_DetectsDanglingToEndpoint()
        {
            var nodes = GetTestNodes();
            var routes = new List<MapRoute>
            {
                new MapRoute { From = "loc_alpha", To = "loc_missing_destination", DistanceKm = 10 }
            };

            var errors = WastelandMapCatalogLoader.ValidateRoutes(nodes, routes);

            Assert.Contains(errors, e => e.Kind == MapRouteErrorKind.DanglingEndpoint && e.ErrorMessage.Contains("'to'"));
        }

        [Fact]
        public void ValidateRoutes_DetectsNegativeAndZeroDistances()
        {
            var nodes = GetTestNodes();
            var routes = new List<MapRoute>
            {
                new MapRoute { From = "loc_alpha", To = "loc_beta", DistanceKm = -5.5f },
                new MapRoute { From = "loc_beta", To = "loc_gamma", DistanceKm = 0f }
            };

            var errors = WastelandMapCatalogLoader.ValidateRoutes(nodes, routes);

            Assert.Equal(2, errors.Count);
            Assert.All(errors, e => Assert.Equal(MapRouteErrorKind.NegativeOrZeroDistance, e.Kind));
        }

        [Fact]
        public void ValidateRoutes_DetectsSelfRoute()
        {
            var nodes = GetTestNodes();
            var routes = new List<MapRoute>
            {
                new MapRoute { From = "loc_alpha", To = "loc_alpha", DistanceKm = 10 } // self-route
            };

            var errors = WastelandMapCatalogLoader.ValidateRoutes(nodes, routes);

            Assert.Single(errors);
            Assert.Equal(MapRouteErrorKind.SelfRoute, errors[0].Kind);
            Assert.Contains("Self-route detected", errors[0].ErrorMessage);
        }

        [Fact]
        public void WastelandMapSystem_SanitizesInvalidRoutesOnConstruction()
        {
            var nodes = GetTestNodes();
            var mixedRoutes = new List<MapRoute>
            {
                new MapRoute { From = "loc_alpha", To = "loc_beta", DistanceKm = 10 }, // valid
                new MapRoute { From = "loc_alpha", To = "loc_alpha", DistanceKm = 5 }, // self-route (should be ignored)
                new MapRoute { From = "loc_alpha", To = "loc_dangling", DistanceKm = 8 }, // dangling (should be ignored)
                new MapRoute { From = "loc_beta", To = "loc_gamma", DistanceKm = -3 }, // negative distance (should be ignored)
                new MapRoute { From = "loc_alpha", To = "loc_beta", DistanceKm = 15 } // duplicate (should be ignored)
            };

            var system = new WastelandMapSystem(new WastelandMapState(), nodes, mixedRoutes);

            // Only the 1 valid route should be present in the active system
            Assert.Single(system.Routes);
            Assert.Equal("loc_alpha", system.Routes[0].From);
            Assert.Equal("loc_beta", system.Routes[0].To);
            Assert.Equal(10f, system.Routes[0].DistanceKm);
        }

        [Fact]
        public void CatalogIntegrityValidator_DetectsMalformedRoutesInCatalogJson()
        {
            string malformedJson = @"{
                ""schema_version"": 2,
                ""nodes"": [
                    { ""id"": ""loc_1"", ""displayName"": ""Node 1"", ""startingUnlocked"": true },
                    { ""id"": ""loc_2"", ""displayName"": ""Node 2"", ""startingUnlocked"": false }
                ],
                ""routes"": [
                    { ""from"": ""loc_1"", ""to"": ""loc_1"", ""distanceKm"": 10 },
                    { ""from"": ""loc_1"", ""to"": ""loc_2"", ""distanceKm"": -4 },
                    { ""from"": ""loc_1"", ""to"": ""loc_2"", ""distanceKm"": 12 },
                    { ""from"": ""loc_1"", ""to"": ""loc_2"", ""distanceKm"": 15 }
                ]
            }";

            var mockFiles = new InMemoryFileIO();
            mockFiles.WriteAllText("wasteland_map_v1.json", malformedJson);

            var report = CatalogIntegrityValidator.Validate(".", mockFiles);

            Assert.NotEmpty(report.Errors);
            Assert.Contains(report.Errors, err => err.Contains("self-route detected"));
            Assert.Contains(report.Errors, err => err.Contains("negative or zero distance"));
            Assert.Contains(report.Errors, err => err.Contains("duplicate route"));
        }

        private sealed class InMemoryFileIO : IFileIO
        {
            private readonly Dictionary<string, string> _files = new Dictionary<string, string>();

            public bool DirectoryExists(string path) => true;
            public bool FileExists(string path) => _files.ContainsKey(path) || _files.ContainsKey(Path.GetFileName(path));
            public string ReadAllText(string path)
            {
                if (_files.TryGetValue(path, out var content)) return content;
                if (_files.TryGetValue(Path.GetFileName(path), out var leafContent)) return leafContent;
                throw new FileNotFoundException(path);
            }
            public void WriteAllText(string path, string contents) => _files[path] = contents;
            public void AppendAllText(string path, string contents) => _files[path] = (_files.TryGetValue(path, out var s) ? s : "") + contents;
            public void CreateDirectory(string path) { }
            public void DeleteFile(string path) => _files.Remove(path);
            public string[] GetFiles(string directory, string pattern) => _files.Keys.ToArray();
            public string[] EnumerateFiles(string directory, string searchPattern, SearchOption searchOption) => _files.Keys.ToArray();
            public string Combine(params string[] paths) => string.Join("/", paths);
            public string GetDirectoryName(string path) => Path.GetDirectoryName(path) ?? "";
            public string GetFileName(string path) => Path.GetFileName(path) ?? "";
        }
    }
}
