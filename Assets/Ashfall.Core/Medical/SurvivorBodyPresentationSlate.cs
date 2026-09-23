// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Medical
{
    public sealed class LimbPresentationRow
    {
        public string LimbKey { get; }
        public string DisplayName { get; }
        public string ConditionTag { get; }
        public string StatusText { get; }
        public string GripContribution { get; }
        public int MobilityContributionPermille { get; }
        public bool RequiresMaintenance { get; }
        public string MaintenanceNotice { get; }

        public LimbPresentationRow(
            string limbKey,
            string displayName,
            string conditionTag,
            string statusText,
            string gripContribution,
            int mobilityContributionPermille,
            bool requiresMaintenance,
            string maintenanceNotice)
        {
            LimbKey = limbKey ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            ConditionTag = conditionTag ?? "intact";
            StatusText = statusText ?? string.Empty;
            GripContribution = gripContribution ?? "None";
            MobilityContributionPermille = Math.Clamp(mobilityContributionPermille, 0, 500);
            RequiresMaintenance = requiresMaintenance;
            MaintenanceNotice = maintenanceNotice ?? string.Empty;
        }
    }

    /// <summary>
    /// F14-G / UNBLOCK-01 §4.7 / §5.10: Survivor Body State UI Presentation Model.
    /// Pure projection over SurvivorBodyState, LimbState, and ProstheticWearEvaluationResult.
    /// Delivers accessible, words-not-color display slates for SurvivorDetailPanel.
    /// Engine-free, immutable, deterministic.
    /// </summary>
    public sealed class SurvivorBodyPresentationSlate
    {
        public string SurvivorId { get; }
        public IReadOnlyList<LimbPresentationRow> Limbs { get; }
        public int EffectiveHands { get; }
        public string GripCapability { get; }
        public int OverallMobilityPermille { get; }
        public bool HasPhantomPain { get; }
        public string PhantomPainAlert { get; }
        public int MaintenanceIssuesCount { get; }
        public bool RequiresImmediateService { get; }
        public string SummaryStatus { get; }

        public SurvivorBodyPresentationSlate(
            string survivorId,
            IReadOnlyList<LimbPresentationRow> limbs,
            int effectiveHands,
            string gripCapability,
            int overallMobilityPermille,
            bool hasPhantomPain,
            string phantomPainAlert,
            int maintenanceIssuesCount,
            bool requiresImmediateService,
            string summaryStatus)
        {
            SurvivorId = survivorId ?? string.Empty;
            Limbs = limbs ?? Array.Empty<LimbPresentationRow>();
            EffectiveHands = Math.Clamp(effectiveHands, 0, 2);
            GripCapability = gripCapability ?? "None";
            OverallMobilityPermille = Math.Clamp(overallMobilityPermille, 0, 1000);
            HasPhantomPain = hasPhantomPain;
            PhantomPainAlert = phantomPainAlert ?? string.Empty;
            MaintenanceIssuesCount = Math.Max(0, maintenanceIssuesCount);
            RequiresImmediateService = requiresImmediateService;
            SummaryStatus = summaryStatus ?? "Operational";
        }

        public static SurvivorBodyPresentationSlate Project(
            string survivorId,
            SurvivorBodyState? bodyState,
            Dictionary<string, int>? prostheticConditions = null,
            bool hasPhantomPain = false,
            Func<string, ItemDefinition?>? catalog = null)
        {
            if (bodyState == null)
            {
                bodyState = SurvivorBodyState.CreateDefaultIntact();
            }

            var rows = new List<LimbPresentationRow>(4);
            int totalHands = 0;
            bool hasFullGrip = false;
            bool hasAnyHand = false;
            int totalMobility = 0;
            int maintenanceIssues = 0;

            // Process Arms
            ProcessArm(bodyState, SurvivorBodyState.LeftArmKey, "Left Arm", prostheticConditions, catalog, rows, ref totalHands, ref hasFullGrip, ref hasAnyHand, ref maintenanceIssues);
            ProcessArm(bodyState, SurvivorBodyState.RightArmKey, "Right Arm", prostheticConditions, catalog, rows, ref totalHands, ref hasFullGrip, ref hasAnyHand, ref maintenanceIssues);

            // Process Legs
            ProcessLeg(bodyState, SurvivorBodyState.LeftLegKey, "Left Leg", prostheticConditions, catalog, rows, ref totalMobility, ref maintenanceIssues);
            ProcessLeg(bodyState, SurvivorBodyState.RightLegKey, "Right Leg", prostheticConditions, catalog, rows, ref totalMobility, ref maintenanceIssues);

            string gripCap;
            if (totalHands == 0)
            {
                gripCap = "No Grip";
            }
            else if (hasFullGrip)
            {
                gripCap = totalHands >= 2 ? "Full Two-Hand Grip" : "Full Single-Hand Grip";
            }
            else
            {
                gripCap = "Simple Only";
            }

            string phantomAlert = hasPhantomPain
                ? "Experiencing phantom limb pain; sleep disturbance likely."
                : string.Empty;

            bool requiresService = maintenanceIssues > 0;

            string summaryStatus;
            if (totalMobility < 300 || totalHands == 0)
            {
                summaryStatus = "Critically Impaired";
            }
            else if (requiresService || hasPhantomPain || totalMobility < 700)
            {
                summaryStatus = "Operational with Restrictions";
            }
            else
            {
                summaryStatus = "Combat Ready";
            }

            return new SurvivorBodyPresentationSlate(
                survivorId: survivorId,
                limbs: rows,
                effectiveHands: totalHands,
                gripCapability: gripCap,
                overallMobilityPermille: totalMobility,
                hasPhantomPain: hasPhantomPain,
                phantomPainAlert: phantomAlert,
                maintenanceIssuesCount: maintenanceIssues,
                requiresImmediateService: requiresService,
                summaryStatus: summaryStatus
            );
        }

        private static void ProcessArm(
            SurvivorBodyState bodyState,
            string limbKey,
            string displayName,
            Dictionary<string, int>? conditions,
            Func<string, ItemDefinition?>? catalog,
            List<LimbPresentationRow> rows,
            ref int totalHands,
            ref bool hasFullGrip,
            ref bool hasAnyHand,
            ref int maintenanceIssues)
        {
            var limb = bodyState.GetLimb(limbKey);
            string cond = limb.Condition.ToLowerInvariant();
            string statusText;
            string gripContribution;
            bool reqMaint = false;
            string maintNotice = string.Empty;

            switch (cond)
            {
                case "intact":
                case "wounded":
                case "infected":
                case "gangrenous":
                    statusText = cond == "intact" ? "Intact" : $"Natural ({Capitalize(cond)})";
                    gripContribution = "Full Grip";
                    totalHands += 1;
                    hasFullGrip = true;
                    hasAnyHand = true;
                    break;

                case "prosthetized":
                case "prosthetic":
                case "bionic":
                    int condPermille = 1000;
                    if (conditions != null && !string.IsNullOrEmpty(limb.ProstheticItemId) && conditions.TryGetValue(limb.ProstheticItemId, out var c))
                    {
                        condPermille = c;
                    }

                    var wearEval = ProstheticConditionWearEngine.EvaluateDailyWear(
                        currentConditionPermille: condPermille,
                        complexity: cond == "bionic" ? ProstheticComplexityClass.AdvancedArticulated : ProstheticComplexityClass.StandardMechanical,
                        laborIntensityPermille: 0,
                        maintenanceQualityPermille: 1000
                    );

                    reqMaint = wearEval.RequiresImmediateMaintenance;
                    if (reqMaint) maintenanceIssues++;
                    maintNotice = wearEval.MaintenanceStatus;

                    int condPercent = wearEval.NetConditionPermille / 10;
                    statusText = $"Prosthetic ({condPercent}% condition)";

                    if (wearEval.NetConditionPermille <= 0)
                    {
                        statusText = "Prosthetic (Broken / Inoperable)";
                        gripContribution = "None";
                    }
                    else
                    {
                        // Check item definition grip
                        bool itemFullGrip = false;
                        if (catalog != null && !string.IsNullOrEmpty(limb.ProstheticItemId))
                        {
                            var def = catalog(limb.ProstheticItemId);
                            if (def?.providesLimb != null && string.Equals(def.providesLimb.gripClass, "full", StringComparison.OrdinalIgnoreCase))
                            {
                                itemFullGrip = true;
                            }
                        }

                        totalHands += 1;
                        hasAnyHand = true;
                        if (itemFullGrip || cond == "bionic")
                        {
                            hasFullGrip = true;
                            gripContribution = "Full Grip";
                        }
                        else
                        {
                            gripContribution = "Simple Grip";
                        }
                    }
                    break;

                case "amputated":
                default:
                    statusText = "Amputated (No Prosthetic)";
                    gripContribution = "None";
                    break;
            }

            rows.Add(new LimbPresentationRow(
                limbKey: limbKey,
                displayName: displayName,
                conditionTag: cond,
                statusText: statusText,
                gripContribution: gripContribution,
                mobilityContributionPermille: 0,
                requiresMaintenance: reqMaint,
                maintenanceNotice: maintNotice
            ));
        }

        private static void ProcessLeg(
            SurvivorBodyState bodyState,
            string limbKey,
            string displayName,
            Dictionary<string, int>? conditions,
            Func<string, ItemDefinition?>? catalog,
            List<LimbPresentationRow> rows,
            ref int totalMobility,
            ref int maintenanceIssues)
        {
            var limb = bodyState.GetLimb(limbKey);
            string cond = limb.Condition.ToLowerInvariant();
            string statusText;
            int mobility = 0;
            bool reqMaint = false;
            string maintNotice = string.Empty;

            switch (cond)
            {
                case "intact":
                    statusText = "Intact";
                    mobility = 500;
                    break;
                case "wounded":
                    statusText = "Natural (Wounded)";
                    mobility = 350;
                    break;
                case "infected":
                case "gangrenous":
                    statusText = $"Natural ({Capitalize(cond)})";
                    mobility = 200;
                    break;

                case "prosthetized":
                case "prosthetic":
                case "bionic":
                    int condPermille = 1000;
                    if (conditions != null && !string.IsNullOrEmpty(limb.ProstheticItemId) && conditions.TryGetValue(limb.ProstheticItemId, out var c))
                    {
                        condPermille = c;
                    }

                    var wearEval = ProstheticConditionWearEngine.EvaluateDailyWear(
                        currentConditionPermille: condPermille,
                        complexity: cond == "bionic" ? ProstheticComplexityClass.AdvancedArticulated : ProstheticComplexityClass.StandardMechanical,
                        laborIntensityPermille: 0,
                        maintenanceQualityPermille: 1000
                    );

                    reqMaint = wearEval.RequiresImmediateMaintenance;
                    if (reqMaint) maintenanceIssues++;
                    maintNotice = wearEval.MaintenanceStatus;

                    int condPercent = wearEval.NetConditionPermille / 10;
                    statusText = $"Prosthetic ({condPercent}% condition)";

                    // Leg contributes up to 500 mobility scaled by biomechanical efficiency
                    mobility = (500 * wearEval.BiomechanicalEfficiencyPermille) / 1000;
                    break;

                case "amputated":
                default:
                    statusText = "Amputated (No Prosthetic)";
                    mobility = 0;
                    break;
            }

            totalMobility += mobility;

            rows.Add(new LimbPresentationRow(
                limbKey: limbKey,
                displayName: displayName,
                conditionTag: cond,
                statusText: statusText,
                gripContribution: "None",
                mobilityContributionPermille: mobility,
                requiresMaintenance: reqMaint,
                maintenanceNotice: maintNotice
            ));
        }

        private static string Capitalize(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            return char.ToUpperInvariant(text[0]) + text.Substring(1);
        }
    }
}
