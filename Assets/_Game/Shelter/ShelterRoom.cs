using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Shelter
{
        /// <summary>
    /// Unlock state for sealed/caved-in rooms (Prompt #5 — Previous Tenants).
    /// </summary>
    public enum RoomUnlockState
    {
        /// <summary>Room is accessible from game start.</summary>
        Unlocked,
        /// <summary>Room is sealed; requires rubble clearing (SurvivorAction).</summary>
        Sealed,
        /// <summary>Rubble is being cleared; in progress with work-hours remaining.</summary>
        Clearing,
        /// <summary>Room was sealed but has been cleared and is now accessible.</summary>
        Cleared
    }

    /// <summary>
    /// A room within the shelter: owns a set of storage slots with spatial adjacency,
    /// tracks ambient contamination level, and contributes to the shelter's indoor
    /// radiation when dirty. Save/load safe.
    /// </summary>
    [Serializable]
    public class ShelterRoom
    {
        /// <summary>Unique room id (e.g. "entry", "stores", "quarters").</summary>
        public string RoomId;

        /// <summary>Ambient contamination level (0..1) of the room air/surfaces.</summary>
        public float AmbientContamination;

        /// <summary>Ambient radiation level in rads/hr (Prompt #40).</summary>
        public float AmbientRadiation { get; set; }

        /// <summary>
        /// Room CO2 concentration in ppm (Prompt #20 / #48). DigOut exertion
        /// and sealed-hatch labor spike this in the entry room; high values
        /// drive atmosphere headache via SleepQuality foul-air thresholds.
        /// </summary>
        public float Co2Ppm { get; set; }

        /// <summary>True when room has mold infestation (#20, #37).</summary>
        public bool HasMold { get; set; }

        /// <summary>Mold severity level (0..1).</summary>
        public float MoldLevel { get; set; }

        /// <summary>Relative humidity 0..1 (Internal Horror — pantry rust / mold).</summary>
        public float Humidity { get; set; }

        /// <summary>
        /// Oxygen volume fraction 0..1 (ambient air ≈ 0.209). Fire and sealed
        /// bulkheads drive this down (Internal Horror — Fire in the Hole).
        /// </summary>
        public float OxygenFraction { get; set; } = DefaultOxygenFraction;

        /// <summary>Room-local CO ppm from fire / fouled air (bunker CO also on PowerNetwork).</summary>
        public float LocalCoPpm { get; set; }

        /// <summary>
        /// Rebar corrosion float (0.0 to 1.0) from carbonation depth & moisture.
        /// When it hits 1.0, triggers Event_Spalling.
        /// </summary>
        public float RebarCorrosion { get; set; }

        /// <summary>Carbonation depth float (0.0 to 1.0) advancing from CO2 and humidity.</summary>
        public float CarbonationDepth { get; set; }

        /// <summary>Material shielding value (0.0 to 1.0). Spalling concrete permanently degrades this.</summary>
        public float MaterialShielding { get; set; } = 1.0f;

        /// <summary>True if concrete spalling has occurred in this room.</summary>
        public bool IsSpalling { get; set; }

        /// <summary>True while a fire is active in this room.</summary>
        public bool IsOnFire { get; set; }

        /// <summary>Fire intensity 0..1 while IsOnFire.</summary>
        public float FireIntensity { get; set; }

        /// <summary>
        /// When true, bulkhead is sealed: no gas exchange with adjacent rooms.
        /// Used to starve a fire of oxygen at the cost of modules inside.
        /// </summary>
        public bool BulkheadSealed { get; set; }

        /// <summary>True after seal-to-extinguish sacrificed modules in this room.</summary>
        public bool ModulesSacrificed { get; set; }

        /// <summary>Ambient oxygen fraction of unpolluted air.</summary>
        public const float DefaultOxygenFraction = 0.209f;

        // -------------------------------------------------------------------
        // Room Unlock State (Prompt #5 — Previous Tenants)
        // -------------------------------------------------------------------

        /// <summary>Current unlock state for this room.</summary>
        public RoomUnlockState UnlockState = RoomUnlockState.Unlocked;

        /// <summary>Work-hours required to clear the rubble. Set when the room
        /// is Sealed; decremented by ClearRubbleActionSO. When zero, the room
        /// transitions to Cleared. Typical: 8-24 hours.</summary>
        public float RubbleClearHoursRemaining;

        /// <summary>Total rubble-clearing work-hours at seal (for UI bar).</summary>
        public float RubbleClearHoursTotal;

        /// <summary>
        /// Diary fragment ids found in this room. Populated when the room
        /// is sealed; revealed one by one as clearing progresses. These ids
        /// index into a DiaryFragmentSO catalog held by the game system.
        /// </summary>
        public System.Collections.Generic.List<string> DiaryFragmentIds = new System.Collections.Generic.List<string>();

        /// <summary>
        /// Which diary fragments have been revealed so far (indexes into DiaryFragmentIds).
        /// </summary>
        public System.Collections.Generic.List<int> RevealedDiaryIndices = new System.Collections.Generic.List<int>();

        /// <summary>Layout defining slot positions, adjacency, and transfer rates.</summary>
        [NonSerialized]
        public StorageLayoutSO Layout;

        /// <summary>Runtime storage slots built from the layout.</summary>
        [SerializeField]
        private List<StorageSlot> _slots = new List<StorageSlot>();

        public IReadOnlyList<StorageSlot> Slots => _slots;

        public event Action<ShelterRoom> OnContaminationChanged;
        public event Action<ShelterRoom, StorageSlot> OnSlotChanged;

        public ShelterRoom() { }

        public ShelterRoom(string roomId, StorageLayoutSO layout)
        {
            RoomId = roomId;
            Layout = layout;
            AmbientContamination = 0f;
            Humidity = 0.35f;
            OxygenFraction = DefaultOxygenFraction;
            LocalCoPpm = 0f;
            IsOnFire = false;
            FireIntensity = 0f;
            BulkheadSealed = false;
            ModulesSacrificed = false;
            if (layout != null)
            {
                _slots = layout.BuildSlots() ?? new List<StorageSlot>();
            }
        }

        /// <summary>Initialize slots from a pre-built list (for save/restore).</summary>
        public void SetSlots(List<StorageSlot> slots)
        {
            _slots = slots ?? new List<StorageSlot>();
        }

        /// <summary>
        /// Threshold above which ambient contamination applies a small indoor rad
        /// contribution and morale penalty to the shelter.
        /// </summary>
        public const float RadPenaltyThreshold = 0.2f;

        /// <summary>
        /// Threshold above which the room is visibly "dirty" (narrative cue).
        /// "the counter won't stop clicking inside."
        /// </summary>
        public const float VisibleThreshold = 0.4f;

        /// <summary>
        /// Indoor rad dose-rate this room contributes to the shelter interior.
        /// Zero below RadPenaltyThreshold; scales linearly above it.
        /// </summary>
        public float GetIndoorRadContribution()
        {
            if (AmbientContamination < RadPenaltyThreshold) return 0f;
            // Scale: 0 at threshold, up to ~5 rads/hr at contamination=1
            return (AmbientContamination - RadPenaltyThreshold) * 7.14f; // 0.8 * 7.14 ≈ 5.7 at 1.0
        }

        /// <summary>
        /// Morale penalty per hour from a contaminated room. Applied to all survivors
        /// in the shelter. "It smells wrong."
        /// </summary>
        public float GetMoralePenaltyPerHour()
        {
            if (AmbientContamination < RadPenaltyThreshold) return 0f;
            // Small penalty: up to -1 morale per hour at max contamination
            return -(AmbientContamination - RadPenaltyThreshold) * 1.25f;
        }

        /// <summary>
        /// Try to add an item with contamination to a specific slot. Returns the slot
        /// index used, or -1 if no valid slot found.
        /// </summary>
        public int AddItem(ItemDefinition item, int amount, int preferredSlot = -1)
        {
            if (item == null || amount <= 0 || _slots == null) return -1;

            // Try preferred slot first
            if (preferredSlot >= 0 && preferredSlot < _slots.Count && _slots[preferredSlot].IsEmpty)
            {
                if (_slots[preferredSlot].AddItem(item, amount))
                {
                    OnSlotChanged?.Invoke(this, _slots[preferredSlot]);
                    return preferredSlot;
                }
            }

            // Find first empty slot
            for (int i = 0; i < _slots.Count; i++)
            {
                if (_slots[i].IsEmpty)
                {
                    if (_slots[i].AddItem(item, amount))
                    {
                        OnSlotChanged?.Invoke(this, _slots[i]);
                        return i;
                    }
                }
            }

            // Try to merge with existing stack of same item
            for (int i = 0; i < _slots.Count; i++)
            {
                if (!_slots[i].IsEmpty && _slots[i].Item != null && _slots[i].Item.id == item.id)
                {
                    if (_slots[i].AddItem(item, amount))
                    {
                        OnSlotChanged?.Invoke(this, _slots[i]);
                        return i;
                    }
                }
            }

            return -1;
        }

        /// <summary>Remove an amount of an item from a specific slot.</summary>
        public bool RemoveFromSlot(int slotIndex, int amount)
        {
            if (slotIndex < 0 || slotIndex >= _slots.Count) return false;
            if (!_slots[slotIndex].RemoveItem(amount)) return false;
            OnSlotChanged?.Invoke(this, _slots[slotIndex]);
            return true;
        }

        /// <summary>
        /// Bring an item into this room: adds it to a slot AND deposits its contamination
        /// into the room's ambient level (the "dirty hatch entry" mechanic).
        /// Returns the slot index, or -1 if no room.
        /// </summary>
        public int BringIntoRoom(ItemDefinition item, int amount)
        {
            int slotIndex = AddItem(item, amount);
            if (slotIndex < 0) return -1;

            // Deposit contamination into ambient: item's contamination * amount * small factor
            float deposit = item.contamination * amount * 0.01f;
            AmbientContamination = Mathf.Clamp01(AmbientContamination + deposit);
            OnContaminationChanged?.Invoke(this);

            return slotIndex;
        }

        /// <summary>
        /// Ambient contribution from all items stored in this room (sum of each slot's
        /// contamination * amount, scaled). Drives the room's contamination upward.
        /// </summary>
        public float GetStoredContaminationLoad()
        {
            if (_slots == null) return 0f;
            float load = 0f;
            for (int i = 0; i < _slots.Count; i++)
            {
                if (_slots[i] != null && !_slots[i].IsEmpty)
                {
                    load += _slots[i].Contamination * _slots[i].Amount;
                }
            }
            return load;
        }

        /// <summary>
        /// Natural decay of the room's ambient contamination (e.g. ventilation, settling).
        /// </summary>
        public void DecayAmbient(float gameHours, float decayRatePerHour)
        {
            if (gameHours <= 0f) return;
            float oldVal = AmbientContamination;
            AmbientContamination = Mathf.Max(0f, AmbientContamination - decayRatePerHour * gameHours);
            if (AmbientContamination != oldVal)
            {
                OnContaminationChanged?.Invoke(this);
            }
        }
    }
}
