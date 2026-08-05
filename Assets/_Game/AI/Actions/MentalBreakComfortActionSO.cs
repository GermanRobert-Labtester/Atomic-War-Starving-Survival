using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// Utility AI: give a broken survivor a high-value comfort item to
    /// accelerate their break's cure progress. The actual item lookup
    /// and consumption happens in the host-injected
    /// <see cref="MentalBreakSystem.ComfortCureHandler"/>; the AI just
    /// scores the opportunity and invokes the system.
    ///
    /// Score: high if any living survivor in the shelter is broken AND
    /// a comfort item is plausibly available (handler is wired). Zero
    /// otherwise. An enabled comfort station multiplies the score.
    /// </summary>
    [CreateAssetMenu(fileName = "Action_MentalBreakComfort", menuName = "ASHFALL/AI/Mental Break Comfort Action")]
    public class MentalBreakComfortActionSO : SurvivorAction
    {
        public MentalBreakComfortActionSO()
        {
            id = "action_mental_break_comfort";
            displayName = "Comfort Broken Survivor";
            description = "Give a broken survivor a comfort item to accelerate the cure of their mental break.";
            basePriority = 0.4f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null) return 0f;
            if (context.MentalBreak == null) return 0f;
            if (context.MentalBreak.ComfortCureHandler == null) return 0f;
            if (context.GetSurvivors == null) return 0f;

            var all = context.GetSurvivors();
            if (all == null || all.Count == 0) return 0f;

            float best = 0f;
            for (int i = 0; i < all.Count; i++)
            {
                var s = all[i];
                if (s == null || !s.IsAlive || !s.HasMentalBreak) continue;
                var br = context.MentalBreak.GetBreak(s.currentMentalBreakId);
                if (br == null || br.comfortItemCureAmount <= 0f) continue;
                if (s.mentalBreakCureProgress >= br.cureHours) continue; // already done

                // Score rises as cure progress approaches the threshold;
                // a near-cured break is the highest priority.
                float remaining = Mathf.Max(0f, br.cureHours - s.mentalBreakCureProgress);
                float score = 1f - Mathf.Clamp01(remaining / Mathf.Max(1f, br.cureHours));
                if (score > best) best = score;
            }

            if (best <= 0f) return 0f;
            return best * GetComfortStationMultiplier(context.Shelter);
        }

        /// <summary>
        /// Enabled comfort station multiplies comfort-care priority.
        /// Disabled or missing station → 1.0 (no bonus).
        /// </summary>
        public static float GetComfortStationMultiplier(Shelter.Shelter shelter)
        {
            if (shelter == null) return 1f;
            var mod = shelter.GetModule(MedicalSystem.ComfortStationModuleId);
            if (mod == null || !mod.IsEnabled) return 1f;

            if (mod.Definition is ComfortStationModuleSO so)
                return Mathf.Max(1f, so.comfortCureScoreMultiplier);

            // Generic comfort_station without SO: modest default boost
            return 1.5f;
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || context.MentalBreak == null) return;
            if (context.GetSurvivors == null) return;

            var all = context.GetSurvivors();
            Survivor target = null;
            float bestScore = -1f;
            for (int i = 0; i < all.Count; i++)
            {
                var s = all[i];
                if (s == null || !s.IsAlive || !s.HasMentalBreak) continue;
                var br = context.MentalBreak.GetBreak(s.currentMentalBreakId);
                if (br == null || br.comfortItemCureAmount <= 0f) continue;
                if (s.mentalBreakCureProgress >= br.cureHours) continue;

                float remaining = Mathf.Max(0f, br.cureHours - s.mentalBreakCureProgress);
                float score = 1f - Mathf.Clamp01(remaining / Mathf.Max(1f, br.cureHours));
                if (score > bestScore)
                {
                    bestScore = score;
                    target = s;
                }
            }
            if (target == null) return;

            // The system handles item lookup + consumption via the
            // host-injected ComfortCureHandler. Try multiple times in case
            // the handler can supply more than one item per call.
            string breakIdBefore = target.currentMentalBreakId;
            bool wasViolent = SocialPerkSystem.IsViolentParanoia(breakIdBefore);
            int attempts = 0;
            while (attempts < 4 && target.HasMentalBreak
                   && target.mentalBreakCureProgress < (context.MentalBreak.GetBreak(target.currentMentalBreakId)?.cureHours ?? float.PositiveInfinity))
            {
                if (!context.MentalBreak.TryCureWithComfortItem(target)) break;
                attempts++;
            }

            // Prompt #211 — comfort cure of ViolentParanoia (no force/med bed)
            // earns De-Escalator for the comforter.
            if (wasViolent && !target.HasMentalBreak && context.SocialPerks != null)
            {
                context.SocialPerks.RecordPeacefulDeEscalation(
                    context.Survivor, context.CurrentDay);
            }
        }
    }
}
