// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: shared base for catalog integration tests.

using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Ashfall.Core.Tests;

/// <summary>
/// Base class for catalog load tests. Provides:
/// - Cached data-directory resolution via <see cref="CatalogLocator.TryFindDataDirectory"/>
/// - Common null/empty assertions
/// </summary>
public abstract class CatalogTestBase
{
    /// <summary>
    /// Resolves the canonical data directory once per test class.
    /// Falls back to <see cref="Directory.GetCurrentDirectory"/> if the
    /// data directory cannot be located.
    /// </summary>
    protected static string DataDirectory
    {
        get
        {
            var cwd = Directory.GetCurrentDirectory();
            CatalogLocator.TryFindDataDirectory(cwd, out var dataDir);
            return string.IsNullOrEmpty(dataDir) ? cwd : dataDir;
        }
    }

    /// <summary>
    /// Asserts that a catalog list is non-null and contains the expected number of entries.
    /// </summary>
    protected static void AssertCount<T>(IEnumerable<T>? list, int expected)
    {
        Assert.NotNull(list);
        Assert.Equal(expected, new List<T>(list).Count);
    }

    /// <summary>
    /// Asserts that every entry in a list has a non-null, non-whitespace string property.
    /// </summary>
    protected static void AssertAllStringsPopulated<T>(IEnumerable<T> list, Func<T, string> selector)
    {
        foreach (var item in list)
        {
            var value = selector(item);
            Assert.False(string.IsNullOrWhiteSpace(value));
        }
    }

    /// <summary>
    /// Asserts that every entry in a list has a numeric property greater than zero.
    /// </summary>
    protected static void AssertAllPositive<T>(IEnumerable<T> list, Func<T, double> selector)
    {
        foreach (var item in list)
        {
            Assert.True(selector(item) > 0);
        }
    }
}
