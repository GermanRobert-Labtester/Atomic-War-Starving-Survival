#nullable enable
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Cooking;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for Plan 136: Wildlife Trapping → Food Pipeline & Cooking System.
    /// Bridges the Core cooking domain authority to inventory, campaign daily lifecycle,
    /// and save persistence without shadow stores.
    /// </summary>
    public sealed class CookingHostSession : HostSessionBase
    {
        public const string CatalogRecipesFile = "recipes_cooking.json";

        public CookingSystem System { get; }
        public ICookingSource? Source { get; set; }
        public InventoryHostSession? InventorySession { get; private set; }
        public bool CatalogLoaded { get; private set; }
        public List<string> LoadErrors { get; } = new List<string>();

        public CookingCensus Census => System.GetCensus();

        public CookingHostSession(CookingSystem? system = null)
        {
            System = system ?? new CookingSystem();

            System.OnCookingCompletedSeam += op => RaiseStateChanged();
            System.OnSkillLevelUpSeam += lvl => RaiseStateChanged();
            System.OnRecipeDiscoveredSeam += id => RaiseStateChanged();
        }

        public void BindInventory(InventoryHostSession? inventorySession)
        {
            InventorySession = inventorySession;
            if (inventorySession != null)
            {
                Source = new InventoryCookingSource(inventorySession.Inventory);
            }
        }

        public bool LoadAuthoredRecipes(string dataDirectory, IFileIO files)
        {
            if (files == null) throw new ArgumentNullException(nameof(files));
            LoadErrors.Clear();

            var result = CookingRecipeCatalogLoader.Load(dataDirectory, files);
            if (!result.Success)
            {
                LoadErrors.AddRange(result.Errors);
                CatalogLoaded = false;
                return false;
            }

            System.BindValidatedRecipes(result.Recipes);
            CatalogLoaded = true;
            RaiseStateChanged();
            return true;
        }

        public ActionResult StartCooking(
            string recipeId,
            string cookId = "cook_survivor",
            string equipmentType = "improvised_stove",
            float currentMinute = 0f)
        {
            var res = System.StartCooking(recipeId, cookId, equipmentType, Source, currentMinute);
            if (res.IsSuccess)
            {
                RaiseStateChanged();
            }
            return res;
        }

        public int ProgressCooking(float deltaMinutes)
        {
            int completed = System.ProgressCooking(deltaMinutes, Source);
            if (completed > 0)
            {
                RaiseStateChanged();
            }
            return completed;
        }

        public ActionResult CancelCooking(string operationId)
        {
            var res = System.CancelCooking(operationId);
            if (res.IsSuccess)
            {
                RaiseStateChanged();
            }
            return res;
        }

        public int TickDay(int currentDay, float dayMinutes = 120f)
        {
            int completed = ProgressCooking(dayMinutes);
            return completed;
        }

        public CookingState CaptureState() => System.CaptureState();

        public void RestoreState(CookingState? state)
        {
            System.RestoreState(state);
            RaiseStateChanged();
        }

        public void RestoreFromPayload(string json)
        {
            var state = CookingSaveStore.RestoreBare(json);
            RestoreState(state);
        }
    }

    /// <summary>
    /// Save store for the "cooking" section (Plan 136).
    /// Uses checksummed JSON envelope with schema versioning.
    /// </summary>
    public static class CookingSaveStore
    {
        public const string SectionName = "cooking";
        public const string FileName = "cooking_save.json";

        private static readonly SaveStore<CookingState> s_store =
            SaveStoreHub.Checksummed<CookingState>(FileName, SaveSectionRegistry.ExpandedShelterLifecycleGroup);

        public static bool TrySave(CookingState state) => s_store.TrySave(state);

        public static CookingState? TryLoad() => s_store.TryLoad();

        public static string CapturePersisted(CookingState state) => s_store.CapturePersisted(state);

        public static string CaptureBare(CookingState state) => s_store.CaptureBare(state);

        public static CookingState? RestoreBare(string json) => s_store.RestoreBare(json);
    }
}
