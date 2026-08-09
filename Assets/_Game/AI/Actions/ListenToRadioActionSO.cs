using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Data;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// AI action: Listen to the radio to extract intel. Score spikes when map uncertainty
    /// is high (player needs intel). Requires operational radio module. Consumes time and
    /// fuel. Success yields IntelNode (PlumeReport, WeatherForecast, etc).
    /// </summary>
    [CreateAssetMenu(fileName = "Action_ListenToRadio", menuName = "ASHFALL/AI/Listen To Radio Action")]
    public class ListenToRadioActionSO : SurvivorAction
    {
        [Header("Listen Parameters")]
        [Tooltip("Hours to spend listening (tuning time)")]
        public float listenHours = 1f;

        [Tooltip("Fuel consumed per hour of listening")]
        public float fuelConsumptionPerHour = 0.5f;

        [Tooltip("Minimum map uncertainty to consider listening (0..1)")]
        public float minMapUncertaintyThreshold = 0.3f;

        [Tooltip("Target frequency ID to tune to (empty = current)")]
        public string targetFrequencyId = "";

        public ListenToRadioActionSO()
        {
            id = "action_listen_to_radio";
            displayName = "Listen to Radio";
            description = "Tune the radio to extract intel from outside broadcasts.";
            basePriority = 0.15f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context == null || context.Survivor == null) return 0f;
            if (context.Shelter == null) return 0f;

            // Check if radio module is installed and operational
            var radioModule = context.Shelter.GetModule("radio");
            if (radioModule == null || !radioModule.IsOperational) return 0f;

            // Check if we have a RadioTunerSystem hook
            // Note: In production, RadioTunerSystem would be injected into AIContext
            // For now, we assume it exists if the radio module is present

            // Score based on map uncertainty: high uncertainty = high priority
            // The more uncertain the map, the more we need intel
            float uncertaintyScore = 0f;
            if (context.MapUncertainty > minMapUncertaintyThreshold)
            {
                // Normalize: threshold = 0, max (1.0) = 1.0
                uncertaintyScore = (context.MapUncertainty - minMapUncertaintyThreshold) /
                                   (1f - minMapUncertaintyThreshold);
            }

            // Check if survivor has enough fatigue to listen (listening is mentally draining)
            float fatiguePenalty = 0f;
            if (context.Survivor.Needs.Fatigue > 70f)
            {
                fatiguePenalty = (context.Survivor.Needs.Fatigue - 70f) / 30f;
            }

            // Final score: uncertainty bonus minus fatigue penalty
            float score = uncertaintyScore - fatiguePenalty * 0.5f;
            return Mathf.Clamp01(score);
        }

        public override void Execute(AIContext context)
        {
            if (context == null || context.Survivor == null) return;
            if (context.Shelter == null) return;

            // In production, this would:
            // 1. Get RadioTunerSystem from context or game state
            // 2. Tune to target frequency (if specified)
            // 3. Spend time listening (consume fuel, advance tuning)
            // 4. If tuning completes, extract intel

            // For now, simulate the action
            var radioModule = context.Shelter.GetModule("radio");
            if (radioModule == null || !radioModule.IsOperational) return;

            // Consume fuel (simplified - in production, this would go through RadioTunerSystem)
            float fuelNeeded = fuelConsumptionPerHour * listenHours;
            if (radioModule.Fuel >= fuelNeeded)
            {
                radioModule.Fuel -= fuelNeeded;

                // Consume survivor fatigue (listening is mentally draining)
                if (context.NeedsSystem != null)
                    context.NeedsSystem.Modify(context.Survivor, NeedKind.Fatigue, 10f * listenHours);
                else
                    context.Survivor.Needs.Fatigue = Mathf.Clamp(context.Survivor.Needs.Fatigue + 10f * listenHours, 0f, 100f);

                Debug.Log($"[AI] {context.Survivor.DisplayName} listened to radio for {listenHours}h");

                // Note: In production, this would trigger RadioTunerSystem.Tick() and
                // potentially extract intel. The actual intel extraction would be handled
                // by the RadioTunerSystem, which would raise OnIntelExtracted event.
            }
        }
    }
}
