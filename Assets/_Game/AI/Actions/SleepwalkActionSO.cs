using System;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Radiation;

namespace AtomicWar._Game.AI.Actions
{
    [CreateAssetMenu(fileName = "NewSleepwalkAction", menuName = "ASHFALL/AI Actions/Sleepwalk")]
    public class SleepwalkActionSO : SurvivorAction
    {
        public const string SleepwalkBreakId = "mental_break_sleepwalking";

        public enum SleepwalkHazard
        {
            WanderOutsideWithoutSuit,
            OpenHatch,
            EatRawRations
        }

        public Action<Survivor, SleepwalkHazard> OnSleepwalkHazardExecuted;
        public Action<Survivor, string> OnSleepwalkBlocked;

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;
            return ScoreAction(context.Survivor, context.InternalLockSystem);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null) return;
            var rng = context.Random ?? AtomicWar._Game.Utilities.SeededRandom.CreateFixed("sleepwalkactionso");
            ExecuteSleepwalk(
                context.Survivor,
                context.InternalLockSystem,
                rng,
                out _,
                context.RadiationSystem);
        }

        public float ScoreAction(Survivor sv, InternalLockSystem lockSystem)
        {
            if (sv == null || !sv.IsAlive) return 0f;
            if (sv.currentMentalBreakId == SleepwalkBreakId) return 0.95f;
            if (sv.RadiationAnxiety > 0.7f && sv.State == SurvivorState.Resting) return 0.85f;
            return 0f;
        }

        public bool ExecuteSleepwalk(
            Survivor sv,
            InternalLockSystem lockSystem,
            System.Random rng,
            out SleepwalkHazard hazard,
            RadiationSystem radiation = null)
        {
            hazard = SleepwalkHazard.WanderOutsideWithoutSuit;
            if (sv == null || !sv.IsAlive) return false;
            if (rng == null) rng = AtomicWar._Game.Utilities.SeededRandom.CreateFixed("sleepwalkactionso");

            string currentRoom = sv.CurrentRoomId ?? "quarters";
            if (lockSystem != null && !lockSystem.CanSleepwalkerEscape(currentRoom))
            {
                OnSleepwalkBlocked?.Invoke(sv, currentRoom);
                return false;
            }

            int roll = rng.Next(3);
            hazard = (SleepwalkHazard)roll;

            switch (hazard)
            {
                case SleepwalkHazard.WanderOutsideWithoutSuit:
                    if (radiation != null)
                        radiation.Expose(sv, 20f, 1f);
                    else
                        sv.RadiationDose = Mathf.Clamp(sv.RadiationDose + 20f, 0f, 100f);
                    break;
                case SleepwalkHazard.OpenHatch:
                    sv.Needs.Warmth = Mathf.Clamp(sv.Needs.Warmth - 30f, 0f, 100f);
                    break;
                case SleepwalkHazard.EatRawRations:
                    // Hunger: higher = worse; eating reduces hunger.
                    sv.Needs.Hunger = Mathf.Clamp(sv.Needs.Hunger - 15f, 0f, 100f);
                    if (radiation != null)
                        radiation.Expose(sv, 10f, 1f);
                    else
                        sv.RadiationDose = Mathf.Clamp(sv.RadiationDose + 10f, 0f, 100f);
                    break;
            }

            OnSleepwalkHazardExecuted?.Invoke(sv, hazard);
            return true;
        }
    }
}
