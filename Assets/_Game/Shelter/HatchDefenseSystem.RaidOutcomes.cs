using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Shelter
{
    public partial class HatchDefenseSystem
    {
        private void TryRecordSoloGuardRepel(
            RaidResolution result,
            RaidEvent raid,
            int day,
            IReadOnlyList<Survivor> survivors)
        {
            // Prompt #222 — Bouncer Holdout quest: sole guard survives a repel.
            if (!result.Repelled || _personalQuests == null || survivors == null
                || _activeGuards.Count != 1)
                return;

            string soleGuardId = GetSoleActiveGuardId();
            Survivor soleGuard = FindLivingSurvivor(survivors, soleGuardId);
            if (soleGuard == null) return;

            int questDay = raid.Day > 0 ? raid.Day : day;
            _personalQuests.RecordSoloHatchDefense(
                soleGuard,
                activeGuardCount: 1,
                survived: true,
                currentDay: questDay);
        }

        private string GetSoleActiveGuardId()
        {
            foreach (var guardEntry in _activeGuards)
                return guardEntry.Key;
            return null;
        }

        private static Survivor FindLivingSurvivor(
            IReadOnlyList<Survivor> survivors,
            string survivorId)
        {
            for (int i = 0; i < survivors.Count; i++)
            {
                var survivor = survivors[i];
                if (survivor != null && survivor.Id == survivorId && survivor.IsAlive)
                    return survivor;
            }

            return null;
        }

        private void ApplyRaidOutcome(RaidResolution result, RaidEvent raid, int day)
        {
            if (result.Repelled)
                ApplyRepelledRaidOutcome(result, raid, day);
            else
                ApplyBreachedRaidOutcome(result, raid, day);
        }

        private void ApplyRepelledRaidOutcome(RaidResolution result, RaidEvent raid, int day)
        {
            ApplyRepelCosts(result);
            result.HatchDamage = 3f
                + (result.RaidStrength / Mathf.Max(1f, result.DefenseScore)) * 8f;
            result.MoraleDelta = RepelMoraleBoost;
            result.Message = string.IsNullOrEmpty(raid.Message)
                ? "Hatch held. Brass on the floor, smoke in the stairwell — but they left."
                : raid.Message + " Held.";
            ApplyMoraleToSurvivors(result.MoraleDelta);
            ApplyHatchWear(result.HatchDamage);

            // #259 Holding the Line: defended hatch without Coward flee.
            RecordDeserterRaidHold(result, day);
        }

        private void ApplyBreachedRaidOutcome(RaidResolution result, RaidEvent raid, int day)
        {
            result.HatchDamage = 15f
                + (result.RaidStrength - result.DefenseScore) * 0.6f;
            result.MoraleDelta = BreachMoralePenalty;
            result.Message = string.IsNullOrEmpty(raid.Message)
                ? "Hatch breached. Hands in the stores. Someone is screaming."
                : raid.Message + " Breached.";
            StealLoot(result);
            RollTrauma(result);
            ApplyMoraleToSurvivors(result.MoraleDelta);
            ApplyHatchWear(result.HatchDamage);

            // #272 Prepper: hatch destroyed (severe breach wear) + survived.
            RecordPrepperHatchDestroyedIfAny(result, day);
        }

        private void CommitRaidResolution(RaidResolution result)
        {
            LastResolution = result;
            TotalRaidsResolved++;
            if (result.Breached) TotalBreaches++;
            _hoursSinceLastRaid = 0f;
            LastRaidSummary = BuildSummary(result);

            // Prompt #202 — bandaging window while hatch-breach raid is "active".
            _raidWindowHoursRemaining = Mathf.Max(_raidWindowHoursRemaining, RaidWindowHours);

            OnRaidResolved?.Invoke(result);
            OnSecurityChanged?.Invoke();
        }

        private static string BuildSummary(RaidResolution result)
        {
            if (result == null || !result.Launched) return "Hatch quiet.";
            if (result.Repelled)
            {
                return $"Repelled (D {result.DefenseScore:0} > R {result.RaidStrength:0})"
                    + (result.AmmoConsumed > 0 ? $", −{result.AmmoConsumed} ammo" : "");
            }

            int stolen = result.StolenItems != null ? result.StolenItems.Count : 0;
            return $"BREACHED (D {result.DefenseScore:0} < R {result.RaidStrength:0}), stole {stolen} stacks";
        }
    }
}
