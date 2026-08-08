using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Empath & Sociopath trait variance (Prompt #8). The Empath gains/loses
    /// morale based on the average morale of the bunker, making them a fragile
    /// barometer for the group. The Sociopath suffers zero morale loss when
    /// another survivor dies — terrifying the others but making them the
    /// perfect cold-blooded scavenger.
    ///
    /// Plain C# system. Tick runs per game-hour; the death hook is called
    /// externally from the event that handles survivor death.
    /// </summary>
    public class EmpathSystem
    {
        /// <summary>How strongly the Empath's morale tracks the bunker average.
        /// 0.3 means they move 30% of the way toward the average each tick.</summary>
        public const float EmpathCouplingStrength = 0.3f;

        /// <summary>Maximum morale delta per hour from empathy (prevents wild swings).</summary>
        public const float MaxEmpathDeltaPerHour = 1.5f;

        /// <summary>Morale penalty applied to non-Sociopath survivors when another
        /// survivor dies. The Sociopath is exempt.</summary>
        public const float DeathMoralePenalty = 15f;

        /// <summary>Extra morale penalty applied by non-Sociopaths when they
        /// witness the Sociopath's indifference to death.</summary>
        public const float SociopathTerrifyPenalty = 5f;

        /// <summary>Fired when the Sociopath's indifference is witnessed by others.
        /// Args: (sociopath, terrifiedSurvivor).</summary>
        public event Action<Survivor, Survivor> OnSociopathTerrified;

        /// <summary>Fired when an Empath's morale is pulled by the bunker average.
        /// Args: (empath, delta).</summary>
        public event Action<Survivor, float> OnEmpathCoupled;

        private NeedsSystem _needsSystem;
        public void SetNeedsSystem(NeedsSystem ns) => _needsSystem = ns;

        // -----------------------------------------------------------------
        // Tick
        // -----------------------------------------------------------------

        /// <summary>
        /// Apply Empath coupling: the Empath's morale drifts toward the bunker
        /// average each tick. Called from GameBootstrap.
        /// </summary>
        public void Tick(float gameHours, IReadOnlyList<Survivor> survivors)
        {
            if (gameHours <= 0f || survivors == null || survivors.Count == 0) return;

            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive || sv.Needs == null) continue;

                if (sv.RiskBias == RiskBiasTrait.Empath)
                {
                    // Compute bunker average EXCLUDING this Empath (so they track the group, not themselves)
                    float bunkerAvg = ComputeBunkerAverageMorale(survivors, excludeSurvivor: sv);
                    ApplyEmpathCoupling(sv, bunkerAvg, gameHours);
                }
            }
        }

        private void ApplyEmpathCoupling(Survivor empath, float bunkerAvg, float gameHours)
        {
            if (empath?.Needs == null) return;
            float currentMorale = empath.Needs.Morale;
            float diff = bunkerAvg - currentMorale;

            // Move a fraction of the way toward the average each hour, capped
            float delta = diff * EmpathCouplingStrength * gameHours;
            delta = Mathf.Clamp(delta, -MaxEmpathDeltaPerHour * gameHours, MaxEmpathDeltaPerHour * gameHours);

            if (Mathf.Abs(delta) > 0.001f)
            {
                if (_needsSystem != null)
                    _needsSystem.Modify(empath, NeedKind.Morale, delta);
                else
                    empath.Needs.Morale = Mathf.Clamp(currentMorale + delta, 0f, 100f);
                OnEmpathCoupled?.Invoke(empath, delta);
            }
        }

        // -----------------------------------------------------------------
        // Death hook — called when any survivor dies
        // -----------------------------------------------------------------

        /// <summary>
        /// Call when <paramref name="deceased"/> dies. All non-Sociopath,
        /// non-deceased survivors suffer DeathMoralePenalty. If a Sociopath
        /// is in the bunker and shrugs it off, other survivors suffer an
        /// additional SociopathTerrifyPenalty.
        /// Returns true if a Sociopath was present and terrified others.
        /// </summary>
        public bool OnSurvivorDied(Survivor deceased, IReadOnlyList<Survivor> allSurvivors)
        {
            if (deceased == null || allSurvivors == null) return false;

            // Find if there's a living Sociopath
            Survivor sociopath = null;
            for (int i = 0; i < allSurvivors.Count; i++)
            {
                var sv = allSurvivors[i];
                if (sv != null && sv.IsAlive && sv.RiskBias == RiskBiasTrait.Sociopath)
                {
                    sociopath = sv;
                    break;
                }
            }

            bool sociopathTerrified = false;

            for (int i = 0; i < allSurvivors.Count; i++)
            {
                var sv = allSurvivors[i];
                if (sv == null || !sv.IsAlive || sv == deceased) continue;

                // Sociopath is immune to death morale loss
                if (sv.RiskBias == RiskBiasTrait.Sociopath) continue;
                if (sv.Needs == null) continue;

                if (_needsSystem != null)
                    _needsSystem.Modify(sv, NeedKind.Morale, -DeathMoralePenalty);
                else
                    sv.Needs.Morale = Mathf.Clamp(sv.Needs.Morale - DeathMoralePenalty, 0f, 100f);

                // If a Sociopath is present and unmoved, others are terrified
                if (sociopath != null)
                {
                    if (_needsSystem != null)
                        _needsSystem.Modify(sv, NeedKind.Morale, -SociopathTerrifyPenalty);
                    else
                        sv.Needs.Morale = Mathf.Clamp(sv.Needs.Morale - SociopathTerrifyPenalty, 0f, 100f);
                    sociopathTerrified = true;
                    OnSociopathTerrified?.Invoke(sociopath, sv);
                }
            }

            return sociopathTerrified;
        }

        // -----------------------------------------------------------------
        // Helpers
        // -----------------------------------------------------------------

        public static float ComputeBunkerAverageMorale(IReadOnlyList<Survivor> survivors, Survivor excludeSurvivor = null)
        {
            if (survivors == null || survivors.Count == 0) return 75f;

            float sum = 0f;
            int count = 0;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv != null && sv.IsAlive && sv.Needs != null && sv != excludeSurvivor)
                {
                    sum += sv.Needs.Morale;
                    count++;
                }
            }
            return count > 0 ? sum / count : 75f;
        }
    }
}
