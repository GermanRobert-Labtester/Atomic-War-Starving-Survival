using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class Action_HarvestOrgansState
    {
        public string actionId = "harvest_organs";
        public float moralePenalty = -40f;
        public float organTradeValue = 500f;
        public bool requiresSurgeon = true;
        public bool hasBeenUsed = false;
    }

    /// <summary>DEMOTE-Action-remaining — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_HarvestOrgans
    {
        public Action_HarvestOrgansState State { get; private set; }

        public event Action<string, int, float> OnOrgansHarvested;
        public event Action<string> OnHarvestFailed;

        public Action_HarvestOrgans()
        {
            State = new Action_HarvestOrgansState();
        }

        public Action_HarvestOrgans(Action_HarvestOrgansState state)
        {
            State = state ?? new Action_HarvestOrgansState();
        }

        public (int organs, float moraleDelta) Harvest(string corpseId, bool isSurgeon)
        {
            if (string.IsNullOrEmpty(corpseId))
            {
                OnHarvestFailed?.Invoke("Invalid corpse ID");
                return (0, 0f);
            }

            if (State.requiresSurgeon && !isSurgeon)
            {
                OnHarvestFailed?.Invoke("Requires surgeon");
                return (0, 0f);
            }

            int organsYielded = UnityEngine.Random.Range(1, 4);
            float moraleDelta = State.moralePenalty;

            State.hasBeenUsed = true;

            OnOrgansHarvested?.Invoke(corpseId, organsYielded, moraleDelta);

            return (organsYielded, moraleDelta);
        }

        public float GetTradeValue()
        {
            return State.organTradeValue;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public Action_HarvestOrgansState CaptureState() => State;

        public void RestoreState(Action_HarvestOrgansState saved)
        {
            if (saved == null) return;
            State = saved;
        }

}
}
