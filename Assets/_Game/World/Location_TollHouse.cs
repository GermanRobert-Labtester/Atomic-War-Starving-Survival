using System;
using System.Collections.Generic;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Location: The Toll House (Warlord Territory, Highway 9).
    /// Danger Level 8, Travel 4.0h, Base Rads 22 mSv/h.
    ///
    /// Once a highway maintenance depot. Now a warlord stronghold under Sable,
    /// who took over after Kael was killed on Day 47. The tribute ledger has
    /// 47 entries. Each entry is a shelter. Each shelter has a weekly payment.
    /// "Always leave one thing. The math must balance."
    /// </summary>
    [Serializable]
    public class TollHouseState
    {
        public string locationId = "kael_tribute_ledger_site";
        public string displayName = "The Toll House (Warlord Territory)";
        public float dangerLevel = 8f;
        public float travelHours = 4.0f;
        public float baseRadsPerHour = 22f;

        // Building state
        public bool garageLooted = false;
        public bool garageLockedBoxLooted = false;
        public bool fuelTankerSiphoned = false;
        public bool dispatchOfficeSearched = false;
        public bool dispatchOfficeDrawerOpened = false;
        public bool storageRoomLooted = false;
        public bool toolShedLooted = false;

        // Quest
        public bool tributeLedgerTaken = false;
        public bool kaelCodeTaken = false;
        public bool sableJournalTaken = false;

        // Holding cell
        public bool holdingCellOpened = false;
        public bool childFreed = false;
        public bool rabbitTaken = false;

        // Grave
        public bool graveVisited = false;
    }

    /// <summary>
    /// The Toll House. "Kael arrived on Day 18 with six men. He took the Toll House.
    /// He took the fuel from the tanker. He set up the first checkpoint. The first
    /// tribute was collected from a family of four in a sedan. He left them a
    /// flashlight. The flashlight was the beginning of the code."
    /// </summary>
    /// <summary>ASHDEEP-Location — The Toll House.</summary>
    public class Location_TollHouse
    {
        private TollHouseState _state = new TollHouseState();

        // ── Loot tables ────────────────────────────────────────────────────
        public static readonly List<string> GarageLoot = new List<string>
        {
            "weapon_shotgun_double", "wire_cutters"
        };

        public static readonly List<string> GarageLockedBoxLoot = new List<string>
        {
            "ammo_12ga_buck", "ammo_12ga_buck",
            "ammo_12ga_buck", "ammo_12ga_buck",
            "ammo_12ga_buck", "ammo_12ga_buck",
            "ammo_12ga_buck", "ammo_12ga_buck",
            "ammo_12ga_buck", "ammo_12ga_buck",
            "ammo_12ga_buck", "ammo_12ga_buck"
        };

        public static readonly List<string> FuelTankerLoot = new List<string>
        {
            "fuel_1l", "fuel_1l", "fuel_1l", "fuel_1l",
            "fuel_1l", "fuel_1l", "fuel_1l", "fuel_1l"
        };

        public static readonly List<string> StorageRoomLoot = new List<string>
        {
            "canned_food", "canned_food", "canned_food", "canned_food",
            "canned_food", "canned_food", "canned_food", "canned_food",
            "canned_food", "canned_food", "canned_food", "canned_food",
            "canned_food", "canned_food", "canned_food"
        };

        public static readonly List<string> ToolShedLoot = new List<string>
        {
            "crowbar", "crowbar"
        };

        // ── Events ─────────────────────────────────────────────────────────
        public event Action<TollHouseState, List<string>> OnLootCollected;
        public event Action<TollHouseState> OnHoldingCellOpened;
        public event Action<TollHouseState> OnChildFreed;
        public event Action<TollHouseState> OnGraveVisited;
        public event Action<TollHouseState> OnDispatchOfficeSearched;

        public TollHouseState State => _state;

        // ── Environmental storytelling ─────────────────────────────────────
        public static class EnvironmentalNarrative
        {
            public const string ArrivalDescription =
                "The Toll House was once a highway maintenance depot. Now it's a warlord stronghold. " +
                "The highway outside is lined with disabled vehicles — the original pileup, " +
                "reinforced with wire and scrap metal as a barricade. Through the gap, " +
                "a child's car seat. Empty. The seatbelt is still buckled.";

            public const string DispatchOfficeDescription =
                "The dispatch office. A chalkboard on the wall: 'THURSDAY COLLECTIONS. " +
                "WEEK 9. 47/47 PAID. ALL CLEAR.' Underneath, in different handwriting: '48 is late.'";

            public const string HoldingCellDescription =
                "The holding cell. Through the small window, a child sits on the floor " +
                "holding a stuffed rabbit. The child is not crying. The child is very still. " +
                "The child has been still for four days.";

            public const string KaelGraveDescription =
                "Kael's grave behind the tool shed. No headstone. Just a helmet on a stick. " +
                "The helmet is military. Kael was not military. He took it from a dead soldier. " +
                "He wore it for three weeks. Then he put it on the grave and never wore it again.";

            public const string KaelCodeDescription =
                "Kael's Code, framed on the dispatch office wall. Five rules, handwritten. " +
                "Rule 1: Always leave one thing. Rule 2: The math must balance. " +
                "Rule 3: Never take from children. Rule 4: Thursday is Thursday. " +
                "Rule 5: The code is the code.";

            public const string SableJournalDescription =
                "Sable's private journal. 'Kael died because he left too much. He left a blanket, " +
                "a flashlight, a can of food. He left them hope. Hope makes people stop paying. " +
                "I leave the flashlight. One thing. Just one. Enough to remember we're not monsters. " +
                "Not enough to forget we're coming Thursday.'";
        }

        // ── Methods ────────────────────────────────────────────────────────

        public List<string> LootGarage()
        {
            if (_state.garageLooted) return new List<string>();
            _state.garageLooted = true;
            var loot = new List<string>(GarageLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> LootLockedBox(string itemId)
        {
            if (_state.garageLockedBoxLooted) return new List<string>();
            if (itemId != "lockpick") return new List<string>();
            _state.garageLockedBoxLooted = true;
            var loot = new List<string>(GarageLockedBoxLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> SiphonFuelTanker()
        {
            if (_state.fuelTankerSiphoned) return new List<string>();
            _state.fuelTankerSiphoned = true;
            var loot = new List<string>(FuelTankerLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public void SearchDispatchOffice()
        {
            if (_state.dispatchOfficeSearched) return;
            _state.dispatchOfficeSearched = true;
            OnDispatchOfficeSearched?.Invoke(_state);
        }

        public string TakeTributeLedger()
        {
            if (_state.tributeLedgerTaken) return null;
            _state.tributeLedgerTaken = true;
            return "tribute_ledger";
        }

        public string TakeKaelCode()
        {
            if (_state.kaelCodeTaken) return null;
            _state.kaelCodeTaken = true;
            return "kael_code_document";
        }

        public bool OpenDispatchDrawer(string itemId)
        {
            if (_state.dispatchOfficeDrawerOpened) return true;
            if (itemId == "lockpick")
            {
                _state.dispatchOfficeDrawerOpened = true;
                return true;
            }
            return false;
        }

        public string TakeSableJournal()
        {
            if (_state.sableJournalTaken || !_state.dispatchOfficeDrawerOpened) return null;
            _state.sableJournalTaken = true;
            return "sable_journal";
        }

        public List<string> LootStorageRoom()
        {
            if (_state.storageRoomLooted) return new List<string>();
            _state.storageRoomLooted = true;
            var loot = new List<string>(StorageRoomLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> LootToolShed()
        {
            if (_state.toolShedLooted) return new List<string>();
            _state.toolShedLooted = true;
            var loot = new List<string>(ToolShedLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public bool OpenHoldingCell(string itemId)
        {
            if (_state.holdingCellOpened) return true;
            if (itemId == "crowbar" || itemId == "lockpick")
            {
                _state.holdingCellOpened = true;
                OnHoldingCellOpened?.Invoke(_state);
                return true;
            }
            return false;
        }

        public void FreeChild()
        {
            if (_state.childFreed || !_state.holdingCellOpened) return;
            _state.childFreed = true;
            OnChildFreed?.Invoke(_state);
        }

        public string TakeStuffedRabbit()
        {
            if (_state.rabbitTaken) return null;
            _state.rabbitTaken = true;
            return "stuffed_rabbit_child";
        }

        public void VisitGrave()
        {
            if (_state.graveVisited) return;
            _state.graveVisited = true;
            OnGraveVisited?.Invoke(_state);
        }

        /// <summary>Full loot sweep.</summary>
        public List<string> FullLootSweep(System.Random rng)
        {
            var allLoot = new List<string>();
            allLoot.AddRange(LootGarage());
            allLoot.AddRange(SiphonFuelTanker());
            allLoot.AddRange(LootStorageRoom());
            allLoot.AddRange(LootToolShed());
            SearchDispatchOffice();
            var ledger = TakeTributeLedger();
            if (ledger != null) allLoot.Add(ledger);
            var code = TakeKaelCode();
            if (code != null) allLoot.Add(code);
            var journal = TakeSableJournal();
            if (journal != null) allLoot.Add(journal);
            var rabbit = TakeStuffedRabbit();
            if (rabbit != null) allLoot.Add(rabbit);
            return allLoot;
        }

        // ── Save / Load ────────────────────────────────────────────────
        public TollHouseState CaptureState() => _state;
        public void RestoreState(TollHouseState saved)
        {
            if (saved != null) _state = saved;
        }
    }
}
