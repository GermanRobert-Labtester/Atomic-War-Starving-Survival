using System;
using System.Collections.Generic;
using System.Text;

namespace Ashfall.Core.Endgame
{
    public enum RegionalFate { CommonwealthFounded, GarrisonMartialLaw, FracturedWarlords, TempestSterilization, TrueReconciliation }
    public enum DemographicOutcome { ThrivingCommunity, HardenedSurvivors, GhostShelter, TotalExtinction }
    public enum MoralStanding { ForgivenAndReconciled, IndenturedDebtState, RuthlessPragmatists }

    [Serializable]
    public sealed class EpilogueEvaluationContext
    {
        public int totalDaysSurvived;
        public int livingDwellerCount;
        public int totalDeathsRecorded;
        public bool grandTreatySigned;
        public bool tempestDecommissioned;
        public bool debtLedgersBurned;
        public bool childrenSurvived;
        public bool velSecretExposed;
    }

    /// <summary>
    /// ASHFALL: THE END OF THE LINE — 32-Permutation Epilogue Matrix Runtime.
    /// Evaluates whole-saga world state flags across 360 to 3,650 days and generates
    /// the authoritative, literary-grade endgame chronicle.
    /// </summary>
    public sealed class EpilogueMatrixRuntime
    {
        public RegionalFate EvaluateRegionalFate(EpilogueEvaluationContext ctx)
        {
            if (ctx == null) return RegionalFate.FracturedWarlords;

            if (ctx.grandTreatySigned && ctx.tempestDecommissioned && ctx.debtLedgersBurned)
                return RegionalFate.TrueReconciliation;

            if (ctx.grandTreatySigned && ctx.debtLedgersBurned)
                return RegionalFate.CommonwealthFounded;

            if (ctx.grandTreatySigned && !ctx.debtLedgersBurned)
                return RegionalFate.GarrisonMartialLaw;

            if (!ctx.tempestDecommissioned && ctx.totalDeathsRecorded > 50)
                return RegionalFate.TempestSterilization;

            return RegionalFate.FracturedWarlords;
        }

        public DemographicOutcome EvaluateDemographics(EpilogueEvaluationContext ctx)
        {
            if (ctx == null || ctx.livingDwellerCount <= 0)
                return DemographicOutcome.TotalExtinction;

            if (ctx.livingDwellerCount >= 8 && ctx.childrenSurvived)
                return DemographicOutcome.ThrivingCommunity;

            if (ctx.livingDwellerCount >= 3)
                return DemographicOutcome.HardenedSurvivors;

            return DemographicOutcome.GhostShelter;
        }

        public MoralStanding EvaluateMoralStanding(EpilogueEvaluationContext ctx)
        {
            if (ctx == null) return MoralStanding.RuthlessPragmatists;

            if (ctx.debtLedgersBurned && ctx.childrenSurvived)
                return MoralStanding.ForgivenAndReconciled;

            if (!ctx.debtLedgersBurned)
                return MoralStanding.IndenturedDebtState;

            return MoralStanding.RuthlessPragmatists;
        }

        public string GenerateEpilogueNarrative(EpilogueEvaluationContext ctx)
        {
            var fate = EvaluateRegionalFate(ctx);
            var demo = EvaluateDemographics(ctx);
            var moral = EvaluateMoralStanding(ctx);

            var sb = new StringBuilder();
            sb.AppendLine("=== SAGA EPILOGUE: THE CHRONICLE OF TESSARAT ===");
            sb.AppendLine($"Simulation Span: {ctx.totalDaysSurvived} Days. Final Census: {ctx.livingDwellerCount} Living, {ctx.totalDeathsRecorded} Inscribed.");
            sb.AppendLine();

            // Regional Fate Text
            switch (fate)
            {
                case RegionalFate.TrueReconciliation:
                    sb.AppendLine("The treaty was ratified without an execution clause. The Tempest's radio tone fell silent on 99.0 MHz. " +
                                  "Across the valley, the high-voltage busbars were grounded into heating coils for the green fields. " +
                                  "The old world is gone, but the children walk on clean soil without counting their paces.");
                    break;
                case RegionalFate.CommonwealthFounded:
                    sb.AppendLine("A civilian commonwealth was declared at the weighbridge. The soldiers laid down their rifles on the salt flats. " +
                                  "The winter was hard, but the corn grew in the spring and no man owed his neighbor for his breath.");
                    break;
                case RegionalFate.GarrisonMartialLaw:
                    sb.AppendLine("The Garrison enforced the curfew with iron discipline. The chimneys smoke on schedule, the bread is weighed to the gram, " +
                                  "and the sentries on the intake towers never sleep.");
                    break;
                case RegionalFate.TempestSterilization:
                    sb.AppendLine("The machine's countdown reached zero. The loitering drones swept the river flats with cold, mechanical precision. " +
                                  "The snow falls on empty concrete, and the geophone pulses into the dead bedrock forever.");
                    break;
                default:
                    sb.AppendLine("The council collapsed in gunfire. The province shattered into six warring outposts, each hoarding their own coal and watching the road with loaded carbines.");
                    break;
            }
            sb.AppendLine();

            // Demographic Text
            switch (demo)
            {
                case DemographicOutcome.ThrivingCommunity:
                    sb.AppendLine("The nursery school at the foundry is full. The children learn to read from old service manuals, and they do not flinch when the thunder rolls.");
                    break;
                case DemographicOutcome.HardenedSurvivors:
                    sb.AppendLine("A handful of grey-haired founders sit around the central stove. Their coats are patched with tarred canvas, and they speak only when the kettle boils.");
                    break;
                case DemographicOutcome.GhostShelter:
                    sb.AppendLine("Only one dweller remains to sweep the corridor. When they go to sleep, they leave the lantern lit in the entryway so the dark feels less heavy.");
                    break;
                default:
                    sb.AppendLine("The bunker doors swung open when the last hinge rusted through. The wind carried dead pine needles into the sleeping bunks.");
                    break;
            }
            sb.AppendLine();

            // Moral Closure
            switch (moral)
            {
                case MoralStanding.ForgivenAndReconciled:
                    sb.AppendLine("The debt ledgers were burned in the furnace. Nobody in the valley carries another man's name stamped on a copper tag.");
                    break;
                case MoralStanding.IndenturedDebtState:
                    sb.AppendLine("The promissory notes survived the bombs. Every harvest, the Syndicate enforcers collect their interest in grain and young labor.");
                    break;
                default:
                    sb.AppendLine("Survival was bought with another's share. The memory of what was done remains etched into the slate walls, heavy and un-washed.");
                    break;
            }

            return sb.ToString();
        }
    }
}
