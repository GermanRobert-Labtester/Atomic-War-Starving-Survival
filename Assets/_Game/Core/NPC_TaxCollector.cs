using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class TaxCollectorState
    {
        public string npcId = "npc_tax_collector";
        public string factionId = "";
        public float taxPercentage = 0.25f;
        public bool isVisible = false;
        public bool hasArrived = false;
    }

    /// <summary>
    /// Prompt #665: NPC: Tax Collector.
    /// Armored Emissary from Super-Faction. Demands percentage of total wealth.
    /// Cannot fight → game-ending war. Must hide loot in FalseWalls.
    /// </summary>
    /// <summary>DEMOTE-NPC-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class NPC_TaxCollector
    {
        private TaxCollectorState _state = new TaxCollectorState();

        public event Action<TaxCollectorState> OnCollectorArrived;
        public event Action<TaxCollectorState, float> OnTaxCalculated;
        public event Action<TaxCollectorState, float> OnLootHidden;
        public event Action<TaxCollectorState, float> OnTaxPaid;

        public TaxCollectorState State => _state;

        public bool Arrive(float totalPlayerWealth)
        {
            if (_state.hasArrived)
                return false;

            _state.hasArrived = true;
            _state.isVisible = true;

            OnCollectorArrived?.Invoke(_state);
            return true;
        }

        public float CalculateTax(float wealth)
        {
            float tax = wealth * _state.taxPercentage;
            OnTaxCalculated?.Invoke(_state, tax);
            return tax;
        }

        public (float hidden, float exposed) TryHideLoot(float wealthToHide, int falseWallSlots)
        {
            if (falseWallSlots <= 0)
                return (0f, wealthToHide);

            float perSlot = wealthToHide / falseWallSlots;
            float hidden = perSlot * falseWallSlots;
            float exposed = wealthToHide - hidden;

            OnLootHidden?.Invoke(_state, hidden);
            return (hidden, exposed);
        }

        public bool PayTax(float amount)
        {
            if (!_state.hasArrived)
                return false;

            OnTaxPaid?.Invoke(_state, amount);
            return true;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public TaxCollectorState CaptureState() => _state;

        public void RestoreState(TaxCollectorState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
