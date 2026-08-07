using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class GameAssetServiceTests
    {
        /// <summary>Records every probe so the tests can assert on lookup count, not just results.</summary>
        private sealed class FakeProvider : IGameAssetProvider
        {
            public readonly Dictionary<string, Object> Assets = new Dictionary<string, Object>();
            public readonly List<string> Probes = new List<string>();

            public T Load<T>(string path) where T : Object
            {
                Probes.Add(path);
                return Assets.TryGetValue(path, out var a) ? a as T : null;
            }

            public int ProbesFor(string path) => Probes.FindAll(p => p == path).Count;
        }

        private static Sprite MakeSprite()
        {
            var tex = new Texture2D(2, 2);
            return Sprite.Create(tex, new Rect(0, 0, 2, 2), Vector2.one * 0.5f);
        }

        private FakeProvider _provider;
        private GameAssetService _service;

        [SetUp]
        public void SetUp()
        {
            _provider = new FakeProvider();
            _service = new GameAssetService(_provider) { LogMissingOnce = false };
        }

        [Test]
        public void GetItemIcon_ResolvesThroughTheCanonicalItemPath()
        {
            var sprite = MakeSprite();
            _provider.Assets[GameAssetKeys.ItemIcon("gas_mask")] = sprite;

            Assert.AreSame(sprite, _service.GetItemIcon("gas_mask"));
            Assert.AreEqual("Art/Items/gas_mask", GameAssetKeys.ItemIcon("gas_mask"));
        }

        [Test]
        public void RepeatedLookups_ProbeTheProviderOnlyOnce()
        {
            string path = GameAssetKeys.ItemIcon("geiger_counter");
            _provider.Assets[path] = MakeSprite();

            for (int i = 0; i < 25; i++)
                _service.GetItemIcon("geiger_counter");

            Assert.AreEqual(1, _provider.ProbesFor(path),
                "A cached sprite must not be re-loaded on every draw.");
        }

        [Test]
        public void MissingAsset_IsProbedOnce_ThenServedFromTheNegativeCache()
        {
            string path = GameAssetKeys.ItemIcon("not_yet_drawn");

            for (int i = 0; i < 25; i++)
                Assert.IsNull(_service.GetItemIcon("not_yet_drawn"),
                    "No placeholder is set, so a miss yields null.");

            // This is the important one. Without negative caching, un-authored art costs
            // a real asset probe every frame it is drawn.
            Assert.AreEqual(1, _provider.ProbesFor(path),
                "A known-missing path must not be re-probed.");
        }

        [Test]
        public void MissingAsset_FallsBackToPlaceholder_AndIsRecordedForTheArtWorkList()
        {
            var placeholder = MakeSprite();
            _service.PlaceholderSprite = placeholder;

            Assert.AreSame(placeholder, _service.GetItemIcon("absent_one"));
            Assert.AreSame(placeholder, _service.GetItemIcon("absent_two"));

            Assert.Contains(GameAssetKeys.ItemIcon("absent_one"),
                new List<string>(_service.MissingPaths));
            Assert.AreEqual(2, _service.MissingPaths.Count);
        }

        [Test]
        public void GetSfx_ReturnsNullRatherThanAPlaceholder_WhenNotAuthored()
        {
            _service.PlaceholderSprite = MakeSprite();

            // Silence is a fine stand-in for missing audio; a stand-in *sound* would be
            // actively misleading, so the placeholder must not leak into audio lookups.
            Assert.IsNull(_service.GetSfx("hatch_slam"));
        }

        [Test]
        public void ClearCache_ReleasesAssets_ButKeepsTheMissingRecord()
        {
            string present = GameAssetKeys.ItemIcon("canteen");
            _provider.Assets[present] = MakeSprite();

            _service.GetItemIcon("canteen");
            _service.GetItemIcon("phantom");
            Assert.AreEqual(1, _service.CachedAssetCount);

            _service.ClearCache();
            Assert.AreEqual(0, _service.CachedAssetCount, "Cache must release its references.");
            Assert.AreEqual(1, _service.MissingPaths.Count,
                "A path that was missing is still missing; re-probing it would undo the win.");

            _service.GetItemIcon("phantom");
            Assert.AreEqual(1, _provider.ProbesFor(GameAssetKeys.ItemIcon("phantom")));
        }

        [Test]
        public void NullOrEmptyId_YieldsPlaceholder_WithoutProbing()
        {
            var placeholder = MakeSprite();
            _service.PlaceholderSprite = placeholder;

            Assert.AreSame(placeholder, _service.GetItemIcon(null));
            Assert.AreSame(placeholder, _service.GetItemIcon(""));
            Assert.IsEmpty(_provider.Probes, "An empty id is not worth a lookup.");
        }

        [Test]
        public void IsValidId_AcceptsSnakeCase_AndRejectsPathBreakingIds()
        {
            Assert.IsTrue(GameAssetKeys.IsValidId("rad_away_2"));
            Assert.IsFalse(GameAssetKeys.IsValidId("Rad_Away"), "ids are lowercase");
            Assert.IsFalse(GameAssetKeys.IsValidId("art/items/x"), "separators break the path");
            Assert.IsFalse(GameAssetKeys.IsValidId("gas mask.png"));
            Assert.IsFalse(GameAssetKeys.IsValidId(null));
            Assert.IsFalse(GameAssetKeys.IsValidId(" "));
        }
    }
}
