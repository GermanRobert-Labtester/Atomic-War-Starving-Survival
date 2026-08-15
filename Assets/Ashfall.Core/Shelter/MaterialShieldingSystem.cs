using System;
using System.Collections.Generic;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// Engine-agnostic port of Unity's MaterialShieldingSystem (Prompt #127).
    /// Material-specific radiation shielding: Wood=10%, Dirt=50%, Concrete=80%,
    /// Lead=99%. The weakest roofed ceiling governs how much fallout bleeds in.
    /// </summary>
    public class MaterialShieldingSystem
    {
        public enum WallMaterial { None, Wood, Dirt, Concrete, Lead }

        public static readonly Dictionary<WallMaterial, float> Attenuation =
            new Dictionary<WallMaterial, float>
            {
                { WallMaterial.None, 0f },
                { WallMaterial.Wood, 0.1f },
                { WallMaterial.Dirt, 0.5f },
                { WallMaterial.Concrete, 0.8f },
                { WallMaterial.Lead, 0.99f }
            };

        private readonly Dictionary<string, WallMaterial> _roomCeilingMaterials =
            new Dictionary<string, WallMaterial>(StringComparer.Ordinal);

        public event Action<string, WallMaterial> OnCeilingUpgraded;

        public WallMaterial GetCeilingMaterial(string roomId)
        {
            return roomId != null && _roomCeilingMaterials.TryGetValue(roomId, out var m)
                ? m
                : WallMaterial.None;
        }

        public float GetCeilingAttenuation(string roomId)
        {
            return Attenuation.TryGetValue(GetCeilingMaterial(roomId), out float a) ? a : 0f;
        }

        public void UpgradeCeiling(string roomId, WallMaterial material)
        {
            if (string.IsNullOrEmpty(roomId)) return;
            _roomCeilingMaterials[roomId] = material;
            OnCeilingUpgraded?.Invoke(roomId, material);
        }

        /// <summary>
        /// Attenuation fraction (0..1) of the weakest surface-facing ceiling — the one
        /// fallout actually comes through. 0 when nothing is roofed yet.
        /// </summary>
        public float GetWeakestCeilingAttenuation()
        {
            if (_roomCeilingMaterials.Count == 0) return 0f;
            float weakest = 1f;
            foreach (var kv in _roomCeilingMaterials)
            {
                float a = Attenuation.TryGetValue(kv.Value, out float v) ? v : 0f;
                if (a < weakest) weakest = a;
            }
            return weakest;
        }

        /// <summary>Effective rad bleed: ambient rad that penetrates the weakest ceiling.</summary>
        public float GetRadiationBleed(float exteriorRads)
        {
            return exteriorRads * (1f - GetWeakestCeilingAttenuation());
        }

        public MaterialShieldingSave CaptureState()
        {
            var keys = new string[_roomCeilingMaterials.Count];
            var vals = new int[_roomCeilingMaterials.Count];
            int i = 0;
            foreach (var kv in _roomCeilingMaterials)
            {
                keys[i] = kv.Key;
                vals[i] = (int)kv.Value;
                i++;
            }
            return new MaterialShieldingSave { RoomIds = keys, Materials = vals };
        }

        public void RestoreState(MaterialShieldingSave s)
        {
            _roomCeilingMaterials.Clear();
            if (s?.RoomIds == null) return;
            for (int i = 0; i < s.RoomIds.Length; i++)
            {
                if (!string.IsNullOrEmpty(s.RoomIds[i]) && s.Materials != null && i < s.Materials.Length)
                {
                    int m = MathfCompat.Clamp(s.Materials[i], 0, (int)WallMaterial.Lead);
                    _roomCeilingMaterials[s.RoomIds[i]] = (WallMaterial)m;
                }
            }
        }
    }

    [Serializable]
    public class MaterialShieldingSave
    {
        public string[] RoomIds;
        public int[] Materials;
    }
}
