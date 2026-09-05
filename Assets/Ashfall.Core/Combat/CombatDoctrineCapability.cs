using System;

namespace Ashfall.Core.Combat
{
    /// <summary>
    /// Typed combat capability projected from research/doctrine systems.
    /// Invariant: Research never directly mutates combat simulation state or UI;
    /// instead it projects this capability context into TacticalCombatSystem.
    /// </summary>
    public sealed class CombatDoctrineCapability
    {
        public bool HasCombatTraining { get; set; }
        public float AccuracyBonus { get; set; }
        public float RecoilMitigation { get; set; }
        public float TacticalMobilityBonus { get; set; }
        public bool HasFortifiedChokepoints { get; set; }
        public float BarrierIntegrityBonus { get; set; }

        public static CombatDoctrineCapability None => new CombatDoctrineCapability();

        /// <summary>
        /// Pure projection from a research query delegate.
        /// </summary>
        public static CombatDoctrineCapability FromResearch(Func<string, bool>? hasKnowledge)
        {
            if (hasKnowledge == null) return None;
            var cap = new CombatDoctrineCapability();
            if (hasKnowledge("knowledge_combat_training"))
            {
                cap.HasCombatTraining = true;
                cap.AccuracyBonus = 0.05f;
                cap.RecoilMitigation = 0.10f;
                cap.TacticalMobilityBonus = 0.05f;
            }
            if (hasKnowledge("knowledge_fortified_chokepoints"))
            {
                cap.HasFortifiedChokepoints = true;
                cap.BarrierIntegrityBonus = 0.20f;
            }
            return cap;
        }
    }
}
