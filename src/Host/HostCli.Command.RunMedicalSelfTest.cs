// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Clock;
using Ashfall.Core.Crafting;
using Ashfall.Core.Economy;
using Ashfall.Core.Endgame;
using Ashfall.Core.Events;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Flags;
using Ashfall.Core.Legacy;
using Ashfall.Core.Medical;
using Ashfall.Core.Muster;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;
using Ashfall.Core.Settings;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.UtilityAI;
using Ashfall.Core.Verdict;
using Ashfall.Core.Warlords;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;
using AtomicWar.GodotApp.Narrative;
using AtomicWar.GodotApp.Settings;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.YearOfAsh;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {

        public static int RunMedicalSelfTest()
        {
            var report = MedicalHeadlessDemo.Run(new GodotLog());

            // MedicalHostSession TickHours & TickVigil runtime probe:
            try
            {
                var session = new MedicalHostSession();
                bool stateChangedInvoked = false;
                session.StateChanged += () => stateChangedInvoked = true;

                // 1. Initial clean state
                session.TickHours(24f);
                bool initialOk = session.TotalMoraleDrain == 0f;

                // 2. Consume doses and enter managed detox
                session.Engine.OnSubstanceConsumed("sv_patient_a", "morphine", ChemicalDependencyKind.Opioid);
                session.Engine.OnSubstanceConsumed("sv_patient_a", "morphine", ChemicalDependencyKind.Opioid);
                session.BeginDetoxDemo("sv_patient_a", "morphine", managed: true);

                // 3. Tick 24 hours under managed detox
                stateChangedInvoked = false;
                session.TickHours(24f);
                bool drainOk = session.TotalMoraleDrain > 0f;
                bool stateOk = stateChangedInvoked && session.IsDirty && session.StateVersion > 0;

                // 4. Tick remaining hours to complete detox (threshold is 96h)
                session.TickHours(100f);
                bool detoxCompleteOk = session.LastEvent.Contains("clean of morphine");

                // 5. Cold turkey withdrawal and tremor penalties
                session.Engine.OnSubstanceConsumed("sv_patient_b", "sedative_amp", ChemicalDependencyKind.Sedative);
                session.Engine.OnSubstanceConsumed("sv_patient_b", "sedative_amp", ChemicalDependencyKind.Sedative);
                session.BeginDetoxDemo("sv_patient_b", "sedative_amp", managed: false);
                session.TickHours(10f);
                bool penaltyActiveOk = session.ActiveCraftingPenalty > 0f && session.ActiveCombatPenalty > 0f;

                session.TickHours(70f);
                bool penaltyClearedOk = session.ActiveCraftingPenalty == 0f && session.ActiveCombatPenalty == 0f;

                // 6. Bedside Vigil ticking
                session.HoldVigil("sv_patient_c");
                bool vigilActiveOk = session.VigilActive;
                float progressBefore = session.VigilProgress;
                session.TickVigil(10.0);
                bool vigilTickedOk = session.VigilProgress > progressBefore;

                // 7. Zero/negative parameter safety
                session.TickHours(0f);
                session.TickHours(-5f);
                session.TickVigil(-1.0);

                bool allOk = initialOk && drainOk && stateOk && detoxCompleteOk && penaltyActiveOk && penaltyClearedOk && vigilActiveOk && vigilTickedOk;
                if (allOk)
                {
                    report.PassedCount++;
                    report.Checks.Add(new HeadlessCheck { Name = "MedicalHostSession.TickHours & TickVigil runtime probe", Passed = true });
                    GD.Print("[PASS] MedicalHostSession.TickHours & TickVigil advance dependencies, detox, penalties, and vigil progress");
                }
                else
                {
                    report.FailedCount++;
                    report.Passed = false;
                    report.Checks.Add(new HeadlessCheck { Name = "MedicalHostSession.TickHours & TickVigil runtime probe", Passed = false });
                    GD.Print($"[FAIL] MedicalHostSession tick probe failed: init={initialOk} drain={drainOk} state={stateOk} detox={detoxCompleteOk} penalty={penaltyActiveOk} cleared={penaltyClearedOk} vigil={vigilActiveOk} vigilTick={vigilTickedOk}");
                }
            }
            catch (Exception e)
            {
                report.FailedCount++;
                report.Passed = false;
                report.Checks.Add(new HeadlessCheck { Name = "MedicalHostSession.TickHours runtime probe exception", Passed = false });
                GD.Print("[FAIL] MedicalHostSession tick probe threw: " + e.Message);
            }

            GD.Print(report.Summary);
            return EmitSummaryFromHeadlessReport("medical_selftest", report);
        }

    }
}
