using System;

namespace Ashfall.Core.Disease
{
    /// <summary>
    /// Typed pathogen containment capability projected from research systems.
    /// Invariant: Research never directly mutates disease simulation state or bed records;
    /// instead it projects this capability context into DiseaseQuarantineCoordinator and DiseaseSystem.
    /// </summary>
    public sealed class ContainmentCapability
    {
        public bool HasPathogenContainment { get; set; }

        /// <summary>
        /// Bounded bonus to isolation efficacy (0..0.15). Adds to base isolation quality.
        /// </summary>
        public float EfficacyBonus { get; set; }

        /// <summary>
        /// Bounded discount to daily care supply consumption (0..0.25).
        /// </summary>
        public float CareEfficiencyBonus { get; set; }

        /// <summary>
        /// Bounded diagnostic / monitoring confidence bonus (0..0.20).
        /// </summary>
        public float MonitoringBonus { get; set; }

        public static ContainmentCapability None => new ContainmentCapability();

        /// <summary>
        /// Pure projection from a research query delegate.
        /// </summary>
        public static ContainmentCapability FromResearch(Func<string, bool>? hasKnowledge)
        {
            if (hasKnowledge == null) return None;
            var cap = new ContainmentCapability();
            if (hasKnowledge("knowledge_pathogen_containment"))
            {
                cap.HasPathogenContainment = true;
                cap.EfficacyBonus = 0.10f; // Bounded, clamped bonus
                cap.CareEfficiencyBonus = 0.20f; // 20% care resource discount
                cap.MonitoringBonus = 0.15f;
            }
            return cap;
        }
    }
}
