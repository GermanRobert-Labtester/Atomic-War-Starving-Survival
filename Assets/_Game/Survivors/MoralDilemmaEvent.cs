using System;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Event data for moral dilemmas triggered under extreme starvation (Prompt #38).
    /// </summary>
    [Serializable]
    public class MoralDilemmaEvent
    {
        public string Id;
        public int Day;
        public float CriticalHunger;
        public int LivingSurvivorCount;
        public int DeadSurvivorCount;
        public bool IsResolved;
        public DesperateChoiceKind ChosenResolution;
        public string Message;
    }
}
