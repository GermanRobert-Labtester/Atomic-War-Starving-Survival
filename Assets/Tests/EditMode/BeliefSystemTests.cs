using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Survivors;
using Ashfall.Core.Journal;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// EditMode tests for the per-survivor belief / risk-perception model: trait-
    /// differentiated Utility AI scoring, and belief updates from OBSERVED outcomes
    /// (never ground truth) — including the "broken instrument + survived anyway"
    /// feedback loop that can dangerously lower an overconfident survivor's perceived risk.
    /// </summary>
    [TestFixture]
    public class BeliefSystemTests
    {
        private static Survivor NewSurvivor(string id, RiskBiasTrait trait)
        {
            return new Survivor { Id = id, RiskBias = trait };
        }

        [Test]
        public void SameWorldState_ParanoidVsDenialist_ProduceDifferentTopAction()
        {
            var belief = new BeliefSystem(rng: new System.Random(1));

            var paranoid = NewSurvivor("paranoid", RiskBiasTrait.Paranoid);
            var denialist = NewSurvivor("denialist", RiskBiasTrait.Denialist);
            paranoid.Needs.Hunger = 60f;
            paranoid.Needs.Thirst = 60f;
            denialist.Needs.Hunger = 60f;
            denialist.Needs.Thirst = 60f;

            const float highUncertainty = 1f; // e.g. a broken/unreliable instrument

            var scavenge = ScriptableObject.CreateInstance<ScavengeActionSO>();
            var guard = ScriptableObject.CreateInstance<GuardActionSO>();
            var candidates = new SurvivorAction[] { scavenge, guard };

            var paranoidContext = new AIContext(paranoid)
            {
                MapUncertainty = highUncertainty,
                BeliefSystem = belief
            };
            var denialistContext = new AIContext(denialist)
            {
                MapUncertainty = highUncertainty,
                BeliefSystem = belief
            };

            var scorer = new ActionScorer();
            float paranoidScavengeScore = scorer.Score(scavenge, paranoidContext);
            float denialistScavengeScore = scorer.Score(scavenge, denialistContext);

            Assert.Less(paranoidScavengeScore, denialistScavengeScore,
                "Under identical world state and needs, a Paranoid survivor should be far less willing to scavenge than a Denialist.");

            var ai = new UtilityAI();
            var paranoidPick = ai.SelectAction(paranoidContext, candidates);
            var denialistPick = ai.SelectAction(denialistContext, candidates);

            Assert.AreNotSame(paranoidPick, denialistPick,
                "Same world state, different traits, should produce a different top Utility AI action.");
        }

        [Test]
        public void BrokenGeigerSurvivedHotTrip_LowersRecklessPerceivedRadRisk()
        {
            var belief = new BeliefSystem(rng: new System.Random(2));
            var reckless = NewSurvivor("reckless", RiskBiasTrait.Reckless);
            float initialRisk = reckless.PerceivedRadRisk;

            // "Broken geiger": full instrument uncertainty (Reliability -> 0 equivalent),
            // so the apparent reading the survivor saw was unreliable/possibly false-safe.
            const float brokenGeigerUncertainty = 1f;
            const float apparentReadingWhenTripped = 5f; // instrument showed a low, falsely-safe rad level

            // A sequence of trips, each "confirming" the broken geiger's false safety.
            belief.ObserveSurvivedHotTrip(reckless, apparentReadingWhenTripped, brokenGeigerUncertainty);
            belief.ObserveSurvivedHotTrip(reckless, apparentReadingWhenTripped, brokenGeigerUncertainty);
            belief.ObserveSurvivedHotTrip(reckless, apparentReadingWhenTripped, brokenGeigerUncertainty);

            Assert.Less(reckless.PerceivedRadRisk, initialRisk,
                "Surviving apparently-safe-but-actually-hot trips should lower perceived risk.");
            Assert.Less(reckless.PerceivedRadRisk, 0.15f,
                "Reckless overconfidence compounded over a survived sequence should drop this dangerously low.");
        }

        [Test]
        public void ObserveSicknessNearby_RaisesPerceivedRadRisk_MoreForParanoid()
        {
            var belief = new BeliefSystem(rng: new System.Random(3));

            var paranoid = NewSurvivor("paranoid", RiskBiasTrait.Paranoid);
            var denialist = NewSurvivor("denialist", RiskBiasTrait.Denialist);
            float paranoidBefore = paranoid.PerceivedRadRisk;
            float denialistBefore = denialist.PerceivedRadRisk;

            var sickSurvivor = NewSurvivor("sick", RiskBiasTrait.Realist);
            sickSurvivor.HasAcuteRadiationSyndrome = true;

            belief.ObserveSicknessNearby(paranoid, sickSurvivor);
            belief.ObserveSicknessNearby(denialist, sickSurvivor);

            float paranoidGain = paranoid.PerceivedRadRisk - paranoidBefore;
            float denialistGain = denialist.PerceivedRadRisk - denialistBefore;

            Assert.Greater(paranoidGain, 0f, "Witnessing sickness should raise perceived risk.");
            Assert.Greater(paranoidGain, denialistGain,
                "The same witnessed event should raise a Paranoid survivor's perceived risk far more than a Denialist's.");
        }

        [Test]
        public void AnxietyRisesWhenRiskAndUncertaintyBothHigh_NotWhenEitherLow()
        {
            const float hoursPerTick = 1f;
            const int ticks = 48;

            var bothHigh = NewSurvivor("both_high", RiskBiasTrait.Cautious);
            bothHigh.PerceivedRadRisk = 0.9f;
            var lowUncertainty = NewSurvivor("low_uncertainty", RiskBiasTrait.Cautious);
            lowUncertainty.PerceivedRadRisk = 0.9f;
            var lowRisk = NewSurvivor("low_risk", RiskBiasTrait.Cautious);
            lowRisk.PerceivedRadRisk = 0.05f;

            var belief = new BeliefSystem(rng: new System.Random(4));

            for (int i = 0; i < ticks; i++)
            {
                belief.Tick(bothHigh, currentMapUncertainty: 0.9f, gameHours: hoursPerTick);
                belief.Tick(lowUncertainty, currentMapUncertainty: 0.05f, gameHours: hoursPerTick);
                belief.Tick(lowRisk, currentMapUncertainty: 0.9f, gameHours: hoursPerTick);
            }

            Assert.IsTrue(bothHigh.HasRadiationAnxietyStatus,
                "High perceived risk AND high uncertainty together should trigger anxiety.");
            Assert.IsFalse(lowUncertainty.HasRadiationAnxietyStatus,
                "High risk but low uncertainty should not trigger anxiety.");
            Assert.IsFalse(lowRisk.HasRadiationAnxietyStatus,
                "Low risk but high uncertainty should not trigger anxiety.");
        }
    }
}
