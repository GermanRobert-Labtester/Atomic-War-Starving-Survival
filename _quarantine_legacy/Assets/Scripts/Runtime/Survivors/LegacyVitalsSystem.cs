using AtomicWar.Core.Events;
using AtomicWar.Runtime.Time;
using UnityEngine;

namespace AtomicWar.Runtime.Survivors
{
    // Domain Events triggered when thresholds are crossed
    public struct SurvivorStarvingEvent
    {
        public SurvivorModel Survivor;
    }

    public struct SurvivorExhaustedEvent
    {
        public SurvivorModel Survivor;
    }

    public struct SurvivorSickEvent
    {
        public SurvivorModel Survivor;
        public float SicknessLevel;
    }

    public struct SurvivorMentallyBrokenEvent
    {
        public SurvivorModel Survivor;
    }

    public struct NeedStatusAlertEvent
    {
        public SurvivorModel Survivor;
        public string AlertMessage;
    }

    /// <summary>
    /// Pure C# system processing survivor physiological needs (Hunger, Fatigue, Health, Sickness, Morale).
    /// All values range strictly from 0 to 100.
    /// Legacy prototype vitals tracker (5 stats) that predates the full 7-need AtomicWar.Runtime.Needs.NeedsSystem
    /// (hunger/thirst/fatigue/warmth/morale/radiation/health). Kept in place so the existing shelter prototype
    /// (GameController, Utility AI actions, ShelterGameUI) keeps working unchanged; not yet migrated to the new model.
    /// </summary>
    public class LegacyVitalsSystem
    {
        // Threshold constants
        public const float StarvationThreshold = 100f;   // 100 = Maximum hunger (Starving)
        public const float ExhaustionThreshold = 100f;   // 100 = Maximum fatigue (Exhausted)
        public const float HighSicknessThreshold = 50f;  // Sickness above this causes health damage
        public const float MentalBreakThreshold = 0f;    // 0 = Morale depleted (Mentally broken)

        private readonly SurvivorSystem _survivorSystem;

        public LegacyVitalsSystem(SurvivorSystem survivorSystem)
        {
            _survivorSystem = survivorSystem;
            EventBus.Subscribe<HourTickEvent>(OnHourTick);
        }

        public void Unsubscribe()
        {
            EventBus.Unsubscribe<HourTickEvent>(OnHourTick);
        }

        /// <summary>
        /// Ticks physiological decay every game hour.
        /// </summary>
        public void OnHourTick(HourTickEvent e)
        {
            foreach (var survivor in _survivorSystem.GetLivingSurvivors())
            {
                UpdateSurvivorNeeds(survivor, 1.0f);
            }
        }

        /// <summary>
        /// Updates a single survivor's needs over elapsed hours.
        /// </summary>
        public void UpdateSurvivorNeeds(SurvivorModel survivor, float hoursPassed)
        {
            if (survivor == null || !survivor.IsAlive) return;

            // 1. Hunger increases over time (0 = full, 100 = starving)
            float hungerRate = survivor.Data != null ? survivor.Data.BaseHungerDecayRate : 2.0f;
            if (survivor.CurrentState == SurvivorState.Crafting || survivor.CurrentState == SurvivorState.Scavenging)
            {
                hungerRate *= 1.5f; // Extra physical exertion
            }
            survivor.Hunger = Mathf.Clamp(survivor.Hunger + (hungerRate * hoursPassed), 0f, 100f);

            // 2. Fatigue increases while awake, decreases while sleeping (0 = rested, 100 = exhausted)
            if (survivor.CurrentState == SurvivorState.Sleeping)
            {
                survivor.Fatigue = Mathf.Clamp(survivor.Fatigue - (12f * hoursPassed), 0f, 100f);
            }
            else
            {
                float fatigueRate = survivor.Data != null ? survivor.Data.BaseFatigueDecayRate : 1.5f;
                survivor.Fatigue = Mathf.Clamp(survivor.Fatigue + (fatigueRate * hoursPassed), 0f, 100f);
            }

            // 3. Morale decreases if hunger is high (> 75) or fatigue is extreme
            if (survivor.Hunger >= 75f)
            {
                float moralLoss = 1.0f * (survivor.Data != null ? survivor.Data.MoralSensitivity : 1.0f);
                survivor.Morale = Mathf.Clamp(survivor.Morale - moralLoss, 0f, 100f);
            }

            // 4. Health decreases if Hunger is 100 (starving) or Sickness is high (> 50)
            float healthDamage = 0f;

            if (survivor.Hunger >= StarvationThreshold)
            {
                healthDamage += 4.0f * hoursPassed; // Starvation health penalty
            }

            if (survivor.Sickness >= HighSicknessThreshold)
            {
                // Scaled sickness health penalty
                healthDamage += (survivor.Sickness / 100f) * 5.0f * hoursPassed;
            }

            if (healthDamage > 0f)
            {
                survivor.Health = Mathf.Clamp(survivor.Health - healthDamage, 0f, 100f);
            }

            // 5. Evaluate state thresholds & raise edge-triggered events
            CheckStateThresholds(survivor);

            // 6. Check death condition
            if (survivor.Health <= 0f)
            {
                string cause = survivor.Hunger >= StarvationThreshold ? "Starvation" : "Severe Illness";
                _survivorSystem.RemoveSurvivor(survivor, cause);
            }
        }

        /// <summary>
        /// Explicitly apply a bad event outcome affecting morale and health.
        /// </summary>
        public void ApplyBadEvent(SurvivorModel survivor, float moralePenalty, float sicknessIncrease = 0f)
        {
            if (survivor == null || !survivor.IsAlive) return;

            if (moralePenalty > 0f)
            {
                float netMoraleLoss = moralePenalty * (survivor.Data != null ? survivor.Data.MoralSensitivity : 1.0f);
                survivor.Morale = Mathf.Clamp(survivor.Morale - netMoraleLoss, 0f, 100f);
            }

            if (sicknessIncrease > 0f)
            {
                survivor.Sickness = Mathf.Clamp(survivor.Sickness + sicknessIncrease, 0f, 100f);
            }

            CheckStateThresholds(survivor);
        }

        /// <summary>
        /// Direct stat modifiers clamped 0-100.
        /// </summary>
        public void ModifyHunger(SurvivorModel survivor, float delta)
        {
            survivor.Hunger = Mathf.Clamp(survivor.Hunger + delta, 0f, 100f);
            CheckStateThresholds(survivor);
        }

        public void ModifyFatigue(SurvivorModel survivor, float delta)
        {
            survivor.Fatigue = Mathf.Clamp(survivor.Fatigue + delta, 0f, 100f);
            CheckStateThresholds(survivor);
        }

        public void ModifySickness(SurvivorModel survivor, float delta)
        {
            survivor.Sickness = Mathf.Clamp(survivor.Sickness + delta, 0f, 100f);
            CheckStateThresholds(survivor);
        }

        public void ModifyMorale(SurvivorModel survivor, float delta)
        {
            survivor.Morale = Mathf.Clamp(survivor.Morale + delta, 0f, 100f);
            CheckStateThresholds(survivor);
        }

        public void ModifyHealth(SurvivorModel survivor, float delta)
        {
            survivor.Health = Mathf.Clamp(survivor.Health + delta, 0f, 100f);
            if (survivor.Health <= 0f)
            {
                _survivorSystem.RemoveSurvivor(survivor, "Fatal Injuries");
            }
        }

        /// <summary>
        /// Checks critical thresholds and triggers edge-detected EventBus notifications.
        /// </summary>
        private void CheckStateThresholds(SurvivorModel survivor)
        {
            // Starvation event
            bool isStarving = survivor.Hunger >= StarvationThreshold;
            if (isStarving && !survivor.WasStarving)
            {
                survivor.WasStarving = true;
                EventBus.Raise(new SurvivorStarvingEvent { Survivor = survivor });
                EventBus.Raise(new NeedStatusAlertEvent
                {
                    Survivor = survivor,
                    AlertMessage = $"{survivor.Data?.CharacterName ?? "Survivor"} is starving!"
                });
            }
            else if (!isStarving)
            {
                survivor.WasStarving = false;
            }

            // Exhaustion event
            bool isExhausted = survivor.Fatigue >= ExhaustionThreshold;
            if (isExhausted && !survivor.WasExhausted)
            {
                survivor.WasExhausted = true;
                EventBus.Raise(new SurvivorExhaustedEvent { Survivor = survivor });
                EventBus.Raise(new NeedStatusAlertEvent
                {
                    Survivor = survivor,
                    AlertMessage = $"{survivor.Data?.CharacterName ?? "Survivor"} collapses from exhaustion!"
                });
            }
            else if (!isExhausted)
            {
                survivor.WasExhausted = false;
            }

            // Sickness event
            bool isSick = survivor.Sickness >= HighSicknessThreshold;
            if (isSick && !survivor.WasSick)
            {
                survivor.WasSick = true;
                survivor.CurrentState = SurvivorState.Sick;
                EventBus.Raise(new SurvivorSickEvent { Survivor = survivor, SicknessLevel = survivor.Sickness });
                EventBus.Raise(new NeedStatusAlertEvent
                {
                    Survivor = survivor,
                    AlertMessage = $"{survivor.Data?.CharacterName ?? "Survivor"} has fallen dangerously sick!"
                });
            }
            else if (!isSick && survivor.WasSick)
            {
                survivor.WasSick = false;
                if (survivor.CurrentState == SurvivorState.Sick)
                {
                    survivor.CurrentState = SurvivorState.Idle;
                }
            }

            // Mentally broken event
            bool isMentallyBroken = survivor.Morale <= MentalBreakThreshold;
            if (isMentallyBroken && !survivor.WasMentallyBroken)
            {
                survivor.WasMentallyBroken = true;
                EventBus.Raise(new SurvivorMentallyBrokenEvent { Survivor = survivor });
                EventBus.Raise(new NeedStatusAlertEvent
                {
                    Survivor = survivor,
                    AlertMessage = $"{survivor.Data?.CharacterName ?? "Survivor"} suffers a mental breakdown!"
                });
            }
            else if (!isMentallyBroken)
            {
                survivor.WasMentallyBroken = false;
            }
        }
    }
}
