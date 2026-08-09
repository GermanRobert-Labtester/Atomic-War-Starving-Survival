using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// Mercy Kill — Euthanasia (Prompt #58). If a survivor is in a Coma or has
    /// unhealable Acute Radiation Syndrome, the player can authorize a mercy kill.
    /// Trait variance: Sociopaths feel nothing, Paranoid survivors lose trust,
    /// Empaths suffer a 10-day mourning debuff.
    /// </summary>
    [CreateAssetMenu(fileName = "NewMercyKillAction", menuName = "ASHFALL/AI Actions/Mercy Kill")]
    public class MercyKillActionSO : SurvivorAction
    {
        [Header("Mercy Kill")]
        [Tooltip("Base utility score when a mercy-kill candidate exists.")]
        [Range(0f, 1f)]
        public float baseScore = 0.7f;

        public const float MourningDurationDays = 10f;
        public const float MourningMoraleDrainPerDay = 2f;
        public const float ParanoidTrustLoss = 20f;
        public const float SociopathMoraleImmunity = 0f;

        /// <summary>Delegate: perform the mercy kill on a target survivor. Returns true if successful.</summary>
        public System.Func<Survivors.Survivor, bool> PerformMercyKill;

        /// <summary>Delegate: apply affinity hit between killer and witnesses.</summary>
        public System.Action<Survivors.Survivor, Survivors.Survivor, float> ApplyAffinityHit;

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null || !context.Survivor.IsAlive) return 0f;
            if (PerformMercyKill == null) return 0f;

            // Find a mercy-kill candidate: Coma or unhealable ARS.
            var survivors = context.GetSurvivors?.Invoke();
            if (survivors == null) return 0f;

            bool hasCandidate = false;
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s == null || !s.IsAlive || s == context.Survivor) continue;
                if (IsMercyCandidate(s, context.MedicalSystem))
                {
                    hasCandidate = true;
                    break;
                }
            }
            if (!hasCandidate) return 0f;

            float score = baseScore;

            // Sociopath: no hesitation.
            if (context.Survivor.RiskBias == Survivors.RiskBiasTrait.Sociopath)
                score += 0.3f;

            // Empath: severe reluctance but may still do it out of compassion.
            if (context.Survivor.RiskBias == Survivors.RiskBiasTrait.Empath)
                score -= 0.2f;

            // High morale survivors are less willing to kill.
            float moraleFactor = Mathf.Lerp(0.5f, 1f,
                (100f - context.Survivor.Needs.Morale) / 100f);
            score *= moraleFactor;

            return Mathf.Clamp01(score);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || PerformMercyKill == null) return;

            var survivors = context.GetSurvivors?.Invoke();
            if (survivors == null) return;

            // Find first mercy candidate.
            Survivors.Survivor candidate = null;
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s == null || !s.IsAlive || s == context.Survivor) continue;
                if (IsMercyCandidate(s, context.MedicalSystem))
                {
                    candidate = s;
                    break;
                }
            }
            if (candidate == null) return;

            bool success = PerformMercyKill(candidate);
            if (!success) return;

            // Trait-driven consequences.
            var killer = context.Survivor;
            var trait = killer.RiskBias;

            if (trait == Survivors.RiskBiasTrait.Sociopath)
            {
                // No morale penalty. Other survivors are terrified.
                for (int i = 0; i < survivors.Count; i++)
                {
                    var w = survivors[i];
                    if (w == null || !w.IsAlive || w == killer || w == candidate) continue;
                    if (context.NeedsSystem != null)
                        context.NeedsSystem.Modify(w, NeedKind.Morale, -(8f));
                    else
                        w.Needs.Morale = Mathf.Clamp(w.Needs.Morale - 8f, 0f, 100f);
                    ApplyAffinityHit?.Invoke(killer, w, -15f);
                }
            }
            else if (trait == Survivors.RiskBiasTrait.Empath)
            {
                // Massive mourning: 10-day debuff.
                if (context.NeedsSystem != null)
                    context.NeedsSystem.Modify(killer, NeedKind.Morale, -(30f));
                else
                    killer.Needs.Morale = Mathf.Clamp(killer.Needs.Morale - 30f, 0f, 100f);
                // Mourning duration handled by external system via OnMercyKill event.
            }
            else if (trait == Survivors.RiskBiasTrait.Paranoid)
            {
                // Other survivors lose trust in the killer.
                for (int i = 0; i < survivors.Count; i++)
                {
                    var w = survivors[i];
                    if (w == null || !w.IsAlive || w == killer || w == candidate) continue;
                    ApplyAffinityHit?.Invoke(killer, w, -ParanoidTrustLoss);
                }
            }
            else
            {
                // Default: moderate morale hit + small affinity damage.
                if (context.NeedsSystem != null)
                    context.NeedsSystem.Modify(killer, NeedKind.Morale, -(15f));
                else
                    killer.Needs.Morale = Mathf.Clamp(killer.Needs.Morale - 15f, 0f, 100f);
                for (int i = 0; i < survivors.Count; i++)
                {
                    var w = survivors[i];
                    if (w == null || !w.IsAlive || w == killer || w == candidate) continue;
                    ApplyAffinityHit?.Invoke(killer, w, -5f);
                }
            }

            Debug.Log($"[MercyKill] {killer.DisplayName} ended {candidate.DisplayName}'s suffering.");
        }

        public static bool IsMercyCandidate(Survivors.Survivor sv, MedicalSystem medical)
        {
            if (sv == null || !sv.IsAlive) return false;
            if (medical == null) return false;

            // Coma patients.
            if (medical.IsComatose(sv)) return true;

            // Unhealable Acute Radiation Syndrome with very low health.
            if (sv.HasAcuteRadiationSyndrome && sv.Needs.Health < 20f) return true;

            // Terminal OrganFailure.
            if (sv.ActiveChronicIllness.HasValue
                && sv.ActiveChronicIllness.Value == Survivors.ChronicIllnessKind.OrganFailure
                && sv.Needs.Health < 15f)
                return true;

            return false;
        }
    }
}
