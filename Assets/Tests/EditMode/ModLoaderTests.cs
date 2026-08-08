using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompt #864 — mod loader: Initialize, Scan, Load, Capture/Restore, SaveSystem.
    /// </summary>
    [TestFixture]
    public class ModLoaderTests
    {
        private string _modsRoot;
        private string _modFolder;

        [SetUp]
        public void SetUp()
        {
            System_ModLoader.Active?.ClearActiveIfSelf();
            // Clear via a throwaway if Active points elsewhere after tests.
            var clear = new System_ModLoader();
            clear.SetAsActive();
            clear.ClearActiveIfSelf();

            _modsRoot = Path.Combine(Path.GetTempPath(), "ashfall_mods_" + Guid.NewGuid().ToString("N"));
            _modFolder = Path.Combine(_modsRoot, "test_pack");
            Directory.CreateDirectory(_modFolder);
            File.WriteAllText(Path.Combine(_modFolder, "manifest.json"),
                "{\n  \"id\": \"test_pack\",\n  \"name\": \"Test Pack\",\n  \"version\": \"1.0\"\n}\n");
            File.WriteAllText(Path.Combine(_modFolder, "items.json"),
                "[\n  { \"id\": \"mod_iodine\", \"displayName\": \"Mod Iodine\" }\n]\n");
            File.WriteAllText(Path.Combine(_modFolder, "recipes.json"),
                "[\n  { \"id\": \"mod_recipe_filter\" }\n]\n");
        }

        [TearDown]
        public void TearDown()
        {
            System_ModLoader.Active?.ClearActiveIfSelf();
            try
            {
                if (!string.IsNullOrEmpty(_modsRoot) && Directory.Exists(_modsRoot))
                    Directory.Delete(_modsRoot, true);
            }
            catch { /* best-effort */ }
        }

        [Test]
        public void Initialize_SetsPathAndClearsState()
        {
            var loader = new System_ModLoader();
            loader.Initialize(_modsRoot);

            Assert.AreEqual(_modsRoot, loader.ModsFolderPath);
            Assert.AreEqual("system_mod_loader", loader.SystemId);
            Assert.AreEqual(0, loader.GetLoadedMods().Count);
            Assert.AreEqual(0, loader.OverrideCount);
            Assert.AreEqual(System_ModLoader.DefaultPriorityModFirst, loader.OverridePriority);
        }

        [Test]
        public void ScanForMods_DiscoversSubfolders()
        {
            var loader = new System_ModLoader();
            loader.Initialize(_modsRoot);

            string discovered = null;
            loader.OnModDiscovered += name => discovered = name;

            var found = loader.ScanForMods();
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual("test_pack", found[0]);
            Assert.AreEqual("test_pack", discovered);
        }

        [Test]
        public void LoadMod_RegistersOverrides_GetOverrideDataWorks()
        {
            var loader = new System_ModLoader();
            loader.Initialize(_modsRoot);

            int loadedItems = -1;
            string loadedName = null;
            loader.OnModLoaded += (name, count) =>
            {
                loadedName = name;
                loadedItems = count;
            };

            loader.LoadMod("test_pack");

            Assert.AreEqual("test_pack", loadedName);
            Assert.AreEqual(2, loadedItems); // items + recipes (not manifest)
            Assert.AreEqual(1, loader.GetLoadedMods().Count);
            Assert.IsTrue(loader.HasOverride("test_pack/items"));
            Assert.IsTrue(loader.HasOverride("test_pack/recipes"));

            string itemsJson = loader.GetOverrideData("test_pack/items");
            Assert.IsNotNull(itemsJson);
            Assert.IsTrue(itemsJson.Contains("mod_iodine"));

            // File-stem lookup used by importers.
            string byStem = loader.FindOverrideByFileStem("items");
            Assert.IsNotNull(byStem);
            Assert.IsTrue(byStem.Contains("mod_iodine"));
        }

        [Test]
        public void ResolveJsonText_PrefersActiveOverride()
        {
            var loader = new System_ModLoader();
            loader.Initialize(_modsRoot);
            loader.LoadMod("test_pack");
            loader.SetAsActive();

            string basePath = Path.Combine(Path.GetTempPath(), "no_such_base_items.json");
            string resolved = System_ModLoader.ResolveJsonText("items", basePath);
            Assert.IsNotNull(resolved);
            Assert.IsTrue(resolved.Contains("mod_iodine"));

            // Without Active, missing base returns null.
            loader.ClearActiveIfSelf();
            Assert.IsNull(System_ModLoader.ResolveJsonText("items", basePath));
        }

        [Test]
        public void CaptureRestore_ReloadsOverridesFromDisk()
        {
            var a = new System_ModLoader();
            a.Initialize(_modsRoot);
            a.LoadMod("test_pack");
            Assert.AreEqual(2, a.OverrideCount);

            var save = a.CaptureState();
            Assert.AreEqual("system_mod_loader", save.system_id);
            Assert.AreEqual(_modsRoot, save.mods_folder_path);
            Assert.AreEqual(1, save.loaded_mods.Count);
            Assert.AreEqual("test_pack", save.loaded_mods[0]);

            // Mutate after capture — snapshot must stay frozen.
            a.Initialize(string.Empty);
            Assert.AreEqual(0, a.OverrideCount);
            Assert.AreEqual(1, save.loaded_mods.Count);

            var b = new System_ModLoader();
            b.RestoreState(save);
            Assert.AreEqual(_modsRoot, b.ModsFolderPath);
            Assert.AreEqual(1, b.GetLoadedMods().Count);
            Assert.AreEqual(2, b.OverrideCount);
            Assert.IsTrue(b.HasOverride("test_pack/items"));

            b.RestoreState(null);
            Assert.AreEqual(0, b.GetLoadedMods().Count);
            Assert.AreEqual(0, b.OverrideCount);
        }

        [Test]
        public void SaveSystemAdapter_ModLoaderSlot_RoundTrip()
        {
            string dir = SaveSystemTestFactory.TempDir("modloader");
            try
            {
                var loaderA = new System_ModLoader();
                loaderA.Initialize(_modsRoot);
                loaderA.LoadMod("test_pack");

                SaveSystem Make(System_ModLoader loader) =>
                    SaveSystemTestFactory.MakeSave(dir, ss => { ss.SetModLoaderSystem(loader); });

                Assert.IsTrue(Make(loaderA).Save("mod_slot"));

                var loaderB = new System_ModLoader();
                Assert.IsTrue(Make(loaderB).Load("mod_slot"));

                Assert.AreEqual(_modsRoot, loaderB.ModsFolderPath);
                Assert.AreEqual(1, loaderB.GetLoadedMods().Count);
                Assert.AreEqual("test_pack", loaderB.GetLoadedMods()[0]);
                Assert.IsTrue(loaderB.HasOverride("test_pack/items"));
                Assert.AreEqual("system_mod_loader", loaderB.SystemId);
            }
            finally
            {
                try { if (Directory.Exists(dir)) Directory.Delete(dir, true); } catch { /* best-effort */ }
            }
        }
    }
}
