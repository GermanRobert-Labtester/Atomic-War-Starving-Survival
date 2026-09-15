// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    public sealed class EchoCatalogTests
    {
        private static string DataDirectory
        {
            get
            {
                string path = AppContext.BaseDirectory;
                while (!string.IsNullOrEmpty(path))
                {
                    string candidate = System.IO.Path.Combine(path, "Assets", "StreamingAssets", "Data");
                    if (System.IO.Directory.Exists(candidate)) return candidate;
                    path = System.IO.Directory.GetParent(path)?.FullName ?? string.Empty;
                }
                throw new InvalidOperationException("could not locate Assets/StreamingAssets/Data");
            }
        }

        [Fact]
        public void CurrentCatalogLoadsAllAuthoredEchoes()
        {
            var result = EchoCatalogLoader.LoadDetailed(
                DataDirectory,
                new FileSystemIO(),
                new SystemTextJsonSerializer());

            Assert.True(result.IsSuccess, string.Join("; ", result.Errors));
            Assert.Equal(23, result.Echoes.Count);
            Assert.All(result.Echoes, echo =>
            {
                Assert.StartsWith("echo_", echo.Id, StringComparison.Ordinal);
                Assert.NotEmpty(echo.Choices);
                Assert.All(echo.Choices, choice => Assert.True(choice.IsExecutable, echo.Id + "/" + choice.ChoiceId));
            });
        }

        [Fact]
        public void MalformedRootAndUnsupportedSchemaAreVisible()
        {
            var malformed = EchoCatalogLoader.LoadDetailed(
                "data",
                new MemoryFileIO("{\"schema_version\":1,\"echoes\":{}}"),
                new SystemTextJsonSerializer());
            Assert.False(malformed.IsSuccess);
            Assert.Contains(malformed.Errors, error => error.Contains("catalog JSON parse failed", StringComparison.Ordinal));

            var unsupported = EchoCatalogLoader.LoadDetailed(
                "data",
                new MemoryFileIO("{\"schema_version\":2,\"echoes\":[]}"),
                new SystemTextJsonSerializer());
            Assert.False(unsupported.IsSuccess);
            Assert.Contains(unsupported.Errors, error => error.Contains("unsupported schema_version", StringComparison.Ordinal));
        }

        [Fact]
        public void DuplicateEchoAndChoiceIdsAreRejected()
        {
            const string json =
                "{\"schema_version\":1,\"echoes\":[" +
                "{\"id\":\"echo_duplicate\",\"title\":\"A\",\"bodyText\":\"B\",\"weight\":1,\"minDay\":1," +
                "\"conditions\":{\"MinDay\":1,\"RequiredFlagId\":\"\"},\"choices\":[" +
                "{\"choiceId\":\"choose\",\"text\":\"Do it\",\"moraleDelta\":1,\"effects\":[]}]}, " +
                "{\"id\":\"echo_duplicate\",\"title\":\"A\",\"bodyText\":\"B\",\"weight\":1,\"minDay\":1," +
                "\"conditions\":{\"MinDay\":1,\"RequiredFlagId\":\"\"},\"choices\":[" +
                "{\"choiceId\":\"choose\",\"text\":\"Do it\",\"moraleDelta\":1,\"effects\":[]}, " +
                "{\"choiceId\":\"choose\",\"text\":\"Again\",\"moraleDelta\":1,\"effects\":[]}]}" +
                "]}";

            var result = EchoCatalogLoader.LoadDetailed(
                "data",
                new MemoryFileIO(json),
                new SystemTextJsonSerializer());

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors, error => error.Contains("duplicate echo id", StringComparison.Ordinal));
        }

        private sealed class MemoryFileIO : IFileIO
        {
            private readonly string _contents;

            public MemoryFileIO(string contents) => _contents = contents;

            public bool DirectoryExists(string path) => true;
            public bool FileExists(string path) => true;
            public string ReadAllText(string path) => _contents;
            public void WriteAllText(string path, string contents) { }
            public string Combine(params string[] parts) => string.Join("/", parts);
            public string[] EnumerateFiles(string path, string pattern, System.IO.SearchOption option) =>
                Array.Empty<string>();
            public void DeleteFile(string path) { }
            public void CreateDirectory(string path) { }
        }
    }
}
