using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Sleepwalking System (#43) — highly fatigued and stressed survivors
    /// occasionally sleepwalk at night, moving food items, unlocking hatch
    /// doors, or wandering into hazardous shelter wings.
    ///
    /// Extends SleepDeprivationSystem. Owns: Survivor.SleepwalkingRisk,
    /// Survivor.LastSleepwalkRoomId, Survivor.DidSleepwalkTonight.
    /// </summary>
    public class SleepwalkingSystem
    {
        public const float BaseSleepwalkChance = 0.08f;
        public const float FatigueMultiplierPerMissedNight = 0.15f;
        public const float StressMultiplier = 0.10f;
        public const float MaxSleepwalkChance = 0.50f;

        public event Action<Survivor, string, string> OnSleepwalkIncident;
        // sv, action, destinationRoom
        public event Action<Survivor, string> OnFoodMoved;
        public event Action<Survivor> OnHatchUnlocked;
        public event Action<Survivor, string> OnWanderedToHazard;

        public Func<Survivor, int> GetMissedNights;
        public Func<Survivor, float> GetStressLevel;
        public Func<List<string>> GetRoomIds;
        public Func<string, bool> IsRoomHazardous;
        public Func<bool> IsHatchLocked;
        public Action UnlockHatch;
        public Action<string, int> MoveFoodItems;
        // itemId, count
        public System.Random Rng;

        public void TickNightCheck(IReadOnlyList<Survivor> survivors, int currentDay)
        {
            if (survivors == null) return;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                if (sv.DidSleepwalkTonight) continue;
                if (sv.State == SurvivorState.Dead || sv.State == SurvivorState.Incapacitated)
                    continue;

                int missedNights = GetMissedNights?.Invoke(sv) ?? 0;
                float stress = GetStressLevel?.Invoke(sv) ?? 0f;

                float chance = BaseSleepwalkChance +
                    missedNights * FatigueMultiplierPerMissedNight +
                    stress * StressMultiplier;
                chance = Math.Min(chance, MaxSleepwalkChance);

                sv.SleepwalkingRisk = chance;

                if ((Rng?.NextDouble() ?? 0.5) < chance)
                {
                    sv.DidSleepwalkTonight = true;
                    TriggerSleepwalk(sv, currentDay);
                }
            }
        }

        private void TriggerSleepwalk(Survivor sv, int day)
        {
            var rooms = GetRoomIds?.Invoke();
            if (rooms == null || rooms.Count == 0) return;

            string destRoom = rooms[Rng?.Next(rooms.Count) ?? 0];
            // Don't sleepwalk to own room
            if (destRoom == sv.CurrentRoomId && rooms.Count > 1)
                destRoom = rooms[(Rng?.Next(rooms.Count - 1) ?? 0)];

            // Determine action: 40% move food, 30% unlock hatch, 30% wander hazard
            float roll = (float)(Rng?.NextDouble() ?? 0.5);
            string action;

            if (roll < 0.4f)
            {
                action = "moved_food";
                MoveFoodItems?.Invoke("food_ration", 1);
                OnFoodMoved?.Invoke(sv, "food_ration");
            }
            else if (roll < 0.7f && (IsHatchLocked?.Invoke() ?? false))
            {
                action = "unlocked_hatch";
                UnlockHatch?.Invoke();
                OnHatchUnlocked?.Invoke(sv);
            }
            else
            {
                action = "wandered_hazard";
                if (IsRoomHazardous?.Invoke(destRoom) ?? false)
                    OnWanderedToHazard?.Invoke(sv, destRoom);
            }

            sv.LastSleepwalkRoomId = destRoom;
            OnSleepwalkIncident?.Invoke(sv, action, destRoom);
        }

        public void ResetNightFlags(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return;
            for (int i = 0; i < survivors.Count; i++)
            {
                if (survivors[i] != null)
                    survivors[i].DidSleepwalkTonight = false;
            }
        }
    }
}
