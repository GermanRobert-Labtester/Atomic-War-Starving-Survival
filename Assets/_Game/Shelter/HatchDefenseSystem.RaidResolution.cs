using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Shelter
{
    public partial class HatchDefenseSystem
    {
        private static RaidResolution CreateRaidResolution(RaidEvent raid)
        {
            return new RaidResolution
            {
                Event = raid,
                Launched = false,
                StolenItems = new List<StolenLootLine>(),
                TraumatizedSurvivorIds = new List<string>()
            };
        }

        private bool TryLaunchRaid(
            RaidEvent raid,
            bool ignoreDayGate,
            RaidResolution result,
            out int day)
        {
            day = 0;
            if (raid == null)
            {
                result.Message = "No raid event.";
                return false;
            }

            day = raid.Day > 0 ? raid.Day : (_getDay != null ? _getDay() : 0);
            if (!ignoreDayGate && raid.Trigger != RaidTrigger.Forced && !IsRaidUnlocked(day))
            {
                result.Message = "Pre-Day 30: hatch raids not yet active.";
                return false;
            }

            // Prompt #184 — suppressing fire pins raiders; no launch while halted.
            if (IsRaidHalted && raid.Trigger != RaidTrigger.Forced)
            {
                result.Message = "Suppressing fire still pins them at the stairwell.";
                return false;
            }

            result.Launched = true;
            return true;
        }

        private void InitializeRaidScores(RaidResolution result, RaidEvent raid)
        {
            result.RaidStrength = Mathf.Max(0f, raid.Strength);
            result.ShelterSecurity = GetShelterSecurity();
            result.GuardBonusApplied = GetGuardBonus();

            // Armor from the raiding faction shapes AP/JHP ammo contribution.
            float raidArmor = FactionArmorResolver != null
                ? FactionArmorResolver(raid.FactionId)
                : 0f;
            result.WeaponPower = GetWeaponPower(null, raidArmor);
        }

        private IReadOnlyList<Survivor> CompleteRaidScoring(RaidResolution result)
        {
            ApplyPerimeterTrapDamage(result);

            IReadOnlyList<Survivor> survivors = _getSurvivors?.Invoke();
            result.WeaponPower *= GetCloseQuartersMultiplier(survivors);
            ApplyWarlordUnarmedDefense(result, survivors);

            result.DefenseScore = result.ShelterSecurity + result.WeaponPower;
            // Strict: equal scores breach; defense must exceed raid strength to repel.
            result.Repelled = result.DefenseScore > result.RaidStrength;
            return survivors;
        }

        private void ApplyPerimeterTrapDamage(RaidResolution result)
        {
            if (_perimeterTraps == null) return;

            float trapDamage = _perimeterTraps.GetTrapDamageAgainstRaiders();
            if (trapDamage > 0f)
                result.RaidStrength = Mathf.Max(0f, result.RaidStrength - trapDamage);
        }

        private float GetCloseQuartersMultiplier(IReadOnlyList<Survivor> survivors)
        {
            float multiplier = 1f;
            if (survivors == null || _combatPerks == null) return multiplier;

            for (int i = 0; i < survivors.Count; i++)
            {
                var guard = survivors[i];
                if (guard == null || !_activeGuards.ContainsKey(guard.Id)) continue;

                float guardMultiplier = _combatPerks.GetCloseQuartersDamageMultiplier(
                    guard,
                    confinedOrBreach: true);
                if (guardMultiplier > multiplier)
                    multiplier = guardMultiplier;
            }

            return multiplier;
        }

        private void ApplyWarlordUnarmedDefense(
            RaidResolution result,
            IReadOnlyList<Survivor> survivors)
        {
            if (_personalQuests == null || result.WeaponPower > 0.01f) return;

            var activeGuards = CollectActiveGuardSurvivors(survivors);
            float warlordBonus = _personalQuests.GetWarlordUnarmedDefenseBonus(
                activeGuards,
                weaponsPresent: false);
            if (warlordBonus > 0f)
                result.WeaponPower = Mathf.Max(result.WeaponPower, warlordBonus);
        }

        private List<Survivor> CollectActiveGuardSurvivors(
            IReadOnlyList<Survivor> survivors)
        {
            var activeGuards = new List<Survivor>();
            foreach (var guardEntry in _activeGuards)
            {
                if (survivors == null) break;
                for (int i = 0; i < survivors.Count; i++)
                {
                    var survivor = survivors[i];
                    if (survivor != null && survivor.Id == guardEntry.Key)
                        activeGuards.Add(survivor);
                }
            }

            return activeGuards;
        }
    }
}
