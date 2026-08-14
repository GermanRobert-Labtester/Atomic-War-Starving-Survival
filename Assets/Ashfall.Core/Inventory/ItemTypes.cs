using System;

namespace Ashfall.Core.Inventory
{
    /// <summary>
    /// Canonical item categories. snake_case ids in the JSON data map to these
    /// via the loader. Ported engine-agnostic from Unity's
    /// AtomicWar._Game.Inventory.ItemType.
    /// </summary>
    public enum ItemType
    {
        Food,
        Water,
        IrradiatedWater,
        Medical,
        AntiRad,
        Iodine,
        Protective,
        Tool,
        Fuel,
        Filter,
        Material,
        Trade,
        Comfort,
        Quest,
        Device,
        Weapon,
        Corpse,
        ContaminatedFood,
        Relic
    }

    /// <summary>Equipment slot a wearable item occupies. One item per slot per survivor.</summary>
    public enum EquipSlot
    {
        None,
        Body,
        Head,
        Face,
        Hands,
        Tool,
        Weapon
    }

    /// <summary>Expiration state for medical supplies and food items.</summary>
    public enum ExpirationState
    {
        Fresh,
        Expired,
        Degraded
    }

    /// <summary>
    /// Engine-agnostic stand-in for UnityEngine.Mathf covering every operation the
    /// ported inventory/device code uses. Kept local so Ashfall.Core stays free of
    /// engine imports (AGENTS.md dual-engine rule).
    /// </summary>
    public static class MathfCompat
    {
        public const float Epsilon = 1e-6f;

        public static float Clamp01(float v) => v < 0f ? 0f : (v > 1f ? 1f : v);
        public static float Clamp(float v, float min, float max) => v < min ? min : (v > max ? max : v);
        public static int Max(int a, int b) => a > b ? a : b;
        public static int Min(int a, int b) => a < b ? a : b;
        public static float Max(float a, float b) => a > b ? a : b;
        public static float Min(float a, float b) => a < b ? a : b;
        public static float Lerp(float a, float b, float t) => a + (b - a) * Clamp01(t);
        public static bool Approximately(float a, float b) => Math.Abs(a - b) < Epsilon;
    }
}
