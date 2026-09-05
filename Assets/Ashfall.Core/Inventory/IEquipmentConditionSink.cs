using System;

namespace Ashfall.Core.Inventory
{
    /// <summary>
    /// Sink for equipment degradation and wear write-backs (Task 44 / Plan 21).
    /// Prevents mutation of disconnected read projections by routing condition changes
    /// directly to the canonical inventory equipment authority.
    /// </summary>
    public interface IEquipmentConditionSink
    {
        /// <summary>
        /// Record wear on an equipped item, decrementing its durability and notifying subscribers.
        /// </summary>
        void RecordWear(EquippedItem item, float wearDelta, string cause = "radiation");
    }
}
