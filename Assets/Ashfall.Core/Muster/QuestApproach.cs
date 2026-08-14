using System;

namespace Ashfall.Core.Muster
{
    /// <summary>
    /// The generic Approach fork used by every branching questline in
    /// Expansion 06 (The Muster). The enum carries no per-questline meaning:
    /// each questline's data documents what A/B/C/D means for it. Questlines
    /// that deliberately expose no real fork (The Provisioned, Iron Raiders)
    /// never receive a valid selection.
    /// </summary>
    public enum QuestApproach { A, B, C, D }

    /// <summary>
    /// The formalized Approach pattern from Expansion 06 Section XIII. Every
    /// Muster questline with a genuine fork implements this; selection routes
    /// to a distinct branch, faction-trust deltas, and an epilogue-matrix
    /// ending key. Host layers drive UI from these members and raise no
    /// gameplay rules of their own.
    /// </summary>
    public interface IApproachQuestline
    {
        string QuestlineId { get; }
        bool IsResolved { get; }
        QuestApproach? SelectedApproach { get; }

        /// <summary>Choose an approach. Returns false when the questline is
        /// already resolved or the approach is not offered by it.</summary>
        bool SelectApproach(QuestApproach approach);

        /// <summary>Ending-text key consumed by the epilogue matrix
        /// (Section XII). Empty until the questline resolves.</summary>
        string ResolveEndingKey();
    }
}
