// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ClothingWarmthSaveStore
// Core State : Ashfall.Core.Inventory.ClothingWarmthSaveState
// Host Caller: Main.ClothingWarmth
// Purpose    : Plan 142 — Clothing & Warmth host session & persistence.
//              DEC-109 ownership: equipped-layer records, wetness, gear
//              condition, and the cold-loss reduction fraction fed to
//              NeedsSystem. Inventory remains physical item custody.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class ClothingWarmthSaveStore
    {
        public const string FileName = "clothing_warmth_save.json";
        public const string SectionName = "clothing_warmth";

        private static readonly SaveStore<ClothingWarmthSaveState> s_store =
            SaveStoreHub.Checksummed<ClothingWarmthSaveState>(FileName, nameof(ClothingWarmthSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(ClothingWarmthSaveState state) => s_store.CaptureBare(state);
        public static ClothingWarmthSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(ClothingWarmthSaveState state) => s_store.TrySave(state);
        public static ClothingWarmthSaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Plan 142 host session. Wraps <see cref="ClothingWarmthSystem"/>.
    /// Exposes equip/unequip, wetness/drying, condition wear, and the
    /// cold-loss reduction fraction consumed by
    /// <c>NeedsSystem.ClothingWarmthReductionProvider</c>.
    /// </summary>
    public sealed class ClothingWarmthHostSession : HostSessionBase
    {
        private readonly ClothingWarmthSystem _system;

        public ClothingWarmthSystem System => _system;
        public string LastEvent { get; private set; } = string.Empty;

        public ClothingWarmthHostSession(ClothingWarmthSaveState? state = null)
        {
            _system = new ClothingWarmthSystem();
            if (state != null) _system.RestoreState(state);
        }

        public static ClothingWarmthHostSession Create(ClothingWarmthSaveState? state = null) =>
            new ClothingWarmthHostSession(state);

        public bool Equip(string survivorId, string itemId, float condition = 1.0f)
        {
            bool ok = _system.EquipClothing(survivorId, itemId, condition);
            if (ok)
            {
                LastEvent = $"Equipped {itemId} on {survivorId}.";
                RaiseStateChanged();
            }
            return ok;
        }

        public bool Unequip(string survivorId, string itemId)
        {
            bool ok = _system.UnequipClothing(survivorId, itemId);
            if (ok)
            {
                LastEvent = $"Unequipped {itemId} from {survivorId}.";
                RaiseStateChanged();
            }
            return ok;
        }

        public void ApplyWetness(string survivorId, float delta) => _system.ApplyWetness(survivorId, delta);
        public void DryClothing(string survivorId, float hours) => _system.DryClothing(survivorId, hours);
        public void DegradeCondition(string survivorId, float wearHours) => _system.DegradeCondition(survivorId, wearHours);

        public float CalculateColdLossReduction(string survivorId) =>
            _system.CalculateColdLossReduction(survivorId);

        public IReadOnlyList<EquippedClothingInstance> GetEquipped(string survivorId) =>
            _system.GetEquipped(survivorId);

        public ClothingWarmthCensus GetCensus() => _system.GetCensus();

        public ClothingWarmthSaveState CaptureState() => _system.CaptureState();

        public void RestoreState(ClothingWarmthSaveState state)
        {
            _system.RestoreState(state);
            LastEvent = "Restored clothing warmth state.";
            RaiseStateChanged();
        }

        public bool TrySave() => ClothingWarmthSaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = ClothingWarmthSaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
