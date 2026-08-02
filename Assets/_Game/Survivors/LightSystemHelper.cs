using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Stateless helper that owns the per-survivor light tick math.
    /// Lives in the Survivors assembly so NeedsSystem can call it without
    /// creating a circular dependency on the Environment assembly.
    ///
    /// PhotoperiodSystem (Environment) delegates to this via its static
    /// TickSurvivorLight method, passing through all required arguments.
    /// </summary>
    public static class LightSystemHelper
    {
        /// <summary>
        /// Advance a single survivor's <see cref="Survivor.LightExposure"/>,
        /// <see cref="Survivor.VitaminDProxy"/>, and hidden morale / health
        /// penalties for one tick.
        ///
        /// <paramref name="effectiveDaylightHours"/> is the product of the
        /// base daylight curve value and the current sky clarity (0..16).
        /// <paramref name="growLightActive"/> is true when the shelter's
        /// grow-light module is running, granting a partial substitute for
        /// natural daylight.
        /// </summary>
        public static void TickSurvivorLight(
            Survivor     sv,
            float        gameHours,
            float        effectiveDaylightHours,
            bool         growLightActive,
            LightProfile lightProfile)
        {
            if (sv == null || !sv.IsAlive || lightProfile == null || gameHours <= 0f)
                return;

            // 1. Effective daylight fraction this tick (0..1 of a full 16-h day)
            float naturalFraction = Mathf.Clamp01(effectiveDaylightHours / 16f);
            float growBonus       = growLightActive ? lightProfile.growLightEquivalentFraction : 0f;
            float lightFraction   = Mathf.Clamp01(naturalFraction + growBonus);

            // 2. LightExposure: accumulates in light, decays in dark
            float lightDelta;
            if (lightFraction > 0.05f)
            {
                lightDelta =  lightProfile.lightExposureGainPerHourDaylight * lightFraction * gameHours;
            }
            else
            {
                lightDelta = -lightProfile.lightExposureLossPerHourDark * gameHours;
            }
            sv.LightExposure = Mathf.Clamp(sv.LightExposure + lightDelta, 0f, 100f);

            // 3. Listless status: set each tick based on current LightExposure level.
            //    Cleared externally by ApplySunLampSession or a vitaminD-tagged meal.
            sv.IsListless = sv.LightExposure <= lightProfile.listlessThreshold;

            // 4. Listless morale drain
            if (sv.IsListless)
            {
                sv.Needs.Morale = Mathf.Clamp(
                    sv.Needs.Morale - lightProfile.listlessMoraleDrainPerHour * gameHours,
                    0f, 100f);
            }

            // 5. VitaminD proxy — slow accumulation/decay
            float vitDDelta;
            if (lightFraction > 0.1f)
            {
                vitDDelta =  lightProfile.vitaminDGainPerHourNormalLight * lightFraction * gameHours;
            }
            else
            {
                vitDDelta = -lightProfile.vitaminDDecayPerHour * gameHours;
            }
            sv.VitaminDProxy = Mathf.Clamp(sv.VitaminDProxy + vitDDelta, 0f, 100f);

            // 6. Hidden vitaminD penalty: silently drains health + morale when low
            if (sv.VitaminDProxy <= lightProfile.vitaminDLowThreshold)
            {
                float depletionRatio = lightProfile.vitaminDLowThreshold > 0f
                    ? (lightProfile.vitaminDLowThreshold - sv.VitaminDProxy) / lightProfile.vitaminDLowThreshold
                    : 1f;
                sv.Needs.Health = Mathf.Clamp(
                    sv.Needs.Health - lightProfile.vitaminDHealthPenaltyPerHour * depletionRatio * gameHours,
                    0f, 100f);
                sv.Needs.Morale = Mathf.Clamp(
                    sv.Needs.Morale - lightProfile.vitaminDMoralePenaltyPerHour * depletionRatio * gameHours,
                    0f, 100f);
            }
        }

        /// <summary>
        /// Apply a one-shot light boost to a survivor's LightExposure (e.g. from a
        /// sun-lamp session or consuming a vitaminD-tagged meal). Clears Listless if
        /// LightExposure rises above <see cref="LightProfile.listlessThreshold"/>.
        /// </summary>
        public static void ApplySunLampSession(
            Survivor     sv,
            float        boostAmount,
            LightProfile lightProfile)
        {
            if (sv == null || lightProfile == null) return;
            sv.LightExposure = Mathf.Clamp(sv.LightExposure + boostAmount, 0f, 100f);
            if (sv.LightExposure > lightProfile.listlessThreshold)
            {
                sv.IsListless = false;
            }
        }

        /// <summary>
        /// Restore VitaminD by consuming a vitaminD-tagged food item.
        /// Does not affect LightExposure or Listless directly.
        /// </summary>
        public static void ApplyVitaminDFood(Survivor sv, LightProfile lightProfile)
        {
            if (sv == null || lightProfile == null) return;
            sv.VitaminDProxy = Mathf.Clamp(
                sv.VitaminDProxy + lightProfile.vitaminDFoodRestoreAmount,
                0f, 100f);
        }
    }
}
