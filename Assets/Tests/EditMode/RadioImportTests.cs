using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using AtomicWar._Game.Data;
using AtomicWar._Game.Editor;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// radio.json had no importer at all until this landed, so its 12 broadcasts
    /// never became assets and RadioCatalogSO had nothing to aggregate.
    /// </summary>
    [TestFixture]
    public class RadioImportTests
    {
        const string TestId = "radio_test_signal";
        const string TestAssetPath = "Assets/_Game/Data/Generated/Radio/" + TestId + ".asset";

        [TearDown]
        public void RemoveGeneratedTestAsset()
        {
            // ImportRadio writes a real asset; do not leave it in the project.
            if (AssetDatabase.LoadAssetAtPath<RadioBroadcastSO>(TestAssetPath) != null)
                AssetDatabase.DeleteAsset(TestAssetPath);
        }

        [Test]
        public void ImportRadio_MapsEveryJsonFieldOntoTheScriptableObject()
        {
            var json = new List<JsonDataImporter.RadioJson>
            {
                new JsonDataImporter.RadioJson
                {
                    id = TestId,
                    minDay = 4,
                    maxDay = 9,
                    message = "Static. Then nothing.",
                    triggerEventId = "filter_failure"
                }
            };

            List<RadioBroadcastSO> assets = JsonDataImporter.ImportRadio(json);

            Assert.AreEqual(1, assets.Count);
            Assert.AreEqual(TestId, assets[0].id);
            Assert.AreEqual(4, assets[0].minDay);
            Assert.AreEqual(9, assets[0].maxDay);
            Assert.AreEqual("Static. Then nothing.", assets[0].message);
            Assert.AreEqual("filter_failure", assets[0].triggerEventId);
        }

        [Test]
        public void ImportRadio_IsIdempotent_ReusingTheSameAssetForARepeatedId()
        {
            var json = new List<JsonDataImporter.RadioJson>
            {
                new JsonDataImporter.RadioJson { id = TestId, minDay = 1, message = "First." }
            };

            var first = JsonDataImporter.ImportRadio(json);

            json[0].minDay = 7;
            json[0].message = "Second.";
            var second = JsonDataImporter.ImportRadio(json);

            Assert.AreSame(first[0], second[0], "a repeated id must update the existing asset, not create a new one");
            Assert.AreEqual(7, second[0].minDay);
            Assert.AreEqual("Second.", second[0].message);
        }
    }
}
