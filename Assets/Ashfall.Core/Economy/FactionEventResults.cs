namespace Ashfall.Core.Economy
{
    /// <summary>Result of a faction raid attempt.</summary>
    public class FactionRaidResult
    {
        public string FactionId;
        public bool Launched;
        public bool Repelled;
        public bool Breached;
        public float HatchDamage;
        public int ShieldingLevel;
        public float RaidStrength;
        public float DefenseScore;
        public float ShelterSecurity;
        public int StolenItemCount;
        public float Aggression;
        public bool SurrenderedAfter;
        public string Message;
    }

    /// <summary>Result of a faction succession event.</summary>
    public class FactionSuccessionResult
    {
        public string FactionId;
        public bool Applied;
        public string PreviousLeader;
        public string NewLeader;
        public int Generation;
        public float OldTrust;
        public float NewTrust;
        public float OldAggression;
        public float NewAggression;
        public string Message;
    }

    /// <summary>Result of a faction surrender demand.</summary>
    public class FactionSurrenderResult
    {
        public string FactionId;
        public bool Applied;
        public bool Auto;
        public float OldTrust;
        public float NewTrust;
        public float OldAggression;
        public float NewAggression;
        public TradeStance NewStance;
        public string Message;
    }

    /// <summary>Scarcity override descriptor (hardcore mode flag).</summary>
    public class ScarcityOverride
    {
        public string Source;
        public bool IsHardcore;
    }
}
