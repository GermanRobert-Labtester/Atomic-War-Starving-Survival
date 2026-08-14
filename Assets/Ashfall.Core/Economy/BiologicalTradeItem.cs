namespace Ashfall.Core.Economy
{
    /// <summary>
    /// Virtual "items" that a faction can demand in lieu of material trade goods.
    /// Represents a piece of a survivor's body offered at the negotiating table:
    /// a pint of blood, a bone-marrow sample, a plasma unit, or a vital organ.
    /// Has no physical representation in the inventory; the cost is paid in
    /// need drain, inflicted afflictions, and interpersonal affinity damage.
    /// </summary>
    public enum BiologicalTradeItem
    {
        /// <summary>Standard whole-blood donation.</summary>
        PintOfBlood = 0,
        /// <summary>Bone-marrow aspirate.</summary>
        BoneMarrow = 1,
        /// <summary>Blood plasma only.</summary>
        Plasma = 2,
        /// <summary>Live organ (kidney, partial-liver). Irreversible.</summary>
        Organ = 3,
    }
}
