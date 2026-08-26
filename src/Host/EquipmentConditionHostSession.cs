using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Crafting;

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

    [Serializable]
    public sealed class EquipmentConditionHostSave
    {
        public string SchemaVersion { get; set; } = "1.0";
        public EquipmentConditionState State { get; set; }
        public string Checksum { get; set; } = string.Empty;
    }

    public static class EquipmentConditionSaveStore
    {
        public const string FileName = "equipment_condition_save.json";
        public const string SectionName = "equipment_condition";

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(EquipmentConditionState state)
        {
            return TryCapture(state);
        }

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static EquipmentConditionState? TryRestoreDirect(string json)
        {
            return TryRestore(json);
        }

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(EquipmentConditionState state)
        {
            try
            {
                if (state == null) return string.Empty;
                return s_json.Serialize(state);
            }
            catch (Exception e)
            {
                GD.PrintErr("[EquipmentConditionSaveStore] capture failed: " + e.Message);
                return string.Empty;
            }
        }

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static EquipmentConditionState? TryRestore(string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json)) return null;
                return s_json.Deserialize<EquipmentConditionState>(json);
            }
            catch (Exception e)
            {
                GD.PrintErr("[EquipmentConditionSaveStore] restore failed: " + e.Message);
                return null;
            }
        }

        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath => SaveSlotRoot.Resolve(FileName);
        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(EquipmentConditionState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new EquipmentConditionHostSave { State = state };
                envelope.Checksum = SaveChecksum.Compute(envelope);
                string path = SavePath;
                string? dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.WriteAllText(path, s_json.Serialize(envelope));
                return true;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Equipment] save failed: " + e.Message);
                return false;
            }
        }

        public static EquipmentConditionState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var envelope = s_json.Deserialize<EquipmentConditionHostSave>(raw);
                if (envelope != null && envelope.State != null)
                {
                    if (string.IsNullOrEmpty(envelope.Checksum)) return null;
                    return envelope.State;
                }
                return s_json.Deserialize<EquipmentConditionState>(raw);
            }
            catch (Exception e)
            {
                GD.PrintErr("[Equipment] load failed: " + e.Message);
                return null;
            }
        }
    }
}
