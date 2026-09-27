// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : SurvivorBodyPresentationSelfTest
// Core Authority     : Ashfall.Core.Medical.SurvivorBodyPresentationSlate (F14-G)
// Purpose            : survivor body state -> accessible limb presentation slate
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    public static class HostCliBodyPresentation
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Survivor Body Presentation Slate Self-Test (F14-G) ===");
            int passed = 0;
            const int total = 8;

            try
            {
                var intact = SurvivorBodyPresentationSlate.Project("survivor_01", SurvivorBodyState.CreateDefaultIntact());
                bool allIntact = true;
                foreach (var limb in intact.Limbs)
                {
                    if (limb.StatusText != "Intact" || limb.RequiresMaintenance) allIntact = false;
                }
                if (intact.EffectiveHands == 2 && intact.GripCapability == "Full Two-Hand Grip"
                    && intact.OverallMobilityPermille == 1000 && intact.SummaryStatus == "Combat Ready"
                    && intact.Limbs.Count == 4 && allIntact && intact.MaintenanceIssuesCount == 0)
                {
                    Console.WriteLine("[PASS] Check 1: an intact survivor renders two full-grip hands, 1000 mobility, combat ready.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 1: intact = {intact.EffectiveHands}/{intact.GripCapability}/{intact.OverallMobilityPermille}/{intact.SummaryStatus}."); }

                var limbLoss = SurvivorBodyState.CreateDefaultIntact();
                limbLoss.SetLimbCondition(SurvivorBodyState.LeftArmKey, "amputated");
                limbLoss.SetLimbCondition(SurvivorBodyState.RightArmKey, "prosthetized", "item_hook_hand");
                var oneHand = SurvivorBodyPresentationSlate.Project(
                    "survivor_02", limbLoss,
                    new Dictionary<string, int> { { "item_hook_hand", 900 } });
                bool leftAmputated = false, rightSimple = false;
                foreach (var limb in oneHand.Limbs)
                {
                    if (limb.LimbKey == SurvivorBodyState.LeftArmKey && limb.StatusText == "Amputated (No Prosthetic)") leftAmputated = true;
                    if (limb.LimbKey == SurvivorBodyState.RightArmKey && limb.GripContribution == "Simple Grip") rightSimple = true;
                }
                if (oneHand.EffectiveHands == 1 && oneHand.GripCapability == "Simple Only" && leftAmputated && rightSimple)
                {
                    Console.WriteLine("[PASS] Check 2: an amputated arm plus a simple hook reports one hand and Simple Only grip.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 2: one-hand = {oneHand.EffectiveHands}/{oneHand.GripCapability}."); }

                var degraded = SurvivorBodyState.CreateDefaultIntact();
                degraded.SetLimbCondition(SurvivorBodyState.RightLegKey, "prosthetized", "item_peg_leg");
                var peg = SurvivorBodyPresentationSlate.Project(
                    "survivor_03", degraded,
                    new Dictionary<string, int> { { "item_peg_leg", 200 } });
                bool pegNotice = false;
                foreach (var limb in peg.Limbs)
                {
                    if (limb.LimbKey == SurvivorBodyState.RightLegKey && limb.RequiresMaintenance && limb.MaintenanceNotice.Length > 0) pegNotice = true;
                }
                if (peg.OverallMobilityPermille < 1000 && peg.RequiresImmediateService && peg.MaintenanceIssuesCount > 0
                    && peg.SummaryStatus == "Operational with Restrictions" && pegNotice)
                {
                    Console.WriteLine($"[PASS] Check 3: a degraded peg leg drops mobility to {peg.OverallMobilityPermille} and flags service.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 3: degraded = mobility {peg.OverallMobilityPermille}, service {peg.RequiresImmediateService}."); }

                var phantomBody = SurvivorBodyState.CreateDefaultIntact();
                phantomBody.SetLimbCondition(SurvivorBodyState.LeftLegKey, "amputated");
                var phantom = SurvivorBodyPresentationSlate.Project("survivor_04", phantomBody, hasPhantomPain: true);
                if (phantom.HasPhantomPain && phantom.PhantomPainAlert.Contains("phantom limb pain")
                    && phantom.SummaryStatus == "Operational with Restrictions")
                {
                    Console.WriteLine("[PASS] Check 4: phantom pain renders a truthful alert and restricts the summary.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 4: phantom = {phantom.HasPhantomPain}/{phantom.SummaryStatus}."); }

                var bilateral = SurvivorBodyState.CreateDefaultIntact();
                bilateral.SetLimbCondition(SurvivorBodyState.LeftLegKey, "amputated");
                bilateral.SetLimbCondition(SurvivorBodyState.RightLegKey, "amputated");
                var grounded = SurvivorBodyPresentationSlate.Project("survivor_05", bilateral);
                if (grounded.OverallMobilityPermille == 0 && grounded.SummaryStatus == "Critically Impaired")
                {
                    Console.WriteLine("[PASS] Check 5: bilateral leg amputation is critically impaired at 0 mobility.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 5: bilateral = {grounded.OverallMobilityPermille}/{grounded.SummaryStatus}."); }

                var nullBody = SurvivorBodyPresentationSlate.Project("survivor_06", null);
                if (nullBody.EffectiveHands == 2 && nullBody.GripCapability == "Full Two-Hand Grip"
                    && nullBody.OverallMobilityPermille == 1000 && nullBody.Limbs.Count == 4)
                {
                    Console.WriteLine("[PASS] Check 6: a null body state falls back to the default intact slate.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 6: null body = {nullBody.EffectiveHands}/{nullBody.GripCapability}."); }

                bool keysOk = false;
                if (intact.Limbs.Count == 4)
                {
                    keysOk = intact.Limbs[0].LimbKey == SurvivorBodyState.LeftArmKey
                        && intact.Limbs[1].LimbKey == SurvivorBodyState.RightArmKey
                        && intact.Limbs[2].LimbKey == SurvivorBodyState.LeftLegKey
                        && intact.Limbs[3].LimbKey == SurvivorBodyState.RightLegKey;
                }
                if (keysOk)
                {
                    Console.WriteLine("[PASS] Check 7: the slate always renders the four canonical limb keys in order.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 7: limb keys/order wrong."); }

                var repeat = SurvivorBodyPresentationSlate.Project(
                    "survivor_03", degraded,
                    new Dictionary<string, int> { { "item_peg_leg", 200 } });
                var after = degraded.GetLimb(SurvivorBodyState.RightLegKey);
                if (repeat.SummaryStatus == peg.SummaryStatus && repeat.OverallMobilityPermille == peg.OverallMobilityPermille
                    && after.Condition == "prosthetized" && after.ProstheticItemId == "item_peg_leg")
                {
                    Console.WriteLine("[PASS] Check 8: projection is deterministic and does not mutate the body state.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 8: determinism/mutation check failed."); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unhandled: {ex.Message}");
            }

            Console.WriteLine($"Survivor body presentation: {passed}/{total} checks passed");
            return passed == total ? 0 : 1;
        }
    }
}
