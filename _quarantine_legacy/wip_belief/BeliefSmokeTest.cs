using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Random = System.Random;
using Object = UnityEngine.Object;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Headless acceptance test: the SAME shelter, same supplies, same seed, and same
    /// persistently-uncertain hot zone produce measurably different outcomes purely
    /// because of which RiskBiasTrait mix is assigned to the roster. Each survivor's
    /// hourly go/no-go decision is driven directly by BeliefSystem.ComputeScavengeMultiplier
    /// (rather than the full Utility AI action-selection stack) so the outcome divergence
    /// is attributable to the belief model itself. Reuses SurvivalSmokeTest's
    /// BalanceTracker/BalanceRecord for outcome bookkeeping.
    /// </summary>
    [TestFixture]
    public class BeliefSmokeTest
    {
        private const float HoursPerDay = 24f;
        private const int SimDays = 20;
        private const float TotalHours = SimDays * HoursPerDay;

        /// <summary>Ambient rad rate of the one scavengeable "hot zone" all rosters face.</summary>
        private const float HotZoneRadPerHour = 15f;

        /// <summary>Constant instrument uncertainty for the hot zone — a persistently unreliable read.</summary>
        private const float HotZoneUncertainty = 0.8f;

        /// <summary>Minimum belief-adjusted Scavenge multiplier for a survivor to go out this hour.</summary>
        private const float ScavengeDecisionThreshold = 0.5f;

        private class RunResult
        {
            public int TotalArsCount;
            public float TotalPeakRadiation;
            public float TotalDaysSurvived;
            public int TotalScavengeHours;
        }

        private RunResult RunSimulation(int seed, RiskBiasTrait[] traits)
        {
            var needsProfile = ScriptableObject.CreateInstance<NeedsProfile>();
            needsProfile.hungerPerHour = 2f;
            needsProfile.thirstPerHour = 3f;
            needsProfile.hungerCritical = 100f;
            needsProfile.thirstCritical = 100f;
            needsProfile.healthLossFromHunger = 1f;
            needsProfile.healthLossFromThirst = 1f;

            var needsSystem = new NeedsSystem(needsProfile, sv => true);
            var radSystem = new RadiationSystem(needsSystem, rng: new Random(seed));
            var belief = new BeliefSystem(rng: new Random(seed));
            var tracker = new BalanceTracker();

            var survivors = new List<Survivor>();
            for (int i = 0; i < traits.Length; i++)
            {
                var sv = new Survivor { Id = $"sv_{i}", RiskBias = traits[i] };
                survivors.Add(sv);
                needsSystem.Register(sv);
                radSystem.Register(sv);
                tracker.Register(sv);
            }

            var result = new RunResult();

            for (float hour = 0; hour < TotalHours; hour++)
            {
                float day = hour / HoursPerDay + 1f;
                needsSystem.Tick(1f);

                foreach (var sv in survivors)
                {
                    if (!sv.IsAlive) continue;

                    belief.Tick(sv, HotZoneUncertainty, 1f);
                    float multiplier = belief.ComputeScavengeMultiplier(sv, HotZoneUncertainty);

                    if (multiplier >= ScavengeDecisionThreshold)
                    {
                        result.TotalScavengeHours++;
                        radSystem.Expose(sv, HotZoneRadPerHour, 1f);
                        if (sv.IsAlive)
                        {
                            // The instrument under-reports danger by HotZoneUncertainty; the
                            // survivor only ever sees this apparent reading, not the truth.
                            float apparentReading = HotZoneRadPerHour * (1f - HotZoneUncertainty);
                            belief.ObserveSurvivedHotTrip(sv, apparentReading, HotZoneUncertainty);
                        }
                    }
                }

                tracker.Tick(1f, day, survivors);
            }

            tracker.FinalizeAlive(SimDays, survivors);
            Object.DestroyImmediate(needsProfile);

            foreach (var r in tracker.Records)
            {
                if (r.DevelopedARS) result.TotalArsCount++;
                result.TotalPeakRadiation += r.PeakRadiation;
                result.TotalDaysSurvived += r.DaysSurvived;
            }
            return result;
        }

        [Test]
        public void SameShelterAndSupplies_ThreeTraitMixes_ProduceThreeDifferentOutcomes()
        {
            const int seed = 7;

            var overCautious = RunSimulation(seed, new[]
            {
                RiskBiasTrait.Paranoid, RiskBiasTrait.Paranoid, RiskBiasTrait.Cautious
            });
            var overconfident = RunSimulation(seed, new[]
            {
                RiskBiasTrait.Reckless, RiskBiasTrait.Denialist, RiskBiasTrait.Reckless
            });
            var balanced = RunSimulation(seed, new[]
            {
                RiskBiasTrait.Realist, RiskBiasTrait.Fatalist, RiskBiasTrait.Realist
            });

            // The three trait mixes must not all land on the same outcome.
            bool allIdentical =
                overCautious.TotalScavengeHours == overconfident.TotalScavengeHours &&
                overconfident.TotalScavengeHours == balanced.TotalScavengeHours &&
                Mathf.Approximately(overCautious.TotalPeakRadiation, overconfident.TotalPeakRadiation) &&
                Mathf.Approximately(overconfident.TotalPeakRadiation, balanced.TotalPeakRadiation);
            Assert.IsFalse(allIdentical,
                "Same shelter, same supplies, same seed — the three trait mixes should not converge on an identical outcome.");

            // Directional check: an over-cautious (Paranoid/Cautious) roster, whose belief-
            // adjusted Scavenge multiplier never clears the go/no-go threshold under this
            // persistent instrument uncertainty, should scavenge far less and absorb far
            // less radiation than an overconfident (Reckless/Denialist) roster, whose
            // multiplier stays high (and gets reinforced by surviving each trip).
            Assert.Less(overCautious.TotalScavengeHours, overconfident.TotalScavengeHours,
                "Paranoid/Cautious roster should go out far less than a Reckless/Denialist roster under the same uncertainty.");
            Assert.Less(overCautious.TotalPeakRadiation, overconfident.TotalPeakRadiation,
                "Paranoid/Cautious roster should absorb far less radiation than a Reckless/Denialist roster.");
        }
    }
}
