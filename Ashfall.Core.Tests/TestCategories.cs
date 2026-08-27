// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Canonical category names for xUnit traits across Ashfall.Core.Tests.
    /// Filter via: dotnet test --filter "Category=Unit|Category=Save|Category=Data|Category=Integration"
    /// </summary>
    public static class TestCategories
    {
        public const string Category = "Category";

        public const string Unit = "Unit";
        public const string Save = "Save";
        public const string Data = "Data";
        public const string Integration = "Integration";
    }

    /// <summary>
    /// Marks a test or test class as a fast, in-memory domain Unit test.
    /// </summary>
    [TraitDiscoverer("Ashfall.Core.Tests.UnitTestDiscoverer", "Ashfall.Core.Tests")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class UnitTestAttribute : Attribute, ITraitAttribute { }

    /// <summary>
    /// Marks a test or test class as a Save/Load, codec, envelope, or serialization test.
    /// </summary>
    [TraitDiscoverer("Ashfall.Core.Tests.SaveTestDiscoverer", "Ashfall.Core.Tests")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class SaveTestAttribute : Attribute, ITraitAttribute { }

    /// <summary>
    /// Marks a test or test class as a Data authority, JSON schema, or Catalog integrity test.
    /// </summary>
    [TraitDiscoverer("Ashfall.Core.Tests.DataTestDiscoverer", "Ashfall.Core.Tests")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class DataTestAttribute : Attribute, ITraitAttribute { }

    /// <summary>
    /// Marks a test or test class as an end-to-end multi-system or campaign Integration test.
    /// </summary>
    [TraitDiscoverer("Ashfall.Core.Tests.IntegrationTestDiscoverer", "Ashfall.Core.Tests")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class IntegrationTestAttribute : Attribute, ITraitAttribute { }

    public sealed class UnitTestDiscoverer : ITraitDiscoverer
    {
        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            yield return new KeyValuePair<string, string>(TestCategories.Category, TestCategories.Unit);
        }
    }

    public sealed class SaveTestDiscoverer : ITraitDiscoverer
    {
        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            yield return new KeyValuePair<string, string>(TestCategories.Category, TestCategories.Save);
        }
    }

    public sealed class DataTestDiscoverer : ITraitDiscoverer
    {
        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            yield return new KeyValuePair<string, string>(TestCategories.Category, TestCategories.Data);
        }
    }

    public sealed class IntegrationTestDiscoverer : ITraitDiscoverer
    {
        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            yield return new KeyValuePair<string, string>(TestCategories.Category, TestCategories.Integration);
        }
    }
}
