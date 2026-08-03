namespace AtomicWar._Game.Economy
{
    /// <summary>
    /// Virtual "items" that a faction can demand in lieu of material trade goods.
    /// Represents a piece of a survivor's body offered at the negotiating table:
    /// a pint of blood, a bone-marrow sample, a plasma unit, or a vital organ.
    /// Has no physical representation in the inventory; the cost is paid in
    /// <see cref="AtomicWar._Game.Survivors.Survivor"/> Need drain, inflicted
    /// afflictions, and InterpersonalAffinity damage.
    ///
    /// When the player has run out of material trade goods and a faction's
    /// trade-stance hits <see cref="TradeStance.Rob"/>, the next negotiation
    /// may surface a BiologicalTradeItem demand — the faction will ask for a
    /// piece of one of the bunker survivors in exchange for water, iodine,
    /// or medicine. The player can accept (pay the cost) or refuse (lose
    /// trust, raise the chance of a hatch raid).
    ///
    /// Mirrors the design intent of Prompt #47 — when there is nothing left
    /// to trade, factions will ask for pieces of the player.
    /// </summary>
    public enum BiologicalTradeItem
    {
        /// <summary>Standard whole-blood donation. Triggers BloodLossAffliction;
        /// cure in ~7 days with high food/water. Carries Infection risk from
        /// the dirty needle.</summary>
        PintOfBlood = 0,

        /// <summary>Bone-marrow aspirate. Longer-term consequence: chronic
        /// fatigue and lowered immune response for 30+ days. Worth more
        /// than a pint to medical factions.</summary>
        BoneMarrow = 1,

        /// <summary>Blood plasma only. Lighter than PintOfBlood (no
        /// BloodLossAffliction), but the donor is fatigued for 48 hours
        /// and may faint if already malnourished.</summary>
        Plasma = 2,

        /// <summary>Live organ (kidney, partial-liver). Irreversible.
        /// Permanent stat cap + a long-term Affliction. Reserved for
        /// end-game desperate trades — factions will only ask for this
        /// when their commander is actively dying and trust ≥ 60.</summary>
        Organ = 3,
    }
}
