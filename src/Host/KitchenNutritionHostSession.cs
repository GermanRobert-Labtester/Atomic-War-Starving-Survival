using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Survivors;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for KitchenNutritionSystem.
    /// Wraps the Core kitchen pipeline (StartPrepJob → TickDay → ServeMeals)
    /// and forwards StateChanged for host wiring. Engine-agnostic Core authority.
    /// </summary>
    public sealed class KitchenNutritionHostSession
    : HostSessionBase{
        public KitchenNutritionSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public KitchenNutritionHostSession(
            KitchenNutritionSystem system,
            Inventory inventory,
            NeedsSystem needs)
        {
            System = system
                ?? new KitchenNutritionSystem(new SeededRng(1986), inventory, needs, new GodotLog());

            System.OnJobCompleted += _ => RaiseStateChanged();
            System.OnMealServed += serving =>
            {
                LastEvent = $"Meal served: {serving.recipeId} to {serving.survivorId} (+{serving.moraleBonus} morale)";
                RaiseStateChanged();
            };
            System.OnKitchenChanged += () => RaiseStateChanged();
        }

        public ActionResult StartPrepJob(string recipeId, string assignedCookId, Dictionary<string, int> inputRequirements)
        {
            var res = System.StartPrepJob(recipeId, assignedCookId, inputRequirements);
            if (res.IsSuccess)
            {
                LastEvent = $"Prep started: {recipeId} by {assignedCookId}";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult ServeMeal(string survivorId, string recipeId)
        {
            var res = System.ServeMeal(survivorId, recipeId);
            if (res.IsSuccess)
            {
                LastEvent = $"Meal served: {recipeId} to {survivorId}";
                RaiseStateChanged();
            }
            return res;
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
            RaiseStateChanged();
        }

        public override void Save()
        {
            if (!IsDirty) return;
            KitchenNutritionSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }

    /// <summary>
    /// KitchenNutritionSaveStore save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). This
    /// shelter-batch section ships the legacy
    /// &lt;c&gt;{ SchemaVersion, State, Checksum }&lt;/c&gt; envelope, preserved
    /// byte-for-byte by the Core &lt;see cref="SchemaVersionedEnvelope{T}"/&gt;
    /// adapter (presence-only checksum, legacy bare-state fallback); path
    /// resolution, atomic write, and error handling live in the service.
    /// </summary>
    public static class KitchenNutritionSaveStore
    {
        public const string FileName = "kitchen_nutrition_save.json";
        public const string SectionName = "kitchen_nutrition";

        private static readonly SaveStore<KitchenNutritionState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(KitchenNutritionSaveStore),
            SchemaVersionedEnvelope<KitchenNutritionState>.Encode,
            SchemaVersionedEnvelope<KitchenNutritionState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        public static bool TrySave(KitchenNutritionState state) => s_store.TrySave(state);

        public static KitchenNutritionState? TryLoad() => s_store.TryLoad();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(KitchenNutritionState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static KitchenNutritionState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(KitchenNutritionState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static KitchenNutritionState? TryRestore(string json) => s_store.RestoreBare(json);
    }
}
