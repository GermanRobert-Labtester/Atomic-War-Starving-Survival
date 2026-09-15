// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Inventory;

namespace Ashfall.Core
{
    /// <summary>
    /// C2 / Plan 22 (§25.2) — the one shared item-classification authority for
    /// consumable semantics (medical / protective / filter canister).
    /// Membership = authored tags (via <see cref="ItemDefinition.HasTag"/>,
    /// alias-normalized by the loader's lowercasing) OR the item's own type.
    /// Replaces hardcoded per-consumer ID lists; a new consumable is classified
    /// by authoring data, not by editing consumers.
    /// </summary>
    public static class ItemTagCatalog
    {
        public const string TagMedical = "medical";
        public const string TagProtective = "protective";
        public const string TagFilter = "filter";

        /// <summary>Medicine and anti-rad consumables (treatments, craft classification).</summary>
        public static bool IsMedical(ItemDefinition? def)
            => def != null && (def.type == ItemType.Medical || def.HasTag(TagMedical));

        /// <summary>Wearable radiation protection (Plan 21 wear authority families).</summary>
        public static bool IsProtective(ItemDefinition? def)
            => def != null && def.isEquipable
                && (def.radProtection > 0f || def.HasTag(TagProtective));

        /// <summary>Replacement filter canisters (shelter air / water treatment).</summary>
        public static bool IsFilterCanister(ItemDefinition? def)
            => def != null && def.HasTag(TagFilter);
    }
}
