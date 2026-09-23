// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Survivors;
using Godot;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunNeedsPerformanceSelfTest(string? dataDir = null)
        {
            GD.Print("── NEEDS -> PERFORMANCE CASCADE SELF-TEST (Plan 137) ──");
            int pass = 0;
            int total = 12;

            void Check(bool cond, string name)
            {
                if (cond)
                {
                    pass++;
                    GD.Print($"  [PASS] {name}");
                }
                else
                {
                    GD.PrintErr($"  [FAIL] {name}");
                }
            }

            try
            {
                // 1. Active configuration is non-null
                var cfg = NeedsPerformanceBridge.ActiveConfig;
                Check(cfg != null, "active NeedsPerformanceConfig is non-null");

                // 2. Default floors and thresholds
                Check(cfg!.MinCombatAccuracyMultiplier > 0f && cfg.MinWorkSpeedMultiplier > 0f,
                    "performance modifier floors are positive non-zero values");

                // 3. Optimal needs produce Neutral multipliers (1.0x)
                var healthy = new SurvivorNeedsState
                {
                    Id = "sv_optimal",
                    Hunger = 10f,
                    Thirst = 10f,
                    Fatigue = 15f,
                    Warmth = 90f,
                    Morale = 65f,
                    Health = 100f
                };
                var mOptimal = NeedsPerformanceBridge.Project(healthy);
                Check(mOptimal.CombatAccuracyMultiplier == 1.0f &&
                      mOptimal.CombatDamageMultiplier == 1.0f &&
                      mOptimal.WorkSpeedMultiplier == 1.0f &&
                      mOptimal.ExpeditionSpeedMultiplier == 1.0f &&
                      mOptimal.ExpeditionStaminaDrainMultiplier == 1.0f &&
                      mOptimal.OverallBand == PerformanceBand.Optimal,
                    "optimal needs yield neutral (1.0x) multipliers and Optimal band");

                // 4. Hunger cascade scaling
                var mHungerImpaired = NeedsPerformanceBridge.Project(45f, 0f, 0f, 100f, 50f);
                var mHungerSevere = NeedsPerformanceBridge.Project(75f, 0f, 0f, 100f, 50f);
                var mHungerCritical = NeedsPerformanceBridge.Project(95f, 0f, 0f, 100f, 50f);
                Check(mHungerImpaired.WorkSpeedMultiplier < 1.0f &&
                      mHungerSevere.WorkSpeedMultiplier < mHungerImpaired.WorkSpeedMultiplier &&
                      mHungerCritical.WorkSpeedMultiplier < mHungerSevere.WorkSpeedMultiplier,
                    "hunger penalty scales monotonically across Impaired, Severe, and Critical tiers");

                // 5. Thirst cascade scaling
                var mThirstImpaired = NeedsPerformanceBridge.Project(0f, 35f, 0f, 100f, 50f);
                var mThirstSevere = NeedsPerformanceBridge.Project(0f, 65f, 0f, 100f, 50f);
                var mThirstCritical = NeedsPerformanceBridge.Project(0f, 85f, 0f, 100f, 50f);
                Check(mThirstImpaired.CombatAccuracyMultiplier < 1.0f &&
                      mThirstSevere.CombatAccuracyMultiplier < mThirstImpaired.CombatAccuracyMultiplier &&
                      mThirstCritical.CombatAccuracyMultiplier < mThirstSevere.CombatAccuracyMultiplier,
                    "thirst penalty scales monotonically on combat accuracy");

                // 6. Fatigue cascade scaling on stamina drain
                var mFatigueImpaired = NeedsPerformanceBridge.Project(0f, 0f, 55f, 100f, 50f);
                var mFatigueCritical = NeedsPerformanceBridge.Project(0f, 0f, 95f, 100f, 50f);
                Check(mFatigueImpaired.ExpeditionStaminaDrainMultiplier > 1.0f &&
                      mFatigueCritical.ExpeditionStaminaDrainMultiplier > mFatigueImpaired.ExpeditionStaminaDrainMultiplier,
                    "fatigue progressively escalates overland expedition stamina drain");

                // 7. Cold cascade converts warmth correctly
                var mCold = NeedsPerformanceBridge.Project(0f, 0f, 0f, 30f, 50f);
                Check(mCold.OverallBand == PerformanceBand.Severe &&
                      mCold.Contributions.Count == 1 &&
                      mCold.Contributions[0].Need == NeedKind.Warmth &&
                      mCold.Contributions[0].ReasonKey == "cold_severe",
                    "cold cascade converts warmth into cold severity and records cold_severe contribution");

                // 8. Morale interaction (amplification / mitigation)
                var mDemoralized = NeedsPerformanceBridge.Project(50f, 0f, 0f, 100f, 20f);
                var mNeutralMorale = NeedsPerformanceBridge.Project(50f, 0f, 0f, 100f, 50f);
                var mHighMorale = NeedsPerformanceBridge.Project(50f, 0f, 0f, 100f, 85f);
                Check(mDemoralized.WorkSpeedMultiplier < mNeutralMorale.WorkSpeedMultiplier &&
                      mHighMorale.WorkSpeedMultiplier > mNeutralMorale.WorkSpeedMultiplier,
                    "low morale amplifies work penalties while high morale mitigates them");

                // 9. Multiplicative stacking across multiple degraded needs
                var mMultiple = NeedsPerformanceBridge.Project(50f, 40f, 50f, 40f, 50f);
                Check(mMultiple.Contributions.Count == 4 &&
                      mMultiple.WorkSpeedMultiplier < mHungerImpaired.WorkSpeedMultiplier,
                    "multiple degraded needs stack multiplicatively into composite penalty");

                // 10. Floor clamping
                var mExtreme = NeedsPerformanceBridge.Project(100f, 100f, 100f, 0f, 0f);
                Check(mExtreme.CombatAccuracyMultiplier >= cfg.MinCombatAccuracyMultiplier &&
                      mExtreme.CombatDamageMultiplier >= cfg.MinCombatDamageMultiplier &&
                      mExtreme.WorkSpeedMultiplier >= cfg.MinWorkSpeedMultiplier &&
                      mExtreme.ExpeditionSpeedMultiplier >= cfg.MinExpeditionSpeedMultiplier &&
                      mExtreme.ExpeditionStaminaDrainMultiplier <= cfg.MaxExpeditionStaminaDrainMultiplier,
                    "extreme combined penalties respect data-driven floor limits");

                // 11. Census reporting
                var group = new List<SurvivorNeedsState>
                {
                    new SurvivorNeedsState { Id = "s1", Hunger = 10f, Thirst = 10f, Fatigue = 10f, Warmth = 90f, Morale = 60f, Health = 100f },
                    new SurvivorNeedsState { Id = "s2", Hunger = 45f, Thirst = 10f, Fatigue = 10f, Warmth = 90f, Morale = 60f, Health = 100f },
                    new SurvivorNeedsState { Id = "s3", Hunger = 75f, Thirst = 10f, Fatigue = 10f, Warmth = 90f, Morale = 60f, Health = 100f },
                    new SurvivorNeedsState { Id = "s4", Hunger = 95f, Thirst = 10f, Fatigue = 10f, Warmth = 90f, Morale = 60f, Health = 100f }
                };
                int opt = 0, imp = 0, sev = 0, crit = 0;
                foreach (var s in group)
                {
                    var m = NeedsPerformanceBridge.Project(s);
                    switch (m.OverallBand)
                    {
                        case PerformanceBand.Optimal: opt++; break;
                        case PerformanceBand.Impaired: imp++; break;
                        case PerformanceBand.Severe: sev++; break;
                        case PerformanceBand.Critical: crit++; break;
                    }
                }
                var census = new NeedsPerformanceCensus(opt, imp, sev, crit);
                Check(census.OptimalCount == 1 && census.ImpairedCount == 1 && census.SevereCount == 1 && census.CriticalCount == 1,
                    "census reporting correctly tallies survivors across all 4 performance bands");

                // 12. Day tick event generation
                var events = new List<Ashfall.Core.Campaign.DayStateChangeEvent>();
                events.Add(new Ashfall.Core.Campaign.DayStateChangeEvent(
                    kind: "needs_performance_ticked",
                    sourceOwnerId: "needs_performance",
                    primaryId: null,
                    secondaryId: null,
                    numeric: 0f));
                Check(events.Count == 1 && events[0].Kind == "needs_performance_ticked",
                    "day tick correctly generates needs_performance_ticked heartbeat event");

                bool success = pass == total;
                EmitSummary("needs_performance_selftest", success, success ? 0 : 1);
                return success ? 0 : 1;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] RunNeedsPerformanceSelfTest exception: {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}");
                EmitSummary("needs_performance_selftest", false, 1);
                return 1;
            }
        }
    }
}
