namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Kinds of chronic illnesses resulting from long-term nuclear fallout exposure or acute radiation syndrome (Prompt #39).
    /// </summary>
    public enum ChronicIllnessKind
    {
        /// <summary>Permanent lung scarring: caps max stamina at 60 and increases fatigue drain.</summary>
        LungFibrosis,
        /// <summary>Ocular damage: reduces surveying and scavenging yield by 50%.</summary>
        RadiationCataracts,
        /// <summary>Bone marrow suppression: accelerates fatigue accumulation by 2x.</summary>
        BoneMarrowDepression,
        /// <summary>Severe organ degradation: causes continuous health bleed.</summary>
        OrganFailure
    }
}
