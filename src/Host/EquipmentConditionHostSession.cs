using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Crafting;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for EquipmentConditionSystem.
    /// Wraps the Core equipment pipeline (RegisterItem → UseItem → StartMaintenance)
    /// and forwards StateChanged for host wiring. Engine-agnostic Core authority.
    /// </summary>
    public sealed class EquipmentConditionHostSession
    : HostSessionBase{
        public EquipmentConditionSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public EquipmentConditionHostSession(
            EquipmentConditionSystem system,
            Inventory inventory,
            CraftingSystem crafting)
        {
            System = system
                ?? new EquipmentConditionSystem(new SeededRng(1986), inventory, crafting, new GodotLog());

            System.OnConditionChanged += _ => RaiseStateChanged();
            System.OnMaintenanceCompleted += _ => RaiseStateChanged();
            System.OnEquipmentChanged += () => RaiseStateChanged();
        }

        public ActionResult RegisterItem(string instanceId, string itemId, string ownerId, EquipmentFamily family, float maxCondition = 100f)
        {
            var res = System.RegisterItem(instanceId, itemId, ownerId, family, maxCondition);
            if (res.IsSuccess)
            {
                LastEvent = $"Equipment registered: {itemId} ({instanceId})";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult UseItem(string instanceId, float wearAmount = 1f)
        {
            var res = System.UseItem(instanceId, wearAmount);
            if (res.IsSuccess)
            {
                LastEvent = $"Equipment used: {instanceId} (-{wearAmount} wear)";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult StartMaintenance(string instanceId, string stationId, MaintenanceType type, List<string> requiredParts)
        {
            var res = System.StartMaintenance(instanceId, stationId, type, requiredParts);
            if (res.IsSuccess)
            {
                LastEvent = $"Maintenance started: {instanceId} ({type})";
                RaiseStateChanged();
            }
            return res;
        }

        public float GetSlipRisk(string instanceId) => System.GetSlipRisk(instanceId);

        public void TickDay(int day)
        {
            System.TickDay(day);
            RaiseStateChanged();
        }

        public override void Save()
        {
            if (!IsDirty) return;
            EquipmentConditionSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }

    /// <summary>
    /// EquipmentConditionSaveStore save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). This
    /// shelter-batch section ships the legacy
    /// &lt;c&gt;{ SchemaVersion, State, Checksum }&lt;/c&gt; envelope, preserved
    /// byte-for-byte by the Core &lt;see cref="SchemaVersionedEnvelope{T}"/&gt;
    /// adapter (presence-only checksum, legacy bare-state fallback); path
    /// resolution, atomic write, and error handling live in the service.
    /// </summary>
    public static class EquipmentConditionSaveStore
    {
        public const string FileName = "equipment_condition_save.json";
        public const string SectionName = "equipment_condition";

        private static readonly SaveStore<EquipmentConditionState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(EquipmentConditionSaveStore),
            SchemaVersionedEnvelope<EquipmentConditionState>.Encode,
            SchemaVersionedEnvelope<EquipmentConditionState>.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        public static bool TrySave(EquipmentConditionState state) => s_store.TrySave(state);

        public static EquipmentConditionState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(EquipmentConditionState state) => s_store.CapturePersisted(state);

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(EquipmentConditionState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static EquipmentConditionState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(EquipmentConditionState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static EquipmentConditionState? TryRestore(string json) => s_store.RestoreBare(json);
    }
}
