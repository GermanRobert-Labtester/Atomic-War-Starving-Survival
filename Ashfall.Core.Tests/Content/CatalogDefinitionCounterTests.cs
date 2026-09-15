// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core.Content;
using Xunit;

namespace Ashfall.Core.Tests.Content
{
    public sealed class CatalogDefinitionCounterTests
    {
        [Fact]
        public void CountsBareArrayRecordsWithoutCountingNestedReferences()
        {
            const string json =
                "[{\"secret_id\":\"secret_one\",\"parts\":[{\"item_id\":\"item_nested\"}]}," +
                "{\"secret_id\":\"secret_two\",\"parts\":[]}]";
            var ids = new List<string>();

            int count = CatalogDefinitionCounter.Count(json, ids);

            Assert.Equal(2, count);
            Assert.Equal(new[] { "secret_one", "secret_two" }, ids);
        }

        [Fact]
        public void CountsFirstLevelCollectionWithUnderscoreId()
        {
            const string json =
                "{\"schema_version\":1,\"items\":[" +
                "{\"set_id\":\"set_one\",\"parts\":[{\"item_id\":\"nested\"}]}," +
                "{\"set_id\":\"set_two\"}]}";
            var ids = new List<string>();

            int count = CatalogDefinitionCounter.Count(json, ids);

            Assert.Equal(2, count);
            Assert.Equal(new[] { "set_one", "set_two" }, ids);
        }
    }
}
