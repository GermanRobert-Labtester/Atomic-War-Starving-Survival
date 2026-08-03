using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    // -------------------------------------------------------------------
    // Typed EventBus payloads for the Day-30 "caught outside" protocol.
    //
    // Publisher: FlashpointChoreographer's EMP step (the same step that
    //   runs EMPEvent.ApplyGlobal, forces Ashfall weather, and unpauses
    //   radiation). The choreographer publishes the signal AFTER the EMP
    //   has done its mechanical work so subscribers can read the resulting
    //   state (e.g. radio destroyed, devices broken).
    //
    // Subscribers: ExpeditionSystem (severs comms + applies trait-driven
    //   behavior on every active expedition). The GameBootstrap wires the
    //   subscription in InitializeSystems.
    //
    // Handlers must be idempotent: the choreography can publish the signal
    // more than once if the EMP step re-fires (it doesn't, but defensive).
    // -------------------------------------------------------------------

    /// <summary>
    /// Fired once on the Day-30 EMP step. Carries the EMP result so the
    /// ExpeditionSystem can correlate the intercept with what was actually
    /// broken (radio destroyed = no comms = no chance of recall).
    /// </summary>
    public readonly struct FlashpointInterceptSignal
    {
        /// <summary>Result of EMPEvent.ApplyGlobal at the time of the intercept.</summary>
        public readonly EmpResult EmpResult;

        /// <summary>Snapshot of expeditions caught outside at the time of the intercept.</summary>
        public readonly IReadOnlyList<ExpeditionState> InterceptedExpeditions;

        public FlashpointInterceptSignal(
            EmpResult empResult,
            IReadOnlyList<ExpeditionState> interceptedExpeditions)
        {
            EmpResult = empResult;
            InterceptedExpeditions = interceptedExpeditions;
        }
    }

    /// <summary>
    /// Fired by ExpeditionSystem when a comms-severed survivor reaches the
    /// hatch. The GameBootstrap (or a dedicated handler) subscribes and
    /// builds the dilemma GameEventSO with the three player choices.
    /// Carries the expedition so the handler can read contamination,
    /// afflictions, and trait for the choice copy and effects.
    /// </summary>
    public readonly struct HatchDilemmaReadySignal
    {
        public readonly ExpeditionState Expedition;
        /// <summary>True if the survivor is still alive (deny-entry will kill them).</summary>
        public readonly bool SurvivorIsAlive;

        public HatchDilemmaReadySignal(ExpeditionState expedition, bool survivorIsAlive)
        {
            Expedition = expedition;
            SurvivorIsAlive = survivorIsAlive;
        }
    }

    /// <summary>
    /// Fired by the dilemma handler after the player resolves the choice.
    /// The ExpeditionSystem listens and applies the consequence (kill
    /// survivor, complete with contamination, etc.) so the loop closes.
    /// </summary>
    public readonly struct HatchDilemmaResolvedSignal
    {
        public enum Resolution
        {
            LetThemIn,
            ForceDeconOutside,
            DenyEntry
        }

        public readonly string ExpeditionId;
        public readonly Resolution Choice;

        public HatchDilemmaResolvedSignal(string expeditionId, Resolution choice)
        {
            ExpeditionId = expeditionId;
            Choice = choice;
        }
    }
}
