// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: shared base for catalog integration tests.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            if (!CatalogLocator.TryFindDataDirectory(cwd, out var dataDir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
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

    /// <summary>
    /// Checks several string fields in one catalog pass while retaining every
    /// failing collection/field/entry in the assertion message.  This is for
    /// structural catalog contracts; behavior and query contracts remain
    /// separate tests.
    /// </summary>
    protected static void AssertStringPropertiesPopulated<T>(
        string collectionName,
        IEnumerable<T> entries,
        Func<T, string?> idSelector,
        params (string Name, Func<T, string?> Selector)[] properties)
    {
        var failures = new List<string>();
        foreach (var entry in entries)
        {
            var id = idSelector(entry) ?? "<null-id>";
            foreach (var (name, selector) in properties)
            {
                if (string.IsNullOrWhiteSpace(selector(entry)))
                    failures.Add($"{collectionName}[{id}].{name} is empty");
            }
        }

        Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
    }

    /// <summary>
    /// Checks numeric catalog fields in one pass and reports all invalid
    /// entries instead of stopping at the first assertion.
    /// </summary>
    protected static void AssertPositiveProperties<T>(
        string collectionName,
        IEnumerable<T> entries,
        Func<T, string?> idSelector,
        params (string Name, Func<T, double> Selector)[] properties)
    {
        var failures = new List<string>();
        foreach (var entry in entries)
        {
            var id = idSelector(entry) ?? "<null-id>";
            foreach (var (name, selector) in properties)
            {
                if (selector(entry) <= 0)
                    failures.Add($"{collectionName}[{id}].{name} must be > 0");
            }
        }

        Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
    }

    /// <summary>
    /// Asserts a group of catalog cardinalities together so one contract test
    /// still reports every count drift in the fixture.
    /// </summary>
    protected static void AssertCounts(params (string Name, int Actual, int Expected)[] counts)
    {
        var failures = counts
            .Where(c => c.Actual != c.Expected)
            .Select(c => $"{c.Name}: expected {c.Expected}, got {c.Actual}")
            .ToArray();

        Assert.True(failures.Length == 0, string.Join(Environment.NewLine, failures));
    }
}
