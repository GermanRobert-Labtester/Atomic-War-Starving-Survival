using NUnit.Framework;
using UnityEditor;
using AtomicWar._Game.Data;
using AtomicWar._Game.Editor;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Ten of GameBootstrap's twelve [SerializeField] data references had no asset
    /// at all: JsonDataImporter writes individual entries but never a catalog.
    ///
    /// These tests run the real generator against the real project, which is the
    /// point -- the generated assets are what Task 3 commits and what the scene
    /// builder wires. Generation is idempotent, so re-running is safe.
    /// </summary>
    [TestFixture]
    public class CatalogGeneratorTests
    {
        const string Root = "Assets/_Game/Data/Generated/Catalogs";
        const string NeedsProfilePath = Root + "/NeedsProfile.asset";

        float _originalHungerPerHour;

        [SetUp]
        public void CaptureTunedValues()
        {
            CatalogGenerator.GenerateAll();
            var profile = AssetDatabase.LoadAssetAtPath<NeedsProfile>(NeedsProfilePath);
            _originalHungerPerHour = profile != null ? profile.hungerPerHour : 2f;
        }

        [TearDown]
        public void RestoreTunedValues()
        {
            // Restore here rather than inline, so a mid-test failure cannot leave
            // the committed profile holding a test value.
            var profile = AssetDatabase.LoadAssetAtPath<NeedsProfile>(NeedsProfilePath);
            if (profile == null) return;

            profile.hungerPerHour = _originalHungerPerHour;
            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();
        }

        [Test]
        public void GenerateAll_CreatesEveryCatalogAndProfileAsset()
        {
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<ItemCatalogSO>($"{Root}/ItemCatalog.asset"));
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<RecipeCatalogSO>($"{Root}/RecipeCatalog.asset"));
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<GameEventCatalogSO>($"{Root}/GameEventCatalog.asset"));
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<LocationCatalogSO>($"{Root}/LocationCatalog.asset"));
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<RadioCatalogSO>($"{Root}/RadioCatalog.asset"));
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<NeedsProfile>($"{Root}/NeedsProfile.asset"));
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<LightProfile>($"{Root}/LightProfile.asset"));
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<SeasonProfile>($"{Root}/SeasonProfile.asset"));
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<WorldPhaseConfigSO>($"{Root}/WorldPhaseConfig.asset"));
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<LootTableSO>($"{Root}/LootTable.asset"));
        }

        [Test]
        public void GenerateAll_PopulatesItemCatalogFromImportedAssets()
        {
            var catalog = AssetDatabase.LoadAssetAtPath<ItemCatalogSO>($"{Root}/ItemCatalog.asset");

            Assert.Greater(catalog.items.Count, 0, "item catalog should aggregate imported ItemDefinition assets");
            Assert.IsNotNull(catalog.GetById("clean_water"), "known seed item should resolve by id");
        }

        [Test]
        public void GenerateAll_DoesNotOverwriteHandTunedProfileValues()
        {
            var profile = AssetDatabase.LoadAssetAtPath<NeedsProfile>(NeedsProfilePath);
            profile.hungerPerHour = 99f;
            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();

            CatalogGenerator.GenerateAll();

            var reloaded = AssetDatabase.LoadAssetAtPath<NeedsProfile>(NeedsProfilePath);
            Assert.AreEqual(99f, reloaded.hungerPerHour,
                "regeneration must not clobber tuned profile values");
        }

        [Test]
        public void GenerateAll_SeedsLootTableValidFromTheEarliestPhase()
        {
            var loot = AssetDatabase.LoadAssetAtPath<LootTableSO>($"{Root}/LootTable.asset");

            Assert.Greater(loot.GetValidEntries(WorldPhase.PreWar).Count, 0,
                "seeded loot must be reachable in the earliest phase");
        }

        /// <summary>
        /// Regression: LocationDefinitionSO, RadioBroadcastSO and SurvivorArchetypeSO
        /// were declared inside another class's file. Unity only links a MonoScript
        /// when the file name matches the class name, so those assets serialized with
        /// m_Script: {fileID: 0} and were invisible to both `t:` search and
        /// LoadAssetAtPath<T> -- the catalogs aggregated zero of them while the assets
        /// sat on disk looking fine.
        /// </summary>
        [Test]
        public void GenerateAll_AggregatesEveryAssetOnDisk_NotSilentlyDroppingUnlinkedTypes()
        {
            AssertCatalogMatchesFolder<LocationCatalogSO>(
                $"{Root}/LocationCatalog.asset", "Locations", c => c.locations.Count);
            AssertCatalogMatchesFolder<RadioCatalogSO>(
                $"{Root}/RadioCatalog.asset", "Radio", c => c.broadcasts.Count);
            AssertCatalogMatchesFolder<ItemCatalogSO>(
                $"{Root}/ItemCatalog.asset", "Items", c => c.items.Count);
        }

        static void AssertCatalogMatchesFolder<T>(string catalogPath, string folderName,
            System.Func<T, int> count) where T : UnityEngine.ScriptableObject
        {
            var dir = $"Assets/_Game/Data/Generated/{folderName}";
            int onDisk = System.IO.Directory.Exists(dir)
                ? System.IO.Directory.GetFiles(dir, "*.asset").Length
                : 0;

            var catalog = AssetDatabase.LoadAssetAtPath<T>(catalogPath);
            Assert.IsNotNull(catalog, $"{catalogPath} should exist");
            Assert.AreEqual(onDisk, count(catalog),
                $"{folderName}: catalog must aggregate every asset on disk");
        }

        [Test]
        public void GenerateAll_IsIdempotent_NotDuplicatingCatalogEntries()
        {
            var catalog = AssetDatabase.LoadAssetAtPath<ItemCatalogSO>($"{Root}/ItemCatalog.asset");
            int countAfterFirstRun = catalog.items.Count;

            CatalogGenerator.GenerateAll();

            var reloaded = AssetDatabase.LoadAssetAtPath<ItemCatalogSO>($"{Root}/ItemCatalog.asset");
            Assert.AreEqual(countAfterFirstRun, reloaded.items.Count,
                "re-running must refresh the list in place, not append");
        }
    }
}
