// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 20 — The Quiet Hand
// Subsystem    : Informant Network & Counter-Intelligence Tradecraft Engine
// Authority    : docs/expansions/wave2/expansion_20_the_quiet_hand_plan.md
//                UNBLOCK-05_EXPANSION_WAVES_C3_EN_GATE.md §3.1, §5.3
// ============================================================================
using System;

namespace Ashfall.Core.Espionage
{
    /// <summary>
    /// Archetype determining informant motivation, loyalty profile, and vulnerability to compromise.
    /// </summary>
    public enum InformantArchetype
    {
        IdeologicalDefector = 0,
        MercenaryBroker = 1,
        CoercedAsset = 2,
        EmbeddedOfficial = 3
    }

    /// <summary>
    /// Tradecraft communication channel used for intelligence transfer.
    /// </summary>
    public enum TradecraftMethod
    {
        DeadDrop = 0,
        DirectBriefing = 1,
        RadioBurstTransmission = 2,
        CutoutCourier = 3
    }

    /// <summary>
    /// State record of an active informant asset in the wasteland.
    /// </summary>
    public sealed class InformantRecord
    {
        public string InformantId { get; set; } = string.Empty;
        public string TargetFactionId { get; set; } = string.Empty;
        public InformantArchetype Archetype { get; set; } = InformantArchetype.IdeologicalDefector;
        public TradecraftMethod ActiveMethod { get; set; } = TradecraftMethod.DeadDrop;
        public int LoyaltyPermille { get; set; } = 700; // 0..1000
        public int SuspicionPermille { get; set; } = 100; // 0..1000
        public int IntelligenceYieldPermille { get; set; } = 500; // 0..1000
        public bool IsCompromised { get; set; }
        public bool IsDoubleAgent { get; set; }

        public InformantRecord Clone() => new InformantRecord
        {
            InformantId = InformantId,
            TargetFactionId = TargetFactionId,
            Archetype = Archetype,
            ActiveMethod = ActiveMethod,
            LoyaltyPermille = LoyaltyPermille,
            SuspicionPermille = SuspicionPermille,
            IntelligenceYieldPermille = IntelligenceYieldPermille,
            IsCompromised = IsCompromised,
            IsDoubleAgent = IsDoubleAgent
        };
    }

    /// <summary>
    /// Outcome of a tradecraft intelligence drop operation.
    /// </summary>
    public readonly struct TradecraftOperationResult
    {
        public bool Success { get; }
        public int IntelPointsDelivered { get; }
        public int SuspicionDeltaPermille { get; }
        public bool AssetCompromised { get; }
        public bool InterceptedByEnemy { get; }

        public TradecraftOperationResult(
            bool success,
            int intelPointsDelivered,
            int suspicionDeltaPermille,
            bool assetCompromised,
            bool interceptedByEnemy)
        {
            Success = success;
            IntelPointsDelivered = Math.Max(0, intelPointsDelivered);
            SuspicionDeltaPermille = suspicionDeltaPermille;
            AssetCompromised = assetCompromised;
            InterceptedByEnemy = interceptedByEnemy;
        }
    }

    /// <summary>
    /// Outcome of an interrogation protocol evaluation.
    /// </summary>
    public readonly struct InterrogationOutcome
    {
        public bool ReliableIntelligenceObtained { get; }
        public int CredibilityScorePermille { get; }
        public int MoraleCostPermille { get; }
        public bool FabricatedIntelWarning { get; }

        public InterrogationOutcome(
            bool reliableIntelligenceObtained,
            int credibilityScorePermille,
            int moraleCostPermille,
            bool fabricatedIntelWarning)
        {
            ReliableIntelligenceObtained = reliableIntelligenceObtained;
            CredibilityScorePermille = Math.Clamp(credibilityScorePermille, 0, 1000);
            MoraleCostPermille = Math.Max(0, moraleCostPermille);
            FabricatedIntelWarning = fabricatedIntelWarning;
        }
    }

    /// <summary>
    /// Pure domain engine governing wasteland informant networks, dead-drop tradecraft,
    /// suspicion accumulation, and counter-intelligence vetting.
    /// Zero engine references, integer permille determinism.
    /// </summary>
    public static class InformantNetworkTradecraftEngine
    {
        public const int CompromiseThresholdPermille = 800;

        /// <summary>
        /// Executes a tradecraft communication or dead-drop operation to retrieve intelligence.
        /// </summary>
        public static TradecraftOperationResult ExecuteTradecraftOperation(
            InformantRecord informant,
            long simTick,
            int worldSeed)
        {
            if (informant == null) throw new ArgumentNullException(nameof(informant));

            if (informant.IsCompromised)
            {
                return new TradecraftOperationResult(false, 0, 50, true, true);
            }

            // Method exposure risk permille: DeadDrop (100), DirectBriefing (400), RadioBurst (250), CutoutCourier (150)
            int methodRisk = informant.ActiveMethod switch
            {
                TradecraftMethod.DeadDrop => 100,
                TradecraftMethod.DirectBriefing => 400,
                TradecraftMethod.RadioBurstTransmission => 250,
                TradecraftMethod.CutoutCourier => 150,
                _ => 200
            };

            // Deterministic roll for operation detection/interception
            int hash = HashCode.Combine(worldSeed, informant.InformantId, simTick, (int)informant.ActiveMethod);
            int roll = Math.Abs(hash % 1000);

            bool intercepted = roll < (methodRisk + informant.SuspicionPermille / 4);
            int suspicionDelta = intercepted ? (methodRisk / 2) : 20;

            informant.SuspicionPermille = Math.Min(1000, informant.SuspicionPermille + suspicionDelta);
            if (informant.SuspicionPermille >= CompromiseThresholdPermille)
            {
                informant.IsCompromised = true;
            }

            if (intercepted)
            {
                return new TradecraftOperationResult(false, 0, suspicionDelta, informant.IsCompromised, true);
            }

            // Intelligence yield scaled by informant yield, loyalty, and method
            long rawYield = ((long)informant.IntelligenceYieldPermille * informant.LoyaltyPermille) / 1000;
            int intelPoints = Math.Max(1, (int)(rawYield / 50)); // 1..20 points

            return new TradecraftOperationResult(true, intelPoints, suspicionDelta, informant.IsCompromised, false);
        }

        /// <summary>
        /// Evaluates a counter-intelligence sweep against an informant to detect compromise or double-agency.
        /// </summary>
        public static bool DetectCompromisedAsset(
            InformantRecord informant,
            int shelterCounterIntelRatingPermille,
            long simTick,
            int worldSeed)
        {
            if (informant == null) throw new ArgumentNullException(nameof(informant));
            if (!informant.IsCompromised && !informant.IsDoubleAgent) return false;

            int detectionPower = Math.Clamp(shelterCounterIntelRatingPermille, 100, 1000);
            int hash = HashCode.Combine(worldSeed, informant.InformantId, simTick, "ci_sweep");
            int roll = Math.Abs(hash % 1000);

            return roll < detectionPower;
        }

        /// <summary>
        /// Evaluates prisoner or defector interrogation according to shelter ethical doctrine.
        /// Inhumane/coercive protocols produce fabricated intelligence and severe morale penalties.
        /// </summary>
        public static InterrogationOutcome EvaluateInterrogation(
            InformantRecord captive,
            bool humaneProtocolsEnforced)
        {
            if (captive == null) throw new ArgumentNullException(nameof(captive));

            if (humaneProtocolsEnforced)
            {
                // Humane interrogation builds rapport: loyalty rises slightly, intelligence is genuine
                int credibility = Math.Min(1000, captive.LoyaltyPermille + 200);
                return new InterrogationOutcome(
                    reliableIntelligenceObtained: true,
                    credibilityScorePermille: credibility,
                    moraleCostPermille: 0,
                    fabricatedIntelWarning: false);
            }
            else
            {
                // Coercive interrogation: captive invents false confessions to stop torture
                return new InterrogationOutcome(
                    reliableIntelligenceObtained: false,
                    credibilityScorePermille: 150, // very low actual truth
                    moraleCostPermille: 400,       // severe shelter demoralization
                    fabricatedIntelWarning: true);
            }
        }
    }
}
