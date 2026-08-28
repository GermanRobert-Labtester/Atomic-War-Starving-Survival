using System;

namespace Ashfall.Core.Combat
{
    /// <summary>
    /// Bridge between the persisted equipment-condition authority
    /// (<see cref="EquipmentConditionSystem"/> — one stored condition per
    /// instance, 0–100) and combat's <see cref="WeaponConditionSystem"/>
    /// mechanics (<see cref="WeaponInstanceState.ConditionPct"/>, 0–1).
    ///
    /// The bridge owns NO durability state of its own: it projects the
    /// authority into combat tokens, converts units, and writes combat wear
    /// back through the authority — so weapon condition has exactly one
    /// persisted home and expedition readiness reads that same home.
    /// </summary>
    public static class WeaponEquipmentBridge
    {
        /// <summary>
        /// Find the best-condition Weapon-family equipment instance matching a
        /// combat weapon id (optionally constrained to one owner). Null when
        /// the authority tracks no such weapon — callers keep their fallback.
        /// </summary>
        public static EquipmentInstance? FindWeaponFor(EquipmentConditionSystem? equipment, string weaponId, string ownerId = "")
        {
            if (equipment?.State?.items == null || string.IsNullOrEmpty(weaponId)) return null;
            EquipmentInstance? best = null;
            foreach (var item in equipment.State.items)
            {
                if (item.family != EquipmentFamily.Weapon) continue;
                if (!string.IsNullOrEmpty(ownerId) &&
                    !string.Equals(item.ownerId, ownerId, StringComparison.Ordinal))
                    continue;
                if (!string.Equals(item.itemId, weaponId, StringComparison.Ordinal)) continue;
                if (best == null || item.condition > best.condition)
                    best = item;
            }
            return best;
        }

        /// <summary>
        /// Project the persisted authority into combat's weapon token. When no
        /// equipment instance exists, returns a pristine token carrying no
        /// instance binding — combat behavior is then unchanged.
        /// </summary>
        public static WeaponInstanceState ToCombatInstance(
            EquipmentConditionSystem? equipment,
            string weaponId,
            string ownerSurvivorId)
        {
            var inst = FindWeaponFor(equipment, weaponId, ownerSurvivorId);
            float pct = inst != null ? Math.Clamp(inst.condition / 100f, 0f, 1f) : 1f;
            return new WeaponInstanceState
            {
                InstanceId = inst?.instanceId ?? string.Empty,
                WeaponId = weaponId,
                OwnerSurvivorId = ownerSurvivorId,
                ConditionPct = pct,
            };
        }

        /// <summary>Write combat wear back through the authority (0–1 → 0–100).</summary>
        public static void ApplyWear(EquipmentConditionSystem? equipment, string instanceId, float degrade)
        {
            if (equipment == null || string.IsNullOrEmpty(instanceId) || degrade <= 0f) return;
            equipment.UseItem(instanceId, degrade * 100f);
        }

        /// <summary>
        /// Sync one combat token's condition delta back to its equipment
        /// instance after combat resolves — the single write-back point, so
        /// combat-core code needs no equipment hooks.
        /// </summary>
        public static void SyncAfterCombat(
            EquipmentConditionSystem? equipment,
            WeaponInstanceState? weapon,
            float conditionPctBefore)
        {
            if (equipment == null || weapon == null || string.IsNullOrEmpty(weapon.InstanceId)) return;
            float delta = conditionPctBefore - weapon.ConditionPct;
            if (delta > 0f)
                ApplyWear(equipment, weapon.InstanceId, delta);
        }

        /// <summary>
        /// Expedition readiness 0..1 from the persisted authority: unusable ⇒ 0,
        /// otherwise degraded by the authority's jam/slip risk. Returns 1 when
        /// no authority or instance is given (foot/sidearm baseline).
        /// </summary>
        public static float Readiness(EquipmentConditionSystem? equipment, string? instanceId)
        {
            if (equipment == null || string.IsNullOrEmpty(instanceId)) return 1f;
            if (!equipment.IsUsable(instanceId)) return 0f;
            float jam = equipment.GetJamRisk(instanceId);
            float slip = equipment.GetSlipRisk(instanceId);
            return Math.Clamp(1f - (jam + slip) / 2f, 0f, 1f);
        }

        /// <summary>Jam risk 0..1 straight from the authority (0 = no data).</summary>
        public static float JamRisk(EquipmentConditionSystem? equipment, string? instanceId)
        {
            if (equipment == null || string.IsNullOrEmpty(instanceId)) return 0f;
            return equipment.GetJamRisk(instanceId);
        }
    }
}
