using System;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public sealed class JuggernautArmorState
    {
        public string itemId = "item_juggernaut_armor";
        public bool immuneToSmallArms = true;
        public float speedMultiplier = 0.1f;
        public bool canFlee = false;
        public bool isEquipped;
        public string equippedBySurvivorId = "";
    }

    public sealed class Item_JuggernautArmor
    {
        public event Action<string> OnArmorEquipped;  // (survivorId)
        public event Action<string> OnArmorRemoved;   // (survivorId)

        private JuggernautArmorState _state = new JuggernautArmorState();

        // Equip the scrap-plate juggernaut armor on a survivor.
        // Grants: immunity to small arms, speed reduced to 10%, cannot flee.
        public void Equip(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId))
                throw new ArgumentNullException(nameof(survivorId));
            if (_state.isEquipped)
                throw new InvalidOperationException("Juggernaut armor is already equipped.");

            _state.isEquipped = true;
            _state.equippedBySurvivorId = survivorId;
            OnArmorEquipped?.Invoke(survivorId);
        }

        // Remove the armor from the currently-equipped survivor.
        public void Remove()
        {
            if (!_state.isEquipped) return;

            string prev = _state.equippedBySurvivorId;
            _state.isEquipped = false;
            _state.equippedBySurvivorId = "";
            OnArmorRemoved?.Invoke(prev);
        }

        // Speed multiplier — 0.1 means 10% of normal speed.
        public float GetSpeedMultiplier() => _state.isEquipped ? _state.speedMultiplier : 1f;

        // Whether the wearer can attempt to flee. Always false while equipped.
        public bool CanFlee() => !_state.isEquipped || _state.canFlee;

        // Whether the wearer is immune to small-arms fire.
        public bool IsImmuneToSmallArms() => _state.isEquipped && _state.immuneToSmallArms;

        public bool IsEquipped => _state.isEquipped;
        public string EquippedBy => _state.equippedBySurvivorId;

        // --- Save / Load -----------------------------------------------------
        public JuggernautArmorState CaptureState() => new JuggernautArmorState
        {
            itemId = _state.itemId,
            immuneToSmallArms = _state.immuneToSmallArms,
            speedMultiplier = _state.speedMultiplier,
            canFlee = _state.canFlee,
            isEquipped = _state.isEquipped,
            equippedBySurvivorId = _state.equippedBySurvivorId
        };

        public void RestoreState(JuggernautArmorState saved)
        {
            _state.itemId = saved.itemId;
            _state.immuneToSmallArms = saved.immuneToSmallArms;
            _state.speedMultiplier = saved.speedMultiplier;
            _state.canFlee = saved.canFlee;
            _state.isEquipped = saved.isEquipped;
            _state.equippedBySurvivorId = saved.equippedBySurvivorId;
        }
    }
}
