using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Per-survivor belief / risk-perception model. Two survivors facing the same
    /// world state can hold different subjective danger senses (PerceivedRadRisk),
    /// updated only from OBSERVED outcomes (what a survivor personally witnessed or
    /// what their instrument apparently showed them) and biased by RiskBiasTrait —
    /// never from ground truth directly. This is what makes a Paranoid and a
    /// Denialist survivor in the same bunker act differently, and what lets a
    /// mis-calibrated/broken instrument (see AtomicWar._Game.Radiation.InstrumentDevice,
    /// AtomicWar._Game.Environment.RadiationKnowledgeMap) quietly poison a survivor's
    /// beliefs into a fatal decision.
    ///
    /// Plain C# system, no MonoBehaviour dependency, EditMode-testable — same
    /// ownership pattern as RadiationSystem: owns and mutates the belief fields on
    /// Survivor, exposes status-change events.
    /// </summary>
    public class BeliefSystem
    {
        public const float AnxietyThreshold = 0.6f;
        public const float NumbnessThreshold = 0.7f;

        /// <summary>Morale drained per game-hour while a survivor is anxious.</summary>
        public const float AnxietyMoraleDrainPerHour = 0.5f;

        /// <summary>Large fixed drop applied to Numbness after a near-death shock.</summary>
        public const float NumbnessShockRecovery = 0.6f;

        public event Action<Survivor, SurvivorStatus> OnStatusGained;
        public event Action<Survivor, SurvivorStatus> OnStatusLost;

        private readonly BeliefProfileCatalogSO _catalog;
        private readonly System.Random _rng;
        private NeedsSystem _needsSystem;

        public void SetNeedsSystem(NeedsSystem needsSystem)
        {
            _needsSystem = needsSystem;
        }

        public BeliefSystem(BeliefProfileCatalogSO catalog = null, System.Random rng = null)
        {
            _catalog = catalog;
            _rng = rng ?? AtomicWar._Game.Utilities.SeededRandom.Create(
                AtomicWar._Game.Utilities.SeededRandom.WorldSeed, "belief_system");
        }

        private BeliefProfileDefaults ProfileFor(RiskBiasTrait trait)
        {
            var asset = _catalog != null ? _catalog.GetByTrait(trait) : null;
            if (asset != null)
            {
                return BeliefProfileDefaults.FromAsset(asset);
            }
            return BeliefProfileDefaults.Defaults[trait];
        }

        /// <summary>
        /// Called when a survivor personally witnesses another survivor become sick
        /// (AcuteRadiationSickness / AcuteRadiationSyndrome). Raises PerceivedRadRisk,
        /// scaled by trait: Paranoid/Cautious over-react, Denialist/Reckless rationalize
        /// it away.
        /// </summary>
        public void ObserveSicknessNearby(Survivor observer, Survivor sickSurvivor)
        {
            if (observer == null || sickSurvivor == null) return;

            var profile = ProfileFor(observer.RiskBias);
            float gain = profile.ExperienceGainRate * profile.SicknessObservedGainMultiplier;
            observer.PerceivedRadRisk = Mathf.Clamp01(observer.PerceivedRadRisk + gain);
        }

        /// <summary>
        /// Called when a survivor returns from a scavenge/survey trip flagged as "hot"
        /// (high ambient rad) and did NOT get sick. The caller passes only the APPARENT
        /// reading the survivor's instrument showed at the time (post-calibration-bias,
        /// e.g. from InstrumentDevice.TryRead / RadiationKnowledgeMap.MeasuredRad) —
        /// ground truth is never consulted here. This is the "beliefs update from
        /// observed outcome, not truth" mechanic: a mis-calibrated instrument that
        /// under-reported danger, combined with the survivor surviving, causes a real
        /// (and for overconfident traits, dangerous) drop in perceived risk — even
        /// though nothing about the actual danger changed. A Denialist trait does NOT
        /// gate this update on a stale/unreliable flag; it ignores the caveat entirely.
        /// </summary>
        public void ObserveSurvivedHotTrip(Survivor survivor, float tripPeakApparentRadLevel, float instrumentUncertaintyAtTime)
        {
            if (survivor == null) return;

            var profile = ProfileFor(survivor.RiskBias);
            float uncertainty = Mathf.Clamp01(instrumentUncertaintyAtTime);
            float drop = profile.ExperienceDecayRate
                         * profile.SurvivedHotTripOverconfidenceMultiplier
                         * (1f - uncertainty * profile.UncertaintyDampens);
            survivor.PerceivedRadRisk = Mathf.Clamp01(survivor.PerceivedRadRisk - Mathf.Max(0f, drop));

            // Gut-vs-instrument trust nudge: a survivor who trusted the (apparently safe)
            // reading and survived trusts instruments a little more — regardless of trait,
            // since none of them know it was wrong. This is intentionally truth-blind.
            survivor.TrustInInstruments = Mathf.Clamp01(survivor.TrustInInstruments + 0.02f);
        }

        /// <summary>
        /// Per-tick belief drift and anxiety/numbness update. Call once per game-hour,
        /// same cadence as RadiationSystem.Tick. currentMapUncertainty is the caller's
        /// best current estimate of how unreliable the survivor's picture of danger is
        /// (e.g. 1 - RadiationKnowledgeMap.GetPlayerView(...).Confidence).
        /// </summary>
        public void Tick(Survivor survivor, float currentMapUncertainty, float gameHours)
        {
            if (survivor == null || gameHours <= 0f) return;

            var profile = ProfileFor(survivor.RiskBias);
            float uncertainty = Mathf.Clamp01(currentMapUncertainty);
            float risk = survivor.PerceivedRadRisk;

            // Anxiety needs BOTH perceived risk and uncertainty to be high.
            float anxietyTarget = Mathf.Clamp01(risk * uncertainty * 2f);
            survivor.RadiationAnxiety = Mathf.MoveTowards(
                survivor.RadiationAnxiety, anxietyTarget, profile.AnxietyGainRate * gameHours);

            // Numbness grows when perceived risk stays low, scaled by trait proneness.
            float numbnessTarget = Mathf.Clamp01((1f - risk) * profile.NumbnessProneness);
            survivor.Numbness = Mathf.MoveTowards(
                survivor.Numbness, numbnessTarget, profile.NumbnessGainRate * gameHours);

            bool wasAnxious = survivor.HasRadiationAnxietyStatus;
            survivor.HasRadiationAnxietyStatus = survivor.RadiationAnxiety >= AnxietyThreshold;
            if (survivor.HasRadiationAnxietyStatus && !wasAnxious)
            {
                OnStatusGained?.Invoke(survivor, SurvivorStatus.RadiationAnxiety);
            }
            else if (!survivor.HasRadiationAnxietyStatus && wasAnxious)
            {
                OnStatusLost?.Invoke(survivor, SurvivorStatus.RadiationAnxiety);
            }

            bool wasNumb = survivor.IsNumb;
            survivor.IsNumb = survivor.Numbness >= NumbnessThreshold;
            if (survivor.IsNumb && !wasNumb)
            {
                OnStatusGained?.Invoke(survivor, SurvivorStatus.Numb);
            }
            else if (!survivor.IsNumb && wasNumb)
            {
                OnStatusLost?.Invoke(survivor, SurvivorStatus.Numb);
            }

            // Anxiety erodes morale; numbness deliberately has no morale cost — the
            // survivor feels fine, which is the failure mode.
            if (survivor.HasRadiationAnxietyStatus)
            {
                if (_needsSystem != null)
                {
                    _needsSystem.Modify(survivor, NeedKind.Morale, -AnxietyMoraleDrainPerHour * gameHours);
                }
                else
                {
                    survivor.Needs.Morale = Mathf.Clamp(
                        survivor.Needs.Morale - AnxietyMoraleDrainPerHour * gameHours, 0f, 100f);
                }
            }
        }

        /// <summary>
        /// Multiplier applied to ScavengeActionSO's raw utility score. Blends gut-vs-
        /// instrument trust, perceived risk, trait baseline, and instrument uncertainty.
        /// A Paranoid survivor facing high uncertainty (e.g. a broken geiger) collapses
        /// toward 0; a Denialist trait is tuned to stay near its baseline regardless of
        /// uncertainty; numbness pushes the multiplier back up (a numb survivor doesn't
        /// hold back, whatever the apparent danger).
        /// </summary>
        public float ComputeScavengeMultiplier(Survivor survivor, float currentMapUncertainty)
        {
            if (survivor == null) return 1f;

            var profile = ProfileFor(survivor.RiskBias);
            float uncertainty = Mathf.Clamp01(currentMapUncertainty);
            float trust = Mathf.Clamp01(survivor.TrustInInstruments);
            float risk = Mathf.Clamp01(survivor.PerceivedRadRisk);

            // Low trust in instruments means the survivor leans on gut feeling instead,
            // so uncertainty in the (distrusted) instrument matters MORE, not less.
            float uncertaintyWeight = profile.ScavengeUncertaintyCurve.Evaluate(uncertainty);
            float effectiveDanger = Mathf.Lerp(risk, Mathf.Clamp01(risk + uncertaintyWeight), 1f - trust);

            float raw = profile.RiskBiasFactor * (1f - effectiveDanger) * (1f - survivor.RadiationAnxiety * 0.8f);
            raw = Mathf.Lerp(raw, 1.2f, survivor.Numbness * 0.5f);
            return Mathf.Clamp(raw, 0f, 1.5f);
        }

        /// <summary>
        /// Optional, dark cure for numbness: a near-death experience shocks the survivor
        /// back into caring. Wire this to a near-death signal (e.g. RadiationSystem
        /// granting AcuteRadiationSyndrome and the survivor recovering) — not called
        /// automatically by this class.
        /// </summary>
        public void ShockRecoverNumbness(Survivor survivor)
        {
            if (survivor == null) return;

            bool wasNumb = survivor.IsNumb;
            survivor.Numbness = Mathf.Max(0f, survivor.Numbness - NumbnessShockRecovery);
            survivor.IsNumb = survivor.Numbness >= NumbnessThreshold;
            if (wasNumb && !survivor.IsNumb)
            {
                OnStatusLost?.Invoke(survivor, SurvivorStatus.Numb);
            }
        }

        /// <summary>
        /// Plain-data mirror of BeliefProfileSO used as a hardcoded fallback so tests
        /// and code paths without a loaded catalog still get trait-differentiated
        /// behaviour (no ScriptableObject asset loading required).
        /// </summary>
        private readonly struct BeliefProfileDefaults
        {
            public readonly float ExperienceGainRate;
            public readonly float ExperienceDecayRate;
            public readonly float SicknessObservedGainMultiplier;
            public readonly float SurvivedHotTripOverconfidenceMultiplier;
            public readonly float UncertaintyDampens;
            public readonly float AnxietyGainRate;
            public readonly float NumbnessGainRate;
            public readonly float NumbnessProneness;
            public readonly float RiskBiasFactor;
            public readonly AnimationCurve ScavengeUncertaintyCurve;

            public BeliefProfileDefaults(
                float experienceGainRate, float experienceDecayRate,
                float sicknessObservedGainMultiplier, float survivedHotTripOverconfidenceMultiplier,
                float uncertaintyDampens, float anxietyGainRate, float numbnessGainRate,
                float numbnessProneness, float riskBiasFactor, AnimationCurve scavengeUncertaintyCurve)
            {
                ExperienceGainRate = experienceGainRate;
                ExperienceDecayRate = experienceDecayRate;
                SicknessObservedGainMultiplier = sicknessObservedGainMultiplier;
                SurvivedHotTripOverconfidenceMultiplier = survivedHotTripOverconfidenceMultiplier;
                UncertaintyDampens = uncertaintyDampens;
                AnxietyGainRate = anxietyGainRate;
                NumbnessGainRate = numbnessGainRate;
                NumbnessProneness = numbnessProneness;
                RiskBiasFactor = riskBiasFactor;
                ScavengeUncertaintyCurve = scavengeUncertaintyCurve;
            }

            public static BeliefProfileDefaults FromAsset(BeliefProfileSO asset)
            {
                return new BeliefProfileDefaults(
                    asset.ExperienceGainRate, asset.ExperienceDecayRate,
                    asset.SicknessObservedGainMultiplier, asset.SurvivedHotTripOverconfidenceMultiplier,
                    asset.UncertaintyDampens, asset.AnxietyGainRate, asset.NumbnessGainRate,
                    asset.NumbnessProneness, asset.RiskBiasFactor, asset.ScavengeUncertaintyCurve);
            }

            public static readonly IReadOnlyDictionary<RiskBiasTrait, BeliefProfileDefaults> Defaults =
                new Dictionary<RiskBiasTrait, BeliefProfileDefaults>
                {
                    // Paranoid: over-reacts to sickness, barely trusts a survived hot trip,
                    // very cautious in uncertainty, low Scavenge baseline.
                    [RiskBiasTrait.Paranoid] = new BeliefProfileDefaults(
                        0.15f, 0.05f, 1.8f, 0.3f, 0.2f, 0.15f, 0.01f, 0.05f, 0.35f,
                        AnimationCurve.Linear(0f, 0f, 1f, 1f)),

                    // Cautious: moderate version of Paranoid.
                    [RiskBiasTrait.Cautious] = new BeliefProfileDefaults(
                        0.15f, 0.08f, 1.3f, 0.6f, 0.35f, 0.08f, 0.015f, 0.1f, 0.7f,
                        AnimationCurve.Linear(0f, 0f, 1f, 0.8f)),

                    // Realist: baseline everything.
                    [RiskBiasTrait.Realist] = new BeliefProfileDefaults(
                        0.15f, 0.1f, 1f, 1f, 0.5f, 0.05f, 0.02f, 0.3f, 1f,
                        AnimationCurve.Linear(0f, 0f, 1f, 0.6f)),

                    // Reckless: shrugs off witnessed sickness, big overconfidence swing
                    // from a survived hot trip, prone to numbness, high Scavenge baseline.
                    [RiskBiasTrait.Reckless] = new BeliefProfileDefaults(
                        0.15f, 0.15f, 0.5f, 1.8f, 0.6f, 0.03f, 0.05f, 0.6f, 1.6f,
                        AnimationCurve.Linear(0f, 0f, 1f, 0.3f)),

                    // Denialist: lowest uptake from witnessed sickness, highest overconfidence
                    // swing, and a nearly-flat uncertainty curve — ignores a stale "safe"
                    // reading's caveat and keeps scavenging into a hot zone.
                    [RiskBiasTrait.Denialist] = new BeliefProfileDefaults(
                        0.15f, 0.2f, 0.3f, 2f, 0.1f, 0.02f, 0.06f, 0.7f, 1.7f,
                        AnimationCurve.Linear(0f, 0f, 1f, 0.1f)),

                    // Fatalist: "it'll happen or it won't" — low anxiety gain (nothing to be
                    // done about it), moderate numbness proneness, middling Scavenge baseline.
                    [RiskBiasTrait.Fatalist] = new BeliefProfileDefaults(
                        0.1f, 0.1f, 0.8f, 1.2f, 0.4f, 0.02f, 0.04f, 0.5f, 1.1f,
                        AnimationCurve.Linear(0f, 0f, 1f, 0.5f)),

                    // Empath: high anxiety gain (feels the bunker's pain), normal scavenge.
                    // Morale coupling is handled by EmpathSystem, not belief profiles.
                    [RiskBiasTrait.Empath] = new BeliefProfileDefaults(
                        0.15f, 0.08f, 1.4f, 0.7f, 0.4f, 0.10f, 0.02f, 0.2f, 1.0f,
                        AnimationCurve.Linear(0f, 0f, 1f, 0.5f)),

                    // Sociopath: nearly zero anxiety gain, very high numbness proneness,
                    // high scavenge baseline (cold-blooded efficiency).
                    [RiskBiasTrait.Sociopath] = new BeliefProfileDefaults(
                        0.05f, 0.25f, 0.2f, 2.5f, 0.1f, 0.01f, 0.1f, 0.9f, 1.8f,
                        AnimationCurve.Linear(0f, 0f, 1f, 0.05f)),
                };
        }
    }
}
