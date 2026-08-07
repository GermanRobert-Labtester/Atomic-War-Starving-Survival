using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Core
{
    /// <summary>One caliber+mod row in the ammo stockpile breakdown.</summary>
    public struct AmmoStockpileRow
    {
        public string ItemId;
        public string CaliberDisplay;
        public string ModLabel;
        public string DisplayName;
        public int Amount;
        public bool IsMilitaryExclusive;
        public float DefensePowerVsLight;
        public float DefensePowerVsMilitary;
    }

    /// <summary>
    /// UI-facing formatters for ammo stockpile, loot tooltips, hatch power preview,
    /// and expedition combat log lines. Pure data — HUD classes stay Core-free.
    /// </summary>
    public partial class Item_AmmoTypes
    {
        public static string ModificationLabel(BulletModification mod)
        {
            switch (mod)
            {
                case BulletModification.SoftLead: return "Soft lead";
                case BulletModification.Fmj: return "FMJ";
                case BulletModification.Jhp: return "JHP";
                case BulletModification.Ap: return "AP";
                case BulletModification.Api: return "API";
                case BulletModification.M855A1: return "M855A1";
                case BulletModification.BoatTail: return "Boat-tail";
                default: return mod.ToString();
            }
        }

        /// <summary>
        /// Inventory tooltip: caliber, mod, craftability, military exclusive badge.
        /// Non-ammo ids return empty (caller keeps default name).
        /// </summary>
        public static string FormatItemTooltip(string itemId)
        {
            if (!TryGetLoad(itemId, out var load)) return string.Empty;

            var sb = new StringBuilder(128);
            sb.Append(load.DisplayName);
            sb.Append(" — ");
            sb.Append(load.CaliberDisplay);
            sb.Append(" / ");
            sb.Append(ModificationLabel(load.Modification));
            sb.Append(" · ");
            sb.Append(load.WeaponClass);

            if (IsExclusiveToFactions(load.Id) || !load.Craftable)
            {
                sb.Append("\n[MILITARY EXCLUSIVE]");
                sb.Append(" Field-only. Not workbench-craftable.");
            }
            else
            {
                sb.Append("\nCivilian load — workbench pressable.");
            }

            if (!string.IsNullOrEmpty(load.Description))
            {
                sb.Append('\n');
                sb.Append(load.Description);
            }
            return sb.ToString();
        }

        public static bool IsMilitaryExclusiveTooltip(string itemId)
        {
            if (!TryGetLoad(itemId, out var load)) return false;
            return IsExclusiveToFactions(load.Id) || !load.Craftable;
        }

        /// <summary>Aggregate ammo stacks by item id from inventory slots.</summary>
        public List<AmmoStockpileRow> BuildStockpileRows(Inventory.Inventory inventory)
        {
            var rows = new List<AmmoStockpileRow>();
            if (inventory?.Slots == null) return rows;

            // Aggregate amounts by ammo id.
            var amounts = new Dictionary<string, int>(StringComparer.Ordinal);
            for (int i = 0; i < inventory.Slots.Count; i++)
            {
                var slot = inventory.Slots[i];
                if (slot?.Item == null || slot.Amount <= 0) continue;
                string id = slot.Item.id;
                if (!IsAmmoItemId(id)) continue;
                // Normalize legacy aliases to catalog id.
                if (TryGetLoad(id, out var load))
                    id = load.Id;
                if (!amounts.ContainsKey(id)) amounts[id] = 0;
                amounts[id] += slot.Amount;
            }

            foreach (var kv in amounts)
            {
                if (!TryGetLoad(kv.Key, out var load)) continue;
                float light = GetAmmoStockpileDefensePower(load.Id, kv.Value, ArmorLightRaider);
                float mil = GetAmmoStockpileDefensePower(load.Id, kv.Value, ArmorMilitary);
                rows.Add(new AmmoStockpileRow
                {
                    ItemId = load.Id,
                    CaliberDisplay = load.CaliberDisplay,
                    ModLabel = ModificationLabel(load.Modification),
                    DisplayName = load.DisplayName,
                    Amount = kv.Value,
                    IsMilitaryExclusive = IsExclusiveToFactions(load.Id) || !load.Craftable,
                    DefensePowerVsLight = light,
                    DefensePowerVsMilitary = mil
                });
            }

            rows.Sort((a, b) =>
            {
                int c = string.Compare(a.CaliberDisplay, b.CaliberDisplay, StringComparison.Ordinal);
                if (c != 0) return c;
                c = string.Compare(a.ModLabel, b.ModLabel, StringComparison.Ordinal);
                if (c != 0) return c;
                return b.Amount.CompareTo(a.Amount);
            });
            return rows;
        }

        /// <summary>Multi-line stockpile breakdown (caliber / mod · count · exclusive tag).</summary>
        public string FormatStockpileBreakdown(Inventory.Inventory inventory, int maxRows = 12)
        {
            var rows = BuildStockpileRows(inventory);
            if (rows.Count == 0) return "AMMO: none stocked.";

            var sb = new StringBuilder(256);
            sb.Append("AMMO STOCKPILE");
            int shown = 0;
            int totalRounds = 0;
            for (int i = 0; i < rows.Count; i++) totalRounds += rows[i].Amount;

            sb.Append(" (").Append(totalRounds).Append(" rds · ").Append(rows.Count).Append(" loads)");
            sb.AppendLine();

            for (int i = 0; i < rows.Count && shown < maxRows; i++)
            {
                var r = rows[i];
                sb.Append("  ");
                sb.Append(r.CaliberDisplay);
                sb.Append(" / ");
                sb.Append(r.ModLabel);
                sb.Append("  ×");
                sb.Append(r.Amount);
                if (r.IsMilitaryExclusive)
                    sb.Append("  [MIL EXCL]");
                sb.AppendLine();
                shown++;
            }
            if (rows.Count > maxRows)
                sb.Append("  … +").Append(rows.Count - maxRows).Append(" more loads");
            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// Hatch arms contribution preview vs light raiders and military armor.
        /// Uses live inventory + optional security for a defense-score sketch.
        /// </summary>
        public string FormatHatchPowerPreview(
            Inventory.Inventory inventory,
            float shelterSecurity,
            HatchDefenseSystemProxy hatch = null)
        {
            // Prefer HatchDefenseSystem via soft callback if provided.
            // Soft / unarmored contacts (raider flesh) vs plate (military).
            float armsSoft = 0f;
            float armsMil = 0f;
            if (hatch != null)
            {
                armsSoft = hatch.GetWeaponPower(ArmorUnarmored);
                armsMil = hatch.GetWeaponPower(ArmorMilitary);
            }
            else
            {
                var rows = BuildStockpileRows(inventory);
                for (int i = 0; i < rows.Count; i++)
                {
                    armsSoft += GetAmmoStockpileDefensePower(
                        rows[i].ItemId, rows[i].Amount, ArmorUnarmored);
                    armsMil += rows[i].DefensePowerVsMilitary;
                }
            }

            float defSoft = shelterSecurity + armsSoft;
            float defMil = shelterSecurity + armsMil;

            var sb = new StringBuilder(160);
            sb.Append("ARMS PREVIEW");
            sb.AppendLine();
            sb.Append("  vs raiders  arms ").Append(armsSoft.ToString("0.0"));
            sb.Append("  def ").Append(defSoft.ToString("0"));
            sb.AppendLine();
            sb.Append("  vs military arms ").Append(armsMil.ToString("0.0"));
            sb.Append("  def ").Append(defMil.ToString("0"));
            if (armsMil + 0.05f < armsSoft)
                sb.Append("\n  Soft loads lose bite on plate — stock AP/API for mil raids.");
            else if (armsMil > armsSoft + 0.5f)
                sb.Append("\n  Armor-piercing stockpile ready for plate.");
            return sb.ToString();
        }

        /// <summary>
        /// One-line combat feedback for the expedition encounter log.
        /// </summary>
        public static string FormatCombatEncounterLog(
            string survivorName,
            string encounterId,
            string ammoId,
            float finalDamage,
            float moraleDelta,
            float healthDelta,
            bool armorPenalty)
        {
            string who = string.IsNullOrEmpty(survivorName) ? "Scavenger" : survivorName;
            string target = string.IsNullOrEmpty(encounterId) ? "contact" : encounterId.Replace('_', ' ');
            string ammo = ammoId;
            if (TryGetLoad(ammoId, out var load))
                ammo = load.DisplayName;

            var sb = new StringBuilder(120);
            sb.Append(who);
            sb.Append(" · ");
            sb.Append(ammo);
            sb.Append(" → ");
            sb.Append(target);
            sb.Append(" (");
            sb.Append(finalDamage.ToString("0.0"));
            sb.Append(" dmg");
            if (armorPenalty) sb.Append(", soft on plate");
            sb.Append(')');

            if (Mathf.Abs(moraleDelta) > 0.01f)
            {
                sb.Append(moraleDelta > 0f ? "  morale +" : "  morale ");
                sb.Append(moraleDelta.ToString("0.#"));
            }
            if (Mathf.Abs(healthDelta) > 0.01f)
            {
                sb.Append(healthDelta > 0f ? "  health +" : "  health ");
                sb.Append(healthDelta.ToString("0.#"));
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// Soft hatch weapon-power probe so UI formatters need not reference Shelter
    /// types beyond a delegate-friendly wrapper constructed by the host.
    /// </summary>
    public sealed class HatchDefenseSystemProxy
    {
        private readonly Func<float, float> _getWeaponPower;

        public HatchDefenseSystemProxy(Func<float, float> getWeaponPower)
        {
            _getWeaponPower = getWeaponPower ?? (_ => 0f);
        }

        public float GetWeaponPower(float targetArmor) => _getWeaponPower(targetArmor);
    }
}
