using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>Prompt #127 — Material-specific radiation shielding. Wood=10%, Dirt=50%, Concrete=80%, Lead=99%.</summary>
    public class MaterialShieldingSystem
    {
        public enum WallMaterial { None, Wood, Dirt, Concrete, Lead }
        public static readonly Dictionary<WallMaterial, float> Attenuation = new Dictionary<WallMaterial, float>
        { {WallMaterial.None,0f},{WallMaterial.Wood,0.1f},{WallMaterial.Dirt,0.5f},{WallMaterial.Concrete,0.8f},{WallMaterial.Lead,0.99f} };

        private readonly Dictionary<string, WallMaterial> _roomCeilingMaterials = new Dictionary<string, WallMaterial>();
        public event Action<string, WallMaterial> OnCeilingUpgraded;

        public WallMaterial GetCeilingMaterial(string roomId) =>
            _roomCeilingMaterials.TryGetValue(roomId, out var m) ? m : WallMaterial.None;

        public float GetCeilingAttenuation(string roomId) =>
            Attenuation.TryGetValue(GetCeilingMaterial(roomId), out float a) ? a : 0f;

        public void UpgradeCeiling(string roomId, WallMaterial material)
        {
            _roomCeilingMaterials[roomId] = material;
            OnCeilingUpgraded?.Invoke(roomId, material);
        }

        /// <summary>Effective rad bleed: ambient rad that penetrates the weakest surface-facing ceiling.</summary>
        public float GetRadiationBleed(float exteriorRads)
        {
            float weakestAttenuation = 0f;
            foreach (var kv in _roomCeilingMaterials)
                weakestAttenuation = Mathf.Min(weakestAttenuation, Attenuation.TryGetValue(kv.Value, out float a) ? a : 0f);
            return exteriorRads * (1f - weakestAttenuation);
        }

        public MaterialShieldingSave CaptureState()
        {
            var keys = new string[_roomCeilingMaterials.Count]; var vals = new int[_roomCeilingMaterials.Count]; int i = 0;
            foreach (var kv in _roomCeilingMaterials) { keys[i] = kv.Key; vals[i] = (int)kv.Value; i++; }
            return new MaterialShieldingSave { RoomIds = keys, Materials = vals };
        }
        public void RestoreState(MaterialShieldingSave s)
        {
            _roomCeilingMaterials.Clear();
            if (s?.RoomIds == null) return;
            for (int i = 0; i < s.RoomIds.Length; i++)
                if (!string.IsNullOrEmpty(s.RoomIds[i]) && s.Materials != null && i < s.Materials.Length)
                    _roomCeilingMaterials[s.RoomIds[i]] = (WallMaterial)Mathf.Clamp(s.Materials[i], 0, 4);
        }
    }
    [Serializable] public class MaterialShieldingSave { public string[] RoomIds; public int[] Materials; }
}
