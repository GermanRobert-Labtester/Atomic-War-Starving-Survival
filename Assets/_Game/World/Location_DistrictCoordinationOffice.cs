using System;
using System.Collections.Generic;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Location: The District Coordination Office (Market Street).
    /// Danger Level 4, Travel 1.5h, Base Rads 12 mSv/h.
    ///
    /// A two-story concrete building where 4,200 civilian records were processed.
    /// The filing cabinets are intact. The records are complete. This is the most
    /// dangerous object in Tessarat — whoever holds these files holds leverage
    /// over every surviving family.
    /// </summary>
    [Serializable]
    public class DistrictCoordinationOfficeState
    {
        public string locationId = "district_coordination_office";
        public string displayName = "The District Coordination Office";
        public float dangerLevel = 4f;
        public float travelHours = 1.5f;
        public float baseRadsPerHour = 12f;

        // Building state
        public bool backOfficeLocked = true;
        public bool wallSafeOpened = false;
        public bool vendingMachineLooted = false;
        public bool cashDrawerLooted = false;
        public bool firstAidKitLooted = false;
        public bool maintenanceClosetLooted = false;
        public bool curtainsTaken = false;
        public bool stampTaken = false;
        public bool keysTaken = false;
        public bool flashlightTaken = false;

        // Records
        public int civilianRecordsRemaining = 4200; // 4,200 total, 1,050 per drawer
        public int filingCabinetDrawersOpened = 0;   // 0-4 drawers
    }

    /// <summary>
    /// The DCO processed 4,200 civilian records. Birth, death, ration allocation,
    /// shelter assignment, conscription deferral. Every citizen of Tessarat existed
    /// in this building as a number in a filing cabinet. The filing cabinet is still
    /// there. The numbers are still in it. The people are mostly not.
    /// </summary>
    /// <summary>ASHDEEP-Location — The District Coordination Office.</summary>
    public class Location_DistrictCoordinationOffice
    {
        private DistrictCoordinationOfficeState _state = new DistrictCoordinationOfficeState();

        // ── Loot tables ────────────────────────────────────────────────────
        public static readonly List<string> VendingMachineLoot = new List<string>
        {
            "canned_food", "canned_food", "canned_food", "canned_food"
        };

        public static readonly List<string> CashDrawerLoot = new List<string>
        {
            "currency" // 30 pre-war bills, useless
        };

        public static readonly List<string> FirstAidKitLoot = new List<string>
        {
            "water_purification_tablet", "water_purification_tablet",
            "water_purification_tablet", "water_purification_tablet",
            "water_purification_tablet", "water_purification_tablet"
        };

        public static readonly List<string> MaintenanceClosetLoot = new List<string>
        {
            "duct_tape", "duct_tape", "duct_tape"
        };

        public static readonly List<string> CurtainLoot = new List<string>
        {
            "cloth", "cloth", "cloth", "cloth",
            "cloth", "cloth", "cloth", "cloth"
        };

        public static readonly List<string> BackOfficeLoot = new List<string>
        {
            "compliance_ledger" // Wall safe — locked, needs lockpick or dco_key_ring
        };

        public static readonly List<string> FilingCabinetLoot = new List<string>
        {
            "civilian_record_files" // One per cabinet drawer
        };

        // ── Events ─────────────────────────────────────────────────────────
        public event Action<DistrictCoordinationOfficeState, List<string>> OnLootCollected;
        public event Action<DistrictCoordinationOfficeState> OnBackOfficeUnlocked;
        public event Action<DistrictCoordinationOfficeState> OnWallSafeOpened;
        public event Action<DistrictCoordinationOfficeState, int> OnFilingCabinetSearched;

        public DistrictCoordinationOfficeState State => _state;

        // ── Environmental storytelling ─────────────────────────────────────
        public static class EnvironmentalNarrative
        {
            public const string ArrivalDescription =
                "The DCO still stands. Windows blown out, but the concrete held. " +
                "The queue barrier is still set up. Number tickets scattered on the floor. " +
                "The last number dispensed is 4,187. There were 4,200 people in Tessarat. " +
                "Thirteen never got their number.";

            public const string LobbyDescription =
                "Six service windows face an empty waiting area. Plastic chairs bolted to the floor. " +
                "A water cooler that was always empty — still empty. Window 6 has a handwritten sign: " +
                "'TEMPORARILY CLOSED. PLEASE RETURN MONDAY.' The Monday never came.";

            public const string WindowThreeDescription =
                "Window Three. A stamp on the desk. A clerk sat here for eleven days after the exchange, " +
                "stamping blank paper. She said if she stopped stamping, she'd have to think about " +
                "the fact that the forms were for people who weren't coming back. " +
                "The stamp is still here. It reads: 'NEXT, PLEASE.'";

            public const string BathroomMirrorDescription =
                "The bathroom mirror. Someone wrote in the condensation — now frozen: " +
                "'I was here. I was number 2,847. I had a name.'";

            public const string LobbyFlagDescription =
                "The flag pole outside is bare. The flag is in the lobby, folded neatly on the " +
                "reception desk. Nobody took it. Nobody flew it. It's just there.";

            public const string BackOfficeDescription =
                "The back office is locked. Inside: the District Coordinator's desk, a locked drawer, " +
                "and a wall safe. The safe contains the master compliance ledger — the document that " +
                "determines who gets rations. The Garrison wants it. The Militia wants it destroyed.";

            public const string FilingCabinetsDescription =
                "Four metal cabinets, each with 1,050 civilian records. Names, addresses, family " +
                "compositions, medical histories, compliance ratings. Complete. This is the most " +
                "dangerous object in Tessarat.";
        }

        // ── Methods ────────────────────────────────────────────────────────

        public bool UnlockBackOffice(string itemId)
        {
            if (!_state.backOfficeLocked) return true;
            if (itemId == "dco_key_ring" || itemId == "lockpick")
            {
                _state.backOfficeLocked = false;
                OnBackOfficeUnlocked?.Invoke(_state);
                return true;
            }
            return false;
        }

        public bool OpenWallSafe(string itemId)
        {
            if (!_state.wallSafeOpened && !_state.backOfficeLocked)
            {
                if (itemId == "dco_key_ring" || itemId == "lockpick" || itemId == "crowbar")
                {
                    _state.wallSafeOpened = true;
                    OnWallSafeOpened?.Invoke(_state);
                    return true;
                }
            }
            return _state.wallSafeOpened;
        }

        public List<string> SearchFilingCabinetDrawer()
        {
            var loot = new List<string>();
            if (_state.filingCabinetDrawersOpened < 4)
            {
                if (_state.keysTaken)
                {
                    _state.filingCabinetDrawersOpened++;
                    _state.civilianRecordsRemaining -= 1050;
                    loot.Add("civilian_record_files");
                    OnFilingCabinetSearched?.Invoke(_state, _state.filingCabinetDrawersOpened);
                }
            }
            return loot;
        }

        public List<string> LootVendingMachine()
        {
            if (_state.vendingMachineLooted) return new List<string>();
            _state.vendingMachineLooted = true;
            var loot = new List<string>(VendingMachineLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> LootCashDrawer()
        {
            if (_state.cashDrawerLooted) return new List<string>();
            _state.cashDrawerLooted = true;
            var loot = new List<string>(CashDrawerLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> LootDeskFlashlight()
        {
            if (_state.flashlightTaken) return new List<string>();
            _state.flashlightTaken = true;
            return new List<string> { "flashlight" };
        }

        public string TakeStamp()
        {
            if (_state.stampTaken) return null;
            _state.stampTaken = true;
            return "stamp_dco_official";
        }

        public string TakeKeys()
        {
            if (_state.keysTaken) return null;
            _state.keysTaken = true;
            return "dco_key_ring";
        }

        public List<string> LootFirstAidKit()
        {
            if (_state.firstAidKitLooted) return new List<string>();
            _state.firstAidKitLooted = true;
            var loot = new List<string>(FirstAidKitLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> LootMaintenanceCloset()
        {
            if (_state.maintenanceClosetLooted) return new List<string>();
            _state.maintenanceClosetLooted = true;
            var loot = new List<string>(MaintenanceClosetLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> TakeCurtains()
        {
            if (_state.curtainsTaken) return new List<string>();
            _state.curtainsTaken = true;
            var loot = new List<string>(CurtainLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        public List<string> LootWallSafe()
        {
            if (!_state.wallSafeOpened) return new List<string>();
            var loot = new List<string>(BackOfficeLoot);
            OnLootCollected?.Invoke(_state, loot);
            return loot;
        }

        /// <summary>Full loot sweep — returns all available loot items from all locations.</summary>
        public List<string> FullLootSweep(System.Random rng)
        {
            var allLoot = new List<string>();
            allLoot.AddRange(LootVendingMachine());
            allLoot.AddRange(LootCashDrawer());
            var stamp = TakeStamp();
            if (stamp != null) allLoot.Add(stamp);
            if (TakeKeys() != null) allLoot.Add("dco_key_ring");
            allLoot.AddRange(LootFirstAidKit());
            allLoot.AddRange(LootMaintenanceCloset());
            allLoot.AddRange(TakeCurtains());
            var flashlight = LootDeskFlashlight();
            if (flashlight.Count > 0) allLoot.Add(flashlight[0]);
            return allLoot;
        }

        // ── Save / Load ────────────────────────────────────────────────
        public DistrictCoordinationOfficeState CaptureState() => _state;
        public void RestoreState(DistrictCoordinationOfficeState saved)
        {
            if (saved != null) _state = saved;
        }
    }
}
