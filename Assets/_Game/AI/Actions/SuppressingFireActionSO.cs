using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// Prompt #184 — Suppressing Fire: waste ammo to pin raiders for 2 hours.
    /// Unlocked only when the survivor has perk_suppressing_fire (50+ rounds expended).
    /// </summary>
    [CreateAssetMenu(fileName = "Action_SuppressingFire", menuName = "ASHFALL/AI/Suppressing Fire Action")]
    public class SuppressingFireActionSO : SurvivorAction
    {
        public SuppressingFireActionSO()
        {
            id = "action_suppressing_fire";
            displayName = "Suppressing Fire";
            description = "Burn five rounds. Pin them at the stairwell. Buy time for the hatch.";
            basePriority = 0.12f;
            progressionDiscipline = "combat";
            progressionXp = 4f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null || !context.Survivor.IsAlive) return 0f;
            if (context.Survivor.CannotFight) return 0f;
            if (context.CombatPerks == null
                || !context.CombatPerks.CanUseSuppressingFire(context.Survivor))
                return 0f;

            // Only useful when a raid is imminent / hatch is under pressure.
            float threat = context.RaidThreatLevel;
            if (context.HatchDefense != null)
            {
                if (context.HatchDefense.IsRaidHalted) return 0f;
                if (context.CurrentDay >= HatchDefenseSystem.RaidUnlockDay)
                    threat = Mathf.Max(threat, 0.25f);
                float sec = context.HatchDefense.GetShelterSecurity();
                if (sec < 35f) threat = Mathf.Max(threat, 0.4f);
            }

            if (threat <= 0.05f) return 0f;

            // Need ammo in inventory
            if (context.Inventory == null
                || CountAmmo(context.Inventory) < CombatPerkSystem.SuppressingFireAmmoCost)
                return 0f;

            return Mathf.Clamp01(0.2f + threat * 0.6f);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || !context.Survivor.IsAlive) return;
            if (context.CombatPerks == null
                || !context.CombatPerks.CanUseSuppressingFire(context.Survivor))
                return;
            if (context.Inventory == null) return;

            if (!ConsumeAmmo(context.Inventory, CombatPerkSystem.SuppressingFireAmmoCost))
                return;

            context.CombatPerks.RecordAmmoExpended(
                context.Survivor,
                CombatPerkSystem.SuppressingFireAmmoCost,
                context.CurrentDay);

            context.HatchDefense?.ApplySuppressingFireHalt(
                CombatPerkSystem.SuppressingFireHaltHours);

            context.Survivor.State = SurvivorState.Working;
            if (context.NeedsSystem != null)
                context.NeedsSystem.Modify(context.Survivor, NeedKind.Fatigue, 8f);
            else
                context.Survivor.Needs.Fatigue = Mathf.Clamp(context.Survivor.Needs.Fatigue + 8f, 0f, 100f);
        }

        public static int CountAmmo(Inventory.Inventory inv)
        {
            if (inv?.Slots == null) return 0;
            int total = 0;
            for (int i = 0; i < inv.Slots.Count; i++)
            {
                var slot = inv.Slots[i];
                if (slot?.Item == null || slot.Amount <= 0) continue;
                if (HatchDefenseSystem.IsAmmoItem(slot.Item)
                    || HatchDefenseSystem.IsAmmoId(slot.Item.id))
                    total += slot.Amount;
            }
            return total;
        }

        public static bool ConsumeAmmo(Inventory.Inventory inv, int amount)
        {
            if (inv?.Slots == null || amount <= 0) return false;
            int remaining = amount;
            for (int i = 0; i < inv.Slots.Count && remaining > 0; i++)
            {
                var slot = inv.Slots[i];
                if (slot?.Item == null) continue;
                if (!HatchDefenseSystem.IsAmmoItem(slot.Item)
                    && !HatchDefenseSystem.IsAmmoId(slot.Item.id))
                    continue;
                int take = Mathf.Min(remaining, slot.Amount);
                if (take <= 0) continue;
                if (!inv.Remove(slot.Item, take)) continue;
                remaining -= take;
            }
            return remaining <= 0;
        }
    }
}
