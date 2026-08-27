// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: CatalogFileSystem direct coverage.

using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests;

/// <summary>
/// Direct tests for <see cref="CatalogFileSystem"/> enumeration behavior.
/// These tests exercise the public contract without going through
/// <see cref="CatalogIntegrityValidator"/>.
/// </summary>
public class CatalogFileSystemTests
{
    private static string DataDirectory
    {
        get
        {
            var cwd = Directory.GetCurrentDirectory();
            CatalogLocator.TryFindDataDirectory(cwd, out var dataDir);
            return string.IsNullOrEmpty(dataDir) ? cwd : dataDir;
        }
    }

    [Fact]
    public void EnumerateJsonFiles_NullFiles_ReturnsEmpty()
    {
        var result = CatalogFileSystem.EnumerateJsonFiles(null!, DataDirectory, SearchOption.TopDirectoryOnly);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void EnumerateJsonFiles_MissingDirectory_ReturnsEmpty()
    {
        var missing = Path.Combine(DataDirectory, "does_not_exist_12345");
        var result = CatalogFileSystem.EnumerateJsonFiles(new FileSystemIO(), missing, SearchOption.TopDirectoryOnly);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void EnumerateJsonFiles_TopDirectoryOnly_ReturnsOnlyRootJsonFiles()
    {
        var files = CatalogFileSystem.EnumerateJsonFiles(new FileSystemIO(), DataDirectory, SearchOption.TopDirectoryOnly);

        Assert.NotNull(files);
        Assert.NotEmpty(files);
        Assert.All(files, f => Assert.EndsWith(".json", f));
        Assert.All(files, f =>
        {
            var dir = Path.GetDirectoryName(f);
            Assert.False(string.IsNullOrEmpty(dir) || !string.Equals(dir, DataDirectory, StringComparison.OrdinalIgnoreCase),
                "TopDirectoryOnly should not recurse: " + f);
        });
        Assert.Contains(files, f => Path.GetFileName(f).Equals("items.json", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void EnumerateJsonFiles_Recursive_ReturnsNestedJsonFiles()
    {
        var files = CatalogFileSystem.EnumerateJsonFiles(new FileSystemIO(), DataDirectory, SearchOption.AllDirectories);

        Assert.NotNull(files);
        Assert.NotEmpty(files);
        Assert.All(files, f => Assert.EndsWith(".json", f));

        var nested = files.Where(f => !string.Equals(Path.GetDirectoryName(f), DataDirectory, StringComparison.OrdinalIgnoreCase)).ToList();
        Assert.True(nested.Count > 0, "recursive search should find at least one nested JSON file");
    }

    [Fact]
    public void EnumerateJsonFiles_NonFileSystemIOAdapter_FallsBackToBCL()
    {
        var fallback = new FallbackFileIO(DataDirectory);
        var result = CatalogFileSystem.EnumerateJsonFiles(fallback, DataDirectory, SearchOption.TopDirectoryOnly);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.All(result, f => Assert.EndsWith(".json", f));
        Assert.Contains(result, f => Path.GetFileName(f).Equals("items.json", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void EnumerateJsonFiles_SearchOptionIsRespected()
    {
        var top = CatalogFileSystem.EnumerateJsonFiles(new FileSystemIO(), DataDirectory, SearchOption.TopDirectoryOnly);
        var all = CatalogFileSystem.EnumerateJsonFiles(new FileSystemIO(), DataDirectory, SearchOption.AllDirectories);

        Assert.NotNull(top);
        Assert.NotNull(all);
        Assert.True(top.Length <= all.Length, "top-level should be a subset of recursive");
    }

    [Fact]
    public void EnumerateJsonFiles_DistinctPaths_NoDuplicates()
    {
        var result = CatalogFileSystem.EnumerateJsonFiles(new FileSystemIO(), DataDirectory, SearchOption.AllDirectories);

        Assert.NotNull(result);
        Assert.Equal(result.Length, result.Distinct(StringComparer.OrdinalIgnoreCase).Count());
    }

    [Fact]
    public void EnumerateJsonFiles_EmptyDirectory_ReturnsEmpty()
    {
        string emptyDir;
        try
        {
            emptyDir = Path.Combine(Path.GetTempPath(), "ashfall_catalogfs_test_" + Path.GetRandomFileName());
            Directory.CreateDirectory(emptyDir);
        }
        catch (Exception ex)
        {
            Assert.True(false, "Could not create temp dir for CatalogFileSystem test: " + ex.Message);
            return;
        }

        try
        {
            var result = CatalogFileSystem.EnumerateJsonFiles(new FileSystemIO(), emptyDir, SearchOption.AllDirectories);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
        finally
        {
            try { Directory.Delete(emptyDir, true); } catch { }
        }
    }

    private sealed class FallbackFileIO : IFileIO
    {
        private readonly string _root;

        public FallbackFileIO(string root) => _root = root;

        public string Combine(params string[] parts) => Path.Combine(parts);
        public bool FileExists(string path) => File.Exists(path);
        public bool DirectoryExists(string path) => Directory.Exists(path);
        public string ReadAllText(string path) => File.ReadAllText(path);
        public void WriteAllText(string path, string contents) => File.WriteAllText(path, contents);
        public void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
