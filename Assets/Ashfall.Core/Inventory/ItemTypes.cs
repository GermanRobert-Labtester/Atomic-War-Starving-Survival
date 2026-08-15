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
}
