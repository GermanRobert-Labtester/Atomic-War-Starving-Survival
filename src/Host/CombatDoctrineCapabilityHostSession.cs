// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Doctrine Capability host binding.
//
// TacticalCombatSystem already ADDS DoctrineCapability.AccuracyBonus to every
// shot and TacticalMobilityBonus to every mobility check, but nothing in the
// host ever assigned the property, so it stayed at CombatDoctrineCapability.None
// and researched doctrine changed nothing. This session projects the capability
// from the ONE knowledge authority (ResearchSystem.HasCapability) onto the live
// combat engine. No second knowledge cache, no new combat stat.
// ============================================================================

using System;
using Ashfall.Core;
using Ashfall.Core.Combat;
// ResearchSystem lives in Ashfall.Core namespace

namespace AtomicWar.GodotApp
{
    public sealed class CombatDoctrineCapabilityHostSession : HostSessionBase
    {
        private readonly Func<TacticalCombatSystem?> _combatProvider;
        private readonly Func<ResearchSystem?> _researchProvider;

        public string LastEvent { get; private set; } = string.Empty;
        public CombatDoctrineCapability Capability { get; private set; } = CombatDoctrineCapability.None;
        public int RecomputeCount { get; private set; }

        public CombatDoctrineCapabilityHostSession(
            Func<TacticalCombatSystem?> combatProvider,
            Func<ResearchSystem?> researchProvider)
        {
            _combatProvider = combatProvider ?? throw new ArgumentNullException(nameof(combatProvider));
            _researchProvider = researchProvider ?? throw new ArgumentNullException(nameof(researchProvider));
        }

        /// <summary>
        /// Recomputes the capability from the live research truth and assigns it to
        /// the live combat engine. Idempotent: repeat calls with unchanged knowledge
        /// produce an equal capability and never accumulate.
        /// </summary>
        public CombatDoctrineCapability Recompute()
        {
            var research = _researchProvider();
            Func<string, bool>? hasKnowledge = research == null
                ? (Func<string, bool>?)null
                : id => research.HasCapability(id);

            var capability = CombatDoctrineCapability.FromResearch(hasKnowledge);

            // The combat engine's own authored knowledge list is the only input; the
            // session caches nothing beyond the resulting projection.
            var combat = _combatProvider();
            if (combat != null) combat.DoctrineCapability = capability;

            Capability = capability;
            RecomputeCount++;
            LastEvent = capability.HasCombatTraining || capability.HasFortifiedChokepoints
                ? $"Doctrine applied: accuracy +{capability.AccuracyBonus:0.00}, mobility +{capability.TacticalMobilityBonus:0.00}, barrier +{capability.BarrierIntegrityBonus:0.00}."
                : "No combat doctrine researched.";
            RaiseStateChanged();
            return capability;
        }

        public bool IsAssignedToLiveEngine()
        {
            var combat = _combatProvider();
            return combat != null && ReferenceEquals(combat.DoctrineCapability, Capability);
        }

        public string StatusLine()
        {
            var c = Capability;
            if (!c.HasCombatTraining && !c.HasFortifiedChokepoints) return "doctrine: none researched";
            return "doctrine:"
                 + (c.HasCombatTraining ? " combat-training" : string.Empty)
                 + (c.HasFortifiedChokepoints ? " fortified-chokepoints" : string.Empty);
        }
    }
}
